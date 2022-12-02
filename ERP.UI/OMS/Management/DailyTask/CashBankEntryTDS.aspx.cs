using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using DevExpress.Web;
using System.Linq;
using BusinessLogicLayer;
using System.Web.Services;
using System.Collections.Generic;
using System.Collections;
using DataAccessLayer;
using DevExpress.Web.Data;
using System.Threading.Tasks;
using System.Web.UI;
using ERP.Models;
using ERP.OMS.Tax_Details.ClassFile;


namespace ERP.OMS.Management.DailyTask
{
    public partial class CashBankEntryTDS : System.Web.UI.Page
    {
        #region LocalVariable
        SqlDataSource Obj_Sds;
        BusinessLogicLayer.DBEngine oDbEngine;
        string strCon;
        string CurrentSegment;
        DataTable DtCurrentSegment;
        DataTable dtXML = new DataTable();
        BusinessLogicLayer.GenericLogSystem oGenericLogSystem;
        FinancialAccounting oFinancialAccounting = new FinancialAccounting();
        string CashBankVoucherFile_XMLPATH = null;
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        CommonBL cbl = new CommonBL();
        //    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        static string ForJournalDate = null;
        string JVNumStr = string.Empty;
        public EntityLayer.CommonELS.UserRightsForPage rights = new EntityLayer.CommonELS.UserRightsForPage();
        CustomerVendorReceiptPaymentBL objCustomerVendorReceiptPaymentBL = new CustomerVendorReceiptPaymentBL();
        PurchaseOrderBL objPurchaseOrderBL = new PurchaseOrderBL();
        GSTtaxDetails gstTaxDetails = new GSTtaxDetails();
        #endregion
        #region Page Property
        //public string PMode
        //{
        //    get { return (string)Session["Mode"]; }
        //    set { Session["Mode"] = value; }
        //}
        //public int PCounter
        //{
        //    get { return (int)ViewState["Counter"]; }
        //    set { ViewState["Counter"] = value; }
        //}
        //public string PCompanyID
        //{
        //    get { return (string)ViewState["CompanyID"]; }
        //    set { ViewState["CompanyID"] = value; }
        //}
        //public string PCurrentSegment
        //{
        //    get { return (string)ViewState["CurrentSegment"]; }
        //    set { ViewState["CurrentSegment"] = value; }
        //}
        //public string PUserID
        //{
        //    get { return (string)ViewState["UserID"]; }
        //    set { ViewState["UserID"] = value; }
        //}
        //public string PFinYear
        //{
        //    get { return (string)ViewState["FinYear"]; }
        //    set { ViewState["FinYear"] = value; }
        //}
        //public string PBranchID
        //{
        //    get { return (string)ViewState["BranchID"]; }
        //    set { ViewState["BranchID"] = value; }
        //}
        //public string PXMLPATH
        //{
        //    get { return (string)Session["CashBankVoucherFile_XMLPATH"]; }
        //    set { Session["CashBankVoucherFile_XMLPATH"] = value; }
        //}
        //public decimal TotalPayment
        //{
        //    get { return (decimal)ViewState["TotalPayment"]; }
        //    set { ViewState["TotalPayment"] = value; }
        //}
        //public decimal TotalRecieve
        //{
        //    get { return (decimal)ViewState["TotalRecieve"]; }
        //    set { ViewState["TotalRecieve"] = value; }
        //}
        public decimal TotalBankBalance
        {
            get { return (decimal)Session["TotalBankBalance"]; }
            set { Session["TotalBankBalance"] = value; }
        }
        //public string ChoosenCurrency
        //{
        //    get { return (string)Session["ChoosenCurrency"]; }
        //    set { Session["ChoosenCurrency"] = value; }
        //}
        //public string ChoosenCurrency
        //{
        //    get { return (string)Session["LocalCurrency"]; }
        //    set { Session["LocalCurrency"] = value; }
        //}
        //This For Log Purpose
        public string LogID
        {
            get { return (string)ViewState["LogID"]; }
            set { ViewState["LogID"] = value; }
        }
        #endregion
        #region PageClass
        //void Bind_Combo(ASPxComboBox Combo, SqlDataSource SDs, string strTextField, string strValueField, int SelectedIndex)
        //{
        //    Combo.DataSource = SDs;
        //    Combo.TextField = strTextField;
        //    Combo.ValueField = strValueField;
        //    Combo.DataBind();
        //    Combo.SelectedIndex = SelectedIndex;

        //}



        //void Bind_Combo(ASPxComboBox Combo, DataSet Ds, string strTextField, string strValueField, int SelectedIndex)
        //{
        //    if (Ds.Tables.Count > 0)
        //    {
        //        if (Ds.Tables[0].Rows.Count > 0)
        //        {
        //            Combo.DataSource = Ds;
        //            Combo.TextField = strTextField;
        //            Combo.ValueField = strValueField;
        //            Combo.DataBind();
        //            Combo.SelectedIndex = SelectedIndex;
        //        }
        //    }

        //}
        //void Bind_Combo_WithSelectedValue(ASPxComboBox Combo, DataSet Ds, string strTextField, string strValueField, object strSelectedValue)
        //{
        //    if (Ds.Tables.Count > 0)
        //    {
        //        if (Ds.Tables[0].Rows.Count > 0)
        //        {
        //            Combo.DataSource = Ds;
        //            Combo.TextField = strTextField;
        //            Combo.ValueField = strValueField;
        //            Combo.DataBind();
        //            if (strSelectedValue.ToString().Trim() != "0")
        //                Combo.Value = strSelectedValue;
        //            else
        //                Combo.SelectedIndex = 0;
        //        }
        //    }

        //}
        //DataSet Bind_Combo(string strcmd)
        //{
        //    DataSet Ds = new DataSet();
        //    using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
        //    {
        //        using (SqlCommand com = new SqlCommand(strcmd, con))
        //        {
        //            using (SqlDataAdapter Da = new SqlDataAdapter(com))
        //            {
        //                Da.Fill(Ds);
        //            }
        //        }
        //    }
        //    return Ds;
        //}
        //DataSet Bind_Combo(string strcmd, string parametername, string parametervalue)
        //{
        //    DataSet Ds = new DataSet();
        //    using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
        //    {
        //        using (SqlCommand com = new SqlCommand(strcmd, con))
        //        {
        //            com.Parameters.AddWithValue(parametername, parametervalue);
        //            using (SqlDataAdapter Da = new SqlDataAdapter(com))
        //            {
        //                Da.Fill(Ds);
        //            }
        //        }
        //    }
        //    return Ds;
        //}
        //void BindGrid(ASPxGridView Grid)
        //{
        //    Grid.DataSource = null;
        //    Grid.DataBind();
        //}
        //void BindGrid(ASPxGridView Grid, DataSet Ds)
        //{
        //    if (Ds.Tables.Count > 0)
        //    {
        //        Grid.DataSource = Ds;
        //        Grid.DataBind();
        //    }
        //    else
        //    {
        //        Grid.DataSource = null;
        //        Grid.DataBind();
        //    }
        //}
        //void BindGrid(ASPxGridView Grid, DataTable Dt)
        //{
        //    if (Dt.Rows.Count > 0)
        //    {
        //        Grid.DataSource = Dt;
        //        Grid.DataBind();
        //    }
        //    else
        //    {
        //        Grid.DataSource = null;
        //        Grid.DataBind();
        //    }
        //}
        //void BindGrid(ASPxGridView Grid, DataSet Ds, String WhichSort)
        //{
        //    DataView TempDV = new DataView(Ds.Tables[0]);
        //    TempDV.Sort = "EntryDateTime " + WhichSort;
        //    Grid.DataSource = TempDV;
        //    Grid.DataBind();
        //}
        //void BindGrid(ASPxGridView Grid, DataTable Dt, String WhichSort)
        //{
        //    DataView TempDV = new DataView(Dt);
        //    TempDV.Sort = "EntryDateTime " + WhichSort;
        //    Grid.DataSource = TempDV;
        //    Grid.DataBind();
        //}
        //DataSet GetDsUsingQuery(string strcmd)
        //{
        //    DataSet Ds = new DataSet();
        //    using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
        //    {
        //        using (SqlCommand com = new SqlCommand(strcmd, con))
        //        {
        //            using (SqlDataAdapter Da = new SqlDataAdapter(com))
        //            {
        //                Da.Fill(Ds);
        //            }
        //        }
        //    }
        //    return Ds;
        //}
        #endregion
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Request.QueryString.AllKeys.Contains("IsTagged"))
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/PopUp.Master";
                //divcross.Visible = false;
            }
            else
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/ERP.Master";
                //divcross.Visible = true;
            }
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //ServerControlIDs OServerControlIDs = new ServerControlIDs();
            //OServerControlIDs.Emit(Page);
        }
        public void Bind_Currency()
        {
            string LocalCurrency = Convert.ToString(Session["LocalCurrency"]);
            string basedCurrency = Convert.ToString(LocalCurrency.Split('~')[0]);
            SqlCurrencyBind.SelectCommand = "select Currency_ID,Currency_AlphaCode from Master_Currency Order By Currency_ID";
            CmbCurrency.DataBind();
        }
        public void Bind_PaymentNumberingScheme()
        {
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string strBranchID = Convert.ToString(Session["userbranchID"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            //SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            // DataTable Schemadt = objSlaesActivitiesBL.GetNumberingSchema(strCompanyID, userbranchHierarchy, FinYear, "14", "Y");
            DataTable Schemadt = GetNumberingSchema(strCompanyID, userbranchHierarchy, FinYear, "14", "Y");
            if (Schemadt != null && Schemadt.Rows.Count > 0)
            {
                CmbScheme.TextField = "SchemaName";
                CmbScheme.ValueField = "Id";
                CmbScheme.DataSource = Schemadt;
                CmbScheme.DataBind();
            }
        }
        public DataTable GetNumberingSchema(string strCompanyID, string strBranchID, string strFinYear, string strType, string strIsSplit)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Fetch_CashBankEntry_DataSet");
            proc.AddVarcharPara("@Action", 100, "GetNumberingSchema");
            proc.AddVarcharPara("@CompanyID", 100, strCompanyID);
            //proc.AddVarcharPara("@BranchID", 100, strBranchID);
            proc.AddVarcharPara("@BranchID", 4000, strBranchID);
            proc.AddVarcharPara("@FinYear", 100, strFinYear);
            proc.AddVarcharPara("@Type", 100, strType);
            proc.AddVarcharPara("@IsSplit", 100, strIsSplit);
            ds = proc.GetTable();
            return ds;
        }
        #region Ref. Cash FUnd Lookup Bind By Rajdip
        protected void EntityServerModeDataCashFund_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Paymentreqhead_Id";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            BusinessLogicLayer.DBEngine BEngine = new BusinessLogicLayer.DBEngine();

            if (hdnisprojectexists.Value == "0")
            {
                var q = from d in dc.V_ApproveCashFundLists
                        //where d.ProjectStatus == "Approved" && d.ProjBracnchid == Convert.ToInt64(ddl_Branch.SelectedValue) //&& d.CustId == Convert.ToString(hdnCustomerId.Value)
                        //orderby d.Proj_Id descending
                        select d;

                e.QueryableSource = q;
            }
            else if (hdnisprojectexists.Value == "1")
            {
                var q = from d in dc.V_ApproveCashFundLists
                        //where d.ProjectStatus == "Approved" && d.ProjBracnchid == Convert.ToInt64(ddl_Branch.SelectedValue) //&& d.CustId == Convert.ToString(hdnCustomerId.Value)
                        where d.Job_No == Convert.ToInt64(lookup_Project.Value)
                        orderby d.Paymentreqhead_Id descending
                        select d;
                e.QueryableSource = q;
            }

        }

        #endregion Rajdip
        public void Bind_ReceiptNumberingScheme()
        {
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string strBranchID = Convert.ToString(Session["userbranchID"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            //SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            //DataTable Schemadt = objSlaesActivitiesBL.GetNumberingSchema(strCompanyID, userbranchHierarchy, FinYear, "2", "Y");
            DataTable Schemadt = GetNumberingSchema(strCompanyID, userbranchHierarchy, FinYear, "2", "Y");
            if (Schemadt != null && Schemadt.Rows.Count > 0)
            {
                CmbScheme.TextField = "SchemaName";
                CmbScheme.ValueField = "Id";
                CmbScheme.DataSource = Schemadt;
                CmbScheme.DataBind();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/dailytask/CashBankEntryList.aspx");
            //   strCon = ConfigurationManager.AppSettings["DBConnectionDefault"].ToString();
            string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
            string AllowProjectInDetailsLevel = cbl.GetSystemSettingsResult("AllowProjectInDetailsLevel");
            hdnisprojectexists.Value = "0";

            //Add setting for Lead Tanmoy 01-12-2020 Start
            string IsLeadAvailableinTransactions = cbl.GetSystemSettingsResult("IsLeadAvailableinTransactions");
            hdnIsLeadAvailableinTransactions.Value = IsLeadAvailableinTransactions;
            //Add setting for Lead Tanmoy 01-12-2020 End

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


            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {
                    hdnProjectSelectInEntryModule.Value = "1";
                    lookup_Project.ClientVisible = true;
                    lblProject.Visible = true;
                    hdnisprojectexists.Value = "1";
                    divprocode.Visible = true;
                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    hdnProjectSelectInEntryModule.Value = "0";
                    lookup_Project.ClientVisible = false;
                    lblProject.Visible = false;
                    
                    hdnisprojectexists.Value = "0";
                    divprocode.Visible = false;
                }
            }

            if (!String.IsNullOrEmpty(AllowProjectInDetailsLevel))
            {
                if (AllowProjectInDetailsLevel.ToUpper().Trim() == "NO")
                {
                    hdnAllowProjectInDetailsLevel.Value = "0";
                    gridTDS.Columns[5].Width = 0;
                }
            }
            //For Hierarchy Start Tanmoy
            string HierarchySelectInEntryModule = cbl.GetSystemSettingsResult("Show_Hierarchy");
            if (!String.IsNullOrEmpty(HierarchySelectInEntryModule))
            {
                if (HierarchySelectInEntryModule.ToUpper().Trim() == "YES")
                {
                    ddlHierarchy.Visible = true;
                    lblHierarchy.Visible = true;
                    lookup_Project.Columns[3].Visible = true;
                    divhir.Visible = true;
                }
                else if (HierarchySelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    ddlHierarchy.Visible = false;
                    lblHierarchy.Visible = false;
                    lookup_Project.Columns[3].Visible = false;
                    divhir.Visible = false;
                }
            }
            //For Hierarchy End Tanmoy

            strCon = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            // oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            oDbEngine = new BusinessLogicLayer.DBEngine();

            oGenericLogSystem = new BusinessLogicLayer.GenericLogSystem();
            //CurrentSegment = HttpContext.Current.Session["userlastsegment"].ToString();
            //hdnSegmentid.Value = CurrentSegment;
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            if (!IsPostBack)
            {
                #region NewTaxblock
                string ItemLevelTaxDetails = string.Empty; string HSNCodewisetaxSchemid = string.Empty; string BranchWiseStateTax = string.Empty; string StateCodeWiseStateIDTax = string.Empty;
                gstTaxDetails.GetTaxData(ref ItemLevelTaxDetails, ref HSNCodewisetaxSchemid, ref BranchWiseStateTax, ref StateCodeWiseStateIDTax, "P");
                HDItemLevelTaxDetails.Value = ItemLevelTaxDetails;
                HDHSNCodewisetaxSchemid.Value = HSNCodewisetaxSchemid;
                HDBranchWiseStateTax.Value = BranchWiseStateTax;
                HDStateCodeWiseStateIDTax.Value = StateCodeWiseStateIDTax;


                #endregion




                if (!rights.CanAdd)
                {
                    btnSaveNew.ClientVisible = false;
                }

                Session.Remove("CashBankDetails");
                Session.Remove("exportval");
                Session.Remove("IBRef");
                Session.Remove("CB_FinalTaxRecord");
                Session.Remove("CashBank_ID");
                hdnEditRfid.Value = "";
                hdn_Mode.Value = "";
                ///rights.CanAdd
                GetFinacialYearBasedQouteDate();
                if (!String.IsNullOrEmpty(Convert.ToString(Session["userbranchID"])))
                {
                    string strdefaultBranch = Convert.ToString(Session["userbranchID"]);
                }
                string[] FinYEnd = Session["FinYearEnd"].ToString().Split(' ');
                string strFinYEnd = FinYEnd[0];
                string LastCompany = HttpContext.Current.Session["LastCompany"].ToString();
                string userid = HttpContext.Current.Session["userid"].ToString();
                string LastFinYear = HttpContext.Current.Session["LastFinYear"].ToString();
                string LocalCurrency = Convert.ToString(Session["LocalCurrency"]);

                Task PopulateStockTrialDataTask = new Task(() => BindAllControlDataOnPageLoad(strFinYEnd, LastCompany, userid, LastFinYear, LocalCurrency));
                PopulateStockTrialDataTask.RunSynchronously();

                //Tanmoy Hierarchy
                bindHierarchy();
                ddlHierarchy.Enabled = false;
                //Tanmoy Hierarchy End

                if (Request.QueryString["key"] == "ADD")
                {
                    DataTable dtposTime = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(50),Lock_Fromdate,110) LockCon_Fromdate,convert(varchar(50),Lock_Todate,110) LockCon_Todate,convert(varchar(10),Lock_Fromdate,105) DataFreeze_Fromdate,convert(varchar(10),Lock_Todate,105) DataFreeze_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Add' and Module_Id=54");

                    if (dtposTime != null && dtposTime.Rows.Count > 0)
                    {
                        hdnLockFromDate.Value = Convert.ToString(dtposTime.Rows[0]["LockCon_Fromdate"]);
                        hdnLockToDate.Value = Convert.ToString(dtposTime.Rows[0]["LockCon_Todate"]);
                        hdnLockFromDateCon.Value = Convert.ToString(dtposTime.Rows[0]["DataFreeze_Fromdate"]);
                        hdnLockToDateCon.Value = Convert.ToString(dtposTime.Rows[0]["DataFreeze_Todate"]);
                    }

                    rbtnType.SelectedValue = "P";
                    hdn_Mode.Value = "Entry";
                    hdnMode.Value = "0";
                    hdnPageStatus.Value = "first";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GridADD()", true);
                    #region To Show By Default Cursor after SAVE AND NEW
                    if (Session["SaveModeCB"] != null)  // it has been removed from coding side of Quotation list 
                    {

                        if (Session["VoucherTypeCB"] != null)
                        {
                            string command = Convert.ToString(Session["VoucherTypeCB"]);
                            rbtnType.SelectedValue = command;
                            if (command == "P")
                            {
                                Bind_PaymentNumberingScheme();
                                CmbScheme.Value = "0";
                            }
                            else if (command == "R")
                            {
                                Bind_ReceiptNumberingScheme();
                                CmbScheme.Value = "0";
                            }
                        }
                        if (Session["schemavalueCB"] != null)  // it has been removed from coding side of Quotation list 
                        {
                            CmbScheme.Value = Convert.ToString(Session["schemavalueCB"]);
                            string strSchemaValue = Convert.ToString(Session["schemavalueCB"]);
                            string[] SchemaValueID = strSchemaValue.Split('~');
                            string branchID = SchemaValueID[3];
                            ddlEnterBranch.SelectedValue = branchID;
                            BindCashBankAccount(branchID);
                        }
                        if (Session["SaveNewValues"] != null)
                        {
                            List<string> SaveNewValues = (Session["SaveNewValues"]) as List<string>;
                            ddlBranch.SelectedValue = SaveNewValues[1];
                            //ddlCashBank.Value = SaveNewValues[0];
                            //string InstrumentType = SaveNewValues[1];
                            //if (InstrumentType=="0")
                            //{
                            //    cmbInstrumentType.Value = "CH";
                            //}
                            //else
                            //{
                            //    cmbInstrumentType.Value = SaveNewValues[1];
                            //}

                            //ddl_AmountAre.Value = SaveNewValues[0];
                        }
                        if (Convert.ToString(Session["SaveModeCB"]) == "A")
                        {
                            dtTDate.Focus();
                            txtVoucherNo.Enabled = false;
                            txtVoucherNo.Text = "Auto";
                        }
                        else if (Convert.ToString(Session["SaveModeCB"]) == "M")
                        {
                            txtVoucherNo.Enabled = true;
                            txtVoucherNo.Text = "";
                            txtVoucherNo.Focus();
                        }
                    }
                    else
                    {
                        CmbScheme.Focus();
                    }
                    #endregion To Show By Default Cursor after SAVE AND NEW


                }
                else
                {
                   // lookup_Project.ClientEnabled = false;
                   // lookup_Project.ClearButton.Visibility = AutoBoolean.False;
                    hdn_Mode.Value = "Edit";
                    lblHeading.Text = "Modify Cash/Bank Voucher";
                    divNumberingScheme.Style.Add("display", "none");
                    divEnterBranch.Style.Add("display", "Block");
                    Session["CashBank_ID"] = Request.QueryString["key"];
                    ViewState["CashBankID"] = Request.QueryString["key"];
                    string CBID = Request.QueryString["key"];
                    FillGrid();
                    hdnPageStatus.Value = "update";
                    hdfIsDelete.Value = "C";
                    hdnEditRfid.Value = Request.QueryString["key"];




                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Gridupdate()", true);
                }
                if (Request.QueryString.AllKeys.Contains("IsTagged"))
                {
                    btncross.Visible = false;
                    hdnPageStatus.Value = "update";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Gridupdate()", true);
                    TaggedView(Request.QueryString["key"]);
                    // Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>CashBankTagged(" + Request.QueryString["key"] + ", '" + Request.QueryString["req"] + "');</script>");
                }
                if (Request.QueryString["req"] != null && Request.QueryString["req"] == "V")
                {
                    lblHeading.Text = "View Cash/Bank Voucher";
                    btnSaveNew.Visible = false;
                    btnSaveRecords.Visible = false;
                }

                if (Request.QueryString["IsTDS"] == "Y")
                {
                    divTDS.Style.Add("display", "block");
                    hdnIsTDS.Value = "1";
                    rbtnType.Enabled = false;
                    dvTDSpop.Style.Add("display", "block");
                    //ddl_AmountAre.ClientEnabled = false;
                    divTDS.Style.Add("display", "block");
                    //gridBatch.Enabled = false;
                }
                else
                {
                    dvTDSpop.Style.Add("display", "none");
                    divTDS.Style.Add("display", "none");
                    hdnIsTDS.Value = "0";
                    rbtnType.Enabled = true;
                    //ddl_AmountAre.ClientEnabled = true;
                    divTDS.Style.Add("display", "none");

                    //gridBatch.Enabled = true;

                }

            }

        }

        protected void EntityServerModeDatacashBankTDS_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Proj_Id";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            BusinessLogicLayer.DBEngine BEngine = new BusinessLogicLayer.DBEngine();
            // Int64 BranchIdOnnum = 1;


            var q = from d in dc.V_ProjectLists
                    where d.ProjectStatus == "Approved" && d.ProjBracnchid == Convert.ToInt64(ddlBranch.SelectedValue)
                    orderby d.Proj_Id descending
                    select d;

            e.QueryableSource = q;

        }
        public void TaggedView(string CBID)
        {
            // gridBatch.JSProperties["cpEdit"] = null;
            Session["CB_FinalTaxRecord"] = null;

            string cask_bankID = CBID.ToString();

            Session["CB_FinalTaxRecord"] = GetCashBankEditedTaxData(CBID);

            DataSet DsOnLoad = new DataSet();
            string VoucherType = "", VoucherNo = "", Date = "", Branch = "", Cash_Bank = "", Currency = "", InstrumentType = "", InstrumentNo = "", InstrumentDate = "",
                Narration = "", InstrumentTypeName = "", BankStatementDate = "", BankValueDate = "", ReceivedFrom = "", PaidTo = "", Tax_Code = "", ContactNo = "",
                ReverseCharge = "0", DraweeBank = "", EnteredBranchID = "", CashBankName = "";
            // strCurrency = "",

            DataTable CashBanktEditdt = null;

            if (CashBanktEditdt != null && CashBanktEditdt.Rows.Count > 0)
            {
                VoucherType = Convert.ToString(CashBanktEditdt.Rows[0]["VoucherType"]);//0
                rbtnType.SelectedValue = VoucherType;
                VoucherNo = Convert.ToString(CashBanktEditdt.Rows[0]["OldVoucherNumber"]);//1
                txtVoucherNo.Text = VoucherNo;
                Date = Convert.ToString(CashBanktEditdt.Rows[0]["TransactionDate"]);//2
                dtTDate.Date = Convert.ToDateTime(Date);
                Branch = Convert.ToString(CashBanktEditdt.Rows[0]["BranchID"]);//3 
                ddlBranch.SelectedValue = Branch;
                Cash_Bank = Convert.ToString(CashBanktEditdt.Rows[0]["CashBankID"]);//4                
                ddlCashBank.Value = Cash_Bank;
                Currency = Convert.ToString(CashBanktEditdt.Rows[0]["CashBank_Currency"]);//5
                CmbCurrency.Value = Currency;
                InstrumentTypeName = Convert.ToString(CashBanktEditdt.Rows[0]["InstrumentType"]);
                if (InstrumentTypeName == "0")
                {
                    InstrumentType = "CH";
                }
                else
                {
                    InstrumentType = Convert.ToString(CashBanktEditdt.Rows[0]["InstrumentType"]);//6
                }
                cmbInstrumentType.Value = InstrumentType;
                InstrumentNo = Convert.ToString(CashBanktEditdt.Rows[0]["InstrumentNumber"]);//7
                txtInstNobth.Text = InstrumentNo.Trim();
                Narration = Convert.ToString(CashBanktEditdt.Rows[0]["Narration"]);//8
                txtNarration.Text = Narration;
                BankStatementDate = Convert.ToString(CashBanktEditdt.Rows[0]["CashBankDetail_BankStatementDate"]);
                BankValueDate = Convert.ToString(CashBanktEditdt.Rows[0]["CashBankDetail_BankValueDate"]);
                ReceivedFrom = Convert.ToString(CashBanktEditdt.Rows[0]["CashBank_ReceivedFrom"]);//12
                txtReceivedFrom.Text = ReceivedFrom;
                PaidTo = Convert.ToString(CashBanktEditdt.Rows[0]["CashBank_PaidTo"]);//13
                txtPaidTo.Text = PaidTo;
                Tax_Code = Convert.ToString(CashBanktEditdt.Rows[0]["Tax_Code"]);
                ddl_AmountAre.SelectedValue = Tax_Code;
                ContactNo = Convert.ToString(CashBanktEditdt.Rows[0]["CashBank_ContactNo"]);
                txtContact.Text = ContactNo;
                ReverseCharge = Convert.ToString(CashBanktEditdt.Rows[0]["CashBank_ReverseCharge"]);
                chk_reversemechenism.Checked = Convert.ToBoolean(ReverseCharge);
                InstrumentDate = Convert.ToString(CashBanktEditdt.Rows[0]["InstrumentDate"]);
                InstDate.Date = Convert.ToDateTime(InstrumentDate);
                DraweeBank = Convert.ToString(CashBanktEditdt.Rows[0]["CashBank_DraweeBank"]);
                txtDraweeBank.Text = DraweeBank;
                EnteredBranchID = Convert.ToString(CashBanktEditdt.Rows[0]["CashBank_EnteredBranchID"]);
                ddlEnterBranch.SelectedValue = EnteredBranchID;
                CashBankName = Convert.ToString(CashBanktEditdt.Rows[0]["CashBankName"]);
                BindCashBankAccount(EnteredBranchID);
            }


            DataTable Voucherdt = null;

            string Recipt = Convert.ToString(Voucherdt.Compute("Sum(ReceiptAmount)", ""));
            string Payment = Convert.ToString(Voucherdt.Compute("Sum(PaymentAmount)", ""));

            //gridBatch.JSProperties["cpEdit"] = VoucherType + "*" + VoucherNo + "*" + Date + "*" + Branch + "*" + Cash_Bank + "*" + Currency + "*" +
            //    InstrumentType + "*" + InstrumentNo + "*" + Narration + "*" + Recipt + "*" + Payment + "*" + cask_bankID + "*" + ReceivedFrom + "*" + PaidTo + "*" +
            //    Tax_Code + "*" + ContactNo + "*" + ReverseCharge + "*" + InstrumentDate + "*" + DraweeBank + "*" + EnteredBranchID + "*" + CashBankName;
            // gridBatch.JSProperties["cpView"] = (command.ToUpper() == "VIEWTAGGED") ? "1" : "0";

            Session["CashBankDetails"] = Voucherdt;

            //gridBatch.JSProperties["cpCBEdit"] = VoucherType + "*" + cask_bankID + "*" + Currency + "*" + InstrumentType;
            hdnView.Value = (Request.QueryString["IsTagged"] != null && Request.QueryString["IsTagged"] == "1") ? "1" : "0";

        }
        public void BindAllControlDataOnPageLoad(string strFinYEnd, string LastCompany, string userid, string LastFinYear, string strLocalCurrency)
        {
            //rbtnType.SelectedValue = "P";
            CreateDataTaxTable();
            BindBranch();
            GetAllDropDownDetailForCashBank();
            InstDate.EditFormatString = objConverter.GetDateFormat("Date");
            string fDate = null;
            string FinYearEnd = strFinYEnd;
            DateTime date3 = DateTime.ParseExact(FinYearEnd, "M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            ForJournalDate = date3.ToString();
            int month = oDBEngine.GetDate().Month;
            int date = oDBEngine.GetDate().Day;
            int Year = oDBEngine.GetDate().Year;
            if (date3 < oDBEngine.GetDate().Date)
            {
                fDate = Convert.ToDateTime(ForJournalDate).Month.ToString() + "/" + Convert.ToDateTime(ForJournalDate).Day.ToString() + "/" + Convert.ToDateTime(ForJournalDate).Year.ToString();
            }
            else
            {
                fDate = Convert.ToDateTime(oDBEngine.GetDate().Date).Month.ToString() + "/" + Convert.ToDateTime(oDBEngine.GetDate().Date).Day.ToString() + "/" + Convert.ToDateTime(oDBEngine.GetDate().Date).Year.ToString();
            }

            InstDate.Value = DateTime.ParseExact(fDate, @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            //PCompanyID = LastCompany;
            // PUserID = userid;
            //PFinYear = LastFinYear;       


            //TotalPayment = 0;
            // TotalRecieve = 0;
            TotalBankBalance = 0;
            Obj_Sds = new SqlDataSource();
            Obj_Sds.ConnectionString = strCon;
            ViewState["LogID"] = oGenericLogSystem.GetLogID();
            BindSystemSettings();


            #region ####### Search Grid Filter #############

            string LocalCurrency = strLocalCurrency;
            string basedCurrency = Convert.ToString(LocalCurrency.Split('~')[0]);
            string CurrencyId = Convert.ToString(basedCurrency[0]);
            CmbCurrency.Value = CurrencyId;


            #endregion

        }
        public void FillGrid()
        {
            //RowIndex = Convert.ToInt32(e.Parameter.Split('~')[1]);
            string CBID = Request.QueryString["key"];
            string cask_bankID;
            //string CBID = GvCBSearch.GetRowValues(RowIndex, "CBID").ToString();
            ViewState["CashBankID"] = CBID;
            // IBRef = GvCBSearch.GetRowValues(RowIndex, "IBRef").ToString();
            //  Session["IBRef"] = IBRef;
            hdnEditRfid.Value = CBID;
            Session["CB_FinalTaxRecord"] = null;
            cask_bankID = CBID;
            hdnEditCBID.Value = CBID;
            Session["CB_FinalTaxRecord"] = GetCashBankEditedTaxData(CBID);
            DataSet DsOnLoad = new DataSet();
            string VoucherType = "", VoucherNo = "", Date = "", Branch = "", Cash_Bank = "", Currency = "", InstrumentType = "", InstrumentNo = "", InstrumentDate = "",
                Narration = "", InstrumentTypeName = "", BankStatementDate = "", BankValueDate = "", ReceivedFrom = "", PaidTo = "", Tax_Code = "", ContactNo = "",
                ReverseCharge = "0", DraweeBank = "", EnteredBranchID = "", CashBankName = "";
            int Paymentreqhead_Id = 0;
            DataSet CashBanktEdit = GetCashBanktEditData();
            DataTable CashBanktEditdt = CashBanktEdit.Tables[0];
            DataTable cashbankproject = new DataTable(); ;
            if (CashBanktEdit.Tables.Count != 1)
            {
                cashbankproject = CashBanktEdit.Tables[1];
            }
            if (CashBanktEdit != null && CashBanktEdit.Tables.Count>0)
            {
                Purchase_BillingShipping.SetBillingShippingTable(CashBanktEdit.Tables[2]);
            }
            if (CashBanktEditdt != null && CashBanktEditdt.Rows.Count > 0)
            {
                VoucherType = Convert.ToString(CashBanktEditdt.Rows[0]["VoucherType"]);//0
                rbtnType.SelectedValue = VoucherType;
                VoucherNo = Convert.ToString(CashBanktEditdt.Rows[0]["OldVoucherNumber"]);//1
                txtVoucherNo.Text = VoucherNo;
                Date = Convert.ToString(CashBanktEditdt.Rows[0]["TransactionDate"]);//2
                dtTDate.Date = Convert.ToDateTime(Date);
                Branch = Convert.ToString(CashBanktEditdt.Rows[0]["BranchID"]);//3 
                ddlBranch.SelectedValue = Branch;
                Cash_Bank = Convert.ToString(CashBanktEditdt.Rows[0]["CashBankID"]);//4                
                ddlCashBank.Value = Cash_Bank;
                Currency = Convert.ToString(CashBanktEditdt.Rows[0]["CashBank_Currency"]);//5
                CmbCurrency.Value = Currency;
                InstrumentTypeName = Convert.ToString(CashBanktEditdt.Rows[0]["InstrumentType"]);
                if (InstrumentTypeName == "0")
                {
                    InstrumentType = "CH";
                }
                else
                {
                    InstrumentType = Convert.ToString(CashBanktEditdt.Rows[0]["InstrumentType"]);//6
                }
                if (CashBanktEdit.Tables.Count != 1)
                {

                    lookup_Project.GridView.Selection.SelectRowByKey(Convert.ToInt64(cashbankproject.Rows[0]["Proj_Id"]));

                    //Tanmoy  Hierarchy
                    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                    DataTable dt2 = oDBEngine.GetDataTable("Select Hierarchy_ID from V_ProjectList where Proj_Id='" + Convert.ToInt64(cashbankproject.Rows[0]["Proj_Id"]) + "'");
                    if (dt2.Rows.Count > 0)
                    {
                        ddlHierarchy.SelectedValue = Convert.ToString(dt2.Rows[0]["Hierarchy_ID"]);
                    }
                    //Tanmoy  Hierarchy End

                }
                cmbInstrumentType.Value = InstrumentType;
                InstrumentNo = Convert.ToString(CashBanktEditdt.Rows[0]["InstrumentNumber"]);//7
                txtInstNobth.Text = InstrumentNo.Trim();
                Narration = Convert.ToString(CashBanktEditdt.Rows[0]["Narration"]);//8
                txtNarration.Text = Narration;
                BankStatementDate = Convert.ToString(CashBanktEditdt.Rows[0]["CashBankDetail_BankStatementDate"]);
                BankValueDate = Convert.ToString(CashBanktEditdt.Rows[0]["CashBankDetail_BankValueDate"]);
                ReceivedFrom = Convert.ToString(CashBanktEditdt.Rows[0]["CashBank_ReceivedFrom"]);//12
                txtReceivedFrom.Text = ReceivedFrom;
                PaidTo = Convert.ToString(CashBanktEditdt.Rows[0]["CashBank_PaidTo"]);//13
                //Rev Rajdip
                Paymentreqhead_Id = Convert.ToInt32(CashBanktEditdt.Rows[0]["Paymentreqhead_Id"]);
                if (Paymentreqhead_Id != 0)
                {
                    //lookup_CashFund.GridView.Selection.SelectRowByKey(Convert.ToInt64(Paymentreqhead_Id));              
                    lookup_CashFund.GridView.Selection.SelectRowByKey(Convert.ToInt64(Paymentreqhead_Id.ToString()));
                }
                //End Rev Rajdip
                txtPaidTo.Text = PaidTo;
                Tax_Code = Convert.ToString(CashBanktEditdt.Rows[0]["Tax_Code"]);
                ddl_AmountAre.SelectedValue = Tax_Code;
                ContactNo = Convert.ToString(CashBanktEditdt.Rows[0]["CashBank_ContactNo"]);
                txtContact.Text = ContactNo;
                ReverseCharge = Convert.ToString(CashBanktEditdt.Rows[0]["CashBank_ReverseCharge"]);
                chk_reversemechenism.Checked = Convert.ToBoolean(ReverseCharge);
                InstrumentDate = Convert.ToString(CashBanktEditdt.Rows[0]["InstrumentDate"]);
                InstDate.Date = Convert.ToDateTime(InstrumentDate);
                DraweeBank = Convert.ToString(CashBanktEditdt.Rows[0]["CashBank_DraweeBank"]);
                txtDraweeBank.Text = DraweeBank;
                EnteredBranchID = Convert.ToString(CashBanktEditdt.Rows[0]["CashBank_EnteredBranchID"]);
                ddlEnterBranch.SelectedValue = EnteredBranchID;
                CashBankName = Convert.ToString(CashBanktEditdt.Rows[0]["CashBankName"]);
                BindCashBankAccount(EnteredBranchID);
            }

            DataTable Voucherdt = GetCashBanktBatchEditData(hdnEditRfid.Value).Tables[0];
            string Recipt = Convert.ToString(Voucherdt.Compute("Sum(ReceiptTDS)", ""));
            string Payment = Convert.ToString(Voucherdt.Compute("Sum(WithDrawlTDS)", ""));
            //txtTotalAmount.Text = Recipt;
            //txtTotalPayment.Text = Payment;
            txtTotalAmount.Text = Payment;
            txtTotalPayment.Text = Recipt;
            // ASPxCallbackGeneral.JSProperties["cpView"] = (command.ToUpper() == "VIEW") ? "1" : "0";
            Session["CashBankDetails"] = GetCashBanktBatchEditData(hdnEditRfid.Value).Tables[0];
            gridTDS.DataBind();
            // gridBatch.JSProperties["cpCBEdit"] = VoucherType + "*" + cask_bankID + "*" + Currency + "*" + InstrumentType;
        }
        public DataSet GetSystemSettings()
        {
            DataSet dt = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Fetch_CashBankEntry_DataSet");
            proc.AddVarcharPara("@Action", 100, "GetSystemSettingValue");
            dt = proc.GetDataSet();
            return dt;
        }
        public void BindSystemSettings()
        {
            DataSet dtSystemSettings = new DataSet();
            dtSystemSettings = GetSystemSettings();
            if (dtSystemSettings.Tables[0] != null && dtSystemSettings.Tables[0].Rows.Count > 0)
            {
                string Variable_Value = Convert.ToString(dtSystemSettings.Tables[0].Rows[0]["Variable_Value"]);
                hdn_CashBankType_InstType.Value = Variable_Value;
            }
            if (dtSystemSettings.Tables[1] != null && dtSystemSettings.Tables[1].Rows.Count > 0)
            {
                string Variable_Value = Convert.ToString(dtSystemSettings.Tables[1].Rows[0]["Variable_Value"]);
                hdnAutoPrint.Value = Variable_Value;
            }
            if (dtSystemSettings.Tables[2] != null && dtSystemSettings.Tables[2].Rows.Count > 0)
            {
                string Variable_Value = Convert.ToString(dtSystemSettings.Tables[2].Rows[0]["Variable_Value"]);
                hdnSubAccountYESNO.Value = Variable_Value;
            }
            if (dtSystemSettings.Tables[3] != null && dtSystemSettings.Tables[3].Rows.Count > 0)
            {
                string Variable_Value = Convert.ToString(dtSystemSettings.Tables[3].Rows[0]["Variable_Value"]);
                hdnPaidToYesNO.Value = Variable_Value;
            }
        }
        //public void BindMainGridList()
        //{          
        //    string FromDate = Convert.ToDateTime(FormDate.Value).ToString("yyyy-MM-dd");
        //    string ToDate = Convert.ToDateTime(toDate.Value).ToString("yyyy-MM-dd");
        //    string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
        //    string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
        //    string finyear = Convert.ToString(Session["LastFinYear"]);
        //    string BranchID = Convert.ToString(cmbBranchfilter.Value);

        //    DataTable Ds = CashBankVoucherDetailsList(userbranch, lastCompany, finyear, BranchID, FromDate, ToDate, "false");
        //    if (Ds.Rows.Count > 0)
        //    {
        //        GvCBSearch.DataSource = Ds;
        //        GvCBSearch.DataBind();
        //    }
        //    else
        //    {
        //        GvCBSearch.DataSource = null;
        //        GvCBSearch.DataBind();
        //    }
        //}
        public DataTable CashBankVoucherDetailsList(string userbranchlist, string lastCompany, string Fiyear, string userbranchID, string FromDate, string ToDate, string filter)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Fetch_CashBankEntry_DataSet");
            proc.AddVarcharPara("@Action", 500, "CashBankVoucherDetailsList");
            proc.AddVarcharPara("@FinYear", 500, Fiyear);
            proc.AddVarcharPara("@CompanyID", 500, lastCompany);
            proc.AddVarcharPara("@userbranchlist", 5000, userbranchlist);
            proc.AddVarcharPara("@BranchID", 3000, userbranchID);
            proc.AddVarcharPara("@FromDate", 10, FromDate);
            proc.AddVarcharPara("@ToDate", 10, ToDate);
            proc.AddVarcharPara("@filter", 10, filter);
            dt = proc.GetTable();
            return dt;
        }
        public void GetAllDropDownDetailForCashBank()
        {
            DataSet dst = new DataSet();
            dst = AllDropDownDetailForCashBank();
            if (dst.Tables[0] != null && dst.Tables[0].Rows.Count > 0)
            {
                ddlBranch.DataTextField = "BANKBRANCH_NAME";
                ddlBranch.DataValueField = "BANKBRANCH_ID";
                ddlBranch.DataSource = dst.Tables[0];
                ddlBranch.DataBind();
            }
            //if (dst.Tables[1] != null && dst.Tables[1].Rows.Count > 0)
            //{
            //    cmbBranchfilter.ValueField = "branch_id";
            //    cmbBranchfilter.TextField = "branch_description";
            //    cmbBranchfilter.DataSource = dst.Tables[1];
            //    cmbBranchfilter.DataBind();
            //    cmbBranchfilter.SelectedIndex = 0;
            //    cmbBranchfilter.Value = Convert.ToString(Session["userbranchID"]);
            //}
            if (dst.Tables[3] != null && dst.Tables[3].Rows.Count > 0)
            {
                CmbCurrency.TextField = "Currency_AlphaCode";
                CmbCurrency.ValueField = "Currency_ID";
                CmbCurrency.DataSource = dst.Tables[3];
                CmbCurrency.DataBind();
            }
            if (dst.Tables[4] != null && dst.Tables[4].Rows.Count > 0)
            {
                CmbScheme.TextField = "SchemaName";
                CmbScheme.ValueField = "ID";
                CmbScheme.DataSource = dst.Tables[4];
                CmbScheme.DataBind();
            }
        }
        public DataSet AllDropDownDetailForCashBank()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Fetch_CashBankEntry_DataSet");
            proc.AddVarcharPara("@Action", 100, "GetAllDropDownData");
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@CompanyID", 500, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@userbranchlist", 5000, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            ds = proc.GetDataSet();
            return ds;
        }
        public void CreateDataTaxTable()
        {
            DataTable TaxRecord = new DataTable();
            TaxRecord.Columns.Add("SlNo", typeof(System.Int32));
            TaxRecord.Columns.Add("TaxCode", typeof(System.String));
            TaxRecord.Columns.Add("AltTaxCode", typeof(System.String));
            TaxRecord.Columns.Add("Percentage", typeof(System.Decimal));
            TaxRecord.Columns.Add("Amount", typeof(System.Decimal));
            Session["CB_FinalTaxRecord"] = TaxRecord;
        }
        public void BindBranch()
        {
            dsBranch.SelectCommand = "SELECT BRANCH_id AS BANKBRANCH_ID , RTRIM(BRANCH_DESCRIPTION)+' ['+ISNULL(RTRIM(BRANCH_CODE),'')+']' AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH where BRANCH_id in(" + Convert.ToString(Session["userbranchHierarchy"]) + ")";
            ddlEnterBranch.DataBind();
        }
        protected void ddlCashBank_Callback(object sender, CallbackEventArgsBase e)
        {
            string userbranch = e.Parameter.Split('~')[0];
            BindCashBankAccount(Convert.ToString(ddlBranch.SelectedValue));
            //  BindCashBankAccount(userbranch);
        }
        public void BindCashBankAccount(string userbranch)
        {
            DBEngine objEngine = new DBEngine();
            DataTable dtCustomer = new DataTable();
            string CompanyId = Convert.ToString(Session["LastCompany"]);
            dtCustomer = GetCustomerCashBank(userbranch, CompanyId, Convert.ToString(Session["CashBank_ID"]));
            if (dtCustomer.Rows.Count > 0)
            {
                ddlCashBank.TextField = "IntegrateMainAccount";
                ddlCashBank.ValueField = "MainAccount_AccountCode";
                ddlCashBank.DataSource = dtCustomer;
                ddlCashBank.DataBind();
            }
            else
            {
                ddlCashBank.TextField = "IntegrateMainAccount";
                ddlCashBank.ValueField = "MainAccount_AccountCode";
                ddlCashBank.DataSource = dtCustomer;
                ddlCashBank.DataBind();
            }

        }
        public DataTable GetCustomerCashBank(string @userbranch, string CompanyId, string CashBankId)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Fetch_CashBankEntry_DataSet");
            proc.AddVarcharPara("@Action", 100, "GetCashBankData");
            proc.AddVarcharPara("@BranchId", 100, @userbranch);
            proc.AddVarcharPara("@CompanyId", 100, CompanyId);
            proc.AddBigIntegerPara("@UserId", Convert.ToInt64(HttpContext.Current.Session["userid"]));
            proc.AddVarcharPara("@CashBank_id", 50, CashBankId);
            ds = proc.GetTable();
            return ds;
        }
        #region  Main Grid Related
        //        void FillAllSearchGrid(bool filter = false)
        //        {


        //            string strQuery_NextPart = null;
        //            string strMainQuery = null;
        //            DataSet DsSearch = new DataSet();
        //            GvCBSearch.Columns[0].FixedStyle = GridViewColumnFixedStyle.Left;
        //            GvCBSearch.Columns[1].FixedStyle = GridViewColumnFixedStyle.Left;
        //            GvCBSearch.Columns[2].FixedStyle = GridViewColumnFixedStyle.Left;


        //            string BranchID = String.Empty;
        //            DateTime TransactionDate = Convert.ToDateTime("0100-01-01");
        //            string VoucherNumber = String.Empty, CashBankAc = String.Empty,
        //                    MainNarration = String.Empty, MainAc = String.Empty,
        //                    SubAc = String.Empty;
        //            string InstType = String.Empty;
        //            string InstNo = String.Empty;
        //            DateTime Instdate = Convert.ToDateTime("0100-01-01");
        //            string CustBank = String.Empty, IssueBank = String.Empty,
        //                    WithFrom = String.Empty, DepstInto = String.Empty,
        //                    AuthLetterRef = String.Empty, PayeeAc = String.Empty,
        //                    Payment = String.Empty, Recieve = String.Empty,
        //                    LineNarration = String.Empty;
        //            GvCBSearch.Columns[2].Visible = true;
        //            GvCBSearch.Columns[4].Visible = true;
        //            // Case 
        //            //When CashBank_TransactionType='P' Then CashBankDetail_PaymentAmount else CashBankDetail_ReceiptAmount
        //            //End as Amount,

        //            string strCommanQueryPart = @"
        //                                        Select CashBank_ID as CBID,
        //            CashBank_CashBankID as CashBankID,
        //            Case 
        //            When CashBank_TransactionType='P' Then 'Payment' else 'Receipt'
        //            End as CashBank_TransactionType,
        //            Convert(Varchar(10),CashBank_TransactionDate,105) as TransactionDate,
        //            CashBank_VoucherNumber as VoucherNumber,
        //            (select Currency_AlphaCode from Master_Currency where Currency_ID=CashBank_Currency )as CashBank_Currency,
        //            (Select top 1 CashBankDetail_InstrumentNumber from trans_cashbankdetail Where CashBank_ID=CashBankDetail_VoucherID)  as InstrumentNumber,
        //            (Select top 1 CashBankDetail_BankValueDate from trans_cashbankdetail Where CashBank_ID=CashBankDetail_VoucherID)as ValueDate,
        //             (Select top 1 CashBankDetail_BankStatementDate from trans_cashbankdetail Where CashBank_ID=CashBankDetail_VoucherID)as BankStatementDate,
        //            CashBank_Narration as Narration,
        //            CashBank_ExchangeSegmentID as ExchangeSegmentID,
        //            CashBank_IBRef as IBRef,
        //           
        //             Case 
        //            When CashBank_TransactionType='P' Then Cast(IsNull(CashBankDetail_PaymentAmount,0)AS DECIMAL(18,2)) 
        //            else  Cast(IsNull(CashBankDetail_ReceiptAmount,0)AS DECIMAL(18,2)) 
        //            End as Amount,
        //            (Select Count(*) from Trans_CashBankVouchers,Trans_CashBankDetail
        //            where CashBank_ID=CashBankDetail_VoucherID
        //            and CashBank_CompanyID='" + Session["LastCompany"].ToString() + @"'
        //             and CashBank_FinYear='" + Session["LastFinYear"].ToString() + "'"
        //                                               + strQuery_NextPart + @") as TotalRow,
        //                                              (Select Max(LockDaysOrDate) From(
        //								            Select GlobalSettings_Name,
        //            Case
        //            When GlobalSettings_Value=1 Then (DATEADD(dd, 0, DATEDIFF(dd, 0, GetDate()-GlobalSettings_LockDays)))
        //            Else (DATEADD(dd, 0, DATEDIFF(dd, 0, GlobalSettings_LockDate))) 
        //            End as LockDaysOrDate
        //            From
        //            (Select GlobalSettings_SegmentID,GlobalSettings_Name,GlobalSettings_LockDays,
        //            GlobalSettings_LockDate,GlobalSettings_Value from  Config_GlobalSettings
        //            Where GlobalSettings_SegmentID=CashBank_ExchangeSegmentID
        //            and GlobalSettings_Name='GS_LCKBNK'
        //            union
        //            Select GlobalSettings_SegmentID,GlobalSettings_Name,GlobalSettings_LockDays,
        //            GlobalSettings_LockDate,GlobalSettings_Value from  Config_GlobalSettings
        //            Where GlobalSettings_SegmentID=CashBank_ExchangeSegmentID
        //            and GlobalSettings_Name='GS_LCKALL') as t1
        //            ) as t2) as MaxLockDate,
        //
        //            (Select user_name from tbl_master_user where user_id=CashBank_CreateUser)as CashBank_CreateUser,
        //            (Select user_name from tbl_master_user where user_id=CashBank_ModifyUser)as CashBank_ModifyUser,
        //            CONVERT(VARCHAR(10),CashBank_CreateDateTime, 105) + '  ' + CONVERT(VARCHAR(8), CashBank_CreateDateTime, 108) as CashBank_CreateDateTime
        //
        //            from Trans_CashBankVouchers 
        //            Inner Join 
        //            (Select CashBankDetail_VoucherID,SUM(CashBankDetail_ReceiptAmount) as CashBankDetail_ReceiptAmount,SUM(CashBankDetail_PaymentAmount)as CashBankDetail_PaymentAmount from trans_cashbankdetail 
        //            group by CashBankDetail_VoucherID) as temp_cashbankdetail on 
        //            Trans_CashBankVouchers.CashBank_ID=temp_cashbankdetail.CashBankDetail_VoucherID
        //            and CashBank_CompanyID='" + Session["LastCompany"].ToString() + @"' 
        //            and (CashBank_TransactionType='P' OR CashBank_TransactionType='R')
        //          
        //            and CashBank_FinYear='" + Session["LastFinYear"].ToString() + "'";

        //            if (filter == true)
        //            {
        //                strCommanQueryPart += " and CashBank_TransactionDate between '" + FormDate.Date.ToString("yyyy-MM-dd") + "' and '" + toDate.Date.ToString("yyyy-MM-dd") + "'";

        //                strCommanQueryPart += " and CashBank_BranchID in(" + ((Convert.ToString(cmbBranchfilter.Value)).Trim() == "0" ? Convert.ToString(Session["userbranchHierarchy"]) : Convert.ToString(cmbBranchfilter.Value)) + ")  order by CashBank_ID desc";
        //            }
        //            else
        //            {
        //                //strCommanQueryPart += " and CashBank_BranchID in(" + Convert.ToString(Session["userbranchHierarchy"]) + ")  order by CashBank_ID desc";
        //                strCommanQueryPart += " and CashBank_BranchID in(" + Convert.ToString(Session["userbranchID"]) + ")  order by CashBank_ID desc";
        //            }


        //            strMainQuery = strCommanQueryPart;


        //            DsSearch = GetDsUsingQuery(strMainQuery);
        //            if (DsSearch.Tables.Count > 0)
        //            {
        //                if (DsSearch.Tables[0].Rows.Count > 0)
        //                {
        //                    BindGrid(GvCBSearch, DsSearch);

        //                }
        //                else
        //                {
        //                    // GvCBSearch.JSProperties["cpIsEmptyDsSearch"] = "NoRecord";
        //                    GvCBSearch.DataSource = null;
        //                    GvCBSearch.DataBind();
        //                    Session["strMainQuery"] = null;
        //                    Session["strSearchBy"] = null;
        //                    Session["strSearchByMainQuery"] = null;
        //                }
        //            }
        //            else
        //            {
        //                //GvCBSearch.JSProperties["cpIsEmptyDsSearch"] = "NoRecord";
        //                GvCBSearch.DataSource = null;
        //                GvCBSearch.DataBind();
        //                Session["strMainQuery"] = null;
        //                Session["strSearchBy"] = null;
        //                Session["strSearchByMainQuery"] = null;
        //            }

        //        }
        //protected void GvCBSearch_CustomCallback(object sender, ASPxGridViewCustomButtonEventArgs e)
        //{
        //    if (!rights.CanDelete)
        //    {
        //        if (e.ButtonID == "CustomBtnDelete")
        //        {
        //            e.Visible = DevExpress.Utils.DefaultBoolean.False;
        //        }
        //    }


        //    if (!rights.CanEdit)
        //    {
        //        if (e.ButtonID == "CustomBtnEdit")
        //        {
        //            e.Visible = DevExpress.Utils.DefaultBoolean.False;
        //        }
        //    }
        //    if (!rights.CanView)
        //    {
        //        if (e.ButtonID == "CustomBtnView")
        //        {
        //            e.Visible = DevExpress.Utils.DefaultBoolean.False;
        //        }
        //    }
        //}
        //protected void GvCBSearch_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        //{
        //    string command = e.Parameters.Split('~')[0];
        //    GvCBSearch.JSProperties["cpDelete"] = null;
        //    if (command == "Delete")
        //    {
        //        int RowIndex = Convert.ToInt32(e.Parameters.Split('~')[1]);
        //        string IBRef = GvCBSearch.GetRowValues(RowIndex, "IBRef").ToString();
        //        Session["IBRef"] = IBRef;
        //        int val = GetCaskBankDeleteData();
        //        if (val == 1)
        //        {
        //            GvCBSearch.JSProperties["cpDelete"] = "Succesfully Deleted";
        //            string FromDate = Convert.ToDateTime(FormDate.Value).ToString("yyyy-MM-dd");
        //            string ToDate = Convert.ToDateTime(toDate.Value).ToString("yyyy-MM-dd");
        //            string BranchID = Convert.ToString(cmbBranchfilter.Value);
        //            string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
        //            string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
        //            string finyear = Convert.ToString(Session["LastFinYear"]);
        //            Session["CashBankListingDetails"] = CashBankVoucherDetailsList(userbranch, lastCompany, finyear, BranchID, FromDate, ToDate, "true");
        //            GvCBSearch.DataBind();
        //        }
        //        else
        //        {
        //            GvCBSearch.JSProperties["cpDelete"] = "Voucher is Reconciled.Cannot Delete";
        //        }

        //    }
        //    else if (command == "FilterGridByDate")
        //    {

        //        string FromDate = Convert.ToString(e.Parameters.Split('~')[1]);
        //        string ToDate = Convert.ToString(e.Parameters.Split('~')[2]);
        //        string BranchID = Convert.ToString(e.Parameters.Split('~')[3]);

        //        string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
        //        string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
        //        string finyear = Convert.ToString(Session["LastFinYear"]);
        //        Session["CashBankListingDetails"] = CashBankVoucherDetailsList(userbranch, lastCompany, finyear, BranchID, FromDate, ToDate, "true");
        //        GvCBSearch.DataBind();
        //    }
        //}
        //protected void GvCBSearch_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        //{
        //    if (!rights.CanDelete)
        //    {
        //        if (e.ButtonID == "CustomBtnDelete")
        //        {
        //            e.Visible = DevExpress.Utils.DefaultBoolean.False;
        //        }
        //    }
        //    if (!rights.CanEdit)
        //    {
        //        if (e.ButtonID == "CustomBtnEdit")
        //        {
        //            e.Visible = DevExpress.Utils.DefaultBoolean.False;
        //        }
        //    }
        //    if (!rights.CanView)
        //    {
        //        if (e.ButtonID == "CustomBtnView")
        //        {
        //            e.Visible = DevExpress.Utils.DefaultBoolean.False;
        //        }
        //    }
        //}
        //protected void GvCBSearch_DataBinding(object sender, EventArgs e)
        //{
        //    //string BranchID = Convert.ToString(cmbBranchfilter.Value);
        //    //string FromDate = Convert.ToDateTime(FormDate.Value).ToString("yyyy-MM-dd");
        //    //string ToDate = Convert.ToDateTime(toDate.Value).ToString("yyyy-MM-dd");

        //    //string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
        //    //string userbranch = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
        //    //string finyear = Convert.ToString(Session["LastFinYear"]);

        //    // DataTable dsdata = CashBankVoucherDetailsList(userbranch, lastCompany, finyear, BranchID, FromDate, ToDate, "true");

        //    DataTable dsdata = (DataTable)Session["CashBankListingDetails"];
        //    GvCBSearch.DataSource = dsdata;
        //}
        #endregion
        public int GetCaskBankDeleteData()
        {
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("Fetch_CashBankEntry_DataSet");
            proc.AddVarcharPara("@Action", 500, "CashBankDeleteDetails");

            proc.AddVarcharPara("@IBRef", 200, Convert.ToString(Session["IBRef"]));
            proc.AddVarcharPara("@ReturnValue", 200, "0", QueryParameterDirection.Output);
            proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
            return rtrnvalue;
        }
        #region Export
        //public void bindexport(int Filter)
        //{
        //    exporter.GridViewID = "GvCBSearch";
        //    string filename = "CashBankVoucher";
        //    exporter.FileName = filename;

        //    exporter.PageHeader.Left = "Cash/Bank Voucher";
        //    exporter.PageFooter.Center = "[Page # of Pages #]";
        //    exporter.PageFooter.Right = "[Date Printed]";

        //    switch (Filter)
        //    {
        //        case 1:
        //            exporter.WritePdfToResponse();
        //            break;
        //        case 2:
        //            exporter.WriteXlsToResponse();
        //            break;
        //        case 3:
        //            exporter.WriteRtfToResponse();
        //            break;
        //        case 4:
        //            exporter.WriteCsvToResponse();
        //            break;
        //    }
        //}
        //protected void drdCashBankExport_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Int32 Filter = int.Parse(drdCashBank.SelectedItem.Value.ToString());
        //    if (Filter != 0)
        //    {
        //        if (Session["exportval"] == null)
        //        {
        //            Session["exportval"] = Filter;
        //            bindexport(Filter);
        //        }
        //        else if (Convert.ToInt32(Session["exportval"]) != Filter)
        //        {
        //            Session["exportval"] = Filter;
        //            bindexport(Filter);
        //        }
        //    }
        //}

        #endregion
        //private void cmbSubAccount_OnCallback(object source, CallbackEventArgsBase e)
        //{
        //    FillSubAccountCombo(source as ASPxComboBox, Convert.ToString(e.Parameter));
        //}
        //protected void FillSubAccountCombo(ASPxComboBox cmb, string MainAccount)
        //{

        //    DataTable DT = GetSubAccount(MainAccount);
        //    cmb.Items.Clear();

        //    if (DT.Rows.Count != 0)
        //    {
        //        foreach (DataRow dr in DT.Rows)
        //        {

        //            cmb.Items.Add(Convert.ToString(dr["Contact_Name"]), Convert.ToString(dr["Column1"]));
        //        }
        //    }
        //}
        //protected DataTable GetSubAccount(string MainAccount)
        //{
        //    DataTable DT = new DataTable();
        //    DBEngine objEngine = new DBEngine();
        //    string MainAccountSelectedValue = MainAccount.Split('|')[0];
        //    string MainAccountSelectedItem = MainAccount.Split('|')[1];
        //    string RequestLetter = "%";

        //    var MainAccountCode = MainAccountSelectedValue;
        //    var SegID = "";
        //    var SegmentName = "";

        //    if (hdn_SegID_SegmentName.Value != "")
        //    {
        //        SegID = hdn_SegID_SegmentName.Value.Split('~')[0];
        //        SegmentName = hdn_SegID_SegmentName.Value.Split('~')[1];
        //    }
        //    var ProcedureName = "SubAccountSelect_New";
        //    var InputName = "CashBank_MainAccountID|clause|branch|exchSegment|SegmentN";
        //    var InputType = "V|V|V|V|V";
        //    var InputValue = MainAccountCode.ToString().Split('~')[0] + "|RequestLetter|" + Session["userbranchHierarchy"] + "|'" + Session["ExchangeSegmentID"] + "'|'" + SegmentName + "'";
        //    var SplitChar = "|";
        //    var CombinedSubQuery = ProcedureName + "$" + InputName + "$" + InputType + "$" + InputValue + "$" + SplitChar;
        //    string[] paramSub = CombinedSubQuery.Split('$');
        //    char SplitSubChar = Convert.ToChar(paramSub[4]);
        //    string ProcedureSubName = Convert.ToString(paramSub[0]);
        //    string[] InputSubName = paramSub[1].Split(SplitSubChar);
        //    string[] InputSubType = paramSub[2].Split(SplitSubChar);
        //    string SetRequestLetter = paramSub[3].Replace("RequestLetter", RequestLetter);
        //    string[] InputSubValue = SetRequestLetter.Split(SplitSubChar);
        //    if (ProcedureSubName.Trim() != String.Empty && (InputSubName.Length == InputSubType.Length) && (InputSubType.Length == InputSubValue.Length))
        //    {
        //        DT = objEngine.SelectProcedureArr(ProcedureSubName, InputSubName, InputSubType, InputSubValue);              

        //    }
        //    return DT;
        //}
        #region WebMethod

        [WebMethod]
        public static string GetTotalBalanceByCashBankID(string CashBankID, string CashBankVoucherID, String PostingDate)
        {
            string VoucherAmount = "0.00", BalanceLimit = "0.00", BalanceExceed = "", ClosingAmt = "0.00";

            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

            DataTable DT = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Fetch_CashBankEntry_DataSet");
            proc.AddVarcharPara("@Action", 500, "GetTotalBalanceByCashBankID");
            proc.AddVarcharPara("@CashBankCode", 200, CashBankID.Split('~')[0]);
            proc.AddVarcharPara("@CashBank_ID", 200, CashBankVoucherID);
            proc.AddVarcharPara("@CashBank", 50, CashBankID.Split('~')[1]);
            proc.AddVarcharPara("@PostingDate", 10, PostingDate);
            proc.AddVarcharPara("@CompanyID", 100, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 100, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddIntegerPara("@UserId", Convert.ToInt32(HttpContext.Current.Session["userid"]));
            proc.AddVarcharPara("@userbranchHierarchy", 50, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            DT = proc.GetTable();


            if (DT.Rows.Count > 0)
            {
                VoucherAmount = Convert.ToString(DT.Rows[0]["VoucherAmount"]);
                BalanceLimit = Convert.ToString(DT.Rows[1]["Cash_Bank_BalanceLimit"]);
                BalanceExceed = Convert.ToString(DT.Rows[1]["NegativeStock"]);
                ClosingAmt = Convert.ToString(DT.Rows[0]["ClosingAmt"]);
            }

            return Convert.ToString(VoucherAmount + "~" + BalanceLimit + "~" + BalanceExceed + "~" + ClosingAmt);
        }

        [WebMethod]
        public static string GetTotalBalanceByToDay(string MainAccountID)
        {
            string VoucherAmount = "0.00", DailyLimit = "0.00", DailyExceed = "";

            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

            DataTable DT = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Fetch_CashBankEntry_DataSet");
            proc.AddVarcharPara("@Action", 500, "GetTotalBalanceByToDay");
            proc.AddVarcharPara("@MainAccountID", 200, MainAccountID);
            // proc.AddVarcharPara("@CashBank_ID", 200, CashBankVoucherID);
            DT = proc.GetTable();


            if (DT.Rows.Count > 0)
            {
                VoucherAmount = Convert.ToString(DT.Rows[0]["PaymentAmount"]);
                if (DT.Rows.Count > 1)
                {
                    DailyLimit = Convert.ToString(DT.Rows[1]["DailtLimit"]);
                    DailyExceed = Convert.ToString(DT.Rows[1]["DailyLimitExceed"]);
                }
            }

            return Convert.ToString(VoucherAmount + "~" + DailyLimit + "~" + DailyExceed);
        }

        [WebMethod]
        public static string GetCurrentBankBalance(string MainAccountID, string BranchID)
        {
            BusinessLogicLayer.Converter oConverter = new BusinessLogicLayer.Converter();
            string MainAccountValID = string.Empty;
            string strColor = string.Empty;
            DataTable DT = new DataTable();
            DBEngine objEngine = new DBEngine();

            DT = objEngine.GetDataTable("Select Sum(AccountsLedger_AmountDr-AccountsLedger_AmountCr) TotalAmount from Trans_AccountsLedger WHERE AccountsLedger_MainAccountID='" + MainAccountID + "' and AccountsLedger_BranchId=" + BranchID);
            if (DT.Rows.Count != 0)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(DT.Rows[0]["TotalAmount"])))
                {
                    MainAccountValID = oConverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(DT.Rows[0]["TotalAmount"]));
                    strColor = Convert.ToDecimal(MainAccountValID) > 0 ? "White" : "Red";
                }
            }
            return MainAccountValID + "~" + strColor;
        }


        [WebMethod]
        public static object GETTDSDOCDETAILS(DateTime TDSPaymentDate, string TDSCode)
        {


            List<TDSPayeeDetails> tdsdet = new List<TDSPayeeDetails>();
            TDSPaymentBL tds = new TDSPaymentBL();
            DataTable DT = new DataTable();
            DT = tds.GetTDSPayment(TDSPaymentDate, TDSCode,"","","");

            tdsdet = (from DataRow dr in DT.Rows
                      select new TDSPayeeDetails()
                      {
                          DETID = Convert.ToString(dr["DETID"]),
                          VendorId = Convert.ToString(dr["VendorId"]),
                          MainAccountID = Convert.ToString(dr["MainAccountID"]),
                          PartyID = Convert.ToString(dr["PartyID"]),
                          TDSTCS_Code = Convert.ToString(dr["TDSTCS_Code"]),
                          PaymentDate = Convert.ToString(dr["PaymentDate"]),
                          Total_Tax = Convert.ToString(dr["Total_Tax"]),
                          Tax_Amount = Convert.ToString(dr["Tax_Amount"]),
                          Surcharge = Convert.ToString(dr["Surcharge"]),
                          EduCess = Convert.ToString(dr["EduCess"]),
                          DocumentNo = Convert.ToString(dr["Document_No"])

                      }).ToList();

            return tdsdet;
        }


        [WebMethod]
        public static object ShowTDSEditDetails(string doc_id)
        {

            TDSeditDetails tdss = new TDSeditDetails();
            List<TDSPayeeDetails> tdsdet = new List<TDSPayeeDetails>();
            TDSPaymentBL tds = new TDSPaymentBL();
            DataSet DT = new DataSet();
            DT = tds.GetTDSPaymentEdit(doc_id);

            if (DT.Tables[0] != null && DT.Tables[0].Rows.Count > 0)
            {
                tdss.BankBrach = Convert.ToString(DT.Tables[0].Rows[0]["BankBrach"]);
                tdss.BankName = Convert.ToString(DT.Tables[0].Rows[0]["BankName"]);
                tdss.BRS = Convert.ToString(DT.Tables[0].Rows[0]["BRS"]);
                tdss.ChallanNo = Convert.ToString(DT.Tables[0].Rows[0]["ChallanNo"]);
                tdss.DeductionON = Convert.ToString(DT.Tables[0].Rows[0]["DeductionON"]);
                tdss.Interest = Convert.ToString(DT.Tables[0].Rows[0]["Interest"]);
                tdss.LateFees = Convert.ToString(DT.Tables[0].Rows[0]["LateFees"]);
                tdss.Others = Convert.ToString(DT.Tables[0].Rows[0]["Others"]);
                tdss.Payment_Date = Convert.ToString(DT.Tables[0].Rows[0]["Payment_Date"]);
                tdss.Quater = Convert.ToString(DT.Tables[0].Rows[0]["Quater"]);
                tdss.SectionID = Convert.ToString(DT.Tables[0].Rows[0]["SectionID"]);
                tdss.Surcharge = Convert.ToString(DT.Tables[0].Rows[0]["Surcharge"]);
                tdss.Tax = Convert.ToString(DT.Tables[0].Rows[0]["Tax"]);
                tdss.Total = Convert.ToString(DT.Tables[0].Rows[0]["Total"]);

            }



            tdsdet = (from DataRow dr in DT.Tables[1].Rows
                      select new TDSPayeeDetails()
                      {
                          DETID = Convert.ToString(dr["DETID"]),
                          VendorId = Convert.ToString(dr["VendorId"]),
                          MainAccountID = Convert.ToString(dr["MainAccountID"]),
                          PartyID = Convert.ToString(dr["PartyID"]),
                          TDSTCS_Code = Convert.ToString(dr["TDSTCS_Code"]),
                          PaymentDate = Convert.ToString(dr["PaymentDate"]),
                          Total_Tax = Convert.ToString(dr["Total_Tax"]),
                          Tax_Amount = Convert.ToString(dr["Tax_Amount"]),
                          Surcharge = Convert.ToString(dr["Surcharge"]),
                          EduCess = Convert.ToString(dr["EduCess"])

                      }).ToList();
            tdss.tdsPayDet = tdsdet;
            return tdss;
        }


        public class TDSeditDetails
        {
            public string SectionID { get; set; }
            public string DeductionON { get; set; }
            public string Payment_Date { get; set; }
            public string Quater { get; set; }
            public string Surcharge { get; set; }
            public string Interest { get; set; }
            public string LateFees { get; set; }
            public string Total { get; set; }
            public string Tax { get; set; }
            public string Others { get; set; }
            public string BankName { get; set; }
            public string BankBrach { get; set; }
            public string BRS { get; set; }
            public string ChallanNo { get; set; }
            public List<TDSPayeeDetails> tdsPayDet { get; set; }
        }

        public class TDSPayeeDetails
        {
            public string DETID { get; set; }
            public string VendorId { get; set; }
            public string MainAccountID { get; set; }
            public string PartyID { get; set; }
            public string TDSTCS_Code { get; set; }
            public string PaymentDate { get; set; }
            public string Total_Tax { get; set; }
            public string Tax_Amount { get; set; }
            public string Surcharge { get; set; }
            public string EduCess { get; set; }
            public string DocumentNo { get; set; }
        }






        //[WebMethod]
        //public static List<string> GetBranchList()
        //{
        //    List<string> obj = new List<string>();
        //    BusinessLogicLayer.Converter oConverter = new BusinessLogicLayer.Converter();
        //    string MainAccountValID = string.Empty;
        //    string strColor = string.Empty;
        //    DataTable DT = new DataTable();
        //    DBEngine objEngine = new DBEngine();

        //    DT = objEngine.GetDataTable("SELECT BRANCH_id AS BANKBRANCH_ID , BRANCH_DESCRIPTION AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH where  BRANCH_id in(" + Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) + ")");
        //    if (DT.Rows.Count != 0)
        //    {



        //        if (DT.Rows.Count != 0)
        //        {
        //            foreach (DataRow dr in DT.Rows)
        //            {

        //                obj.Add(Convert.ToString(dr["BANKBRANCH_NAME"]) + "|" + Convert.ToString(dr["BANKBRANCH_ID"]));
        //            }
        //        }
        //    }


        //    return obj;
        //}
        //[WebMethod]
        //public static List<string> GetCashBankAccountList(string CombinedQuery, string BranchID)
        //{
        //    List<string> obj = new List<string>();
        //    DataTable DT = new DataTable();
        //    DBEngine objEngine = new DBEngine();
        //    string RequestLetter = "%";
        //    string[] param = CombinedQuery.Replace("--", "+").Replace("^^", "%").Split('$');
        //    string strQuery_Table = param[0].Trim() != String.Empty ? param[0] : null;
        //    string strQuery_FieldName = param[1].Trim() != String.Empty ? param[1] : null;
        //    string strQuery_WhereClause = param[2].Trim() != String.Empty ? param[2] : null;
        //    string strQuery_OrderBy = param[3].Trim() != String.Empty ? param[3] : null;
        //    string strQuery_GroupBy = param[4].Trim() != String.Empty ? param[4] : null;
        //    if (strQuery_Table != null)
        //    {
        //        strQuery_Table = strQuery_Table.Replace("RequestLetter", RequestLetter);
        //    }
        //    if (strQuery_FieldName != null)
        //    {
        //        strQuery_FieldName = strQuery_FieldName.Replace("RequestLetter", RequestLetter);
        //    }
        //    if (strQuery_WhereClause != null)
        //    {
        //        strQuery_WhereClause = strQuery_WhereClause.Replace("RequestLetter", RequestLetter);
        //    }

        //    DT = objEngine.GetDataTable(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_OrderBy, strQuery_GroupBy);

        //    DataTable restrictedDT = objEngine.GetDataTable("select branch_id,MainAccount_id from tbl_master_ledgerBranch_map");


        //    if (DT.Rows.Count != 0)
        //    {
        //        foreach (DataRow dr in DT.Rows)
        //        {
        //            if (BranchID.Trim() != "0")
        //            {
        //                DataRow[] restrictedTablerow = restrictedDT.Select("MainAccount_id=" + Convert.ToString(dr["MainAccount_ReferenceID"]));
        //                if (restrictedTablerow.Length > 0)
        //                {
        //                    DataTable restrictedTable = restrictedTablerow.CopyToDataTable();
        //                    DataRow[] restrictedRow = restrictedTable.Select("branch_id=" + BranchID);
        //                    if (restrictedRow.Length > 0)
        //                    {
        //                        obj.Add(Convert.ToString(dr["IntegrateMainAccount"]) + "|" + Convert.ToString(dr["MainAccount_AccountCode"]));
        //                    }
        //                }
        //                else
        //                {
        //                    obj.Add(Convert.ToString(dr["IntegrateMainAccount"]) + "|" + Convert.ToString(dr["MainAccount_AccountCode"]));
        //                }

        //            }
        //            else
        //            {
        //                obj.Add(Convert.ToString(dr["IntegrateMainAccount"]) + "|" + Convert.ToString(dr["MainAccount_AccountCode"]));
        //            }
        //        }



        //    }

        //    return obj;
        //}
        [WebMethod]
        public static string getSchemeType(string sel_scheme_id)
        {
            string strschematype = "", strschemalength = "", strschemavalue = "", strschemaBranch = "";

            // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

            DataTable DT = objEngine.GetDataTable("tbl_master_Idschema", " schema_type,length,Branch ", " Id = " + Convert.ToInt32(sel_scheme_id));

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                strschematype = Convert.ToString(DT.Rows[i]["schema_type"]);
                strschemalength = Convert.ToString(DT.Rows[i]["length"]);
                strschemaBranch = Convert.ToString(DT.Rows[i]["Branch"]);
                strschemavalue = strschematype + "~" + strschemalength + "~" + strschemaBranch;
            }
            return Convert.ToString(strschemavalue);
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
                status = objMShortNameCheckingBL.CheckUnique(Convert.ToString(VoucherNo).Trim(), "0", "CaskBankVoucher_Check");
            }
            return status;
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
        public static String CheckTdsByMainAccountID(string MainAccountID)
        {

            //   BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

            DataTable DT = objEngine.GetDataTable("Master_MainAccount", " MainAccount_TDSRate ", " MainAccount_ReferenceID = " + Convert.ToInt32(MainAccountID));
            string TDSRate = "";
            if (DT.Rows.Count > 0)
            {
                TDSRate = Convert.ToString(DT.Rows[0]["MainAccount_TDSRate"]);
                if (Convert.ToString(DT.Rows[0]["MainAccount_TDSRate"]) != "")
                {
                    TDSRate = "YES";
                }
                else
                {
                    TDSRate = "NO";
                }
            }

            return TDSRate;
        }
        #endregion
        private void bindMainAccount(object source, CallbackEventArgsBase e)
        {
            if (e.Parameter != "")
            {
                ASPxComboBox currentCombo = source as ASPxComboBox;
                currentCombo.DataSource = GetMainAccountForBatchByBranchID(e.Parameter);
                currentCombo.DataBind();
            }

        }
        protected void gridTDS_BatchUpdate(object sender, ASPxDataBatchUpdateEventArgs e)
        {
            gridTDS.JSProperties["cpVouvherNo"] = "";
            gridTDS.JSProperties["cpSaveSuccessOrFail"] = null;
            string ValidateSubAccount = "";
            string Action = Convert.ToString(hdnMode.Value);
            DataTable TdsDt = new DataTable();


            if (Action == "0")
            {
                TdsDt.Columns.Add("CashReportID", typeof(string));
                TdsDt.Columns.Add("MainAccount", typeof(string));
                TdsDt.Columns.Add("SubAccount", typeof(string));
                TdsDt.Columns.Add("WithDrawl", typeof(string));
                TdsDt.Columns.Add("Receipt", typeof(string));
                TdsDt.Columns.Add("Narration", typeof(string));
                TdsDt.Columns.Add("Status", typeof(string));
                TdsDt.Columns.Add("IsTDS", typeof(string));
                TdsDt.Columns.Add("UniqueID", typeof(string));
                TdsDt.Columns.Add("IsTDSSource", typeof(string));
                TdsDt.Columns.Add("TDSPercentage", typeof(string));

                TdsDt.Columns.Add("ProjectId", typeof(Int64));
                TdsDt.Columns.Add("Project_Code", typeof(string));
            }
            else
            {
                string VoucherNo = Convert.ToString(Session["VoucherNumber"]);
                string IBRef = Convert.ToString(Session["VoucherIBRef"]);
                if (Convert.ToString(Session["ErrorMsg"]) != "HasError")
                {

                    TdsDt = GetCashBanktBatchEditData(hdnEditRfid.Value, "").Tables[0]; ;
                    foreach (DataRow dr in TdsDt.Rows)
                    {
                        dr["MainAccount"] = Convert.ToString(dr["gvColMainAccount"]);
                        dr["SubAccount"] = Convert.ToString(dr["gvColSubAccount"]);
                        dr["TDSPercentage"] = Convert.ToString(dr["TDSPercentage"]).Split('~')[0];
                    }
                    TdsDt.Columns.Remove("gvColMainAccount");
                    TdsDt.Columns.Remove("gvColSubAccount");
                    TdsDt.Columns.Remove("gvMainAcCode");
                    TdsDt.Columns.Remove("IsSubledger");
                }
                else
                {
                    TdsDt.Columns.Add("CashReportID", typeof(string));
                    TdsDt.Columns.Add("MainAccount", typeof(string));
                    TdsDt.Columns.Add("SubAccount", typeof(string));
                    TdsDt.Columns.Add("WithDrawl", typeof(string));
                    TdsDt.Columns.Add("Receipt", typeof(string));
                    TdsDt.Columns.Add("Narration", typeof(string));
                    TdsDt.Columns.Add("Status", typeof(string));
                    TdsDt.Columns.Add("IsTDS", typeof(string));
                    TdsDt.Columns.Add("UniqueID", typeof(string));
                    TdsDt.Columns.Add("IsTDSSource", typeof(string));
                    TdsDt.Columns.Add("TDSPercentage", typeof(string));

                    TdsDt.Columns.Add("ProjectId", typeof(Int64));
                    TdsDt.Columns.Add("Project_Code", typeof(string));
                }
            }

            foreach (var args in e.InsertValues)
            {
                string MainAccount = Convert.ToString(args.NewValues["gvColMainAccountTDS"]);
                string SubAccount = Convert.ToString(args.NewValues["gvColSubAccountTDS"]);
                string WithDrawl = Convert.ToString(args.NewValues["WithDrawlTDS"]);
                string Receipt = Convert.ToString(args.NewValues["ReceiptTDS"]);
                string Narration = Convert.ToString(args.NewValues["NarrationTDS"]);

                string IsTDS = Convert.ToString(args.NewValues["IsTDS"]);
                string UniqueID = Convert.ToString(args.NewValues["UniqueID"]);
                string IsTDSSource = Convert.ToString(args.NewValues["IsTDSSource"]);
                string TDSPercentage = Convert.ToString(args.NewValues["TDSPercentage"]);

                Int64 ProjectId = Convert.ToInt64(args.NewValues["ProjectId"]);
                string Project_Code = Convert.ToString(args.NewValues["Project_Code"]);


                if ((Convert.ToDecimal(WithDrawl) > 0 && MainAccount != "" && MainAccount != null) || (Convert.ToDecimal(Receipt) > 0 && MainAccount != "" && MainAccount != null))
                {
                    TdsDt.Rows.Add("0", MainAccount, SubAccount, WithDrawl, Receipt, Narration, "I", IsTDS, UniqueID, IsTDSSource, TDSPercentage.Split('~')[0], ProjectId, Project_Code);
                }

            }

            foreach (var args in e.UpdateValues)
            {
                string CashReportID = Convert.ToString(args.Keys["CashReportID"]);
                string MainAccount = Convert.ToString(args.NewValues["gvColMainAccountTDS"]);
                string SubAccount = Convert.ToString(args.NewValues["gvColSubAccountTDS"]);
                string WithDrawl = Convert.ToString(args.NewValues["WithDrawlTDS"]);
                string Receipt = Convert.ToString(args.NewValues["ReceiptTDS"]);
                string Narration = Convert.ToString(args.NewValues["NarrationTDS"]);

                string IsTDS = Convert.ToString(args.NewValues["IsTDS"]);
                string UniqueID = Convert.ToString(args.NewValues["UniqueID"]);
                string IsTDSSource = Convert.ToString(args.NewValues["IsTDSSource"]);
                string TDSPercentage = Convert.ToString(args.NewValues["TDSPercentage"]);

                Int64 ProjectId = Convert.ToInt64(args.NewValues["ProjectId"]);
                string Project_Code = Convert.ToString(args.NewValues["Project_Code"]);



                bool isDeleted = false;

                foreach (var arg in e.DeleteValues)
                {
                    string DeleteID = Convert.ToString(arg.Keys["CashReportID"]);
                    if (DeleteID == CashReportID)
                    {
                        isDeleted = true;
                        break;
                    }
                }
                TdsDt.AcceptChanges();

                if (isDeleted == false)
                {
                    if ((Convert.ToDecimal(WithDrawl) > 0 && MainAccount != "" && MainAccount != null) || (Convert.ToDecimal(Receipt) > 0 && MainAccount != "" && MainAccount != null))
                    {

                        if (Convert.ToString(Session["ErrorMsg"]) == "")
                        {
                            DataRow dr = TdsDt.Select("CashReportID ='" + CashReportID + "'").FirstOrDefault(); // finds all rows with id==2 and selects first or null if haven't found any

                            if (dr != null)
                            {
                                dr["MainAccount"] = MainAccount;
                                dr["SubAccount"] = SubAccount;
                                dr["WithDrawl"] = WithDrawl;
                                dr["Receipt"] = Receipt;
                                dr["Narration"] = Narration;
                                dr["Status"] = "U";
                                dr["IsTDS"] = IsTDS;
                                dr["UniqueID"] = UniqueID;
                                dr["IsTDSSource"] = IsTDSSource;
                                dr["TDSPercentage"] = TDSPercentage.Split('~')[0];
                                dr["ProjectId"] = ProjectId;
                                dr["Project_Code"] = Project_Code;


                            }

                        }
                        else

                            TdsDt.Rows.Add("0", MainAccount, SubAccount, WithDrawl, Receipt, Narration, "I", IsTDS, UniqueID, IsTDSSource, TDSPercentage.Split('~')[0], ProjectId, Project_Code);
                    }
                }
            }

            foreach (var args in e.DeleteValues)
            {
                string CashReportID = Convert.ToString(args.Keys["CashReportID"]);
                DataRow dr = TdsDt.Select("CashReportID ='" + CashReportID + "'").FirstOrDefault(); // finds all rows with id==2 and selects first or null if haven't found any
                if (dr != null)
                {
                    dr["Status"] = "D";
                }
            }

            string validate = "";

            if (hdn_Mode.Value == "Entry")
            {
                string strSchemeType = Convert.ToString(CmbScheme.Value);
                string[] SchemeList = strSchemeType.Split(new string[] { "~" }, StringSplitOptions.None);

                validate = checkNMakeJVCode(Convert.ToString(txtVoucherNo.Text), Convert.ToInt32(SchemeList[0]));
            }


            if (validate == "outrange" || validate == "duplicate" || validate == "HasError")
            {
                gridTDS.JSProperties["cpSaveSuccessOrFail"] = validate;
            }
            else if (ValidateSubAccount == "Subaccountmandatory")
            {
                gridTDS.JSProperties["cpSaveSuccessOrFail"] = ValidateSubAccount;
            }
            else
            {
                if (Action == "0")
                {
                    DataTable dtSchema = oDBEngine.GetDataTable("tbl_master_idschema", "prefix, suffix, digit, startno, schema_type", "id=" + Convert.ToInt32(CmbScheme.Value.ToString().Split('~')[0]));
                    int scheme_type = Convert.ToInt32(dtSchema.Rows[0]["schema_type"]);
                    if (scheme_type != 0)
                    {
                        gridTDS.JSProperties["cpVouvherNo"] = JVNumStr;
                    }
                }

                string ActionType = "", strIBRef = "", strScheme_ID = "";
                if (hdn_Mode.Value == "Entry")
                {
                    string strSchemeType = Convert.ToString(CmbScheme.Value);
                    string[] SchemeList = strSchemeType.Split(new string[] { "~" }, StringSplitOptions.None);

                    strScheme_ID = SchemeList[0];

                    DataTable dtSchema = oDBEngine.GetDataTable("tbl_master_idschema", "prefix, suffix, digit, startno, schema_type", "id=" + Convert.ToInt32(SchemeList[0]));
                    if (dtSchema != null && dtSchema.Rows.Count > 0)
                    {
                        int scheme_type = Convert.ToInt32(dtSchema.Rows[0]["schema_type"]);
                        if (scheme_type != 0)
                        {
                            gridTDS.JSProperties["cpVouvherNo"] = JVNumStr;
                        }
                        ActionType = "ADD";
                        strIBRef = "";
                    }
                }
                else
                {
                    ActionType = "Edit";
                    strIBRef = Convert.ToString(Session["IBRef"]);
                }

                string strEditCashBankID = Convert.ToString(hdnEditCBID.Value);
                string strCashBankBranchID = Convert.ToString(ddlBranch.SelectedValue);

                string strEnterBranchID = Convert.ToString(ddlEnterBranch.SelectedValue);

                string strTransactionDate = Convert.ToString(dtTDate.Date);
                string strCBankID = Convert.ToString(ddlCashBank.Value);
                string strCashBankID = strCBankID.Split('~')[0];
                string strExchangeSegmentID = "1";
                string strTransactionType = Convert.ToString(rbtnType.SelectedValue);
                string strEntryUserProfile = "F";
                string strNarration = txtNarration.Text;
                string strCurrency = Convert.ToString(CmbCurrency.Value);
                string strInstrumentType;
                if (Convert.ToString(cmbInstrumentType.Value) == "CH")
                {
                    strInstrumentType = Convert.ToString(0);
                }
                else
                {
                    strInstrumentType = Convert.ToString(cmbInstrumentType.Value);
                }

                CommonBL cbl = new CommonBL();
                string ProjectSelectcashbankModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
                Int64 ProjId = 0;
                if (lookup_Project.Text != "")
                {
                    string projectCode = lookup_Project.Text;
                    DataTable dt = oDbEngine.GetDataTable("select Proj_Id from Master_ProjectManagement where Proj_Code='" + projectCode + "'");
                    ProjId = Convert.ToInt64(dt.Rows[0]["Proj_Id"]);
                }
                else if (lookup_Project.Text == "")
                {
                    //if (ProjectSelectcashbankModule.ToUpper().Trim() == "NO")
                    //{
                    //    DataTable dtproj = GetProjectEditData(hdnEditRfid.Value);
                    //    if (dtproj != null)
                    //    {
                    //        ProjId = Convert.ToInt64(dtproj.Rows[0]["Proj_Id"]);
                    //    }
                    //    else
                    //    {
                    //        ProjId = 0;
                    //    }

                    //}
                    //else
                    //{
                    ProjId = 0;
                    //}

                }


                string strInstrumentNumber = txtInstNobth.Text.Trim();
                string strInstrumentDate = Convert.ToString(InstDate.Date);
                string strrate = txtRate.Text.Trim();
                string strReceivedFrom = txtReceivedFrom.Text.Trim();
                string strPaidTo = txtPaidTo.Text.Trim();
                string strCashBankName = Convert.ToString(ddlCashBank.Text);
                string strTaxOption = Convert.ToString(ddl_AmountAre.SelectedValue);
                string strContactNo = Convert.ToString(txtContact.Text.Trim());
                #region ##### Added By : Samrat Roy -- to get BillingShipping user control data
                DataTable tempBillAddress = new DataTable();
                //chinmoy commeented for lightweight billingshiping
                //tempBillAddress = BillingShippingControl.SaveBillingShippingControlData();
                tempBillAddress = Purchase_BillingShipping.GetBillingShippingTable();
                #endregion

                int i = 1;
                foreach (DataRow Dr in TdsDt.Rows)
                {

                    Dr["CashReportID"] = i;
                    i = i + 1;
                }
                TdsDt.AcceptChanges();
                if (TdsDt.Rows.Count > 0)
                {
                 int id=  Save_Record(ActionType, strEditCashBankID, JVNumStr, strCashBankBranchID, strTransactionDate, strCashBankID, strExchangeSegmentID, strTransactionType, strEntryUserProfile
                                                   , strNarration, ProjId, strIBRef, strCurrency, strInstrumentType, strInstrumentNumber, strInstrumentDate, strCashBankName, strrate,
                                                   TdsDt, strReceivedFrom, strPaidTo, tempBillAddress, strTaxOption, strContactNo,
                                                   (chk_reversemechenism.Checked == true) ? 1 : 0, null, strScheme_ID, txtDraweeBank.Text.Trim(), strEnterBranchID);
                 if (id>0)
                    {

                        hdnEditRfid.Value = "";
                        hdn_Mode.Value = "";
                        gridTDS.JSProperties["cpSaveSuccessOrFail"] = "successInsert";
                        Session["CashBankDetails"] = null;
                        Session["CB_FinalTaxRecord"] = null;


                        #region To Show By Default Cursor after SAVE AND NEW
                        if (ActionType == "ADD") // session has been removed from quotation list page working good
                        {

                            string schemavalue = Convert.ToString(CmbScheme.Value);
                            Session["schemavalueCB"] = schemavalue;
                            Session["VoucherTypeCB"] = strTransactionType;


                            List<string> myList = new List<string> { strTaxOption, strCashBankBranchID };
                            //List<string> myList = new List<string>{strCashBankID, strInstrumentType, strTaxOption};
                            Session["SaveNewValues"] = myList;

                            string schematype = Convert.ToString(txtVoucherNo.Text);
                            if (schematype == "Auto")
                            {
                                Session["SaveModeCB"] = "A";
                                hdnSchemaType.Value = Convert.ToString("1");
                            }
                            else
                            {
                                Session["SaveModeCB"] = "M";
                            }
                        }

                        #endregion
                    }
                     else if(id==-12)
                    {
                        gridTDS.JSProperties["cpSaveSuccessOrFail"] = "EmptyProject";
                    }
                 else if (id == -9)
                 {
                     DataTable dts = new DataTable();
                     dts = GetAddLockStatus();
                     gridTDS.JSProperties["cpSaveSuccessOrFail"] = "AddLock";
                     gridTDS.JSProperties["cpAddLockStatus"] = (Convert.ToString(dts.Rows[0]["Lock_Fromdate"]) + " to " + Convert.ToString(dts.Rows[0]["Lock_Todate"]));
                 }
                    else
                    {
                        gridTDS.JSProperties["cpSaveSuccessOrFail"] = "errorInsert";
                    }
                }
            }
        }

        public DataTable GetAddLockStatus()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Fetch_CashBankEntry_DataSet");
            proc.AddVarcharPara("@Action", 500, "GetAddLockForCashbank");

            dt = proc.GetTable();
            return dt;

        }

        //chinmoy added for inline project code start 10-12-2019
        protected void ProjectCodeCallback_Callback(object sender, CallbackEventArgsBase e)
        {
            DataTable dtDocFroProject = new DataTable();
            string Projectid = e.Parameter.Split('~')[1];
            //string DocNo = e.Parameter.Split('~')[2];
            if (hdnProjectSelectInEntryModule.Value == "1")
            {

                if (Projectid != "0")
                {
                    dtDocFroProject = GetProjectCodeDetailsOnDocument(Projectid);
                }
                else
                {
                    dtDocFroProject = GetProjectCodeDetailsOnDocument();
                }
            }
            else
            {
                dtDocFroProject = GetProjectCodeDetailsOnDocument();
            }

            Session["VendorPayRecProjectCodefromDoc"] = dtDocFroProject;
            if (dtDocFroProject != null && dtDocFroProject.Rows.Count > 0)
            {
                lookupPopup_ProjectCode.DataSource = dtDocFroProject;
                lookupPopup_ProjectCode.DataBind();
            }
            else
            {
                lookupPopup_ProjectCode.DataSource = null;
                lookupPopup_ProjectCode.DataBind();
            }

        }

        //chinmoy added for projecvt code start 10-12-2019
        protected void lookup_ProjectCode_DataBinding(object sender, EventArgs e)
        {
            DataTable dsdata = (DataTable)Session["VendorPayRecProjectCodefromDoc"];
            lookupPopup_ProjectCode.DataSource = dsdata;
        }

        //End




        protected string checkNMakeJVCode(string manual_str, int sel_schema_Id)
        {
            //  oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            oDbEngine = new BusinessLogicLayer.DBEngine();

            DataTable dtSchema = new DataTable();
            DataTable dtC = new DataTable();
            string prefCompCode = string.Empty, sufxCompCode = string.Empty, startNo, paddedStr, sqlQuery = string.Empty;
            int EmpCode, prefLen, sufxLen, paddCounter;
            bool suppressZero = false;

            if (sel_schema_Id > 0)
            {
                dtSchema = oDbEngine.GetDataTable("tbl_master_idschema", "prefix, suffix, digit, startno, schema_type,suppressZero", "id=" + sel_schema_Id);
                int scheme_type = Convert.ToInt32(dtSchema.Rows[0]["schema_type"]);

                if (scheme_type != 0)
                {
                    startNo = dtSchema.Rows[0]["startno"].ToString();
                    paddCounter = Convert.ToInt32(dtSchema.Rows[0]["digit"]);
                    paddedStr = startNo.PadLeft(paddCounter, '0');
                    prefCompCode = (dtSchema.Rows[0]["prefix"].ToString() == "TCURDATE/") ? (dtTDate.Date.ToString("ddMMyyyy") + "/") : (dtSchema.Rows[0]["prefix"].ToString());
                    sufxCompCode = (dtSchema.Rows[0]["suffix"].ToString() == "/TCURDATE") ? ("/" + dtTDate.Date.ToString("ddMMyyyy")) : (dtSchema.Rows[0]["suffix"].ToString());
                    prefLen = Convert.ToInt32(prefCompCode.Length);
                    sufxLen = Convert.ToInt32(sufxCompCode.Length);
                    suppressZero = Convert.ToBoolean(dtSchema.Rows[0]["suppressZero"]);

                    if (!suppressZero)
                    {

                        sqlQuery = "SELECT max(tjv.CashBank_VoucherNumber) FROM Trans_CashBankVouchers tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + paddCounter + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.CashBank_VoucherNumber))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.CashBank_VoucherNumber))) = 1 and CashBank_VoucherNumber like '" + prefCompCode + "%'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, CashBank_CreateDateTime) = CONVERT(DATE, GETDATE())";
                        dtC = oDbEngine.GetDataTable(sqlQuery);
                    }
                    else
                    {

                        //sqlQuery = "SELECT max(tjv.CashBank_VoucherNumber) FROM Trans_CashBankVouchers tjv WHERE dbo.RegexMatch('";
                        //if (prefLen > 0)
                        //    sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        //sqlQuery += "[0-9]*";
                        //if (sufxLen > 0)
                        //    sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        ////sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.CashBank_VoucherNumber))) = 1 and CashBank_VoucherNumber like '" + prefCompCode + "%'";
                        //dtC = oDBEngine.GetDataTable(sqlQuery);

                        int i = startNo.Length;
                        while (i < paddCounter)
                        {


                            sqlQuery = "SELECT max(tjv.CashBank_VoucherNumber) FROM Trans_CashBankVouchers tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            sqlQuery += "[0-9]{" + i + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.CashBank_VoucherNumber))) = 1 and CashBank_VoucherNumber like '" + prefCompCode + "%'";

                            if (prefLen == 0 && sufxLen == 0)
                            {
                                sqlQuery += " and LEN(tjv.CashBank_VoucherNumber)=" + i;
                            }

                            dtC = oDBEngine.GetDataTable(sqlQuery);
                            if (dtC.Rows[0][0].ToString() == "")
                            {
                                break;
                            }
                            i++;
                        }
                        if (i != 1)
                        {
                            sqlQuery = "SELECT max(tjv.CashBank_VoucherNumber) FROM Trans_CashBankVouchers tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            sqlQuery += "[0-9]{" + (i - 1) + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.CashBank_VoucherNumber)) = 1 and CashBank_VoucherNumber like '" + prefCompCode + "%'";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.CashBank_VoucherNumber))) = 1 and CashBank_VoucherNumber like '" + prefCompCode + "%'";

                            if (prefLen == 0 && sufxLen == 0)
                            {
                                sqlQuery += " and LEN(tjv.CashBank_VoucherNumber)=" + (i - 1);
                            }
                            dtC = oDBEngine.GetDataTable(sqlQuery);
                        }


                    }
                    if (dtC.Rows[0][0].ToString() == "")
                    {
                        sqlQuery = "SELECT max(tjv.CashBank_VoucherNumber) FROM Trans_CashBankVouchers tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.CashBank_VoucherNumber))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.CashBank_VoucherNumber))) = 1 and CashBank_VoucherNumber like '" + prefCompCode + "%'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, CashBank_CreateDateTime) = CONVERT(DATE, GETDATE())";

                        if (prefLen == 0 && sufxLen == 0)
                        {
                            sqlQuery += " and LEN(tjv.CashBank_VoucherNumber)=" + (paddCounter - 1);
                        }
                        dtC = oDbEngine.GetDataTable(sqlQuery);
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
                            if (!suppressZero)
                                paddedStr = EmpCode.ToString().PadLeft(paddCounter, '0');
                            else
                                paddedStr = EmpCode.ToString();


                            JVNumStr = prefCompCode + paddedStr + sufxCompCode;
                            return "ok";
                        }
                    }
                    else
                    {
                        if (!suppressZero)
                            paddedStr = startNo.PadLeft(paddCounter, '0');
                        else
                            paddedStr = startNo;
                        JVNumStr = prefCompCode + paddedStr + sufxCompCode;
                        return "ok";
                    }
                }
                else
                {
                    sqlQuery = "SELECT CashBank_VoucherNumber FROM Trans_CashBankVouchers WHERE CashBank_VoucherNumber LIKE '" + manual_str.Trim() + "'";
                    dtC = oDbEngine.GetDataTable(sqlQuery);
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



        public int  Save_Record(string ActionType, string strEditCashBankID, string strVoucherNumber, string strCashBankBranchID, string strTransactionDate,
  string strCashBankID,
   string strExchangeSegmentID, string strTransactionType, string strEntryUserProfile, string strNarration, Int64 ProjId, string strIBRef,
string strCurrency, string strInstrumentType, string strInstrumentNumber, string strInstrumentDate, string strCashBankName, string strrate,
DataTable strCashBankdt, string strReceivedFrom, string strPaidTo, DataTable tempBillAddress, string strTaxOption, string strContactNo, int reverseMechanism,
    DataTable reverseTaxTable, string strScheme_ID, string strDraweeBank, string strEnterBranchID

    )
        {
            try
            {
                gridTDS.JSProperties["cpExitNew"] = null;
                gridTDS.JSProperties["cpType"] = strTransactionType;
                if (hdn_Mode.Value == "Entry")
                {

                    strIBRef = "CB_" + Session["userid"].ToString() + "_" + strTransactionType + "_" + JVNumStr.Replace("/", "");

                }
                //Rev rajdip
                int refcashfundid = 0;
                if (lookup_CashFund.Text.ToString() != "" && lookup_CashFund.Text.ToString() != null)
                {
                    refcashfundid = Convert.ToInt32(lookup_CashFund.Value);
                }
                //End Rev Rajdip
                DataSet dsInst = new DataSet();
                //  SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                SqlCommand cmd = new SqlCommand("prc_CaskBankTDSInsert", con);


                DataTable dtCashBTds = new DataTable();
                dtCashBTds = GetCashbankTDSProjectDataSource();

                foreach (DataRow dr in strCashBankdt.Rows)
                {
                    dtCashBTds.Rows.Add(Convert.ToString(dr["CashReportID"]), Convert.ToString(dr["MainAccount"]), Convert.ToString(dr["SubAccount"]), Convert.ToDecimal(dr["WithDrawl"]),
                       Convert.ToString(dr["Receipt"]), Convert.ToString(dr["Narration"]), Convert.ToString(dr["Status"]), Convert.ToString(dr["IsTDS"]), Convert.ToString(dr["UniqueID"]), Convert.ToString(dr["IsTDSSource"]), Convert.ToString(dr["TDSPercentage"]));
                }

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", ActionType);//1
                cmd.Parameters.AddWithValue("@EditCashBankID", strEditCashBankID);//2
                cmd.Parameters.AddWithValue("@VoucherNumber", strVoucherNumber);//3

                cmd.Parameters.AddWithValue("@CompanyID", Convert.ToString(Session["LastCompany"]));//4
                cmd.Parameters.AddWithValue("@FinYear", Convert.ToString(Session["LastFinYear"]));//5
                cmd.Parameters.AddWithValue("@CreateUser", Convert.ToString(Session["userid"]));//6

                cmd.Parameters.AddWithValue("@CashBankBranchID", strCashBankBranchID);//7
                cmd.Parameters.AddWithValue("@TransactionDate", strTransactionDate);//8
                cmd.Parameters.AddWithValue("@CashBankCashBankID", strCashBankID);//9

                cmd.Parameters.AddWithValue("@ExchangeSegmentID", strExchangeSegmentID);//10
                cmd.Parameters.AddWithValue("@TransactionType", strTransactionType);//11
                cmd.Parameters.AddWithValue("@EntryUserProfile", strEntryUserProfile);//12

                cmd.Parameters.AddWithValue("@Narration", strNarration);//13
                cmd.Parameters.AddWithValue("@IBRef", strIBRef);//14
                cmd.Parameters.AddWithValue("@CurrencyID", strCurrency);//15

                cmd.Parameters.AddWithValue("@CashBankDetailInstrumentType", strInstrumentType);//16
                cmd.Parameters.AddWithValue("@CashBankDetailInstrumentNumber", strInstrumentNumber);//17
                cmd.Parameters.AddWithValue("@CashBankDetailInstrumentDate", strInstrumentDate);//18
                cmd.Parameters.AddWithValue("@AccountsLedgerCashBankName", strCashBankName);//19
                cmd.Parameters.AddWithValue("@rate", strrate);         //20
                cmd.Parameters.AddWithValue("@CashBankVoucherDetails", dtCashBTds);//21
                cmd.Parameters.AddWithValue("@CashBankVoucherDetailsForProject", strCashBankdt);//21
                cmd.Parameters.AddWithValue("@CashBankReceivedFrom", strReceivedFrom);//22
                cmd.Parameters.AddWithValue("@CashBankPaidTo", strPaidTo);//23
                cmd.Parameters.AddWithValue("@BillAddress", tempBillAddress);//24
                cmd.Parameters.AddWithValue("@TaxOption", strTaxOption);//24
                cmd.Parameters.AddWithValue("@ReverseMechanism", reverseMechanism);//24
                cmd.Parameters.AddWithValue("@EnterBranchID", strEnterBranchID); //25
                cmd.Parameters.AddWithValue("@ContactNo", strContactNo);//24
                cmd.Parameters.AddWithValue("@Scheme_ID", strScheme_ID);//25
                cmd.Parameters.AddWithValue("@DraweeBank", strDraweeBank);//26
                cmd.Parameters.AddWithValue("@Project_Id", ProjId);
                //Rev Rajdip
                cmd.Parameters.AddWithValue("@Paymentreqhead_Id", refcashfundid);
                //End Rev Rajdip
                SqlParameter output = new SqlParameter("@ReturnValueID", SqlDbType.Int);
                output.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(output);
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                int savestatus = Convert.ToInt32((output.Value).ToString());
                cmd.Dispose();
                con.Dispose();

                gridTDS.JSProperties["cpAutoID"] = output.Value;

                return savestatus;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }



        protected void gridTDS_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "MainAccountTDS")
            {
                e.Editor.ReadOnly = true;
                e.Editor.Focus();
            }
            else if (e.Column.FieldName == "bthSubAccountTDS")
            {
                e.Editor.ReadOnly = true;
            }
            else
            {
                e.Editor.ReadOnly = false;
            }

            if (hdnProjectSelectInEntryModule.Value == "1")
            {
                if (e.Column.FieldName == "Project_Code")
                {
                    e.Editor.ReadOnly = true;
                }
            }
        }

        protected void gridTDS_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string type = Convert.ToString(e.Parameters.Split('~')[0]);
            gridTDS.JSProperties["cpSaveSuccessOrFail"] = null;

            if (type == "Edit")
            {
                int RowIndex = Convert.ToInt32(e.Parameters.Split('~')[1]);
                string VoucherNumber = "";
                string IBRef = "";
                hdnJournalNo.Value = VoucherNumber;
                hdnIBRef.Value = IBRef;
                Session["VoucherNumber"] = VoucherNumber;
                Session["VoucherIBRef"] = IBRef;

                DataTable Voucherdt = GetJournalDetailsTDS("Details", VoucherNumber, IBRef);
                string Credit = Convert.ToString(Voucherdt.Compute("Sum(Receipt)", ""));
                string Debit = Convert.ToString(Voucherdt.Compute("Sum(WithDrawl)", ""));


                DataTable Detailsdt = GetJournalDetailsTDS("Header", VoucherNumber, IBRef);
                if (Detailsdt != null && Detailsdt.Rows.Count > 0)
                {
                    string BranchId = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_BranchID"]);
                    string BillNumber = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_BillNumber"]);
                    string TransactionDate = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_TransactionDate"]);
                    string JvNarration = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_Narration"]);
                    string PlaceOfSupply = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_PlaceOfSupply"]);
                    string Taxoption = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_TaxOption"]);
                    string IsPartyJournal = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_IsPartyJournal"]);
                    string PartyCount = Convert.ToString(Detailsdt.Rows[0]["PartyCount"]);
                    string IsRCMD = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_IsRCM"]);
                    //hdnIsPartyLedger.Value = PartyCount;

                    gridTDS.JSProperties["cpEdit"] = BillNumber + "~" + JvNarration + "~" + BranchId + "~" + Credit + "~" + Debit + "~" + TransactionDate + "~" + PlaceOfSupply + "~" + Taxoption + "~" + IsPartyJournal + "~" + PartyCount + "~" + IsRCMD;
                }

                gridTDS.DataSource = GetVoucherTDS();
                gridTDS.DataBind();


            }


            else if (type == "View")
            {

                string VoucherNumber = e.Parameters.Split('~')[1];
                //string IBRef = GvJvSearch.GetRowValues(0, "IBRef").ToString();

                hdnJournalNo.Value = VoucherNumber;
                // hdnIBRef.Value = IBRef;


                // Session["VoucherNumber"] = VoucherNumber;
                // Session["VoucherIBRef"] = IBRef;

                DataTable Voucherdt = GetJournalDetailsTDS("DetailsTagging", VoucherNumber, "0");

                string Credit = Convert.ToString(Voucherdt.Compute("Sum(Receipt)", ""));
                string Debit = Convert.ToString(Voucherdt.Compute("Sum(WithDrawl)", ""));


                ///   DataTable Detailsdt = GetJournalDetails("Header", VoucherNumber, IBRef);

                DataTable Detailsdt = GetJournalDetailsTDS("HeaderTagging", VoucherNumber, "0");



                if (Detailsdt != null && Detailsdt.Rows.Count > 0)
                {
                    string BranchId = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_BranchID"]);
                    string BillNumber = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_BillNumber"]);
                    string TransactionDate = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_TransactionDate"]);
                    string JvNarration = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_Narration"]);
                    string PlaceOfSupply = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_PlaceOfSupply"]);
                    string Taxoption = Convert.ToString(Detailsdt.Rows[0]["JournalVoucher_TaxOption"]);

                    gridTDS.JSProperties["cpEdit"] = BillNumber + "~" + JvNarration + "~" + BranchId + "~" + Credit + "~" + Debit + "~" + TransactionDate + "~" + PlaceOfSupply + "~" + Taxoption;
                }




                DataTable Voucherdt1 = GetJournalDetailsTDS("DetailsTagging", VoucherNumber, "0");
                Session["TagViewJournal"] = Voucherdt1;

                //gridTDS.DataSource = GetVoucherTagging(VoucherNumber, Voucherdt1);
                //gridTDS.DataBind();
                Session.Remove("TagViewJournal");

            }
            else if (type == "BlanckEdit")
            {
                gridTDS.DataSource = null;
                gridTDS.DataBind();

            }
        }

        public IEnumerable GetVoucherTDS()
        {
            DataSet DsOnLoad = new DataSet();
            DataTable tempdt = new DataTable();
            List<VOUCHERLISTTDS> VoucherList = new List<VOUCHERLISTTDS>();

            string VoucherNumber = Convert.ToString(hdnJournalNo.Value);
            string IBRef = Convert.ToString(hdnIBRef.Value);
            DataTable Voucherdt = GetJournalDetailsTDS("Details", VoucherNumber, IBRef);

            if (Voucherdt.Rows.Count > 0 && Voucherdt != null)
            {
                for (int i = 0; i < Voucherdt.Rows.Count; i++)
                {
                    VOUCHERLISTTDS Vouchers = new VOUCHERLISTTDS();
                    Vouchers.CashReportID = Convert.ToString(Voucherdt.Rows[i]["CashReportID"]);
                    Vouchers.MainAccountTDS = Convert.ToString(Voucherdt.Rows[i]["MainAccount"]).Trim();
                    Vouchers.bthSubAccountTDS = Convert.ToString(Voucherdt.Rows[i]["bthSubAccount"]).Trim();
                    Vouchers.WithDrawlTDS = Convert.ToString(Voucherdt.Rows[i]["WithDrawl"]);
                    Vouchers.ReceiptTDS = Convert.ToString(Voucherdt.Rows[i]["Receipt"]);
                    Vouchers.NarrationTDS = Convert.ToString(Voucherdt.Rows[i]["Narration"]);
                    Vouchers.gvColMainAccountTDS = Convert.ToString(Voucherdt.Rows[i]["gvColMainAccount"]).Trim();
                    Vouchers.gvColSubAccountTDS = Convert.ToString(Voucherdt.Rows[i]["gvColSubAccount"]).Trim();
                    Vouchers.gvMainAcCodeTDS = Convert.ToString(Voucherdt.Rows[i]["gvMainAcCode"]).Trim();
                    Vouchers.IsSubledgerTDS = Convert.ToString(Voucherdt.Rows[i]["IsSubledger"]).Trim();
                    Vouchers.IsTDS = Convert.ToString(Voucherdt.Rows[i]["IsTDS"]).Trim(); ;
                    Vouchers.UniqueID = Convert.ToString(Voucherdt.Rows[i]["UniqueID"]).Trim(); ;
                    Vouchers.IsTDSSource = Convert.ToString(Voucherdt.Rows[i]["IsTDSSource"]).Trim(); ;
                    Vouchers.TDSPercentage = Convert.ToString(Voucherdt.Rows[i]["TDSPercentage"]).Trim(); ;
                    VoucherList.Add(Vouchers);
                }
            }

            return VoucherList;
        }

        public DataTable GetJournalDetailsTDS(string Action, string JournalID, string IBRef)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_JournalVoucherDetailsTDS");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@JournalID", 500, JournalID);
            proc.AddVarcharPara("@IBRef", 500, IBRef);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetProjectCodeDetailsOnDocument()
        {


            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_JournalVoucherDetailsTDS");
            proc.AddVarcharPara("@Action", 500, "CashBankTDSProjectList");
            proc.AddVarcharPara("@BranchId", 20, ddlBranch.SelectedValue);
            dt = proc.GetTable();
            return dt;
        }


        public DataTable GetCashbankTDSProjectDataSource()
        {
            DataTable RcpDt = new DataTable();
            RcpDt.Columns.Add("CashReportID", typeof(System.String));
            RcpDt.Columns.Add("MainAccount", typeof(System.String));
            RcpDt.Columns.Add("SubAccount", typeof(System.String));
            RcpDt.Columns.Add("WithDrawl", typeof(System.Decimal));
            RcpDt.Columns.Add("Receipt", typeof(System.String));
            RcpDt.Columns.Add("Narration", typeof(System.String));
            RcpDt.Columns.Add("Status", typeof(System.String));
            RcpDt.Columns.Add("IsTDS", typeof(System.String));
            RcpDt.Columns.Add("UniqueID", typeof(System.String));
            RcpDt.Columns.Add("IsTDSSource", typeof(System.String));
            RcpDt.Columns.Add("TDSPercentage", typeof(System.String));
             return RcpDt;
        }

        public DataTable GetProjectCodeDetailsOnDocument(string ProjectId)
        {


            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_JournalVoucherDetailsTDS");
            proc.AddVarcharPara("@Action", 500, "CashBankTDSProjectListMappingProjectCode");
            proc.AddVarcharPara("@BranchId", 20, ddlBranch.SelectedValue);
            proc.AddBigIntegerPara("@ProjectId", Convert.ToInt64(ProjectId));
            dt = proc.GetTable();
            return dt;
        }

        protected void GridTDS_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void GridTDS_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void GridTDS_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            e.Cancel = true;
        }
        #region Class
        public class MainAccountBind
        {
            public string AccountCode { get; set; }
            public string IntegrateMainAccount { get; set; }
        }
        public class SubAccountBind
        {
            public string SubAccount_Name { get; set; }
            public string SubAccount_ReferenceID { get; set; }
        }
        public class VOUCHERLIST
        {
            public string SrlNo { get; set; }
            public string CashBankID { get; set; }
            public string MainAccount { get; set; }
            public string bthSubAccount { get; set; }
            public string btnRecieve { get; set; }
            public string btnPayment { get; set; }
            public string btnLineNarration { get; set; }
            public string TDS { get; set; }
            public string TaxAmount { get; set; }
            public string NetAmount { get; set; }
            public string ReverseApplicable { get; set; }
            public string gvColMainAccount { get; set; }
            public string gvColSubAccount { get; set; }
            public string IsSubledger { get; set; }
            public Int64 ProjectId { get; set; }
            public string Project_Code { get; set; }


        }
        public class TaxSetailsJson
        {
            public string SchemeName { get; set; }
            public int vissibleIndex { get; set; }
            public string applicableOn { get; set; }
            public string applicableBy { get; set; }
        }
        public class TaxDetails
        {
            public int Taxes_ID { get; set; }
            public string Taxes_Name { get; set; }

            public double Amount { get; set; }
            public string TaxField { get; set; }

            public string taxCodeName { get; set; }

            public decimal calCulatedOn { get; set; }

        }
        #endregion
        #region IEnumerable
        public IEnumerable GetMainAccountForBatch()
        {
            List<MainAccountBind> MainAccountList = new List<MainAccountBind>();
            //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

            string strQuery_Table = "Master_MainAccount";

            // string strQuery_FieldName = "Select MainAccount_Name+\' [ \'+rtrim(ltrim(MainAccount_AccountCode))+\' ]\' as MainAccount_Name1,cast(MainAccount_ReferenceID as varchar)+\'~\'+MainAccount_SubLedgerType+\'~MAINAC~\'+MainAccount_AccountType as MainAccount_ReferenceID from Master_MainAccount where MainAccount_BankCashType Not In ('Bank','Cash') ";
            string strQuery_FieldName = "Select MainAccount_Name+\' [ \'+rtrim(ltrim(MainAccount_AccountCode))+\' ]\' as MainAccount_Name1,MainAccount_ReferenceID from Master_MainAccount where MainAccount_BankCashType Not In ('Bank','Cash') ";

            string strQuery_WhereClause = "";
            string strQuery_OrderBy = "";
            string strQuery_GroupBy = "";
            string CombinedQuery = strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy;


            DataTable DT = new DataTable();
            string RequestLetter = "%";
            string[] param = CombinedQuery.Replace("--", "+").Replace("^^", "%").Split('$');
            string strQueryTable = param[0].Trim() != String.Empty ? param[0] : null;
            string strQueryFieldName = param[1].Trim() != String.Empty ? param[1] : null;
            string strQueryWhereClause = param[2].Trim() != String.Empty ? param[2] : null;
            string strQueryOrderBy = param[3].Trim() != String.Empty ? param[3] : null;
            string strQueryGroupBy = param[4].Trim() != String.Empty ? param[4] : null;
            if (strQuery_Table != null)
            {
                strQuery_Table = strQuery_Table.Replace("RequestLetter", RequestLetter);
            }

            if (strQuery_WhereClause != null)
            {
                strQuery_WhereClause = strQuery_WhereClause.Replace("RequestLetter", RequestLetter);
            }
            // DT = objEngine.GetDataTable(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_OrderBy, strQuery_GroupBy);
            DT = objEngine.GetDataTable(strQuery_FieldName);

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                MainAccountBind Main_Account = new MainAccountBind();
                Main_Account.AccountCode = Convert.ToString(DT.Rows[i]["MainAccount_ReferenceID"]);
                Main_Account.IntegrateMainAccount = Convert.ToString(DT.Rows[i]["MainAccount_Name1"]);
                MainAccountList.Add(Main_Account);
            }

            return MainAccountList;
        }
        public IEnumerable GetMainAccountForBatchByBranchID(string BranchId)
        {
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            List<MainAccountBind> MainAccountList = new List<MainAccountBind>();
            // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

            string strQuery_Table = "Master_MainAccount";
            string strQuery_FieldName = "Select MainAccount_Name+\' [ \'+rtrim(ltrim(MainAccount_AccountCode))+\' ]\' as MainAccount_Name1,MainAccount_ReferenceID,MainAccount_branchId from Master_MainAccount where MainAccount_BankCashType Not In ('Bank','Cash') AND MainAccount_branchId in ('" + BranchId + "','0') AND MainAccount_BankCompany in ('" + strCompanyID + "','')";
            //string strQuery_FieldName = "Select MainAccount_Name+\' [ \'+rtrim(ltrim(MainAccount_AccountCode))+\' ]\' as MainAccount_Name1,cast(MainAccount_ReferenceID as varchar)+\'~\'+MainAccount_SubLedgerType+\'~MAINAC~\'+MainAccount_AccountType as MainAccount_ReferenceID from Master_MainAccount where MainAccount_branchId="+BranchId;
            //string strQuery_WhereClause = " (MainAccount_Name like (\'%RequestLetter%\') or MainAccount_AccountCode like (\'%RequestLetter%\')) and isnull(MainAccount_BankCashType,'abc') not in('Cash','Bank')";
            string strQuery_WhereClause = "";
            string strQuery_OrderBy = "";
            string strQuery_GroupBy = "";
            string CombinedQuery = strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy;


            DataTable DT = new DataTable();
            string RequestLetter = "%";
            string[] param = CombinedQuery.Replace("--", "+").Replace("^^", "%").Split('$');
            string strQueryTable = param[0].Trim() != String.Empty ? param[0] : null;
            string strQueryFieldName = param[1].Trim() != String.Empty ? param[1] : null;
            string strQueryWhereClause = param[2].Trim() != String.Empty ? param[2] : null;
            string strQueryOrderBy = param[3].Trim() != String.Empty ? param[3] : null;
            string strQueryGroupBy = param[4].Trim() != String.Empty ? param[4] : null;
            if (strQuery_Table != null)
            {
                strQuery_Table = strQuery_Table.Replace("RequestLetter", RequestLetter);
            }

            if (strQuery_WhereClause != null)
            {
                strQuery_WhereClause = strQuery_WhereClause.Replace("RequestLetter", RequestLetter);
            }
            // DT = objEngine.GetDataTable(strQuery_Table, strQuery_FieldName, strQuery_WhereClause, strQuery_OrderBy, strQuery_GroupBy);
            DT = objEngine.GetDataTable(strQuery_FieldName);
            DataTable restrictedDT = oDbEngine.GetDataTable("select branch_id,MainAccount_id from tbl_master_ledgerBranch_map");
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                DataRow[] restrictedTablerow = restrictedDT.Select("MainAccount_id=" + Convert.ToString(DT.Rows[i]["MainAccount_ReferenceID"]));

                if (restrictedTablerow.Length > 0)
                {
                    DataTable restrictedTable = restrictedTablerow.CopyToDataTable();
                    DataRow[] restrictedRow = restrictedTable.Select("branch_id=" + BranchId);
                    if (restrictedRow.Length > 0)
                    {
                        MainAccountBind Main_Account = new MainAccountBind();
                        Main_Account.AccountCode = Convert.ToString(DT.Rows[i]["MainAccount_ReferenceID"]);
                        Main_Account.IntegrateMainAccount = Convert.ToString(DT.Rows[i]["MainAccount_Name1"]);
                        MainAccountList.Add(Main_Account);
                    }
                }
                else
                {
                    MainAccountBind Main_Account = new MainAccountBind();
                    Main_Account.AccountCode = Convert.ToString(DT.Rows[i]["MainAccount_ReferenceID"]);
                    Main_Account.IntegrateMainAccount = Convert.ToString(DT.Rows[i]["MainAccount_Name1"]);
                    MainAccountList.Add(Main_Account);
                }

            }
            return MainAccountList;
        }
        public IEnumerable GetSubAccount(string ProcedureSubName, string[] InputSubName, string[] InputSubType, string[] InputSubValue)
        {
            //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

            DataTable DT = objEngine.SelectProcedureArr(ProcedureSubName, InputSubName, InputSubType, InputSubValue);

            List<SubAccountBind> SubAccountList = new List<SubAccountBind>();

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                SubAccountBind SubAccounts = new SubAccountBind();
                SubAccounts.SubAccount_ReferenceID = Convert.ToString(DT.Rows[i][1]);
                SubAccounts.SubAccount_Name = Convert.ToString(DT.Rows[i][0]);
                SubAccountList.Add(SubAccounts);
            }

            return SubAccountList;
        }
        public IEnumerable GetSubAccount()
        {

            //    BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

            DataTable DT = objEngine.GetDataTable("tbl_master_branch", " (ISNULL(ltrim(rtrim(b.cnt_firstname)),'') + ''+ ISNULL(b.cnt_middlename,'')+ '' + ISNULL(b.cnt_lastName,'''')) +' ['+isnull(ltrim(rtrim(b.cnt_UCC)),'')+']' +' ['+(select rtrim(branch_description)  ", " branch_id in (select cnt_branchid from tbl_master_contact where cnt_internalid=b.cnt_internalId))+']' as Contact_Name,b.cnt_internalId+'~NAB~NAB~'+isnull(ltrim(rtrim(cnt_firstName)),'')+''+isnull(cnt_middleName,'')+''+isnull(cnt_lastName,'') as SubAccount_ReferenceID FROM tbl_master_contact as b ");

            List<SubAccountBind> SubAccountList = new List<SubAccountBind>();
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                SubAccountBind SubAccounts = new SubAccountBind();
                SubAccounts.SubAccount_ReferenceID = Convert.ToString(DT.Rows[i][1]);
                SubAccounts.SubAccount_Name = Convert.ToString(DT.Rows[i][0]);
                SubAccountList.Add(SubAccounts);
            }
            return SubAccountList;
        }
        public IEnumerable GetVoucher()
        {
            DataSet DsOnLoad = new DataSet();
            DataTable tempdt = new DataTable();
            DBEngine objEngine = new DBEngine();
            List<VOUCHERLIST> VoucherList = new List<VOUCHERLIST>();

            DataTable Voucherdt = GetCashBanktBatchEditData(hdnEditRfid.Value).Tables[0];
            for (int i = 0; i < Voucherdt.Rows.Count; i++)
            {
                VOUCHERLIST Vouchers = new VOUCHERLIST();
                Vouchers.SrlNo = Convert.ToString(Voucherdt.Rows[i]["srlNo"]);
                Vouchers.CashBankID = Convert.ToString(Voucherdt.Rows[i]["CashReportID"]);
                Vouchers.gvColMainAccount = Convert.ToString(Voucherdt.Rows[i]["MainAccount"]);
                Vouchers.gvColSubAccount = Convert.ToString(Voucherdt.Rows[i]["SubAccount"]);

                Vouchers.btnPayment = Convert.ToString(Voucherdt.Rows[i]["PaymentAmount"]);
                Vouchers.btnRecieve = Convert.ToString(Voucherdt.Rows[i]["ReceiptAmount"]);
                Vouchers.btnLineNarration = Convert.ToString(Voucherdt.Rows[i]["Remarks"]);
                Vouchers.TDS = "";
                Vouchers.TaxAmount = Convert.ToString(Voucherdt.Rows[i]["TaxAmount"]);
                Vouchers.NetAmount = Convert.ToString(Voucherdt.Rows[i]["NetAmount"]);
                Vouchers.ReverseApplicable = Convert.ToString(Voucherdt.Rows[i]["ReverseApplicable"]);
                Vouchers.MainAccount = Convert.ToString(Voucherdt.Rows[i]["MainAccountID"]);
                Vouchers.bthSubAccount = Convert.ToString(Voucherdt.Rows[i]["SubAccountID"]);
                Vouchers.IsSubledger = Convert.ToString(Voucherdt.Rows[i]["IsSubledger"]);

                VoucherList.Add(Vouchers);
            }
            return VoucherList;
        }
        #endregion
        #region Init Method
        //protected void SubAccount_Init(object sender, EventArgs e)
        //{
        //    ASPxComboBox SubAccountCombo = sender as ASPxComboBox;
        //    GridViewEditItemTemplateContainer container = SubAccountCombo.NamingContainer as GridViewEditItemTemplateContainer;
        //    string mainAccount = Convert.ToString(container.Grid.GetRowValues(container.Grid.VisibleStartIndex, "MainAccount"));
        //    gridBatch.JSProperties["cplastMainAccountID"] = mainAccount;            
        //    SubAccountCombo.DataSource = GetSubAccountNew(mainAccount.ToString().Split('~')[0], "", Convert.ToString(Session["userbranchHierarchy"]), "", "");
        //}

        protected void Page_Init(object sender, EventArgs e)
        {
            SqlDataSourceMainAccount.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlDataSourceSubAccount.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlSchematype.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlCurrency.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            dsBranch.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            dsTDS.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            dsSupplyState.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            dsTaxType.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            //((GridViewDataComboBoxColumn)gridBatch.Columns["MainAccount"]).PropertiesComboBox.DataSource = GetMainAccountForBatch();
            //((GridViewDataComboBoxColumn)gridBatch.Columns["bthSubAccount"]).PropertiesComboBox.DataSource = GetSubAccountNew("", "", "", "ALL", "");
            if (!IsPostBack)
            {
            }
        }
        #endregion
        //public void Bind_SubAccountByMainAccount(string StrMain)
        //{
        //    string mainAccount = StrMain;
        //    gridBatch.JSProperties["cplastMainAccountID"] = mainAccount;

        //    string RequestLetter = "%";
        //    var SegID = "";
        //    var SegmentName = "";

        //    if (hdn_SegID_SegmentName.Value != "")
        //    {
        //        SegID = hdnSegmentid.Value;
        //        SegmentName = hdn_SegID_SegmentName.Value;
        //    }


        //    var ProcedureName = "SubAccountSelect_New";
        //    var InputName = "CashBank_MainAccountID|clause|branch|exchSegment|SegmentN";
        //    var InputType = "V|V|V|V|V";
        //    var InputValue = mainAccount.ToString().Split('~')[0] + "|RequestLetter|" + Session["userbranchHierarchy"] + "|'" + Session["ExchangeSegmentID"] + "'|'" + SegmentName + "'";
        //    var SplitChar = "|";
        //    var CombinedSubQuery = ProcedureName + "$" + InputName + "$" + InputType + "$" + InputValue + "$" + SplitChar;


        //    string[] paramSub = CombinedSubQuery.Split('$');


        //    char SplitSubChar = Convert.ToChar(paramSub[4]);
        //    string ProcedureSubName = Convert.ToString(paramSub[0]);
        //    string[] InputSubName = paramSub[1].Split(SplitSubChar);
        //    string[] InputSubType = paramSub[2].Split(SplitSubChar);
        //    string SetRequestLetter = paramSub[3].Replace("RequestLetter", RequestLetter);
        //    string[] InputSubValue = SetRequestLetter.Split(SplitSubChar);

        //    ((GridViewDataComboBoxColumn)gridBatch.Columns["bthSubAccount"]).PropertiesComboBox.DataSource = GetSubAccount(ProcedureSubName, InputSubName, InputSubType, InputSubValue);
        //}
        protected void SubAccount_Callback(object sender, CallbackEventArgsBase e)
        {
            ASPxComboBox c = sender as ASPxComboBox;
            string MainAccount = Convert.ToString(e.Parameter.Split('~')[0]);
            if (Convert.ToString(e.Parameter.Split('~')[0]) != "null" && Convert.ToString(e.Parameter.Split('~')[0]) != "")
            {
                string sunAccount = Convert.ToString(e.Parameter.Split('~')[1]);
                c.DataSource = GetSubAccountNew(MainAccount, "", Convert.ToString(Session["userbranchHierarchy"]), "", sunAccount);
                c.DataBind();
            }
            if (MainAccount != "" && MainAccount != "null")
            {
                DataSet dsInst = new DataSet();
                //  SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));


                SqlCommand cmd = new SqlCommand("Fetch_CashBankEntry_DataSet", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "ReverseApplicable");
                cmd.Parameters.AddWithValue("@MainAccID", MainAccount);
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();
                c.JSProperties["cpReverseApplicable"] = (dsInst.Tables[0].Rows.Count > 0) ? (Convert.ToString(dsInst.Tables[0].Rows[0][0])) : "0";
                c.JSProperties["cpIsTaxable"] = (dsInst.Tables[0].Rows.Count > 0) ? (Convert.ToString(dsInst.Tables[0].Rows[0][1])) : "";

            }
        }
        public DataTable TaxCalculated(DataTable strCashBankdt, DataTable taxTable, string Type, string TransactionDate, string BranchId, string ShippingState, string strTaxOption)
        {

            #region VoucharType  Payment
            if (Type == "P")
            {
                foreach (DataRow dtr in strCashBankdt.Rows)
                {
                    string strMainAccount = Convert.ToString(dtr["MainAccount"]);
                    decimal Recieve = Convert.ToDecimal(dtr["ReceiptAmount"]);
                    decimal Payment = Convert.ToDecimal(dtr["PaymentAmount"]);


                    string fromState = "", shippingStateCode = "", TaxType = "", roundOfPlus = "", roundofMinus = "";
                    DataTable fetchedData = oDBEngine.GetDataTable("select StateCode  from tbl_master_branch br inner join tbl_master_state st on br.branch_state=st.id where branch_id=" + BranchId);
                    if (fetchedData != null)
                        fromState = Convert.ToString(fetchedData.Rows[0][0]).Trim();

                    fetchedData = oDBEngine.GetDataTable("select StateCode from tbl_master_state where id=" + ShippingState);
                    if (fetchedData != null)
                        shippingStateCode = Convert.ToString(fetchedData.Rows[0][0]).Trim();

                    fetchedData = oDBEngine.GetDataTable("select TaxRates_ID from Config_TaxRates config inner join Master_Taxes masterTax on config.TaxRates_TaxCode = masterTax.Taxes_ID  where  TaxRates_RoundingOff='R+' and Taxes_ApplicableFor in ('B','" + Type + "')");
                    if (fetchedData.Rows.Count > 0)
                        roundOfPlus = Convert.ToString(fetchedData.Rows[0][0]).Trim();

                    fetchedData = oDBEngine.GetDataTable("select TaxRates_ID from Config_TaxRates config inner join Master_Taxes masterTax on config.TaxRates_TaxCode = masterTax.Taxes_ID  where  TaxRates_RoundingOff='R-' and Taxes_ApplicableFor in ('B','" + Type + "')");
                    if (fetchedData.Rows.Count > 0)
                        roundofMinus = Convert.ToString(fetchedData.Rows[0][0]).Trim();


                    #region setTaxType
                    if (fromState == shippingStateCode)
                    {
                        TaxType = "SGST";
                        if (shippingStateCode == "4" || shippingStateCode == "35" || shippingStateCode == "26" || shippingStateCode == "25" || shippingStateCode == "7" || shippingStateCode == "31" || shippingStateCode == "34")
                        {
                            TaxType = "UTGST";
                        }
                    }
                    else
                    {
                        TaxType = "IGST";
                    }

                    #endregion

                    DataTable taxDetail = new DataTable();
                    ProcedureExecute proc = new ProcedureExecute("Fetch_CashBankEntry_DataSet");
                    proc.AddVarcharPara("@Action", 500, "LoadOtherTaxDetails");
                    proc.AddVarcharPara("@MainAccountID", 100, Convert.ToString(strMainAccount));
                    proc.AddVarcharPara("@dtTDate", 10, dtTDate.Date.ToString("yyyy-MM-dd"));
                    proc.AddVarcharPara("@Type", 5, Type);
                    taxDetail = proc.GetTable();

                    #region DeleteExtra GstCode

                    if (TaxType == "UTGST")
                    {
                        foreach (DataRow dr in taxDetail.Rows)
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                            {
                                dr.Delete();
                            }
                        }
                    }
                    else if (TaxType == "IGST")
                    {
                        foreach (DataRow dr in taxDetail.Rows)
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                            {
                                dr.Delete();
                            }
                        }
                    }
                    else if (TaxType == "SGST")
                    {
                        foreach (DataRow dr in taxDetail.Rows)
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                            {
                                dr.Delete();
                            }
                        }
                    }
                    taxDetail.AcceptChanges();
                    #endregion


                    decimal TotalPercentage = 0, InclusiveAmt = 0, TaxAmount = 0, RoundOfAmt = 0;
                    foreach (DataRow taxRow in taxDetail.Rows)
                    {
                        if (Convert.ToString(taxRow["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(taxRow["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(taxRow["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(taxRow["TaxTypeCode"]).Trim() == "UTGST")
                        {
                            TotalPercentage = TotalPercentage + Convert.ToDecimal(taxRow["TaxRates_Rate"]);
                        }
                    }
                    decimal backcalculatePercent = 1 + (TotalPercentage / 100);

                    if (strTaxOption == "1")
                    {
                        InclusiveAmt = Payment;
                    }
                    else if (strTaxOption == "2")
                    {
                        InclusiveAmt = Payment / backcalculatePercent;
                    }
                    foreach (DataRow taxexistingRow in taxDetail.Rows)
                    {
                        TaxAmount = 0;

                        if (Convert.ToString(taxexistingRow["TaxTypeCode"]).Trim() != "O")
                        {
                            DataRow txRecordRow = taxTable.NewRow();
                            txRecordRow["SlNo"] = dtr["SrlNo"];
                            txRecordRow["TaxCode"] = taxexistingRow["Taxes_ID"];
                            txRecordRow["AltTaxCode"] = "0";
                            txRecordRow["Percentage"] = taxexistingRow["TaxRates_Rate"];

                            TaxAmount = InclusiveAmt * (Convert.ToDecimal(taxexistingRow["TaxRates_Rate"]) / 100);

                            txRecordRow["Amount"] = Math.Round(TaxAmount, 2);

                            taxTable.Rows.Add(txRecordRow);

                        }
                    }



                }
            }

            #endregion

            #region VoucharType Receipt
            if (Type == "R")
            {
                foreach (DataRow dtr in strCashBankdt.Rows)
                {
                    string strMainAccount = Convert.ToString(dtr["MainAccount"]);
                    decimal Recieve = Convert.ToDecimal(dtr["ReceiptAmount"]);
                    decimal Payment = Convert.ToDecimal(dtr["PaymentAmount"]);


                    string fromState = "", shippingStateCode = "", TaxType = "", roundOfPlus = "", roundofMinus = "";
                    DataTable fetchedData = oDBEngine.GetDataTable("select StateCode  from tbl_master_branch br inner join tbl_master_state st on br.branch_state=st.id where branch_id=" + BranchId);
                    if (fetchedData != null)
                        fromState = Convert.ToString(fetchedData.Rows[0][0]).Trim();

                    fetchedData = oDBEngine.GetDataTable("select StateCode from tbl_master_state where id=" + ShippingState);
                    if (fetchedData != null)
                        shippingStateCode = Convert.ToString(fetchedData.Rows[0][0]).Trim();

                    fetchedData = oDBEngine.GetDataTable("select TaxRates_ID from Config_TaxRates config inner join Master_Taxes masterTax on config.TaxRates_TaxCode = masterTax.Taxes_ID  where  TaxRates_RoundingOff='R+' and Taxes_ApplicableFor in ('B','" + Type + "')");
                    if (fetchedData.Rows.Count > 0)
                        roundOfPlus = Convert.ToString(fetchedData.Rows[0][0]).Trim();

                    fetchedData = oDBEngine.GetDataTable("select TaxRates_ID from Config_TaxRates config inner join Master_Taxes masterTax on config.TaxRates_TaxCode = masterTax.Taxes_ID  where  TaxRates_RoundingOff='R-' and Taxes_ApplicableFor in ('B','" + Type + "')");
                    if (fetchedData.Rows.Count > 0)
                        roundofMinus = Convert.ToString(fetchedData.Rows[0][0]).Trim();


                    #region setTaxType
                    if (fromState == shippingStateCode)
                    {
                        TaxType = "SGST";
                        if (shippingStateCode == "4" || shippingStateCode == "35" || shippingStateCode == "26" || shippingStateCode == "25" || shippingStateCode == "7" || shippingStateCode == "31" || shippingStateCode == "34")
                        {
                            TaxType = "UTGST";
                        }
                    }
                    else
                    {
                        TaxType = "IGST";
                    }

                    #endregion

                    DataTable taxDetail = new DataTable();
                    ProcedureExecute proc = new ProcedureExecute("Fetch_CashBankEntry_DataSet");
                    proc.AddVarcharPara("@Action", 500, "LoadOtherTaxDetails");
                    proc.AddVarcharPara("@MainAccountID", 100, Convert.ToString(strMainAccount));
                    proc.AddVarcharPara("@dtTDate", 10, dtTDate.Date.ToString("yyyy-MM-dd"));
                    proc.AddVarcharPara("@Type", 5, "S");
                    taxDetail = proc.GetTable();

                    #region DeleteExtra GstCode

                    if (TaxType == "UTGST")
                    {
                        foreach (DataRow dr in taxDetail.Rows)
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                            {
                                dr.Delete();
                            }
                        }
                    }
                    else if (TaxType == "IGST")
                    {
                        foreach (DataRow dr in taxDetail.Rows)
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                            {
                                dr.Delete();
                            }
                        }
                    }
                    else if (TaxType == "SGST")
                    {
                        foreach (DataRow dr in taxDetail.Rows)
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                            {
                                dr.Delete();
                            }
                        }
                    }
                    taxDetail.AcceptChanges();
                    #endregion


                    decimal TotalPercentage = 0, InclusiveAmt = 0, TaxAmount = 0, RoundOfAmt = 0;
                    foreach (DataRow taxRow in taxDetail.Rows)
                    {
                        if (Convert.ToString(taxRow["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(taxRow["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(taxRow["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(taxRow["TaxTypeCode"]).Trim() == "UTGST")
                        {
                            TotalPercentage = TotalPercentage + Convert.ToDecimal(taxRow["TaxRates_Rate"]);
                        }
                    }
                    decimal backcalculatePercent = 1 + (TotalPercentage / 100);

                    if (strTaxOption == "1")
                    {
                        InclusiveAmt = Recieve;
                    }
                    else if (strTaxOption == "2")
                    {
                        InclusiveAmt = Recieve / backcalculatePercent;
                    }
                    foreach (DataRow taxexistingRow in taxDetail.Rows)
                    {
                        TaxAmount = 0;

                        if (Convert.ToString(taxexistingRow["TaxTypeCode"]).Trim() != "O")
                        {
                            DataRow txRecordRow = taxTable.NewRow();
                            txRecordRow["SlNo"] = dtr["SrlNo"];
                            txRecordRow["TaxCode"] = taxexistingRow["Taxes_ID"];
                            txRecordRow["AltTaxCode"] = "0";
                            txRecordRow["Percentage"] = taxexistingRow["TaxRates_Rate"];

                            TaxAmount = InclusiveAmt * (Convert.ToDecimal(taxexistingRow["TaxRates_Rate"]) / 100);


                            txRecordRow["Amount"] = Math.Round(TaxAmount, 2);

                            taxTable.Rows.Add(txRecordRow);

                        }
                    }



                }
            }

            #endregion
            return taxTable;
        }
        public IEnumerable GetVoucherTDS(DataTable Voucherdt)
        {
            DataSet DsOnLoad = new DataSet();
            DataTable tempdt = new DataTable();
            DBEngine objEngine = new DBEngine();
            List<VOUCHERLIST> VoucherList = new List<VOUCHERLIST>();

            for (int i = 0; i < Voucherdt.Rows.Count; i++)
            {
                VOUCHERLIST Vouchers = new VOUCHERLIST();
                Vouchers.SrlNo = Convert.ToString(Voucherdt.Rows[i]["SrlNo"]);
                Vouchers.CashBankID = Convert.ToString(Voucherdt.Rows[i]["CashReportID"]);
                Vouchers.MainAccount = Convert.ToString(Voucherdt.Rows[i]["MainAccountID"]);
                Vouchers.bthSubAccount = Convert.ToString(Voucherdt.Rows[i]["SubAccountID"]);
                Vouchers.btnPayment = Convert.ToString(Voucherdt.Rows[i]["PaymentAmount"]);
                Vouchers.btnRecieve = Convert.ToString(Voucherdt.Rows[i]["ReceiptAmount"]);
                Vouchers.btnLineNarration = Convert.ToString(Voucherdt.Rows[i]["Remarks"]);
                Vouchers.TDS = "";
                Vouchers.TaxAmount = Convert.ToString(Voucherdt.Rows[i]["TaxAmount"]);
                Vouchers.NetAmount = Convert.ToString(Voucherdt.Rows[i]["NetAmount"]);
                Vouchers.ReverseApplicable = Convert.ToString(Voucherdt.Rows[i]["ReverseApplicable"]);
                Vouchers.gvColMainAccount = Convert.ToString(Voucherdt.Rows[i]["MainAccount"]);
                Vouchers.gvColSubAccount = Convert.ToString(Voucherdt.Rows[i]["SubAccount"]);
                Vouchers.IsSubledger = Convert.ToString(Voucherdt.Rows[i]["IsSubledger"]);
                VoucherList.Add(Vouchers);
            }

            return VoucherList;
        }
        public DataTable GetCashBankEditedTaxData(string CashBackId)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Fetch_CashBankEntry_DataSet");
            proc.AddVarcharPara("@Action", 500, "CashBankEditedTaxDetails");
            proc.AddVarcharPara("@CashBankId", 500, Convert.ToString(CashBackId));
            ds = proc.GetDataSet();
            return ds.Tables[0];
        }
        public DataTable GetTDSData(string MainAcc, string payment)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Fetch_CashBankEntry_DataSet");
            proc.AddVarcharPara("@Action", 500, "BindTDS");
            proc.AddVarcharPara("@PaymentAmount", 2000, payment);
            proc.AddIntegerPara("@MainAccID", Convert.ToInt32(MainAcc));
            dt = proc.GetTable();
            return dt;
        }
        public DataSet GetCashBanktEditData()
        {
            DataSet dt = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Fetch_CashBankEntry_DataSet");
            proc.AddVarcharPara("@Action", 500, "CashBankEditDetailsTDS");
            // proc.AddVarcharPara("@IBRef", 200, Convert.ToString(Session["IBRef"]));
            proc.AddVarcharPara("@CashBank_ID", 200, Convert.ToString(Session["CashBank_ID"]));
            dt = proc.GetDataSet();
            return dt;
        }
        public DataSet GetCashBanktBatchEditData(string CashBank_ID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Fetch_CashBankEntry_DataSet");
            proc.AddVarcharPara("@Action", 500, "CashBankEditBatchTDSDetails");
            //  proc.AddVarcharPara("@IBRef", 200, Convert.ToString(Session["IBRef"]));
            proc.AddVarcharPara("@CashBank_ID", 200, CashBank_ID);
            ds = proc.GetDataSet();
            return ds;
        }
        public DataTable GetProjectEditData(string contraid)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_Search_ContraVoucher");
            proc.AddVarcharPara("@Action", 500, "ProjectEditdata");
            proc.AddIntegerPara("@Contra_ID", Convert.ToInt32(contraid));
            dt = proc.GetTable();
            return dt;
        }
        public DataSet GetCashBanktBatchEditData(string CashBank_ID, string Overload)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Fetch_CashBankEntry_DataSet");
            proc.AddVarcharPara("@Action", 500, "CashBankEditBatchTDSDetailsEdit");
            //  proc.AddVarcharPara("@IBRef", 200, Convert.ToString(Session["IBRef"]));
            proc.AddVarcharPara("@CashBank_ID", 200, CashBank_ID);
            ds = proc.GetDataSet();
            return ds;
        }
        public void SetReciptPayment(ref decimal Recipt, ref decimal Payment)
        {

            DataSet DsOnLoad = new DataSet();
            DataTable Voucherdt = new DataTable();

            if (File.Exists(Server.MapPath(CashBankVoucherFile_XMLPATH)))
            {
                if (DsOnLoad.Tables.Count > 0) { DsOnLoad.Tables.Remove(DsOnLoad.Tables[0]); DsOnLoad.Clear(); }
                DsOnLoad.ReadXml(Server.MapPath(CashBankVoucherFile_XMLPATH));
            }
            if (DsOnLoad.Tables.Count > 0 && DsOnLoad != null)
            {
                Voucherdt = DsOnLoad.Tables[0];

                for (int i = 0; i < Voucherdt.Rows.Count; i++)
                {
                    Recipt = Recipt + Convert.ToDecimal(Voucherdt.Rows[i]["ReceiptAmount"]);
                    Payment = Payment + Convert.ToDecimal(Voucherdt.Rows[i]["PaymentAmount"]);
                }
            }


        }
        public IEnumerable GetVoucherNull()
        {
            DataSet DsOnLoad = new DataSet();
            DataTable tempdt = new DataTable();
            DBEngine objEngine = new DBEngine();
            List<VOUCHERLIST> VoucherList = new List<VOUCHERLIST>();


            return VoucherList;
        }
        protected void CmbScheme_Callback(object sender, CallbackEventArgsBase e)
        {
            string command = e.Parameter;
            if (command == "P")
            {
                Bind_PaymentNumberingScheme();
                CmbScheme.Value = "0";
            }
            else if (command == "R")
            {
                Bind_ReceiptNumberingScheme();
                CmbScheme.Value = "0";
            }
        }
        public IEnumerable GetSubAccountNew(string strMainAccount, string strClause, string strBranch, string strType, string strSubAccount)
        {
            DataTable DT = GetSubAccountTable(strMainAccount, strClause, strBranch, strType, strSubAccount);

            List<SubAccountBind> SubAccountList = new List<SubAccountBind>();

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                SubAccountBind SubAccounts = new SubAccountBind();
                SubAccounts.SubAccount_ReferenceID = Convert.ToString(DT.Rows[i]["SubAccount_ReferenceID"]);
                SubAccounts.SubAccount_Name = Convert.ToString(DT.Rows[i]["Contact_Name"]);
                SubAccountList.Add(SubAccounts);
            }

            return SubAccountList;
        }
        public DataTable GetSubAccountTable(string strMainAccount, string strClause, string strBranch, string strType, string strSubAccount)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_FethSubAccountCashBankVoucher");
            proc.AddVarcharPara("@CashBank_MainAccountID", 500, strMainAccount);
            proc.AddVarcharPara("@clause", 500, strClause);
            proc.AddVarcharPara("@branch", 500, strBranch);
            proc.AddVarcharPara("@SelectionType", 500, strType);
            proc.AddVarcharPara("@SubAccount", 500, strSubAccount);
            dt = proc.GetTable();
            return dt;
        }



        public void DeleteTaxDetails(string SrlNo)
        {
            DataTable TaxDetailTable = new DataTable();
            if (Session["CB_FinalTaxRecord"] != null)
            {
                TaxDetailTable = (DataTable)Session["CB_FinalTaxRecord"];

                var rows = TaxDetailTable.Select("SlNo ='" + SrlNo + "'");
                foreach (var row in rows)
                {
                    row.Delete();
                }
                TaxDetailTable.AcceptChanges();

                Session["CB_FinalTaxRecord"] = TaxDetailTable;
            }
        }
        public void UpdateTaxDetails(string oldSrlNo, string newSrlNo)
        {
            DataTable TaxDetailTable = new DataTable();
            if (Session["CB_FinalTaxRecord"] != null)
            {
                TaxDetailTable = (DataTable)Session["CB_FinalTaxRecord"];

                for (int i = 0; i < TaxDetailTable.Rows.Count; i++)
                {
                    DataRow dr = TaxDetailTable.Rows[i];
                    string Product_SrlNo = Convert.ToString(dr["SlNo"]);
                    if (oldSrlNo == Product_SrlNo)
                    {
                        dr["SlNo"] = newSrlNo;
                    }
                }
                TaxDetailTable.AcceptChanges();

                Session["CB_FinalTaxRecord"] = TaxDetailTable;
            }
        }
        public DataTable GetListGridDataByFilter(string userbranchlist, string lastCompany, string Fiyear, string userbranchID, string FromDate, string ToDate)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Fetch_CashBankEntry_DataSet");
            proc.AddVarcharPara("@Action", 500, "CashBankGridList");
            proc.AddVarcharPara("@FinYear", 500, Fiyear);
            proc.AddVarcharPara("@CompanyID", 500, lastCompany);
            proc.AddVarcharPara("@BranchList", 5000, userbranchlist);
            proc.AddVarcharPara("@BranchID", 3000, userbranchID);
            proc.AddVarcharPara("@FromDate", 10, FromDate);
            proc.AddVarcharPara("@ToDate", 10, ToDate);
            dt = proc.GetTable();
            return dt;
        }
        protected void acpCrossBtn_Callback(object sender, CallbackEventArgsBase e)
        {
            Session.Remove("CashBankDetails");
        }

        //protected void gridBatch_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        //{
        //    if (e.RowType != GridViewRowType.Data)
        //        return;

        //string parentLedgerId = Convert.ToString(e.GetValue("MainAccount"));
        //if (parentLedgerId.Trim() !="")
        //{
        //e.Row.Cells[9].Controls[0].Visible = false;
        //}
        //}
        //protected void GvCBSearch_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        //{
        //    e.Text = string.Format("{0}", e.Value);
        //}
        //protected void ASPxCallbackGeneral_Callback(object sender, CallbackEventArgsBase e)
        //{
        //    string command = e.Parameter.Split('~')[0];
        //    int RowIndex=0;
        //    string IBRef="";
        //    string cask_bankID="";

        //    if (command == "Edit" || command == "View")
        //    {
        //        RowIndex = Convert.ToInt32(e.Parameter.Split('~')[1]);
        //        string CBID = "";
        //        //string CBID = GvCBSearch.GetRowValues(RowIndex, "CBID").ToString();
        //        ViewState["CashBankID"] = CBID;
        //       // IBRef = GvCBSearch.GetRowValues(RowIndex, "IBRef").ToString();
        //        Session["IBRef"] = IBRef;
        //        Session["CashBank_ID"] = CBID;

        //        hdnEditRfid.Value = IBRef;              
        //        Session["CB_FinalTaxRecord"] = null;
        //        gridBatch.JSProperties["cpNOTEdit"] = null;
        //       // cask_bankID = GvCBSearch.GetRowValues(RowIndex, "CBID").ToString();
        //        hdnEditCBID.Value = cask_bankID;
        //        Session["CB_FinalTaxRecord"] = GetCashBankEditedTaxData(CBID);
        //        DataSet DsOnLoad = new DataSet();
        //        string VoucherType = "", VoucherNo = "", Date = "", Branch = "", Cash_Bank = "", Currency = "", InstrumentType = "", InstrumentNo = "", InstrumentDate = "",
        //            Narration = "", InstrumentTypeName = "", BankStatementDate = "", BankValueDate = "", ReceivedFrom = "", PaidTo = "", Tax_Code = "", ContactNo = "",
        //            ReverseCharge = "0", DraweeBank = "", EnteredBranchID = "", CashBankName = "";

        //        DataTable CashBanktEditdt = GetCashBanktEditData();
        //        if (CashBanktEditdt != null && CashBanktEditdt.Rows.Count > 0)
        //        {
        //            VoucherType = Convert.ToString(CashBanktEditdt.Rows[0]["VoucherType"]);//0
        //            rbtnType.SelectedValue = VoucherType;
        //            VoucherNo = Convert.ToString(CashBanktEditdt.Rows[0]["OldVoucherNumber"]);//1
        //            txtVoucherNo.Text = VoucherNo;
        //            Date = Convert.ToString(CashBanktEditdt.Rows[0]["TransactionDate"]);//2
        //            dtTDate.Date = Convert.ToDateTime(Date);
        //            Branch = Convert.ToString(CashBanktEditdt.Rows[0]["BranchID"]);//3 
        //            ddlBranch.SelectedValue = Branch;

        //            Cash_Bank = Convert.ToString(CashBanktEditdt.Rows[0]["CashBankID"]);//4                
        //            ddlCashBank.Value = Cash_Bank;
        //            Currency = Convert.ToString(CashBanktEditdt.Rows[0]["CashBank_Currency"]);//5
        //            CmbCurrency.Value = Currency;
        //            InstrumentTypeName = Convert.ToString(CashBanktEditdt.Rows[0]["InstrumentType"]);

        //            if (InstrumentTypeName == "0")
        //            {
        //                InstrumentType = "CH";
        //            }
        //            else
        //            {
        //                InstrumentType = Convert.ToString(CashBanktEditdt.Rows[0]["InstrumentType"]);//6
        //            }
        //            cmbInstrumentType.Value = InstrumentType;

        //            InstrumentNo = Convert.ToString(CashBanktEditdt.Rows[0]["InstrumentNumber"]);//7
        //            txtInstNobth.Text = InstrumentNo;
        //            Narration = Convert.ToString(CashBanktEditdt.Rows[0]["Narration"]);//8
        //            txtNarration.Text = Narration;
        //            BankStatementDate = Convert.ToString(CashBanktEditdt.Rows[0]["CashBankDetail_BankStatementDate"]);

        //            BankValueDate = Convert.ToString(CashBanktEditdt.Rows[0]["CashBankDetail_BankValueDate"]);


        //            ReceivedFrom = Convert.ToString(CashBanktEditdt.Rows[0]["CashBank_ReceivedFrom"]);//12
        //            txtReceivedFrom.Text = ReceivedFrom;
        //            PaidTo = Convert.ToString(CashBanktEditdt.Rows[0]["CashBank_PaidTo"]);//13
        //            txtPaidTo.Text = PaidTo;
        //            Tax_Code = Convert.ToString(CashBanktEditdt.Rows[0]["Tax_Code"]);
        //            ddl_AmountAre.Value = Tax_Code;
        //            ContactNo = Convert.ToString(CashBanktEditdt.Rows[0]["CashBank_ContactNo"]);
        //            txtContact.Text = ContactNo;
        //            ReverseCharge = Convert.ToString(CashBanktEditdt.Rows[0]["CashBank_ReverseCharge"]);
        //            chk_reversemechenism.Checked = Convert.ToBoolean(ReverseCharge);

        //            InstrumentDate = Convert.ToString(CashBanktEditdt.Rows[0]["InstrumentDate"]);
        //            InstDate.Date = Convert.ToDateTime(InstrumentDate);
        //            DraweeBank = Convert.ToString(CashBanktEditdt.Rows[0]["CashBank_DraweeBank"]);
        //            txtDraweeBank.Text = DraweeBank;
        //            EnteredBranchID = Convert.ToString(CashBanktEditdt.Rows[0]["CashBank_EnteredBranchID"]);
        //            ddlEnterBranch.SelectedValue = EnteredBranchID;
        //            CashBankName = Convert.ToString(CashBanktEditdt.Rows[0]["CashBankName"]);
        //            BindCashBankAccount(EnteredBranchID);
        //        }
        //        ASPxCallbackGeneral.JSProperties["cpCBEdit"] = VoucherType + "*" + cask_bankID + "*" + Currency + "*" +InstrumentType;
        //        DataTable Voucherdt = GetCashBanktBatchEditData(hdnEditRfid.Value).Tables[0];
        //        string Recipt = Convert.ToString(Voucherdt.Compute("Sum(ReceiptAmount)", ""));
        //        string Payment = Convert.ToString(Voucherdt.Compute("Sum(PaymentAmount)", ""));
        //        txtTotalAmount.Text = Recipt;
        //        txtTotalPayment.Text = Payment;
        //        ASPxCallbackGeneral.JSProperties["cpView"] = (command.ToUpper() == "VIEW") ? "1" : "0";
        //        Session["CashBankDetails"] = GetCashBanktBatchEditData(hdnEditRfid.Value).Tables[0];
        //        gridBatch.DataSource = GetVoucher();
        //        gridBatch.DataBind();

        //    }
        //}


        #region  Main Account
        //protected void EntityServerMainDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        //{
        //    e.KeyExpression = "MainAccount_ReferenceID";

        //    string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
        //    string IsFilter = Convert.ToString(hfIsFilter.Value);
        //    string strBranchID = (Convert.ToString(hdnBranchId.Value) == "") ? "0" : Convert.ToString(hdnBranchId.Value);

        //    string strBranchID1 = strBranchID + "0";

        //    List<int> branchidlist;
        //    ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

        //    if (IsFilter == "Y")
        //    {

        //        branchidlist = new List<int>(Array.ConvertAll(strBranchID1.Split(','), int.Parse));
        //        var q = from d in dc.v_MainAccountLists
        //                where
        //                branchidlist.Contains(Convert.ToInt32(d.MainAccount_branchId))
        //                orderby d.MainAccount_Name descending
        //                select d;
        //        e.QueryableSource = q;

        //    }
        //    else
        //    {                
        //        var q = from d in dc.v_MainAccountLists                        
        //                orderby d.MainAccount_Name descending
        //                select d;
        //        e.QueryableSource = q;
        //    }

        //}

        protected void ASPxMainAccountComboBox_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            //if (Convert.ToString(e.Filter).Trim() != "" && Convert.ToString(e.Filter).Length>4)
            if (Convert.ToString(e.Filter).Trim() != "")
            {
                ASPxComboBox comboBox = (ASPxComboBox)source;
                DataTable dt = new DataTable();
                string filter = "%" + Convert.ToString(e.Filter) + "%";
                int startindex = Convert.ToInt32(e.BeginIndex + 1);
                int EndIndex = Convert.ToInt32(e.EndIndex + 1);
                string strBranchID = (Convert.ToString(hdnBranchId.Value) == "") ? "0" : Convert.ToString(hdnBranchId.Value);
                string strCompanyID = Convert.ToString(Session["LastCompany"]);
                dt = GetMainAccountTableNew(strBranchID, filter, startindex, EndIndex, strCompanyID);
                comboBox.DataSource = dt;
                comboBox.DataBind();
            }
        }
        protected void ASPxMainComboBox_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            long value = 0;
            if (e.Value == null || !Int64.TryParse(e.Value.ToString(), out value))
                return;
            ASPxComboBox comboBox = (ASPxComboBox)source;
            SqlDataSourceMainAccount.SelectCommand = @"SELECT MainAccount_ReferenceID,MainAccount_Name,MainAccount_SubLedgerType,MainAccount_ReverseApplicable,TAXable FROM v_MainAccountList WHERE (MainAccount_ReferenceID = @ID) ";

            SqlDataSourceMainAccount.SelectParameters.Clear();
            SqlDataSourceMainAccount.SelectParameters.Add("ID", TypeCode.String, e.Value.ToString());
            comboBox.DataSource = SqlDataSourceMainAccount;
            comboBox.DataBind();
        }

        public DataTable GetMainAccountTableNew(string strBranchID, string filter, int startindex, int EndIndex, string strCompanyID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Fetch_CashBankEntry_DataSet");
            proc.AddVarcharPara("@Action", 100, "GetMainAccountList");
            proc.AddVarcharPara("@CompanyID", 500, strCompanyID);
            proc.AddVarcharPara("@filter", 100, filter);
            proc.AddIntegerPara("@startIndex", startindex);
            proc.AddIntegerPara("@endIndex", EndIndex);
            proc.AddVarcharPara("@BranchID", 100, strBranchID);
            dt = proc.GetTable();
            return dt;
        }
        #endregion

        protected void ASPxComboBox_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            if (e.Filter != "")
            {
                ASPxComboBox comboBox = (ASPxComboBox)source;
                DataTable dt = new DataTable();
                string filter = "%" + Convert.ToString(e.Filter) + "%";
                int startindex = Convert.ToInt32(e.BeginIndex + 1);
                int EndIndex = Convert.ToInt32(e.EndIndex + 1);
                string MainAccountID = hdnMainAccountId.Value;

                dt = GetSubAccountTableNew(MainAccountID, filter, startindex, EndIndex);
                comboBox.DataSource = dt;
                comboBox.DataBind();
            }
        }
        public DataTable GetSubAccountTableNew(string strMainAccount, string filter, int startindex, int EndIndex)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Fetch_CashBankEntry_DataSet");
            proc.AddVarcharPara("@Action", 100, "GetSubAccountListBYMainAccount");
            proc.AddVarcharPara("@MainAccountID", 500, strMainAccount);
            proc.AddVarcharPara("@filter", 100, filter);
            proc.AddIntegerPara("@startIndex", startindex);
            proc.AddIntegerPara("@endIndex", EndIndex);
            dt = proc.GetTable();
            return dt;
        }
        protected void ASPxComboBox_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            long value = 0;
            if (e.Value == null || !Int64.TryParse(e.Value.ToString(), out value))
                return;
            ASPxComboBox comboBox = (ASPxComboBox)source;
            SqlDataSourceSubAccount.SelectCommand = @"SELECT SubAccount_ReferenceID,Contact_Name,MainAccount_SubLedgerType,mainaccount_referenceid FROM v_SubAccountList WHERE (SubAccount_ReferenceID = @ID) ";

            SqlDataSourceSubAccount.SelectParameters.Clear();
            SqlDataSourceSubAccount.SelectParameters.Add("ID", TypeCode.String, e.Value.ToString());
            comboBox.DataSource = SqlDataSourceSubAccount;
            comboBox.DataBind();
        }


        public void GetFinacialYearBasedQouteDate()
        {
            String finyear = "";
            string setdate = null;
            if (Session["LastFinYear"] != null)
            {
                SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
                finyear = Convert.ToString(Session["LastFinYear"]).Trim();
                DataTable dtFinYear = objSlaesActivitiesBL.GetFinacialYearBasedQouteDate(finyear);
                if (dtFinYear != null && dtFinYear.Rows.Count > 0)
                {
                    Session["FinYearStartDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearStartDate"]);
                    Session["FinYearEndDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearEndDate"]);
                    if (Session["FinYearStartDate"] != null)
                    {
                        dtTDate.MinDate = Convert.ToDateTime(Convert.ToString(Session["FinYearStartDate"]));
                    }
                    if (Session["FinYearEndDate"] != null)
                    {
                        dtTDate.MaxDate = Convert.ToDateTime(Convert.ToString(Session["FinYearEndDate"]));
                    }
                    if (oDBEngine.GetDate().Date >= Convert.ToDateTime(Convert.ToString(Session["FinYearStartDate"])) && oDBEngine.GetDate().Date <= Convert.ToDateTime(Convert.ToString(Session["FinYearEndDate"])))
                    {
                        dtTDate.Date = DateTime.Now;

                    }
                    else if (oDBEngine.GetDate().Date >= Convert.ToDateTime(Convert.ToString(Session["FinYearStartDate"])) && oDBEngine.GetDate().Date >= Convert.ToDateTime(Convert.ToString(Session["FinYearEndDate"])))
                    {
                        setdate = Convert.ToString(Convert.ToDateTime(Session["FinYearEndDate"]).Month) + "/" + Convert.ToString(Convert.ToDateTime(Session["FinYearEndDate"]).Day) + "/" + Convert.ToString(Convert.ToDateTime(Session["FinYearEndDate"]).Year);
                        dtTDate.Value = DateTime.ParseExact(setdate, @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        //dt_PLQuote.Value = DateTime.ParseExact(Convert.ToString(Session["FinYearStartDate"]), @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else if (oDBEngine.GetDate().Date <= Convert.ToDateTime(Convert.ToString(Session["FinYearStartDate"])) && oDBEngine.GetDate().Date <= Convert.ToDateTime(Convert.ToString(Session["FinYearEndDate"])))
                    {
                        setdate = Convert.ToString(Convert.ToDateTime(Session["FinYearStartDate"]).Month) + "/" + Convert.ToString(Convert.ToDateTime(Session["FinYearStartDate"]).Day) + "/" + Convert.ToString(Convert.ToDateTime(Session["FinYearStartDate"]).Year);
                        dtTDate.Value = DateTime.ParseExact(setdate, @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                        //dt_PLQuote.Value = DateTime.ParseExact(Convert.ToString(Session["FinYearEndDate"]), @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    }
                }
            }
            //dt_PLQuote.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
        }


        protected void gridTDS_DataBinding(object sender, EventArgs e)
        {
            if (Session["CashBankDetails"] != null)
            {
                gridTDS.DataSource = (DataTable)Session["CashBankDetails"];
            }
        }

        //Tanmoy Hierarchy
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
        //Tanmoy Hierarchy End

        //Tanmoy Hierarchy
        public void bindHierarchy()
        {
            DataTable hierarchydt = oDBEngine.GetDataTable("select 0 as ID ,'Select' as H_Name union select ID,H_Name from V_HIERARCHY");
            if (hierarchydt != null && hierarchydt.Rows.Count > 0)
            {
                ddlHierarchy.DataTextField = "H_Name";
                ddlHierarchy.DataValueField = "ID";
                ddlHierarchy.DataSource = hierarchydt;
                ddlHierarchy.DataBind();
            }
        }
        //End Tanmoy Hierarchy


        //Tanmoy Setting Wise Lead show
        [WebMethod]
        public static String getMainAccountType(string MainAccountCode)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            string SubLedgerType = "";
            DataTable dt2 = oDBEngine.GetDataTable("select top 1 MainAccount_SubLedgerType from v_SubAccountList where mainaccount_referenceid='" + MainAccountCode + "'");
            if (dt2 != null && dt2.Rows.Count > 0)
            {
                SubLedgerType = Convert.ToString(dt2.Rows[0]["MainAccount_SubLedgerType"]);
                return SubLedgerType;
            }
            else
            {
                SubLedgerType = "";
                return SubLedgerType;
            }
        }
        //Tanmoy Setting Wise Lead show
    }
}

public class VOUCHERLISTTDS
{
    public string CashReportID { get; set; }
    public string MainAccountTDS { get; set; }
    public string bthSubAccountTDS { get; set; }
    public string WithDrawlTDS { get; set; }
    public string ReceiptTDS { get; set; }
    public string NarrationTDS { get; set; }
    public string gvColMainAccountTDS { get; set; }
    public string gvColSubAccountTDS { get; set; }
    public string gvMainAcCodeTDS { get; set; }
    public string IsSubledgerTDS { get; set; }
    public string IsTDS { get; set; }
    public string UniqueID { get; set; }

    public string IsTDSSource { get; set; }

    public string TDSPercentage { get; set; }
    public Int64 ProjectId { get; set; }
    public string Project_Code { get; set; }


}