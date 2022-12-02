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
    public partial class Reports_frmReportExchangeObligation : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        DailyReports dailyrep = new DailyReports();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        ClsDropDownlistNameSpace.clsDropDownList clsdrp = new ClsDropDownlistNameSpace.clsDropDownList();
        static string[] idlist;
        static DataSet ds = new DataSet();
        int pageindex = 0;
        int pagecount = 0;
        int pageSize;
        int rowcount = 0;

        //static decimal LOTSRESULT_B_Sum;
        static decimal BFVALUE_Sum;
        static decimal BUYVALUE_Sum;
        static decimal SELLVALUE_Sum;
        static decimal CFVALUE_Sum;
        static decimal MTMPRM_Sum;
        static decimal FINSETT_Sum;
        static decimal NETOBLIGATION_Sum;

        //for email
        string data;
        static string SubClients = "";
        DataSet tblEmail;


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

            if (!IsPostBack)
            {
                date();
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");

            }

            //for sending email
            Page.ClientScript.RegisterStartupScript(GetType(), "JScript2", "<script language='javascript'>ForFilterOff();</script>");
            txtSelectionID.Attributes.Add("onkeyup", "callAjax1(this,'GetMailId',event)");
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            FillCombo();
        }
        void date()
        {
            dtFor.EditFormatString = oconverter.GetDateFormat("Date");

            string[] idlist = oDBEngine.GetDate().GetDateTimeFormats();
            dtFor.Value = Convert.ToDateTime(idlist[2]);
            dtFor.Value = Convert.ToDateTime(idlist[2]);
            dtFor_hidden.Text = Convert.ToDateTime(idlist[2]).ToString();

        }
        protected void btn_show_Click(object sender, EventArgs e)
        {
            bindgrid();
        }
        void bindgrid()
        {
            //ViewState["LOTSRESULT_B_Sum"] = null;
            //LOTSRESULT_B_Sum = 0;
            ViewState["BFVALUE_Sum"] = null;
            BFVALUE_Sum = 0;
            ViewState["BUYVALUE_Sum"] = null;
            BUYVALUE_Sum = 0;
            ViewState["SELLVALUE_Sum"] = null;
            SELLVALUE_Sum = 0;
            ViewState["CFVALUE_Sum"] = null;
            CFVALUE_Sum = 0;
            ViewState["MTMPRM_Sum"] = null;
            MTMPRM_Sum = 0;
            ViewState["FINSETT_Sum"] = null;
            FINSETT_Sum = 0;
            ViewState["NETOBLIGATION_Sum"] = null;
            NETOBLIGATION_Sum = 0;

            ds = dailyrep.Sp_ExchangeObligationCOMM(dtFor.Value.ToString(), Session["usersegid"].ToString(),
                Convert.ToInt32(HttpContext.Current.Session["ExchangeSegmentID"].ToString()), Session["LastCompany"].ToString(), "All");
            tblEmail = ds;
            int tabelrow = ds.Tables.Count;
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[tabelrow - 1].Rows[0]["status"].ToString() == "1")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "hi1", "Message();", true);
                }
                else
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (ds.Tables[0].Rows[i]["SettlementPrice"] != DBNull.Value)
                            ds.Tables[0].Rows[i]["SettlementPrice"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["SettlementPrice"].ToString()));

                        if (ds.Tables[0].Rows[i]["BFVALUE"] != DBNull.Value)
                            ds.Tables[0].Rows[i]["BFVALUE"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["BFVALUE"].ToString()));

                        if (ds.Tables[0].Rows[i]["BUYVALUE"] != DBNull.Value)
                            ds.Tables[0].Rows[i]["BUYVALUE"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["BUYVALUE"].ToString()));

                        if (ds.Tables[0].Rows[i]["SELLVALUE"] != DBNull.Value)
                            ds.Tables[0].Rows[i]["SELLVALUE"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["SELLVALUE"].ToString()));

                        if (ds.Tables[0].Rows[i]["SETTPRICE"] != DBNull.Value)
                            ds.Tables[0].Rows[i]["SETTPRICE"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["SETTPRICE"].ToString()));

                        if (ds.Tables[0].Rows[i]["CFVALUE"] != DBNull.Value)
                            ds.Tables[0].Rows[i]["CFVALUE"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["CFVALUE"].ToString()));

                        if (ds.Tables[0].Rows[i]["MTMPRM"] != DBNull.Value)
                            ds.Tables[0].Rows[i]["MTMPRM"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["MTMPRM"].ToString()));

                        if (ds.Tables[0].Rows[i]["NETOBLIGATION"] != DBNull.Value)
                            ds.Tables[0].Rows[i]["NETOBLIGATION"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["NETOBLIGATION"].ToString()));

                    }

                    pageSize = 15;
                    pagecount = ds.Tables[0].Rows.Count / pageSize + 1;
                    TotalPages.Value = pagecount.ToString();
                    grdExchange.PageIndex = pageindex;
                    CurrentPage.Value = pageindex.ToString();
                    if (pageindex <= 0)
                    {
                        pageindex = 0;


                        ScriptManager.RegisterStartupScript(this, this.GetType(), "hide1", "DisableExchange('P');", true);
                    }
                    if (pageindex >= int.Parse(TotalPages.Value.ToString()))
                    {
                        pageindex = int.Parse(TotalPages.Value.ToString()) - 1;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "hide1", "DisableExchange('N');", true);
                    }
                    if (pageindex >= (int.Parse(TotalPages.Value.ToString()) - 1))
                    {
                        pageindex = int.Parse(TotalPages.Value.ToString()) - 1;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "hide1", "DisableExchange('N');", true);
                    }

                    BFVALUE_Sum += Convert.ToDecimal(ds.Tables[1].Rows[0]["BFVALUE_Sum"]);
                    ViewState["BFVALUE_Sum"] = BFVALUE_Sum;

                    BUYVALUE_Sum += Convert.ToDecimal(ds.Tables[1].Rows[0]["BUYVALUE_Sum"]);
                    ViewState["BUYVALUE_Sum"] = BUYVALUE_Sum;

                    SELLVALUE_Sum += Convert.ToDecimal(ds.Tables[1].Rows[0]["SELLVALUE_Sum"]);
                    ViewState["SELLVALUE_Sum"] = SELLVALUE_Sum;

                    CFVALUE_Sum += Convert.ToDecimal(ds.Tables[1].Rows[0]["CFVALUE_Sum"]);
                    ViewState["CFVALUE_Sum"] = CFVALUE_Sum;

                    MTMPRM_Sum += Convert.ToDecimal(ds.Tables[1].Rows[0]["MTMPRM_Sum"]);
                    ViewState["MTMPRM_Sum"] = MTMPRM_Sum;

                    FINSETT_Sum += Convert.ToDecimal(ds.Tables[1].Rows[0]["FINSETT_Sum"]);
                    ViewState["FINSETT_Sum"] = FINSETT_Sum;

                    NETOBLIGATION_Sum += Convert.ToDecimal(ds.Tables[1].Rows[0]["NETOBLIGATION_Sum"]);
                    ViewState["NETOBLIGATION_Sum"] = NETOBLIGATION_Sum;


                    if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "14")
                    {
                        grdExchange.Columns[1].Visible = false;
                        grdExchange.Columns[2].Visible = false;
                        grdExchange.Columns[3].Visible = false;
                        grdExchange.Columns[4].Visible = false;
                        grdExchange.Columns[9].HeaderText = "Dlv Qty";
                        grdExchange.Columns[11].HeaderText = "Dlv Value";
                        grdExchange.Columns[12].HeaderText = "MTM";
                        grdExchange.Columns[13].Visible = false;
                    }
                    grdExchange.DataSource = ds;
                    grdExchange.DataBind();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "line", "line();", true);

                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "hi", "NoRecord()", true);
            }
        }
        protected void NavigationLink_Click(Object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "First":
                    pageindex = 0;
                    break;
                case "Next":
                    pageindex = int.Parse(CurrentPage.Value) + 1;
                    break;
                case "Prev":
                    if (int.Parse(CurrentPage.Value) == 0)
                        pageindex = 0;
                    else
                        pageindex = int.Parse(CurrentPage.Value) - 1;
                    break;
                case "Last":
                    pageindex = int.Parse(TotalPages.Value);
                    break;
                default:
                    pageindex = int.Parse(e.CommandName.ToString());
                    break;
            }

            bindgrid();
        }


        protected void grdExchange_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                //Label lblLOTSRESULT_B_Sum = (Label)e.Row.FindControl("lblLOTSRESULT_B_Sum");
                Label lblBFVALUE_Sum = (Label)e.Row.FindControl("lblBFVALUE_Sum");
                Label lblBUYVALUE_Sum = (Label)e.Row.FindControl("lblBUYVALUE_Sum");
                Label lblSELLVALUE_Sum = (Label)e.Row.FindControl("lblSELLVALUE_Sum");
                Label lblCFVALUE_Sum = (Label)e.Row.FindControl("lblCFVALUE_Sum");
                Label lblMTMPRM_Sum = (Label)e.Row.FindControl("lblMTMPRM_Sum");
                Label lblFINSETT_Sum = (Label)e.Row.FindControl("lblFINSETT_Sum");
                Label lblNETOBLIGATION_Sum = (Label)e.Row.FindControl("lblNETOBLIGATION_Sum");


                //lblLOTSRESULT_B_Sum.Text = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ViewState["LOTSRESULT_B_Sum"].ToString()));
                lblBFVALUE_Sum.Text = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["BFVALUE_Sum"].ToString()));
                lblBUYVALUE_Sum.Text = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["BUYVALUE_Sum"].ToString()));
                lblSELLVALUE_Sum.Text = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["SELLVALUE_Sum"].ToString()));
                lblCFVALUE_Sum.Text = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ViewState["CFVALUE_Sum"].ToString()));
                lblMTMPRM_Sum.Text = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["MTMPRM_Sum"].ToString()));
                lblFINSETT_Sum.Text = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["FINSETT_Sum"].ToString()));
                lblNETOBLIGATION_Sum.Text = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["NETOBLIGATION_Sum"].ToString()));

            }
        }
        protected void grdExchange_RowCreated(object sender, GridViewRowEventArgs e)
        {
            string rowID = String.Empty;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                rowID = "row" + e.Row.RowIndex;
                e.Row.Attributes.Add("id", "row" + e.Row.RowIndex);
                e.Row.Attributes.Add("onclick", "ChangeRowColorgrid(" + "'" + rowID + "','" + ds.Tables[0].Rows.Count + "'" + ")");
            }
        }
        protected void Export_Click(object sender, EventArgs e)
        {
            export();
        }
        void export()
        {
            DataTable dt = new DataTable();
            dt = ds.Tables[2];

            DataTable dt1 = new DataTable();
            dt1 = ds.Tables[3];

            DataRow row = dt.NewRow();
            row["Instrument"] = "Total :";

            if (dt1.Rows[0]["BFVALUE_Sum"] != DBNull.Value)
                row["BFValue"] = Convert.ToDecimal(dt1.Rows[0]["BFVALUE_Sum"]);

            if (dt1.Rows[0]["BUYVALUE_Sum"] != DBNull.Value)
                row["BuyValue"] = Convert.ToDecimal(dt1.Rows[0]["BUYVALUE_Sum"]);

            if (dt1.Rows[0]["SELLVALUE_Sum"] != DBNull.Value)
                row["SellValue"] = Convert.ToDecimal(dt1.Rows[0]["SELLVALUE_Sum"]);

            if (dt1.Rows[0]["CFVALUE_Sum"] != DBNull.Value)
                row["CFValue"] = Convert.ToDecimal(dt1.Rows[0]["CFVALUE_Sum"]);

            if (dt1.Rows[0]["MTMPRM_Sum"] != DBNull.Value)
                row["MtmPrm"] = Convert.ToDecimal(dt1.Rows[0]["MTMPRM_Sum"]);

            if (dt1.Rows[0]["FINSETT_Sum"] != DBNull.Value)
                row["SellValue"] = Convert.ToDecimal(dt1.Rows[0]["FINSETT_Sum"]);

            if (dt1.Rows[0]["MTMPRM_Sum"] != DBNull.Value)
                row["MtmPrm"] = Convert.ToDecimal(dt1.Rows[0]["MTMPRM_Sum"]);

            if (dt1.Rows[0]["NETOBLIGATION_Sum"] != DBNull.Value)
                row["NetObligation"] = Convert.ToDecimal(dt1.Rows[0]["NETOBLIGATION_Sum"]);

            dt.Rows.Add(row);
            if (HttpContext.Current.Session["ExchangeSegmentID"].ToString() == "14")
            {
                dt.Columns.Remove("ExpiryDate");
                dt.Columns.Remove("BFQtyLots");
                dt.Columns.Remove("OpenPrice");
                dt.Columns.Remove("BFValue");
                dt.Columns.Remove("FinSett");
                dt.Columns["CFQtyLots"].ColumnName = "DlvQty";
                dt.Columns["CFValue"].ColumnName = "DlvValue";
                dt.Columns["MtmPrm"].ColumnName = "Mtm";
            }
            DataTable dtSegment = new DataTable();
            dtSegment = ds.Tables[4].Copy();

            oconverter.ExportWithDatatable(dt, dtSegment, "Net Position");
        }


        //--------------------Region for sending email---------------

        private void FillCombo()
        {
            if (HttpContext.Current.Session["userlastsegment"].ToString() == "4")
            {
                string[,] r = new string[1, 2];
                r[0, 0] = "EM";
                r[0, 1] = "Employees";
                clsdrp.AddDataToDropDownList(r, cmbsearchOption);
            }
            else
            {
                string[,] r = new string[1, 2];
                r[0, 0] = "EM";
                r[0, 1] = "Employees";
                //r[1, 0] = "CL";
                //r[1, 1] = "Customers";
                //r[2, 0] = "LD";
                //r[2, 1] = "Lead";
                //r[3, 0] = "CD";
                //r[3, 1] = "CDSL Client";
                //r[4, 0] = "ND";
                //r[4, 1] = "NSDL Client";
                //r[5, 0] = "BP";
                //r[5, 1] = "Business Partne";
                //r[6, 0] = "RA";
                //r[6, 1] = "Relationship Partner";
                //r[7, 0] = "SB";
                //r[7, 1] = "Sub Broker";
                //r[8, 0] = "FR";
                //r[8, 1] = "Franchisees";
                clsdrp.AddDataToDropDownList(r, cmbsearchOption);

            }

        }
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] idlist = id.Split('~');
            string str = "";
            string str1 = "";
            if (idlist[0] == "ComboChange")
            {
                //MainAcID = idlist[1];
            }
            else
            {
                string[] cl = idlist[1].Split(',');
                for (int i = 0; i < cl.Length; i++)
                {
                    string[] val = cl[i].Split(';');
                    string[] AcVal = val[0].Split('-');
                    if (str == "")
                    {
                        str = AcVal[0];
                        str1 = "'" + AcVal[0] + "'" + ";" + val[1];
                    }
                    else
                    {
                        str += "," + AcVal[0];
                        str1 += "," + "'" + AcVal[0] + "'" + ";" + val[1];
                    }

                    SubClients = str;
                    data = str1;
                }
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript3", "MailsendT();", true);
            }
        }


        string ICallbackEventHandler.GetCallbackResult()
        {


            return SubClients;
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            bindgrid();
            //DataTable ds = tblEmail.Tables[0];
            DataSet ds = tblEmail;
            string disptbl = "";
            string SpanText = oconverter.ArrangeDate2(dtFor.Value.ToString());
            if (SubClients != "")
            {
                string[] clnt = SubClients.ToString().Split(',');
                int k = clnt.Length;
                for (int j = 0; j < clnt.Length; j++)
                {
                    disptbl = "<table width=\"1050px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\"><tr style=\"background-color:#BB694D;color:White;\"><td  align=\"center\">Instrument</td><td align=\"center\">Expiry Date</td><td align=\"center\">B/F Qty(Lots)</td><td align=\"center\">Open Price</td><td align=\"center\">B/F Value</td><td align=\"center\">Day Buy(Lots)</td><td align=\"center\">Buy Value</td><td align=\"center\">Day Sell(Lots)</td><td align=\"center\">Sell Value</td><td align=\"center\">C/F Qty(Lots)</td><td align=\"center\">Sett Price</td><td align=\"center\">C/F Value</td><td align=\"center\">Mtm/Prm</td><td align=\"center\">Fin Sett</td><td align=\"center\">Net Obligation</td></tr>";
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (ds.Tables[0].Rows[i]["SettlementPrice"] != DBNull.Value)
                            ds.Tables[0].Rows[i]["SettlementPrice"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["SettlementPrice"].ToString()));

                        if (ds.Tables[0].Rows[i]["BFVALUE"] != DBNull.Value)
                            ds.Tables[0].Rows[i]["BFVALUE"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["BFVALUE"].ToString()));

                        if (ds.Tables[0].Rows[i]["BUYVALUE"] != DBNull.Value)
                            ds.Tables[0].Rows[i]["BUYVALUE"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["BUYVALUE"].ToString()));

                        if (ds.Tables[0].Rows[i]["SELLVALUE"] != DBNull.Value)
                            ds.Tables[0].Rows[i]["SELLVALUE"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["SELLVALUE"].ToString()));

                        if (ds.Tables[0].Rows[i]["SETTPRICE"] != DBNull.Value)
                            ds.Tables[0].Rows[i]["SETTPRICE"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["SETTPRICE"].ToString()));

                        if (ds.Tables[0].Rows[i]["CFVALUE"] != DBNull.Value)
                            ds.Tables[0].Rows[i]["CFVALUE"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["CFVALUE"].ToString()));

                        if (ds.Tables[0].Rows[i]["MTMPRM"] != DBNull.Value)
                            ds.Tables[0].Rows[i]["MTMPRM"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["MTMPRM"].ToString()));

                        if (ds.Tables[0].Rows[i]["NETOBLIGATION"] != DBNull.Value)
                            ds.Tables[0].Rows[i]["NETOBLIGATION"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["NETOBLIGATION"].ToString()));

                        disptbl += "<tr><td>&nbsp;" + ds.Tables[0].Rows[i]["tabSymbol"] + "</td><td>&nbsp;" + ds.Tables[0].Rows[i]["Commodity_ExpiryDate"] + "</td><td align=\"right\">&nbsp;" + ds.Tables[0].Rows[i]["LOTSRESULT_B"] + "</td><td align=\"right\">&nbsp;" + ds.Tables[0].Rows[i]["SettlementPrice"] + "</td><td align=\"right\">&nbsp;" + ds.Tables[0].Rows[i]["BFVALUE"] + "</td><td align=\"right\">&nbsp;" + ds.Tables[0].Rows[i]["DAYBUY"] + "</td><td align=\"right\">&nbsp;" + ds.Tables[0].Rows[i]["BUYVALUE"] + "</td><td align=\"right\">&nbsp;" + ds.Tables[0].Rows[i]["DAYSELL"] + "</td><td align=\"right\">&nbsp;" + ds.Tables[0].Rows[i]["SELLVALUE"] + "</td><td align=\"right\">&nbsp;" + ds.Tables[0].Rows[i]["CFQTY_I"] + "</td><td align=\"right\">&nbsp;" + ds.Tables[0].Rows[i]["SETTPRICE"] + "</td><td align=\"right\">&nbsp;" + ds.Tables[0].Rows[i]["CFVALUE"] + "</td><td align=\"right\">&nbsp;" + ds.Tables[0].Rows[i]["MTMPRM"] + "</td><td align=\"right\">&nbsp;" + ds.Tables[0].Rows[i]["FINSETT"] + "</td><td align=\"right\">&nbsp;" + ds.Tables[0].Rows[i]["NETOBLIGATION"] + "</td></tr>";
                    }
                    disptbl += "<tr style=\"background-color: #FFD4AA; color: Black;\"><td>Total:</td><td>&nbsp;</td><td align=\"right\">&nbsp;</td><td align=\"right\">&nbsp;</td><td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["BFVALUE_Sum"].ToString())) + "</td><td align=\"right\">&nbsp;</td><td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["BUYVALUE_Sum"].ToString())) + "</td><td align=\"right\">&nbsp;</td><td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["SELLVALUE_Sum"].ToString())) + "</td><td align=\"right\">&nbsp;</td><td align=\"right\">&nbsp;</td><td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ViewState["CFVALUE_Sum"].ToString())) + "</td><td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["MTMPRM_Sum"].ToString())) + "</td><td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ViewState["FINSETT_Sum"].ToString())) + "</td><td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["NETOBLIGATION_Sum"].ToString())) + "</td></tr>";
                    disptbl += "</table>";

                    string emailbdy = disptbl;
                    string contactid = clnt[j].ToString();
                    string billdate = oconverter.ArrangeDate2(dtFor.Value.ToString());
                    //  string billdate = oconverter.ArrangeDate2(DateTime.Today.ToString());
                    string Subject = "Exchange Obligation Report For  " + billdate;
                    if (oDBEngine.SendReportSt(emailbdy, contactid, billdate, Subject) == true)
                    {
                        //Page.ClientScript.RegisterStartupScript(GetType(), "jscript6", "<script>ForFilterOff();</script>");
                        //Page.ClientScript.RegisterStartupScript(GetType(), "jscript12", "<script>displaydate('" + SpanText + "');</script>");
                        Page.ClientScript.RegisterStartupScript(GetType(), "jscript7", "<script>MailsendT();</script>");
                    }
                    else
                    {
                        //Page.ClientScript.RegisterStartupScript(GetType(), "jscript8", "<script>ForFilterOff();</script>");
                        //Page.ClientScript.RegisterStartupScript(GetType(), "jscript12", "<script>displaydate('" + SpanText + "');</script>");
                        Page.ClientScript.RegisterStartupScript(GetType(), "jscript9", "<script>MailsendF();</script>");
                    }
                }

            }
            else
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "jscript10", "<script>ForFilterOff();</script>");
                //Page.ClientScript.RegisterStartupScript(GetType(), "jscript12", "<script>displaydate('" + SpanText + "');</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "jscript11", "<script>MailsendFT();</script>");
            }
            //Page.ClientScript.RegisterStartupScript(GetType(), "jscript12", "<script>ForFilterOff();</script>");
        }

    }
}