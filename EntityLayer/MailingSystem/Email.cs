using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.MailingSystem
{
  public  class Email
    {
    }
    public class EmailSenderEL
    {
        public string sendMailForm { get; set; }
        public string sendMailTo { get; set; }
        public string mailSubject { get; set; }
        public string mailBody { get; set; }

        public string mailPassword { get; set; }
        public string SMTPServer { get; set; }
        public int SmtpPort { get; set; }

        public string SMTPUser { get; set; }

        public bool EnableSsl { get; set; }
        public string sender { get; set; }

        public string SenderName { get; set; }

        public string CC { get; set; }

        public string BCC { get; set; }
        public string mailAttachment { get; set; }
    }

}
