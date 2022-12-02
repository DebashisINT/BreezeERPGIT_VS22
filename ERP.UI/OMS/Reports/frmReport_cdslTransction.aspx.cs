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
    public partial class Reports_new_frmReport_cdslTransction : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        ClsDropDownlistNameSpace.clsDropDownList cls = new ClsDropDownlistNameSpace.clsDropDownList();
        #region Global Variable
        //string companyId = "";
        string dp = "CDSL";
        private static DataTable DT = new DataTable();
        String billnoFinYear, financilaYear;
        static string isinid = "", cmpid = "", SettlementID = "";
        static int pageindex = 0, totolRecord = 0;
        PagedDataSource pds = new PagedDataSource();
        private DataTable dtBenTypeSubtype = new DataTable();
        static string stdate, endDate;
        string data;
        static string Clients;
        string CombinedGroupByQuery = string.Empty;
        //---------For Sending Email
        ExcelFile objExcel = new ExcelFile();
        string EmailHTML = "";
        BusinessLogicLayer.Reports objReports = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        static DataTable dtTemp = new DataTable();
        DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        #endregion

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
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
                string currentDate = oDBEngine.GetDate(103).ToString();
                currentDate = currentDate.Replace("/", "-");
                string fromDate = currentDate.Replace(currentDate.Substring(0, currentDate.IndexOf("-")), "01");
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
                //----Sub Query where Clause condition
                //string strSubQueryISIN_Where = " Where ltrim(rtrim(CdslTransaction_BenAccountNumber))=ltrim(rtrim(substring(CdslClients_BOID,9,len(CdslClients_BOID)))) and CDSLISIN_Number=CdslTransaction_ISIN and (CdslTransaction_Date between '" + fromDate + " 00:00:00" + "' and '" + toDate + " 23:59:59" + "')";
                //if (clientTypeValue != "All")
                //{
                //    strSubQueryISIN_Where += " and CdslClients_BOStatus='" + clientTypeValue + "' ";
                //}
                //if (clientHiddenValue != "")
                //{
                //    strSubQueryISIN_Where += " and  CdslClients_BOID in ( Select * From dbo.FnsplitReturnTable( '" + clientHiddenValue + "',',')) ";
                //}
                ////-------------------------
                //string strQueryISIN_Table = " (Select isnull(ltrim(rtrim(CDSLISIN_ShortName)),'') + ' [' + cast([CDSLISIN_Number]  as varchar(100)) + ']' as ISINName,ltrim(rtrim(CDSLISIN_Number))  As ISINID From Master_CDSLISIN Where CDSLISIN_Number in (Select Distinct CDSLISIN_Number From Master_CdslClients,Trans_CdslTransaction,Master_CDSLISIN " + strSubQueryISIN_Where + " ) )T1 ";
                //string strQueryISIN_FieldName = " Top 10 * ";
                //string strQueryISIN_WhereClause = " ISINName Like 'RequestLetter%' or ISINID Like '%RequestLetter%'";
                //string strQueryISIN_GroupBy = "";
                //string strQueryISIN_OrderBy = " ISINName ";

                string strQueryISIN_Table = " Master_CDSLISIN ";
                string strQueryISIN_FieldName = " distinct Top 10 isnull(ltrim(rtrim(CDSLISIN_ShortName)),'') + ' [' + cast([CDSLISIN_Number]  as varchar(100)) + ']' as ISINName,ltrim(rtrim(CDSLISIN_Number)) As ISINID ";
                string strQueryISIN_WhereClause = " CDSLISIN_ShortName Like 'RequestLetter%' or CDSLISIN_Number Like '%RequestLetter%'";
                string strQueryISIN_GroupBy = "";
                string strQueryISIN_OrderBy = " ISINName ";
                CombinedISINQuery = strQueryISIN_Table + "$" + strQueryISIN_FieldName + "$" + strQueryISIN_WhereClause + "$" + strQueryISIN_GroupBy + "$" + strQueryISIN_OrderBy;
                CombinedISINQuery = CombinedISINQuery.Replace("'", "\\'");
                # endregion

                CombinedGroupByQuery = CombinedISINQuery;
                txtISIN.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + CombinedGroupByQuery + "')");
            }
            if (WhichCall != null && WhichCall.Contains("CallAjax-Settlement"))
            {
                #region Settlement CallAjaxList
                //string strQuerySettlement_Table = " Trans_CdslTransaction,Master_CdslClients,Master_CDSLISIN ";
                //string strQuerySettlement_WhereClause = " ltrim(rtrim(CdslTransaction_BenAccountNumber))=ltrim(rtrim(substring(CdslClients_BOID,9,len(CdslClients_BOID)))) and Master_CDSLISIN.CDSLISIN_Number=Trans_CdslTransaction.CdslTransaction_ISIN ";
                //strQuerySettlement_WhereClause += " and (CdslTransaction_Date between '" + fromDate + " 00:00:00" + "' and '" + toDate + " 23:59:59" + "')";
                //if (clientTypeValue != "All")
                //    strQuerySettlement_WhereClause += "  and CdslClients_BOStatus='" + clientTypeValue + "' ";
                //if (clientHiddenValue != "")
                //    strQuerySettlement_WhereClause += " and  CdslClients_BOID= '" + clientHiddenValue + "' ";
                //if (isinHiddenValue != "")
                //    strQuerySettlement_WhereClause += " and  CdslTransaction_ISIN= '" + isinHiddenValue + "'  ";
                //strQuerySettlement_WhereClause += " and  CdslTransaction_SettlementID Like '%RequestLetter%' ";

                string strQuerySettlement_Table = " Master_CdslCalendar ";
                string strQuerySettlement_FieldName = " distinct top 10 CdslCalendar_SettlementID as SettlementName, CdslCalendar_SettlementID  As SettlementID ";
                string strQuerySettlement_WhereClause = " CdslCalendar_SettlementID Like '%RequestLetter%' ";
                string strQuerySettlement_GroupBy = "";
                string strQuerySettlement_OrderBy = "";
                string CombinedSettlementQuery = strQuerySettlement_Table + "$" + strQuerySettlement_FieldName + "$" + strQuerySettlement_WhereClause + "$" + strQuerySettlement_GroupBy + "$" + strQuerySettlement_OrderBy;
                CombinedSettlementQuery = CombinedSettlementQuery.Replace("'", "\\'");
                # endregion
                CombinedGroupByQuery = CombinedSettlementQuery;
                txtSettlement.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + CombinedSettlementQuery + "')");
            }
            if (WhichCall == "CallAjax-Client")
            {
                #region Client CallAjaxList
                string strQueryClient_Table = " Master_CdslClients ";
                string strQueryClient_FieldName = " Top 10 isnull(ltrim(rtrim(CdslClients_FirstHolderName)),'') + ' [' + cast(CdslClients_BOID  as varchar(20)) +']' as ClientName,'Clients@' + isnull(ltrim(rtrim(CdslClients_FirstHolderName)),'') + ' [' + cast(CdslClients_BOID as varchar(20)) + ']@' + cast(CdslClients_BOID as Varchar(20)) as ClientID,CdslClients_FirstHolderName,CdslClients_BOID ";
                string strQueryClient_WhereClause = " CdslClients_FirstHolderName like '%RequestLetter%' or  CdslClients_BOID like '%RequestLetter%' ";
                string strQueryClient_GroupBy = "";
                string strQueryClient_OrderBy = " ClientName ";

                string CombinedClientQuery = strQueryClient_Table + "$" + strQueryClient_FieldName + "$" + strQueryClient_WhereClause + "$" + strQueryClient_GroupBy + "$" + strQueryClient_OrderBy;
                CombinedClientQuery = CombinedClientQuery.Replace("'", "\\'");
                # endregion
                CombinedGroupByQuery = CombinedClientQuery;
                txtSelection.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + CombinedGroupByQuery + "')");
            }
            if (WhichCall == "CallAjax-Branch")
            {
                #region Branch CallAjaxList
                string strQueryBranch_Table = " (Select branch_description,branch_code,branch_id From Tbl_master_Branch  where Branch_id in (Select Distinct CdslClients_BranchID From Master_CdslClients,Trans_CdslTransaction where substring(ltrim(rtrim(CdslClients_BOID)),9,len(CdslClients_BOID))=CdslTransaction_BenAccountNumber) )T1 ";
                //string strQueryBranch_Table = " tbl_Master_Branch,Master_CdslClients,Trans_CdslTransaction  ";
                string strQueryBranch_FieldName = " Distinct Top 10 ltrim(rtrim(branch_description))+' ['+ltrim(rtrim(branch_code))+']' as BranchName,'Branch@'+ltrim(rtrim(branch_description))+' ['+ltrim(rtrim(branch_code))+']@'+cast(branch_id as Varchar(8)) as BranchID  ";
                //string strQueryBranch_WhereClause = " branch_id=CdslClients_BranchID and ltrim(rtrim(CdslTransaction_BenAccountNumber))=ltrim(rtrim(substring(CdslClients_BOID,9,len(CdslClients_BOID)))) and branch_description Like '%RequestLetter%' or branch_code Like '%RequestLetter%' ";
                string strQueryBranch_WhereClause = " branch_description Like '%RequestLetter%' or branch_code Like '%RequestLetter%'  ";
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

                string strQueryGroup_Table = " ( Select LTRIM(RTRIM(gpm_Description))+' ['+LTRIM(RTRIM(gpm_code))+']' as GroupName, 'Group@'+LTRIM(RTRIM(gpm_Description))+' ['+LTRIM(RTRIM(gpm_code))+']@'+ Cast(gpm_id as Varchar(8)) as GroupID,gpm_code From (Select grp_ContactID,gpm_Description,gpm_code,gpm_id from tbl_master_groupMaster,tbl_trans_group Where gpm_id=grp_groupMaster and gpm_Type='Sub Broker' ) T1 Inner Join (Select distinct CdslClients_BOID From Master_CdslClients,Trans_CdslTransaction Where ltrim(rtrim(CdslTransaction_BenAccountNumber))=ltrim(rtrim(substring(CdslClients_BOID,9,len(CdslClients_BOID))))) T2 On CdslClients_BOID=grp_ContactID ) T3 ";
                string strQueryGroup_FieldName = " Distinct top 10 GroupName,GroupID ";
                string strQueryGroup_WhereClause = " GroupID Like '%RequestLetter%' or GroupName Like '%RequestLetter%'  or gpm_code Like '%RequestLetter%' ";
                string strQueryGroup_GroupBy = "";
                string strQueryGroup_OrderBy = " GroupName ";
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
            //FillAllGroups(grpType);
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
                DtClientSBySelBranch = oDBEngine.GetDataTable(" Master_CdslClients , Trans_CdslTransaction ", " Distinct CdslClients_BOID ", " ltrim(rtrim(CdslTransaction_BenAccountNumber))=ltrim(rtrim(substring(CdslClients_BOID,9,len(CdslClients_BOID))))  ");
            }
            else
            {
                DtClientSBySelBranch = oDBEngine.GetDataTable(" Master_CdslClients , Trans_CdslTransaction ", " Distinct CdslClients_BOID ", " ltrim(rtrim(CdslTransaction_BenAccountNumber))=ltrim(rtrim(substring(CdslClients_BOID,9,len(CdslClients_BOID)))) and CdslClients_BranchID in (" + branchId + ")");
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
            if (groupId == "All" || groupId == null)
            {
                DtClientSBySelGroup = oDBEngine.GetDataTable(" Master_CdslClients ", " Distinct CdslClients_BOID ", " CdslClients_BOID in (Select grp_contactId from tbl_trans_group where grp_groupType='" + ddlGroupType.SelectedValue + "')");
            }
            else
            {
                DtClientSBySelGroup = oDBEngine.GetDataTable(" Master_CdslClients ", " Distinct CdslClients_BOID ", " CdslClients_BOID in (Select grp_contactId from tbl_trans_group where grp_groupMaster in (" + groupId + ") and grp_groupType='" + ddlGroupType.SelectedValue + "')");
            }
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

        void BindClientsOnMailExcelPdfGenerate()
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

        void BindClientsOnGenerate()
        {
            if (hdnGenerateValue.Value == "S")
            {
                BindClientsOnScreenGenerate();
            }
            else
            {
                BindClientsOnMailExcelPdfGenerate();
            }
        }

        void bindDetails()
        {
            stdate = Convert.ToDateTime(dtfrom.Value).ToString("yyyy-MM-dd");
            endDate = Convert.ToDateTime(dtto.Value).ToString("yyyy-MM-dd");
            DT.Clear();
            DT.Dispose();
            totolRecord = 0;
            pageindex = 0;
            string BoID = string.Empty;
            string isin = string.Empty;
            string SettleID = string.Empty;
            string boStatus = string.Empty;

            if (cmpid != "")
            {
                BoID = cmpid;
            }
            else
            {
                BoID = "na";
            }
            if (isinid != "")
            {
                isin = isinid;
            }
            else
            {
                isin = "na";
            }
            if (SettlementID != "")
            {
                SettleID = SettlementID;
            }
            else
            {
                SettleID = "na";
            }
            if (CmbClientType.Value.ToString() == "All")
            {
                boStatus = "na";
            }
            else
            {
                boStatus = Convert.ToString(CmbClientType.Value);
            }
            DT = objReports.cdslTransctionShowList(
                stdate, endDate, companyId(), BoID, isin, SettleID, boStatus,
                Convert.ToString(Session["userid"]), Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"])
                );

            if (DT.Rows.Count > 0)
            {
                if (hdnGenerateValue.Value == "E")
                {
                    export();
                }
                else if (hdnGenerateValue.Value == "P")
                {
                    showCrystalReport();
                }
                else if (hdnGenerateValue.Value == "M")
                {
                    SentMail();
                }
                else if (hdnGenerateValue.Value == "S")
                {
                    totolRecord = DT.Rows.Count;
                    bindTopHeader(0);
                    pageing();
                    norecord.Visible = false;
                    listRecord.Text = pageindex + 1 + " of " + totolRecord + " Reocrds.";
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
            }
        }

        void bindTopHeader(int i)
        {
            lblClientId.Text = DT.Rows[i]["CdslClients_BOID"].ToString();
            category.Text = DT.Rows[i]["CdslClients_AccountCategory"].ToString();
            status.Text = DT.Rows[i]["CdslClients_AccountStatus"].ToString();
            holders.Text = DT.Rows[i]["name"].ToString();
            oDBEngine.DeleteValue("Tmp_CDSL_Transction", " Create_User=" + Session["userid"].ToString());
            ViewState["startRowIndex"] = 0;
            ViewState["pageSize"] = 30;
            ViewState["totalRecord"] = 0;
            ViewState["List"] = null;
            ViewState["prevIsin"] = "";
            ViewState["prevSett"] = "";
            bindList();
        }

        # region Pagination Next Previous
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
            if ((int)ViewState["startRowIndex"] >= (int)ViewState["totalRecord"])
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
            listRecord.Text = pageindex + 1 + " of " + totolRecord + " Clients Transction.";
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
            if (s == " 0")
                return " ";
            else
                return s;
        }

        void bindList()
        {
            stdate = Convert.ToDateTime(dtfrom.Value).ToString("yyyy-MM-dd");
            endDate = Convert.ToDateTime(dtto.Value).ToString("yyyy-MM-dd");
            DataTable List = new DataTable();
            List.Clear();
            string isin = string.Empty;
            string SettleID = string.Empty;
            if (isinid != "")
            {
                isin = isinid;
            }
            else
            {
                isin = "na";
            }
            if (SettlementID != "")
            {
                SettleID = SettlementID;
            }
            else
            {
                SettleID = "na";
            }
            List = objReports.cdslTransctionDisplay1(
                stdate,
                endDate,
               Convert.ToString(lblClientId.Text),
               isin,
               SettleID,
               Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"])
                );
            // List.Dispose();

            DataColumn dc0 = new DataColumn("CdslTransaction_ID", System.Type.GetType("System.String"));
            List.Columns.Add(dc0);
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
                    if (List.Rows[k - 1]["CDSLISIN_Number"].ToString() == List.Rows[k]["CDSLISIN_Number"].ToString() && List.Rows[k - 1]["CdslTransaction_SettlementID"].ToString() == List.Rows[k]["CdslTransaction_SettlementID"].ToString()
                                && List.Rows[k]["transactionType"].ToString() == List.Rows[k - 1]["transactionType"].ToString())
                    {
                        List.Rows[k]["CdslTransaction_Quantity"] = Convert.ToString(Convert.ToDecimal(List.Rows[k - 1]["CdslTransaction_Quantity"].ToString()) + Convert.ToDecimal(List.Rows[k]["credit"].ToString()) - Convert.ToDecimal(List.Rows[k]["debit"].ToString()));
                    }
                    else
                    {
                        List.Rows[k]["CdslTransaction_Quantity"] = Convert.ToString(Convert.ToDecimal(List.Rows[k]["openingbalance"].ToString()) + Convert.ToDecimal(List.Rows[k]["credit"].ToString()) - Convert.ToDecimal(List.Rows[k]["debit"].ToString()));
                        List.Rows[k - 1]["ClosingStatus"] = "Y";
                        List.Rows[k]["openingStatus"] = "Y";
                    }
                }
                else
                {
                    List.Rows[0]["CdslTransaction_Quantity"] = Convert.ToString(Convert.ToDecimal(List.Rows[0]["openingbalance"].ToString()) + Convert.ToDecimal(List.Rows[0]["credit"].ToString()) - Convert.ToDecimal(List.Rows[0]["debit"].ToString()));
                    List.Rows[0]["openingStatus"] = "Y";
                    List.Rows[List.Rows.Count - 1]["ClosingStatus"] = "Y";
                }
                List.Rows[k]["Create_User"] = Session["userid"].ToString();
                List.Rows[k]["RowNo"] = k + 1;
            }
            for (int k = 0; k < List.Rows.Count; k++)
            {
                List.Rows[k]["CdslTransaction_Quantity"] = oconverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(List.Rows[k]["CdslTransaction_Quantity"].ToString()));
                List.Rows[k]["openingbalance"] = oconverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(List.Rows[k]["openingbalance"].ToString()));
                List.Rows[k]["closingbalance"] = oconverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(List.Rows[k]["closingbalance"].ToString()));
                List.Rows[k]["credit"] = oconverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(List.Rows[k]["credit"].ToString()));
                List.Rows[k]["debit"] = oconverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(List.Rows[k]["debit"].ToString()));
            }

            if (List.Rows.Count > 0)
            {
                list.Style["display"] = "display";
                tblpage.Style["display"] = "display";
                ViewState["List"] = List;
                ViewState["totalRecord"] = List.Rows.Count;
                lblTotalTransction.Text = List.Rows.Count.ToString();
                lblTotalTransction1.Text = lblTotalTransction.Text;
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
            // }
            List.Clear();
            List.Dispose();
        }

        private string companyId()
        {
            string[] yearSplit;
            financilaYear = HttpContext.Current.Session["LastFinYear"].ToString();
            yearSplit = financilaYear.Split('-');
            billnoFinYear = "-" + yearSplit[0].Substring(2) + yearSplit[1].Substring(2).Trim() + "-";
            DataTable lastSegMemt = oDBEngine.GetDataTable("(select exch_compId,exch_internalId,exch_TMCode," +
                                                    " isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Segment from tbl_master_companyExchange where exch_compid in " +
                                                        " (select top 1 ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + HttpContext.Current.Session["userlastsegment"] + ")) as D ", "*", "Segment in(select seg_name from tbl_master_segment where seg_id=" + HttpContext.Current.Session["userlastsegment"] + ")");

            return lastSegMemt.Rows[0][0].ToString();
        }

        # region Screen
        void FillAllBranches()
        {
            //string[,] str = oDBEngine.GetFieldValue("(Select distinct (Select ltrim(rtrim(branch_description))+' ['+ltrim(rtrim(branch_code))+']' from tbl_master_branch where branch_id=CdslClients_BranchID) BranchName,ltrim(rtrim(CdslClients_BranchID)) as BranchID from Master_CdslClients) T1 ", " BranchID,BranchName", null, 2);
            string[,] str = oDBEngine.GetFieldValue(" (Select Distinct cast(branch_id as Varchar(8)) as BranchID ,isnull(ltrim(rtrim(branch_description)),'')+' ['+isnull(ltrim(rtrim(branch_code)),'')+']' as BranchName From  (Select branch_description,branch_code,branch_id From Tbl_master_Branch where Branch_id in (Select Distinct CdslClients_BranchID From Master_CdslClients,Trans_CdslTransaction where CdslClients_BOID is not null and substring(ltrim(rtrim(CdslClients_BOID)),9,len(CdslClients_BOID))=CdslTransaction_BenAccountNumber) )T1 ) T2 ", " BranchID,BranchName", null, 2);
            cls.AddDataToDropDownList(str, ddlSelectedBranch);
        }

        //void FillAllGroups(string groupType)
        void FillAllGroups(string groupType, string dateFrom, string dateTo, string isinForAllGroupDDLFill, string settlementForAllGroupDDLFill)
        {
            if (groupType != "0")
            {
                string strQueryWhere = " where  substring(ltrim(rtrim(CdslClients_BOID)),9,len(ltrim(rtrim(CdslClients_BOID)))) = ltrim(rtrim(CdslTransaction_BenAccountNumber))";
                //strQueryWhere = strQueryWhere + " and CdslTransaction_Date  Between '" + dateFrom + "'  and  '" + dateTo + "' ";
                //if (isinForAllGroupDDLFill != "na") strQueryWhere = strQueryWhere + " and CdslTransaction_ISIN='" + isinForAllGroupDDLFill + "' ";
                //if (settlementForAllGroupDDLFill != "na") strQueryWhere = strQueryWhere + " and  CdslTransaction_SettlementID='" + settlementForAllGroupDDLFill + "' ";

                string[,] str = oDBEngine.GetFieldValue(" (Select  Distinct  ltrim(rtrim(CdslClients_BOID))  as  ClientID	 From  Master_CdslClients , Trans_CdslTransaction   " + strQueryWhere + ")  T1   inner join    (Select Distinct grp_contactId , isnull(LTRIM(RTRIM(Gpm_Description)),'')+'['+isnull(LTRIM(RTRIM(gpm_code)),'')+']'  GroupName , gpm_id as GroupID  from  tbl_master_groupMaster , tbl_trans_group   Where  gpm_id=grp_groupMaster  and  gpm_Type='" + groupType + "')  T2  on  ClientID=grp_contactId ", " Distinct GroupID,GroupName ", null, 2, null);
                cls.AddDataToDropDownList(str, ddlSelectedGroup);
            }
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorOngroupType", "ErrorOngroupType();", true);
        }

        void FillDdlSelectedBranch(string selectedBranchList)
        {
            DataTable dtSelBranch = null;
            if (ddlSelectedBranch.Items.Count > 0) ddlSelectedBranch.Items.Clear();
            if (selectedBranchList != null) dtSelBranch = oDBEngine.GetDataTable(" tbl_master_branch ", " ltrim(rtrim(branch_id)) as BranchID, LTRIM(RTRIM(branch_description))+' ['+LTRIM(RTRIM(branch_code))+']' As BranchName ", " branch_id in (" + selectedBranchList + ")");
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
            if (selectedGroupList != null) dtSelGroup = oDBEngine.GetDataTable(" tbl_master_groupMaster ", " ltrim(rtrim(gpm_Description))+' ['+ltrim(rtrim(gpm_code))+']' as GroupName,ltrim(rtrim(gpm_id)) as GroupID ", "gpm_Type='" + ddlGroupType.SelectedValue + "' and gpm_id in (" + selectedGroupList + ")");
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
            if (hdnGenerateValue.Value == "S")
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
            if (hdnGenerateValue.Value == "S")
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
            RstTable = objReports.cdslFeatchTransction(
                Convert.ToString(Session["userid"]),
                startIndex,
                endIndex
                );

            dtTemp = RstTable;
            //////////////////////////////////////////////////
            String strHtml = String.Empty;

            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            string prevIsin = "", prevSett = "", prevtrnasType = "";
            int flag = 0;
            for (int k = 0; k < RstTable.Rows.Count; k++)
            {
                if (prevIsin == RstTable.Rows[k]["CDSLISIN_Number"].ToString() && prevSett == RstTable.Rows[k]["CdslTransaction_SettlementID"].ToString() &&
                        prevtrnasType == RstTable.Rows[k]["transactionType"].ToString())
                {
                    flag = flag + 1;

                    if (RstTable.Rows[k]["openingStatus"].ToString() == "Y")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k]["stdate"].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td align=\"right\">" + RstTable.Rows[k]["openingbalance"].ToString() + "</td></tr>";
                    }
                    strHtml += "<tr style=\"background-color:" + GetRowColor(flag) + "\" ><td>" + RstTable.Rows[k]["CdslTransaction_Date"].ToString() + "</td>";
                    strHtml += "<td colspan=2>" + RstTable.Rows[k]["CdslTransaction_Description"].ToString() + "</td>";
                    strHtml += "<td align=\"right\">" + ZeroCheck(RstTable.Rows[k]["credit"].ToString()) + "</td>";
                    strHtml += "<td align=\"right\">" + ZeroCheck(RstTable.Rows[k]["debit"].ToString()) + "</td>";
                    strHtml += "<td align=\"right\">" + RstTable.Rows[k]["CdslTransaction_Quantity"].ToString() + "</td></tr>";
                    if (RstTable.Rows[k]["ClosingStatus"].ToString() == "Y")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k]["eddate"].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td align=\"right\">" + RstTable.Rows[k]["closingbalance"].ToString() + "</td></tr>";
                    }
                }
                else
                {
                    prevIsin = RstTable.Rows[k]["CDSLISIN_Number"].ToString();
                    prevSett = RstTable.Rows[k]["CdslTransaction_SettlementID"].ToString();
                    prevtrnasType = RstTable.Rows[k]["transactionType"].ToString();
                    strHtml += "<tr style=\"background-color: #FDE9D9;text-align:left\"><td>ISIN</td><td><b>" + RstTable.Rows[k]["CDSLISIN_Number"].ToString() + "</b></td>";
                    strHtml += "<td>Security Name</td><td><b>" + RstTable.Rows[k]["CDSLISIN_ShortName"].ToString() + RstTable.Rows[k]["transactionType"].ToString() + "</b></td>";
                    strHtml += "<td>Settlement No:</td><td><b>" + RstTable.Rows[k]["CdslTransaction_SettlementID"].ToString() + "</b></td></tr>";
                    strHtml += "<tr style=\"background-color: #DBEEF3\"><td><b>Date<b/></td>";
                    strHtml += "<td colspan=2><b>Particulars</b></td>";
                    strHtml += "<td align=\"right\"><b>Credit</b></td>";
                    strHtml += "<td align=\"right\"><b>Debit</b></td>";
                    strHtml += "<td align=\"right\"><b>Current Balance</b></td></tr>";
                    if (RstTable.Rows[k]["openingStatus"].ToString() == "Y")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k]["stdate"].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td align=\"right\">" + RstTable.Rows[k]["openingbalance"].ToString() + "</td></tr>";
                    }
                    flag = flag + 1;
                    strHtml += "<tr style=\"background-color:" + GetRowColor(flag) + "\"><td>" + RstTable.Rows[k]["CdslTransaction_Date"].ToString() + "</td>";
                    strHtml += "<td colspan=2>" + RstTable.Rows[k]["CdslTransaction_Description"].ToString() + "</td>";
                    strHtml += "<td align=\"right\">" + ZeroCheck(RstTable.Rows[k]["credit"].ToString()) + "</td>";
                    strHtml += "<td align=\"right\">" + ZeroCheck(RstTable.Rows[k]["debit"].ToString()) + "</td>";
                    strHtml += "<td align=\"right\">" + RstTable.Rows[k]["CdslTransaction_Quantity"].ToString() + "</td></tr>";

                    if (RstTable.Rows[k]["ClosingStatus"].ToString() == "Y")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k]["eddate"].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td align=\"right\">" + RstTable.Rows[k]["closingbalance"].ToString() + "</td></tr>";
                    }
                }
            }
            strHtml += "</table>";
            display.InnerHtml = strHtml;
            EmailHTML = strHtml;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "hi", "height();", true);
        }
        # endregion Screen

        # region EMail Sent
        void SentMail()
        {
            string disptbl = "";
            if (DT.Rows.Count > 0)
            {
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    //=====
                    pageindex = i;
                    bindTopHeader(pageindex);
                    pageing();
                    //===========
                    generateEmailTable();
                    string emailbdy = "";
                    if (DT.Rows.Count == 1)
                    {
                        disptbl = "<table width=\"1050px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\">";
                        disptbl += "<tr><td><table width=\"1050px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\"><tr style=\"background-color:#BB694D;color:White;\"><td align=\"left\">Client ID:  " + lblClientId.Text + "</td><td align=\"left\"> Category:  " + category.Text + "</td><td align=\"left\">Status:  " + status.Text + "</td><td align=\"left\">Name Of Holders:  " + holders.Text + "</td></tr></table></td></tr>";
                        disptbl += "<tr><td>";
                        emailbdy = disptbl + EmailHTML + "</td></tr></table>";
                    }
                    else
                    {
                        pageindex = i;
                        bindTopHeader(pageindex);
                        pageing();
                        disptbl = "<table width=\"1050px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\">";
                        disptbl += "<tr><td><table width=\"1050px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\"><tr><td align=\"left\">Client ID:" + lblClientId.Text + "</td><td align=\"left\"> Category:" + category.Text + "</td><td align=\"left\">Status:" + status.Text + "</td><td align=\"left\">Name Of Holders:" + holders.Text + "</td></tr></table></td></tr>";
                        disptbl += "<tr><td>";
                        emailbdy = disptbl + EmailHTML + "</td></tr></table>";
                    }
                    DataTable dtCnt = oDBEngine.GetDataTable(" tbl_master_email  ", "  * ", "eml_cntId='" + DT.Rows[i]["CdslClients_BOID"] + "'");
                    string mailid = string.Empty;
                    string branchContact = DT.Rows[i]["CdslClients_BOID"].ToString();
                    string billdate = oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " To " + oconverter.ArrangeDate2(dtto.Value.ToString());
                    string Subject = "CDSL Transaction Report for  " + billdate;
                    for (int k = 0; k < dtCnt.Rows.Count; k++)
                    {
                        this.Page.ClientScript.RegisterStartupScript(GetType(), "ShowMainFilter", "<script>ShowMainFilter();</script>");
                        mailid = dtCnt.Rows[k]["eml_email"].ToString().Trim();
                        if (mailid.Length > 0)
                        {
                            if (oDBEngine.SendReportBr(emailbdy, mailid, billdate, Subject, branchContact) == true)
                            {
                                //this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript78", "<script>eMailHideDetailShowFilter();</script>");
                                this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript6", "<script>MailsendT();</script>");
                            }
                            else
                            {
                                if (DT.Rows.Count <= 1)
                                {
                                    //this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript75", "<script>eMailHideDetailShowFilter();</script>");
                                    this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript9", "<script>MailsendF();</script>");
                                }
                            }
                        }
                        else
                        {
                            //this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript25", "<script>eMailHideDetailShowFilter();</script>");
                            this.Page.ClientScript.RegisterStartupScript(GetType(), "jscript59", "<script>alert('Mail id not found....');</script>");
                        }
                    }
                    EmailHTML = "";
                    disptbl = "";
                }
            }
        }

        void generateEmailTable()
        {
            int startIndex, endIndex;
            startIndex = 0;
            endIndex = (int)ViewState["totalRecord"];
            /////////////////////////////////////////////////////
            DataTable RstTable = new DataTable();
            RstTable = objReports.cdslFeatchTransction(
             Convert.ToString(Session["userid"]),
             startIndex,
             endIndex
             );

            dtTemp = RstTable;
            //////////////////////////////////////////////////
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 0 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            string prevIsin = "", prevSett = "", prevtrnasType = "";
            int flag = 0;
            for (int k = 0; k < RstTable.Rows.Count; k++)
            {
                if (prevIsin == RstTable.Rows[k]["CDSLISIN_Number"].ToString() && prevSett == RstTable.Rows[k]["CdslTransaction_SettlementID"].ToString() &&
                        prevtrnasType == RstTable.Rows[k]["transactionType"].ToString())
                {
                    flag = flag + 1;
                    if (RstTable.Rows[k]["openingStatus"].ToString() == "Y")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k]["stdate"].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td align=\"right\">" + RstTable.Rows[k]["openingbalance"].ToString() + "</td></tr>";
                    }
                    strHtml += "<tr style=\"background-color:" + GetRowColor(flag) + "\" ><td>" + RstTable.Rows[k]["CdslTransaction_Date"].ToString() + "</td>";
                    strHtml += "<td colspan=2>" + RstTable.Rows[k]["CdslTransaction_Description"].ToString() + "</td>";
                    strHtml += "<td align=\"right\">" + ZeroCheck(RstTable.Rows[k]["credit"].ToString()) + "</td>";
                    strHtml += "<td align=\"right\">" + ZeroCheck(RstTable.Rows[k]["debit"].ToString()) + "</td>";
                    strHtml += "<td align=\"right\">" + RstTable.Rows[k]["CdslTransaction_Quantity"].ToString() + "</td></tr>";
                    if (RstTable.Rows[k]["ClosingStatus"].ToString() == "Y")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k]["eddate"].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td align=\"right\">" + RstTable.Rows[k]["closingbalance"].ToString() + "</td></tr>";
                    }
                }
                else
                {
                    prevIsin = RstTable.Rows[k]["CDSLISIN_Number"].ToString();
                    prevSett = RstTable.Rows[k]["CdslTransaction_SettlementID"].ToString();
                    prevtrnasType = RstTable.Rows[k]["transactionType"].ToString();
                    strHtml += "<tr style=\"background-color: #FDE9D9;text-align:left\"><td>ISIN</td><td><b>" + RstTable.Rows[k]["CDSLISIN_Number"].ToString() + "</b></td>";
                    strHtml += "<td>Security Name</td><td><b>" + RstTable.Rows[k]["CDSLISIN_ShortName"].ToString() + RstTable.Rows[k]["transactionType"].ToString() + "</b></td>";
                    strHtml += "<td>Settlement No:</td><td><b>" + RstTable.Rows[k]["CdslTransaction_SettlementID"].ToString() + "</b></td></tr>";
                    strHtml += "<tr style=\"background-color: #DBEEF3\"><td><b>Date<b/></td>";
                    strHtml += "<td colspan=2><b>Particulars</b></td>";
                    strHtml += "<td align=\"right\"><b>Credit</b></td>";
                    strHtml += "<td align=\"right\"><b>Debit</b></td>";
                    strHtml += "<td align=\"right\"><b>Current Balance</b></td></tr>";
                    if (RstTable.Rows[k]["openingStatus"].ToString() == "Y")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k]["stdate"].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\" ><b>Opening Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td align=\"right\">" + RstTable.Rows[k]["openingbalance"].ToString() + "</td></tr>";
                    }
                    flag = flag + 1;
                    strHtml += "<tr style=\"background-color:" + GetRowColor(flag) + "\"><td>" + RstTable.Rows[k]["CdslTransaction_Date"].ToString() + "</td>";
                    strHtml += "<td colspan=2>" + RstTable.Rows[k]["CdslTransaction_Description"].ToString() + "</td>";
                    strHtml += "<td align=\"right\">" + ZeroCheck(RstTable.Rows[k]["credit"].ToString()) + "</td>";
                    strHtml += "<td align=\"right\">" + ZeroCheck(RstTable.Rows[k]["debit"].ToString()) + "</td>";
                    strHtml += "<td align=\"right\">" + RstTable.Rows[k]["CdslTransaction_Quantity"].ToString() + "</td></tr>";

                    if (RstTable.Rows[k]["ClosingStatus"].ToString() == "Y")
                    {
                        strHtml += "<tr><td>" + RstTable.Rows[k]["eddate"].ToString() + "</td>";
                        strHtml += "<td colspan=2 style=\"color:Blue;\"><b>Closing Balance</b></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td></td>";
                        strHtml += "<td align=\"right\">" + RstTable.Rows[k]["closingbalance"].ToString() + "</td></tr>";
                    }
                }
            }
            strHtml += "</table>";
            EmailHTML = strHtml;
        }
        # endregion

        # region Excel Export
        void export()
        {
            DataTable dtEx = new DataTable();
            dtEx.Columns.Add("Date");
            dtEx.Columns.Add("Particulars");
            dtEx.Columns.Add("Credit");
            dtEx.Columns.Add("Debit");
            dtEx.Columns.Add("Current Balance");

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
                row9[0] = "Client ID: " + lblClientId.Text + "   *******    Category:" + category.Text + "   *******   Status:  " + status.Text + "   ********   Name Of Holders:  " + holders.Text;
                row9[1] = "Test";
                dtEx.Rows.Add(row9);
                string prevIsin = "", prevSett = "", prevtrnasType = "";
                int flag = 0;
                dtTemp.Clear();
                generateExTable();
                for (int k = 0; k < dtTemp.Rows.Count; k++)
                {
                    if (prevIsin == dtTemp.Rows[k]["CDSLISIN_Number"].ToString() && prevSett == dtTemp.Rows[k]["CdslTransaction_SettlementID"].ToString() &&
                            prevtrnasType == dtTemp.Rows[k]["transactionType"].ToString())
                    {
                        flag = flag + 1;
                        if (dtTemp.Rows[k]["openingStatus"].ToString() == "Y")
                        {
                            DataRow row1 = dtEx.NewRow();
                            row1[0] = dtTemp.Rows[k]["stdate"].ToString();
                            row1[1] = "Opening Balance";
                            row1[2] = "";
                            row1[3] = "";
                            row1[4] = dtTemp.Rows[k]["openingbalance"].ToString();
                            dtEx.Rows.Add(row1);
                        }
                        DataRow row2 = dtEx.NewRow();
                        row2[0] = dtTemp.Rows[k]["CdslTransaction_Date"].ToString();
                        row2[1] = dtTemp.Rows[k]["CdslTransaction_Description"].ToString();
                        row2[2] = ZeroCheck(dtTemp.Rows[k]["credit"].ToString());
                        row2[3] = ZeroCheck(dtTemp.Rows[k]["debit"].ToString());
                        row2[4] = dtTemp.Rows[k]["CdslTransaction_Quantity"].ToString();
                        dtEx.Rows.Add(row2);

                        DataRow row3 = dtEx.NewRow();
                        if (dtTemp.Rows[k]["ClosingStatus"].ToString() == "Y")
                        {
                            row3[0] = dtTemp.Rows[k]["eddate"].ToString();
                            row3[1] = "Closing Balance";
                            row3[2] = "";
                            row3[3] = "";
                            row3[4] = dtTemp.Rows[k]["closingbalance"].ToString();
                            dtEx.Rows.Add(row3);
                        }
                    }
                    else
                    {
                        prevIsin = dtTemp.Rows[k]["CDSLISIN_Number"].ToString();
                        prevSett = dtTemp.Rows[k]["CdslTransaction_SettlementID"].ToString();
                        prevtrnasType = dtTemp.Rows[k]["transactionType"].ToString();

                        DataRow row4 = dtEx.NewRow();
                        row4[0] = "ISIN : " + dtTemp.Rows[k]["CDSLISIN_Number"].ToString() + " .............. Security Name: " + dtTemp.Rows[k]["CDSLISIN_ShortName"].ToString() + dtTemp.Rows[k]["transactionType"].ToString() + " ..............  Settlement No:" + dtTemp.Rows[k]["CdslTransaction_SettlementID"].ToString();
                        row4[1] = "Test";
                        dtEx.Rows.Add(row4);
                        DataRow row5 = dtEx.NewRow();
                        row5[0] = "Date";
                        row5[1] = "Particulars";
                        row5[2] = "Credit";
                        row5[3] = "Debit";
                        row5[4] = "Current Balance";
                        dtEx.Rows.Add(row5);
                        if (dtTemp.Rows[k]["openingStatus"].ToString() == "Y")
                        {
                            DataRow row6 = dtEx.NewRow();
                            row6[0] = dtTemp.Rows[k]["stdate"].ToString();
                            row6[1] = "Opening Balance";
                            row6[2] = "";
                            row6[3] = "";
                            row6[4] = dtTemp.Rows[k]["openingbalance"].ToString();
                            dtEx.Rows.Add(row6);
                        }
                        DataRow row7 = dtEx.NewRow();
                        row7[0] = dtTemp.Rows[k]["CdslTransaction_Date"].ToString();
                        row7[1] = dtTemp.Rows[k]["CdslTransaction_Description"].ToString();
                        row7[2] = ZeroCheck(dtTemp.Rows[k]["credit"].ToString());
                        row7[3] = ZeroCheck(dtTemp.Rows[k]["debit"].ToString());
                        row7[4] = dtTemp.Rows[k]["CdslTransaction_Quantity"].ToString();
                        dtEx.Rows.Add(row7);

                        if (dtTemp.Rows[k]["ClosingStatus"].ToString() == "Y")
                        {
                            DataRow row8 = dtEx.NewRow();
                            row8[0] = dtTemp.Rows[k]["eddate"].ToString();
                            row8[1] = "Closing Balance";
                            row8[2] = "";
                            row8[3] = "";
                            row8[4] = dtTemp.Rows[k]["closingbalance"].ToString();
                            dtEx.Rows.Add(row8);
                        }
                    }
                }
            }
            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            DataTable dtReportHeader = new DataTable();
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString() + "[" + HttpContext.Current.Session["usersegid"] + "]";
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            DrRowR1[0] = "CDSL Transaction Report (From  " + oconverter.ArrangeDate2(dtfrom.Value.ToString()) + " To " + oconverter.ArrangeDate2(dtto.Value.ToString()) + ")";
            dtReportHeader.Rows.Add(DrRowR1);

            DataTable dtReportFooter = new DataTable();
            dtReportFooter.Columns.Add(new DataColumn("Footer", typeof(String)));
            DataRow FooterRow = dtReportFooter.NewRow();
            FooterRow[0] = "* * *  End Of Report * * *   ";
            dtReportFooter.Rows.Add(FooterRow);

            if (hdnGenerateValue.Value == "E")
            {
                objExcel.ExportToExcelforExcel(dtEx, "CDSL Transaction Report", "Branch/Group Total", dtReportHeader, dtReportFooter);
            }
            //else if (hdnGenerateValue.Value == "P")
            //{
            //    objExcel.ExportToPDF(dtEx, "CDSL Transaction Report", "Branch/Group Total", dtReportHeader, dtReportFooter);
            //}
        }

        void generateExTable()
        {
            int startIndex, endIndex;
            startIndex = 0;
            endIndex = (int)ViewState["totalRecord"];
            lblTransction.Text = endIndex.ToString();
            lblTransction1.Text = lblTransction.Text;

            /////////////////////////////////////////////////////
            DataTable RstTable = new DataTable();
            RstTable = objReports.cdslFeatchTransction(
             Convert.ToString(Session["userid"]),
             startIndex,
             endIndex
             );

            dtTemp = RstTable;
        }
        # endregion

        #region pdf Creation By crystal Report
        public void showCrystalReport()
        {
            int logoStatus;
            string BoID = string.Empty;
            string isin = string.Empty;
            string SettleID = string.Empty;
            string boStatus = string.Empty;

            DataSet transDs = new DataSet();
            DataSet holdingDs = new DataSet();
            DataSet logo = new DataSet();
            DataRow drow;
            byte[] logoinByte;

            string[] stdate = dtfrom.Value.ToString().Split(' ');
            string startdate = stdate[0].ToString() + " 00:00:00";
            string[] endDate = dtto.Value.ToString().Split(' ');
            string enddt = endDate[0].ToString() + " 23:59:00";
            //stdate = Convert.ToDateTime(dtfrom.Value).ToString("yyyy-MM-dd");
            //endDate = Convert.ToDateTime(dtto.Value).ToString("yyyy-MM-dd");

            logoStatus = 1;
            logo.Tables.Add();
            logo.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
            drow = logo.Tables[0].NewRow();
            if (oconverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\logo.jpg"), out logoinByte) == 1)
            {
                drow["Image"] = logoinByte;
            }
            else
            {
                logoStatus = 0;
                ScriptManager.RegisterStartupScript(this, HttpContext.Current.GetType(), "logo", "alert('Logo not Found.');", true);
            }
            logo.Tables[0].Rows.Add(drow);
            if (logoStatus == 1)
            {
                if (cmpid != "")
                {
                    BoID = cmpid;
                }
                else
                {
                    BoID = "na";
                }
                if (isinid != "")
                {

                    isin = isinid;
                }
                else
                {
                    isin = "na";
                }
                if (SettlementID != "")
                {
                    SettleID = SettlementID;
                }
                else
                {
                    SettleID = "na";
                }
                if (CmbClientType.Value.ToString() == "All")
                {
                    boStatus = "na";
                }
                else
                {
                    boStatus = Convert.ToString(CmbClientType.Value);
                }

                transDs = objReports.cdslTransctionShowwithDematandPledge(
                    startdate,
                    enddt,
                    companyId(),
                    dp,
                    BoID,
                    isin,
                    SettleID,
                    boStatus,
                   Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"])
                    );




                String CompanyId, dpId, SegmentId;
                DataTable lastSegMemt = oDBEngine.GetDataTable("(select exch_compId,exch_internalId,exch_TMCode," +
                                               " isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Segment from tbl_master_companyExchange where exch_compid in " +
                                                   " (select top 1 ls_lastCompany from tbl_trans_Lastsegment where ls_lastSegment=" + HttpContext.Current.Session["userlastsegment"] + ")) as D ", "*", "Segment in(select seg_name from tbl_master_segment where seg_id=" + HttpContext.Current.Session["userlastsegment"] + ")");

                CompanyId = lastSegMemt.Rows[0][0].ToString();
                dpId = lastSegMemt.Rows[0][2].ToString();
                SegmentId = lastSegMemt.Rows[0][1].ToString();
                String financilaYear, billnoFinYear, month;
                financilaYear = HttpContext.Current.Session["LastFinYear"].ToString(); //HttpContext.Current.Session["LastFinYear"].ToString();
                string[] yearSplit;
                yearSplit = financilaYear.Split('-');
                billnoFinYear = "-" + yearSplit[0].Substring(2) + yearSplit[1].Substring(2).Trim() + "-";
                if (dtto.Text.Split('-')[1] == "01")
                {
                    month = "JAN";
                }
                else if (dtto.Text.Split('-')[1] == "02")
                {
                    month = "FEB";
                }
                else if (dtto.Text.Split('-')[1] == "03")
                {
                    month = "MAR";
                }
                else if (dtto.Text.Split('-')[1] == "04")
                {
                    month = "APR";
                }
                else if (dtto.Text.Split('-')[1] == "05")
                {
                    month = "MAY";
                }
                else if (dtto.Text.Split('-')[1] == "06")
                {
                    month = "JUN";
                }
                else if (dtto.Text.Split('-')[1] == "07")
                {
                    month = "JUL";
                }
                else if (dtto.Text.Split('-')[1] == "08")
                {
                    month = "AUG";
                }
                else if (dtto.Text.Split('-')[1] == "09")
                {
                    month = "SEP";
                }
                else if (dtto.Text.Split('-')[1] == "10")
                {
                    month = "OCT";
                }
                else if (dtto.Text.Split('-')[1] == "11")
                {
                    month = "NOV";
                }
                else
                {
                    month = "DEC";
                }
                string BenAccount = string.Empty;
                if (cmpid == "na")
                {
                    BenAccount = "NA";
                }
                else
                {
                    BenAccount = cmpid.Substring(8);
                }
                holdingDs = objReports.cdslBill_ReportHolding_transaction(
                    "CDSL" + "-" + month + billnoFinYear,
                    BenAccount,
                    "NA",
                    SegmentId,
                    CompanyId,
                    "CDSL",
                     "0.00",
                     "PinCode",
                     dpId
                    );

                if (transDs.Tables[0].Rows.Count > 0)
                {
                    for (int k = 0; k < transDs.Tables[0].Rows.Count; k++)
                    {
                        if (k > 0)
                        {
                            if (transDs.Tables[0].Rows[k - 1]["CDSLISIN_Number"].ToString() == transDs.Tables[0].Rows[k]["CDSLISIN_Number"].ToString() && transDs.Tables[0].Rows[k - 1]["CdslTransaction_SettlementID"].ToString() == transDs.Tables[0].Rows[k]["CdslTransaction_SettlementID"].ToString()
                                                         && transDs.Tables[0].Rows[k - 1]["transactionType"].ToString() == transDs.Tables[0].Rows[k]["transactionType"].ToString())
                            {
                                transDs.Tables[0].Rows[k]["CdslTransaction_Quantity"] = Convert.ToString(Convert.ToDecimal(transDs.Tables[0].Rows[k - 1]["CdslTransaction_Quantity"].ToString()) + Convert.ToDecimal(transDs.Tables[0].Rows[k]["credit"].ToString()) - Convert.ToDecimal(transDs.Tables[0].Rows[k]["debit"].ToString()));
                            }
                            else
                            {
                                transDs.Tables[0].Rows[k]["CdslTransaction_Quantity"] = Convert.ToString(Convert.ToDecimal(transDs.Tables[0].Rows[k]["openingbalance"].ToString()) + Convert.ToDecimal(transDs.Tables[0].Rows[k]["credit"].ToString()) - Convert.ToDecimal(transDs.Tables[0].Rows[k]["debit"].ToString()));
                            }
                        }
                        else
                        {
                            transDs.Tables[0].Rows[0]["CdslTransaction_Quantity"] = Convert.ToString(Convert.ToDecimal(transDs.Tables[0].Rows[0]["openingbalance"].ToString()) + Convert.ToDecimal(transDs.Tables[0].Rows[0]["credit"].ToString()) - Convert.ToDecimal(transDs.Tables[0].Rows[0]["debit"].ToString()));
                        }
                    }
                    for (int k = 0; k < transDs.Tables[0].Rows.Count; k++)
                    {
                        if (transDs.Tables[0].Rows[k]["credit"].ToString() != "0.000")
                            transDs.Tables[0].Rows[k]["credit"] = oconverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(transDs.Tables[0].Rows[k]["credit"].ToString()));
                        else
                            transDs.Tables[0].Rows[k]["credit"] = DBNull.Value;

                        if (transDs.Tables[0].Rows[k]["debit"].ToString() != "0.000")
                            transDs.Tables[0].Rows[k]["debit"] = oconverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(transDs.Tables[0].Rows[k]["debit"].ToString()));
                        else
                            transDs.Tables[0].Rows[k]["debit"] = DBNull.Value;

                        transDs.Tables[0].Rows[k]["CdslTransaction_Quantity"] = oconverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(transDs.Tables[0].Rows[k]["CdslTransaction_Quantity"].ToString()));
                        transDs.Tables[0].Rows[k]["openingbalance"] = oconverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(transDs.Tables[0].Rows[k]["openingbalance"].ToString()));
                        transDs.Tables[0].Rows[k]["closingbalance"] = oconverter.getFormattedvalueWithCheckingDecimalPlaceOriginalSign(Convert.ToDecimal(transDs.Tables[0].Rows[k]["closingbalance"].ToString()));
                    }
                }

                ReportDocument cdslTransctionReportDocu = new ReportDocument();
                string[] connPath = ConfigurationSettings.AppSettings["DBConnectionDefault"].Split(';');
                string path = Server.MapPath("..\\Reports\\cdsltrans_holding.rpt");
                cdslTransctionReportDocu.Load(path);

                cdslTransctionReportDocu.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                if (transDs.Tables[0].Rows.Count > 0)
                {
                    cdslTransctionReportDocu.SetDataSource(transDs.Tables[0]);
                }
                else
                {
                    cdslTransctionReportDocu.SetDataSource(transDs.Tables[1]);
                }
                cdslTransctionReportDocu.Subreports["logo"].SetDataSource(logo);
                cdslTransctionReportDocu.Subreports["holding"].SetDataSource(holdingDs);
                cdslTransctionReportDocu.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, "CDSL Transction");
                transDs.Clear();
                transDs.Dispose();
                holdingDs.Clear();
                holdingDs.Dispose();
                //   }
            }
        }
        # endregion

        protected void btnShowGenerate_Click(object sender, EventArgs e)
        {
            BindClientsOnGenerate();
            if (hdnGenerateValue.Value == "S")
            {
                list.Style["display"] = "display";
                tblpage.Style["display"] = "display";
                bindDetails();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Reset", "Reset();", true);
            }
            else
            {
                list.Style["display"] = "none";
                tblpage.Style["display"] = "none";
                bindDetails();
            }
            //HiddenField_Client.Value = "";            

        }
    }
}