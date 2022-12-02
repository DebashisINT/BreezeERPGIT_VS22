using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS
{
    public partial class AlertMessage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                LinkButton1.Text = "Back to login";
            }
            //else
            //{
            //    LinkButton1.Text = "Back to Home Page";
            //}
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            //if (HttpContext.Current.Session["userid"] == null)
            //{
            HttpContext.Current.Cache.Remove("LastLandingUri_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim());
                HttpContext.Current.Session.Abandon();
                Response.Redirect("/oms/Login.aspx");
            //}
            //else
            //{
            //    Response.Redirect("/oms/management/ProjectMainPage.aspx");
            //}
        }
    }
}