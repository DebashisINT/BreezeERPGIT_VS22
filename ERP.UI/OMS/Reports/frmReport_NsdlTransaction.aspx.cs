using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_NsdlTransaction : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
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
        BusinessLogicLayer.GenericMethod oGenericMethod;
        ClsDropDownlistNameSpace.clsDropDownList clsdropdown = new ClsDropDownlistNameSpace.clsDropDownList();

        DailyReports dailyreport = new DailyReports();

        #endregion

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
                //string fromDate = currentDate.Replace(currentDate.Substring(0, currentDate.IndexOf("-")), "01");
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
            if (id.Contains("CallAjax-ISIN") || id.Contains("CallAjax-Settlement") || id == "CallAjax-Client" || id == "CallAjax-Branch" || id == "CallAjax-Group")
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

        #region CallAjax
        void CallUserList(string WhichCall)
        {
            stdate = Convert.ToDateTime(dtfrom.Value).ToString("yyyy-MM-dd");
            endDate = Convert.ToDateTime(dtto.Value).ToString("yyyy-MM-dd");

            #region For ISIN and Settlement after pageload get Date Range and list Specific ISIN and Settlement items
            string fromDate = null;
            string toDate = null;
            string clientTypeValue = null;
            string clientHiddenValue = null;
            string isinHiddenValue = null;
            if (WhichCall != null && WhichCall.Contains("@"))
            {
                string[] splitDates = WhichCall.Split('@');
                fromDate = splitDates[1].ToString();
                toDate = splitDates[2].ToString();
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
                //string strQueryISIN_Table = " Trans_NsdlTransaction,Master_NsdlClients,Master_NSDLISIN ";
                //string strQueryISIN_FieldName = " distinct top 10 (ltrim(rtrim(NSDLISIN_CompanyName)) + '   [ ' + convert(varchar,[NSDLISIN_Number]) + ' ]')  as ISINName,NSDLISIN_Number as ISINID ";
                //string strQueryISIN_WhereClause = " Master_NsdlClients.NsdlClients_BenAccountID=Trans_NsdlTransaction.NsdlTransaction_BenAccountNumber and  Master_NSDLISIN.NSDLISIN_Number=Trans_NsdlTransaction.NsdlTransaction_ISIN ";
                //strQueryISIN_WhereClause += " and NsdlTransaction_Date between '" + fromDate + " 00:00:00" + "' and '" + toDate + " 23:59:59" + "'";
                //if (clientTypeValue != "All")
                //    strQueryISIN_WhereClause += " and NsdlClients_BenType='" + clientTypeValue + "' ";
                //if (clientHiddenValue != "")
                //    strQueryISIN_WhereClause += " and  NsdlClients_BenAccountID in (select * From dbo.fnSplitReturnTable( '" + clientHiddenValue + "',','))";
                //strQueryISIN_WhereClause += " and  (NSDLISIN_Number Like '%RequestLetter%' or NSDLISIN_CompanyName Like 'RequestLetter%') ";

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
                //string strQuerySettlement_Table = " Trans_NsdlTransaction,Master_NsdlClients,Master_NSDLISIN ";
                //string strQuerySettlement_FieldName = " distinct top 10 NsdlTransaction_SettlementNumber  as SettlementName, NsdlTransaction_SettlementNumber   as SettlementID ";
                //string strQuerySettlement_WhereClause = " Master_NsdlClients.NsdlClients_BenAccountID=Trans_NsdlTransaction.NsdlTransaction_BenAccountNumber and  Master_NSDLISIN.NSDLISIN_Number=Trans_NsdlTransaction.NsdlTransaction_ISIN ";
                //strQuerySettlement_WhereClause += " and NsdlTransaction_Date between '" + fromDate + " 00:00:00" + "' and '" + toDate + " 23:59:59" + "'";
                //if (clientTypeValue != "All")
                //    strQuerySettlement_WhereClause += " and NsdlClients_BenType='" + clientTypeValue + "'";
                //if (clientHiddenValue != "")
                //    strQuerySettlement_WhereClause += " and  NsdlClients_BenAccountID in (select * From dbo.fnSplitReturnTable( '" + clientHiddenValue + "',','))";
                //if (isinHiddenValue != "")
                //    strQuerySettlement_WhereClause += " and  NsdlTransaction_ISIN= '" + isinHiddenValue + "'";
                //strQuerySettlement_WhereClause += " and  NsdlTransaction_SettlementNumber Like '%RequestLetter%'  ";

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
        }
        #endregion

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

        //void bindFrmToDate()
        //{
        //    int transMonth, transYear;
        //    if (DateTime.Now.Month == 1)
        //    {
        //        transMonth = 12;
        //        transYear = DateTime.Now.Year - 1;
        //    }
        //    else
        //    {
        //        transMonth = DateTime.Now.Month - 1;
        //        transYear = DateTime.Now.Year;
        //    }
        //    DateTime firstDay = new DateTime(transYear, transMonth, 1);
        //    DateTime lastDayOfMonth = firstDay.AddMonths(1).AddTicks(-1);
        //    string month = String.Format("{0:MM}", lastDayOfMonth);
        //    string date = String.Format("{0:dd-MM-yyyy}", lastDayOfMonth);
        //    string[] dateSplit = date.Split('-');

        //    dtto.Text = dateSplit[0] + "-" + month + "-" + dateSplit[2];

        //    month = String.Format("{0:MM}", firstDay);
        //    date = String.Format("{0:dd-MM-yyyy}", firstDay);
        //    dateSplit = date.Split('-');

        //    dtfrom.Text = dateSplit[0] + "-" + month + "-" + dateSplit[2];
        //}

        void bindDetails()
        {
            string select, where;
            stdate = Convert.ToDateTime(dtfrom.Value).ToString("yyyy-MM-dd");
            endDate = Convert.ToDateTime(dtto.Value).ToString("yyyy-MM-dd");
            DT.Clear();
            DT.Dispose();
            totolRecord = 0;
            pageindex = 0;




            if (cmpid != "")
            {
                DT = dailyreport.ShowNsdlTransactionHeaderList(stdate, endDate, cmpid, isinid, SettlementID, CmbClientType.Value.ToString(),
                    Session["userid"].ToString(), HttpContext.Current.Session["userbranchHierarchy"].ToString());
            }
            else
            {
                norecord.Visible = true;
                list.Style["display"] = "none";
                tblpage.Style["display"] = "none";
                norecord.Style["display"] = "display";
            }

            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{
            //    SqlCommand cmd = new SqlCommand("sp_ShowNsdlTransactionHeaderList", con);
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.AddWithValue("@stdate", stdate);
            //    cmd.Parameters.AddWithValue("@eddate", endDate);
            //    if (cmpid != "")
            //    {
            //        cmd.Parameters.AddWithValue("@benAccNo", cmpid);
            //    }
            //    if (isinid != "")
            //    {
            //        cmd.Parameters.AddWithValue("@isin", isinid);
            //    }
            //    else
            //    {
            //        cmd.Parameters.AddWithValue("@isin", "na");
            //    }
            //    if (SettlementID != "")
            //    {
            //        cmd.Parameters.AddWithValue("@settlement_id", SettlementID);
            //    }
            //    else
            //    {
            //        cmd.Parameters.AddWithValue("@settlement_id", "na");
            //    }
            //    if (CmbClientType.Value == "All")
            //    {
            //        cmd.Parameters.AddWithValue("@bentype", "na");
            //    }
            //    else
            //    {
            //        cmd.Parameters.AddWithValue("@bentype", CmbClientType.Value);
            //    }
            //    cmd.Parameters.AddWithValue("@userid", Session["userid"]);
            //    cmd.Parameters.AddWithValue("@branchid", HttpContext.Current.Session["userbranchHierarchy"].ToString());
            //    if (cmpid != "")
            //    {
            //        cmd.CommandTimeout = 0;
            //        SqlDataAdapter da = new SqlDataAdapter();
            //        da.SelectCommand = cmd;
            //        da.Fill(DT);
            //    }
            //    else
            //    {
            //        norecord.Visible = true;
            //        list.Style["display"] = "none";
            //        tblpage.Style["display"] = "none";
            //        norecord.Style["display"] = "display";
            //    }
            //}
            if (DT.Rows.Count > 0)
            {
                totolRecord = DT.Rows.Count;
                if (ddlGenerate.SelectedValue == "E")
                {
                    export();
                }
                else if (ddlGenerate.SelectedValue == "P")
                {
                    showCrystalReport();
                }
                else if (ddlGenerate.SelectedValue == "M")
                {
                    SentMail();
                }
                else if (ddlGenerate.SelectedValue == "S")
                {

                    bindTopHeader(0);
                    pageing();
                    norecord.Visible = false;
                    listRecord.Text = pageindex + 1 + " of " + totolRecord + " Clients.";
                    if (DT.Rows.Count == 1)
                        tblpage.Style["display"] = "none";
                    else
                        tblpage.Style["display"] = "display";
                }
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

        void bindTopHeader(int i)
        {
            lblClientId.Text = DT.Rows[i]["NsdlClients_BenAccountID"].ToString();
            status.Text = DT.Rows[i]["NsdlClients_BeneficiaryStatus"].ToString();
            holders.Text = DT.Rows[i]["names"].ToString();
            BenAccId = DT.Rows[i]["NsdlClients_BenAccountID"].ToString().Trim();
            BenType = Convert.ToInt32(DT.Rows[i]["NsdlClients_BenType"]);
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{
            //    SqlCommand cmd = new SqlCommand("sp_NsdlTransaction_FetchTypeSubtype_totaltrans", con);
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.AddWithValue("@stdate", stdate);
            //    cmd.Parameters.AddWithValue("@eddate", endDate);
            //    cmd.Parameters.AddWithValue("@BenAccNo", lblClientId.Text.Trim());
            //    cmd.Parameters.AddWithValue("@BenType", BenType);
            //    cmd.CommandTimeout = 0;
            //    SqlDataAdapter da = new SqlDataAdapter();
            //    da.SelectCommand = cmd;
            //    da.Fill(dtBenTypeSubtype);
            //}
            dtBenTypeSubtype = dailyreport.NsdlTransaction_FetchTypeSubtype_totaltrans(stdate, endDate, lblClientId.Text.Trim(), BenType);

            category.Text = dtBenTypeSubtype.Rows[0]["TypeSubtype"].ToString();
            lblTotalTransction.Text = dtBenTypeSubtype.Rows[0]["totaltrans"].ToString();
            lblTotalTransction1.Text = lblTotalTransction.Text;
            oDBEngine.DeleteValue("Tmp_NSDL_Transaction", " Create_User=" + Session["userid"].ToString());
            ViewState["startRowIndex"] = 0;
            ViewState["pageSize"] = 30;
            ViewState["totalRecord"] = 0;
            ViewState["List"] = null;
            ViewState["prevIsin"] = "";
            ViewState["prevSett"] = "";
            bindList();
        }

        # region Pagination Next Previous Purpose
        protected void btnFirst(object sender, EventArgs e)
        {
            pageindex = 0;
            bindTopHeader(pageindex);
            pageing();
        }

        protected void btnPrevious(object sender, EventArgs e)
        {
            if (pageindex > 0)
            {
                pageindex = pageindex - 1;
            }
            bindTopHeader(pageindex);
            pageing();
        }

        protected void btnNext(object sender, EventArgs e)
        {
            if (pageindex < totolRecord)
            {
                pageindex = pageindex + 1;
            }
            bindTopHeader(pageindex);
            pageing();
        }

        protected void btnLast(object sender, EventArgs e)
        {
            pageindex = totolRecord - 1;
            bindTopHeader(pageindex);
            pageing();
        }

        protected void btnTransnNext_Click(object sender, EventArgs e)
        {
            ViewState["startRowIndex"] = ((int)ViewState["startRowIndex"] + (int)ViewState["pageSize"]);
            if (((int)ViewState["totalRecord"] - (int)ViewState["startRowIndex"]) <= (int)ViewState["pageSize"])
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
            generateTable();
        }

        protected void btnTransPrevious_Click(object sender, EventArgs e)
        {
            ViewState["startRowIndex"] = (int)ViewState["startRowIndex"] - (int)ViewState["pageSize"];
            if ((int)ViewState["startRowIndex"] < 0)
            {
                ViewState["startRowIndex"] = 0;
            }
            if ((int)ViewState["startRowIndex"] == 0)
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
            generateTable();
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

        void bindList()
        {
            stdate = Convert.ToDateTime(dtfrom.Value).ToString("yyyy-MM-dd");
            endDate = Convert.ToDateTime(dtto.Value).ToString("yyyy-MM-dd");
            DataTable List = new DataTable();
            List.Clear();
            SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{
            //    SqlCommand cmd = new SqlCommand("sp_ShowNsdlTransaction", con);
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.AddWithValue("@stdate", stdate);
            //    cmd.Parameters.AddWithValue("@eddate", endDate);
            //    cmd.Parameters.AddWithValue("@benAccNo", lblClientId.Text.Trim());
            //    cmd.Parameters.AddWithValue("@benType", BenType);
            //    if (isinid != "")
            //    {
            //        cmd.Parameters.AddWithValue("@isin", isinid);
            //    }
            //    else
            //    {
            //        cmd.Parameters.AddWithValue("@isin", "na");
            //    }
            //    if (SettlementID != "")
            //    {
            //        cmd.Parameters.AddWithValue("@settlement_id", SettlementID);
            //    }
            //    else
            //    {
            //        cmd.Parameters.AddWithValue("@settlement_id", "na");
            //    }
            //    cmd.CommandTimeout = 0;
            //    SqlDataAdapter da = new SqlDataAdapter();
            //    da.SelectCommand = cmd;
            //    da.Fill(List);
            List = dailyreport.ShowNsdlTransaction(stdate, endDate, lblClientId.Text.Trim(), BenType, isinid.Trim(), SettlementID.Trim());

            DataColumn dc = new DataColumn("openingStatus", System.Type.GetType("System.String"));
            List.Columns.Add(dc);
            DataColumn dc1 = new DataColumn("ClosingStatus", System.Type.GetType("System.String"));
            List.Columns.Add(dc1);
            DataColumn dc2 = new DataColumn("Create_User", System.Type.GetType("System.String"));
            List.Columns.Add(dc2);
            DataColumn dc3 = new DataColumn("RowNo", System.Type.GetType("System.String"));
            List.Columns.Add(dc3);
            for (int k = 0; k < List.Rows.Count; k++)
            {
                if (k > 0)
                {
                    if (List.Rows[k - 1]["NSDLISIN_Number"].ToString() == List.Rows[k]["NSDLISIN_Number"].ToString()
                        && List.Rows[k - 1]["NsdlTransaction_SettlementNumber"].ToString() == List.Rows[k]["NsdlTransaction_SettlementNumber"].ToString()
                        && List.Rows[k - 1]["AccountType"].ToString() == List.Rows[k]["AccountType"].ToString())
                    {
                        if (List.Rows[k]["AccountType"].ToString() == "F")
                        {
                            List.Rows[k]["FreeQty"] = Convert.ToString(Convert.ToDecimal(List.Rows[k - 1]["FreeQty"].ToString()) + Convert.ToDecimal(List.Rows[k]["credit"].ToString()) - Convert.ToDecimal(List.Rows[k]["debit"].ToString()));
                        }
                        else if (List.Rows[k]["AccountType"].ToString() == "D")
                        {
                            List.Rows[k]["DematQty"] = Convert.ToString(Convert.ToDecimal(List.Rows[k - 1]["DematQty"].ToString()) + Convert.ToDecimal(List.Rows[k]["credit"].ToString()) - Convert.ToDecimal(List.Rows[k]["debit"].ToString()));
                        }
                        else if (List.Rows[k]["AccountType"].ToString() == "R")
                        {
                            List.Rows[k]["RematQty"] = Convert.ToString(Convert.ToDecimal(List.Rows[k - 1]["RematQty"].ToString()) + Convert.ToDecimal(List.Rows[k]["credit"].ToString()) - Convert.ToDecimal(List.Rows[k]["debit"].ToString()));
                        }
                        else if (List.Rows[k]["AccountType"].ToString() == "P")
                        {
                            List.Rows[k]["PledgedQty"] = Convert.ToString(Convert.ToDecimal(List.Rows[k - 1]["PledgedQty"].ToString()) + Convert.ToDecimal(List.Rows[k]["credit"].ToString()) - Convert.ToDecimal(List.Rows[k]["debit"].ToString()));
                        }
                    }
                    else
                    {
                        if (List.Rows[k]["AccountType"].ToString() == "F")
                        {
                            List.Rows[k]["FreeQty"] = Convert.ToString(Convert.ToDecimal(List.Rows[k]["OpeningFreeBalance"].ToString()) + Convert.ToDecimal(List.Rows[k]["credit"].ToString()) - Convert.ToDecimal(List.Rows[k]["debit"].ToString()));
                            List.Rows[k]["openingStatus"] = "F";
                            List.Rows[k]["ISINName"] = Convert.ToString(List.Rows[k]["ISINName"]) + " ( Free )";
                        }
                        else if (List.Rows[k]["AccountType"].ToString() == "D")
                        {
                            List.Rows[k]["DematQty"] = Convert.ToString(Convert.ToDecimal(List.Rows[k]["OpeningDematBalance"].ToString()) + Convert.ToDecimal(List.Rows[k]["credit"].ToString()) - Convert.ToDecimal(List.Rows[k]["debit"].ToString()));
                            List.Rows[k]["openingStatus"] = "D";
                            List.Rows[k]["ISINName"] = Convert.ToString(List.Rows[k]["ISINName"]) + " ( Demat )";
                        }
                        else if (List.Rows[k]["AccountType"].ToString() == "R")
                        {
                            List.Rows[k]["RematQty"] = Convert.ToString(Convert.ToDecimal(List.Rows[k]["OpeningRematBalance"].ToString()) + Convert.ToDecimal(List.Rows[k]["credit"].ToString()) - Convert.ToDecimal(List.Rows[k]["debit"].ToString()));
                            List.Rows[k]["openingStatus"] = "R";
                            List.Rows[k]["ISINName"] = Convert.ToString(List.Rows[k]["ISINName"]) + " ( Remat/RePurchase/Redemption )";
                        }
                        else if (List.Rows[k]["AccountType"].ToString() == "P")
                        {
                            List.Rows[k]["PledgedQty"] = Convert.ToString(Convert.ToDecimal(List.Rows[k]["OpeningPledgedBalance"].ToString()) + Convert.ToDecimal(List.Rows[k]["credit"].ToString()) - Convert.ToDecimal(List.Rows[k]["debit"].ToString()));
                            List.Rows[k]["openingStatus"] = "P";
                            List.Rows[k]["ISINName"] = Convert.ToString(List.Rows[k]["ISINName"]) + " ( Pledged )";
                        }
                        if (List.Rows[k - 1]["AccountType"].ToString() == "F")
                        {
                            List.Rows[k - 1]["ClosingStatus"] = "F";
                        }
                        else if (List.Rows[k - 1]["AccountType"].ToString() == "D")
                        {
                            List.Rows[k - 1]["ClosingStatus"] = "D";
                        }
                        else if (List.Rows[k - 1]["AccountType"].ToString() == "R")
                        {
                            List.Rows[k - 1]["ClosingStatus"] = "R";
                        }
                        else if (List.Rows[k - 1]["AccountType"].ToString() == "P")
                        {
                            List.Rows[k - 1]["ClosingStatus"] = "P";
                        }
                    }
                }
                else
                {
                    if (List.Rows[0]["AccountType"].ToString() == "F")
                    {
                        List.Rows[0]["FreeQty"] = Convert.ToString(Convert.ToDecimal(List.Rows[0]["OpeningFreeBalance"].ToString()) + Convert.ToDecimal(List.Rows[0]["credit"].ToString()) - Convert.ToDecimal(List.Rows[0]["debit"].ToString()));
                        List.Rows[0]["openingStatus"] = "F";
                        List.Rows[k]["ISINName"] = Convert.ToString(List.Rows[k]["ISINName"]) + " ( Free )";
                    }
                    else if (List.Rows[0]["AccountType"].ToString() == "D")
                    {
                        List.Rows[0]["DematQty"] = Convert.ToString(Convert.ToDecimal(List.Rows[0]["OpeningDematBalance"].ToString()) + Convert.ToDecimal(List.Rows[0]["credit"].ToString()) - Convert.ToDecimal(List.Rows[0]["debit"].ToString()));
                        List.Rows[0]["openingStatus"] = "D";
                        List.Rows[k]["ISINName"] = Convert.ToString(List.Rows[k]["ISINName"]) + " ( Demat )";
                    }
                    else if (List.Rows[0]["AccountType"].ToString() == "R")
                    {
                        List.Rows[0]["RematQty"] = Convert.ToString(Convert.ToDecimal(List.Rows[0]["OpeningRematBalance"].ToString()) + Convert.ToDecimal(List.Rows[0]["credit"].ToString()) - Convert.ToDecimal(List.Rows[0]["debit"].ToString()));
                        List.Rows[0]["openingStatus"] = "R";
                        List.Rows[k]["ISINName"] = Convert.ToString(List.Rows[k]["ISINName"]) + " ( Remat/RePurchase/Redemption )";
                    }
                    else if (List.Rows[0]["AccountType"].ToString() == "P")
                    {
                        List.Rows[0]["PledgedQty"] = Convert.ToString(Convert.ToDecimal(List.Rows[0]["OpeningPledgedBalance"].ToString()) + Convert.ToDecimal(List.Rows[0]["credit"].ToString()) - Convert.ToDecimal(List.Rows[0]["debit"].ToString()));
                        List.Rows[0]["openingStatus"] = "P";
                        List.Rows[k]["ISINName"] = Convert.ToString(List.Rows[k]["ISINName"]) + " ( Pledged )";
                    }
                    if (List.Rows[List.Rows.Count - 1]["AccountType"].ToString() == "F")
                    {
                        List.Rows[List.Rows.Count - 1]["ClosingStatus"] = "F";
                    }
                    else if (List.Rows[List.Rows.Count - 1]["AccountType"].ToString() == "D")
                    {
                        List.Rows[List.Rows.Count - 1]["ClosingStatus"] = "D";
                    }
                    else if (List.Rows[List.Rows.Count - 1]["AccountType"].ToString() == "R")
                    {
                        List.Rows[List.Rows.Count - 1]["ClosingStatus"] = "R";
                    }
                    else if (List.Rows[List.Rows.Count - 1]["AccountType"].ToString() == "P")
                    {
                        List.Rows[List.Rows.Count - 1]["ClosingStatus"] = "P";
                    }
                }
                List.Rows[k]["Create_User"] = Session["userid"].ToString();
                List.Rows[k]["RowNo"] = k + 1;
            }
            SqlBulkCopy sbc = new SqlBulkCopy(con);
            sbc.DestinationTableName = "Tmp_NSDL_Transaction";
            if (con.State.Equals(ConnectionState.Open))
            {
                con.Close();
            }
            con.Open();
            sbc.WriteToServer(List);
            con.Close();
            if (List.Rows.Count > 0)
            {
                list.Style["display"] = "display";
                tblpage.Style["display"] = "display";
                ViewState["List"] = List;
                ViewState["totalRecord"] = List.Rows.Count;
                if (List.Rows.Count < (int)ViewState["pageSize"])
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
                generateTable();
            }
            else
            {
                list.Style["display"] = "none";
                tblpage.Style["display"] = "none";
            }
            List.Clear();
            List.Dispose();
            //}
        }

        # region Screen
        void FillAllBranches()
        {
            //string[,] str = oDBEngine.GetFieldValue("(Select distinct (Select ltrim(rtrim(branch_description))+' ['+ltrim(rtrim(branch_code))+']' from tbl_master_branch where branch_id=NsdlClients_BranchID) BranchName,ltrim(rtrim(NsdlClients_BranchID)) as BranchID from Master_NsdlClients) T1 ", " BranchID,BranchName", null, 2);
            string[,] str = oDBEngine.GetFieldValue(" (Select isnull(ltrim(rtrim(branch_description)),'')+' ['+isnull(ltrim(rtrim(branch_code)),'')+']' as BranchName,ltrim(rtrim(branch_id)) as BranchID from tbl_master_branch where branch_id in (Select NsdlClients_BranchID From  Master_NsdlClients,Trans_NsdlTransaction where NsdlClients_BenAccountID=NsdlTransaction_BenAccountNumber)) T1  ", " Distinct BranchID, BranchName ", null, 2, " BranchName ");
            clsdropdown.AddDataToDropDownList(str, ddlSelectedBranch);
        }

        void FillAllGroups(string groupType, string dateFrom, string dateTo, string isinForAllGroupDDLFill, string settlementForAllGroupDDLFill)
        {
            if (groupType != "0")
            {
                string strQueryWhere = " where  NsdlClients_BenAccountID = NsdlTransaction_BenAccountNumber ";
                //strQueryWhere = strQueryWhere + " and NsdlTransaction_Date  Between '" + dateFrom + "'  and  '" + dateTo + "' ";
                //if (isinForAllGroupDDLFill != "na") strQueryWhere = strQueryWhere + " and NsdlTransaction_ISIN='" + isinForAllGroupDDLFill + "' ";
                //if (settlementForAllGroupDDLFill != "na") strQueryWhere = strQueryWhere + " and  NsdlTransaction_SettlementNumber='" + settlementForAllGroupDDLFill + "' ";

                string[,] str = oDBEngine.GetFieldValue(" (Select Distinct GroupID,GroupName From  (Select  Distinct  (NsdlClients_DPID  +  cast(ltrim(rtrim(NsdlClients_BenAccountID))  AS  VARCHAR(8)))  as  ClientID	 From  Master_NsdlClients , Trans_NsdlTransaction   " + strQueryWhere + ")  T1   inner join    (Select grp_contactId , isnull(LTRIM(RTRIM(Gpm_Description)),'')+'['+isnull(LTRIM(RTRIM(gpm_code)),'')+']'  GroupName , gpm_id as GroupID  from  tbl_master_groupMaster , tbl_trans_group   Where  gpm_id=grp_groupMaster  and  gpm_Type='" + groupType + "')  T2  on  ClientID=grp_contactId  ) T3 ", " GroupID,GroupName ", null, 2, null);
                clsdropdown.AddDataToDropDownList(str, ddlSelectedGroup);
            }
            //else
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorOngroupType", "ErrorOngroupType();", true);

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

        void generateTable()
        {
            int startIndex, endIndex;
            startIndex = (int)ViewState["startRowIndex"];
            endIndex = startIndex + (int)ViewState["pageSize"];
            if (endIndex >= (int)ViewState["totalRecord"])
            {
                endIndex = (int)ViewState["totalRecord"];
            }
            lblTransction.Text = endIndex.ToString();
            lblTransction1.Text = lblTransction.Text;
            /////////////////////////////////////////////////////
            DataTable RstTable = new DataTable();
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{
            //    SqlCommand cmd = new SqlCommand("sp_Nsdl_FetchTransaction", con);
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.AddWithValue("@userid", Session["userid"]);
            //    cmd.Parameters.AddWithValue("@startRowIndex", startIndex);
            //    cmd.Parameters.AddWithValue("@endIndex", endIndex);
            //    cmd.CommandTimeout = 0;
            //    SqlDataAdapter da = new SqlDataAdapter();
            //    da.SelectCommand = cmd;
            //    da.Fill(RstTable);
            //}

            RstTable = dailyreport.Nsdl_FetchTransaction(Session["userid"].ToString(), startIndex, endIndex);

            dtTemp = RstTable;
            //////////////////////////////////////////////////
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            string prevIsin = "", prevSett = "", prevAccountType = "";
            int flag = 0;
            for (int k = 0; k < RstTable.Rows.Count; k++)
            {
                if (prevIsin == RstTable.Rows[k][3].ToString() && prevSett == RstTable.Rows[k][6].ToString()
                && prevAccountType == RstTable.Rows[k]["AccountType"].ToString())
                {
                    flag = flag + 1;
                    if (RstTable.Rows[k]["openingStatus"].ToString() == "F")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k][12].ToString())) + "</b></td></tr>";
                    }
                    else if (RstTable.Rows[k]["openingStatus"].ToString() == "D")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k][14].ToString())) + "</b></td></tr>";
                    }
                    else if (RstTable.Rows[k]["openingStatus"].ToString() == "R")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k][16].ToString())) + "</b></td></tr>";
                    }
                    else if (RstTable.Rows[k]["openingStatus"].ToString() == "P")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k][18].ToString())) + "</b></td></tr>";
                    }
                    strHtml += "<tr style=\"background-color:" + GetRowColor(flag) + "\" ><td>" + RstTable.Rows[k][5].ToString() + "</td>";
                    strHtml += "<td >" + RstTable.Rows[k][11].ToString() + "</td>";
                    strHtml += "<td colspan=2>" + RstTable.Rows[k]["Particulars"].ToString() + "</td>";
                    strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["credit"].ToString()))) + "</td>";
                    strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["debit"].ToString()))) + "</td>";
                    if (RstTable.Rows[k]["AccountType"].ToString() == "F")
                        strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["FreeQty"].ToString()))) + "</td></tr>";
                    else if (RstTable.Rows[k]["AccountType"].ToString() == "D")
                        strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["DematQty"].ToString()))) + "</td></tr>";
                    else if (RstTable.Rows[k]["AccountType"].ToString() == "R")
                        strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["RematQty"].ToString()))) + "</td></tr>";
                    else if (RstTable.Rows[k]["AccountType"].ToString() == "P")
                        strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["PledgedQty"].ToString()))) + "</td></tr>";
                    if (RstTable.Rows[k]["ClosingStatus"].ToString() == "F")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingFreeBalance"].ToString())) + "</b></td></tr>";
                    }
                    else if (RstTable.Rows[k]["ClosingStatus"].ToString() == "D")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingDematBalance"].ToString())) + "</b></td></tr>";
                    }
                    else if (RstTable.Rows[k]["ClosingStatus"].ToString() == "R")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingRematBalance"].ToString())) + "</b></td></tr>";
                    }
                    else if (RstTable.Rows[k]["ClosingStatus"].ToString() == "P")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingPledgedBalance"].ToString())) + "</b></td></tr>";
                    }
                }
                else
                {
                    prevIsin = RstTable.Rows[k][3].ToString();
                    prevSett = RstTable.Rows[k][6].ToString();
                    prevAccountType = RstTable.Rows[k]["AccountType"].ToString();
                    strHtml += "<tr style=\"background-color: #FDE9D9\"><td>ISIN:</td><td><b>" + RstTable.Rows[k][3].ToString() + "</b></td>";
                    strHtml += "<td >Security Name:</td>";
                    string ISINName = RstTable.Rows[k]["NsdlISIN_Name"].ToString();
                    if (RstTable.Rows[k]["AccountType"].ToString() == "F")
                    {
                        ISINName = ISINName.Replace("( Free )", "");
                    }
                    strHtml += "<td ><b>" + ISINName + "</b></td>";
                    strHtml += "<td></td>";
                    strHtml += "<td>Settlement No:</td><td><b>" + RstTable.Rows[k][6].ToString() + "</b></td></tr>";
                    strHtml += "<tr style=\"background-color: #DBEEF3\"><td><b>Date<b/></td>";
                    strHtml += "<td><b>Ref. No.</b></td>";
                    strHtml += "<td colspan=2><b>Particulars</b></td>";
                    strHtml += "<td align=\"right\"><b>Credit</b></td>";
                    strHtml += "<td align=\"right\"><b>Debit</b></td>";
                    strHtml += "<td align=\"right\"><b>Current Balance</b></td></tr>";
                    if (RstTable.Rows[k]["openingStatus"].ToString() == "F")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["OpeningFreeBalance"])) + "</b></td></tr>";
                    }
                    else if (RstTable.Rows[k]["openingStatus"].ToString() == "D")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["OpeningDematBalance"].ToString())) + "</b></td></tr>";
                    }
                    else if (RstTable.Rows[k]["openingStatus"].ToString() == "R")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["OpeningRematBalance"].ToString())) + "</b></td></tr>";
                    }
                    else if (RstTable.Rows[k]["openingStatus"].ToString() == "P")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["OpeningPledgedBalance"].ToString())) + "</b></td></tr>";
                    }
                    flag = flag + 1;
                    strHtml += "<tr style=\"background-color:" + GetRowColor(flag) + "\"><td>" + RstTable.Rows[k][5].ToString() + "</td>";
                    strHtml += "<td>" + RstTable.Rows[k][11].ToString() + "</td>";
                    strHtml += "<td colspan=2>" + RstTable.Rows[k]["Particulars"].ToString() + "</td>";
                    strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["credit"].ToString()))) + "</td>";
                    strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["debit"].ToString()))) + "</td>";
                    if (RstTable.Rows[k]["AccountType"].ToString() == "F")
                        strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["FreeQty"].ToString()))) + "</td></tr>";
                    else if (RstTable.Rows[k]["AccountType"].ToString() == "D")
                        strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["DematQty"].ToString()))) + "</td></tr>";
                    else if (RstTable.Rows[k]["AccountType"].ToString() == "R")
                        strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["RematQty"].ToString()))) + "</td></tr>";
                    else if (RstTable.Rows[k]["AccountType"].ToString() == "P")
                        strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["PledgedQty"].ToString()))) + "</td></tr>";
                    if (RstTable.Rows[k]["ClosingStatus"].ToString() == "F")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingFreeBalance"].ToString())) + "</b></td></tr>";
                    }
                    else if (RstTable.Rows[k]["ClosingStatus"].ToString() == "D")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingDematBalance"].ToString())) + "</b></td></tr>";
                    }
                    else if (RstTable.Rows[k]["ClosingStatus"].ToString() == "R")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingRematBalance"].ToString())) + "</b></td></tr>";
                    }
                    else if (RstTable.Rows[k]["ClosingStatus"].ToString() == "P")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingPledgedBalance"].ToString())) + "</b></td></tr>";
                    }
                }
            }
            strHtml += "</table>";
            display.InnerHtml = strHtml;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "hi", "height();", true);
        }

        protected void btnShowGrid_Click(object sender, EventArgs e)
        {
            BindClientsOnGenerate();
            list.Style["display"] = "display";
            tblpage.Style["display"] = "display";
            bindDetails();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Reset", "Reset();", true);
        }
        # endregion Screen

        # region EMail Sent
        void SentMail()
        {
            string disptbl = "";
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                pageindex = i;
                bindTopHeader(pageindex);
                pageing();
                generateEmailTable();
                disptbl = "<table width=\"1050px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\">";
                disptbl += "<tr><td><table width=\"1050px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\"><tr style=\"background-color:#BB694D;color:White;\"><td align=\"left\">Client ID:  " + lblClientId.Text + "</td><td align=\"left\"> Category:  " + category.Text + "</td><td align=\"left\">Status:  " + status.Text + "</td><td align=\"left\">Name Of Holders:  " + holders.Text + "</td></tr></table></td></tr>";
                disptbl += "<tr><td>";
                string emailbdy = disptbl + EmailHTML + "</td></tr></table>";
                string mailid = string.Empty;
                string dpid = string.Empty;
                string emlcnt = string.Empty;
                DataTable dp = oDBEngine.GetDataTable(" master_nsdlclients  ", "  top 1  *  ", "NsdlClients_BenAccountID='" + DT.Rows[i]["NsdlClients_BenAccountId"] + "'");
                dpid = dp.Rows[0]["NsdlClients_DpID"].ToString().Trim();
                emlcnt = dp.Rows[0]["NsdlClients_DpID"].ToString().Trim() + "" + DT.Rows[i]["NsdlClients_BenAccountId"].ToString().Trim();
                DataTable dtCnt = oDBEngine.GetDataTable(" tbl_master_email  ", "  *  ", "eml_cntId='" + emlcnt + "'");
                string branchContact = DT.Rows[i]["NsdlClients_BenAccountId"].ToString().Trim();
                string billdate = oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " To " + oconverter.ArrangeDate2(dtto.Value.ToString());
                string Subject = "NSDL Transaction Report for  " + billdate;
                for (int k = 0; k < dtCnt.Rows.Count; k++)
                {
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "showScreen12", "<script>showScreen();</script>");
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "Reset123", "<script>Reset();</script>");
                    mailid = dtCnt.Rows[k]["eml_email"].ToString().Trim();
                    if (mailid.Length > 0)
                    {
                        if (oDBEngine.SendReportBr(emailbdy, mailid, billdate, Subject, branchContact) == true)
                        {
                            pageindex = 0;
                            bindTopHeader(pageindex);
                            pageing();
                            this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript6", "<script>MailsendT();</script>");
                        }
                        else
                        {
                            if (DT.Rows.Count <= 1)
                            {
                                pageindex = 0;
                                bindTopHeader(pageindex);
                                pageing();
                                this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript9", "<script>MailsendF();</script>");
                            }
                        }
                    }
                    else
                    {
                        pageindex = 0;
                        bindTopHeader(pageindex);
                        pageing();
                        this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript59", "<script>alert('Mail id not found....');</script>");
                    }
                }
                EmailHTML = "";
                disptbl = "";
            }
        }

        void generateEmailTable()
        {
            int startIndex, endIndex;
            startIndex = 0;
            endIndex = (int)ViewState["totalRecord"];
            lblTransction.Text = endIndex.ToString();
            lblTransction1.Text = lblTransction.Text;
            /////////////////////////////////////////////////////
            DataTable RstTable = new DataTable();
            SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{
            //    SqlCommand cmd = new SqlCommand("sp_Nsdl_FetchTransaction", con);
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.AddWithValue("@userid", Session["userid"]);
            //    cmd.Parameters.AddWithValue("@startRowIndex", startIndex);
            //    cmd.Parameters.AddWithValue("@endIndex", endIndex);
            //    cmd.CommandTimeout = 0;
            //    SqlDataAdapter da = new SqlDataAdapter();
            //    da.SelectCommand = cmd;
            //    da.Fill(RstTable);
            //}
            RstTable = dailyreport.Nsdl_FetchTransaction(Session["userid"].ToString(), startIndex, endIndex);
            dtTemp = RstTable;
            //////////////////////////////////////////////////
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            string prevIsin = "", prevSett = "", prevAccountType = "";
            int flag = 0;
            for (int k = 0; k < RstTable.Rows.Count; k++)
            {
                if (prevIsin == RstTable.Rows[k][3].ToString() && prevSett == RstTable.Rows[k][6].ToString()
                && prevAccountType == RstTable.Rows[k]["AccountType"].ToString())
                {
                    flag = flag + 1;
                    if (RstTable.Rows[k]["openingStatus"].ToString() == "F")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k][12].ToString())) + "</b></td></tr>";
                    }
                    else if (RstTable.Rows[k]["openingStatus"].ToString() == "D")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k][14].ToString())) + "</b></td></tr>";
                    }
                    else if (RstTable.Rows[k]["openingStatus"].ToString() == "R")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k][16].ToString())) + "</b></td></tr>";
                    }
                    else if (RstTable.Rows[k]["openingStatus"].ToString() == "P")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k][18].ToString())) + "</b></td></tr>";
                    }
                    strHtml += "<tr style=\"background-color:" + GetRowColor(flag) + "\" ><td>" + RstTable.Rows[k][5].ToString() + "</td>";
                    strHtml += "<td >" + RstTable.Rows[k][11].ToString() + "</td>";
                    strHtml += "<td colspan=2>" + RstTable.Rows[k]["Particulars"].ToString() + "</td>";
                    strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["credit"].ToString()))) + "</td>";
                    strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["debit"].ToString()))) + "</td>";
                    if (RstTable.Rows[k]["AccountType"].ToString() == "F")
                        strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["FreeQty"].ToString()))) + "</td></tr>";
                    else if (RstTable.Rows[k]["AccountType"].ToString() == "D")
                        strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["DematQty"].ToString()))) + "</td></tr>";
                    else if (RstTable.Rows[k]["AccountType"].ToString() == "R")
                        strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["RematQty"].ToString()))) + "</td></tr>";
                    else if (RstTable.Rows[k]["AccountType"].ToString() == "P")
                        strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["PledgedQty"].ToString()))) + "</td></tr>";
                    if (RstTable.Rows[k]["ClosingStatus"].ToString() == "F")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingFreeBalance"].ToString())) + "</b></td></tr>";
                    }
                    else if (RstTable.Rows[k]["ClosingStatus"].ToString() == "D")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingDematBalance"].ToString())) + "</b></td></tr>";
                    }
                    else if (RstTable.Rows[k]["ClosingStatus"].ToString() == "R")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingRematBalance"].ToString())) + "</b></td></tr>";
                    }
                    else if (RstTable.Rows[k]["ClosingStatus"].ToString() == "P")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingPledgedBalance"].ToString())) + "</b></td></tr>";
                    }
                }
                else
                {
                    prevIsin = RstTable.Rows[k][3].ToString();
                    prevSett = RstTable.Rows[k][6].ToString();
                    prevAccountType = RstTable.Rows[k]["AccountType"].ToString();
                    strHtml += "<tr style=\"background-color: #FDE9D9\"><td>ISIN:</td><td><b>" + RstTable.Rows[k][3].ToString() + "</b></td>";
                    strHtml += "<td >Security Name:</td>";
                    string ISINName = RstTable.Rows[k]["NsdlISIN_Name"].ToString();
                    if (RstTable.Rows[k]["AccountType"].ToString() == "F")
                    {
                        ISINName = ISINName.Replace("( Free )", "");
                    }
                    strHtml += "<td ><b>" + ISINName + "</b></td>";
                    strHtml += "<td></td>";
                    strHtml += "<td>Settlement No:</td><td><b>" + RstTable.Rows[k][6].ToString() + "</b></td></tr>";
                    strHtml += "<tr style=\"background-color: #DBEEF3\"><td><b>Date<b/></td>";
                    strHtml += "<td><b>Ref. No.</b></td>";
                    strHtml += "<td colspan=2><b>Particulars</b></td>";
                    strHtml += "<td align=\"right\"><b>Credit</b></td>";
                    strHtml += "<td align=\"right\"><b>Debit</b></td>";
                    strHtml += "<td align=\"right\"><b>Current Balance</b></td></tr>";
                    if (RstTable.Rows[k]["openingStatus"].ToString() == "F")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["OpeningFreeBalance"])) + "</b></td></tr>";
                    }
                    else if (RstTable.Rows[k]["openingStatus"].ToString() == "D")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["OpeningDematBalance"].ToString())) + "</b></td></tr>";
                    }
                    else if (RstTable.Rows[k]["openingStatus"].ToString() == "R")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["OpeningRematBalance"].ToString())) + "</b></td></tr>";
                    }
                    else if (RstTable.Rows[k]["openingStatus"].ToString() == "P")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][0].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["OpeningPledgedBalance"].ToString())) + "</b></td></tr>";
                    }
                    flag = flag + 1;
                    strHtml += "<tr style=\"background-color:" + GetRowColor(flag) + "\"><td>" + RstTable.Rows[k][5].ToString() + "</td>";
                    strHtml += "<td>" + RstTable.Rows[k][11].ToString() + "</td>";
                    strHtml += "<td colspan=2>" + RstTable.Rows[k]["Particulars"].ToString() + "</td>";
                    strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["credit"].ToString()))) + "</td>";
                    strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["debit"].ToString()))) + "</td>";
                    if (RstTable.Rows[k]["AccountType"].ToString() == "F")
                        strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["FreeQty"].ToString()))) + "</td></tr>";
                    else if (RstTable.Rows[k]["AccountType"].ToString() == "D")
                        strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["DematQty"].ToString()))) + "</td></tr>";
                    else if (RstTable.Rows[k]["AccountType"].ToString() == "R")
                        strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["RematQty"].ToString()))) + "</td></tr>";
                    else if (RstTable.Rows[k]["AccountType"].ToString() == "P")
                        strHtml += "<td align=\"right\">" + ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(RstTable.Rows[k]["PledgedQty"].ToString()))) + "</td></tr>";
                    if (RstTable.Rows[k]["ClosingStatus"].ToString() == "F")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingFreeBalance"].ToString())) + "</b></td></tr>";
                    }
                    else if (RstTable.Rows[k]["ClosingStatus"].ToString() == "D")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingDematBalance"].ToString())) + "</b></td></tr>";
                    }
                    else if (RstTable.Rows[k]["ClosingStatus"].ToString() == "R")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingRematBalance"].ToString())) + "</b></td></tr>";
                    }
                    else if (RstTable.Rows[k]["ClosingStatus"].ToString() == "P")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k][1].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td style=\"color:Blue;\" align=\"right\"><b>" + String.Format("{0:0.###}", Convert.ToDecimal(RstTable.Rows[k]["ClosingPledgedBalance"].ToString())) + "</b></td></tr>";
                    }
                }
            }
            strHtml += "</table>";
            EmailHTML = strHtml;
        }

        protected void btnSentMail_Click(object sender, EventArgs e)
        {
            BindClientsOnGenerate();
            if (ddlGenerate.SelectedValue == "M")
            {
                list.Style["display"] = "display";
                tblpage.Style["display"] = "display";
                bindDetails();
            }
        }
        # endregion

        # region Excel Export
        void export()
        {
            DataTable dtEx = new DataTable();
            dtEx.Columns.Add("Date");
            dtEx.Columns.Add("Ref. No.");
            dtEx.Columns.Add("Particulars");
            dtEx.Columns.Add("Credit");
            dtEx.Columns.Add("Debit");
            dtEx.Columns.Add("Current Balance");
            string prevIsin = "", prevSett = "", prevAccountType = "";
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                dtTemp.Clear();
                pageindex = i;
                bindTopHeader(pageindex);
                pageing();
                DataRow rowHead = dtEx.NewRow();
                rowHead[0] = "  ";
                rowHead[1] = "Test";
                dtEx.Rows.Add(rowHead);
                DataRow row9 = dtEx.NewRow();
                row9[0] = "Client ID: " + lblClientId.Text + " ..........||.......... Category:" + category.Text + " ..........||.......... Status:  " + status.Text + "..........||.......... Name Of Holders:  " + holders.Text;
                row9[1] = "Test";
                dtEx.Rows.Add(row9);
                generateExpTable();
                for (int k = 0; k < dtTemp.Rows.Count; k++)
                {
                    if (prevIsin == dtTemp.Rows[k][3].ToString() && prevSett == dtTemp.Rows[k][6].ToString()
                    && prevAccountType == dtTemp.Rows[k]["AccountType"].ToString())
                    {
                        DataRow row6 = dtEx.NewRow();
                        if (dtTemp.Rows[k]["openingStatus"].ToString() == "F")
                        {
                            row6[0] = dtTemp.Rows[k][0].ToString();
                            row6[1] = "Opening Balance";
                            row6[2] = "";
                            row6[3] = "";
                            row6[4] = "";
                            row6[5] = String.Format("{0:0.###}", Convert.ToDecimal(dtTemp.Rows[k][12].ToString()));
                            dtEx.Rows.Add(row6);
                        }
                        else if (dtTemp.Rows[k]["openingStatus"].ToString() == "D")
                        {
                            row6[0] = dtTemp.Rows[k][0].ToString();
                            row6[1] = "Opening Balance";
                            row6[2] = "";
                            row6[3] = "";
                            row6[4] = "";
                            row6[5] = String.Format("{0:0.###}", Convert.ToDecimal(dtTemp.Rows[k][14].ToString()));
                            dtEx.Rows.Add(row6);
                        }
                        else if (dtTemp.Rows[k]["openingStatus"].ToString() == "R")
                        {
                            row6[0] = dtTemp.Rows[k][0].ToString();
                            row6[1] = "Opening Balance";
                            row6[2] = "";
                            row6[3] = "";
                            row6[4] = "";
                            row6[5] = String.Format("{0:0.###}", Convert.ToDecimal(dtTemp.Rows[k][16].ToString()));
                            dtEx.Rows.Add(row6);
                        }
                        else if (dtTemp.Rows[k]["openingStatus"].ToString() == "P")
                        {
                            row6[0] = dtTemp.Rows[k][0].ToString();
                            row6[1] = "Opening Balance";
                            row6[2] = "";
                            row6[3] = "";
                            row6[4] = "";
                            row6[5] = String.Format("{0:0.###}", Convert.ToDecimal(dtTemp.Rows[k][18].ToString()));
                            dtEx.Rows.Add(row6);
                        }
                        DataRow row7 = dtEx.NewRow();
                        row7[0] = dtTemp.Rows[k][5].ToString();
                        if (ddlGenerate.SelectedItem.Value == "E")
                            row7[1] = "'" + dtTemp.Rows[k][11].ToString();
                        else if (ddlGenerate.SelectedItem.Value == "P")
                            row7[1] = dtTemp.Rows[k][11].ToString();
                        row7[2] = dtTemp.Rows[k]["Particulars"].ToString();
                        row7[3] = ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(dtTemp.Rows[k]["credit"].ToString())));
                        row7[4] = ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(dtTemp.Rows[k]["debit"].ToString())));
                        if (dtTemp.Rows[k]["AccountType"].ToString() == "F")
                            row7[5] = ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(dtTemp.Rows[k]["FreeQty"].ToString())));
                        else if (dtTemp.Rows[k]["AccountType"].ToString() == "D")
                            row7[5] = ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(dtTemp.Rows[k]["DematQty"].ToString())));
                        else if (dtTemp.Rows[k]["AccountType"].ToString() == "R")
                            row7[5] = ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(dtTemp.Rows[k]["RematQty"].ToString())));
                        else if (dtTemp.Rows[k]["AccountType"].ToString() == "P")
                            row7[5] = ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(dtTemp.Rows[k]["PledgedQty"].ToString())));
                        dtEx.Rows.Add(row7);
                        DataRow row8 = dtEx.NewRow();
                        if (dtTemp.Rows[k]["ClosingStatus"].ToString() == "F")
                        {
                            row8[0] = dtTemp.Rows[k][1].ToString();
                            row8[1] = "Closing Balance";
                            row8[2] = "";
                            row8[3] = "";
                            row8[4] = "";
                            row8[5] = String.Format("{0:0.###}", Convert.ToDecimal(dtTemp.Rows[k]["ClosingFreeBalance"].ToString()));
                            dtEx.Rows.Add(row8);
                        }
                        else if (dtTemp.Rows[k]["ClosingStatus"].ToString() == "D")
                        {
                            row8[0] = dtTemp.Rows[k][1].ToString();
                            row8[1] = "Closing Balance";
                            row8[2] = "";
                            row8[3] = "";
                            row8[4] = "";
                            row8[5] = String.Format("{0:0.###}", Convert.ToDecimal(dtTemp.Rows[k]["ClosingDematBalance"].ToString()));
                            dtEx.Rows.Add(row8);
                        }
                        else if (dtTemp.Rows[k]["ClosingStatus"].ToString() == "R")
                        {
                            row8[0] = dtTemp.Rows[k][1].ToString();
                            row8[1] = "Closing Balance";
                            row8[2] = "";
                            row8[3] = "";
                            row8[4] = "";
                            row8[5] = String.Format("{0:0.###}", Convert.ToDecimal(dtTemp.Rows[k]["ClosingRematBalance"].ToString()));
                            dtEx.Rows.Add(row8);
                        }
                        else if (dtTemp.Rows[k]["ClosingStatus"].ToString() == "P")
                        {
                            row8[0] = dtTemp.Rows[k][1].ToString();
                            row8[1] = "Closing Balance";
                            row8[2] = "";
                            row8[3] = "";
                            row8[4] = "";
                            row8[5] = String.Format("{0:0.###}", Convert.ToDecimal(dtTemp.Rows[k]["ClosingPledgedBalance"].ToString()));
                            dtEx.Rows.Add(row8);
                        }
                    }
                    else
                    {
                        prevIsin = dtTemp.Rows[k][3].ToString();
                        prevSett = dtTemp.Rows[k][6].ToString();
                        prevAccountType = dtTemp.Rows[k]["AccountType"].ToString();
                        string ISINName = dtTemp.Rows[k]["NsdlISIN_Name"].ToString();
                        if (dtTemp.Rows[k]["AccountType"].ToString() == "F")
                        {
                            ISINName = ISINName.Replace("( Free )", "");
                        }
                        DataRow row1 = dtEx.NewRow();
                        row1[0] = "ISIN: " + dtTemp.Rows[k][3].ToString() + "  ||   Security Name:" + ISINName + "  ||   Settlement No:" + dtTemp.Rows[k][6].ToString();
                        row1[1] = "Test";
                        dtEx.Rows.Add(row1);

                        DataRow row2 = dtEx.NewRow();
                        row2[0] = "Date";
                        row2[1] = "Ref. No.";
                        row2[2] = "Particulars";
                        row2[3] = "Credit";
                        row2[4] = "Debit";
                        row2[5] = "Current Balance";
                        dtEx.Rows.Add(row2);
                        DataRow row3 = dtEx.NewRow();
                        if (dtTemp.Rows[k]["openingStatus"].ToString() == "F")
                        {
                            row3[0] = dtTemp.Rows[k][0].ToString();
                            row3[1] = "Opening Balance";
                            row3[2] = "";
                            row3[3] = "";
                            row3[4] = "";
                            row3[5] = String.Format("{0:0.###}", Convert.ToDecimal(dtTemp.Rows[k]["OpeningFreeBalance"]));
                            dtEx.Rows.Add(row3);
                        }
                        else if (dtTemp.Rows[k]["openingStatus"].ToString() == "D")
                        {

                            row3[0] = dtTemp.Rows[k][0].ToString();
                            row3[1] = "Opening Balance";
                            row3[2] = "";
                            row3[3] = "";
                            row3[4] = "";
                            row3[5] = String.Format("{0:0.###}", Convert.ToDecimal(dtTemp.Rows[k]["OpeningDematBalance"].ToString()));
                            dtEx.Rows.Add(row3);
                        }
                        else if (dtTemp.Rows[k]["openingStatus"].ToString() == "R")
                        {
                            row3[0] = dtTemp.Rows[k][0].ToString();
                            row3[1] = "Opening Balance";
                            row3[2] = "";
                            row3[3] = "";
                            row3[4] = "";
                            row3[5] = String.Format("{0:0.###}", Convert.ToDecimal(dtTemp.Rows[k]["OpeningRematBalance"].ToString()));
                            dtEx.Rows.Add(row3);
                        }
                        else if (dtTemp.Rows[k]["openingStatus"].ToString() == "P")
                        {
                            row3[0] = dtTemp.Rows[k][0].ToString();
                            row3[1] = "Opening Balance";
                            row3[2] = "";
                            row3[3] = "";
                            row3[4] = "";
                            row3[5] = String.Format("{0:0.###}", Convert.ToDecimal(dtTemp.Rows[k]["OpeningPledgedBalance"].ToString()));
                            dtEx.Rows.Add(row3);
                        }
                        DataRow row4 = dtEx.NewRow();
                        row4[0] = dtTemp.Rows[k][5].ToString();
                        if (ddlGenerate.SelectedItem.Value == "E")
                            row4[1] = "'" + dtTemp.Rows[k][11].ToString();
                        else if (ddlGenerate.SelectedItem.Value == "P")
                            row4[1] = dtTemp.Rows[k][11].ToString();
                        row4[2] = dtTemp.Rows[k]["Particulars"].ToString();
                        row4[3] = ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(dtTemp.Rows[k]["credit"].ToString())));
                        row4[4] = ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(dtTemp.Rows[k]["debit"].ToString())));

                        if (dtTemp.Rows[k]["AccountType"].ToString() == "F")
                            row4[5] = ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(dtTemp.Rows[k]["FreeQty"].ToString())));
                        else if (dtTemp.Rows[k]["AccountType"].ToString() == "D")
                            row4[5] = ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(dtTemp.Rows[k]["DematQty"].ToString())));
                        else if (dtTemp.Rows[k]["AccountType"].ToString() == "R")
                            row4[5] = ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(dtTemp.Rows[k]["RematQty"].ToString())));
                        else if (dtTemp.Rows[k]["AccountType"].ToString() == "P")
                            row4[5] = ZeroCheck(String.Format("{0:#.###}", Convert.ToDecimal(dtTemp.Rows[k]["PledgedQty"].ToString())));
                        dtEx.Rows.Add(row4);
                        DataRow row5 = dtEx.NewRow();
                        if (dtTemp.Rows[k]["ClosingStatus"].ToString() == "F")
                        {
                            row5[0] = dtTemp.Rows[k][1].ToString();
                            row5[1] = "Closing Balance";
                            row5[2] = "";
                            row5[3] = "";
                            row5[4] = "";
                            row5[5] = String.Format("{0:0.###}", Convert.ToDecimal(dtTemp.Rows[k]["ClosingFreeBalance"].ToString()));
                            dtEx.Rows.Add(row5);
                        }
                        else if (dtTemp.Rows[k]["ClosingStatus"].ToString() == "D")
                        {
                            row5[0] = dtTemp.Rows[k][1].ToString();
                            row5[1] = "Closing Balance";
                            row5[2] = "";
                            row5[3] = "";
                            row5[4] = "";
                            row5[5] = String.Format("{0:0.###}", Convert.ToDecimal(dtTemp.Rows[k]["ClosingDematBalance"].ToString()));
                            dtEx.Rows.Add(row5);
                        }
                        else if (dtTemp.Rows[k]["ClosingStatus"].ToString() == "R")
                        {
                            row5[0] = dtTemp.Rows[k][1].ToString();
                            row5[1] = "Closing Balance";
                            row5[2] = "";
                            row5[3] = "";
                            row5[4] = "";
                            row5[5] = String.Format("{0:0.###}", Convert.ToDecimal(dtTemp.Rows[k]["ClosingRematBalance"].ToString()));
                            dtEx.Rows.Add(row5);
                        }
                        else if (dtTemp.Rows[k]["ClosingStatus"].ToString() == "P")
                        {
                            row5[0] = dtTemp.Rows[k][1].ToString();
                            row5[1] = "Closing Balance";
                            row5[2] = "";
                            row5[3] = "";
                            row5[4] = "";
                            row5[5] = String.Format("{0:0.###}", Convert.ToDecimal(dtTemp.Rows[k]["ClosingPledgedBalance"].ToString()));
                            dtEx.Rows.Add(row5);
                        }
                    }
                }
            }
            pageindex = 0;
            bindTopHeader(pageindex);
            pageing();
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



            if (ddlGenerate.SelectedItem.Value == "E")
            {
                objExcel.ExportToExcelforExcel(dtEx, "NSDL Transaction Report", "Branch/Group Total", dtReportHeader, dtReportFooter);
            }
            else if (ddlGenerate.SelectedItem.Value == "P")
            {
                objExcel.ExportToPDF(dtEx, "NSDL Transaction Report", "Branch/Group Total", dtReportHeader, dtReportFooter);
            }
        }

        void generateExpTable()
        {
            int startIndex, endIndex;
            startIndex = 0;
            endIndex = (int)ViewState["totalRecord"];
            /////////////////////////////////////////////////////
            DataTable RstTable = new DataTable();
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{
            //    SqlCommand cmd = new SqlCommand("sp_Nsdl_FetchTransaction", con);
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.AddWithValue("@userid", Session["userid"]);
            //    cmd.Parameters.AddWithValue("@startRowIndex", startIndex);
            //    cmd.Parameters.AddWithValue("@endIndex", endIndex);
            //    cmd.CommandTimeout = 0;
            //    SqlDataAdapter da = new SqlDataAdapter();
            //    da.SelectCommand = cmd;
            //    da.Fill(RstTable);
            //}
            RstTable = dailyreport.Nsdl_FetchTransaction(Session["userid"].ToString(), startIndex, endIndex);

            dtTemp = RstTable;
        }

        protected void btnExcelExport_Click(object sender, EventArgs e)
        {
            BindClientsOnGenerate();
            string nsdlClientID = cmpid;
            if (ddlGenerate.SelectedValue == "E")
            {
                list.Style["display"] = "display";
                tblpage.Style["display"] = "display";
                bindDetails();
            }
        }

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
        # endregion

        #region pdf Creation By crystal Report
        void CountOnCrystalReport()
        {
            if (counter != 0 && ddlGenerate.SelectedValue == "P")
                showCrystalReport();
        }

        public void showCrystalReport()
        {
            string select, where;
            stdate = Convert.ToDateTime(dtfrom.Value).ToString("yyyy-MM-dd");
            endDate = Convert.ToDateTime(dtto.Value).ToString("yyyy-MM-dd");
            companyId = Session["LastCompany"].ToString();

            DataSet transDs = new DataSet();
            DataSet holdingDs = new DataSet();
            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{
            //    SqlCommand cmd = new SqlCommand("sp_ShowNsdlTransactionReport", con);
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.AddWithValue("@stdate", stdate);
            //    cmd.Parameters.AddWithValue("@eddate", endDate);
            //    cmd.Parameters.AddWithValue("@compID", companyId);
            //    cmd.Parameters.AddWithValue("@dp", dp);
            //    if (cmpid != "")
            //    {
            //        cmd.Parameters.AddWithValue("@benAccNo", cmpid);
            //    }
            //    else
            //    {
            //        cmd.Parameters.AddWithValue("@benAccNo", "na");
            //    }
            //    if (isinid != "")
            //    {
            //        cmd.Parameters.AddWithValue("@isin", isinid);
            //    }
            //    else
            //    {
            //        cmd.Parameters.AddWithValue("@isin", "na");
            //    }
            //    if (SettlementID != "")
            //    {
            //        cmd.Parameters.AddWithValue("@settlement_id", SettlementID);
            //    }
            //    else
            //    {
            //        cmd.Parameters.AddWithValue("@settlement_id", "na");
            //    }
            //    if (CmbClientType.Value == "All")
            //    {
            //        cmd.Parameters.AddWithValue("@bentype", "na");
            //    }
            //    else
            //    {
            //        cmd.Parameters.AddWithValue("@bentype", CmbClientType.Value);
            //    }
            //    cmd.Parameters.AddWithValue("@branchid", HttpContext.Current.Session["userbranchHierarchy"].ToString());
            //    cmd.CommandTimeout = 0;
            //    SqlDataAdapter da = new SqlDataAdapter();
            //    da.SelectCommand = cmd;
            //    da.Fill(transDs);

            //    SqlCommand cmdHolding = new SqlCommand("sp_ShowNsdlTransactionHoldingReport", con);
            //    cmdHolding.CommandType = CommandType.StoredProcedure;
            //    cmdHolding.Parameters.AddWithValue("@stdate", stdate);
            //    cmdHolding.Parameters.AddWithValue("@eddate", endDate);
            //    if (cmpid != "")
            //    {
            //        cmdHolding.Parameters.AddWithValue("@benAccNo", cmpid);
            //    }
            //    else
            //    {
            //        cmdHolding.Parameters.AddWithValue("@benAccNo", "na");
            //    }
            //    cmdHolding.CommandTimeout = 0;
            //    SqlDataAdapter daHolding = new SqlDataAdapter();
            //    daHolding.SelectCommand = cmdHolding;
            //    daHolding.Fill(holdingDs);
            //}
            transDs = dailyreport.ShowNsdlTransactionReport(stdate, endDate, companyId, dp, cmpid, isinid, SettlementID, CmbClientType.Value.ToString(), HttpContext.Current.Session["userbranchHierarchy"].ToString());
            holdingDs = dailyreport.ShowNsdlTransactionHoldingReport(stdate, endDate, cmpid);

            for (int k = 0; k < transDs.Tables[0].Rows.Count; k++)
            {
                if (k > 0)
                {
                    if (transDs.Tables[0].Rows[k - 1]["NSDLISIN_Number"].ToString() == transDs.Tables[0].Rows[k]["NSDLISIN_Number"].ToString()
                        && transDs.Tables[0].Rows[k - 1]["NsdlTransaction_SettlementNumber"].ToString() == transDs.Tables[0].Rows[k]["NsdlTransaction_SettlementNumber"].ToString()
                        && transDs.Tables[0].Rows[k - 1]["AccountType"].ToString() == transDs.Tables[0].Rows[k]["AccountType"].ToString())
                    {
                        transDs.Tables[0].Rows[k]["NsdlTransaction_Quantity"] = Convert.ToString(Convert.ToDecimal(transDs.Tables[0].Rows[k - 1]["NsdlTransaction_Quantity"].ToString()) + Convert.ToDecimal(transDs.Tables[0].Rows[k]["credit"].ToString()) - Convert.ToDecimal(transDs.Tables[0].Rows[k]["debit"].ToString()));
                    }
                    else
                    {
                        transDs.Tables[0].Rows[k]["NsdlTransaction_Quantity"] = Convert.ToString(Convert.ToDecimal(transDs.Tables[0].Rows[k]["openingbalance"].ToString()) + Convert.ToDecimal(transDs.Tables[0].Rows[k]["credit"].ToString()) - Convert.ToDecimal(transDs.Tables[0].Rows[k]["debit"].ToString()));
                    }
                }
                else
                {
                    transDs.Tables[0].Rows[0]["NsdlTransaction_Quantity"] = Convert.ToString(Convert.ToDecimal(transDs.Tables[0].Rows[0]["openingbalance"].ToString()) + Convert.ToDecimal(transDs.Tables[0].Rows[0]["credit"].ToString()) - Convert.ToDecimal(transDs.Tables[0].Rows[0]["debit"].ToString()));
                }
            }

            //====Start For Logo Add==============
            byte[] logoinByte;
            DataTable dtLogo = new DataTable();
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
            DataSet dsLogo = new DataSet();
            dsLogo.Tables.Add(dtLogo);
            //====End For Logo Add==============

            if (transDs.Tables[0].Rows.Count > 0)
            {
                if (holdingDs.Tables[0].Rows.Count == 0)
                {
                    DataRow dataRow = holdingDs.Tables[0].NewRow();
                    dataRow[0] = "00000";
                    dataRow[2] = "00000";
                    dataRow[3] = "00000";
                    dataRow[4] = "00000";
                    dataRow[5] = "00000";
                    dataRow[6] = "00000";
                    dataRow[7] = "00000";
                    dataRow[8] = "00000";
                    dataRow[9] = "00000";
                    holdingDs.Tables[0].Rows.Add(dataRow);
                }
                //======Generate XSD===========================            
                //transDs.Tables[1].WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlTransaction.xsd");
                //holdingDs.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlHolding.xsd");
                //dsLogo.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlTransaction_CompanyLogo.xsd");

                ReportDocument cdslTransctionReportDocu = new ReportDocument();
                string[] connPath = ConfigurationSettings.AppSettings["DBConnectionDefault"].Split(';');
                string path = Server.MapPath("..\\Reports\\NsdlTransaction.rpt");
                cdslTransctionReportDocu.Load(path);
                cdslTransctionReportDocu.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                cdslTransctionReportDocu.SetDataSource(transDs.Tables[0]);
                cdslTransctionReportDocu.Subreports["NsdlHolding"].SetDataSource(holdingDs);
                cdslTransctionReportDocu.Subreports["NSDL_CompanyLogo"].SetDataSource(dsLogo.Tables[0]);
                cdslTransctionReportDocu.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, "NSDL Transaction");
                transDs.Clear();
                transDs.Dispose();
                holdingDs.Clear();
                holdingDs.Dispose();
            }
            else
            {
                //======Generate XSD===========================            
                //transDs.Tables[1].WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlTransaction.xsd");
                //holdingDs.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlHolding.xsd");
                //dsLogo.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\NsdlTransaction_CompanyLogo.xsd");

                ReportDocument cdslTransctionReportDocu = new ReportDocument();
                string[] connPath = ConfigurationSettings.AppSettings["DBConnectionDefault"].Split(';');
                string path = Server.MapPath("..\\Reports\\NsdlTransaction.rpt");
                cdslTransctionReportDocu.Load(path);
                cdslTransctionReportDocu.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                cdslTransctionReportDocu.SetDataSource(transDs.Tables[1]);
                cdslTransctionReportDocu.Subreports["NsdlHolding"].SetDataSource(holdingDs);
                cdslTransctionReportDocu.Subreports["NSDL_CompanyLogo"].SetDataSource(dsLogo.Tables[0]);
                cdslTransctionReportDocu.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, "NSDL Transaction Cum Holding");
                transDs.Clear();
                transDs.Dispose();
                holdingDs.Clear();
                holdingDs.Dispose();
            }
        }
        # endregion
    }
}