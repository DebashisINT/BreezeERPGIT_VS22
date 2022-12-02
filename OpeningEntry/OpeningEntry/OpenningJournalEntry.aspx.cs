using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using DevExpress.Web.Data;
using EntityLayer.CommonELS;
using OpeningEntry.OpeningEntry.DBML;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OpeningEntry.OpeningEntry
{
    public partial class OpenningJournalEntry : System.Web.UI.Page
    {
        #region Global Veriable

        BusinessLogicLayer.OtherTasks oOtherTasks = new BusinessLogicLayer.OtherTasks();
        BusinessLogicLayer.DailyTaskOther oDailyTaskOther = new BusinessLogicLayer.DailyTaskOther();


        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        Converter oconverter = new Converter();
        string JVNumStr = string.Empty;
        //string Session["ErrorMsg"] = string.Empty;
        public static EntityLayer.CommonELS.UserRightsForPage rights;
        bool globalBranchFilter = true;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] != null)
            {
                if (!IsPostBack)
                {

                    dsBranch.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    dsSupplyState.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    dsTaxType.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    SqlDataSourceMainAccount.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    SqlDataSourceSubAccount.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    SqlSchematype.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);




                    rights = new UserRightsForPage();
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/dailytask/JournalEntry.aspx");

                    string[] segmentname = oDBEngine.GetFieldValue1("tbl_master_segment", "Seg_Name", "Seg_id=" + HttpContext.Current.Session["userlastsegment"], 1);
                    ViewState["SegmentName"] = segmentname[0];

                    #region Journal Date

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

                    //DataTable DtWhichSegment = oDBEngine.GetDataTable("(select exch_internalId, isnull((select top 1 exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Segment from tbl_master_companyExchange where exch_compid in (select  ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Session["userlastsegment"].ToString() + " and ls_UserID=" + Session["userid"] + ") and exch_compId='" + HttpContext.Current.Session["LastCompany"].ToString() + "') as D", "*", " Segment in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ")");                
                    DataTable DtWhichSegment = oDBEngine.GetDataTable("(select exch_internalId, isnull((select top 1 exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId),exch_membershiptype) as Segment from tbl_master_companyExchange where exch_compid in (select  ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Convert.ToString(Session["userlastsegment"]) + " and ls_UserID=" + Session["userid"] + ") and exch_compId='" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "') as D", "*", " Segment in(select seg_name from tbl_master_segment where seg_id=" + Convert.ToString(Session["userlastsegment"]) + ")");

                    if (DtWhichSegment.Rows.Count > 0)
                    {
                        ViewState["WhichSegment"] = 1; //Convert.ToString(DtWhichSegment.Rows[0][0]);
                        hdnSegmentid.Value = Convert.ToString(ViewState["WhichSegment"]);
                    }
                    else
                    {
                        ViewState["WhichSegment"] = null;
                        hdnSegmentid.Value = null;
                    }

                    globalBranchFilter = false;
                    //FillSearchGrid();
                    BindBranchFrom();

                    string strdefaultBranch = Convert.ToString(Session["userbranchID"]);
                    ddlBranch.SelectedValue = strdefaultBranch;

                    Session["VoucherNumber"] = null;
                    Session["VoucherIBRef"] = null;
                    Session["exportval"] = null;
                    Session.Remove("GridfullInfo");
                    Session.Remove("ErrorMsg");
                    hdnJournalNo.Value = "";
                    hdnIBRef.Value = "";
                    txt_Credit.Enabled = false;
                    txt_Debit.Enabled = false;


                    if (Request.QueryString.AllKeys.Contains("IsTagged"))
                    {
                        btncross.Visible = false;
                        Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>journalledger(" + Request.QueryString["key"] + ", '" + Request.QueryString["req"] + "');</script>");
                    }

                    #region ####### Search Grid Filter #############

                    //FormDate.Date = DateTime.Now;
                    //toDate.Date = DateTime.Now;
                    DateTime fromDate = Convert.ToDateTime(HttpContext.Current.Session["FinYearStart"]);
                    fromDate = fromDate.AddDays(-1);


                    toDate.Date = fromDate;
                    FormDate.Date = fromDate;

                    toDate.MaxDate = fromDate;
                    FormDate.MaxDate = fromDate;

                    DataSet AllDet = new DataSet();
                    DataTable branchtable = new DataTable();
                    ProcedureExecute proc = new ProcedureExecute("prc_JournalVoucherDetails");
                    proc.AddVarcharPara("@Action", 100, "getBranchListbyHierchy");
                    proc.AddVarcharPara("@BranchList", 1000, Convert.ToString(Session["userbranchHierarchy"]));

                    AllDet = proc.GetDataSet();

                    branchtable = AllDet.Tables[0];

                    cmbBranchfilter.DataSource = branchtable;
                    cmbBranchfilter.ValueField = "branch_id";
                    cmbBranchfilter.TextField = "branch_description";
                    cmbBranchfilter.DataBind();

                    if (AllDet.Tables[1] != null && AllDet.Tables[1].Rows.Count > 0)
                    {
                        HiddenSubMandatory.Value = Convert.ToString(AllDet.Tables[1].Rows[0]["IsSubledgerMandatory"]);
                    }


                    #endregion
                    BindSystemSettings();
                }

            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
        }
        #region System setup
        public DataSet GetSystemSettings()
        {
            DataSet dt = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_JournalVoucherDetails");
            proc.AddVarcharPara("@Action", 100, "GetSystemSettingValue");
            dt = proc.GetDataSet();
            return dt;
        }
        public void BindSystemSettings()
        {
            DataSet dtSystemSettings = new DataSet();
            dtSystemSettings = GetSystemSettings();
            if (dtSystemSettings.Tables[0] != null && dtSystemSettings.Tables[0].Rows.Count > 0)
            {
                string Variable_Value = Convert.ToString(dtSystemSettings.Tables[0].Rows[0]["Variable_Value"]);
                hdnAutoPrint.Value = Variable_Value;
            }
        }
        #endregion

        #region Main Grid

        public void BindBranchFrom()
        {
            dsBranch.SelectCommand = "SELECT BRANCH_id AS BANKBRANCH_ID , RTRIM(BRANCH_DESCRIPTION)+' ['+ISNULL(RTRIM(BRANCH_CODE),'')+']' AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH where BRANCH_id in(" + Convert.ToString(Session["userbranchHierarchy"]) + ")";
            ddlBranch.DataBind();
        }
        protected void GridFullInfo_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            int updtcnt = 0;
            int deletecnt = 0;
            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            string WhichType = null;
            string QuoteStatus = "";
            string remarks = "";

            if (WhichCall == "FilterGridByDate")
            {
                globalBranchFilter = true;
                string FromDate = Convert.ToString(e.Parameters.Split('~')[1]);
                string ToDate = Convert.ToString(e.Parameters.Split('~')[2]);
                string BranchID = Convert.ToString(e.Parameters.Split('~')[3]);


                Session["GridfullInfo"] = FillSearchGridGridFullInfo(ToDate, FromDate, BranchID);
                GridFullInfo.DataSource = (DataTable)Session["GridfullInfo"];
                GridFullInfo.DataBind();
            }





        }
        public DataTable FillSearchGridGridFullInfo(string Todate, string Fromdate, string BranchID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_JournalVoucherDetails");
            proc.AddVarcharPara("@Action", 500, "GetGridDetailsFull");
            proc.AddVarcharPara("@TODATE", 500, Todate);
            proc.AddVarcharPara("@FROMDATE", 500, Fromdate);
            proc.AddVarcharPara("@BranchID", 500, BranchID);
            dt = proc.GetTable();
            return dt;

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
                int RowUpdated = 0;
                RowIndex = Convert.ToInt32(e.Parameters.Split('~')[1]);
                string IBRef = GvJvSearch.GetRowValues(RowIndex, "IBRef").ToString();
                string VoucherNumber = GvJvSearch.GetRowValues(RowIndex, "VoucherNumber").ToString();

                RowUpdated = oDailyTaskOther.Delete_JV(Convert.ToString(IBRef), Convert.ToString(VoucherNumber), Convert.ToInt32(Session["userid"].ToString()));

                if (RowUpdated > 0)
                {
                    GvJvSearch.JSProperties["cpJVDelete"] = "Successfully Deleted";
                }
                else
                {
                    GvJvSearch.JSProperties["cpJVDelete"] = "Problem in Deleting. Sry for Inconvenience";
                }
            }
            else if (PCBCommandName == "FilterGridByDate")
            {
                globalBranchFilter = true;
                DateTime FromDate = Convert.ToDateTime(e.Parameters.Split('~')[1]);
                DateTime ToDate = Convert.ToDateTime(e.Parameters.Split('~')[2]);
                string BranchID = Convert.ToString(e.Parameters.Split('~')[3]);

                GvJvSearch.DataSource = FillSearchGrid(true);
                GvJvSearch.DataBind();
            }
        }
        protected void GridFullInfo_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            GridFullInfo.GroupBy(GridFullInfo.Columns["Voucher_NO"]);
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
        protected void GridFullInfo_DataBinding(object sender, EventArgs e)
        {

            if (Session["GridfullInfo"] != null)
            {
                GridFullInfo.DataSource = (DataTable)Session["GridfullInfo"];
            }

        }
        //protected void GvJvSearch_DataBinding(object sender, EventArgs e)
        //{
        //    GvJvSearch.DataSource = FillSearchGrid(globalBranchFilter);

        //    //globalBranchFilter = false;
        //    #region ########## Existing Code ################

        //    //SqlDataAdapter DaSearchCallBack = new SqlDataAdapter();
        //    //DataSet DsSearchCallBack = new DataSet();
        //    //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
        //    //{
        //    //    string FinYear = Convert.ToString(Session["LastFinYear"]).Trim();
        //    //    string SegmentID = Convert.ToString(ViewState["WhichSegment"]).Trim();
        //    //    string CompanyID = Convert.ToString(Session["LastCompany"]).Trim();
        //    //    string branchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);

        //    //    using (SqlCommand com = new SqlCommand("Search_JournalVoucher", con))
        //    //    {
        //    //        com.CommandType = CommandType.StoredProcedure;
        //    //        com.Parameters.AddWithValue("@FinYear", FinYear);
        //    //        com.Parameters.AddWithValue("@CompanyID", CompanyID);
        //    //        com.Parameters.AddWithValue("@ExchSegmentID", SegmentID);
        //    //        com.Parameters.AddWithValue("@BranchID", branchHierarchy);

        //    //        if (Session["StrQuery"] != null)
        //    //        {
        //    //            com.Parameters.AddWithValue("@QueryAdPart", Convert.ToString(Session["StrQuery"]));
        //    //        }
        //    //        else { com.Parameters.AddWithValue("@QueryAdPart", ""); }

        //    //        using (DaSearchCallBack = new SqlDataAdapter(com))
        //    //        {
        //    //            DsSearchCallBack.Clear();
        //    //            DaSearchCallBack.Fill(DsSearchCallBack);
        //    //        }
        //    //    }
        //    //}

        //    //if (DsSearchCallBack.Tables.Count > 0)
        //    //{
        //    //    if (DsSearchCallBack.Tables[0].Rows.Count > 0)
        //    //    {
        //    //        GvJvSearch.DataSource = DsSearchCallBack.Tables[0];
        //    //    }
        //    //}

        //    #endregion
        //}
        //void FillSearchGrid()
        //{
        //    SqlDataAdapter DaSearchCallBack = new SqlDataAdapter();
        //    DataSet DsSearchCallBack = new DataSet();
        //    using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
        //    {
        //        string FinYear = Convert.ToString(Session["LastFinYear"]).Trim();
        //        string SegmentID = Convert.ToString(ViewState["WhichSegment"]).Trim();
        //        string CompanyID = Convert.ToString(Session["LastCompany"]).Trim();
        //        string branchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);

        //        using (SqlCommand com = new SqlCommand("Search_JournalVoucher", con))
        //        {
        //            com.CommandType = CommandType.StoredProcedure;
        //            com.Parameters.AddWithValue("@FinYear", FinYear);
        //            com.Parameters.AddWithValue("@CompanyID", CompanyID);
        //            com.Parameters.AddWithValue("@ExchSegmentID", SegmentID);
        //            com.Parameters.AddWithValue("@BranchID", branchHierarchy);

        //            if (Session["StrQuery"] != null)
        //            {
        //                com.Parameters.AddWithValue("@QueryAdPart", Convert.ToString(Session["StrQuery"]));
        //            }
        //            else { com.Parameters.AddWithValue("@QueryAdPart", ""); }

        //            using (DaSearchCallBack = new SqlDataAdapter(com))
        //            {
        //                DsSearchCallBack.Clear();
        //                DaSearchCallBack.Fill(DsSearchCallBack);
        //            }
        //        }
        //    }
        //    if (DsSearchCallBack.Tables.Count > 0)
        //    {
        //        if (DsSearchCallBack.Tables[0].Rows.Count > 0)
        //        {
        //            BindGrid(GvJvSearch, DsSearchCallBack.Tables[0]);
        //        }
        //        else
        //        {
        //            BindGrid(GvJvSearch);
        //        }
        //    }
        //    else
        //    {
        //        BindGrid(GvJvSearch);
        //    }
        //}
        private DataTable FillSearchGrid(bool filter = false)  /// filter used for From Date, To Date & Branch wise filter in List Grid
        {
            SqlDataAdapter DaSearchCallBack = new SqlDataAdapter();
            DataSet DsSearchCallBack = new DataSet();

            // using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))

            using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
            {
                string FinYear = Convert.ToString(Session["LastFinYear"]).Trim();
                string SegmentID = Convert.ToString(ViewState["WhichSegment"]).Trim();
                string CompanyID = Convert.ToString(Session["LastCompany"]).Trim();
                string branchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);

                using (SqlCommand com = new SqlCommand("Search_JournalVoucher", con))
                {
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@FinYear", FinYear);
                    com.Parameters.AddWithValue("@CompanyID", CompanyID);
                    com.Parameters.AddWithValue("@ExchSegmentID", SegmentID);

                    if (filter == true)
                    {
                        com.Parameters.AddWithValue("@BranchID", (Convert.ToString(cmbBranchfilter.Value).Trim() == "0") ? Convert.ToString(Session["userbranchHierarchy"]).Trim() : Convert.ToString(cmbBranchfilter.Value).Trim());
                        com.Parameters.AddWithValue("@FromDate", FormDate.Date.ToString("yyyy-MM-dd"));
                        com.Parameters.AddWithValue("@ToDate", toDate.Date.ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        com.Parameters.AddWithValue("@BranchID", branchHierarchy);
                    }

                    if (Session["StrQuery"] != null)
                    {
                        com.Parameters.AddWithValue("@QueryAdPart", Convert.ToString(Session["StrQuery"]));
                    }
                    else { com.Parameters.AddWithValue("@QueryAdPart", ""); }

                    using (DaSearchCallBack = new SqlDataAdapter(com))
                    {
                        DsSearchCallBack.Clear();
                        DaSearchCallBack.Fill(DsSearchCallBack);
                    }
                }
            }

            return DsSearchCallBack.Tables[0];


            #region ######## Existing Code #############
            //SqlDataAdapter DaSearchCallBack = new SqlDataAdapter();
            //DataSet DsSearchCallBack = new DataSet();
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{
            //    string FinYear = Convert.ToString(Session["LastFinYear"]).Trim();
            //    string SegmentID = Convert.ToString(ViewState["WhichSegment"]).Trim();
            //    string CompanyID = Convert.ToString(Session["LastCompany"]).Trim();
            //    string branchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);

            //    using (SqlCommand com = new SqlCommand("Search_JournalVoucher", con))
            //    {
            //        com.CommandType = CommandType.StoredProcedure;
            //        com.Parameters.AddWithValue("@FinYear", FinYear);
            //        com.Parameters.AddWithValue("@CompanyID", CompanyID);
            //        com.Parameters.AddWithValue("@ExchSegmentID", SegmentID);

            //        if (filter == true)
            //        {
            //            com.Parameters.AddWithValue("@BranchID", (Convert.ToString(cmbBranchfilter.Value).Trim() == "0") ? Convert.ToString(Session["userbranchHierarchy"]).Trim() : Convert.ToString(cmbBranchfilter.Value).Trim());
            //            com.Parameters.AddWithValue("@FromDate", FormDate.Date.ToString("yyyy-MM-dd"));
            //            com.Parameters.AddWithValue("@ToDate", toDate.Date.ToString("yyyy-MM-dd"));
            //        }
            //        else
            //        {
            //            com.Parameters.AddWithValue("@BranchID", branchHierarchy);
            //        }

            //        if (Session["StrQuery"] != null)
            //        {
            //            com.Parameters.AddWithValue("@QueryAdPart", Convert.ToString(Session["StrQuery"]));
            //        }
            //        else { com.Parameters.AddWithValue("@QueryAdPart", ""); }

            //        using (DaSearchCallBack = new SqlDataAdapter(com))
            //        {
            //            DsSearchCallBack.Clear();
            //            DaSearchCallBack.Fill(DsSearchCallBack);
            //        }
            //    }
            //}
            //if (DsSearchCallBack.Tables.Count > 0)
            //{
            //    if (DsSearchCallBack.Tables[0].Rows.Count > 0)
            //    {
            //        BindGrid(GvJvSearch, DsSearchCallBack.Tables[0]);
            //    }
            //    else
            //    {
            //        BindGrid(GvJvSearch);
            //    }
            //}
            //else
            //{
            //    BindGrid(GvJvSearch);
            //}
            #endregion
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
            public string MainAccount { get; set; }
            public string bthSubAccount { get; set; }
            public string WithDrawl { get; set; }
            public string Receipt { get; set; }
            public string Narration { get; set; }
            public string gvColMainAccount { get; set; }
            public string gvColSubAccount { get; set; }
            public string gvMainAcCode { get; set; }
            public string IsSubledger { get; set; }
        }

        #endregion

        #region Get Mainaccount, Subaccount & Journal Details

        public IEnumerable GetAllMainAccount()
        {
            List<MainAccount> MainAccountList = new List<MainAccount>();
            //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
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
            // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = objEngine.GetDataTable("Master_MainAccount", " MainAccount_ReferenceID as CountryID,MainAccount_Name+' [ '+rtrim(ltrim(MainAccount_AccountCode))+' ]' as CountryName ", " MainAccount_BankCashType Not In ('Bank','Cash') AND MainAccount_branchId in ('" + branchId + "','0') AND MainAccount_BankCompany in ('" + strCompanyID + "','')");

            DataTable restrictedDT = objEngine.GetDataTable("select branch_id,MainAccount_id from tbl_master_ledgerBranch_map");

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                DataRow[] restrictedTablerow = restrictedDT.Select("MainAccount_id=" + Convert.ToString(DT.Rows[i]["CountryID"]));

                if (restrictedTablerow.Length > 0)
                {
                    DataTable restrictedTable = restrictedTablerow.CopyToDataTable();
                    DataRow[] restrictedRow = restrictedTable.Select("branch_id=" + branchId);
                    if (restrictedRow.Length > 0)
                    {
                        MainAccount MainAccounts = new MainAccount();
                        MainAccounts.CountryID = Convert.ToString(DT.Rows[i]["CountryID"]);
                        MainAccounts.CountryName = Convert.ToString(DT.Rows[i]["CountryName"]);
                        MainAccountList.Add(MainAccounts);
                    }
                }
                else
                {
                    MainAccount MainAccounts = new MainAccount();
                    MainAccounts.CountryID = Convert.ToString(DT.Rows[i]["CountryID"]);
                    MainAccounts.CountryName = Convert.ToString(DT.Rows[i]["CountryName"]);
                    MainAccountList.Add(MainAccounts);
                }
            }

            return MainAccountList;
        }
        public IEnumerable GetSubAccount(string strMainAccount, string strBranch, string strType, string strSubAccount)
        {
            DataTable DT = GetSubAccountTable(strMainAccount, strBranch, strType, strSubAccount);

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
        public DataTable GetSubAccountTable(string strMainAccount, string strBranch, string strType, string strSubAccount)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_FethSubAccount");
            proc.AddVarcharPara("@CashBank_MainAccountID", 500, strMainAccount);
            proc.AddVarcharPara("@clause", 500, "");
            proc.AddVarcharPara("@branch", 500, strBranch);
            proc.AddVarcharPara("@SelectionType", 500, strType);
            proc.AddVarcharPara("@SubAccount", 500, strSubAccount);
            dt = proc.GetTable();
            return dt;
        }
        public IEnumerable GetVoucher()
        {
            DataSet DsOnLoad = new DataSet();
            DataTable tempdt = new DataTable();
            List<VOUCHERLIST> VoucherList = new List<VOUCHERLIST>();

            string VoucherNumber = Convert.ToString(hdnJournalNo.Value);
            string IBRef = Convert.ToString(hdnIBRef.Value);
            DataTable Voucherdt = GetJournalDetails("Details", VoucherNumber, IBRef);

            if (Voucherdt.Rows.Count > 0 && Voucherdt != null)
            {
                for (int i = 0; i < Voucherdt.Rows.Count; i++)
                {
                    VOUCHERLIST Vouchers = new VOUCHERLIST();
                    Vouchers.CashReportID = Convert.ToString(Voucherdt.Rows[i]["CashReportID"]);
                    Vouchers.MainAccount = Convert.ToString(Voucherdt.Rows[i]["MainAccount"]).Trim();
                    Vouchers.bthSubAccount = Convert.ToString(Voucherdt.Rows[i]["bthSubAccount"]).Trim();
                    Vouchers.WithDrawl = Convert.ToString(Voucherdt.Rows[i]["WithDrawl"]);
                    Vouchers.Receipt = Convert.ToString(Voucherdt.Rows[i]["Receipt"]);
                    Vouchers.Narration = Convert.ToString(Voucherdt.Rows[i]["Narration"]);
                    Vouchers.gvColMainAccount = Convert.ToString(Voucherdt.Rows[i]["gvColMainAccount"]).Trim();
                    Vouchers.gvColSubAccount = Convert.ToString(Voucherdt.Rows[i]["gvColSubAccount"]).Trim();
                    Vouchers.gvMainAcCode = Convert.ToString(Voucherdt.Rows[i]["gvMainAcCode"]).Trim();
                    Vouchers.IsSubledger = Convert.ToString(Voucherdt.Rows[i]["IsSubledger"]).Trim();
                    VoucherList.Add(Vouchers);
                }
            }

            return VoucherList;
        }

        //public IEnumerable GetVoucher(DataTable journalDetails)
        //{
        //    DataSet DsOnLoad = new DataSet();
        //    DataTable tempdt = new DataTable();
        //    List<VOUCHERLIST> VoucherList = new List<VOUCHERLIST>();
        //    DataTable Voucherdt = journalDetails;

        //    if (Voucherdt.Rows.Count > 0 && Voucherdt != null)
        //    {
        //        for (int i = 0; i < Voucherdt.Rows.Count; i++)
        //        {
        //            VOUCHERLIST Vouchers = new VOUCHERLIST();
        //            Vouchers.CashReportID = Convert.ToString(Voucherdt.Rows[i]["CashReportID"]);
        //            Vouchers.MainAccount = Convert.ToString(Voucherdt.Rows[i]["MainAccount"]).Trim();
        //            Vouchers.bthSubAccount = Convert.ToString(Voucherdt.Rows[i]["bthSubAccount"]).Trim();
        //            Vouchers.WithDrawl = Convert.ToString(Voucherdt.Rows[i]["WithDrawl"]);
        //            Vouchers.Receipt = Convert.ToString(Voucherdt.Rows[i]["Receipt"]);
        //            Vouchers.Narration = Convert.ToString(Voucherdt.Rows[i]["Narration"]);
        //            Vouchers.gvColMainAccount = Convert.ToString(Voucherdt.Rows[i]["gvColMainAccount"]).Trim();
        //            Vouchers.gvColSubAccount = Convert.ToString(Voucherdt.Rows[i]["gvColSubAccount"]).Trim();
        //            Vouchers.gvMainAcCode = Convert.ToString(Voucherdt.Rows[i]["gvMainAcCode"]).Trim();
        //            VoucherList.Add(Vouchers);
        //        }
        //    }

        //    return VoucherList;
        //}
        public DataTable GetJournalDetails(string Action, string JournalID, string IBRef)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_JournalVoucherDetails");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@JournalID", 500, JournalID);
            proc.AddVarcharPara("@IBRef", 500, IBRef);
            dt = proc.GetTable();
            return dt;
        }
        public static DataTable GetSelectedStateOfSupply(string Action, string BranchId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_JournalVoucherDetails");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@BranchID", 500, BranchId);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetRCM(string Action, string MNainAcId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_JournalVoucherDetails");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@MainAcId", 500, MNainAcId);
            dt = proc.GetTable();
            return dt;
        }


        #endregion

        #region Grid Events


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


        }
        protected void Page_Init(object sender, EventArgs e)
        {
            //((GridViewDataComboBoxColumn)grid.Columns["CountryID"]).PropertiesComboBox.DataSource = GetAllMainAccount();
            // ((GridViewDataComboBoxColumn)grid.Columns["CityID"]).PropertiesComboBox.DataSource = GetSubAccount("", Convert.ToString(Session["userbranchID"]), "ALL", "");

            if (!IsPostBack)
            {
                grid.DataBind();
            }
        }
        protected void CityCmb_Callback(object sender, CallbackEventArgsBase e)
        {
            if (Convert.ToString(e.Parameter.Split('~')[0]) != "null" && Convert.ToString(e.Parameter.Split('~')[0]) != "")
            {
                string countryID = Convert.ToString(e.Parameter.Split('~')[0]);
                string cityID = Convert.ToString(e.Parameter.Split('~')[1]);

                ASPxComboBox c = sender as ASPxComboBox;
                c.DataSource = GetSubAccount(Convert.ToString(countryID), "", "", cityID);//DataProvider.GetCities(countryID);
                c.DataBind();

                DataTable dt = GetRCM("GetRCM", countryID);
                string BranchStateId = Convert.ToString(dt.Rows[0]["IsRCM"]);
                c.JSProperties["cpIsRCM"] = BranchStateId;

            }
        }
        protected void grid_DataBinding(object sender, EventArgs e)
        {
            if (Request.QueryString.AllKeys.Contains("IsTagged"))
            {
                if (Session["TagViewJournal"] != null)
                {


                    grid.DataSource = GetVoucherTagging("", (DataTable)Session["TagViewJournal"]);


                }

            }

            else
            {
                if (Convert.ToString(Session["ErrorMsg"]) == "")
                {
                    grid.DataSource = GetVoucher();
                }
                else
                {

                }
            }
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
            cityCombo.DataSource = GetSubAccount(Convert.ToString(countryID), "", "", "");
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

            //if (e.Column.FieldName == "CountryID")
            //{
            //    ((ASPxComboBox)e.Editor).Callback += new CallbackEventHandlerBase(bindMainAccount);
            //}

            if (e.Column.FieldName == "MainAccount")
            {
                e.Editor.ReadOnly = true;
                e.Editor.Focus();
            }
            else if (e.Column.FieldName == "bthSubAccount")
            {
                e.Editor.ReadOnly = true;
            }
            else
            {
                e.Editor.ReadOnly = false;
            }
        }
        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string type = Convert.ToString(e.Parameters.Split('~')[0]);
            grid.JSProperties["cpSaveSuccessOrFail"] = null;

            if (type == "Edit")
            {
                int RowIndex = Convert.ToInt32(e.Parameters.Split('~')[1]);
                string VoucherNumber = GvJvSearch.GetRowValues(RowIndex, "VoucherNumber").ToString();
                string IBRef = GvJvSearch.GetRowValues(RowIndex, "IBRef").ToString();

                hdnJournalNo.Value = VoucherNumber;
                hdnIBRef.Value = IBRef;
                Session["VoucherNumber"] = VoucherNumber;
                Session["VoucherIBRef"] = IBRef;

                DataTable Voucherdt = GetJournalDetails("Details", VoucherNumber, IBRef);
                string Credit = Convert.ToString(Voucherdt.Compute("Sum(Receipt)", ""));
                string Debit = Convert.ToString(Voucherdt.Compute("Sum(WithDrawl)", ""));


                DataTable Detailsdt = GetJournalDetails("Header", VoucherNumber, IBRef);
                if (Detailsdt != null && Detailsdt.Rows.Count > 0)
                {
                    string BranchId = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_BranchID"]);
                    string BillNumber = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_BillNumber"]);
                    string TransactionDate = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_TransactionDate"]);
                    string JvNarration = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_Narration"]);
                    string PlaceOfSupply = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_PlaceOfSupply"]);
                    string Taxoption = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_TaxOption"]);
                    string IsPartyJournal = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_IsPartyJournal"]);
                    string PartyCount = Convert.ToString(Detailsdt.Rows[0]["PartyCount"]);
                    string IsRCMD = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_IsRCM"]);
                    //hdnIsPartyLedger.Value = PartyCount;

                    grid.JSProperties["cpEdit"] = BillNumber + "~" + JvNarration + "~" + BranchId + "~" + Credit + "~" + Debit + "~" + TransactionDate + "~" + PlaceOfSupply + "~" + Taxoption + "~" + IsPartyJournal + "~" + PartyCount + "~" + IsRCMD;
                }

                grid.DataSource = GetVoucher();
                grid.DataBind();
            }


            else if (type == "View")
            {

                string VoucherNumber = e.Parameters.Split('~')[1];
                //string IBRef = GvJvSearch.GetRowValues(0, "IBRef").ToString();

                hdnJournalNo.Value = VoucherNumber;
                // hdnIBRef.Value = IBRef;


                // Session["VoucherNumber"] = VoucherNumber;
                // Session["VoucherIBRef"] = IBRef;

                DataTable Voucherdt = GetJournalDetails("DetailsTagging", VoucherNumber, "0");

                string Credit = Convert.ToString(Voucherdt.Compute("Sum(Receipt)", ""));
                string Debit = Convert.ToString(Voucherdt.Compute("Sum(WithDrawl)", ""));


                ///   DataTable Detailsdt = GetJournalDetails("Header", VoucherNumber, IBRef);

                DataTable Detailsdt = GetJournalDetails("HeaderTagging", VoucherNumber, "0");



                if (Detailsdt != null && Detailsdt.Rows.Count > 0)
                {
                    string BranchId = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_BranchID"]);
                    string BillNumber = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_BillNumber"]);
                    string TransactionDate = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_TransactionDate"]);
                    string JvNarration = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_Narration"]);
                    string PlaceOfSupply = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_PlaceOfSupply"]);
                    string Taxoption = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_TaxOption"]);

                    grid.JSProperties["cpEdit"] = BillNumber + "~" + JvNarration + "~" + BranchId + "~" + Credit + "~" + Debit + "~" + TransactionDate + "~" + PlaceOfSupply + "~" + Taxoption;
                }




                DataTable Voucherdt1 = GetJournalDetails("DetailsTagging", VoucherNumber, "0");
                Session["TagViewJournal"] = Voucherdt1;

                grid.DataSource = GetVoucherTagging(VoucherNumber, Voucherdt1);
                grid.DataBind();
                Session.Remove("TagViewJournal");

            }
            else if (type == "BlanckEdit")
            {
                grid.DataSource = null;
                grid.DataBind();

            }
        }
        public IEnumerable GetVoucherTagging(string journal, DataTable Voucherdt)
        {
            DataSet DsOnLoad = new DataSet();
            DataTable tempdt = new DataTable();
            List<VOUCHERLIST> VoucherList = new List<VOUCHERLIST>();

            string VoucherNumber = Convert.ToString(hdnJournalNo.Value);
            //  string IBRef = Convert.ToString(hdnIBRef.Value);


            if (Voucherdt.Rows.Count > 0 && Voucherdt != null)
            {
                for (int i = 0; i < Voucherdt.Rows.Count; i++)
                {
                    VOUCHERLIST Vouchers = new VOUCHERLIST();
                    Vouchers.CashReportID = Convert.ToString(Voucherdt.Rows[i]["CashReportID"]);
                    Vouchers.MainAccount = Convert.ToString(Voucherdt.Rows[i]["MainAccount"]).Trim();
                    Vouchers.bthSubAccount = Convert.ToString(Voucherdt.Rows[i]["bthSubAccount"]).Trim();
                    Vouchers.WithDrawl = Convert.ToString(Voucherdt.Rows[i]["WithDrawl"]);
                    Vouchers.Receipt = Convert.ToString(Voucherdt.Rows[i]["Receipt"]);
                    Vouchers.Narration = Convert.ToString(Voucherdt.Rows[i]["Narration"]);
                    Vouchers.gvColMainAccount = Convert.ToString(Voucherdt.Rows[i]["gvColMainAccount"]).Trim();
                    Vouchers.gvColSubAccount = Convert.ToString(Voucherdt.Rows[i]["gvColSubAccount"]).Trim();
                    Vouchers.gvMainAcCode = Convert.ToString(Voucherdt.Rows[i]["gvMainAcCode"]).Trim();
                    Vouchers.IsSubledger = Convert.ToString(Voucherdt.Rows[i]["IsSubledger"]).Trim();
                    VoucherList.Add(Vouchers);
                }
            }

            return VoucherList;
        }



        protected void grid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {



            grid.JSProperties["cpVouvherNo"] = "";
            grid.JSProperties["cpSaveSuccessOrFail"] = null;
            string ValidateSubAccount = "";
            string Action = Convert.ToString(hdnMode.Value);
            DataTable Journaldt = new DataTable();


            if (Action == "0")
            {
                Journaldt.Columns.Add("CashReportID", typeof(string));
                Journaldt.Columns.Add("MainAccount", typeof(string));
                Journaldt.Columns.Add("SubAccount", typeof(string));
                Journaldt.Columns.Add("WithDrawl", typeof(string));
                Journaldt.Columns.Add("Receipt", typeof(string));
                Journaldt.Columns.Add("Narration", typeof(string));
                Journaldt.Columns.Add("Status", typeof(string));
            }
            else
            {
                string VoucherNo = Convert.ToString(Session["VoucherNumber"]);
                string IBRef = Convert.ToString(Session["VoucherIBRef"]);
                if (Convert.ToString(Session["ErrorMsg"]) != "HasError")
                {

                    Journaldt = GetJournalDetails("Details", VoucherNo, IBRef);
                    foreach (DataRow dr in Journaldt.Rows)
                    {
                        dr["MainAccount"] = Convert.ToString(dr["gvColMainAccount"]);
                        dr["bthSubAccount"] = Convert.ToString(dr["gvColSubAccount"]);
                    }
                    Journaldt.Columns.Remove("gvColSubAccount");
                    Journaldt.Columns.Remove("gvColMainAccount");
                    Journaldt.Columns.Remove("gvMainAcCode");
                    Journaldt.Columns.Remove("IsSubledger");
                }
                else
                {
                    Journaldt.Columns.Add("CashReportID", typeof(string));
                    Journaldt.Columns.Add("MainAccount", typeof(string));
                    Journaldt.Columns.Add("SubAccount", typeof(string));
                    Journaldt.Columns.Add("WithDrawl", typeof(string));
                    Journaldt.Columns.Add("Receipt", typeof(string));
                    Journaldt.Columns.Add("Narration", typeof(string));
                    Journaldt.Columns.Add("Status", typeof(string));
                }
            }

            foreach (var args in e.InsertValues)
            {
                string MainAccount = Convert.ToString(args.NewValues["gvColMainAccount"]);
                string SubAccount = Convert.ToString(args.NewValues["gvColSubAccount"]);
                string WithDrawl = Convert.ToString(args.NewValues["WithDrawl"]);
                string Receipt = Convert.ToString(args.NewValues["Receipt"]);
                string Narration = Convert.ToString(args.NewValues["Narration"]);

                if ((Convert.ToDecimal(WithDrawl) > 0 && MainAccount != "" && MainAccount != null) || (Convert.ToDecimal(Receipt) > 0 && MainAccount != "" && MainAccount != null))
                {
                    Journaldt.Rows.Add("0", MainAccount, SubAccount, WithDrawl, Receipt, Narration, "I");
                }
            }

            foreach (var args in e.UpdateValues)
            {
                string CashReportID = Convert.ToString(args.Keys["CashReportID"]);
                string MainAccount = Convert.ToString(args.NewValues["gvColMainAccount"]);
                string SubAccount = Convert.ToString(args.NewValues["gvColSubAccount"]);
                string WithDrawl = Convert.ToString(args.NewValues["WithDrawl"]);
                string Receipt = Convert.ToString(args.NewValues["Receipt"]);
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
                Journaldt.AcceptChanges();

                if (isDeleted == false)
                {
                    if ((Convert.ToDecimal(WithDrawl) > 0 && MainAccount != "" && MainAccount != null) || (Convert.ToDecimal(Receipt) > 0 && MainAccount != "" && MainAccount != null))
                    {

                        if (Convert.ToString(Session["ErrorMsg"]) == "")
                        {
                            DataRow dr = Journaldt.Select("CashReportID ='" + CashReportID + "'").FirstOrDefault(); // finds all rows with id==2 and selects first or null if haven't found any

                            if (dr != null)
                            {
                                dr["MainAccount"] = MainAccount;
                                dr["bthSubAccount"] = SubAccount;
                                dr["WithDrawl"] = WithDrawl;
                                dr["Receipt"] = Receipt;
                                dr["Narration"] = Narration;
                                dr["Status"] = "U";
                            }

                        }
                        else

                            Journaldt.Rows.Add("0", MainAccount, SubAccount, WithDrawl, Receipt, Narration, "I");
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





            string validate = checkNMakeJVCode(Convert.ToString(txtBillNo.Text), Convert.ToInt32(CmbScheme.SelectedValue));
            //foreach (DataRow dr in Journaldt.Rows)
            //{
            //    string SupplyState = Convert.ToString(ddlSupplyState.SelectedItem.Text);
            //    string Supplystate = SupplyState.Substring(SupplyState.IndexOf(':') + 1, (SupplyState.Length - (SupplyState.IndexOf(':') + 1))).Replace(")", "");
            //    string strBranchID = Convert.ToString(ddlBranch.SelectedValue);
            //    DataTable dt = new DataTable();
            //    ProcedureExecute proc = new ProcedureExecute("prc_JournalVoucherDetails");
            //    proc.AddVarcharPara("@Action", 500, "GetReverseLedger");
            //    proc.AddVarcharPara("@MainAcIDForReverseLedger", 500, Convert.ToString(dr["MainAccount"]));
            //    proc.AddVarcharPara("@AmountCr", 500, Convert.ToString(dr["Receipt"]));
            //    proc.AddVarcharPara("@AmountDr", 500, Convert.ToString(dr["Withdrawl"]));
            //    proc.AddVarcharPara("@CompanyID", 500, Convert.ToString(Session["LastCompany"]));
            //    proc.AddVarcharPara("@SupplyState", 500, Convert.ToString(Supplystate));
            //    proc.AddVarcharPara("@BranchID", 500, Convert.ToString(strBranchID));
            //    dt = proc.GetTable();
            //    if (dt.Rows.Count > 0)
            //    {
            //        string str = Convert.ToString(dt.Rows[0]["RcmType"]);
            //        if (str != "NonRcm")
            //        {
            //            //var ddr = dt.AsEnumerable().Where(x => x.Field<string>("TaxRates_ReverseChargeMainAccount") == "").Single();
            //            DataRow drr = dt.Select("TaxRates_ReverseChargeMainAccount is null or TaxRates_ReverseChargeMainAccount =''").FirstOrDefault();
            //            //.Select("TaxRates_Reverse is null or TaxRates_Reverse=''").FirstOrDefault();
            //            if (drr != null)
            //            {
            //                validate = "HasError";
            //                break;
            //            }

            //        }
            //        //string str = "";
            //        //string str = "";
            //    }

            //}



            if (validate == "outrange" || validate == "duplicate" || validate == "HasError")
            {
                grid.JSProperties["cpSaveSuccessOrFail"] = validate;
            }
            else if (ValidateSubAccount == "Subaccountmandatory")
            {
                grid.JSProperties["cpSaveSuccessOrFail"] = ValidateSubAccount;
            }
            else
            {
                if (Action == "0")
                {
                    DataTable dtSchema = oDBEngine.GetDataTable("tbl_master_idschema", "prefix, suffix, digit, startno, schema_type", "id=" + Convert.ToInt32(CmbScheme.SelectedValue));
                    int scheme_type = Convert.ToInt32(dtSchema.Rows[0]["schema_type"]);
                    if (scheme_type != 0)
                    {
                        grid.JSProperties["cpVouvherNo"] = JVNumStr;
                    }
                }

                string strFinYear = Convert.ToString(Session["LastFinYear"]);
                string strCompanyID = Convert.ToString(Session["LastCompany"]);
                string strBranchID = Convert.ToString(ddlBranch.SelectedValue);
                string strSegmentID = Convert.ToString(ViewState["WhichSegment"]);
                string strCurrency = Convert.ToString(Session["LocalCurrency"]).Split('~')[0];
                string strUserID = Convert.ToString(Session["userid"]);
                string JournalDate = Convert.ToString(tDate.Value);
                string MainNarration = Convert.ToString(txtNarration.Text);
                string SupplyState = Convert.ToString(ddlSupplyState.SelectedItem.Text);
                string TaxOption = Convert.ToString(ddl_AmountAre.SelectedItem.Value);
                string Supplystate = SupplyState.Substring(SupplyState.IndexOf(':') + 1, (SupplyState.Length - (SupplyState.IndexOf(':') + 1))).Replace(")", "");
                string SupplyStateId = Convert.ToString(ddlSupplyState.SelectedItem.Value);
                bool IsRCM = (bool)IsRcm.Value;
                string JournalID = "", IBRef = "";
                if (Action == "0")
                {
                    Action = "Add";
                    JournalID = "";
                    IBRef = oconverter.GetAutoGenerateNo();
                }
                else
                {
                    Action = "Edit";
                    JournalID = Convert.ToString(Session["VoucherNumber"]);
                    IBRef = Convert.ToString(Session["VoucherIBRef"]);
                }
                int i = 1;
                foreach (DataRow Dr in Journaldt.Rows)
                {

                    Dr["CashReportID"] = i;
                    i = i + 1;
                }
                Journaldt.AcceptChanges();
                if (Journaldt.Rows.Count > 0)
                {
                    if (ModifyJournal(Action, JournalID, JVNumStr, strFinYear, strCompanyID, strBranchID, JournalDate, strSegmentID, strCurrency, IBRef, MainNarration, strUserID, Journaldt, Supplystate, TaxOption, SupplyStateId, IsRCM) == true)
                    {
                        Session["VoucherNumber"] = null;
                        Session["VoucherIBRef"] = null;
                        Session["ErrorMsg"] = null;
                        hdnJournalNo.Value = "";
                        hdnIBRef.Value = "";
                        grid.JSProperties["cpSaveSuccessOrFail"] = "successInsert";
                    }

                    else
                    {
                        if (Convert.ToString(Session["ErrorMsg"]) == "" || Convert.ToString(Session["ErrorMsg"]) == null)
                        {
                            grid.JSProperties["cpSaveSuccessOrFail"] = "errorInsert";
                        }
                        else
                        {
                            grid.JSProperties["cpSaveSuccessOrFail"] = Session["ErrorMsg"];


                        }
                    }
                }
            }
        }
        protected void GridFullInfo_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {

            e.Text = string.Format("{0}", Convert.ToDecimal(e.Value));

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
        public bool ModifyJournal(string ActionType, string JournalID, string BillNo, string FinYear, string CompanyID, string BranchID, string JournalDate, string SegmentID,
                                   string CurrencyID, string IBRef, string Narration, string UserID, DataTable JournalDetails, string SupplyState, string TaxOption, string SupplyStateId, bool IsRCM)
        {
            try
            {
                DataSet dsInst = new DataSet();
                //  SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                SqlCommand cmd = new SqlCommand("prc_InsertJournalVoucherEntry", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", ActionType);
                cmd.Parameters.AddWithValue("@JournalID", JournalID);
                cmd.Parameters.AddWithValue("@BillNo", BillNo);
                cmd.Parameters.AddWithValue("@FinYear", FinYear);
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                cmd.Parameters.AddWithValue("@BranchID", BranchID);
                cmd.Parameters.AddWithValue("@JournalDate", Convert.ToDateTime(JournalDate));
                cmd.Parameters.AddWithValue("@SegmentID", SegmentID);
                cmd.Parameters.AddWithValue("@CurrencyID", CurrencyID);
                cmd.Parameters.AddWithValue("@IBRef", IBRef);
                cmd.Parameters.AddWithValue("@Narration", Narration);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@JournalDetails", JournalDetails);
                cmd.Parameters.AddWithValue("@SupplyState", SupplyState);
                cmd.Parameters.AddWithValue("@TaxOption", TaxOption);
                cmd.Parameters.AddWithValue("@SupplyStateId", SupplyStateId);
                cmd.Parameters.AddWithValue("@IsRCM", IsRCM);
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();



                Session["ErrorMsg"] = Convert.ToString(dsInst.Tables[0].Rows[0]["Error"]);
                if (Convert.ToString(Session["ErrorMsg"]) != "HasError")
                {
                    return true;
                }
                else
                {
                    List<VOUCHERLIST> VoucherList = new List<VOUCHERLIST>();
                    if (dsInst.Tables[1].Rows.Count > 0 && dsInst.Tables[1] != null)
                    {
                        for (int i = 0; i < dsInst.Tables[1].Rows.Count; i++)
                        {
                            VOUCHERLIST Vouchers = new VOUCHERLIST();
                            Vouchers.CashReportID = Convert.ToString(dsInst.Tables[1].Rows[i]["CashReportID"]);
                            Vouchers.gvColMainAccount = Convert.ToString(dsInst.Tables[1].Rows[i]["gvColMainAccount"]).Trim();
                            Vouchers.gvColSubAccount = Convert.ToString(dsInst.Tables[1].Rows[i]["gvColSubAccount"]).Trim();
                            Vouchers.WithDrawl = Convert.ToString(dsInst.Tables[1].Rows[i]["WithDrawl"]);
                            Vouchers.Receipt = Convert.ToString(dsInst.Tables[1].Rows[i]["Receipt"]);
                            Vouchers.Narration = Convert.ToString(dsInst.Tables[1].Rows[i]["Narration"]);
                            Vouchers.MainAccount = Convert.ToString(dsInst.Tables[1].Rows[i]["MainAccount"]).Trim();
                            Vouchers.bthSubAccount = Convert.ToString(dsInst.Tables[1].Rows[i]["bthSubAccount"]).Trim();
                            Vouchers.gvMainAcCode = Convert.ToString(dsInst.Tables[1].Rows[i]["gvMainAcCode"]).Trim();
                            Vouchers.IsSubledger = Convert.ToString(dsInst.Tables[1].Rows[i]["IsSubledger"]).Trim();
                            VoucherList.Add(Vouchers);
                        }
                    }



                    grid.DataSource = VoucherList;
                    grid.DataBind();
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
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
                status = objMShortNameCheckingBL.CheckUnique(Convert.ToString(VoucherNo).Trim(), Type, "JournalVoucher_Check");
            }
            return status;
        }
        [WebMethod]
        public static string getSchemeType(string sel_scheme_id)
        {
            string strschematype = "", strschemalength = "", strschemavalue = "", strschemaBranch = "";

            // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

            DataTable DT = objEngine.GetDataTable("tbl_master_Idschema", " schema_type,length,Branch ", " Id = " + Convert.ToInt32(sel_scheme_id));

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                strschematype = Convert.ToString(DT.Rows[i]["schema_type"]);
                strschemalength = Convert.ToString(DT.Rows[i]["length"]);
                strschemaBranch = Convert.ToString(DT.Rows[i]["Branch"]);
                strschemavalue = strschematype + "~" + strschemalength + "~" + strschemaBranch;
            }

            DataTable dt = GetSelectedStateOfSupply("GetBranchStateCode", strschemaBranch);
            string BranchStateId = Convert.ToString(dt.Rows[0]["StateCode"]);
            strschemavalue = strschemavalue + "~" + BranchStateId;
            return Convert.ToString(strschemavalue);
        }

        #endregion

        #region Others

        //protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
        //    if (Filter != 0)
        //    {
        //        if (Session["exportval"] == null)
        //        {
        //            Session["exportval"] = Filter;
        //            bindexport(Filter);
        //        }
        //        else if (Convert.ToInt32(Session["exportval"]) != Filter)
        //        {
        //            Session["exportval"] = Filter;
        //            bindexport(Filter);
        //        }
        //    }
        //}
        public void bindexport(int Filter)
        {
            // GvJvSearch.Columns[11].Visible = false;

            string filename = "JournalVoucher";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Journal Voucher";
            exporter.PageFooter.Center = "[Page # of Pages #]";

            if (ASPxPageControl1.ActiveTabPage.Index == 0)
            {
                exporter.GridViewID = "GvJvSearch";
            }
            else if (ASPxPageControl1.ActiveTabPage.Index == 1)
            {
                exporter.GridViewID = "GridFullInfo";
                exporter.Landscape = true;
            }
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
                    prefCompCode = (dtSchema.Rows[0]["prefix"].ToString() == "TCURDATE/") ? (tDate.Date.ToString("ddMMyyyy") + "/") : (dtSchema.Rows[0]["prefix"].ToString());
                    sufxCompCode = (dtSchema.Rows[0]["suffix"].ToString() == "/TCURDATE") ? ("/" + tDate.Date.ToString("ddMMyyyy")) : (dtSchema.Rows[0]["suffix"].ToString());
                    prefLen = Convert.ToInt32(prefCompCode.Length);
                    sufxLen = Convert.ToInt32(sufxCompCode.Length);

                    if ((dtSchema.Rows[0]["prefix"].ToString() == "TCURDATE/") || (dtSchema.Rows[0]["suffix"].ToString() == "/TCURDATE"))
                    {
                        sqlQuery = "SELECT max(tjv.JournalVoucher_BillNumber) FROM Trans_JournalVoucher tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + paddCounter + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[\\" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1 and JournalVoucher_BillNumber like '%" + sufxCompCode + "'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, JournalVoucher_CreateDateTime) = CONVERT(DATE, GETDATE())";
                        dtC = oDBEngine.GetDataTable(sqlQuery);

                        if (dtC.Rows[0][0].ToString() == "")
                        {
                            sqlQuery = "SELECT max(tjv.JournalVoucher_BillNumber) FROM Trans_JournalVoucher tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[\\" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1 and JournalVoucher_BillNumber like '%" + sufxCompCode + "'";
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
                        sqlQuery = "SELECT max(tjv.JournalVoucher_BillNumber) FROM Trans_JournalVoucher tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + paddCounter + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1 and JournalVoucher_BillNumber like '" + prefCompCode + "%'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, JournalVoucher_CreateDateTime) = CONVERT(DATE, GETDATE())";
                        dtC = oDBEngine.GetDataTable(sqlQuery);

                        if (dtC.Rows[0][0].ToString() == "")
                        {
                            sqlQuery = "SELECT max(tjv.JournalVoucher_BillNumber) FROM Trans_JournalVoucher tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            else if (scheme_type == 2)
                                sqlQuery += "^";
                            sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_BillNumber))) = 1 and JournalVoucher_BillNumber like '" + prefCompCode + "%'";
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
                }
                else
                {
                    sqlQuery = "SELECT JournalVoucher_BillNumber FROM Trans_JournalVoucher WHERE JournalVoucher_BillNumber LIKE '" + manual_str.Trim() + "'";
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
            Response.Redirect("JournalEntry.aspx");
        }

        #endregion
        protected void SelectPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            if (strSplitCommand == "Bindalldesignes")
            {
                string[] filePaths = new string[] { };
                string DesignPath = "";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\JournalVoucher\DocDesign\Designes";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\JournalVoucher\DocDesign\Designes";
                }
                string fullpath = Server.MapPath("~");
                fullpath = fullpath.Replace("ERP.UI\\", "");
                string DesignFullPath = fullpath + DesignPath;
                filePaths = System.IO.Directory.GetFiles(DesignFullPath, "*.repx");

                foreach (string filename in filePaths)
                {
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
                    CmbDesignName.Items.Add(name, reportValue);
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
            e.KeyExpression = "VoucherNumber";

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

                    OpeningEntryDBMLDataContext dc = new OpeningEntryDBMLDataContext(connectionString);
                    var q = from d in dc.v_JournalEntryLists
                            where d.TransactionDate >= Convert.ToDateTime(strFromDate) && d.TransactionDate <= Convert.ToDateTime(strToDate)
                            && branchidlist.Contains(Convert.ToInt32(d.BranchID))
                            orderby d.TransactionDate descending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));

                    OpeningEntryDBMLDataContext dc = new OpeningEntryDBMLDataContext(connectionString);
                    var q = from d in dc.v_JournalEntryLists
                            where
                            d.TransactionDate >= Convert.ToDateTime(strFromDate) && d.TransactionDate <= Convert.ToDateTime(strToDate) &&
                            branchidlist.Contains(Convert.ToInt32(d.BranchID))
                            orderby d.TransactionDate descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                OpeningEntryDBMLDataContext dc = new OpeningEntryDBMLDataContext(connectionString);
                var q = from d in dc.v_JournalEntryLists
                        where d.BranchID == '0'
                        orderby d.TransactionDate descending
                        select d;
                e.QueryableSource = q;
            }
        }
        #region Main Account Pop Up
        protected void ASPxMainAccountComboBox_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            if (e.Filter != "")
            {
                ASPxComboBox comboBox = (ASPxComboBox)source;
                DataTable dt = new DataTable();
                string filter = "%" + Convert.ToString(e.Filter) + "%";
                int startindex = Convert.ToInt32(e.BeginIndex + 1);
                int EndIndex = Convert.ToInt32(e.EndIndex + 1);
                string strBranchID = (Convert.ToString(hdnBranchId.Value) == "") ? "0" : Convert.ToString(hdnBranchId.Value);
                string strCompanyID = Convert.ToString(Session["LastCompany"]);
                dt = GetMainAccountTableNew(strBranchID, filter, startindex, EndIndex, strCompanyID);
                comboBox.DataSource = dt;
                comboBox.DataBind();
            }
        }
        protected void ASPxMainComboBox_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            long value = 0;
            if (e.Value == null || !Int64.TryParse(e.Value.ToString(), out value))
                return;
            ASPxComboBox comboBox = (ASPxComboBox)source;
            SqlDataSourceMainAccount.SelectCommand = @"SELECT MainAccount_ReferenceID,MainAccount_Name,MainAccount_SubLedgerType,MainAccount_ReverseApplicable,TAXable,MainAccount_AccountCode FROM v_MainAccountList_journal WHERE (MainAccount_ReferenceID = @ID) ";

            SqlDataSourceMainAccount.SelectParameters.Clear();
            SqlDataSourceMainAccount.SelectParameters.Add("ID", TypeCode.String, e.Value.ToString());
            comboBox.DataSource = SqlDataSourceMainAccount;
            comboBox.DataBind();
        }

        public DataTable GetMainAccountTableNew(string strBranchID, string filter, int startindex, int EndIndex, string strCompanyID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_JournalVoucherDetails");
            proc.AddVarcharPara("@Action", 100, "GetMainAccountList");
            proc.AddVarcharPara("@CompanyID", 500, strCompanyID);
            proc.AddVarcharPara("@filter", 100, filter);
            proc.AddIntegerPara("@startIndex", startindex);
            proc.AddIntegerPara("@endIndex", EndIndex);
            proc.AddVarcharPara("@BranchID", 100, strBranchID);
            dt = proc.GetTable();
            return dt;
        }


        protected void ASPxComboBox_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            if (e.Filter != "")
            {
                ASPxComboBox comboBox = (ASPxComboBox)source;
                DataTable dt = new DataTable();
                string filter = "%" + Convert.ToString(e.Filter) + "%";
                int startindex = Convert.ToInt32(e.BeginIndex + 1);
                int EndIndex = Convert.ToInt32(e.EndIndex + 1);
                string MainAccountID = hdnMainAccountId.Value;

                dt = GetSubAccountTableNew(MainAccountID, filter, startindex, EndIndex);
                comboBox.DataSource = dt;
                comboBox.DataBind();
            }
        }
        public DataTable GetSubAccountTableNew(string strMainAccount, string filter, int startindex, int EndIndex)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_JournalVoucherDetails");
            proc.AddVarcharPara("@Action", 100, "GetSubAccountListBYMainAccount");
            proc.AddVarcharPara("@MainAccountID", 500, strMainAccount);
            proc.AddVarcharPara("@filter", 100, filter);
            proc.AddIntegerPara("@startIndex", startindex);
            proc.AddIntegerPara("@endIndex", EndIndex);
            dt = proc.GetTable();
            return dt;
        }
        protected void ASPxComboBox_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            long value = 0;
            if (e.Value == null || !Int64.TryParse(e.Value.ToString(), out value))
                return;
            ASPxComboBox comboBox = (ASPxComboBox)source;
            SqlDataSourceSubAccount.SelectCommand = @"SELECT SubAccount_ReferenceID,Contact_Name,MainAccount_SubLedgerType,mainaccount_referenceid FROM v_SubAccountList WHERE (SubAccount_ReferenceID = @ID) ";

            SqlDataSourceSubAccount.SelectParameters.Clear();
            SqlDataSourceSubAccount.SelectParameters.Add("ID", TypeCode.String, e.Value.ToString());
            comboBox.DataSource = SqlDataSourceSubAccount;
            comboBox.DataBind();
        }

        #endregion

        protected void grid_RowValidating(object sender, ASPxDataValidationEventArgs e)
        {

            string IsSubledger = "";
            string SubAccount = "";
            string MainAccount = "";

            foreach (GridViewColumn column in grid.Columns)
            {
                GridViewDataColumn dataColumn = column as GridViewDataColumn;
                if (dataColumn == null) continue;
                if (e.NewValues[dataColumn.FieldName] != null && dataColumn.FieldName == "IsSubledger")
                    IsSubledger = Convert.ToString(e.NewValues[dataColumn.FieldName]);
                if (e.NewValues[dataColumn.FieldName] != null && dataColumn.FieldName == "bthSubAccount")
                    SubAccount = Convert.ToString(e.NewValues[dataColumn.FieldName]);
                if (e.NewValues[dataColumn.FieldName] != null && dataColumn.FieldName == "MainAccount")
                    MainAccount = Convert.ToString(e.NewValues[dataColumn.FieldName]);
                //e.Errors[dataColumn] = "Value cannot be null.";
            }

            if (HiddenSubMandatory.Value == "Yes" && IsSubledger != "" && IsSubledger != null && IsSubledger != "None" && string.IsNullOrEmpty(SubAccount) && !string.IsNullOrEmpty(MainAccount))
            {
                AddError(e.Errors, grid.Columns["bthSubAccount"], "Sub ledger is set as mandatory.");
            }
        }

        void AddError(Dictionary<GridViewColumn, string> errors, GridViewColumn column, string errorText)
        {
            if (errors.ContainsKey(column)) return;
            errors[column] = errorText;
        }
    }
}