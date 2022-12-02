using System;
using System.Configuration;
using System.Data;
using BusinessLogicLayer;
namespace ERP.OMS.Management.Activities
{
    public partial class management_activities_frm_CandidateDetail : ERP.OMS.ViewState_class.VSPage
    {
       // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]); MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        DataTable DT = new DataTable();
        Converter Oconverter = new Converter();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                drpAggrementDate.EditFormatString = Oconverter.GetDateFormat("DateTime");
                drpOfferLetterDate.EditFormatString = Oconverter.GetDateFormat("DateTime");
                drpJoinDateAggrement.EditFormatString = Oconverter.GetDateFormat("DateTime");

                int ID = int.Parse(Request.QueryString["Id"].ToString());
                hdID.Value = ID.ToString();
                // DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
                DT = oDBEngine.GetDataTable("tbl_trans_RecruitmentDetail INNER JOIN   tbl_master_education ON tbl_trans_RecruitmentDetail.rde_EduQualification = tbl_master_education.edu_id INNER JOIN    tbl_master_education tbl_master_Profession ON tbl_trans_RecruitmentDetail.rde_ProfQualification = tbl_master_Profession.edu_id INNER JOIN    tbl_master_maritalstatus ON tbl_trans_RecruitmentDetail.rde_MaritalStatus = tbl_master_maritalstatus.mts_id INNER JOIN   tbl_master_jobResponsibility ON tbl_trans_RecruitmentDetail.rde_CurrentJobProfile = tbl_master_jobResponsibility.job_id INNER JOIN    tbl_master_jobResponsibility tbl_master_PreviousjobResponsibility ON    tbl_trans_RecruitmentDetail.rde_PreviousJobProfile = tbl_master_PreviousjobResponsibility.job_id", "tbl_trans_RecruitmentDetail.rde_Activityid, tbl_trans_RecruitmentDetail.rde_Name AS Name,   tbl_trans_RecruitmentDetail.rde_SourceType AS SourceType,   CASE tbl_trans_RecruitmentDetail.rde_CandidateSex WHEN 0 THEN 'Male' WHEN 1 THEN 'Female' END AS Sex,   convert(varchar(11),tbl_trans_RecruitmentDetail.rde_DOB,113) AS Dob, tbl_master_education.edu_education AS Education,  tbl_master_Profession.edu_education AS ProfQualification, tbl_trans_RecruitmentDetail.rde_Certification AS Certification,    tbl_trans_RecruitmentDetail.rde_WorkExp AS WorkExp, tbl_trans_RecruitmentDetail.rde_CTC AS CTC,    tbl_trans_RecruitmentDetail.rde_ReasonForChange AS ReasonForChange, cast(tbl_trans_RecruitmentDetail.rde_ProbableJoinDate as datetime) AS JoinDate,tbl_trans_RecruitmentDetail.rde_ResidenceLocation AS Location, tbl_trans_RecruitmentDetail.rde_CurrentEmployer AS CurrentEmployer,  tbl_master_jobResponsibility.job_responsibility AS JobResponsibility, tbl_trans_RecruitmentDetail.rde_CurrentCTC AS CurrentCTC,tbl_trans_RecruitmentDetail.rde_PreviousEmployer AS PreviousEmployer,  tbl_master_PreviousjobResponsibility.job_responsibility AS PreviousJobResponsibility,    tbl_trans_RecruitmentDetail.rde_PreviousCTC AS PreviousCTc, tbl_master_maritalstatus.mts_maritalStatus AS MaritalStatus,    tbl_trans_RecruitmentDetail.rde_NoofDepedent AS NoofDependents, tbl_trans_RecruitmentDetail.rde_PhoneNo AS PhoneNo,   tbl_trans_RecruitmentDetail.rde_Email AS Email, tbl_trans_RecruitmentDetail.rde_ApprovedCTC AS ApprovedCTC,   tbl_trans_RecruitmentDetail.rde_JoinDate AS JoinDate, tbl_trans_RecruitmentDetail.rde_OfferLetterIssueDate AS OfferedLetter,   tbl_trans_RecruitmentDetail.rde_EmploymentSignDate AS SignDate, tbl_trans_RecruitmentDetail.rde_Reason AS Reason,   CASE isnull(tbl_trans_RecruitmentDetail.rde_EmployementConfirmed, 3) WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' END AS Confirmed ,tbl_trans_RecruitmentDetail.rde_JoinDate, tbl_trans_RecruitmentDetail.rde_OfferLetterIssueDate, tbl_trans_RecruitmentDetail.rde_EmploymentSignDate, tbl_trans_RecruitmentDetail.rde_ApprovedCTC", " tbl_trans_RecruitmentDetail.rde_Id =" + ID);
                if (DT.Rows.Count != 0)
                {
                    lblName.Text = DT.Rows[0][1].ToString();
                    lblSourceType.Text = DT.Rows[0][2].ToString();
                    lblSex.Text = DT.Rows[0][3].ToString();
                    lblDOB.Text = DT.Rows[0][4].ToString();
                    lblQualification.Text = DT.Rows[0][5].ToString();
                    lblProfQualification.Text = DT.Rows[0][6].ToString();
                    lblCertification.Text = DT.Rows[0][7].ToString();
                    lblExpYrs.Text = DT.Rows[0][8].ToString();
                    lblDesiredCTC.Text = DT.Rows[0][9].ToString();
                    tlblReasonforChange.Text = DT.Rows[0][10].ToString();
                    lblPJD.Text = DT.Rows[0][11].ToString();
                    lblLocality.Text = DT.Rows[0][12].ToString();
                    lblCurrentEmployer.Text = DT.Rows[0][13].ToString();
                    lblCurrentJobProfile.Text = DT.Rows[0][14].ToString();
                    lblCurrentCTC.Text = DT.Rows[0][15].ToString();
                    lblPreviousCTC.Text = DT.Rows[0][16].ToString();
                    lblPreviousJobProfile.Text = DT.Rows[0][17].ToString();
                    lblPreviousCTC.Text = DT.Rows[0][18].ToString();
                    lblMaritalStatus.Text = DT.Rows[0][19].ToString();
                    lblNoofDependent.Text = DT.Rows[0][20].ToString();
                    lblPhone.Text = DT.Rows[0][21].ToString();
                    lblEmailId.Text = DT.Rows[0][22].ToString();
                    txtApprovedCTC.Text = DT.Rows[0][23].ToString();
                    if (DT.Rows[0][24].ToString() != "" && DT.Rows[0][24].ToString() != "1/1/1900 12:00:00 AM")
                    {
                        drpAggrementDate.Value = Convert.ToDateTime(DT.Rows[0][24].ToString());
                    }
                    else
                        drpAggrementDate.Value = "";
                    if (DT.Rows[0][25].ToString() != "" && DT.Rows[0][25].ToString() != "1/1/1900 12:00:00 AM")
                    {
                        drpOfferLetterDate.Value = Convert.ToDateTime(DT.Rows[0][25].ToString());
                    }
                    else
                        drpOfferLetterDate.Value = "";
                    if (DT.Rows[0][26].ToString() != "" && DT.Rows[0][26].ToString() != "1/1/1900 12:00:00 AM")
                    {
                        drpJoinDateAggrement.Value = Convert.ToDateTime(DT.Rows[0][26].ToString());
                    }
                    else
                        drpJoinDateAggrement.Value = "";
                }
                DT = oDBEngine.GetDataTable("tbl_trans_RecruitmentDetail INNER JOIN    tbl_trans_Interview ON tbl_trans_RecruitmentDetail.rde_Id = tbl_trans_Interview.int_InternalId AND   tbl_trans_RecruitmentDetail.rde_IntId = tbl_trans_Interview.int_Id", "tbl_trans_Interview.int_InterviewDataTime, CASE ISNULL(tbl_trans_Interview.int_CommunicationSkill, 10)  WHEN 10 THEN '' WHEN 0 THEN 'Clearly Outstanding' WHEN 1 THEN 'Excellent' WHEN 2 THEN 'Satisfactory' WHEN 3 THEN 'Some Deficiency' WHEN    4 THEN 'Unsatisfactory' END AS CommunicationSkills, CASE ISNULL(tbl_trans_Interview.int_Personality, 10)   WHEN 10 THEN '' WHEN 0 THEN 'Clearly Outstanding' WHEN 1 THEN 'Excellent' WHEN 2 THEN 'Satisfactory' WHEN 3 THEN 'Some Deficiency' WHEN     4 THEN 'Unsatisfactory' END AS Personalty, CASE ISNULL(tbl_trans_Interview.int_SalesOrientationApproach, 10)  WHEN 10 THEN '' WHEN 0 THEN 'Clearly Outstanding' WHEN 1 THEN 'Excellent' WHEN 2 THEN 'Satisfactory' WHEN 3 THEN 'Some Deficiency' WHEN   4 THEN 'Unsatisfactory' END AS SalesOrientationApproach, CASE ISNULL(tbl_trans_Interview.[int_JobKnowledge&ComputerSkill], 10)  WHEN 10 THEN '' WHEN 0 THEN 'Clearly Outstanding' WHEN 1 THEN 'Excellent' WHEN 2 THEN 'Satisfactory' WHEN 3 THEN 'Some Deficiency' WHEN  4 THEN 'Unsatisfactory' END AS JobKnowledge, CASE ISNULL(tbl_trans_Interview.int_ProblemSolvingSkill, 10)  WHEN 10 THEN '' WHEN 0 THEN 'Cl early Outstanding' WHEN 1 THEN 'Excellent' WHEN 2 THEN 'Satisfactory' WHEN 3 THEN 'Some Deficiency' WHEN   4 THEN 'Unsatisfactory' END AS ProblemSolvingSkills, CASE ISNULL(tbl_trans_Interview.int_LeadershipSkill, 10)  WHEN 10 THEN '' WHEN 0 THEN 'Clearly Outstanding' WHEN 1 THEN 'Excellent' WHEN 2 THEN 'Satisfactory' WHEN 3 THEN 'Some Deficiency' WHEN  4 THEN 'Unsatisfactory' END AS LeadershipSkills, CASE ISNULL(tbl_trans_Interview.int_InterestInOrganisation, 10)  WHEN 10 THEN '' WHEN 0 THEN 'Clearly Outstanding' WHEN 1 THEN 'Excellent' WHEN 2 THEN 'Satisfactory' WHEN 3 THEN 'Some Deficiency' WHEN  4 THEN 'Unsatisfactory' END AS InterestInOrganisation, CASE ISNULL(tbl_trans_Interview.int_InterestinPosition, 10)   WHEN 0 THEN 'Clearly Outstanding' WHEN 10 THEN '' WHEN 1 THEN 'Excellent' WHEN 2 THEN 'Satisfactory' WHEN 3 THEN 'Some Deficiency' WHEN  4 THEN 'Unsatisfactory' END AS InterestInPosition, CASE ISNULL(tbl_trans_Interview.int_ClearityofCareerGoal, 10)  WHEN 0 THEN 'Clearly Outstanding' WHEN 10 THEN '' WHEN 1 THEN 'Excellent' WHEN 2 THEN 'Satisfactory' WHEN 3 THEN 'Some Deficiency' WHEN   4 THEN 'Unsatisfactory' END AS CarrerGoal,   CASE tbl_trans_Interview.int_InterviewOutcome WHEN 0 THEN 'Due' WHEN 1 THEN 'Recruit' WHEN 2 THEN 'Eliminate' WHEN 3 THEN 'Hold' WHEN 4 THEN  'FurtherInterview' WHEN 5 THEN 'PostpondInterview' WHEN 6 THEN 'CancelInterview' END AS Outcome", " tbl_trans_RecruitmentDetail.rde_Id = " + ID);
                if (DT.Rows.Count != 0)
                {
                    lblCommunicationSkills.Text = DT.Rows[0][1].ToString();
                    lblPersonalty.Text = DT.Rows[0][2].ToString();
                    lblSalesOrientationApproach.Text = DT.Rows[0][3].ToString();
                    lblJobKnowledge.Text = DT.Rows[0][4].ToString();
                    lblProblemSolvingSkills.Text = DT.Rows[0][5].ToString();
                    lblLeadershipSkills.Text = DT.Rows[0][6].ToString();
                    lblIntInOrganisation.Text = DT.Rows[0][7].ToString();
                    lblIntInPosition.Text = DT.Rows[0][8].ToString();
                    lblCarrerGoal.Text = DT.Rows[0][9].ToString();
                    lblInterviewOutcome.Text = DT.Rows[0][10].ToString();
                }
                DT = oDBEngine.GetDataTable("tbl_trans_Interview INNER JOIN   tbl_master_branch ON tbl_trans_Interview.int_InterviewPlace = tbl_master_branch.branch_id INNER JOIN  tbl_master_user ON tbl_trans_Interview.int_InterviewerName = tbl_master_user.user_id", "tbl_trans_Interview.int_InterviewDataTime InterviewDatatime, tbl_master_user.user_name as Interviewer, tbl_master_branch.branch_description as Branch,  CASE isnull(tbl_trans_Interview.int_InterviewOutcome, 10)  WHEN 10 THEN '' WHEN 0 THEN 'Due' WHEN 1 THEN 'Recruit' WHEN 2 THEN 'Eliminate' WHEN 3 THEN 'Hold' WHEN 4 THEN 'Furture Interview' WHEN   5 THEN 'PostPond Interview' WHEN 6 THEN 'Cancel Interview' END AS InterviewOutcome", "tbl_trans_Interview.int_InternalId=" + ID);
                if (DT.Rows.Count != 0)
                {
                    GridInterviewHistory.DataSource = DT;
                    GridInterviewHistory.DataBind();
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string fieldWvalue = "rde_ApprovedCTC=" + txtApprovedCTC.Text;
            if (drpAggrementDate.Value != "" && drpAggrementDate.Value != "1/1/1900 12:00:00 AM")
                fieldWvalue += ",rde_EmploymentSignDate='" + drpAggrementDate.Value + "'";
            else
                fieldWvalue += ",rde_EmploymentSignDate=''";
            if (drpOfferLetterDate.Value != "" && drpOfferLetterDate.Value != "1/1/1900 12:00:00 AM")
                fieldWvalue += ",rde_OfferLetterIssueDate='" + drpOfferLetterDate.Value + "'";
            else
                fieldWvalue += ",rde_OfferLetterIssueDate=''";
            if (drpJoinDateAggrement.Value != "" && drpJoinDateAggrement.Value != "1/1/1900 12:00:00 AM")
                fieldWvalue += ",rde_JoinDate='" + drpJoinDateAggrement.Value + "'";
            else
                fieldWvalue += ",rde_JoinDate=''";
            if (fieldWvalue != "")
            {
                // DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);

                int NoOfRowEffected = oDBEngine.SetFieldValue("tbl_trans_RecruitmentDetail", fieldWvalue, " rde_Id=" + hdID.Value);
            }
            //__________________Here we will write code to close window and return to the calling page____//

        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }
    }
}