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
    public partial class ViewSIPDF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string InvoiceId = "";
            if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["key"])))
                InvoiceId = Convert.ToString(Request.QueryString["key"]);
            if (!IsPostBack)
            {
                string FilePath = string.Empty;
                string fullpath = Server.MapPath("~");
                string DBName = "";
                string FileName = FilePath;
                if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["dbname"])))
                    DBName = Convert.ToString(Request.QueryString["dbname"]);
                string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority +
                                                                Request.ApplicationPath.TrimEnd('/') + "/";

                string Physical_Path = "";


                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    FilePath = baseUrl + "/Reports/Reports/RepxReportDesign/SalesInvoice/DocDesign/PDFFiles/" + "SalesInvoice-Original-" + InvoiceId + ".pdf";
                    Physical_Path = Server.MapPath("~/Reports/Reports/RepxReportDesign/SalesInvoice/DocDesign/PDFFiles/" + "SalesInvoice-Original-" + InvoiceId + ".pdf");
                }
                else
                {
                    FilePath = baseUrl + "/Reports/RepxReportDesign/SalesInvoice/DocDesign/PDFFiles/" + "SalesInvoice-Original-" + InvoiceId + ".pdf";
                    Physical_Path = Server.MapPath("~/Reports/RepxReportDesign/SalesInvoice/DocDesign/PDFFiles/" + "SalesInvoice-Original-" + InvoiceId + ".pdf");

                }
                Physical_Path = FilePath.Replace("ERP.UI\\", "");

                if (!File.Exists(Physical_Path))
                {
                    Export.ExportToPDF exportToPDF = new Export.ExportToPDF();
                    exportToPDF.ExportToPdfforEmail("SalesInvoice~D", "Invoice", Server.MapPath("~"), "1", Convert.ToString(InvoiceId), DBName);
                }
                Response.Redirect(FilePath);
            }
        }

           
    }
}