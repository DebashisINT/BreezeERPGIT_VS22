using BusinessLogicLayer;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;
using DataAccessLayer;
using DevExpress.Web;
using EntityLayer.CommonELS;
using ERP.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;


namespace ERP.OMS.Management.Master
{
    public partial class management_master_HRrecruitmentagent : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        Int32 ID;
        public string pageAccess = "";
        //DBEngine oDBEngine = new DBEngine(string.Empty);
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty); MULTI
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        clsDropDownList oclsDropDownList = new clsDropDownList();
        CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        BusinessLogicLayer.Recruitment_Agents oHrRecritmentGeneralBL = new BusinessLogicLayer.Recruitment_Agents();
        BusinessLogicLayer.RemarkCategoryBL reCat = new BusinessLogicLayer.RemarkCategoryBL();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
                //Rev Rajdip
                //branchdtl.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                //BranchdataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                //GstinSettingsButton.contact_type = "VENDOR";
                //End Rev rajdip
            }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            branchdtl.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            BranchdataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            GstinSettingsButton.contact_type = "VENDOR";
        }
        //rev rajdip
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
        public void CreateBranchTable()
        {
            DataTable branchListtable = new DataTable();
            branchListtable.Columns.Add("Branch_id", typeof(System.Int32));
            Session["BranchListTableForVendor"] = branchListtable;
        }
        private void SetBranchRecordToSessionTable(string Keyvalue)
        {
            DataTable branchListtable = oDBEngine.GetDataTable("select branch_id Branch_id from tbl_master_VendorBranch_map where Ven_InternalId='" + Keyvalue + "'");
            Session["BranchListTableForVendor"] = branchListtable;
        }

        //end rev rajdip
        protected void Page_Load(object sender, EventArgs e) 
        {
            // rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/HRrecruitmentagent.aspx");
            //if (HttpContext.Current.Session["userid"] == null)
            //{
            //   //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            //}
            //Session["requesttype"] = Request.QueryString["id"].ToString();


            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/HRrecruitmentagent.aspx?requesttype=VendorService");
            CommonBL ComBL = new CommonBL();
            string AutoNumberAllow = ComBL.GetSystemSettingsResult("UniqueAutoNumberVendorMaster");
            if (HttpContext.Current.Session["userid"] != null)
            {
                if (!IsPostBack)
                {
                    //chinmoy added 02-04-2020 start	
                    //chinmoy added for Numbering Scheme start	
                    if (Convert.ToString(Session["requesttype"]) == "DV")
                    {
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
                    }
                    //End	
                    //End
                    //Rev rajdip
                    DDLBind();
                    branchdtl.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    branchdtl.SelectCommand = "select '0' as branch_id ,  '--ALL--' as branch_description union all select branch_id,branch_description from tbl_master_branch order by branch_description";
                    cmbMultiBranches.DataBind();
                    cmbMultiBranches.Value = "0";
                    Session["ContactType"] = "Relationship Manager";
                    
                    if (Request.QueryString["id"] != "ADD")
                    {
                        //hddnApplicationMode.Value = "Edit";
                        hddnApplicationMode.Value = "Copy";
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
                            HttpContext.Current.Session["KeyVal"] = "0";
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
                            //                        "cnt_ucc, cnt_salutation,  cnt_firstName, cnt_middleName, cnt_lastName, cnt_shortName, cnt_branchId, cnt_sex, cnt_maritalStatus, case cnt_DOB when '1/1/1900 12:00:00 AM' then null else cnt_DOB end as cnt_DOB,case cnt_anniversaryDate when '1/1/1900 12:00:00 AM' then null else cnt_anniversaryDate end as cnt_anniversaryDate, cnt_legalStatus, cnt_education, cnt_profession, cnt_organization, cnt_jobResponsibility, cnt_designation, cnt_industry, cnt_contactSource, cnt_referedBy, cnt_contactType, cnt_contactStatus,CNT_GSTIN,cnt_mainAccount,cnt_PrintNameToCheque,cnt_EntityType,AccountGroupID",
                            //                        " cnt_id=" + ID, 27);
                            ContactData = oDBEngine.GetFieldValue("tbl_master_contact",
                                                    "cnt_ucc, cnt_salutation,  cnt_firstName, cnt_middleName, cnt_lastName, cnt_shortName, cnt_branchId, cnt_sex, cnt_maritalStatus, case cnt_DOB when '1/1/1900 12:00:00 AM' then null else cnt_DOB end as cnt_DOB,case cnt_anniversaryDate when '1/1/1900 12:00:00 AM' then null else cnt_anniversaryDate end as cnt_anniversaryDate, cnt_legalStatus, cnt_education, cnt_profession, cnt_organization, cnt_jobResponsibility, cnt_designation, cnt_industry, cnt_contactSource, cnt_referedBy, cnt_contactType, cnt_contactStatus,CNT_GSTIN,cnt_mainAccount,cnt_PrintNameToCheque,cnt_EntityType,AccountGroupID,Alternative_Code",
                                                    " cnt_id=" + ID, 28);
                            //end of rev srijeeta  mantis issue 0024515
                        }
                        else
                        {
                            //rev srijeeta  mantis issue 0024515
                            //ContactData = oDBEngine.GetFieldValue("tbl_master_contact",
                            //                        "cnt_ucc, cnt_salutation,  cnt_firstName, cnt_middleName, cnt_lastName, cnt_shortName, cnt_branchId, cnt_sex, cnt_maritalStatus, case cnt_DOB when '1/1/1900 12:00:00 AM' then null else cnt_DOB end as cnt_DOB,case cnt_anniversaryDate when '1/1/1900 12:00:00 AM' then null else cnt_anniversaryDate end as cnt_anniversaryDate, cnt_legalStatus, cnt_education, cnt_profession, cnt_organization, cnt_jobResponsibility, cnt_designation, cnt_industry, cnt_contactSource, cnt_referedBy, cnt_contactType, cnt_contactStatus,CNT_GSTIN,cnt_mainAccount,cnt_PrintNameToCheque,cnt_EntityType,AccountGroupID",
                            //                        " cnt_id=" + HttpContext.Current.Session["KeyVal"], 27);
                            ContactData = oDBEngine.GetFieldValue("tbl_master_contact",
                                                    "cnt_ucc, cnt_salutation,  cnt_firstName, cnt_middleName, cnt_lastName, cnt_shortName, cnt_branchId, cnt_sex, cnt_maritalStatus, case cnt_DOB when '1/1/1900 12:00:00 AM' then null else cnt_DOB end as cnt_DOB,case cnt_anniversaryDate when '1/1/1900 12:00:00 AM' then null else cnt_anniversaryDate end as cnt_anniversaryDate, cnt_legalStatus, cnt_education, cnt_profession, cnt_organization, cnt_jobResponsibility, cnt_designation, cnt_industry, cnt_contactSource, cnt_referedBy, cnt_contactType, cnt_contactStatus,CNT_GSTIN,cnt_mainAccount,cnt_PrintNameToCheque,cnt_EntityType,AccountGroupID,Alternative_Code",
                                                    " cnt_id=" + HttpContext.Current.Session["KeyVal"], 28);

                            //end of rev srijeeta  mantis issue 0024515

                        }

                        //____________Value Allocation_______________//
                        string vendorid = ID.ToString();
                        ValueAllocation(ContactData);
                        DataTable Schemadt = GetData(vendorid, "GetPan");
                        if (Schemadt.Rows.Count > 0)
                        {
                            if (Schemadt.Rows[0]["crg_Number"].ToString() != "" && Schemadt.Rows[0]["crg_Number"].ToString() != null && Schemadt.Rows[0]["crg_Number"].ToString() != "0")
                            {
                                string Pan = Schemadt.Rows[0]["crg_Number"].ToString();
                                //txtNumber.Text = Pan.ToString();
                                // ddl_numberingScheme.SelectedValue = schemaid.ToString();
                            }
                        }
                        SetBranchRecordToSessionTable(Convert.ToString(HttpContext.Current.Session["KeyVal_InternalIDVen"]));
                        ShowBranchName(Convert.ToString(HttpContext.Current.Session["KeyVal_InternalIDVen"]));
                    }
                    else
                    {
                        hddnApplicationMode.Value = "Add";
                        //GstinSettingsButton.Visible = false;

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
                        //DisabledTabPage();

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
                    //ENd Rev Rajdip
                    EmployeeDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

                    EmployeeGrid.SettingsCookies.CookiesID = "BreeezeErpGridCookiesHRrecruitmentagentEmployeeGrid";

                    this.Page.ClientScript.RegisterStartupScript(GetType(), "setCookieOnStorage", "<script>addCookiesKeyOnStorage('BreeezeErpGridCookiesHRrecruitmentagentEmployeeGrid');</script>");
                    Session["exportval"] = null;

                    //---------------------------------------------------------------------------
                    CommonBL cbl = new CommonBL();
                    string ISLigherpage = cbl.GetSystemSettingsResult("LighterVendorEntryPage");
                    if (!String.IsNullOrEmpty(ISLigherpage))
                    {
                        if (ISLigherpage == "Yes")
                        {
                            hidIsLigherContactPage.Value = "1";
                        }
                    }
                    //--------------------------------------------------------------------------
                     //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>"); 
                    //string requesttype = Convert.ToString(Request.QueryString["requesttype"]);
                    //string ContType = "";
                    //switch (requesttype)
                    //{
                    //    case "DataVendor":
                    //        ContType = "Data Vendor";
                    //        rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/HRrecruitmentagent.aspx?requesttype=DataVendor");
                    //        break;
                    //    case "VendorService":
                    //        ContType = "Vendor Service Provider";
                    //        rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/HRrecruitmentagent.aspx?requesttype=VendorService");
                    //        break;
                    //}
                    //Session["requesttype"] = ContType;
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            //Comment By sanjib due to validate branchwise 1612017

            //EmployeeDataSource.SelectCommand = "select tbl_master_contact.cnt_id AS cnt_Id,tbl_master_contact.cnt_internalId AS Id,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name,"+
            //  " (SELECT DISTINCT STUFF( (select case when (ISNULL(phf_phoneNumber,'')!='' And phf_type='Residence') then ' (R)'+ phf_phoneNumber + ' '  "+
            //  " when (ISNULL(phf_phoneNumber,'')!='' And phf_type='Office') then ' (O)'+ phf_phoneNumber + ' ' "+
            //    " when (ISNULL(phf_phoneNumber,'')!='' And phf_type='Correspondence') then ' (C)'+ phf_phoneNumber + ' ' "+
            //    " when (ISNULL(phf_phoneNumber,'')!='' And phf_type='Mobile') then ' (M)'+ phf_phoneNumber + ' '"+
            //    " when (ISNULL(phf_phoneNumber,'')!='' And phf_type='Fax') then ' (F)'+ phf_phoneNumber + ' ' else '' end "+
            //    " from tbl_master_phonefax where phf_cntId=tbl_master_contact.cnt_internalId FOR XML PATH('')),1,1,'') as Numbers FROM tbl_master_phonefax) as phone,"+
            //   "cnt_shortName as Unique_ID,(Select cntstu_contactStatus from tbl_master_contactstatus where cntstu_id=cnt_contactStatus) as Status,cnt_PrintNameToCheque,CNT_GSTIN gstin,AG.AccountGroup_Name " +
            //"from tbl_master_contact left JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id left join master_AccountGroup AG on tbl_master_contact.AccountGroupID=AG.AccountGroup_ReferenceID   where  cnt_contacttype='DV'  order by cnt_id desc";

            //Bellow line commented by debjyoti 
            //Reason: On this page id is always null, for this reason request type become blank             
            //     Session["requesttype"] = Convert.ToString(Request.QueryString["id"]);
            Session["requesttype"] = "DV";
           
        }
        public DataTable GetData(string vendorid, string Action)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GetVendordata");
            proc.AddVarcharPara("@Action", 100, Action);
            proc.AddVarcharPara("@Id", 4000, vendorid);
            ds = proc.GetTable();
            return ds;
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

                txtCode.Text = ContactData[0, 5];
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
              
                //Debjyoti GSTIN 060217
                string GSTIN = "";
                
                #region Rajdip For GST IN
                //if (ContactData[0, 22] != "")
                //{

                //    GSTIN = ContactData[0, 22];
                //    txtGSTIN1.Text = GSTIN.Substring(0, 2);
                //    txtGSTIN2.Text = GSTIN.Substring(2, 10);
                //    txtGSTIN3.Text = GSTIN.Substring(12, 3);
                //    radioregistercheck.SelectedValue = "1";

                //    hddnGSTIN2Val.Value = Convert.ToString(txtGSTIN1.Text) + Convert.ToString(txtGSTIN2.Text) + Convert.ToString(txtGSTIN3.Text);
                //}
                //else
                //{
                //    radioregistercheck.SelectedValue = "0";
                //    hddnGSTIN2Val.Value = "";
                //}
                radioregistercheck.SelectedValue = "0";
                #endregion Rajdip
                #region Subhabrata/BindApplicableFrom
                //Subhabrata

                DataTable dt_CustVendHistory = null;
                dt_CustVendHistory = objCRMSalesOrderDtlBL.GetCustVendHistoryId(Convert.ToString(HttpContext.Current.Session["KeyVal"]));
                if (dt_CustVendHistory != null && dt_CustVendHistory.Rows.Count > 0)
                {
                   // dt_ApplicableFrom.Date = DateTime.ParseExact(Convert.ToString(dt_CustVendHistory.Rows[0]["ApplicableFrom"]), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                }

                //End
                #endregion

                hndTaxRates_MainAccount_hidden.Value = ContactData[0, 23];

                txtNameInCheque.Text = Convert.ToString(ContactData[0, 24]);
                rdl_VendorType.SelectedValue = Convert.ToString(ContactData[0, 25]);
                ddlAssetLiability.SelectedValue = Convert.ToString(ContactData[0, 26]);
                //rev srijeeta  mantis issue 0024515
                
                alttext.Text = Convert.ToString(ContactData[0, 27]);
                //rev srijeeta  mantis issue 0024515
                

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
        //Rev Rajdip
        public void DDLBind()
        {


            string[,] Data = oDBEngine.GetFieldValue("tbl_master_salutation", "sal_id, sal_name", null, 2, "sal_name");

            // oclsDropDownList.AddDataToDropDownList(Data, CmbSalutation);
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
        //rev rajdip
        public class Classcopy
        {
            public string id { get; set; }
            //public string Name { get; set; }
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
        }
        [WebMethod]
        public static List<Classcopy> GetDataFromVendor(string keyValue)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            string[,] ContactData;
            DataTable dtAddress = new DataTable();
            //int ID = Convert.ToInt32(keyValue.ToString());

            //if (ID != 0) // lead edit
            //{


            //if (HttpContext.Current.Session["userid"] != null)
            //{
            //SearchKey = SearchKey.Replace("'", "''");
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            //DataTable classes = new DataTable();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("prc_GetCopyToVendordata", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@VendorId", keyValue);
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
                                  }).ToList();
            //}


            //}

            return Classcopytoproduct;

        }
        //End Rev Rajdip
        //End Rev rajdip
        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "cnt_Id";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString; MULTI
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            ERPDataClassesDataContext dc1 = new ERPDataClassesDataContext(connectionString);

            var q = from d in dc1.v_VendorMasterLists

                    orderby d.CreateDate descending

                    select d;
            e.QueryableSource = q;
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
            }
        }
        public void bindexport(int Filter)
        {
            EmployeeGrid.Columns[3].Visible = false;
            string filename = "Vendors/Service Providers";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Vendors/Service Providers";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";

            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsToResponse();
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }
        }
        //Rev Rajdip
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Boolean result = false;
            hdnflg.Value = "1";
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]); multi
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            string branch = string.Empty;
            branch = "0";
            string OLDID;
            OLDID = Request.QueryString["id"].ToString();
            if (hdnflag.Value.ToString() == "1")
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "jAlert('Please Enter Valid PAN');", true);
                //ClientScript.RegisterStartupScript(typeof(Page), "Confirm", "<script type='text/javascript'>callConfirm();</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Please Enter Valid PAN.!')</script>");
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript3", "<script >hideotherstatus();</script>");
                return;
                // Page.Response.Redirect(Page.Request.Url.ToString(), true);
            }
            //*************************************************************INSERT*************************************************************
            try
            {
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                string uniqueid = "";
                int NumberingId = 0;
                if ((hdnAutoNumStg.Value == "DVNumb1") && (hdnTransactionType.Value == "DV"))
                {
                    NumberingId = Convert.ToInt32(hdnNumberingId.Value.Trim());
                    uniqueid = hddnDocNo.Value.Trim();
                }
                else
                {
                    uniqueid = txtCode.Text.ToString().Trim();
                }
                if(uniqueid.ToString().ToUpper()!="AUTO")
                { 
                SqlDataAdapter daunique = new SqlDataAdapter("select * from tbl_master_contact where cnt_UCC='" + uniqueid + "'", con);
                DataTable dtunique = new DataTable();
                daunique.Fill(dtunique);
                if (dtunique.Rows.Count > 0)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Unique Code Already Exist!')</script>");
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript3", "<script >hideotherstatus();</script>");
                    return;
                }
                }
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
                string valueforReffer = "";
                if (Convert.ToBoolean(refferByDD.Value))
                {
                    valueforReffer = Convert.ToString(RefferedByValue.Value);
                }
                else
                {
                    valueforReffer = txtReferedBy.Text.Trim();
                }

                //rev srijeeta  mantis issue 0024515
                string Alternative_Code = "";
                if (alttext.Text != "")
                {
                    Alternative_Code = alttext.Text;
                }
                //end of rev srijeeta  mantis issue
                string GSTIN = "";
                GSTIN = txtGSTIN1.Text.Trim() + txtGSTIN2.Text.Trim() + txtGSTIN3.Text.Trim();


                if (GSTIN != "")
                {
                    result = ContactGeneralBL.ISUniqueGSTIN("", "0", GSTIN, "DV");
                    if (result)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script>jAlert('Duplicate GSTIN number.!')</script>");
                        Page.ClientScript.RegisterStartupScript(GetType(), "JScript3", "<script >hideotherstatus();</script>");
                        return;
                    }
                }
                if (hdnflg.Value != "0")
                {
                    bool IsSaved = false;
                    DateTime DtIncoorporationDtae;
                    if (DateOfIncoorporation.Value != null)
                    {
                        DtIncoorporationDtae = Convert.ToDateTime(DateOfIncoorporation.Value);
                    }
                    else
                    {
                        DtIncoorporationDtae = Convert.ToDateTime("01-01-1900");
                    }
                    //rev srijeeta  mantis issue 0024515
                    //string output = oHrRecritmentGeneralBL.Insert_ContactGeneralForCopyVendor(DtIncoorporationDtae,OLDID, "Vendor", "", Convert.ToString(CmbSalutation.SelectedItem.Value), txtFirstName.Text.Trim(),
                    //"", "", uniqueid, branch, "", "", dtDob, dtanniversary, Convert.ToString(cmbLegalStatus.SelectedItem.Value), "", "", "", "", "", "", "", dtReg, "", "",
                    //Convert.ToString(cmbSource.SelectedItem.Value), valueforReffer, Convert.ToString(Session["requesttype"]), Convert.ToString(cmbContactStatus.SelectedItem.Value),
                    //Convert.ToString(HttpContext.Current.Session["userid"]), "", "", "", GSTIN, Convert.ToString(txtNameInCheque.Text), rdl_VendorType.SelectedValue, NumberingId);
                    string output = oHrRecritmentGeneralBL.Insert_ContactGeneralForCopyVendor(DtIncoorporationDtae, OLDID, "Vendor", "",  Alternative_Code,Convert.ToString(CmbSalutation.SelectedItem.Value), txtFirstName.Text.Trim(),
                   "", "", uniqueid, branch, "", "", dtDob, dtanniversary, Convert.ToString(cmbLegalStatus.SelectedItem.Value), "", "", "", "", "", "", "", dtReg, "", "",
                   Convert.ToString(cmbSource.SelectedItem.Value), valueforReffer, Convert.ToString(Session["requesttype"]), Convert.ToString(cmbContactStatus.SelectedItem.Value),
                   Convert.ToString(HttpContext.Current.Session["userid"]), "", "", "", GSTIN, Convert.ToString(txtNameInCheque.Text), rdl_VendorType.SelectedValue, NumberingId);
                    //end of rev srijeeta  mantis issue 0024515

                    string InternalID = string.Empty;
                    string LastId = string.Empty;
                    InternalID =output.Split('~')[0];
                    LastId = output.Split('~')[1];
                    
                    #region Rajdip

                 
                    Employee_BL ebl = new Employee_BL();
                    string User_Id = Convert.ToString(Session["userid"]);                  
                    //IsSaved = ebl.AddCustVendHistory(Convert.ToString(GSTIN), Convert.ToInt32(HttpContext.Current.Session["KeyVal"]),
                    //Convert.ToDateTime(dt_ApplicableFrom.Value), User_Id, "GSTIN_Vendor");
                    DataTable ds = new DataTable();
                    int NewId = 0;
                    if (LastId != "" && LastId != null)
                    { 
                    NewId = Convert.ToInt32(LastId);
                    }
                    //DataTable dtnewid = GetNewId("GetNewid", txt_CustDocNo.Text.ToString());
                    //if (dtnewid.Rows.Count > 0)
                    //{
                    //    if (dtnewid.Rows[0]["cnt_id"].ToString() != "" && dtnewid.Rows[0]["cnt_id"].ToString() != null)
                    //    {
                    //        NewId =Convert.ToInt32(dtnewid.Rows[0]["cnt_id"].ToString());
                    //    }
                    //}

                    if (GSTIN != "" && GSTIN != null && dt_ApplicableFrom.Value != null && dt_ApplicableFrom.Value!="")
                    { 
                    ProcedureExecute procGst = new ProcedureExecute("prc_GstApplicatbleForm");
                    procGst.AddNVarcharPara("@NewGSTIN_ID", 150, Convert.ToString(GSTIN));
                    //procGst.AddNVarcharPara("@Cnt_Id", 150, Convert.ToString(txtCode.Text.ToString()));
                    procGst.AddIntegerPara("@newId", NewId);
                    procGst.AddNVarcharPara("@Cnt_Id", 150, Convert.ToString(txt_CustDocNo.Text.ToString()));
                    procGst.AddPara("@ApplicableFrom", Convert.ToDateTime(dt_ApplicableFrom.Value));
                    procGst.AddNVarcharPara("@CreateBy", 150, User_Id);
                    procGst.AddNVarcharPara("@Action", 150, "GSTIN_Vendor");
                    procGst.RunActionQuery();
                    }
                    SqlDataAdapter daUccName = new SqlDataAdapter("select Top 1 * from tbl_master_contact Order by cnt_id desc", con);
                    DataTable dtUccName = new DataTable();
                    daUccName.Fill(dtUccName);
                    string uccname = string.Empty;
                    if (dtUccName.Rows.Count > 0)
                    {
                        uccname = dtUccName.Rows[0]["cnt_UCC"].ToString();
                    }
                    if (txtNumber.Text.ToString() != "" && hdnflag.Value.ToString() != "1")
                    {
                        ProcedureExecute pro = new ProcedureExecute("Prc_InsertPandetails");
                        pro.AddVarcharPara("@Action", 100, "INSERTPAN");
                        pro.AddVarcharPara("@crg_cntId", 50, uccname);
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


                    #endregion Rajdip
                    //chinmoy added below code 02-04-2020 start	
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
                                    if (hdnAutoNumStg.Value == "LDAutoNum1")
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
                    //end

                    //Add Amin Account and sub Account
                    Int32 rowsEffected = oDBEngine.SetFieldValue("tbl_master_contact", "cnt_mainAccount='" + hndTaxRates_MainAccount_hidden.Value + "',cnt_subAccount='" + hndTaxRates_SubAccount_hidden.Value + "',AccountGroupID='" + ddlAssetLiability.SelectedItem.Value + "'", " cnt_internalId='" + InternalID + "'");
                    //Udf Add mode
                    DataTable udfTable = (DataTable)Session["UdfDataOnAdd"];
                    if (udfTable != null)
                        Session["UdfDataOnAdd"] = reCat.insertRemarksCategoryAddMode("DV", Convert.ToString(InternalID), udfTable, Convert.ToString(Session["userid"]));

                    string BranchList = GetBranchList();

                    int brmap = reCat.insertVendorBranchMap(BranchList, InternalID, Convert.ToInt16(branch));
                    //*****************************************************Vendor BILLING SHIPPING**********************************************************************
                    if (chkcopy.Checked == true)
                    {
                        ProcedureExecute proc = new ProcedureExecute("prc_GetSetVendorCopy_Address");
                        proc.AddVarcharPara("@action", 500, "SaveBillingShipping");
                        proc.AddVarcharPara("@OldId", 10, OLDID);
                        proc.AddIntegerPara("@newId", NewId);
                        proc.AddVarcharPara("@cntUcc", 50, uniqueid);
                        proc.AddVarcharPara("@GSTIN", 20, GSTIN);
                        proc.AddVarcharPara("@vcnt_shortName", 20, uniqueid);
                        proc.AddVarcharPara("@UserID", 20, HttpContext.Current.Session["userid"].ToString());
                        proc.RunActionQuery();

                        //Rev Rajdip
                        //------------------------------------------------------********************************************-----------------------------------------
                        DataTable IsBankdetailsexist = oDBEngine.GetDataTable("select * from tbl_trans_contactBankDetails where cbd_cntId IN(select cnt_internalId from tbl_master_contact where cnt_id='" + OLDID + "')");
                        if (IsBankdetailsexist.Rows.Count > 0 && chkcopy.Checked == true)
                        {
                            ProcedureExecute procBankdetailsinsertofcopyproduct = new ProcedureExecute("prc_BankdetailsinsertofCOPYTOVENDOR");
                            procBankdetailsinsertofcopyproduct.AddVarcharPara("@cnt_id", 100, OLDID);
                            //procBankdetailsinsertofcopyproduct.AddVarcharPara("@UNIQUEID", 100, UniqueID);
                            procBankdetailsinsertofcopyproduct.RunActionQuery();
                        }

                    }
                    //----------- Tier Structure End--------------
                    string[,] cnt_id = oDBEngine.GetFieldValue(" tbl_master_contact", " cnt_id", " cnt_internalId='" + InternalID + "'", 1);
                    if (Convert.ToString(cnt_id[0, 0]) != "n")
                    {
                        // Response.Redirect("HRrecruitmentagent_general.aspx?id=" + Convert.ToString(cnt_id[0, 0]), false)
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "jAlert('Vendor Added Successfully.!');", true);
                        Page.ClientScript.RegisterStartupScript(GetType(), "JScript3", "<script >getbacktolisting();</script>");
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "jAlert('Code Already Exists !');", true);
                    Page.ClientScript.RegisterStartupScript(GetType(), "JScript3", "<script >hideotherstatus();</script>");
                }
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "jAlert('Please try again!');", true);
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript3", "<script >hideotherstatus();</script>");
            }
            // }

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
        public DataTable GetNewId(string Action, string UCC)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GstApplicatbleForm");
            proc.AddVarcharPara("@Action", 100, Action);
            proc.AddVarcharPara("@UCC", 100, UCC);
            ds = proc.GetTable();
            return ds;
        }
        //End Rev Rajdip
        protected void EmployeeGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            EmployeeGrid.ClearSort();
            EmployeeGrid.DataBind();
            if (e.Parameters == "s")
                EmployeeGrid.Settings.ShowFilterRow = true;


            //-----------------------------------
            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            string WhichType = null;
            int deletecnt = 0;
            if (Convert.ToString(e.Parameters).Contains("~"))
            {
                if (Convert.ToString(e.Parameters).Split('~')[1] != "")
                {
                    WhichType = Convert.ToString(e.Parameters).Split('~')[1];

                }
                if (WhichCall == "Delete")
                {
                    MasterDataCheckingBL objMasterDataCheckingBL = new MasterDataCheckingBL();

                    deletecnt = objMasterDataCheckingBL.DeleteLeadOrContact(WhichType);
                    if (deletecnt > 0)
                    {

                        EmployeeGrid.JSProperties["cpDelete"] = "Success";
                        //EmployeeDataSource.SelectCommand = "select tbl_master_contact.cnt_id AS cnt_Id,tbl_master_contact.cnt_internalId AS Id,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name, (select top 1 case when phf_phoneNumber is null then '' else '(O)'+ phf_phoneNumber end from tbl_master_phonefax where phf_cntId=tbl_master_contact.cnt_internalId ) AS phone  from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id where  cnt_contacttype='DV'  order by cnt_id desc";

                        //EmployeeDataSource.SelectCommand = "select tbl_master_contact.cnt_id AS cnt_Id,tbl_master_contact.cnt_internalId AS Id,tbl_master_branch.branch_description AS BranchName,ISNULL(cnt_firstName, '') + ' ' + ISNULL(cnt_middleName, '') + ' ' + ISNULL(cnt_lastName, '') AS Name," +                           
                        // " (SELECT DISTINCT STUFF( (select case when (ISNULL(phf_phoneNumber,'')!='' And phf_type='Residence') then ' (R)'+ phf_phoneNumber + ' '  " +
                        // " when (ISNULL(phf_phoneNumber,'')!='' And phf_type='Office') then ' (O)'+ phf_phoneNumber + ' ' " +
                        //   " when (ISNULL(phf_phoneNumber,'')!='' And phf_type='Correspondence') then ' (C)'+ phf_phoneNumber + ' ' " +
                        //   " when (ISNULL(phf_phoneNumber,'')!='' And phf_type='Mobile') then ' (M)'+ phf_phoneNumber + ' '" +
                        //   " when (ISNULL(phf_phoneNumber,'')!='' And phf_type='Fax') then ' (F)'+ phf_phoneNumber + ' ' else '' end " +
                        //   " from tbl_master_phonefax where phf_cntId=tbl_master_contact.cnt_internalId FOR XML PATH('')),1,1,'') as Numbers FROM tbl_master_phonefax) as phone," +
                        //  "cnt_shortName as Unique_ID,(Select cntstu_contactStatus from tbl_master_contactstatus where cntstu_id=cnt_contactStatus) as Status,cnt_PrintNameToCheque,CNT_GSTIN gstin  from tbl_master_contact INNER JOIN tbl_master_branch ON tbl_master_contact.cnt_branchid = tbl_master_branch.branch_id where  cnt_contacttype='DV'  order by cnt_id desc";


                        //EmployeeGrid.DataBind();
                    }
                    else
                        EmployeeGrid.JSProperties["cpDelete"] = "Fail";
                }
            }

            //-----------------------------------

            if (e.Parameters == "All")
            {
                EmployeeGrid.FilterExpression = string.Empty;
            }
        }

        public string Alternative_Code { get; set; }
    }
}