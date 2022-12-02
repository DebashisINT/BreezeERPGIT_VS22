using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;
using UtilityLayer;
using System.Net.NetworkInformation;
using System.Management;

using System.Runtime.InteropServices;

namespace ERP.OMS
{
    public partial class DirectLogin : System.Web.UI.Page
    {

       // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        BusinessLogicLayer.GenericMethod oGenericMethod;
        protected void Page_Load(object sender, EventArgs e)
        {

            string userNamed = Request.QueryString["userId"];
            string passcode = Request.QueryString["passwrd"];

            Encryption epasswrd = new Encryption();
            string Encryptpass = epasswrd.Encrypt(passcode);

            string Validuser;


            Validuser = oDBEngine.AuthenticateUser(userNamed, Encryptpass).ToString();

            //string usermac = getMac();

            //lblmac.Text = usermac;


            //lblmac.Text = usermac;

            //  Validuser = oDBEngine.AuthenticateUser(this.txtUserName.Text, this.txtPassword.Text).ToString();

            if (Validuser == "Y")
            {

                // string usermac = GetMACAddress();



                //Disabled for the time being 21/08/2017
                //  string usermac = Getmaccccaddress22();



                /// bool getaccess = GetMacaddressSettings(Convert.ToString(HttpContext.Current.Session["userid"]), usermac);


                //Disabled for the time being 21/08/2017

                bool getaccess = true;


                //   getaccess = true;

                if (getaccess == true)
                {
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "JScrip", "<script language='JavaScript'>ForNextPage();</script>");
                    HttpCookie cookie = new HttpCookie("sKey");
                    cookie.Value = userNamed;
                    cookie.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(cookie);

                    HttpCookie ERPACTIVEURL = new HttpCookie("ERPACTIVEURL");
                    ERPACTIVEURL.Value = "1";
                    Response.Cookies.Add(ERPACTIVEURL);



                    string returl = "";
                    //Code added by Debjyoti 
                    if (HttpContext.Current.Cache["LastLandingUri_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim()] != null)
                    {
                        Response.Redirect(Convert.ToString(HttpContext.Current.Cache["LastLandingUri_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim()]), true);
                    }
                    //Code end here 

                    if (string.IsNullOrWhiteSpace(returl))
                    {
                        Response.Redirect("management/ProjectMainPage.aspx", false);
                    }
                    else
                    {
                        Response.Redirect(returl, false);
                    }
                }
                else
                {
                    Session.Abandon(); 
                }

            }
        }
    }
}