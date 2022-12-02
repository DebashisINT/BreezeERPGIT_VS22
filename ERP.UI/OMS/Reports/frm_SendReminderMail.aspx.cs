using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace ERP.OMS.Reports
{
    public partial class Reports_frm_SendReminderMail : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AspxFromdate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                AspxFromdate.EditFormatString = objConverter.GetDateFormat("Date");
                AspxTodate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                AspxTodate.EditFormatString = objConverter.GetDateFormat("Date");
            }
            Page.ClientScript.RegisterStartupScript(GetType(), "CallingHeight", "<script language='JavaScript'>height();</script>");

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            //grdMainGrid.Columns[2].Visible = false;
            BindGrid();
        }
        private void BindGrid()
        {
            DataTable dtMain = new DataTable();
            dtMain = oDBEngine.GetDataTable("tbl_trans_InsMain as a inner join tbl_trans_InsDetail as b on a.trn_transno=b.trn_transno inner join  tbl_master_contact as c on c.cnt_internalid=a.trn_contactid ", "distinct  a.trn_scheme,isnull(c.cnt_firstname,'')+isnull(c.cnt_middlename,'')+isnull(c.cnt_lastname,'') as Name,c.cnt_branchid as BranchID,a.trn_transno,a.trn_transdate,a.trn_contactid as id,convert(varchar(11),cast(b.trn_paymentdate as datetime),106)as PaymentDate,b.trn_paymentdate,convert(varchar(11),cast(trn_issuedate as datetime),106)as IssueDate,a.trn_policyno,a.trn_premiumamt,(select Rep_partnerid from tbl_trans_contactInfo where tbl_trans_contactInfo.cnt_internalid=a.trn_contactid)as spoc,(select cnt_referedby from tbl_master_contact where tbl_master_contact.cnt_internalid=a.trn_contactid )as referedby,(select prds_description from tbl_master_products where prds_internalID=a.trn_scheme )as product", "cast(b.trn_paymentdate as datetime) >='" + AspxFromdate.Value.ToString() + "' and cast(b.trn_paymentdate as datetime)<='" + AspxTodate.Value.ToString() + "' and trn_payrecdate is null and c.cnt_branchid in (" + HttpContext.Current.Session["userbranchHierarchy"] + ")", "a.trn_contactid");
            grdMainGrid.DataSource = dtMain;
            grdMainGrid.DataBind();
        }
        protected void btnSendMail_Click(object sender, EventArgs e)
        {
            string CustomerID = "";
            for (int i = 0; i < grdMainGrid.Rows.Count; i++)
            {
                CheckBox chk = (CheckBox)grdMainGrid.Rows[i].FindControl("chkSel");
                string UserId = oDBEngine.GetFieldValue("tbl_master_user", "user_contactid", "user_id='" + HttpContext.Current.Session["userid"].ToString().Trim() + "'", 1)[0, 0];
                DataTable SenderEmailID = oDBEngine.GetDataTable("tbl_master_email", "eml_email,eml_cntid", "eml_type='Official' and eml_cntid='" + UserId.ToString() + "'");
                if (chk.Checked == true)
                {
                    Label lbl = (Label)grdMainGrid.Rows[i].FindControl("lblId");
                    CustomerID = "" + lbl.Text.ToString() + "";
                    DataTable EmailidR = oDBEngine.GetDataTable("tbl_master_email", "distinct eml_email,eml_cntid", "eml_type='Official' and eml_cntid ='" + CustomerID.ToString().Trim() + "'");
                    if (EmailidR.Rows.Count > 0)
                    {
                        Label lblP = (Label)grdMainGrid.Rows[i].FindControl("lblPdate");
                        Label lblPNO = (Label)grdMainGrid.Rows[i].FindControl("lblPolicyNo");
                        Label lblPolicyAmt = (Label)grdMainGrid.Rows[i].FindControl("lblPolicyNo");
                        Label lblProductID = (Label)grdMainGrid.Rows[i].FindControl("lblPID");
                        string ActivityId = "PD-" + lblProductID.Text.ToString();
                        DateTime SendDateTime = Convert.ToDateTime(lblP.Text.ToString());
                        SendDateTime = Convert.ToDateTime(SendDateTime.AddDays(-1));
                        DataTable dataExit = oDBEngine.GetDataTable("Trans_EmailRecipients", "emailrecipients_contactleadid", "emailrecipients_contactleadid='" + CustomerID.ToString().Trim() + "' and emailrecipients_activityid='" + ActivityId.ToString().Trim() + "' and emailrecipients_senddatetime='" + SendDateTime.ToString().Trim() + "'");
                        if (dataExit.Rows.Count == 0)
                        {
                            string EmailContent = "Dear customer your premimum due date is on " + lblP.Text.ToString() + "Policy details:-Policy No " + lblPNO.Text.ToString() + "Amount " + lblPolicyAmt.Text.ToString();
                            //oDBEngine.InsurtFieldValue("trans_emails", "Emails_SenderEmailID, Emails_Subject, Emails_Content, Emails_HasAttachement, Emails_CreateApplication, Emails_CreateUser,Emails_CreateDateTime,  Emails_CompanyID, Emails_Segment", "'" + SenderEmailID.Rows[0][0].ToString() + "','Premium Due Reminder','" + EmailContent.ToString() + "','Y','480','" + Session["userid"].ToString() + "','" + oDBEngine.GetDate().ToString() + "','" + HttpContext.Current.Session["LastCompany"] + "','" + HttpContext.Current.Session["userlastsegment"].ToString() + "'");

                            string EmailSub = "Premium Due Reminder";
                            string atchflile = "N";
                            string menuId = "480";
                            string cmpintid = HttpContext.Current.Session["LastCompany"].ToString();
                            DataTable dtsg = oDBEngine.GetDataTable(" tbl_master_segment  ", "*", "seg_id='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'");
                            string segmentname = dtsg.Rows[0]["seg_name"].ToString();
                            String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                            SqlConnection lcon = new SqlConnection(con);
                            lcon.Open();
                            SqlCommand lcmdEmplInsert = new SqlCommand("InsertTransEmail", lcon);
                            lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                            lcmdEmplInsert.Parameters.AddWithValue("@Emails_SenderEmailID", SenderEmailID.Rows[0][0].ToString());
                            lcmdEmplInsert.Parameters.AddWithValue("@Emails_Subject", EmailSub);
                            lcmdEmplInsert.Parameters.AddWithValue("@Emails_Content", EmailContent);
                            lcmdEmplInsert.Parameters.AddWithValue("@Emails_HasAttachement", atchflile);
                            lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateApplication", menuId);
                            lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateUser", Session["userid"]);
                            lcmdEmplInsert.Parameters.AddWithValue("@Emails_Type", "S");
                            lcmdEmplInsert.Parameters.AddWithValue("@Emails_CompanyID", cmpintid);
                            lcmdEmplInsert.Parameters.AddWithValue("@Emails_Segment", segmentname);
                            SqlParameter parameter = new SqlParameter("@result", SqlDbType.BigInt);
                            parameter.Direction = ParameterDirection.Output;
                            lcmdEmplInsert.Parameters.Add(parameter);
                            lcmdEmplInsert.ExecuteNonQuery();
                            // Mantis Issue 24802
                            if (lcon.State == ConnectionState.Open)
                            {
                                lcon.Close();
                            }
                            // End of Mantis Issue 24802
                            string InternalID = parameter.Value.ToString();
                            oDBEngine.InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType, EmailRecipients_SendDateTime, EmailRecipients_Status,EmailRecipients_ActivityID", "'" + InternalID.ToString() + "','" + CustomerID.ToString() + "','" + EmailidR.Rows[0][0].ToString() + "','TO','" + SendDateTime.ToString() + "','P','" + ActivityId.ToString().Trim() + "'");
                        }
                    }
                }

            }



        }
        protected void grdMainGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdMainGrid.PageIndex = e.NewPageIndex;
            BindGrid();
        }
        //protected void grdMainGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.Header)
        //    {
        //        ((CheckBox)e.Row.FindControl("cbSelectAll")).Attributes.Add("onclick", "javascript:SelectAllInterSegment('" + ((CheckBox)e.Row.FindControl("cbSelectAll")).ClientID + "')");
        //    }
        //}
    }
}