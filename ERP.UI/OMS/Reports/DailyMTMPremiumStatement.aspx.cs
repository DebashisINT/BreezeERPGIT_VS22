using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Reports
{

    public partial class Reports_DailyMTMPremiumStatement : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.Reports oReports = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        ExcelFile objExcel = new ExcelFile();
        DataTable dtExport = new DataTable();
        string data;
        DataTable Distinctclient = new DataTable();
        string strGrandTotal = "";
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
            if (!IsPostBack)
            {

                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load('" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "');</script>");
                date();
                chkboxliststyle();
            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//

        }
        public int CurrentPage
        {

            get
            {
                if (this.ViewState["CurrentPage"] == null)
                    return 0;
                else
                    return Convert.ToInt16(this.ViewState["CurrentPage"].ToString());
            }

            set
            {
                this.ViewState["CurrentPage"] = value;
            }

        }

        public int LastPage
        {
            get
            {
                if (this.ViewState["LastPage"] == null)
                    return 0;
                else
                    return Convert.ToInt16(this.ViewState["LastPage"].ToString());
            }
            set
            {
                this.ViewState["LastPage"] = value;
            }

        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] idlist = id.Split('~');
            string[] cl = idlist[1].Split(',');
            string str = "";
            string str1 = "";
            for (int i = 0; i < cl.Length; i++)
            {
                if (idlist[0].ToString().Trim() == "Clients" || idlist[0].ToString().Trim() == "Expiry")
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
                else
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

            }

            if (idlist[0] == "Clients")
            {
                data = "Clients~" + str;
            }
            else if (idlist[0] == "Product")
            {
                data = "Product~" + str;
            }
            else if (idlist[0] == "Group")
            {
                data = "Group~" + str;
            }
            else if (idlist[0] == "Branch")
            {
                data = "Branch~" + str;
            }
            else if (idlist[0] == "BranchGroup")
            {
                data = "BranchGroup~" + str;
            }
            else if (idlist[0] == "Expiry")
            {
                data = "Expiry~" + str;
            }
            else if (idlist[0] == "MAILEMPLOYEE")
            {
                data = "MAILEMPLOYEE~" + str;
            }


        }
        void date()
        {
            DtFrom.EditFormatString = oconverter.GetDateFormat("Date");
            DtFrom.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            DtTo.EditFormatString = oconverter.GetDateFormat("Date");
            //DtTo.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            DtTo.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());

        }
        void chkboxliststyle()
        {
            foreach (ListItem item in chktfilterDetail.Items)
            {
                item.Attributes.Add("style", "font-family:Times New Roman;color:#461B7E;font-size:9px");
            }
            foreach (ListItem item in chktfilterSummary.Items)
            {
                item.Attributes.Add("style", "font-family:Times New Roman;color:#461B7E;font-size:9px");
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
        protected void btnScreen_Click(object sender, EventArgs e)
        {

            FilterColumnCheck();


        }
        void FilterColumnCheck()
        {
            string BfQtyFilter = "N";
            string BfPriceFilter = "N";
            string BuyQtyFilter = "N";
            string BuyValueFilter = "N";
            string SellQtyFilter = "N";
            string SellValueFilter = "N";
            string CfQtyFilter = "N";
            string CfPriceFilter = "N";
            string MTMFilter = "N";
            string PremiumFilter = "N";
            string AsnExcFilter = "N";
            string FinSettFilter = "N";
            string TranChargeFilter = "N";
            string StampDutyFilter = "N";
            string TotalServTaxFilter = "N";
            string SebiFeeFilter = "N";
            string STTFilter = "N";
            string NetObligationFilter = "N";
            string OtherChargeFilter = "N";
            int FilterCount = 0;

            if (DLLRptView.SelectedItem.Value.ToString().Trim() == "9" || DLLRptView.SelectedItem.Value.ToString().Trim() == "1" || DLLRptView.SelectedItem.Value.ToString().Trim() == "4")
            {/////////For Summary

                foreach (ListItem listitem in chktfilterSummary.Items)
                {
                    if (listitem.Selected)
                    {
                        if (listitem.Value == "MTM P/L")
                        {
                            MTMFilter = "Y";
                            FilterCount = FilterCount + 1;
                        }
                        if (listitem.Value == "Premium")
                        {
                            PremiumFilter = "Y";
                            FilterCount = FilterCount + 1;
                        }
                        if (listitem.Value == "Asn/Exc")
                        {
                            AsnExcFilter = "Y";
                            FilterCount = FilterCount + 1;
                        }
                        if (listitem.Value == "Fin.Sett")
                        {
                            FinSettFilter = "Y";
                            FilterCount = FilterCount + 1;
                        }
                        if (listitem.Value == "TranCharge")
                        {
                            TranChargeFilter = "Y";
                            FilterCount = FilterCount + 1;
                        }
                        if (listitem.Value == "StampDuty")
                        {
                            StampDutyFilter = "Y";
                            FilterCount = FilterCount + 1;
                        }
                        if (listitem.Value == "TotalServTax")
                        {
                            TotalServTaxFilter = "Y";
                            FilterCount = FilterCount + 1;
                        }
                        if (listitem.Value == "SebiFee")
                        {
                            SebiFeeFilter = "Y";
                            FilterCount = FilterCount + 1;
                        }
                        if (listitem.Value == "STT")
                        {
                            STTFilter = "Y";
                            FilterCount = FilterCount + 1;
                        }
                        if (listitem.Value == "NetObligation")
                        {
                            NetObligationFilter = "Y";
                            FilterCount = FilterCount + 1;
                        }
                        if (listitem.Value == "OtherCharge")
                        {
                            OtherChargeFilter = "Y";
                            FilterCount = FilterCount + 1;
                        }

                    }
                }
            }
            else if (DLLRptView.SelectedItem.Value.ToString().Trim() == "11" || DLLRptView.SelectedItem.Value.ToString().Trim() == "3" || DLLRptView.SelectedItem.Value.ToString().Trim() == "6")
            {  /////////For Detail
                foreach (ListItem listitem in chktfilterDetail.Items)
                {
                    if (listitem.Selected)
                    {
                        if (listitem.Value == "B/f Qty")
                        {
                            BfQtyFilter = "Y";
                            FilterCount = FilterCount + 1;
                        }
                        if (listitem.Value == "B/F Price")
                        {
                            BfPriceFilter = "Y";
                            FilterCount = FilterCount + 1;
                        }
                        if (listitem.Value == "Buy Qty")
                        {
                            BuyQtyFilter = "Y";
                            FilterCount = FilterCount + 1;
                        }
                        if (listitem.Value == "Buy Value")
                        {
                            BuyValueFilter = "Y";
                            FilterCount = FilterCount + 1;
                        }
                        if (listitem.Value == "Sell Qty")
                        {
                            SellQtyFilter = "Y";
                            FilterCount = FilterCount + 1;
                        }
                        if (listitem.Value == "Sell Value")
                        {
                            SellValueFilter = "Y";
                            FilterCount = FilterCount + 1;
                        }
                        if (listitem.Value == "C/f Qty")
                        {
                            CfQtyFilter = "Y";
                            FilterCount = FilterCount + 1;
                        }
                        if (listitem.Value == "C/f Price")
                        {
                            CfPriceFilter = "Y";
                            FilterCount = FilterCount + 1;
                        }
                        if (listitem.Value == "MTM P/L")
                        {
                            MTMFilter = "Y";
                            FilterCount = FilterCount + 1;
                        }
                        if (listitem.Value == "Premium")
                        {
                            PremiumFilter = "Y";
                            FilterCount = FilterCount + 1;
                        }
                        if (listitem.Value == "Asn/Exc")
                        {
                            AsnExcFilter = "Y";
                            FilterCount = FilterCount + 1;
                        }
                        if (listitem.Value == "Fin.Sett")
                        {
                            FinSettFilter = "Y";
                            FilterCount = FilterCount + 1;
                        }
                    }
                }

            }
            else
            {  /////////For Detail
                if (DLLRptView.SelectedItem.Value.ToString().Trim() != "7" && DLLRptView.SelectedItem.Value.ToString().Trim() != "8" && DLLRptView.SelectedItem.Value.ToString().Trim() != "10")
                {
                    foreach (ListItem listitem in ChkfilterDetailProduct.Items)
                    {
                        if (listitem.Selected)
                        {
                            if (listitem.Value == "MTM P/L")
                            {
                                MTMFilter = "Y";
                                FilterCount = FilterCount + 1;
                            }
                            if (listitem.Value == "Premium")
                            {
                                PremiumFilter = "Y";
                                FilterCount = FilterCount + 1;
                            }
                            if (listitem.Value == "Asn/Exc")
                            {
                                AsnExcFilter = "Y";
                                FilterCount = FilterCount + 1;
                            }
                            if (listitem.Value == "Fin.Sett")
                            {
                                FinSettFilter = "Y";
                                FilterCount = FilterCount + 1;
                            }
                        }
                    }

                }
            }
            if (DLLRptView.SelectedItem.Value.ToString().Trim() == "7" || DLLRptView.SelectedItem.Value.ToString().Trim() == "8" || DLLRptView.SelectedItem.Value.ToString().Trim() == "10")
            {
                FilterCount = 10;
            }
            if (Convert.ToInt32(FilterCount.ToString().Trim()) >= 3)
            {
                ///////SP Call
                procedure(BfQtyFilter, BfPriceFilter, BuyQtyFilter, BuyValueFilter, SellQtyFilter,
                          SellValueFilter, CfQtyFilter, CfPriceFilter,
                          MTMFilter, PremiumFilter, AsnExcFilter, FinSettFilter,
                          TranChargeFilter, StampDutyFilter, TotalServTaxFilter, SebiFeeFilter,
                          NetObligationFilter, OtherChargeFilter, STTFilter);

                ds = (DataSet)ViewState["dataset"];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (DLLRptView.SelectedItem.Value.ToString().Trim() == "7" || DLLRptView.SelectedItem.Value.ToString().Trim() == "8" || DLLRptView.SelectedItem.Value.ToString().Trim() == "9" || DLLRptView.SelectedItem.Value.ToString().Trim() == "10" || DLLRptView.SelectedItem.Value.ToString().Trim() == "12")
                    {
                        if (ddlGeneration.SelectedItem.Value == "2")
                        {
                            export(ds);
                        }
                        else
                        {
                            email(ds);
                        }
                    }
                    else
                    {
                        if (ddlGeneration.SelectedItem.Value.ToString() == "1")///Screen
                        {
                            ddlbandforClient(ds);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordDisplay", "RecordDisplay();", true);
                        }
                        if (ddlGeneration.SelectedItem.Value.ToString() == "2")///Export
                        {
                            export(ds);
                        }
                        if (ddlGeneration.SelectedItem.Value.ToString() == "3")////Email
                        {
                            email(ds);
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('1');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('6');", true);
            }
        }
        void procedure(string BfQtyFilter, string BfPriceFilter,
                        string BuyQtyFilter, string BuyValueFilter, string SellQtyFilter,
                        string SellValueFilter, string CfQtyFilter, string CfPriceFilter,
                        string MTMFilter, string PremiumFilter, string AsnExcFilter,
                        string FinSettFilter, string TranChargeFilter, string StampDutyFilter,
                        string TotalServTaxFilter, string SebiFeeFilter, string NetObligationFilter, string OtherChargeFilter, string STTFilter)
        {
            string usermail = string.Empty;
            string ClientId = string.Empty;
            string Asset = string.Empty;
            string Expiry = string.Empty;
            string GrpType = string.Empty;
            string GrpId = string.Empty;
            string CalType = string.Empty;
            if (ddlGeneration.SelectedItem.Value == "3")
            {
                usermail = Convert.ToString(ddloptionformail.SelectedItem.Text);
            }
            else
            {
                usermail = "N";
            }



            if (rdbClientALL.Checked)
            {
                ClientId = "ALL";
            }
            else
            {
                ClientId = Convert.ToString(HiddenField_Client.Value);
            }

            if (rdbAssetAll.Checked)
            {
                Asset = "ALL";
            }
            else
            {
                Asset = Convert.ToString(HiddenField_Product.Value);
            }
            if (rdbExpiryAll.Checked)
            {
                Expiry = "ALL";
            }
            else
            {
                Expiry = Convert.ToString(HiddenField_Expiry.Value);
            }

            if (ddlGroup.SelectedItem.Value.ToString() == "0")
            {
                GrpType = "BRANCH";
                if (rdbranchAll.Checked)
                {
                    GrpId = "ALL";
                }
                else
                {
                    GrpId = Convert.ToString(HiddenField_Branch.Value);
                }
            }
            else if (ddlGroup.SelectedItem.Value.ToString() == "2")
            {
                GrpType = "BRANCHGROUP";
                if (rdbranchAll.Checked)
                {
                    GrpId = "ALL";
                }
                else
                {
                    GrpId = Convert.ToString(HiddenField_BranchGroup.Value);
                }
            }
            else
            {
                GrpType = Convert.ToString(ddlgrouptype.SelectedItem.Text);
                if (rdddlgrouptypeAll.Checked)
                {
                    GrpId = "ALL";
                }
                else
                {
                    GrpId = Convert.ToString(HiddenField_Group.Value);
                }
            }

            if (DLLRptView.SelectedItem.Value.ToString().Trim() == "4" || DLLRptView.SelectedItem.Value.ToString().Trim() == "5" || DLLRptView.SelectedItem.Value.ToString().Trim() == "6")
            {
                if (Session["ExchangeSegmentID"].ToString().Trim() == "2" || Session["ExchangeSegmentID"].ToString().Trim() == "5" || Session["ExchangeSegmentID"].ToString().Trim() == "20")
                    CalType = "NS%";
                else
                    CalType = "MC%";
            }
            else
            {
                CalType = "CL%";
            }
            if (Session["ExchangeSegmentID"].ToString().Trim() == "2" || Session["ExchangeSegmentID"].ToString().Trim() == "5" || Session["ExchangeSegmentID"].ToString().Trim() == "20")
            {
                ds = oReports.Report_DailyMTMPremiumStatement(
                   usermail,
                     Convert.ToString(ddlGeneration.SelectedItem.Value),
                     Convert.ToString(DtFrom.Value),
                     Convert.ToString(DtTo.Value),
                    ClientId,
                     Convert.ToString(Session["LastFinYear"]),
                    Asset,
                     Expiry,
                     Convert.ToString(HttpContext.Current.Session["usersegid"]),
                     Convert.ToString(Session["LastCompany"]),
                     GrpType,
                     GrpId,
                     Convert.ToString(Session["userbranchHierarchy"]),
                     Convert.ToString(DLLRptView.SelectedItem.Value),
                     CalType,
                    BfQtyFilter.ToString().Trim(),
                    BfPriceFilter.ToString().Trim(),
                   BuyQtyFilter.ToString().Trim(),
                   BuyValueFilter.ToString().Trim(),
                   SellQtyFilter.ToString().Trim(),
                    SellValueFilter.ToString().Trim(),
                   CfQtyFilter.ToString().Trim(),
                   CfPriceFilter.ToString().Trim(),
                   MTMFilter.ToString().Trim(),
                   PremiumFilter.ToString().Trim(),
                   AsnExcFilter.ToString().Trim(),
                    FinSettFilter.ToString().Trim(),
                   TranChargeFilter.ToString().Trim(),
                   StampDutyFilter.ToString().Trim(),
                 TotalServTaxFilter.ToString().Trim(),
                  SebiFeeFilter.ToString().Trim(),
                   NetObligationFilter.ToString().Trim(),
                  NetObligationFilter.ToString().Trim(),
                   STTFilter.ToString().Trim()
                    );
            }
            else
            {
                ds = oReports.Report_DailyMTMPremiumStatementComm(
                     usermail,
                     Convert.ToString(ddlGeneration.SelectedItem.Value),
                     Convert.ToString(DtFrom.Value),
                     Convert.ToString(DtTo.Value),
                    ClientId,
                     Convert.ToString(Session["LastFinYear"]),
                    Asset,
                     Expiry,
                     Convert.ToString(HttpContext.Current.Session["usersegid"]),
                     Convert.ToString(Session["LastCompany"]),
                     GrpType,
                     GrpId,
                     Convert.ToString(Session["userbranchHierarchy"]),
                     Convert.ToString(DLLRptView.SelectedItem.Value),
                     CalType,
                    BfQtyFilter.ToString().Trim(),
                    BfPriceFilter.ToString().Trim(),
                   BuyQtyFilter.ToString().Trim(),
                   BuyValueFilter.ToString().Trim(),
                   SellQtyFilter.ToString().Trim(),
                    SellValueFilter.ToString().Trim(),
                   CfQtyFilter.ToString().Trim(),
                   CfPriceFilter.ToString().Trim(),
                   MTMFilter.ToString().Trim(),
                   PremiumFilter.ToString().Trim(),
                   AsnExcFilter.ToString().Trim(),
                    FinSettFilter.ToString().Trim(),
                   TranChargeFilter.ToString().Trim(),
                   StampDutyFilter.ToString().Trim(),
                 TotalServTaxFilter.ToString().Trim(),
                  SebiFeeFilter.ToString().Trim(),
                   NetObligationFilter.ToString().Trim(),
                  NetObligationFilter.ToString().Trim(),
                   STTFilter.ToString().Trim());

            }
            ViewState["dataset"] = ds;


        }

        void FnHtml(DataSet ds, string clientid)
        {
            //////////For header
            String strHtmlheader = String.Empty;
            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }
            str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report Period " + oconverter.ArrangeDate2(DtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(DtTo.Value.ToString());
            str = str + " ; Report View : " + DLLRptView.SelectedItem.Text.ToString().Trim();

            strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtmlheader += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\">" + str + "</td></tr></table>";

            ////////Detail Display
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

            DataView viewclient = new DataView();
            viewclient = ds.Tables[0].DefaultView;
            if (ddlGeneration.SelectedValue == "1")
                viewclient.RowFilter = "(CustomerId='" + clientid.ToString().Trim() + "' or CustomerId='ZZZ')";
            else
                viewclient.RowFilter = "(CustomerId='" + clientid.ToString().Trim() + "')";
            DataTable dt = new DataTable();
            dt = viewclient.ToTable();

            dt.Columns.Remove("CustomerId");
            if (ddlGeneration.SelectedItem.Value == "3")
            {
                if ((DLLRptView.SelectedItem.Value == "7") || (DLLRptView.SelectedItem.Value == "8") || (DLLRptView.SelectedItem.Value == "9") || (DLLRptView.SelectedItem.Value == "10"))
                {
                    if (ddloptionformail.SelectedItem.Text != "User")
                    {
                        dt.Columns.Remove("Grpid");
                        dt.Columns.Remove("GrpName");
                    }
                }
            }

            //////////////TABLE HEADER BIND
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + dt.Columns[i].ColumnName + "</b></td>";
            }
            strHtml += "</tr>";

            int flag = 0;
            foreach (DataRow dr1 in dt.Rows)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dr1[j] != DBNull.Value)
                    {
                        if (dr1[j].ToString().Trim().StartsWith("Client Name") || dr1[j].ToString().Trim().StartsWith("Grand Total") || dr1[j].ToString().Trim().StartsWith("Exchange"))
                        {
                            strHtml += "</tr>";
                            strHtml += "<tr style=\"text-align:left\">";
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                        }
                        else if (dr1[j].ToString().Trim().StartsWith("Scrip Name"))
                        {
                            strHtml += "</tr>";
                            strHtml += "<tr style=\"background-color:#D2B9D3 ;text-align:left\">";
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                        }
                        else
                        {
                            if (IsNumeric(dr1[j].ToString()) == true)
                            {
                                strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                            {
                                strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                        }
                    }
                    else
                    {
                        strHtml += "<td>&nbsp;</td>";
                    }
                }

                strHtml += "</tr>";
            }
            if (ddlGeneration.SelectedValue == "3" && ddloptionformail.SelectedItem.Text == "User")
            {

                if (cmbclient.Items[cmbclient.Items.Count - 1].Value == clientid)
                {
                    strHtml = strHtml + strGrandTotal;

                }

            }

            strHtml += "</table>";
            if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "1")
            {
                DivHeader.InnerHtml = strHtmlheader;
                Divdisplay.InnerHtml = strHtml;
            }
            else
            {
                ViewState["mail"] = strHtmlheader + strHtml;
            }



        }
        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }
        public static bool IsNumeric(object value)
        {
            double dbl;
            return double.TryParse(value.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out dbl);
        }
        void ddlbandforClient(DataSet ds)
        {
            DataView viewData = new DataView();
            viewData = ds.Tables[0].DefaultView;
            viewData.RowFilter = " CustomerId<>'ZZZ'";
            DataTable dt = new DataTable();
            dt = viewData.ToTable();

            Distinctclient = new DataTable();
            DataView viewClient = new DataView(dt);
            Distinctclient = viewClient.ToTable(true, new string[] { "CustomerId" });

            if (Distinctclient.Rows.Count > 0)
            {
                cmbclient.DataSource = Distinctclient;
                cmbclient.DataValueField = "CustomerId";
                cmbclient.DataTextField = "CustomerId";
                cmbclient.DataBind();

            }
            ViewState["clients"] = Distinctclient;
            LastPage = Distinctclient.Rows.Count - 1;
            CurrentPage = 0;
            bind_Details();
        }
        void bind_Details()
        {
            Distinctclient = (DataTable)ViewState["clients"];
            cmbclient.SelectedIndex = CurrentPage;
            if (LastPage > -1)
            {
                listRecord.Text = CurrentPage + 1 + " of " + Distinctclient.Rows.Count.ToString() + " Record.";

            }
            ds = (DataSet)ViewState["dataset"];
            FnHtml(ds, cmbclient.SelectedItem.Value.ToString());
            ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordDisplay", "RecordDisplay();", true);
            ShowHidePreviousNext_of_Clients();
        }
        void ShowHidePreviousNext_of_Clients()
        {
            if (LastPage == 0 || LastPage == -1)
            {
                ASPxFirst.Style["Display"] = "none";
                ASPxPrevious.Style["Display"] = "none";
                ASPxNext.Style["Display"] = "none";
                ASPxLast.Style["Display"] = "none";

            }
            else
            {
                ASPxFirst.Style["Display"] = "Display";
                ASPxPrevious.Style["Display"] = "Display";
                ASPxNext.Style["Display"] = "Display";
                ASPxLast.Style["Display"] = "Display";

            }

            if (CurrentPage == LastPage && LastPage != 0)
            {

                ASPxFirst.Enabled = true;
                ASPxPrevious.Enabled = true;

                ASPxNext.Enabled = false;
                ASPxLast.Enabled = false;

            }
            else
                if (CurrentPage == 0 && LastPage != 0)
                {
                    ASPxFirst.Enabled = false;
                    ASPxPrevious.Enabled = false;

                    ASPxNext.Enabled = true;
                    ASPxLast.Enabled = true;


                }
                else
                {
                    ASPxFirst.Enabled = true;
                    ASPxPrevious.Enabled = true;
                    ASPxNext.Enabled = true;
                    ASPxLast.Enabled = true;
                }
        }
        protected void ASPxFirst_Click(object sender, EventArgs e)
        {
            hiddencount.Value = "0";
            CurrentPage = 0;
            bind_Details();
        }
        protected void ASPxPrevious_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 0)
            {
                hiddencount.Value = "0";
                CurrentPage = CurrentPage - 1;
                bind_Details();
            }
        }
        protected void ASPxNext_Click(object sender, EventArgs e)
        {
            if (CurrentPage < LastPage)
            {
                hiddencount.Value = "0";
                CurrentPage = CurrentPage + 1;
                bind_Details();
            }
        }
        protected void ASPxLast_Click(object sender, EventArgs e)
        {
            hiddencount.Value = "0";
            CurrentPage = LastPage;
            bind_Details();
        }
        void export(DataSet ds)
        {
            DataTable dtExport = ds.Tables[0].Copy();
            if (DLLRptView.SelectedItem.Value.ToString().Trim() != "7" && DLLRptView.SelectedItem.Value.ToString().Trim() != "8" && DLLRptView.SelectedItem.Value.ToString().Trim() != "10")
            {
                dtExport.Columns.Remove("CustomerId");
                //dtExport.Columns.Remove("GrpName");
                //dtExport.Columns.Remove("Grpid");
            }

            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }
            str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report Period " + oconverter.ArrangeDate2(DtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(DtTo.Value.ToString());
            str = str + "Report View : " + DLLRptView.SelectedItem.Text.ToString().Trim();


            DataTable dtReportHeader = new DataTable();
            DataTable dtReportFooter = new DataTable();

            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            DrRowR1[0] = str.ToString().Trim();
            dtReportHeader.Rows.Add(DrRowR1);


            DataRow HeaderRow1 = dtReportHeader.NewRow();
            dtReportHeader.Rows.Add(HeaderRow1);
            DataRow HeaderRow2 = dtReportHeader.NewRow();
            dtReportHeader.Rows.Add(HeaderRow2);

            dtReportFooter.Columns.Add(new DataColumn("Footer", typeof(String))); //0
            DataRow FooterRow1 = dtReportFooter.NewRow();
            dtReportFooter.Rows.Add(FooterRow1);
            DataRow FooterRow2 = dtReportFooter.NewRow();
            dtReportFooter.Rows.Add(FooterRow2);
            DataRow FooterRow = dtReportFooter.NewRow();
            FooterRow[0] = "* * *  End Of Report * * *   ";
            dtReportFooter.Rows.Add(FooterRow);

            objExcel.ExportToExcelforExcel(dtExport, "Daily MTM/Premium Statement", "Client Total", dtReportHeader, dtReportFooter);

        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            FilterColumnCheck();

        }
        protected void btnSendmail_Click(object sender, EventArgs e)
        {
            FilterColumnCheck();
        }

        void email(DataSet ds)
        {
            string Date = oconverter.ArrangeDate2(DtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(DtTo.Value.ToString());
            if (ddloptionformail.SelectedItem.Text == "User")
            {

                ViewState["mail"] = "";
                ViewState["Allmail"] = "";

                DataView viewData = new DataView();
                viewData = ds.Tables[0].DefaultView;
                viewData.RowFilter = " CustomerId<>'ZZZ'";
                DataTable dt = new DataTable();
                dt = viewData.ToTable();

                Distinctclient = new DataTable();
                DataView viewClient = new DataView(dt);
                Distinctclient = viewClient.ToTable(true, new string[] { "CustomerId" });

                if (Distinctclient.Rows.Count > 0)
                {
                    cmbclient.DataSource = Distinctclient;
                    cmbclient.DataValueField = "CustomerId";
                    cmbclient.DataTextField = "CustomerId";
                    cmbclient.DataBind();

                }

                ////for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                ////{
                ////    FnHtml(ds, cmbclient.SelectedItem.Value.ToString());
                ////    ViewState["Allmail"] = ViewState["Allmail"].ToString().Trim() + ViewState["mail"].ToString().Trim();
                ////}
                //**

                DataView viewGrand = new DataView();
                viewGrand = ds.Tables[0].DefaultView;
                viewGrand.RowFilter = " CustomerId='ZZZ'";
                DataTable dtGrand = new DataTable();
                dtGrand = viewData.ToTable();

                if (dtGrand.Columns[0].ColumnName == "CustomerId")
                    dtGrand.Columns.Remove("CustomerId");
                if (ddloptionformail.SelectedItem.Text != "User")
                {
                    if (ddlGeneration.SelectedItem.Value == "3")
                    {

                        dtGrand.Columns.Remove("Grpid");
                        dtGrand.Columns.Remove("Grpname");
                    }
                }
                ////
                //DataRow[] drGrand= ds.Tables[0].Select("CustomerId='ZZZ'");

                if (dtGrand.Rows.Count > 0)
                {
                    DataRow dr1 = dtGrand.Rows[0];

                    for (int j = 0; j < dr1.ItemArray.Length; j++)
                    {
                        if (dr1[j] != DBNull.Value)
                        {
                            if (dr1[j].ToString().Trim().StartsWith("Grand Total"))
                            {
                                //strGrandTotal = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                                strGrandTotal += "<tr style=\"text-align:left\">";
                                strGrandTotal += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap><b>" + dr1[j] + "</b></td>";
                            }
                            else
                            {
                                if (IsNumeric(dr1[j].ToString()) == true)
                                {
                                    strGrandTotal += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap>" + dr1[j] + "</td>";

                                }
                                else
                                {
                                    strGrandTotal += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap>" + dr1[j] + "</td>";

                                }
                            }
                        }
                        else
                        {
                            strGrandTotal += "<td>&nbsp;</td>";
                        }

                    }
                    strGrandTotal += "</tr>";
                }



                //**

                for (int i = 0; i < cmbclient.Items.Count; i++)
                {
                    FnHtml(ds, cmbclient.Items[i].Value);
                    ViewState["Allmail"] = ViewState["Allmail"].ToString().Trim() + ViewState["mail"].ToString().Trim();
                }


                ////

                ////DataView viewGrand = new DataView();
                ////viewGrand = ds.Tables[0].DefaultView;
                ////viewGrand.RowFilter = " CustomerId='ZZZ'";
                ////DataTable dtGrand = new DataTable();
                ////dtGrand = viewData.ToTable();

                ////if (dtGrand.Columns[0].ColumnName == "CustomerId")
                ////    dtGrand.Columns.Remove("CustomerId");
                ////////
                //////DataRow[] drGrand= ds.Tables[0].Select("CustomerId='ZZZ'");

                ////if (dtGrand.Rows.Count > 0)
                ////{
                ////    DataRow dr1 = dtGrand.Rows[0];                       

                ////   for (int j = 0; j < dr1.ItemArray.Length; j++)
                ////    {
                ////        if (dr1[j] != DBNull.Value)
                ////        {
                ////            if ( dr1[j].ToString().Trim().StartsWith("Grand Total"))
                ////            {
                ////                strGrandTotal = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                ////                strGrandTotal += "<tr style=\"text-align:left\">";
                ////                strGrandTotal += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap><b>" + dr1[j] + "</b></td>";
                ////            }
                ////            else
                ////            {
                ////                if (IsNumeric(dr1[j].ToString()) == true)
                ////                {
                ////                    strGrandTotal += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap>" + dr1[j] + "</td>";

                ////                }
                ////                else
                ////                {
                ////                    strGrandTotal += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap>" + dr1[j] + "</td>";

                ////                }
                ////            }
                ////        }
                ////        else
                ////        {
                ////            strGrandTotal += "<td>&nbsp;</td>";
                ////        }

                ////    }
                ////    strGrandTotal += "</table>";
                ////}
                //// ViewState["Allmail"] = ViewState["Allmail"].ToString() + strGrandTotal;
                string[] clnt = HiddenField_emmail.Value.ToString().Split(',');


                for (int i = 0; i < clnt.Length; i++)
                {
                    if (oDBEngine.SendReportSt(ViewState["Allmail"].ToString().Trim(), clnt[i].ToString().Trim(), Date.ToString().Trim(), "Daily MTM/Premium Statement [" + Date.ToString().Trim() + "]") == true)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('4');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('5');", true);

                    }
                }
            }
            else if (ddloptionformail.SelectedItem.Text == "Group/Branch")
            {
                DataView viewData = new DataView();
                viewData = ds.Tables[0].DefaultView;
                viewData.RowFilter = " grpid <>'abcd' ";
                DataTable dtclient = new DataTable();
                dtclient = viewData.ToTable();

                Distinctclient = new DataTable();
                DataView viewClient = new DataView(dtclient);
                Distinctclient = viewClient.ToTable(true, new string[] { "grpid", "Grpname" });


                if (Distinctclient.Rows.Count > 0)
                {
                    cmbclient.DataSource = Distinctclient;
                    cmbclient.DataValueField = "grpid";
                    cmbclient.DataTextField = "Grpname";
                    cmbclient.DataBind();

                }
                for (int i = 0; i < cmbclient.Items.Count; i++)
                {
                    DataView viewsingleclient = new DataView();
                    viewsingleclient = ds.Tables[0].DefaultView;
                    viewsingleclient.RowFilter = "(grpid='" + cmbclient.Items[i].Value + "')";
                    DataTable dt = new DataTable();
                    dt = viewsingleclient.ToTable();
                    dt.Columns.Remove("grpid");
                    String strHtml = String.Empty;
                    String strHtmlheader = String.Empty;
                    string strFooter = string.Empty;
                    int flag = 0;
                    // strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                    if (DLLRptView.SelectedItem.Value == "7" || DLLRptView.SelectedItem.Value == "8" || DLLRptView.SelectedItem.Value == "10")
                    {
                        dt.Columns.Remove("GrpName");
                        strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                        for (int cname = 0; cname < dt.Columns.Count; cname++)
                        {
                            strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + dt.Columns[cname].ColumnName + "</b></td>";
                        }
                        strHtml += "</tr>";


                        foreach (DataRow dr1 in dt.Rows)
                        {
                            flag = flag + 1;
                            strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                            for (int j = 0; j < dt.Columns.Count; j++)
                            {
                                if (dr1[j] != DBNull.Value)
                                {
                                    if ((dr1[j].ToString().Trim().StartsWith("Group :")) || (dr1[j].ToString().Trim().StartsWith("Branch :")))
                                    {
                                        strHtml += "</tr>";
                                        strHtml += "<tr style=\"background-color:lavender ;text-align:left\">";
                                        strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                                    }
                                    else if (dr1[j].ToString().Trim().StartsWith("Test"))
                                    {
                                        strHtml += "<td>&nbsp;</td>";
                                    }
                                    else
                                    {
                                        if (IsNumeric(dr1[j].ToString()) == true)
                                        {
                                            strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                                        }
                                        else
                                        {
                                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                                        }
                                    }
                                }
                                else
                                {
                                    strHtml += "<td>&nbsp;</td>";
                                }
                            }

                            strHtml += "</tr>";
                        }
                        strHtml = strHtmlheader + strHtml + strFooter + "</table>";
                    }
                    else
                    {
                        dt.Columns.Remove("CustomerId");
                        dt.Columns.Remove("Grpname");

                        strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                        for (int p = 0; p < dt.Columns.Count; p++)
                        {
                            strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + dt.Columns[p].ColumnName + "</b></td>";
                        }
                        strHtml += "</tr>";


                        foreach (DataRow dr1 in dt.Rows)
                        {

                            flag = flag + 1;
                            strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                            for (int j = 0; j < dt.Columns.Count; j++)
                            {

                                if (dr1[j] != DBNull.Value)
                                {
                                    if (dr1[j].ToString().Trim().StartsWith("Client Name") || dr1[j].ToString().Trim().StartsWith("Grand Total") || dr1[j].ToString().Trim().StartsWith("Exchange"))
                                    {
                                        strHtml += "</tr>";
                                        strHtml += "<tr style=\"text-align:left\">";
                                        strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                                    }
                                    else if (dr1[j].ToString().Trim().StartsWith("Scrip Name"))
                                    {
                                        strHtml += "</tr>";
                                        strHtml += "<tr style=\"background-color:#D2B9D3 ;text-align:left\">";
                                        strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                                    }
                                    else
                                    {
                                        if (IsNumeric(dr1[j].ToString()) == true)
                                        {
                                            strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                                        }
                                        else
                                        {
                                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                                        }
                                    }
                                }
                                else
                                {
                                    strHtml += "<td>&nbsp;</td>";
                                }
                            }

                            strHtml += "</tr>";

                        }
                    }
                    // strHtml += "</table>";
                    if (ddlGroup.SelectedItem.Value == "0")
                    {

                        if (oDBEngine.SendReportBranchWise(strHtml, cmbclient.Items[i].Value, oDBEngine.GetDate().ToString(), "Daily MTM/Premium Statement [" + Date.ToString().Trim() + "]", "Branch"))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('4');", true);
                            Page.ClientScript.RegisterStartupScript(GetType(), "JScript12", "<script language='javascript'>Page_Load();</script>");
                        }


                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('5');", true);
                            Page.ClientScript.RegisterStartupScript(GetType(), "JScript112", "<script language='javascript'>Page_Load();</script>");

                        }
                    }
                    else
                    {
                        if (oDBEngine.SendReportBrportfoliogroup(strHtml, cmbclient.Items[i].Text.ToString().Trim(), Date.ToString().Trim(), "Daily MTM/Premium Statement [" + Date.ToString().Trim() + "]", cmbclient.Items[i].Value))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('4');", true);
                            Page.ClientScript.RegisterStartupScript(GetType(), "JScript212", "<script language='javascript'>Page_Load();</script>");
                        }


                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('5');", true);
                            Page.ClientScript.RegisterStartupScript(GetType(), "JScript412", "<script language='javascript'>Page_Load();</script>");

                        }
                    }


                }

            }
            else
            {
                DataView viewData = new DataView();
                viewData = ds.Tables[0].DefaultView;
                viewData.RowFilter = " CustomerId<>'ZZZ'";
                DataTable dtclient = new DataTable();
                dtclient = viewData.ToTable();

                Distinctclient = new DataTable();
                DataView viewClient = new DataView(dtclient);
                Distinctclient = viewClient.ToTable(true, new string[] { "CustomerId" });

                if (Distinctclient.Rows.Count > 0)
                {
                    cmbclient.DataSource = Distinctclient;
                    cmbclient.DataValueField = "CustomerId";
                    cmbclient.DataTextField = "CustomerId";
                    cmbclient.DataBind();

                }

                for (int i = 0; i < cmbclient.Items.Count; i++)
                {
                    DataView viewsingleclient = new DataView();
                    viewsingleclient = ds.Tables[0].DefaultView;
                    viewsingleclient.RowFilter = "(CustomerId='" + cmbclient.Items[i].Value + "')";
                    DataTable dt = new DataTable();
                    dt = viewsingleclient.ToTable();
                    dt.Columns.Remove("CustomerId");
                    String strHtml = String.Empty;
                    String strHtmlheader = String.Empty;
                    string strFooter = string.Empty;
                    // strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";

                    strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                    for (int cname = 0; cname < dt.Columns.Count; cname++)
                    {
                        strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + dt.Columns[cname].ColumnName + "</b></td>";
                    }
                    strHtml += "</tr>";

                    int flag = 0;
                    foreach (DataRow dr1 in dt.Rows)
                    {
                        flag = flag + 1;
                        strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";

                        if (dr1[0].ToString().Trim().StartsWith("Client Name"))
                        {
                            for (int j = 0; j < dt.Columns.Count; j++)
                            {
                                if (dr1[j] != DBNull.Value)
                                {
                                    if (dr1[j].ToString().Trim().StartsWith("Client Name") || dr1[j].ToString().Trim().StartsWith("Exchange"))
                                    {
                                        // strHtml += "</tr>";
                                        strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                                        strHtmlheader += "<tr style=\"background-color:lavender ;text-align:left\">";
                                        strHtmlheader += "<td align=\"left\" colspan='" + dt.Columns.Count + "'><b>" + dr1[j].ToString().Trim();
                                        strHtmlheader += "</td></tr>";

                                        strFooter = "<tr style=\"text-align:left\">";
                                        strFooter += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\"><b>Total</b></td>";
                                    }
                                    else if (dr1[j].ToString().Trim().StartsWith("Scrip Name"))
                                    {
                                        strFooter += "</tr>";
                                        strFooter += "<tr style=\"background-color:#D2B9D3 ;text-align:left\">";
                                        strFooter += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                                    }
                                    else
                                    {
                                        if (IsNumeric(dr1[j].ToString()) == true)
                                        {
                                            strFooter += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                                        }
                                        else
                                        {
                                            strFooter += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                                        }
                                    }
                                }
                                else
                                {
                                    strFooter += "<td>&nbsp;</td>";
                                }

                            }
                        }
                        else
                        {

                            for (int j = 0; j < dt.Columns.Count; j++)
                            {
                                if (dr1[j] != DBNull.Value)
                                {
                                    //if (dr1[j].ToString().Trim().StartsWith("Client Name") || dr1[j].ToString().Trim().StartsWith("Exchange"))
                                    //{
                                    //    // strHtml += "</tr>";
                                    //    strHtml += "<tr style=\"text-align:left\">";
                                    //    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                                    //}
                                    if (dr1[j].ToString().Trim().StartsWith("Scrip Name"))
                                    {
                                        strHtml += "</tr>";
                                        strHtml += "<tr style=\"background-color:#D2B9D3 ;text-align:left\">";
                                        strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                                    }
                                    else
                                    {
                                        if (IsNumeric(dr1[j].ToString()) == true)
                                        {
                                            strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                                        }
                                        else
                                        {
                                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + dt.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                                        }
                                    }
                                }
                                else
                                {
                                    strHtml += "<td>&nbsp;</td>";
                                }
                            }
                        }
                        strHtml += "</tr>";
                    }
                    strHtml = strHtmlheader + strHtml + strFooter + "</table>";
                    // strHtml += "</table>";


                    if (oDBEngine.SendReportSt(strHtml, cmbclient.Items[i].Value, Date.ToString().Trim(), "Daily MTM/Premium Statement [" + Date.ToString().Trim() + "]") == true)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('4');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('5');", true);

                    }


                }



            }


        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            ds = (DataSet)ViewState["dataset"];
            export(ds);
        }
        protected void ddlGeneration_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlGeneration.SelectedValue == "3")
            {
                tabRespective.Visible = true;
                string strRptView = DLLRptView.SelectedItem.Text;
                if ((DLLRptView.SelectedItem.Value != "4") || (DLLRptView.SelectedItem.Value != "5") || (DLLRptView.SelectedItem.Value != "6") || (DLLRptView.SelectedItem.Value != "7") || (DLLRptView.SelectedItem.Value != "8") || (DLLRptView.SelectedItem.Value != "4") || (DLLRptView.SelectedItem.Value != "9") || (DLLRptView.SelectedItem.Value != "10"))
                {
                    ddloptionformail.Items.Clear();
                    ddloptionformail.Items.Add(new ListItem("User", "1"));
                    ddloptionformail.Items.Add(new ListItem("Client", "2"));

                }
                //else if (strRptView.Contains("Group +"))
                else if ((DLLRptView.SelectedItem.Value == "7") || (DLLRptView.SelectedItem.Value == "8") || (DLLRptView.SelectedItem.Value == "9") || (DLLRptView.SelectedItem.Value == "10"))
                {
                    ddloptionformail.Items.Clear();
                    ddloptionformail.Items.Add(new ListItem("User", "1"));
                    ddloptionformail.Items.Add(new ListItem("Group/Branch", "3"));
                }
                else
                {
                    ddloptionformail.Items.Clear();
                    ddloptionformail.Items.Add(new ListItem("User", "1"));
                }
            }
            else
                tabRespective.Visible = false;

        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            if (ddlGeneration.SelectedValue == "3")
            {
                tabRespective.Visible = true;
                string strRptView = DLLRptView.SelectedItem.Text;
                if (strRptView.Contains("Client"))
                //if ((DLLRptView.SelectedItem.Value != "4") || (DLLRptView.SelectedItem.Value != "5") || (DLLRptView.SelectedItem.Value != "6") || (DLLRptView.SelectedItem.Value != "7") || (DLLRptView.SelectedItem.Value != "8")  || (DLLRptView.SelectedItem.Value != "9") || (DLLRptView.SelectedItem.Value != "10"))
                {
                    ddloptionformail.Items.Clear();
                    ddloptionformail.Items.Add(new ListItem("Client", "2"));
                    ddloptionformail.Items.Add(new ListItem("User", "1"));
                    //ddloptionformail.Items.Add(new ListItem("Client", "2"));
                    //cmbsearchOption.Visible = true;

                }
                if (strRptView.Contains("Group +"))
                //else if ((DLLRptView.SelectedItem.Value == "7") || (DLLRptView.SelectedItem.Value == "8") || (DLLRptView.SelectedItem.Value == "9") || (DLLRptView.SelectedItem.Value == "10"))
                {
                    ddloptionformail.Items.Clear();
                    ddloptionformail.Items.Add(new ListItem("Group/Branch", "3"));
                    ddloptionformail.Items.Add(new ListItem("User", "1"));
                    //ddloptionformail.Items.Add(new ListItem("Group/Branch", "3"));
                    //cmbsearchOption.Visible = true;
                }
                if (strRptView.Contains("&"))
                //else if ((DLLRptView.SelectedItem.Value == "7") || (DLLRptView.SelectedItem.Value == "8") || (DLLRptView.SelectedItem.Value == "9") || (DLLRptView.SelectedItem.Value == "10"))
                {
                    ddloptionformail.Items.Clear();
                    //ddloptionformail.Items.Add(new ListItem("User", "1"));
                    ddloptionformail.Items.Add(new ListItem("Group/Branch", "1"));
                    //cmbsearchOption.Visible = false;
                }
                if (strRptView.Contains("Exchange"))
                {
                    ddloptionformail.Items.Clear();
                    ddloptionformail.Items.Add(new ListItem("User", "1"));
                    //cmbsearchOption.Visible = true;
                }
            }
            else
                tabRespective.Visible = false;
        }



    }
}