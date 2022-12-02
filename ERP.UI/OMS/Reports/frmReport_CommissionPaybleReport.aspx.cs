using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_CommissionPaybleReport : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        static string Branch;
        static string Clients;
        static string InsuComp;
        static string Products;
        static string TeleCaller;
        static string SaleRep;
        static string Associate;
        static string SubBroker;
        DataTable DT = new DataTable();
        string data;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["short"] = "";
                dtDate.EditFormatString = oconverter.GetDateFormat("Date");
                dtDate.Value = oDBEngine.GetDate().AddDays((-1 * oDBEngine.GetDate().Day) + 1);
                dtToDate.EditFormatString = oconverter.GetDateFormat("Date");
                dtToDate.Value = oDBEngine.GetDate();

                rdbbAll.Attributes.Add("OnClick", "SegAll('Branch')");
                rdbClientA.Attributes.Add("OnClick", "SegAll('Clients')");
                rdbInsuCompA.Attributes.Add("OnClick", "SegAll('Ins.Comp')");
                rdbProductA.Attributes.Add("OnClick", "SegAll('Products')");
                rdbTelecallerA.Attributes.Add("OnClick", "SegAll('TeleCaller')");
                rdbSalesRepresentativeA.Attributes.Add("OnClick", "SegAll('Sales Rep.')");
                rdbAssociateA.Attributes.Add("OnClick", "SegAll('Associate')");
                rdbSubBroakerA.Attributes.Add("OnClick", "SegAll('Sub Broker')");

                rdbbSelected.Attributes.Add("OnClick", "SegSelected('Branch')");
                rdbClientS.Attributes.Add("OnClick", "SegSelected('Clients')");
                rdbInsuCompS.Attributes.Add("OnClick", "SegSelected('Ins.Comp')");
                rdbProductS.Attributes.Add("OnClick", "SegSelected('Products')");
                rdbTelecallerS.Attributes.Add("OnClick", "SegSelected('TeleCaller')");
                rdbSalesRepresentativeS.Attributes.Add("OnClick", "SegSelected('Sales Rep.')");
                rdbAssociateS.Attributes.Add("OnClick", "SegSelected('Associate')");
                rdbSubBroakerS.Attributes.Add("OnClick", "SegSelected('Sub Broker')");

                txtsubscriptionID.Attributes.Add("onkeyup", "showOptions(this,'SearchTransInsurance',event)");
                Page.ClientScript.RegisterStartupScript(GetType(), "pageload", "<script>PageLoad();</script>");
                //_____For performing operation without refreshing page___//
                String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
                String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
                //___________-end here___//
            }

        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            FillGrid();

        }
        public void FillGrid()
        {

            string startdate = dtDate.Date.Month.ToString() + "/" + dtDate.Date.Day.ToString() + "/" + dtDate.Date.Year.ToString() + " 00:01 AM";
            string Enddate = dtToDate.Date.Month.ToString() + "/" + dtToDate.Date.Day.ToString() + "/" + dtToDate.Date.Year.ToString() + " 11:59 PM";
            // string wherecondition = " trn_company='" + HttpContext.Current.Session["LastCompany"].ToString() + "' and trn_FinancialYear='" + HttpContext.Current.Session["LastFinYear"].ToString() + "' and cast(trn_TransDate as datetime) between '" + dtDate.Value + "' and '" + dtToDate.Value + "'";

            string wherecondition = " trn_Referal not in (select cnt_internalid from tbl_master_contact where cnt_firstname like '%DIRECT CLIENT%') and trn_Id  is not null and trn_Status=4 and trn_company='" + HttpContext.Current.Session["LastCompany"].ToString() + "' and trn_FinancialYear='" + HttpContext.Current.Session["LastFinYear"].ToString() + "' and (CAST(trn_TransDate AS datetime) >= CONVERT(varchar,'" + startdate + "', 101)) and (CAST(trn_TransDate AS datetime) <= CONVERT(varchar,'" + Enddate + "', 101)) ";

            //  string whereCondGI = " GeneralInsurance_CompanyID='" + HttpContext.Current.Session["LastCompany"].ToString() + "' and GeneralInsurance_FinYear='" + HttpContext.Current.Session["LastFinYear"].ToString() + "' and cast(GeneralInsurance_TransactionDate as datetime) between '" + dtDate.Value + "' and '" + dtToDate.Value + "'";

            string whereCondGI = " GeneralInsurance_CompanyID='" + HttpContext.Current.Session["LastCompany"].ToString() + "' and GeneralInsurance_FinYear='" + HttpContext.Current.Session["LastFinYear"].ToString() + "'  and   (CAST(GeneralInsurance_TransactionDate AS datetime) >= CONVERT(varchar,'" + startdate + "', 101)) and (CAST(GeneralInsurance_TransactionDate AS datetime) <= CONVERT(varchar,'" + Enddate + "', 101)) ";

            if (rdbbAll.Checked != true)
            {
                if (Branch != "")
                {
                    wherecondition += " and trn_Branch in (" + Branch + ")";
                    whereCondGI += " and GeneralInsurance_BranchID in (" + Branch + ")";
                }
            }
            if (rdbClientA.Checked != true)
            {
                if (Clients != "")
                {
                    wherecondition += " and  trn_ContactId in (" + Clients + ")";
                    whereCondGI += " and GeneralInsurance_ContactID in (" + Clients + ")";
                }
            }
            if (rdbInsuCompA.Checked != true)
            {
                if (InsuComp != "")
                {
                    wherecondition += " and  trn_Scheme in ( select prd_internalId from tbl_master_productsDetails where prd_insurerName in(" + InsuComp + "))";
                    whereCondGI += " and  GeneralInsurance_InsuranceCompany in(" + InsuComp + ")";
                }
            }
            if (rdbProductA.Checked != true)
            {
                if (Products != "")
                {
                    wherecondition += " and  trn_Scheme in (" + Products + ")";
                    whereCondGI += " and  GeneralInsurance_ProductID in(" + Products + ")";
                }
            }
            if (rdbTelecallerA.Checked != true)
            {
                if (TeleCaller != "")
                {
                    wherecondition += " and  trn_Telecaller in (" + TeleCaller + ")";
                    whereCondGI += " and  GeneralInsurance_TeleCaller in(" + TeleCaller + ")";
                }
            }
            if (rdbSalesRepresentativeA.Checked != true)
            {
                if (SaleRep != "")
                {
                    wherecondition += " and  trn_Fos in (" + SaleRep + ")";
                    whereCondGI += " and  GeneralInsurance_SalesRep in(" + SaleRep + ")";
                }
            }
            if (rdbAssociateA.Checked != true)
            {
                if (Associate != "")
                {
                    wherecondition += " and  trn_Referal in (" + Associate + ")";
                    whereCondGI += " and  GeneralInsurance_Associates in(" + Associate + ")";
                }
            }
            if (rdbSubBroakerA.Checked != true)
            {
                if (SubBroker != "")
                {
                    wherecondition += " and  trn_SubBroker in (" + SubBroker + ")";
                    whereCondGI += " and  GeneralInsurance_BrokerFranchise in(" + SubBroker + ")";
                }
            }
            if (drpPolicyStatus.SelectedValue != "A")
            {
                wherecondition += " and  trn_Status in (" + drpPolicyStatus.SelectedValue + ")";
                whereCondGI += " and  GeneralInsurance_PolicyStatus in(" + drpPolicyStatus.SelectedValue + ")";
            }
            if (cmbReportType.SelectedItem.Value == "L")
            {
                DT = oDBEngine.GetDataTable("( select trn_id,trn_Scheme,trn_transMode,trn_BusinessClass,trn_IssueDate,trn_PPT, cast(trn_transDate as datetime) as transDate,convert(varchar(11),cast(trn_transDate as datetime),113) as transDateF,cast(trn_issueDate as datetime) as issueDate,convert(varchar(11),cast(trn_issueDate as datetime),113) as issueDateF, " +
                    " (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_contact where cnt_internalid=trn_contactID) as Customer," +
                    " (select branch_description+'['+branch_code+']' from tbl_master_branch where branch_id=trn_branch) as branch," +
                    " (select prds_description from tbl_master_products where prds_internalId=trn_Scheme) as policy," +
                    " case trn_Status when 0 then 'Business in Hand' when 1 then 'Cancelled' when 2 then 'Canclled from inception' when 3 then 'Cheque Bounced' when 4 then 'Issued' when 5 then 'Lapsed' when 6 then 'Login' else 'N/A' end as status," +
                    " (case trn_transMode when 12 then 'Monthly' when 4 then 'Quartely' when 2 then 'Half Yr.' when 1 then 'Yearly' when 0 then 'One Inst' end) as mode," +
                    " trn_PremiumAmt as premiumAmt," +
                    //" (case trn_transMode when 0 then trn_PremiumAmt*12 when 1 then trn_PremiumAmt*4 when 2 then trn_PremiumAmt*2 when 3 then trn_PremiumAmt when 4 then trn_PremiumAmt else 'N/A' end ) as WAPI," +
                    " (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+  '['+ case when cnt_UCC='' then cnt_shortName else cnt_UCC end + ']'  from tbl_master_contact where cnt_internalid=trn_Telecaller) as TeleCaller," +
                    " (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+ '['+ case when cnt_UCC='' then cnt_shortName else cnt_UCC end + ']' from tbl_master_contact where cnt_internalid=trn_Fos) as SalesRep," +
                    " (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+ '['+ case when cnt_UCC='' then cnt_shortName else cnt_UCC end  + ']' from tbl_master_contact where cnt_internalid=trn_SubBroker) as SubBroker," +
                    " (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+ '['+ case when cnt_UCC='' then cnt_shortName else cnt_UCC end  + ']' from tbl_master_contact where cnt_internalid=trn_Referal) as Associates from  tbl_trans_InsMainUpdate where + " + wherecondition + ") as D", " *, (select dbo.[Insurance_PayoutCommissionCalculation](D.trn_Scheme,D.trn_transMode,D.trn_BusinessClass,D.premiumAmt,'Base',D.trn_IssueDate,D.trn_PPT)) as BaseCommission ", null);

            }
            else if (cmbReportType.SelectedItem.Value == "G")
            {
                DT = oDBEngine.GetDataTable(" [Trans_GeneralInsurance] ", " cast(GeneralInsurance_TransactionDate as datetime) as transDate,convert(varchar(11),cast(GeneralInsurance_TransactionDate as datetime),113) as transDateF,cast(GeneralInsurance_IssueDate as datetime) as issueDate,convert(varchar(11),cast(GeneralInsurance_IssueDate as datetime),113) as issueDateF, " +
                    " (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_contact where cnt_internalid=GeneralInsurance_ContactID) as Customer," +
                    " (select branch_description+'['+branch_code+']' from tbl_master_branch where branch_id=GeneralInsurance_BranchID) as branch," +
                    " (select prds_description from tbl_master_products where prds_internalId=GeneralInsurance_ProductID) as policy," +
                    " case GeneralInsurance_PolicyStatus when 0 then 'Business in Hand' when 1 then 'Cancelled' when 2 then 'Canclled from inception' when 3 then 'Cheque Bounced' when 4 then 'Issued' when 5 then 'Lapsed' when 6 then 'Login' when 'A' then 'All' else 'N/A' end as status," +
                    " '' as mode," +
                    " GeneralInsurance_GrossPremium as premiumAmt," +
                    " '' as WAPI," +
                    " (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_contact where cnt_internalid=GeneralInsurance_TeleCaller) as TeleCaller," +
                    " (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_contact where cnt_internalid=GeneralInsurance_SalesRep) as SalesRep," +
                    " (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_contact where cnt_internalid=GeneralInsurance_BrokerFranchise) as SubBroker," +
                    " (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_contact where cnt_internalid=GeneralInsurance_Associates) as Associates "
                    , whereCondGI);
            }
            else if (cmbReportType.SelectedItem.Value == "B")
            {
                DT = oDBEngine.GetDataTable("( (select cast(trn_transDate as datetime) as transDate,convert(varchar(11),cast(trn_transDate as datetime),113) as transDateF,cast(trn_issueDate as datetime) as issueDate,convert(varchar(11),cast(trn_issueDate as datetime),113) as issueDateF, " +
                    " (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_contact where cnt_internalid=trn_contactID) as Customer," +
                    " (select branch_description+'['+branch_code+']' from tbl_master_branch where branch_id=trn_branch) as branch," +
                    " (select prds_description from tbl_master_products where prds_internalId=trn_Scheme) as policy," +
                    " case trn_Status when 0 then 'Business in Hand' when 1 then 'Cancelled' when 2 then 'Canclled from inception' when 3 then 'Cheque Bounced' when 4 then 'Issued' when 5 then 'Lapsed' when 6 then 'Login' else 'N/A' end as status," +
                    " (case trn_transMode when 0 then 'Monthly' when 1 then 'Quartely' when 2 then 'Half Yr.' when 3 then 'Annual' when 4 then 'One Inst' else 'N/A' end) as mode," +
                    " trn_PremiumAmt as premiumAmt," +
                    " (case trn_transMode when 0 then trn_PremiumAmt*12 when 1 then trn_PremiumAmt*4 when 2 then trn_PremiumAmt*2 when 3 then trn_PremiumAmt when 4 then trn_PremiumAmt else 'N/A' end ) as WAPI," +
                    " (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_contact where cnt_internalid=trn_Telecaller) as TeleCaller," +
                    " (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_contact where cnt_internalid=trn_Fos) as SalesRep," +
                    " (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_contact where cnt_internalid=trn_SubBroker) as SubBroker," +
                    " (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_contact where cnt_internalid=trn_Referal) as Associates from tbl_trans_InsMain where " +
                     wherecondition + ")" +
                     " union all " +
                     " (select cast(GeneralInsurance_TransactionDate as datetime) as transDate,convert(varchar(11),cast(GeneralInsurance_TransactionDate as datetime),113) as transDateF,cast(GeneralInsurance_IssueDate as datetime) as issueDate,convert(varchar(11),cast(GeneralInsurance_IssueDate as datetime),113) as issueDateF, " +
                    " (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_contact where cnt_internalid=GeneralInsurance_ContactID) as Customer," +
                    " (select branch_description+'['+branch_code+']' from tbl_master_branch where branch_id=GeneralInsurance_BranchID) as branch," +
                    " (select prds_description from tbl_master_products where prds_internalId=GeneralInsurance_ProductID) as policy," +
                    " case GeneralInsurance_PolicyStatus when 0 then 'Business in Hand' when 1 then 'Cancelled' when 2 then 'Canclled from inception' when 3 then 'Cheque Bounced' when 4 then 'Issued' when 5 then 'Lapsed' when 6 then 'Login' when 'A' then 'All' else 'N/A' end as status," +
                    " '' as mode," +
                    " GeneralInsurance_GrossPremium as premiumAmt," +
                    " 0 as WAPI," +
                    " (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_contact where cnt_internalid=GeneralInsurance_TeleCaller) as TeleCaller," +
                    " (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_contact where cnt_internalid=GeneralInsurance_SalesRep) as SalesRep," +
                    " (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_contact where cnt_internalid=GeneralInsurance_BrokerFranchise) as SubBroker," +
                    " (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_contact where cnt_internalid=GeneralInsurance_Associates) as Associates from Trans_GeneralInsurance where " +
                     whereCondGI + ")) as D", "*", null);
            }
            if (ViewState["short"].ToString() != "")
            {
                DT.DefaultView.Sort = ViewState["short"].ToString();
            }
            grdCashBankBook.DataSource = DT.DefaultView;
            grdCashBankBook.DataBind();
            ScriptManager.RegisterStartupScript(this, GetType(), "height", "ShowGrid('" + DT.Rows.Count + "')", true);
        }

        protected void NavigationLink_Click(Object sender, CommandEventArgs e)
        {/*
        switch (e.CommandName)
        {
            case "First":
                pageindex = 0;
                break;
            case "Next":
                pageindex = int.Parse(CurrentPage.Value) + 1;
                break;
            case "Prev":
                pageindex = int.Parse(CurrentPage.Value) - 1;
                break;
            case "Last":
                pageindex = int.Parse(TotalPages.Value);
                break;
            default:
                pageindex = int.Parse(e.CommandName.ToString());
                break;
        }
        FillGrid();*/
        }
        //protected void grdCashBankBook_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        string lcVar1 = ((DataRowView)e.Row.DataItem)["premiumAmt"].ToString();
        //        //string lcVar2 = ((DataRowView)e.Row.DataItem)["WAPI"].ToString();
        //        if (lcVar1 == "")
        //        {
        //            lcVar1 = "0";
        //            e.Row.Cells[7].Text = "";
        //        }
        //        else
        //            e.Row.Cells[7].Text = oconverter.getFormattedvalue(decimal.Parse(lcVar1));

        //        //if (lcVar2 == "")
        //        //{
        //        //    lcVar2 = "0";
        //        //    e.Row.Cells[8].Text = "";
        //        //}
        //        //else
        //        //    e.Row.Cells[8].Text = oconverter.getFormattedvalue(decimal.Parse(lcVar2));
        //    }
        //}
        //protected void grdCashBankBook_RowCreated(object sender, GridViewRowEventArgs e)
        //{
        //    string rowID = String.Empty;
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        rowID = "row" + e.Row.RowIndex;
        //        e.Row.Attributes.Add("id", "row" + e.Row.RowIndex);
        //        e.Row.Attributes.Add("onclick", "ChangeRowColor('" + rowID + "'," + DT.Rows.Count + ")");
        //    }

        //}
        //protected void grdCashBankBook_Sorting(object sender, GridViewSortEventArgs e)
        //{
        //    // Check the DataTable itself for the previous sort expression.
        //    if (ViewState["short"].ToString() == (e.SortExpression.ToString() + " ASC"))
        //    {
        //        ViewState["short"] = e.SortExpression + " DESC";
        //    }
        //    else // Handles cases where the previous sort expression was the current expression descending, another expression, or none at all.
        //    {
        //        ViewState["short"] = e.SortExpression + " ASC";
        //    }
        //    FillGrid();
        //}
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string str = "";
            string str1 = "";
            string[] idlist = id.Split('~');
            if (idlist.Length > 1)
            {
                string[] SelectedValue = idlist[1].Split(',');

                for (int i = 0; i < SelectedValue.Length; i++)
                {
                    string[] val = SelectedValue[i].Split(';');
                    if (str == "")
                    {
                        str = "'" + val[0] + "'";
                        str1 = val[0] + ";" + val[1];
                    }
                    else
                    {
                        str += ",'" + val[0] + "'";
                        str1 += "," + val[0] + ";" + val[1];
                    }
                }
            }
            if (idlist[0] == "Branch")
            {
                Branch = str;
                data = "Branch~" + str1;
            }
            else if (idlist[0] == "Clients")
            {
                Clients = str;
                data = "Clients~" + str1;
            }
            else if (idlist[0] == "Ins.Comp")
            {
                InsuComp = str;
                data = "InsuComp~" + str1;
            }
            else if (idlist[0] == "Products")
            {
                Products = str;
                data = "Products~" + str1;
            }
            else if (idlist[0] == "TeleCaller")
            {
                TeleCaller = str;
                data = "TeleCaller~" + str1;
            }
            else if (idlist[0] == "Sales Rep.")
            {
                SaleRep = str;
                data = "SaleRep~" + str1;
            }
            else if (idlist[0] == "Associate")
            {
                Associate = str;
                data = "Associate~" + str1;
            }
            else if (idlist[0] == "Sub Broker")
            {
                SubBroker = str;
                data = "SubBroker~" + str1;
            }

        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }

        public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
        {

        }
        protected void Export_Click(object sender, EventArgs e)
        {
            string wherecondition = " trn_company='" + HttpContext.Current.Session["LastCompany"].ToString() + "' and trn_FinancialYear='" + HttpContext.Current.Session["LastFinYear"].ToString() + "' and cast(trn_TransDate as datetime) between '" + dtDate.Value + "' and '" + dtToDate.Value + "'";

            if (rdbbAll.Checked != true)
            {
                if (Branch != "")
                {
                    wherecondition += " and trn_Branch in (" + Branch + ")";
                }
            }
            if (rdbClientA.Checked != true)
            {
                if (Clients != "")
                {
                    wherecondition += " and  trn_ContactId in (" + Clients + ")";
                }
            }
            if (rdbInsuCompA.Checked != true)
            {
                if (InsuComp != "")
                {
                    wherecondition += " and  trn_Scheme in ( select prd_internalId from tbl_master_productsDetails where prd_insurerName in(" + InsuComp + "))";
                }
            }
            if (rdbProductA.Checked != true)
            {
                if (Products != "")
                {
                    wherecondition += " and  trn_Scheme in (" + Products + ")";
                }
            }
            if (rdbTelecallerA.Checked != true)
            {
                if (TeleCaller != "")
                {
                    wherecondition += " and  trn_Telecaller in (" + TeleCaller + ")";
                }
            }
            if (rdbSalesRepresentativeA.Checked != true)
            {
                if (SaleRep != "")
                {
                    wherecondition += " and  trn_Fos in (" + SaleRep + ")";
                }
            }
            if (rdbAssociateA.Checked != true)
            {
                if (Associate != "")
                {
                    wherecondition += " and  trn_Referal in (" + Associate + ")";
                }
            }
            if (rdbSubBroakerA.Checked != true)
            {
                if (SubBroker != "")
                {
                    wherecondition += " and  trn_SubBroker in (" + SubBroker + ")";
                }
            }
            if (drpPolicyStatus.SelectedValue != "A")
            {
                wherecondition += " and  trn_Status in (" + drpPolicyStatus.SelectedValue + ")";
            }
            string query = " select convert(varchar(11),cast(trn_transDate as datetime),113) as 'Transaction Date',convert(varchar(11),cast(trn_issueDate as datetime),113) as 'Issue Date', " +
                " (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_contact where cnt_internalid=trn_contactID) as Customer," +
                " (select branch_description+'['+branch_code+']' from tbl_master_branch where branch_id=trn_branch) as Branch," +
                " (select prds_description from tbl_master_products where prds_internalId=trn_Scheme) as Policy," +
                " case trn_Status when 0 then 'Business in Hand' when 1 then 'Cancelled' when 2 then 'Canclled from inception' when 3 then 'Cheque Bounced' when 4 then 'Issued' when 5 then 'Lapsed' when 6 then 'Login' else 'N/A' end as Status," +
                " (case trn_transMode when 0 then 'Monthly' when 1 then 'Quartely' when 2 then 'Half Yr.' when 3 then 'Annual' when 4 then 'One Inst' else 'N/A' end) as 'Transaction Mode'," +
                " trn_PremiumAmt as 'Premium Amount'," +
                " (case trn_transMode when 0 then trn_PremiumAmt*12 when 1 then trn_PremiumAmt*4 when 2 then trn_PremiumAmt*2 when 3 then trn_PremiumAmt when 4 then trn_PremiumAmt else 'N/A' end ) as WAPI," +
                " (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_contact where cnt_internalid=trn_Telecaller) as 'Tele Caller'," +
                " (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_contact where cnt_internalid=trn_Fos) as 'Sales Representative'," +
                " (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_contact where cnt_internalid=trn_SubBroker) as 'Sub Broker'," +
                " (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_contact where cnt_internalid=trn_Referal) as Associates " +
                " from tbl_trans_InsMain where " + wherecondition;
            oconverter.ExportToExcel(query, "TransactionDetailTotal");
        }
        protected void grdCashBankBook_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdCashBankBook.PageIndex = e.NewPageIndex;
            FillGrid();
        }
    }
}