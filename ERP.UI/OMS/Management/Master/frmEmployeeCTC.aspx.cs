using ClsDropDownlistNameSpace;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Collections.Generic;
using BusinessLogicLayer;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_frmEmployeeCTC : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {

        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        clsDropDownList clsdropdown = new clsDropDownList();
        BusinessLogicLayer.Converter Oconverter = new BusinessLogicLayer.Converter();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToString(Request.QueryString["ContID"]) != "N")
            {
                DataTable dtCnt = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalid", "cnt_id='" + Convert.ToString(Request.QueryString["ContID"]) + "'");
                HttpContext.Current.Session["KeyVal_InternalID"] = dtCnt.Rows[0][0].ToString();
            }
            else
            {

                string abc = Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]);
            }

            if (!IsPostBack)
            {
                JoiningDate.UseMaskBehavior = true;
                JoiningDate.EditFormatString = Oconverter.GetDateFormat();
                JoiningDate.EditFormatString = "dd-MM-yyyy";

                cmbLeaveEff.UseMaskBehavior = true;
                cmbLeaveEff.EditFormatString = Oconverter.GetDateFormat();
                cmbLeaveEff.EditFormatString = "dd-MM-yyyy";
                ShowForm();
                /*Code   Commented   By Priti on 06122016 to use jquery Choosen*/
                // txtReportTo.Attributes.Add("onkeyup", "CallList(this,'SearchByEmp',event)");
                // txtReportTo.Attributes.Add("onfocus", "CallList(this,'SearchByEmp',event)");
                //txtAReportHead.Attributes.Add("onkeyup", "CallList(this,'SearchByEmp',event)");

                // txtAReportHead.Attributes.Add("onfocus", "CallList(this,'SearchByEmp',event)");
                // txtReportTo.Attributes.Add("onclick", "CallList(this,'SearchByEmp',event)");
                // txtColleague.Attributes.Add("onkeyup", "CallList(this,'SearchByEmp',event)");

                // txtColleague.Attributes.Add("onfocus", "CallList(this,'SearchByEmp',event)");
                // txtAReportHead.Attributes.Add("onclick", "CallList(this,'SearchByEmp',event)");
                // txtColleague.Attributes.Add("onclick", "CallList(this,'SearchByEmp',event)");
                //...........end.................
            }
            //btnSave.Attributes.Add("onclick", "javascript:return ValidateCTC();");
            txtCTC.Attributes.Add("onKeypress", "return MaskMoney(event)");
            txtBasic.Attributes.Add("onKeypress", "return MaskMoney(event)");
            txtHRA.Attributes.Add("onKeypress", "return MaskMoney(event)");
            txtCCA.Attributes.Add("onKeypress", "return MaskMoney(event)");
            txtSpAl.Attributes.Add("onKeypress", "return MaskMoney(event)");
            txtChAL.Attributes.Add("onKeypress", "return MaskMoney(event)");

            txtPf.Attributes.Add("onKeypress", "return MaskMoney(event)");
            txtMedAl.Attributes.Add("onKeypress", "return MaskMoney(event)");
            txtLTA.Attributes.Add("onKeypress", "return MaskMoney(event)");
            txtConvence.Attributes.Add("onKeypress", "return MaskMoney(event)");
            txtMbAl.Attributes.Add("onKeypress", "return MaskMoney(event)");
            txtCarAl.Attributes.Add("onKeyup", "return MaskMoneyDecimal(event)");

            txtUniform.Attributes.Add("onKeyup", "return MaskMoneyDecimal(event)");
            txtBook.Attributes.Add("onKeyup", "return MaskMoneyDecimal(event)");
            txtSeminar.Attributes.Add("onKeyup", "return MaskMoneyDecimal(event)");
            txtOther.Attributes.Add("onKeyup", "return MaskMoneyDecimal(event)");


        }
        /*Code  Added  By Priti on 06122016 to use jquery Choosen*/
        [WebMethod]
        public static List<string> GetEmpCTC(string reqStr)
        {
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = new DataTable();
            DT.Rows.Clear();
            DT = oDBEngine.GetDataTable("tbl_master_employee, tbl_master_contact,tbl_trans_employeeCTC", "ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name, tbl_master_employee.emp_id as Id    ", " tbl_master_employee.emp_contactId = tbl_trans_employeeCTC.emp_cntId and  tbl_trans_employeeCTC.emp_cntId = tbl_master_contact.cnt_internalId   and cnt_contactType='EM'  and (cnt_firstName Like '" + reqStr + "%' or cnt_shortName like '" + reqStr + "%')");
            List<string> obj = new List<string>();
            foreach (DataRow dr in DT.Rows)
            {

                obj.Add(Convert.ToString(dr["Name"]) + "|" + Convert.ToString(dr["Id"]));
            }
            return obj;
        }
        //...............code end........
        private void ShowForm()
        {
            if (Request.QueryString["id"] != "ADD")
            {
                string popupScript = "";
                string query = Convert.ToString(Request.QueryString["link"]);
                int Unqid_empCTC = Convert.ToInt32(Convert.ToString(Request.QueryString["id"]));
                //string[,] InternalId;
                //HttpContext.Current.Session["KeyVal_InternalID"] = InternalId[0, 0];
                string[,] ContactData;
                string[,] CntExitEmp;


                //Code Added by  Sandip on 20032017 to prevent Branchid in during edit mode.
                cmbBranch.Enabled = false;
                //Code Above Added by  Sandip on 20032017 to prevent Branchid in during edit mode.


                //Code For Validate CTC Applicable From Date and Leave Effected From
                string[,] DOJ;
                DOJ = oDBEngine.GetFieldValue("Tbl_Master_Employee", "Emp_DateOfJoining", " emp_contactId='" + Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]) + "'", 1);
                Hidden_DOJ.Value = DOJ[0, 0];
                /////////////////////////////////////////////////////////////////////////////
                CntExitEmp = oDBEngine.GetFieldValue("tbl_trans_employeeCTC A LEFT JOIN tbl_master_designation ON A.emp_Designation = tbl_master_designation.deg_id LEFT JOIN tbl_master_costCenter ON A.emp_Department = tbl_master_costCenter.cost_id LEFT JOIN tbl_master_company ON A.emp_Organization = tbl_master_company.cmp_id  LEFT JOIN tbl_master_employee ON A.emp_cntId = tbl_master_employee.emp_contactId ",
                                           "A.emp_effectiveDate AS EffectiveDate,convert(varchar(11),A.emp_effectiveDate,113) AS EffectiveDate1,convert(varchar(11),A.emp_effectiveuntil,113) AS EffectiveUntil1, A.emp_effectiveuntil AS EffectiveUntil, A.emp_id AS Id, tbl_master_designation.deg_designation AS Designation, tbl_master_costCenter.cost_description AS Department, tbl_master_company.cmp_Name AS CompanyName, A.emp_id, A.emp_cntId, A.emp_effectiveDate as emp_dateofJoining, A.emp_effectiveuntil, A.emp_Organization, A.emp_JobResponsibility, A.emp_Designation, (select branch_description from tbl_master_branch where branch_id= A.emp_branch) as BranchName,	A.emp_branch,A.emp_LeaveSchemeAppliedFrom,A.emp_type, A.emp_Department, A.emp_reportTo, A.emp_deputy, A.emp_colleague, A.emp_workinghours, A.emp_currentCTC, A.emp_basic, A.emp_HRA, A.emp_CCA, A.emp_spAllowance, A.emp_childrenAllowance, A.emp_totalLeavePA, A.emp_PF, A.emp_medicalAllowance, A.emp_LTA, A.emp_convence, A.emp_mobilePhoneExp,A.EMP_CarAllowance, A.EMP_UniformAllowance,A.EMP_BooksPeriodicals ,A.EMP_SeminarAllowance ,A.EMP_OtherAllowance,A.emp_totalMedicalLeavePA, A.CreateDate, A.CreateUser, A.LastModifyDate, A.LastModifyUser,A.emp_Remarks",
                                           " (A.emp_cntId = '" + Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]) + "'and A.emp_id='" + Unqid_empCTC + "') and (emp_effectiveuntil is null or emp_effectiveuntil='1900-01-01 00:00:00.000')", 47);

                ContactData = oDBEngine.GetFieldValue("tbl_trans_employeeCTC A LEFT JOIN tbl_master_designation ON A.emp_Designation = tbl_master_designation.deg_id LEFT JOIN tbl_master_costCenter ON A.emp_Department = tbl_master_costCenter.cost_id LEFT JOIN tbl_master_company ON A.emp_Organization = tbl_master_company.cmp_id  LEFT JOIN tbl_master_employee ON A.emp_cntId = tbl_master_employee.emp_contactId ",
                               "A.emp_effectiveDate AS EffectiveDate,convert(varchar(11),A.emp_effectiveDate,113) AS EffectiveDate1,convert(varchar(11),A.emp_effectiveuntil,113) AS EffectiveUntil1, A.emp_effectiveuntil AS EffectiveUntil, A.emp_id AS Id, tbl_master_designation.deg_designation AS Designation, tbl_master_costCenter.cost_description AS Department, tbl_master_company.cmp_Name AS CompanyName, A.emp_id, A.emp_cntId, A.emp_effectiveDate as emp_dateofJoining, A.emp_effectiveuntil, A.emp_Organization, A.emp_JobResponsibility, A.emp_Designation, (select branch_description from tbl_master_branch where branch_id= A.emp_branch) as BranchName,	A.emp_branch,A.emp_LeaveSchemeAppliedFrom,A.emp_type, A.emp_Department, A.emp_reportTo, A.emp_deputy, A.emp_colleague, A.emp_workinghours, A.emp_currentCTC, A.emp_basic, A.emp_HRA, A.emp_CCA, A.emp_spAllowance, A.emp_childrenAllowance, A.emp_totalLeavePA, A.emp_PF, A.emp_medicalAllowance, A.emp_LTA, A.emp_convence, A.emp_mobilePhoneExp,A.EMP_CarAllowance, A.EMP_UniformAllowance,A.EMP_BooksPeriodicals ,A.EMP_SeminarAllowance ,A.EMP_OtherAllowance,A.emp_totalMedicalLeavePA, A.CreateDate, A.CreateUser, A.LastModifyDate, A.LastModifyUser,A.emp_Remarks",
                               " (A.emp_cntId = '" + Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]) + "'and A.emp_id='" + Unqid_empCTC + "') ", 47);

                /// Coded By Samrat Roy -- 18/04/2017 
                /// To Genarate Contact Type on Employee Type Selection(DME/ISD)
                PopulateContactType(ContactData);
                ///////// ############################

                if (CntExitEmp[0, 0] == "n")
                {
                    DisbledCTC();
                }
                if (ContactData[0, 10] != "" && ContactData[0, 10] != "01/01/1900" && ContactData[0, 10] != "1/1/1900 12:00:00 AM")
                {
                    JoiningDate.Value = Convert.ToDateTime(ContactData[0, 10]);
                    Hidden_CTCAppFrom.Value = ContactData[0, 10];
                }
                else
                    JoiningDate.Value = null;
                if (ContactData[0, 17] != "" && ContactData[0, 17] != "01/01/1900" && ContactData[0, 17] != "1/1/1900 12:00:00 AM")
                {
                    cmbLeaveEff.Value = Convert.ToDateTime(ContactData[0, 17]);
                    Hidden_LEF.Value = ContactData[0, 17];
                }
                else
                    cmbLeaveEff.Value = null;

                string[,] DataOrg = oDBEngine.GetFieldValue("tbl_master_Company", "cmp_id, cmp_name", null, 2, "cmp_name");
                if (ContactData[0, 12] != "")
                {
                    clsdropdown.AddDataToDropDownList(DataOrg, cmbOrganization, Int32.Parse(ContactData[0, 12]));
                }
                else
                {

                    clsdropdown.AddDataToDropDownList(DataOrg, cmbOrganization, 0);
                }
                string[,] DataJR = oDBEngine.GetFieldValue("tbl_master_jobresponsibility", "job_id, job_responsibility", null, 2, "job_responsibility");
                if (ContactData[0, 13] != "")
                {
                    clsdropdown.AddDataToDropDownList(DataJR, cmbJobResponse, Int32.Parse(ContactData[0, 13]));
                }
                else
                {

                    clsdropdown.AddDataToDropDownList(DataJR, cmbJobResponse, 0);
                }
                string[,] DataBr = oDBEngine.GetFieldValue("tbl_master_branch", "branch_id, branch_description ", null, 2, "branch_description");
                if (ContactData[0, 16] != "")
                {
                    clsdropdown.AddDataToDropDownList(DataBr, cmbBranch, Int32.Parse(ContactData[0, 16]));
                }
                else
                {

                    clsdropdown.AddDataToDropDownList(DataBr, cmbBranch, 0);
                }
                string[,] DataDSG = oDBEngine.GetFieldValue("tbl_master_Designation", "deg_id, deg_designation ", null, 2, "deg_designation");
                if (ContactData[0, 14] != "")
                {
                    clsdropdown.AddDataToDropDownList(DataDSG, cmbDesg, Int32.Parse(ContactData[0, 14]));
                }
                else
                {

                    clsdropdown.AddDataToDropDownList(DataDSG, cmbDesg, 0);
                }

                string[,] DataETYPE = oDBEngine.GetFieldValue("tbl_master_employeeType", "emptpy_id, emptpy_type ", null, 2, "emptpy_type");
                if (ContactData[0, 18] != "")
                {
                    clsdropdown.AddDataToDropDownList(DataETYPE, EmpType, Int32.Parse(ContactData[0, 18]));
                }
                else
                {

                    clsdropdown.AddDataToDropDownList(DataETYPE, EmpType, 0);
                }

                string[,] DataCC = oDBEngine.GetFieldValue("tbl_master_costCenter", "cost_id, cost_description ", " cost_costCenterType = 'department' ", 2, "cost_description");
                if (ContactData[0, 19] != "")
                {
                    clsdropdown.AddDataToDropDownList(DataCC, cmbDept, Int32.Parse(ContactData[0, 19]));
                }
                else
                {

                    clsdropdown.AddDataToDropDownList(DataCC, cmbDept, 0);
                }
                string[,] DataWH = oDBEngine.GetFieldValue("tbl_EmpWorkingHours", "Id,Name  ", null, 2, "Name");
                if (ContactData[0, 23] != "")
                {
                    clsdropdown.AddDataToDropDownList(DataWH, cmbWorkingHr, Int32.Parse(ContactData[0, 23]));
                }
                else
                {

                    clsdropdown.AddDataToDropDownList(DataWH, cmbWorkingHr, 0);
                }

                string[,] DataLVS = oDBEngine.GetFieldValue("tbl_master_LeaveScheme", "ls_id, ls_name  ", null, 2, "ls_name");
                if (ContactData[0, 30] != "")
                {
                    clsdropdown.AddDataToDropDownList(DataLVS, cmbLeaveP, Int32.Parse(ContactData[0, 30]));
                }
                else
                {

                    clsdropdown.AddDataToDropDownList(DataLVS, cmbLeaveP, 0);
                }

                txtCTC.Text = ContactData[0, 24];
                txtBasic.Text = ContactData[0, 25];

                txtHRA.Text = ContactData[0, 26];
                txtCCA.Text = ContactData[0, 27];

                txtSpAl.Text = ContactData[0, 28];
                txtChAL.Text = ContactData[0, 29];

                txtPf.Text = ContactData[0, 31];
                txtMedAl.Text = ContactData[0, 32];

                txtLTA.Text = ContactData[0, 33];
                txtConvence.Text = ContactData[0, 34];

                txtMbAl.Text = ContactData[0, 35];
                txtCarAl.Text = ContactData[0, 36];

                txtUniform.Text = ContactData[0, 37];
                txtBook.Text = ContactData[0, 38];

                txtSeminar.Text = ContactData[0, 39];
                txtOther.Text = ContactData[0, 40];

                txtRemarks.Text = ContactData[0, 46];



                string[,] DataRT = oDBEngine.GetFieldValue(" tbl_master_employee, tbl_master_contact", " ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name ", " tbl_master_employee.emp_contactId = tbl_master_contact.cnt_internalId and  tbl_master_employee.emp_id ='" + ContactData[0, 20].ToString() + "'", 1);
                if (DataRT[0, 0] != "n")
                {
                    // txtReportTo.Text = DataRT[0, 0];
                    txtReportTo_hidden.Value = ContactData[0, 20].ToString();
                    lstReportTo.Text = ContactData[0, 20].ToString();
                }
                else
                {
                    lstReportTo.Text = ContactData[0, 20].ToString();
                    // txtReportTo.Text = ContactData[0, 20].ToString();
                }

                string[,] DataRH = oDBEngine.GetFieldValue(" tbl_master_employee, tbl_master_contact", " ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name ", " tbl_master_employee.emp_contactId = tbl_master_contact.cnt_internalId and  tbl_master_employee.emp_id ='" + ContactData[0, 21].ToString() + "'", 1);
                if (DataRH[0, 0] != "n")
                {
                    //txtAReportHead.Text = DataRH[0, 0];
                    txtAReportHead_hidden.Value = ContactData[0, 21].ToString();
                    lstReportHead.Text = ContactData[0, 21].ToString();
                }
                else
                {
                    // txtAReportHead.Text = ContactData[0, 21].ToString();
                    lstReportHead.Text = ContactData[0, 21].ToString();
                }

                string[,] DataCLG = oDBEngine.GetFieldValue(" tbl_master_employee, tbl_master_contact", " ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name ", " tbl_master_employee.emp_contactId = tbl_master_contact.cnt_internalId and  tbl_master_employee.emp_id ='" + ContactData[0, 22].ToString() + "'", 1);
                if (DataCLG[0, 0] != "n")
                {
                    //txtColleague.Text = DataCLG[0, 0];
                    txtColleague_hidden.Value = ContactData[0, 22].ToString();
                    lstColleague.Text = ContactData[0, 22].ToString();
                }
                else
                {
                    //txtColleague.Text = ContactData[0, 22].ToString();
                    lstColleague.Text = ContactData[0, 22].ToString();
                }




                cmbOrganization.Items.Insert(0, new ListItem("--Select--", "0"));
                cmbJobResponse.Items.Insert(0, new ListItem("--Select--", "0"));
                cmbBranch.Items.Insert(0, new ListItem("--Select--", "0"));
                cmbDesg.Items.Insert(0, new ListItem("--Select--", "0"));
                EmpType.Items.Insert(0, new ListItem("--Select--", "0"));
                cmbDept.Items.Insert(0, new ListItem("--Select--", "0"));
                cmbWorkingHr.Items.Insert(0, new ListItem("--Select--", "0"));
                cmbLeaveP.Items.Insert(0, new ListItem("--Select--", "0"));
                //}
                //else
                //{
                //    //popupScript = "<script language='javascript'>" + "alert('You Cannot update this record!.. ');window.parent.location.href='" + query + "';window.parent.popup.Hide();</script>";
                //    //ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
                //    DisbledCTC();

                //}


            }
            else
            {


                DataTable DT_empCTC = oDBEngine.GetDataTable(" tbl_trans_employeeCTC ", " emp_id ", " emp_cntId='" + Session["KeyVal_InternalID"] + "'");
                if (DT_empCTC.Rows.Count > 0)
                    JoiningDate.Value = oDBEngine.GetDate();
                else
                {
                    DataTable dt = oDBEngine.GetDataTable(" tbl_master_employee", "emp_dateofJoining", " (emp_contactId = '" + Convert.ToString(Session["KeyVal_InternalID"]) + "')");
                    JoiningDate.Value = dt.Rows[0][0];

                }

                string[,] Data = oDBEngine.GetFieldValue("tbl_master_Company", "cmp_id, cmp_name", null, 2, "cmp_name");
                clsdropdown.AddDataToDropDownList(Data, cmbOrganization);
                Data = oDBEngine.GetFieldValue("tbl_master_jobresponsibility", "job_id, job_responsibility", null, 2, "job_responsibility");
                clsdropdown.AddDataToDropDownList(Data, cmbJobResponse);
                Data = oDBEngine.GetFieldValue("tbl_master_branch", "branch_id, branch_description ", null, 2, "branch_description");
                clsdropdown.AddDataToDropDownList(Data, cmbBranch);
                Data = oDBEngine.GetFieldValue("tbl_master_Designation", "deg_id, deg_designation ", null, 2, "deg_designation");
                clsdropdown.AddDataToDropDownList(Data, cmbDesg);
                Data = oDBEngine.GetFieldValue("tbl_master_employeeType", "emptpy_id, emptpy_type ", null, 2, "emptpy_type");
                clsdropdown.AddDataToDropDownList(Data, EmpType);
                Data = oDBEngine.GetFieldValue("tbl_master_costCenter", "cost_id, cost_description ", " cost_costCenterType = 'department' ", 2, "cost_description");
                clsdropdown.AddDataToDropDownList(Data, cmbDept);
                Data = oDBEngine.GetFieldValue("tbl_EmpWorkingHours", "Id,Name  ", null, 2, "Name");
                clsdropdown.AddDataToDropDownList(Data, cmbWorkingHr);
                Data = oDBEngine.GetFieldValue("tbl_master_LeaveScheme", "ls_id, ls_name  ", null, 2, "ls_name");
                clsdropdown.AddDataToDropDownList(Data, cmbLeaveP);
                cmbOrganization.Items.Insert(0, new ListItem("--Select--", "0"));
                cmbJobResponse.Items.Insert(0, new ListItem("--Select--", "0"));
                cmbBranch.Items.Insert(0, new ListItem("--Select--", "0"));
                cmbDesg.Items.Insert(0, new ListItem("--Select--", "0"));
                EmpType.Items.Insert(0, new ListItem("--Select--", "0"));
                cmbDept.Items.Insert(0, new ListItem("--Select--", "0"));
                cmbWorkingHr.Items.Insert(0, new ListItem("--Select--", "0"));
                cmbLeaveP.Items.Insert(0, new ListItem("--Select--", "0"));
                PopulateBasicEmployeeInfo();

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            

            //------------------
            string EmployeeId = Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]);
            string[] LoggedInStatus = oDBEngine.GetFieldValue1("tbl_master_user", "user_status", " user_Contactid='" + EmployeeId + "'", 1);

            if (LoggedInStatus[0] == "1")
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Associated user already logged in. To do any change user need to sign out from the system.')</script>", false);

            }
            else
            {
                string ReportTo = string.Empty;
                string ReportHead = string.Empty;
                string Colleague = string.Empty;
                string popupScript = "";
                // string query = Request.QueryString["link"].ToString();
                string query = Convert.ToString(Request.QueryString["link"]);
                /*Code  Added and Commented   By Priti on 06122016 to use jquery Choosen*/
                //if (txtReportTo.Text != "")
                //{
                //    if (txtReportTo_hidden.Value != "")
                //    {

                //        ReportTo = txtReportTo_hidden.Value;
                //    }
                //    else
                //        ReportTo = txtReportTo.Text;
                //}
                ReportTo = txtReportTo_hidden.Value;
                //if (txtColleague.Text != "")
                //{
                //    if (txtColleague_hidden.Value != "")
                //    {

                //        Colleague = txtColleague_hidden.Value;
                //    }
                //    else
                //        Colleague = txtColleague.Text;
                //}
                Colleague = txtColleague_hidden.Value;

                //if (txtAReportHead.Text != "")
                //{
                //    if (txtAReportHead_hidden.Value != "")
                //    {

                //        ReportHead = txtAReportHead_hidden.Value;
                //    }
                //    else
                //         ReportHead = txtAReportHead.Text;
                //}
                ReportHead = txtAReportHead_hidden.Value;
                //.........code end..........


                if (Request.QueryString["id"] != "ADD")
                {
                    string empid = Convert.ToString(Request.QueryString["id"]);

                    //String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    String con = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    SqlConnection lcon = new SqlConnection(con);
                    lcon.Open();
                    SqlCommand lcmdEmplInsert = new SqlCommand("HR_Update_EmployeeCTC", lcon);
                    lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                    //lcmdEmplInsert.Parameters.AddWithValue("@emp_cntId", HttpContext.Current.Session["KeyVal_InternalID"].ToString());
                    if (JoiningDate.Value != null)
                    {
                        lcmdEmplInsert.Parameters.AddWithValue("@emp_dateofJoining", JoiningDate.Value);
                    }
                    else
                    {
                        lcmdEmplInsert.Parameters.AddWithValue("@emp_dateofJoining", "");
                    }

                    if (txtCarAl.Text == "")
                    {
                        txtCarAl.Text = "0.0";
                    }
                    if (txtUniform.Text == "")
                    {
                        txtUniform.Text = "0.0";
                    }
                    if (txtBook.Text == "")
                    {
                        txtBook.Text = "0.0";
                    }
                    if (txtSeminar.Text == "")
                    {
                        txtSeminar.Text = "0.0";
                    }
                    if (txtOther.Text == "")
                    {
                        txtOther.Text = "0.0";
                    }
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_organization", cmbOrganization.SelectedItem.Value);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_JobResponsibility", cmbJobResponse.SelectedItem.Value);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_Designation", cmbDesg.SelectedItem.Value);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_type", EmpType.SelectedItem.Value);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_Department", cmbDept.SelectedItem.Value);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_reportTo", ReportTo);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_deputy", ReportHead);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_colleague", Colleague);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_workinghours", cmbWorkingHr.SelectedItem.Value);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_currentCTC", txtCTC.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_basic", txtBasic.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_HRA", txtHRA.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_CCA", txtCCA.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_spAllowance", txtSpAl.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_childrenAllowance", txtChAL.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_totalLeavePA", cmbLeaveP.SelectedItem.Value);//-------------------------
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_PF", txtPf.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_medicalAllowance", txtMedAl.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_LTA", txtLTA.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_convence", txtConvence.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_mobilePhoneExp", txtMbAl.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_totalMedicalLeavePA", "");//-------------------
                    //lcmdEmplInsert.Parameters.AddWithValue("@userid", HttpContext.Current.Session["userid"]);
                    if (Convert.ToString(HttpContext.Current.Session["userid"]) != null)
                    {
                        lcmdEmplInsert.Parameters.AddWithValue("@userid", HttpContext.Current.Session["userid"]);
                    }
                    lcmdEmplInsert.Parameters.AddWithValue("@Id", empid);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_branch", cmbBranch.SelectedItem.Value);
                    if (cmbLeaveEff.Value != null)
                    {
                        lcmdEmplInsert.Parameters.AddWithValue("@emp_LeaveSchemeAppliedFrom", cmbLeaveEff.Value);
                    }
                    else
                    {
                        lcmdEmplInsert.Parameters.AddWithValue("@emp_LeaveSchemeAppliedFrom", "");
                    }

                    if (Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]) != null)
                    {
                        lcmdEmplInsert.Parameters.AddWithValue("@emp_cntId", Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]));
                    }
                    //lcmdEmplInsert.Parameters.AddWithValue("@emp_cntId", HttpContext.Current.Session["KeyVal_InternalID"].ToString());
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_Remarks", txtRemarks.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@EMP_CarAllowance", txtCarAl.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@EMP_UniformAllowance", txtUniform.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@EMP_BooksPeriodicals", txtBook.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@EMP_SeminarAllowance", txtSeminar.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@EMP_OtherAllowance", txtOther.Text);
                    lcmdEmplInsert.ExecuteNonQuery();
                    // Mantis Issue 24802
                    if (lcon.State == ConnectionState.Open)
                    {
                        lcon.Close();
                    }
                    // End of Mantis Issue 24802

                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Successfully Uploaded.')</script>", false);

                    //popupScript = "<script language='javascript'>" + "jAlert('Successfully Uploaded');window.parent.location.href='" + query + "';window.parent.popup.Hide();</script>";
                    popupScript = "<script language='javascript'>" + "window.parent.location.href='" + query + "';window.parent.popup.Hide();</script>";

                    ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);

                }
                else
                {
                    if (txtCarAl.Text == "")
                    {
                        txtCarAl.Text = "0.0";
                    }
                    if (txtUniform.Text == "")
                    {
                        txtUniform.Text = "0.0";
                    }
                    if (txtBook.Text == "")
                    {
                        txtBook.Text = "0.0";
                    }
                    if (txtSeminar.Text == "")
                    {
                        txtSeminar.Text = "0.0";
                    }
                    if (txtOther.Text == "")
                    {
                        txtOther.Text = "0.0";
                    }
                    //String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    String con = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    SqlConnection lcon = new SqlConnection(con);
                    lcon.Open();
                    SqlCommand lcmdEmplInsert = new SqlCommand("EmployeeCTCInsert", lcon);
                    lcmdEmplInsert.CommandType = CommandType.StoredProcedure;
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_cntId", Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]));
                    if (JoiningDate.Value != null)
                    {
                        lcmdEmplInsert.Parameters.AddWithValue("@emp_dateofJoining", JoiningDate.Value);
                    }
                    else
                    {
                        lcmdEmplInsert.Parameters.AddWithValue("@emp_dateofJoining", "");
                    }
                    // lcmdEmplInsert.Parameters.AddWithValue("@emp_dateofJoining", txtSelectionID_hidden.Value);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_Organization", cmbOrganization.SelectedItem.Value);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_JobResponsibility", cmbJobResponse.SelectedItem.Value);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_Designation", cmbDesg.SelectedItem.Value);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_type", EmpType.SelectedItem.Value);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_Department", cmbDept.SelectedItem.Value);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_reportTo", ReportTo);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_deputy", ReportHead);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_colleague", Colleague);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_workinghours", cmbWorkingHr.SelectedItem.Value);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_currentCTC", txtCTC.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_basic", txtBasic.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_HRA", txtHRA.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_CCA", txtCCA.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_spAllowance", txtSpAl.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_childrenAllowance", txtChAL.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_totalLeavePA", cmbLeaveP.SelectedItem.Value);//-------------------------
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_PF", txtPf.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_medicalAllowance", txtMedAl.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_LTA", txtLTA.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_convence", txtConvence.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_mobilePhoneExp", txtMbAl.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_totalMedicalLeavePA", "");//-------------------
                    lcmdEmplInsert.Parameters.AddWithValue("@userid", HttpContext.Current.Session["userid"]);
                    if (cmbLeaveEff.Value != null)
                    {
                        lcmdEmplInsert.Parameters.AddWithValue("@emp_LeaveSchemeAppliedFrom", cmbLeaveEff.Value);
                    }
                    else
                    {
                        lcmdEmplInsert.Parameters.AddWithValue("@emp_LeaveSchemeAppliedFrom", "");
                    }

                    // lcmdEmplInsert.Parameters.AddWithValue("@emp_LeaveSchemeAppliedFrom", txtSettPr.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_branch", cmbBranch.SelectedItem.Value);
                    lcmdEmplInsert.Parameters.AddWithValue("@emp_Remarks", txtRemarks.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@EMP_CarAllowance", txtCarAl.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@EMP_UniformAllowance", txtUniform.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@EMP_BooksPeriodicals", txtBook.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@EMP_SeminarAllowance", txtSeminar.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@EMP_OtherAllowance", txtOther.Text);
                    lcmdEmplInsert.ExecuteNonQuery();
                    // Mantis Issue 24802
                    if (lcon.State == ConnectionState.Open)
                    {
                        lcon.Close();
                    }
                    // End of Mantis Issue 24802
                 
                    popupScript = "<script language='javascript'>" + "jAlert('Successfully Updated');window.parent.location.href='" + query + "';window.parent.popup.Hide();</script>";
                    ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
                }

              
                if (EmpType.SelectedItem.Value == "19")
                {
                    string contactPrefix = string.Empty;
                    switch (EmpType.SelectedItem.Value)
                    {                        
                        case "19":
                            contactPrefix = "DV";
                            break;

                    }
                    //String con = ConfigurationSettings.AppSettings["DBConnectionDefault"];MULTI
                    String con = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    SqlConnection lcon = new SqlConnection(con);
                    lcon.Open();
                    SqlCommand lcmdContactType = new SqlCommand("prc_InsertUpdateContactTypeOnEmpType", lcon);
                    lcmdContactType.CommandType = CommandType.StoredProcedure;
                    lcmdContactType.Parameters.AddWithValue("@InternalID", Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]));
                    lcmdContactType.Parameters.AddWithValue("@ContactID", ddlContactType.SelectedValue);
                    lcmdContactType.Parameters.AddWithValue("@ContactTypePrefix", contactPrefix);
                    lcmdContactType.ExecuteNonQuery();
                    // Mantis Issue 24802
                    if (lcon.State == ConnectionState.Open)
                    {
                        lcon.Close();
                    }
                    // End of Mantis Issue 24802
                    if (string.IsNullOrEmpty(ddlContactType.SelectedValue))
                    {
                        Employee_BL objEmployee_BL = new Employee_BL();
                        objEmployee_BL.DeleteContactType(Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]));
                    }
                }
            }
        }

        public void DisbledCTC()
        {

            JoiningDate.Enabled = false;
            cmbOrganization.Enabled = false;
            cmbJobResponse.Enabled = false;
            cmbBranch.Enabled = false;
            cmbDesg.Enabled = false;
            EmpType.Enabled = false;
            cmbDept.Enabled = false;
            /*Code  Added  By Priti on 06122016 to use jquery Choosen*/

            lstReportTo.Enabled = false;
            lstColleague.Enabled = false;
            lstReportHead.Enabled = false;
            //txtReportTo.Enabled = false;
            // txtAReportHead.Enabled = false;
            // txtColleague.Enabled = false;
            //........end........
            lstReportTo.Enabled = false;
            lstColleague.Enabled = false;
            lstReportHead.Enabled = false;

            txtCTC.Enabled = false;
            txtBasic.Enabled = false;
            txtHRA.Enabled = false;
            txtCCA.Enabled = false;
            txtSpAl.Enabled = false;
            txtChAL.Enabled = false;
            txtPf.Enabled = false;
            txtMedAl.Enabled = false;
            txtLTA.Enabled = false;
            txtConvence.Enabled = false;
            txtMbAl.Enabled = false;
            txtCarAl.Enabled = false;
            txtUniform.Enabled = false;
            txtBook.Enabled = false;
            txtSeminar.Enabled = false;
            txtOther.Enabled = false;
            cmbWorkingHr.Enabled = false;
            cmbLeaveP.Enabled = false;
            cmbLeaveEff.Enabled = false;
            txtRemarks.Enabled = false;

            btnSave.Enabled = false;



        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

            string popupScript = "";
            string query = Convert.ToString(Request.QueryString["link"]);

            popupScript = "<script language='javascript'>" + "window.parent.location.href='" + query + "';window.parent.popup.Hide();</script>";
            ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);

        }



        public void PopulateBasicEmployeeInfo()
        {
            string popupScript = "";
            //string query = Convert.ToString(Request.QueryString["link"]);
            //int Unqid_empCTC = Convert.ToInt32(Convert.ToString(Request.QueryString["id"]));
            //string[,] InternalId;
            //HttpContext.Current.Session["KeyVal_InternalID"] = InternalId[0, 0];
            string[,] ContactData;
            string[,] CntExitEmp;
            ContactData = oDBEngine.GetFieldValue("tbl_trans_employeeCTC A LEFT JOIN tbl_master_designation ON A.emp_Designation = tbl_master_designation.deg_id LEFT JOIN tbl_master_costCenter ON A.emp_Department = tbl_master_costCenter.cost_id LEFT JOIN tbl_master_company ON A.emp_Organization = tbl_master_company.cmp_id  LEFT JOIN tbl_master_employee ON A.emp_cntId = tbl_master_employee.emp_contactId ", "A.emp_effectiveDate AS EffectiveDate,convert(varchar(11),A.emp_effectiveDate,113) AS EffectiveDate1,convert(varchar(11),A.emp_effectiveuntil,113) AS EffectiveUntil1, A.emp_effectiveuntil AS EffectiveUntil, A.emp_id AS Id, tbl_master_designation.deg_designation AS Designation, tbl_master_costCenter.cost_description AS Department, tbl_master_company.cmp_Name AS CompanyName, A.emp_id, A.emp_cntId, A.emp_effectiveDate as emp_dateofJoining, A.emp_effectiveuntil, A.emp_Organization, A.emp_JobResponsibility, A.emp_Designation, (select branch_description from tbl_master_branch where branch_id= A.emp_branch) as BranchName,	A.emp_branch,A.emp_LeaveSchemeAppliedFrom,A.emp_type, A.emp_Department, A.emp_reportTo, A.emp_deputy, A.emp_colleague, A.emp_workinghours, A.emp_currentCTC, A.emp_basic, A.emp_HRA, A.emp_CCA, A.emp_spAllowance, A.emp_childrenAllowance, A.emp_totalLeavePA, A.emp_PF, A.emp_medicalAllowance, A.emp_LTA, A.emp_convence, A.emp_mobilePhoneExp,A.EMP_CarAllowance, A.EMP_UniformAllowance,A.EMP_BooksPeriodicals ,A.EMP_SeminarAllowance ,A.EMP_OtherAllowance,A.emp_totalMedicalLeavePA, A.CreateDate, A.CreateUser, A.LastModifyDate, A.LastModifyUser,A.emp_Remarks", "  A.emp_id =(select max(ec.emp_id) from tbl_trans_employeeCTC ec where emp_cntId= '" + Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]) + "') ", 47);

            /// Coded By Samrat Roy -- 18/04/2017 
            /// To Genarate Contact Type on Employee Type Selection(DME/ISD)
            PopulateContactType(ContactData);
            ///////// ############################

            string[,] DataOrg = oDBEngine.GetFieldValue("tbl_master_Company", "cmp_id, cmp_name", null, 2, "cmp_name");
            if (ContactData[0, 12] != "")
            {
                clsdropdown.AddDataToDropDownList(DataOrg, cmbOrganization, Int32.Parse(ContactData[0, 12]));
            }
            else
            {

                clsdropdown.AddDataToDropDownList(DataOrg, cmbOrganization, 0);
            }
            string[,] DataJR = oDBEngine.GetFieldValue("tbl_master_jobresponsibility", "job_id, job_responsibility", null, 2, "job_responsibility");
            if (ContactData[0, 13] != "")
            {
                clsdropdown.AddDataToDropDownList(DataJR, cmbJobResponse, Int32.Parse(ContactData[0, 13]));
            }
            else
            {

                clsdropdown.AddDataToDropDownList(DataJR, cmbJobResponse, 0);
            }
            string[,] DataBr = oDBEngine.GetFieldValue("tbl_master_branch", "branch_id, branch_description ", null, 2, "branch_description");
            //if (ContactData[0, 16] != "")
            //{
            //    clsdropdown.AddDataToDropDownList(DataBr, cmbBranch, Int32.Parse(ContactData[0, 16]));
            //}
            //else
            //{

            //    clsdropdown.AddDataToDropDownList(DataBr, cmbBranch, 0);
            //}
            string[,] DataDSG = oDBEngine.GetFieldValue("tbl_master_Designation", "deg_id, deg_designation ", null, 2, "deg_designation");
            if (ContactData[0, 14] != "")
            {
                clsdropdown.AddDataToDropDownList(DataDSG, cmbDesg, Int32.Parse(ContactData[0, 14]));
            }
            else
            {

                clsdropdown.AddDataToDropDownList(DataDSG, cmbDesg, 0);
            }

            string[,] DataETYPE = oDBEngine.GetFieldValue("tbl_master_employeeType", "emptpy_id, emptpy_type ", null, 2, "emptpy_type");
            if (ContactData[0, 18] != "")
            {
                clsdropdown.AddDataToDropDownList(DataETYPE, EmpType, Int32.Parse(ContactData[0, 18]));
            }
            else
            {

                clsdropdown.AddDataToDropDownList(DataETYPE, EmpType, 0);
            }

            string[,] DataCC = oDBEngine.GetFieldValue("tbl_master_costCenter", "cost_id, cost_description ", " cost_costCenterType = 'department' ", 2, "cost_description");
            if (ContactData[0, 19] != "")
            {
                clsdropdown.AddDataToDropDownList(DataCC, cmbDept, Int32.Parse(ContactData[0, 19]));
            }
            else
            {

                clsdropdown.AddDataToDropDownList(DataCC, cmbDept, 0);
            }
            string[,] DataWH = oDBEngine.GetFieldValue("tbl_EmpWorkingHours", "Id,Name  ", null, 2, "Name");
            if (ContactData[0, 23] != "")
            {
                clsdropdown.AddDataToDropDownList(DataWH, cmbWorkingHr, Int32.Parse(ContactData[0, 23]));
            }
            else
            {

                clsdropdown.AddDataToDropDownList(DataWH, cmbWorkingHr, 0);
            }

            string[,] DataLVS = oDBEngine.GetFieldValue("tbl_master_LeaveScheme", "ls_id, ls_name  ", null, 2, "ls_name");
            if (ContactData[0, 30] != "")
            {
                clsdropdown.AddDataToDropDownList(DataLVS, cmbLeaveP, Int32.Parse(ContactData[0, 30]));
            }
            else
            {

                clsdropdown.AddDataToDropDownList(DataLVS, cmbLeaveP, 0);
            }

            //txtCTC.Text = ContactData[0, 24];
            //txtBasic.Text = ContactData[0, 25];

            //txtHRA.Text = ContactData[0, 26];
            //txtCCA.Text = ContactData[0, 27];

            //txtSpAl.Text = ContactData[0, 28];
            //txtChAL.Text = ContactData[0, 29];

            //txtPf.Text = ContactData[0, 31];
            //txtMedAl.Text = ContactData[0, 32];

            //txtLTA.Text = ContactData[0, 33];
            //txtConvence.Text = ContactData[0, 34];

            //txtMbAl.Text = ContactData[0, 35];
            //txtCarAl.Text = ContactData[0, 36];

            //txtUniform.Text = ContactData[0, 37];
            //txtBook.Text = ContactData[0, 38];

            //txtSeminar.Text = ContactData[0, 39];
            //txtOther.Text = ContactData[0, 40];

            //txtRemarks.Text = ContactData[0, 46];



            string[,] DataRT = oDBEngine.GetFieldValue(" tbl_master_employee, tbl_master_contact", " ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name ", " tbl_master_employee.emp_contactId = tbl_master_contact.cnt_internalId and  tbl_master_employee.emp_id ='" + ContactData[0, 20].ToString() + "'", 1);
            if (DataRT[0, 0] != "n")
            {
                // txtReportTo.Text = DataRT[0, 0];
                txtReportTo_hidden.Value = ContactData[0, 20].ToString();
                lstReportTo.Text = ContactData[0, 20].ToString();
            }
            else
            {
                lstReportTo.Text = ContactData[0, 20].ToString();
                // txtReportTo.Text = ContactData[0, 20].ToString();
            }

            string[,] DataRH = oDBEngine.GetFieldValue(" tbl_master_employee, tbl_master_contact", " ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name ", " tbl_master_employee.emp_contactId = tbl_master_contact.cnt_internalId and  tbl_master_employee.emp_id ='" + ContactData[0, 21].ToString() + "'", 1);
            if (DataRH[0, 0] != "n")
            {
                //txtAReportHead.Text = DataRH[0, 0];
                txtAReportHead_hidden.Value = ContactData[0, 21].ToString();
                lstReportHead.Text = ContactData[0, 21].ToString();
            }
            else
            {
                // txtAReportHead.Text = ContactData[0, 21].ToString();
                lstReportHead.Text = ContactData[0, 21].ToString();
            }

            string[,] DataCLG = oDBEngine.GetFieldValue(" tbl_master_employee, tbl_master_contact", " ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name ", " tbl_master_employee.emp_contactId = tbl_master_contact.cnt_internalId and  tbl_master_employee.emp_id ='" + ContactData[0, 22].ToString() + "'", 1);
            if (DataCLG[0, 0] != "n")
            {
                //txtColleague.Text = DataCLG[0, 0];
                txtColleague_hidden.Value = ContactData[0, 22].ToString();
                lstColleague.Text = ContactData[0, 22].ToString();
            }
            else
            {
                //txtColleague.Text = ContactData[0, 22].ToString();
                lstColleague.Text = ContactData[0, 22].ToString();
            }
        }

        /// Coded By Samrat Roy -- 18/04/2017 
        /// To Get Contact Type selection on Employee Type (DME/ISD)
        private void PopulateContactType(string[,] ContactData)
        {
           
            if (ContactData[0, 18] == "19")
            {
                divContactType.Visible = true;
                string contactPrefix = string.Empty;
                string lblContactTypeText = string.Empty;
                switch (ContactData[0, 18])
                {                  
                    case "19":
                        contactPrefix = "DV";
                        lblContactType.Text = "Vendor";
                        break;

                }

                Employee_AddNew_BL objEmployee_AddNew_BL = new Employee_AddNew_BL();
                DataTable ContactDt = objEmployee_AddNew_BL.GetContactTypeonEmployeeType(contactPrefix);



                string[,] ContactMappedData;
                ContactMappedData = oDBEngine.GetFieldValue("tbl_trans_EmployeeTypeMapping", "Emp_ContactId as ContactID", "Emp_EmpInternalId = '" + ContactData[0, 9] + "'", 1);

                ddlContactType.DataTextField = "Name";
                ddlContactType.DataValueField = "ID";
                ddlContactType.DataSource = ContactDt;
                ddlContactType.DataBind();
                ddlContactType.Items.Insert(0, new ListItem("--Select--", ""));
                ddlContactType.SelectedValue = (ContactMappedData[0, 0]);
            }
            else
            {
                divContactType.Visible = false;
            }

            ////############################################################/////
        }


    }
}