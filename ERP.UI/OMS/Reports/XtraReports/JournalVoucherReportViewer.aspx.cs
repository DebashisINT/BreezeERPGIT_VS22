using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.XtraReports.Parameters;
using BusinessLogicLayer;
using System.Data;

namespace ERP.OMS.Reports.XtraReports
{
    public partial class JournalVoucherReportViewer : System.Web.UI.Page
    {
        DBEngine odbEngine = new DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {
            JournalVoucherXtraReport jv = new JournalVoucherXtraReport();
           // jv.CompanyName = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string companyInternalId = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            DataTable dt  = odbEngine.GetDataTable("select cmp_Name  from tbl_master_company  where cmp_internalid='"+companyInternalId+"'");
            if (dt.Rows.Count > 0) 
            {
                jv.CompanyName = Convert.ToString(dt.Rows[0]["cmp_Name"]);
            }


            if (Request.QueryString["id"] != null)
            { 
                var param1 = jv.Parameters.GetByName("Id");
                param1.Value = Convert.ToInt32(Request.QueryString["id"]);
            }
           

            ASPxDocumentViewer1.Report = jv; 
            
        }
    }
}