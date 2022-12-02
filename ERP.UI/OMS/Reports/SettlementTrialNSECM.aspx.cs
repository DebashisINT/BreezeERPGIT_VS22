using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_SettlementTrialNSECM : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        ExcelFile objExcel = new ExcelFile();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        DailyReports dailyrep = new DailyReports();
        DataSet ds = new DataSet();
        string branchname = null;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //DBEngine oDBEngine = new DBEngine(string.Empty);
        string data;
        ReportDocument ReportDocument = new ReportDocument();
        string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
        string GroupOrBranch = null;
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
            string SpanText = "Settlement Trial For " + Session["LastSettNo"].ToString().Substring(0, 7);
            Page.ClientScript.RegisterStartupScript(GetType(), "pageload", "<script>Page_Load('" + SpanText + "');</script>");

            if (!IsPostBack)
            {
                litSettlementTypeCurrent.InnerText = Session["LastSettNo"].ToString().Substring(7, 1);
                ddlGroupType.DataSource = oDBEngine.GetDataTable("tbl_master_groupmaster", "distinct gpm_type", null);
                ddlGroupType.DataTextField = "gpm_type";
                ddlGroupType.DataValueField = "gpm_type";
                ddlGroupType.DataBind();

            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//


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


                string[] val = cl[i].Split(';');
                string[] AcVal = val[0].Split('-');
                if (str == "")
                {
                    if (idlist[0] == "MAILEMPLOYEE")
                    {
                        str = AcVal[0];

                    }

                }
                else
                {
                    if (idlist[0] == "MAILEMPLOYEE")
                    {
                        str += "," + AcVal[0];
                    }

                }

            }


            if (idlist[0] == "MAILEMPLOYEE")
            {
                data = "MAILEMPLOYEE~" + str;
            }
        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }
        void datebind()
        {
            string settelmentdate = null;
            string[,] DtStartEnddate = oDBEngine.GetFieldValue("Master_Settlements", "cast(Settlements_StartDateTime as varchar)+','+cast(Settlements_FundsPayin as varchar)+','+cast(Settlements_FundsPayout as varchar)", " Ltrim(RTRIM(settlements_Number))='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ltrim(RTRIM(settlements_TypeSuffix))='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and settlements_ExchangeSegmentId='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "'", 1);
            if (DtStartEnddate[0, 0] != "n")
            {
                string[] idlist2 = DtStartEnddate[0, 0].ToString().Split(','); // fetch startdate and FundsPayin and End Date from Master_Settlements
                settelmentdate = Convert.ToDateTime(idlist2[0]).ToString();
                HiddenField_Date.Value = settelmentdate.ToString().Trim();

                string checkselect = null;
                if (rdbSettTypeCurrent.Checked)
                {
                    checkselect = Session["LastSettNo"].ToString().Substring(7, 1).ToString().Trim();
                }
                else
                {
                    checkselect = "ALL";
                }
                ViewState["header"] = "Settlement Trail For " + Convert.ToDateTime(HiddenField_Date.Value).ToString("dd-MMM-yyyy") + " ; Settlement Number : " + Session["LastSettNo"].ToString().Substring(0, 7).ToString().Trim() + "  Settlement Type : " + checkselect.ToString().Trim();
                ViewState["header"] = ViewState["header"].ToString().Trim() + " ; Funds Payin :" + oconverter.ArrangeDate2(idlist2[1].ToString().Trim()) + " ; Funds Payout :" + oconverter.ArrangeDate2(idlist2[2].ToString().Trim());
            }

        }
        void datebindmail()
        {
            string settelmentdate = null;
            string[,] DtStartEnddate = oDBEngine.GetFieldValue("Master_Settlements", "cast(Settlements_StartDateTime as varchar)+','+cast(Settlements_FundsPayin as varchar)+','+cast(Settlements_FundsPayout as varchar)", " Ltrim(RTRIM(settlements_Number))='" + Session["LastSettNo"].ToString().Substring(0, 7) + "' and ltrim(RTRIM(settlements_TypeSuffix))='" + Session["LastSettNo"].ToString().Substring(7, 1) + "' and settlements_ExchangeSegmentId='" + HttpContext.Current.Session["ExchangeSegmentID"].ToString() + "'", 1);
            if (DtStartEnddate[0, 0] != "n")
            {
                string[] idlist2 = DtStartEnddate[0, 0].ToString().Split(','); // fetch startdate and FundsPayin and End Date from Master_Settlements
                settelmentdate = Convert.ToDateTime(idlist2[0]).ToString();
                HiddenField_Date.Value = settelmentdate.ToString().Trim();

                string checkselect = null;
                if (rdbSettTypeCurrent.Checked)
                {
                    checkselect = Session["LastSettNo"].ToString().Substring(7, 1).ToString().Trim();
                }
                else
                {
                    checkselect = "ALL";
                }
                ViewState["headermail"] = "Settlement Trail For " + Convert.ToDateTime(HiddenField_Date.Value).ToString("dd-MMM-yyyy") + " ; Settlement Number : " + Session["LastSettNo"].ToString().Substring(0, 7).ToString().Trim() + "  Settlement Type : " + checkselect.ToString().Trim();
                ViewState["headermail"] = ViewState["headermail"].ToString().Trim() + " ; Funds Payin :" + oconverter.ArrangeDate2(idlist2[1].ToString().Trim());
            }

        }
        void procedure()
        {
            datebind();
            datebindmail();

            ds = dailyrep.Report_SettlementTrialNSECM(HiddenField_Date.Value, Session["usersegid"].ToString(), Session["LastCompany"].ToString(),
                Session["LastSettNo"].ToString().Substring(0, 7), rdbSettTypeAll.Checked ? "All" : Session["LastSettNo"].ToString().Substring(7, 1),
                HttpContext.Current.Session["ExchangeSegmentID"].ToString(), ddlType.SelectedItem.Value.ToString().Trim(),
                (ddlType.SelectedItem.Value.ToString().Trim() == "4" || ddlType.SelectedItem.Value.ToString().Trim() == "5") ? ddlGroupType.SelectedValue.Trim() : "");

            ViewState["dataset"] = ds;
        }


        void htmltableClientWise()
        {
            ds = (DataSet)ViewState["dataset"];

            String strHtmlAllClient = String.Empty;
            int colcount = 0;
            strHtmlAllClient = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtmlAllClient += "<tr style=\"background-color: #DBEEF3;\">";

            strHtmlAllClient += "<td align=\"center\">Client Name</td>";
            strHtmlAllClient += "<td align=\"center\">Difference </br> Obligation</td>";
            strHtmlAllClient += "<td align=\"center\">Delivery In </br>Obligation</td>";
            strHtmlAllClient += "<td align=\"center\">Delivery Out </br>Obligation</td>";
            strHtmlAllClient += "<td align=\"center\">Net </br>Obligation</td>";
            colcount = 5;

            if (ds.Tables[1].Rows[0]["Result_TRANCHARGE"].ToString().Trim() != "0.00")
            {
                strHtmlAllClient += "<td align=\"center\">Tran </br>Charges</td>";
                colcount = 6;
            }
            if (ds.Tables[1].Rows[0]["Result_STAMPCHARGE"].ToString().Trim() != "0.00")
            {
                strHtmlAllClient += "<td align=\"center\">Stamp </br>Duty</td>";
                colcount = 7;
            }
            if (ds.Tables[1].Rows[0]["Result_SRVTAX"].ToString().Trim() != "0.00")
            {
                strHtmlAllClient += "<td align=\"center\">Serv Tax </br>& </br>Cess</td>";
                colcount = 8;
            }
            if (ds.Tables[1].Rows[0]["Result_STTAX"].ToString().Trim() != "0.00")
            {
                strHtmlAllClient += "<td align=\"center\">Sec Tran </br>Charges</td>";
                colcount = 9;
            }
            if (ds.Tables[1].Rows[0]["Result_SEBICHARGE"].ToString().Trim() != "0.00")
            {
                strHtmlAllClient += "<td align=\"center\">SEBI </br> Fee</td>";
                colcount = 10;
            }
            if (ds.Tables[1].Rows[0]["Result_DIFFBRKG"].ToString().Trim() != "0.00")
            {
                strHtmlAllClient += "<td align=\"center\">Min.Prcsg </br> Charge</td>";
                colcount = 11;
            }
            if (ds.Tables[1].Rows[0]["Result_DELIVERYCHARGE"].ToString().Trim() != "0.00")
            {
                strHtmlAllClient += "<td align=\"center\">Delivery </br> Charges</td>";
                colcount = 12;
            }
            if (ds.Tables[1].Rows[0]["Result_NETROUNDOFF"].ToString().Trim() != "0.00")
            {
                strHtmlAllClient += "<td align=\"center\">Round-Off </br>Adjustment</td>";
                colcount = 13;
            }
            strHtmlAllClient += "<td align=\"center\">Net Receivable </br>(Dr.)</td>";
            strHtmlAllClient += "<td align=\"center\">Net Payable </br>(Cr.)</td></tr>";

            colcount = colcount + 2;

            int flag = 0;
            branchname = null;
            decimal TempSum = decimal.Zero;
            decimal GrandTotalBranchNetRoundOff = decimal.Zero;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                flag = flag + 1;
                if (ds.Tables[0].Rows[i]["clientname"].ToString() != "Client Net :")
                {
                    if (ds.Tables[0].Rows[i]["NetRoundOff"] != DBNull.Value)
                        TempSum = TempSum + Convert.ToDecimal(ds.Tables[0].Rows[i]["NetRoundOff"].ToString());
                }
                else
                {
                    GrandTotalBranchNetRoundOff = TempSum;
                    TempSum = decimal.Zero;
                }
                if (ddlType.SelectedItem.Value.ToString().Trim() == "1" || ddlType.SelectedItem.Value.ToString().Trim() == "4")
                {
                    GroupOrBranch = ddlType.SelectedItem.Value.ToString().Trim() == "1" ? "Branch Name" : "Group Name";
                    if (branchname != ds.Tables[0].Rows[i]["branchname"].ToString().Trim() + " [ " + ds.Tables[0].Rows[i]["branchcode"] + " ]" && ds.Tables[0].Rows[i]["branchname"].ToString().Trim() != "Z")
                    {

                        strHtmlAllClient += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                        strHtmlAllClient += "<td align=\"left\" nowrap=\"nowrap;\" colspan='" + colcount.ToString().Trim() + "'><b> " + GroupOrBranch + " :" + ds.Tables[0].Rows[i]["branchname"].ToString() + " [ " + ds.Tables[0].Rows[i]["branchcode"] + " ]" + "</b></td>";
                        strHtmlAllClient += "</tr>";
                        flag = flag + 1;

                    }
                }
                GroupOrBranch = ddlType.SelectedItem.Value.ToString().Trim() == "1" ? "Branch Total :" : "Group Total :";
                strHtmlAllClient += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                branchname = ds.Tables[0].Rows[i]["branchname"].ToString().Trim() + " [ " + ds.Tables[0].Rows[i]["branchcode"] + " ]";

                if (ds.Tables[0].Rows[i]["ClientName"].ToString() == GroupOrBranch || ds.Tables[0].Rows[i]["ClientName"].ToString() == "Total :")
                {
                    strHtmlAllClient += "<td align=\"left\" nowrap=\"nowrap;\" ><b>" + ds.Tables[0].Rows[i]["ClientName"].ToString() + "</b></td>";
                }
                else if (ds.Tables[0].Rows[i]["ClientName"].ToString() == "Diff (If Any) :")
                {
                    strHtmlAllClient += "<td style=\"color:maroon;\" align=\"left\" nowrap=\"nowrap;\"><b>" + ds.Tables[0].Rows[i]["ClientName"].ToString() + "</b></td>";
                }
                else if (ds.Tables[0].Rows[i]["ClientName"].ToString() == "Client Total :" || ds.Tables[0].Rows[i]["ClientName"].ToString() == "Client Net :" || ds.Tables[0].Rows[i]["ClientName"].ToString() == "Exchange Obligation :" || ds.Tables[0].Rows[i]["ClientName"].ToString() == "STT Payable To Exchange :")
                {
                    strHtmlAllClient += "<td align=\"left\" nowrap=\"nowrap;\"><b>" + ds.Tables[0].Rows[i]["ClientName"].ToString() + "</b></td>";
                }
                else
                {
                    strHtmlAllClient += "<td align=\"left\" nowrap=\"nowrap;\" >" + ds.Tables[0].Rows[i]["ClientName"].ToString() + "</td>";
                }

                if (ds.Tables[0].Rows[i]["DiffObligation"] != DBNull.Value)
                {
                    strHtmlAllClient += "<td align=\"right\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["DiffObligation"].ToString() + "</td>";
                }
                else
                {
                    strHtmlAllClient += "<td>&nbsp;</td>";
                }

                if (ds.Tables[0].Rows[i]["DiffInObligation"] != DBNull.Value)
                {
                    strHtmlAllClient += "<td align=\"right\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["DiffInObligation"].ToString() + "</td>";
                }
                else
                {
                    strHtmlAllClient += "<td>&nbsp;</td>";
                }

                if (ds.Tables[0].Rows[i]["DiffOutObligation"] != DBNull.Value)
                {
                    strHtmlAllClient += "<td align=\"right\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["DiffOutObligation"].ToString() + "</td>";
                }
                else
                {
                    strHtmlAllClient += "<td>&nbsp;</td>";
                }

                if (ds.Tables[0].Rows[i]["NetAmnt"] != DBNull.Value)
                {
                    strHtmlAllClient += "<td align=\"right\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["NetAmnt"].ToString() + "</td>";
                }
                else
                {
                    strHtmlAllClient += "<td>&nbsp;</td>";
                }
                if (ds.Tables[1].Rows[0]["Result_TRANCHARGE"].ToString().Trim() != "0.00")
                {
                    if (ds.Tables[0].Rows[i]["Trancharge"] != DBNull.Value)
                    {
                        strHtmlAllClient += "<td align=\"right\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["Trancharge"].ToString() + "</td>";
                    }
                    else
                    {
                        strHtmlAllClient += "<td>&nbsp;</td>";
                    }

                }
                if (ds.Tables[1].Rows[0]["Result_STAMPCHARGE"].ToString().Trim() != "0.00")
                {
                    if (ds.Tables[0].Rows[i]["STAMPCHARGE"] != DBNull.Value)
                    {
                        strHtmlAllClient += "<td align=\"right\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["STAMPCHARGE"].ToString() + "</td>";
                    }
                    else
                    {
                        strHtmlAllClient += "<td>&nbsp;</td>";
                    }

                }
                if (ds.Tables[1].Rows[0]["Result_SRVTAX"].ToString().Trim() != "0.00")
                {
                    if (ds.Tables[0].Rows[i]["SRVTAX"] != DBNull.Value)
                    {
                        strHtmlAllClient += "<td align=\"right\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["SRVTAX"].ToString() + "</td>";
                    }
                    else
                    {
                        strHtmlAllClient += "<td>&nbsp;</td>";
                    }

                }
                if (ds.Tables[1].Rows[0]["Result_STTAX"].ToString().Trim() != "0.00")
                {
                    if (ds.Tables[0].Rows[i]["Sttax"] != DBNull.Value)
                    {
                        strHtmlAllClient += "<td align=\"right\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["Sttax"].ToString() + "</td>";
                    }
                    else
                    {
                        strHtmlAllClient += "<td>&nbsp;</td>";
                    }
                }
                if (ds.Tables[1].Rows[0]["Result_SEBICHARGE"].ToString().Trim() != "0.00")
                {
                    if (ds.Tables[0].Rows[i]["SEBICHARGE"] != DBNull.Value)
                    {
                        strHtmlAllClient += "<td align=\"right\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["SEBICHARGE"].ToString() + "</td>";
                    }
                    else
                    {
                        strHtmlAllClient += "<td>&nbsp;</td>";
                    }

                }

                if (ds.Tables[1].Rows[0]["Result_DIFFBRKG"].ToString().Trim() != "0.00")
                {
                    if (ds.Tables[0].Rows[i]["DIFFBRKG"] != DBNull.Value)
                    {
                        strHtmlAllClient += "<td align=\"right\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["DIFFBRKG"].ToString() + "</td>";
                    }
                    else
                    {
                        strHtmlAllClient += "<td>&nbsp;</td>";
                    }

                }
                if (ds.Tables[1].Rows[0]["Result_DELIVERYCHARGE"].ToString().Trim() != "0.00")
                {
                    if (ds.Tables[0].Rows[i]["DELIVERYCHARGE"] != DBNull.Value)
                    {
                        strHtmlAllClient += "<td align=\"right\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["DELIVERYCHARGE"].ToString() + "</td>";
                    }
                    else
                    {
                        strHtmlAllClient += "<td>&nbsp;</td>";
                    }

                }

                if (ds.Tables[1].Rows[0]["Result_NETROUNDOFF"].ToString().Trim() != "0.00")
                {
                    if (ds.Tables[0].Rows[i]["NetROUNDOFF"] != DBNull.Value)
                    {
                        strHtmlAllClient += "<td align=\"right\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["NetROUNDOFF"].ToString() + "</td>";
                    }
                    else
                    {
                        if (ds.Tables[0].Rows[i]["clientname"].ToString() == "Client Net :")
                        {
                            strHtmlAllClient += "<td align=\"right\" nowrap=\"nowrap;\">" + GrandTotalBranchNetRoundOff + "</td>";
                            GrandTotalBranchNetRoundOff = decimal.Zero;
                        }
                        else
                            strHtmlAllClient += "<td>&nbsp;</td>";
                    }

                }
                if (ds.Tables[0].Rows[i]["NetDr"] != DBNull.Value)
                {
                    if (ds.Tables[0].Rows[i]["ClientName"].ToString() == "Diff (If Any) :")
                    {
                        strHtmlAllClient += "<td style=\"color:maroon;\" align=\"right\" nowrap=\"nowrap;\" ><b>" + ds.Tables[0].Rows[i]["NetDr"].ToString() + "</b></td>";
                    }

                    else if (ds.Tables[0].Rows[i]["ClientName"].ToString() == "Client Net :" || ds.Tables[0].Rows[i]["ClientName"].ToString() == "Exchange Obligation :" || ds.Tables[0].Rows[i]["ClientName"].ToString() == "STT Payable To Exchange :" || ds.Tables[0].Rows[i]["ClientName"].ToString() == "Total :")
                    {
                        strHtmlAllClient += "<td align=\"right\" nowrap=\"nowrap;\" ><b>" + ds.Tables[0].Rows[i]["NetDr"].ToString() + "</b></td>";
                    }
                    else
                    {
                        strHtmlAllClient += "<td align=\"right\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["NetDr"].ToString() + "</td>";
                    }
                }

                else
                {
                    strHtmlAllClient += "<td>&nbsp;</td>";
                }


                if (ds.Tables[0].Rows[i]["NetCr"] != DBNull.Value)
                {
                    if (ds.Tables[0].Rows[i]["ClientName"].ToString() == "Diff (If Any) :")
                    {
                        strHtmlAllClient += "<td style=\"color:maroon;\" align=\"right\" nowrap=\"nowrap;\" ><b>" + ds.Tables[0].Rows[i]["NetCr"].ToString() + "</b></td>";
                    }
                    else if (ds.Tables[0].Rows[i]["ClientName"].ToString() == "Client Net :" || ds.Tables[0].Rows[i]["ClientName"].ToString() == "Exchange Obligation :" || ds.Tables[0].Rows[i]["ClientName"].ToString() == "STT Payable To Exchange :" || ds.Tables[0].Rows[i]["ClientName"].ToString() == "Total :")
                    {
                        strHtmlAllClient += "<td align=\"right\" nowrap=\"nowrap;\" ><b>" + ds.Tables[0].Rows[i]["NetCr"].ToString() + "</b></td>";
                    }
                    else
                    {
                        strHtmlAllClient += "<td align=\"right\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["NetCr"].ToString() + "</td>";
                    }
                }

                else
                {
                    strHtmlAllClient += "<td>&nbsp;</td>";
                }

                strHtmlAllClient += "</tr>";
            }

            strHtmlAllClient += "</table>";

            display.InnerHtml = strHtmlAllClient;
            DIVdisplayPERIOD.InnerHtml = ViewState["header"].ToString().Trim();
            if (ddlGeneration.SelectedItem.Value == "1")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "display", "displayalldata();", true);
            }
            else
            {
                //ViewState["mail"] = strHtmlAllClient;
                ViewState["mail"] = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr style=\"background-color: #DBEEF3;\"><td align=\"center\">" + ViewState["headermail"] + "</td></tr></table>" + strHtmlAllClient;
            }

        }

        void htmltableBranchWise()
        {
            ds = (DataSet)ViewState["dataset"];

            String strHtmlAllClient = String.Empty;
            int colcount = 0;
            strHtmlAllClient = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtmlAllClient += "<tr style=\"background-color: #DBEEF3;\">";
            GroupOrBranch = ddlType.SelectedItem.Value.ToString().Trim() == "1" || ddlType.SelectedItem.Value.ToString().Trim() == "2" ? "Branch Name" : "Group Name";
            strHtmlAllClient += "<td align=\"center\">" + GroupOrBranch + "</td>";
            strHtmlAllClient += "<td align=\"center\">Code</td>";
            strHtmlAllClient += "<td align=\"center\">Difference </br> Obligation</td>";
            strHtmlAllClient += "<td align=\"center\">Delivery In </br>Obligation</td>";
            strHtmlAllClient += "<td align=\"center\">Delivery Out </br>Obligation</td>";
            strHtmlAllClient += "<td align=\"center\">Net </br>Obligation</td>";
            colcount = 6;

            if (ds.Tables[1].Rows[0]["Result_TRANCHARGE"].ToString().Trim() != "0.00")
            {
                strHtmlAllClient += "<td align=\"center\">Tran </br>Charges</td>";
                colcount = 7;
            }
            if (ds.Tables[1].Rows[0]["Result_STAMPCHARGE"].ToString().Trim() != "0.00")
            {
                strHtmlAllClient += "<td align=\"center\">Stamp </br>Duty</td>";
                colcount = 8;
            }
            if (ds.Tables[1].Rows[0]["Result_SRVTAX"].ToString().Trim() != "0.00")
            {
                strHtmlAllClient += "<td align=\"center\">Serv Tax </br>& </br>Cess</td>";
                colcount = 9;
            }
            if (ds.Tables[1].Rows[0]["Result_STTAX"].ToString().Trim() != "0.00")
            {
                strHtmlAllClient += "<td align=\"center\">Sec Tran </br>Charges</td>";
                colcount = 10;
            }
            if (ds.Tables[1].Rows[0]["Result_SEBICHARGE"].ToString().Trim() != "0.00")
            {
                strHtmlAllClient += "<td align=\"center\">SEBI </br> Fee</td>";
                colcount = 11;
            }
            if (ds.Tables[1].Rows[0]["Result_DIFFBRKG"].ToString().Trim() != "0.00")
            {
                strHtmlAllClient += "<td align=\"center\">Min.Prcsg </br> Charge</td>";
                colcount = 12;
            }
            if (ds.Tables[1].Rows[0]["Result_DELIVERYCHARGE"].ToString().Trim() != "0.00")
            {
                strHtmlAllClient += "<td align=\"center\">Delivery </br> Charge</td>";
                colcount = 13;
            }

            if (ds.Tables[1].Rows[0]["Result_NETROUNDOFF"].ToString().Trim() != "0.00")
            {
                strHtmlAllClient += "<td align=\"center\">Round-Off </br>Adjustment</td>";
                colcount = 14;
            }
            strHtmlAllClient += "<td align=\"center\">Net Receivable </br>(Dr.)</td>";
            strHtmlAllClient += "<td align=\"center\">Net Payable </br>(Cr.)</td></tr>";

            colcount = colcount + 2;

            int flag = 0;
            branchname = null;
            decimal TempSum = decimal.Zero;
            decimal BranchNetRoundOff = decimal.Zero;
            decimal GrandTotalBranchNetRoundOff = decimal.Zero;


            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                GroupOrBranch = ddlType.SelectedItem.Value.ToString().Trim() == "1" || ddlType.SelectedItem.Value.ToString().Trim() == "2" ? "Branch Total :" : "Group Total :";
                if (ds.Tables[0].Rows[i]["clientname"].ToString().Trim() != GroupOrBranch)
                    TempSum = TempSum + (ds.Tables[0].Rows[i]["NETROUNDOFF"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["NETROUNDOFF"].ToString()) : 0);
                else
                {
                    BranchNetRoundOff = TempSum;
                    GrandTotalBranchNetRoundOff = GrandTotalBranchNetRoundOff + TempSum;
                    TempSum = decimal.Zero;
                }


                if (ds.Tables[0].Rows[i]["clientname"].ToString().Trim() == GroupOrBranch || ds.Tables[0].Rows[i]["branchname"].ToString().Trim() == "Z")
                {
                    flag = flag + 1;
                    strHtmlAllClient += "<tr id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";


                    if (ds.Tables[0].Rows[i]["ClientName"].ToString() == "Client Total :" || ds.Tables[0].Rows[i]["ClientName"].ToString() == "Branch Net :" || ds.Tables[0].Rows[i]["ClientName"].ToString() == "Exchange Obligation :" || ds.Tables[0].Rows[i]["ClientName"].ToString() == "STT Payable To Exchange :" || ds.Tables[0].Rows[i]["ClientName"].ToString() == "Total :")
                    {
                        strHtmlAllClient += "<td align=\"left\" nowrap=\"nowrap;\"><b>" + ds.Tables[0].Rows[i]["ClientName"].ToString() + "</b></td>";
                    }

                    else if (ds.Tables[0].Rows[i]["ClientName"].ToString() == "Diff (If Any) :")
                    {
                        strHtmlAllClient += "<td style=\"color:maroon;\" align=\"left\" nowrap=\"nowrap;\"><b>" + ds.Tables[0].Rows[i]["ClientName"].ToString() + "</b></td>";
                    }

                    else if (ds.Tables[0].Rows[i]["branchName"].ToString() == "Z")
                    {
                        strHtmlAllClient += "<td align=\"left\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["ClientName"].ToString() + "</td>";
                    }
                    else
                    {
                        strHtmlAllClient += "<td align=\"left\" nowrap=\"nowrap;\" >" + ds.Tables[0].Rows[i]["branchname"].ToString() + "</td>";
                    }






                    if (ds.Tables[0].Rows[i]["branchcode"] != DBNull.Value)
                    {
                        strHtmlAllClient += "<td align=\"right\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["branchcode"].ToString() + "</td>";
                    }
                    else
                    {
                        strHtmlAllClient += "<td>&nbsp;</td>";
                    }


                    if (ds.Tables[0].Rows[i]["DiffObligation"] != DBNull.Value)
                    {
                        strHtmlAllClient += "<td align=\"right\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["DiffObligation"].ToString() + "</td>";
                    }
                    else
                    {
                        strHtmlAllClient += "<td>&nbsp;</td>";
                    }

                    if (ds.Tables[0].Rows[i]["DiffInObligation"] != DBNull.Value)
                    {
                        strHtmlAllClient += "<td align=\"right\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["DiffInObligation"].ToString() + "</td>";
                    }
                    else
                    {
                        strHtmlAllClient += "<td>&nbsp;</td>";
                    }

                    if (ds.Tables[0].Rows[i]["DiffOutObligation"] != DBNull.Value)
                    {
                        strHtmlAllClient += "<td align=\"right\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["DiffOutObligation"].ToString() + "</td>";
                    }
                    else
                    {
                        strHtmlAllClient += "<td>&nbsp;</td>";
                    }

                    if (ds.Tables[0].Rows[i]["NetAmnt"] != DBNull.Value)
                    {
                        strHtmlAllClient += "<td align=\"right\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["NetAmnt"].ToString() + "</td>";
                    }
                    else
                    {
                        strHtmlAllClient += "<td>&nbsp;</td>";
                    }
                    if (ds.Tables[1].Rows[0]["Result_TRANCHARGE"].ToString().Trim() != "0.00")
                    {
                        if (ds.Tables[0].Rows[i]["Trancharge"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td align=\"right\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["Trancharge"].ToString() + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }

                    }
                    if (ds.Tables[1].Rows[0]["Result_STAMPCHARGE"].ToString().Trim() != "0.00")
                    {
                        if (ds.Tables[0].Rows[i]["STAMPCHARGE"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td align=\"right\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["STAMPCHARGE"].ToString() + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }

                    }
                    if (ds.Tables[1].Rows[0]["Result_SRVTAX"].ToString().Trim() != "0.00")
                    {
                        if (ds.Tables[0].Rows[i]["SRVTAX"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td align=\"right\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["SRVTAX"].ToString() + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }

                    }
                    if (ds.Tables[1].Rows[0]["Result_STTAX"].ToString().Trim() != "0.00")
                    {
                        if (ds.Tables[0].Rows[i]["Sttax"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td align=\"right\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["Sttax"].ToString() + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }
                    }
                    if (ds.Tables[1].Rows[0]["Result_SEBICHARGE"].ToString().Trim() != "0.00")
                    {
                        if (ds.Tables[0].Rows[i]["SEBICHARGE"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td align=\"right\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["SEBICHARGE"].ToString() + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }

                    }

                    if (ds.Tables[1].Rows[0]["Result_DIFFBRKG"].ToString().Trim() != "0.00")
                    {
                        if (ds.Tables[0].Rows[i]["DIFFBRKG"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td align=\"right\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["DIFFBRKG"].ToString() + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }

                    }
                    if (ds.Tables[1].Rows[0]["Result_DELIVERYCHARGE"].ToString().Trim() != "0.00")
                    {
                        if (ds.Tables[0].Rows[i]["DELIVERYCHARGE"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td align=\"right\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["DELIVERYCHARGE"].ToString() + "</td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td>&nbsp;</td>";
                        }

                    }


                    if (ds.Tables[1].Rows[0]["Result_NETROUNDOFF"].ToString().Trim() != "0.00")
                    {
                        if (ds.Tables[0].Rows[i]["NetROUNDOFF"] != DBNull.Value)
                        {
                            strHtmlAllClient += "<td align=\"right\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["NetROUNDOFF"].ToString() + "</td>";
                        }
                        else
                        {
                            if (BranchNetRoundOff != 0)
                                strHtmlAllClient += "<td align=\"right\" nowrap=\"nowrap;\">" + BranchNetRoundOff + "</td>";
                            else
                            {
                                GroupOrBranch = ddlType.SelectedItem.Value.ToString().Trim() == "1" || ddlType.SelectedItem.Value.ToString().Trim() == "2" ? "Branch Net :" : "Client Net :";
                                if (ds.Tables[0].Rows[i]["ClientName"].ToString() == GroupOrBranch)
                                    strHtmlAllClient += "<td align=\"right\" nowrap=\"nowrap;\">" + GrandTotalBranchNetRoundOff + "</td>";
                                else
                                    strHtmlAllClient += "<td>&nbsp;</td>";
                            }
                        }
                        BranchNetRoundOff = decimal.Zero;

                    }
                    if (ds.Tables[0].Rows[i]["NetDr"] != DBNull.Value)
                    {
                        if (ds.Tables[0].Rows[i]["ClientName"].ToString() == "Diff (If Any) :")
                        {
                            strHtmlAllClient += "<td style=\"color:maroon;\" align=\"right\" nowrap=\"nowrap;\" ><b>" + ds.Tables[0].Rows[i]["NetDr"].ToString() + "</b></td>";
                        }

                        else if (ds.Tables[0].Rows[i]["ClientName"].ToString() == "Branch Net :" || ds.Tables[0].Rows[i]["ClientName"].ToString() == "Exchange Obligation :" || ds.Tables[0].Rows[i]["ClientName"].ToString() == "STT Payable To Exchange :" || ds.Tables[0].Rows[i]["ClientName"].ToString() == "Total :")
                        {
                            strHtmlAllClient += "<td align=\"right\" nowrap=\"nowrap;\" ><b>" + ds.Tables[0].Rows[i]["NetDr"].ToString() + "</b></td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td align=\"right\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["NetDr"].ToString() + "</td>";
                        }
                    }

                    else
                    {
                        strHtmlAllClient += "<td>&nbsp;</td>";
                    }


                    if (ds.Tables[0].Rows[i]["NetCr"] != DBNull.Value)
                    {
                        if (ds.Tables[0].Rows[i]["ClientName"].ToString() == "Diff (If Any) :")
                        {
                            strHtmlAllClient += "<td style=\"color:maroon;\" align=\"right\" nowrap=\"nowrap;\" ><b>" + ds.Tables[0].Rows[i]["NetCr"].ToString() + "</b></td>";
                        }
                        else if (ds.Tables[0].Rows[i]["ClientName"].ToString() == "Branch Net :" || ds.Tables[0].Rows[i]["ClientName"].ToString() == "Exchange Obligation :" || ds.Tables[0].Rows[i]["ClientName"].ToString() == "STT Payable To Exchange :" || ds.Tables[0].Rows[i]["ClientName"].ToString() == "Total :")
                        {
                            strHtmlAllClient += "<td align=\"right\" nowrap=\"nowrap;\" ><b>" + ds.Tables[0].Rows[i]["NetCr"].ToString() + "</b></td>";
                        }
                        else
                        {
                            strHtmlAllClient += "<td align=\"right\" nowrap=\"nowrap;\">" + ds.Tables[0].Rows[i]["NetCr"].ToString() + "</td>";
                        }
                    }

                    else
                    {
                        strHtmlAllClient += "<td>&nbsp;</td>";
                    }

                    strHtmlAllClient += "</tr>";
                }
            }

            strHtmlAllClient += "</table>";
            DIVdisplayPERIOD.InnerHtml = ViewState["header"].ToString().Trim();
            display.InnerHtml = strHtmlAllClient;

            if (ddlGeneration.SelectedItem.Value == "1")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "display", "displayalldata();", true);
            }
            else
            {
                ViewState["mail"] = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + "><tr style=\"background-color: #DBEEF3;\"><td align=\"center\">" + ViewState["headermail"] + "</td></tr></table>" + strHtmlAllClient;
            }
        }

        void exportclient()
        {
            ds = (DataSet)ViewState["dataset"];

            DataTable dtExport = new DataTable();

            dtExport.Clear();
            int colcount = 0;

            dtExport.Columns.Add("Client Name", Type.GetType("System.String"));
            dtExport.Columns.Add("Code", Type.GetType("System.String"));
            dtExport.Columns.Add("Difference Obligation", Type.GetType("System.String"));
            dtExport.Columns.Add("Delivery In Obligation", Type.GetType("System.String"));
            dtExport.Columns.Add("Delivery Out Obligation", Type.GetType("System.String"));
            dtExport.Columns.Add("Net Obligation", Type.GetType("System.String"));

            colcount = 6;

            if (ds.Tables[1].Rows[0]["Result_TRANCHARGE"].ToString().Trim() != "0.00")
            {

                dtExport.Columns.Add("Tran Charges", Type.GetType("System.String"));
                colcount = 7;
            }
            if (ds.Tables[1].Rows[0]["Result_STAMPCHARGE"].ToString().Trim() != "0.00")
            {
                dtExport.Columns.Add("Stamp Duty", Type.GetType("System.String"));
                colcount = 8;
            }
            if (ds.Tables[1].Rows[0]["Result_SRVTAX"].ToString().Trim() != "0.00")
            {
                dtExport.Columns.Add("Serv Tax & Cess", Type.GetType("System.String"));
                colcount = 9;
            }
            if (ds.Tables[1].Rows[0]["Result_STTAX"].ToString().Trim() != "0.00")
            {
                dtExport.Columns.Add("Sec Tran Charges", Type.GetType("System.String"));
                colcount = 10;
            }
            if (ds.Tables[1].Rows[0]["Result_SEBICHARGE"].ToString().Trim() != "0.00")
            {
                dtExport.Columns.Add("SEBI Fee", Type.GetType("System.String"));
                colcount = 11;
            }
            if (ds.Tables[1].Rows[0]["Result_DIFFBRKG"].ToString().Trim() != "0.00")
            {
                dtExport.Columns.Add("Min.Prcsg Charge", Type.GetType("System.String"));
                colcount = 12;
            }
            if (ds.Tables[1].Rows[0]["Result_DELIVERYCHARGE"].ToString().Trim() != "0.00")
            {
                dtExport.Columns.Add("Delivery Charges", Type.GetType("System.String"));
                colcount = 13;
            }
            if (ds.Tables[1].Rows[0]["Result_NETROUNDOFF"].ToString().Trim() != "0.00")
            {
                dtExport.Columns.Add("Round-Off Adjustment", Type.GetType("System.String"));
                colcount = 14;
            }

            dtExport.Columns.Add("Net Receivable (Dr.)", Type.GetType("System.String"));
            dtExport.Columns.Add("Net Payable (Cr.)", Type.GetType("System.String"));
            colcount = colcount + 2;

            int flag = 0;
            branchname = null;
            decimal TempSum = decimal.Zero;
            decimal GrandTotalBranchNetRoundOff = decimal.Zero;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (ds.Tables[0].Rows[i]["clientname"].ToString() != "Client Net :")
                {
                    if (ds.Tables[0].Rows[i]["NetRoundOff"] != DBNull.Value)
                        TempSum = TempSum + Convert.ToDecimal(ds.Tables[0].Rows[i]["NetRoundOff"].ToString());
                }
                else
                {
                    GrandTotalBranchNetRoundOff = TempSum;
                    TempSum = decimal.Zero;
                }
                if (ddlType.SelectedItem.Value.ToString().Trim() == "1" || ddlType.SelectedItem.Value.ToString().Trim() == "4")
                {
                    GroupOrBranch = ddlType.SelectedItem.Value.ToString().Trim() == "1" ? "Branch Name" : "Group Name";
                    if (branchname != ds.Tables[0].Rows[i]["branchname"].ToString().Trim() + " [ " + ds.Tables[0].Rows[i]["branchcode"] + " ]" && ds.Tables[0].Rows[i]["branchname"].ToString().Trim() != "Z")
                    {
                        DataRow rowbranch = dtExport.NewRow();
                        rowbranch[0] = GroupOrBranch + " : " + ds.Tables[0].Rows[i]["BranchName"].ToString() + " [ " + ds.Tables[0].Rows[i]["branchcode"] + " ]";
                        rowbranch[1] = "Test";
                        dtExport.Rows.Add(rowbranch);
                    }
                }
                branchname = ds.Tables[0].Rows[i]["branchname"].ToString().Trim() + " [ " + ds.Tables[0].Rows[i]["branchcode"] + " ]";

                DataRow row = dtExport.NewRow();
                if (ds.Tables[0].Rows[i]["ClientName"] != DBNull.Value)
                {
                    row["Client Name"] = ds.Tables[0].Rows[i]["ClientName"].ToString();
                }


                if (ds.Tables[0].Rows[i]["Clientucc"] != DBNull.Value)
                {
                    row["Code"] = ds.Tables[0].Rows[i]["Clientucc"].ToString();
                }


                if (ds.Tables[0].Rows[i]["DiffObligation"] != DBNull.Value)
                {
                    row["Difference Obligation"] = ds.Tables[0].Rows[i]["DiffObligation"].ToString();
                }


                if (ds.Tables[0].Rows[i]["DiffInObligation"] != DBNull.Value)
                {
                    row["Delivery In Obligation"] = ds.Tables[0].Rows[i]["DiffInObligation"].ToString();
                }


                if (ds.Tables[0].Rows[i]["DiffOutObligation"] != DBNull.Value)
                {
                    row["Delivery Out Obligation"] = ds.Tables[0].Rows[i]["DiffOutObligation"].ToString();
                }


                if (ds.Tables[0].Rows[i]["NetAmnt"] != DBNull.Value)
                {
                    row["Net Obligation"] = ds.Tables[0].Rows[i]["NetAmnt"].ToString();
                }

                if (ds.Tables[1].Rows[0]["Result_TRANCHARGE"].ToString().Trim() != "0.00")
                {
                    if (ds.Tables[0].Rows[i]["Trancharge"] != DBNull.Value)
                    {
                        row["Tran Charges"] = ds.Tables[0].Rows[i]["Trancharge"].ToString();
                    }


                }
                if (ds.Tables[1].Rows[0]["Result_STAMPCHARGE"].ToString().Trim() != "0.00")
                {
                    if (ds.Tables[0].Rows[i]["STAMPCHARGE"] != DBNull.Value)
                    {
                        row["Stamp Duty"] = ds.Tables[0].Rows[i]["STAMPCHARGE"].ToString();
                    }

                }
                if (ds.Tables[1].Rows[0]["Result_SRVTAX"].ToString().Trim() != "0.00")
                {
                    if (ds.Tables[0].Rows[i]["SRVTAX"] != DBNull.Value)
                    {
                        row["Serv Tax & Cess"] = ds.Tables[0].Rows[i]["SRVTAX"].ToString();
                    }

                }
                if (ds.Tables[1].Rows[0]["Result_STTAX"].ToString().Trim() != "0.00")
                {
                    if (ds.Tables[0].Rows[i]["Sttax"] != DBNull.Value)
                    {
                        row["Sec Tran Charges"] = ds.Tables[0].Rows[i]["Sttax"].ToString();
                    }

                }
                if (ds.Tables[1].Rows[0]["Result_SEBICHARGE"].ToString().Trim() != "0.00")
                {
                    if (ds.Tables[0].Rows[i]["SEBICHARGE"] != DBNull.Value)
                    {
                        row["SEBI Fee"] = ds.Tables[0].Rows[i]["SEBICHARGE"].ToString();
                    }

                }
                if (ds.Tables[1].Rows[0]["Result_DIFFBRKG"].ToString().Trim() != "0.00")
                {
                    if (ds.Tables[0].Rows[i]["DIFFBRKG"] != DBNull.Value)
                    {
                        row["Min.Prcsg Charge"] = ds.Tables[0].Rows[i]["DIFFBRKG"].ToString();
                    }

                }
                if (ds.Tables[1].Rows[0]["Result_DELIVERYCHARGE"].ToString().Trim() != "0.00")
                {
                    if (ds.Tables[0].Rows[i]["DELIVERYCHARGE"] != DBNull.Value)
                    {
                        row["Delivery Charges"] = ds.Tables[0].Rows[i]["DELIVERYCHARGE"].ToString();
                    }

                }
                if (ds.Tables[1].Rows[0]["Result_NETROUNDOFF"].ToString().Trim() != "0.00")
                {
                    if (ds.Tables[0].Rows[i]["NetROUNDOFF"] != DBNull.Value)
                    {
                        row["Round-Off Adjustment"] = ds.Tables[0].Rows[i]["NetROUNDOFF"].ToString();
                    }
                    else
                    {
                        if (ds.Tables[0].Rows[i]["clientname"].ToString() == "Client Net :")
                        {
                            row["Round-Off Adjustment"] = GrandTotalBranchNetRoundOff;
                            GrandTotalBranchNetRoundOff = decimal.Zero;
                        }
                        else
                            row["Round-Off Adjustment"] = "";
                    }
                }
                if (ds.Tables[0].Rows[i]["NetDr"] != DBNull.Value)
                {

                    row["Net Receivable (Dr.)"] = ds.Tables[0].Rows[i]["NetDr"].ToString();

                }


                if (ds.Tables[0].Rows[i]["NetCr"] != DBNull.Value)
                {

                    row["Net Payable (Cr.)"] = ds.Tables[0].Rows[i]["NetCr"].ToString();

                }
                dtExport.Rows.Add(row);
            }



            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            DataTable dtReportHeader = new DataTable();
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            DrRowR1[0] = ViewState["header"].ToString().Trim();
            dtReportHeader.Rows.Add(DrRowR1);
            DataRow DrRowR2 = dtReportHeader.NewRow();
            dtReportHeader.Rows.Add(DrRowR2);
            DataRow HeaderRow1 = dtReportHeader.NewRow();
            dtReportHeader.Rows.Add(HeaderRow1);
            DataRow HeaderRow2 = dtReportHeader.NewRow();
            dtReportHeader.Rows.Add(HeaderRow2);

            DataTable dtReportFooter = new DataTable();
            dtReportFooter.Columns.Add(new DataColumn("Footer", typeof(String))); //0
            DataRow FooterRow1 = dtReportFooter.NewRow();
            dtReportFooter.Rows.Add(FooterRow1);
            DataRow FooterRow2 = dtReportFooter.NewRow();
            dtReportFooter.Rows.Add(FooterRow2);
            DataRow FooterRow = dtReportFooter.NewRow();
            FooterRow[0] = "* * *  End Of Report * * *   ";
            dtReportFooter.Rows.Add(FooterRow);

            if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "1")////screen
            {
                if (ddlExport.SelectedItem.Value == "E")
                {
                    objExcel.ExportToExcelforExcel(dtExport, "Settlement Trial ", "Client Net :", dtReportHeader, dtReportFooter);
                }
                else if (ddlExport.SelectedItem.Value == "P")
                {
                    objExcel.ExportToPDF(dtExport, "Settlement Trial ", "Client Net :", dtReportHeader, dtReportFooter);
                }

            }
            if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "2")////excel
            {
                objExcel.ExportToExcelforExcel(dtExport, "Settlement Trial ", "Client Net :", dtReportHeader, dtReportFooter);
            }

        }
        void exportbranch()
        {
            ds = (DataSet)ViewState["dataset"];

            DataTable dtExport = new DataTable();

            dtExport.Clear();
            int colcount = 0;
            GroupOrBranch = ddlType.SelectedItem.Value.ToString().Trim() == "1" || ddlType.SelectedItem.Value.ToString().Trim() == "2" ? "Branch Name" : "Group Name";
            dtExport.Columns.Add(GroupOrBranch, Type.GetType("System.String"));
            dtExport.Columns.Add("Difference Obligation", Type.GetType("System.String"));
            dtExport.Columns.Add("Delivery In Obligation", Type.GetType("System.String"));
            dtExport.Columns.Add("Delivery Out Obligation", Type.GetType("System.String"));
            dtExport.Columns.Add("Net Obligation", Type.GetType("System.String"));

            colcount = 5;

            if (ds.Tables[1].Rows[0]["Result_TRANCHARGE"].ToString().Trim() != "0.00")
            {

                dtExport.Columns.Add("Tran Charges", Type.GetType("System.String"));
                colcount = 6;
            }
            if (ds.Tables[1].Rows[0]["Result_STAMPCHARGE"].ToString().Trim() != "0.00")
            {
                dtExport.Columns.Add("Stamp Duty", Type.GetType("System.String"));
                colcount = 7;
            }
            if (ds.Tables[1].Rows[0]["Result_SRVTAX"].ToString().Trim() != "0.00")
            {
                dtExport.Columns.Add("Serv Tax & Cess", Type.GetType("System.String"));
                colcount = 8;
            }
            if (ds.Tables[1].Rows[0]["Result_STTAX"].ToString().Trim() != "0.00")
            {
                dtExport.Columns.Add("Sec Tran Charges", Type.GetType("System.String"));
                colcount = 9;
            }
            if (ds.Tables[1].Rows[0]["Result_SEBICHARGE"].ToString().Trim() != "0.00")
            {
                dtExport.Columns.Add("SEBI Fee", Type.GetType("System.String"));
                colcount = 10;
            }
            if (ds.Tables[1].Rows[0]["Result_DIFFBRKG"].ToString().Trim() != "0.00")
            {
                dtExport.Columns.Add("Min.Prcsg Charge", Type.GetType("System.String"));
                colcount = 11;
            }
            if (ds.Tables[1].Rows[0]["Result_DELIVERYCHARGE"].ToString().Trim() != "0.00")
            {
                dtExport.Columns.Add("Delivery Charges", Type.GetType("System.String"));
                colcount = 12;
            }
            if (ds.Tables[1].Rows[0]["Result_NETROUNDOFF"].ToString().Trim() != "0.00")
            {
                dtExport.Columns.Add("Round-Off Adjustment", Type.GetType("System.String"));
                colcount = 13;
            }

            dtExport.Columns.Add("Net Receivable (Dr.)", Type.GetType("System.String"));
            dtExport.Columns.Add("Net Payable (Cr.)", Type.GetType("System.String"));
            colcount = colcount + 2;

            int flag = 0;
            branchname = null;
            decimal TempSum = decimal.Zero;
            decimal BranchNetRoundOff = decimal.Zero;
            decimal GrandTotalBranchNetRoundOff = decimal.Zero;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                GroupOrBranch = ddlType.SelectedItem.Value.ToString().Trim() == "1" || ddlType.SelectedItem.Value.ToString().Trim() == "2" ? "Branch Total :" : "Group Total :";
                if (ds.Tables[0].Rows[i]["clientname"].ToString().Trim() != GroupOrBranch)
                    TempSum = TempSum + (ds.Tables[0].Rows[i]["NETROUNDOFF"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["NETROUNDOFF"].ToString()) : 0);
                else
                {
                    BranchNetRoundOff = TempSum;
                    GrandTotalBranchNetRoundOff = GrandTotalBranchNetRoundOff + TempSum;
                    TempSum = decimal.Zero;
                }
                if (ds.Tables[0].Rows[i]["clientname"].ToString().Trim() == GroupOrBranch || ds.Tables[0].Rows[i]["branchname"].ToString().Trim() == "Z")
                {
                    DataRow row = dtExport.NewRow();
                    if (ds.Tables[0].Rows[i]["ClientName"] != DBNull.Value)
                    {
                        GroupOrBranch = ddlType.SelectedItem.Value.ToString().Trim() == "1" || ddlType.SelectedItem.Value.ToString().Trim() == "2" ? "Branch Name" : "Group Name";
                        if (ds.Tables[0].Rows[i]["branchname"].ToString().Trim() == "Z")
                        {
                            row[GroupOrBranch] = ds.Tables[0].Rows[i]["Clientname"].ToString();
                        }
                        else
                        {
                            row[GroupOrBranch] = ds.Tables[0].Rows[i]["BranchName"].ToString() + " [ " + ds.Tables[0].Rows[i]["branchcode"] + " ]";
                        }
                    }


                    if (ds.Tables[0].Rows[i]["DiffObligation"] != DBNull.Value)
                    {
                        row["Difference Obligation"] = ds.Tables[0].Rows[i]["DiffObligation"].ToString();
                    }


                    if (ds.Tables[0].Rows[i]["DiffInObligation"] != DBNull.Value)
                    {
                        row["Delivery In Obligation"] = ds.Tables[0].Rows[i]["DiffInObligation"].ToString();
                    }


                    if (ds.Tables[0].Rows[i]["DiffOutObligation"] != DBNull.Value)
                    {
                        row["Delivery Out Obligation"] = ds.Tables[0].Rows[i]["DiffOutObligation"].ToString();
                    }


                    if (ds.Tables[0].Rows[i]["NetAmnt"] != DBNull.Value)
                    {
                        row["Net Obligation"] = ds.Tables[0].Rows[i]["NetAmnt"].ToString();
                    }

                    if (ds.Tables[1].Rows[0]["Result_TRANCHARGE"].ToString().Trim() != "0.00")
                    {
                        if (ds.Tables[0].Rows[i]["Trancharge"] != DBNull.Value)
                        {
                            row["Tran Charges"] = ds.Tables[0].Rows[i]["Trancharge"].ToString();
                        }


                    }
                    if (ds.Tables[1].Rows[0]["Result_STAMPCHARGE"].ToString().Trim() != "0.00")
                    {
                        if (ds.Tables[0].Rows[i]["STAMPCHARGE"] != DBNull.Value)
                        {
                            row["Stamp Duty"] = ds.Tables[0].Rows[i]["STAMPCHARGE"].ToString();
                        }

                    }
                    if (ds.Tables[1].Rows[0]["Result_SRVTAX"].ToString().Trim() != "0.00")
                    {
                        if (ds.Tables[0].Rows[i]["SRVTAX"] != DBNull.Value)
                        {
                            row["Serv Tax & Cess"] = ds.Tables[0].Rows[i]["SRVTAX"].ToString();
                        }

                    }
                    if (ds.Tables[1].Rows[0]["Result_STTAX"].ToString().Trim() != "0.00")
                    {
                        if (ds.Tables[0].Rows[i]["Sttax"] != DBNull.Value)
                        {
                            row["Sec Tran Charges"] = ds.Tables[0].Rows[i]["Sttax"].ToString();
                        }

                    }
                    if (ds.Tables[1].Rows[0]["Result_SEBICHARGE"].ToString().Trim() != "0.00")
                    {
                        if (ds.Tables[0].Rows[i]["SEBICHARGE"] != DBNull.Value)
                        {
                            row["SEBI Fee"] = ds.Tables[0].Rows[i]["SEBICHARGE"].ToString();
                        }

                    }
                    if (ds.Tables[1].Rows[0]["Result_DIFFBRKG"].ToString().Trim() != "0.00")
                    {
                        if (ds.Tables[0].Rows[i]["DIFFBRKG"] != DBNull.Value)
                        {
                            row["Min.Prcsg Charge"] = ds.Tables[0].Rows[i]["DIFFBRKG"].ToString();
                        }

                    }
                    if (ds.Tables[1].Rows[0]["Result_DELIVERYCHARGE"].ToString().Trim() != "0.00")
                    {
                        if (ds.Tables[0].Rows[i]["DELIVERYCHARGE"] != DBNull.Value)
                        {
                            row["Delivery Charges"] = ds.Tables[0].Rows[i]["DELIVERYCHARGE"].ToString();
                        }

                    }
                    if (ds.Tables[1].Rows[0]["Result_NETROUNDOFF"].ToString().Trim() != "0.00")
                    {
                        if (ds.Tables[0].Rows[i]["NetROUNDOFF"] != DBNull.Value)
                        {
                            row["Round-Off Adjustment"] = ds.Tables[0].Rows[i]["NetROUNDOFF"].ToString();
                        }
                        else
                        {
                            if (BranchNetRoundOff != 0)
                                row["Round-Off Adjustment"] = BranchNetRoundOff;
                            else
                            {
                                GroupOrBranch = ddlType.SelectedItem.Value.ToString().Trim() == "1" || ddlType.SelectedItem.Value.ToString().Trim() == "2" ? "Branch Net :" : "Client Net :";
                                if (ds.Tables[0].Rows[i]["ClientName"].ToString() == GroupOrBranch)
                                    row["Round-Off Adjustment"] = GrandTotalBranchNetRoundOff;
                                else
                                    row["Round-Off Adjustment"] = "";
                            }
                        }
                        BranchNetRoundOff = decimal.Zero;
                    }
                    if (ds.Tables[0].Rows[i]["NetDr"] != DBNull.Value)
                    {

                        row["Net Receivable (Dr.)"] = ds.Tables[0].Rows[i]["NetDr"].ToString();

                    }


                    if (ds.Tables[0].Rows[i]["NetCr"] != DBNull.Value)
                    {

                        row["Net Payable (Cr.)"] = ds.Tables[0].Rows[i]["NetCr"].ToString();
                    }

                    dtExport.Rows.Add(row);
                }


            }


            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            DataTable dtReportHeader = new DataTable();
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            DrRowR1[0] = ViewState["header"].ToString().Trim();
            dtReportHeader.Rows.Add(DrRowR1);
            DataRow DrRowR2 = dtReportHeader.NewRow();
            dtReportHeader.Rows.Add(DrRowR2);
            DataRow HeaderRow1 = dtReportHeader.NewRow();
            dtReportHeader.Rows.Add(HeaderRow1);
            DataRow HeaderRow2 = dtReportHeader.NewRow();
            dtReportHeader.Rows.Add(HeaderRow2);

            DataTable dtReportFooter = new DataTable();
            dtReportFooter.Columns.Add(new DataColumn("Footer", typeof(String))); //0
            DataRow FooterRow1 = dtReportFooter.NewRow();
            dtReportFooter.Rows.Add(FooterRow1);
            DataRow FooterRow2 = dtReportFooter.NewRow();
            dtReportFooter.Rows.Add(FooterRow2);
            DataRow FooterRow = dtReportFooter.NewRow();
            FooterRow[0] = "* * *  End Of Report * * *   ";
            dtReportFooter.Rows.Add(FooterRow);

            if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "1")////screen
            {
                if (ddlExport.SelectedItem.Value == "E")
                {
                    objExcel.ExportToExcelforExcel(dtExport, "Settlement Trial ", "Client Net", dtReportHeader, dtReportFooter);
                }
                else if (ddlExport.SelectedItem.Value == "P")
                {
                    objExcel.ExportToPDF(dtExport, "Settlement Trial ", "Client Net", dtReportHeader, dtReportFooter);
                }

            }
            if (ddlGeneration.SelectedItem.Value.ToString().Trim() == "2")////excel
            {
                objExcel.ExportToExcelforExcel(dtExport, "Settlement Trial ", "Client Net", dtReportHeader, dtReportFooter);
            }
        }


        protected void btnScreen_Click(object sender, EventArgs e)
        {

            procedure();
            ds = (DataSet)ViewState["dataset"];
            if (ds.Tables.Count > 0)
            {

                if (ddlType.SelectedItem.Value.ToString().Trim() == "2" || ddlType.SelectedItem.Value.ToString().Trim() == "5")
                {
                    htmltableBranchWise();
                }
                else
                {
                    htmltableClientWise();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "norecord", "NoRecord();", true);

            }
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            procedure();
            ds = (DataSet)ViewState["dataset"];
            if (ds.Tables.Count > 0)
            {
                if (ddlType.SelectedItem.Value.ToString().Trim() == "1" || ddlType.SelectedItem.Value.ToString().Trim() == "4" || ddlType.SelectedItem.Value.ToString().Trim() == "3")
                {
                    exportclient();
                }
                else
                {
                    exportbranch();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "norecord", "NoRecord();", true);

            }

        }

        protected void ddlExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlType.SelectedItem.Value == "2")
            {
                exportbranch();
            }
            else
            {

                exportclient();
            }
        }

        protected void btnMail_Click(object sender, EventArgs e)
        {
            procedure();
            if (ddlType.SelectedItem.Value.ToString().Trim() == "2")
            {
                htmltableBranchWise();
            }
            else
            {
                htmltableClientWise();
            }
            string[] clnt = HiddenField_emmail.Value.ToString().Split(',');
            int kk = clnt.Length;
            for (int i = 0; i < clnt.Length; i++)
            {
                if (oDBEngine.SendReportSt(ViewState["mail"].ToString().Trim(), clnt[i].ToString().Trim(), ViewState["headermail"].ToString().Trim(), ViewState["headermail"].ToString().Trim()) == true)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>alert('Mail Send Successfully!');</script>");
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "PageScript", "<script language='javascript'>alert('Error Sending Mail Please Try Again!');</script>");
                }
            }
        }


        protected void btnPDF_Click(object sender, EventArgs e)
        {
            procedure();
            DataSet dsReport = (DataSet)ViewState["dataset"];
            if (ds.Tables[0].Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD3", "NORECORD(1);", true);
            }
            else
            {

                //ds.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\SettlementTrialNSECM.xsd");
                string path = HttpContext.Current.Server.MapPath("..\\Reports\\SettlementTrialNSECM.rpt");
                ReportDocument.Load(path);
                ReportDocument.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
                ReportDocument.SetDataSource(dsReport.Tables[0]);
                ReportDocument.Subreports["SettlementTrialHeader"].SetDataSource(dsReport.Tables[2]);
                ReportDocument.SetParameterValue("@ShowDate", (object)ViewState["header"]);
                ReportDocument.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                ReportDocument.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "Settlement Trial");
            }
        }
    }
}