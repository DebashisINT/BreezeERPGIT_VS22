using ClsDropDownlistNameSpace;
using System;
using System.Configuration;
using System.Web;

namespace ERP.OMS.Management
{
    public partial class management_frm_UpdateInterview : System.Web.UI.Page
    {
        int ID;
        int ID1;
        //   Converter ObjConverter = new Converter();
        BusinessLogicLayer.Converter OConvert = new BusinessLogicLayer.Converter();
        clsDropDownList oclsDropDownList = new clsDropDownList();
        protected void Page_Load(object sender, EventArgs e)
        {
            drpInterviewDate.EditFormatString = OConvert.GetDateFormat("DateTime");
            drpInterviewDate.Attributes.Add("readonly", "true");
            ID = int.Parse(Request.QueryString["id"].ToString());
            if (!IsPostBack)
            {
                ID = int.Parse(Request.QueryString["id"].ToString());
                string mode = Request.QueryString["mode"].ToString();
                if (mode == "edit")
                {
                    Session["mode"] = mode;
                    EditCandidate();
                }
                if (mode == "new")
                {
                    Session["mode"] = mode;
                    NewCandidate();
                }

            }
            if (Request.QueryString["id1"] != null)
            {
                ID1 = int.Parse(Request.QueryString["id1"].ToString());
            }
        }

        private void NewCandidate()
        {
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            string[,] Data = oDBEngine.GetFieldValue("tbl_master_user ", " user_id as Id, user_name+' ['+user_loginId+']' AS Name", null, 2, "user_name");
            oclsDropDownList.AddDataToDropDownList(Data, drpInterviewer, 0);
            Data = oDBEngine.GetFieldValue("tbl_master_branch ", "branch_id as id,branch_description as Desc1", null, 2, "branch_description");
            oclsDropDownList.AddDataToDropDownList(Data, drpIntPlace, 0);

            //drpInterviewDate.Text = ObjConverter.DateConverter_d_m_y(oDBEngine.GetDate().ToString());

            ID = int.Parse(Request.QueryString["id"].ToString());
            if (Request.QueryString["id1"] != null)
            {
                Data = oDBEngine.GetFieldValue("tbl_trans_ReferalAgentDetail", "rdd_CandidateName,rdd_activityid", " rdd_id=" + ID, 2);
            }
            else
            {
                Data = oDBEngine.GetFieldValue("tbl_trans_RecruitmentDetail", "rde_name,rde_activityid", " rde_id=" + ID, 2);
            }
            txtName.Text = Data[0, 0].ToString();
            txtActivity.Text = Data[0, 1].ToString();
            txtRecruit.Text = ID.ToString();
        }

        private void EditCandidate()
        {
            ID = int.Parse(Request.QueryString["id"].ToString());
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            if (Request.QueryString["id1"] != null)
            {
                string[,] data = oDBEngine.GetFieldValue("tbl_trans_ReferalAgentDetail A,tbl_trans_Interview  B", "A.rdd_CandidateName as rde_name,A.rdd_activityid as rde_activityid,B.int_InterviewDataTime as int_InterviewDataTime,B.int_InterviewPlace as int_InterviewPlace,B.int_InterviewerName as int_InterviewerName", " rdd_id=" + ID + " and B.int_internalid=A.rdd_id", 5);
                if (data[0, 0] != "n")
                {
                    string[,] Data = oDBEngine.GetFieldValue("tbl_master_user ", "user_id as Id, user_name+' ['+user_loginId+']' AS Name ", null, 2, "user_name");
                    oclsDropDownList.AddDataToDropDownList(Data, drpInterviewer, int.Parse(data[0, 4].ToString()));
                    Data = oDBEngine.GetFieldValue("tbl_master_branch ", "branch_id as id,branch_description as Desc1", null, 2, "branch_description");
                    oclsDropDownList.AddDataToDropDownList(Data, drpIntPlace, int.Parse(data[0, 3].ToString()));

                    drpInterviewDate.Value = Convert.ToDateTime(data[0, 2].ToString());
                    txtName.Text = data[0, 0].ToString();
                    txtActivity.Text = data[0, 1].ToString();
                    txtRecruit.Text = ID.ToString();
                }
                else
                {
                    lblmessage.Text = "First Create Interview For This Candidate. You can create Interview from here as wel";
                    Session["mode"] = "new";
                    NewCandidate();

                }
            }
            else
            {
                string[,] checkVal = oDBEngine.GetFieldValue(" tbl_trans_RecruitmentDetail A,tbl_trans_Interview  B ", " B.int_NextInterviewData ", " rde_id=" + ID + " and B.int_internalid=A.rde_id", 1, " int_id desc ");

                string[,] data;
                if (checkVal[0, 0] != "n" && checkVal[0, 0] != "" && checkVal[0, 0] != null)
                {
                    data = oDBEngine.GetFieldValue("tbl_trans_RecruitmentDetail A,tbl_trans_Interview  B", "A.rde_name as rde_name,A.rde_activityid as rde_activityid,B.int_NextInterviewData as int_InterviewDataTime,B.int_NextInterviewPlace as int_InterviewPlace,B.int_NextInterviewer as int_InterviewerName", " rde_id=" + ID + " and B.int_internalid=A.rde_id", 5);
                }
                else
                    data = oDBEngine.GetFieldValue("tbl_trans_RecruitmentDetail A,tbl_trans_Interview  B", "A.rde_name as rde_name,A.rde_activityid as rde_activityid,B.int_InterviewDataTime as int_InterviewDataTime,B.int_InterviewPlace as int_InterviewPlace,B.int_InterviewerName as int_InterviewerName", " rde_id=" + ID + " and B.int_internalid=A.rde_id", 5);
                if (data[0, 0] != "n")
                {
                    string[,] Data = oDBEngine.GetFieldValue("tbl_master_user ", "user_id as Id, user_name+' ['+user_loginId+']' AS Name", null, 2, "user_name");
                    oclsDropDownList.AddDataToDropDownList(Data, drpInterviewer, int.Parse(data[0, 4].ToString()));
                    Data = oDBEngine.GetFieldValue("tbl_master_branch ", "branch_id as id,branch_description as Desc1", null, 2, "branch_description");
                    oclsDropDownList.AddDataToDropDownList(Data, drpIntPlace, int.Parse(data[0, 3].ToString()));

                    drpInterviewDate.Value = Convert.ToDateTime(data[0, 2].ToString());
                    txtName.Text = data[0, 0].ToString();
                    txtActivity.Text = data[0, 1].ToString();
                    txtRecruit.Text = ID.ToString();
                    //__This will help to modify message & reminders in update OR reshedule interview!
                    hdDateTime.Text = data[0, 2].ToString();
                    hdInterviewer.Text = data[0, 4].ToString();
                    hdPlace.Text = data[0, 3].ToString();
                }
                else
                {
                    lblmessage.Text = "First Create Interview For This Candidate. You can create Interview from here as wel";
                    Session["mode"] = "new";
                    NewCandidate();

                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Session["mode"].ToString() == "new")
            {
                string fields = "int_ActivityId, int_InternalId, int_InterviewDataTime, int_InterviewPlace, int_InterviewerName";
                string values = txtActivity.Text + ",'" + txtRecruit.Text + "','" + drpInterviewDate.Value.ToString() + "'," + drpIntPlace.SelectedItem.Value + "," + drpInterviewer.SelectedItem.Value;

                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                int noofRowEffected = oDBEngine.InsurtFieldValue("tbl_trans_Interview", fields, values);
                if (noofRowEffected != 0)
                {
                    string[,] data = oDBEngine.GetFieldValue(" tbl_trans_Interview ", " top 1 int_id ", " int_activityid=" + txtActivity.Text + " and int_InternalId='" + txtRecruit.Text + "'", 1, " int_id desc ");
                    if (Request.QueryString["id1"] != null)
                    {
                        noofRowEffected = oDBEngine.SetFieldValue("tbl_trans_ReferalAgentDetail", "rdd_Internalid=" + data[0, 0].ToString(), " rdd_Id=" + txtRecruit.Text);
                    }
                    else
                    {
                        noofRowEffected = oDBEngine.SetFieldValue("tbl_trans_RecruitmentDetail", "rde_IntId=" + data[0, 0].ToString(), " rde_Id=" + txtRecruit.Text);
                    }
                    //__ message and reminders!
                    oDBEngine.messageTableUpdate(drpInterviewer.SelectedValue, HttpContext.Current.Session["userid"].ToString(), "Interview", drpInterviewDate.Text, drpInterviewDate.Text, "", "", Request.QueryString["id"].ToString(), "message");
                    string MsgContent = " Hello  " + drpInterviewer.SelectedItem.Text + " an Interview has been schedule to you at " + drpIntPlace.SelectedItem.Text + " on dated  " + drpInterviewDate.Text + " with " + txtName.Text + ".";

                    fields = "[rem_createUser],[rem_createDate],[rem_targetUser],[rem_startDate],[rem_endDate],[rem_reminderContent],[rem_displayTricker],[rem_actionTaken],[rem_sourceid],[CreateDate],[CreateUser]";
                    values = "'0',getdate(),'" + drpInterviewer.SelectedItem.Value + "','" + drpInterviewDate.Value.ToString() + "','" + Convert.ToDateTime(drpInterviewDate.Value.ToString()).AddHours(24).ToString() + "','" + MsgContent + "','1','0','" + Request.QueryString["id"].ToString() + "',getdate(),'" + HttpContext.Current.Session["userid"].ToString() + "'";
                    oDBEngine.InsurtFieldValue(" tbl_trans_reminder ", fields, values);
                }
            }
            if (Session["mode"].ToString() == "edit")
            {
                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                //_check wether data for nextInterview ispresent or not. If yesy then update next interview detaild
                string[,] checkVal = oDBEngine.GetFieldValue(" tbl_trans_Interview ", " int_NextInterviewData ", " int_activityid=" + txtActivity.Text + " and int_InternalId='" + txtRecruit.Text + "'", 1, " int_id desc ");
                string fields = "";
                if (checkVal[0, 0] != "n" && checkVal[0, 0] != "" && checkVal[0, 0] != null)
                {
                    fields = "int_NextInterviewData='" + drpInterviewDate.Value.ToString() + "', int_NextInterviewPlace=" + drpIntPlace.SelectedItem.Value + ", int_NextInterviewer=" + drpInterviewer.SelectedItem.Value;
                }
                else
                    fields = "int_InterviewDataTime='" + drpInterviewDate.Value.ToString() + "', int_InterviewPlace=" + drpIntPlace.SelectedItem.Value + ", int_InterviewerName=" + drpInterviewer.SelectedItem.Value;
                int noofRowEffected = oDBEngine.SetFieldValue("tbl_trans_Interview", fields, " int_activityid=" + txtActivity.Text + " and int_InternalId='" + txtRecruit.Text + "'");
                if (noofRowEffected == 0)
                {
                    lblmessage.Text = "Update Failed!";
                }
                //__ message and reminders!
                string MsgContent = "";
                string Values = "";
                if (drpInterviewer.SelectedValue.Trim() == hdInterviewer.Text.Trim())
                {
                    MsgContent = "Hello " + drpInterviewer.SelectedItem.Text + " ! " + HttpContext.Current.Session["username"].ToString() + " Has Reschedule interview allotted to you, That will Heald on " + drpInterviewDate.Text + " at " + drpIntPlace.SelectedItem.Text + ".  Have a great day !!";
                    fields = " msg_createuser, msg_createdate, msg_targetuser, msg_content, msg_messageread, msg_sourceid, CreateDate, CreateUser";
                    Values = "0,'" + oDBEngine.GetDate() + "'," + drpInterviewer.SelectedValue + ",'" + MsgContent + "',0," + Request.QueryString["id"].ToString() + ",'" + oDBEngine.GetDate() + "'," + HttpContext.Current.Session["userid"];
                    int NoOfRowsEffected = oDBEngine.InsurtFieldValue(" tbl_master_message ", fields, Values);
                    //__ reminder
                    MsgContent = " Hello  " + drpInterviewer.SelectedItem.Text + " an Interview has been schedule to you at " + drpIntPlace.SelectedItem.Text + " on dated  " + drpInterviewDate.Text + " with " + txtName.Text + ".";
                    fields = "[rem_startDate]='" + drpInterviewDate.Value.ToString() + "',[rem_endDate]='" + Convert.ToDateTime(drpInterviewDate.Value.ToString()).AddHours(24).ToString() + "',[rem_reminderContent]='" + MsgContent + "',[rem_displayTricker]='1',[rem_actionTaken]='0',LastModifyDate=getdate(),LastModifyUser=" + HttpContext.Current.Session["userid"].ToString();
                    oDBEngine.SetFieldValue(" tbl_trans_reminder ", fields, " rem_id=(select top 1 A.rem_id from tbl_trans_reminder A where A.rem_sourceid=" + Request.QueryString["id"].ToString() + " and A.rem_targetUser=" + hdInterviewer.Text.Trim() + " order by A.rem_id desc)");
                }
                else
                {
                    oDBEngine.messageTableUpdate(drpInterviewer.SelectedValue, HttpContext.Current.Session["userid"].ToString(), "Interview", drpInterviewDate.Text, drpInterviewDate.Text, "", "", Request.QueryString["id"].ToString(), "message");
                    //__reminder
                    MsgContent = " Hello  " + drpInterviewer.SelectedItem.Text + " an Interview has been schedule to you at " + drpIntPlace.SelectedItem.Text + " on dated  " + drpInterviewDate.Text + " with " + txtName.Text + ".";

                    fields = "[rem_createUser],[rem_createDate],[rem_targetUser],[rem_startDate],[rem_endDate],[rem_reminderContent],[rem_displayTricker],[rem_actionTaken],[rem_sourceid],[CreateDate],[CreateUser]";
                    Values = "'0',getdate(),'" + drpInterviewer.SelectedItem.Value + "','" + drpInterviewDate.Value.ToString() + "','" + Convert.ToDateTime(drpInterviewDate.Value.ToString()).AddHours(24).ToString() + "','" + MsgContent + "','1','0','" + Request.QueryString["id"].ToString() + "',getdate(),'" + HttpContext.Current.Session["userid"].ToString() + "'";
                    oDBEngine.InsurtFieldValue(" tbl_trans_reminder ", fields, Values);
                }

            }
            if (Request.QueryString["id1"] != null)
            {
                Response.Write("<script language='javaScript'>var hd=window.opener.document.getElementById('ctl00$ContentPlaceHolder3$Hd1');hd.value='" + ID1 + "';window.opener.CandidateRefresh(); window.close(); </script>");
            }
            else
            {
                Response.Redirect("frm_UserRecruitmentEmployee_Detail.aspx?id=" + Session["KeyVal"], false);
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["id1"] != null)
            {
                Response.Write("<script language='javaScript'>var hd=window.opener.document.getElementById('ctl00$ContentPlaceHolder3$Hd1');hd.value='" + ID1 + "';window.opener.CandidateRefresh(); window.close(); </script>");
            }
            else
            {
                Response.Redirect("frm_UserRecruitmentEmployee_Detail.aspx?id=" + Session["KeyVal"], false);
            }
        }

    }
}