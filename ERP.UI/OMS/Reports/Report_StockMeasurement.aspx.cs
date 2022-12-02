using System;
using System.Data;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_Report_StockMeasurement : System.Web.UI.Page//, System.Web.UI.ICallbackEventHandler
    {
        #region Global Variable
        //string data;
        BusinessLogicLayer.GenericMethod oGenericMethod;
        BusinessLogicLayer.GenericStoreProcedure oGenericStoreProcedure;
        AspxHelper oAspxHelper;
        //string CombinedGroupByQuery = null;
        Converter oconverter = new Converter();
        #endregion

        #region PropertyVariable
        string DateFrom; string DateTo;
        //string GroupBy; string GroupByID; string GroupByType; string ContractType;
        #endregion

        #region Page Properties
        public string P_DateTo
        {
            get { return DateTo; }
            set { DateTo = value; }
        }
        public string P_DateFrom
        {
            get { return DateFrom; }
            set { DateFrom = value; }
        }
        //public string P_GroupBy
        //{
        //    get { return GroupBy; }
        //    set { GroupBy = value; }
        //}
        //public string P_GroupByID
        //{
        //    get { return GroupByID; }
        //    set { GroupByID = value; }
        //}
        //public string P_GroupByType
        //{
        //    get { return GroupByType; }
        //    set { GroupByType = value; }
        //}
        //public string P_ContractType
        //{
        //    get { return ContractType; }
        //    set { ContractType = value; }
        //}
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
            this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>Height('500','500');</script>");
            if (!IsPostBack)
            {
                oGenericMethod = new BusinessLogicLayer.GenericMethod();
                dtFrom.Value = oGenericMethod.GetDate();
                dtTo.Value = oGenericMethod.GetDate();

                Page.ClientScript.RegisterStartupScript(GetType(), "PageLoad12", "<script>PageLoad();</script>");
            }
            ////_____For performing operation without refreshing page___//
            //String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            //String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            ////___________-end here___// 
        }

        //string ICallbackEventHandler.GetCallbackResult()
        //{
        //    return data;
        //}

        //void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        //{
        //    string id = eventArgument.ToString();
        //    if (id == "CallAjax-Client" || id == "CallAjax-Branch" || id.Contains("CallAjax-Group"))
        //    {
        //        CallUserList(id);
        //        CombinedGroupByQuery = CombinedGroupByQuery.Replace("\\'", "'");
        //        data = "AjaxQuery@" + CombinedGroupByQuery;
        //    }
        //    else
        //    {
        //        string[] idlist = id.Split('^');
        //        string recieveServerIDs = "";
        //        for (int i = 0; i < idlist.Length; i++)
        //        {
        //            string[] strVal = idlist[i].Split('!');
        //            string[] ids = strVal[0].Split('~');
        //            string whichCall = ids[ids.Length - 1];
        //            if (whichCall == "BRANCH")
        //            {
        //                if (recieveServerIDs == "")
        //                    recieveServerIDs = idlist[i];
        //                else
        //                    recieveServerIDs += "^" + idlist[i];
        //                data = "Branch@" + recieveServerIDs.ToString();
        //            }
        //            else if (whichCall == "GROUP")
        //            {
        //                if (recieveServerIDs == "")
        //                    recieveServerIDs = idlist[i];
        //                else
        //                    recieveServerIDs += "^" + idlist[i];
        //                data = "Group@" + recieveServerIDs.ToString();
        //            }
        //            else if (whichCall == "CLIENT")
        //            {
        //                if (recieveServerIDs == "")
        //                    recieveServerIDs = idlist[i];
        //                else
        //                    recieveServerIDs += "^" + idlist[i];
        //                data = "Client@" + recieveServerIDs.ToString();
        //            }
        //        }
        //    }
        //}

        void SetPropertiesValue()
        {
            //Date
            DateFrom = Convert.ToDateTime(dtFrom.Value).ToString("yyyy-MM-dd");
            DateTo = Convert.ToDateTime(dtTo.Value).ToString("yyyy-MM-dd");
            //GroupBy ,GroupByID and GroupByType
            //if (CmbGroupBy.SelectedIndex == 0)
            //{
            //    GroupBy = "CLIENT";
            //    if (RblClient.SelectedIndex == 0) GroupByID = "ALL";
            //    else
            //    {
            //        GroupByID = string.Empty;
            //        if (HiddenField_ClientBranchGroup.Value.Trim() != "")
            //        {
            //            string[] clntItems = (HiddenField_ClientBranchGroup.Value.Trim()).Split('^');
            //            for (int i = 0; i < clntItems.Length; i++)
            //            {
            //                string[] clntItem = clntItems[i].Split('!');
            //                string[] clntIds = clntItem[0].Split('~');
            //                if (GroupByID == "")
            //                    GroupByID = clntIds[0];
            //                else
            //                    GroupByID = GroupByID + "," + clntIds[0];
            //            }
            //        }
            //        else
            //            GroupByID = "Error:Client";
            //    }
            //    GroupByType = "";

            //}
            //else if (CmbGroupBy.SelectedIndex == 1)
            //{
            //    GroupBy = "BRANCH";
            //    if (RblBranch.SelectedIndex == 0) GroupByID = "ALL";
            //    else
            //    {
            //        GroupByID = string.Empty;
            //        if (HiddenField_ClientBranchGroup.Value.Trim() != "")
            //        {
            //            string[] branchItems = (HiddenField_ClientBranchGroup.Value.Trim()).Split('^');
            //            for (int i = 0; i < branchItems.Length; i++)
            //            {
            //                string[] branchItem = branchItems[i].Split('!');
            //                string[] branchIds = branchItem[0].Split('~');
            //                if (GroupByID == "")
            //                    GroupByID = branchIds[0];
            //                else
            //                    GroupByID = GroupByID + "," + branchIds[0];
            //            }
            //        }
            //        else
            //            GroupByID = "Error:Branch";
            //    }
            //    GroupByType = "";
            //}
            //else if (CmbGroupBy.SelectedIndex == 2)
            //{
            //    GroupBy = "GROUP";
            //    if (RblGroup.SelectedIndex == 0) GroupByID = "ALL";
            //    else
            //    {
            //        GroupByID = string.Empty;
            //        if (HiddenField_ClientBranchGroup.Value.Trim() != "")
            //        {
            //            string[] grpItems = (HiddenField_ClientBranchGroup.Value.Trim()).Split('^');
            //            for (int i = 0; i < grpItems.Length; i++)
            //            {
            //                string[] grpItem = grpItems[i].Split('!');
            //                string[] grpIds = grpItem[0].Split('~');
            //                if (GroupByID == "")
            //                    GroupByID = grpIds[0];
            //                else
            //                    GroupByID = GroupByID + "," + grpIds[0];
            //            }
            //        }
            //        else
            //            GroupByID = "Error:Group";
            //    }
            //    if (CmbGroupType.SelectedIndex != 0) GroupByType = CmbGroupType.SelectedItem.Value.ToString();
            //}
            //ContractType = CmbContractType.SelectedItem.Value.ToString();
        }

        string PageValidation()
        {
            string strError = String.Empty;
            //if (GroupBy == "BRANCH")
            //{
            //    if (GroupByID.Split(':')[0] == "Error")
            //        strError = "BranchErr";
            //}
            //else if (GroupBy == "GROUP")
            //{
            //    if (GroupByID.Split(':')[0] == "Error")
            //        strError = "GroupErr";
            //}
            //else
            //{
            //    if (GroupByID.Split(':')[0] == "Error")
            //        strError = "ClientErr";
            //}
            return strError;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            SetPropertiesValue();
            string strPageValidationMsg = PageValidation();
            if (strPageValidationMsg == String.Empty)
            {
                Procedure();
            }
            //else if (strPageValidationMsg == "BranchErr")
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorMsg1", "ErrorMsg(BranchErr);", true);
            //}
            //else if (strPageValidationMsg == "GroupErr")
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorMsg2", "ErrorMsg(GroupErr);", true);
            //}
            //else if (strPageValidationMsg == "ClientErr")
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorMsg3", "ErrorMsg(ClientErr);", true);
            //}
        }
        #endregion

        #region CallAjax
        //void CallUserList(string WhichCall)
        //{
        //    oGenericMethod = new GenericMethod();
        //    if (WhichCall == "CallAjax-Client")
        //    {
        //        string specificSeg = Session["Segmentname"].ToString().Replace("-", " - ");
        //        //CombinedGroupByQuery = oGenericMethod.GetClient_SegmentFilter(Session["LastCompany"].ToString(), "'" + specificSeg + "'");

        //        string strQuery_Table = @"(Select cnt_firstName,cnt_middleName,cnt_lastName,crg_tcode,cnt_internalId  from tbl_master_contact,tbl_master_contactExchange Where cnt_internalId=crg_cntID And Left(cnt_internalId,2)='CL' And crg_company='" + Session["LastCompany"].ToString() + "' And crg_exchange in ('" + specificSeg + "')) SpecificCompSpecificSeg";
        //        string strQuery_FieldName = "Top 10  isnull(Ltrim(Rtrim(cnt_firstName)),'')+' '+isnull(Ltrim(Rtrim(cnt_middleName)),'') +' '+isnull(Ltrim(Rtrim(cnt_lastName)),'')+'['+Ltrim(Rtrim(crg_tcode))+']' TextField, LTRIM(Rtrim(cnt_internalId)) ValueField";
        //        string strQuery_WhereClause = @"crg_tcode like 'RequestLetter%' Or cnt_firstName like 'RequestLetter%'  Or cnt_middleName like 'RequestLetter%' Or cnt_lastName like 'RequestLetter%'";
        //        string strQuery_OrderBy = "TextField";
        //        string strQuery_GroupBy = null;
        //        CombinedGroupByQuery = oGenericMethod.ReturnCombinedQuery(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_GroupBy, strQuery_OrderBy, "CLIENT");
        //        txtSelectionID.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + CombinedGroupByQuery + "')");
        //    }
        //    if (WhichCall == "CallAjax-Branch")
        //    {
        //        CombinedGroupByQuery = oGenericMethod.GetAllBranch(10);
        //    }
        //    if (WhichCall.Contains("CallAjax-Group"))
        //    {
        //        string groupType = WhichCall.Split('~')[1].ToString();
        //        CombinedGroupByQuery = oGenericMethod.GetAllGroups(groupType, 10);
        //    }
        //}
        #endregion

        # region Bind Group Type
        //protected int BindGroupType()
        //{
        //    int result = 0;
        //    CmbGroupType.Items.Clear();
        //    oGenericMethod = new GenericMethod();
        //    DataTable DtGroup = oGenericMethod.GetDataTable("tbl_master_groupmaster", "distinct gpm_Type as ID,gpm_Type as Value", null);
        //    if (DtGroup.Rows.Count > 0)
        //    {
        //        oAspxHelper = new AspxHelper();
        //        oAspxHelper.Bind_Combo(CmbGroupType, DtGroup, "ID", "Value", 0);
        //        CmbGroupType.Items.Insert(0, new ListEditItem("Select GroupType", "0"));
        //        DtGroup.Dispose();
        //        result = 1;
        //    }
        //    return result;
        //}

        //protected void CmbGroupType_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        //{
        //    CmbGroupType.JSProperties["cpBindGroupType"] = null;
        //    int bindResult = BindGroupType();
        //    if (bindResult == 1)
        //        CmbGroupType.JSProperties["cpBindGroupType"] = "Y";
        //    else
        //        CmbGroupType.JSProperties["cpBindGroupType"] = "N";
        //}
        # endregion

        #region User Defined Methods
        protected void Procedure()
        {
            oGenericMethod = new BusinessLogicLayer.GenericMethod();
            string[] strSpParam = new string[4];
            strSpParam[0] = "CompanyID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|10|" + Session["LastCompany"].ToString() + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[1] = "ExchangeSegmentID|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|5|" + Session["usersegid"].ToString().Trim() + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[2] = "ProductEffectFrom|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|15|" + DateFrom + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            strSpParam[3] = "ProductEffectTo|" + BusinessLogicLayer.GenericStoreProcedure.ParamDBType.Varchar + "|15|" + DateTo + "|" + BusinessLogicLayer.GenericStoreProcedure.ParamType.ExParam;
            //strSpParam[1] = "Finyear|" + GenericStoreProcedure.ParamDBType.Varchar + "|10|" + Session["LastFinYear"].ToString() + "|" + GenericStoreProcedure.ParamType.ExParam;
            //strSpParam[3] = "SettlementNumber|" + GenericStoreProcedure.ParamDBType.Varchar + "|7|" + Session["LastSettNo"].ToString().Trim().Substring(0, 7).ToString() + "|" + GenericStoreProcedure.ParamType.ExParam;
            //strSpParam[5] = "GroupBy|" + GenericStoreProcedure.ParamDBType.Varchar + "|15|" + GroupBy + "|" + GenericStoreProcedure.ParamType.ExParam;
            //strSpParam[6] = "GroupByID|" + GenericStoreProcedure.ParamDBType.Varchar + "|-1|" + GroupByID + "|" + GenericStoreProcedure.ParamType.ExParam;
            //strSpParam[7] = "GroupByType|" + GenericStoreProcedure.ParamDBType.Varchar + "|50|" + GroupByType + "|" + GenericStoreProcedure.ParamType.ExParam;
            //strSpParam[8] = "ContractType|" + GenericStoreProcedure.ParamDBType.Varchar + "|1|" + ContractType + "|" + GenericStoreProcedure.ParamType.ExParam;
            //strSpParam[9] = "FileType|" + GenericStoreProcedure.ParamDBType.Varchar + "|10|" + exportmode.ToString().Trim() + "|" + GenericStoreProcedure.ParamType.ExParam;
            oGenericStoreProcedure = new BusinessLogicLayer.GenericStoreProcedure();
            DataSet ds = new DataSet();
            ds = oGenericStoreProcedure.Procedure_DataSet(strSpParam, "Report_StockMeasurement");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ExportToExcel_Generic(ds, "2007");
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "NORECORD12", "<script>NORECORD();</script>");
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "NORECORD23", "<script>NORECORD();</script>");
            }
        }

        protected void ExportToExcel_Generic(DataSet excelDS, string ExcelVersion)
        {
            if (excelDS.Tables[0].Rows.Count > 0)
            {

                GenericExcelExport oGenericExcelExport = new GenericExcelExport();
                string strDownloadFileName = "";
                oGenericMethod = new BusinessLogicLayer.GenericMethod();
                string exlDateTime = oGenericMethod.GetDate(113).ToString();
                string exlTime = exlDateTime.Replace(":", "");
                exlTime = exlTime.Replace(" ", "");
                string FileName = "Stock_" + exlTime;
                strDownloadFileName = "~/Documents/";
                string strTimePeriod = null;
                strTimePeriod = "For The Date Between " + oconverter.ArrangeDate2(DateFrom) + "  To " + oconverter.ArrangeDate2(DateTo);
                string searchCriteria = null;
                searchCriteria = "Search By ";
                //if (rdbSegmentAll.Checked == true)
                //    searchCriteria += "All Segment, ";
                //else
                //    searchCriteria += "Selected Segment, ";
                //if (ddlGroup.SelectedValue == "1")
                //{
                //    if (rdddlgrouptypeAll.Checked == true)
                //        searchCriteria += "All " + ddlGroup.SelectedItem.Text.ToString().Trim();
                //    else if (rdddlgrouptypeSelected.Checked == true)
                //        searchCriteria += "Selected " + ddlGroup.SelectedItem.Text.ToString().Trim();
                //}
                //else
                //{
                //    if (rdbranchclientAll.Checked == true)
                //        searchCriteria += "All " + ddlGroup.SelectedItem.Text.ToString().Trim();
                //    else
                //        searchCriteria += "Selected " + ddlGroup.SelectedItem.Text.ToString().Trim();
                //}
                string[] strHead = new string[5];
                strHead[0] = exlDateTime;
                strHead[1] = searchCriteria;
                strHead[2] = strTimePeriod;
                strHead[3] = "Segment Wise Stock Analysis";
                oGenericMethod = new BusinessLogicLayer.GenericMethod();
                DataTable dtcompname = oGenericMethod.GetDataTable(" tbl_master_company  ", "cmp_Name", "cmp_internalid='" + HttpContext.Current.Session["LastCompany"] + "' ");
                strHead[4] = "Company Name " + dtcompname.Rows[0]["cmp_Name"].ToString();


                //=====Static Columns=====
                string[] ColumnType = { "V", "V", "V", "V", "V", "N", "N", "N", "V" };
                string[] ColumnSize = { "10", "10", "25", "25", "4", "28,6", "28,4", "28,4", "10" };
                string[] ColumnWidthSize = { "10", "8", "20", "20", "5", "15", "15", "15", "10" };

                oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, excelDS.Tables[0], Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD2", "NORECORD();", true);
            }
        }
        #endregion
    }
}