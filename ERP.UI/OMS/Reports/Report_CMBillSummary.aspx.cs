using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
//using DevExpress.Web.ASPxEditors;
using DevExpress.Web;
//using DevExpress.Web.ASPxTabControl;

public partial class Reports_Report_CMBillSummary : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
{
    #region Global Variable
    string data;
    BusinessLogicLayer.GenericMethod oGenericMethod;
    //GenericStoreProcedure oGenericStoreProcedure;
    AspxHelper oAspxHelper;
    GenericExcelExport oGenericExcelExport;
    string CombinedGroupByQuery = null;
    BusinessLogicLayer.Converter oConverter = new BusinessLogicLayer.Converter();
    //DataSet ds;
    #endregion

    #region PropertyVariable
    protected DateTime currentFromDate, currentToDate;
    protected string TotNetObligation, TotSTT, TotTranCharge, TotStampDuty, TotDlvCharge, TotSebiFee, TotOtherCharge, TotServTax, TotNetBillAmount, TotTurnover, TotTotalBrokerage, TotUnrealizedServTax, TotUnrealizedSTT;
    static string DateFrom, DateTo, GroupBy, GroupByID, GroupByType, OrderBy, ReportType, ReportBy, Company, Segment;
    static int PageSize, PageNumber;

    string FilteredShowColumns;
    #endregion

    #region Page Properties
    public string P_DateFrom
    {
        get { return DateFrom; }
        set { DateFrom = value; }
    }
    public string P_DateTo
    {
        get { return DateTo; }
        set { DateTo = value; }
    }
    public string P_GroupBy
    {
        get { return GroupBy; }
        set { GroupBy = value; }
    }
    public string P_GroupByID
    {
        get { return GroupByID; }
        set { GroupByID = value; }
    }
    public string P_GroupByType
    {
        get { return GroupByType; }
        set { GroupByType = value; }
    }
    public string P_OrderBy
    {
        get { return OrderBy; }
        set { OrderBy = value; }
    }
    public string P_ReportType
    {
        get { return ReportType; }
        set { ReportType = value; }
    }
    public string P_ReportBy
    {
        get { return ReportBy; }
        set { ReportBy = value; }
    }
    public int P_PageSize
    {
        get { return PageSize; }
        set { PageSize = value; }
    }
    public int P_PageNumber
    {
        get { return PageNumber; }
        set { PageNumber = value; }
    }
    public string P_Company
    {
        get { return Company; }
        set { Company = value; }
    }
    public string P_Segment
    {
        get { return Segment; }
        set { Segment = value; }
    }

    public string P_FilteredShowColumns
    {
        get { return FilteredShowColumns; }
        set { FilteredShowColumns = value; }
    }
    #endregion

    #region Page Methods
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string[] PageSession = null;
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            oGenericMethod.PageInitializer(BusinessLogicLayer.GenericMethod.WhichCall.DistroyUnWantedSession_AllExceptPage, PageSession);
            oGenericMethod.PageInitializer(BusinessLogicLayer.GenericMethod.WhichCall.DistroyUnWantedSession_Page, PageSession);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session["userid"] == null)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
        }
        this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>Height('260','260');</script>");
        if (!IsPostBack)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "Reset", "<script>Reset();</script>");
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            DateTime Date = oGenericMethod.GetDate();
            currentFromDate = Date.AddDays((-1 * Date.Day) + 1);
            currentToDate = Date;
            DtFrom.Value = currentFromDate;
            DtTo.Value = currentToDate;
            Page.ClientScript.RegisterStartupScript(GetType(), "PageLoad12", "<script>PageLoad();</script>");       
       }
        //_____For performing operation without refreshing page___//
        String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
        String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
        //___________-end here___// 
    }

    string ICallbackEventHandler.GetCallbackResult()
    {
        return data;
    }

    void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
    {
        string id = eventArgument.ToString();
        if (id == "CallAjax-Client" || id == "CallAjax-Branch" || id.Contains("CallAjax-Group"))
        {
            CallUserList(id);
            CombinedGroupByQuery = CombinedGroupByQuery.Replace("\\'", "'");
            data = "AjaxQuery@" + CombinedGroupByQuery;
        }
        else
        {
            string[] idlist = id.Split('^');
            string recieveServerIDs = "";
            for (int i = 0; i < idlist.Length; i++)
            {
                string[] strVal = idlist[i].Split('!');
                string[] ids = strVal[0].Split('~');
                string whichCall = ids[ids.Length - 1];
                if (whichCall == "BRANCH")
                {
                    if (recieveServerIDs == "")
                        recieveServerIDs = idlist[i];
                    else
                        recieveServerIDs += "^" + idlist[i];
                    data = "Branch@" + recieveServerIDs.ToString();
                }
                else if (whichCall == "GROUP")
                {
                    if (recieveServerIDs == "")
                        recieveServerIDs = idlist[i];
                    else
                        recieveServerIDs += "^" + idlist[i];
                    data = "Group@" + recieveServerIDs.ToString();
                }
                else if (whichCall == "CLIENT")
                {
                    if (recieveServerIDs == "")
                        recieveServerIDs = idlist[i];
                    else
                        recieveServerIDs += "^" + idlist[i];
                    data = "Client@" + recieveServerIDs.ToString();
                }
            }
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        SetPropertiesValue();
        PageNumber = 1;
        string strPageValidationMsg = PageValidation();
        if (strPageValidationMsg == String.Empty)
        {
            DataSet ds = new DataSet();
            ds = GetCMBillSummaryData();
            //Procedure();
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 1)
                {
                    if (ReportBy == "E")
                    {
                        Export_Excel(ds);
                    }
                    else if (ReportBy == "S")
                    {

                        if (!ds.Tables[0].Columns.Contains("NetObligation"))
                            gvwCMBillSummary.Columns[4].Visible = false;
                        if (!ds.Tables[0].Columns.Contains("STT"))
                            gvwCMBillSummary.Columns[5].Visible = false;
                        if (!ds.Tables[0].Columns.Contains("TranCharge"))
                            gvwCMBillSummary.Columns[6].Visible = false;
                        if (!ds.Tables[0].Columns.Contains("StampDuty"))
                            gvwCMBillSummary.Columns[7].Visible = false;
                        if (!ds.Tables[0].Columns.Contains("DlvCharge"))
                        {
                            //gvwCMBillSummary.HeaderRow.Columns[7].Visible = false;
                            gvwCMBillSummary.Columns[8].Visible = false;
                        }
                        if (!ds.Tables[0].Columns.Contains("SebiFee"))
                            gvwCMBillSummary.Columns[9].Visible = false;
                        if (!ds.Tables[0].Columns.Contains("OtherCharge"))
                            gvwCMBillSummary.Columns[10].Visible = false;
                        if (!ds.Tables[0].Columns.Contains("ServTax"))
                            gvwCMBillSummary.Columns[11].Visible = false;
                        if (!ds.Tables[0].Columns.Contains("NetBillAmount"))
                            gvwCMBillSummary.Columns[12].Visible = false;
                        if (!ds.Tables[0].Columns.Contains("Turnover"))
                            gvwCMBillSummary.Columns[13].Visible = false;
                        if (!ds.Tables[0].Columns.Contains("TotalBrokerage"))
                            gvwCMBillSummary.Columns[14].Visible = false;
                        if (!ds.Tables[0].Columns.Contains("UnrealizedServTax"))
                            gvwCMBillSummary.Columns[15].Visible = false;
                        if (!ds.Tables[0].Columns.Contains("UnrealizedSTT"))
                            gvwCMBillSummary.Columns[16].Visible = false;

                        gvwCMBillSummary.DataSource = ds.Tables[0];
                        gvwCMBillSummary.DataBind();

                        //Showing Total Calculation and Columns To Show
                        DataTable dtTotal = new DataTable();
                        dtTotal = ds.Tables[2];
                        if (dtTotal.Rows.Count > 0)
                        {
                            GetTotalRecords_ShowColumns(dtTotal);
                        }
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "SearchFilter_Hide1", "fn_SearchFilter_Hide();", true);
                    }
                }
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD1", "NORECORD();", true);
            }
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD12", "NORECORD();", true);

        }
        else if (strPageValidationMsg == "BranchErr")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorMsg1", "ErrorMsg(BranchErr);", true);
        }
        else if (strPageValidationMsg == "GroupErr")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorMsg2", "ErrorMsg(GroupErr);", true);
        }
        else if (strPageValidationMsg == "ClientErr")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorMsg3", "ErrorMsg(ClientErr);", true);
        }
    }

    protected void gvwCMBillSummary_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        for (int i = 4; i < gvwCMBillSummary.Columns.Count; i++)
        {
            if ((e.Row.Cells[i].Text == "NetObligation") || (e.Row.Cells[i].Text == "STT")
                || (e.Row.Cells[i].Text == "TranCharge") || (e.Row.Cells[i].Text == "StampDuty")
                || (e.Row.Cells[i].Text == "DlvCharge") || (e.Row.Cells[i].Text == "SebiFee")
                || (e.Row.Cells[i].Text == "OtherCharge") || (e.Row.Cells[i].Text == "ServTax")
                || (e.Row.Cells[i].Text == "NetBillAmount") || (e.Row.Cells[i].Text == "Turnover")
                || (e.Row.Cells[i].Text == "TotalBrokerage") || (e.Row.Cells[i].Text == "UnrealizedServTax")
                || (e.Row.Cells[i].Text == "UnrealizedSTT"))
            {
                e.Row.Cells[i].CssClass = "cellHeader";
            }
            else
            {
                e.Row.Cells[i].CssClass = "cellRecord txt_right";
                e.Row.Cells[i].Text = ConvertToNegetive(e.Row.Cells[i].Text);
            }
        }
    }
    #endregion

    #region CallAjax
    void CallUserList(string WhichCall)
    {
        oGenericMethod = new BusinessLogicLayer.GenericMethod();
        if (WhichCall == "CallAjax-Client")
        {
            string specificSeg = Session["Segmentname"].ToString().Replace("-", " - ");
            //CombinedGroupByQuery = oGenericMethod.GetClient_SegmentFilter(Session["LastCompany"].ToString(), "'" + specificSeg + "'");

            string strQuery_Table = @"(Select cnt_firstName,cnt_middleName,cnt_lastName,crg_tcode,cnt_internalId  from tbl_master_contact,tbl_master_contactExchange Where cnt_internalId=crg_cntID And Left(cnt_internalId,2)='CL' And crg_company='" + Session["LastCompany"].ToString() + "' And crg_exchange in ('" + specificSeg + "')) SpecificCompSpecificSeg";
            string strQuery_FieldName = "Top 10  isnull(Ltrim(Rtrim(cnt_firstName)),'')+' '+isnull(Ltrim(Rtrim(cnt_middleName)),'') +' '+isnull(Ltrim(Rtrim(cnt_lastName)),'')+'['+Ltrim(Rtrim(crg_tcode))+']' TextField, LTRIM(Rtrim(cnt_internalId)) ValueField";
            string strQuery_WhereClause = @"crg_tcode like 'RequestLetter%' Or cnt_firstName like 'RequestLetter%'  Or cnt_middleName like 'RequestLetter%' Or cnt_lastName like 'RequestLetter%'";
            string strQuery_OrderBy = "TextField";
            string strQuery_GroupBy = null;
            CombinedGroupByQuery = oGenericMethod.ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "CLIENT");
            txtSelectionID.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + CombinedGroupByQuery + "')");
        }
        if (WhichCall == "CallAjax-Branch")
        {
            CombinedGroupByQuery = oGenericMethod.GetAllBranch(10);
        }
        if (WhichCall.Contains("CallAjax-Group"))
        {
            string groupType = WhichCall.Split('~')[1].ToString();
            CombinedGroupByQuery = oGenericMethod.GetAllGroups(groupType, 10);
        }
    }
    #endregion

    # region Bind Group Type
    protected int BindGroupType()
    {
        int result = 0;
        CmbGroupType.Items.Clear();
        oGenericMethod = new BusinessLogicLayer.GenericMethod();
        DataTable DtGroup = oGenericMethod.GetDataTable("tbl_master_groupmaster", "distinct gpm_Type as ID,gpm_Type as Value", null);
        if (DtGroup.Rows.Count > 0)
        {
            oAspxHelper = new AspxHelper();
            oAspxHelper.Bind_Combo(CmbGroupType, DtGroup, "ID", "Value", 0);
            CmbGroupType.Items.Insert(0, new ListEditItem("Select GroupType", "0"));
            DtGroup.Dispose();
            result = 1;
        }
        return result;
    }

    protected void CmbGroupType_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
    {
        CmbGroupType.JSProperties["cpBindGroupType"] = null;
        int bindResult = BindGroupType();
        if (bindResult == 1)
            CmbGroupType.JSProperties["cpBindGroupType"] = "Y";
        else
            CmbGroupType.JSProperties["cpBindGroupType"] = "N";
    }
    # endregion

    #region User Defined Methods
    void SetPropertiesValue()
    {
        //Date
        DateFrom = Convert.ToDateTime(DtFrom.Value).ToString("yyyy-MM-dd");
        DateTo = Convert.ToDateTime(DtTo.Value).ToString("yyyy-MM-dd");
        //GroupBy ,GroupByID and GroupByType
        if (CmbGroupBy.SelectedIndex == 0)
        {
            GroupBy = "CLIENT";
            if (RblClient.SelectedIndex == 0) GroupByID = "ALL";
            else
            {
                GroupByID = string.Empty;
                if (HiddenField_ClientBranchGroup.Value.Trim() != "")
                {
                    string[] clntItems = (HiddenField_ClientBranchGroup.Value.Trim()).Split('^');
                    for (int i = 0; i < clntItems.Length; i++)
                    {
                        string[] clntItem = clntItems[i].Split('!');
                        string[] clntIds = clntItem[0].Split('~');
                        if (GroupByID == "")
                            GroupByID = clntIds[0];
                        else
                            GroupByID = GroupByID + "," + clntIds[0];
                    }
                }
                else
                    GroupByID = "Error:Client";
            }
            GroupByType = "";

        }
        else if (CmbGroupBy.SelectedIndex == 1)
        {
            GroupBy = "BRANCH";
            if (RblBranch.SelectedIndex == 0) GroupByID = "ALL";
            else
            {
                GroupByID = string.Empty;
                if (HiddenField_ClientBranchGroup.Value.Trim() != "")
                {
                    string[] branchItems = (HiddenField_ClientBranchGroup.Value.Trim()).Split('^');
                    for (int i = 0; i < branchItems.Length; i++)
                    {
                        string[] branchItem = branchItems[i].Split('!');
                        string[] branchIds = branchItem[0].Split('~');
                        if (GroupByID == "")
                            GroupByID = branchIds[0];
                        else
                            GroupByID = GroupByID + "," + branchIds[0];
                    }
                }
                else
                    GroupByID = "Error:Branch";
            }
            GroupByType = "";
        }
        else if (CmbGroupBy.SelectedIndex == 2)
        {
            GroupBy = "GROUP";
            if (RblGroup.SelectedIndex == 0) GroupByID = "ALL";
            else
            {
                GroupByID = string.Empty;
                if (HiddenField_ClientBranchGroup.Value.Trim() != "")
                {
                    string[] grpItems = (HiddenField_ClientBranchGroup.Value.Trim()).Split('^');
                    for (int i = 0; i < grpItems.Length; i++)
                    {
                        string[] grpItem = grpItems[i].Split('!');
                        string[] grpIds = grpItem[0].Split('~');
                        if (GroupByID == "")
                            GroupByID = grpIds[0];
                        else
                            GroupByID = GroupByID + "," + grpIds[0];
                    }
                }
                else
                    GroupByID = "Error:Group";
            }
            if (CmbGroupType.SelectedIndex != 0) GroupByType = CmbGroupType.SelectedItem.Value.ToString();
        }
        //OrderBy
        OrderBy = CmbOrderBy.SelectedItem.Value.ToString();
        //ReportType
        ReportType = CmbReportType.SelectedItem.Value.ToString();
        //ReportBy
        ReportBy = CmbReportBy.SelectedItem.Value.ToString();
        //PageSize
        PageSize = 25;
        //Company
        Company = Session["LastCompany"].ToString();
        //Segment
        Segment = Session["usersegid"].ToString().Trim();
    }

    string PageValidation()
    {
        string strError = String.Empty;
        if (GroupBy == "BRANCH")
        {
            if (GroupByID.Split(':')[0] == "Error")
                strError = "BranchErr";
        }
        else if (GroupBy == "GROUP")
        {
            if (GroupByID.Split(':')[0] == "Error")
                strError = "GroupErr";
        }
        else
        {
            if (GroupByID.Split(':')[0] == "Error")
                strError = "ClientErr";
        }
        return strError;
    }

    protected string ConvertToShortString(string input)
    {
        if (input.Length > 40)
        {
            input = input.Substring(0, 40);
        }
        if ((input.Contains("*Branch")) || (input.Contains("*Group")) || (input.Contains("*Client")) || (input.Contains("*Grand")))
            input = "<b>" + input + "</b>";

        return input;
    }

    protected string ConvertToNegetive(string input)
    {
        if (input.Contains("-"))
        {
            input = input.Replace("-", "");
            input = "<span style=\"color:red\">(" + input + ")</span>";
        }
        return input;
    }

    private static DataSet GetData()
    {
        //oGenericMethod = new GenericMethod();
        //string[] strSpParam = new string[12];
        //strSpParam[0] = "DateFrom|" + GenericStoreProcedure.ParamDBType.DateTime + "|15|" + DateFrom + "|" + GenericStoreProcedure.ParamType.ExParam;
        //strSpParam[1] = "DateTo|" + GenericStoreProcedure.ParamDBType.DateTime + "|15|" + DateTo + "|" + GenericStoreProcedure.ParamType.ExParam;
        //strSpParam[2] = "GroupBy|" + GenericStoreProcedure.ParamDBType.Varchar + "|10|" + GroupBy + "|" + GenericStoreProcedure.ParamType.ExParam;
        //strSpParam[3] = "GroupByID|" + GenericStoreProcedure.ParamDBType.Varchar + "|-1|" + GroupByID + "|" + GenericStoreProcedure.ParamType.ExParam;
        //strSpParam[4] = "GroupByType|" + GenericStoreProcedure.ParamDBType.Varchar + "|200|" + GroupByType + "|" + GenericStoreProcedure.ParamType.ExParam;
        //strSpParam[5] = "SegmentID|" + GenericStoreProcedure.ParamDBType.Int + "|4|" + Convert.ToInt32(Session["usersegid"].ToString().Trim()) + "|" + GenericStoreProcedure.ParamType.ExParam;
        //strSpParam[6] = "OrderBy|" + GenericStoreProcedure.ParamDBType.Int + "|4|" + OrderBy + "|" + GenericStoreProcedure.ParamType.ExParam;
        //strSpParam[7] = "ReportType|" + GenericStoreProcedure.ParamDBType.Int + "|4|" + ReportType + "|" + GenericStoreProcedure.ParamType.ExParam;
        //strSpParam[8] = "ReportBy|" + GenericStoreProcedure.ParamDBType.Varchar + "|1|" + ReportBy + "|" + GenericStoreProcedure.ParamType.ExParam;
        //strSpParam[9] = "PageNumber|" + GenericStoreProcedure.ParamDBType.Varchar + "|10|" + PageNumber + "|" + GenericStoreProcedure.ParamType.ExParam;
        //strSpParam[10] = "PageSize|" + GenericStoreProcedure.ParamDBType.Varchar + "|10|" + PageSize + "|" + GenericStoreProcedure.ParamType.ExParam;
        //strSpParam[11] = "CompanyID|" + GenericStoreProcedure.ParamDBType.Varchar + "|10|" + Company + "|" + GenericStoreProcedure.ParamType.ExParam;
        //oGenericStoreProcedure = new GenericStoreProcedure();
        //DataSet ds = new DataSet();
        //ds = oGenericStoreProcedure.Procedure_DataSet(strSpParam, "Report_CMBillSummary");
        //return ds;

        string[] InputName = new string[12];
        string[] InputType = new string[12];
        string[] InputValue = new string[12];

        InputName[0] = "DateFrom";
        InputName[1] = "DateTo";
        InputName[2] = "GroupBy";
        InputName[3] = "GroupByID";
        InputName[4] = "GroupByType";
        InputName[5] = "SegmentID";
        InputName[6] = "OrderBy";
        InputName[7] = "ReportType";
        InputName[8] = "ReportBy";
        InputName[9] = "PageNumber";
        InputName[10] = "PageSize";
        InputName[11] = "CompanyID";

        InputType[0] = "V";
        InputType[1] = "V";
        InputType[2] = "V";
        InputType[3] = "V";
        InputType[4] = "V";
        InputType[5] = "V";
        InputType[6] = "V";
        InputType[7] = "V";
        InputType[8] = "V";
        InputType[9] = "V";
        InputType[10] = "V";
        InputType[11] = "V";

        InputValue[0] = DateFrom;
        InputValue[1] = DateTo;
        InputValue[2] = GroupBy;
        InputValue[3] = GroupByID;
        InputValue[4] = GroupByType;
        InputValue[5] = Segment;
        InputValue[6] = OrderBy;
        InputValue[7] = ReportType;
        InputValue[8] = ReportBy;
        InputValue[9] = PageNumber.ToString();
        InputValue[10] = PageSize.ToString();
        InputValue[11] = Company;

        return BusinessLogicLayer.SQLProcedures.SelectProcedureArrDS("Report_CMBillSummary", InputName, InputType, InputValue);
    }

    public static DataSet GetCMBillSummaryData()
    {
        return GetData();
    }

    [WebMethod]
    public static string GetCMBillSummary(int pageNumber)
    {
        PageNumber = pageNumber;

        return GetCMBillSummaryData().GetXml();
    }

    protected string GetTotalRecords_ShowColumns(DataTable dt)
    {
        FilteredShowColumns = "Y~Y~Y";//====For Client~SettNum~BillDate====
        if (dt.Columns.Contains("NetObligation"))
        {
            TotNetObligation = dt.Rows[0]["NetObligation"].ToString();
            FilteredShowColumns = FilteredShowColumns + "~Y";
        }
        else
            FilteredShowColumns = FilteredShowColumns + "~N";
        if (dt.Columns.Contains("STT"))
        {
            TotSTT = dt.Rows[0]["STT"].ToString();
            FilteredShowColumns = FilteredShowColumns + "~Y";
        }
        else
            FilteredShowColumns = FilteredShowColumns + "~N";
        if (dt.Columns.Contains("TranCharge"))
        {
            TotTranCharge = dt.Rows[0]["TranCharge"].ToString();
            FilteredShowColumns = FilteredShowColumns + "~Y";
        }
        else
            FilteredShowColumns = FilteredShowColumns + "~N";
        if (dt.Columns.Contains("StampDuty"))
        {
            TotStampDuty = dt.Rows[0]["StampDuty"].ToString();
            FilteredShowColumns = FilteredShowColumns + "~Y";
        }
        else
            FilteredShowColumns = FilteredShowColumns + "~N";
        if (dt.Columns.Contains("DlvCharge"))
        {
            TotDlvCharge = dt.Rows[0]["DlvCharge"].ToString();
            FilteredShowColumns = FilteredShowColumns + "~Y";
        }
        else
            FilteredShowColumns = FilteredShowColumns + "~N";
        if (dt.Columns.Contains("SebiFee"))
        {
            TotSebiFee = dt.Rows[0]["SebiFee"].ToString();
            FilteredShowColumns = FilteredShowColumns + "~Y";
        }
        else
            FilteredShowColumns = FilteredShowColumns + "~N";
        if (dt.Columns.Contains("OtherCharge"))
        {
            TotOtherCharge = dt.Rows[0]["OtherCharge"].ToString();
            FilteredShowColumns = FilteredShowColumns + "~Y";
        }
        else
            FilteredShowColumns = FilteredShowColumns + "~N";
        if (dt.Columns.Contains("ServTax"))
        {
            TotServTax = dt.Rows[0]["ServTax"].ToString();
            FilteredShowColumns = FilteredShowColumns + "~Y";
        }
        else
            FilteredShowColumns = FilteredShowColumns + "~N";
        if (dt.Columns.Contains("NetBillAmount"))
        {
            TotNetBillAmount = dt.Rows[0]["NetBillAmount"].ToString();
            FilteredShowColumns = FilteredShowColumns + "~Y";
        }
        else
            FilteredShowColumns = FilteredShowColumns + "~N";
        if (dt.Columns.Contains("Turnover"))
        {
            TotTurnover = dt.Rows[0]["Turnover"].ToString();
            FilteredShowColumns = FilteredShowColumns + "~Y";
        }
        else
            FilteredShowColumns = FilteredShowColumns + "~N";
        if (dt.Columns.Contains("TotalBrokerage"))
        {
            TotTotalBrokerage = dt.Rows[0]["TotalBrokerage"].ToString();
            FilteredShowColumns = FilteredShowColumns + "~Y";
        }
        else
            FilteredShowColumns = FilteredShowColumns + "~N";
        if (dt.Columns.Contains("UnrealizedServTax"))
        {
            TotUnrealizedServTax = dt.Rows[0]["UnrealizedServTax"].ToString();
            FilteredShowColumns = FilteredShowColumns + "~Y";
        }
        else
            FilteredShowColumns = FilteredShowColumns + "~N";
        if (dt.Columns.Contains("UnrealizedSTT"))
        {
            TotUnrealizedSTT = dt.Rows[0]["UnrealizedSTT"].ToString();
            FilteredShowColumns = FilteredShowColumns + "~Y";
        }
        else
            FilteredShowColumns = FilteredShowColumns + "~N";

        HDNFilterCol.Value = FilteredShowColumns;
        return FilteredShowColumns;// For in Export_Excel to Show Columns 
    }

    void Export_Excel(DataSet DsExport)
    {
        string strSavePath = String.Empty;
        oGenericMethod = new BusinessLogicLayer.GenericMethod();
        string exlDateTime = oGenericMethod.GetDate(113).ToString();
        string exlTime = exlDateTime.Replace(":", "");
        exlTime = exlTime.Replace(" ", "");
        string FileName = "CMBillSummary_" + exlTime;
        strSavePath = "~/Documents/";

        string strReportHeader = null;
        strReportHeader = "CM Bill Summary For ";
        if (ReportType == "1") strReportHeader = strReportHeader + CmbReportType.SelectedItem.Text.ToString() + " Report";

        string searchCriteria = null;
        searchCriteria = "Search By From "+oConverter.ArrangeDate2(DateFrom)+ " To "+oConverter.ArrangeDate2(DateTo)+" Date Range, ";

        if (CmbGroupBy.SelectedItem.Value.ToString() == "B")
        {
            if (RblBranch.SelectedIndex == 0)
                searchCriteria += "All";
            else
                searchCriteria += "Selected";
            searchCriteria += " Branch";
        }
        else if (CmbGroupBy.SelectedItem.Value.ToString() == "G")
        {
            if (RblGroup.SelectedIndex == 0)
                searchCriteria += "All";
            else if (RblGroup.SelectedIndex == 1)
                searchCriteria += "Selected";
            searchCriteria += " Group Of " + GroupByType + " Group Type ";
        }
        else if (CmbGroupBy.SelectedItem.Value.ToString() == "C")
        {
            if (RblClient.SelectedIndex == 0)
                searchCriteria += "All";
            else
                searchCriteria += "Selected";
            searchCriteria += " Client";
        }

        searchCriteria = searchCriteria + ", " + CmbOrderBy.SelectedItem.Text.ToString() + " Order By";        

        string[] strHead = new string[3];
        strHead[0] = exlDateTime;
        strHead[1] = searchCriteria;
        strHead[2] = strReportHeader;

        oGenericExcelExport = new GenericExcelExport();
        DataTable DtExport = new DataTable();

        if (DsExport.Tables[0].Rows.Count > 1)
        {
            DtExport = DsExport.Tables[0];
            DtExport.Columns.Remove("AutoID");
            DtExport.AcceptChanges();
            if (ReportType == "1")
            {
                string[] ColumnType = { "V", "V", "V", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N" };
                string[] ColumnSize = { "150","10", "12", "28,6", "28,6", "28,6", "28,6", "28,6", "28,6", "28,6", "28,6", "28,6", "28,6", "28,6", "28,6", "28,6" };
                string[] ColumnWidthSize = { "30","12", "12", "15", "15", "15", "15", "15", "15", "15", "15", "15", "15", "15", "15", "15" };

                ArrayList ColumnTypeList = new ArrayList(ColumnType);
                ArrayList ColumnSizeList = new ArrayList(ColumnSize);
                ArrayList ColumnWidthSizeList = new ArrayList(ColumnWidthSize);

                int removeCounter = 0;
                string[] filterDXLCol = (GetTotalRecords_ShowColumns(DtExport).ToString()).Split('~');
                for (int i = 0; i <= DtExport.Columns.Count - 1; i++)
                {
                    if (filterDXLCol[i].ToString() == "N")
                    {
                        ColumnTypeList.RemoveAt(i - removeCounter);
                        ColumnSizeList.RemoveAt(i - removeCounter);
                        ColumnWidthSizeList.RemoveAt(i - removeCounter);

                        //DtExport.Columns.RemoveAt(i - removeCounter);
                        //DtExport.AcceptChanges();

                        removeCounter = removeCounter + 1;
                    }
                }

                ColumnType = (string[])ColumnTypeList.ToArray(typeof(string));
                ColumnSize = (string[])ColumnSizeList.ToArray(typeof(string));
                ColumnWidthSize = (string[])ColumnWidthSizeList.ToArray(typeof(string));

                oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, DtExport, Server.MapPath(strSavePath), "2007", FileName, strHead, null);
            }
        }
    }
    #endregion
}
