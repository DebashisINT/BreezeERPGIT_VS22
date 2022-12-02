using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using BusinessLogicLayer;
using UtilityLayer;
using System.Text;
using System.Net.Mail;

namespace ERP.OMS
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.GenericMethod oGenericMethod;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnReLogin_Click(object sender, EventArgs e)
        {
            string message = SendMailToUser(txtEmail.Text.Trim());
        }
        protected void lbtnBackToLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
        public string SendMailToUser(string userId)
        {
            System.Net.Mail.MailMessage Message = new System.Net.Mail.MailMessage();
            string[,] EmailId = oDBEngine.GetFieldValue("tbl_master_email A, tbl_master_user B", "A.eml_email, B.user_password ,B.USER_ID,B.USER_CONTACTID",
                                        "B.user_contactId = A.eml_cntId and A.eml_type ='official' and B.user_leavedate is null and B.user_loginId='" + userId + "'", 4);
            if (EmailId[0, 0] != "n")
            {
                //DataTable dtComp = oDBEngine.GetDataTable("tbl_master_company ", "cmp_internalid,(select top 1 eml_email from tbl_master_email where eml_cntId=cmp_internalid and eml_type ='official' ) as CompanyEmail ", "cmp_id =(select top 1 emp_organization from TBL_TRANS_EMPLOYEECTC where emp_cntid='" + EmailId[0, 3].ToString() + "')");


                string[,] strComp = oDBEngine.GetFieldValue("tbl_master_company", "cmp_internalid,(select top 1 eml_email from tbl_master_email where eml_cntId=cmp_internalid and eml_type ='official' ) as CompanyEmail ","cmp_id =(select top 1 emp_organization from TBL_TRANS_EMPLOYEECTC where emp_cntid='" + EmailId[0, 3].ToString() + "')",2);
                
                
                
                //string rtn = "SUCCESS";
                string[,] strcont = oDBEngine.GetFieldValue("config_emailAccounts", "EmailAccounts_EmailID,EmailAccounts_Password,EmailAccounts_SMTP,EmailAccounts_SMTPPort",
                                        "EmailAccounts_InUse='Y'", 4);
                string pass = "";

                //System.Net.Mail.MailMessage Message = new System.Net.Mail.MailMessage();

                //if (EmailId != null)
                //{
                pass = EmailId[0, 1];

                ///Decrypt  Password
                Encryption epasswrd = new Encryption();
                string Decryptpass = epasswrd.Decrypt(pass);




                string EmlHtml = "<table width=\"100%\" style=\"font-size:10pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">";
                EmlHtml += "<tr><td   align=\"left\">   A request was made to send you your user name and password. Your details are as follows:<br /><br /></td></tr>";
                EmlHtml += "<tr><td   align=\"left\">";
                EmlHtml += "<table width=\"400px\" style=\"font-size:10pt;\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">";
                EmlHtml += "<tr><td colspan=\"2\" style=\"background-color:#BB694D;color:White;\" align=\"center\"> UserID/Password Information</td></tr>";
                EmlHtml += "<tr><td text-align=\"left\"> User Id :-   </td><td text-align=\"right\"> " + userId + " </td></tr>";
                EmlHtml += "<tr><td text-align=\"left\">Password :- </td><td text-align=\"right\">" + Decryptpass + " </td></tr>";
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
                //if (ConfigurationManager.AppSettings["DisplayName"].ToString() == "")
                if (Convert.ToString(ConfigurationManager.AppSettings["DisplayName"]) == "" || Convert.ToString(ConfigurationManager.AppSettings["DisplayName"]) == null)
                {
                    EmlHtml += "<tr><td   align=\"left\"></td></tr>";
                }
                else
                {
                    EmlHtml += "<tr><td   align=\"left\">" + Convert.ToString(ConfigurationManager.AppSettings["DisplayName"]) + "</td></tr>";
                }

                EmlHtml += "</table>";

                try
                {
                    //String con = System.Configuration.ConfigurationSettings.AppSettings["DBConnectionDefault"]; MULTI
                    String con = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    SqlConnection lcon = new SqlConnection(con);
                    lcon.Open();
                    SqlCommand lcmdEmplInsert = new SqlCommand("InsertTransEmail", lcon);
                    lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                    //lcmdEmplInsert.Parameters.AddWithValue("@Emails_SenderEmailID", dtComp.Rows[0]["CompanyEmail"].ToString());
                    lcmdEmplInsert.Parameters.AddWithValue("@Emails_SenderEmailID", strComp[0,1].ToString());
                    lcmdEmplInsert.Parameters.AddWithValue("@Emails_Subject", "Your Login-ID/ Password Detail");
                    lcmdEmplInsert.Parameters.AddWithValue("@Emails_Content", EmlHtml);
                    lcmdEmplInsert.Parameters.AddWithValue("@Emails_HasAttachement", "N");
                    lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateApplication", DBNull.Value);
                    lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateUser", EmailId[0, 2].ToString());
                    lcmdEmplInsert.Parameters.AddWithValue("@Emails_Type", "N");
                    //lcmdEmplInsert.Parameters.AddWithValue("@Emails_CompanyID", dtComp.Rows[0]["cmp_internalid"].ToString());
                    lcmdEmplInsert.Parameters.AddWithValue("@Emails_CompanyID", strComp[0, 0].ToString());
                    lcmdEmplInsert.Parameters.AddWithValue("@Emails_Segment", "CRM");
                    SqlParameter parameter = new SqlParameter("@result", SqlDbType.BigInt);
                    parameter.Direction = ParameterDirection.Output;
                    lcmdEmplInsert.Parameters.Add(parameter);
                    lcmdEmplInsert.ExecuteNonQuery();
                    // Mantis Issue 24802
                    if (lcon.State == ConnectionState.Open)
                    {
                        lcon.Close();
                    }
                    // End of Mantis Issue 24802
                    string InternalID = parameter.Value.ToString();
                    if (InternalID.ToString() != "")
                    {
                        ////////string fValues2 = "'" + InternalID + "','" + EmailId[0, 3].ToString() + "','" + EmailId[0, 0].ToString() + "','TO','" + Convert.ToDateTime(DateTime.Now) + "','" + "P" + "'";
                        ////////oDBEngine.InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status", fValues2);








                        string DisplayName = "";
                        if (Convert.ToString(ConfigurationManager.AppSettings["DisplayName"])!= "")
                        {
                            DisplayName = Convert.ToString(ConfigurationManager.AppSettings["DisplayName"]);
                        }
                        else
                        {
                            DisplayName = Convert.ToString(ConfigurationManager.AppSettings["CredentialUserName"]);
                        }

                       

                        string From = strcont[0,0].ToString();
                        string To = EmailId[0, 0].ToString();
                        //string CC = Convert.ToString(ConfigurationManager.AppSettings["CredentialUserName"]);
                        ////string CC = strcont[0, 0].ToString();
                        string Subject = "Your Login-ID/ Password Detail";
                        string Body = EmlHtml;
                        string SmtpHost = strcont[0,2].ToString();
                        int SmtpPort = Convert.ToInt32(strcont[0,3]);
                        string CredentialUserName = strcont[0,0].ToString();;
                        string CredentialPassword = strcont[0,1].ToString();;
                        bool Ssl = true;
                        MailMessage message = new MailMessage();
                        message.From = new System.Net.Mail.MailAddress(From, DisplayName);
                        message.To.Add(new System.Net.Mail.MailAddress(To));
                        message.SubjectEncoding = Encoding.UTF8;
                        message.BodyEncoding = Encoding.UTF8;
                        ////message.CC.Add(new System.Net.Mail.MailAddress(CC));
                        message.Subject = Subject;
                        message.IsBodyHtml = true;

                        

                        //MailAddress Sender = new MailAddress("chakrabortysubha64@gmai.com");
                        //MailAddress receiver = new MailAddress(ConfigurationManager.AppSettings["LogFromEmail"]);
                        //MailAddress receiver = new MailAddress("chakrabortysubha64@gmail.com");


                        MailAddress receiver = new MailAddress(EmailId[0, 0].ToString());
                        SmtpClient smtp = new SmtpClient()
                        {
                            //Host = "smtp.gmail.com",
                            Host = strcont[0,2].ToString(),
                            //Port = 587,
                            Port = Convert.ToInt32(strcont[0,3]),
                            EnableSsl = true,
                            Credentials = new System.Net.NetworkCredential(CredentialUserName, CredentialPassword)

                        };

                        message.Body = Body;
                        smtp.Send(message);

                        //System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                        //client.Host = SmtpHost;
                        //client.Port = SmtpPort;
                        //client.EnableSsl = Ssl;
                        //client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                        //client.UseDefaultCredentials = false;
                        //client.Credentials = new System.Net.NetworkCredential(CredentialUserName, CredentialPassword);
                        //client.Send(Message);







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

                //string DisplayName = "";
                //if (ConfigurationManager.AppSettings["DisplayName"].ToString() != "")
                //{
                //    DisplayName = ConfigurationManager.AppSettings["DisplayName"].ToString();
                //}
                //else
                //{
                //    DisplayName = ConfigurationManager.AppSettings["CredentialUserName"].ToString();
                //}
                //string From = ConfigurationManager.AppSettings["CredentialUserName"].ToString();
                //string To = EmailId[0, 0].ToString();
                //string CC = ConfigurationManager.AppSettings["CredentialUserName"].ToString();
                //string Subject = "Your Login-ID/ Password Detail";
                //string Body = EmlHtml;
                //string SmtpHost = ConfigurationManager.AppSettings["SmtpHost"].ToString();
                //int SmtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"].ToString());
                //string CredentialUserName = ConfigurationManager.AppSettings["CredentialUserName"].ToString();
                //string CredentialPassword = ConfigurationManager.AppSettings["CredentialPassword"].ToString();
                //bool Ssl = Convert.ToBoolean(ConfigurationManager.AppSettings["UseSSL"].ToString());

                //Message.From = new System.Net.Mail.MailAddress(From, DisplayName);
                //Message.To.Add(new System.Net.Mail.MailAddress(To));
                //Message.SubjectEncoding = Encoding.UTF8;
                //Message.BodyEncoding = Encoding.UTF8;
                //Message.CC.Add(new System.Net.Mail.MailAddress(CC));
                //Message.Subject = Subject;
                //Message.IsBodyHtml = true;
                //Message.Body = Body;

                //System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                //client.Host = SmtpHost;
                //client.Port = SmtpPort;
                //client.EnableSsl = Ssl;
                //client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                //client.UseDefaultCredentials = false;
                //client.Credentials = new System.Net.NetworkCredential(CredentialUserName, CredentialPassword);
                //client.Send(Message);
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
    }
}