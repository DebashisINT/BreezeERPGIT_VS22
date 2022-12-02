using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;

namespace ERP.OMS.Management
{
    public partial class management_frm_ReallocatePhonecall : System.Web.UI.Page
    {
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        DataTable dt = new DataTable();
        clsDropDownList oclsDropDownList = new clsDropDownList();
        BusinessLogicLayer.Converter ObjConvert = new BusinessLogicLayer.Converter();
        protected void Page_Load(object sender, EventArgs e)
        {
            CalenderControl();
            if (!IsPostBack)
            {
                FillDropDown();
            }
        }
        public void CalenderControl()
        {
            string today = ObjConvert.DateConverter(oDBEngine.GetDate().ToString(), "dd/mm/yyyy hh:mm");
            ImgStart.Attributes.Add("OnClick", "displayCalendar(TxtStartDate,'dd/mm/yyyy hh:ii',this,true,null,'0','0')");
            TxtStartDate.Attributes.Add("onfocus", "displayCalendar(TxtStartDate ,'dd/mm/yyyy hh:ii',this,true,null,'0','0')");
            ImgEnd.Attributes.Add("OnClick", "displayCalendar(TxtEndDate,'dd/mm/yyyy hh:ii',this,true,null,'0','0')");
            TxtEndDate.Attributes.Add("onfocus", "displayCalendar(TxtEndDate ,'dd/mm/yyyy hh:ii',this,true,null,'0','0')");
            TxtStartDate.Attributes.Add("readonly", "true");
            TxtEndDate.Attributes.Add("readonly", "true");
        }
        public void FillDropDown()
        {
            drpPriority.SelectedIndex = 0;
            string temp = "";

            string userId = oDBEngine.getChildUser(Session["userid"].ToString(), temp);
            string[,] ActType = oDBEngine.GetFieldValue("tbl_master_activitytype", "aty_id,aty_activityType", null, 2, "aty_activityType");
            if (ActType[0, 0] != "n")
            {
                oclsDropDownList.AddDataToDropDownList(ActType, drpActType);
            }
            string[,] UserWork = oDBEngine.GetFieldValue("tbl_master_user", "user_id,user_name", " user_id in(" + userId + ")", 2, "user_name");
            if (UserWork[0, 0] != "n")
            {
                oclsDropDownList.AddDataToDropDownList(UserWork, drpUserWork);
            }
            switch (Request.QueryString["type"].ToString())
            {
                case "SalesVisit":
                    drpActType.SelectedValue = Convert.ToInt32(4).ToString();
                    dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN   tbl_master_user ON tbl_trans_Activies.act_assignedBy = tbl_master_user.user_id INNER JOIN   tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId INNER JOIN    tbl_master_lead ON tbl_trans_salesVisit.slv_leadcotactId = tbl_master_lead.cnt_internalId", "tbl_master_lead.cnt_internalId AS LeadId, ISNULL(tbl_master_lead.cnt_firstName, '') + ISNULL(tbl_master_lead.cnt_middleName, '')   + ISNULL(tbl_master_lead.cnt_lastName, '') AS Name, tbl_trans_Activies.act_id AS ActId, tbl_trans_salesVisit.slv_id AS PhoneCallid,   tbl_trans_Activies.act_scheduledDate AS SchDate, tbl_master_user.user_name AS Allotedby", "tbl_trans_salesvisit.slv_lastdatevisit IS NULL And tbl_trans_Activies.act_id ='" + Request.QueryString["id"].ToString() + "' order by tbl_trans_Activies.act_id");
                    if (dt.Rows.Count > 0)
                    {
                        grdCalls.DataSource = dt.DefaultView;
                        grdCalls.DataBind();
                    }
                    break;
                case "PhoneCall":
                    drpActType.SelectedValue = Convert.ToInt32(1).ToString();
                    dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN    tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId INNER JOIN    tbl_master_lead ON tbl_trans_phonecall.phc_leadcotactId = tbl_master_lead.cnt_internalId INNER JOIN   tbl_master_user ON tbl_trans_Activies.act_assignedBy = tbl_master_user.user_id", "tbl_master_lead.cnt_internalId AS LeadId, ISNULL(tbl_master_lead.cnt_firstName, '') + ISNULL(tbl_master_lead.cnt_middleName, '')  + ISNULL(tbl_master_lead.cnt_lastName, '') AS Name, tbl_trans_Activies.act_id AS ActId, tbl_trans_phonecall.phc_id AS PhoneCallid,  tbl_trans_Activies.act_scheduledDate AS SchDate, tbl_master_user.user_name AS Allotedby", " tbl_trans_phonecall.phc_callDate IS NULL And tbl_trans_Activies.act_assignedTo =" + Session["userid"].ToString() + " And tbl_trans_Activies.act_id ='" + Request.QueryString["id"].ToString() + "' order by tbl_trans_Activies.act_id");
                    if (dt.Rows.Count > 0)
                    {
                        grdCalls.DataSource = dt.DefaultView;
                        grdCalls.DataBind();
                    }
                    break;
                case "RecruitmentEmployee":
                    btnCondition.Visible = false;
                    drpActType.SelectedValue = Convert.ToInt32(7).ToString();
                    break;
                case "RecruitmentAgent":
                    btnCondition.Visible = false;
                    drpActType.SelectedValue = Convert.ToInt32(8).ToString();
                    break;
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string sSql = "";
            string actNo = "";
            string id = "";
            string dDate1 = ObjConvert.DateConverter(TxtStartDate.Text, "mm/dd/yyyy hh:mm");
            string sStartDate = Convert.ToDateTime(dDate1).ToShortDateString();
            string sStartTime = Convert.ToDateTime(dDate1).ToShortTimeString();
            string dDate2 = ObjConvert.DateConverter(TxtEndDate.Text, "mm/dd/yyyy hh:mm");
            string sEndDate = Convert.ToDateTime(dDate2).ToShortDateString();
            string sEndTime = Convert.ToDateTime(dDate2).ToShortTimeString();
            //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
            DBEngine oDBEngine = new DBEngine();
            switch (Request.QueryString["type"].ToString())
            {
                case "SalesVisit":
                    string[,] actNo1 = oDBEngine.GetFieldValue("tbl_trans_Activies", "max(act_activityNo)", " act_activityNo like '%SW%'", 1);
                    if (actNo1[0, 0] != "n")
                    {
                        actNo = actNo1[0, 0];
                    }
                    string count = "0";//session["count"].tostring() this is initialize from manager folder
                    oDBEngine.InsurtFieldValue("tbl_trans_Activies", "act_branchId,act_activityType,act_description,act_assignedBy,act_assignedTo,act_createDate,act_contactlead,act_scheduledDate,act_expectedDate,act_activityno,act_priority,act_scheduledTime,act_expectedTime,act_previousActno", "'" + HttpContext.Current.Session["userbranchID"].ToString() + "','" + drpActType.SelectedValue + "','" + txtDesc.Text + "','" + Session["userid"].ToString() + "','" + drpUserWork.SelectedValue + "','" + oDBEngine.GetDate().ToShortDateString() + "','" + count + "','" + sStartDate + "','" + sEndDate + "','" + actNo + "','" + drpPriority.SelectedValue + "','" + sStartTime + "','" + sEndTime + "','" + Request.QueryString["id"].ToString() + "'");
                    string[,] lastid = oDBEngine.GetFieldValue("tbl_trans_Activies", "max(act_id)", null, 1);
                    if (lastid[0, 0] != "n")
                    {
                        id = lastid[0, 0];
                    }
                    if (txtNoCont.Text != "" && Convert.ToInt32(txtNoCont.Text) > 0)
                    {
                        dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN   tbl_master_user ON tbl_trans_Activies.act_assignedBy = tbl_master_user.user_id INNER JOIN   tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId INNER JOIN    tbl_master_lead ON tbl_trans_salesVisit.slv_leadcotactId = tbl_master_lead.cnt_internalId", "tbl_master_lead.cnt_internalId AS LeadId, ISNULL(tbl_master_lead.cnt_firstName, '') + ISNULL(tbl_master_lead.cnt_middleName, '')   + ISNULL(tbl_master_lead.cnt_lastName, '') AS Name, tbl_trans_Activies.act_id AS ActId, tbl_trans_salesVisit.slv_id AS PhoneCallid,   tbl_trans_Activies.act_scheduledDate AS SchDate, tbl_master_user.user_name AS Allotedby", " tbl_trans_Activies.act_id ='" + Request.QueryString["id"].ToString() + "' order by tbl_trans_Activies.act_id");
                        int countt = 0;
                        if (dt.Rows.Count > Convert.ToInt32(txtNoCont.Text))
                        {
                            countt = Convert.ToInt32(txtNoCont.Text);
                        }
                        else
                        {
                            countt = dt.Rows.Count;
                        }
                        for (int i = 0; i <= countt - 1; i++)
                        {
                            oDBEngine.SetFieldValue("tbl_trans_salesVisit", "slv_activityId='" + id + "'", "slv_id='" + dt.Rows[i]["Phonecallid"].ToString() + "'");
                            oDBEngine.SetFieldValue("tbl_trans_Activies", "act_previousActno='" + dt.Rows[i]["Actid"].ToString() + "'", " act_id='" + id + "'");
                            string updatephoneid = "";
                            DataTable dt1 = oDBEngine.GetDataTable("tbl_trans_salesVisit", "isnull(slv_PreviousActivityId,'') as Previousid", " slv_id='" + dt.Rows[i]["Phonecallid"].ToString() + "'");
                            if (dt1.Rows.Count > 0)
                            {
                                updatephoneid = dt1.Rows[0][0].ToString();
                                if (updatephoneid != "")
                                {
                                    oDBEngine.SetFieldValue("tbl_trans_phonecall", "phc_NextActivityId='" + actNo + "'", " phc_id='" + updatephoneid + "'");
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i <= grdCalls.Rows.Count - 1; i++)
                        {
                            GridViewRow row = grdCalls.Rows[i];
                            CheckBox chkData = (CheckBox)row.FindControl("chk1");
                            if (chkData.Checked == true)
                            {
                                Label lblLead = (Label)row.FindControl("lblLeadid");
                                string leadid = lblLead.Text;
                                Label lblPhone = (Label)row.FindControl("lblPhoneCall");
                                string Phoneid = lblPhone.Text;
                                Label lblActid = (Label)row.FindControl("lblActid");
                                string Actid = lblActid.Text;
                                oDBEngine.SetFieldValue("tbl_trans_salesVisit", "slv_activityId='" + id + "'", " slv_id='" + Phoneid + "'");
                                oDBEngine.SetFieldValue("tbl_trans_Activies", "act_previousActno='" + Actid + "'", " act_id='" + id + "'");
                                string updatephoneid = "";
                                DataTable dt1 = oDBEngine.GetDataTable("tbl_trans_salesVisit", "isnull(slv_PreviousActivityId,'') as Previousid", "slv_id='" + Phoneid + "'");
                                if (dt1.Rows.Count > 0)
                                {
                                    updatephoneid = dt1.Rows[0][0].ToString();
                                    if (updatephoneid != "")
                                    {
                                        oDBEngine.SetFieldValue("tbl_trans_phonecall", "phc_NextActivityId='" + actNo + "'", " phc_id='" + updatephoneid + "'");
                                    }
                                }
                            }
                        }
                    }
                    break;
                case "PhoneCall":
                    string[,] actNo2 = oDBEngine.GetFieldValue("tbl_trans_Activies", "max(act_activityNo)", " act_activityNo like '%PC%'", 1);
                    if (actNo2[0, 0] != "n")
                    {
                        actNo = actNo2[0, 0];
                    }
                    string count1 = "0";//session["count"].tostring() this is initialize from manager folder
                    oDBEngine.InsurtFieldValue("tbl_trans_Activies", "act_branchId,act_activityType,act_description,act_assignedBy,act_assignedTo,act_createDate,act_contactlead,act_scheduledDate,act_expectedDate,act_activityno,act_priority,act_scheduledTime,act_expectedTime,act_previousActno", "'" + HttpContext.Current.Session["userbranchID"].ToString() + "','" + drpActType.SelectedValue + "','" + txtDesc.Text + "','" + Session["userid"].ToString() + "','" + drpUserWork.SelectedValue + "','" + oDBEngine.GetDate().ToShortDateString() + "','" + count1 + "','" + sStartDate + "','" + sEndDate + "','" + actNo + "','" + drpPriority.SelectedValue + "','" + sStartTime + "','" + sEndTime + "','" + Request.QueryString["id"].ToString() + "'");
                    string[,] lastid1 = oDBEngine.GetFieldValue("tbl_trans_Activies", "max(act_id)", null, 1);
                    if (lastid1[0, 0] != "n")
                    {
                        id = lastid1[0, 0];
                    }
                    if (txtNoCont.Text != "" && Convert.ToInt32(txtNoCont.Text) > 0)
                    {
                        dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN    tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId INNER JOIN    tbl_master_lead ON tbl_trans_phonecall.phc_leadcotactId = tbl_master_lead.cnt_internalId INNER JOIN   tbl_master_user ON tbl_trans_Activies.act_assignedBy = tbl_master_user.user_id", "tbl_master_lead.cnt_internalId AS LeadId, ISNULL(tbl_master_lead.cnt_firstName, '') + ISNULL(tbl_master_lead.cnt_middleName, '')  + ISNULL(tbl_master_lead.cnt_lastName, '') AS Name, tbl_trans_Activies.act_id AS ActId, tbl_trans_phonecall.phc_id AS PhoneCallid,  tbl_trans_Activies.act_scheduledDate AS SchDate, tbl_master_user.user_name AS Allotedby", " tbl_trans_phonecall.phc_callDate IS NULL And tbl_trans_Activies.act_assignedTo =" + Session["userid"].ToString() + " And tbl_trans_Activies.act_id ='" + Request.QueryString["id"].ToString() + "' order by tbl_trans_Activies.act_id");
                        int countt = 0;
                        if (dt.Rows.Count > Convert.ToInt32(txtNoCont.Text))
                        {
                            countt = Convert.ToInt32(txtNoCont.Text);
                        }
                        else
                        {
                            countt = dt.Rows.Count;
                        }
                        for (int i = 0; i <= countt - 1; i++)
                        {
                            oDBEngine.SetFieldValue("tbl_trans_phonecall", "phc_activityId='" + id + "'", " phc_id='" + dt.Rows[i]["Phonecallid"].ToString() + "'");
                            if (i == dt.Rows.Count - 1) break;
                        }
                    }
                    else
                    {
                        for (int i = 0; i <= grdCalls.Rows.Count - 1; i++)
                        {
                            GridViewRow row = grdCalls.Rows[i];
                            CheckBox chkData = (CheckBox)row.FindControl("chk1");
                            if (chkData.Checked == true)
                            {
                                Label lblLead = (Label)row.FindControl("lblLeadid");
                                string leadid = lblLead.Text;
                                Label lblPhone = (Label)row.FindControl("lblPhoneCall");
                                string Phoneid = lblPhone.Text;
                                Label lblActid = (Label)row.FindControl("lblActid");
                                string Actid = lblActid.Text;
                                oDBEngine.SetFieldValue("tbl_trans_phonecall", "phc_activityId='" + id + "'", " phc_id='" + Phoneid + "'");
                                oDBEngine.SetFieldValue("tbl_trans_Activies", "act_previousActno='" + Actid + "'", "act_id='" + id + "'");
                            }
                        }
                    }
                    break;
                case "RecruitmentEmployee":
                    string[,] actNo3 = oDBEngine.GetFieldValue("tbl_trans_Activies", "max(act_activityNo)", " act_activityNo like '%RE%'", 1);
                    if (actNo3[0, 0] != "n")
                    {
                        actNo = actNo3[0, 0];
                    }
                    string count2 = "0";//session["count"].tostring() this is initialize from manager folder
                    oDBEngine.InsurtFieldValue("tbl_trans_Activies", "act_branchId,act_activityType,act_description,act_assignedBy,act_assignedTo,act_createDate,act_contactlead,act_scheduledDate,act_expectedDate,act_activityno,act_priority,act_scheduledTime,act_expectedTime,act_previousActno", "'" + HttpContext.Current.Session["userbranchID"].ToString() + "','" + drpActType.SelectedValue + "','" + txtDesc.Text + "','" + Session["userid"].ToString() + "','" + drpUserWork.SelectedValue + "','" + oDBEngine.GetDate().ToShortDateString() + "','" + count2 + "','" + sStartDate + "','" + sEndDate + "','" + actNo + "','" + drpPriority.SelectedValue + "','" + sStartTime + "','" + sEndTime + "','" + Request.QueryString["id"].ToString() + "'");
                    string[,] lastid2 = oDBEngine.GetFieldValue("tbl_trans_Activies", "max(act_id)", null, 1);
                    if (lastid2[0, 0] != "n")
                    {
                        id = lastid2[0, 0];
                    }
                    oDBEngine.SetFieldValue("tbl_trans_Recruitment", "rd_ActivityId='" + id + "'", " rd_ActivityId='" + Request.QueryString["id"].ToString() + "'");
                    break;
                case "RecruitmentAgent":
                    string[,] actNo4 = oDBEngine.GetFieldValue("tbl_trans_Activies", "max(act_activityNo)", " act_activityNo like '%RR%'", 1);
                    if (actNo4[0, 0] != "n")
                    {
                        actNo = actNo4[0, 0];
                    }
                    string count3 = "0";//session["count"].tostring() this is initialize from manager folder
                    oDBEngine.InsurtFieldValue("tbl_trans_Activies", "act_branchId,act_activityType,act_description,act_assignedBy,act_assignedTo,act_createDate,act_contactlead,act_scheduledDate,act_expectedDate,act_activityno,act_priority,act_scheduledTime,act_expectedTime,act_previousActno", "'" + HttpContext.Current.Session["userbranchID"].ToString() + "','" + drpActType.SelectedValue + "','" + txtDesc.Text + "','" + Session["userid"].ToString() + "','" + drpUserWork.SelectedValue + "','" + oDBEngine.GetDate().ToShortDateString() + "','" + count3 + "','" + sStartDate + "','" + sEndDate + "','" + actNo + "','" + drpPriority.SelectedValue + "','" + sStartTime + "','" + sEndTime + "','" + Request.QueryString["id"].ToString() + "'");
                    string[,] lastid3 = oDBEngine.GetFieldValue("tbl_trans_Activies", "max(act_id)", null, 1);
                    if (lastid3[0, 0] != "n")
                    {
                        id = lastid3[0, 0];
                    }
                    oDBEngine.SetFieldValue("tbl_trans_ReferalAgentRecuruitment", "rar_Activityid='" + id + "'", " rar_Activityid='" + Request.QueryString["id"].ToString() + "'");
                    break;
            }
            string popupScript = "";
            popupScript = "<script language='javascript'>" + "window.opener.location.href=window.opener.location.href;window.close();</script>";
            ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
        }
        protected void grdCalls_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
            DBEngine oDBEngine = new DBEngine();
            grdCalls.PageIndex = e.NewPageIndex;
            if (Request.QueryString["type"].ToString() == "PhoneCall")
            {
                dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN    tbl_trans_phonecall ON tbl_trans_Activies.act_id = tbl_trans_phonecall.phc_activityId INNER JOIN    tbl_master_lead ON tbl_trans_phonecall.phc_leadcotactId = tbl_master_lead.cnt_internalId INNER JOIN   tbl_master_user ON tbl_trans_Activies.act_assignedBy = tbl_master_user.user_id", "tbl_master_lead.cnt_internalId AS LeadId, ISNULL(tbl_master_lead.cnt_firstName, '') + ISNULL(tbl_master_lead.cnt_middleName, '')  + ISNULL(tbl_master_lead.cnt_lastName, '') AS Name, tbl_trans_Activies.act_id AS ActId, tbl_trans_phonecall.phc_id AS PhoneCallid,  tbl_trans_Activies.act_scheduledDate AS SchDate, tbl_master_user.user_name AS Allotedby", " tbl_trans_phonecall.phc_callDate IS NULL And tbl_trans_Activies.act_assignedTo =" + Session["userid"].ToString() + " And tbl_trans_Activies.act_id ='" + Request.QueryString["id"].ToString() + "' order by tbl_trans_Activies.act_id");
            }
            else
            {
                dt = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN   tbl_master_user ON tbl_trans_Activies.act_assignedBy = tbl_master_user.user_id INNER JOIN   tbl_trans_salesVisit ON tbl_trans_Activies.act_id = tbl_trans_salesVisit.slv_activityId INNER JOIN    tbl_master_lead ON tbl_trans_salesVisit.slv_leadcotactId = tbl_master_lead.cnt_internalId", "tbl_master_lead.cnt_internalId AS LeadId, ISNULL(tbl_master_lead.cnt_firstName, '') + ISNULL(tbl_master_lead.cnt_middleName, '')   + ISNULL(tbl_master_lead.cnt_lastName, '') AS Name, tbl_trans_Activies.act_id AS ActId, tbl_trans_salesVisit.slv_id AS PhoneCallid,   tbl_trans_Activies.act_scheduledDate AS SchDate, tbl_master_user.user_name AS Allotedby", " tbl_trans_salesvisit.slv_lastdatevisit IS NULL And tbl_trans_Activies.act_id ='" + Request.QueryString["id"].ToString() + "' order by tbl_trans_Activies.act_id");
            }
            if (dt.Rows.Count > 0)
            {
                grdCalls.DataSource = dt.DefaultView;
                grdCalls.DataBind();
            }
        }
        protected void btnCondition_Click(object sender, EventArgs e)
        {
            pnlCondition.Visible = true;
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            string popupScript = "";
            popupScript = "<script language='javascript'>" + "window.close();</script>";
            ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
        }
    }
}