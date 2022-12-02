using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;
using UtilityLayer;

namespace ERP.OMS
{
    public partial class VendorLogin : System.Web.UI.Page
    {

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.GenericMethod oGenericMethod;

       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] != null)
            {
                //if (HttpContext.Current.Cache["LastLandingUri_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim()] != null)
                //{
                //    Response.Redirect(Convert.ToString(HttpContext.Current.Cache["LastLandingUri_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim()]), false);
                //}
                ////Code end here 
                //else
                //{
                Response.Redirect("management/Vendor/VendorDashboard.aspx", true);
               // }
            }

            lblVersion.Text = getApplicationVersion();
            txtPassword.Attributes.Add("onkeypress", "capLock(event)");
         

            if (Session["IPnotallowed"] != null)
            {
                Session.Remove("IPnotallowed");
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript15", "<script language='javascript'>alert('You are not authorized to login from this location..');</script>");
            }

            if (!Page.IsPostBack)
            {
                this.txtUserName.Focus();
                this.txtUserName.Text = "";
                this.txtPassword.Text = "";

                if (Request.QueryString["rurl"] != null)
                {
                    rurl.Value = Convert.ToString(Request.QueryString["rurl"]);
                }
            }
        }

        protected void Login_User(object sender, EventArgs e)
        {
            oGenericMethod = new BusinessLogicLayer.GenericMethod();

            string Licns_GlobalExpiry = oGenericMethod.EncryptDecript(1, "GetAnyNodeValue~//GlobalExpiry~", System.AppDomain.CurrentDomain.BaseDirectory + "License.txt");

            if (Licns_GlobalExpiry.Length == 0)
                Licns_GlobalExpiry = "2099-12-31";

            if (!Licns_GlobalExpiry.Contains("Corrupted"))
            {
                if (oDBEngine.GetDate() >= Convert.ToDateTime(Licns_GlobalExpiry))
                {
                    //lblExpire.InnerText = "System Has Expired!!!";
                }
                else
                {
                    //// Encrypt  the Password   
                    this.lblMessage.Visible = true;
                    if ((this.txtUserName.Text == "") || (this.txtPassword.Text == ""))
                    {
                        lblMessage.Text = "User Name OR Password can not be Blank";
                        return;
                    }
                    else
                    {
                     
                        Encryption epasswrd = new Encryption();
                        string Encryptpass = epasswrd.Encrypt(txtPassword.Text.Trim());

                        lblMessage.Text = "";
                        string Validuser;
                        Validuser = oDBEngine.AuthenticateVendorUser(this.txtUserName.Text, Encryptpass).ToString();
                        if (Validuser == "Y")
                        {
                            HttpCookie cookie = new HttpCookie("sKey");
                            cookie.Value = txtUserName.Text;
                            cookie.Expires = DateTime.Now.AddDays(1);
                            Response.Cookies.Add(cookie);


                            string returl = rurl.Value;
                            //if (HttpContext.Current.Cache["LastLandingUri_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim()] != null)
                            //{
                            //    Response.Redirect(Convert.ToString(HttpContext.Current.Cache["LastLandingUri_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim()]), true);
                            //}
                            //Code end here 

                            //if (string.IsNullOrWhiteSpace(returl))
                            //{
                                Response.Redirect("management/Vendor/VendorDashboard.aspx", true);
                            //}
                            //else
                            //{
                            //    Response.Redirect(returl, false);
                            //}
                        }
                        else
                        {
                            lblMessage.Text = Validuser;
                        }
                    }
                }
            }
            else
            {

            }
        }

        protected void get_password(object sender, EventArgs e)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        }

        protected void get_back(object sender, EventArgs e)
        {



        }

        protected void LinkButton1_Click1(object sender, EventArgs e)
        {
            Response.Redirect("ForgotPassword.aspx");

        }




        public string getApplicationVersion()
        {
            string[,] getData;
            getData = oDBEngine.GetFieldValue("Master_CurrentDBVersion", "CurrentDBVersion_Number", null, 1);

            return getData[0, 0];
        }
    }
}


