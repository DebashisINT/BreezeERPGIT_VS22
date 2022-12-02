using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using ClsDropDownlistNameSpace;
using System.Web.Services;
using System.Collections.Generic;
using DataAccessLayer;
using BusinessLogicLayer;

//using DevExpress.Web.ASPxTabControl;

namespace ERP.OMS.Management.Master
{
    public partial class management_master_HRrecruitmentagent_general : ERP.OMS.ViewState_class.VSPage//System.Web.UI.Page
    {
        Int32 ID;
        //Converter objConverter = new Converter();
        //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        clsDropDownList oclsDropDownList = new clsDropDownList();
        BusinessLogicLayer.Recruitment_Agents oHrRecritmentGeneralBL = new BusinessLogicLayer.Recruitment_Agents();
        BusinessLogicLayer.RemarkCategoryBL reCat = new BusinessLogicLayer.RemarkCategoryBL();
        CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();
        BusinessLogicLayer.RemarkCategoryBL brmap = new BusinessLogicLayer.RemarkCategoryBL();
        protected void Page_Init(object sender, EventArgs e)
        {

            branchdtl.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            BranchdataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            ///dev Bapi
            productdtl.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            ProductdataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            //end dev Bapi

            GstinSettingsButton.contact_type = "VENDOR";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CommonBL ComBL = new CommonBL();
            string AutoNumberAllow = ComBL.GetSystemSettingsResult("UniqueAutoNumberVendorMaster");
            string PrefreedProductCheck = ComBL.GetSystemSettingsResult("AllowPreferredProducts");


            if (!IsPostBack)
            {

                //chinmoy added for Numbering Scheme start
                if (Convert.ToString(Session["requesttype"]) == "DV")
                {

                    dvNameAsPerPan.Style.Add("display", "block");
                    dvDeducteestat.Style.Add("display", "block");
                    dvTaxDeducteedType.Style.Add("display", "block");
                    if (!String.IsNullOrEmpty(AutoNumberAllow))
                    {
                        if (AutoNumberAllow == "Yes")
                        {
                            hdnAutoNumStg.Value = "DVNumb1";
                            hdnTransactionType.Value = "DV";
                            dvUniqueId.Style.Add("display", "None");
                            ddl_Num.Style.Add("display", "block");
                            dvCustDocNo.Style.Add("display", "block");
                            NumberingSchemeBind();
                        }
                        else
                        {
                            hdnAutoNumStg.Value = "DVNumb0";
                            hdnTransactionType.Value = "";
                            dvUniqueId.Style.Add("display", "block");
                        }
                    }
                    if (!String.IsNullOrEmpty(PrefreedProductCheck))
                    {
                        if (PrefreedProductCheck == "Yes")
                        {
                            divProduct.Style.Add("display", "block");
                        }
                        else
                        {
                            divProduct.Style.Add("display", "none");
                        }

                    }

                }
                //End
                // GetFinacialYearBasedQouteDate();
                SetDateFormat();

                //Get Udf Count
                IsUdfpresent.Value = Convert.ToString(getUdfCount());

                DDLBind();

                branchdtl.SelectCommand = "select '0' as branch_id ,  '--ALL--' as branch_description union all select branch_id,branch_description from tbl_master_branch order by branch_description";
                cmbMultiBranches.DataBind();
                cmbMultiBranches.Value = "0";

                //Rev Bapi
                productdtl.SelectCommand = "select '0' as product_id ,  '--ALL--' as product_description  union all select sProducts_ID,sProducts_Name from MASTER_SPRODUCTS";
                cmbMultiProducts.DataBind();
                cmbMultiProducts.Value = "0";
                //End Rev Bapi



                Session["ContactType"] = "Relationship Manager";
                if (Request.QueryString["id"] != "ADD")
                {
                    if ((hdnAutoNumStg.Value == "DVNumb1") && (hdnTransactionType.Value == "DV"))
                    {
                        ddl_Num.Style.Add("display", "none");
                        txt_CustDocNo.ClientEnabled = false;
                    }
                    hddnApplicationMode.Value = "Edit";
                    if (Request.QueryString["id"] != null)
                    {
                        ID = Int32.Parse(Request.QueryString["id"]);
                        HttpContext.Current.Session["KeyVal"] = ID;
                        HdId.Value = Convert.ToString(ID);
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
                    HttpContext.Current.Session["KeyVal_InternalIDVen"] = InternalId[0, 0];
                    HttpContext.Current.Session["KeyVal_InternalID"] = InternalId[0, 0];
                    Keyval_internalId.Value = InternalId[0, 0];
                    //set the internal key val Id also in hidden field so that we can pass it in ajax call 
                    //debjyoti28-11-2016

                    hdKeyVal_InternalID.Value = Convert.ToString(InternalId[0, 0]);

                    string[,] ContactData;
                    if (ID != 0)
                    {
                        //rev srijeeta  mantis issue 0024515
                        //ContactData = oDBEngine.GetFieldValue("tbl_master_contact",
                        //                        "cnt_ucc, cnt_salutation,  cnt_firstName, cnt_middleName, cnt_lastName, cnt_shortName, cnt_branchId, cnt_sex, cnt_maritalStatus, case cnt_DOB when '1/1/1900 12:00:00 AM' then null else cnt_DOB end as cnt_DOB,case cnt_anniversaryDate when '1/1/1900 12:00:00 AM' then null else cnt_anniversaryDate end as cnt_anniversaryDate, cnt_legalStatus, cnt_education, cnt_profession, cnt_organization, cnt_jobResponsibility, cnt_designation, cnt_industry, cnt_contactSource, cnt_referedBy, cnt_contactType, cnt_contactStatus,CNT_GSTIN,cnt_mainAccount,cnt_PrintNameToCheque,cnt_EntityType,AccountGroupID,isnull(Cnt_NameAsPerPan,'') Cnt_NameAsPerPan,isnull(Cnt_DeducteeStatus,'') Cnt_DeducteeStatus,isnull(CNT_TAX_ENTITYTYPE,'') CNT_TAX_ENTITYTYPE",
                        //                        " cnt_id=" + ID, 30);
                        ContactData = oDBEngine.GetFieldValue("tbl_master_contact",
                                                "cnt_ucc, cnt_salutation,  cnt_firstName, cnt_middleName, cnt_lastName, cnt_shortName, cnt_branchId, cnt_sex, cnt_maritalStatus, case cnt_DOB when '1/1/1900 12:00:00 AM' then null else cnt_DOB end as cnt_DOB,case cnt_anniversaryDate when '1/1/1900 12:00:00 AM' then null else cnt_anniversaryDate end as cnt_anniversaryDate, cnt_legalStatus, cnt_education, cnt_profession, cnt_organization, cnt_jobResponsibility, cnt_designation, cnt_industry, cnt_contactSource, cnt_referedBy, cnt_contactType, cnt_contactStatus,CNT_GSTIN,cnt_mainAccount,cnt_PrintNameToCheque,cnt_EntityType,AccountGroupID,isnull(Cnt_NameAsPerPan,'') Cnt_NameAsPerPan,isnull(Cnt_DeducteeStatus,'') Cnt_DeducteeStatus,isnull(CNT_TAX_ENTITYTYPE,'') CNT_TAX_ENTITYTYPE,Alternative_Code",
                                                " cnt_id=" + ID, 31);
                        //end of rev srijeeta  mantis issue 0024515
                        //Rev Rajdip For Pan No
                        SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                        SqlDataAdapter dapanno = new SqlDataAdapter("select TOP 1 crg_Number from tbl_master_contactRegistration where crg_cntId=(select cnt_internalId from tbl_master_contact where cnt_id='" + ID + "') and crg_type='Pan Card'", con);
                        DataTable dtpan = new DataTable();
                        dapanno.Fill(dtpan);
                        if (dtpan.Rows.Count > 0)
                        {
                            txtNumber.Text = dtpan.Rows[0]["crg_Number"].ToString();
                        }
                        //End Rev Rajdip For Pan No
                    }
                    else
                    {
                        //rev srijeeta  mantis issue 0024515
                        //ContactData = oDBEngine.GetFieldValue("tbl_master_contact",
                        //                        "cnt_ucc, cnt_salutation,  cnt_firstName, cnt_middleName, cnt_lastName, cnt_shortName, cnt_branchId, cnt_sex, cnt_maritalStatus, case cnt_DOB when '1/1/1900 12:00:00 AM' then null else cnt_DOB end as cnt_DOB,case cnt_anniversaryDate when '1/1/1900 12:00:00 AM' then null else cnt_anniversaryDate end as cnt_anniversaryDate, cnt_legalStatus, cnt_education, cnt_profession, cnt_organization, cnt_jobResponsibility, cnt_designation, cnt_industry, cnt_contactSource, cnt_referedBy, cnt_contactType, cnt_contactStatus,CNT_GSTIN,cnt_mainAccount,cnt_PrintNameToCheque,cnt_EntityType,AccountGroupID,isnull(Cnt_NameAsPerPan,'') Cnt_NameAsPerPan,isnull(Cnt_DeducteeStatus,'') Cnt_DeducteeStatus,isnull(CNT_TAX_ENTITYTYPE,'') CNT_TAX_ENTITYTYPE", Alternative_Code,
                        //                        " cnt_id=" + HttpContext.Current.Session["KeyVal"], 30);
                        ContactData = oDBEngine.GetFieldValue("tbl_master_contact",
                                                "cnt_ucc, cnt_salutation,  cnt_firstName, cnt_middleName, cnt_lastName, cnt_shortName, cnt_branchId, cnt_sex, cnt_maritalStatus, case cnt_DOB when '1/1/1900 12:00:00 AM' then null else cnt_DOB end as cnt_DOB,case cnt_anniversaryDate when '1/1/1900 12:00:00 AM' then null else cnt_anniversaryDate end as cnt_anniversaryDate, cnt_legalStatus, cnt_education, cnt_profession, cnt_organization, cnt_jobResponsibility, cnt_designation, cnt_industry, cnt_contactSource, cnt_referedBy, cnt_contactType, cnt_contactStatus,CNT_GSTIN,cnt_mainAccount,cnt_PrintNameToCheque,cnt_EntityType,AccountGroupID,isnull(Cnt_NameAsPerPan,'') Cnt_NameAsPerPan,isnull(Cnt_DeducteeStatus,'') Cnt_DeducteeStatus,isnull(CNT_TAX_ENTITYTYPE,'') CNT_TAX_ENTITYTYPE,Alternative_Code",
                                                " cnt_id=" + HttpContext.Current.Session["KeyVal"], 31);
                        //end of rev srijeeta  mantis issue 0024515
                        //Rev Rajdip For Pan No
                        SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                        SqlDataAdapter dapanno = new SqlDataAdapter("select TOP 1 crg_Number from tbl_master_contactRegistration where crg_cntId=(select cnt_internalId from tbl_master_contact where cnt_id='" + HttpContext.Current.Session["KeyVal"] + "') and crg_type='Pan Card'", con);
                        DataTable dtpan = new DataTable();
                        dapanno.Fill(dtpan);
                        if (dtpan.Rows.Count > 0)
                        {
                            txtNumber.Text = dtpan.Rows[0]["crg_Number"].ToString();
                        }
                        //End Rev Rajdip For Pan No
                    }

                    //____________ Value Allocation _______________//

                    ValueAllocation(ContactData);

                    SetBranchRecordToSessionTable(Convert.ToString(HttpContext.Current.Session["KeyVal_InternalIDVen"]));
                    ShowBranchName(Convert.ToString(HttpContext.Current.Session["KeyVal_InternalIDVen"]));

                    //Rev Bapi 
                 //   SetProductRecordToSessionTable(Convert.ToString(HttpContext.Current.Session["KeyVal_InternalIDVen"]));
                 //   ShowProductName(Convert.ToString(HttpContext.Current.Session["KeyVal_InternalIDVen"]));


                    ////if (lblSelectedProduct.Text =="")
                    ////{
                    ////    SetProductRecordVendorMappingToSessionTable(Convert.ToString(HttpContext.Current.Session["KeyVal_InternalIDVen"]));
                    ////    ShowProductNameVendorMapping(Convert.ToString(HttpContext.Current.Session["KeyVal_InternalIDVen"]));
                    ////}

                    SetProductRecordVendorMappingToSessionTable(Convert.ToString(HttpContext.Current.Session["KeyVal_InternalIDVen"]));
                    ShowProductNameVendorMapping(Convert.ToString(HttpContext.Current.Session["KeyVal_InternalIDVen"]));


                    //End Rev Bapi
                }
                else
                {
                    hddnApplicationMode.Value = "Add";
                    GstinSettingsButton.Visible = false;

                    CmbSalutation.SelectedValue = "0";

                    txtFirstName.Text = "";

                    txtCode.Text = "";
                    refferByDD.Value = "true";
                    cmbBranch.SelectedIndex.Equals(0);

                    DateOfIncoorporation.Value = "";

                    cmbLegalStatus.SelectedValue = "3";


                    cmbSource.SelectedIndex.Equals(0);
                    cmbContactStatus.SelectedIndex.Equals(0);
                    //----Making TABs Disable------//
                    DisabledTabPage();

                    //-----End---------------------//
                    HttpContext.Current.Session["KeyVal"] = "0";
                    HttpContext.Current.Session["KeyVal_InternalIDVen"] = string.Empty;
                    HttpContext.Current.Session["KeyVal_InternalID"] = string.Empty;
                    HdId.Value = "0";

                    Keyval_internalId.Value = "Add";
                }
                string[,] EmployeeNameID = oDBEngine.GetFieldValue(" tbl_master_contact ", " case when cnt_firstName is null then '' else cnt_firstName end + ' '+case when cnt_middleName is null then '' else cnt_middleName end+ ' '+case when cnt_lastName is null then '' else cnt_lastName end+' ['+cnt_shortName+']' as name ", " cnt_internalId='" + HttpContext.Current.Session["KeyVal_InternalIDVen"] + "'", 1);
                if (EmployeeNameID[0, 0] != "n")
                {
                    lblHeader.Text = EmployeeNameID[0, 0].ToUpper();
                }
            }

        }


        public void NumberingSchemeBind()
        {
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            string actionqry = "GetAllDropDownDetailForVendorMaster";
            DataTable Schemadt = GetAllDropDownDetailForCustomerMaster(userbranchHierarchy, actionqry);

            if (Schemadt != null && Schemadt.Rows.Count > 0)
            {
                ddl_numberingScheme.DataTextField = "SchemaName";
                ddl_numberingScheme.DataValueField = "Id";
                ddl_numberingScheme.DataSource = Schemadt;
                ddl_numberingScheme.DataBind();
            }

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
        }
        //dt_P
        public void DDLBind()
        {


            string[,] Data = oDBEngine.GetFieldValue("tbl_master_salutation", "sal_id, sal_name", null, 2, "sal_name");

            oclsDropDownList.AddDataToDropDownList(Data, CmbSalutation);
            CmbSalutation.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0"));
            CmbSalutation.SelectedValue = "0";

            Data = oDBEngine.GetFieldValue("tbl_master_branch", "branch_id, branch_description ", null, 2, "branch_description");

            oclsDropDownList.AddDataToDropDownList(Data, cmbBranch);
            Data = oDBEngine.GetFieldValue(" tbl_master_ContactSource", "cntsrc_id, cntsrc_sourcetype", null, 2, "cntsrc_sourcetype");


            // oclsDropDownList.AddDataToDropDownList(Data, cmbMultiBranches);
            //Data = oDBEngine.GetFieldValue(" tbl_master_ContactSource", "cntsrc_id, cntsrc_sourcetype", null, 2, "cntsrc_sourcetype");


            oclsDropDownList.AddDataToDropDownList(Data, cmbSource);
            cmbSource.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0"));
            cmbSource.SelectedValue = "0";

            Data = oDBEngine.GetFieldValue(" tbl_master_contactstatus", "cntstu_id, cntstu_contactStatus", null, 2, "cntstu_contactStatus");

            oclsDropDownList.AddDataToDropDownList(Data, cmbContactStatus);
            cmbContactStatus.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0"));
            cmbContactStatus.SelectedValue = "0";


            Data = oDBEngine.GetFieldValue("tbl_master_legalstatus", "lgl_id, lgl_legalStatus", null, 2, "lgl_legalStatus");

            oclsDropDownList.AddDataToDropDownList(Data, cmbLegalStatus);


            Data = oDBEngine.GetFieldValue("Master_AccountGroup", "AccountGroup_ReferenceID, AccountGroup_Name+' ('+AccountGroup_Type+')'", "AccountGroup_Type= 'Liability' or AccountGroup_Type='Asset'", 2, "AccountGroup_Name");
            oclsDropDownList.AddDataToDropDownList(Data, ddlAssetLiability);
            ddlAssetLiability.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0"));
        }



        public void DisabledTabPage()
        {
            TabPage page = ASPxPageControl1.TabPages.FindByName("CorresPondence");
            page.Visible = false;
            page = ASPxPageControl1.TabPages.FindByName("ContactPreson");
            page.Visible = false;
            page = ASPxPageControl1.TabPages.FindByName("BankDetails");
            page.Visible = false;

            page = ASPxPageControl1.TabPages.FindByName("Documents");
            page.Visible = false;
            page = ASPxPageControl1.TabPages.FindByName("GroupMember");
            page.Visible = false;

            page = ASPxPageControl1.TabPages.FindByName("Registration");
            page.Visible = false;
            page = ASPxPageControl1.TabPages.FindByName("TDS");
            page.Visible = false;
        }
        public void ValueAllocation(string[,] ContactData)
        {
            try
            {

                if (ContactData[0, 1] != "")
                {
                    CmbSalutation.SelectedValue = ContactData[0, 1];
                }
                else
                {
                    CmbSalutation.SelectedIndex.Equals(0);
                }

                txtFirstName.Text = ContactData[0, 2];

                if (hdnAutoNumStg.Value == "DVNumb1" && hdnTransactionType.Value == "DV")
                {
                    txt_CustDocNo.Text = ContactData[0, 5];
                }
                else
                {
                    txtCode.Text = ContactData[0, 5];
                }
                //if (ContactData[0, 6] != "")
                //{
                //    cmbBranch.SelectedValue = ContactData[0, 6];
                //}
                //else
                //{
                //    cmbBranch.SelectedIndex.Equals(0);
                //}


                if (ContactData[0, 6] != "")
                {
                    cmbMultiBranches.Value = ContactData[0, 6];
                }
                else
                {
                    cmbMultiBranches.SelectedIndex.Equals(0);
                }

                if (ContactData[0, 9] != "")
                    DateOfIncoorporation.Value = Convert.ToDateTime(ContactData[0, 9]);
                if (ContactData[0, 11] != "")
                {
                    cmbLegalStatus.SelectedValue = ContactData[0, 11];
                }
                else
                {
                    cmbLegalStatus.SelectedIndex.Equals(0);
                }

                if (ContactData[0, 18] != "")
                {
                    cmbSource.SelectedValue = ContactData[0, 18];
                }
                else
                {
                    cmbSource.SelectedIndex.Equals(0);
                }
                //debjyoti 25-11-2016
                //reason: switch in listbox and textbox
                // txtReferedBy.Text = ContactData[0, 19];
                refferByDD.Value = Convert.ToString(isDropDown(Convert.ToInt32(cmbSource.SelectedValue)));
                if (Convert.ToBoolean(refferByDD.Value))
                {
                    lstReferedBy.Text = ContactData[0, 19];
                    RefferedByValue.Value = Convert.ToString(ContactData[0, 19]);
                }
                else
                {
                    txtReferedBy.Text = ContactData[0, 19];
                }
                //end 25-11-2016

                if (ContactData[0, 21] != "")
                {
                    cmbContactStatus.SelectedValue = ContactData[0, 21];
                }
                else
                {
                    cmbContactStatus.SelectedIndex.Equals(0);
                }

                if (ContactData[0, 27] != "")
                {
                    txtNameAsPerPan.Text = ContactData[0, 27];
                }
                else
                {
                    txtNameAsPerPan.Text = "";
                }

                if (ContactData[0, 28] != "")
                {
                    cmbDeducteestat.Value = ContactData[0, 28];
                }

                if (ContactData[0, 29] != "")
                {
                    cmbTaxdeducteedType.Value = ContactData[0, 29];
                }
                //rev srijeeta  mantis issue 0024515
                if (ContactData[0, 30] != "")
                {
                    ASPxTextBox1.Text = ContactData[0, 30];
                }
                //end of rev srijeeta  mantis issue 0024515

                //Debjyoti GSTIN 060217
                string GSTIN = "";
                if (ContactData[0, 22] != "")
                {

                    GSTIN = ContactData[0, 22];
                    txtGSTIN1.Text = GSTIN.Substring(0, 2);
                    txtGSTIN2.Text = GSTIN.Substring(2, 10);
                    txtGSTIN3.Text = GSTIN.Substring(12, 3);
                    radioregistercheck.SelectedValue = "1";

                    hddnGSTIN2Val.Value = Convert.ToString(txtGSTIN1.Text) + Convert.ToString(txtGSTIN2.Text) + Convert.ToString(txtGSTIN3.Text);
                }
                else
                {
                    radioregistercheck.SelectedValue = "0";
                    hddnGSTIN2Val.Value = "";
                }

                #region Subhabrata/BindApplicableFrom
                //Subhabrata

                DataTable dt_CustVendHistory = null;
                dt_CustVendHistory = objCRMSalesOrderDtlBL.GetCustVendHistoryId(Convert.ToString(HttpContext.Current.Session["KeyVal"]));
                if (dt_CustVendHistory != null && dt_CustVendHistory.Rows.Count > 0)
                {
                    dt_ApplicableFrom.Date = DateTime.ParseExact(Convert.ToString(dt_CustVendHistory.Rows[0]["ApplicableFrom"]), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                }

                //End
                #endregion

                hndTaxRates_MainAccount_hidden.Value = ContactData[0, 23];

                txtNameInCheque.Text = Convert.ToString(ContactData[0, 24]);
                rdl_VendorType.SelectedValue = Convert.ToString(ContactData[0, 25]);
                ddlAssetLiability.SelectedValue = Convert.ToString(ContactData[0, 26]);


                ProcedureExecute pro = new ProcedureExecute("prc_VendorMainAccountCheck");
                pro.AddVarcharPara("@Action", 100, "CountTransactionForVendor");
                pro.AddVarcharPara("@VendorId", 50, Convert.ToString(Session["KeyVal"]));
                pro.AddVarcharPara("@MainAccountCode", 200, ContactData[0, 23]);
                pro.AddVarcharPara("@cnt", 50, "", QueryParameterDirection.Output);

                int i = pro.RunActionQuery();
                string rtrnvalue = pro.GetParaValue("@cnt").ToString();

                //string[] getData = oDBEngine.GetFieldValue1("Trans_AccountsLedger", "COUNT(*)", "AccountsLedger_MainAccountID='" + ContactData[0, 23] + "' and  AccountsLedger_MainAccountID<>''", 1);
                if (rtrnvalue == "0")
                    hdIsMainAccountInUse.Value = "notInUse";
                else
                    hdIsMainAccountInUse.Value = "IsInUse";

            }
            catch
            {
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Boolean result = false;
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]); multi
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            string branch = string.Empty;
            CommonBL ComBL = new CommonBL();
            branch = "0";
            if (hdnflag.Value.ToString() == "1")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "jAlert('Please Enter Valid PAN');", true);

                return;
            }
            //UPDATE
            if (Convert.ToString(HttpContext.Current.Session["KeyVal"]) != "0")
            {
                 string GSTINNumber = "";
                if (Convert.ToString(Session["requesttype"]) == "DV")
                { 
                   GSTINNumber = Convert.ToString(txtGSTIN1.Text.Trim()) +  Convert.ToString(txtGSTIN2.Text.Trim()) +  Convert.ToString(txtGSTIN3.Text.Trim());
                   if(Convert.ToString(txtNumber.Text.Trim())=="" && GSTINNumber !="" && Convert.ToString(txtGSTIN2.Text.Trim()) !="")
                  {
                    txtNumber.Text = Convert.ToString(txtGSTIN2.Text.Trim());
                  }
                }
                //Rev Rajdip For Update Pan No
                ProcedureExecute pro = new ProcedureExecute("Prc_InsertPandetails");
                pro.AddVarcharPara("@Action", 100, "UPDATEPAN");
                pro.AddVarcharPara("@crg_cntId", 50, Convert.ToString(Session["KeyVal"]));
                pro.AddVarcharPara("@crg_contactType", 200, "contact");
                pro.AddVarcharPara("@crg_type", 200, "Pan Card");
                pro.AddVarcharPara("@crg_Number", 200, txtNumber.Text.ToString());
                pro.AddVarcharPara("@crg_place", 200, "");
                pro.AddVarcharPara("@crg_validDate", 200, "");
                pro.AddVarcharPara("@crg_Date", 200, "");
                pro.AddVarcharPara("@crg_verify", 200, "0");
                pro.AddVarcharPara("@crg_PanExmptProofType", 200, "");
                pro.AddVarcharPara("@crg_PanExmptProofNumber", 200, "");
                pro.AddIntegerPara("@CreateUser", ID);
                pro.AddVarcharPara("@MESSAGE", 50, "", QueryParameterDirection.Output);
                int i = pro.RunActionQuery();
                //if (i < 1)
                //{
                //    Page.ClientScript.RegisterStartupScript(GetType(), "Script", "<script>alert('Pan No. Exists!')</script>");
                //    return;
                //}
                //END Rev Rajdip For Update Pan No
                string requesttype = string.Empty;
                if (!string.IsNullOrEmpty(Convert.ToString(Session["requesttype"])))
                {
                    requesttype = Convert.ToString(Session["requesttype"]);

                }
                string today = Convert.ToString(oDBEngine.GetDate());

                //debjyoti 25-11-2016
                //reason: switch in listbox and textbox
                DateTime DtIncoorporationDtae;
                if (DateOfIncoorporation.Value != null)
                {
                    DtIncoorporationDtae = Convert.ToDateTime(DateOfIncoorporation.Value);
                }
                else
                {
                    DtIncoorporationDtae = Convert.ToDateTime("01-01-1900");
                }
                //Debjyoti GstIN 060217
                string GSTIN = "";
                if (radioregistercheck.SelectedValue == "1")
                {
                    GSTIN = txtGSTIN1.Text.Trim() + txtGSTIN2.Text.Trim() + txtGSTIN3.Text.Trim();
                }

                if (GSTIN != "")
                {
                    string DuplicateGSTINCustomer = ComBL.GetSystemSettingsResult("DuplicateGSTINCustomer");
                    if (!String.IsNullOrEmpty(DuplicateGSTINCustomer))
                    {
                        if (DuplicateGSTINCustomer == "No")
                        {
                            result = ContactGeneralBL.ISUniqueGSTIN(Convert.ToString(HttpContext.Current.Session["KeyVal"]), "1", GSTIN, "DV");
                            if (result)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "jAlert('Duplicate GSTIN number.');", true);
                                return;
                            }
                        }
                    }
                }
                //End Here

                //chinmoy start 31-03-2020	
                string UniqueNamne = "";
                if (hdnAutoNumStg.Value == "DVNumb1" && hdnTransactionType.Value == "DV")
                {
                    UniqueNamne = hddnDocNo.Value.Trim();
                }
                else
                {
                    UniqueNamne = txtCode.Text.Trim();
                }
                //end
                //rev srijeeta  mantis issue 0024515
                string Alternative_Code = "";
                if (ASPxTextBox1.Text != "")
                {
                    Alternative_Code = ASPxTextBox1.Text;
                }

                //end of rev srijeeta  mantis issue 0024515
                String value = "";

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

                if (Convert.ToBoolean(refferByDD.Value))
                {
                    //rev srijeeta  mantis issue 0024515
                    //value = "cnt_salutation=" + CmbSalutation.SelectedItem.Value + ",  cnt_firstName='" + txtFirstName.Text + "', cnt_shortName='" + UniqueNamne + "',  cnt_DOB='" + DtIncoorporationDtae + "',  cnt_legalStatus=" + cmbLegalStatus.SelectedItem.Value + ", cnt_branchId=" + branch + ",  cnt_contactSource=" + cmbSource.SelectedItem.Value + ", cnt_referedBy='" + Convert.ToString(RefferedByValue.Value) + "', cnt_contactType='" + requesttype + "', cnt_contactStatus=" + cmbContactStatus.SelectedItem.Value + ", lastModifyDate ='" + today + "', lastModifyUser=" + HttpContext.Current.Session["userid"] + ",CNT_GSTIN='" + GSTIN + "',cnt_PrintNameToCheque='" + txtNameInCheque.Text + "' ,cnt_EntityType='" + rdl_VendorType.SelectedValue + "',Cnt_NameAsPerPan='" + NameAsperPan + "',Cnt_DeducteeStatus='" + DeducteeStatus + "' ,CNT_TAX_ENTITYTYPE='" + Convert.ToString(cmbTaxdeducteedType.Value) + "'  "; // + Session["userid"] 
                    value = "cnt_salutation=" + CmbSalutation.SelectedItem.Value + ",  cnt_firstName='" + txtFirstName.Text + "', cnt_shortName='" + UniqueNamne + "',  cnt_DOB='" + DtIncoorporationDtae + "',  cnt_legalStatus=" + cmbLegalStatus.SelectedItem.Value + ", cnt_branchId=" + branch + ",  cnt_contactSource=" + cmbSource.SelectedItem.Value + ", cnt_referedBy='" + Convert.ToString(RefferedByValue.Value) + "', cnt_contactType='" + requesttype + "', cnt_contactStatus=" + cmbContactStatus.SelectedItem.Value + ", lastModifyDate ='" + today + "', lastModifyUser=" + HttpContext.Current.Session["userid"] + ",CNT_GSTIN='" + GSTIN + "',cnt_PrintNameToCheque='" + txtNameInCheque.Text + "' ,cnt_EntityType='" + rdl_VendorType.SelectedValue + "',Cnt_NameAsPerPan='" + NameAsperPan + "',Cnt_DeducteeStatus='" + DeducteeStatus + "' ,CNT_TAX_ENTITYTYPE='" + Convert.ToString(cmbTaxdeducteedType.Value) + "' ,  Alternative_Code ='" + ASPxTextBox1.Text + "'  "; // + Session["userid"]
                    //end of rev srijeeta  mantis issue 0024515
                }
                else
                {
                    //rev srijeeta  mantis issue 0024515
                    //value = "cnt_salutation=" + CmbSalutation.SelectedItem.Value + ",  cnt_firstName='" + txtFirstName.Text + "', cnt_shortName='" + UniqueNamne + "',  cnt_DOB='" + DtIncoorporationDtae + "',  cnt_legalStatus=" + cmbLegalStatus.SelectedItem.Value + ", cnt_branchId=" + branch + ",  cnt_contactSource=" + cmbSource.SelectedItem.Value + ", cnt_referedBy='" + txtReferedBy.Text + "', cnt_contactType='" + requesttype + "', cnt_contactStatus=" + cmbContactStatus.SelectedItem.Value + ", lastModifyDate ='" + today + "', lastModifyUser=" + HttpContext.Current.Session["userid"] + ",CNT_GSTIN='" + GSTIN + "',cnt_PrintNameToCheque='" + txtNameInCheque.Text + "',cnt_EntityType='" + rdl_VendorType.SelectedValue + "',Cnt_NameAsPerPan='" + NameAsperPan + "',Cnt_DeducteeStatus='" + DeducteeStatus + "',CNT_TAX_ENTITYTYPE='" + Convert.ToString(cmbTaxdeducteedType.Value) +  "'  "; // + Session["userid"]
                    value = "cnt_salutation=" + CmbSalutation.SelectedItem.Value + ",  cnt_firstName='" + txtFirstName.Text + "', cnt_shortName='" + UniqueNamne + "',  cnt_DOB='" + DtIncoorporationDtae + "',  cnt_legalStatus=" + cmbLegalStatus.SelectedItem.Value + ", cnt_branchId=" + branch + ",  cnt_contactSource=" + cmbSource.SelectedItem.Value + ", cnt_referedBy='" + txtReferedBy.Text + "', cnt_contactType='" + requesttype + "', cnt_contactStatus=" + cmbContactStatus.SelectedItem.Value + ", lastModifyDate ='" + today + "', lastModifyUser=" + HttpContext.Current.Session["userid"] + ",CNT_GSTIN='" + GSTIN + "',cnt_PrintNameToCheque='" + txtNameInCheque.Text + "',cnt_EntityType='" + rdl_VendorType.SelectedValue + "',Cnt_NameAsPerPan='" + NameAsperPan + "',Cnt_DeducteeStatus='" + DeducteeStatus + "',CNT_TAX_ENTITYTYPE='" + Convert.ToString(cmbTaxdeducteedType.Value) + "' ,  Alternative_Code ='" + ASPxTextBox1.Text + "'  "; // + Session["userid"] 
                    //end of rev srijeeta  mantis issue 0024515
                }


                #region Subhabrata
                //Subhabrata GSTIN To Be UPDATED if changes is made.
                bool IsSaved = false;
                bool flagEntity = false;
                Employee_BL ebl = new Employee_BL();
                string User_Id = Convert.ToString(Session["userid"]);
                if (Convert.ToString(hddnGSTINFlag.Value).ToUpper() == "UPDATE")
                {


                    IsSaved = ebl.AddCustVendHistory(Convert.ToString(GSTIN), Convert.ToInt32(HttpContext.Current.Session["KeyVal"]),
                        Convert.ToDateTime(dt_ApplicableFrom.Value), User_Id, "GSTIN_Vendor");
                    flagEntity = true;
                }
                else if (Convert.ToString(hddnGSTINFlag.Value).ToUpper() == "NOTUPDATE")
                {
                    IsSaved = ebl.AddCustVendHistory(Convert.ToString(GSTIN), Convert.ToInt32(HttpContext.Current.Session["KeyVal"]),
                       Convert.ToDateTime(dt_ApplicableFrom.Value), User_Id, "GSTIN_UpdateVend");
                }
                //End

                #endregion


                Int32 rowsEffected = oDBEngine.SetFieldValue("tbl_master_contact", value, " cnt_id=" + HttpContext.Current.Session["KeyVal"]);

                Int32 rowsEffected2 = oDBEngine.SetFieldValue("tbl_master_contact", "cnt_mainAccount='" + hndTaxRates_MainAccount_hidden.Value + "',cnt_subAccount='" + hndTaxRates_SubAccount_hidden.Value + "',AccountGroupID='" + ddlAssetLiability.SelectedItem.Value + "'    ", " cnt_id=" + HttpContext.Current.Session["KeyVal"]);

                string[,] EmployeeNameID = oDBEngine.GetFieldValue(" tbl_master_contact ", " case when cnt_firstName is null then '' else cnt_firstName end + ' '+case when cnt_middleName is null then '' else cnt_middleName end+ ' '+case when cnt_lastName is null then '' else cnt_lastName end+' ['+cnt_shortName+']' as name ", " cnt_internalId='" + HttpContext.Current.Session["KeyVal_InternalIDVen"] + "'", 1);
                if (EmployeeNameID[0, 0] != "n")
                {
                    lblHeader.Text = EmployeeNameID[0, 0].ToUpper();
                }

             

                // Add branches for Vendor
                string BranchList = GetBranchList();
                //if (BranchList != "")
                //{
                int brmap = reCat.insertVendorBranchMap(BranchList, Convert.ToString(HttpContext.Current.Session["KeyVal_InternalIDVen"]), Convert.ToInt16(branch));
                //}

                //dev Bapi

                string ProductListUpdate = GetProductList();
                //if (BranchList != "")
                //{
                int prmap = insertVendorProductMap(ProductListUpdate, Convert.ToString(HttpContext.Current.Session["KeyVal_InternalIDVen"]),"DV",Convert.ToInt16(HttpContext.Current.Session["userid"]));
                //end dev bapi


                if (flagEntity)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('New GSTIN Updated Successfully')</script>");
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Updated Successfully')</script>");
                }

            }
            else
            {   //INSERT
                try
                {


                    if (!reCat.isAllMandetoryDone((DataTable)Session["UdfDataOnAdd"], "DV"))
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>InvalidUDF();</script>");
                        return;
                    }

                    DateTime dtDob, dtanniversary, dtReg;

                    string dd = Convert.ToString(Session["requesttype"]);

                    if (DateOfIncoorporation.Value != null)
                    {

                        dtDob = Convert.ToDateTime(DateOfIncoorporation.Value);
                    }
                    else
                    {
                        dtDob = Convert.ToDateTime("01-01-1900");
                    }


                    dtanniversary = Convert.ToDateTime("01-01-1900");
                    dtReg = Convert.ToDateTime("01-01-1900");

                    //debjyoti Take the value from drop down and textbox
                    string valueforReffer = "";
                    if (Convert.ToBoolean(refferByDD.Value))
                    {
                        valueforReffer = Convert.ToString(RefferedByValue.Value);
                    }
                    else
                    {
                        valueforReffer = txtReferedBy.Text.Trim();
                    }


                    string GSTIN = "";
                    GSTIN = txtGSTIN1.Text.Trim() + txtGSTIN2.Text.Trim() + txtGSTIN3.Text.Trim();


                    if (GSTIN != "")
                    {
                        string DuplicateGSTINCustomer = ComBL.GetSystemSettingsResult("DuplicateGSTINCustomer");
                        if (!String.IsNullOrEmpty(DuplicateGSTINCustomer))
                        {
                            if (DuplicateGSTINCustomer == "No")
                            {
                                result = ContactGeneralBL.ISUniqueGSTIN("", "0", GSTIN, "DV");
                                if (result)
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "jAlert('Duplicate GSTIN number.');", true);
                                    return;
                                }
                            }
                        }
                    }
                    int numberingId = 0;
                    string UccName = "";
                    if (hdnAutoNumStg.Value == "DVNumb1" && hdnTransactionType.Value == "DV")
                    {
                        numberingId = Convert.ToInt32(hdnNumberingId.Value.Trim());
                        UccName = hddnDocNo.Value.Trim();

                    }
                    else
                    {
                        UccName = txtCode.Text.Trim();
                    }

                    //rev srijeeta  mantis issue 0024515
                    string Alternative_Code = "";
                    if (ASPxTextBox1.Text != "")
                    {
                        Alternative_Code = ASPxTextBox1.Text;
                    }


                    //end of rev srijeeta  mantis issue 0024515


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
                    //dev Bapi

                    string ProductList = GetProductList();
                    //End Dev Bapi

                    //rev srijeeta  mantis issue 0024515
                    //string InternalID = oHrRecritmentGeneralBL.Insert_ContactGeneral("Vendor", UccName, Convert.ToString(CmbSalutation.SelectedItem.Value), txtFirstName.Text.Trim(),
                    //                    "", "", UccName, branch, "", "", dtDob,
                    //                    dtanniversary, Convert.ToString(cmbLegalStatus.SelectedItem.Value),
                    //                     "", "", "", "", "", "", "", dtReg, "", "", Convert.ToString(cmbSource.SelectedItem.Value),
                    //                 valueforReffer, Convert.ToString(Session["requesttype"]), Convert.ToString(cmbContactStatus.SelectedItem.Value),
                    //                   Convert.ToString(HttpContext.Current.Session["userid"]), "", "", "", GSTIN, Convert.ToString(txtNameInCheque.Text), rdl_VendorType.SelectedValue, numberingId, NameAsperPan, DeducteeStatus, Convert.ToString(cmbTaxdeducteedType.Value),ProductList);
                    string InternalID = oHrRecritmentGeneralBL.Insert_ContactGeneral("Vendor", UccName, Alternative_Code, Convert.ToString(CmbSalutation.SelectedItem.Value), txtFirstName.Text.Trim(),
                                        "", "", UccName, branch, "", "", dtDob,
                                        dtanniversary, Convert.ToString(cmbLegalStatus.SelectedItem.Value),
                                         "", "", "", "", "", "", "", dtReg, "", "", Convert.ToString(cmbSource.SelectedItem.Value),
                                     valueforReffer, Convert.ToString(Session["requesttype"]), Convert.ToString(cmbContactStatus.SelectedItem.Value),
                                       Convert.ToString(HttpContext.Current.Session["userid"]), "", "", "", GSTIN, Convert.ToString(txtNameInCheque.Text), rdl_VendorType.SelectedValue, numberingId, NameAsperPan, DeducteeStatus, Convert.ToString(cmbTaxdeducteedType.Value), ProductList);
                    //end of rev srijeeta  mantis issue 0024515
                    //Add Amin Account and sub Account


                    //chinmoy  31-03-2020 start	
                    if (hdnAutoNumStg.Value == "DVNumb1")
                    {
                        if (InternalID != "")
                        {
                            BusinessLogicLayer.DBEngine BEngine = new BusinessLogicLayer.DBEngine();
                            DataTable dts = new DataTable();
                            DataTable delete = new DataTable();
                            dts = BEngine.GetDataTable("select isnull(cnt_UCC,'') cnt_UCC from tbl_master_contact where cnt_internalId='" + InternalID + "'");
                            if (dts.Rows.Count == 1)
                            {
                                if (Convert.ToString(dts.Rows[0]["cnt_UCC"]) == "Auto")
                                {
                                    delete = BEngine.GetDataTable("delete from tbl_master_contact where cnt_internalId='" + InternalID + "'");
                                    if (hdnAutoNumStg.Value == "DVNumb1")
                                    {
                                        txt_CustDocNo.Text = "Auto";
                                        txt_CustDocNo.ClientEnabled = false;
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

                    //Rev Rajdip For Pan Card
                    if (txtNumber.Text != "")
                    {
                        SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                        SqlDataAdapter da = new SqlDataAdapter("select * from tbl_master_contactRegistration WHERE crg_Number='" + txtNumber.Text.ToString() + "'", con);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            //Page.ClientScript.RegisterStartupScript(GetType(), "Script", "<script>alert('Pan No. Exists !')</script>");
                            //return;
                        }

                        if (hdnAutoNumStg.Value == "DVNumb1" && hdnTransactionType.Value == "DV")
                        {
                            DataTable dtUcc = new DataTable();
                            dtUcc = oDBEngine.GetDataTable("select ISNULL(cnt_UCC,'') cnt_UCC from tbl_master_contact where cnt_internalId='" + InternalID.Trim() + "'");

                            UccName = Convert.ToString(dtUcc.Rows[0]["cnt_UCC"]);
                        }
                        else
                        {
                            UccName = txtCode.Text.Trim();
                        }
                        DataTable dtpanduplicate=oDBEngine.GetDataTable("select  ISNULL(crg_Number,'') crg_Number from tbl_master_contactRegistration where crg_cntId='" + InternalID.Trim() + "'");
                        if (dtpanduplicate == null && dtpanduplicate.Rows.Count == 0)
                        {
                            Int32 ID = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                            ProcedureExecute pro = new ProcedureExecute("Prc_InsertPandetails");
                            pro.AddVarcharPara("@Action", 100, "INSERTPAN");
                            pro.AddVarcharPara("@crg_cntId", 50, UccName);
                            pro.AddVarcharPara("@crg_contactType", 200, "contact");
                            pro.AddVarcharPara("@crg_type", 200, "Pan Card");
                            pro.AddVarcharPara("@crg_Number", 200, txtNumber.Text.ToString());
                            pro.AddVarcharPara("@crg_place", 200, "");
                            pro.AddVarcharPara("@crg_validDate", 200, "");
                            pro.AddVarcharPara("@crg_Date", 200, "");
                            pro.AddVarcharPara("@crg_verify", 200, "0");
                            pro.AddVarcharPara("@crg_PanExmptProofType", 200, "");
                            pro.AddVarcharPara("@crg_PanExmptProofNumber", 200, "");
                            pro.AddIntegerPara("@CreateUser", ID);
                            pro.AddVarcharPara("@MESSAGE", 50, "", QueryParameterDirection.Output);
                            int i = pro.RunActionQuery();
                        }
                        //if (i < 1)
                        //{
                        //    Page.ClientScript.RegisterStartupScript(GetType(), "Script", "<script>alert('Pan No. Exists !')</script>");
                        //    return;
                        //}
                    }
                    //End Rev Rajdip
                    Int32 rowsEffected = oDBEngine.SetFieldValue("tbl_master_contact", "cnt_mainAccount='" + hndTaxRates_MainAccount_hidden.Value + "',cnt_subAccount='" + hndTaxRates_SubAccount_hidden.Value + "',AccountGroupID='" + ddlAssetLiability.SelectedItem.Value + "'", " cnt_internalId='" + InternalID + "'");

                    //Udf Add mode
                    DataTable udfTable = (DataTable)Session["UdfDataOnAdd"];
                    if (udfTable != null)
                        Session["UdfDataOnAdd"] = reCat.insertRemarksCategoryAddMode("DV", Convert.ToString(InternalID), udfTable, Convert.ToString(Session["userid"]));

                    string BranchList = GetBranchList();

                    int brmap = reCat.insertVendorBranchMap(BranchList, InternalID, Convert.ToInt16(branch));


                    //----------- Tier Structure End--------------




                    string[,] cnt_id = oDBEngine.GetFieldValue(" tbl_master_contact", " cnt_id", " cnt_internalId='" + InternalID + "'", 1);
                    if (Convert.ToString(cnt_id[0, 0]) != "n")
                    {
                        Response.Redirect("HRrecruitmentagent_general.aspx?id=" + Convert.ToString(cnt_id[0, 0]), false);
                    }

                }
                catch
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "Script", "<script>jAlert('Code Already Exists !')</script>");
                }
            }


        }


        public string GetBranchList()
        {
            DataTable branchListtable = (DataTable)Session["BranchListTableForVendor"];
            string branchlist = "";
            if (branchListtable != null)
            {
                foreach (DataRow dr in branchListtable.Rows)
                {
                    branchlist = branchlist + "," + Convert.ToString(dr["Branch_id"]);

                }
            }

            branchlist = branchlist.TrimStart(',');
            return branchlist;
        }
        public string GetProductList()
        {
            DataTable productListtable = (DataTable)Session["ProductListTableForVendor"];
            string productlist = "";
            if (productListtable != null)
            {
                foreach (DataRow dr in productListtable.Rows)
                {
                    productlist = productlist + "," + Convert.ToString(dr["Product_id"]);

                }
            }

            productlist = productlist.TrimStart(',');
            return productlist;
        }
        //name: Debjyoti 28-11-2016 
        //Purpose: bellow function will format the dateedit control with proper format 
        public void SetDateFormat()
        {
            DateOfIncoorporation.TimeSectionProperties.Visible = false;
            DateOfIncoorporation.UseMaskBehavior = true;
            DateOfIncoorporation.EditFormatString = "dd-MM-yyyy";
            DateOfIncoorporation.DisplayFormatString = "dd-MM-yyyy";

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


        [WebMethod]
        public static List<string> GetReffer(string sourceId, string hdKeyValIntId)
        {
            //Purpose : Replace .ToString() with Convert.ToString(..)
            //Name : Sudip 
            // Dated : 22-12-2016

            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]); MULTI

            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = new DataTable();
            DT.Rows.Clear();
            switch (Convert.ToString(sourceId))
            {
                case "1":
                case "2":
                case "5":
                case "6":
                case "7":
                case "9":
                case "11":
                case "12":
                case "13":
                case "15":
                case "16":
                case "17":
                case "18":
                    break;
                case "0":
                    DT = oDBEngine.GetDataTable("tbl_master_contact con,tbl_master_branch b", "   (ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') + ' [' + ISNULL(con.cnt_shortName, '')+']'+'[' + b.branch_description + ']') AS cnt_firstName,con.cnt_internalId as cnt_internalId", " (cnt_contactType='EM')   and con.cnt_branchid=b.branch_id");
                    break;
                case "3":
                    DT = oDBEngine.GetDataTable("tbl_master_contact con,tbl_master_branch b", "   (ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') + ' [' + ISNULL(con.cnt_shortName, '')+']'+'[' + b.branch_description + ']') AS cnt_firstName,con.cnt_internalId as cnt_internalId", " cnt_contactType='DV'  and con.cnt_branchid=b.branch_id");
                    break;
                case "4":
                    DT = oDBEngine.GetDataTable("tbl_master_contact con,tbl_master_branch b", "    (ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') + ' [' + ISNULL(con.cnt_shortName, '')+']'+'[' + b.branch_description + ']') AS cnt_firstName,con.cnt_internalId as cnt_internalId", " (cnt_contactType='EM' or cnt_contactType='CL')  and con.cnt_branchid=b.branch_id");
                    break;
                case "8":
                    DT = oDBEngine.GetDataTable("tbl_master_contact con,tbl_master_branch b", "   (ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') + ' [' + ISNULL(con.cnt_shortName, '')+']'+'[' + b.branch_description + ']') AS cnt_firstName,con.cnt_internalId as cnt_internalId", " cnt_contactType='RA' and con.cnt_branchid=b.branch_id");
                    break;
                case "10":
                    DT = oDBEngine.GetDataTable("tbl_master_contact con,tbl_master_branch b", "   (ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') + ' [' + ISNULL(con.cnt_shortName, '')+']'+'[' + b.branch_description + ']') AS cnt_firstName,con.cnt_internalId as cnt_internalId", " cnt_contactType='RC'  and con.cnt_branchid=b.branch_id");
                    break;
                case "14":
                    DT = oDBEngine.GetDataTable("tbl_master_contact", " Top 10 (ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') + ' [' + ISNULL(cnt_shortName, '')+']') AS cnt_firstName,cnt_internalId", " cnt_internalId='" + hdKeyValIntId + "'  ");
                    // using InterNal Id
                    break;
                case "20":
                    DT = oDBEngine.GetDataTable("tbl_master_contact con,tbl_master_branch b", "  (ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') + ' [' + ISNULL(con.cnt_UCC, '')+']'+'[' + b.branch_description + ']') AS cnt_firstName,con.cnt_internalId as cnt_internalId", " cnt_contactType='CL'    and con.cnt_branchid=b.branch_id");
                    break;
                case "24":
                    DT = oDBEngine.GetDataTable("tbl_master_contact con,tbl_master_branch b", "  (ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') + ' [' + ISNULL(con.cnt_shortName, '')+']'+'[' + b.branch_description + ']') AS cnt_firstName,con.cnt_internalId as cnt_internalId", " cnt_contactType='SB'    and con.cnt_branchid=b.branch_id");
                    break;
                case "25":
                    DT = oDBEngine.GetDataTable("tbl_master_contact con,tbl_master_branch b", "  (ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') + ' [' + ISNULL(con.cnt_shortName,'')+']'+'[' + b.branch_description + ']') AS cnt_firstName,con.cnt_internalId as cnt_internalId", " cnt_contactType='FR'    and con.cnt_branchid=b.branch_id");
                    break;
                default:
                    DT = oDBEngine.GetDataTable("tbl_master_contact con,tbl_master_branch b", "   (ISNULL(con.cnt_firstName, '') + ' ' + ISNULL(con.cnt_middleName, '') + ' ' + ISNULL(con.cnt_lastName, '') + ' [' + ISNULL(con.cnt_shortName, '')+']'+'[' + b.branch_description + ']') AS cnt_firstName,con.cnt_internalId as cnt_internalId", " cnt_contactType='PR'    and con.cnt_branchid=b.branch_id");
                    break;
            };
            List<string> obj = new List<string>();
            foreach (DataRow dr in DT.Rows)
            {

                obj.Add(Convert.ToString(dr["cnt_firstName"]) + "|" + Convert.ToString(dr["cnt_internalId"]));
            }
            return obj;
        }
        [WebMethod]
        public static bool CheckUniqueCode(string CategoriesShortCode, string Code)
        {
            bool flag = false;
            try
            {
                BusinessLogicLayer.MShortNameCheckingBL obj = new BusinessLogicLayer.MShortNameCheckingBL();
                flag = obj.CheckUnique(CategoriesShortCode.Trim(), Code, "VendorServiceProvider");

            }
            catch (Exception ex)
            {

            }
            finally
            {
            }
            return flag;
        }


        protected void branchGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string receviedString = e.Parameters;
            branchGrid.JSProperties["cpReceviedString"] = receviedString;

            if (receviedString == "SelectAllBranchesFromList")
            {
                branchGrid.Selection.SelectAll();
            }

            if (receviedString == "ClearSelectedBranch")
            {
                branchGrid.Selection.UnselectAll();
            }

            else if (receviedString == "SetAllRecordToDataTable")
            {
                List<object> branchList = branchGrid.GetSelectedFieldValues("branch_id");
                CreateBranchTable();
                DataTable branchListtable = (DataTable)Session["BranchListTableForVendor"];
                foreach (object obj in branchList)
                {
                    if (Convert.ToInt32(obj) != 0)
                        branchListtable.Rows.Add(Convert.ToInt32(obj));

                }

                if (hdnBranchAllSelected.Value == "0")
                {
                    if (branchListtable.Rows.Count > 0)
                    {
                        branchGrid.JSProperties["cpBrselected"] = 1;

                    }
                    else
                    {
                        branchGrid.JSProperties["cpBrselected"] = 0;
                    }

                }

                Session["BranchListTableForVendor"] = branchListtable;
            }
            else if (receviedString == "SetAllSelectedRecord")
            {
                DataTable branchListtable = (DataTable)Session["BranchListTableForVendor"];
                branchGrid.Selection.UnselectAll();
                if (branchListtable != null)
                {
                    foreach (DataRow dr in branchListtable.Rows)
                    {
                        branchGrid.Selection.SelectRowByKey(dr["Branch_id"]);
                        if (Convert.ToString(dr["Branch_id"]) == "0")
                        {
                            branchGrid.JSProperties["cpBrChecked"] = 1;
                        }
                    }
                }
            }


        }

        //rev Bapi
        protected void productGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string receviedString = e.Parameters;
            productGrid.JSProperties["cpReceviedString"] = receviedString;

            if (receviedString == "SelectAllProductsFromList")
            {
                productGrid.Selection.SelectAll();
            }

            if (receviedString == "ClearSelectedproduct")
            {
                productGrid.Selection.UnselectAll();
            }

            else if (receviedString == "SetAllRecordToDataTable")
            {
                List<object> productList = productGrid.GetSelectedFieldValues("product_id");
                CreateProductTable();
                DataTable productListtable = (DataTable)Session["ProductListTableForVendor"];
                foreach (object obj in productList)
                {
                    if (Convert.ToInt32(obj) != 0)
                        productListtable.Rows.Add(Convert.ToInt32(obj));

                }

                if (hdnBranchAllSelected.Value == "0")
                {
                    if (productListtable.Rows.Count > 0)
                    {
                        productGrid.JSProperties["cpPrselected"] = 1;

                    }
                    else
                    {
                        productGrid.JSProperties["cpPrselected"] = 0;
                    }

                }

                Session["ProductListTableForVendor"] = productListtable;
            }
            else if (receviedString == "SetAllSelectedRecord")
            {
                DataTable productListtable = (DataTable)Session["ProductListTableForVendor"];
                productGrid.Selection.UnselectAll();
                if (productListtable != null)
                {
                    foreach (DataRow dr in productListtable.Rows)
                    {
                        productGrid.Selection.SelectRowByKey(dr["Product_id"]);
                        if (Convert.ToString(dr["Product_id"]) == "0")
                        {
                            productGrid.JSProperties["cpPrChecked"] = 1;
                        }
                    }
                }
            }


        }



        


        private void ShowProductName(string Keyvalue)
        {
            string SelectedProduct = string.Empty;
            DataTable productListtable = oDBEngine.GetDataTable("select m.ProductID Product_id,b.sProducts_Name as product_Description   from ContactProductMasterMapping m left join MASTER_SPRODUCTS b on m.ProductID=b.sProducts_ID where m.cnt_InternalID='" + Keyvalue + "'");

            if (productListtable != null && productListtable.Rows.Count > 0 && Convert.ToString(productListtable.Rows[0]["Product_Id"]) == "0")
            { lblSelectedProduct.Text = "All Product"; }
            else
            {
                if (productListtable != null)
                {
                    foreach (DataRow dr in productListtable.Rows)
                    {

                        SelectedProduct = SelectedProduct + ", " + dr["product_description"];
                    }
                }
                if (SelectedProduct.Length > 1)
                {
                    lblSelectedProduct.Text = SelectedProduct.Substring(1, SelectedProduct.Length - 1);
                }
                else
                {
                    lblSelectedProduct.Text = "";
                }
            }


        }

        private void ShowProductNameVendorMapping(string Keyvalue)
        {
            string SelectedProduct = string.Empty;
            DataTable productListtable = oDBEngine.GetDataTable("select m.PRIODUCT_ID Product_id,b.sProducts_Name as product_Description   from MasterProduct_Vendors_MappingDetails m left join MASTER_SPRODUCTS b on m.PRIODUCT_ID=b.sProducts_ID where m.VENDOR_IDS='" + Keyvalue + "'");

            if (productListtable != null && productListtable.Rows.Count > 0 && Convert.ToString(productListtable.Rows[0]["Product_Id"]) == "0")
            { lblSelectedProduct.Text = "All Product"; }
            else
            {
                if (productListtable != null)
                {
                    foreach (DataRow dr in productListtable.Rows)
                    {

                        SelectedProduct = SelectedProduct + ", " + dr["product_description"];
                    }
                }
                if (SelectedProduct.Length > 1)
                {
                    lblSelectedProduct.Text = SelectedProduct.Substring(1, SelectedProduct.Length - 1);
                }
                else
                {
                    lblSelectedProduct.Text = "";
                }
            }


        }
        public int insertVendorProductMap(string ProductIDs, string Ven_Internalid,string cnntype,int UserID )
        {
            ProcedureExecute proc;
            try
            {


                using (proc = new ProcedureExecute("Prc_VendorProductMap"))
                {

                    proc.AddVarcharPara("@ProductList", 500, ProductIDs);
                    proc.AddVarcharPara("@Vendor_InternalId", 50, Ven_Internalid);
                    proc.AddIntegerPara("@UserId", UserID);
                    proc.AddVarcharPara("@Cnnt",100, cnntype);
                    
                    int i = proc.RunActionQuery();
                    return i;
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



        private void SetProductRecordToSessionTable(string Keyvalue)
        {
            DataTable productListtable = oDBEngine.GetDataTable("select  ProductID Product_id  from ContactProductMasterMapping where cnt_InternalID='" + Keyvalue + "'");
            Session["ProductListTableForVendor"] = productListtable;
        }
        private void SetProductRecordVendorMappingToSessionTable(string Keyvalue)
        {
            DataTable productListtable = oDBEngine.GetDataTable("select  PRIODUCT_ID Product_id  from MasterProduct_Vendors_MappingDetails where VENDOR_IDS='" + Keyvalue + "'");
            Session["ProductListTableForVendor"] = productListtable;
        }


        //End Rev Bapi
        private void SetBranchRecordToSessionTable(string Keyvalue)
        {
            DataTable branchListtable = oDBEngine.GetDataTable("select branch_id Branch_id from tbl_master_VendorBranch_map where Ven_InternalId='" + Keyvalue + "'");
            Session["BranchListTableForVendor"] = branchListtable;
        }

        



        private void ShowBranchName(string Keyvalue)
        {
            string SelectedBranch = string.Empty;
            DataTable branchListtable = oDBEngine.GetDataTable("select m.branch_id Branch_id,b.branch_description  from tbl_master_VendorBranch_map m left join tbl_master_branch b on m.branch_id=b.branch_id where m.Ven_InternalId='" + Keyvalue + "'");

            if (branchListtable != null && branchListtable.Rows.Count > 0 && Convert.ToString(branchListtable.Rows[0]["Branch_Id"]) == "0")
            { lblSelectedBranch.Text = "All Branch"; }
            else
            {
                if (branchListtable != null)
                {
                    foreach (DataRow dr in branchListtable.Rows)
                    {

                        SelectedBranch = SelectedBranch + ", " + dr["branch_description"];
                    }
                }
                if (SelectedBranch.Length > 1)
                {
                    lblSelectedBranch.Text = SelectedBranch.Substring(1, SelectedBranch.Length - 1);
                }
                else
                {
                    lblSelectedBranch.Text = "";
                }
            }


        }
        public void CreateBranchTable()
        {
            DataTable branchListtable = new DataTable();
            branchListtable.Columns.Add("Branch_id", typeof(System.Int32));
            Session["BranchListTableForVendor"] = branchListtable;
        }

        //Rev Bapi

        public void CreateProductTable()
        {
            DataTable productListtable = new DataTable();
            productListtable.Columns.Add("Product_id", typeof(System.Int32));
            Session["ProductListTableForVendor"] = productListtable;
        }
        //End Rev Bapi


        public bool isDropDown(int val)
        {
            //Purpose : Replace .ToString() with Convert.ToString(..)
            //Name : Sudip 
            // Dated : 22-12-2016

            switch (Convert.ToString(val))
            {
                case "1":
                    return false;
                case "2":
                    return false;
                case "5":
                    return false;
                case "6":
                    return false;
                case "7":
                    return false;
                case "9":
                    return false;
                case "11":
                    return false;
                case "12":
                    return false;
                case "13":
                    return false;
                case "15":
                    return false;
                case "16":
                    return false;
                case "17":
                    return false;
                case "18":
                    return false;
                    break;
                case "0":
                    return true;
                    break;
                case "3":
                    return true;
                    break;
                case "4":
                    return true;
                    break;
                case "8":
                    return true;
                    break;
                case "10":
                    return true;
                    break;
                case "14":
                    //   DT = oDBEngine.GetDataTable("tbl_master_contact", " Top 10 (ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') + ' [' + ISNULL(cnt_shortName, '')+']') AS cnt_firstName,cnt_internalId", " cnt_internalId='" + Session["KeyVal_InternalID"].ToString() + "'  ");
                    return true;
                    break;
                case "20":
                    return true;
                    break;
                case "24":
                    return true;
                    break;
                case "25":
                    return true;
                    break;
                default:
                    return false;
                    break;
            };
            return false;
        }
        [WebMethod]
        public static List<string> GetMainAccountList(string reqStr)
        {
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]); MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = new DataTable();
            DT.Rows.Clear();
            // DT = oDBEngine.GetDataTable("Master_MainAccount", "MainAccount_Name,MainAccount_AccountCode ", " MainAccount_Name like '" + reqStr + "%'");

            DT = oDBEngine.GetDataTable("Master_MainAccount", "MainAccount_Name, MainAccount_AccountCode ", " MainAccount_AccountCode not like 'SYS%'");

            List<string> obj = new List<string>();
            foreach (DataRow dr in DT.Rows)
            {

                obj.Add(Convert.ToString(dr["MainAccount_Name"]) + "|" + Convert.ToString(dr["MainAccount_AccountCode"]));
            }
            return obj;
        }
        [WebMethod]
        public static List<string> GetSubAccountList(string reqStr, string mainreqStr)
        {
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = new DataTable();

            //  DT = oDBEngine.GetDataTable("Master_SubAccount", "SubAccount_Name,SubAccount_ReferenceID ", " SubAccount_MainAcReferenceID = '" + mainreqStr + "' and SubAccount_Name like '" + reqStr + "%'");
            ProcedureExecute proc = new ProcedureExecute("prc_ProductMaster_bindData");
            proc.AddVarcharPara("@action", 20, "GetMainAccount");
            DT = proc.GetTable();
            List<string> obj = new List<string>();
            foreach (DataRow dr in DT.Rows)
            {

                //  obj.Add(Convert.ToString(dr["SubAccount_Name"]) + "|" + Convert.ToString(dr["SubAccount_ReferenceID"]));
            }
            return obj;
        }

        protected int getUdfCount()
        {
            DataTable udfCount = oDBEngine.GetDataTable("select 1 from tbl_master_remarksCategory rc where cat_applicablefor='DV' and ( exists (select * from tbl_master_udfGroup where id=rc.cat_group_id and grp_isVisible=1) or rc.cat_group_id=0)");
            return udfCount.Rows.Count;
        }

        [WebMethod]
        public static List<string> GetAccountGroupList()
        {
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);   MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

            DataTable DT = oDBEngine.GetDataTable("select AccountGroup_ReferenceID, AccountGroup_Name from Master_AccountGroup  where AccountGroup_Type= 'Liability' or AccountGroup_Type='Asset'");


            List<string> obj = new List<string>();
            foreach (DataRow dr in DT.Rows)
            {

                obj.Add(Convert.ToString(dr["AccountGroup_Name"]) + "|" + Convert.ToString(dr["AccountGroup_ReferenceID"]));
            }
            return obj;
        }



        public string Alternative_Code { get; set; }
    }
}