using System;
using System.Data;
using BusinessLogicLayer;
namespace ERP.OMS.Management.Activities
{
    public partial class management_activities_frm_UserRecruitmentEmployee_Detail_popup : ERP.OMS.ViewState_class.VSPage
    {
        //DBEngine oDBEngine = new DBEngine(string.Empty);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);


        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Request.QueryString["id"];
            DataTable DT = oDBEngine.GetDataTable(" tbl_trans_Interview left JOIN    tbl_trans_RecruitmentDetail ON tbl_trans_Interview.int_Id = tbl_trans_RecruitmentDetail.rde_IntId left JOIN    tbl_master_user ON tbl_master_user.user_id = tbl_trans_Interview.int_InterviewerName  ", " convert(varchar(11),int_InterviewDataTime,106)+' '+substring(convert(varchar(19),int_InterviewDataTime,100),13,19) AS InterviewDate, convert(varchar(11),int_NextInterviewData,106)+' '+substring(convert(varchar(19),int_NextInterviewData,100),13,19) AS NextInterviewDate," +
                        " CASE isnull(int_InterviewOutcome,12) when 12 then '' WHEN 0 THEN 'Due' WHEN 1 THEN 'Recruit' WHEN 2 THEN 'Eliminate' WHEN 3 THEN 'Hold' WHEN 4 THEN 'Further Interview '    WHEN 5 THEN 'PostPond Interview' WHEN 6 THEN 'Cancel Interview' END AS Result, " +
                        " tbl_master_user.user_name AS Interviewer, " +
                        " tbl_trans_Interview.int_InterviewPlace,     CASE tbl_trans_Interview.int_NextInterviewer WHEN '' THEN '' ELSE     (SELECT     User_name      FROM          tbl_master_user     WHERE      user_id = int_NextInterviewer) END AS NextInterviewer,     " +
                        " (SELECT     branch_description   FROM          tbl_master_branch     WHERE      branch_id = int_NextInterviewPlace) AS NextInterviewPlace  ",
                        " int_ActivityId = " + Session["KeyVal"] + " And int_InternalId =" + id, " int_id asc ");
            GridCandi1.DataSource = DT.DefaultView;
            GridCandi1.DataBind();
        }
    }
}