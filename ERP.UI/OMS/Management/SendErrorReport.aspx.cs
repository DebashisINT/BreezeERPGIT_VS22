using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ionic.Zip;
using System.IO;
using System.Net.Mail;
using System.Net;
using BusinessLogicLayer;
using DataAccessLayer;
using System.Xml;
using System.Configuration;

namespace ERP.OMS.Management
{
    public partial class Management_SendErrorReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnSendMail_Click(object sender, EventArgs e)
        {
            string toemailid = txtToEmailId.Text;
            ApplicationErrorBL obj = new ApplicationErrorBL();
            try
            {
                string CompanyName = Convert.ToString(Session["LastCompany"]);
                string zipfilepath = string.Empty;
                string path = "~/XMLErrorFiles/";
                //string zipfilename = CreateZipFolder(path,CompanyName);
                using (ZipFile zip = new ZipFile())
                {
                    string Path = Server.MapPath(path);
                    List<string> filenames = Directory.GetFiles(Path).ToList();
                    string filename = Path + "Error_" + CompanyName + ".xml";

                    var b = filenames.Contains(filename);
                    string[] a = filenames.Where(x => x == filename).Take(1).ToArray();

                    zip.AddFiles(a, "files");
                    zip.Save(Server.MapPath("~/XMLErrorFiles/Error_" + CompanyName + ".zip"));
                }

                MailAddress SendFrom = new MailAddress(ConfigurationManager.AppSettings["FromEmailId"].ToString());
                MailAddress SendTo = new MailAddress(toemailid);

                MailMessage MyMessage = new MailMessage(SendFrom, SendTo);

                MyMessage.Subject = "Error Report";
                MyMessage.Body = "Please find the detailed error report attached herewith.";


                Attachment attachFile = new Attachment(Server.MapPath("~/XMLErrorFiles/Error_" + CompanyName + ".zip"));
                FileInfo fileinf = new FileInfo(Server.MapPath("~/XMLErrorFiles/Error_" + CompanyName + ".zip"));

                if (fileinf.Length <= 20971520)
                {
                    MyMessage.Attachments.Add(attachFile);

                }
                else
                {
                    Response.Write("Attachment size must be less than 20 Mb");
                }
                NetworkCredential cred = new NetworkCredential(ConfigurationManager.AppSettings["FromEmailId"].ToString(), ConfigurationManager.AppSettings["FromPassword"].ToString());
                SmtpClient emailClient = new SmtpClient();
                emailClient.Credentials = cred;
                emailClient.EnableSsl = true;
                emailClient.Host = "smtp.gmail.com";
                emailClient.Port = 25;
                emailClient.Send(MyMessage);
                MyMessage.Dispose();
                Response.Write("Mail Sent");

                //XmlDocument doc = new XmlDocument();
                //doc.Load(Server.MapPath("~/XMLErrorFiles/Error_" + CompanyName + ".xml"));
                string text = File.ReadAllText(Server.MapPath("~/XMLErrorFiles/Error_" + CompanyName + ".xml"));
                string UserId = Session["userid"].ToString();
                int i = obj.ApplicationErrorInsert(UserId, text);
                if (i > 0)
                {
                    if (File.Exists(Server.MapPath("~/XMLErrorFiles/Error_" + CompanyName + ".xml")))
                    {
                        File.Delete(Server.MapPath("~/XMLErrorFiles/Error_" + CompanyName + ".xml"));
                        File.Delete(Server.MapPath("~/XMLErrorFiles/Error_" + CompanyName + ".zip"));
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}