using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;
using UtilityLayer;



public partial class pLogin : System.Web.UI.Page
{
    
    
    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
    BusinessLogicLayer.GenericMethod oGenericMethod;

    protected void Page_Load(object sender, EventArgs e)
    {
        Session["ErpConnection"] = ConfigurationManager.ConnectionStrings["ERP_ConnectionString"].ConnectionString; 
        if (HttpContext.Current.Session["userid"] != null)
        {
            //Code added by Debjyoti 
            HttpContext.Current.Session["DeveloperRedirect"] = "Yes";
            if (HttpContext.Current.Cache["LastLandingUri_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim()] != null)
            {
                Response.Redirect(Convert.ToString(HttpContext.Current.Cache["LastLandingUri_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim()]), false);
            }
            //Code end here 
            else
            {
                Response.Redirect("ProjectMainPage.aspx", false);
            }
        }
       
        lblVersion.Text = getApplicationVersion();
        txtPassword.Attributes.Add("onkeypress", "capLock(event)");
        String[] PageFooterTags = ConfigurationManager.AppSettings["PageFooterTags"].ToString().Split('/');
        if (PageFooterTags != null)
        {
            //tdCopyRight.InnerText = PageFooterTags[0];
            //tdPowerBy.InnerText = PageFooterTags[1];
            //tdTechnialBy.InnerText = PageFooterTags[2];
        }
        else
        {
            //tdCopyRight.InnerText = "Copyright © 2009 Influx Technologies";
            //tdPowerBy.InnerText = "Powered By INFLUX";
            //tdTechnialBy.InnerText = "Technicals by : Influx Technologies";
        }

        if (Session["IPnotallowed"] != null)
        {
            Session.Remove("IPnotallowed");
            Page.ClientScript.RegisterStartupScript(GetType(), "JScript15", "<script language='javascript'>alert('You are not authorized to login from this location..');</script>");
        }

        if (!Page.IsPostBack)
        {
            //lblCompHead.Text = oDBEngine.GetFieldValue("tbl_master_company", "cmp_name", "cmp_id=45", 1)[0, 0].ToString();
            this.txtUserName.Focus();
            this.txtUserName.Text = "";
            this.txtPassword.Text = "";
            //this.lblMessage.Visible = false;

            if (Request.QueryString["rurl"] != null)
            {
                rurl.Value = Convert.ToString(Request.QueryString["rurl"]);
            }
        }
    }

    protected void Login_User(object sender, EventArgs e)
    {
        //Expired System Code
        //(this Section Will Be Improve And Dynamic.But Now Required So That Date Fixed)


        //string srt = System.AppDomain.CurrentDomain.BaseDirectory + "/License.txt";
        //Console.Write(srt);


        /* ------------ For 3 tier structure--------------------
          // oGenericMethod = new BusinessLogicLayer.GenericMethod();
        */
        oGenericMethod = new BusinessLogicLayer.GenericMethod();
      
       // string Licns_GlobalExpiry = oGenericMethod.EncryptDecript(1, "GetAnyNodeValue~//GlobalExpiry~", HttpContext.Current.Server.MapPath(@"./License.txt"));
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
                    ///Encrypt  Password
                    Encryption epasswrd = new Encryption();
                    string Encryptpass = epasswrd.Encrypt(txtPassword.Text.Trim());

                    ///////////////////

                    //validate your user here (Forms Auth or Database, for example)
                    // this could be a new "illegal" logon, so we need to check
                    // if these credentials are already in the Cache

                    /////////////////////
               
                   lblMessage.Text = "";
                    string Validuser;
                    // This Fucntion will check wehter this user is authorised or not!
                    // If authorised then page will pass to "ProjectMainPage.aspx"
                    //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);

                   

                    Validuser = oDBEngine.AuthenticateUser(this.txtUserName.Text, Encryptpass).ToString();


                  //  Validuser = oDBEngine.AuthenticateUser(this.txtUserName.Text, this.txtPassword.Text).ToString();

                    if (Validuser == "Y")
                    {


                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "JScrip", "<script language='JavaScript'>ForNextPage();</script>");
                        HttpCookie cookie = new HttpCookie("sKey");
                        cookie.Value = txtUserName.Text;
                        cookie.Expires = DateTime.Now.AddDays(1);
                        Response.Cookies.Add(cookie);


                        string returl = rurl.Value;
                        //Code added by Debjyoti 
                        if (HttpContext.Current.Cache["LastLandingUri_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim()] != null)
                        {
                            Response.Redirect(Convert.ToString(HttpContext.Current.Cache["LastLandingUri_" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim()]), true);
                        }
                        //Code end here 

                        if (string.IsNullOrWhiteSpace(returl))
                        {
                            Response.Redirect("ProjectMainPage.aspx", false);
                        }
                        else
                        {
                            Response.Redirect(returl, false);
                        }
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
           // lblExpire.InnerText = "License Corrupted!!!";
        }
    }

    protected void get_password(object sender, EventArgs e)
    {

        /*------For 3 tier structure------
          //  DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
       */

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
    
        //string validUserLoginId = oDBEngine.AuthenticateUser(this.txtForgetPass.Text, null);
        //if (validUserLoginId == "Y")
        //{
        //    //string message = SendMailToUser(this.txtForgetPass.Text);
        //    //Panelmain.Visible = true;
        //    //PanelForgetPass.Visible = false;
        //    this.txtUserName.Focus();
        //    this.txtUserName.Text = "";
        //    this.txtPassword.Text = "";
        //    //this.lblMessage.Visible = true;

        //    //if (message == "aaaa")
        //    //{
        //    //    Page.ClientScript.RegisterStartupScript(GetType(), "JScript15", "<script language='javascript'>alert('No Email ID Found For This User ID');</script>");
        //    //}
        //    //this.lblMessage.Text = "An email with password has been sent to your officiaml emailid.";
        //}
        //else
        //{
        //    //LabelFrogetMessage.Text = validUserLoginId;
        //}
    }

    protected void get_back(object sender, EventArgs e)
    {
        //Panelmain.Visible = true;
        //PanelForgetPass.Visible = false;
        //this.txtUserName.Focus();
        //this.txtUserName.Text = "";
        //this.txtPassword.Text = "";
        //this.lblMessage.Visible = false;

       
    }

    protected void LinkButton1_Click1(object sender, EventArgs e)
    {
        Response.Redirect("ForgotPassword.aspx");
        //Panelmain.Visible = false;
        //PanelForgetPass.Visible = true;
        //txtForgetPass.Text = "";
        //txtForgetPass.Focus();
        //LabelFrogetMessage.Text = "";
    }

    //public string SendMailToUser(string userId)
    //{
    //    string[,] EmailId = GetFieldValue("tbl_master_email A, tbl_master_user B", "A.eml_email, B.user_password",
    //                                    "B.user_contactId = A.eml_cntId and A.eml_type ='official " +
    //                                    "' and B.user_leavedate is null' and B.user_loginId='" + userId + "'", 2);
    //    //B.user_leavedate='true' and
    //    if (EmailId != null)
    //    {
    //        MailMessage Message = new MailMessage();
    //        Message.From = new MailAddress("influxcrm@gmail.com", "password");
    //        Message.To.Add(new MailAddress(EmailId[0, 0].ToString()));
    //        Message.CC.Add(new MailAddress("influxcrm@gmail.com"));
    //        Message.Subject = "Your Login-ID/ Password Detail";
    //        Message.IsBodyHtml = true;
    //        Random randomgenerator = new Random();
    //        int randumNumber;
    //        randumNumber = randomgenerator.Next(111111, 999999);
    //        string pass = userId + "N" + randumNumber;
    //        Message.Body = "<table border='0' + cellpadding='3' cellspacing='3' width= '100%' font-weight= 'normal' font-size= '8pt'  color= 'black' font-style= normal font-family= 'Verdana'>";
    //        Message.Body += "<tr><td colspan='2' bordercolor= 'tan' borderwidth='1px' valign='top'  background-color= 'antiquewhite' text-align: 'center' >   -: UserID/Password Information :- </td></tr>";
    //        Message.Body += "<tr><td bordercolor='tan' borderwidth='1px' valign='top'  background-color='antiquewhite' text-align= 'center'> User Id :-   </td><td bordercolor='tan' borderwidth='1px' background-color= 'antiquewhite' text-align = 'left' > " + userId + " </td></tr>";
    //        Message.Body += "<tr><td border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top; border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite; text-align: center;  >Previous Password :- </td><td bordercolor='tan' borderwidth='1px' background-color= 'antiquewhite' text-align= 'left' >" + EmailId[0, 1] + " </td></tr>";
    //        Message.Body += "<tr><td border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top; border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite; text-align: center;  >New Password :- </td><td border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top; border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite; text-align: left; >" + pass + " </td></tr>";
    //        Message.Body += "<tr><td colspan='2'  style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top; border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite; text-align: center; '>   -: CRM :- </td></tr>";
    //        Message.Body += "</table>";
    //        SetFieldValue("tbl_master_user", "user_password = '" + pass + "'", " user_loginId='" + userId + "'");
    //        SmtpClient client = new SmtpClient("smtp.gmail.com");
    //        client.Credentials = new System.Net.NetworkCredential("influxcrm@gmail.com", "influxcrm2009");
    //        //client.send(Message);
    //        client.Send(Message);

    //        //return " Get your new password in offcial mail account ";
    //        return pass;
    //    }
    //    else
    //    {
    //        return " User ID does not exist ";
    //    }
    //}

    public string SendMailToUser(string userId)
    {
        System.Net.Mail.MailMessage Message = new System.Net.Mail.MailMessage();
        string[,] EmailId = oDBEngine.GetFieldValue("tbl_master_email A, tbl_master_user B", "A.eml_email, B.user_password ,B.USER_ID,B.USER_CONTACTID",
                                    "B.user_contactId = A.eml_cntId and A.eml_type ='official' and B.user_leavedate is null and B.user_loginId='" + userId + "'", 4);
        if (EmailId[0, 0] != "n")
        {
            DataTable dtComp = oDBEngine.GetDataTable("tbl_master_company ", "cmp_internalid,(select top 1 eml_email from tbl_master_email where eml_cntId=cmp_internalid and eml_type ='official' ) as CompanyEmail ", "cmp_id =(select top 1 emp_organization from TBL_TRANS_EMPLOYEECTC where emp_cntid='" + EmailId[0, 3].ToString() + "')");

            //string rtn = "SUCCESS";
            string pass = "";

            //System.Net.Mail.MailMessage Message = new System.Net.Mail.MailMessage();

            //if (EmailId != null)
            //{
            pass = EmailId[0, 1];
            string EmlHtml = "<table width=\"100%\" style=\"font-size:10pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">";
            EmlHtml += "<tr><td   align=\"left\">   A request was made to send you your user name and password. Your details are as follows:<br /><br /></td></tr>";
            EmlHtml += "<tr><td   align=\"left\">";
            EmlHtml += "<table width=\"400px\" style=\"font-size:10pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">";
            EmlHtml += "<tr><td colspan=\"2\" style=\"background-color:#BB694D;color:White;\" align=\"center\"> UserID/Password Information</td></tr>";
            EmlHtml += "<tr><td text-align=\"left\"> User Id :-   </td><td text-align=\"right\"> " + userId + " </td></tr>";
            EmlHtml += "<tr><td text-align=\"left\">Password :- </td><td text-align=\"right\">" + pass + " </td></tr>";
            EmlHtml += "</table>";
            EmlHtml += "</td></tr>";
            EmlHtml += "<tr><td   align=\"left\"></td></tr>";
            EmlHtml += "<tr><td   align=\"left\"><br /><br /></td></tr>";
            EmlHtml += "<tr><td   align=\"left\"> <b>PLEASE NOTE:</b> Anyone can request this information, but only you will receive</td></tr>";
            EmlHtml += "<tr><td   align=\"left\">this email. This is done so that you can access your information from anywhere,</td></tr>";
            EmlHtml += "<tr><td   align=\"left\">using any computer. If you received this email but did not yourself request the</td></tr>";
            EmlHtml += "<tr><td   align=\"left\">information, then rest assured that the person making the request did not gain access</td></tr>";
            EmlHtml += "<tr><td   align=\"left\">to any of your information. </td></tr>";
            EmlHtml += "<tr><td   align=\"left\">Regards,</td></tr>";
            if (ConfigurationManager.AppSettings["DisplayName"].ToString() == "")
            {
                EmlHtml += "<tr><td   align=\"left\"></td></tr>";
            }
            else
            {
                EmlHtml += "<tr><td   align=\"left\">" + ConfigurationManager.AppSettings["DisplayName"].ToString() + "</td></tr>";
            }

            EmlHtml += "</table>";

            try
            {
                String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"];
                SqlConnection lcon = new SqlConnection(con);
                lcon.Open();
                SqlCommand lcmdEmplInsert = new SqlCommand("InsertTransEmail", lcon);
                lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                lcmdEmplInsert.Parameters.AddWithValue("@Emails_SenderEmailID", dtComp.Rows[0]["CompanyEmail"].ToString());
                lcmdEmplInsert.Parameters.AddWithValue("@Emails_Subject", "Your Login-ID/ Password Detail");
                lcmdEmplInsert.Parameters.AddWithValue("@Emails_Content", EmlHtml);
                lcmdEmplInsert.Parameters.AddWithValue("@Emails_HasAttachement", "N");
                lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateApplication", DBNull.Value);
                lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateUser", EmailId[0, 2].ToString());
                lcmdEmplInsert.Parameters.AddWithValue("@Emails_Type", "N");
                lcmdEmplInsert.Parameters.AddWithValue("@Emails_CompanyID", dtComp.Rows[0]["cmp_internalid"].ToString());
                lcmdEmplInsert.Parameters.AddWithValue("@Emails_Segment", "CRM");
                SqlParameter parameter = new SqlParameter("@result", SqlDbType.BigInt);
                parameter.Direction = ParameterDirection.Output;
                lcmdEmplInsert.Parameters.Add(parameter);
                lcmdEmplInsert.ExecuteNonQuery();
                string InternalID = parameter.Value.ToString();
                if (InternalID.ToString() != "")
                {
                    string fValues2 = "'" + InternalID + "','" + EmailId[0, 3].ToString() + "','" + EmailId[0, 0].ToString() + "','TO','" + Convert.ToDateTime(DateTime.Now) + "','" + "P" + "'";
                    oDBEngine.InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status", fValues2);
                    Page.ClientScript.RegisterStartupScript(GetType(), "asr", "<script language='javascript'>alert('An email with password has been sent to your officiaml emailid.');</script>");
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "asr", "<script language='javascript'>alert('Please try again!..');</script>");
                }
            }
            catch (Exception ex)
            {
            }

            //string DisplayName ="";
            //if (ConfigurationManager.AppSettings["DisplayName"].ToString() != "")
            //{
            //     DisplayName = ConfigurationManager.AppSettings["DisplayName"].ToString();
            //}
            //else
            //{
            //    DisplayName = ConfigurationManager.AppSettings["CredentialUserName"].ToString();
            //}
            // string From =ConfigurationManager.AppSettings["CredentialUserName"].ToString();
            // string To = EmailId[0, 0].ToString();
            // string CC = ConfigurationManager.AppSettings["CredentialUserName"].ToString();
            // string Subject = "Your Login-ID/ Password Detail";
            // string Body = EmlHtml;
            // string SmtpHost =  ConfigurationManager.AppSettings["SmtpHost"].ToString();
            // int SmtpPort =  Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"].ToString());
            // string CredentialUserName = ConfigurationManager.AppSettings["CredentialUserName"].ToString();
            // string CredentialPassword = ConfigurationManager.AppSettings["CredentialPassword"].ToString();
            // bool Ssl = Convert.ToBoolean(ConfigurationManager.AppSettings["UseSSL"].ToString());

            // Message.From = new System.Net.Mail.MailAddress(From, DisplayName);
            // Message.To.Add(new System.Net.Mail.MailAddress(To));
            // Message.SubjectEncoding = Encoding.UTF8;
            // Message.BodyEncoding = Encoding.UTF8;
            // Message.CC.Add(new System.Net.Mail.MailAddress(CC));
            // Message.Subject = Subject;
            // Message.IsBodyHtml = true;
            // Message.Body = Body;

            // System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            // client.Host = SmtpHost;
            // client.Port = SmtpPort;
            // client.EnableSsl = Ssl;
            // client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            // client.UseDefaultCredentials = false;
            // client.Credentials = new System.Net.NetworkCredential(CredentialUserName, CredentialPassword);
            // client.Send(Message);
            return pass;
            //}
            //else
            //{
            //    return " User ID does not exist ";
            //}
        }
        else
        {
            //   // Page.ClientScript.RegisterStartupScript(GetType(), "JScript18", "<script language='javascript'>Page_Load();</script>");
            return "aaaa";
        }
    }

    public string getApplicationVersion()
    {       
        string[,] getData;
       getData= oDBEngine.GetFieldValue("Master_CurrentDBVersion", "CurrentDBVersion_Number", null, 1);

       return getData[0,0];
    }
}


