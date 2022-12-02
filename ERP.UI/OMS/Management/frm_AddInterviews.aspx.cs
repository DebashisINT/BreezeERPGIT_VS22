using System;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;

namespace ERP.OMS.Management
{
    public partial class management_frm_AddInterviews : System.Web.UI.Page
    {
        BusinessLogicLayer.Converter OConvert = new BusinessLogicLayer.Converter();
        public string pageAccess = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);

        clsDropDownList ObjDrop = new clsDropDownList();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            txtHoldUntillDate.EditFormatString = OConvert.GetDateFormat("DateTime");
            
            txtNextInterviewDate.EditFormatString = OConvert.GetDateFormat("DateTime");
            txtJoinDateAggrement.EditFormatString = OConvert.GetDateFormat("DateTime");
            if (!IsPostBack)
            {
                txtName.Text = Request.QueryString["name"].ToString();
                txtInterViewer.Text = Request.QueryString["Interviewer"].ToString();
                int_id.Value = Request.QueryString["InterviewId"].ToString();
                ID.Value = Request.QueryString["id"].ToString();

                string[,] data ={
                {"0","Clearly Outstanding"},
                {"1","Excellent"},
                {"2","Satisfactory"},
                {"3","Some deficiency"},
                {"4","Unsatisfactory"}
            };
                ObjDrop.AddDataToDropDownList(data, drpIntInOrganisation);
                ObjDrop.AddDataToDropDownList(data, drpInterestInPosition);
                ObjDrop.AddDataToDropDownList(data, drpCarretGoals);
                ObjDrop.AddDataToDropDownList(data, drpProblemSolvingSkills);
                ObjDrop.AddDataToDropDownList(data, drpCommunicationSkills);
                ObjDrop.AddDataToDropDownList(data, drpLeadershipSkills);
                ObjDrop.AddDataToDropDownList(data, drpPersonality);
                ObjDrop.AddDataToDropDownList(data, drpJobKnowledge);
                ObjDrop.AddDataToDropDownList(data, drpSalesOrientationApproach);
                data = oDBEngine.GetFieldValue(" tbl_master_user ", " user_id, RTRIM(user_name)+' ['+user_loginId+']' ", null, 2, " user_name");
                ObjDrop.AddDataToDropDownList(data, drpNextInterviewer);
                data = oDBEngine.GetFieldValue(" tbl_master_branch ", " branch_id as id,branch_description as Desc1 ", null, 2);
                ObjDrop.AddDataToDropDownList(data, drpNextInterviewPlace);

                FillData();
                Page.ClientScript.RegisterStartupScript(GetType(), "PageLoad", "<script>Page_Load();</script>");
            }
            drpInterviewOutcome.Attributes.Add("onchange", "ShowDropdown()");
        }

        private void FillData()
        {
            try
            {
                int ActId = 0;
                if (Session["KeyVal"] != null)
                {
                    ActId = int.Parse(Session["KeyVal"].ToString());
                }
                else
                {
                    ActId = int.Parse(Request.QueryString["id1"].ToString());
                }
                //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
                string[,] data = oDBEngine.GetFieldValue("tbl_master_branch INNER JOIN  tbl_trans_Interview ON tbl_master_branch.branch_id = tbl_trans_Interview.int_InterviewPlace INNER JOIN  tbl_master_user ON tbl_trans_Interview.int_InterviewerName = tbl_master_user.user_id inner join tbl_trans_RecruitmentDetail on rde_id=int_InternalId", "convert(varchar(25),int_InterviewDataTime,113) as InterviewDate, int_InterviewPlace as InterviewPlace, int_InterviewerName as Interviewer, isnull(int_CommunicationSkill,0) as commSkills, isnull(int_Personality,0) as Personality,  isnull(int_SalesOrientationApproach,0) as salesOrientationApproach, isnull([int_JobKnowledge&ComputerSkill],0) as JobKnowledge, isnull(int_ProblemSolvingSkill,0) as ProblemSolvingSkills, isnull(int_LeadershipSkill,0) as LeadershipSkills, isnull(int_InterestInOrganisation,0) as InterestinOrg,  isnull(int_InterestinPosition,0) as IntinPosition, isnull(int_ClearityofCareerGoal,0) as CareerGoal, isnull(int_InterviewOutcome,0) as InterviewOutcome, isnull(int_RecomndedCTC,0) as RecomondedCTC, cast(int_HoldUntilDate as datetime) as HoldUntilDate, cast(int_NextInterviewData as datetime) as NextInterviewDate,  isnull(int_NextInterviewPlace,'') as NextInterviewPlace, isnull(int_NextInterviewer,0) as NextInterviewer, isnull(int_Notes,'') as Remarks, tbl_master_user.user_name  AS InterviewerName, tbl_master_branch.branch_description,rde_ApprovedCTC,rde_joinDate", " int_Id=" + int_id.Value + " and int_InternalId=" + ID.Value + " and int_ActivityId=" + ActId, 23);
                if (data[0, 0] != "n")
                {
                    txtInterviewDate.Text = data[0, 0].ToString();
                    txtInterviewPlace.Text = data[0, 19].ToString();
                    txtInterViewer.Text = data[0, 20].ToString();
                    if (data[0, 3].ToString() != "0")
                        drpCommunicationSkills.SelectedIndex = int.Parse(data[0, 3].ToString());
                    if (data[0, 4].ToString() != "0")
                        drpPersonality.SelectedIndex = int.Parse(data[0, 4].ToString());
                    if (data[0, 5].ToString() != "0")
                        drpSalesOrientationApproach.SelectedIndex = int.Parse(data[0, 5].ToString());
                    if (data[0, 6].ToString() != "0")
                        drpJobKnowledge.SelectedIndex = int.Parse(data[0, 6].ToString());
                    if (data[0, 7].ToString() != "0")
                        drpProblemSolvingSkills.SelectedIndex = int.Parse(data[0, 7].ToString());
                    if (data[0, 8].ToString() != "0")
                        drpLeadershipSkills.SelectedIndex = int.Parse(data[0, 8].ToString());
                    if (data[0, 9].ToString() != "0")
                        drpIntInOrganisation.SelectedIndex = int.Parse(data[0, 9].ToString());
                    if (data[0, 10].ToString() != "0")
                        drpInterestInPosition.SelectedIndex = int.Parse(data[0, 10].ToString());
                    if (data[0, 11].ToString() != "0")
                        drpCarretGoals.SelectedIndex = int.Parse(data[0, 11].ToString());
                    if (data[0, 12].ToString() != "0")      //________visibility on Interview Outcomes______//
                    {
                        //switch (data[0, 12].ToString())
                        //{
                        //    case "0":
                        //        ShowHide.Enabled = false;
                        //        pnlSelected.Enabled = false;
                        //        drpInterviewOutcome.SelectedIndex = int.Parse(data[0, 12].ToString());
                        //        break;
                        //    case "1":
                        //        ShowHide.Enabled = false;
                        //        pnlSelected.Enabled = true;
                        //        txtEliminationReason.Enabled = false;

                        //        drpInterviewOutcome.SelectedIndex = int.Parse(data[0, 12].ToString());
                        //        break;
                        //    case "2":
                        //        ShowHide.Enabled = false;
                        //        pnlSelected.Enabled = true;
                        //        txtApprovedCTC.Enabled = false;
                        //        txtJoinDateAggrement.Enabled = false;
                        //        drpInterviewOutcome.SelectedIndex = int.Parse(data[0, 12].ToString());
                        //        break;
                        //    case "3":
                        //        ShowHide.Enabled = true;
                        //        txtNextInterviewDate.Enabled = false;
                        //        pnlSelected.Enabled = false;
                        //        drpInterviewOutcome.SelectedIndex = int.Parse(data[0, 12].ToString());
                        //        break;
                        //    case "4":
                        //        ShowHide.Enabled = true;
                        //        txtHoldUntillDate.Enabled = false;
                        //        pnlSelected.Enabled = false;
                        //        drpInterviewOutcome.SelectedIndex = int.Parse(data[0, 12].ToString());
                        //        break;
                        //    case "5":
                        //        ShowHide.Enabled = true;
                        //        txtHoldUntillDate.Enabled = false;
                        //        pnlSelected.Enabled = false;
                        //        drpInterviewOutcome.SelectedIndex = int.Parse(data[0, 12].ToString());
                        //        break;
                        //    case "6":
                        //        ShowHide.Enabled = false;
                        //        pnlSelected.Enabled = true;
                        //        txtApprovedCTC.Enabled = false;
                        //        txtJoinDateAggrement.Enabled = false;
                        //        drpInterviewOutcome.SelectedIndex = int.Parse(data[0, 12].ToString());
                        //        break;
                        //}
                        drpInterviewOutcome.SelectedIndex = int.Parse(data[0, 12].ToString());
                    }
                    if (data[0, 14] != null && data[0, 14].ToString() != "1900-01-01 00:00:00.000" && data[0, 14].ToString() != "" && data[0, 14].ToString() != "1/1/1900 12:00:00 AM" && data[0, 14].ToString() != "01/01/1900")
                        txtHoldUntillDate.Value = Convert.ToDateTime(data[0, 14].ToString());
                    else
                        txtHoldUntillDate.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                    if (data[0, 15] != null && data[0, 15].ToString() != "1900-01-01 00:00:00.000" && data[0, 15].ToString() != "" && data[0, 15].ToString() != "1/1/1900 12:00:00 AM" && data[0, 15].ToString() != "01/01/1900")
                        txtNextInterviewDate.Value = Convert.ToDateTime(data[0, 15].ToString());
                    else
                        txtNextInterviewDate.Value = oDBEngine.GetDate();
                    if (data[0, 16].ToString() != "")
                        drpNextInterviewPlace.SelectedValue = data[0, 16].ToString();
                    if (data[0, 17].ToString() != "")
                        drpNextInterviewer.SelectedValue = data[0, 17].ToString();
                    if (data[0, 18].ToString() != "")
                        txtRemarks.Text = data[0, 18].ToString();
                    txtApprovedCTC.Text = data[0, 21].ToString();
                    if (data[0, 21].ToString() != null && data[0, 21].ToString() != "")
                        txtJoinDateAggrement.Value = Convert.ToDateTime(data[0, 22].ToString());
                }
            }
            catch
            {
                string jscript = "<script language='javascript'>alert('Please Create Interview');window.close();</script>";
                ClientScript.RegisterStartupScript(GetType(), "JScript", jscript);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int ActId = 0;
            if (Session["KeyVal"] != null)
            {
                ActId = int.Parse(Session["KeyVal"].ToString());
            }
            else
            {
                ActId = int.Parse(Request.QueryString["id1"].ToString());
            }
            BusinessLogicLayer.Converter oConverter = new BusinessLogicLayer.Converter();
            int NoOfRowEffected;
            string[,] nextInt = oDBEngine.GetFieldValue(" tbl_trans_Interview ", " int_NextInterviewData ", " int_Id=" + int_id.Value + " And int_InternalId =" + ID.Value + " And int_ActivityId =" + ActId, 1);
            if (nextInt[0, 0] != "n" && nextInt[0, 0] == "")
            {
                string fieldsWdata = "";
                fieldsWdata = "int_CommunicationSkill=" + drpCommunicationSkills.SelectedItem.Value + ",int_Personality=" + drpPersonality.SelectedItem.Value + ",int_SalesOrientationApproach=" + drpSalesOrientationApproach.SelectedItem.Value + ",[int_JobKnowledge&ComputerSkill]=" + drpJobKnowledge.SelectedItem.Value + ",int_ProblemSolvingSkill=" + drpProblemSolvingSkills.SelectedItem.Value + ",int_LeadershipSkill=" + drpLeadershipSkills.SelectedItem.Value + ",int_InterestInOrganisation=" + drpIntInOrganisation.SelectedItem.Value + ",int_InterestinPosition=" + drpInterestInPosition.SelectedItem.Value + ",int_ClearityofCareerGoal=" + drpCarretGoals.SelectedItem.Value + ",int_InterviewOutcome=" + drpInterviewOutcome.SelectedItem.Value;
                if (drpInterviewOutcome.SelectedItem.Value == "3")
                {
                    fieldsWdata += ",int_HoldUntilDate='" + txtHoldUntillDate.Value.ToString() + "'";
                }
                if (drpInterviewOutcome.SelectedItem.Value == "4" || drpInterviewOutcome.SelectedItem.Value == "5")
                {
                    fieldsWdata += ",int_NextInterviewData='" + txtNextInterviewDate.Value.ToString() + "',int_NextInterviewPlace=" + drpNextInterviewPlace.SelectedItem.Value + ",int_NextInterviewer=" + drpNextInterviewer.SelectedItem.Value;
                }
                else if (drpInterviewOutcome.SelectedItem.Value == "1")
                {
                    fieldsWdata += ",int_RecomndedCTC=" + txtApprovedCTC.Text;
                }
                fieldsWdata += ",int_Notes='" + txtRemarks.Text + "',LastModifyDate=getdate(),LastModifyUser=" + HttpContext.Current.Session["userid"].ToString();
                NoOfRowEffected = oDBEngine.SetFieldValue("tbl_trans_Interview", fieldsWdata, " int_Id=" + int_id.Value + " And int_InternalId =" + ID.Value + " And int_ActivityId =" + ActId);
            }
            else
            {
                string fields = "";
                string data = "";
                fields = "int_ActivityId,int_InternalId, int_CommunicationSkill,int_Personality,int_SalesOrientationApproach,[int_JobKnowledge&ComputerSkill],int_ProblemSolvingSkill,int_LeadershipSkill,int_InterestInOrganisation,int_InterestinPosition,int_ClearityofCareerGoal,int_InterviewOutcome";
                data = ActId + "," + ID.Value + "," + drpCommunicationSkills.SelectedItem.Value + "," + drpPersonality.SelectedItem.Value + "," + drpSalesOrientationApproach.SelectedItem.Value + "," + drpJobKnowledge.SelectedItem.Value + "," + drpProblemSolvingSkills.SelectedItem.Value + "," + drpLeadershipSkills.SelectedItem.Value + "," + drpIntInOrganisation.SelectedItem.Value + "," + drpInterestInPosition.SelectedItem.Value + "," + drpCarretGoals.SelectedItem.Value + "," + drpInterviewOutcome.SelectedItem.Value;
                if (drpInterviewOutcome.SelectedItem.Value == "3")
                {
                    string fieldsWdata = "";
                    fieldsWdata = "int_CommunicationSkill=" + drpCommunicationSkills.SelectedItem.Value + ",int_Personality=" + drpPersonality.SelectedItem.Value + ",int_SalesOrientationApproach=" + drpSalesOrientationApproach.SelectedItem.Value + ",[int_JobKnowledge&ComputerSkill]=" + drpJobKnowledge.SelectedItem.Value + ",int_ProblemSolvingSkill=" + drpProblemSolvingSkills.SelectedItem.Value + ",int_LeadershipSkill=" + drpLeadershipSkills.SelectedItem.Value + ",int_InterestInOrganisation=" + drpIntInOrganisation.SelectedItem.Value + ",int_InterestinPosition=" + drpInterestInPosition.SelectedItem.Value + ",int_ClearityofCareerGoal=" + drpCarretGoals.SelectedItem.Value + ",int_InterviewOutcome=" + drpInterviewOutcome.SelectedItem.Value;
                    if (drpInterviewOutcome.SelectedItem.Value == "3")
                    {
                        fieldsWdata += ",int_HoldUntilDate='" + txtHoldUntillDate.Value.ToString() + "'";
                    }
                    NoOfRowEffected = oDBEngine.SetFieldValue("tbl_trans_Interview", fieldsWdata, " int_Id=" + int_id.Value + " And int_InternalId =" + ID.Value + " And int_ActivityId =" + ActId);
                }
                else
                {
                    if (drpInterviewOutcome.SelectedItem.Value == "4" || drpInterviewOutcome.SelectedItem.Value == "5")
                    {
                        fields += ",int_InterviewDataTime,int_InterviewPlace,int_InterviewerName";
                        data += ",'" + txtNextInterviewDate.Value.ToString() + "'," + drpNextInterviewPlace.SelectedItem.Value + "," + drpNextInterviewer.SelectedItem.Value;
                    }
                    fields += ",int_Notes,CreateDate,CreateUser";
                    data += ",'" + txtRemarks.Text + "',getdate()," + HttpContext.Current.Session["userid"].ToString();
                    NoOfRowEffected = oDBEngine.InsurtFieldValue("tbl_trans_Interview", fields, data);
                    string[,] data1 = oDBEngine.GetFieldValue(" tbl_trans_Interview ", " top 1 int_id ", " int_InternalId =" + ID.Value + " And int_ActivityId =" + ActId, 1, " int_id desc ");
                    NoOfRowEffected = oDBEngine.SetFieldValue("tbl_trans_RecruitmentDetail", "rde_IntId=" + data1[0, 0].ToString(), " rde_Id=" + ID.Value);
                }
            }
            if (NoOfRowEffected != 0)
            {
                if (Request.QueryString["id1"] != null)
                {
                    switch (drpInterviewOutcome.SelectedItem.Value)
                    {
                        case "1":
                            NoOfRowEffected = oDBEngine.SetFieldValue("tbl_trans_ReferalAgentDetail", " rdd_InductionConfirmed=1,rdd_AggrementDate='" + txtJoinDateAggrement.Value.ToString() + "'", " rdd_Id=" + ID.Value);
                            if (NoOfRowEffected != 0)
                            {
                                int vacancies = 0;
                                int selected = 0;
                                string[,] data = oDBEngine.GetFieldValue("tbl_trans_ReferalAgentRecuruitment", "rar_Requirement", " rar_ActivityId=" + ActId, 1);
                                if (data[0, 0].ToString() != "n")
                                {
                                    vacancies = int.Parse(data[0, 0].ToString());
                                }
                                data = oDBEngine.GetFieldValue("tbl_trans_ReferalAgentDetail", "CASE ISNULL(rdd_InductionConfirmed, 5) WHEN 5 THEN '' WHEN 0 THEN '0' WHEN 1 THEN '1' END AS Confirmed", " rdd_ActivityId=" + ActId, 1);
                                if (data[0, 0].ToString() != "n")
                                {
                                    for (int i = 0; i < data.Length; i++)
                                    {
                                        if (data[i, 0].ToString() == "1")
                                            selected += 1;
                                    }
                                    if (vacancies == selected)
                                        NoOfRowEffected = oDBEngine.SetFieldValue("tbl_trans_Activies", " act_actualEndDate='" + oDBEngine.GetDate() + "' ", " act_id=" + ActId);
                                }
                            }
                            break;
                        case "2":
                            NoOfRowEffected = oDBEngine.SetFieldValue("tbl_trans_ReferalAgentDetail", " rdd_InductionConfirmed=0,rdd_ReasonforRejection='" + txtRemarks.Text + "' ", " rdd_Id=" + ID.Value);
                            break;
                        case "4":
                            oDBEngine.messageTableUpdate(drpNextInterviewer.SelectedItem.Value, "0", "Recruitment", txtNextInterviewDate.Value.ToString(), Convert.ToDateTime(txtNextInterviewDate.Value.ToString()).AddDays(1).ToString(), "", "", ActId.ToString(), "activity");
                            string MsgContent = " Hello  " + drpNextInterviewer.SelectedItem.Text + " ! " + HttpContext.Current.Session["username"].ToString() + " has allotted a New [ Interview ] type of activity to you, to be held on " + txtNextInterviewDate.Text + " at " + drpNextInterviewPlace.SelectedItem.Text;

                            string fields = "[rem_createUser],[rem_createDate],[rem_targetUser],[rem_startDate],[rem_endDate],[rem_reminderContent],[rem_displayTricker],[rem_actionTaken],[rem_sourceid],[CreateDate],[CreateUser]";
                            string values = "'0',getdate(),'" + drpNextInterviewer.SelectedValue + "','" + txtNextInterviewDate.Value + "','" + txtNextInterviewDate.Value + "','" + MsgContent + "','1','0','" + ActId + "',getdate(),'" + HttpContext.Current.Session["userid"].ToString() + "'";
                            oDBEngine.InsurtFieldValue(" tbl_trans_reminder ", fields, values);
                            break;
                        case "5":
                            oDBEngine.messageTableUpdate(drpNextInterviewer.SelectedItem.Value, "0", "Recruitment", txtNextInterviewDate.Value.ToString(), Convert.ToDateTime(txtNextInterviewDate.Value.ToString()).AddDays(1).ToString(), "", "", ActId.ToString(), "activity");
                            MsgContent = " Hello  " + drpNextInterviewer.SelectedItem.Text + " ! " + HttpContext.Current.Session["username"].ToString() + " has allotted a New [ Interview ] type of activity to you, to be held on " + txtNextInterviewDate.Text + " at " + drpNextInterviewPlace.SelectedItem.Text;

                            fields = "[rem_createUser],[rem_createDate],[rem_targetUser],[rem_startDate],[rem_endDate],[rem_reminderContent],[rem_displayTricker],[rem_actionTaken],[rem_sourceid],[CreateDate],[CreateUser]";
                            values = "'0',getdate(),'" + drpNextInterviewer.SelectedValue + "','" + txtNextInterviewDate.Value + "','" + txtNextInterviewDate.Value + "','" + MsgContent + "','1','0','" + ActId + "',getdate(),'" + HttpContext.Current.Session["userid"].ToString() + "'";
                            oDBEngine.InsurtFieldValue(" tbl_trans_reminder ", fields, values);
                            break;
                        case "6":
                            NoOfRowEffected = oDBEngine.SetFieldValue("tbl_trans_ReferalAgentDetail", " rdd_InductionConfirmed=0,rdd_ReasonforRejection='" + txtRemarks.Text + "' ", " rdd_Id=" + ID.Value);
                            break;
                    }
                }
                else
                {
                    switch (drpInterviewOutcome.SelectedItem.Value)
                    {
                        case "1":
                            NoOfRowEffected = oDBEngine.SetFieldValue("tbl_trans_RecruitmentDetail", " rde_EmployementConfirmed=1,rde_JoinDate='" + txtJoinDateAggrement.Value.ToString() + "', rde_ApprovedCTC =" + txtApprovedCTC.Text, " rde_Id=" + ID.Value);
                            if (NoOfRowEffected != 0)
                            {
                                int vacancies = 0;
                                int selected = 0;
                                string[,] data = oDBEngine.GetFieldValue("tbl_trans_Recruitment", "rd_Vacancies", " rd_ActivityId=" + ActId, 1);
                                if (data[0, 0].ToString() != "n")
                                {
                                    vacancies = int.Parse(data[0, 0].ToString());
                                }
                                data = oDBEngine.GetFieldValue("tbl_trans_RecruitmentDetail", "CASE ISNULL(rde_EmployementConfirmed, 5) WHEN 5 THEN '' WHEN 0 THEN '0' WHEN 1 THEN '1' END AS Confirmed", " rde_Activityid=" + ActId, 1);
                                if (data[0, 0].ToString() != "n")
                                {
                                    for (int i = 0; i < data.Length; i++)
                                    {
                                        if (data[i, 0].ToString() == "1")
                                            selected += 1;
                                    }
                                    if (vacancies == selected)
                                        NoOfRowEffected = oDBEngine.SetFieldValue("tbl_trans_Activies", " act_actualEndDate='" + oDBEngine.GetDate() + "' ", " act_id=" + ActId);
                                }
                            }
                            break;
                        case "2":
                            NoOfRowEffected = oDBEngine.SetFieldValue("tbl_trans_RecruitmentDetail", " rde_EmployementConfirmed=0,rde_Reason='" + txtRemarks + "' ", " rde_Id=" + ID.Value);
                            break;
                        case "4":
                            oDBEngine.messageTableUpdate(drpNextInterviewer.SelectedItem.Value, "0", "Recruitment", txtNextInterviewDate.Value.ToString(), Convert.ToDateTime(txtNextInterviewDate.Value.ToString()).AddDays(1).ToString(), "", "", ActId.ToString(), "activity");
                            string MsgContent = " Hello  " + drpNextInterviewer.SelectedItem.Text + " ! " + HttpContext.Current.Session["username"].ToString() + " has allotted a New [ Interview ] type of activity to you, to be held on " + txtNextInterviewDate.Text + " at " + drpNextInterviewPlace.SelectedItem.Text;

                            string fields = "[rem_createUser],[rem_createDate],[rem_targetUser],[rem_startDate],[rem_endDate],[rem_reminderContent],[rem_displayTricker],[rem_actionTaken],[rem_sourceid],[CreateDate],[CreateUser]";
                            string values = "'0',getdate(),'" + drpNextInterviewer.SelectedValue + "','" + txtNextInterviewDate.Value + "','" + txtNextInterviewDate.Value + "','" + MsgContent + "','1','0','" + ActId + "',getdate(),'" + HttpContext.Current.Session["userid"].ToString() + "'";
                            oDBEngine.InsurtFieldValue(" tbl_trans_reminder ", fields, values);
                            break;
                        case "5":
                            oDBEngine.messageTableUpdate(drpNextInterviewer.SelectedItem.Value, "0", "Recruitment", txtNextInterviewDate.Value.ToString(), Convert.ToDateTime(txtNextInterviewDate.Value.ToString()).AddDays(1).ToString(), "", "", ActId.ToString(), "activity");
                            MsgContent = " Hello  " + drpNextInterviewer.SelectedItem.Text + " ! " + HttpContext.Current.Session["username"].ToString() + " has allotted a New [ Interview ] type of activity to you, to be held on " + txtNextInterviewDate.Text + " at " + drpNextInterviewPlace.SelectedItem.Text;

                            fields = "[rem_createUser],[rem_createDate],[rem_targetUser],[rem_startDate],[rem_endDate],[rem_reminderContent],[rem_displayTricker],[rem_actionTaken],[rem_sourceid],[CreateDate],[CreateUser]";
                            values = "'0',getdate(),'" + drpNextInterviewer.SelectedValue + "','" + txtNextInterviewDate.Value + "','" + txtNextInterviewDate.Value + "','" + MsgContent + "','1','0','" + ActId + "',getdate(),'" + HttpContext.Current.Session["userid"].ToString() + "'";
                            oDBEngine.InsurtFieldValue(" tbl_trans_reminder ", fields, values);
                            break;
                        case "6":
                            NoOfRowEffected = oDBEngine.SetFieldValue("tbl_trans_RecruitmentDetail", " rde_EmployementConfirmed=0,rde_Reason='" + txtRemarks + "' ", " rde_Id=" + ID.Value);
                            break;
                    }
                }
            }
            if (Request.QueryString["id1"] != null)
            {
                Response.Write("<script language='javaScript'>var hd=window.opener.document.getElementById('ctl00$ContentPlaceHolder3$Hd1');hd.value='" + ActId + "';window.opener.CandidateRefresh(); window.close(); </script>");
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
                Response.Write("<script language='javaScript'> window.close(); </script>");
            }
            else
            {
                Response.Redirect("frm_UserRecruitmentEmployee_Detail.aspx?id=" + Session["KeyVal"], false);
            }
        }
    }
}
