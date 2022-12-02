using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace CRM.Models
{
    public class crmSharing
    {
        public List<crmShareEmail> emails { get; set; }
        public List<crmShareSMS> phones { get; set; }


        internal System.Data.DataSet GetEntityDetails(string Module_Name, string Module_id)
        {
            try
            {
                string Actionname = "";
                if (Module_Name!="Lead")
                {
                    Actionname = "EDITDETAILS";
                }
                else
                {
                    Actionname = "LEADEDITDETAILS";
                }
                int OutputId = 0;
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("crm_Share", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ACTION", Actionname);
                cmd.Parameters.AddWithValue("@Module_Name", Module_Name);
                cmd.Parameters.AddWithValue("@Module_id", Module_id);
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();
                return dsInst;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal String SaveLog( string ActionType, string ToEmail, string CCEmail, string BCCEmail, string Subject, string EmailBody,string module_id,string module_name)
        {
            DataTable dtchkassign = new DataTable();

            int OutputId = 0;
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("PRC_EMAIL_ACTIVITY", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ACTION_TYPE", ActionType);
            cmd.Parameters.AddWithValue("@LEAD_ENTITY_ID", module_id);
            cmd.Parameters.AddWithValue("@EMAILTO", ToEmail);
            cmd.Parameters.AddWithValue("@EMAILCC", CCEmail);
            cmd.Parameters.AddWithValue("@EMAILBCC", BCCEmail);
            cmd.Parameters.AddWithValue("@EMAILSUBJECT", Subject);
            cmd.Parameters.AddWithValue("@EMAILDETAILS", EmailBody);
            cmd.Parameters.AddWithValue("@EMAIL_STATUS", "Success");
            cmd.Parameters.AddWithValue("@MODULENAME", module_name);
            cmd.Parameters.AddWithValue("@CREATED_BY", Convert.ToInt32(HttpContext.Current.Session["userid"].ToString()));

            SqlParameter output = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 500);
            output.Direction = ParameterDirection.Output;
            SqlParameter outputText = new SqlParameter("@ReturnCode", SqlDbType.VarChar, 20);
            outputText.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(output);
            cmd.Parameters.Add(outputText);

            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dtchkassign);
            cmd.Dispose();
            con.Dispose();

            OutputId = Convert.ToInt32(cmd.Parameters["@ReturnCode"].Value.ToString());
            string strCPRID = Convert.ToString(cmd.Parameters["@ReturnMessage"].Value.ToString());

            return strCPRID;
        }

        internal String SendMail(string ActionType, string ToEmail, string CCEmail, string BCCEmail, string Subject, string EmailBody, string module_id, string module_name, HttpFileCollectionBase files)
        {
            try
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable dtFromEmailDet = new DataTable();
                dtFromEmailDet = oDBEngine.GetDataTable("select top(1) EmailAccounts_EmailID,EmailAccounts_Password,EmailAccounts_FromName,LTRIM(RTRIM(EmailAccounts_SMTP)) AS EmailAccounts_SMTP,LTRIM(RTRIM(EmailAccounts_SMTPPort)) AS EmailAccounts_SMTPPort from Config_EmailAccounts where EmailAccounts_InUse='Y'");
                var Email = dtFromEmailDet.Rows[0][0].ToString();
                var Password = dtFromEmailDet.Rows[0][1].ToString();
                
                //var Email = "subhra.mukherjee@indusnet.co.in";
                //var Password = "subhra@12345";
                var FromWhere = dtFromEmailDet.Rows[0][2].ToString();
                var OutgoingSMTPHost = dtFromEmailDet.Rows[0][3].ToString();
                var OutgoingPort = dtFromEmailDet.Rows[0][4].ToString();
                MailMessage mail = new MailMessage();
                SmtpClient smtp = new SmtpClient(OutgoingSMTPHost);
                var FromAdd = Email;
                string[] ToAdd = ToEmail.Split(',');
                string[] CcAdd = CCEmail.Split(',');
                string[] BccAdd = BCCEmail.Split(',');
                var Body = EmailBody;
                var EmailSubject = Subject;
                mail.From = new MailAddress(FromAdd, FromWhere);

                foreach (string to in ToAdd)
                {
                    mail.To.Add(to);
                }
                foreach (string cc in CcAdd)
                {
                    if (cc != "")
                    {
                        mail.CC.Add(cc);
                    }
                }
                foreach (string bcc in BccAdd)
                {
                    if (bcc != "")
                    {
                        mail.Bcc.Add(bcc);
                    }
                }
                
                mail.Subject = EmailSubject;
                mail.IsBodyHtml = true;
                mail.Body = Body;
                HttpPostedFileBase file = null;
                HttpFileCollectionBase filess = files;
                if (filess != null)
                {
                    for (int i = 0; i < filess.Count; i++)
                    {

                        if (filess[i] != null && files[i].ContentLength > 0)
                        {
                            var attachment = new Attachment(filess[i].InputStream, filess[i].FileName);
                            mail.Attachments.Add(attachment);
                        }
                    }

                }


                smtp.Host = OutgoingSMTPHost.Trim();
                smtp.Port = Convert.ToInt32(OutgoingPort);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(FromAdd, Password);
                smtp.EnableSsl = true;
                
                smtp.Send(mail);
                smtp.Dispose();
                mail.Dispose();
                return "Success";
            }
            catch (Exception EX)
            {
                return null;
            }
        }

        internal string SaveSMS(string ActionType, string Contactid, string MobileNo, string SmsContent, string Module_Name)
        {
            DataTable dtchkassignsms = new DataTable();

            int OutputId = 0;
            SqlConnection con1 = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("PRC_SMS_ACTIVITY", con1);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ACTION_TYPE", ActionType);
            cmd.Parameters.AddWithValue("@ENTITY_ID", Contactid);
            cmd.Parameters.AddWithValue("@MOBILENO", MobileNo);
            cmd.Parameters.AddWithValue("@SMSCONTENT", SmsContent);
            cmd.Parameters.AddWithValue("@SMS_STATUS", "Success");
            cmd.Parameters.AddWithValue("@MODULENAME", Module_Name);
            cmd.Parameters.AddWithValue("@CREATED_BY", Convert.ToInt32(HttpContext.Current.Session["userid"].ToString()));
            SqlParameter output = new SqlParameter("@ReturnMessage", SqlDbType.VarChar, 500);
            output.Direction = ParameterDirection.Output;
            SqlParameter outputText = new SqlParameter("@ReturnCode", SqlDbType.VarChar, 20);
            outputText.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(output);
            cmd.Parameters.Add(outputText);

            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dtchkassignsms);
            cmd.Dispose();
            con1.Dispose();

            OutputId = Convert.ToInt32(cmd.Parameters["@ReturnCode"].Value.ToString());
            string strCPRID = Convert.ToString(cmd.Parameters["@ReturnMessage"].Value.ToString());

            return strCPRID;
        }
        public string SmsSent(string username, string password, string Provider, string senderId, string mobile, string message, string type)
        {


            string response = "";
            string url = Provider + "?username=" + username + "&password=" + password + "&type=" + type + "&sender=" + senderId + "&mobile=" + mobile + "&message=" + message;
            if (mobile.Trim() != "")
            {
                try
                {
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    response = httpResponse.StatusCode.ToString();
                }
                catch
                {
                    return "0";
                }
            }
            return response;
        }

    }

    public class crmShareEmail
    {
        public string Entity_Name { get; set; }
        public string Entity_Email { get; set; }

    }

    public class crmShareSMS
    {
        public string Entity_Name { get; set; }
        public string Entity_Phone { get; set; }

    }
}