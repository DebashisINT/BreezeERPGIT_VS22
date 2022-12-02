using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dashboard_React.ajax.mngNotification
{
    public partial class mngNotification : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        public static object GetAllNotificationData(string action)
        {
            List<allDataClass> lEfficency = new List<allDataClass>();
            ProcedureExecute proc = new ProcedureExecute("PRC_MGMTNOTIFICATIONDB_REPORT");
            proc.AddVarcharPara("@Action", 100, action);

            DataTable CallData = proc.GetTable();
            //var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new allDataClass()
                          {
                              TOTCUST = Convert.ToString(dr["TOTCUST"]),
                              TOTVEND = Convert.ToString(dr["TOTVEND"]),
                              TOTEMP = Convert.ToString(dr["TOTEMP"]),
                              CNTINF = Convert.ToString(dr["CNTINF"]),
                              CNTTRANS = Convert.ToString(dr["CNTTRANS"])
                          }).ToList();
            return lEfficency;
        }
        [WebMethod]
        public static object GetAllCustomer(string action)
        {
            List<customerClass> lEfficency = new List<customerClass>();
            ProcedureExecute proc = new ProcedureExecute("PRC_MGMTNOTIFICATIONDB_REPORT");
            proc.AddVarcharPara("@Action", 100, action);

            DataTable CallData = proc.GetTable();
            //var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new customerClass()
                          {
                              COMPANY = Convert.ToString(dr["COMPANY"]),
                              CONTACTPERSON = Convert.ToString(dr["CONTACTPERSON"]),
                              EVENT_TYPE = Convert.ToString(dr["EVENT_TYPE"]),
                              PHNO = Convert.ToString(dr["PHNO"]),
                              EMAIL = Convert.ToString(dr["EMAIL"])
                          }).ToList();
            return lEfficency;
        }

        [WebMethod]
        public static object GetAllVendor(string action)
        {
            List<vendorClass> lEfficency = new List<vendorClass>();
            ProcedureExecute proc = new ProcedureExecute("PRC_MGMTNOTIFICATIONDB_REPORT");
            proc.AddVarcharPara("@Action", 100, action);

            DataTable CallData = proc.GetTable();
            //var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new vendorClass()
                          {
                              COMPANY = Convert.ToString(dr["COMPANY"]),
                              CONTACTPERSON = Convert.ToString(dr["CONTACTPERSON"]),
                              EVENT_TYPE = Convert.ToString(dr["EVENT_TYPE"]),
                              PHNO = Convert.ToString(dr["PHNO"]),
                              EMAIL = Convert.ToString(dr["EMAIL"])
                          }).ToList();
            return lEfficency;
        }
        [WebMethod]
        public static object GetAllInfluencer(string action)
        {
            List<influencerClass> lEfficency = new List<influencerClass>();
            ProcedureExecute proc = new ProcedureExecute("PRC_MGMTNOTIFICATIONDB_REPORT");
            proc.AddVarcharPara("@Action", 100, action);

            DataTable CallData = proc.GetTable();
            //var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new influencerClass()
                          {
                              COMPANY = Convert.ToString(dr["COMPANY"]),
                              CONTACTPERSON = Convert.ToString(dr["CONTACTPERSON"]),
                              EVENT_TYPE = Convert.ToString(dr["EVENT_TYPE"]),
                              PHNO = Convert.ToString(dr["PHNO"]),
                              EMAIL = Convert.ToString(dr["EMAIL"])
                          }).ToList();
            return lEfficency;
        }

        [WebMethod]
        public static object GetAllTransporter(string action)
        {
            List<transporterClass> lEfficency = new List<transporterClass>();
            ProcedureExecute proc = new ProcedureExecute("PRC_MGMTNOTIFICATIONDB_REPORT");
            proc.AddVarcharPara("@Action", 100, action);

            DataTable CallData = proc.GetTable();
            //var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new transporterClass()
                          {
                              COMPANY = Convert.ToString(dr["COMPANY"]),
                              CONTACTPERSON = Convert.ToString(dr["CONTACTPERSON"]),
                              EVENT_TYPE = Convert.ToString(dr["EVENT_TYPE"]),
                              PHNO = Convert.ToString(dr["PHNO"]),
                              EMAIL = Convert.ToString(dr["EMAIL"])
                          }).ToList();
            return lEfficency;
        }

        [WebMethod]
        public static object GetAllEmployee(string action)
        {
            List<employeeClass> lEfficency = new List<employeeClass>();
            ProcedureExecute proc = new ProcedureExecute("PRC_MGMTNOTIFICATIONDB_REPORT");
            proc.AddVarcharPara("@Action", 100, action);

            DataTable CallData = proc.GetTable();
            //var random = new Random();
            lEfficency = (from DataRow dr in CallData.Rows
                          select new employeeClass()
                          {
                              COMPANY = Convert.ToString(dr["COMPANY"]),
                              CONTACTPERSON = Convert.ToString(dr["CONTACTPERSON"]),
                              EVENT_TYPE = Convert.ToString(dr["EVENT_TYPE"]),
                              PHNO = Convert.ToString(dr["PHNO"]),
                              EMAIL = Convert.ToString(dr["EMAIL"])
                          }).ToList();
            return lEfficency;
        }

        [WebMethod]
        public static string SendEmail(string Email, string body, string subject)
        {
            String Status = SendMail(Email, "", "", subject, body, null);
           return Status;
        }

        internal static String SendMail(string ToEmail, string CCEmail, string BCCEmail, string Subject, string EmailBody, HttpFileCollectionBase files)
        {
            try
            {

                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable dtFromEmailDet = oDBEngine.GetDataTable(" select top(1) EmailAccounts_EmailID,EmailAccounts_Password,EmailAccounts_FromName,LTRIM(RTRIM(EmailAccounts_SMTP)) AS EmailAccounts_SMTP,LTRIM(RTRIM(EmailAccounts_SMTPPort)) AS EmailAccounts_SMTPPort from Config_EmailAccounts where EmailAccounts_InUse='Y' ");

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
    }


    public class employeeClass
    {
        public string COMPANY { get; set; }
        public string CONTACTPERSON { get; set; }
        public string EVENT_TYPE { get; set; }
        public string PHNO { get; set; }
        public string EMAIL { get; set; }

    }
    public class transporterClass
    {
        public string COMPANY { get; set; }
        public string CONTACTPERSON { get; set; }
        public string EVENT_TYPE { get; set; }
        public string PHNO { get; set; }
        public string EMAIL { get; set; }

    }
    public class influencerClass
    {
        public string COMPANY { get; set; }
        public string CONTACTPERSON { get; set; }
        public string EVENT_TYPE { get; set; }
        public string PHNO { get; set; }
        public string EMAIL { get; set; }

    }
    public class vendorClass
    {
        public string COMPANY { get; set; }
        public string CONTACTPERSON { get; set; }
        public string EVENT_TYPE { get; set; }
        public string PHNO { get; set; }
        public string EMAIL { get; set; }

    }
    public class customerClass
    {
        public string COMPANY { get; set; }
        public string CONTACTPERSON { get; set; }
        public string EVENT_TYPE { get; set; }
        public string PHNO { get; set; }
        public string EMAIL { get; set; }

    }
    public class allDataClass
    {
        public string TOTCUST { get; set; }
        public string TOTVEND { get; set; }
        public string TOTEMP { get; set; }
        public string CNTINF { get; set; }
        public string CNTTRANS { get; set; }

    }
    
}