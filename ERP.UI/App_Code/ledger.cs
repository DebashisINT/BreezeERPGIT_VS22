using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Mail;
using System.IO;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.Runtime.Serialization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using BusinessLogicLayer;
/// <summary>
/// Summary description for ledger
/// </summary>
public class ledger
{
    static string SubLedgerType = "";
    static string Branch;
    static string Segment;
    static string MainAcID;
    static string SubAcID;
    string data;
    decimal openingBal;
    decimal debitTotal = 0;
    decimal creditTotal = 0;
    string SegmentID = null;
    static string MainAcIDforOp;
    static DataTable dtCashBankBook = new DataTable();
    static DataTable dtLedgerView = new DataTable();
    //DBEngine oDbEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
    DBEngine oDbEngine = new DBEngine();
    Converter oconverter = new Converter();
    //ExcelFile objExcel = new ExcelFile();
    static string Check;

	public ledger()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public void ExportToPDF(DataTable datatable, string fname, string compareString, DataTable dtHeader, DataTable dtFooter)
    {
        Document pdfReport = new Document(PageSize.A4, 25, 25, 40, 25);
        System.IO.MemoryStream msReport = new System.IO.MemoryStream();
        PdfWriter writer = PdfWriter.GetInstance(pdfReport, msReport);
        pdfReport.Open();
        PdfPTable ptHeader = new PdfPTable(dtHeader.Columns.Count);
        for (int intH = 1; intH <= dtHeader.Rows.Count; intH++)
        {
            PdfPCell cell = new PdfPCell();
            cell.BorderWidth = 0.00f;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Colspan = dtHeader.Columns.Count;
            cell.BackgroundColor = CMYKColor.WHITE;
            cell.Phrase = new Phrase(dtHeader.Rows[intH - 1][0].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 8, Font.BOLD, CMYKColor.BLUE));
            ptHeader.AddCell(cell);
        }
        pdfReport.Add(ptHeader);
        //pdfReport.Close();




        PdfPTable ptData = new PdfPTable(datatable.Columns.Count);

        float[] headerwidths = new float[datatable.Columns.Count]; // percentage
        for (int intK = 0; intK < datatable.Columns.Count; intK++)
        {
            if (datatable.Columns[intK].ColumnName.ToString().ToUpper().Contains("DESC"))
                headerwidths[intK] = (100 / (datatable.Columns.Count + 2)) * 3;
            else
                headerwidths[intK] = 100 / (datatable.Columns.Count + 2);
        }
        ptData.SetWidths(headerwidths);
        ptData.WidthPercentage = 100;
        ptData.DefaultCell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
        ptData.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;

        //Insert the Table Headers
        for (int intK = 0; intK < datatable.Columns.Count; intK++)
        {
            PdfPCell cell = new PdfPCell();
            cell.BorderWidth = 0.001f;
            cell.Phrase = new Phrase(datatable.Columns[intK].ColumnName.ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 7, Font.BOLD, CMYKColor.BLUE));
            ptData.AddCell(cell);
        }

        ptData.HeaderRows = 1;  // this is the end of the table header

        //Insert the Table Data
        int gridvalue = 0;
        int evenOdd = 0;
        for (int intJ = 1; intJ <= datatable.Rows.Count; intJ++)
        {

            if (gridvalue != intJ)
            {
                if (datatable.Rows[intJ - 1][0] == DBNull.Value && datatable.Rows[intJ - 1][1] == DBNull.Value)
                {
                    evenOdd += 1;
                    gridvalue = intJ + 1;
                    if (evenOdd % 2 != 0)
                    {
                        for (int intK = 0; intK < datatable.Columns.Count; intK++)
                        {
                            bool val = IsNumeric(datatable.Rows[intJ][intK].ToString());
                            if (val == false)
                            {
                                PdfPCell cell = new PdfPCell();
                                cell.BorderWidth = 0.001f;
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Phrase = new Phrase(datatable.Rows[intJ][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.NORMAL, CMYKColor.BLUE));
                                ptData.AddCell(cell);
                            }
                            else
                            {
                                PdfPCell cell = new PdfPCell();
                                cell.BorderWidth = 0.001f;
                                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                if (Convert.ToDecimal(datatable.Rows[intJ][intK].ToString()) < 0)
                                    cell.Phrase = new Phrase(datatable.Rows[intJ][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.NORMAL, CMYKColor.RED));
                                else
                                    cell.Phrase = new Phrase(datatable.Rows[intJ][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.NORMAL, CMYKColor.BLUE));

                                ptData.AddCell(cell);
                            }

                        }
                    }
                    else
                    {
                        for (int intK = 0; intK < datatable.Columns.Count; intK++)
                        {
                            bool val = IsNumeric(datatable.Rows[intJ][intK].ToString());
                            if (val == false)
                            {
                                PdfPCell cell = new PdfPCell();
                                cell.BorderWidth = 0.001f;
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.Phrase = new Phrase(datatable.Rows[intJ][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.NORMAL, CMYKColor.BLUE));
                                ptData.AddCell(cell);
                            }
                            else
                            {
                                PdfPCell cell = new PdfPCell();
                                cell.BorderWidth = 0.001f;
                                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                if (Convert.ToDecimal(datatable.Rows[intJ][intK].ToString()) < 0)
                                    cell.Phrase = new Phrase(datatable.Rows[intJ][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.NORMAL, CMYKColor.RED));
                                else
                                    cell.Phrase = new Phrase(datatable.Rows[intJ][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.NORMAL, CMYKColor.BLUE));
                                ptData.AddCell(cell);
                            }

                        }
                    }
                }
                else if (datatable.Rows[intJ - 1][0].ToString() == compareString || datatable.Rows[intJ - 1][1].ToString() == compareString || datatable.Rows[intJ - 1][2].ToString() == compareString || datatable.Rows[intJ - 1][3].ToString() == compareString)
                {
                    for (int intK = 0; intK < datatable.Columns.Count; intK++)
                    {
                        bool val = IsNumeric(datatable.Rows[intJ - 1][intK].ToString());
                        if (val == false)
                        {
                            PdfPCell cell = new PdfPCell();
                            cell.BorderWidth = 0.001f;
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Phrase = new Phrase(datatable.Rows[intJ - 1][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.NORMAL, CMYKColor.BLUE));
                            ptData.AddCell(cell);
                        }
                        else
                        {
                            PdfPCell cell = new PdfPCell();
                            cell.BorderWidth = 0.001f;
                            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            if (Convert.ToDecimal(datatable.Rows[intJ - 1][intK].ToString()) < 0)
                                cell.Phrase = new Phrase(datatable.Rows[intJ - 1][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.NORMAL, CMYKColor.RED));
                            else
                                cell.Phrase = new Phrase(datatable.Rows[intJ - 1][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.NORMAL, CMYKColor.BLUE));

                            ptData.AddCell(cell);
                        }

                    }
                }
                else if (datatable.Rows[intJ - 1][0] != DBNull.Value && datatable.Rows[intJ - 1][1] == "Test" && datatable.Rows[intJ - 1][2] == DBNull.Value && datatable.Rows[intJ - 1][3] == DBNull.Value)
                {
                    PdfPCell cell = new PdfPCell();
                    cell.BorderWidth = 0.001f;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = datatable.Columns.Count;
                    cell.BackgroundColor = CMYKColor.WHITE;
                    cell.Phrase = new Phrase(datatable.Rows[intJ - 1][0].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.BOLD, CMYKColor.BLUE));
                    ptData.AddCell(cell);
                }
                else
                {
                    for (int intK = 0; intK < datatable.Columns.Count; intK++)
                    {
                        bool val = IsNumeric(datatable.Rows[intJ - 1][intK].ToString());
                        if (val == false)
                        {
                            PdfPCell cell = new PdfPCell();
                            cell.BorderWidth = 0.001f;
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.Phrase = new Phrase(datatable.Rows[intJ - 1][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6));
                            ptData.AddCell(cell);
                        }
                        else
                        {
                            PdfPCell cell = new PdfPCell();
                            cell.BorderWidth = 0.001f;
                            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            if (Convert.ToDecimal(datatable.Rows[intJ - 1][intK].ToString()) < 0)
                                cell.Phrase = new Phrase(datatable.Rows[intJ - 1][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6, Font.NORMAL, CMYKColor.RED));
                            else
                                cell.Phrase = new Phrase(datatable.Rows[intJ - 1][intK].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 6));
                            ptData.AddCell(cell);
                        }

                    }
                }
            }

        }

        //Insert the Table

        pdfReport.Add(ptData);

        //Closes the Report and writes to Memory Stream
        //PdfPTable ptFooter = new PdfPTable(dtFooter.Columns.Count);
        //for (int intF = 1; intF <= dtFooter.Rows.Count; intF++)
        //{
        //    if (dtFooter.Rows[intF - 1][0] == DBNull.Value)
        //    {
        //        PdfPCell cell = new PdfPCell();
        //        cell.BorderWidth = 0.00f;
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell.Colspan = dtFooter.Columns.Count;
        //        cell.BackgroundColor = CMYKColor.WHITE;
        //        cell.Phrase = new Phrase(dtFooter.Rows[intF - 1][0].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 8, Font.BOLD, CMYKColor.BLUE));
        //        ptFooter.AddCell(cell);
        //    }
        //    else
        //    {
        //        PdfPCell cell = new PdfPCell();
        //        cell.BorderWidth = 0.00f;
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell.Colspan = dtFooter.Columns.Count;
        //        cell.BackgroundColor = CMYKColor.WHITE;
        //        cell.Phrase = new Phrase(dtFooter.Rows[intF - 1][0].ToString(), FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 8, Font.BOLD, CMYKColor.BLUE));
        //        ptFooter.AddCell(cell);
        //    }

        //}
        //pdfReport.Add(ptFooter);
        pdfReport.Close();

        //Writes the Memory Stream Data to Response Object
        string saveTo=HttpContext.Current.Server.MapPath("../Documents") +"\\"+ fname + ".pdf";
        HttpContext.Current.Response.Clear();
        //HttpContext.Current.Response.AppendHeader("content-disposition", "attachment;filename=" + fname + ".pdf");
        //HttpContext.Current.Response.Charset = "";
        //HttpContext.Current.Response.ContentType = "application/pdf";
        //HttpContext.Current.Response.BinaryWrite(msReport.ToArray());
        FileStream writeStream = new FileStream(saveTo, FileMode.Create, FileAccess.Write);
        ReadWriteStream(msReport.ToArray(), writeStream);
        HttpContext.Current.Session["mailpath"] = HttpContext.Current.Session["mailpath"].ToString() + HttpContext.Current.Server.MapPath("../Documents") + "\\" + fname + ".pdf" + "~";
        HttpContext.Current.Response.Clear();
    }
    private void ReadWriteStream(Byte[] readStream, Stream writeStream)
    {       
        int bytesRead = readStream.Length;
        // write the required bytes      
        writeStream.Write(readStream, 0, bytesRead);   
        writeStream.Close();
    }
    public bool IsNumeric(string Value)
    {
        try
        {
            Convert.ToDecimal(Value);
            return true;
        }
        catch
        {
            return false;
        }
    }


    public void Exportledger()
    {

        DataTable CompanyName = oDbEngine.GetDataTable("tbl_master_company", "cmp_Name", " cmp_internalId='" + HttpContext.Current.Session["LastCompany"].ToString() + "'");
        System.Globalization.NumberFormatInfo currencyFormat = new System.Globalization.CultureInfo("en-us").NumberFormat;
        currencyFormat.CurrencySymbol = "";
        currencyFormat.CurrencyNegativePattern = 2;
        decimal receipt = 0;
        decimal Payment = 0;
        decimal closingRate = 0;
        string CheckingValueParam = null;
        DataTable OpenBalance = new DataTable();
        DataTable dtCashBankBook1 = new DataTable();
        DataTable dtLedger = new DataTable();
        Segment = HttpContext.Current.Session["SegmentID"].ToString();
        //DataTable dtSegment = oDbEngine.GetDataTable("tbl_master_companyExchange", "exch_internalId", "exch_compId='" + HttpContext.Current.Session["CompanyID"].ToString() + "'");
        //if (dtSegment.Rows.Count > 0)
        //{
        //    for (int i = 0; i < dtSegment.Rows.Count; i++)
        //    {
        //        if (Segment == null)
        //            Segment = dtSegment.Rows[i][0].ToString();
        //        else
        //            Segment += "," + dtSegment.Rows[i][0].ToString();
        //    }
        //}


        Branch = HttpContext.Current.Session["userbranchHierarchy"].ToString();

        if (Segment == null)
        {
            Segment = HttpContext.Current.Session["SegmentID"].ToString();
        }
        string mainAccountSearch = null;
        string SubAccountSearch = null;

        mainAccountSearch = "and accountsledger_MainAccountID in('SYSTM00001')";
        MainAcIDforOp = "'SYSTM00001'";


        for (int l = 0; l < 1; l++)
        {
            receipt = 0;
            Payment = 0;
            string valItem = HttpContext.Current.Session["userid"].ToString();

            SubAccountSearch = " and AccountsLedger_SubAccountID in('" + valItem + "') ";
            dtCashBankBook1 = oDbEngine.GetDataTable("Trans_accountsledger a ", "convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,convert(varchar(11),a.accountsledger_valuedate,113) as ValueDate,a.accountsledger_TransactionReferenceID, a.accountsledger_Narration,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,case when a.Accountsledger_AmountDr='0.00000000' then null else cast(a.Accountsledger_AmountDr as varchar(max)) end as Accountsledger_AmountCr,case when a.Accountsledger_AmountCr='0.00000000' then null else cast(a.Accountsledger_AmountCr as varchar(max)) end as Accountsledger_AmountDr,'0.0' as Closing", " accountsledger_companyID='" + HttpContext.Current.Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + HttpContext.Current.Session["fromdate"] + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + HttpContext.Current.Session["todate"] + "')) as datetime)  " + mainAccountSearch + " and AccountsLedger_BranchID in(" + Branch + ") " + SubAccountSearch + "  and AccountsLedger_SubAccountID is not null and AccountsLedger_TransactionType<>'OpeningBalance'", " accountsledger_transactiondate,accountsledger_TransactionReferenceID");
            OpenBalance = oDbEngine.OpeningBalanceOnlyJournal(MainAcIDforOp, valItem, Convert.ToDateTime(HttpContext.Current.Session["fromdate"].ToString()), Segment, HttpContext.Current.Session["CompanyID"].ToString(), Convert.ToDateTime(HttpContext.Current.Session["todate"].ToString()));

            DataTable dtCashBankBook_New = dtCashBankBook1.Copy();
            dtCashBankBook_New.Rows.Clear();
            DataRow newRow = dtCashBankBook_New.NewRow();
            
            for (int j = 0; j < dtCashBankBook1.Rows.Count; j++)
            
            {
                newRow = dtCashBankBook_New.NewRow();
                newRow[0] = dtCashBankBook1.Rows[j]["TrDate"];
                newRow[1] = dtCashBankBook1.Rows[j]["ValueDate"];
                newRow[2] = dtCashBankBook1.Rows[j]["accountsledger_TransactionReferenceID"];
                newRow[3] = dtCashBankBook1.Rows[j]["accountsledger_Narration"];
                newRow[4] = dtCashBankBook1.Rows[j]["accountsledger_InstrumentNumber"];
                newRow[5] = dtCashBankBook1.Rows[j]["SettlementNumber"];
                newRow[6] = dtCashBankBook1.Rows[j]["Accountsledger_AmountCr"];
                newRow[7] = dtCashBankBook1.Rows[j]["Accountsledger_AmountDr"];
                string Dr = "0";
                string Cr = "0";
                if (dtCashBankBook1.Rows[j]["Accountsledger_AmountDr"].ToString() != "")
                {
                    Cr = dtCashBankBook1.Rows[j]["Accountsledger_AmountDr"].ToString();
                }
                if (dtCashBankBook1.Rows[j]["Accountsledger_AmountCr"].ToString() != "")
                    Dr = dtCashBankBook1.Rows[j]["Accountsledger_AmountCr"].ToString();
                if (j == 0)
                {

                    newRow[8] = decimal.Parse(Cr) - decimal.Parse(Dr) + (Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()));
                    closingRate = decimal.Parse(Cr) - decimal.Parse(Dr) + (Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()));

                }
                else
                {
                    newRow[8] = decimal.Parse(Cr) - decimal.Parse(Dr) + closingRate;
                    closingRate = decimal.Parse(Cr) - decimal.Parse(Dr) + closingRate;
                }
                dtCashBankBook_New.Rows.Add(newRow);
                if (dtCashBankBook1.Rows[j]["Accountsledger_AmountDr"].ToString() != "")
                    receipt += Convert.ToDecimal(dtCashBankBook1.Rows[j]["Accountsledger_AmountDr"].ToString());
                if (dtCashBankBook1.Rows[j]["Accountsledger_AmountCr"].ToString() != "")
                    Payment += Convert.ToDecimal(dtCashBankBook1.Rows[j]["Accountsledger_AmountCr"].ToString());
            }
            if (dtCashBankBook1.Rows.Count != 0)
            {
                dtCashBankBook1.Rows.Clear();
                dtCashBankBook1 = dtCashBankBook_New.Copy();
                DataTable dtCashBankBook_New1 = dtCashBankBook1.Copy();
                dtCashBankBook_New1.Rows.Clear();
                string Type = "";
                DataRow newRow5 = dtCashBankBook_New1.NewRow();

                Type = "Clients - Trading A/c  ";

                CheckingValueParam = Type + ": " + oDbEngine.GetDataTable("tbl_master_contact", "(rtrim(ltrim(cnt_firstname)) +' ['+rtrim(ltrim(cnt_ucc))+']') as username", "cnt_internalid='" + HttpContext.Current.Session["userid"].ToString() + "'").Rows[0][0].ToString().Trim() + " " + "Period :" + " " + oconverter.ArrangeDate2(HttpContext.Current.Session["fromdate"].ToString()) + "  - " + oconverter.ArrangeDate2(HttpContext.Current.Session["todate"].ToString());
                newRow5[0] = Type + "Period :" + " " + oconverter.ArrangeDate2(HttpContext.Current.Session["fromdate"].ToString()) + "  - " + oconverter.ArrangeDate2(HttpContext.Current.Session["todate"].ToString());
                newRow5[1] = "Test";
                dtCashBankBook_New1.Rows.Add(newRow5);
                DataRow newRow1 = dtCashBankBook_New1.NewRow();
                newRow1[1] = oconverter.ArrangeDate2(Convert.ToDateTime(HttpContext.Current.Session["fromdate"]).ToShortDateString());
                newRow1[3] = "Opening Balance";
                if (Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()) < 0)
                {
                    newRow1[6] = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()) * (-1);
                    Payment += Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()) * (-1);
                    newRow1[8] = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString());
                }
                else
                {
                    newRow1[7] = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString());
                    receipt += Convert.ToDecimal(OpenBalance.Rows[0][1].ToString());
                    newRow1[8] = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString());
                }
                dtCashBankBook_New1.Rows.Add(newRow1);
                for (int i = 0; i < dtCashBankBook1.Rows.Count; i++)
                {
                    newRow1 = dtCashBankBook_New1.NewRow();
                    newRow1[0] = dtCashBankBook1.Rows[i]["TrDate"];
                    newRow1[1] = dtCashBankBook1.Rows[i]["ValueDate"];
                    newRow1[2] = dtCashBankBook1.Rows[i]["accountsledger_TransactionReferenceID"];
                    newRow1[3] = dtCashBankBook1.Rows[i]["accountsledger_Narration"];
                    newRow1[4] = dtCashBankBook1.Rows[i]["accountsledger_InstrumentNumber"];
                    newRow1[5] = dtCashBankBook1.Rows[i]["SettlementNumber"];
                    newRow1[6] = dtCashBankBook1.Rows[i]["Accountsledger_AmountCr"];
                    newRow1[7] = dtCashBankBook1.Rows[i]["Accountsledger_AmountDr"];
                    newRow1[8] = dtCashBankBook1.Rows[i]["Closing"];
                    dtCashBankBook_New1.Rows.Add(newRow1);
                    openingBal = decimal.Parse(dtCashBankBook1.Rows[i]["Closing"].ToString());
                }
                dtCashBankBook1.Rows.Clear();
                dtCashBankBook1 = dtCashBankBook_New1.Copy();
                DataRow DrRow1 = dtCashBankBook1.NewRow();
                dtCashBankBook1.Rows.Add(DrRow1);
                DataRow DrRow = dtCashBankBook1.NewRow();
                DrRow[1] = oconverter.ArrangeDate2(Convert.ToDateTime(HttpContext.Current.Session["todate"]).ToShortDateString());
                DrRow[3] = "Closing Balance";
                if (dtCashBankBook1.Rows.Count == 0)
                {
                    openingBal = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString());
                }
                DrRow[8] = openingBal.ToString("c", currencyFormat);
                if (receipt != 0)
                    DrRow[7] = receipt.ToString("c", currencyFormat);

                if (Payment != 0)
                    DrRow[6] = Payment.ToString("c", currencyFormat);

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
                        {
                            dtCashBankBook1.Rows[k]["Closing"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtCashBankBook1.Rows[k]["Closing"].ToString()));

                        }
                    }
                }
                if (HttpContext.Current.Session["userid"] != null)
                {
                    dtLedger = dtCashBankBook1.Copy();
                }

            }
            else
            {
                dtCashBankBook1.Rows.Clear();
                dtCashBankBook1 = dtCashBankBook_New.Copy();
                DataTable dtCashBankBook_New1 = dtCashBankBook1.Copy();
                dtCashBankBook_New1.Rows.Clear();
                string Type = "";
                DataRow newRow5 = dtCashBankBook_New1.NewRow();

                Type = "Clients - Trading A/c  ";

                CheckingValueParam = Type + ": " + oDbEngine.GetDataTable("tbl_master_contact", "(rtrim(ltrim(cnt_firstname)) +' ['+rtrim(ltrim(cnt_ucc))+']') as username", "cnt_internalid='" + HttpContext.Current.Session["userid"].ToString() + "'").Rows[0][0].ToString().Trim() + " " + "Period :" + " " + oconverter.ArrangeDate2(HttpContext.Current.Session["fromdate"].ToString()) + "  - " + oconverter.ArrangeDate2(HttpContext.Current.Session["todate"].ToString());
                newRow5[0] = Type + "Period :" + " " + oconverter.ArrangeDate2(HttpContext.Current.Session["fromdate"].ToString()) + "  - " + oconverter.ArrangeDate2(HttpContext.Current.Session["todate"].ToString());
                newRow5[1] = "Test";
                dtCashBankBook_New1.Rows.Add(newRow5);
                DataRow newRow1 = dtCashBankBook_New1.NewRow();
                newRow1[1] = oconverter.ArrangeDate2(Convert.ToDateTime(HttpContext.Current.Session["fromdate"]).ToShortDateString());
                newRow1[3] = "Opening Balance";
                if (Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()) < 0)
                {
                    newRow1[6] = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()) * (-1);
                    Payment += Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()) * (-1);
                    newRow1[8] = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString());
                }
                else
                {
                    newRow1[7] = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString());
                    receipt += Convert.ToDecimal(OpenBalance.Rows[0][1].ToString());
                    newRow1[8] = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString());
                }
                dtCashBankBook_New1.Rows.Add(newRow1);
                for (int i = 0; i < dtCashBankBook1.Rows.Count; i++)
                {
                    newRow1 = dtCashBankBook_New1.NewRow();
                    newRow1[0] = dtCashBankBook1.Rows[i]["TrDate"];
                    newRow1[1] = dtCashBankBook1.Rows[i]["ValueDate"];
                    newRow1[2] = dtCashBankBook1.Rows[i]["accountsledger_TransactionReferenceID"];
                    newRow1[3] = dtCashBankBook1.Rows[i]["accountsledger_Narration"];
                    newRow1[4] = dtCashBankBook1.Rows[i]["accountsledger_InstrumentNumber"];
                    newRow1[5] = dtCashBankBook1.Rows[i]["SettlementNumber"];
                    newRow1[6] = dtCashBankBook1.Rows[i]["Accountsledger_AmountCr"];
                    newRow1[7] = dtCashBankBook1.Rows[i]["Accountsledger_AmountDr"];
                    newRow1[8] = dtCashBankBook1.Rows[i]["Closing"];
                    dtCashBankBook_New1.Rows.Add(newRow1);
                    openingBal = decimal.Parse(dtCashBankBook1.Rows[i]["Closing"].ToString());
                }
                dtCashBankBook1.Rows.Clear();
                dtCashBankBook1 = dtCashBankBook_New1.Copy();
                DataRow DrRow1 = dtCashBankBook1.NewRow();
                dtCashBankBook1.Rows.Add(DrRow1);
                DataRow DrRow = dtCashBankBook1.NewRow();
                DrRow[1] = oconverter.ArrangeDate2(Convert.ToDateTime(HttpContext.Current.Session["todate"]).ToShortDateString());
                DrRow[3] = "Closing Balance";
                if (dtCashBankBook1.Rows.Count == 0)
                {
                    openingBal = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString());
                }
                DrRow[8] = openingBal.ToString("c", currencyFormat);
                if (receipt != 0)
                    DrRow[7] = receipt.ToString("c", currencyFormat);

                if (Payment != 0)
                    DrRow[6] = Payment.ToString("c", currencyFormat);

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
                        {
                            dtCashBankBook1.Rows[k]["Closing"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtCashBankBook1.Rows[k]["Closing"].ToString()));

                        }
                    }
                }
                if (HttpContext.Current.Session["userid"] != null)
                {
                    dtLedger = dtCashBankBook1.Copy();
                }
            }

               




            }
            # region isnull
            if (HttpContext.Current.Session["userid"] == null)
        {

            receipt = 0;
            Payment = 0;
            SubAccountSearch = null;
            //dtCashBankBook1 = oDbEngine.GetDataTable("Trans_accountsledger a ", "convert(varchar(11),a.accountsledger_transactiondate,113) as TrDate,convert(varchar(11),a.accountsledger_valuedate,113) as ValueDate,a.accountsledger_TransactionReferenceID, a.accountsledger_Narration,a.accountsledger_InstrumentNumber,(a.accountsledger_SettlementNumber+' '+a.accountsledger_SettlementType) as SettlementNumber,case when a.Accountsledger_AmountDr='0.00000000' then null else cast(a.Accountsledger_AmountDr as varchar(max)) end as Accountsledger_AmountCr,case when a.Accountsledger_AmountCr='0.00000000' then null else cast(a.Accountsledger_AmountCr as varchar(max)) end as Accountsledger_AmountDr,'0.0' as Closing", " accountsledger_companyID='" + HttpContext.Current.Session["CompanyID"].ToString() + "' and accountsledger_ExchangeSegmentID in(" + Segment + ") and  cast(DATEADD(dd, 0, DATEDIFF(dd, 0,accountsledger_transactiondate)) as datetime) between cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + HttpContext.Current.Session["fromdate"].ToString() + "')) as datetime) and cast(DATEADD(dd, 0, DATEDIFF(dd, 0,'" + HttpContext.Current.Session["todate"].ToString() + "')) as datetime)  " + mainAccountSearch + " and AccountsLedger_BranchID in(" + Branch + ") " + SubAccountSearch + "   and AccountsLedger_TransactionType<>'OpeningBalance'", " accountsledger_transactiondate,accountsledger_TransactionReferenceID");
            OpenBalance = oDbEngine.OpeningBalanceOnlyJournal(MainAcIDforOp, null, Convert.ToDateTime(HttpContext.Current.Session["fromdate"].ToString()), Segment, HttpContext.Current.Session["CompanyID"].ToString(), Convert.ToDateTime(HttpContext.Current.Session["todate"].ToString()));
            DataTable dtCashBankBook_New = dtCashBankBook1.Copy();
            dtCashBankBook_New.Rows.Clear();
            DataRow newRow = dtCashBankBook_New.NewRow();
            for (int j = 0; j < dtCashBankBook1.Rows.Count; j++)
            {
                newRow = dtCashBankBook_New.NewRow();
                newRow[0] = dtCashBankBook1.Rows[j]["TrDate"];
                newRow[1] = dtCashBankBook1.Rows[j]["ValueDate"];
                newRow[2] = dtCashBankBook1.Rows[j]["accountsledger_TransactionReferenceID"];
                newRow[3] = dtCashBankBook1.Rows[j]["accountsledger_Narration"];
                newRow[4] = dtCashBankBook1.Rows[j]["accountsledger_InstrumentNumber"];
                newRow[5] = dtCashBankBook1.Rows[j]["SettlementNumber"];
                newRow[6] = dtCashBankBook1.Rows[j]["Accountsledger_AmountCr"];
                newRow[7] = dtCashBankBook1.Rows[j]["Accountsledger_AmountDr"];
                string Dr = "0";
                string Cr = "0";
                if (dtCashBankBook1.Rows[j]["Accountsledger_AmountDr"].ToString() != "")
                {
                    Cr = dtCashBankBook1.Rows[j]["Accountsledger_AmountDr"].ToString();
                }
                if (dtCashBankBook1.Rows[j]["Accountsledger_AmountCr"].ToString() != "")
                    Dr = dtCashBankBook1.Rows[j]["Accountsledger_AmountCr"].ToString();
                if (j == 0)
                {
                    newRow[8] = decimal.Parse(Cr) - decimal.Parse(Dr) + (Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()));
                    closingRate = decimal.Parse(Cr) - decimal.Parse(Dr) + (Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()));

                }
                else
                {
                    newRow[8] = decimal.Parse(Cr) - decimal.Parse(Dr) + closingRate;
                    closingRate = decimal.Parse(Cr) - decimal.Parse(Dr) + closingRate;
                }
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
            string Type = "";
            DataRow newRow5 = dtCashBankBook_New1.NewRow();

            Type = "Clients - Trading A/c  ";

            CheckingValueParam = Type + ": Period :" + " " + oconverter.ArrangeDate2(HttpContext.Current.Session["fromdate"].ToString()) + "  - " + oconverter.ArrangeDate2(HttpContext.Current.Session["todate"].ToString());
            newRow5[0] = Type + ": Period :" + " " + oconverter.ArrangeDate2(HttpContext.Current.Session["fromdate"].ToString()) + "  - " + oconverter.ArrangeDate2(HttpContext.Current.Session["todate"].ToString());
            newRow5[1] = "Test";
            dtCashBankBook_New1.Rows.Add(newRow5);
            DataRow newRow1 = dtCashBankBook_New1.NewRow();
            newRow1[1] = oconverter.ArrangeDate2(Convert.ToDateTime(HttpContext.Current.Session["fromdate"]).ToShortDateString());
            newRow1[3] = "Opening Balance";
            if (Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()) < 0)
            {
                newRow1[6] = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()) * (-1);
                Payment += Convert.ToDecimal(OpenBalance.Rows[0][1].ToString()) * (-1);
                newRow1[8] = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString());
            }
            else
            {
                newRow1[7] = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString());
                receipt += Convert.ToDecimal(OpenBalance.Rows[0][1].ToString());
                newRow1[8] = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString());
            }
            dtCashBankBook_New1.Rows.Add(newRow1);
            for (int i = 0; i < dtCashBankBook1.Rows.Count; i++)
            {
                newRow1 = dtCashBankBook_New1.NewRow();
                newRow1[0] = dtCashBankBook1.Rows[i]["TrDate"];
                newRow1[1] = dtCashBankBook1.Rows[i]["ValueDate"];
                newRow1[2] = dtCashBankBook1.Rows[i]["accountsledger_TransactionReferenceID"];
                newRow1[3] = dtCashBankBook1.Rows[i]["accountsledger_Narration"];
                newRow1[4] = dtCashBankBook1.Rows[i]["accountsledger_InstrumentNumber"];
                newRow1[5] = dtCashBankBook1.Rows[i]["SettlementNumber"];
                newRow1[6] = dtCashBankBook1.Rows[i]["Accountsledger_AmountCr"];
                newRow1[7] = dtCashBankBook1.Rows[i]["Accountsledger_AmountDr"];
                newRow1[8] = dtCashBankBook1.Rows[i]["Closing"];
                dtCashBankBook_New1.Rows.Add(newRow1);
                openingBal = decimal.Parse(dtCashBankBook1.Rows[i]["Closing"].ToString());
            }
            dtCashBankBook1.Rows.Clear();
            dtCashBankBook1 = dtCashBankBook_New1.Copy();
            DataRow DrRow1 = dtCashBankBook1.NewRow();
            dtCashBankBook1.Rows.Add(DrRow1);
            DataRow DrRow = dtCashBankBook1.NewRow();
            DrRow[3] = "Closing Balance";
            if (dtCashBankBook1.Rows.Count == 0)
            {
                openingBal = Convert.ToDecimal(OpenBalance.Rows[0][1].ToString());
            }
            DrRow[8] = openingBal.ToString("c", currencyFormat);
            if (receipt != 0)
                DrRow[7] = receipt.ToString("c", currencyFormat);

            if (Payment != 0)
                DrRow[6] = Payment.ToString("c", currencyFormat);

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
                    {
                        dtCashBankBook1.Rows[k]["Closing"] = oconverter.formatmoneyinUs(Convert.ToDecimal(dtCashBankBook1.Rows[k]["Closing"].ToString()));

                    }
                }
            }
            dtLedger = dtCashBankBook1.Copy();

        }
            # endregion
        DataTable dtReportHeader = new DataTable();
        dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
        DataRow HeaderRow = dtReportHeader.NewRow();
        HeaderRow[0] = CompanyName.Rows[0][0].ToString() +" [" +HttpContext.Current.Session["segmentname"].ToString()+"]";
        dtReportHeader.Rows.Add(HeaderRow);
        DataRow DrRowR1 = dtReportHeader.NewRow();
        DrRowR1[0] = "Ledger For the  Period [" + oconverter.ArrangeDate2(HttpContext.Current.Session["fromdate"].ToString()) + " To " + oconverter.ArrangeDate2(HttpContext.Current.Session["todate"].ToString()) + "]" + " Segment :" + HttpContext.Current.Session["segmentname"].ToString();
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
        DataRow FooterRow3 = dtReportFooter.NewRow();
        dtReportFooter.Rows.Add(FooterRow3);
        DataRow FooterRow = dtReportFooter.NewRow();
        //FooterRow[0] = "* * *  End Of Report * * *         [" + oconverter.ArrangeDate2(DateTime.Now.ToString(), "Test") + "]";
        FooterRow[0] = "* * *  End Of Report * * *   ";
        dtReportFooter.Rows.Add(FooterRow);

        DataTable dtExport = new DataTable();
        dtExport = dtLedger.Copy();
        dtExport.Columns[2].ColumnName = "Voucher No.";
        dtExport.Columns[3].ColumnName = "Description";
        dtExport.Columns[4].ColumnName = "Instrument No.";
        dtExport.Columns[5].ColumnName = "Settlement No.";
        dtExport.Columns[6].ColumnName = "Debit";
        dtExport.Columns[7].ColumnName = "Credit";
        dtExport.AcceptChanges();

        ExportToPDF(dtExport, oDbEngine.GetDataTable("tbl_master_contact", "(rtrim(ltrim(cnt_firstname)) +' ['+rtrim(ltrim(cnt_ucc))+']') as username", "cnt_internalid='" + HttpContext.Current.Session["userid"].ToString() + "'").Rows[0][0].ToString().Trim() + "_" + HttpContext.Current.Session["segmentname"].ToString() +"_"+ oconverter.ArrangeDate2(HttpContext.Current.Session["fromdate"].ToString()) + "  - " + oconverter.ArrangeDate2(HttpContext.Current.Session["todate"].ToString()), "Closing Balance", dtReportHeader, dtReportFooter);

    }
    
    
}
