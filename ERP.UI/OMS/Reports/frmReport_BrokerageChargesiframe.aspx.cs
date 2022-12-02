using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_BrokerageChargesiframe : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        PeriodicalReports periodiaclrep = new PeriodicalReports();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        ExcelFile objExcel = new ExcelFile();
        DataTable dtExport = new DataTable();
        string data;
        DataRow[] drow;
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
            if (Session["ExchangeSegmentID"].ToString().Trim() == "1" || Session["ExchangeSegmentID"].ToString().Trim() == "4" || Session["ExchangeSegmentID"].ToString().Trim() == "15" || Session["ExchangeSegmentID"].ToString().Trim() == "19")
            {
                tr_sett.Visible = true;

            }
            else
            {
                tr_sett.Visible = false;
            }
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {
                SettNo();
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
        void SettNo()
        {
            // litSettlementNo.InnerText = Session["LastSettNo"].ToString().Substring(0, 7);
            litSettlementType.InnerText = Session["LastSettNo"].ToString().Substring(7, 1);
            //HiddenField_SettNo.Value = Session["LastSettNo"].ToString().Substring(0, 7);
            //HiddenField_Setttype.Value = "'" + Session["LastSettNo"].ToString().Substring(7, 1) + "'";


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
                if (idlist[0].ToString().Trim() == "Clients" || idlist[0].ToString().Trim() == "Company" || idlist[0].ToString().Trim() == "Broker")
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
                else if (idlist[0].ToString().Trim() == "UserID")
                {
                    string[] val = cl[i].Split(';');

                    if (str == "")
                    {
                        str = "'" + val[0] + "'";
                        str1 = "'" + val[0] + "'" + ";" + val[1];
                    }
                    else
                    {
                        str += ",'" + val[0] + "'";
                        str1 += "," + "'" + val[0] + "'" + ";" + val[1];
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
            else if (idlist[0] == "Broker")
            {
                data = "Broker~" + str;
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
            else if (idlist[0] == "Segment")
            {
                data = "Segment~" + str;
            }
            else if (idlist[0] == "UserID")
            {
                data = "UserID~" + str;
            }
            else if (idlist[0] == "MAILEMPLOYEE")
            {
                data = "MAILEMPLOYEE~" + str;
            }
            else if (idlist[0] == "Company")
            {
                data = "Company~" + str;
            }
            else if (idlist[0] == "SettlementType")
            {
                data = "SettlementType~" + str;
            }

        }
        void date()
        {
            DtFrom.EditFormatString = oconverter.GetDateFormat("Date");
            DtFrom.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            DtTo.EditFormatString = oconverter.GetDateFormat("Date");
            DtTo.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            Segment();
        }
        void Segment()
        {
            HiddenField_Segment.Value = Session["usersegid"].ToString();

            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "1")
                litSegmentMain.InnerText = "NSE-CM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "2")
                litSegmentMain.InnerText = "NSE-FO";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "3")
                litSegmentMain.InnerText = "NSE-CDX";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "4")
                litSegmentMain.InnerText = "BSE-CM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "5")
                litSegmentMain.InnerText = "BSE-FO";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "6")
                litSegmentMain.InnerText = "BSE-CDX";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "7")
                litSegmentMain.InnerText = "MCX-COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "8")
                litSegmentMain.InnerText = "MCXSX-CDX";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "9")
                litSegmentMain.InnerText = "NCDEX-COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "10")
                litSegmentMain.InnerText = "DGCX-COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "11")
                litSegmentMain.InnerText = "NMCE-COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "12")
                litSegmentMain.InnerText = "ICEX-COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "13")
                litSegmentMain.InnerText = "USE-CDX";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "14")
                litSegmentMain.InnerText = "NSEL-SPOT";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "15")
                litSegmentMain.InnerText = "CSE-CM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "17")
                litSegmentMain.InnerText = "ACE-COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "19")
                litSegmentMain.InnerText = "MCXSX-CM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "20")
                litSegmentMain.InnerText = "MCXSX-FO";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "21")
                litSegmentMain.InnerText = "BFX-COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "22")
                litSegmentMain.InnerText = "INSX-COMM";
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "23")
                litSegmentMain.InnerText = "INFX-COMM";


        }
        void chkboxliststyle()
        {

            foreach (ListItem item in chktfilter.Items)
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
        void SpCall()
        {
            if (DLLRptView.SelectedItem.Value.ToString().Trim() == "10" || DLLRptView.SelectedItem.Value.ToString().Trim() == "11" || DLLRptView.SelectedItem.Value.ToString().Trim() == "12" || DLLRptView.SelectedItem.Value.ToString().Trim() == "13")
            {
                ProcedureAcrossSegment();
            }
            else
            {
                Procedure();
            }
        }
        void ProcedureAcrossSegment()
        {
            string CompanyId = "";
            string GrpType = "";
            string GrpId = "";
            string Chk_Consolidate = "";
            string ClientId = "";
            string SettType = "";

            if (RdbAllCompany.Checked)
            {
                CompanyId = "ALL";
            }
            else if (RdbCurrentCompany.Checked)
            {
                CompanyId = "'" + Session["LastCompany"].ToString().Trim() + "'";
            }
            else
            {
                CompanyId = HiddenField_Company.Value;
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
                    GrpId = HiddenField_Branch.Value;
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
                    GrpId = HiddenField_BranchGroup.Value;
                }
            }
            else
            {
                GrpType = ddlgrouptype.SelectedItem.Text.ToString().Trim();
                if (rdddlgrouptypeAll.Checked)
                {
                    GrpId = "ALL";
                }
                else
                {
                    GrpId = HiddenField_Group.Value;
                }
            }
            if (ChkCOnsolidatedAcrossSegment.Checked)
            {
                Chk_Consolidate = "Y";
            }
            else
            {
                Chk_Consolidate = "N";
            }
            if (rdbClientALL.Checked)
            {
                ClientId = "ALL";
            }
            else
            {
                ClientId = HiddenField_Client.Value;
            }

            if (RadbSettlementTypeAll.Checked)
            {
                SettType = "All";
            }
            if (RadbSettlementTypeselection.Checked)
            {
                string newitem1 = "";
                string[] array1 = HiddenField_Setttype.Value.ToString().Trim().Split(',');
                for (int i = 0; i < array1.Length; i++)
                {
                    if (newitem1 == "")
                        newitem1 = "'" + array1[i] + "'";
                    else
                        newitem1 = newitem1 + ",'" + array1[i] + "'";
                }
                SettType = newitem1;
            }
            if (rdbSettlementTypeSelected.Checked)
            {
                string crntsett1 = "";
                crntsett1 = "'" + Session["LastSettNo"].ToString().Substring(7, 1).ToString().Trim() + "'";
                SettType = crntsett1;
            }
            ds = periodiaclrep.BokerageChargesStatementALL(DtFrom.Value.ToString(), DtTo.Value.ToString(), rdbSegmentAll.Checked ? "ALL" : HiddenField_Segment.Value.ToString(),
                CompanyId, GrpType, GrpId, Session["userbranchHierarchy"].ToString(), DLLRptView.SelectedItem.Value.ToString().Trim(), Chk_Consolidate, ClientId, SettType);

            ViewState["dataset"] = ds;
            if (DLLRptView.SelectedItem.Value.ToString().Trim() == "10" || DLLRptView.SelectedItem.Value.ToString().Trim() == "11" || DLLRptView.SelectedItem.Value.ToString().Trim() == "12")
            {
                if (ChkCOnsolidatedAcrossSegment.Checked)
                {
                    ds.Tables[0].Columns.Remove("StatusOrder");
                    ds.Tables[0].Columns.Remove("Grp");
                }

            }


        }
        void Procedure()
        {
            string CommandText = "";
            if (Session["ExchangeSegmentID"].ToString().Trim() == "1" || Session["ExchangeSegmentID"].ToString().Trim() == "2" || Session["ExchangeSegmentID"].ToString().Trim() == "4" || Session["ExchangeSegmentID"].ToString().Trim() == "5" || Session["ExchangeSegmentID"].ToString().Trim() == "15" || Session["ExchangeSegmentID"].ToString().Trim() == "19" || Session["ExchangeSegmentID"].ToString().Trim() == "20")
            {
                CommandText = "BokerageChargesStatement";
            }
            else
            {
                CommandText = "BokerageChargesStatementComm";
            }
            string Broker = "";
            string ClientId = "";
            string Asset = "";
            string TerminalId = "";
            string Segment = "";
            if (ddlviewby.SelectedItem.Value == "2")
            {
                Broker = "BO";
                if (rdbbrokerall.Checked)
                {
                    ClientId = "ALL";
                }
                else
                {
                    ClientId = HiddenField_Broker.Value;
                }
            }
            if (ddlviewby.SelectedItem.Value == "1")
            {
                Broker = "NA";
                if (rdbClientALL.Checked)
                {
                    ClientId = "ALL";
                }
                else
                {
                    ClientId = HiddenField_Client.Value;
                }
            }

            if (rdbAssetAll.Checked)
            {
                Asset = "ALL";
            }
            else
            {
                Asset = HiddenField_Product.Value;
            }
            if (rdbUserIDAll.Checked)
            {
                TerminalId = "ALL";
            }
            else
            {
                TerminalId = HiddenField_UserID.Value;
            }
            if (rdbSegmentAll.Checked)
            {
                Segment = "ALL";
            }
            else
            {
                Segment = HiddenField_Segment.Value;
            }
            string GrpType = "";
            string GrpId = "";
            if (ddlGroup.SelectedItem.Value.ToString() == "0")
            {
                GrpType = "BRANCH";
                if (rdbranchAll.Checked)
                {
                    GrpId = "ALL";
                }
                else
                {
                    GrpId = HiddenField_Branch.Value;
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
                    GrpId = HiddenField_BranchGroup.Value;
                }
            }
            else
            {
                GrpType = ddlgrouptype.SelectedItem.Text.ToString().Trim();
                if (rdddlgrouptypeAll.Checked)
                {
                    GrpId = "ALL";
                }
                else
                {
                    GrpId = HiddenField_Group.Value;
                }
            }

            string TopRecord = "";
            if (txtTopRecord.Value.ToString().Trim() == "0" || txtTopRecord.Value.ToString().Trim() == "")
            {
                TopRecord = "0";
            }
            else
            {
                TopRecord = txtTopRecord.Value.ToString().Trim();
            }

            ViewState["Turnover"] = "unchk";
            ViewState["Brokerage"] = "unchk";

            foreach (ListItem listitem in chktfilter.Items)
            {
                if (listitem.Selected)
                {
                    if (listitem.Value == "Turnover")
                    {
                        ViewState["Turnover"] = "chk";
                    }
                    if (listitem.Value == "Brokerage")
                    {
                        ViewState["Brokerage"] = "chk";
                    }
                }
            }
            string Filter = "";
            string SettType = "";
            if (ViewState["Turnover"].ToString().Trim() == "chk" && ViewState["Brokerage"].ToString().Trim() == "chk")
            {
                Filter = "Both";
            }
            if (ViewState["Turnover"].ToString().Trim() == "chk" && ViewState["Brokerage"].ToString().Trim() == "unchk")
            {
                Filter = "Turnover";
            }
            if (ViewState["Turnover"].ToString().Trim() == "unchk" && ViewState["Brokerage"].ToString().Trim() == "chk")
            {
                Filter = "Brokerage";
            }
            if (ViewState["Turnover"].ToString().Trim() == "unchk" && ViewState["Brokerage"].ToString().Trim() == "unchk")
            {
                Filter = "Both";
            }
            if (CommandText == "BokerageChargesStatement")
            {
                if (RadbSettlementTypeAll.Checked)
                {
                    SettType = "All";
                }
                if (RadbSettlementTypeselection.Checked)
                {
                    string newitem = "";
                    string[] array = HiddenField_Setttype.Value.ToString().Trim().Split(',');
                    for (int i = 0; i < array.Length; i++)
                    {
                        if (newitem == "")
                            newitem = "'" + array[i] + "'";
                        else
                            newitem = newitem + ",'" + array[i] + "'";
                    }
                    SettType = newitem;
                }
                if (rdbSettlementTypeSelected.Checked)
                {
                    string crntsett = "";
                    crntsett = "'" + Session["LastSettNo"].ToString().Substring(7, 1).ToString().Trim() + "'";
                    SettType = crntsett;
                }
            }

            ds = periodiaclrep.BokerageChargesStatement_All(CommandText, DtFrom.Value.ToString(), DtTo.Value.ToString(), Broker, ClientId, Asset, TerminalId,
                Segment, Session["LastCompany"].ToString(), GrpType, GrpId, Session["userbranchHierarchy"].ToString(), DLLRptView.SelectedItem.Value.ToString().Trim(),
                ChkConsolidate.Checked ? "Y" : "N", ChkConsolidateSegmentScrip.Checked ? "Y" : "N", TopRecord, DDlSortOrder.SelectedItem.Value.ToString().Trim(),
                Filter, SettType);

            ViewState["dataset"] = ds;
        }
        protected void btnScreen_Click(object sender, EventArgs e)
        {
            SpCall();
            ds = (DataSet)ViewState["dataset"];
            if (ds.Tables[0].Rows.Count > 0)
            {

                FnHtmls(ds);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "RecordDisplay", "RecordDisplay();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('1');", true);
            }
        }
        void FnHtmls(DataSet ds)
        {
            //////////For header
            //ds.Tables[0].Columns.Remove("BranchID");

            String strHtmlheader = String.Empty;
            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }
            str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report Period " + oconverter.ArrangeDate2(DtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(DtTo.Value.ToString());
            str = str + " ; Report View : " + DLLRptView.SelectedItem.Text.ToString().Trim();

            strHtmlheader = "<table width=\"90%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtmlheader += "<tr><td align=\"left\" colspan=" + ds.Tables[0].Columns.Count + " style=\"color:Blue;\">" + str + "</td></tr></table>";


            ////////Detail Display
            String strHtml = String.Empty;
            strHtml = "<table width=\"90%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";


            //////////////TABLE HEADER BIND
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
            {
                strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + ds.Tables[0].Columns[i].ColumnName + "</b></td>";
            }
            strHtml += "</tr>";

            int flag = 0;
            foreach (DataRow dr1 in ds.Tables[0].Rows)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                {

                    if (dr1[j] != DBNull.Value)
                    {
                        if (dr1[j].ToString().Trim().StartsWith("Grand") || dr1[j].ToString().Trim().StartsWith("Total") || dr1[j].ToString().Trim().StartsWith("User-Id") || dr1[j].ToString().Trim().StartsWith("Branch/Group Name") || dr1[j].ToString().Trim().StartsWith("Net Total") || dr1[j].ToString().Trim().StartsWith("Client Name") || dr1[j].ToString().Trim().StartsWith("Month Name") || dr1[j].ToString().Trim().StartsWith("Instrument"))
                        {
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                        }
                        else
                        {
                            if (IsNumeric(dr1[j].ToString()) == true)
                            {
                                strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                            {
                                strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

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
        void FnHtml(DataSet ds)
        {
            //////////For header
            ds.Tables[0].Columns.Remove("BranchID");

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


            //////////////TABLE HEADER BIND
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
            {
                strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + ds.Tables[0].Columns[i].ColumnName + "</b></td>";
            }
            strHtml += "</tr>";

            int flag = 0;
            foreach (DataRow dr1 in ds.Tables[0].Rows)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                {

                    if (dr1[j] != DBNull.Value)
                    {
                        if (dr1[j].ToString().Trim().StartsWith("Grand") || dr1[j].ToString().Trim().StartsWith("Total") || dr1[j].ToString().Trim().StartsWith("User-Id") || dr1[j].ToString().Trim().StartsWith("Branch/Group Name") || dr1[j].ToString().Trim().StartsWith("Net Total") || dr1[j].ToString().Trim().StartsWith("Client Name") || dr1[j].ToString().Trim().StartsWith("Month Name") || dr1[j].ToString().Trim().StartsWith("Instrument"))
                        {
                            strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                        }
                        else
                        {
                            if (IsNumeric(dr1[j].ToString()) == true)
                            {
                                strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                            }
                            else
                            {
                                strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + ds.Tables[0].Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

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
        void FnHtmlbranch(DataRow[] drow)
        {
            //////////For header
            // drow[0].Table.Columns.Remove("BranchID");

            String strHtmlheader = String.Empty;
            string str = null;
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                str = "[" + ddlgrouptype.SelectedItem.Text.ToString().Trim() + "]";
            }
            str = ddlGroup.SelectedItem.Text.ToString().Trim() + " Wise " + str + "Report Period " + oconverter.ArrangeDate2(DtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(DtTo.Value.ToString());
            str = str + " ; Report View : " + DLLRptView.SelectedItem.Text.ToString().Trim();

            strHtmlheader = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtmlheader += "<tr><td align=\"left\" colspan=" + drow[0].Table.Columns.Count + " style=\"color:Blue;\">" + str + "</td></tr></table>";


            ////////Detail Display
            String strHtml = String.Empty;
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";


            //////////////TABLE HEADER BIND
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            for (int i = 0; i < drow[0].Table.Columns.Count; i++)
            {
                if (i != 1)
                {
                    strHtml += "<td align=\"center\" style=\"font-size:smaller;\" nowrap=nowrap;><b>" + drow[0].Table.Columns[i].ColumnName + "</b></td>";
                }
            }
            strHtml += "</tr>";

            int flag = 0;
            foreach (DataRow dr1 in drow)
            {
                flag = flag + 1;
                strHtml += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                for (int j = 0; j < drow[0].Table.Columns.Count; j++)
                {
                    if (j != 1)
                    {

                        if (dr1[j] != DBNull.Value)
                        {
                            if (dr1[j].ToString().Trim().StartsWith("Grand") || dr1[j].ToString().Trim().StartsWith("Total") || dr1[j].ToString().Trim().StartsWith("User-Id") || dr1[j].ToString().Trim().StartsWith("Branch/Group Name") || dr1[j].ToString().Trim().StartsWith("Net Total") || dr1[j].ToString().Trim().StartsWith("Client Name") || dr1[j].ToString().Trim().StartsWith("Month Name") || dr1[j].ToString().Trim().StartsWith("Instrument"))
                            {
                                strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + drow[0].Table.Columns[j].ColumnName + "\"><b>" + dr1[j] + "</b></td>";
                            }
                            else
                            {
                                if (IsNumeric(dr1[j].ToString()) == true)
                                {
                                    strHtml += "<td align=\"right\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + drow[0].Table.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

                                }
                                else
                                {
                                    strHtml += "<td align=\"left\" style=\"font-size:smaller;\"  nowrap=nowrap; title=\"" + drow[0].Table.Columns[j].ColumnName + "\">" + dr1[j] + "</td>";

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
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            SpCall();
            ds = (DataSet)ViewState["dataset"];
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Columns.Contains("BranchID"))
                    ds.Tables[0].Columns.Remove("BranchID");

                export(ds);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('1');", true);
            }

        }
        void export(DataSet ds)
        {
            DataTable dtExport = ds.Tables[0].Copy();

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

            objExcel.ExportToExcelforExcel(dtExport, "Brokerage Statement", "Client Total", dtReportHeader, dtReportFooter);

        }

        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            ds = (DataSet)ViewState["dataset"];
            export(ds);
        }

        protected void btnSendmail_Click(object sender, EventArgs e)
        {
            mailsend();
        }

        void mailsend()
        {
            SpCall();
            ds = (DataSet)ViewState["dataset"];/////////////HiddenField_BranchGroup//////////HiddenField_Client
            if (ds.Tables[0].Rows.Count > 0)
            {
                email(ds);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('1');", true);
            }
        }
        void email(DataSet ds)
        {
            //DataTable dtc=new DataTable();
            string[,] clntg = null;
            string[,] clntb = null;
            string[,] clntb1 = null;
            string[,] clntg1 = null;
            string valuestr = null;
            string[] clnt = null;
            string[] branch = null;
            string[] group = null;
            string[] branchall = null;
            branch = HiddenField_Branch.Value.ToString().Split(',');
            group = HiddenField_Group.Value.ToString().Split(',');
            //branchall = oDBEngine.GetFieldValue("tbl_master_branch", "branch_id", "branch_id is not null", 200);
            //branchall = branchall.ToString().Split(',');
            ViewState["mail"] = "mail";
            DataTable dt = new DataTable();
            dt.Columns.Add("BranchId");
            dt.Rows.Add("BranchId");
            ds.Tables.Add(dt);
            DataView dv = ds.Tables[0].DefaultView;
            DataTable dtd = dv.ToTable(true, "BranchId");

            ViewState["date"] = oconverter.ArrangeDate2(DtFrom.Value.ToString()) + " - " + oconverter.ArrangeDate2(DtTo.Value.ToString());
            if (ddloptionformail.SelectedItem.Value == "1")
            {
                FnHtml(ds);
                clnt = HiddenField_emmail.Value.ToString().Split(',');
            }
            if (ddloptionformail.SelectedItem.Value == "2")
            {
                if (rdbranchAll.Checked == true)
                {
                    for (int i = 0; i < dtd.Rows.Count; i++)
                    {
                        drow = ds.Tables[0].Select("BranchID='" + dtd.Rows[i]["BranchID"].ToString().Trim() + "'");
                        //drow = ds.Tables[0].Select(dtd.Columns["BranchID"].ToString());
                        //drow = ds.Tables[0].Select("BranchID=18");

                        if (drow.Length > 0)
                        {
                            FnHtmlbranch(drow);
                            valuestr = drow[0]["BranchID"].ToString().Trim();
                            clntb1 = oDBEngine.GetFieldValue("tbl_master_branch", "branch_cpemail", "branch_id in (" + valuestr.ToString().Trim() + ")", 1);
                            if (oDBEngine.SendReportSt1(ViewState["mail"].ToString().Trim(), clntb1[0, 0].ToString().Trim(), ViewState["date"].ToString().Trim(), "Brokerage Statement [" + ViewState["date"].ToString().Trim() + "]") == true)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('4');", true);
                            }
                        }
                    }
                }
                else
                {



                    for (int i = 0; i < branch.Length; i++)
                    // for(int i =0; i < dtd.Columns.Count; i++)
                    {
                        drow = ds.Tables[0].Select("BranchID=" + branch[i]);
                        if (drow.Length > 0)
                        {
                            FnHtmlbranch(drow);

                            clntb = oDBEngine.GetFieldValue("tbl_master_branch", "branch_cpemail", "branch_id in (" + branch[i] + ")", 1);
                            if (oDBEngine.SendReportSt1(ViewState["mail"].ToString().Trim(), clntb[0, 0].ToString().Trim(), ViewState["date"].ToString().Trim(), "Brokerage Statement [" + ViewState["date"].ToString().Trim() + "]") == true)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('4');", true);
                            }
                        }
                    }

                }
            }
            if (ddloptionformail.SelectedItem.Value == "3")
            {
                // HiddenField_Client.Value=HiddenField_Client.Value.Replace("'","");
                //clnt = HiddenField_Client.Value.ToString().Split(',');
                if (rdddlgrouptypeAll.Checked == true)
                {
                    for (int i = 0; i < dtd.Rows.Count; i++)
                    {
                        drow = ds.Tables[0].Select("BranchID='" + dtd.Rows[i]["BranchID"].ToString().Trim() + "'");
                        //drow = ds.Tables[0].Select(dtd.Columns["BranchID"].ToString());
                        //drow = ds.Tables[0].Select("BranchID=18");

                        if (drow.Length > 0)
                        {
                            FnHtmlbranch(drow);
                            valuestr = drow[0]["BranchID"].ToString().Trim();
                            clntg1 = oDBEngine.GetFieldValue("tbl_master_groupmaster", "gpm_emailid", "gpm_id in (" + valuestr.ToString().Trim() + ")", 1);
                            if (oDBEngine.SendReportSt2(ViewState["mail"].ToString().Trim(), clntg1[0, 0].ToString().Trim(), ViewState["date"].ToString().Trim(), "Brokerage Statement [" + ViewState["date"].ToString().Trim() + "]") == true)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('4');", true);
                            }
                        }
                    }
                }
                else
                {

                    for (int i = 0; i < group.Length; i++)
                    {
                        drow = ds.Tables[0].Select("BranchID=" + group[i]);
                        if (drow.Length > 0)
                        {

                            FnHtmlbranch(drow);
                            clntg = oDBEngine.GetFieldValue("tbl_master_groupmaster", "gpm_emailid", "gpm_id in (" + group[i] + ")", 1);//HiddenField_Group
                            if (oDBEngine.SendReportSt2(ViewState["mail"].ToString().Trim(), clntg[0, 0].ToString().Trim(), ViewState["date"].ToString().Trim(), "Brokerage Statement [" + ViewState["date"].ToString().Trim() + "]") == true)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('4');", true);
                            }
                        }
                    }
                }
            }
            if (ddloptionformail.SelectedItem.Value == "1")
            {
                for (int i = 0; i < clnt.Length; i++)
                {
                    if (oDBEngine.SendReportSt(ViewState["mail"].ToString().Trim(), clnt[i].ToString().Trim(), ViewState["date"].ToString().Trim(), "Brokerage Statement [" + ViewState["date"].ToString().Trim() + "]") == true)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('4');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('5');", true);

                    }
                }
            }
            //else if (ddloptionformail.SelectedItem.Value == "2")
            //{
            //    for (int i = 0; i < clntb.Length; i++)
            //    {
            //        if (oDBEngine.SendReportSt1(ViewState["mail"].ToString().Trim(), clntb[i, 0].ToString().Trim(), ViewState["date"].ToString().Trim(), "Brokerage Statement [" + ViewState["date"].ToString().Trim() + "]") == true)
            //        {
            //            ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('4');", true);
            //        }
            //        else
            //        {
            //            ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('5');", true);

            //        }
            //    }
            //}
            //else if (ddloptionformail.SelectedItem.Value == "3")
            //{
            //    for (int i = 0; i < clntg.Length; i++)
            //    {
            //        if (oDBEngine.SendReportSt2(ViewState["mail"].ToString().Trim(), clntg[i, 0].ToString().Trim(), ViewState["date"].ToString().Trim(), "Brokerage Statement [" + ViewState["date"].ToString().Trim() + "]") == true)
            //        {
            //            ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('4');", true);
            //        }
            //        else
            //        {
            //            ScriptManager.RegisterStartupScript(this, this.GetType(), "fnNoRecord", "fnNoRecord('5');", true);

            //        }
            //    }
            //}
        }
    }
}