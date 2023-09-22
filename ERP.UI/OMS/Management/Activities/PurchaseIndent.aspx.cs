/**************************************************************************************************************************
* Rev 1.0       Priti      V2.0.39      11-07-2023     0026549:A setting is required to enter the backdated entries in Purchase Indent
* Rev 2.0       Sanchita    V2.0.39     22-09-2023     GST is showing Zero in the TAX Window whereas GST in the Grid calculated. 
 *                                                     Session["MultiUOMData"] has been renamed to Session["MultiUOMDataIND"]
 *                                                     Mantis: 26843
* ************************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using DevExpress.Web;
using System.Collections;
using System.Web.Services;
using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web.Data;
using ERP.Models;
using System.Text.RegularExpressions;
using System.Globalization;
using ERP.OMS.Management.Activities.Services;
using System.Net;
using System.Threading.Tasks;
namespace ERP.OMS.Management.Activities
{
    public partial class PurchaseIndent : ERP.OMS.ViewState_class.VSPage// System.Web.UI.Page
    {
        PurchaseIndentBL objPurchaseIndentBL = new PurchaseIndentBL();
        BusinessLogicLayer.RemarkCategoryBL reCat = new BusinessLogicLayer.RemarkCategoryBL();
        //CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();
        //CRMSalesDtlBL objPurchaseIndentBL = new CRMSalesDtlBL();

        #region Sandip Section For Approval Section Start
        ERPDocPendingApprovalBL objERPDocPendingApproval = new ERPDocPendingApprovalBL();
        int KeyValue = 0;
        DataTable Remarks = null;
        #endregion Sandip Section For Approval Dtl Section End

        #region LocalVariable
        SqlDataSource Obj_Sds;
        //BusinessLogicLayer.DBEngine oDbEngine;
        string strCon;
        string CurrentSegment;
        DataTable DtCurrentSegment;
        DataTable dtXML = new DataTable();
        BusinessLogicLayer.GenericLogSystem oGenericLogSystem;
        // PurchaseIndentCS objPurchaseIndentBL = new PurchaseIndentCS();
        string CashBankVoucherFile_XMLPATH = null;
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();


        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        public bool isApprove { get; set; }
        static string ForJournalDate = null;
        string JVNumStr = string.Empty;
        public EntityLayer.CommonELS.UserRightsForPage rights = new EntityLayer.CommonELS.UserRightsForPage();
        #endregion

        CommonBL cbl = new CommonBL();

        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            #region Sandip Section For Approval Section to Change Master Page Dyanamically Start
            //Rev Debashis
            //if (Request.QueryString.AllKeys.Contains("status"))
            if (Request.QueryString.AllKeys.Contains("status") || Request.QueryString.AllKeys.Contains("IsTagged"))
            //End of Rev Debashis
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/PopUp.Master";
                //divcross.Visible = false;
            }
            else
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/ERP.Master";
                //divcross.Visible = true;
            }
            #endregion Sandip Section For Approval Dtl Section End

            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);


                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            SqlSchematype.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            dsBranch.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlCurrency.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlCurrencyBind.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            dsHierarchy.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            UomSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            AltUomSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            String finyear = ""; 
            finyear = Convert.ToString(Session["LastFinYear"]).Trim();
            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            DataTable dtFinYear = objSlaesActivitiesBL.GetFinacialYearBasedQouteDate(finyear);
            Session["FinYearStartDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearStartDate"]);
            Session["FinYearEndDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearEndDate"]);

            FormDate.MaxDate = Convert.ToDateTime(Session["FinYearEndDate"]);
            FormDate.MinDate = Convert.ToDateTime(Session["FinYearStartDate"]);
            toDate.MaxDate = Convert.ToDateTime(Session["FinYearEndDate"]);
            toDate.MinDate = Convert.ToDateTime(Session["FinYearStartDate"]);

            //Rev Tanmoy

            string ApprovalPurchaseIndentSettings = cbl.GetSystemSettingsResult("ApprovalPurchaseIndentSettings");
            //Mantis Issue 25053
            string SMSRequiredInDirectorApproval = cbl.GetSystemSettingsResult("SMSRequiredInDirectorApproval");
            hdnSettings.Value = SMSRequiredInDirectorApproval;
            //End of Mantis Issue 25053
            
            if (!String.IsNullOrEmpty(ApprovalPurchaseIndentSettings))
            {
                if (ApprovalPurchaseIndentSettings == "Yes")
                {
                    hdnApprovalReqInq.Value = "1";
                    isApprove = true;
                    // Mantis Issue 25235
                    //Grid_PurchaseIndent.Columns[9].Visible = true;
                    //Grid_PurchaseIndent.Columns[10].Visible = true;
                    //Grid_PurchaseIndent.Columns[11].Visible = true;
                    Grid_PurchaseIndent.Columns[10].Visible = true;
                    Grid_PurchaseIndent.Columns[11].Visible = true;
                    Grid_PurchaseIndent.Columns[12].Visible = true;
                    // End of Mantis Issue 25235
                    
                }
                else if (ApprovalPurchaseIndentSettings.ToUpper().Trim() == "NO")
                {
                    hdnApprovalReqInq.Value = "0";
                    dvAppRejRemarks.Style.Add("display", "none");
                    dvRevision.Style.Add("display", "none");
                    dvRevisionDate.Style.Add("display", "none");
                    dvApprove.Style.Add("display", "none");
                    dvReject.Style.Add("display", "none");
                    isApprove = false;
                    // Mantis Issue 25235
                    //Grid_PurchaseIndent.Columns[9].Visible = false;
                    //Grid_PurchaseIndent.Columns[10].Visible = false;
                    //Grid_PurchaseIndent.Columns[11].Visible = false;
                    Grid_PurchaseIndent.Columns[10].Visible = false;
                    Grid_PurchaseIndent.Columns[11].Visible = false;
                    Grid_PurchaseIndent.Columns[12].Visible = false;
                    // End of Mantis Issue 25235
                    
                }
            }


            string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {
                    // Mantis Issue 25235
                    //Grid_PurchaseIndent.Columns[8].Visible = true;
                    Grid_PurchaseIndent.Columns[9].Visible = true;
                    // End of Mantis Issue 25235


                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    // Mantis Issue 25235
                    //Grid_PurchaseIndent.Columns[8].Visible = false;
                    Grid_PurchaseIndent.Columns[9].Visible = false;
                    // End of Mantis Issue 25235
                }
            }

            string ProjectMandatoryInEntry = cbl.GetSystemSettingsResult("ProjectMandatoryInEntry");

            if (!String.IsNullOrEmpty(ProjectMandatoryInEntry))
            {
                if (ProjectMandatoryInEntry == "Yes")
                {
                    hdnProjectMandatory.Value = "1";



                }
                else if (ProjectMandatoryInEntry.ToUpper().Trim() == "NO")
                {
                    hdnProjectMandatory.Value = "0";


                }
            }

            string MultiUOMSelection = cbl.GetSystemSettingsResult("MultiUOMSelection");
            if (!String.IsNullOrEmpty(MultiUOMSelection))
            {
                if (MultiUOMSelection.ToUpper().Trim() == "YES")
                {
                    hddnMultiUOMSelection.Value = "1";

                }
                else if (MultiUOMSelection.ToUpper().Trim() == "NO")
                {
                    hddnMultiUOMSelection.Value = "0";
                    gridBatch.Columns[7].Width = 0;

                    // Rev Mantis Issue 24428/24429
                    gridBatch.Columns[8].Width = 0;
                    gridBatch.Columns[9].Width = 0;
                    // End of Rev Mantis Issue 24428/24429
                }
            }
            if (hdnApproveStatus.Value == "")
            {
                hdnApproveStatus.Value = "0";
            }
            //string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {
                    hdnProjectSelectInEntryModule.Value = "1";
                    lookup_Project.ClientVisible = true;
                    lblProject.Visible = true;

                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    hdnProjectSelectInEntryModule.Value = "0";
                    lookup_Project.ClientVisible = false;
                    lblProject.Visible = false;
                }
            }
            //End Rev Tanmoy

            //For Hierarchy Start Tanmoy
            string HierarchySelectInEntryModule = cbl.GetSystemSettingsResult("Show_Hierarchy");
            if (!String.IsNullOrEmpty(HierarchySelectInEntryModule))
            {
                if (HierarchySelectInEntryModule.ToUpper().Trim() == "YES")
                {
                    ddlHierarchy.Visible = true;
                    lblHierarchy.Visible = true;
                    lookup_Project.Columns[3].Visible = true;
                }
                else if (HierarchySelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    ddlHierarchy.Visible = false;
                    lblHierarchy.Visible = false;
                    lookup_Project.Columns[3].Visible = false;
                }
            }

            string MRPTaggingRequiredPurchaseIndent = cbl.GetSystemSettingsResult("MRPTaggingRequiredPurchaseIndent");

            if (!String.IsNullOrEmpty(MRPTaggingRequiredPurchaseIndent))
            {
                if (MRPTaggingRequiredPurchaseIndent == "Yes")
                {
                    hdnShowMRPTaggingRequiredPurchaseIndent.Value = "1";
                    DivMRP.Style.Add("display", "!inline-block");
                    // Mantis Issue 25235
                    //Grid_PurchaseIndent.Columns[13].Visible = true;
                    Grid_PurchaseIndent.Columns[14].Visible = true;
                    // End of Mantis Issue 25235
                    
                }
                else if (MRPTaggingRequiredPurchaseIndent.ToUpper().Trim() == "NO")
                {
                    hdnShowMRPTaggingRequiredPurchaseIndent.Value = "0";
                    DivMRP.Style.Add("display", "none");
                    // Mantis Issue 25235
                    //Grid_PurchaseIndent.Columns[13].Visible = false;
                    Grid_PurchaseIndent.Columns[14].Visible = false;
                    // End of Mantis Issue 25235
                    
                }
            }
            //For Hierarchy End Tanmoy

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/PurchaseIndent.aspx");
            if (!IsPostBack)
            {
                string ForBranchTaggingPurchase = cbl.GetSystemSettingsResult("ForBranchTaggingPurchase");

                if (!String.IsNullOrEmpty(ForBranchTaggingPurchase))
                {
                    if (ForBranchTaggingPurchase == "Yes")
                    {
                        hdnForBranchTaggingPurchase.Value = "1";
                        DivForUnit.Style.Add("display", "!inline-block");
                    }
                    else if (ForBranchTaggingPurchase.ToUpper().Trim() == "NO")
                    {
                        hdnForBranchTaggingPurchase.Value = "0";
                        DivForUnit.Style.Add("display", "none");
                    }
                }

                //Rev 1.0
                string BackDatedEntryPurchaseIndent = cbl.GetSystemSettingsResult("BackDatedEntryPurchaseIndent");
                if (!String.IsNullOrEmpty(BackDatedEntryPurchaseIndent))
                {
                    if (BackDatedEntryPurchaseIndent.ToUpper().Trim() == "YES")
                    {
                        HdnBackDatedEntryPurchaseIndent.Value = "1";
                    }
                    else if (BackDatedEntryPurchaseIndent.ToUpper().Trim() == "NO")
                    {
                        HdnBackDatedEntryPurchaseIndent.Value = "0";
                    }
                }
                //Rev 1.0 End
                // Mantis Issue 25235
                string IsVendorRequiredInPurchaseIndent = cbl.GetSystemSettingsResult("IsVendorRequiredInPurchaseIndent");

                if (!String.IsNullOrEmpty(IsVendorRequiredInPurchaseIndent))
                {
                    if (IsVendorRequiredInPurchaseIndent.ToUpper().Trim() == "YES")
                    {
                        hdnVendorRequiredInPurchaseIndent.Value = "1";
                        DivForVendor.Style.Add("display", "!inline-block");
                        Grid_PurchaseIndent.Columns[8].Visible = true;
                    }
                    else if (IsVendorRequiredInPurchaseIndent.ToUpper().Trim() == "NO")
                    {
                        hdnVendorRequiredInPurchaseIndent.Value = "0";
                        DivForVendor.Style.Add("display", "none");
                        Grid_PurchaseIndent.Columns[8].Visible = false ;
                    }
                }
                // End of Mantis Issue 25235

                //................Cookies..................
                if (this.Page.MasterPageFile != "/OMS/MasterPage/PopUp.Master")
                {
                    Grid_PurchaseIndent.SettingsCookies.CookiesID = "BreeezeErpGridCookiesGrid_PurchaseIndentPagePurchaseIndent";
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "setCookieOnStorage", "<script>addCookiesKeyOnStorage('BreeezeErpGridCookiesGrid_PurchaseIndentPagePurchaseIndent');</script>");
                }
                //...........Cookies End............... 
                string userbranchHierachy = Convert.ToString(Session["userbranchHierarchy"]);
                string FinYear = Convert.ToString(Session["LastFinYear"]);
                PopulateDropDown(userbranchHierachy, FinYear);
                IsUdfpresent.Value = Convert.ToString(getUdfCount());
                BindHierarchy();
                ddlHierarchy.Enabled = false;
                BindBranchFrom();

                SetFinYearCurrentDate();
                Session["PurchaseIndateDetails"] = null;
                Session["Indent_Id"] = null;
                Session["IndentIdPO"] = null;
                Session["InlineRemarks"] = null;
                Session["MultiUOMDataIND"] = null;
                Session["IndentRequiData"] = null;
                Session["SI_ProductDetails"] = null;
                this.Page.ClientScript.RegisterStartupScript(GetType(), "CS", "<script>PageLoad();</script>");
                if (!String.IsNullOrEmpty(Convert.ToString(Session["userbranchID"])))
                {
                    string strdefaultBranch = Convert.ToString(Session["userbranchID"]);
                    ddlBranch.SelectedValue = strdefaultBranch;
                }
                if (!String.IsNullOrEmpty(Convert.ToString(Session["LocalCurrency"])))
                {
                    string LocalCurrency = Convert.ToString(Session["LocalCurrency"]);
                    string basedCurrency = Convert.ToString(LocalCurrency.Split('~')[0]);
                    string CurrencyId = Convert.ToString(basedCurrency[0]);
                    CmbCurrency.Value = CurrencyId;
                }
                #region Sandip Section For Approval Section Start
                #region Session Remove Section Start
                Session.Remove("INPendingApproval");
                Session.Remove("INUserWiseERPDocCreation");
                #endregion Session Remove Section End
                ConditionWiseShowStatusButton();

                if (Request.QueryString.AllKeys.Contains("status"))
                {
                    string indentid = Convert.ToString(Request.QueryString["key"]);
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "StartingSetupForApproval(" + indentid + ")", true);
                    btncross.Visible = false;
                    btnnew.Visible = false;
                    ApprovalCross.Visible = true;
                    ddlBranch.Enabled = false;
                }
                //Rev Debashis
                else if (Request.QueryString.AllKeys.Contains("IsTagged"))
                {
                    string indentid = Convert.ToString(Request.QueryString["key"]);
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "StartingSetupForApproval(" + indentid + ")", true);
                    btncross.Visible = false;
                    btnnew.Visible = false;
                    ApprovalCross.Visible = true;
                    ddlBranch.Enabled = false;
                    Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>zoompurchaseindent(" + Request.QueryString["key"] + ", '" + Request.QueryString["req"] + "');</script>");
                }
                //End of Rev Debashis
                else
                {
                    btncross.Visible = true;
                    btnnew.Visible = true;
                    ApprovalCross.Visible = false;
                    ddlBranch.Enabled = true;
                }
                #endregion Sandip Section For Approval Dtl Section End

                //Subhra 28-02-2019

                MasterSettings objmaster = new MasterSettings();
                hdnConvertionOverideVisible.Value = objmaster.GetSettings("ConvertionOverideVisible");
                hdnShowUOMConversionInEntry.Value = objmaster.GetSettings("ShowUOMConversionInEntry");

                //Subhra 28-02-2019
            }
            //FillGrid();
            Bind_NumberingScheme();
            // Code Commented By Sam TO avoid rebind again and again by server 
            // It reflects always branchid 1 as head office during save
            // Version 1.0.0.1
            //BindBranchFrom();
            // Code End
            #region Sandip Section For Approval Section Start
            if (divPendingWaiting.Visible == true)
            {
                PopulateUserWiseERPDocCreation();
                PopulateApprovalPendingCountByUserLevel();
                PopulateERPDocApprovalPendingListByUserLevel();
                if (Request.QueryString.AllKeys.Contains("status"))
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "StartingSetupForApproval()", true);
                }
                //else
                //{
                //    if (hdn_Mode.Value == "Edit" && hdngridkeyval.Value!="")
                //    {
                //        IsExistsDocumentInERPDocApproveStatus(hdngridkeyval.Value);
                //    }
                //    else
                //    {
                //        btnnew.Visible = true;
                //        btnSaveExit.Visible = true;
                //    }
                //}

            }
            #endregion Sandip Section For Approval Dtl Section End
            if (hdn_Mode.Value == "Entry")
            {
                Keyval_internalId.Value = "Add";
            }
            //Mantis Issue 25053
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

            string DBName = con.Database;
            AddmodeExecuted(DBName);
            //AddModalEmployee(DBName);
            //End of Mantis Issue 25053

        }
        //Mantis Issue 25053
        private void AddmodeExecuted(string DBName)
        {

            PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
            DataTable branchtable = dsFetchAll(DBName);

            dddlApprovalEmployee.DataSource = branchtable;
            dddlApprovalEmployee.ValueField = "cnt_internalId";
            dddlApprovalEmployee.TextField = "DirectorName";
            dddlApprovalEmployee.DataBind();
            dddlApprovalEmployee.SelectedIndex = 0;
        }
        [WebMethod]
        public static object AddModalEmployee(string DBName)
        {
            DataTable branchtable = new DataTable();
            //DataTable ds = new DataTable();
            //string oSql = hdDbName.value;
            string oSql = Convert.ToString(DBName);
            SqlConnection oSqlConnection = new SqlConnection(oSql);
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("prc_PurchaseIndentDetailsList", oSqlConnection);
            cmd.Parameters.AddWithValue("@ACTION", "FetchEmployee");
            cmd.CommandType = CommandType.StoredProcedure;
            adapter.SelectCommand = cmd;
            adapter.Fill(branchtable);
            oSqlConnection.Close();

            List<ddlDirEmployee> All = new List<ddlDirEmployee>();
            All = (from DataRow dr in branchtable.Rows
                   select new ddlDirEmployee()
                   {
                       cnt_internalId = dr["cnt_internalId"].ToString(),
                       DirectorName = dr["DirectorName"].ToString()
                   }).ToList();

            return All;
        }
        public class ddlDirEmployee
        {
            public string cnt_internalId { get; set; }
            public string DirectorName { get; set; }
        }
        public DataTable dsFetchAll(string DBName)
        {
            DataTable ds = new DataTable();
            string oSql = Convert.ToString(GetConnectionString(DBName));
            SqlConnection oSqlConnection = new SqlConnection(oSql);
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("prc_PurchaseIndentDetailsList", oSqlConnection);
            cmd.Parameters.AddWithValue("@ACTION", "FetchEmployee");
            cmd.CommandType = CommandType.StoredProcedure;
            adapter.SelectCommand = cmd;
            adapter.Fill(ds);
            oSqlConnection.Close();
            return ds;
        }
        public string GetConnectionString(string dbName)
        {
            string Conn = "";
            string DtSource = ConfigurationSettings.AppSettings["sqlDatasource"];
            string UserId = ConfigurationSettings.AppSettings["sqlUserId"];
            string Pwd = ConfigurationSettings.AppSettings["sqlPassword"];
            string IntSq = ConfigurationSettings.AppSettings["sqlAuth"];
            string ispool = ConfigurationSettings.AppSettings["isPool"];
            string poolsize = ConfigurationSettings.AppSettings["PoolSize"];


            SqlConnectionStringBuilder connectionString = new SqlConnectionStringBuilder();
            connectionString.DataSource = DtSource;
            connectionString.InitialCatalog = dbName;
            if (IntSq == "Windows")
            {
                connectionString.IntegratedSecurity = true;
            }
            else
            {
                connectionString.PersistSecurityInfo = true;
                connectionString.IntegratedSecurity = false;
                connectionString.UserID = UserId;
                connectionString.Password = Pwd;
            }
            connectionString.ConnectTimeout = 950;
            connectionString.Pooling = Convert.ToBoolean(ispool);
            connectionString.MaxPoolSize = Convert.ToInt32(poolsize);
            string str = connectionString.ConnectionString;
            hdDbName.Value = str;
            return str;
        }
        //End of Mantis Issue 25053
        public void SetFinYearCurrentDate()
        {
            tDate.EditFormatString = objConverter.GetDateFormat("Date");
            string fDate = null;

            //DateTime dt = DateTime.ParseExact("3/31/2016", "MM/dd/yyy", CultureInfo.InvariantCulture);
            string[] FinYEnd = Convert.ToString(Session["FinYearEnd"]).Split(' ');
            string FinYearEnd = FinYEnd[0];

            DateTime date3 = DateTime.ParseExact(FinYearEnd, "M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            ForJournalDate = Convert.ToString(date3);

            //ForJournalDate =Session["FinYearEnd"].ToString();
            int month = oDBEngine.GetDate().Month;
            int date = oDBEngine.GetDate().Day;
            int Year = oDBEngine.GetDate().Year;

            if (date3 < oDBEngine.GetDate().Date)
            {
                fDate = Convert.ToString(Convert.ToDateTime(ForJournalDate).Month) + "/" + Convert.ToString(Convert.ToDateTime(ForJournalDate).Day) + "/" + Convert.ToString(Convert.ToDateTime(ForJournalDate).Year);
            }
            else
            {
                fDate = Convert.ToString(Convert.ToDateTime(oDBEngine.GetDate().Date).Month) + "/" + Convert.ToString(Convert.ToDateTime(oDBEngine.GetDate().Date).Day) + "/" + Convert.ToString(Convert.ToDateTime(oDBEngine.GetDate().Date).Year);
            }

            tDate.Value = DateTime.ParseExact(fDate, @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        }
        protected int getUdfCount()
        {
            DataTable udfCount = oDBEngine.GetDataTable("select 1 from tbl_master_remarksCategory rc where cat_applicablefor='PI' and ( exists (select * from tbl_master_udfGroup where id=rc.cat_group_id and grp_isVisible=1) or rc.cat_group_id=0)");
            return udfCount.Rows.Count;
        }
        public void Bind_NumberingScheme()
        {
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string strBranchID = Convert.ToString(Session["userbranchID"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            DataTable Schemadt = objSlaesActivitiesBL.GetNumberingSchema(strCompanyID, userbranchHierarchy, FinYear, "15", "N");
            if (Schemadt != null && Schemadt.Rows.Count > 0)
            {
                CmbScheme.TextField = "SchemaName";
                CmbScheme.ValueField = "Id";
                CmbScheme.DataSource = Schemadt;
                CmbScheme.DataBind();
            }
        }
        //Mantis Issue 24912
        [WebMethod]
        public static object GetNumberingSchemeByType(string id)
        {
            //string Type = "Copy";
            string BranchID = "";
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable dtBranch = oDBEngine.GetDataTable("select Indent_BranchIdFor from tbl_trans_Indent where Indent_Id='" + id + "'");
            if (dtBranch.Rows.Count > 0)
            {
                // HttpContext.Current.Session["BranchID"] = dtBranch.Rows[0]["CashBank_BranchID"].ToString();
                BranchID = dtBranch.Rows[0]["Indent_BranchIdFor"].ToString();
            }
            string strCompanyID = HttpContext.Current.Session["LastCompany"].ToString();
            string strBranchID = HttpContext.Current.Session["userbranchID"].ToString();
            string FinYear = HttpContext.Current.Session["LastFinYear"].ToString();
            string userbranchHierarchy = BranchID;
            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            DataTable Schemadt = objSlaesActivitiesBL.GetNumberingSchema(strCompanyID, userbranchHierarchy, FinYear, "15", "N");
            //if (Schemadt != null && Schemadt.Rows.Count > 0)
            //{
            //    CmbScheme.TextField = "SchemaName";
            //    CmbScheme.ValueField = "Id";
            //    CmbScheme.DataSource = Schemadt;
            //    CmbScheme.DataBind();
            //}

            //return "Bind";

            List<ddlNumberingSchema> All = new List<ddlNumberingSchema>();
            All = (from DataRow dr in Schemadt.Rows
                                   select new ddlNumberingSchema()
                                   {
                                       Id = dr["Id"].ToString(),
                                       Name = dr["SchemaName"].ToString()
                                   }).ToList();

            return All;
        }

        [WebMethod]
        public static object GetProductDetails(string id)
        {
            //string Type = "Copy";  

            //DataTable Schemadt = (DataTable)HttpContext.Current.Session["PurchaseIndateDetails"];
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseIndentDetailsList");
            // Rev Mantis Issue 24428/24429
            //proc.AddVarcharPara("@Action", 500, "BindBratchGridPurChaseIndent");
            proc.AddVarcharPara("@Action", 500, "BindBratchGridPurChaseIndent_New");
            // End of Rev Mantis Issue 24428/24429
            //proc.AddIntegerPara("@Indent_Id", Convert.ToInt32(ViewState["Indent_Id"]));
            //proc.AddIntegerPara("@Indent_Id", Convert.ToInt32(HttpContext.Current.Session["Indent_Id"].ToString()));
            proc.AddIntegerPara("@Indent_Id", Convert.ToInt32(id));
            ds = proc.GetDataSet();
            DataTable Schemadt = ds.Tables[0];

            List<ddlNumberingSchema> All = new List<ddlNumberingSchema>();
            All = (from DataRow dr in Schemadt.Rows
                   select new ddlNumberingSchema()
                   {
                       Id = dr["ProductID"].ToString(),
                       Name = dr["Description"].ToString()
                   }).ToList();

            return All;
        }
        public class ddlNumberingSchema
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }
        //End of Mantis Issue 24912
        public void BindBranchFrom()
        {
            //dsBranch.SelectCommand = "SELECT BRANCH_id AS BANKBRANCH_ID , RTRIM(BRANCH_DESCRIPTION)+' ['+ISNULL(RTRIM(BRANCH_CODE),'')+']' AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH where BRANCH_id in(" + Convert.ToString(Session["userbranchHierarchy"]) + ")";
            dsBranch.SelectCommand = "SELECT BRANCH_id AS BANKBRANCH_ID , RTRIM(BRANCH_DESCRIPTION)+' ['+ISNULL(RTRIM(BRANCH_CODE),'')+']' AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH ";
            ddlBranch.DataBind();
            ddlForBranch.DataBind();

        }
        public void bindexport(int Filter)
        {
            //Code  Added and Commented By Priti on 20122016 to use Export Header,date
            //exporter.GridView = Grid_ContraVoucher;

            exporter.GridViewID = "Grid_PurchaseIndent";
            string filename = "PurchaseIndentRequisition";
            exporter.FileName = filename;

            exporter.PageHeader.Left = "Purchase Indent Requisition";
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
        protected void drdExport_SelectedIndexChanged(object sender, EventArgs e)
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

        protected void callback_InlineRemarks_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            string SrlNo = e.Parameter.Split('~')[1];
            string RemarksVal = e.Parameter.Split('~')[2];
            Remarks = new DataTable();

            callback_InlineRemarks.JSProperties["cpDisplayFocus"] = "";
            if (strSplitCommand == "RemarksFinal")
            {
                if (Session["InlineRemarks"] != null)
                {
                    Remarks = (DataTable)Session["InlineRemarks"];
                }
                else
                {
                    if (Remarks == null || Remarks.Rows.Count == 0)
                    {
                        Remarks.Columns.Add("SrlNo", typeof(string));
                        Remarks.Columns.Add("ProjectAdditionRemarks", typeof(string));
                    }
                }
                DataRow[] deletedRow = Remarks.Select("SrlNo=" + SrlNo);
                if (deletedRow.Length > 0)
                {
                    foreach (DataRow dr in deletedRow)
                    {
                        Remarks.Rows.Remove(dr);
                    }
                    Remarks.AcceptChanges();
                }
                Remarks.Rows.Add(SrlNo, RemarksVal);
                Session["InlineRemarks"] = Remarks;
            }
            else if (strSplitCommand == "DisplayRemarks")
            {
                DataTable Remarksdt = (DataTable)Session["InlineRemarks"];
                if (Remarksdt != null)
                {
                    DataView dvData = new DataView(Remarksdt);
                    dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                    if (dvData.Count > 0)
                        txtInlineRemarks.Text = dvData[0]["ProjectAdditionRemarks"].ToString();
                    else
                        txtInlineRemarks.Text = "";
                }
                callback_InlineRemarks.JSProperties["cpDisplayFocus"] = "DisplayRemarksFocus";
            }
        }
        //**Abhisek**//
        public void PopulateDropDown(string branchHierchy, string FinYear)
        {
            DataSet dst = GetAllDropDownDetail(branchHierchy, FinYear);

            FormDate.Date = DateTime.Now;
            toDate.Date = DateTime.Now;
            //dtBilldate.Date = DateTime.Now;

            if (dst.Tables[0] != null && dst.Tables[0].Rows.Count > 0)
            {
                cmbBranchfilter.DataSource = dst.Tables[0];
                cmbBranchfilter.ValueField = "branch_id";
                cmbBranchfilter.TextField = "branch_description";
                cmbBranchfilter.DataBind();

                cmbBranchfilter.Value = Convert.ToString(Session["userbranchID"]);
            }


        }
        public DataSet GetAllDropDownDetail(string branchList, string FinYear)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Prc_DownPaymentEntry_Details");
            proc.AddVarcharPara("@Action", 100, "GetAllDropDownDetail");
            proc.AddVarcharPara("@BranchList", 4000, branchList);
            proc.AddVarcharPara("@FinYear", 10, FinYear);
            ds = proc.GetDataSet();
            return ds;
        }


        // Mantis Issue 25394
        protected void CallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            string returnPara = Convert.ToString(e.Parameter);
            DateTime dtFrom;
            DateTime dtTo;
            dtFrom = Convert.ToDateTime(FormDate.Date);
            dtTo = Convert.ToDateTime(toDate.Date);
            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");

            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);
            Task PopulateStockTrialDataTask = new Task(() => GetPurchaseIndentdata(FROMDATE, TODATE, strBranchID));
            PopulateStockTrialDataTask.RunSynchronously();
        }
        public void GetPurchaseIndentdata(string FROMDATE, string TODATE, string BRANCH_ID)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_PurchaseIndent_List", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                cmd.Parameters.AddWithValue("@TODATE", TODATE);
                if (BRANCH_ID == "0")
                {
                    cmd.Parameters.AddWithValue("@BRANCHID", Convert.ToString(Session["userbranchHierarchy"]));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@BRANCHID", BRANCH_ID);
                }
                cmd.Parameters.AddWithValue("@USERID", Convert.ToInt32(Session["userid"]));
                //cmd.Parameters.AddWithValue("@ACTION", hFilterType.Value);
                cmd.Parameters.AddWithValue("@ACTION", "ALL");
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);

                cmd.Dispose();
                con.Dispose();
            }
            catch (Exception ex)
            {

            }
        }
        // End of Mantis Issue 25394
        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Indent_Id";

            // string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);



            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);
            // Mantis Issue 25394
            string UserID = Convert.ToString(HttpContext.Current.Session["userid"]);
            // End of Mantis Issue 25394

            List<int> branchidlist;
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            if (IsFilter == "Y")
            {
                // Mantis Issue 25394
                //if (strBranchID == "0")
                //{
                //    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                //    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));

                //    var q = from d in dc.V_PurchaseIndentLists
                //            where d.Indent_RequisitionDateTimeFormat >= Convert.ToDateTime(strFromDate) && d.Indent_RequisitionDateTimeFormat <= Convert.ToDateTime(strToDate) &&
                //            d.Indent_FinYear == Convert.ToString(Session["LastFinYear"]) &&
                //            d.Indent_Company == Convert.ToString(Session["LastCompany"]) &&
                //            branchidlist.Contains(Convert.ToInt32(d.Indent_BranchIdFor))
                //            orderby d.Indent_RequisitionDateTimeFormat descending
                //            select d;
                //    e.QueryableSource = q;
                //}
                //else
                //{
                //    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                //    var q = from d in dc.V_PurchaseIndentLists
                //            where
                //            d.Indent_RequisitionDateTimeFormat >= Convert.ToDateTime(strFromDate) && d.Indent_RequisitionDateTimeFormat <= Convert.ToDateTime(strToDate) &&
                //            branchidlist.Contains(Convert.ToInt32(d.Indent_BranchIdFor)) &&
                //            d.Indent_FinYear == Convert.ToString(Session["LastFinYear"]) &&
                //            d.Indent_Company == Convert.ToString(Session["LastCompany"])
                //            orderby d.Indent_RequisitionDateTimeFormat descending
                //            select d;
                //    e.QueryableSource = q;
                //}

                var q = from d in dc.PurchaseIndentLists
                        where Convert.ToString(d.USERID) == UserID
                        orderby d.SEQ descending
                        select d;
                e.QueryableSource = q;
                // End of Mantis Issue 25394
            }
            else
            {
                // Mantis Issue 25394
                //var q = from d in dc.V_PurchaseIndentLists
                //        where d.Indent_FinYear == Convert.ToString(Session["LastFinYear"]) &&
                //                d.Indent_Company == Convert.ToString(Session["LastCompany"]) &&
                //                d.Indent_BranchIdFor == 0
                //        orderby d.Indent_RequisitionDateTimeFormat descending
                //        select d;
                //e.QueryableSource = q;
                var q = from d in dc.PurchaseIndentLists
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ descending
                        select d;
                e.QueryableSource = q;
                // End of Mantis Issue 25394
            }
        }

        public void FillGrid()
        {
            //DataTable dtdata = GetGridData();
            DataTable dtdata = GetPurchaseIndentListGridData();


            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                Grid_PurchaseIndent.DataSource = dtdata;
                Grid_PurchaseIndent.DataBind();
            }
            else
            {
                Grid_PurchaseIndent.DataSource = null;
                Grid_PurchaseIndent.DataBind();
            }
        }
        public DataTable GetPurchaseIndentListGridData()
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseIndentDetailsList");
            proc.AddVarcharPara("@Action", 500, "PurchaseIndent");
            proc.AddVarcharPara("@CampanyID", 500, Convert.ToString(Session["LastCompany"]));
            proc.AddVarcharPara("@userbranchHierarchy", 500, Convert.ToString(Session["userbranchHierarchy"]));
            // proc.AddVarcharPara("@Companybranch", 500, Convert.ToString(Session["userbranchID"]));
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            dt = proc.GetTable();
            return dt;
        }

        public class Product
        {
            public string ProductID { get; set; }
            public string ProductName { get; set; }
        }
        public DataSet GetProductData()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseIndentDetailsList");
            proc.AddVarcharPara("@Action", 500, "ProductDetailsPurchaseIndent");
            ds = proc.GetDataSet();
            return ds;
        }


        public static string ApproveRejectProject(string ApproveRemarks, int ApproveRejStatus, string OrderId)
        {
            string returnValue = "";
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseIndentDetailsList");
            proc.AddVarcharPara("@Action", 200, "ApproveRejectDetails");
            proc.AddVarcharPara("@Project_ApproveRejectREmarks", 5000, ApproveRemarks);
            proc.AddIntegerPara("@Project_ApproveStatus", ApproveRejStatus);
            proc.AddVarcharPara("@Order_Id", 10, OrderId);
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            proc.RunActionQuery();
            returnValue = Convert.ToString(proc.GetParaValue("@ReturnValue"));
            return returnValue;
        }
        public IEnumerable GetProduct()
        {
            List<Product> ProductList = new List<Product>();
            // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();


            DataTable DT = GetProductData().Tables[0];
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                Product Products = new Product();
                Products.ProductID = Convert.ToString(DT.Rows[i]["Products_ID"]);
                Products.ProductName = Convert.ToString(DT.Rows[i]["Products_Name"]);
                ProductList.Add(Products);
            }

            return ProductList;
        }
        //protected void Page_Init(object sender, EventArgs e)
        //{
        //    ((GridViewDataComboBoxColumn)gridBatch.Columns["gvColProduct"]).PropertiesComboBox.DataSource = GetProduct();
        //}
        [WebMethod]
        public static string getSchemeType(string sel_scheme_id)
        {
            //BusinessLogicLayer.DBEngine oDbEngine1 = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            //string[] scheme = oDbEngine1.GetFieldValue1("tbl_master_Idschema", "schema_type", "Id = " + Convert.ToInt32(sel_scheme_id), 1);
            //return Convert.ToString(scheme[0]);

            string strschematype = "", strschemalength = "", strschemavalue = "", strschemaBranch = "", Valid_From = "", Valid_Upto = "";


            // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();



            DataTable DT = objEngine.GetDataTable("tbl_master_Idschema", " schema_type,length,Branch,Valid_From,Valid_Upto ", " Id = " + Convert.ToInt32(sel_scheme_id));

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                strschematype = Convert.ToString(DT.Rows[i]["schema_type"]);
                strschemalength = Convert.ToString(DT.Rows[i]["length"]);
                strschemaBranch = Convert.ToString(DT.Rows[i]["Branch"]);
                Valid_From = Convert.ToDateTime(DT.Rows[i]["Valid_From"]).ToString("MM-dd-yyyy");
                Valid_Upto = Convert.ToDateTime(DT.Rows[i]["Valid_Upto"]).ToString("MM-dd-yyyy");
                strschemavalue = strschematype + "~" + strschemalength + "~" + strschemaBranch + "~" + Valid_From + "~" + Valid_Upto;
            }
            return Convert.ToString(strschemavalue);
        }
        [WebMethod]
        public static String GetRate(string basedCurrency, string Currency_ID, string Campany_ID)
        {

            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            DataTable dt = objSlaesActivitiesBL.GetCurrentConvertedRate(Convert.ToInt16(basedCurrency), Convert.ToInt16(Currency_ID), Campany_ID);
            string SalesRate = "";
            if (dt.Rows.Count > 0)
            {
                SalesRate = Convert.ToString(dt.Rows[0]["SalesRate"]);
            }

            return SalesRate;
        }


        [WebMethod]
        public static int Duplicaterevnumbercheck(string RevNo, string Order)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            int returnVal = 0;
            CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();
            DataTable dtrev = new DataTable();
            DataTable OrderNumber = new DataTable();
            OrderNumber = oDBEngine.GetDataTable("select ISNULL(Indent_RequisitionNumber,'') Order_Number from tbl_trans_Indent  where Indent_Id='" + Order + "'");
            string orderno;
            if (OrderNumber.Rows.Count > 0)
            {
                orderno = Convert.ToString(OrderNumber.Rows[0]["Order_Number"]);
            }
            else
            {
                orderno = "";
            }
            //dtrev = objCRMSalesOrderDtlBL.DuplicateRevisionNumber(RevNo, Convert.ToString(OrderNumber.Rows[0]["Order_Number"]));	
            //dtrev = objCRMSalesOrderDtlBL.DuplicateRevisionNumber(RevNo, Convert.ToString(OrderNumber.Rows[0]["Order_Number"]));	
            dtrev = oDBEngine.GetDataTable("select  Indent_RevisionNo as Order_RevisionNo from tbl_trans_Indent where  Indent_RequisitionNumber='" + orderno + "'");
            //if (dtrev.Rows.Count > 0)	
            //{	
            //    returnVal = 1;	
            //}	
            if (dtrev.Rows.Count > 0)
            {
                for (int i = 0; i < dtrev.Rows.Count; i++)
                {
                    if (dtrev.Rows[i]["Order_RevisionNo"].ToString() == RevNo)
                    {
                        returnVal = 1;
                    }
                }
            }
            return returnVal;
        }
        [WebMethod]
        public static object GetApproveRejectStatus(string Id)
        {
            int status = 0;
            DataTable dts = new DataTable();
            dts = ApproveRejectProjectStatus(Id);
            status = Convert.ToInt32(dts.Rows[0]["Project_ApproveStatus"]);
            return status;
        }
        [WebMethod]
        public static string SetApproveReject(string ApproveRemarks, int ApproveRejStatus, string OrderId)
        {

            string val = ApproveRejectProject(ApproveRemarks, ApproveRejStatus, OrderId);
            return val;
        }



        [WebMethod]
        public static object GetPackingQuantity(Int32 UomId, Int32 AltUomId, Int64 ProductID)
        {
            List<MultiUOMPacking> RateLists = new List<MultiUOMPacking>();

            decimal packing_quantity = 0;
            decimal sProduct_quantity = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseIndentDetailsList");
            proc.AddVarcharPara("@Action", 500, "PackingQuantityDetails");
            proc.AddIntegerPara("@UomId", UomId);
            proc.AddIntegerPara("@AltUomId", AltUomId);
            proc.AddBigIntegerPara("@PackingProductId", ProductID);
            DataTable dt = proc.GetTable();
            RateLists = DbHelpers.ToModelList<MultiUOMPacking>(dt);
            if (dt != null && dt.Rows.Count > 0)
            {
                packing_quantity = Convert.ToDecimal(dt.Rows[0]["packing_quantity"]);
                sProduct_quantity = Convert.ToDecimal(dt.Rows[0]["sProduct_quantity"]);
            }
            //return packing_quantity + '~' + sProduct_quantity;
            return RateLists;
        }

        [WebMethod]
        public static object AutoPopulateAltQuantity(Int64 ProductID)
        {
            List<MultiUOMPacking> RateLists = new List<MultiUOMPacking>();

            decimal packing_quantity = 0;
            decimal sProduct_quantity = 0;
            Int32 AltUOMId = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseIndentDetailsList");
            proc.AddVarcharPara("@Action", 500, "AutoPopulateAltQuantityDetails");
            proc.AddBigIntegerPara("@PackingProductId", ProductID);
            DataTable dt = proc.GetTable();
            RateLists = DbHelpers.ToModelList<MultiUOMPacking>(dt);
            if (dt != null && dt.Rows.Count > 0)
            {
                packing_quantity = Convert.ToDecimal(dt.Rows[0]["packing_quantity"]);
                sProduct_quantity = Convert.ToDecimal(dt.Rows[0]["sProduct_quantity"]);
                AltUOMId = Convert.ToInt32(dt.Rows[0]["AltUOMId"]);
            }
            //return packing_quantity + '~' + sProduct_quantity;
            return RateLists;
        }

        public class BratchGridLIST
        {
            public string SrlNo { get; set; }
            public string PurchaseIndentID { get; set; }
            public string gvColProduct { get; set; }
            public string gvColDiscription { get; set; }
            public string gvColQuantity { get; set; }
            public string gvColUOM { get; set; }
            public string gvColRate { get; set; }
            public string gvColValue { get; set; }
            public Nullable<DateTime> ExpectedDeliveryDate { get; set; }

            public string Status { get; set; }
            public string AvailableStock { get; set; }
            public string ProductName { get; set; }
            //Subhra 01-03-2019
            public string gvColIndentDetailsId { get; set; }
            public string Remarks { get; set; }

            public string ServiceTempDetails_ID { get; set; }
            public string ServiceTemplate_ID { get; set; }

            //  Manis 24428
            public string Order_AltQuantity { get; set; }
            public string Order_AltUOM { get; set; }
            // End  Manis 24428
            public string BalanceQty { get; set; }
        }

        public class MultiUOMPacking
        {
            public decimal packing_quantity { get; set; }
            public decimal sProduct_quantity { get; set; }

            public Int32 AltUOMId { get; set; }
        }

        protected void Grid_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void Grid_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void Grid_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            e.Cancel = true;
        }


        protected void gridBatch_DataBinding(object sender, EventArgs e)
        {
            if (Session["PurchaseIndateDetails"] != null)
            {
                DataTable dvData = (DataTable)Session["PurchaseIndateDetails"];
                DataView dvDataView = new DataView(dvData);
                dvDataView.RowFilter = "Status <> 'D'";

                gridBatch.DataSource = GetPurchaseIndent(dvDataView.ToTable());
            }
            else
            {
                gridBatch.DataSource = BindBratchGrid();
            }
        }
        protected void gridBatch_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "SrlNo")
            {
                e.Editor.ReadOnly = true;
            }
            else
                if (e.Column.FieldName == "gvColDiscription")
                {
                    e.Editor.ReadOnly = true;
                }
                else if (e.Column.FieldName == "gvColUOM")
                {
                    e.Editor.ReadOnly = true;
                }
                //else if (e.Column.FieldName == "gvColRate")
                //{
                //    e.Editor.ReadOnly = true;
                //}
                else if (e.Column.FieldName == "gvColAvailableStock")
                {
                    e.Editor.ReadOnly = true;
                }
                else if (e.Column.FieldName == "gvColValue")
                {
                    e.Editor.ReadOnly = true;
                }
                else if (e.Column.FieldName == "ProductName")
                {
                    e.Editor.ReadOnly = true;
                }
                //Manis 24428 
                else if (e.Column.FieldName == "Order_AltQuantity")
                {
                    e.Editor.Enabled = true;
                }
                else if (e.Column.FieldName == "Order_AltUOM")
                {
                    e.Editor.Enabled = true;
                }
                else if (e.Column.FieldName == "gvColQuantity" && hddnMultiUOMSelection.Value == "1")
                {
                    e.Editor.Enabled = true;
                }
                //End Mantis 24428
                else
                {
                    e.Editor.ReadOnly = false;
                }

        }

        protected void gridBatch_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            DataTable Indentdt = new DataTable();
            string IsDeleteFrom = Convert.ToString(hdfIsDelete.Value);

            if (Session["PurchaseIndateDetails"] != null)
            {
                Indentdt = (DataTable)Session["PurchaseIndateDetails"];
            }
            else
            {
                Indentdt.Columns.Add("SrlNo", typeof(string));//1
                Indentdt.Columns.Add("IndentDetailsId", typeof(string));//2
                Indentdt.Columns.Add("ProductID", typeof(string));//3
                Indentdt.Columns.Add("Description", typeof(string));//4
                Indentdt.Columns.Add("Quantity", typeof(string));//5
                Indentdt.Columns.Add("UOM", typeof(string));//6
                Indentdt.Columns.Add("Rate", typeof(string));//7
                Indentdt.Columns.Add("ValueInBaseCurrency", typeof(string));//8
                Indentdt.Columns.Add("ExpectedDeliveryDate", typeof(string));//9

                Indentdt.Columns.Add("Status", typeof(string));//10
                Indentdt.Columns.Add("AvailableStock", typeof(string));//11
                Indentdt.Columns.Add("ProductName", typeof(string));//12
                Indentdt.Columns.Add("Remarks", typeof(string));//13

                Indentdt.Columns.Add("ServiceTempDetails_ID", typeof(string));
                Indentdt.Columns.Add("ServiceTemplate_ID", typeof(string));

                // Rev  Manis 24428
                Indentdt.Columns.Add("Order_AltQuantity", typeof(string));
                Indentdt.Columns.Add("Order_AltUOM", typeof(string));
                // End  Manis 24428
            }

            foreach (var args in e.InsertValues)
            {
                string ProductDetails = Convert.ToString(args.NewValues["gvColProduct"]);

                if (ProductDetails != "" && ProductDetails != "0")
                {
                    string SrlNo = Convert.ToString(args.NewValues["SrlNo"]);

                    string[] ProductDetailsList = ProductDetails.Split(new string[] { "||@||" }, StringSplitOptions.None);
                    string ProductID = ProductDetailsList[0];

                    string ProductName = Convert.ToString(args.NewValues["ProductName"]);
                    string Description = Convert.ToString(args.NewValues["gvColDiscription"]);
                    string Quantity = (Convert.ToString(args.NewValues["gvColQuantity"]) != "") ? Convert.ToString(args.NewValues["gvColQuantity"]) : "0";
                    string UOM = Convert.ToString(args.NewValues["gvColUOM"]);

                    // Rev  Manis 24428
                    string Order_AltQuantity = Convert.ToString(args.NewValues["Order_AltQuantity"]);
                    string Order_AltUOM = Convert.ToString(args.NewValues["Order_AltUOM"]);
                    // End of  Manis 24428
                    //string UOM = Convert.ToString(ProductDetailsList[3]);
                    string Rate = Convert.ToString(args.NewValues["gvColRate"]);
                    // string StockQuantity = Convert.ToString(args.NewValues["gvColValue"]);
                    //string Rate = Convert.ToString(ProductDetailsList[7]);
                    string Amount = (Convert.ToString(args.NewValues["gvColValue"]) != "") ? Convert.ToString(args.NewValues["gvColValue"]) : "0";
                    string Date = Convert.ToString(args.NewValues["ExpectedDeliveryDate"]);
                    string Remarks = Convert.ToString(args.NewValues["Remarks"]);


                    string ServiceTempDetails_ID = (Convert.ToString(args.NewValues["ServiceTempDetails_ID"]) != "") ? Convert.ToString(args.NewValues["ServiceTempDetails_ID"]) : "0";
                    string ServiceTemplate_ID = (Convert.ToString(args.NewValues["ServiceTemplate_ID"]) != "") ? Convert.ToString(args.NewValues["ServiceTemplate_ID"]) : "0";
                    ///string ServiceTempDetails_ID = Convert.ToString(args.NewValues["ServiceTempDetails_ID"]);
                    //string ServiceTemplate_ID = Convert.ToString(args.NewValues["ServiceTemplate_ID"]);
                    // Rev  Manis 24428
                    //Indentdt.Rows.Add(SrlNo, "0", ProductDetails, Description, Quantity, UOM, Rate, Amount, Date, "I", 0, ProductName, Remarks, ServiceTempDetails_ID, ServiceTemplate_ID);
                    Indentdt.Rows.Add(SrlNo, "0", ProductDetails, Description, Quantity, UOM, Rate, Amount, Date, "I", 0, ProductName, Remarks, ServiceTempDetails_ID, ServiceTemplate_ID, Order_AltQuantity, Order_AltUOM);
                    //Indentdt.Rows.Add(SrlNo,"0", ProductID,"",Quantity, UOM, Rate, Amount, Date, "I",0);
                    // End of  Manis 24428
                }
            }
            foreach (var args in e.UpdateValues)
            {
                string SrlNo = Convert.ToString(args.NewValues["SrlNo"]);
                string IndentDetailsId = Convert.ToString(args.Keys["PurchaseIndentID"]);
                string ProductDetails = Convert.ToString(args.NewValues["gvColProduct"]);

                bool isDeleted = false;
                foreach (var arg in e.DeleteValues)
                {
                    string DeleteID = Convert.ToString(arg.Keys["PurchaseIndentID"]);
                    if (DeleteID == IndentDetailsId)
                    {
                        isDeleted = true;
                        break;
                    }
                }

                if (isDeleted == false)
                {
                    if (ProductDetails != "" && ProductDetails != "0")
                    {
                        string[] ProductDetailsList = ProductDetails.Split(new string[] { "||@||" }, StringSplitOptions.None);
                        string ProductID = Convert.ToString(ProductDetailsList[0]);

                        string ProductName = Convert.ToString(args.NewValues["ProductName"]);
                        string Description = Convert.ToString(args.NewValues["gvColDiscription"]);
                        string Quantity = Convert.ToString(args.NewValues["gvColQuantity"]);
                        // string UOM = Convert.ToString(ProductDetailsList[3]);
                        string UOM = Convert.ToString(args.NewValues["gvColUOM"]);

                        // Rev  Manis 24428
                        string Order_AltQuantity = Convert.ToString(args.NewValues["Order_AltQuantity"]);
                        string Order_AltUOM = Convert.ToString(args.NewValues["Order_AltUOM"]);
                        // End of  Manis 24428

                        string Rate = Convert.ToString(args.NewValues["gvColRate"]);
                        // string StockQuantity = Convert.ToString(args.NewValues["gvColValue"]);
                        // string Rate = Convert.ToString(ProductDetailsList[7]);
                        string Amount = (Convert.ToString(args.NewValues["gvColValue"]) != "") ? Convert.ToString(args.NewValues["gvColValue"]) : "0";
                        string Date = Convert.ToString(args.NewValues["ExpectedDeliveryDate"]);
                        string Remarks = Convert.ToString(args.NewValues["Remarks"]);
                        string ServiceTempDetails_ID = Convert.ToString(args.NewValues["ServiceTempDetails_ID"]);
                        string ServiceTemplate_ID = Convert.ToString(args.NewValues["ServiceTemplate_ID"]);

                        bool Isexists = false;
                        foreach (DataRow drr in Indentdt.Rows)
                        {
                            string OldQuotationID = Convert.ToString(drr["IndentDetailsId"]);

                            if (OldQuotationID == IndentDetailsId)
                            {
                                Isexists = true;
                                drr["SrlNo"] = SrlNo;
                                drr["IndentDetailsId"] = OldQuotationID;
                                drr["ProductID"] = ProductDetails;
                                drr["Description"] = Description;
                                drr["Quantity"] = Quantity;
                                drr["UOM"] = UOM;
                                drr["Rate"] = Rate;
                                drr["ValueInBaseCurrency"] = Amount;
                                drr["ExpectedDeliveryDate"] = Date;
                                drr["Status"] = "U";
                                drr["AvailableStock"] = "0";
                                drr["ProductName"] = ProductName;
                                drr["Remarks"] = Remarks;
                                drr["ServiceTempDetails_ID"] = ServiceTempDetails_ID;
                                drr["ServiceTemplate_ID"] = ServiceTemplate_ID;
                                // Rev  Manis 24428
                                drr["Order_AltQuantity"] = Order_AltQuantity;
                                drr["Order_AltUOM"] = Order_AltUOM;
                                // End of  Manis 24428
                                
                                break;
                            }
                        }

                        if (Isexists == false)
                        {

                            // Rev  Manis 24428
                            // Indentdt.Rows.Add(SrlNo, IndentDetailsId, ProductID,"",Quantity, UOM, Rate, Amount, Date, "U",0);
                           // Indentdt.Rows.Add(SrlNo, IndentDetailsId, ProductDetails, "", Quantity, UOM, Rate, Amount, Date, "U", 0, ProductName, Remarks, ServiceTempDetails_ID, ServiceTemplate_ID);
                            Indentdt.Rows.Add(SrlNo, IndentDetailsId, ProductDetails, "", Quantity, UOM, Rate, Amount, Date, "U", 0, ProductName, Remarks, ServiceTempDetails_ID, ServiceTemplate_ID, Order_AltQuantity, Order_AltUOM);

                            // End of  Manis 24428
                        }
                    }
                }
            }
            foreach (var args in e.DeleteValues)
            {
                string IndentDetailsID = Convert.ToString(args.Keys["PurchaseIndentID"]);

                for (int i = Indentdt.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = Indentdt.Rows[i];
                    string delQuotationID = Convert.ToString(dr["IndentDetailsID"]);

                    if (delQuotationID == IndentDetailsID)
                        dr.Delete();
                }
                Indentdt.AcceptChanges();

                if (IndentDetailsID.Contains("~") != true)
                {
                    Indentdt.Rows.Add(0, IndentDetailsID, 0, "", 0, 0, 0, 0, DateTime.Now, "D", 0, "",0,0);
                }
            }
            int j = 1;
            foreach (DataRow dr in Indentdt.Rows)
            {
                string Status = Convert.ToString(dr["Status"]);
                dr["SrlNo"] = j.ToString();

                if (Status != "D")
                {
                    if (Status == "I")
                    {
                        string strID = Convert.ToString("Q~" + j);
                        dr["IndentDetailsID"] = strID;
                    }
                    j++;
                }
            }
            Indentdt.AcceptChanges();

            Session["PurchaseIndateDetails"] = Indentdt;
            if (IsDeleteFrom != "D")
            {
                string ActionType = "";
                if (hdn_Mode.Value == "Entry")
                {
                    ActionType = "ADD";
                }
                else if (hdn_Mode.Value == "Edit")
                {
                    ActionType = "Edit";
                }

                string strIndentType = "PI";
                string strRequisitionNumber = Convert.ToString(txtVoucherNo.Text.Trim());
                string strIndent_Id = Convert.ToString(hdnEditIndentID.Value);
                string strRequisitionDate = Convert.ToString(tDate.Date);
                string strBranch1 = Convert.ToString(ddlBranch.SelectedValue);
                string strBranch = Convert.ToString(ddlBranch.SelectedItem.Value);

                string strForBranch = "0";
                if (hdnForBranchTaggingPurchase.Value == "1")
                {
                    strForBranch = Convert.ToString(ddlForBranch.SelectedItem.Value);
                }

                string strPurpose = Convert.ToString(txtMemoPurpose.Text);
                string strCurrency = Convert.ToString(CmbCurrency.SelectedItem.Value);
                string strExchangeRate = Convert.ToString(txtRate.Text);
                string currency = Convert.ToString(HttpContext.Current.Session["LocalCurrency"]);
                string[] ActCurrency = currency.Split('~');
                int BaseCurrencyId = Convert.ToInt32(ActCurrency[0]);
                // Mantis Issue 25235
                string strVendor = Convert.ToString(hdnCustomerId.Value);
                // End of Mantis Issue 25235

                DataTable tempQuotation = Indentdt.Copy();

                foreach (DataRow dr in tempQuotation.Rows)
                {
                    string Status = Convert.ToString(dr["Status"]);

                    if (Status == "I")
                    {
                        dr["IndentDetailsId"] = "0";

                        string[] list = Convert.ToString(dr["ProductID"]).Split(new string[] { "||@||" }, StringSplitOptions.None);
                        dr["ProductID"] = Convert.ToString(list[0]);
                        dr["UOM"] = Convert.ToString(list[3]);
                        string rate = Convert.ToString(dr["Rate"]);
                        dr["Rate"] = Convert.ToString(Math.Round(Convert.ToDecimal(rate), 2));
                        dr["Description"] = "";
                    }
                    else if (Status == "U" || Status == "")
                    {
                        string[] list = Convert.ToString(dr["ProductID"]).Split(new string[] { "||@||" }, StringSplitOptions.None);
                        dr["ProductID"] = Convert.ToString(list[0]);
                        dr["UOM"] = Convert.ToString(list[3]);
                        string rate = Convert.ToString(dr["Rate"]);
                        dr["Rate"] = Convert.ToString(Math.Round(Convert.ToDecimal(rate), 2));
                        dr["Description"] = "";
                        string IndentDetailsID = Convert.ToString(dr["IndentDetailsId"]);
                        if (IndentDetailsID.Contains("~") == true)
                        {
                            dr["IndentDetailsId"] = "0";
                        }
                    }
                }
                tempQuotation.AcceptChanges();

                #region Sandip Section For Approval Section Start
                string approveStatus = "";
                if (Request.QueryString["status"] != null)
                {
                    approveStatus = Convert.ToString(Request.QueryString["status"]);
                }
                #endregion Sandip Section For Approval Section Start
                string validate = "";
                string strSchemeType = "";

                if (hdn_Mode.Value == "Entry")
                {
                    //validate = checkNMakeJVCode(strRequisitionNumber.Trim(), Convert.ToInt32(CmbScheme.Value));
                    JVNumStr = txtVoucherNo.Text.Trim();
                    strSchemeType = Convert.ToString(CmbScheme.Value.ToString().Split('~')[0]);
                }
                else
                {
                    JVNumStr = txtVoucherNo.Text.Trim();
                }
                DataView dvData = new DataView(tempQuotation);
                dvData.RowFilter = "Status<>'D'";
                DataTable dt_tempQuotation = dvData.ToTable();

                var duplicateRecords = dt_tempQuotation.AsEnumerable()
                .GroupBy(r => r["ProductID"]) //coloumn name which has the duplicate values
                .Where(gr => gr.Count() > 1)
                 .Select(g => g.Key);

                foreach (var d in duplicateRecords)
                {
                    validate = "duplicateProduct";
                }


                if (hddnMultiUOMSelection.Value == "1")
                {
                    foreach (DataRow dr in tempQuotation.Rows)
                    {
                        DataTable dtStockUOMch = new DataTable();
                        dtStockUOMch = oDBEngine.GetDataTable("select isnull(sProduct_StockUOM,0) sProduct_StockUOM from Master_sProducts where sProducts_ID='" + Convert.ToString(dr["ProductID"]) + "'");

                        string StockUOM = Convert.ToString(dtStockUOMch.Rows[0]["sProduct_StockUOM"]);

                        string strSrlNo = Convert.ToString(dr["SrlNo"]);

                        decimal strProductQuantity = Convert.ToDecimal(dr["Quantity"]);
                        decimal strUOMQuantity = 0;
                        //Rev work start 25.07.2022 mantise no:25072
                        DataTable dtb = new DataTable();
                        string MSrlno = string.Empty;
                        dtb = (DataTable)Session["MultiUOMDataIND"];
                       
                        if (dtb != null)
                        {
                            //Rev work close 25.07.2022 mantise no:25072
                            if (StockUOM != "0")
                            {

                                GetQuantityBaseOnProductforDetailsId(strSrlNo, ref strUOMQuantity);

                                //Rev work start 25.07.2022 mantise no:25072                                   
                                        DataView dvDataView = new DataView(dtb);
                                        dvDataView.RowFilter = "SrlNo = '" + strSrlNo + "'"; 
                                       
                                        for (int i = 0; i < dvDataView.Count; i++)
                                        {
                                            MSrlno = dvDataView[i]["SrlNo"].ToString();
                                        }

                                        if (MSrlno!=strSrlNo)
                                        {
                                            validate = "checkMultiUOMData";
                                            gridBatch.JSProperties["cpcheckMultiUOMData"] = strSrlNo;
                                            break;
                                        }
                                   
                                
                               /* if (Session["MultiUOMDataIND"] != null)
                                {
                                    // Mantis Issue 24428
                                    //if (strUOMQuantity != null)
                                    //{
                                    //    if (strProductQuantity != strUOMQuantity)
                                    //    {
                                    //        validate = "checkMultiUOMData";
                                    //        gridBatch.JSProperties["cpcheckMultiUOMData"] = strSrlNo;
                                    //        break;
                                    //    }
                                    //}
                                    // End of Mantis Issue 24428
                                }
                                else if (Session["MultiUOMDataIND"] == null)
                                {
                                    validate = "checkMultiUOMData";
                                    gridBatch.JSProperties["cpcheckMultiUOMData"] = strSrlNo;
                                    break;
                                }*/
                                // Rev work close 25.07.2022 mantise no:25072
                            }
                            //Rev work start 25.07.2022 mantise no:25072
                        }
                        else
                        {                           
                                validate = "checkMultiUOMData";
                                gridBatch.JSProperties["cpcheckMultiUOMData"] = strSrlNo;
                                break;
                         
                        }
                        //Rev work close 25.07.2022 mantise no:25072
                    }

                }


                if (hdnPageStatus.Value == "update")
                {
                    if (tempQuotation.Rows.Count > 0)
                    {
                        foreach (DataRow dr in tempQuotation.Rows)
                        {
                            string ProductID = Convert.ToString(dr["ProductID"]);
                            decimal ProductQuantity = Convert.ToDecimal(dr["Quantity"]);
                            string Status = Convert.ToString(dr["Status"]);
                            DataTable dtq = new DataTable();
                            DataTable dtdetails = new DataTable();
                            string IndentId = "0";
                            if (Convert.ToString(hdnEditOrderId.Value) == "")
                            {
                                IndentId = "0";
                            }
                            else if (Convert.ToString(hdnEditOrderId.Value) != "")
                            {
                                IndentId = Convert.ToString(hdnEditOrderId.Value);
                            }

                            //if (IsPITransactionExist(Convert.) == false && IsPIIndentTransactionExist(PurIndentID) == false)
                            //if (IsPITransactionExist(IndentId) == true)
                            //{
                            decimal Sum = 0, sum1 = 0;
                            dtq = oDBEngine.GetDataTable("select top 1 ISNULL(TotalQty,0) TotalQty,isnull(BalanceQty,0) BalanceQty from tbl_trans_RequisitionBalanceMapForPurchaseOrder where ProductId='" + ProductID + "' and RequisitionId=(select Indent_Id from tbl_trans_Indent where Indent_RequisitionNumber='" + txtVoucherNo.Text + "' and ISNULL(IndentLastEntry,0)=1)");
                            // dtdetails = oDBEngine.GetDataTable("select top 1 isnull(OrderDetails_Quantity,0.0000) Quantity from tbl_trans_PurchaseOrderDetails where  Requisition_Id='" + Convert.ToInt64(IndentId) + "' and OrderDetails_ProductId='" + ProductID + "'");
                            if (Status != "D" && dtq != null && dtq.Rows.Count > 0)
                            {

                                if (ProductQuantity < Convert.ToDecimal(dtq.Rows[0]["TotalQty"]))
                                {
                                    validate = "ExceedQuantity";
                                    break;
                                }
                            }
                            //    }
                            //else  if (IsPITransactionExist(IndentId) == true)
                            //{
                            //    dtdetails = oDBEngine.GetDataTable("select top 1 isnull(PurchaseQuotationDetails_Quantity,0.0000) Quantity from tbl_trans_PurchaseQuotationProducts where  PurchaseQuotation_IndentId='" + Convert.ToInt64(IndentId) + "' and OrderDetails_ProductId='" + ProductID + "'");
                            //    if (Status != "D" && dtq.Rows.Count > 0)
                            //    {
                            //        if (ProductQuantity < Convert.ToDecimal(dtq.Rows[0]["Quantity"]))
                            //        {
                            //            validate = "ExceedQuantity";
                            //            break;
                            //        }
                            //    }
                            //}



                        }
                    }
                }




                foreach (DataRow dr in tempQuotation.Rows)
                {
                    decimal ProductQuantity = Convert.ToDecimal(dr["Quantity"]);
                    string Status = Convert.ToString(dr["Status"]);

                    if (Status != "D")
                    {
                        if (ProductQuantity == 0)
                        {
                            validate = "nullQuantity";
                            break;
                        }
                    }
                }
                if (validate == "outrange" || validate == "duplicate" || validate == "nullQuantity" || validate == "duplicateProduct" || validate == "checkMultiUOMData" || validate == "ExceedQuantity")
                {
                    gridBatch.JSProperties["cpSaveSuccessOrFail"] = validate;
                }
                else
                {
                    //Rev Tanmoy Project
                    CommonBL cbl = new CommonBL();
                    string ProjectSelectcashbankModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");

                    Int64 ProjId = 0;
                    if (lookup_Project.Text != "")
                    {
                        string projectCode = lookup_Project.Text;
                        DataTable dt = oDBEngine.GetDataTable("select Proj_Id from Master_ProjectManagement where Proj_Code='" + projectCode + "'");
                        ProjId = Convert.ToInt64(dt.Rows[0]["Proj_Id"]);
                    }
                    else if (lookup_Project.Text == "")
                    {
                        ProjId = 0;
                    }
                    //End Rev Tanmoy Project

                    //datatable for MultiUOm start chinmoy 14-01-2020
                    DataTable MultiUOMDetails = new DataTable();

                    if (Session["MultiUOMDataIND"] != null)
                    {
                        // Mantis Issue 24428
                        DataTable MultiUOM = (DataTable)Session["MultiUOMDataIND"];
                       // MultiUOMDetails = MultiUOM.DefaultView.ToTable(false, "SrlNo", "Quantity", "UOM", "AltUOM", "AltQuantity", "UomId", "AltUomId", "ProductId");
                        MultiUOMDetails = MultiUOM.DefaultView.ToTable(false, "SrlNo", "Quantity", "UOM", "AltUOM", "AltQuantity", "UomId", "AltUomId", "ProductId", "BaseRate", "AltRate", "UpdateRow");
                        // End of Mantis Issue 24428
                    }
                    else
                    {
                        MultiUOMDetails.Columns.Add("SrlNo", typeof(string));
                        MultiUOMDetails.Columns.Add("Quantity", typeof(Decimal));
                        MultiUOMDetails.Columns.Add("UOM", typeof(string));
                        MultiUOMDetails.Columns.Add("AltUOM", typeof(string));
                        MultiUOMDetails.Columns.Add("AltQuantity", typeof(Decimal));
                        MultiUOMDetails.Columns.Add("UomId", typeof(Int64));
                        MultiUOMDetails.Columns.Add("AltUomId", typeof(Int64));
                        MultiUOMDetails.Columns.Add("ProductId", typeof(Int64));
                        // MultiUOMDetails.Columns.Add("DetailsId", typeof(string));

                        // Mantis Issue 24428
                        MultiUOMDetails.Columns.Add("BaseRate", typeof(Decimal));
                        MultiUOMDetails.Columns.Add("AltRate", typeof(Decimal));
                        MultiUOMDetails.Columns.Add("UpdateRow", typeof(string));
                        // End of Mantis Issue 24428

                    }



                    int striscomplete = 0;


                    string AppRejRemarks = "";
                    DataTable IndentEditdetails = GetPurchaseIndentEditData();
                    if (IndentEditdetails != null && IndentEditdetails.Rows.Count > 0)
                    {
                        if (hdnPageStatForApprove.Value != "PO")
                        {
                            hdnApproveStatus.Value = Convert.ToString(IndentEditdetails.Rows[0]["Indent_ApproveStatus"]);
                        }
                        AppRejRemarks = Convert.ToString(IndentEditdetails.Rows[0]["Indent_ApprovalRemarks"]);
                        txtAppRejRemarks.Text = AppRejRemarks;
                    }
                    if (hdnApproveStatus.Value == "")
                    {
                        hdnApproveStatus.Value = "0";
                    }
                    int ApproveRejectstatus = Convert.ToInt32(hdnApproveStatus.Value);
                    string RevisionNo = Convert.ToString(txtRevisionNo.Text);
                    AppRejRemarks = Convert.ToString(hdnApprovalRemarksValue.Value);
                    string RevisionDate = "";
                    if (hdnApprovalReqInq.Value == "1")
                    {
                        RevisionDate = Convert.ToString(txtRevisionDate.Text);
                    }

                    //End
                    //Rev Subhra 01-03-2019 For UOM
                    #region Add New Filed To Check from Table

                    DataTable duplicatedt2 = new DataTable();
                    duplicatedt2.Columns.Add("productid", typeof(Int64));
                    duplicatedt2.Columns.Add("slno", typeof(Int32));
                    duplicatedt2.Columns.Add("Quantity", typeof(Decimal));
                    duplicatedt2.Columns.Add("packing", typeof(Decimal));
                    duplicatedt2.Columns.Add("PackingUom", typeof(Int32));
                    duplicatedt2.Columns.Add("PackingSelectUom", typeof(Int32));

                    if (HttpContext.Current.Session["SessionPackingDetails"] != null)
                    {
                        List<ProductQuantity> obj = new List<ProductQuantity>();
                        obj = (List<ProductQuantity>)HttpContext.Current.Session["SessionPackingDetails"];
                        foreach (var item in obj)
                        {
                            duplicatedt2.Rows.Add(item.productid, item.slno, item.Quantity, item.packing, item.PackingUom, item.PackingSelectUom);
                        }
                    }
                    HttpContext.Current.Session["SessionPackingDetails"] = null;
                    DataTable dtAddlDesc = (DataTable)Session["InlineRemarks"];
                    #endregion
                    string AppSettingsVal = Convert.ToString(hdnApprovalReqInq.Value);


                    String ORDER_IDs = "";
                    string IndentRequisitionDate = string.Empty;
                    for (int i = 0; i < taggingGrid.GetSelectedFieldValues("MRP_ID").Count; i++)
                    {
                        ORDER_IDs += "," + Convert.ToString(taggingGrid.GetSelectedFieldValues("MRP_ID")[i]);
                    }
                    ORDER_IDs = ORDER_IDs.TrimStart(',');

                    if (ORDER_IDs == "")
                    {
                        ORDER_IDs = hdnComponent.Value;
                    }

                    String ORDERDetailsIDs = "", ProductIDs="";

                    for (int i = 0; i < grid_Products.GetSelectedFieldValues("ProductID").Count; i++)
                    {
                        ProductIDs += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("ProductID")[i]);
                        ORDERDetailsIDs += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("ComponentDetailsID")[i]);
                    }
                    ORDERDetailsIDs = ORDERDetailsIDs.TrimStart(',');

                    if(ORDERDetailsIDs=="")
                    {
                        ORDERDetailsIDs=hdnComponentDetailsIDs.Value;
                    }

                    String TempDetailsID = "";

                    for (int i = 0; i < gridTemplateproducts.GetSelectedFieldValues("ComponentDetailsID").Count; i++)
                    {
                        TempDetailsID += "," + Convert.ToString(gridTemplateproducts.GetSelectedFieldValues("ComponentDetailsID")[i]);
                    }
                    TempDetailsID = TempDetailsID.TrimStart(',');


                    foreach (DataRow dr in tempQuotation.Rows)
                    {
                        string ExpectedDeliveryDate = Convert.ToString(dr["ExpectedDeliveryDate"]);

                        if(ExpectedDeliveryDate=="")
                        {
                            dr["ExpectedDeliveryDate"] =Convert.ToString("1900-01-01 00:00:00.000");
                        }

                    }

                    string FromDate = dtFromDate.Date.ToString("yyyy-MM-dd");
                    string ToDate = dtToDate.Date.ToString("yyyy-MM-dd");

                    tempQuotation.AcceptChanges();
                    if (tempQuotation.Columns.Contains("BalanceQty"))
                    {
                        tempQuotation.Columns.Remove("BalanceQty");
                        tempQuotation.AcceptChanges();
                    }

                    //if (Save_Record(strIndentType, strIndent_Id, JVNumStr, strRequisitionDate, strBranch, strPurpose, strCurrency, strExchangeRate, BaseCurrencyId, tempQuotation, ActionType, approveStatus) == false)
                    // Mantis Issue 25235 [strVendor added to parameter ]
                    Save_Record(strIndentType, strIndent_Id, strSchemeType, ref JVNumStr, strRequisitionDate, strBranch, strPurpose, strCurrency, strExchangeRate, BaseCurrencyId, tempQuotation, ActionType, approveStatus, duplicatedt2, ProjId, MultiUOMDetails,
                        ref striscomplete, dtAddlDesc, ApproveRejectstatus, RevisionNo, RevisionDate, AppRejRemarks, AppSettingsVal, strForBranch, ORDER_IDs, ORDERDetailsIDs, TempDetailsID, FromDate, ToDate, ProductIDs, strVendor);
                    //End of Mantis Issue 25235
                    if (striscomplete != 1 && striscomplete != -12)
                    {
                        gridBatch.JSProperties["cpSaveSuccessOrFail"] = "errorInsert";
                    }
                    else if (striscomplete == -12)
                    {
                        gridBatch.JSProperties["cpSaveSuccessOrFail"] = "EmptyProject";
                    }
                    else
                    {
                        gridBatch.JSProperties["cpVouvherNo"] = JVNumStr;

                        //Mantis Issue 25053
                        if (ActionType == "ADD")
                        {
                            string SMSRequiredInDirectorApproval = cbl.GetSystemSettingsResult("SMSRequiredInDirectorApproval");
                            if (SMSRequiredInDirectorApproval == "Yes")
                            {
                                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                                string DataBase = con.Database;

                                string baseUrl = System.Configuration.ConfigurationSettings.AppSettings["baseUrl"];
                                //string baseUrl = "https://3.7.30.86:85";
                                string IndentId = hdnIndentId.Value;
                                string LongURL = baseUrl + "/ServiceManagement/Transaction/indentMView/indentApproval.aspx?id=" + IndentId + "&UniqueKey=" + Convert.ToString(DataBase);

                                string tinyURL = ShortURL(LongURL);
                                string EmpId = hdnEmployee.Value;
                                ProcedureExecute proc1 = new ProcedureExecute("prc_PurchaseIndentDetailsList");
                                proc1.AddPara("@Action", Convert.ToString("ApprovalSendSMS"));
                                proc1.AddPara("@tinyURL", Convert.ToString(tinyURL));
                                proc1.AddPara("@EmpId", Convert.ToString(EmpId));
                                // Mantis Issue 25241
                                proc1.AddPara("@Indent_Id", IndentId);
                                proc1.AddPara("@DataBase", DataBase);
                                // End of Mantis Issue 25241
                                proc1.GetTable();
                            }
                        }
                        
                        //End of Mantis Issue 25053
                    }

                    #region Sandip Section For Approval Section Start
                    if (approveStatus != "")
                    {
                        gridBatch.JSProperties["cpApproverStatus"] = "approve";
                    }
                    #endregion Sandip Section For Approval Section Start
                    e.Handled = true;
                }


            }
            else
            {
                DataView dvData = new DataView(Indentdt);
                dvData.RowFilter = "Status <> 'D'";

                gridBatch.DataSource = GetPurchaseIndent(dvData.ToTable());
                gridBatch.DataBind();
            }
        }
        //Mantis Issue 25053
        private static string ShortURL(string LongUrl)
        {
            try
            {
                if (LongUrl.Length <= 30)
                {
                    return LongUrl;
                }
                if (!LongUrl.ToLower().StartsWith("http") && !LongUrl.ToLower().StartsWith("ftp"))
                {
                    LongUrl = "http://" + LongUrl;
                }
                var request = WebRequest.Create("http://tinyurl.com/api-create.php?url=" + LongUrl);
                var res = request.GetResponse();
                string text;
                using (var reader = new StreamReader(res.GetResponseStream()))
                {
                    text = reader.ReadToEnd();
                }
                return text;
            }
            catch (Exception)
            {
                return LongUrl;
            }
        }

        [WebMethod]
        public static string SendSMSManualNo(String PIndentId, String EmployeeId)
        {
            //string SMSRequiredInDirectorApproval = cbl.GetSystemSettingsResult("SMSRequiredInDirectorApproval");
            //if (SMSRequiredInDirectorApproval == "Yes")
            //{
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                string DataBase = con.Database;

                string baseUrl = System.Configuration.ConfigurationSettings.AppSettings["baseUrl"];
                //string baseUrl = "https://3.7.30.86:85";
                string IndentId = PIndentId;
                string LongURL = baseUrl + "/ServiceManagement/Transaction/indentMView/indentApproval.aspx?id=" + IndentId + "&UniqueKey=" + Convert.ToString(DataBase);

                string tinyURL = ShortURL(LongURL);
                string EmpId = EmployeeId;
                //ProcedureExecute proc1 = new ProcedureExecute("prc_PurchaseIndentDetailsList");
                //proc1.AddPara("@Action", Convert.ToString("ApprovalSendSMS"));
                //proc1.AddPara("@tinyURL", Convert.ToString(tinyURL));
                //proc1.AddPara("@EmpId", Convert.ToString(EmpId));
                //proc1.GetTable();
            //}
                string output = string.Empty;
                try
                {
                    int NoOfRowEffected = 0;
                    if (HttpContext.Current.Session["userid"] != null)
                    {
                        int user_id = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                        ProcedureExecute proc = new ProcedureExecute("prc_PurchaseIndentDetailsList");
                        proc.AddVarcharPara("@Action", 500, "ApprovalSendSMS");
                        proc.AddPara("@tinyURL", Convert.ToString(tinyURL));
                        proc.AddPara("@EmpId", Convert.ToString(EmpId));
                        // Mantis Issue 25241
                        proc.AddPara("@Indent_Id", IndentId);
                        proc.AddPara("@DataBase", DataBase);
                        // End of Mantis Issue 25241
                        NoOfRowEffected = proc.RunActionQuery();
                        if (NoOfRowEffected > 0)
                        {
                        }
                        output = "true";
                    }
                }
                catch (Exception ex)
                {
                    output = ex.Message.ToString();
                }
                return output;
        }
        //End of Mantis Issue 25053
        public DataTable GetProjectEditData(string VendorePayID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CustomerPaymentReciptProjectID");
            proc.AddIntegerPara("@Receipt_ID", Convert.ToInt32(VendorePayID));
            proc.AddVarcharPara("@Action", 100, "Purchase_Indent");
            dt = proc.GetTable();
            return dt;
        }

        public IEnumerable GetPurchaseIndent(DataTable PurchaseIndentdt)
        {
            List<BratchGridLIST> BratchGridLists = new List<BratchGridLIST>();
            DataColumnCollection dtc = PurchaseIndentdt.Columns;
            for (int i = 0; i < PurchaseIndentdt.Rows.Count; i++)
            {
                BratchGridLIST BratchGrid_LIST = new BratchGridLIST();
                BratchGrid_LIST.SrlNo = Convert.ToString(PurchaseIndentdt.Rows[i]["SrlNo"]);
                BratchGrid_LIST.PurchaseIndentID = Convert.ToString(PurchaseIndentdt.Rows[i]["IndentDetailsId"]);
                BratchGrid_LIST.gvColProduct = Convert.ToString(PurchaseIndentdt.Rows[i]["ProductID"]);
                BratchGrid_LIST.gvColDiscription = Convert.ToString(PurchaseIndentdt.Rows[i]["Description"]);
                BratchGrid_LIST.gvColQuantity = Convert.ToString(PurchaseIndentdt.Rows[i]["Quantity"]);
                BratchGrid_LIST.gvColUOM = Convert.ToString(PurchaseIndentdt.Rows[i]["UOM"]);
                BratchGrid_LIST.gvColRate = Convert.ToString(PurchaseIndentdt.Rows[i]["Rate"]);

                BratchGrid_LIST.gvColValue = Convert.ToString(PurchaseIndentdt.Rows[i]["ValueInBaseCurrency"]);
                string ExpectedDeliveryDate = Convert.ToString(PurchaseIndentdt.Rows[i]["ExpectedDeliveryDate"]);
                if (!String.IsNullOrEmpty(ExpectedDeliveryDate))
                {
                    BratchGrid_LIST.ExpectedDeliveryDate = Convert.ToDateTime(PurchaseIndentdt.Rows[i]["ExpectedDeliveryDate"]);//8
                }
                else
                {
                    BratchGrid_LIST.ExpectedDeliveryDate = null;
                }
                BratchGrid_LIST.Status = Convert.ToString(PurchaseIndentdt.Rows[i]["Status"]);
                BratchGrid_LIST.AvailableStock = Convert.ToString(PurchaseIndentdt.Rows[i]["AvailableStock"]);
                BratchGrid_LIST.ProductName = Convert.ToString(PurchaseIndentdt.Rows[i]["ProductName"]);//12
                if (dtc.Contains("Remarks"))
                {
                    BratchGrid_LIST.Remarks = Convert.ToString(PurchaseIndentdt.Rows[i]["Remarks"]);
                }
                BratchGrid_LIST.ServiceTempDetails_ID = Convert.ToString(PurchaseIndentdt.Rows[i]["ServiceTempDetails_ID"]);
                BratchGrid_LIST.ServiceTemplate_ID = Convert.ToString(PurchaseIndentdt.Rows[i]["ServiceTemplate_ID"]);

                BratchGrid_LIST.Order_AltQuantity = Convert.ToString(PurchaseIndentdt.Rows[i]["Order_AltQuantity"]);
                BratchGrid_LIST.Order_AltUOM = Convert.ToString(PurchaseIndentdt.Rows[i]["Order_AltUOM"]);
                /*Rev work start date:09.05.2022 Mantise No 0024875: An error message is appearing while saving Purchase Indent*/
               // BratchGrid_LIST.BalanceQty = Convert.ToString(PurchaseIndentdt.Rows[i]["BalanceQty"]);
                /*Rev work close date:09.05.2022 Mantise No 0024875: An error message is appearing while saving Purchase Indent*/
                BratchGridLists.Add(BratchGrid_LIST);
            }

            return BratchGridLists;
        }
        protected string checkNMakeJVCode(string manual_str, int sel_schema_Id)
        {
            //oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            //oDbEngine = new BusinessLogicLayer.DBEngine();



            DataTable dtSchema = new DataTable();
            DataTable dtC = new DataTable();
            string prefCompCode = string.Empty, sufxCompCode = string.Empty, startNo, paddedStr, sqlQuery = string.Empty;
            int EmpCode, prefLen, sufxLen, paddCounter;

            if (sel_schema_Id > 0)
            {
                dtSchema = oDBEngine.GetDataTable("tbl_master_idschema", "prefix, suffix, digit, startno, schema_type", "id=" + sel_schema_Id);
                int scheme_type = Convert.ToInt32(dtSchema.Rows[0]["schema_type"]);

                if (scheme_type != 0)
                {
                    startNo = dtSchema.Rows[0]["startno"].ToString();
                    paddCounter = Convert.ToInt32(dtSchema.Rows[0]["digit"]);
                    paddedStr = startNo.PadLeft(paddCounter, '0');
                    prefCompCode = dtSchema.Rows[0]["prefix"].ToString();
                    sufxCompCode = dtSchema.Rows[0]["suffix"].ToString();
                    prefLen = Convert.ToInt32(prefCompCode.Length);
                    sufxLen = Convert.ToInt32(sufxCompCode.Length);

                    sqlQuery = "SELECT max(tjv.Indent_RequisitionNumber) FROM tbl_trans_Indent tjv WHERE dbo.RegexMatch('";
                    if (prefLen > 0)
                        sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                    else if (scheme_type == 2)
                        sqlQuery += "^";
                    sqlQuery += "[0-9]{" + paddCounter + "}";
                    if (sufxLen > 0)
                        sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                    // sqlQuery += "?$', LTRIM(RTRIM(tjv.Indent_RequisitionNumber))) = 1";
                    sqlQuery += "?$', LTRIM(RTRIM(tjv.Indent_RequisitionNumber))) = 1 and Indent_RequisitionNumber like '" + prefCompCode + "%'";
                    if (scheme_type == 2)
                        sqlQuery += " AND CONVERT(DATE, Indent_CreatedDate) = CONVERT(DATE, GETDATE())";
                    dtC = oDBEngine.GetDataTable(sqlQuery);

                    if (dtC.Rows[0][0].ToString() == "")
                    {
                        sqlQuery = "SELECT max(tjv.Indent_RequisitionNumber) FROM tbl_trans_Indent tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.Indent_RequisitionNumber))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.Indent_RequisitionNumber))) = 1 and Indent_RequisitionNumber like '" + prefCompCode + "%'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, Indent_CreatedDate) = CONVERT(DATE, GETDATE())";
                        dtC = oDBEngine.GetDataTable(sqlQuery);
                    }

                    if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
                    {
                        string uccCode = dtC.Rows[0][0].ToString().Trim();
                        int UCCLen = uccCode.Length;
                        int decimalPartLen = UCCLen - (prefCompCode.Length + sufxCompCode.Length);
                        string uccCodeSubstring = uccCode.Substring(prefCompCode.Length, decimalPartLen);
                        EmpCode = Convert.ToInt32(uccCodeSubstring) + 1;
                        // out of range journal scheme
                        if (EmpCode.ToString().Length > paddCounter)
                        {
                            return "outrange";
                        }
                        else
                        {
                            paddedStr = EmpCode.ToString().PadLeft(paddCounter, '0');
                            JVNumStr = prefCompCode + paddedStr + sufxCompCode;
                            return "ok";
                        }
                    }
                    else
                    {
                        JVNumStr = startNo.PadLeft(paddCounter, '0');
                        JVNumStr = prefCompCode + paddedStr + sufxCompCode;
                        return "ok";
                    }
                }
                else
                {
                    sqlQuery = "SELECT Indent_RequisitionNumber FROM tbl_trans_Indent WHERE Indent_RequisitionNumber LIKE '" + manual_str.Trim() + "'";
                    dtC = oDBEngine.GetDataTable(sqlQuery);
                    // duplicate manual entry check
                    if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
                    {
                        return "duplicate";
                    }

                    JVNumStr = manual_str.Trim();
                    return "ok";
                }
            }
            else
            {
                return "noid";
            }
        }

        //Rev Subhra 01-03-2019 For UOM
        //private bool Save_Record(string strIndentType,string strIndent_Id, string strRequisitionNumber, string strRequisitionDate, string strBranch, string strPurpose, string strCurrency,
        // string strExchangeRate,int BaseCurrencyId,
        // DataTable Indentdt, string ActionType, string approveStatus)


        // Mantis Issue 25235 [ parameter strVendor added ]
        public void Save_Record(string strIndentType, string strIndent_Id, string strSchemeType, ref string strRequisitionNumber, string strRequisitionDate, string strBranch, string strPurpose, string strCurrency,
            string strExchangeRate, int BaseCurrencyId,
            DataTable Indentdt, string ActionType, string approveStatus, DataTable PackingDetailsdt, Int64 ProjId, DataTable MultiUOMDetails, ref int striscomplete,
            DataTable dtAddlDesc, int ApproveRejectstatus, string RevisionNo, string RevisionDate, string AppRejRemarks, string AppSettingsVal, string strForBranch
            , string ORDER_IDs, string ORDERDetailsIDs, string TempDetailsID, string FromDate, string ToDate, string ProductIDs, string strVendor)

        //End of Rev
        {
            try
            {
                gridBatch.JSProperties["cpExitNew"] = null;
                gridBatch.JSProperties["cpVouvherNo"] = null;

                
                DataSet dsInst = new DataSet();

                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                if (hdnApprovalReqInq.Value == "0")
                {
                    ApproveRejectstatus = 1;
                }
                //Mantis Issue 24912
                //if (hdnPageStatus.Value == "Copy")
                //{
                //    ApproveRejectstatus = 0;
                //}
                //End of Mantis Issue 24912
                //Rev  Manis 24428
                //SqlCommand cmd = new SqlCommand("prc_PurchaseIndent", con);

                SqlCommand cmd = new SqlCommand("prc_PurchaseIndentNew", con);

                //Rev Manis 24428

                cmd.CommandType = CommandType.StoredProcedure;

                if ((ApproveRejectstatus == 1) && (hdnPageStatForApprove.Value == "") && (hdnApprovalReqInq.Value == "1"))
                {
                    cmd.Parameters.AddWithValue("@Action", "ADD");
                    strRequisitionNumber = txtVoucherNo.Text;
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Action", ActionType);
                }
                // cmd.Parameters.AddWithValue("@Action", ActionType);

                cmd.Parameters.AddWithValue("@ApprovalSettingsVal", AppSettingsVal);
                cmd.Parameters.AddWithValue("@Indent_EditId", strIndent_Id);
                cmd.Parameters.AddWithValue("@IndentType", strIndentType);

                cmd.Parameters.AddWithValue("@SchemaID", strSchemeType);
                cmd.Parameters.AddWithValue("@Indent_RequisitionNumber", strRequisitionNumber);

                if (!String.IsNullOrEmpty(strRequisitionDate))
                    cmd.Parameters.AddWithValue("@Indent_RequisitionDate", Convert.ToDateTime(strRequisitionDate));

                //cmd.Parameters.AddWithValue("@Indent_RequisitionDate", strRequisitionDate);
                cmd.Parameters.AddWithValue("@Indent_BranchIdFor", strBranch);
                cmd.Parameters.AddWithValue("@Indent_BranchIdTo", "0");
                cmd.Parameters.AddWithValue("@Indent_Purpose", strPurpose);
                cmd.Parameters.AddWithValue("@Indent_baseCurrencyId", BaseCurrencyId);
                cmd.Parameters.AddWithValue("@Indent_CurrencyId", strCurrency);
                cmd.Parameters.AddWithValue("@Indent_ExchangeRtae", strExchangeRate);

                cmd.Parameters.AddWithValue("@ForBranchId", strForBranch);
                cmd.Parameters.AddWithValue("@Indent_Company", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@Indent_FinYear", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@Indent_CreatedBy", Convert.ToString(Session["userid"]));

                cmd.Parameters.AddWithValue("@PurchaseIndentDetails", Indentdt);
                cmd.Parameters.AddWithValue("@Project_Id", ProjId);
                // Mantis Issue 25235
                cmd.Parameters.AddWithValue("@strVendor", strVendor);
                // End of Mantis Issue 25235
                #region By Subhra Insert Or Update UOM conversion in Indent
                cmd.Parameters.AddWithValue("@PackingDetails", PackingDetailsdt);
                cmd.Parameters.AddWithValue("@MultiUOMDetails", MultiUOMDetails);
                cmd.Parameters.AddWithValue("@udt_Addldesc", dtAddlDesc);
                #endregion

                cmd.Parameters.AddWithValue("@RevisionNo", RevisionNo);
                cmd.Parameters.AddWithValue("@ApproveRejectStatus", ApproveRejectstatus);
                cmd.Parameters.AddWithValue("@ApproveRejectRemarks", AppRejRemarks);
                if (RevisionDate != "1/1/0001 12:00:00 AM")
                {
                    if (!String.IsNullOrEmpty(RevisionDate))
                        //cmd.Parameters.AddWithValue("@ProjectValidfromDate", Convert.ToDateTime(projectValidFrom));	
                        cmd.Parameters.AddWithValue("@RevisionDate", DateTime.ParseExact(RevisionDate, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"));
                }

                // SqlParameter output = new SqlParameter("@ReturnValueID", SqlDbType.Int);

                cmd.Parameters.AddWithValue("@ORDER_IDs", ORDER_IDs);
                cmd.Parameters.AddWithValue("@ORDERDetailsIDs", ORDERDetailsIDs);
                cmd.Parameters.AddWithValue("@TempDetailsID", TempDetailsID);
                if (FromDate != "0001-01-01")
                {
                    cmd.Parameters.AddWithValue("@FromDate", FromDate);
                }

                if (ToDate != "0001-01-01")
                {
                    cmd.Parameters.AddWithValue("@ToDate", ToDate);
                }

               
                cmd.Parameters.AddWithValue("@ProductIDs", ProductIDs);




                cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
                cmd.Parameters.Add("@ReturnValueID", SqlDbType.Int);
                cmd.Parameters.Add("@ReturnText", SqlDbType.VarChar, 500);
                cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
                cmd.Parameters["@ReturnValueID"].Direction = ParameterDirection.Output;
                cmd.Parameters["@ReturnText"].Direction = ParameterDirection.Output;

                //cmd.Parameters.Add(output);

                #region Sandip Section For Approval Section Start
                cmd.Parameters.AddWithValue("@approveStatus", approveStatus);
                #endregion Sandip Section For Approval Section Start
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);

                striscomplete = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());
                string strQuoteID = Convert.ToString(cmd.Parameters["@ReturnValueID"].Value.ToString());
                //Mantis Issue 25053
                hdnIndentId.Value = Convert.ToString(cmd.Parameters["@ReturnValueID"].Value.ToString());
                //End of Mantis Issue 25053
                JVNumStr = Convert.ToString(cmd.Parameters["@ReturnText"].Value.ToString());
                cmd.Dispose();
                con.Dispose();
                gridBatch.JSProperties["cpExitNew"] = "YES";


                //Udf Add mode
                DataTable udfTable = (DataTable)Session["UdfDataOnAdd"];
                if (udfTable != null)
                {
                    Session["UdfDataOnAdd"] = reCat.insertRemarksCategoryAddMode("PI", "PurchaseIndent" + Convert.ToString(strQuoteID), udfTable, Convert.ToString(Session["userid"]));
                }
                // return true;
            }
            catch (Exception ex)
            {
                // return false;
            }
        }
        [WebMethod]
        public static bool CheckUniqueName(string VoucherNo)
        {
            MShortNameCheckingBL objMShortNameCheckingBL = new MShortNameCheckingBL();
            DataTable dt = new DataTable();
            Boolean status = false;
            BusinessLogicLayer.GenericMethod oGeneric = new BusinessLogicLayer.GenericMethod();

            if (VoucherNo != "" && Convert.ToString(VoucherNo).Trim() != "")
            {
                status = objMShortNameCheckingBL.CheckUnique(Convert.ToString(VoucherNo).Trim(), "0", "PurchaseIndent_Check");
            }
            return status;
        }

        protected void MultiUOM_DataBinding(object sender, EventArgs e)
        {
            //DataTable dt = (DataTable)Session["MultiUOMDataIND"];
            //if(dt !=null && dt.Rows.Count >0 )
            //{
            //    DataView dvData = new DataView(dt);
            //    //dvData.RowFilter = "Product_SrlNo = '" + SerialID + "'";

            //    grid_MultiUOM.DataSource = dvData;
            //    grid_MultiUOM.DataBind();
            //}
            //else
            //{
            //    grid_MultiUOM.DataSource = null;
            //    grid_MultiUOM.DataBind();
            //}
        }

        protected void MultiUOM_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string SpltCmmd = e.Parameters.Split('~')[0];
            grid_MultiUOM.JSProperties["cpDuplicateAltUOM"] = "";
            // Rev Mantis Issue 24428/24429
            grid_MultiUOM.JSProperties["cpSetBaseQtyRateInGrid"] = "";
            // End of Rev Mantis Issue 24428/24429
            grid_MultiUOM.JSProperties["cpExistMultiUOMRow"] = "";
            if (SpltCmmd == "MultiUOMDisPlay")
            {
                grid_MultiUOM.JSProperties["cpOpenFocus"] = "";
                DataTable MultiUOMData = new DataTable();

                if (Session["MultiUOMDataIND"] != null)
                {
                    MultiUOMData = (DataTable)Session["MultiUOMDataIND"];
                }
                else
                {
                    MultiUOMData.Columns.Add("SrlNo", typeof(string));
                    MultiUOMData.Columns.Add("Quantity", typeof(Decimal));
                    MultiUOMData.Columns.Add("UOM", typeof(string));
                    MultiUOMData.Columns.Add("AltUOM", typeof(string));
                    MultiUOMData.Columns.Add("AltQuantity", typeof(Decimal));
                    MultiUOMData.Columns.Add("UomId", typeof(Int64));
                    MultiUOMData.Columns.Add("AltUomId", typeof(Int64));
                    MultiUOMData.Columns.Add("ProductId", typeof(Int64));

                }
                if (MultiUOMData != null && MultiUOMData.Rows.Count > 0)
                {
                    string SrlNo = e.Parameters.Split('~')[1];
                    DataView dvData = new DataView(MultiUOMData);
                    //dvData.RowFilter = "Product_SrlNo = '" + SerialID + "'";
                    dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                    grid_MultiUOM.DataSource = dvData;
                    grid_MultiUOM.DataBind();
                }
                else
                {
                    grid_MultiUOM.DataSource = MultiUOMData.DefaultView;
                    grid_MultiUOM.DataBind();
                }
                grid_MultiUOM.JSProperties["cpOpenFocus"] = "OpenFocus";
            }



            else if (SpltCmmd == "SaveDisplay")
            {

                string Validcheck = "";
                DataTable MultiUOMSaveData = new DataTable();

                int MultiUOMSR = 1;

                string SrlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                string Quantity = Convert.ToString(e.Parameters.Split('~')[2]);
                string UOM = Convert.ToString(e.Parameters.Split('~')[3]);
                string AltUOM = Convert.ToString(e.Parameters.Split('~')[4]);
                string AltQuantity = Convert.ToString(e.Parameters.Split('~')[5]);
                string UomId = Convert.ToString(e.Parameters.Split('~')[6]);
                string AltUomId = Convert.ToString(e.Parameters.Split('~')[7]);
                string ProductId = Convert.ToString(e.Parameters.Split('~')[8]);

                // Mantis Issue 24428
                string BaseRate = Convert.ToString(e.Parameters.Split('~')[9]);
                string AltRate = Convert.ToString(e.Parameters.Split('~')[10]);
                string UpdateRow = Convert.ToString(e.Parameters.Split('~')[11]);




                // End of Mantis Issue 24428

                DataTable allMultidataDetails = (DataTable)Session["MultiUOMDataIND"];



                if (allMultidataDetails != null && allMultidataDetails.Rows.Count > 0)
                {
                    DataRow[] MultiUoMresult = allMultidataDetails.Select("SrlNo ='" + SrlNo + "'");

                    foreach (DataRow item in MultiUoMresult)
                    {
                        if ((AltUomId == item["AltUomId"].ToString()) || (UomId == item["AltUomId"].ToString()))
                        {
                            if (AltQuantity == item["AltQuantity"].ToString())
                            {
                                Validcheck = "DuplicateUOM";
                                grid_MultiUOM.JSProperties["cpDuplicateAltUOM"] = "DuplicateAltUOM";
                                break;
                            }
                        }
                        // Mantis Issue 24428  [ if "Update Row" checkbox is checked, then all existing Update Row in the grid will be set to False]
                        if (UpdateRow == "True")
                        {
                            item["UpdateRow"] = "False";
                        }
                        // End of Mantis Issue 24428 
                    }
                }

                if (Validcheck != "DuplicateUOM")
                {
                    if (Session["MultiUOMDataIND"] != null)
                    {

                        MultiUOMSaveData = (DataTable)Session["MultiUOMDataIND"];

                    }
                    else
                    {
                        MultiUOMSaveData.Columns.Add("SrlNo", typeof(string));
                        MultiUOMSaveData.Columns.Add("Quantity", typeof(Decimal));
                        MultiUOMSaveData.Columns.Add("UOM", typeof(string));
                        MultiUOMSaveData.Columns.Add("AltUOM", typeof(string));
                        MultiUOMSaveData.Columns.Add("AltQuantity", typeof(Decimal));
                        MultiUOMSaveData.Columns.Add("UomId", typeof(Int64));
                        MultiUOMSaveData.Columns.Add("AltUomId", typeof(Int64));
                        MultiUOMSaveData.Columns.Add("ProductId", typeof(Int64));


                        // Mantis Issue 24428
                        MultiUOMSaveData.Columns.Add("BaseRate", typeof(Decimal));
                        MultiUOMSaveData.Columns.Add("AltRate", typeof(Decimal));
                        MultiUOMSaveData.Columns.Add("UpdateRow", typeof(string));

                        DataColumn myDataColumn = new DataColumn();
                        myDataColumn.AllowDBNull = false;
                        myDataColumn.AutoIncrement = true;
                        myDataColumn.AutoIncrementSeed = 1;
                        myDataColumn.AutoIncrementStep = 1;
                        myDataColumn.ColumnName = "MultiUOMSR";
                        myDataColumn.DataType = System.Type.GetType("System.Int32");
                        myDataColumn.Unique = true;
                        MultiUOMSaveData.Columns.Add(myDataColumn);

                        // End of Mantis Issue 24428
                    }
                    // Mantis Issue 24428
                      DataRow thisRow;
                      if (MultiUOMSaveData.Rows.Count > 0)
                      {
                          //thisRow = (DataRow)MultiUOMSaveData.Rows[MultiUOMSaveData.Rows.Count - 1];
                          //Rev Mantis Issue 24428/24429
                          MultiUOMSR = Convert.ToInt32(MultiUOMSaveData.Compute("max([MultiUOMSR])", string.Empty)) + 1;
                          //End Rev Mantis Issue 24428/24429
                          MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, BaseRate, AltRate, UpdateRow, MultiUOMSR);
                      }
                      else
                      {
                          MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, BaseRate, AltRate, UpdateRow, MultiUOMSR);
                      }
                
                    MultiUOMSaveData.AcceptChanges();
                    Session["MultiUOMDataIND"] = MultiUOMSaveData;

                    if (MultiUOMSaveData != null && MultiUOMSaveData.Rows.Count > 0)
                    {
                        DataView dvData = new DataView(MultiUOMSaveData);
                        dvData.RowFilter = "SrlNo = '" + SrlNo + "'";

                        grid_MultiUOM.DataSource = dvData;
                        grid_MultiUOM.DataBind();
                    }
                    else
                    {
                        //MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId);
                        //Session["MultiUOMDataIND"] = MultiUOMSaveData;
                        grid_MultiUOM.DataSource = MultiUOMSaveData.DefaultView;
                        grid_MultiUOM.DataBind();
                    }


                }
            }

            else if (SpltCmmd == "MultiUomDelete")
            {
                string AltUOMKeyValuewithqnty = e.Parameters.Split('~')[1];
                string AltUOMKeyValue = AltUOMKeyValuewithqnty.Split('|')[0];
                string AltUOMKeyqnty = AltUOMKeyValuewithqnty.Split('|')[1];

                string SrlNo = Convert.ToString(e.Parameters.Split('~')[2]);
                DataTable dt = (DataTable)Session["MultiUOMDataIND"];

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] MultiUoMresult = dt.Select("SrlNo ='" + SrlNo + "'");
                    foreach (DataRow item in MultiUoMresult)
                    {
                        if (AltUOMKeyValue.ToString() == item["AltUomId"].ToString())
                        {
                            //dt.Rows.Remove(item);
                            if (AltUOMKeyqnty.ToString() == item["AltQuantity"].ToString())
                            {
                                item.Table.Rows.Remove(item);
                                break;
                            }
                        }
                    }
                }
                Session["MultiUOMDataIND"] = dt;
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataView dvData = new DataView(dt);
                    dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                    grid_MultiUOM.DataSource = dvData;
                    grid_MultiUOM.DataBind();
                }
                else
                {
                    grid_MultiUOM.DataSource = null;
                    grid_MultiUOM.DataBind();
                }
            }


            else if (SpltCmmd == "CheckMultiUOmDetailsQuantity")
            {
                string SrlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                DataTable dt = (DataTable)Session["MultiUOMDataIND"];

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] MultiUoMresult = dt.Select("SrlNo ='" + SrlNo + "'");
                    foreach (DataRow item in MultiUoMresult)
                    {
                        item.Table.Rows.Remove(item);
                    }
                }
                Session["MultiUOMDataIND"] = dt;
            }

              // Mantis Issue 24428
            else if (SpltCmmd == "EditData")
            {
                string AltUOMKeyValuewithqnty = e.Parameters.Split('~')[1];
                string AltUOMKeyValue = e.Parameters.Split('~')[2];
                string AltUOMKeyqnty = AltUOMKeyValuewithqnty.Split('|')[1];

                string SrlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                DataTable dt = (DataTable)Session["MultiUOMDataIND"];

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] MultiUoMresult = dt.Select("MultiUOMSR ='" + AltUOMKeyValue + "'");

                    Decimal BaseQty = Convert.ToDecimal(MultiUoMresult[0]["Quantity"]);
                    Decimal BaseRate = Convert.ToDecimal(MultiUoMresult[0]["BaseRate"]);

                    Decimal AltQty = Convert.ToDecimal(MultiUoMresult[0]["AltQuantity"]);
                    Decimal AltRate = Convert.ToDecimal(MultiUoMresult[0]["AltRate"]);
                    Decimal AltUom = Convert.ToDecimal(MultiUoMresult[0]["AltUomId"]);
                    bool UpdateRow = Convert.ToBoolean(MultiUoMresult[0]["UpdateRow"]);

                    grid_MultiUOM.JSProperties["cpAllDetails"] = "EditData";

                    grid_MultiUOM.JSProperties["cpBaseQty"] = BaseQty;
                    grid_MultiUOM.JSProperties["cpBaseRate"] = BaseRate;


                    grid_MultiUOM.JSProperties["cpAltQty"] = AltQty;
                    grid_MultiUOM.JSProperties["cpAltUom"] = AltUom;
                    grid_MultiUOM.JSProperties["cpAltRate"] = AltRate;
                    grid_MultiUOM.JSProperties["cpUpdatedrow"] = UpdateRow;
                    grid_MultiUOM.JSProperties["cpuomid"] = AltUOMKeyValue;
                }
                Session["MultiUOMDataIND"] = dt;
            }



            else if (SpltCmmd == "UpdateRow")
            {

                string Validcheck = "";
                string SrlNoR = e.Parameters.Split('~')[1];
                string AltUOMKeyValue = e.Parameters.Split('~')[7];
                string AltUOMKeyqnty = e.Parameters.Split('~')[5];
                string muid = e.Parameters.Split('~')[12];
                string SrlNo = "0";
              
                DataTable MultiUOMSaveData = new DataTable();

                DataTable dt = (DataTable)Session["MultiUOMDataIND"];

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] MultiUoMresult = dt.Select("MultiUOMSR ='" + muid + "'");
                    foreach (DataRow item in MultiUoMresult)
                    {
                        SrlNo = Convert.ToString(item["SrlNo"]);
                    }
                }

                
                if (dt.Columns.Contains("DetailsID"))
                {
                    dt.Columns.Remove("DetailsID");
                }
                
                //string SrlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                
                string Quantity = Convert.ToString(e.Parameters.Split('~')[2]);
                string UOM = Convert.ToString(e.Parameters.Split('~')[3]);
                string AltUOM = Convert.ToString(e.Parameters.Split('~')[4]);
                string AltQuantity = Convert.ToString(e.Parameters.Split('~')[5]);
                string UomId = Convert.ToString(e.Parameters.Split('~')[6]);
                string AltUomId = Convert.ToString(e.Parameters.Split('~')[7]);
                string ProductId = Convert.ToString(e.Parameters.Split('~')[8]);

                string BaseRate = Convert.ToString(e.Parameters.Split('~')[9]);
                string AltRate = Convert.ToString(e.Parameters.Split('~')[10]);
                string UpdateRow = Convert.ToString(e.Parameters.Split('~')[11]);
               
                
                DataRow[] MultiUoMresultResult = dt.Select("SrlNo ='" + SrlNo + "' and  MultiUOMSR <>'" + muid + "'");

                foreach (DataRow item in MultiUoMresultResult)
                {
                    if ((AltUomId == item["AltUomId"].ToString()) || (UomId == item["AltUomId"].ToString()))
                    {
                        if (AltQuantity == item["AltQuantity"].ToString())
                        {
                            Validcheck = "DuplicateUOM";
                            grid_MultiUOM.JSProperties["cpDuplicateAltUOM"] = "DuplicateAltUOM";
                            break;
                        }
                    }
                    // [ if "Update Row" checkbox is checked, then all existing Update Row in the grid will be set to False]
                    if (UpdateRow == "True")
                    {
                        item["UpdateRow"] = "False";
                    }
                   
                }

                if (Validcheck != "DuplicateUOM")
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataRow[] MultiUoMresult = dt.Select("MultiUOMSR ='" + muid + "'");
                        foreach (DataRow item in MultiUoMresult)
                        {
                           
                            SrlNo = Convert.ToString(item["SrlNo"]);
                           
                            item.Table.Rows.Remove(item);
                            break;

                        }
                    }

                    dt.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, BaseRate, AltRate, UpdateRow, muid);
                }
               
                Session["MultiUOMDataIND"] = dt;
                MultiUOMSaveData = (DataTable)Session["MultiUOMDataIND"];

                MultiUOMSaveData.AcceptChanges();
                Session["MultiUOMDataIND"] = MultiUOMSaveData;

                if (MultiUOMSaveData != null && MultiUOMSaveData.Rows.Count > 0)
                {
                    DataView dvData = new DataView(MultiUOMSaveData);
                    // dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
    
                    dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                    grid_MultiUOM.DataSource = dvData;
                    grid_MultiUOM.DataBind();
                }



                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    DataView dvData = new DataView(dt);
                //    dvData.RowFilter = "SrlNo = '" + SrlNo + "'";

                //    grid_MultiUOM.DataSource = dvData;
                //    grid_MultiUOM.DataBind();
                //}
                //else
                //{
                //    //MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId);
                //    //Session["MultiUOMDataIND"] = MultiUOMSaveData;
                //    grid_MultiUOM.DataSource = dt.DefaultView;
                //    grid_MultiUOM.DataBind();
                //}

            }
          
            else if (SpltCmmd == "SetBaseQtyRateInGrid")
            {
                DataTable dt = new DataTable();

                if (Session["MultiUOMDataIND"] != null)
                {
                    string SrlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                    dt = (DataTable)HttpContext.Current.Session["MultiUOMDataIND"];
                    DataRow[] MultiUoMresult = dt.Select("SrlNo ='" + SrlNo + "' and UpdateRow ='True'");

                    Int64 SelNo = Convert.ToInt64(MultiUoMresult[0]["SrlNo"]);
                    Decimal BaseQty = Convert.ToDecimal(MultiUoMresult[0]["Quantity"]);
                    Decimal BaseRate = Convert.ToDecimal(MultiUoMresult[0]["BaseRate"]);

                    Decimal AltQty = Convert.ToDecimal(MultiUoMresult[0]["AltQuantity"]);
                    string AltUom = Convert.ToString(MultiUoMresult[0]["AltUOM"]);

                    grid_MultiUOM.JSProperties["cpSetBaseQtyRateInGrid"] = "1";
                    grid_MultiUOM.JSProperties["cpBaseQty"] = BaseQty;
                    grid_MultiUOM.JSProperties["cpBaseRate"] = BaseRate;


                    grid_MultiUOM.JSProperties["cpAltQty"] = AltQty;
                    grid_MultiUOM.JSProperties["cpAltUom"] = AltUom;


                    //if (Session["OrderDetails"] != null)
                    //{
                    //    DataTable SalesOrderdt = (DataTable)Session["OrderDetails"];

                    //    DataRow[] drSalesOrder = SalesOrderdt.Select("SrlNo ='" + SelNo + "'");
                    //    if (drSalesOrder.Length > 0)
                    //    {
                    //        drSalesOrder[0]["Quantity"] = BaseQty;
                    //        drSalesOrder[0]["SalePrice"] = BaseRate;
                    //    }

                    //}

                }
            }
            // End of Mantis Issue 24428


        }

        protected void gridBatch_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string command = e.Parameters.Split('~')[0];
            if (command == "Edit" || command == "View")
            {
                // int RowIndex = Convert.ToInt32(e.Parameters.Split('~')[1]);
                // string Indent_Id = Grid_PurchaseIndent.GetRowValues(RowIndex, "Indent_Id").ToString();
                string IndentId = Convert.ToString(e.Parameters.Split('~')[1]);
                //string Indent_Id = Grid_PurchaseIndent.GetRowValues(RowIndex, "Indent_Id").ToString();	
                string Indent_Id = IndentId;

                // ViewState["Indent_Id"] = Indent_Id;
                Session["Indent_Id"] = Indent_Id;
                Keyval_internalId.Value = "PurchaseIndent" + Indent_Id;

                DataTable PurchaseIndentEditdt = GetPurchaseIndentEditData();
                if (PurchaseIndentEditdt != null && PurchaseIndentEditdt.Rows.Count > 0)
                {
                    string Indent_RequisitionNumber = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Indent_RequisitionNumber"]);//0
                    string Indent_RequisitionDate = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Indent_RequisitionDate"]);//1
                    string Indent_BranchIdFor = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Indent_BranchIdFor"]);//2
                    string Indent_Purpose = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Indent_Purpose"]);//3
                    string Indent_CurrencyId = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Indent_CurrencyId"]);//4
                    string Indent_ExchangeRtae = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Indent_ExchangeRtae"]);//5
                    string Indent_ProjID = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Proj_Id"]);//6
                    // Mantis Issue 25235
                    string Indent_VendorId = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Vendor_Id"]);//7
                    string Indent_VendorName = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Vendor_Name"]);//7
                    // End of Mantis Issue 25235

                    string RevisionNo = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Indent_RevisionNo"]);
                    txtRevisionNo.Text = RevisionNo;
                    string ApproveProjectRem = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Indent_ApprovalRemarks"]);
                    txtAppRejRemarks.Text = ApproveProjectRem;
                    hdnApproveStatus.Value = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Indent_ApproveStatus"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(PurchaseIndentEditdt.Rows[0]["Indent_RevisionDate"])))
                    {
                        txtRevisionDate.Date = Convert.ToDateTime(PurchaseIndentEditdt.Rows[0]["Indent_RevisionDate"]);
                        txtRevisionDate.MinDate = Convert.ToDateTime(Convert.ToDateTime(PurchaseIndentEditdt.Rows[0]["Indent_RevisionDate"]).ToShortDateString());
                    }
                    else
                    {
                        txtRevisionDate.Date = Convert.ToDateTime(Indent_RequisitionDate);
                        txtRevisionDate.MinDate = Convert.ToDateTime(Convert.ToDateTime(Indent_RequisitionDate).ToShortDateString());
                    }

                    txtVoucherNo.Text = Indent_RequisitionNumber.Trim();
                    string ForBranch = Convert.ToString(PurchaseIndentEditdt.Rows[0]["ForBranch"]);
                    //string ProjectSelectEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
                    //lookup_Project.GridView.Selection.SelectRowByKey(Convert.ToInt64(PurchaseIndentEditdt.Rows[0]["Proj_Id"]));

                    string ForDate = Convert.ToString(PurchaseIndentEditdt.Rows[0]["FromDate"]);
                    string ToDate = Convert.ToString(PurchaseIndentEditdt.Rows[0]["ToDate"]);

                    #region Indent Tagging & Product Tagging
                    string Quoids = Convert.ToString(PurchaseIndentEditdt.Rows[0]["MRPIDs"]);
                    string ContactNos = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Order_Numbers"]);
                    string QuoComponent = "", QuoComponentNumber = "", QuoComponentDate = "", TempDetailIds="";
                    string ComponentDetailsIDs = "", ContactComponentNumber = "", ProductID = "";
                    string _ContactDetailIDs = Convert.ToString(PurchaseIndentEditdt.Rows[0]["MRPDetailsID"]);
                    if (!String.IsNullOrEmpty(Quoids) && Quoids.Split(',')[0] != "0")
                    {
                        
                        BindLookUp(Indent_RequisitionDate, Indent_BranchIdFor);
                        string[] eachQuo = Quoids.Split(',');
                        

                        for (int i = 0; i < taggingGrid.VisibleRowCount; i++)
                        {
                            string PurchaseOrder_Id = Convert.ToString(taggingGrid.GetRowValues(i, "MRP_ID"));
                            if (eachQuo.Contains(PurchaseOrder_Id))
                            {
                                QuoComponent += "," + Convert.ToString(taggingGrid.GetRowValues(i, "MRP_ID"));
                                QuoComponentNumber += "," + Convert.ToString(taggingGrid.GetRowValues(i, "MRP_No"));
                                QuoComponentDate += "," + Convert.ToString(taggingGrid.GetRowValues(i, "MRP_Date"));

                                taggingGrid.Selection.SelectRow(i);
                            }
                        }
                        QuoComponent = QuoComponent.TrimStart(',');
                        QuoComponentNumber = QuoComponentNumber.TrimStart(',');
                        QuoComponentDate = QuoComponentDate.TrimStart(',');

                        


                        //taggingList.Text = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Indent_Numbers"]);

                        
                        DataTable dtDetails = GetComponentProductList("GetOrderProducts", QuoComponent);
                        Session["SI_ProductDetails"] = dtDetails;
                        grid_Products.DataSource = dtDetails;
                        grid_Products.DataBind();

                       

                        if (!String.IsNullOrEmpty(_ContactDetailIDs) && _ContactDetailIDs.Split(',')[0] != "0")
                        {
                            string[] eachDetailID = _ContactDetailIDs.Split(',');
                           

                            for (int i = 0; i < grid_Products.VisibleRowCount; i++)
                            {
                                string DetailsID = Convert.ToString(grid_Products.GetRowValues(i, "ComponentDetailsID"));
                                if (eachDetailID.Contains(DetailsID))
                                {
                                    ComponentDetailsIDs += "," + Convert.ToString(grid_Products.GetRowValues(i,"ComponentDetailsID"));
                                    ProductID += "," + Convert.ToString(grid_Products.GetRowValues(i,"ProductID"));
                                    ContactComponentNumber += "," + Convert.ToString(grid_Products.GetRowValues(i,"ComponentNumber"));

                                    grid_Products.Selection.SelectRow(i);
                                }
                             }
                            ComponentDetailsIDs = ComponentDetailsIDs.TrimStart(',');
                            ProductID = ProductID.TrimStart(',');
                            ContactComponentNumber = ContactComponentNumber.TrimStart(',');


                            
                           

                            DataTable dtTempDetails = GetComponentTemplateProductList("GetSeletedTemplateProductsTaggedContract", ProductID);
                            Session["SI_TemplateProductDetails"] = dtTempDetails;
                            gridTemplateproducts.DataSource = dtTempDetails;
                            gridTemplateproducts.DataBind();


                            string ServiceTempDetailIds = Convert.ToString(PurchaseIndentEditdt.Rows[0]["ServiceTempDetailIds"]);

                            if (!String.IsNullOrEmpty(ServiceTempDetailIds) && ServiceTempDetailIds.Split(',')[0] != "0")
                            {
                                string[] eachTempID = ServiceTempDetailIds.Split(',');

                                for (int i = 0; i < gridTemplateproducts.VisibleRowCount; i++)
                                {
                                    string DetailsID = Convert.ToString(gridTemplateproducts.GetRowValues(i, "ComponentDetailsID"));
                                    if (eachTempID.Contains(DetailsID))
                                    {
                                        TempDetailIds += "," + Convert.ToString(gridTemplateproducts.GetRowValues(i, "ComponentDetailsID"));
                                        gridTemplateproducts.Selection.SelectRow(i);
                                    }
                                }
                                TempDetailIds = TempDetailIds.TrimStart(',');
                            }
                            


                        }

                        
                    }
                    #endregion





                    // Mantis Issue 25235 [ Indent_VendorId and Indent_VendorName added ]
                    gridBatch.JSProperties["cpEdit"] = Indent_RequisitionNumber + "~" + Indent_RequisitionDate + "~" + Indent_BranchIdFor + "~" + Indent_Purpose + "~" + Indent_CurrencyId
                        + "~" + Indent_ExchangeRtae + "~" + Indent_Id + "~" + Indent_ProjID + "~" + RevisionNo + "~" + ApproveProjectRem + "~" + ForBranch + "~" + ContactNos + "~" + QuoComponent
                        + "~" + ComponentDetailsIDs + "~" + TempDetailIds + "~" + ForDate + "~" + ToDate + "~" + QuoComponent + "~" + _ContactDetailIDs + "~" + Indent_VendorId + "~" + Indent_VendorName;
                    gridBatch.JSProperties["cpView"] = (command.ToUpper() == "VIEW") ? "1" : "0";
                }
                Session["MultiUOMDataIND"] = GetMultiUOMData();
                gridBatch.DataSource = BindBratchGrid();
                gridBatch.DataBind();

                Session["PurchaseIndateDetails"] = GetPurchaseIndentData().Tables[0];
                Session["InlineRemarks"] = GetPurchaseIndentData().Tables[1];
                #region Sandip Section For Approval To Show Hide Save Naw and Save Exit Button
                string result = "";
                result = IsExistsDocumentInERPDocApproveStatus(Indent_Id);
                if (result != "")
                {
                    if (result == "A")
                    {
                        gridBatch.JSProperties["cpApproval"] = "A";
                    }
                    else if (result == "R")
                    {
                        gridBatch.JSProperties["cpApproval"] = "R";
                    }
                }
                else
                {
                    gridBatch.JSProperties["cpApproval"] = null;
                }
                #endregion Sandip Section For Approval To Show Hide Save Naw and Save Exit Button
                if (IsPITransactionExist(Indent_Id) && hdnApprovalReqInq.Value != "1")
                {
                    gridBatch.JSProperties["cpBtnVisible"] = "false";
                }
                if (IsPIIndentTransactionExist(Indent_Id) && hdnApprovalReqInq.Value != "1")
                {
                    gridBatch.JSProperties["cpBtnVisible"] = "false";
                }

                //DataTable Quotationdt = (DataTable)Session["PurchaseIndateDetails"];
                // gridBatch.DataSource = GetPurchaseIndent(Quotationdt);
                // gridBatch.DataBind();
            }
            #region Sandip Section For Approval Section Start
            if (command == "ApprovalEdit")
            {
                int Indent_Id = Convert.ToInt32(e.Parameters.Split('~')[1]);

                //string Indent_Id = Grid_PurchaseIndent.GetRowValues(RowIndex, "Indent_Id").ToString();
                // ViewState["Indent_Id"] = Indent_Id;
                Session["Indent_Id"] = Indent_Id;

                DataTable PurchaseIndentEditdt = GetPurchaseIndentEditData();
                if (PurchaseIndentEditdt != null && PurchaseIndentEditdt.Rows.Count > 0)
                {
                    string Indent_RequisitionNumber = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Indent_RequisitionNumber"]);//0
                    string Indent_RequisitionDate = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Indent_RequisitionDate"]);//1
                    string Indent_BranchIdFor = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Indent_BranchIdFor"]);//2
                    string Indent_Purpose = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Indent_Purpose"]);//3
                    string Indent_CurrencyId = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Indent_CurrencyId"]);//4
                    string Indent_ExchangeRtae = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Indent_ExchangeRtae"]);//5
                    // Mantis Issue 25070
                    string Indent_ProjID = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Proj_Id"]);//6
                    // End of Mantis Issue 25070

                    string RevisionNo = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Indent_RevisionNo"]);
                    txtRevisionNo.Text = RevisionNo;
                    string ApproveProjectRem = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Indent_ApprovalRemarks"]);
                    txtAppRejRemarks.Text = ApproveProjectRem;
                    hdnApproveStatus.Value = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Indent_ApproveStatus"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(PurchaseIndentEditdt.Rows[0]["Indent_RevisionDate"])))
                    {
                        txtRevisionDate.Date = Convert.ToDateTime(PurchaseIndentEditdt.Rows[0]["Indent_RevisionDate"]);
                        txtRevisionDate.MinDate = Convert.ToDateTime(Convert.ToDateTime(PurchaseIndentEditdt.Rows[0]["Indent_RevisionDate"]).ToShortDateString());
                    }
                    else
                    {
                        txtRevisionDate.Date = Convert.ToDateTime(Indent_RequisitionDate);
                        txtRevisionDate.MinDate = Convert.ToDateTime(Convert.ToDateTime(Indent_RequisitionDate).ToShortDateString());
                    }
                    // Mantis Issue 25235
                    string Indent_VendorId = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Vendor_Id"]);//7
                    string Indent_VendorName = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Vendor_Name"]);//7
                    // End of Mantis Issue 25235
                    
                    txtVoucherNo.Text = Indent_RequisitionNumber.Trim();
                   // Mantis Issue 25070
                    //gridBatch.JSProperties["cpEdit"] = Indent_RequisitionNumber + "~" + Indent_RequisitionDate + "~" + Indent_BranchIdFor + "~" + Indent_Purpose + "~" + Indent_CurrencyId
                    //    + "~" + Indent_ExchangeRtae + "~" + Indent_Id + "~" + RevisionNo + "~" + ApproveProjectRem;


                    string ForBranch = Convert.ToString(PurchaseIndentEditdt.Rows[0]["ForBranch"]);
                    string ForDate = Convert.ToString(PurchaseIndentEditdt.Rows[0]["FromDate"]);
                    string ToDate = Convert.ToString(PurchaseIndentEditdt.Rows[0]["ToDate"]);
                    string Quoids = Convert.ToString(PurchaseIndentEditdt.Rows[0]["ContactIDs"]);
                    string ContactNos = Convert.ToString(PurchaseIndentEditdt.Rows[0]["Order_Numbers"]);
                    string QuoComponent = "", QuoComponentNumber = "", QuoComponentDate = "", TempDetailIds = "";
                    string ComponentDetailsIDs = "", ContactComponentNumber = "", ProductID = "";
                    // Mantis Issue 25235 [ existing issue ]
                    string _ContactDetailIDs = "";
                    // End of Mantis Issue 25235

                    // Mantis Issue 25235 [  Indent_VendorId and Indent_VendorName added]
                    gridBatch.JSProperties["cpEdit"] = Indent_RequisitionNumber + "~" + Indent_RequisitionDate + "~" + Indent_BranchIdFor + "~" + Indent_Purpose + "~" + Indent_CurrencyId
                       + "~" + Indent_ExchangeRtae + "~" + Indent_Id + "~" + Indent_ProjID + "~" + RevisionNo + "~" + ApproveProjectRem + "~" + ForBranch + "~" + ContactNos + "~" + QuoComponent
                       + "~" + ComponentDetailsIDs + "~" + TempDetailIds + "~" + ForDate + "~" + ToDate + "~" + QuoComponent + "~" + _ContactDetailIDs + "~" + Indent_VendorId + "~" + Indent_VendorName;
                    gridBatch.JSProperties["cpView"] = (command.ToUpper() == "VIEW") ? "1" : "0";
                    // End of Mantis Issue 25070

                }

                gridBatch.DataSource = BindBratchGrid();
                gridBatch.DataBind();

                Session["PurchaseIndateDetails"] = GetPurchaseIndentData().Tables[0];
                Session["InlineRemarks"] = GetPurchaseIndentData().Tables[1];
                //DataTable Quotationdt = (DataTable)Session["PurchaseIndateDetails"];
                // gridBatch.DataSource = GetPurchaseIndent(Quotationdt);
                // gridBatch.DataBind();
            }


            #endregion Sandip Section For Approval Section Start

            if (command == "Display")
            {
                string IsDeleteFrom = Convert.ToString(hdfIsDelete.Value);
                if (IsDeleteFrom == "D")
                {
                    DataTable Quotationdt = (DataTable)Session["PurchaseIndateDetails"];
                    gridBatch.DataSource = GetPurchaseIndent(Quotationdt);
                    gridBatch.DataBind();

                    gridBatch.JSProperties["cpAddNewRow"] = "AddNewRow";
                }
            }
            if (command == "BindInventoryProductsDetails")
            {

                string strAction = e.Parameters.Split('~')[1];
                string ComponentDetailsIDs = string.Empty;
                string ProductID = "";
                string ComponentNumber = "";
                if (grid_Products.GetSelectedFieldValues("ComponentDetailsID").Count > 0)
                {
                    for (int i = 0; i < grid_Products.GetSelectedFieldValues("ComponentDetailsID").Count; i++)
                    {
                        ComponentDetailsIDs += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("ComponentDetailsID")[i]);
                        ProductID += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("ProductID")[i]);
                        ComponentNumber += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("ComponentNumber")[i]);
                    }
                    ComponentDetailsIDs = ComponentDetailsIDs.TrimStart(',');
                    ProductID = ProductID.TrimStart(',');
                    ComponentNumber = ComponentNumber.TrimStart(',');

                   
                   // string[] array = ProductID.Split(',');

                   // var differentRecords = array
                   //.GroupBy(x => x)
                   //.Select(y => y.First());

                    //foreach (var item in differentRecords)
                    //{
                    //    gridBatch.JSProperties["cpRtnMsg"] = "Duplicate";
                    //    return;
                    //}

                    //if (array.Length>1)
                    //{                    
                    //    for(int i=0;i<array.Length-1;i++)
                    //    {
                    //        if(array[i]!=array[i+1])
                    //        {
                    //            gridBatch.JSProperties["cpRtnMsg"] = "Duplicate";
                    //            return;
                    //        }
                    //    }
                    //}

                    String QuoComponent = "", SerProductID="";
                    if (taggingGrid.GetSelectedFieldValues("MRP_ID").Count > 0)
                    {
                        for (int i = 0; i < taggingGrid.GetSelectedFieldValues("MRP_ID").Count; i++)
                        {
                            QuoComponent += "," + Convert.ToString(taggingGrid.GetSelectedFieldValues("MRP_ID")[i]);
                        }

                        QuoComponent = QuoComponent.TrimStart(',');
                    }

                    if (grid_Products.GetSelectedFieldValues("ProductID").Count > 0)
                    {
                        for (int i = 0; i < grid_Products.GetSelectedFieldValues("ProductID").Count; i++)
                        {
                            SerProductID += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("ProductID")[i]);
                        }

                        SerProductID = SerProductID.TrimStart(',');
                    }




                 
                   
                 
                    //strAction = "BindGridMRPProducts";

                    DataTable dtDetails = GetBindProductList(strAction, ComponentDetailsIDs, QuoComponent ,SerProductID);
                    Session["PurchaseIndateDetails"] = dtDetails;
                    gridBatch.DataSource = BindBratchGrid(dtDetails);
                    gridBatch.DataBind();
                }
            }

        }

        protected void BindLookUp(string OrderDate, string BranchId)
        {   
            string strBranch = Convert.ToString(Session["userbranchHierarchy"]);
            DataTable IndentTable = new DataTable();

            //IndentTable = GetContractData(OrderDate, BranchId);
            IndentTable = GetMRP(OrderDate, BranchId);
            Session["IndentRequiData"] = IndentTable;
            if (IndentTable.Rows.Count > 0)
            {
                taggingGrid.DataSource = IndentTable;
                taggingGrid.DataBind();
            }
            else
            {
                taggingGrid.DataSource = IndentTable;
                taggingGrid.DataBind();
            }
           
        }
        public IEnumerable BindBratchGrid(DataTable PurchaseIndentdt)
        {
            DataSet DsOnLoad = new DataSet();
            DataTable tempdt = new DataTable();
            DBEngine objEngine = new DBEngine();
            List<BratchGridLIST> BratchGridLists = new List<BratchGridLIST>();
            // DataTable PurchaseIndentdt = GetPurchaseIndentData().Tables[0];
            for (int i = 0; i < PurchaseIndentdt.Rows.Count; i++)
            {
                BratchGridLIST BratchGrid_LIST = new BratchGridLIST();
                BratchGrid_LIST.SrlNo = Convert.ToString(PurchaseIndentdt.Rows[i]["SrlNo"]);//1
                BratchGrid_LIST.PurchaseIndentID = Convert.ToString(PurchaseIndentdt.Rows[i]["IndentDetailsId"]);//2
                //Rev 1.0 Subhra 01.03.2019
                BratchGrid_LIST.gvColIndentDetailsId = Convert.ToString(PurchaseIndentdt.Rows[i]["IndentDetailsId"]);//2
                //End of Rev
                BratchGrid_LIST.gvColProduct = Convert.ToString(PurchaseIndentdt.Rows[i]["ProductID"]);//3
                BratchGrid_LIST.gvColDiscription = Convert.ToString(PurchaseIndentdt.Rows[i]["Description"]);//4
                BratchGrid_LIST.gvColQuantity = Convert.ToString(PurchaseIndentdt.Rows[i]["Quantity"]);//5
                BratchGrid_LIST.gvColUOM = Convert.ToString(PurchaseIndentdt.Rows[i]["UOM"]);//6
                BratchGrid_LIST.gvColRate = Convert.ToString(PurchaseIndentdt.Rows[i]["Rate"]);//7

                BratchGrid_LIST.gvColValue = Convert.ToString(PurchaseIndentdt.Rows[i]["ValueInBaseCurrency"]);//9
                string ExpectedDeliveryDate = Convert.ToString(PurchaseIndentdt.Rows[i]["ExpectedDeliveryDate"]);
                if (!String.IsNullOrEmpty(ExpectedDeliveryDate))
                {
                    BratchGrid_LIST.ExpectedDeliveryDate = Convert.ToDateTime(PurchaseIndentdt.Rows[i]["ExpectedDeliveryDate"]);//8
                }
                else
                {
                    BratchGrid_LIST.ExpectedDeliveryDate = null;
                }
                BratchGrid_LIST.Status = Convert.ToString(PurchaseIndentdt.Rows[i]["Status"]);
                BratchGrid_LIST.AvailableStock = Convert.ToString(PurchaseIndentdt.Rows[i]["AvailableStock"]);//11
                BratchGrid_LIST.ProductName = Convert.ToString(PurchaseIndentdt.Rows[i]["ProductName"]);//12
                if (PurchaseIndentdt.Columns.Contains("Remarks"))
                {
                    BratchGrid_LIST.Remarks = Convert.ToString(PurchaseIndentdt.Rows[i]["Remarks"]);
                }
                else
                {
                    BratchGrid_LIST.Remarks = "";
                }

                BratchGrid_LIST.ServiceTempDetails_ID = Convert.ToString(PurchaseIndentdt.Rows[i]["ServiceTempDetails_ID"]);
                BratchGrid_LIST.ServiceTemplate_ID = Convert.ToString(PurchaseIndentdt.Rows[i]["ServiceTemplate_ID"]);

                BratchGrid_LIST.Order_AltQuantity = Convert.ToString(PurchaseIndentdt.Rows[i]["Order_AltQuantity"]);
                BratchGrid_LIST.Order_AltUOM = Convert.ToString(PurchaseIndentdt.Rows[i]["Order_AltUOM"]);
                BratchGridLists.Add(BratchGrid_LIST);
            }
            return BratchGridLists;
        }
        //public DataTable GetBindProductList(string Action, string ComponentList, string SOList, string FromDate, string ToDate, string ProductID)
        //{
        //    DataTable dt = new DataTable();
        //    ProcedureExecute proc = new ProcedureExecute("prc_PurchaseIndentDetailsList");
        //    proc.AddVarcharPara("@Action", 500, Action);
        //    proc.AddVarcharPara("@ComponentList", 2000, ComponentList);
        //    proc.AddVarcharPara("@SOList", 2000, SOList);
        //    proc.AddVarcharPara("@FromDate", 50, FromDate);
        //    proc.AddVarcharPara("@ToDate", 50, ToDate);
        //    proc.AddVarcharPara("@ProductIDList", 50, ProductID);
        //    dt = proc.GetTable();
        //    return dt;
        //}

        public DataTable GetBindProductList(string Action, string ComponentList, string SOList, string ProductID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseIndentDetailsList");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@ComponentList", 2000, ComponentList);
            proc.AddVarcharPara("@SOList", 2000, SOList);
           
            proc.AddVarcharPara("@ProductIDList", 50, ProductID);
            dt = proc.GetTable();
            return dt;
        }
        private bool IsPITransactionExist(string PIid)
        {
            bool IsExist = false;
            if (PIid != "" && Convert.ToString(PIid).Trim() != "")
            {
                DataTable dt = new DataTable();
                dt = objPurchaseIndentBL.CheckPITraanaction(PIid);
                if (dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0]["isexist"]) != "0")
                {
                    IsExist = true;
                }
            }

            return IsExist;
        }

        private bool IsPIIndentTransactionExist(string PIid)
        {
            bool IsExist = false;
            if (PIid != "" && Convert.ToString(PIid).Trim() != "")
            {
                DataTable dt = new DataTable();
                dt = CheckPIIndentTraanaction(PIid);
                if (dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0]["isexist"]) != "0")
                {
                    IsExist = true;
                }
            }
            return IsExist;
        }
        public DataTable CheckPIIndentTraanaction(string piid)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseIndentDetailsList");
            proc.AddVarcharPara("@Action", 100, "PurchaseIndentTagOrNotInPurchaseQuotation");
            proc.AddVarcharPara("@Indent_Id", 200, piid);
            dt = proc.GetTable();
            return dt;
        }

        protected void Grid_PurchaseIndent_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string command = e.Parameters.Split('~')[0];
            Grid_PurchaseIndent.JSProperties["cpDelete"] = null;
            string PurIndentID = null;
            if (Convert.ToString(e.Parameters).Contains("~"))
            {
                if (Convert.ToString(e.Parameters).Split('~')[1] != "")
                {
                    //PurIndentID = Convert.ToString(e.Parameters).Split('~')[1];
                    int RowIndex = Convert.ToInt32(e.Parameters.Split('~')[1]);
                    PurIndentID = Convert.ToString(RowIndex);

                    string RowIndexValue = Convert.ToString(e.Parameters.Split('~')[1]);
                    //PurIndentID = Grid_PurchaseIndent.GetRowValues(RowIndex, "Indent_Id").ToString();	
                    PurIndentID = RowIndexValue;
                }
            }
            if (command == "Delete")
            {
                int istrans = 0;
                if (IsPITransactionExist(PurIndentID) == false && IsPIIndentTransactionExist(PurIndentID) == false)
                {
                    istrans = 1;
                    string RowIndexVal = Convert.ToString(e.Parameters.Split('~')[1]);
                    //string Indent_Id = Grid_PurchaseIndent.GetRowValues(RowIndex, "Indent_Id").ToString();	
                    string Indent_Id = RowIndexVal;
                    // ViewState["Indent_Id"] = Indent_Id;	
                    //Session["Indent_Id"] = Indent_Id;	
                    int val = GetPurchaseIndentDeleteData(Indent_Id);
                    if (val == 1)
                    {
                        Grid_PurchaseIndent.JSProperties["cpDelete"] = "Succesfully Deleted";
                    }
                }
                //else if (!IsPIIndentTransactionExist(PurIndentID) && istrans==1)
                //{
                //    string RowIndexVal = Convert.ToString(e.Parameters.Split('~')[1]);
                //    //string Indent_Id = Grid_PurchaseIndent.GetRowValues(RowIndex, "Indent_Id").ToString();	
                //    string Indent_Id = RowIndexVal;
                //    // ViewState["Indent_Id"] = Indent_Id;
                //    //Session["Indent_Id"] = Indent_Id;
                //    int val = GetPurchaseIndentDeleteData(Indent_Id);
                //    if (val == 1)
                //    {
                //        Grid_PurchaseIndent.JSProperties["cpDelete"] = "Succesfully Deleted";
                //    }
                //}
                else
                {

                    Grid_PurchaseIndent.JSProperties["cpDelete"] = "Transaction exist. Cannot Delete.";
                }

            }
            //FillGrid();
        }
        public int GetPurchaseIndentDeleteData(string Indent_Id)
        {
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseIndentDetailsList");
            proc.AddVarcharPara("@Action", 500, "PurchaseIndentDeleteDetails");
            // Int32 ID = Convert.ToInt32(ViewState["Indent_Id"]);
            //Int32 ID = Convert.ToInt32(Session["Indent_Id"]);
            Int32 ID = Convert.ToInt32(Indent_Id);
            proc.AddIntegerPara("@Indent_Id", ID);
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }
        public IEnumerable BindBratchGrid()
        {
            DataSet DsOnLoad = new DataSet();
            DataTable tempdt = new DataTable();
            DBEngine objEngine = new DBEngine();
            List<BratchGridLIST> BratchGridLists = new List<BratchGridLIST>();
            DataTable PurchaseIndentdt = GetPurchaseIndentData().Tables[0];
            for (int i = 0; i < PurchaseIndentdt.Rows.Count; i++)
            {
                BratchGridLIST BratchGrid_LIST = new BratchGridLIST();
                BratchGrid_LIST.SrlNo = Convert.ToString(PurchaseIndentdt.Rows[i]["SrlNo"]);//1
                BratchGrid_LIST.PurchaseIndentID = Convert.ToString(PurchaseIndentdt.Rows[i]["IndentDetailsId"]);//2
                //Rev 1.0 Subhra 01.03.2019
                BratchGrid_LIST.gvColIndentDetailsId = Convert.ToString(PurchaseIndentdt.Rows[i]["IndentDetailsId"]);//2
                //End of Rev
                BratchGrid_LIST.gvColProduct = Convert.ToString(PurchaseIndentdt.Rows[i]["ProductID"]);//3
                BratchGrid_LIST.gvColDiscription = Convert.ToString(PurchaseIndentdt.Rows[i]["Description"]);//4
                BratchGrid_LIST.gvColQuantity = Convert.ToString(PurchaseIndentdt.Rows[i]["Quantity"]);//5
                BratchGrid_LIST.gvColUOM = Convert.ToString(PurchaseIndentdt.Rows[i]["UOM"]);//6
                BratchGrid_LIST.gvColRate = Convert.ToString(PurchaseIndentdt.Rows[i]["Rate"]);//7

                BratchGrid_LIST.gvColValue = Convert.ToString(PurchaseIndentdt.Rows[i]["ValueInBaseCurrency"]);//9
                string ExpectedDeliveryDate = Convert.ToString(PurchaseIndentdt.Rows[i]["ExpectedDeliveryDate"]);
                if (!String.IsNullOrEmpty(ExpectedDeliveryDate))
                {
                    BratchGrid_LIST.ExpectedDeliveryDate = Convert.ToDateTime(PurchaseIndentdt.Rows[i]["ExpectedDeliveryDate"]);//8
                }
                else
                {
                    BratchGrid_LIST.ExpectedDeliveryDate = null;
                }
                BratchGrid_LIST.Status = Convert.ToString(PurchaseIndentdt.Rows[i]["Status"]);
                BratchGrid_LIST.AvailableStock = Convert.ToString(PurchaseIndentdt.Rows[i]["AvailableStock"]);//11
                BratchGrid_LIST.ProductName = Convert.ToString(PurchaseIndentdt.Rows[i]["ProductName"]);//12
                if (PurchaseIndentdt.Columns.Contains("Remarks"))
                {
                    BratchGrid_LIST.Remarks = Convert.ToString(PurchaseIndentdt.Rows[i]["Remarks"]);
                }
                else
                {
                    BratchGrid_LIST.Remarks = "";
                }
                BratchGrid_LIST.ServiceTempDetails_ID = Convert.ToString(PurchaseIndentdt.Rows[i]["ServiceTempDetails_ID"]);
                BratchGrid_LIST.ServiceTemplate_ID = Convert.ToString(PurchaseIndentdt.Rows[i]["ServiceTemplate_ID"]);

                // Mantis Issue 24428
                BratchGrid_LIST.Order_AltQuantity = Convert.ToString(PurchaseIndentdt.Rows[i]["Order_AltQuantity"]);
                BratchGrid_LIST.Order_AltUOM = Convert.ToString(PurchaseIndentdt.Rows[i]["Order_AltUOM"]);
                // End of Mantis Issue 24428

                BratchGridLists.Add(BratchGrid_LIST);
            }
            return BratchGridLists;
        }


        public DataTable GetMultiUOMData()
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseIndentDetailsList");
            // Rev Mantis Issue 24428/24429
            //proc.AddVarcharPara("@Action", 500, "MultiUOMQuotationDetails");
            proc.AddVarcharPara("@Action", 500, "MultiUOMQuotationDetails_New");
            // End of Rev Mantis Issue 24428/24429
            // proc.AddVarcharPara("@ChallanID", 500, Convert.ToString(Session["Indent_Id"]));
            proc.AddIntegerPara("@Indent_Id", Convert.ToInt32(Session["Indent_Id"]));
            ds = proc.GetTable();
            return ds;
        }
        public DataSet GetPurchaseIndentData()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseIndentDetailsList");
            // Rev Mantis Issue 24428/24429
            //proc.AddVarcharPara("@Action", 500, "BindBratchGridPurChaseIndent");
            proc.AddVarcharPara("@Action", 500, "BindBratchGridPurChaseIndent_New");
            // End of Rev Mantis Issue 24428/24429
            //proc.AddIntegerPara("@Indent_Id", Convert.ToInt32(ViewState["Indent_Id"]));
            proc.AddIntegerPara("@Indent_Id", Convert.ToInt32(Session["Indent_Id"]));
            ds = proc.GetDataSet();
            return ds;
        }
        public DataTable GetPurchaseIndentEditData()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseIndentDetailsList");
            proc.AddVarcharPara("@Action", 500, "PurchaseIndentEditDetails");
            //proc.AddIntegerPara("@Indent_Id", Convert.ToInt32(ViewState["Indent_Id"]));
            proc.AddIntegerPara("@Indent_Id", Convert.ToInt32(Session["Indent_Id"]));
            //proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            //proc.AddVarcharPara("@branchId", 500, Convert.ToString(Session["userbranchID"]));
            //proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            dt = proc.GetTable();
            return dt;
        }

        public static DataTable ApproveRejectProjectStatus(string Id)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseIndentDetailsList");
            proc.AddVarcharPara("@Action", 200, "GetApproveDetails");
            proc.AddIntegerPara("@Indent_Id", Convert.ToInt32(Id));
            dt = proc.GetTable();
            return dt;
        }
        protected void Grid_PurchaseIndent_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            if (!rights.CanDelete)
            {
                if (e.ButtonID == "CustomBtnDelete")
                {
                    e.Visible = DevExpress.Utils.DefaultBoolean.False;
                }
            }


            if (!rights.CanEdit)
            {
                if (e.ButtonID == "CustomBtnEdit")
                {
                    e.Visible = DevExpress.Utils.DefaultBoolean.False;
                }
            }
            if (!rights.CanView)
            {
                if (e.ButtonID == "CustomBtnView")
                {
                    e.Visible = DevExpress.Utils.DefaultBoolean.False;
                }
            }
            if (e.ButtonID == "CustomBtnApprove")
            {
                if (!rights.CanApproved || !isApprove)
                {
                    e.Visible = DevExpress.Utils.DefaultBoolean.False;
                }
                //else if (!rights.CanApproved && !isApprove)	
                //{	
                //    e.Visible = DevExpress.Utils.DefaultBoolean.False;	
                //}	

            }
            if (!rights.CanPrint)
            {
                if (e.ButtonID == "CustomBtnPrint")
                {
                    e.Visible = DevExpress.Utils.DefaultBoolean.False;
                }
            }
        }
        [WebMethod]
        public static String getAvilableStock(string ProductID, string LastFinYear, string Campany_ID, string BranchFor)
        {


            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();



            //            string query = @"select  (Stock_Open+Stock_ID)-(ISNULL(Stock_Out,0))as Available_stock,
            //            (select UOM_Name from Master_UOM where UOM_ID=Stock_QuantityUnit)as UOM_Name
            //            from Trans_Stock where Stock_ProductID='" + ProductID + "' and Stock_Company='" + Campany_ID + "' and Stock_FinYear='" + LastFinYear + "'";
            //            DataTable dt = oDBEngine.GetDataTable(query);
            string Available_Stock = "", UOM_Name = "";

            DataTable dt2 = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableQuotation(" + BranchFor + ",'" + Convert.ToString(Campany_ID) + "','" + Convert.ToString(LastFinYear) + "'," + ProductID + ") as branchopenstock");

            if (dt2.Rows.Count > 0)
            {
                Available_Stock = Convert.ToString(Math.Round(Convert.ToDecimal(dt2.Rows[0]["branchopenstock"]), 2));
                return Available_Stock;
            }
            else
            {
                Available_Stock = "0.00";
                return Available_Stock;
            }

            //            if (dt != null && dt.Rows.Count > 0)
            //            {
            //                Available_Stock = Convert.ToString(dt.Rows[0]["Available_stock"]);
            //                UOM_Name = Convert.ToString(dt.Rows[0]["UOM_Name"]);
            //            }
            //            return Available_Stock + " " + UOM_Name;
        }

        #region Sandip Section For Approval Section Start

        #region Approval Waiting or Pending User Level Wise Section Start

        public void GetQuantityBaseOnProductforDetailsId(string strSrlNo, ref decimal strUOMQuantity)
        {
            decimal sum = 0;

            DataTable MultiUOMData = new DataTable();
            if (Session["MultiUOMDataIND"] != null)
            {
                MultiUOMData = (DataTable)Session["MultiUOMDataIND"];
                for (int i = 0; i < MultiUOMData.Rows.Count; i++)
                {
                    DataRow dr = MultiUOMData.Rows[i];
                    string UomSRlNo = Convert.ToString(dr["SrlNo"]);

                    if (strSrlNo == UomSRlNo)
                    {
                        string strQuantity = (Convert.ToString(dr["Quantity"]) != "") ? Convert.ToString(dr["Quantity"]) : "0";
                        var weight = Decimal.Parse(Regex.Match(strQuantity, "[0-9]*\\.*[0-9]*").Value);

                        sum = Convert.ToDecimal(weight);
                    }
                }
            }

            strUOMQuantity = sum;

        }
        public void PopulateERPDocApprovalPendingListByUserLevel() // Checked and Modified By Sandip
        {
            DataTable dtdata = new DataTable();
            if (Session["userid"] != null)
            {
                if (Session["userbranchID"] != null)
                {
                    int userid = 0;
                    userid = Convert.ToInt32(Session["userid"]);

                    dtdata = objERPDocPendingApproval.PopulateERPDocApprovalPendingListByUserLevel(userid, "PI");
                    if (dtdata != null && dtdata.Rows.Count > 0)
                    {
                        gridPendingApproval.DataSource = dtdata;
                        gridPendingApproval.DataBind();
                        Session["INPendingApproval"] = dtdata;  // Commented For Temporary Purpose
                    }
                    else
                    {
                        gridPendingApproval.DataSource = null;
                        gridPendingApproval.DataBind();
                    }
                }
            }
        }

        public void PopulateApprovalPendingCountByUserLevel()  // Checked and Modified By Sandip 
        {
            int userid = 0;
            if (Session["userid"] != null)
            {
                if (Session["userbranchID"] != null)
                {

                    userid = Convert.ToInt32(Session["userid"]);
                }
            }
            DataTable dtdata = new DataTable();
            dtdata = objERPDocPendingApproval.PopulateERPDocApprovalPendingCountByUserLevel(userid, "PI");
            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                lblWaiting.Text = "(" + Convert.ToString(dtdata.Rows[0]["ID"]) + ")";
            }
            else
            {
                lblWaiting.Text = "";
            }
        }


        protected void gridPendingApproval_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e) // Checked and Modified By Sandip
        {
            gridPendingApproval.JSProperties["cpinsert"] = null;
            gridPendingApproval.JSProperties["cpEdit"] = null;
            gridPendingApproval.JSProperties["cpUpdate"] = null;
            gridPendingApproval.JSProperties["cpDelete"] = null;
            gridPendingApproval.JSProperties["cpExists"] = null;
            gridPendingApproval.JSProperties["cpUpdateValid"] = null;
            int userid = 0;
            if (Session["userid"] != null)
            {
                Session.Remove("INPendingApproval");
                userid = Convert.ToInt32(Session["userid"]);
                PopulateERPDocApprovalPendingListByUserLevel();
                gridPendingApproval.JSProperties["cpEdit"] = "F";
                Session.Remove("INUserWiseERPDocCreation");
            }
            if (Session["KeyValue"] != null)
            {
                Session.Remove("KeyValue");
            }

        }

        protected void chkapprove_Init(object sender, EventArgs e)  // Checked and Modified By Sandip
        {
            ASPxCheckBox Dcheckbox = (ASPxCheckBox)sender;
            int itemindex = (((ASPxCheckBox)sender).NamingContainer as GridViewDataItemTemplateContainer).ItemIndex;
            KeyValue = Convert.ToInt32((((ASPxCheckBox)sender).NamingContainer as GridViewDataItemTemplateContainer).KeyValue);
            Session["KeyValue"] = KeyValue;
            Dcheckbox.ClientSideEvents.CheckedChanged = String.Format("function(s, e) {{ GetApprovedQuoteId(s, e, {0}) }}", itemindex);

        }


        protected void chkreject_Init(object sender, EventArgs e) // Checked and Modified By Sandip
        {
            ASPxCheckBox Dcheckbox = (ASPxCheckBox)sender;
            int itemindex = (((ASPxCheckBox)sender).NamingContainer as GridViewDataItemTemplateContainer).ItemIndex;
            KeyValue = Convert.ToInt32((((ASPxCheckBox)sender).NamingContainer as GridViewDataItemTemplateContainer).KeyValue);
            Session["KeyValue"] = KeyValue;
            Dcheckbox.ClientSideEvents.CheckedChanged = String.Format("function(s, e) {{ GetRejectedQuoteId(s, e, {0}) }}", itemindex);

        }

        #endregion Approval Waiting or Pending User Level Wise Section End
        #region Created User Wise List Quotation after Clicking on Status Button Section Start  (call in page load)

        protected void gridUserWiseQuotation_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            PopulateUserWiseERPDocCreation();
        }
        public void PopulateUserWiseERPDocCreation()
        {
            int userid = 0;
            if (Session["userid"] != null)
            {
                if (Session["userbranchID"] != null)
                {
                    userid = Convert.ToInt32(Session["userid"]);
                }
            }
            DataTable dtdata = new DataTable();
            if (Session["INUserWiseERPDocCreation"] == null)
            {

                dtdata = objERPDocPendingApproval.PopulateUserWiseERPDocCreation(userid, "PI");
            }
            else
            {
                dtdata = (DataTable)Session["INUserWiseERPDocCreation"];
            }
            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                gridUserWiseQuotation.DataSource = dtdata;
                gridUserWiseQuotation.DataBind();
                Session["INUserWiseERPDocCreation"] = dtdata;
            }
            else
            {
                gridUserWiseQuotation.DataSource = null;
                gridUserWiseQuotation.DataBind();
            }

        }
        #endregion #region Created User Wise List Quotation after Clicking on Status Button Section End


        #region To Show Hide Status and Pending Approval Button Configuration Wise Start
        public void ConditionWiseShowStatusButton()
        {
            int i = 0;
            int j = 0;
            int k = 0;
            int branchid = 0;
            if (Session["userbranchID"] != null)
            {
                branchid = Convert.ToInt32(Session["userbranchID"]);
            }
            //Session["userbranchHierarchy"])

            #region Sam Section For Showing Status and Approval waiting Button on 22052017
            j = objERPDocPendingApproval.ConditionWiseShowApprovalStatusButton(6, Convert.ToString(Session["userbranchHierarchy"]), Convert.ToString(Session["userid"]), "PI");

            if (j == 1)
            {
                spanStatus.Visible = true;
            }
            else
            {
                spanStatus.Visible = false;
            }


            k = objERPDocPendingApproval.ConditionWiseShowApprovalPendingButton(6, Convert.ToString(Session["userbranchHierarchy"]), Convert.ToString(Session["userid"]), "PI");

            if (k == 1)
            {
                divPendingWaiting.Visible = true;
            }
            else
            {
                divPendingWaiting.Visible = false;
            }



            #endregion Sam Section For Showing Status and Approval waiting Button on 22052017
            // Cross Branch Section by Sam on 10052017 Start  
            //i = objERPDocPendingApproval.ConditionWiseShowApprovalDtlStatusButton(8, branchid, Convert.ToString(Session["userid"]), "PB");  // Entity Id 8 For Purchase Invoice
            ////i = objERPDocPendingApproval.ConditionWiseShowApprovalDtlStatusButton(8, branchid, Convert.ToString(Session["userid"]), "PB");  // 7 for Purchase Order Module 
            ////i = objERPDocPendingApproval.ConditionWiseShowStatusButton(8, branchid, Convert.ToString(Session["userid"])); //Entity Id 8 For Purchase Invoice
            //// Cross Branch Section by Sam on 10052017 End 
            //if (i == 1)
            //{
            //    spanStatus.Visible = true;
            //    divPendingWaiting.Visible = true;
            //}
            //else if (i == 2)
            //{
            //    spanStatus.Visible = false;
            //    divPendingWaiting.Visible = true;
            //}
            //else
            //{
            //    spanStatus.Visible = false;
            //    divPendingWaiting.Visible = false;
            //}
        }

        #endregion To Show Hide Status and Pending Approval Button Configuration Wise End

        //#region To Show Hide Status and Pending Approval Button Configuration Wise Start
        //public void ConditionWiseShowStatusButton()
        //{
        //    int i = 0;
        //    int branchid = 0;
        //    if (Session["userbranchID"] != null)
        //    {
        //        branchid = Convert.ToInt32(Session["userbranchID"]);
        //    }
        //    // Cross Branch Section by Sam on 10052017 Start  
        //    i = objERPDocPendingApproval.ConditionWiseShowApprovalDtlStatusButton(6, branchid, Convert.ToString(Session["userid"]), "PI");  // 7 for Purchase Order Module 
        //    //i = objERPDocPendingApproval.ConditionWiseShowStatusButton(7, branchid, Convert.ToString(Session["userid"]));  // 7 for Purchase Order Module 
        //    // Cross Branch Section by Sam on 10052017 End 
        //    //i = objERPDocPendingApproval.ConditionWiseShowStatusButton(6, branchid, Convert.ToString(Session["userid"]));  // 6 for Purchase Indent Module 
        //    if (i == 1)
        //    {
        //        spanStatus.Visible = true;
        //        divPendingWaiting.Visible = true;
        //    }
        //    else if (i == 2)
        //    {
        //        spanStatus.Visible = false;
        //        divPendingWaiting.Visible = true;
        //    }
        //    else 
        //    {
        //        spanStatus.Visible = false;
        //        divPendingWaiting.Visible = false;
        //    }
        //}

        //#endregion To Show Hide Status and Pending Approval Button Configuration Wise End

        #region After Approval Or rejected Number to reflect of Pending Approval Section  Start

        [WebMethod]
        public static string GetPendingCase()
        {
            string strPending = "(0)";

            ERPDocPendingApprovalBL objERPDocPendingApproval = new ERPDocPendingApprovalBL();
            int userid = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            //int userlevel = objCRMSalesDtlBL.GetUserLevelByUserID(userid);

            DataTable dtdata = new DataTable();
            dtdata = objERPDocPendingApproval.PopulateERPDocApprovalPendingCountByUserLevel(userid, "PI");
            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                strPending = "(" + Convert.ToString(dtdata.Rows[0]["ID"]) + ")";
            }

            return strPending;
        }

        #endregion After Approval Or rejected Number to reflect of Pending Approval Section  End

        public string IsExistsDocumentInERPDocApproveStatus(string id)
        {
            string result = "";

            string editable = "";
            string status = "";
            DataTable dt = new DataTable();
            int quoteid = Convert.ToInt32(id);
            dt = objERPDocPendingApproval.IsExistsDocumentInERPDocApproveStatus(quoteid, 6);  // 6 For Purchase Indent
            if (dt.Rows.Count > 0)
            {
                editable = Convert.ToString(dt.Rows[0]["editable"]);
                if (editable == "0")
                {

                    lbl_quotestatusmsg.Visible = true;
                    status = Convert.ToString(dt.Rows[0]["Status"]);
                    if (status == "Approved")
                    {
                        result = "A";
                        lbl_quotestatusmsg.Text = "Document already Approved.";

                    }
                    if (status == "Rejected")
                    {
                        result = "R";
                        lbl_quotestatusmsg.Text = "Document already Rejected.";

                    }
                    btnnew.Visible = false;
                    btnSaveExit.Visible = false;
                }
                else
                {
                    result = "X";
                    lbl_quotestatusmsg.Visible = false;
                    // btnnew.Visible = true;
                    // btnSaveExit.Visible = true;
                }
                return result;
            }
            else
            {
                return result;
            }
        }

        #endregion Sandip Section For Approval Dtl Section End
        protected void acpAvailableStock_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string performpara = e.Parameter;
            string strProductID = Convert.ToString(performpara.Split('~')[0]);
            string strBranch = Convert.ToString(ddlBranch.SelectedValue);

            acpAvailableStock.JSProperties["cpstock"] = "0.00";

            try
            {

                DataTable dt2 = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableQuotation(" + strBranch + ",'" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]) + "'," + strProductID + ") as branchopenstock");

                if (dt2.Rows.Count > 0)
                {
                    acpAvailableStock.JSProperties["cpstock"] = Convert.ToString(Math.Round(Convert.ToDecimal(dt2.Rows[0]["branchopenstock"]), 2));
                }
                else
                {
                    acpAvailableStock.JSProperties["cpstock"] = "0.00";
                }
            }
            catch (Exception ex)
            {
            }
        }
        protected void acbpCrpUdf_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (hdn_Mode.Value == "Entry")
            {
                if (reCat.isAllMandetoryDone((DataTable)Session["UdfDataOnAdd"], "PI") == false)
                {
                    acbpCrpUdf.JSProperties["cpUDFPI"] = "false";

                }
                else
                {
                    acbpCrpUdf.JSProperties["cpUDFPI"] = "true";
                }
            }
            else
            {
                acbpCrpUdf.JSProperties["cpUDFPI"] = "true";
            }
        }



        protected void gridPendingApproval_PageIndexChanged(object sender, EventArgs e)
        {
            PopulateERPDocApprovalPendingListByUserLevel();
        }


        public void FillPOLIstGrid(string Indent_Id)
        {

            DataTable dtdata = GetPOListGridData(Indent_Id);


            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                gridPOLIst.DataSource = dtdata;
                gridPOLIst.DataBind();
            }
            else
            {
                gridPOLIst.DataSource = null;
                gridPOLIst.DataBind();
            }
        }
        public DataTable GetPOListGridData(string Indent_Id)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseIndentDetailsList");
            proc.AddVarcharPara("@Action", 500, "GetPurchaseOrderListByIndentID");
            proc.AddVarcharPara("@CampanyID", 500, Convert.ToString(Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            //proc.AddBigIntegerPara("@Indent_Id",Convert.ToInt32(Indent_Id));
            proc.AddBigIntegerPara("@Indent_Id", Convert.ToInt32(Session["IndentIdPO"]));

            dt = proc.GetTable();
            return dt;
        }
        protected void gridPOLIst_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string Indent_Id = Convert.ToString(e.Parameters.Split('~')[1]);
            // string Indent_Id = Grid_PurchaseIndent.GetRowValues(RowIndex, "Indent_Id").ToString();
            Session["IndentIdPO"] = Indent_Id;
            string command = e.Parameters.Split('~')[0];
            if (command == "BindPOLIst")
            {
                FillPOLIstGrid(Indent_Id);
            }

        }

        protected void gridPOLIst_DataBinding(object sender, EventArgs e)
        {

            DataTable dtdata = GetPOListGridData("0");
            if (dtdata != null && dtdata.Rows.Count > 0)
            {
                gridPOLIst.DataSource = dtdata;
            }

        }

        protected void gridUserWiseQuotation_PageIndexChanged(object sender, EventArgs e)
        {
            PopulateUserWiseERPDocCreation();
        }

        //Rev Subhra 0019337  23-01-2019 
        protected void SelectPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            if (strSplitCommand == "Bindalldesignes")
            {
                string[] filePaths = new string[] { };
                string DesignPath = "";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\PurchaseIndent\DocDesign\Designes";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\PurchaseIndent\DocDesign\Designes";
                }
                string fullpath = Server.MapPath("~");
                fullpath = fullpath.Replace("ERP.UI\\", "");
                string DesignFullPath = fullpath + DesignPath;
                filePaths = System.IO.Directory.GetFiles(DesignFullPath, "*.repx");

                foreach (string filename in filePaths)
                {
                    string reportname = Path.GetFileNameWithoutExtension(filename);
                    string name = "";
                    if (reportname.Split('~').Length > 1)
                    {
                        name = reportname.Split('~')[0];
                    }
                    else
                    {
                        name = reportname;
                    }
                    string reportValue = reportname;
                    CmbDesignName.Items.Add(name, reportValue);
                }
                CmbDesignName.SelectedIndex = 0;
            }
            else
            {
                string DesignPath = @"Reports\Reports\REPXReports";
                string fullpath = Server.MapPath("~");
                fullpath = fullpath.Replace("ERP.UI\\", "");
                string filename = @"\RepxReportViewer.aspx";
                string DesignFullPath = fullpath + DesignPath + filename;
                string reportName = Convert.ToString(CmbDesignName.Value);
                SelectPanel.JSProperties["cpSuccess"] = "Success";
            }
        }
        //End of Rev Subhra 23-01-2019 
        //Rev Subhra 01-03-2019


        [WebMethod]
        public static Int32 GetQuantityfromSL(string SLNo)
        {

            DataTable dt = new DataTable();
            int SLVal = 0;
            if (HttpContext.Current.Session["MultiUOMDataIND"] != null)
            {
                DataRow[] MultiUoMresult;
                
                dt = (DataTable)HttpContext.Current.Session["MultiUOMDataIND"];
                // Mantis Issue 24428
               // MultiUoMresult = dt.Select("SrlNo ='" + SLNo + "'");
                MultiUoMresult = dt.Select("UpdateRow ='True'");
                // End of Mantis Issue 24428

                SLVal = MultiUoMresult.Length;


            }

            return SLVal;
        }


        [WebMethod]
        public static string SetSessionPacking(string list)
        {
            System.Web.Script.Serialization.JavaScriptSerializer jsSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            List<ProductQuantity> finalResult = jsSerializer.Deserialize<List<ProductQuantity>>(list);
            //var result = JsonConvert.DeserializeObject<ProductQuantity>(list);
            HttpContext.Current.Session["SessionPackingDetails"] = finalResult;
            return null;

        }
        //End of Rev


        //Rev Tanmoy 23-09-2019
        protected void ProjectServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Proj_Id";
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string branch = ddlBranch.SelectedValue;
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);

            var q = from d in dc.V_ProjectLists
                    where d.ProjectStatus == "Approved" && d.ProjBracnchid == Convert.ToInt32(branch)
                    orderby d.Proj_Id descending
                    select d;
           
            e.QueryableSource = q;
        }
        //End Rev


        //Rev Tanmoy for Hierarchy
        public void BindHierarchy()
        {
            dsHierarchy.SelectCommand = "select 0 as Hierarchy_ID ,'Select' as HIERARCHY_NAME union select ID as Hierarchy_ID,H_Name as HIERARCHY_NAME from V_HIERARCHY ";
            ddlHierarchy.DataBind();
        }


        [WebMethod]
        public static string ClosedPurchaseIndentOnRequest(string keyValue, string Reason)
        {
            PurchaseIndentBL PurchaseIndentBL = new PurchaseIndentBL();
            int CancelOrder = 0;
            CancelOrder = PurchaseIndentBL.ClosedPurchaseIndent(keyValue, Reason);


            return Convert.ToString(CancelOrder);

        }

        [WebMethod]
        public static String getHierarchyID(string ProjID)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            string Hierarchy_ID = "";

            DataTable dt2 = oDBEngine.GetDataTable("Select Hierarchy_ID from V_ProjectList where Proj_Code='" + ProjID + "'");
            if (dt2.Rows.Count > 0)
            {
                Hierarchy_ID = Convert.ToString(dt2.Rows[0]["Hierarchy_ID"]);
                return Hierarchy_ID;
            }
            else
            {
                Hierarchy_ID = "0";
                return Hierarchy_ID;
            }
        }
        //End Rev Tanmoy for Hierarchy

        protected void taggingGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = Convert.ToString(e.Parameters.Split('~')[0]);

            if (strSplitCommand == "BindComponentGrid")
            {


                string strBranch = Convert.ToString(ddlBranch.SelectedValue);
                string IndentDate = tDate.Date.ToString("yyyy-MM-dd");

                string status = string.Empty;

                if (Convert.ToString(Request.QueryString["key"]) != "ADD")
                {
                    status = "DONE";
                }
                else
                {
                    status = "NOT-DONE";
                }
                DataTable IndentTable = new DataTable();
                if (rdl_Salesquotation.SelectedValue == "MRP")
                {
                    //IndentTable = GetContractData(IndentDate, strBranch);
                    IndentTable = GetMRP(IndentDate, strBranch);
                }

                if (IndentTable != null && IndentTable.Rows.Count > 0)
                {
                    Session["IndentRequiData"] = IndentTable;
                    taggingGrid.DataSource = IndentTable;
                    taggingGrid.DataBind();

                    taggingGrid.JSProperties["cpTaggedMRP"] = "Yes";
                }
                else
                {
                    btnTaggingSave.ClientEnabled =true ;

                    taggingGrid.JSProperties["cpTaggedMRP"] = "No";
                }

            }
            else if (strSplitCommand == "SelectAndDeSelectProducts")
            {
                ASPxGridView gv = sender as ASPxGridView;
                string command = e.Parameters.ToString();
                string State = Convert.ToString(e.Parameters.Split('~')[1]);
                if (State == "SelectAll")
                {
                    for (int i = 0; i < gv.VisibleRowCount; i++)
                    {
                        gv.Selection.SelectRow(i);
                    }
                }
                if (State == "UnSelectAll")
                {
                    for (int i = 0; i < gv.VisibleRowCount; i++)
                    {
                        gv.Selection.UnselectRow(i);
                    }
                }
                if (State == "Revart")
                {
                    for (int i = 0; i < gv.VisibleRowCount; i++)
                    {
                        if (gv.Selection.IsRowSelected(i))
                            gv.Selection.UnselectRow(i);
                        else
                            gv.Selection.SelectRow(i);
                    }
                }
            }
        }
        protected void taggingGrid_DataBinding(object sender, EventArgs e)
        {
            if (Session["IndentRequiData"] != null)
            {
                taggingGrid.DataSource = (DataTable)Session["IndentRequiData"];
            }
        }
        public DataTable GetContractData(string IndentDate, string BranchId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseIndentDetailsList");
            proc.AddVarcharPara("@Action", 500, "GetContactOrder");
            proc.AddVarcharPara("@Indentdate", 50, IndentDate);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@BranchId", 500, BranchId);
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetMRP(string IndentDate, string BranchId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseIndentDetailsList");
            proc.AddVarcharPara("@Action", 500, "GetMRP");
            proc.AddVarcharPara("@Indentdate", 50, IndentDate);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@BranchId", 500, BranchId);
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            dt = proc.GetTable();
            return dt;
        }
        protected void cgridProducts_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];
            string ComponentIDs = e.Parameters.Split('~')[1];
            string ComponentDetailsIDs = e.Parameters.Split('~')[2];

            if (strSplitCommand == "BindProductsDetails")
            {
                if (ComponentIDs != "")
                {
                    taggingGrid.DataBind();
                    string[] eachQuo = ComponentIDs.Split(',');
                    for (int i = 0; i < taggingGrid.VisibleRowCount; i++)
                    {
                        string PurchaseOrder_Id = Convert.ToString(taggingGrid.GetRowValues(i, "MRP_ID"));
                        if (eachQuo.Contains(PurchaseOrder_Id))
                        {
                            taggingGrid.Selection.SelectRow(i);
                        }
                    }
                }




                String QuoComponent = "", QuoComponentNumber = "", QuoComponentDate = "";
                if (taggingGrid.GetSelectedFieldValues("MRP_ID").Count > 0)
                {
                    for (int i = 0; i < taggingGrid.GetSelectedFieldValues("MRP_ID").Count; i++)
                    {
                        QuoComponent += "," + Convert.ToString(taggingGrid.GetSelectedFieldValues("MRP_ID")[i]);
                        QuoComponentDate += "," + Convert.ToString(taggingGrid.GetSelectedFieldValues("MRP_Date")[i]);
                        QuoComponentNumber += "," + Convert.ToString(taggingGrid.GetSelectedFieldValues("MRP_No")[i]);
                    }

                    QuoComponent = QuoComponent.TrimStart(',');
                    QuoComponentDate = QuoComponentDate.TrimStart(',');
                    QuoComponentNumber = QuoComponentNumber.TrimStart(',');
                    if (taggingGrid.GetSelectedFieldValues("MRP_ID").Count > 0)
                    {
                        if (taggingGrid.GetSelectedFieldValues("MRP_ID").Count > 1)
                        {
                            QuoComponentDate = "Multiple MRP Dates";
                        }
                    }
                    else
                    {
                        QuoComponentDate = "";
                    }



                    string strAction = "";
                    string strType = Convert.ToString(rdl_Salesquotation.SelectedValue);

                    if (strType == "MRP")
                    {
                       // strAction = "GetOrderProducts";
                        strAction = "GetMRPProducts";
                    }

                    DataTable dtDetails = null;

                    if (ComponentDetailsIDs != "")
                    {
                         dtDetails = GetComponentProductList("GetOrderProducts", QuoComponent);
                    }
                    else
                    {
                         dtDetails = GetComponentProductList(strAction, QuoComponent);
                    }
                    

                  
                    Session["SI_ProductDetails"] = dtDetails;
                    grid_Products.DataSource = dtDetails;
                    grid_Products.DataBind();

                    if (ComponentDetailsIDs != "")
                    {
                        string[] eachQuo = ComponentDetailsIDs.Split(',');
                        for (int i = 0; i < grid_Products.VisibleRowCount; i++)
                        {
                            string PurchaseOrder_Id = Convert.ToString(grid_Products.GetRowValues(i, "ComponentDetailsID"));
                            if (eachQuo.Contains(PurchaseOrder_Id))
                            {
                                grid_Products.Selection.SelectRow(i);
                            }
                        }
                    }
                    else
                    {

                    }



                    grid_Products.JSProperties["cpComponentDetails"] = QuoComponentNumber + "~" + QuoComponentDate;
                }
                else
                {
                    grid_Products.DataSource = null;
                    grid_Products.DataBind();
                }

            }
            if (strSplitCommand == "SelectAndDeSelectProducts")
            {
                ASPxGridView gv = sender as ASPxGridView;
                string command = e.Parameters.ToString();
                string State = Convert.ToString(e.Parameters.Split('~')[1]);
                if (State == "SelectAll")
                {
                    for (int i = 0; i < gv.VisibleRowCount; i++)
                    {
                        gv.Selection.SelectRow(i);
                    }
                }
                if (State == "UnSelectAll")
                {
                    for (int i = 0; i < gv.VisibleRowCount; i++)
                    {
                        gv.Selection.UnselectRow(i);
                    }
                }
                if (State == "Revart")
                {
                    for (int i = 0; i < gv.VisibleRowCount; i++)
                    {
                        if (gv.Selection.IsRowSelected(i))
                            gv.Selection.UnselectRow(i);
                        else
                            gv.Selection.SelectRow(i);
                    }
                }
            }
        }
        protected void grid_Products_DataBinding(Object sender, EventArgs e)
        {
            if (Session["SI_ProductDetails"] != null)
            {
                DataTable Quotationdt = (DataTable)Session["SI_ProductDetails"];
                DataView dvData = new DataView(Quotationdt);
                grid_Products.DataSource = GetProductDetails(dvData.ToTable());
            }

        }
        public class ProductDetails
        {
            public string SrlNo { get; set; }
            public string ComponentID { get; set; }
            public string ComponentDetailsID { get; set; }
            public string ProductID { get; set; }
            public string ComponentNumber { get; set; }
            public string ProductsName { get; set; }
            public string ProductDescription { get; set; }
            public string Quantity { get; set; }

        }
        public IEnumerable GetProductDetails(DataTable ProductDet)
        {
            List<ProductDetails> ProductDetailsList = new List<ProductDetails>();

            if (ProductDet != null && ProductDet.Rows.Count > 0)
            {
                for (int i = 0; i < ProductDet.Rows.Count; i++)
                {
                    ProductDetails Quotations = new ProductDetails();

                    Quotations.SrlNo = Convert.ToString(ProductDet.Rows[i]["SrlNo"]);
                    Quotations.ComponentID = Convert.ToString(ProductDet.Rows[i]["ComponentID"]);
                    Quotations.ComponentDetailsID = Convert.ToString(ProductDet.Rows[i]["ComponentDetailsID"]);
                    Quotations.ProductID = Convert.ToString(ProductDet.Rows[i]["ProductID"]);
                    Quotations.ComponentNumber = Convert.ToString(ProductDet.Rows[i]["ComponentNumber"]);
                    Quotations.ProductsName = Convert.ToString(ProductDet.Rows[i]["ProductsName"]);
                    Quotations.ProductDescription = Convert.ToString(ProductDet.Rows[i]["ProductDescription"]);
                    Quotations.Quantity = Convert.ToString(ProductDet.Rows[i]["Quantity"]);


                    ProductDetailsList.Add(Quotations);
                }
            }

            return ProductDetailsList;
        }

        public DataTable GetComponentProductList(string Action, string ComponentList)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseIndentDetailsList");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@ComponentList", 2000, ComponentList);
            proc.AddVarcharPara("@Indent_Id", 2000, Convert.ToString(Session["Indent_Id"]));
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetComponentTemplateProductList(string Action, string ComponentList)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseIndentDetailsList");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@ComponentList", 2000, ComponentList);
            dt = proc.GetTable();
            return dt;
        }
        protected void gridTemplateproducts_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];
            if (strSplitCommand == "BindTempProductsDetails")
            {
                Session["SI_TemplateProductDetails"] = null;
                string ComponentDetailsIDs = string.Empty;
                string ProductID = "";
                string ComponentNumber = "";
                if (grid_Products.GetSelectedFieldValues("ComponentDetailsID").Count > 0)
                {
                    for (int i = 0; i < grid_Products.GetSelectedFieldValues("ComponentDetailsID").Count; i++)
                    {
                        ComponentDetailsIDs += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("ComponentDetailsID")[i]);
                        ProductID += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("ProductID")[i]);
                        ComponentNumber += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("ComponentNumber")[i]);
                    }
                    ComponentDetailsIDs = ComponentDetailsIDs.TrimStart(',');
                    ProductID = ProductID.TrimStart(',');
                    ComponentNumber = ComponentNumber.TrimStart(',');

                    string[] array = ProductID.Split(',');

                    if (array.Length > 1)
                    {
                        for (int i = 0; i < array.Length - 1; i++)
                        {
                            if (array[i] != array[i + 1])
                            {
                                gridTemplateproducts.JSProperties["cpRtnMsg"] = "Duplicate";
                                return;
                            }
                        }
                    }

                    string strAction = "";
                    strAction = "GetSeletedTemplateProductsTaggedContract";

                    DataTable dtDetails = GetComponentTemplateProductList(strAction, ProductID);
                    Session["SI_TemplateProductDetails"] = dtDetails;
                    gridTemplateproducts.DataSource = dtDetails;
                    gridTemplateproducts.DataBind();


                    // grid_Products.JSProperties["cpComponentDetails"] = QuoComponentNumber + "~" + QuoComponentDate;
                }
                else
                {
                    gridTemplateproducts.DataSource = null;
                    gridTemplateproducts.DataBind();
                }

            }
            if (strSplitCommand == "SelectAndDeSelectProducts")
            {
                ASPxGridView gv = sender as ASPxGridView;
                string command = e.Parameters.ToString();
                string State = Convert.ToString(e.Parameters.Split('~')[1]);
                if (State == "SelectAll")
                {
                    for (int i = 0; i < gv.VisibleRowCount; i++)
                    {
                        gv.Selection.SelectRow(i);
                    }
                }
                if (State == "UnSelectAll")
                {
                    for (int i = 0; i < gv.VisibleRowCount; i++)
                    {
                        gv.Selection.UnselectRow(i);
                    }
                }
                if (State == "Revart")
                {
                    for (int i = 0; i < gv.VisibleRowCount; i++)
                    {
                        if (gv.Selection.IsRowSelected(i))
                            gv.Selection.UnselectRow(i);
                        else
                            gv.Selection.SelectRow(i);
                    }
                }
            }
        }

        protected void gridTemplateproducts_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_TemplateProductDetails"] != null)
            {
                DataTable Quotationdt = (DataTable)Session["SI_TemplateProductDetails"];
                DataView dvData = new DataView(Quotationdt);
                gridTemplateproducts.DataSource = GetProductDetails(dvData.ToTable());
            }
        }
    }

}