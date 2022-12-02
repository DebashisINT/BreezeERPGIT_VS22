using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Reports.XtraReports
{
    
    public partial class JournalVoucherXtraReport : DevExpress.XtraReports.UI.XtraReport
    {
        public string CompanyName { get; set; }

        BusinessLogicLayer.AmountInWordBL amountInWord = new AmountInWordBL();
        int SlNo;
        double debitSummry, creditSummry;
        public JournalVoucherXtraReport()
        {
            InitializeComponent();
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            SlNo = 0;
           
        }

        private void xrCellSlNo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            SlNo++;

            XRTableCell cl = sender as XRTableCell;
            cl.Text = Convert.ToString(SlNo);
            
        }

        private void xrTableCell10_AfterPrint(object sender, EventArgs e)
        {
        //    xrLabel15.Text = amountInWord.NumberToWords(Convert.ToInt32(debitSummry)) + " Only";
            
            XRTableCell celDebit = sender as XRTableCell;
            debitSummry += Convert.ToDouble(celDebit.Text);
            xrLabel12.Text = string.Format("Dr {0:0.00}", debitSummry);
           
        }
     
        private void PageFooter_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

          
        }

        private void xrTableCell11_AfterPrint(object sender, EventArgs e)
        {
           
            XRTableCell celCrdt = sender as XRTableCell;
            creditSummry += Convert.ToDouble(celCrdt.Text);
            xrLabel14.Text = string.Format("Cr {0:0.00}", creditSummry);
        }

        private void PageFooter_AfterPrint(object sender, EventArgs e)
        {
          //  xrLabel15.Text = amountInWord.NumberToWords(Convert.ToInt32(debitSummry)) + " Only";

             //xrLabel12.Text = string.Format("Dr {0:0.00}", debitSummry);
            //xrLabel14.Text = string.Format("Cr {0:0.00}", creditSummry);

          //  debitSummry = 0;
            //creditSummry = 0;
        }

        private void xrLabel15_AfterPrint(object sender, EventArgs e)
        {
            XRLabel cl = sender as XRLabel;
           // cl.Text = amountInWord.NumberToWords(Convert.ToInt32(debitSummry)) + " Only";
        }

        private void xrLabel12_AfterPrint(object sender, EventArgs e)
        {
            XRLabel cl = sender as XRLabel;
            cl.Text = string.Format("Dr {0:0.00}", debitSummry);
          //  debitSummry = 0;
        }

        private void xrLabel14_AfterPrint(object sender, EventArgs e)
        {
            XRLabel cl = sender as XRLabel;
           // cl.Text = string.Format("Cr {0:0.00}", creditSummry);
          //  creditSummry = 0;
        }

        private void xrLabel9_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRLabel companyName = sender as XRLabel;
            if (CompanyName.Trim()!="")
            companyName.Text ="For "+ CompanyName;
            
                
        }

        private void ReportFooter_AfterPrint(object sender, EventArgs e)
        {
          
        }

        private void ReportFooter1_AfterPrint(object sender, EventArgs e)
        {
            xrLabel15.Text = amountInWord.NumberToWords(Convert.ToInt32(debitSummry)) + " Only";
        }

       
    }
}
