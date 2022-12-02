using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BusinessLogicLayer;
public partial class SignOff : System.Web.UI.Page
{
    DBEngine oDBEngine = new DBEngine("");
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Response.Write(); 
            // gridStatusDataSource.
            //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'

            if (HttpContext.Current.Session["userid"] == null)
            {
                Session.Abandon();
                Response.Redirect("~/OMS/login.aspx");
            }
            //string sPath = HttpContext.Current.Request.Url.ToString();
            //oDBEngine.Call_CheckPageaccessebility(sPath);
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

        //if (Request.QueryString["DXCallbackErrorMessage"] != null)
        //{
        //    HttpContext.Current.Cache.Remove("LastLandingUri_" + Convert.ToString(HttpContext.Current.Session["userid"]));
        //    Session.Abandon();
        //    DestroyUserRightSession();
        //    Response.Redirect("login.aspx",true);
        //}



        int NoofRows = 0;
        HttpCookie cookie = Request.Cookies["sKey"];
        string getCookie = "";
        if (cookie != null)
        {
            getCookie = cookie.Value.ToString();
        }
        string IPNAme = System.Web.HttpContext.Current.Request.UserHostAddress;
        NoofRows = oDBEngine.SetFieldValue("tbl_master_user", "user_status='0',user_lastIP='" + IPNAme + "'", " user_loginid='" + getCookie + "'");

        HttpCookie myCookie = new HttpCookie("sKey");
        myCookie.Expires = DateTime.Now.AddDays(-1);
        Response.Cookies.Add(myCookie);
        HttpContext.Current.Cache.Remove("LastLandingUri_" + Convert.ToString(HttpContext.Current.Session["userid"]));
        Session.Abandon();
        DestroyUserRightSession();


        if (Session["Company_Code"] == null)
        {
            if (Session["Company_Code_ESS"] == null)
                Response.Redirect("login.aspx", false);
            else
               // Response.Redirect("/EmployeeSelfService/EmpLogin/Login/" + Convert.ToString(Session["Company_Code_ESS"]), false);
                Response.Redirect("login.aspx", false);
        }
        else
        {
            Response.Redirect("/Login/Login/" + Convert.ToString(Session["Company_Code"]), false);
            //Response.Redirect("login.aspx", false);

        }
    }
    public static void DestroyUserRightSession()
    {
        if (HttpContext.Current.Session["UserRightSession"] != null)
        {
            HttpContext.Current.Session["UserRightSession"] = null;
        }
    }


}
