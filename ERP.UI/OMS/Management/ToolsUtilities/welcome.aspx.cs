using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
////using DevExpress.Web;
//using DevExpress.Web;
using BusinessLogicLayer;

namespace ERP.OMS.Management.ToolsUtilities
{
    public partial class management_utilities_welcome : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        Utilities obj = new Utilities();
        DataTable DT = new DataTable();
        string data;
        //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Converter OConvert = new BusinessLogicLayer.Converter();
        DataSet dsCrystal = new DataSet();
        public string pageAccess = "";
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
        string send = "";
        string totalbody = "";
        string hrchyid = "";
        string hrchyonlyid = "";
        string _temp = "";
        string _userId1 = "";
        DataTable dtonlyid = new DataTable();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckUserSession(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            //btnshow.Visible = false;
            DataTable dtuserid = oDBEngine.GetDataTable("Select emp_cntid from TBL_TRANS_EMPLOYEEctc WHERE emp_effectiveuntil is null and emp_reportto not in (select emp_id from tbl_master_employee where emp_contactID in(select emp_cntID FROM TBL_TRANS_EMPLOYEEctc where emp_effectiveuntil is null))");
            hrchyid = dtuserid.Rows[0]["emp_cntid"].ToString();
            dtonlyid = oDBEngine.GetDataTable("select user_id from tbl_master_user where user_contactid='" + hrchyid + "'");

            if (!IsPostBack)
            {
                DateTime dttime = oDBEngine.GetDate();
                DateTime dttime1 = Convert.ToDateTime(dttime);
                DateTime dttime2 = Convert.ToDateTime(dttime).AddDays(1);
                Session["fromdate"] = dttime;
                Session["todate"] = dttime2;
                Session["mode"] = "Today";
                //divdisplay("6696");
                FillGrid("Today");


                Response.Write("<script>var tim='" + oDBEngine.GetDate().TimeOfDay.ToString() + "';</script>");
                string userid = HttpContext.Current.Session["userid"].ToString();
                //hdUserList.Value = oDBEngine.getChildUserNotColleague(userid, "");

                hdUserList.Value = oDBEngine.getChildUserNotColleague(userid, "", true);
                //string[,] data = oDBEngine.GetFieldValue(" tbl_master_user", " user_id,(user_name + '['+(select cnt_shortName from tbl_master_contact where cnt_internalId=tbl_master_user.user_contactId)+']') as user_name", " user_branchId in(" + HttpContext.Current.Session["userbranchHierarchy"] + ") and (user_leavedate='true' OR user_leavedate is null)", 2);
                string[,] data = oDBEngine.GetFieldValue(" tbl_master_user", " user_id,(user_name + '['+(select cnt_shortName from tbl_master_contact where cnt_internalId=tbl_master_user.user_contactId)+']') as user_name", " user_branchId in(" + HttpContext.Current.Session["userbranchHierarchy"] + ") and (user_leavedate='true' OR user_leavedate is null)", 2);
                // oDBEngine.AddDataToDropDownList(data, cmbCreatedFor);
                //________This script is for firing javascript when page load first___//
                if (!ClientScript.IsStartupScriptRegistered("Today"))
                    ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>btnToday_click();</script>");
                //______________________________End Script____________________________//

                DateTime dt = oDBEngine.GetDate();
                txtStart.EditFormatString = OConvert.GetDateFormat("DateTime");
                txtEnd.EditFormatString = OConvert.GetDateFormat("DateTime");
                txtStartDate.EditFormatString = OConvert.GetDateFormat("DateTime");
                txtEndDate.EditFormatString = OConvert.GetDateFormat("DateTime");
                txtStart1.EditFormatString = OConvert.GetDateFormat("DateTime");
                txtEnd1.EditFormatString = OConvert.GetDateFormat("DateTime");
                txtStart2.EditFormatString = OConvert.GetDateFormat("DateTime");
                txtEnd2.EditFormatString = OConvert.GetDateFormat("DateTime");
                cmbCreatedFor.Attributes.Add("onkeyup", "CallAjax(this,'user',event)");
                cmbinchargefor.Attributes.Add("onkeyup", "CallAjax(this,'user',event)");
                //cmbsubject.Attributes.Add("onkeyup", "CallAjax(this,'Remindercategory',event)");
                //subject.Attributes.Add("onclick", "javascript:ShowMissingData");
                string[,] list = oDBEngine.GetFieldValue(" tbl_master_template ", " tem_id,tem_shortmsg ", " (tem_type=1) and ((tem_accesslevel=1) or (createuser=" + HttpContext.Current.Session["userid"] + ")) ", 2, " tem_shortmsg DESC ");
                // oDBEngine.AddDataToDropDownList(list, cmbTemplate, true);
                //cmbTemplate.Attributes.Add("onchange", "frmOpenNewWindow1('frmshowtemplate_welcome.aspx?tem_id='+ window.document.aspnetForm.ctl00_ContentPlaceHolder3_cmbTemplate.options[window.document.aspnetForm.ctl00_ContentPlaceHolder3_cmbTemplate.selectedIndex].value,'250','1000')");
            }
            FillGrid(Session["mode"].ToString());

            //_____For performing operation without refreshing page___//
            if (chkrepeat.Checked == true)
            {
                string bbb = "111";
            }
            if (delterms.Value == "1")
            {
                Session["reapet"] = "1";
            }
            else
            {
                Session["reapet"] = "2";
            }
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            Session["reply"] = td_reply.Text.ToString().Trim();


        }
        protected void cbAll_Init(object sender, EventArgs e)
        {
            ASPxCheckBox chk = sender as ASPxCheckBox;
            ASPxGridView grid = (chk.NamingContainer as GridViewHeaderTemplateContainer).Grid;
            chk.Checked = (grid.Selection.Count == grid.VisibleRowCount);

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
            if (idparam.Contains("~"))
            {
                string[] rcv = idparam.Split('~');
                idparam = rcv[0];
                string emailsid = rcv[1];
                DataTable dtbody = oDBEngine.GetDataTable("select emails_content from trans_emails where emails_id='" + emailsid + "'");
                string bdfirst = dtbody.Rows[0]["emails_content"].ToString();

                String strHtml = String.Empty;
                int flag = 0;
                //DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else convert(varchar(19),ISNULL(rem_attendDate,''),100) end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag ", " convert(varchar(10),rem_startDate,103) = convert(varchar(10),getdate(),103) and ((rem_createUser =" + HttpContext.Current.Session["userid"] + ") OR (rem_targetUser =" + HttpContext.Current.Session["userid"] + ") or (rem_inchargetargetuser =" + HttpContext.Current.Session["userid"] + ")) order by rem_startDate,rem_priority,rem_flag desc ");
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

                convDateTime4 = DT.Rows[0]["CreateDate"].ToString();
                convDateTime4 = convDateTime4.Substring(0, 7) + convDateTime4.Substring(9);
                DT.Rows[0]["CreateDate"] = convDateTime4;

                convDateTime2 = DT.Rows[0]["EndDate"].ToString();
                convDateTime2 = convDateTime2.Substring(0, 7) + convDateTime2.Substring(9);
                substr2 = convDateTime2.Substring(4, 2);
                convDateTime2 = convDateTime.Replace(substr2, "");
                final2 = substr2 + " " + convDateTime2;
                // DT.Rows[0]["EndDate"] = final2;
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

                display.InnerHtml = strHtml;
                send = strHtml;
                totalbody = bdfirst + send;
            }
            else
            {
                String strHtml = String.Empty;
                int flag = 0;
                //DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else convert(varchar(19),ISNULL(rem_attendDate,''),100) end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag ", " convert(varchar(10),rem_startDate,103) = convert(varchar(10),getdate(),103) and ((rem_createUser =" + HttpContext.Current.Session["userid"] + ") OR (rem_targetUser =" + HttpContext.Current.Session["userid"] + ") or (rem_inchargetargetuser =" + HttpContext.Current.Session["userid"] + ")) order by rem_startDate,rem_priority,rem_flag desc ");
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
                // DT.Rows[0]["StartDate"] = final;

                convDateTime4 = DT.Rows[0]["CreateDate"].ToString();
                convDateTime4 = convDateTime4.Substring(0, 7) + convDateTime4.Substring(9);
                DT.Rows[0]["CreateDate"] = convDateTime4;

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

                display.InnerHtml = strHtml;
                send = strHtml;
            }
        }






        protected void GridReminder_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpInsertError"] = "ADD";

        }
        protected void btnhide_Click(object sender, EventArgs e)
        {

        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(cmbExport.SelectedItem.Value.ToString());
            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsToResponse();
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }
        }

        private void FillGrid(string TypeReminder)
        {

            // DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            DataTable DT = new DataTable();

            _userId1 = oDBEngine.getChildUser(HttpContext.Current.Session["userid"].ToString(), _temp);
            int len = _userId1.Length;
            if (TypeReminder == "Today")
            {

                Session["mode"] = TypeReminder;

                //DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else  Convert(Varchar,rem_attenddate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_attenddate, 100), 13, 7)  end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company ", " convert(varchar(10),rem_startDate,103) = convert(varchar(10),getdate(),103) and ((rem_createUser =" + HttpContext.Current.Session["userid"] + ") OR (rem_targetUser =" + HttpContext.Current.Session["userid"] + ") or (rem_inchargetargetuser =" + HttpContext.Current.Session["userid"] + ")) order by rem_startDate,rem_priority,rem_flag desc ");
                if (HttpContext.Current.Session["userid"].ToString() != dtonlyid.Rows[0]["user_id"].ToString())
                {
                    DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else  Convert(Varchar,rem_attenddate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_attenddate, 100), 13, 7)  end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company ", " convert(varchar(10),rem_startDate,103) = convert(varchar(10),getdate(),103) and ((rem_createUser =" + HttpContext.Current.Session["userid"] + ") OR (rem_targetUser =" + HttpContext.Current.Session["userid"] + ") or (rem_inchargetargetuser =" + HttpContext.Current.Session["userid"] + "))  order by rem_startDate,rem_priority,rem_flag desc ");
                }
                else
                {
                    //DT = oDBEngine.GetDataTable("Select case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else  Convert(Varchar,rem_attenddate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_attenddate, 100), 13, 7)  end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company ,* from (Select * from tbl_trans_reminder) T1 inner Join (Select User_ID from tbl_master_user Where user_contactId in (Select emp_cntID from tbl_trans_employeeCTC Where emp_reportTo= (Select emp_id from tbl_master_employee Where emp_contactId=(Select emp_cntId from tbl_trans_employeeCTC Where emp_reportTo=0 and emp_effectiveuntil is null)))) T2 on rem_createUser=User_ID or rem_targetUser=User_ID or rem_inchargetargetuser=User_ID WHERE  convert(varchar(10),rem_startDate,103) = convert(varchar(10),getdate(),103) order by rem_startDate,rem_priority,rem_flag desc");
                    DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else  Convert(Varchar,rem_attenddate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_attenddate, 100), 13, 7)  end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company ", " convert(varchar(10),rem_startDate,103) = convert(varchar(10),getdate(),103) and ((rem_createUser in (" + _userId1 + ")) OR (rem_targetUser in (" + _userId1 + ")) or (rem_inchargetargetuser in (" + _userId1 + ")))  order by rem_startDate,rem_priority,rem_flag desc ");
                }
                for (int m = 0; m < DT.Rows.Count; m++)
                {
                    final = "";
                    substr = "";
                    final2 = "";
                    substr2 = "";
                    final3 = "";
                    substr3 = "";
                    string attnddt = "";
                    convDateTime = DT.Rows[m]["StartDate"].ToString();
                    convDateTime = convDateTime.Substring(0, 7) + convDateTime.Substring(9);
                    substr = convDateTime.Substring(4, 2);
                    convDateTime = convDateTime.Replace(substr, "");
                    final = substr + " " + convDateTime;
                    //DT.Rows[m]["StartDate"] = final;

                    convDateTime4 = DT.Rows[m]["CreateDate"].ToString();
                    convDateTime4 = convDateTime4.Substring(0, 7) + convDateTime4.Substring(9);
                    DT.Rows[m]["CreateDate"] = convDateTime4;

                    convDateTime2 = DT.Rows[m]["EndDate"].ToString();
                    convDateTime2 = convDateTime2.Substring(0, 7) + convDateTime2.Substring(9);
                    substr2 = convDateTime2.Substring(4, 2);
                    convDateTime2 = convDateTime.Replace(substr2, "");
                    final2 = substr2 + " " + convDateTime2;
                    //DT.Rows[m]["EndDate"] = final2;
                    attnddt = DT.Rows[m]["attenddate"].ToString().Trim();
                    if (attnddt.Length > 0)
                    //if ((DT.Rows[m]["attenddate"].ToString().Trim() != " ") || (DT.Rows[m]["attenddate"].ToString().Trim() != ""))
                    {
                        convDateTime3 = DT.Rows[m]["attenddate"].ToString();
                        convDateTime3 = convDateTime3.Substring(0, 7) + convDateTime3.Substring(9);
                        substr3 = convDateTime3.Substring(4, 2);
                        convDateTime3 = convDateTime3.Replace(substr3, "");
                        final3 = substr3 + " " + convDateTime3;
                        //DT.Rows[m]["attenddate"] = final3;
                    }

                }
                GridReminder.DataSource = DT.DefaultView;
                GridReminder.DataBind();

            }
            else if (TypeReminder == "Pending")
            {
                Session["mode"] = TypeReminder;
                if (HttpContext.Current.Session["userid"].ToString() != dtonlyid.Rows[0]["user_id"].ToString())
                {
                    //DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_sourceid WHEN 0 THEN (SELECT     tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser) ELSE 'System' END AS CreateBy, rem_createDate AS CreateDate, rem_targetUser AS TargetId,(SELECT tbl_master_user.user_name FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, rem_startDate AS StartDate, rem_endDate AS EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'NotAttended' ELSE 'Attended' END AS Status ", " rem_actionTaken=0 order by rem_startDate desc ");
                    DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else convert(varchar(19),ISNULL(rem_attendDate,''),100) end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company ", " rem_actionTaken<>1 and ((rem_createUser =" + HttpContext.Current.Session["userid"] + ") OR (rem_targetUser =" + HttpContext.Current.Session["userid"] + ") or (rem_inchargetargetuser =" + HttpContext.Current.Session["userid"] + ")) order by rem_startDate,rem_priority,rem_flag desc ");
                }
                else
                {
                    DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else convert(varchar(19),ISNULL(rem_attendDate,''),100) end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company ", " rem_actionTaken<>1 and ((rem_createUser in (" + _userId1 + ")) OR (rem_targetUser in (" + _userId1 + ")) or (rem_inchargetargetuser in (" + _userId1 + "))) order by rem_startDate,rem_priority,rem_flag desc ");
                }
                for (int m = 0; m < DT.Rows.Count; m++)
                {
                    final = "";
                    substr = "";
                    final2 = "";
                    substr2 = "";
                    final3 = "";
                    substr3 = "";
                    convDateTime = DT.Rows[m]["StartDate"].ToString();
                    convDateTime = convDateTime.Substring(0, 7) + convDateTime.Substring(9);
                    substr = convDateTime.Substring(4, 2);
                    convDateTime = convDateTime.Replace(substr, "");
                    final = substr + " " + convDateTime;
                    //DT.Rows[m]["StartDate"] = final;
                    convDateTime4 = DT.Rows[m]["CreateDate"].ToString();
                    convDateTime4 = convDateTime4.Substring(0, 7) + convDateTime4.Substring(9);
                    DT.Rows[m]["CreateDate"] = convDateTime4;

                    convDateTime2 = DT.Rows[m]["EndDate"].ToString();
                    convDateTime2 = convDateTime2.Substring(0, 7) + convDateTime2.Substring(9);
                    substr2 = convDateTime2.Substring(4, 2);
                    convDateTime2 = convDateTime.Replace(substr2, "");
                    final2 = substr2 + " " + convDateTime2;
                    //DT.Rows[m]["EndDate"] = final2;

                    //convDateTime3 = DT.Rows[m]["attenddate"].ToString();
                    //convDateTime3 = convDateTime3.Substring(0, 7) + convDateTime3.Substring(9);
                    //substr3 = convDateTime3.Substring(4, 2);
                    //convDateTime3 = convDateTime3.Replace(substr3, "");
                    //final3 = substr3 + " " + convDateTime3;
                    //DT.Rows[m]["attenddate"] = final3;
                }
                GridReminder.DataSource = DT.DefaultView;
                GridReminder.DataBind();
            }
            else if (TypeReminder == "Attended")
            {
                Session["mode"] = TypeReminder;
                if (HttpContext.Current.Session["userid"].ToString() != dtonlyid.Rows[0]["user_id"].ToString())
                {
                    DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else  Convert(Varchar,rem_attenddate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_attenddate, 100), 13, 7)  end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company", " rem_actionTaken=1 and ((rem_createUser =" + HttpContext.Current.Session["userid"] + ") OR (rem_targetUser =" + HttpContext.Current.Session["userid"] + ") or (rem_inchargetargetuser =" + HttpContext.Current.Session["userid"] + ")) order by rem_startDate,rem_priority desc ");
                }
                else
                {
                    DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else  Convert(Varchar,rem_attenddate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_attenddate, 100), 13, 7)  end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company", " rem_actionTaken=1 and ((rem_createUser in (" + _userId1 + ")) OR (rem_targetUser in (" + _userId1 + ")) or (rem_inchargetargetuser in (" + _userId1 + "))) order by rem_startDate,rem_priority desc ");
                }
                //DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " top 10 rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_sourceid WHEN 0 THEN (SELECT     tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser) ELSE 'System' END AS CreateBy, rem_createDate AS CreateDate, rem_targetUser AS TargetId,(SELECT tbl_master_user.user_name FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, rem_startDate AS StartDate, rem_endDate AS EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'NotAttended' ELSE 'Attended' END AS Status ", " rem_actionTaken>0 order by rem_startDate desc ");
                for (int m = 0; m < DT.Rows.Count; m++)
                {
                    final = "";
                    substr = "";
                    final2 = "";
                    substr2 = "";
                    final3 = "";
                    substr3 = "";
                    convDateTime = DT.Rows[m]["StartDate"].ToString();
                    convDateTime = convDateTime.Substring(0, 7) + convDateTime.Substring(9);
                    substr = convDateTime.Substring(4, 2);
                    convDateTime = convDateTime.Replace(substr, "");
                    final = substr + " " + convDateTime;
                    //DT.Rows[m]["StartDate"] = final;

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
            }
            else if (TypeReminder == "Filter")
            {
                Session["mode"] = TypeReminder;
                if (HttpContext.Current.Session["userid"].ToString() != dtonlyid.Rows[0]["user_id"].ToString())
                {
                    DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else  Convert(Varchar,rem_attenddate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_attenddate, 100), 13, 7)  end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company ", " rem_actionTaken>0 and ((rem_createUser =" + HttpContext.Current.Session["userid"] + ") OR (rem_targetUser =" + HttpContext.Current.Session["userid"] + ") or (rem_inchargetargetuser =" + HttpContext.Current.Session["userid"] + ")) and rem_startDate between '" + txtStart.Value.ToString() + "' and '" + txtEnd.Value.ToString() + "' order by rem_startDate,rem_priority desc ");
                }
                else
                {
                    DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else  Convert(Varchar,rem_attenddate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_attenddate, 100), 13, 7)  end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company ", " rem_actionTaken>0 and ((rem_createUser in (" + _userId1 + ")) OR (rem_targetUser in (" + _userId1 + ")) or (rem_inchargetargetuser in (" + _userId1 + "))) and rem_startDate between '" + txtStart.Value.ToString() + "' and '" + txtEnd.Value.ToString() + "' order by rem_startDate,rem_priority desc ");
                }
                for (int m = 0; m < DT.Rows.Count; m++)
                {
                    final = "";
                    substr = "";
                    final2 = "";
                    substr2 = "";
                    final3 = "";
                    substr3 = "";
                    convDateTime = DT.Rows[m]["StartDate"].ToString();
                    convDateTime = convDateTime.Substring(0, 7) + convDateTime.Substring(9);
                    substr = convDateTime.Substring(4, 2);
                    convDateTime = convDateTime.Replace(substr, "");
                    final = substr + " " + convDateTime;
                    //DT.Rows[m]["StartDate"] = final;

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
            }
            else if (TypeReminder == "Filter1")
            {
                Session["mode"] = TypeReminder;
                if (HttpContext.Current.Session["userid"].ToString() != dtonlyid.Rows[0]["user_id"].ToString())
                {
                    DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else convert(varchar(19),ISNULL(rem_attendDate,''),100) end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company   ", " rem_actionTaken=0 and ((rem_createUser =" + HttpContext.Current.Session["userid"] + ") OR (rem_targetUser =" + HttpContext.Current.Session["userid"] + ") or (rem_inchargetargetuser =" + HttpContext.Current.Session["userid"] + ")) and rem_createDate between '" + txtStart1.Value.ToString() + "' and '" + txtEnd1.Value.ToString() + "' order by rem_createDate,rem_flag desc ");
                }
                else
                {
                    DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else convert(varchar(19),ISNULL(rem_attendDate,''),100) end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company   ", " rem_actionTaken=0 and ((rem_createUser in (" + _userId1 + ")) OR (rem_targetUser in (" + _userId1 + ")) or (rem_inchargetargetuser in (" + _userId1 + "))) and rem_createDate between '" + txtStart1.Value.ToString() + "' and '" + txtEnd1.Value.ToString() + "' order by rem_createDate,rem_flag desc ");
                }
                for (int m = 0; m < DT.Rows.Count; m++)
                {
                    final = "";
                    substr = "";
                    final2 = "";
                    substr2 = "";
                    final3 = "";
                    substr3 = "";
                    convDateTime = DT.Rows[m]["StartDate"].ToString();
                    convDateTime = convDateTime.Substring(0, 7) + convDateTime.Substring(9);
                    substr = convDateTime.Substring(4, 2);
                    convDateTime = convDateTime.Replace(substr, "");
                    final = substr + " " + convDateTime;
                    //DT.Rows[m]["StartDate"] = final;

                    convDateTime4 = DT.Rows[m]["CreateDate"].ToString();
                    convDateTime4 = convDateTime4.Substring(0, 7) + convDateTime4.Substring(9);
                    DT.Rows[m]["CreateDate"] = convDateTime4;

                    convDateTime2 = DT.Rows[m]["EndDate"].ToString();
                    convDateTime2 = convDateTime2.Substring(0, 7) + convDateTime2.Substring(9);
                    substr2 = convDateTime2.Substring(4, 2);
                    convDateTime2 = convDateTime.Replace(substr2, "");
                    final2 = substr2 + " " + convDateTime2;
                    //DT.Rows[m]["EndDate"] = final2;

                    //convDateTime3 = DT.Rows[m]["attenddate"].ToString();
                    //convDateTime3 = convDateTime3.Substring(0, 7) + convDateTime3.Substring(9);
                    //substr3 = convDateTime3.Substring(4, 2);
                    //convDateTime3 = convDateTime3.Replace(substr3, "");
                    //final3 = substr3 + " " + convDateTime3;
                    //DT.Rows[m]["attenddate"] = final3;
                }
                GridReminder.DataSource = DT.DefaultView;
                GridReminder.DataBind();

            }
            else if (TypeReminder == "Filter2")
            {
                Session["mode"] = TypeReminder;
                if (HttpContext.Current.Session["userid"].ToString() != dtonlyid.Rows[0]["user_id"].ToString())
                {
                    DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else  Convert(Varchar,rem_attenddate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_attenddate, 100), 13, 7)  end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company   ", " ((rem_createUser =" + HttpContext.Current.Session["userid"] + ") OR (rem_targetUser =" + HttpContext.Current.Session["userid"] + ") or (rem_inchargetargetuser =" + HttpContext.Current.Session["userid"] + ")) and rem_createDate between '" + txtStart2.Value.ToString() + "' and '" + txtEnd2.Value.ToString() + "' order by rem_createDate,rem_flag desc ");
                }
                else
                {
                    DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else  Convert(Varchar,rem_attenddate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_attenddate, 100), 13, 7)  end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company   ", " ((rem_createUser in (" + _userId1 + ")) OR (rem_targetUser in (" + _userId1 + ")) or (rem_inchargetargetuser in (" + _userId1 + "))) and rem_createDate between '" + txtStart2.Value.ToString() + "' and '" + txtEnd2.Value.ToString() + "' order by rem_createDate,rem_flag desc ");
                }
                for (int m = 0; m < DT.Rows.Count; m++)
                {
                    final = "";
                    substr = "";
                    final2 = "";
                    substr2 = "";
                    final3 = "";
                    substr3 = "";
                    convDateTime = DT.Rows[m]["StartDate"].ToString();
                    convDateTime = convDateTime.Substring(0, 7) + convDateTime.Substring(9);
                    substr = convDateTime.Substring(4, 2);
                    convDateTime = convDateTime.Replace(substr, "");
                    final = substr + " " + convDateTime;
                    //DT.Rows[m]["StartDate"] = final;

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

            }
            else if (TypeReminder == "s")
            {
                Session["mode"] = TypeReminder;
                GridReminder.Settings.ShowFilterRow = true;
                if (HttpContext.Current.Session["userid"].ToString() != dtonlyid.Rows[0]["user_id"].ToString())
                {
                    DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else  Convert(Varchar,rem_attenddate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_attenddate, 100), 13, 7)  end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company  ", "((rem_createUser =" + HttpContext.Current.Session["userid"] + ") OR (rem_targetUser =" + HttpContext.Current.Session["userid"] + ") or (rem_inchargetargetuser =" + HttpContext.Current.Session["userid"] + ")) ");
                }
                else
                {
                    DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else  Convert(Varchar,rem_attenddate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_attenddate, 100), 13, 7)  end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company  ", "((rem_createUser in (" + _userId1 + ")) OR (rem_targetUser in (" + _userId1 + ")) or (rem_inchargetargetuser in (" + _userId1 + "))) ");
                }
                for (int m = 0; m < DT.Rows.Count; m++)
                {
                    final = "";
                    substr = "";
                    final2 = "";
                    substr2 = "";
                    final3 = "";
                    substr3 = "";
                    string attnddt = "";
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
                    // DT.Rows[m]["EndDate"] = final2;

                    attnddt = DT.Rows[m]["attenddate"].ToString().Trim();
                    if (attnddt.Length > 0)
                    //if ((DT.Rows[m]["attenddate"].ToString().Trim() != " ") || (DT.Rows[m]["attenddate"].ToString().Trim() != ""))
                    {
                        convDateTime3 = DT.Rows[m]["attenddate"].ToString();
                        convDateTime3 = convDateTime3.Substring(0, 7) + convDateTime3.Substring(9);
                        substr3 = convDateTime3.Substring(4, 2);
                        convDateTime3 = convDateTime3.Replace(substr3, "");
                        final3 = substr3 + " " + convDateTime3;
                        //DT.Rows[m]["attenddate"] = final3;
                    }
                }
                GridReminder.DataSource = DT.DefaultView;
                GridReminder.DataBind();
                //FillGrid(Session["mode"].ToString());
            }
            else if (TypeReminder == "All")
            {
                Session["mode"] = TypeReminder;
                if (HttpContext.Current.Session["userid"].ToString() != dtonlyid.Rows[0]["user_id"].ToString())
                {
                    DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN '' THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else  Convert(Varchar,rem_attenddate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_attenddate, 100), 13, 7)  end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company  ", " ((rem_createUser =" + HttpContext.Current.Session["userid"] + ") OR (rem_targetUser =" + HttpContext.Current.Session["userid"] + ") or (rem_inchargetargetuser =" + HttpContext.Current.Session["userid"] + ")) order by rem_startDate,rem_priority desc ");
                }
                else
                {
                    DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else  Convert(Varchar,rem_attenddate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_attenddate, 100), 13, 7)  end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company  ", " ((rem_createUser in (" + _userId1 + ")) OR (rem_targetUser in (" + _userId1 + ")) or (rem_inchargetargetuser in (" + _userId1 + "))) order by rem_startDate,rem_priority desc ");
                }
                for (int m = 0; m < DT.Rows.Count; m++)
                {
                    final = "";
                    substr = "";
                    final2 = "";
                    substr2 = "";
                    final3 = "";
                    substr3 = "";
                    convDateTime = DT.Rows[m]["StartDate"].ToString();
                    convDateTime = convDateTime.Substring(0, 7) + convDateTime.Substring(9);
                    substr = convDateTime.Substring(4, 2);
                    convDateTime = convDateTime.Replace(substr, "");
                    final = substr + " " + convDateTime;
                    //DT.Rows[m]["StartDate"] = final;

                    convDateTime4 = DT.Rows[m]["CreateDate"].ToString();
                    convDateTime4 = convDateTime4.Substring(0, 7) + convDateTime4.Substring(9);
                    DT.Rows[m]["CreateDate"] = convDateTime4;

                    convDateTime2 = DT.Rows[m]["EndDate"].ToString();
                    convDateTime2 = convDateTime2.Substring(0, 7) + convDateTime2.Substring(9);
                    substr2 = convDateTime2.Substring(4, 2);
                    convDateTime2 = convDateTime.Replace(substr2, "");
                    final2 = substr2 + " " + convDateTime2;
                    //DT.Rows[m]["EndDate"] = final2;
                    //if (DT.Rows[m]["attenddate"] != " ")
                    //{
                    //    convDateTime3 = DT.Rows[m]["attenddate"].ToString();
                    //    convDateTime3 = convDateTime3.Substring(0, 7) + convDateTime3.Substring(9);
                    //    substr3 = convDateTime3.Substring(4, 2);
                    //    convDateTime3 = convDateTime3.Replace(substr3, "");
                    //    final3 = substr3 + " " + convDateTime3;
                    //    DT.Rows[m]["attenddate"] = final3;
                    //}
                }
                GridReminder.DataSource = DT.DefaultView;
                GridReminder.DataBind();
            }
        }

        protected void GridReminder_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            // GridReminder.VisibleColumns[16].Visible = false;
            if (e.RowType != DevExpress.Web.GridViewRowType.Data) return;
            int rowindex = e.VisibleIndex;
            string priority = GridReminder.GetRowValues(rowindex, "priority").ToString();
            string flag = GridReminder.GetRowValues(rowindex, "flag").ToString();
            string status = GridReminder.GetRowValues(rowindex, "Status").ToString();
            string replycontent = GridReminder.GetRowValues(rowindex, "replycontent").ToString();
            string ContactID = e.GetValue("Rid").ToString();
            //string CntName = e.GetValue("Name").ToString();

            //if (replycontent == "Attend From Header Please leave Your suggestion")
            //{
            //    //e.Row.Cells[9].Style.Add("color", "#FDCB0A");

            //    //e.Row.Cells[9].Style.Add("text-decoration", "blink");
            //    e.Row.Cells[9].Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + " this.style.backgroundColor='#FDCB0A';");
            //    e.Row.Cells[9].Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");


            //}
            if (flag == "F")
            {
                e.Row.Cells[15].Style.Add("color", "#008080");
                e.Row.Cells[15].ToolTip = "Fortnightly";
            }
            if (flag == "Q")
            {
                e.Row.Cells[15].Style.Add("color", "#9ACD32");
                e.Row.Cells[15].ToolTip = "Quarterly";
            }
            if (flag == "M")
            {
                e.Row.Cells[15].Style.Add("color", "#800080");
                e.Row.Cells[15].ToolTip = "Monthly";
            }
            if (flag == "W")
            {
                e.Row.Cells[15].Style.Add("color", "Blue");
                e.Row.Cells[15].ToolTip = "Weekly";
            }
            if (flag == "D")
            {
                e.Row.Cells[15].Style.Add("color", " #4B0082");
                e.Row.Cells[15].ToolTip = "Daily";
            }
            if (flag == "R")
            {
                e.Row.Cells[15].Style.Add("color", "BLACK");
                e.Row.Cells[15].ToolTip = "Random";
            }
            if (flag == "S")
            {
                e.Row.Cells[15].Style.Add("color", "Navy");
                e.Row.Cells[15].ToolTip = "Semi Annually";
            }
            if (flag == "A")
            {
                e.Row.Cells[15].Style.Add("color", "Lime");
                e.Row.Cells[15].ToolTip = "Annually";
            }

            if (priority == "U")
            {

                e.Row.Cells[10].Style.Add("color", "Red");
                e.Row.Cells[10].ToolTip = "Urgent";



            }
            if (priority == "H")
            {

                e.Row.Cells[10].Style.Add("color", "Brown");
                e.Row.Cells[10].ToolTip = "High";


            }
            if (priority == "N")
            {
                e.Row.Cells[10].Style.Add("color", "Blue");
                e.Row.Cells[10].ToolTip = "Normal";

            }
            if (priority == "L")
            {
                e.Row.Cells[10].Style.Add("color", "Black");
                e.Row.Cells[10].ToolTip = "Low";

            }
            if (status == "Open")
            {

                e.Row.Cells[9].Style.Add("cursor", "hand");
                // e.Row.Cells[12].Attributes.Add("onclick", "javascript:ShowMissingData('" + ContactID + "');");
                e.Row.Cells[9].Attributes.Add("onclick", "javascript:showhistory('" + ContactID + '~' + status + "');");
                e.Row.Cells[9].ToolTip = "ADD/VIEW";
                e.Row.Cells[13].Style.Add("color", "Blue");


                //Tr1.Visible = true;
            }
            if (status != "Closed")
            {
                e.Row.Cells[13].Style.Add("cursor", "hand");
                e.Row.Cells[13].ToolTip = "Click To Change Status !";
                e.Row.Cells[13].Attributes.Add("onclick", "javascript:Changestatus('" + ContactID + "');");



            }
            if (status == "Closed")
            {
                e.Row.Cells[9].Style.Add("cursor", "hand");
                //e.Row.Cells[12].Attributes.Add("onclick", "javascript:showhistory('" + ContactID + "');");
                e.Row.Cells[9].Attributes.Add("onclick", "javascript:showhistory('" + ContactID + '~' + status + "');");
                e.Row.Cells[9].ToolTip = "Only View !";
                e.Row.Cells[13].Style.Add("color", "Green");
            }
            if (status == "Pending")
            {
                e.Row.Cells[13].Style.Add("color", "Red");
            }


        }
        protected void test_Click(object sender, ImageClickEventArgs e)
        {
            FillGrid("Today");
            ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>btnToday_click();</script>");
        }
        void SendReport(string emailbdy, string contactid, string contactidtarget, string contactidincharge, DateTime billdate, string Subject, DateTime createdate)
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
                //    SqlCommand lcmdEmplInsert = new SqlCommand("InsertTransEmailforreminder", lcon);
                //lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                //lcmdEmplInsert.Parameters.AddWithValue("@Emails_SenderEmailID", senderemail);
                //lcmdEmplInsert.Parameters.AddWithValue("@Emails_Subject", Subject);
                //lcmdEmplInsert.Parameters.AddWithValue("@Emails_Content", "<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Name:" + ClientName + "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">" + Subject + "</td></tr> <tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Segment: " + segmentname + "</td></tr><tr><td>" + emailbdy + "</td></tr></table>");
                //lcmdEmplInsert.Parameters.AddWithValue("@Emails_HasAttachement", atchflile);
                //lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateApplication", menuId);
                //lcmdEmplInsert.Parameters.AddWithValue("@Emails_CreateUser", HttpContext.Current.Session["userid"]);
                //lcmdEmplInsert.Parameters.AddWithValue("@Emails_Type", "N");
                //if (HttpContext.Current.Session["LastCompany"].ToString() != "") 
                //{
                //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
                //}
                //else
                //{
                //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_CompanyID", cmpintid.ToString().Trim());
                //}
                //lcmdEmplInsert.Parameters.AddWithValue("@Emails_Segment", segmentname);
                //lcmdEmplInsert.Parameters.AddWithValue("@Emails_createdate", createdate);
                //SqlParameter parameter = new SqlParameter("@result", SqlDbType.BigInt);
                //parameter.Direction = ParameterDirection.Output;
                //lcmdEmplInsert.Parameters.Add(parameter);
                //lcmdEmplInsert.ExecuteNonQuery();
                //    string InternalID = parameter.Value.ToString();
                //    //  ###########---recipients-----------------                   

                //    //DataTable dt1 = oDBEngine.GetDataTable(" tbl_master_email ", "top 1 *", "eml_cntId='" + contactid + "' and eml_type='Official'");
                //    //string mailid = dt1.Rows[0]["eml_email"].ToString();

                //    string fValues3 = "'" + InternalID + "','" + contactlead + "','" + mailid + "','TO','" + billdate + "','" + "P" + "','" + menuId + "'";
                //    oDBEngine.InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status,EmailRecipients_ActivityID", fValues3);
                //}
                string EmailContent = "<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Name:" + ClientName + "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">" + Subject + "</td></tr> <tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Segment: " + segmentname + "</td></tr><tr><td>" + emailbdy + "</td></tr></table>";
                string InternalID = obj.InsertTransEmailforreminder(senderemail, Subject, EmailContent, atchflile, menuId, HttpContext.Current.Session["userid"].ToString(),
                    "N", HttpContext.Current.Session["LastCompany"].ToString() != "" ? HttpContext.Current.Session["LastCompany"].ToString() : cmpintid.ToString().Trim(),
                    segmentname, createdate);

                string fValues3 = "'" + InternalID + "','" + contactlead + "','" + mailid + "','TO','" + billdate + "','" + "P" + "','" + menuId + "'";
                oDBEngine.InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status,EmailRecipients_ActivityID", fValues3);

            }
            if (target != "")
            {

                string contactlead2 = "";

                DataTable dtlead1 = oDBEngine.GetDataTable("select user_contactid from tbl_master_user where user_id = '" + contactidtarget + "'");


                if (dtlead1 != null && dtlead1.Rows.Count > 0)
                {
                    contactlead2 = Convert.ToString(dtlead1.Rows[0]["user_contactid"]);
                }

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
                //    SqlCommand lcmdEmplInsert = new SqlCommand("InsertTransEmailforreminder", lcon);
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
                //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_createdate", createdate);
                //    SqlParameter parameter = new SqlParameter("@result", SqlDbType.BigInt);
                //    parameter.Direction = ParameterDirection.Output;
                //    lcmdEmplInsert.Parameters.Add(parameter);
                //    lcmdEmplInsert.ExecuteNonQuery();
                //    string InternalID = parameter.Value.ToString();
                //    //  ###########---recipients-----------------                   

                //    //DataTable dt1 = oDBEngine.GetDataTable(" tbl_master_email ", "top 1 *", "eml_cntId='" + contactid + "' and eml_type='Official'");
                //    //string mailid = dt1.Rows[0]["eml_email"].ToString();

                //    string fValues3 = "'" + InternalID + "','" + contactlead2 + "','" + target + "','TO','" + billdate + "','" + "P" + "','"+ menuId +"'";
                //    oDBEngine.InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status,EmailRecipients_ActivityID", fValues3);
                //}

                string EmailContent = "<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Name:" + ClientName + "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">" + Subject + "</td></tr> <tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Segment: " + segmentname + "</td></tr><tr><td>" + emailbdy + "</td></tr></table>";
                string InternalID = obj.InsertTransEmailforreminder(senderemail, Subject, EmailContent, atchflile, menuId, HttpContext.Current.Session["userid"].ToString(),
                    "N", HttpContext.Current.Session["LastCompany"].ToString() != "" ? HttpContext.Current.Session["LastCompany"].ToString() : cmpintid.ToString().Trim(),
                    segmentname, createdate);

                string fValues3 = "'" + InternalID + "','" + contactlead2 + "','" + target + "','TO','" + billdate + "','" + "P" + "','" + menuId + "'";
                oDBEngine.InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status,EmailRecipients_ActivityID", fValues3);

            }
            if (incharge != "")
            {
                string contactlead1 = "";

                DataTable dtlead12 = oDBEngine.GetDataTable("select user_contactid from tbl_master_user where user_id = '" + contactidincharge + "'");


                if (dtlead12 != null && dtlead12.Rows.Count > 0)
                {
                    contactlead1 = Convert.ToString(dtlead12.Rows[0]["user_contactid"]);
                }


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
                //    SqlCommand lcmdEmplInsert = new SqlCommand("InsertTransEmailforreminder", lcon);
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
                //    lcmdEmplInsert.Parameters.AddWithValue("@Emails_createdate", createdate);
                //    SqlParameter parameter = new SqlParameter("@result", SqlDbType.BigInt);
                //    parameter.Direction = ParameterDirection.Output;
                //    lcmdEmplInsert.Parameters.Add(parameter);
                //    lcmdEmplInsert.ExecuteNonQuery();
                //    string InternalID = parameter.Value.ToString();
                //    //  ###########---recipients-----------------                   

                //    //DataTable dt1 = oDBEngine.GetDataTable(" tbl_master_email ", "top 1 *", "eml_cntId='" + contactid + "' and eml_type='Official'");
                //    //string mailid = dt1.Rows[0]["eml_email"].ToString();

                //    string fValues3 = "'" + InternalID + "','" + contactlead1 + "','" + incharge + "','TO','" + billdate + "','" + "P" + "','"+ menuId +"'";
                //    oDBEngine.InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status,EmailRecipients_ActivityID", fValues3);
                //}
                string EmailContent = "<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Name:" + ClientName + "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">" + Subject + "</td></tr> <tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Segment: " + segmentname + "</td></tr><tr><td>" + emailbdy + "</td></tr></table>";
                string InternalID = obj.InsertTransEmailforreminder(senderemail, Subject, EmailContent, atchflile, menuId, HttpContext.Current.Session["userid"].ToString(),
                    "N", HttpContext.Current.Session["LastCompany"].ToString() != "" ? HttpContext.Current.Session["LastCompany"].ToString() : cmpintid.ToString().Trim(),
                    segmentname, createdate);

                string fValues3 = "'" + InternalID + "','" + contactlead1 + "','" + incharge + "','TO','" + billdate + "','" + "P" + "','" + menuId + "'";
                oDBEngine.InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status,EmailRecipients_ActivityID", fValues3);

            }
            //}



            //catch (Exception)
            //{
            //    return false;
            //}
            //return true;
        }

        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string mode = Session["mode"].ToString();
            string id = eventArgument.ToString();
            string[] idlist = id.Split('~');
            #region Edit
            if (idlist[0] == "Edit")
            {
                //cmbinchargefor.Visible = true;
                Session["KeyVal"] = idlist[1].ToString();
                DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " rem_id,rem_createUser,rem_targetUser,rem_displayTricker,rem_startDate,rem_endDate,rem_reminderContent,rem_actionTaken,case when rem_priority=1 then 'n'  when rem_priority=2 then 'H' when rem_priority=3 then 'L' when rem_priority=4 then 'U' end as priority,isnull(rem_priority,'') as rem_priority,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_cmpid as cmpid,(select ltrim(rtrim( cmp_Name ))+ '  '+' [ ' + cmp_onroleshortname + ' ] ' from tbl_master_company where cmp_id=rem_cmpid) as company ", " rem_id = " + idlist[1]);
                if (DT.Rows.Count > 0)
                {
                    string useid = HttpContext.Current.Session["userid"].ToString();
                    if (DT.Rows[0][1].ToString() == HttpContext.Current.Session["userid"].ToString())
                    {
                        if (DT.Rows[0]["rem_actionTaken"].ToString() != "1")
                        {
                            string[,] new2 = oDBEngine.GetFieldValue("tbl_master_user", "isnull(rtrim(user_name),'')  +' ['+isnull(rtrim(user_loginid),'')+']' as user_loginid", "user_id= '" + DT.Rows[0]["inchargeid"].ToString() + "'", 1);
                            string[,] new1 = oDBEngine.GetFieldValue("tbl_master_user", "isnull(rtrim(user_name),'')  +' ['+isnull(rtrim(user_loginid),'')+']' as user_loginid", "user_id= '" + DT.Rows[0]["rem_targetUser"].ToString() + "'", 1);
                            string[,] new3 = oDBEngine.GetFieldValue("tbl_master_company", "isnull(rtrim(cmp_onroleshortname),'')  as cmpname", "cmp_id= '" + DT.Rows[0]["cmpid"].ToString() + "'", 1);
                            //cmbinchargefor.Text = DT.Rows[0]["incharge"].ToString();
                            if (new2[0, 0] != "n")
                            {
                                cmbinchargefor.Text = new2[0, 0].ToString().Trim();
                                cmbinchargefor.Enabled = false;
                            }
                            else
                            {
                                cmbinchargefor.Text = "";
                            }
                            if (new3[0, 0] != "n")
                            {
                                cmbcompany.Text = new3[0, 0].ToString().Trim();
                                //cmbinchargefor.Enabled = false;
                            }
                            else
                            {
                                cmbcompany.Text = "";
                            }

                            cmbCreatedFor.Text = new1[0, 0].ToString().Trim();
                            cmbTicker.SelectedValue = DT.Rows[0]["rem_displayTricker"].ToString();
                            cmbpriority.SelectedValue = DT.Rows[0]["rem_priority"].ToString();
                            Converter oConverter = new Converter();
                            cmbsubject.Text = DT.Rows[0]["shortname"].ToString();
                            //if (DT.Rows[0]["inchargeid"] != null)
                            //{
                            //    chkincharge.Checked = true;
                            //    cmbinchargefor.Text = DT.Rows[0]["incharge"].ToString().Trim();
                            //}
                            //txtStartDate.Text = oConverter.DateConverter_d_m_y(DT.Rows[0]["rem_startDate"].ToString(), "dd/mm/yyyy hh:mm");
                            //txtEndDate.Text = oConverter.DateConverter_d_m_y(DT.Rows[0]["rem_endDate"].ToString(), "dd/mm/yyyy hh:mm");
                            txtcontent.Text = DT.Rows[0]["rem_reminderContent"].ToString();
                            data = "Edit" + '~' + cmbCreatedFor.Text.ToString() + "~" + DT.Rows[0]["rem_displayTricker"].ToString() + "~" + DT.Rows[0]["rem_startDate"].ToString() + "~" + DT.Rows[0]["rem_endDate"].ToString() + "~" + DT.Rows[0]["rem_reminderContent"].ToString() + "~" + DT.Rows[0]["rem_priority"].ToString() + "~" + DT.Rows[0]["rem_targetUser"].ToString() + "~" + idlist[1] + "~" + DT.Rows[0]["shortname"].ToString() + "~" + cmbinchargefor.Text.ToString().Trim() + "~" + DT.Rows[0]["inchargeid"].ToString() + "~" + DT.Rows[0]["company"].ToString();
                        }
                        else
                            data = "Edit~nauth";
                    }
                    else
                    {
                        //data = "You are not Authorise to Change data!!";
                        data = "Edit~nauth";
                    }
                }
            }
            #endregion
            #region Delete
            if (idlist[0] == "Delete")
            {
                Session["KeyVal"] = idlist[1].ToString();
                DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " * ", " rem_id in   ( " + idlist[1] + ")");
                if (DT.Rows.Count > 0)
                {
                    for (int m = 0; m < DT.Rows.Count; m++)
                    {
                        string useid = HttpContext.Current.Session["userid"].ToString();

                        if (DT.Rows[m][1].ToString() == HttpContext.Current.Session["userid"].ToString())
                        {
                            if (DT.Rows[m]["rem_actionTaken"].ToString() == "0")
                            {


                                oDBEngine.InsertDataFromAnotherTable("tbl_trans_reminder_log", "tbl_trans_reminder", "rem_createUser, rem_createDate, rem_targetUser, rem_startDate, rem_endDate, rem_reminderContent, rem_displayTricker, rem_actionTaken, rem_sourceid,CreateDate,CreateUser,LastModifyDate,LastModifyUser,rem_replycontent,rem_priority,rem_attendDate", " rem_id in   ( " + idlist[1] + ")");
                                oDBEngine.DeleteValue(" tbl_trans_reminder ", " rem_id in   ( " + DT.Rows[m][0] + ")");
                                data = "Delete~Y~" + Session["mode"];
                            }

                            else
                                data = "Delete~nauth";
                        }


                        else
                        {
                            //data = "You are not Authorise to Change data!!";
                            data = "Delete~nauth";
                        }
                    }
                }
            }
            #endregion
            #region Attend
            if (idlist[0] == "Attend")
            {
                DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " * ", " rem_id in   ( " + idlist[1] + ")");
                if ((DT.Rows[0][3].ToString() == HttpContext.Current.Session["userid"].ToString()) || (DT.Rows[0][18].ToString() == HttpContext.Current.Session["userid"].ToString()))
                {
                    int NoOfRecordsEffected = oDBEngine.SetFieldValue(" tbl_trans_reminder ", " rem_actionTaken=" + idlist[3].Trim() + ",rem_replycontent='" + idlist[2].Trim() + "',rem_attendDate='" + oDBEngine.GetDate() + "' ", " rem_id in   ( " + idlist[1] + ")");
                    if (NoOfRecordsEffected > 0)
                    {
                        Session["reply"] = null;
                        data = "Attend~Y~" + Session["mode"];
                    }
                }
                else
                    data = "Attend~N";
            }
            #endregion
            #region Save
            if (idlist[0] == "Save")
            {
                DataTable dttime = new DataTable();
                string[] spllit = idlist[15].Split('[');
                DataTable dtcomp = oDBEngine.GetDataTable("select cmp_id from tbl_master_company where cmp_name='" + spllit[0] + "'");
                int compid = Convert.ToInt16(dtcomp.Rows[0][0].ToString());
                DataTable dtcategory = oDBEngine.GetDataTable("select remindercategory_id from  Master_Remindercategory where Remindercategory_shortname ='" + idlist[14] + "'");
                int categoryid = Convert.ToInt16(dtcategory.Rows[0][0].ToString());
                string test = cmbfrequency.SelectedItem.Value;
                int NoOfRecordsEffected = 0;
                string[,] remshortname = oDBEngine.GetFieldValue("Master_Remindercategory", "Remindercategory_shortname", "Remindercategory_id=" + categoryid + "", 1);
                //Converter oConverter = new Converter();
                //string stsrtDate = oConverter.DateConverter(idlist[3], "mm/dd/yyyy hh:mm");
                //string stEndDate = oConverter.DateConverter(idlist[4], "mm/dd/yyyy hh:mm");
                string[] dt11 = idlist[3].Split(' ');
                string[] dt12 = idlist[4].Split(' ');
                if (dt11[1].StartsWith("0:"))
                {
                    string replace = dt11[1].Replace("0:", "12:");
                    idlist[3] = dt11[0] + " " + replace;
                }

                if (dt12[1].StartsWith("0:"))
                {
                    string replace1 = dt12[1].Replace("0:", "12:");
                    idlist[4] = dt12[0] + " " + replace1;
                }
                string stsrtDate = idlist[3];
                string stEndDate = idlist[4];
                DateTime dtnewforcreateemail = Convert.ToDateTime(idlist[3]);
                //divdisplay(idlist[12]);


                if (idlist[7] != "Chk")
                {

                    if (idlist[12] != "")
                    {
                        if (idlist[13] == "undefined")
                        {
                            idlist[13] = "";
                        }
                        //NoOfRecordsEffected = oDBEngine.SetFieldValue(" tbl_trans_reminder ", " rem_inchargetargetuser='" + idlist[13] + "',rem_categoryid='" + idlist[14] + "',rem_targetUser='" + idlist[1] + "',rem_startDate='" + stsrtDate + "',rem_endDate='" + stEndDate + "',rem_reminderContent='" + idlist[5] + "',rem_displayTricker='" + idlist[2] + "',rem_priority='" + idlist[6] + "',LastModifyDate='" + oDBEngine.GetDate() + "',LastModifyUser=" + HttpContext.Current.Session["userid"] + " ", " rem_id= " + idlist[12]);
                        NoOfRecordsEffected = oDBEngine.SetFieldValue(" tbl_trans_reminder ", " rem_cmpid=" + compid + ",rem_inchargetargetuser='" + idlist[13] + "',rem_categoryid='" + categoryid + "',rem_targetUser='" + idlist[1] + "',rem_startDate='" + stsrtDate + "',rem_endDate='" + stEndDate + "',rem_reminderContent='" + idlist[5] + "',rem_displayTricker='" + idlist[2] + "',rem_priority='" + idlist[6] + "',LastModifyDate='" + oDBEngine.GetDate() + "',LastModifyUser=" + HttpContext.Current.Session["userid"] + " ", " rem_id= " + idlist[12]);
                        divdisplay(idlist[12]);
                        SendReport(send.ToString().Trim(), HttpContext.Current.Session["userid"].ToString(), idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), dtnewforcreateemail, "Task # " + idlist[12] + " : Task Updation Intimation [" + remshortname[0, 0] + "]", oDBEngine.GetDate());
                        //if (SendReport(send.ToString().Trim(), HttpContext.Current.Session["userid"].ToString(), idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), dtnewforcreateemail, "Conversation Details for [" + remshortname[0, 0] + "]") == true)
                        //{

                        //}
                    }
                    else
                    {
                        NoOfRecordsEffected = oDBEngine.InsurtFieldValue(" tbl_trans_reminder ", " rem_targetUser, rem_startDate, rem_endDate, rem_reminderContent, rem_displayTricker,CreateUser,CreateDate,rem_createUser,rem_createDate,rem_actionTaken,rem_priority,rem_flag,rem_inchargetargetuser,rem_categoryid,rem_cmpid ", "'" + idlist[1] + "','" + stsrtDate + "','" + stEndDate + "','" + idlist[5] + "','" + idlist[2] + "'," + HttpContext.Current.Session["userid"] + ",'" + oDBEngine.GetDate() + "'," + HttpContext.Current.Session["userid"] + ",'" + oDBEngine.GetDate() + "','0','" + idlist[6] + "','R','" + idlist[13] + "'," + categoryid + "," + compid + "");
                        DataTable dtmaxid = oDBEngine.GetDataTable("select  rem_id from tbl_trans_reminder where rem_createdate=(select  max(rem_createdate) from tbl_trans_reminder) ");
                        string idfordivdisplay = dtmaxid.Rows[0]["rem_id"].ToString();
                        string[] countdate = stsrtDate.Split(' ');
                        string dtstatdt = OConvert.DateConverter(countdate[0], "mm/dd/yyyy");
                        string emails_id = "";
                        string mixedid = "";
                        //////////////new development for email///////////////
                        string usercontacttarget = "";
                        string usercontactincharge = "";
                        DataTable dttargetbody = new DataTable();
                        DataTable dtinchargebody = new DataTable();

                        DataTable dttarget = oDBEngine.GetDataTable("select user_contactid from tbl_master_user where user_id = '" + idlist[1] + "'");
                        if (dttarget != null && dttarget.Rows.Count > 0)
                        {
                            usercontacttarget = Convert.ToString(dttarget.Rows[0]["user_contactid"]);
                        }
                        dttargetbody = oDBEngine.GetDataTable("select distinct  EmailRecipients_MainID,* from Trans_EmailRecipients where  convert(varchar(10),EmailRecipients_SendDateTime,103)='" + dtstatdt + "' and EmailRecipients_ActivityID='352' and EmailRecipients_ContactLeadID='" + usercontacttarget + "'");
                        if (idlist[13] != "")
                        {
                            DataTable dtincharge = oDBEngine.GetDataTable("select user_contactid from tbl_master_user where user_id = '" + idlist[13] + "'");
                            if (dtincharge != null && dtincharge.Rows.Count > 0)
                            {
                                usercontactincharge = Convert.ToString(dtincharge.Rows[0]["user_contactid"]);
                                dtinchargebody = oDBEngine.GetDataTable("select distinct  EmailRecipients_MainID,* from Trans_EmailRecipients where  convert(varchar(10),EmailRecipients_SendDateTime,103)='" + dtstatdt + "' and EmailRecipients_ActivityID='352' and EmailRecipients_ContactLeadID='" + usercontactincharge + "'");
                            }

                        }
                        if ((dttargetbody.Rows.Count > 0) || (dtinchargebody.Rows.Count > 0))
                        {
                            if (dttargetbody.Rows.Count > 0)
                            {
                                emails_id = dttargetbody.Rows[0]["EmailRecipients_MainID"].ToString();
                                mixedid = idfordivdisplay + "~" + emails_id;
                                divdisplay(mixedid);
                                NoOfRecordsEffected = oDBEngine.SetFieldValue("Trans_emails", "emails_content='" + totalbody.ToString().Trim() + "'", "emails_id =" + emails_id + "");
                            }
                            else
                            {


                                ///////////////////////////////////////////////////////
                                divdisplay(idfordivdisplay);



                                //SendReport(send.ToString().Trim(), HttpContext.Current.Session["userid"].ToString(), idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), dtnewforcreateemail, "Task # " + idfordivdisplay + " : Daily Task Sheet [" + remshortname[0, 0] + "]", Convert.ToDateTime(stsrtDate.ToString()));
                                SendReport(send.ToString().Trim(), "1234567", idlist[1].ToString().Trim(), "12345678", dtnewforcreateemail, "Daily Task Sheet", Convert.ToDateTime(stsrtDate.ToString()));
                            }




                            if (dtinchargebody.Rows.Count > 0)
                            {
                                emails_id = dtinchargebody.Rows[0]["EmailRecipients_MainID"].ToString();
                                mixedid = idfordivdisplay + "~" + emails_id;
                                divdisplay(mixedid);
                                NoOfRecordsEffected = oDBEngine.SetFieldValue("Trans_emails", "emails_content='" + totalbody.ToString().Trim() + "'", "emails_id =" + emails_id + "");
                            }

                            else
                            {


                                ///////////////////////////////////////////////////////
                                divdisplay(idfordivdisplay);



                                //SendReport(send.ToString().Trim(), HttpContext.Current.Session["userid"].ToString(), idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), dtnewforcreateemail, "Task # " + idfordivdisplay + " : Daily Task Sheet [" + remshortname[0, 0] + "]", Convert.ToDateTime(stsrtDate.ToString()));
                                SendReport(send.ToString().Trim(), "1234567", "123456789", idlist[13].ToString().Trim(), dtnewforcreateemail, "Daily Task Sheet", Convert.ToDateTime(stsrtDate.ToString()));
                            }
                        }
                        else
                        {


                            ///////////////////////////////////////////////////////
                            divdisplay(idfordivdisplay);



                            //SendReport(send.ToString().Trim(), HttpContext.Current.Session["userid"].ToString(), idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), dtnewforcreateemail, "Task # " + idfordivdisplay + " : Daily Task Sheet [" + remshortname[0, 0] + "]", Convert.ToDateTime(stsrtDate.ToString()));
                            SendReport(send.ToString().Trim(), "1234567", idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), dtnewforcreateemail, "Daily Task Sheet", Convert.ToDateTime(stsrtDate.ToString()));
                        }

                    }





                    if (NoOfRecordsEffected > 0)
                    {

                        data = "Save~Y~" + Session["mode"];
                    }
                    else
                        data = "Save~N~" + Session["mode"];
                }
                else
                {
                    DateTime dtnew = Convert.ToDateTime(idlist[3]);
                    DateTime dtnew1 = Convert.ToDateTime(idlist[4]);

                    if (idlist[8] == "day")
                    {
                        int cl = Convert.ToInt32(idlist[9]);
                        // DateTime dtnew = Convert.ToDateTime(idlist[3]);
                        // DateTime dtnew1 = Convert.ToDateTime(idlist[4]);  HH:mm:ss.fff");
                        for (int m = 0; m < cl; m++)
                        {
                            DateTime dt = oDBEngine.GetDate();
                            string dt123 = dt.ToString("yyyy-MM-dd HH:mm:ss.fff");



                            NoOfRecordsEffected = oDBEngine.InsurtFieldValue(" tbl_trans_reminder ", " rem_targetUser, rem_startDate, rem_endDate, rem_reminderContent, rem_displayTricker,CreateUser,CreateDate,rem_createUser,rem_createDate,rem_actionTaken,rem_priority,rem_flag,rem_inchargetargetuser,rem_categoryid,rem_cmpid ", "'" + idlist[1] + "','" + dtnew + "','" + dtnew1 + "','" + idlist[5] + "','" + idlist[2] + "'," + HttpContext.Current.Session["userid"] + ",'" + oDBEngine.GetDate() + "'," + HttpContext.Current.Session["userid"] + ",'" + dt123 + "','0','" + idlist[6] + "','D','" + idlist[13] + "'," + categoryid + "," + compid + "");
                            DataTable dtmaxidday = oDBEngine.GetDataTable("select  rem_id from tbl_trans_reminder where rem_createdate=(select  max(rem_createdate) from tbl_trans_reminder) ");
                            string idfordivdisplayday = dtmaxidday.Rows[0]["rem_id"].ToString();
                            ////////////////////////////
                            string countdate = Convert.ToDateTime(dtnew).ToString();
                            string dtstatdt = OConvert.DateConverter(countdate, "mm/dd/yyyy");
                            string emails_id = "";
                            string mixedid = "";
                            //////////////new development for email///////////////
                            string usercontacttarget = "";
                            string usercontactincharge = "";
                            DataTable dttargetbody = new DataTable();
                            DataTable dtinchargebody = new DataTable();

                            DataTable dttarget = oDBEngine.GetDataTable("select user_contactid from tbl_master_user where user_id = '" + idlist[1] + "'");
                            if (dttarget != null && dttarget.Rows.Count > 0)
                            {
                                usercontacttarget = Convert.ToString(dttarget.Rows[0]["user_contactid"]);
                            }
                            dttargetbody = oDBEngine.GetDataTable("select distinct  EmailRecipients_MainID,* from Trans_EmailRecipients where  convert(varchar(10),EmailRecipients_SendDateTime,103)='" + dtstatdt + "' and EmailRecipients_ActivityID='352' and EmailRecipients_ContactLeadID='" + usercontacttarget + "'");
                            if (idlist[13] != "")
                            {
                                DataTable dtincharge = oDBEngine.GetDataTable("select user_contactid from tbl_master_user where user_id = '" + idlist[13] + "'");
                                if (dtincharge != null && dtincharge.Rows.Count > 0)
                                {
                                    usercontactincharge = Convert.ToString(dtincharge.Rows[0]["user_contactid"]);
                                    dtinchargebody = oDBEngine.GetDataTable("select distinct  EmailRecipients_MainID,* from Trans_EmailRecipients where  convert(varchar(10),EmailRecipients_SendDateTime,103)='" + dtstatdt + "' and EmailRecipients_ActivityID='352' and EmailRecipients_ContactLeadID='" + usercontactincharge + "'");
                                }

                            }
                            if ((dttargetbody.Rows.Count > 0) || (dtinchargebody.Rows.Count > 0))
                            {
                                if (dttargetbody.Rows.Count > 0)
                                {
                                    emails_id = dttargetbody.Rows[0]["EmailRecipients_MainID"].ToString();
                                    mixedid = idfordivdisplayday + "~" + emails_id;
                                    divdisplay(mixedid);
                                    NoOfRecordsEffected = oDBEngine.SetFieldValue("Trans_emails", "emails_content='" + totalbody.ToString().Trim() + "'", "emails_id =" + emails_id + "");
                                }

                                else
                                {


                                    ///////////////////////////////////////////////////////
                                    divdisplay(idfordivdisplayday);



                                    //SendReport(send.ToString().Trim(), HttpContext.Current.Session["userid"].ToString(), idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), dtnewforcreateemail, "Task # " + idfordivdisplay + " : Daily Task Sheet [" + remshortname[0, 0] + "]", Convert.ToDateTime(stsrtDate.ToString()));
                                    SendReport(send.ToString().Trim(), "12345678", idlist[1].ToString().Trim(), "12345678", Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd")), " Daily Task Sheet", Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                                }



                                if (dtinchargebody.Rows.Count > 0)
                                {
                                    emails_id = dtinchargebody.Rows[0]["EmailRecipients_MainID"].ToString();
                                    mixedid = idfordivdisplayday + "~" + emails_id;
                                    divdisplay(mixedid);
                                    NoOfRecordsEffected = oDBEngine.SetFieldValue("Trans_emails", "emails_content='" + totalbody.ToString().Trim() + "'", "emails_id =" + emails_id + "");
                                }

                                else
                                {


                                    ///////////////////////////////////////////////////////
                                    divdisplay(idfordivdisplayday);



                                    //SendReport(send.ToString().Trim(), HttpContext.Current.Session["userid"].ToString(), idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), dtnewforcreateemail, "Task # " + idfordivdisplay + " : Daily Task Sheet [" + remshortname[0, 0] + "]", Convert.ToDateTime(stsrtDate.ToString()));
                                    SendReport(send.ToString().Trim(), "12345678", "12345678", idlist[13].ToString().Trim(), Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd")), " Daily Task Sheet", Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                                }
                            }

                            else
                            {


                                ///////////////////////////////////////////////////////
                                divdisplay(idfordivdisplayday);



                                //SendReport(send.ToString().Trim(), HttpContext.Current.Session["userid"].ToString(), idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), dtnewforcreateemail, "Task # " + idfordivdisplay + " : Daily Task Sheet [" + remshortname[0, 0] + "]", Convert.ToDateTime(stsrtDate.ToString()));
                                SendReport(send.ToString().Trim(), "12345678", idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd")), " Daily Task Sheet", Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                            }

                            //divdisplay(idfordivdisplayday);
                            //SendReport(send.ToString().Trim(), "12345678", idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd")), " Daily Task Sheet", Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd HH:mm:ss.fff")));

                            dtnew = Convert.ToDateTime(dtnew).AddDays(1);
                            dtnew1 = Convert.ToDateTime(dtnew1).AddDays(1);

                            if ((idlist[10] == "satryes") && (idlist[11] == "sunyes"))
                            {
                                if (dtnew.DayOfWeek == DayOfWeek.Saturday)
                                {
                                    dtnew = Convert.ToDateTime(dtnew).AddDays(2);
                                    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(2);
                                }
                                if (dtnew.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    dtnew = Convert.ToDateTime(dtnew).AddDays(1);
                                    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(1);
                                }

                            }
                            if ((idlist[11] == "sunyes") && (idlist[10] != "satryes"))
                            {
                                if (dtnew.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    dtnew = Convert.ToDateTime(dtnew).AddDays(1);
                                    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(1);
                                }
                                //if (dtnew1.DayOfWeek == DayOfWeek.Sunday)
                                //{
                                //    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(1);
                                //}
                            }
                            if ((idlist[11] != "sunyes") && (idlist[10] == "satryes"))
                            {
                                if (dtnew.DayOfWeek == DayOfWeek.Saturday)
                                {
                                    dtnew = Convert.ToDateTime(dtnew).AddDays(1);
                                    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(1);
                                }
                                //if (dtnew1.DayOfWeek == DayOfWeek.Sunday)
                                //{
                                //    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(1);
                                //}
                            }

                        }
                        if (NoOfRecordsEffected > 0)
                        {
                            data = "Save~Y~" + Session["mode"];
                        }
                        else
                            data = "Save~N~" + Session["mode"];
                    }
                    if (idlist[8] == "week")
                    {
                        int cl = Convert.ToInt32(idlist[9]);
                        //DateTime dtnew = Convert.ToDateTime(idlist[3]);
                        //DateTime dtnew1 = Convert.ToDateTime(idlist[4]);
                        for (int m = 0; m < cl; m++)
                        {
                            DateTime dt = oDBEngine.GetDate();
                            string dt123 = dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
                            NoOfRecordsEffected = oDBEngine.InsurtFieldValue(" tbl_trans_reminder ", " rem_targetUser, rem_startDate, rem_endDate, rem_reminderContent, rem_displayTricker,CreateUser,CreateDate,rem_createUser,rem_createDate,rem_actionTaken,rem_priority,rem_flag,rem_inchargetargetuser,rem_categoryid,rem_cmpid ", "'" + idlist[1] + "','" + dtnew + "','" + dtnew1 + "','" + idlist[5] + "','" + idlist[2] + "'," + HttpContext.Current.Session["userid"] + ",'" + oDBEngine.GetDate() + "'," + HttpContext.Current.Session["userid"] + ",'" + dt123 + "','0','" + idlist[6] + "','W','" + idlist[13] + "'," + categoryid + "," + compid + "");
                            DataTable dtmaxidweek = oDBEngine.GetDataTable("select  rem_id from tbl_trans_reminder where rem_createdate=(select  max(rem_createdate) from tbl_trans_reminder) ");
                            string idfordivdisplayweek = dtmaxidweek.Rows[0]["rem_id"].ToString();
                            string countdate = Convert.ToDateTime(dtnew).ToString();
                            string dtstatdt = OConvert.DateConverter(countdate, "mm/dd/yyyy");
                            string emails_id = "";
                            string mixedid = "";
                            //////////////new development for email///////////////
                            string usercontacttarget = "";
                            string usercontactincharge = "";
                            DataTable dttargetbody = new DataTable();
                            DataTable dtinchargebody = new DataTable();

                            DataTable dttarget = oDBEngine.GetDataTable("select user_contactid from tbl_master_user where user_id = '" + idlist[1] + "'");
                            if (dttarget != null && dttarget.Rows.Count > 0)
                            {
                                usercontacttarget = Convert.ToString(dttarget.Rows[0]["user_contactid"]);
                            }
                            dttargetbody = oDBEngine.GetDataTable("select distinct  EmailRecipients_MainID,* from Trans_EmailRecipients where  convert(varchar(10),EmailRecipients_SendDateTime,103)='" + dtstatdt + "' and EmailRecipients_ActivityID='352' and EmailRecipients_ContactLeadID='" + usercontacttarget + "'");
                            if (idlist[13] != "")
                            {
                                DataTable dtincharge = oDBEngine.GetDataTable("select user_contactid from tbl_master_user where user_id = '" + idlist[13] + "'");
                                if (dtincharge != null && dtincharge.Rows.Count > 0)
                                {
                                    usercontactincharge = Convert.ToString(dtincharge.Rows[0]["user_contactid"]);
                                    dtinchargebody = oDBEngine.GetDataTable("select distinct  EmailRecipients_MainID,* from Trans_EmailRecipients where  convert(varchar(10),EmailRecipients_SendDateTime,103)='" + dtstatdt + "' and EmailRecipients_ActivityID='352' and EmailRecipients_ContactLeadID='" + usercontactincharge + "'");
                                }

                            }
                            if ((dttargetbody.Rows.Count > 0) || (dtinchargebody.Rows.Count > 0))
                            {
                                if (dttargetbody.Rows.Count > 0)
                                {
                                    emails_id = dttargetbody.Rows[0]["EmailRecipients_MainID"].ToString();
                                    mixedid = idfordivdisplayweek + "~" + emails_id;
                                    divdisplay(mixedid);
                                    NoOfRecordsEffected = oDBEngine.SetFieldValue("Trans_emails", "emails_content='" + totalbody.ToString().Trim() + "'", "emails_id =" + emails_id + "");
                                }

                                else
                                {


                                    ///////////////////////////////////////////////////////
                                    divdisplay(idfordivdisplayweek);



                                    //SendReport(send.ToString().Trim(), HttpContext.Current.Session["userid"].ToString(), idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), dtnewforcreateemail, "Task # " + idfordivdisplay + " : Daily Task Sheet [" + remshortname[0, 0] + "]", Convert.ToDateTime(stsrtDate.ToString()));
                                    SendReport(send.ToString().Trim(), "12345678", idlist[1].ToString().Trim(), "12345678", Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd")), " Daily Task Sheet", Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                                }



                                if (dtinchargebody.Rows.Count > 0)
                                {
                                    emails_id = dtinchargebody.Rows[0]["EmailRecipients_MainID"].ToString();
                                    mixedid = idfordivdisplayweek + "~" + emails_id;
                                    divdisplay(mixedid);
                                    NoOfRecordsEffected = oDBEngine.SetFieldValue("Trans_emails", "emails_content='" + totalbody.ToString().Trim() + "'", "emails_id =" + emails_id + "");
                                }

                                else
                                {


                                    ///////////////////////////////////////////////////////
                                    divdisplay(idfordivdisplayweek);



                                    //SendReport(send.ToString().Trim(), HttpContext.Current.Session["userid"].ToString(), idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), dtnewforcreateemail, "Task # " + idfordivdisplay + " : Daily Task Sheet [" + remshortname[0, 0] + "]", Convert.ToDateTime(stsrtDate.ToString()));
                                    SendReport(send.ToString().Trim(), "12345678", "12345678", idlist[13].ToString().Trim(), Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd")), " Daily Task Sheet", Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                                }
                            }

                            else
                            {


                                ///////////////////////////////////////////////////////
                                divdisplay(idfordivdisplayweek);



                                //SendReport(send.ToString().Trim(), HttpContext.Current.Session["userid"].ToString(), idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), dtnewforcreateemail, "Task # " + idfordivdisplay + " : Daily Task Sheet [" + remshortname[0, 0] + "]", Convert.ToDateTime(stsrtDate.ToString()));
                                SendReport(send.ToString().Trim(), "12345678", idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd")), " Daily Task Sheet", Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                            }

                            //divdisplay(idfordivdisplayweek);
                            //SendReport(send.ToString().Trim(), "12345678", idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd")), " Daily Task Sheet ", Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                            dtnew = Convert.ToDateTime(dtnew).AddDays(7);
                            dtnew1 = Convert.ToDateTime(dtnew1).AddDays(7);
                            if ((idlist[10] == "satryes") && (idlist[11] == "sunyes"))
                            {
                                if (dtnew.DayOfWeek == DayOfWeek.Saturday)
                                {
                                    dtnew = Convert.ToDateTime(dtnew).AddDays(2);
                                    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(2);
                                }
                                //if (dtnew1.DayOfWeek == DayOfWeek.Saturday)
                                //{
                                //    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(1);
                                //}

                            }
                            if ((idlist[11] == "sunyes") && (idlist[10] != "satryes"))
                            {
                                if (dtnew.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    dtnew = Convert.ToDateTime(dtnew).AddDays(1);
                                    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(1);
                                }
                                if (dtnew.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    dtnew = Convert.ToDateTime(dtnew).AddDays(1);
                                    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(1);
                                }
                            }
                            if ((idlist[11] != "sunyes") && (idlist[10] == "satryes"))
                            {
                                if (dtnew.DayOfWeek == DayOfWeek.Saturday)
                                {
                                    dtnew = Convert.ToDateTime(dtnew).AddDays(1);
                                    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(1);
                                }
                                //if (dtnew1.DayOfWeek == DayOfWeek.Sunday)
                                //{
                                //    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(1);
                                //}
                            }
                        }
                        if (NoOfRecordsEffected > 0)
                        {
                            data = "Save~Y~" + Session["mode"];
                        }
                        else
                            data = "Save~N~" + Session["mode"];
                    }
                    if (idlist[8] == "fortnight")
                    {
                        int cl = Convert.ToInt32(idlist[9]);
                        //DateTime dtnew = Convert.ToDateTime(idlist[3]);
                        //DateTime dtnew1 = Convert.ToDateTime(idlist[4]);
                        for (int m = 0; m < cl; m++)
                        {
                            DateTime dt = oDBEngine.GetDate();
                            string dt123 = dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
                            NoOfRecordsEffected = oDBEngine.InsurtFieldValue(" tbl_trans_reminder ", " rem_targetUser, rem_startDate, rem_endDate, rem_reminderContent, rem_displayTricker,CreateUser,CreateDate,rem_createUser,rem_createDate,rem_actionTaken,rem_priority,rem_flag,rem_inchargetargetuser,rem_categoryid,rem_cmpid  ", "'" + idlist[1] + "','" + dtnew + "','" + dtnew1 + "','" + idlist[5] + "','" + idlist[2] + "'," + HttpContext.Current.Session["userid"] + ",'" + oDBEngine.GetDate() + "'," + HttpContext.Current.Session["userid"] + ",'" + dt123 + "','0','" + idlist[6] + "','F','" + idlist[13] + "'," + categoryid + "," + compid + "");
                            DataTable dtmaxidfort = oDBEngine.GetDataTable("select  rem_id from tbl_trans_reminder where rem_createdate=(select  max(rem_createdate) from tbl_trans_reminder) ");
                            string idfordivdisplayfort = dtmaxidfort.Rows[0]["rem_id"].ToString();

                            string countdate = Convert.ToDateTime(dtnew).ToString();
                            string dtstatdt = OConvert.DateConverter(countdate, "mm/dd/yyyy");
                            string emails_id = "";
                            string mixedid = "";
                            //////////////new development for email///////////////
                            string usercontacttarget = "";
                            string usercontactincharge = "";
                            DataTable dttargetbody = new DataTable();
                            DataTable dtinchargebody = new DataTable();

                            DataTable dttarget = oDBEngine.GetDataTable("select user_contactid from tbl_master_user where user_id = '" + idlist[1] + "'");
                            if (dttarget != null && dttarget.Rows.Count > 0)
                            {
                                usercontacttarget = Convert.ToString(dttarget.Rows[0]["user_contactid"]);
                            }
                            dttargetbody = oDBEngine.GetDataTable("select distinct  EmailRecipients_MainID,* from Trans_EmailRecipients where  convert(varchar(10),EmailRecipients_SendDateTime,103)='" + dtstatdt + "' and EmailRecipients_ActivityID='352' and EmailRecipients_ContactLeadID='" + usercontacttarget + "'");
                            if (idlist[13] != "")
                            {
                                DataTable dtincharge = oDBEngine.GetDataTable("select user_contactid from tbl_master_user where user_id = '" + idlist[13] + "'");
                                if (dtincharge != null && dtincharge.Rows.Count > 0)
                                {
                                    usercontactincharge = Convert.ToString(dtincharge.Rows[0]["user_contactid"]);
                                    dtinchargebody = oDBEngine.GetDataTable("select distinct  EmailRecipients_MainID,* from Trans_EmailRecipients where  convert(varchar(10),EmailRecipients_SendDateTime,103)='" + dtstatdt + "' and EmailRecipients_ActivityID='352' and EmailRecipients_ContactLeadID='" + usercontactincharge + "'");
                                }

                            }
                            if ((dttargetbody.Rows.Count > 0) || (dtinchargebody.Rows.Count > 0))
                            {
                                if (dttargetbody.Rows.Count > 0)
                                {
                                    emails_id = dttargetbody.Rows[0]["EmailRecipients_MainID"].ToString();
                                    mixedid = idfordivdisplayfort + "~" + emails_id;
                                    divdisplay(mixedid);
                                    NoOfRecordsEffected = oDBEngine.SetFieldValue("Trans_emails", "emails_content='" + totalbody.ToString().Trim() + "'", "emails_id =" + emails_id + "");
                                }

                                else
                                {


                                    ///////////////////////////////////////////////////////
                                    divdisplay(idfordivdisplayfort);



                                    //SendReport(send.ToString().Trim(), HttpContext.Current.Session["userid"].ToString(), idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), dtnewforcreateemail, "Task # " + idfordivdisplay + " : Daily Task Sheet [" + remshortname[0, 0] + "]", Convert.ToDateTime(stsrtDate.ToString()));
                                    SendReport(send.ToString().Trim(), "12345678", idlist[1].ToString().Trim(), "12345678", Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd")), " Daily Task Sheet", Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                                }



                                if (dtinchargebody.Rows.Count > 0)
                                {
                                    emails_id = dtinchargebody.Rows[0]["EmailRecipients_MainID"].ToString();
                                    mixedid = idfordivdisplayfort + "~" + emails_id;
                                    divdisplay(mixedid);
                                    NoOfRecordsEffected = oDBEngine.SetFieldValue("Trans_emails", "emails_content='" + totalbody.ToString().Trim() + "'", "emails_id =" + emails_id + "");
                                }

                                else
                                {


                                    ///////////////////////////////////////////////////////
                                    divdisplay(idfordivdisplayfort);



                                    //SendReport(send.ToString().Trim(), HttpContext.Current.Session["userid"].ToString(), idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), dtnewforcreateemail, "Task # " + idfordivdisplay + " : Daily Task Sheet [" + remshortname[0, 0] + "]", Convert.ToDateTime(stsrtDate.ToString()));
                                    SendReport(send.ToString().Trim(), "12345678", "12345678", idlist[13].ToString().Trim(), Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd")), " Daily Task Sheet", Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                                }
                            }

                            else
                            {


                                ///////////////////////////////////////////////////////
                                divdisplay(idfordivdisplayfort);



                                //SendReport(send.ToString().Trim(), HttpContext.Current.Session["userid"].ToString(), idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), dtnewforcreateemail, "Task # " + idfordivdisplay + " : Daily Task Sheet [" + remshortname[0, 0] + "]", Convert.ToDateTime(stsrtDate.ToString()));
                                SendReport(send.ToString().Trim(), "12345678", idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd")), " Daily Task Sheet", Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                            }


                            //divdisplay(idfordivdisplayfort);
                            //SendReport(send.ToString().Trim(), "12345678", idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd")), "Daily Task Sheet", Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                            dtnew = Convert.ToDateTime(dtnew).AddDays(15);
                            dtnew1 = Convert.ToDateTime(dtnew1).AddDays(15);
                            if ((idlist[10] == "satryes") && (idlist[11] == "sunyes"))
                            {
                                if (dtnew.DayOfWeek == DayOfWeek.Saturday)
                                {
                                    dtnew = Convert.ToDateTime(dtnew).AddDays(2);
                                    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(2);
                                }
                                //if (dtnew1.DayOfWeek == DayOfWeek.Saturday)
                                //{
                                //    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(1);
                                //}

                            }
                            if ((idlist[11] == "sunyes") && (idlist[10] != "satryes"))
                            {
                                if (dtnew.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    dtnew = Convert.ToDateTime(dtnew).AddDays(1);
                                    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(1);
                                }
                                if (dtnew.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    dtnew = Convert.ToDateTime(dtnew).AddDays(1);
                                    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(1);
                                }
                            }
                            if ((idlist[11] != "sunyes") && (idlist[10] == "satryes"))
                            {
                                if (dtnew.DayOfWeek == DayOfWeek.Saturday)
                                {
                                    dtnew = Convert.ToDateTime(dtnew).AddDays(1);
                                    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(1);
                                }
                                //if (dtnew1.DayOfWeek == DayOfWeek.Sunday)
                                //{
                                //    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(1);
                                //}
                            }
                        }
                        if (NoOfRecordsEffected > 0)
                        {
                            data = "Save~Y~" + Session["mode"];
                        }
                        else
                            data = "Save~N~" + Session["mode"];
                    }

                    if (idlist[8] == "quarter")
                    {
                        int cl = Convert.ToInt32(idlist[9]);
                        //DateTime dtnew = Convert.ToDateTime(idlist[3]);
                        //DateTime dtnew1 = Convert.ToDateTime(idlist[4]);
                        for (int m = 0; m < cl; m++)
                        {
                            DateTime dt = oDBEngine.GetDate();
                            string dt123 = dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
                            NoOfRecordsEffected = oDBEngine.InsurtFieldValue(" tbl_trans_reminder ", " rem_targetUser, rem_startDate, rem_endDate, rem_reminderContent, rem_displayTricker,CreateUser,CreateDate,rem_createUser,rem_createDate,rem_actionTaken,rem_priority,rem_flag,rem_inchargetargetuser,rem_categoryid,rem_cmpid ", "'" + idlist[1] + "','" + dtnew + "','" + dtnew1 + "','" + idlist[5] + "','" + idlist[2] + "'," + HttpContext.Current.Session["userid"] + ",'" + oDBEngine.GetDate() + "'," + HttpContext.Current.Session["userid"] + ",'" + dt123 + "','0','" + idlist[6] + "','Q','" + idlist[13] + "'," + categoryid + "," + compid + "");
                            DataTable dtmaxidquarter = oDBEngine.GetDataTable("select  rem_id from tbl_trans_reminder where rem_createdate=(select  max(rem_createdate) from tbl_trans_reminder) ");
                            string idfordivdisplayquarter = dtmaxidquarter.Rows[0]["rem_id"].ToString();
                            //divdisplay(idfordivdisplayquarter);
                            //SendReport(send.ToString().Trim(), "12345678", idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd")), "Daily Task Sheet", Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd HH:mm:ss.fff")));

                            string countdate = Convert.ToDateTime(dtnew).ToString();
                            string dtstatdt = OConvert.DateConverter(countdate, "mm/dd/yyyy");
                            string emails_id = "";
                            string mixedid = "";
                            //////////////new development for email///////////////
                            string usercontacttarget = "";
                            string usercontactincharge = "";
                            DataTable dttargetbody = new DataTable();
                            DataTable dtinchargebody = new DataTable();

                            DataTable dttarget = oDBEngine.GetDataTable("select user_contactid from tbl_master_user where user_id = '" + idlist[1] + "'");
                            if (dttarget != null && dttarget.Rows.Count > 0)
                            {
                                usercontacttarget = Convert.ToString(dttarget.Rows[0]["user_contactid"]);
                            }
                            dttargetbody = oDBEngine.GetDataTable("select distinct  EmailRecipients_MainID,* from Trans_EmailRecipients where  convert(varchar(10),EmailRecipients_SendDateTime,103)='" + dtstatdt + "' and EmailRecipients_ActivityID='352' and EmailRecipients_ContactLeadID='" + usercontacttarget + "'");
                            if (idlist[13] != "")
                            {
                                DataTable dtincharge = oDBEngine.GetDataTable("select user_contactid from tbl_master_user where user_id = '" + idlist[13] + "'");
                                if (dtincharge != null && dtincharge.Rows.Count > 0)
                                {
                                    usercontactincharge = Convert.ToString(dtincharge.Rows[0]["user_contactid"]);
                                    dtinchargebody = oDBEngine.GetDataTable("select distinct  EmailRecipients_MainID,* from Trans_EmailRecipients where  convert(varchar(10),EmailRecipients_SendDateTime,103)='" + dtstatdt + "' and EmailRecipients_ActivityID='352' and EmailRecipients_ContactLeadID='" + usercontactincharge + "'");
                                }

                            }
                            if ((dttargetbody.Rows.Count > 0) || (dtinchargebody.Rows.Count > 0))
                            {
                                if (dttargetbody.Rows.Count > 0)
                                {
                                    emails_id = dttargetbody.Rows[0]["EmailRecipients_MainID"].ToString();
                                    mixedid = idfordivdisplayquarter + "~" + emails_id;
                                    divdisplay(mixedid);
                                    NoOfRecordsEffected = oDBEngine.SetFieldValue("Trans_emails", "emails_content='" + totalbody.ToString().Trim() + "'", "emails_id =" + emails_id + "");
                                }

                                else
                                {


                                    ///////////////////////////////////////////////////////
                                    divdisplay(idfordivdisplayquarter);



                                    //SendReport(send.ToString().Trim(), HttpContext.Current.Session["userid"].ToString(), idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), dtnewforcreateemail, "Task # " + idfordivdisplay + " : Daily Task Sheet [" + remshortname[0, 0] + "]", Convert.ToDateTime(stsrtDate.ToString()));
                                    SendReport(send.ToString().Trim(), "12345678", idlist[1].ToString().Trim(), "12345678", Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd")), " Daily Task Sheet", Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                                }



                                if (dtinchargebody.Rows.Count > 0)
                                {
                                    emails_id = dtinchargebody.Rows[0]["EmailRecipients_MainID"].ToString();
                                    mixedid = idfordivdisplayquarter + "~" + emails_id;
                                    divdisplay(mixedid);
                                    NoOfRecordsEffected = oDBEngine.SetFieldValue("Trans_emails", "emails_content='" + totalbody.ToString().Trim() + "'", "emails_id =" + emails_id + "");
                                }

                                else
                                {


                                    ///////////////////////////////////////////////////////
                                    divdisplay(idfordivdisplayquarter);



                                    //SendReport(send.ToString().Trim(), HttpContext.Current.Session["userid"].ToString(), idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), dtnewforcreateemail, "Task # " + idfordivdisplay + " : Daily Task Sheet [" + remshortname[0, 0] + "]", Convert.ToDateTime(stsrtDate.ToString()));
                                    SendReport(send.ToString().Trim(), "12345678", "12345678", idlist[13].ToString().Trim(), Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd")), " Daily Task Sheet", Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                                }
                            }

                            else
                            {


                                ///////////////////////////////////////////////////////
                                divdisplay(idfordivdisplayquarter);



                                //SendReport(send.ToString().Trim(), HttpContext.Current.Session["userid"].ToString(), idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), dtnewforcreateemail, "Task # " + idfordivdisplay + " : Daily Task Sheet [" + remshortname[0, 0] + "]", Convert.ToDateTime(stsrtDate.ToString()));
                                SendReport(send.ToString().Trim(), "12345678", idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd")), " Daily Task Sheet", Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                            }


                            dtnew = Convert.ToDateTime(dtnew).AddMonths(3);
                            dtnew1 = Convert.ToDateTime(dtnew1).AddMonths(3);
                            if ((idlist[10] == "satryes") && (idlist[11] == "sunyes"))
                            {
                                if (dtnew.DayOfWeek == DayOfWeek.Saturday)
                                {
                                    dtnew = Convert.ToDateTime(dtnew).AddDays(2);
                                    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(2);
                                }
                                //if (dtnew1.DayOfWeek == DayOfWeek.Saturday)
                                //{
                                //    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(1);
                                //}

                            }
                            if ((idlist[11] == "sunyes") && (idlist[10] != "satryes"))
                            {
                                if (dtnew.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    dtnew = Convert.ToDateTime(dtnew).AddDays(1);
                                    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(1);
                                }
                                if (dtnew.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    dtnew = Convert.ToDateTime(dtnew).AddDays(1);
                                    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(1);
                                }
                            }
                            if ((idlist[11] != "sunyes") && (idlist[10] == "satryes"))
                            {
                                if (dtnew.DayOfWeek == DayOfWeek.Saturday)
                                {
                                    dtnew = Convert.ToDateTime(dtnew).AddDays(1);
                                    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(1);
                                }
                                //if (dtnew1.DayOfWeek == DayOfWeek.Sunday)
                                //{
                                //    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(1);
                                //}
                            }
                        }
                        if (NoOfRecordsEffected > 0)
                        {
                            data = "Save~Y~" + Session["mode"];
                        }
                        else
                            data = "Save~N~" + Session["mode"];
                    }
                    ////////////semi///////////////////

                    if (idlist[8] == "semi")
                    {
                        int cl = Convert.ToInt32(idlist[9]);
                        //DateTime dtnew = Convert.ToDateTime(idlist[3]);
                        //DateTime dtnew1 = Convert.ToDateTime(idlist[4]);
                        for (int m = 0; m < cl; m++)
                        {
                            DateTime dt = oDBEngine.GetDate();
                            string dt123 = dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
                            NoOfRecordsEffected = oDBEngine.InsurtFieldValue(" tbl_trans_reminder ", " rem_targetUser, rem_startDate, rem_endDate, rem_reminderContent, rem_displayTricker,CreateUser,CreateDate,rem_createUser,rem_createDate,rem_actionTaken,rem_priority,rem_flag,rem_inchargetargetuser,rem_categoryid,rem_cmpid  ", "'" + idlist[1] + "','" + dtnew + "','" + dtnew1 + "','" + idlist[5] + "','" + idlist[2] + "'," + HttpContext.Current.Session["userid"] + ",'" + oDBEngine.GetDate() + "'," + HttpContext.Current.Session["userid"] + ",'" + dt123 + "','0','" + idlist[6] + "','S','" + idlist[13] + "'," + categoryid + "," + compid + "");
                            DataTable dtmaxidsemi = oDBEngine.GetDataTable("select  rem_id from tbl_trans_reminder where rem_createdate=(select  max(rem_createdate) from tbl_trans_reminder) ");
                            string idfordivdisplaysemi = dtmaxidsemi.Rows[0]["rem_id"].ToString();
                            //divdisplay(idfordivdisplaysemi);
                            //SendReport(send.ToString().Trim(), "12345678", idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd")), "Daily Task Sheet", Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd HH:mm:ss.fff")));

                            string countdate = Convert.ToDateTime(dtnew).ToString();
                            string dtstatdt = OConvert.DateConverter(countdate, "mm/dd/yyyy");
                            string emails_id = "";
                            string mixedid = "";
                            //////////////new development for email///////////////
                            string usercontacttarget = "";
                            string usercontactincharge = "";
                            DataTable dttargetbody = new DataTable();
                            DataTable dtinchargebody = new DataTable();

                            DataTable dttarget = oDBEngine.GetDataTable("select user_contactid from tbl_master_user where user_id = '" + idlist[1] + "'");
                            if (dttarget != null && dttarget.Rows.Count > 0)
                            {
                                usercontacttarget = Convert.ToString(dttarget.Rows[0]["user_contactid"]);
                            }
                            dttargetbody = oDBEngine.GetDataTable("select distinct  EmailRecipients_MainID,* from Trans_EmailRecipients where  convert(varchar(10),EmailRecipients_SendDateTime,103)='" + dtstatdt + "' and EmailRecipients_ActivityID='352' and EmailRecipients_ContactLeadID='" + usercontacttarget + "'");
                            if (idlist[13] != "")
                            {
                                DataTable dtincharge = oDBEngine.GetDataTable("select user_contactid from tbl_master_user where user_id = '" + idlist[13] + "'");
                                if (dtincharge != null && dtincharge.Rows.Count > 0)
                                {
                                    usercontactincharge = Convert.ToString(dtincharge.Rows[0]["user_contactid"]);
                                    dtinchargebody = oDBEngine.GetDataTable("select distinct  EmailRecipients_MainID,* from Trans_EmailRecipients where  convert(varchar(10),EmailRecipients_SendDateTime,103)='" + dtstatdt + "' and EmailRecipients_ActivityID='352' and EmailRecipients_ContactLeadID='" + usercontactincharge + "'");
                                }

                            }
                            if ((dttargetbody.Rows.Count > 0) || (dtinchargebody.Rows.Count > 0))
                            {
                                if (dttargetbody.Rows.Count > 0)
                                {
                                    emails_id = dttargetbody.Rows[0]["EmailRecipients_MainID"].ToString();
                                    mixedid = idfordivdisplaysemi + "~" + emails_id;
                                    divdisplay(mixedid);
                                    NoOfRecordsEffected = oDBEngine.SetFieldValue("Trans_emails", "emails_content='" + totalbody.ToString().Trim() + "'", "emails_id =" + emails_id + "");
                                }

                                else
                                {


                                    ///////////////////////////////////////////////////////
                                    divdisplay(idfordivdisplaysemi);



                                    //SendReport(send.ToString().Trim(), HttpContext.Current.Session["userid"].ToString(), idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), dtnewforcreateemail, "Task # " + idfordivdisplay + " : Daily Task Sheet [" + remshortname[0, 0] + "]", Convert.ToDateTime(stsrtDate.ToString()));
                                    SendReport(send.ToString().Trim(), "12345678", idlist[1].ToString().Trim(), "12345678", Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd")), " Daily Task Sheet", Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                                }



                                if (dtinchargebody.Rows.Count > 0)
                                {
                                    emails_id = dtinchargebody.Rows[0]["EmailRecipients_MainID"].ToString();
                                    mixedid = idfordivdisplaysemi + "~" + emails_id;
                                    divdisplay(mixedid);
                                    NoOfRecordsEffected = oDBEngine.SetFieldValue("Trans_emails", "emails_content='" + totalbody.ToString().Trim() + "'", "emails_id =" + emails_id + "");
                                }

                                else
                                {


                                    ///////////////////////////////////////////////////////
                                    divdisplay(idfordivdisplaysemi);



                                    //SendReport(send.ToString().Trim(), HttpContext.Current.Session["userid"].ToString(), idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), dtnewforcreateemail, "Task # " + idfordivdisplay + " : Daily Task Sheet [" + remshortname[0, 0] + "]", Convert.ToDateTime(stsrtDate.ToString()));
                                    SendReport(send.ToString().Trim(), "12345678", "12345678", idlist[13].ToString().Trim(), Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd")), " Daily Task Sheet", Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                                }
                            }

                            else
                            {


                                ///////////////////////////////////////////////////////
                                divdisplay(idfordivdisplaysemi);



                                //SendReport(send.ToString().Trim(), HttpContext.Current.Session["userid"].ToString(), idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), dtnewforcreateemail, "Task # " + idfordivdisplay + " : Daily Task Sheet [" + remshortname[0, 0] + "]", Convert.ToDateTime(stsrtDate.ToString()));
                                SendReport(send.ToString().Trim(), "12345678", idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd")), " Daily Task Sheet", Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                            }


                            dtnew = Convert.ToDateTime(dtnew).AddMonths(6);
                            dtnew1 = Convert.ToDateTime(dtnew1).AddMonths(6);
                            if ((idlist[10] == "satryes") && (idlist[11] == "sunyes"))
                            {
                                if (dtnew.DayOfWeek == DayOfWeek.Saturday)
                                {
                                    dtnew = Convert.ToDateTime(dtnew).AddDays(2);
                                    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(2);
                                }
                                //if (dtnew1.DayOfWeek == DayOfWeek.Saturday)
                                //{
                                //    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(1);
                                //}

                            }
                            if ((idlist[11] == "sunyes") && (idlist[10] != "satryes"))
                            {
                                if (dtnew.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    dtnew = Convert.ToDateTime(dtnew).AddDays(1);
                                    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(1);
                                }
                                if (dtnew.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    dtnew = Convert.ToDateTime(dtnew).AddDays(1);
                                    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(1);
                                }
                            }
                            if ((idlist[11] != "sunyes") && (idlist[10] == "satryes"))
                            {
                                if (dtnew.DayOfWeek == DayOfWeek.Saturday)
                                {
                                    dtnew = Convert.ToDateTime(dtnew).AddDays(1);
                                    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(1);
                                }
                                //if (dtnew1.DayOfWeek == DayOfWeek.Sunday)
                                //{
                                //    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(1);
                                //}
                            }
                        }
                        if (NoOfRecordsEffected > 0)
                        {
                            data = "Save~Y~" + Session["mode"];
                        }
                        else
                            data = "Save~N~" + Session["mode"];
                    }

                    ///////////////////////////////////
                    /////////////////Annuall////////////////

                    if (idlist[8] == "annual")
                    {
                        int cl = Convert.ToInt32(idlist[9]);
                        //DateTime dtnew = Convert.ToDateTime(idlist[3]);
                        //DateTime dtnew1 = Convert.ToDateTime(idlist[4]);
                        for (int m = 0; m < cl; m++)
                        {
                            DateTime dt = oDBEngine.GetDate();
                            string dt123 = dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
                            NoOfRecordsEffected = oDBEngine.InsurtFieldValue(" tbl_trans_reminder ", " rem_targetUser, rem_startDate, rem_endDate, rem_reminderContent, rem_displayTricker,CreateUser,CreateDate,rem_createUser,rem_createDate,rem_actionTaken,rem_priority,rem_flag,rem_inchargetargetuser,rem_categoryid,rem_cmpid ", "'" + idlist[1] + "','" + dtnew + "','" + dtnew1 + "','" + idlist[5] + "','" + idlist[2] + "'," + HttpContext.Current.Session["userid"] + ",'" + oDBEngine.GetDate() + "'," + HttpContext.Current.Session["userid"] + ",'" + dt123 + "','0','" + idlist[6] + "','A','" + idlist[13] + "'," + categoryid + "," + compid + "");
                            DataTable dtmaxidannual = oDBEngine.GetDataTable("select  rem_id from tbl_trans_reminder where rem_createdate=(select  max(rem_createdate) from tbl_trans_reminder) ");
                            string idfordivdisplayannual = dtmaxidannual.Rows[0]["rem_id"].ToString();
                            //divdisplay(idfordivdisplayannual);
                            //SendReport(send.ToString().Trim(), "12345678", idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd")), "Daily Task Sheet", Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd HH:mm:ss.fff")));

                            string countdate = Convert.ToDateTime(dtnew).ToString();
                            string dtstatdt = OConvert.DateConverter(countdate, "mm/dd/yyyy");
                            string emails_id = "";
                            string mixedid = "";
                            //////////////new development for email///////////////
                            string usercontacttarget = "";
                            string usercontactincharge = "";
                            DataTable dttargetbody = new DataTable();
                            DataTable dtinchargebody = new DataTable();

                            DataTable dttarget = oDBEngine.GetDataTable("select user_contactid from tbl_master_user where user_id = '" + idlist[1] + "'");
                            if (dttarget != null && dttarget.Rows.Count > 0)
                            {
                                usercontacttarget = Convert.ToString(dttarget.Rows[0]["user_contactid"]);
                            }
                            dttargetbody = oDBEngine.GetDataTable("select distinct  EmailRecipients_MainID,* from Trans_EmailRecipients where  convert(varchar(10),EmailRecipients_SendDateTime,103)='" + dtstatdt + "' and EmailRecipients_ActivityID='352' and EmailRecipients_ContactLeadID='" + usercontacttarget + "'");
                            if (idlist[13] != "")
                            {
                                DataTable dtincharge = oDBEngine.GetDataTable("select user_contactid from tbl_master_user where user_id = '" + idlist[13] + "'");
                                if (dtincharge != null && dtincharge.Rows.Count > 0)
                                {
                                    usercontactincharge = Convert.ToString(dtincharge.Rows[0]["user_contactid"]);
                                    dtinchargebody = oDBEngine.GetDataTable("select distinct  EmailRecipients_MainID,* from Trans_EmailRecipients where  convert(varchar(10),EmailRecipients_SendDateTime,103)='" + dtstatdt + "' and EmailRecipients_ActivityID='352' and EmailRecipients_ContactLeadID='" + usercontactincharge + "'");
                                }

                            }
                            if ((dttargetbody.Rows.Count > 0) || (dtinchargebody.Rows.Count > 0))
                            {
                                if (dttargetbody.Rows.Count > 0)
                                {
                                    emails_id = dttargetbody.Rows[0]["EmailRecipients_MainID"].ToString();
                                    mixedid = idfordivdisplayannual + "~" + emails_id;
                                    divdisplay(mixedid);
                                    NoOfRecordsEffected = oDBEngine.SetFieldValue("Trans_emails", "emails_content='" + totalbody.ToString().Trim() + "'", "emails_id =" + emails_id + "");
                                }

                                else
                                {


                                    ///////////////////////////////////////////////////////
                                    divdisplay(idfordivdisplayannual);



                                    //SendReport(send.ToString().Trim(), HttpContext.Current.Session["userid"].ToString(), idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), dtnewforcreateemail, "Task # " + idfordivdisplay + " : Daily Task Sheet [" + remshortname[0, 0] + "]", Convert.ToDateTime(stsrtDate.ToString()));
                                    SendReport(send.ToString().Trim(), "12345678", idlist[1].ToString().Trim(), "12345678", Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd")), " Daily Task Sheet", Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                                }



                                if (dtinchargebody.Rows.Count > 0)
                                {
                                    emails_id = dtinchargebody.Rows[0]["EmailRecipients_MainID"].ToString();
                                    mixedid = idfordivdisplayannual + "~" + emails_id;
                                    divdisplay(mixedid);
                                    NoOfRecordsEffected = oDBEngine.SetFieldValue("Trans_emails", "emails_content='" + totalbody.ToString().Trim() + "'", "emails_id =" + emails_id + "");
                                }

                                else
                                {


                                    ///////////////////////////////////////////////////////
                                    divdisplay(idfordivdisplayannual);



                                    //SendReport(send.ToString().Trim(), HttpContext.Current.Session["userid"].ToString(), idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), dtnewforcreateemail, "Task # " + idfordivdisplay + " : Daily Task Sheet [" + remshortname[0, 0] + "]", Convert.ToDateTime(stsrtDate.ToString()));
                                    SendReport(send.ToString().Trim(), "12345678", "12345678", idlist[13].ToString().Trim(), Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd")), " Daily Task Sheet", Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                                }
                            }

                            else
                            {


                                ///////////////////////////////////////////////////////
                                divdisplay(idfordivdisplayannual);



                                //SendReport(send.ToString().Trim(), HttpContext.Current.Session["userid"].ToString(), idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), dtnewforcreateemail, "Task # " + idfordivdisplay + " : Daily Task Sheet [" + remshortname[0, 0] + "]", Convert.ToDateTime(stsrtDate.ToString()));
                                SendReport(send.ToString().Trim(), "12345678", idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd")), " Daily Task Sheet", Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                            }



                            dtnew = Convert.ToDateTime(dtnew).AddYears(1);
                            dtnew1 = Convert.ToDateTime(dtnew1).AddYears(1);
                            if ((idlist[10] == "satryes") && (idlist[11] == "sunyes"))
                            {
                                if (dtnew.DayOfWeek == DayOfWeek.Saturday)
                                {
                                    dtnew = Convert.ToDateTime(dtnew).AddDays(2);
                                    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(2);
                                }
                                //if (dtnew1.DayOfWeek == DayOfWeek.Saturday)
                                //{
                                //    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(1);
                                //}

                            }
                            if ((idlist[11] == "sunyes") && (idlist[10] != "satryes"))
                            {
                                if (dtnew.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    dtnew = Convert.ToDateTime(dtnew).AddDays(1);
                                    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(1);
                                }
                                if (dtnew.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    dtnew = Convert.ToDateTime(dtnew).AddDays(1);
                                    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(1);
                                }
                            }
                            if ((idlist[11] != "sunyes") && (idlist[10] == "satryes"))
                            {
                                if (dtnew.DayOfWeek == DayOfWeek.Saturday)
                                {
                                    dtnew = Convert.ToDateTime(dtnew).AddDays(1);
                                    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(1);
                                }
                                //if (dtnew1.DayOfWeek == DayOfWeek.Sunday)
                                //{
                                //    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(1);
                                //}
                            }
                        }
                        if (NoOfRecordsEffected > 0)
                        {
                            data = "Save~Y~" + Session["mode"];
                        }
                        else
                            data = "Save~N~" + Session["mode"];
                    }


                    ////////////////////////////////////////


                    if (idlist[8] == "month")
                    {
                        int cl = Convert.ToInt32(idlist[9]);
                        //DateTime dtnew = Convert.ToDateTime(idlist[3]);
                        //DateTime dtnew1 = Convert.ToDateTime(idlist[4]);
                        for (int m = 0; m < cl; m++)
                        {
                            DateTime dt = oDBEngine.GetDate();
                            string dt123 = dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
                            NoOfRecordsEffected = oDBEngine.InsurtFieldValue(" tbl_trans_reminder ", " rem_targetUser, rem_startDate, rem_endDate, rem_reminderContent, rem_displayTricker,CreateUser,CreateDate,rem_createUser,rem_createDate,rem_actionTaken,rem_priority,rem_flag,rem_inchargetargetuser,rem_categoryid,rem_cmpid ", "'" + idlist[1] + "','" + dtnew + "','" + dtnew1 + "','" + idlist[5] + "','" + idlist[2] + "'," + HttpContext.Current.Session["userid"] + ",'" + oDBEngine.GetDate() + "'," + HttpContext.Current.Session["userid"] + ",'" + dt123 + "','0','" + idlist[6] + "','M','" + idlist[13] + "'," + categoryid + "," + compid + "");
                            DataTable dtmaxidmonth = oDBEngine.GetDataTable("select  rem_id from tbl_trans_reminder where rem_createdate=(select  max(rem_createdate) from tbl_trans_reminder) ");
                            string idfordivdisplaymonth = dtmaxidmonth.Rows[0]["rem_id"].ToString();
                            //divdisplay(idfordivdisplaymonth);
                            //SendReport(send.ToString().Trim(), "12345678", idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd")), "Daily Task Sheet", Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd HH:mm:ss.fff")));

                            string countdate = Convert.ToDateTime(dtnew).ToString();
                            string dtstatdt = OConvert.DateConverter(countdate, "mm/dd/yyyy");
                            string emails_id = "";
                            string mixedid = "";
                            //////////////new development for email///////////////
                            string usercontacttarget = "";
                            string usercontactincharge = "";
                            DataTable dttargetbody = new DataTable();
                            DataTable dtinchargebody = new DataTable();

                            DataTable dttarget = oDBEngine.GetDataTable("select user_contactid from tbl_master_user where user_id = '" + idlist[1] + "'");
                            if (dttarget != null && dttarget.Rows.Count > 0)
                            {
                                usercontacttarget = Convert.ToString(dttarget.Rows[0]["user_contactid"]);
                            }
                            dttargetbody = oDBEngine.GetDataTable("select distinct  EmailRecipients_MainID,* from Trans_EmailRecipients where  convert(varchar(10),EmailRecipients_SendDateTime,103)='" + dtstatdt + "' and EmailRecipients_ActivityID='352' and EmailRecipients_ContactLeadID='" + usercontacttarget + "'");
                            if (idlist[13] != "")
                            {
                                DataTable dtincharge = oDBEngine.GetDataTable("select user_contactid from tbl_master_user where user_id = '" + idlist[13] + "'");
                                if (dtincharge != null && dtincharge.Rows.Count > 0)
                                {
                                    usercontactincharge = Convert.ToString(dtincharge.Rows[0]["user_contactid"]);
                                    dtinchargebody = oDBEngine.GetDataTable("select distinct  EmailRecipients_MainID,* from Trans_EmailRecipients where  convert(varchar(10),EmailRecipients_SendDateTime,103)='" + dtstatdt + "' and EmailRecipients_ActivityID='352' and EmailRecipients_ContactLeadID='" + usercontactincharge + "'");
                                }

                            }
                            if ((dttargetbody.Rows.Count > 0) || (dtinchargebody.Rows.Count > 0))
                            {
                                if (dttargetbody.Rows.Count > 0)
                                {
                                    emails_id = dttargetbody.Rows[0]["EmailRecipients_MainID"].ToString();
                                    mixedid = idfordivdisplaymonth + "~" + emails_id;
                                    divdisplay(mixedid);
                                    NoOfRecordsEffected = oDBEngine.SetFieldValue("Trans_emails", "emails_content='" + totalbody.ToString().Trim() + "'", "emails_id =" + emails_id + "");
                                }

                                else
                                {


                                    ///////////////////////////////////////////////////////
                                    divdisplay(idfordivdisplaymonth);



                                    //SendReport(send.ToString().Trim(), HttpContext.Current.Session["userid"].ToString(), idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), dtnewforcreateemail, "Task # " + idfordivdisplay + " : Daily Task Sheet [" + remshortname[0, 0] + "]", Convert.ToDateTime(stsrtDate.ToString()));
                                    SendReport(send.ToString().Trim(), "12345678", idlist[1].ToString().Trim(), "12345678", Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd")), " Daily Task Sheet", Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                                }



                                if (dtinchargebody.Rows.Count > 0)
                                {
                                    emails_id = dtinchargebody.Rows[0]["EmailRecipients_MainID"].ToString();
                                    mixedid = idfordivdisplaymonth + "~" + emails_id;
                                    divdisplay(mixedid);
                                    NoOfRecordsEffected = oDBEngine.SetFieldValue("Trans_emails", "emails_content='" + totalbody.ToString().Trim() + "'", "emails_id =" + emails_id + "");
                                }

                                else
                                {


                                    ///////////////////////////////////////////////////////
                                    divdisplay(idfordivdisplaymonth);



                                    //SendReport(send.ToString().Trim(), HttpContext.Current.Session["userid"].ToString(), idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), dtnewforcreateemail, "Task # " + idfordivdisplay + " : Daily Task Sheet [" + remshortname[0, 0] + "]", Convert.ToDateTime(stsrtDate.ToString()));
                                    SendReport(send.ToString().Trim(), "12345678", "12345678", idlist[13].ToString().Trim(), Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd")), " Daily Task Sheet", Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                                }
                            }

                            else
                            {


                                ///////////////////////////////////////////////////////
                                divdisplay(idfordivdisplaymonth);



                                //SendReport(send.ToString().Trim(), HttpContext.Current.Session["userid"].ToString(), idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), dtnewforcreateemail, "Task # " + idfordivdisplay + " : Daily Task Sheet [" + remshortname[0, 0] + "]", Convert.ToDateTime(stsrtDate.ToString()));
                                SendReport(send.ToString().Trim(), "12345678", idlist[1].ToString().Trim(), idlist[13].ToString().Trim(), Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd")), " Daily Task Sheet", Convert.ToDateTime(dtnew.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                            }




                            dtnew = Convert.ToDateTime(dtnew).AddMonths(1);
                            dtnew1 = Convert.ToDateTime(dtnew1).AddMonths(1);
                            //dtnew1 = Convert.ToDateTime(dtnew1).AddDays(30);
                            if ((idlist[10] == "satryes") && (idlist[11] == "sunyes"))
                            {
                                if (dtnew.DayOfWeek == DayOfWeek.Saturday)
                                {
                                    dtnew = Convert.ToDateTime(dtnew).AddDays(2);
                                    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(2);
                                }
                                if (dtnew.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    dtnew = Convert.ToDateTime(dtnew).AddDays(1);
                                    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(1);

                                }
                                //if (dtnew1.DayOfWeek == DayOfWeek.Saturday)
                                //{
                                //    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(1);
                                //}

                            }
                            if ((idlist[11] == "sunyes") && (idlist[10] != "satryes"))
                            {
                                if (dtnew.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    dtnew = Convert.ToDateTime(dtnew).AddDays(1);
                                    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(1);
                                }
                                //if (dtnew1.DayOfWeek == DayOfWeek.Sunday)
                                //{
                                //    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(1);
                                //}
                            }
                            if ((idlist[11] != "sunyes") && (idlist[10] == "satryes"))
                            {
                                if (dtnew.DayOfWeek == DayOfWeek.Saturday)
                                {
                                    dtnew = Convert.ToDateTime(dtnew).AddDays(1);
                                    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(1);
                                }
                                //if (dtnew1.DayOfWeek == DayOfWeek.Sunday)
                                //{
                                //    dtnew1 = Convert.ToDateTime(dtnew1).AddDays(1);
                                //}
                            }
                        }
                        if (NoOfRecordsEffected > 0)
                        {
                            data = "Save~Y~" + Session["mode"];
                        }
                        else
                            data = "Save~N~" + Session["mode"];
                    }
                }

                Session["reapet"] = null;
            }
            #endregion
        }




        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;

        }

        protected void GridReminder_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string param = e.Parameters.ToString();

            Session["type"] = param.ToString().Trim();
            if (param == "Today")
            {
                Session["mode"] = param;

                //DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else  Convert(Varchar,rem_attenddate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_attenddate, 100), 13, 7)  end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company ", " convert(varchar(10),rem_startDate,103) = convert(varchar(10),getdate(),103) and ((rem_createUser =" + HttpContext.Current.Session["userid"] + ") OR (rem_targetUser =" + HttpContext.Current.Session["userid"] + ") or (rem_inchargetargetuser =" + HttpContext.Current.Session["userid"] + ")) order by rem_startDate,rem_priority,rem_flag desc ");
                if (HttpContext.Current.Session["userid"].ToString() != dtonlyid.Rows[0]["user_id"].ToString())
                {
                    DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else  Convert(Varchar,rem_attenddate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_attenddate, 100), 13, 7)  end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company ", " convert(varchar(10),rem_startDate,103) = convert(varchar(10),getdate(),103) and ((rem_createUser =" + HttpContext.Current.Session["userid"] + ") OR (rem_targetUser =" + HttpContext.Current.Session["userid"] + ") or (rem_inchargetargetuser =" + HttpContext.Current.Session["userid"] + "))  order by rem_startDate,rem_priority,rem_flag desc ");
                }
                else
                {

                    DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else  Convert(Varchar,rem_attenddate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_attenddate, 100), 13, 7)  end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company ", " convert(varchar(10),rem_startDate,103) = convert(varchar(10),getdate(),103) and ((rem_createUser in (" + _userId1 + ")) OR (rem_targetUser in (" + _userId1 + ")) or (rem_inchargetargetuser in (" + _userId1 + ")))  order by rem_startDate,rem_priority,rem_flag desc ");
                }
                for (int m = 0; m < DT.Rows.Count; m++)
                {
                    final = "";
                    substr = "";
                    final2 = "";
                    substr2 = "";
                    final3 = "";
                    substr3 = "";
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
                    // DT.Rows[m]["EndDate"] = final2;

                    //convDateTime3 = DT.Rows[m]["attenddate"].ToString();
                    //convDateTime3 = convDateTime3.Substring(0, 7) + convDateTime3.Substring(9);
                    //substr3 = convDateTime3.Substring(4, 2);
                    //convDateTime3 = convDateTime3.Replace(substr3, "");
                    //final3 = substr3 + " " + convDateTime3;
                    //DT.Rows[m]["attenddate"] = final3;
                }
                GridReminder.DataSource = DT.DefaultView;
                GridReminder.DataBind();
            }
            else if (param == "Pending")
            {
                Session["mode"] = param;
                //filterForm.Visible = true;
                //DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else  Convert(Varchar,rem_attenddate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_attenddate, 100), 13, 7)  end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag ,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company", " rem_actionTaken<>1 and ((rem_createUser =" + HttpContext.Current.Session["userid"] + ") OR (rem_targetUser =" + HttpContext.Current.Session["userid"] + ")) order by rem_startDate,rem_priority desc ");
                if (HttpContext.Current.Session["userid"].ToString() != dtonlyid.Rows[0]["user_id"].ToString())
                {
                    //DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_sourceid WHEN 0 THEN (SELECT     tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser) ELSE 'System' END AS CreateBy, rem_createDate AS CreateDate, rem_targetUser AS TargetId,(SELECT tbl_master_user.user_name FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, rem_startDate AS StartDate, rem_endDate AS EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'NotAttended' ELSE 'Attended' END AS Status ", " rem_actionTaken=0 order by rem_startDate desc ");
                    DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else convert(varchar(19),ISNULL(rem_attendDate,''),100) end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company ", " rem_actionTaken<>1 and ((rem_createUser =" + HttpContext.Current.Session["userid"] + ") OR (rem_targetUser =" + HttpContext.Current.Session["userid"] + ") or (rem_inchargetargetuser =" + HttpContext.Current.Session["userid"] + ")) order by rem_startDate,rem_priority,rem_flag desc ");
                }
                else
                {
                    DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else convert(varchar(19),ISNULL(rem_attendDate,''),100) end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company ", " rem_actionTaken<>1 and ((rem_createUser in (" + _userId1 + ")) OR (rem_targetUser in (" + _userId1 + ")) or (rem_inchargetargetuser in (" + _userId1 + "))) order by rem_startDate,rem_priority,rem_flag desc ");
                }
                for (int m = 0; m < DT.Rows.Count; m++)
                {
                    final = "";
                    substr = "";
                    final2 = "";
                    substr2 = "";
                    final3 = "";
                    substr3 = "";
                    convDateTime = DT.Rows[m]["StartDate"].ToString();
                    convDateTime = convDateTime.Substring(0, 7) + convDateTime.Substring(9);
                    substr = convDateTime.Substring(4, 2);
                    convDateTime = convDateTime.Replace(substr, "");
                    final = substr + " " + convDateTime;
                    //DT.Rows[m]["StartDate"] = final;

                    convDateTime4 = DT.Rows[m]["CreateDate"].ToString();
                    convDateTime4 = convDateTime4.Substring(0, 7) + convDateTime4.Substring(9);
                    DT.Rows[m]["CreateDate"] = convDateTime4;

                    convDateTime2 = DT.Rows[m]["EndDate"].ToString();
                    convDateTime2 = convDateTime2.Substring(0, 7) + convDateTime2.Substring(9);
                    substr2 = convDateTime2.Substring(4, 2);
                    convDateTime2 = convDateTime.Replace(substr2, "");
                    final2 = substr2 + " " + convDateTime2;
                    //DT.Rows[m]["EndDate"] = final2;

                    //convDateTime3 = DT.Rows[m]["attenddate"].ToString();
                    //convDateTime3 = convDateTime3.Substring(0, 7) + convDateTime3.Substring(9);
                    //substr3 = convDateTime3.Substring(4, 2);
                    //convDateTime3 = convDateTime3.Replace(substr3, "");
                    //final3 = substr3 + " " + convDateTime3;
                    //DT.Rows[m]["attenddate"] = final3;
                }
                GridReminder.DataSource = DT.DefaultView;
                GridReminder.DataBind();
            }
            else if (param == "Attended")
            {
                Session["mode"] = param;
                //DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else  Convert(Varchar,rem_attenddate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_attenddate, 100), 13, 7)  end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company  ", " rem_actionTaken=1 and ((rem_createUser =" + HttpContext.Current.Session["userid"] + ") OR (rem_targetUser =" + HttpContext.Current.Session["userid"] + ")) order by rem_startDate,rem_priority desc ");
                if (HttpContext.Current.Session["userid"].ToString() != dtonlyid.Rows[0]["user_id"].ToString())
                {
                    DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else  Convert(Varchar,rem_attenddate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_attenddate, 100), 13, 7)  end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company", " rem_actionTaken=1 and ((rem_createUser =" + HttpContext.Current.Session["userid"] + ") OR (rem_targetUser =" + HttpContext.Current.Session["userid"] + ") or (rem_inchargetargetuser =" + HttpContext.Current.Session["userid"] + ")) order by rem_startDate,rem_priority desc ");
                }
                else
                {
                    DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else  Convert(Varchar,rem_attenddate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_attenddate, 100), 13, 7)  end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company", " rem_actionTaken=1 and ((rem_createUser in (" + _userId1 + ")) OR (rem_targetUser in (" + _userId1 + ")) or (rem_inchargetargetuser in (" + _userId1 + "))) order by rem_startDate,rem_priority desc ");
                }
                for (int m = 0; m < DT.Rows.Count; m++)
                {
                    final = "";
                    substr = "";
                    final2 = "";
                    substr2 = "";
                    final3 = "";
                    substr3 = "";
                    convDateTime = DT.Rows[m]["StartDate"].ToString();
                    convDateTime = convDateTime.Substring(0, 7) + convDateTime.Substring(9);
                    substr = convDateTime.Substring(4, 2);
                    convDateTime = convDateTime.Replace(substr, "");
                    final = substr + " " + convDateTime;
                    //DT.Rows[m]["StartDate"] = final;

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
            }
            else if (param == "All")
            {
                Session["mode"] = param;
                //DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else  Convert(Varchar,rem_attenddate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_attenddate, 100), 13, 7)  end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company  ", "  ((rem_createUser =" + HttpContext.Current.Session["userid"] + ") OR (rem_targetUser =" + HttpContext.Current.Session["userid"] + ")) order by rem_startDate,rem_priority desc ");
                if (HttpContext.Current.Session["userid"].ToString() != dtonlyid.Rows[0]["user_id"].ToString())
                {
                    DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN '' THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else  Convert(Varchar,rem_attenddate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_attenddate, 100), 13, 7)  end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company  ", " ((rem_createUser =" + HttpContext.Current.Session["userid"] + ") OR (rem_targetUser =" + HttpContext.Current.Session["userid"] + ") or (rem_inchargetargetuser =" + HttpContext.Current.Session["userid"] + ")) order by rem_startDate,rem_priority desc ");
                }
                else
                {
                    DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else  Convert(Varchar,rem_attenddate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_attenddate, 100), 13, 7)  end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company  ", " ((rem_createUser in (" + _userId1 + ")) OR (rem_targetUser in (" + _userId1 + ")) or (rem_inchargetargetuser in (" + _userId1 + "))) order by rem_startDate,rem_priority desc ");
                }
                for (int m = 0; m < DT.Rows.Count; m++)
                {
                    final = "";
                    substr = "";
                    final2 = "";
                    substr2 = "";
                    final3 = "";
                    substr3 = "";
                    convDateTime = DT.Rows[m]["StartDate"].ToString();
                    convDateTime = convDateTime.Substring(0, 7) + convDateTime.Substring(9);
                    substr = convDateTime.Substring(4, 2);
                    convDateTime = convDateTime.Replace(substr, "");
                    final = substr + " " + convDateTime;
                    //DT.Rows[m]["StartDate"] = final;

                    convDateTime4 = DT.Rows[m]["CreateDate"].ToString();
                    convDateTime4 = convDateTime4.Substring(0, 7) + convDateTime4.Substring(9);
                    DT.Rows[m]["CreateDate"] = convDateTime4;

                    convDateTime2 = DT.Rows[m]["EndDate"].ToString();
                    convDateTime2 = convDateTime2.Substring(0, 7) + convDateTime2.Substring(9);
                    substr2 = convDateTime2.Substring(4, 2);
                    convDateTime2 = convDateTime.Replace(substr2, "");
                    final2 = substr2 + " " + convDateTime2;
                    //DT.Rows[m]["EndDate"] = final2;

                    //convDateTime3 = DT.Rows[m]["attenddate"].ToString();
                    //convDateTime3 = convDateTime3.Substring(0, 7) + convDateTime3.Substring(9);
                    //substr3 = convDateTime3.Substring(4, 2);
                    //convDateTime3 = convDateTime3.Replace(substr3, "");
                    //final3 = substr3 + " " + convDateTime3;
                    //DT.Rows[m]["attenddate"] = final3;
                }
                GridReminder.DataSource = DT.DefaultView;
                GridReminder.DataBind();
            }
            else if (param == "Filter")
            {
                Session["mode"] = param;
                //DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else  Convert(Varchar,rem_attenddate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_attenddate, 100), 13, 7)  end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company   ", " rem_actionTaken=1 and ((rem_createUser =" + HttpContext.Current.Session["userid"] + ") OR (rem_targetUser =" + HttpContext.Current.Session["userid"] + ")) and rem_startDate between '" + txtStart.Value.ToString() + "' and '" + txtEnd.Value.ToString() + "' order by rem_startDate,rem_priority desc ");
                if (HttpContext.Current.Session["userid"].ToString() != dtonlyid.Rows[0]["user_id"].ToString())
                {
                    DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else  Convert(Varchar,rem_attenddate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_attenddate, 100), 13, 7)  end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company ", " rem_actionTaken=1 and ((rem_createUser =" + HttpContext.Current.Session["userid"] + ") OR (rem_targetUser =" + HttpContext.Current.Session["userid"] + ") or (rem_inchargetargetuser =" + HttpContext.Current.Session["userid"] + ")) and rem_startDate between '" + txtStart.Value.ToString() + "' and '" + txtEnd.Value.ToString() + "' order by rem_startDate,rem_priority desc ");
                }
                else
                {
                    DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else  Convert(Varchar,rem_attenddate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_attenddate, 100), 13, 7)  end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company ", " rem_actionTaken=1 and ((rem_createUser in (" + _userId1 + ")) OR (rem_targetUser in (" + _userId1 + ")) or (rem_inchargetargetuser in (" + _userId1 + "))) and rem_startDate between '" + txtStart.Value.ToString() + "' and '" + txtEnd.Value.ToString() + "' order by rem_startDate,rem_priority desc ");
                }
                for (int m = 0; m < DT.Rows.Count; m++)
                {
                    final = "";
                    substr = "";
                    final2 = "";
                    substr2 = "";
                    final3 = "";
                    substr3 = "";
                    convDateTime = DT.Rows[m]["StartDate"].ToString();
                    convDateTime = convDateTime.Substring(0, 7) + convDateTime.Substring(9);
                    substr = convDateTime.Substring(4, 2);
                    convDateTime = convDateTime.Replace(substr, "");
                    final = substr + " " + convDateTime;
                    //DT.Rows[m]["StartDate"] = final;

                    convDateTime4 = DT.Rows[m]["CreateDate"].ToString();
                    convDateTime4 = convDateTime4.Substring(0, 7) + convDateTime4.Substring(9);
                    DT.Rows[m]["CreateDate"] = convDateTime4;

                    convDateTime2 = DT.Rows[m]["EndDate"].ToString();
                    convDateTime2 = convDateTime2.Substring(0, 7) + convDateTime2.Substring(9);
                    substr2 = convDateTime2.Substring(4, 2);
                    convDateTime2 = convDateTime.Replace(substr2, "");
                    final2 = substr2 + " " + convDateTime2;
                    //DT.Rows[m]["EndDate"] = final2;

                    //convDateTime3 = DT.Rows[m]["attenddate"].ToString();
                    //convDateTime3 = convDateTime3.Substring(0, 7) + convDateTime3.Substring(9);
                    //substr3 = convDateTime3.Substring(4, 2);
                    //convDateTime3 = convDateTime3.Replace(substr3, "");
                    //final3 = substr3 + " " + convDateTime3;
                    //DT.Rows[m]["attenddate"] = final3;
                }
                GridReminder.DataSource = DT.DefaultView;
                GridReminder.DataBind();

            }
            else if (param == "Filter1")
            {
                Session["mode"] = param;
                //DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else  Convert(Varchar,rem_attenddate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_attenddate, 100), 13, 7)  end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company ", " rem_actionTaken<>1 and ((rem_createUser =" + HttpContext.Current.Session["userid"] + ") OR (rem_targetUser =" + HttpContext.Current.Session["userid"] + ")) and rem_createDate between '" + txtStart1.Value.ToString() + "' and '" + txtEnd1.Value.ToString() + "' order by rem_createDate,rem_flag desc ");
                if (HttpContext.Current.Session["userid"].ToString() != dtonlyid.Rows[0]["user_id"].ToString())
                {
                    DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else convert(varchar(19),ISNULL(rem_attendDate,''),100) end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company   ", " rem_actionTaken<>1 and ((rem_createUser =" + HttpContext.Current.Session["userid"] + ") OR (rem_targetUser =" + HttpContext.Current.Session["userid"] + ") or (rem_inchargetargetuser =" + HttpContext.Current.Session["userid"] + ")) and rem_createDate between '" + txtStart1.Value.ToString() + "' and '" + txtEnd1.Value.ToString() + "' order by rem_createDate,rem_flag desc ");
                }
                else
                {
                    DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else convert(varchar(19),ISNULL(rem_attendDate,''),100) end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company   ", " rem_actionTaken<>1 and ((rem_createUser in (" + _userId1 + ")) OR (rem_targetUser in (" + _userId1 + ")) or (rem_inchargetargetuser in (" + _userId1 + "))) and rem_createDate between '" + txtStart1.Value.ToString() + "' and '" + txtEnd1.Value.ToString() + "' order by rem_createDate,rem_flag desc ");
                }
                for (int m = 0; m < DT.Rows.Count; m++)
                {
                    final = "";
                    substr = "";
                    final2 = "";
                    substr2 = "";
                    final3 = "";
                    substr3 = "";
                    convDateTime = DT.Rows[m]["StartDate"].ToString();
                    convDateTime = convDateTime.Substring(0, 7) + convDateTime.Substring(9);
                    substr = convDateTime.Substring(4, 2);
                    convDateTime = convDateTime.Replace(substr, "");
                    final = substr + " " + convDateTime;
                    //DT.Rows[m]["StartDate"] = final;

                    convDateTime4 = DT.Rows[m]["CreateDate"].ToString();
                    convDateTime4 = convDateTime4.Substring(0, 7) + convDateTime4.Substring(9);
                    DT.Rows[m]["CreateDate"] = convDateTime4;

                    convDateTime2 = DT.Rows[m]["EndDate"].ToString();
                    convDateTime2 = convDateTime2.Substring(0, 7) + convDateTime2.Substring(9);
                    substr2 = convDateTime2.Substring(4, 2);
                    convDateTime2 = convDateTime.Replace(substr2, "");
                    final2 = substr2 + " " + convDateTime2;
                    //DT.Rows[m]["EndDate"] = final2;

                    //convDateTime3 = DT.Rows[m]["attenddate"].ToString();
                    //convDateTime3 = convDateTime3.Substring(0, 7) + convDateTime3.Substring(9);
                    //substr3 = convDateTime3.Substring(4, 2);
                    //convDateTime3 = convDateTime3.Replace(substr3, "");
                    //final3 = substr3 + " " + convDateTime3;
                    //DT.Rows[m]["attenddate"] = final3;
                }
                GridReminder.DataSource = DT.DefaultView;
                GridReminder.DataBind();

            }
            else if (param == "Filter2")
            {
                Session["mode"] = param;
                //DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else  Convert(Varchar,rem_attenddate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_attenddate, 100), 13, 7)  end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company   ", "  ((rem_createUser =" + HttpContext.Current.Session["userid"] + ") OR (rem_targetUser =" + HttpContext.Current.Session["userid"] + ")) and rem_createDate between '" + txtStart2.Value.ToString() + "' and '" + txtEnd2.Value.ToString() + "' order by rem_createDate,rem_flag desc ");
                if (HttpContext.Current.Session["userid"].ToString() != dtonlyid.Rows[0]["user_id"].ToString())
                {
                    DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else  Convert(Varchar,rem_attenddate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_attenddate, 100), 13, 7)  end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company   ", " ((rem_createUser =" + HttpContext.Current.Session["userid"] + ") OR (rem_targetUser =" + HttpContext.Current.Session["userid"] + ") or (rem_inchargetargetuser =" + HttpContext.Current.Session["userid"] + ")) and rem_createDate between '" + txtStart2.Value.ToString() + "' and '" + txtEnd2.Value.ToString() + "' order by rem_createDate,rem_flag desc ");
                }
                else
                {
                    DT = oDBEngine.GetDataTable(" tbl_trans_reminder ", " case when rem_categoryid is null then '' else (select ltrim(rtrim(Remindercategory_shortname)) from Master_Remindercategory where Remindercategory_id=rem_categoryid ) end as shortname,rem_id AS Rid, CASE rem_sourceid WHEN 0 THEN rem_createUser ELSE 0 END AS CreaterId, CASE rem_createUser WHEN 0 THEN 'System' ELSE (SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_createUser)  END AS CreateBy, Convert(varchar(11), rem_createDate,  106) AS CreateDate, rem_targetUser AS TargetId,rem_inchargetargetuser as inchargeid,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_inchargetargetuser) AS incharge,(SELECT tbl_master_user.user_name  FROM tbl_master_user WHERE user_id = rem_targetUser) AS Target, Convert(Varchar,rem_startDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_startDate, 100), 13, 7) as StartDate, Convert(Varchar,rem_endDate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_endDate, 100), 13, 7) as EndDate, rem_reminderContent AS Content, rem_displayTricker AS Tickier, CASE rem_actiontaken WHEN 0 THEN 'Pending' WHEN 2 THEN 'Open' ELSE 'Closed' END AS Status,ISNULL(rem_replycontent,'') as replycontent,CASE rem_actiontaken WHEN 0 THEN ' ' else  Convert(Varchar,rem_attenddate,106)+' '+SUBSTRING(CONVERT(varchar(24), rem_attenddate, 100), 13, 7)  end as attenddate,case  when rem_priority=1 then 'L' when rem_priority=2 then 'N' when rem_priority=3 then 'H' When rem_priority=4 then 'U' end as priority,isnull(rem_flag,'R') as flag,(select ltrim(rtrim(cmp_onroleshortname)) from tbl_master_company where cmp_id=rem_cmpid) as company   ", " ((rem_createUser in (" + _userId1 + ")) OR (rem_targetUser in (" + _userId1 + ")) or (rem_inchargetargetuser in (" + _userId1 + "))) and rem_createDate between '" + txtStart2.Value.ToString() + "' and '" + txtEnd2.Value.ToString() + "' order by rem_createDate,rem_flag desc ");
                }
                for (int m = 0; m < DT.Rows.Count; m++)
                {
                    final = "";
                    substr = "";
                    final2 = "";
                    substr2 = "";
                    final3 = "";
                    substr3 = "";
                    convDateTime = DT.Rows[m]["StartDate"].ToString();
                    convDateTime = convDateTime.Substring(0, 7) + convDateTime.Substring(9);
                    substr = convDateTime.Substring(4, 2);
                    convDateTime = convDateTime.Replace(substr, "");
                    final = substr + " " + convDateTime;
                    //DT.Rows[m]["StartDate"] = final;

                    convDateTime4 = DT.Rows[m]["CreateDate"].ToString();
                    convDateTime4 = convDateTime4.Substring(0, 7) + convDateTime4.Substring(9);
                    DT.Rows[m]["CreateDate"] = convDateTime4;

                    convDateTime2 = DT.Rows[m]["EndDate"].ToString();
                    convDateTime2 = convDateTime2.Substring(0, 7) + convDateTime2.Substring(9);
                    substr2 = convDateTime2.Substring(4, 2);
                    convDateTime2 = convDateTime.Replace(substr2, "");
                    final2 = substr2 + " " + convDateTime2;
                    //DT.Rows[m]["EndDate"] = final2;

                    //convDateTime3 = DT.Rows[m]["attenddate"].ToString();
                    //convDateTime3 = convDateTime3.Substring(0, 7) + convDateTime3.Substring(9);
                    //substr3 = convDateTime3.Substring(4, 2);
                    //convDateTime3 = convDateTime3.Replace(substr3, "");
                    //final3 = substr3 + " " + convDateTime3;
                    //DT.Rows[m]["attenddate"] = final3;
                }
                GridReminder.DataSource = DT.DefaultView;
                GridReminder.DataBind();

            }
            else if (e.Parameters == "s")
            {
                Session["mode"] = param;
                GridReminder.Settings.ShowFilterRow = true;
                FillGrid(Session["mode"].ToString());
            }
            else if (e.Parameters == "subject")
            {
                //  Response.Redirect("subjectselection_reminder.aspx", false);
                //ASPxWebControl.RedirectOnCallback("subjectselection_reminder.aspx");

            }

        }

    }
}