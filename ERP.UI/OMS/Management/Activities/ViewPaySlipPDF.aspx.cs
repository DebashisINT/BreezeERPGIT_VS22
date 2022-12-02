using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities
{
    public partial class ViewPaySlipPDF : System.Web.UI.Page
    {
        protected void Page_Load(object  sender, EventArgs e)
        {
            string EmployeeCode = "";
            if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["key"])))
                EmployeeCode = Convert.ToString(Request.QueryString["key"]);
            if (!IsPostBack)
            {
                string FilePath = string.Empty;
                string fullpath = Server.MapPath("~");
                string DBName = "";
                string FileName = FilePath;
                if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["dbname"])))
                    DBName = Convert.ToString(Request.QueryString["dbname"]);

                string YYMM = "";
                if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["Period"])))
                    YYMM = Convert.ToString(Request.QueryString["Period"]);

                string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority +
                                                                Request.ApplicationPath.TrimEnd('/') + "/";

                string Physical_Path = "";

                string YY = YYMM.Substring(0, 2);
                string MM = YYMM.Substring(2, 2);

               
                String mapPath = Server.MapPath("~/");
                String PDFFileName = EmployeeCode + ".pdf";


                //if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                //{
                //   FilePath = baseUrl + "/Reports/Reports/RepxReportDesign/SalesInvoice/DocDesign/PDFFiles/" + "SalesInvoice-Original-" + InvoiceId + ".pdf";
                //    Physical_Path = Server.MapPath("~/Reports/Reports/RepxReportDesign/SalesInvoice/DocDesign/PDFFiles/" + "SalesInvoice-Original-" + InvoiceId + ".pdf");
                //}
                //else
                //{
                //    FilePath = baseUrl + "/Reports/RepxReportDesign/SalesInvoice/DocDesign/PDFFiles/" + "SalesInvoice-Original-" + InvoiceId + ".pdf";
                //    Physical_Path = Server.MapPath("~/Reports/RepxReportDesign/SalesInvoice/DocDesign/PDFFiles/" + "SalesInvoice-Original-" + InvoiceId + ".pdf");

                //}

                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    Physical_Path = Server.MapPath("~/CommonFolder/Payslip/" + YY + "/" + MM + "/" + PDFFileName);
                }
                else
                {
                    Physical_Path = Server.MapPath("~/CommonFolder/Payslip/" + YY + "/" + MM + "/" + PDFFileName);
                }

                Physical_Path = FilePath.Replace("ERP.UI\\", "");

                //if (!File.Exists(Physical_Path))
                if (File.Exists(Physical_Path))
                {
                    //Export.ExportToPDF exportToPDF = new Export.ExportToPDF();
                    //exportToPDF.ExportToPdfforEmail("SalesInvoice~D", "Invoice", Server.MapPath("~"), "1", Convert.ToString(InvoiceId), DBName);
                    Response.Redirect(FilePath);
                }
               // Response.Redirect(FilePath);
            }
        }

           
    }
}