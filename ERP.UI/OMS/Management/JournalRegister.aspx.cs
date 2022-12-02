using System;
using System.Data;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Management
{
    public partial class management_JournalRegister : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        static string Branch;
        static string Segment;
        static string EntryUser;
        string data;
        static DataTable dtReport = new DataTable();
        Converter oconverter = new Converter();
        ExcelFile objExcel = new ExcelFile();
        //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        DBEngine oDBEngine = new DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetDatteFinYear();
                dtFrom.EditFormatString = oconverter.GetDateFormat("Date");
                dtTo.EditFormatString = oconverter.GetDateFormat("Date");

                Page.ClientScript.RegisterStartupScript(GetType(), "JS", "<script language='JavaScript'>Page_Load();</script>");
            }
            //_____For performing operation without refreshing page___//
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//
        }
        protected void SetDatteFinYear()
        {
            DataTable dtFinYear = oDBEngine.GetDataTable("MASTER_FINYEAR ", "FINYEAR_STARTDATE, FINYEAR_ENDDATE ", " FINYEAR_CODE ='" + Session["LastFinYear"].ToString() + "'");
            DateTime StartDate = Convert.ToDateTime(dtFinYear.Rows[0]["FINYEAR_STARTDATE"].ToString());
            DateTime EndDate = Convert.ToDateTime(dtFinYear.Rows[0]["FINYEAR_ENDDATE"].ToString());
            DateTime TodayDate = Convert.ToDateTime(oDBEngine.GetDate().ToShortDateString());
            if (EndDate < TodayDate)
            {
                dtFrom.Value = StartDate;
                dtTo.Value = EndDate;
            }
            else
            {
                dtFrom.Value = StartDate;
                dtTo.Value = TodayDate;
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
            string str = "";
            string str1 = "";
            string[] cl = idlist[1].Split(',');
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
            else if (idlist[0] == "EntryUser")
            {
                EntryUser = str;
                data = "EntryUser~" + str1;
            }
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            String strHtmlAllClient = String.Empty;
            string GenerationType = null;
            string BranchID = null;
            string SegmentID = null;
            string EntryUserID = null;
            string BranchID1 = null;
            string SegmentID1 = null;
            string EntryUserID1 = null;
            string VoucherDate = null;
            string segmentname = null;
            string VoucherNumber = null;
            string prefix = null;
            decimal AmountDR = 0;
            decimal AmountCr = 0;
            decimal TotalAmountDR = 0;
            decimal TotalAmountCr = 0;
            int count = 0;

            if (chkIgnoreSystem.Checked == true)
                GenerationType = " JournalVoucher_Prefix not in(select VoucherType_Code from Master_VoucherType) ";
            else
                GenerationType = " JournalVoucher_GenerationType in('A','M')";
            if (radSpecific.Checked == true)
                prefix = "and Journalvoucher_Prefix ='" + txtAccountCode.Text.ToString().Trim().ToUpper() + "'";
            else
                prefix = "";

            if (RadSegmentAll.Checked == true)
            {
                //SegmentID = null;
                //SegmentID1 = null;
                SegmentID1 = " and JournalVoucher_ExchangeSegmentID in(select A.EXCH_INTERNALID AS SEGMENTID from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + Session["LastCompany"].ToString() + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID )";
                SegmentID = " and JournalVoucherDetail_ExchangeSegmentID in(select A.EXCH_INTERNALID AS SEGMENTID from (SELECT TMCE.* FROM TBL_MASTER_COMPANYEXCHANGE AS TMCE WHERE  TMCE.EXCH_COMPID='" + Session["LastCompany"].ToString() + "' ) AS A LEFT OUTER JOIN   TBL_MASTER_EXCHANGE AS TME ON TME.EXH_CNTID=A.EXCH_EXCHID )";

            }
            else
            {
                if (Segment == null)
                {
                    SegmentID1 = " and JournalVoucher_ExchangeSegmentID in(" + Session["usersegid"].ToString() + ")";
                    SegmentID = " and JournalVoucherDetail_ExchangeSegmentID in(" + Session["usersegid"].ToString() + ")";
                }
                else
                {
                    SegmentID1 = " and JournalVoucher_ExchangeSegmentID in(" + Segment + ")";
                    SegmentID = " and JournalVoucherDetail_ExchangeSegmentID in(" + Segment + ")";
                }
            }
            if (RadBranchAll.Checked == true)
            {
                //BranchID = null;
                //BranchID1 = null;
                BranchID1 = " and JournalVoucher_BranchID in(select  branch_id from tbl_master_branch where branch_id in(" + Session["userbranchHierarchy"].ToString() + "))";
                BranchID = " and JournalVoucherDetail_BranchID in(select  branch_id from tbl_master_branch where branch_id in(" + Session["userbranchHierarchy"].ToString() + "))";
            }
            else
            {
                BranchID1 = " and JournalVoucher_BranchID in(" + Branch + ")";
                BranchID = " and JournalVoucherDetail_BranchID in(" + Branch + ")";
            }
            if (RadEntryAll.Checked == true)
            {
                EntryUserID = null;
                EntryUserID1 = null;
            }
            else
            {
                EntryUserID = " and JournalVoucherDetail_CreateUser in(" + EntryUser + ")";
                EntryUserID1 = " and JournalVoucher_CreateUser in(" + EntryUser + ")";
            }
            DataTable DtShow = oDBEngine.GetDataTable("Trans_JournalVoucherDetail", "convert(varchar(11),JournalVoucherDetail_TransactionDate,113) as Date,JournalVoucherDetail_VoucherNumber,(select top 1 MainAccount_Name from Master_MainAccount where MainAccount_AccountCode=Trans_JournalVoucherDetail.JournalVoucherDetail_MainAccountCode)+isnull((case when JournalVoucherDetail_SubAccountCode is null then '' else (' [' + (isnull((select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'')+' ['+isnull(ltrim(rtrim(cnt_ucc)),'')+']' from tbl_master_contact where cnt_internalid=Trans_JournalVoucherDetail.JournalVoucherDetail_SubAccountCode ),isnull((select top 1 subaccount_name+' ['+isnull(ltrim(rtrim(subaccount_code)),'')+']' from master_subaccount where cast(subaccount_code as varchar)=Trans_JournalVoucherDetail.JournalVoucherDetail_SubAccountCode and SubAccount_MainAcReferenceID=Trans_JournalVoucherDetail.JournalVoucherDetail_MainAccountCode),isnull((select subaccount_name+' ['+isnull(ltrim(rtrim(subaccount_code)),'')+']' from master_subaccount where cast(subaccount_referenceid as varchar)=Trans_JournalVoucherDetail.JournalVoucherDetail_SubAccountCode and SubAccount_MainAcReferenceID=Trans_JournalVoucherDetail.JournalVoucherDetail_MainAccountCode),isnull((select top 1 cdslclients_firstholdername+'['+isnull(ltrim(rtrim(cdslclients_benaccountnumber)),'')+']' from master_cdslclients where cast(cdslclients_benaccountnumber as varchar)=Trans_JournalVoucherDetail.JournalVoucherDetail_SubAccountCode),(select nsdlclients_benfirstholdername+' ['+isnull(ltrim(rtrim(nsdlclients_benaccountid)),'')+']' from master_nsdlclients where cast(nsdlclients_benaccountid as varchar)=Trans_JournalVoucherDetail.JournalVoucherDetail_SubAccountCode))))))+']') end),'') as AccountName,isnull((select top 1 JournalVoucher_Narration from Trans_JournalVoucher where JournalVoucher_VoucherNumber=Trans_JournalVoucherDetail.JournalVoucherDetail_VoucherNumber and JournalVoucher_TransactionDate=cast(DATEADD(dd, 0, DATEDIFF(dd, 0,Trans_JournalVoucherDetail.JournalVoucherDetail_TransactionDate)) as datetime) and JournalVoucher_ExchangeSegmentID=JournalVoucherDetail_ExchangeSegmentID),'')+' |'+isnull(JournalVoucherDetail_Narration,'') as Description,case when JournalVoucherDetail_AmountDr=0 then null else JournalVoucherDetail_AmountDr end as AmountDR,case when JournalVoucherDetail_AmountCR=0 then null else JournalVoucherDetail_AmountCR end as AmountCR,(select isnull(((select exh_shortName from tbl_master_Exchange where exh_cntId=exch_exchId)+'-'+exch_segmentId),exch_membershipType) from tbl_master_CompanyExchange where exch_internalID=JournalVoucherDetail_ExchangeSegmentID) as SegmentName", " JournalVoucherDetail_VoucherNumber in(select JournalVoucher_VoucherNumber from Trans_JournalVoucher where " + GenerationType + " " + SegmentID1 + " " + EntryUserID1 + " " + BranchID1 + " " + prefix + " and JournalVoucher_TransactionDate between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)) " + SegmentID + " " + BranchID + " " + EntryUserID + " and JournalVoucherDetail_TransactionDate between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtFrom.Value + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + dtTo.Value + "')) as datetime)", " JournalVoucherDetail_TransactionDate,JournalVoucherDetail_VoucherNumber,JournalVoucherDetail_ExchangeSegmentID");
            if (DtShow.Rows.Count > 0)
            {
                dtReport.Rows.Clear();
                dtReport.Dispose();
                dtReport = new DataTable();

                dtReport.Columns.Add(new DataColumn("Account Name", typeof(String)));
                dtReport.Columns.Add(new DataColumn("Description", typeof(String)));
                dtReport.Columns.Add(new DataColumn("Amount Dr.", typeof(String)));
                dtReport.Columns.Add(new DataColumn("Amount Cr.", typeof(String)));

                strHtmlAllClient += "<table border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtmlAllClient += "<tr style=\"background-color: #DBEEF3;\">";
                strHtmlAllClient += "<td align=\"center\">Account Name</td>";
                strHtmlAllClient += "<td align=\"center\">Description</td>";
                strHtmlAllClient += "<td align=\"center\">Amount Dr.</td>";
                strHtmlAllClient += "<td align=\"center\">Amount Cr.</td></tr>";
                for (int i = 0; i < DtShow.Rows.Count; i++)
                {

                    DataRow drW = dtReport.NewRow();
                    if (i == 0)
                    {
                        DataRow dr2 = dtReport.NewRow();
                        VoucherDate = DtShow.Rows[i]["Date"].ToString();
                        VoucherNumber = DtShow.Rows[i]["JournalVoucherDetail_VoucherNumber"].ToString();
                        segmentname = DtShow.Rows[i]["SegmentName"].ToString();
                        strHtmlAllClient += "<tr style=\" background-color:#fff0f5;text-align:center\">";
                        strHtmlAllClient += "<td align=\"left\" colspan=\"4\" style=\"font-size:small;background-color:#804000; color:#ffffff\">Voucher Date: " + DtShow.Rows[i]["Date"].ToString() + "  | Voucher Number: " + DtShow.Rows[i]["JournalVoucherDetail_VoucherNumber"].ToString() + " | Segment: " + DtShow.Rows[i]["SegmentName"].ToString() + "</td></tr>";
                        dr2[0] = "Voucher Date: " + DtShow.Rows[i]["Date"].ToString() + "  | Voucher Number: " + DtShow.Rows[i]["JournalVoucherDetail_VoucherNumber"].ToString() + " | Segment: " + DtShow.Rows[i]["SegmentName"].ToString();
                        dr2[1] = "Test";
                        dtReport.Rows.Add(dr2);
                    }
                    if (VoucherDate != DtShow.Rows[i]["Date"].ToString() || VoucherNumber != DtShow.Rows[i]["JournalVoucherDetail_VoucherNumber"].ToString() || segmentname != DtShow.Rows[i]["SegmentName"].ToString())
                    {
                        // strHtmlAllClient += "<table border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 2 + " cellspacing=" + 2 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                        DataRow dr = dtReport.NewRow();
                        DataRow dr2 = dtReport.NewRow();
                        //DataRow drBla = dtReport.NewRow();
                        //dtReport.Rows.Add(drBla);
                        strHtmlAllClient += "<tr style=\"background-color:lavender;\">";
                        strHtmlAllClient += "<td align=\"right\" colspan=\"2\" style=\"font-size:xx-small\"><b>Total</b></td>";
                        strHtmlAllClient += "<td align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(AmountDR) + "</td>";
                        strHtmlAllClient += "<td align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(AmountCr) + "</td>";
                        strHtmlAllClient += "</tr>";

                        dr[0] = "";
                        dr[1] = "Total";
                        dr[2] = oconverter.getFormattedvalue(AmountDR);
                        dr[3] = oconverter.getFormattedvalue(AmountCr);
                        dtReport.Rows.Add(dr);

                        VoucherDate = DtShow.Rows[i]["Date"].ToString();
                        VoucherNumber = DtShow.Rows[i]["JournalVoucherDetail_VoucherNumber"].ToString();
                        segmentname = DtShow.Rows[i]["SegmentName"].ToString();
                        strHtmlAllClient += "<tr style=\" background-color:#fff0f5;text-align:center\">";
                        strHtmlAllClient += "<td align=\"left\" colspan=\"4\" style=\"font-size:small;background-color:#804000; color:#ffffff\">Voucher Date: " + DtShow.Rows[i]["Date"].ToString() + "  | Voucher Number: " + DtShow.Rows[i]["JournalVoucherDetail_VoucherNumber"].ToString() + " | Segment: " + DtShow.Rows[i]["SegmentName"].ToString() + "</td></tr>";

                        dr2[0] = "Voucher Date: " + DtShow.Rows[i]["Date"].ToString() + "  | Voucher Number: " + DtShow.Rows[i]["JournalVoucherDetail_VoucherNumber"].ToString() + " | Segment: " + DtShow.Rows[i]["SegmentName"].ToString();
                        dr2[1] = "Test";
                        dtReport.Rows.Add(dr2);
                        AmountDR = 0;
                        AmountCr = 0;
                        count = 0;
                    }
                    if (count % 2 == 0)
                    {
                        strHtmlAllClient += "<tr style=\" background-color:#E5E0EC;text-align:center\">";
                    }
                    else
                        strHtmlAllClient += "<tr style=\" background-color:#fff0f5;text-align:center\">";
                    strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + DtShow.Rows[i]["AccountName"].ToString() + "</td>";
                    strHtmlAllClient += "<td align=\"left\" style=\"font-size:xx-small\">" + DtShow.Rows[i]["Description"].ToString() + "</td>";

                    drW[0] = DtShow.Rows[i]["AccountName"].ToString();
                    drW[1] = DtShow.Rows[i]["Description"].ToString();

                    if (DtShow.Rows[i]["AmountDR"] != DBNull.Value)
                    {
                        strHtmlAllClient += "<td align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(DtShow.Rows[i]["AmountDR"].ToString())) + "</td>";
                        AmountDR += Convert.ToDecimal(DtShow.Rows[i]["AmountDR"].ToString());
                        TotalAmountDR += Convert.ToDecimal(DtShow.Rows[i]["AmountDR"].ToString());
                        drW[2] = oconverter.getFormattedvalue(Convert.ToDecimal(DtShow.Rows[i]["AmountDR"].ToString()));
                    }
                    else
                    {
                        strHtmlAllClient += "<td align=\"right\" style=\"font-size:xx-small\">&nbsp;</td>";
                        drW[2] = "";
                    }
                    if (DtShow.Rows[i]["AmountCR"] != DBNull.Value)
                    {
                        strHtmlAllClient += "<td align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(DtShow.Rows[i]["AmountCR"].ToString())) + "</td>";
                        AmountCr += Convert.ToDecimal(DtShow.Rows[i]["AmountCR"].ToString());
                        TotalAmountCr += Convert.ToDecimal(DtShow.Rows[i]["AmountCR"].ToString());
                        drW[3] = oconverter.getFormattedvalue(Convert.ToDecimal(DtShow.Rows[i]["AmountCR"].ToString()));
                    }
                    else
                    {
                        strHtmlAllClient += "<td align=\"right\" style=\"font-size:xx-small\">&nbsp;</td>";
                        drW[3] = "";
                    }
                    dtReport.Rows.Add(drW);
                    strHtmlAllClient += "</tr>";
                    if (i == DtShow.Rows.Count - 1)
                    {
                        strHtmlAllClient += "<tr style=\"background-color:lavender;\">";
                        strHtmlAllClient += "<td align=\"right\" colspan=\"2\" style=\"font-size:xx-small\"><b>Total</b></td>";
                        strHtmlAllClient += "<td align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(AmountDR) + "</td>";
                        strHtmlAllClient += "<td align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(AmountCr) + "</td>";
                        strHtmlAllClient += "</tr>";
                        //DataRow drBla = dtReport.NewRow();
                        //dtReport.Rows.Add(drBla);
                        DataRow dr = dtReport.NewRow();
                        dr[0] = "";
                        dr[1] = "Total";
                        dr[2] = oconverter.getFormattedvalue(AmountDR);
                        dr[3] = oconverter.getFormattedvalue(AmountCr);
                        dtReport.Rows.Add(dr);
                    }
                    count += 1;
                }
                strHtmlAllClient += "<tr style=\"background-color: #DBEEF3;\">";
                strHtmlAllClient += "<td align=\"right\" colspan=\"2\"  style=\"font-size:xx-small\"><b>Grand Total</b></td>";
                strHtmlAllClient += "<td align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(TotalAmountDR) + "</td>";
                strHtmlAllClient += "<td align=\"right\" style=\"font-size:xx-small\">" + oconverter.getFormattedvalueWithounDecimalPlaceOriginalSign(TotalAmountCr) + "</td>";
                strHtmlAllClient += "</tr>";
                strHtmlAllClient += "</table>";
                DataRow drBla1 = dtReport.NewRow();
                dtReport.Rows.Add(drBla1);
                DataRow dr3 = dtReport.NewRow();
                dr3[0] = "";
                dr3[1] = "Grand Total";
                dr3[2] = oconverter.getFormattedvalue(TotalAmountDR);
                dr3[3] = oconverter.getFormattedvalue(TotalAmountCr);
                dtReport.Rows.Add(dr3);
                //ViewState["dtReport"] = dtReport;
            }
            if (DtShow.Rows.Count > 0)
            {
                DivShow.InnerHtml = strHtmlAllClient;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct2", "HeightCall();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct2", "alert('No Record Found !!');", true);
            }
        }
        protected void ddlExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable CompanyName = oDBEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + Session["LastCompany"].ToString() + "'");
            DataTable dtReportHeader = new DataTable();
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = CompanyName.Rows[0][0].ToString();
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            DrRowR1[0] = " Journal Register ";
            dtReportHeader.Rows.Add(DrRowR1);
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
                objExcel.ExportToExcelforExcel(dtReport, "Journal Register", "Total", dtReportHeader, dtReportFooter);

            }
            else if (ddlExport.SelectedItem.Value == "P")
            {
                objExcel.ExportToPDF(dtReport, "Journal Register", "Total", dtReportHeader, dtReportFooter);
            }
        }
    }
}