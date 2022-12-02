using System;
namespace ERP.OMS.Reports
{
    public partial class Reports_frmBannedClient : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string ID = Request.QueryString["id"].ToString();
            Session["bannedentry"] = ID;
        }
    }
}