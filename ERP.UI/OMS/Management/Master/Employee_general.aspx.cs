using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using DevExpress.Web.ASPxTabControl;
using DevExpress.Web;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;
using ERP.OMS.CustomFunctions;
using System.Web.Services;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_Employee_general : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        Int32 ID;
        public string WLanguage = "";
        public string SpLanguage = "";
        // Tier architecture
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]); MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        //Converter Oconverter = new Converter();
        BusinessLogicLayer.Converter Oconverter = new BusinessLogicLayer.Converter();
        clsDropDownList OclsDropDownList = new clsDropDownList();
        CommonBL cbl = new CommonBL();
        static string CntID = string.Empty;

        SelectListOptions objSelectListOptions = new SelectListOptions();

        protected override void OnPreInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] == "ADD")
                {

                    TabVisibility("Off");
                    base.OnPreInit(e);
                }
            }


        }

        protected int getUdfCount()
        {
            DataTable udfCount = oDBEngine.GetDataTable("select 1 from tbl_master_remarksCategory rc where cat_applicablefor='Em'   and ( exists (select * from tbl_master_udfGroup where id=rc.cat_group_id and grp_isVisible=1) or rc.cat_group_id=0)");
            return udfCount.Rows.Count;
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            lblfilesize.Visible = false;
            if (!IsPostBack)
            {
                string IsSTBManagementRequired = cbl.GetSystemSettingsResult("IsSTBManagementRequired");
                //Mantis Issue 0025113
                string SMSRequiredInDirectorApproval = cbl.GetSystemSettingsResult("SMSRequiredInDirectorApproval");
                //End of Mantis Issue 250113
                if (!String.IsNullOrEmpty(IsSTBManagementRequired))
                {
                    //Mantis Issue 0025113
                    //if (IsSTBManagementRequired.ToUpper().Trim() == "YES")
                    if (IsSTBManagementRequired.ToUpper().Trim() == "YES" || SMSRequiredInDirectorApproval.ToUpper().Trim() == "YES")
                    //End of Mantis Issue 250113
                    {
                        lblchkIsDirector.Style.Add("display", "!inline-block");
                        DivchkIsDirector.Style.Add("display", "!inline-block");

                    }
                    //Mantis Issue 0025113
                    //else if (IsSTBManagementRequired.ToUpper().Trim() == "NO")
                    else if (IsSTBManagementRequired.ToUpper().Trim() == "NO" && SMSRequiredInDirectorApproval.ToUpper().Trim() == "NO")
                    //End of Mantis Issue 250113
                    {
                        lblchkIsDirector.Style.Add("display", "none");
                        DivchkIsDirector.Style.Add("display", "none");
                    }
                }

                #region Check Payroll Active Or Not

                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
                DataTable ConfigDT = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='IsPayrollActive' AND IsActive=1");
                if (ConfigDT != null && ConfigDT.Rows.Count > 0)
                {
                    string IsMandatory = Convert.ToString(ConfigDT.Rows[0]["Variable_Value"]).Trim();
                    if (IsMandatory == "Yes")
                    {
                        ASPxPageControl1.TabPages.FindByName("StautoryDocumentDetails").Visible = true;
                        ASPxPageControl1.TabPages.FindByName("PFESI").Visible = true;
                        ASPxPageControl1.TabPages.FindByName("othrdtls").Visible = false;
                    }
                    else
                    {
                        ASPxPageControl1.TabPages.FindByName("StautoryDocumentDetails").Visible = false;
                        ASPxPageControl1.TabPages.FindByName("PFESI").Visible = false;
                        ASPxPageControl1.TabPages.FindByName("othrdtls").Visible = false;
                    }
                }
                else
                {
                    ASPxPageControl1.TabPages.FindByName("StautoryDocumentDetails").Visible = false;
                    ASPxPageControl1.TabPages.FindByName("PFESI").Visible = false;
                    ASPxPageControl1.TabPages.FindByName("othrdtls").Visible = false;
                }

                #endregion

                hdnimagesrc.Value = "";
                lblfilesize.Visible = false;
                IsUdfpresent.Value = Convert.ToString(getUdfCount());

                objSelectListOptions = new SelectListOptions();
                DataTable DT = new DataTable();
                var gender = cmbGender.SelectedItem.Value;
                gender = cmbGender.SelectedValue;
                string reqStr = "%";
                // DT = oDBEngine.GetDataTable("tbl_master_contact con,tbl_master_branch b", " Top 10 (ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') + ' [' + ISNULL(con.cnt_shortName, '')+']'+'[' + b.branch_description + ']') AS cnt_firstName,con.cnt_internalId as cnt_internalId", " (cnt_contactType='EM') and cnt_firstName Like '" + reqStr.Trim() + "%'  and con.cnt_branchid=b.branch_id");
                DT = objSelectListOptions.getRefrred(gender, Session["KeyVal_InternalID"]);
                ddlReferedBy.DataSource = DT;
                ddlReferedBy.DataValueField = "cnt_internalId";
                ddlReferedBy.DataTextField = "cnt_firstName";
                ddlReferedBy.DataBind();

                VendorTDSBl tdsdetails = new VendorTDSBl(); 
                DataTable tdsMaster = tdsdetails.GetTDSMASTERLIST();
                if (tdsMaster != null && tdsMaster.Rows.Count > 0)
                {
                    cmbTDS.TextField = "TYPE_NAME";
                    cmbTDS.ValueField = "ID";
                    cmbTDS.DataSource = tdsMaster;
                    cmbTDS.DataBind();
                }

                ddlReferedBy.Items.Insert(0, "");
                DOBDate.UseMaskBehavior = true;
                DOBDate.EditFormatString = Oconverter.GetDateFormat();
                DOBDate.EditFormatString = "dd-MM-yyyy";
                #region Rajdip
                txtjoinningdate.UseMaskBehavior = true;
                txtjoinningdate.EditFormatString = Oconverter.GetDateFormat();
                txtjoinningdate.EditFormatString = "dd-MM-yyyy";
                txtconfirmationdate.UseMaskBehavior = true;
                txtconfirmationdate.EditFormatString = Oconverter.GetDateFormat();
                txtconfirmationdate.EditFormatString = "dd-MM-yyyy";
                #endregion Rajdip
                AnniversaryDate.UseMaskBehavior = true;
                AnniversaryDate.EditFormatString = Oconverter.GetDateFormat();
                AnniversaryDate.EditFormatString = "dd-MM-yyyy";


                string previousPageUrl = string.Empty;
                if (Request.UrlReferrer != null)
                    previousPageUrl = Request.UrlReferrer.AbsoluteUri;
                else
                    previousPageUrl = Page.ResolveUrl("~/OMS/Management/ProjectMainPage.aspx");

                ViewState["previousPageUrl"] = previousPageUrl;
                // goBackCrossBtn.NavigateUrl = previousPageUrl;
                //DOBDate.UseMaskBehavior = true;
                //DOBDate.EditFormatString = Oconverter.GetDateFormat();
                //AnniversaryDate.UseMaskBehavior = true;
                //AnniversaryDate.EditFormatString = Oconverter.GetDateFormat();

                ShowForm();
                //txtReferedBy.Attributes.Add("onkeyup", "CallList(this,'referedby',event)");
                //txtReferedBy.Attributes.Add("onfocus", "CallList(this,'referedby',event)");
                //txtReferedBy.Attributes.Add("onclick", "CallList(this,'referedby',event)");
                Session["ContactType"] = "employee";
                string[,] EmployeeNameID = oDBEngine.GetFieldValue(" tbl_master_contact ", " case when cnt_firstName is null then '' else cnt_firstName end + ' '+case when cnt_middleName is null then '' else cnt_middleName end+ ' '+case when cnt_lastName is null then '' else cnt_lastName end+' ['+cnt_shortName+']' as name ", " cnt_internalId='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'", 1);
                if (EmployeeNameID[0, 0] != "n")
                {
                    lblHeader.Text = EmployeeNameID[0, 0].ToUpper();
                }
                cmbSource.Attributes.Add("onchange", "FreeHiddenField()");
            }
            if (!ClientScript.IsStartupScriptRegistered("Today"))
                ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>AtTheTimePageLoad();</script>");
        }
        [WebMethod]
        public static List<string> GetReferredBy(string firstname)
        {
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]); MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = new DataTable();
            DT.Rows.Clear();
            DT = oDBEngine.GetDataTable("tbl_master_contact con,tbl_master_branch b", "  (ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') + ' [' + ISNULL(con.cnt_shortName, '')+']'+'[' + b.branch_description + ']') AS cnt_firstName,con.cnt_internalId as cnt_internalId", " (cnt_contactType='EM') and cnt_firstName Like '" + firstname + "%'  and con.cnt_branchid=b.branch_id");
            List<string> obj = new List<string>();
            foreach (DataRow dr in DT.Rows)
            {

                obj.Add(Convert.ToString(dr["cnt_firstName"]) + "|" + Convert.ToString(dr["cnt_internalId"]));
            }
            return obj;
            //    Response.Write("No Record Found###No Record Found|");


        }
        private void ShowForm()
        {

            if (Request.QueryString["id"] != "ADD")
            {
                TrLang.Visible = true;
                if (Request.QueryString["id"] != null && Convert.ToString(Request.QueryString["id"]) != "")
                {
                    ID = Int32.Parse(Request.QueryString["id"]);
                    HttpContext.Current.Session["KeyVal"] = ID;
                }
                string[,] InternalId;

                if (ID != 0)
                {
                    InternalId = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_internalId", "cnt_id=" + ID, 1);
                }
                else
                {
                    InternalId = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_internalId", "cnt_id=" + HttpContext.Current.Session["KeyVal"], 1);
                }
                // if emp code is  system generated txtAliasName should be un editable #ag18102016


                if (InternalId[0, 0].Trim().StartsWith("EM"))
                {
                   // txtAliasName.Enabled = false;
                    LinkButton1.Enabled = false;
                }
                else
                {
                   // txtAliasName.Enabled = true;
                    LinkButton1.Enabled = true;
                }
                // if emp code is  system generated txtAliasName should be un editable #ag18102016k
                HttpContext.Current.Session["KeyVal_InternalID"] = InternalId[0, 0];
                LanGuage();
                string[,] ContactData;
                string[,] TDSTYpe;
                string[,] IsDirector;
                if (ID != 0)
                {
                    ContactData = oDBEngine.GetFieldValue("tbl_master_contact",
                                            "cnt_ucc, cnt_salutation,  cnt_firstName, cnt_middleName, cnt_lastName, cnt_shortName, cnt_branchId, cnt_sex, cnt_maritalStatus, cnt_DOB, cnt_anniversaryDate, cnt_legalStatus, cnt_education, cnt_profession, cnt_organization, cnt_jobResponsibility, cnt_designation, cnt_industry, cnt_contactSource, cnt_referedBy, cnt_contactType, cnt_contactStatus,cnt_bloodgroup,WebLogIn,isnull(TDSRATE_TYPE,0) TDSRATE_TYPE",
                                            " cnt_id=" + ID, 24);
                    TDSTYpe = oDBEngine.GetFieldValue("tbl_master_contact","isnull(TDSRATE_TYPE,0) TDSRATE_TYPE","cnt_id=" + ID,1);

                    IsDirector = oDBEngine.GetFieldValue("tbl_master_contact", "Is_Director", "cnt_id=" + ID, 1);
                }
                else
                {
                    ContactData = oDBEngine.GetFieldValue("tbl_master_contact",
                                            "cnt_ucc, cnt_salutation,  cnt_firstName, cnt_middleName, cnt_lastName, cnt_shortName, cnt_branchId, cnt_sex, cnt_maritalStatus,cnt_DOB, cnt_anniversaryDate, cnt_legalStatus, cnt_education, cnt_profession, cnt_organization, cnt_jobResponsibility, cnt_designation, cnt_industry, cnt_contactSource, cnt_referedBy, cnt_contactType, cnt_contactStatus,cnt_bloodgroup,WebLogIn,isnull(TDSRATE_TYPE,0) TDSRATE_TYPE",
                                            " cnt_id=" + HttpContext.Current.Session["KeyVal"], 24);
                    TDSTYpe = oDBEngine.GetFieldValue("tbl_master_contact", "isnull(TDSRATE_TYPE,0) TDSRATE_TYPE", "cnt_id=" + HttpContext.Current.Session["KeyVal"], 1);
                    IsDirector = oDBEngine.GetFieldValue("tbl_master_contact", "Is_Director", "cnt_id=" + ID, 1);
                }

                if(TDSTYpe[0,0] !="")
                {
                    cmbTDS.Value = Convert.ToString(TDSTYpe[0, 0]);
                }
                if (IsDirector[0, 0] != "n")
                {

                    if (Convert.ToBoolean(IsDirector[0, 0]))
                    {
                        chkIsDirector.Checked = true;
                    }
                    else
                    {
                        chkIsDirector.Checked = false;
                    }
                }

                
                //____________ Value BINDING and Allocation _______________//

                //txtClentUcc.Text = ContactData[0, 0];
                string[,] Data = oDBEngine.GetFieldValue("tbl_master_salutation", "sal_id, sal_name", null, 2);

                if (ContactData[0, 19] != "")
                {
                    ddlReferedBy.SelectedIndex = ddlReferedBy.Items.IndexOf(ddlReferedBy.Items.FindByValue(ContactData[0, 19].Trim()));
                }
                if (ContactData[0, 1] != "")
                {
                    // oDBEngine.AddDataToDropDownList(Data, CmbSalutation, Int32.Parse(ContactData[0, 1]));
                    OclsDropDownList.AddDataToDropDownList(Data, CmbSalutation, Int32.Parse(ContactData[0, 1]));

                }
                else
                {

                    //oDBEngine.AddDataToDropDownList(Data, CmbSalutation, 0);
                    OclsDropDownList.AddDataToDropDownList(Data, CmbSalutation, 0);
                }

                txtFirstNmae.Text = ContactData[0, 2];
                txtMiddleName.Text = ContactData[0, 3];
                txtLastName.Text = ContactData[0, 4];
                txtAliasName.Text = ContactData[0, 5];
                cmbBloodgroup.SelectedValue = ContactData[0, 22];
                Data = oDBEngine.GetFieldValue("tbl_master_branch", "branch_id, branch_description ", null, 2);
                if (ContactData[0, 6] != "")
                {
                    //oDBEngine.AddDataToDropDownList(Data, cmbBranch, Int32.Parse(ContactData[0, 6]));
                    OclsDropDownList.AddDataToDropDownList(Data, cmbBranch, Int32.Parse(ContactData[0, 6]));
                }
                else
                {
                    //oDBEngine.AddDataToDropDownList(Data, cmbBranch, 0);
                    OclsDropDownList.AddDataToDropDownList(Data, cmbBranch, 0);
                }
                if (ContactData[0, 7] != "")
                {
                    cmbGender.SelectedValue = ContactData[0, 7];
                }
                else
                {
                    cmbGender.SelectedIndex.Equals(0);
                }


                Data = oDBEngine.GetFieldValue(" tbl_master_maritalstatus", " mts_id, mts_maritalStatus", null, 2);

                if (ContactData[0, 8] != "")
                {
                    //oDBEngine.AddDataToDropDownList(Data, cmbMaritalStatus, Int32.Parse(ContactData[0, 8]));
                    OclsDropDownList.AddDataToDropDownList(Data, cmbMaritalStatus, Int32.Parse(ContactData[0, 8]));
                }
                else
                {
                    //oDBEngine.AddDataToDropDownList(Data, cmbMaritalStatus, 0);
                    OclsDropDownList.AddDataToDropDownList(Data, cmbMaritalStatus, 0);
                }
                if (ContactData[0, 9] != "" && ContactData[0, 9] != "01/01/1900" && ContactData[0, 9] != "1/1/1900 12:00:00 AM")
                    DOBDate.Value = Convert.ToDateTime(ContactData[0, 9]);
                else
                    DOBDate.Value = null;
                if (ContactData[0, 10] != "" && ContactData[0, 10] != "01/01/1900" && ContactData[0, 10] != "1/1/1900 12:00:00 AM")
                    AnniversaryDate.Value = Convert.ToDateTime(ContactData[0, 10]);
                else
                    AnniversaryDate.Value = null;
                Data = oDBEngine.GetFieldValue("tbl_master_legalstatus", "lgl_id, lgl_legalStatus", null, 2);

                if (ContactData[0, 11] != "")
                {
                    //oDBEngine.AddDataToDropDownList(Data, cmbLegalStatus, Int32.Parse(ContactData[0, 11]));
                    OclsDropDownList.AddDataToDropDownList(Data, cmbLegalStatus, Int32.Parse(ContactData[0, 11]));
                }
                else
                {
                    //oDBEngine.AddDataToDropDownList(Data, cmbLegalStatus, 0);
                    OclsDropDownList.AddDataToDropDownList(Data, cmbLegalStatus, 0);
                }
                Data = oDBEngine.GetFieldValue("tbl_master_education", "edu_id, edu_education", null, 2);

                if (ContactData[0, 12] != "")
                {
                    //oDBEngine.AddDataToDropDownList(Data, cmbEducation, Int32.Parse(ContactData[0, 12]));
                    OclsDropDownList.AddDataToDropDownList(Data, cmbEducation, Int32.Parse(ContactData[0, 12]));
                }
                else
                {
                    //oDBEngine.AddDataToDropDownList(Data, cmbEducation, 0);
                    OclsDropDownList.AddDataToDropDownList(Data, cmbEducation, 0);
                }
                Data = oDBEngine.GetFieldValue("tbl_master_profession", "pro_id, pro_professionName", null, 2, "pro_professionName");

                
                if (ContactData[0, 13] != "")
                {
                    //oDBEngine.AddDataToDropDownList(Data, cmbProfession, Int32.Parse(ContactData[0, 13]));
                    OclsDropDownList.AddDataToDropDownList(Data, cmbProfession, Int32.Parse(ContactData[0, 13]));
                }
                else
                {
                    //oDBEngine.AddDataToDropDownList(Data, cmbProfession, 0);
                    OclsDropDownList.AddDataToDropDownList(Data, cmbProfession, 0);
                }

                Data = oDBEngine.GetFieldValue(" tbl_master_ContactSource", "cntsrc_id, cntsrc_sourcetype", null, 2);

                if (ContactData[0, 18] != "")
                {
                    //oDBEngine.AddDataToDropDownList(Data, cmbSource, Int32.Parse(ContactData[0, 18]));
                    OclsDropDownList.AddDataToDropDownList(Data, cmbSource, Int32.Parse(ContactData[0, 18]));
                }
                else
                {
                    //oDBEngine.AddDataToDropDownList(Data, cmbSource, 0);
                    OclsDropDownList.AddDataToDropDownList(Data, cmbSource, 0);
                }
                hdReferenceBy.Value = ContactData[0, 19].ToString();
           
                Data = oDBEngine.GetFieldValue(" tbl_master_contact", " (ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') + ' [' + cnt_shortName +']') AS cnt_firstName ", " cnt_internalid='" + ContactData[0, 19].ToString() + "'", 1);
                if (Data[0, 0] != "n")
                {
                    //txtReferedBy.Text = Data[0, 0];
                    txtReferedBy_hidden.Text = ContactData[0, 19].ToString();
                }
                else
                    //txtReferedBy.Text = ContactData[0, 19].ToString();
                if (ContactData[0, 23].ToString() == "Yes")
                    chkAllow.Checked = true;
                cmbBranch.Enabled = false;

                cmbLegalStatus.Items.Insert(0, new ListItem("--Select--", "0"));
                cmbEducation.Items.Insert(0, new ListItem("--Select--", "0"));
                cmbProfession.Items.Insert(0, new ListItem("--Select--", "0"));

                cmbSource.Items.Insert(0, new ListItem("--Select--", "0"));
                cmbMaritalStatus.Items.Insert(0, new ListItem("--Select--", "0"));
                cmbBranch.Items.Insert(0, new ListItem("--Select--", "0"));


                Data = oDBEngine.GetFieldValue(" tbl_master_employee", "Usehierchy,Inactive,UpdateDayPermission", "emp_contactId='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'", 3);

                if (Convert.ToBoolean(Data[0, 0]))
                {
                    chkUsehierchy.Checked = true;
                }
                else {
                    chkUsehierchy.Checked = false;
                }

                if (Convert.ToBoolean(Data[0, 1]))
                {
                    chkInactive.Checked = true;
                }
                else
                {
                    chkInactive.Checked = false;
                }

                txtUpdateDays.Text = Convert.ToString(Data[0, 2]);
                //Rev Rajdip
                DataTable dtGetJoiningandConfirmationdate = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                con.Open();
                //SqlCommand cmd = new SqlCommand("Proc_GetSubLedger", con);
                SqlCommand cmd = new SqlCommand("Prc_GetEmployeeDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@emp_contactId", HttpContext.Current.Session["KeyVal_InternalID"]);
                cmd.Parameters.AddWithValue("@Action", "GetJoiningandConfirmationdate");
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(dtGetJoiningandConfirmationdate);
                
                if (dtGetJoiningandConfirmationdate.Rows.Count>0)
                { 
                if (dtGetJoiningandConfirmationdate.Rows[0]["emp_dateofJoining"].ToString() != "" && dtGetJoiningandConfirmationdate.Rows[0]["emp_dateofJoining"].ToString() != null && dtGetJoiningandConfirmationdate.Rows[0]["emp_dateofJoining"]!=DBNull.Value)
                {
                    txtjoinningdate.Value = Convert.ToDateTime(dtGetJoiningandConfirmationdate.Rows[0]["emp_dateofJoining"]);
                }
                if (dtGetJoiningandConfirmationdate.Rows[0]["Date_Of_Confirmation"].ToString() != "" && dtGetJoiningandConfirmationdate.Rows[0]["Date_Of_Confirmation"].ToString() != null && dtGetJoiningandConfirmationdate.Rows[0]["Date_Of_Confirmation"] != DBNull.Value)
                {
                    txtconfirmationdate.Value = Convert.ToDateTime(dtGetJoiningandConfirmationdate.Rows[0]["Date_Of_Confirmation"]);
                }
                if (dtGetJoiningandConfirmationdate.Rows[0]["emp_image"].ToString() != "" && dtGetJoiningandConfirmationdate.Rows[0]["emp_image"].ToString() != null && dtGetJoiningandConfirmationdate.Rows[0]["emp_image"] != DBNull.Value)
                {
                    string UploadFileDirectory = System.Web.Configuration.WebConfigurationManager.AppSettings["SaveFile"].ToString();
                    imageResult.Src = "~/" + UploadFileDirectory + dtGetJoiningandConfirmationdate.Rows[0]["emp_image"].ToString();
                    hdnimagesrc.Value = dtGetJoiningandConfirmationdate.Rows[0]["emp_image"].ToString();
                    //imageResult.Attributes["src"] = dtGetJoiningandConfirmationdate.Rows[0]["emp_image"].ToString();
                }
                }
                DataTable dtgetname = new DataTable();
                //SqlCommand cmd = new SqlCommand("Proc_GetSubLedger", con);
                SqlCommand cmdgetname = new SqlCommand("Prc_GetEmployeeDetails", con);
                cmdgetname.CommandType = CommandType.StoredProcedure;
                cmdgetname.Parameters.AddWithValue("@emp_contactId", HttpContext.Current.Session["KeyVal_InternalID"]);
                cmdgetname.Parameters.AddWithValue("@Action", "GetName");
                cmdgetname.CommandTimeout = 0;
                SqlDataAdapter dagetname = new SqlDataAdapter();
                dagetname.SelectCommand = cmdgetname;
                dagetname.Fill(dtgetname);

                if (dtgetname.Rows.Count > 0) { 
                if (dtgetname.Rows[0]["Name"].ToString() != "" && dtgetname.Rows[0]["Name"].ToString() != null && dtgetname.Rows[0]["Name"] != DBNull.Value)
                {
                    lblname.Text = dtgetname.Rows[0]["Name"].ToString();
                }
                }
                DataTable dtgetoccupation = new DataTable();
                SqlCommand cmdgetoccupation = new SqlCommand("Prc_GetEmployeeDetails", con);
                cmdgetoccupation.CommandType = CommandType.StoredProcedure;
                cmdgetoccupation.Parameters.AddWithValue("@emp_contactId", HttpContext.Current.Session["KeyVal_InternalID"]);
                cmdgetoccupation.Parameters.AddWithValue("@Action", "GetOccupation");
                cmdgetoccupation.CommandTimeout = 0;
                SqlDataAdapter dagetoccupation = new SqlDataAdapter();
                dagetoccupation.SelectCommand = cmdgetoccupation;
                dagetoccupation.Fill(dtgetoccupation);
                if (dtgetoccupation.Rows.Count>0)
                { 
                if (dtgetoccupation.Rows[0]["pro_professionName"].ToString() != "" && dtgetoccupation.Rows[0]["pro_professionName"].ToString() != null
                    && dtgetoccupation.Rows[0]["pro_professionName"] != DBNull.Value)
                {
                    lblprofession.Text = dtgetoccupation.Rows[0]["pro_professionName"].ToString();
                }
                }
                DataTable dtgetaddress = new DataTable();
                SqlCommand cmdgetaddress= new SqlCommand("Prc_GetEmployeeDetails", con);
                cmdgetaddress.CommandType = CommandType.StoredProcedure;
                cmdgetaddress.Parameters.AddWithValue("@emp_contactId", HttpContext.Current.Session["KeyVal_InternalID"]);
                cmdgetaddress.Parameters.AddWithValue("@Action", "GetAddress");
                cmdgetaddress.CommandTimeout = 0;
                SqlDataAdapter dagetaddress = new SqlDataAdapter();
                dagetaddress.SelectCommand = cmdgetaddress;
                dagetaddress.Fill(dtgetaddress);
                if(dtgetaddress.Rows.Count>0)
                {
                    if (dtgetaddress.Rows[0]["city_name"].ToString() != "" && dtgetaddress.Rows[0]["city_name"].ToString() != null
                    && dtgetaddress.Rows[0]["city_name"] != DBNull.Value)
                {
                    lbllocation.Text = dtgetaddress.Rows[0]["city_name"].ToString();
                }
                }
                DataTable dtgetuser = new DataTable();
                SqlCommand cmdgetuser = new SqlCommand("Prc_GetEmployeeDetails", con);
                cmdgetuser.CommandType = CommandType.StoredProcedure;
                cmdgetuser.Parameters.AddWithValue("@emp_contactId", Session["userid"]);
                cmdgetuser.Parameters.AddWithValue("@Action", "GetUserstatus");
                cmdgetuser.CommandTimeout = 0;
                SqlDataAdapter dagetuser = new SqlDataAdapter();
                dagetuser.SelectCommand = cmdgetuser;
                dagetuser.Fill(dtgetuser);
                if (dtgetuser.Rows.Count > 0)
                {
                    if (dtgetuser.Rows[0]["user_status"].ToString() != "" && dtgetuser.Rows[0]["user_status"].ToString() != null
                    && dtgetuser.Rows[0]["user_status"] != DBNull.Value)
                    {
                        if (dtgetuser.Rows[0]["user_status"].ToString() == "1")
                        { 
                            active.Visible = true;
                            inactive.Visible = false;
                        }
                        else
                        {
                            active.Visible = false;
                            inactive.Visible = true;
                        }
                    }
                }
                DataTable dtgetsupervisor = new DataTable();
                SqlCommand cmdgetsupervisor = new SqlCommand("Prc_GetEmployeeDetails", con);
                cmdgetsupervisor.CommandType = CommandType.StoredProcedure;
                cmdgetsupervisor.Parameters.AddWithValue("@emp_contactId", HttpContext.Current.Session["KeyVal_InternalID"]);
                cmdgetsupervisor.Parameters.AddWithValue("@Action", "Getemployeesupervisor");
                cmdgetsupervisor.CommandTimeout = 0;
                SqlDataAdapter dagetsupervisor = new SqlDataAdapter();
                dagetsupervisor.SelectCommand = cmdgetsupervisor;
                dagetsupervisor.Fill(dtgetsupervisor);
                if (dtgetsupervisor.Rows.Count > 0)
                {
                    if (dtgetsupervisor.Rows[0]["REPORTTO"].ToString() != "" && dtgetsupervisor.Rows[0]["REPORTTO"].ToString() != null
                    && dtgetsupervisor.Rows[0]["REPORTTO"] != DBNull.Value)
                    {
                        lblsupervisor.Text = dtgetsupervisor.Rows[0]["REPORTTO"].ToString();
                    }
                }
                con.Close();
                con.Dispose();
                //End Rev Rajdip
            }
            else
            {
                //Binding of comboBox start here//
                //------------------------------//
                TrLang.Visible = false;
                string[,] Data = oDBEngine.GetFieldValue("tbl_master_salutation", "sal_id, sal_name", null, 2, "sal_name");
                //oDBEngine.AddDataToDropDownList(Data, CmbSalutation);
                OclsDropDownList.AddDataToDropDownList(Data, CmbSalutation);
                CmbSalutation.SelectedValue = "1";
                Data = oDBEngine.GetFieldValue("tbl_master_education", "edu_id, edu_education", null, 2, "edu_education");
                //oDBEngine.AddDataToDropDownList(Data, cmbEducation);
                OclsDropDownList.AddDataToDropDownList(Data, cmbEducation);
                Data = oDBEngine.GetFieldValue("tbl_master_branch", "branch_id, branch_description ", null, 2, "branch_description");
                //oDBEngine.AddDataToDropDownList(Data, cmbBranch);
                OclsDropDownList.AddDataToDropDownList(Data, cmbBranch);
                Data = oDBEngine.GetFieldValue(" tbl_master_maritalstatus", " mts_id, mts_maritalStatus", null, 2, "mts_maritalStatus");
                //oDBEngine.AddDataToDropDownList(Data, cmbMaritalStatus);
                OclsDropDownList.AddDataToDropDownList(Data, cmbMaritalStatus);
                Data = oDBEngine.GetFieldValue(" tbl_master_ContactSource", "cntsrc_id, cntsrc_sourcetype", null, 2, "cntsrc_sourcetype");
                //oDBEngine.AddDataToDropDownList(Data, cmbSource);
                OclsDropDownList.AddDataToDropDownList(Data, cmbSource);
                Data = oDBEngine.GetFieldValue("tbl_master_legalstatus", "lgl_id, lgl_legalStatus", null, 2, "lgl_legalStatus");
                //oDBEngine.AddDataToDropDownList(Data, cmbLegalStatus);
                OclsDropDownList.AddDataToDropDownList(Data, cmbLegalStatus);
                Data = oDBEngine.GetFieldValue("tbl_master_profession", "pro_id, pro_professionName", null, 2, "pro_professionName");
                //oDBEngine.AddDataToDropDownList(Data, cmbProfession);
                OclsDropDownList.AddDataToDropDownList(Data, cmbProfession);
                cmbLegalStatus.Items.Insert(0, new ListItem("--Select--", "0"));
                cmbEducation.Items.Insert(0, new ListItem("--Select--", "0"));
                cmbProfession.Items.Insert(0, new ListItem("--Select--", "0"));
                cmbBranch.Items.Insert(0, new ListItem("--Select--", "0"));

                cmbSource.Items.Insert(0, new ListItem("--Select--", "0"));
                cmbMaritalStatus.Items.Insert(0, new ListItem("--Select--", "0"));

                //cmbJobResponsibility.Items.Insert(0, new ListItem("--Select--", "0"));
                //cmbDesignation.Items.Insert(0, new ListItem("--Select--", "0"));

                //cmbIndustry.Items.Insert(0, new ListItem("--Select--", "0"));
                //------select branch
                string branchid = HttpContext.Current.Session["userbranchID"].ToString();
                DataTable dtname = oDBEngine.GetDataTable(" tbl_master_branch", "  branch_description", " branch_id= '" + branchid + "'");
                string branchName = dtname.Rows[0]["branch_description"].ToString();
                //cmbBranch.Items.Insert(0, new ListItem(branchName, branchid));
                // -------------------

                //          End Here            //
                //------------------------------//
                CmbSalutation.SelectedIndex.Equals(0);
                txtFirstNmae.Text = "";
                txtMiddleName.Text = "";
                txtLastName.Text = "";
                txtAliasName.Text = "";
                cmbBranch.SelectedIndex.Equals(0);
                cmbGender.SelectedIndex.Equals(0);
                cmbMaritalStatus.SelectedIndex.Equals(0);

                lstReferedBy.Items.FindByValue(hdReferenceBy.Value).Selected = true;

                DOBDate.Value = null;
                //txtAnniversary.Text = "";
                AnniversaryDate.Value = null;
                cmbLegalStatus.SelectedIndex.Equals(0);
                cmbEducation.SelectedIndex.Equals(0);
                cmbSource.SelectedIndex.Equals(0);
                TabVisibility("Off");
                cmbBranch.Enabled = true;
                HttpContext.Current.Session["KeyVal"] = 0;
                HttpContext.Current.Session["KeyVal_InternalID"] = string.Empty;
            }
        }

        private void TabVisibility(string onoff)
        {
            if (onoff == "Off")
            {
                //----Making TABs Disable------//
                TabPage page = ASPxPageControl1.TabPages.FindByName("CorresPondence");
                // page.Enabled = false;
                page.Visible = false;
                page = ASPxPageControl1.TabPages.FindByName("BankDetails");
                //page.Enabled = false;
                page.Visible = false;
                page = ASPxPageControl1.TabPages.FindByName("DPDetails");
                // page.Enabled = false;
                page.Visible = false;
                page = ASPxPageControl1.TabPages.FindByName("Documents");
                //page.Enabled = false;
                page.Visible = false;
                page = ASPxPageControl1.TabPages.FindByName("FamilyMembers");
                //page.Enabled = false;
                page.Visible = false;
                page = ASPxPageControl1.TabPages.FindByName("Registration");
                //page.Enabled = false;
                page.Visible = false;
                page = ASPxPageControl1.TabPages.FindByName("GroupMember");
                //page.Enabled = false;
                page.Visible = false;



                page = ASPxPageControl1.TabPages.FindByName("Employee");
                //page.Enabled = false;
                page.Visible = false;
                page = ASPxPageControl1.TabPages.FindByName("EmployeeCTC");
                //page.Enabled = false;
                page.Visible = false;
                page = ASPxPageControl1.TabPages.FindByName("Remarks");
                //page.Enabled = false;
                page.Visible = false;
                page = ASPxPageControl1.TabPages.FindByName("Education");
                //page.Enabled = false;
                page.Visible = false;
                page = ASPxPageControl1.TabPages.FindByName("Subscription");
                //page.Enabled = false;
                page.Visible = false;
            }
            if (onoff == "on")
            {
                TabPage page = ASPxPageControl1.TabPages.FindByName("CorresPondence");
                //page.Enabled = true;
                page.Visible = true;
                page = ASPxPageControl1.TabPages.FindByName("BankDetails");
                //page.Enabled = true;
                page.Visible = true;
                page = ASPxPageControl1.TabPages.FindByName("DPDetails");
                //page.Enabled = true;
                page.Visible = true;
                page = ASPxPageControl1.TabPages.FindByName("Documents");
                //page.Enabled = true;
                page.Visible = true;
                page = ASPxPageControl1.TabPages.FindByName("FamilyMembers");
                //page.Enabled = true;
                page.Visible = true;
                page = ASPxPageControl1.TabPages.FindByName("Registration");
                //page.Enabled = true;
                page.Visible = true;
                page = ASPxPageControl1.TabPages.FindByName("GroupMember");
                //page.Enabled = true;
                page.Visible = true;
                page = ASPxPageControl1.TabPages.FindByName("Employee");
                //page.Enabled = true;
                page.Visible = true;
                page = ASPxPageControl1.TabPages.FindByName("EmployeeCTC");
                //page.Enabled = true;
                page.Visible = true;
                page = ASPxPageControl1.TabPages.FindByName("Remarks");
                // page.Enabled = true;
                page.Visible = true;
                page = ASPxPageControl1.TabPages.FindByName("Education");
                // page.Enabled = false;
                page.Visible = true;
                page = ASPxPageControl1.TabPages.FindByName("Subscription");
                // page.Enabled = false;
                page.Visible = true;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string webLogin = "";
            string Password = "";
            string referedID = "";
            referedID = ddlReferedBy.SelectedValue.ToString();
            //if (txtReferedBy.Text != "")
            //{
            //    if (txtReferedBy_hidden.Text != "")
            //    {
            //        string[] refrnceBy = txtReferedBy.Text.ToString().Split('!');
            //        //txtReferedBy.Text = refrnceBy[1].ToString();
            //        referedID = txtReferedBy_hidden.Text;
            //        // referedID = ddlReferedBy.SelectedValue.ToString();
            //    }
            //    else
            //        referedID = txtReferedBy.Text;
            //    // referedID = ddlReferedBy.SelectedValue.ToString();
            //}

            string Is_Director = "";
            if (chkIsDirector.Checked)
                Is_Director = "1";
            else
                Is_Director = "0";

            referedID = txtReferedBy_hidden.Text;
            if (int.Parse(HttpContext.Current.Session["KeyVal"].ToString()) != 0)        //________For Update
            {
                DateTime today = oDBEngine.GetDate();
                if (chkAllow.Checked == true)
                {
                    webLogin = "Yes";
                    Password = txtAliasName.Text;
                }
                else
                {
                    webLogin = "No";
                }
                String value = "cnt_ucc='" + txtAliasName.Text + "', cnt_salutation=" + CmbSalutation.SelectedItem.Value + ",  cnt_firstName='" + txtFirstNmae.Text.ToUpper() + "', cnt_middleName='" + txtMiddleName.Text.ToUpper() + "', cnt_lastName='" + txtLastName.Text.ToUpper() + "', cnt_shortName='" + txtAliasName.Text + "', cnt_branchId=" + cmbBranch.SelectedItem.Value + ", cnt_sex=" + cmbGender.SelectedItem.Value + ", cnt_maritalStatus=" + cmbMaritalStatus.SelectedItem.Value + ", cnt_DOB='" + DOBDate.Value + "', cnt_anniversaryDate='" + AnniversaryDate.Value + "', cnt_legalStatus=" + cmbLegalStatus.SelectedItem.Value + ", cnt_education=" + cmbEducation.SelectedItem.Value + ",  cnt_contactSource=" + cmbSource.SelectedItem.Value + ", cnt_referedBy='" + referedID + "', cnt_contactType='EM',cnt_bloodgroup='" + cmbBloodgroup.SelectedItem.Value + "',WebLogIn='" + webLogin + "',PassWord='" + Password + "', lastModifyDate ='" + today + "', lastModifyUser=" + HttpContext.Current.Session["userid"] + ",TDSRATE_TYPE="+Convert.ToInt64(cmbTDS.Value)+" ,cnt_profession=" + cmbProfession.SelectedValue; // + Session["userid"] ;
                string[,] AName = oDBEngine.GetFieldValue("tbl_master_employee", "emp_contactId,emp_uniqueCode", " emp_uniqueCode='" + txtAliasName.Text + "'", 2);
                if (AName[0, 0] != "n")
                {
                    if (AName[0, 1] != "")
                    {
                        if (AName[0, 0] != Session["KeyVal_InternalID"].ToString())
                        {
                            Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('Employee Code Already Exists')</script>");
                            return;
                        }
                    }

                }
                Int32 rowsEffected = oDBEngine.SetFieldValue("tbl_master_contact", value, " cnt_id=" + HttpContext.Current.Session["KeyVal"]);


                string Hierchy = "";
                if (chkUsehierchy.Checked)
                    Hierchy = "Usehierchy=1";
                else
                    Hierchy = "Usehierchy=0";


                string Inactive = "";
                if (chkInactive.Checked)
                    Inactive = "Inactive=1";
                else
                    Inactive = "Inactive=0";

                

                if (chkInactive.Checked)
                    rowsEffected = oDBEngine.SetFieldValue(" tbl_master_user ", " user_inactive='Y'", " user_contactId='" + HttpContext.Current.Session["KeyVal_InternalID"].ToString() + "'");

                //rowsEffected = oDBEngine.SetFieldValue(" tbl_master_employee ", " emp_uniqueCode='" + txtAliasName.Text + "'," + Hierchy + "," + Inactive + "," + Is_Director + ",UpdateDayPermission=" + txtUpdateDays.Text, " emp_contactId='" + HttpContext.Current.Session["KeyVal_InternalID"].ToString() + "'");
                rowsEffected = oDBEngine.SetFieldValue(" tbl_master_employee ", " emp_uniqueCode='" + txtAliasName.Text + "'," + Hierchy + "," + Inactive + ",UpdateDayPermission=" + txtUpdateDays.Text, " emp_contactId='" + HttpContext.Current.Session["KeyVal_InternalID"].ToString() + "'");

                     
                
                
                #region For DOJ & DOC & image


                     string ImageName = "";
                     ImageName = fileupload.FileName;
                     string UploadFileDirectory = System.Web.Configuration.WebConfigurationManager.AppSettings["SaveFile"].ToString(); //"~/CommonFolder";




                     //string folderPath = Server.MapPath("../../../assests/UserImages/");
                     //Check whether Directory (Folder) exists.
                     //string filewithfolderpath = string.Empty;
                     string vPath = string.Empty;
                     if (fileupload.FileName != "" && fileupload.FileName != null)
                     {
                         //if (!Directory.Exists(folderPath))
                         //{
                         //    //If Directory (Folder) does not exists.Create it.
                         //    Directory.CreateDirectory(folderPath);
                         //}

                         int imgSize = fileupload.PostedFile.ContentLength;

                         string ext = System.IO.Path.GetExtension(this.fileupload.PostedFile.FileName);

                         if (fileupload.PostedFile.ContentLength > 1048576)
                         {
                             lblfilesize.Visible = true;
                             return;
                         }
                         if (ext != ".jpg" && ext != ".png" && ext != ".gif" && ext != ".jpeg")
                         {
                            //Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('Please choose only .jpg, .png and .gif image types!')", true);
                             return;
                         }
                       
                        //Save the File to the Directory (Folder).

                         //fileupload.SaveAs(folderPath + Path.GetFileName(fileupload.FileName));
                         //filewithfolderpath = "../../../assests/UserImages/" + Path.GetFileName(fileupload.FileName);

                         string uploadpath = string.Empty;
                         uploadpath="~/"+UploadFileDirectory;
                         ImageName = HttpContext.Current.Session["KeyVal_InternalID"].ToString() + '_' + ImageName;
                         vPath = Path.Combine(Server.MapPath(uploadpath), ImageName);
                         fileupload.SaveAs(vPath);
                        
                       
                     }
                     else
                     {
                         ImageName = hdnimagesrc.Value;
                     }

                     DateTime joinningdate = new DateTime();
                     joinningdate = txtjoinningdate.Date;
                     DateTime confirmationdate = new DateTime();
                     confirmationdate = txtconfirmationdate.Date;
                     if (joinningdate>confirmationdate)
                     {
                         //Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('Date Of Confirmation should be after the Date of Joining')</script>");
                         Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Date Of Confirmation cannot be prior to Date Of Joining')</script>");
                         return;
                     }
                    
                     CultureInfo ci = CultureInfo.InvariantCulture;
                     DateTime lowerbound;
                     DateTime Upperbound;
                     DateTime joindate;
                     DateTime confirmdate;
                     lowerbound = Convert.ToDateTime("01/01/1753");
                     Upperbound = DateTime.ParseExact("12/31/9999", "MM/dd/yyyy", ci);
                     if (joinningdate <= lowerbound || joinningdate >= Upperbound)
                     {
                         txtjoinningdate.Date = DateTime.Now;
                         return;
                     }
                     if (confirmationdate <= lowerbound || confirmationdate >= Upperbound)
                     {
                         txtconfirmationdate.Date = DateTime.Now;
                         return;
                     }
                     try
                     {
                         SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                         DataTable dtsetemp = new DataTable();
                         SqlCommand cmd = new SqlCommand("Prc_SetEmployeeDetails", con);
                         cmd.CommandType = CommandType.StoredProcedure;
                         cmd.Parameters.AddWithValue("@Action", "SetJoiningandConfirmationdate");
                         cmd.Parameters.AddWithValue("@DOJ", txtjoinningdate.Value);
                         cmd.Parameters.AddWithValue("@DOC", txtconfirmationdate.Value);
                         cmd.Parameters.AddWithValue("@imgpath", ImageName);
                         cmd.Parameters.AddWithValue("@emp_contactId", HttpContext.Current.Session["KeyVal_InternalID"].ToString());
                         cmd.Parameters.AddWithValue("@IsDirector", Is_Director);
                         cmd.CommandTimeout = 0;
                         SqlDataAdapter da = new SqlDataAdapter();
                         da.SelectCommand = cmd;
                         da.Fill(dtsetemp);
                         cmd.Dispose();
                         con.Dispose();
                     }
                     catch
                     {
                        
                     }
                     #endregion Rajdip
                     Response.Redirect("employee_general.aspx?id=" + HttpContext.Current.Session["KeyVal"], false);
            }
            else               //_________For Insurt
            {
                try
                {
                    //String con = ConfigurationSettings.AppSettings["DBConnectionDefault"]; MULTI
                    String con = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    SqlConnection lcon = new SqlConnection(con);
                    lcon.Open();
                    SqlCommand lcmdEmplInsert = new SqlCommand("EmployeeInsert", lcon);
                    lcmdEmplInsert.CommandType = CommandType.StoredProcedure;

                    lcmdEmplInsert.Parameters.AddWithValue("@cnt_ucc", txtAliasName.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@cnt_salutation", CmbSalutation.SelectedItem.Value);
                    lcmdEmplInsert.Parameters.AddWithValue("@cnt_firstName", txtFirstNmae.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@cnt_middleName", txtMiddleName.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@cnt_lastName", txtLastName.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@cnt_shortName", txtAliasName.Text);
                    lcmdEmplInsert.Parameters.AddWithValue("@cnt_branchId", cmbBranch.SelectedItem.Value);
                    lcmdEmplInsert.Parameters.AddWithValue("@cnt_sex", cmbGender.SelectedItem.Value);
                    lcmdEmplInsert.Parameters.AddWithValue("@cnt_maritalStatus", cmbMaritalStatus.SelectedItem.Value);
                    //if (txtDOB.Text != null)
                    if (DOBDate.Value != null)
                    {
                        lcmdEmplInsert.Parameters.AddWithValue("@cnt_DOB", DOBDate.Value);
                    }
                    else
                    {
                        lcmdEmplInsert.Parameters.AddWithValue("@cnt_DOB", "");
                    }
                    if (AnniversaryDate.Value != null)
                    {
                        lcmdEmplInsert.Parameters.AddWithValue("@cnt_anniversaryDate", AnniversaryDate.Value);
                    }
                    else
                    {
                        lcmdEmplInsert.Parameters.AddWithValue("@cnt_anniversaryDate", "");
                    }
                    //___________For Returning InternalID_________//
                    SqlParameter parameter = new SqlParameter("@result", SqlDbType.VarChar, 50);
                    parameter.Direction = ParameterDirection.Output;
                    ///_______________________________________________//

                    lcmdEmplInsert.Parameters.AddWithValue("@cnt_legalStatus", cmbLegalStatus.SelectedItem.Value);
                    lcmdEmplInsert.Parameters.AddWithValue("@cnt_education", cmbEducation.SelectedItem.Value);
                    lcmdEmplInsert.Parameters.AddWithValue("@cnt_contactSource", cmbSource.SelectedItem.Value);
                    lcmdEmplInsert.Parameters.AddWithValue("@cnt_referedBy", referedID);
                    lcmdEmplInsert.Parameters.AddWithValue("@cnt_contactType", "EM");
                    lcmdEmplInsert.Parameters.AddWithValue("@lastModifyUser", HttpContext.Current.Session["userid"]);
                    lcmdEmplInsert.Parameters.AddWithValue("@UserContactID", HttpContext.Current.Session["usercontactID"]);
                    lcmdEmplInsert.Parameters.AddWithValue("@bloodgroup", cmbBloodgroup.SelectedItem.Value);

                    if (chkAllow.Checked == true)
                    {
                        webLogin = "Yes";
                        Password = txtAliasName.Text;
                    }
                    else
                    {
                        webLogin = "No";
                    }
                    lcmdEmplInsert.Parameters.AddWithValue("@WebLogIn", webLogin);
                    lcmdEmplInsert.Parameters.AddWithValue("@PassWord", Password);
                    lcmdEmplInsert.Parameters.AddWithValue("@TDSType", Convert.ToInt64(cmbTDS.Value));
                    lcmdEmplInsert.Parameters.AddWithValue("@IsDirector", Is_Director);                   
                    lcmdEmplInsert.Parameters.Add(parameter);
                    lcmdEmplInsert.ExecuteNonQuery();
                    // Mantis Issue 24802
                    if (lcon.State == ConnectionState.Open)
                    {
                        lcon.Close();
                    }
                    // End of Mantis Issue 24802

                    string InternalID = parameter.Value.ToString();
                    CntID = parameter.Value.ToString();
                    //HttpContext.Current.Session["KeyVal_InternalID"] = InternalID;
                    string[,] cnt_id = oDBEngine.GetFieldValue(" tbl_master_contact", " cnt_id", " cnt_internalId='" + InternalID + "'", 1);
                    if (cnt_id[0, 0].ToString() != "n")
                    {
                        Response.Redirect("employee_general.aspx?id=" + cnt_id[0, 0].ToString(), false);
                    }
                    TrLang.Visible = true;
                }
                catch
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>alert('Employee Code Already Exists')</script>");
                }
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }
        //protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
        //{
        //    this.EnsureChildControls();
        //    //this.SimpleControl.RenderControl(writer);
        //}
        public void LanGuage()
        {
            string InternalId = HttpContext.Current.Session["KeyVal_InternalID"].ToString();//"EMA0000003";        
            string[,] ListlngId = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_speakLanguage,cnt_writeLanguage", "cnt_internalId='" + InternalId + "'", 2);
            string speak = ListlngId[0, 0];
            SpLanguage = speak;
            string write = ListlngId[0, 1];
            WLanguage = write;
            if (speak != "")
            {
                string spk = "";
                string[] st = speak.Split(',');
                for (int i = 0; i <= st.GetUpperBound(0); i++)
                {
                    string[,] ListlngId1 = oDBEngine.GetFieldValue("tbl_master_language", "lng_language", "lng_id= '" + st[i] + "'", 1);
                    string Id = ListlngId1[0, 0];
                    spk += Id + ", ";
                }
                int spklng = spk.LastIndexOf(',');
                spk = spk.Substring(0, spklng);
                LitSpokenLanguage.Text = spk;
            }
            if (write != "")
            {
                string wrt = "";
                string[] wrte = write.Split(',');
                for (int i = 0; i <= wrte.GetUpperBound(0); i++)
                {
                    string[,] ListlngId1 = oDBEngine.GetFieldValue("tbl_master_language", "lng_language", "lng_id= '" + wrte[i] + "'", 1);
                    string Id = ListlngId1[0, 0];
                    wrt += Id + ",";
                }
                int wrtlng = wrt.LastIndexOf(',');
                wrt = wrt.Substring(0, wrtlng);
                LitWrittenLanguage.Text = wrt;
            }

        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            //DataTable dtV = new DataTable();
            DataTable dtS = new DataTable();
            DataTable dtB = new DataTable();
            DataTable dtC = new DataTable();
            DataTable dtSchemaOn = new DataTable();
            DataTable dtSchemaOff = new DataTable();
            DataTable dtCTC = new DataTable();
            string CompCode = string.Empty;
            int EmpCode;
            String ShortName = string.Empty;
            string TempCode = string.Empty;
            string startNo, paddedStr;
            int paddCounter;

            if (Request.QueryString["id"] == "ADD")
            {
                dtCTC = oDBEngine.GetDataTable("tbl_trans_employeeCTC", " *", "emp_cntID='" + CntID + "'");
            }
            else
            {
                dtCTC = oDBEngine.GetDataTable("tbl_trans_employeeCTC", " *", "emp_cntID='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'");
            }

            if (dtCTC.Rows.Count > 0)
            {
                dtS = oDBEngine.GetDataTable("tbl_master_company", "cmp_OffRoleShortName, cmp_OnRoleShortName, onrole_schema_id, offrole_schema_id", "cmp_id=" + dtCTC.Rows[0]["emp_Organization"] + "");

                if (dtS.Rows[0]["onrole_schema_id"].ToString() != "" && dtS.Rows[0]["offrole_schema_id"].ToString() != "")
                {
                    dtSchemaOn = oDBEngine.GetDataTable("tbl_master_idschema", "prefix, suffix, digit, startno", "id=" + Convert.ToInt64(dtS.Rows[0]["onrole_schema_id"]));
                    dtSchemaOff = oDBEngine.GetDataTable("tbl_master_idschema", "prefix, suffix, digit, startno", "id=" + Convert.ToInt64(dtS.Rows[0]["offrole_schema_id"]));

                    if (dtCTC.Rows[0]["emp_type"].ToString().Length != 0)
                    {
                        if (dtCTC.Rows[0]["emp_type"].ToString() == "1")
                        {
                            startNo = dtSchemaOn.Rows[0]["startno"].ToString();
                            paddCounter = Convert.ToInt32(dtSchemaOn.Rows[0]["digit"]);
                            paddedStr = startNo.PadLeft(paddCounter, '0');
                            CompCode = dtSchemaOn.Rows[0]["prefix"].ToString() + dtSchemaOn.Rows[0]["suffix"].ToString();
                        }
                        else
                        {
                            startNo = dtSchemaOff.Rows[0]["startno"].ToString();
                            paddCounter = Convert.ToInt32(dtSchemaOff.Rows[0]["digit"]);
                            paddedStr = startNo.PadLeft(paddCounter, '0');
                            CompCode = dtSchemaOff.Rows[0]["prefix"].ToString() + dtSchemaOff.Rows[0]["suffix"].ToString();
                        }

                        dtC = oDBEngine.GetDataTable("tbl_master_contact", "max(Cnt_UCC) ", "Cnt_UCC like '" +
                            CompCode.ToString() + "[0-9]%' and cnt_internalid like 'EM%'");
                        if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString() != "")
                        {
                            string uccCode = dtC.Rows[0][0].ToString();
                            int UCCLen = uccCode.Length;
                            int ccLen = Convert.ToInt32(CompCode.ToString().Length);
                            int decimalPartLen = Convert.ToInt32(UCCLen - ccLen);
                            string uccCodeSubstring = uccCode.Substring(ccLen, decimalPartLen);

                            EmpCode = Convert.ToInt32(uccCodeSubstring) + 1;
                            CompCode = CompCode + EmpCode.ToString();
                        }
                        else
                        {
                            startNo = "1";
                            paddedStr = startNo.PadLeft(paddCounter, '0');
                            CompCode = CompCode + paddedStr;
                        }
                        oDBEngine.SetFieldValue("tbl_master_contact", "Cnt_UCC ='" + CompCode.ToString() + "'", " cnt_internalid ='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'");
                        oDBEngine.SetFieldValue("tbl_master_employee", "emp_uniquecode='" + CompCode.ToString() + "'", "emp_contactID='" + Session["KeyVal_InternalID"] + "'");
                        txtAliasName.Text = CompCode.ToString();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct", "jAlert('Employee Type Not Found !);", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct55", "jAlert('Company On / Off Role Schema Not Defined !');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct43", "jAlert('Please Add Employee CTC Details First !');", true);
            }

        }
    }
}
