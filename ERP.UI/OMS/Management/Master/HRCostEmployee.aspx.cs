using System;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_HRCostEmployee : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            EmployeeSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}