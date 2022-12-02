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
    public partial class Reports_frmReportExchangeObligationFO : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        ExcelFile objExcel = new ExcelFile();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        ClsDropDownlistNameSpace.clsDropDownList clsdrp = new ClsDropDownlistNameSpace.clsDropDownList();
        DailyReports dailyrep = new DailyReports();
        //DBEngine oDBEngine = new DBEngine(string.Empty);
        static string[] idlist;
        DataSet ds = new DataSet();
        int pageindex = 0;
        int pagecount = 0;
        int pageSize;
        int rowcount = 0;

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

            if (!IsPostBack)
            {

                date();
                fn_cmbunderlying();
                fn_cmbexpiry();
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");

            }

            //_____For performing operation without refreshing page___//

            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//


            //for sending email
            txtSelectID.Attributes.Add("onkeyup", "callAjax1(this,'GetMailId',event)");
            String cbReference1 = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveSvrData", "context");
            String callbackScript1 = "function CallServer1(arg, context){ " + cbReference1 + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer1", callbackScript1, true);
            FillCombo();


        }
        void date()
        {
            dtFor.EditFormatString = oconverter.GetDateFormat("Date");
            dtFor.Value = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] idlist = id.Split('~');
            string str = "";
            string str1 = "";


            string[] cl = idlist[1].Split(',');
            for (int i = 0; i < cl.Length; i++)
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

            if (idlist[0] == "UNDERLYING")
            {
                data = "UNDERLYING~" + str;
            }
            else
            {
                SubClients = str;
            }
            //fn_cmbexpiry();

        }

        void fn_cmbexpiry()
        {
            if (rdbunderlyingAll.Checked)
            {
                fn_cmbunderlying();
            }
            cmbexpiry.Items.Clear();
            DataTable dtExpiry = new DataTable();
            if (HiddenField_Product.Value != "")
            {
                dtExpiry = oDBEngine.GetDataTable("master_equity", "distinct convert(varchar(9),Equity_EffectUntil,6) as expirydate,Equity_EffectUntil", "equity_exchsegmentid='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and Equity_ProductID in(" + HiddenField_Product.Value + ") and Equity_EffectUntil>='" + dtFor.Value + "'", " Equity_EffectUntil");
            }
            else
            { dtExpiry = oDBEngine.GetDataTable("master_equity", "distinct convert(varchar(9),Equity_EffectUntil,6) as expirydate,Equity_EffectUntil", "equity_exchsegmentid='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and Equity_EffectUntil>='" + dtFor.Value + "'", " Equity_EffectUntil"); }
            if (dtExpiry.Rows.Count > 0)
            {
                cmbexpiry.DataSource = dtExpiry;
                cmbexpiry.DataValueField = "Equity_EffectUntil";
                cmbexpiry.DataTextField = "expirydate";
                cmbexpiry.DataBind();
                cmbexpiry.Items.Insert(0, "ALL");
            }
            else
            {
                cmbexpiry.Items.Insert(0, "ALL");
            }

        }

        void fn_cmbunderlying()
        {
            string UNDERLYING = null;
            if (cmbinstrutype.SelectedItem.Value == "0")
            {
                DataTable dtunderlying = oDBEngine.GetDataTable("master_equity", "distinct Equity_ProductID", "equity_exchsegmentid='" + HttpContext.Current.Session["ExchangeSegmentID"] + "'", " Equity_ProductID");
                if (dtunderlying.Rows.Count > 0)
                {
                    UNDERLYING = null;
                    for (int i = 0; i < dtunderlying.Rows.Count; i++)
                    {
                        if (UNDERLYING == null)
                            UNDERLYING = dtunderlying.Rows[i][0].ToString();
                        else
                            UNDERLYING += "," + dtunderlying.Rows[i][0].ToString();
                    }
                }
            }
            else if (cmbinstrutype.SelectedItem.Value == "1")
            {
                DataTable dtunderlying = oDBEngine.GetDataTable("master_equity", "distinct Equity_ProductID", "equity_exchsegmentid='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and (Equity_FOIdentifier='FUTSTK' or Equity_FOIdentifier='OPTSTK')", " Equity_ProductID");
                if (dtunderlying.Rows.Count > 0)
                {
                    UNDERLYING = null;
                    for (int i = 0; i < dtunderlying.Rows.Count; i++)
                    {
                        if (UNDERLYING == null)
                            UNDERLYING = dtunderlying.Rows[i][0].ToString();
                        else
                            UNDERLYING += "," + dtunderlying.Rows[i][0].ToString();
                    }
                }
            }
            else if (cmbinstrutype.SelectedItem.Value == "2")
            {
                DataTable dtunderlying = oDBEngine.GetDataTable("master_equity", "distinct Equity_ProductID", "equity_exchsegmentid='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and Equity_FOIdentifier='FUTSTK'", " Equity_ProductID");
                if (dtunderlying.Rows.Count > 0)
                {
                    UNDERLYING = null;
                    for (int i = 0; i < dtunderlying.Rows.Count; i++)
                    {
                        if (UNDERLYING == null)
                            UNDERLYING = dtunderlying.Rows[i][0].ToString();
                        else
                            UNDERLYING += "," + dtunderlying.Rows[i][0].ToString();
                    }
                }
            }
            else if (cmbinstrutype.SelectedItem.Value == "3")
            {
                DataTable dtunderlying = oDBEngine.GetDataTable("master_equity", "distinct Equity_ProductID", "equity_exchsegmentid='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and equity_FOIdentifier='OPTSTK'", " Equity_ProductID");
                if (dtunderlying.Rows.Count > 0)
                {
                    UNDERLYING = null;
                    for (int i = 0; i < dtunderlying.Rows.Count; i++)
                    {
                        if (UNDERLYING == null)
                            UNDERLYING = dtunderlying.Rows[i][0].ToString();
                        else
                            UNDERLYING += "," + dtunderlying.Rows[i][0].ToString();
                    }
                }
            }
            else if (cmbinstrutype.SelectedItem.Value == "4")
            {
                DataTable dtunderlying = oDBEngine.GetDataTable("master_equity", "distinct Equity_ProductID", "equity_exchsegmentid='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and (Equity_FOIdentifier='FUTIDX' or Equity_FOIdentifier='OPTIDX')", " Equity_ProductID");
                if (dtunderlying.Rows.Count > 0)
                {
                    UNDERLYING = null;
                    for (int i = 0; i < dtunderlying.Rows.Count; i++)
                    {
                        if (UNDERLYING == null)
                            UNDERLYING = dtunderlying.Rows[i][0].ToString();
                        else
                            UNDERLYING += "," + dtunderlying.Rows[i][0].ToString();
                    }
                }
            }
            else if (cmbinstrutype.SelectedItem.Value == "5")
            {
                DataTable dtunderlying = oDBEngine.GetDataTable("master_equity", "distinct Equity_ProductID", "equity_exchsegmentid='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and Equity_FOIdentifier='FUTIDX'", " Equity_ProductID");
                if (dtunderlying.Rows.Count > 0)
                {
                    UNDERLYING = null;
                    for (int i = 0; i < dtunderlying.Rows.Count; i++)
                    {
                        if (UNDERLYING == null)
                            UNDERLYING = dtunderlying.Rows[i][0].ToString();
                        else
                            UNDERLYING += "," + dtunderlying.Rows[i][0].ToString();
                    }
                }
            }
            else if (cmbinstrutype.SelectedItem.Value == "6")
            {

                DataTable dtunderlying = oDBEngine.GetDataTable("master_equity", "distinct Equity_ProductID", "equity_exchsegmentid='" + HttpContext.Current.Session["ExchangeSegmentID"] + "' and Equity_FOIdentifier='OPTIDX'", " Equity_ProductID");
                if (dtunderlying.Rows.Count > 0)
                {
                    UNDERLYING = null;
                    for (int i = 0; i < dtunderlying.Rows.Count; i++)
                    {
                        if (UNDERLYING == null)
                            UNDERLYING = dtunderlying.Rows[i][0].ToString();
                        else
                            UNDERLYING += "," + dtunderlying.Rows[i][0].ToString();
                    }
                }
            }

            if (UNDERLYING != null)
            {
                HiddenField_Product.Value = UNDERLYING;
            }


        }
        void fn_underlyingselected()
        {
            if (rdbunderlyingSelected.Checked)
            {
                string UNDERLYING = null;
                DataTable dtunderlying = oDBEngine.GetDataTable("master_products", "distinct PRODUCTS_ID", "PRODUCTS_DERIVEDFROMID in (" + HiddenField_Product.Value + ")", " PRODUCTS_ID");
                if (dtunderlying.Rows.Count > 0)
                {

                    for (int i = 0; i < dtunderlying.Rows.Count; i++)
                    {
                        if (UNDERLYING == null)
                            UNDERLYING = dtunderlying.Rows[i][0].ToString();
                        else
                            UNDERLYING += "," + dtunderlying.Rows[i][0].ToString();
                    }
                }
                if (UNDERLYING != null)
                {
                    HiddenField_Product.Value = UNDERLYING;
                }
            }
        }
        protected void btn_show_Click(object sender, EventArgs e)
        {
            fn_underlyingselected();
            procedure();
        }
        void procedure()
        {
            fn_underlyingselected();
            ViewState["BFVALUE_Sum"] = null;
            ViewState["BUYVALUE_Sum"] = null;
            ViewState["SELLVALUE_Sum"] = null;
            ViewState["CFVALUE_Sum"] = null;
            ViewState["MTM_Sum"] = null;
            ViewState["PRM_Sum"] = null;
            ViewState["FINSETT_Sum"] = null;
            ViewState["NETOBLIGATION_Sum"] = null;
            ViewState["AXNESN_Sum"] = null;
            ViewState["FINSETT_Sum"] = null;

            ds = dailyrep.Sp_ExchangeObligationFO(dtFor.Value.ToString(), Session["usersegid"].ToString(), HttpContext.Current.Session["ExchangeSegmentID"].ToString(), Session["LastCompany"].ToString(),
                HiddenField_Product.Value, cmbexpiry.SelectedItem.Text.ToString(), "All");
            ViewState["dataset"] = ds;
            tblEmail = ds;
            ViewState["tblEmail"] = tblEmail;
            int tabelrow = ds.Tables.Count;
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[tabelrow - 1].Rows[0]["status"].ToString() == "1")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "hi1", "NoRecord(2);", true);
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

                        if (ds.Tables[0].Rows[i]["MTM"] != DBNull.Value)
                            ds.Tables[0].Rows[i]["MTM"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["MTM"].ToString()));

                        if (ds.Tables[0].Rows[i]["FutFin"] != DBNull.Value)
                            ds.Tables[0].Rows[i]["FutFin"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["FutFin"].ToString()));

                        if (ds.Tables[0].Rows[i]["PRM"] != DBNull.Value)
                            ds.Tables[0].Rows[i]["PRM"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["PRM"].ToString()));

                        if (ds.Tables[0].Rows[i]["ExcAsn"] != DBNull.Value)
                            ds.Tables[0].Rows[i]["ExcAsn"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["ExcAsn"].ToString()));

                        if (ds.Tables[0].Rows[i]["NETOBLIGATION"] != DBNull.Value)
                            ds.Tables[0].Rows[i]["NETOBLIGATION"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["NETOBLIGATION"].ToString()));

                    }


                    ViewState["BFVALUE_Sum"] = Convert.ToDecimal(ds.Tables[1].Rows[0]["BFVALUE_Sum"]);
                    ViewState["BUYVALUE_Sum"] = Convert.ToDecimal(ds.Tables[1].Rows[0]["BUYVALUE_Sum"]); ;
                    ViewState["SELLVALUE_Sum"] = Convert.ToDecimal(ds.Tables[1].Rows[0]["SELLVALUE_Sum"]);
                    ViewState["CFVALUE_Sum"] = Convert.ToDecimal(ds.Tables[1].Rows[0]["CFVALUE_Sum"]);
                    ViewState["MTM_Sum"] = Convert.ToDecimal(ds.Tables[1].Rows[0]["MTM_Sum"]);
                    ViewState["PRM_Sum"] = Convert.ToDecimal(ds.Tables[1].Rows[0]["PRM_Sum"]);
                    ViewState["FINSETT_Sum"] = Convert.ToDecimal(ds.Tables[1].Rows[0]["FutFin_Sum"]);
                    ViewState["NETOBLIGATION_Sum"] = Convert.ToDecimal(ds.Tables[1].Rows[0]["NETOBLIGATION_Sum"]);
                    ViewState["AXNESN_Sum"] = Convert.ToDecimal(ds.Tables[1].Rows[0]["ExcAsn_Sum"]);

                    /////////////////////////////DISPLAY DATE/////////////////////////////////////
                    string SpanText = oconverter.ArrangeDate2(dtFor.Value.ToString());
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "displaydate('" + SpanText + "')", true);

                    ///////////////////////////////END//////////////////////////////////////////
                    bindgrid();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "hi", "NoRecord(1)", true);
            }

        }
        void bindgrid()
        {
            ds = (DataSet)ViewState["dataset"];
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



            grdExchange.DataSource = ds;
            grdExchange.DataBind();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "line", "line();", true);


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
                Label lblFutFin_Sum = (Label)e.Row.FindControl("lblFutFin_Sum");
                Label lblBFVALUE_Sum = (Label)e.Row.FindControl("lblBFVALUE_Sum");
                Label lblBUYVALUE_Sum = (Label)e.Row.FindControl("lblBUYVALUE_Sum");
                Label lblSELLVALUE_Sum = (Label)e.Row.FindControl("lblSELLVALUE_Sum");
                Label lblCFVALUE_Sum = (Label)e.Row.FindControl("lblCFVALUE_Sum");
                Label lblMTM_Sum = (Label)e.Row.FindControl("lblMTM_Sum");
                Label lblPRM_Sum = (Label)e.Row.FindControl("lblPRM_Sum");
                Label lblASNEXCSETT_Sum = (Label)e.Row.FindControl("lblASNEXCSETT_Sum");
                Label lblNETOBLIGATION_Sum = (Label)e.Row.FindControl("lblNETOBLIGATION_Sum");


                lblFutFin_Sum.Text = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ViewState["FINSETT_Sum"].ToString()));
                lblBFVALUE_Sum.Text = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["BFVALUE_Sum"].ToString()));
                lblBUYVALUE_Sum.Text = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["BUYVALUE_Sum"].ToString()));
                lblSELLVALUE_Sum.Text = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["SELLVALUE_Sum"].ToString()));
                lblCFVALUE_Sum.Text = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ViewState["CFVALUE_Sum"].ToString()));
                lblMTM_Sum.Text = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["MTM_Sum"].ToString()));
                lblPRM_Sum.Text = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["PRM_Sum"].ToString()));
                lblASNEXCSETT_Sum.Text = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["AXNESN_Sum"].ToString()));
                lblNETOBLIGATION_Sum.Text = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["NETOBLIGATION_Sum"].ToString()));

            }
        }
        protected void grdExchange_RowCreated(object sender, GridViewRowEventArgs e)
        {
            string rowID = String.Empty;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ds = (DataSet)ViewState["dataset"];
                rowID = "row" + e.Row.RowIndex;
                e.Row.Attributes.Add("id", "row" + e.Row.RowIndex);
                e.Row.Attributes.Add("onclick", "ChangeRowColorgrid(" + "'" + rowID + "','" + ds.Tables[0].Rows.Count + "'" + ")");
            }
        }

        void export()
        {
            ds = (DataSet)ViewState["dataset"];
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dt3 = new DataTable();
            dt.Rows.Clear();
            dt.Dispose();
            dt1.Rows.Clear();
            dt1.Dispose();
            dt3.Clear();
            dt3 = ds.Tables[2];

            dt = dt3.Copy();
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

            if (dt1.Rows[0]["MTM_Sum"] != DBNull.Value)
                row["Mtm"] = Convert.ToDecimal(dt1.Rows[0]["MTM_Sum"]);

            if (dt1.Rows[0]["PRM_Sum"] != DBNull.Value)
                row["Premium"] = Convert.ToDecimal(dt1.Rows[0]["PRM_Sum"]);

            if (dt1.Rows[0]["FutFin_Sum"] != DBNull.Value)
                row["FutFin"] = Convert.ToDecimal(dt1.Rows[0]["FutFin_Sum"]);

            if (dt1.Rows[0]["ASNEXCAmount_Sum"] != DBNull.Value)
                row["ASNEXCAmount"] = Convert.ToDecimal(dt1.Rows[0]["ASNEXCAmount_Sum"]);

            if (dt1.Rows[0]["NETOBLIGATION_Sum"] != DBNull.Value)
                row["NetObligation"] = Convert.ToDecimal(dt1.Rows[0]["NETOBLIGATION_Sum"]);

            dt.Rows.Add(row);

            //DataTable dtSegment = new DataTable();
            //dtSegment = ds.Tables[4].Copy();
            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            DataTable dtReportHeader = new DataTable();
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            DrRowR1[0] = "Exchange Obligation For [" + ds.Tables[4].Rows[0]["segmentname"].ToString() + "]" + ' ' + oconverter.ArrangeDate2(dtFor.Value.ToString());
            dtReportHeader.Rows.Add(DrRowR1);
            DataRow DrRowR2 = dtReportHeader.NewRow();
            //DrRowR2[0] = txtBankName.Text;
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

            if (ddlExport.SelectedItem.Value == "E")
            {
                objExcel.ExportToExcelforExcel(dt, "Exchange Obligation", "Total :", dtReportHeader, dtReportFooter);
            }
            else if (ddlExport.SelectedItem.Value == "P")
            {
                objExcel.ExportToPDF(dt, "Exchange Obligation", "Total :", dtReportHeader, dtReportFooter);
            }

            //oconverter.ExportWithDatatable(dt, dtSegment, "Net Position");
        }


        protected void cmbinstrutype_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "check", "rdbcheckchange();", true);
            fn_cmbunderlying();
            fn_cmbexpiry();

        }

        protected void btnhide_Click(object sender, EventArgs e)
        {
            fn_cmbexpiry();
        }

        protected void ddlExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            export();
        }


        //--------------------Region for sending email---------------

        private void FillCombo()
        {
            if (HttpContext.Current.Session["userlastsegment"].ToString() == "4")
            {
                string[,] r = new string[1, 2];
                r[0, 0] = "EM";
                r[0, 1] = "Employees";
                clsdrp.AddDataToDropDownList(r, cmbsearch);
            }
            else
            {
                string[,] r = new string[1, 2];
                r[0, 0] = "EM";
                r[0, 1] = "Employees";
                clsdrp.AddDataToDropDownList(r, cmbsearch);

            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {


            tblEmail = (DataSet)ViewState["tblEmail"];
            DataSet ds = tblEmail;
            string disptbl = "";
            string SpanText = oconverter.ArrangeDate2(dtFor.Value.ToString());
            if (SubClients != "")
            {
                string[] clnt = SubClients.ToString().Split(',');
                int k = clnt.Length;
                for (int j = 0; j < clnt.Length; j++)
                {
                    disptbl = "<table width=\"1050px\" style=\"font-size:8pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\"><tr style=\"background-color:#BB694D;color:White;\"><td  align=\"center\">Instrument</td><td align=\"center\">Expiry Date</td><td align=\"center\">B/F Qty</td><td align=\"center\">Open Price</td><td align=\"center\">B/F Value</td><td align=\"center\">Day Buy</td><td align=\"center\">Buy Value</td><td align=\"center\">Day Sell</td><td align=\"center\">Sell Value</td><td align=\"center\">C/F Qty</td><td align=\"center\">Sett Price</td><td align=\"center\">C/F Value</td><td align=\"center\">Premium</td><td align=\"center\">MTM</td><td align=\"center\">Future FinSett</td><td align=\"center\">ASN/EXC <br />Amount</td><td align=\"center\">Net Obligation</td></tr>";
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

                        if (ds.Tables[0].Rows[i]["MTM"] != DBNull.Value)
                            ds.Tables[0].Rows[i]["MTM"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["MTM"].ToString()));

                        if (ds.Tables[0].Rows[i]["FutFin"] != DBNull.Value)
                            ds.Tables[0].Rows[i]["FutFin"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["FutFin"].ToString()));

                        if (ds.Tables[0].Rows[i]["PRM"] != DBNull.Value)
                            ds.Tables[0].Rows[i]["PRM"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["PRM"].ToString()));

                        if (ds.Tables[0].Rows[i]["ExcAsn"] != DBNull.Value)
                            ds.Tables[0].Rows[i]["ExcAsn"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["ExcAsn"].ToString()));

                        if (ds.Tables[0].Rows[i]["NETOBLIGATION"] != DBNull.Value)
                            ds.Tables[0].Rows[i]["NETOBLIGATION"] = oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ds.Tables[0].Rows[i]["NETOBLIGATION"].ToString()));
                        disptbl += "<tr><td>&nbsp;" + ds.Tables[0].Rows[i]["tabSymbol"] + "</td><td>&nbsp;" + ds.Tables[0].Rows[i]["Equity_EffectUntil"] + "</td><td align=\"right\">&nbsp;" + ds.Tables[0].Rows[i]["LOTSRESULT_B"] + "</td><td align=\"right\">&nbsp;" + ds.Tables[0].Rows[i]["SettlementPrice"] + "</td><td align=\"right\">&nbsp;" + ds.Tables[0].Rows[i]["BFVALUE"] + "</td><td align=\"right\">&nbsp;" + ds.Tables[0].Rows[i]["DAYBUY"] + "</td><td align=\"right\">&nbsp;" + ds.Tables[0].Rows[i]["BUYVALUE"] + "</td><td align=\"right\">&nbsp;" + ds.Tables[0].Rows[i]["DAYSELL"] + "</td><td align=\"right\">&nbsp;" + ds.Tables[0].Rows[i]["SELLVALUE"] + "</td><td align=\"right\">&nbsp;" + ds.Tables[0].Rows[i]["CFQTY_I1"] + "</td><td align=\"right\">&nbsp;" + ds.Tables[0].Rows[i]["SETTPRICE"] + "</td><td align=\"right\">&nbsp;" + ds.Tables[0].Rows[i]["CFVALUE"] + "</td><td align=\"right\">&nbsp;" + ds.Tables[0].Rows[i]["PRM"] + "</td><td align=\"right\">&nbsp;" + ds.Tables[0].Rows[i]["MTM"] + "</td><td align=\"right\">&nbsp;" + ds.Tables[0].Rows[i]["FutFin"] + "</td><td align=\"right\">&nbsp;" + ds.Tables[0].Rows[i]["ExcAsn"] + "</td><td align=\"right\">&nbsp;" + ds.Tables[0].Rows[i]["NETOBLIGATION"] + "</td></tr>";
                    }
                    disptbl += "<tr style=\"background-color: #FFD4AA; color: Black;\"><td>Total:</td><td>&nbsp;</td><td align=\"right\">&nbsp;</td><td align=\"right\">&nbsp;</td><td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["BFVALUE_Sum"].ToString())) + "</td><td align=\"right\">&nbsp;</td><td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["BUYVALUE_Sum"].ToString())) + "</td><td align=\"right\">&nbsp;</td><td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["SELLVALUE_Sum"].ToString())) + "</td><td align=\"right\">&nbsp;</td><td align=\"right\">&nbsp;</td><td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ViewState["CFVALUE_Sum"].ToString())) + "</td><td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["PRM_Sum"].ToString())) + "</td><td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["MTM_Sum"].ToString())) + "</td><td align=\"right\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(ViewState["FINSETT_Sum"].ToString())) + "</td><td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["AXNESN_Sum"].ToString())) + "</td><td align=\"right\">" + oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(ViewState["NETOBLIGATION_Sum"].ToString())) + "</td></tr>";
                    disptbl += "</table>";

                    string emailbdy = disptbl;
                    string contactid = clnt[j].ToString();
                    string billdate = oconverter.ArrangeDate2(dtFor.Value.ToString());
                    //  string billdate = oconverter.ArrangeDate2(DateTime.Today.ToString());
                    string Subject = "Exchange Obligation Report For  " + billdate;
                    if (oDBEngine.SendReportSt(emailbdy, contactid, billdate, Subject) == true)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "jscript6", "<script>ForFilterOff();</script>");
                        Page.ClientScript.RegisterStartupScript(GetType(), "jscript12", "<script>displaydate('" + SpanText + "');</script>");
                        Page.ClientScript.RegisterStartupScript(GetType(), "jscript7", "<script>MailsendT();</script>");
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "jscript8", "<script>ForFilterOff();</script>");
                        Page.ClientScript.RegisterStartupScript(GetType(), "jscript12", "<script>displaydate('" + SpanText + "');</script>");
                        Page.ClientScript.RegisterStartupScript(GetType(), "jscript9", "<script>MailsendF();</script>");
                    }
                }

            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "jscript10", "<script>ForFilterOff();</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "jscript12", "<script>displaydate('" + SpanText + "');</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "jscript11", "<script>MailsendFT();</script>");
            }
            Page.ClientScript.RegisterStartupScript(GetType(), "jscript12", "<script>ForFilterOff();</script>");

        }


    }
}