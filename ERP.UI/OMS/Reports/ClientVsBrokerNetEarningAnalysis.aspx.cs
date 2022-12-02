using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_ClientVsBrokerNetEarningAnalysis : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        #region Global Variable
        string data;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        GenericMethod oGenericMethod;
        GenericExcelExport oGenericExcelExport;
        string CombinedGroupByQuery = null;
        protected DateTime currentFromDate;
        protected DateTime currentToDate;
        #endregion

        #region PropertyVariable
        string Company; string DateFrom; string DateTo;
        string Segment; string ReportType;
        string GroupBy; string GroupByID; string GroupByType;
        #endregion

        #region Page Properties
        public string P_Company
        {
            get { return Company; }
            set { Company = value; }
        }
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
        public string P_Segment
        {
            get { return Segment; }
            set { Segment = value; }
        }
        public string P_ReportType
        {
            get { return ReportType; }
            set { ReportType = value; }
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
        #endregion

        #region CallAjax
        void CallUserList(string WhichCall)
        {
            oGenericMethod = new GenericMethod();
            if (WhichCall == "CallAjax-Branch")
            {
                CombinedGroupByQuery = oGenericMethod.GetAllBranch();
            }
            if (WhichCall == "CallAjax-Group")
            {
                CombinedGroupByQuery = oGenericMethod.GetAllGroups(ddlGrouptype.SelectedValue.ToString());
            }
            if (WhichCall == "CallAjax-Client")
            {
                string specificSeg = Session["Segmentname"].ToString().Replace("-", " - ");
                CombinedGroupByQuery = oGenericMethod.GetClient_SegmentFilter(Session["LastCompany"].ToString(), "'" + specificSeg + "'");//"'INMX - COMM'"
            }
        }
        #endregion

        #region Page Methods
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            if (id == "CallAjax-Branch" || id == "CallAjax-Group" || id == "CallAjax-Client")
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

        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        #endregion

        #region User Define Methods
        void SetPropertiesValue()
        {
            //Company
            Company = Session["LastCompany"].ToString();
            //Segment
            Segment = Session["usersegid"].ToString();
            //Date
            DateFrom = Convert.ToDateTime(DtFrom.Value).ToString("yyyy-MM-dd");
            DateTo = Convert.ToDateTime(DtTo.Value).ToString("yyyy-MM-dd");
            //ReportType
            ReportType = CmbReportType.SelectedItem.Value.ToString();
            //GroupBy ,GroupByID and GroupByType
            if (CmbGroupBy.SelectedIndex == 0)
            {
                GroupBy = "BRANCH";
                if (RblBranch.SelectedIndex == 0) GroupByID = "ALL";
                else
                {
                    GroupByID = string.Empty;
                    if (HDNBranch.Value.Trim() != "")
                    {
                        string[] branchItems = (HDNBranch.Value.Trim()).Split('^');
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
            else if (CmbGroupBy.SelectedIndex == 1)
            {
                GroupBy = "GROUP";
                if (RblGroup.SelectedIndex == 0) GroupByID = "ALL";
                else
                {
                    GroupByID = string.Empty;
                    if (HDNGroup.Value.Trim() != "")
                    {
                        string[] grpItems = (HDNGroup.Value.Trim()).Split('^');
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
                if (ddlGrouptype.SelectedIndex != 0) GroupByType = ddlGrouptype.SelectedValue.Trim();
            }
            else
            {
                GroupBy = "CLIENT";
                if (RblClient.SelectedIndex == 0) GroupByID = "ALL";
                else
                {
                    GroupByID = string.Empty;
                    if (HDNClient.Value.Trim() != "")
                    {
                        string[] clntItems = (HDNClient.Value.Trim()).Split('^');
                        for (int i = 0; i < clntItems.Length; i++)
                        {
                            string[] clntItem = clntItems[i].Split('!');
                            string[] clntIds = clntItem[0].Split('~');
                            if (GroupByID == "")
                                GroupByID = clntIds[4];
                            else
                                GroupByID = GroupByID + "," + clntIds[4];
                        }
                    }
                    else
                        GroupByID = "Error:Client";
                }
                GroupByType = "";
            }
        }

        string PageValidation()
        {
            string strError = String.Empty;
            if (GroupBy == "BRANCH")
            {
                if (GroupByID != "" && strError != String.Empty)
                    if (GroupByID.Split(':')[0] == "Error")
                        strError = "There is No Proper Branch Selection!!!";
            }
            else if (GroupBy == "GROUP")
            {
                if (GroupByID != "" && strError != String.Empty)
                    if (GroupByID.Split(':')[0] == "Error")
                        strError = "There is No Proper Group Selection!!!";
            }
            else
            {
                if (GroupByID != "" && strError != String.Empty)
                    if (GroupByID.Split(':')[0] == "Error")
                        strError = "There is No Proper Client Selection!!!";
            }
            return strError;
        }
        #endregion

        # region Bind Group Type
        public void BindGroup()
        {
            ddlGrouptype.Items.Clear();
            // oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            DataTable DtGroup = oDBEngine.GetDataTable("tbl_master_groupmaster", "distinct gpm_Type", null);
            if (DtGroup.Rows.Count > 0)
            {
                ddlGrouptype.DataSource = DtGroup;
                ddlGrouptype.DataTextField = "gpm_Type";
                ddlGrouptype.DataValueField = "gpm_Type";
                ddlGrouptype.DataBind();
                ddlGrouptype.Items.Insert(0, new ListItem("Select GroupType", "0"));
                DtGroup.Dispose();
            }
            else
            {
                ddlGrouptype.Items.Insert(0, new ListItem("Select GroupType", "0"));
            }
        }
        protected void btnhide_Click(object sender, EventArgs e)
        {
            if (CmbGroupBy.SelectedItem.Value.ToString() == "G")
            {
                if (HDNGroup.Value.ToString().Trim() == "")
                {
                    BindGroup();
                }
            }
        }
        # endregion

        #region BusinessLogic
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string[] PageSession = null;
                oGenericMethod = new GenericMethod();
                oGenericMethod.PageInitializer(GenericMethod.WhichCall.DistroyUnWantedSession_AllExceptPage, PageSession);
                oGenericMethod.PageInitializer(GenericMethod.WhichCall.DistroyUnWantedSession_Page, PageSession);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            Page.ClientScript.RegisterStartupScript(GetType(), "PageLoad", "<script>Page_Load();</script>");
            if (!IsPostBack)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "Reset", "<script>Reset();</script>");
                //oDBEngine = new DBEngine(null);
                DateTime Date = oDBEngine.GetDate();
                currentFromDate = Date.AddDays((-1 * Date.Day) + 1);
                currentToDate = Date;
                DtFrom.Value = currentFromDate;
                DtTo.Value = currentToDate;
            }

            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            //___________-end here___//      
        }

        private DataSet GetData()
        {
            string[] InputName = new string[11];
            string[] InputType = new string[11];
            string[] InputValue = new string[11];

            InputName[0] = "CompanyID";
            InputName[1] = "SegmentID";
            InputName[2] = "SettNumber";
            InputName[3] = "SettType";
            InputName[4] = "DateFrom";
            InputName[5] = "DateTo";
            InputName[6] = "ReportType";
            InputName[7] = "OnlyNegative";
            InputName[8] = "GrpBy";
            InputName[9] = "GrpByID";
            InputName[10] = "GrpType";

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

            InputValue[0] = Company;
            InputValue[1] = Segment;
            InputValue[2] = Session["LastSettNo"].ToString().Substring(0, Session["LastSettNo"].ToString().Length - 2);
            InputValue[3] = Session["LastSettNo"].ToString().Substring(Session["LastSettNo"].ToString().Length - 2);
            InputValue[4] = DateFrom;
            InputValue[5] = DateTo;
            InputValue[6] = ReportType;
            if (ChkOnlyNegetive.Checked == true) InputValue[7] = "T";
            else InputValue[7] = "F";
            InputValue[8] = GroupBy;
            InputValue[9] = GroupByID;
            InputValue[10] = GroupByType;

            return SQLProcedures.SelectProcedureArrDS("ClientVsBrokerNetEarningAnalysis", InputName, InputType, InputValue);
        }

        protected void BtnShow_Click(object sender, EventArgs e)
        {
            SetPropertiesValue();
            string strPageValidationMsg = PageValidation();
            if (strPageValidationMsg == String.Empty)
            {
                DataSet ds = new DataSet();
                ds = GetData();
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 1)
                    {
                        ExportToExcel(ds);
                    }
                    else
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD1", "NORECORD();", true);
                }
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD12", "NORECORD();", true);
            }
        }

        void ExportToExcel(DataSet DsExport)
        {
            string strSavePath = String.Empty;
            //oDBEngine = new DBEngine(null);
            string exlDateTime = oDBEngine.GetDate(113).ToString();
            string exlTime = exlDateTime.Replace(":", "");
            exlTime = exlTime.Replace(" ", "");
            string FileName = "ClientVsBrokerNetEarn_" + exlTime;
            strSavePath = "~/Documents/";

            string strReportHeader = null;
            strReportHeader = "Client-Vs-Broker Net Earning Analysis ";
            strReportHeader = strReportHeader + CmbReportType.SelectedItem.Text + " Report";

            string searchCriteria = null;
            searchCriteria = "Search By Date From " + DtFrom.Text + " To " + DtTo.Text + ", ";

            searchCriteria += "Segment - " + Session["Segmentname"].ToString() + ", Settlement Number - " + Session["LastSettNo"].ToString() + " And ";

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
                searchCriteria += " Group Of" + ddlGrouptype.SelectedItem.Text.ToString().Trim() + " Group Type";
            }
            else if (CmbGroupBy.SelectedItem.Value.ToString() == "C")
            {
                if (RblClient.SelectedIndex == 0)
                    searchCriteria += "All";
                else
                    searchCriteria += "Selected";
                searchCriteria += " Client";
            }

            string[] strHead = new string[3];
            strHead[0] = exlDateTime;
            strHead[1] = searchCriteria;
            strHead[2] = strReportHeader;

            oGenericExcelExport = new GenericExcelExport();
            //oDBEngine = new DBEngine(null);
            DataTable DtExport = new DataTable();
            DtExport = DsExport.Tables[0];

            if (DsExport.Tables[0].Rows.Count > 1)
            {
                if (ReportType == "1")//Client,[Trd.Date],[Ord.Number],[Trd.Number],[Trd.Code],[Broker],[Prd.Name],[Lots],[Mkt.Price],[Turnover],[Brkg.Earned],[Brkg.Paid],[Net.Earning]
                {
                    string[] ColumnType = { "V", "V", "V", "V", "V", "V", "V", "N", "N", "N", "N", "N", "N" };
                    string[] ColumnSize = { "150", "15", "20", "20", "20", "120", "50", "20,0", "19,4", "24,2", "22,2", "22,2", "22,2" };
                    string[] ColumnWidthSize = { "30", "15", "10", "10", "10", "30", "30", "12", "12", "16", "12", "12", "12" };

                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, DtExport, Server.MapPath(strSavePath), "2007", FileName, strHead, null);
                }
                if (ReportType == "2")//Client,[Trade Date],[Trd.Code],[Broker],[Prd.Name],[Lots],[Turnover],[Brkg.Earned],[Brkg.Paid],[Net.Earning]
                {
                    string[] ColumnType = { "V", "V", "V", "V", "V", "N", "N", "N", "N", "N" };
                    string[] ColumnSize = { "150", "15", "20", "120", "50", "20,0", "24,2", "22,2", "22,2", "22,2" };
                    string[] ColumnWidthSize = { "30", "15", "10", "30", "30", "12", "16", "12", "12", "12" };

                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, DtExport, Server.MapPath(strSavePath), "2007", FileName, strHead, null);
                }
                if (ReportType == "3")//Client,[Trade Date],[Trd.Code],[Broker],[Lots],[Turnover],[Brkg.Earned],[Brkg.Paid],[Net.Earning]
                {
                    string[] ColumnType = { "V", "V", "V", "V", "N", "N", "N", "N", "N" };
                    string[] ColumnSize = { "150", "15", "20", "120", "20,0", "24,2", "22,2", "22,2", "22,2" };
                    string[] ColumnWidthSize = { "30", "15", "10", "30", "12", "16", "12", "12", "12" };

                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, DtExport, Server.MapPath(strSavePath), "2007", FileName, strHead, null);
                }
                if (ReportType == "4")//Client,[Trd.Code],[Broker],[Prd.Name],[Lots],[Turnover],[Brkg.Earned],[Brkg.Paid],[Net.Earning]
                {
                    string[] ColumnType = { "V", "V", "V", "V", "N", "N", "N", "N", "N" };
                    string[] ColumnSize = { "150", "20", "120", "50", "20,0", "24,2", "22,2", "22,2", "22,2" };
                    string[] ColumnWidthSize = { "30", "10", "30", "30", "12", "16", "12", "12", "12" };

                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, DtExport, Server.MapPath(strSavePath), "2007", FileName, strHead, null);
                }
                if (ReportType == "5")//Client,[Trd.Code],[Broker],[Lots],[Turnover],[Brkg.Earned],[Brkg.Paid],[Net.Earning]
                {
                    string[] ColumnType = { "V", "V", "V", "N", "N", "N", "N", "N" };
                    string[] ColumnSize = { "150", "20", "120", "20,0", "24,2", "22,2", "22,2", "22,2" };
                    string[] ColumnWidthSize = { "30", "10", "30", "12", "16", "12", "12", "12" };

                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, DtExport, Server.MapPath(strSavePath), "2007", FileName, strHead, null);
                }
            }
        }
        #endregion

    }
}