//using System;
//using System.Drawing;
//using System.Collections;
//using System.ComponentModel;
//using DevExpress.XtraReports.UI;
//using BusinessLogicLayer;
//using System.IO;
//using System.Web.UI.HtmlControls;
//using System.Web.HttpServerUtility;

using System.Drawing;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.IO;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;

using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
//using DevExpress.Web.ASPxTabControl;
using DevExpress.Web;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;


namespace ERP.OMS.Reports.XtraReports
{
    public partial class SalesInvoiceXtraReport : DevExpress.XtraReports.UI.XtraReport
    {

     
        public string CompanyName { get; set; }
        public string CompanybigLogo { get; set; }
        public string LocalCurr { get; set; }
        public int PrintCopy { get; set; }
      

        BusinessLogicLayer.AmountInWordBL amountInWord = new AmountInWordBL();
        double pagewiseTotalAmnt;
        int serial_no;
        string barcode_slno = "";
        public SalesInvoiceXtraReport()
        {
            InitializeComponent();
        }
 

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            serial_no = 0;
        }

        private void xrCompanyName_AfterPrint(object sender, EventArgs e)
        {
            XRLabel cellCompanyname = sender as XRLabel;
            if (CompanyName.Trim() != "")
                cellCompanyname.Text = CompanyName;
            if (CompanybigLogo.Trim() != "")
            {
                xrPicLogo.ImageUrl = CompanybigLogo;
            }
            else
            {
                xrPicLogo.ImageUrl = ("~/assests/logoshort.png");
            }
           

        }

  
        private void xrTableCell26_AfterPrint(object sender, EventArgs e)
        {

            //XRTableCell cellAmount = sender as XRTableCell;
            //pagewiseTotalAmnt += Convert.ToDouble(cellAmount.Text);

            //xrLabel21.Text = "An Amount of Rs. " + pagewiseTotalAmnt + "(" + amountInWord.NumberToWords(Convert.ToInt32(pagewiseTotalAmnt)) + " Only" + ") has been debited to your Account towards return of following goods as per details given bellow.";
            xrLabel27.Text = "For " + CompanyName;
            if (PrintCopy==1)
            {
                xrLabel21.Text = "Original For Recipient";
            }
            else if (PrintCopy==2)
            {
                xrLabel21.Text = "Duplicate For Transporter";
            }
            else
            {
                xrLabel21.Text = "Triplicate For Supplier";
            }
           
            
        }

        private void ReportFooter_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrTable8.Visible = true;
            xrTable9.Visible = true;
            xrLabel26.Visible = true;
            xrLabel27.Visible = true;

            //xrLabel25.Text = amountInWord.NumberToWords(Convert.ToInt32(Convert.ToDouble(xrTableCell43.Text))) + " Only";
            //xrLabel21.Text = "An Amount of Rs. " + Convert.ToInt32(Convert.ToDouble(xrTableCell43.Text)) + "(" + amountInWord.NumberToWords(Convert.ToInt32(Convert.ToDouble(xrTableCell43.Text))) + " Only" + ") has been debited to your Account towards return of following goods as per details given bellow.";
        }

        private void xrLabel25_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //xrLabel25.Text = "An Amount of Rs. " + Convert.ToInt32(Convert.ToDouble(xrTableCell43.Text)) + "(" + amountInWord.NumberToWords(Convert.ToInt32(Convert.ToDouble(xrTableCell43.Text))) + ") Only";
            xrLabel25.Text = "An Amount of " + LocalCurr.ToString().Split('~')[1].Trim() + "[" + LocalCurr.ToString().Split('~')[2].Trim() + "] " + xrTableCell43.Text + "(" + amountInWord.NumberToWords(Convert.ToInt32(Convert.ToDouble(xrTableCell43.Text))) + ") Only";
        }

        private void xrLabel21_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string amntInwords="";

            //amntInwords = "An Amount of Rs. " + Convert.ToInt32(Convert.ToDouble(xrLabel23.Text)) + "(" + amountInWord.NumberToWords(Convert.ToInt32(Convert.ToDouble(xrLabel23.Text))) + " Only" + ") has been debited to your Account towards return of following" + System.Environment.NewLine;
            //amntInwords = amntInwords + " goods as per details given bellow.";

            //xrLabel21.Text = amntInwords;

          //  xrLabel21.Text = "An Amount of Rs. " + Convert.ToInt32(Convert.ToDouble(xrLabel23.Text)) + "(" + amountInWord.NumberToWords(Convert.ToInt32(Convert.ToDouble(xrLabel23.Text))) + " Only" + ") has been debited to your Account towards return of following goods as per details given bellow.";
        }


   
        private void Detail1_AfterPrint(object sender, EventArgs e)
        {
          if( Convert.ToInt32(DetailReport.GetCurrentColumnValue("Is_Taxpresent"))!=0)
          {
              GroupHeader2.Visible = true;
              GroupFooter2.Visible = true;
          }
            else
          {
              GroupHeader2.Visible = false;
              GroupFooter2.Visible = false;
          }
        }

        private void xrSubreport1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRSubreport xrSubReport = (XRSubreport)sender;
            xrSubReport.ReportSource.Parameters["InvoiceId"].Value = Convert.ToInt32(xrLabel19.Text);
        }
    

    }
}
