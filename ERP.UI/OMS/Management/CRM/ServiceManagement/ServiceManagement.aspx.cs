using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.CRM.ServiceManagement
{
    public partial class ServiceManagemrnt : System.Web.UI.Page
    {

        protected void Page_init(object sender, EventArgs e)
        {
            dsCustomer.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}