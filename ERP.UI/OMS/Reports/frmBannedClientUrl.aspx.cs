using System;
namespace ERP.OMS.Reports
{
    public partial class Reports_frmBannedClientUrl : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["BannedEntityCircularLink"].ToString() != "")
            //{
            //    string str = Session["BannedEntityCircularLink"].ToString();
            //    Response.Redirect(str);
            //}
            string Str = Session["bannedentry"].ToString();
            Session["bannedentry"] = null;
            Response.Redirect(Str);
        }
    }
}