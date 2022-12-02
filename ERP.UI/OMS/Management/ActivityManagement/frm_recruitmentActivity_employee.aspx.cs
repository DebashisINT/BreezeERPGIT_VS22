using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;

namespace ERP.OMS.Management.ActivityManagement
{
    public partial class management_activitymanagement_frm_recruitmentActivity_employee : System.Web.UI.Page
    {
        DataTable DT = new DataTable();
        DataTable DT1 = new DataTable();
        DataTable UserInfo = new DataTable();
        DataTable DTRecruit = new DataTable();
        DataTable DTIntervw = new DataTable();
        BusinessLogicLayer.Converter OConvert = new BusinessLogicLayer.Converter();
        clsDropDownList clsdrp = new clsDropDownList();
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
                //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
                pnlCall.Visible = false;
                string userid = HttpContext.Current.Session["userid"].ToString();
                hdUserList.Value = oDBEngine.getChildUserNotColleague(userid, "");

                //________Binding DropDownList)___________//

                string[,] data = oDBEngine.GetFieldValue(" tbl_master_activitytype", " aty_id,aty_activityType", null, 2, "aty_activityType");
                //oDBEngine.AddDataToDropDownList(data, drpActType, 7);
                clsdrp.AddDataToDropDownList(data, drpActType, 7);
                drpActType.Enabled = false;
                data = oDBEngine.GetFieldValue(" tbl_master_user", " user_id,user_name", " user_id in(" + hdUserList.Value + ")", 2, "user_name");
                //oDBEngine.AddDataToDropDownList(data, drpUserWork, 0);
                clsdrp.AddDataToDropDownList(data, drpUserWork, 0);
                data = oDBEngine.GetFieldValue(" tbl_master_company", " cmp_id,cmp_Name", null, 2, "cmp_Name");
                // oDBEngine.AddDataToDropDownList(data, drpCompany, 0);
                clsdrp.AddDataToDropDownList(data, drpCompany, 0);
                data = oDBEngine.GetFieldValue(" tbl_master_costCenter", " cost_id,cost_description", " cost_costCenterType='Department'", 2, "cost_description");
                //oDBEngine.AddDataToDropDownList(data, drpDepartment, 0);
                clsdrp.AddDataToDropDownList(data, drpDepartment, 0);
                data = oDBEngine.GetFieldValue(" tbl_master_branch", " branch_id,branch_description", null, 2, "branch_description");
                // oDBEngine.AddDataToDropDownList(data, drpBranch, 0);
                clsdrp.AddDataToDropDownList(data, drpBranch, 0);
                data = oDBEngine.GetFieldValue(" tbl_master_jobResponsibility", " job_id,job_responsibility", null, 2, "job_responsibility");
                // oDBEngine.AddDataToDropDownList(data, drpJobResponsbility, 0);
                clsdrp.AddDataToDropDownList(data, drpJobResponsbility, 0);
                data = oDBEngine.GetFieldValue(" tbl_master_designation", " deg_id,deg_designation", null, 2, "deg_designation");
                //oDBEngine.AddDataToDropDownList(data, drpDesignation, 0);
                clsdrp.AddDataToDropDownList(data, drpDesignation, 0);
                data = oDBEngine.GetFieldValue(" tbl_master_education", " edu_id,edu_education", null, 2, "edu_education");
                //oDBEngine.AddDataToDropDownList(data, drpEducation, 0);
                clsdrp.AddDataToDropDownList(data, drpEducation, 0);
                data = oDBEngine.GetFieldValue(" tbl_master_education", " edu_id,edu_education", null, 2, "edu_education");
                //oDBEngine.AddDataToDropDownList(data, drpProfessional, 0);
                clsdrp.AddDataToDropDownList(data, drpProfessional, 0);
                data = oDBEngine.GetFieldValue(" tbl_master_industry", " ind_id,ind_industry", null, 2, "ind_industry");
                //oDBEngine.AddDataToDropDownList(data, drpIndustry, 0);
                clsdrp.AddDataToDropDownList(data, drpIndustry, 0);

                DateTime dt = oDBEngine.GetDate();

                txtStartDate.EditFormatString = OConvert.GetDateFormat("DateTime");
                txtEndDate.EditFormatString = OConvert.GetDateFormat("DateTime");
                txtJoinDate.EditFormatString = OConvert.GetDateFormat("Date");
                txtStartDate.Attributes.Add("readonly", "true");
                txtEndDate.Attributes.Add("readonly", "true");
                txtJoinDate.Attributes.Add("readonly", "true");


                DT1 = oDBEngine.GetDataTable("tbl_trans_Activies INNER JOIN  tbl_master_user ON tbl_trans_Activies.act_assignedTo = tbl_master_user.user_id INNER JOIN   tbl_trans_Recruitment ON tbl_trans_Activies.act_id = tbl_trans_Recruitment.rd_ActivityId", "tbl_master_user.user_name AS UserName, tbl_master_user.user_id as UserId, tbl_trans_Activies.act_scheduledDate AS ScheduleDate,  tbl_trans_Activies.act_scheduledTime AS ScheduleTime, tbl_trans_Activies.act_expectedDate AS EndDate,  tbl_trans_Activies.act_expectedTime AS EndTime, tbl_trans_Activies.act_id AS Activityid", " tbl_trans_Activies.act_activityType =7 And (tbl_trans_Activies.act_assignedBy IN(" + hdUserList.Value + ") or tbl_trans_Activies.act_assignedTo =" + HttpContext.Current.Session["userid"] + ")");
                if (DT1.Rows.Count != 0)
                {
                    Converter oconverter = new Converter();

                    DataColumn col1 = new DataColumn("UserId");
                    DataColumn col2 = new DataColumn("SNo");
                    DataColumn col3 = new DataColumn("User");
                    DataColumn col4 = new DataColumn("Pending Acttivity");
                    DataColumn col5 = new DataColumn("Scheduled End Date");
                    DataColumn col6 = new DataColumn("Expected End Date");
                    DataColumn col7 = new DataColumn("Vacancies");
                    DataColumn col8 = new DataColumn("Candidate Screened");
                    DataColumn col9 = new DataColumn("Interviewed");
                    DataColumn col10 = new DataColumn("Selected");
                    DataColumn col11 = new DataColumn("Eliminated");
                    DataColumn col12 = new DataColumn("Pending Call");
                    DT.Columns.Add(col1);
                    DT.Columns.Add(col2);
                    DT.Columns.Add(col3);
                    DT.Columns.Add(col4);
                    DT.Columns.Add(col5);
                    DT.Columns.Add(col6);
                    DT.Columns.Add(col7);
                    DT.Columns.Add(col8);
                    DT.Columns.Add(col9);
                    DT.Columns.Add(col10);
                    DT.Columns.Add(col11);
                    DT.Columns.Add(col12);

                    UserInfo = DT.Clone();
                    int count = 0;
                    Boolean bVal = false;
                    int CandidateCount = 0;
                    int interviewCount = 0;
                    int Selected = 0;
                    string schEndDate = "1/1/1970 00:00";
                    string userName = "";
                    string userId = "";
                    int Vacancies = 0;
                    int actNO = 0;
                    int eliminated = 0;
                    int sNo = 0;
                    for (int i = 0; i < DT1.Rows.Count; i++)
                    {
                        userId = DT1.Rows[i][1].ToString();
                        count = 0;
                        actNO = 0;
                        CandidateCount = 0;
                        interviewCount = 0;
                        schEndDate = "1/1/1970 00:00";
                        Selected = 0;
                        bVal = false;
                        CandidateCount = 0;
                        userName = DT1.Rows[i][0].ToString();
                        Vacancies = 0;
                        eliminated = 0;
                        int tempCandidateCount = 0;
                        int tempinterviewCount = 0;
                        int tempSelected = 0;
                        int tempVacancies = 0;
                        int tempeliminated = 0;
                        do
                        {

                            if (DateTime.Parse(DT1.Rows[i][2].ToString()) > DateTime.Parse(schEndDate))
                                //schEndDate = oconverter.DateConverter_d_m_y(DT1.Rows[i][2].ToString(),"dd/mm/yyyy");
                                schEndDate = DT1.Rows[i][2].ToString();
                            DTRecruit = oDBEngine.GetDataTable("tbl_trans_Recruitment", "rd_Vacancies", " rd_ActivityId =" + DT1.Rows[i][6].ToString());
                            if (DTRecruit.Rows.Count != 0)
                                tempVacancies += int.Parse(DTRecruit.Rows[0][0].ToString());
                            DTRecruit = oDBEngine.GetDataTable(" tbl_trans_RecruitmentDetail", " rde_Id, isnull(rde_EmployementConfirmed,3) as Confirmed", " rde_ActivityId =" + DT1.Rows[i][6].ToString());
                            if (DTRecruit.Rows.Count > 0)
                            {
                                tempCandidateCount += DTRecruit.Rows.Count;
                                for (int j = 0; j < DTRecruit.Rows.Count; j++)
                                {
                                    if (int.Parse(DTRecruit.Rows[j][1].ToString()) == 1)
                                        tempSelected += 1;
                                    if (int.Parse(DTRecruit.Rows[j][1].ToString()) == 0)
                                        tempeliminated += 1;
                                    DTIntervw = oDBEngine.GetDataTable(" tbl_trans_Interview", " int_Id, int_InterviewOutcome", " int_InternalId =" + DTRecruit.Rows[j][0].ToString() + " And int_InterviewOutcome IS NOT NULL");
                                    if (DTIntervw.Rows.Count > 0)
                                        tempinterviewCount += 1;
                                }
                            }

                            if (tempSelected != tempVacancies)
                            {
                                actNO += 1;
                                CandidateCount = tempCandidateCount;
                                interviewCount = tempinterviewCount;
                                Selected = tempSelected;
                                Vacancies = tempVacancies;
                                eliminated = tempeliminated;
                                bVal = true;
                            }

                            i += 1;
                            if (i > DT1.Rows.Count - 1)
                                break;
                        }
                        while (userId == DT1.Rows[i][1].ToString());
                        i = i - 1;
                        if (bVal == true)
                        {
                            sNo += 1;
                            //DataRow row1 = UserInfo.NewRow();
                            DataRow row1 = DT.NewRow();
                            row1["UserId"] = userId;
                            row1["SNo"] = sNo;
                            row1["User"] = userName;
                            row1["Pending Acttivity"] = actNO;
                            row1["Expected End Date"] = schEndDate;
                            row1["Scheduled End Date"] = schEndDate;
                            row1["Vacancies"] = Vacancies;
                            row1["Candidate Screened"] = CandidateCount;
                            row1["Interviewed"] = interviewCount;
                            row1["Selected"] = Selected;
                            row1["Eliminated"] = eliminated;
                            row1["Pending Call"] = Vacancies - Selected;
                            //UserInfo.Rows.Add(row1);
                            DT.Rows.Add(row1);
                        }
                    }

                }

                //if (UserInfo.Rows.Count != 0)
                if (DT.Rows.Count != 0)
                {
                    //GridUserInfo.DataSource = UserInfo;
                    GridNewActivity.DataSource = DT.DefaultView;
                    GridNewActivity.DataBind();
                }

                DT = oDBEngine.GetDataTable(" tbl_trans_RecruitmentDetail INNER JOIN    tbl_trans_Recruitment ON tbl_trans_RecruitmentDetail.rde_Activityid = tbl_trans_Recruitment.rd_ActivityId INNER JOIN   tbl_master_jobResponsibility ON tbl_trans_Recruitment.rd_JobResponsibility = tbl_master_jobResponsibility.job_id INNER JOIN   tbl_master_branch ON tbl_trans_Recruitment.rd_Branch = tbl_master_branch.branch_id INNER JOIN   tbl_master_costCenter ON tbl_trans_Recruitment.rd_Department = tbl_master_costCenter.cost_id INNER JOIN    tbl_trans_Activies ON tbl_trans_Recruitment.rd_ActivityId = tbl_trans_Activies.act_id", " tbl_trans_RecruitmentDetail.rde_Name AS Name, tbl_master_jobResponsibility.job_responsibility AS JobResponsibility,   tbl_master_costCenter.cost_description AS Department, tbl_master_branch.branch_description AS Branch,    tbl_trans_Recruitment.rd_JoinDate AS JoinDate, tbl_trans_RecruitmentDetail.rde_OfferLetterIssueDate AS OfferLetterDate,    tbl_trans_RecruitmentDetail.rde_EmploymentSignDate AS AggrementDate, tbl_trans_RecruitmentDetail.rde_PhoneNo AS PhoneNo,    tbl_trans_RecruitmentDetail.rde_Email AS Email , tbl_trans_RecruitmentDetail.rde_Id AS Id ", " ((tbl_trans_Activies.act_assignedBy in( " + hdUserList.Value + ")) or (tbl_trans_Activies.act_assignedTo in (" + hdUserList.Value + "))) AND (tbl_trans_Activies.act_actualEndDate IS NULL) And tbl_trans_RecruitmentDetail.rde_EmployementConfirmed = 1");
                if (DT.Rows.Count != 0)
                {
                    grdSelectedCandidate.DataSource = DT;
                    grdSelectedCandidate.DataBind();
                }
                //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (txtVacancies.Text == "" || txtExp.Text == "" || txtMin.Text == "" || txtMax.Text == "" || txtMinAge.Text == "" || txtMaxAge.Text == "" || txtStartDate.Value == null || txtEndDate.Value == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "create", "<script language='javascript'>alert('Please Fill The details!');</script>");
                return;
            }
            else
            {
                string act_no = oDBEngine.GetInternalId("RE", " tbl_trans_Activies", " act_activityNo", " act_activityNo");
                int NoOfRowsEffected = oDBEngine.InsurtFieldValue(" tbl_trans_Activies", " act_branchId,act_activityType,act_description,act_assignedBy,act_assignedTo,act_createDate,act_contactlead,act_scheduledDate,act_expectedDate,act_activityno,act_priority,act_instruction", HttpContext.Current.Session["userid"] + ",'" + drpActType.SelectedValue + "','" + txtDesc.Text + "'," + HttpContext.Current.Session["userid"] + "," + drpUserWork.SelectedValue + ",'" + oDBEngine.GetDate() + "','','" + txtStartDate.Value.ToString() + "','" + txtEndDate.Value.ToString() + "','" + act_no + "','" + drpPriority.SelectedValue + "','" + txtInstNote.Text + "'");
                string[,] lastID = oDBEngine.GetFieldValue(" tbl_trans_Activies", " act_id", " act_activityno='" + act_no + "' and act_branchId=" + HttpContext.Current.Session["userid"], 1);
                string fields = "rd_ActivityId,rd_Company,rd_Department,rd_Branch,rd_JobResponsibility,rd_Designation,rd_Vacancies,rd_YearofExp,rd_OfferedCTCmin,rd_OfferedCTCmax,rd_EduQualification,rd_ProfQualification,rd_Skilles,rd_Sex,rd_MinAge,rd_MaxAge,rd_PreferedLocality,rd_ExtActivity,rd_Hobbies,rd_PreferedIndustry,rd_JoinDate";
                string values = int.Parse(lastID[0, 0].ToString()) + "," + drpCompany.SelectedValue + "," + drpDepartment.SelectedValue + "," + drpBranch.SelectedValue + "," + drpJobResponsbility.SelectedValue + "," + drpDesignation.SelectedValue + "," + txtVacancies.Text + "," + txtExp.Text + "," + txtMin.Text + "," + txtMax.Text + "," + drpEducation.SelectedValue + "," + drpProfessional.SelectedValue + ",'" + txtSkills.Text + "'," + drpSex.SelectedValue + "," + txtMinAge.Text + "," + txtMaxAge.Text + ",'" + txtLocality.Text + "','" + txtCurAct.Text + "','" + txtHobbies.Text + "'," + drpIndustry.SelectedValue + ",'" + txtJoinDate.Value.ToString() + "'";
                NoOfRowsEffected = oDBEngine.InsurtFieldValue("tbl_trans_Recruitment", fields, values);

                //__sending Message & Reminder
                oDBEngine.messageTableUpdate(drpUserWork.SelectedValue, "0", "Recruitment", txtStartDate.Text, txtEndDate.Text, drpPriority.SelectedItem.Text, txtInstNote.Text, lastID[0, 0].ToString(), "activity");
                //string MsgContent = " Hello  " + drpUserWork.SelectedItem.Text + " ! " + HttpContext.Current.Session["username"].ToString() + " has allotted a New [ Recruitment ] type of activity to you, to be started by " + txtStartDate.Text + " and finished by " + txtEndDate.Text + " with " + drpPriority.SelectedItem.Text + " priority. Have a great day !!";
                //if (txtInstNote.Text != "")
                //{
                //    MsgContent += " Note :-" + txtInstNote.Text + "";
                //}
                //fields = "[rem_createUser],[rem_createDate],[rem_targetUser],[rem_startDate],[rem_endDate],[rem_reminderContent],[rem_displayTricker],[rem_actionTaken],[rem_sourceid],[CreateDate],[CreateUser]";
                //values = "'0',getdate(),'" + drpUserWork.SelectedItem.Value + "','" + txtStartDate.Value.ToString() + "','" + txtEndDate.Value.ToString() + "','" + MsgContent + "','1','0','" + lastID[0, 0].ToString() + "',getdate(),'" + HttpContext.Current.Session["userid"].ToString() + "'";
                //oDBEngine.InsurtFieldValue(" tbl_trans_reminder ", fields, values);
                Response.Redirect("frm_recruitmentActivity_employee.aspx", false);
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            GridUserInfo.ActiveTabIndex = 0;
        }
        protected void btnGenerateCode_Click(object sender, EventArgs e)
        {
            Response.Write("<script language='javaScript'> alert('No Logic For This Button!!') </script>");
        }
        protected void btnCancelOffer_Click(object sender, EventArgs e)
        {
            if (grdSelectedCandidate.Rows.Count != 0)
            {
                for (int i = 0; i < grdSelectedCandidate.Rows.Count; i++)
                {
                    GridViewRow rownew = grdSelectedCandidate.Rows[i];
                    CheckBox chkbox = new CheckBox();
                    chkbox = (CheckBox)rownew.FindControl("chk1");
                    if (chkbox.Checked)
                    {
                        Label lblId = new Label();
                        lblId = (Label)rownew.FindControl("lblId");
                        //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
                        int NoOfRowsEffected = oDBEngine.SetFieldValue(" tbl_trans_RecruitmentDetail", " rde_EmployementConfirmed=0", " rde_Id=" + lblId.Text);
                        break;
                    }
                    else if (i == grdSelectedCandidate.Rows.Count - 1)
                        Response.Write("<script language='javaScript'> alert('Select atleast One Candidate!!') </script>");

                }
            }
        }
        protected void grdDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)    //___This condition is for filtering header and footer, application will done on Rows only___//
            {
                CheckBox chkbox = new CheckBox();
                chkbox = (CheckBox)e.Row.FindControl("chk1");
                Label lblId = new Label();
                lblId = (Label)e.Row.FindControl("lblId");
                chkbox.Attributes.Add("onclick", "javascript:chkclicked(this,'" + lblId.Text + "');");
            }
        }
        protected void GridUserInfo_ActiveTabChanged(object source, DevExpress.Web.TabControlEventArgs e)
        {

        }
        protected void grdSelectedCandidate_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}