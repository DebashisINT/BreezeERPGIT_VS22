using System;
using System.Data;
using System.Web;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_CustomerEmailDetails : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            String MailID = Request.QueryString["id"].ToString();
            GetEmailsDetail();
        }
        public void GetEmailsDetail()
        {
            String MailID = Request.QueryString["id"].ToString();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            dt1 = oDBEngine.GetDataTable("Trans_EmailAttachment", "EmailAttachment_ID,EmailAttachment_MainID,EmailAttachment_Path", "EmailAttachment_MainID=" + MailID + "");
            if (HttpContext.Current.Session["userlastsegment"].ToString() == "1")
            {
                dt = oDBEngine.GetDataTable("Trans_Emails as E,Trans_EmailRecipients as R", " R.EmailRecipients_ID,E.Emails_ID,(Select  user_name from  tbl_master_user where  user_id = E.Emails_CreateUser) as sendFrom,case when  SUBSTRING( R.EmailRecipients_ContactLeadID , 1 , 2 )='LD' Then (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_lead where cnt_internalId=R.EmailRecipients_ContactLeadID)Else(select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_contact where cnt_internalId=R.EmailRecipients_ContactLeadID) end as RecepientsName,R.EmailRecipients_RecipientEmailID,E.Emails_Subject,E.Emails_Content,R.EmailRecipients_RecipientType,Convert(varchar(30),R.EmailRecipients_SentDateTime,100) as SentDate,R.EmailRecipients_Status,R.EmailRecipients_AttemptNumber", " E.Emails_ID=R.EmailRecipients_MainID  and E.Emails_ID=" + MailID + "");
            }
            else
            {
                dt = oDBEngine.GetDataTable("Trans_Emails as E,Trans_EmailRecipients as R", " R.EmailRecipients_ID,E.Emails_ID,(Select  user_name from  tbl_master_user where  user_id = E.Emails_CreateUser) as sendFrom,case when  SUBSTRING( R.EmailRecipients_ContactLeadID , 1 , 2 )='LD' Then (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_lead where cnt_internalId=R.EmailRecipients_ContactLeadID)Else(select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_contact where cnt_internalId=R.EmailRecipients_ContactLeadID) end as RecepientsName,R.EmailRecipients_RecipientEmailID,E.Emails_Subject,E.Emails_Content,R.EmailRecipients_RecipientType,Convert(varchar,R.EmailRecipients_SentDateTime,100) as SentDate,R.EmailRecipients_Status", " E.Emails_ID=R.EmailRecipients_MainID  and E.Emails_ID=" + MailID + "");
            }
            string rid = dt.Rows[0]["EmailRecipients_ID"].ToString();
            dt2 = oDBEngine.GetDataTable("Trans_EmailLog", "EmailLog_ID,Convert(varchar(30),EmailLog_SentDateTime,100) as EmailLog_SentDateTime,case when EmailLog_DeliveryStatus='P'  then 'Pending' when EmailLog_DeliveryStatus='F'  then 'Fail' when EmailLog_DeliveryStatus='B'  then 'Bounced' when EmailLog_DeliveryStatus='X'  then 'Parmanent Failure' when EmailLog_DeliveryStatus='D'  then 'Delivered' else 'Pending'   END  as EmailLog_DeliveryStatus,EmailLog_Reason", "EmailLog_EmailRecipientID=" + rid + "");
            lblContent.Text = dt.Rows[0]["Emails_Content"].ToString();
            lblSub.Text = dt.Rows[0]["Emails_Subject"].ToString();
            lblType.Text = dt.Rows[0]["EmailRecipients_RecipientType"].ToString();
            lblName.Text = dt.Rows[0]["RecepientsName"].ToString();
            lblEmail.Text = "(" + dt.Rows[0]["EmailRecipients_RecipientEmailID"].ToString() + ")";

            //if (dt1.Rows.Count != 0)
            //{
            //    AttachGrid.DataSource = dt1.DefaultView;
            //    AttachGrid.DataBind();
            //}
            //else
            //{
            //    AttachGrid.Visible = false;
            //}

            //if (dt2.Rows.Count != 0)
            //{
            //    logGrid.DataSource = dt2.DefaultView;
            //    logGrid.DataBind();
            //}
            //else
            //{
            //    logGrid.Visible = false;
            //}


            string disTBL = "<table cellspacing=\"1\" cellpadding=\"1\" border=\"1\"  width=\"900px\" style=\"background-color:#dcdcdc\">";
            if (dt1.Rows.Count != 0)
            {
                disTBL += "<tr><td>Attachment</td><td>View</td></tr>";
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    // disTBL += "<tr style=\"background-color:#ffffff\"><td>" + i+1 + "</td><td>" + dt1.Rows[i]["EmailAttachment_Path"].ToString() + "</td></tr>";
                    //string[] docsrc = dt1.Rows[i]["EmailAttachment_Path"].ToString().Split('\\');
                    //int b = docsrc.Length;
                    //string[] docname = docsrc[b - 1].ToString().Split('_');

                    disTBL += "<tr style=\"background-color:#ffffff\"><td>" + dt1.Rows[i]["EmailAttachment_Path"].ToString() + "</td><td><a href=\"ViewAttachment.aspx?id=" + dt1.Rows[i]["EmailAttachment_Path"].ToString() + "\" target=\"_blank\">View</a></td></tr>";
                }
            }
            else
            {
                disTBL += "<tr><td>No Attachment.</td></tr>";
            }
            disTBL += "</table>";
            lblAtt.Text = disTBL;

            string disLogTBL = "<table cellspacing=\"1\" cellpadding=\"1\" border=\"1\"  width=\"900px\" style=\"background-color:#dcdcdc\">";
            disLogTBL += "<tr><td colspan=\"3\" align=\"left\" style=\"font-size:12px;font-weight:bold\">Sent Details</td></tr>";
            if (dt2.Rows.Count != 0)
            {
                disLogTBL += "<tr><td>Sent Datetime</td><td>Status</td><td>Reason</td></tr>";
                for (int i = 0; i < dt2.Rows.Count; i++)
                {

                    disLogTBL += "<tr style=\"background-color:#ffffff\"><td>" + dt2.Rows[i]["EmailLog_SentDateTime"].ToString() + "</td><td>" + dt2.Rows[i]["EmailLog_DeliveryStatus"].ToString() + "</td><td>" + dt2.Rows[i]["EmailLog_Reason"].ToString() + "</td></tr>";
                }
            }
            else
            {
                disLogTBL += "<tr><td>No Attachment.</td></tr>";
            }
            disLogTBL += "</table>";
            lblLog.Text = disLogTBL;


        }
    }

}