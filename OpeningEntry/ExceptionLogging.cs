using System;
using System.Collections.Generic;

using context = System.Web.HttpContext; 
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Text;
using System.Diagnostics;
using System.Net.Mail;
using System.Web.Hosting;
using System.IO;
using System.Collections.Specialized;
using BusinessLogicLayer;
namespace OpeningEntry
{

    //Done by :Subhabrata
    public class ExceptionLogging
    {
        private static String exepurl;
        static SqlConnection con;

        //Done By:Subhabrata
        #region ConnectionOpen/Close
        private static void connecttion()
        {
            string constr = ConfigurationManager.ConnectionStrings["crmConnectionString"].ToString();
            con = new SqlConnection(constr);
            con.Open();
        }
        #endregion

        //Done By:Subhabrata
        #region SendExceptionToDB
        public static void SendExcepToDB(Exception exdb)
        {

            connecttion();
            exepurl = context.Current.Request.Url.ToString();
            SqlCommand com = new SqlCommand("ExceptionLoggingToDataBase", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@ExceptionMsg", Convert.ToString(exdb.Message));
            com.Parameters.AddWithValue("@ExceptionType", Convert.ToString(exdb.GetType().Name));
            com.Parameters.AddWithValue("@ExceptionURL", exepurl);
            com.Parameters.AddWithValue("@ExceptionSource", Convert.ToString(exdb.StackTrace));
            com.ExecuteNonQuery();



        }

        #endregion

        //Done By:Subhabrata
        #region MethodForException
        //public static void SendExceptionMail(Exception e)
        //{
        //    string strEmails=ConfigurationManager.AppSettings["LogToEmail"].ToString(); //ConfigurationSettings.AppSettings["LogToEmail"].ToString();
        //    if (strEmails.Length > 0)
        //    {
        //        string[] arEmails = strEmails.Split(Convert.ToChar("|"));
        //        MailMessage strMessage = new MailMessage();
        //        strMessage.BodyFormat = MailFormat.Html;
        //        strMessage.To = arEmails[0];
        //        for (int i = 1; i < arEmails.Length; i++)
        //            strMessage.Cc = arEmails[i];
        //        string strFromEmail = ConfigurationManager.AppSettings["LogFromEmail"].ToString();
        //        strMessage.From = strFromEmail;
        //        strMessage.Subject = "OMS: Exception dated " + DateTime.Now + " !";
        //        string sExceptionDescription = FormatExceptionDescription(e);
        //        strMessage.Body = sExceptionDescription;
        //        SmtpMail.SmtpServer = System.Configuration.ConfigurationSettings.AppSettings["smtpServer"].ToString();
        //        try
        //        {
        //            SmtpMail.Send(strMessage);
        //        }
        //        catch (Exception excm)
        //        {
        //            Debug.WriteLine(excm.Message);
        //            throw;
        //        }
        //    }
        //    else
        //    {
        //        return;
        //    }
        //}
        #endregion

        //Done By:Subhabrata 
        #region SendMailFunctionality
        public static int SendEmailToAssigneeByUser(string AssignedTo, string AssignedBy, 
                          ListDictionary replacements, DataTable dtEmailConfig, string activityname,int trSenderType)
        {
            int m = 0;
            try
            {
                
                string Email_AssignedTo = AssignedTo;
                string Email_AssignedBy = AssignedBy;
                //if (string.IsNullOrEmpty(Email_AssignedBy) && string.IsNullOrEmpty(Email_AssignedTo))
                //{

                MailMessage message = new MailMessage();
                string[] arEmails = AssignedTo.Split(Convert.ToChar("|"));
                for (int i = 1; i < arEmails.Length; i++)
                {
                    message.CC.Add(arEmails[i]);
                }

                MailAddress Sender = new MailAddress(dtEmailConfig.Rows[0].Field<string>("EmailAccounts_EmailID"));
                //MailAddress receiver = new MailAddress(ConfigurationManager.AppSettings["LogFromEmail"]);
                //MailAddress receiver = new MailAddress("chakrabortysubha64@gmail.com");
              
                
                MailAddress receiver = new MailAddress(AssignedTo);
                SmtpClient smtp = new SmtpClient()
                {
                    //Host = "smtp.gmail.com",
                    Host = Convert.ToString(dtEmailConfig.Rows[0].Field<string>("EmailAccounts_SMTP")),
                    //Port = 587,
                    Port = Convert.ToInt32(dtEmailConfig.Rows[0].Field<string>("EmailAccounts_SMTPPort")),
                    EnableSsl = true,
                    Credentials = new System.Net.NetworkCredential(Convert.ToString(dtEmailConfig.Rows[0].Field<string>("EmailAccounts_EmailID")), Convert.ToString(dtEmailConfig.Rows[0].Field<string>("EmailAccounts_Password")))

                };

                #region Sales Activity Subjecct
                //if (trSenderType == 4)//Sales Assignment
                //{
                //    message.Subject = "Sales Activity Assigned - " + activityname + " ";
                //}
                //else if (trSenderType == 5)//Sales Re-assigned
                //{
                //    message.Subject = GetEmailTemplateSubjectBySenderType(trSenderType) + " - " + activityname;
                //}
                //else if(trSenderType==6)//Sales Activity Feedback
                //{
                //    message.Subject = GetEmailTemplateSubjectBySenderType(trSenderType) + " - " + activityname;
                //}
                //else if(trSenderType==7)//Phone Calls
                //{
                //    message.Subject = GetEmailTemplateSubjectBySenderType(trSenderType) + " - " + activityname;
                //}
                //else if (trSenderType == 9)//SMS
                //{
                //    message.Subject = GetEmailTemplateSubjectBySenderType(trSenderType) + " - " + activityname;
                //}
                //else if (trSenderType == 8)//Emails
                //{
                //    message.Subject = GetEmailTemplateSubjectBySenderType(trSenderType) + " - " + activityname;
                //}
                //else if (trSenderType == 11)//Meeting
                //{
                //    message.Subject = GetEmailTemplateSubjectBySenderType(trSenderType) + " - " + activityname;
                //}
                //else if (trSenderType == 10)//Sales Visit
                //{
                //    message.Subject = GetEmailTemplateSubjectBySenderType(trSenderType) + " - " + activityname;
                //}
                #endregion

                message.Subject = GetEmailTemplateSubjectBySenderType(trSenderType) + " - " + activityname + " ";

                message.From = Sender;
                message.To.Add(receiver);
                //string sExceptionDescription = FormatExceptionDescription(e,i);

                //EmailTemplate
                List<String> mailTemplateKeys = new List<String>();
                List<String> mailTemplateValues = new List<String>();
                foreach (string columnkeys in replacements.Keys)
                {
                    mailTemplateKeys.Add(columnkeys);
                }
                foreach (string columnvalues in replacements.Values)
                {
                    mailTemplateValues.Add(columnvalues);
                }
                //CommentedBy:subhabrata
                //string mailBody = GetMailBodyTemplate(TemplatePath);
                //End

                string mailBody = GetEmailTemplateBySenderType(trSenderType);
                for (int j = 0; j < mailTemplateKeys.Count; j++)
                {
                    mailBody = mailBody.Replace(mailTemplateKeys[j].ToString(), mailTemplateValues[j].ToString());
                }
                //End
                message.Body = mailBody;
                //message.Body = sExceptionDescription;
                message.IsBodyHtml = true;
                smtp.Send(message);
                m = 1;
            }
            catch(Exception ex)
            {
                m = -1;
                
            }
           
            //}
            return m;
        }
        #endregion

        //Done By:Subhabrata
        #region ExceptionFetchingFunctionality
        protected static string FormatExceptionDescription(Exception e)
        {
            StringBuilder sb = new StringBuilder();
            //HttpContext context = HttpContext.Current;
            sb.Append("<b>Time of Error: </b>" + DateTime.Now.ToString("g") + "<br />");
            sb.Append("<b>URL: </b>" + context.Current.Request.Url.ToString() + "<br/>");
            while (e != null)
            {
                sb.Append("<b>Message: </b>" + e.Message + "<br />");
                sb.Append("<b> Source: </b>" + e.Source + "<br />");
                sb.Append("<b>StackTrace: </b>" + e.StackTrace + "<br />");
                sb.Append(Environment.NewLine + "<br />");
                e = e.InnerException;
            }
            sb.Append("--------------------------------------------------------" + "<br />");
            sb.Append("Regards," + "<br />");
            sb.Append("Admin");
            return sb.ToString();
        }
        #endregion

        //Done By:Subhabrata
        #region ReadTemplateAndFetchStringValueFunctionality
        public static string GetMailBodyTemplate(string TemplatePath)
        {
            
            var strPath = HostingEnvironment.MapPath(TemplatePath);
            FileInfo file = new FileInfo(strPath);
            String str = string.Empty;
            if (file != null)
            {
                FileStream fm = File.Open(strPath, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fm);
                str = sr.ReadToEnd();
                sr.Close();
                fm.Close();
            }
            return str;
        }
        #endregion

        //Done By:Subhabrata
        #region HTMLDecodedValueFromDatabase
        public static string GetEmailTemplateBySenderType(int senderType)
        {
            string strTemplateVal = string.Empty;
            Employee_BL emplBL = new Employee_BL();
            DataTable dt = new DataTable();
            dt = emplBL.GetEmailTemplateBySenderType(senderType);
            if (dt.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dt.Rows[0].Field<string>("tem_msg")))
                {
                    strTemplateVal = System.Web.HttpUtility.HtmlDecode(Convert.ToString(dt.Rows[0].Field<string>("tem_msg")));
                }
                
            }
            return strTemplateVal;
        }
        #endregion

        #region EmailtemplateSubject
        public static string GetEmailTemplateSubjectBySenderType(int senderType)
        {
            string strTemplateVal = string.Empty;
            Employee_BL emplBL = new Employee_BL();
            DataTable dt = new DataTable();
            dt = emplBL.GetEmailTemplateBySenderType(senderType);
            if (dt.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dt.Rows[0].Field<string>("tem_shortmsg")))
                {
                    strTemplateVal = System.Web.HttpUtility.HtmlDecode(Convert.ToString(dt.Rows[0].Field<string>("tem_shortmsg")));
                }

            }
            return strTemplateVal;
        }
        #endregion

    }
}