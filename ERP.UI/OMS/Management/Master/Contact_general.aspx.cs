/***********************************************************************************************************
 * 1.0    02-01-2024       V2.0.42     Sanchita     Settings required to Check Duplicate Customer Master Name. Mantis: 27125
 **************************************************************************************************************/ 
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
using System.Web.Script.Services;
using Newtonsoft.Json;
using System.Net.Http;



//using System.Collections.Generic;
namespace ERP.OMS.Management.Master
{
    public partial class management_Master_Contact_general : ERP.OMS.ViewState_class.VSPage
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
            GstinSettingsButton.contact_type = "CUST";
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

            CommonBL ComBL = new CommonBL();
            //string AutoNumberAllow = ComBL.GetSystemSettingsResult("UniqueAutoNumberMaster");
            string UniqueAutoNumberCustomerMaster = ComBL.GetSystemSettingsResult("UniqueAutoNumberCustomerMaster");
            string UniqueAutoNumberLeadMaster = ComBL.GetSystemSettingsResult("UniqueAutoNumberLeadMaster");
            string UniqueAutoNumberSalesmanMaster = ComBL.GetSystemSettingsResult("UniqueAutoNumberSalesmanMaster");
            string UniqueAutoNumberTransporterMaster = ComBL.GetSystemSettingsResult("UniqueAutoNumberTransporterMaster");
            string UniqueAutoNumberInfluencerMaster = ComBL.GetSystemSettingsResult("UniqueAutoNumberInfluencerMaster");

            //Add section for Transcation Category Tanmoy
            MasterSettings objmaster = new MasterSettings();

            hdnDocumentSegmentSettings.Value = objmaster.GetSettings("DocumentSegment");
            hdnActiveEInvoice.Value = objmaster.GetSettings("ActiveEInvoice");

            if (hdnActiveEInvoice.Value == "0")
            {
                DivTransactionCategory.Style.Add("display", "none");
            }
            else
            {
                DivTransactionCategory.Style.Add("display", "block");
            }


            if (hdnDocumentSegmentSettings.Value == "0")
            {
                PanelDocumentSegment.Style.Add("display", "none");
                DivServiceBranch.Style.Add("display", "none");
            }
            else
            {
                PanelDocumentSegment.Style.Add("display", "block");
                DivServiceBranch.Style.Add("display", "block");
            }


            //Add section for Transcation Category Tanmoy

            Session["InsertMode"] = null;
            //hddnDocNo.Value = "";
            //hdnAutoNumStg.Value = "";
            //hdnTransactionType.Value = "";
            //hdnNumberingId.Value = "";

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
                        divTDSDeductee.Style.Add("display", "block");
                        pnlVehicleNo.Style.Add("display", "none");
                        td_EnrollmentId.Style.Add("display", "none");
                        td_registered.Style.Add("display", "none");
                        dvNameAsPerPan.Style.Add("display", "block");
                        dvDeducteestat.Style.Add("display", "block");
                        PanelDocumentSegment.Style.Add("display", "none");
                        DivServiceBranch.Style.Add("display", "none");



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
                        divTDSDeductee.Style.Add("display", "none");

                        divSelectEmployee.Style.Add("display", "none");
                        pnlVehicleNo.Style.Add("display", "none");
                        td_EnrollmentId.Style.Add("display", "none");
                        PanelDocumentSegment.Style.Add("display", "none");
                        DivServiceBranch.Style.Add("display", "none");
                        break;
                    case "Customer/Client":
                        this.Title = "Customer/Client";
                        pnlCredit.Style.Add("display", "block");
                        cmbBranch.Style.Add("Display", "none");
                        divClientBranch.Style.Add("Display", "none");
                        divSelectEmployee.Style.Add("display", "none");
                        pnlVehicleNo.Style.Add("display", "none");
                        td_registered.Style.Add("display", "block");
                        divTDSDeductee.Style.Add("display", "none");
                        dvTCSApplicable.Style.Add("display", "block");
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
                        dvTaxDeducteeType.Style.Add("display", "block");
                        //divSelectEmployee.Style.Add("display", "none");
                        //pnlVehicleNo.Style.Add("display", "none");
                        //td_registered.Style.Add("display", "none");

                        //...end...
                        if (hdnDocumentSegmentSettings.Value == "0")
                        {
                            PanelDocumentSegment.Style.Add("display", "none");
                            DivServiceBranch.Style.Add("display", "none");
                        }
                        else
                        {
                            PanelDocumentSegment.Style.Add("display", "block");
                            DivServiceBranch.Style.Add("display", "block");
                        }

                        break;
                    case "Debtor":
                        this.Title = "Debtor";
                        lblCreditcard.Visible = true;
                        ChkCreditcard.Visible = true;
                        lblcreditDays.Visible = true;
                        txtcreditDays.Visible = true;
                        lblCreditLimit.Visible = true;
                        txtCreditLimit.Visible = true;
                        divTDSDeductee.Style.Add("display", "none");
                        td_EnrollmentId.Style.Add("display", "none");
                        PanelDocumentSegment.Style.Add("display", "none");
                        DivServiceBranch.Style.Add("display", "none");
                        break;
                    case "Franchisee":
                        pnlCredit.Style.Add("display", "none");
                        this.Title = "Franchisee";
                        ASPxLabel4.Text = "Unique ID";
                        
                        divSelectEmployee.Style.Add("display", "none");
                        divTDSDeductee.Style.Add("display", "none");

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
                        PanelDocumentSegment.Style.Add("display", "none");
                        break;
                    case "Consultant":
                        this.Title = "Consultant";
                        ASPxLabel4.Text = "Unique ID";
                        
                        divTDSDeductee.Style.Add("display", "none");

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
                        PanelDocumentSegment.Style.Add("display", "none");
                        DivServiceBranch.Style.Add("display", "none");
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
                        divTDSDeductee.Style.Add("display", "none");

                        pnlVehicleNo.Style.Add("display", "none");
                        td_EnrollmentId.Style.Add("display", "none");
                        td_registered.Style.Add("display", "none");
                        PanelDocumentSegment.Style.Add("display", "none");
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
                        divTDSDeductee.Style.Add("display", "none");

                        pnlVehicleNo.Style.Add("display", "none");
                        td_EnrollmentId.Style.Add("display", "none");
                        td_registered.Style.Add("display", "none");
                        PanelDocumentSegment.Style.Add("display", "none");
                        DivServiceBranch.Style.Add("display", "none");
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
                        divTDSDeductee.Style.Add("display", "none");

                        divSelectEmployee.Style.Add("display", "none");
                        td_EnrollmentId.Style.Add("display", "none");
                        pnlVehicleNo.Style.Add("display", "none");
                        td_registered.Style.Add("display", "none");
                        PanelDocumentSegment.Style.Add("display", "none");
                        DivServiceBranch.Style.Add("display", "none");
                        break;
                    case "Salesman/Agents":
                        this.Title = "Salesman/Agents";
                        ASPxLabel4.Text = "Unique ID";
                        ASPxLabel23.Visible = true;
                        txtDateRegis.Enabled = false;
                        ASPxLabel20.Text = "Salesman/Agent Type";
                        //coad added By Priti on 16122016
                        pnlCredit.Style.Add("display", "none");
                        divTDSDeductee.Style.Add("display", "none");

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
                        PanelDocumentSegment.Style.Add("display", "none");
                        DivServiceBranch.Style.Add("display", "none");
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
                        divTDSDeductee.Style.Add("display", "block");

                        divSelectEmployee.Style.Add("display", "none");
                        pnlVehicleNo.Style.Add("display", "block");
                        td_EnrollmentId.Style.Add("display", "block");
                        dvNameAsPerPan.Style.Add("display", "block");
                        dvDeducteestat.Style.Add("display", "block");
                        dvTaxDeducteeType.Style.Add("display", "block");
                        PanelDocumentSegment.Style.Add("display", "none");
                        DivServiceBranch.Style.Add("display", "none");
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
                        PanelDocumentSegment.Style.Add("display", "none");
                        DivServiceBranch.Style.Add("display", "none");
                        //...end...
                        break;
                }

            }

            DateTime dt = oDBEngine.GetDate();
            Session["InsertMode"] = Request.QueryString["id"];
            if (!IsPostBack)
            {
                //Add Rev Tanmoy 24-05-2021
                BindLoginUserInternalId();
                //end of Rev Tanmoy 24-05-2021
                //DataTable allModule = PopulateDetails();
                //ddlSegmentMandatory1.DataSource = allModule;
                //ddlSegmentMandatory1.ValueField = "Module_Id";
                //ddlSegmentMandatory1.TextField = "Module_Name";
                //ddlSegmentMandatory1.DataBind();

                //ddlSegmentMandatory2.DataSource = allModule;
                //ddlSegmentMandatory2.ValueField = "Module_Id";
                //ddlSegmentMandatory2.TextField = "Module_Name";
                //ddlSegmentMandatory2.DataBind();

                //ddlSegmentMandatory3.DataSource = allModule;
                //ddlSegmentMandatory3.ValueField = "Module_Id";
                //ddlSegmentMandatory3.TextField = "Module_Name";
                //ddlSegmentMandatory3.DataBind();

                //ddlSegmentMandatory4.DataSource = allModule;
                //ddlSegmentMandatory4.ValueField = "Module_Id";
                //ddlSegmentMandatory4.TextField = "Module_Name";
                //ddlSegmentMandatory4.DataBind();

                //ddlSegmentMandatory5.DataSource = allModule;
                //ddlSegmentMandatory5.ValueField = "Module_Id";
                //ddlSegmentMandatory5.TextField = "Module_Name";
                //ddlSegmentMandatory5.DataBind();


                BindTDS();


                //chinmoy added for Auto Number scheme start
                Session["InsertMode"] = Request.QueryString["id"];
                if (Session["requesttype"] != null)
                {
                    string RequestType = Convert.ToString(Session["requesttype"]);
                    if (RequestType == "Customer/Client")
                    {
                        if (!String.IsNullOrEmpty(UniqueAutoNumberCustomerMaster))
                        {
                            if (UniqueAutoNumberCustomerMaster == "Yes")
                            {
                                hdnAutoNumStg.Value = "1";
                                hdnTransactionType.Value = "CL";
                                //dvIdType.Visible = false;
                                //ASPxLabel12.Visible = false;
                                //ASPxLabelS12.Visible = false;
                                //dvClentUcc.Visible = false;
                                dvIdType.Style.Add("display", "none");
                                dvUniqueId.Style.Add("display", "none");
                                ASPxLabelS12.Style.Add("display", "none");
                                dvClentUcc.Style.Add("display", "none");
                                ddl_Num.Style.Add("display", "block");
                                dvCustDocNo.Style.Add("display", "block");
                                NumberingSchemeBind();

                            }
                            else if (UniqueAutoNumberCustomerMaster.ToUpper().Trim() == "NO")
                            {
                                hdnAutoNumStg.Value = "0";
                                hdnTransactionType.Value = "";
                                //dvIdType.Visible = true;
                                //ASPxLabel12.Visible = true;
                                //ASPxLabelS12.Visible = true;
                                //dvClentUcc.Visible = true;
                                dvIdType.Style.Add("display", "block");
                                dvUniqueId.Style.Add("display", "block");
                                ASPxLabelS12.Style.Add("display", "block");
                                dvClentUcc.Style.Add("display", "block");

                                //ddl_Num.Visible = false;
                                //dvCustDocNo.Visible = false;

                            }
                        }
                        if (hdnDocumentSegmentSettings.Value == "1")
                        {
                            AllBranchBind();
                        }
                    }

                    if (RequestType == "Lead")
                    {
                        if (!String.IsNullOrEmpty(UniqueAutoNumberLeadMaster))
                        {
                            if (UniqueAutoNumberLeadMaster == "Yes")
                            {
                                hdnAutoNumStg.Value = "LDAutoNum1";
                                hdnTransactionType.Value = "LD";
                                //DvLDUniqueId.Visible = false;
                                DvLDUniqueId.Style.Add("display", "none");
                                //LDddl_Num.Visible = true;
                                //LDdvCustDocNo.Visible = true;
                                LDddl_Num.Style.Add("display", "block");
                                LDdvCustDocNo.Style.Add("display", "block");
                                LeadNumberingSchemeBind();
                            }
                            else if (UniqueAutoNumberLeadMaster.ToUpper().Trim() == "NO")
                            {
                                hdnAutoNumStg.Value = "LDAutoNum0";
                                hdnTransactionType.Value = "";
                                //LDddl_Num.Visible = false;
                                //LDdvCustDocNo.Visible = false;
                                //DvLDUniqueId.Visible = true;
                                DvLDUniqueId.Style.Add("display", "block");
                            }
                        }
                    }

                    if (RequestType == "Salesman/Agents")
                    {
                        if (!String.IsNullOrEmpty(UniqueAutoNumberSalesmanMaster))
                        {
                            if (UniqueAutoNumberSalesmanMaster == "Yes")
                            {
                                hdnAutoNumStg.Value = "AGAutoNum1";
                                hdnTransactionType.Value = "AG";
                                //dvIdType.Visible = false;
                                //ASPxLabel12.Visible = false;
                                //ASPxLabelS12.Visible = false;
                                //dvClentUcc.Visible = false;
                                dvIdType.Style.Add("display", "none");
                                dvUniqueId.Style.Add("display", "none");
                                ASPxLabelS12.Style.Add("display", "none");
                                dvClentUcc.Style.Add("display", "none");
                                ddl_Num.Style.Add("display", "block");
                                dvCustDocNo.Style.Add("display", "block");
                                SalesmanNumberingSchemeBind();
                            }
                            else if (UniqueAutoNumberSalesmanMaster.ToUpper().Trim() == "NO")
                            {
                                hdnAutoNumStg.Value = "AGAutoNum0";
                                hdnTransactionType.Value = "";
                                //dvIdType.Visible = true;
                                //ASPxLabel12.Visible = true;
                                //ASPxLabelS12.Visible = true;
                                //dvClentUcc.Visible = true;
                                dvIdType.Style.Add("display", "block");
                                dvUniqueId.Style.Add("display", "block");
                                ASPxLabelS12.Style.Add("display", "block");
                                dvClentUcc.Style.Add("display", "block");

                            }
                        }
                    }


                    if (RequestType == "Transporter")
                    {
                        if (!String.IsNullOrEmpty(UniqueAutoNumberTransporterMaster))
                        {
                            if (UniqueAutoNumberTransporterMaster == "Yes")
                            {
                                hdnAutoNumStg.Value = "TRAutoNum1";
                                hdnTransactionType.Value = "TR";
                                dvIdType.Style.Add("display", "none");
                                dvClentUcc.Style.Add("display", "none");
                                dvUniqueId.Style.Add("display", "none");
                                ddl_Num.Style.Add("display", "block");
                                dvCustDocNo.Style.Add("display", "block");
                                TransporterNumberingSchemeBind();
                            }
                            else if (UniqueAutoNumberTransporterMaster.ToUpper().Trim() == "NO")
                            {
                                hdnAutoNumStg.Value = "TRAutoNum0";
                                hdnTransactionType.Value = "";
                                dvIdType.Style.Add("display", "block");
                                dvClentUcc.Style.Add("display", "block");
                                dvUniqueId.Style.Add("display", "block");
                            }
                        }
                    }
                    if (RequestType == "Relationship Partners")
                    {
                        if (!String.IsNullOrEmpty(UniqueAutoNumberInfluencerMaster))
                        {
                            if (UniqueAutoNumberInfluencerMaster == "Yes")
                            {
                                hdnAutoNumStg.Value = "RAAutoNum1";
                                hdnTransactionType.Value = "RA";
                                dvIdType.Style.Add("display", "none");
                                dvClentUcc.Style.Add("display", "none");
                                dvUniqueId.Style.Add("display", "none");
                                ddl_Num.Style.Add("display", "block");
                                dvCustDocNo.Style.Add("display", "block");
                                InfluencerNumberingSchemeBind();
                            }
                            else if (UniqueAutoNumberInfluencerMaster.ToUpper().Trim() == "NO")
                            {
                                hdnAutoNumStg.Value = "RAAutoNum0";
                                hdnTransactionType.Value = "";
                                dvIdType.Style.Add("display", "block");
                                dvClentUcc.Style.Add("display", "block");
                                dvUniqueId.Style.Add("display", "block");
                            }
                        }
                    }

                }
                //End
                //Start Rev set  Tanmoy 12-08-2019
                SendSmsBL objTdsTcsBL = new SendSmsBL();
                DataTable dtSms = objTdsTcsBL.GetSMSSettings("Show_SendSMS_CheckBox");
                if (dtSms != null && dtSms.Rows.Count > 0)
                {
                    hdnsendsms.Value = dtSms.Rows[0]["Variable_Value"].ToString();
                }
                if (hdnsendsms.Value == "No")
                {
                    divSendSMS.Attributes["class"] = "hidden";
                }
                //End Rev Tanmoy

                // GetFinacialYearBasedQouteDate();
                if (Session["requesttype"] != null)
                {
                    radioregistercheck.Attributes.Add("onclick", "registeredCheckChangeEvent()");
                    GstinSettingsButton.Visible = false;
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



                if (Convert.ToString(Session["requesttype"]) != "Lead")
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
                        txt_CustDocNo.ClientEnabled = false;
                        //rev srijeeta mantis issue 0024515
                        ASPxTextBox1.ClientEnabled = true;
                        //end of rev srijeeta mantis issue 0024515
                        
                        ddl_Num.Style.Add("display", "none");
                        LDtxt_CustDocNo.ClientEnabled = false;
                        LDddl_Num.Style.Add("display", "none");
                        txtClentUcc.ClientEnabled = false;
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
                        //rev srijeeta mantis issue 0024515[cnt_Alternative_Code column has been added and last parameter change from 48 to 49 ]
                        string[,] ContactData;
                        if (ID != 0)
                        {

                            ContactData = oDBEngine.GetFieldValue("tbl_master_contact",
                                                    "cnt_ucc, cnt_salutation,  cnt_firstName, cnt_middleName, cnt_lastName, cnt_shortName, cnt_branchId, cnt_sex, cnt_maritalStatus, case cnt_DOB when '1/1/1900 12:00:00 AM' then null else cnt_DOB end as cnt_DOB, case cnt_anniversaryDate when '1/1/1900 12:00:00 AM' then null else cnt_anniversaryDate end as cnt_anniversaryDate, cnt_legalStatus, cnt_education, cnt_profession, cnt_organization, cnt_jobResponsibility, cnt_designation, cnt_industry, cnt_contactSource, cnt_referedBy, cnt_contactType, cnt_contactStatus,case cnt_RegistrationDate when '1/1/1900 12:00:00 AM' then null else cnt_RegistrationDate end as cnt_RegistrationDate,cnt_rating,cnt_reason,cnt_bloodgroup,WebLogin,isnull(cnt_placeofincorporation,''),isnull(cnt_BusinessComncDate,''),isnull(cnt_OtherOccupation,''),isnull(cnt_nationality,'1'),cnt_IsCreditHold,cnt_CreditDays,cnt_CreditLimit,Statustype,CNT_GSTIN,cnt_AssociatedEmp,cnt_mainAccount,cnt_IdType,Enrolment_ID,cnt_PrintNameToCheque,TDSRATE_TYPE,isnull(Cnt_NameAsPerPan,'') Cnt_NameAsPerPan,isnull(Cnt_DeducteeStatus,'') Cnt_DeducteeStatus,isnull(CNT_TAX_ENTITYTYPE,'') CNT_TAX_ENTITYTYPE,isnull(Is_TCSApplicable,0) Is_TCSApplicable,isnull(CNT_TransactionCategory,0) CNT_TransactionCategory,ServiceBranchID,Alternative_Code",
                                                    " cnt_id=" + ID, 49);
                            
                        }
                        else
                        {

                            ContactData = oDBEngine.GetFieldValue("tbl_master_contact",
                                                    "cnt_ucc, cnt_salutation,  cnt_firstName, cnt_middleName, cnt_lastName, cnt_shortName, cnt_branchId, cnt_sex, cnt_maritalStatus, case cnt_DOB when '1/1/1900 12:00:00 AM' then null else cnt_DOB end as cnt_DOB, case cnt_anniversaryDate when '1/1/1900 12:00:00 AM' then null else cnt_anniversaryDate end as cnt_anniversaryDate, cnt_legalStatus, cnt_education, cnt_profession, cnt_organization, cnt_jobResponsibility, cnt_designation, cnt_industry, cnt_contactSource, cnt_referedBy, cnt_contactType, cnt_contactStatus,case cnt_RegistrationDate when '1/1/1900 12:00:00 AM' then null else cnt_RegistrationDate end as cnt_RegistrationDate,cnt_rating,cnt_reason,cnt_bloodgroup,WebLogin,isnull(cnt_placeofincorporation,''),isnull(cnt_BusinessComncDate,''),isnull(cnt_OtherOccupation,''),isnull(cnt_nationality,'1'),cnt_IsCreditHold,cnt_CreditDays,cnt_CreditLimit,Statustype,CNT_GSTIN,cnt_AssociatedEmp,cnt_mainAccount,cnt_IdType,Enrolment_ID,cnt_PrintNameToCheque,ISNULL(TDSRATE_TYPE,0) TDSRATE_TYPE,isnull(Cnt_NameAsPerPan,'') Cnt_NameAsPerPan,isnull(Cnt_DeducteeStatus,'') Cnt_DeducteeStatus,isnull(CNT_TAX_ENTITYTYPE,'') CNT_TAX_ENTITYTYPE,isnull(Is_TCSApplicable,0) Is_TCSApplicable,isnull(CNT_TransactionCategory,0) CNT_TransactionCategory,ServiceBranchID,Alternative_Code",
                          " cnt_id=" + HttpContext.Current.Session["KeyVal"], 49);
                           
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
                            page = ASPxPageControl1.TabPages.FindByName("TDS");
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

                        DataTable tdsdt = new DataTable();
                        ProcedureExecute proctds = new ProcedureExecute("PRC_CUSTOMER_TDS");
                        proctds.AddVarcharPara("@action", 50, "SELECT");
                        proctds.AddVarcharPara("@InetrnalId", 50, InternalId[0, 0]);
                        tdsdt = proctds.GetTable();
                        if (tdsdt != null && tdsdt.Rows.Count > 0)
                        {
                            aspxDeducteesNew.Value = Convert.ToString(tdsdt.Rows[0]["TDS_DEDUCTEES"]);
                        }
                        else
                        {
                            aspxDeducteesNew.Value = "";
                        }

                        if (Convert.ToString(Session["requesttype"]) == "Customer/Client")
                        {
                            GstinSettingsButton.Visible = true;

                            string DocumentSegment = "";
                            DataSet ds = new DataSet();
                            ProcedureExecute proc = new ProcedureExecute("prc_ENTITY_SEGMENT_MAP");
                            proc.AddVarcharPara("@action", 50, "SelectENTITY_SEGMENT_MAP");
                            proc.AddVarcharPara("@InetrnalId", 50, InternalId[0, 0]);
                            ds = proc.GetDataSet();
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                hdnSegmentMAPID.Value = Convert.ToString(ds.Tables[0].Rows[0]["ID"]);

                                DocumentSegment = Convert.ToString(ds.Tables[0].Rows[0]["Segment1"]);
                                DocumentSegment = DocumentSegment + "~" + Convert.ToString(ds.Tables[0].Rows[0]["Segment2"]);
                                DocumentSegment = DocumentSegment + "~" + Convert.ToString(ds.Tables[0].Rows[0]["Segment3"]);
                                DocumentSegment = DocumentSegment + "~" + Convert.ToString(ds.Tables[0].Rows[0]["Segment4"]);
                                DocumentSegment = DocumentSegment + "~" + Convert.ToString(ds.Tables[0].Rows[0]["Segment5"]);

                                HdnDocumentSegment.Value = DocumentSegment;


                                txtSegment1.Text = Convert.ToString(ds.Tables[0].Rows[0]["Segment1"]);
                                txtSegment2.Text = Convert.ToString(ds.Tables[0].Rows[0]["Segment2"]);
                                txtSegment3.Text = Convert.ToString(ds.Tables[0].Rows[0]["Segment3"]);
                                txtSegment4.Text = Convert.ToString(ds.Tables[0].Rows[0]["Segment4"]);
                                txtSegment5.Text = Convert.ToString(ds.Tables[0].Rows[0]["Segment5"]);



                                txtUsedFor1.Text = Convert.ToString(ds.Tables[0].Rows[0]["UsedFor1"]);
                                txtUsedFor2.Text = Convert.ToString(ds.Tables[0].Rows[0]["UsedFor2"]);
                                txtUsedFor3.Text = Convert.ToString(ds.Tables[0].Rows[0]["UsedFor3"]);
                                txtUsedFor4.Text = Convert.ToString(ds.Tables[0].Rows[0]["UsedFor4"]);
                                txtUsedFor5.Text = Convert.ToString(ds.Tables[0].Rows[0]["UsedFor5"]);





                            }
                            DataSet ds1 = new DataSet();
                            ProcedureExecute proc1 = new ProcedureExecute("prc_ENTITY_SEGMENT_MAP");
                            proc1.AddVarcharPara("@action", 50, "SelectCustomerSEGMENT_MAP");
                            proc1.AddVarcharPara("@InetrnalId", 50, InternalId[0, 0]);
                            ds1 = proc1.GetDataSet();
                            if (ds1 != null)
                            {
                                if (ds1.Tables[0].Rows.Count > 0)
                                {
                                    hdnSegmentMandatory1.Value = Convert.ToString(ds1.Tables[0].Rows[0]["Module_Id"]);
                                }
                                if (ds1.Tables[1].Rows.Count > 0)
                                {
                                    hdnSegmentMandatory2.Value = Convert.ToString(ds1.Tables[1].Rows[0]["Module_Id"]);
                                }
                                if (ds1.Tables[2].Rows.Count > 0)
                                {
                                    hdnSegmentMandatory3.Value = Convert.ToString(ds1.Tables[2].Rows[0]["Module_Id"]);
                                }
                                if (ds1.Tables[3].Rows.Count > 0)
                                {
                                    hdnSegmentMandatory4.Value = Convert.ToString(ds1.Tables[3].Rows[0]["Module_Id"]);
                                }
                                if (ds1.Tables[4].Rows.Count > 0)
                                {
                                    hdnSegmentMandatory5.Value = Convert.ToString(ds1.Tables[4].Rows[0]["Module_Id"]);
                                }


                            }
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
                        txtClentUcc.ClientEnabled = false;
                        ddlIdType.Visible = false;
                        LDtxt_CustDocNo.ClientEnabled = false;
                        LDddl_Num.Style.Add("display", "none");
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
                            //REV SRIJEETA  mantis issue 0024515
                            //ContactData = oDBEngine.GetFieldValue("tbl_master_contact",
                            //                        "cnt_ucc, cnt_salutation,  cnt_firstName, cnt_middleName, cnt_lastName, cnt_shortName, cnt_branchId, cnt_sex, cnt_maritalStatus, case cnt_DOB when '1/1/1900 12:00:00 AM' then null else cnt_DOB end as cnt_DOB, case cnt_anniversaryDate when '1/1/1900 12:00:00 AM' then null else cnt_anniversaryDate end as cnt_anniversaryDate, cnt_legalStatus, cnt_education, cnt_profession, cnt_organization, cnt_jobResponsibility, cnt_designation, cnt_industry, cnt_contactSource, cnt_referedBy, cnt_contactType, cnt_contactStatus,case cnt_RegistrationDate when '1/1/1900 12:00:00 AM' then null else cnt_RegistrationDate end as cnt_RegistrationDate,cnt_rating,cnt_reason,cnt_bloodgroup,WebLogin,isnull(cnt_placeofincorporation,''),isnull(cnt_BusinessComncDate,''),isnull(cnt_OtherOccupation,''),isnull(cnt_nationality,'1'),cnt_IsCreditHold,cnt_CreditDays,cnt_CreditLimit,cnt_PrintNameToCheque,case EnteredDate when '1/1/1900 12:00:00 AM' then null else EnteredDate end as EnteredDate ",
                            //                        " cnt_id=" + ID, 36);
                            ContactData = oDBEngine.GetFieldValue("tbl_master_contact",
                                                    "cnt_ucc, cnt_salutation,  cnt_firstName, cnt_middleName, cnt_lastName, cnt_shortName, cnt_branchId, cnt_sex, cnt_maritalStatus, case cnt_DOB when '1/1/1900 12:00:00 AM' then null else cnt_DOB end as cnt_DOB, case cnt_anniversaryDate when '1/1/1900 12:00:00 AM' then null else cnt_anniversaryDate end as cnt_anniversaryDate, cnt_legalStatus, cnt_education, cnt_profession, cnt_organization, cnt_jobResponsibility, cnt_designation, cnt_industry, cnt_contactSource, cnt_referedBy, cnt_contactType, cnt_contactStatus,case cnt_RegistrationDate when '1/1/1900 12:00:00 AM' then null else cnt_RegistrationDate end as cnt_RegistrationDate,cnt_rating,cnt_reason,cnt_bloodgroup,WebLogin,isnull(cnt_placeofincorporation,''),isnull(cnt_BusinessComncDate,''),isnull(cnt_OtherOccupation,''),isnull(cnt_nationality,'1'),cnt_IsCreditHold,cnt_CreditDays,cnt_CreditLimit,cnt_PrintNameToCheque,case EnteredDate when '1/1/1900 12:00:00 AM' then null else EnteredDate end as EnteredDate ,Alternative_Code",
                                                    " cnt_id=" + ID, 36);
                            //END OF REV SRIJEETA  mantis issue 0024515
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
                            //REV SRIJEETA  mantis issue 0024515
                            //ContactData = oDBEngine.GetFieldValue("tbl_master_contact",
                            //"cnt_ucc, cnt_salutation,  cnt_firstName, cnt_middleName, cnt_lastName, cnt_shortName, cnt_branchId, cnt_sex, cnt_maritalStatus, case cnt_DOB when '1/1/1900 12:00:00 AM' then null else cnt_DOB end as cnt_DOB, case cnt_anniversaryDate when '1/1/1900 12:00:00 AM' then null else cnt_anniversaryDate end as cnt_anniversaryDate, cnt_legalStatus, cnt_education, cnt_profession, cnt_organization, cnt_jobResponsibility, cnt_designation, cnt_industry, cnt_contactSource, cnt_referedBy, cnt_contactType, cnt_contactStatus,case cnt_RegistrationDate when '1/1/1900 12:00:00 AM' then null else cnt_RegistrationDate end as cnt_RegistrationDate,cnt_rating,cnt_reason,cnt_bloodgroup,WebLogin,isnull(cnt_placeofincorporation,''),isnull(cnt_BusinessComncDate,''),isnull(cnt_OtherOccupation,''),isnull(cnt_nationality,'1'),cnt_IsCreditHold,cnt_CreditDays,cnt_CreditLimit,cnt_PrintNameToCheque,case EnteredDate when '1/1/1900 12:00:00 AM' then null else EnteredDate end as EnteredDate ",
                            //" cnt_id=" + HttpContext.Current.Session["KeyVal"], 36);
                            ContactData = oDBEngine.GetFieldValue("tbl_master_contact",
                            "cnt_ucc, cnt_salutation,  cnt_firstName, cnt_middleName, cnt_lastName, cnt_shortName, cnt_branchId, cnt_sex, cnt_maritalStatus, case cnt_DOB when '1/1/1900 12:00:00 AM' then null else cnt_DOB end as cnt_DOB, case cnt_anniversaryDate when '1/1/1900 12:00:00 AM' then null else cnt_anniversaryDate end as cnt_anniversaryDate, cnt_legalStatus, cnt_education, cnt_profession, cnt_organization, cnt_jobResponsibility, cnt_designation, cnt_industry, cnt_contactSource, cnt_referedBy, cnt_contactType, cnt_contactStatus,case cnt_RegistrationDate when '1/1/1900 12:00:00 AM' then null else cnt_RegistrationDate end as cnt_RegistrationDate,cnt_rating,cnt_reason,cnt_bloodgroup,WebLogin,isnull(cnt_placeofincorporation,''),isnull(cnt_BusinessComncDate,''),isnull(cnt_OtherOccupation,''),isnull(cnt_nationality,'1'),cnt_IsCreditHold,cnt_CreditDays,cnt_CreditLimit,cnt_PrintNameToCheque,case EnteredDate when '1/1/1900 12:00:00 AM' then null else EnteredDate end as EnteredDate,Alternative_Code ",
                            " cnt_id=" + HttpContext.Current.Session["KeyVal"], 36);
                            //END OF REV SRIJEETA  mantis issue 0024515
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
                        //LDcmbMaritalStatus.SelectedIndex = 4;
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

            if (Convert.ToString(Session["requesttype"]) != "Lead")
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
        //Rev Rajdip

        public DataTable PopulateDetails()
        {
            ProcedureExecute proc = new ProcedureExecute("prc_ENTITY_SEGMENT_MAP");
            proc.AddVarcharPara("@Action", 50, "PopulateAllModule");
            return proc.GetTable();
        }
        public class Classcopy
        {
            public string id { get; set; }
            //public string Name { get; set; }
            public string cnt_IdType { get; set; }
            public string contactpersonforshipping { get; set; }
            public string cnt_ucc { get; set; }
            public string cnt_salutation { get; set; }
            public string cnt_firstName { get; set; }
            public string cnt_middleName { get; set; }
            public string cnt_lastName { get; set; }
            public string cnt_shortName { get; set; }
            public string cnt_branchId { get; set; }
            public string cnt_sex { get; set; }
            public string cnt_maritalStatus { get; set; }
            public string cnt_DOB { get; set; }
            public string cnt_anniversaryDate { get; set; }
            public string cnt_legalStatus { get; set; }
            public string cnt_education { get; set; }
            public string cnt_profession { get; set; }
            public string cnt_organization { get; set; }
            public string cnt_jobResponsibility { get; set; }
            public string cnt_designation { get; set; }
            public string cnt_industry { get; set; }
            public string cnt_contactSource { get; set; }
            public string cnt_referedBy { get; set; }
            public string cnt_contactType { get; set; }
            public string cnt_contactStatus { get; set; }
            public string cnt_RegistrationDate { get; set; }
            public string cnt_rating { get; set; }
            public string cnt_reason { get; set; }
            public string cnt_bloodgroup { get; set; }
            public string WebLogin { get; set; }
            public string cnt_placeofincorporation { get; set; }
            public string cnt_BusinessComncDate { get; set; }
            public string cnt_OtherOccupation { get; set; }
            public string cnt_nationality { get; set; }
            public string cnt_IsCreditHold { get; set; }
            public string cnt_CreditDays { get; set; }
            public string cnt_CreditLimit { get; set; }
            public string cnt_PrintNameToCheque { get; set; }
            public string EnteredDate { get; set; }
            public string grp_groupMaster { get; set; }
            //-----------------------------Master Contact details end------------------
            public string add_addressType { get; set; }
            public string add_address1 { get; set; }
            public string add_address2 { get; set; }
            public string add_address3 { get; set; }
            public string add_landMark { get; set; }
            public string add_country { get; set; }
            public string add_state { get; set; }
            public string add_pin { get; set; }
            public string add_activityId { get; set; }
            public string CreateDate { get; set; }
            public string CreateUser { get; set; }
            public string add_phone { get; set; }
            public string add_Email { get; set; }
            public string add_Website { get; set; }
            public string add_designation { get; set; }
            public string add_address4 { get; set; }
            public string Distance { get; set; }
            public string Add_EcomId { get; set; }
            public string add_city { get; set; }
            public string CNT_GSTIN { get; set; }
            public bool TCSApplicable { get; set; }


            public string Alternative_Code { get; set; }
        }

        [WebMethod]
        public static bool CheckUniqueNumberingCode(string uccName, string Type)
        {
            bool flag = false;
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            try
            {
                MShortNameCheckingBL objShortNameChecking = new MShortNameCheckingBL();
                flag = objShortNameChecking.CheckUnique(uccName, "0", Type);
            }
            catch (Exception ex)
            {

            }
            finally
            {
            }
            return flag;
        }


        //[WebMethod(EnableSession = true)]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod]
        public static object PopulateAllModule(string SegmentMAPID, string SegmnetNo)
        {
            //string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            string actionqry = "PopulateAllModule";

            List<ModuleDetails> GrpDet = new List<ModuleDetails>();
            {
                DataTable addtab = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_ENTITY_SEGMENT_MAP");
                proc.AddVarcharPara("@action", 100, actionqry);
                proc.AddVarcharPara("@Segment_Map_ID", 100, SegmentMAPID);
                proc.AddVarcharPara("@SegmentNo", 10, SegmnetNo);


                addtab = proc.GetTable();
                GrpDet = (from DataRow dr in addtab.Rows
                          select new ModuleDetails
                          {
                              Module_Id = Convert.ToString(dr["Module_Id"]),
                              Module_Name = Convert.ToString(dr["Module_Name"]),
                              IsChecked = Convert.ToString(dr["IsChecked"])

                          }).ToList();

            }
            return GrpDet;
        }

        public class ModuleDetails
        {

            public string Module_Id { get; set; }
            public string Module_Name { get; set; }

            public string IsChecked { get; set; }
        }
        [WebMethod]
        public static List<Classcopy> GetDataFromCustomer(string keyValue)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            string[,] ContactData;
            DataTable dtAddress = new DataTable();
            int ID = Convert.ToInt32(keyValue.ToString());

            //if (ID != 0) // lead edit
            //{


            //if (HttpContext.Current.Session["userid"] != null)
            //{
            //SearchKey = SearchKey.Replace("'", "''");
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            //DataTable classes = new DataTable();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("prc_GetCopyToCustomerdata", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@cnt_id", ID);
            
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dtAddress);

            cmd.Dispose();
            con.Dispose();
            List<Classcopy> Classcopytoproduct = new List<Classcopy>();
            Classcopytoproduct = (from DataRow dr in dtAddress.Rows
                                  select new Classcopy()
                                  {
                                      //id = dr["ID"].ToString(),
                                      //Name = dr["Name"].ToString(),

                                      cnt_ucc = dr["cnt_ucc"].ToString(),
                                      //rev srijeeta  mantis issue 0024515
                                      Alternative_Code = dr["Alternative_Code"].ToString(),
                                      //end of rev srijeeta  mantis issue 0024515
                                       cnt_salutation = dr["cnt_salutation"].ToString(),

                                      cnt_firstName = dr["cnt_firstName"].ToString(),
                                      cnt_middleName = dr["cnt_middleName"].ToString(),
                                      cnt_lastName = dr["cnt_lastName"].ToString(),
                                      cnt_shortName = dr["cnt_shortName"].ToString(),
                                      cnt_branchId = dr["cnt_branchId"].ToString(),
                                      cnt_sex = dr["cnt_sex"].ToString(),
                                      cnt_maritalStatus = dr["cnt_maritalStatus"].ToString(),
                                      cnt_DOB = dr["cnt_DOB"].ToString(),
                                      cnt_anniversaryDate = dr["cnt_anniversaryDate"].ToString(),
                                      cnt_legalStatus = dr["cnt_legalStatus"].ToString(),
                                      cnt_education = dr["cnt_education"].ToString(),
                                      cnt_profession = dr["cnt_profession"].ToString(),
                                      cnt_organization = dr["cnt_organization"].ToString(),
                                      cnt_jobResponsibility = dr["cnt_jobResponsibility"].ToString(),
                                      cnt_designation = dr["cnt_designation"].ToString(),
                                      cnt_industry = dr["cnt_industry"].ToString(),
                                      cnt_contactSource = dr["cnt_contactSource"].ToString(),
                                      cnt_referedBy = dr["cnt_referedBy"].ToString(),
                                      cnt_contactType = dr["cnt_contactType"].ToString(),
                                      cnt_contactStatus = dr["cnt_contactStatus"].ToString(),
                                      cnt_RegistrationDate = dr["cnt_RegistrationDate"].ToString(),
                                      cnt_rating = dr["cnt_rating"].ToString(),
                                      cnt_reason = dr["cnt_reason"].ToString(),
                                      cnt_bloodgroup = dr["cnt_bloodgroup"].ToString(),
                                      WebLogin = dr["WebLogin"].ToString(),
                                      cnt_placeofincorporation = dr["cnt_placeofincorporation"].ToString(),
                                      cnt_BusinessComncDate = dr["cnt_BusinessComncDate"].ToString(),
                                      cnt_OtherOccupation = dr["cnt_OtherOccupation"].ToString(),
                                      cnt_nationality = dr["cnt_nationality"].ToString(),
                                      cnt_IsCreditHold = dr["cnt_IsCreditHold"].ToString(),
                                      cnt_CreditDays = dr["cnt_CreditDays"].ToString(),
                                      cnt_CreditLimit = dr["cnt_CreditLimit"].ToString(),
                                      cnt_PrintNameToCheque = dr["cnt_PrintNameToCheque"].ToString(),
                                      EnteredDate = dr["EnteredDate"].ToString(),
                                      //------------------------------------------------
                                      add_addressType = dr["add_addressType"].ToString(),
                                      add_address1 = dr["add_address1"].ToString(),
                                      add_address2 = dr["add_address2"].ToString(),
                                      add_address3 = dr["add_address3"].ToString(),
                                      add_landMark = dr["add_landMark"].ToString(),
                                      add_country = dr["add_country"].ToString(),
                                      add_state = dr["add_state"].ToString(),
                                      add_pin = dr["add_pin"].ToString(),
                                      add_activityId = dr["add_activityId"].ToString(),
                                      CreateDate = dr["CreateDate"].ToString(),
                                      CreateUser = dr["CreateUser"].ToString(),
                                      add_phone = dr["add_phone"].ToString(),
                                      add_Email = dr["add_Email"].ToString(),
                                      add_Website = dr["add_Website"].ToString(),
                                      add_designation = dr["add_designation"].ToString(),
                                      add_address4 = dr["add_address4"].ToString(),
                                      Distance = dr["Distance"].ToString(),
                                      Add_EcomId = dr["Add_EcomId"].ToString(),
                                      add_city = dr["add_city"].ToString(),
                                      CNT_GSTIN = dr["CNT_GSTIN"].ToString(),
                                      contactpersonforshipping = dr["contactpersonforshipping"].ToString(),
                                      cnt_IdType = dr["cnt_IdType"].ToString(),
                                      grp_groupMaster = dr["grp_groupMaster"].ToString(),
                                      TCSApplicable = Convert.ToBoolean(dr["TCSApplicable"].ToString())
                                     
                                     
                                  }).ToList();
            //}


            //}
            return Classcopytoproduct;

        }
        //End Rev Rajdip

        public void NumberingSchemeBind()
        {
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            string actionqry = "GetAllDropDownDetailForCustomerMaster";
            DataTable Schemadt = GetAllDropDownDetailForCustomerMaster(userbranchHierarchy, actionqry);

            if (Schemadt != null && Schemadt.Rows.Count > 0)
            {
                ddl_numberingScheme.DataTextField = "SchemaName";
                ddl_numberingScheme.DataValueField = "Id";
                ddl_numberingScheme.DataSource = Schemadt;
                ddl_numberingScheme.DataBind();
            }

        }

        public void AllBranchBind()
        {

            DataTable BranchDT = GetAllBranch();

            if (BranchDT != null && BranchDT.Rows.Count > 0)
            {
                ddlServiceBranch.DataTextField = "branch_description";
                ddlServiceBranch.DataValueField = "branch_id";
                ddlServiceBranch.DataSource = BranchDT;
                ddlServiceBranch.DataBind();
            }

        }



        public void TransporterNumberingSchemeBind()
        {
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            string actionqry = "GetAllDropDownDetailForTransPorterMaster";
            DataTable Schemadt = GetAllDropDownDetailForCustomerMaster(userbranchHierarchy, actionqry);

            if (Schemadt != null && Schemadt.Rows.Count > 0)
            {
                ddl_numberingScheme.DataTextField = "SchemaName";
                ddl_numberingScheme.DataValueField = "Id";
                ddl_numberingScheme.DataSource = Schemadt;
                ddl_numberingScheme.DataBind();
            }

        }

        public void InfluencerNumberingSchemeBind()
        {
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            string actionqry = "GetAllDropDownDetailForInfluencerMaster";
            DataTable Schemadt = GetAllDropDownDetailForCustomerMaster(userbranchHierarchy, actionqry);

            if (Schemadt != null && Schemadt.Rows.Count > 0)
            {
                ddl_numberingScheme.DataTextField = "SchemaName";
                ddl_numberingScheme.DataValueField = "Id";
                ddl_numberingScheme.DataSource = Schemadt;
                ddl_numberingScheme.DataBind();
            }

        }
        public void SalesmanNumberingSchemeBind()
        {
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            string actionqry = "GetAllDropDownDetailForSalesManMaster";
            DataTable Schemadt = GetAllDropDownDetailForCustomerMaster(userbranchHierarchy, actionqry);

            if (Schemadt != null && Schemadt.Rows.Count > 0)
            {
                ddl_numberingScheme.DataTextField = "SchemaName";
                ddl_numberingScheme.DataValueField = "Id";
                ddl_numberingScheme.DataSource = Schemadt;
                ddl_numberingScheme.DataBind();
            }

        }

        public void LeadNumberingSchemeBind()
        {
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            string actionqry = "GetAllDropDownDetailForLeadMaster";
            DataTable Schemadt = GetAllDropDownDetailForCustomerMaster(userbranchHierarchy, actionqry);

            if (Schemadt != null && Schemadt.Rows.Count > 0)
            {
                LDddl_numberingScheme.DataTextField = "SchemaName";
                LDddl_numberingScheme.DataValueField = "Id";
                LDddl_numberingScheme.DataSource = Schemadt;
                LDddl_numberingScheme.DataBind();
            }

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

            //Debjyoti 30-12-2016
            //Reason: UDF count
            IsUdfpresent.Value = Convert.ToString(getUdfCount());
            //End Debjyoti 30-12-2016
        }
        public DataTable GetAllDropDownDetailForCustomerMaster(string UserBranch, string Qry)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesActivity");
            proc.AddVarcharPara("@Action", 100, Qry);
            proc.AddVarcharPara("@userbranchlist", 4000, UserBranch);
            ds = proc.GetTable();
            return ds;
        }

        public DataTable GetAllBranch()
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 100, "GetAllBranch");
            ds = proc.GetTable();
            return ds;
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
                if (hdnAutoNumStg.Value == "1" && hdnTransactionType.Value == "CL")
                {
                    txt_CustDocNo.Text = ContactData[0, 0];
                }
                if (hdnAutoNumStg.Value == "LDAutoNum1" && hdnTransactionType.Value == "LD")
                {
                    LDtxt_CustDocNo.Text = ContactData[0, 0];
                }
                if (hdnAutoNumStg.Value == "AGAutoNum1" && hdnTransactionType.Value == "AG")
                {
                    txt_CustDocNo.Text = ContactData[0, 0];
                }
                if (hdnAutoNumStg.Value == "TRAutoNum1" && hdnTransactionType.Value == "TR")
                {
                    txt_CustDocNo.Text = ContactData[0, 0];
                }
                if (hdnAutoNumStg.Value == "RAAutoNum1" && hdnTransactionType.Value == "RA")
                {
                    txt_CustDocNo.Text = ContactData[0, 0];
                }
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
                if (ContactData[0, 42] != "")
                {
                    txtNameAsPerPan.Text = ContactData[0, 42];
                }
                else
                {
                    txtNameAsPerPan.Text = "";
                }

                if (ContactData[0, 43] != "")
                {
                    cmbDeducteestat.Value = ContactData[0, 43];
                }
                if (ContactData[0, 45] != "" && ContactData[0, 45] != null)
                {
                    TCSApplicable.Value = Convert.ToBoolean(ContactData[0, 45]);
                }

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
                cmbTDS.Value = ContactData[0, 41];
                cmbTaxdeducteedType.Value = ContactData[0, 44];

                cmbTransCategory.Value = ContactData[0, 46];

                ddlServiceBranch.SelectedValue = ContactData[0, 47];
                //rev srijeeta  mantis issue 0024515
                ASPxTextBox1.Text = ContactData[0, 48];
                //end of rev srijeeta  mantis issue 0024515
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
            page = ASPxPageControl1.TabPages.FindByName("TDS");
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
            //chinmoy added  31-03-2020 start 
            String Uniquename = "";
            if (hdnAutoNumStg.Value == "1")
            {

                Uniquename = hddnDocNo.Value.Trim();

            }
            else if (hdnAutoNumStg.Value == "LDAutoNum1")
            {

                Uniquename = hddnDocNo.Value.Trim();

            }
            else if (hdnAutoNumStg.Value == "AGAutoNum1")
            {

                Uniquename = hddnDocNo.Value.Trim();

            }
            else if (hdnAutoNumStg.Value == "LDAutoNum0")
            {

                Uniquename = LDtxtClentUcc.Text.Trim();

            }
            else if (hdnAutoNumStg.Value == "TRAutoNum1")
            {

                Uniquename = hddnDocNo.Value.Trim();

            }
            else if (hdnAutoNumStg.Value == "RAAutoNum1")
            {

                Uniquename = hddnDocNo.Value.Trim();

            }
            else
            {
                Uniquename = txtClentUcc.Text.Trim();
            }
            //End
            SendSmsBL ObjSms = new SendSmsBL();
            CommonBL ComBL = new CommonBL();
            string msgBody = ObjSms.getMsgbody(1);
            if (!string.IsNullOrEmpty(msgBody))
            {
                msgBody = msgBody.Replace("@Customer_name", LDtxtFirstNmae.Text.ToString().Trim());
                // msgBody = msgBody.Replace("@Date", LDcmbProfession.Text);
                //msgBody = msgBody.Replace("@Profession", dt_EnteredOn.Value.ToString());
            }

            // Rev 1.0
            if (txtFirstNmae.Text != "")
            {
                if (Convert.ToString(Session["requesttype"]) == "Customer/Client")
                {
                    string AllowDuplicateCustomerName = ComBL.GetSystemSettingsResult("AllowDuplicateCustomerName");
                    if (!String.IsNullOrEmpty(AllowDuplicateCustomerName))
                    {
                        if (AllowDuplicateCustomerName == "No")
                        {
                            bool flag = false;
                            BusinessLogicLayer.MShortNameCheckingBL obj = new BusinessLogicLayer.MShortNameCheckingBL();
                            flag = obj.CheckUniqueVendorName(txtFirstNmae.Text, Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]), "CheckUniqueCustomerName");

                            if (!flag)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "jAlert('Duplicate Name.');", true);
                                return;
                            }
                        }
                    }
                }
                else if (Convert.ToString(Session["requesttype"]) == "Transporter")
                {
                    string AllowDuplicateTransporterName = ComBL.GetSystemSettingsResult("AllowDuplicateTransporterName");
                    if (!String.IsNullOrEmpty(AllowDuplicateTransporterName))
                    {
                        if (AllowDuplicateTransporterName == "No")
                        {
                            bool flag = false;
                            BusinessLogicLayer.MShortNameCheckingBL obj = new BusinessLogicLayer.MShortNameCheckingBL();
                            flag = obj.CheckUniqueVendorName(txtFirstNmae.Text, Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]), "CheckUniqueTransporterName");

                            if (!flag)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "jAlert('Duplicate Name.');", true);
                                return;
                            }
                        }
                    }
                }
            }
            // End of Rev 1.0

            string ContType = "";
            string gstin = "";
            Boolean result = false;
            if (txtGSTIN1.Text.Trim() != "" && txtGSTIN2.Text.Trim() != "" && txtGSTIN3.Text.Trim() != "")
            {
                ContType = retContactType(Convert.ToString(Session["requesttype"]));
                string DuplicateGSTINCustomer = ComBL.GetSystemSettingsResult("DuplicateGSTINCustomer");
                if (!String.IsNullOrEmpty(DuplicateGSTINCustomer))
                {
                    if (DuplicateGSTINCustomer == "No")
                    {
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
            if (Convert.ToString(Session["requesttype"]) != "Lead")
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
                    //txtAliasName.Text = txtClentUcc.Text;
                    txtAliasName.Text = Uniquename;

                }
                if (ContType != "XC")
                {
                    if (Request.QueryString["formtype"] != null)
                    {

                    }

                    else
                    {
                       // DataTable dt1 = oDBEngine.GetDataTable("tbl_master_contactExchange,tbl_master_contact", "top 1 cnt_ucc,crg_tcode", "cnt_ucc='" + Uniquename + "' or crg_tcode='" + Uniquename + "'  and cnt_id='" + Convert.ToString(HttpContext.Current.Session["KeyVal"]) + "'");
                        DataTable dt1 = oDBEngine.GetDataTable("tbl_master_contact", "top 1 cnt_ucc,cnt_shortName", "cnt_ucc='" + Uniquename + "'   and cnt_id='" + Convert.ToString(HttpContext.Current.Session["KeyVal"]) + "'");
                        
                        DataTable dt2 = oDBEngine.GetDataTable("tbl_master_contact", " cnt_id", "cnt_id='" + Convert.ToString(HttpContext.Current.Session["KeyVal"]) + "'");

                        //############### Updated By Samrat Roy --- changes done due to edit is not working --- 24/04/2017
                        //string[,] abcd = oDBEngine.GetFieldValue("tbl_master_contactExchange,tbl_master_contact", "top 1 cnt_ucc,crg_tcode", "(cnt_ucc='" + Convert.ToString(Uniquename) + "' or crg_tcode='" + Convert.ToString(Uniquename) + "')  and cnt_id='" + Convert.ToString(HttpContext.Current.Session["KeyVal"]) + "' and cnt_contactType='" + ContType + "'", 2);
                        string[,] abcd = oDBEngine.GetFieldValue("tbl_master_contact", "top 1 cnt_ucc,cnt_shortName", "cnt_ucc='" + Convert.ToString(Uniquename) + "'  and cnt_id='" + Convert.ToString(HttpContext.Current.Session["KeyVal"]) + "' and cnt_contactType='" + ContType + "'", 2);
                        
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
                                //Password = txtClentUcc.Text;
                                Password = Uniquename;
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
                                    value = "Statustype='" + status + "',cnt_ucc='" + Uniquename + "', cnt_salutation=" + CmbSalutation.SelectedItem.Value + ",  cnt_firstName='" + txtFirstNmae.Text + "', cnt_middleName='" + txtMiddleName.Text + "', cnt_lastName='" + txtLastName.Text + "', cnt_shortName='" + txtAliasName.Text + "', cnt_branchId=" + cmbBranch.SelectedItem.Value + ", cnt_sex=" + cmbGender.SelectedItem.Value + ", cnt_maritalStatus=" + cmbMaritalStatus.SelectedItem.Value + ", cnt_DOB='" + txtDOB.Value + "', cnt_anniversaryDate='" + txtAnniversary.Value + "', cnt_legalStatus=" + cmbLegalStatus.SelectedItem.Value + ", cnt_education=" + cmbEducation.SelectedItem.Value + ", cnt_profession=" + cmbProfession.SelectedItem.Value + ", cnt_organization='" + txtOrganization.Text + "', cnt_jobResponsibility=" + cmbJobResponsibility.SelectedItem.Value + ", cnt_designation=" + cmbDesignation.SelectedItem.Value + ", cnt_industry=" + cmbIndustry.SelectedItem.Value + ", cnt_contactSource=" + cmbSource.SelectedItem.Value + ", cnt_referedBy='" + txtReferedBy.Text + "', cnt_contactType='" + ContType + "', cnt_contactStatus=" + cmbContactStatus.SelectedItem.Value + ",cnt_RegistrationDate='" + txtDateRegis.Value + "',cnt_rating='" + cmbRating.SelectedItem.Value + "',cnt_reason='" + TxtContactStatus.Text + "',cnt_bloodgroup='" + cmbBloodgroup.SelectedItem.Value + "',WebLogIn='" + webLogin + "',PassWord='" + Password + "', lastModifyDate ='" + today + "',cnt_nationality='" + Convert.ToInt32(Convert.ToString(country)) + "',cnt_PlaceOfIncorporation='',cnt_BusinessComncDate='',cnt_OtherOccupation='" + other + "', lastModifyUser=" + HttpContext.Current.Session["userid"] + "',  Enrolment_ID='" + TransporterEnrollmentId + "',cnt_PrintNameToCheque='" + txtPname.Text + "',CNT_TAX_ENTITYTYPE='" + cmbTaxdeducteedType.Value + "',Is_TCSApplicable='" + TCSApplicable.Value + "',CNT_TransactionCategory='" + cmbTransCategory.Value + "',ServiceBranchID='" + ddlServiceBranch.SelectedValue + "'"; // + Session["userid"]
                                }
                                else
                                {
                                    value = "Statustype='" + status + "',cnt_ucc='" + Uniquename + "', cnt_salutation=" + CmbSalutation.SelectedItem.Value + ",  cnt_firstName='" + txtFirstNmae.Text + "', cnt_middleName='" + txtMiddleName.Text + "', cnt_lastName='" + txtLastName.Text + "', cnt_shortName='" + txtAliasName.Text + "', cnt_branchId=" + cmbBranch.SelectedItem.Value + ", cnt_sex=" + cmbGender.SelectedItem.Value + ", cnt_maritalStatus=" + cmbMaritalStatus.SelectedItem.Value + ", cnt_DOB='" + txtDOB.Value + "', cnt_anniversaryDate='" + txtAnniversary.Value + "', cnt_legalStatus=" + cmbLegalStatus.SelectedItem.Value + ", cnt_education=" + cmbEducation.SelectedItem.Value + ", cnt_profession=" + cmbProfession.SelectedItem.Value + ", cnt_organization='" + txtOrganization.Text + "', cnt_jobResponsibility=" + cmbJobResponsibility.SelectedItem.Value + ", cnt_designation=" + cmbDesignation.SelectedItem.Value + ", cnt_industry=" + cmbIndustry.SelectedItem.Value + ", cnt_contactSource=" + cmbSource.SelectedItem.Value + ", cnt_referedBy='" + txtReferedBy.Text + "', cnt_contactType='" + ContType + "', cnt_contactStatus=" + cmbContactStatus.SelectedItem.Value + ",cnt_RegistrationDate='" + txtDateRegis.Value + "',cnt_rating='" + cmbRating.SelectedItem.Value + "',cnt_reason='" + TxtContactStatus.Text + "',cnt_bloodgroup='" + cmbBloodgroup.SelectedItem.Value + "',WebLogIn='" + webLogin + "',PassWord='" + Password + "', lastModifyDate ='" + today + "',cnt_nationality='" + Convert.ToInt32(Convert.ToString(country)) + "',cnt_PlaceOfIncorporation='" + txtincorporation.Text.Trim() + "',cnt_BusinessComncDate='" + txtFromDate.Value + "',cnt_OtherOccupation='" + other + "',lastModifyUser=" + HttpContext.Current.Session["userid"] + "',  Enrolment_ID='" + TransporterEnrollmentId + "',cnt_PrintNameToCheque='" + txtPname.Text + "',CNT_TAX_ENTITYTYPE='" + cmbTaxdeducteedType.Value + "',Is_TCSApplicable='" + TCSApplicable.Value + "',CNT_TransactionCategory='" + cmbTransCategory.Value + "',ServiceBranchID='" + ddlServiceBranch.SelectedValue + "'"; // + Session["userid"]
                                }
                                oDBEngine.SetFieldValue("tbl_master_contact", value, " cnt_id=" + HttpContext.Current.Session["KeyVal"]);

                                string ModuleType = Convert.ToString(Session["requesttype"]);

                                if (ModuleType == "Customer/Client" && txtGSTIN2.Text.Trim() != "")
                                {
                                    string KeyValId = Convert.ToString(HttpContext.Current.Session["KeyVal"]);
                                    DataTable dt = UpdatePanNumber(KeyValId, txtGSTIN2.Text.Trim());
                                }

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
                                                string NameAsperPan = "";
                                                string DeducteeStatus = "";
                                                if (txtNameAsPerPan.Text != "")
                                                {
                                                    NameAsperPan = txtNameAsPerPan.Text;
                                                }
                                                if (Convert.ToString(cmbDeducteestat.Value) != "")
                                                {
                                                    DeducteeStatus = Convert.ToString(cmbDeducteestat.Value);
                                                }

                                                value = "Statustype='" + status + "',cnt_ucc='" + Uniquename + "', cnt_salutation=" + CmbSalutation.SelectedItem.Value + ",  cnt_firstName='" + txtFirstNmae.Text + "', cnt_middleName='" + txtMiddleName.Text + "', cnt_lastName='" + txtLastName.Text + "', cnt_shortName='" + txtAliasName.Text + "', cnt_branchId=" + cmbBranch.SelectedItem.Value + ", cnt_sex=" + cmbGender.SelectedItem.Value + ", cnt_maritalStatus=" + cmbMaritalStatus.SelectedItem.Value + ", cnt_DOB='" + txtDOB.Value + "', cnt_anniversaryDate='" + txtAnniversary.Value + "', cnt_legalStatus=" + cmbLegalStatus.SelectedItem.Value + ", cnt_education=" + cmbEducation.SelectedItem.Value + ", cnt_profession=" + cmbProfession.SelectedItem.Value + ", cnt_organization='" + txtOrganization.Text + "', cnt_jobResponsibility=" + cmbJobResponsibility.SelectedItem.Value + ", cnt_designation=" + cmbDesignation.SelectedItem.Value + ", cnt_industry=" + cmbIndustry.SelectedItem.Value + ", cnt_contactSource=" + cmbSource.SelectedItem.Value + ", cnt_referedBy='" + txtReferedBy_hidden.Text + "', cnt_contactType='" + ContType + "', cnt_contactStatus=" + cmbContactStatus.SelectedItem.Value + ",cnt_RegistrationDate='" + txtDateRegis.Value + "',cnt_rating='" + cmbRating.SelectedItem.Value + "',cnt_reason='" + TxtContactStatus.Text + "',cnt_bloodgroup='" + cmbBloodgroup.SelectedItem.Value + "',WebLogIn='" + webLogin + "',PassWord='" + Password + "', lastModifyDate ='" + today + "',cnt_PlaceOfIncorporation='',cnt_BusinessComncDate='',cnt_OtherOccupation='" + other + "',cnt_nationality='" + Convert.ToInt32(Convert.ToString(country)) + "',cnt_IsCreditHold='" + Creditcard + "',cnt_CreditDays='" + Convert.ToInt32(txtcreditDays.Text) + "' ,cnt_CreditLimit='" + Convert.ToDecimal(txtCreditLimit.Text) + "', lastModifyUser='" + HttpContext.Current.Session["userid"] + "',  Enrolment_ID='" + TransporterEnrollmentId + "',cnt_PrintNameToCheque='" + txtPname.Text + "',TDSRATE_TYPE='" + cmbTDS.Value + "',Cnt_NameAsPerPan='" + NameAsperPan + "',Cnt_DeducteeStatus='" + DeducteeStatus + "',CNT_TAX_ENTITYTYPE='" + cmbTaxdeducteedType.Value + "',Is_TCSApplicable='" + TCSApplicable.Value + "',CNT_TransactionCategory='" + cmbTransCategory.Value + "',ServiceBranchID='" + ddlServiceBranch.SelectedValue + "'"; // + Session["userid"]

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
                                                //Rev Subhra 13-09-2019
                                                //value = "Statustype='" + status + "',cnt_ucc='" + txtClentUcc.Text + "', cnt_salutation=" + CmbSalutation.SelectedItem.Value + ",  cnt_firstName='" + txtFirstNmae.Text + "', cnt_middleName='" + txtMiddleName.Text + "', cnt_lastName='" + txtLastName.Text + "', cnt_shortName='" + txtAliasName.Text + "', cnt_branchId=" + cmbBranch.SelectedItem.Value + ", cnt_sex=" + cmbGender.SelectedItem.Value + ", cnt_maritalStatus=" + cmbMaritalStatus.SelectedItem.Value + ", cnt_DOB='" + txtDOB.Value + "', cnt_anniversaryDate='" + txtAnniversary.Value + "', cnt_legalStatus=" + cmbLegalStatus.SelectedItem.Value + ", cnt_education=" + cmbEducation.SelectedItem.Value + ", cnt_profession=" + cmbProfession.SelectedItem.Value + ", cnt_organization='" + txtOrganization.Text + "', cnt_jobResponsibility=" + cmbJobResponsibility.SelectedItem.Value + ", cnt_designation=" + cmbDesignation.SelectedItem.Value + ", cnt_industry=" + cmbIndustry.SelectedItem.Value + ", cnt_contactSource=" + cmbSource.SelectedItem.Value + ", cnt_referedBy='" + txtReferedBy_hidden.Text + "', cnt_contactType='" + ContType + "', cnt_contactStatus=" + cmbContactStatus.SelectedItem.Value + ",cnt_RegistrationDate='" + txtDateRegis.Value + "',cnt_rating='" + cmbRating.SelectedItem.Value + "',cnt_reason='" + TxtContactStatus.Text + "',cnt_bloodgroup='" + cmbBloodgroup.SelectedItem.Value + "',WebLogIn='" + webLogin + "',PassWord='" + Password + "', lastModifyDate ='" + today + "',cnt_PlaceOfIncorporation='" + txtincorporation.Text.ToString().Trim() + "',cnt_nationality='" + Convert.ToInt32(Convert.ToString(country)) + "',cnt_BusinessComncDate='" + txtFromDate.Value + "',cnt_OtherOccupation='" + other + "',cnt_IsCreditHold='" + Creditcard + "',cnt_CreditDays='" + Convert.ToInt32(txtcreditDays.Text) + "' ,cnt_CreditLimit='" + Convert.ToDecimal(txtCreditLimit.Text) + "', lastModifyUser=" + HttpContext.Current.Session["userid"] + ",CNT_GSTIN='" + GSTIN + "',cnt_AssociatedEmp= '" + hidAssociatedEmp.Value + "',cnt_mainAccount='" + hndTaxRates_MainAccount_hidden.Value + "',  Enrolment_ID='" + TransporterEnrollmentId + "',cnt_PrintNameToCheque='" + txtPname.Text + "'"; // + Session["userid"]
                                                string vendtype = "";
                                                string NameAsperPan = "";
                                                string DeducteeStatus = "";
                                                
                                                if (txtNameAsPerPan.Text != "")
                                                {
                                                    NameAsperPan = txtNameAsPerPan.Text;
                                                }
                                                //rev srijeeta  mantis issue 0024515
                                                string Alternative_Code = "";
                                                if (ASPxTextBox1.Text != "")
                                                {
                                                    Alternative_Code = ASPxTextBox1.Text;
                                                }
                                                //end of rev srijeeta  mantis issue 0024515

                                                if (Convert.ToString(cmbDeducteestat.Value) != "")
                                                {
                                                    DeducteeStatus = Convert.ToString(cmbDeducteestat.Value);
                                                }
                                                //rev srijeeta[add Alternative_Code]  mantis issue 0024515
                                                if (radioregistercheck.SelectedValue == "0")
                                                {
                                                    vendtype = "";
                                                    value = "Statustype='" + status + "',cnt_ucc='" + Uniquename + "', cnt_salutation=" + CmbSalutation.SelectedItem.Value + ",  cnt_firstName='" + txtFirstNmae.Text + "', cnt_middleName='" + txtMiddleName.Text + "', cnt_lastName='" + txtLastName.Text + "', cnt_shortName='" + txtAliasName.Text + "', cnt_branchId=" + cmbBranch.SelectedItem.Value + ", cnt_sex=" + cmbGender.SelectedItem.Value + ", cnt_maritalStatus=" + cmbMaritalStatus.SelectedItem.Value + ", cnt_DOB='" + txtDOB.Value + "', cnt_anniversaryDate='" + txtAnniversary.Value + "', cnt_legalStatus=" + cmbLegalStatus.SelectedItem.Value + ", cnt_education=" + cmbEducation.SelectedItem.Value + ", cnt_profession=" + cmbProfession.SelectedItem.Value + ", cnt_organization='" + txtOrganization.Text + "', cnt_jobResponsibility=" + cmbJobResponsibility.SelectedItem.Value + ", cnt_designation=" + cmbDesignation.SelectedItem.Value + ", cnt_industry=" + cmbIndustry.SelectedItem.Value + ", cnt_contactSource=" + cmbSource.SelectedItem.Value + ", cnt_referedBy='" + txtReferedBy_hidden.Text + "', cnt_contactType='" + ContType + "', cnt_contactStatus=" + cmbContactStatus.SelectedItem.Value + ",cnt_RegistrationDate='" + txtDateRegis.Value + "',cnt_rating='" + cmbRating.SelectedItem.Value + "',cnt_reason='" + TxtContactStatus.Text + "',cnt_bloodgroup='" + cmbBloodgroup.SelectedItem.Value + "',WebLogIn='" + webLogin + "',PassWord='" + Password + "', lastModifyDate ='" + today + "',cnt_PlaceOfIncorporation='" + txtincorporation.Text.ToString().Trim() + "',cnt_nationality='" + Convert.ToInt32(Convert.ToString(country)) + "',cnt_BusinessComncDate='" + txtFromDate.Value + "',cnt_OtherOccupation='" + other + "',cnt_IsCreditHold='" + Creditcard + "',cnt_CreditDays='" + Convert.ToInt32(txtcreditDays.Text) + "' ,cnt_CreditLimit='" + Convert.ToDecimal(txtCreditLimit.Text) + "', lastModifyUser=" + HttpContext.Current.Session["userid"] + ",CNT_GSTIN='" + GSTIN + "',cnt_AssociatedEmp= '" + hidAssociatedEmp.Value + "',cnt_mainAccount='" + hndTaxRates_MainAccount_hidden.Value + "',  Enrolment_ID='" + TransporterEnrollmentId + "',cnt_PrintNameToCheque='" + txtPname.Text + "',TDSRATE_TYPE='" + cmbTDS.Value + "',Cnt_NameAsPerPan='" + NameAsperPan + "',Cnt_DeducteeStatus='" + DeducteeStatus + "',CNT_TAX_ENTITYTYPE='" + cmbTaxdeducteedType.Value + "',Is_TCSApplicable='" + TCSApplicable.Value + "',CNT_TransactionCategory='" + cmbTransCategory.Value + "',ServiceBranchID='" + ddlServiceBranch.SelectedValue +  "',Alternative_Code='" + ASPxTextBox1.Text + "'";
                                                }
                                                  
                                                else
                                                {
                                                    vendtype = "R";
                                                    value = "Statustype='" + status + "',cnt_ucc='" + Uniquename + "', cnt_salutation=" + CmbSalutation.SelectedItem.Value + ",  cnt_firstName='" + txtFirstNmae.Text + "', cnt_middleName='" + txtMiddleName.Text + "', cnt_lastName='" + txtLastName.Text + "', cnt_shortName='" + txtAliasName.Text + "', cnt_branchId=" + cmbBranch.SelectedItem.Value + ", cnt_sex=" + cmbGender.SelectedItem.Value + ", cnt_maritalStatus=" + cmbMaritalStatus.SelectedItem.Value + ", cnt_DOB='" + txtDOB.Value + "', cnt_anniversaryDate='" + txtAnniversary.Value + "', cnt_legalStatus=" + cmbLegalStatus.SelectedItem.Value + ", cnt_education=" + cmbEducation.SelectedItem.Value + ", cnt_profession=" + cmbProfession.SelectedItem.Value + ", cnt_organization='" + txtOrganization.Text + "', cnt_jobResponsibility=" + cmbJobResponsibility.SelectedItem.Value + ", cnt_designation=" + cmbDesignation.SelectedItem.Value + ", cnt_industry=" + cmbIndustry.SelectedItem.Value + ", cnt_contactSource=" + cmbSource.SelectedItem.Value + ", cnt_referedBy='" + txtReferedBy_hidden.Text + "', cnt_contactType='" + ContType + "', cnt_contactStatus=" + cmbContactStatus.SelectedItem.Value + ",cnt_RegistrationDate='" + txtDateRegis.Value + "',cnt_rating='" + cmbRating.SelectedItem.Value + "',cnt_reason='" + TxtContactStatus.Text + "',cnt_bloodgroup='" + cmbBloodgroup.SelectedItem.Value + "',WebLogIn='" + webLogin + "',PassWord='" + Password + "', lastModifyDate ='" + today + "',cnt_PlaceOfIncorporation='" + txtincorporation.Text.ToString().Trim() + "',cnt_nationality='" + Convert.ToInt32(Convert.ToString(country)) + "',cnt_BusinessComncDate='" + txtFromDate.Value + "',cnt_OtherOccupation='" + other + "',cnt_IsCreditHold='" + Creditcard + "',cnt_CreditDays='" + Convert.ToInt32(txtcreditDays.Text) + "' ,cnt_CreditLimit='" + Convert.ToDecimal(txtCreditLimit.Text) + "', lastModifyUser=" + HttpContext.Current.Session["userid"] + ",CNT_GSTIN='" + GSTIN + "',cnt_AssociatedEmp= '" + hidAssociatedEmp.Value + "',cnt_mainAccount='" + hndTaxRates_MainAccount_hidden.Value + "',  Enrolment_ID='" + TransporterEnrollmentId + "',cnt_PrintNameToCheque='" + txtPname.Text + "',cnt_EntityType='" + vendtype + "',TDSRATE_TYPE='" + cmbTDS.Value + "',Cnt_NameAsPerPan='" + NameAsperPan + "',Cnt_DeducteeStatus='" + DeducteeStatus + "',CNT_TAX_ENTITYTYPE='" + cmbTaxdeducteedType.Value + "',Is_TCSApplicable='" + TCSApplicable.Value + "',CNT_TransactionCategory='" + cmbTransCategory.Value + "',ServiceBranchID='" + ddlServiceBranch.SelectedValue + "',Alternative_Code='" + ASPxTextBox1.Text + "'"; // + Session["userid"]
                                                }
                                                //end of rev srijeeta  mantis issue 0024515
                                                //End of Rev



                                            }

                                            #region Subhabrata

                                            bool IsSaved = false;
                                            bool flagEntity = false;
                                            Employee_BL ebl = new Employee_BL();
                                            string User_Id = Convert.ToString(Session["userid"]);
                                            if (Convert.ToString(hddnGSTINFlag.Value).ToUpper() == "UPDATE")
                                            {

                                                //Rev Rajdip
                                                //IsSaved = ebl.AddCustVendHistory(Convert.ToString(gstin), Convert.ToInt32(HttpContext.Current.Session["KeyVal"]),
                                                //    Convert.ToDateTime(dt_ApplicableFrom.Value), User_Id, "GSTIN_Customer");
                                                string GSTIN;
                                                GSTIN = txtGSTIN1.Text.Trim() + txtGSTIN2.Text.Trim() + txtGSTIN3.Text.Trim();
                                                IsSaved = ebl.AddCustVendHistory(Convert.ToString(GSTIN), Convert.ToInt32(HttpContext.Current.Session["KeyVal"]),
                                                    Convert.ToDateTime(dt_ApplicableFrom.Value), User_Id, "GSTIN_Customer");
                                                //End Rev Rajdip
                                                flagEntity = true;
                                            }
                                            else if (Convert.ToString(hddnGSTINFlag.Value).ToUpper() == "NOTUPDATE")
                                            {

                                                IsSaved = ebl.AddCustVendHistory(Convert.ToString(gstin), Convert.ToInt32(HttpContext.Current.Session["KeyVal"]),
                                                   Convert.ToDateTime(dt_ApplicableFrom.Value), User_Id, "GSTIN_UpdateCust");
                                            }







                                            #endregion

                                            //Rev Subhra 03-06-2019
                                            if (Convert.ToString(Session["requesttype"]) == "Lead")
                                            {
                                                oDBEngine.SetFieldValue("tbl_master_contact", "EnteredDate='" + dt_EnteredOn.Value + "' ", " cnt_id=" + HttpContext.Current.Session["KeyVal"]);
                                            }
                                            //End of Rev 03-06-2019
                                            string ModuleType = Convert.ToString(Session["requesttype"]);

                                            if (ModuleType == "Customer/Client" && txtGSTIN2.Text.Trim() != "")
                                            {
                                                string KeyValId = Convert.ToString(HttpContext.Current.Session["KeyVal"]);
                                                DataTable dt = UpdatePanNumber(KeyValId, txtGSTIN2.Text.Trim());
                                            }

                                            if (Convert.ToString(hdnGSTINFlagTranspoter.Value).ToUpper() == "UPDATE")
                                            {

                                                gstin = txtGSTIN1.Text.Trim() + txtGSTIN2.Text.Trim() + txtGSTIN3.Text.Trim();
                                                IsSaved = ebl.AddCustVendHistory(Convert.ToString(gstin), Convert.ToInt32(HttpContext.Current.Session["KeyVal"]),
                                                    Convert.ToDateTime(dt_ApplicableFrom.Value), User_Id, "GSTIN_Transporter");
                                                flagEntity = true;
                                            }
                                            else if (Convert.ToString(hdnGSTINFlagTranspoter.Value).ToUpper() == "NOTUPDATE")
                                            {
                                                gstin = txtGSTIN1.Text.Trim() + txtGSTIN2.Text.Trim() + txtGSTIN3.Text.Trim();
                                                IsSaved = ebl.AddCustVendHistory(Convert.ToString(gstin), Convert.ToInt32(HttpContext.Current.Session["KeyVal"]),
                                                   Convert.ToDateTime(dt_ApplicableFrom.Value), User_Id, "GSTIN_UpdateTransporter");
                                            }


                                            oDBEngine.SetFieldValue("tbl_master_contact", value, " cnt_id=" + HttpContext.Current.Session["KeyVal"]);

                                            ProcedureExecute proc1;
                                            using (proc1 = new ProcedureExecute("PRC_CUSTOMER_TDS"))
                                            {
                                                proc1.AddVarcharPara("@ACTION", 50, "Update");
                                                proc1.AddVarcharPara("@cnt_id", 50, Convert.ToString(HttpContext.Current.Session["KeyVal"]));
                                                proc1.AddPara("@TDS_DEDUCTEES", Convert.ToString(aspxDeducteesNew.Value));
                                                int i = proc1.RunActionQuery();
                                            }

                                            string DocumentSegment = HdnDocumentSegment.Value;
                                            if (!string.IsNullOrEmpty(DocumentSegment) && DocumentSegment != "0~0~0~0~0")
                                            {
                                                string[] _DocumentSegment = DocumentSegment.Split('~');
                                                string segmentMendatorychecking = "";
                                                string SegmentMandatory1 = hdnSegmentMandatory1.Value;
                                                string ddlSegmentMandatory1 = "";
                                                if (SegmentMandatory1 != "")
                                                {
                                                    ddlSegmentMandatory1 = String.Join(",", SegmentMandatory1);
                                                    segmentMendatorychecking = String.Join(",", ddlSegmentMandatory1);

                                                }

                                                string SegmentMandatory2 = hdnSegmentMandatory2.Value;
                                                string ddlSegmentMandatory2 = "";
                                                if (SegmentMandatory2 != "")
                                                {
                                                    ddlSegmentMandatory2 = String.Join(",", SegmentMandatory2);
                                                    segmentMendatorychecking = String.Join(",", segmentMendatorychecking, SegmentMandatory2);

                                                }

                                                string SegmentMandatory3 = hdnSegmentMandatory3.Value;
                                                string ddlSegmentMandatory3 = "";
                                                if (SegmentMandatory3 != "")
                                                {
                                                    ddlSegmentMandatory3 = String.Join(",", SegmentMandatory3);
                                                    segmentMendatorychecking = String.Join(",", segmentMendatorychecking, SegmentMandatory3);

                                                }

                                                string SegmentMandatory4 = hdnSegmentMandatory4.Value;
                                                string ddlSegmentMandatory4 = "";
                                                if (SegmentMandatory4 != "")
                                                {
                                                    ddlSegmentMandatory4 = String.Join(",", SegmentMandatory4);
                                                    segmentMendatorychecking = String.Join(",", segmentMendatorychecking, SegmentMandatory4);
                                                }


                                                string SegmentMandatory5 = hdnSegmentMandatory5.Value;
                                                string ddlSegmentMandatory5 = "";
                                                if (SegmentMandatory5 != "")
                                                {
                                                    ddlSegmentMandatory5 = String.Join(",", SegmentMandatory5);
                                                    segmentMendatorychecking = String.Join(",", segmentMendatorychecking, SegmentMandatory5);
                                                }


                                                //  segmentMendatorychecking = String.Join(",", ddlSegmentMandatory1, ddlSegmentMandatory2, ddlSegmentMandatory3, ddlSegmentMandatory4, ddlSegmentMandatory5);

                                                if (segmentMendatorychecking != "")
                                                {


                                                    var duplicateRecords = segmentMendatorychecking
                                                    .GroupBy(p => p) //coloumn name which has the duplicate values
                                                   .Where(gr => gr.Count() > 1)
                                                   .Select(g => g.Key);

                                                    var validate = "";

                                                    if (duplicateRecords.Count() > 0)
                                                    {
                                                        validate = "duplicateProduct";
                                                    }
                                                    //foreach (var d in duplicateRecords)
                                                    //{
                                                    //    validate = "duplicateProduct";
                                                    //}
                                                    if (validate == "duplicateProduct")
                                                    {
                                                        Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Same Segment Mandatory For different Segment are not allowed.')</script>");
                                                        return;
                                                    }
                                                }


                                                ProcedureExecute proc;
                                                string rtrnvalue = "";
                                                try
                                                {
                                                    using (proc = new ProcedureExecute("prc_ENTITY_SEGMENT_MAP"))
                                                    {
                                                        proc.AddVarcharPara("@action", 50, "UpdateENTITY_SEGMENT_MAP");
                                                        proc.AddVarcharPara("@cnt_id", 50, Convert.ToString(HttpContext.Current.Session["KeyVal"]));

                                                        //proc.AddVarcharPara("@Segment1", 100, _DocumentSegment[0]);
                                                        //proc.AddVarcharPara("@Segment2", 100, _DocumentSegment[1]);
                                                        //proc.AddVarcharPara("@Segment3", 100, _DocumentSegment[2]);
                                                        //proc.AddVarcharPara("@Segment4", 100, _DocumentSegment[3]);
                                                        //proc.AddVarcharPara("@Segment5", 100, _DocumentSegment[4]);

                                                        if (Convert.ToString(txtSegment1.Text) == "")
                                                        {
                                                            txtSegment1.Text = "0";
                                                        }
                                                        if (Convert.ToString(txtSegment2.Text) == "")
                                                        {
                                                            txtSegment2.Text = "0";
                                                        }
                                                        if (Convert.ToString(txtSegment3.Text) == "")
                                                        {
                                                            txtSegment3.Text = "0";
                                                        }
                                                        if (Convert.ToString(txtSegment4.Text) == "")
                                                        {
                                                            txtSegment4.Text = "0";
                                                        }
                                                        if (Convert.ToString(txtSegment5.Text) == "")
                                                        {
                                                            txtSegment5.Text = "0";
                                                        }

                                                        proc.AddVarcharPara("@Segment1", 100, txtSegment1.Text);
                                                        proc.AddVarcharPara("@Segment2", 100, txtSegment2.Text);
                                                        proc.AddVarcharPara("@Segment3", 100, txtSegment3.Text);
                                                        proc.AddVarcharPara("@Segment4", 100, txtSegment4.Text);
                                                        proc.AddVarcharPara("@Segment5", 100, txtSegment5.Text);


                                                        proc.AddVarcharPara("@SegmentUsedFor1", 100, txtUsedFor1.Text);
                                                        proc.AddVarcharPara("@SegmentUsedFor2", 100, txtUsedFor2.Text);
                                                        proc.AddVarcharPara("@SegmentUsedFor3", 100, txtUsedFor3.Text);
                                                        proc.AddVarcharPara("@SegmentUsedFor4", 100, txtUsedFor4.Text);
                                                        proc.AddVarcharPara("@SegmentUsedFor5", 100, txtUsedFor5.Text);

                                                        proc.AddVarcharPara("@SegmentMandatory1", 100, Convert.ToString(ddlSegmentMandatory1));
                                                        proc.AddVarcharPara("@SegmentMandatory2", 100, Convert.ToString(ddlSegmentMandatory2));
                                                        proc.AddVarcharPara("@SegmentMandatory3", 100, Convert.ToString(ddlSegmentMandatory3));
                                                        proc.AddVarcharPara("@SegmentMandatory4", 100, Convert.ToString(ddlSegmentMandatory4));
                                                        proc.AddVarcharPara("@SegmentMandatory5", 100, Convert.ToString(ddlSegmentMandatory5));

                                                        int i = proc.RunActionQuery();
                                                    }
                                                }

                                                catch (Exception ex)
                                                {
                                                    throw ex;
                                                }

                                                finally
                                                {
                                                    proc = null;
                                                }
                                            }

                                            //--------------------
                                            if (Convert.ToString(Session["requesttype"]) == "Transporter")
                                            {
                                                string vehicleNos = VehicleNo_hidden.Value;

                                                //string _vehicleNo=vehicleNos.Split(",").ToString();
                                                if (!string.IsNullOrEmpty(vehicleNos))
                                                {
                                                    string[] _vehicleNo = vehicleNos.Split(',');

                                                    foreach (var word in _vehicleNo)
                                                    {
                                                        if (Regex.IsMatch(word, @"^[a-zA-Z0-9]+$"))
                                                        {

                                                        }
                                                        else
                                                        {

                                                            Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Special Characters are not allowed in Vehicle No.')</script>");
                                                            return;

                                                        }
                                                    }
                                                }

                                                if (Convert.ToString(cmbLegalStatus.SelectedValue) == "54")//Local
                                                {
                                                    //Contact ct = new Contact();
                                                    //ct.Delete_TransporterVehicles(Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]));
                                                    //VehicleNo_hidden.Value = "";

                                                    //if (Regex.IsMatch(vehicleNos, @"^[a-zA-Z0-9\s.\?\,\'\;\:\!\-]+$"))

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
                                    if (Convert.ToString(Session["requesttype"]) == "Transporter")
                                    {
                                        string vehicleNos = VehicleNo_hidden.Value;

                                        if (!string.IsNullOrEmpty(vehicleNos))
                                        {
                                            string[] _vehicleNo = vehicleNos.Split(',');

                                            foreach (var word in _vehicleNo)
                                            {
                                                if (Regex.IsMatch(word, @"^[a-zA-Z0-9]+$"))
                                                {

                                                }
                                                else
                                                {

                                                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Special Characters are not allowed in Vehicle No.')</script>");
                                                    return;

                                                }
                                            }
                                        }

                                        if (Convert.ToString(cmbLegalStatus.SelectedValue) == "54")//Local
                                        {
                                            if (!string.IsNullOrEmpty(vehicleNos))
                                            {
                                                Contact ct = new Contact();
                                                ct.Insert_TransporterVehicles(Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]), vehicleNos);
                                            }
                                        }
                                        else
                                        {
                                            Contact ct = new Contact();
                                            ct.Delete_TransporterVehicles(Convert.ToString(HttpContext.Current.Session["KeyVal_InternalID"]));
                                            VehicleNo_hidden.Value = "";
                                        }
                                    }

                                    String value = "Statustype='" + status + "',cnt_ucc='" + Uniquename + "', cnt_salutation=" + CmbSalutation.SelectedItem.Value + ",  cnt_firstName='" + txtFirstNmae.Text + "', cnt_middleName='" + txtMiddleName.Text + "', cnt_lastName='" + txtLastName.Text + "', cnt_shortName='" + txtAliasName.Text + "',cnt_PrintNameToCheque='" + txtPname.Text + "' ,CNT_TAX_ENTITYTYPE='" + cmbTaxdeducteedType.Value + "',Is_TCSApplicable='" + TCSApplicable.Value + "',CNT_TransactionCategory='" + cmbTransCategory.Value + "',cnt_branchId=" + cmbBranch.SelectedItem.Value + ", cnt_sex=" + cmbGender.SelectedItem.Value + ", cnt_maritalStatus=" + cmbMaritalStatus.SelectedItem.Value + ", cnt_DOB='" + txtDOB.Value + "', cnt_anniversaryDate='" + txtAnniversary.Value + "', cnt_legalStatus=" + cmbLegalStatus.SelectedItem.Value + ", cnt_education=" + cmbEducation.SelectedItem.Value + ", cnt_profession=" + cmbProfession.SelectedItem.Value + ", cnt_organization='" + txtOrganization.Text + "', cnt_jobResponsibility=" + cmbJobResponsibility.SelectedItem.Value + ", cnt_designation=" + cmbDesignation.SelectedItem.Value + ", cnt_industry=" + cmbIndustry.SelectedItem.Value + ", cnt_contactSource=" + cmbSource.SelectedItem.Value + ", cnt_referedBy='" + txtReferedBy_hidden.Text + "', cnt_contactType='" + ContType + "', cnt_contactStatus=" + cmbContactStatus.SelectedItem.Value + ",cnt_RegistrationDate='" + txtDateRegis.Value + "',cnt_rating='" + cmbRating.SelectedItem.Value + "',cnt_reason='" + TxtContactStatus.Text + "',cnt_bloodgroup='" + cmbBloodgroup.SelectedItem.Value + "',WebLogIn='" + webLogin + "',PassWord='" + Password + "', lastModifyDate ='" + today + "',lastModifyUser=" + HttpContext.Current.Session["userid"]; // + Session["userid"]
                                    oDBEngine.SetFieldValue("tbl_master_contact", value, " cnt_id=" + HttpContext.Current.Session["KeyVal"]);
                                    string ModuleType1 = Convert.ToString(Session["requesttype"]);

                                    if (ModuleType1 == "Customer/Client" && txtGSTIN2.Text.Trim() != "")
                                    {
                                        string KeyValId = Convert.ToString(HttpContext.Current.Session["KeyVal"]);
                                        DataTable dt = UpdatePanNumber(KeyValId, txtGSTIN2.Text.Trim());
                                    }
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
                                string ModuleType2 = Convert.ToString(Session["requesttype"]);

                                if (ModuleType2 == "Customer/Client" && txtGSTIN2.Text.Trim() != "")
                                {
                                    string KeyValId = Convert.ToString(HttpContext.Current.Session["KeyVal"]);
                                    DataTable dt = UpdatePanNumber(KeyValId, txtGSTIN2.Text.Trim());
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

                                    //Rev Subhra 13-09-2019
                                    string vendtype = "";
                                    if (radioregistercheck.SelectedValue == "0")
                                    {
                                        vendtype = "";
                                    }
                                    else
                                    {
                                        vendtype = "R";
                                    }
                                    //End of Rev
                                    // chinmoy edited for Auto Number scheme start


                                    DataTable dtDelete = new DataTable();
                                    dtDelete = oDBEngine.GetDataTable("delete from tbl_master_contact where cnt_UCC='Auto'");

                                    int numberingId = 0;
                                    string UccName = "";
                                    if (hdnAutoNumStg.Value == "1" && ContType == "CL")
                                    {
                                        numberingId = Convert.ToInt32(hdnNumberingId.Value.Trim());
                                        UccName = hddnDocNo.Value.Trim();

                                    }
                                    else if (hdnAutoNumStg.Value == "LDAutoNum1" && ContType == "LD")
                                    {
                                        numberingId = Convert.ToInt32(hdnNumberingId.Value.Trim());
                                        UccName = hddnDocNo.Value.Trim();

                                    }
                                    else if (hdnAutoNumStg.Value == "AGAutoNum1" && ContType == "AG")
                                    {
                                        numberingId = Convert.ToInt32(hdnNumberingId.Value.Trim());
                                        UccName = hddnDocNo.Value.Trim();

                                    }
                                    else if (hdnAutoNumStg.Value == "TRAutoNum1" && ContType == "TR")
                                    {
                                        numberingId = Convert.ToInt32(hdnNumberingId.Value.Trim());
                                        UccName = hddnDocNo.Value.Trim();

                                    }
                                    else if (hdnAutoNumStg.Value == "RAAutoNum1" && ContType == "RA")
                                    {
                                        numberingId = Convert.ToInt32(hdnNumberingId.Value.Trim());
                                        UccName = hddnDocNo.Value.Trim();

                                    }
                                    else
                                    {
                                        UccName = txtClentUcc.Text.Trim();
                                    }

                                    string NameAsperPan = "";
                                    string DeducteeStatus = "";
                                    if (txtNameAsPerPan.Text != "")
                                    {
                                        NameAsperPan = txtNameAsPerPan.Text;
                                    }
                                    if (Convert.ToString(cmbDeducteestat.Value) != "")
                                    {
                                        DeducteeStatus = Convert.ToString(cmbDeducteestat.Value);
                                    }

                                    // Rev Srijeeta  mantis issue 0024515
                                    string Alternative_Code = "";
                                    if (ASPxTextBox1.Text != "")
                                    {
                                        Alternative_Code = ASPxTextBox1.Text;
                                    }
                                    // End of Rev Srijeeta  mantis issue 0024515

                                    //End
                                    if (Convert.ToString(Session["requesttype"]) == "Transporter")
                                    {
                                        string vehicleNos_add = VehicleNo_hidden.Value;

                                        if (!string.IsNullOrEmpty(vehicleNos_add))
                                        {
                                            string[] _vehicleNo = vehicleNos_add.Split(',');

                                            foreach (var word in _vehicleNo)
                                            {
                                                if (Regex.IsMatch(word, @"^[a-zA-Z0-9]+$"))
                                                {

                                                }
                                                else
                                                {

                                                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Special Characters are not allowed in Vehicle No.')</script>");
                                                    return;

                                                }
                                            }
                                        }
                                        ////if (!Regex.IsMatch(vehicleNos_add, @"^[a-zA-Z0-9\s.\?\,\'\;\:\!\-]+$"))
                                        //if (Regex.IsMatch(vehicleNos_add, @"^[a-zA-Z0-9\,]+$"))
                                        //{

                                        //}
                                        //else
                                        //{
                                        //    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Special Characters are not allowed in Vehicle No.')</script>");
                                        //    return;
                                        //}
                                    }
                                    //REV SRIJEETA  mantis issue 0024515
                                    //string InternalId = oContactGeneralBL.Insert_ContactGeneral(dd, UccName, CmbSalutation.SelectedItem.Value,
                                    //                  txtFirstNmae.Text.Trim(), txtMiddleName.Text.Trim(), txtLastName.Text.Trim(),
                                    //                  txtAliasName.Text.Trim(), cmbBranch.SelectedItem.Value, cmbGender.SelectedItem.Value,
                                    //                  cmbMaritalStatus.SelectedItem.Value, dtDob, dtanniversary, cmbLegalStatus.SelectedItem.Value,
                                    //                  cmbEducation.SelectedItem.Value, cmbProfession.SelectedItem.Value, txtOrganization.Text.Trim(),
                                    //                  cmbJobResponsibility.SelectedItem.Value, cmbDesignation.SelectedItem.Value, cmbIndustry.SelectedItem.Value,
                                    //                  cmbSource.SelectedItem.Value, txtReferedBy.Text.Trim(), txtRPartner_hidden.Text.Trim(), ContType,
                                    //                  cmbContactStatus.SelectedItem.Value, dtReg, cmbRating.SelectedItem.Value, TxtContactStatus.Text.Trim(),
                                    //                  cmbBloodgroup.SelectedItem.Value, webLogin, Password, Convert.ToString(HttpContext.Current.Session["userid"]), vPlaceincop,
                                    //                  dtBusiness, other, country, Creditcard, Convert.ToInt32(txtcreditDays.Text.Trim()), Convert.ToDecimal(txtCreditLimit.Text.Trim()), Convert.ToString(cmbContactStatusclient.SelectedItem.Value),
                                    //                  GSTIN, hidAssociatedEmp.Value, txtPname.Text, cmbTDS.Value.ToString(), vendtype, numberingId, NameAsperPan, DeducteeStatus, Convert.ToString(cmbTaxdeducteedType.Value), Convert.ToBoolean(TCSApplicable.Value),txtGSTIN2.Text.Trim(), Convert.ToString(cmbTransCategory.Value), Convert.ToString(ddlServiceBranch.SelectedValue)
                                    //                  
                                    //                  );
                                    string InternalId = oContactGeneralBL.Insert_ContactGeneral(dd, UccName,Alternative_Code, CmbSalutation.SelectedItem.Value,
                                                      txtFirstNmae.Text.Trim(), txtMiddleName.Text.Trim(), txtLastName.Text.Trim(),
                                                      txtAliasName.Text.Trim(), cmbBranch.SelectedItem.Value, cmbGender.SelectedItem.Value,
                                                      cmbMaritalStatus.SelectedItem.Value, dtDob, dtanniversary, cmbLegalStatus.SelectedItem.Value,
                                                      cmbEducation.SelectedItem.Value, cmbProfession.SelectedItem.Value, txtOrganization.Text.Trim(),
                                                      cmbJobResponsibility.SelectedItem.Value, cmbDesignation.SelectedItem.Value, cmbIndustry.SelectedItem.Value,
                                                      cmbSource.SelectedItem.Value, txtReferedBy.Text.Trim(), txtRPartner_hidden.Text.Trim(), ContType,
                                                      cmbContactStatus.SelectedItem.Value, dtReg, cmbRating.SelectedItem.Value, TxtContactStatus.Text.Trim(),
                                                      cmbBloodgroup.SelectedItem.Value, webLogin, Password, Convert.ToString(HttpContext.Current.Session["userid"]), vPlaceincop,
                                                      dtBusiness, other, country, Creditcard, Convert.ToInt32(txtcreditDays.Text.Trim()), Convert.ToDecimal(txtCreditLimit.Text.Trim()), Convert.ToString(cmbContactStatusclient.SelectedItem.Value),
                                                      GSTIN, hidAssociatedEmp.Value, txtPname.Text, cmbTDS.Value.ToString(), vendtype, numberingId, NameAsperPan, DeducteeStatus, Convert.ToString(cmbTaxdeducteedType.Value), Convert.ToBoolean(TCSApplicable.Value), txtGSTIN2.Text.Trim(), Convert.ToString(cmbTransCategory.Value), Convert.ToString(ddlServiceBranch.SelectedValue)
                                        
                                                      );
                                    //end of REV SRIJEETA  mantis issue 0024515
                                    if (InternalId != "")
                                    {

                                        string RequesttttType = Convert.ToString(Session["requesttype"]);
                                        if (RequesttttType == "Customer/Client")
                                        {
                                            CommonBL ComnonBL = new CommonBL();


                                            string SyncCustomertoFSMWhileCreating = ComnonBL.GetSystemSettingsResult("SyncCustomertoFSMWhileCreating");
                                            if (SyncCustomertoFSMWhileCreating.ToUpper() == "YES")
                                            {
                                                CustomerSync(InternalId);
                                            }
                                        }


                                        ProcedureExecute proc1;
                                        using (proc1 = new ProcedureExecute("PRC_CUSTOMER_TDS"))
                                        {
                                            proc1.AddVarcharPara("@ACTION", 50, "ADD");
                                            proc1.AddPara("@InetrnalId", InternalId);
                                            proc1.AddPara("@TDS_DEDUCTEES", Convert.ToString(aspxDeducteesNew.Value));
                                            int i = proc1.RunActionQuery();
                                        }


                                        string DocumentSegment = HdnDocumentSegment.Value;
                                        if (!string.IsNullOrEmpty(DocumentSegment))
                                        {
                                            string[] _DocumentSegment = DocumentSegment.Split('~');

                                            ProcedureExecute proc;
                                            string rtrnvalue = "";
                                            try
                                            {
                                                using (proc = new ProcedureExecute("prc_ENTITY_SEGMENT_MAP"))
                                                {
                                                    proc.AddVarcharPara("@action", 50, "InsertENTITY_SEGMENT_MAP");
                                                    proc.AddVarcharPara("@InetrnalId", 50, InternalId);

                                                    //proc.AddVarcharPara("@Segment1", 100, _DocumentSegment[0]);
                                                    //proc.AddVarcharPara("@Segment2", 100, _DocumentSegment[1]);
                                                    //proc.AddVarcharPara("@Segment3", 100, _DocumentSegment[2]);
                                                    //proc.AddVarcharPara("@Segment4", 100, _DocumentSegment[3]);
                                                    //proc.AddVarcharPara("@Segment5", 100, _DocumentSegment[4]);

                                                    proc.AddVarcharPara("@Segment1", 100, txtSegment1.Text);
                                                    proc.AddVarcharPara("@Segment2", 100, txtSegment2.Text);
                                                    proc.AddVarcharPara("@Segment3", 100, txtSegment3.Text);
                                                    proc.AddVarcharPara("@Segment4", 100, txtSegment4.Text);
                                                    proc.AddVarcharPara("@Segment5", 100, txtSegment5.Text);

                                                    proc.AddVarcharPara("@SegmentUsedFor1", 100, txtUsedFor1.Text);
                                                    proc.AddVarcharPara("@SegmentUsedFor2", 100, txtUsedFor2.Text);
                                                    proc.AddVarcharPara("@SegmentUsedFor3", 100, txtUsedFor3.Text);
                                                    proc.AddVarcharPara("@SegmentUsedFor4", 100, txtUsedFor4.Text);
                                                    proc.AddVarcharPara("@SegmentUsedFor5", 100, txtUsedFor5.Text);

                                                    //proc.AddVarcharPara("@SegmentMandatory1", 100, Convert.ToString(ddlSegmentMandatory1.Value));
                                                    //proc.AddVarcharPara("@SegmentMandatory2", 100, Convert.ToString(ddlSegmentMandatory2.Value));
                                                    //proc.AddVarcharPara("@SegmentMandatory3", 100, Convert.ToString(ddlSegmentMandatory3.Value));
                                                    //proc.AddVarcharPara("@SegmentMandatory4", 100, Convert.ToString(ddlSegmentMandatory4.Value));
                                                    //proc.AddVarcharPara("@SegmentMandatory5", 100, Convert.ToString(ddlSegmentMandatory5.Value));


                                                    string SegmentMandatory1 = hdnSegmentMandatory1.Value;
                                                    string ddlSegmentMandatory1 = "";
                                                    if (SegmentMandatory1 != "")
                                                    {
                                                        ddlSegmentMandatory1 = String.Join(",", SegmentMandatory1);
                                                    }

                                                    string SegmentMandatory2 = hdnSegmentMandatory2.Value;
                                                    string ddlSegmentMandatory2 = "";
                                                    if (SegmentMandatory2 != "")
                                                    {
                                                        ddlSegmentMandatory2 = String.Join(",", SegmentMandatory2);
                                                    }

                                                    string SegmentMandatory3 = hdnSegmentMandatory3.Value;
                                                    string ddlSegmentMandatory3 = "";
                                                    if (SegmentMandatory3 != "")
                                                    {
                                                        ddlSegmentMandatory3 = String.Join(",", SegmentMandatory3);
                                                    }

                                                    string SegmentMandatory4 = hdnSegmentMandatory4.Value;
                                                    string ddlSegmentMandatory4 = "";
                                                    if (SegmentMandatory4 != "")
                                                    {
                                                        ddlSegmentMandatory4 = String.Join(",", SegmentMandatory4);
                                                    }

                                                    string SegmentMandatory5 = hdnSegmentMandatory5.Value;
                                                    string ddlSegmentMandatory5 = "";
                                                    if (SegmentMandatory5 != "")
                                                    {
                                                        ddlSegmentMandatory5 = String.Join(",", SegmentMandatory5);
                                                    }
                                                    proc.AddVarcharPara("@SegmentMandatory1", 100, Convert.ToString(ddlSegmentMandatory1));
                                                    proc.AddVarcharPara("@SegmentMandatory2", 100, Convert.ToString(ddlSegmentMandatory2));
                                                    proc.AddVarcharPara("@SegmentMandatory3", 100, Convert.ToString(ddlSegmentMandatory3));
                                                    proc.AddVarcharPara("@SegmentMandatory4", 100, Convert.ToString(ddlSegmentMandatory4));
                                                    proc.AddVarcharPara("@SegmentMandatory5", 100, Convert.ToString(ddlSegmentMandatory5));

                                                    int i = proc.RunActionQuery();

                                                }
                                            }

                                            catch (Exception ex)
                                            {
                                                throw ex;
                                            }

                                            finally
                                            {
                                                proc = null;
                                            }
                                        }
                                    }



                                    if ((hdnAutoNumStg.Value == "1") || (hdnAutoNumStg.Value == "LDAutoNum1") || (hdnAutoNumStg.Value == "AGAutoNum1") || (hdnAutoNumStg.Value == "TRAutoNum1") || (hdnAutoNumStg.Value == "RAAutoNum1"))
                                    {
                                        if (InternalId != "")
                                        {
                                            BusinessLogicLayer.DBEngine BEngine = new BusinessLogicLayer.DBEngine();
                                            DataTable dts = new DataTable();
                                            DataTable delete = new DataTable();
                                            dts = BEngine.GetDataTable("select isnull(cnt_UCC,'') cnt_UCC from tbl_master_contact where cnt_internalId='" + InternalId + "'");
                                            if (dts.Rows.Count == 1)
                                            {
                                                if (Convert.ToString(dts.Rows[0]["cnt_UCC"]) == "Auto")
                                                {
                                                    delete = BEngine.GetDataTable("delete from tbl_master_contact where cnt_internalId='" + InternalId + "'");
                                                    if (hdnAutoNumStg.Value == "LDAutoNum1")
                                                    {
                                                        LDtxt_CustDocNo.Text = "Auto";
                                                        LDtxt_CustDocNo.ClientEnabled = false;
                                                    }
                                                    else
                                                    {
                                                        txt_CustDocNo.Text = "Auto";
                                                        txt_CustDocNo.ClientEnabled = false;
                                                    }
                                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "jAlert('Either Unique ID Exists OR Unique ID Exhausted.')", true);
                                                }
                                            }
                                        }
                                    }



                                    if (Convert.ToString(Session["requesttype"]) == "Customer/Client")
                                    {
                                        Int32 rowsEffected = oDBEngine.SetFieldValue("tbl_master_contact", "cnt_mainAccount='" + hndTaxRates_MainAccount_hidden.Value + "',cnt_IdType=" + ddlIdType.SelectedItem.Value, " cnt_internalId='" + InternalId + "'");
                                        if (rowsEffected > 0)
                                        {
                                            oContactGeneralBL.Insert_DataByIDType(InternalId, ddlIdType.SelectedItem.Value, Uniquename);
                                        }
                                    }
                                    string ModuleType = Convert.ToString(Session["requesttype"]);

                                    if (ModuleType == "Customer/Client" && txtGSTIN2.Text.Trim() != "")
                                    {
                                        DataTable dt;
                                        string KeyValId = Convert.ToString(HttpContext.Current.Session["KeyVal"]);
                                        dt = UpdatePanNumber(KeyValId, txtGSTIN2.Text.Trim());

                                        
                                    }
                                    //Rev Tanmoy 09-08-2019 send SMS
                                    String msgResponse = "";
                                    if (msgBody != "")
                                    {
                                        msgResponse = ObjSms.sendSMS(txtSMSPhnNo.Text, msgBody);
                                    }

                                    if (Convert.ToString(Session["requesttype"]) == "Lead")
                                    {
                                        int stus = ObjSms.SmsStatusSave(msgBody, "Lead", msgResponse, txtSMSPhnNo.Text);
                                        if (msgResponse == "")
                                        {
                                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "jAlert('SMS Configuration Need.')", true);
                                        }
                                    }
                                    //End Rev Tanmoy

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
                                            oContactGeneralBL.Insert_DataByIDType(InternalId, ddlIdType.SelectedItem.Value, Uniquename);
                                        }
                                    }
                                    string ModuleType4 = Convert.ToString(Session["requesttype"]);

                                    if (ModuleType4 == "Customer/Client" && txtGSTIN2.Text.Trim() != "")
                                    {
                                        string KeyValId = Convert.ToString(HttpContext.Current.Session["KeyVal"]);
                                        DataTable dt = UpdatePanNumber(KeyValId, txtGSTIN2.Text.Trim());
                                    }

                                    HttpContext.Current.Session["KeyVal_InternalID"] = InternalId;


                                    if (Convert.ToString(Session["requesttype"]) == "Transporter")
                                    {
                                        Int32 rowsEffected = oDBEngine.SetFieldValue("tbl_master_contact", "cnt_mainAccount='" + hndTaxRates_MainAccount_hidden.Value + "',Enrolment_ID='" + TransporterEnrollmentId + "'", " cnt_internalId='" + InternalId + "'");



                                        string vehicleNos = VehicleNo_hidden.Value;
                                        if (Convert.ToString(cmbLegalStatus.SelectedValue) == "54")//Local
                                        {
                                            if (!string.IsNullOrEmpty(vehicleNos))
                                            {
                                                string[] _vehicleNo = vehicleNos.Split(',');

                                                foreach (var word in _vehicleNo)
                                                {
                                                    if (Regex.IsMatch(word, @"^[a-zA-Z0-9]+$"))
                                                    {

                                                    }
                                                    else
                                                    {

                                                        Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Special Characters are not allowed in Vehicle No.')</script>");
                                                        return;

                                                    }
                                                }
                                            }



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
                                    string ModuleType7 = Convert.ToString(Session["requesttype"]);

                                    if (ModuleType7 == "Customer/Client" && txtGSTIN2.Text.Trim() != "")
                                    {
                                        string KeyValId = Convert.ToString(HttpContext.Current.Session["KeyVal"]);
                                        DataTable dt = UpdatePanNumber(KeyValId, txtGSTIN2.Text.Trim());
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
                                            oDBEngine.InsurtFieldValue("tbl_master_contactexchange", "crg_cntid,crg_company,crg_exchange,crg_tcode,createdate,createuser,crg_sttpattern,crg_sttwap,crg_SegmentID", "'" + Convert.ToString(InternalId).Trim() + "','" + HttpContext.Current.Session["LastCompany"].ToString().Trim() + "','" + Convert.ToString(segregis).Trim() + "','" + Uniquename + "','" + Convert.ToString(oDBEngine.GetDate()) + "','" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "','D','W','" + Convert.ToString(HttpContext.Current.Session["usersegid"]).Trim() + "'");
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
                             //REV SRIJEETA  mantis issue 0024515
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
                            //                               , GSTIN, hidAssociatedEmp.Value, txtPname.Text, cmbTDS.Value.ToString(), Convert.ToString(cmbTaxdeducteedType.Value), Convert.ToInt32(TCSApplicable.Value), txtGSTIN2.Text.Trim(),Convert.ToString(cmbTransCategory.Value), Convert.ToString(ddlServiceBranch.SelectedValue)
                            //               );
                            string InternalId = oContactGeneralBL.Insert_ContactGeneral(dd, "","1", CmbSalutation.SelectedItem.Value,
                                                          txtFirstNmae.Text.Trim(), txtMiddleName.Text.Trim(), txtLastName.Text.Trim(),
                                                          txtAliasName.Text.Trim(), cmbBranch.SelectedItem.Value, cmbGender.SelectedItem.Value,
                                                          cmbMaritalStatus.SelectedItem.Value, dtDob, dtanniversary, cmbLegalStatus.SelectedItem.Value,
                                                          cmbEducation.SelectedItem.Value, cmbProfession.SelectedItem.Value, txtOrganization.Text.Trim(),
                                                          cmbJobResponsibility.SelectedItem.Value, cmbDesignation.SelectedItem.Value, cmbIndustry.SelectedItem.Value,
                                                          cmbSource.SelectedItem.Value, txtReferedBy.Text.Trim(), txtRPartner_hidden.Text.Trim(), ContType,
                                                          cmbContactStatus.SelectedItem.Value, dtReg, cmbRating.SelectedItem.Value, TxtContactStatus.Text.Trim(),
                                                          cmbBloodgroup.SelectedItem.Value, webLogin, Password, Convert.ToString(HttpContext.Current.Session["userid"]), vPlaceincop,
                                                          dtBusiness, other, country, Creditcard, Convert.ToInt32(txtcreditDays.Text.Trim()), Convert.ToDecimal(txtCreditLimit.Text.Trim()), Convert.ToString(cmbContactStatusclient.SelectedItem.Value)
                                                          , GSTIN, hidAssociatedEmp.Value, txtPname.Text, cmbTDS.Value.ToString(), Convert.ToString(cmbTaxdeducteedType.Value), Convert.ToInt32(TCSApplicable.Value), txtGSTIN2.Text.Trim(), Convert.ToString(cmbTransCategory.Value), Convert.ToString(ddlServiceBranch.SelectedValue)
                                          );
                            //end of REV SRIJEETA  mantis issue 0024515

                            DataTable udfTable = (DataTable)Session["UdfDataOnAdd"];
                            //Rev Subhra 03-06-2019
                            if (Convert.ToString(Session["requesttype"]) == "Lead")
                            {
                                Int32 rowsEffected = oDBEngine.SetFieldValue("tbl_master_contact", "EnteredDate='" + dtenteredon + "' ", " cnt_internalId='" + InternalId + "'");
                            }
                            string ModuleType = Convert.ToString(Session["requesttype"]);

                            if (ModuleType == "Customer/Client" && txtGSTIN2.Text.Trim() != "")
                            {
                                string KeyValId = Convert.ToString(HttpContext.Current.Session["KeyVal"]);
                                DataTable dt = UpdatePanNumber(KeyValId, txtGSTIN2.Text.Trim());
                            }
                            //End of Rev 03-06-2019

                            //Rev Tanmoy 09-08-2019 send SMS
                            String msgResponse = "";
                            if (msgBody != "")
                            {
                                msgResponse = ObjSms.sendSMS(txtSMSPhnNo.Text, msgBody);
                            }
                            int stus = ObjSms.SmsStatusSave(msgBody, "Lead", msgResponse, txtSMSPhnNo.Text);
                            if (msgResponse == "")
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "jAlert('SMS Configuration Need.')", true);
                            }
                            //End Rev Tanmoy

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
                                String value = "cnt_ucc='" + Uniquename + "', cnt_salutation=" + CmbSalutation.SelectedItem.Value + ",  cnt_firstName='" + txtFirstNmae.Text + "', cnt_middleName='" + txtMiddleName.Text + "', cnt_lastName='" + txtLastName.Text + "', cnt_shortName='" + txtAliasName.Text + "', cnt_branchId=" + cmbBranch.SelectedItem.Value + ", cnt_sex=" + cmbGender.SelectedItem.Value + ", cnt_maritalStatus=" + cmbMaritalStatus.SelectedItem.Value + ", cnt_DOB='" + txtDOB.Value + "', cnt_anniversaryDate='" + txtAnniversary.Value + "', cnt_legalStatus=" + cmbLegalStatus.SelectedItem.Value + ", cnt_education=" + cmbEducation.SelectedItem.Value + ", cnt_profession=" + cmbProfession.SelectedItem.Value + ", cnt_organization='" + txtOrganization.Text + "', cnt_jobResponsibility=" + cmbJobResponsibility.SelectedItem.Value + ", cnt_designation=" + cmbDesignation.SelectedItem.Value + ", cnt_industry=" + cmbIndustry.SelectedItem.Value + ", cnt_contactSource=" + cmbSource.SelectedItem.Value + ", cnt_referedBy='" + txtReferedBy.Text + "', cnt_contactType='" + ContType + "', cnt_contactStatus=" + cmbContactStatus.SelectedItem.Value + ",cnt_RegistrationDate='" + txtDateRegis.Value + "',cnt_rating='" + cmbRating.SelectedItem.Value + "',cnt_reason='" + TxtContactStatus.Text + "',cnt_bloodgroup='" + cmbBloodgroup.SelectedItem.Value + "',WebLogIn='" + webLogin + "',PassWord='" + Password + "', lastModifyDate ='" + today + "', lastModifyUser=" + HttpContext.Current.Session["userid"]; // + Session["userid"]
                                oDBEngine.SetFieldValue("tbl_master_contact", value, " cnt_id=" + HttpContext.Current.Session["KeyVal"]);
                            }
                            else
                            {
                                String value = "cnt_ucc='" + Uniquename + "', cnt_salutation=" + CmbSalutation.SelectedItem.Value + ",  cnt_firstName='" + txtFirstNmae.Text + "', cnt_middleName='" + txtMiddleName.Text + "', cnt_lastName='" + txtLastName.Text + "', cnt_shortName='" + txtAliasName.Text + "', cnt_branchId=" + cmbBranch.SelectedItem.Value + ", cnt_sex=" + cmbGender.SelectedItem.Value + ", cnt_maritalStatus=" + cmbMaritalStatus.SelectedItem.Value + ", cnt_DOB='" + txtDOB.Value + "', cnt_anniversaryDate='" + txtAnniversary.Value + "', cnt_legalStatus=" + cmbLegalStatus.SelectedItem.Value + ", cnt_education=" + cmbEducation.SelectedItem.Value + ", cnt_profession=" + cmbProfession.SelectedItem.Value + ", cnt_organization='" + txtOrganization.Text + "', cnt_jobResponsibility=" + cmbJobResponsibility.SelectedItem.Value + ", cnt_designation=" + cmbDesignation.SelectedItem.Value + ", cnt_industry=" + cmbIndustry.SelectedItem.Value + ", cnt_contactSource=" + cmbSource.SelectedItem.Value + ", cnt_referedBy='" + txtReferedBy_hidden.Text + "', cnt_contactType='" + ContType + "', cnt_contactStatus=" + cmbContactStatus.SelectedItem.Value + ",cnt_RegistrationDate='" + txtDateRegis.Value + "',cnt_rating='" + cmbRating.SelectedItem.Value + "',cnt_reason='" + TxtContactStatus.Text + "',cnt_bloodgroup='" + cmbBloodgroup.SelectedItem.Value + "',WebLogIn='" + webLogin + "',PassWord='" + Password + "', lastModifyDate ='" + today + "', lastModifyUser=" + HttpContext.Current.Session["userid"]; // + Session["userid"]

                                oDBEngine.SetFieldValue("tbl_master_contact", value, " cnt_id=" + HttpContext.Current.Session["KeyVal"]);

                                Page.ClientScript.RegisterStartupScript(GetType(), "JScript3", "<script language='javascript'>PageLoad();</script>");
                                Response.Redirect("Contact_general.aspx?id=" + HttpContext.Current.Session["KeyVal"], false);

                                return;
                            }
                            string ModuleType = Convert.ToString(Session["requesttype"]);

                            if (ModuleType == "Customer/Client" && txtGSTIN2.Text.Trim() != "")
                            {
                                string KeyValId = Convert.ToString(HttpContext.Current.Session["KeyVal"]);
                                DataTable dt = UpdatePanNumber(KeyValId, txtGSTIN2.Text.Trim());
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
                        DataTable dt1 = oDBEngine.GetDataTable("tbl_master_contactExchange,tbl_master_contact", "top 1 cnt_ucc,crg_tcode", "cnt_ucc='" + Uniquename.ToString() + "' or crg_tcode='" + Uniquename.ToString() + "'  and cnt_id='" + Convert.ToString(HttpContext.Current.Session["KeyVal"]) + "'");
                        DataTable dt2 = oDBEngine.GetDataTable("tbl_master_contact", " cnt_id", "cnt_id='" + Convert.ToString(HttpContext.Current.Session["KeyVal"]) + "'");
                        string[,] abcd = oDBEngine.GetFieldValue("tbl_master_contactExchange,tbl_master_contact", "top 1 cnt_ucc,crg_tcode", "cnt_ucc='" + Uniquename.ToString() + "' or crg_tcode='" + Uniquename.ToString() + "'  and cnt_id='" + Convert.ToString(HttpContext.Current.Session["KeyVal"]) + "'", 2);
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



                                value = "Statustype='" + status + "',cnt_ucc='" + Uniquename + "', cnt_salutation=" + LDCmbSalutation.SelectedItem.Value + ",  cnt_firstName='" + LDtxtFirstNmae.Text + "', cnt_middleName='" + LDtxtMiddleName.Text + "', cnt_lastName='" + LDtxtLastName.Text + "', cnt_shortName='" + LDtxtAliasName.Text + "', cnt_branchId=" + LDcmbBranch.SelectedItem.Value + ", cnt_sex=" + LDcmbGender.SelectedItem.Value + ", cnt_maritalStatus=" + LDcmbMaritalStatus.SelectedItem.Value + ", cnt_DOB='" + LDtxtDOB.Value + "', cnt_anniversaryDate='" + LDtxtAnniversary.Value + "', cnt_legalStatus=" + LDcmbLegalStatus.SelectedItem.Value + ", cnt_education=" + LDcmbEducation.SelectedItem.Value + ", cnt_profession=" + LDcmbProfession.SelectedItem.Value + ", cnt_organization='" + LDtxtOrganization.Text + "', cnt_jobResponsibility=" + LDcmbJobResponsibility.SelectedItem.Value + ", cnt_designation=" + LDcmbDesignation.SelectedItem.Value + ", cnt_industry=" + LDcmbIndustry.SelectedItem.Value + ", cnt_contactSource=" + LDcmbSource.SelectedItem.Value + ", cnt_referedBy='" + LDtxtReferedBy_hidden.Value + "', cnt_contactType='" + ContType + "', cnt_contactStatus=" + LDcmbContactStatus.SelectedItem.Value + ",cnt_RegistrationDate='',cnt_rating='" + LDcmbRating.SelectedItem.Value + "',cnt_reason='',cnt_bloodgroup='" + LDcmbBloodgroup.SelectedItem.Value + "',WebLogIn='" + webLogin + "',PassWord='" + Password + "', lastModifyDate ='" + today + "',cnt_PlaceOfIncorporation='',cnt_BusinessComncDate='',cnt_OtherOccupation='" + other + "', lastModifyUser=" + HttpContext.Current.Session["userid"]; // + Session["userid"]


                                oDBEngine.SetFieldValue("tbl_master_contact", value, " cnt_id=" + HttpContext.Current.Session["KeyVal"]);
                                string ModuleType = Convert.ToString(Session["requesttype"]);

                                if (ModuleType == "Customer/Client" && txtGSTIN2.Text.Trim() != "")
                                {
                                    string KeyValId = Convert.ToString(HttpContext.Current.Session["KeyVal"]);
                                    DataTable dt = UpdatePanNumber(KeyValId, txtGSTIN2.Text.Trim());
                                }
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

                                                value = "Statustype='" + status + "',cnt_ucc='" + Uniquename + "', cnt_salutation=" + LDCmbSalutation.SelectedItem.Value + ",  cnt_firstName='" + LDtxtFirstNmae.Text + "', cnt_middleName='" + LDtxtMiddleName.Text + "', cnt_lastName='" + LDtxtLastName.Text + "', cnt_shortName='" + LDtxtAliasName.Text + "', cnt_branchId=" + LDcmbBranch.SelectedItem.Value + ", cnt_sex=" + LDcmbGender.SelectedItem.Value + ", cnt_maritalStatus=" + LDcmbMaritalStatus.SelectedItem.Value + ", cnt_DOB='" + LDtxtDOB.Value + "', cnt_anniversaryDate='" + LDtxtAnniversary.Value + "', cnt_legalStatus=" + LDcmbLegalStatus.SelectedItem.Value + ", cnt_education=" + LDcmbEducation.SelectedItem.Value + ", cnt_profession=" + LDcmbProfession.SelectedItem.Value + ", cnt_organization='" + LDtxtOrganization.Text + "', cnt_jobResponsibility=" + LDcmbJobResponsibility.SelectedItem.Value + ", cnt_designation=" + LDcmbDesignation.SelectedItem.Value + ", cnt_industry=" + LDcmbIndustry.SelectedItem.Value + ", cnt_contactSource=" + LDcmbSource.SelectedItem.Value + ", cnt_referedBy='" + LDtxtReferedBy_hidden.Value + "', cnt_contactType='" + ContType + "', cnt_contactStatus=" + LDcmbContactStatus.SelectedItem.Value + ",cnt_RegistrationDate='',cnt_rating='" + LDcmbRating.SelectedItem.Value + "',cnt_reason='',cnt_bloodgroup='" + LDcmbBloodgroup.SelectedItem.Value + "',WebLogIn='" + webLogin + "',PassWord='" + Password + "', lastModifyDate ='" + today + "',cnt_PlaceOfIncorporation='',cnt_BusinessComncDate='',cnt_OtherOccupation='" + other + "', lastModifyUser='" + HttpContext.Current.Session["userid"] + "'";


                                            }
                                            else
                                            {
                                                value = "Statustype=" + status + ",cnt_ucc='" + Uniquename + "', cnt_salutation=" + LDCmbSalutation.SelectedItem.Value + ",  cnt_firstName='" + LDtxtFirstNmae.Text + "', cnt_middleName='" + LDtxtMiddleName.Text + "', cnt_lastName='" + LDtxtLastName.Text + "', cnt_shortName='" + LDtxtAliasName.Text + "', cnt_branchId=" + LDcmbBranch.SelectedItem.Value + ", cnt_sex=" + LDcmbGender.SelectedItem.Value + ", cnt_maritalStatus=" + LDcmbMaritalStatus.SelectedItem.Value + ", cnt_DOB='" + LDtxtDOB.Value + "', cnt_anniversaryDate='" + LDtxtAnniversary.Value + "', cnt_legalStatus=" + LDcmbLegalStatus.SelectedItem.Value + ", cnt_education=" + LDcmbEducation.SelectedItem.Value + ", cnt_profession=" + LDcmbProfession.SelectedItem.Value + ", cnt_organization='" + LDtxtOrganization.Text + "', cnt_jobResponsibility=" + LDcmbJobResponsibility.SelectedItem.Value + ", cnt_designation=" + LDcmbDesignation.SelectedItem.Value + ", cnt_industry=" + LDcmbIndustry.SelectedItem.Value + ", cnt_contactSource=" + LDcmbSource.SelectedItem.Value + ", cnt_referedBy='" + LDtxtReferedBy_hidden.Value + "', cnt_contactType='" + ContType + "', cnt_contactStatus=" + LDcmbContactStatus.SelectedItem.Value + ",cnt_RegistrationDate='',cnt_rating='" + LDcmbRating.SelectedItem.Value + "',cnt_reason='',cnt_bloodgroup='" + LDcmbBloodgroup.SelectedItem.Value + "',WebLogIn='" + webLogin + "',PassWord='" + Password + "', lastModifyDate ='" + today + "',cnt_PlaceOfIncorporation='',cnt_nationality='" + Convert.ToInt32(Convert.ToString(country)) + "',cnt_BusinessComncDate='',cnt_OtherOccupation='" + other + "', lastModifyUser='" + HttpContext.Current.Session["userid"] + "'";
                                            }

                                            oDBEngine.SetFieldValue("tbl_master_contact", value, " cnt_id=" + HttpContext.Current.Session["KeyVal"]);
                                            string ModuleType = Convert.ToString(Session["requesttype"]);

                                            if (ModuleType == "Customer/Client" && txtGSTIN2.Text.Trim() != "")
                                            {
                                                string KeyValId = Convert.ToString(HttpContext.Current.Session["KeyVal"]);
                                                DataTable dt = UpdatePanNumber(KeyValId, txtGSTIN2.Text.Trim());
                                            }
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

                                            ////Rev Tanmoy 09-08-2019 send SMS
                                            //String msgResponse = "";
                                            //if (msgBody != "")
                                            //{
                                            //    msgResponse = ObjSms.sendSMS(txtSMSPhnNo.Text, msgBody);
                                            //}

                                            //int stus = ObjSms.SmsStatusSave(msgBody, "Lead", msgResponse, txtSMSPhnNo.Text);
                                            //if (msgResponse == "")
                                            //{
                                            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('SMS Configuration Need.')", true);
                                            //}
                                            ////End Rev Tanmoy

                                            return;

                                        }
                                    }
                                }

                                if (dt1.Rows.Count <= 0)
                                {
                                    String value = "Statustype='" + status + "',cnt_ucc='" + Uniquename + "', cnt_salutation=" + LDCmbSalutation.SelectedItem.Value + ",  cnt_firstName='" + LDtxtFirstNmae.Text + "', cnt_middleName='" + LDtxtMiddleName.Text + "', cnt_lastName='" + LDtxtLastName.Text + "', cnt_shortName='" + LDtxtAliasName.Text + "', cnt_branchId=" + LDcmbBranch.SelectedItem.Value + ", cnt_sex=" + LDcmbGender.SelectedItem.Value + ", cnt_maritalStatus=" + LDcmbMaritalStatus.SelectedItem.Value + ", cnt_DOB='" + LDtxtDOB.Value + "', cnt_anniversaryDate='" + LDtxtAnniversary.Value + "', cnt_legalStatus=" + LDcmbLegalStatus.SelectedItem.Value + ", cnt_education=" + LDcmbEducation.SelectedItem.Value + ", cnt_profession=" + LDcmbProfession.SelectedItem.Value + ", cnt_organization='" + LDtxtOrganization.Text + "', cnt_jobResponsibility=" + LDcmbJobResponsibility.SelectedItem.Value + ", cnt_designation=" + LDcmbDesignation.SelectedItem.Value + ", cnt_industry=" + LDcmbIndustry.SelectedItem.Value + ", cnt_contactSource=" + LDcmbSource.SelectedItem.Value + ", cnt_referedBy='" + LDtxtReferedBy_hidden.Value + "', cnt_contactType='" + ContType + "', cnt_contactStatus=" + LDcmbContactStatus.SelectedItem.Value + ",cnt_RegistrationDate='',cnt_rating='" + LDcmbRating.SelectedItem.Value + "',cnt_reason='',cnt_bloodgroup='" + LDcmbBloodgroup.SelectedItem.Value + "',WebLogIn='" + webLogin + "',PassWord='" + Password + "', lastModifyDate ='" + today + "', lastModifyUser=" + HttpContext.Current.Session["userid"]; // + Session["userid"]
                                    oDBEngine.SetFieldValue("tbl_master_contact", value, " cnt_id=" + HttpContext.Current.Session["KeyVal"]);
                                    string ModuleType = Convert.ToString(Session["requesttype"]);

                                    if (ModuleType == "Customer/Client" && txtGSTIN2.Text.Trim() != "")
                                    {
                                        string KeyValId = Convert.ToString(HttpContext.Current.Session["KeyVal"]);
                                        DataTable dt = UpdatePanNumber(KeyValId, txtGSTIN2.Text.Trim());
                                    }
                                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Saved Succesfully')</script>");

                                    ////Rev Tanmoy 09-08-2019 send SMS
                                    //String msgResponse = "";
                                    //if (msgBody != "")
                                    //{
                                    //    msgResponse = ObjSms.sendSMS(txtSMSPhnNo.Text, msgBody);
                                    //}

                                    //int stus = ObjSms.SmsStatusSave(msgBody, "Lead", msgResponse, txtSMSPhnNo.Text);
                                    //if (msgResponse == "")
                                    //{
                                    //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('SMS Configuration Need.')", true);
                                    //}
                                    ////End Rev Tanmoy

                                    return;
                                }
                                else
                                {
                                    if (hdnAutoNumStg.Value == "LDAutoNum0")
                                    {
                                        Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('You have entered duplicate Unique ID.')</script>");
                                        txtClentUcc.Text = "";
                                        txtClentUcc.Focus();
                                        return;
                                    }
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

                                    // chinmoy edited for Auto Number scheme start
                                    int numberId = 0;
                                    string LDUccName = "";
                                    if (hdnAutoNumStg.Value == "1" && ContType == "CL")
                                    {
                                        numberId = Convert.ToInt32(hdnNumberingId.Value.Trim());
                                        LDUccName = hddnDocNo.Value.Trim();

                                    }
                                    else if (hdnAutoNumStg.Value == "LDAutoNum1" && ContType == "LD")
                                    {
                                        numberId = Convert.ToInt32(hdnNumberingId.Value.Trim());
                                        LDUccName = hddnDocNo.Value.Trim();

                                    }
                                    else if (hdnAutoNumStg.Value == "LDAutoNum0" && ContType == "LD")
                                    {
                                        LDUccName = LDtxtClentUcc.Text.Trim();
                                    }

                                    //End
                                    //REV SRIJEETA  mantis issue 0024515
                                    //string InternalId = oContactGeneralBL.Insert_ContactGeneral(dd, LDUccName, LDCmbSalutation.SelectedItem.Value,
                                    //                    LDtxtFirstNmae.Text.Trim(), LDtxtMiddleName.Text.Trim(), LDtxtLastName.Text.Trim(),
                                    //                    LDtxtAliasName.Text.Trim(), LDcmbBranch.SelectedItem.Value, LDcmbGender.SelectedItem.Value,
                                    //                    LDcmbMaritalStatus.SelectedItem.Value, dtDob, dtanniversary, LDcmbLegalStatus.SelectedItem.Value,
                                    //                    LDcmbEducation.SelectedItem.Value, LDcmbProfession.SelectedItem.Value, LDtxtOrganization.Text.Trim(),
                                    //                    LDcmbJobResponsibility.SelectedItem.Value, LDcmbDesignation.SelectedItem.Value, LDcmbIndustry.SelectedItem.Value,
                                    //                    LDcmbSource.SelectedItem.Value, LDtxtReferedBy_hidden.Value, txtRPartner_hidden.Text.Trim(), ContType,
                                    //                    LDcmbContactStatus.SelectedItem.Value, dtReg, LDcmbRating.SelectedItem.Value, TxtContactStatus.Text.Trim(),
                                    //                    LDcmbBloodgroup.SelectedItem.Value, webLogin, Password, Convert.ToString(HttpContext.Current.Session["userid"]), vPlaceincop,
                                    //                    dtBusiness, other, country, Creditcard, Convert.ToInt32(txtcreditDays.Text.Trim()), Convert.ToDecimal(txtCreditLimit.Text.Trim()), Convert.ToString(cmbContactStatusclient.SelectedItem.Value)
                                    //                    , GSTIN, hidAssociatedEmp.Value, txtPname.Text, null, null, numberId
                                    //    );
                                    string InternalId = oContactGeneralBL.Insert_ContactGeneral(dd, LDUccName,"1", LDCmbSalutation.SelectedItem.Value,
                                                        LDtxtFirstNmae.Text.Trim(), LDtxtMiddleName.Text.Trim(), LDtxtLastName.Text.Trim(),
                                                        LDtxtAliasName.Text.Trim(), LDcmbBranch.SelectedItem.Value, LDcmbGender.SelectedItem.Value,
                                                        LDcmbMaritalStatus.SelectedItem.Value, dtDob, dtanniversary, LDcmbLegalStatus.SelectedItem.Value,
                                                        LDcmbEducation.SelectedItem.Value, LDcmbProfession.SelectedItem.Value, LDtxtOrganization.Text.Trim(),
                                                        LDcmbJobResponsibility.SelectedItem.Value, LDcmbDesignation.SelectedItem.Value, LDcmbIndustry.SelectedItem.Value,
                                                        LDcmbSource.SelectedItem.Value, LDtxtReferedBy_hidden.Value, txtRPartner_hidden.Text.Trim(), ContType,
                                                        LDcmbContactStatus.SelectedItem.Value, dtReg, LDcmbRating.SelectedItem.Value, TxtContactStatus.Text.Trim(),
                                                        LDcmbBloodgroup.SelectedItem.Value, webLogin, Password, Convert.ToString(HttpContext.Current.Session["userid"]), vPlaceincop,
                                                        dtBusiness, other, country, Creditcard, Convert.ToInt32(txtcreditDays.Text.Trim()), Convert.ToDecimal(txtCreditLimit.Text.Trim()), Convert.ToString(cmbContactStatusclient.SelectedItem.Value)
                                                        , GSTIN, hidAssociatedEmp.Value, txtPname.Text, null, null, numberId
                                        );
                                    //REV SRIJEETA  mantis issue 0024515
                                    //chinmoy  31-03-2020 start

                                    if ((hdnAutoNumStg.Value == "1") || (hdnAutoNumStg.Value == "LDAutoNum1") || (hdnAutoNumStg.Value == "AGAutoNum1") || (hdnAutoNumStg.Value == "TRAutoNum1") || (hdnAutoNumStg.Value == "RAAutoNum1"))
                                    {
                                        if (InternalId != "")
                                        {

                                            string RequesttttType = Convert.ToString(Session["requesttype"]);
                                            if (RequesttttType == "Customer/Client")
                                            {
                                                CommonBL ComnonBL = new CommonBL();


                                                string SyncCustomertoFSMWhileCreating = ComnonBL.GetSystemSettingsResult("SyncCustomertoFSMWhileCreating");
                                                if (SyncCustomertoFSMWhileCreating.ToUpper() == "YES")
                                                {
                                                    CustomerSync(InternalId);
                                                }
                                            }

                                            BusinessLogicLayer.DBEngine BEngine = new BusinessLogicLayer.DBEngine();
                                            DataTable dts = new DataTable();
                                            DataTable delete = new DataTable();
                                            dts = BEngine.GetDataTable("select isnull(cnt_UCC,'') cnt_UCC from tbl_master_contact where cnt_internalId='" + InternalId + "'");
                                            if (dts.Rows.Count == 1)
                                            {
                                                if (Convert.ToString(dts.Rows[0]["cnt_UCC"]) == "Auto")
                                                {
                                                    delete = BEngine.GetDataTable("delete from tbl_master_contact where cnt_internalId='" + InternalId + "'");
                                                    if (hdnAutoNumStg.Value == "LDAutoNum1")
                                                    {
                                                        LDtxt_CustDocNo.Text = "Auto";
                                                        LDtxt_CustDocNo.ClientEnabled = false;
                                                    }
                                                    else
                                                    {
                                                        txt_CustDocNo.Text = "Auto";
                                                        txt_CustDocNo.ClientEnabled = false;
                                                    }
                                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "jAlert('Either Unique ID Exists OR Unique ID Exhausted.')", true);
                                                }
                                            }
                                        }
                                    }
                                    //End

                                    //Rev Subhra 03-06-2019
                                    if (Convert.ToString(Session["requesttype"]) == "Lead")
                                    {
                                        Int32 rowsEffected = oDBEngine.SetFieldValue("tbl_master_contact", "EnteredDate='" + dtenteredon + "' ", " cnt_internalId='" + InternalId + "'");
                                    }
                                    string ModuleType = Convert.ToString(Session["requesttype"]);

                                    if (ModuleType == "Customer/Client" && txtGSTIN2.Text.Trim() != "")
                                    {
                                        string KeyValId = Convert.ToString(HttpContext.Current.Session["KeyVal"]);
                                        DataTable dt = UpdatePanNumber(KeyValId, txtGSTIN2.Text.Trim());
                                    }
                                    //End of Rev 03-06-2019

                                    //Rev Tanmoy 09-08-2019 send SMS
                                    String msgResponse = "";
                                    if (msgBody != "")
                                    {
                                        msgResponse = ObjSms.sendSMS(txtSMSPhnNo.Text, msgBody);
                                    }
                                    int stus = ObjSms.SmsStatusSave(msgBody, "Lead", msgResponse, txtSMSPhnNo.Text);
                                    if (msgResponse == "")
                                    {
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "jAlert('SMS Configuration Need.')", true);
                                    }
                                    //End Rev Tanmoy

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
                                            oDBEngine.InsurtFieldValue("tbl_master_contactexchange", "crg_cntid,crg_company,crg_exchange,crg_tcode,createdate,createuser,crg_sttpattern,crg_sttwap,crg_SegmentID", "'" + Convert.ToString(InternalId).Trim() + "','" + Convert.ToString(HttpContext.Current.Session["LastCompany"]).Trim() + "','" + Convert.ToString(segregis).Trim() + "','" + Uniquename.ToString().Trim() + "','" + Convert.ToString(oDBEngine.GetDate()) + "','" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "','D','W','" + Convert.ToString(HttpContext.Current.Session["usersegid"]).Trim() + "'");
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
                            //REV SRIJEETA  mantis issue 0024515
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
                            string InternalId = oContactGeneralBL.Insert_ContactGeneral(dd, "","1", CmbSalutation.SelectedItem.Value,
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
                            //end of rev srijeeta mantis issue 0024515

                            //Rev Subhra 03-06-2019
                            if (Convert.ToString(Session["requesttype"]) == "Lead")
                            {
                                Int32 rowsEffected = oDBEngine.SetFieldValue("tbl_master_contact", "EnteredDate='" + dtenteredon + "' ", " cnt_internalId='" + InternalId + "'");
                            }
                            //End of Rev 03-06-2019
                            string ModuleType = Convert.ToString(Session["requesttype"]);

                            if (ModuleType == "Customer/Client" && txtGSTIN2.Text.Trim() != "")
                            {
                                string KeyValId = Convert.ToString(HttpContext.Current.Session["KeyVal"]);
                                DataTable dt = UpdatePanNumber(KeyValId, txtGSTIN2.Text.Trim());
                            }
                            //Rev Tanmoy 09-08-2019 send SMS
                            String msgResponse = "";
                            if (msgBody != "")
                            {
                                msgResponse = ObjSms.sendSMS(txtSMSPhnNo.Text, msgBody);
                            }
                            int stus = ObjSms.SmsStatusSave(msgBody, "Lead", msgResponse, txtSMSPhnNo.Text);
                            if (msgResponse == "")
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "jAlert('SMS Configuration Need.')", true);
                            }
                            //End Rev Tanmoy

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
                                    oDBEngine.InsurtFieldValue("tbl_master_contactexchange", "crg_cntid,crg_company,crg_exchange,crg_tcode,createdate,createuser,crg_sttpattern,crg_sttwap,crg_SegmentID", "'" + Convert.ToString(InternalId).Trim() + "','" + Convert.ToString(HttpContext.Current.Session["LastCompany"]).Trim() + "','" + Convert.ToString(segregis).Trim() + "','" + Uniquename.ToString().Trim() + "','" + oDBEngine.GetDate().ToString() + "','" + Convert.ToString(HttpContext.Current.Session["userid"]).Trim() + "','D','W','" + Convert.ToString(HttpContext.Current.Session["usersegid"]).Trim() + "'");
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
                                String value = "cnt_ucc='" + Uniquename + "', cnt_salutation=" + LDCmbSalutation.SelectedItem.Value + ",  cnt_firstName='" + LDtxtFirstNmae.Text + "', cnt_middleName='" + LDtxtMiddleName.Text + "', cnt_lastName='" + LDtxtLastName.Text + "', cnt_shortName='" + LDtxtAliasName.Text + "',cnt_PrintNameToCheque='" + txtPname.Text + "', cnt_branchId=" + LDcmbBranch.SelectedItem.Value + ", cnt_sex=" + LDcmbGender.SelectedItem.Value + ", cnt_maritalStatus=" + LDcmbMaritalStatus.SelectedItem.Value + ", cnt_DOB='" + LDtxtDOB.Value + "', cnt_anniversaryDate='" + LDtxtAnniversary.Value + "', cnt_legalStatus=" + LDcmbLegalStatus.SelectedItem.Value + ", cnt_education=" + LDcmbEducation.SelectedItem.Value + ", cnt_profession=" + LDcmbProfession.SelectedItem.Value + ", cnt_organization='" + LDtxtOrganization.Text + "', cnt_jobResponsibility=" + LDcmbJobResponsibility.SelectedItem.Value + ", cnt_designation=" + LDcmbDesignation.SelectedItem.Value + ", cnt_industry=" + LDcmbIndustry.SelectedItem.Value + ", cnt_contactSource=" + LDcmbSource.SelectedItem.Value + ", cnt_referedBy='" + LDtxtReferedBy_hidden.Value + "', cnt_contactType='" + ContType + "', cnt_contactStatus=" + LDcmbContactStatus.SelectedItem.Value + ",cnt_RegistrationDate='',cnt_rating='" + LDcmbRating.SelectedItem.Value + "',cnt_reason='',cnt_bloodgroup='" + LDcmbBloodgroup.SelectedItem.Value + "',WebLogIn='" + webLogin + "',PassWord='" + Password + "', lastModifyDate ='" + today + "', lastModifyUser=" + HttpContext.Current.Session["userid"]; // + Session["userid"]
                                oDBEngine.SetFieldValue("tbl_master_contact", value, " cnt_id=" + HttpContext.Current.Session["KeyVal"]);
                            }
                            else
                            {
                                String value = "Statustype='" + status + "',cnt_ucc='" + Uniquename + "', cnt_salutation=" + LDCmbSalutation.SelectedItem.Value + ",  cnt_firstName='" + LDtxtFirstNmae.Text + "', cnt_middleName='" + LDtxtMiddleName.Text + "', cnt_lastName='" + LDtxtLastName.Text + "', cnt_shortName='" + LDtxtAliasName.Text + "', cnt_PrintNameToCheque='" + txtPname.Text + "',cnt_branchId=" + LDcmbBranch.SelectedItem.Value + ", cnt_sex=" + LDcmbGender.SelectedItem.Value + ", cnt_maritalStatus=" + LDcmbMaritalStatus.SelectedItem.Value + ", cnt_DOB='" + LDtxtDOB.Value + "', cnt_anniversaryDate='" + LDtxtAnniversary.Value + "', cnt_legalStatus=" + LDcmbLegalStatus.SelectedItem.Value + ", cnt_education=" + LDcmbEducation.SelectedItem.Value + ", cnt_profession=" + LDcmbProfession.SelectedItem.Value + ", cnt_organization='" + LDtxtOrganization.Text + "', cnt_jobResponsibility=" + LDcmbJobResponsibility.SelectedItem.Value + ", cnt_designation=" + LDcmbDesignation.SelectedItem.Value + ", cnt_industry=" + LDcmbIndustry.SelectedItem.Value + ", cnt_contactSource=" + LDcmbSource.SelectedItem.Value + ", cnt_referedBy='" + LDtxtReferedBy_hidden.Value + "', cnt_contactType='" + ContType + "', cnt_contactStatus=" + LDcmbContactStatus.SelectedItem.Value + ",cnt_RegistrationDate='',cnt_rating='" + LDcmbRating.SelectedItem.Value + "',cnt_reason='',cnt_bloodgroup='" + LDcmbBloodgroup.SelectedItem.Value + "',WebLogIn='" + webLogin + "',PassWord='" + Password + "', lastModifyDate ='" + today + "', lastModifyUser=" + HttpContext.Current.Session["userid"]; // + Session["userid"]

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
                            string ModuleType = Convert.ToString(Session["requesttype"]);

                            if (ModuleType == "Customer/Client" && txtGSTIN2.Text.Trim() != "")
                            {
                                string KeyValId = Convert.ToString(HttpContext.Current.Session["KeyVal"]);
                                DataTable dt = UpdatePanNumber(KeyValId, txtGSTIN2.Text.Trim());
                            }

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

        private void btnif(bool p)
        {
            throw new NotImplementedException();
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

        // Rev 1.0
        [WebMethod]
        public static bool CheckUniqueCustomerName(string CategoriesFirstName, string Code)
        {
            bool flag = true;

            if (Convert.ToString(System.Web.HttpContext.Current.Session["requesttype"]) == "Customer/Client" || 
                    Convert.ToString(System.Web.HttpContext.Current.Session["requesttype"]) == "Transporter")
            {
                try
                {
                    BusinessLogicLayer.MShortNameCheckingBL obj = new BusinessLogicLayer.MShortNameCheckingBL();

                    if (Convert.ToString(System.Web.HttpContext.Current.Session["requesttype"]) == "Customer/Client")
                    {
                        flag = obj.CheckUniqueVendorName(CategoriesFirstName.Trim(), Code, "CheckUniqueCustomerName");
                    }
                    else if(Convert.ToString(System.Web.HttpContext.Current.Session["requesttype"]) == "Transporter")
                    {
                        flag = obj.CheckUniqueVendorName(CategoriesFirstName.Trim(), Code, "CheckUniqueTransporterName");
                    }

                }
                catch (Exception ex)
                {

                }
                finally
                {
                }
            }
            return flag;
        }
        // End of Rev 1.0


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
            Data = oDBEngine.GetFieldValue(" tbl_master_maritalstatus", " mts_id, mts_maritalStatus", null, 2, "mts_id desc");

            oclsDropDownList.AddDataToDropDownList(Data, LDcmbMaritalStatus);
            Data = oDBEngine.GetFieldValue(" tbl_master_contactstatus", "cntstu_id, cntstu_contactStatus", null, 2, "cntstu_contactStatus");

            oclsDropDownList.AddDataToDropDownList(Data, LDcmbContactStatus);


            Data = oDBEngine.GetFieldValue("tbl_master_legalstatus", "lgl_id, lgl_legalStatus", null, 2, "lgl_legalStatus");

            oclsDropDownList.AddDataToDropDownList(Data, LDcmbLegalStatus);
            LDCmbSalutation.SelectedValue = "1";
            LDcmbRating.SelectedValue = "1";

            // Code  Added  By Sam on 15112016 to Add Select as optional Data

            //LDCmbSalutation.Items.Insert(0, new ListItem("--Select--", "0"));
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

            if (hdnAutoNumStg.Value == "LDAutoNum1" && hdnTransactionType.Value == "LD")
            {
                LDtxt_CustDocNo.Text = ContactData[0, 0];
            }

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

        public DataTable UpdatePanNumber(string Id, string GSTIN)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_CustomerPANInsert");
            proc.AddVarcharPara("@Action", 500, "InsertPanNUmber");
            proc.AddVarcharPara("@lastModifyUser", 100, Convert.ToString(HttpContext.Current.Session["userid"]));
            proc.AddBigIntegerPara("@Cnt_Id", Convert.ToInt64(Id));
            proc.AddVarcharPara("@CNT_GSTIN", 30, GSTIN);
            dt = proc.GetTable();
            return dt;
        }

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
        public static List<string> GetDocumentSegment()
        {

            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = new DataTable();
            DT.Rows.Clear();

            ProcedureExecute proc = new ProcedureExecute("prc_ProductMaster_bindData");
            proc.AddVarcharPara("@action", 20, "GetSegmentLayout");
            DT = proc.GetTable();

            List<string> obj = new List<string>();
            foreach (DataRow dr in DT.Rows)
            {
                //obj.Add(Convert.ToString(dr["ID"]) + "|" + Convert.ToString(dr["NAME"]) + "|" + Convert.ToString(dr["UsedFor"]) + "|" + Convert.ToString(dr["MandatoryFor"]));
                obj.Add(Convert.ToString(dr["ID"]) + "|" + Convert.ToString(dr["NAME"]));

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


        public class RegisterShopInputPortal
        {
            public string session_token { get; set; }
            //[Required]
            public string user_id { get; set; }
            //[Required]
            public string shop_name { get; set; }
            //[Required]
            public string address { get; set; }
            //[Required]
            public string pin_code { get; set; }
            //[Required]
            public string shop_lat { get; set; }
            //[Required]
            public string shop_long { get; set; }
            //[Required]
            public string owner_name { get; set; }
            //[Required]
            public string owner_contact_no { get; set; }
            //[Required]
            public string owner_email { get; set; }
            public int? type { get; set; }
            public string dob { get; set; }
            public string date_aniversary { get; set; }
            public string shop_id { get; set; }
            public string added_date { get; set; }
            public string assigned_to_pp_id { get; set; }
            public string assigned_to_dd_id { get; set; }
            public string amount { get; set; }
            public Nullable<DateTime> family_member_dob { get; set; }
            public string director_name { get; set; }
            public string key_person_name { get; set; }
            public string phone_no { get; set; }
            public Nullable<DateTime> addtional_dob { get; set; }
            public Nullable<DateTime> addtional_doa { get; set; }
            public Nullable<DateTime> doc_family_member_dob { get; set; }
            public string specialization { get; set; }
            public string average_patient_per_day { get; set; }
            public string category { get; set; }
            public string doc_address { get; set; }
            public string doc_pincode { get; set; }
            public string is_chamber_same_headquarter { get; set; }
            public string is_chamber_same_headquarter_remarks { get; set; }
            public string chemist_name { get; set; }
            public string chemist_address { get; set; }
            public string chemist_pincode { get; set; }
            public string assistant_name { get; set; }
            public string assistant_contact_no { get; set; }
            public Nullable<DateTime> assistant_dob { get; set; }
            public Nullable<DateTime> assistant_doa { get; set; }
            public Nullable<DateTime> assistant_family_dob { get; set; }
            public string EntityCode { get; set; }
            public string Entity_Location { get; set; }
            public string Alt_MobileNo { get; set; }
            public string Entity_Status { get; set; }
            public string Entity_Type { get; set; }
            public string ShopOwner_PAN { get; set; }
            public string ShopOwner_Aadhar { get; set; }
            public string Remarks { get; set; }
            public string AreaId { get; set; }
            public string CityId { get; set; }
            public string Entered_by { get; set; }
            public string entity_id { get; set; }
            public string party_status_id { get; set; }
            public string retailer_id { get; set; }
            public string dealer_id { get; set; }
            public string beat_id { get; set; }
            public string IsServicePoint { get; set; }
        }

        public class RegisterShopOutput
        {
            public string status { get; set; }
            public string message { get; set; }
            public string session_token { get; set; }
        }

        public static object CustomerSync(String InternalId)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable dts = new DataTable();
            dts = oDBEngine.GetDataTable("select cnt_Id from tbl_master_contact where cnt_internalId='" + InternalId + "'");
            if (dts.Rows.Count == 1)
            {
                String weburl = System.Configuration.ConfigurationSettings.AppSettings["FSMAPIBaseUrl"];
                string apiUrl = weburl + "ShopRegisterPortal/CustomerSyncinShop";
                RegisterShopOutput oview = new RegisterShopOutput();
                int userid = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
                RegisterShopInputPortal empDtls = new RegisterShopInputPortal();
                DataTable dt = new DataTable();

                ProcedureExecute proc = new ProcedureExecute("PRC_EmployeeDetailsForSync");
                proc.AddPara("@ACTION", "CustomerDetails");
                proc.AddPara("@ContactID", dts.Rows[0]["cnt_Id"].ToString());
                dt = proc.GetTable();
                if (dt != null && dt.Rows.Count > 0)
                {

                    DateTime date1 = DateTime.Parse("1970-01-01");
                    DateTime date2 = System.DateTime.Now;
                    var Difference_In_Time = Convert.ToString((int)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                    var middle = (Math.Round(Convert.ToDecimal(Difference_In_Time) / 1000) * 1155) + 1;

                    empDtls.session_token = "zksjfhjsdjkskjdh";
                    empDtls.user_id = Convert.ToString(378);
                    empDtls.shop_name = Convert.ToString(dt.Rows[0]["PartyName"]);
                    empDtls.address = Convert.ToString(dt.Rows[0]["ADDRESS1"]);
                    empDtls.pin_code = Convert.ToString(dt.Rows[0]["PinCode"]);
                    empDtls.shop_lat = Convert.ToString(dt.Rows[0]["PartyLocationLat"]);
                    empDtls.shop_long = Convert.ToString(dt.Rows[0]["PartyLocationLong"]);
                    empDtls.owner_name = Convert.ToString(dt.Rows[0]["Owner"]);
                    empDtls.owner_contact_no = Convert.ToString(dt.Rows[0]["Contact"]);
                    empDtls.owner_email = Convert.ToString(dt.Rows[0]["Email"]);
                    empDtls.type = Convert.ToInt32(dt.Rows[0]["Type"]);
                    empDtls.dob = Convert.ToString(dt.Rows[0]["DOB"]);
                    empDtls.date_aniversary = Convert.ToString(dt.Rows[0]["Anniversary"]);
                    empDtls.shop_id = Convert.ToString(dt.Rows[0]["AssignToUser"]) + "_" + Convert.ToString(Difference_In_Time);
                    empDtls.added_date = Convert.ToString(System.DateTime.Now);
                    empDtls.assigned_to_pp_id = "";
                    empDtls.assigned_to_dd_id = "";
                    empDtls.EntityCode = Convert.ToString(dt.Rows[0]["PartyCode"]);
                    empDtls.Entity_Location = Convert.ToString(dt.Rows[0]["Location"]);
                    empDtls.Alt_MobileNo = Convert.ToString(dt.Rows[0]["AlternateContact"]);
                    //empDtls.State_ID = Convert.ToString(dt.Rows[0]["State"]);
                    empDtls.Entity_Status = Convert.ToString(dt.Rows[0]["Status"]);
                    empDtls.Entity_Type = Convert.ToString(dt.Rows[0]["EntityCategory"]);
                    empDtls.ShopOwner_PAN = Convert.ToString(dt.Rows[0]["OwnerPAN"]);
                    empDtls.ShopOwner_Aadhar = Convert.ToString(dt.Rows[0]["OwnerAadhaar"]);
                    empDtls.Remarks = Convert.ToString(dt.Rows[0]["Remarks"]);
                    empDtls.AreaId = Convert.ToString(dt.Rows[0]["Area"]);
                    empDtls.CityId = Convert.ToString(dt.Rows[0]["District"]);
                    empDtls.Entered_by = Convert.ToString(userid);
                    empDtls.retailer_id = Convert.ToString("0");
                    empDtls.dealer_id = Convert.ToString("0");
                    empDtls.entity_id = Convert.ToString("0");
                    empDtls.party_status_id = Convert.ToString(dt.Rows[0]["PartyStatus"]);
                    empDtls.beat_id = Convert.ToString(dt.Rows[0]["GroupBeat"]);
                    empDtls.IsServicePoint = Convert.ToString(dt.Rows[0]["IsServicePoint"]);
                }

                String Status = "Failed";
                String FailedReason = "";
                string data = JsonConvert.SerializeObject(empDtls);

                HttpClient httpClient = new HttpClient();
                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent(data), "data");
                var result = httpClient.PostAsync(apiUrl, form).Result;

                oview = JsonConvert.DeserializeObject<RegisterShopOutput>(result.Content.ReadAsStringAsync().Result);

                if (Convert.ToString(oview.status) == "200")
                {
                    Status = "Success";
                }
                else if (Convert.ToString(oview.status) == "202")
                {
                    FailedReason = "Customer Name Not found";
                }
                else if (Convert.ToString(oview.status) == "203")
                {
                    FailedReason = "Entity Code not found";
                }
                else if (Convert.ToString(oview.status) == "204")
                {
                    FailedReason = "Owner Name Not found";
                }
                else if (Convert.ToString(oview.status) == "205")
                {
                    FailedReason = "Customer Address not found";
                }
                else if (Convert.ToString(oview.status) == "206")
                {
                    FailedReason = "Pin Code not found";
                }
                else if (Convert.ToString(oview.status) == "207")
                {
                    FailedReason = "Customer Contact number not found";
                }
                else if (Convert.ToString(oview.status) == "208")
                {
                    FailedReason = "User or session token not matched";
                }
                else if (Convert.ToString(oview.status) == "209")
                {
                    FailedReason = "Duplicate Customer Id or contact number";
                }
                else if (Convert.ToString(oview.status) == "210")
                {
                    FailedReason = "Duplicate contact number";
                }

                ProcedureExecute proc1 = new ProcedureExecute("PRC_EmployeeDetailsForSync");
                proc1.AddPara("@ACTION", "SyncLog");
                proc1.AddPara("@ContactID", dts.Rows[0]["cnt_Id"].ToString());
                proc1.AddPara("@CustomerName", Convert.ToString(dt.Rows[0]["PartyName"]));
                proc1.AddPara("@CustomerAddress", Convert.ToString(dt.Rows[0]["ADDRESS1"]));
                proc1.AddPara("@CustomerPhone", Convert.ToString(dt.Rows[0]["Contact"]));
                proc1.AddPara("@SyncBy", userid);
                proc1.AddPara("@Status", Status);
                proc1.AddPara("@FailedReason", FailedReason);
                proc1.AddPara("@Shop_Code", empDtls.shop_id);
                int i = proc1.RunActionQuery();
            }
            return new { status = "ok" };
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

        //Rev Start Tanmoy 24-05-2021
        public void BindLoginUserInternalId()
        {
            DataTable dt = new DataTable();
            dt = oDBEngine.GetDataTable("tbl_master_user, tbl_master_contact ", "ISNULL(cnt_AssociatedEmp, '') AS cnt_internalId ", " tbl_master_contact.cnt_AssociatedEmp=tbl_master_user.user_contactId  and cnt_contactType='AG' and user_id = '" + Convert.ToString(Session["userid"]) + "'");

            if (dt != null && dt.Rows.Count > 0)
            {

                hdnLoginUserSalesmanAgentsInternalId.Value = Convert.ToString(dt.Rows[0]["cnt_internalId"]);
            }
            else
            {
                hdnLoginUserSalesmanAgentsInternalId.Value = "";
            }
        }

        public void BindTDS()
        {
            DataTable tdsMaster = new DataTable();
            ProcedureExecute proc;
            using (proc = new ProcedureExecute("PRC_TDSMASTERLIST"))
            {
                tdsMaster = proc.GetTable();
            }
            if (tdsMaster != null && tdsMaster.Rows.Count > 0)
            {
                aspxDeducteesNew.TextField = "TYPE_NAME";
                aspxDeducteesNew.ValueField = "ID";
                aspxDeducteesNew.DataSource = tdsMaster;
                aspxDeducteesNew.DataBind();
            }
        }

        public string Alternative_Code { get; set; }
    }
}