using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using DevExpress.Web;
using BusinessLogicLayer;
using System.Globalization;
using System.Web.Services;
using System.Collections;
using System.Linq;
using DevExpress.Web.Data;
using EntityLayer.CommonELS;
public partial class management_DailyTask_journalvoucherentry : ERP.OMS.ViewState_class.VSPage
{
    #region Local_Varibal
    BusinessLogicLayer.OtherTasks oOtherTasks = new BusinessLogicLayer.OtherTasks();
    BusinessLogicLayer.DailyTaskOther oDailyTaskOther = new BusinessLogicLayer.DailyTaskOther();


  //  BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

    BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
    public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
    DataTable DtTable = new DataTable();
    DataTable dtReport = new DataTable();
    DataTable TempDt = new DataTable();
    static int ReptID = 0;
    static int ID = 0;
    static string ForJournalDate = null;
    string JournalEntryID = null;
    int NoofRowsAffected = 0;
    string CurrentSegment;
    string CompanyID;
    Converter oconverter = new Converter();
    Dictionary<string, object> values = new Dictionary<string, object>();
    Boolean Lock = false;
    DataSet DsSearchCallBack;
    SqlDataAdapter DaSearchCallBack;
    string JournalVoucherFile_XMLPATH = null;
    string JournalVoucherFile_VALIDATEXMLPATH = null;
    string JVNumStr = string.Empty;
    //This For Log Purpose
    GenericLogSystem oGenericLogSystem;
    #endregion
    #region Page Property
    public int PCounter
    {
        get { return (int)ViewState["Counter"]; }
        set { ViewState["Counter"] = value; }
    }
    public int PBranch
    {
        get { return (int)ViewState["BranchCounter"]; }
        set { ViewState["BranchCounter"] = value; }
    }
    public int PNonBranch
    {
        get { return (int)ViewState["NonBranchCounter"]; }
        set { ViewState["NonBranchCounter"] = value; }
    }
    public decimal TotalDebit
    {
        get { return (decimal)ViewState["TotalDebit"]; }
        set { ViewState["TotalDebit"] = value; }
    }
    public decimal TotalCredit
    {
        get { return (decimal)ViewState["TotalCredit"]; }
        set { ViewState["TotalCredit"] = value; }
    }
    //Currency Setting
    public string ChoosenCurrency
    {
        get { return (string)Session["ChoosenCurrency"]; }
        set { Session["ChoosenCurrency"] = value; }
    }
    //This For Log Purpose
    public string LogID
    {
        get { return (string)ViewState["LogID"]; }
        set { ViewState["LogID"] = value; }
    }
    #endregion
    #region PageClass
    void BindGrid(ASPxGridView Grid)
    {
        TotalDebit = 0;
        TotalCredit = 0;
        Grid.DataSource = null;
        Grid.DataBind();
    }
    void BindGrid(ASPxGridView Grid, DataSet Ds)
    {
        TotalCredit = 0;
        TotalDebit = 0;
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
        TotalCredit = 0;
        TotalDebit = 0;
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
    void BindGrid(ASPxGridView Grid, DataSet Ds, String WhichSort)
    {
        TotalCredit = 0;
        TotalDebit = 0;
        DataView TempDV = new DataView(Ds.Tables[0]);
        TempDV.Sort = "EntryDateTime " + WhichSort;
        Grid.DataSource = TempDV;
        Grid.DataBind();
    }
    void BindGrid(ASPxGridView Grid, DataTable Dt, String WhichSort)
    {
        TotalCredit = 0;
        TotalDebit = 0;
        DataView TempDV = new DataView(Dt);
        TempDV.Sort = "EntryDateTime " + WhichSort;
        Grid.DataSource = TempDV;
        Grid.DataBind();
    }
    bool IsNumeric(string value)
    {
        try { Convert.ToInt32(value); return true; }
        catch { return false; }
    }
    bool IsRecordAreadyInserted(string MainAccount, string SubAccountCode, string BranchNonBranch)
    {
        if (File.Exists(Server.MapPath(JournalVoucherFile_XMLPATH)))
        {
            DataSet Ds_RecordExists = new DataSet();
            Ds_RecordExists.ReadXml(Server.MapPath(JournalVoucherFile_XMLPATH));
            DataColumn[] colPk = new DataColumn[2];
            colPk[0] = Ds_RecordExists.Tables[0].Columns["JournalVoucherDetail_MainAccountCode"];
            colPk[1] = Ds_RecordExists.Tables[0].Columns["JournalVoucherDetail_SubAccountCode"];
            Ds_RecordExists.Tables[0].PrimaryKey = colPk;
            object[] findTheseVals = new object[2];
            findTheseVals[0] = MainAccount;
            if (!IsNumeric(BranchNonBranch))
                findTheseVals[1] = SubAccountCode.Split('~')[0];
            else
                findTheseVals[1] = SubAccountCode.Substring(0, SubAccountCode.LastIndexOf('~'));
            DataRow foundRow = Ds_RecordExists.Tables[0].Rows.Find(findTheseVals);
            if (foundRow != null)
                return true;
            else
                return false;
        }
        return false;
    }
    void FillSearchGrid()
    {
        hdnMode.Value = "1"; //Edit
        DaSearchCallBack = new SqlDataAdapter();
        DsSearchCallBack = new DataSet();
        //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))


        using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
        {
            string ss = Convert.ToString(Session["LastFinYear"]);
            string sss = Convert.ToString(ViewState["WhichSegment"]);
            string branchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);

            using (SqlCommand com = new SqlCommand("Search_JournalVoucher", con))
            {
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@FinYear", Convert.ToString(Session["LastFinYear"]));
                com.Parameters.AddWithValue("@CompanyID", CompanyID);
                com.Parameters.AddWithValue("@ExchSegmentID", Convert.ToString(ViewState["WhichSegment"]).Trim());
                com.Parameters.AddWithValue("@BranchID", branchHierarchy);

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
        if (DsSearchCallBack.Tables.Count > 0)
        {
            if (DsSearchCallBack.Tables[0].Rows.Count > 0)
            {
                BindGrid(GvJvSearch, DsSearchCallBack.Tables[0]);
            }
            else
            {
                BindGrid(GvJvSearch);
            }
        }
        else
        {
            BindGrid(GvJvSearch);
        }
    }
    void CreateJVE_XMLFile(string IBRef, string VoucherNumber)
    {
        DataSet Ds_JVE;
        SqlDataAdapter Da_JVE;
        string JVEFile_XMLPATH = "../Documents/" + "JVE_" + IBRef;
        if (File.Exists(Server.MapPath(JVEFile_XMLPATH)))
        {
            Ds_JVE = new DataSet();
            Ds_JVE.ReadXml(Server.MapPath(JVEFile_XMLPATH));
            string UserID = Convert.ToString(Ds_JVE.Tables[0].Rows[0]["UserID"]);
            string[] UserName = oDBEngine.GetFieldValue1("tbl_master_user", "User_Name", "user_id=" + UserID, 1);
            ViewState["JVE_FileAlreadyUsedBy"] = UserName[0] + "~" + UserID;
        }
        else
        {
            Ds_JVE = new DataSet();
          //  using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))

            using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))

            {
                using (SqlCommand com = new SqlCommand("Fetch_JVE_DataSet", con))
                {
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@CrgExchange", Convert.ToString(ViewState["SegmentName"]));
                    com.Parameters.AddWithValue("@UserID", Convert.ToString(Session["userid"]));
                    com.Parameters.AddWithValue("@VoucherNumber", VoucherNumber);
                    com.Parameters.AddWithValue("@IBRef", IBRef);
                    com.Parameters.AddWithValue("@TradeCurrency", Convert.ToString(Session["TradeCurrency"]).Split('~')[0]);
                    com.Parameters.AddWithValue("@CompanyID", Convert.ToString(Session["LastCompany"]));

                    using (Da_JVE = new SqlDataAdapter(com))
                    {
                        Ds_JVE.Clear();
                        Da_JVE.Fill(Ds_JVE);
                    }
                }
            }
            if (Ds_JVE.Tables.Count > 0)
            {
                if (Ds_JVE.Tables[0].Rows.Count > 0)
                {
                    Ds_JVE.Tables[0].TableName = "DtJournalVoucher";
                    Ds_JVE.WriteXml(Server.MapPath(JVEFile_XMLPATH));
                }
            }
        }
    }

    #endregion
    protected void Page_PreInit(object senduserbranchHierarchyer, EventArgs e)
    {

        if (!IsPostBack)
        {
            //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
            string sPath = Convert.ToString(HttpContext.Current.Request.Url);
            oDBEngine.Call_CheckPageaccessebility(sPath);
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {



        dsCompany.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        dsBranch.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        SqlSchematype.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);


        rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/DailyTask/JournalVoucherEntry.aspx");
        oGenericLogSystem = new GenericLogSystem();
        if (HttpContext.Current.Session["userid"] == null)
        {
            //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
        }


        string[] segmentname = oDBEngine.GetFieldValue1("tbl_master_segment", "Seg_Name", "Seg_id=" + HttpContext.Current.Session["userlastsegment"], 1);
        ViewState["SegmentName"] = segmentname[0];

        hdn_SegID_SegmentName.Value = segmentname[0];

        //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
        ViewState["DebitWhenEdit"] = null;
        ViewState["CreditWhenEdit"] = null;
        string UserPageAccess = Convert.ToString(HttpContext.Current.Session["PageAccess"]);
        CurrentSegment = Convert.ToString(HttpContext.Current.Session["userlastsegment"]);

        hdnSegmentid.Value = CurrentSegment;

        CompanyID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
        if (!IsPostBack)
        {
            string strdefaultBranch = Convert.ToString(Session["userbranchID"]);
            ddlBranch.SelectedValue = strdefaultBranch;

            //string Originalpath = HttpContext.Current.Request.Url.AbsolutePath;
            //string path = Originalpath.Replace("/OMS/", "/");
            //DataTable dtPath = oDBEngine.GetDataTable("tbl_trans_menu", "ltrim(rtrim(mnu_menuName))", " mnu_menuLink='" + Convert.ToString(path).Trim() + "'");
            //if (dtPath != null && dtPath.Rows.Count > 0)
            //{
            //    this.Title = Convert.ToString(dtPath.Rows[0][0]);
            //}
            //else
            //{
            //    this.Title = "Welcome to BreezeERP";
            //}

            //Intializing Variable
            Session["exportval"] = null;
            TotalDebit = 0;
            TotalCredit = 0;
            if (Session["StrQuery"] != null) Session["StrQuery"] = null;
            //This For Log Purpose
            ViewState["LogID"] = oGenericLogSystem.GetLogID();

            //Currency Setting
            this.Page.ClientScript.RegisterStartupScript(GetType(), "CS", "<script>PageLoad_ForCurrency();</script>");

            //End Intializing Variable

            tDate.EditFormatString = objConverter.GetDateFormat("Date");
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

            tDate.Value = DateTime.ParseExact(fDate, @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            if (Request.QueryString["id"] != null)
            {
                dtReport.Clear();
                ReptID = 0;
                ID = 0;
                BindGridForEdit();
                Page.ClientScript.RegisterStartupScript(GetType(), "JSc", "<script language='javascript'>Narration_Off()</script>");
            }
            else
            {
                dtReport.Clear();
                ReptID = 0;
                ID = 0;
                txtAccountCode.Text = "JV";
                Session["KeyVal"] = "0";

                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load()</script>");
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
            }

            //Currency Setting
            B_ChoosenCurrency.InnerText = "Voucher Currency : " + Convert.ToString(Session["ActiveCurrency"]).Split('~')[1].Trim() + "[" +
                   Convert.ToString(Session["ActiveCurrency"]).Split('~')[2].Trim() + "]";
            if (!CbpChoosenCurrency.IsCallback)
            {
                if (Convert.ToString(Session["LocalCurrency"]).Trim() != Convert.ToString(Session["TradeCurrency"]).Trim())
                {
                    B_ChoosenCurrency.Attributes.Add("onclick", "ChangeCurrency()");
                    B_ChoosenCurrency.Style.Add("cursor", "hand");
                }
            }
        }

        if (Session["StrQuery"] != null) FillSearchGrid();
        Page.ClientScript.RegisterStartupScript(GetType(), "JS", "<script language='javascript'>Page_Load1()</script>");
        hdnCompanyid.Value = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
        //if (ComboMode.SelectedIndex == 0)
        if (hdnMode.Value == "0")
        {
            //Set XML File Path
            JournalVoucherFile_XMLPATH = "../Documents/" + "JV_" + Convert.ToString(Session["userid"]) + "_" + txtAccountCode.Text.Trim() + "_" + Convert.ToString(ViewState["WhichSegment"]).Trim() + "_" + Convert.ToDateTime(tDate.Value).ToString("dd/MM/yyyy").Replace("/", "");
            JournalVoucherFile_VALIDATEXMLPATH = "../Documents/" + "JV_" + Convert.ToString(Session["userid"]) + "_" + txtAccountCode.Text.Trim() + "_" + Convert.ToString(ViewState["WhichSegment"]).Trim() + "_" + Convert.ToDateTime(tDate.Value).ToString("dd/MM/yyyy").Replace("/", "") + "Validate";


        }
        else
        {
            if (Session["IBRef"] != null)
            {
                JournalVoucherFile_XMLPATH = "../Documents/" + "JVE_" + Convert.ToString(Session["IBRef"]);
                JournalVoucherFile_VALIDATEXMLPATH = "../Documents/" + "JVE_" + Convert.ToString(Session["IBRef"]) + "Validate";
            }
        }
        Session["JournalVoucherFile_XMLPATH"] = JournalVoucherFile_XMLPATH;
        OnLoadBindGrid();
        FillSearchGrid();
    }
    DataSet Bind_Combo(string strcmd, string parametername, string parametervalue)
    {
        DataSet Ds = new DataSet();
      //  using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))


        using (SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
        {
            using (SqlCommand com = new SqlCommand(strcmd, con))
            {
                com.Parameters.AddWithValue(parametername, parametervalue);
                using (SqlDataAdapter Da = new SqlDataAdapter(com))
                {
                    Da.Fill(Ds);
                }
            }
        }
        return Ds;
    }
    public void OnLoadBindGrid()
    {

        DataSet DsOnLoad = new DataSet();
        DataSet DsOnLoadValidation = new DataSet();
        if (File.Exists(Server.MapPath(JournalVoucherFile_XMLPATH)))
        {
            if (DsOnLoad.Tables.Count > 0) { DsOnLoad.Tables.Remove(DsOnLoad.Tables[0]); DsOnLoad.Clear(); }
            DsOnLoad.ReadXml(Server.MapPath(JournalVoucherFile_XMLPATH));
            BindGrid(DetailsGrid, DsOnLoad, "DESC");
            if (DsOnLoad.Tables.Count > 0)
            {
                ViewState["BillNo"] = Convert.ToString(DsOnLoad.Tables[0].Rows[0][22]);
                ViewState["JvNarration"] = Convert.ToString(DsOnLoad.Tables[0].Rows[0][8]);
                ViewState["BranchSelectedValue"] = Convert.ToString(DsOnLoad.Tables[0].Rows[0][23]);
                ViewState["Prefix"] = Convert.ToString(DsOnLoad.Tables[0].Rows[0][19]);
                //Currency Setting
                DetailsGrid.JSProperties["cpSetCurrencyNameSymbol"] = null;
                ChoosenCurrency = Convert.ToString(DsOnLoad.Tables[0].Rows[0]["ChoosenCurrency"]);
                DetailsGrid.JSProperties["cpSetCurrencyNameSymbol"] = Convert.ToString(ChoosenCurrency).Split('~')[1].Trim() + "~" +
                    ChoosenCurrency.Split('~')[2].Trim();
            }
        }
        else
        {
            BindGrid(DetailsGrid);
            ViewState["BillNo"] = null;
            ViewState["JvNarration"] = null;
            ViewState["BranchSelectedValue"] = null;
            ViewState["Prefix"] = null;
            //Currency Setting
            ChoosenCurrency = Convert.ToString(Session["ActiveCurrency"]);
        }
        if (File.Exists(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH)))
        {
            if (DsOnLoadValidation.Tables.Count > 0) { DsOnLoadValidation.Tables.Remove(DsOnLoad.Tables[0]); DsOnLoadValidation.Clear(); }
            DsOnLoadValidation.ReadXml(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
            if (DsOnLoadValidation.Tables.Count > 0)
            {
                if (DsOnLoadValidation.Tables[0].Columns.Count > 3)
                {
                    File.Delete(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
                    foreach (DataRow row in DsOnLoad.Tables[0].Rows)
                        AddData_EntryValidationXML(Convert.ToString(row["BranchNonBranch"]));
                }
            }

        }
        else
        {
            if (DsOnLoad.Tables.Count > 0)
            {
                if (DsOnLoad.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in DsOnLoad.Tables[0].Rows)
                        AddData_EntryValidationXML(Convert.ToString(row["BranchNonBranch"]));
                }
            }
        }

        DsOnLoad.Dispose();
    }
    public void BindGridForEdit()
    {
        Session["KeyVal"] = Convert.ToString(Request.QueryString["id"]);
        dtReport = oDBEngine.GetDataTable("Trans_JournalVoucherDetail", "cast(journalvoucherdetail_ID as int) as CashReportID,journalvoucherdetail_VoucherNumber as journalvoucherdetail_ID,ltrim(rtrim(JournalVoucherDetail_ExchangeSegmentID)) as JournalVoucherDetail_ExchangeSegmentID,ltrim(rtrim(JournalVoucherDetail_BranchID)) as JournalVoucherDetail_BranchID,(select MainAccount_ReferenceID from master_mainaccount where MainAccount_AccountCode=Trans_JournalVoucherDetail.JournalVoucherDetail_MainAccountCode) as JournalVoucherDetail_MainAccountCode,JournalVoucherDetail_SubAccountCode,cast(ltrim(rtrim(JournalVoucherDetail_AmountDr)) as decimal(20,5)) as JournalVoucherDetail_AmountDr,cast(ltrim(rtrim(JournalVoucherDetail_AmountCr)) as decimal(20,5)) as JournalVoucherDetail_AmountCr,ltrim(rtrim(JournalVoucherDetail_Narration)) as JournalVoucherDetail_Narration,(isnull((select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'')+' ['+isnull(ltrim(rtrim(cnt_ucc)),'')+']' from tbl_master_contact where cnt_internalid=Trans_JournalVoucherDetail.JournalVoucherDetail_SubAccountCode),isnull((select top 1 subaccount_name+' ['+isnull(ltrim(rtrim(subaccount_code)),'')+']' from master_subaccount where cast(subaccount_code as varchar)=Trans_JournalVoucherDetail.JournalVoucherDetail_SubAccountCode and SubAccount_MainAcReferenceID=Trans_JournalVoucherDetail.JournalVoucherDetail_MainAccountCode),isnull((select subaccount_name+' ['+isnull(ltrim(rtrim(subaccount_code)),'')+']' from master_subaccount where cast(subaccount_referenceid as varchar)=Trans_JournalVoucherDetail.JournalVoucherDetail_SubAccountCode),isnull((select top 1 cdslclients_firstholdername+'['+isnull(ltrim(rtrim(cdslclients_benaccountnumber)),'')+']' from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=Trans_JournalVoucherDetail.JournalVoucherDetail_SubAccountCode),(select nsdlclients_benfirstholdername+' ['+isnull(ltrim(rtrim(nsdlclients_benaccountid)),'')+']' from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=Trans_JournalVoucherDetail.JournalVoucherDetail_SubAccountCode)))))) as SubAccount1,isnull((select MainAccount_Name from Master_MainAccount where cast(MainAccount_ReferenceId as varchar)=cast(Trans_JournalVoucherDetail.JournalVoucherDetail_MainAccountCode as varchar)),isnull((select MainAccount_Name from master_mainaccount where mainaccount_accountcode=Trans_JournalVoucherDetail.JournalVoucherDetail_MainAccountCode),'')) as MainAccount1,case JournalVoucherDetail_AmountDr when '0.0000' then null else convert(varchar(50),cast(JournalVoucherDetail_AmountDr as money),1) end as WithDrawl,case JournalVoucherDetail_AmountCr when '0.0000' then null else convert(varchar(50),cast(JournalVoucherDetail_AmountCr as money),1) end as Receipt,cast(ltrim(rtrim(JournalVoucherDetail_ValueDate)) as datetime) as JournalVoucherDetail_ValueDate,convert(varchar(12),JournalVoucherDetail_ValueDate,113) as JournalVoucherDetail_ValueDate1,isnull((select MainAccount_SubLedgerType from Master_MainAccount where cast(MainAccount_AccountCode as varchar)=cast(Trans_JournalVoucherDetail.JournalVoucherDetail_MainAccountCode as varchar)),'Systm') as Type," + Session["userid"].ToString() + " as UserID", " JournalVoucherDetail_VoucherNumber='" + Request.QueryString["id"].ToString() + "' and JournalVoucherDetail_ExchangeSegmentID=" + Request.QueryString["exch"].ToString().Trim() + " and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,JournalVoucherDetail_TransactionDate)) as datetime)=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + Request.QueryString["date"].ToString().Trim() + "')) as datetime)", " JournalVoucherDetail_SubAccountCode desc");
        if (dtReport.Rows.Count > 0)
        {
            ReptID = Convert.ToInt32(dtReport.Compute("max(CashReportID)", ""));
            string gridForVoucherID = Convert.ToString(dtReport.Rows[0][1]).Substring(0, 2);
            if (gridForVoucherID == "YF")
            {
                DetailsGrid.Enabled = false;

            }
        }
        DataTable dtEdit = oDBEngine.GetDataTable("trans_journalvoucher", "ltrim(rtrim(journalvoucher_companyid)) as journalvoucher_companyid,ltrim(rtrim(journalvoucher_ExchangeSegmentID)) as journalvoucher_ExchangeSegmentID,ltrim(rtrim(journalvoucher_BranchID)) as journalvoucher_BranchID,ltrim(rtrim(journalvoucher_SettlementNumber)) as journalvoucher_SettlementNumber,ltrim(rtrim(journalvoucher_BillNumber)) as journalvoucher_BillNumber,cast(journalvoucher_TransactionDate as datetime) as journalvoucher_TransactionDate,journalvoucher_Prefix,ltrim(rtrim(journalvoucher_Narration)) as journalvoucher_Narration,ltrim(rtrim(journalvoucher_VoucherNumber)) as journalvoucher_VoucherNumber,(select isnull(rtrim(settlements_Number),'')+ ' [ ' + isnull(rtrim(settlements_typeSuffix),'') + ' ]' as SettlementName from Master_Settlements where settlements_ID=trans_journalvoucher.journalvoucher_SettlementNumber) as SettNumber ", " journalvoucher_vouchernumber='" + dtReport.Rows[0][1].ToString() + "' and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,JournalVoucher_TransactionDate)) as datetime)=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + Request.QueryString["date"].ToString().Trim() + "')) as datetime) and journalvoucher_exchangesegmentid=" + dtReport.Rows[0][2].ToString() + "");
        if (dtEdit.Rows.Count > 0)
        {
            ViewState["WhichCompany"] = Convert.ToString(dtEdit.Rows[0][0]);
            hdnCompanyid.Value = Convert.ToString(ViewState["WhichCompany"]);
            DataTable dtExch = oDBEngine.GetDataTable("(SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + Convert.ToString(dtEdit.Rows[0][0]) + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID ", "A.EXCH_INTERNALID AS SEGMENTID ,isnull((TME.EXH_ShortName + '-' + A.EXCH_SEGMENTID),exch_membershipType) AS EXCHANGENAME ", null);
            ddlBranch.DataBind();
            ddlBranch.SelectedValue = Convert.ToString(dtEdit.Rows[0][2]);
            tDate.Value = Convert.ToString(Convert.ToDateTime(dtEdit.Rows[0][5]));
            txtAccountCode.Text = Convert.ToString(dtEdit.Rows[0][6]);
            txtNarration.Text = Convert.ToString(dtEdit.Rows[0][7]);
            txtBillNo.Text = Convert.ToString(dtEdit.Rows[0][4]);
            ddlBranch.Enabled = false;
            txtAccountCode.Enabled = false;

        }
        ViewState["mytable"] = dtReport;
        hddnEdit.Value = "Edit";
        BindGrid(DetailsGrid, dtReport, "DESC");

    }
    protected void grdAdd_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {

    }

    [System.Web.Services.WebMethod]
    public static string GetContactName(string custid)
    {
        string closingBalance = null;
        management_DailyTask_journalvoucherentry objPage = new management_DailyTask_journalvoucherentry();

       // BusinessLogicLayer.DBEngine oDbEngine1 = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDbEngine1 = new BusinessLogicLayer.DBEngine();


        BusinessLogicLayer.Converter objConverter1 = new BusinessLogicLayer.Converter();
        string[] dateandparam = custid.Split('~');
        // DateTime date = Convert.ToDateTime(objConverter1.DateConverter1(dateandparam[2].ToString(), "mm/dd/yyyy"));

        DateTime date = Convert.ToDateTime(ForJournalDate);

        string[,] mainacc = oDbEngine1.GetFieldValue("master_mainaccount", "MainAccount_Accountcode", "mainaccount_referenceid=" + dateandparam[3] + "", 1);
        string mainaccCode = mainacc[0, 0];
        mainaccCode = "'" + mainaccCode + "'";
        string SubAccID = dateandparam[4];
        DataTable dtClose = oDbEngine1.OpeningBalanceJournal1(mainaccCode, SubAccID, date, dateandparam[1], dateandparam[0], date, Convert.ToInt32(objPage.ChoosenCurrency.Split('~')[0]));
        closingBalance = Convert.ToString(dtClose.Rows[0][0]);
        return closingBalance;
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        DataSet dsCrystal = new DataSet();
        DataSet DSJournalReturn = new DataSet();
        DSJournalReturn = (DataSet)Session["DSJournalReturn"];
        DataTable DtJournalReturn = DSJournalReturn.Tables[0];
        DateTime TranDate = Convert.ToDateTime(Convert.ToString(DtJournalReturn.Rows[0][3])).Date;
        string tabledata = objConverter.ConvertDataTableToXML(DtJournalReturn);

        dsCrystal = oDailyTaskOther.JournalVoucherPrintFromInsert(
         Convert.ToString(tabledata),
         Convert.ToString(TranDate)
           );
        //String conn = ConfigurationManager.AppSettings["DBConnectionDefault"];
        //SqlConnection con = new SqlConnection(conn);
        //SqlCommand cmd3 = new SqlCommand("JournalVoucherPrintFromInsert", con);
        //cmd3.CommandType = CommandType.StoredProcedure;
        //cmd3.Parameters.AddWithValue("@journalData", tabledata);
        //cmd3.Parameters.AddWithValue("@TransactionDate", TranDate);
        //cmd3.CommandTimeout = 0;
        //SqlDataAdapter Adap = new SqlDataAdapter();
        //Adap.SelectCommand = cmd3;
        //Adap.Fill(dsCrystal);
        //cmd3.Dispose();
        //con.Dispose();
        //GC.Collect();

        dsCrystal.WriteXml(ConfigurationManager.AppSettings["SaveCSVsql"] + "//Reports//Journal.xsd");



      //  string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
        string[] connPath = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]).Split(';');


        ReportDocument reportObj = new ReportDocument();
        string ReportPath = Server.MapPath("..\\Reports\\Journal.rpt");
        reportObj.Load(ReportPath);
        reportObj.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
        reportObj.SetDataSource(dsCrystal);
        //reportObj.Subreports["logo"].SetDataSource(dsCrystal.Tables[0]);
        reportObj.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "Journal");
        reportObj.Dispose();
        GC.Collect();
        Session["DSJournalReturn"] = null;
    }
    protected void DetailsGrid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
    {
        decimal diff = 0.00M; ;
        string RemainingCredit = "0.00";
        string RemainingDebit = "0.00";
        if (ViewState["CreditWhenEdit"] != null)
        {
            if (Convert.ToString(ViewState["CreditWhenEdit"]).Trim() != "0.00")
            {
                TotalCredit = Convert.ToDecimal(Convert.ToString(ViewState["CreditWhenEdit"])) + TotalCredit;
                TotalCredit = TotalCredit > 0 ? TotalCredit / 2 : TotalCredit;
                TotalDebit = TotalDebit > 0 ? TotalDebit / 2 : TotalDebit;
            }
        }


        if (ViewState["DebitWhenEdit"] != null)
        {
            if (Convert.ToString(ViewState["DebitWhenEdit"]).Trim() != "0.00")
            {
                TotalDebit = Convert.ToDecimal(Convert.ToString(ViewState["DebitWhenEdit"])) + TotalDebit;
                TotalCredit = TotalCredit > 0 ? TotalCredit / 2 : TotalCredit;
                TotalDebit = TotalDebit > 0 ? TotalDebit / 2 : TotalDebit;
            }
        }

        if (ViewState["DebitWhenEdit"] == null && ViewState["CreditWhenEdit"] == null)
        {
            TotalCredit = TotalCredit > 0 ? TotalCredit / 2 : TotalCredit;
            TotalDebit = TotalDebit > 0 ? TotalDebit / 2 : TotalDebit;
        }


        if (TotalDebit < TotalCredit)
        {
            diff = TotalCredit - TotalDebit;
            RemainingCredit = "0.00";
            RemainingDebit = Convert.ToString(diff); ;
            tdSaveButton.Visible = false;
        }
        else if (TotalDebit > TotalCredit)
        {
            diff = TotalDebit - TotalCredit;
            RemainingCredit = Convert.ToString(diff);
            RemainingDebit = "0.00";
            tdSaveButton.Visible = false;
        }
        else
        {
            RemainingCredit = "0.00";
            RemainingDebit = "0.00";
            tdSaveButton.Visible = true;
        }
        ViewState["CreditWhenEdit"] = "0.00";
        ViewState["DebitWhenEdit"] = "0.00";
        e.Properties["cpTotalDebitCredit"] = TotalDebit + "~" + TotalCredit + "~" + RemainingDebit + "~" + RemainingCredit;

    }
    protected void DetailsGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        string strSplitCommand = e.Parameters.Split('~')[0];
        if (strSplitCommand == "Add")
        {
            string strSplitBranch = e.Parameters.Split('~')[1];
            //if (!IsRecordAreadyInserted(txtMainAccount_hidden.Value, txtSubAccount_hidden.Value, strSplitBranch))
            //{
            Boolean AllowDisAllow = Convert.ToBoolean(AllowDisAllowEntry(strSplitBranch).Split('~')[0]);
            if (AllowDisAllow)
            {
                try
                {
                    AddData_ToGrid(strSplitBranch);
                    AddData_EntryValidationXML(strSplitBranch);
                    //This For Log Purpose
                    string strLogID = Convert.ToString(ViewState["LogID"]);
                    // oGenericLogSystem.CreateLog("", "", BusinessLogicLayer.GenericLogSystem.LogState.XmlAdd, Session["UserID"].ToString(), "", "Trans_JournalVoucher", strLogID, GenericLogSystem.LogType.JV);
                    // oGenericLogSystem.CreateLog("", "", BusinessLogicLayer.GenericLogSystem.LogState.XmlAdd, Session["UserID"].ToString(), "", "Trans_JournalVoucherDetail", strLogID, GenericLogSystem.LogType.JV);
                }
                catch
                {
                    //This For Log Purpose
                    string strLogID = Convert.ToString(ViewState["LogID"]);
                    // oGenericLogSystem.CreateLog("", "",BusinessLogicLayer.GenericLogSystem.LogState.XmlError, Session["UserID"].ToString(), "", "Trans_JournalVoucher", strLogID, GenericLogSystem.LogType.JV);
                    //oGenericLogSystem.CreateLog("", "", BusinessLogicLayer.GenericLogSystem.LogState.XmlError, Session["UserID"].ToString(), "", "Trans_JournalVoucherDetail", strLogID, GenericLogSystem.LogType.JV);
                }

                DetailsGrid.JSProperties["cpEntryNotAllow"] = "Empty";
                DetailsGrid.JSProperties["cpSaveSuccessOrFail"] = "undefined";
                DetailsGrid.JSProperties["cpSuccessDiscard"] = "undefined";
                DetailsGrid.JSProperties["cpEntryData"] = "undefined";
            }
            else
            {
                string Reason = AllowDisAllowEntry(strSplitBranch).Split('~')[1];
                if (Reason == "Branch") DetailsGrid.JSProperties["cpEntryNotAllow"] = "You Can Not Select More Than One Non-Branch Accounts For This Entry";
                else DetailsGrid.JSProperties["cpEntryNotAllow"] = "You Can Not Select More Than One Branch Accounts For This Entry";
                DetailsGrid.JSProperties["cpSaveSuccessOrFail"] = "undefined";
                DetailsGrid.JSProperties["cpSuccessDiscard"] = "undefined";
                DetailsGrid.JSProperties["cpEntryData"] = "undefined";
            }
            //}
            //else
            //{
            //    DetailsGrid.JSProperties["cpEntryNotAllow"] = "Record Already Exist.";
            //}
        }
        if (strSplitCommand == "Save")
        {
            DetailsGrid.JSProperties["cpSaveSuccessOrFail"] = Save_Records();
            DetailsGrid.JSProperties["cpEntryNotAllow"] = "undefined";
            DetailsGrid.JSProperties["cpSuccessDiscard"] = "undefined";
            DetailsGrid.JSProperties["cpEntryData"] = "undefined";
        }
        if (strSplitCommand == "Discard")
        {
            DetailsGrid.JSProperties["cpSuccessDiscard"] = Discard_All();
            DetailsGrid.JSProperties["cpSaveSuccessOrFail"] = "undefined";
            DetailsGrid.JSProperties["cpEntryNotAllow"] = "undefined";
            DetailsGrid.JSProperties["cpEntryData"] = "undefined";
        }
        if (strSplitCommand == "New")
        {

        }
        if (strSplitCommand == "Entry")
        {
            EntryButtonClick();
            DetailsGrid.JSProperties["cpSaveSuccessOrFail"] = "undefined";
            DetailsGrid.JSProperties["cpEntryNotAllow"] = "undefined";
            DetailsGrid.JSProperties["cpSuccessDiscard"] = "undefined";
            DetailsGrid.JSProperties["cpDrCrAfterAdd"] = "undefined";
            DetailsGrid.JSProperties["cpEntryData"] = "EntryData";
            DetailsGrid.JSProperties["cpBillNo"] = ViewState["BillNo"] != null ? Convert.ToString(ViewState["BillNo"]) : "EmptyString";
            DetailsGrid.JSProperties["cpJvNarration"] = ViewState["JvNarration"] != null ? Convert.ToString(ViewState["JvNarration"]) : "EmptyString";
            DetailsGrid.JSProperties["cpBranchSelectedValue"] = ViewState["BranchSelectedValue"] != null ? Convert.ToString(ViewState["BranchSelectedValue"]) : "EmptyString";
            DetailsGrid.JSProperties["cpPrefix"] = ViewState["Prefix"] != null ? Convert.ToString(ViewState["Prefix"]) : "EmptyString";
        }
        if (strSplitCommand == "DiscardOnRefresh")
        {
            ViewState["BillNo"] = null;
            ViewState["JvNarration"] = null;
            ViewState["BranchSelectedValue"] = null;
            ViewState["Prefix"] = null;
            Discard_All();
            Session["IBRef"] = null;
            Session["OldJVVoucherNumber"] = null;
            Session["OldWhichTypeItem"] = null;
        }
        if (strSplitCommand == "MainSubAccountChange")
        {

            Session["SubAccount"] = "Yes";


            TextBox txtSubAccountE = (TextBox)DetailsGrid.FindEditFormTemplateControl("txtSubAccountE");
            HiddenField txtMainAccountE_hidden = (HiddenField)DetailsGrid.FindEditFormTemplateControl("txtMainAccountE_hidden");
            HiddenField txtSubAccountE_hidden = (HiddenField)DetailsGrid.FindEditFormTemplateControl("txtSubAccountE_hidden");


            ASPxComboBox ComboMainAccountE = (ASPxComboBox)DetailsGrid.FindEditFormTemplateControl("CmbMainAccount");


            ASPxComboBox ComboSubAccountE = (ASPxComboBox)DetailsGrid.FindEditFormTemplateControl("CmbSubAccount");
            DataSet DsCheckMainAccountChange = new DataSet();
            int rowindex = DetailsGrid.EditingRowVisibleIndex;
            if (rowindex != -1)
            {
                string KeyValue = Convert.ToString(DetailsGrid.GetRowValues(rowindex, "CashReportID"));
                if (File.Exists(Server.MapPath(JournalVoucherFile_XMLPATH)))
                {
                    if (DsCheckMainAccountChange.Tables.Count > 0) { DsCheckMainAccountChange.Tables.Remove(DsCheckMainAccountChange.Tables[0]); DsCheckMainAccountChange.Clear(); }
                    DsCheckMainAccountChange.ReadXml(Server.MapPath(JournalVoucherFile_XMLPATH));
                    DsCheckMainAccountChange.Tables[0].PrimaryKey = new DataColumn[] { DsCheckMainAccountChange.Tables[0].Columns["CashReportID"] };
                    DataRow row = DsCheckMainAccountChange.Tables[0].Rows.Find(KeyValue);
                    if (Convert.ToString(row["MainAccountID"]).Split('~')[0] != txtMainAccountE_hidden.Value.Split('~')[0])
                    {
                        txtSubAccountE.Text = String.Empty;
                        DetailsGrid.JSProperties["cpMainAccountChange"] = txtMainAccountE_hidden.Value;
                        row["IsVoucherNumberChange"] = "Y";
                        DsCheckMainAccountChange.AcceptChanges();
                    }
                    else
                    {
                        DetailsGrid.JSProperties["cpMainAccountChange"] = "undefined";
                    }
                }
            }
        }

    }
    void AddData_ToGrid(string BranchID)
    {
        DataSet DsAddXML = new DataSet();
        string Segment = "";
        string MainAccID = null;
        string[] mainAccountID = hdnMainAccountId.Value.ToString().Split('~');
        if (mainAccountID.Length > 1)
            MainAccID = mainAccountID[0];
        else
            MainAccID = hdnMainAccountId.Value.ToString();
        if (File.Exists(Server.MapPath(JournalVoucherFile_XMLPATH)))
        {
            if (DsAddXML.Tables.Count > 0) { DsAddXML.Tables.Remove(DsAddXML.Tables[0]); DsAddXML.Clear(); }
            DsAddXML.ReadXml(Server.MapPath(JournalVoucherFile_XMLPATH));
            if (DsAddXML.Tables[0].Rows.Count > 0)
            {
                PCounter = Convert.ToInt32(DsAddXML.Tables[0].Rows[DsAddXML.Tables[0].Rows.Count - 1]["CashReportID"].ToString()) + 1;

            }
            DataRow drReport = DsAddXML.Tables[0].NewRow();
            drReport[0] = PCounter;
            drReport[1] = Convert.ToString(Session["KeyVal"]);
            drReport[2] = Convert.ToString(ViewState["WhichSegment"]);
            drReport[3] = (BranchID != "NAB") ? BranchID : Convert.ToString(ddlBranch.SelectedItem.Value);
            drReport[4] = MainAccID;
            drReport[5] = hdnSubAccountId.Value != "" ? ((BranchID != "NAB") ? hdnSubAccountId.Value.ToString().Split('~')[0] + "~" + hdnSubAccountId.Value.ToString().Split('~')[1] + "~" + hdnSubAccountId.Value.ToString().Split('~')[2] : hdnSubAccountId.Value != String.Empty ? hdnSubAccountId.Value.Split('~')[0] : String.Empty) : String.Empty;

            //drReport[5] = (BranchID != "NAB") ? txtSubAccount_hidden.Value.ToString().Split('~')[0] + "~" + txtSubAccount_hidden.Value.ToString().Split('~')[1] + "~" + txtSubAccount_hidden.Value.ToString().Split('~')[2] : txtSubAccount_hidden.Value != String.Empty ? txtSubAccount_hidden.Value.Split('~')[0] : String.Empty;
            drReport[6] = txtdebit.Text;
            drReport[7] = txtcredit.Text;
            drReport[11] = objConverter.getFormattedvalue(Convert.ToDecimal(txtdebit.Text));
            drReport[12] = objConverter.getFormattedvalue(Convert.ToDecimal(txtcredit.Text));
            drReport[8] = txtNarration.Text;
            //drReport[10] = txtMainAccount.Text.ToString();
            drReport[10] = hdnMainAccountText.Value.ToString();
            //drReport[9] = txtSubAccount_hidden.Value != "" ? txtSubAccount.Text.ToString() : String.Empty;
            drReport[9] = hdnSubAccountId.Value != "" ? hdnSubAccountText.Value.ToString() : String.Empty;
            drReport[13] = tDate.Value;
            drReport[14] = objConverter.ArrangeDate(Convert.ToDateTime(tDate.Value.ToString()).ToShortDateString());
            //string[] MainAC = txtMainAccount_hidden.Value.ToString().Split('~');

            string[] MainAC = hdnMainAccountId.Value.ToString().Split('~');
            DataTable dtType = new DataTable();
            if (MainAC.Length > 1)
                dtType = oDBEngine.GetDataTable("master_mainaccount", "ltrim(rtrim(mainaccount_SubLedgerType))", " mainaccount_ReferenceID=" + MainAC[0].ToString() + "");
            else
                dtType = oDBEngine.GetDataTable("master_mainaccount", "ltrim(rtrim(mainaccount_SubLedgerType))", " mainaccount_ReferenceID=" + hdnMainAccountId.Value.ToString() + "");
            //dtType = oDBEngine.GetDataTable("master_mainaccount", "ltrim(rtrim(mainaccount_SubLedgerType))", " mainaccount_ReferenceID=" + txtMainAccount_hidden.Value.ToString() + "");
            drReport[15] = dtType.Rows[0][0].ToString();
            drReport[16] = Convert.ToString(Session["userid"]);
            drReport[17] = oDBEngine.GetDate();
            drReport[18] = hdn_Brch_NonBrch.Value;
            drReport[19] = txtAccountCode.Text;
            //drReport[20] = txtSubAccount_hidden.Value != "" ? txtSubAccount_hidden.Value.Split('~')[3] : String.Empty;
            drReport[20] = hdnSubAccountId.Value != "" ? hdnSubAccountId.Value.Split('~')[3] : String.Empty;
            drReport[21] = txtNarration1.Text.Trim();
            drReport[22] = txtBillNo.Text;
            drReport[23] = ddlBranch.SelectedIndex;
            //if (ComboMode.SelectedItem.Text == "Edit")
            if (hdnMode.Value == "1") //Edit
            {
                drReport[24] = String.Empty;
                drReport[25] = "GenerateNew";
            }
            else
            {
                drReport[24] = String.Empty;
                drReport[25] = String.Empty;
            }
            drReport[26] = String.Empty;
            drReport[27] = ChoosenCurrency;
            DsAddXML.Tables[0].Rows.Add(drReport);
            DsAddXML.Tables[0].AcceptChanges();
            DsAddXML.Tables[0].WriteXml(Server.MapPath(JournalVoucherFile_XMLPATH));
            DsAddXML.Dispose();
            txtSubAccount_hidden.Value = String.Empty;
            hdn_Brch_NonBrch.Value = String.Empty;
        }
        else
        {


            if (DsAddXML.Tables.Count > 0) { DsAddXML.Tables.Remove(DsAddXML.Tables[0]); DsAddXML.Clear(); }
            dtReport = DsAddXML.Tables.Add();
            dtReport.Columns.Add(new DataColumn("CashReportID", typeof(int))); //0
            dtReport.Columns.Add(new DataColumn("JournalVoucherDetail_ID", typeof(String)));//1
            dtReport.Columns.Add(new DataColumn("JournalVoucherDetail_ExchangeSegmentID", typeof(String)));//2
            dtReport.Columns.Add(new DataColumn("JournalVoucherDetail_BranchID", typeof(String)));//3
            dtReport.Columns.Add(new DataColumn("JournalVoucherDetail_MainAccountCode", typeof(String)));//4
            dtReport.Columns.Add(new DataColumn("JournalVoucherDetail_SubAccountCode", typeof(String)));//5
            dtReport.Columns.Add(new DataColumn("JournalVoucherDetail_AmountDr", typeof(Decimal)));//6
            dtReport.Columns.Add(new DataColumn("JournalVoucherDetail_AmountCr", typeof(Decimal)));//7
            dtReport.Columns.Add(new DataColumn("JournalVoucherDetail_Narration", typeof(String)));//8
            dtReport.Columns.Add(new DataColumn("SubAccount1", typeof(string)));//9
            dtReport.Columns.Add(new DataColumn("MainAccount1", typeof(string)));//10
            dtReport.Columns.Add(new DataColumn("WithDrawl", typeof(string)));//11
            dtReport.Columns.Add(new DataColumn("Receipt", typeof(string)));//12
            dtReport.Columns.Add(new DataColumn("JournalVoucherDetail_ValueDate", typeof(DateTime)));//13
            dtReport.Columns.Add(new DataColumn("JournalVoucherDetail_ValueDate1", typeof(string)));//14
            dtReport.Columns.Add(new DataColumn("Type", typeof(string)));//15
            dtReport.Columns.Add(new DataColumn("UserID", typeof(string)));//16
            dtReport.Columns.Add(new DataColumn("EntryDateTime", typeof(DateTime)));//17
            dtReport.Columns.Add(new DataColumn("BranchNonBranch", typeof(string)));//18
            dtReport.Columns.Add(new DataColumn("Prefix", typeof(string)));//19
            dtReport.Columns.Add(new DataColumn("SubAccountName", typeof(string)));//20
            dtReport.Columns.Add(new DataColumn("SubNarration", typeof(string)));//21
            dtReport.Columns.Add(new DataColumn("BillNo", typeof(string)));//22
            dtReport.Columns.Add(new DataColumn("DDLSelectedBranch", typeof(string)));//23
            dtReport.Columns.Add(new DataColumn("OldJVTranDate", typeof(string)));//24
            dtReport.Columns.Add(new DataColumn("OldJVVoucherNumber", typeof(string)));//25
            dtReport.Columns.Add(new DataColumn("OldWhichTypeItem", typeof(string)));//26
            //Currency Setting
            dtReport.Columns.Add(new DataColumn("ChoosenCurrency", typeof(string)));//27 To Remember Last ChoosenCurrency



            DataRow drReport = dtReport.NewRow();
            drReport[0] = 1;
            drReport[1] = Session["KeyVal"].ToString();
            drReport[2] = ViewState["WhichSegment"].ToString();
            drReport[3] = (BranchID != "NAB") ? BranchID : ddlBranch.SelectedItem.Value.ToString();
            drReport[4] = MainAccID;
            //drReport[5] = (BranchID != "NAB") ? txtSubAccount_hidden.Value.ToString().Split('~')[0] + "~" + txtSubAccount_hidden.Value.ToString().Split('~')[1] + "~" + txtSubAccount_hidden.Value.ToString().Split('~')[2] : txtSubAccount_hidden.Value != String.Empty ? txtSubAccount_hidden.Value.Split('~')[0] : String.Empty;
            drReport[5] = hdnSubAccountId.Value != "" ? ((BranchID != "NAB") ? hdnSubAccountId.Value.ToString().Split('~')[0] + "~" + hdnSubAccountId.Value.ToString().Split('~')[1] + "~" + hdnSubAccountId.Value.ToString().Split('~')[2] : hdnSubAccountId.Value != String.Empty ? hdnSubAccountId.Value.Split('~')[0] : String.Empty) : String.Empty;

            drReport[6] = txtdebit.Text;
            drReport[7] = txtcredit.Text;
            drReport[11] = objConverter.getFormattedvalue(Convert.ToDecimal(txtdebit.Text));
            drReport[12] = objConverter.getFormattedvalue(Convert.ToDecimal(txtcredit.Text));
            drReport[8] = txtNarration.Text;
            //drReport[10] = txtMainAccount.Text.ToString();
            drReport[10] = hdnMainAccountText.Value.ToString();
            //drReport[9] = txtSubAccount_hidden.Value != "" ? txtSubAccount.Text.ToString() : String.Empty;
            drReport[9] = hdnSubAccountId.Value != "" ? hdnSubAccountText.Value.ToString() : String.Empty;
            drReport[13] = tDate.Value;
            drReport[14] = objConverter.ArrangeDate(Convert.ToDateTime(tDate.Value.ToString()).ToShortDateString());
            //string[] MainAC = txtMainAccount_hidden.Value.ToString().Split('~');
            string[] MainAC = hdnMainAccountId.Value.ToString().Split('~');
            DataTable dtType = new DataTable();
            if (MainAC.Length > 1)
                dtType = oDBEngine.GetDataTable("master_mainaccount", "ltrim(rtrim(mainaccount_SubLedgerType))", " mainaccount_ReferenceID=" + MainAC[0].ToString() + "");
            else
                dtType = oDBEngine.GetDataTable("master_mainaccount", "ltrim(rtrim(mainaccount_SubLedgerType))", " mainaccount_ReferenceID=" + hdnMainAccountId.Value.ToString() + "");
            //dtType = oDBEngine.GetDataTable("master_mainaccount", "ltrim(rtrim(mainaccount_SubLedgerType))", " mainaccount_ReferenceID=" + txtMainAccount_hidden.Value.ToString() + "");
            drReport[15] = dtType.Rows[0][0].ToString();
            drReport[16] = Session["userid"].ToString();
            drReport[17] = oDBEngine.GetDate();
            drReport[18] = hdn_Brch_NonBrch.Value;
            drReport[19] = txtAccountCode.Text;
            //drReport[20] = txtSubAccount_hidden.Value != "" ? txtSubAccount_hidden.Value.Split('~')[3] : String.Empty;
            drReport[20] = hdnSubAccountId.Value != "" ? hdnSubAccountId.Value.Split('~')[3] : String.Empty;
            drReport[21] = txtNarration1.Text.Trim();
            drReport[22] = txtBillNo.Text;
            drReport[23] = ddlBranch.SelectedIndex;
            if (hdnMode.Value == "1") //Edit
            {
                drReport[24] = String.Empty;
                drReport[25] = "GenerateNew";
            }
            else
            {
                drReport[24] = String.Empty;
                drReport[25] = String.Empty;
            }
            drReport[26] = String.Empty;
            drReport[27] = ChoosenCurrency;
            dtReport.Rows.Add(drReport);
            dtReport.AcceptChanges();
            DsAddXML.Tables[0].TableName = "DtJournalVoucher";
            DsAddXML.WriteXml(Server.MapPath(JournalVoucherFile_XMLPATH));

        }
        BindGrid(DetailsGrid, DsAddXML, "DESC");
        txtSubAccount_hidden.Value = String.Empty;
        hdn_Brch_NonBrch.Value = String.Empty;
        txtMainAccount_hidden.Value = String.Empty;
        DsAddXML.Dispose();
    }
    [WebMethod]
    public static string getSchemeType(string sel_scheme_id)
    {
        //BusinessLogicLayer.DBEngine oDbEngine1 = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDbEngine1 = new BusinessLogicLayer.DBEngine();


        string[] scheme = oDbEngine1.GetFieldValue1("tbl_master_Idschema", "schema_type", "Id = " + Convert.ToInt32(sel_scheme_id), 1);
        return Convert.ToString(scheme[0]);
    }

    /* This function check & generate Voucher No Automatically or Manually
     * store the Voucher no into - JVNumStr variable for auto 
     * checkNMakeJVCode(string, int)
     */
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

                sqlQuery = "SELECT max(tjv.JournalVoucher_VoucherNumber) FROM Trans_JournalVoucher tjv WHERE dbo.RegexMatch('";
                if (prefLen > 0)
                    sqlQuery += "[" + prefCompCode + "]{" + prefLen + "}";
                else if (scheme_type == 2)
                    sqlQuery += "^";
                sqlQuery += "[0-9]{" + paddCounter + "}";
                if (sufxLen > 0)
                    sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_VoucherNumber))) = 1";
                if (scheme_type == 2)
                    sqlQuery += " AND CONVERT(DATE, JournalVoucher_CreateDateTime) = CONVERT(DATE, GETDATE())";
                dtC = oDBEngine.GetDataTable(sqlQuery);

                if (dtC.Rows[0][0].ToString() == "")
                {
                    sqlQuery = "SELECT max(tjv.JournalVoucher_VoucherNumber) FROM Trans_JournalVoucher tjv WHERE dbo.RegexMatch('";
                    if (prefLen > 0)
                        sqlQuery += "[" + prefCompCode + "]{" + prefLen + "}";
                    else if (scheme_type == 2)
                        sqlQuery += "^";
                    sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                    if (sufxLen > 0)
                        sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                    sqlQuery += "?$', LTRIM(RTRIM(tjv.JournalVoucher_VoucherNumber))) = 1";
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
                sqlQuery = "SELECT JournalVoucher_VoucherNumber FROM Trans_JournalVoucher WHERE JournalVoucher_VoucherNumber LIKE '" + manual_str.Trim() + "'";
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

    string Save_Records()
    {
        grid.JSProperties["cpVouvherNo"] = "";
        hdnJournalCode.Value = "";

        DataSet DsSaveRecordXML = new DataSet();
        DataSet DsSaveRecordXMLValidation = new DataSet();
        DataSet DsFetchToPrint;
        SqlDataAdapter DaFetchToPrint;
        string WhichTypeItemsExist = null;
        string StrAutoGeneration = oconverter.GetAutoGenerateNo();
        int NonBranchItemsCount, BranchItemsCount;
        int RowAffected = 0;
        if (File.Exists(Server.MapPath(JournalVoucherFile_XMLPATH)))
        {
            if (DsSaveRecordXML.Tables.Count > 0) { DsSaveRecordXML.Tables.Remove(DsSaveRecordXML.Tables[0]); DsSaveRecordXML.Clear(); }
            DsSaveRecordXML.ReadXml(Server.MapPath(JournalVoucherFile_XMLPATH));
            if (hdnMode.Value == "1") //Edit
            {
                if (DsSaveRecordXML.Tables.Count > 0)
                {
                    if (DsSaveRecordXML.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < DsSaveRecordXML.Tables[0].Rows.Count; i++)
                        {
                            DsSaveRecordXML.Tables[0].Rows[i]["JournalVoucherDetail_Narration"] = txtNarration.Text.Trim() != String.Empty ? txtNarration.Text : String.Empty;
                            DsSaveRecordXML.Tables[0].Rows[i]["SubNarration"] = DsSaveRecordXML.Tables[0].Rows[i]["SubNarration"].ToString().Trim();
                            DsSaveRecordXML.Tables[0].Rows[i]["BillNo"] = txtBillNo.Text.Trim() != String.Empty ? txtBillNo.Text : String.Empty;
                            DsSaveRecordXML.Tables[0].Rows[i]["JournalVoucherDetail_ValueDate"] = tDate.Value;
                            DsSaveRecordXML.Tables[0].Rows[i]["JournalVoucherDetail_ValueDate1"] = objConverter.ArrangeDate(Convert.ToDateTime(tDate.Value.ToString()).ToShortDateString());
                        }
                        DsSaveRecordXML.AcceptChanges();
                    }
                }
            }
            else
            {
                if (DsSaveRecordXML.Tables.Count > 0)
                {
                    if (DsSaveRecordXML.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < DsSaveRecordXML.Tables[0].Rows.Count; i++)
                        {
                            DsSaveRecordXML.Tables[0].Rows[i]["JournalVoucherDetail_Narration"] = txtNarration.Text.Trim() != String.Empty ? txtNarration.Text : String.Empty;
                        }
                        DsSaveRecordXML.AcceptChanges();
                    }
                }
            }
        }
        if (File.Exists(Server.MapPath(JournalVoucherFile_XMLPATH)))
        {
            if (DsSaveRecordXMLValidation.Tables.Count > 0) { DsSaveRecordXMLValidation.Tables.Remove(DsSaveRecordXMLValidation.Tables[0]); DsSaveRecordXMLValidation.Clear(); }
            DsSaveRecordXMLValidation.ReadXml(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
            NonBranchItemsCount = Convert.ToInt32(DsSaveRecordXMLValidation.Tables[0].Rows[1][1].ToString());
            BranchItemsCount = Convert.ToInt32(DsSaveRecordXMLValidation.Tables[0].Rows[0][1].ToString());
            if (NonBranchItemsCount == 0 && BranchItemsCount > 0)
                WhichTypeItemsExist = "B";
            else if (BranchItemsCount == 0 && NonBranchItemsCount > 0)
                WhichTypeItemsExist = "NB";
            else if (NonBranchItemsCount == 1 && BranchItemsCount > 0)
                WhichTypeItemsExist = "BGN";
            else
                WhichTypeItemsExist = "NGB";

        }

        //This For Log Purpose
        string strLogID = ViewState["LogID"].ToString();

        if (DsSaveRecordXML.Tables[0].Rows.Count > 0)
        {
            if (ViewState["WhichSegment"] != null)
            {
                string OldVoucherNumber = string.Empty;
                string OldIBRef = string.Empty;
                string OldWhichTypeItem = string.Empty;
                OldVoucherNumber = (Session["OldJVVoucherNumber"] != null) ? Session["OldJVVoucherNumber"].ToString() : String.Empty;
                OldIBRef = (Session["IBRef"] != null) ? Session["IBRef"].ToString() : String.Empty;
                OldWhichTypeItem = (Session["OldWhichTypeItem"] != null) ? Session["OldWhichTypeItem"].ToString() : String.Empty;

                string Ctype = hdnMode.Value;
                string CtypeText = string.Empty;
                if (Ctype == "1") //Edit
                    CtypeText = "Edit";
                else
                    CtypeText = "Entry";

                //DsFetchToPrint = oOtherTasks.InsertJournalVoucherEntry(DsSaveRecordXML.GetXml(), Session["userid"].ToString(), Session["LastFinYear"].ToString(), CompanyID,
                // Convert.ToDateTime(tDate.Value).ToString("yyyy-MM-dd"), Convert.ToString(System.Data.SqlTypes.SqlChars.Null), Convert.ToString(System.Data.SqlTypes.SqlChars.Null), txtBillNo.Text,
                // txtAccountCode.Text, ViewState["WhichSegment"].ToString().Trim(), WhichTypeItemsExist, StrAutoGeneration, Session["EntryProfileType"].ToString(),
                // Convert.ToString(ComboMode.SelectedItem), OldIBRef, OldVoucherNumber, OldWhichTypeItem, Convert.ToInt32(ChoosenCurrency.Split('~')[0]));

                if (Ctype != "1")
                {
                    //string validate = checkNMakeJVCode(Convert.ToString(txtBillNo.Text), Convert.ToInt32(CmbScheme.SelectedItem.Value));
                    //string validate = checkNMakeJVCode(Convert.ToString(txtBillNo.Text), Convert.ToInt32(rblScheme.SelectedValue));
                    string validate = checkNMakeJVCode(Convert.ToString(txtBillNo.Text), Convert.ToInt32(CmbScheme.SelectedValue));

                    if (validate == "outrange")
                    {
                        return "outrange";
                    }
                    else if (validate == "duplicate")
                    {
                        return "duplicate";
                    }
                    else
                    {
                        txtBillNo.Text = JVNumStr;
                    }

                    //DataTable dtSchema = oDBEngine.GetDataTable("tbl_master_idschema", "prefix, suffix, digit, startno, schema_type", "id=" + Convert.ToInt32(CmbScheme.SelectedItem.Value));
                    DataTable dtSchema = oDBEngine.GetDataTable("tbl_master_idschema", "prefix, suffix, digit, startno, schema_type", "id=" + Convert.ToInt32(CmbScheme.SelectedValue));
                    int scheme_type = Convert.ToInt32(dtSchema.Rows[0]["schema_type"]);
                    if (scheme_type != 0)
                    {
                        grid.JSProperties["cpVouvherNo"] = JVNumStr;
                    }
                }

                DsFetchToPrint = oOtherTasks.InsertJournalVoucherEntry(DsSaveRecordXML.GetXml(), Session["userid"].ToString(), Session["LastFinYear"].ToString(), CompanyID,
                 Convert.ToDateTime(tDate.Value).ToString("yyyy-MM-dd"), Convert.ToString(System.Data.SqlTypes.SqlChars.Null), Convert.ToString(System.Data.SqlTypes.SqlChars.Null), txtBillNo.Text,
                 txtAccountCode.Text, ViewState["WhichSegment"].ToString().Trim(), WhichTypeItemsExist, StrAutoGeneration, Session["EntryProfileType"].ToString(),
                 Convert.ToString(CtypeText), OldIBRef, OldVoucherNumber, OldWhichTypeItem, Convert.ToInt32(ChoosenCurrency.Split('~')[0]));

                // using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
                // {
                //using (SqlCommand com = new SqlCommand("Insert_JournalVoucherEntry", con))
                // {
                //com.CommandType = CommandType.StoredProcedure;
                //com.Parameters.AddWithValue("@JournalVoucherXML", DsSaveRecordXML.GetXml());
                //com.Parameters.AddWithValue("@createuser", Session["userid"].ToString());
                //com.Parameters.AddWithValue("@finyear", Session["LastFinYear"].ToString());
                //com.Parameters.AddWithValue("@compID", CompanyID);
                //com.Parameters.AddWithValue("@JournalVoucherDetail_TransactionDate", Convert.ToDateTime(tDate.Value));
                //com.Parameters.AddWithValue("@JournalVoucher_SettlementNumber", System.Data.SqlTypes.SqlChars.Null);
                //com.Parameters.AddWithValue("@JournalVoucher_SettlementType", System.Data.SqlTypes.SqlChars.Null);
                //com.Parameters.AddWithValue("@JournalVoucher_BillNumber", txtBillNo.Text);
                //com.Parameters.AddWithValue("@JournalVoucher_Prefix", txtAccountCode.Text);
                //com.Parameters.AddWithValue("@segmentid", ViewState["WhichSegment"].ToString().Trim());
                //com.Parameters.AddWithValue("@WhichTypeItemsExist", WhichTypeItemsExist);
                //com.Parameters.AddWithValue("@IBRef", StrAutoGeneration);
                //com.Parameters.AddWithValue("@EntryUserProfile", Session["EntryProfileType"].ToString());
                //com.Parameters.AddWithValue("@FormMode", ComboMode.SelectedItem.ToString());
                //com.Parameters.AddWithValue("@OldIBRef", (Session["IBRef"] != null) ? Session["IBRef"].ToString() : String.Empty);
                //com.Parameters.AddWithValue("@OldVoucherNumber", (Session["OldJVVoucherNumber"] != null) ? Session["OldJVVoucherNumber"].ToString() : String.Empty);
                //com.Parameters.AddWithValue("@OldWhichTypeItem", (Session["OldWhichTypeItem"] != null) ? Session["OldWhichTypeItem"].ToString() : String.Empty);
                ////Currency Setting
                //com.Parameters.AddWithValue("@CurrencyID", ChoosenCurrency.Split('~')[0]);
                //com.CommandTimeout = 0;
                //This For Log Purpose
                if (CtypeText == "Edit")
                {
                    if (Session["IBRef"] != null)
                    {
                        string IBRef = Session["IBRef"].ToString();
                        //oGenericLogSystem.CreateLog("Trans_JournalVoucher", "JournalVoucher_IBRef='" + IBRef + "'", GenericLogSystem.LogState.XmlSaving, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucher", strLogID, GenericLogSystem.LogType.JV);
                        //oGenericLogSystem.CreateLog("Trans_JournalVoucherDetail", "JournalVoucherDetail_IBRef='" + IBRef + "'", GenericLogSystem.LogState.XmlSaving, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucherDetail", strLogID, GenericLogSystem.LogType.JV);
                    }
                }

                //  using (DaFetchToPrint = new SqlDataAdapter(com))
                //  {
                //  DsFetchToPrint.Clear();
                //  DaFetchToPrint.Fill(DsFetchToPrint);
                //This For Log Purpose
                if (CtypeText == "Edit")
                {
                    if (Session["IBRef"] != null)
                    {
                        string IBRef = Session["IBRef"].ToString();
                        // oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlSaved, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucher", strLogID, GenericLogSystem.LogType.JV);
                        // oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlSaved, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucherDetail", strLogID, GenericLogSystem.LogType.JV);
                    }
                }
                else
                {
                    string IBRef = StrAutoGeneration;
                    //oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlSaved, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucher", strLogID, GenericLogSystem.LogType.JV);
                    //oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlSaved, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucherDetail", strLogID, GenericLogSystem.LogType.JV);
                }
                //  }
                if (DsFetchToPrint.Tables.Count > 0)
                {
                    if (DsFetchToPrint.Tables[0].Rows.Count > 0)
                    {
                        try
                        {
                            File.Delete(Server.MapPath(JournalVoucherFile_XMLPATH));
                            File.Delete(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
                        }
                        catch
                        {
                            File.Delete(Server.MapPath(JournalVoucherFile_XMLPATH));
                            File.Delete(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
                        }
                        BindGrid(DetailsGrid);
                        Session["DSJournalReturn"] = DsFetchToPrint;
                        Session["StrQuery"] = null;
                        Session["IBRef"] = null;
                        Session["OldJVVoucherNumber"] = null;
                        Session["OldWhichTypeItem"] = null;

                        //Currency Setting
                        Session["ActiveCurrency"] = Session["TradeCurrency"];

                        return "Success";

                    }
                }
                else
                {
                    //This For Log Purpose
                    //oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlError, Session["UserID"].ToString(), Session["IBRef"].ToString(), "Trans_JournalVoucher", strLogID, GenericLogSystem.LogType.JV);
                    return "Problem";
                }
                // }
                // }
            }
            else
            {
                //This For Log Purpose
                //oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlError, Session["UserID"].ToString(), Session["IBRef"].ToString(), "Trans_JournalVoucherDetail", strLogID, GenericLogSystem.LogType.JV);
                return "Problem";
            }
        }
        DsSaveRecordXMLValidation.Dispose();
        DsSaveRecordXML.Dispose();

        //This For Log Purpose
        //oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlError, Session["UserID"].ToString(), Session["IBRef"].ToString(), "Trans_JournalVoucherDetail", strLogID, GenericLogSystem.LogType.JV);
        return "Problem";

    }
    string Discard_All()
    {
        string IBRef = Session["IBRef"] != null ? Session["IBRef"].ToString() : String.Empty;
        if (File.Exists(Server.MapPath(JournalVoucherFile_XMLPATH)))
        {
            try
            {
                File.Delete(Server.MapPath(JournalVoucherFile_XMLPATH));
                File.Delete(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
            }
            catch
            {
                File.Delete(Server.MapPath(JournalVoucherFile_XMLPATH));
                File.Delete(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
            }
            BindGrid(DetailsGrid);
            //This For Log Purpose
            string strLogID = ViewState["LogID"].ToString();
            //oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlExit, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucher", strLogID, GenericLogSystem.LogType.JV);
            //oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlExit, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucherDetail", strLogID, GenericLogSystem.LogType.JV);
            return "SuccessDiscard";
        }
        return "Problem";
    }
    protected void DetailsGrid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
    {
        TotalDebit = 0;
        TotalCredit = 0;
    }
    void EntryButtonClick()
    {
        if (TotalDebit < TotalCredit)
        {
            tdSaveButton.Visible = false;
        }
        else if (TotalDebit > TotalCredit)
        {
            tdSaveButton.Visible = false;
        }
        else if (TotalDebit == TotalCredit)
        {
            tdSaveButton.Visible = true;
        }
        else
        {

        }
    }
    void AddData_EntryValidationXML(string branchID)
    {
        DataSet DsEntryValidXML = new DataSet();
        Boolean RecordExist = false;
        string BranchIDContant = String.Empty;
        if (File.Exists(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH)))
        {
            if (DsEntryValidXML.Tables.Count > 0) { DsEntryValidXML.Tables.Remove(DsEntryValidXML.Tables[0]); DsEntryValidXML.Tables[0].Clear(); }
            DsEntryValidXML.ReadXml(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
            if (DsEntryValidXML.Tables.Count > 0)
            {
                if (DsEntryValidXML.Tables[0].Rows.Count > 0)
                {
                    PCounter = Convert.ToInt32(DsEntryValidXML.Tables[0].Rows[DsEntryValidXML.Tables[0].Rows.Count - 1]["CashReportID"].ToString()) + 1;
                }
            }
            if (DsEntryValidXML.Tables[0].Rows.Count > 0)
            {
                if (branchID == "NAB")
                {
                    DsEntryValidXML.Tables[0].Rows[1][1] = Convert.ToInt32(DsEntryValidXML.Tables[0].Rows[1][1].ToString()) + 1;
                    DsEntryValidXML.Tables[0].AcceptChanges();
                }
                else
                {
                    for (int i = 2; i < DsEntryValidXML.Tables[0].Rows.Count; i++)
                    {
                        BranchIDContant = DsEntryValidXML.Tables[0].Rows[i][0].ToString();
                        if (branchID == BranchIDContant)
                        {
                            RecordExist = true;
                            DsEntryValidXML.Tables[0].Rows[i][1] = Convert.ToInt32(DsEntryValidXML.Tables[0].Rows[i][1].ToString()) + 1;
                            DsEntryValidXML.Tables[0].AcceptChanges();
                            break;
                        }
                    }
                    if (!RecordExist)
                    {
                        if (IsNumeric(branchID))
                        {
                            DataRow DrValidEntry = DsEntryValidXML.Tables[0].NewRow();
                            DsEntryValidXML.Tables[0].Rows[0][1] = Convert.ToInt32(DsEntryValidXML.Tables[0].Rows[0][1].ToString()) + 1;
                            DsEntryValidXML.Tables[0].AcceptChanges();
                            DrValidEntry[0] = branchID;
                            DrValidEntry[1] = 1;
                            DrValidEntry[2] = PCounter;
                            DsEntryValidXML.Tables[0].Rows.Add(DrValidEntry);
                        }
                    }
                }
            }
            DsEntryValidXML.WriteXml(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
            hdn_Brch_NonBrch.Value = String.Empty;
        }
        else
        {
            if (DsEntryValidXML.Tables.Count > 0) { DsEntryValidXML.Tables.Remove(DsEntryValidXML.Tables[0]); DsEntryValidXML.Tables[0].Clear(); }
            DataTable DtValidEntry = DsEntryValidXML.Tables.Add();
            DtValidEntry.Columns.Add(new DataColumn("ID", typeof(string))); //0
            DtValidEntry.Columns.Add(new DataColumn("EntryCount", typeof(int)));//1
            DtValidEntry.Columns.Add(new DataColumn("CashReportID", typeof(int))); //2
            //DtValidEntry.Columns["CashReportID"].AutoIncrement = true;
            //DtValidEntry.Columns["CashReportID"].AutoIncrementSeed = 1;
            DataRow DrValidEntry = DtValidEntry.NewRow();
            DrValidEntry[0] = "BR";
            DrValidEntry[1] = 0;
            DrValidEntry[2] = 1;
            DtValidEntry.Rows.Add(DrValidEntry);
            DrValidEntry = DtValidEntry.NewRow();
            DrValidEntry[0] = "NB";
            DrValidEntry[1] = 0;
            DrValidEntry[2] = 2;
            DtValidEntry.Rows.Add(DrValidEntry);
            if (IsNumeric(branchID))
            {
                DrValidEntry = DtValidEntry.NewRow();
                DrValidEntry[0] = branchID;
                DrValidEntry[1] = 1;
                DrValidEntry[2] = 3;
                DtValidEntry.Rows.Add(DrValidEntry);
                DtValidEntry.Rows[0][1] = Convert.ToInt32(DtValidEntry.Rows[0][1].ToString()) + 1;
                DtValidEntry.AcceptChanges();
            }
            else
            {
                DrValidEntry = DtValidEntry.NewRow();
                DtValidEntry.Rows[1][1] = Convert.ToInt32(DtValidEntry.Rows[1][1].ToString()) + 1;
                DtValidEntry.AcceptChanges();
            }
            DsEntryValidXML.Tables[0].TableName = "JvValidEntry";
            DsEntryValidXML.WriteXml(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
            hdn_Brch_NonBrch.Value = String.Empty;
        }
        DsEntryValidXML.Dispose();
    }
    string AllowDisAllowEntry(string branchID)
    {
        DataSet DsAllowDisAllowEntry = new DataSet();
        int BranchCount, NonBranchCount;
        Boolean NotInBranchCount = true;
        if (File.Exists(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH)))
        {
            DsAllowDisAllowEntry.ReadXml(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
            DsAllowDisAllowEntry.Tables[0].PrimaryKey = new DataColumn[] { DsAllowDisAllowEntry.Tables[0].Columns["ID"] };
            DataRow row = DsAllowDisAllowEntry.Tables[0].Rows.Find(branchID);
            if (row != null)
            {
                NotInBranchCount = false;
            }
            BranchCount = Convert.ToInt32(DsAllowDisAllowEntry.Tables[0].Rows[0][1].ToString());
            NonBranchCount = Convert.ToInt32(DsAllowDisAllowEntry.Tables[0].Rows[1][1].ToString());
            DsAllowDisAllowEntry.Dispose();
            if (IsNumeric(branchID))
            {
                if (NonBranchCount > 1)
                    if (BranchCount == 1 && NotInBranchCount)
                        return "false~Branch";
                    else
                        return "true~";
                else
                    return "true~";
            }
            else
            {
                if (BranchCount > 1)
                    if (NonBranchCount == 1)
                        return "false~NonBranch";
                    else
                        return "true~";
                else
                    return "true~";
            }

        }
        return "true~";

    }
    string AllowDisAllowEntryWhenNAB(string DiffbranchIDOrNonBranch, string EditObjId)
    {
        int RowEntryCount = 0;
        Boolean NotInBranchCount = true;
        DataSet DsEntryWhenNAB = new DataSet();
        if (DsEntryWhenNAB.Tables.Count > 0) { DsEntryWhenNAB.Tables.Remove(DsEntryWhenNAB.Tables[0]); DsEntryWhenNAB.Clear(); }
        int BranchCount = 0, NonBranchCount = 0;
        if (File.Exists(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH)))
        {
            DsEntryWhenNAB.ReadXml(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
            DsEntryWhenNAB.Tables[0].PrimaryKey = new DataColumn[] { DsEntryWhenNAB.Tables[0].Columns["ID"] };
            if (DiffbranchIDOrNonBranch != "NAB" && EditObjId == "NAB")
            {
                BranchCount = Convert.ToInt32(DsEntryWhenNAB.Tables[0].Rows[0][1].ToString());
                NonBranchCount = Convert.ToInt32(DsEntryWhenNAB.Tables[0].Rows[1][1].ToString()) - 1;
            }
            else if (DiffbranchIDOrNonBranch == "NAB" && EditObjId != "NAB")
            {
                DataRow row = DsEntryWhenNAB.Tables[0].Rows.Find(DiffbranchIDOrNonBranch);
                if (row != null)
                {
                    RowEntryCount = Convert.ToInt32(row["EntryCount"].ToString());
                    if (RowEntryCount == 1)
                    {
                        BranchCount = Convert.ToInt32(DsEntryWhenNAB.Tables[0].Rows[0][1].ToString()) - 1;
                        NonBranchCount = Convert.ToInt32(DsEntryWhenNAB.Tables[0].Rows[1][1].ToString());
                    }
                    else
                    {
                        BranchCount = Convert.ToInt32(DsEntryWhenNAB.Tables[0].Rows[0][1].ToString());
                        NonBranchCount = Convert.ToInt32(DsEntryWhenNAB.Tables[0].Rows[1][1].ToString());
                    }
                }
                else
                {
                    BranchCount = Convert.ToInt32(DsEntryWhenNAB.Tables[0].Rows[0][1].ToString());
                    NonBranchCount = Convert.ToInt32(DsEntryWhenNAB.Tables[0].Rows[1][1].ToString());
                }
            }
            else
            {
                NonBranchCount = Convert.ToInt32(DsEntryWhenNAB.Tables[0].Rows[1][1].ToString());
                BranchCount = Convert.ToInt32(DsEntryWhenNAB.Tables[0].Rows[0][1].ToString());
            }
            DataRow row1 = DsEntryWhenNAB.Tables[0].Rows.Find(DiffbranchIDOrNonBranch);
            if (row1 != null)
            {
                NotInBranchCount = false;
            }
        }
        DsEntryWhenNAB.Dispose();

        if (IsNumeric(DiffbranchIDOrNonBranch))
        {
            if (NonBranchCount > 1 && NotInBranchCount)
                if (BranchCount == 1)
                    return "false~Branch";
                else
                    return "true~";
            else
                return "true~";
        }
        else
        {
            if (BranchCount > 1)
                if (NonBranchCount == 1)
                    return "false~NonBranch";
                else
                    return "true~";
            else
                return "true~";
        }


    }
    string AllowDisAllowEntryWhenBranch(string DiffbranchIDOrNonBranch, string EditObjId)
    {
        DataSet DsEntryWhenBranch;
        int BranchCount = 0, NonBranchCount = 0;
        int RowEntryCount;
        Boolean NotInBranchCount = true;
        if (File.Exists(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH)))
        {
            DsEntryWhenBranch = new DataSet();
            if (DsEntryWhenBranch.Tables.Count > 0) { DsEntryWhenBranch.Tables.Remove(DsEntryWhenBranch.Tables[0]); DsEntryWhenBranch.Clear(); }
            DsEntryWhenBranch.ReadXml(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
            DsEntryWhenBranch.Tables[0].PrimaryKey = new DataColumn[] { DsEntryWhenBranch.Tables[0].Columns["ID"] };
            DataRow row = DsEntryWhenBranch.Tables[0].Rows.Find(DiffbranchIDOrNonBranch);
            if (row != null)
            {
                RowEntryCount = Convert.ToInt32(row["EntryCount"].ToString());
                if (RowEntryCount == 1)
                {
                    BranchCount = Convert.ToInt32(DsEntryWhenBranch.Tables[0].Rows[0][1].ToString()) - 1;
                    NonBranchCount = Convert.ToInt32(DsEntryWhenBranch.Tables[0].Rows[1][1].ToString());
                }
                else
                {
                    BranchCount = Convert.ToInt32(DsEntryWhenBranch.Tables[0].Rows[0][1].ToString());
                    NonBranchCount = Convert.ToInt32(DsEntryWhenBranch.Tables[0].Rows[1][1].ToString());
                }
            }
            else
            {
                BranchCount = Convert.ToInt32(DsEntryWhenBranch.Tables[0].Rows[0][1].ToString());
                if (BranchCount == 1)
                {
                    row = DsEntryWhenBranch.Tables[0].Rows.Find(EditObjId);
                    if (row != null)
                    {
                        RowEntryCount = Convert.ToInt32(row["EntryCount"].ToString());
                        if (RowEntryCount == 1)
                        {
                            BranchCount = Convert.ToInt32(DsEntryWhenBranch.Tables[0].Rows[0][1].ToString()) - 1;
                            NonBranchCount = Convert.ToInt32(DsEntryWhenBranch.Tables[0].Rows[1][1].ToString());
                        }
                        else
                        {
                            BranchCount = Convert.ToInt32(DsEntryWhenBranch.Tables[0].Rows[0][1].ToString());
                            NonBranchCount = Convert.ToInt32(DsEntryWhenBranch.Tables[0].Rows[1][1].ToString());
                        }
                    }
                }
                else
                {
                    BranchCount = Convert.ToInt32(DsEntryWhenBranch.Tables[0].Rows[0][1].ToString());
                    NonBranchCount = Convert.ToInt32(DsEntryWhenBranch.Tables[0].Rows[1][1].ToString());
                }
                DataRow row1 = DsEntryWhenBranch.Tables[0].Rows.Find(DiffbranchIDOrNonBranch);
                if (row1 != null)
                {
                    NotInBranchCount = false;
                }
            }
            DsEntryWhenBranch.Dispose();
            if (IsNumeric(DiffbranchIDOrNonBranch))
            {
                if (NonBranchCount > 1 && NotInBranchCount)
                    if (BranchCount == 1)
                        return "false~Branch";
                    else
                        return "true~";
                else
                    return "true~";
            }
            else
            {
                if (BranchCount > 1)
                    if (NonBranchCount == 1)
                        return "false~NonBranch";
                    else
                        return "true~";
                else
                    return "true~";
            }
        }
        return "true~";

    }
    protected void DetailsGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
    {
        DataSet DsRowDeleteXML = new DataSet();
        DataSet DsRowDeleteXMLValid = new DataSet();
        if (File.Exists(Server.MapPath(JournalVoucherFile_XMLPATH)))
        {
            if (DsRowDeleteXML.Tables.Count > 0) { DsRowDeleteXML.Tables.Remove(DsRowDeleteXML.Tables[0]); DsRowDeleteXML.Clear(); }
            DsRowDeleteXML.ReadXml(Server.MapPath(JournalVoucherFile_XMLPATH));
            DsRowDeleteXML.Tables[0].PrimaryKey = new DataColumn[] { DsRowDeleteXML.Tables[0].Columns["CashReportID"] };
            DataRow row = DsRowDeleteXML.Tables[0].Rows.Find(e.Keys["CashReportID"]);
            string branchNonBranch = row["BranchNonBranch"].ToString();
            DsRowDeleteXML.Tables[0].Rows.Remove(row);
            e.Cancel = true;
            BindGrid(DetailsGrid, DsRowDeleteXML, "DESC");
            try
            {
                if (File.Exists(Server.MapPath(JournalVoucherFile_XMLPATH)))
                {
                    File.Delete(Server.MapPath(JournalVoucherFile_XMLPATH));
                    if (DsRowDeleteXML.Tables.Count > 0)
                    {
                        if (DsRowDeleteXML.Tables[0].Rows.Count > 0)
                        {
                            DsRowDeleteXML.WriteXml(Server.MapPath(JournalVoucherFile_XMLPATH));
                        }
                    }
                }
                if (File.Exists(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH)))
                {
                    DsRowDeleteXMLValid = new DataSet();
                    DsRowDeleteXMLValid.ReadXml(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
                    DsRowDeleteXMLValid.Tables[0].PrimaryKey = new DataColumn[] { DsRowDeleteXMLValid.Tables[0].Columns["ID"] };
                    if (branchNonBranch == "NAB")
                    {
                        row = DsRowDeleteXMLValid.Tables[0].Rows.Find("NB");
                        row["EntryCount"] = Convert.ToString(Convert.ToInt32(row["EntryCount"].ToString()) - 1);
                        row.AcceptChanges();
                    }
                    else
                    {
                        row = DsRowDeleteXMLValid.Tables[0].Rows.Find(branchNonBranch);
                        row["EntryCount"] = Convert.ToString(Convert.ToInt32(row["EntryCount"].ToString()) - 1);
                        row.AcceptChanges();
                        if (row["EntryCount"].ToString() == "0")
                        {
                            DsRowDeleteXMLValid.Tables[0].Rows[0][1] = Convert.ToInt32(DsRowDeleteXMLValid.Tables[0].Rows[0][1].ToString()) - 1;
                            DsRowDeleteXMLValid.Tables[0].Rows.Remove(row);
                            DsRowDeleteXMLValid.AcceptChanges();
                        }
                    }
                    if (DsRowDeleteXMLValid.Tables.Count > 0)
                    {
                        if (DsRowDeleteXMLValid.Tables[0].Rows.Count > 0)
                        {
                            DsRowDeleteXMLValid.WriteXml(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
                        }
                    }
                }
                if (DetailsGrid.VisibleRowCount == 0)
                {
                    if (File.Exists(Server.MapPath(JournalVoucherFile_XMLPATH)))
                    {
                        File.Delete(Server.MapPath(JournalVoucherFile_XMLPATH));
                    }
                    if (File.Exists(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH)))
                    {
                        File.Delete(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
                    }
                }

                TotalCredit = 0;
                TotalDebit = 0;
            }
            catch
            {
                DsRowDeleteXMLValid.Dispose();
                DsRowDeleteXML.Dispose();
            }
        }
        else
        {
            if (File.Exists(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH)))
            {
                File.Delete(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
            }
        }
        DsRowDeleteXMLValid.Dispose();
        DsRowDeleteXML.Dispose();
    }
    protected void DetailsGrid_HtmlEditFormCreated(object sender, ASPxGridViewEditFormEventArgs e)
    {
        TotalCredit = 0;
        TotalDebit = 0;
        int rowindex = DetailsGrid.EditingRowVisibleIndex;

        string BranchNonBranchE = DetailsGrid.GetRowValues(rowindex, "BranchNonBranch").ToString();
        string MainAccountE = DetailsGrid.GetRowValues(rowindex, "MainAccount1").ToString();
        string SubAccountE = DetailsGrid.GetRowValues(rowindex, "SubAccount1").ToString();
        string NarrationE = DetailsGrid.GetRowValues(rowindex, "JournalVoucherDetail_Narration").ToString();
        string LineNarrationE = DetailsGrid.GetRowValues(rowindex, "SubNarration").ToString();
        //BranchID
        string BranchID = DetailsGrid.GetRowValues(rowindex, "BranchNonBranch").ToString();
        //RecordId
        string KeyValue = DetailsGrid.GetRowValues(rowindex, "CashReportID").ToString();

        //EditRecord Debit
        string DebitE = DetailsGrid.GetRowValues(rowindex, "WithDrawl").ToString();
        ViewState["DebitWhenEdit"] = DebitE;
        //EditRecord Credit
        string CreditE = DetailsGrid.GetRowValues(rowindex, "Receipt").ToString();
        ViewState["CreditWhenEdit"] = CreditE;

        DataSet DsDetailsXML = new DataSet();
        if (!string.IsNullOrEmpty(JournalVoucherFile_XMLPATH))
        {
            Session["JournalVoucherFile_XMLPATH"] = JournalVoucherFile_XMLPATH;
        }
        else
        {
            JournalVoucherFile_XMLPATH = Session["JournalVoucherFile_XMLPATH"].ToString();
        }
        DsDetailsXML.ReadXml(Server.MapPath(JournalVoucherFile_XMLPATH));
        DsDetailsXML.Tables[0].PrimaryKey = new DataColumn[] { DsDetailsXML.Tables[0].Columns["CashReportID"] };
        DataRow row = DsDetailsXML.Tables[0].Rows.Find(KeyValue);


        HiddenField txtMainAccountE_hidden = (HiddenField)DetailsGrid.FindEditFormTemplateControl("txtMainAccountE_hidden");
        HiddenField txtSubAccountE_hidden = (HiddenField)DetailsGrid.FindEditFormTemplateControl("txtSubAccountE_hidden");


        ASPxTextBox TxtDebitE = (ASPxTextBox)DetailsGrid.FindEditFormTemplateControl("txtdebit");
        ASPxTextBox TxtCreditE = (ASPxTextBox)DetailsGrid.FindEditFormTemplateControl("txtcredit");
        //Narration
        string strNarrationE = txtNarration.Text;
        //LineNarration
        TextBox TxtLineNarrationE = (TextBox)DetailsGrid.FindEditFormTemplateControl("txtNarration1");



        ASPxComboBox ComboMainAccountE = (ASPxComboBox)DetailsGrid.FindEditFormTemplateControl("CmbMainAccount");


        var strQuery_Table = "Master_MainAccount";
        var strQuery_FieldName = "  MainAccount_Name+\' [ \'+rtrim(ltrim(MainAccount_AccountCode))+\' ]\' as MainAccount_Name1,cast(MainAccount_ReferenceID as varchar)+\'~\'+MainAccount_SubLedgerType+\'~MAINAC~\'+MainAccount_AccountType as MainAccount_ReferenceID";
        var strQuery_WhereClause = "(MainAccount_Name like (\'%RequestLetter%\') or MainAccount_AccountCode like (\'%RequestLetter%\')) and isnull(MainAccount_BankCashType,'abc') not in('Cash','Bank')";
        var strQuery_OrderBy = "";
        var strQuery_GroupBy = "";
        var CombinedQuery = strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy;



        List<string> obj = new List<string>();
        DataTable DT = new DataTable();
        DBEngine objEngine = new DBEngine();
        string RequestLetter = "%";
        string[] param = CombinedQuery.Replace("--", "+").Replace("^^", "%").Split('$');
        string Query_Table = param[0].Trim() != String.Empty ? param[0] : null;
        string Query_FieldName = param[1].Trim() != String.Empty ? param[1] : null;
        string Query_WhereClause = param[2].Trim() != String.Empty ? param[2] : null;
        string Query_OrderBy = param[3].Trim() != String.Empty ? param[3] : null;
        string Query_GroupBy = param[4].Trim() != String.Empty ? param[4] : null;
        if (Query_Table != null)
        {
            Query_Table = Query_Table.Replace("RequestLetter", RequestLetter);
        }
        if (Query_FieldName != null)
        {
            Query_FieldName = Query_FieldName.Replace("RequestLetter", RequestLetter);
        }
        if (Query_WhereClause != null)
        {
            Query_WhereClause = Query_WhereClause.Replace("RequestLetter", RequestLetter);
        }



        String lcSql;
        lcSql = "Select " + Query_FieldName + " from " + Query_Table;
        if (Query_WhereClause != null)
        {
            lcSql += " WHERE " + Query_WhereClause;
        }
        if (Query_GroupBy != null)
        {
            lcSql += " group By " + Query_GroupBy;
        }
        if (Query_OrderBy != null)
        {
            lcSql += " Order By " + Query_OrderBy;
        }
        DataSet Ds = new DataSet();

        Ds = Bind_Combo(lcSql, "MainAccount_Name1", "MainAccount_ReferenceID");

        ComboMainAccountE.DataSource = Ds;
        ComboMainAccountE.TextField = "MainAccount_Name1";
        ComboMainAccountE.ValueField = "MainAccount_ReferenceID";
        ComboMainAccountE.DataBind();


        int cindex = 0;
        foreach (var item in ComboMainAccountE.Items)
        {
            if (item.ToString() == Convert.ToString(row["MainAccount1"]))
            {
                break;
            }
            else
            {
                cindex = cindex + 1;
            }
        }



        ComboMainAccountE.SelectedIndex = cindex;


        //  Bind and select Sub Account

        ASPxComboBox ComboSubAccountE = (ASPxComboBox)DetailsGrid.FindEditFormTemplateControl("CmbSubAccount");
        var MainAccountCode = ComboMainAccountE.SelectedItem.Value;
        var SegID = "";
        var SegmentName = "";

        if (hdn_SegID_SegmentName.Value != "")
        {
            SegID = hdnSegmentid.Value;
            SegmentName = hdn_SegID_SegmentName.Value;
        }


        var ProcedureName = "SubAccountSelect_New";
        var InputName = "CashBank_MainAccountID|clause|branch|exchSegment|SegmentN";
        var InputType = "V|V|V|V|V";
        var InputValue = MainAccountCode.ToString().Split('~')[0] + "|RequestLetter|" + Session["userbranchHierarchy"] + "|'" + Session["ExchangeSegmentID"] + "'|'" + SegmentName + "'";
        var SplitChar = "|";
        var CombinedSubQuery = ProcedureName + "$" + InputName + "$" + InputType + "$" + InputValue + "$" + SplitChar;


        string[] paramSub = CombinedSubQuery.Split('$');


        char SplitSubChar = Convert.ToChar(paramSub[4]);
        string ProcedureSubName = Convert.ToString(paramSub[0]);
        string[] InputSubName = paramSub[1].Split(SplitSubChar);
        string[] InputSubType = paramSub[2].Split(SplitSubChar);
        string SetRequestLetter = paramSub[3].Replace("RequestLetter", RequestLetter);
        string[] InputSubValue = SetRequestLetter.Split(SplitSubChar);
        if (ProcedureSubName.Trim() != String.Empty && (InputSubName.Length == InputSubType.Length) && (InputSubType.Length == InputSubValue.Length))
        {
            DT = objEngine.SelectProcedureArr(ProcedureSubName, InputSubName, InputSubType, InputSubValue);
        }

        if (ComboSubAccountE.Items.Count > 0)
        {
            ComboSubAccountE.Items.Clear();
            ComboSubAccountE.Text = "";
        }

        ComboSubAccountE.DataSource = DT;
        ComboSubAccountE.TextField = DT.Columns[0].ColumnName;//"Contact_Name";
        ComboSubAccountE.ValueField = DT.Columns[1].ColumnName;// "SubAccount_ReferenceID";
        ComboSubAccountE.DataBind();


        int cSubindex = 0;
        foreach (var item in ComboSubAccountE.Items)
        {


            //if (item.ToString() == Convert.ToString(row["SubAccountName"]))
            if (item.ToString() == Convert.ToString(SubAccountE))
            {
                break;
            }
            else
            {
                cSubindex = cSubindex + 1;
            }
        }

        ComboSubAccountE.SelectedIndex = cSubindex;

        // combosunaccount populated and selected end        
        values.Clear();
        if (MainAccountE.Trim() == Convert.ToString(row["MainAccount1"]).Trim() && SubAccountE.Trim() == Convert.ToString(row["SubAccountName"]).Trim() && DebitE.Trim() == TxtDebitE.Text.Trim() && CreditE.Trim() == TxtCreditE.Text.Trim() && NarrationE.Trim() == strNarrationE.Trim() && LineNarrationE.Trim() == TxtLineNarrationE.Text.Trim())
        {
            values.Add("IsUpdating", "False");
        }
        else
        {
            values.Add("IsUpdating", "True");
            if (MainAccountE.Trim() == Convert.ToString(row["MainAccount1"]).Trim() && SubAccountE.Trim() == Convert.ToString(row["SubAccountName"]).Trim()) values.Add("MainAcOrSubAcChange", "False");
            else values.Add("MainAcOrSubAcChange", "True");
            values.Add("RecordID", KeyValue);
            values.Add("MainAccount", txtMainAccountE_hidden.Value != String.Empty ? ((txtMainAccountE_hidden.Value.ToString().Split('~').Length > 1) ? txtMainAccountE_hidden.Value.ToString().Split('~')[0] : txtMainAccountE_hidden.Value.ToString()) : String.Empty);
            values.Add("MainAccountText", txtMainAccountE_hidden.Value != String.Empty ? txtMainAccountE_hidden.Value : String.Empty);
            values.Add("SubAccount", txtSubAccountE_hidden.Value != String.Empty ? txtSubAccountE_hidden.Value.ToString() : String.Empty);
            values.Add("SubAccountText", txtSubAccountE_hidden.Value != String.Empty ? txtSubAccountE_hidden.Value : String.Empty);
            values.Add("BranchID", txtSubAccountE_hidden.Value != String.Empty ? ((txtSubAccountE_hidden.Value.Split('~').Length > 1) ? txtSubAccountE_hidden.Value.Split('~')[1] : "NAB") : BranchID);
            //values.Add("Debit", TxtDebitE.Text);
            //values.Add("Credit", TxtCreditE.Text);
            values.Add("Debit", DebitE);
            values.Add("Credit", CreditE);
            values.Add("Narration", TxtLineNarrationE.Text.Trim());
            values.Add("WhichRecordEdit", BranchNonBranchE);
            string[] MainAC = txtMainAccountE_hidden.Value.ToString().Split('~');
            if (txtMainAccountE_hidden.Value != String.Empty)
            {
                DataTable dtType = new DataTable();
                if (MainAC.Length > 1)
                    dtType = oDBEngine.GetDataTable("master_mainaccount", "ltrim(rtrim(mainaccount_SubLedgerType))", " mainaccount_ReferenceID=" + txtMainAccountE_hidden.Value.ToString().Split('~')[0] + "");
                else
                    dtType = oDBEngine.GetDataTable("master_mainaccount", "ltrim(rtrim(mainaccount_SubLedgerType))", " mainaccount_ReferenceID=" + txtMainAccountE_hidden.Value.ToString() + "");
                values.Add("Type", dtType.Rows[0][0].ToString());
            }
            else
            {
                values.Add("Type", "NotChange");
            }
        }


        if (ViewState["CancelClick"] != null)
        {
            ViewState["DebitWhenEdit"] = null;
            ViewState["CreditWhenEdit"] = null;
            ViewState["CancelClick"] = null;
        }
    }
    protected void DetailsGrid_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
    {
        if (values["IsUpdating"].Equals("False"))
        {
            e.Cancel = true;
            DetailsGrid.CancelEdit();
            return;
        }
        Boolean AllowDisAllow;
        DataSet DsUpdateXMLValid = new DataSet();
        String WhichRecordEdit = null;
        WhichRecordEdit = values["WhichRecordEdit"].ToString();
        if (IsNumeric(WhichRecordEdit))
        {
            if (WhichRecordEdit != values["BranchID"].ToString())
            {
                if (values["BranchID"].ToString() == "NAB")
                    AllowDisAllow = Convert.ToBoolean(AllowDisAllowEntryWhenNAB(values["BranchID"].ToString(), WhichRecordEdit).Split('~')[0]);
                else
                    AllowDisAllow = Convert.ToBoolean(AllowDisAllowEntryWhenBranch(values["BranchID"].ToString(), WhichRecordEdit).Split('~')[0]);
                if (AllowDisAllow)
                {
                    UpdateMainXML();
                    if (DsUpdateXMLValid.Tables.Count > 0) { DsUpdateXMLValid.Tables.Remove(DsUpdateXMLValid.Tables[0]); DsUpdateXMLValid.Clear(); }
                    DataRow row;
                    if (File.Exists(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH)))
                    {
                        DsUpdateXMLValid.ReadXml(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
                        DsUpdateXMLValid.Tables[0].PrimaryKey = new DataColumn[] { DsUpdateXMLValid.Tables[0].Columns["ID"] };
                        if (values["BranchID"].ToString() == "NAB")
                        {
                            row = DsUpdateXMLValid.Tables[0].Rows.Find("NB");
                            row["EntryCount"] = Convert.ToString((Convert.ToInt32(row["EntryCount"].ToString()) + 1));
                            row.AcceptChanges();
                            row = DsUpdateXMLValid.Tables[0].Rows.Find(WhichRecordEdit);
                            row["EntryCount"] = Convert.ToString((Convert.ToInt32(row["EntryCount"].ToString()) - 1));
                            row.AcceptChanges();
                            if (row["EntryCount"].ToString() == "0")
                            {
                                DsUpdateXMLValid.Tables[0].Rows.Remove(row);
                                DsUpdateXMLValid.Tables[0].Rows[0][1] = Convert.ToInt32(DsUpdateXMLValid.Tables[0].Rows[0][1].ToString()) - 1;
                                DsUpdateXMLValid.AcceptChanges();
                            }


                        }
                        else
                        {
                            row = DsUpdateXMLValid.Tables[0].Rows.Find(values["BranchID"].ToString());
                            if (row == null)
                            {
                                DsUpdateXMLValid.Tables[0].PrimaryKey = new DataColumn[] { DsUpdateXMLValid.Tables[0].Columns["ID"] };
                                row = DsUpdateXMLValid.Tables[0].Rows.Find(WhichRecordEdit);
                                row["EntryCount"] = Convert.ToString((Convert.ToInt32(row["EntryCount"].ToString()) - 1));
                                row.AcceptChanges();
                                if (row["EntryCount"].ToString() == "0")
                                {
                                    DsUpdateXMLValid.Tables[0].Rows.Remove(row);
                                    DsUpdateXMLValid.Tables[0].Rows[0][1] = Convert.ToInt32(DsUpdateXMLValid.Tables[0].Rows[0][1].ToString()) - 1;
                                }
                                DsUpdateXMLValid.AcceptChanges();
                                DsUpdateXMLValid.Tables[0].PrimaryKey = new DataColumn[] { DsUpdateXMLValid.Tables[0].Columns["CashReportID"] };
                                DataRow NewRow = DsUpdateXMLValid.Tables[0].NewRow();
                                NewRow["ID"] = values["BranchID"].ToString();
                                NewRow["EntryCount"] = "1";
                                NewRow["CashReportID"] = Convert.ToInt32(DsUpdateXMLValid.Tables[0].Rows[DsUpdateXMLValid.Tables[0].Rows.Count - 1]["CashReportID"].ToString()) + 1;
                                DsUpdateXMLValid.Tables[0].Rows.Add(NewRow);
                                DsUpdateXMLValid.Tables[0].Rows[0][1] = Convert.ToInt32(DsUpdateXMLValid.Tables[0].Rows[0][1].ToString()) + 1;
                                DsUpdateXMLValid.AcceptChanges();
                            }
                            else
                            {
                                row["EntryCount"] = Convert.ToString((Convert.ToInt32(row["EntryCount"].ToString()) + 1));
                                row.AcceptChanges();
                                row = DsUpdateXMLValid.Tables[0].Rows.Find(WhichRecordEdit);
                                row["EntryCount"] = Convert.ToString((Convert.ToInt32(row["EntryCount"].ToString()) - 1));
                                if (row["EntryCount"].ToString() == "0")
                                {
                                    DsUpdateXMLValid.Tables[0].Rows.Remove(row);
                                    DsUpdateXMLValid.AcceptChanges();
                                    DsUpdateXMLValid.Tables[0].Rows[0][1] = Convert.ToInt32(DsUpdateXMLValid.Tables[0].Rows[0][1].ToString()) - 1;
                                }

                            }
                        }
                        if (DsUpdateXMLValid.Tables.Count > 0)
                        {
                            if (DsUpdateXMLValid.Tables[0].Rows.Count > 0)
                            {
                                DsUpdateXMLValid.WriteXml(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
                            }
                        }
                    }
                    //This For Log Purpose
                    string strLogID = ViewState["LogID"].ToString();
                    //oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlUpdated, Session["UserID"].ToString(), "", "Trans_JournalVoucher", strLogID, GenericLogSystem.LogType.JV);
                    //oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlUpdated, Session["UserID"].ToString(), "", "Trans_JournalVoucherDetail", strLogID, GenericLogSystem.LogType.JV);
                }
            }
            else
            {
                UpdateMainXML();
                //This For Log Purpose
                string strLogID = ViewState["LogID"].ToString();
                //oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlUpdated, Session["UserID"].ToString(), "", "Trans_JournalVoucher", strLogID, GenericLogSystem.LogType.JV);
                //oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlUpdated, Session["UserID"].ToString(), "", "Trans_JournalVoucherDetail", strLogID, GenericLogSystem.LogType.JV);
            }
        }
        else if (WhichRecordEdit == "NAB")
        {
            if (WhichRecordEdit != values["BranchID"].ToString())
            {
                AllowDisAllow = Convert.ToBoolean(AllowDisAllowEntryWhenNAB(values["BranchID"].ToString(), WhichRecordEdit).Split('~')[0]);
                if (AllowDisAllow)
                {
                    UpdateMainXML();
                    if (DsUpdateXMLValid.Tables.Count > 0) { DsUpdateXMLValid.Tables.Remove(DsUpdateXMLValid.Tables[0]); DsUpdateXMLValid.Clear(); }
                    DsUpdateXMLValid.ReadXml(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
                    DsUpdateXMLValid.Tables[0].PrimaryKey = new DataColumn[] { DsUpdateXMLValid.Tables[0].Columns["ID"] };
                    DataRow row = DsUpdateXMLValid.Tables[0].Rows.Find(values["BranchID"].ToString());
                    if (row == null)
                    {
                        DsUpdateXMLValid.Tables[0].PrimaryKey = new DataColumn[] { DsUpdateXMLValid.Tables[0].Columns["CashReportID"] };
                        DataRow NewRow = DsUpdateXMLValid.Tables[0].NewRow();
                        NewRow["ID"] = values["BranchID"].ToString();
                        NewRow["EntryCount"] = "1";
                        NewRow["CashReportID"] = Convert.ToInt32(DsUpdateXMLValid.Tables[0].Rows[DsUpdateXMLValid.Tables[0].Rows.Count - 1]["CashReportID"].ToString()) + 1;
                        DsUpdateXMLValid.Tables[0].Rows.Add(NewRow);
                        DsUpdateXMLValid.Tables[0].Rows[1][1] = Convert.ToInt32(DsUpdateXMLValid.Tables[0].Rows[1][1].ToString()) - 1;
                        DsUpdateXMLValid.Tables[0].Rows[0][1] = Convert.ToInt32(DsUpdateXMLValid.Tables[0].Rows[0][1].ToString()) + 1;
                        DsUpdateXMLValid.AcceptChanges();
                    }
                    else
                    {
                        row["EntryCount"] = Convert.ToString((Convert.ToInt32(row["EntryCount"].ToString()) + 1));
                        row.AcceptChanges();
                        DsUpdateXMLValid.Tables[0].Rows[1][1] = Convert.ToInt32(DsUpdateXMLValid.Tables[0].Rows[1][1].ToString()) - 1;
                        DsUpdateXMLValid.AcceptChanges();
                    }
                }
                if (DsUpdateXMLValid.Tables.Count > 0)
                {
                    if (DsUpdateXMLValid.Tables[0].Rows.Count > 0)
                    {
                        DsUpdateXMLValid.WriteXml(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
                    }
                }
            }
            else
            {
                UpdateMainXML();
            }

        }
        TotalCredit = 0;
        TotalCredit = 0;
        DsUpdateXMLValid.Dispose();
        e.Cancel = true;
        DetailsGrid.CancelEdit();

    }
    void UpdateMainXML()
    {

        HiddenField txtMainAccountE_hidden = (HiddenField)DetailsGrid.FindEditFormTemplateControl("txtMainAccountE_hidden");
        HiddenField txtSubAccountE_hidden = (HiddenField)DetailsGrid.FindEditFormTemplateControl("txtSubAccountE_hidden");
        ASPxComboBox ComboMainAccountE = (ASPxComboBox)DetailsGrid.FindEditFormTemplateControl("CmbMainAccount");
        ASPxComboBox ComboSubAccountE = (ASPxComboBox)DetailsGrid.FindEditFormTemplateControl("CmbSubAccount");
        ASPxTextBox DebitAmt = (ASPxTextBox)DetailsGrid.FindEditFormTemplateControl("txtdebit");
        ASPxTextBox CreditAmt = (ASPxTextBox)DetailsGrid.FindEditFormTemplateControl("txtcredit");


        DataSet DsUpdateXML = new DataSet();
        if (DsUpdateXML.Tables.Count > 0) { DsUpdateXML.Tables.Remove(DsUpdateXML.Tables[0]); DsUpdateXML.Clear(); }
        DsUpdateXML.ReadXml(Server.MapPath(JournalVoucherFile_XMLPATH));
        DsUpdateXML.Tables[0].PrimaryKey = new DataColumn[] { DsUpdateXML.Tables[0].Columns["CashReportID"] };
        DataRow row = DsUpdateXML.Tables[0].Rows.Find(values["RecordID"]);
        if (values["BranchID"].ToString() == "NAB")
        {
            row["JournalVoucherDetail_BranchID"] = ddlBranch.SelectedValue;
            row["JournalVoucherDetail_SubAccountCode"] = values["SubAccount"].ToString() != String.Empty ? values["SubAccount"].ToString().Split('~')[0] : row["JournalVoucherDetail_SubAccountCode"].ToString();
        }
        else
        {
            row["JournalVoucherDetail_BranchID"] = values["BranchID"].ToString();
            row["JournalVoucherDetail_SubAccountCode"] = values["SubAccount"].ToString() != String.Empty ? values["SubAccount"].ToString().Split('~')[0] + '~' + values["SubAccount"].ToString().Split('~')[1] + '~' + values["SubAccount"].ToString().Split('~')[2] : row["JournalVoucherDetail_SubAccountCode"].ToString();
        }
        row["JournalVoucherDetail_MainAccountCode"] = values["MainAccount"].ToString() != String.Empty ? values["MainAccount"].ToString() : row["JournalVoucherDetail_MainAccountCode"].ToString();
        row["JournalVoucherDetail_AmountDr"] = DebitAmt.Text; //values["Debit"].ToString();
        row["JournalVoucherDetail_AmountCr"] = CreditAmt.Text;// values["Credit"].ToString();
        row["SubNarration"] = values["Narration"].ToString();
        row["MainAccount1"] = Convert.ToString(ComboMainAccountE.Text); //txtMainAccountE_hidden.Value; //values["MainAccountText"].ToString() != String.Empty ? values["MainAccountText"].ToString() : row["MainAccount1"].ToString();
        row["SubAccount1"] = Convert.ToString(ComboSubAccountE.Text);// txtSubAccountE_hidden.Value; // values["SubAccountText"].ToString() != String.Empty ? values["SubAccountText"].ToString() : row["SubAccount1"].ToString();
        row["WithDrawl"] = DebitAmt.Text;//values["Debit"].ToString();
        row["Receipt"] = CreditAmt.Text; //values["Credit"].ToString();
        row["Type"] = values["Type"].ToString() != String.Empty ? values["Type"].ToString() : row["Type"].ToString();
        row["BranchNonBranch"] = values["BranchID"].ToString() != String.Empty ? values["BranchID"].ToString() : row["BranchNonBranch"].ToString();
        if (values["MainAcOrSubAcChange"].ToString() == "True")
            row["OldJVVoucherNumber"] = "GenerateNew";
        row.AcceptChanges();
        DsUpdateXML.AcceptChanges();
        File.Delete(Server.MapPath(JournalVoucherFile_XMLPATH));
        DsUpdateXML.WriteXml(Server.MapPath(JournalVoucherFile_XMLPATH));
        DsUpdateXML.Dispose();
    }


    protected void DetailsGrid_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
    {
        DataSet DsCanCancelEdit = new DataSet();
        TotalCredit = 0;
        TotalCredit = 0;
        ViewState["CreditWhenEdit"] = null;
        ViewState["DebitWhenEdit"] = null;
        if (DsCanCancelEdit.Tables.Count > 0) { DsCanCancelEdit.Tables.Remove(DsCanCancelEdit.Tables[0]); DsCanCancelEdit.Clear(); }
        DsCanCancelEdit.ReadXml(Server.MapPath(JournalVoucherFile_XMLPATH));
        ViewState["CancelClick"] = "true";
        BindGrid(DetailsGrid, DsCanCancelEdit, "DESC");
        DsCanCancelEdit.Dispose();
    }
    protected void DetailsGrid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
    {
        if (e.RowType != GridViewRowType.Data) return;
        int rowindex = e.VisibleIndex;
        string RowDebitValue = DetailsGrid.GetRowValues(rowindex, "JournalVoucherDetail_AmountDr").ToString();
        string RowCreditValue = DetailsGrid.GetRowValues(rowindex, "JournalVoucherDetail_AmountCr").ToString();
        TotalDebit = TotalDebit + Convert.ToDecimal(RowDebitValue != String.Empty ? RowDebitValue : "0.00");
        TotalCredit = TotalCredit + Convert.ToDecimal(RowCreditValue != String.Empty ? RowCreditValue : "0.00");
    }
    protected void GvJvSearch_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        DataSet Ds_JVE;
        SqlDataAdapter Da_JVE;
        int RowIndex;
        string PCBCommandName = e.Parameters.Split('~')[0];
        string IBRef;
        string OldVoucherNumber = String.Empty;
        string OldWhichTypeOfItem = String.Empty;
        string VoucherNumber;
        string JVEFile_XMLPATH;
        if (PCBCommandName == "PCB_BtnShow")
        {
            BindGrid(GvJvSearch);
            Session["StrQuery"] = null;
            string StrQuery = " and JournalVoucher_TransactionDate='" + Convert.ToDateTime(tDate.Value).ToString("MM-dd-yyyy").Substring(0, 10) + "'";
            DsSearchCallBack = new DataSet();
            DaSearchCallBack = new SqlDataAdapter();
            if (ChkBranch.Checked) StrQuery = StrQuery + " and JournalVoucher_BranchID='" + ddlBranch.SelectedValue + "'";
            if (ChkBillNo.Checked) StrQuery = StrQuery + " and JournalVoucher_BillNumber='" + txtBillNo.Text + "'";
            if (ChkPrefix.Checked) StrQuery = StrQuery + " and JournalVoucher_Prefix='" + txtAccountCode.Text + "'";
            if (ChkNarration.Checked) StrQuery = StrQuery + " and JournalVoucher_Narration='" + txtNarration.Text + "'";
            Session["StrQuery"] = StrQuery;
            FillSearchGrid();
            GvJvSearch.JSProperties["cpJVE_FileAlreadyUsedBy"] = null;
            GvJvSearch.JSProperties["cpEntryEventFire"] = null;
            GvJvSearch.JSProperties["cpJVDelete"] = null;
            GvJvSearch.JSProperties["cpJVClose"] = null;
        }
        if (PCBCommandName == "PCB_EditEnd")
        {
            BindGrid(GvJvSearch);
            RowIndex = Convert.ToInt32(e.Parameters.Split('~')[1]);
            IBRef = GvJvSearch.GetRowValues(RowIndex, "IBRef").ToString();
            JVEFile_XMLPATH = "../Documents/" + "JVE_" + IBRef;
            if (Session["IBRef"] != null)
            {
                if (File.Exists(Server.MapPath(JVEFile_XMLPATH)))
                {
                    File.Delete(Server.MapPath(JVEFile_XMLPATH));
                    File.Delete(Server.MapPath(JVEFile_XMLPATH + "Validate"));
                }
            }
            Session["StrQuery"] = null;
            Session["IBRef"] = null;
            GvJvSearch.Visible = false;
            GvJvSearch.JSProperties["cpJVE_FileAlreadyUsedBy"] = null;
            GvJvSearch.JSProperties["cpEntryEventFire"] = null;
            GvJvSearch.JSProperties["cpJVDelete"] = null;
            GvJvSearch.JSProperties["cpJVClose"] = null;

        }
        if (PCBCommandName == "PCB_BtnOkE")
        {
            RowIndex = Convert.ToInt32(e.Parameters.Split('~')[1]);

            ////////////////////////////////////////////////////////////////////////////////////////////////

            IBRef = GvJvSearch.GetRowValues(RowIndex, "IBRef").ToString();
            JVEFile_XMLPATH = "../Documents/" + "JVE_" + IBRef;
            if (File.Exists(Server.MapPath(JVEFile_XMLPATH)))
            {
                File.Delete(Server.MapPath(JVEFile_XMLPATH));
                File.Delete(Server.MapPath(JVEFile_XMLPATH + "Validate"));
                //This For Log Purpose
                string strLogID = ViewState["LogID"].ToString();
            }
            Session["StrQuery"] = null;
            Session["IBRef"] = null;
            GvJvSearch.JSProperties["cpJVClose"] = "File Successfully Close";
            GvJvSearch.JSProperties["cpJVE_FileAlreadyUsedBy"] = null;
            GvJvSearch.JSProperties["cpEntryEventFire"] = null;
            GvJvSearch.JSProperties["cpJVDelete"] = null;
            Session["IBRef"] = null;
            Session["OldJVVoucherNumber"] = null;
            Session["OldWhichTypeItem"] = null;

            //////////////////////////////////////////////////////////////////////////////////////////////

            IBRef = GvJvSearch.GetRowValues(RowIndex, "IBRef").ToString();
            OldWhichTypeOfItem = GvJvSearch.GetRowValues(RowIndex, "WhichTypeItem").ToString();
            VoucherNumber = GvJvSearch.GetRowValues(RowIndex, "VoucherNumber").ToString();
            //VoucherNumber = GvJvSearch.GetRowValues(RowIndex, "BillNumber").ToString();

            CreateJVE_XMLFile(IBRef, VoucherNumber);
            JournalVoucherFile_XMLPATH = JVEFile_XMLPATH;

            grid.DataSource = GetVoucher();
            grid.DataBind();

            if (ViewState["JVE_FileAlreadyUsedBy"] != null)
            {
                if (ViewState["JVE_FileAlreadyUsedBy"].ToString().Split('~')[1] == Session["userid"].ToString())
                {
                    GvJvSearch.JSProperties["cpJVE_FileAlreadyUsedBy"] = "HimSelef~";
                }
                else
                {
                    GvJvSearch.JSProperties["cpJVE_FileAlreadyUsedBy"] = "Other~" + ViewState["JVE_FileAlreadyUsedBy"].ToString();
                }
                GvJvSearch.JSProperties["cpEntryEventFire"] = null;
            }
            else
            {
                BindGrid(GvJvSearch);
                GvJvSearch.JSProperties["cpJVE_FileAlreadyUsedBy"] = null;
                GvJvSearch.JSProperties["cpEntryEventFire"] = "true";
                //This For Log Purpose
                string strLogID = ViewState["LogID"].ToString();
                //oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlOpenForEdit, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucher", strLogID, GenericLogSystem.LogType.JV);
                // oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlOpenForEdit, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucherDetail", strLogID, GenericLogSystem.LogType.JV);
            }
            GvJvSearch.JSProperties["cpJVDelete"] = null;
            GvJvSearch.JSProperties["cpJVClose"] = null;
            Session["IBRef"] = IBRef;
            Session["OldJVVoucherNumber"] = VoucherNumber;
            Session["OldWhichTypeItem"] = OldWhichTypeOfItem;
        }
        if (PCBCommandName == "PCB_DeleteBtnOkE")
        {
            int RowUpdated = 0;
            RowIndex = Convert.ToInt32(e.Parameters.Split('~')[1]);
            IBRef = GvJvSearch.GetRowValues(RowIndex, "IBRef").ToString();
            VoucherNumber = GvJvSearch.GetRowValues(RowIndex, "VoucherNumber").ToString();
            //This For Log Purpose
            string strLogID = ViewState["LogID"].ToString();
            //oGenericLogSystem.CreateLog("Trans_JournalVoucher", "JournalVoucher_IBRef='" + IBRef + "'", GenericLogSystem.LogState.XmlDeleting, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucher", strLogID, GenericLogSystem.LogType.JV);
            //  oGenericLogSystem.CreateLog("Trans_JournalVoucherDetail", "JournalVoucherDetail_IBRef='" + IBRef + "'", GenericLogSystem.LogState.XmlDeleting, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucherDetail", strLogID, GenericLogSystem.LogType.JV);

            RowUpdated = oDailyTaskOther.Delete_JV(
        Convert.ToString(IBRef),
        Convert.ToString(VoucherNumber),Convert.ToInt32( Session["userid"])
          );
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{
            //    using (SqlCommand com = new SqlCommand("Delete_JV", con))
            //    {
            //        com.CommandType = CommandType.StoredProcedure;
            //        com.Parameters.AddWithValue("@IBRef", IBRef);
            //        com.Parameters.AddWithValue("@VoucherNumber", VoucherNumber);
            //        con.Open();
            //        RowUpdated=com.ExecuteNonQuery();
            //        con.Close();
            //    }
            //}
            if (RowUpdated > 0)
            {
                GvJvSearch.JSProperties["cpJVDelete"] = "Successfully Deleted";
                //This For Log Purpose
                //oGenericLogSystem.CreateLog("Trans_JournalVoucher", "JournalVoucher_IBRef='" + IBRef + "'", GenericLogSystem.LogState.XmlDeleted, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucher", strLogID, GenericLogSystem.LogType.JV);
                //oGenericLogSystem.CreateLog("Trans_JournalVoucherDetail", "JournalVoucherDetail_IBRef='" + IBRef + "'", GenericLogSystem.LogState.XmlDeleted, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucherDetail", strLogID, GenericLogSystem.LogType.JV);
            }
            else
            {
                GvJvSearch.JSProperties["cpJVDelete"] = "Problem in Deleting.Sry for Inconvenience";
                //This For Log Purpose
                // oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlError, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucher", strLogID, GenericLogSystem.LogType.JV);
                // oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlError, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucherDetail", strLogID, GenericLogSystem.LogType.JV);
            }
            GvJvSearch.JSProperties["cpJVE_FileAlreadyUsedBy"] = null;
            GvJvSearch.JSProperties["cpEntryEventFire"] = null;
        }
        if (PCBCommandName == "PCB_BindAfterDelete")
        {
            FillSearchGrid();
            GvJvSearch.JSProperties["cpJVE_FileAlreadyUsedBy"] = null;
            GvJvSearch.JSProperties["cpEntryEventFire"] = null;
            GvJvSearch.JSProperties["cpJVDelete"] = null;
            GvJvSearch.JSProperties["cpJVClose"] = null;
        }
        if (PCBCommandName == "PCB_ContinueWith")
        {
            RowIndex = Convert.ToInt32(e.Parameters.Split('~')[1]);
            IBRef = GvJvSearch.GetRowValues(RowIndex, "IBRef").ToString();
            VoucherNumber = GvJvSearch.GetRowValues(RowIndex, "VoucherNumber").ToString();
            GvJvSearch.JSProperties["cpJVE_FileAlreadyUsedBy"] = null;
            GvJvSearch.JSProperties["cpEntryEventFire"] = null;
            GvJvSearch.JSProperties["cpJVDelete"] = null;
            GvJvSearch.JSProperties["cpJVClose"] = null;
            GvJvSearch.JSProperties["cpEntryEventFire"] = "true";
            Session["IBRef"] = IBRef;
            //This For Log Purpose
            string strLogID = ViewState["LogID"].ToString();
            // oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlOpenForEdit, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucher", strLogID, GenericLogSystem.LogType.JV);
            //oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlOpenForEdit, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucherDetail", strLogID, GenericLogSystem.LogType.JV);
        }
        if (PCBCommandName == "PCB_FreshEntry")
        {
            RowIndex = Convert.ToInt32(e.Parameters.Split('~')[1]);
            IBRef = GvJvSearch.GetRowValues(RowIndex, "IBRef").ToString();
            VoucherNumber = GvJvSearch.GetRowValues(RowIndex, "VoucherNumber").ToString();
            JVEFile_XMLPATH = "../Documents/" + "JVE_" + IBRef;
            Ds_JVE = new DataSet();
            Ds_JVE = oDailyTaskOther.Fetch_JVE_DataSet(
     Convert.ToString(ViewState["SegmentName"]),
     Convert.ToString(Session["userid"]),
       Convert.ToString(VoucherNumber),
         Convert.ToString(IBRef),
           Convert.ToString(Session["TradeCurrency"].ToString().Split('~')[0]),
             Convert.ToString(Session["LastCompany"])
       );
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{
            //    using (SqlCommand com = new SqlCommand("Fetch_JVE_DataSet", con))
            //    {
            //        com.CommandType = CommandType.StoredProcedure;
            //        com.Parameters.AddWithValue("@CrgExchange", ViewState["SegmentName"].ToString());
            //        com.Parameters.AddWithValue("@UserID", Session["userid"].ToString());
            //        com.Parameters.AddWithValue("@VoucherNumber", VoucherNumber);
            //        com.Parameters.AddWithValue("@IBRef", IBRef);
            //        com.Parameters.AddWithValue("@TradeCurrency", Session["TradeCurrency"].ToString().Split('~')[0]);
            //        com.Parameters.AddWithValue("@CompanyID", Session["LastCompany"].ToString());


            //        using (Da_JVE = new SqlDataAdapter(com))
            //        {
            //            Ds_JVE.Clear();
            //            Da_JVE.Fill(Ds_JVE);
            //        }
            //    }
            //}
            if (Ds_JVE.Tables.Count > 0)
            {
                if (Ds_JVE.Tables[0].Rows.Count > 0)
                {
                    if (File.Exists(Server.MapPath(JVEFile_XMLPATH)))
                    {
                        File.Delete(Server.MapPath(JVEFile_XMLPATH));
                        File.Delete(Server.MapPath(JVEFile_XMLPATH + "Validate"));
                    }
                    Ds_JVE.Tables[0].TableName = "DtJournalVoucher";
                    Ds_JVE.WriteXml(Server.MapPath(JVEFile_XMLPATH));
                    //This For Log Purpose
                    string strLogID = ViewState["LogID"].ToString();
                    //oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlOpenForEdit, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucher", strLogID, GenericLogSystem.LogType.JV);
                    //oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlOpenForEdit, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucherDetail", strLogID, GenericLogSystem.LogType.JV);

                }
            }
            GvJvSearch.JSProperties["cpJVE_FileAlreadyUsedBy"] = null;
            GvJvSearch.JSProperties["cpEntryEventFire"] = true;
            GvJvSearch.JSProperties["cpJVDelete"] = null;
            GvJvSearch.JSProperties["cpJVClose"] = null;
            Session["IBRef"] = IBRef;

        }
        if (PCBCommandName == "PCB_CloseEntry")
        {
            RowIndex = Convert.ToInt32(e.Parameters.Split('~')[1]);
            IBRef = GvJvSearch.GetRowValues(RowIndex, "IBRef").ToString();
            JVEFile_XMLPATH = "../Documents/" + "JVE_" + IBRef;
            if (File.Exists(Server.MapPath(JVEFile_XMLPATH)))
            {
                File.Delete(Server.MapPath(JVEFile_XMLPATH));
                File.Delete(Server.MapPath(JVEFile_XMLPATH + "Validate"));
                //This For Log Purpose
                string strLogID = ViewState["LogID"].ToString();
                // oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlExit, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucher", strLogID, GenericLogSystem.LogType.JV);
                // oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlExit, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucherDetail", strLogID, GenericLogSystem.LogType.JV);
            }
            Session["StrQuery"] = null;
            Session["IBRef"] = null;
            GvJvSearch.JSProperties["cpJVClose"] = "File Successfully Close";
            GvJvSearch.JSProperties["cpJVE_FileAlreadyUsedBy"] = null;
            GvJvSearch.JSProperties["cpEntryEventFire"] = null;
            GvJvSearch.JSProperties["cpJVDelete"] = null;
            Session["IBRef"] = null;
            Session["OldJVVoucherNumber"] = null;
            Session["OldWhichTypeItem"] = null;
        }



    }
    protected void GvJvSearch_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
    {

    }
    protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 Filter = int.Parse(drdExport.SelectedItem.Value.ToString());
        exporter.GridViewID = "DetailsGrid";
        exporter.FileName = "JournalVoucherDetails";
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
    protected void drdExport1_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 Filter = int.Parse(Convert.ToString(drdExport1.SelectedItem.Value));
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
        GvJvSearch.Columns[11].Visible = false;
        string filename = "JournalVoucher";
        exporter.FileName = filename;
        exporter.GridViewID = "GvJvSearch";

        exporter.PageHeader.Left = "Journal Voucher";
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
    protected void DetailsGrid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
    {
        if (Session["LCKJV"] != null)
        {
            DateTime LockDate = Convert.ToDateTime(Session["LCKJV"].ToString());
            if (Convert.ToDateTime(tDate.Value) <= LockDate)
            {
                e.Visible = false;
                DetailsGrid.JSProperties["cpHideAddBtnOnLock"] = "false";
            }
            else
            {
                DetailsGrid.JSProperties["cpHideAddBtnOnLock"] = "true";
            }
        }
    }
    //Currency Setting
    protected void CbpChoosenCurrency_Callback(object source, CallbackEventArgsBase e)
    {
        CbpChoosenCurrency.JSProperties["cpChangeCurrencyParam"] = null;

        string WhichCall = e.Parameter.Split('~')[0];
        if (WhichCall == "ChangeCurrency")
        {
            string ActiveCurrencyID = String.Empty;
            string ActiveCurrencyName = String.Empty;
            string ActiveCurrencySymbol = String.Empty;
            string CurrencyInXmlFile = String.Empty;
            string ActiveCurrency = Session["ActiveCurrency"].ToString();
            string TradeCurrency = Session["TradeCurrency"].ToString();
            string LocalCurrency = Session["LocalCurrency"].ToString();

            //Change ChoosenCurrency In XmlFile if Exists
            if (File.Exists(Server.MapPath(JournalVoucherFile_XMLPATH)))
            {
                DataSet DsChangeChooseCurrency = new DataSet();
                DsChangeChooseCurrency.ReadXml(Server.MapPath(JournalVoucherFile_XMLPATH));
                CurrencyInXmlFile = DsChangeChooseCurrency.Tables[0].Rows[0]["ChoosenCurrency"].ToString();
                if (CurrencyInXmlFile != ActiveCurrency)
                {
                    if (ActiveCurrency == TradeCurrency)
                    {
                        ActiveCurrencyID = TradeCurrency.Split('~')[0];
                        ActiveCurrencyName = TradeCurrency.Split('~')[1];
                        ActiveCurrencySymbol = TradeCurrency.Split('~')[2];
                        Session["ActiveCurrency"] = TradeCurrency;
                    }
                    else
                    {
                        ActiveCurrencyID = LocalCurrency.Split('~')[0];
                        ActiveCurrencyName = LocalCurrency.Split('~')[1];
                        ActiveCurrencySymbol = LocalCurrency.Split('~')[2];
                        Session["ActiveCurrency"] = LocalCurrency;
                    }
                    for (int i = 0; i < DsChangeChooseCurrency.Tables[0].Rows.Count; i++)
                    {
                        DsChangeChooseCurrency.Tables[0].Rows[i]["ChoosenCurrency"] = Session["ActiveCurrency"].ToString();
                    }
                    DsChangeChooseCurrency.AcceptChanges();
                    File.Delete(Server.MapPath(JournalVoucherFile_XMLPATH));
                    DsChangeChooseCurrency.WriteXml(Server.MapPath(JournalVoucherFile_XMLPATH));
                    DsChangeChooseCurrency.Dispose();
                    CbpChoosenCurrency.JSProperties["cpChangeCurrencyParam"] = ActiveCurrencyName + '~' + ActiveCurrencySymbol;
                }
                else
                {
                    CbpChoosenCurrency.JSProperties["cpChangeCurrencyParam"] = CurrencyInXmlFile.Split('~')[1] + '~' + CurrencyInXmlFile.Split('~')[2];

                }
            }
            else
            {
                if (ActiveCurrency == TradeCurrency)
                {
                    ActiveCurrencyID = TradeCurrency.Split('~')[0];
                    ActiveCurrencyName = TradeCurrency.Split('~')[1];
                    ActiveCurrencySymbol = TradeCurrency.Split('~')[2];
                    Session["ActiveCurrency"] = TradeCurrency;
                }
                else
                {
                    ActiveCurrencyID = LocalCurrency.Split('~')[0];
                    ActiveCurrencyName = LocalCurrency.Split('~')[1];
                    ActiveCurrencySymbol = LocalCurrency.Split('~')[2];
                    Session["ActiveCurrency"] = LocalCurrency;
                }
                CbpChoosenCurrency.JSProperties["cpChangeCurrencyParam"] = ActiveCurrencyName + '~' + ActiveCurrencySymbol;
            }
        }
    }
    protected void CbpAcBalance_Callback(object source, CallbackEventArgsBase e)
    {
        CbpAcBalance.JSProperties["cpAcBalance"] = "undefined";
        string command = e.Parameter.Split('~')[0];
        if (command == "AcBalance")
        {
            string MainAccountID = e.Parameter.Split('~')[1];
            string SubAccountID = e.Parameter.Split('~')[2] != String.Empty ? e.Parameter.Split('~')[2] : null;
            string[,] MainAcCode = oDBEngine.GetFieldValue("master_mainaccount", "MainAccount_Accountcode", "mainaccount_referenceid=" + MainAccountID + "", 1);
            DateTime FinYearEnd = Convert.ToDateTime(Session["FinYearEnd"].ToString()); //Session["FinYearEnd"].ToString();
            DataTable DtCurrentSegment = oDBEngine.GetDataTable("(select exch_internalId, isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Segment from tbl_master_companyExchange where exch_compid in (select top 1 ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + Session["userlastsegment"].ToString() + " and ls_userid=" + Session["UserID"].ToString() + ") and exch_compId='" + HttpContext.Current.Session["LastCompany"].ToString() + "') as D", "*", " Segment in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ")");
            String CurrentSegment = DtCurrentSegment.Rows[0][0].ToString();
            DataTable DtAcBalance = oDBEngine.OpeningBalanceJournal1("'" + MainAcCode[0, 0] + "'", SubAccountID, FinYearEnd, CurrentSegment,
                Session["LastCompany"].ToString(), FinYearEnd, Convert.ToInt32(ChoosenCurrency.Split('~')[0]));
            Converter oConverter = new Converter();
            if (DtAcBalance.Rows.Count > 0 && DtAcBalance.Rows[0][0].ToString() != String.Empty)
            {
                string strColor = null;
                string strDrCr = null;
                strColor = (Convert.ToDecimal(DtAcBalance.Rows[0][0].ToString()) > 0) ? "Blue" : "Red";
                strDrCr = (Convert.ToDecimal(DtAcBalance.Rows[0][0].ToString()) > 0) ? " Dr." : " Cr.";
                CbpAcBalance.JSProperties["cpAcBalance"] = oConverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(DtAcBalance.Rows[0][0].ToString())) + strDrCr + '~' + strColor;
            }
            else
            {
                CbpAcBalance.JSProperties["cpAcBalance"] = "undefined";
            }

        }
    }
    //////////
    protected void CmbMainAccount_Callback(object source, CallbackEventArgsBase e)
    {

        string WhichCall = e.Parameter.Split('|')[0];
        string MainAccountSelectedValue = e.Parameter.Split('|')[1];
        string MainAccountSelectedItem = e.Parameter.Split('|')[2];


        DetailsGrid.JSProperties["cpShowDetailPopUpDirectlyForContra"] = null;
        int rowindex = DetailsGrid.EditingRowVisibleIndex;
        string VoucherType = DetailsGrid.GetRowValues(rowindex, "VoucherType").ToString();
        DataSet DsGvAddRecordXML = new DataSet();
        string KeyValue = DetailsGrid.GetRowValues(rowindex, "RecordID").ToString();
        DsGvAddRecordXML.ReadXml(Server.MapPath(JournalVoucherFile_XMLPATH));
        DsGvAddRecordXML.Tables[0].PrimaryKey = new DataColumn[] { DsGvAddRecordXML.Tables[0].Columns["RecordID"] };
        DataRow row = DsGvAddRecordXML.Tables[0].Rows.Find(KeyValue);
        HiddenField txtMainAccountE_hidden = (HiddenField)DetailsGrid.FindEditFormTemplateControl("txtMainAccountE_hidden");
        HiddenField txtSubAccountE_hidden = (HiddenField)DetailsGrid.FindEditFormTemplateControl("txtSubAccountE_hidden");
        //  ListBox lstMainAccountE = (ListBox)GvAddRecordDisplay.FindEditFormTemplateControl("lstMainAccountE");




        //combomainaccount populated and selected kaushik 7-12-2016
        ASPxComboBox ComboMainAccountE = (ASPxComboBox)DetailsGrid.FindEditFormTemplateControl("CmbMainAccount");


        var strQuery_Table = "Master_MainAccount";
        var strQuery_FieldName = "  MainAccount_Name+\' [ \'+rtrim(ltrim(MainAccount_AccountCode))+\' ]\' as MainAccount_Name1,cast(MainAccount_ReferenceID as varchar)+\'~\'+MainAccount_SubLedgerType+\'~MAINAC~\'+MainAccount_AccountType as MainAccount_ReferenceID";
        var strQuery_WhereClause = " (MainAccount_Name like (\'%RequestLetter%\') or MainAccount_AccountCode like (\'%RequestLetter%\')) and isnull(MainAccount_BankCashType,'abc') not in('Cash','Bank')";
        var strQuery_OrderBy = "";
        var strQuery_GroupBy = "";
        var CombinedQuery = strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy;



        List<string> obj = new List<string>();
        DataTable DT = new DataTable();
        DBEngine objEngine = new DBEngine();
        string RequestLetter = "%";
        string[] param = CombinedQuery.Replace("--", "+").Replace("^^", "%").Split('$');
        string Query_Table = param[0].Trim() != String.Empty ? param[0] : null;
        string Query_FieldName = param[1].Trim() != String.Empty ? param[1] : null;
        string Query_WhereClause = param[2].Trim() != String.Empty ? param[2] : null;
        string Query_OrderBy = param[3].Trim() != String.Empty ? param[3] : null;
        string Query_GroupBy = param[4].Trim() != String.Empty ? param[4] : null;
        if (Query_Table != null)
        {
            Query_Table = Query_Table.Replace("RequestLetter", RequestLetter);
        }
        if (Query_FieldName != null)
        {
            Query_FieldName = Query_FieldName.Replace("RequestLetter", RequestLetter);
        }
        if (Query_WhereClause != null)
        {
            Query_WhereClause = Query_WhereClause.Replace("RequestLetter", RequestLetter);
        }



        String lcSql;
        lcSql = "Select " + Query_FieldName + " from " + Query_Table;
        if (Query_WhereClause != null)
        {
            lcSql += " WHERE " + Query_WhereClause;
        }
        if (Query_GroupBy != null)
        {
            lcSql += " group By " + Query_GroupBy;
        }
        if (Query_OrderBy != null)
        {
            lcSql += " Order By " + Query_OrderBy;
        }
        DataSet Ds = new DataSet();

        Ds = Bind_Combo(lcSql, "MainAccount_Name1", "MainAccount_ReferenceID");

        ComboMainAccountE.DataSource = Ds;
        ComboMainAccountE.TextField = "MainAccount_Name1";
        ComboMainAccountE.ValueField = "MainAccount_ReferenceID";
        ComboMainAccountE.DataBind();


        int cindex = 0;
        foreach (var item in ComboMainAccountE.Items)
        {
            if (item.ToString() == Convert.ToString(MainAccountSelectedItem))
            {
                break;
            }
            else
            {
                cindex = cindex + 1;
            }
        }



        ComboMainAccountE.SelectedIndex = cindex;



        // combomainaccount populated and selected kaushik 7-12-2016  end


        //  combosubaccount populated and selected kaushik 7-12-2016
        ASPxComboBox ComboSubAccountE = (ASPxComboBox)DetailsGrid.FindEditFormTemplateControl("CmbSubAccount");
        ComboSubAccountE.Items.Clear();

        var MainAccountCode = MainAccountSelectedValue;
        var SegID = "";
        var SegmentName = "";

        if (hdn_SegID_SegmentName.Value != "")
        {
            SegID = hdn_SegID_SegmentName.Value.Split('~')[0];
            SegmentName = hdn_SegID_SegmentName.Value.Split('~')[1];
        }


        var ProcedureName = "SubAccountSelect_New";
        var InputName = "CashBank_MainAccountID|clause|branch|exchSegment|SegmentN";
        var InputType = "V|V|V|V|V";
        var InputValue = MainAccountCode.ToString().Split('~')[0] + "|RequestLetter|" + Session["userbranchHierarchy"] + "|'" + Session["ExchangeSegmentID"] + "'|'" + SegmentName + "'";
        var SplitChar = "|";
        var CombinedSubQuery = ProcedureName + "$" + InputName + "$" + InputType + "$" + InputValue + "$" + SplitChar;


        string[] paramSub = CombinedSubQuery.Split('$');


        char SplitSubChar = Convert.ToChar(paramSub[4]);
        string ProcedureSubName = Convert.ToString(paramSub[0]);
        string[] InputSubName = paramSub[1].Split(SplitSubChar);
        string[] InputSubType = paramSub[2].Split(SplitSubChar);
        string SetRequestLetter = paramSub[3].Replace("RequestLetter", RequestLetter);
        string[] InputSubValue = SetRequestLetter.Split(SplitSubChar);
        if (ProcedureSubName.Trim() != String.Empty && (InputSubName.Length == InputSubType.Length) && (InputSubType.Length == InputSubValue.Length))
        {
            DT = objEngine.SelectProcedureArr(ProcedureSubName, InputSubName, InputSubType, InputSubValue);
            //if (DT.Rows.Count != 0)
            //{
            //    foreach (DataRow dr in DT.Rows)
            //    {

            //        obj.Add(Convert.ToString(dr["Contact_Name"]) + "|" + Convert.ToString(dr["Column1"]));
            //    }
            //}

        }


        if (ComboSubAccountE.Items.Count > 0)
        {
            ComboSubAccountE.Items.Clear();
            ComboSubAccountE.Text = "";
        }
        ComboSubAccountE.DataSource = DT;
        ComboSubAccountE.TextField = DT.Columns[0].ColumnName;//"Contact_Name";
        ComboSubAccountE.ValueField = DT.Columns[1].ColumnName;//"SubAccount_ReferenceID";
        ComboSubAccountE.DataBind();


        // combosunaccount populated and selected end

    }
    protected void CmbSubAccount_Callback(object source, CallbackEventArgsBase e)
    {

        string WhichCall = e.Parameter.Split('|')[0];
        string MainAccountSelectedValue = e.Parameter.Split('|')[1];
        string MainAccountSelectedItem = e.Parameter.Split('|')[2];


        DetailsGrid.JSProperties["cpShowDetailPopUpDirectlyForContra"] = null;
        int rowindex = DetailsGrid.EditingRowVisibleIndex;
        //  string VoucherType = DetailsGrid.GetRowValues(rowindex, "VoucherType").ToString();
        DataSet DsGvAddRecordXML = new DataSet();
        string KeyValue = DetailsGrid.GetRowValues(rowindex, "CashReportID").ToString();
        DsGvAddRecordXML.ReadXml(Server.MapPath(JournalVoucherFile_XMLPATH));
        DsGvAddRecordXML.Tables[0].PrimaryKey = new DataColumn[] { DsGvAddRecordXML.Tables[0].Columns["CashReportID"] };
        DataRow row = DsGvAddRecordXML.Tables[0].Rows.Find(KeyValue);
        HiddenField txtMainAccountE_hidden = (HiddenField)DetailsGrid.FindEditFormTemplateControl("txtMainAccountE_hidden");
        HiddenField txtSubAccountE_hidden = (HiddenField)DetailsGrid.FindEditFormTemplateControl("txtSubAccountE_hidden");
        //  ListBox lstMainAccountE = (ListBox)GvAddRecordDisplay.FindEditFormTemplateControl("lstMainAccountE");




        //combomainaccount populated and selected kaushik 7-12-2016
        ASPxComboBox ComboMainAccountE = (ASPxComboBox)DetailsGrid.FindEditFormTemplateControl("CmbMainAccount");


        var strQuery_Table = "Master_MainAccount";
        var strQuery_FieldName = "  MainAccount_Name+\' [ \'+rtrim(ltrim(MainAccount_AccountCode))+\' ]\' as MainAccount_Name1,cast(MainAccount_ReferenceID as varchar)+\'~\'+MainAccount_SubLedgerType+\'~MAINAC~\'+MainAccount_AccountType as MainAccount_ReferenceID";
        var strQuery_WhereClause = " (MainAccount_Name like (\'%RequestLetter%\') or MainAccount_AccountCode like (\'%RequestLetter%\')) and isnull(MainAccount_BankCashType,'abc') not in('Cash','Bank')";
        var strQuery_OrderBy = "";
        var strQuery_GroupBy = "";
        var CombinedQuery = strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy;



        List<string> obj = new List<string>();
        DataTable DT = new DataTable();
        DBEngine objEngine = new DBEngine();
        string RequestLetter = "%";
        string[] param = CombinedQuery.Replace("--", "+").Replace("^^", "%").Split('$');
        string Query_Table = param[0].Trim() != String.Empty ? param[0] : null;
        string Query_FieldName = param[1].Trim() != String.Empty ? param[1] : null;
        string Query_WhereClause = param[2].Trim() != String.Empty ? param[2] : null;
        string Query_OrderBy = param[3].Trim() != String.Empty ? param[3] : null;
        string Query_GroupBy = param[4].Trim() != String.Empty ? param[4] : null;
        if (Query_Table != null)
        {
            Query_Table = Query_Table.Replace("RequestLetter", RequestLetter);
        }
        if (Query_FieldName != null)
        {
            Query_FieldName = Query_FieldName.Replace("RequestLetter", RequestLetter);
        }
        if (Query_WhereClause != null)
        {
            Query_WhereClause = Query_WhereClause.Replace("RequestLetter", RequestLetter);
        }



        String lcSql;
        lcSql = "Select " + Query_FieldName + " from " + Query_Table;
        if (Query_WhereClause != null)
        {
            lcSql += " WHERE " + Query_WhereClause;
        }
        if (Query_GroupBy != null)
        {
            lcSql += " group By " + Query_GroupBy;
        }
        if (Query_OrderBy != null)
        {
            lcSql += " Order By " + Query_OrderBy;
        }
        DataSet Ds = new DataSet();

        Ds = Bind_Combo(lcSql, "MainAccount_Name1", "MainAccount_ReferenceID");

        ComboMainAccountE.DataSource = Ds;
        ComboMainAccountE.TextField = "MainAccount_Name1";
        ComboMainAccountE.ValueField = "MainAccount_ReferenceID";
        ComboMainAccountE.DataBind();


        int cindex = 0;
        foreach (var item in ComboMainAccountE.Items)
        {
            if (item.ToString() == Convert.ToString(MainAccountSelectedItem))
            {
                break;
            }
            else
            {
                cindex = cindex + 1;
            }
        }



        ComboMainAccountE.SelectedIndex = cindex;



        // combomainaccount populated and selected kaushik 7-12-2016  end


        //  combosubaccount populated and selected kaushik 7-12-2016
        ASPxComboBox ComboSubAccountE = (ASPxComboBox)DetailsGrid.FindEditFormTemplateControl("CmbSubAccount");
        ComboSubAccountE.Items.Clear();

        var MainAccountCode = MainAccountSelectedValue;
        var SegID = "";
        var SegmentName = "";

        if (hdn_SegID_SegmentName.Value != "")
        {
            SegID = hdnSegmentid.Value;   // hdn_SegID_SegmentName.Value.Split('~')[0];
            SegmentName = hdn_SegID_SegmentName.Value;  //hdn_SegID_SegmentName.Value.Split('~')[1];
        }


        var ProcedureName = "SubAccountSelect_New";
        var InputName = "CashBank_MainAccountID|clause|branch|exchSegment|SegmentN";
        var InputType = "V|V|V|V|V";
        var InputValue = MainAccountCode.ToString().Split('~')[0] + "|RequestLetter|" + Session["userbranchHierarchy"] + "|'" + Session["ExchangeSegmentID"] + "'|'" + SegmentName + "'";
        var SplitChar = "|";
        var CombinedSubQuery = ProcedureName + "$" + InputName + "$" + InputType + "$" + InputValue + "$" + SplitChar;


        string[] paramSub = CombinedSubQuery.Split('$');


        char SplitSubChar = Convert.ToChar(paramSub[4]);
        string ProcedureSubName = Convert.ToString(paramSub[0]);
        string[] InputSubName = paramSub[1].Split(SplitSubChar);
        string[] InputSubType = paramSub[2].Split(SplitSubChar);
        string SetRequestLetter = paramSub[3].Replace("RequestLetter", RequestLetter);
        string[] InputSubValue = SetRequestLetter.Split(SplitSubChar);
        if (ProcedureSubName.Trim() != String.Empty && (InputSubName.Length == InputSubType.Length) && (InputSubType.Length == InputSubValue.Length))
        {
            DT = objEngine.SelectProcedureArr(ProcedureSubName, InputSubName, InputSubType, InputSubValue);

        }


        if (ComboSubAccountE.Items.Count > 0)
        {
            ComboSubAccountE.Items.Clear();
            ComboSubAccountE.Text = "";
        }
        ComboSubAccountE.DataSource = DT;
        ComboSubAccountE.TextField = DT.Columns[0].ColumnName;//"Contact_Name";
        ComboSubAccountE.ValueField = DT.Columns[1].ColumnName;//"SubAccount_ReferenceID";
        ComboSubAccountE.DataBind();






        // combosunaccount populated and selected end

    }


    [WebMethod]
    public static List<string> GetMainAccountList(string CombinedQuery)
    {
        List<string> obj = new List<string>();
        DataTable DT = new DataTable();
        DBEngine objEngine = new DBEngine();
        string RequestLetter = "%";
        string[] param = CombinedQuery.Replace("--", "+").Replace("^^", "%").Split('$');
        string strQuery_Table = param[0].Trim() != String.Empty ? param[0] : null;
        string strQuery_FieldName = param[1].Trim() != String.Empty ? param[1] : null;
        string strQuery_WhereClause = param[2].Trim() != String.Empty ? param[2] : null;
        string strQuery_OrderBy = param[3].Trim() != String.Empty ? param[3] : null;
        string strQuery_GroupBy = param[4].Trim() != String.Empty ? param[4] : null;
        if (strQuery_Table != null)
        {
            strQuery_Table = strQuery_Table.Replace("RequestLetter", RequestLetter);
        }
        if (strQuery_FieldName != null)
        {
            strQuery_FieldName = strQuery_FieldName.Replace("RequestLetter", RequestLetter);
        }
        if (strQuery_WhereClause != null)
        {
            strQuery_WhereClause = strQuery_WhereClause.Replace("RequestLetter", RequestLetter);
        }
        DT = objEngine.GetDataTable(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_OrderBy, strQuery_GroupBy);
        if (DT.Rows.Count != 0)
        {
            foreach (DataRow dr in DT.Rows)
            {

                obj.Add(Convert.ToString(dr["MainAccount_Name1"]) + "|" + Convert.ToString(dr["MainAccount_ReferenceID"]));
            }


        }

        return obj;
    }


    [WebMethod]
    public static List<string> GetSubAccountList(string CombinedQuery)
    {


        List<string> obj = new List<string>();
        DataTable DT = new DataTable();
        DBEngine objEngine = new DBEngine();
        string RequestLetter = "%";
        string[] param = CombinedQuery.Split('$');


        char SplitChar = Convert.ToChar(param[4]);
        string ProcedureName = Convert.ToString(param[0]);
        string[] InputName = param[1].Split(SplitChar);
        string[] InputType = param[2].Split(SplitChar);
        string SetRequestLetter = param[3].Replace("RequestLetter", RequestLetter);
        string[] InputValue = SetRequestLetter.Split(SplitChar);
        if (ProcedureName.Trim() != String.Empty && (InputName.Length == InputType.Length) && (InputType.Length == InputValue.Length))
        {
            DT = objEngine.SelectProcedureArr(ProcedureName, InputName, InputType, InputValue);
            if (DT.Rows.Count != 0)
            {
                foreach (DataRow dr in DT.Rows)
                {

                    obj.Add(Convert.ToString(dr[0]) + "|" + Convert.ToString(dr[1]));
                }
            }

        }


        return obj;
    }

    #region Batch Editing Grid --> Added By Sudip

    public IEnumerable GetMainAccount()
    {
        List<MainAccount> MainAccountList = new List<MainAccount>();
       // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

        DataTable DT = objEngine.GetDataTable("Master_MainAccount", " MainAccount_Name+' [ '+rtrim(ltrim(MainAccount_AccountCode))+' ]' as CountryName,cast(MainAccount_ReferenceID as varchar)+'~'+MainAccount_SubLedgerType+'~MAINAC~'+MainAccount_AccountType as CountryID ", " MainAccount_BankCashType Not In ('Bank','Cash') ");

        for (int i = 0; i < DT.Rows.Count; i++)
        {
            MainAccount MainAccounts = new MainAccount();
            MainAccounts.CountryID = Convert.ToString(DT.Rows[i]["CountryID"]);
            MainAccounts.CountryName = Convert.ToString(DT.Rows[i]["CountryName"]);
            MainAccountList.Add(MainAccounts);
        }

        return MainAccountList;
    }

    public IEnumerable GetMainAccountByBranch(string branchId)
    {
        List<MainAccount> MainAccountList = new List<MainAccount>();
       // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);


        BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
        DataTable DT = objEngine.GetDataTable("Master_MainAccount", " MainAccount_Name+' [ '+rtrim(ltrim(MainAccount_AccountCode))+' ]' as CountryName,cast(MainAccount_ReferenceID as varchar)+'~'+MainAccount_SubLedgerType+'~MAINAC~'+MainAccount_AccountType as CountryID ", " MainAccount_BankCashType Not In ('Bank','Cash') AND MainAccount_branchId=" + branchId);

        for (int i = 0; i < DT.Rows.Count; i++)
        {
            MainAccount MainAccounts = new MainAccount();
            MainAccounts.CountryID = Convert.ToString(DT.Rows[i]["CountryID"]);
            MainAccounts.CountryName = Convert.ToString(DT.Rows[i]["CountryName"]);
            MainAccountList.Add(MainAccounts);
        }

        return MainAccountList;
    }

    public IEnumerable GetSubAccount(string ProcedureSubName, string[] InputSubName, string[] InputSubType, string[] InputSubValue)
    {
      //  BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

        DataTable DT = objEngine.SelectProcedureArr(ProcedureSubName, InputSubName, InputSubType, InputSubValue);

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
    public IEnumerable GetSubAccount()
    {
        //DataTable DT = new DataTable();
        //DT.Columns.Add("CityName", typeof(string));
        //DT.Columns.Add("CityID", typeof(string));

      //  BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

        DataTable DT = objEngine.GetDataTable("tbl_master_branch", " (ISNULL(ltrim(rtrim(b.cnt_firstname)),'') + ''+ ISNULL(b.cnt_middlename,'')+ '' + ISNULL(b.cnt_lastName,'''')) +' ['+isnull(ltrim(rtrim(b.cnt_UCC)),'')+']' +' ['+(select rtrim(branch_description)  ", " branch_id in (select cnt_branchid from tbl_master_contact where cnt_internalid=b.cnt_internalId))+']' as Contact_Name,b.cnt_internalId+'~NAB~NAB~'+isnull(ltrim(rtrim(cnt_firstName)),'')+''+isnull(cnt_middleName,'')+''+isnull(cnt_lastName,'') as SubAccount_ReferenceID FROM tbl_master_contact as b ");

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
    public IEnumerable GetVoucher()
    {
        DataSet DsOnLoad = new DataSet();
        DataTable tempdt = new DataTable();
        List<VOUCHERLIST> VoucherList = new List<VOUCHERLIST>();

        if (File.Exists(Server.MapPath(JournalVoucherFile_XMLPATH)))
        {
            if (DsOnLoad.Tables.Count > 0) { DsOnLoad.Tables.Remove(DsOnLoad.Tables[0]); DsOnLoad.Clear(); }
            DsOnLoad.ReadXml(Server.MapPath(JournalVoucherFile_XMLPATH));
        }
        DataTable Voucherdt = new DataTable();
        if (DsOnLoad.Tables.Count > 0 && DsOnLoad != null)
        {
            Voucherdt = DsOnLoad.Tables[0].DefaultView.ToTable(false, "CashReportID", "MainAccount1", "SubAccount1", "WithDrawl", "Receipt", "SubNarration");

            for (int i = 0; i < Voucherdt.Rows.Count; i++)
            {
                VOUCHERLIST Vouchers = new VOUCHERLIST();
                Vouchers.CashReportID = Convert.ToString(Voucherdt.Rows[i][0]);
                Vouchers.MainAccount1 = Convert.ToString(Voucherdt.Rows[i][1]);
                Vouchers.SubAccount1 = Convert.ToString(Voucherdt.Rows[i][2]);
                Vouchers.WithDrawl = Convert.ToString(Voucherdt.Rows[i][3]);
                Vouchers.Receipt = Convert.ToString(Voucherdt.Rows[i][4]);
                Vouchers.Narration = Convert.ToString(Voucherdt.Rows[i][5]);
                VoucherList.Add(Vouchers);
            }
        }

        return VoucherList;
    }
    public string GetSubAccountText(string strMainValue, string strSubValue)
    {
        string strText = "";

        ///////////////////////////////////////////////////////////////////////////////////

        string RequestLetter = "%";
        var SegID = "";
        var SegmentName = "";

        if (hdn_SegID_SegmentName.Value != "")
        {
            SegID = hdnSegmentid.Value;
            SegmentName = hdn_SegID_SegmentName.Value;
        }


        var ProcedureName = "SubAccountSelect_New";
        var InputName = "CashBank_MainAccountID|clause|branch|exchSegment|SegmentN";
        var InputType = "V|V|V|V|V";
        var InputValue = strMainValue.ToString().Split('~')[0] + "|RequestLetter|" + Session["userbranchHierarchy"] + "|'" + Session["ExchangeSegmentID"] + "'|'" + SegmentName + "'";
        var SplitChar = "|";
        var CombinedSubQuery = ProcedureName + "$" + InputName + "$" + InputType + "$" + InputValue + "$" + SplitChar;


        string[] paramSub = CombinedSubQuery.Split('$');


        char SplitSubChar = Convert.ToChar(paramSub[4]);
        string ProcedureSubName = Convert.ToString(paramSub[0]);
        string[] InputSubName = paramSub[1].Split(SplitSubChar);
        string[] InputSubType = paramSub[2].Split(SplitSubChar);
        string SetRequestLetter = paramSub[3].Replace("RequestLetter", RequestLetter);
        string[] InputSubValue = SetRequestLetter.Split(SplitSubChar);

        //////////////////////////////////////////////////////////////////////////////////

      //  BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

        DataTable DT = objEngine.SelectProcedureArr(ProcedureSubName, InputSubName, InputSubType, InputSubValue);

        DataRow[] result = DT.Select("SubAccount_ReferenceID ='" + strSubValue + "'");

        // Display.
        foreach (DataRow row in result)
        {
            strText = Convert.ToString(row["Contact_Name"]);
        }

        return strText;
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        ((GridViewDataComboBoxColumn)grid.Columns["MainAccount1"]).PropertiesComboBox.DataSource = GetMainAccount();
        ((GridViewDataComboBoxColumn)grid.Columns["SubAccount1"]).PropertiesComboBox.DataSource = GetSubAccount();

        if (!IsPostBack)
        {
            grid.DataBind();
        }
    }
    public void SetDebitCredit(ref decimal Credit, ref decimal Debit)
    {

        DataSet DsOnLoad = new DataSet();
        DataTable Voucherdt = new DataTable();

        if (File.Exists(Server.MapPath(JournalVoucherFile_XMLPATH)))
        {
            if (DsOnLoad.Tables.Count > 0) { DsOnLoad.Tables.Remove(DsOnLoad.Tables[0]); DsOnLoad.Clear(); }
            DsOnLoad.ReadXml(Server.MapPath(JournalVoucherFile_XMLPATH));
        }
        if (DsOnLoad.Tables.Count > 0 && DsOnLoad != null)
        {
            Voucherdt = DsOnLoad.Tables[0];

            for (int i = 0; i < Voucherdt.Rows.Count; i++)
            {
                Credit = Credit + Convert.ToDecimal(Voucherdt.Rows[i]["WithDrawl"]);
                Debit = Debit + Convert.ToDecimal(Voucherdt.Rows[i]["Receipt"]);
            }
        }

        //txt_Credit.Text = Debit.ToString();
        //txt_Debit.Text = Credit.ToString();

        //txtTCredit.ReadOnly = true;
        //txtTDebit.ReadOnly = true;

        //if (Credit == Debit)
        //{
        //    btnSaveRecords.Visible = true;
        //    btn_SaveRecords.Visible = true;
        //}
        //else
        //{
        //    btnSaveRecords.Visible = false;
        //    btn_SaveRecords.Visible = false;
        //}
    }
    protected void CityCmb_Callback(object sender, CallbackEventArgsBase e)
    {
        String countryID = Convert.ToString(e.Parameter);
        ASPxComboBox c = sender as ASPxComboBox;

        ///////////////////////////////////////////////////////////////////////////////////

        string RequestLetter = "%";
        var SegID = "";
        var SegmentName = "";

        if (hdn_SegID_SegmentName.Value != "")
        {
            SegID = hdnSegmentid.Value;
            SegmentName = hdn_SegID_SegmentName.Value;
        }


        var ProcedureName = "SubAccountSelect_New";
        var InputName = "CashBank_MainAccountID|clause|branch|exchSegment|SegmentN";
        var InputType = "V|V|V|V|V";
        var InputValue = countryID.ToString().Split('~')[0] + "|RequestLetter|" + Session["userbranchHierarchy"] + "|'" + Session["ExchangeSegmentID"] + "'|'" + SegmentName + "'";
        var SplitChar = "|";
        var CombinedSubQuery = ProcedureName + "$" + InputName + "$" + InputType + "$" + InputValue + "$" + SplitChar;


        string[] paramSub = CombinedSubQuery.Split('$');


        char SplitSubChar = Convert.ToChar(paramSub[4]);
        string ProcedureSubName = Convert.ToString(paramSub[0]);
        string[] InputSubName = paramSub[1].Split(SplitSubChar);
        string[] InputSubType = paramSub[2].Split(SplitSubChar);
        string SetRequestLetter = paramSub[3].Replace("RequestLetter", RequestLetter);
        string[] InputSubValue = SetRequestLetter.Split(SplitSubChar);

        //////////////////////////////////////////////////////////////////////////////////

        c.DataSource = GetSubAccount(ProcedureSubName, InputSubName, InputSubType, InputSubValue);
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
        string countryID = Convert.ToString(container.Grid.GetRowValues(container.Grid.VisibleStartIndex, "MainAccount1"));
        grid.JSProperties["cplastCountryID"] = countryID;

        ///////////////////////////////////////////////////////////////////////////////////

        string RequestLetter = "%";
        var SegID = "";
        var SegmentName = "";

        if (hdn_SegID_SegmentName.Value != "")
        {
            SegID = hdnSegmentid.Value;
            SegmentName = hdn_SegID_SegmentName.Value;
        }


        var ProcedureName = "SubAccountSelect_New";
        var InputName = "CashBank_MainAccountID|clause|branch|exchSegment|SegmentN";
        var InputType = "V|V|V|V|V";
        var InputValue = countryID.ToString().Split('~')[0] + "|RequestLetter|" + Session["userbranchHierarchy"] + "|'" + Session["ExchangeSegmentID"] + "'|'" + SegmentName + "'";
        var SplitChar = "|";
        var CombinedSubQuery = ProcedureName + "$" + InputName + "$" + InputType + "$" + InputValue + "$" + SplitChar;


        string[] paramSub = CombinedSubQuery.Split('$');


        char SplitSubChar = Convert.ToChar(paramSub[4]);
        string ProcedureSubName = Convert.ToString(paramSub[0]);
        string[] InputSubName = paramSub[1].Split(SplitSubChar);
        string[] InputSubType = paramSub[2].Split(SplitSubChar);
        string SetRequestLetter = paramSub[3].Replace("RequestLetter", RequestLetter);
        string[] InputSubValue = SetRequestLetter.Split(SplitSubChar);

        //////////////////////////////////////////////////////////////////////////////////

        cityCombo.DataSource = GetSubAccount(ProcedureSubName, InputSubName, InputSubType, InputSubValue);
    }
    protected void grid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
    {
        string strmode = hdnJNMode.Value;
        string type = hdnRefreshType.Value;
        hdnMode.Value = strmode;

        if (hdnMode.Value == "0")
        {
            if (File.Exists(Server.MapPath(JournalVoucherFile_XMLPATH)))
            {
                File.Delete(Server.MapPath(JournalVoucherFile_XMLPATH));
                File.Delete(Server.MapPath(JournalVoucherFile_XMLPATH + "Validate"));
            }
        }

        //for (int i = 0; i < grid.VisibleRowCount; i++)
        //{
        //    string MainAccount = Convert.ToString(grid.GetRowValues(i, "MainAccount1"));
        //    string SubAccount = Convert.ToString(grid.GetRowValues(i, "SubAccount1"));
        //    string WithDrawl = Convert.ToString(grid.GetRowValues(i, "WithDrawl"));
        //    string Receipt = Convert.ToString(grid.GetRowValues(i, "Receipt"));
        //    string Narration = Convert.ToString(grid.GetRowValues(i, "Narration"));
        //}

        foreach (var args in e.InsertValues)
        {
            string MainAccount = Convert.ToString(args.NewValues["MainAccount1"]);
            string SubAccount = Convert.ToString(args.NewValues["SubAccount1"]);
            string WithDrawl = Convert.ToString(args.NewValues["WithDrawl"]);
            string Receipt = Convert.ToString(args.NewValues["Receipt"]);
            string Narration = Convert.ToString(args.NewValues["Narration"]);

            hdnMainAccountId.Value = MainAccount;
            hdnSubAccountId.Value = SubAccount;
            hdnMainAccountText.Value = MainAccount;
            hdnSubAccountText.Value = SubAccount;

            txtdebit.Text = WithDrawl;
            txtcredit.Text = Receipt;
            txtNarration1.Text = Narration;

            if (MainAccount != "" && MainAccount != null)
            {
                CreateXML_FromData("ADD", "");
            }
            //JournalVoucher.InsertCustomer((string)args.NewValues["MainAccount1"], (string)args.NewValues["SubAccount1"], (string)args.NewValues["WithDrawl"], (string)args.NewValues["Receipt"], (string)args.NewValues["Narration"]);
        }

        foreach (var args in e.UpdateValues)
        {
            string CashReportID = Convert.ToString(args.Keys["CashReportID"]);
            string MainAccount = Convert.ToString(args.NewValues["MainAccount1"]);
            string SubAccount = Convert.ToString(args.NewValues["SubAccount1"]);
            string WithDrawl = Convert.ToString(args.NewValues["WithDrawl"]);
            string Receipt = Convert.ToString(args.NewValues["Receipt"]);
            string Narration = Convert.ToString(args.NewValues["Narration"]);

            //hdnMainAccountId.Value = MainAccount;
            //hdnSubAccountId.Value = SubAccount;
            //hdnMainAccountText.Value = MainAccount;
            //hdnSubAccountText.Value = SubAccount;

            //txtdebit.Text = WithDrawl;
            //txtcredit.Text = Receipt;
            //txtNarration1.Text = Narration;

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
                int count = MainAccount.Split('~').Length - 1;
                if (count != 3)
                {
                    //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                    BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();


                    DataTable DT = objEngine.GetDataTable("Master_MainAccount", " MainAccount_Name+' [ '+rtrim(ltrim(MainAccount_AccountCode))+' ]' as MainAccount_Name,cast(MainAccount_ReferenceID as varchar)+'~'+MainAccount_SubLedgerType+'~MAINAC~'+MainAccount_AccountType as MainAccount_ID ", " MainAccount_Name+' [ '+rtrim(ltrim(MainAccount_AccountCode))+' ]' ='" + MainAccount + "'");
                    MainAccount = DT.Rows[0][1].ToString();
                }

                hdnMainAccountId.Value = MainAccount;
                hdnSubAccountId.Value = SubAccount;
                hdnMainAccountText.Value = MainAccount;
                hdnSubAccountText.Value = SubAccount;

                txtdebit.Text = WithDrawl;
                txtcredit.Text = Receipt;
                txtNarration1.Text = Narration;

                if (MainAccount != "" && CashReportID != "" && MainAccount != null)
                {
                    //CreateXML_FromData("EDIT", CashReportID);
                    CreateXML_FromData("DELETE", CashReportID);
                    CreateXML_FromData("ADD", "");
                }
            }
            //JournalVoucher.UpdateCustomer((string)args.NewValues["MainAccount1"], (string)args.NewValues["SubAccount1"], (string)args.NewValues["WithDrawl"], (string)args.NewValues["Receipt"], (string)args.NewValues["Narration"]);
        }

        foreach (var args in e.DeleteValues)
        {
            string CashReportID = Convert.ToString(args.Keys["CashReportID"]);
            CreateXML_FromData("DELETE", CashReportID);

            //JournalVoucher.DeleteCustomer((string)args.Keys["CustomerID"]);
        }

        DetailsGrid.JSProperties["cpSaveSuccessOrFail"] = grid.JSProperties["cpSaveSuccessOrFail"] = Save_Records();
        DetailsGrid.JSProperties["cpEntryNotAllow"] = "undefined";
        DetailsGrid.JSProperties["cpSuccessDiscard"] = "undefined";
        DetailsGrid.JSProperties["cpEntryData"] = "undefined";

        e.Handled = true;
    }
    public void CreateXML_FromData(string strMode, string CashReportID)
    {
        string strSplitBranch = Convert.ToString(ddlBranch.SelectedValue);
        Boolean AllowDisAllow = Convert.ToBoolean(AllowDisAllowEntry(strSplitBranch).Split('~')[0]);
        if (strMode == "ADD")
        {
            if (AllowDisAllow)
            {
                try
                {
                    AddData_ToGrid(strSplitBranch);
                    AddData_EntryValidationXML(strSplitBranch);
                    //This For Log Purpose
                    string strLogID = ViewState["LogID"].ToString();
                }
                catch
                {
                    //This For Log Purpose
                    string strLogID = ViewState["LogID"].ToString();
                }

                DetailsGrid.JSProperties["cpEntryNotAllow"] = "Empty";
                DetailsGrid.JSProperties["cpSaveSuccessOrFail"] = "undefined";
                DetailsGrid.JSProperties["cpSuccessDiscard"] = "undefined";
                DetailsGrid.JSProperties["cpEntryData"] = "undefined";
            }
            else
            {
                string Reason = AllowDisAllowEntry(strSplitBranch).Split('~')[1];
                if (Reason == "Branch") DetailsGrid.JSProperties["cpEntryNotAllow"] = "You Can Not Select More Than One Non-Branch Accounts For This Entry";
                else DetailsGrid.JSProperties["cpEntryNotAllow"] = "You Can Not Select More Than One Branch Accounts For This Entry";
                DetailsGrid.JSProperties["cpSaveSuccessOrFail"] = "undefined";
                DetailsGrid.JSProperties["cpSuccessDiscard"] = "undefined";
                DetailsGrid.JSProperties["cpEntryData"] = "undefined";
            }
        }
        else if (strMode == "EDIT")
        {
            string txtMainAccountE_hidden = hdnMainAccountId.Value;
            string txtSubAccountE_hidden = hdnSubAccountText.Value;
            string ComboMainAccountE = hdnMainAccountId.Value;
            string ComboSubAccountE = hdnSubAccountId.Value;
            string DebitAmt = txtdebit.Text;
            string CreditAmt = txtcredit.Text;
            string Narration = txtNarration.Text;


            DataSet DsUpdateXML = new DataSet();
            if (DsUpdateXML.Tables.Count > 0) { DsUpdateXML.Tables.Remove(DsUpdateXML.Tables[0]); DsUpdateXML.Clear(); }
            DsUpdateXML.ReadXml(Server.MapPath(JournalVoucherFile_XMLPATH));
            DsUpdateXML.Tables[0].PrimaryKey = new DataColumn[] { DsUpdateXML.Tables[0].Columns["CashReportID"] };
            DataRow row = DsUpdateXML.Tables[0].Rows.Find(CashReportID);
            if (ddlBranch.SelectedValue == "NAB")
            {
                row["JournalVoucherDetail_BranchID"] = ddlBranch.SelectedValue;
                row["JournalVoucherDetail_SubAccountCode"] = ComboSubAccountE != String.Empty ? ComboSubAccountE.Split('~')[0] : row["JournalVoucherDetail_SubAccountCode"].ToString();
            }
            else
            {
                row["JournalVoucherDetail_BranchID"] = strSplitBranch;
                row["JournalVoucherDetail_SubAccountCode"] = ComboSubAccountE != String.Empty ? ComboSubAccountE.Split('~')[0] + '~' + ComboSubAccountE.Split('~')[1] + '~' + ComboSubAccountE.Split('~')[2] : row["JournalVoucherDetail_SubAccountCode"].ToString();
            }
            row["JournalVoucherDetail_MainAccountCode"] = ComboMainAccountE.ToString() != String.Empty ? ComboMainAccountE.ToString() : row["JournalVoucherDetail_MainAccountCode"].ToString();
            row["JournalVoucherDetail_AmountDr"] = DebitAmt;
            row["JournalVoucherDetail_AmountCr"] = CreditAmt;
            row["SubNarration"] = Narration;
            row["MainAccount1"] = Convert.ToString(ComboMainAccountE);
            row["SubAccount1"] = Convert.ToString(txtSubAccountE_hidden);
            row["WithDrawl"] = DebitAmt;
            row["Receipt"] = CreditAmt;
            row["Type"] = row["Type"].ToString();
            row["BranchNonBranch"] = row["BranchNonBranch"].ToString();
            row["OldJVVoucherNumber"] = "GenerateNew";
            row.AcceptChanges();
            DsUpdateXML.AcceptChanges();
            File.Delete(Server.MapPath(JournalVoucherFile_XMLPATH));
            DsUpdateXML.WriteXml(Server.MapPath(JournalVoucherFile_XMLPATH));
            DsUpdateXML.Dispose();
        }
        else if (strMode == "DELETE")
        {
            DataSet DsRowDeleteXML = new DataSet();
            DataSet DsRowDeleteXMLValid = new DataSet();
            if (File.Exists(Server.MapPath(JournalVoucherFile_XMLPATH)))
            {
                if (DsRowDeleteXML.Tables.Count > 0) { DsRowDeleteXML.Tables.Remove(DsRowDeleteXML.Tables[0]); DsRowDeleteXML.Clear(); }
                DsRowDeleteXML.ReadXml(Server.MapPath(JournalVoucherFile_XMLPATH));
                DsRowDeleteXML.Tables[0].PrimaryKey = new DataColumn[] { DsRowDeleteXML.Tables[0].Columns["CashReportID"] };
                DataRow row = DsRowDeleteXML.Tables[0].Rows.Find(CashReportID);
                string branchNonBranch = row["BranchNonBranch"].ToString();
                DsRowDeleteXML.Tables[0].Rows.Remove(row);

                BindGrid(DetailsGrid, DsRowDeleteXML, "DESC");
                try
                {
                    if (File.Exists(Server.MapPath(JournalVoucherFile_XMLPATH)))
                    {
                        File.Delete(Server.MapPath(JournalVoucherFile_XMLPATH));
                        if (DsRowDeleteXML.Tables.Count > 0)
                        {
                            if (DsRowDeleteXML.Tables[0].Rows.Count > 0)
                            {
                                DsRowDeleteXML.WriteXml(Server.MapPath(JournalVoucherFile_XMLPATH));
                            }
                        }
                    }
                    if (File.Exists(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH)))
                    {
                        DsRowDeleteXMLValid = new DataSet();
                        DsRowDeleteXMLValid.ReadXml(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
                        DsRowDeleteXMLValid.Tables[0].PrimaryKey = new DataColumn[] { DsRowDeleteXMLValid.Tables[0].Columns["ID"] };
                        if (branchNonBranch == "NAB")
                        {
                            row = DsRowDeleteXMLValid.Tables[0].Rows.Find("NB");
                            row["EntryCount"] = Convert.ToString(Convert.ToInt32(row["EntryCount"].ToString()) - 1);
                            row.AcceptChanges();
                        }
                        else
                        {
                            row = DsRowDeleteXMLValid.Tables[0].Rows.Find(branchNonBranch);
                            row["EntryCount"] = Convert.ToString(Convert.ToInt32(row["EntryCount"].ToString()) - 1);
                            row.AcceptChanges();
                            if (row["EntryCount"].ToString() == "0")
                            {
                                DsRowDeleteXMLValid.Tables[0].Rows[0][1] = Convert.ToInt32(DsRowDeleteXMLValid.Tables[0].Rows[0][1].ToString()) - 1;
                                DsRowDeleteXMLValid.Tables[0].Rows.Remove(row);
                                DsRowDeleteXMLValid.AcceptChanges();
                            }
                        }
                        if (DsRowDeleteXMLValid.Tables.Count > 0)
                        {
                            if (DsRowDeleteXMLValid.Tables[0].Rows.Count > 0)
                            {
                                DsRowDeleteXMLValid.WriteXml(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
                            }
                        }
                    }
                    if (DetailsGrid.VisibleRowCount == 0)
                    {
                        if (File.Exists(Server.MapPath(JournalVoucherFile_XMLPATH)))
                        {
                            File.Delete(Server.MapPath(JournalVoucherFile_XMLPATH));
                        }
                        if (File.Exists(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH)))
                        {
                            File.Delete(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
                        }
                    }

                    TotalCredit = 0;
                    TotalDebit = 0;
                }
                catch
                {
                    DsRowDeleteXMLValid.Dispose();
                    DsRowDeleteXML.Dispose();
                }
            }
            else
            {
                if (File.Exists(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH)))
                {
                    File.Delete(Server.MapPath(JournalVoucherFile_VALIDATEXMLPATH));
                }
            }
            DsRowDeleteXMLValid.Dispose();
            DsRowDeleteXML.Dispose();
        }
    }
    protected void btnJournalAdd_Click(object sender, EventArgs e)
    {
        hdnMode.Value = "0";

        if (hdnMode.Value == "0")
        {
            //Set XML File Path
            JournalVoucherFile_XMLPATH = "../Documents/" + "JV_" + Session["userid"].ToString() + "_" + txtAccountCode.Text.Trim() + "_" + ViewState["WhichSegment"].ToString().Trim() + "_" + Convert.ToDateTime(tDate.Value).ToString("dd/MM/yyyy").Replace("/", "");
            JournalVoucherFile_VALIDATEXMLPATH = "../Documents/" + "JV_" + Session["userid"].ToString() + "_" + txtAccountCode.Text.Trim() + "_" + ViewState["WhichSegment"].ToString().Trim() + "_" + Convert.ToDateTime(tDate.Value).ToString("dd/MM/yyyy").Replace("/", "") + "Validate";


        }
        else
        {
            if (Session["IBRef"] != null)
            {
                JournalVoucherFile_XMLPATH = "../Documents/" + "JVE_" + Session["IBRef"].ToString();
                JournalVoucherFile_VALIDATEXMLPATH = "../Documents/" + "JVE_" + Session["IBRef"].ToString() + "Validate";
            }
        }
        Session["JournalVoucherFile_XMLPATH"] = JournalVoucherFile_XMLPATH;
        //BindVoucherGrid();

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "OnAddButtonClick()", true);
    }
    protected void FillSubAccount(ASPxComboBox cmb, string MainAccount)
    {
        string RequestLetter = "%";
        var SegID = "";
        var SegmentName = "";

        if (hdn_SegID_SegmentName.Value != "")
        {
            SegID = hdnSegmentid.Value;
            SegmentName = hdn_SegID_SegmentName.Value;
        }


        var ProcedureName = "SubAccountSelect_New";
        var InputName = "CashBank_MainAccountID|clause|branch|exchSegment|SegmentN";
        var InputType = "V|V|V|V|V";
        var InputValue = MainAccount.ToString().Split('~')[0] + "|RequestLetter|" + Session["userbranchHierarchy"] + "|'" + Session["ExchangeSegmentID"] + "'|'" + SegmentName + "'";
        var SplitChar = "|";
        var CombinedSubQuery = ProcedureName + "$" + InputName + "$" + InputType + "$" + InputValue + "$" + SplitChar;


        string[] paramSub = CombinedSubQuery.Split('$');


        char SplitSubChar = Convert.ToChar(paramSub[4]);
        string ProcedureSubName = Convert.ToString(paramSub[0]);
        string[] InputSubName = paramSub[1].Split(SplitSubChar);
        string[] InputSubType = paramSub[2].Split(SplitSubChar);
        string SetRequestLetter = paramSub[3].Replace("RequestLetter", RequestLetter);
        string[] InputSubValue = SetRequestLetter.Split(SplitSubChar);

        //////////////////////////////////////////////////////////////////////////////////

        cmb.DataSource = GetSubAccount(ProcedureSubName, InputSubName, InputSubType, InputSubValue);
        cmb.DataBind();

        //string[,] state = GetState(country);
        //cmb.Items.Clear();

        //for (int i = 0; i < state.GetLength(0); i++)
        //{
        //    cmb.Items.Add(state[i, 1], state[i, 0]);
        //}
    }


    private void bindMainAccount(object source, CallbackEventArgsBase e)
    {
        //FillStateCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
        ASPxComboBox currentCombo = source as ASPxComboBox;
        currentCombo.DataSource = GetMainAccountByBranch( e.Parameter);
        currentCombo.DataBind();
    }


    protected void grid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
    {
        if (e.Column.FieldName == "MainAccount1")
        {

            ((ASPxComboBox)e.Editor).Callback += new CallbackEventHandlerBase(bindMainAccount);
        }

        e.Editor.ReadOnly = false;
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
    protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
    {
        DataSet Ds_JVE;
        SqlDataAdapter Da_JVE;
        int RowIndex;
        string PCBCommandName = e.Parameters.Split('~')[0];
        string IBRef;
        string OldVoucherNumber = String.Empty;
        string OldWhichTypeOfItem = String.Empty;
        string VoucherNumber;
        string JVEFile_XMLPATH;

        if (PCBCommandName == "DeleteAllRecord")
        {
            ASPxGridView jvGrid = sender as ASPxGridView;
            jvGrid.DataSource = null;
            jvGrid.DataBind();
            return;
        }

        RowIndex = Convert.ToInt32(e.Parameters.Split('~')[1]);

        //////////////////////////////////////////////////////////////////////////////////////////////

        IBRef = GvJvSearch.GetRowValues(RowIndex, "IBRef").ToString();
        JVEFile_XMLPATH = "../Documents/" + "JVE_" + IBRef;
        if (File.Exists(Server.MapPath(JVEFile_XMLPATH)))
        {
            File.Delete(Server.MapPath(JVEFile_XMLPATH));
            File.Delete(Server.MapPath(JVEFile_XMLPATH + "Validate"));
            //This For Log Purpose
            string strLogID = ViewState["LogID"].ToString();
        }
        Session["StrQuery"] = null;
        Session["IBRef"] = null;
        GvJvSearch.JSProperties["cpJVClose"] = "File Successfully Close";
        GvJvSearch.JSProperties["cpJVE_FileAlreadyUsedBy"] = null;
        GvJvSearch.JSProperties["cpEntryEventFire"] = null;
        GvJvSearch.JSProperties["cpJVDelete"] = null;
        Session["IBRef"] = null;
        Session["OldJVVoucherNumber"] = null;
        Session["OldWhichTypeItem"] = null;

        //////////////////////////////////////////////////////////////////////////////////////////////

        IBRef = GvJvSearch.GetRowValues(RowIndex, "IBRef").ToString();
        OldWhichTypeOfItem = GvJvSearch.GetRowValues(RowIndex, "WhichTypeItem").ToString();
        VoucherNumber = GvJvSearch.GetRowValues(RowIndex, "VoucherNumber").ToString();
        //VoucherNumber = GvJvSearch.GetRowValues(RowIndex, "BillNumber").ToString();

        CreateJVE_XMLFile(IBRef, VoucherNumber);
        JournalVoucherFile_XMLPATH = JVEFile_XMLPATH;

        DataSet DsOnLoad = new DataSet();
        string BillNo = "", JvNarration = "", BranchSelectedValue = "", Prefix = "";
        decimal Credit = 0, Debit = 0;
        SetDebitCredit(ref Credit, ref Debit);

        if (File.Exists(Server.MapPath(JVEFile_XMLPATH)))
        {
            if (DsOnLoad.Tables.Count > 0) { DsOnLoad.Tables.Remove(DsOnLoad.Tables[0]); DsOnLoad.Clear(); }
            DsOnLoad.ReadXml(Server.MapPath(JVEFile_XMLPATH));
            if (DsOnLoad.Tables.Count > 0)
            {
                BillNo = DsOnLoad.Tables[0].Rows[0][22].ToString();
                JvNarration = DsOnLoad.Tables[0].Rows[0][8].ToString();
                BranchSelectedValue = DsOnLoad.Tables[0].Rows[0][3].ToString();
                Prefix = DsOnLoad.Tables[0].Rows[0][19].ToString();
            }
        }

        grid.JSProperties["cpEdit"] = BillNo + "~" + JvNarration + "~" + BranchSelectedValue + "~" + Credit + "~" + Debit;

        grid.DataSource = GetVoucher();
        grid.DataBind();

        if (ViewState["JVE_FileAlreadyUsedBy"] != null)
        {
            if (ViewState["JVE_FileAlreadyUsedBy"].ToString().Split('~')[1] == Session["userid"].ToString())
            {
                GvJvSearch.JSProperties["cpJVE_FileAlreadyUsedBy"] = "HimSelef~";
            }
            else
            {
                GvJvSearch.JSProperties["cpJVE_FileAlreadyUsedBy"] = "Other~" + ViewState["JVE_FileAlreadyUsedBy"].ToString();
            }
            GvJvSearch.JSProperties["cpEntryEventFire"] = null;
        }
        else
        {
            BindGrid(GvJvSearch);
            GvJvSearch.JSProperties["cpJVE_FileAlreadyUsedBy"] = null;
            GvJvSearch.JSProperties["cpEntryEventFire"] = "true";
            //This For Log Purpose
            string strLogID = ViewState["LogID"].ToString();
            //oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlOpenForEdit, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucher", strLogID, GenericLogSystem.LogType.JV);
            // oGenericLogSystem.CreateLog("", "", GenericLogSystem.LogState.XmlOpenForEdit, Session["UserID"].ToString(), IBRef, "Trans_JournalVoucherDetail", strLogID, GenericLogSystem.LogType.JV);
        }
        GvJvSearch.JSProperties["cpJVDelete"] = null;
        GvJvSearch.JSProperties["cpJVClose"] = null;
        Session["IBRef"] = IBRef;
        Session["OldJVVoucherNumber"] = VoucherNumber;
        Session["OldWhichTypeItem"] = OldWhichTypeOfItem;
    }
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
    //public void BindList()
    //{
    //    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
    //    DataTable DT = oDBEngine.GetDataTable("tbl_master_Idschema", " ID,SchemaName ", " TYPE_ID='1' AND IsActive=1 Order by SchemaName ");
    //    rblScheme.DataSource = DT;
    //    rblScheme.DataTextField = "SchemaName";
    //    rblScheme.DataValueField = "ID";
    //    rblScheme.DataBind();
    //}
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
        public string MainAccount1 { get; set; }
        public string SubAccount1 { get; set; }
        public string WithDrawl { get; set; }
        public string Receipt { get; set; }
        public string Narration { get; set; }
    }

    #endregion
}