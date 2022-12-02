using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class management_AddCandidateForOLetter : System.Web.UI.Page
    {
        int ID;
        int ID1;
        Converter OConvert = new Converter();
        ClsDropDownlistNameSpace.clsDropDownList cls = new ClsDropDownlistNameSpace.clsDropDownList();
        public string pageAccess = "";
        DBEngine oDBEngine = new DBEngine(string.Empty);

        //----------------------For Documents-------------
        public string[] InputName = new string[5];
        public string[] InputType = new string[5];
        public string[] InputValue = new string[5];
        //----------------------End-----------------------
        
        protected void Page_Load(object sender, EventArgs e)
        {
            // Replace .ToString() with Convert.ToString(..) By Sudip on 20-12-2016

            txtPhone.Attributes.Add("onKeypress", "return MaskMoney(event)");
            txtExpYrs.Attributes.Add("onKeypress", "return MaskMoney(event)");
            txtCurrentCTC.Attributes.Add("onKeypress", "return MaskMoney(event)");
            txtDesiredCTC.Attributes.Add("onKeypress", "return MaskMoney(event)");
            txtPreviousCTC.Attributes.Add("onKeypress", "return MaskMoney(event)");
            txtAprovedCTC.Attributes.Add("onKeypress", "return MaskMoney(event)");
            btnSave.Attributes.Add("Onclick", "Javascript:return ValidatePage();");

            // Choosen list by Sudip on 20-12-2016
            //txtReportTo.Attributes.Add("onkeyup", "CallList(this,'SearchByEmp',event)");

            //Button1.Attributes.Add("Onclick", "Javascript:return textValidation();");
            //----------------------For Documents-----------------------
            Session["KeyVal_InternalID"] = Request.QueryString["id"];
            //if (!IsPostBack)
            //{
            //    //string[,] EmployeeNameID = oDBEngine.GetFieldValue(" tbl_master_contact ", " case when cnt_firstName is null then '' else cnt_firstName end + ' '+case when cnt_middleName is null then '' else cnt_middleName end+ ' '+case when cnt_lastName is null then '' else cnt_lastName end+' ['+cnt_shortName+']' as name ", " cnt_internalId='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'", 1);
            //    //if (EmployeeNameID[0, 0] != "n")
            //    //{
            //    //    lblHeader.Text = EmployeeNameID[0, 0].ToUpper();
            //    //}
            //}

            // ----------------------End-----------------------


            // Session Handel By Sudip on 20-12-2016
            //if (HttpContext.Current.Session["userid"] == null)
            //{
            //    Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            //}

            if (HttpContext.Current.Session["userid"] != null)
            {
                if (!IsPostBack)
                {
                    //ID = int.Parse(Request.QueryString["id"].ToString());
                    string mode = Convert.ToString(Request.QueryString["mode"]);
                    string id = Convert.ToString(Request.QueryString["id"]);

                    LabelID.Text = Convert.ToString(id);
                    LabelMode.Text = Convert.ToString(mode);

                    if (mode == "edit")
                    {
                        Session["mode"] = mode;
                        EditCandidate();
                        BindGrid();
                    }
                    if (mode == "new")
                    {
                        Session["mode"] = mode;
                        NewCandidate();
                    }
                    else if (mode == "show")
                    {
                        Session["mode"] = mode;
                        EditCandidate();
                        BindGrid();
                        //    hideAll();
                    }

                    DataTable dtReportingTo = oDBEngine.GetDataTable("tbl_master_employee, tbl_master_contact", "emp_id ,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name ", " tbl_master_employee.emp_contactId = tbl_master_contact.cnt_internalId ");
                    if(dtReportingTo!=null && dtReportingTo.Rows.Count>0)
                    {
                        lstReportTo.DataSource = dtReportingTo;
                        lstReportTo.DataTextField = "Name";
                        lstReportTo.DataValueField = "emp_id";
                        lstReportTo.DataBind();
                    }
                }
                else
                {
                    EmployeeDocumentGrid.Visible = true;
                }
                drpSourceType.Attributes.Add("onchange", "ondropdown();");
                txtReferedBy.Attributes.Add("onkeyup", "call_ajax(this,'referedby',event)");
                //____________Calling Calendar on click and onfocus_______________//
                DateTime dt = oDBEngine.GetDate();

                txtDOB.EditFormatString = OConvert.GetDateFormat("Date");
                txtPJD.EditFormatString = OConvert.GetDateFormat("Date");
                txtPJD.Attributes.Add("readonly", "true");
                txtDOB.Attributes.Add("readonly", "true");
                string addr = txtLocality.Text;
                string orgadd = addr.Replace("''", " ");
                txtLocality.Text = orgadd;
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
        }
        private void NewCandidate()
        {
            EmployeeDocumentGrid.Visible = false;

            //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            DBEngine oDBEngine = new DBEngine();
            txtName.Text = "";
            txtLocality.Text = "";
            txtReferedBy.Text = "";
            txtNoofDependent.Text = "";
            txtPhone.Text = "";
            txtEmailId.Text = "";
            txtCertification.Text = "";
            txtCurrentEmployer.Text = "";
            txtExpYrs.Text = "0";
            txtCurrentCTC.Text = "0";
            txtDesiredCTC.Text = "0";
            txtPreviousCTC.Text = "0";
            txtPreviousEmployer.Text = "";
            txtReasonforChange.Text = "";
            //drpSourceType.SelectedIndex = 1;
            string[,] Data = oDBEngine.GetFieldValue("tbl_master_contactSource ", "cntsrc_id,cntsrc_sourceType", null, 2, "cntsrc_sourceType");
            cls.AddDataToDropDownList(Data, drpSourceType, 0);
            Data = oDBEngine.GetFieldValue("tbl_master_salutation", "sal_id, sal_name", null, 2, "sal_name");
            cls.AddDataToDropDownList(Data, CmbSalutation);
            CmbSalutation.SelectedValue = "1";
            Data = oDBEngine.GetFieldValue("tbl_master_Designation", "deg_id, deg_designation ", null, 2, "deg_designation");
            cls.AddDataToDropDownList(Data, cmbDesg);
            //Data = oDBEngine.GetFieldValue("tbl_master_branch", "branch_id, branch_description ", "branch_id in (" + HttpContext.Current.Session["userbranchHierarchy"] + ")", 2, "branch_description");
            Data = oDBEngine.GetFieldValue("tbl_master_branch", "branch_id, branch_description ", null, 2, "branch_description");
            cls.AddDataToDropDownList(Data, cmbBranch);
            Data = oDBEngine.GetFieldValue("tbl_master_Company", "cmp_id, cmp_name", null, 2, "cmp_name");
            cls.AddDataToDropDownList(Data, cmbOrganization);
            Data = oDBEngine.GetFieldValue("tbl_master_maritalstatus ", "mts_id,mts_maritalStatus", null, 2);
            cls.AddDataToDropDownList(Data, drpMaritalStatus, 0);
            Data = oDBEngine.GetFieldValue("tbl_master_education ", "edu_id,edu_education", null, 2, "edu_education");
            cls.AddDataToDropDownList(Data, drpQualification, 0);
            Data = oDBEngine.GetFieldValue("tbl_master_education ", "edu_id,edu_education", null, 2, "edu_education");
            cls.AddDataToDropDownList(Data, drpProfQualification, 0);
            Data = oDBEngine.GetFieldValue("tbl_master_jobResponsibility ", "job_id,job_responsibility", null, 2, "job_responsibility");
            cls.AddDataToDropDownList(Data, drpCurrentJobProfile, 0);
            Data = oDBEngine.GetFieldValue("tbl_master_jobResponsibility ", "job_id,job_responsibility", null, 2, "job_responsibility");
            cls.AddDataToDropDownList(Data, drpPreviousJobProfile, 0);
            Data = oDBEngine.GetFieldValue("tbl_master_employeeType", "emptpy_id, emptpy_type ", null, 2, "emptpy_type");
            cls.AddDataToDropDownList(Data, EmpType);
            Data = oDBEngine.GetFieldValue("tbl_master_costCenter", "cost_id, cost_description ", " cost_costCenterType = 'department' ", 2, "cost_description");
            cls.AddDataToDropDownList(Data, cmbDept);



            txtPJD.Text = "01-01-0100";

            drpQualification.Items.Insert(0, new ListItem("--Select--", "0"));
            drpProfQualification.Items.Insert(0, new ListItem("--Select--", "0"));
            drpCurrentJobProfile.Items.Insert(0, new ListItem("--Select--", "0"));
            drpPreviousJobProfile.Items.Insert(0, new ListItem("--Select--", "0"));
            drpMaritalStatus.Items.Insert(0, new ListItem("--Select--", "0"));
            cmbBranch.Items.Insert(0, new ListItem("--Select--", "0"));
            cmbOrganization.Items.Insert(0, new ListItem("--Select--", "0"));
            cmbDesg.Items.Insert(0, new ListItem("--Select--", "0"));
            drpSourceType.Items.Insert(0, new ListItem("--Select--", "0"));
            EmpType.Items.Insert(0, new ListItem("--Select--", "0"));
            cmbDept.Items.Insert(0, new ListItem("--Select--", "0"));


        }
        private void EditCandidate()
        {
            // Replace .ToString() with Convert.ToString(..) By Sudip on 20-12-2016

            int IdCopy = ID;
            //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            DBEngine oDBEngine = new DBEngine();
            if (Request.QueryString["id"] != null)
            {
                string[,] DT = oDBEngine.GetFieldValue("tbl_trans_RecruitmentDetailTemp", "rde_Name as Name, rde_ResidenceLocation as Locality, isnull(rde_SourceName,'') as SourceName,rde_NoofDepedent as NoofDependent,rde_PhoneNo as Phoneno,  rde_Email as Email, rde_Certification as Certification, rde_CurrentEmployer as CurrentEmployer, rde_WorkExp as WorkExperience, rde_CurrentCTC as CurrentCTC, rde_CTC as DesiredCtc,rde_PreviousCTC as PreviousCTC,rde_PreviousEmployer as PreviousEmployer, rde_ReasonForChange as ReasonForChange, rde_SourceType  as SourceType, isnull(rde_MaritalStatus,12) as rde_MaritalStatus,rde_EduQualification as Qualification, rde_ProfQualification as ProfQualification, rde_CurrentJobProfile as CurrentJobProfile, rde_PreviousJobProfile as PreviousJobProfile, rde_ProbableJoinDate as ProbableJoinDate,rde_DOB as Dob, rde_CandidateSex as Sex ,rde_Company,rde_Branch,rde_Designation, rde_ApprovedCTC,rde_Salutation,rde_FatherName,rde_EmpType,rde_Status,rde_ReportTo,rde_Dept ", "rde_Id=" + Request.QueryString["id"], 33);
                if (DT[0, 0] != "n")
                {
                    txtName.Text = DT[0, 0];
                    txtLocality.Text = DT[0, 1];
                    if (DT[0, 2] != "")
                    {
                        string[,] SName = oDBEngine.GetFieldValue("tbl_master_contact", "ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '')+' [' + ISNULL(cnt_UCC, '')+']'", " cnt_internalId='" + DT[0, 2] + "'", 1);
                        if (SName[0, 0] != "n")
                        {
                            txtReferedBy.Text = SName[0, 0];
                        }
                    }
                    txtReferedBy_hidden.Value = DT[0, 2];
                    txtNoofDependent.Text = DT[0, 3];
                    txtPhone.Text = DT[0, 4];
                    txtEmailId.Text = DT[0, 5];
                    txtCertification.Text = DT[0, 6];
                    txtCurrentEmployer.Text = DT[0, 7];
                    txtExpYrs.Text = DT[0, 8];
                    txtCurrentCTC.Text = DT[0, 9];
                    txtDesiredCTC.Text = DT[0, 10];
                    txtPreviousCTC.Text = DT[0, 11];
                    txtPreviousEmployer.Text = DT[0, 12];
                    txtReasonforChange.Text = DT[0, 13];
                    //drpSourceType.SelectedIndex = 1;
                    string[,] Data = oDBEngine.GetFieldValue("tbl_master_contactSource ", "cntsrc_id,cntsrc_sourceType", null, 2, "cntsrc_sourceType");
                    if (DT[0, 14] != "")
                    {
                        cls.AddDataToDropDownList(Data, drpSourceType, int.Parse(Convert.ToString(DT[0, 14])));
                    }
                    else
                    {
                        cls.AddDataToDropDownList(Data, drpSourceType, 0);
                    }
                    Data = oDBEngine.GetFieldValue("tbl_master_maritalstatus ", "mts_id,mts_maritalStatus", null, 2, "mts_maritalStatus");
                    if (DT[0, 15] != "")
                    {
                        cls.AddDataToDropDownList(Data, drpMaritalStatus, int.Parse(Convert.ToString(DT[0, 15])));
                    }
                    else
                    {
                        cls.AddDataToDropDownList(Data, drpMaritalStatus, 0);
                    }

                    Data = oDBEngine.GetFieldValue("tbl_master_education ", "edu_id,edu_education", null, 2, "edu_education");
                    if (DT[0, 16] != "")
                    {
                        cls.AddDataToDropDownList(Data, drpQualification, int.Parse(Convert.ToString(DT[0, 16])));
                    }
                    else
                    {
                        cls.AddDataToDropDownList(Data, drpQualification, 0);
                    }
                    Data = oDBEngine.GetFieldValue("tbl_master_education ", "edu_id,edu_education", null, 2, "edu_education");
                    if (DT[0, 17] != "")
                    {
                        cls.AddDataToDropDownList(Data, drpProfQualification, int.Parse(Convert.ToString(DT[0, 17])));
                    }
                    else
                    {
                        cls.AddDataToDropDownList(Data, drpProfQualification, 0);
                    }
                    Data = oDBEngine.GetFieldValue("tbl_master_jobResponsibility ", "job_id,job_responsibility", null, 2, "job_responsibility");
                    if (DT[0, 18] != "")
                    {
                        cls.AddDataToDropDownList(Data, drpCurrentJobProfile, int.Parse(Convert.ToString(DT[0, 18])));
                    }
                    else
                    {
                        cls.AddDataToDropDownList(Data, drpCurrentJobProfile, 0);
                    }
                    Data = oDBEngine.GetFieldValue("tbl_master_jobResponsibility ", "job_id,job_responsibility", null, 2, "job_responsibility");
                    if (DT[0, 19] != "")
                    {
                        cls.AddDataToDropDownList(Data, drpPreviousJobProfile, int.Parse(Convert.ToString(DT[0, 19])));
                    }
                    else
                    {
                        cls.AddDataToDropDownList(Data, drpPreviousJobProfile, 0);
                    }
                    Data = oDBEngine.GetFieldValue("tbl_master_Company", "cmp_id, cmp_name", null, 2, "cmp_name");
                    if (DT[0, 23] != "")
                    {
                        cls.AddDataToDropDownList(Data, cmbOrganization, int.Parse(Convert.ToString(DT[0, 23])));
                    }
                    else
                    {
                        cls.AddDataToDropDownList(Data, cmbOrganization, 0);
                    }

                    //Data = oDBEngine.GetFieldValue("tbl_master_branch", "branch_id, branch_description ", "branch_id in (" + HttpContext.Current.Session["userbranchHierarchy"] + ")", 2, "branch_description");
                    Data = oDBEngine.GetFieldValue("tbl_master_branch", "branch_id, branch_description ", null, 2, "branch_description");
                    if (DT[0, 24] != "")
                    {
                        cls.AddDataToDropDownList(Data, cmbBranch, int.Parse(Convert.ToString(DT[0, 24])));
                    }
                    else
                    {
                        cls.AddDataToDropDownList(Data, cmbBranch, 0);
                    }
                    Data = oDBEngine.GetFieldValue("tbl_master_Designation", "deg_id, deg_designation ", null, 2, "deg_designation");
                    if (DT[0, 25] != "")
                    {
                        cls.AddDataToDropDownList(Data, cmbDesg, int.Parse(Convert.ToString(DT[0, 25])));
                    }
                    else
                    {
                        cls.AddDataToDropDownList(Data, cmbDesg, 0);
                    }


                    Data = oDBEngine.GetFieldValue("tbl_master_employeeType", "emptpy_id, emptpy_type ", null, 2, "emptpy_type");
                    if (DT[0, 29] != "")
                    {
                        cls.AddDataToDropDownList(Data, EmpType, Int32.Parse(DT[0, 29]));
                    }
                    else
                    {

                        cls.AddDataToDropDownList(Data, EmpType, 0);
                    }

                    Data = oDBEngine.GetFieldValue("tbl_master_costCenter", "cost_id, cost_description ", " cost_costCenterType = 'department' ", 2, "cost_description");
                    if (DT[0, 32] != "")
                    {
                        cls.AddDataToDropDownList(Data, cmbDept, Int32.Parse(DT[0, 32]));
                    }
                    else
                    {

                        cls.AddDataToDropDownList(Data, cmbDept, 0);
                    }



                    Data = oDBEngine.GetFieldValue("tbl_master_salutation", "sal_id, sal_Name  ", null, 2, "sal_Name");
                    cls.AddDataToDropDownList(Data, CmbSalutation, int.Parse(Convert.ToString(DT[0, 27])));

                    txtAprovedCTC.Text = DT[0, 26];
                    //drpPJD.Value = DT[0, 20].ToString();
                    Converter oConverter = new Converter();
                    txtPJD.Value = Convert.ToDateTime(Convert.ToString(DT[0, 20]));
                    txtDOB.Value = Convert.ToDateTime(Convert.ToString(DT[0, 21]));
                    //drpDOB.Value = DT[0, 21].ToString();

                    if (DT[0, 22] != "")
                    {
                        drpSex.SelectedValue = DT[0, 22];
                    }
                    else
                    {
                        drpSex.SelectedIndex.Equals(0);
                    }


                    // drpSex.SelectedIndex = int.Parse(DT[0, 22].ToString());
                    txtFatherName.Text = DT[0, 28];

                    if (Convert.ToString(DT[0, 31]) != "")
                    {
                        // Choosen list by Sudip on 20-12-2016

                        DataTable dtReportingTo = oDBEngine.GetDataTable("tbl_master_employee, tbl_master_contact", "emp_id ,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name ", " tbl_master_employee.emp_contactId = tbl_master_contact.cnt_internalId ");
                        if (dtReportingTo != null && dtReportingTo.Rows.Count > 0)
                        {
                            lstReportTo.DataSource = dtReportingTo;
                            lstReportTo.DataTextField = "Name";
                            lstReportTo.DataValueField = "emp_id";
                            lstReportTo.DataBind();
                        }

                        string value = Convert.ToString((DT[0, 31]));
                        txtReportTo_hidden.Value = value;


                        //foreach (ListItem item in lstReportTo.Items)
                        //{
                        //    if (item.Value==value)
                        //    {
                        //        item.Selected=true;
                        //        break;
                        //        // item ...
                        //    }
                        //}

                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "FuncTrigger()", true);

                        //string[,] DataRT = oDBEngine.GetFieldValue(" tbl_master_employee, tbl_master_contact", " ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name ", " tbl_master_employee.emp_contactId = tbl_master_contact.cnt_internalId and  tbl_master_employee.emp_id ='" + DT[0, 31].ToString() + "'", 1);
                        //if (DataRT[0, 0] != "n")
                        //{
                        //    txtReportTo.Text = DataRT[0, 0];
                        //    txtReportTo_hidden.Value = DT[0, 31].ToString();
                        //}
                        //else
                        //{
                        //    txtReportTo.Text = DT[0, 31].ToString();
                        //}
                    }


                    drpQualification.Items.Insert(0, new ListItem("--Select--", "0"));
                    drpProfQualification.Items.Insert(0, new ListItem("--Select--", "0"));
                    drpCurrentJobProfile.Items.Insert(0, new ListItem("--Select--", "0"));
                    drpPreviousJobProfile.Items.Insert(0, new ListItem("--Select--", "0"));
                    drpMaritalStatus.Items.Insert(0, new ListItem("--Select--", "0"));
                    cmbBranch.Items.Insert(0, new ListItem("--Select--", "0"));
                    cmbOrganization.Items.Insert(0, new ListItem("--Select--", "0"));
                    cmbDesg.Items.Insert(0, new ListItem("--Select--", "0"));
                    drpSourceType.Items.Insert(0, new ListItem("--Select--", "0"));
                    EmpType.Items.Insert(0, new ListItem("--Select--", "0"));
                    cmbDept.Items.Insert(0, new ListItem("--Select--", "0"));


                    if (Convert.ToString(DT[0, 30]) == "Y")
                    {
                        Session["mode"] = "show";
                        btnSave.Visible = false;
                        hideAll();
                        EmployeeDocumentGrid.Columns[6].Visible = false;

                    }
                    else
                    {
                        Session["mode"] = "edit";
                        btnSave.Visible = true;

                    }
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Replace .ToString() with Convert.ToString(..) By Sudip on 20-12-2016


            DataTable dtPan = new DataTable();
            string Actid = "";
            if (Convert.ToString(Session["mode"]) == "new")
            {

                if (Convert.ToString(txtNoofDependent.Text).Length > 0)
                {
                    dtPan = oDBEngine.GetDataTable("tbl_trans_RecruitmentDetailTemp", "*", " rde_NoofDepedent='" + Convert.ToString(txtNoofDependent.Text).Trim() + "' ");
                }

                if (Convert.ToString(cmbOrganization.SelectedItem.Value) == "0")
                {
                    cmbOrganization.Focus();
                    Page.ClientScript.RegisterStartupScript(GetType(), "pagecall", "<script language='javascript'>jAlert('Please Select Organization.');</script>");
                }
                else if (Convert.ToString(cmbBranch.SelectedItem.Value) == "0")
                {
                    cmbBranch.Focus();
                    Page.ClientScript.RegisterStartupScript(GetType(), "pagecall", "<script language='javascript'>jAlert('Please Select Branch.');</script>");
                }
                else if (Convert.ToString(cmbDesg.SelectedItem.Value) == "0")
                {
                    cmbDesg.Focus();
                    Page.ClientScript.RegisterStartupScript(GetType(), "pagecall", "<script language='javascript'>jAlert('Please Select Designation.');</script>");
                }
                else if (Convert.ToString(txtAprovedCTC.Text) == "")
                {
                    txtAprovedCTC.Focus();
                    Page.ClientScript.RegisterStartupScript(GetType(), "pagecall", "<script language='javascript'>jAlert('CTC can not be blank.');</script>");
                }
                else if (Convert.ToString(EmpType.SelectedItem.Value) == "0")
                {
                    EmpType.Focus();
                    Page.ClientScript.RegisterStartupScript(GetType(), "pagecall", "<script language='javascript'>jAlert('Please Seect Employee Type.');</script>");
                }
                else if (dtPan.Rows.Count > 0)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "pagecall", "<script language='javascript'>jAlert('Candidate exists eith same PAN Number');</script>");
                    txtNoofDependent.Focus();
                }
                else
                {
                    //DBEngine oDEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
                    DBEngine oDEngine = new DBEngine();
                    //String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    String con = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    SqlConnection lcon = new SqlConnection(con);
                    lcon.Open();
                    SqlCommand lcmdEmplInsert = new SqlCommand("sp_AddNewCandidate", lcon);
                    lcmdEmplInsert.CommandType = CommandType.StoredProcedure;

                    lcmdEmplInsert.Parameters.AddWithValue("@rde_Salutation", CmbSalutation.SelectedItem.Value);
                    lcmdEmplInsert.Parameters.AddWithValue("@rde_Name", txtName.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@rde_SourceType", drpSourceType.SelectedItem.Value);
                    if (txtReferedBy_hidden.Value != "")
                    {
                        lcmdEmplInsert.Parameters.AddWithValue("@rde_SourceName", DBNull.Value);
                    }
                    else
                    {
                        lcmdEmplInsert.Parameters.AddWithValue("@rde_SourceName", txtReferedBy_hidden.Value);
                    }
                    lcmdEmplInsert.Parameters.AddWithValue("@rde_CandidateSex", drpSex.SelectedItem.Value);
                    if (txtDOB.Value != null)
                    {
                        lcmdEmplInsert.Parameters.AddWithValue("@rde_DOB", txtDOB.Value);
                    }
                    else
                    {
                        lcmdEmplInsert.Parameters.AddWithValue("@rde_DOB", "");
                    }
                    lcmdEmplInsert.Parameters.AddWithValue("@rde_EduQualification", drpQualification.SelectedItem.Value);
                    lcmdEmplInsert.Parameters.AddWithValue("@rde_ProfQualification", drpProfQualification.SelectedItem.Value);
                    lcmdEmplInsert.Parameters.AddWithValue("@rde_Certification", txtCertification.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@rde_WorkExp", txtExpYrs.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@rde_CTC", txtDesiredCTC.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@rde_ReasonForChange", txtReasonforChange.Text);
                    if (txtPJD.Value != null)
                    {
                        lcmdEmplInsert.Parameters.AddWithValue("@rde_ProbableJoinDate", txtPJD.Value);
                    }
                    else
                    {
                        lcmdEmplInsert.Parameters.AddWithValue("@rde_ProbableJoinDate", "");
                    }
                    lcmdEmplInsert.Parameters.AddWithValue("@rde_ResidenceLocation", txtLocality.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@rde_CurrentEmployer", txtCurrentEmployer.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@rde_CurrentJobProfile", drpCurrentJobProfile.SelectedItem.Value);
                    lcmdEmplInsert.Parameters.AddWithValue("@rde_CurrentCTC", txtCurrentCTC.Text);
                    //lcmdEmplInsert.Parameters.AddWithValue("@emp_currentCTC", txtCTC.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@rde_PreviousEmployer", txtPreviousEmployer.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@rde_PreviousJobProfile", drpPreviousJobProfile.SelectedItem.Value);
                    lcmdEmplInsert.Parameters.AddWithValue("@rde_PreviousCTC", txtPreviousCTC.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@rde_NoofDepedent", txtNoofDependent.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@rde_PhoneNo", txtPhone.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@rde_Email", txtEmailId.Text);//-------------------------
                    lcmdEmplInsert.Parameters.AddWithValue("@rde_MaritalStatus", drpMaritalStatus.SelectedItem.Value);
                    lcmdEmplInsert.Parameters.AddWithValue("@rde_Company", cmbOrganization.SelectedItem.Value);
                    lcmdEmplInsert.Parameters.AddWithValue("@rde_Branch", cmbBranch.SelectedItem.Value);
                    lcmdEmplInsert.Parameters.AddWithValue("@rde_Designation", cmbDesg.SelectedItem.Value);
                    lcmdEmplInsert.Parameters.AddWithValue("@rde_ApprovedCTC", txtAprovedCTC.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@CreateDate", oDBEngine.GetDate());//-------------------
                    lcmdEmplInsert.Parameters.AddWithValue("@CreateUser", HttpContext.Current.Session["userid"]);
                    lcmdEmplInsert.Parameters.AddWithValue("@rde_Status", "N");
                    lcmdEmplInsert.Parameters.AddWithValue("@rde_FatherName", txtFatherName.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@rde_EmpType", EmpType.SelectedItem.Value);
                    lcmdEmplInsert.Parameters.AddWithValue("@rde_ReportTo", txtReportTo_hidden.Value);
                    lcmdEmplInsert.Parameters.AddWithValue("@rde_Dept", cmbDept.SelectedItem.Value);
                    SqlParameter parameter = new SqlParameter("@result", SqlDbType.BigInt);
                    parameter.Direction = ParameterDirection.Output;
                    lcmdEmplInsert.Parameters.Add(parameter);
                    lcmdEmplInsert.ExecuteNonQuery();
                    // Mantis Issue 24802
                    if (lcon.State == ConnectionState.Open)
                    {
                        lcon.Close();
                    }
                    // End of Mantis Issue 24802
                    string InternalID = Convert.ToString(parameter.Value);
                    if (Convert.ToString(InternalID) != "")
                    {
                        LabelID.Text = InternalID;
                        LabelMode.Text = "new";
                        EmployeeDocumentGrid.Visible = true;
                        this.Page.ClientScript.RegisterStartupScript(GetType(), "script1", "<script>alert('Candidate Added Successfully.');</script>");
                        hideAll();
                        BindGrid();

                        //ClientScript.RegisterStartupScript(this.GetType(), "Success", "<script type='text/javascript'>alert('Candidate Added Successfully');window.location='../../GenerateOfferLater.aspx';</script>'");
                    }
                }

            }
            if (Convert.ToString(Session["mode"]) == "edit")
            {
                if (Convert.ToString(txtNoofDependent.Text).Length > 0)
                {
                    dtPan = oDBEngine.GetDataTable("tbl_trans_RecruitmentDetailTemp", "*", " rde_NoofDepedent='" + Convert.ToString(txtNoofDependent.Text).Trim() + "' and rde_id !='" + Convert.ToString(Request.QueryString["id"]) + "' ");
                }


                //DBEngine oBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
                DBEngine oBEngine = new DBEngine();
                string values = "";
                string CanID = Convert.ToString(Request.QueryString["id"]);
                if (Request.QueryString["id"] != null)
                {
                    values = "rde_Name='" + txtName.Text + "',rde_SourceType=" + drpSourceType.SelectedItem.Value + ",";
                }
                if (int.Parse(Convert.ToString(drpSourceType.SelectedItem.Value)) == 3 || int.Parse(Convert.ToString(drpSourceType.SelectedItem.Value)) == 4 || int.Parse(Convert.ToString(drpSourceType.SelectedItem.Value)) == 5 || int.Parse(Convert.ToString(drpSourceType.SelectedItem.Value)) == 8 || int.Parse(Convert.ToString(drpSourceType.SelectedItem.Value)) == 10 || int.Parse(Convert.ToString(drpSourceType.SelectedItem.Value)) == 14)
                {
                    if (txtReferedBy_hidden.Value != " ")
                    {
                        string IntrnlID = txtReferedBy_hidden.Value;
                        values += "rde_SourceName='" + IntrnlID + "',";
                    }
                    else
                    {
                        lblmessage.Text = "Either \"Source Type\" OR The \"Source Name\" is not correct!";
                        return;
                    }
                }
                if (Request.QueryString["id"] != null)
                {
                    //  +",rde_FatherName="+ txtFatherName.Text
                    values += "rde_CandidateSex=" + drpSex.SelectedItem.Value + ",rde_DOB='" + Convert.ToString(txtDOB.Value) + "',rde_EduQualification=" + drpQualification.SelectedItem.Value + ",rde_ProfQualification=" + drpProfQualification.SelectedItem.Value + ",rde_Certification='" + txtCertification.Text + "',";
                    values += "rde_WorkExp='" + txtExpYrs.Text + "',rde_CTC='" + txtDesiredCTC.Text + "',rde_FatherName='" + txtFatherName.Text + "', rde_ReasonForChange='" + txtReasonforChange.Text + "',rde_ProbableJoinDate='" + Convert.ToString(txtPJD.Value) + "',rde_ResidenceLocation='" + txtLocality.Text + "',rde_CurrentEmployer='" + txtCurrentEmployer.Text + "',rde_CurrentJobProfile=" + drpCurrentJobProfile.SelectedItem.Value + ",rde_CurrentCTC='" + txtCurrentCTC.Text + "',rde_PreviousEmployer='" + txtPreviousEmployer.Text + "',rde_PreviousJobProfile=" + drpPreviousJobProfile.SelectedItem.Value + ",rde_PreviousCTC='" + txtPreviousCTC.Text + "',rde_NoofDepedent='" + txtNoofDependent.Text + "',rde_PhoneNo='" + txtPhone.Text + "',rde_Email='" + txtEmailId.Text + "',rde_MaritalStatus=" + drpMaritalStatus.SelectedItem.Value + ",rde_Company=" + cmbOrganization.SelectedItem.Value + ",rde_Branch=" + cmbBranch.SelectedItem.Value + ",rde_Designation=" + cmbDesg.SelectedItem.Value + ",rde_ApprovedCTC='" + txtAprovedCTC.Text + "',rde_Salutation=" + CmbSalutation.SelectedItem.Value + ",rde_EmpType=" + EmpType.SelectedItem.Value + ",rde_ReportTo=" + txtReportTo_hidden.Value + ",rde_Dept=" + cmbDept.SelectedItem.Value;
                    if (Convert.ToString(cmbOrganization.SelectedItem.Value) == "0")
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "pagecall", "<script language='javascript'>alert('Please Select Organization.');</script>");
                    }
                    else if (Convert.ToString(cmbBranch.SelectedItem.Value) == "0")
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "pagecall", "<script language='javascript'>alert('Please Select Branch.');</script>");
                    }
                    else if (Convert.ToString(cmbDesg.SelectedItem.Value) == "0")
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "pagecall", "<script language='javascript'>alert('Please Select Designation.');</script>");
                    }
                    else if (txtAprovedCTC.Text == "")
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "pagecall", "<script language='javascript'>alert('CTC can not be blank.');</script>");
                    }
                    else if (Convert.ToString(EmpType.SelectedItem.Value) == "0")
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "pagecall", "<script language='javascript'>alert('Please Seect Employee Type.');</script>");
                    }
                    else if (dtPan.Rows.Count > 0)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "pagecall", "<script language='javascript'>alert('Candidate exists with same PAN Number');</script>");
                    }
                    else
                    {
                        int NoRowEffected = oDBEngine.SetFieldValue("tbl_trans_RecruitmentDetailTemp", values, " rde_Id=" + CanID);
                        this.Page.ClientScript.RegisterStartupScript(GetType(), "script1", "<script>parent.editwin.close();</script>");
                    }

                }
            }

        }
        //protected void btnCancel_Click(object sender, EventArgs e)
        //{
        //    this.Page.ClientScript.RegisterStartupScript(GetType(), "script56", "<script>parent.editwin.close();</script>");

        //}



        //----------------------For Documents-----------------------
        public void BindGrid()
        {
            // Replace .ToString() with Convert.ToString(..) By Sudip on 20-12-2016

            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataColumn col1 = new DataColumn("Id");
            DataColumn col2 = new DataColumn("Type");
            DataColumn col3 = new DataColumn("FileName");
            DataColumn col4 = new DataColumn("Src");
            DataColumn col5 = new DataColumn("FilePath");
            //DataColumn col6 = new DataColumn("Slno");
            dt.Columns.Add(col1);
            dt.Columns.Add(col2);
            dt.Columns.Add(col3);
            dt.Columns.Add(col4);
            dt.Columns.Add(col5);
            //dt.Columns.Add(col6);
            dt1 = oDBEngine.GetDataTable("tbl_master_document INNER JOIN tbl_master_documentType ON tbl_master_document.doc_documentTypeId = tbl_master_documentType.dty_id", "tbl_master_document.doc_id AS Id, tbl_master_documentType.dty_documentType AS Type,tbl_master_document.doc_documentName AS FileName, tbl_master_document.doc_source AS Src,doc_buildingid,doc_Floor,doc_RoomNo,doc_CellNo,doc_FileNo", "doc_contactId='" + LabelID.Text + "'");
            if (dt1.Rows.Count != 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    if (Convert.ToString(dt1.Rows[i][4]) == "0")
                    {
                        DataRow RowNew = dt.NewRow();
                        RowNew["Id"] = Convert.ToString(dt1.Rows[i][0]);
                        RowNew["Type"] = Convert.ToString(dt1.Rows[i][1]);
                        RowNew["FileName"] = Convert.ToString(dt1.Rows[i][2]);
                        RowNew["Src"] = Convert.ToString(dt1.Rows[i][3]);
                        //RowNew["Slno"] = i + 1;
                        string BName = "N/A";
                        RowNew["FilePath"] = BName;
                        dt.Rows.Add(RowNew);
                    }
                    else
                    {
                        DataRow RowNew = dt.NewRow();
                        RowNew["Id"] = Convert.ToString(dt1.Rows[i][0]);
                        RowNew["Type"] = Convert.ToString(dt1.Rows[i][1]);
                        RowNew["FileName"] = Convert.ToString(dt1.Rows[i][2]);
                        RowNew["Src"] = Convert.ToString(dt1.Rows[i][3]);
                        //RowNew["Slno"] = i + 1;
                        string BuildingName = "";

                        if (Convert.ToString(dt1.Rows[i][4]) != "")
                        {

                            string[,] bname1 = oDBEngine.GetFieldValue("tbl_master_building", "bui_name", " bui_id='" + Convert.ToString(dt1.Rows[i][4]) + "'", 1);
                            if (bname1[0, 0] != "n")
                            {
                                BuildingName = bname1[0, 0];
                            }
                        }
                        string BName = "Building-  " + BuildingName + " " + "/ Floor No : " + Convert.ToString(dt1.Rows[i][5]) + " " + "/ Room No-" + Convert.ToString(dt1.Rows[i][6]) + " " + "/ Cabinet No-" + Convert.ToString(dt1.Rows[i][7]) + " " + "/ File No-" + Convert.ToString(dt1.Rows[i][8]);
                        RowNew["FilePath"] = BName;
                        dt.Rows.Add(RowNew);
                    }
                }
            }
            EmployeeDocumentGrid.DataSource = dt.DefaultView;
            EmployeeDocumentGrid.DataBind();
        }
        protected void EmployeeDocumentGrid_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            // Replace .ToString() with Convert.ToString(..) By Sudip on 20-12-2016

            ClearArray();
            string Id = Convert.ToString(e.Keys[0]);
            /*-------Insert Information in employee Log table before update data-------*/
            InputName[0] = "Module";
            InputType[0] = "V";
            InputValue[0] = "DocDelete";

            InputName[1] = "filterId";
            InputType[1] = "V";
            InputValue[1] = Id.Trim();

            InputName[2] = "ModifyUser";
            InputType[2] = "V";
            InputValue[2] = Convert.ToString(HttpContext.Current.Session["userid"]);

            SQLProcedures.SelectProcedureArr("Employee_Log_Update", InputName, InputType, InputValue);
            /*-------END--------------------------------*/
            oDBEngine.DeleteValue("tbl_master_document", " doc_id='" + Id + "'");
            BindGrid();
        }
        public void ClearArray()
        {
            Array.Clear(InputName, 0, InputName.Length - 1);
            Array.Clear(InputType, 0, InputType.Length - 1);
            Array.Clear(InputValue, 0, InputValue.Length - 1);
        }
        protected void EmployeeDocumentGrid_DataBound(object sender, EventArgs e)
        {

        }
        public void hideAll()
        {
            CmbSalutation.Enabled = false;
            txtLocality.Enabled = false;
            drpSourceType.Enabled = false;
            drpMaritalStatus.Enabled = false;
            txtReferedBy.Enabled = false;
            txtNoofDependent.Enabled = false;
            drpSex.Enabled = false;
            txtPhone.Enabled = false;
            txtDOB.Enabled = false;
            txtEmailId.Enabled = false;
            drpQualification.Enabled = false;
            drpProfQualification.Enabled = false;
            txtCertification.Enabled = false;
            txtCurrentEmployer.Enabled = false;
            txtExpYrs.Enabled = false;
            drpCurrentJobProfile.Enabled = false;
            txtCurrentCTC.Enabled = false;
            txtDesiredCTC.Enabled = false;
            txtPreviousCTC.Enabled = false;
            txtPreviousEmployer.Enabled = false;
            txtPJD.Enabled = false;
            drpPreviousJobProfile.Enabled = false;
            txtReasonforChange.Enabled = false;
            txtFatherName.Enabled = false;
            cmbOrganization.Enabled = false;
            cmbBranch.Enabled = false;
            cmbDesg.Enabled = false;
            txtAprovedCTC.Enabled = false;
            txtName.Enabled = false;
            EmpType.Enabled = false;
            btnSave.Visible = false;
            //txtReportTo.Enabled = false;
            lstReportTo.Enabled = false;
            // Choosen list by Sudip on 20-12-2016

        }
        //----------------------For Documents-----------------------
    }

}