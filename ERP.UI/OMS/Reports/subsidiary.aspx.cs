using System;
using System.IO;
using System.Web;

namespace ERP.OMS.Reports
{
    public partial class Reports_subsidiary : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string tmpPdfPath1;
            tmpPdfPath1 = string.Empty;
            tmpPdfPath1 = HttpContext.Current.Server.MapPath(@"..\Documents\TempPdfLocation\");
            string abcd = tmpPdfPath1 + "Subsidiatry.pdf";
            FileInfo file = new FileInfo(abcd);



            // Checking if file exists
            if (file.Exists)
            {
                // Clear the content of the response
                Response.ClearContent();

                // LINE1: Add the file name and attachment, which will force the open/cance/save dialog to show, to the header
                ////Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);

                // Add the file size into the response header
                Response.AddHeader("Content-Length", file.Length.ToString());

                // Set the ContentType
                Response.ContentType = "application/pdf";

                // Write the file into the response (TransmitFile is for ASP.NET 2.0. In ASP.NET 1.1 you have to use WriteFile instead)
                Response.TransmitFile(file.FullName);

                // End the response
                //file.Delete();
                Response.End();
            }

        }
    }
}