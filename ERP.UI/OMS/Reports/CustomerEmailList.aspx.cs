using System;
using System.Data;
using System.Web.UI.WebControls;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_CustomerEmailList : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        BusinessLogicLayer.Converter OConvert = new BusinessLogicLayer.Converter();
        protected void Page_Load(object sender, EventArgs e)
        {

            this.Page.ClientScript.RegisterStartupScript(GetType(), "script1", "<script>height();</script>");

            txtFromDate.EditFormatString = OConvert.GetDateFormat("Date");
            txtToDate.EditFormatString = OConvert.GetDateFormat("Date");
            lbltxt.Text = "";
            if (!IsPostBack)
            {

                this.Page.ClientScript.RegisterStartupScript(GetType(), "script2", "<script>Pageload();</script>");
                txtFromDate.Value = Convert.ToDateTime(DateTime.Today);
                txtToDate.Value = Convert.ToDateTime(DateTime.Today);
                fillGrid();
            }

        }
        protected void grdGeneralTrial_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdGeneralTrial.PageIndex = e.NewPageIndex;
            fillGridSpecific();
        }
        protected void fillGrid()
        {
            DataTable dt = new DataTable();
            dt = oDBEngine.GetDataTable("Trans_Emails as E,Trans_EmailRecipients as R", "top 10 Convert(varchar(30),E.Emails_CreateDateTime,100) as Emails_CreateDateTime,E.Emails_ID,(Select  user_name from  tbl_master_user where  user_id = E.Emails_CreateUser) as sendFrom,case when  SUBSTRING( R.EmailRecipients_ContactLeadID , 1 , 2 )='LD' Then (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_lead where cnt_internalId=R.EmailRecipients_ContactLeadID)Else(select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_contact where cnt_internalId=R.EmailRecipients_ContactLeadID) end as RecepientsName,R.EmailRecipients_RecipientEmailID as RecipientsEmailID,E.Emails_Subject as Subject,SUBSTRING( E.Emails_Content , 1 , 15 )+'...' as Content,R.EmailRecipients_RecipientType  as RecipientType,Convert(varchar(30),R.EmailRecipients_SentDateTime,100) as SentDate,case when R.EmailRecipients_Status='P'  then 'Pending' when R.EmailRecipients_Status='F'  then 'Fail' when R.EmailRecipients_Status='B'  then 'Bounced' when R.EmailRecipients_Status='X'  then 'Parmanent Failure' when R.EmailRecipients_Status='D'  then 'Delivered' else 'Pending'   END  as Status,R.EmailRecipients_AttemptNumber as AttempNo ", " E.Emails_ID=R.EmailRecipients_MainID  and  R.EmailRecipients_ContactLeadID='CLP0000302' order by  E.Emails_ID Desc");
            grdGeneralTrial.DataSource = dt;
            grdGeneralTrial.DataBind();
        }
        protected void fillGridSpecific()
        {


            string types = rbUser.Value.ToString();
            DataTable dt = new DataTable();

            if (rbUser.Value.ToString() == "A")
            {
                string startdate = txtFromDate.Date.Month.ToString() + "/" + txtFromDate.Date.Day.ToString() + "/" + txtFromDate.Date.Year.ToString() + " 01:00 AM";
                string Enddate = txtToDate.Date.Month.ToString() + "/" + txtToDate.Date.Day.ToString() + "/" + txtToDate.Date.Year.ToString() + " 11:55 PM";
                dt = oDBEngine.GetDataTable("Trans_Emails as E,Trans_EmailRecipients as R", "Convert(varchar(30),E.Emails_CreateDateTime,100) as Emails_CreateDateTime,E.Emails_ID,(Select  user_name from  tbl_master_user where  user_id = E.Emails_CreateUser) as sendFrom,case when  SUBSTRING( R.EmailRecipients_ContactLeadID , 1 , 2 )='LD' Then (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_lead where cnt_internalId=R.EmailRecipients_ContactLeadID)Else(select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_contact where cnt_internalId=R.EmailRecipients_ContactLeadID) end as RecepientsName,R.EmailRecipients_RecipientEmailID as RecipientsEmailID,E.Emails_Subject as Subject,SUBSTRING( E.Emails_Content , 1 , 15 )+'...' as Content,R.EmailRecipients_RecipientType  as RecipientType,Convert(varchar(30),R.EmailRecipients_SentDateTime,100) as SentDate,case when R.EmailRecipients_Status='P'  then 'Pending' when R.EmailRecipients_Status='F'  then 'Fail' when R.EmailRecipients_Status='B'  then 'Bounced' when R.EmailRecipients_Status='X'  then 'Parmanent Failure' when R.EmailRecipients_Status='D'  then 'Delivered' else 'Pending'   END  as Status,R.EmailRecipients_AttemptNumber as AttempNo ", " E.Emails_ID=R.EmailRecipients_MainID  and  R.EmailRecipients_ContactLeadID='CLP0000302' and (CAST(R.EmailRecipients_SentDateTime AS datetime) >= CONVERT(varchar,'" + startdate + "', 101)) and (CAST(R.EmailRecipients_SentDateTime AS datetime) <= CONVERT(varchar,'" + Enddate + "', 101))   order by  E.Emails_ID Desc");
                if (dt.Rows.Count != 0)
                {

                    grdGeneralTrial.DataSource = dt;
                    grdGeneralTrial.DataBind();
                }
                else
                {

                    grdGeneralTrial.DataSource = dt;
                    grdGeneralTrial.DataBind();
                    lbltxt.Text = "No record found....";
                    lbltxt.Visible = true;
                }
                this.Page.ClientScript.RegisterStartupScript(GetType(), "script5", "<script>ShowEmployeeFilterForm('A');</script>");
            }
            else if (rbUser.Value.ToString() == "S")
            {
                string subject = txtSubject.Text.ToString();
                dt = oDBEngine.GetDataTable("Trans_Emails as E,Trans_EmailRecipients as R", "Convert(varchar(30),E.Emails_CreateDateTime,100) as Emails_CreateDateTime,E.Emails_ID,(Select  user_name from  tbl_master_user where  user_id = E.Emails_CreateUser) as sendFrom,case when  SUBSTRING( R.EmailRecipients_ContactLeadID , 1 , 2 )='LD' Then (select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_lead where cnt_internalId=R.EmailRecipients_ContactLeadID)Else(select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']' from tbl_master_contact where cnt_internalId=R.EmailRecipients_ContactLeadID) end as RecepientsName,R.EmailRecipients_RecipientEmailID as RecipientsEmailID,E.Emails_Subject as Subject,SUBSTRING( E.Emails_Content , 1 , 15 )+'...' as Content,R.EmailRecipients_RecipientType  as RecipientType,Convert(varchar(30),R.EmailRecipients_SentDateTime,100) as SentDate,case when R.EmailRecipients_Status='P'  then 'Pending' when R.EmailRecipients_Status='F'  then 'Fail' when R.EmailRecipients_Status='B'  then 'Bounced' when R.EmailRecipients_Status='X'  then 'Parmanent Failure' when R.EmailRecipients_Status='D'  then 'Delivered' else 'Pending'   END  as Status,R.EmailRecipients_AttemptNumber as AttempNo ", " E.Emails_ID=R.EmailRecipients_MainID  and  R.EmailRecipients_ContactLeadID='CLP0000302' and E.Emails_Subject like   '" + subject + "%' order by  E.Emails_ID Desc");
                if (dt.Rows.Count != 0)
                {

                    grdGeneralTrial.DataSource = dt;
                    grdGeneralTrial.DataBind();
                }
                else
                {

                    grdGeneralTrial.DataSource = dt;
                    grdGeneralTrial.DataBind();
                    lbltxt.Text = "No record found....";
                    lbltxt.Visible = true;
                }
                this.Page.ClientScript.RegisterStartupScript(GetType(), "script6", "<script>ShowEmployeeFilterForm('S');</script>");

            }


        }
        protected void btnGetReport_Click(object sender, EventArgs e)
        {
            fillGridSpecific();
        }
        protected void btnHide_Click(object sender, EventArgs e)
        {
            fillGrid();

            this.Page.ClientScript.RegisterStartupScript(GetType(), "script8", "<script>Pageload();</script>");
        }
    }

}