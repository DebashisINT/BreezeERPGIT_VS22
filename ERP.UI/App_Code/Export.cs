using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for Export
/// </summary>
public class Export
{
	public Export()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public void ViewChequeNumber(DataTable dtbl, string Type)
    {
        DataTable dtExport = new DataTable();
        dtExport = dtbl.Copy(); 
        DataTable dtblheader = new DataTable();
        DataTable dtblfooter = new DataTable();
        if (dtbl.Rows.Count > 0)
        {
            dtExport.Columns[0].ColumnName = "ID";
            dtExport.Columns[1].ColumnName = "Cheque Number";
            dtExport.Columns[2].ColumnName = "Account Name";
            dtExport.Columns[3].ColumnName = "Voucher Number";
            dtExport.Columns[4].ColumnName = "InstrumentDate";
            dtExport.Columns[5].ColumnName = "TransactionDate";
            dtExport.Columns[6].ColumnName = "Payment";
            dtExport.Columns[7].ColumnName = "SubAccountID";
            dtExport.Columns[8].ColumnName = "MainAccountID";

            dtExport.Columns.Remove("InstrumentDate");
            dtExport.Columns.Remove("SubAccountID");
            dtExport.Columns.Remove("MainAccountID");
            dtExport.AcceptChanges();
        
            ExcelFile ef = new ExcelFile();

            DataTable dtReportHeader = new DataTable();
            dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String))); //0
            DataRow HeaderRow = dtReportHeader.NewRow();
            HeaderRow[0] = "Cheque Entries";
            dtReportHeader.Rows.Add(HeaderRow);
            DataRow DrRowR1 = dtReportHeader.NewRow();
            DrRowR1[0] = "* * * Start Of Report * * *  ";
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
            //FooterRow[0] = "* * *  End Of Report * * *         [" + oconverter.ArrangeDate2(DateTime.Now.ToString(), "Test") + "]";
            FooterRow[0] = "* * *  End Of Report * * *   ";

            dtReportFooter.Rows.Add(FooterRow);


            ef.ExportToPDF(dtExport, "Report", "report", dtReportHeader, dtReportFooter);
        }
        
    }
}
