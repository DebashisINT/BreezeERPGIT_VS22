using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.XtraReports.Parameters;

namespace ERP.OMS.Reports.XtraReports
{
    public partial class ProductReportViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          
                ProductXtraReport newProductXtraReport = new ProductXtraReport();
                if (Session["SelectedProductList"] != null)
                {
                    var param1 = newProductXtraReport.Parameters.GetByName("prodList");
                    param1.Value = Convert.ToString(Session["SelectedProductList"]);
                    ViewState["SelectedProductList"] = Session["SelectedProductList"];
                    Session.Remove("SelectedProductList");
                }
                else
                {
                   // Response.Redirect("~/OMS/reports/master/RptProductMasterReport.aspx");
                    var param1 = newProductXtraReport.Parameters.GetByName("prodList");
                    param1.Value = Convert.ToString(ViewState["SelectedProductList"]);
                    
                }
                ASPxDocumentViewer1.Report = newProductXtraReport;
           
            
        }
    }
}