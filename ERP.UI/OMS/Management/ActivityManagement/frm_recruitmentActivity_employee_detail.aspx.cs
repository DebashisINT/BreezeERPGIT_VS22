using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;

namespace ERP.OMS.Management.ActivityManagement
{
    public partial class management_activitymanagement_frm_recruitmentActivity_employee_detail : System.Web.UI.Page
    {
        BusinessLogicLayer.Converter OConvert = new BusinessLogicLayer.Converter();
        public string pageAccess = "";
        // DBEngine oDBEngine = new DBEngine(string.Empty);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        clsDropDownList clsdrp = new clsDropDownList();

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
                hdID.Value = Request.QueryString["id"].ToString();
                pnlCall.Visible = false;
                pnlActivityDetail.Visible = false;
                FillGrid();
            }
        }

        private void FillGrid()
        {
            // DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            DataTable DT = oDBEngine.GetDataTable(" tbl_trans_Activies INNER JOIN  tbl_master_user ON tbl_trans_Activies.act_assignedTo = tbl_master_user.user_id INNER JOIN   tbl_trans_Recruitment ON tbl_trans_Activies.act_id = tbl_trans_Recruitment.rd_ActivityId", " tbl_master_user.user_name AS UserName, tbl_master_user.user_id as UserId, tbl_trans_Activies.act_scheduledDate AS ScheduleDate,  tbl_trans_Activies.act_scheduledTime AS ScheduleTime, tbl_trans_Activies.act_expectedDate AS EndDate,  tbl_trans_Activies.act_expectedTime AS EndTime, tbl_trans_Activies.act_id AS Activityid,tbl_trans_Activies.act_activityNo as ActNo", " tbl_trans_Activies.act_activityType =7 And tbl_master_user.user_id =" + hdID.Value);

            string Actid = "";
            lblname.Text = "Name: " + DT.Rows[0][0].ToString();
            hdUserNmae.Value = DT.Rows[0][0].ToString();
            DataTable DtUserInfo = new DataTable();
            DataColumn colNo = new DataColumn("SNO");
            DataColumn colActNO = new DataColumn("Activity NO");
            DataColumn colVacancies = new DataColumn("Vacancy");
            DataColumn colInterviewed = new DataColumn("Interviewed");
            DataColumn colSelected = new DataColumn("Selected");
            DataColumn colPending = new DataColumn("Pending");
            DataColumn colActivityNo = new DataColumn("Activity");
            DtUserInfo.Columns.Add(colNo);
            DtUserInfo.Columns.Add(colActNO);
            DtUserInfo.Columns.Add(colVacancies);
            DtUserInfo.Columns.Add(colInterviewed);
            DtUserInfo.Columns.Add(colSelected);
            DtUserInfo.Columns.Add(colPending);
            DtUserInfo.Columns.Add(colActivityNo);
            DataRow rownew = DtUserInfo.NewRow();

            int sNo = 0;
            int count = 0;
            int CountVacancy = 0;
            int CountInterview = 0;
            int CountPending = 0;
            int CountSelected = 0;
            Boolean bVal = false;
            string ActivityNo = "";

            if (DT.Rows.Count > 0)
            {
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    Actid = DT.Rows[i][6].ToString();
                    CountPending = 0;
                    count = 0;
                    CountVacancy = 0;
                    CountInterview = 0;
                    CountSelected = 0;

                    do
                    {
                        int tempinterviewCount = 0;
                        int tempSelected = 0;
                        int tempVacancies = 0;
                        ActivityNo = DT.Rows[i][7].ToString();
                        DataTable DTrecruite = new DataTable();
                        DataTable DTinterview = new DataTable();
                        DTrecruite = oDBEngine.GetDataTable(" tbl_trans_Recruitment", " rd_Vacancies", " rd_ActivityId ='" + DT.Rows[i][6].ToString() + "'");
                        if (DTrecruite.Rows.Count > 0)
                            tempVacancies = int.Parse(DTrecruite.Rows[0][0].ToString());
                        DTrecruite = oDBEngine.GetDataTable(" tbl_trans_RecruitmentDetail", " rde_Id, isnull(rde_EmployementConfirmed,3) as Confirmed", " rde_ActivityId ='" + DT.Rows[i][6].ToString() + "'");
                        if (DTrecruite.Rows.Count > 0)
                        {
                            for (int j = 0; j < DTrecruite.Rows.Count; j++)
                            {
                                if (int.Parse(DTrecruite.Rows[j][1].ToString()) == 1)
                                    tempSelected += 1;
                                DTinterview = oDBEngine.GetDataTable(" tbl_trans_Interview", " int_Id, int_InterviewOutcome", " int_InternalId =" + DTrecruite.Rows[j][0].ToString());
                                if (DTinterview.Rows.Count > 0)
                                    tempinterviewCount += 1;

                            }
                        }
                        if (tempSelected != tempVacancies)
                        {
                            CountInterview = tempinterviewCount;
                            CountSelected = tempSelected;
                            CountVacancy = tempVacancies;
                            bVal = true;
                        }
                        i += 1;
                        if (i > DT.Rows.Count - 1)
                            break;
                    }
                    while (Actid == DT.Rows[i][6].ToString());
                    i = i - 1;
                    DataRow row1 = DtUserInfo.NewRow();
                    sNo += 1;
                    row1["SNO"] = sNo;
                    row1["Activity No"] = Actid;
                    row1["Vacancy"] = CountVacancy;
                    row1["Interviewed"] = CountInterview;
                    row1["Selected"] = CountSelected;
                    row1["Pending"] = CountVacancy - CountSelected;
                    row1["Activity"] = ActivityNo;
                    DtUserInfo.Rows.Add(row1);
                }
            }
            if (DtUserInfo.Rows.Count > 0)
            {
                grdDetail.DataSource = DtUserInfo;
                grdDetail.DataBind();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Converter oconverter = new Converter();
            //txtStartDate.Text = oconverter.DateConverter(txtStartDate.Text, "mm/dd/yyyy hh:mm");
            //txtEndDate.Text = oconverter.DateConverter(txtEndDate.Text, "mm/dd/yyyy hh:mm");
            //txtJoinDate.Text = oconverter.DateConverter(txtJoinDate.Text, "mm/dd/yyyy hh:mm");
            string fieldsWvalues = " act_branchId=" + HttpContext.Current.Session["userbranchID"] + ",act_activityType='" + drpActType.SelectedItem.Value + "',act_description='" + txtDesc.Text + "',act_instruction='" + txtInstNote.Text + "',act_assignedBy=" + HttpContext.Current.Session["userid"] + ",act_assignedTo=" + drpUserWork.SelectedItem.Value + ",act_scheduledDate='" + txtStartDate.Value.ToString() + "',act_expectedDate='" + txtEndDate.Value.ToString() + "',act_priority=" + drpPriority.SelectedItem.Value + " ";
            //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            int noOfRowsEffected = oDBEngine.SetFieldValue(" tbl_trans_Activies", fieldsWvalues, " act_id=" + hdCandiId.Value);
            fieldsWvalues = " rd_Company=" + drpCompany.SelectedItem.Value + ",rd_Department=" + drpDepartment.SelectedItem.Value + ", rd_Branch=" + drpBranch.SelectedItem.Value + ",rd_JobResponsibility=" + drpJobResponsbility.SelectedValue + ",rd_Designation=" + drpDesignation.SelectedValue + ",rd_Vacancies='" + txtVacancies.Text + "',rd_YearofExp='" + txtExp.Text + "',rd_OfferedCTCmin='" + txtMin.Text + "',rd_OfferedCTCmax='" + txtMax.Text + "',rd_EduQualification=" + drpEducation.SelectedValue + ",rd_ProfQualification=" + drpProfessional.SelectedValue + ",rd_Skilles='" + txtSkills.Text + "',rd_Sex=" + drpSex.SelectedValue + ",rd_MinAge=" + txtMinAge.Text + ",rd_MaxAge=" + txtMaxAge.Text + ",rd_PreferedLocality='" + txtLocality.Text + "',rd_ExtActivity='" + txtCurAct.Text + "',rd_Hobbies='" + txtHobbies.Text + "',rd_PreferedIndustry=" + drpIndustry.SelectedValue + ",rd_JoinDate='" + txtJoinDate.Value.ToString() + "' ";
            noOfRowsEffected = oDBEngine.SetFieldValue(" tbl_trans_Recruitment", fieldsWvalues, " rd_ActivityId=" + hdCandiId.Value);
            DeselectCheckBox();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            DeselectCheckBox();
        }
        protected void DeselectCheckBox()
        {
            pnlCall.Visible = false;
            //_______to make check boxed uncheked________//
            if (grdDetail.Rows.Count > 0)
            {
                for (int i = 0; i < grdDetail.Rows.Count; i++)
                {
                    GridViewRow newrow = grdDetail.Rows[i];
                    CheckBox chkbx = new CheckBox();
                    chkbx = (CheckBox)newrow.FindControl("chkDetail");
                    if (chkbx.Checked)
                    {
                        chkbx.Checked = false;
                    }
                }
            }
        }
        protected void btnModify_Click(object sender, EventArgs e)
        {
            CallFillDataToForm();
        }
        private void CallFillDataToForm()
        {

            if (grdDetail.Rows.Count > 0)
            {
                for (int i = 0; i < grdDetail.Rows.Count; i++)
                {
                    GridViewRow newrow = grdDetail.Rows[i];
                    CheckBox chkbx = new CheckBox();
                    chkbx = (CheckBox)newrow.FindControl("chkDetail");
                    if (chkbx.Checked)
                    {
                        Label CandID = (Label)newrow.FindControl("lblActivityNo");
                        hdCandiId.Value = CandID.Text;
                        FillDataToForm(CandID.Text);
                        break;
                    }
                }
            }
        }
        private void FillDataToForm(string CandId)
        {
            pnlCall.Visible = true;
            pnlActivityDetail.Visible = false;
            //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            DataTable DT = oDBEngine.GetDataTable(" tbl_trans_Recruitment INNER JOIN   tbl_trans_Activies ON tbl_trans_Recruitment.rd_ActivityId = tbl_trans_Activies.act_id", " tbl_trans_Recruitment.rd_Company AS Company, tbl_trans_Recruitment.rd_Department AS Department, tbl_trans_Recruitment.rd_Branch AS Branch,  tbl_trans_Recruitment.rd_JobResponsibility AS JobResponsibility, tbl_trans_Recruitment.rd_Designation AS Designation,  tbl_trans_Recruitment.rd_Vacancies AS Vacancies, tbl_trans_Recruitment.rd_YearofExp AS YrsofExp,   tbl_trans_Recruitment.rd_OfferedCTCmin AS MinCTC, tbl_trans_Recruitment.rd_OfferedCTCmax AS MaxCTC,   tbl_trans_Recruitment.rd_EduQualification AS EduQualification, tbl_trans_Recruitment.rd_ProfQualification AS ProfQualification,   tbl_trans_Recruitment.rd_Skilles AS Skills, tbl_trans_Recruitment.rd_Sex AS Sex, tbl_trans_Recruitment.rd_MinAge AS MinAGe,  tbl_trans_Recruitment.rd_MaxAge AS MaxAge, tbl_trans_Recruitment.rd_PreferedLocality AS Locality,   tbl_trans_Recruitment.rd_ExtActivity AS ExtActivity, tbl_trans_Recruitment.rd_Hobbies AS Hobbies,   tbl_trans_Recruitment.rd_PreferedIndustry AS Industry, tbl_trans_Recruitment.rd_JoinDate AS JoinDate,   tbl_trans_Activies.act_activityType AS ActivityType, tbl_trans_Activies.act_description AS Description,  tbl_trans_Activies.act_assignedTo AS Assignedby, tbl_trans_Activies.act_priority AS Priority, tbl_trans_Activies.act_scheduledDate AS SchDate,   tbl_trans_Activies.act_scheduledTime AS SchTime, tbl_trans_Activies.act_expectedDate AS ExpectedDate,  tbl_trans_Activies.act_expectedTime AS ExpectedTime, tbl_trans_Activies.act_instruction AS Instruction", " tbl_trans_Activies.act_id =" + CandId);
            if (DT.Rows.Count > 0)
            {
                string[,] data = oDBEngine.GetFieldValue(" tbl_master_activitytype", " aty_id,aty_activityType", null, 2, "aty_activityType");
                //oDBEngine.AddDataToDropDownList(data, drpActType, 7);
                clsdrp.AddDataToDropDownList(data, drpActType, 7);
                drpActType.Enabled = false;
                data = oDBEngine.GetFieldValue(" tbl_master_user", " user_id,user_name", null, 2, "user_name");
                //oDBEngine.AddDataToDropDownList(data, drpUserWork, int.Parse(DT.Rows[0][22].ToString()));
                clsdrp.AddDataToDropDownList(data, drpUserWork, int.Parse(DT.Rows[0][22].ToString()));

                data = oDBEngine.GetFieldValue(" tbl_master_company", " cmp_id,cmp_Name", null, 2, "cmp_Name");
                //oDBEngine.AddDataToDropDownList(data, drpCompany, int.Parse(DT.Rows[0][0].ToString()));
                clsdrp.AddDataToDropDownList(data, drpCompany, int.Parse(DT.Rows[0][0].ToString()));

                data = oDBEngine.GetFieldValue(" tbl_master_costCenter", " cost_id,cost_description", " cost_costCenterType='Department'", 2, "cost_description");
                //oDBEngine.AddDataToDropDownList(data, drpDepartment, int.Parse(DT.Rows[0][1].ToString()));
                clsdrp.AddDataToDropDownList(data, drpDepartment, int.Parse(DT.Rows[0][1].ToString()));

                data = oDBEngine.GetFieldValue(" tbl_master_branch", " branch_id,branch_description", null, 2, "branch_description");
                //oDBEngine.AddDataToDropDownList(data, drpBranch, int.Parse(DT.Rows[0][2].ToString()));
                clsdrp.AddDataToDropDownList(data, drpBranch, int.Parse(DT.Rows[0][2].ToString()));

                data = oDBEngine.GetFieldValue(" tbl_master_jobResponsibility", " job_id,job_responsibility", null, 2, "job_responsibility");
                //oDBEngine.AddDataToDropDownList(data, drpJobResponsbility, int.Parse(DT.Rows[0][3].ToString()));
                clsdrp.AddDataToDropDownList(data, drpJobResponsbility, int.Parse(DT.Rows[0][3].ToString()));

                data = oDBEngine.GetFieldValue(" tbl_master_designation", " deg_id,deg_designation", null, 2, "deg_designation");
                //oDBEngine.AddDataToDropDownList(data, drpDesignation, int.Parse(DT.Rows[0][4].ToString()));
                clsdrp.AddDataToDropDownList(data, drpDesignation, int.Parse(DT.Rows[0][4].ToString()));

                data = oDBEngine.GetFieldValue(" tbl_master_education", " edu_id,edu_education", null, 2, "edu_education");
                // oDBEngine.AddDataToDropDownList(data, drpEducation, int.Parse(DT.Rows[0][9].ToString()));
                clsdrp.AddDataToDropDownList(data, drpEducation, int.Parse(DT.Rows[0][9].ToString()));

                data = oDBEngine.GetFieldValue(" tbl_master_education", " edu_id,edu_education", null, 2, "edu_education");
                // oDBEngine.AddDataToDropDownList(data, drpProfessional, int.Parse(DT.Rows[0][10].ToString()));
                clsdrp.AddDataToDropDownList(data, drpProfessional, int.Parse(DT.Rows[0][10].ToString()));

                data = oDBEngine.GetFieldValue(" tbl_master_industry", " ind_id,ind_industry", null, 2, "ind_industry");
                //oDBEngine.AddDataToDropDownList(data, drpIndustry, int.Parse(DT.Rows[0][18].ToString()));
                clsdrp.AddDataToDropDownList(data, drpIndustry, int.Parse(DT.Rows[0][18].ToString()));


                DateTime dt = oDBEngine.GetDate();
                //txtStartDate.Attributes.Add("onfocus", "displayCalendar(txtStartDate,'dd/mm/yyyy hh:ii',this,true,null,'0','0')");
                //imgStartDate.Attributes.Add("OnClick", "displayCalendar(txtStartDate,'dd/mm/yyyy hh:ii',this,true,null,'0','0')");
                //txtEndDate.Attributes.Add("onfocus", "displayCalendar(txtEndDate,'dd/mm/yyyy hh:ii',this,true,null,'0','0')");
                //imgEndDate.Attributes.Add("OnClick", "displayCalendar(txtEndDate,'dd/mm/yyyy hh:ii',this,true,null,'0','0')");
                //txtJoinDate.Attributes.Add("onfocus", "displayCalendar(txtJoinDate,'dd/mm/yyyy hh:ii',this,true,null,'0','0')");
                //imgJoinDate.Attributes.Add("OnClick", "displayCalendar(txtJoinDate,'dd/mm/yyyy hh:ii',this,true,null,'0','0')");
                txtStartDate.EditFormatString = OConvert.GetDateFormat("Date");
                txtEndDate.EditFormatString = OConvert.GetDateFormat("Date");
                txtJoinDate.EditFormatString = OConvert.GetDateFormat("Date");
                txtStartDate.Attributes.Add("readonly", "true");
                txtEndDate.Attributes.Add("readonly", "true");
                txtJoinDate.Attributes.Add("readonly", "true");
                txtVacancies.Text = DT.Rows[0][5].ToString();
                txtExp.Text = DT.Rows[0][6].ToString();
                txtMin.Text = DT.Rows[0][7].ToString();
                txtMax.Text = DT.Rows[0][8].ToString();
                txtSkills.Text = DT.Rows[0][11].ToString();
                txtMinAge.Text = DT.Rows[0][13].ToString();
                txtMaxAge.Text = DT.Rows[0][14].ToString();
                txtLocality.Text = DT.Rows[0][15].ToString();
                txtCurAct.Text = DT.Rows[0][16].ToString();
                txtHobbies.Text = DT.Rows[0][17].ToString();
                txtDesc.Text = DT.Rows[0][21].ToString();
                txtInstNote.Text = DT.Rows[0][28].ToString();
                ////Converter oconverter=new Converter();
                //txtStartDate.Text = oconverter.DateConverter_d_m_y(DT.Rows[0][24].ToString());
                //txtEndDate.Text = oconverter.DateConverter_d_m_y(DT.Rows[0][26].ToString());
                //txtJoinDate.Text = oconverter.DateConverter_d_m_y(DT.Rows[0][19].ToString());
                txtStartDate.Value = Convert.ToDateTime(DT.Rows[0][24].ToString());
                txtEndDate.Value = Convert.ToDateTime(DT.Rows[0][26].ToString());
                txtJoinDate.Value = Convert.ToDateTime(DT.Rows[0][19].ToString());



            }
        }
        protected void btnReassign_Click(object sender, EventArgs e)
        {
            CallFillDataToForm();
        }
        protected void btnReschedule_Click(object sender, EventArgs e)
        {
            CallFillDataToForm();
        }
        protected void btnShowDetail_Click(object sender, EventArgs e)
        {
            pnlActivityDetail.Visible = true;
            pnlCall.Visible = false;

            if (grdDetail.Rows.Count > 0)
            {
                for (int i = 0; i < grdDetail.Rows.Count; i++)
                {
                    GridViewRow newrow = grdDetail.Rows[i];
                    CheckBox chkbx = new CheckBox();
                    chkbx = (CheckBox)newrow.FindControl("chkDetail");
                    if (chkbx.Checked)
                    {
                        DBEngine oDBEngine = new DBEngine();

                        //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);

                        Label CandID = (Label)newrow.FindControl("lblActivityNo");
                        //hdCandiId.Value = CandID.Text;
                        //FillDataToForm(CandID.Text);
                        DataTable DT = oDBEngine.GetDataTable(" tbl_trans_Recruitment INNER JOIN   tbl_master_designation ON tbl_trans_Recruitment.rd_Designation = tbl_master_designation.deg_id INNER JOIN   tbl_master_education ON tbl_trans_Recruitment.rd_EduQualification = tbl_master_education.edu_id INNER JOIN tbl_master_branch ON tbl_trans_Recruitment.rd_Branch = tbl_master_branch.branch_id INNER JOIN tbl_master_jobResponsibility ON tbl_trans_Recruitment.rd_JobResponsibility = tbl_master_jobResponsibility.job_id ", " tbl_master_jobResponsibility.job_responsibility AS JobResponsibility, tbl_master_branch.branch_description AS Branch,  tbl_master_education.edu_education AS EducationQualification, tbl_trans_Recruitment.rd_Vacancies AS NoofVacancies,   tbl_trans_Recruitment.rd_YearofExp AS YearofExp, convert(varchar(10),tbl_trans_Recruitment.rd_JoinDate,103) as JoinDate", " tbl_trans_Recruitment.rd_ActivityId =" + CandID.Text);
                        grdActivityDetail.DataSource = DT;
                        grdActivityDetail.DataBind();
                        break;
                    }
                }
            }
            DeselectCheckBox();
        }
        protected void grdDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)    //___This condition is for filtering header and footer, application will done on Rows only___//
            {
                CheckBox chkbox = new CheckBox();
                chkbox = (CheckBox)e.Row.FindControl("chkDetail");
                Label lblId = new Label();
                lblId = (Label)e.Row.FindControl("lblActivityNo");
                chkbox.Attributes.Add("onclick", "javascript:chkclicked(this,'" + lblId.Text + "');");
            }
        }
    }
}