using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using DevExpress.Web;
using EntityLayer.CommonELS;
namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_IFrameCashBankBook : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        int pageindex = 0;
        int pagecount = 0;
        int pageSize;
        int rowcount = 0;
        static string Branch;
        string BtnclickShow = "";
        string Segment;
        string data;
        decimal openingBal;
        decimal debitTotal = 0;
        decimal creditTotal = 0;
        string SegmentID = null;

        DataTable dtCashBankBook = new DataTable();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        ExcelFile objExcel = new ExcelFile();

        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        frmReport_IFrameCashBankBookBL OfrmReport_IFrameCashBankBookBL = new frmReport_IFrameCashBankBookBL();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
            //    string sPath = HttpContext.Current.Request.Url.ToString();
            //    oDBEngine.Call_CheckPageaccessebility(sPath);
            //}
        }
        protected void Page_Load(object sender, EventArgs e)
        {


            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Reports/frmReport_IFrameCashBankBook.aspx");
            if (!IsPostBack)
            {

                if (Request.QueryString["mainacc"] != null)
                {
                    dtFromG.EditFormatString = oconverter.GetDateFormat("Date");
                    dtToG.EditFormatString = oconverter.GetDateFormat("Date");
                    HDNMAIN.Value = Request.QueryString["mainacc"].ToString();
                    FillGrid();
                }


                Branch = null;
                Segment = null;
                dtDate.EditFormatString = oconverter.GetDateFormat("Date");
                dtToDate.EditFormatString = oconverter.GetDateFormat("Date");
                dtSearchDate.EditFormatString = oconverter.GetDateFormat("Date");
                string fDate = null;
                string tDate = null;
                fDate = Session["FinYearStart"].ToString();
                tDate = Session["FinYearEnd"].ToString();
                dtDate.Value = Convert.ToDateTime(fDate);
                dtToDate.Value = Convert.ToDateTime(tDate);

                Session["SegmentID"] = "0";

                rdbSegAll.Attributes.Add("OnClick", "SegAll('seg')");
                rdbSegSelected.Attributes.Add("OnClick", "SegSelected('seg')");
                rdbbAll.Attributes.Add("OnClick", "SegAll('branch')");
                rdbbSelected.Attributes.Add("OnClick", "SegSelected('branch')");
                txtsubscriptionID.Attributes.Add("onkeyup", "showOptions(this,'SearchBankSegmentBranch',event)");

                //In Case Of NSDL And CDSL ExchangeSegmentID For Bank Fetch
                if (Session["UserSegID"].ToString().Length == 8)
                    hdn_NsdlCdslExchangeSegment.Value = oDBEngine.GetFieldValue("tbl_master_companyExchange", "exch_internalId", "exch_TMCode='" + Session["UserSegID"].ToString() + "'", 1)[0, 0];


            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//
            Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load()</script>");
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
            if (idlist[0] == "Branch")
            {
                Branch = str;
                data = "Branch~" + str1;
            }
            else if (idlist[0] == "Segment")
            {
                Segment = str;
                data = "Segment~" + str1;
            }

        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        protected void grdCashBankSummary_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string lcVar1 = ((DataRowView)e.Row.DataItem)["Accountsledger_AmountDr"].ToString();
                string lcVar2 = ((DataRowView)e.Row.DataItem)["Accountsledger_AmountCr"].ToString();
                if (lcVar1 == "")
                {
                    lcVar1 = "0";
                    e.Row.Cells[7].Text = "";
                }
                else
                    e.Row.Cells[7].Text = oconverter.getFormattedvalue(decimal.Parse(lcVar1));

                if (lcVar2 == "")
                {
                    lcVar2 = "0";
                    e.Row.Cells[8].Text = "";
                }
                else
                    e.Row.Cells[8].Text = oconverter.getFormattedvalue(decimal.Parse(lcVar2));
                debitTotal += decimal.Parse(lcVar1);
                creditTotal += decimal.Parse(lcVar2);
                System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo("hi-IN").NumberFormat;
                currencyFormat.CurrencySymbol = "";
                currencyFormat.CurrencyNegativePattern = 2;
                if (((DataRowView)e.Row.DataItem)["AccountName"].ToString().Trim() == "Opening Balance")
                {
                    openingBal = decimal.Parse(lcVar1) - decimal.Parse(lcVar2) + openingBal;
                    if (openingBal < 0)
                    {
                        e.Row.Cells[9].Text = openingBal.ToString("c", currencyFormat);//.Substring(1, openingBal.ToString("c", currencyFormat).Length - 1);
                        e.Row.Cells[9].ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        e.Row.Cells[9].Text = openingBal.ToString("c", currencyFormat);
                        //e.Row.Cells[7].Text = "Dr";
                    }
                }
                else
                {
                    openingBal = decimal.Parse(lcVar1) - decimal.Parse(lcVar2) + openingBal;
                    if (openingBal < 0)
                    {
                        e.Row.Cells[9].Text = openingBal.ToString("c", currencyFormat);//.Substring(1, openingBal.ToString("c", currencyFormat).Length - 1);
                        e.Row.Cells[9].ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        e.Row.Cells[9].Text = openingBal.ToString("c", currencyFormat);
                        //e.Row.Cells[7].Text = "Dr";
                    }
                }
                Label TradeDate = (Label)e.Row.FindControl("lblTradeDate");
                Label ReferenceID = (Label)e.Row.FindControl("lblVoucherNo");
                Label MainID = (Label)e.Row.FindControl("lblMainID");
                Label SubID = (Label)e.Row.FindControl("lblSubID");
                Label CompID = (Label)e.Row.FindControl("lblCompID");
                Label SegID = (Label)e.Row.FindControl("lblSegID");
                if (Session["EntryProfileType"].ToString() == "F")
                {
                    if (Request.QueryString["mainacc"] != null)
                    {
                        if (TradeDate.Text != String.Empty)
                        {

                            if (Session["LCKBNK"] != null)
                            {
                                if (Convert.ToDateTime(TradeDate.Text) >= Convert.ToDateTime(Session["LCKBNK"].ToString()))
                                {
                                    ((Label)e.Row.FindControl("lblVoucherNo")).Attributes.Add("onclick", "javascript:updateCashBankDetail('" + TradeDate.Text + "','" + ReferenceID.Text + "','" + MainID.Text + "','" + SubID.Text + "','" + CompID.Text + "','" + SegID.Text + "');");

                                    e.Row.Cells[2].ToolTip = "Click to View Detail!";
                                    e.Row.Cells[2].Style.Add("cursor", "hand");
                                }
                                else
                                {
                                    e.Row.Cells[2].ToolTip = "Voucher Locked !";
                                }
                            }
                            else
                            {
                                ((Label)e.Row.FindControl("lblVoucherNo")).Attributes.Add("onclick", "javascript:updateCashBankDetail('" + TradeDate.Text + "','" + ReferenceID.Text + "','" + MainID.Text + "','" + SubID.Text + "','" + CompID.Text + "','" + SegID.Text + "');");
                                e.Row.Cells[2].ToolTip = "Click to View Detail!";
                                e.Row.Cells[2].Style.Add("cursor", "hand");
                            }
                        }
                    }
                    else
                    {
                        if (TradeDate.Text != String.Empty)
                        {
                            if (Session["LCKBNK"] != null)
                            {
                                if (Convert.ToDateTime(TradeDate.Text) >= Convert.ToDateTime(Session["LCKBNK"].ToString()))
                                {
                                    ((Label)e.Row.FindControl("lblVoucherNo")).Attributes.Add("onclick", "javascript:updateCashBankDetail('" + TradeDate.Text + "','" + ReferenceID.Text + "','" + MainID.Text + "','" + SubID.Text + "','" + CompID.Text + "','" + SegID.Text + "');");

                                    e.Row.Cells[2].ToolTip = "Click to View Detail!";
                                    e.Row.Cells[2].Style.Add("cursor", "hand");
                                }
                                else
                                {
                                    e.Row.Cells[2].ToolTip = "Voucher Locked !";
                                }
                            }
                            else
                            {
                                ((Label)e.Row.FindControl("lblVoucherNo")).Attributes.Add("onclick", "javascript:updateCashBankDetail('" + TradeDate.Text + "','" + ReferenceID.Text + "','" + MainID.Text + "','" + SubID.Text + "','" + CompID.Text + "','" + SegID.Text + "');");
                                e.Row.Cells[2].ToolTip = "Click to View Detail!";
                                e.Row.Cells[2].Style.Add("cursor", "hand");
                            }
                        }
                    }
                }
            }
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                GridViewRow row = e.Row;
                LinkButton fpage = (LinkButton)row.FindControl("FirstPage");
            }


        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            FillGrid();

        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            BtnclickShow = "Click";
            HDNMAIN.Value = Request.QueryString["mainacc"].ToString();
            FillGrid();

        }
        public void FillGrid()
        {
            decimal receipt = 0;
            decimal Payment = 0;
            pageSize = 2500000;
            grdCashBankBook.PageSize = pageSize;
            System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo("hi-IN").NumberFormat;
            currencyFormat.CurrencySymbol = "";
            currencyFormat.CurrencyNegativePattern = 2;
            String RedirectTo = "";

            //bellow two line added by debjyoti
            dtDate.Value = dtFromG.Value;
            dtToDate.Value = dtToG.Value;

            if (Request.QueryString["mainacc"] != null)
            {
                if (BtnclickShow == "Click")
                {

                    dtDate.Value = dtFromG.Value;
                    dtToDate.Value = dtToG.Value;
                    SpanShowHeader.InnerText = "   Period  From    " + oconverter.ArrangeDate2(dtFromG.Value.ToString()) + "      To    " + oconverter.ArrangeDate2(dtToG.Value.ToString());


                    string Spantext = Request.QueryString["AccName"].ToString();// +"  ( From ) : " + oconverter.ArrangeDate2(dtDate.Value.ToString()) + "  ( To ) : " + oconverter.ArrangeDate2(dtToDate.Value.ToString()) + "";
                    string SpanText1 = oconverter.ArrangeDate2(dtDate.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtToDate.Value.ToString());
                    // ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "ShowHide('" + Spantext + "','" + SpanText1 + "')", true);
                    //Page.ClientScript.RegisterStartupScript(GetType(), "Filter", "<script>ShowHide('" + Spantext + "','" + SpanText1 + "');</script>");

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct14", "ShowDateRange()", true);

                    RedirectTo = "";
                }
                else
                {
                    DataTable dtFinYear = oDBEngine.GetDataTable("MASTER_FINYEAR ", " FINYEAR_STARTDATE ", " FINYEAR_CODE ='" + Session["LastFinYear"].ToString() + "'");
                    DateTime StartDate = Convert.ToDateTime(dtFinYear.Rows[0][0].ToString());
                    DateTime dtTranDate = new DateTime();
                    DateTime date = Convert.ToDateTime(Request.QueryString["TDate"].ToString());
                    DateTime ToDay = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
                    if (date < ToDay)
                        dtTranDate = Convert.ToDateTime(date.AddDays(-45).ToShortDateString());
                    else
                        dtTranDate = Convert.ToDateTime(oDBEngine.GetDate().AddDays(-45).ToShortDateString());


                    if (dtTranDate < StartDate)
                    {
                        dtTranDate = StartDate;
                        dtDate.Value = dtTranDate;
                        dtToDate.Value = date;
                        dtFromG.Value = dtTranDate;
                        dtToG.Value = date;
                    }
                    else
                    {
                        dtDate.Value = dtTranDate;
                        dtToDate.Value = date;
                        dtFromG.Value = dtTranDate;
                        dtToG.Value = date;
                    }

                    SpanShowHeader.InnerText = "   Period  From    " + oconverter.ArrangeDate2(dtTranDate.ToString()) + "      To    " + oconverter.ArrangeDate2(date.ToString());

                    string Spantext = Request.QueryString["AccName"].ToString();// +"  ( From ) : " + oconverter.ArrangeDate2(dtDate.Value.ToString()) + "  ( To ) : " + oconverter.ArrangeDate2(dtToDate.Value.ToString()) + "";
                    string SpanText1 = oconverter.ArrangeDate2(dtDate.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtToDate.Value.ToString());
                    // ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "ShowHide('" + Spantext + "','" + SpanText1 + "')", true);
                    Page.ClientScript.RegisterStartupScript(GetType(), "Filter", "<script>ShowHide('" + Spantext + "','" + SpanText1 + "');</script>");
                    RedirectTo = "GeneralTrial";
                }
                HDNMAIN.Value = Request.QueryString["mainacc"].ToString();
                txtBankName_hidden.Value = Request.QueryString["mainacc"].ToString();
                //dtDate.Value=Request.QueryString["date"].ToString();
                //dtToDate.Value= Request.QueryString["TDate"].ToString();
                Segment = Request.QueryString["Segment"].ToString();
                Branch = Session["userbranchHierarchy"].ToString();



            }
            else
            {

                string Spantext = txtBankName.Text;// +"  ( From ) : " + oconverter.ArrangeDate2(dtDate.Value.ToString()) + "  ( To ) : " + oconverter.ArrangeDate2(dtToDate.Value.ToString()) + "";
                string SpanText1 = oconverter.ArrangeDate2(dtDate.Value.ToString()) + "  - " + oconverter.ArrangeDate2(dtToDate.Value.ToString());
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "ShowHide('" + Spantext + "','" + SpanText1 + "')", true);



                if (rdbSegAll.Checked == true)
                {
                    DataTable dtSegment = oDBEngine.GetDataTable("tbl_master_companyExchange", "exch_internalId", "exch_compId='" + Session["LastCompany"].ToString() + "'");
                    if (dtSegment.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtSegment.Rows.Count; i++)
                        {
                            if (Segment == null)
                                Segment = dtSegment.Rows[i][0].ToString();
                            else
                                Segment += "," + dtSegment.Rows[i][0].ToString();
                        }
                    }
                    //if (Session["userlastsegment"].ToString() == "5")
                    //{
                    //    Segment += "0" + Segment;
                    //}
                }
                else
                {
                    Segment = HDNSeg.Value;
                }
                if (rdbbAll.Checked == true)
                {
                    Branch = Session["userbranchHierarchy"].ToString();
                }
                if (Segment == null || Segment == "")
                {
                    Segment = Session["SegmentID"].ToString();
                }
            }


            //dtCashBankBook = oDBEngine.GetDataTable("(select AccountsLedger_BranchID,accountsledger_MainAccountID,accountsledger_transactiondate,accountsledger_TransactionReferenceID,cast(Accountsledger_AmountDr as varchar(max)) as Accountsledger_AmountDr,cast(Accountsledger_AmountCr as varchar(max)) as Accountsledger_AmountCr,accountsledger_companyID,accountsledger_ExchangeSegmentID from Trans_accountsledger where accountsledger_companyID='" + Session["LastCompany"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and accountsledger_TransactionType='Cash_Bank' and accountsledger_transactiondate between ('" + dtDate.Value + "') and ('" + dtToDate.Value + "') and accountsledger_MainAccountID ='" + txtBankName_hidden.Value + "'and AccountsLedger_BranchID in(" + Branch + ") and AccountsLedger_FinYear='" + Session["LastFinYear"].ToString() + "') as D,Trans_accountsledger a", " convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,convert(varchar(11),a.accountsledger_valuedate,113) as ValueDate,a.accountsledger_TransactionReferenceID,a.accountsledger_Narration,isnull((select isnull(mainaccount_name,'') from master_mainaccount where mainaccount_accountcode=a.accountsledger_MainAccountID),'')+'-'+	isnull((case when isnumeric(a.accountsledger_SubAccountID)=0 	then (select top 1 isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'')+' ['+isnull(ltrim(rtrim(cnt_UCC)),'')+']' from tbl_master_contact where ltrim(rtrim(cnt_internalid))=ltrim(rtrim(a.accountsledger_SubAccountID))) else (select top 1 rtrim(SubAccount_Name)+' ['+isnull(ltrim(rtrim(SubAccount_Code)),'')+']' from master_subAccount where SubAccount_Code = ltrim(rtrim(a.accountsledger_SubAccountID)))end),'') as AccountName,a.accountsledger_InstrumentNumber,convert(varchar(11),a.accountsledger_InstrumentDate,113) as InstDate,case when a.Accountsledger_AmountCr='0.00000000' then null else a.Accountsledger_AmountCr end as Accountsledger_AmountDr,case when a.Accountsledger_AmountDr='0.00000000' then null else a.Accountsledger_AmountDr end as Accountsledger_AmountCr,'0.0' as Closing,a.accountsledger_transactiondate as TrDate1,a.accountsledger_MainAccountID as MainID,a.accountsledger_SubAccountID as SubID,a.accountsledger_companyID as CompanyID,a.accountsledger_exchangeSegmentID as SegID ", " a.accountsledger_transactiondate=D.accountsledger_transactiondate and a.accountsledger_companyID=D.accountsledger_companyID and a.accountsledger_ExchangeSegmentID=D.accountsledger_ExchangeSegmentID and a.accountsledger_TransactionReferenceID=D.accountsledger_TransactionReferenceID and a.accountsledger_MainAccountID<>D.accountsledger_MainAccountID and a.accountsledger_BranchID=D.accountsledger_branchid order by a.accountsledger_transactiondate,a.accountsledger_TransactionReferenceID desc ");
            DataSet ds = new DataSet();

            ds = OfrmReport_IFrameCashBankBookBL.Fetch_CashBankBook(txtBankName_hidden.Value, dtDate.Value.ToString(), dtToDate.Value.ToString(),
                                         Segment, Session["LastCompany"].ToString(), Session["LastFinYear"].ToString(), RedirectTo, Branch,
                                         Session["ActiveCurrency"].ToString().Split('~')[0], Session["TradeCurrency"].ToString().Split('~')[0]);

            ViewState["dataset"] = ds;

            dtCashBankBook = ds.Tables[0];
            DataTable OpenBalance = new DataTable();
            if (Request.QueryString["mainacc"] != null)
            {

                OpenBalance = oDBEngine.OpeningBalance(txtBankName_hidden.Value, null, Convert.ToDateTime(dtDate.Value), Segment, Session["LastCompany"].ToString(), Convert.ToDateTime(dtToDate.Value), Convert.ToInt32(Session["ActiveCurrency"].ToString().Split('~')[0]), Convert.ToInt32(Session["TradeCurrency"].ToString().Split('~')[0]));

            }
            else
            {
                OpenBalance = oDBEngine.OpeningBalance(txtBankName_hidden.Value, null, Convert.ToDateTime(dtDate.Value), Segment, Session["LastCompany"].ToString(), Convert.ToDateTime(dtToDate.Value), Convert.ToInt32(Session["ActiveCurrency"].ToString().Split('~')[0]), Convert.ToInt32(Session["TradeCurrency"].ToString().Split('~')[0]));

            }
            DataTable dtCashBankBook_New = dtCashBankBook.Copy();
            dtCashBankBook_New.Rows.Clear();
            DataRow newRow = dtCashBankBook_New.NewRow();
            newRow[1] = oconverter.ArrangeDate2(Convert.ToDateTime(dtDate.Value).ToShortDateString());
            newRow[4] = "Opening Balance";
            newRow[9] = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()) * (-1);
            dtCashBankBook_New.Rows.Add(newRow);
            for (int i = 0; i < dtCashBankBook.Rows.Count; i++)
            {
                newRow = dtCashBankBook_New.NewRow();
                newRow[0] = dtCashBankBook.Rows[i]["TrDate"];
                newRow[1] = dtCashBankBook.Rows[i]["ValueDate"];
                newRow[2] = dtCashBankBook.Rows[i]["accountsledger_TransactionReferenceID"];
                newRow[3] = dtCashBankBook.Rows[i]["accountsledger_Narration"];
                newRow[4] = dtCashBankBook.Rows[i]["AccountName"];
                newRow[5] = dtCashBankBook.Rows[i]["accountsledger_InstrumentNumber"];
                newRow[6] = dtCashBankBook.Rows[i]["InstDate"];
                newRow[7] = dtCashBankBook.Rows[i]["Accountsledger_AmountDr"];
                newRow[8] = dtCashBankBook.Rows[i]["Accountsledger_AmountCr"];
                newRow[9] = dtCashBankBook.Rows[i]["Closing"];
                newRow[10] = dtCashBankBook.Rows[i]["TrDate1"];
                newRow[11] = dtCashBankBook.Rows[i]["MainID"];
                newRow[12] = dtCashBankBook.Rows[i]["SubID"];
                newRow[13] = dtCashBankBook.Rows[i]["CompanyID"];
                newRow[14] = dtCashBankBook.Rows[i]["SegID"];
                dtCashBankBook_New.Rows.Add(newRow);
                if (dtCashBankBook.Rows[i]["Accountsledger_AmountDr"].ToString() != "")
                    receipt += Convert.ToDecimal(dtCashBankBook.Rows[i]["Accountsledger_AmountDr"].ToString());
                if (dtCashBankBook.Rows[i]["Accountsledger_AmountCr"].ToString() != "")
                    Payment += Convert.ToDecimal(dtCashBankBook.Rows[i]["Accountsledger_AmountCr"].ToString());
            }
            dtCashBankBook.Rows.Clear();
            dtCashBankBook = dtCashBankBook_New.Copy();
            pagecount = dtCashBankBook.Rows.Count / pageSize + 1;
            TotalPages.Value = pagecount.ToString();
            if (pageindex <= 0)
            {
                pageindex = 0;
                openingBal = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()) * (-1);
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "hide", "Disable('P');", true);
                // Page.ClientScript.RegisterStartupScript(GetType(), "hide", "<script language='javascript'>Disable('P');</script>");
            }
            if (pageindex >= int.Parse(TotalPages.Value.ToString()))
            {
                pageindex = int.Parse(TotalPages.Value.ToString()) - 1;
             //   ScriptManager.RegisterStartupScript(this, this.GetType(), "hide", "Disable('N');", true);
                // Page.ClientScript.RegisterStartupScript(GetType(), "hide", "<script language='javascript'>Disable('N');</script>");
            }
            if (pageindex >= (int.Parse(TotalPages.Value.ToString()) - 1))
            {
              //  ScriptManager.RegisterStartupScript(this, this.GetType(), "hide", "Disable('N');", true);
                // Page.ClientScript.RegisterStartupScript(GetType(), "hide", "<script language='javascript'>Disable('N');</script>");
            }
            if (pageindex > 0)
            {
                int totalRecord = (pageindex) * pageSize;
                openingBal = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()) * (-1);
                decimal DR = 0;
                decimal CR = 0;
                for (int i = 0; i < totalRecord; i++)
                {
                    if (dtCashBankBook.Rows[i]["Accountsledger_AmountDr"].ToString() != "")
                        DR = decimal.Parse(dtCashBankBook.Rows[i]["Accountsledger_AmountDr"].ToString());
                    else
                        DR = 0;
                    if (dtCashBankBook.Rows[i]["Accountsledger_AmountCr"].ToString() != "")
                        CR = decimal.Parse(dtCashBankBook.Rows[i]["Accountsledger_AmountCr"].ToString());
                    else
                        CR = 0;
                    openingBal = DR - CR + openingBal;
                }
            }
            grdCashBankBook.PageIndex = pageindex;
            CurrentPage.Value = pageindex.ToString();
            rowcount = 0;
            ViewState["dtCashBankBook"] = dtCashBankBook;
            grdCashBankBook.DataSource = dtCashBankBook;
            grdCashBankBook.DataBind();
           

            if (dtCashBankBook.Rows.Count > 0)
            {
                for (int k = 1; k < dtCashBankBook.Rows.Count; k++)
                {
                    if (dtCashBankBook.Rows[k]["Accountsledger_AmountDr"].ToString() != "")
                        dtCashBankBook.Rows[k]["Accountsledger_AmountDr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtCashBankBook.Rows[k]["Accountsledger_AmountDr"].ToString()));

                    if (dtCashBankBook.Rows[k]["Accountsledger_AmountCr"].ToString() != "")
                        dtCashBankBook.Rows[k]["Accountsledger_AmountCr"] = oconverter.getFormattedvaluewithoriginalsign(Convert.ToDecimal(dtCashBankBook.Rows[k]["Accountsledger_AmountCr"].ToString()));

                }
            }
            if (pageindex == int.Parse(TotalPages.Value.ToString()) - 1)
            {
                DataRow closingDr = dtCashBankBook.NewRow();
                grdCashBankBook.FooterRow.Cells[4].Text = "Closing Balance";
                closingDr["AccountName"] = "Closing Balance";


                grdCashBankBook.FooterRow.Cells[9].Text = openingBal.ToString("c", currencyFormat);
                closingDr["Closing"] = openingBal.ToString("c", currencyFormat);

                if (receipt != 0)
                {
                    grdCashBankBook.FooterRow.Cells[7].Text = receipt.ToString("c", currencyFormat);
                    closingDr["Accountsledger_AmountDr"] = receipt.ToString("c", currencyFormat);
                }
                else
                {
                    grdCashBankBook.FooterRow.Cells[7].Text = "";
                    //closingDr["Accountsledger_AmountDr"] = "";
                }
                if (Payment != 0)
                {
                    grdCashBankBook.FooterRow.Cells[8].Text = Payment.ToString("c", currencyFormat);
                    closingDr["Accountsledger_AmountCr"] = Payment.ToString("c", currencyFormat);
                }
                else
                {
                    grdCashBankBook.FooterRow.Cells[8].Text = "";
                //    closingDr["Accountsledger_AmountCr"] = "";
                }
                grdCashBankBook.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Left;
                grdCashBankBook.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Right;
                grdCashBankBook.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                grdCashBankBook.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                grdCashBankBook.FooterRow.Cells[4].ForeColor = System.Drawing.Color.White;
                grdCashBankBook.FooterRow.Cells[9].ForeColor = System.Drawing.Color.White;
                grdCashBankBook.FooterRow.Cells[7].ForeColor = System.Drawing.Color.White;
                grdCashBankBook.FooterRow.Cells[8].ForeColor = System.Drawing.Color.White;
                grdCashBankBook.FooterRow.Cells[4].Font.Bold = true;
                grdCashBankBook.FooterRow.Cells[9].Font.Bold = true;
                grdCashBankBook.FooterRow.Cells[7].Font.Bold = true;
                grdCashBankBook.FooterRow.Cells[8].Font.Bold = true;
                grdCashBankBook.FooterRow.Cells[9].Wrap = false;
                grdCashBankBook.FooterRow.Cells[7].Wrap = false;
                grdCashBankBook.FooterRow.Cells[8].Wrap = false;

                
                dtCashBankBook.Rows.Add(closingDr);
             
               
            }
            //Debjyoti
            aspxGrdCashBankBook.DataSource = dtCashBankBook;
            aspxGrdCashBankBook.DataBind();

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
                    pageindex = int.Parse(CurrentPage.Value) - 1;
                    break;
                case "Last":
                    pageindex = int.Parse(TotalPages.Value);
                    break;
                default:
                    pageindex = int.Parse(e.CommandName.ToString());
                    break;
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "JSHeight", "CallHeight();", true);
            FillGrid();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            decimal receipt = 0;
            decimal Payment = 0;
            string whereclause = null;
            string whereclause1 = null;
            string trDate = null;
            if (dtSearchDate.Value == null)
            {
                trDate = "1/1/0001 12:00:00 AM";
            }
            if (txtVouno.Text != "Voucher Number" && trDate != "1/1/0001 12:00:00 AM")
            {
                whereclause = " accountsledger_TransactionReferenceID like '" + txtVouno.Text + "%' and accountsledger_transactiondate='" + dtSearchDate.Value + "' ";
            }
            else if (trDate != "1/1/0001 12:00:00 AM" && txtVouno.Text == "Voucher Number")
            {
                whereclause = " accountsledger_transactiondate='" + dtSearchDate.Value + "' ";
            }
            else if (trDate == "1/1/0001 12:00:00 AM" && txtVouno.Text != "Voucher Number")
            {
                whereclause = " accountsledger_TransactionReferenceID like '" + txtVouno.Text + "%' ";
            }
            if (txtInstNo.Text != "Instrument Number")
            {
                whereclause = " accountsledger_InstrumentNumber like '" + txtInstNo.Text + "%'";
            }
            pageSize = 2500000;
            grdCashBankBook.PageSize = pageSize;
            System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo("hi-IN").NumberFormat;
            currencyFormat.CurrencySymbol = "";
            currencyFormat.CurrencyNegativePattern = 2;
            dtCashBankBook = (DataTable)ViewState["dtCashBankBook"];

            DataView foundRow = new DataView(dtCashBankBook);
            foundRow.RowFilter = whereclause;
            dtCashBankBook = foundRow.Table.Clone();
            foreach (DataRowView dvr in foundRow)
            {
                dtCashBankBook.ImportRow(dvr.Row);
            }
            dtCashBankBook.AcceptChanges();
            DataTable OpenBalance = oDBEngine.OpeningBalance(txtBankName_hidden.Value, null, Convert.ToDateTime(dtDate.Value), Segment, Session["LastCompany"].ToString(), Convert.ToDateTime(dtToDate.Value), Convert.ToInt32(Session["ActiveCurrency"].ToString().Split('~')[0]), Convert.ToInt32(Session["TradeCurrency"].ToString().Split('~')[0]));
            DataTable dtCashBankBook_New = dtCashBankBook.Copy();
            dtCashBankBook_New.Rows.Clear();
            DataRow newRow = dtCashBankBook_New.NewRow();
            newRow[1] = oconverter.ArrangeDate2(Convert.ToDateTime(dtDate.Value).ToShortDateString());
            newRow[4] = "Opening Balance";
            newRow[9] = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()) * (-1);
            dtCashBankBook_New.Rows.Add(newRow);
            for (int i = 0; i < dtCashBankBook.Rows.Count; i++)
            {
                newRow = dtCashBankBook_New.NewRow();
                newRow[0] = dtCashBankBook.Rows[i]["TrDate"];
                newRow[1] = dtCashBankBook.Rows[i]["ValueDate"];
                newRow[2] = dtCashBankBook.Rows[i]["accountsledger_TransactionReferenceID"];
                newRow[3] = dtCashBankBook.Rows[i]["accountsledger_Narration"];
                newRow[4] = dtCashBankBook.Rows[i]["AccountName"];
                newRow[5] = dtCashBankBook.Rows[i]["accountsledger_InstrumentNumber"];
                newRow[6] = dtCashBankBook.Rows[i]["InstDate"];
                newRow[7] = dtCashBankBook.Rows[i]["Accountsledger_AmountDr"];
                newRow[8] = dtCashBankBook.Rows[i]["Accountsledger_AmountCr"];
                newRow[9] = dtCashBankBook.Rows[i]["Closing"];
                newRow[10] = dtCashBankBook.Rows[i]["TrDate1"];
                newRow[11] = dtCashBankBook.Rows[i]["MainID"];
                newRow[12] = dtCashBankBook.Rows[i]["SubID"];
                newRow[13] = dtCashBankBook.Rows[i]["CompanyID"];
                newRow[14] = dtCashBankBook.Rows[i]["SegID"];
                dtCashBankBook_New.Rows.Add(newRow);
                if (dtCashBankBook.Rows[i]["Accountsledger_AmountDr"].ToString() != "")
                    receipt += Convert.ToDecimal(dtCashBankBook.Rows[i]["Accountsledger_AmountDr"].ToString());
                if (dtCashBankBook.Rows[i]["Accountsledger_AmountCr"].ToString() != "")
                    Payment += Convert.ToDecimal(dtCashBankBook.Rows[i]["Accountsledger_AmountCr"].ToString());
            }
            dtCashBankBook.Rows.Clear();
            dtCashBankBook = dtCashBankBook_New.Copy();
            pagecount = dtCashBankBook.Rows.Count / pageSize + 1;
            TotalPages.Value = pagecount.ToString();
            if (pageindex <= 0)
            {
                pageindex = 0;
                openingBal = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()) * (-1);
            //    Page.ClientScript.RegisterStartupScript(GetType(), "hide", "<script language='javascript'>Disable('P');</script>");
            }
            if (pageindex >= int.Parse(TotalPages.Value.ToString()))
            {
                pageindex = int.Parse(TotalPages.Value.ToString()) - 1;
            //    Page.ClientScript.RegisterStartupScript(GetType(), "hide", "<script language='javascript'>Disable('N');</script>");
            }
            if (pageindex >= (int.Parse(TotalPages.Value.ToString()) - 1))
            {
               // ScriptManager.RegisterStartupScript(this, this.GetType(), "hide", "Disable('N');", true);
                // Page.ClientScript.RegisterStartupScript(GetType(), "hide", "<script language='javascript'>Disable('N');</script>");
            }
            if (pageindex > 0)
            {
                int totalRecord = (pageindex) * pageSize;
                openingBal = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()) * (-1);
                decimal DR = 0;
                decimal CR = 0;
                for (int i = 0; i < totalRecord; i++)
                {
                    if (dtCashBankBook.Rows[i]["Accountsledger_AmountDr"].ToString() != "")
                        DR = decimal.Parse(dtCashBankBook.Rows[i]["Accountsledger_AmountDr"].ToString());
                    else
                        DR = 0;
                    if (dtCashBankBook.Rows[i]["Accountsledger_AmountCr"].ToString() != "")
                        CR = decimal.Parse(dtCashBankBook.Rows[i]["Accountsledger_AmountCr"].ToString());
                    else
                        CR = 0;
                    openingBal = DR - CR + openingBal;
                }
            }
            grdCashBankBook.PageIndex = pageindex;
            CurrentPage.Value = pageindex.ToString();
            rowcount = 0;
            grdCashBankBook.DataSource = dtCashBankBook;
            grdCashBankBook.DataBind();
            //Debjyoti
            aspxGrdCashBankBook.DataSource = dtCashBankBook;
            aspxGrdCashBankBook.DataBind();
            
            if (pageindex == int.Parse(TotalPages.Value.ToString()) - 1)
            {
                grdCashBankBook.FooterRow.Cells[4].Text = "Closing Balance";
                grdCashBankBook.FooterRow.Cells[9].Text = openingBal.ToString("c", currencyFormat);
                grdCashBankBook.FooterRow.Cells[7].Text = receipt.ToString("c", currencyFormat);
                grdCashBankBook.FooterRow.Cells[8].Text = Payment.ToString("c", currencyFormat);
                grdCashBankBook.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Left;
                grdCashBankBook.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Right;
                grdCashBankBook.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                grdCashBankBook.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                grdCashBankBook.FooterRow.Cells[4].ForeColor = System.Drawing.Color.White;
                grdCashBankBook.FooterRow.Cells[9].ForeColor = System.Drawing.Color.White;
                grdCashBankBook.FooterRow.Cells[7].ForeColor = System.Drawing.Color.White;
                grdCashBankBook.FooterRow.Cells[8].ForeColor = System.Drawing.Color.White;
                grdCashBankBook.FooterRow.Cells[4].Font.Bold = true;
                grdCashBankBook.FooterRow.Cells[9].Font.Bold = true;
                grdCashBankBook.FooterRow.Cells[7].Font.Bold = true;
                grdCashBankBook.FooterRow.Cells[8].Font.Bold = true;
                grdCashBankBook.FooterRow.Cells[9].Wrap = false;
                grdCashBankBook.FooterRow.Cells[7].Wrap = false;
                grdCashBankBook.FooterRow.Cells[8].Wrap = false;
            }
            dtSearchDate.Value = "01-01-0100";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "ShowHideSearch()", true);
        }

        protected void grdCashBankBook_RowCreated(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#FFE9BA';");
            //    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#FFFFFF';");
            //}
            string rowID = String.Empty;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                rowID = "row" + e.Row.RowIndex;
                e.Row.Attributes.Add("id", "row" + e.Row.RowIndex);
                e.Row.Attributes.Add("onclick", "ChangeRowColor(" + "'" + rowID + "','" + dtCashBankBook.Rows.Count + "'" + ")");
            }

        }
        protected void grdCashBankBook_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            if (GridViewSortDirection == SortDirection.Ascending)
            {
                GridViewSortDirection = SortDirection.Descending;
                SortGridView(sortExpression, " DESC");
            }
            else
            {
                GridViewSortDirection = SortDirection.Ascending;
                SortGridView(sortExpression, " ASC");
            }
        }
        public SortDirection GridViewSortDirection
        {

            get
            {

                if (ViewState["sortDirection"] == null)

                    ViewState["sortDirection"] = SortDirection.Ascending;

                return (SortDirection)ViewState["sortDirection"];

            }

            set { ViewState["sortDirection"] = value; }

        }
        private void SortGridView(string sortExpression, string direction)
        {

            // You can cache the DataTable for improving performance

            //DataTable dt = dtSubsidiary;
            dtCashBankBook = (DataTable)ViewState["dtCashBankBook"];
            DataView dv = new DataView(dtCashBankBook);
            dv.Sort = sortExpression + direction;
            grdCashBankBook.DataSource = dv;
            grdCashBankBook.DataBind();

            //Debjyoti 
            aspxGrdCashBankBook.DataSource = dv;
            aspxGrdCashBankBook.DataBind();
        }
        protected void ddlExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo("en-us").NumberFormat;
            currencyFormat.CurrencySymbol = "";
            currencyFormat.CurrencyNegativePattern = 2;
            decimal receipt = 0;
            decimal Payment = 0;
            decimal openingBal = 0;

            if (rdbSegAll.Checked == true)
            {
                DataTable dtSegment = oDBEngine.GetDataTable("tbl_master_companyExchange", "exch_internalId", "exch_compId='" + Session["LastCompany"].ToString() + "'");
                if (dtSegment.Rows.Count > 0)
                {
                    for (int i = 0; i < dtSegment.Rows.Count; i++)
                    {
                        if (Segment == null)
                            Segment = dtSegment.Rows[i][0].ToString();
                        else
                            Segment += "," + dtSegment.Rows[i][0].ToString();
                    }
                }
                //if (Session["userlastsegment"].ToString() == "5")
                //{
                //    Segment += "0" + Segment;
                //}
            }
            else
            {
                Segment = HDNSeg.Value;
            }
            if (Segment == null || Segment == "")
            {
                Segment = Session["SegmentID"].ToString();
            }

            DataTable dtCashBankBook1 = oDBEngine.GetDataTable("(select accountsledger_MainAccountID,accountsledger_transactiondate,accountsledger_TransactionReferenceID,Accountsledger_AmountDr ,Accountsledger_AmountCr,accountsledger_companyID,accountsledger_ExchangeSegmentID from Trans_accountsledger where accountsledger_companyID='" + Session["LastCompany"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and accountsledger_TransactionType='Cash_Bank' and accountsledger_transactiondate between ('" + dtDate.Value + "') and ('" + dtToDate.Value + "') and accountsledger_MainAccountID ='" + txtBankName_hidden.Value + "'and AccountsLedger_BranchID in(" + Branch + ")  and AccountsLedger_FinYear='" + Session["LastFinYear"].ToString() + "') as D,Trans_accountsledger a", " convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,convert(varchar(11),a.accountsledger_valuedate,113) as ValueDate,a.accountsledger_TransactionReferenceID,a.accountsledger_Narration,isnull((select isnull(mainaccount_name,'') from master_mainaccount where mainaccount_accountcode=a.accountsledger_MainAccountID),'')+'-'+	isnull((case when isnumeric(a.accountsledger_SubAccountID)=0 	then (select top 1 isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'')+' ['+isnull(ltrim(rtrim(cnt_UCC)),'')+']' from tbl_master_contact where ltrim(rtrim(cnt_internalid))=ltrim(rtrim(a.accountsledger_SubAccountID))) else (select top 1 rtrim(SubAccount_Name)+' ['+isnull(ltrim(rtrim(SubAccount_Code)),'')+']' from master_subAccount where SubAccount_Code = ltrim(rtrim(a.accountsledger_SubAccountID)))end),'') as AccountName,a.accountsledger_InstrumentNumber,convert(varchar(11),a.accountsledger_InstrumentDate,113) as InstDate,case when a.Accountsledger_AmountCr='0.00000000' then null else cast(a.Accountsledger_AmountCr as varchar(max)) end as Accountsledger_AmountDr,case when a.Accountsledger_AmountDr='0.00000000' then null else cast(a.Accountsledger_AmountDr as varchar(max)) end as Accountsledger_AmountCr,'0.0' as Closing,a.accountsledger_transactiondate as TrDate1,a.accountsledger_MainAccountID as MainID,a.accountsledger_SubAccountID as SubID,a.accountsledger_companyID as CompanyID,a.accountsledger_exchangeSegmentID as SegID ", " a.accountsledger_transactiondate=D.accountsledger_transactiondate and a.accountsledger_companyID=D.accountsledger_companyID and a.accountsledger_ExchangeSegmentID=D.accountsledger_ExchangeSegmentID and a.accountsledger_TransactionReferenceID=D.accountsledger_TransactionReferenceID and a.accountsledger_MainAccountID<>D.accountsledger_MainAccountID order by a.accountsledger_transactiondate,a.accountsledger_TransactionReferenceID desc ");
            DataTable OpenBalance = oDBEngine.OpeningBalance(txtBankName_hidden.Value, null, Convert.ToDateTime(dtDate.Value), Segment, Session["LastCompany"].ToString(), Convert.ToDateTime(dtToDate.Value), Convert.ToInt32(Session["ActiveCurrency"].ToString().Split('~')[0]), Convert.ToInt32(Session["TradeCurrency"].ToString().Split('~')[0]));
            DataTable dtCashBankBook_New = dtCashBankBook1.Copy();
            dtCashBankBook_New.Rows.Clear();
            DataRow newRow = dtCashBankBook_New.NewRow();
            decimal closingRate = 0;
            for (int j = 0; j < dtCashBankBook1.Rows.Count; j++)
            {
                newRow = dtCashBankBook_New.NewRow();
                newRow[0] = dtCashBankBook1.Rows[j]["TrDate"];
                newRow[1] = dtCashBankBook1.Rows[j]["ValueDate"];
                newRow[2] = dtCashBankBook1.Rows[j]["accountsledger_TransactionReferenceID"];
                newRow[3] = dtCashBankBook1.Rows[j]["accountsledger_Narration"];
                newRow[4] = dtCashBankBook1.Rows[j]["AccountName"];
                newRow[5] = dtCashBankBook1.Rows[j]["accountsledger_InstrumentNumber"];
                newRow[6] = dtCashBankBook1.Rows[j]["InstDate"];
                newRow[7] = dtCashBankBook1.Rows[j]["Accountsledger_AmountDr"];
                newRow[8] = dtCashBankBook1.Rows[j]["Accountsledger_AmountCr"];
                string Dr = "0";
                string Cr = "0";
                if (dtCashBankBook1.Rows[j]["Accountsledger_AmountDr"].ToString() != "")
                {
                    Dr = dtCashBankBook1.Rows[j]["Accountsledger_AmountDr"].ToString();
                }
                if (dtCashBankBook1.Rows[j]["Accountsledger_AmountCr"].ToString() != "")
                    Cr = dtCashBankBook1.Rows[j]["Accountsledger_AmountCr"].ToString();
                if (j == 0)
                {
                    newRow[9] = decimal.Parse(Dr) - decimal.Parse(Cr) + (Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()) * (-1));
                    closingRate = decimal.Parse(Dr) - decimal.Parse(Cr) + (Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()) * (-1));
                }
                else
                {
                    newRow[9] = decimal.Parse(Dr) - decimal.Parse(Cr) + closingRate;
                    closingRate = decimal.Parse(Dr) - decimal.Parse(Cr) + closingRate;
                }
                newRow[10] = dtCashBankBook1.Rows[j]["TrDate1"];
                newRow[11] = dtCashBankBook1.Rows[j]["MainID"];
                newRow[12] = dtCashBankBook1.Rows[j]["SubID"];
                newRow[13] = dtCashBankBook1.Rows[j]["CompanyID"];
                newRow[14] = dtCashBankBook1.Rows[j]["SegID"];
                dtCashBankBook_New.Rows.Add(newRow);
                if (dtCashBankBook1.Rows[j]["Accountsledger_AmountDr"].ToString() != "")
                    receipt += Convert.ToDecimal(dtCashBankBook1.Rows[j]["Accountsledger_AmountDr"].ToString());
                if (dtCashBankBook1.Rows[j]["Accountsledger_AmountCr"].ToString() != "")
                    Payment += Convert.ToDecimal(dtCashBankBook1.Rows[j]["Accountsledger_AmountCr"].ToString());
            }
            dtCashBankBook1.Rows.Clear();
            dtCashBankBook1 = dtCashBankBook_New.Copy();
            DataTable dtCashBankBook_New1 = dtCashBankBook1.Copy();
            dtCashBankBook_New1.Rows.Clear();
            DataRow newRow1 = dtCashBankBook_New1.NewRow();
            newRow1[1] = oconverter.ArrangeDate2(Convert.ToDateTime(dtDate.Value).ToShortDateString());
            newRow1[4] = "Opening Balance";
            if (Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()) < 0)
                newRow1[9] = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()) * (-1);
            else
                newRow1[9] = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString());
            dtCashBankBook_New1.Rows.Add(newRow1);
            for (int i = 0; i < dtCashBankBook1.Rows.Count; i++)
            {
                newRow1 = dtCashBankBook_New1.NewRow();
                newRow1[0] = dtCashBankBook1.Rows[i]["TrDate"];
                newRow1[1] = dtCashBankBook1.Rows[i]["ValueDate"];
                newRow1[2] = dtCashBankBook1.Rows[i]["accountsledger_TransactionReferenceID"];
                newRow1[3] = dtCashBankBook1.Rows[i]["accountsledger_Narration"];
                newRow1[4] = dtCashBankBook1.Rows[i]["AccountName"];
                newRow1[5] = dtCashBankBook1.Rows[i]["accountsledger_InstrumentNumber"];
                newRow1[6] = dtCashBankBook1.Rows[i]["InstDate"];
                newRow1[7] = dtCashBankBook1.Rows[i]["Accountsledger_AmountDr"];
                newRow1[8] = dtCashBankBook1.Rows[i]["Accountsledger_AmountCr"];
                newRow1[9] = dtCashBankBook1.Rows[i]["Closing"];
                newRow1[10] = dtCashBankBook1.Rows[i]["TrDate1"];
                newRow1[11] = dtCashBankBook1.Rows[i]["MainID"];
                newRow1[12] = dtCashBankBook1.Rows[i]["SubID"];
                newRow1[13] = dtCashBankBook1.Rows[i]["CompanyID"];
                newRow1[14] = dtCashBankBook1.Rows[i]["SegID"];
                dtCashBankBook_New1.Rows.Add(newRow1);
                openingBal = decimal.Parse(dtCashBankBook1.Rows[i]["Closing"].ToString());
            }
            if (dtCashBankBook1.Rows.Count == 0)
            {
                if (Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()) < 0)
                    openingBal = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()) * (-1);
                else
                    openingBal = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString());
            }
            dtCashBankBook1.Rows.Clear();
            dtCashBankBook1 = dtCashBankBook_New1.Copy();
            DataRow DrRow1 = dtCashBankBook1.NewRow();
            dtCashBankBook1.Rows.Add(DrRow1);
            DataRow DrRow = dtCashBankBook1.NewRow();
            DrRow[4] = "Closing Balance";
            DrRow[9] = openingBal.ToString("c", currencyFormat);
            if (receipt != 0)
                DrRow[7] = receipt.ToString("c", currencyFormat);

            if (Payment != 0)
                DrRow[8] = Payment.ToString("c", currencyFormat);

            dtCashBankBook1.Rows.Add(DrRow);
            dtCashBankBook1.AcceptChanges();
            if (dtCashBankBook1.Rows.Count > 0)
            {
                for (int k = 0; k < dtCashBankBook1.Rows.Count; k++)
                {
                    if (dtCashBankBook1.Rows[k]["Accountsledger_AmountDr"].ToString() != "")
                        dtCashBankBook1.Rows[k]["Accountsledger_AmountDr"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtCashBankBook1.Rows[k]["Accountsledger_AmountDr"].ToString()));
                    if (dtCashBankBook1.Rows[k]["Accountsledger_AmountCr"].ToString() != "")
                        dtCashBankBook1.Rows[k]["Accountsledger_AmountCr"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtCashBankBook1.Rows[k]["Accountsledger_AmountCr"].ToString()));
                    if (dtCashBankBook1.Rows[k]["Closing"].ToString() != "")
                        dtCashBankBook1.Rows[k]["Closing"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtCashBankBook1.Rows[k]["Closing"].ToString()));

                }
            }
            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            DataTable dtReportHeader = new DataTable();
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            DrRowR1[0] = "Cash/Bank Book For the  Period [" + oconverter.ArrangeDate2(dtDate.Value.ToString()) + " To " + oconverter.ArrangeDate2(dtToDate.Value.ToString()) + "]";
            dtReportHeader.Rows.Add(DrRowR1);
            DataRow DrRowR2 = dtReportHeader.NewRow();
            DrRowR2[0] = txtBankName.Text;
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
            //FooterRow[0] = "* * *  End Of Report * * *         [" + oconverter.ArrangeDate2(oDBEngine.GetDate().ToString(), "Test") + "]";
            FooterRow[0] = "* * *  End Of Report * * *   ";
            dtReportFooter.Rows.Add(FooterRow);

            DataTable dtExport = new DataTable();
            dtExport = dtCashBankBook1.Copy();
            dtExport.Columns[2].ColumnName = "Reference ID";
            dtExport.Columns[3].ColumnName = "Narration";
            dtExport.Columns[5].ColumnName = "Inst.Number";
            dtExport.Columns[7].ColumnName = "Debit";
            dtExport.Columns[8].ColumnName = "Credit";
            dtExport.Columns.Remove("TrDate1");
            dtExport.Columns.Remove("MainID");
            dtExport.Columns.Remove("SubID");
            dtExport.Columns.Remove("CompanyID");
            dtExport.Columns.Remove("SegID");
            dtExport.AcceptChanges();
            if (ddlExport.SelectedItem.Value == "E")
            {
                //oconverter.ExcelImport(dtBilling, "Daily Billing");
                objExcel.ExportToExcelforExcel(dtExport, "Cash Bank Book", "Closing Balance", dtReportHeader, dtReportFooter);
            }
            else if (ddlExport.SelectedItem.Value == "P")
            {
                objExcel.ExportToPDF(dtExport, "Cash Bank Book", "Closing Balance", dtReportHeader, dtReportFooter);
            }
        }


        #region New Changes
        protected void aspxGrdCashBankBook_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            BtnclickShow = "Click";
            FillGrid();
        }

      
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
            }
        }

        public void bindexport(int Filter)
        {

            string filename = "Cash/Bank";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Cash/Bank";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";

            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsToResponse();
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }
        }

        #endregion


    }
}