using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;

namespace ERP.OMS.Management.ToolsUtilities
{
    public partial class management_utilities_frmrfa : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        clsDropDownList clsdropdown = new clsDropDownList();
        string data = "";
        public string pageAccess = "";

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            if (!IsPostBack)
            {
                string[,] rfa = oDBEngine.GetFieldValue("tbl_master_rfa", "rfa_id,rfa_shortname", null, 2, "rfa_shortname");
                if (rfa[0, 0] != "n")
                {
                    clsdropdown.AddDataToDropDownList(rfa, lst_templates, true);
                }
                BindGridMessage();
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>btnCancel_Click();</script>");
                lst_templates.Attributes.Add("onchange", "Rfa_Template()");
                lst_requestcategory.Attributes.Add("onchange", "GridBind()");
                String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
                String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            }
        }
        public void BindGridMessage()
        {
            DataTable dt = new DataTable();
            if (lst_requestcategory.SelectedValue == "0")
            {
                dt = oDBEngine.GetDataTable("tbl_trans_rfa INNER JOIN tbl_trans_rfadetails ON tbl_trans_rfa.req_id = tbl_trans_rfadetails.rtd_reqnumber INNER JOIN tbl_master_user tbl_master_user_1 ON tbl_trans_rfa.req_currapprover = tbl_master_user_1.user_id INNER JOIN tbl_master_rfa ON tbl_trans_rfa.req_rfaid = tbl_master_rfa.rfa_id LEFT OUTER JOIN tbl_master_user tbl_master_user_2 ON tbl_trans_rfa.req_preapprover = tbl_master_user_2.user_id  WHERE (tbl_trans_rfa.req_AprRejNo is null) AND (tbl_trans_rfa.req_preapprover is null) and (tbl_trans_rfa.req_currstatus = 0) and (req_userid=" + Session["userid"].ToString() + ")", "DISTINCT  tbl_trans_rfadetails.rtd_reqnumber,rfa_shortname as [Request Id], tbl_master_user_2.user_name AS [Previous Approver], CASE tbl_trans_rfa.req_prestatus WHEN 0 THEN 'Unread' WHEN 1 THEN 'Approved' WHEN 3 THEN 'Approved With MOdification' WHEN 2 THEN 'Reject' WHEN 4 THEN 'Forward to Higher Authority' WHEN 5 THEN 'Pendding' END AS [Previous Status], tbl_trans_rfa.req_prenote AS [Previous Note], tbl_master_user_1.user_name AS [current Approver], CASE tbl_trans_rfa.req_currstatus WHEN 0 THEN 'Unread' WHEN 1 THEN 'Approved' WHEN 3 THEN 'Approved With MOdification' WHEN 2 THEN 'Reject' WHEN 4 THEN 'Forward to Hidher Authority' WHEN 5 THEN 'Pendding' END AS [current Status], tbl_trans_rfa.req_currnote AS [current Note], req_aprrejno AS [Approval No], tbl_trans_rfa.req_description AS mesage", null, "rtd_reqnumber DESC");
            }
            else if (lst_requestcategory.SelectedValue == "1")
            {
                dt = oDBEngine.GetDataTable("tbl_trans_rfa INNER JOIN tbl_trans_rfadetails ON tbl_trans_rfa.req_id = tbl_trans_rfadetails.rtd_reqnumber INNER JOIN tbl_master_user tbl_master_user_1 ON tbl_trans_rfa.req_currapprover = tbl_master_user_1.user_id INNER JOIN tbl_master_rfa ON tbl_trans_rfa.req_rfaid = tbl_master_rfa.rfa_id LEFT OUTER JOIN tbl_master_user tbl_master_user_2 ON tbl_trans_rfa.req_preapprover = tbl_master_user_2.user_id WHERE (tbl_trans_rfa.req_AprRejNo is null) AND ((tbl_trans_rfa.req_preapprover > 0) or (tbl_trans_rfa.req_currstatus > 0))  and (req_userid=" + Session["userid"].ToString() + ")", "DISTINCT  tbl_trans_rfadetails.rtd_reqnumber,rfa_shortname as [Request Id], tbl_master_user_2.user_name AS [Previous Approver], CASE tbl_trans_rfa.req_prestatus WHEN 0 THEN 'Unread' WHEN 1 THEN 'Approved' WHEN 3 THEN 'Approved With MOdification' WHEN 2 THEN 'Reject' WHEN 4 THEN 'Forward to Higher Authority' WHEN 5 THEN 'Pendding' END AS [Previous Status], tbl_trans_rfa.req_prenote AS [Previous Note], tbl_master_user_1.user_name AS [current Approver], CASE tbl_trans_rfa.req_currstatus WHEN 0 THEN 'Unread' WHEN 1 THEN 'Approved' WHEN 3 THEN 'Approved With MOdification' WHEN 2 THEN 'Reject' WHEN 4 THEN 'Forward to Hidher Authority' WHEN 5 THEN 'Pendding' END AS [current Status], tbl_trans_rfa.req_currnote AS [current Note], req_aprrejno AS [Approval No], tbl_trans_rfa.req_description AS mesage", null, "rtd_reqnumber DESC");
            }
            else if (lst_requestcategory.SelectedValue == "2")
            {
                dt = oDBEngine.GetDataTable("tbl_trans_rfa INNER JOIN tbl_trans_rfadetails ON tbl_trans_rfa.req_id = tbl_trans_rfadetails.rtd_reqnumber INNER JOIN tbl_master_user tbl_master_user_1 ON tbl_trans_rfa.req_currapprover = tbl_master_user_1.user_id INNER JOIN tbl_master_rfa ON tbl_trans_rfa.req_rfaid = tbl_master_rfa.rfa_id LEFT OUTER JOIN tbl_master_user tbl_master_user_2 ON tbl_trans_rfa.req_preapprover = tbl_master_user_2.user_id", "DISTINCT tbl_trans_rfadetails.rtd_reqnumber,rfa_shortname as [Request Id], tbl_master_user_2.user_name AS [Previous Approver], CASE tbl_trans_rfa.req_prestatus WHEN 0 THEN 'Unread' WHEN 1 THEN 'Approved' WHEN 3 THEN 'Approved With MOdification' WHEN 2 THEN 'Reject' WHEN 4 THEN 'Forward to Higher Authority' WHEN 5 THEN 'Pendding' WHEN 6 THEN 'Recorded' END AS [Previous Status], tbl_trans_rfa.req_prenote AS [Previous Note], tbl_master_user_1.user_name AS [current Approver], CASE tbl_trans_rfa.req_currstatus WHEN 0 THEN 'Unread' WHEN 1 THEN 'Approved' WHEN 3 THEN 'Approved With MOdification' WHEN 2 THEN 'Reject' WHEN 4 THEN 'Forward to Hidher Authority' WHEN 5 THEN 'Pendding' END AS [current Status], tbl_trans_rfa.req_currnote AS [current Note], req_aprrejno AS [Approval No], tbl_trans_rfa.req_description AS mesage", " (tbl_trans_rfa.req_AprRejNo <> N'') AND (tbl_trans_rfa.req_currstatus <> 2) and (req_userid=" + Session["userid"].ToString() + ")", "rtd_reqnumber DESC");
            }
            else
            {
                dt = oDBEngine.GetDataTable("tbl_trans_rfa INNER JOIN tbl_trans_rfadetails ON tbl_trans_rfa.req_id = tbl_trans_rfadetails.rtd_reqnumber INNER JOIN tbl_master_user tbl_master_user_1 ON tbl_trans_rfa.req_currapprover = tbl_master_user_1.user_id INNER JOIN tbl_master_rfa ON tbl_trans_rfa.req_rfaid = tbl_master_rfa.rfa_id LEFT OUTER JOIN tbl_master_user tbl_master_user_2 ON tbl_trans_rfa.req_preapprover = tbl_master_user_2.user_id", "DISTINCT tbl_trans_rfadetails.rtd_reqnumber,rfa_shortname as [Request Id], tbl_master_user_2.user_name AS [Previous Approver], CASE tbl_trans_rfa.req_prestatus WHEN 0 THEN 'Unread' WHEN 1 THEN 'Approved' WHEN 3 THEN 'Approved With MOdification' WHEN 2 THEN 'Reject' WHEN 4 THEN 'Forward to Higher Authority' WHEN 5 THEN 'Pendding' WHEN 6 THEN 'Recorded' END AS [Previous Status], tbl_trans_rfa.req_prenote AS [Previous Note], tbl_master_user_1.user_name AS [current Approver], CASE tbl_trans_rfa.req_currstatus WHEN 0 THEN 'Unread' WHEN 1 THEN 'Approved' WHEN 3 THEN 'Approved With MOdification' WHEN 2 THEN 'Reject' WHEN 4 THEN 'Forward to Hidher Authority' WHEN 5 THEN 'Pendding' END AS [current Status], tbl_trans_rfa.req_currnote AS [current Note], req_aprrejno AS [Approval No], tbl_trans_rfa.req_description AS mesage", " (tbl_trans_rfa.req_AprRejNo <> N'') AND (tbl_trans_rfa.req_currstatus = 2) and (req_userid=" + Session["userid"].ToString() + ")", "rtd_reqnumber DESC");
            }
            if (dt.Rows.Count > 0)
            {
                grd_request.DataSource = dt;
                grd_request.DataBind();
            }
            dt.Dispose();
        }
        protected void grd_reqhistory_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string param = e.Parameters.ToString();
            DataTable dt = oDBEngine.GetDataTable("tbl_trans_rfadetails INNER JOIN tbl_master_user ON tbl_trans_rfadetails.rtd_appuser = tbl_master_user.user_id LEFT OUTER JOIN tbl_master_user tbl_master_user_1 ON tbl_trans_rfadetails.rtd_nexapprovalid = tbl_master_user_1.user_id", "tbl_trans_rfadetails.rtd_id AS ID, tbl_master_user.user_name AS [Approver Name], convert(varchar(11),tbl_trans_rfadetails.rtd_readdate,113) AS [Read Date],CASE tbl_trans_rfadetails.rtd_status WHEN 0 THEN 'Unread' WHEN 1 THEN 'Approved' WHEN 3 THEN 'Approved With MOdification' WHEN 2 THEN 'Reject' WHEN 4 THEN 'Forward to Higher Authority' WHEN 5 THEN 'Pendding' WHEN 6 THEN 'Recorded' END AS [Previous Status], tbl_trans_rfadetails.rtd_note AS Note,tbl_master_user_1.user_name AS [Next Approver]", " tbl_trans_rfadetails.rtd_reqnumber='" + param + "'", "rtd_id");
            if (dt.Rows.Count > 0)
            {
                grd_reqhistory.DataSource = dt;
                grd_reqhistory.DataBind();
            }
        }
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            data = "";
            string id = eventArgument.ToString();
            string[] idlist = id.Split('~');
            if (idlist[0] == "Save")
            {
                int NoOfRowsAffected = 0;
                string current_approver = "";
                string request_closedate = "";
                string detail_request_id = "";
                string request_id = "";
                DataTable dtTemp = oDBEngine.GetDataTable("tbl_master_rfa", "*", " rfa_id='" + idlist[2] + "'");
                request_id = dtTemp.Rows[0]["rfa_id"].ToString() + Format(oDBEngine.GetDate().Month.ToString()) + oDBEngine.GetDate().Year.ToString().Substring(2, 2);
                request_id = oDBEngine.GetInternalId_Req(request_id, "tbl_trans_rfa", "req_id", "req_id");
                detail_request_id = oDBEngine.GetInternalId_Req("RFA" + Format(oDBEngine.GetDate().Month.ToString()) + oDBEngine.GetDate().Year.ToString().Substring(2, 2), "tbl_trans_rfadetails", "rtd_id", "rtd_id");
                request_closedate = oDBEngine.GetDate().AddHours(Convert.ToDouble(dtTemp.Rows[0]["rfa_hoursallowed"].ToString())).ToString();
                int result;
                if (idlist[3] != "")
                {
                    if (Int32.TryParse(idlist[3], out result))
                    {
                        string[] b_id = Session["userbranchHierarchy"].ToString().Split(',');
                        for (int k = 0; k < b_id.Length; k++)
                        {
                            string[,] cApprover = oDBEngine.GetFieldValue("tbl_master_user INNER JOIN   tbl_trans_employeeCTC ON tbl_master_user.user_contactId = tbl_trans_employeeCTC.emp_cntId WHERE  (tbl_master_user.user_branchId =" + b_id[k] + ") AND (tbl_master_user.user_id NOT IN  (SELECT     tbl_master_user_2.user_id   FROM  tbl_master_user AS tbl_master_user_2 INNER JOIN   tbl_trans_employeeCTC AS tbl_trans_employeeCTC_2 ON  tbl_master_user_2.user_contactId = tbl_trans_employeeCTC_2.emp_cntId INNER JOIN  tbl_master_employee ON tbl_trans_employeeCTC_2.emp_reportTo = tbl_master_employee.emp_id INNER JOIN  tbl_master_user AS tbl_master_user_1 ON tbl_master_employee.emp_contactId = tbl_master_user_1.user_contactId INNER JOIN  tbl_trans_employeeCTC AS tbl_trans_employeeCTC_1 ON  tbl_master_user_1.user_contactId = tbl_trans_employeeCTC_1.emp_cntId   WHERE      (tbl_master_user_2.user_branchId =" + b_id[k] + ") AND (tbl_trans_employeeCTC_2.emp_Department = " + idlist[3] + ") AND  (tbl_trans_employeeCTC_1.emp_Department =" + idlist[3] + ") AND (tbl_master_user_1.user_branchId =" + b_id[k] + "))) AND  (tbl_trans_employeeCTC.emp_Department =" + idlist[3] + ")", "tbl_master_user.user_id", null, 1);
                            if (cApprover[0, 0] != "n")
                            {
                                current_approver = cApprover[0, 0];
                            }
                        }
                    }
                    else if (idlist[3] == "HOD")
                    {
                        string[,] cApprover = oDBEngine.GetFieldValue("tbl_master_user tbl_master_user_1 INNER JOIN tbl_trans_employeeCTC tbl_trans_employeeCTC_1 ON tbl_master_user_1.user_contactId = tbl_trans_employeeCTC_1.emp_cntId INNER JOIN  tbl_master_employee ON tbl_trans_employeeCTC_1.emp_reportTo = tbl_master_employee.emp_id INNER JOIN tbl_master_user tbl_master_user_2 ON tbl_master_employee.emp_contactId = tbl_master_user_2.user_contactId", "tbl_master_user_2.user_id AS reporthead", " tbl_master_user_1.user_id = '" + Session["userid"].ToString() + "'", 1);
                        if (cApprover[0, 0] != "n")
                        {
                            current_approver = cApprover[0, 0];
                        }
                    }
                    else if (idlist[3] == "OBH")
                    {
                        string[,] cApprover = oDBEngine.GetFieldValue("tbl_master_user INNER JOIN tbl_master_branch ON tbl_master_user.user_branchId = tbl_master_branch.branch_id INNER JOIN  tbl_master_user tbl_master_user_1 ON tbl_master_branch.branch_head = tbl_master_user_1.user_contactId", "tbl_master_user_1.user_id AS head", " tbl_master_user.user_id = '" + Session["userid"].ToString() + "'", 1);
                        if (cApprover[0, 0] != "n")
                        {
                            current_approver = cApprover[0, 0];
                        }
                    }
                    else if (idlist[3].Substring(0, 2) == "EM")
                    {
                        string[,] cApprover = oDBEngine.GetFieldValue("tbl_master_user", "user_id AS head", " user_contactid = '" + idlist[3] + "'", 1);
                        if (cApprover[0, 0] != "n")
                        {
                            current_approver = cApprover[0, 0];
                        }
                    }
                    else if (idlist[3].Substring(0, 2) == "BR")
                    {
                        string[,] cApprover = oDBEngine.GetFieldValue("tbl_master_branch", "branch_head AS head", " branch_internalid = '" + idlist[3] + "'", 1);
                        if (cApprover[0, 0] != "n")
                        {
                            current_approver = cApprover[0, 0];
                        }
                    }
                    else if (idlist[3] == "RH")
                    {
                        string[,] cApprover = oDBEngine.GetFieldValue("tbl_master_user tbl_master_user_1 INNER JOIN tbl_trans_employeeCTC tbl_trans_employeeCTC_1 ON tbl_master_user_1.user_contactId = tbl_trans_employeeCTC_1.emp_cntId INNER JOIN  tbl_master_employee ON tbl_trans_employeeCTC_1.emp_reportTo = tbl_master_employee.emp_id INNER JOIN tbl_master_user tbl_master_user_2 ON tbl_master_employee.emp_contactId = tbl_master_user_2.user_contactId", "tbl_master_user_2.user_id AS reporthead", " tbl_master_user_1.user_id = '" + Session["userid"].ToString() + "'", 1);
                        if (cApprover[0, 0] != "n")
                        {
                            current_approver = cApprover[0, 0];
                        }
                    }
                }
                NoOfRowsAffected = oDBEngine.InsurtFieldValue("tbl_trans_rfa", "req_id,req_description,req_rfaid,req_userid,req_startdate,req_currapprover,req_currstatus,req_closedate,req_totallevel,req_currentlevel,CreateDate,CreateUser", "'" + request_id + "','" + idlist[1] + "','" + dtTemp.Rows[0]["rfa_id"].ToString() + "','" + Session["userid"].ToString() + "','" + oDBEngine.GetDate().ToString() + "','" + current_approver + "',0,'" + request_closedate + "','" + dtTemp.Rows[0]["rfa_totallevel"].ToString() + "',1,'" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'");
                NoOfRowsAffected = oDBEngine.InsurtFieldValue("tbl_trans_rfadetails", "rtd_id,rtd_reqnumber,rtd_appuser,rtd_status,CreateDate,CreateUser", "'" + detail_request_id + "','" + request_id + "','" + current_approver + "',0,'" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'");
                try
                {
                    string mail_To = current_approver;
                    string Mail_From = Session["userid"].ToString();
                    string[,] mailTo = oDBEngine.GetFieldValue("tbl_master_email INNER JOIN tbl_master_user ON tbl_master_email.eml_cntId = tbl_master_user.user_contactId where tbl_master_user.user_id= '" + mail_To + "'", "Top 1 eml_email", null, 1);
                    if (mailTo[0, 0] != "n")
                    {
                        mail_To = mailTo[0, 0];
                    }
                    if (mail_To != "")
                    {
                        string[,] mailFrom = oDBEngine.GetFieldValue("tbl_master_email INNER JOIN tbl_master_user ON tbl_master_email.eml_cntId = tbl_master_user.user_contactId where tbl_master_user.user_id= '" + Mail_From + "'", "Top 1 eml_email", null, 1);
                        if (mailFrom[0, 0] != "n")
                        {
                            Mail_From = mailFrom[0, 0];
                        }
                        if (Mail_From == "")
                        {
                            Mail_From = "binay@influxerp.com";
                        }
                        string Request_By = "";
                        string[,] RequestBy = oDBEngine.GetFieldValue("tbl_master_contact INNER JOIN tbl_master_user ON tbl_master_contact.cnt_internalId = tbl_master_user.user_contactId", "Top 1 IsNull(tbl_master_contact.cnt_firstName,'')+' '+IsNull(tbl_master_contact.cnt_middleName,'')+' '+IsNull(tbl_master_contact.cnt_lastName,'')", " tbl_master_user.user_id='" + Session["userid"].ToString() + "'", 1);
                        if (RequestBy[0, 0] != "n")
                        {
                            Request_By = RequestBy[0, 0];
                        }
                        string Subject = "";
                        string[,] Subject1 = oDBEngine.GetFieldValue("tbl_master_rfa", "rfa_shortname", " rfa_id='" + dtTemp.Rows[0]["rfa_id"].ToString() + "'", 1);
                        if (Subject1[0, 0] != "n")
                        {
                            Subject = Subject1[0, 0];
                        }
                        string Body = "<table border='0' cellpadding='3' cellspacing='3' style='width: 100%; font-weight: normal; font-size: 8pt; text-transform: capitalize; color: black; font-style: normal; font-family: Verdana;'><tr><td colspan='3' style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top; border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite; text-align: left; font-weight: bold;'> Request By :-  " + Request_By + "</td></tr><tr><td colspan='3' style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top; border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite; text-align: left'> Request :- " + idlist[1] + "</td></tr></table>";
                        objConverter.SendMail(Mail_From, mail_To, Subject, Body);

                    }
                }
                catch
                {
                }
                if (NoOfRowsAffected != 0)
                {
                    data = "Save~Y";
                }
            }
        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        public string Format(string currentMonth)
        {
            int cMonth = Convert.ToInt32(currentMonth.Length);
            if (cMonth == 1)
            {
                currentMonth = "0" + currentMonth;
            }
            return currentMonth;
        }
        protected void grd_request_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string param = e.Parameters.ToString();
            DataTable dt = new DataTable();
            if (param == "0")
            {
                dt = oDBEngine.GetDataTable("tbl_trans_rfa INNER JOIN tbl_trans_rfadetails ON tbl_trans_rfa.req_id = tbl_trans_rfadetails.rtd_reqnumber INNER JOIN tbl_master_user tbl_master_user_1 ON tbl_trans_rfa.req_currapprover = tbl_master_user_1.user_id INNER JOIN tbl_master_rfa ON tbl_trans_rfa.req_rfaid = tbl_master_rfa.rfa_id LEFT OUTER JOIN tbl_master_user tbl_master_user_2 ON tbl_trans_rfa.req_preapprover = tbl_master_user_2.user_id  WHERE (tbl_trans_rfa.req_AprRejNo is null) AND (tbl_trans_rfa.req_preapprover is null) and (tbl_trans_rfa.req_currstatus = 0) and (req_userid=" + Session["userid"].ToString() + ")", "DISTINCT  tbl_trans_rfadetails.rtd_reqnumber,rfa_shortname as [Request Id], tbl_master_user_2.user_name AS [Previous Approver], CASE tbl_trans_rfa.req_prestatus WHEN 0 THEN 'Unread' WHEN 1 THEN 'Approved' WHEN 3 THEN 'Approved With MOdification' WHEN 2 THEN 'Reject' WHEN 4 THEN 'Forward to Higher Authority' WHEN 5 THEN 'Pendding' END AS [Previous Status], tbl_trans_rfa.req_prenote AS [Previous Note], tbl_master_user_1.user_name AS [current Approver], CASE tbl_trans_rfa.req_currstatus WHEN 0 THEN 'Unread' WHEN 1 THEN 'Approved' WHEN 3 THEN 'Approved With MOdification' WHEN 2 THEN 'Reject' WHEN 4 THEN 'Forward to Hidher Authority' WHEN 5 THEN 'Pendding' END AS [current Status], tbl_trans_rfa.req_currnote AS [current Note], req_aprrejno AS [Approval No], tbl_trans_rfa.req_description AS mesage", null, "rtd_reqnumber DESC");
            }
            else if (param == "1")
            {
                dt = oDBEngine.GetDataTable("tbl_trans_rfa INNER JOIN tbl_trans_rfadetails ON tbl_trans_rfa.req_id = tbl_trans_rfadetails.rtd_reqnumber INNER JOIN tbl_master_user tbl_master_user_1 ON tbl_trans_rfa.req_currapprover = tbl_master_user_1.user_id INNER JOIN tbl_master_rfa ON tbl_trans_rfa.req_rfaid = tbl_master_rfa.rfa_id LEFT OUTER JOIN tbl_master_user tbl_master_user_2 ON tbl_trans_rfa.req_preapprover = tbl_master_user_2.user_id WHERE (tbl_trans_rfa.req_AprRejNo is null) AND ((tbl_trans_rfa.req_preapprover > 0) or (tbl_trans_rfa.req_currstatus > 0))  and (req_userid=" + Session["userid"].ToString() + ")", "DISTINCT  tbl_trans_rfadetails.rtd_reqnumber,rfa_shortname as [Request Id], tbl_master_user_2.user_name AS [Previous Approver], CASE tbl_trans_rfa.req_prestatus WHEN 0 THEN 'Unread' WHEN 1 THEN 'Approved' WHEN 3 THEN 'Approved With MOdification' WHEN 2 THEN 'Reject' WHEN 4 THEN 'Forward to Higher Authority' WHEN 5 THEN 'Pendding' END AS [Previous Status], tbl_trans_rfa.req_prenote AS [Previous Note], tbl_master_user_1.user_name AS [current Approver], CASE tbl_trans_rfa.req_currstatus WHEN 0 THEN 'Unread' WHEN 1 THEN 'Approved' WHEN 3 THEN 'Approved With MOdification' WHEN 2 THEN 'Reject' WHEN 4 THEN 'Forward to Hidher Authority' WHEN 5 THEN 'Pendding' END AS [current Status], tbl_trans_rfa.req_currnote AS [current Note], req_aprrejno AS [Approval No], tbl_trans_rfa.req_description AS mesage", null, "rtd_reqnumber DESC");
            }
            else if (param == "2")
            {
                dt = oDBEngine.GetDataTable("tbl_trans_rfa INNER JOIN tbl_trans_rfadetails ON tbl_trans_rfa.req_id = tbl_trans_rfadetails.rtd_reqnumber INNER JOIN tbl_master_user tbl_master_user_1 ON tbl_trans_rfa.req_currapprover = tbl_master_user_1.user_id INNER JOIN tbl_master_rfa ON tbl_trans_rfa.req_rfaid = tbl_master_rfa.rfa_id LEFT OUTER JOIN tbl_master_user tbl_master_user_2 ON tbl_trans_rfa.req_preapprover = tbl_master_user_2.user_id", "DISTINCT tbl_trans_rfadetails.rtd_reqnumber,rfa_shortname as [Request Id], tbl_master_user_2.user_name AS [Previous Approver], CASE tbl_trans_rfa.req_prestatus WHEN 0 THEN 'Unread' WHEN 1 THEN 'Approved' WHEN 3 THEN 'Approved With MOdification' WHEN 2 THEN 'Reject' WHEN 4 THEN 'Forward to Higher Authority' WHEN 5 THEN 'Pendding' WHEN 6 THEN 'Recorded' END AS [Previous Status], tbl_trans_rfa.req_prenote AS [Previous Note], tbl_master_user_1.user_name AS [current Approver], CASE tbl_trans_rfa.req_currstatus WHEN 0 THEN 'Unread' WHEN 1 THEN 'Approved' WHEN 3 THEN 'Approved With MOdification' WHEN 2 THEN 'Reject' WHEN 4 THEN 'Forward to Hidher Authority' WHEN 5 THEN 'Pendding' END AS [current Status], tbl_trans_rfa.req_currnote AS [current Note], req_aprrejno AS [Approval No], tbl_trans_rfa.req_description AS mesage", " (tbl_trans_rfa.req_AprRejNo <> N'') AND (tbl_trans_rfa.req_currstatus <> 2) and (req_userid=" + Session["userid"].ToString() + ")", "rtd_reqnumber DESC");
            }
            else
            {
                dt = oDBEngine.GetDataTable("tbl_trans_rfa INNER JOIN tbl_trans_rfadetails ON tbl_trans_rfa.req_id = tbl_trans_rfadetails.rtd_reqnumber INNER JOIN tbl_master_user tbl_master_user_1 ON tbl_trans_rfa.req_currapprover = tbl_master_user_1.user_id INNER JOIN tbl_master_rfa ON tbl_trans_rfa.req_rfaid = tbl_master_rfa.rfa_id LEFT OUTER JOIN tbl_master_user tbl_master_user_2 ON tbl_trans_rfa.req_preapprover = tbl_master_user_2.user_id", "DISTINCT tbl_trans_rfadetails.rtd_reqnumber,rfa_shortname as [Request Id], tbl_master_user_2.user_name AS [Previous Approver], CASE tbl_trans_rfa.req_prestatus WHEN 0 THEN 'Unread' WHEN 1 THEN 'Approved' WHEN 3 THEN 'Approved With MOdification' WHEN 2 THEN 'Reject' WHEN 4 THEN 'Forward to Higher Authority' WHEN 5 THEN 'Pendding' WHEN 6 THEN 'Recorded' END AS [Previous Status], tbl_trans_rfa.req_prenote AS [Previous Note], tbl_master_user_1.user_name AS [current Approver], CASE tbl_trans_rfa.req_currstatus WHEN 0 THEN 'Unread' WHEN 1 THEN 'Approved' WHEN 3 THEN 'Approved With MOdification' WHEN 2 THEN 'Reject' WHEN 4 THEN 'Forward to Hidher Authority' WHEN 5 THEN 'Pendding' END AS [current Status], tbl_trans_rfa.req_currnote AS [current Note], req_aprrejno AS [Approval No], tbl_trans_rfa.req_description AS mesage", " (tbl_trans_rfa.req_AprRejNo <> N'') AND (tbl_trans_rfa.req_currstatus = 2) and (req_userid=" + Session["userid"].ToString() + ")", "rtd_reqnumber DESC");
            }
            if (dt.Rows.Count > 0)
            {
                grd_request.DataSource = dt;
                grd_request.DataBind();
            }
            else
            {
                grd_request.DataSource = dt;
                grd_request.DataBind();
            }
            dt.Dispose();
        }
    }
}