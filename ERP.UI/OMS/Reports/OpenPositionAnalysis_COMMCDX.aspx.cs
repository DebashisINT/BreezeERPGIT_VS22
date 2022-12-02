using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_OpenPositionAnalysis_COMMCDX : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        #region Local Variable
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        string CombinedGroupByQuery = null;
        string data;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            if (!IsPostBack)
            {
                dtPosition.Date = oDBEngine.GetDate();
                Page.ClientScript.RegisterStartupScript(GetType(), "PageLoad12", "<script>PageLoad();</script>");
                CallUserList(null);
            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___// 

        }

        #region CallAjax
        void CallUserList(string WhichCall)
        {
            string CombinedClientQuery = string.Empty;
            CombinedGroupByQuery = CombinedClientQuery;
            string segmentName = null;
            DataTable dtSegmnt = oDBEngine.GetCompanyDetail(Session["LastCompany"].ToString(), Session["UserSegID"].ToString());
            segmentName = dtSegmnt.Rows[0][4].ToString() + " - " + dtSegmnt.Rows[0][5].ToString();
            if (WhichCall == "CallAjax-Branch")
            {
                string strQueryBranch_Table = @" tbl_master_branch ";
                string strQueryBranch_FieldName = " top 10 ltrim(rtrim(branch_description))+' ['+ltrim(rtrim(branch_code))+']' as BranchName,branch_id as BranchID ";
                string strQueryBranch_WhereClause = " (branch_description Like 'RequestLetter%' or branch_code Like 'RequestLetter%') and branch_id in(" + Session["userbranchHierarchy"].ToString() + ") ";
                string strQueryBranch_GroupBy = "";
                string strQueryBranch_OrderBy = " BranchName ";
                string CombinedBranchQuery = strQueryBranch_Table + "$" + strQueryBranch_FieldName + "$" + strQueryBranch_WhereClause + "$" + strQueryBranch_GroupBy + "$" + strQueryBranch_OrderBy;
                CombinedBranchQuery = CombinedBranchQuery.Replace("'", "\\'");
                CombinedGroupByQuery = CombinedBranchQuery;
            }
            if (WhichCall == "CallAjax-Group")
            {
                if (ddlgrouptype.SelectedValue != "0")
                {
                    string strQueryGroup_Table = " tbl_master_groupMaster ";
                    string strQueryGroup_FieldName = " top 10 ltrim(rtrim(gpm_Description))+' ['+ltrim(rtrim(gpm_code))+']' as GroupName,Cast(gpm_id as varchar) as GroupID ";
                    string strQueryGroup_WhereClause = @" gpm_Type='" + ddlgrouptype.SelectedItem.Text + "' and gpm_Description Like 'RequestLetter%' ";
                    string strQueryGroup_GroupBy = "";
                    string strQueryGroup_OrderBy = " gpm_Description ";
                    string CombinedGroupQuery = strQueryGroup_Table + "$" + strQueryGroup_FieldName + "$" + strQueryGroup_WhereClause + "$" + strQueryGroup_GroupBy + "$" + strQueryGroup_OrderBy;
                    CombinedGroupQuery = CombinedGroupQuery.Replace("'", "\\'");
                    CombinedGroupByQuery = CombinedGroupQuery;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Validate", "Validate(3);", true);
                }
            }
            if (WhichCall == "CallAjax-Client")
            {
                string strQueryClient_Table = @" tbl_master_contact,tbl_master_branch,tbl_master_contactexchange ";
                string strQueryClient_FieldName = @" distinct top 10  isnull(rtrim(cnt_firstName),'')+' '+isnull(rtrim(cnt_middlename),'')+' '+isnull(rtrim(cnt_lastName),'')+' ['+isnull(ltrim(rtrim(crg_tcode)),'')+']'  as ClientName ,cnt_internalID as ClientID ";
                string strQueryClient_WhereClause = @" cnt_branchid=branch_id and crg_cntid=cnt_internalid  and cnt_internalid like 'CL%' AND crg_company ='" + Session["LastCompany"].ToString() + "' AND (crg_tcode LIKE 'RequestLetter%'  OR CNT_FIRSTNAME  LIKE 'RequestLetter%') ";
                string strQueryClient_GroupBy = "";
                string strQueryClient_OrderBy = " ";
                CombinedClientQuery = strQueryClient_Table + "$" + strQueryClient_FieldName + "$" + strQueryClient_WhereClause + "$" + strQueryClient_GroupBy + "$" + strQueryClient_OrderBy;
                CombinedClientQuery = CombinedClientQuery.Replace("'", "\\'");
                CombinedGroupByQuery = CombinedClientQuery;
            }
        }
        #endregion

        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }

        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            if (id == "CallAjax-Client" || id == "CallAjax-Branch" || id == "CallAjax-Group")
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
                    if (idlist[0] != "Client")
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
                            str = "'" + AcVal[0] + "'";
                            str1 = "'" + AcVal[0] + "'" + ";" + val[1];
                        }
                        else
                        {
                            str += ",'" + AcVal[0] + "'";
                            str1 += "," + "'" + AcVal[0] + "'" + ";" + val[1];
                        }
                    }
                }
                if (idlist[0] == "Branch")
                {
                    data = "Branch~" + str;
                }
                else if (idlist[0] == "Group")
                {
                    data = "Group~" + str;
                }
                else if (idlist[0] == "Client")
                {
                    data = "Client~" + str;
                }
            }
        }

        public void BindGroup()
        {
            ddlgrouptype.Items.Clear();
            DataTable DtGroup = oDBEngine.GetDataTable("tbl_master_groupmaster", "distinct gpm_Type", null);
            if (DtGroup.Rows.Count > 0)
            {
                ddlgrouptype.DataSource = DtGroup;
                ddlgrouptype.DataTextField = "gpm_Type";
                ddlgrouptype.DataValueField = "gpm_Type";
                ddlgrouptype.DataBind();
                ddlgrouptype.Items.Insert(0, new ListItem("Select GroupType", "0"));
                DtGroup.Dispose();
            }
            else
            {
                ddlgrouptype.Items.Insert(0, new ListItem("Select GroupType", "0"));
            }
        }

        protected void btnhide_Click(object sender, EventArgs e)
        {
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                if (HiddenField_Group.Value.ToString().Trim() == "")
                {
                    BindGroup();
                }
            }
        }

        protected DataTable BindData()
        {
            DataTable dt = null;
            if (ddlGroup.SelectedItem.Value.ToString().Trim() == "0")/////if branch
            {
                if (rdbranchclientSelected.Checked == true && HiddenField_Branch.Value.ToString().Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD1", "NORECORD(1);", true);
                }
                else
                {
                    dt = runProcedure();
                }
            }
            if (ddlGroup.SelectedItem.Value.ToString().Trim() == "1")/////if group
            {
                if (ddlgrouptype.SelectedItem.Value == "0")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD1", "NORECORD(1);", true);
                }
                else
                {
                    if (rdddlgrouptypeSelected.Checked == true && HiddenField_Group.Value.ToString().Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD1", "NORECORD(1);", true);
                    }
                    else
                    {
                        dt = runProcedure();
                    }
                }
            }
            if (ddlGroup.SelectedItem.Value.ToString().Trim() == "2")/////if client wise
            {
                if (rdbranchclientSelected.Checked == true && HiddenField_Client.Value.ToString().Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD1", "NORECORD(1);", true);
                }
                else
                {
                    dt = runProcedure();
                }
            }
            return dt;
        }

        protected DataTable runProcedure()
        {
            string[] InputName = new string[6];
            string[] InputType = new string[6];
            string[] InputValue = new string[6];

            InputName[0] = "PositionDate";
            InputName[1] = "CompanyID";
            InputName[2] = "GrpBy";
            InputName[3] = "GrpByID";
            InputName[4] = "GrpType";
            InputName[5] = "RptType";

            InputType[0] = "D";
            InputType[1] = "V";
            InputType[2] = "V";
            InputType[3] = "V";
            InputType[4] = "V";
            InputType[5] = "V";

            InputValue[0] = dtPosition.Date.ToString();
            InputValue[1] = Session["LastCompany"].ToString();
            if (ddlGroup.SelectedItem.Value.ToString() == "0")/////option Branch 
            {
                InputValue[2] = "BRANCH";
                if (rdbranchclientAll.Checked)
                {
                    InputValue[3] = "ALL";
                    InputValue[4] = "";
                }
                else
                {
                    InputValue[3] = HiddenField_Branch.Value;
                    InputValue[4] = "";
                }
            }
            else if (ddlGroup.SelectedItem.Value.ToString() == "1")/////option Group
            {
                InputValue[2] = "GROUP";
                string groupType = ddlgrouptype.SelectedItem.Text.ToString().Trim();
                if (rdddlgrouptypeAll.Checked)
                {
                    InputValue[3] = "ALL";
                    InputValue[4] = groupType;
                }
                else
                {
                    InputValue[3] = HiddenField_Group.Value;
                    InputValue[4] = groupType;
                }
            }
            else if (ddlGroup.SelectedItem.Value.ToString() == "2")/////option Clients
            {
                InputValue[2] = "CLIENT";
                if (rdbranchclientAll.Checked)
                {
                    InputValue[3] = "ALL";
                    InputValue[4] = "";
                }
                else
                {
                    InputValue[3] = HiddenField_Client.Value.Replace("'", "");
                    InputValue[4] = "";
                }
            }
            else
            {
                InputValue[2] = "BRANCH";
                InputValue[3] = "ALL";
                InputValue[4] = "";
            }
            InputValue[5] = ddlRptType.SelectedValue.ToString();
            DataTable dtData = SQLProcedures.SelectProcedureArr("Report_OpenPostionAnalysis_CommCdx", InputName, InputType, InputValue);
            return dtData;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataTable resultDt = null;
            if (dtPosition.Value == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Validate", "Validate(1);", true);
            }
            else if (ddlRptType.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Validate", "Validate(2);", true);
            }
            else
            {
                resultDt = BindData();
                if (resultDt != null)
                    ExportToExcel_Generic(resultDt, "2007");
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD", "NORECORD(2);", true);
            }
        }

        protected void ExportToExcel_Generic(DataTable Dt, string ExcelVersion)
        {
            if ((Dt.Rows.Count > 1) && (!Dt.Rows[0]["AssetName"].ToString().Contains("Grand Total")))
            {
                GenericExcelExport oGenericExcelExport = new GenericExcelExport();
                string strDownloadFileName = "";
                string strReportHeader = null;
                strReportHeader = "For Position Date Of " + oconverter.ArrangeDate2(dtPosition.Value.ToString());
                string searchCriteria = null;
                searchCriteria = "Search By ";
                if (ddlGroup.SelectedValue == "1")
                {
                    if (rdddlgrouptypeAll.Checked == true)
                        searchCriteria += "All " + ddlGroup.SelectedItem.Text.ToString().Trim();
                    else if (rdddlgrouptypeSelected.Checked == true)
                        searchCriteria += "Selected " + ddlGroup.SelectedItem.Text.ToString().Trim();
                }
                else
                {
                    if (rdbranchclientAll.Checked == true)
                        searchCriteria += "All " + ddlGroup.SelectedItem.Text.ToString().Trim();
                    else
                        searchCriteria += "Selected " + ddlGroup.SelectedItem.Text.ToString().Trim();
                }
                string exlDateTime = oDBEngine.GetDate(113).ToString();
                string exlTime = exlDateTime.Replace(":", "");
                exlTime = exlTime.Replace(" ", "");
                string FileName = "OpenPositionCommCdx_" + exlTime;
                strDownloadFileName = "~/Documents/";
                string[] strHead = new string[4];
                strHead[0] = exlDateTime;
                strHead[1] = strReportHeader + " And " + searchCriteria;
                strHead[2] = "COMM_CDX Open Position Analysis - " + ddlRptType.SelectedItem.Value;
                DataTable dtcompname = oDBEngine.GetDataTable(" tbl_master_company  ", "cmp_Name", "cmp_internalid='" + HttpContext.Current.Session["LastCompany"] + "' ");
                strHead[3] = "Company Name " + dtcompname.Rows[0]["cmp_Name"].ToString();

                if (Dt.Columns.Contains("SerialByExpiry"))
                {
                    Dt.Columns.Remove("SerialByExpiry");
                }
                Dt.AcceptChanges();

                if (ddlRptType.SelectedValue == "Summary")//Summary wise 
                {                               //5v-1n-2v-6n-1v-1n-1v            
                    string[] ColumnType = { "V", "V", "V", "V", "V", "N", "V", "V", "N", "N", "N", "N", "N", "N", "V", "N", "V" };
                    string[] ColumnSize = { "50", "15", "50", "20", "5", "18,4", "20", "10", "18,0", "18,0", "18,0", "22,6", "28,2", "28,3", "15", "28,2", "10" };
                    string[] ColumnWidthSize = { "30", "10", "25", "15", "5", "12", "10", "10", "12", "12", "12", "15", "15", "15", "5", "15", "5" };
                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                }
                if (ddlRptType.SelectedValue == "Detail")//Detail  6v-1n-2v-13n-1v-2n-1v-2n-1v-2n
                {
                    string[] ColumnType = { "V", "V", "V", "V", "V", "V", "N", "V", "V", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "N", "V", "N", "N", "V", "N", "N", "V", "N", "N" };
                    string[] ColumnSize = { "50", "15", "15", "50", "20", "5", "18,4", "40", "10", "18,0", "28,4", "28,2", "28,2", "18,0", "28,4", "28,2", "28,2", "18,0", "28,4", "22,6", "28,2", "28,2", "20", "20,8", "28,2", "15", "20,8", "28,3", "10", "20,8", "28,2" };
                    string[] ColumnWidthSize = { "30", "10", "10", "25", "10", "5", "15", "15", "5", "12", "15", "15", "15", "12", "15", "15", "15", "12", "15", "18", "15", "15", "5", "15", "15", "5", "15", "15", "5", "15", "15" };
                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, Dt, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Reset", "Reset();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD", "NORECORD(2);", true);
            }
        }
    }
}