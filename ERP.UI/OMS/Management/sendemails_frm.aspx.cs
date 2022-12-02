using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using BusinessLogicLayer;

namespace ERP.OMS.Management
{
    public partial class Reports_Sendemails_frm : System.Web.UI.Page
    {
        GetAutomaticMail mail = new GetAutomaticMail();
        BusinessLogicLayer.Converter c = new BusinessLogicLayer.Converter();
        string topicid, ContactID, str;
        DataSet ds = new DataSet();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        Utilities obj = new Utilities();

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {

                DataTable dt2 = new DataTable();
                dt2.Columns.Add("filepathServer", System.Type.GetType("System.String"));
                string sBody = getemail();
                string str2 = Request.QueryString["subject"].ToString();
                string str1 = Request.QueryString["senderid"].ToString();
                //string str3 = ConfigurationSettings.AppSettings["FromUserName"].ToString();

                if (sBody != null && sBody != "a" && sBody != "")
                {

                    if (HttpContext.Current.Session["mail"] != null)
                    {
                        try
                        {
                            HttpContext.Current.Session["mailpath"] = HttpContext.Current.Session["mailpath"].ToString().Remove(HttpContext.Current.Session["mailpath"].ToString().Length - 1, 1);


                            string[] mailpath = HttpContext.Current.Session["mailpath"].ToString().Split('~');

                            for (int g = 0; g < mailpath.Length; g++)
                            {
                                dt2.Rows.Add(mailpath[g]);

                            }
                            oDBEngine.SendattachmentMail(str1, ConfigurationSettings.AppSettings["CredentialUserName"].ToString(), dt2, str2, "", "", "", "");

                            for (int i = 0; i < dt2.Rows.Count; i++)
                            {

                                if (File.Exists(dt2.Rows[i][0].ToString()))
                                {
                                    File.Delete(dt2.Rows[i][0].ToString());
                                }
                            }
                            mail.insert_notification_log(ViewState["topiccode"].ToString(), "R", ViewState["RequestID"].ToString(), mail.contactidnew1, Request.QueryString["senderid"].ToString(), "S", ds.Tables[2].Rows[0][0].ToString(), "s");

                            Response.Write("Send Successfully");
                        }
                        catch
                        {
                            mail.insert_notification_log(ViewState["topiccode"].ToString(), "R", ViewState["RequestID"].ToString(), mail.contactidnew1, Request.QueryString["senderid"].ToString(), "F", ds.Tables[2].Rows[0][0].ToString(), "B");//B-blank
                            Response.Write("Do not have any value in the segment");
                        }
                    }
                    else
                    {
                        DataTable dtCFG = oDBEngine.GetDataTable("CONFIG_EMAILACCOUNTS ", " TOP 1 EMAILACCOUNTS_COMPANYID,EMAILACCOUNTS_EMAILID  ", " EMAILACCOUNTS_SEGMENTID=1 AND EMAILACCOUNTS_INUSE='Y'");


                        //   c.SendMailHtmlBody(ConfigurationSettings.AppSettings["CredentialUserName"].ToString(), Request.QueryString["senderid"].ToString(), Request.QueryString["subject"].ToString(), sBody);
                        //using (SqlConnection lcon = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))MULTI
                        using (SqlConnection lcon = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"])))
                        {
                            // SqlConnection lcon = new SqlConnection(con);
                            lcon.Open();
                            // using (SqlCommand lcmdEmplInsert = new SqlCommand("InsertTransEmail", lcon))
                            // {
                            //lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                            //lcmdEmplInsert.Parameters.AddWithValue("@Emails_SenderEmailID", dtCFG.Rows[0][1].ToString());
                            //lcmdEmplInsert.Parameters.AddWithValue("@Emails_Subject", Request.QueryString["subject"].ToString());
                            //lcmdEmplInsert.Parameters.AddWithValue("@Emails_Content", sBody);
                            //lcmdEmplInsert.Parameters.AddWithValue("@Emails_HasAttachement", "N");
                            //lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateApplication", "1");
                            //lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateUser", "275");
                            //lcmdEmplInsert.Parameters.AddWithValue("@Emails_Type", "N");
                            //lcmdEmplInsert.Parameters.AddWithValue("@Emails_CompanyID", dtCFG.Rows[0][0].ToString());
                            //lcmdEmplInsert.Parameters.AddWithValue("@Emails_Segment", "CRM");
                            //SqlParameter parameter = new SqlParameter("@result", SqlDbType.BigInt);
                            //parameter.Direction = ParameterDirection.Output;
                            //lcmdEmplInsert.Parameters.Add(parameter);
                            //lcmdEmplInsert.ExecuteNonQuery();
                            //string InternalID = parameter.Value.ToString();
                            //  ###########---recipients-----------------                   

                            //DataTable dt1 = oDBEngine.GetDataTable(" tbl_master_email ", "top 1 *", "eml_cntId='" + contactid + "' and eml_type='Official'");
                            //string mailid = dt1.Rows[0]["eml_email"].ToString();

                            string InternalID = obj.InsertTransEmail(dtCFG.Rows[0][1].ToString(), Request.QueryString["subject"].ToString(), sBody,
                                "N", "1", "275", "N", dtCFG.Rows[0][0].ToString(), "CRM");

                            string fValues3 = "'" + InternalID + "','" + ViewState["ContactID"].ToString() + "','" + Request.QueryString["senderid"].ToString() + "','TO','" + DateTime.Now.ToString() + "','" + "P" + "'";
                            oDBEngine.InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status", fValues3);
                            //lcon.Close();
                            //lcon.Dispose();
                            // }
                        }
                        mail.insert_notification_log(ViewState["topiccode"].ToString(), "R", ViewState["RequestID"].ToString(), mail.contactidnew1, Request.QueryString["senderid"].ToString(), "S", ds.Tables[2].Rows[0][0].ToString(), "s");//s-success



                        Response.Write("Send Successfully");
                    }

                }
                else
                {
                    mail.insert_notification_log(ViewState["topiccode"].ToString(), "R", ViewState["RequestID"].ToString(), mail.contactidnew1, Request.QueryString["senderid"].ToString(), "F", ds.Tables[2].Rows[0][0].ToString(), "N");//N-no subscription
                    Response.Write("Do not have any value in the segment ");
                }

            }
            catch
            {

                DataTable dt2 = new DataTable();
                dt2.Columns.Add("filepathServer", System.Type.GetType("System.String"));
                HttpContext.Current.Session["mailpath"] = HttpContext.Current.Session["mailpath"].ToString().Remove(HttpContext.Current.Session["mailpath"].ToString().Length - 1, 1);
                string[] mailpath = HttpContext.Current.Session["mailpath"].ToString().Split('~');

                for (int g = 0; g < mailpath.Length; g++)
                {
                    dt2.Rows.Add(mailpath[g]);

                }
                mail.insert_notification_log(ViewState["topiccode"].ToString(), "R", ViewState["RequestID"].ToString(), mail.contactidnew1, Request.QueryString["senderid"].ToString(), "F", ds.Tables[2].Rows[0][0].ToString(), "N");
                Response.Write("   Can not send");
                if (HttpContext.Current.Session["mail"] != null)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {

                        if (File.Exists(dt2.Rows[i][0].ToString()))
                        {
                            File.Delete(dt2.Rows[i][0].ToString());
                        }
                    }
                }
            }


        }

        private string getemail()
        {
            try
            {
                ds.Reset();
                ds = mail.fetch_status_email(Request.QueryString["senderid"].ToString(), getwords(Request.QueryString["subject"].ToString()));
                string contactid = "";

                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    contactid = contactid + ds.Tables[1].Rows[i][0].ToString() + '|';

                }

                contactid = contactid.Remove(contactid.Length - 1, 1);

                contactid = contactid.Trim();
                ViewState["ContactID"] = ds.Tables[1].Rows[0][0].ToString();

                string s = "";
                switch (ds.Tables[0].Rows[0][0].ToString())
                {

                    case "CLNT000004":
                        try
                        {

                            s = mail.cdslHoldingFrmEmail(contactid, getwords(Request.QueryString["subject"].ToString()));
                        }
                        catch
                        {
                            s = null;
                        }
                        break;
                    case "CLNT000002":
                        try
                        {

                            s = mail.GetTradeRegister(contactid, getwords(Request.QueryString["subject"].ToString()));

                        }
                        catch
                        {
                            s = null;
                        }
                        break;

                    case "CLNT000003":
                        try
                        {

                            s = mail.GetNetPosition(contactid, getwords(Request.QueryString["subject"].ToString()));

                        }
                        catch
                        {
                            s = null;
                        }
                        break;
                    case "CLNT000015":
                        try
                        {

                            s = mail.GetCollateralReport(contactid, getwords(Request.QueryString["subject"].ToString()));

                        }
                        catch
                        {
                            s = null;
                        }
                        break;

                    case "CLNT000035":
                        try
                        {

                            s = mail.GetNetWorth(contactid, getwords(Request.QueryString["subject"].ToString()));

                        }
                        catch
                        {
                            s = null;
                        }
                        break;
                    case "CLNT000017":
                        try
                        {

                            s = mail.GetPendingDelivery(contactid, getwords(Request.QueryString["subject"].ToString()));

                        }
                        catch
                        {
                            s = null;
                        }
                        break;
                    case "CLNT000008":
                        try
                        {

                            s = mail.GetLedgerWithObligationBreakup(contactid, getwords(Request.QueryString["subject"].ToString()));

                        }
                        catch
                        {
                            s = null;
                        }
                        break;
                    case "CLNT000019":
                        try
                        {

                            s = mail.cdslbillFrmEmail(contactid, getwords(Request.QueryString["subject"].ToString()));

                        }
                        catch
                        {
                            s = null;
                        }
                        break;
                    case "CLNT000005":
                        try
                        {

                            s = mail.cdsltransactionFrmEmail(contactid, getwords(Request.QueryString["subject"].ToString().ToUpper()));

                        }
                        catch
                        {
                            s = null;
                        }
                        break;
                    //case "CLNT000001":
                    //    try
                    //    {

                    //        s = mail.ContractNoteEmail(contactid, getwords(Request.QueryString["subject"].ToString()));

                    //    }
                    //    catch
                    //    {
                    //        s = null;
                    //    }
                    //    break;

                }
                ViewState["topiccode"] = ds.Tables[0].Rows[0][0];
                ViewState["RequestID"] = ds.Tables[2].Rows[0][0];
                return s;
            }
            catch
            {
                ViewState["topiccode"] = "";
                ViewState["RequestID"] = "";
                mail.insert_notification_log(ViewState["topiccode"].ToString(), "R", ViewState["RequestID"].ToString(), mail.contactidnew1, Request.QueryString["senderid"].ToString(), "F", ds.Tables[2].Rows[0][0].ToString(), "W");//W-wrong subject
                Response.Write("Can not send subject is invalid");
                return "a";
            }

        }

        public string[] getwords(string sub)
        {
            string str = sub;
            str = str.Trim();
            string[] str1 = str.Split(' ');
            string j = "";
            for (int i = 0; i < str1.Length; i++)
            {
                if (str1[i] != "")
                {
                    j = j + " " + str1[i];
                }
                else
                {
                    j = j;
                }


            }
            j = j.Trim();
            str1 = j.Split(' ');

            return str1;
        }

    }
}