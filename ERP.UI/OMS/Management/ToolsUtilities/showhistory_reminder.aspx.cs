using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
using BusinessLogicLayer;

namespace ERP.OMS.Management.ToolsUtilities
{
    public partial class management_utilities_showhistory_reminder : System.Web.UI.Page
    {
        //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        Utilities obj = new Utilities();
        protected string id = "";
        string[] id1 = new string[0];
        string convDateTime = "";
        string convDateTime2 = "";
        string convDateTime3 = "";
        string convDateTime4 = "";
        string final = "";
        string substr = "";
        string final2 = "";
        string substr2 = "";
        string final3 = "";
        string substr3 = "";
        string final4 = "";
        string substr4 = "";
        string send = "";
        DataTable DT = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            id = Request.QueryString["id"].ToString();
            id1 = id.Split('~');
            HiddenField1.Value = id.ToString();
            DataTable DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else Convert(Varchar,rem_attenddate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_attenddate, 100), 13, 7)  end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company ", "rem_id=" + id1[0] + "");
            for (int m = 0; m < DT.Rows.Count; m++)
            {
                final = "";
                substr = "";
                final2 = "";
                substr2 = "";
                final3 = "";
                substr3 = "";
                substr4 = "";

                convDateTime = DT.Rows[m]["StartDate"].ToString();
                convDateTime = convDateTime.Substring(0, 7) + convDateTime.Substring(9);
                substr = convDateTime.Substring(4, 2);
                convDateTime = convDateTime.Replace(substr, "");
                final = substr + " " + convDateTime;
                // DT.Rows[m]["StartDate"] = final;

                convDateTime4 = DT.Rows[m]["CreateDate"].ToString();
                convDateTime4 = convDateTime4.Substring(0, 7) + convDateTime4.Substring(9);
                DT.Rows[m]["CreateDate"] = convDateTime4;

                convDateTime2 = DT.Rows[m]["EndDate"].ToString();
                convDateTime2 = convDateTime2.Substring(0, 7) + convDateTime2.Substring(9);
                substr2 = convDateTime2.Substring(4, 2);
                convDateTime2 = convDateTime.Replace(substr2, "");
                final2 = substr2 + " " + convDateTime2;
                //DT.Rows[m]["EndDate"] = final2;

                convDateTime3 = DT.Rows[m]["attenddate"].ToString();
                convDateTime3 = convDateTime3.Substring(0, 7) + convDateTime3.Substring(9);
                substr3 = convDateTime3.Substring(4, 2);
                convDateTime3 = convDateTime3.Replace(substr3, "");
                final3 = substr3 + " " + convDateTime3;
                //DT.Rows[m]["attenddate"] = final3;
            }
            GridReminder.DataSource = DT.DefaultView;
            GridReminder.DataBind();
            DataTable dtshow = oDBEngine.GetDataTable("select Reminderremarks_content as content,(select USER_NAME from tbl_master_user where user_id=Reminderremarks_createuser) as name,convert (varchar (20),Reminderremarks_createdatetime,100) as time from Trans_Reminderremarks where Reminderremarks_mainid = " + id1[0] + " order by Reminderremarks_id desc ");
            //DataTable dtshow = oDBEngine.GetDataTable("select Reminderremarks_content as content,(select USER_NAME from tbl_master_user where user_id=Reminderremarks_createuser) as name,convert (varchar (20),Reminderremarks_createdatetime,100) as time from Trans_Reminderremarks where Reminderremarks_mainid = 6696 and  Reminderremarks_createdatetime>'2012-08-15'");
            for (int m = 0; m < dtshow.Rows.Count; m++)
            {
                final = "";
                substr = "";
                final2 = "";
                substr2 = "";
                convDateTime = dtshow.Rows[m]["time"].ToString();
                convDateTime = convDateTime.Substring(0, 7) + convDateTime.Substring(9);
                substr = convDateTime.Substring(4, 2);
                convDateTime = convDateTime.Replace(substr, "");
                final = substr + " " + convDateTime;
                dtshow.Rows[m]["time"] = final;

            }
            rptConversion.DataSource = dtshow;
            rptConversion.DataBind();
            if (!IsPostBack)
            {
                if (id1[1] == "Closed")
                {
                    tr_insert.Visible = false;
                    tr_btn.Visible = false;
                    btnAdd.Visible = false;
                }
                else
                {
                    tr_insert.Visible = false;
                    tr_btn.Visible = false;
                    btnAdd.Visible = true;
                    //tr_insert.Visible = true;
                    //tr_btn.Visible = true;
                }
            }
        }
        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }
        void divdisplay(string idparam)
        {
            String strHtml = String.Empty;
            int flag = 0;
            //DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(varchar(19), rem_startDate,  100) AS StartDate, Convert(varchar(19), rem_EndDate,  100) AS EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else convert(varchar(19),ISNULL(rem_attendDate,''),100) end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag ", " convert(varchar(10),rem_startDate,103) = convert(varchar(10),getdate(),103) and ((rem_createUser =" + HttpContext.Current.Session["userid"] + ") OR (rem_targetUser =" + HttpContext.Current.Session["userid"] + ") or (rem_inchargetargetuser =" + HttpContext.Current.Session["userid"] + ")) order by rem_startDate,rem_priority,rem_flag desc ");
            DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else Convert(Varchar,rem_attenddate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_attenddate, 100), 13, 7)  end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_Name )) + ''+' [ ' + cmp_onroleshortname + ' ] ' from tbl_master_company where cmp_id=rem_cmpid) as company ", " rem_id=" + idparam + "");
            strHtml = "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml += "<tr style=\"background-color:lavender ;text-align:center\">";
            strHtml += "<td align=\"left\" colspan=3><b>Task Updation Log</b></td></tr>";

            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"center\" ><b> Create By</b></td>";
            strHtml += "<td align=\"center\" ><b> Create Dt</b></td>";
            strHtml += "<td align=\"center\" ><b>Token #</b></td>";
            strHtml += "<td align=\"center\" ><b>Task For</b></td>";
            strHtml += "<td align=\"center\" ><b> Follow-up By</b></td>";
            strHtml += "<td align=\"center\" ><b> Comp</b></td>";
            strHtml += "<td align=\"center\" ><b>Subject</b></td>";
            strHtml += "<td align=\"center\" ><b> Task</b></td>";
            strHtml += "<td align=\"center\" ><b>Task Updates</b></td>";
            strHtml += "<td align=\"center\" ><b> Prty</b></td>";
            strHtml += "<td align=\"center\" ><b> Start By</b></td>";
            strHtml += "<td align=\"center\" ><b>Finish By</b></td>";
            strHtml += "<td align=\"center\" ><b> Status</b></td>";
            strHtml += "<td align=\"center\" ><b> Attend Dt</b></td>";

            strHtml += "<td align=\"center\" ><b> Type</b></td>";
            string count = DT.Rows.Count.ToString();
            flag = flag + 1;
            final = "";
            substr = "";
            final2 = "";
            substr2 = "";
            final3 = "";
            substr3 = "";
            string attnddt = "";
            convDateTime = DT.Rows[0]["StartDate"].ToString();
            convDateTime = convDateTime.Substring(0, 7) + convDateTime.Substring(9);
            substr = convDateTime.Substring(4, 2);
            convDateTime = convDateTime.Replace(substr, "");
            final = substr + " " + convDateTime;
            //DT.Rows[0]["StartDate"] = final;
            convDateTime2 = DT.Rows[0]["EndDate"].ToString();
            convDateTime2 = convDateTime2.Substring(0, 7) + convDateTime2.Substring(9);
            substr2 = convDateTime2.Substring(4, 2);
            convDateTime2 = convDateTime.Replace(substr2, "");
            final2 = substr2 + " " + convDateTime2;
            //DT.Rows[0]["EndDate"] = final2;
            attnddt = DT.Rows[0]["attenddate"].ToString().Trim();
            if (attnddt.Length > 0)
            //if ((DT.Rows[m]["attenddate"].ToString().Trim() != " ") || (DT.Rows[m]["attenddate"].ToString().Trim() != ""))
            {
                convDateTime3 = DT.Rows[0]["attenddate"].ToString();
                convDateTime3 = convDateTime3.Substring(0, 7) + convDateTime3.Substring(9);
                substr3 = convDateTime3.Substring(4, 2);
                convDateTime3 = convDateTime3.Replace(substr3, "");
                final3 = substr3 + " " + convDateTime3;
                //DT.Rows[0]["attenddate"] = final3;
            }
            else
            {
                DT.Rows[0]["attenddate"] = "";
            }
            strHtml += "<tr style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
            strHtml += "<td align=\"left\">" + DT.Rows[0]["CreateBy"].ToString() + "</td>";
            strHtml += "<td align=\"left\">" + DT.Rows[0]["CreateDate"].ToString() + "</td>";
            strHtml += "<td align=\"left\">" + DT.Rows[0]["Rid"].ToString() + "</td>";
            strHtml += "<td align=\"left\">" + DT.Rows[0]["Target"].ToString() + "</td>";
            strHtml += "<td align=\"left\">" + DT.Rows[0]["incharge"].ToString() + "</td>";

            strHtml += "<td align=\"left\">" + DT.Rows[0]["company"].ToString() + "</td>";
            strHtml += "<td align=\"left\">" + DT.Rows[0]["shortname"].ToString() + "</td>";
            strHtml += "<td align=\"left\">" + DT.Rows[0]["Content"].ToString() + "</td>";
            strHtml += "<td align=\"left\">" + DT.Rows[0]["replycontent"].ToString() + "</td>";
            strHtml += "<td align=\"left\">" + DT.Rows[0]["priority"].ToString() + "</td>";


            strHtml += "<td align=\"left\">" + DT.Rows[0]["StartDate"].ToString() + "</td>";
            strHtml += "<td align=\"left\">" + DT.Rows[0]["EndDate"].ToString() + "</td>";
            strHtml += "<td align=\"left\">" + DT.Rows[0]["Status"].ToString() + "</td>";
            strHtml += "<td align=\"left\">" + DT.Rows[0]["attenddate"].ToString() + "</td>";

            strHtml += "<td align=\"left\">" + DT.Rows[0]["flag"].ToString() + "</td>";
            //strHtml += "<td align=\"left\">" + count + "</td>";
            strHtml += "</tr>";
            strHtml += "</table>";
            //}
            DataTable dtlog = oDBEngine.GetDataTable("select ROW_NUMBER() over (ORDER BY Reminderremarks_id desc) as srno,Reminderremarks_content as content,(select USER_NAME from tbl_master_user where user_id=Reminderremarks_createuser) as name,convert (varchar (20),Reminderremarks_createdatetime,100) as time from Trans_Reminderremarks where Reminderremarks_mainid = " + idparam + " order by Reminderremarks_id desc ");
            if (dtlog.Rows.Count > 0)
            {
                strHtml += "<table width=\"100%\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtml += "<tr style=\"background-color:lavender ;text-align:center\">";
                strHtml += "<td align=\"left\" colspan=2><b>Log Details</b></td></tr>";
                strHtml += "<tr style=\"background-color: #DBEEF3;\">";
                strHtml += "<td align=\"center\" width=\"5%\"><b> Sr No.</b></td>";
                strHtml += "<td align=\"center\" width=\"15%\" ><b> Name</b></td>";
                strHtml += "<td align=\"center\" ><b>Content</b></td>";
                strHtml += "<td align=\"right\" width=\"15%\"><b> Time</b></td>";
                for (int p = 0; p < dtlog.Rows.Count; p++)
                {
                    final = "";
                    substr = "";
                    final2 = "";
                    substr2 = "";
                    convDateTime = dtlog.Rows[p]["time"].ToString();
                    convDateTime = convDateTime.Substring(0, 7) + convDateTime.Substring(9);
                    substr = convDateTime.Substring(4, 2);
                    convDateTime = convDateTime.Replace(substr, "");
                    final = substr + " " + convDateTime;
                    dtlog.Rows[p]["time"] = final;
                    strHtml += "<tr style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                    strHtml += "<td align=\"left\">" + dtlog.Rows[p]["srno"].ToString() + "</td>";
                    strHtml += "<td align=\"left\">" + dtlog.Rows[p]["name"].ToString() + "</td>";
                    strHtml += "<td align=\"left\">" + dtlog.Rows[p]["content"].ToString() + "</td>";
                    strHtml += "<td align=\"left\">" + dtlog.Rows[p]["time"].ToString() + "</td>";

                }
                strHtml += "</table>";
            }
            //else
            //{
            //    strHtml += "</table>";
            //}

            //display.InnerHtml = strHtml;
            send = strHtml;
            //ViewState["mail"] = strHtml;
            //return MailSend(strHtml.ToString().Trim());
        }
        void SendReport(string emailbdy, string contactid, string contactidtarget, string contactidincharge, DateTime billdate, string Subject)
        {
            //   DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            string atchflile = "N";
            string sPath = HttpContext.Current.Request.Url.ToString();
            string[] PageName = sPath.ToString().Split('/');
            DataTable dt = oDBEngine.GetDataTable(" tbl_trans_menu ", "mnu_id ", " mnu_menuLink like '%" + PageName[5] + "%' and mnu_segmentid='" + HttpContext.Current.Session["userlastsegment"] + "'");
            string menuId = "352";
            string segmentname = "CRM";
            string contactlead = "";
            DataTable dt3 = new DataTable();
            DataTable dtlead = oDBEngine.GetDataTable("select user_contactid from tbl_master_user where user_id = '" + contactid + "'");


            if (dtlead != null && dtlead.Rows.Count > 0)
            {
                contactlead = Convert.ToString(dtlead.Rows[0]["user_contactid"]);
            }
            //if (dt.Rows.Count != 0)
            //{
            //    menuId = dt.Rows[0]["mnu_id"].ToString();

            //}
            //else
            //{
            //    menuId = "";
            //}
            //try
            //{
            DataTable dt1 = oDBEngine.GetDataTable(" tbl_master_email ", "top 1 *", "eml_cntId= (select user_contactid from tbl_master_user where user_id = '" + contactid + "')");
            DataTable dt2 = oDBEngine.GetDataTable(" tbl_master_email ", "top 1 *", "eml_cntId= (select user_contactid from tbl_master_user where user_id = '" + contactidtarget + "')");
            if (contactidincharge != "")
            {
                dt3 = oDBEngine.GetDataTable(" tbl_master_email ", "top 1 *", "eml_cntId= (select user_contactid from tbl_master_user where user_id = '" + contactidincharge + "')");
            }
            string mailid = "";
            string target = "";
            string incharge = "";
            if (dt1 != null && dt1.Rows.Count > 0)
            {

                mailid = Convert.ToString(dt1.Rows[0]["eml_email"]);

            }
            if (dt2 != null && dt2.Rows.Count > 0)
            {
                target = Convert.ToString(dt2.Rows[0]["eml_email"]);
            }
            if (dt3 != null && dt3.Rows.Count > 0)
            {
                incharge = Convert.ToString(dt3.Rows[0]["eml_email"]);
            }
            if (mailid != "")
            {

                string contactID = HttpContext.Current.Session["usercontactID"].ToString();
                DataTable dtcmp = oDBEngine.GetDataTable(" tbl_master_company  ", "*", "cmp_id=(select emp_organization from tbl_trans_employeectc where emp_cntId='" + contactID + "' and emp_effectiveuntil is null)");
                string cmpintid = dtcmp.Rows[0]["cmp_internalid"].ToString();
                //DataTable dtsg = oDBEngine.GetDataTable(" tbl_master_segment  ", "*", "seg_id='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'");
                //string segmentname = dtsg.Rows[0]["seg_name"].ToString();

                DataTable dtname = oDBEngine.GetDataTable(" tbl_master_Contact  ", "(isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']') as ClientName", "cnt_internalId=(select user_contactid from tbl_master_user where user_id = '" + contactid + "') ");
                string ClientName = dtname.Rows[0]["ClientName"].ToString();

                string senderemail = "";
                string[,] data = oDBEngine.GetFieldValue(" tbl_master_user,tbl_master_email ", " eml_email  AS EmployId", " user_contactId=eml_cntId and eml_type='Official' and user_id = " + HttpContext.Current.Session["userid"], 1);
                if (data[0, 0] != "n")
                {
                    senderemail = data[0, 0];

                }

                //  String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                //  SqlConnection lcon = new SqlConnection(con);
                //using (SqlConnection lcon = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
                //{
                //    lcon.Open();
                //    SqlCommand lcmdEmplInsert = new SqlCommand("InsertTransEmail", lcon);
                //    lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_SenderEmailID", senderemail);
                //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_Subject", Subject);
                //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_Content", "<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Name:" + ClientName + "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">" + Subject + "</td></tr> <tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Segment: " + segmentname + "</td></tr><tr><td>" + emailbdy + "</td></tr></table>");
                //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_HasAttachement", atchflile);
                //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateApplication", menuId);
                //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateUser", HttpContext.Current.Session["userid"]);
                //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_Type", "N");
                //    if (HttpContext.Current.Session["LastCompany"].ToString() != "")
                //    {
                //        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
                //    }
                //    else
                //    {
                //        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CompanyID", cmpintid.ToString().Trim());
                //    }
                //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_Segment", segmentname);
                //    SqlParameter parameter = new SqlParameter("@result", SqlDbType.BigInt);
                //    parameter.Direction = ParameterDirection.Output;
                //    lcmdEmplInsert.Parameters.Add(parameter);
                //    lcmdEmplInsert.ExecuteNonQuery();
                //    string InternalID = parameter.Value.ToString();
                //    //  ###########---recipients-----------------                   

                //    //DataTable dt1 = oDBEngine.GetDataTable(" tbl_master_email ", "top 1 *", "eml_cntId='" + contactid + "' and eml_type='Official'");
                //    //string mailid = dt1.Rows[0]["eml_email"].ToString();

                //    string fValues3 = "'" + InternalID + "','" + contactlead + "','" + mailid + "','TO','" + billdate + "','" + "P" + "','"+ menuId +"'";
                //    oDBEngine.InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status,EmailRecipients_ActivityID", fValues3);
                //}
                string EmailContent = "<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Name:" + ClientName + "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">" + Subject + "</td></tr> <tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Segment: " + segmentname + "</td></tr><tr><td>" + emailbdy + "</td></tr></table>";
                string InternalID = obj.InsertTransEmail(senderemail, Subject, EmailContent, atchflile, menuId, HttpContext.Current.Session["userid"].ToString(),
                    "N", HttpContext.Current.Session["LastCompany"].ToString() != "" ? HttpContext.Current.Session["LastCompany"].ToString() : cmpintid.ToString().Trim(),
                    segmentname);

                string fValues3 = "'" + InternalID + "','" + contactlead + "','" + mailid + "','TO','" + billdate + "','" + "P" + "','" + menuId + "'";
                oDBEngine.InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status,EmailRecipients_ActivityID", fValues3);

            }
            if (target != "")
            {

                string contactID = HttpContext.Current.Session["usercontactID"].ToString();
                DataTable dtcmp = oDBEngine.GetDataTable(" tbl_master_company  ", "*", "cmp_id=(select emp_organization from tbl_trans_employeectc where emp_cntId='" + contactID + "' and emp_effectiveuntil is null)");
                string cmpintid = dtcmp.Rows[0]["cmp_internalid"].ToString();
                //DataTable dtsg = oDBEngine.GetDataTable(" tbl_master_segment  ", "*", "seg_id='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'");
                //string segmentname = dtsg.Rows[0]["seg_name"].ToString();

                DataTable dtname = oDBEngine.GetDataTable(" tbl_master_Contact  ", "(isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']') as ClientName", "cnt_internalId=(select user_contactid from tbl_master_user where user_id = '" + contactidtarget + "') ");
                string ClientName = dtname.Rows[0]["ClientName"].ToString();

                string senderemail = "";
                string[,] data = oDBEngine.GetFieldValue(" tbl_master_user,tbl_master_email ", " eml_email  AS EmployId", " user_contactId=eml_cntId and eml_type='Official' and user_id = " + HttpContext.Current.Session["userid"], 1);
                if (data[0, 0] != "n")
                {
                    senderemail = data[0, 0];

                }

                //  String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                //  SqlConnection lcon = new SqlConnection(con);
                //using (SqlConnection lcon = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
                //{
                //    lcon.Open();
                //    SqlCommand lcmdEmplInsert = new SqlCommand("InsertTransEmail", lcon);
                //    lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_SenderEmailID", senderemail);
                //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_Subject", Subject);
                //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_Content", "<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Name:" + ClientName + "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">" + Subject + "</td></tr> <tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Segment: " + segmentname + "</td></tr><tr><td>" + emailbdy + "</td></tr></table>");
                //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_HasAttachement", atchflile);
                //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateApplication", menuId);
                //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateUser", HttpContext.Current.Session["userid"]);
                //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_Type", "N");
                //    if (HttpContext.Current.Session["LastCompany"].ToString() != "")
                //    {
                //        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
                //    }
                //    else
                //    {
                //        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CompanyID", cmpintid.ToString().Trim());
                //    }
                //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_Segment", segmentname);
                //    SqlParameter parameter = new SqlParameter("@result", SqlDbType.BigInt);
                //    parameter.Direction = ParameterDirection.Output;
                //    lcmdEmplInsert.Parameters.Add(parameter);
                //    lcmdEmplInsert.ExecuteNonQuery();
                //    string InternalID = parameter.Value.ToString();
                //    //  ###########---recipients-----------------                   

                //    //DataTable dt1 = oDBEngine.GetDataTable(" tbl_master_email ", "top 1 *", "eml_cntId='" + contactid + "' and eml_type='Official'");
                //    //string mailid = dt1.Rows[0]["eml_email"].ToString();

                //    string fValues3 = "'" + InternalID + "','" + contactlead + "','" + target + "','TO','" + billdate + "','" + "P" + "','"+ menuId +"'";
                //    oDBEngine.InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status,EmailRecipients_ActivityID", fValues3);
                //}
                string EmailContent = "<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Name:" + ClientName + "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">" + Subject + "</td></tr> <tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Segment: " + segmentname + "</td></tr><tr><td>" + emailbdy + "</td></tr></table>";
                string InternalID = obj.InsertTransEmail(senderemail, Subject, EmailContent, atchflile, menuId, HttpContext.Current.Session["userid"].ToString(),
                    "N", HttpContext.Current.Session["LastCompany"].ToString() != "" ? HttpContext.Current.Session["LastCompany"].ToString() : cmpintid.ToString().Trim(),
                    segmentname);

                string fValues3 = "'" + InternalID + "','" + contactlead + "','" + mailid + "','TO','" + billdate + "','" + "P" + "','" + menuId + "'";
                oDBEngine.InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status,EmailRecipients_ActivityID", fValues3);

            }
            if (incharge != "")
            {

                string contactID = HttpContext.Current.Session["usercontactID"].ToString();
                DataTable dtcmp = oDBEngine.GetDataTable(" tbl_master_company  ", "*", "cmp_id=(select emp_organization from tbl_trans_employeectc where emp_cntId='" + contactID + "' and emp_effectiveuntil is null)");
                string cmpintid = dtcmp.Rows[0]["cmp_internalid"].ToString();
                //DataTable dtsg = oDBEngine.GetDataTable(" tbl_master_segment  ", "*", "seg_id='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'");
                //string segmentname = dtsg.Rows[0]["seg_name"].ToString();

                DataTable dtname = oDBEngine.GetDataTable(" tbl_master_Contact  ", "(isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']') as ClientName", "cnt_internalId=(select user_contactid from tbl_master_user where user_id = '" + contactidincharge + "') ");
                string ClientName = dtname.Rows[0]["ClientName"].ToString();
                //string ClientName = "";

                string senderemail = "";
                string[,] data = oDBEngine.GetFieldValue(" tbl_master_user,tbl_master_email ", " eml_email  AS EmployId", " user_contactId=eml_cntId and eml_type='Official' and user_id = " + HttpContext.Current.Session["userid"], 1);
                if (data[0, 0] != "n")
                {
                    senderemail = data[0, 0];

                }

                //  String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];
                //  SqlConnection lcon = new SqlConnection(con);
                //using (SqlConnection lcon = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
                //{
                //    lcon.Open();
                //    SqlCommand lcmdEmplInsert = new SqlCommand("InsertTransEmail", lcon);
                //    lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_SenderEmailID", senderemail);
                //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_Subject", Subject);
                //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_Content", "<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Name:" + ClientName + "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">" + Subject + "</td></tr> <tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Segment: " + segmentname + "</td></tr><tr><td>" + emailbdy + "</td></tr></table>");
                //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_HasAttachement", atchflile);
                //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateApplication", menuId);
                //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateUser", HttpContext.Current.Session["userid"]);
                //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_Type", "N");
                //    if (HttpContext.Current.Session["LastCompany"].ToString() != "")
                //    {
                //        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
                //    }
                //    else
                //    {
                //        lcmdEmplInsert.Parameters.AddWithValue("@Emails_CompanyID", cmpintid.ToString().Trim());
                //    }
                //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_Segment", segmentname);
                //    SqlParameter parameter = new SqlParameter("@result", SqlDbType.BigInt);
                //    parameter.Direction = ParameterDirection.Output;
                //    lcmdEmplInsert.Parameters.Add(parameter);
                //    lcmdEmplInsert.ExecuteNonQuery();
                //    string InternalID = parameter.Value.ToString();
                //    //  ###########---recipients-----------------                   

                //    //DataTable dt1 = oDBEngine.GetDataTable(" tbl_master_email ", "top 1 *", "eml_cntId='" + contactid + "' and eml_type='Official'");
                //    //string mailid = dt1.Rows[0]["eml_email"].ToString();

                //    string fValues3 = "'" + InternalID + "','" + contactlead + "','" + incharge + "','TO','" + billdate + "','" + "P" + "','"+ menuId +"'";
                //    oDBEngine.InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status,EmailRecipients_ActivityID", fValues3);
                //}
                string EmailContent = "<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Name:" + ClientName + "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">" + Subject + "</td></tr> <tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Segment: " + segmentname + "</td></tr><tr><td>" + emailbdy + "</td></tr></table>";
                string InternalID = obj.InsertTransEmail(senderemail, Subject, EmailContent, atchflile, menuId, HttpContext.Current.Session["userid"].ToString(),
                    "N", HttpContext.Current.Session["LastCompany"].ToString() != "" ? HttpContext.Current.Session["LastCompany"].ToString() : cmpintid.ToString().Trim(),
                    segmentname);

                string fValues3 = "'" + InternalID + "','" + contactlead + "','" + mailid + "','TO','" + billdate + "','" + "P" + "','" + menuId + "'";
                oDBEngine.InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status,EmailRecipients_ActivityID", fValues3);

            }
            //}



            //catch (Exception)
            //{
            //    return false;
            //}
            //return true;
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            tr_insert.Visible = true;
            tr_btn.Visible = true;
            btnAdd.Visible = false;

        }
        protected void btnYes_Click(object sender, EventArgs e)
        {
            string contactidcreate = "";
            string contactidtarget = "";
            string contactidincharge = "";
            string category = "";
            string text = txtReportTo.Text.ToString().Trim();
            int noinsert = 0;
            noinsert = oDBEngine.InsurtFieldValue("trans_Reminderremarks", "Reminderremarks_mainid,Reminderremarks_content,Reminderremarks_createuser,Reminderremarks_createdatetime", "'" + id1[0] + "','" + text.ToString().Trim() + "','" + HttpContext.Current.Session["userid"].ToString() + "','" + oDBEngine.GetDate().ToString() + "'");
            int insert = oDBEngine.SetFieldValue("tbl_trans_reminder", "rem_replycontent='" + text.ToString().Trim() + "',rem_attendDate='" + oDBEngine.GetDate() + "' ", "rem_id=" + id1[0] + "");
            divdisplay(id1[0]);
            DataTable dtcontact = oDBEngine.GetDataTable("select rem_createUser,rem_targetUser,rem_inchargetargetuser,(select Remindercategory_shortname from Master_Remindercategory where Remindercategory_id=rem_categoryid) as category from tbl_trans_reminder where rem_id='" + id1[0] + "'");
            contactidcreate = dtcontact.Rows[0]["rem_createUser"].ToString();
            contactidtarget = dtcontact.Rows[0]["rem_targetUser"].ToString();
            contactidincharge = dtcontact.Rows[0]["rem_inchargetargetuser"].ToString();
            category = dtcontact.Rows[0]["category"].ToString();
            DateTime dt = oDBEngine.GetDate();
            SendReport(send.ToString().Trim(), contactidcreate, contactidtarget, contactidincharge, Convert.ToDateTime(dt.ToString("yyyy-MM-dd")), "Task # " + id1[0] + " :Task Updation Intimation [" + category + "]");
            string p1 = id1[0] + "/" + "0" + "/" + "1";
            string popUpscript = "";
            popUpscript = "<script language='javascript'>window.opener.PopulateGrid('" + p1 + "');window.close();</script>";
            ClientScript.RegisterStartupScript(GetType(), "JScript", popUpscript);
            string popUpscript1 = "";
            popUpscript1 = "alert('Successfully Saved'); parent.editwin.close();";
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, popUpscript1, true);
        }
        protected void btnNo_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "JScript4", "<script>parent.editwin.close();</script>");
        }
        protected void btnprint_Click(object sender, EventArgs e)
        {
            //DataTable dsprint = new DataTable();
            //dsprint = oDBEngine.GetDataTable("select ROW_NUMBER() over (ORDER BY Reminderremarks_id) as srno  ,Reminderremarks_content as content,(select USER_NAME from tbl_master_user where user_id=Reminderremarks_createuser) as name,Reminderremarks_createdatetime as time from Trans_Reminderremarks where Reminderremarks_mainid = " + id1[0] + " ");
            DataSet dsCrystal = new DataSet();
            //dsprint.TableName = "dtprint";
            //String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
            String conn = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlConnection con = new SqlConnection(conn);
            SqlCommand cmd1 = new SqlCommand("Reminderdatafetch", con);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@id", id1[0]);
            //cmd1.Parameters.AddWithValue("@id", "6696");
            cmd1.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd1;
            Adap.Fill(dsCrystal);
            cmd1.Dispose();
            con.Dispose();
            GC.Collect();

            //dsCrystal.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"].ToString().Trim() + "//Reports//remindercontect.xsd");
            string[] connPath = ConfigurationManager.AppSettings["DBConnectionDefault"].Split(';');
            ReportDocument reportObj = new ReportDocument();
            string ReportPath = Server.MapPath("..\\management\\reminder.rpt");
            reportObj.Load(ReportPath);
            reportObj.SetDatabaseLogon(connPath[2].Substring(connPath[2].IndexOf("=")).Trim(), connPath[3].Substring(connPath[3].IndexOf("=")).Trim(), connPath[0].Substring(connPath[0].IndexOf("=")).Trim(), connPath[1].Substring(connPath[1].IndexOf("=")).Trim(), false);
            reportObj.SetDataSource(dsCrystal.Tables[0]);
            reportObj.Subreports["reminderdetails"].SetDataSource(dsCrystal.Tables[1]);
            reportObj.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "Reminder Conversation");
            reportObj.Dispose();
            GC.Collect();
        }

    }
}