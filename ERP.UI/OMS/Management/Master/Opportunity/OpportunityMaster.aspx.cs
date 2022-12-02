using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;
using EntityLayer.CommonELS;
using System.Web.Services;
using System.Collections.Generic;
using System.Web.Services;
using DataAccessLayer;
using System.Collections;
//using DevExpress.Web.Data; 
//using ERP.OMS.Reports; 
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ERP.OMS.Management.Master.Opportunity
{
    public partial class OpportunityMaster : System.Web.UI.Page
    {
        Int32 ID;
        public string WLanguage = "";
        public string SpLanguage = "";
        string SubAcName = "";
        string segregis = "";
        string UserLastSegment = "";
        CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();
        //string RequestTypes = string.Empty;//added by sanjib 20122016
        //Converter objConverter = new Converter();
        //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        clsDropDownList oclsDropDownList = new clsDropDownList();
        BusinessLogicLayer.Contact oContactGeneralBL = new BusinessLogicLayer.Contact();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        BusinessLogicLayer.RemarkCategoryBL reCat = new BusinessLogicLayer.RemarkCategoryBL();

        public string pageAccess = "";

        protected void Page_Init(object sender, EventArgs e)
        {
           // GstinSettingsButton.contact_type = "CUST";
        }
        protected override void OnPreInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
                if (Request.QueryString["id"] == "ADD")
                {
                    //   DisabledTabPage();
                    base.OnPreInit(e);
                }
            }
        }

        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                //string sPath = HttpContext.Current.Request.Url.ToString();
                //oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Convert.ToString(Request.QueryString["contact_type"]) != "Lead")
            //{
            DataTable dtbranch = new DataTable();
            //LDtxtReferedBy_hidden.Value = "0";

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/Contact_general.aspx");
            //if (HttpContext.Current.Session["userid"] == null)
            //{
            //    // Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            //}

            if (Session["requesttype"] != null) // Lead add/edit
            {
                string RequestType = Convert.ToString(Session["requesttype"]);
                Session["ContactType"] = Convert.ToString(Session["requesttype"]);

                cmbContactStatusclient.Visible = false;
                txtContactStatusclient.Visible = false;

                cmbBranch.Style.Add("Display", "block");
                ASPxLabel10.Style.Add("Display", "block");
                switch (Convert.ToString(Session["requesttype"]))
                {
                    //case "Relationship Partner": //by sanjib due to mismatch
                    case "Relationship Partners":
                        this.Title = "Influencer";

                        ASPxLabel4.Text = "BP/RP Code";
                        pnlCredit.Style.Add("display", "none");
                        ASPxLabel23.Visible = true;
                        txtDateRegis.Visible = true;
                        lblCreditcard.Visible = true;
                        ChkCreditcard.Enabled = false;
                        lblcreditDays.Visible = true;
                        txtcreditDays.Enabled = false;
                        lblCreditLimit.Visible = true;
                        txtCreditLimit.Enabled = false;
                        divSelectEmployee.Style.Add("display", "none");
                        pnlVehicleNo.Style.Add("display", "none");
                        td_EnrollmentId.Style.Add("display", "none");
                        td_registered.Style.Add("display", "none");
                        break;
                    case "Partner":
                        this.Title = "Partner";
                        ASPxLabel4.Text = "BP/RP Code";
                        pnlCredit.Style.Add("display", "none");
                        ASPxLabel23.Visible = true;
                        txtDateRegis.Visible = true;
                        lblCreditcard.Visible = true;
                        ChkCreditcard.Enabled = false;
                        lblcreditDays.Visible = true;
                        txtcreditDays.Enabled = false;
                        lblCreditLimit.Visible = true;
                        txtCreditLimit.Enabled = false;
                        divSelectEmployee.Style.Add("display", "none");
                        pnlVehicleNo.Style.Add("display", "none");
                        td_EnrollmentId.Style.Add("display", "none");
                        break;
                    case "Customer/Client":
                        this.Title = "Customer/Client";
                        pnlCredit.Style.Add("display", "block");
                        cmbBranch.Style.Add("Display", "none");


                        divClientBranch.Style.Add("Display", "none");
                        divSelectEmployee.Style.Add("display", "none");
                        pnlVehicleNo.Style.Add("display", "none");
                        td_registered.Style.Add("display", "block");
                        //coad added By Priti on 16122016                       
                        lblCreditcard.Visible = true;
                        ChkCreditcard.Enabled = true;
                        lblcreditDays.Visible = true;
                        txtcreditDays.Enabled = true;
                        lblCreditLimit.Visible = true;
                        txtCreditLimit.Enabled = true;
                        cmbContactStatusclient.Visible = true;
                        txtContactStatusclient.Visible = true;
                        td_EnrollmentId.Style.Add("display", "none");
                        //divSelectEmployee.Style.Add("display", "none");
                        //pnlVehicleNo.Style.Add("display", "none");
                        //td_registered.Style.Add("display", "none");

                        //...end...
                        break;
                    case "Debtor":
                        this.Title = "Debtor";
                        lblCreditcard.Visible = true;
                        ChkCreditcard.Visible = true;
                        lblcreditDays.Visible = true;
                        txtcreditDays.Visible = true;
                        lblCreditLimit.Visible = true;
                        txtCreditLimit.Visible = true;
                        td_EnrollmentId.Style.Add("display", "none");
                        break;
                    case "Franchisee":
                        pnlCredit.Style.Add("display", "none");
                        this.Title = "Franchisee";
                        ASPxLabel4.Text = "Unique ID";
                        divSelectEmployee.Style.Add("display", "none");
                        pnlVehicleNo.Style.Add("display", "none");
                        td_registered.Style.Add("display", "none");
                        ASPxLabel23.Visible = true;
                        txtDateRegis.Enabled = false;
                        //coad added By Priti on 16122016
                        lblCreditcard.Visible = true;
                        ChkCreditcard.Enabled = false;
                        lblcreditDays.Visible = true;
                        txtcreditDays.Enabled = false;
                        lblCreditLimit.Visible = true;
                        txtCreditLimit.Enabled = false;
                        td_EnrollmentId.Style.Add("display", "none");
                        //divSelectEmployee.Style.Add("display", "none");
                        //pnlVehicleNo.Style.Add("display", "none");
                        //td_registered.Style.Add("display", "none");
                        break;
                    case "Consultant":
                        this.Title = "Consultant";
                        ASPxLabel4.Text = "Unique ID";
                        ASPxLabel23.Visible = true;
                        txtDateRegis.Enabled = false;
                        //coad added By Priti on 16122016
                        lblCreditcard.Visible = true;
                        ChkCreditcard.Enabled = false;
                        lblcreditDays.Visible = true;
                        txtcreditDays.Enabled = false;
                        lblCreditLimit.Visible = true;
                        txtCreditLimit.Enabled = false;
                        pnlVehicleNo.Style.Add("display", "none");
                        td_EnrollmentId.Style.Add("display", "none");
                        td_registered.Style.Add("display", "none");
                        break;
                    case "Share Holder":
                        this.Title = "Share Holders";
                        ASPxLabel4.Text = "Unique ID";
                        ASPxLabel23.Visible = true;
                        txtDateRegis.Enabled = false;
                        //coad added By Priti on 16122016
                        lblCreditcard.Visible = true;
                        ChkCreditcard.Enabled = false;
                        lblcreditDays.Visible = true;
                        txtcreditDays.Enabled = false;
                        lblCreditLimit.Visible = true;
                        txtCreditLimit.Enabled = false;
                        pnlVehicleNo.Style.Add("display", "none");
                        td_EnrollmentId.Style.Add("display", "none");
                        td_registered.Style.Add("display", "none");
                        break;
                    case "Creditors":
                        this.Title = "Creditors";
                        ASPxLabel4.Text = "Unique ID";
                        ASPxLabel23.Visible = true;
                        txtDateRegis.Enabled = false;
                        //coad added By Priti on 16122016
                        lblCreditcard.Visible = true;
                        ChkCreditcard.Enabled = false;
                        lblcreditDays.Visible = true;
                        txtcreditDays.Enabled = false;
                        lblCreditLimit.Visible = true;
                        txtCreditLimit.Enabled = false;
                        pnlVehicleNo.Style.Add("display", "none");
                        td_EnrollmentId.Style.Add("display", "none");
                        td_registered.Style.Add("display", "none");
                        break;
                    case "OtherEntity":
                        this.Title = "Other Entity";
                        ASPxLabel4.Text = "Unique ID";
                        ASPxLabel23.Visible = true;
                        txtDateRegis.Enabled = false;
                        //coad added By Priti on 16122016
                        pnlCredit.Style.Add("display", "none");
                        lblCreditcard.Visible = true;
                        ChkCreditcard.Enabled = false;
                        lblcreditDays.Visible = true;
                        txtcreditDays.Enabled = false;
                        lblCreditLimit.Visible = true;
                        txtCreditLimit.Enabled = false;
                        divSelectEmployee.Style.Add("display", "none");
                        td_EnrollmentId.Style.Add("display", "none");
                        pnlVehicleNo.Style.Add("display", "none");
                        td_registered.Style.Add("display", "none");
                        break;
                    case "Salesman/Agents":
                        this.Title = "Salesman/Agents";
                        ASPxLabel4.Text = "Unique ID";
                        ASPxLabel23.Visible = true;
                        txtDateRegis.Enabled = false;
                        ASPxLabel20.Text = "Salesman/Agent Type";
                        //coad added By Priti on 16122016
                        pnlCredit.Style.Add("display", "none");
                        divSelectEmployee.Style.Add("display", "block");
                        pnlVehicleNo.Style.Add("display", "none");
                        td_registered.Style.Add("display", "none");
                        td_EnrollmentId.Style.Add("display", "none");
                        lblCreditcard.Visible = true;
                        ChkCreditcard.Enabled = false;
                        lblcreditDays.Visible = true;
                        txtcreditDays.Enabled = false;
                        lblCreditLimit.Visible = true;
                        txtCreditLimit.Enabled = false;
                        cmbContactStatusclient.Visible = true;
                        txtContactStatusclient.Visible = true;
                        //divSelectEmployee.Style.Add("display", "block");
                        //pnlVehicleNo.Style.Add("display", "none");
                        //td_registered.Style.Add("display", "none");
                        break;
                    case "Transporter":
                        this.Title = "Transporter Details";
                        ASPxLabel20.Text = "Transporter Type";
                        pnlCredit.Style.Add("display", "none");
                        td_registered.Style.Add("display", "block");
                        lblHeadTitle.Text = "Add / Edit Transporter Details";
                        lblCreditcard.Visible = false;
                        ChkCreditcard.Enabled = false;
                        lblcreditDays.Visible = false;
                        txtcreditDays.Enabled = false;
                        lblCreditLimit.Visible = false;
                        txtCreditLimit.Enabled = false;
                        cmbContactStatusclient.Visible = true;
                        txtContactStatusclient.Visible = true;
                        divSelectEmployee.Style.Add("display", "none");
                        pnlVehicleNo.Style.Add("display", "block");
                        td_EnrollmentId.Style.Add("display", "block");

                        break;
                    case "Opportunity":
                        this.Title = "Opportunity";
                        ASPxLabel23.Visible = true;
                        txtDateRegis.Enabled = false;
                        //coad added By Priti on 16122016
                        pnlCredit.Style.Add("display", "none");
                        lblCreditcard.Visible = true;
                        ChkCreditcard.Enabled = false;
                        lblcreditDays.Visible = true;
                        txtcreditDays.Enabled = false;
                        lblCreditLimit.Visible = true;
                        txtCreditLimit.Enabled = false;
                        pnlVehicleNo.Style.Add("display", "none");
                        td_EnrollmentId.Style.Add("display", "none");
                        td_registered.Style.Add("display", "none");

                        break;

                    default: //lead add/edit
                        this.Title = "Lead";
                        ASPxLabel4.Text = "Unique ID";
                        ASPxLabel23.Visible = true;
                        txtDateRegis.Enabled = false;
                        //coad added By Priti on 16122016
                        pnlCredit.Style.Add("display", "none");
                        lblCreditcard.Visible = true;
                        ChkCreditcard.Enabled = false;
                        lblcreditDays.Visible = true;
                        txtcreditDays.Enabled = false;
                        lblCreditLimit.Visible = true;
                        txtCreditLimit.Enabled = false;
                        pnlVehicleNo.Style.Add("display", "none");
                        td_EnrollmentId.Style.Add("display", "none");
                        td_registered.Style.Add("display", "none");
                        //...end...
                        break;
                }

            }

            DateTime dt = oDBEngine.GetDate();

            if (!IsPostBack)
            {

                // GetFinacialYearBasedQouteDate();
                if (Session["requesttype"] != null)
                {
                    radioregistercheck.Attributes.Add("onclick", "registeredCheckChangeEvent()");
                    //GstinSettingsButton.Visible = false;
                    if (Convert.ToString(Session["requesttype"]) == "Relationship Manager")
                    {

                        lblHeadTitle.Text = "Add/Edit Salesman/Agents";
                    }
                    else if (Convert.ToString(Session["requesttype"]) == "Partner" || Convert.ToString(Session["requesttype"]) == "businesspartner")
                    {
                        lblHeadTitle.Text = "Add/Edit Business Partners";
                    }
                    else if (Convert.ToString(Session["requesttype"]) == "OtherEntity")
                    {
                        lblHeadTitle.Text = "Add/Edit Other Entity";
                    }
                    else if (Convert.ToString(Session["requesttype"]) == "Relationship Partners")
                    {
                        lblHeadTitle.Text = "Add/Edit Influencer";
                    }
                    else
                    {
                        lblHeadTitle.Text = "Add/Edit " + Convert.ToString(Session["requesttype"]);
                    }

                }



                if (Convert.ToString(Session["requesttype"]) != "Lead" && Convert.ToString(Session["requesttype"]) != "Opportunity")
                {

                    table_others.Visible = true;
                    table_leads.Visible = false;

                    if (Session["serverdate"] != null)
                    {
                        txtFromDate.Date = Convert.ToDateTime(Convert.ToString(Session["serverdate"]));
                    }

                    //if (Session["Name"] != null)
                    //{
                    //    //lblName.Text = Session["Name"].ToString();
                    //}
                    cmbProfession.Attributes.Add("onchange", "javascript:ProfessionStatus()");
                    cmbContactStatus.Attributes.Add("onchange", "javascript:ContactStatus()");
                    cmbSource.Attributes.Add("onchange", "javascript:SourceStatus()");



                    txtReferedBy.Attributes.Add("onkeyup", "CallList(this,'referedby',event)");
                    txtRPartner.Attributes.Add("onkeyup", "ajax_showOptions(this,'SearchByEmployees',event,'1','Main')");
                    // Code Added and Commented By Sam on 15112016 to avoid autocomplete and using dropdown                



                    //if (Request.QueryString["formtype"] != null)
                    //{
                    //    string Internal_ID = Convert.ToString(Session["InternalId"]);
                    //    DDLBind();
                    //    string[,] ContactData;
                    //    ContactData = oDBEngine.GetFieldValue("tbl_master_lead", "cnt_ucc,cnt_salutation,cnt_firstName,cnt_middleName,cnt_lastName,cnt_shortName,cnt_branchId,cnt_sex,cnt_maritalStatus,case cnt_DOB when '1/1/1900 12:00:00 AM' then null else cnt_DOB end as cnt_DOB,case cnt_anniversaryDate when '1/1/1900 12:00:00 AM' then null else cnt_anniversaryDate end as cnt_anniversaryDate,cnt_legalStatus,cnt_education,cnt_profession,cnt_organization,cnt_jobResponsibility,cnt_designation,cnt_industry,cnt_contactSource,cnt_referedBy,cnt_contactType,cnt_contactStatus", " cnt_internalId='" + Internal_ID + "'", 22);
                    //    ValueAllocation(ContactData);
                    //    DisabledTabPage();
                    //}

                    //else
                    //{
                    //if (Request.QueryString["requesttypeP"] != null)
                    //{
                    //    string Internal_ID = Convert.ToString(Session["LeadId"]);
                    //    DDLBind();
                    //    string[,] ContactData;
                    //    ContactData = oDBEngine.GetFieldValue("tbl_master_lead", "cnt_ucc,cnt_salutation,cnt_firstName,cnt_middleName,cnt_lastName,cnt_shortName,cnt_branchId,cnt_sex,cnt_maritalStatus,case cnt_DOB when '1/1/1900 12:00:00 AM' then null else cnt_DOB end as cnt_DOB,case cnt_anniversaryDate when '1/1/1900 12:00:00 AM' then null else cnt_anniversaryDate end as cnt_anniversaryDate,cnt_legalStatus,cnt_education,cnt_profession,cnt_organization,cnt_jobResponsibility,cnt_designation,cnt_industry,cnt_contactSource,cnt_referedBy,cnt_contactType,cnt_contactStatus", " cnt_internalId='" + Internal_ID + "'", 22);
                    //    ValueAllocation(ContactData);
                    //    DisabledTabPage();
                    //}
                    //else
                    //{
                    //Binding of comboBox start here//
                    //------------------------------//

                    DDLBind();

                    if (Convert.ToString(Session["requesttype"]) == "Transporter")
                    {
                        if (cmbBranch.Items[0].ToString() != "--All--")
                        {
                            cmbBranch.Items.Insert(0, new ListItem("--All--", "0"));
                        }
                    }
                    //          End Here            //
                    //------------------------------//
                    if (Request.QueryString["id"] != "ADD")
                    {
                        //Debjyoti Code Added 03-02-17
                        //Reason: Short Name Should be read only in case of edit customer
                        //if (Convert.ToString(Session["requesttype"]) == "Customer/Client")
                        //{
                        txtClentUcc.Enabled = false;
                        ddlIdType.Enabled = false;
                        // }

                        hddnApplicationMode.Value = "E";
                        if (Request.QueryString["id"] != null)
                        {
                            ID = Int32.Parse(Request.QueryString["id"]);
                            HttpContext.Current.Session["KeyVal"] = ID;
                        }
                        string[,] InternalId;

                        if (ID != 0)
                        {
                            InternalId = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_internalId,cnt_firstname", "cnt_id=" + ID, 2);
                            // HiddenField = InternalId[0, 0];
                        }
                        else
                        {
                            InternalId = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_internalId,cnt_firstname", "cnt_id=" + HttpContext.Current.Session["KeyVal"], 2);
                        }
                        HttpContext.Current.Session["KeyVal_InternalID"] = InternalId[0, 0];
                        KeyVal_InternalID.Value = InternalId[0, 0];

                        if (InternalId[0, 0] != "n")
                        {
                            HttpContext.Current.Session["name"] = InternalId[0, 1];
                        }

                        string[,] ContactData;
                        if (ID != 0)
                        {
                            ContactData = oDBEngine.GetFieldValue("tbl_master_contact",
                                                    "cnt_ucc, cnt_salutation,  cnt_firstName, cnt_middleName, cnt_lastName, cnt_shortName, cnt_branchId, cnt_sex, cnt_maritalStatus, case cnt_DOB when '1/1/1900 12:00:00 AM' then null else cnt_DOB end as cnt_DOB, case cnt_anniversaryDate when '1/1/1900 12:00:00 AM' then null else cnt_anniversaryDate end as cnt_anniversaryDate, cnt_legalStatus, cnt_education, cnt_profession, cnt_organization, cnt_jobResponsibility, cnt_designation, cnt_industry, cnt_contactSource, cnt_referedBy, cnt_contactType, cnt_contactStatus,case cnt_RegistrationDate when '1/1/1900 12:00:00 AM' then null else cnt_RegistrationDate end as cnt_RegistrationDate,cnt_rating,cnt_reason,cnt_bloodgroup,WebLogin,isnull(cnt_placeofincorporation,''),isnull(cnt_BusinessComncDate,''),isnull(cnt_OtherOccupation,''),isnull(cnt_nationality,'1'),cnt_IsCreditHold,cnt_CreditDays,cnt_CreditLimit,Statustype,CNT_GSTIN,cnt_AssociatedEmp,cnt_mainAccount,cnt_IdType,Enrolment_ID,cnt_PrintNameToCheque",
                                                    " cnt_id=" + ID, 41);
                        }
                        else
                        {
                            ContactData = oDBEngine.GetFieldValue("tbl_master_contact",
                                                    "cnt_ucc, cnt_salutation,  cnt_firstName, cnt_middleName, cnt_lastName, cnt_shortName, cnt_branchId, cnt_sex, cnt_maritalStatus, case cnt_DOB when '1/1/1900 12:00:00 AM' then null else cnt_DOB end as cnt_DOB, case cnt_anniversaryDate when '1/1/1900 12:00:00 AM' then null else cnt_anniversaryDate end as cnt_anniversaryDate, cnt_legalStatus, cnt_education, cnt_profession, cnt_organization, cnt_jobResponsibility, cnt_designation, cnt_industry, cnt_contactSource, cnt_referedBy, cnt_contactType, cnt_contactStatus,case cnt_RegistrationDate when '1/1/1900 12:00:00 AM' then null else cnt_RegistrationDate end as cnt_RegistrationDate,cnt_rating,cnt_reason,cnt_bloodgroup,WebLogin,isnull(cnt_placeofincorporation,''),isnull(cnt_BusinessComncDate,''),isnull(cnt_OtherOccupation,''),isnull(cnt_nationality,'1'),cnt_IsCreditHold,cnt_CreditDays,cnt_CreditLimit,Statustype,CNT_GSTIN,cnt_AssociatedEmp,cnt_mainAccount,cnt_IdType,Enrolment_ID,cnt_PrintNameToCheque",
                                                    " cnt_id=" + HttpContext.Current.Session["KeyVal"], 41);
                        }

                        //____________ Value Allocation _______________//
                        if (Convert.ToString(Session["requesttype"]) == "OtherEntity")
                        {

                            txtClentUcc.Visible = true;
                            ddlIdType.Visible = true;
                            ASPxLabel12.Visible = true;

                            LinkButton1.Visible = false;
                            ASPxLabelS12.Visible = false;
                            td_star.Style.Add("color", "#DDECFE");
                            TabPage page = ASPxPageControl1.TabPages.FindByName("Correspondence");
                            page.Visible = true;
                            page = ASPxPageControl1.TabPages.FindByName("BankDetails");
                            page.Visible = false;
                            page = ASPxPageControl1.TabPages.FindByName("DPDetails");
                            page.Visible = false;
                            page = ASPxPageControl1.TabPages.FindByName("Documents");
                            page.Visible = true;
                            page = ASPxPageControl1.TabPages.FindByName("FamilyMembers");
                            page.Visible = false;
                            page = ASPxPageControl1.TabPages.FindByName("Registration");
                            page.Visible = false;
                            page = ASPxPageControl1.TabPages.FindByName("GroupMember");
                            page.Visible = false;
                            page = ASPxPageControl1.TabPages.FindByName("Deposit");
                            page.Visible = false;
                            page = ASPxPageControl1.TabPages.FindByName("Remarks");
                            page.Visible = true;
                            page = ASPxPageControl1.TabPages.FindByName("Education");
                            page.Visible = false;
                            page = ASPxPageControl1.TabPages.FindByName("Trad. Prof.");
                            page.Visible = false;
                            page = ASPxPageControl1.TabPages.FindByName("Other");
                            page.Visible = false;
                            page = ASPxPageControl1.TabPages.FindByName("Subscription");
                            page.Visible = false;

                        }
                        ValueAllocation(ContactData);
                        Contact cts = new Contact();


                        if (Convert.ToString(Session["requesttype"]) == "Transporter")
                        {

                            if (Convert.ToString(cmbLegalStatus.SelectedValue) == "54")//Local
                            { pnlVehicleNo.Style.Add("display", "block"); }
                            else { pnlVehicleNo.Style.Add("display", "none"); }

                            TabPage page = ASPxPageControl1.TabPages.FindByName("GroupMember");
                            page.Visible = false;

                            DataTable TransporterVehicles = cts.Get_TransporterVehicles(InternalId[0, 0]);
                            if (TransporterVehicles != null)
                            {
                                if (TransporterVehicles.Rows.Count > 0)
                                {
                                    VehicleNo_hidden.Value = Convert.ToString(TransporterVehicles.Rows[0][0]);
                                }
                            }
                        }

                        if (Convert.ToString(Session["requesttype"]) == "Customer/Client")
                        {
                            //GstinSettingsButton.Visible = true;
                        }

                    }
                    else
                    {
                        hddnApplicationMode.Value = "A";
                        TrLang.Visible = false;
                        CmbSalutation.SelectedIndex.Equals(0);
                        HttpContext.Current.Session["KeyVal_InternalID"] = null;
                        //For Udf data
                        KeyVal_InternalID.Value = "Add";
                        txtFirstNmae.Text = "";
                        txtMiddleName.Text = "";
                        txtLastName.Text = "";
                        txtAliasName.Text = "";

                        cmbBranch.SelectedIndex.Equals(0);
                        cmbGender.SelectedIndex.Equals(0);
                        cmbMaritalStatus.SelectedIndex.Equals(0);
                        txtDOB.Value = "";
                        txtAnniversary.Value = "";
                        cmbLegalStatus.SelectedIndex.Equals(0);

                        cmbEducation.SelectedIndex.Equals(0);
                        cmbProfession.SelectedIndex.Equals(0);
                        cmbJobResponsibility.SelectedIndex.Equals(0);
                        cmbDesignation.SelectedIndex.Equals(0);
                        cmbIndustry.SelectedIndex.Equals(0);
                        cmbSource.SelectedIndex.Equals(0);
                        cmbContactStatus.SelectedIndex.Equals(0);

                        cmbContactStatusclient.SelectedIndex.Equals(0);

                        //----Making TABs Disable------//
                        DisabledTabPage();

                        //-----End---------------------//
                        HttpContext.Current.Session["KeyVal"] = "0";
                    }



                    //  }
                    // }
                    if (Session["requesttype"] != null)
                    {
                        if (Convert.ToString(Session["requesttype"]) == "Relationship Manager")
                        {
                            ASPxLabel5.Text = "Name";
                            ASPxLabel4.Text = "Code";
                            cmbLegalStatus.SelectedValue = "3";
                            CmbSalutation.SelectedValue = "24";
                        }
                        if (Convert.ToString(Session["requesttype"]) == "OtherEntity")
                        {

                            txtClentUcc.Visible = true;
                            ddlIdType.Visible = true;
                            ASPxLabel12.Visible = true;

                            LinkButton1.Visible = false;
                            ASPxLabelS12.Visible = false;
                            td_star.Style.Add("color", "#DDECFE");
                        }
                        if (Convert.ToString(Session["requesttype"]) == "Transporter")
                        {
                            if (Convert.ToString(cmbLegalStatus.SelectedValue) == "54")//Local
                            { pnlVehicleNo.Style.Add("display", "block"); }
                            else { pnlVehicleNo.Style.Add("display", "none"); }
                        }
                    }
                }
                else //Leads add/edit
                {
                    table_others.Visible = false;
                    table_leads.Visible = true;
                    TabPage pageFM = ASPxPageControl1.TabPages.FindByName("FamilyMembers");
                    pageFM.Visible = true;
                    //if (Request.QueryString["formtype"] != null)
                    //{
                    //    string Internal_ID = Convert.ToString(Session["InternalId"]);
                    //    string IID = Internal_ID.Substring(0, 2);
                    //    LD_DDLBind();
                    //    string[,] ContactData;
                    //    if (IID == "LD")
                    //    {
                    //        ContactData = oDBEngine.GetFieldValue("tbl_master_lead", "cnt_ucc,cnt_salutation,cnt_firstName,cnt_middleName,cnt_lastName,cnt_shortName,cnt_branchId,cnt_sex,cnt_maritalStatus,case cnt_DOB when '1/1/1900 12:00:00 AM' then null else cnt_DOB end as cnt_DOB,case cnt_anniversaryDate when '1/1/1900 12:00:00 AM' then null else cnt_anniversaryDate end as cnt_anniversaryDate,cnt_legalStatus,cnt_education,cnt_profession,cnt_organization,cnt_jobResponsibility,cnt_designation,cnt_industry,cnt_contactSource,cnt_referedBy,cnt_contactType,cnt_contactStatus,cnt_rating,cnt_bloodgroup,cnt_AssociatedEmp", " cnt_internalId='" + Internal_ID + "'", 25);
                    //    }
                    //    else
                    //    {
                    //        ContactData = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_ucc,cnt_salutation,cnt_firstName,cnt_middleName,cnt_lastName,cnt_shortName,cnt_branchId,cnt_sex,cnt_maritalStatus,case cnt_DOB when '1/1/1900 12:00:00 AM' then null else cnt_DOB end as cnt_DOB,case cnt_anniversaryDate when '1/1/1900 12:00:00 AM' then null else cnt_anniversaryDate end as cnt_anniversaryDate,cnt_legalStatus,cnt_education,cnt_profession,cnt_organization,cnt_jobResponsibility,cnt_designation,cnt_industry,cnt_contactSource,cnt_referedBy,cnt_contactType,cnt_contactStatus,cnt_rating,cnt_bloodgroup,cnt_AssociatedEmp", " cnt_internalId='" + Internal_ID + "'", 25);
                    //    }
                    //    LD_ValueAllocation(ContactData);
                    //    DisabledTabPage();
                    //}
                    //else // lead edit
                    //{
                    //if (Request.QueryString["requesttypeP"] != null)
                    //{
                    //    string Internal_ID = Convert.ToString(Session["LeadId"]);
                    //    string IID = Internal_ID.Substring(0, 2);
                    //    LD_DDLBind();
                    //    string[,] ContactData;
                    //    if (IID == "LD")
                    //    {
                    //        ContactData = oDBEngine.GetFieldValue("tbl_master_lead", "cnt_ucc,cnt_salutation,cnt_firstName,cnt_middleName,cnt_lastName,cnt_shortName,cnt_branchId,cnt_sex,cnt_maritalStatus,case cnt_DOB when '1/1/1900 12:00:00 AM' then null else cnt_DOB end as cnt_DOB,case cnt_anniversaryDate when '1/1/1900 12:00:00 AM' then null else cnt_anniversaryDate end as cnt_anniversaryDate,cnt_legalStatus,cnt_education,cnt_profession,cnt_organization,cnt_jobResponsibility,cnt_designation,cnt_industry,cnt_contactSource,cnt_referedBy,cnt_contactType,cnt_contactStatus,cnt_rating,cnt_bloodgroup", " cnt_internalId='" + Internal_ID + "'", 24);
                    //    }
                    //    else
                    //    {
                    //        ContactData = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_ucc,cnt_salutation,cnt_firstName,cnt_middleName,cnt_lastName,cnt_shortName,cnt_branchId,cnt_sex,cnt_maritalStatus,case cnt_DOB when '1/1/1900 12:00:00 AM' then null else cnt_DOB end as cnt_DOB,case cnt_anniversaryDate when '1/1/1900 12:00:00 AM' then null else cnt_anniversaryDate end as cnt_anniversaryDate,cnt_legalStatus,cnt_education,cnt_profession,cnt_organization,cnt_jobResponsibility,cnt_designation,cnt_industry,cnt_contactSource,cnt_referedBy,cnt_contactType,cnt_contactStatus,cnt_rating,cnt_bloodgroup", " cnt_internalId='" + Internal_ID + "'", 24);
                    //    }
                    //    LD_ValueAllocation(ContactData);
                    //    DisabledTabPage();
                    //}
                    //else  // lead add/edit
                    //{
                    if (Request.QueryString["id"] != "ADD") // lead edit
                    {
                        txtClentUcc.Enabled = false;
                        ddlIdType.Visible = false;
                        if (Request.QueryString["id"] != null) // lead edit
                        {
                            ID = Int32.Parse(Request.QueryString["id"]);
                            HttpContext.Current.Session["KeyVal"] = ID;
                            string[,] InternalId;
                            InternalId = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_internalId,cnt_firstname", "cnt_id=" + ID, 1);

                            HttpContext.Current.Session["KeyVal_InternalID"] = InternalId[0, 0];
                            //for Udf Data
                            KeyVal_InternalID.Value = InternalId[0, 0];
                        }
                        string[,] ContactData;
                        if (ID != 0) // lead edit
                        {
                            //ContactData = oDBEngine.GetFieldValue("tbl_master_lead",
                            //                        "cnt_ucc, cnt_salutation,  cnt_firstName, cnt_middleName, cnt_lastName, cnt_shortName, cnt_branchId, cnt_sex, cnt_maritalStatus, case cnt_DOB when '1/1/1900 12:00:00 AM' then null else cnt_DOB end as cnt_DOB,case cnt_anniversaryDate when '1/1/1900 12:00:00 AM' then null else cnt_anniversaryDate end as cnt_anniversaryDate, cnt_legalStatus, cnt_education, cnt_profession, cnt_organization, cnt_jobResponsibility, cnt_designation, cnt_industry, cnt_contactSource, cnt_referedBy, cnt_contactType, cnt_contactStatus,cnt_rating,cnt_bloodgroup",
                            //                        " cnt_id=" + ID, 24);


                            // Code  Added and Commented By Sam on 15112016 to increase the number of field from 24 to 30 

                            //Rev Subhra-----03-06-2019

                            //ContactData = oDBEngine.GetFieldValue("tbl_master_contact",
                            //                        "cnt_ucc, cnt_salutation,  cnt_firstName, cnt_middleName, cnt_lastName, cnt_shortName, cnt_branchId, cnt_sex, cnt_maritalStatus, case cnt_DOB when '1/1/1900 12:00:00 AM' then null else cnt_DOB end as cnt_DOB, case cnt_anniversaryDate when '1/1/1900 12:00:00 AM' then null else cnt_anniversaryDate end as cnt_anniversaryDate, cnt_legalStatus, cnt_education, cnt_profession, cnt_organization, cnt_jobResponsibility, cnt_designation, cnt_industry, cnt_contactSource, cnt_referedBy, cnt_contactType, cnt_contactStatus,case cnt_RegistrationDate when '1/1/1900 12:00:00 AM' then null else cnt_RegistrationDate end as cnt_RegistrationDate,cnt_rating,cnt_reason,cnt_bloodgroup,WebLogin,isnull(cnt_placeofincorporation,''),isnull(cnt_BusinessComncDate,''),isnull(cnt_OtherOccupation,''),isnull(cnt_nationality,'1'),cnt_IsCreditHold,cnt_CreditDays,cnt_CreditLimit,cnt_PrintNameToCheque ",
                            //                        " cnt_id=" + ID, 31);
                            ContactData = oDBEngine.GetFieldValue("tbl_master_contact",
                                                    "cnt_ucc, cnt_salutation,  cnt_firstName, cnt_middleName, cnt_lastName, cnt_shortName, cnt_branchId, cnt_sex, cnt_maritalStatus, case cnt_DOB when '1/1/1900 12:00:00 AM' then null else cnt_DOB end as cnt_DOB, case cnt_anniversaryDate when '1/1/1900 12:00:00 AM' then null else cnt_anniversaryDate end as cnt_anniversaryDate, cnt_legalStatus, cnt_education, cnt_profession, cnt_organization, cnt_jobResponsibility, cnt_designation, cnt_industry, cnt_contactSource, cnt_referedBy, cnt_contactType, cnt_contactStatus,case cnt_RegistrationDate when '1/1/1900 12:00:00 AM' then null else cnt_RegistrationDate end as cnt_RegistrationDate,cnt_rating,cnt_reason,cnt_bloodgroup,WebLogin,isnull(cnt_placeofincorporation,''),isnull(cnt_BusinessComncDate,''),isnull(cnt_OtherOccupation,''),isnull(cnt_nationality,'1'),cnt_IsCreditHold,cnt_CreditDays,cnt_CreditLimit,cnt_PrintNameToCheque,case EnteredDate when '1/1/1900 12:00:00 AM' then null else EnteredDate end as EnteredDate ",
                                                    " cnt_id=" + ID, 36);

                            //End of Rev Subhra-----03-06-2019

                            //ContactData = oDBEngine.GetFieldValue("tbl_master_contact",
                            //                        "cnt_ucc, cnt_salutation,  cnt_firstName, cnt_middleName, cnt_lastName, cnt_shortName, cnt_branchId, cnt_sex, cnt_maritalStatus, case cnt_DOB when '1/1/1900 12:00:00 AM' then null else cnt_DOB end as cnt_DOB, case cnt_anniversaryDate when '1/1/1900 12:00:00 AM' then null else cnt_anniversaryDate end as cnt_anniversaryDate, cnt_legalStatus, cnt_education, cnt_profession, cnt_organization, cnt_jobResponsibility, cnt_designation, cnt_industry, cnt_contactSource, cnt_referedBy, cnt_contactType, cnt_contactStatus,case cnt_RegistrationDate when '1/1/1900 12:00:00 AM' then null else cnt_RegistrationDate end as cnt_RegistrationDate,cnt_rating,cnt_reason,cnt_bloodgroup,WebLogin,isnull(cnt_placeofincorporation,''),isnull(cnt_BusinessComncDate,''),isnull(cnt_OtherOccupation,''),isnull(cnt_nationality,'1') ",
                            //                        " cnt_id=" + ID, 24);

                            // Code Above Added and Commented By Sam on 15112016 to  increase the number of field from 24 to 30 


                        }
                        else
                        {
                            //ContactData = oDBEngine.GetFieldValue("tbl_master_lead",
                            //                        "cnt_ucc, cnt_salutation,  cnt_firstName, cnt_middleName, cnt_lastName, cnt_shortName, cnt_branchId, cnt_sex, cnt_maritalStatus, case cnt_DOB when '1/1/1900 12:00:00 AM' then null else cnt_DOB end as cnt_DOB,case cnt_anniversaryDate when '1/1/1900 12:00:00 AM' then null else cnt_anniversaryDate end as cnt_anniversaryDate, cnt_legalStatus, cnt_education, cnt_profession, cnt_organization, cnt_jobResponsibility, cnt_designation, cnt_industry, cnt_contactSource, cnt_referedBy, cnt_contactType, cnt_contactStatus,cnt_rating,cnt_bloodgroup",
                            //                        " cnt_id=" + HttpContext.Current.Session["KeyVal"], 24);
                            //Rev Subhra-----03-06-2019
                            ContactData = oDBEngine.GetFieldValue("tbl_master_contact",
                            "cnt_ucc, cnt_salutation,  cnt_firstName, cnt_middleName, cnt_lastName, cnt_shortName, cnt_branchId, cnt_sex, cnt_maritalStatus, case cnt_DOB when '1/1/1900 12:00:00 AM' then null else cnt_DOB end as cnt_DOB, case cnt_anniversaryDate when '1/1/1900 12:00:00 AM' then null else cnt_anniversaryDate end as cnt_anniversaryDate, cnt_legalStatus, cnt_education, cnt_profession, cnt_organization, cnt_jobResponsibility, cnt_designation, cnt_industry, cnt_contactSource, cnt_referedBy, cnt_contactType, cnt_contactStatus,case cnt_RegistrationDate when '1/1/1900 12:00:00 AM' then null else cnt_RegistrationDate end as cnt_RegistrationDate,cnt_rating,cnt_reason,cnt_bloodgroup,WebLogin,isnull(cnt_placeofincorporation,''),isnull(cnt_BusinessComncDate,''),isnull(cnt_OtherOccupation,''),isnull(cnt_nationality,'1'),cnt_IsCreditHold,cnt_CreditDays,cnt_CreditLimit,cnt_PrintNameToCheque,case EnteredDate when '1/1/1900 12:00:00 AM' then null else EnteredDate end as EnteredDate ",
                            " cnt_id=" + HttpContext.Current.Session["KeyVal"], 36);

                            //End of Rev Subhra-----03-06-2019
                        }
                        LD_DDLBind(); // lead edit
                        LD_ValueAllocation(ContactData); //lead edit

                    }
                    else // Lead Add
                    {

                        LD_DDLBind();
                        LDCmbSalutation.SelectedIndex.Equals(0);
                        LDtxtFirstNmae.Text = "";
                        LDtxtMiddleName.Text = "";
                        LDtxtLastName.Text = "";
                        LDtxtAliasName.Text = "";
                        LDcmbBranch.SelectedIndex.Equals(0);
                        LDcmbGender.SelectedIndex.Equals(0);
                        LDcmbMaritalStatus.SelectedIndex = 4;
                        //cmbDOB.Value = "";
                        LDtxtDOB.Value = "";
                        //cmbAnniversary.Value = "";
                        LDtxtAnniversary.Value = "";
                        LDcmbLegalStatus.SelectedIndex.Equals(0);
                        LDcmbEducation.SelectedIndex.Equals(0);
                        LDcmbProfession.SelectedIndex.Equals(0);
                        LDcmbJobResponsibility.SelectedIndex.Equals(0);
                        LDcmbDesignation.SelectedIndex.Equals(0);
                        LDcmbIndustry.SelectedIndex.Equals(0);
                        LDcmbSource.SelectedIndex.Equals(0);
                        LDcmbContactStatus.SelectedIndex.Equals(0);
                        //----Making TABs Disable------//
                        DisabledTabPage();
                        //page = ASPxPageControl1.TabPages.FindByName("EmployeeCTC");
                        //page.Enabled = false;
                        //-----End---------------------//
                        HttpContext.Current.Session["KeyVal"] = 0;
                        KeyVal_InternalID.Value = "Add";
                    }
                    //}
                    // }
                }
            }

            if (Convert.ToString(Session["requesttype"]) != "Lead" || Convert.ToString(Session["requesttype"]) != "Opportunity")
            {
                if (HttpContext.Current.Session["userlastsegment"] != null)
                {
                    UserLastSegment = Convert.ToString(HttpContext.Current.Session["userlastsegment"]);
                    if (UserLastSegment != "1" && UserLastSegment != "4" && UserLastSegment != "6" && UserLastSegment != "9" && UserLastSegment != "10")
                    {
                        if (Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]).Trim() == "1")
                            segregis = "NSE - CM";
                        if (Convert.ToString(HttpContext.Current.Session["ExchangeSegmentID"]).Trim() == "4")
                            segregis = "BSE - CM";


                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "jAlert('Session has timed out');", true);
                    Response.Redirect("~/OMS/login.aspx");
                }
            }
            else //Lead Add/Edit
            {
                DateTime dtLD = oDBEngine.GetDate();
                txtReferedBy.Attributes.Add("onkeyup", "LDCallList(this,'referedby',event)");
                //________This script is for firing javascript when page load first___//
                if (!ClientScript.IsStartupScriptRegistered("Today"))
                    ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>AtTheTimePageLoad();</script>");
                //______________________________End Script____________________________//
            }

            SetUdfApplicableValue();

            if (Request.QueryString["id"] == "ADD" && ddlnational.Items.Count != 0)
            {

                ddlnational.ClearSelection(); //making sure the previous selection has been cleared
                ddlnational.Items.FindByValue("78").Selected = true;
            }

            string[,] Data;
            Data = oDBEngine.GetFieldValue("tbl_master_branch", "branch_id, branch_description ", null, 2, "branch_description");

            if (Session["userbranchID"] != "")
            {

                oclsDropDownList.AddDataToDropDownList(Data, LDcmbBranch, Int32.Parse(Session["userbranchID"].ToString()));
                if (Convert.ToString(Session["requesttype"]) == "Transporter")
                {
                    if (cmbBranch.Items[0].ToString() != "--All--")
                    {
                        cmbBranch.Items.Insert(0, new ListItem("--All--", "0"));
                    }
                }
            }
            #region time
            dt_EnteredOn.TimeSectionProperties.Visible = true;
            dt_EnteredOn.UseMaskBehavior = true;
            dt_EnteredOn.EditFormatString = "dd-MM-yyyy hh:mm tt";
            #endregion
        }


        public void GetFinacialYearBasedQouteDate()
        {
            String finyear = "";
            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            if (Session["LastFinYear"] != null)
            {
                finyear = Convert.ToString(Session["LastFinYear"]).Trim();
                DataTable dtFinYear = objSlaesActivitiesBL.GetFinacialYearBasedQouteDate(finyear);
                if (dtFinYear != null && dtFinYear.Rows.Count > 0)
                {
                    Session["FinYearStartDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearStartDate"]);
                    Session["FinYearEndDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearEndDate"]);
                    if (Session["FinYearStartDate"] != null)
                    {
                        dt_ApplicableFrom.MinDate = Convert.ToDateTime(Convert.ToString(Session["FinYearStartDate"]));
                    }
                    if (Session["FinYearEndDate"] != null)
                    {
                        dt_ApplicableFrom.MaxDate = Convert.ToDateTime(Convert.ToString(Session["FinYearEndDate"]));
                        // dt_ApplicableFrom.MaxDate = DateTime.Now;
                    }
                }
            }
            //dt_PLQuote.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
        }

        protected void SetUdfApplicableValue()
        {
            if (Convert.ToString(Session["requesttype"]) == "Customer/Client")
            {
                hdKeyVal.Value = "Cus";
            }
            else if (Convert.ToString(Session["requesttype"]) == "Relationship Partners")
            {
                hdKeyVal.Value = "RP";
            }
            else if (Convert.ToString(Session["requesttype"]) == "Sub Broker")
            {
                hdKeyVal.Value = "Sb";
            }
            else if (Convert.ToString(Session["requesttype"]) == "Franchisee")
            {
                hdKeyVal.Value = "fr";
            }
            else if (Convert.ToString(Session["requesttype"]) == "Data Vendor")
            {
                hdKeyVal.Value = "DV";
            }
            else if (Convert.ToString(Session["requesttype"]) == "Consultant")
            {
                hdKeyVal.Value = "CNS";
            }
            else if (Convert.ToString(Session["requesttype"]) == "Partner")
            {
                hdKeyVal.Value = "BP";
            }
            else if (Convert.ToString(Session["requesttype"]) == "Salesman/Agents")
            {
                hdKeyVal.Value = "SA";
            }
            else if (Convert.ToString(Session["requesttype"]) == "OtherEntity")
            {
                hdKeyVal.Value = "OE";
            }
            else if (Convert.ToString(Session["requesttype"]) == "Share Holder")
            {
                hdKeyVal.Value = "SH";
            }
            else if (Convert.ToString(Session["requesttype"]).ToLower().Trim().Contains("lead"))
            {
                hdKeyVal.Value = "Ld";
            }
            else
            {
                hdKeyVal.Value = "OP";
            }

            //Debjyoti 30-12-2016
            //Reason: UDF count
            IsUdfpresent.Value = Convert.ToString(getUdfCount());
            //End Debjyoti 30-12-2016
        }

        protected int getUdfCount()
        {
            DataTable udfCount = oDBEngine.GetDataTable("select 1 from tbl_master_remarksCategory rc where cat_applicablefor='" + hdKeyVal.Value + "'  and ( exists (select * from tbl_master_udfGroup where id=rc.cat_group_id and grp_isVisible=1) or rc.cat_group_id=0)");
            return udfCount.Rows.Count;
        }
        public void ValueAllocation(string[,] ContactData)
        {
            try
            {
                LanGuage();
                TrLang.Visible = true;
                txtClentUcc.Text = ContactData[0, 0];
                ddlIdType.SelectedValue = ContactData[0, 38];
                //if (chkAllow.Checked == true)
                //{
                //    webLogin = "Yes";
                //    Password = txtClentUcc.Text;
                //}
                //else
                //{
                //    webLogin = "No";
                //}


                // Code Added and Commented By Sam on 15112016 to set the nationality dropdown of Customer Client

                //if (ContactData[0, 30] != "1")
                //    ddlnational.SelectedIndex = 1;
                //if (ddlnational.SelectedItem.Value != "1")
                //{

                //if (Request.QueryString["id"] != null)
                //{
                //    countryname = oDBEngine.GetDataTable("select t1.*,cou_country from  (Select isnull(cnt_nationality,'1')as nationality from tbl_master_contact WHERE   cnt_id='" + Request.QueryString["id"] + "') t1 left outer join tbl_master_country on t1.nationality= cou_id");
                //}
                //else
                //{
                //    countryname = oDBEngine.GetDataTable("select t1.*,cou_country from  (Select isnull(cnt_nationality,'1')as nationality from tbl_master_contact WHERE   cnt_id='" + HttpContext.Current.Session["KeyVal"] + "') t1 left outer join tbl_master_country on t1.nationality= cou_id");
                //}
                //ddlnational.SelectedValue =Convert.ToString(countryname.Rows[0]["cnt_nationality"]);
                //    txtcountry_hidden.Text = countryname.Rows[0][0].ToString();
                //    txtcountry.Text = countryname.Rows[0][1].ToString();
                //}
                DataTable countryname = new DataTable();

                //below condition has been added by sanjib due to some time query string was null and throwing the error26122016.
                if (Request.QueryString["id"] != null && Convert.ToString(Request.QueryString["id"]) != string.Empty)
                {
                    countryname = oDBEngine.GetDataTable("Select isnull(cnt_nationality,'1')as nationality from tbl_master_contact WHERE   cnt_id=" + Request.QueryString["id"] + "");

                }
                else
                {
                    countryname = oDBEngine.GetDataTable("Select isnull(cnt_nationality,'1')as nationality from tbl_master_contact WHERE   cnt_id=" + HttpContext.Current.Session["KeyVal"] + "");

                }
                ddlnational.SelectedValue = Convert.ToString(countryname.Rows[0]["nationality"]);

                countryname = null;
                if (Request.QueryString["id"] != null && Convert.ToString(Request.QueryString["id"]) != string.Empty)
                {
                    countryname = oDBEngine.GetDataTable("Select isnull(cnt_contactStatus,'1')as cnt_contactStatus from tbl_master_contact WHERE   cnt_id=" + Request.QueryString["id"] + "");

                }
                else
                {
                    countryname = oDBEngine.GetDataTable("Select isnull(cnt_contactStatus,'1')as cnt_contactStatus from tbl_master_contact WHERE   cnt_id=" + HttpContext.Current.Session["KeyVal"] + "");

                }

                //cmbContactStatusclient.SelectedValue = Convert.ToString(countryname.Rows[0]["cnt_contactStatus"]);

                // Code Above Added and Commented By Sam on 15112016 to set the nationality dropdown of Customer Client

                txtFromDate.Value = Convert.ToDateTime(ContactData[0, 28]);
                txtincorporation.Text = ContactData[0, 27];
                txtotheroccu.Text = ContactData[0, 29];
                if (ContactData[0, 26] == "Yes")
                {
                    //chkAllow.Enabled = false;
                    chkAllow.Checked = true;
                }
                else if (ContactData[0, 26] == "No")
                {
                    chkAllow.Checked = false;
                    //chkAllow.Enabled = true;
                }
                if (ContactData[0, 1] != "")
                {
                    CmbSalutation.SelectedValue = ContactData[0, 1];
                }
                else
                {
                    CmbSalutation.SelectedIndex.Equals(0);
                }

                txtFirstNmae.Text = ContactData[0, 2];
                txtMiddleName.Text = ContactData[0, 3];
                txtLastName.Text = ContactData[0, 4];

                //Subhabrata

                DataTable dt_CustVendHistory = null;
                if (HttpContext.Current.Session["KeyVal"] != null)
                {
                    dt_CustVendHistory = objCRMSalesOrderDtlBL.GetCustVendHistoryId(Convert.ToString(HttpContext.Current.Session["KeyVal"]));
                }
                else
                {
                    dt_CustVendHistory = objCRMSalesOrderDtlBL.GetCustVendHistoryId(Convert.ToString(Request.QueryString["id"]));
                }

                if (dt_CustVendHistory != null && dt_CustVendHistory.Rows.Count > 0)
                {
                    dt_ApplicableFrom.Date = DateTime.ParseExact(Convert.ToString(dt_CustVendHistory.Rows[0]["ApplicableFrom"]), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                }

                //End


                //dt_ApplicableFrom.Date = Convert.ToDateTime(ApplicableFromData[0, 0]);

                if (Convert.ToString(Session["requesttype"]) == "OtherEntity")
                {
                    txtClentUcc.Text = ContactData[0, 5];
                }

                txtAliasName.Text = ContactData[0, 5];
                Session["Name"] = txtFirstNmae.Text + " " + txtMiddleName.Text + " " + txtLastName.Text + " [" + txtClentUcc.Text + "]";
                cmbBloodgroup.SelectedValue = ContactData[0, 25];


                if (ContactData[0, 6] != "")
                {
                    cmbBranch.SelectedValue = ContactData[0, 6];
                }
                else
                {
                    cmbBranch.SelectedIndex.Equals(0);
                }
                if (ContactData[0, 7] != "")
                {
                    cmbGender.SelectedValue = ContactData[0, 7];
                }
                else
                {
                    cmbGender.SelectedIndex.Equals(0);
                }
                if (ContactData[0, 8] != "")
                {
                    cmbMaritalStatus.SelectedValue = ContactData[0, 8];
                }
                else
                {
                    cmbMaritalStatus.SelectedIndex.Equals(0);
                }
                if (ContactData[0, 9] != "")
                {
                    txtDOB.Value = Convert.ToDateTime(ContactData[0, 9]);
                }
                if (ContactData[0, 10] != "")
                {
                    txtAnniversary.Value = Convert.ToDateTime(ContactData[0, 10]);
                }
                if (ContactData[0, 11] != "")
                {
                    cmbLegalStatus.SelectedValue = ContactData[0, 11];

                    if (Convert.ToString(Session["requesttype"]) == "Transporter")
                    {
                        if (ContactData[0, 11] == "54")//Local
                        { pnlVehicleNo.Style.Add("display", "block"); }
                        else { pnlVehicleNo.Style.Add("display", "none"); }
                    }


                }
                else
                {
                    cmbLegalStatus.SelectedIndex.Equals(0);
                }
                if (ContactData[0, 12] != "")
                {
                    cmbEducation.SelectedValue = ContactData[0, 12];
                }
                else
                {
                    cmbEducation.SelectedIndex.Equals(0);
                }
                if (ContactData[0, 13] != "")
                {
                    cmbProfession.SelectedValue = ContactData[0, 13];
                }
                else
                {
                    cmbProfession.SelectedIndex.Equals(0);
                }

                txtOrganization.Text = ContactData[0, 14];
                if (ContactData[0, 15] != "")
                {
                    cmbJobResponsibility.SelectedValue = ContactData[0, 15];
                }
                else
                {
                    cmbJobResponsibility.SelectedIndex.Equals(0);
                }
                if (ContactData[0, 16] != "")
                {
                    cmbDesignation.SelectedValue = ContactData[0, 16];
                }
                else
                {
                    cmbDesignation.SelectedIndex.Equals(0);
                }
                if (ContactData[0, 17] != "")
                {
                    cmbIndustry.SelectedValue = ContactData[0, 17];
                }
                else
                {
                    cmbIndustry.SelectedIndex.Equals(0);
                }
                if (ContactData[0, 18] != "")
                {
                    cmbSource.SelectedValue = ContactData[0, 18];
                }
                else
                {
                    cmbSource.SelectedIndex.Equals(0);
                }
                string ReferedBy = ContactData[0, 19];
                //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                if (ReferedBy != "")
                {
                    if (cmbSource.SelectedItem.Value != "18")
                    {
                        //txtReferedBy1.Visible = false;
                        //txtReferedBy.Visible = true;
                        string[,] RID = oDBEngine.GetFieldValue("tbl_master_contact con,tbl_master_contact con1,tbl_master_branch b", "con.cnt_internalId as id,ltrim(rtrim(ISNULL(con.cnt_firstName, '') ))+ ' ' + ltrim(rtrim(ISNULL(con.cnt_middleName, ''))) + ' ' + ltrim(rtrim(ISNULL(con.cnt_lastName, ''))) + '['+ltrim(rtrim(isnull(ISNULL(con.cnt_ucc,con.cnt_shortname),'')))+']' + '[' + ltrim(rtrim(isnull(b.branch_description,''))) + ']' as name ", " con1.cnt_internalId='" + Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]) + "' and  con.cnt_internalId=con1.cnt_referedBy and con.cnt_branchid=b.branch_id", 2);
                        if (RID[0, 0] != "n")
                        {
                            //txtReferedBy1.Visible == false;
                            txtReferedBy.Text = RID[0, 1];
                            txtReferedBy_hidden.Text = RID[0, 0];
                        }
                    }
                    else
                    {
                        //txtReferedBy.Visible = false;
                        //txtReferedBy1.Visible = true;
                        string[,] RID = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_internalId,cnt_referedBy", "cnt_internalId='" + Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]) + "' and cnt_referedBy is not null", 2);
                        if (RID[0, 0] != "n")
                        {
                            txtReferedBy.Text = RID[0, 1];
                            //txtReferedBy_hidden1.Text = RID[0, 0];
                            //txtReferedBy.Visible == false;
                        }
                    }
                }
                TxtContactStatus.Text = ContactData[0, 24];
                if (ContactData[0, 20] != "")
                {
                    cmbContactStatus.SelectedValue = ContactData[0, 21];

                    //cmbContactStatusclient.SelectedValue = ContactData[0, 21];
                }
                else
                {
                    cmbContactStatus.SelectedIndex.Equals(0);

                    cmbContactStatusclient.SelectedIndex.Equals(0);
                }
                if (ContactData[0, 22] != "")
                    txtDateRegis.Value = Convert.ToDateTime(ContactData[0, 22]);
                if (ContactData[0, 23] != "")
                {
                    cmbRating.SelectedValue = ContactData[0, 23];
                }
                else
                {
                    cmbRating.SelectedIndex.Equals(0);
                }
                //.................... Code  Added and commented By Priti on15122016 to add 3 fields Creditcard,creditDays,CreditLimit
                ChkCreditcard.Value = ContactData[0, 31];
                if (ContactData[0, 31] == "True")
                {
                    ChkCreditcard.Checked = true;
                }
                else
                {
                    ChkCreditcard.Checked = false;
                }
                txtcreditDays.Value = ContactData[0, 32];
                txtCreditLimit.Text = ContactData[0, 33];
                cmbContactStatusclient.Value = ContactData[0, 34];

                //Debjyoti For GSTIN 060217
                string GSTIN = "";
                if (ContactData[0, 35] != "")
                {

                    GSTIN = ContactData[0, 35];
                    txtGSTIN1.Text = GSTIN.Substring(0, 2);
                    txtGSTIN2.Text = GSTIN.Substring(2, 10);



                    txtGSTIN3.Text = GSTIN.Substring(12, 3);


                    hddnGSTIN2Val.Value = Convert.ToString(txtGSTIN1.Text) + Convert.ToString(txtGSTIN2.Text) + Convert.ToString(txtGSTIN3.Text);

                    if (Convert.ToString(Session["requesttype"]) == "Transporter")
                    {
                        radioregistercheck.SelectedValue = "1";
                    }
                    if (Convert.ToString(Session["requesttype"]) == "Customer/Client")
                    {
                        radioregistercheck.SelectedValue = "1";
                    }
                }
                else
                {
                    if (Convert.ToString(Session["requesttype"]) == "Transporter")
                    {
                        radioregistercheck.SelectedValue = "0";
                    }
                    if (Convert.ToString(Session["requesttype"]) == "Customer/Client")
                    {
                        radioregistercheck.SelectedValue = "0";
                    }
                }

                hidAssociatedEmp.Value = ContactData[0, 36];

                hndTaxRates_MainAccount_hidden.Value = ContactData[0, 37];
                txt_EnrollmentId.Text = ContactData[0, 39];
                txtPname.Text = ContactData[0, 40];
                string[] getData = oDBEngine.GetFieldValue1("Trans_AccountsLedger", "COUNT(*)", "AccountsLedger_MainAccountID='" + ContactData[0, 37] + "' and  AccountsLedger_MainAccountID<>''", 1);
                if (getData[0] == "0")
                    hdIsMainAccountInUse.Value = "notInUse";
                else
                    hdIsMainAccountInUse.Value = "IsInUse";
                //............end.....................
                string[,] RelPartner = oDBEngine.GetFieldValue("tbl_master_contact contact,tbl_trans_contactInfo info", "(contact.cnt_firstName +' '+contact.cnt_middleName+' '+contact.cnt_lastName +'['+contact.cnt_shortName+']') as Name,info.Rep_partnerid", " info.Rep_partnerid=contact.cnt_internalid and info.cnt_internalid='" + Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]) + "' and info.ToDate is null", 2);
                if (RelPartner[0, 0] != "n")
                {
                    txtRPartner.Text = RelPartner[0, 0];
                    txtRPartner_hidden.Text = RelPartner[0, 1];
                    ViewState["PartnerId"] = RelPartner[0, 1];
                }
                string[,] Email = oDBEngine.GetFieldValue("tbl_master_email", "top 1 eml_email", " eml_cntId='" + txtRPartner_hidden.Text + "'  and eml_email<>''", 1);
                if (Email[0, 0] != "n")
                {
                    TxtEmail.Text = Email[0, 0];
                }
                string[,] Phone = oDBEngine.GetFieldValue("tbl_master_phonefax", "top 1 phf_phoneNumber", " phf_cntId='" + txtRPartner_hidden.Text + "' and phf_phonenumber<>''", 1);
                if (Phone[0, 0] != "n")
                {
                    TxtPhone.Text = Phone[0, 0];
                }

            }
            catch
            {
            }
        }

        public void DDLBind()
        {
            string[,] Data = oDBEngine.GetFieldValue("tbl_master_salutation", "sal_id, sal_name", null, 2, "sal_name");
            //oDBEngine.AddDataToDropDownList(Data, CmbSalutation);
            oclsDropDownList.AddDataToDropDownList(Data, CmbSalutation);
            Data = oDBEngine.GetFieldValue("tbl_master_education", "edu_id, edu_education", null, 2, "edu_education");
            //oDBEngine.AddDataToDropDownList(Data, cmbEducation);
            oclsDropDownList.AddDataToDropDownList(Data, cmbEducation);
            Data = oDBEngine.GetFieldValue("tbl_master_profession", "pro_id, pro_professionName", null, 2, "pro_professionName");
            //oDBEngine.AddDataToDropDownList(Data, cmbProfession);
            oclsDropDownList.AddDataToDropDownList(Data, cmbProfession);
            Data = oDBEngine.GetFieldValue("tbl_master_jobresponsibility", "job_id, job_responsibility", null, 2, "job_responsibility");
            //oDBEngine.AddDataToDropDownList(Data, cmbJobResponsibility);
            oclsDropDownList.AddDataToDropDownList(Data, cmbJobResponsibility);
            Data = oDBEngine.GetFieldValue("tbl_master_Designation", "deg_id, deg_designation", null, 2, "deg_designation");
            //oDBEngine.AddDataToDropDownList(Data, cmbDesignation);
            oclsDropDownList.AddDataToDropDownList(Data, cmbDesignation);
            Data = oDBEngine.GetFieldValue("tbl_master_branch", "branch_id,(rtrim(ltrim(branch_description))+' ['+rtrim(ltrim(branch_code))+']') as branch_description", null, 2, "branch_description");
            //oDBEngine.AddDataToDropDownList(Data, cmbBranch);
            oclsDropDownList.AddDataToDropDownList(Data, cmbBranch);

            Data = oDBEngine.GetFieldValue("tbl_master_industry", "ind_id, ind_industry  ", null, 2, "ind_industry");
            //oDBEngine.AddDataToDropDownList(Data, cmbIndustry);
            oclsDropDownList.AddDataToDropDownList(Data, cmbIndustry);
            Data = oDBEngine.GetFieldValue(" tbl_master_ContactSource", "cntsrc_id, cntsrc_sourcetype", null, 2, "cntsrc_sourcetype");
            //oDBEngine.AddDataToDropDownList(Data, cmbSource);
            oclsDropDownList.AddDataToDropDownList(Data, cmbSource);
            Data = oDBEngine.GetFieldValue("tbl_master_leadRating", " rat_id, rat_LeadRating  ", null, 2, "rat_LeadRating");
            //oDBEngine.AddDataToDropDownList(Data, cmbRating);
            oclsDropDownList.AddDataToDropDownList(Data, cmbRating);
            Data = oDBEngine.GetFieldValue(" tbl_master_maritalstatus", " mts_id, mts_maritalStatus", null, 2, "mts_maritalStatus");
            //oDBEngine.AddDataToDropDownList(Data, cmbMaritalStatus);
            oclsDropDownList.AddDataToDropDownList(Data, cmbMaritalStatus);
            Data = oDBEngine.GetFieldValue(" tbl_master_contactstatus", "cntstu_id, cntstu_contactStatus", null, 2, "cntstu_contactStatus");
            //oDBEngine.AddDataToDropDownList(Data, cmbContactStatus);
            oclsDropDownList.AddDataToDropDownList(Data, cmbContactStatus);

            // oclsDropDownList.AddDataToDropDownList(Data, cmbContactStatusclient);

            if (Convert.ToString(Session["requesttype"]) == "Transporter")
            {
                Data = oDBEngine.GetFieldValue("tbl_master_legalstatus", "lgl_id, lgl_legalStatus", "lgl_legalStatus in ('General','Local')", 2, "lgl_legalStatus");
            }
            else
            {
                Data = oDBEngine.GetFieldValue("tbl_master_legalstatus", "lgl_id, lgl_legalStatus", null, 2, "lgl_legalStatus");
            }



            //oDBEngine.AddDataToDropDownList(Data, cmbLegalStatus);
            oclsDropDownList.AddDataToDropDownList(Data, cmbLegalStatus);

            //................. Code Added  By Sam on 15112016 to fill natinaolity drop down..........................

            Data = oDBEngine.GetFieldValue("Master_Nationality", "Nationality_id ,Nationality_Description", null, 2, "Nationality_Description");
            oclsDropDownList.AddDataToDropDownList(Data, ddlnational);

            //................ Code Above Added and Commented By Sam on 15112016 to fill natinaolity drop down.........
            CmbSalutation.SelectedValue = "1";
            cmbRating.SelectedValue = "1";
            cmbLegalStatus.SelectedValue = "1";
            cmbContactStatus.SelectedValue = "1";

            //cmbContactStatusclient.SelectedValue = "1";

            cmbEducation.Items.Insert(0, new ListItem("--Select--", "0"));
            cmbProfession.Items.Insert(0, new ListItem("--Select--", "0"));
            cmbJobResponsibility.Items.Insert(0, new ListItem("--Select--", "0"));
            cmbDesignation.Items.Insert(0, new ListItem("--Select--", "0"));
            //cmbLegalStatus.Items.Insert(0, new ListItem("--Select--", "0"));
            //cmbContactStatus.Items.Insert(0, new ListItem("--Select--", "0"));
            cmbIndustry.Items.Insert(0, new ListItem("--Select--", "0"));
            cmbSource.Items.Insert(0, new ListItem("--Select--", "0"));
            cmbMaritalStatus.Items.Insert(0, new ListItem("--Select--", "0"));

            //------select branch
            // .............................Code Commented and Added by Sam on 09122016 to make selected branch name default instead of inserting already existing branch. ..................................... 
            int branchindex = 0;
            if (cmbBranch.Items.Count > 0)
            {
                if (Session["userbranchID"] != null)
                {
                    foreach (ListItem li in cmbBranch.Items)
                    {
                        if (li.Value == Convert.ToString(Session["userbranchID"]))
                        {
                            break;
                        }
                        else
                        {
                            branchindex = branchindex + 1;
                        }
                    }
                }
                cmbBranch.SelectedIndex = branchindex;
            }

            //string branchid = HttpContext.Current.Session["userbranchID"].ToString();
            //DataTable dtname = oDBEngine.GetDataTable(" tbl_master_branch", "  branch_description", " branch_id= '" + branchid + "'");
            //string branchName = dtname.Rows[0]["branch_description"].ToString();
            //cmbBranch.Items.Insert(0, new ListItem(branchName, branchid));
            // .............................Code Above Commented and Added by Sam on 13122016...................................... 


            // -------------------
            //  cmbMaritalStatus.SelectedValue = "6";
            Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>PageLoad();</script>");
        }

        public void DisabledTabPage()
        {
            TabPage page = ASPxPageControl1.TabPages.FindByName("Correspondence");
            page.Visible = false;
            page = ASPxPageControl1.TabPages.FindByName("BankDetails");
            page.Visible = false;
            page = ASPxPageControl1.TabPages.FindByName("DPDetails");
            page.Visible = false;
            page = ASPxPageControl1.TabPages.FindByName("Documents");
            page.Visible = false;
            page = ASPxPageControl1.TabPages.FindByName("FamilyMembers");
            page.Visible = false;
            page = ASPxPageControl1.TabPages.FindByName("Registration");
            page.Visible = false;
            page = ASPxPageControl1.TabPages.FindByName("GroupMember");
            page.Visible = false;
            page = ASPxPageControl1.TabPages.FindByName("Deposit");
            page.Visible = false;
            page = ASPxPageControl1.TabPages.FindByName("Remarks");
            page.Visible = false;
            page = ASPxPageControl1.TabPages.FindByName("Education");
            page.Visible = false;
            page = ASPxPageControl1.TabPages.FindByName("Trad. Prof.");
            page.Visible = false;
            page = ASPxPageControl1.TabPages.FindByName("Other");
            page.Visible = false;
            page = ASPxPageControl1.TabPages.FindByName("Subscription");
            page.Visible = false;

            //btnUdf.Visible = false;
        }



        public string retContactType(string requesttype)
        {
            string ContType = "";
            switch (requesttype)
            {
                case "Customer/Client":
                    ContType = "CL";
                    break;
                case "OtherEntity":
                    ContType = "XC";
                    break;
                case "Sub Broker":
                    ContType = "SB";
                    break;
                case "Franchisee":
                    ContType = "FR";
                    break;
                case "Relationship Partners":
                    ContType = "RA";
                    break;
                case "Broker":
                    ContType = "BO";
                    break;
                case "Relationship Manager":
                    ContType = "RC";
                    break;
                case "Data Vendor":
                    ContType = "DV";
                    break;
                case "Vendor":
                    ContType = "VR";
                    break;
                case "Partner":
                    ContType = "PR";
                    break;
                case "Consultant":
                    ContType = "CS";
                    break;
                case "Share Holder":
                    ContType = "SH";
                    break;
                case "Creditors":
                    ContType = "CR";
                    break;
                case "Debtor":
                    ContType = "DR";
                    break;
                case "Lead":
                    ContType = "LD";
                    break;
                case "Transporter":
                    ContType = "TR";
                    break;
                case "Salesman/Agents":
                    ContType = "AG";
                    break;
            }
            return ContType;
        }

        public bool ISUniqueGSTINByUserType(string ContType)
        {
            string customerid = "";
            string gstin = "";
            Boolean result = false;
            gstin = txtGSTIN1.Text.Trim() + txtGSTIN2.Text.Trim() + txtGSTIN3.Text.Trim();
            if (Convert.ToString(Request.QueryString["id"]) == "ADD")
            {
                result = ContactGeneralBL.ISUniqueGSTIN(customerid, "0", gstin, ContType);
            }
            else
            {
                if (Request.QueryString.AllKeys.Contains("id"))
                {
                    customerid = Convert.ToString(Request.QueryString["id"]);
                }
                else
                {
                    customerid = Convert.ToString(HttpContext.Current.Session["KeyVal"]);
                }

                result = ContactGeneralBL.ISUniqueGSTIN(customerid, "1", gstin, ContType);
            }
            return result;
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            string ContType = "";
            string gstin = "";
            Boolean result = false;
            if (txtGSTIN1.Text.Trim() != "" && txtGSTIN2.Text.Trim() != "" && txtGSTIN3.Text.Trim() != "")
            {
                ContType = retContactType(Convert.ToString(Session["requesttype"]));
                result = ISUniqueGSTINByUserType(ContType);
                if (result)
                {
                    //txtGSTIN1.Text = "";
                    //txtGSTIN2.Text = "";
                    //txtGSTIN3.Text = "";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "jAlert('Duplicate GSTIN number.');", true);
                    return;
                }
            }

            Boolean Creditcard;
            if (ChkCreditcard.Checked)
            {
                Creditcard = true;
            }
            else
            {
                Creditcard = false;
            }
            if (Convert.ToString(Session["requesttype"]) != "Lead" || Convert.ToString(Session["requesttype"]) != "Opportunity")
            {
                string TransporterEnrollmentId = "";
                string country = ddlnational.SelectedValue;

                string status = Convert.ToString(cmbContactStatusclient.Value);

                string webLogin = "";
                string Password = "";

                if (Session["requesttype"] != null)
                {
                    ContType = retContactType(Convert.ToString(Session["requesttype"]));
                }


                if (ContType == "XC")
                {
                    DisabledTabPage();
                    TabPage page = ASPxPageControl1.TabPages.FindByName("Correspondence");
                    page.Visible = true;
                    txtAliasName.Text = txtClentUcc.Text;

                }
                if (ContType != "XC")
                {
                    if (Request.QueryString["formtype"] != null)
                    {

                    }

                    else
                    {
                        DataTable dt1 = oDBEngine.GetDataTable("tbl_master_contactExchange,tbl_master_contact", "top 1 cnt_ucc,crg_tcode", "cnt_ucc='" + txtClentUcc.Text.Trim() + "' or crg_tcode='" + txtClentUcc.Text.Trim() + "'  and cnt_id='" + Convert.ToString(HttpContext.Current.Session["KeyVal"]) + "'");
                        DataTable dt2 = oDBEngine.GetDataTable("tbl_master_contact", " cnt_id", "cnt_id='" + Convert.ToString(HttpContext.Current.Session["KeyVal"]) + "'");

                        //############### Updated By Samrat Roy --- changes done due to edit is not working --- 24/04/2017
                        string[,] abcd = oDBEngine.GetFieldValue("tbl_master_contactExchange,tbl_master_contact", "top 1 cnt_ucc,crg_tcode", "(cnt_ucc='" + Convert.ToString(txtClentUcc.Text.Trim()) + "' or crg_tcode='" + Convert.ToString(txtClentUcc.Text) + "')  and cnt_id='" + Convert.ToString(HttpContext.Current.Session["KeyVal"]) + "' and cnt_contactType='" + ContType + "'", 2);
                        string[,] bName = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_ucc,cnt_id", "cnt_id='" + Convert.ToString(HttpContext.Current.Session["KeyVal"]) + "' and cnt_contactType='" + ContType + "'", 2);

                        if (Convert.ToString(HttpContext.Current.Session["KeyVal"]) != "0")        //________For Update
                        {

                            if (Convert.ToString(Session["requesttype"]) == "Customer/Client")
                            {
                                DataTable dtPan = oDBEngine.GetDataTable("tbl_master_contactRegistration", "crg_cntId", "crg_cntId='" + Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]) + "' and crg_type='Pan Card' and crg_Number is not null");
                                if (dtPan.Rows.Count <= 0)
                                {
                                    if (cmbContactStatusclient.Value == "A")
                                    {

                                        DataTable DT = oDBEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='PAN_Required' AND IsActive=1");
                                        if (DT != null && DT.Rows.Count > 0)
                                        {
                                            if (Convert.ToString(DT.Rows[0]["Variable_Value"]).Trim().ToUpper() == "YES")
                                            {
                                                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('PAN Number is requied to save the customer details.Go to Registration Tab and update PAN details. ')</script>");
                                                Page.ClientScript.RegisterStartupScript(GetType(), "JScript3", "<script >hideotherstatus();</script>");
                                                return;
                                            }
                                        }

                                    }
                                }

                            }


                            SubAcName = txtFirstNmae.Text.Trim() + " " + txtMiddleName.Text.Trim() + " " + txtLastName.Text.Trim();
                            Int32 RowEffect = oDBEngine.SetFieldValue("master_subaccount", " SubAccount_Name='" + SubAcName + "'", " SubAccount_Code='" + Convert.ToString(Session["KeyVal_InternalID"]) + "'");

                            string today = Convert.ToString(oDBEngine.GetDate());
                            if (chkAllow.Checked == true)
                            {
                                webLogin = "Yes";
                                Password = txtClentUcc.Text;
                            }
                            else
                            {
                                webLogin = "No";
                            }
                            if (cmbSource.SelectedItem.Value == "0")
                            {
                                txtReferedBy_hidden.Text = null;
                            }
                            if (txtReferedBy.Text == "")
                            {
                                txtReferedBy_hidden.Text = null;
                            }
                            if (Convert.ToString(Session["requesttype"]) == "Transporter")
                            {
                                TransporterEnrollmentId = txt_EnrollmentId.Text.Trim();
                            }
                            if (cmbSource.SelectedItem.Value == "18")
                            {
                                String value = "";
                                string other = "";
                                if (cmbProfession.SelectedValue == "20")
                                    other = txtotheroccu.Text.Trim();
                                else
                                    other = "";
                                if (txtincorporation.Text == "")
                                {
                                    value = "Statustype='" + status + "',cnt_ucc='" + txtClentUcc.Text + "', cnt_salutation=" + CmbSalutation.SelectedItem.Value + ",  cnt_firstName='" + txtFirstNmae.Text + "', cnt_middleName='" + txtMiddleName.Text + "', cnt_lastName='" + txtLastName.Text + "', cnt_shortName='" + txtAliasName.Text + "', cnt_branchId=" + cmbBranch.SelectedItem.Value + ", cnt_sex=" + cmbGender.SelectedItem.Value + ", cnt_maritalStatus=" + cmbMaritalStatus.SelectedItem.Value + ", cnt_DOB='" + txtDOB.Value + "', cnt_anniversaryDate='" + txtAnniversary.Value + "', cnt_legalStatus=" + cmbLegalStatus.SelectedItem.Value + ", cnt_education=" + cmbEducation.SelectedItem.Value + ", cnt_profession=" + cmbProfession.SelectedItem.Value + ", cnt_organization='" + txtOrganization.Text + "', cnt_jobResponsibility=" + cmbJobResponsibility.SelectedItem.Value + ", cnt_designation=" + cmbDesignation.SelectedItem.Value + ", cnt_industry=" + cmbIndustry.SelectedItem.Value + ", cnt_contactSource=" + cmbSource.SelectedItem.Value + ", cnt_referedBy='" + txtReferedBy.Text + "', cnt_contactType='" + ContType + "', cnt_contactStatus=" + cmbContactStatus.SelectedItem.Value + ",cnt_RegistrationDate='" + txtDateRegis.Value + "',cnt_rating='" + cmbRating.SelectedItem.Value + "',cnt_reason='" + TxtContactStatus.Text + "',cnt_bloodgroup='" + cmbBloodgroup.SelectedItem.Value + "',WebLogIn='" + webLogin + "',PassWord='" + Password + "', lastModifyDate ='" + today + "',cnt_nationality='" + Convert.ToInt32(Convert.ToString(country)) + "',cnt_PlaceOfIncorporation='',cnt_BusinessComncDate='',cnt_OtherOccupation='" + other + "', lastModifyUser=" + HttpContext.Current.Session["userid"] + "',  Enrolment_ID='" + TransporterEnrollmentId + "',cnt_PrintNameToCheque='" + txtPname.Text + "'"; // + Session["userid"]
                                }
                                else
                                {
                                    value = "Statustype='" + status + "',cnt_ucc='" + txtClentUcc.Text + "', cnt_salutation=" + CmbSalutation.SelectedItem.Value + ",  cnt_firstName='" + txtFirstNmae.Text + "', cnt_middleName='" + txtMiddleName.Text + "', cnt_lastName='" + txtLastName.Text + "', cnt_shortName='" + txtAliasName.Text + "', cnt_branchId=" + cmbBranch.SelectedItem.Value + ", cnt_sex=" + cmbGender.SelectedItem.Value + ", cnt_maritalStatus=" + cmbMaritalStatus.SelectedItem.Value + ", cnt_DOB='" + txtDOB.Value + "', cnt_anniversaryDate='" + txtAnniversary.Value + "', cnt_legalStatus=" + cmbLegalStatus.SelectedItem.Value + ", cnt_education=" + cmbEducation.SelectedItem.Value + ", cnt_profession=" + cmbProfession.SelectedItem.Value + ", cnt_organization='" + txtOrganization.Text + "', cnt_jobResponsibility=" + cmbJobResponsibility.SelectedItem.Value + ", cnt_designation=" + cmbDesignation.SelectedItem.Value + ", cnt_industry=" + cmbIndustry.SelectedItem.Value + ", cnt_contactSource=" + cmbSource.SelectedItem.Value + ", cnt_referedBy='" + txtReferedBy.Text + "', cnt_contactType='" + ContType + "', cnt_contactStatus=" + cmbContactStatus.SelectedItem.Value + ",cnt_RegistrationDate='" + txtDateRegis.Value + "',cnt_rating='" + cmbRating.SelectedItem.Value + "',cnt_reason='" + TxtContactStatus.Text + "',cnt_bloodgroup='" + cmbBloodgroup.SelectedItem.Value + "',WebLogIn='" + webLogin + "',PassWord='" + Password + "', lastModifyDate ='" + today + "',cnt_nationality='" + Convert.ToInt32(Convert.ToString(country)) + "',cnt_PlaceOfIncorporation='" + txtincorporation.Text.Trim() + "',cnt_BusinessComncDate='" + txtFromDate.Value + "',cnt_OtherOccupation='" + other + "',lastModifyUser=" + HttpContext.Current.Session["userid"] + "',  Enrolment_ID='" + TransporterEnrollmentId + "',cnt_PrintNameToCheque='" + txtPname.Text + "'"; // + Session["userid"]
                                }
                                oDBEngine.SetFieldValue("tbl_master_contact", value, " cnt_id=" + HttpContext.Current.Session["KeyVal"]);
                                //Rev Subhra 03-06-2019
                                if (Convert.ToString(Session["requesttype"]) == "Lead")
                                {
                                    oDBEngine.SetFieldValue("tbl_master_contact", "EnteredDate='" + dt_EnteredOn.Value + "' ", " cnt_id=" + HttpContext.Current.Session["KeyVal"]);
                                }
                                //End of Rev 03-06-2019


                            }
                            else
                            {
                                if (dt1.Rows.Count > 0)
                                {
                                    if (bName[0, 0] != "n")
                                    {
                                        if (bName[0, 0] == abcd[0, 0])
                                        {
                                            String value = "";
                                            string other = "";
                                            if (cmbProfession.SelectedValue == "20")
                                                other = txtotheroccu.Text.Trim();
                                            else
                                                other = "";

                                            if (!string.IsNullOrEmpty(txtincorporation.Text))
                                            {
                                                if (txtcreditDays.Text == string.Empty)
                                                {
                                                    txtcreditDays.Text = "0";
                                                }

                                                value = "Statustype='" + status + "',cnt_ucc='" + txtClentUcc.Text + "', cnt_salutation=" + CmbSalutation.SelectedItem.Value + ",  cnt_firstName='" + txtFirstNmae.Text + "', cnt_middleName='" + txtMiddleName.Text + "', cnt_lastName='" + txtLastName.Text + "', cnt_shortName='" + txtAliasName.Text + "', cnt_branchId=" + cmbBranch.SelectedItem.Value + ", cnt_sex=" + cmbGender.SelectedItem.Value + ", cnt_maritalStatus=" + cmbMaritalStatus.SelectedItem.Value + ", cnt_DOB='" + txtDOB.Value + "', cnt_anniversaryDate='" + txtAnniversary.Value + "', cnt_legalStatus=" + cmbLegalStatus.SelectedItem.Value + ", cnt_education=" + cmbEducation.SelectedItem.Value + ", cnt_profession=" + cmbProfession.SelectedItem.Value + ", cnt_organization='" + txtOrganization.Text + "', cnt_jobResponsibility=" + cmbJobResponsibility.SelectedItem.Value + ", cnt_designation=" + cmbDesignation.SelectedItem.Value + ", cnt_industry=" + cmbIndustry.SelectedItem.Value + ", cnt_contactSource=" + cmbSource.SelectedItem.Value + ", cnt_referedBy='" + txtReferedBy_hidden.Text + "', cnt_contactType='" + ContType + "', cnt_contactStatus=" + cmbContactStatus.SelectedItem.Value + ",cnt_RegistrationDate='" + txtDateRegis.Value + "',cnt_rating='" + cmbRating.SelectedItem.Value + "',cnt_reason='" + TxtContactStatus.Text + "',cnt_bloodgroup='" + cmbBloodgroup.SelectedItem.Value + "',WebLogIn='" + webLogin + "',PassWord='" + Password + "', lastModifyDate ='" + today + "',cnt_PlaceOfIncorporation='',cnt_BusinessComncDate='',cnt_OtherOccupation='" + other + "',cnt_nationality='" + Convert.ToInt32(Convert.ToString(country)) + "',cnt_IsCreditHold='" + Creditcard + "',cnt_CreditDays='" + Convert.ToInt32(txtcreditDays.Text) + "' ,cnt_CreditLimit='" + Convert.ToDecimal(txtCreditLimit.Text) + "', lastModifyUser=" + HttpContext.Current.Session["userid"] + "',  Enrolment_ID='" + TransporterEnrollmentId + "',cnt_PrintNameToCheque='" + txtPname.Text + "'"; // + Session["userid"]

                                            }
                                            else
                                            {
                                                if (txtcreditDays.Text == string.Empty)
                                                {
                                                    txtcreditDays.Text = "0";
                                                }

                                                //Debjyoti GSTIN 060217
                                                string GSTIN = "";
                                                if (radioregistercheck.SelectedValue != "0")
                                                {
                                                    GSTIN = txtGSTIN1.Text.Trim() + txtGSTIN2.Text.Trim() + txtGSTIN3.Text.Trim();
                                                }



                                                value = "Statustype='" + status + "',cnt_ucc='" + txtClentUcc.Text + "', cnt_salutation=" + CmbSalutation.SelectedItem.Value + ",  cnt_firstName='" + txtFirstNmae.Text + "', cnt_middleName='" + txtMiddleName.Text + "', cnt_lastName='" + txtLastName.Text + "', cnt_shortName='" + txtAliasName.Text + "', cnt_branchId=" + cmbBranch.SelectedItem.Value + ", cnt_sex=" + cmbGender.SelectedItem.Value + ", cnt_maritalStatus=" + cmbMaritalStatus.SelectedItem.Value + ", cnt_DOB='" + txtDOB.Value + "', cnt_anniversaryDate='" + txtAnniversary.Value + "', cnt_legalStatus=" + cmbLegalStatus.SelectedItem.Value + ", cnt_education=" + cmbEducation.SelectedItem.Value + ", cnt_profession=" + cmbProfession.SelectedItem.Value + ", cnt_organization='" + txtOrganization.Text + "', cnt_jobResponsibility=" + cmbJobResponsibility.SelectedItem.Value + ", cnt_designation=" + cmbDesignation.SelectedItem.Value + ", cnt_industry=" + cmbIndustry.SelectedItem.Value + ", cnt_contactSource=" + cmbSource.SelectedItem.Value + ", cnt_referedBy='" + txtReferedBy_hidden.Text + "', cnt_contactType='" + ContType + "', cnt_contactStatus=" + cmbContactStatus.SelectedItem.Value + ",cnt_RegistrationDate='" + txtDateRegis.Value + "',cnt_rating='" + cmbRating.SelectedItem.Value + "',cnt_reason='" + TxtContactStatus.Text + "',cnt_bloodgroup='" + cmbBloodgroup.SelectedItem.Value + "',WebLogIn='" + webLogin + "',PassWord='" + Password + "', lastModifyDate ='" + today + "',cnt_PlaceOfIncorporation='" + txtincorporation.Text.ToString().Trim() + "',cnt_nationality='" + Convert.ToInt32(Convert.ToString(country)) + "',cnt_BusinessComncDate='" + txtFromDate.Value + "',cnt_OtherOccupation='" + other + "',cnt_IsCreditHold='" + Creditcard + "',cnt_CreditDays='" + Convert.ToInt32(txtcreditDays.Text) + "' ,cnt_CreditLimit='" + Convert.ToDecimal(txtCreditLimit.Text) + "', lastModifyUser=" + HttpContext.Current.Session["userid"] + ",CNT_GSTIN='" + GSTIN + "',cnt_AssociatedEmp= '" + hidAssociatedEmp.Value + "',cnt_mainAccount='" + hndTaxRates_MainAccount_hidden.Value + "',  Enrolment_ID='" + TransporterEnrollmentId + "',cnt_PrintNameToCheque='" + txtPname.Text + "'"; // + Session["userid"]
                                            }


                                            #region Subhabrata

                                            bool IsSaved = false;
                                            bool flagEntity = false;
                                            Employee_BL ebl = new Employee_BL();
                                            string User_Id = Convert.ToString(Session["userid"]);
                                            if (Convert.ToString(hddnGSTINFlag.Value).ToUpper() == "UPDATE")
                                            {


                                                IsSaved = ebl.AddCustVendHistory(Convert.ToString(gstin), Convert.ToInt32(HttpContext.Current.Session["KeyVal"]),
                                                    Convert.ToDateTime(dt_ApplicableFrom.Value), User_Id, "GSTIN_Customer");
                                                flagEntity = true;
                                            }
                                            else if (Convert.ToString(hddnGSTINFlag.Value).ToUpper() == "NOTUPDATE")
                                            {
                                                IsSaved = ebl.AddCustVendHistory(Convert.ToString(gstin), Convert.ToInt32(HttpContext.Current.Session["KeyVal"]),
                                                   Convert.ToDateTime(dt_ApplicableFrom.Value), User_Id, "GSTIN_UpdateCust");
                                            }




                                            oDBEngine.SetFieldValue("tbl_master_contact", value, " cnt_id=" + HttpContext.Current.Session["KeyVal"]);


                                            #endregion

                                            //Rev Subhra 03-06-2019
                                            if (Convert.ToString(Session["requesttype"]) == "Lead")
                                            {
                                                oDBEngine.SetFieldValue("tbl_master_contact", "EnteredDate='" + dt_EnteredOn.Value + "' ", " cnt_id=" + HttpContext.Current.Session["KeyVal"]);
                                            }
                                            //End of Rev 03-06-2019



                                            //--------------------
                                            if (Convert.ToString(Session["requesttype"]) == "Transporter")
                                            {
                                                string vehicleNos = VehicleNo_hidden.Value;

                                                if (Convert.ToString(cmbLegalStatus.SelectedValue) == "54")//Local
                                                {
                                                    //Contact ct = new Contact();
                                                    //ct.Delete_TransporterVehicles(Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]));
                                                    //VehicleNo_hidden.Value = "";

                                                    if (!string.IsNullOrEmpty(vehicleNos))
                                                    {
                                                        Contact ct = new Contact();
                                                        ct.Insert_TransporterVehicles(Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]), vehicleNos);
                                                    }
                                                }
                                                else
                                                {
                                                    //if (!string.IsNullOrEmpty(vehicleNos))
                                                    //{
                                                    //    Contact ct = new Contact();
                                                    //    ct.Insert_TransporterVehicles(Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]), vehicleNos);
                                                    //}

                                                    Contact ct = new Contact();
                                                    ct.Delete_TransporterVehicles(Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]));
                                                    VehicleNo_hidden.Value = "";
                                                }
                                            }
                                            //---------------------

                                            if (txtRPartner.Text != "")
                                            {
                                                string[,] count = oDBEngine.GetFieldValue("tbl_trans_contactInfo", "cnt_internalid", "cnt_internalId='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'", 1);
                                                if (count[0, 0] != "n")
                                                {
                                                    string valueforspoc = "cnt_internalId='" + HttpContext.Current.Session["KeyVal_InternalID"] + "',Rep_partnerId='" + txtRPartner_hidden.Text.Trim() + "',FromDate='" + Convert.ToString(oDBEngine.GetDate()) + "',branchId='" + cmbBranch.SelectedItem.Value + "',CreateDate='" + Convert.ToString(oDBEngine.GetDate()) + "',CreateUser=" + HttpContext.Current.Session["userid"];
                                                    oDBEngine.SetFieldValue("tbl_trans_contactInfo", valueforspoc, " cnt_internalId='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'");
                                                }
                                                else
                                                {
                                                    oDBEngine.InsurtFieldValue("tbl_trans_contactInfo", "cnt_internalid,Rep_partnerid,FromDate,branchid,CreateDate,CreateUser", "'" + Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]) + "','" + txtRPartner_hidden.Text + "','" + Convert.ToString(oDBEngine.GetDate()) + "','" + cmbBranch.SelectedItem.Value + "','" + Convert.ToString(oDBEngine.GetDate()) + "','" + Convert.ToString(Session["userid"]) + "'");
                                                }
                                            }
                                            else
                                            {
                                                oDBEngine.DeleteValue("tbl_trans_contactInfo", "cnt_internalId='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'");
                                            }


                                            if (Convert.ToString(Session["requesttype"]) == "Transporter")
                                            {
                                                TransporterEnrollmentId = txt_EnrollmentId.Text.Trim();
                                                if (Convert.ToString(cmbLegalStatus.SelectedValue) == "54")//Local
                                                {
                                                    pnlVehicleNo.Style.Add("display", "block");
                                                }
                                                else
                                                {
                                                    pnlVehicleNo.Style.Add("display", "none");
                                                }
                                                td_EnrollmentId.Style.Add("display", "block");
                                            }
                                            else
                                            {
                                                td_EnrollmentId.Style.Add("display", "none");
                                            }
                                            Page.ClientScript.RegisterStartupScript(GetType(), "JScript3", "<script language='javascript'>PageLoad();</script>");
                                            if (flagEntity)
                                            {
                                                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Saved & New GSTIN Updated Successfully')</script>");
                                            }
                                            else
                                            {
                                                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Saved Successfully')</script>");
                                            }


                                            return;
                                            //}
                                        }
                                    }
                                }

                                if (dt1.Rows.Count <= 0)
                                {
                                    String value = "Statustype='" + status + "',cnt_ucc='" + txtClentUcc.Text + "', cnt_salutation=" + CmbSalutation.SelectedItem.Value + ",  cnt_firstName='" + txtFirstNmae.Text + "', cnt_middleName='" + txtMiddleName.Text + "', cnt_lastName='" + txtLastName.Text + "', cnt_shortName='" + txtAliasName.Text + "',cnt_PrintNameToCheque='" + txtPname.Text + "' ,cnt_branchId=" + cmbBranch.SelectedItem.Value + ", cnt_sex=" + cmbGender.SelectedItem.Value + ", cnt_maritalStatus=" + cmbMaritalStatus.SelectedItem.Value + ", cnt_DOB='" + txtDOB.Value + "', cnt_anniversaryDate='" + txtAnniversary.Value + "', cnt_legalStatus=" + cmbLegalStatus.SelectedItem.Value + ", cnt_education=" + cmbEducation.SelectedItem.Value + ", cnt_profession=" + cmbProfession.SelectedItem.Value + ", cnt_organization='" + txtOrganization.Text + "', cnt_jobResponsibility=" + cmbJobResponsibility.SelectedItem.Value + ", cnt_designation=" + cmbDesignation.SelectedItem.Value + ", cnt_industry=" + cmbIndustry.SelectedItem.Value + ", cnt_contactSource=" + cmbSource.SelectedItem.Value + ", cnt_referedBy='" + txtReferedBy_hidden.Text + "', cnt_contactType='" + ContType + "', cnt_contactStatus=" + cmbContactStatus.SelectedItem.Value + ",cnt_RegistrationDate='" + txtDateRegis.Value + "',cnt_rating='" + cmbRating.SelectedItem.Value + "',cnt_reason='" + TxtContactStatus.Text + "',cnt_bloodgroup='" + cmbBloodgroup.SelectedItem.Value + "',WebLogIn='" + webLogin + "',PassWord='" + Password + "', lastModifyDate ='" + today + "',lastModifyUser=" + HttpContext.Current.Session["userid"]; // + Session["userid"]
                                    oDBEngine.SetFieldValue("tbl_master_contact", value, " cnt_id=" + HttpContext.Current.Session["KeyVal"]);
                                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript2", "<script language='javascript'>PageLoad();</script>");
                                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Saved Succesfully')</script>");

                                    return;
                                }
                                else
                                {
                                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript2", "<script language='javascript'>PageLoad();</script>");

                                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('You have entered duplicate Unique ID.')</script>");

                                    txtClentUcc.Text = "";
                                    txtClentUcc.Focus();
                                    return;
                                }
                                //Rev Subhra 03-06-2019
                                if (Convert.ToString(Session["requesttype"]) == "Lead")
                                {
                                    oDBEngine.SetFieldValue("tbl_master_contact", "EnteredDate='" + dt_EnteredOn.Value + "' ", " cnt_id=" + HttpContext.Current.Session["KeyVal"]);
                                }
                                //End of Rev 03-06-2019

                            }


                            if (ViewState["PartnerId"] != null)
                            {
                                if (Convert.ToString(ViewState["PartnerId"]) != txtRPartner_hidden.Text)
                                {
                                    Int32 rowsAffected = oDBEngine.SetFieldValue("tbl_trans_contactInfo", "ToDate='" + today + "',LastModifyDate='" + today + "',LastModifyUser='" + Convert.ToString(Session["userid"]) + "'", " cnt_internalId='" + Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]) + "'");
                                    oDBEngine.InsurtFieldValue("tbl_trans_contactInfo", "cnt_internalid,Rep_partnerid,FromDate,branchid,CreateDate,CreateUser", "'" + Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]) + "','" + txtRPartner_hidden.Text + "','" + today + "','" + cmbBranch.SelectedItem.Value + "','" + today + "','" + Convert.ToString(Session["userid"]) + "'");
                                    ViewState["PartnerId"] = txtRPartner_hidden.Text;
                                }
                            }
                            else
                            {
                                if (txtRPartner_hidden.Text != "")
                                {
                                    oDBEngine.InsurtFieldValue("tbl_trans_contactInfo", "cnt_internalid,Rep_partnerid,FromDate,branchid,CreateDate,CreateUser", "'" + HttpContext.Current.Session["KeyVal_InternalID"].ToString() + "','" + txtRPartner_hidden.Text + "','" + today + "','" + cmbBranch.SelectedItem.Value + "','" + today + "','" + Session["userid"].ToString() + "'");
                                    oDBEngine.DeleteValue("tbl_trans_contactInfo", " cnt_internalid='" + Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]) + "' and Rep_partnerId=''");
                                }
                            }
                        }
                        else               //For Insert
                        {
                            if (Request.QueryString["requesttypeP"] != null)
                            {

                            }
                            else
                            {
                                try
                                {

                                    DateTime dtDob, dtanniversary, dtReg, dtBusiness, dtenteredon;

                                    if (!reCat.isAllMandetoryDone((DataTable)Session["UdfDataOnAdd"], Convert.ToString(hdKeyVal.Value)))
                                    {
                                        Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>InvalidUDF();</script>");
                                        return;
                                    }

                                    string dd = Convert.ToString(Session["requesttype"]);

                                    // Code Added by Sam on 09022018 for Mantis Issue 0015725 for Transport Enrollment ID new field Added Section Start


                                    if (dd == "Transporter")
                                    {
                                        TransporterEnrollmentId = txt_EnrollmentId.Text.Trim();
                                    }
                                    // Code Added by Sam on 09022018 for Mantis Issue 0015725 for Transport Enrollment ID new field Added Section End

                                    if (txtDOB.Value != null)
                                    {
                                        dtDob = Convert.ToDateTime(txtDOB.Value);
                                    }
                                    else
                                    {
                                        dtDob = Convert.ToDateTime("01-01-1900");
                                    }

                                    if (txtAnniversary.Value != null)
                                    {
                                        dtanniversary = Convert.ToDateTime(txtAnniversary.Value);
                                    }
                                    else
                                    {
                                        dtanniversary = Convert.ToDateTime("01-01-1900");
                                    }

                                    //Rev ----Subhra-----03-06-2019
                                    if (dt_EnteredOn.Value != null)
                                    {
                                        dtenteredon = Convert.ToDateTime(dt_EnteredOn.Value);
                                    }
                                    else
                                    {
                                        dtenteredon = Convert.ToDateTime("01-01-1900");
                                    }
                                    //End of Rev Subhra 03-06-2019

                                    if (txtDateRegis.Value != null)
                                    {
                                        dtReg = Convert.ToDateTime(txtDateRegis.Value);
                                    }
                                    else
                                    {
                                        dtReg = Convert.ToDateTime("01-01-1900");
                                    }


                                    if (txtClentUcc.Text != "")
                                    {
                                        webLogin = "Yes";
                                        Password = txtClentUcc.Text;
                                    }
                                    else
                                    {
                                        webLogin = "No";
                                        Password = "";
                                    }


                                    string other = "";
                                    if (cmbProfession.SelectedValue == "20")
                                        other = txtotheroccu.Text.Trim();
                                    else
                                        other = "";

                                    if (txtClentUcc.Text != "")
                                    {
                                        webLogin = "Yes";
                                        Password = txtClentUcc.Text;
                                    }
                                    else
                                    {
                                        webLogin = "No";
                                        Password = "";
                                    }



                                    string vPlaceincop;

                                    if (txtincorporation.Text == "")
                                    {
                                        vPlaceincop = "";
                                        dtBusiness = Convert.ToDateTime("01-01-1900");
                                    }
                                    else
                                    {
                                        vPlaceincop = txtincorporation.Text.Trim();
                                        dtBusiness = Convert.ToDateTime(txtFromDate.Value);
                                    }
                                    if (txtcreditDays.Text == string.Empty)
                                    {
                                        txtcreditDays.Text = "0";
                                    }

                                    string GSTIN = "";

                                    if (radioregistercheck.SelectedValue != "0")
                                    {
                                        GSTIN = txtGSTIN1.Text.Trim() + txtGSTIN2.Text.Trim() + txtGSTIN3.Text.Trim();
                                    }

                                    //rev srijeeta

                                    //string InternalId = oContactGeneralBL.Insert_ContactGeneral(dd, txtClentUcc.Text.Trim(), CmbSalutation.SelectedItem.Value,
                                    //                  txtFirstNmae.Text.Trim(), txtMiddleName.Text.Trim(), txtLastName.Text.Trim(),
                                    //                  txtAliasName.Text.Trim(), cmbBranch.SelectedItem.Value, cmbGender.SelectedItem.Value,
                                    //                  cmbMaritalStatus.SelectedItem.Value, dtDob, dtanniversary, cmbLegalStatus.SelectedItem.Value,
                                    //                  cmbEducation.SelectedItem.Value, cmbProfession.SelectedItem.Value, txtOrganization.Text.Trim(),
                                    //                  cmbJobResponsibility.SelectedItem.Value, cmbDesignation.SelectedItem.Value, cmbIndustry.SelectedItem.Value,
                                    //                  cmbSource.SelectedItem.Value, txtReferedBy.Text.Trim(), txtRPartner_hidden.Text.Trim(), ContType,
                                    //                  cmbContactStatus.SelectedItem.Value, dtReg, cmbRating.SelectedItem.Value, TxtContactStatus.Text.Trim(),
                                    //                  cmbBloodgroup.SelectedItem.Value, webLogin, Password, Convert.ToString(HttpContext.Current.Session["userid"]), vPlaceincop,
                                    //                  dtBusiness, other, country, Creditcard, Convert.ToInt32(txtcreditDays.Text.Trim()), Convert.ToDecimal(txtCreditLimit.Text.Trim()), Convert.ToString(cmbContactStatusclient.SelectedItem.Value),
                                    //                  GSTIN, hidAssociatedEmp.Value, txtPname.Text);
                                    string InternalId = oContactGeneralBL.Insert_ContactGeneral(dd, txtClentUcc.Text.Trim(),"", CmbSalutation.SelectedItem.Value,
                                                      txtFirstNmae.Text.Trim(), txtMiddleName.Text.Trim(), txtLastName.Text.Trim(),
                                                      txtAliasName.Text.Trim(), cmbBranch.SelectedItem.Value, cmbGender.SelectedItem.Value,
                                                      cmbMaritalStatus.SelectedItem.Value, dtDob, dtanniversary, cmbLegalStatus.SelectedItem.Value,
                                                      cmbEducation.SelectedItem.Value, cmbProfession.SelectedItem.Value, txtOrganization.Text.Trim(),
                                                      cmbJobResponsibility.SelectedItem.Value, cmbDesignation.SelectedItem.Value, cmbIndustry.SelectedItem.Value,
                                                      cmbSource.SelectedItem.Value, txtReferedBy.Text.Trim(), txtRPartner_hidden.Text.Trim(), ContType,
                                                      cmbContactStatus.SelectedItem.Value, dtReg, cmbRating.SelectedItem.Value, TxtContactStatus.Text.Trim(),
                                                      cmbBloodgroup.SelectedItem.Value, webLogin, Password, Convert.ToString(HttpContext.Current.Session["userid"]), vPlaceincop,
                                                      dtBusiness, other, country, Creditcard, Convert.ToInt32(txtcreditDays.Text.Trim()), Convert.ToDecimal(txtCreditLimit.Text.Trim()), Convert.ToString(cmbContactStatusclient.SelectedItem.Value),
                                                      GSTIN, hidAssociatedEmp.Value, txtPname.Text);
                                    //end of rev srijeeta
                                    if (Convert.ToString(Session["requesttype"]) == "Customer/Client")
                                    {
                                        Int32 rowsEffected = oDBEngine.SetFieldValue("tbl_master_contact", "cnt_mainAccount='" + hndTaxRates_MainAccount_hidden.Value + "',cnt_IdType=" + ddlIdType.SelectedItem.Value, " cnt_internalId='" + InternalId + "'");
                                        if (rowsEffected > 0)
                                        {
                                            oContactGeneralBL.Insert_DataByIDType(InternalId, ddlIdType.SelectedItem.Value, txtClentUcc.Text.Trim());
                                        }
                                    }

                                    //Rev Subhra 03-06-2019
                                    if (Convert.ToString(Session["requesttype"]) == "Lead")
                                    {
                                        Int32 rowsEffected = oDBEngine.SetFieldValue("tbl_master_contact", "EnteredDate='" + dtenteredon + "' ", " cnt_internalId='" + InternalId + "'");
                                    }
                                    //End of Rev 03-06-2019

                                    if (Convert.ToString(Session["requesttype"]) == "Relationship Partners")
                                    {
                                        Int32 rowsEffected = oDBEngine.SetFieldValue("tbl_master_contact", "cnt_mainAccount='" + hndTaxRates_MainAccount_hidden.Value + "'", " cnt_internalId='" + InternalId + "'");
                                        if (rowsEffected > 0)
                                        {
                                            oContactGeneralBL.Insert_DataByIDType(InternalId, ddlIdType.SelectedItem.Value, txtClentUcc.Text.Trim());
                                        }
                                    }

                                    HttpContext.Current.Session["KeyVal_InternalID"] = InternalId;


                                    if (Convert.ToString(Session["requesttype"]) == "Transporter")
                                    {
                                        Int32 rowsEffected = oDBEngine.SetFieldValue("tbl_master_contact", "cnt_mainAccount='" + hndTaxRates_MainAccount_hidden.Value + "',Enrolment_ID='" + TransporterEnrollmentId + "'", " cnt_internalId='" + InternalId + "'");
                                        //if (rowsEffected > 0)
                                        //{
                                        //    oContactGeneralBL.Insert_DataByIDType(InternalId, ddlIdType.SelectedItem.Value, txtClentUcc.Text.Trim());
                                        //}


                                        string vehicleNos = VehicleNo_hidden.Value;
                                        if (Convert.ToString(cmbLegalStatus.SelectedValue) == "54")//Local
                                        {
                                            //Contact ct = new Contact();
                                            //ct.Delete_TransporterVehicles(Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]));
                                            //VehicleNo_hidden.Value = "";
                                            if (!string.IsNullOrEmpty(vehicleNos))
                                            {
                                                Contact ct = new Contact();
                                                ct.Insert_TransporterVehicles(Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]), vehicleNos);
                                            }
                                        }
                                        else
                                        {
                                            //if (!string.IsNullOrEmpty(vehicleNos))
                                            //{
                                            //    Contact ct = new Contact();
                                            //    ct.Insert_TransporterVehicles(Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]), vehicleNos);
                                            //}

                                            Contact ct = new Contact();
                                            ct.Delete_TransporterVehicles(Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]));
                                            VehicleNo_hidden.Value = "";
                                        }
                                    }

                                    //.........end..........
                                    DataTable udfTable = (DataTable)Session["UdfDataOnAdd"];
                                    if (udfTable != null)
                                        Session["UdfDataOnAdd"] = reCat.insertRemarksCategoryAddMode(Convert.ToString(hdKeyVal.Value), InternalId, udfTable, Convert.ToString(Session["userid"]));


                                    HttpContext.Current.Session["KeyVal_InternalID"] = InternalId;

                                    UserLastSegment = Convert.ToString(HttpContext.Current.Session["userlastsegment"]);
                                    if (UserLastSegment != "1" && UserLastSegment != "4" && UserLastSegment != "6" && UserLastSegment != "9" && UserLastSegment != "10")
                                    {
                                        if (InternalId.Contains("CL"))
                                        {
                                            oDBEngine.InsurtFieldValue("tbl_master_contactexchange", "crg_cntid,crg_company,crg_exchange,crg_tcode,createdate,createuser,crg_sttpattern,crg_sttwap,crg_SegmentID", "'" + Convert.ToString(InternalId).Trim() + "','" + HttpContext.Current.Session["LastCompany"].ToString().Trim() + "','" + Convert.ToString(segregis).Trim() + "','" + txtClentUcc.Text.Trim() + "','" + Convert.ToString(oDBEngine.GetDate()) + "','" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "','D','W','" + Convert.ToString(HttpContext.Current.Session["usersegid"]).Trim() + "'");
                                        }
                                    }

                                    string popupScript = "";
                                    popupScript = "<script language='javascript'>" + "PageLoad();</script>";
                                    ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
                                    string[,] cnt_id = oDBEngine.GetFieldValue(" tbl_master_contact", " cnt_id", " cnt_internalId='" + InternalId + "'", 1);
                                    if (Convert.ToString(cnt_id[0, 0]) != "n")
                                    {
                                        Response.Redirect("Contact_general.aspx?id=" + Convert.ToString(cnt_id[0, 0]), false);
                                    }

                                }
                                catch
                                {
                                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript5", "<script language='javascript'>PageLoad();</script>");
                                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Internal Error.')</script>");
                                    txtClentUcc.Text = "";
                                    txtClentUcc.Focus();

                                }
                            }
                        }
                    }
                }
                else
                {
                    if (txtAliasName.Text.Length > 0)
                    {

                        string today = Convert.ToString(oDBEngine.GetDate());
                        if (Convert.ToString(HttpContext.Current.Session["KeyVal"]) == "0")
                        {
                            //----------------- For Tier Structure Start--------------------------------
                            if (!reCat.isAllMandetoryDone((DataTable)Session["UdfDataOnAdd"], Convert.ToString(hdKeyVal.Value)))
                            {
                                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>InvalidUDF();</script>");
                                return;
                            }
                            DateTime dtDob, dtanniversary, dtReg, dtBusiness, dtenteredon;

                            string dd = Convert.ToString(Session["requesttype"]);

                            if (txtDOB.Value != null)
                            {
                                dtDob = Convert.ToDateTime(txtDOB.Value);
                            }
                            else
                            {
                                dtDob = Convert.ToDateTime("01-01-1900");
                            }

                            if (txtAnniversary.Value != null)
                            {
                                dtanniversary = Convert.ToDateTime(txtAnniversary.Value);
                            }
                            else
                            {
                                dtanniversary = Convert.ToDateTime("01-01-1900");
                            }
                            //Rev ----Subhra-----03-06-2019
                            if (dt_EnteredOn.Value != null)
                            {
                                dtenteredon = Convert.ToDateTime(dt_EnteredOn.Value);
                            }
                            else
                            {
                                dtenteredon = Convert.ToDateTime("01-01-1900");
                            }
                            //End of Rev Subhra 03-06-2019
                            if (txtDateRegis.Value != null)
                            {
                                dtReg = Convert.ToDateTime(txtDateRegis.Value);
                            }
                            else
                            {
                                dtReg = Convert.ToDateTime("01-01-1900");
                            }


                            if (txtClentUcc.Text != "")
                            {
                                webLogin = "Yes";
                                Password = txtClentUcc.Text;
                            }
                            else
                            {
                                webLogin = "No";
                                Password = "";
                            }


                            string other = "";
                            if (cmbProfession.SelectedValue == "20")
                                other = Convert.ToString(txtotheroccu.Text).Trim();
                            else
                                other = "";

                            if (txtClentUcc.Text != "")
                            {
                                webLogin = "Yes";
                                Password = txtClentUcc.Text;
                            }
                            else
                            {
                                webLogin = "No";
                                Password = "";
                            }

                            string vPlaceincop;

                            if (txtincorporation.Text == "")
                            {
                                vPlaceincop = "";
                                dtBusiness = Convert.ToDateTime("01-01-1900");
                            }
                            else
                            {
                                vPlaceincop = txtincorporation.Text.Trim();
                                dtBusiness = Convert.ToDateTime(txtFromDate.Value);
                            }

                            //debjyoti 060217
                            string GSTIN = "";
                            // GSTIN = txtGSTIN1.Text.Trim() + txtGSTIN2.Text.Trim() + txtGSTIN3.Text.Trim();
                            if (radioregistercheck.SelectedValue != "0")
                            {
                                GSTIN = txtGSTIN1.Text.Trim() + txtGSTIN2.Text.Trim() + txtGSTIN3.Text.Trim();
                            }
                            //rev srijeeta
                            //string InternalId = oContactGeneralBL.Insert_ContactGeneral(dd, "", CmbSalutation.SelectedItem.Value,
                            //                               txtFirstNmae.Text.Trim(), txtMiddleName.Text.Trim(), txtLastName.Text.Trim(),
                            //                               txtAliasName.Text.Trim(), cmbBranch.SelectedItem.Value, cmbGender.SelectedItem.Value,
                            //                               cmbMaritalStatus.SelectedItem.Value, dtDob, dtanniversary, cmbLegalStatus.SelectedItem.Value,
                            //                               cmbEducation.SelectedItem.Value, cmbProfession.SelectedItem.Value, txtOrganization.Text.Trim(),
                            //                               cmbJobResponsibility.SelectedItem.Value, cmbDesignation.SelectedItem.Value, cmbIndustry.SelectedItem.Value,
                            //                               cmbSource.SelectedItem.Value, txtReferedBy.Text.Trim(), txtRPartner_hidden.Text.Trim(), ContType,
                            //                               cmbContactStatus.SelectedItem.Value, dtReg, cmbRating.SelectedItem.Value, TxtContactStatus.Text.Trim(),
                            //                               cmbBloodgroup.SelectedItem.Value, webLogin, Password, Convert.ToString(HttpContext.Current.Session["userid"]), vPlaceincop,
                            //                               dtBusiness, other, country, Creditcard, Convert.ToInt32(txtcreditDays.Text.Trim()), Convert.ToDecimal(txtCreditLimit.Text.Trim()), Convert.ToString(cmbContactStatusclient.SelectedItem.Value)
                            //                               , GSTIN, hidAssociatedEmp.Value, txtPname.Text
                            //               );
                            string InternalId = oContactGeneralBL.Insert_ContactGeneral(dd, "", "",CmbSalutation.SelectedItem.Value,
                                                           txtFirstNmae.Text.Trim(), txtMiddleName.Text.Trim(), txtLastName.Text.Trim(),
                                                           txtAliasName.Text.Trim(), cmbBranch.SelectedItem.Value, cmbGender.SelectedItem.Value,
                                                           cmbMaritalStatus.SelectedItem.Value, dtDob, dtanniversary, cmbLegalStatus.SelectedItem.Value,
                                                           cmbEducation.SelectedItem.Value, cmbProfession.SelectedItem.Value, txtOrganization.Text.Trim(),
                                                           cmbJobResponsibility.SelectedItem.Value, cmbDesignation.SelectedItem.Value, cmbIndustry.SelectedItem.Value,
                                                           cmbSource.SelectedItem.Value, txtReferedBy.Text.Trim(), txtRPartner_hidden.Text.Trim(), ContType,
                                                           cmbContactStatus.SelectedItem.Value, dtReg, cmbRating.SelectedItem.Value, TxtContactStatus.Text.Trim(),
                                                           cmbBloodgroup.SelectedItem.Value, webLogin, Password, Convert.ToString(HttpContext.Current.Session["userid"]), vPlaceincop,
                                                           dtBusiness, other, country, Creditcard, Convert.ToInt32(txtcreditDays.Text.Trim()), Convert.ToDecimal(txtCreditLimit.Text.Trim()), Convert.ToString(cmbContactStatusclient.SelectedItem.Value)
                                                           , GSTIN, hidAssociatedEmp.Value, txtPname.Text
                                           );
                            //end of rev srijeeta

                            DataTable udfTable = (DataTable)Session["UdfDataOnAdd"];
                            //Rev Subhra 03-06-2019
                            if (Convert.ToString(Session["requesttype"]) == "Lead")
                            {
                                Int32 rowsEffected = oDBEngine.SetFieldValue("tbl_master_contact", "EnteredDate='" + dtenteredon + "' ", " cnt_internalId='" + InternalId + "'");
                            }
                            //End of Rev 03-06-2019
                            if (udfTable != null)
                                Session["UdfDataOnAdd"] = reCat.insertRemarksCategoryAddMode(Convert.ToString(hdKeyVal.Value), InternalId, udfTable, Convert.ToString(Session["userid"]));
                            //----------------------------------For Tier Structure End-------------------------------------------------------------------------------


                            HttpContext.Current.Session["KeyVal_InternalID"] = InternalId;

                            UserLastSegment = Convert.ToString(HttpContext.Current.Session["userlastsegment"]);
                            if (UserLastSegment != "1" && UserLastSegment != "4" && UserLastSegment != "6" && UserLastSegment != "9" && UserLastSegment != "10")
                            {
                                if (InternalId.Contains("CL"))
                                {
                                    oDBEngine.InsurtFieldValue("tbl_master_contactexchange", "crg_cntid,crg_company,crg_exchange,crg_tcode,createdate,createuser,crg_sttpattern,crg_sttwap,crg_SegmentID", "'" + InternalId.ToString().Trim() + "','" + HttpContext.Current.Session["LastCompany"].ToString().Trim() + "','" + segregis.ToString().Trim() + "','" + txtClentUcc.Text.ToString().Trim() + "','" + oDBEngine.GetDate().ToString() + "','" + HttpContext.Current.Session["userid"].ToString().Trim() + "','D','W','" + HttpContext.Current.Session["usersegid"].ToString().Trim() + "'");
                                }
                            }


                            string popupScript = "";
                            popupScript = "<script language='javascript'>" + "PageLoad();</script>";
                            Page.ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
                            string[,] cnt_id = oDBEngine.GetFieldValue(" tbl_master_contact", " cnt_id", " cnt_internalId='" + InternalId + "'", 1);
                            if (Convert.ToString(cnt_id[0, 0]) != "n")
                            {
                                Response.Redirect("Contact_general.aspx?id=" + Convert.ToString(cnt_id[0, 0]), false);
                            }
                        }
                        else
                        {
                            if (chkAllow.Checked == true)
                            {
                                webLogin = "Yes";
                                Password = txtClentUcc.Text;
                            }
                            else
                            {
                                webLogin = "No";
                            }
                            if (cmbSource.SelectedItem.Value == "0")
                            {
                                txtReferedBy_hidden.Text = null;
                            }
                            if (txtReferedBy.Text == "")
                            {
                                txtReferedBy_hidden.Text = null;
                            }
                            if (cmbSource.SelectedItem.Value == "18")
                            {
                                String value = "cnt_ucc='" + txtClentUcc.Text + "', cnt_salutation=" + CmbSalutation.SelectedItem.Value + ",  cnt_firstName='" + txtFirstNmae.Text + "', cnt_middleName='" + txtMiddleName.Text + "', cnt_lastName='" + txtLastName.Text + "', cnt_shortName='" + txtAliasName.Text + "', cnt_branchId=" + cmbBranch.SelectedItem.Value + ", cnt_sex=" + cmbGender.SelectedItem.Value + ", cnt_maritalStatus=" + cmbMaritalStatus.SelectedItem.Value + ", cnt_DOB='" + txtDOB.Value + "', cnt_anniversaryDate='" + txtAnniversary.Value + "', cnt_legalStatus=" + cmbLegalStatus.SelectedItem.Value + ", cnt_education=" + cmbEducation.SelectedItem.Value + ", cnt_profession=" + cmbProfession.SelectedItem.Value + ", cnt_organization='" + txtOrganization.Text + "', cnt_jobResponsibility=" + cmbJobResponsibility.SelectedItem.Value + ", cnt_designation=" + cmbDesignation.SelectedItem.Value + ", cnt_industry=" + cmbIndustry.SelectedItem.Value + ", cnt_contactSource=" + cmbSource.SelectedItem.Value + ", cnt_referedBy='" + txtReferedBy.Text + "', cnt_contactType='" + ContType + "', cnt_contactStatus=" + cmbContactStatus.SelectedItem.Value + ",cnt_RegistrationDate='" + txtDateRegis.Value + "',cnt_rating='" + cmbRating.SelectedItem.Value + "',cnt_reason='" + TxtContactStatus.Text + "',cnt_bloodgroup='" + cmbBloodgroup.SelectedItem.Value + "',WebLogIn='" + webLogin + "',PassWord='" + Password + "', lastModifyDate ='" + today + "', lastModifyUser=" + HttpContext.Current.Session["userid"]; // + Session["userid"]
                                oDBEngine.SetFieldValue("tbl_master_contact", value, " cnt_id=" + HttpContext.Current.Session["KeyVal"]);
                            }
                            else
                            {
                                String value = "cnt_ucc='" + txtClentUcc.Text + "', cnt_salutation=" + CmbSalutation.SelectedItem.Value + ",  cnt_firstName='" + txtFirstNmae.Text + "', cnt_middleName='" + txtMiddleName.Text + "', cnt_lastName='" + txtLastName.Text + "', cnt_shortName='" + txtAliasName.Text + "', cnt_branchId=" + cmbBranch.SelectedItem.Value + ", cnt_sex=" + cmbGender.SelectedItem.Value + ", cnt_maritalStatus=" + cmbMaritalStatus.SelectedItem.Value + ", cnt_DOB='" + txtDOB.Value + "', cnt_anniversaryDate='" + txtAnniversary.Value + "', cnt_legalStatus=" + cmbLegalStatus.SelectedItem.Value + ", cnt_education=" + cmbEducation.SelectedItem.Value + ", cnt_profession=" + cmbProfession.SelectedItem.Value + ", cnt_organization='" + txtOrganization.Text + "', cnt_jobResponsibility=" + cmbJobResponsibility.SelectedItem.Value + ", cnt_designation=" + cmbDesignation.SelectedItem.Value + ", cnt_industry=" + cmbIndustry.SelectedItem.Value + ", cnt_contactSource=" + cmbSource.SelectedItem.Value + ", cnt_referedBy='" + txtReferedBy_hidden.Text + "', cnt_contactType='" + ContType + "', cnt_contactStatus=" + cmbContactStatus.SelectedItem.Value + ",cnt_RegistrationDate='" + txtDateRegis.Value + "',cnt_rating='" + cmbRating.SelectedItem.Value + "',cnt_reason='" + TxtContactStatus.Text + "',cnt_bloodgroup='" + cmbBloodgroup.SelectedItem.Value + "',WebLogIn='" + webLogin + "',PassWord='" + Password + "', lastModifyDate ='" + today + "', lastModifyUser=" + HttpContext.Current.Session["userid"]; // + Session["userid"]

                                oDBEngine.SetFieldValue("tbl_master_contact", value, " cnt_id=" + HttpContext.Current.Session["KeyVal"]);

                                Page.ClientScript.RegisterStartupScript(GetType(), "JScript3", "<script language='javascript'>PageLoad();</script>");
                                Response.Redirect("Contact_general.aspx?id=" + HttpContext.Current.Session["KeyVal"], false);

                                return;
                            }
                        }
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "JScript3", "<script language='javascript'>PageLoad();</script>");
                        Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Please Insert Unique ID')</script>");

                        Response.Redirect("Contact_general.aspx?id=" + HttpContext.Current.Session["KeyVal"], false);
                    }
                }

                if (Request.QueryString["formtype"] != null)
                {
                    if (Convert.ToString(Request.QueryString["formtype"]) == "leadSales")
                    {
                        string popupScript = "";
                        popupScript = "<script language='javascript'>" + "window.opener.location.href=window.opener.location.href;window.close();</script>";
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
            else /////////////////// For Leads add/edit
            {

                string country = ddlnational.SelectedValue;

                string status = Convert.ToString(cmbContactStatusclient.SelectedItem.Value);
                string webLogin = "";
                string Password = "";
                // string ContType = "";
                if (Session["requesttype"] != null) //Lead add
                {
                    ContType = retContactType(Convert.ToString(Session["requesttype"]));
                    //switch (Convert.ToString(Session["requesttype"]))
                    //{
                    //    case "Lead":
                    //        ContType = "LD";
                    //        break;
                    //}
                }
                if (ContType != "XC") //Lead add/ edit
                {
                    if (Request.QueryString["formtype"] != null)
                    {

                    }
                    else  // Lead Add/edit
                    {
                        DataTable dt1 = oDBEngine.GetDataTable("tbl_master_contactExchange,tbl_master_contact", "top 1 cnt_ucc,crg_tcode", "cnt_ucc='" + LDtxtClentUcc.Text.ToString() + "' or crg_tcode='" + LDtxtClentUcc.Text.ToString() + "'  and cnt_id='" + Convert.ToString(HttpContext.Current.Session["KeyVal"]) + "'");
                        DataTable dt2 = oDBEngine.GetDataTable("tbl_master_contact", " cnt_id", "cnt_id='" + Convert.ToString(HttpContext.Current.Session["KeyVal"]) + "'");
                        string[,] abcd = oDBEngine.GetFieldValue("tbl_master_contactExchange,tbl_master_contact", "top 1 cnt_ucc,crg_tcode", "cnt_ucc='" + LDtxtClentUcc.Text.ToString() + "' or crg_tcode='" + LDtxtClentUcc.Text.ToString() + "'  and cnt_id='" + Convert.ToString(HttpContext.Current.Session["KeyVal"]) + "'", 2);
                        string[,] bName = oDBEngine.GetFieldValue("tbl_master_contact", "cnt_ucc,cnt_id", "cnt_id='" + HttpContext.Current.Session["KeyVal"].ToString() + "'", 2);
                        if (Convert.ToString(HttpContext.Current.Session["KeyVal"]) != "0")        //For Update
                        {
                            SubAcName = LDtxtFirstNmae.Text.ToString().Trim() + " " + LDtxtMiddleName.Text.ToString().Trim() + " " + LDtxtLastName.Text.ToString().Trim();
                            Int32 RowEffect = oDBEngine.SetFieldValue("master_subaccount", " SubAccount_Name='" + SubAcName + "'", " SubAccount_Code='" + Convert.ToString(Session["KeyVal_InternalID"]) + "'");

                            string today = Convert.ToString(oDBEngine.GetDate());
                            if (chkAllow.Checked == true)
                            {
                                webLogin = "Yes";
                                Password = txtClentUcc.Text;
                            }
                            else
                            {
                                webLogin = "No";
                            }



                            if (LDcmbSource.SelectedItem.Value == "18")  // Lead Edit
                            {
                                String value = "";
                                string other = "";
                                if (LDcmbProfession.SelectedValue == "20")
                                    other = "";
                                else
                                    other = "";



                                value = "Statustype='" + status + "',cnt_ucc='" + LDtxtClentUcc.Text + "', cnt_salutation=" + LDCmbSalutation.SelectedItem.Value + ",  cnt_firstName='" + LDtxtFirstNmae.Text + "', cnt_middleName='" + LDtxtMiddleName.Text + "', cnt_lastName='" + LDtxtLastName.Text + "', cnt_shortName='" + LDtxtAliasName.Text + "', cnt_branchId=" + LDcmbBranch.SelectedItem.Value + ", cnt_sex=" + LDcmbGender.SelectedItem.Value + ", cnt_maritalStatus=" + LDcmbMaritalStatus.SelectedItem.Value + ", cnt_DOB='" + LDtxtDOB.Value + "', cnt_anniversaryDate='" + LDtxtAnniversary.Value + "', cnt_legalStatus=" + LDcmbLegalStatus.SelectedItem.Value + ", cnt_education=" + LDcmbEducation.SelectedItem.Value + ", cnt_profession=" + LDcmbProfession.SelectedItem.Value + ", cnt_organization='" + LDtxtOrganization.Text + "', cnt_jobResponsibility=" + LDcmbJobResponsibility.SelectedItem.Value + ", cnt_designation=" + LDcmbDesignation.SelectedItem.Value + ", cnt_industry=" + LDcmbIndustry.SelectedItem.Value + ", cnt_contactSource=" + LDcmbSource.SelectedItem.Value + ", cnt_referedBy='" + LDtxtReferedBy_hidden.Value + "', cnt_contactType='" + ContType + "', cnt_contactStatus=" + LDcmbContactStatus.SelectedItem.Value + ",cnt_RegistrationDate='',cnt_rating='" + LDcmbRating.SelectedItem.Value + "',cnt_reason='',cnt_bloodgroup='" + LDcmbBloodgroup.SelectedItem.Value + "',WebLogIn='" + webLogin + "',PassWord='" + Password + "', lastModifyDate ='" + today + "',cnt_PlaceOfIncorporation='',cnt_BusinessComncDate='',cnt_OtherOccupation='" + other + "', lastModifyUser=" + HttpContext.Current.Session["userid"]; // + Session["userid"]


                                oDBEngine.SetFieldValue("tbl_master_contact", value, " cnt_id=" + HttpContext.Current.Session["KeyVal"]);
                            }
                            else  // Lead Edit
                            {
                                if (dt1.Rows.Count > 0)
                                {
                                    if (bName[0, 0] != "n")
                                    {
                                        if (bName[0, 0] == abcd[0, 0])
                                        {
                                            String value = "";
                                            string other = "";
                                            if (cmbProfession.SelectedValue == "20")
                                                other = txtotheroccu.Text.ToString().Trim();
                                            else
                                                other = "";
                                            if (txtincorporation.Text == "")
                                            {

                                                value = "Statustype='" + status + "',cnt_ucc='" + LDtxtClentUcc.Text + "', cnt_salutation=" + LDCmbSalutation.SelectedItem.Value + ",  cnt_firstName='" + LDtxtFirstNmae.Text + "', cnt_middleName='" + LDtxtMiddleName.Text + "', cnt_lastName='" + LDtxtLastName.Text + "', cnt_shortName='" + LDtxtAliasName.Text + "', cnt_branchId=" + LDcmbBranch.SelectedItem.Value + ", cnt_sex=" + LDcmbGender.SelectedItem.Value + ", cnt_maritalStatus=" + LDcmbMaritalStatus.SelectedItem.Value + ", cnt_DOB='" + LDtxtDOB.Value + "', cnt_anniversaryDate='" + LDtxtAnniversary.Value + "', cnt_legalStatus=" + LDcmbLegalStatus.SelectedItem.Value + ", cnt_education=" + LDcmbEducation.SelectedItem.Value + ", cnt_profession=" + LDcmbProfession.SelectedItem.Value + ", cnt_organization='" + LDtxtOrganization.Text + "', cnt_jobResponsibility=" + LDcmbJobResponsibility.SelectedItem.Value + ", cnt_designation=" + LDcmbDesignation.SelectedItem.Value + ", cnt_industry=" + LDcmbIndustry.SelectedItem.Value + ", cnt_contactSource=" + LDcmbSource.SelectedItem.Value + ", cnt_referedBy='" + LDtxtReferedBy_hidden.Value + "', cnt_contactType='" + ContType + "', cnt_contactStatus=" + LDcmbContactStatus.SelectedItem.Value + ",cnt_RegistrationDate='',cnt_rating='" + LDcmbRating.SelectedItem.Value + "',cnt_reason='',cnt_bloodgroup='" + LDcmbBloodgroup.SelectedItem.Value + "',WebLogIn='" + webLogin + "',PassWord='" + Password + "', lastModifyDate ='" + today + "',cnt_PlaceOfIncorporation='',cnt_BusinessComncDate='',cnt_OtherOccupation='" + other + "', lastModifyUser='" + HttpContext.Current.Session["userid"] + "'";


                                            }
                                            else
                                            {
                                                value = "Statustype=" + status + ",cnt_ucc='" + LDtxtClentUcc.Text + "', cnt_salutation=" + LDCmbSalutation.SelectedItem.Value + ",  cnt_firstName='" + LDtxtFirstNmae.Text + "', cnt_middleName='" + LDtxtMiddleName.Text + "', cnt_lastName='" + LDtxtLastName.Text + "', cnt_shortName='" + LDtxtAliasName.Text + "', cnt_branchId=" + LDcmbBranch.SelectedItem.Value + ", cnt_sex=" + LDcmbGender.SelectedItem.Value + ", cnt_maritalStatus=" + LDcmbMaritalStatus.SelectedItem.Value + ", cnt_DOB='" + LDtxtDOB.Value + "', cnt_anniversaryDate='" + LDtxtAnniversary.Value + "', cnt_legalStatus=" + LDcmbLegalStatus.SelectedItem.Value + ", cnt_education=" + LDcmbEducation.SelectedItem.Value + ", cnt_profession=" + LDcmbProfession.SelectedItem.Value + ", cnt_organization='" + LDtxtOrganization.Text + "', cnt_jobResponsibility=" + LDcmbJobResponsibility.SelectedItem.Value + ", cnt_designation=" + LDcmbDesignation.SelectedItem.Value + ", cnt_industry=" + LDcmbIndustry.SelectedItem.Value + ", cnt_contactSource=" + LDcmbSource.SelectedItem.Value + ", cnt_referedBy='" + LDtxtReferedBy_hidden.Value + "', cnt_contactType='" + ContType + "', cnt_contactStatus=" + LDcmbContactStatus.SelectedItem.Value + ",cnt_RegistrationDate='',cnt_rating='" + LDcmbRating.SelectedItem.Value + "',cnt_reason='',cnt_bloodgroup='" + LDcmbBloodgroup.SelectedItem.Value + "',WebLogIn='" + webLogin + "',PassWord='" + Password + "', lastModifyDate ='" + today + "',cnt_PlaceOfIncorporation='',cnt_nationality='" + Convert.ToInt32(Convert.ToString(country)) + "',cnt_BusinessComncDate='',cnt_OtherOccupation='" + other + "', lastModifyUser='" + HttpContext.Current.Session["userid"] + "'";
                                            }

                                            oDBEngine.SetFieldValue("tbl_master_contact", value, " cnt_id=" + HttpContext.Current.Session["KeyVal"]);

                                            //Rev Subhra 03-06-2019
                                            if (Convert.ToString(Session["requesttype"]) == "Lead")
                                            {
                                                oDBEngine.SetFieldValue("tbl_master_contact", "EnteredDate='" + dt_EnteredOn.Value + "' ", " cnt_id=" + HttpContext.Current.Session["KeyVal"]);
                                            }
                                            //End of Rev 03-06-2019

                                            if (txtRPartner.Text != "")
                                            {
                                                string[,] count = oDBEngine.GetFieldValue("tbl_trans_contactInfo", "cnt_internalid", "cnt_internalId='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'", 1);
                                                if (count[0, 0] != "n")
                                                {
                                                    string valueforspoc = "cnt_internalId='" + HttpContext.Current.Session["KeyVal_InternalID"] + "',Rep_partnerId='',FromDate='" + Convert.ToString(oDBEngine.GetDate()) + "',branchId='" + LDcmbBranch.SelectedItem.Value + "',CreateDate='" + Convert.ToString(oDBEngine.GetDate()) + "',CreateUser=" + HttpContext.Current.Session["userid"];
                                                    oDBEngine.SetFieldValue("tbl_trans_contactInfo", valueforspoc, " cnt_internalId='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'");
                                                }
                                                else
                                                {
                                                    oDBEngine.InsurtFieldValue("tbl_trans_contactInfo", "cnt_internalid,Rep_partnerid,FromDate,branchid,CreateDate,CreateUser", "'" + Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]) + "','','" + Convert.ToString(oDBEngine.GetDate()) + "','" + LDcmbBranch.SelectedItem.Value + "','" + Convert.ToString(oDBEngine.GetDate()) + "','" + Convert.ToString(Session["userid"]) + "'");
                                                }
                                            }
                                            else
                                            {
                                                oDBEngine.DeleteValue("tbl_trans_contactInfo", "cnt_internalId='" + HttpContext.Current.Session["KeyVal_InternalID"] + "'");
                                            }


                                            Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Saved Succesfully')</script>");

                                            return;

                                        }
                                    }
                                }

                                if (dt1.Rows.Count <= 0)
                                {
                                    String value = "Statustype='" + status + "',cnt_ucc='" + LDtxtClentUcc.Text + "', cnt_salutation=" + LDCmbSalutation.SelectedItem.Value + ",  cnt_firstName='" + LDtxtFirstNmae.Text + "', cnt_middleName='" + LDtxtMiddleName.Text + "', cnt_lastName='" + LDtxtLastName.Text + "', cnt_shortName='" + LDtxtAliasName.Text + "', cnt_branchId=" + LDcmbBranch.SelectedItem.Value + ", cnt_sex=" + LDcmbGender.SelectedItem.Value + ", cnt_maritalStatus=" + LDcmbMaritalStatus.SelectedItem.Value + ", cnt_DOB='" + LDtxtDOB.Value + "', cnt_anniversaryDate='" + LDtxtAnniversary.Value + "', cnt_legalStatus=" + LDcmbLegalStatus.SelectedItem.Value + ", cnt_education=" + LDcmbEducation.SelectedItem.Value + ", cnt_profession=" + LDcmbProfession.SelectedItem.Value + ", cnt_organization='" + LDtxtOrganization.Text + "', cnt_jobResponsibility=" + LDcmbJobResponsibility.SelectedItem.Value + ", cnt_designation=" + LDcmbDesignation.SelectedItem.Value + ", cnt_industry=" + LDcmbIndustry.SelectedItem.Value + ", cnt_contactSource=" + LDcmbSource.SelectedItem.Value + ", cnt_referedBy='" + LDtxtReferedBy_hidden.Value + "', cnt_contactType='" + ContType + "', cnt_contactStatus=" + LDcmbContactStatus.SelectedItem.Value + ",cnt_RegistrationDate='',cnt_rating='" + LDcmbRating.SelectedItem.Value + "',cnt_reason='',cnt_bloodgroup='" + LDcmbBloodgroup.SelectedItem.Value + "',WebLogIn='" + webLogin + "',PassWord='" + Password + "', lastModifyDate ='" + today + "', lastModifyUser=" + HttpContext.Current.Session["userid"]; // + Session["userid"]
                                    oDBEngine.SetFieldValue("tbl_master_contact", value, " cnt_id=" + HttpContext.Current.Session["KeyVal"]);
                                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Saved Succesfully')</script>");

                                    return;
                                }
                                else
                                {

                                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('You have entered duplicate Unique ID.')</script>");
                                    txtClentUcc.Text = "";
                                    txtClentUcc.Focus();
                                    return;
                                }
                            }


                        }
                        else  //For Lead Insert
                        {
                            if (Request.QueryString["requesttypeP"] != null)
                            {

                            }
                            else // Lead Add
                            {
                                try
                                {
                                    if (!reCat.isAllMandetoryDone((DataTable)Session["UdfDataOnAdd"], Convert.ToString(hdKeyVal.Value)))
                                    {
                                        Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>InvalidUDF();</script>");
                                        return;
                                    }

                                    if (country == "")
                                    {
                                        country = "78";
                                    }
                                    DateTime dtDob, dtanniversary, dtReg, dtBusiness, dtenteredon;

                                    string dd = Convert.ToString(Session["requesttype"]);

                                    if (LDtxtDOB.Value != null)
                                    {
                                        dtDob = Convert.ToDateTime(LDtxtDOB.Value);
                                    }
                                    else
                                    {
                                        dtDob = Convert.ToDateTime("01-01-1900");
                                    }

                                    if (LDtxtAnniversary.Value != null)
                                    {
                                        dtanniversary = Convert.ToDateTime(LDtxtAnniversary.Value);
                                    }
                                    else
                                    {
                                        dtanniversary = Convert.ToDateTime("01-01-1900");
                                    }
                                    //Rev ----Subhra-----03-06-2019
                                    if (dt_EnteredOn.Value != null)
                                    {
                                        dtenteredon = Convert.ToDateTime(dt_EnteredOn.Value);
                                    }
                                    else
                                    {
                                        dtenteredon = Convert.ToDateTime("01-01-1900");
                                    }
                                    //End of Rev Subhra 03-06-2019
                                    if (txtDateRegis.Value != null)
                                    {
                                        dtReg = Convert.ToDateTime(txtDateRegis.Value);
                                    }
                                    else
                                    {
                                        dtReg = Convert.ToDateTime("01-01-1900");
                                    }


                                    if (LDtxtClentUcc.Text != "")
                                    {
                                        webLogin = "Yes";
                                        Password = txtClentUcc.Text;
                                    }
                                    else
                                    {
                                        webLogin = "No";
                                        Password = "";
                                    }


                                    string other = "";
                                    if (LDcmbProfession.SelectedValue == "20")
                                        other = txtotheroccu.Text.ToString().Trim();
                                    else
                                        other = "";

                                    if (LDtxtClentUcc.Text != "")
                                    {
                                        webLogin = "Yes";
                                        Password = LDtxtClentUcc.Text;
                                    }
                                    else
                                    {
                                        webLogin = "No";
                                        Password = "";
                                    }

                                    string vPlaceincop;

                                    if (txtincorporation.Text == "")
                                    {
                                        vPlaceincop = "";
                                        dtBusiness = Convert.ToDateTime("01-01-1900");
                                    }
                                    else
                                    {
                                        vPlaceincop = txtincorporation.Text.ToString().Trim();
                                        dtBusiness = Convert.ToDateTime(txtFromDate.Value);
                                    }

                                    //debjyoti 060217
                                    string GSTIN = "";
                                    // GSTIN = txtGSTIN1.Text.Trim() + txtGSTIN2.Text.Trim() + txtGSTIN3.Text.Trim();
                                    if (radioregistercheck.SelectedValue != "0")
                                    {
                                        GSTIN = txtGSTIN1.Text.Trim() + txtGSTIN2.Text.Trim() + txtGSTIN3.Text.Trim();
                                    }

                                    //rev srijeeta
                                    //string InternalId = oContactGeneralBL.Insert_ContactGeneral(dd, LDtxtClentUcc.Text.Trim(), LDCmbSalutation.SelectedItem.Value,
                                    //                    LDtxtFirstNmae.Text.Trim(), LDtxtMiddleName.Text.Trim(), LDtxtLastName.Text.Trim(),
                                    //                    LDtxtAliasName.Text.Trim(), LDcmbBranch.SelectedItem.Value, LDcmbGender.SelectedItem.Value,
                                    //                    LDcmbMaritalStatus.SelectedItem.Value, dtDob, dtanniversary, LDcmbLegalStatus.SelectedItem.Value,
                                    //                    LDcmbEducation.SelectedItem.Value, LDcmbProfession.SelectedItem.Value, LDtxtOrganization.Text.Trim(),
                                    //                    LDcmbJobResponsibility.SelectedItem.Value, LDcmbDesignation.SelectedItem.Value, LDcmbIndustry.SelectedItem.Value,
                                    //                    LDcmbSource.SelectedItem.Value, LDtxtReferedBy_hidden.Value, txtRPartner_hidden.Text.Trim(), ContType,
                                    //                    LDcmbContactStatus.SelectedItem.Value, dtReg, LDcmbRating.SelectedItem.Value, TxtContactStatus.Text.Trim(),
                                    //                    LDcmbBloodgroup.SelectedItem.Value, webLogin, Password, Convert.ToString(HttpContext.Current.Session["userid"]), vPlaceincop,
                                    //                    dtBusiness, other, country, Creditcard, Convert.ToInt32(txtcreditDays.Text.Trim()), Convert.ToDecimal(txtCreditLimit.Text.Trim()), Convert.ToString(cmbContactStatusclient.SelectedItem.Value)
                                    //                    , GSTIN, hidAssociatedEmp.Value, txtPname.Text
                                    //    );
                                    string InternalId = oContactGeneralBL.Insert_ContactGeneral(dd, LDtxtClentUcc.Text.Trim(), "",LDCmbSalutation.SelectedItem.Value,
                                                        LDtxtFirstNmae.Text.Trim(), LDtxtMiddleName.Text.Trim(), LDtxtLastName.Text.Trim(),
                                                        LDtxtAliasName.Text.Trim(), LDcmbBranch.SelectedItem.Value, LDcmbGender.SelectedItem.Value,
                                                        LDcmbMaritalStatus.SelectedItem.Value, dtDob, dtanniversary, LDcmbLegalStatus.SelectedItem.Value,
                                                        LDcmbEducation.SelectedItem.Value, LDcmbProfession.SelectedItem.Value, LDtxtOrganization.Text.Trim(),
                                                        LDcmbJobResponsibility.SelectedItem.Value, LDcmbDesignation.SelectedItem.Value, LDcmbIndustry.SelectedItem.Value,
                                                        LDcmbSource.SelectedItem.Value, LDtxtReferedBy_hidden.Value, txtRPartner_hidden.Text.Trim(), ContType,
                                                        LDcmbContactStatus.SelectedItem.Value, dtReg, LDcmbRating.SelectedItem.Value, TxtContactStatus.Text.Trim(),
                                                        LDcmbBloodgroup.SelectedItem.Value, webLogin, Password, Convert.ToString(HttpContext.Current.Session["userid"]), vPlaceincop,
                                                        dtBusiness, other, country, Creditcard, Convert.ToInt32(txtcreditDays.Text.Trim()), Convert.ToDecimal(txtCreditLimit.Text.Trim()), Convert.ToString(cmbContactStatusclient.SelectedItem.Value)
                                                        , GSTIN, hidAssociatedEmp.Value, txtPname.Text
                                        );
                                    //end of rev srijeeta


                                    //Rev Subhra 03-06-2019
                                    if (Convert.ToString(Session["requesttype"]) == "Lead")
                                    {
                                        Int32 rowsEffected = oDBEngine.SetFieldValue("tbl_master_contact", "EnteredDate='" + dtenteredon + "' ", " cnt_internalId='" + InternalId + "'");
                                    }
                                    //End of Rev 03-06-2019

                                    DataTable udfTable = (DataTable)Session["UdfDataOnAdd"];
                                    if (udfTable != null)
                                        Session["UdfDataOnAdd"] = reCat.insertRemarksCategoryAddMode(Convert.ToString(hdKeyVal.Value), InternalId, udfTable, Convert.ToString(Session["userid"]));

                                    //-----------------------------------Tier Structure End-----------------------------------------------------------------------

                                    HttpContext.Current.Session["KeyVal_InternalID"] = InternalId;

                                    UserLastSegment = Convert.ToString(HttpContext.Current.Session["userlastsegment"]);
                                    if (UserLastSegment != "1" && UserLastSegment != "4" && UserLastSegment != "6" && UserLastSegment != "9" && UserLastSegment != "10")
                                    {
                                        if (InternalId.Contains("CL"))
                                        {
                                            oDBEngine.InsurtFieldValue("tbl_master_contactexchange", "crg_cntid,crg_company,crg_exchange,crg_tcode,createdate,createuser,crg_sttpattern,crg_sttwap,crg_SegmentID", "'" + Convert.ToString(InternalId).Trim() + "','" + Convert.ToString(HttpContext.Current.Session["LastCompany"]).Trim() + "','" + Convert.ToString(segregis).Trim() + "','" + LDtxtClentUcc.Text.ToString().Trim() + "','" + Convert.ToString(oDBEngine.GetDate()) + "','" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "','D','W','" + Convert.ToString(HttpContext.Current.Session["usersegid"]).Trim() + "'");
                                        }
                                    }

                                    string popupScript = "";
                                    ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
                                    string[,] cnt_id = oDBEngine.GetFieldValue(" tbl_master_contact", " cnt_id", " cnt_internalId='" + InternalId + "'", 1);
                                    if (Convert.ToString(cnt_id[0, 0]) != "n")
                                    {
                                        Response.Redirect("Contact_general.aspx?id=" + Convert.ToString(cnt_id[0, 0]), false);
                                    }

                                }
                                catch
                                {

                                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('You have entered duplicate Unique ID.')</script>");
                                    txtClentUcc.Text = "";
                                    txtClentUcc.Focus();

                                }
                            }
                        }
                    }
                }
                else
                {
                    if (LDtxtAliasName.Text.Length > 0)
                    {
                        string today = Convert.ToString(oDBEngine.GetDate());
                        if (Convert.ToString(HttpContext.Current.Session["KeyVal"]) == "0")
                        {
                            DateTime dtDob, dtanniversary, dtReg, dtBusiness, dtenteredon;
                            if (!reCat.isAllMandetoryDone((DataTable)Session["UdfDataOnAdd"], Convert.ToString(hdKeyVal.Value)))
                            {
                                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>InvalidUDF();</script>");
                                return;
                            }
                            string dd = Convert.ToString(Session["requesttype"]);

                            if (LDtxtDOB.Value != null)
                            {
                                dtDob = Convert.ToDateTime(LDtxtDOB.Value);
                            }
                            else
                            {
                                dtDob = Convert.ToDateTime("01-01-1900");
                            }

                            if (LDtxtAnniversary.Value != null)
                            {
                                dtanniversary = Convert.ToDateTime(LDtxtAnniversary.Value);
                            }
                            else
                            {
                                dtanniversary = Convert.ToDateTime("01-01-1900");
                            }

                            //Rev ----Subhra-----03-06-2019
                            if (dt_EnteredOn.Value != null)
                            {
                                dtenteredon = Convert.ToDateTime(dt_EnteredOn.Value);
                            }
                            else
                            {
                                dtenteredon = Convert.ToDateTime("01-01-1900");
                            }
                            //End of Rev Subhra 03-06-2019

                            dtReg = Convert.ToDateTime("01-01-1900");



                            if (LDtxtClentUcc.Text != "")
                            {
                                webLogin = "Yes";
                                Password = LDtxtClentUcc.Text;
                            }
                            else
                            {
                                webLogin = "No";
                                Password = "";
                            }


                            string other = "";
                            if (LDcmbProfession.SelectedValue == "20")
                                other = "";
                            else
                                other = "";

                            if (LDtxtClentUcc.Text != "")
                            {
                                webLogin = "Yes";
                                Password = LDtxtClentUcc.Text;
                            }
                            else
                            {
                                webLogin = "No";
                                Password = "";
                            }

                            string vPlaceincop;


                            vPlaceincop = txtincorporation.Text.ToString().Trim();
                            dtBusiness = Convert.ToDateTime(txtFromDate.Value);


                            //debjyoti 060217
                            string GSTIN = "";
                            //  GSTIN = txtGSTIN1.Text.Trim() + txtGSTIN2.Text.Trim() + txtGSTIN3.Text.Trim();
                            if (radioregistercheck.SelectedValue != "0")
                            {
                                GSTIN = txtGSTIN1.Text.Trim() + txtGSTIN2.Text.Trim() + txtGSTIN3.Text.Trim();
                            }
                            //rev srijeeta
                            //string InternalId = oContactGeneralBL.Insert_ContactGeneral(dd, "", CmbSalutation.SelectedItem.Value,
                            //                               txtFirstNmae.Text.Trim(), txtMiddleName.Text.Trim(), txtLastName.Text.Trim(),
                            //                               txtAliasName.Text.Trim(), cmbBranch.SelectedItem.Value, cmbGender.SelectedItem.Value,
                            //                               cmbMaritalStatus.SelectedItem.Value, dtDob, dtanniversary, cmbLegalStatus.SelectedItem.Value,
                            //                               cmbEducation.SelectedItem.Value, cmbProfession.SelectedItem.Value, txtOrganization.Text.Trim(),
                            //                               cmbJobResponsibility.SelectedItem.Value, cmbDesignation.SelectedItem.Value, cmbIndustry.SelectedItem.Value,
                            //                               cmbSource.SelectedItem.Value, txtReferedBy.Text.Trim(), txtRPartner_hidden.Text.Trim(), ContType,
                            //                               cmbContactStatus.SelectedItem.Value, dtReg, cmbRating.SelectedItem.Value, TxtContactStatus.Text.Trim(),
                            //                               cmbBloodgroup.SelectedItem.Value, webLogin, Password, Convert.ToString(HttpContext.Current.Session["userid"]), vPlaceincop,
                            //                               dtBusiness, other, country, Creditcard, Convert.ToInt32(txtcreditDays.Text.Trim()), Convert.ToDecimal(txtCreditLimit.Text.Trim()), Convert.ToString(cmbContactStatusclient.SelectedItem.Value)
                            //                               , GSTIN, hidAssociatedEmp.Value, txtPname.Text
                            //               );
                            string InternalId = oContactGeneralBL.Insert_ContactGeneral(dd, "", "",CmbSalutation.SelectedItem.Value,
                                                           txtFirstNmae.Text.Trim(), txtMiddleName.Text.Trim(), txtLastName.Text.Trim(),
                                                           txtAliasName.Text.Trim(), cmbBranch.SelectedItem.Value, cmbGender.SelectedItem.Value,
                                                           cmbMaritalStatus.SelectedItem.Value, dtDob, dtanniversary, cmbLegalStatus.SelectedItem.Value,
                                                           cmbEducation.SelectedItem.Value, cmbProfession.SelectedItem.Value, txtOrganization.Text.Trim(),
                                                           cmbJobResponsibility.SelectedItem.Value, cmbDesignation.SelectedItem.Value, cmbIndustry.SelectedItem.Value,
                                                           cmbSource.SelectedItem.Value, txtReferedBy.Text.Trim(), txtRPartner_hidden.Text.Trim(), ContType,
                                                           cmbContactStatus.SelectedItem.Value, dtReg, cmbRating.SelectedItem.Value, TxtContactStatus.Text.Trim(),
                                                           cmbBloodgroup.SelectedItem.Value, webLogin, Password, Convert.ToString(HttpContext.Current.Session["userid"]), vPlaceincop,
                                                           dtBusiness, other, country, Creditcard, Convert.ToInt32(txtcreditDays.Text.Trim()), Convert.ToDecimal(txtCreditLimit.Text.Trim()), Convert.ToString(cmbContactStatusclient.SelectedItem.Value)
                                                           , GSTIN, hidAssociatedEmp.Value, txtPname.Text
                                           );
                            //end of rev srijeeta
                            ////end of rev srijeeta
                            //Rev Subhra 03-06-2019
                            if (Convert.ToString(Session["requesttype"]) == "Lead")
                            {
                                Int32 rowsEffected = oDBEngine.SetFieldValue("tbl_master_contact", "EnteredDate='" + dtenteredon + "' ", " cnt_internalId='" + InternalId + "'");
                            }
                            //End of Rev 03-06-2019

                            DataTable udfTable = (DataTable)Session["UdfDataOnAdd"];
                            if (udfTable != null)
                                Session["UdfDataOnAdd"] = reCat.insertRemarksCategoryAddMode(Convert.ToString(hdKeyVal.Value), InternalId, udfTable, Convert.ToString(Session["userid"]));
                            //----------------------------------For Tier Structure End-------------------------------------------------------------------------------


                            HttpContext.Current.Session["KeyVal_InternalID"] = InternalId;

                            UserLastSegment = Convert.ToString(HttpContext.Current.Session["userlastsegment"]);
                            if (UserLastSegment != "1" && UserLastSegment != "4" && UserLastSegment != "6" && UserLastSegment != "9" && UserLastSegment != "10")
                            {
                                if (InternalId.Contains("CL"))
                                {
                                    oDBEngine.InsurtFieldValue("tbl_master_contactexchange", "crg_cntid,crg_company,crg_exchange,crg_tcode,createdate,createuser,crg_sttpattern,crg_sttwap,crg_SegmentID", "'" + Convert.ToString(InternalId).Trim() + "','" + Convert.ToString(HttpContext.Current.Session["LastCompany"]).Trim() + "','" + Convert.ToString(segregis).Trim() + "','" + LDtxtClentUcc.Text.ToString().Trim() + "','" + oDBEngine.GetDate().ToString() + "','" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "','D','W','" + Convert.ToString(HttpContext.Current.Session["usersegid"]).Trim() + "'");
                                }
                            }

                            string popupScript = "";
                            Page.ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
                            string[,] cnt_id = oDBEngine.GetFieldValue(" tbl_master_contact", " cnt_id", " cnt_internalId='" + InternalId + "'", 1);
                            if (Convert.ToString(cnt_id[0, 0]) != "n")
                            {
                                Response.Redirect("Contact_general.aspx?id=" + Convert.ToString(cnt_id[0, 0]), false);
                            }
                        }
                        else
                        {

                            webLogin = "No";

                            if (LDcmbSource.SelectedItem.Value == "0")
                            {
                                LDtxtReferedBy_hidden.Value = null;
                            }

                            if (LDcmbSource.SelectedItem.Value == "18")
                            {
                                String value = "cnt_ucc='" + LDtxtClentUcc.Text + "', cnt_salutation=" + LDCmbSalutation.SelectedItem.Value + ",  cnt_firstName='" + LDtxtFirstNmae.Text + "', cnt_middleName='" + LDtxtMiddleName.Text + "', cnt_lastName='" + LDtxtLastName.Text + "', cnt_shortName='" + LDtxtAliasName.Text + "',cnt_PrintNameToCheque='" + txtPname.Text + "', cnt_branchId=" + LDcmbBranch.SelectedItem.Value + ", cnt_sex=" + LDcmbGender.SelectedItem.Value + ", cnt_maritalStatus=" + LDcmbMaritalStatus.SelectedItem.Value + ", cnt_DOB='" + LDtxtDOB.Value + "', cnt_anniversaryDate='" + LDtxtAnniversary.Value + "', cnt_legalStatus=" + LDcmbLegalStatus.SelectedItem.Value + ", cnt_education=" + LDcmbEducation.SelectedItem.Value + ", cnt_profession=" + LDcmbProfession.SelectedItem.Value + ", cnt_organization='" + LDtxtOrganization.Text + "', cnt_jobResponsibility=" + LDcmbJobResponsibility.SelectedItem.Value + ", cnt_designation=" + LDcmbDesignation.SelectedItem.Value + ", cnt_industry=" + LDcmbIndustry.SelectedItem.Value + ", cnt_contactSource=" + LDcmbSource.SelectedItem.Value + ", cnt_referedBy='" + LDtxtReferedBy_hidden.Value + "', cnt_contactType='" + ContType + "', cnt_contactStatus=" + LDcmbContactStatus.SelectedItem.Value + ",cnt_RegistrationDate='',cnt_rating='" + LDcmbRating.SelectedItem.Value + "',cnt_reason='',cnt_bloodgroup='" + LDcmbBloodgroup.SelectedItem.Value + "',WebLogIn='" + webLogin + "',PassWord='" + Password + "', lastModifyDate ='" + today + "', lastModifyUser=" + HttpContext.Current.Session["userid"]; // + Session["userid"]
                                oDBEngine.SetFieldValue("tbl_master_contact", value, " cnt_id=" + HttpContext.Current.Session["KeyVal"]);
                            }
                            else
                            {
                                String value = "Statustype='" + status + "',cnt_ucc='" + txtClentUcc.Text + "', cnt_salutation=" + LDCmbSalutation.SelectedItem.Value + ",  cnt_firstName='" + LDtxtFirstNmae.Text + "', cnt_middleName='" + LDtxtMiddleName.Text + "', cnt_lastName='" + LDtxtLastName.Text + "', cnt_shortName='" + LDtxtAliasName.Text + "', cnt_PrintNameToCheque='" + txtPname.Text + "',cnt_branchId=" + LDcmbBranch.SelectedItem.Value + ", cnt_sex=" + LDcmbGender.SelectedItem.Value + ", cnt_maritalStatus=" + LDcmbMaritalStatus.SelectedItem.Value + ", cnt_DOB='" + LDtxtDOB.Value + "', cnt_anniversaryDate='" + LDtxtAnniversary.Value + "', cnt_legalStatus=" + LDcmbLegalStatus.SelectedItem.Value + ", cnt_education=" + LDcmbEducation.SelectedItem.Value + ", cnt_profession=" + LDcmbProfession.SelectedItem.Value + ", cnt_organization='" + LDtxtOrganization.Text + "', cnt_jobResponsibility=" + LDcmbJobResponsibility.SelectedItem.Value + ", cnt_designation=" + LDcmbDesignation.SelectedItem.Value + ", cnt_industry=" + LDcmbIndustry.SelectedItem.Value + ", cnt_contactSource=" + LDcmbSource.SelectedItem.Value + ", cnt_referedBy='" + LDtxtReferedBy_hidden.Value + "', cnt_contactType='" + ContType + "', cnt_contactStatus=" + LDcmbContactStatus.SelectedItem.Value + ",cnt_RegistrationDate='',cnt_rating='" + LDcmbRating.SelectedItem.Value + "',cnt_reason='',cnt_bloodgroup='" + LDcmbBloodgroup.SelectedItem.Value + "',WebLogIn='" + webLogin + "',PassWord='" + Password + "', lastModifyDate ='" + today + "', lastModifyUser=" + HttpContext.Current.Session["userid"]; // + Session["userid"]

                                oDBEngine.SetFieldValue("tbl_master_contact", value, " cnt_id=" + HttpContext.Current.Session["KeyVal"]);

                                Page.ClientScript.RegisterStartupScript(GetType(), "JScript3", "<script language='javascript'>PageLoad();</script>");
                                Response.Redirect("Contact_general.aspx?id=" + HttpContext.Current.Session["KeyVal"], false);

                                return;
                            }

                            //Rev Subhra 03-06-2019
                            if (Convert.ToString(Session["requesttype"]) == "Lead")
                            {
                                oDBEngine.SetFieldValue("tbl_master_contact", "EnteredDate='" + dt_EnteredOn.Value + "' ", " cnt_id=" + HttpContext.Current.Session["KeyVal"]);
                            }
                            //End of Rev 03-06-2019

                        }
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Please Insert Unique ID')</script>");
                        Response.Redirect("Contact_general.aspx?id=" + HttpContext.Current.Session["KeyVal"], false);
                    }
                }

                if (Request.QueryString["formtype"] != null)
                {
                    if (Convert.ToString(Request.QueryString["formtype"]) == "leadSales")
                    {
                        string popupScript = "";
                        popupScript = "<script language='javascript'>" + "window.opener.location.href=window.opener.location.href;window.close();</script>";
                        ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
                    }
                    else if (Convert.ToString(Request.QueryString["formtype"]) == "lead")
                    {
                        string popupScript = "";
                        popupScript = "<script language='javascript'>" + "parent.editwin.close();</script>";
                        ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
                    }
                    else if (Convert.ToString(Request.QueryString["formtype"]) == "lead123")
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
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["formtype"] != null)
            {
                //string popupScript = "";
                //popupScript = "<script language='javascript'>" + "window.close();</script>";
                //ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript4", "<script>parent.editwin.close();</script>");
            }
            else
            {
                //string popupScript = "";
                //popupScript = "<script language='javascript'>" + "window.close();</script>";
                //ClientScript.RegisterStartupScript(GetType(), "PopupScript", popupScript);
                //Page.ClientScript.RegisterStartupScript(GetType(), "JScript4567", "<script language='javascript'>window.close;</script>");
                //Response.Write("<script language=javascript> window.close(); </script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript2", "<script>parent.editwin.close();</script>");
                //Response.End();
                // Response.Redirect("frmContactMain.aspx?id=" +"customer"+"|" + HttpContext.Current.Session["KeyVal_InternalID"], false);
            }
        }

        public void LanGuage()
        {
            string InternalId = Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]);//"EMA0000003";
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
                    spk += Id + " | ";
                }
                int spklng = spk.LastIndexOf('|');
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
                    wrt += Id + " | ";
                }
                int wrtlng = wrt.LastIndexOf('|');
                wrt = wrt.Substring(0, wrtlng);
                LitWrittenLanguage.Text = wrt;
            }
        }


        protected void ASPxPageControl1_ActiveTabChanged(object source, TabControlEventArgs e)
        {
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            if (txtClentUcc.Text.ToString() != "")
            {

                string prefx = txtClentUcc.Text.ToString();

                string InternalID = oContactGeneralBL.Get_UCCCode(prefx);



                if (InternalID != "")
                {
                    txtClentUcc.Text = InternalID;
                }
                else
                {
                    lblErr.Text = "</br>No UCC found..Type another UCC.";
                    lblErr.Visible = true;
                }
            }
            else
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct9", "popup();", true);
            }
        }

        [WebMethod]
        public static List<string> ALLEmployee(string reqStr)
        {
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = new DataTable();
            DT.Rows.Clear();
            DT = oDBEngine.GetDataTable("tbl_master_employee, tbl_master_contact,tbl_trans_employeeCTC ", "ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') +'['+cnt_shortName+']' AS Name, cnt_internalId as Id    ", " tbl_master_employee.emp_contactId = tbl_trans_employeeCTC.emp_cntId and tbl_trans_employeeCTC.emp_cntId = tbl_master_contact.cnt_internalId    and cnt_contactType='EM' and (emp_dateofLeaving is null or emp_dateofLeaving='1/1/1900 12:00:00 AM' OR emp_dateofLeaving>getdate()) and (cnt_firstName Like '" + reqStr + "%' or cnt_shortName like '" + reqStr + "%')");

            List<string> obj = new List<string>();
            foreach (DataRow dr in DT.Rows)
            {

                obj.Add(Convert.ToString(dr["Name"]) + "|" + Convert.ToString(dr["Id"]));
            }

            return obj;
        }

        [WebMethod]
        public static List<string> EmployeeName(string Empcode)
        {
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]); MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = new DataTable();
            DT.Rows.Clear();
            DT = oDBEngine.GetDataTable("tbl_master_contact", "ISNULL(cnt_firstName, '') + REPLACE(' ' + ISNULL(cnt_middleName, '') + ' ','  ',' ') + ISNULL(cnt_lastName, '') AS Name, cnt_shortName as ShortCode    ", " cnt_contactType='EM' and cnt_InternalId='" + Empcode + "'");

            List<string> obj = new List<string>();
            foreach (DataRow dr in DT.Rows)
            {

                obj.Add(Convert.ToString(dr["Name"]) + "|" + Convert.ToString(dr["ShortCode"]));
            }

            return obj;
        }


        [WebMethod]
        public static string CheckUniqueName(string clientName, string procode)
        {
            MShortNameCheckingBL mshort = new MShortNameCheckingBL();
            bool IsPresent = false;
            string ContType = "";
            if (HttpContext.Current.Session["requesttype"] != null)
            {
                switch (Convert.ToString(HttpContext.Current.Session["requesttype"]))
                {
                    case "Customer/Client":
                        ContType = "CL";
                        break;
                    case "OtherEntity":
                        ContType = "XC";
                        break;
                    case "Sub Broker":
                        ContType = "SB";
                        break;
                    case "Franchisee":
                        ContType = "FR";
                        break;
                    case "Relationship Partners"://added 's' by sanjib due to mismatch
                        ContType = "RA";
                        break;
                    case "Broker":
                        ContType = "BO";
                        break;
                    case "Relationship Manager":
                        ContType = "RC";
                        break;
                    case "Data Vendor":
                        ContType = "DV";
                        break;
                    case "Vendor":
                        ContType = "VR";
                        break;
                    case "Partner":
                        ContType = "PR";
                        break;
                    case "Consultant":
                        ContType = "CS";
                        break;
                    case "Share Holder":
                        ContType = "SH";
                        break;
                    case "Creditors":
                        ContType = "CR";
                        break;
                    case "Debtor":
                        ContType = "DR";
                        break;
                    case "Lead":
                        ContType = "LD";
                        break;
                    case "Transporter":
                        ContType = "TR";
                        break;
                }
            }
            string entityName = "";
            procode = Convert.ToString(HttpContext.Current.Session["KeyVal"]);
            if (procode == "0")
            {
                IsPresent = mshort.CheckUniqueWithtypeContactMaster(clientName, procode, "MasterContactType", ContType, ref entityName);
            }
            else
            {
                IsPresent = mshort.CheckUniqueWithtypeContactMaster(clientName, procode, "Mastercustomerclient", ContType, ref entityName);
            }


            return Convert.ToString(IsPresent) + "~" + entityName;
        }




        #region Leads
        public void LD_DDLBind() // Lead add/Edit
        {
            string[,] Data = oDBEngine.GetFieldValue("tbl_master_salutation", "sal_id, sal_name", null, 2, "sal_name");

            oclsDropDownList.AddDataToDropDownList(Data, LDCmbSalutation);
            Data = oDBEngine.GetFieldValue("tbl_master_education", "edu_id, edu_education", null, 2, "edu_education");

            oclsDropDownList.AddDataToDropDownList(Data, LDcmbEducation);
            Data = oDBEngine.GetFieldValue("tbl_master_profession", "pro_id, pro_professionName", null, 2, "pro_professionName");

            oclsDropDownList.AddDataToDropDownList(Data, LDcmbProfession);
            Data = oDBEngine.GetFieldValue("tbl_master_jobresponsibility", "job_id, job_responsibility", null, 2, "job_responsibility");

            oclsDropDownList.AddDataToDropDownList(Data, LDcmbJobResponsibility);
            Data = oDBEngine.GetFieldValue("tbl_master_Designation", "deg_id, deg_designation", null, 2, "deg_designation");

            oclsDropDownList.AddDataToDropDownList(Data, LDcmbDesignation);
            Data = oDBEngine.GetFieldValue("tbl_master_branch", "branch_id, branch_description ", null, 2, "branch_description");

            oclsDropDownList.AddDataToDropDownList(Data, LDcmbBranch);
            Data = oDBEngine.GetFieldValue("tbl_master_industry", "ind_id, ind_industry  ", null, 2, "ind_industry");

            oclsDropDownList.AddDataToDropDownList(Data, LDcmbIndustry);
            Data = oDBEngine.GetFieldValue(" tbl_master_ContactSource", "cntsrc_id, cntsrc_sourcetype", null, 2, "cntsrc_sourcetype");

            oclsDropDownList.AddDataToDropDownList(Data, LDcmbSource);
            Data = oDBEngine.GetFieldValue("tbl_master_leadRating", " rat_id, rat_LeadRating  ", null, 2, "rat_LeadRating");

            oclsDropDownList.AddDataToDropDownList(Data, LDcmbRating);
            Data = oDBEngine.GetFieldValue(" tbl_master_maritalstatus", " mts_id, mts_maritalStatus", null, 2, "mts_maritalStatus");

            oclsDropDownList.AddDataToDropDownList(Data, LDcmbMaritalStatus);
            Data = oDBEngine.GetFieldValue(" tbl_master_contactstatus", "cntstu_id, cntstu_contactStatus", null, 2, "cntstu_contactStatus");

            oclsDropDownList.AddDataToDropDownList(Data, LDcmbContactStatus);


            Data = oDBEngine.GetFieldValue("tbl_master_legalstatus", "lgl_id, lgl_legalStatus", null, 2, "lgl_legalStatus");

            oclsDropDownList.AddDataToDropDownList(Data, LDcmbLegalStatus);
            LDCmbSalutation.SelectedValue = "1";
            LDcmbRating.SelectedValue = "1";

            // Code  Added  By Sam on 15112016 to Add Select as optional Data

            LDCmbSalutation.Items.Insert(0, new ListItem("--Select--", "0"));
            LDcmbLegalStatus.Items.Insert(0, new ListItem("--Select--", "0"));
            LDcmbEducation.Items.Insert(0, new ListItem("--Select--", "0"));
            LDcmbProfession.Items.Insert(0, new ListItem("--Select--", "0"));
            LDcmbJobResponsibility.Items.Insert(0, new ListItem("--Select--", "0"));
            LDcmbDesignation.Items.Insert(0, new ListItem("--Select--", "0"));
            LDcmbIndustry.Items.Insert(0, new ListItem("--Select--", "0"));
            LDcmbSource.Items.Insert(0, new ListItem("--Select--", "0"));
            //LDcmbContactStatus.Items.Insert(0, new ListItem("--Select--", "0"));
            LDcmbBloodgroup.Items.Insert(0, new ListItem("--Select--", "0"));
            // Code Above Added and Commented By Sam on 15112016 to  
            LDcmbContactStatus.SelectedValue = "5";


        }
        public void LD_ValueAllocation(string[,] ContactData) // lead edit
        {
            LDtxtClentUcc.Text = ContactData[0, 0];
            string[,] Data = oDBEngine.GetFieldValue("tbl_master_salutation", "sal_id, sal_name", null, 2, "sal_name");
            if (ContactData[0, 1] != "")
            {
                oclsDropDownList.AddDataToDropDownList(Data, LDCmbSalutation, Int32.Parse(ContactData[0, 1]));
            }
            else
            {

                oclsDropDownList.AddDataToDropDownList(Data, LDCmbSalutation, 0);
            }
            LDtxtFirstNmae.Text = ContactData[0, 2];
            LDtxtMiddleName.Text = ContactData[0, 3];
            LDtxtLastName.Text = ContactData[0, 4];
            LDtxtAliasName.Text = ContactData[0, 5];
            LDcmbBloodgroup.SelectedValue = ContactData[0, 25];

            hidAssociatedEmp.Value = ContactData[0, 25];

            Data = oDBEngine.GetFieldValue("tbl_master_branch", "branch_id, branch_description ", null, 2, "branch_description");
            if (ContactData[0, 6] != "")
            {
                oclsDropDownList.AddDataToDropDownList(Data, LDcmbBranch, Int32.Parse(ContactData[0, 6]));
            }
            else
            {
                oclsDropDownList.AddDataToDropDownList(Data, LDcmbBranch, 0);
            }
            if (ContactData[0, 7] != "")
            {
                LDcmbGender.SelectedValue = ContactData[0, 7];
            }
            else
            {
                LDcmbGender.SelectedIndex.Equals(0);
            }
            Data = oDBEngine.GetFieldValue(" tbl_master_maritalstatus", " mts_id, mts_maritalStatus", null, 2, "mts_maritalStatus");

            if (ContactData[0, 8] != "")
            {
                oclsDropDownList.AddDataToDropDownList(Data, LDcmbMaritalStatus, Int32.Parse(ContactData[0, 8]));
            }
            else
            {
                oclsDropDownList.AddDataToDropDownList(Data, LDcmbMaritalStatus, 0);
            }

            if (ContactData[0, 9] != "")
            {
                LDtxtDOB.Value = Convert.ToDateTime(ContactData[0, 9]);
            }

            if (ContactData[0, 10] != "")
            {
                LDtxtAnniversary.Value = Convert.ToDateTime(ContactData[0, 10]);
            }
            //Rev----Subhra------03-06-2019-------
            if (ContactData[0, 35] != "")
            {
                dt_EnteredOn.Value = Convert.ToDateTime(ContactData[0, 35]);
            }
            //End of Rev----------Subhra------03-06-2019-------
            Data = oDBEngine.GetFieldValue("tbl_master_legalstatus", "lgl_id, lgl_legalStatus", null, 2, "lgl_legalStatus");

            if (ContactData[0, 11] != "")
            {
                oclsDropDownList.AddDataToDropDownList(Data, LDcmbLegalStatus, Int32.Parse(ContactData[0, 11]));
            }
            else
            {
                oclsDropDownList.AddDataToDropDownList(Data, LDcmbLegalStatus, 0);
            }

            Data = oDBEngine.GetFieldValue("tbl_master_education", "edu_id, edu_education", null, 2, "edu_education");

            if (ContactData[0, 12] != "")
            {
                oclsDropDownList.AddDataToDropDownList(Data, LDcmbEducation, Int32.Parse(ContactData[0, 12]));
            }
            else
            {
                oclsDropDownList.AddDataToDropDownList(Data, LDcmbEducation, 0);
            }

            Data = oDBEngine.GetFieldValue("tbl_master_profession", "pro_id, pro_professionName", null, 2, "pro_professionName");

            if (ContactData[0, 13] != "")
            {
                oclsDropDownList.AddDataToDropDownList(Data, LDcmbProfession, Int32.Parse(ContactData[0, 13]));

            }
            else
            {
                oclsDropDownList.AddDataToDropDownList(Data, LDcmbProfession, 0);
            }


            LDtxtOrganization.Text = ContactData[0, 14];
            Data = oDBEngine.GetFieldValue("tbl_master_jobresponsibility", "job_id, job_responsibility", null, 2, "job_responsibility");

            if (ContactData[0, 15] != "")
            {
                oclsDropDownList.AddDataToDropDownList(Data, LDcmbJobResponsibility, Int32.Parse(ContactData[0, 15]));
            }
            else
            {
                oclsDropDownList.AddDataToDropDownList(Data, LDcmbJobResponsibility, 0);
            }


            Data = oDBEngine.GetFieldValue("tbl_master_Designation", "deg_id, deg_designation", null, 2, "deg_designation");

            if (ContactData[0, 16] != "")
            {
                oclsDropDownList.AddDataToDropDownList(Data, LDcmbDesignation, Int32.Parse(ContactData[0, 16]));
            }
            else
            {
                oclsDropDownList.AddDataToDropDownList(Data, LDcmbDesignation, 0);
            }


            Data = oDBEngine.GetFieldValue("tbl_master_industry", "ind_id, ind_industry  ", null, 2, "ind_industry");

            if (ContactData[0, 17] != "")
            {
                oclsDropDownList.AddDataToDropDownList(Data, LDcmbIndustry, Int32.Parse(ContactData[0, 17]));
            }
            else
            {
                oclsDropDownList.AddDataToDropDownList(Data, LDcmbIndustry, 0);
            }



            Data = oDBEngine.GetFieldValue(" tbl_master_ContactSource", "cntsrc_id, cntsrc_sourcetype", null, 2, "cntsrc_sourcetype");

            if (ContactData[0, 18] != "")
            {
                oclsDropDownList.AddDataToDropDownList(Data, LDcmbSource, Int32.Parse(ContactData[0, 18]));
            }
            else
            {
                oclsDropDownList.AddDataToDropDownList(Data, LDcmbSource, 0);
            }
            // Code  Added  By Sam on 17112016 to set defaultvalue if no record found
            LDcmbLegalStatus.Items.Insert(0, new ListItem("--Select--", "0"));
            LDcmbEducation.Items.Insert(0, new ListItem("--Select--", "0"));
            LDcmbProfession.Items.Insert(0, new ListItem("--Select--", "0"));
            LDcmbJobResponsibility.Items.Insert(0, new ListItem("--Select--", "0"));
            LDcmbDesignation.Items.Insert(0, new ListItem("--Select--", "0"));
            LDcmbIndustry.Items.Insert(0, new ListItem("--Select--", "0"));
            LDcmbSource.Items.Insert(0, new ListItem("--Select--", "0"));
            if (ContactData[0, 11] == "0")
            {

                LDcmbLegalStatus.SelectedValue = "0";
            }

            if (ContactData[0, 12] == "0")
            {

                LDcmbEducation.SelectedValue = "0";
            }

            if (ContactData[0, 13] == "0")
            {

                LDcmbProfession.SelectedValue = "0";
            }

            if (ContactData[0, 15] == "0")
            {

                LDcmbJobResponsibility.SelectedValue = "0";
            }

            if (ContactData[0, 16] == "0")
            {

                LDcmbDesignation.SelectedValue = "0";
            }

            if (ContactData[0, 17] == "0")
            {

                LDcmbIndustry.SelectedValue = "0";
            }

            if (ContactData[0, 18] == "0")
            {

                LDcmbSource.SelectedValue = "0";
            }

            // Code Above Added and Commented By Sam on 17112016 to  

            //document.getElementById("td_lAnniversary").style.display = 'none';
            //       document.getElementById("td_tAnniversary").style.display = 'none';
            //       document.getElementById("td_lGender").style.display = 'none';
            //       document.getElementById("td_dGender").style.display = 'none';
            //       document.getElementById("td_lMarital").style.display = 'none';
            //       document.getElementById("td_dMarital").style.display = 'none';



            //changed refby fill data due to blank data was coming so i have set by session

            LDtxtReferedBy_hidden.Value = Convert.ToString(ContactData[0, 19]);
            if (!string.IsNullOrEmpty(Convert.ToString(ContactData[0, 19])))
            {
                Data = oDBEngine.GetFieldValue(" tbl_master_contact", " (ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') + ' [' + cnt_shortName +']') AS cnt_firstName ", " cnt_internalid='" + ContactData[0, 19].ToString() + "'", 1);
                if (Data[0, 0] != "n")
                {
                    // LDtxtReferedBy_hidden.Value = Convert.ToString(ContactData[0, 19]);

                    lstconverttounit.SelectedValue = Convert.ToString(ContactData[0, 19]);
                    //LDtxtReferedBy.Text = Data[0, 0]; //comment by sanjib due to changed textbox to choosen
                }
                else
                {
                    //LDtxtReferedBy.Text = ""; //comment by sanjib due to changed textbox to choosen
                    lstconverttounit.SelectedIndex = -1;
                    LDtxtReferedBy_hidden.Value = "0";
                }
            }
            else
            {
                string Internal_ID = Convert.ToString(Session["InternalId"]);
                LDtxtReferedBy_hidden.Value = Internal_ID;
                Data = oDBEngine.GetFieldValue(" tbl_master_contact", " (ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') + ' [' + cnt_shortName +']') AS cnt_firstName ", " cnt_internalid='" + Internal_ID + "'", 1);
                if (Data[0, 0] != "n")
                {
                    //LDtxtReferedBy_hidden.Value = Internal_ID;

                    lstconverttounit.SelectedValue = Convert.ToString(ContactData[0, 19]);
                    //LDtxtReferedBy.Text = Data[0, 0]; //comment by sanjib due to changed textbox to choosen
                }
                else
                {
                    lstconverttounit.SelectedIndex = -1;
                    LDtxtReferedBy_hidden.Value = "0";
                }


            }



            Data = oDBEngine.GetFieldValue(" tbl_master_contactstatus", "cntstu_id, cntstu_contactStatus", null, 2, "cntstu_contactStatus");

            if (ContactData[0, 21] != "")
            {
                oclsDropDownList.AddDataToDropDownList(Data, LDcmbContactStatus, Int32.Parse(ContactData[0, 21]));
            }
            else
            {
                oclsDropDownList.AddDataToDropDownList(Data, LDcmbContactStatus, 0);
            }
            Data = oDBEngine.GetFieldValue("tbl_master_leadRating", " rat_id, rat_LeadRating  ", null, 2, "rat_LeadRating");

            oclsDropDownList.AddDataToDropDownList(Data, LDcmbRating);
            if (ContactData[0, 23] != "")
            {
                oclsDropDownList.AddDataToDropDownList(Data, LDcmbRating, Int32.Parse(ContactData[0, 23]));
            }
            else
            {
                oclsDropDownList.AddDataToDropDownList(Data, LDcmbRating, 0);
            }
            Session["Name"] = LDtxtFirstNmae.Text + " " + LDtxtMiddleName.Text + " " + LDtxtLastName.Text + " [" + LDtxtClentUcc.Text + "]";



        }



        #endregion

        [WebMethod]
        public static List<string> GetMainAccountList(string reqStr)
        {
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = new DataTable();
            DT.Rows.Clear();
            // DT = oDBEngine.GetDataTable("Master_MainAccount", "MainAccount_Name,MainAccount_AccountCode ", " MainAccount_Name like '" + reqStr + "%'");

            // DT = oDBEngine.GetDataTable("Master_MainAccount", "MainAccount_Name, MainAccount_AccountCode ", " MainAccount_AccountCode not like 'SYS%'");
            ProcedureExecute proc = new ProcedureExecute("prc_ProductMaster_bindData");
            proc.AddVarcharPara("@action", 20, "GetMainAccount");
            DT = proc.GetTable();

            List<string> obj = new List<string>();
            foreach (DataRow dr in DT.Rows)
            {

                obj.Add(Convert.ToString(dr["MainAccount_Name"]) + "|" + Convert.ToString(dr["MainAccount_AccountCode"]));
            }
            return obj;
        }


        [WebMethod]
        public static string CheckContactStatus(string Cid)
        {
            string isactive = "0";
            if (Cid == "A")
            {
                //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable dtaddress = oDBEngine.GetDataTable("tbl_master_address", "add_cntId", "add_cntId='" + Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]) + "' and add_entity='Customer/Client' and add_addresstype='Billing' and isnull(add_address1,'')<>'' and isnull(add_phone,'')<>'' and isnull(add_country,'')<>'' and isnull(add_state,'')<>'' and isnull(add_city,'')<>'' and isnull(add_pin,'')<>'' ");
                if (dtaddress.Rows.Count <= 0)
                {
                    isactive = "1";
                }
            }

            return isactive;

        }



        [WebMethod]
        public static List<string> GetrefBy(string query)
        {

            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = new DataTable();
            DT.Rows.Clear();
            DT = oDBEngine.GetDataTable(query);
            List<string> obj = new List<string>();
            foreach (DataRow dr in DT.Rows)
            {

                obj.Add(Convert.ToString(dr["cnt_firstName"]) + "|" + Convert.ToString(dr["cnt_internalid"]));
            }
            return obj;

        }


        public static bool CheckUniqueCode(string customerid, string mode, string gstin)
        {
            bool flag = false;
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            ContactGeneralBL objContactGeneralBL = new ContactGeneralBL();
            try
            {

                DataTable dt = new DataTable();
                dt = objContactGeneralBL.CheckUniqueGSTIN(customerid, Convert.ToInt32(mode), gstin);

                //if (mode == "0")
                //{
                //    dt = oGenericMethod.GetDataTable("SELECT * FROM [dbo].[Master_Color] WHERE [Color_Code] = " + "'" + customerid + "'");
                //}
                //else
                //{
                //    dt = oGenericMethod.GetDataTable("SELECT * FROM [dbo].[Master_Color] WHERE [Color_Code] = " + "'" + customerid + "' and Color_ID<>'" + mode + "'");
                //}
                int cnt = 0;
                if (dt != null && dt.Rows.Count > 0)
                {
                    cnt = dt.Rows.Count;
                }


                if (cnt > 0)
                {
                    flag = true;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
            }
            return flag;
        }


        [WebMethod]
        public static object GetDuplicateGSTIN(string GSTIN)
        {
            string stat = "0";
            bool res = ContactGeneralBL.ISUniqueGSTIN("", "0", GSTIN, "CL");
            if (res == true)
                stat = "1";

            var storiesObj1 = new { status = stat };
            return storiesObj1;
        }
    }
}