using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Management.ToolsUtilities
{
    public partial class management_utilities_frmrfa_transaction : System.Web.UI.Page, ICallbackEventHandler
    {
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        // DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
       // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        string data = "";
        int current_level = 0;
        int total_leval = 0;
        string rfa_id = "";
        string set_rule = "";
        string status = "False";
        string HREC = "";
        string current_approver = "";
        string Previous_approver = "";
        int k = 0;
        public string pageAccess = "";
        //DBEngine oDBEngine = new DBEngine(string.Empty);
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
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            ////this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            if (!IsPostBack)
            {
                BindIncomingRfa();
                lst_requesttype.Attributes.Add("onchange", "GridBind()");
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");
            }
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
        }
        public void BindIncomingRfa()
        {
            DataTable dt = oDBEngine.GetDataTable("tbl_trans_rfa INNER JOIN tbl_master_rfa ON tbl_trans_rfa.req_rfaid = tbl_master_rfa.rfa_id INNER JOIN tbl_master_user ON tbl_trans_rfa.req_userid = tbl_master_user.user_id LEFT OUTER JOIN tbl_master_user tbl_master_user_1 ON tbl_trans_rfa.req_preapprover = tbl_master_user_1.user_id", "DISTINCT  tbl_master_rfa.rfa_shortname as shortname , tbl_master_user.user_name as createuser, tbl_trans_rfa.req_id req_id, convert(varchar(11),tbl_trans_rfa.req_startdate,113) as createdate,convert(varchar(11),tbl_trans_rfa.req_closedate,113) as closedate, CASE tbl_trans_rfa.req_prestatus WHEN 0 THEN 'Unread' WHEN 1 THEN 'Approved' WHEN 3 THEN 'Approved With MOdification' WHEN 2 THEN 'Reject' WHEN 4 THEN 'Forward to Hidher Authority' WHEN 5 THEN 'Pendding' END + ' [ ' + tbl_trans_rfa.req_prenote + '] '  as status, tbl_master_user_1.user_name as approver", " req_currapprover=" + Session["userid"].ToString() + " and req_currstatus=0");
            grd_incomingrfa.DataSource = dt;
            grd_incomingrfa.DataBind();
        }
        #region ICallbackEventHandler Members

        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }

        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            data = "";
            string eid = eventArgument.ToString();
            string[] idlist = eid.Split('~');
            #region Save
            if (idlist[0] == "Save")
            {
                int NoofRowsAffected = 0;
                if (idlist[3] == "5" || idlist[3] == "0")
                {
                    NoofRowsAffected = oDBEngine.SetFieldValue("tbl_trans_rfa", "req_currnote='" + idlist[4] + "',req_currstatus=5,LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Session["userid"].ToString() + "'", " req_id='" + idlist[5] + "'");
                }
                DataTable dt = new DataTable();
                dt = oDBEngine.GetDataTable("tbl_trans_rfa INNER JOIN tbl_master_rfa ON tbl_trans_rfa.req_rfaid = tbl_master_rfa.rfa_id", "req_currapprover,req_totallevel,req_currentlevel,tbl_trans_rfa.req_description, tbl_master_rfa.rfa_id, tbl_master_rfa.rfa_shortname, tbl_trans_rfa.req_currstatus, tbl_trans_rfa.req_currnote", " req_id='" + idlist[5] + "'");
                if (dt.Rows.Count > 0)
                {
                    current_level = Convert.ToInt32(dt.Rows[0]["req_currentlevel"].ToString());
                    total_leval = Convert.ToInt32(dt.Rows[0]["req_totallevel"].ToString());
                    rfa_id = dt.Rows[0]["rfa_id"].ToString();
                    Previous_approver = dt.Rows[0]["req_currapprover"].ToString();
                }
                if (current_level >= 1 && total_leval > current_level)
                {
                    for (k = 0; k < (total_leval - (current_level + 1)); k++)
                    {
                        string[,] hrec1 = oDBEngine.GetFieldValue("tbl_master_rfa", "rfa_target" + current_level + (k + 1), "rfa_id='" + rfa_id + "'", 1);
                        if (hrec1[0, 0] != "n")
                        {
                            HREC = hrec1[0, 0];
                        }
                        string[,] setrule1 = oDBEngine.GetFieldValue("tbl_master_rfa", "rfa_rule" + current_level + (k + 1), "rfa_id='" + rfa_id + "'", 1);
                        if (setrule1[0, 0] != "n")
                        {
                            set_rule = setrule1[0, 0];
                        }
                        string[] rules = set_rule.Split(',');
                        if (rules.Length > 1)
                        {
                            foreach (string x in rules)
                            {
                                if (x == idlist[3])
                                {
                                    if (next_user() != Session["userid"].ToString())
                                    {
                                        status = "True";
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (set_rule == idlist[3])
                            {
                                if (next_user() != Session["userid"].ToString())
                                {
                                    status = "True";
                                }
                            }
                        }
                    }
                    if (status == "True")
                    {
                        string unique_id = "";
                        string[,] uniqueid = oDBEngine.GetFieldValue("tbl_trans_rfadetails", "TOP 1 rtd_id", " rtd_reqnumber='" + idlist[5] + "'", 1);
                        if (uniqueid[0, 0] != "n")
                        {
                            unique_id = uniqueid[0, 0];
                        }
                        NoofRowsAffected = oDBEngine.SetFieldValue("tbl_trans_rfadetails", "rtd_readdate='" + oDBEngine.GetDate().ToString() + "',rtd_status='" + idlist[3] + "',rtd_note='" + idlist[4] + "',rtd_nexapprovalid='" + current_approver + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Session["userid"].ToString() + "'", " rtd_reqnumber='" + idlist[5] + "' and rtd_id='" + unique_id + "' and rtd_appuser='" + Session["userid"].ToString() + "'");
                        NoofRowsAffected = oDBEngine.SetFieldValue("tbl_trans_rfa", "req_currentLevel='" + (current_level + (k + 1)) + "',req_preapprover='" + Previous_approver + "',req_prestatus='" + idlist[3] + "',req_prenote='" + idlist[4] + "',req_currapprover='" + current_approver + "',req_currstatus=0,LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Session["userid"].ToString() + "'", " req_id='" + idlist[5] + "'");
                        string detail_request_id = oDBEngine.GetInternalId_Req("RFA" + Format(oDBEngine.GetDate().Month.ToString()) + oDBEngine.GetDate().Year.ToString().Substring(2, 2), "tbl_trans_rfadetails", "rtd_id", "rtd_id");
                        NoofRowsAffected = oDBEngine.InsurtFieldValue("tbl_trans_rfadetails", "rtd_id,rtd_reqnumber,rtd_appuser,rtd_status,CreateDate,CreateUser", "'" + detail_request_id + "','" + idlist[5] + "','" + current_approver + "',0,'" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'");
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
                                string id = "";
                                string[,] id1 = oDBEngine.GetFieldValue("tbl_trans_rfa", "TOP 1 req_userid", " req_id='" + idlist[5] + "'", 1);
                                if (id1[0, 0] != "n")
                                {
                                    id = id1[0, 0];
                                }
                                string[,] RequestBy = oDBEngine.GetFieldValue("tbl_master_contact INNER JOIN tbl_master_user ON tbl_master_contact.cnt_internalId = tbl_master_user.user_contactId", "Top 1 IsNull(tbl_master_contact.cnt_firstName,'')+' '+IsNull(tbl_master_contact.cnt_middleName,'')+' '+IsNull(tbl_master_contact.cnt_lastName,'')", " tbl_master_user.user_id='" + id + "'", 1);
                                if (RequestBy[0, 0] != "n")
                                {
                                    Request_By = RequestBy[0, 0];
                                }
                                string Subject = "";
                                string[,] Subject1 = oDBEngine.GetFieldValue("tbl_master_rfa INNER JOIN tbl_trans_rfa ON tbl_master_rfa.rfa_id = tbl_trans_rfa.req_rfaid", "DISTINCT (tbl_master_rfa.rfa_shortname)", " tbl_trans_rfa.req_id='" + idlist[5] + "'", 1);
                                if (Subject1[0, 0] != "n")
                                {
                                    Subject = Subject1[0, 0];
                                }
                                DataTable dtTemp = oDBEngine.GetDataTable("tbl_trans_rfadetails INNER JOIN tbl_master_user ON tbl_trans_rfadetails.rtd_appuser = tbl_master_user.user_id LEFT OUTER JOIN tbl_master_user tbl_master_user_1 ON tbl_trans_rfadetails.rtd_nexapprovalid = tbl_master_user_1.user_id", "tbl_trans_rfadetails.rtd_id AS ID, tbl_master_user.user_name AS [Approver Name], tbl_trans_rfadetails.rtd_readdate AS [Read Date],CASE tbl_trans_rfadetails.rtd_status WHEN 0 THEN 'Unread' WHEN 1 THEN 'Approved' WHEN 3 THEN 'Approved With MOdification' WHEN 2 THEN 'Reject' WHEN 4 THEN 'Forward to Higher Authority' WHEN 5 THEN 'Pendding' END AS [Previous Status], tbl_trans_rfadetails.rtd_note AS Note,tbl_master_user_1.user_name AS [Next Approver]", " tbl_trans_rfadetails.rtd_reqnumber='" + idlist[5] + "'", "rtd_id");
                                string Body = "<table border='0' cellpadding='3' cellspacing='3' style='width: 100%; font-weight: normal; font-size: 8pt; text-transform: capitalize; color: black; font-style: normal; font-family: Verdana;'><tr><td colspan='3' style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top; border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite; text-align: left; font-weight: bold;'> Request By :-  " + Request_By + "</td></tr><tr><td colspan='3' style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top; border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite; text-align: left'> Request :- " + idlist[2] + "</td></tr>";
                                int Q = 0;
                                for (int k1 = 0; k1 < dtTemp.Rows.Count; k1++)
                                {
                                    if (Q == 0)
                                    {
                                        Body += "<tr><td style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top; border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite; text-align: left; font-weight: bold;'> [ Approver Name ] </td><td style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top; border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite; text-align: left; font-weight: bold; '> [ Previous Status] </td><td style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top;border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite;text-align: left; font-weight: bold;'>[ Note ]</td></tr>";
                                        Q = Q + 1;
                                    }
                                    Body += "<tr><td style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top; border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite; text-align: left'>" + dtTemp.Rows[k1]["Approver Name"].ToString() + "</td><td style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top; border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite; text-align: left'> [ " + dtTemp.Rows[k1]["Previous Status"].ToString() + " ] </td><td style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top;border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite;text-align: left'>[ " + dtTemp.Rows[k1]["Note"].ToString() + " ]</td></tr>";
                                }
                                Body += "</table>";
                                objConverter.SendMail(Mail_From, mail_To, Subject, Body);
                                NoofRowsAffected = oDBEngine.InsurtFieldValue("tbl_trans_email", "hem_senderemail,hem_mailmsg,hem_temid,hem_sendertype,hem_activityid,CreateDate,CreateUser", "'" + mail_To + "','Nothing','0','0','0','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'");
                            }
                        }
                        catch
                        {
                        }
                    }
                    if (status == "False")
                    {
                        string final_id = "";
                        string[,] finalid = oDBEngine.GetFieldValue("tbl_trans_rfadetails", "rtd_id", " rtd_reqnumber='" + idlist[5] + "' and rtd_appuser='" + Session["userid"].ToString() + "'", 1);
                        if (finalid[0, 0] != "n")
                        {
                            final_id = finalid[0, 0];
                        }
                        NoofRowsAffected = oDBEngine.SetFieldValue("tbl_trans_rfa", "req_currnote='" + idlist[4] + "',req_currstatus='" + idlist[3] + "',req_aprrejno='" + final_id + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Session["userid"].ToString() + "'", " req_id='" + idlist[5] + "'");
                        string unique_id = "";
                        string[,] uniqueid = oDBEngine.GetFieldValue("tbl_trans_rfadetails", "TOP 1 rtd_id", " rtd_reqnumber='" + idlist[5] + "'", 1, "rtd_id DESC");
                        if (uniqueid[0, 0] != "n")
                        {
                            unique_id = uniqueid[0, 0];
                        }
                        NoofRowsAffected = oDBEngine.SetFieldValue("tbl_trans_rfadetails", "rtd_readdate='" + oDBEngine.GetDate().ToString() + "',rtd_status='" + idlist[3] + "',rtd_note='" + idlist[4] + "',rtd_nexapprovalid=0,LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Session["userid"].ToString() + "'", " rtd_reqnumber='" + idlist[5] + "' and rtd_id='" + unique_id + "' and rtd_appuser='" + Session["userid"].ToString() + "'");
                        try
                        {
                            string mTo = "";
                            string[,] mto1 = oDBEngine.GetFieldValue("tbl_trans_rfa", "TOP 1 req_userid", " req_id='" + idlist[5] + "'", 1);
                            if (mto1[0, 0] != "n")
                            {
                                mTo = mto1[0, 0];
                            }
                            string mail_To = mTo;
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
                                string id = "";
                                string[,] id1 = oDBEngine.GetFieldValue("tbl_trans_rfa", "TOP 1 req_userid", " req_id='" + idlist[5] + "'", 1);
                                if (id1[0, 0] != "n")
                                {
                                    id = id1[0, 0];
                                }
                                string[,] RequestBy = oDBEngine.GetFieldValue("tbl_master_contact INNER JOIN tbl_master_user ON tbl_master_contact.cnt_internalId = tbl_master_user.user_contactId", "Top 1 IsNull(tbl_master_contact.cnt_firstName,'')+' '+IsNull(tbl_master_contact.cnt_middleName,'')+' '+IsNull(tbl_master_contact.cnt_lastName,'')", " tbl_master_user.user_id='" + id + "'", 1);
                                if (RequestBy[0, 0] != "n")
                                {
                                    Request_By = RequestBy[0, 0];
                                }
                                string Subject = "";
                                string[,] Subject1 = oDBEngine.GetFieldValue("tbl_master_rfa INNER JOIN tbl_trans_rfa ON tbl_master_rfa.rfa_id = tbl_trans_rfa.req_rfaid", "DISTINCT (tbl_master_rfa.rfa_shortname)", " tbl_trans_rfa.req_id='" + idlist[5] + "'", 1);
                                if (Subject1[0, 0] != "n")
                                {
                                    Subject = Subject1[0, 0];
                                }
                                DataTable dtTemp = oDBEngine.GetDataTable("tbl_trans_rfadetails INNER JOIN tbl_master_user ON tbl_trans_rfadetails.rtd_appuser = tbl_master_user.user_id LEFT OUTER JOIN tbl_master_user tbl_master_user_1 ON tbl_trans_rfadetails.rtd_nexapprovalid = tbl_master_user_1.user_id", "tbl_trans_rfadetails.rtd_id AS ID, tbl_master_user.user_name AS [Approver Name], tbl_trans_rfadetails.rtd_readdate AS [Read Date],CASE tbl_trans_rfadetails.rtd_status WHEN 0 THEN 'Unread' WHEN 1 THEN 'Approved' WHEN 3 THEN 'Approved With MOdification' WHEN 2 THEN 'Reject' WHEN 4 THEN 'Forward to Higher Authority' WHEN 5 THEN 'Pendding' END AS [Previous Status], tbl_trans_rfadetails.rtd_note AS Note,tbl_master_user_1.user_name AS [Next Approver]", " tbl_trans_rfadetails.rtd_reqnumber='" + idlist[5] + "'", "rtd_id");
                                string Body = "<table border='0' cellpadding='3' cellspacing='3' style='width: 100%; font-weight: normal; font-size: 8pt; text-transform: capitalize; color: black; font-style: normal; font-family: Verdana;'><tr><td colspan='3' style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top; border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite; text-align: left; font-weight: bold;'> Request By :-  " + Request_By + "</td></tr><tr><td colspan='3' style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top; border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite; text-align: left'> Request :- " + idlist[2] + "</td></tr>";
                                int Q = 0;
                                for (int k1 = 0; k1 < dtTemp.Rows.Count; k1++)
                                {
                                    if (Q == 0)
                                    {
                                        Body += "<tr><td style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top; border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite; text-align: left; font-weight: bold;'> [ Approver Name ] </td><td style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top; border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite; text-align: left; font-weight: bold; '> [ Previous Status] </td><td style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top;border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite;text-align: left; font-weight: bold;'>[ Note ]</td></tr>";
                                        Q = Q + 1;
                                    }
                                    Body += "<tr><td style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top; border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite; text-align: left'>" + dtTemp.Rows[k1]["Approver Name"].ToString() + "</td><td style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top; border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite; text-align: left'> [ " + dtTemp.Rows[k1]["Previous Status"].ToString() + " ] </td><td style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top;border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite;text-align: left'>[ " + dtTemp.Rows[k1]["Note"].ToString() + " ]</td></tr>";
                                }
                                Body += "</table>";
                                objConverter.SendMail(Mail_From, mail_To, Subject, Body);
                                NoofRowsAffected = oDBEngine.InsurtFieldValue("tbl_trans_email", "hem_senderemail,hem_mailmsg,hem_temid,hem_sendertype,hem_activityid,CreateDate,CreateUser", "'" + mail_To + "','Nothing','0','0','0','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'");
                            }
                        }
                        catch
                        {
                        }
                    }
                    if (total_leval == current_level && (Convert.ToInt32(idlist[3]) > 0 || Convert.ToInt32(idlist[3]) < 5))
                    {
                        string final_id = "";
                        string[,] finalid = oDBEngine.GetFieldValue("tbl_trans_rfadetails", "rtd_id", " rtd_reqnumber='" + idlist[5] + "' and rtd_appuser='" + Session["userid"].ToString() + "'", 1);
                        if (finalid[0, 0] != "n")
                        {
                            final_id = finalid[0, 0];
                        }
                        NoofRowsAffected = oDBEngine.SetFieldValue("tbl_trans_rfa", "req_currnote='" + idlist[4] + "',req_currstatus='" + idlist[3] + "',req_aprrejno='" + final_id + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Session["userid"].ToString() + "'", " req_id='" + idlist[5] + "'");
                        string unique_id = "";
                        string[,] uniqueid = oDBEngine.GetFieldValue("tbl_trans_rfadetails", "TOP 1 rtd_id", " rtd_reqnumber='" + idlist[5] + "'", 1, "rtd_id DESC");
                        if (uniqueid[0, 0] != "n")
                        {
                            unique_id = uniqueid[0, 0];
                        }
                        NoofRowsAffected = oDBEngine.SetFieldValue("tbl_trans_rfadetails", "rtd_readdate='" + oDBEngine.GetDate().ToString() + "',rtd_status='" + idlist[3] + "',rtd_note='" + idlist[4] + "',rtd_nexapprovalid=0,LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Session["userid"].ToString() + "'", " rtd_reqnumber='" + idlist[5] + "' and rtd_id='" + unique_id + "' and rtd_appuser='" + Session["userid"].ToString() + "'");
                        try
                        {
                            string mTo = "";
                            string[,] mto1 = oDBEngine.GetFieldValue("tbl_trans_rfa", "TOP 1 req_userid", " req_id='" + idlist[5] + "'", 1);
                            if (mto1[0, 0] != "n")
                            {
                                mTo = mto1[0, 0];
                            }
                            string mail_To = mTo;
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
                                string id = "";
                                string[,] id1 = oDBEngine.GetFieldValue("tbl_trans_rfa", "TOP 1 req_userid", " req_id='" + idlist[5] + "'", 1);
                                if (id1[0, 0] != "n")
                                {
                                    id = id1[0, 0];
                                }
                                string[,] RequestBy = oDBEngine.GetFieldValue("tbl_master_contact INNER JOIN tbl_master_user ON tbl_master_contact.cnt_internalId = tbl_master_user.user_contactId", "Top 1 IsNull(tbl_master_contact.cnt_firstName,'')+' '+IsNull(tbl_master_contact.cnt_middleName,'')+' '+IsNull(tbl_master_contact.cnt_lastName,'')", " tbl_master_user.user_id='" + id + "'", 1);
                                if (RequestBy[0, 0] != "n")
                                {
                                    Request_By = RequestBy[0, 0];
                                }
                                string Subject = "";
                                string[,] Subject1 = oDBEngine.GetFieldValue("tbl_master_rfa INNER JOIN tbl_trans_rfa ON tbl_master_rfa.rfa_id = tbl_trans_rfa.req_rfaid", "DISTINCT (tbl_master_rfa.rfa_shortname)", " tbl_trans_rfa.req_id='" + idlist[5] + "'", 1);
                                if (Subject1[0, 0] != "n")
                                {
                                    Subject = Subject1[0, 0];
                                }
                                DataTable dtTemp = oDBEngine.GetDataTable("tbl_trans_rfadetails INNER JOIN tbl_master_user ON tbl_trans_rfadetails.rtd_appuser = tbl_master_user.user_id LEFT OUTER JOIN tbl_master_user tbl_master_user_1 ON tbl_trans_rfadetails.rtd_nexapprovalid = tbl_master_user_1.user_id", "tbl_trans_rfadetails.rtd_id AS ID, tbl_master_user.user_name AS [Approver Name], tbl_trans_rfadetails.rtd_readdate AS [Read Date],CASE tbl_trans_rfadetails.rtd_status WHEN 0 THEN 'Unread' WHEN 1 THEN 'Approved' WHEN 3 THEN 'Approved With MOdification' WHEN 2 THEN 'Reject' WHEN 4 THEN 'Forward to Higher Authority' WHEN 5 THEN 'Pendding' END AS [Previous Status], tbl_trans_rfadetails.rtd_note AS Note,tbl_master_user_1.user_name AS [Next Approver]", " tbl_trans_rfadetails.rtd_reqnumber='" + idlist[5] + "'", "rtd_id");
                                string Body = "<table border='0' cellpadding='3' cellspacing='3' style='width: 100%; font-weight: normal; font-size: 8pt; text-transform: capitalize; color: black; font-style: normal; font-family: Verdana;'><tr><td colspan='3' style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top; border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite; text-align: left; font-weight: bold;'> Request By :-  " + Request_By + "</td></tr><tr><td colspan='3' style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top; border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite; text-align: left'> Request :- " + idlist[2] + "</td></tr>";
                                int Q = 0;
                                for (int k1 = 0; k1 < dtTemp.Rows.Count; k1++)
                                {
                                    if (Q == 0)
                                    {
                                        Body += "<tr><td style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top; border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite; text-align: left; font-weight: bold;'> [ Approver Name ] </td><td style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top; border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite; text-align: left; font-weight: bold; '> [ Previous Status] </td><td style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top;border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite;text-align: left; font-weight: bold;'>[ Note ]</td></tr>";
                                        Q = Q + 1;
                                    }
                                    Body += "<tr><td style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top; border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite; text-align: left'>" + dtTemp.Rows[k1]["Approver Name"].ToString() + "</td><td style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top; border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite; text-align: left'> [ " + dtTemp.Rows[k1]["Previous Status"].ToString() + " ] </td><td style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top;border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite;text-align: left'>[ " + dtTemp.Rows[k1]["Note"].ToString() + " ]</td></tr>";
                                }
                                Body += "</table>";
                                objConverter.SendMail(Mail_From, mail_To, Subject, Body);
                                NoofRowsAffected = oDBEngine.InsurtFieldValue("tbl_trans_email", "hem_senderemail,hem_mailmsg,hem_temid,hem_sendertype,hem_activityid,CreateDate,CreateUser", "'" + mail_To + "','Nothing','0','0','0','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'");
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                data = "Save~Y";
            }
            #endregion
            #region Read
            if (idlist[0] == "Read")
            {
                try
                {
                    int NoofAffected = 0;
                    NoofAffected = oDBEngine.SetFieldValue("tbl_trans_rfa", "req_currstatus=5,LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Session["userid"].ToString() + "'", " req_id='" + idlist[1] + "'");
                    DataTable dt = oDBEngine.GetDataTable("tbl_trans_rfa INNER JOIN tbl_master_rfa ON tbl_trans_rfa.req_rfaid = tbl_master_rfa.rfa_id", "req_currapprover,req_totallevel,req_currentlevel,tbl_trans_rfa.req_description, tbl_master_rfa.rfa_id, tbl_master_rfa.rfa_shortname, tbl_trans_rfa.req_currstatus, tbl_trans_rfa.req_currnote", " req_id='" + idlist[1] + "'");
                    string topic = dt.Rows[0]["rfa_shortname"].ToString();
                    string content = dt.Rows[0]["req_description"].ToString();
                    string note = dt.Rows[0]["req_currnote"].ToString();
                    string status = dt.Rows[0]["req_currstatus"].ToString();
                    string Clevel = dt.Rows[0]["req_currentlevel"].ToString();
                    string Tlevel = dt.Rows[0]["req_totallevel"].ToString();
                    NoofAffected = oDBEngine.SetFieldValue("tbl_trans_rfadetails", "rtd_readdate='" + oDBEngine.GetDate().ToString() + "',rtd_status=5,rtd_note='" + note + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Session["userid"].ToString() + "'", " rtd_reqnumber='" + idlist[1] + "' and rtd_appuser='" + Session["userid"].ToString() + "'");
                    data = "Read" + "~" + topic + "~" + content + "~" + note + "~" + status + "~" + Clevel + "~" + Tlevel + "~" + idlist[1];
                }
                catch
                {
                }
            }
            #endregion
            #region Modify
            if (idlist[0] == "Modify")
            {
                int NoofAffected = 0;
                NoofAffected = oDBEngine.SetFieldValue("tbl_trans_rfa", "req_currstatus=5,LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Session["userid"].ToString() + "'", " req_id='" + idlist[1] + "'");
                DataTable dt = oDBEngine.GetDataTable("tbl_trans_rfa INNER JOIN tbl_master_rfa ON tbl_trans_rfa.req_rfaid = tbl_master_rfa.rfa_id", "req_currapprover,req_totallevel,req_currentlevel,tbl_trans_rfa.req_description, tbl_master_rfa.rfa_id, tbl_master_rfa.rfa_shortname, tbl_trans_rfa.req_currstatus, tbl_trans_rfa.req_currnote", " req_id='" + idlist[1] + "'");
                if (dt != null && dt.Rows.Count > 0)
                {
                    string topic = Convert.ToString(dt.Rows[0]["rfa_shortname"]);
                    string content = Convert.ToString(dt.Rows[0]["req_description"]);
                    string note = Convert.ToString(dt.Rows[0]["req_currnote"]);
                    string status = Convert.ToString(dt.Rows[0]["req_currstatus"]);
                    string Clevel = Convert.ToString(dt.Rows[0]["req_currentlevel"]);
                    string Tlevel = Convert.ToString(dt.Rows[0]["req_totallevel"]);
                    data = "Modify" + "~" + topic + "~" + content + "~" + note + "~" + status + "~" + Clevel + "~" + Tlevel + "~" + idlist[1];
                }
            }
            #endregion
            #region Record
            if (idlist[0] == "Record")
            {
                int NoofRowsAffected = 0;
                string final_id = "";
                string[,] finalid = oDBEngine.GetFieldValue("tbl_trans_rfadetails", "rtd_id", " rtd_reqnumber='" + idlist[5] + "' and rtd_appuser='" + Session["userid"].ToString() + "'", 1);
                if (finalid[0, 0] != "n")
                {
                    final_id = finalid[0, 0];
                }
                NoofRowsAffected = oDBEngine.SetFieldValue("tbl_trans_rfa", "req_currnote='" + idlist[4] + "',req_currstatus='6',req_aprrejno='" + final_id + "',LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Session["userid"].ToString() + "'", " req_id='" + idlist[5] + "'");
                string unique_id = "";
                string[,] uniqueid = oDBEngine.GetFieldValue("tbl_trans_rfadetails", "TOP 1 rtd_id", " rtd_reqnumber='" + idlist[5] + "'", 1, "rtd_id DESC");
                if (uniqueid[0, 0] != "n")
                {
                    unique_id = uniqueid[0, 0];
                }
                NoofRowsAffected = oDBEngine.SetFieldValue("tbl_trans_rfadetails", "rtd_readdate='" + oDBEngine.GetDate().ToString() + "',rtd_status='6',rtd_note='" + idlist[4] + "',rtd_nexapprovalid=0,LastModifyDate='" + oDBEngine.GetDate().ToString() + "',LastModifyUser='" + Session["userid"].ToString() + "'", " rtd_reqnumber='" + idlist[5] + "' and rtd_id='" + unique_id + "' and rtd_appuser='" + Session["userid"].ToString() + "'");
                try
                {
                    string mTo = "";
                    string[,] mto1 = oDBEngine.GetFieldValue("tbl_trans_rfa", "TOP 1 req_userid", " req_id='" + idlist[5] + "'", 1);
                    if (mto1[0, 0] != "n")
                    {
                        mTo = mto1[0, 0];
                    }
                    string mail_To = mTo;
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
                        string id = "";
                        string[,] id1 = oDBEngine.GetFieldValue("tbl_trans_rfa", "TOP 1 req_userid", " req_id='" + idlist[5] + "'", 1);
                        if (id1[0, 0] != "n")
                        {
                            id = id1[0, 0];
                        }
                        string[,] RequestBy = oDBEngine.GetFieldValue("tbl_master_contact INNER JOIN tbl_master_user ON tbl_master_contact.cnt_internalId = tbl_master_user.user_contactId", "Top 1 IsNull(tbl_master_contact.cnt_firstName,'')+' '+IsNull(tbl_master_contact.cnt_middleName,'')+' '+IsNull(tbl_master_contact.cnt_lastName,'')", " tbl_master_user.user_id='" + id + "'", 1);
                        if (RequestBy[0, 0] != "n")
                        {
                            Request_By = RequestBy[0, 0];
                        }
                        string Subject = "";
                        string[,] Subject1 = oDBEngine.GetFieldValue("tbl_master_rfa INNER JOIN tbl_trans_rfa ON tbl_master_rfa.rfa_id = tbl_trans_rfa.req_rfaid", "DISTINCT (tbl_master_rfa.rfa_shortname)", " tbl_trans_rfa.req_id='" + idlist[5] + "'", 1);
                        if (Subject1[0, 0] != "n")
                        {
                            Subject = Subject1[0, 0];
                        }
                        DataTable dtTemp = oDBEngine.GetDataTable("tbl_trans_rfadetails INNER JOIN tbl_master_user ON tbl_trans_rfadetails.rtd_appuser = tbl_master_user.user_id LEFT OUTER JOIN tbl_master_user tbl_master_user_1 ON tbl_trans_rfadetails.rtd_nexapprovalid = tbl_master_user_1.user_id", "tbl_trans_rfadetails.rtd_id AS ID, tbl_master_user.user_name AS [Approver Name], tbl_trans_rfadetails.rtd_readdate AS [Read Date],CASE tbl_trans_rfadetails.rtd_status WHEN 0 THEN 'Unread' WHEN 1 THEN 'Approved' WHEN 3 THEN 'Approved With MOdification' WHEN 2 THEN 'Reject' WHEN 4 THEN 'Forward to Higher Authority' WHEN 5 THEN 'Pendding' END AS [Previous Status], tbl_trans_rfadetails.rtd_note AS Note,tbl_master_user_1.user_name AS [Next Approver]", " tbl_trans_rfadetails.rtd_reqnumber='" + idlist[5] + "'", "rtd_id");
                        string Body = "<table border='0' cellpadding='3' cellspacing='3' style='width: 100%; font-weight: normal; font-size: 8pt; text-transform: capitalize; color: black; font-style: normal; font-family: Verdana;'><tr><td colspan='3' style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top; border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite; text-align: left; font-weight: bold;'> Request By :-  " + Request_By + "</td></tr><tr><td colspan='3' style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top; border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite; text-align: left'> Request :- " + idlist[2] + "</td></tr>";
                        int Q = 0;
                        for (int k1 = 0; k1 < dtTemp.Rows.Count; k1++)
                        {
                            if (Q == 0)
                            {
                                Body += "<tr><td style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top; border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite; text-align: left; font-weight: bold;'> [ Approver Name ] </td><td style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top; border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite; text-align: left; font-weight: bold; '> [ Previous Status] </td><td style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top;border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite;text-align: left; font-weight: bold;'>[ Note ]</td></tr>";
                                Q = Q + 1;
                            }
                            Body += "<tr><td style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top; border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite; text-align: left'>" + dtTemp.Rows[k1]["Approver Name"].ToString() + "</td><td style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top; border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite; text-align: left'> [ " + dtTemp.Rows[k1]["Previous Status"].ToString() + " ] </td><td style='border-right: tan 1px solid; border-top: tan 1px solid; vertical-align: top;border-left: tan 1px solid; border-bottom: tan 1px solid; background-color: antiquewhite;text-align: left'>[ " + dtTemp.Rows[k1]["Note"].ToString() + " ]</td></tr>";
                        }
                        Body += "</table>";
                        objConverter.SendMail(Mail_From, mail_To, Subject, Body);
                        NoofRowsAffected = oDBEngine.InsurtFieldValue("tbl_trans_email", "hem_senderemail,hem_mailmsg,hem_temid,hem_sendertype,hem_activityid,CreateDate,CreateUser", "'" + mail_To + "','Nothing','0','0','0','" + oDBEngine.GetDate().ToString() + "','" + Session["userid"].ToString() + "'");
                    }
                }
                catch
                {
                }
                data = "Record~Y";
            }
            #endregion
        }

        #endregion
        protected void grd_incomingrfa_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string param = e.Parameters.ToString();
            DataTable dt = new DataTable();
            if (param == "1")
            {
                dt = oDBEngine.GetDataTable("tbl_trans_rfa INNER JOIN tbl_master_rfa ON tbl_trans_rfa.req_rfaid = tbl_master_rfa.rfa_id INNER JOIN tbl_master_user ON tbl_trans_rfa.req_userid = tbl_master_user.user_id LEFT OUTER JOIN tbl_master_user tbl_master_user_1 ON tbl_trans_rfa.req_preapprover = tbl_master_user_1.user_id", "DISTINCT  tbl_master_rfa.rfa_shortname as shortname , tbl_master_user.user_name as createuser, tbl_trans_rfa.req_id req_id, convert(varchar(11),tbl_trans_rfa.req_startdate,113) as createdate,convert(varchar(11),tbl_trans_rfa.req_closedate,113) as closedate, CASE tbl_trans_rfa.req_prestatus WHEN 0 THEN 'Unread' WHEN 1 THEN 'Approved' WHEN 3 THEN 'Approved With MOdification' WHEN 2 THEN 'Reject' WHEN 4 THEN 'Forward to Hidher Authority' WHEN 5 THEN 'Pendding' END + ' [ ' + tbl_trans_rfa.req_prenote + '] '  as status, tbl_master_user_1.user_name as approver", " req_currapprover=" + Session["userid"].ToString() + " and req_currstatus=0");
            }
            else if (param == "2")
            {
                dt = oDBEngine.GetDataTable("tbl_trans_rfa INNER JOIN tbl_master_rfa ON tbl_trans_rfa.req_rfaid = tbl_master_rfa.rfa_id INNER JOIN tbl_master_user ON tbl_trans_rfa.req_userid = tbl_master_user.user_id Left Outer JOIN tbl_master_user tbl_master_user_1 ON tbl_trans_rfa.req_preapprover = tbl_master_user_1.user_id", "DISTINCT  tbl_master_rfa.rfa_shortname as shortname , tbl_master_user.user_name as createuser, tbl_trans_rfa.req_id req_id, convert(varchar(11),tbl_trans_rfa.req_startdate,113) as createdate,convert(varchar(11),tbl_trans_rfa.req_closedate,113) as closedate, CASE tbl_trans_rfa.req_prestatus WHEN 0 THEN 'Unread' WHEN 1 THEN 'Approved' WHEN 3 THEN 'Approved With MOdification' WHEN 2 THEN 'Reject' WHEN 4 THEN 'Forward to Hidher Authority' WHEN 5 THEN 'Pendding' END + ' [ ' + tbl_trans_rfa.req_prenote + '] ' as status, tbl_master_user_1.user_name as approver", "req_currapprover=" + Session["userid"].ToString() + " and req_currstatus=5");
            }
            else if (param == "3")
            {
                dt = oDBEngine.GetDataTable("tbl_trans_rfa INNER JOIN  tbl_trans_rfadetails ON tbl_trans_rfa.req_id = tbl_trans_rfadetails.rtd_reqnumber INNER JOIN   tbl_master_rfa ON tbl_trans_rfa.req_rfaid = tbl_master_rfa.rfa_id INNER JOIN tbl_master_user ON tbl_trans_rfa.req_preapprover = tbl_master_user.user_id INNER JOIN  tbl_master_user tbl_master_user_1 ON tbl_trans_rfa.req_userid = tbl_master_user_1.user_id WHERE  ((tbl_trans_rfa.req_currstatus = 0) AND (tbl_trans_rfadetails.rtd_appuser = " + Session["userid"].ToString() + ") OR  (tbl_trans_rfa.req_currstatus = 5)) and req_Currapprover <> " + Session["userid"].ToString() + "", "DISTINCT  tbl_trans_rfa.req_id AS req_id, tbl_master_rfa.rfa_shortname AS shortname, tbl_master_user_1.user_name AS createuser,convert(varchar(11),tbl_trans_rfa.req_startdate,113) AS createdate,convert(varchar(11),tbl_trans_rfa.req_closedate,113) as closedate,CASE tbl_trans_rfa.req_prestatus WHEN 0 THEN 'Unread' WHEN 1 THEN 'Approved' WHEN 3 THEN 'Approved With MOdification' WHEN 2 THEN 'Reject' WHEN 4 THEN 'Forward to Hidher Authority' WHEN 5 THEN 'Pendding' END + ' [ ' + tbl_trans_rfa.req_prenote + '] ' as status, tbl_master_user.user_name AS approver", null);
            }
            else if (param == "4")
            {
                dt = oDBEngine.GetDataTable("tbl_trans_rfa INNER JOIN  tbl_trans_rfadetails ON tbl_trans_rfa.req_id = tbl_trans_rfadetails.rtd_reqnumber INNER JOIN  tbl_master_rfa ON tbl_trans_rfa.req_rfaid = tbl_master_rfa.rfa_id LEFT OUTER JOIN   tbl_master_user ON tbl_trans_rfa.req_preapprover = tbl_master_user.user_id INNER JOIN  tbl_master_user tbl_master_user_1 ON tbl_trans_rfa.req_userid = tbl_master_user_1.user_id WHERE  (tbl_trans_rfa.req_aprrejno <> '') and rtd_appuser=" + Session["userid"].ToString() + " ", "DISTINCT tbl_trans_rfa.req_id AS req_id, tbl_master_rfa.rfa_shortname AS shortname, tbl_master_user_1.user_name AS createuser,convert(varchar(11),tbl_trans_rfa.req_startdate,113) AS createdate,convert(varchar(11),tbl_trans_rfa.req_closedate,113) as closedate, CASE tbl_trans_rfa.req_prestatus WHEN 0 THEN 'Unread' WHEN 1 THEN 'Approved' WHEN 3 THEN 'Approved With MOdification' WHEN 2 THEN 'Reject' WHEN 4 THEN 'Forward to Hidher Authority' WHEN 5 THEN 'Pendding' WHEN 6 THEN 'Recorded' END + ' [ ' + tbl_trans_rfa.req_prenote + '] ' as status,tbl_master_user.user_name AS approver", null);
            }
            else if (param == "Save")
            {
                dt = oDBEngine.GetDataTable("tbl_trans_rfa INNER JOIN  tbl_master_rfa ON tbl_trans_rfa.req_rfaid = tbl_master_rfa.rfa_id INNER JOIN  tbl_master_user ON tbl_trans_rfa.req_userid = tbl_master_user.user_id", "DISTINCT  tbl_master_rfa.rfa_shortname as shortname , tbl_master_user.user_name as createuser, tbl_trans_rfa.req_id req_id, convert(varchar(11),tbl_trans_rfa.req_startdate,113) as createdate, tbl_trans_rfa.req_prestatus as status", " req_currapprover=" + Session["userid"].ToString() + " and req_currstatus=0");
            }
            else if (param == "Cancel")
            {
                //dt = oDBEngine.GetDataTable("tbl_trans_rfa INNER JOIN tbl_master_rfa ON tbl_trans_rfa.req_rfaid = tbl_master_rfa.rfa_id INNER JOIN tbl_master_user ON tbl_trans_rfa.req_userid = tbl_master_user.user_id INNER JOIN tbl_master_user tbl_master_user_1 ON tbl_trans_rfa.req_currapprover = tbl_master_user_1.user_id", "DISTINCT  tbl_master_rfa.rfa_shortname as shortname , tbl_master_user.user_name as createuser, tbl_trans_rfa.req_id req_id, tbl_trans_rfa.req_startdate as createdate, CASE tbl_trans_rfa.req_currstatus WHEN 0 THEN 'Unread' WHEN 1 THEN 'Approved' WHEN 3 THEN 'Approved With MOdification' WHEN 2 THEN 'Reject' WHEN 4 THEN 'Forward to Hidher Authority' WHEN 5 THEN 'Pendding' END as status, tbl_master_user_1.user_name as approver", " req_currapprover=" + Session["userid"].ToString() + " and req_currstatus=0", "req_startdate DESC");
            }
            if (dt.Rows.Count > 0)
            {
                grd_incomingrfa.DataSource = dt;
                grd_incomingrfa.DataBind();
            }
            else
            {
                grd_incomingrfa.DataSource = dt;
                grd_incomingrfa.DataBind();
            }
            dt.Dispose();
        }
        public string next_user()
        {
            int result;
            if (HREC != "")
            {
                if (Int32.TryParse(HREC, out result))
                {
                    string[] b_id = Session["userbranchHierarchy"].ToString().Split(',');
                    for (int k = 0; k < b_id.Length; k++)
                    {
                        string[,] cApprover = oDBEngine.GetFieldValue("tbl_master_user INNER JOIN   tbl_trans_employeeCTC ON tbl_master_user.user_contactId = tbl_trans_employeeCTC.emp_cntId WHERE  (tbl_master_user.user_branchId =" + b_id[k] + ") AND (tbl_master_user.user_id NOT IN  (SELECT     tbl_master_user_2.user_id   FROM  tbl_master_user AS tbl_master_user_2 INNER JOIN   tbl_trans_employeeCTC AS tbl_trans_employeeCTC_2 ON  tbl_master_user_2.user_contactId = tbl_trans_employeeCTC_2.emp_cntId INNER JOIN  tbl_master_employee ON tbl_trans_employeeCTC_2.emp_reportTo = tbl_master_employee.emp_id INNER JOIN  tbl_master_user AS tbl_master_user_1 ON tbl_master_employee.emp_contactId = tbl_master_user_1.user_contactId INNER JOIN  tbl_trans_employeeCTC AS tbl_trans_employeeCTC_1 ON  tbl_master_user_1.user_contactId = tbl_trans_employeeCTC_1.emp_cntId   WHERE      (tbl_master_user_2.user_branchId =" + b_id[k] + ") AND (tbl_trans_employeeCTC_2.emp_Department = " + HREC + ") AND  (tbl_trans_employeeCTC_1.emp_Department =" + HREC + ") AND (tbl_master_user_1.user_branchId =" + b_id[k] + "))) AND  (tbl_trans_employeeCTC.emp_Department =" + HREC + ")", "tbl_master_user.user_id", null, 1);
                        if (cApprover[0, 0] != "n")
                        {
                            current_approver = cApprover[0, 0];
                        }
                    }
                }
                else if (HREC == "HOD")
                {
                    string[,] cApprover = oDBEngine.GetFieldValue("tbl_master_user tbl_master_user_1 INNER JOIN tbl_trans_employeeCTC tbl_trans_employeeCTC_1 ON tbl_master_user_1.user_contactId = tbl_trans_employeeCTC_1.emp_cntId INNER JOIN  tbl_master_employee ON tbl_trans_employeeCTC_1.emp_reportTo = tbl_master_employee.emp_id INNER JOIN tbl_master_user tbl_master_user_2 ON tbl_master_employee.emp_contactId = tbl_master_user_2.user_contactId", "tbl_master_user_2.user_id AS reporthead", " tbl_master_user_1.user_id = '" + Session["userid"].ToString() + "'", 1);
                    if (cApprover[0, 0] != "n")
                    {
                        current_approver = cApprover[0, 0];
                    }
                }
                else if (HREC == "OBH")
                {
                    string[,] cApprover = oDBEngine.GetFieldValue("tbl_master_user INNER JOIN tbl_master_branch ON tbl_master_user.user_branchId = tbl_master_branch.branch_id INNER JOIN  tbl_master_user tbl_master_user_1 ON tbl_master_branch.branch_head = tbl_master_user_1.user_contactId", "tbl_master_user_1.user_id AS head", " tbl_master_user.user_id = '" + Session["userid"].ToString() + "'", 1);
                    if (cApprover[0, 0] != "n")
                    {
                        current_approver = cApprover[0, 0];
                    }
                }
                else if (HREC.Substring(0, 2) == "EM")
                {
                    string[,] cApprover = oDBEngine.GetFieldValue("tbl_master_user", "user_id AS head", " user_contactid = '" + HREC + "'", 1);
                    if (cApprover[0, 0] != "n")
                    {
                        current_approver = cApprover[0, 0];
                    }
                }
                else if (HREC.Substring(0, 2) == "BR")
                {
                    string[,] cApprover = oDBEngine.GetFieldValue("tbl_master_branch", "branch_head AS head", " branch_internalid = '" + HREC + "'", 1);
                    if (cApprover[0, 0] != "n")
                    {
                        current_approver = cApprover[0, 0];
                    }
                }
                else if (HREC == "RH")
                {
                    string[,] cApprover = oDBEngine.GetFieldValue("tbl_master_user tbl_master_user_1 INNER JOIN tbl_trans_employeeCTC tbl_trans_employeeCTC_1 ON tbl_master_user_1.user_contactId = tbl_trans_employeeCTC_1.emp_cntId INNER JOIN  tbl_master_employee ON tbl_trans_employeeCTC_1.emp_reportTo = tbl_master_employee.emp_id INNER JOIN tbl_master_user tbl_master_user_2 ON tbl_master_employee.emp_contactId = tbl_master_user_2.user_contactId", "tbl_master_user_2.user_id AS reporthead", " tbl_master_user_1.user_id = '" + Session["userid"].ToString() + "'", 1);
                    if (cApprover[0, 0] != "n")
                    {
                        current_approver = cApprover[0, 0];
                    }
                }
            }
            return current_approver;
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
        protected void grd_reqhistory_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string param = e.Parameters.ToString();
            DataTable dtTemp = oDBEngine.GetDataTable("tbl_trans_rfadetails INNER JOIN tbl_master_user ON tbl_trans_rfadetails.rtd_appuser = tbl_master_user.user_id LEFT OUTER JOIN tbl_master_user tbl_master_user_1 ON tbl_trans_rfadetails.rtd_nexapprovalid = tbl_master_user_1.user_id", "tbl_trans_rfadetails.rtd_id AS ID, tbl_master_user.user_name AS [Approver Name], convert(varchar(11),tbl_trans_rfadetails.rtd_readdate,113) AS [Read Date],CASE tbl_trans_rfadetails.rtd_status WHEN 0 THEN 'Unread' WHEN 1 THEN 'Approved' WHEN 3 THEN 'Approved With MOdification' WHEN 2 THEN 'Reject' WHEN 4 THEN 'Forward to Higher Authority' WHEN 5 THEN 'Pendding'when 6 THEN 'Recorded' END AS [Previous Status], tbl_trans_rfadetails.rtd_note AS Note,tbl_master_user_1.user_name AS [Next Approver]", " tbl_trans_rfadetails.rtd_reqnumber='" + param + "'", "rtd_id");
            if (dtTemp.Rows.Count > 0)
            {
                grd_reqhistory.DataSource = dtTemp;
                grd_reqhistory.DataBind();
            }
        }
    }
}