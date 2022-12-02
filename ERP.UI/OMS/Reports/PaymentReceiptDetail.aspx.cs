using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;

namespace ERP.OMS.Reports
{
    public partial class Reports_PaymentReceiptDetail : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        DataSet ds = new DataSet();
        string data;
        string PageNum = null, PageSize = null;
        ExcelFile objExcel = new ExcelFile();
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
               // Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {

                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
                FnSegment();
                Date();
            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//

        }
        void Date()
        {
            DtFromDate.EditFormatString = oconverter.GetDateFormat("Date");
            DtToDate.EditFormatString = oconverter.GetDateFormat("Date");

            DataTable dtFinYear = oDBEngine.GetDataTable("MASTER_FINYEAR ", " FINYEAR_StartDATE,FINYEAR_ENDDATE ", " FINYEAR_CODE ='" + Session["LastFinYear"].ToString() + "'");
            DateTime StartDate = Convert.ToDateTime(dtFinYear.Rows[0][0].ToString());
            DateTime EndDate = Convert.ToDateTime(dtFinYear.Rows[0][1].ToString());
            DtFromDate.Value = Convert.ToDateTime(StartDate.ToShortDateString());
            DtToDate.Value = Convert.ToDateTime(EndDate.ToShortDateString());
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
                if (idlist[0].ToString().Trim() == "Company" || idlist[0].ToString().Trim() == "SubAccount" || idlist[0].ToString().Trim() == "Bank")
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
            data = idlist[0] + "~" + str;


        }
        void FnSegment()
        {

            DataTable dtSegid = new DataTable();
            if (Session["userlastsegment"].ToString() == "9")
            {
                dtSegid = oDBEngine.GetDataTable("tbl_master_companyexchange", "Exch_internalid", "exch_tmcode='" + Session["usersegid"].ToString().Trim() + "' and exch_compid='" + Session["LastCompany"].ToString().Trim() + "' and exch_membershiptype='NSDL'");
                litSegmentMain.InnerText = "NSDL";
                HiddenField_Segment.Value = dtSegid.Rows[0][0].ToString().Trim();
            }
            else if (Session["userlastsegment"].ToString() == "10")
            {
                dtSegid = oDBEngine.GetDataTable("tbl_master_companyexchange", "Exch_internalid", "exch_tmcode='" + Session["usersegid"].ToString().Trim() + "' and exch_compid='" + Session["LastCompany"].ToString().Trim() + "'  and exch_membershiptype='CDSL'");
                litSegmentMain.InnerText = "CDSL";
                HiddenField_Segment.Value = dtSegid.Rows[0][0].ToString().Trim();
            }
            else
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


            }
        }
        DataSet SPCall(string Pageing)
        {
            string[] InputName = new string[19];
            string[] InputType = new string[19];
            string[] InputValue = new string[19];



            /////////////////Parameter Name
            InputName[0] = "AccountType";
            InputName[1] = "MainAc";
            InputName[2] = "SubAc";
            InputName[3] = "Type";
            InputName[4] = "Company";
            InputName[5] = "Segment";
            InputName[6] = "BankAc";
            InputName[7] = "InstrumentType";
            InputName[8] = "1stInstrumentNumber";
            InputName[9] = "2ndInstrumentNumber";
            InputName[10] = "ShowOnlyThirdParty";
            InputName[11] = "FromTransactionDate";
            InputName[12] = "ToTransactionDate";
            InputName[13] = "Amnt";
            InputName[14] = "Status";
            InputName[15] = "ShowUnclearedformorethanChk";
            InputName[16] = "ShowUnclearedformorethan";
            InputName[17] = "SortOrder";
            InputName[18] = "Pageing";

            /////////////////Parameter Data Type
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
            InputType[10] = "V";
            InputType[11] = "V";
            InputType[12] = "V";
            InputType[13] = "V";
            InputType[14] = "V";
            InputType[15] = "V";
            InputType[16] = "V";
            InputType[17] = "V";
            InputType[18] = "V";


            /////////////////Parameter Value
            InputValue[0] = DdlAccountType.SelectedItem.Value.ToString().Trim();
            if (RdMainAcAll.Checked)
                InputValue[1] = "ALL";
            else
                InputValue[1] = HiddenField_txtMainAccountCode.Text.ToString().Trim();

            if (RdbSubAcAll.Checked)
                InputValue[2] = "ALL";
            else
                InputValue[2] = HiddenField_SubAC.Value.ToString().Trim();

            InputValue[3] = FnType();

            if (RdbAllCompany.Checked)
                InputValue[4] = "ALL";
            else if (RdbCurrentCompany.Checked)
                InputValue[4] = "'" + Session["LastCompany"].ToString().Trim() + "'";
            else
                InputValue[4] = HiddenField_Company.Value.ToString().Trim();

            if (rdbSegmentAll.Checked)
                InputValue[5] = "ALL";
            else if (rdSegmentSelected.Checked)
                InputValue[5] = HiddenField_Segment.Value.ToString().Trim();
            else
                InputValue[5] = Session["usersegid"].ToString().Trim();

            if (rdbBankAll.Checked)
                InputValue[6] = "ALL";
            else
                InputValue[6] = HiddenField_BRSAC.Value.ToString().Trim();

            InputValue[7] = FnInstrumentType();

            if (txt1stInstrumentNumber.Text.ToString().Trim() == "")
                InputValue[8] = "NA";
            else
                InputValue[8] = txt1stInstrumentNumber.Text.ToString().Trim();

            if (txt2ndInstrumentNumber.Text.ToString().Trim() == "")
                InputValue[9] = "NA";
            else
                InputValue[9] = txt2ndInstrumentNumber.Text.ToString().Trim();

            InputValue[10] = ChkShowOnlyThirdParty.Checked.ToString().Trim();
            InputValue[11] = DtFromDate.Value.ToString().Trim();
            InputValue[12] = DtToDate.Value.ToString().Trim();
            InputValue[13] = FnAmount();
            InputValue[14] = FnStatus();
            InputValue[15] = ChkShowUnclearedformorethan.Checked.ToString().Trim();
            InputValue[16] = TxtShowUnclearedformorethan.Value.ToString().Trim();
            InputValue[17] = DdlSortOrder.SelectedItem.Value.ToString().Trim();
            InputValue[18] = Pageing.ToString().Trim();

            //////////////Sp Call
            ds = BusinessLogicLayer.SQLProcedures.SelectProcedureArrDS("[Report_PaymentReceiptQuerry]", InputName, InputType, InputValue);



            return ds;
        }

        #region ReturnString
        protected string FnType()
        {
            string PType = null;
            string RType = null;

            if (ChkTypePayments.Checked)
                PType = "P";
            else
                PType = "N";

            if (ChkTypeReceipts.Checked)
                RType = "R";
            else
                RType = "N";

            if (PType.ToString().Trim() == "N" && RType.ToString().Trim() == "R")
            {
                return "(CashBank_TransactionType='R')";
            }
            else if (PType.ToString().Trim() == "P" && RType.ToString().Trim() == "N")
            {
                return "(CashBank_TransactionType='P')";
            }
            else
            {
                return "(CashBank_TransactionType='P' or CashBank_TransactionType='R')";
            }
        }
        protected string FnInstrumentType()
        {
            string InstrumentTypeC = null;
            string InstrumentTypeE = null;
            string InstrumentTypeD = null;
            string InstrumentType = "(";

            if (ChkInstrumentTypeCheque.Checked)
            {
                InstrumentTypeC = "C";
                InstrumentType = "(CashBankDetail_InstrumentType='C' ";
            }
            else
                InstrumentTypeC = "N";

            if (ChkInstrumentTypeETrf.Checked)
            {
                InstrumentTypeE = "E";

                if (InstrumentTypeC.ToString().Trim() == "C")
                    InstrumentType = InstrumentType + " or CashBankDetail_InstrumentType='E' ";
                else
                    InstrumentType = "(CashBankDetail_InstrumentType='E'";

            }
            else
                InstrumentTypeE = "N";

            if (ChkInstrumentTypeDraft.Checked)
            {
                InstrumentTypeD = "D";

                if (InstrumentTypeC.ToString().Trim() == "C" && InstrumentTypeE.ToString().Trim() == "E" ||
                    InstrumentTypeC.ToString().Trim() == "N" && InstrumentTypeE.ToString().Trim() == "E" ||
                    InstrumentTypeC.ToString().Trim() == "C" && InstrumentTypeE.ToString().Trim() == "N"
                    )

                    InstrumentType = InstrumentType + " or CashBankDetail_InstrumentType='D')";

                else if (InstrumentTypeC.ToString().Trim() == "N" && InstrumentTypeE.ToString().Trim() == "N")
                    InstrumentType = "(CashBankDetail_InstrumentType='D')";

                else
                    InstrumentType = ")";


            }
            else
                InstrumentTypeD = "N";

            if (InstrumentTypeC.ToString().Trim() == "N" && InstrumentTypeE.ToString().Trim() == "N" && InstrumentTypeD.ToString().Trim() == "N")
                InstrumentType = "(CashBankDetail_InstrumentType='C' or CashBankDetail_InstrumentType='E or CashBankDetail_InstrumentType='D')";
            else if (InstrumentTypeC.ToString().Trim() == "C" && InstrumentTypeE.ToString().Trim() == "N" && InstrumentTypeD.ToString().Trim() == "N" ||
                     InstrumentTypeC.ToString().Trim() == "N" && InstrumentTypeE.ToString().Trim() == "E" && InstrumentTypeD.ToString().Trim() == "N")
                InstrumentType = InstrumentType + ")";



            return InstrumentType.ToString().Trim();


        }
        protected string FnAmount()
        {
            string PType = null;
            string RType = null;

            if (ChkTypePayments.Checked)
                PType = "P";
            else
                PType = "N";

            if (ChkTypeReceipts.Checked)
                RType = "R";
            else
                RType = "N";

            if (PType.ToString().Trim() == "N" && RType.ToString().Trim() == "R")
            {
                return "(abs(isnull(CashBankDetail_ReceiptAmount,0)) Between '" + TxtAmntFrom.Value.ToString().Trim() + "' and '" + TxtAmntTo.Value.ToString().Trim() + "') ";
            }
            else if (PType.ToString().Trim() == "P" && RType.ToString().Trim() == "N")
            {
                return "(abs(isnull(CashBankDetail_PaymentAmount,0)) Between '" + TxtAmntFrom.Value.ToString().Trim() + "' and '" + TxtAmntTo.Value.ToString().Trim() + "') ";
            }
            else
            {
                return "( (abs(isnull(CashBankDetail_PaymentAmount,0)) Between '" + TxtAmntFrom.Value.ToString().Trim() + "' and '" + TxtAmntTo.Value.ToString().Trim() + "') or (abs(isnull(CashBankDetail_ReceiptAmount,0)) Between '" + TxtAmntFrom.Value.ToString().Trim() + "' and '" + TxtAmntTo.Value.ToString().Trim() + "') )";
            }
        }
        protected string FnStatus()
        {
            string StatusU = null;
            string StatusC = null;

            if (ChkStatusUncleared.Checked)
                StatusU = "U";
            else
                StatusU = "N";

            if (ChkStatuscleared.Checked)
                StatusC = "C";
            else
                StatusC = "N";

            if (StatusU.ToString().Trim() == "U" && StatusC.ToString().Trim() == "N")
            {
                return "( isnull(CashBankDetail_BankValueDate,'1900-01-01 00:00:00.000')='1900-01-01 00:00:00.000' or CashBankDetail_BankValueDate>'" + DtToDate.Value.ToString().Trim() + "')";
            }
            else if (StatusU.ToString().Trim() == "N" && StatusC.ToString().Trim() == "C")
            {
                return "( CashBankDetail_BankValueDate is not null or isnull(CashBankDetail_BankValueDate,'1900-01-01 00:00:00.000')<>'1900-01-01 00:00:00.000')";
            }
            else
            {
                return "Both";
            }
        }
        #endregion
        protected void GridPaymentReceiptQuery_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string Pageing = null;
            PageNum = e.Parameters.Split('~')[1];
            string command = e.Parameters.Split('~')[0];
            PageSize = GridPaymentReceiptQuery.SettingsPager.PageSize.ToString();
            Pageing = "WHERE [Srl. No] BETWEEN ((" + PageNum + "- 1) * " + PageSize + " )+ " + 1 + " AND " + PageNum + "* " + PageSize;


            ds = SPCall(Pageing);
            if (ds.Tables[0].Rows.Count > 0)
            {
                BindGrid(ds);
                int TotalItems = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString().Trim());
                int TotalPage = TotalItems % Convert.ToInt32(PageSize) == 0 ? (TotalItems / Convert.ToInt32(PageSize)) : (TotalItems / Convert.ToInt32(PageSize)) + 1;
                if (command == "Show")
                {
                    GridPaymentReceiptQuery.JSProperties["cpIsEmptyDsSearch"] = "No~1~" + TotalPage + "~" + TotalItems;

                }
                else
                {
                    GridPaymentReceiptQuery.JSProperties["cpIsEmptyDsSearch"] = "No~" + PageNum + '~' + TotalPage + "~" + TotalItems;
                }

            }
            else
            {
                GridPaymentReceiptQuery.JSProperties["cpIsEmptyDsSearch"] = "NoRecord";

            }

        }
        void BindGrid(DataSet ds)
        {
            if (ds.Tables.Count > 0)
            {
                GridPaymentReceiptQuery.DataSource = ds;
                GridPaymentReceiptQuery.DataBind();
            }
            else
            {
                GridPaymentReceiptQuery.DataSource = null;
                GridPaymentReceiptQuery.DataBind();
            }
        }

        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            SPCall("ALL");
            Export(ds);
        }
        //void Export(DataSet ds)
        //{
        //    DataTable dtExport = ds.Tables[0].Copy();


        //    DataTable dtReportHeader = new DataTable();
        //    DataTable dtReportFooter = new DataTable();

        //    dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0

        //    DataRow HeaderRow = dtReportHeader.NewRow();
        //    string strDate =" For The Period : " + oconverter.ArrangeDate2(DtFromDate.Value.ToString()) + " - " + oconverter.ArrangeDate2(DtToDate.Value.ToString());
        //    HeaderRow[0] = strDate.ToString().Trim();
        //    dtReportHeader.Rows.Add(HeaderRow);



        //    DataRow HeaderRow1 = dtReportHeader.NewRow();
        //    dtReportHeader.Rows.Add(HeaderRow1);
        //    DataRow HeaderRow2 = dtReportHeader.NewRow();
        //    dtReportHeader.Rows.Add(HeaderRow2);

        //    dtReportFooter.Columns.Add(new DataColumn("Footer", typeof(String))); //0
        //    DataRow FooterRow1 = dtReportFooter.NewRow();
        //    dtReportFooter.Rows.Add(FooterRow1);
        //    DataRow FooterRow2 = dtReportFooter.NewRow();
        //    dtReportFooter.Rows.Add(FooterRow2);
        //    DataRow FooterRow = dtReportFooter.NewRow();
        //    FooterRow[0] = "* * *  End Of Report * * *   ";
        //    dtReportFooter.Rows.Add(FooterRow);

        //    if (cmbExport.SelectedItem.Value.ToString().Trim() == "E")
        //    {
        //        objExcel.ExportToExcelforExcel(dtExport, "Query Payment/Receipt Transactions", "Total:", dtReportHeader, dtReportFooter);
        //    }
        //    if (cmbExport.SelectedItem.Value.ToString().Trim() == "P")
        //    {
        //        objExcel.ExportToPDF(dtExport,"Query Payment/Receipt Transactions", "Total:", dtReportHeader, dtReportFooter);
        //    }

        //}


        void Export(DataSet ds)
        {
            ExcelFile objExcel = new ExcelFile();

            string searchCriteria = null;
            BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
            searchCriteria = " For The Period : " + oconverter.ArrangeDate2(DtFromDate.Value.ToString()) + " - " + oconverter.ArrangeDate2(DtToDate.Value.ToString());

            DataTable dtExport = ds.Tables[0].Copy();
            BusinessLogicLayer.GenericExcelExport oGenericExcelExport = new BusinessLogicLayer.GenericExcelExport();
            string strDownloadFileName = "";
            string exlDateTime = oDBEngine.GetDate(113).ToString();
            string exlTime = exlDateTime.Replace(":", "");
            exlTime = exlTime.Replace(" ", "");
            string FileName = "PaymentReceiptDetails_" + exlTime;
            strDownloadFileName = "~/Documents/";
            DataTable dtcompany = oDBEngine.GetDataTable("select ltrim(rtrim(cmp_Name)) as company from tbl_master_company where cmp_internalid ='" + Session["LastCompany"].ToString() + "'");
            string[] strHead = new string[3];
            strHead[0] = exlDateTime;
            strHead[1] = searchCriteria;
            strHead[2] = "PaymentRecirptDetails Of " + dtcompany.Rows[0]["company"];
            string ExcelVersion = "2007";                                                                 //Lots
            string[] ColumnType = { "I", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V", "V" };
            string[] ColumnSize = { "10", "30", "50", "50", "50", "50", "50", "50", "50", "50", "50", "50", "50", "50", "50", "50" };
            string[] ColumnWidthSize = { "10", "20", "40", "40", "40", "30", "25", "10", "15", "15", "15", "15", "25", "20", "5", "5" };
            oGenericExcelExport.ExportToExcel(ColumnType, ColumnSize, ColumnWidthSize, dtExport, Server.MapPath(strDownloadFileName), ExcelVersion, FileName, strHead, null);
        }
    }
}