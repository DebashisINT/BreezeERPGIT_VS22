using System;
namespace ERP.OMS.Management
{
    public partial class management_fetch_status_email_report : System.Web.UI.Page
    {
        GetAutomaticMail mail = new GetAutomaticMail();
        protected void Page_Load(object sender, EventArgs e)
        {
            // string i=  mail.fetch_status("aa.bb@yahoo.com", "PNDG CA");
        }
    }
}
