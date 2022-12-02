using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
namespace ERP.OMS.Management.Activities
{
    public partial class management_activities_frm_UserRecruitmentEmployee_Detail : ERP.OMS.ViewState_class.VSPage
    {

        DataTable DTInterview = new DataTable();
        public string pageAccess = "";
        //DBEngine oDBEngine = new DBEngine(string.Empty);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);

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
            if (!IsPostBack)
            {
                int keyval = int.Parse(Request.QueryString["id"].ToString());
                Session["KeyVal"] = keyval;
                Label1.Text = Session["KeyVal"].ToString();
                //________Fill activity Details______________//
                fillActivityDetails();
                //________Fill Recruitment Details______________//
                fillRecruitmentDetails();
            }

        }

        private void fillRecruitmentDetails()
        {
            //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            DataTable DTCandi = new DataTable();
            DataTable DT = new DataTable();
            DT = oDBEngine.GetDataTable("tbl_trans_RecruitmentDetail", "rde_Id AS Id, rde_Name AS Name,rde_IntId", "rde_Activityid = " + Session["KeyVal"]);
            DataColumn Col1 = new DataColumn("Id");
            DataColumn Col2 = new DataColumn("Name");
            DataColumn Col3 = new DataColumn("NextInterview");
            DataColumn Col4 = new DataColumn("InterviewDate");
            DataColumn Col5 = new DataColumn("Result");
            DataColumn Col6 = new DataColumn("interviewid");
            DataColumn Col7 = new DataColumn("Interviewer");
            DataColumn Col8 = new DataColumn("NextInterviewer");
            DataColumn Col9 = new DataColumn("NextIntPlace");
            DTCandi.Columns.Add(Col1);
            DTCandi.Columns.Add(Col2);
            DTCandi.Columns.Add(Col3);
            DTCandi.Columns.Add(Col4);
            DTCandi.Columns.Add(Col5);
            DTCandi.Columns.Add(Col6);
            DTCandi.Columns.Add(Col7);
            DTCandi.Columns.Add(Col8);
            DTCandi.Columns.Add(Col9);

            if (DT.Rows.Count != 0)
            {
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    DTInterview = oDBEngine.GetDataTable("tbl_trans_Interview LEFT JOIN    tbl_trans_RecruitmentDetail ON tbl_trans_Interview.int_Id = tbl_trans_RecruitmentDetail.rde_IntId LEFT JOIN    tbl_master_user ON tbl_master_user.user_id = tbl_trans_Interview.int_InterviewerName", "int_Id as Intid, convert(varchar(11),int_InterviewDataTime,106)+' '+substring(convert(varchar(19),int_InterviewDataTime,100),13,19) AS InterviewDate, convert(varchar(11),int_NextInterviewData,106)+' '+substring(convert(varchar(19),int_NextInterviewData,100),13,19) AS NextInterviewDate,   CASE isnull(int_InterviewOutcome,12) when 12 then '' WHEN 0 THEN 'Due' WHEN 1 THEN 'Recruit' WHEN 2 THEN 'Eliminate' WHEN 3 THEN 'Hold' WHEN 4 THEN 'Further Interview '    WHEN 5 THEN 'PostPond Interview' WHEN 6 THEN 'Cancel Interview' END AS Result, tbl_master_user.user_name AS Interviewer,    tbl_trans_Interview.int_NextInterviewData AS NextInterviewDate, tbl_trans_Interview.int_InterviewPlace,     CASE tbl_trans_Interview.int_NextInterviewer WHEN '' THEN '' ELSE     (SELECT     User_name      FROM          tbl_master_user     WHERE      user_id = int_NextInterviewer) END AS NextInterviewer,     (SELECT     branch_description   FROM          tbl_master_branch     WHERE      branch_id = int_NextInterviewPlace) AS NextInterviewPlace ", "int_ActivityId = " + Session["KeyVal"] + " And int_InternalId = " + DT.Rows[i][0].ToString(), " int_id desc");
                    DataRow rownew = DTCandi.NewRow();
                    rownew["Id"] = DT.Rows[i][0];
                    rownew["Name"] = DT.Rows[i][1];
                    rownew["interviewid"] = DT.Rows[i][2];
                    if (DTInterview.Rows.Count != 0)
                    {
                        //if (DTInterview.Rows[0][3].ToString() == "Due")
                        //{
                        //DataTable DTdata = new DataTable();
                        //DTdata = oDBEngine.GetDataTable("tbl_trans_Interview INNER JOIN  tbl_trans_RecruitmentDetail ON tbl_trans_Interview.int_ActivityId = tbl_trans_RecruitmentDetail.rde_Activityid AND   tbl_trans_Interview.int_InternalId = tbl_trans_RecruitmentDetail.rde_Id INNER JOIN     tbl_master_user ON tbl_master_user.user_id = tbl_trans_Interview.int_InterviewerName ", "int_Id as Intid,convert(varchar(11),int_InterviewDataTime,106)+' '+substring(convert(varchar(19),int_InterviewDataTime,100),13,19) AS InterviewDate,convert(varchar(11),int_NextInterviewData,106)+' '+substring(convert(varchar(19),int_NextInterviewData,100),13,19) AS NextInterviewDate,CASE int_InterviewOutcome WHEN 0 THEN 'Due' WHEN 1 THEN 'Recruit' WHEN 2 THEN 'Eliminate' WHEN 3 THEN 'Hold' WHEN 4 THEN 'Further Interview '    WHEN 5 THEN 'PostPond Interview' WHEN 6 THEN 'Cancel Interview' END AS Result, tbl_master_user.user_name AS Interviewer,tbl_trans_Interview.int_NextInterviewData AS NextInterviewDate,tbl_trans_Interview.int_InterviewPlace,CASE tbl_trans_Interview.int_NextInterviewer WHEN '' THEN '' ELSE     (SELECT     User_name      FROM          tbl_master_user     WHERE      user_id = int_NextInterviewer) END AS NextInterviewer,(SELECT     branch_description   FROM          tbl_master_branch     WHERE      branch_id = int_NextInterviewPlace) AS NextInterviewPlace", "int_ActivityId = " + Session["KeyVal"] + " And int_InternalId = " + DT.Rows[i][0].ToString() + " And Int_id <> " + DTInterview.Rows[0][0] , " int_id desc");
                        //if (DTdata.Rows.Count != 0)
                        //{
                        //    rownew["InterviewDate"] = DTdata.Rows[0][1].ToString();
                        //    if (DTdata.Rows[0][2].ToString() != "")
                        //    {
                        //        rownew["NextInterview"] = DTdata.Rows[0][2].ToString();
                        //    }
                        //    else
                        //        rownew["NextInterview"] = "";

                        //    rownew["Result"] = DTdata.Rows[0][3].ToString();
                        //    rownew["Interviewer"] = DTdata.Rows[0][4].ToString();
                        //    if (DTdata.Rows[0][7].ToString() != "")
                        //    {
                        //        rownew["NextInterviewer"] = DTdata.Rows[0][7].ToString();
                        //    }
                        //    else
                        //        rownew["NextInterviewer"] = "";
                        //    if (DTdata.Rows[0][8].ToString() != "")
                        //    {
                        //        rownew["NextIntPlace"] = DTdata.Rows[0][8].ToString();
                        //    }
                        //    else
                        //        rownew["NextIntPlace"] = "";
                        //}
                        //else
                        //{
                        rownew["InterviewDate"] = DTInterview.Rows[0][1].ToString();
                        if (DTInterview.Rows[0][2].ToString() != "")
                        {
                            rownew["NextInterview"] = DTInterview.Rows[0][2].ToString();
                        }
                        else
                            rownew["NextInterview"] = "";

                        rownew["Result"] = DTInterview.Rows[0][3].ToString();
                        rownew["Interviewer"] = DTInterview.Rows[0][4].ToString();
                        if (DTInterview.Rows[0][7].ToString() != "")
                        {
                            rownew["NextInterviewer"] = DTInterview.Rows[0][7].ToString();
                        }
                        else
                            rownew["NextInterviewer"] = "";
                        if (DTInterview.Rows[0][8].ToString() != "")
                        {
                            rownew["NextIntPlace"] = DTInterview.Rows[0][8].ToString();
                        }
                        else
                            rownew["NextIntPlace"] = "";
                        //}
                        //}
                        //else
                        //{
                        //    rownew["InterviewDate"] = DTInterview.Rows[0][1].ToString();
                        //    if (DTInterview.Rows[0][2].ToString() != "")
                        //    {
                        //        rownew["NextInterview"] = DTInterview.Rows[0][2].ToString();
                        //    }
                        //    else
                        //        rownew["NextInterview"] = "";

                        //    rownew["Result"] = DTInterview.Rows[0][3].ToString();
                        //    rownew["Interviewer"] = DTInterview.Rows[0][4].ToString();
                        //    if (DTInterview.Rows[0][7].ToString() != "")
                        //    {
                        //        rownew["NextInterviewer"] = DTInterview.Rows[0][7].ToString();
                        //    }
                        //    else
                        //        rownew["NextInterviewer"] = "";
                        //    if (DTInterview.Rows[0][8].ToString() != "")
                        //    {
                        //        rownew["NextIntPlace"] = DTInterview.Rows[0][8].ToString();
                        //    }
                        //    else
                        //        rownew["NextIntPlace"] = "";
                        //}

                    }
                    else
                    {
                        rownew["InterviewDate"] = "";
                        rownew["NextInterview"] = "";
                        rownew["Result"] = "";
                        rownew["Interviewer"] = "";
                        rownew["NextInterviewer"] = "";
                        rownew["NextIntPlace"] = "";
                    }
                    DTCandi.Rows.Add(rownew);
                }
            }

            //GridCandi.DataSource = DTCandi;
            //GridCandi.DataBind();

            GridCandi1.DataSource = DTCandi;
            GridCandi1.DataBind();
        }

        private void fillActivityDetails()
        {
            //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            DataTable DT = new DataTable();
            DT = oDBEngine.GetDataTable("tbl_trans_Recruitment INNER JOIN    tbl_trans_Activies ON tbl_trans_Recruitment.rd_ActivityId = tbl_trans_Activies.act_id INNER JOIN   tbl_master_user ON tbl_trans_Activies.act_assignedBy = tbl_master_user.user_id INNER JOIN    tbl_master_education ON tbl_trans_Recruitment.rd_EduQualification = tbl_master_education.edu_id INNER JOIN   tbl_master_education tbl_master_Professional ON tbl_trans_Recruitment.rd_ProfQualification = tbl_master_Professional.edu_id INNER JOIN    tbl_master_jobResponsibility ON tbl_trans_Recruitment.rd_JobResponsibility = tbl_master_jobResponsibility.job_id INNER JOIN   tbl_master_designation ON tbl_trans_Recruitment.rd_Designation = tbl_master_designation.deg_id INNER JOIN  tbl_master_industry ON tbl_master_industry.ind_id = tbl_trans_Recruitment.rd_PreferedIndustry INNER JOIN   tbl_master_costCenter ON tbl_trans_Recruitment.rd_Department = tbl_master_costCenter.cost_id", "tbl_trans_Activies.act_activityNo as ActNo,tbl_trans_Activies.act_createDate AS CreateDate, tbl_trans_Activies.act_scheduledDate AS StartDate, tbl_trans_Activies.act_expectedDate AS EndDate,   tbl_master_user.user_name AS assignedby,case tbl_trans_Activies.act_priority when 0 then 'Low' when 1 then 'Normal' when 2 then 'High' when 3 then 'Urgent' when 4 then 'Immediate' end  AS Priority, tbl_trans_Recruitment.rd_OfferedCTCmin AS MinCTC,   tbl_trans_Recruitment.rd_OfferedCTCmax AS MaxCTC, tbl_trans_Recruitment.rd_YearofExp AS YearofExp,   tbl_master_education.edu_education AS Qualification, tbl_master_Professional.edu_education AS ProfQualification,   tbl_trans_Recruitment.rd_Vacancies as Vacancies, tbl_master_jobResponsibility.job_responsibility as JobResponsibility, tbl_master_designation.deg_designation as Designation,   tbl_trans_Recruitment.rd_JoinDate as JoinDate, tbl_trans_Recruitment.rd_Skilles as Skills, case  tbl_trans_Recruitment.rd_Sex when 0 then 'Male' when 1 then 'Female' when 2 then 'No Preference' End  as Sex,rd_Hobbies, rd_ExtActivity, rd_MinAge, rd_MaxAge, rd_PreferedLocality,rd_PreferedIndustry, ind_industry,rd_department,tbl_trans_Activies.act_instruction ,tbl_master_costCenter.cost_description", "tbl_trans_Activies.act_id=" + Session["KeyVal"]);
            lblActiID.Text = DT.Rows[0][0].ToString();
            lblDateOfAllocation.Text = DT.Rows[0][1].ToString();
            lblAllotedBy.Text = DT.Rows[0][4].ToString();
            lblShedStartDT.Text = DT.Rows[0][2].ToString();
            lblPriority.Text = DT.Rows[0][5].ToString();
            lblShedEndDT.Text = DT.Rows[0][3].ToString();
            lblVacancies.Text = DT.Rows[0][11].ToString();
            lblActualStartDT.Text = "";

            lblJobResponsibility.Text = DT.Rows[0][12].ToString();
            lblDepartment.Text = DT.Rows[0][26].ToString();
            lblPosition.Text = DT.Rows[0][13].ToString();
            DataTable DTHOD = new DataTable();
            DTHOD = oDBEngine.GetDataTable("tbl_master_costCenter INNER JOIN    tbl_master_contact ON tbl_master_costCenter.cost_costCenterHead = tbl_master_contact.cnt_internalId", "ISNULL(tbl_master_contact.cnt_firstName, '') + ' ' + ISNULL(tbl_master_contact.cnt_middleName, '') + ' ' + ISNULL(tbl_master_contact.cnt_lastName, '')   AS Name", "tbl_master_costCenter.cost_id =" + DT.Rows[0][24].ToString());
            if (DTHOD.Rows.Count != 0)
            {
                lblHOD.Text = DTHOD.Rows[0][0].ToString();
            }
            lblEduQualification.Text = DT.Rows[0]["Qualification"].ToString();
            lblCTCRange.Text = DT.Rows[0][6].ToString() + " To " + DT.Rows[0][7].ToString();
            lblMinExp.Text = DT.Rows[0][8].ToString();
            lblExtCurrentActivities.Text = DT.Rows[0][18].ToString();
            lblIndustryExp.Text = DT.Rows[0][23].ToString();
            lblSex.Text = DT.Rows[0]["Sex"].ToString();
            lblSkills.Text = DT.Rows[0][15].ToString();
            lblAge.Text = DT.Rows[0][19].ToString() + " - " + DT.Rows[0][20].ToString();
            lblJoinByDate.Text = DT.Rows[0][14].ToString();
            lblLocationPresference.Text = DT.Rows[0][21].ToString();
            txtNotes.Text = DT.Rows[0][25].ToString();
        }
        protected void btnAddCandidate_Click(object sender, EventArgs e)
        {
            Response.Redirect("frm_AddCandidate.aspx?id=" + Session["KeyVal"] + "&" + "mode=new", false);
        }
        protected void btnUpdateCandidate_Click(object sender, EventArgs e)
        {
            if (GridCandi1.Rows.Count != 0)
            {
                for (int i = 0; i < GridCandi1.Rows.Count; i++)
                {
                    GridViewRow newrow = GridCandi1.Rows[i];
                    CheckBox chkbx = new CheckBox();
                    chkbx = (CheckBox)newrow.FindControl("ChkRDId");
                    if (chkbx.Checked)
                    {
                        Label CandID = (Label)newrow.FindControl("lblCandidateId");
                        Response.Redirect("frm_AddCandidate.aspx?id=" + CandID.Text + "&" + "mode=edit", false);
                    }
                    else
                    {
                        lblmessage.Text = "Select candidate by clicking the check box from candidate list!";
                    }
                }

            }
            else
            {
                lblmessage.Text = "First Add candidate, then select and modify!";
            }
        }
        protected void btnCreateInterview_Click(object sender, EventArgs e)
        {

            if (GridCandi1.Rows.Count != 0)
            {
                for (int i = 0; i < GridCandi1.Rows.Count; i++)
                {
                    GridViewRow newrow = GridCandi1.Rows[i];
                    CheckBox chkbx = new CheckBox();
                    chkbx = (CheckBox)newrow.FindControl("ChkRDId");
                    if (chkbx.Checked)
                    {
                        Label CandID = (Label)newrow.FindControl("lblCandidateId");
                        Response.Redirect("frm_UpdateInterview.aspx?id=" + CandID.Text + "&" + "mode=new", false);
                    }
                    else
                    {
                        lblmessage.Text = "Select candidate by clicking the check box from candidate list!";
                    }
                }

            }
            else
            {
                lblmessage.Text = "First Add candidate, then select and modify!";
            }
        }
        protected void btnModifyInterview_Click(object sender, EventArgs e)
        {
            if (GridCandi1.Rows.Count != 0)
            {
                for (int i = 0; i < GridCandi1.Rows.Count; i++)
                {
                    GridViewRow newrow = GridCandi1.Rows[i];
                    CheckBox chkbx = new CheckBox();
                    chkbx = (CheckBox)newrow.FindControl("ChkRDId");
                    if (chkbx.Checked)
                    {
                        Label CandID = (Label)newrow.FindControl("lblCandidateId");
                        Response.Redirect("frm_UpdateInterview.aspx?id=" + CandID.Text + "&" + "mode=edit", false);
                    }
                    else
                    {
                        lblmessage.Text = "Select candidate by clicking the check box from candidate list!";
                    }
                }

            }
            else
            {
                lblmessage.Text = "First Add candidate, then select and modify!";
            }
        }
        protected void btnUpdateInterview_Click(object sender, EventArgs e)
        {
            if (GridCandi1.Rows.Count != 0)
            {
                for (int i = 0; i < GridCandi1.Rows.Count; i++)
                {
                    GridViewRow newrow = GridCandi1.Rows[i];
                    CheckBox chkbx = new CheckBox();
                    chkbx = (CheckBox)newrow.FindControl("ChkRDId");
                    if (chkbx.Checked)
                    {
                        Label CandID = (Label)newrow.FindControl("lblCandidateId");
                        Label Interviewer = (Label)newrow.FindControl("lblInterviewId1");
                        Label InterviewId = (Label)newrow.FindControl("lblInterviewId");
                        Label name = (Label)newrow.FindControl("lblName");
                        if (Interviewer.Text != "")
                        {
                            //Label 
                            Response.Redirect("frm_AddInterviews.aspx?id=" + CandID.Text + "&name=" + name.Text + "&interviewer=" + Interviewer.Text + "&interviewId=" + InterviewId.Text, false);
                        }
                        else
                            lblmessage.Text = "Inteview has not been created!";
                    }
                    else
                    {
                        lblmessage.Text = "Select candidate by clicking the check box from candidate list!";
                    }
                }

            }
            else
            {
                lblmessage.Text = "First Add candidate, then create Interview and then update IT!";
            }
            //Response.Redirect("frm_AddInterviews.aspx", false);
        }
        #region This is to select only one check box in a grid list
        protected void GridCandi1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)    //___This condition is for filtering header and footer, application will done on Rows only___//
            {
                CheckBox chkbox = new CheckBox();
                chkbox = (CheckBox)e.Row.FindControl("ChkRDId");
                Label lblId = new Label();
                lblId = (Label)e.Row.FindControl("lblCandidateId");
                chkbox.Attributes.Add("onclick", "javascript:chkclicked(this,'" + lblId.Text + "');");
            }
        }
        #endregion

    }
}