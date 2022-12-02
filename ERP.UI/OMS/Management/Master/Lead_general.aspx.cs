using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
//using DevExpress.Web.ASPxTabControl;
using DevExpress.Web;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;

namespace ERP.OMS.Management.Master
{
    public partial class management_Master_Lead_general : ERP.OMS.ViewState_class.VSPage
    {
        Int32 ID;
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        BusinessLogicLayer.Converter oConverter = new BusinessLogicLayer.Converter();

        clsDropDownList oclsDropDownList = new clsDropDownList();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (HttpContext.Current.Session["userid"] == null)
            {
                //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            ////this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");

            if (!IsPostBack)
            {
                txtDOB.EditFormatString = oConverter.GetDateFormat("Date");
                txtAnniversary.EditFormatString = oConverter.GetDateFormat("Date");
                Session["ContactType"] = "Lead";
                if (Request.QueryString["formtype"] != null)
                {
                    string Internal_ID = Session["InternalId"].ToString();
                    string IID = Internal_ID.Substring(0, 2);
                    DDLBind();
                    string[,] ContactData;
                    if (IID == "LD")
                    {
                        ContactData = oDBEngine.GetFieldValue("tbl_master_lead", "cnt_ucc,cnt_salutation,cnt_firstName,cnt_middleName,cnt_lastName,cnt_shortName,cnt_branchId,cnt_sex,cnt_maritalStatus,case cnt_DOB when '1/1/1900 12:00:00 AM' then null else cnt_DOB end as cnt_DOB,case cnt_anniversaryDate when '1/1/1900 12:00:00 AM' then null else cnt_anniversaryDate end as cnt_anniversaryDate,cnt_legalStatus,cnt_education,cnt_profession,cnt_organization,cnt_jobResponsibility,cnt_designation,cnt_industry,cnt_contactSource,cnt_referedBy,cnt_contactType,cnt_contactStatus,cnt_rating,cnt_bloodgroup", " cnt_internalId='" + Internal_ID + "'", 24);
                    }
                    else
                    {
                        ContactData = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_ucc,cnt_salutation,cnt_firstName,cnt_middleName,cnt_lastName,cnt_shortName,cnt_branchId,cnt_sex,cnt_maritalStatus,case cnt_DOB when '1/1/1900 12:00:00 AM' then null else cnt_DOB end as cnt_DOB,case cnt_anniversaryDate when '1/1/1900 12:00:00 AM' then null else cnt_anniversaryDate end as cnt_anniversaryDate,cnt_legalStatus,cnt_education,cnt_profession,cnt_organization,cnt_jobResponsibility,cnt_designation,cnt_industry,cnt_contactSource,cnt_referedBy,cnt_contactType,cnt_contactStatus,cnt_rating,cnt_bloodgroup", " cnt_internalId='" + Internal_ID + "'", 24);
                    }
                    ValueAllocation(ContactData);
                    DisabledTabPage();
                }
                else
                {
                    if (Request.QueryString["requesttypeP"] != null)
                    {
                        string Internal_ID = Session["LeadId"].ToString();
                        string IID = Internal_ID.Substring(0, 2);
                        DDLBind();
                        string[,] ContactData;
                        if (IID == "LD")
                        {
                            ContactData = oDBEngine.GetFieldValue("tbl_master_lead", "cnt_ucc,cnt_salutation,cnt_firstName,cnt_middleName,cnt_lastName,cnt_shortName,cnt_branchId,cnt_sex,cnt_maritalStatus,case cnt_DOB when '1/1/1900 12:00:00 AM' then null else cnt_DOB end as cnt_DOB,case cnt_anniversaryDate when '1/1/1900 12:00:00 AM' then null else cnt_anniversaryDate end as cnt_anniversaryDate,cnt_legalStatus,cnt_education,cnt_profession,cnt_organization,cnt_jobResponsibility,cnt_designation,cnt_industry,cnt_contactSource,cnt_referedBy,cnt_contactType,cnt_contactStatus,cnt_rating,cnt_bloodgroup", " cnt_internalId='" + Internal_ID + "'", 24);
                        }
                        else
                        {
                            ContactData = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_ucc,cnt_salutation,cnt_firstName,cnt_middleName,cnt_lastName,cnt_shortName,cnt_branchId,cnt_sex,cnt_maritalStatus,case cnt_DOB when '1/1/1900 12:00:00 AM' then null else cnt_DOB end as cnt_DOB,case cnt_anniversaryDate when '1/1/1900 12:00:00 AM' then null else cnt_anniversaryDate end as cnt_anniversaryDate,cnt_legalStatus,cnt_education,cnt_profession,cnt_organization,cnt_jobResponsibility,cnt_designation,cnt_industry,cnt_contactSource,cnt_referedBy,cnt_contactType,cnt_contactStatus,cnt_rating,cnt_bloodgroup", " cnt_internalId='" + Internal_ID + "'", 24);
                        }
                        ValueAllocation(ContactData);
                        DisabledTabPage();
                    }
                    else
                    {
                        if (Request.QueryString["id"] != "ADD")
                        {
                            if (Request.QueryString["id"] != null)
                            {
                                ID = Int32.Parse(Request.QueryString["id"]);
                                HttpContext.Current.Session["KeyVal"] = ID;
                                string[,] InternalId;
                                InternalId = oDBEngine.GetFieldValue("tbl_master_lead", "cnt_internalId", "cnt_id=" + ID, 1);
                                HttpContext.Current.Session["KeyVal_InternalID"] = InternalId[0, 0];
                            }
                            string[,] ContactData;
                            if (ID != 0)
                            {
                                ContactData = oDBEngine.GetFieldValue("tbl_master_lead",
                                                        "cnt_ucc, cnt_salutation,  cnt_firstName, cnt_middleName, cnt_lastName, cnt_shortName, cnt_branchId, cnt_sex, cnt_maritalStatus, case cnt_DOB when '1/1/1900 12:00:00 AM' then null else cnt_DOB end as cnt_DOB,case cnt_anniversaryDate when '1/1/1900 12:00:00 AM' then null else cnt_anniversaryDate end as cnt_anniversaryDate, cnt_legalStatus, cnt_education, cnt_profession, cnt_organization, cnt_jobResponsibility, cnt_designation, cnt_industry, cnt_contactSource, cnt_referedBy, cnt_contactType, cnt_contactStatus,cnt_rating,cnt_bloodgroup",
                                                        " cnt_id=" + ID, 24);
                            }
                            else
                            {
                                ContactData = oDBEngine.GetFieldValue("tbl_master_lead",
                                                        "cnt_ucc, cnt_salutation,  cnt_firstName, cnt_middleName, cnt_lastName, cnt_shortName, cnt_branchId, cnt_sex, cnt_maritalStatus, case cnt_DOB when '1/1/1900 12:00:00 AM' then null else cnt_DOB end as cnt_DOB,case cnt_anniversaryDate when '1/1/1900 12:00:00 AM' then null else cnt_anniversaryDate end as cnt_anniversaryDate, cnt_legalStatus, cnt_education, cnt_profession, cnt_organization, cnt_jobResponsibility, cnt_designation, cnt_industry, cnt_contactSource, cnt_referedBy, cnt_contactType, cnt_contactStatus,cnt_rating,cnt_bloodgroup",
                                                        " cnt_id=" + HttpContext.Current.Session["KeyVal"], 24);
                            }
                            DDLBind();
                            ValueAllocation(ContactData);

                        }
                        else
                        {

                            DDLBind();
                            CmbSalutation.SelectedIndex.Equals(0);
                            txtFirstNmae.Text = "";
                            txtMiddleName.Text = "";
                            txtLastName.Text = "";
                            txtAliasName.Text = "";
                            cmbBranch.SelectedIndex.Equals(0);
                            cmbGender.SelectedIndex.Equals(0);
                            cmbMaritalStatus.SelectedIndex = 4;
                            //cmbDOB.Value = "";
                            txtDOB.Value = "";
                            //cmbAnniversary.Value = "";
                            txtAnniversary.Value = "";
                            cmbLegalStatus.SelectedIndex.Equals(0);
                            cmbEducation.SelectedIndex.Equals(0);
                            cmbProfession.SelectedIndex.Equals(0);
                            cmbJobResponsibility.SelectedIndex.Equals(0);
                            cmbDesignation.SelectedIndex.Equals(0);
                            cmbIndustry.SelectedIndex.Equals(0);
                            cmbSource.SelectedIndex.Equals(0);
                            cmbContactStatus.SelectedIndex.Equals(0);
                            //----Making TABs Disable------//
                            DisabledTabPage();
                            //page = ASPxPageControl1.TabPages.FindByName("EmployeeCTC");
                            //page.Enabled = false;
                            //-----End---------------------//
                            HttpContext.Current.Session["KeyVal"] = 0;
                        }
                    }
                }

            }

            DateTime dt = oDBEngine.GetDate();
            txtReferedBy.Attributes.Add("onkeyup", "CallList(this,'referedby',event)");
            //________This script is for firing javascript when page load first___//
            if (!ClientScript.IsStartupScriptRegistered("Today"))
                ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>AtTheTimePageLoad();</script>");
            //______________________________End Script____________________________//
        }
        public void DisabledTabPage()
        {
            TabPage page = ASPxPageControl1.TabPages.FindByName("CorresPondence");
            //page.Enabled = false;
            page.Visible = false;
            page = ASPxPageControl1.TabPages.FindByName("BankDetails");
            //page.Enabled = false;
            page.Visible = false;
            page = ASPxPageControl1.TabPages.FindByName("DPDetails");
            //page.Enabled = false;
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
            page = ASPxPageControl1.TabPages.FindByName("Remarks");
            page.Visible = false;
        }
        public void DDLBind()
        {
            string[,] Data = oDBEngine.GetFieldValue("tbl_master_salutation", "sal_id, sal_name", null, 2, "sal_name");

            oclsDropDownList.AddDataToDropDownList(Data, CmbSalutation);
            Data = oDBEngine.GetFieldValue("tbl_master_education", "edu_id, edu_education", null, 2, "edu_education");

            oclsDropDownList.AddDataToDropDownList(Data, cmbEducation);
            Data = oDBEngine.GetFieldValue("tbl_master_profession", "pro_id, pro_professionName", null, 2, "pro_professionName");

            oclsDropDownList.AddDataToDropDownList(Data, cmbProfession);
            Data = oDBEngine.GetFieldValue("tbl_master_jobresponsibility", "job_id, job_responsibility", null, 2, "job_responsibility");

            oclsDropDownList.AddDataToDropDownList(Data, cmbJobResponsibility);
            Data = oDBEngine.GetFieldValue("tbl_master_Designation", "deg_id, deg_designation", null, 2, "deg_designation");

            oclsDropDownList.AddDataToDropDownList(Data, cmbDesignation);
            Data = oDBEngine.GetFieldValue("tbl_master_branch", "branch_id, branch_description ", null, 2, "branch_description");

            oclsDropDownList.AddDataToDropDownList(Data, cmbBranch);
            Data = oDBEngine.GetFieldValue("tbl_master_industry", "ind_id, ind_industry  ", null, 2, "ind_industry");

            oclsDropDownList.AddDataToDropDownList(Data, cmbIndustry);
            Data = oDBEngine.GetFieldValue(" tbl_master_ContactSource", "cntsrc_id, cntsrc_sourcetype", null, 2, "cntsrc_sourcetype");

            oclsDropDownList.AddDataToDropDownList(Data, cmbSource);
            Data = oDBEngine.GetFieldValue("tbl_master_leadRating", " rat_id, rat_LeadRating  ", null, 2, "rat_LeadRating");

            oclsDropDownList.AddDataToDropDownList(Data, cmbRating);
            Data = oDBEngine.GetFieldValue(" tbl_master_maritalstatus", " mts_id, mts_maritalStatus", null, 2, "mts_maritalStatus");

            oclsDropDownList.AddDataToDropDownList(Data, cmbMaritalStatus);
            Data = oDBEngine.GetFieldValue(" tbl_master_contactstatus", "cntstu_id, cntstu_contactStatus", null, 2, "cntstu_contactStatus");

            oclsDropDownList.AddDataToDropDownList(Data, cmbContactStatus);
            Data = oDBEngine.GetFieldValue("tbl_master_legalstatus", "lgl_id, lgl_legalStatus", null, 2, "lgl_legalStatus");

            oclsDropDownList.AddDataToDropDownList(Data, cmbLegalStatus);
            CmbSalutation.SelectedValue = "1";
            cmbRating.SelectedValue = "1";
        }
        public void ValueAllocation(string[,] ContactData)
        {
            txtClentUcc.Text = ContactData[0, 0];
            string[,] Data = oDBEngine.GetFieldValue("tbl_master_salutation", "sal_id, sal_name", null, 2, "sal_name");
            if (ContactData[0, 1] != "")
            {
                oclsDropDownList.AddDataToDropDownList(Data, CmbSalutation, Int32.Parse(ContactData[0, 1]));
            }
            else
            {

                oclsDropDownList.AddDataToDropDownList(Data, CmbSalutation, 0);
            }
            txtFirstNmae.Text = ContactData[0, 2];
            txtMiddleName.Text = ContactData[0, 3];
            txtLastName.Text = ContactData[0, 4];
            txtAliasName.Text = ContactData[0, 5];
            cmbBloodgroup.SelectedValue = ContactData[0, 23];
            Data = oDBEngine.GetFieldValue("tbl_master_branch", "branch_id, branch_description ", null, 2, "branch_description");
            if (ContactData[0, 6] != "")
            {
                oclsDropDownList.AddDataToDropDownList(Data, cmbBranch, Int32.Parse(ContactData[0, 6]));
            }
            else
            {
                oclsDropDownList.AddDataToDropDownList(Data, cmbBranch, 0);
            }
            if (ContactData[0, 7] != "")
            {
                cmbGender.SelectedValue = ContactData[0, 7];
            }
            else
            {
                cmbGender.SelectedIndex.Equals(0);
            }
            Data = oDBEngine.GetFieldValue(" tbl_master_maritalstatus", " mts_id, mts_maritalStatus", null, 2, "mts_maritalStatus");

            if (ContactData[0, 8] != "")
            {
                oclsDropDownList.AddDataToDropDownList(Data, cmbMaritalStatus, Int32.Parse(ContactData[0, 8]));
            }
            else
            {
                oclsDropDownList.AddDataToDropDownList(Data, cmbMaritalStatus, 0);
            }

            if (ContactData[0, 9] != "")
            {
                txtDOB.Value = Convert.ToDateTime(ContactData[0, 9]);
            }

            if (ContactData[0, 10] != "")
            {
                txtAnniversary.Value = Convert.ToDateTime(ContactData[0, 10]);
            }
            Data = oDBEngine.GetFieldValue("tbl_master_legalstatus", "lgl_id, lgl_legalStatus", null, 2, "lgl_legalStatus");

            if (ContactData[0, 11] != "")
            {
                oclsDropDownList.AddDataToDropDownList(Data, cmbLegalStatus, Int32.Parse(ContactData[0, 11]));
            }
            else
            {
                oclsDropDownList.AddDataToDropDownList(Data, cmbLegalStatus, 0);
            }
            Data = oDBEngine.GetFieldValue("tbl_master_education", "edu_id, edu_education", null, 2, "edu_education");

            if (ContactData[0, 12] != "")
            {
                oclsDropDownList.AddDataToDropDownList(Data, cmbEducation, Int32.Parse(ContactData[0, 12]));
            }
            else
            {
                oclsDropDownList.AddDataToDropDownList(Data, cmbEducation, 0);
            }
            Data = oDBEngine.GetFieldValue("tbl_master_profession", "pro_id, pro_professionName", null, 2, "pro_professionName");

            if (ContactData[0, 13] != "")
            {
                oclsDropDownList.AddDataToDropDownList(Data, cmbProfession, Int32.Parse(ContactData[0, 13]));

            }
            else
            {
                oclsDropDownList.AddDataToDropDownList(Data, cmbProfession, 0);
            }

            txtOrganization.Text = ContactData[0, 14];
            Data = oDBEngine.GetFieldValue("tbl_master_jobresponsibility", "job_id, job_responsibility", null, 2, "job_responsibility");

            if (ContactData[0, 15] != "")
            {
                oclsDropDownList.AddDataToDropDownList(Data, cmbJobResponsibility, Int32.Parse(ContactData[0, 15]));
            }
            else
            {
                oclsDropDownList.AddDataToDropDownList(Data, cmbJobResponsibility, 0);
            }
            Data = oDBEngine.GetFieldValue("tbl_master_Designation", "deg_id, deg_designation", null, 2, "deg_designation");

            if (ContactData[0, 16] != "")
            {
                oclsDropDownList.AddDataToDropDownList(Data, cmbDesignation, Int32.Parse(ContactData[0, 16]));
            }
            else
            {
                oclsDropDownList.AddDataToDropDownList(Data, cmbDesignation, 0);
            }
            Data = oDBEngine.GetFieldValue("tbl_master_industry", "ind_id, ind_industry  ", null, 2, "ind_industry");

            if (ContactData[0, 17] != "")
            {
                oclsDropDownList.AddDataToDropDownList(Data, cmbIndustry, Int32.Parse(ContactData[0, 17]));
            }
            else
            {
                oclsDropDownList.AddDataToDropDownList(Data, cmbIndustry, 0);
            }
            Data = oDBEngine.GetFieldValue(" tbl_master_ContactSource", "cntsrc_id, cntsrc_sourcetype", null, 2, "cntsrc_sourcetype");

            if (ContactData[0, 18] != "")
            {
                oclsDropDownList.AddDataToDropDownList(Data, cmbSource, Int32.Parse(ContactData[0, 18]));
            }
            else
            {
                oclsDropDownList.AddDataToDropDownList(Data, cmbSource, 0);
            }
            txtReferedBy_hidden.Text = ContactData[0, 19].ToString();
            Data = oDBEngine.GetFieldValue(" tbl_master_contact", " (ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') + ' [' + cnt_shortName +']') AS cnt_firstName ", " cnt_internalid='" + ContactData[0, 19].ToString() + "'", 1);
            if (Data[0, 0] != "n")
            {
                txtReferedBy.Text = Data[0, 0];
            }
            else
                txtReferedBy.Text = "";
            Data = oDBEngine.GetFieldValue(" tbl_master_contactstatus", "cntstu_id, cntstu_contactStatus", null, 2, "cntstu_contactStatus");

            if (ContactData[0, 21] != "")
            {
                oclsDropDownList.AddDataToDropDownList(Data, cmbContactStatus, Int32.Parse(ContactData[0, 21]));
            }
            else
            {
                oclsDropDownList.AddDataToDropDownList(Data, cmbContactStatus, 0);
            }
            Data = oDBEngine.GetFieldValue("tbl_master_leadRating", " rat_id, rat_LeadRating  ", null, 2, "rat_LeadRating");

            oclsDropDownList.AddDataToDropDownList(Data, cmbRating);
            if (ContactData[0, 22] != "")
            {
                oclsDropDownList.AddDataToDropDownList(Data, cmbRating, Int32.Parse(ContactData[0, 22]));
            }
            else
            {
                oclsDropDownList.AddDataToDropDownList(Data, cmbRating, 0);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);multi
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            BusinessLogicLayer.Converter oConverter = new BusinessLogicLayer.Converter();
            //txtDOB.Text = oConverter.DateConverter(txtDOB.Text, "mm/dd/yyyy hh:mm");
            //txtAnniversary.Text = oConverter.DateConverter(txtAnniversary.Text, "mm/dd/yyyy hh:mm");
            string referedID = "";
            if (txtReferedBy.Text != "")
            {
                if (txtReferedBy_hidden.Text != "")
                {
                    referedID = txtReferedBy_hidden.Text;
                }
            }
            if (Request.QueryString["formtype"] != null)
            {
                string Internal_ID = Session["InternalId"].ToString();
                string ID = Internal_ID.Substring(0, 2);
                string today = oDBEngine.GetDate().ToString();
                String value = "cnt_ucc='" + txtClentUcc.Text + "', cnt_salutation=" + CmbSalutation.SelectedItem.Value + ",  cnt_firstName='" + txtFirstNmae.Text + "', cnt_middleName='" + txtMiddleName.Text + "', cnt_lastName='" + txtLastName.Text + "', cnt_shortName='" + txtAliasName.Text + "', cnt_branchId=" + cmbBranch.SelectedItem.Value + ", cnt_sex=" + cmbGender.SelectedItem.Value + ", cnt_maritalStatus=" + cmbMaritalStatus.SelectedItem.Value + ", cnt_DOB='" + txtDOB.Value + "', cnt_anniversaryDate='" + txtAnniversary.Value + "', cnt_legalStatus=" + cmbLegalStatus.SelectedItem.Value + ", cnt_education=" + cmbEducation.SelectedItem.Value + ", cnt_profession=" + cmbProfession.SelectedItem.Value + ", cnt_organization='" + txtOrganization.Text + "', cnt_jobResponsibility=" + cmbJobResponsibility.SelectedItem.Value + ", cnt_designation=" + cmbDesignation.SelectedItem.Value + ", cnt_industry=" + cmbIndustry.SelectedItem.Value + ", cnt_contactSource=" + cmbSource.SelectedItem.Value + ", cnt_referedBy='" + txtReferedBy_hidden.Text + "', cnt_contactStatus=" + cmbContactStatus.SelectedItem.Value + ",cnt_rating='" + cmbRating.SelectedItem.Value + "',cnt_bloodgroup='" + cmbBloodgroup.SelectedItem.Value + "', lastModifyDate ='" + today + "', lastModifyUser=" + HttpContext.Current.Session["userid"]; // + Session["userid"] 


                if (ID == "LD")
                {
                    Int32 rowsEffected = oDBEngine.SetFieldValue("tbl_master_lead", value, " cnt_internalId='" + Internal_ID + "'");
                }
                else
                {
                    Int32 rowsEffected = oDBEngine.SetFieldValue("tbl_master_contact", value, " cnt_internalId='" + Internal_ID + "'");
                }
            }
            else
            {
                if (Request.QueryString["requesttypeP"] != null)
                {
                    string Internal_ID = Session["LeadId"].ToString();
                    string ID = Internal_ID.Substring(0, 2);
                    string today = oDBEngine.GetDate().ToString();
                    String value = "cnt_ucc='" + txtClentUcc.Text + "', cnt_salutation=" + CmbSalutation.SelectedItem.Value + ",  cnt_firstName='" + txtFirstNmae.Text + "', cnt_middleName='" + txtMiddleName.Text + "', cnt_lastName='" + txtLastName.Text + "', cnt_shortName='" + txtAliasName.Text + "', cnt_branchId=" + cmbBranch.SelectedItem.Value + ", cnt_sex=" + cmbGender.SelectedItem.Value + ", cnt_maritalStatus=" + cmbMaritalStatus.SelectedItem.Value + ", cnt_DOB='" + txtDOB.Value + "', cnt_anniversaryDate='" + txtAnniversary.Value + "', cnt_legalStatus=" + cmbLegalStatus.SelectedItem.Value + ", cnt_education=" + cmbEducation.SelectedItem.Value + ", cnt_profession=" + cmbProfession.SelectedItem.Value + ", cnt_organization='" + txtOrganization.Text + "', cnt_jobResponsibility=" + cmbJobResponsibility.SelectedItem.Value + ", cnt_designation=" + cmbDesignation.SelectedItem.Value + ", cnt_industry=" + cmbIndustry.SelectedItem.Value + ", cnt_contactSource=" + cmbSource.SelectedItem.Value + ", cnt_referedBy='" + txtReferedBy_hidden.Text + "', cnt_contactStatus=" + cmbContactStatus.SelectedItem.Value + ",cnt_rating='" + cmbRating.SelectedItem.Value + "',cnt_bloodgroup='" + cmbBloodgroup.SelectedItem.Value + "', lastModifyDate ='" + today + "', lastModifyUser=" + HttpContext.Current.Session["userid"]; // + Session["userid"] 

                    //oDBEngine.RunStoredProcedure(EmployeeUpdate, value);
                    if (ID == "LD")
                    {
                        Int32 rowsEffected = oDBEngine.SetFieldValue("tbl_master_lead", value, " cnt_internalId='" + Internal_ID + "'");
                    }
                    else
                    {
                        Int32 rowsEffected = oDBEngine.SetFieldValue("tbl_master_contact", value, " cnt_internalId='" + Internal_ID + "'");
                    }
                    string popupScript = "";
                    popupScript = "<script language='javascript'>" + "window.close();</script>";
                    ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
                }
                else
                {
                    if (int.Parse(HttpContext.Current.Session["KeyVal"].ToString()) != 0)        //________For Update
                    {
                        DateTime today = oDBEngine.GetDate();
                        String value = "cnt_ucc='" + txtClentUcc.Text + "', cnt_salutation=" + CmbSalutation.SelectedItem.Value + ",  cnt_firstName='" + txtFirstNmae.Text + "', cnt_middleName='" + txtMiddleName.Text + "', cnt_lastName='" + txtLastName.Text + "', cnt_shortName='" + txtAliasName.Text + "', cnt_branchId=" + cmbBranch.SelectedItem.Value + ", cnt_sex=" + cmbGender.SelectedItem.Value + ", cnt_maritalStatus=" + cmbMaritalStatus.SelectedItem.Value + ", cnt_DOB='" + txtDOB.Value + "', cnt_anniversaryDate='" + txtAnniversary.Value + "', cnt_legalStatus=" + cmbLegalStatus.SelectedItem.Value + ", cnt_education=" + cmbEducation.SelectedItem.Value + ", cnt_profession=" + cmbProfession.SelectedItem.Value + ", cnt_organization='" + txtOrganization.Text + "', cnt_jobResponsibility=" + cmbJobResponsibility.SelectedItem.Value + ", cnt_designation=" + cmbDesignation.SelectedItem.Value + ", cnt_industry=" + cmbIndustry.SelectedItem.Value + ", cnt_contactSource=" + cmbSource.SelectedItem.Value + ", cnt_referedBy='" + referedID + "', cnt_contactType='LD', cnt_contactStatus=" + cmbContactStatus.SelectedItem.Value + ",cnt_rating='" + cmbRating.SelectedItem.Value + "',cnt_bloodgroup='" + cmbBloodgroup.SelectedItem.Value + "', lastModifyDate ='" + today + "', lastModifyUser=" + HttpContext.Current.Session["userid"]; // + Session["userid"] ;

                        //oDBEngine.RunStoredProcedure(EmployeeUpdate, value);
                        Int32 rowsEffected = oDBEngine.SetFieldValue("tbl_master_lead", value, " cnt_id=" + HttpContext.Current.Session["KeyVal"]);
                    }
                    else               //_________For Insurt
                    {
                        Leads objLeads = new Leads();
                        string dob = string.Empty;
                        string Anniversary = string.Empty;
                        if (txtDOB.Value != null)
                        {
                            dob = txtDOB.Value.ToString();
                        }
                        if (txtAnniversary.Value != null)
                        {
                            Anniversary = Convert.ToString(txtAnniversary.Value);
                        }




                        bool chkAllow = false;
                        string InternalID = objLeads.btnSave_Click_BL(txtClentUcc.Text, CmbSalutation.SelectedItem.Value, txtFirstNmae.Text,
         txtMiddleName.Text, txtLastName.Text, txtAliasName.Text, cmbBranch.SelectedItem.Value,
          cmbGender.SelectedItem.Value, cmbMaritalStatus.SelectedItem.Value, dob, Anniversary,
          cmbLegalStatus.SelectedItem.Value, cmbEducation.SelectedItem.Value, cmbSource.SelectedItem.Value, referedID, cmbBloodgroup.SelectedItem.Value,
          cmbProfession.SelectedItem.Value, txtOrganization.Text, cmbJobResponsibility.SelectedItem.Value, cmbDesignation.SelectedItem.Value, cmbIndustry.SelectedItem.Value,
          cmbContactStatus.SelectedItem.Value, cmbRating.SelectedItem.Value);
                        //HttpContext.Current.Session["KeyVal_InternalID"] = InternalID;
                        string[,] cnt_id = oDBEngine.GetFieldValue(" tbl_master_lead", " cnt_id", " cnt_internalId='" + InternalID + "'", 1);
                        if (cnt_id[0, 0].ToString() != "n")
                        {
                            Response.Redirect("Lead_general.aspx?id=" + cnt_id[0, 0].ToString(), false);
                        }
                    }
                }
            }
            if (Request.QueryString["formtype"] != null)
            {
                if (Request.QueryString["formtype"].ToString() == "leadSales")
                {
                    string popupScript = "";
                    popupScript = "<script language='javascript'>" + "window.opener.location.href=window.opener.location.href;window.close();</script>";
                    ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
                }
                else if (Request.QueryString["formtype"].ToString() == "lead")
                {
                    string popupScript = "";
                    popupScript = "<script language='javascript'>" + "parent.editwin.close();</script>";
                    ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
                }
                else if (Request.QueryString["formtype"].ToString() == "lead123")
                {
                    string popupScript = "";
                    popupScript = "<script language='javascript'>" + "parent.editwin.close();</script>";
                    ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
                }
                else
                {
                    string popupScript = "";
                    popupScript = "<script language='javascript'>" + "window.close();</script>";
                    ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
                }
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }
    }
}