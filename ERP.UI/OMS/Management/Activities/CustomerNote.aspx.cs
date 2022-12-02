using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using DevExpress.Web.Data;
using EntityLayer.CommonELS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using ERP.Models;

namespace ERP.OMS.Management.Activities
{
    public partial class CustomerNote : OMS.ViewState_class.VSPage//System.Web.UI.Page
    {
        SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
        #region Global Veriable
        BusinessLogicLayer.OtherTasks oOtherTasks = new BusinessLogicLayer.OtherTasks();
        BusinessLogicLayer.DailyTaskOther oDailyTaskOther = new BusinessLogicLayer.DailyTaskOther();
        
      //  BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        DebitCreditNoteBL objDebitCreditBL = new DebitCreditNoteBL();
        BusinessLogicLayer.RemarkCategoryBL reCat = new BusinessLogicLayer.RemarkCategoryBL();
        Converter oconverter = new Converter();
        string JVNumStr = string.Empty;
        public static EntityLayer.CommonELS.UserRightsForPage rights;
        public static string CashReportID_Vendor = "0";
        #endregion
        #region Page_Init
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
        #endregion
        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] != null)
            {
                if (!IsPostBack)
                {

                   




                    hfPageLoadFlag.Value = "T";
                   // CustomerComboBox.FilterMinLength = 4;

                    SessionClear();
                    rights = new UserRightsForPage();
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/CustomerNote.aspx");

                    #region Listing Note Date

                    string finyear = "";
                    DateTime MinDate, MaxDate;

                    if (Session["LastFinYear"] != null)
                    {
                        finyear = Convert.ToString(Session["LastFinYear"]).Trim();
                        DataTable dtFinYear = objSlaesActivitiesBL.GetFinacialYearBasedQouteDate(finyear);
                        if (dtFinYear != null && dtFinYear.Rows.Count > 0)
                        {
                            MinDate = Convert.ToDateTime(dtFinYear.Rows[0]["finYearStartDate"]);
                            MaxDate = Convert.ToDateTime(dtFinYear.Rows[0]["finYearEndDate"]);

                            FormDate.Value = null;
                            FormDate.MinDate = MinDate;
                            FormDate.MaxDate = MaxDate;

                            toDate.Value = null;
                            toDate.MinDate = MinDate;
                            toDate.MaxDate = MaxDate;


                            if (DateTime.Now > MaxDate)
                            {
                                FormDate.Value = MaxDate;
                                toDate.Value = MaxDate;
                            }
                            else
                            {
                                FormDate.Value = DateTime.Now;
                                toDate.Value = DateTime.Now;
                            }
                        }
                    }



                    #endregion
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

                    tDate.Value = DateTime.ParseExact(fDate, @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    #endregion
                    SessionClear();
                    //FillSearchGrid();
                    BindBranchFrom();
                    BindCurrency();
                    BindBranchListGrid();
                  

                    string strdefaultBranch = Convert.ToString(Session["userbranchID"]);
                    ddlBranch.SelectedValue = strdefaultBranch;

                    hdnNotelNo.Value = "";

                    Session["exportval"] = null;
                    if (Request.QueryString.AllKeys.Contains("IsTagged"))
                    {
                        divcross.Visible = false;
                        Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>Customernoteledger(" + Request.QueryString["key"] + ");</script>");

                    }

                    IsUdfpresent.Value = Convert.ToString(getUdfCount());
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
        }

        #endregion

        #region Main Grid
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
                    txt_Rate.ClientEnabled = false;
                }
                else
                {
                    ddl_Currency.SelectedIndex = no;
                    txt_Rate.ClientEnabled = true;
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
                //FillSearchGrid();
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
                else if (strIsComplete == -2)
                {
                    GvJvSearch.JSProperties["cpJVDelete"] = "Customer Debit/Credit Note already used in Adjustment of Document - advance with debit note.";
                }
                else if (strIsComplete == -3)
                {
                    GvJvSearch.JSProperties["cpJVDelete"] = "Customer Debit/Credit Note already used in Adjustment of Document - Credit Note Customer against the sale Invoice.";
                }
                else
                {
                    GvJvSearch.JSProperties["cpJVDelete"] = "Problem in Deleting. Sry for Inconvenience";
                }
            }
            else if (PCBCommandName == "FilterGridByDate")
            {
                string FromDate = Convert.ToString(e.Parameters.Split('~')[1]);
                string ToDate = Convert.ToString(e.Parameters.Split('~')[2]);
                string BranchID = Convert.ToString(e.Parameters.Split('~')[3]);

                string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                string finyear = Convert.ToString(Session["LastFinYear"]);
                Session["CustomerDrCrNoteListingDetails"] = VendorDrCrNoteList(userbranch, lastCompany, finyear, BranchID, FromDate, ToDate, "True");
                GvJvSearch.DataBind();
            }
        }
        public DataTable VendorDrCrNoteList(string userbranchlist, string lastCompany, string Fiyear, string userbranchID, string FromDate, string ToDate, string Filter)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("proc_DebitCreditNoteDetails");
            proc.AddVarcharPara("@Action", 500, "GetCustomerSearchGrid_ByDate");
            proc.AddVarcharPara("@FinYear", 500, Fiyear);
            proc.AddVarcharPara("@CompanyID", 500, lastCompany);
            proc.AddVarcharPara("@userbranchlist", 4000, userbranchlist);
            proc.AddVarcharPara("@BranchID", 3000, userbranchID);
            proc.AddVarcharPara("@FromDate", 10, FromDate);
            proc.AddVarcharPara("@ToDate", 10, ToDate);
            proc.AddVarcharPara("@filter", 100, Filter);
            dt = proc.GetTable();
            return dt;
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
            if (!rights.CanView)
            {
                if (e.ButtonID == "CustomBtnView")
                {
                    e.Visible = DevExpress.Utils.DefaultBoolean.False;
                }
            }
        }
        //protected void GvJvSearch_DataBinding(object sender, EventArgs e)
        //{
        //    //string FinYear = Convert.ToString(Session["LastFinYear"]).Trim();
        //    //string CompanyID = Convert.ToString(Session["LastCompany"]).Trim();
        //    //string branchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);

        //    //DataTable dtdata = objDebitCreditBL.GetSearchGridData(branchHierarchy, CompanyID, FinYear, "GetCustomerSearchGrid");
        //    //if (dtdata != null && dtdata.Rows.Count > 0)
        //    //{
        //    //    GvJvSearch.DataSource = dtdata;
        //    //}
        //    //else
        //    //{
        //    //    GvJvSearch.DataSource = null;
        //    //}

        //    if (Session["CustomerDrCrNoteListingDetails"] != null)
        //    {
        //        DataTable dsdata = (DataTable)Session["CustomerDrCrNoteListingDetails"];
        //        GvJvSearch.DataSource = dsdata;
        //    }
        //}
        //void FillSearchGrid()
        //{
        //    string FinYear = Convert.ToString(Session["LastFinYear"]).Trim();
        //    string CompanyID = Convert.ToString(Session["LastCompany"]).Trim();
        //    string branchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);

        //    DataTable dtdata = objDebitCreditBL.GetSearchGridData(branchHierarchy, CompanyID, FinYear, "GetCustomerSearchGrid");
        //    if (dtdata != null && dtdata.Rows.Count > 0)
        //    {
        //        GvJvSearch.DataSource = dtdata;
        //        GvJvSearch.DataBind();
        //    }
        //    else
        //    {
        //        GvJvSearch.DataSource = null;
        //        GvJvSearch.DataBind();
        //    }
        //}
        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
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
            public string Parent_LedgerID { get; set; }
            public string Parent_Sl { get; set; }
            public string gvColMainAccount { get; set; }
            public string gvColSubAccount { get; set; }
            public string IsSubledger { get; set; }
        }

        #endregion
        #region Get Mainaccount, Subaccount & Journal Details
        public IEnumerable GetAllMainAccount()
        {
            List<MainAccount> MainAccountList = new List<MainAccount>();
            
            
          //  BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
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

            //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
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
        public DataTable GetLedgerHSNSACMapping(string strMainAccount)
        {
            DataTable taxDetail = new DataTable();

            ProcedureExecute proc = new ProcedureExecute("prc_Tax_Ledger");
            proc.AddVarcharPara("@ApplicableFor", 50, "S");
            proc.AddIntegerPara("@mainAccount_id", string.IsNullOrEmpty(strMainAccount) ? 0 : Convert.ToInt32(strMainAccount));
            proc.AddVarcharPara("@action", 150, "GetTaxDetails");
            proc.AddVarcharPara("@S_quoteDate", 10, tDate.Date.ToString("yyyy-MM-dd"));
            taxDetail = proc.GetTable();
            return taxDetail;
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
                Session["SESS_CUSTOMERNOTE_TAX"] = (DataTable)GetNoteDetails("NoteTaxDetails", VoucherNumber);
                DataTable DTTaxTable = (DataTable)Session["SESS_CUSTOMERNOTE_TAX"];
                int dtCount = 1;
                foreach (DataRow DR in Voucherdt.Rows)
                {
                    DataRow[] HRow = Voucherdt.Select("Parent_Sl =" + Convert.ToString(DR["CashReportID"]));
                    foreach (var lst in HRow.ToList())
                    {
                        lst["Parent_Sl"] = Convert.ToString(dtCount).Trim();
                        lst["Parent_LedgerID"] = Convert.ToString(DR["MainAccount"]).Trim();
                    }
                    if (DTTaxTable != null && DTTaxTable.Rows.Count > 0)
                    {
                        DataRow[] TRow = DTTaxTable.Select("TaxParent_Sl =" + Convert.ToString(DR["CashReportID"]));
                        foreach (var lst in TRow.ToList())
                        {
                            lst["TaxParent_Sl"] = Convert.ToString(dtCount).Trim();
                        }
                    }
                    DR["CashReportID"] = Convert.ToString(dtCount).Trim();
                    dtCount++;
                }
                //Voucherdt.Columns["CashReportID"].DataType = typeof(String);
                Session["SESS_CustomerNoteLedgerDT"] = Voucherdt;
                for (int i = 0; i < Voucherdt.Rows.Count; i++)
                {
                    VOUCHERLIST Vouchers = new VOUCHERLIST();
                    Vouchers.CashReportID = Convert.ToString(Voucherdt.Rows[i][0]);
                    //Vouchers.CountryID = Convert.ToString(Voucherdt.Rows[i][1]);
                    //Vouchers.CityID = Convert.ToString(Voucherdt.Rows[i][2]);
                    Vouchers.CountryID = Convert.ToString(Voucherdt.Rows[i]["MainAccountID"]).Trim();
                    Vouchers.CityID = Convert.ToString(Voucherdt.Rows[i]["SubAccountID"]).Trim();     
                
                    //Vouchers.CountryID = Convert.ToString(Voucherdt.Rows[i][1]).Trim();
                    //Vouchers.CityID = Convert.ToString(Voucherdt.Rows[i][2]).Trim();
                    Vouchers.WithDrawl = Convert.ToString(Voucherdt.Rows[i][3]);
                    Vouchers.Receipt = Convert.ToString(Voucherdt.Rows[i][4]);
                    Vouchers.Narration = Convert.ToString(Voucherdt.Rows[i][5]);                

                    try
                    {
                        if (DTTaxTable != null && DTTaxTable.Rows.Count > 0)
                        {
                            Vouchers.Parent_LedgerID = Convert.ToString(Voucherdt.Rows[i][7]);
                            Vouchers.Parent_Sl = Convert.ToString(Voucherdt.Rows[i][8]);
                        }
                        else
                        {
                            Vouchers.Parent_LedgerID = "0";
                            Vouchers.Parent_Sl = "0";
                        }
                    }
                    catch (Exception ex) { }
                    //Vouchers.gvColMainAccount = Convert.ToString(Voucherdt.Rows[i]["MainAccount"]).Trim();
                    //Vouchers.gvColSubAccount = Convert.ToString(Voucherdt.Rows[i]["SubAccount"]).Trim();
                    Vouchers.gvColMainAccount = Convert.ToString(Voucherdt.Rows[i][1]);
                    Vouchers.gvColSubAccount = Convert.ToString(Voucherdt.Rows[i][2]);
                    Vouchers.IsSubledger = Convert.ToString(Voucherdt.Rows[i]["IsSubledger"]).Trim();     
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
        public int HSNSACMappingFlag(string strMainAccount)
        {
            DataTable DT = GetLedgerHSNSACMapping(strMainAccount);

            int returnHSNSACMapFlag = 0;

            if (DT != null)
            {
                if (DT.Rows.Count > 0)
                {
                    returnHSNSACMapFlag = string.IsNullOrEmpty(Convert.ToString(DT.Rows[0][0]).Trim()) ? 0 : 1;
                }
            }
            return returnHSNSACMapFlag;
        }
        #endregion

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
            string strschematype = "", strschemalength = "", strschemavalue = "", strbranchID = "";

           // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();


            DataTable DT = objEngine.GetDataTable("tbl_master_Idschema", " schema_type,length,IsNull(Branch,0) as Branch", " Id = " + Convert.ToInt32(sel_scheme_id));

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                strschematype = Convert.ToString(DT.Rows[i]["schema_type"]);
                strschemalength = Convert.ToString(DT.Rows[i]["length"]);
                strbranchID = Convert.ToString(DT.Rows[i]["Branch"]);

                strschemavalue = strschematype + "~" + strschemalength + "~" + strbranchID;
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
            // GvJvSearch.Columns[11].Visible = false;

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
                        sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                    else if (scheme_type == 2)
                        sqlQuery += "^";
                    sqlQuery += "[0-9]{" + paddCounter + "}";
                    if (sufxLen > 0)
                        sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                    //sqlQuery += "?$', LTRIM(RTRIM(tjv.DCNote_DocumentNumber))) = 1";
                    sqlQuery += "?$', LTRIM(RTRIM(tjv.DCNote_DocumentNumber))) = 1 and DCNote_DocumentNumber like '" + prefCompCode + "%'";
                    if (scheme_type == 2)
                        sqlQuery += " AND CONVERT(DATE, JournalVoucher_CreateDateTime) = CONVERT(DATE, GETDATE())";
                    dtC = oDBEngine.GetDataTable(sqlQuery);

                    if (dtC.Rows[0][0].ToString() == "")
                    {
                        sqlQuery = "SELECT max(tjv.DCNote_DocumentNumber) FROM Trans_CustDebitCreditNote tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        // sqlQuery += "?$', LTRIM(RTRIM(tjv.DCNote_DocumentNumber))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.DCNote_DocumentNumber))) = 1 and DCNote_DocumentNumber like '" + prefCompCode + "%'";
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
            string userbranchHierarchy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
            string LastFinYear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

            string query = "Select ID,SchemaName +(Case When (SELECT Isnull(branch_description,'') FROM tbl_master_branch where branch_id=Branch) Is Null Then '' Else ' ('+(SELECT Isnull(branch_description,'') FROM tbl_master_branch where branch_id=Branch)+')' End) as SchemaName From tbl_master_Idschema  Where TYPE_ID=(Case When '" + sel_type_id + "'='Dr' Then '25' Else '26' End) AND IsActive=1 AND Isnull(Branch,'') in (select s FROM dbo.GetSplit(',','" + userbranchHierarchy + "')) AND Isnull(comapanyInt,'')='" + LastCompany + "' AND financial_year_id=(Select Top 1 FinYear_ID FROM Master_FinYear WHERE FinYear_Code='" + LastFinYear + "')";


            //string constr = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
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
        protected void ddlInvoice_Callback(object sender, CallbackEventArgsBase e)
        {
            string NoteType = e.Parameter.Split('~')[0];
            string CustomerID = e.Parameter.Split('~')[1];
            string Action = "";

            //if (NoteType == "Dr") Action = "CustomerDebit";
            //else if (NoteType == "Cr") Action = "CustomerCredit";

            Action = "CustomerDebit";
            BinDInvoiceDetails(Action, CustomerID);

            PurchaseOrderBL objPurchaseOrderBL = new PurchaseOrderBL();
            DataTable GSTNTable = objPurchaseOrderBL.GetVendorGSTIN(CustomerID);
            if (GSTNTable != null && GSTNTable.Rows.Count > 0)
            {
                string strGSTN = Convert.ToString(GSTNTable.Rows[0]["CNT_GSTIN"]).Trim();
                if (strGSTN != "")
                {
                    ddlInvoice.JSProperties["cpGSTN"] = "Yes";
                }
                else
                {
                    ddlInvoice.JSProperties["cpGSTN"] = "No";
                }
            }
        }
        public void BinDInvoiceDetails(string Action, string CustomerID)
        {
            string FinYear = Convert.ToString(Session["LastFinYear"]).Trim();
            string BranchList = Convert.ToString(ddlBranch.SelectedValue); //Convert.ToString(Session["userbranchHierarchy"]);

            DataTable dt = objDebitCreditBL.GetInvoiceDetails(Action, CustomerID, FinYear, BranchList);
            if (dt != null && dt.Rows.Count > 0)
            {
                ddlInvoice.TextField = "InvoiceNumber";
                ddlInvoice.ValueField = "InvoiceID";
                ddlInvoice.DataSource = dt;
                ddlInvoice.DataBind();
                ddlInvoice.SelectedIndex = 0;
            }
        }
        private void BindBranchListGrid()
        {
            DataSet dst = new DataSet();
            string userbranch = Convert.ToString(Session["userbranchHierarchy"]);

            dst = GetAllDropDownBranchForVendorDrCrNote(userbranch);
            cmbBranchfilter.ValueField = "branch_id";
            cmbBranchfilter.TextField = "branch_description";
            cmbBranchfilter.DataSource = dst.Tables[0];
            cmbBranchfilter.DataBind();

            cmbBranchfilter.Value = Convert.ToString(Session["userbranchID"]);
        }
        public DataSet GetAllDropDownBranchForVendorDrCrNote(string @userbranch)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("proc_DebitCreditNoteDetails");
            proc.AddVarcharPara("@Action", 100, "GetAllDropDownBranchForVendorDrCrNote");
            proc.AddVarcharPara("@userbranchlist", 4000, @userbranch);
            ds = proc.GetDataSet();
            return ds;
        }

        #endregion

        #region Tax Section
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
        protected void taxUpdatePanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string performpara = e.Parameter;
            if (performpara.Split('~')[0] == "DelProdbySl")
            {
                DataTable MainTaxDataTable = (DataTable)Session["LEDGER_CUSTOMERNOTE_FinalTaxRecord"];

                DataRow[] deletedRow = MainTaxDataTable.Select("SlNo=" + performpara.Split('~')[1]);
                if (deletedRow.Length > 0)
                {
                    foreach (DataRow dr in deletedRow)
                    {
                        MainTaxDataTable.Rows.Remove(dr);
                    }

                }

                Session["LEDGER_CUSTOMERNOTE_FinalTaxRecord"] = MainTaxDataTable;
                //GetStock(Convert.ToString(performpara.Split('~')[2]));
                //DeleteWarehouse(Convert.ToString(performpara.Split('~')[1]));
                DataTable taxDetails = (DataTable)Session["SI_TaxDetails"];
                if (taxDetails != null)
                {
                    foreach (DataRow dr in taxDetails.Rows)
                    {
                        dr["Amount"] = "0.00";
                    }
                    Session["SI_TaxDetails"] = taxDetails;
                }
            }
            else if (performpara.Split('~')[0] == "DeleteAllTax")
            {
                CreateDataTaxTable();

                DataTable taxDetails = (DataTable)Session["SI_TaxDetails"];

                if (taxDetails != null)
                {
                    foreach (DataRow dr in taxDetails.Rows)
                    {
                        dr["Amount"] = "0.00";
                    }
                    Session["SI_TaxDetails"] = taxDetails;
                }
            }
            else
            {
                DataTable MainTaxDataTable = (DataTable)Session["LEDGER_CUSTOMERNOTE_FinalTaxRecord"];

                DataRow[] deletedRow = MainTaxDataTable.Select("SlNo=" + performpara.Split('~')[1]);
                if (deletedRow.Length > 0)
                {
                    foreach (DataRow dr in deletedRow)
                    {
                        MainTaxDataTable.Rows.Remove(dr);
                    }

                }

                Session["LEDGER_CUSTOMERNOTE_FinalTaxRecord"] = MainTaxDataTable;
                DataTable taxDetails = (DataTable)Session["SI_TaxDetails"];
                if (taxDetails != null)
                {
                    foreach (DataRow dr in taxDetails.Rows)
                    {
                        dr["Amount"] = "0.00";
                    }
                    Session["SI_TaxDetails"] = taxDetails;
                }
            }
        }
        public double ReCalculateTaxAmount(string slno, double amount)
        {
            DataTable MainTaxDataTable = (DataTable)Session["LEDGER_CUSTOMERNOTE_FinalTaxRecord"];
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
            Session["LEDGER_CUSTOMERNOTE_FinalTaxRecord"] = MainTaxDataTable;

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
            TaxRecord.Columns.Add("TaxParent_Sl", typeof(System.String));
            Session["LEDGER_CUSTOMERNOTE_FinalTaxRecord"] = TaxRecord;
        }
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
            proc.AddVarcharPara("@QuotationID", 500, Convert.ToString(Session["SI_InvoiceID"]));
            ds = proc.GetDataSet();
            return ds;
        }
        public DataSet GetQuotationEditedTaxData()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "ProductEditedTaxDetails");
            proc.AddVarcharPara("@InvoiceID", 500, Convert.ToString(Session["SI_InvoiceID"]));
            ds = proc.GetDataSet();
            return ds;
        }
        public DataTable GetComponentEditedTaxData(string ComponentDetailsIDs, string strType)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
            proc.AddVarcharPara("@Action", 500, "ComponentProductTax");
            proc.AddVarcharPara("@SelectedComponentList", 500, ComponentDetailsIDs);
            proc.AddVarcharPara("@ComponentType", 500, strType);
            ds = proc.GetTable();
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
            int mainAccount_id = Convert.ToInt32(HDmainAccount_id.Value);
            /// ----------- When Amount wants to change and tax will be re-calculated
            CashReportID_Vendor = (string.IsNullOrEmpty(hfCashReportID.Value) ? CashReportID_Vendor : hfCashReportID.Value);
            hfCashReportID.Value = string.Empty;
            /// ------ End ----- When Amount wants to change and tax will be re-calculated ---- End -----
           
            if (e.Parameters.Split('~')[0] == "SaveGST")
            {
                DataTable TaxRecord = (DataTable)Session["LEDGER_CUSTOMER_FinalTaxRecord"];

                //For GST/CST/VAT
                if (cmbGstCstVat.Value != null)
                {                    
                    DataRow newRowGST = TaxRecord.NewRow();                   
                    newRowGST["slNo"] = mainAccount_id;
                    newRowGST["Percentage"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[1];
                    newRowGST["TaxCode"] = "0";
                    newRowGST["AltTaxCode"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[0];
                    newRowGST["Amount"] = txtGstCstVat.Text;
                    TaxRecord.Rows.Add(newRowGST);
                   
                }
                //End Here

                aspxGridTax.JSProperties["cpUpdated"] = "";
                Session["LEDGER_CUSTOMER_FinalTaxRecord"] = TaxRecord;
            }
            else
            {
                #region fetch All data For Tax

                DataTable taxDetail = new DataTable();               
                DataTable MainTaxDataTable = (DataTable)Session["SESS_CUSTOMERNOTE_TAX"];
                DataTable databaseReturnTable = (DataTable)Session["SI_QuotationTaxDetails"];

                ProcedureExecute proc = new ProcedureExecute("prc_Tax_Ledger");
                proc.AddVarcharPara("@ApplicableFor", 50, "S");
                proc.AddIntegerPara("@mainAccount_id", mainAccount_id);
                proc.AddVarcharPara("@action", 150, "GetTaxDetails");
                proc.AddVarcharPara("@S_quoteDate", 10, tDate.Date.ToString("yyyy-MM-dd"));
                taxDetail = proc.GetTable();

                //Get Company Gstin 09032017
                string CompInternalId = Convert.ToString(Session["LastCompany"]);
                string[] compGstin = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_gstin", "cmp_internalid='" + CompInternalId + "'", 1);

                //Get BranchStateCode
                string BrancgStateCode = "", BranchGSTIN = "";
                DataTable BranchTable = oDBEngine.GetDataTable("select StateCode,branch_GSTIN   from tbl_master_branch branch inner join tbl_master_state st on branch.branch_state=st.id where branch_id=" + Convert.ToString(ddlBranch.SelectedValue));
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
                string ShippingState = "";               

                string sstateCode = BillingShippingControl.GetShippingStateCode(Keyval_internalId.Value.ToUpper());
                ShippingState = sstateCode;
                if (ShippingState.Trim() != "")
                {
                    ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                }
                if (ShippingState.Trim() != "" && BrancgStateCode.Trim() != "")
                {
                    if (compGstin.Length > 0)
                    {
                        if (BrancgStateCode.Substring(0, 2) == ShippingState)
                        {
                            //Check if the state is in union territories then only UTGST will apply
                            //   Chandigarh     Andaman and Nicobar Islands    DADRA & NAGAR HAVELI    DAMAN & DIU             Lakshadweep              PONDICHERRY
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
                }
                //If Company GSTIN is blank then Delete All CGST,UGST,IGST,CGST
                if (BrancgStateCode.Trim() == "")
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
                decimal ProdGrossAmt = Convert.ToDecimal(HdProdGrossAmt.Value);
                decimal ProdNetAmt = Convert.ToDecimal(HdProdNetAmt.Value);
                List<TaxDetails> TaxDetailsDetails = new List<TaxDetails>();
                if (e.Parameters.Split('~')[0] == "New")
                {
                    //if (!string.IsNullOrEmpty(Keyval_internalId.Value.Replace("CustomerNote", "")))
                    if (Keyval_internalId.Value.Replace("CustomerNote", "").ToUpper() != "ADD")
                    {
                        
                    }
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

                        if (Convert.ToString(dr["TaxCalculateMethods"]) == "A")
                        {
                            obj.Taxes_Name = Convert.ToString(dr["Taxes_Name"]) + "(+)";
                        }
                        else
                        {
                            obj.Taxes_Name = Convert.ToString(dr["Taxes_Name"]) + "(-)";
                        }
                        obj.Amount = Math.Round(Convert.ToDouble(obj.calCulatedOn * (Convert.ToDecimal(obj.TaxField) / 100)), 2);                        
                        if (MainTaxDataTable != null)
                        {
                            DataRow[] filtr = MainTaxDataTable.Select("TaxCode ='" + obj.Taxes_ID + "' and TaxParent_Sl=" + Convert.ToString(CashReportID_Vendor));
                            if (filtr.Length > 0)
                            {
                                obj.Amount = Convert.ToDouble(filtr[0]["Amount"]);
                                if (obj.Taxes_ID == 0)
                                {                                  
                                    aspxGridTax.JSProperties["cpComboCode"] = Convert.ToString(filtr[0]["AltTaxCode"]);
                                }
                                else
                                    obj.TaxField = Convert.ToString(filtr[0]["Percentage"]);
                            }
                        }

                        TaxDetailsDetails.Add(obj);
                    }
                }
                else
                {
                    string keyValue = e.Parameters.Split('~')[0];
                    DataTable TaxRecord = (DataTable)Session["SESS_CUSTOMERNOTE_TAX"];
                    foreach (DataRow dr in taxDetail.Rows)
                    {
                        TaxDetails obj = new TaxDetails();
                        obj.Taxes_ID = Convert.ToInt32(dr["Taxes_ID"]);
                        obj.taxCodeName = Convert.ToString(dr["taxCodeName"]);
                        if (Convert.ToString(dr["TaxCalculateMethods"]) == "A")
                            obj.Taxes_Name = Convert.ToString(dr["Taxes_Name"]) + "(+)";
                        else
                            obj.Taxes_Name = Convert.ToString(dr["Taxes_Name"]) + "(-)";

                        if (string.IsNullOrEmpty(Convert.ToString(dr["TaxRates_Rate"])))
                        {
                            obj.TaxField = "";
                        }
                        else
                        {
                            obj.TaxField = Convert.ToString(dr["TaxRates_Rate"]);
                        }
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
                        obj.Amount = Math.Round(Convert.ToDouble(obj.calCulatedOn * (Convert.ToDecimal(obj.TaxField) / 100)), 2);
                        #endregion

                        if (MainTaxDataTable != null)
                        {
                            if (MainTaxDataTable.Rows.Count > 0)
                            {
                                DataRow[] filtr = MainTaxDataTable.Select("TaxCode ='" + obj.Taxes_ID + "' and TaxParent_Sl=" + Convert.ToString(CashReportID_Vendor));
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
                                else //// this code will execute when amount recalculate on amount change
                                {
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

                                    if (Convert.ToString(dr["TaxCalculateMethods"]) == "A")
                                    {
                                        obj.Taxes_Name = Convert.ToString(dr["Taxes_Name"]) + "(+)";

                                    }
                                    else
                                    {
                                        obj.Taxes_Name = Convert.ToString(dr["Taxes_Name"]) + "(-)";
                                    }

                                    obj.Amount = Math.Round(Convert.ToDouble(obj.calCulatedOn * (Convert.ToDecimal(obj.TaxField) / 100)), 2);
                                }
                            }
                            else //// this code will execute when amount recalculate on amount change
                            {
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

                                if (Convert.ToString(dr["TaxCalculateMethods"]) == "A")
                                {
                                    obj.Taxes_Name = Convert.ToString(dr["Taxes_Name"]) + "(+)";

                                }
                                else
                                {
                                    obj.Taxes_Name = Convert.ToString(dr["Taxes_Name"]) + "(-)";
                                }

                                obj.Amount = Math.Round(Convert.ToDouble(obj.calCulatedOn * (Convert.ToDecimal(obj.TaxField) / 100)), 2);
                            }
                        }
                        TaxDetailsDetails.Add(obj);
                    }
                    Session["LEDGER_CUSTOMER_FinalTaxRecord"] = TaxRecord;
                }
                aspxGridTax.JSProperties["cpJsonData"] = createJsonForDetails(taxDetail);

                retMsg = Convert.ToString(GetTotalTaxAmount(TaxDetailsDetails));
                aspxGridTax.JSProperties["cpUpdated"] = "ok~" + retMsg;

                TaxDetailsDetails = setCalculatedOn(TaxDetailsDetails, taxDetail);

                //proc = new ProcedureExecute("proc_DebitCreditNoteDetails");
                //proc.AddVarcharPara("@Action", 100, "VendorNoteTaxDetails");
                //proc.AddIntegerPara("@NoteID", string.IsNullOrEmpty(hdnNotelNo.Value) ? 0 : Convert.ToInt32(hdnNotelNo.Value));
                //taxDetail = proc.GetTable();



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
          
            CreateDataTaxTable();           
            int slNo = Convert.ToInt32(HDmainAccount_id.Value);

            DataTable TaxRecord = (DataTable)Session["LEDGER_CUSTOMERNOTE_FinalTaxRecord"];
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
                    newRow["slNo"] = CashReportID_Vendor; //slNo;
                    newRow["Percentage"] = Percentage;
                    newRow["TaxCode"] = TaxCode;
                    newRow["AltTaxCode"] = "0";
                    newRow["Amount"] = Amount;
                    newRow["TaxParent_Sl"] = CashReportID_Vendor;
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
                    newRowGST["slNo"] = CashReportID_Vendor; //slNo;
                    newRowGST["Percentage"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[1];
                    newRowGST["TaxCode"] = "0";
                    newRowGST["AltTaxCode"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[0];
                    newRowGST["Amount"] = txtGstCstVat.Text;
                    newRowGST["TaxParent_Sl"] = CashReportID_Vendor;
                    TaxRecord.Rows.Add(newRowGST);
                }
            }
            //End Here
            Session["LEDGER_CUSTOMERNOTE_FinalTaxRecord"] = TaxRecord;

            #region # Main Tax Session ###
            DataTable dtMainTax = (DataTable)Session["SESS_CUSTOMERNOTE_TAX"];
            if (dtMainTax != null)
            {
                try
                {
                    var datadtMainTax = dtMainTax.AsEnumerable().Where(r => r.Field<string>("TaxParent_Sl").Trim().Equals(CashReportID_Vendor));
                    if (datadtMainTax != null && datadtMainTax.Count() > 0)
                    {
                        foreach (var row in datadtMainTax.ToList())
                            row.Delete();
                    }
                }
                catch (Exception ex) { }
            }
            else
            {
                DataTable TaxMainRecord = new DataTable();

                TaxMainRecord.Columns.Add("SlNo", typeof(System.Int32));
                TaxMainRecord.Columns.Add("TaxCode", typeof(System.String));
                TaxMainRecord.Columns.Add("AltTaxCode", typeof(System.String));
                TaxMainRecord.Columns.Add("Percentage", typeof(System.Decimal));
                TaxMainRecord.Columns.Add("Amount", typeof(System.Decimal));
                TaxMainRecord.Columns.Add("TaxParent_Sl", typeof(System.String));
                Session["SESS_CUSTOMERNOTE_TAX"] = TaxMainRecord;
            }

            int taxMainCount = 1;
            foreach (DataRow DR in TaxRecord.Rows)
            {
                DataRow newRow = ((DataTable)Session["SESS_CUSTOMERNOTE_TAX"]).NewRow();
                newRow["SlNo"] = taxMainCount++;
                newRow["TaxCode"] = Convert.ToString(DR["TaxCode"]);
                newRow["AltTaxCode"] = "0";
                newRow["Percentage"] = Convert.ToDecimal(DR["Percentage"]);
                newRow["Amount"] = Convert.ToString(DR["Amount"]);
                newRow["TaxParent_Sl"] = CashReportID_Vendor;

                ((DataTable)Session["SESS_CUSTOMERNOTE_TAX"]).Rows.Add(newRow);
            }
            #endregion

            int count = 1;
            foreach (DataRow DR in TaxRecord.Rows)
            {
                DR["slNo"] = Convert.ToString(count++).Trim();
            }
            count = 1;
            string Parent_LedgerID = string.Empty;

            DataTable LedgerDT = (DataTable)Session["SESS_CustomerNoteLedgerDT"];
            if (Keyval_internalId.Value.Replace("VendorNote", "").ToUpper() != "ADD")
            {
                Parent_LedgerID = Convert.ToString(slNo);
            }
            else
            {
                Parent_LedgerID = LedgerDT.Rows[LedgerDT.Rows.Count - 1]["MainAccount"].ToString();
            }
           
            if (Session["SESS_CustomerNoteLedgerDT"] != null && ((DataTable)Session["SESS_CustomerNoteLedgerDT"]).Rows.Count > 0)
            {
                ((DataTable)Session["SESS_CUSTOMERNOTE_TAX"]).AcceptChanges();
                var data = LedgerDT.AsEnumerable().Where(r => r.Field<string>("Parent_Sl").Trim().Equals(CashReportID_Vendor));
                if (data != null && data.Count() > 0)
                {
                    int dataCount = 1;


                    foreach (var row in data.ToList())
                    {
                        if (TaxRecord.Rows.Count >= dataCount)
                        {
                            row["SubAccount"] = null;
                            row["WithDrawl"] = Convert.ToString(TaxRecord.Rows[dataCount - 1]["Amount"]);
                            row["TaxCode"] = Convert.ToString(TaxRecord.Rows[dataCount - 1]["TaxCode"]);
                            row["Percentage"] = Convert.ToDecimal(TaxRecord.Rows[dataCount - 1]["Percentage"]);
                        }
                        dataCount++;
                    }

                    LedgerDT.AcceptChanges();
                }
                else
                {
                    foreach (DataRow DR in TaxRecord.Rows)
                    {
                        DataRow newRow = ((DataTable)Session["SESS_CustomerNoteLedgerDT"]).NewRow();
                       // DataTable dtTbl = GetLedgerByTaxCode(Convert.ToString(DR["TaxCode"]));
                        //newRow["MainAccount"] = Convert.ToString(dtTbl.Rows[0]["MainAccount_ReferenceID"]);
                        newRow["MainAccount"] = GetLedgerByTaxCode(Convert.ToString(DR["TaxCode"]));
                       
                        newRow["SubAccount"] = null;
                        newRow["WithDrawl"] = Convert.ToString(DR["Amount"]);
                        newRow["Receipt"] = "0";
                        newRow["Status"] = "I";
                        newRow["Parent_LedgerID"] = Parent_LedgerID;
                        newRow["Parent_Sl"] = CashReportID_Vendor;
                        newRow["TaxCode"] = Convert.ToString(DR["TaxCode"]);
                        newRow["Percentage"] = Convert.ToDecimal(DR["Percentage"]);
                        ((DataTable)Session["SESS_CustomerNoteLedgerDT"]).Rows.Add(newRow);
                    }
                }

                DataTable dtLedgerFinal = new DataTable();

                dtLedgerFinal = ((DataTable)Session["SESS_CustomerNoteLedgerDT"]).Copy();

                DataTable Journaldt = new DataTable();
                Journaldt.Columns.Add("CashReportID", typeof(string));
                Journaldt.Columns.Add("MainAccount", typeof(string));
                Journaldt.Columns.Add("SubAccount", typeof(string));
                Journaldt.Columns.Add("WithDrawl", typeof(string));
                Journaldt.Columns.Add("Receipt", typeof(string));
                Journaldt.Columns.Add("Narration", typeof(string));
                Journaldt.Columns.Add("Status", typeof(string));

                Journaldt.Columns.Add("Parent_LedgerID", typeof(string));
                Journaldt.Columns.Add("TaxCode", typeof(System.String));
                Journaldt.Columns.Add("Percentage", typeof(System.Decimal));
                Journaldt.Columns.Add("Parent_Sl", typeof(string));

                Journaldt.Columns.Add("MainAccountID", typeof(string));
                Journaldt.Columns.Add("SubAccountID", typeof(string));
                Journaldt.Columns.Add("IsSubledger", typeof(string));
                int dataLedgerCount = 1;
                foreach (DataRow DRLedger in dtLedgerFinal.Rows)
                {
                    if (Convert.ToString(DRLedger["Parent_Sl"]).Trim() != Convert.ToString(DRLedger["CashReportID"]).Trim())
                    {
                        if (Convert.ToString(DRLedger["Parent_Sl"]).Trim() == "0")
                        {
                            DataRow dr = Journaldt.NewRow();
                            dr["CashReportID"] = dataLedgerCount;
                            dr["MainAccount"] = DRLedger["MainAccount"];
                            dr["SubAccount"] = DRLedger["SubAccount"];
                            

                            dr["WithDrawl"] = DRLedger["WithDrawl"];
                            dr["Receipt"] = DRLedger["Receipt"];
                            dr["Narration"] = DRLedger["Narration"];
                            dr["Status"] = DRLedger["Status"];
                            dr["Parent_LedgerID"] = DRLedger["Parent_LedgerID"];
                            dr["TaxCode"] = DRLedger["TaxCode"];
                            dr["Percentage"] = DRLedger["Percentage"];
                            dr["Parent_Sl"] = DRLedger["Parent_Sl"];
                            DataTable dtTbl = GetLedgerByMainAccount(Convert.ToString(DRLedger["MainAccount"]));
                            dr["MainAccountID"] = Convert.ToString(dtTbl.Rows[0]["MainAccount_AccountCode"]);
                            //dr["MainAccountID"] = DRLedger["MainAccountID"];
                            dr["SubAccountID"] = DRLedger["SubAccountID"];
                            dr["IsSubledger"] = DRLedger["IsSubledger"];
                            Journaldt.Rows.Add(dr);
                            if (Session["SESS_CUSTOMERNOTE_TAX"] != null && ((DataTable)Session["SESS_CUSTOMERNOTE_TAX"]).Rows.Count > 0)
                            {
                                var query1 = ((DataTable)Session["SESS_CUSTOMERNOTE_TAX"]).AsEnumerable().Where(r => r.Field<string>("TaxParent_Sl") == Convert.ToString(DRLedger["CashReportID"]).Trim()); // Child Ledger Delete 
                                if (query1 != null && query1.Count() > 0)
                                {
                                    foreach (var row in query1.ToList())
                                        row["TaxParent_Sl"] = dataLedgerCount.ToString();
                                }
                            }


                            int dataSubLedgerCount = 1;
                            foreach (DataRow DRSubLedger in dtLedgerFinal.Rows)
                            {
                                if (Convert.ToString(DRSubLedger["Parent_Sl"]).Trim() == Convert.ToString(DRLedger["CashReportID"]).Trim())
                                {
                                    DataRow drSub = Journaldt.NewRow();
                                    drSub["CashReportID"] = dataLedgerCount + dataSubLedgerCount;
                                    drSub["MainAccount"] = DRSubLedger["MainAccount"];
                                    drSub["SubAccount"] = DRSubLedger["SubAccount"];
                                  

                                    drSub["WithDrawl"] = DRSubLedger["WithDrawl"];
                                    drSub["Receipt"] = DRSubLedger["Receipt"];
                                    drSub["Narration"] = DRSubLedger["Narration"];
                                    drSub["Status"] = DRSubLedger["Status"];
                                    drSub["Parent_LedgerID"] = DRSubLedger["Parent_LedgerID"];
                                    drSub["TaxCode"] = DRSubLedger["TaxCode"];
                                    drSub["Percentage"] = DRSubLedger["Percentage"];
                                    drSub["Parent_Sl"] = dataLedgerCount;
                                    //drSub["MainAccountID"] = DRSubLedger["MainAccountID"];
                                    DataTable dtSubTbl = GetLedgerByMainAccount(Convert.ToString(DRSubLedger["MainAccount"]));
                                    drSub["MainAccountID"] = Convert.ToString(dtSubTbl.Rows[0]["MainAccount_AccountCode"]);
                                    drSub["SubAccountID"] = DRSubLedger["SubAccountID"];
                                    drSub["IsSubledger"] = DRSubLedger["IsSubledger"];
                                    Journaldt.Rows.Add(drSub);
                                    dataSubLedgerCount++;
                                }
                            }
                            dataLedgerCount = dataLedgerCount + dataSubLedgerCount;
                        }
                    }
                }

                dtLedgerFinal.Dispose();

                Session["SESS_CustomerNoteLedgerDT"] = Journaldt;

                foreach (DataRow DR in ((DataTable)Session["SESS_CustomerNoteLedgerDT"]).Rows)
                {
                    DR["CashReportID"] = Convert.ToString(count++).Trim();
                }

                grid.DataSource = GetVoucher((DataTable)Session["SESS_CustomerNoteLedgerDT"]);
                grid.DataBind();
            }
        }
        public DataTable GetLedgerByMainAccount(string MainAccount)
        {
            ProcedureExecute proc = new ProcedureExecute("proc_DebitCreditNoteDetails");
            proc.AddVarcharPara("@action", 150, "GetMainAccountName");
            proc.AddIntegerPara("@MainAccountID",Convert.ToInt32(MainAccount));
            DataTable DT = proc.GetTable();
            return DT;
        }
        public DataTable GetLedgerBySubAccount(string SubAccount)
        {
            ProcedureExecute proc = new ProcedureExecute("proc_DebitCreditNoteDetails");
            proc.AddVarcharPara("@action", 150, "GetSubAccountName");
            proc.AddVarcharPara("@SubAccountID", 100, SubAccount);
            DataTable DT = proc.GetTable();
            return DT;
        }
        public string GetLedgerByTaxCode(string TaxCode)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_Tax_Ledger");
            proc.AddVarcharPara("@action", 150, "GetMainAccount");
            proc.AddVarcharPara("@TaxCode", 100, TaxCode);
            DataTable DT = proc.GetTable();

            if (DT.Rows.Count > 0)
            {
                return Convert.ToString(DT.Rows[0][0]);
            }
            else
            {
                return string.Empty;
            }
        }
        public IEnumerable GetVoucher(DataTable voucherDT)
        {
            List<VOUCHERLIST> VoucherList = new List<VOUCHERLIST>();
            if (voucherDT != null)
            {
                if (voucherDT.Rows.Count > 0 && voucherDT != null)
                {
                    for (int i = 0; i < voucherDT.Rows.Count; i++)
                    {
                        VOUCHERLIST Vouchers = new VOUCHERLIST();                      

                        Vouchers.CashReportID = Convert.ToString(voucherDT.Rows[i][0]);                        
                        //Vouchers.CountryID = Convert.ToString(voucherDT.Rows[i][1]).Trim();
                        //Vouchers.CityID = Convert.ToString(voucherDT.Rows[i][2]).Trim();
                        Vouchers.CountryID = Convert.ToString(voucherDT.Rows[i]["MainAccountID"]).Trim();
                        Vouchers.CityID = Convert.ToString(voucherDT.Rows[i]["SubAccountID"]).Trim();                          

                        Vouchers.WithDrawl = Convert.ToString(voucherDT.Rows[i][3]);
                        Vouchers.Receipt = Convert.ToString(voucherDT.Rows[i][4]);
                        Vouchers.Narration = Convert.ToString(voucherDT.Rows[i][5]);
                        try
                        {
                            Vouchers.Parent_LedgerID = Convert.ToString(voucherDT.Rows[i][7]);
                            Vouchers.Parent_Sl = Convert.ToString(voucherDT.Rows[i]["Parent_Sl"]);
                        }
                        catch (Exception ex) { }                      
                        Vouchers.gvColMainAccount = Convert.ToString(voucherDT.Rows[i][1]);
                        Vouchers.gvColSubAccount = Convert.ToString(voucherDT.Rows[i][2]);
                        VoucherList.Add(Vouchers);
                    }
                }
            }
            return VoucherList;
        }
        protected void cmbGstCstVat_Callback(object sender, CallbackEventArgsBase e)
        {
            //DateTime quoteDate = Convert.ToDateTime(dt_PLQuote.Date.ToString("yyyy-MM-dd"));

            //PopulateGSTCSTVATCombo(quoteDate.ToString("yyyy-MM-dd"));
            //CreateDataTaxTable();
            //DataTable taxTableItemLvl = (DataTable)Session["LEDGER_CUSTOMERNOTE_FinalTaxRecord"];
            //foreach (DataRow dr in taxTableItemLvl.Rows)
            //    dr.Delete();
            //taxTableItemLvl.AcceptChanges();
            //Session["LEDGER_CUSTOMERNOTE_FinalTaxRecord"] = taxTableItemLvl;
        }
        protected void cmbGstCstVatcharge_Callback(object sender, CallbackEventArgsBase e)
        {
            //Session["SI_TaxDetails"] = null;
            //DateTime quoteDate = Convert.ToDateTime(dt_PLQuote.Date.ToString("yyyy-MM-dd"));
            //PopulateChargeGSTCSTVATCombo(quoteDate.ToString("yyyy-MM-dd"));
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
            if (Session["LEDGER_CUSTOMERNOTE_FinalTaxRecord"] != null)
            {
                TaxDetailTable = (DataTable)Session["LEDGER_CUSTOMERNOTE_FinalTaxRecord"];

                var rows = TaxDetailTable.Select("SlNo ='" + SrlNo + "'");
                foreach (var row in rows)
                {
                    row.Delete();
                }
                TaxDetailTable.AcceptChanges();

                Session["LEDGER_CUSTOMERNOTE_FinalTaxRecord"] = TaxDetailTable;
            }
        }
        public void UpdateTaxDetails(string oldSrlNo, string newSrlNo)
        {
            DataTable TaxDetailTable = new DataTable();
            if (Session["LEDGER_CUSTOMERNOTE_FinalTaxRecord"] != null)
            {
                TaxDetailTable = (DataTable)Session["LEDGER_CUSTOMERNOTE_FinalTaxRecord"];

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

                Session["LEDGER_CUSTOMERNOTE_FinalTaxRecord"] = TaxDetailTable;
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
            //string LastCompany = "";
            //if (Convert.ToString(Session["LastCompany"]) != null)
            //{
            //    LastCompany = Convert.ToString(Session["LastCompany"]);
            //}
            //ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            //proc.AddVarcharPara("@Action", 500, "LoadChargeGSTCSTVATCombo");
            //proc.AddVarcharPara("@S_QuoteAdd_CompanyID", 10, Convert.ToString(LastCompany));
            //proc.AddVarcharPara("@S_quoteDate", 10, quoteDate);
            //DataTable DT = proc.GetTable();
            //cmbGstCstVatcharge.DataSource = DT;
            //cmbGstCstVatcharge.TextField = "Taxes_Name";
            //cmbGstCstVatcharge.ValueField = "Taxes_ID";
            //cmbGstCstVatcharge.DataBind();
        }

        #endregion

        #region Supporting Function For GST
        protected void exUpdatePanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string listData = e.Parameter;
            if (listData.Split('~')[0] == "SessClear")
            {
                SessionClear();

                grid.DataSource = null;
                grid.DataBind();
            }
        }

        private void SessionClear()
        {
            Session.Remove("LEDGER_CUSTOMERNOTE_FinalTaxRecord");
            Session.Remove("SESS_CustomerNoteLedgerDT");
            Session.Remove("SESS_CUSTOMERNOTE_TAX");
            Session.Remove("CustomerDrCrNoteListingDetails");
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

        #endregion

        #region Grid Events
        protected void grid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data)
                return;

            string parentLedgerId = Convert.ToString(e.GetValue("Parent_LedgerID"));
            if (parentLedgerId.Trim() != "0")
            {
               
                e.Row.ForeColor = System.Drawing.Color.FromArgb(0xCC, 0x00, 0x00);
                e.Row.Cells[0].Controls[0].Visible = false;               
                e.Row.Cells[5].Controls[0].Visible = false;

               
                
              
            }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            //((GridViewDataComboBoxColumn)grid.Columns["CountryID"]).PropertiesComboBox.DataSource = GetAllMainAccount();
            //((GridViewDataComboBoxColumn)grid.Columns["CityID"]).PropertiesComboBox.DataSource = GetSubAccount("", Convert.ToString(Session["userbranchID"]), "ALL");

            dsBranch.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlSchematype.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            dsCustomer.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlReasonIssuedocument.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);


            SqlDataSourceapplicable.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            CustomerDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);


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
            if (hfPageLoadFlag.Value == "T")
            {
                if (Session["SESS_CustomerNoteLedgerDT"] != null && ((DataTable)Session["SESS_CustomerNoteLedgerDT"]).Rows.Count > 0)
                {
                    grid.DataSource = GetVoucher((DataTable)Session["SESS_CustomerNoteLedgerDT"]);
                }
                else
                {
                    grid.DataSource = GetVoucher();
                }
            }
            else
            {
                grid.DataSource = null;
            }

        }
        public void BindVoucherGrid()
        {
            grid.DataSource = GetVoucher();
            grid.DataBind();
        }
        //protected void CityCmb_Init(object sender, EventArgs e)
        //{
        //    ASPxComboBox cityCombo = sender as ASPxComboBox;
        //    GridViewEditItemTemplateContainer container = cityCombo.NamingContainer as GridViewEditItemTemplateContainer;
        //    string countryID = Convert.ToString(container.Grid.GetRowValues(container.Grid.VisibleStartIndex, "CountryID"));
        //    grid.JSProperties["cplastCountryID"] = countryID;
        //    cityCombo.DataSource = GetSubAccount(Convert.ToString(countryID), "", "");
        //}
        //private void bindMainAccount(object source, CallbackEventArgsBase e)
        //{
        //    ASPxComboBox currentCombo = source as ASPxComboBox;
        //    currentCombo.DataSource = GetMainAccount(e.Parameter);
        //    currentCombo.DataBind();
        //}
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
                e.Editor.ReadOnly = true;
                //((ASPxComboBox)e.Editor).Callback += new CallbackEventHandlerBase(bindMainAccount);
            }
            else if (e.Column.FieldName == "CityID")
            {
                e.Editor.ReadOnly = true;
            }
            else
            {
                e.Editor.ReadOnly = false;
            }

            
        }
        protected void grid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            grid.JSProperties["cpVouvherNo"] = "";
            grid.JSProperties["cpSaveSuccessOrFail"] = null;

            string Action = Convert.ToString(hdnMode.Value);
            DataTable Journaldt = new DataTable();
            string val = HDParentSlNo.Value;
            CashReportID_Vendor = val;

            if (Action == "0")
            {
                Journaldt.Columns.Add("CashReportID", typeof(string));
                Journaldt.Columns.Add("MainAccount", typeof(string));
                Journaldt.Columns.Add("SubAccount", typeof(string));
                Journaldt.Columns.Add("WithDrawl", typeof(string));
                Journaldt.Columns.Add("Receipt", typeof(string));
                Journaldt.Columns.Add("Narration", typeof(string));
                Journaldt.Columns.Add("Status", typeof(string));

                Journaldt.Columns.Add("Parent_LedgerID", typeof(string));
                Journaldt.Columns.Add("TaxCode", typeof(System.String));
                Journaldt.Columns.Add("Percentage", typeof(System.Decimal));
                Journaldt.Columns.Add("Parent_Sl", typeof(string));

                Journaldt.Columns.Add("MainAccountID", typeof(string));
                Journaldt.Columns.Add("SubAccountID", typeof(string));
                Journaldt.Columns.Add("IsSubledger", typeof(string));
            }
            else
            {
                string VoucherNo = Convert.ToString(hdnNotelNo.Value);

                Journaldt = GetNoteDetails("Details", VoucherNo);
            }
            if (Session["SESS_CustomerNoteLedgerDT"] != null)
            {
                Journaldt = (DataTable)Session["SESS_CustomerNoteLedgerDT"];
            }
            foreach (var args in e.InsertValues)
            {              
                string MainAccount = Convert.ToString(args.NewValues["gvColMainAccount"]);
                string SubAccount = Convert.ToString(args.NewValues["gvColSubAccount"]);

                string WithDrawl = Convert.ToString(args.NewValues["WithDrawl"]);
                string Receipt = "0";
                string Narration = Convert.ToString(args.NewValues["Narration"]);

                string MainAccountID=Convert.ToString(args.NewValues["CountryID"]);
                string SubAccountID = Convert.ToString(args.NewValues["CityID"]);
                string IsSubledger = Convert.ToString(args.NewValues["IsSubledger"]);

                if ((Convert.ToDecimal(WithDrawl) > 0 && MainAccount != "" && MainAccount != null) || (Convert.ToDecimal(Receipt) > 0 && MainAccount != "" && MainAccount != null))
                {                   

                    if (string.IsNullOrEmpty(val) || val == "0")
                    {
                        int maxRowNo = Journaldt.Rows.Count + 1;
                        CashReportID_Vendor = maxRowNo.ToString();
                        HDParentSlNo.Value = maxRowNo.ToString();
                    }
                    else
                    {
                        CashReportID_Vendor = val;
                    }


                    //Journaldt.Rows.Add("0", MainAccount, SubAccount, WithDrawl, Receipt, Narration, "I");
                    DataRow dr = Journaldt.NewRow();
                    dr["CashReportID"] = CashReportID_Vendor;
                    dr["MainAccount"] = MainAccount;
                    dr["SubAccount"] = SubAccount;
                    dr["WithDrawl"] = WithDrawl;
                    dr["Receipt"] = Receipt;
                    dr["Narration"] = Narration;
                    dr["Status"] = "I";
                    dr["Parent_LedgerID"] = "0";
                    dr["TaxCode"] = "0";
                    dr["Percentage"] = 0;
                    dr["Parent_Sl"] = "0";
                    dr["MainAccountID"] = MainAccountID;
                    dr["SubAccountID"] = SubAccountID;
                    dr["IsSubledger"] = IsSubledger;
                    Journaldt.Rows.Add(dr);
                }
            }

            foreach (var args in e.UpdateValues)
            {
                string CashReportID = Convert.ToString(args.Keys["CashReportID"]);
                string MainAccount = Convert.ToString(args.NewValues["gvColMainAccount"]);
                string SubAccount = Convert.ToString(args.NewValues["gvColSubAccount"]);
                string WithDrawl = Convert.ToString(args.NewValues["WithDrawl"]);
                string Receipt = "0";
                string Narration = Convert.ToString(args.NewValues["Narration"]);
                string MainAccountID = Convert.ToString(args.NewValues["CountryID"]);
                string SubAccountID = Convert.ToString(args.NewValues["CityID"]);
                string IsSubledger = Convert.ToString(args.NewValues["IsSubledger"]);
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
                            dr["MainAccountID"] = MainAccountID;
                            dr["SubAccountID"] = SubAccountID;
                            dr["IsSubledger"] = IsSubledger;
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

            if (HDstatus.Value != "S")
            {
                int count = 1;
                foreach (DataRow DR in Journaldt.Rows)
                {
                    DataRow[] HRow = Journaldt.Select("Parent_Sl =" + Convert.ToString(DR["CashReportID"]));
                    foreach (var lst in HRow.ToList())
                    {
                        lst["Parent_Sl"] = Convert.ToString(count).Trim();
                    }
                    DR["CashReportID"] = Convert.ToString(count).Trim();
                    count++;
                }
            }


            Session["SESS_CustomerNoteLedgerDT"] = Journaldt;

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

                string strFinYear = Convert.ToString(Session["LastFinYear"]);
                string strCompanyID = Convert.ToString(Session["LastCompany"]);
                string strBranchID = Convert.ToString(ddlBranch.SelectedValue);
                string strCurrency = Convert.ToString(Session["LocalCurrency"]).Split('~')[0];
                string strUserID = Convert.ToString(Session["userid"]);

                string NoteType = Convert.ToString(ddlNoteType.SelectedValue);
                string NoteDate = Convert.ToString(tDate.Value);
                string MainNarration = Convert.ToString(txtNarration.Text);
                string CustomerName = Convert.ToString(hdfLookupCustomer.Value);
                string InvoiceNo = (Convert.ToString(ddlInvoice.Value) != "") ? Convert.ToString(ddlInvoice.Value) : "0";
                string Currency = Convert.ToString(ddl_Currency.SelectedValue);
                decimal Rate = Convert.ToDecimal(txt_Rate.Value);

                string strPartyInvoice = Convert.ToString(txtPartyInvoice.Text);
                string strPartyDate = "";
                if (dt_PartyDate.Date.ToString("yyyy-MM-dd") != "0001-01-01") strPartyDate = dt_PartyDate.Date.ToString("yyyy-MM-dd");

                string strReason = Convert.ToString(ddl_Reason.SelectedValue);

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

                bool mappingFlag = true;
                #region ########## HSN/SAC mapping Check ##############
                if (HDstatus.Value == "S")
                {
                    DataTable TaxDetailTable = null;
                    try { TaxDetailTable = Journaldt.AsEnumerable().Where(R => R.Field<String>("Parent_LedgerID").Trim() != "0").CopyToDataTable(); }
                    catch (Exception) { }

                    #region ####### Re-Aranged Sl No ##########

                    int dataCount = 1;

                    foreach (DataRow DRLedger in Journaldt.Rows)
                    {
                        string tempCashReportID = Convert.ToString(DRLedger["CashReportID"]);

                        if (TaxDetailTable != null)
                        {
                            foreach (DataRow DRTax in TaxDetailTable.Rows)
                            {
                                if (Convert.ToString(DRTax["Parent_Sl"]).Trim() == tempCashReportID.Trim())
                                {
                                    DRTax["Parent_Sl"] = dataCount.ToString();
                                }
                            }
                        }

                        DRLedger["CashReportID"] = dataCount.ToString();

                        if (Journaldt != null)
                        {
                            foreach (DataRow DRSubLedger in Journaldt.Rows)
                            {
                                if (Convert.ToString(DRSubLedger["Parent_Sl"]).Trim() == tempCashReportID.Trim())
                                {
                                    DRSubLedger["Parent_Sl"] = dataCount.ToString();
                                }
                            }
                        }
                        dataCount++;
                    }


                    #endregion


                    foreach (DataRow DR in Journaldt.Rows)
                    {
                        int hsnSacFlag = HSNSACMappingFlag(Convert.ToString(DR["MainAccount"]));
                        if (hsnSacFlag == 1)
                        {
                            if (Convert.ToString(DR["Parent_Sl"]).Trim() == "0")
                            {
                                if (TaxDetailTable != null)
                                {
                                    var _result = TaxDetailTable.AsEnumerable().Where(r => r.Field<string>("Parent_Sl") == Convert.ToString(DR["CashReportID"])).Select(r => r.Field<string>("Parent_Sl")).FirstOrDefault();
                                    if (_result == null || _result == "")
                                    {
                                        mappingFlag = false;
                                        grid.JSProperties["cpSaveSuccessOrFail"] = "TaxRequired";
                                        break;
                                    }
                                    else
                                    {
                                        mappingFlag = true;
                                        grid.JSProperties["cpSaveSuccessOrFail"] = null;
                                    }
                                }
                                else
                                {
                                    mappingFlag = false;
                                    grid.JSProperties["cpSaveSuccessOrFail"] = "TaxRequired";
                                }
                            }

                        }
                    }
                }


                #endregion


                if (HDstatus.Value != "D" && mappingFlag == true)
                {
                    DataTable TaxDetailTable = null;
                    try
                    {
                        TaxDetailTable = Journaldt.AsEnumerable().Where(R => R.Field<String>("Parent_LedgerID").Trim() != "0").CopyToDataTable();
                        TaxDetailTable.Columns.Remove("MainAccount");
                        TaxDetailTable.Columns.Remove("SubAccount");
                        TaxDetailTable.Columns.Remove("Narration");
                        TaxDetailTable.Columns.Remove("Status");
                        //TaxDetailTable = (DataTable)Session["SESS_CUSTOMERNOTE_TAX"];
                        //TaxDetailTable.Columns.Add("Parent_LedgerID", typeof(string));
                    }
                    catch (Exception ex) { }
                    try
                    {
                        Journaldt.Columns.Remove("TaxCode");
                        Journaldt.Columns.Remove("Percentage");
                        Journaldt.Columns.Remove("Parent_LedgerID");
                        Journaldt.Columns.Remove("Parent_Sl");
                        Journaldt.Columns.Remove("IsSubledger");
                    }
                    catch (Exception ex) { }

                    #region ##### Added By : Samrat Roy -- to get BillingShipping user control data
                    DataTable tempBillAddress = new DataTable();
                    tempBillAddress = BillingShippingControl.SaveBillingShippingControlData();
                    #endregion

                    DataTable Tax = new DataTable();

                    Tax.Columns.Add("SlNo", typeof(System.Int64));
                    Tax.Columns.Add("Amount", typeof(System.Decimal));
                    Tax.Columns.Add("AltTaxCode", typeof(System.Int64));
                    Tax.Columns.Add("Parent_LedgerID", typeof(System.Int64));
                    Tax.Columns.Add("TaxCode", typeof(System.Int64));
                    Tax.Columns.Add("Percentage", typeof(System.Decimal));
                    Tax.Columns.Add("Parent_Sl", typeof(System.Int64));

                   
                    if (TaxDetailTable != null)
                    {
                        foreach (DataRow DR in TaxDetailTable.Rows)
                        {
                            DataRow newRow = Tax.NewRow();
                            newRow["SlNo"] = Convert.ToInt32(DR["CashReportID"]);
                            newRow["Amount"] = Convert.ToDecimal(DR["WithDrawl"]);
                            newRow["AltTaxCode"] = 0;
                            newRow["Parent_LedgerID"] = string.IsNullOrEmpty(Convert.ToString(DR["Parent_LedgerID"])) ? 0 : Convert.ToDecimal(DR["Parent_LedgerID"]);
                            newRow["TaxCode"] = string.IsNullOrEmpty(Convert.ToString(DR["TaxCode"])) ? 0 : Convert.ToDecimal(DR["TaxCode"]);
                            newRow["Percentage"] = Convert.ToDecimal(DR["Percentage"]);
                            newRow["Parent_Sl"] = string.IsNullOrEmpty(Convert.ToString(DR["Parent_Sl"])) ? 0 : Convert.ToDecimal(DR["Parent_Sl"]);
                            Tax.Rows.Add(newRow);
                        }
                    }
                    DataTable tempCashBank = Journaldt.Copy();
                    tempCashBank = tempCashBank.DefaultView.ToTable(false,"CashReportID", "MainAccount", "SubAccount", "WithDrawl", "Receipt", "Narration", "Status");

                    objDebitCreditBL.ModifyDrCrNote(Action, NotelNo, JVNumStr, strFinYear, strCompanyID, strBranchID, NoteDate, strCurrency, MainNarration, NoteType, CustomerName, InvoiceNo, Currency, Rate, strUserID, tempCashBank, ref IsComplete, ref OutNotelNo, strPartyInvoice, strPartyDate, tempBillAddress, Tax, strReason);

                    
                    if (IsComplete == 1)
                    {
                        SessionClear();
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
                else
                {
                    grid.DataSource = GetVoucher(Journaldt);
                    grid.DataBind();
                }
            }
        }

        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string type = Convert.ToString(e.Parameters.Split('~')[0]);
            grid.JSProperties["cpSaveSuccessOrFail"] = null;

            #region Edit
            if (type == "Edit" || type == "View")
            {

                int RowIndex = Convert.ToInt32(e.Parameters.Split('~')[1]);
                string NoteID = Convert.ToString(GvJvSearch.GetRowValues(RowIndex, "DCNote_ID")).Trim();

                #region Tax Edit Data
                hdnNotelNo.Value = NoteID;
                ProcedureExecute proc = new ProcedureExecute("proc_DebitCreditNoteDetails");
                proc.AddVarcharPara("@Action", 150, "NoteTaxDetails");
                proc.AddVarcharPara("@NoteID", 50, NoteID);
                DataTable TaxEditData = proc.GetTable();
                Session["SESS_TaxEditData"] = TaxEditData;
                #endregion

                DataTable Voucherdt = GetNoteDetails("Details", NoteID);


                string Credit = Convert.ToString(Voucherdt.Compute("Sum(Receipt)", ""));
                string Debit = Convert.ToString(Voucherdt.Compute("Sum(WithDrawl)", ""));

                int dtCount = 1;
                foreach (DataRow DR in Voucherdt.Rows)
                {
                    DataRow[] HRow = Voucherdt.Select("Parent_Sl =" + Convert.ToString(DR["CashReportID"]));
                    foreach (var lst in HRow.ToList())
                    {
                        lst["Parent_Sl"] = Convert.ToString(dtCount).Trim();
                        lst["Parent_LedgerID"] = Convert.ToString(DR["MainAccount"]).Trim();
                    }                  

                    if (TaxEditData != null)
                    {
                        DataRow[] TRow = TaxEditData.Select("TaxParent_Sl =" + Convert.ToString(DR["CashReportID"]));
                        foreach (var lst in TRow.ToList())
                        {
                            lst["TaxParent_Sl"] = Convert.ToString(dtCount).Trim();
                        }
                    }

                    DR["CashReportID"] = Convert.ToString(dtCount).Trim();

                    dtCount++;
                }


                DataTable Detailsdt = GetNoteDetails("Header", NoteID);
                if (Detailsdt != null && Detailsdt.Rows.Count > 0)
                {
                    string BranchId = Convert.ToString(Detailsdt.Rows[0]["BranchID"]);
                    string BillNumber = Convert.ToString(Detailsdt.Rows[0]["DocumentNumber"]);
                    string TransactionDate = Convert.ToString(Detailsdt.Rows[0]["DocumentDate"]);
                    string Narration = Convert.ToString(Detailsdt.Rows[0]["Narration"]);

                    string NoteType = Convert.ToString(Detailsdt.Rows[0]["NoteType"]);
                    string CustomerID = Convert.ToString(Detailsdt.Rows[0]["CustomerID"]);
                    string CustomerName = Convert.ToString(Detailsdt.Rows[0]["Name"]);
                    string CustomerUnique = Convert.ToString(Detailsdt.Rows[0]["uniquename"]);
                    string InvoiceID = Convert.ToString(Detailsdt.Rows[0]["InvoiceID"]);
                    string CurrencyId = Convert.ToString(Detailsdt.Rows[0]["CurrencyId"]);
                    string Rate = Convert.ToString(Detailsdt.Rows[0]["Rate"]);

                    string PartyInvoiceNo = Convert.ToString(Detailsdt.Rows[0]["PartyInvoiceNo"]);
                    string PartyInvoiceDate = Convert.ToString(Detailsdt.Rows[0]["PartyInvoiceDate"]);

                    string TaggedDocNumber = Convert.ToString(Detailsdt.Rows[0]["TaggedDocNumber"]);
                    string ReasonID = Convert.ToString(Detailsdt.Rows[0]["ReasonID"]);

                    string MargeDate = BillNumber + "~" + Narration + "~" + BranchId + "~" + Credit + "~" + Debit + "~" + TransactionDate + "~" + NoteType + "~" + CustomerID + "~" + CurrencyId + "~" + Rate + "~" + NoteID + "~" + InvoiceID + "~" + PartyInvoiceNo + "~" + PartyInvoiceDate + "~" + TaggedDocNumber + "~" + CustomerName + "~" + CustomerUnique + "~" + ReasonID;
                    grid.JSProperties["cpEdit"] = MargeDate;
                    grid.JSProperties["cpView"] = (type.ToUpper() == "VIEW") ? "1" : "0";
                }
                Decimal cpTotalAmount = 0;
                foreach (DataRow DR in Voucherdt.Rows)
                {
                    cpTotalAmount += Convert.ToDecimal(DR["WithDrawl"]);
                }

                Decimal cpTotalTaxAmount = 0;
                if (TaxEditData != null)
                {
                    foreach (DataRow DRTax in TaxEditData.Rows)
                    {
                        cpTotalTaxAmount += Convert.ToDecimal(DRTax["Amount"]);
                    }
                    grid.JSProperties["cpTotalTaxableAmount"] = (cpTotalAmount - cpTotalTaxAmount);
                }
                else
                {
                    grid.JSProperties["cpTotalTaxableAmount"] = cpTotalTaxAmount;
                }
               
                grid.JSProperties["cpTotalTaxAmount"] = cpTotalTaxAmount;
                grid.JSProperties["cpTotalAmount"] = cpTotalAmount;

                grid.DataSource = GetVoucher();
                grid.DataBind();

               

            }
            #endregion
            #region Editledger
            else if (type == "Editledger")
            {

                int RowIndex = Convert.ToInt32(e.Parameters.Split('~')[1]);
                string NoteID = Convert.ToString(RowIndex).Trim();

                #region Tax Edit Data
                hdnNotelNo.Value = NoteID;
                ProcedureExecute proc = new ProcedureExecute("proc_DebitCreditNoteDetails");
                proc.AddVarcharPara("@Action", 150, "Details");
                proc.AddVarcharPara("@NoteID", 50, NoteID);
                DataTable TaxEditData = proc.GetTable();
                Session["SESS_TaxEditData"] = TaxEditData;
                #endregion

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
                    string InvoiceID = Convert.ToString(Detailsdt.Rows[0]["InvoiceID"]);
                    string CurrencyId = Convert.ToString(Detailsdt.Rows[0]["CurrencyId"]);
                    string Rate = Convert.ToString(Detailsdt.Rows[0]["Rate"]);

                    string PartyInvoiceNo = Convert.ToString(Detailsdt.Rows[0]["PartyInvoiceNo"]);
                    string PartyInvoiceDate = Convert.ToString(Detailsdt.Rows[0]["PartyInvoiceDate"]);
                    string CustomerName = Convert.ToString(Detailsdt.Rows[0]["Name"]);
                    string CustomerUnique = Convert.ToString(Detailsdt.Rows[0]["uniquename"]);
                    string TaggedDocNumber = Convert.ToString(Detailsdt.Rows[0]["TaggedDocNumber"]);
                    string ReasonID = Convert.ToString(Detailsdt.Rows[0]["ReasonID"]);

                    string MargeDate = BillNumber + "~" + Narration + "~" + BranchId + "~" + Credit + "~" + Debit + "~" + TransactionDate + "~" + NoteType + "~" + CustomerID + "~" + CurrencyId + "~" + Rate + "~" + NoteID + "~" + InvoiceID + "~" + PartyInvoiceNo + "~" + PartyInvoiceDate + "~" + TaggedDocNumber + "~" + CustomerName + "~" + CustomerUnique + "~" + ReasonID;
                    grid.JSProperties["cpEdit"] = MargeDate;
                    grid.JSProperties["cpView"] = (type.ToUpper() == "VIEW") ? "1" : "0";
                }

                grid.DataSource = GetVoucher();
                grid.DataBind();
            }

            #endregion
            #region BlanckEdit
            else if (type == "BlanckEdit")
            {
                grid.DataSource = null;
                grid.DataBind();
            }
            #endregion
            #region AgainDisplay
            else if (type == "AgainDisplay")
            {
                DataTable dt = (DataTable)Session["SESS_CustomerNoteLedgerDT"];
                Decimal cpTotalAmount = 0;
                if (dt != null)
                {
                    foreach (DataRow DR in dt.Rows)
                    {
                        cpTotalAmount += Convert.ToDecimal(DR["WithDrawl"]);
                    }
                }

                Decimal cpTotalTaxAmount = 0;
                DataTable TaxEditData = (DataTable)Session["SESS_CUSTOMERNOTE_TAX"];
                if (TaxEditData != null)
                {
                    foreach (DataRow DRTax in TaxEditData.Rows)
                    {
                        cpTotalTaxAmount += Convert.ToDecimal(DRTax["Amount"]);
                    }
                }
                grid.JSProperties["cpTotalTaxableAmount"] = (cpTotalAmount - cpTotalTaxAmount);
                grid.JSProperties["cpTotalTaxAmount"] = cpTotalTaxAmount;
                grid.JSProperties["cpTotalAmount"] = cpTotalAmount;

                grid.DataSource = GetVoucher((DataTable)Session["SESS_CustomerNoteLedgerDT"]);
                grid.DataBind();
            }
            #endregion
            #region Delete
            else if (type == "Delete")
            {
                try
                {
                    string SlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                    DataTable DT = (DataTable)Session["SESS_CustomerNoteLedgerDT"];

                    var query1 = DT.AsEnumerable().Where(r => r.Field<string>("Parent_Sl") == SlNo); // Child Ledger Delete 
                    if (query1 != null && query1.Count() > 0)
                    {
                        foreach (var row in query1.ToList())
                            row.Delete();
                    }
                    try
                    {
                        foreach (DataRow dr in DT.Rows)
                        {

                            string CashReportID = Convert.ToString(dr["CashReportID"]);
                            if (CashReportID == SlNo)
                            {
                                dr.Delete();
                            }

                        }
                    }
                    catch (Exception ex) { }
                    DT.AcceptChanges();

                    Session["SESS_CustomerNoteLedgerDT"] = DT;
                    if (((DataTable)Session["SESS_CustomerNoteLedgerDT"]) != null && ((DataTable)Session["SESS_CustomerNoteLedgerDT"]).Rows.Count>0)
                    {
                        DataTable dtTax = (DataTable)Session["SESS_CUSTOMERNOTE_TAX"];
                        query1 = dtTax.AsEnumerable().Where(r => r.Field<string>("TaxParent_Sl") == SlNo.Trim()); // Child Ledger Delete 
                        if (query1 != null && query1.Count() > 0)
                        {
                            foreach (var row in query1.ToList())
                                row.Delete();
                        }
                        dtTax.AcceptChanges();
                        Session["SESS_CUSTOMERNOTE_TAX"] = dtTax;

                        #region ####### Re-Aranged Sl No ##########

                        int dataCount = 1;

                        foreach (DataRow DRLedger in DT.Rows)
                        {
                            string tempCashReportID = Convert.ToString(DRLedger["CashReportID"]);

                            if (dtTax != null)
                            {
                                foreach (DataRow DRTax in dtTax.Rows)
                                {
                                    if (Convert.ToString(DRTax["TaxParent_Sl"]).Trim() == tempCashReportID.Trim())
                                    {
                                        DRTax["TaxParent_Sl"] = dataCount.ToString();
                                    }
                                }
                            }

                            DRLedger["CashReportID"] = dataCount.ToString();

                            if (DT != null)
                            {
                                foreach (DataRow DRSubLedger in DT.Rows)
                                {
                                    if (Convert.ToString(DRSubLedger["Parent_Sl"]).Trim() == tempCashReportID.Trim())
                                    {
                                        DRSubLedger["Parent_Sl"] = dataCount.ToString();
                                    }
                                }
                            }

                            dataCount++;
                        }
                        #endregion
                    }
                   
                }
                catch (Exception ex) { }


                double cpTotalAmount = 0;
                if (((DataTable)Session["SESS_CustomerNoteLedgerDT"]) != null)
                {
                    foreach (DataRow DR in ((DataTable)Session["SESS_CustomerNoteLedgerDT"]).Rows)
                    {
                        cpTotalAmount += Convert.ToDouble(DR["WithDrawl"]);
                    }
                }

                double cpTotalTaxAmount = 0;
                DataTable TaxEditData = (DataTable)Session["SESS_CUSTOMERNOTE_TAX"];
                if (TaxEditData != null)
                {
                    foreach (DataRow DRTax in TaxEditData.Rows)
                    {
                        cpTotalTaxAmount += Convert.ToDouble(DRTax["Amount"]);
                    }
                    grid.JSProperties["cpTotalTaxableAmount"] = (cpTotalAmount - cpTotalTaxAmount);
                }
                else
                {
                    grid.JSProperties["cpTotalTaxableAmount"] = cpTotalTaxAmount;
                }
               
                grid.JSProperties["cpTotalTaxAmount"] = cpTotalTaxAmount;
                grid.JSProperties["cpTotalAmount"] = cpTotalAmount;
                grid.DataSource = GetVoucher((DataTable)Session["SESS_CustomerNoteLedgerDT"]);
                grid.DataBind();
            }
            #endregion

            #region TaxReCalculate
            else if (type == "TaxReCalculate")
            {
                string cashReportID = e.Parameters.Split('~')[1];
                string newWithDrawl = e.Parameters.Split('~')[2];
                string actionFor = e.Parameters.Split('~')[3];

                DataTable dt = (DataTable)Session["SESS_CustomerNoteLedgerDT"];

                var query1 = dt.AsEnumerable().Where(r => r.Field<string>("Parent_Sl") == cashReportID); // Child Ledger Delete 
                if (query1 != null && query1.Count() > 0)
                {
                    foreach (var row in query1.ToList())
                        row.Delete();
                }

                dt.AcceptChanges();
                //query1 = dt.AsEnumerable().Where(r => Convert.ToString(r.Field<Int64>("CashReportID")) == cashReportID); // Child Ledger Delete 
                query1 = dt.AsEnumerable().Where(r => r.Field<string>("CashReportID") == cashReportID); // Child Ledger Delete 
                if (query1 != null && query1.Count() > 0)
                {
                    foreach (var row in query1.ToList())
                    {
                        row["WithDrawl"] = string.IsNullOrEmpty(newWithDrawl) ? 0 : Convert.ToDecimal(newWithDrawl);
                        if (actionFor == "D")
                        {
                            if (!string.IsNullOrEmpty(e.Parameters.Split('~')[4]))
                                row["MainAccount"] = Convert.ToString(e.Parameters.Split('~')[4]);
                        }
                        //if (!string.IsNullOrEmpty(e.Parameters.Split('~')[4]) && actionFor == "D")
                        //    row["MainAccount"] = Convert.ToString(e.Parameters.Split('~')[4]);
                    }

                }
                dt.AcceptChanges();

                Session["SESS_CustomerNoteLedgerDT"] = dt;

                DataTable dtTax = (DataTable)Session["SESS_CUSTOMERNOTE_TAX"];
                if (dtTax != null)
                {
                    query1 = dtTax.AsEnumerable().Where(r => r.Field<string>("TaxParent_Sl") == cashReportID); // Child Ledger Delete 
                    if (query1 != null && query1.Count() > 0)
                    {
                        foreach (var row in query1.ToList())
                            row.Delete();
                    }

                    dtTax.AcceptChanges();
                    Session["SESS_CUSTOMERNOTE_TAX"] = dtTax;
                }

                #region ####### Re-Aranged Sl No ##########

                int dataCount = 1;

                foreach (DataRow DRLedger in dt.Rows)
                {
                    string tempCashReportID = Convert.ToString(DRLedger["CashReportID"]);

                    if (dtTax != null)
                    {
                        foreach (DataRow DRTax in dtTax.Rows)
                        {
                            if (Convert.ToString(DRTax["TaxParent_Sl"]).Trim() == tempCashReportID.Trim())
                            {
                                DRTax["TaxParent_Sl"] = dataCount.ToString();
                            }
                        }
                    }

                    DRLedger["CashReportID"] = dataCount.ToString();

                    if (dt != null)
                    {
                        foreach (DataRow DRSubLedger in dt.Rows)
                        {
                            if (Convert.ToString(DRSubLedger["Parent_Sl"]).Trim() == tempCashReportID.Trim())
                            {
                                DRSubLedger["Parent_Sl"] = dataCount.ToString();
                            }
                        }
                    }


                    dataCount++;
                }


                #endregion

                double cpTotalAmount = 0;
                foreach (DataRow DR in dt.Rows)
                {
                    cpTotalAmount += Convert.ToDouble(DR["WithDrawl"]);
                }

                double cpTotalTaxAmount = 0;
                if (dtTax != null)
                {
                    foreach (DataRow DRTax in dtTax.Rows)
                    {
                        cpTotalTaxAmount += Convert.ToDouble(DRTax["Amount"]);
                    }
                }
                grid.JSProperties["cpTotalTaxableAmount"] = (cpTotalAmount - cpTotalTaxAmount);
                grid.JSProperties["cpTotalTaxAmount"] = cpTotalTaxAmount;
                grid.JSProperties["cpTotalAmount"] = cpTotalAmount;

                if (actionFor == "A")
                {
                    /// JSProperties denotes request comes from Amount Change Event
                    grid.JSProperties["cpReCalTax"] = 1;
                }
                else if (actionFor == "D")
                {
                    /// JSProperties denotes request comes from Ledger Change Event
                    grid.JSProperties["cpReCalTaxLedger"] = 1;
                }


                grid.DataSource = GetVoucher((DataTable)Session["SESS_CustomerNoteLedgerDT"]);
                grid.DataBind();
            }
            #endregion
            #region DeleteTaxCalculate
            else if (type == "DeleteTaxCalculate")
            {
                string delCashReportID = Convert.ToString(e.Parameters.Split('~')[1]);
                string strMainAccount = Convert.ToString(e.Parameters.Split('~')[2]);
                string strMainAccountName = Convert.ToString(e.Parameters.Split('~')[3]);
                DataTable tempCreditNote = (DataTable)Session["SESS_CustomerNoteLedgerDT"];
                Double amount;
                double cpTotalTaxAmount = 0;
                Double TaxAmount;
                if (delCashReportID!="null")
                {
                    for (int i = tempCreditNote.Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow dr = tempCreditNote.Rows[i];
                        string strCashReportID = Convert.ToString(dr["CashReportID"]);
                        string Parent_Sl = Convert.ToString(dr["Parent_Sl"]);
                        if (strCashReportID == delCashReportID)
                        {
                            dr["MainAccountID"] = strMainAccountName;
                            dr["MainAccount"] = strMainAccount;
                            Double stramount = Convert.ToDouble(dr["WithDrawl"]);
                            Double TaxableAmount = Convert.ToDouble(txtTaxableAmount.Value);
                            txtTaxableAmount.Value = TaxableAmount - stramount;
                            grid.JSProperties["cpTotalTaxableAmount"] = TaxableAmount - stramount;
                        }
                        if (delCashReportID == Parent_Sl)
                        {
                            amount = Convert.ToDouble(dr["WithDrawl"]);
                            dr.Delete();
                            TaxAmount = Convert.ToDouble(txtTaxAmount.Value);

                            txtTaxAmount.Value = TaxAmount - amount;
                            grid.JSProperties["cpTotalTaxAmount"] = TaxAmount - amount;

                        }
                    }
                    tempCreditNote.AcceptChanges();

                    //tempCreditNote = tempCreditNote.DefaultView.ToTable(false, "CashReportID", "MainAccount", "SubAccount", "WithDrawl", "Receipt", "Narration", "Status");

                    grid.DataSource = GetVoucher(tempCreditNote);
                    grid.DataBind();
                    double cpTotalAmount = 0;
                    foreach (DataRow DR in tempCreditNote.Rows)
                    {
                        cpTotalAmount += Convert.ToDouble(DR["WithDrawl"]);
                    }
                    grid.JSProperties["cpTotalAmount"] = cpTotalAmount;

                    grid.JSProperties["cpDeleteTax"] = "true";
                }
                
            }
            #endregion
        }
        #endregion

        #region predictive Customer

        //protected void ASPxComboBox_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        //{
        //    if (e.Filter != "")
        //    {
        //        ASPxComboBox comboBox = (ASPxComboBox)source;
        //        CustomerDataSource.SelectCommand =
        //               @"select cnt_internalid,uniquename,Name,Billing from (SELECT cnt_internalid,uniquename,Name,Billing, row_number()over(order by t.[Name]) as [rn]  from v_pos_customerDetails  as t where (([uniquename] + ' ' + [Name] ) LIKE @filter)) as st where st.[rn] between @startIndex and @endIndex";

        //        CustomerDataSource.SelectParameters.Clear();
        //        CustomerDataSource.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
        //        CustomerDataSource.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
        //        CustomerDataSource.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
        //        comboBox.DataSource = CustomerDataSource;
        //        comboBox.DataBind();
        //    }
        //}
        //protected void ASPxComboBox_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        //{
        //    long value = 0;
        //    if (e.Value == null || !Int64.TryParse(e.Value.ToString(), out value))
        //        return;
        //    ASPxComboBox comboBox = (ASPxComboBox)source;
        //    CustomerDataSource.SelectCommand = @"SELECT cnt_internalid,uniquename,Name,Billing FROM v_pos_customerDetails WHERE (cnt_internalid = @ID) ";

        //    CustomerDataSource.SelectParameters.Clear();
        //    CustomerDataSource.SelectParameters.Add("ID", TypeCode.String, e.Value.ToString());
        //    comboBox.DataSource = CustomerDataSource;
        //    comboBox.DataBind();
        //}
        //protected void SetCustomerDDbyValue(string customerId)
        //{

        //    CustomerDataSource.SelectCommand = @"SELECT cnt_internalid,uniquename,Name,Billing FROM v_pos_customerDetails WHERE (cnt_internalid = @ID) ";

        //    CustomerDataSource.SelectParameters.Clear();
        //    CustomerDataSource.SelectParameters.Add("ID", TypeCode.String, customerId);
        //    CustomerComboBox.DataSource = CustomerDataSource;
        //    CustomerComboBox.DataBind();
        //    CustomerComboBox.Value = customerId;
        //}

        #endregion

        [WebMethod]
        public static string HSNSACMappingFLag(string LedgerID, object TDate)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            int returnHSNSACMapFlag = 0;
            if(LedgerID!="0")
            {              

                TDate = TDate.ToString().Trim().Substring(6, 4) + "-" + TDate.ToString().Trim().Substring(3, 2) + "-" + TDate.ToString().Trim().Substring(0, 2);
                DataTable taxDetail = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_Tax_Ledger");
                proc.AddVarcharPara("@ApplicableFor", 50, "P");
                proc.AddIntegerPara("@mainAccount_id", string.IsNullOrEmpty(LedgerID) ? 0 : Convert.ToInt32(LedgerID));
                proc.AddVarcharPara("@action", 150, "GetTaxDetails");
                proc.AddVarcharPara("@S_quoteDate", 10, Convert.ToDateTime(TDate).ToString("yyyy-MM-dd"));
                taxDetail = proc.GetTable();
                DataTable DT = taxDetail;              

                if (DT != null)
                {
                    if (DT.Rows.Count > 0)
                    {
                        returnHSNSACMapFlag = string.IsNullOrEmpty(Convert.ToString(DT.Rows[0][0]).Trim()) ? 0 : 1;
                    }
                }
               
            }
            return js.Serialize(returnHSNSACMapFlag);
            
        }
        protected void SelectPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            if (strSplitCommand == "Bindalldesignes")
            {
                string[] filePaths = new string[] { };
                string DesignPath = "";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\CustDrCrNote\DocDesign\Designes";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\CustDrCrNote\DocDesign\Designes";
                }
                string fullpath = Server.MapPath("~");
                fullpath = fullpath.Replace("ERP.UI\\", "");
                string DesignFullPath = fullpath + DesignPath;
                filePaths = System.IO.Directory.GetFiles(DesignFullPath, "*.repx");
                string CrDrNoteType = Convert.ToString(HdCrDrNoteType.Value);
                if (CrDrNoteType == "Credit Note")
                {
                    CrDrNoteType = "Cr";
                }
                else{
                    CrDrNoteType = "Dr";
                }

                foreach (string filename in filePaths)
                {
                    string reportname = Path.GetFileNameWithoutExtension(filename);
                    string name = "";
                    if (reportname.Contains(CrDrNoteType))
                    {
                        if (reportname.Split('~').Length > 1)
                        {
                            name = reportname.Split('~')[0];
                        }
                        else
                        {
                            name = reportname;
                        }
                        string reportValue = reportname;
                        CmbDesignName.Items.Add(name, reportValue);
                    }
                }
                CmbDesignName.SelectedIndex = 0;
            }
            else
            {
                string DesignPath = @"Reports\Reports\REPXReports";
                string fullpath = Server.MapPath("~");
                fullpath = fullpath.Replace("ERP.UI\\", "");
                string filename = @"\RepxReportViewer.aspx";
                string DesignFullPath = fullpath + DesignPath + filename;

                string reportName = Convert.ToString(CmbDesignName.Value);
                SelectPanel.JSProperties["cpSuccess"] = "Success";
            }
        }
        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "DCNote_ID";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);



            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);

            List<int> branchidlist;

            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));

                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    var q = from d in dc.v_CustomerNoteLists
                            where branchidlist.Contains(Convert.ToInt32(d.BranchID))
                            && d.NoteDate >= Convert.ToDateTime(strFromDate) && d.NoteDate <= Convert.ToDateTime(strToDate)
                            
                            orderby d.NoteDate descending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                    var q = from d in dc.v_CustomerNoteLists
                            where
                            branchidlist.Contains(Convert.ToInt32(d.BranchID))
                            && d.NoteDate >= Convert.ToDateTime(strFromDate) && d.NoteDate <= Convert.ToDateTime(strToDate)                             
                            orderby d.NoteDate descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                var q = from d in dc.v_CustomerNoteLists
                        where d.BranchID == '0'
                        orderby d.NoteDate descending
                        select d;
                e.QueryableSource = q;
            }
        }

        #region  Main Account
        //protected void ASPxMainAccountComboBox_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        //{           
        //    if (Convert.ToString(e.Filter).Trim() != "")
        //    {
        //        ASPxComboBox comboBox = (ASPxComboBox)source;
        //        DataTable dt = new DataTable();
        //        string filter = "%" + Convert.ToString(e.Filter) + "%";
        //        int startindex = Convert.ToInt32(e.BeginIndex + 1);
        //        int EndIndex = Convert.ToInt32(e.EndIndex + 1);
        //        string strBranchID = (Convert.ToString(hdnBranchId.Value) == "") ? "0" : Convert.ToString(hdnBranchId.Value);
        //        string strCompanyID = Convert.ToString(Session["LastCompany"]);
        //        dt = GetMainAccountTableNew(strBranchID, filter, startindex, EndIndex, strCompanyID);
        //        comboBox.DataSource = dt;
        //        comboBox.DataBind();
        //    }
        //}
        //protected void ASPxMainComboBox_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        //{
        //    long value = 0;
        //    if (e.Value == null || !Int64.TryParse(e.Value.ToString(), out value))
        //        return;
        //    ASPxComboBox comboBox = (ASPxComboBox)source;
        //    SqlDataSourceMainAccount.SelectCommand = @"SELECT MainAccount_ReferenceID,MainAccount_Name,MainAccount_SubLedgerType,MainAccount_ReverseApplicable,TAXable FROM v_MainAccountList WHERE (MainAccount_ReferenceID = @ID) ";

        //    SqlDataSourceMainAccount.SelectParameters.Clear();
        //    SqlDataSourceMainAccount.SelectParameters.Add("ID", TypeCode.String, e.Value.ToString());
        //    comboBox.DataSource = SqlDataSourceMainAccount;
        //    comboBox.DataBind();
        //}

        //public DataTable GetMainAccountTableNew(string strBranchID, string filter, int startindex, int EndIndex, string strCompanyID)
        //{
        //    DataTable dt = new DataTable();
        //    ProcedureExecute proc = new ProcedureExecute("Fetch_CashBankEntry_DataSet");
        //    proc.AddVarcharPara("@Action", 100, "GetMainAccountList");
        //    proc.AddVarcharPara("@CompanyID", 500, strCompanyID);
        //    proc.AddVarcharPara("@filter", 100, filter);
        //    proc.AddIntegerPara("@startIndex", startindex);
        //    proc.AddIntegerPara("@endIndex", EndIndex);
        //    proc.AddVarcharPara("@BranchID", 100, strBranchID);
        //    dt = proc.GetTable();
        //    return dt;
        //}
        #endregion

        #region  Sub Account
        //protected void SubAcountComboBox_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        //{
        //    if (e.Filter != "")
        //    {
        //        ASPxComboBox comboBox = (ASPxComboBox)source;
        //        DataTable dt = new DataTable();
        //        string filter = "%" + Convert.ToString(e.Filter) + "%";
        //        int startindex = Convert.ToInt32(e.BeginIndex + 1);
        //        int EndIndex = Convert.ToInt32(e.EndIndex + 1);
        //        string MainAccountID = hdnMainAccountId.Value;

        //        dt = GetSubAccountTableNew(MainAccountID, filter, startindex, EndIndex);
        //        comboBox.DataSource = dt;
        //        comboBox.DataBind();
        //    }
        //}
        //public DataTable GetSubAccountTableNew(string strMainAccount, string filter, int startindex, int EndIndex)
        //{
        //    DataTable dt = new DataTable();
        //    ProcedureExecute proc = new ProcedureExecute("Fetch_CashBankEntry_DataSet");
        //    proc.AddVarcharPara("@Action", 100, "GetSubAccountListBYMainAccount");
        //    proc.AddVarcharPara("@MainAccountID", 500, strMainAccount);
        //    proc.AddVarcharPara("@filter", 100, filter);
        //    proc.AddIntegerPara("@startIndex", startindex);
        //    proc.AddIntegerPara("@endIndex", EndIndex);
        //    dt = proc.GetTable();
        //    return dt;
        //}
        //protected void SubAcountComboBox_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        //{
        //    long value = 0;
        //    if (e.Value == null || !Int64.TryParse(e.Value.ToString(), out value))
        //        return;
        //    ASPxComboBox comboBox = (ASPxComboBox)source;
        //    SqlDataSourceSubAccount.SelectCommand = @"SELECT SubAccount_ReferenceID,Contact_Name,MainAccount_SubLedgerType,mainaccount_referenceid FROM v_SubAccountList WHERE (SubAccount_ReferenceID = @ID) ";

        //    SqlDataSourceSubAccount.SelectParameters.Clear();
        //    SqlDataSourceSubAccount.SelectParameters.Add("ID", TypeCode.String, e.Value.ToString());
        //    comboBox.DataSource = SqlDataSourceSubAccount;
        //    comboBox.DataBind();
        //}
        #endregion

    }
}
