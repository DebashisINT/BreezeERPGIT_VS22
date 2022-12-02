using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using BusinessLogicLayer;
using System.Net.Mail;
using System.Net;
using EntityLayer.MailingSystem;

namespace UtilityLayer
{
    public class SendEmailUL
    {
        public static int sendMailInHtmlFormat(EmailSenderEL objEmail, out string Message)
        {
            MailMessage message = new MailMessage();
            Message = "";
            try
            {
                string[] AllRecipient = null;
                string[] AllCC = objEmail.CC.Split(',');
                //List<string> AllBCC = CommonHelperMethods.GetStringToStringList(objEmail.BCC, ',');
                if (objEmail.sendMailTo.Contains(","))
                {
                    AllRecipient = objEmail.sendMailTo.Split(',');
                }

                message.Body = objEmail.mailBody;
                message.Subject = objEmail.mailSubject;
                message.IsBodyHtml = true;
                message.Priority = MailPriority.High;

                if (objEmail.mailAttachment != "" && objEmail.mailAttachment != null)
                {

                    if (System.IO.File.Exists(objEmail.mailAttachment))
                    {
                        System.Net.Mail.Attachment attachment;
                        attachment = new System.Net.Mail.Attachment(objEmail.mailAttachment);
                        message.Attachments.Add(attachment);
                    }

                }

                message.From = new MailAddress(objEmail.sendMailForm, objEmail.SenderName);
                //message.From = new MailAddress(FromName, FromName);

                if (AllRecipient != null && AllRecipient.Length > 0)
                {
                    foreach (var item in AllRecipient)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            message.To.Add(item);
                        }
                    }
                }
                else
                {
                    message.To.Add(objEmail.sendMailTo);
                }

                if (AllCC != null && AllCC.Count() > 0)
                {
                    foreach (var item in AllCC)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            message.CC.Add(item);
                        }
                    }
                }

                //if (AllBCC != null && AllBCC.Count > 0)
                //{
                //    foreach (var item in AllBCC)
                //    {
                //        if (!string.IsNullOrEmpty(item))
                //        {
                //            message.Bcc.Add(item);
                //        }
                //    }
                //}
                //var userpassword = new HRMS_UtilityLayer.EncryptionUL().Decrypt(objEmail.mailPassword);
                var userpassword = objEmail.mailPassword;
                SmtpClient mailClient = new SmtpClient(objEmail.SMTPServer, objEmail.SmtpPort);
                mailClient.UseDefaultCredentials = false;
                NetworkCredential cred = new NetworkCredential(objEmail.SMTPUser, userpassword);

                mailClient.Credentials = cred;
          mailClient.EnableSsl = objEmail.EnableSsl;
        // mailClient.EnableSsl = true;
                mailClient.Send(message);

                message.Dispose();
                return 1;
            }

            catch (Exception ex)
            {
                message.Dispose();
                Message = ex.Message;
                return 0;
            }
        }
    }


   
}
