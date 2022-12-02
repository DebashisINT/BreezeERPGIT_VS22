using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ErrorPages_ErrorPage : System.Web.UI.Page
{
    public string ErrorMessage = "";
    public string ErrorDescription = "";
    protected void Page_Load(object sender, EventArgs e)
    {


        ErrorMessage = "You do not have permission to perform this operation";
        ErrorDescription = "Please Contact the  Administrator.";

        hdnGoAhead.NavigateUrl = "~/OMS/Logn.aspx";
       
    }

    protected void returnToHomePage(object sender, EventArgs e)
    {
        string url = HttpContext.Current.Request.Url.ToString().ToLower();
        if (url.Contains("/admin/"))
        {
            Response.Redirect("../Admin/login.aspx");
        }
        else
        {
            Response.Redirect("../index.aspx");
        }
    }
}