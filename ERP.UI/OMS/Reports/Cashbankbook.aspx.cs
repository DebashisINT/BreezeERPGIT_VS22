using System;
using System.Data;
using System.Web.UI;
using DevExpress.Web;
//using DevExpress.Web;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_Cashbankbook : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        DataSet ds = new DataSet();
        BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.GenericExcelExport oGenericExcelExport;
        string WhichCall;
        DataSet Ds_Global;
        string Segment;
        string branch;
        int TotalItems = 0;
        int TotalPage = 0;
        string TotalAmoutDr = "0.00";
        string TotalAmoutCr = "0.00";
        string TotalAmoutClosing = "0.00";
        AspxHelper oAspxHelper = new AspxHelper();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        #region Page Properties
        public string P_PageSize
        {
            get { return (string)ViewState["PageSize"]; }
            set { ViewState["PageSize"] = value; }
        }
        public string P_PageNumAfterNav
        {
            get { return (string)Session["PageNumAfterNav"]; }
            set { Session["PageNumAfterNav"] = value; }
        }
        public string _TotalAmountDr
        {
            get { return TotalAmoutDr; }
            set { TotalAmoutDr = value; }
        }
        public string _TotalAmountCr
        {
            get { return TotalAmoutCr; }
            set { TotalAmoutCr = value; }
        }
        public string _TotalAmountClosing
        {
            get { return TotalAmoutClosing; }
            set { TotalAmoutClosing = value; }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load()</script>");
            if (!IsPostBack)
            {
                P_PageSize = "10";
                string fDate = null;
                string tDate = null;
                fDate = Session["FinYearStart"].ToString();
                tDate = Session["FinYearEnd"].ToString();
                dtDate.Value = Convert.ToDateTime(fDate);
                dtToDate.Value = Convert.ToDateTime(tDate);

            }

            String cbReference1 = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveSvrData", "context");
            String callbackScript1 = "function CallServer(arg, context){ " + cbReference1 + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript1, true);
            txtbank.Attributes.Add("onkeyup", "CallAjax(this,'GenericAjaxList',event,'" + Gm.GetCashBankAccAQuery("S", Session["Usersegid"].ToString(), "CashBank") + "')");


        }


        protected void gridasset_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != DevExpress.Web.GridViewRowType.Data) return;
            int rowindex = e.VisibleIndex;
            string closing = gridasset.GetRowValues(rowindex, "FinalClosing").ToString();
            string company = gridasset.GetRowValues(rowindex, "CompanyID").ToString();
            string mainid = gridasset.GetRowValues(rowindex, "MainID").ToString();
            string subid = gridasset.GetRowValues(rowindex, "SubID").ToString();
            string referenceid = gridasset.GetRowValues(rowindex, "TransactionReferenceID").ToString();
            string tradedate = gridasset.GetRowValues(rowindex, "TrDate").ToString();
            string segid = gridasset.GetRowValues(rowindex, "SegID").ToString();
            if (closing.Contains("-"))
            {
                e.Row.Cells[9].Style.Add("color", "Red");
            }
            if (tradedate != "")
            {

                if (Session["LCKBNK"] != null)
                {
                    if (Convert.ToDateTime(tradedate) >= Convert.ToDateTime(Session["LCKBNK"].ToString()))
                    {
                        if (referenceid != "")
                        {
                            e.Row.Cells[2].Style.Add("cursor", "hand");
                            e.Row.Cells[2].ToolTip = "View Details!";
                            e.Row.Cells[2].Attributes.Add("onclick", "javascript:updateCashBankDetail('" + tradedate + "','" + referenceid + "','" + mainid + "','" + subid + "','" + company + "','" + segid + "');");
                        }
                    }
                    else
                    {
                        e.Row.Cells[2].ToolTip = "Voucher Locked !";
                    }
                }
                else
                {
                    if (referenceid != "")
                    {
                        e.Row.Cells[2].Style.Add("cursor", "hand");
                        e.Row.Cells[2].ToolTip = "View Details!";
                        e.Row.Cells[2].Attributes.Add("onclick", "javascript:updateCashBankDetail('" + tradedate + "','" + referenceid + "','" + mainid + "','" + subid + "','" + company + "','" + segid + "');");
                    }

                }
            }





        }

        #region Global Variable Declaration
        string data;
        BusinessLogicLayer.GenericMethod Gm = new BusinessLogicLayer.GenericMethod();
        string CombinedGroupByQuery = string.Empty;
        #endregion

        #region UserdefinedMethod
        void Callajaxfromjavascript(string WhichCall)
        {
            string combinedqueryforproduct = "";
            string CombinedISINQuery = string.Empty;
            CombinedGroupByQuery = CombinedISINQuery;
            if (WhichCall.Contains("Ajax-Branch"))
            {
                string combinedqueryforclient = Gm.GetAllBranch();
                CombinedGroupByQuery = combinedqueryforclient;

            }
            if (WhichCall.Contains("Ajax-Segment"))
            {
                Gm.GetSegments("A", ref combinedqueryforproduct, 10, Session["LastCompany"].ToString(), null);
                CombinedGroupByQuery = combinedqueryforproduct;

            }

        }

        #endregion
        #region CallbackEvent and Syncronization With JavaScript

        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();

            if (id.Contains("Ajax-Branch") || id.Contains("Ajax-Segment"))
            {
                Callajaxfromjavascript(id);
                CombinedGroupByQuery = CombinedGroupByQuery.Replace("\\'", "'");
                data = "AjaxQuery@" + CombinedGroupByQuery;
            }
            else
            {
                string[] idlist = id.Split('^');
                string[] cl = idlist[1].Split('!');
                string Receiveserverid = "";
                for (int i = 0; i < cl.Length; i++)
                {
                    if (idlist[0] != "Clients")
                    {
                        string[] val = cl[i].Split(';');
                        if (Receiveserverid == "")
                            Receiveserverid = val[0].Split('~')[0];
                        else
                            Receiveserverid += "," + val[0].Split('~')[0];
                    }
                }

                if (idlist[0] == "Segment")
                {

                    data = "Segment@" + Receiveserverid;
                }
                else
                {
                    data = "Branch@" + Receiveserverid;
                }

            }
        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }

        #endregion

        protected void gridasset_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            gridasset.JSProperties["cpExcelExport"] = null;
            string strHeadercompany = String.Empty;
            string strHeaderbankname = String.Empty;
            DataTable dtbankname = oDbEngine.GetDataTable("Select top 1 MainAccount_AccountCode+'-'+MainAccount_Name+' [ '+MainAccount_BankAcNumber+' ]' from Master_MainAccount where MainAccount_AccountCode='" + txtbank_hidden.Text.ToString().Split('~')[0].ToString().Trim() + "'");
            strHeaderbankname = "Bank Name : - " + dtbankname.Rows[0][0].ToString();
            WhichCall = e.Parameters.Split('~')[0];
            string test = txtbank_hidden.Text.ToString().Split('~')[0];
            if (rdbSgment.Value == "A")
            {
                DataTable dtSegment = oDbEngine.GetDataTable("tbl_master_companyExchange", "exch_internalId", "exch_compId='" + Session["LastCompany"].ToString() + "'");
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
            }
            else
            {
                Segment = HiddenField_Segment.Value.ToString();
            }
            if (rdbBranch.Value == "A")
            {
                branch = Session["userbranchHierarchy"].ToString();
            }

            else
            {

                branch = HiddenField_Branch.Value.ToString();
            }

            if (WhichCall == "Show")
            {
                Ds_Global = new DataSet();
                Ds_Global = Cashbank(test, dtDate.Text.Split('-')[2] + "-" + dtDate.Text.Split('-')[1] + "-" + dtDate.Text.Split('-')[0],
                                     dtToDate.Text.Split('-')[2] + "-" + dtToDate.Text.Split('-')[1] + "-" + dtToDate.Text.Split('-')[0], Segment,
                                     Session["LastCompany"].ToString(), Session["LastFinYear"].ToString(), "", branch, Session["ActiveCurrency"].ToString().Split('~')[0],
                                     Session["TradeCurrency"].ToString().Split('~')[0], P_PageSize, "1", "N");
                if (Ds_Global.Tables.Count > 0)
                {
                    if (Ds_Global.Tables[0].Rows.Count > 0)
                    {
                        //TotalItems = Convert.ToInt32(Ds_Global.Tables[0].Rows[0]["TotalRecord"].ToString());
                        gridasset.Caption = strHeaderbankname + "  From " + oconverter.ArrangeDate2(dtDate.Value.ToString()) + "  To  " + oconverter.ArrangeDate2(dtToDate.Value.ToString());
                        TotalItems = Convert.ToInt32(Ds_Global.Tables[1].Rows[0][0].ToString());
                        TotalPage = TotalItems % Convert.ToInt32(P_PageSize) == 0 ? (TotalItems / Convert.ToInt32(P_PageSize)) : (TotalItems / Convert.ToInt32(P_PageSize)) + 1;
                        gridasset.JSProperties["cpRefreshNavPanel"] = "ShowBtnClick~1~" + TotalPage.ToString() + '~' + TotalItems.ToString();
                        oAspxHelper.BindGrid(gridasset, Ds_Global);

                    }
                    else
                        oAspxHelper.BindGrid(gridasset);
                }

            }
            if (WhichCall == "SearchByNavigation")
            {
                Ds_Global = new DataSet();
                string strPageNum = String.Empty;
                string strNavDirection = String.Empty;
                int PageNumAfterNav = 0;
                strPageNum = e.Parameters.Split('~')[3];
                strNavDirection = e.Parameters.Split('~')[4];

                //Set Page Number
                if (strNavDirection == "RightNav")
                    PageNumAfterNav = Convert.ToInt32(strPageNum) + 10;
                if (strNavDirection == "LeftNav")
                    PageNumAfterNav = Convert.ToInt32(strPageNum) - 10;
                if (strNavDirection == "PageNav")
                    PageNumAfterNav = Convert.ToInt32(strPageNum);


                Ds_Global = Cashbank(test, dtDate.Text.Split('-')[2] + "-" + dtDate.Text.Split('-')[1] + "-" + dtDate.Text.Split('-')[0],
                                    dtToDate.Text.Split('-')[2] + "-" + dtToDate.Text.Split('-')[1] + "-" + dtToDate.Text.Split('-')[0], Segment,
                                    Session["LastCompany"].ToString(), Session["LastFinYear"].ToString(), "", branch, Session["ActiveCurrency"].ToString().Split('~')[0],
                                    Session["TradeCurrency"].ToString().Split('~')[0], P_PageSize, PageNumAfterNav.ToString(), "N");

                if (Ds_Global.Tables.Count > 0)
                {
                    if (Ds_Global.Tables[0].Rows.Count > 0)
                    {
                        //TotalItems = Convert.ToInt32(Ds_Global.Tables[0].Rows[0]["TotalRecord"].ToString());
                        //TotalItems = 20;
                        gridasset.Caption = strHeaderbankname + "  From " + oconverter.ArrangeDate2(dtDate.Value.ToString()) + "  To  " + oconverter.ArrangeDate2(dtToDate.Value.ToString());
                        TotalItems = Convert.ToInt32(Ds_Global.Tables[1].Rows[0][0].ToString());
                        TotalPage = TotalItems % Convert.ToInt32(P_PageSize) == 0 ? (TotalItems / Convert.ToInt32(P_PageSize)) : (TotalItems / Convert.ToInt32(P_PageSize)) + 1;
                        //GrdEmployee.JSProperties["cpPagerSetting"] = strPageNum + "~" + TotalPage + "~" + TotalItems;
                        oAspxHelper.BindGrid(gridasset, Ds_Global);
                        gridasset.JSProperties["cpRefreshNavPanel"] = strNavDirection + '~' + strPageNum + '~' + TotalPage.ToString() + '~' + TotalItems.ToString();
                    }
                    else
                        oAspxHelper.BindGrid(gridasset);
                }
                else
                    oAspxHelper.BindGrid(gridasset);

                P_PageNumAfterNav = PageNumAfterNav.ToString();
            }
            if (WhichCall == "ExcelExport")
            {
                gridasset.JSProperties["cpExcelExport"] = "T";
            }
            if (WhichCall == "Callfromanotherpages")
            {
                Ds_Global = new DataSet();
                if (P_PageNumAfterNav == null)
                    P_PageNumAfterNav = "1";
                Ds_Global = Cashbank(test, dtDate.Text.Split('-')[2] + "-" + dtDate.Text.Split('-')[1] + "-" + dtDate.Text.Split('-')[0],
                                     dtToDate.Text.Split('-')[2] + "-" + dtToDate.Text.Split('-')[1] + "-" + dtToDate.Text.Split('-')[0], Segment,
                                     Session["LastCompany"].ToString(), Session["LastFinYear"].ToString(), "", branch, Session["ActiveCurrency"].ToString().Split('~')[0],
                                     Session["TradeCurrency"].ToString().Split('~')[0], P_PageSize, P_PageNumAfterNav, "N");
                if (Ds_Global.Tables.Count > 0)
                {
                    if (Ds_Global.Tables[0].Rows.Count > 0)
                    {
                        //TotalItems = Convert.ToInt32(Ds_Global.Tables[0].Rows[0]["TotalRecord"].ToString());
                        gridasset.Caption = strHeaderbankname + "  From " + oconverter.ArrangeDate2(dtDate.Value.ToString()) + "  To  " + oconverter.ArrangeDate2(dtToDate.Value.ToString());
                        TotalItems = Convert.ToInt32(Ds_Global.Tables[1].Rows[0][0].ToString());
                        TotalPage = TotalItems % Convert.ToInt32(P_PageSize) == 0 ? (TotalItems / Convert.ToInt32(P_PageSize)) : (TotalItems / Convert.ToInt32(P_PageSize)) + 1;
                        gridasset.JSProperties["cpRefreshNavPanel"] = "PageNav" + '~' + P_PageNumAfterNav + '~' + TotalPage.ToString() + '~' + TotalItems.ToString();
                        oAspxHelper.BindGrid(gridasset, Ds_Global);

                    }
                    else
                        oAspxHelper.BindGrid(gridasset);
                }
                P_PageNumAfterNav = null;

            }
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdbSgment.Value == "A")
            {
                DataTable dtSegment = oDbEngine.GetDataTable("tbl_master_companyExchange", "exch_internalId", "exch_compId='" + Session["LastCompany"].ToString() + "'");
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
            }
            else
            {
                Segment = HiddenField_Segment.Value.ToString();
            }
            if (rdbBranch.Value == "A")
            {
                branch = Session["userbranchHierarchy"].ToString();
            }

            else
            {

                branch = HiddenField_Branch.Value.ToString();
            }

            Ds_Global = new DataSet();

            Ds_Global = Cashbank(txtbank_hidden.Text.ToString().Split('~')[0], dtDate.Text.Split('-')[2] + "-" + dtDate.Text.Split('-')[1] + "-" + dtDate.Text.Split('-')[0],
                                    dtToDate.Text.Split('-')[2] + "-" + dtToDate.Text.Split('-')[1] + "-" + dtToDate.Text.Split('-')[0], Segment,
                                    Session["LastCompany"].ToString(), Session["LastFinYear"].ToString(), "", branch, Session["ActiveCurrency"].ToString().Split('~')[0],
                                    Session["TradeCurrency"].ToString().Split('~')[0], "1", "10", "Y");
            ExportToExcel(Ds_Global, dtDate.Text.Split('-')[2] + "-" + dtDate.Text.Split('-')[1] + "-" + dtDate.Text.Split('-')[0], dtToDate.Text.Split('-')[2] + "-" + dtToDate.Text.Split('-')[1] + "-" + dtToDate.Text.Split('-')[0]);
        }
        void ExportToExcel(DataSet DsExport, string FromDOJ, string ToDOJ)
        {
            oGenericExcelExport = new BusinessLogicLayer.GenericExcelExport();
            DataTable DtExport = new DataTable();
            string strHeadercompany = String.Empty;
            string strHeaderbankname = String.Empty;
            string strHeader = String.Empty;
            string[] ReportHeader = new string[3];
            string strSavePath = String.Empty;

            strHeader = "Cashbank Book From " + Convert.ToDateTime(FromDOJ == "" ? "1900-01-01" : FromDOJ).ToString("dd-MMM-yyyy") + " To " +
                Convert.ToDateTime(ToDOJ == "" ? "9999-12-31" : ToDOJ).ToString("dd-MMM-yyyy");
            DataTable dtheader = oDbEngine.GetDataTable("select ltrim(rtrim(cmp_name)) from tbl_master_company where cmp_internalid='" + Session["LastCompany"].ToString().Trim() + "'");
            strHeadercompany = dtheader.Rows[0][0].ToString();
            DataTable dtbankname = oDbEngine.GetDataTable("Select top 1 MainAccount_AccountCode+'-'+MainAccount_Name+' [ '+MainAccount_BankAcNumber+' ]' from Master_MainAccount where MainAccount_AccountCode='" + txtbank_hidden.Text.ToString().Split('~')[0].ToString().Trim() + "'");
            strHeaderbankname = "Bank Name : - " + dtbankname.Rows[0][0].ToString();
            //strHeader = strHeader + (SearchBy != "" ? (SearchBy == "EC" ? " (Search By : Employee Code " + (FindOption == "0" ? " Like '" : " = '") + SearchString :
            //    " (Search By : Employee Name " + (FindOption == "0" ? " Like '" : " = '") + SearchString) : "");

            string exlDateTime = oDbEngine.GetDate(113).ToString();
            string exlTime = exlDateTime.Replace(":", "");
            exlTime = exlTime.Replace(" ", "");

            if (DsExport.Tables.Count > 0)
                if (DsExport.Tables[0].Rows.Count > 0)
                {
                    DtExport = DsExport.Tables[0];
                    DtExport.Columns.Remove("finaldr");
                    DtExport.Columns.Remove("finalcr");
                    DtExport.Columns.Remove("finalcl");
                    DtExport.AcceptChanges();
                    ReportHeader[0] = strHeader;
                    ReportHeader[1] = strHeaderbankname;
                    ReportHeader[2] = strHeadercompany;
                    string FileName = "CashbankBook_" + exlTime;
                    strSavePath = "~/Documents/";

                    //SRLNO,Name,FatherName,DOJ,Department,BranchName,CTC,ReportTo,Designation,Company,
                    //Email_Ids,PhoneMobile_Numbers,PanCardNumber,CreatedBy
                    string[] ColumnType = { "V", "V", "V", "V", "V", "V", "V", "N", "N", "N" };
                    string[] ColumnSize = { "100", "100", "50", "100", "100", "100", "50", "28,2", "28,2", "28,2" };
                    string[] ColumnWidthSize = { "10", "10", "12", "50", "75", "20", "12", "20", "20", "20" };

                    oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, DtExport, Server.MapPath(strSavePath), "2007", FileName, ReportHeader, null);
                }
        }



        DataSet Cashbank(string mainaccountid, string fromdate, string todate, string segment,
                          string company, string finyear, string RedirectTo, string branch, string activecurrency,
                          string tradecurrency, string pagesize, string pageno, string Export)
        {
            string[] InputName = new string[13];
            string[] InputType = new string[13];
            string[] InputValue = new string[13];

            DataSet DsCashBank = new DataSet();
            InputName[0] = "mainaccountid";
            InputName[1] = "fromdate";
            InputName[2] = "todate";
            InputName[3] = "segment";
            InputName[4] = "company";
            InputName[5] = "finyear";

            InputName[6] = "BranchID";
            InputName[7] = "RedirectTo";
            InputName[8] = "ActiveCurrency";
            InputName[9] = "TradeCurrency";
            InputName[10] = "PageSize";
            InputName[11] = "PageNumber";
            InputName[12] = "Export";

            InputType[0] = "V";
            InputType[1] = "V";
            InputType[2] = "V";
            InputType[3] = "V";
            InputType[4] = "V";
            InputType[5] = "V";
            InputType[6] = "V";
            InputType[7] = "V";
            InputType[8] = "V";
            InputType[9] = "V";
            InputType[10] = "I";
            InputType[11] = "I";
            InputType[12] = "V";

            InputValue[0] = mainaccountid;
            InputValue[1] = fromdate;
            InputValue[2] = todate;
            InputValue[3] = segment;
            InputValue[4] = company;
            InputValue[5] = finyear;

            InputValue[6] = branch;
            InputValue[7] = RedirectTo;
            InputValue[8] = activecurrency;
            InputValue[9] = tradecurrency;
            InputValue[10] = pagesize;
            InputValue[11] = pageno;
            InputValue[12] = Export;

            DsCashBank = BusinessLogicLayer.SQLProcedures.SelectProcedureArrDS("Report_Cashbankbook", InputName, InputType, InputValue);
            if (DsCashBank.Tables.Count > 0)
                if (DsCashBank.Tables[0].Rows.Count > 0)
                {
                    //if (Export == "N")
                    //{
                    TotalAmoutDr = (DsCashBank.Tables[0].Rows[0]["finaldr"].ToString());
                    TotalAmoutCr = (DsCashBank.Tables[0].Rows[0]["finalcr"].ToString());
                    TotalAmoutClosing = (DsCashBank.Tables[0].Rows[0]["finalcl"].ToString());
                    //}
                    return DsCashBank;
                }
            return null;
        }
        //protected string GetSummaryText(GridViewFooterCellTemplateContainer container)
        //{
        //    return "Closing";
        //}
        protected string GetSummaryValueDr(object c)
        {
            return TotalAmoutDr;
        }
        protected string GetSummaryValueCr(object c)
        {
            return TotalAmoutCr;
        }
        protected string GetSummaryValueClosing(object c)
        {
            return TotalAmoutClosing;
        }
    }
}