using System;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_HRCostBranch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BranchSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
    }
}