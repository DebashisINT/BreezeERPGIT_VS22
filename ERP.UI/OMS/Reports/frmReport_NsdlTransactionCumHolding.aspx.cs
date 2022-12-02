using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using BusinessLogicLayer;
namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_NsdlTransactionCumHolding : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        #region Global Variable
        string companyId = "";
        string dp = "NSDL";
        private static DataTable DT = new DataTable();
        static string isinid = "", cmpid = "", SettlementID = "";
        static int counter = 0, pageindex = 0, totolRecord = 0;
        PagedDataSource pds = new PagedDataSource();
        private int Repcounter = 0;
        static int BenType;
        static string BenAccId;
        private DataTable dtBenTypeSubtype = new DataTable();
        static string stdate, endDate;
        string data;
        static string Clients;
        string CombinedGroupByQuery = string.Empty;
        //---------For Sending Email
        ExcelFile objExcel = new ExcelFile();
        string EmailHTML = "";
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        static DataTable dtTemp = new DataTable();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        GenericMethod oGenericMethod;
        GenericStoreProcedure oGenericStoreProcedure;
        ClsDropDownlistNameSpace.clsDropDownList clsdrp = new ClsDropDownlistNameSpace.clsDropDownList();

        //static string[] cmpIds = null;
        #endregion

        #region Page Method
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);

                //string[] PageSession = { "SessionddlSelectedBranchGroup" };
                //oGenericMethod = new GenericMethod();
                //oGenericMethod.PageInitializer(GenericMethod.WhichCall.DistroyUnWantedSession_AllExceptPage, PageSession);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");

            //________This script is for firing javascript when page load first___//
            if (!ClientScript.IsStartupScriptRegistered("Today"))
                ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>PageLoad();</script>");
            // ______________________________End Script____________________________//
            if (!IsPostBack)
            {
                counter = 0;
                string currentDate = oDBEngine.GetDate(103).ToString();
                currentDate = currentDate.Replace("/", "-");
                string fromDate = (currentDate.Substring(0, currentDate.IndexOf("-"))).Replace(currentDate.Substring(0, currentDate.IndexOf("-")), "01") + currentDate.Substring(currentDate.IndexOf("-"));
                dtfrom.Text = fromDate;
                dtto.Text = currentDate;
                list.Style["display"] = "none";
                norecord.Style["display"] = "none";
                tblpage.Style["display"] = "none";
                CallUserList(null);
                FillAllBranches();
            }
            BindParams();
            if (Session["SessionddlSelectedBranchGroup"] != null)
            {
                string sessionBranchGroup = Session["SessionddlSelectedBranchGroup"].ToString();
                string[] selBranchGroup = sessionBranchGroup.Split('~');
                if (selBranchGroup[0] == "SelectedBranch")
                {
                    string selectedBranchList = selBranchGroup[1];
                    FillDdlSelectedBranch(selectedBranchList);
                    Session["SessionddlSelectedBranchGroup"] = null;
                }
                if (selBranchGroup[0] == "SelectedGroup")
                {
                    string selectedGroupList = selBranchGroup[1];
                    FillDdlSelectedGroup(selectedGroupList);
                    Session["SessionddlSelectedBranchGroup"] = null;
                }
            }
            counter = counter + 1;
            String cbReference1 = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveSvrData", "context");
            String callbackScript1 = "function CallServer1(arg, context){ " + cbReference1 + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer1", callbackScript1, true);
        }

        void BindParams()
        {
            if (HiddenField_Client.Value.Trim() != "")
            {
                cmpid = Clients;
            }
            else if (rbGroupBy.SelectedIndex == 0 && rbUser.SelectedIndex == 0)
            {
                cmpid = "na";
            }
            if (rbISIN.SelectedIndex == 1 && txtISIN_hidden.Text.Trim() != "")
            {
                isinid = txtISIN_hidden.Text;
            }
            else if (rbISIN.SelectedIndex == 0)
            {
                isinid = "";
            }
            if (rbSettlement.SelectedIndex == 1 && txtSettlement_hidden.Text.Trim() != "")
            {
                SettlementID = txtSettlement_hidden.Text;
            }
            else if (rbSettlement.SelectedIndex == 0)
            {
                SettlementID = "";
            }
        }

        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }

        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            if (id.Contains("CallAjax-ISIN") || id.Contains("CallAjax-Settlement") || id == "CallAjax-Client" || id == "CallAjax-Branch" || id == "CallAjax-Group" || id.Contains("CallAjax-FilteredClient"))
            {
                CallUserList(id);
                CombinedGroupByQuery = CombinedGroupByQuery.Replace("\\'", "'");
                data = "AjaxQuery~" + CombinedGroupByQuery;
            }
            else
            {
                string[] idlist = id.Split('~');
                string[] cl = idlist[1].Split(',');
                string str = "";
                string str1 = "";
                for (int i = 0; i < cl.Length; i++)
                {
                    if (idlist[0] != "Clients")
                    {
                        string[] val = cl[i].Split(';');
                        if (str == "")
                        {
                            str = val[0];
                            str1 = val[0] + ";" + val[1];
                        }
                        else
                        {
                            str += "," + val[0];
                            str1 += "," + val[0] + ";" + val[1];
                        }
                    }
                    else
                    {
                        string[] val = cl[i].Split(';');
                        string[] AcVal = val[0].Split('-');
                        if (str == "")
                        {
                            if (idlist[0] == "Clients")
                            {
                                str = AcVal[0];
                            }
                            else
                            {
                                str = "'" + AcVal[0] + "'";
                                str1 = "'" + AcVal[0] + "'" + ";" + val[1];
                            }
                        }
                        else
                        {
                            if (idlist[0] == "Clients")
                            {
                                str += "," + AcVal[0];
                            }
                            else
                            {
                                str += ",'" + AcVal[0] + "'";
                                str1 += "," + "'" + AcVal[0] + "'" + ";" + val[1];
                            }
                        }
                    }
                }
                if (idlist[0] == "Clients")
                {
                    Clients = str;
                    data = "Clients~" + str;
                }
                else if (idlist[0] == "Branch")
                {
                    data = "Branch~" + str;
                    Session.Add("SessionddlSelectedBranchGroup", "SelectedBranch~" + str);
                }
                else if (idlist[0] == "Group")
                {
                    data = "Group~" + str;
                    Session.Add("SessionddlSelectedBranchGroup", "SelectedGroup~" + str);
                }
            }
        }

        protected void ddlGroupType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string grpType = ddlGroupType.SelectedValue;
            stdate = Convert.ToDateTime(dtfrom.Value).ToString("yyyy-MM-dd");
            endDate = Convert.ToDateTime(dtto.Value).ToString("yyyy-MM-dd");
            if (isinid == "")
                isinid = "na";
            if (SettlementID == "")
                SettlementID = "na";
            FillAllGroups(grpType, stdate, endDate, isinid, SettlementID);
        }

        protected void btnGroupTypehide_Click(object sender, EventArgs e)
        {
            if (rbGroupBy.SelectedItem.Value.ToString() == "G")
            {
                BindGroupType();
            }
        }
        #endregion

        #region CallAjax
        void CallUserList(string WhichCall)
        {
            stdate = Convert.ToDateTime(dtfrom.Value).ToString("yyyy-MM-dd");
            endDate = Convert.ToDateTime(dtto.Value).ToString("yyyy-MM-dd");

            #region For ISIN and Settlement after pageload get Date Range and list Specific ISIN and Settlement items
            string pagefromDate = null;
            string pagetoDate = null;
            string clientTypeValue = null;
            string clientHiddenValue = null;
            string isinHiddenValue = null;
            if (WhichCall != null && WhichCall.Contains("@"))
            {
                string[] splitDates = WhichCall.Split('@');
                pagefromDate = splitDates[1].ToString();
                pagetoDate = splitDates[2].ToString();
                clientTypeValue = splitDates[3].ToString();
                clientHiddenValue = splitDates[4].ToString();
                isinHiddenValue = splitDates[5].ToString();
            }
            #endregion

            string CombinedISINQuery = string.Empty;
            CombinedGroupByQuery = CombinedISINQuery;
            if (WhichCall != null && WhichCall.Contains("CallAjax-ISIN"))
            {
                #region ISIN CallAjaxList
                string strQueryISIN_Table = " Master_NSDLISIN ";
                string strQueryISIN_FieldName = " distinct top 10 (ltrim(rtrim(NSDLISIN_CompanyName)) + '   [ ' + convert(varchar,[NSDLISIN_Number]) + ' ]')  as ISINName,NSDLISIN_Number as ISINID ";
                string strQueryISIN_WhereClause = " NSDLISIN_CompanyName Like '%RequestLetter%' or NSDLISIN_Number Like 'RequestLetter%' ";
                string strQueryISIN_GroupBy = "";
                string strQueryISIN_OrderBy = " ISINName";
                CombinedISINQuery = strQueryISIN_Table + "$" + strQueryISIN_FieldName + "$" + strQueryISIN_WhereClause + "$" + strQueryISIN_GroupBy + "$" + strQueryISIN_OrderBy;
                CombinedISINQuery = CombinedISINQuery.Replace("'", "\\'");
                CombinedGroupByQuery = CombinedISINQuery;
                txtISIN.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + CombinedISINQuery + "')");
                # endregion
            }
            if (WhichCall != null && WhichCall.Contains("CallAjax-Settlement"))
            {
                #region Settlement CallAjaxList
                string strQuerySettlement_Table = " Master_NsdlCalendar ";
                string strQuerySettlement_FieldName = " distinct top 10 NsdlCalendar_SettlementNumber as SettlementName, NsdlCalendar_SettlementNumber  As SettlementID ";
                string strQuerySettlement_WhereClause = " NsdlCalendar_SettlementNumber Like '%RequestLetter%' ";
                string strQuerySettlement_GroupBy = "";
                string strQuerySettlement_OrderBy = " SettlementName";
                string CombinedSettlementQuery = strQuerySettlement_Table + "$" + strQuerySettlement_FieldName + "$" + strQuerySettlement_WhereClause + "$" + strQuerySettlement_GroupBy + "$" + strQuerySettlement_OrderBy;
                CombinedSettlementQuery = CombinedSettlementQuery.Replace("'", "\\'");
                CombinedGroupByQuery = CombinedSettlementQuery;
                txtSettlement.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + CombinedSettlementQuery + "')");
                # endregion
            }
            if (WhichCall == "CallAjax-Client")
            {
                #region Client CallAjaxList
                string strQueryClient_Table = @"(Select  distinct NsdlClients_BenFirstHolderName,NsdlClients_BenAccountID from Master_NsdlClients,Trans_NsdlTransaction where NsdlClients_BenAccountID = NsdlTransaction_BenAccountNumber) as DD ";
                string strQueryClient_FieldName = @" top 10 isNull(ltrim(rtrim(NsdlClients_BenFirstHolderName)),'')+' ['+cast(NsdlClients_BenAccountID as Varchar(8))+']' as ClientName,'Clients@'+ltrim(rtrim(NsdlClients_BenFirstHolderName))+' ['+cast(NsdlClients_BenAccountID as Varchar(8))+']@'+cast(NsdlClients_BenAccountID as Varchar(8)) as ClientID  ";
                string strQueryClient_WhereClause = @" NsdlClients_BenAccountID Like '%RequestLetter%' or NsdlClients_BenFirstHolderName Like 'RequestLetter%'";
                string strQueryClient_GroupBy = "";
                string strQueryClient_OrderBy = " NsdlClients_BenFirstHolderName ";

                string CombinedClientQuery = strQueryClient_Table + "$" + strQueryClient_FieldName + "$" + strQueryClient_WhereClause + "$" + strQueryClient_GroupBy + "$" + strQueryClient_OrderBy;
                CombinedClientQuery = CombinedClientQuery.Replace("'", "\\'");
                # endregion

                CombinedGroupByQuery = CombinedClientQuery;
                txtSelection.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + CombinedGroupByQuery + "')");
            }
            if (WhichCall == "CallAjax-Branch")
            {
                #region Branch CallAjaxList
                string strQueryBranch_Table = @" ( Select Distinct * From (Select isnull(ltrim(rtrim(branch_description)),'')+' ['+isnull(ltrim(rtrim(branch_code)),'')+']' as BranchName,'Branch@'+isnull(ltrim(rtrim(branch_description)),'')+' ['+isnull(ltrim(rtrim(branch_code)),'')+']@'+cast(ltrim(rtrim(branch_id)) as Varchar(8)) as BranchID,branch_code from tbl_master_branch where branch_id in (Select NsdlClients_BranchID From  Master_NsdlClients,Trans_NsdlTransaction where NsdlClients_BenAccountID=NsdlTransaction_BenAccountNumber)) T1  ) T2 ";
                string strQueryBranch_FieldName = " Top 10 BranchName,BranchID ";
                string strQueryBranch_WhereClause = " BranchName like '%RequestLetter%' or branch_code like '%RequestLetter%'";
                string strQueryBranch_GroupBy = "";
                string strQueryBranch_OrderBy = " BranchName ";
                string CombinedBranchQuery = strQueryBranch_Table + "$" + strQueryBranch_FieldName + "$" + strQueryBranch_WhereClause + "$" + strQueryBranch_GroupBy + "$" + strQueryBranch_OrderBy;
                CombinedBranchQuery = CombinedBranchQuery.Replace("'", "\\'");
                # endregion

                CombinedGroupByQuery = CombinedBranchQuery;
            }
            if (WhichCall == "CallAjax-Group")
            {
                #region Group CallAjaxList
                string strQueryGroup_Table = " (Select Distinct (NsdlClients_DPID + cast(ltrim(rtrim(NsdlClients_BenAccountID)) AS VARCHAR(8))) as ClientID  From  Master_NsdlClients,Trans_NsdlTransaction where NsdlClients_BenAccountID = NsdlTransaction_BenAccountNumber) T1 inner join  (Select grp_contactId,Gpm_Description,gpm_code,gpm_id  from  tbl_master_groupMaster,tbl_trans_group  Where gpm_id=grp_groupMaster and gpm_Type='" + ddlGroupType.SelectedValue + "') T2 on ClientID=grp_contactId ";
                string strQueryGroup_FieldName = " Distinct isNull(LTRIM(RTRIM(Gpm_Description)),'')+'['+isNull(LTRIM(RTRIM(gpm_code)),'')+']' GroupName,'Group@'+isnull(LTRIM(RTRIM(Gpm_Description)),'')+'['+isNull(LTRIM(RTRIM(gpm_code)),'')+']@'+cast(gpm_id as varchar(8)) as GroupID ";
                string strQueryGroup_WhereClause = " Gpm_Description Like '%RequestLetter%' or gpm_code like '%RequestLetter%' ";
                string strQueryGroup_GroupBy = "";
                string strQueryGroup_OrderBy = "";
                string CombinedGroupQuery = strQueryGroup_Table + "$" + strQueryGroup_FieldName + "$" + strQueryGroup_WhereClause + "$" + strQueryGroup_GroupBy + "$" + strQueryGroup_OrderBy;
                CombinedGroupQuery = CombinedGroupQuery.Replace("'", "\\'");
                # endregion

                CombinedGroupByQuery = CombinedGroupQuery;
            }
            if (WhichCall != null && WhichCall.Contains("CallAjax-FilteredClient"))
            {
                //===Modified For Transactions Merge With Holding and avoid diff SP Call=================
                if (cmpid != "")
                {
                    string WhereSql = "Where NsdlClients_branchid in (" + HttpContext.Current.Session["userbranchHierarchy"].ToString() + ")";
                    string OrderBySql = " order by FilteredName";
                    if (cmpid != "na")
                        WhereSql = WhereSql + " And NsdlClients_BenAccountID in (" + cmpid + ")";
                    else
                        WhereSql = WhereSql + " And NsdlClients_BenAccountID in (Select * From #TmpTransClients)";
                    if (CmbClientType.Value.ToString() != "All")
                    {
                        if (CmbClientType.Value.ToString() == "other")
                        {
                            WhereSql = WhereSql + " and NsdlClients_BenType NOT IN (01,05,06,04)";
                        }
                        else
                        {
                            WhereSql = WhereSql + " and NsdlClients_BenType=" + CmbClientType.Value;
                        }
                    }
                    WhereSql = WhereSql + " And (NsdlClients_BenFirstHolderName like 'RequestLetter%' or NsdlClients_BenAccountID like 'RequestLetter%')";

                    CombinedGroupByQuery = @" Declare @openHoldingDate varchar(15),@closeHoldingDate varchar(15)
		                                    
                                            Select  @openHoldingDate=Convert(varchar(10),Max(NsdlHolding_HoldingDateTime),110) 
                                            from Trans_NsdlHolding
                                            where NsdlHolding_HoldingDateTime < '" + Convert.ToDateTime(pagefromDate).ToString("yyyy-MM-dd") +

                                            @"' Select @closeHoldingDate=Convert(varchar(10),Max(NsdlHolding_HoldingDateTime),110) 
                                            from Trans_NsdlHolding 
                                            where NsdlHolding_HoldingDateTime <= '" + Convert.ToDateTime(pagetoDate).ToString("yyyy-MM-dd") +
                                            @"'        and NsdlHolding_HoldingDateTime >= '" + Convert.ToDateTime(pagefromDate).ToString("yyyy-MM-dd") +

                                            @"' IF OBJECT_ID('tempdb..#TmpTransClients') is not null
	                                            Drop Table #TmpTransClients
                                            
                                            Create Table #TmpTransClients(BenAccNumber int)

                                            Insert into #TmpTransClients
                                            select distinct(NsdlHolding_BenAccountNumber) 
                                            From Master_NsdlClients
                                            Inner Join
                                            Trans_NsdlHolding
                                            On NsdlClients_BenAccountID=NsdlHolding_BenAccountNumber
                                            where NsdlHolding_HoldingDateTime=@openHoldingDate
                                            Union
                                            select distinct(NsdlTransaction_BenAccountNumber) 
                                            from Master_NsdlClients	
                                            Inner Join
                                            Trans_NsdlTransaction
                                            On NsdlClients_BenAccountID=NsdlTransaction_BenAccountNumber 
                                            where NsdlTransaction_Date between '" + Convert.ToDateTime(pagefromDate).ToString("yyyy-MM-dd") +
                                            @"'          and '" + Convert.ToDateTime(pagetoDate).ToString("yyyy-MM-dd") +
                                            @"'  Union 
                                            select distinct(NsdlHolding_BenAccountNumber) 
                                            From Master_NsdlClients
                                            Inner Join
                                            Trans_NsdlHolding
                                            On NsdlClients_BenAccountID=NsdlHolding_BenAccountNumber 	
                                            where NsdlHolding_HoldingDateTime=@closeHoldingDate

                                            Select Top 10 ltrim(rtrim(NsdlClients_BenFirstHolderName))+' ['+cast(NsdlClients_BenAccountID as varchar(8))+']' as FilteredName
			                                            ,NsdlClients_BenAccountID FilteredID
                                            From Master_NsdlClients "
                                                + WhereSql + OrderBySql +
                                            @" Drop Table #TmpTransClients";

                    txtFilteredClient.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxListQuery',event,'" + CombinedGroupByQuery + "')");
                }
            }
        }
        #endregion

        #region Page Properties
        string ToDate, FromDate, Company, BenAccNumbers, ISIN, Settlement, Ben_Type;
        int ISINIndex, ISINSize;

        public string P_ToDate
        {
            get { return ToDate; }
            set { ToDate = value; }
        }
        public string P_FromDate
        {
            get { return FromDate; }
            set { FromDate = value; }
        }
        public string P_BenAccNumbers
        {
            get { return BenAccNumbers; }
            set { BenAccNumbers = value; }
        }
        public string P_ISIN
        {
            get { return ISIN; }
            set { ISIN = value; }
        }
        public string P_Settlement
        {
            get { return Settlement; }
            set { Settlement = value; }
        }
        public string P_Ben_Type
        {
            get { return Ben_Type; }
            set { Ben_Type = value; }
        }
        public int P_ISINIndex
        {
            get { return ISINIndex; }
            set { ISINIndex = value; }
        }
        public int P_ISINSize
        {
            get { return ISINSize; }
            set { ISINSize = value; }
        }
        #endregion

        #region Set Properties
        protected void SetPropertiesValue()
        {
            ToDate = Convert.ToDateTime(dtto.Value).ToString("yyyy-MM-dd");
            FromDate = Convert.ToDateTime(dtfrom.Value).ToString("yyyy-MM-dd");
            Company = Session["LastCompany"].ToString();
            if (cmpid != "")
                BenAccNumbers = cmpid;
            else
                BenAccNumbers = "na";

            if (isinid != "")
                ISIN = isinid;
            else
                ISIN = "na";

            if (SettlementID != "")
                Settlement = SettlementID;
            else
                Settlement = "na";

            if (CmbClientType.Value.ToString() == "All")
                Ben_Type = "na";
            else
                Ben_Type = CmbClientType.Value.ToString();

            if (ddlGenerate.SelectedValue.ToString() == "S")
                ISINIndex = (int)ViewState["startISINIndex"];
            else
                ISINIndex = 0;

            ISINSize = 15;
        }
        #endregion

        #region User Defined Method
        public void BindGroupType()
        {
            ddlGroupType.Items.Clear();
            DataTable DtGroup = oDBEngine.GetDataTable(" tbl_master_groupmaster ", " distinct gpm_Type ", null);
            if (DtGroup.Rows.Count > 0)
            {
                ddlGroupType.DataSource = DtGroup;
                ddlGroupType.DataTextField = "gpm_Type";
                ddlGroupType.DataValueField = "gpm_Type";
                ddlGroupType.DataBind();
                ddlGroupType.Items.Insert(0, new ListItem("Select GroupType", "0"));
                DtGroup.Dispose();
            }
            else
            {
                ddlGroupType.Items.Insert(0, new ListItem("Select GroupType", "0"));
            }
        }

        string GetClientIdsByBranchID(string branchId)
        {
            DataTable DtClientSBySelBranch = null;
            if (branchId == "All")
            {
                DtClientSBySelBranch = oDBEngine.GetDataTable(" Master_NsdlClients,Trans_NsdlTransaction ", " NsdlClients_BenAccountID ", " NsdlClients_BenAccountID = NsdlTransaction_BenAccountNumber ");
            }
            else
            {
                DtClientSBySelBranch = oDBEngine.GetDataTable(" Master_NsdlClients ", " NsdlClients_BenAccountID ", " NsdlClients_BranchID in (" + branchId + ")");
            }
            string clientList = "";
            if (DtClientSBySelBranch.Rows.Count > 0)
            {
                int size = DtClientSBySelBranch.Rows.Count;
                for (int i = 0; i < size - 1; i++)
                {
                    clientList += DtClientSBySelBranch.Rows[i][0].ToString() + ",";
                }
                clientList += DtClientSBySelBranch.Rows[size - 1][0].ToString();
            }
            return clientList;
        }

        string GetClientIdsByGroupIDType(string groupId, string groupType)
        {
            DataTable DtClientSBySelGroup = null;
            if (groupId == "All" || groupId == null || groupId == string.Empty)
                DtClientSBySelGroup = oDBEngine.GetDataTable(" Master_NsdlClients,Trans_NsdlTransaction ", " Distinct NsdlClients_BenAccountID ", " (NsdlTransaction_Date Between '" + Convert.ToDateTime(dtfrom.Value).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(dtto.Value).ToString("yyyy-MM-dd") + "')	and (NsdlClients_BenAccountID=NsdlTransaction_BenAccountNumber)	and (NsdlClients_DPID + cast(ltrim(rtrim(NsdlClients_BenAccountID)) AS VARCHAR(8))) in (Select grp_contactId from tbl_trans_group where grp_groupType='" + groupType + "') ");
            else
                DtClientSBySelGroup = oDBEngine.GetDataTable(" Master_NsdlClients ", " NsdlClients_BenAccountID ", " (NsdlClients_DPID + cast(ltrim(rtrim(NsdlClients_BenAccountID)) AS VARCHAR(8))) in (Select grp_contactId from tbl_trans_group where grp_groupMaster in (" + groupId + ") and grp_groupType='" + groupType + "')");

            string clientList = "";
            if (DtClientSBySelGroup.Rows.Count > 0)
            {
                int size = DtClientSBySelGroup.Rows.Count;
                for (int i = 0; i < size - 1; i++)
                {
                    clientList += DtClientSBySelGroup.Rows[i][0].ToString() + ",";
                }
                clientList += DtClientSBySelGroup.Rows[size - 1][0].ToString();
            }
            return clientList;
        }

        void BindClientsOnMailExpPdfGenerate()
        {
            if ((rbGroupBy.SelectedIndex == 0) && (rbUser.SelectedIndex == 0))//for client all
            {
                cmpid = "na";
            }
            else if ((rbGroupBy.SelectedIndex == 1) && (rbBranch.SelectedIndex == 0))//for Branch all
            {
                cmpid = "na";
            }
            else if ((rbGroupBy.SelectedIndex == 0) && (rbUser.SelectedIndex == 1))
            {
                cmpid = HiddenField_Client.Value;
            }
            else if ((rbGroupBy.SelectedIndex == 1) && (rbBranch.SelectedIndex == 1))
            {
                string branches = null;
                if (HiddenField_Branch.Value != null)
                    branches = HiddenField_Branch.Value;
                cmpid = GetClientIdsByBranchID(branches);
            }
            else if (rbGroupBy.SelectedIndex == 2)
            {
                string selGroupType = null;
                if (ddlGroupType.SelectedValue != "0")
                {
                    selGroupType = ddlGroupType.SelectedValue;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorOngroupType", "ErrorOngroupType();", true);
                }
                if (rbGroup.SelectedIndex == 0)
                {
                    cmpid = GetClientIdsByGroupIDType("All", selGroupType);
                }
                else if (rbGroup.SelectedIndex == 1)
                {
                    string groupes = null;
                    if (HiddenField_Group.Value != null)
                        groupes = HiddenField_Group.Value;
                    cmpid = GetClientIdsByGroupIDType(groupes, selGroupType);
                }
            }
        }

        void BindClientsOnScreenGenerate()
        {
            string defaultBranchID = null;
            string defaultGroupID = null;
            string defaultGroupType = null;
            if (rbGroupBy.SelectedIndex == 1)   // By branch
            {
                if (rbBranch.SelectedIndex == 0)
                {
                    if (ddlSelectedBranch.Items.Count > 0)
                    {
                        defaultBranchID = ddlSelectedBranch.Items[0].Value;
                    }
                }
                else if (rbBranch.SelectedIndex == 1)
                {
                    string branches = null;
                    if (HiddenField_Branch.Value != null)
                        branches = HiddenField_Branch.Value;
                    if (branches.Contains(","))
                    {
                        string[] branchList = branches.Split(',');
                        defaultBranchID = branchList[0];
                    }
                    else
                    {
                        defaultBranchID = branches;
                    }
                }
                cmpid = GetClientIdsByBranchID(defaultBranchID);
            }
            else if (rbGroupBy.SelectedIndex == 2)    // By Group
            {
                if (ddlGroupType.SelectedValue != "0")
                {
                    defaultGroupType = ddlGroupType.SelectedValue;
                }
                if (rbGroup.SelectedIndex == 0)
                {
                    if (ddlSelectedGroup.Items.Count > 0)
                    {
                        defaultGroupID = ddlSelectedGroup.Items[0].Value;
                    }
                }
                else if (rbGroup.SelectedIndex == 1)
                {
                    string groupes = null;
                    if (HiddenField_Group.Value != null)
                        groupes = HiddenField_Group.Value;
                    if (groupes.Contains(","))
                    {
                        string[] groupList = groupes.Split(',');
                        defaultGroupID = groupList[0];
                    }
                    else
                    {
                        defaultGroupID = HiddenField_Group.Value;
                    }
                }
                cmpid = GetClientIdsByGroupIDType(defaultGroupID, defaultGroupType);
            }
            else if (rbGroupBy.SelectedIndex == 0)
            {
                if (rbUser.SelectedIndex == 0)
                {
                    cmpid = "na";
                }
                else if (rbUser.SelectedIndex == 1)
                {
                    if (HiddenField_Client.Value != string.Empty)
                        cmpid = HiddenField_Client.Value;
                }
            }
        }

        void BindClientsOnGenerate()
        {
            if (ddlGenerate.SelectedValue == "S")
            {
                BindClientsOnScreenGenerate();
            }
            else if ((ddlGenerate.SelectedValue == "E") || (ddlGenerate.SelectedValue == "P"))
            {
                BindClientsOnMailExpPdfGenerate();
            }
            else if (ddlGenerate.SelectedValue == "M")
            {
                BindClientsOnMailExpPdfGenerate();
            }
        }

        void bindDetails()
        {
            string select, where;
            stdate = Convert.ToDateTime(dtfrom.Value).ToString("yyyy-MM-dd");
            endDate = Convert.ToDateTime(dtto.Value).ToString("yyyy-MM-dd");
            DT.Clear();
            DT.Dispose();
            totolRecord = 0;
            pageindex = 0;

            //===Modified For Transactions Merge With Holding and avoid diff SP Call=================
            if (cmpid != "")
            {
                string WhereSql = "Where NsdlClients_branchid in (" + HttpContext.Current.Session["userbranchHierarchy"].ToString() + ")";
                string OrderBySql = " order by names";
                if (cmpid != "na")
                    WhereSql = WhereSql + " And NsdlClients_BenAccountID in (" + cmpid + ")";
                else
                    WhereSql = WhereSql + " And NsdlClients_BenAccountID in (Select * From #TmpTransClients)";
                if (CmbClientType.Value.ToString() != "All")
                {
                    if (CmbClientType.Value.ToString() == "other")
                    {
                        WhereSql = WhereSql + " and NsdlClients_BenType NOT IN (01,05,06,04)";
                    }
                    else
                    {
                        WhereSql = WhereSql + " and NsdlClients_BenType=" + CmbClientType.Value;
                    }
                }
                DT = oDBEngine.GetDataTable(@"  Declare @openHoldingDate varchar(15),@closeHoldingDate varchar(15)
		                                    
                                            Select  @openHoldingDate=Convert(varchar(10),Max(NsdlHolding_HoldingDateTime),110) 
                                            from Trans_NsdlHolding
                                            where NsdlHolding_HoldingDateTime < '" + Convert.ToDateTime(dtfrom.Value).ToString("yyyy-MM-dd") +

                                            @"' Select @closeHoldingDate=Convert(varchar(10),Max(NsdlHolding_HoldingDateTime),110) 
                                            from Trans_NsdlHolding 
                                            where NsdlHolding_HoldingDateTime <= '" + Convert.ToDateTime(dtto.Value).ToString("yyyy-MM-dd") +
                                            @"'        and NsdlHolding_HoldingDateTime >= '" + Convert.ToDateTime(dtfrom.Value).ToString("yyyy-MM-dd") +

                                            @"' IF OBJECT_ID('tempdb..#TmpTransClients') is not null
	                                            Drop Table #TmpTransClients
                                            Create Table #TmpTransClients(BenAccNumber int)

                                            Insert into #TmpTransClients
                                            select distinct(NsdlHolding_BenAccountNumber) 
                                            From Master_NsdlClients
                                            Inner Join
                                            Trans_NsdlHolding
                                            On NsdlClients_BenAccountID=NsdlHolding_BenAccountNumber
                                            where NsdlHolding_HoldingDateTime=@openHoldingDate
                                            Union
                                            select distinct(NsdlTransaction_BenAccountNumber) 
                                            from Master_NsdlClients	
                                            Inner Join
                                            Trans_NsdlTransaction
                                            On NsdlClients_BenAccountID=NsdlTransaction_BenAccountNumber 
                                            where NsdlTransaction_Date between '" + Convert.ToDateTime(dtfrom.Value).ToString("yyyy-MM-dd") +
                                            @"'          and '" + Convert.ToDateTime(dtto.Value).ToString("yyyy-MM-dd") +
                                            @"'  Union 
                                            select distinct(NsdlHolding_BenAccountNumber) 
                                            From Master_NsdlClients
                                            Inner Join
                                            Trans_NsdlHolding
                                            On NsdlClients_BenAccountID=NsdlHolding_BenAccountNumber 	
                                            where NsdlHolding_HoldingDateTime=@closeHoldingDate

                                            Select distinct NsdlClients_BenAccountID, NsdlClients_BenType, dbo.fn_BenStatus(NsdlClients_BeneficiaryStatus)as NsdlClients_BeneficiaryStatus
                                                ,ltrim(rtrim(NsdlClients_BenFirstHolderName))+case when len(ltrim(Rtrim(NsdlClients_BenSecondHolderName)))=0 then '' else ', '+ltrim(rtrim(NsdlClients_BenSecondHolderName)) end
                                                +case when len(ltrim(Rtrim(NsdlClients_BenThirdHolderName)))=0 then '' else ', '+ltrim(rtrim(NsdlClients_BenThirdHolderName)) end as names
                                                ,Convert(varchar(9),getdate(),6)  as rptdate, Convert(varchar(9),cast('" + stdate + "' as datetime),6) as stdate,Convert(varchar(9),cast('" + endDate + "' as datetime),6) as enddate" +
                                            @" From Master_NsdlClients " + WhereSql + OrderBySql +
                                            @" Drop Table #TmpTransClients"
                                            );
            }
            else
            {
                norecord.Visible = true;
                list.Style["display"] = "none";
                tblpage.Style["display"] = "none";
                norecord.Style["display"] = "display";
            }
            if (DT.Rows.Count > 0)
            {
                totolRecord = DT.Rows.Count;//=====Total Clients Transaction Showing 
                if (ddlGenerate.SelectedValue == "E")
                {
                    showExcelReport();
                }
                else if (ddlGenerate.SelectedValue == "P")
                {
                    showCrystalReport();
                }
                else if (ddlGenerate.SelectedValue == "S")
                {
                    ShowScreenDetail();
                }
                //else if (ddlGenerate.SelectedValue == "M")
                //{
                //    SentMail();
                //}            
            }
            else
            {
                norecord.Visible = true;
                list.Style["display"] = "none";
                tblpage.Style["display"] = "none";
                norecord.Style["display"] = "display";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Reset32", "Reset();", true);
            }
        }

        protected DataSet Procedure(string BenAccNumber)
        {
            DataSet dsTransDetail;
            oGenericMethod = new GenericMethod();
            string[] strSpParam = new string[11];
            strSpParam[0] = "FromDate|" + GenericStoreProcedure.ParamDBType.Varchar + "|30|" + FromDate + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[1] = "ToDate|" + GenericStoreProcedure.ParamDBType.Varchar + "|30|" + ToDate + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[2] = "BenAccNo|" + GenericStoreProcedure.ParamDBType.Varchar + "|8|" + BenAccNumber + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[3] = "ISIN|" + GenericStoreProcedure.ParamDBType.Varchar + "|30|" + ISIN + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[4] = "SettlementID|" + GenericStoreProcedure.ParamDBType.Varchar + "|30|" + Settlement + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[5] = "BenType|" + GenericStoreProcedure.ParamDBType.Varchar + "|30|" + Ben_Type + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[6] = "CompanyID|" + GenericStoreProcedure.ParamDBType.Varchar + "|30|" + Company + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[7] = "BranchID|" + GenericStoreProcedure.ParamDBType.Varchar + "|-1|" + HttpContext.Current.Session["userbranchHierarchy"].ToString() + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[8] = "ReportType|" + GenericStoreProcedure.ParamDBType.Char + "|1|" + ddlGenerate.SelectedValue.ToString() + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[9] = "ISINIndex|" + GenericStoreProcedure.ParamDBType.Int + "|10|" + ISINIndex + "|" + GenericStoreProcedure.ParamType.ExParam;
            strSpParam[10] = "ISINSize|" + GenericStoreProcedure.ParamDBType.Int + "|10|" + ISINSize + "|" + GenericStoreProcedure.ParamType.ExParam;

            dsTransDetail = new DataSet();
            oGenericStoreProcedure = new GenericStoreProcedure();
            dsTransDetail = oGenericStoreProcedure.Procedure_DataSet(strSpParam, "Report_NSDLTransaction");

            return dsTransDetail;
        }

        protected DataSet GetClientsData()
        {
            //===Add Data In Trans dataTable Through Looping For Get Data One By One Clients===========
            DataSet DSClientData = new DataSet();
            DataTable DTClientTrans = new DataTable();
            if ((cmpid != null) || (cmpid != string.Empty))
            {
                if (cmpid == "na")
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        if (i == 0)
                            cmpid = DT.Rows[i][0].ToString();
                        else
                            cmpid = cmpid + "," + DT.Rows[i][0].ToString();
                    }
                }

                string[] cmpIds = cmpid.Split(',');
                for (int i = 0; i < cmpIds.Length; i++)
                {
                    DataTable dtTempByBenAcc = new DataTable();
                    DataSet dsTempByBenAcc = Procedure(cmpIds[i].ToString());
                    dtTempByBenAcc = dsTempByBenAcc.Tables[0];

                    if (dtTempByBenAcc.Rows.Count > 0)
                    {
                        if (DTClientTrans.Rows.Count > 0)
                        {
                            int colCount = dtTempByBenAcc.Columns.Count;
                            foreach (DataRow drTemp in dtTempByBenAcc.Rows)
                            {
                                DataRow rowTemp = DTClientTrans.NewRow();
                                for (int j = 0; j < colCount; j++)
                                {
                                    rowTemp[j] = drTemp[j];
                                }
                                DTClientTrans.Rows.Add(rowTemp);
                            }
                        }
                        else
                        {
                            DTClientTrans = dtTempByBenAcc.Copy();
                        }
                    }
                    dtTempByBenAcc.Clear();
                    dtTempByBenAcc.Dispose();
                }
                DTClientTrans.AcceptChanges();
            }
            DSClientData.Tables.Add(DTClientTrans);
            DSClientData.AcceptChanges();

            return DSClientData;
        }

        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "lavender";
        }

        protected string ZeroCheck(string s)
        {
            if ((s.IndexOf('.') == -1))     //whole number, no fractional part OR empty string
                return s;
            else
            {
                string[] arrS = s.Split('.');
                if (arrS[0] == "")
                    return "0" + s;
                else
                    return s;
            }
        }
        #endregion

        # region Pagination Next Previous Purpose
        protected void btnFirst(object sender, EventArgs e)
        {
            pageindex = 0;
            ViewState["startISINIndex"] = null;
            ShowScreenDetail();
        }

        protected void btnPrevious(object sender, EventArgs e)
        {
            if (pageindex > 0)
            {
                pageindex = pageindex - 1;
            }
            ViewState["startISINIndex"] = null;
            ShowScreenDetail();
        }

        protected void btnNext(object sender, EventArgs e)
        {
            if (pageindex < totolRecord)
            {
                pageindex = pageindex + 1;
            }
            ViewState["startISINIndex"] = null;
            ShowScreenDetail();
        }

        protected void btnLast(object sender, EventArgs e)
        {
            pageindex = totolRecord - 1;
            ViewState["startISINIndex"] = null;
            ShowScreenDetail();
        }

        protected void btnTransnNext_Click(object sender, EventArgs e)
        {
            ViewState["startISINIndex"] = ((int)ViewState["startISINIndex"] + (int)ViewState["ISINSize"]);
            if (((int)ViewState["TotalISIN"] - (int)ViewState["startISINIndex"]) <= (int)ViewState["ISINSize"])
            {
                btnTransnNext.Enabled = false;
                btnTransnNext1.Enabled = false;
            }
            else
            {
                btnTransnNext.Enabled = true;
                btnTransnNext1.Enabled = true;
            }
            btnTransPrevious.Enabled = true;
            btnTransPrevious1.Enabled = true;
            ShowScreenDetail();
        }

        protected void btnTransPrevious_Click(object sender, EventArgs e)
        {
            ViewState["startISINIndex"] = (int)ViewState["startISINIndex"] - (int)ViewState["ISINSize"];
            if ((int)ViewState["startISINIndex"] < 0)
            {
                ViewState["startISINIndex"] = 0;
            }
            if ((int)ViewState["startISINIndex"] == 0)
            {
                btnTransPrevious.Enabled = false;
                btnTransPrevious1.Enabled = false;
            }
            else
            {
                btnTransPrevious.Enabled = true;
                btnTransPrevious1.Enabled = true;
            }
            btnTransnNext.Enabled = true;
            btnTransnNext1.Enabled = true;
            ShowScreenDetail();
        }

        void pageing()
        {
            if (pageindex == 0)
            {
                ASPxFirst.Enabled = false;
                ASPxPrevious.Enabled = false;
                ASPxNext.Enabled = true;
                ASPxLast.Enabled = true;
            }
            if (pageindex == totolRecord - 1)
            {
                ASPxNext.Enabled = false;
                ASPxLast.Enabled = false;
                ASPxFirst.Enabled = true;
                ASPxPrevious.Enabled = true;
            }
            if (pageindex > 0 && pageindex < totolRecord - 1)
            {
                ASPxFirst.Enabled = true;
                ASPxPrevious.Enabled = true;
                ASPxNext.Enabled = true;
                ASPxLast.Enabled = true;
            }
            if (totolRecord == 1)
            {
                ASPxFirst.Enabled = false;
                ASPxPrevious.Enabled = false;
                ASPxNext.Enabled = false;
                ASPxLast.Enabled = false;
                tblpage.Style["display"] = "none";
            }
            else
            {
                tblpage.Style["display"] = "display";
            }
            listRecord.Text = pageindex + 1 + " of " + totolRecord + " Client's Transactions.";
        }
        # endregion

        # region Screen
        void FillAllBranches()
        {
            string[,] str = oDBEngine.GetFieldValue(" (Select isnull(ltrim(rtrim(branch_description)),'')+' ['+isnull(ltrim(rtrim(branch_code)),'')+']' as BranchName,ltrim(rtrim(branch_id)) as BranchID from tbl_master_branch where branch_id in (Select NsdlClients_BranchID From  Master_NsdlClients,Trans_NsdlTransaction where NsdlClients_BenAccountID=NsdlTransaction_BenAccountNumber)) T1  ", " Distinct BranchID, BranchName ", null, 2, " BranchName ");
            clsdrp.AddDataToDropDownList(str, ddlSelectedBranch);
        }

        void FillAllGroups(string groupType, string dateFrom, string dateTo, string isinForAllGroupDDLFill, string settlementForAllGroupDDLFill)
        {
            if (groupType != "0")
            {
                string strQueryWhere = " where  NsdlClients_BenAccountID = NsdlTransaction_BenAccountNumber ";
                string[,] str = oDBEngine.GetFieldValue(" (Select Distinct GroupID,GroupName From  (Select  Distinct  (NsdlClients_DPID  +  cast(ltrim(rtrim(NsdlClients_BenAccountID))  AS  VARCHAR(8)))  as  ClientID	 From  Master_NsdlClients , Trans_NsdlTransaction   " + strQueryWhere + ")  T1   inner join    (Select grp_contactId , isnull(LTRIM(RTRIM(Gpm_Description)),'')+'['+isnull(LTRIM(RTRIM(gpm_code)),'')+']'  GroupName , gpm_id as GroupID  from  tbl_master_groupMaster , tbl_trans_group   Where  gpm_id=grp_groupMaster  and  gpm_Type='" + groupType + "')  T2  on  ClientID=grp_contactId  ) T3 ", " GroupID,GroupName ", null, 2, null);
                clsdrp.AddDataToDropDownList(str, ddlSelectedGroup);
            }
        }

        void FillDdlSelectedBranch(string selectedBranchList)
        {
            DataTable dtSelBranch = null;
            if (ddlSelectedBranch.Items.Count > 0) ddlSelectedBranch.Items.Clear();
            if (selectedBranchList != null) dtSelBranch = oDBEngine.GetDataTable(" tbl_master_branch ", " isnull(LTRIM(RTRIM(branch_description)),'')+' ['+isnull(LTRIM(RTRIM(branch_code)),'')+']' As BranchName,ltrim(rtrim(branch_id)) as BranchID ", " branch_id in (" + selectedBranchList + ")");
            if (dtSelBranch.Rows.Count > 0)
            {
                ddlSelectedBranch.DataSource = dtSelBranch;
                ddlSelectedBranch.DataTextField = "BranchName";
                ddlSelectedBranch.DataValueField = "BranchID";
                ddlSelectedBranch.DataBind();
                dtSelBranch.Dispose();
            }
        }

        void FillDdlSelectedGroup(string selectedGroupList)
        {
            DataTable dtSelGroup = null;
            if (ddlSelectedGroup.Items.Count > 0) ddlSelectedGroup.Items.Clear();
            //========for group by query
            if (selectedGroupList != null) dtSelGroup = oDBEngine.GetDataTable(" tbl_master_groupMaster ", " isnull(ltrim(rtrim(gpm_Description)),'')+' ['+isnull(ltrim(rtrim(gpm_code)),'')+']' as GroupName,ltrim(rtrim(gpm_id)) as GroupID ", "gpm_Type='" + ddlGroupType.SelectedValue + "' and gpm_id in (" + selectedGroupList + ")");
            if (dtSelGroup.Rows.Count > 0)
            {
                ddlSelectedGroup.DataSource = dtSelGroup;
                ddlSelectedGroup.DataTextField = "GroupName";
                ddlSelectedGroup.DataValueField = "GroupID";
                ddlSelectedGroup.DataBind();
                dtSelGroup.Dispose();
            }
        }

        protected void ddlSelectedBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            string branchId = ddlSelectedBranch.SelectedValue;
            cmpid = GetClientIdsByBranchID(branchId);
            if (ddlGenerate.SelectedValue == "S")
            {
                list.Style["display"] = "display";
                tblpage.Style["display"] = "display";
                bindDetails();
                HiddenField_Client.Value = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Reset", "Reset();", true);
            }
        }

        protected void ddlSelectedGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            string groupId = ddlSelectedGroup.SelectedValue;
            string groupType = ddlGroupType.SelectedValue;
            cmpid = GetClientIdsByGroupIDType(groupId, groupType);
            if (ddlGenerate.SelectedValue == "S")
            {
                list.Style["display"] = "display";
                tblpage.Style["display"] = "display";
                bindDetails();
                HiddenField_Client.Value = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Reset", "Reset();", true);
            }
        }

        protected void btnShowGrid_Click(object sender, EventArgs e)
        {
            BindClientsOnGenerate();
            list.Style["display"] = "display";
            tblpage.Style["display"] = "display";
            bindDetails();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Reset", "Reset();", true);
        }

        public void ShowScreenDetail()
        {
            DataSet dsClientTrans;
            DataTable DTClientTrans;
            DataTable dtScreen;

            if ((cmpid != null) || (cmpid != string.Empty))
            {
                if (cmpid == "na")
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        if (i == 0)
                            cmpid = DT.Rows[i][0].ToString();
                        else
                            cmpid = cmpid + "," + DT.Rows[i][0].ToString();
                    }
                }
            }

            string[] cmpIds = cmpid.Split(',');
            cmpIds = cmpid.Split(',');
            totolRecord = cmpIds.Length;

            if (ViewState["startISINIndex"] == null)
            {
                ViewState["startISINIndex"] = 0;
                ViewState["ISINSize"] = 15;
                ViewState["TotalISIN"] = 0;
            }

            SetPropertiesValue();

            dsClientTrans = new DataSet();
            dsClientTrans = Procedure(cmpIds[pageindex].ToString());

            if (dsClientTrans.Tables.Count > 1)
            {
                DTClientTrans = new DataTable();
                DTClientTrans = dsClientTrans.Tables[0];
                ViewState["TotalISIN"] = Convert.ToInt32(dsClientTrans.Tables[1].Rows[0][0].ToString());

                if ((int)ViewState["startISINIndex"] == 0)// && (pageindex == 0) 
                {
                    if ((int)ViewState["TotalISIN"] <= (int)ViewState["ISINSize"])
                    {
                        btnTransnNext.Visible = false;
                        btnTransnNext1.Visible = false;
                        btnTransPrevious.Visible = false;
                        btnTransPrevious1.Visible = false;
                    }
                    else
                    {
                        btnTransnNext.Visible = true;
                        btnTransnNext1.Visible = true;
                        btnTransnNext.Enabled = true;
                        btnTransnNext1.Enabled = true;
                        btnTransPrevious.Visible = true;
                        btnTransPrevious1.Visible = true;
                        btnTransPrevious.Enabled = false;
                        btnTransPrevious1.Enabled = false;
                    }
                }
                dtScreen = new DataTable();
                if (DTClientTrans.Rows.Count > 0)
                {
                    dtScreen = DTClientTrans.Copy();
                    int transCount = 0;
                    for (int k = 0; k < dtScreen.Rows.Count; k++)
                    {
                        if (k > 0)
                        {
                            if (dtScreen.Rows[k - 1]["BenAccountID"].ToString() != dtScreen.Rows[k]["BenAccountID"].ToString()
                                || dtScreen.Rows[k - 1]["ISINNumber"].ToString() != dtScreen.Rows[k]["ISINNumber"].ToString()
                                || dtScreen.Rows[k - 1]["SettlementNumber"].ToString() != dtScreen.Rows[k]["SettlementNumber"].ToString()
                                || dtScreen.Rows[k - 1]["AccountType"].ToString() != dtScreen.Rows[k]["AccountType"].ToString())
                            {
                                transCount = 0;
                            }

                            if (dtScreen.Rows[k - 1]["BenAccountID"].ToString() == dtScreen.Rows[k]["BenAccountID"].ToString()
                                && dtScreen.Rows[k - 1]["ISINNumber"].ToString() == dtScreen.Rows[k]["ISINNumber"].ToString()
                                && dtScreen.Rows[k - 1]["SettlementNumber"].ToString() == dtScreen.Rows[k]["SettlementNumber"].ToString()
                                && dtScreen.Rows[k - 1]["AccountType"].ToString() == dtScreen.Rows[k]["AccountType"].ToString())
                            {
                                if ((dtScreen.Rows[k]["Ref. No."].ToString() == "Test")
                                    || (dtScreen.Rows[k]["Ref. No."].ToString() == "Opening Balance")
                                    || (dtScreen.Rows[k]["Ref. No."].ToString() == "Closing Balance"))
                                {
                                    dtScreen.Rows[k]["Current Balance"] = dtScreen.Rows[k]["Current Balance"].ToString();
                                }
                                else
                                {
                                    if ((dtScreen.Rows[k]["Ref. No."].ToString() == "")
                                        && (dtScreen.Rows[k]["Particulars"].ToString() == "No Transaction Recorded For This ISIN Of This Period"))
                                    {
                                        dtScreen.Rows[0]["Current Balance"] = "0.000";
                                    }
                                    else
                                    {
                                        if (transCount > 0)
                                        {
                                            dtScreen.Rows[k]["Current Balance"] = Convert.ToString(Convert.ToDecimal(dtScreen.Rows[k - 1]["Current Balance"].ToString()) + Convert.ToDecimal(dtScreen.Rows[k]["credit"].ToString()) - Convert.ToDecimal(dtScreen.Rows[k]["debit"].ToString()));

                                        }
                                        else
                                        {
                                            dtScreen.Rows[k]["Current Balance"] = Convert.ToString(Convert.ToDecimal(dtScreen.Rows[k]["openingbalance"].ToString()) + Convert.ToDecimal(dtScreen.Rows[k]["credit"].ToString()) - Convert.ToDecimal(dtScreen.Rows[k]["debit"].ToString()));
                                            transCount = transCount + 1;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    for (int m = 0; m < dtScreen.Rows.Count; m++)
                    {
                        if (dtScreen.Rows[m]["Ref. No."].ToString() == "Test")
                        {
                            dtScreen.Rows[m]["Credit"] = "";
                            dtScreen.Rows[m]["Debit"] = "";
                            dtScreen.Rows[m]["Current Balance"] = "";
                        }
                        if (dtScreen.Rows[m]["Credit"].ToString() == "0.000") dtScreen.Rows[m]["Credit"] = "";
                        if (dtScreen.Rows[m]["Debit"].ToString() == "0.000") dtScreen.Rows[m]["Debit"] = "";
                        if ((dtScreen.Rows[m]["Ref. No."].ToString() == "")
                            && (dtScreen.Rows[m]["Particulars"].ToString() == "No Transaction Recorded For This ISIN Of This Period"))
                        {
                            dtScreen.Rows[m]["Date"] = "";
                            dtScreen.Rows[m]["Current Balance"] = "";
                        }
                    }
                    dtScreen.Columns.Remove("SettlementNumber");
                    dtScreen.Columns.Remove("ISINNumber");
                    dtScreen.Columns.Remove("OpeningBalance");
                    dtScreen.Columns.Remove("BenAccountID");
                    dtScreen.AcceptChanges();

                    if ((int)ViewState["startISINIndex"] == 0)
                    {
                        string strClientDet = dtScreen.Rows[0][0].ToString();
                        if (strClientDet.Contains("||"))
                        {
                            strClientDet = strClientDet.Replace(".", "");
                            strClientDet = strClientDet.Replace("||", "|");

                            string[] strClientDetails = strClientDet.Split('|');
                            lblClientId.Text = strClientDetails[0].ToString();
                            category.Text = strClientDetails[1].ToString();
                            status.Text = strClientDetails[2].ToString();
                            holders.Text = strClientDetails[3].ToString();
                        }
                    }
                }

                string scrHTML = "<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" class=\"gridcellleft\">";
                if (dtScreen.Rows.Count > 0)
                {
                    if ((int)ViewState["startISINIndex"] == 0)
                    {
                        dtScreen.Rows.Remove(dtScreen.Rows[0]);//==Remove ClientDetails Row
                        ViewState["startISINIndex"] = 1;
                    }
                    dtScreen.AcceptChanges();

                    int startIndex, endIndex;
                    startIndex = (int)ViewState["startISINIndex"];
                    endIndex = startIndex + (int)ViewState["ISINSize"] - 1;
                    if (endIndex >= (int)ViewState["TotalISIN"])
                    {
                        endIndex = (int)ViewState["TotalISIN"];
                    }

                    lblTransction.Text = "(" + Convert.ToString(startIndex) + " - " + Convert.ToString(endIndex) + ")";
                    lblTotalTransction.Text = "Total " + dsClientTrans.Tables[1].Rows[0][0].ToString();

                    lblTransction1.Text = lblTransction.Text;
                    lblTotalTransction1.Text = lblTotalTransction.Text;

                    for (int j = 0; j < dtScreen.Rows.Count; j++)
                    {
                        if (dtScreen.Rows[j][0].ToString().Contains("ISIN:"))
                        {
                            string strIsinDetail = dtScreen.Rows[j][0].ToString();
                            strIsinDetail = strIsinDetail.Replace(".", "");
                            strIsinDetail = strIsinDetail.Replace("||", "|");
                            string[] strIsinDetails = strIsinDetail.Split('|');
                            scrHTML += "<tr><td colspan=\"6\">&nbsp;</td></tr>";
                            scrHTML += "<tr style=\"background-color:#FFD8AA\">";
                            scrHTML += "<td colspan=\"2\"><b>" + strIsinDetails[0] + "</b></td><td><b>" + strIsinDetails[1] + "</b></td><td colspan=\"3\">" + strIsinDetails[2] + "</td>";
                            scrHTML += "</tr>";
                            scrHTML += "<tr style=\"background-color:#ccc\">";
                            scrHTML += "<td><b>Date</b></td><td><b>Ref. No.</b></td><td><b>Particulars</b></td><td align=\"right\"><b>Credit</b></td><td align=\"right\"><b>Debit</b></td><td align=\"right\"><b>Current Balance</b></td>";
                            scrHTML += "</tr>";
                        }
                        else
                        {
                            if ((dtScreen.Rows[j][1].ToString().Contains("Opening")) || (dtScreen.Rows[j][1].ToString().Contains("Closing")))
                                scrHTML += "<tr style=\"background-color:#eee\">";
                            else
                                scrHTML += "<tr>";
                            scrHTML += "<td>" + dtScreen.Rows[j][0].ToString() + "</td>";
                            if ((dtScreen.Rows[j][1].ToString().Contains("Opening")) || (dtScreen.Rows[j][1].ToString().Contains("Closing")))
                                scrHTML += "<td style=\"color:blue\"><b>" + dtScreen.Rows[j][1].ToString() + "</b></td>";
                            else
                                scrHTML += "<td>" + dtScreen.Rows[j][1].ToString() + "</td>";
                            scrHTML += "<td>" + dtScreen.Rows[j][2].ToString() + " " + (string)(dtScreen.Rows[j][3].ToString() == "NULL" ? "" : dtScreen.Rows[j][3].ToString()) + "</td>";
                            scrHTML += "<td align=\"right\">" + ZeroCheck(dtScreen.Rows[j][4].ToString()) + "</td>";
                            scrHTML += "<td align=\"right\">" + ZeroCheck(dtScreen.Rows[j][5].ToString()) + "</td>";
                            if ((dtScreen.Rows[j][1].ToString().Contains("Opening")) || (dtScreen.Rows[j][1].ToString().Contains("Closing")))
                                scrHTML += "<td style=\"color:blue\" align=\"right\"><b>" + ZeroCheck(dtScreen.Rows[j][6].ToString()) + "</b></td>";
                            else
                                scrHTML += "<td align=\"right\">" + ZeroCheck(dtScreen.Rows[j][6].ToString()) + "</td>";
                            scrHTML += "</tr>";
                        }
                    }

                    pageing();
                    norecord.Visible = false;
                    listRecord.Text = pageindex + 1 + " of " + totolRecord + " Clients.";
                }
                scrHTML += "</table>";
                display.InnerHtml = scrHTML;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "hi", "height();", true);

                dtScreen.Clear();
                dtScreen.Dispose();
                DTClientTrans.Clear();
                DTClientTrans.Dispose();
            }
            else
            {
                norecord.Visible = true;
                list.Style["display"] = "none";
                tblpage.Style["display"] = "none";
                norecord.Style["display"] = "display";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Reset32", "Reset();", true);
            }
            dsClientTrans.Clear();
            dsClientTrans.Dispose();
        }

        protected void lnkBtnTransByClient_Click(object sender, EventArgs e)
        {
            if ((cmpid != null) || (cmpid != string.Empty))
            {
                if (cmpid == "na")
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        if (i == 0)
                            cmpid = DT.Rows[i][0].ToString();
                        else
                            cmpid = cmpid + "," + DT.Rows[i][0].ToString();
                    }
                }
            }
            string[] cmpIds = cmpid.Split(',');
            for (int i = 0; i < cmpIds.Length; i++)
            {
                if (cmpIds[i].ToString() == txtFilteredClient_hidden.Text)
                {
                    pageindex = i;
                }
            }
            ViewState["startISINIndex"] = null;
            ShowScreenDetail();
        }
        # endregion Screen

        #region EXCEL
        protected void btnExcelExport_Click(object sender, EventArgs e)
        {
            BindClientsOnGenerate();
            if (ddlGenerate.SelectedValue == "E")
            {
                list.Style["display"] = "display";
                tblpage.Style["display"] = "display";
                bindDetails();
            }
        }

        public void showExcelReport()
        {
            DataSet transDs;
            DataTable dtEx;
            SetPropertiesValue();
            transDs = new DataSet();
            transDs = GetClientsData();
            if (transDs.Tables[0].Rows.Count > 0)
            {
                dtEx = new DataTable();
                dtEx = transDs.Tables[0].Copy();
                int transCount = 0;
                for (int k = 0; k < dtEx.Rows.Count; k++)
                {
                    if (k > 0)
                    {
                        if (dtEx.Rows[k - 1]["BenAccountID"].ToString() != dtEx.Rows[k]["BenAccountID"].ToString()
                            || dtEx.Rows[k - 1]["ISINNumber"].ToString() != dtEx.Rows[k]["ISINNumber"].ToString()
                            || dtEx.Rows[k - 1]["SettlementNumber"].ToString() != dtEx.Rows[k]["SettlementNumber"].ToString()
                            || dtEx.Rows[k - 1]["AccountType"].ToString() != dtEx.Rows[k]["AccountType"].ToString())
                        {
                            transCount = 0;
                        }

                        if (dtEx.Rows[k - 1]["BenAccountID"].ToString() == dtEx.Rows[k]["BenAccountID"].ToString()
                            && dtEx.Rows[k - 1]["ISINNumber"].ToString() == dtEx.Rows[k]["ISINNumber"].ToString()
                            && dtEx.Rows[k - 1]["SettlementNumber"].ToString() == dtEx.Rows[k]["SettlementNumber"].ToString()
                            && dtEx.Rows[k - 1]["AccountType"].ToString() == dtEx.Rows[k]["AccountType"].ToString())
                        {
                            if ((dtEx.Rows[k]["Ref. No."].ToString() == "Test")
                                || (dtEx.Rows[k]["Ref. No."].ToString() == "Opening Balance")
                                || (dtEx.Rows[k]["Ref. No."].ToString() == "Closing Balance"))
                            {
                                dtEx.Rows[k]["Current Balance"] = dtEx.Rows[k]["Current Balance"].ToString();
                            }
                            else
                            {
                                if ((dtEx.Rows[k]["Ref. No."].ToString() == "")
                                    && (dtEx.Rows[k]["Particulars"].ToString() == "No Transaction Recorded For This ISIN Of This Period"))
                                {
                                    dtEx.Rows[0]["Current Balance"] = "0.000";
                                }
                                else
                                {
                                    if (transCount > 0)
                                    {
                                        dtEx.Rows[k]["Current Balance"] = Convert.ToString(Convert.ToDecimal(dtEx.Rows[k - 1]["Current Balance"].ToString()) + Convert.ToDecimal(dtEx.Rows[k]["credit"].ToString()) - Convert.ToDecimal(dtEx.Rows[k]["debit"].ToString()));
                                    }
                                    else
                                    {
                                        dtEx.Rows[k]["Current Balance"] = Convert.ToString(Convert.ToDecimal(dtEx.Rows[k]["openingbalance"].ToString()) + Convert.ToDecimal(dtEx.Rows[k]["credit"].ToString()) - Convert.ToDecimal(dtEx.Rows[k]["debit"].ToString()));
                                        transCount = transCount + 1;
                                    }
                                }
                            }
                        }
                    }
                }

                for (int m = 0; m < dtEx.Rows.Count; m++)
                {
                    if (dtEx.Rows[m]["Ref. No."].ToString() == "Test")
                    {
                        dtEx.Rows[m]["Credit"] = "";
                        dtEx.Rows[m]["Debit"] = "";
                        dtEx.Rows[m]["Current Balance"] = "";
                    }
                    if (dtEx.Rows[m]["Credit"].ToString() == "0.000") dtEx.Rows[m]["Credit"] = "";
                    if (dtEx.Rows[m]["Debit"].ToString() == "0.000") dtEx.Rows[m]["Debit"] = "";
                    if ((dtEx.Rows[m]["Ref. No."].ToString() == "")
                        && (dtEx.Rows[m]["Particulars"].ToString() == "No Transaction Recorded For This ISIN Of This Period"))
                    {
                        dtEx.Rows[m]["Date"] = " ";
                        dtEx.Rows[m]["Current Balance"] = "";
                    }
                }
                dtEx.Columns.Remove("SettlementNumber");
                dtEx.Columns.Remove("ISINNumber");
                dtEx.Columns.Remove("AccountType");
                dtEx.Columns.Remove("OpeningBalance");
                dtEx.Columns.Remove("BenAccountID");
                dtEx.AcceptChanges();


                DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
                DataTable dtReportHeader = new DataTable();
                dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
                DataRow HeaderRow = dtReportHeader.NewRow();
                HeaderRow[0] = CompanyName.Rows[0][0].ToString() + "[" + HttpContext.Current.Session["usersegid"] + "]";
                dtReportHeader.Rows.Add(HeaderRow);
                DataRow DrRowR1 = dtReportHeader.NewRow();
                DrRowR1[0] = "NSDL Transaction Report (From  " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " To " + oconverter.ArrangeDate2(dtto.Value.ToString()) + ")";
                dtReportHeader.Rows.Add(DrRowR1);
                DataTable dtReportFooter = new DataTable();
                dtReportFooter.Columns.Add(new DataColumn("Footer", typeof(String))); //0
                DataRow FooterRow = dtReportFooter.NewRow();
                FooterRow[0] = "* * *  End Of Report * * *   ";
                dtReportFooter.Rows.Add(FooterRow);

                objExcel.ExportToExcelforExcel(dtEx, "NSDL Transaction Cum Holding Report", "Branch/Group Total", dtReportHeader, dtReportFooter);

                CompanyName.Clear();
                CompanyName.Dispose();
                dtReportHeader.Clear();
                dtReportHeader.Dispose();
                dtReportFooter.Clear();
                dtReportFooter.Dispose();
                dtEx.Clear();
                dtEx.Dispose();
            }
            else
            {
                norecord.Visible = true;
                list.Style["display"] = "none";
                tblpage.Style["display"] = "none";
                norecord.Style["display"] = "display";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Reset33", "Reset();", true);
            }

            transDs.Clear();
            transDs.Dispose();
        }
        #endregion

        #region PDF
        protected void btnPdfExport_Click(object sender, EventArgs e)
        {
            BindClientsOnGenerate();
            if (ddlGenerate.SelectedValue == "P")
            {
                list.Style["display"] = "display";
                tblpage.Style["display"] = "display";
                bindDetails();
            }
        }

        public void showCrystalReport()
        {
            DataSet dsLogo;
            DataTable dtLogo;
            DataSet transDs;
            //===Add Data In Trans dataTable Through Looping For Get Data One By One Clients===========
            SetPropertiesValue();
            transDs = new DataSet();
            transDs = GetClientsData();
            for (int k = 0; k < transDs.Tables[0].Rows.Count; k++)
            {
                if (k > 0)
                {
                    if (transDs.Tables[0].Rows[k - 1]["NsdlClients_BenAccountID"].ToString() == transDs.Tables[0].Rows[k]["NsdlClients_BenAccountID"].ToString()
                        && transDs.Tables[0].Rows[k - 1]["NSDLISIN_Number"].ToString() == transDs.Tables[0].Rows[k]["NSDLISIN_Number"].ToString()
                        && transDs.Tables[0].Rows[k - 1]["NsdlTransaction_SettlementNumber"].ToString() == transDs.Tables[0].Rows[k]["NsdlTransaction_SettlementNumber"].ToString()
                        && transDs.Tables[0].Rows[k - 1]["AccountType"].ToString() == transDs.Tables[0].Rows[k]["AccountType"].ToString())
                    {
                        if (transDs.Tables[0].Rows[k]["NsdlTransaction_Particulars"].ToString() != "No Transaction Recorded For This ISIN Of This Period")
                            transDs.Tables[0].Rows[k]["NsdlTransaction_Quantity"] = Convert.ToString(Convert.ToDecimal(transDs.Tables[0].Rows[k - 1]["NsdlTransaction_Quantity"].ToString()) + Convert.ToDecimal(transDs.Tables[0].Rows[k]["credit"].ToString()) - Convert.ToDecimal(transDs.Tables[0].Rows[k]["debit"].ToString()));
                    }
                    else
                    {
                        if (transDs.Tables[0].Rows[k]["NsdlTransaction_Particulars"].ToString() != "No Transaction Recorded For This ISIN Of This Period")
                            transDs.Tables[0].Rows[k]["NsdlTransaction_Quantity"] = Convert.ToString(Convert.ToDecimal(transDs.Tables[0].Rows[k]["openingbalance"].ToString()) + Convert.ToDecimal(transDs.Tables[0].Rows[k]["credit"].ToString()) - Convert.ToDecimal(transDs.Tables[0].Rows[k]["debit"].ToString()));
                    }
                }
                else
                {
                    if (transDs.Tables[0].Rows[0]["NsdlTransaction_Particulars"].ToString() != "No Transaction Recorded For This ISIN Of This Period")
                        transDs.Tables[0].Rows[0]["NsdlTransaction_Quantity"] = Convert.ToString(Convert.ToDecimal(transDs.Tables[0].Rows[0]["openingbalance"].ToString()) + Convert.ToDecimal(transDs.Tables[0].Rows[0]["credit"].ToString()) - Convert.ToDecimal(transDs.Tables[0].Rows[0]["debit"].ToString()));
                }
            }
            transDs.AcceptChanges();
            //====Start For Logo Add==============
            byte[] logoinByte;
            dtLogo = new DataTable();
            dtLogo.Columns.Add("Image", System.Type.GetType("System.Byte[]"));

            if (oconverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\logo.jpg"), out logoinByte) != 1)
            {
                ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "logo", "alert('Logo not Found.');", true);
            }
            else
            {
                dtLogo.Rows.Add(logoinByte);
            }
            dtLogo.AcceptChanges();
            dsLogo = new DataSet();
            dsLogo.Tables.Add(dtLogo);
            //====End For Logo Add==============

            if (transDs.Tables[0].Rows.Count > 0)
            {
                //======Generate XSD===========================            
                //transDs.Tables[0].WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlTransactionCumHolding.xsd");
                //dsLogo.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlTransaction_CompanyLogo.xsd");

                ReportDocument cdslTransctionReportDocu = new ReportDocument();
                string[] connPath = ConfigurationSettings.AppSettings["DBConnectionDefault"].Split(';');
                string path = Server.MapPath("..\\Reports\\NsdlTransactionCumHolding.rpt");
                cdslTransctionReportDocu.Load(path);
                cdslTransctionReportDocu.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                cdslTransctionReportDocu.SetDataSource(transDs.Tables[0]);
                cdslTransctionReportDocu.Subreports["NSDL_CompanyLogo"].SetDataSource(dsLogo.Tables[0]);
                cdslTransctionReportDocu.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, "NSDL Transaction Cum Holding");
            }
            else
            {
                norecord.Visible = true;
                list.Style["display"] = "none";
                tblpage.Style["display"] = "none";
                norecord.Style["display"] = "display";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Reset32", "Reset();", true);
            }
            dtLogo.Clear();
            dtLogo.Dispose();
            dsLogo.Clear();
            dsLogo.Dispose();
            transDs.Clear();
            transDs.Dispose();
        }
        #endregion
    }
}