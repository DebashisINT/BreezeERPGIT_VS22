using EmployeeSelfService.Areas.EmployeeSelfService.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web;
using System.Web.Http;


namespace EmployeeSelfService.Areas.EmployeeSelfService.Controllers
{
    public class SendEmailController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage SendEmail(inputEmailClass model)
        {
             
            DataTable dtFromEmailDet = new DataTable();

            String con = Convert.ToString(APIConnction.ApiConnction);
            SqlCommand sqlcmd = new SqlCommand();

            SqlConnection sqlcon = new SqlConnection(con);
            sqlcon.Open();
            sqlcmd = new SqlCommand("select top(1) EmailAccounts_EmailID,EmailAccounts_Password,EmailAccounts_FromName,LTRIM(RTRIM(EmailAccounts_SMTP)) AS EmailAccounts_SMTP,LTRIM(RTRIM(EmailAccounts_SMTPPort)) AS EmailAccounts_SMTPPort from Config_EmailAccounts where EmailAccounts_InUse='Y'", sqlcon);
            sqlcmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
            da.Fill(dtFromEmailDet);
            sqlcon.Close();
            
            
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
            string[] ToAdd = model.ToEmail.Split(',');
            string[] CcAdd = model.CCEmail.Split(',');
            string[] BccAdd = model.BCCEmail.Split(',');
            var Body = model.EmailBody;
            var EmailSubject = model.Subject;
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
            
            smtp.Host = OutgoingSMTPHost.Trim();
            smtp.Port = Convert.ToInt32(OutgoingPort);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(FromAdd, Password);
            smtp.EnableSsl = true;

            smtp.Send(mail);
            smtp.Dispose();
            mail.Dispose();
            var message = Request.CreateResponse(HttpStatusCode.OK,"success");
            return message;
               
        }
    }

}
