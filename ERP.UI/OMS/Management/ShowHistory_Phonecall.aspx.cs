using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Management
{
    public partial class management_Activities_ShowHistory_Phonecall : System.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        public string pageAccess = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            ShowHistory();
        }
        public void ShowHistory()
        {

            if (Request.QueryString["id1"] != null)
            {
                string Id = Convert.ToString(Request.QueryString["id1"]);
                BindHistory(Id);
            }
            else
            {
                if (Session["InternalId"] != null)
                {
                    string Id = Convert.ToString(Session["InternalId"]);
                    BindHistory(Id);
                }
            }
        }
        public void BindHistory(string Id)
        {
            string lcid = string.Empty;
            if (lcid!=null && lcid.Length > 2)
            { lcid = Id.Substring(0, 2); }
          
            string[,] Name;
            if (lcid == "LD")
            {
                Name = oDBEngine.GetFieldValue("tbl_master_lead", "isnull(cnt_firstname,'') + ' ' + isnull(cnt_middlename,'') + ' ' + isnull(cnt_lastname,'') as Name", "cnt_internalid='" + Id + "'", 1);
            }
            else
            {
                Name = oDBEngine.GetFieldValue("tbl_master_contact", "isnull(cnt_firstname,'') + ' ' + isnull(cnt_middlename,'') + ' ' + isnull(cnt_lastname,'') as Name", "cnt_internalid='" + Id + "'", 1);
            }
            if (Name[0, 0] != "n")
            {
                lblName.Text = "Lead Name : " + Name[0, 0];
            }
            string[,] createUser1;
            if (lcid == "LD")
            {
                createUser1 = oDBEngine.GetFieldValue("tbl_master_user inner join tbl_master_lead on tbl_master_user.user_id=tbl_master_lead.createuser", "user_name", "cnt_internalid='" + Id + "'", 1);
            }
            else
            {
                createUser1 = oDBEngine.GetFieldValue("tbl_master_user inner join tbl_master_contact on tbl_master_user.user_id=tbl_master_contact.createuser", "user_name", "cnt_internalid='" + Id + "'", 1);
            }
            string createUser = "";
            if (createUser1[0, 0] != "n")
            {
                createUser = createUser1[0, 0];
            }
            string[,] createDate1;
            if (lcid == "LD")
            {
                createDate1 = oDBEngine.GetFieldValue("tbl_master_lead", "(convert(varchar(11),createdate,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), createdate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), createdate, 22), 3))) as createdate", "cnt_internalid='" + Id + "'", 1);
            }
            else
            {
                createDate1 = oDBEngine.GetFieldValue("tbl_master_contact", "(convert(varchar(11),createdate,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), createdate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), createdate, 22), 3))) as createdate", "cnt_internalid='" + Id + "'", 1);
            }
            string createDate = "";
            if (createDate1[0, 0] != "n")
            {
                createDate = createDate1[0, 0];
            }
            string[,] DataSource1;
            if (lcid == "LD")
            {
                DataSource1 = oDBEngine.GetFieldValue("tbl_master_contactsource inner join tbl_master_lead on tbl_master_contactsource.cntsrc_id=tbl_master_lead.cnt_contactsource", "cntsrc_sourceType", "cnt_internalid='" + Id + "'", 1);
            }
            else
            {
                DataSource1 = oDBEngine.GetFieldValue("tbl_master_contactsource inner join tbl_master_contact on tbl_master_contactsource.cntsrc_id=tbl_master_contact.cnt_contactsource", "cntsrc_sourceType", "cnt_internalid='" + Id + "'", 1);
            }
            string DataSource = "";
            if (DataSource1[0, 0] != "n")
            {
                DataSource = DataSource1[0, 0];
            }
            string[,] SourceName1;
            if (lcid == "LD")
            {
                SourceName1 = oDBEngine.GetFieldValue("tbl_master_contact inner join tbl_master_lead  on tbl_master_contact.cnt_internalid=tbl_master_lead.cnt_referedby", "isnull(tbl_master_contact.cnt_firstname,'') + ' ' + isnull(tbl_master_contact.cnt_middlename,'') + ' ' + isnull(tbl_master_contact.cnt_lastname,'') as Name", "tbl_master_lead.cnt_internalid='" + Id + "'", 1);
            }
            else
            {
                SourceName1 = oDBEngine.GetFieldValue("tbl_master_contact c,tbl_master_contact c1", "isnull(c1.cnt_firstname,'') + ' ' + isnull(c1.cnt_middlename,'') + ' ' + isnull(c1.cnt_lastname,'') as Name", "c1.cnt_internalId=c.cnt_referedBy and c.cnt_internalId='" + Id + "'", 1);
            }
            string SourceName = "";
            if (SourceName1[0, 0] != "n")
            {
                SourceName = SourceName1[0, 0];
            }
            showContactPanel.InnerHtml = "<table width='100%'><tr><td align='left' style='font-family: Tahoma,Arial, Verdana, sans-serif;font-size:10px;'><b>Create User : </b>" + createUser + " </td><td align='center' style='font-family: Tahoma,Arial, Verdana, sans-serif;font-size:10px;'><b>Create Date : </b>" + createDate + "</td><td align='right' style='font-family: Tahoma,Arial, Verdana, sans-serif;font-size:10px;'><b>Contact Source : </b>" + DataSource + " [" + SourceName + "]" + "<br></td></tr></table>";
            string[,] ActName1;
            if (lcid == "LD")
            {
                ActName1 = oDBEngine.GetFieldValue("tbl_master_lead", "case cnt_status when 'due' then 'Due' else (SELECT tbl_master_activitytype.aty_activityType FROM tbl_trans_Activies INNER JOIN tbl_master_activitytype ON tbl_trans_Activies.act_activityType = tbl_master_activitytype.aty_id where tbl_trans_Activies.act_activityNo=tbl_master_lead.cnt_status) end as ActType", "cnt_internalid='" + Id + "'", 1);
            }
            else
            {
                ActName1 = oDBEngine.GetFieldValue("tbl_master_contact", "case cnt_status when 'due' then 'Due' else (SELECT tbl_master_activitytype.aty_activityType FROM tbl_trans_Activies INNER JOIN tbl_master_activitytype ON tbl_trans_Activies.act_activityType = tbl_master_activitytype.aty_id where tbl_trans_Activies.act_activityNo=tbl_master_contact.cnt_status) end as ActType", "cnt_internalid='" + Id + "'", 1);
            }
            string ActName = "";
            if (ActName1[0, 0] != "n")
            {
                ActName = ActName1[0, 0];
            }
            string[,] UserName1;
            if (lcid == "LD")
            {
                UserName1 = oDBEngine.GetFieldValue("tbl_master_lead", "case cnt_status when 'due' then 'None' else (SELECT tbl_master_user.user_name + ' -> ' + (SELECT (SELECT in_a.user_name AS Expr1 FROM tbl_master_employee in_b INNER JOIN tbl_master_user in_a ON in_b.emp_contactId = in_a.user_contactId WHERE in_b.emp_id = (SELECT TOP 1 emp_reportto FROM tbl_trans_employeectc WHERE(emp_cntid = a.user_contactid) ORDER BY emp_id DESC)) FROM tbl_master_user a INNER JOIN tbl_master_employee b ON a.user_contactId = b.emp_contactId WHERE(a.user_id = tbl_master_user.user_id)) + ' [' + tbl_master_branch.branch_description + ']' AS Expr1 FROM  tbl_trans_Activies INNER JOIN tbl_master_user ON tbl_trans_Activies.act_assignedTo = tbl_master_user.user_id INNER JOIN tbl_master_branch ON tbl_master_user.user_branchId = tbl_master_branch.branch_id where tbl_trans_Activies.act_activityNo=tbl_master_lead.cnt_status) end as ActType", "cnt_internalid='" + Id + "'", 1);
            }
            else
            {
                UserName1 = oDBEngine.GetFieldValue("tbl_master_contact", "case cnt_status when 'due' then 'None' else (SELECT tbl_master_user.user_name + ' -> ' + (SELECT (SELECT in_a.user_name AS Expr1 FROM tbl_master_employee in_b INNER JOIN tbl_master_user in_a ON in_b.emp_contactId = in_a.user_contactId WHERE in_b.emp_id = (SELECT TOP 1 emp_reportto FROM tbl_trans_employeectc WHERE(emp_cntid = a.user_contactid) ORDER BY emp_id DESC)) FROM tbl_master_user a INNER JOIN tbl_master_employee b ON a.user_contactId = b.emp_contactId WHERE(a.user_id = tbl_master_user.user_id)) + ' [' + tbl_master_branch.branch_description + ']' AS Expr1 FROM  tbl_trans_Activies INNER JOIN tbl_master_user ON tbl_trans_Activies.act_assignedTo = tbl_master_user.user_id INNER JOIN tbl_master_branch ON tbl_master_user.user_branchId = tbl_master_branch.branch_id where tbl_trans_Activies.act_activityNo=tbl_master_contact.cnt_status) end as ActType", "cnt_internalid='" + Id + "'", 1);
            }
            string UserName = "";
            if (UserName1[0, 0] != "n")
            {
                UserName = UserName1[0, 0];
            }
            showActivityPanl.InnerHtml = "<table width='100%'><tr><td align='left' style='font-family: Tahoma,Arial, Verdana, sans-serif;font-size:10px;'><b>Current Activity : </b>" + ActName + "<b>  By : </b>" + UserName + "<br></td></tr></table>";
            DataTable dt = new DataTable();
            DataTable Phone_Call = new DataTable();
            Phone_Call = oDBEngine.GetDataTable("tbl_trans_phonecalldetails INNER JOIN tbl_trans_phonecall ON tbl_trans_phonecalldetails.phd_phoneCallId = tbl_trans_phonecall.phc_id INNER JOIN tbl_master_calldispositions ON tbl_trans_phonecalldetails.phd_callDispose = tbl_master_calldispositions.call_id INNER JOIN tbl_master_user ON tbl_trans_phonecalldetails.CreateUser = tbl_master_user.user_id", "(convert(varchar(11),tbl_trans_phonecalldetails.phd_callDate,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_phonecalldetails.phd_callDate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_phonecalldetails.phd_callDate, 22), 3))) AS CallDate,(convert(varchar(11),tbl_trans_phonecalldetails.phd_nextCall,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_phonecalldetails.phd_nextCall, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_phonecalldetails.phd_nextCall, 22), 3))) AS NextCall, tbl_trans_phonecalldetails.phd_note AS Note, tbl_master_calldispositions.call_dispositions, tbl_master_user.user_name, isnull(tbl_trans_phonecalldetails.phd_callType,'') as CallType", "tbl_trans_phonecall.phc_leadcotactId='" + Id + "' order by CONVERT(varchar(17), tbl_trans_phonecalldetails.phd_callDate, 113)");
            dt.Columns.Add("Type");
            dt.Columns.Add("Call/VisitDate");
            dt.Columns.Add("Description");
            dt.Columns.Add("Next Call/Visit");
            dt.Columns.Add("User");
            if (Phone_Call.Rows.Count != 0)
            {
                for (int i = 0; i < Phone_Call.Rows.Count; i++)
                {
                    DataRow NewRow = dt.NewRow();
                    if (Convert.ToString(Phone_Call.Rows[i]["CallType"]) == "")
                    {
                        NewRow[0] = "PhoneCall";
                    }
                    else
                    {
                        NewRow[0] = "ReminderCall";
                    }
                    NewRow[1] = Convert.ToString(Phone_Call.Rows[i]["CallDate"]);
                    NewRow[2] = Convert.ToString(Phone_Call.Rows[i]["call_dispositions"]) + "  [" + Convert.ToString(Phone_Call.Rows[i]["Note"]) + "]";
                    NewRow[3] = Convert.ToString(Phone_Call.Rows[i]["NextCall"]);
                    NewRow[4] = Convert.ToString(Phone_Call.Rows[i]["user_name"]);
                    dt.Rows.Add(NewRow);
                }

            }
            DataTable Courtest_Call = new DataTable();
            Courtest_Call = oDBEngine.GetDataTable("tbl_trans_CourtesyCalls INNER JOIN tbl_master_CourtesyCallFeedback ON tbl_trans_CourtesyCalls.cpc_feedBack = tbl_master_CourtesyCallFeedback.ccf_id INNER JOIN tbl_master_SalesVisitOutCome ON tbl_trans_CourtesyCalls.cpc_outcome = tbl_master_SalesVisitOutCome.slv_Id INNER JOIN tbl_master_user ON tbl_trans_CourtesyCalls.CreateUser = tbl_master_user.user_id", "(convert(varchar(11),tbl_trans_CourtesyCalls.cpc_callStartTime,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_CourtesyCalls.cpc_callStartTime, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_CourtesyCalls.cpc_callStartTime, 22), 3))) AS CallDate, tbl_master_CourtesyCallFeedback.ccf_feedback,tbl_master_SalesVisitOutCome.slv_SalesVisitOutcome, tbl_trans_CourtesyCalls.cpc_note, tbl_master_user.user_name", " tbl_trans_CourtesyCalls.cpc_leadcontactId='" + Id + "' order by cpc_id");
            if (Courtest_Call.Rows.Count != 0)
            {
                for (int i = 0; i < Courtest_Call.Rows.Count; i++)
                {
                    DataRow NewRow = dt.NewRow();
                    NewRow[0] = "Courtest Call";
                    NewRow[1] = Convert.ToString(Courtest_Call.Rows[i]["CallDate"]);
                    NewRow[2] = "<b>CourtesyCall Feedback : </b>" + Convert.ToString(Courtest_Call.Rows[i]["ccf_feedback"]) + "  <b>Visit Outcome : </b>" + Convert.ToString(Courtest_Call.Rows[i]["slv_SalesVisitOutcome"]) + " [" + Convert.ToString(Courtest_Call.Rows[i]["cpc_note"]) + "]";
                    NewRow[3] = "";
                    NewRow[4] = Convert.ToString(Courtest_Call.Rows[i]["user_name"]);
                    dt.Rows.Add(NewRow);
                }
            }
            DataTable Sales_Visit = new DataTable();
            Sales_Visit = oDBEngine.GetDataTable("tbl_trans_SalesVisitDetail INNER JOIN tbl_master_SalesVisitOutCome ON tbl_trans_SalesVisitDetail.slv_SalesVisitOutcome = tbl_master_SalesVisitOutCome.slv_Id INNER JOIN tbl_master_user ON tbl_trans_SalesVisitDetail.CreateUser = tbl_master_user.user_id INNER JOIN tbl_trans_salesVisit ON tbl_trans_SalesVisitDetail.slv_SalesVisitId = tbl_trans_salesVisit.slv_id", "(convert(varchar(11),tbl_trans_SalesVisitDetail.slv_VisitDateTime,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_SalesVisitDetail.slv_VisitDateTime, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_SalesVisitDetail.slv_VisitDateTime, 22), 3))) AS VisitDate,(convert(varchar(11),tbl_trans_SalesVisitDetail.slv_nextVisit,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_SalesVisitDetail.slv_nextVisit, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_SalesVisitDetail.slv_nextVisit, 22), 3))) as slv_nextVisit, tbl_trans_SalesVisitDetail.slv_notes,tbl_master_SalesVisitOutCome.slv_SalesVisitOutcome, tbl_master_user.user_name,tbl_master_SalesVisitOutCome.slv_Category as OutCome", " slv_leadcotactid='" + Id + "' order by convert(varchar(17),slv_VisitDateTime,113)");
            if (Sales_Visit.Rows.Count != 0)
            {
                for (int i = 0; i < Sales_Visit.Rows.Count; i++)
                {
                    DataRow NewRow = dt.NewRow();
                    NewRow[0] = "SalesVisit";
                    NewRow[1] = Convert.ToString(Sales_Visit.Rows[i]["VisitDate"]);
                    string Sales = "";
                    if (Convert.ToInt32(Sales_Visit.Rows[i]["Outcome"]) > 8)
                    {
                        Sales = "<b>Phone Follow Up :- </b>";
                    }
                    else
                    {
                        Sales = "<b>Visited :- </b>";
                    }
                    NewRow[2] = Sales + Convert.ToString(Sales_Visit.Rows[i]["slv_SalesVisitOutcome"]) + " [" + Convert.ToString(Sales_Visit.Rows[i]["slv_Notes"]) + "]";
                    NewRow[3] = Convert.ToString(Sales_Visit.Rows[i]["slv_nextVisit"]);
                    NewRow[4] = Convert.ToString(Sales_Visit.Rows[i]["user_name"]);
                    dt.Rows.Add(NewRow);
                }
            }
            DataTable DtSales = new DataTable();
            DtSales = oDBEngine.GetDataTable("tbl_trans_salesDetails INNER JOIN tbl_trans_Sales ON tbl_trans_salesDetails.sad_salesId = tbl_trans_Sales.sls_id INNER JOIN tbl_master_SalesStatus ON tbl_trans_salesDetails.sad_SaleStage = tbl_master_SalesStatus.sls_id INNER JOIN tbl_master_user ON tbl_trans_salesDetails.CreateUser = tbl_master_user.user_id", "(convert(varchar(11),tbl_trans_salesDetails.sad_DateTime,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_salesDetails.sad_DateTime, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_salesDetails.sad_DateTime, 22), 3))) as sad_dateTime, tbl_trans_salesDetails.sad_Notes, tbl_master_SalesStatus.sls_status, tbl_master_user.user_name, tbl_trans_salesDetails.sad_activityType, (convert(varchar(11),tbl_trans_salesDetails.sad_nextVisitDate,113)+ ' '+ LTRIM(SUBSTRING(CONVERT(VARCHAR(20), tbl_trans_salesDetails.sad_nextVisitDate, 22), 10, 5)+ RIGHT(CONVERT(VARCHAR(20), tbl_trans_salesDetails.sad_nextVisitDate, 22), 3))) as sad_nextVisitDate, tbl_trans_salesDetails.sad_visitPurpose", " sad_cntId='" + Id + "' order by sad_DateTime");
            if (DtSales.Rows.Count != 0)
            {
                for (int i = 0; i < DtSales.Rows.Count; i++)
                {
                    DataRow NewRow = dt.NewRow();
                    NewRow[0] = "Sales";
                    NewRow[1] = Convert.ToString(DtSales.Rows[i]["sad_DateTime"]);
                    string VisitType = "Visited :- ";
                    if (Convert.ToString(DtSales.Rows[i]["sad_ActivityType"]) != "")
                    {
                        if (Convert.ToInt32(DtSales.Rows[i]["sad_ActivityType"]) == 1)
                        {
                            VisitType = "Phone Follow Up :- ";
                        }
                    }
                    NewRow[2] = "<b>" + VisitType + "</b>" + Convert.ToString(DtSales.Rows[i]["sad_visitpurpose"]) + " [" + Convert.ToString(DtSales.Rows[i]["sad_Notes"]) + "]";
                    NewRow[3] = Convert.ToString(DtSales.Rows[i]["sad_nextvisitdate"]) + " [" + Convert.ToString(DtSales.Rows[i]["sls_status"]) + "]";
                    NewRow[4] = Convert.ToString(DtSales.Rows[i]["user_name"]);
                    dt.Rows.Add(NewRow);
                }
            }
            DataTable Email = new DataTable();
            Email = oDBEngine.GetDataTable("tbl_trans_email INNER JOIN tbl_master_template ON tbl_trans_email.hem_temid = tbl_master_template.tem_id INNER JOIN tbl_master_user ON tbl_trans_email.CreateUser = tbl_master_user.user_id INNER JOIN tbl_trans_email_attachment ON tbl_trans_email.hem_id=tbl_trans_email_attachment.hem_id", "convert(varchar(17),tbl_trans_email.CreateDate,113) as CreateDate, tbl_master_template.tem_shortmsg,tbl_master_user.user_name, tbl_trans_email_attachment.hem_attachment", " tbl_trans_email.hem_activityid='" + Id + "' order by convert(varchar(17),tbl_trans_email.CreateDate,113)");
            if (Email.Rows.Count != 0)
            {
                for (int i = 0; i < Email.Rows.Count; i++)
                {
                    DataRow NewRow = dt.NewRow();
                    NewRow[0] = "Email";
                    NewRow[1] = Convert.ToString(Email.Rows[i]["CreateDate"]);
                    string attach = Convert.ToString(Email.Rows[i]["hem_attachment"]);
                    if (attach != "")
                    {
                        int s = attach.LastIndexOf("\\");
                        string ss = attach.Substring(43);
                        attach = "<u>Attachment :</u> " + ss.ToUpper();
                    }
                    NewRow[2] = "<b>" + Convert.ToString(Email.Rows[i]["tem_shortmsg"]) + "</b>     [" + attach + "]";
                    NewRow[3] = "";
                    NewRow[4] = Convert.ToString(Email.Rows[i]["user_name"]);
                    dt.Rows.Add(NewRow);
                }
            }
            grdShowHistory.DataSource = dt.DefaultView;
            grdShowHistory.DataBind();
        }
        protected void grdShowHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int i = 0; i <= e.Row.Cells.Count - 1; i++)
                {
                    e.Row.Cells[i].Text = Convert.ToString(Server.HtmlDecode(e.Row.Cells[i].Text));
                }
            }
        }
    }
}