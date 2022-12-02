using DataAccessLayer;
using DevExpress.Export;
using DevExpress.XtraPrinting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Attendance
{
    public partial class DailyAttRegister : System.Web.UI.Page
    {
       // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                loadDropdown();
                toDate.MinDate = Convert.ToDateTime(Session["FinYearStart"]);
                toDate.MaxDate = Convert.ToDateTime(Session["FinYearEnd"]);

                if (DateTime.Now > Convert.ToDateTime(Session["FinYearEnd"]))
                {
                    toDate.Date = Convert.ToDateTime(Session["FinYearEnd"]);
                }
                else {
                    toDate.Date = DateTime.Now;
                }
            }
        }


        private void loadDropdown()
        {
            ProcedureExecute proc = new ProcedureExecute("prc_Followup");
            proc.AddVarcharPara("@action", 100, "populateParent");
            DataTable dt = proc.GetTable();

            cmbMainUnit.DataSource = dt;
            cmbMainUnit.TextField = "branch_description";
            cmbMainUnit.ValueField = "branch_id";
            cmbMainUnit.DataBind();
        }

        [WebMethod]
        public static object GetAllDetailsByBranch(string BranchId)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_Followup");
            proc.AddVarcharPara("@action", 100, "populateChild");
            proc.AddPara("@userbranchHierarchy", Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            proc.AddVarcharPara("@Parentbranch", 100, BranchId);
            DataTable dt = proc.GetTable();
            List<ERP.OMS.Management.Activities.PosSalesInvoice.KeyValueClass> BranchChild = new List<ERP.OMS.Management.Activities.PosSalesInvoice.KeyValueClass>();
            BranchChild = (from DataRow dr in dt.Rows
                           select new ERP.OMS.Management.Activities.PosSalesInvoice.KeyValueClass()
                           {
                               Id = dr["branch_id"].ToString(),
                               Name = dr["branch_description"].ToString()
                           }).ToList();

            return BranchChild;
        }

        protected void gridAttendance_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            gridAttendance.JSProperties["cpCallPara"] = e.Parameters;
            if (e.Parameters == "SendMail")
            {
                try
                {
                    DataTable MailDt = oDBEngine.GetDataTable("select EmailAccounts_SMTP,EmailAccounts_EmailID,EmailAccounts_SMTPPort,EmailAccounts_Password from Config_EmailAccounts where EmailAccounts_InUse='Y'");

                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient(Convert.ToString(MailDt.Rows[0]["EmailAccounts_SMTP"]));
                    mail.From = new MailAddress(Convert.ToString(MailDt.Rows[0]["EmailAccounts_EmailID"]));
                    string[] MailList = MailTO.Value.Split(';');
                    foreach (string smailId in MailList)
                    {
                        if (smailId.Trim() != "")
                            mail.To.Add(smailId.Trim());
                    }


                    string[] CCMailList = CCMail.Value.Split(';');
                    foreach (string smailId in CCMailList)
                    {
                        if (smailId.Trim() != "")
                            mail.CC.Add(smailId.Trim());
                    }


                    MemoryStream ms = new MemoryStream();
                    exporter.WriteXls(ms);
                    ms.Position = 0;
                    mail.Subject = mailSubject.Text;
                    mail.Body = MailBody.Text;
                    //Attachment attachment;
                    //attachment = new Attachment(ms, "GridExported.xls", "application/ms-excel");


                    Attachment attachment;
                    attachment = new Attachment(ms, "Daily_Attendance Register [" + toDate.Date.ToString("dd-MM-yyyy") + "].xls", "application/ms-excel");

                    mail.Attachments.Add(attachment);

                    SmtpServer.Port = Convert.ToInt32(MailDt.Rows[0]["EmailAccounts_SMTPPort"]);
                    SmtpServer.Credentials = new System.Net.NetworkCredential(Convert.ToString(MailDt.Rows[0]["EmailAccounts_EmailID"]), Convert.ToString(MailDt.Rows[0]["EmailAccounts_Password"]));
                    SmtpServer.EnableSsl = true;

                    SmtpServer.Send(mail);
                    attachment.Dispose();
                    SmtpServer.Dispose();
                    mail.Dispose();
                    gridAttendance.JSProperties["cpRetMsg"] = "Mail Sent Successfully.";
                }
                catch (Exception ex) {
                    gridAttendance.JSProperties["cpRetMsg"] = ex.Message;
                }

            }


        }

        protected void gridAttendance_DataBinding(object sender, EventArgs e)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_DailyAttRegister");
            proc.AddVarcharPara("@action", 100, "GenerateReport");
            proc.AddVarcharPara("@BranchList", 100, hdBranchList.Value);
            proc.AddDateTimePara("@rptDate", toDate.Date);
            proc.AddPara("@ShowInActive", hdShowInactive.Value);
            proc.AddPara("@considerPayBranch", chkPayrollBranch.Checked);
            DataTable dt = proc.GetTable();
            gridAttendance.DataSource = dt;
        }

        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            { 
                bindexport(Filter); 
            }
        }


        public void bindexport(int Filter)
        {

            string filename = "Daily Attendance Sheet";
            exporter.FileName = filename;
             
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";
            exporter.Landscape = true;

            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                      
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }
        }
    
    
        [WebMethod]
        public static object GetEmail(string SerarchKey)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_DailyAttRegister");
            proc.AddVarcharPara("@action", 100, "GetEmail");
            proc.AddVarcharPara("@searchKey", 100, SerarchKey);
            DataTable dt = proc.GetTable();
            List<retEmail> retEmailList = new List<retEmail>();
            retEmailList = (from DataRow dr in dt.Rows
                           select new retEmail()
                           {
                               Id = dr["eml_email"].ToString(),
                               Email = dr["eml_email"].ToString()
                           }).ToList();

            return retEmailList;
        }

        public class retEmail {
            public string Id { get; set; }
            public string Email { get; set; }
        }



    }
}