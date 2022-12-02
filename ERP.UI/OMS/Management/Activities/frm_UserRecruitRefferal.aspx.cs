using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;
namespace ERP.OMS.Management.Activities
{
    public partial class management_Activities_frm_UserRecruitRefferal : System.Web.UI.Page, ICallbackEventHandler
    {
       // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        
        
        string data;
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
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {
                DataTable DT = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN   tbl_trans_ReferalAgentRecuruitment ON tbl_trans_Activies.act_id = tbl_trans_ReferalAgentRecuruitment.rar_Activityid INNER JOIN  tbl_master_branch ON tbl_trans_ReferalAgentRecuruitment.rar_Branch = tbl_master_branch.branch_id INNER JOIN   tbl_master_company ON tbl_trans_ReferalAgentRecuruitment.rar_Company = tbl_master_company.cmp_id", "tbl_trans_Activies.act_id AS Id, tbl_master_company.cmp_Name AS Company, tbl_master_branch.branch_description AS Branch,  tbl_trans_ReferalAgentRecuruitment.rar_Requirement AS Vacancies", " tbl_trans_Activies.act_activityType =8 And (tbl_trans_Activies.act_assignedTo in (" + Session["userid"].ToString() + "))");
                if (DT != null)
                {
                    gridNewCall.DataSource = DT.DefaultView;
                    gridNewCall.DataBind();
                }
                gridNewCall.Columns[0].Visible = false;
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            }
            String rbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg,context) {" + rbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "CallServer", callbackScript, true);
            Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>InitialPosition();</script>");
        }
        protected void gridNewCalls_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string param = e.Parameters.ToString();
            string UserId = oDBEngine.getChildUser(Session["userid"].ToString(), "");
            DataTable DT = new DataTable();
            switch (param)
            {
                case "New Activity":
                    DT = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN   tbl_trans_ReferalAgentRecuruitment ON tbl_trans_Activies.act_id = tbl_trans_ReferalAgentRecuruitment.rar_Activityid INNER JOIN  tbl_master_branch ON tbl_trans_ReferalAgentRecuruitment.rar_Branch = tbl_master_branch.branch_id INNER JOIN   tbl_master_company ON tbl_trans_ReferalAgentRecuruitment.rar_Company = tbl_master_company.cmp_id", "tbl_trans_Activies.act_id AS Id, tbl_master_company.cmp_Name AS Company, tbl_master_branch.branch_description AS Branch,  tbl_trans_ReferalAgentRecuruitment.rar_Requirement AS Vacancies", " tbl_trans_Activies.act_activityType =8 And (tbl_trans_Activies.act_assignedTo in (" + Session["userid"].ToString() + " ))");
                    break;
                case "Pending":
                    DT = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN  tbl_trans_ReferalAgentRecuruitment ON tbl_trans_Activies.act_id = tbl_trans_ReferalAgentRecuruitment.rar_ActivityId INNER JOIN tbl_master_company ON tbl_trans_ReferalAgentRecuruitment.rar_Company = tbl_master_company.cmp_id INNER JOIN   tbl_master_jobResponsibility ON tbl_trans_ReferalAgentRecuruitment.rar_jobresponsibility = tbl_master_jobResponsibility.job_id INNER JOIN   tbl_master_branch ON tbl_trans_ReferalAgentRecuruitment.rar_Branch = tbl_master_branch.branch_id INNER JOIN    tbl_trans_ReferalAgentDetail ON tbl_trans_ReferalAgentRecuruitment.rar_Activityid = tbl_trans_ReferalAgentDetail.rdd_Activityid INNER JOIN  tbl_trans_Interview ON tbl_trans_ReferalAgentDetail.rdd_Activityid = tbl_trans_Interview.int_ActivityId AND   tbl_trans_ReferalAgentDetail.rdd_Id = tbl_trans_Interview.int_InternalId", "tbl_trans_Activies.act_id AS Id, tbl_trans_ReferalAgentDetail.rdd_CandidateName AS Company, tbl_master_branch.branch_description AS Branch,  tbl_master_jobResponsibility.job_responsibility AS Department", "  (tbl_trans_Interview.int_InterviewOutcome = 0) and tbl_trans_Activies.act_assignedTo='" + Session["userid"].ToString() + "'");
                    break;
                case "SelectedInterview":
                    DT = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN  tbl_trans_ReferalAgentRecuruitment ON tbl_trans_Activies.act_id = tbl_trans_ReferalAgentRecuruitment.rar_ActivityId INNER JOIN tbl_master_company ON tbl_trans_ReferalAgentRecuruitment.rar_Company = tbl_master_company.cmp_id INNER JOIN   tbl_master_jobResponsibility ON tbl_trans_ReferalAgentRecuruitment.rar_jobresponsibility = tbl_master_jobResponsibility.job_id INNER JOIN   tbl_master_branch ON tbl_trans_ReferalAgentRecuruitment.rar_Branch = tbl_master_branch.branch_id INNER JOIN    tbl_trans_ReferalAgentDetail ON tbl_trans_ReferalAgentRecuruitment.rar_Activityid = tbl_trans_ReferalAgentDetail.rdd_Activityid INNER JOIN  tbl_trans_Interview ON tbl_trans_ReferalAgentDetail.rdd_Activityid = tbl_trans_Interview.int_ActivityId AND   tbl_trans_ReferalAgentDetail.rdd_Id = tbl_trans_Interview.int_InternalId", "tbl_trans_Activies.act_id AS Id, tbl_trans_ReferalAgentDetail.rdd_CandidateName AS Company, tbl_master_branch.branch_description AS Branch,  tbl_master_jobResponsibility.job_responsibility AS Department", " (tbl_trans_Interview.int_InterviewOutcome <> 0) and tbl_trans_Activies.act_assignedTo='" + Session["userid"].ToString() + "'");
                    break;
                case "SelectedCandidate":
                    DT = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN  tbl_trans_ReferalAgentRecuruitment ON tbl_trans_Activies.act_id = tbl_trans_ReferalAgentRecuruitment.rar_ActivityId INNER JOIN tbl_master_company ON tbl_trans_ReferalAgentRecuruitment.rar_Company = tbl_master_company.cmp_id INNER JOIN   tbl_master_jobResponsibility ON tbl_trans_ReferalAgentRecuruitment.rar_jobresponsibility = tbl_master_jobResponsibility.job_id INNER JOIN   tbl_master_branch ON tbl_trans_ReferalAgentRecuruitment.rar_Branch = tbl_master_branch.branch_id INNER JOIN    tbl_trans_ReferalAgentDetail ON tbl_trans_ReferalAgentRecuruitment.rar_Activityid = tbl_trans_ReferalAgentDetail.rdd_Activityid INNER JOIN  tbl_trans_Interview ON tbl_trans_ReferalAgentDetail.rdd_Activityid = tbl_trans_Interview.int_ActivityId AND   tbl_trans_ReferalAgentDetail.rdd_Id = tbl_trans_Interview.int_InternalId", "tbl_trans_Activies.act_id AS Id, tbl_trans_ReferalAgentDetail.rdd_CandidateName AS Company, tbl_master_branch.branch_description AS Branch,  tbl_master_jobResponsibility.job_responsibility AS Department", " (tbl_trans_Interview.int_InterviewOutcome = 1) and tbl_trans_Activies.act_assignedTo='" + Session["userid"].ToString() + "'");
                    break;
                case "ElininatedCandidate":
                    DT = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN  tbl_trans_ReferalAgentRecuruitment ON tbl_trans_Activies.act_id = tbl_trans_ReferalAgentRecuruitment.rar_ActivityId INNER JOIN tbl_master_company ON tbl_trans_ReferalAgentRecuruitment.rar_Company = tbl_master_company.cmp_id INNER JOIN   tbl_master_jobResponsibility ON tbl_trans_ReferalAgentRecuruitment.rar_jobresponsibility = tbl_master_jobResponsibility.job_id INNER JOIN   tbl_master_branch ON tbl_trans_ReferalAgentRecuruitment.rar_Branch = tbl_master_branch.branch_id INNER JOIN    tbl_trans_ReferalAgentDetail ON tbl_trans_ReferalAgentRecuruitment.rar_Activityid = tbl_trans_ReferalAgentDetail.rdd_Activityid INNER JOIN  tbl_trans_Interview ON tbl_trans_ReferalAgentDetail.rdd_Activityid = tbl_trans_Interview.int_ActivityId AND   tbl_trans_ReferalAgentDetail.rdd_Id = tbl_trans_Interview.int_InternalId", "tbl_trans_Activies.act_id AS Id, tbl_trans_ReferalAgentDetail.rdd_CandidateName AS Company, tbl_master_branch.branch_description AS Branch,  tbl_master_jobResponsibility.job_responsibility AS Department", " (tbl_trans_Interview.int_InterviewOutcome = 2) and tbl_trans_Activies.act_assignedTo='" + Session["userid"].ToString() + "'");
                    break;
            }
            if (DT != null)
            {
                gridNewCalls.DataSource = DT.DefaultView;
                gridNewCalls.DataBind();
            }
            gridNewCalls.Columns[0].Visible = false;
        }

        #region ICallbackEventHandler Members

        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }

        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] IdList = id.Split('~');
            #region Edit
            if (IdList[0] == "Edit")
            {
                DataTable DT = new DataTable();
                DT = oDBEngine.GetDataTable("tbl_trans_ReferalAgentRecuruitment INNER JOIN    tbl_trans_Activies ON tbl_trans_ReferalAgentRecuruitment.rar_ActivityId = tbl_trans_Activies.act_id INNER JOIN   tbl_master_user ON tbl_trans_Activies.act_assignedBy = tbl_master_user.user_id INNER JOIN    tbl_master_education ON tbl_trans_ReferalAgentRecuruitment.rar_EducationQualification = tbl_master_education.edu_id INNER JOIN   tbl_master_education tbl_master_Professional ON tbl_trans_ReferalAgentRecuruitment.rar_ProfessionalQualification = tbl_master_Professional.edu_id INNER JOIN tbl_master_jobResponsibility ON tbl_trans_ReferalAgentRecuruitment.rar_jobresponsibility=tbl_master_jobResponsibility.job_id", "convert(varchar(11),tbl_trans_Activies.act_createDate,113) AS CreateDate, convert(varchar(11),tbl_trans_Activies.act_scheduledDate,113) AS StartDate,convert(varchar(11),tbl_trans_Activies.act_expectedDate,113) AS EndDate,tbl_trans_ReferalAgentRecuruitment.rar_Requirement AS Vacancies,tbl_master_user.user_name AS assignedby,case tbl_trans_Activies.act_priority when 0 then 'Low' when 1 then 'Normal' when 2 then 'High' when 3 then 'Urgent' when 4 then 'Immediate' end  AS Priority,tbl_trans_ReferalAgentRecuruitment.rar_Experience AS YearofExp,   tbl_master_education.edu_education AS Qualification, tbl_master_Professional.edu_education AS ProfQualification,case  tbl_trans_ReferalAgentRecuruitment.rar_Sex when 0 then 'Male' when 1 then 'Female' End  as Sex,rar_MinAge, rar_MaxAge, rar_PreferedLocality,tbl_master_jobResponsibility.job_responsibility as JobResponsibility,rar_skills,rar_jobresponsibility ", " tbl_trans_Activies.act_id=" + IdList[1]);
                if (DT != null && DT.Rows.Count > 0)
                {
                    lblDateofAllotment.Text = DT.Rows[0]["CreateDate"].ToString();
                    lblSchStartDate.Text = DT.Rows[0]["StartDate"].ToString();
                    lblSchEndDate.Text = DT.Rows[0]["EndDate"].ToString();
                    lblActivityId.Text = IdList[1];
                    lblAllotedby.Text = DT.Rows[0]["assignedby"].ToString();
                    lblPriority.Text = DT.Rows[0]["Priority"].ToString();
                    lblMinExp.Text = DT.Rows[0]["YearofExp"].ToString();
                    lblEduQualification.Text = DT.Rows[0]["Qualification"].ToString();
                    lblProfQualification.Text = DT.Rows[0]["ProfQualification"].ToString();
                    lblVacancies.Text = DT.Rows[0]["Vacancies"].ToString();
                    lblJobResponsibility.Text = DT.Rows[0]["JobResponsibility"].ToString();
                    lblSex.Text = DT.Rows[0]["Sex"].ToString();
                    lblSkills.Text = DT.Rows[0]["rar_skills"].ToString();
                    lblfromAge.Text = DT.Rows[0]["rar_MinAge"].ToString() + " " + "Yrs.";
                    lblToAge.Text = DT.Rows[0]["rar_MaxAge"].ToString() + " " + "Yrs.";
                    lblLocalityPref.Text = DT.Rows[0]["rar_PreferedLocality"].ToString();
                    DataTable DTHOD = new DataTable();
                    DTHOD = oDBEngine.GetDataTable("tbl_master_costCenter INNER JOIN    tbl_master_contact ON tbl_master_costCenter.cost_costCenterHead = tbl_master_contact.cnt_internalId", "ISNULL(tbl_master_contact.cnt_firstName, '') + ' ' + ISNULL(tbl_master_contact.cnt_middleName, '') + ' ' + ISNULL(tbl_master_contact.cnt_lastName, '')   AS Name", "tbl_master_costCenter.cost_id =" + DT.Rows[0]["rar_jobresponsibility"].ToString());
                    if (DTHOD.Rows.Count != 0)
                    {
                        lblHOD.Text = DTHOD.Rows[0][0].ToString();
                    }
                    data = "Edit" + "~" + lblDateofAllotment.Text + "~" + lblSchStartDate.Text + "~" + lblSchEndDate.Text + "~" + lblActivityId.Text + "~" + lblAllotedby.Text + "~" + lblPriority.Text + "~" + lblMinExp.Text + "~" + lblEduQualification.Text + "~" + lblProfQualification.Text + "~" + lblVacancies.Text + "~" + lblJobResponsibility.Text + "~" + lblSex.Text + "~" + lblSkills.Text + "~" + lblfromAge.Text + "~" + lblToAge.Text + "~" + lblLocalityPref.Text + "~" + lblHOD.Text;
                }
                else
                {
                    data = "Edit~N";
                }


            }
            #endregion
            #region
            if (IdList[0] == "Candidate")
            {
                DataTable Dt = (DataTable)Session["DtCandidate"];
                String Expression = "Id='" + IdList[1] + "'";
                DataRow[] Row;
                Row = Dt.Select(Expression);
                data = "Candidate" + "~" + Row[0][0] + "~" + Row[0][1] + "~" + Row[0][3] + "~" + Row[0][5];
            }
            #endregion
        }

        #endregion
        public void BindCandidate(string Actid)
        {
            DataTable DTCandi = new DataTable();
            DataTable DT = new DataTable();
            DataTable DTInterview = new DataTable();
            DT = oDBEngine.GetDataTable("tbl_trans_ReferalAgentDetail", "rdd_Id AS Id, rdd_CandidateName AS Name,rdd_Internalid", "rdd_Activityid = " + Actid);
            DataColumn Col1 = new DataColumn("Id");
            DataColumn Col2 = new DataColumn("Name");
            DataColumn Col3 = new DataColumn("InterviewDate");
            DataColumn Col4 = new DataColumn("Interviewer");
            DataColumn Col5 = new DataColumn("Result");
            DataColumn Col6 = new DataColumn("interviewid");
            DataColumn Col7 = new DataColumn("NextInterview");
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
                    DTInterview = oDBEngine.GetDataTable("tbl_trans_Interview INNER JOIN    tbl_trans_ReferalAgentDetail ON tbl_trans_Interview.int_Id = tbl_trans_ReferalAgentDetail.rdd_Internalid INNER JOIN    tbl_master_user ON tbl_master_user.user_id = tbl_trans_Interview.int_InterviewerName", "int_Id as Intid,convert(varchar(11),int_InterviewDataTime,113) AS InterviewDate, convert(varchar(11),int_NextInterviewData,113) AS NextInterviewDate,   CASE isnull(int_InterviewOutcome,12) when 12 then '' WHEN 0 THEN 'Due' WHEN 1 THEN 'Recruit' WHEN 2 THEN 'Eliminate' WHEN 3 THEN 'Hold' WHEN 4 THEN 'Further Interview '    WHEN 5 THEN 'PostPond Interview' WHEN 6 THEN 'Cancel Interview' END AS Result, tbl_master_user.user_name AS Interviewer,    convert(varchar(11),tbl_trans_Interview.int_NextInterviewData,113) AS NextInterviewDate, tbl_trans_Interview.int_InterviewPlace,     CASE tbl_trans_Interview.int_NextInterviewer WHEN '' THEN '' ELSE     (SELECT     User_name      FROM          tbl_master_user     WHERE      user_id = int_NextInterviewer) END AS NextInterviewer,     (SELECT     branch_description   FROM          tbl_master_branch     WHERE      branch_id = int_NextInterviewPlace) AS NextInterviewPlace ", "int_ActivityId = " + Actid + " And int_InternalId = " + DT.Rows[i][0].ToString());
                    DataRow rownew = DTCandi.NewRow();
                    rownew["Id"] = DT.Rows[i][0];
                    rownew["Name"] = DT.Rows[i][1];
                    rownew["interviewid"] = DT.Rows[i][2];
                    if (DTInterview.Rows.Count != 0)
                    {
                        if (DTInterview.Rows[0][3].ToString() == "Due")
                        {
                            DataTable DTdata = new DataTable();
                            DTdata = oDBEngine.GetDataTable("tbl_trans_Interview INNER JOIN  tbl_trans_ReferalAgentDetail ON tbl_trans_Interview.int_ActivityId = tbl_trans_ReferalAgentDetail.rdd_Activityid AND   tbl_trans_Interview.int_InternalId = tbl_trans_ReferalAgentDetail.rdd_Id INNER JOIN     tbl_master_user ON tbl_master_user.user_id = tbl_trans_Interview.int_InterviewerName ", "int_Id as Intid,convert(varchar(11),int_InterviewDataTime,113) AS InterviewDate,convert(varchar(11),int_NextInterviewData,113) AS NextInterviewDate,CASE int_InterviewOutcome WHEN 0 THEN 'Due' WHEN 1 THEN 'Recruit' WHEN 2 THEN 'Eliminate' WHEN 3 THEN 'Hold' WHEN 4 THEN 'Further Interview '    WHEN 5 THEN 'PostPond Interview' WHEN 6 THEN 'Cancel Interview' END AS Result, tbl_master_user.user_name AS Interviewer,convert(varchar(11),tbl_trans_Interview.int_NextInterviewData,113) AS NextInterviewDate,tbl_trans_Interview.int_InterviewPlace,CASE tbl_trans_Interview.int_NextInterviewer WHEN '' THEN '' ELSE     (SELECT     User_name      FROM          tbl_master_user     WHERE      user_id = int_NextInterviewer) END AS NextInterviewer,(SELECT     branch_description   FROM          tbl_master_branch     WHERE      branch_id = int_NextInterviewPlace) AS NextInterviewPlace", "int_ActivityId = " + Actid + " And int_InternalId = " + DT.Rows[i][0].ToString() + " And Int_id <> " + DTInterview.Rows[0][0] + " Order by intid desc");
                            if (DTdata.Rows.Count != 0)
                            {
                                rownew["InterviewDate"] = DTdata.Rows[0][1].ToString();
                                if (DTdata.Rows[0][2].ToString() != "")
                                {
                                    rownew["NextInterview"] = DTdata.Rows[0][2].ToString();
                                }
                                else
                                    rownew["NextInterview"] = "";

                                rownew["Result"] = DTdata.Rows[0][3].ToString();
                                rownew["Interviewer"] = DTdata.Rows[0][4].ToString();
                                if (DTdata.Rows[0][7].ToString() != "")
                                {
                                    rownew["NextInterviewer"] = DTdata.Rows[0][7].ToString();
                                }
                                else
                                    rownew["NextInterviewer"] = "";
                                if (DTdata.Rows[0][8].ToString() != "")
                                {
                                    rownew["NextIntPlace"] = DTdata.Rows[0][8].ToString();
                                }
                                else
                                    rownew["NextIntPlace"] = "";
                            }
                            else
                            {
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
                            }
                        }
                        else
                        {
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
                        }

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
            Session["DtCandidate"] = DTCandi;
            gridCandidate.DataSource = DTCandi;
            gridCandidate.DataBind();
            gridCandidate.Columns[0].Visible = false;
            gridCandidate.Columns[5].Visible = false;
        }
        protected void gridCandidate_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string param = e.Parameters.ToString();
            BindCandidate(param);
        }
    }
}