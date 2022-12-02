using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_FOCDXTurnoverBrkgAnalysisForAPeriod : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        #region Global Variable
        string data;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        GenericMethod oGenericMethod;
        GenericExcelExport oGenericExcelExport;
        string CombinedGroupByQuery = null;
        protected DateTime currentFromDate;
        protected DateTime currentToDate;
        protected String ExchSegName;
        #endregion

        #region PropertyVariable
        string Company; string CompanyName; string DateFrom; string DateTo;
        string Segment; string ExchangeSegmentName; string ReportType;
        string GroupBy; string GroupByID; string GroupByType;
        string Product;
        #endregion

        #region Page Properties
        public string P_Company
        {
            get { return Company; }
            set { Company = value; }
        }
        public string P_CompanyName
        {
            get { return CompanyName; }
            set { CompanyName = value; }
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
        public string P_ExchangeSegmentName
        {
            get { return ExchangeSegmentName; }
            set { ExchangeSegmentName = value; }
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
        public string P_Product
        {
            get { return Product; }
            set { Product = value; }
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
                CombinedGroupByQuery = oGenericMethod.GetClient_SegmentFilter(Session["LastCompany"].ToString(), "'" + specificSeg + "'");
            }
            if (WhichCall == "CallAjax-Product")
            {
                string CombinedProductQuery = oGenericMethod.GetAssetsOrDerivative("A", 0, 0, "NA");
                CombinedGroupByQuery = CombinedProductQuery;
            }
        }
        #endregion

        #region Page Methods
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            if (id == "CallAjax-Branch" || id == "CallAjax-Group" || id == "CallAjax-Client" || id == "CallAjax-Product")
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
                    else if (whichCall == "ASSET")
                    {
                        if (recieveServerIDs == "")
                            recieveServerIDs = idlist[i];
                        else
                            recieveServerIDs += "^" + idlist[i];
                        data = "Product@" + recieveServerIDs.ToString();
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
            //Product
            if (ReportType == "A" || ReportType == "S")
            {
                if (RblProduct.SelectedIndex == 0) Product = "ALL";
                else
                {
                    Product = string.Empty;
                    if (HDNProduct.Value.Trim() != "")
                    {
                        string[] prodItems = (HDNProduct.Value.Trim()).Split('^');
                        for (int i = 0; i < prodItems.Length; i++)
                        {
                            string[] prodItem = prodItems[i].Split('!');
                            string[] prodIds = prodItem[0].Split('~');
                            if (Product == "")
                                Product = prodIds[0];
                            else
                                Product = Product + "," + prodIds[0];
                        }
                    }
                    else
                        Product = "Error:Product";
                }
            }
            else
                Product = "";

        }

        string PageValidation()
        {
            string strError = String.Empty;
            if (GroupBy == "BRANCH")
            {
                if (GroupByID != "" && strError != String.Empty)
                    if (GroupByID.Split(':')[0] == "Error")
                        strError = "BranchErr";
            }
            else if (GroupBy == "GROUP")
            {
                if (GroupByID != "" && strError != String.Empty)
                    if (GroupByID.Split(':')[0] == "Error")
                        strError = "GroupErr";
            }
            else
            {
                if (GroupByID != "" && strError != String.Empty)
                    if (GroupByID.Split(':')[0] == "Error")
                        strError = "ClientErr";
            }
            //if (Product != "" && strError != String.Empty)
            if (Product.Split(':')[0] == "Error")
                strError = "ProductErr";
            return strError;
        }
        #endregion

        # region Bind Group Type
        public void BindGroup()
        {
            ddlGrouptype.Items.Clear();
            //oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
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
            //Set Company and Exchange Segment ID,Name            
            Company = Session["LastCompany"].ToString();
            Segment = Session["usersegid"].ToString();
            oGenericMethod = new GenericMethod();
            DataTable dtCompSegDetail = oGenericMethod.GetCompanyDetail(Company, Segment);
            if (dtCompSegDetail.Rows.Count > 0)
            {
                CompanyName = dtCompSegDetail.Rows[0][1].ToString();
                ExchangeSegmentName = dtCompSegDetail.Rows[0][5].ToString();
                ExchSegName = ExchangeSegmentName;
            }
            if (!IsPostBack)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "Reset", "<script>Reset();</script>");

                //Set Current From Date ,To Date
                // oDBEngine = new DBEngine(null);
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
            InputName[3] = "DateFrom";
            InputName[4] = "DateTo";
            InputName[5] = "ReportType";
            InputName[6] = "GrpBy";
            InputName[7] = "GrpByID";
            InputName[8] = "GrpType";
            InputName[9] = "ProductID";
            InputName[10] = "ExchangeSegmentID";

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
            InputValue[3] = DateFrom;
            InputValue[4] = DateTo;
            InputValue[5] = ReportType;
            InputValue[6] = GroupBy;
            InputValue[7] = GroupByID;
            InputValue[8] = GroupByType;
            InputValue[9] = Product;
            InputValue[10] = Session["ExchangeSegmentID"].ToString();//FO-[2,5,20],CDX-[3,6,8,13]      

            if (ExchangeSegmentName == "FO")
                return SQLProcedures.SelectProcedureArrDS("FOTurnover&BrkgAnalysisForAPeriod", InputName, InputType, InputValue);
            else
                return SQLProcedures.SelectProcedureArrDS("CDXTurnover&BrkgAnalysisForAPeriod", InputName, InputType, InputValue);

        }

        protected void BtnShow_Click(object sender, EventArgs e)
        {
            SetPropertiesValue();
            string strPageValidationMsg = PageValidation();
            if (strPageValidationMsg == String.Empty)
            {
                DataSet ds = new DataSet();
                ds = GetData();
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 2)  //for *Grand total and Blank Row Before *Grand Total Show 
                        {
                            ExportToExcel(ds);
                        }
                        else
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD1", "NORECORD();", true);
                    }
                    else
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD12", "NORECORD();", true);
                }
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD123", "NORECORD();", true);
            }
            else if (strPageValidationMsg == "BranchErr")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorMsg1", "ErrorMsg(BranchErr);", true);
            }
            else if (strPageValidationMsg == "GroupErr")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorMsg1", "ErrorMsg(GroupErr);", true);
            }
            else if (strPageValidationMsg == "ClientErr")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorMsg1", "ErrorMsg(ClientErr);", true);
            }
            else if (strPageValidationMsg == "ProductErr")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorMsg1", "ErrorMsg(ProductErr);", true);
            }
        }

        void ExportToExcel(DataSet DsExport)
        {
            string strSavePath = String.Empty;
            // oDBEngine = new DBEngine(null);
            string exlDateTime = oDBEngine.GetDate(113).ToString();
            string exlTime = exlDateTime.Replace(":", "");
            exlTime = exlTime.Replace(" ", "");
            string FileName = "FOTurnoverBrkgAnalysisForAPeriod_" + exlTime;
            strSavePath = "~/Documents/";

            string strReportHeader = null;
            strReportHeader = ExchangeSegmentName + " Segment Turnover & Brokerage Analysis For A Period Of ";
            strReportHeader = strReportHeader + CmbReportType.SelectedItem.Text + " Report";

            string searchCriteria = null;
            searchCriteria = "Search By Date From " + DtFrom.Text + " To " + DtTo.Text + ", ";

            searchCriteria += "Segment - " + Session["Segmentname"].ToString() + ", Settlement Number - " + Session["LastSettNo"].ToString();
            if (ReportType != "A") searchCriteria += " And ";
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
                searchCriteria += " Group Of " + ddlGrouptype.SelectedItem.Text.ToString().Trim() + " Group Type ";
            }
            else if (CmbGroupBy.SelectedItem.Value.ToString() == "C")
            {
                if (RblClient.SelectedIndex == 0)
                    searchCriteria += "All";
                else
                    searchCriteria += "Selected";
                searchCriteria += " Client";
            }
            if (ReportType == "A")
            {
                if (RblProduct.SelectedIndex == 0)
                    searchCriteria += "And All Product";
                else
                    searchCriteria += "And Selected Product";
            }
            string[] strHead = new string[4];
            strHead[0] = exlDateTime;
            strHead[1] = searchCriteria;
            strHead[2] = strReportHeader;
            strHead[3] = CompanyName;
            oGenericExcelExport = new GenericExcelExport();
            //oDBEngine = new DBEngine(null);
            DataTable DtExport = new DataTable();
            DtExport = DsExport.Tables[0];

            if (DsExport.Tables[0].Rows[0][0] != string.Empty)
            {
                if (ReportType == "A")
                {
                    //[Client Name],[Code],[Br.Code],[Product],[Fut Lots],[Fut TO],[Fut Brkg],[Fut Fin.Sett Lots],[Fut Fin.Sett TO],[Fut Fin.Sett Brkg],
                    //[Opt Lots],[Opt TO],[Opt Strk.TO],[Opt Brkg],[Opt Fin.Sett Lots],[Opt Fin.Sett TO],[Opt Fin.Sett StrkTO],[Opt Fin.Sett SpotTO],
                    //[Opt Fin.Sett Brkg],[Total Lots],[Total TO],[Total Brkg]											

                    string[] ColumnType = { "V", "V", "V", "V", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N" };
                    string[] ColumnSize = { "120", "20", "5", "50", "15,0", "22,2", "20,2", "15,0", "22,2", "20,2", "15,0", "22,2", "22,2", "20,2", "15,0", "22,2", "22,2", "22,2", "20,2", "15,0", "24,2", "22,2" };
                    string[] ColumnWidthSize = { "30", "10", "5", "20", "12", "16", "12", "12", "16", "12", "12", "16", "16", "12", "12", "16", "16", "16", "12", "12", "16", "12" };

                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, DtExport, Server.MapPath(strSavePath), "2007", FileName, strHead, null);
                }
                if (ReportType == "B")
                {
                    //[Client Name],[Code],[Br.Code],[Fut Lots],[Fut TO],[Fut Brkg],[Fut Fin.Sett Lots],[Fut Fin.Sett TO],[Fut Fin.Sett Brkg],
                    //[Opt Lots,[Opt TO],[Opt Strk.TO],[Opt Brkg],[Opt Fin.Sett Lots],[Opt Fin.Sett TO],[Opt Fin.Sett StrkTO],[Opt Fin.Sett SpotTO],
                    //[Opt Fin.Sett Brkg],[Total Lots],[Total TO],[Total Brkg]											

                    string[] ColumnType = { "V", "V", "V", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N" };
                    string[] ColumnSize = { "120", "20", "5", "15,0", "22,2", "20,2", "15,0", "22,2", "20,2", "15,0", "22,2", "22,2", "20,2", "15,0", "22,2", "22,2", "22,2", "20,2", "15,0", "24,2", "22,2" };
                    string[] ColumnWidthSize = { "30", "10", "5", "12", "16", "12", "12", "16", "12", "12", "16", "16", "12", "12", "16", "16", "16", "12", "12", "16", "12" };

                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, DtExport, Server.MapPath(strSavePath), "2007", FileName, strHead, null);
                }
                if (ReportType == "C")
                {
                    //[Client Name],[Code],[Br.Code],[Fut Lots],[Fut TO],[Fut Brkg],[Fut Fin.Sett Lots],[Fut Fin.Sett TO],[Fut Fin.Sett Brkg],
                    //[Opt Lots,[Opt TO],[Opt Strk.TO],[Opt Brkg],[Opt Fin.Sett Lots],[Opt Fin.Sett TO],[Opt Fin.Sett StrkTO],[Opt Fin.Sett SpotTO],
                    //[Opt Fin.Sett Brkg],[Total Lots],[Total TO],[Total Brkg]											
                    //STT,TXCharge,CLCharge,SDCharge,SebiFee,OtherCharge,ServTax 

                    string[] ColumnType = { "V", "V", "V", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N" };
                    string[] ColumnSize = { "120", "20", "5", "15,0", "22,2", "20,2", "15,0", "22,2", "20,2", "15,0", "22,2", "22,2", "20,2", "15,0", "22,2", "22,2", "22,2", "20,2", "15,0", "24,2", "22,2", "22,2", "22,2", "22,2", "22,2", "22,2", "22,2", "22,2" };
                    string[] ColumnWidthSize = { "30", "10", "5", "12", "16", "12", "12", "16", "12", "12", "16", "16", "12", "12", "16", "16", "16", "12", "12", "16", "12", "12", "12", "12", "12", "12", "12", "12" };

                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, DtExport, Server.MapPath(strSavePath), "2007", FileName, strHead, null);
                }
                if (ReportType == "S")
                {
                    //[Product],[Fut Lots],[Fut TO],[Fut Brkg],[Fut Fin.Sett Lots],[Fut Fin.Sett TO],[Fut Fin.Sett Brkg],
                    //[Opt Lots,[Opt TO],[Opt Strk.TO],[Opt Brkg],[Opt Fin.Sett Lots],[Opt Fin.Sett TO],[Opt Fin.Sett StrkTO],[Opt Fin.Sett SpotTO],
                    //[Opt Fin.Sett Brkg],[Total Lots],[Total TO],[Total Brkg]											

                    string[] ColumnType = { "V", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N" };
                    string[] ColumnSize = { "50", "15,0", "22,2", "20,2", "15,0", "22,2", "20,2", "15,0", "22,2", "22,2", "20,2", "15,0", "22,2", "22,2", "22,2", "20,2", "15,0", "24,2", "22,2" };
                    string[] ColumnWidthSize = { "20", "12", "16", "12", "12", "16", "12", "12", "16", "16", "12", "12", "16", "16", "16", "12", "12", "16", "12" };

                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, DtExport, Server.MapPath(strSavePath), "2007", FileName, strHead, null);
                }
            }
        }
        #endregion

    }
}