using DevExpress.Export;
using DevExpress.XtraPrinting;
using ERP.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Followup
{
    public partial class FollowupDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            List<int> branchidlist = new List<int>(Array.ConvertAll(Request.QueryString["BranchId"].Split(','), int.Parse));
            e.KeyExpression = "UniqueKey";
              //  string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            ;

                ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
                var q = from d in dc.V_followupDetails
                        where (d.ComapreDate == null ||( d.ComapreDate >= Convert.ToDateTime(Request.QueryString["FromDt"]) &&
                                  d.ComapreDate <= Convert.ToDateTime(Request.QueryString["ToDt"]))) &&
                        d.CustId == Request.QueryString["CustId"] &&
                        branchidlist.Contains(Convert.ToInt32(d.Invoice_BranchId))
                        orderby d.branch_description 
                        select d;
                e.QueryableSource = q;
             
        }

        protected void drdExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                bindexport(Filter);

            }
        }


        public void bindexport(int Filter)
        {


            exporter.PageHeader.Left = "Payment Follow-up";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";
            exporter.Landscape = true;

            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }
        }
    }
}