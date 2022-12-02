using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;
namespace ERP.OMS.Management.Activities
{
    public partial class management_activities_frm_UserRecruitmentEmployee : ERP.OMS.ViewState_class.VSPage
    {
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
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            if (!IsPostBack)
            {

            }
            String requesttype = Request.QueryString["id"];
            switch (requesttype)
            {
                case "NewActivity":
                    ASPxPageControl1.ActiveTabIndex = 1;
                    FillGrid("NewActivity");
                    break;
                case "PendingInterview":
                    ASPxPageControl1.ActiveTabIndex = 2;
                    FillGrid("PendingInterview");
                    break;
                case "CompletedInterview":
                    ASPxPageControl1.ActiveTabIndex = 3;
                    FillGrid("CompletedInterview");
                    break;
                case "SelectedCandidate":
                    ASPxPageControl1.ActiveTabIndex = 4;
                    FillGrid("SelectedCandidate");
                    break;
                case "EliminatedCandidate":
                    ASPxPageControl1.ActiveTabIndex = 5;
                    FillGrid("EliminatedCandidate");
                    break;
            }
        }

        private void FillGrid(String ActivityType)
        {
            DataTable DT = new DataTable();
            DataTable DT1 = new DataTable();
            DataTable DT2 = new DataTable();
            DataColumn Col1 = new DataColumn("Id");
            DataColumn Col2 = new DataColumn("ActNo");
            DataColumn Col3 = new DataColumn("Company");
            DataColumn Col4 = new DataColumn("Branch");
            DataColumn Col5 = new DataColumn("Department");
            DataColumn Col6 = new DataColumn("Vacancies");
            DataColumn Col7 = new DataColumn("UserName");
            DataColumn Col8 = new DataColumn("JobResponsibility");
            DT1.Columns.Add(Col1);
            DT1.Columns.Add(Col2);
            DT1.Columns.Add(Col3);
            DT1.Columns.Add(Col4);
            DT1.Columns.Add(Col5);
            DT1.Columns.Add(Col6);
            DT1.Columns.Add(Col7);
            DT1.Columns.Add(Col8);
            //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            switch (ActivityType)
            {
                case "NewActivity":
                    DataColumn Col9 = new DataColumn("Selected");
                    DT1.Columns.Add(Col9);
                    DT = oDBEngine.GetDataTable("tbl_trans_Activies,tbl_trans_Recruitment, tbl_master_company,  tbl_master_costCenter,tbl_master_branch, tbl_master_user, tbl_master_jobResponsibility", "tbl_trans_Activies.act_id AS Id,tbl_trans_Activies.act_activityNo as ActNo, tbl_master_company.cmp_Name AS Company, tbl_master_branch.branch_description AS Branch,  tbl_master_costCenter.cost_description AS Department,tbl_trans_Recruitment.rd_Vacancies as Vacancies , tbl_master_user.user_name AS UserName, tbl_master_jobResponsibility.job_responsibility as JobResponsibility", "tbl_trans_Activies.act_activityType =7 And tbl_trans_Activies.act_assignedTo =" + HttpContext.Current.Session["userid"] + " and tbl_trans_Activies.act_id = tbl_trans_Recruitment.rd_ActivityId and tbl_trans_Recruitment.rd_Company = tbl_master_company.cmp_id and tbl_trans_Recruitment.rd_Department = tbl_master_costCenter.cost_id and tbl_trans_Recruitment.rd_Branch = tbl_master_branch.branch_id and tbl_trans_Activies.act_assignedBy = tbl_master_user.user_id and  tbl_trans_Recruitment.rd_JobResponsibility = tbl_master_jobResponsibility.job_id");
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            DT2 = oDBEngine.GetDataTable("tbl_trans_RecruitmentDetail", "rde_Id", "rde_Activityid =" + DT.Rows[i][0]);
                            if (DT2.Rows.Count == 0)
                            {
                                DataRow RowNew = DT1.NewRow();
                                RowNew["Id"] = DT.Rows[i][0];
                                RowNew["ActNo"] = DT.Rows[i][1];
                                RowNew["Company"] = DT.Rows[i][2];
                                RowNew["Branch"] = DT.Rows[i][3];
                                RowNew["Department"] = DT.Rows[i][4];
                                RowNew["Vacancies"] = DT.Rows[i][5];
                                RowNew["UserName"] = DT.Rows[i][6];
                                RowNew["JobResponsibility"] = DT.Rows[i][7];
                                RowNew["Selected"] = 0;
                                DT1.Rows.Add(RowNew);
                            }
                        }
                    }
                    if (DT1.Rows.Count != 0)
                    {
                        GridNewActivity.DataSource = DT1;
                        GridNewActivity.DataBind();
                    }
                    break;
                case "PendingInterview":
                    Col9 = new DataColumn("Selected");
                    DT1.Columns.Add(Col9);
                    DT = oDBEngine.GetDataTable("tbl_trans_Activies,tbl_trans_Recruitment, tbl_master_company,  tbl_master_costCenter,tbl_master_branch, tbl_master_user, tbl_master_jobResponsibility", "tbl_trans_Activies.act_id AS Id,tbl_trans_Activies.act_activityNo as ActNo, tbl_master_company.cmp_Name AS Company, tbl_master_branch.branch_description AS Branch,  tbl_master_costCenter.cost_description AS Department,tbl_trans_Recruitment.rd_Vacancies as Vacancies , tbl_master_user.user_name AS UserName, tbl_master_jobResponsibility.job_responsibility as JobResponsibility", "tbl_trans_Activies.act_activityType =7 And tbl_trans_Activies.act_assignedTo =" + HttpContext.Current.Session["userid"] + " and tbl_trans_Activies.act_id = tbl_trans_Recruitment.rd_ActivityId and tbl_trans_Recruitment.rd_Company = tbl_master_company.cmp_id and tbl_trans_Recruitment.rd_Department = tbl_master_costCenter.cost_id and tbl_trans_Recruitment.rd_Branch = tbl_master_branch.branch_id and tbl_trans_Activies.act_assignedBy = tbl_master_user.user_id and  tbl_trans_Recruitment.rd_JobResponsibility = tbl_master_jobResponsibility.job_id");
                    if (DT.Rows.Count != 0)
                    {
                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            DT2 = oDBEngine.GetDataTable("tbl_trans_RecruitmentDetail", "rde_Id", "rde_Activityid =" + DT.Rows[i][0]);
                            if (DT2.Rows.Count > 0)
                            {
                                DataTable DT3 = oDBEngine.GetDataTable("tbl_trans_RecruitmentDetail", "rde_Id", " rde_EmployementConfirmed=1 And rde_Activityid =" + DT.Rows[i][0]);
                                DataRow RowNew = DT1.NewRow();
                                RowNew["Id"] = DT.Rows[i][0];
                                RowNew["ActNo"] = DT.Rows[i][1];
                                RowNew["Company"] = DT.Rows[i][2];
                                RowNew["Branch"] = DT.Rows[i][3];
                                RowNew["Department"] = DT.Rows[i][4];
                                RowNew["Vacancies"] = DT.Rows[i][5];
                                RowNew["UserName"] = DT.Rows[i][6];
                                RowNew["JobResponsibility"] = DT.Rows[i][7];
                                if (DT3.Rows.Count != 0)
                                {
                                    RowNew["Selected"] = DT3.Rows.Count;
                                }
                                else
                                {
                                    RowNew["Selected"] = 0;
                                }
                                DT1.Rows.Add(RowNew);
                            }
                        }
                    }
                    if (DT1.Rows.Count != 0)
                    {
                        GridNewActivity1.DataSource = DT1;
                        GridNewActivity1.DataBind();
                    }
                    break;
                case "CompletedInterview":
                    DT = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN   tbl_trans_Recruitment ON tbl_trans_Activies.act_id = tbl_trans_Recruitment.rd_ActivityId INNER JOIN   tbl_master_company ON tbl_trans_Recruitment.rd_Company = tbl_master_company.cmp_id INNER JOIN   tbl_master_costCenter ON tbl_trans_Recruitment.rd_Department = tbl_master_costCenter.cost_id INNER JOIN   tbl_master_branch ON tbl_trans_Recruitment.rd_Branch = tbl_master_branch.branch_id INNER JOIN   tbl_trans_RecruitmentDetail ON tbl_trans_Recruitment.rd_ActivityId = tbl_trans_RecruitmentDetail.rde_Activityid INNER JOIN   tbl_trans_Interview ON tbl_trans_RecruitmentDetail.rde_Activityid = tbl_trans_Interview.int_ActivityId AND    tbl_trans_RecruitmentDetail.rde_Id = tbl_trans_Interview.int_InternalId INNER JOIN    tbl_master_user ON tbl_trans_Activies.act_assignedBy = tbl_master_user.user_id  INNER JOIN  tbl_master_jobResponsibility ON tbl_trans_Recruitment.rd_JobResponsibility = tbl_master_jobResponsibility.job_id", "distinct tbl_trans_Activies.act_id AS Id,tbl_trans_Activies.act_activityNo as ActNo, tbl_trans_RecruitmentDetail.rde_Name AS CandidateName, tbl_master_branch.branch_description AS Branch,  tbl_master_costCenter.cost_description AS Department, tbl_trans_Recruitment.rd_Vacancies AS Vacancies,   tbl_master_user.user_name AS UserName , tbl_master_company.cmp_Name AS Company , tbl_master_jobResponsibility.job_responsibility as JobResponsibility", " tbl_trans_Activies.act_actualEndDate IS NULL AND tbl_trans_Interview.int_InterviewOutcome <> 0 And tbl_trans_Activies.act_assignedTo =" + HttpContext.Current.Session["userid"]);
                    if (DT.Rows.Count != 0)
                    {
                        GridCompltIntervw.DataSource = DT;
                        GridCompltIntervw.DataBind();
                    }
                    break;
                case "SelectedCandidate":
                    DT = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN   tbl_trans_Recruitment ON tbl_trans_Activies.act_id = tbl_trans_Recruitment.rd_ActivityId INNER JOIN   tbl_master_company ON tbl_trans_Recruitment.rd_Company = tbl_master_company.cmp_id INNER JOIN   tbl_master_costCenter ON tbl_trans_Recruitment.rd_Department = tbl_master_costCenter.cost_id INNER JOIN   tbl_master_branch ON tbl_trans_Recruitment.rd_Branch = tbl_master_branch.branch_id INNER JOIN   tbl_trans_RecruitmentDetail ON tbl_trans_Recruitment.rd_ActivityId = tbl_trans_RecruitmentDetail.rde_Activityid INNER JOIN   tbl_trans_Interview ON tbl_trans_RecruitmentDetail.rde_Activityid = tbl_trans_Interview.int_ActivityId AND    tbl_trans_RecruitmentDetail.rde_Id = tbl_trans_Interview.int_InternalId INNER JOIN    tbl_master_user ON tbl_trans_Activies.act_assignedBy = tbl_master_user.user_id  INNER JOIN  tbl_master_jobResponsibility ON tbl_trans_Recruitment.rd_JobResponsibility = tbl_master_jobResponsibility.job_id", "distinct tbl_trans_Activies.act_id AS Id,tbl_trans_Activies.act_activityNo as ActNo, tbl_trans_RecruitmentDetail.rde_Name AS CandidateName, tbl_master_branch.branch_description AS Branch,  tbl_master_costCenter.cost_description AS Department, tbl_trans_Recruitment.rd_Vacancies AS Vacancies,   tbl_master_user.user_name AS UserName , tbl_master_company.cmp_Name AS Company , tbl_master_jobResponsibility.job_responsibility as JobResponsibility, tbl_trans_Interview.int_InternalId AS RecruitmentId", " tbl_trans_Activies.act_actualEndDate IS NULL AND tbl_trans_Interview.int_InterviewOutcome =1 And tbl_trans_Activies.act_assignedTo =" + HttpContext.Current.Session["userid"]);
                    if (DT.Rows.Count != 0)
                    {
                        GridCompltIntervw1.DataSource = DT;
                        GridCompltIntervw1.DataBind();
                    }
                    break;
                case "EliminatedCandidate":
                    DT = oDBEngine.GetDataTable(" tbl_trans_Activies INNER JOIN   tbl_trans_Recruitment ON tbl_trans_Activies.act_id = tbl_trans_Recruitment.rd_ActivityId INNER JOIN   tbl_master_company ON tbl_trans_Recruitment.rd_Company = tbl_master_company.cmp_id INNER JOIN   tbl_master_costCenter ON tbl_trans_Recruitment.rd_Department = tbl_master_costCenter.cost_id INNER JOIN   tbl_master_branch ON tbl_trans_Recruitment.rd_Branch = tbl_master_branch.branch_id INNER JOIN   tbl_trans_RecruitmentDetail ON tbl_trans_Recruitment.rd_ActivityId = tbl_trans_RecruitmentDetail.rde_Activityid INNER JOIN   tbl_trans_Interview ON tbl_trans_RecruitmentDetail.rde_Activityid = tbl_trans_Interview.int_ActivityId AND    tbl_trans_RecruitmentDetail.rde_Id = tbl_trans_Interview.int_InternalId INNER JOIN    tbl_master_user ON tbl_trans_Activies.act_assignedBy = tbl_master_user.user_id INNER JOIN  tbl_master_jobResponsibility ON tbl_trans_Recruitment.rd_JobResponsibility = tbl_master_jobResponsibility.job_id ", " distinct tbl_trans_Activies.act_id AS Id,tbl_trans_Activies.act_activityNo as ActNo, tbl_trans_RecruitmentDetail.rde_Name AS CandidateName, tbl_master_branch.branch_description AS Branch,  tbl_master_costCenter.cost_description AS Department, tbl_trans_Recruitment.rd_Vacancies AS Vacancies,   tbl_master_user.user_name AS UserName , tbl_master_company.cmp_Name AS Company , tbl_master_jobResponsibility.job_responsibility as JobResponsibility", " tbl_trans_Activies.act_actualEndDate IS NULL AND tbl_trans_Interview.int_InterviewOutcome = 2 And tbl_trans_Activies.act_assignedTo = " + HttpContext.Current.Session["userid"]);
                    if (DT.Rows.Count != 0)
                    {
                        GridEliminated.DataSource = DT;
                        GridEliminated.DataBind();
                    }
                    break;
            }
        }

        protected void ASPxPageControl1_ActiveTabChanged(object source, DevExpress.Web.TabControlEventArgs e)
        {

        }
    }
}