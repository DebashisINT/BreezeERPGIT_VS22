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
using System.Collections;
using DevExpress.Web.Data;
using DataAccessLayer;
using System.ComponentModel;
using System.Linq;
using EntityLayer;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using BusinessLogicLayer.EmailDetails;
using EntityLayer.MailingSystem;
using UtilityLayer;
using ERP.OMS.Tax_Details.ClassFile;
using ERP.OMS.ViewState_class;
using ERP.Models; 

namespace ERP.OMS.Management.Activities
{
    public partial class TPurchaseInvoice : VSPage //: System.Web.UI.Page
    {
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
       // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        clsDropDownList oclsDropDownList = new clsDropDownList();
        BusinessLogicLayer.Contact oContactGeneralBL = new BusinessLogicLayer.Contact();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        string UniquePurchaseInvoice = string.Empty;
        #region Sandip Section For Approval Section Start
        ERPDocPendingApprovalBL objERPDocPendingApproval = new ERPDocPendingApprovalBL();
        #endregion Sandip Section For Approval Dtl Section End
        #region Sam Section Start
        //PurchaseInvoiceBL objPurchaseInvoice = new PurchaseInvoiceBL();
        TransitPurchaseInvoiceBL objPurchaseInvoice = new TransitPurchaseInvoiceBL();
        //string UniquePurchaseInvoice = string.Empty; 

        string PurchaseInvoiceIds = string.Empty;
        #endregion Sam Section End
        SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
        PurchaseOrderBL objPurchaseOrderBL = new PurchaseOrderBL();
        BusinessLogicLayer.RemarkCategoryBL reCat = new BusinessLogicLayer.RemarkCategoryBL();
        public string pageAccess = "";
        static string ForJournalDate = null;
        public string InvoiceCreatedFromDoc = "";
        public string InvoiceCreatedFromDoc_Ids = "";
        //string PurchaseInvoiceIds = string.Empty;
        string strPurchaseInvoiceID = "";
        GSTtaxDetails gstTaxDetails = new GSTtaxDetails();

        string userbranch = "";  //using To get Child Branch List
        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            try
            {
                if (Request.QueryString.AllKeys.Contains("status"))
                {
                    this.Page.MasterPageFile = "~/OMS/MasterPage/PopUp.Master";
                    //divcross.Visible = false;
                }
                else if (Request.QueryString.AllKeys.Contains("IsTagged"))
                {
                    this.Page.MasterPageFile = "~/OMS/MasterPage/PopUp.Master";
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
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "alert('" + ex.Message + "');", true);
            }
        }
        protected int getUdfCount()
        {
            try
            {
                DataTable udfCount = oDBEngine.GetDataTable("select 1 from tbl_master_remarksCategory rc where cat_applicablefor='TPB'  and ( exists (select * from tbl_master_udfGroup where id=rc.cat_group_id and grp_isVisible=1) or rc.cat_group_id=0)");
                return udfCount.Rows.Count;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "alert('" + ex.Message + "');", true);
                return 0;
            }
        }

        protected void Page_Init(object sender, EventArgs e) // lead add
        {
            SqlSchematype.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            //SqlIndentRequisitionNo.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
           // Sqlvendor.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            //CountrySelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
           // StateSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
           // SelectCity.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
           // SelectArea.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
           // SelectPin.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            tdsDatasource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/TPurchaseInvoice.aspx");
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            CommonBL ComBL = new CommonBL();
            string ProjectSelectInEntryModule = ComBL.GetSystemSettingsResult("ProjectSelectInEntryModule");


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


            string HierarchySelectInEntryModule = ComBL.GetSystemSettingsResult("Show_Hierarchy");


            if (!String.IsNullOrEmpty(HierarchySelectInEntryModule))
            {
                if (HierarchySelectInEntryModule.ToUpper().Trim() == "YES")
                {
                    hdnHierarchySelectInEntryModule.Value = "1";
                    ddlHierarchy.Visible = true;
                    lblHierarchy.Visible = true;
                    lookup_Project.Columns[3].Visible = true;
                }
                else if (HierarchySelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    hdnHierarchySelectInEntryModule.Value = "0";
                    ddlHierarchy.Visible = false;
                    lblHierarchy.Visible = false;
                    lookup_Project.Columns[3].Visible = false;
                }
            }

            string ProjectMandatoryInEntry = ComBL.GetSystemSettingsResult("ProjectMandatoryInEntry");

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



            if (!IsPostBack)
            {
                CommonBL cbl = new CommonBL();
                string BackDatedEntryPurchaseReturnManual = cbl.GetSystemSettingsResult("BackDatedEntryTPI");
                if (!String.IsNullOrEmpty(BackDatedEntryPurchaseReturnManual))
                {
                    if (BackDatedEntryPurchaseReturnManual.ToUpper().Trim() == "YES")
                    {
                        HdnBackDatedEntryPurchaseGRN.Value = "1";

                    }
                    else if (BackDatedEntryPurchaseReturnManual.ToUpper().Trim() == "NO")
                    {
                        HdnBackDatedEntryPurchaseGRN.Value = "0";

                    }
                }


                bindHierarchy();
                ddlHierarchy.Enabled = false;
                #region NewTaxblock
                string ItemLevelTaxDetails = string.Empty; string HSNCodewisetaxSchemid = string.Empty; string BranchWiseStateTax = string.Empty; string StateCodeWiseStateIDTax = string.Empty;
                gstTaxDetails.GetTaxData(ref ItemLevelTaxDetails, ref HSNCodewisetaxSchemid, ref BranchWiseStateTax, ref StateCodeWiseStateIDTax, "P");
                HDItemLevelTaxDetails.Value = ItemLevelTaxDetails;
                HDHSNCodewisetaxSchemid.Value = HSNCodewisetaxSchemid;
                HDBranchWiseStateTax.Value = BranchWiseStateTax;
                HDStateCodeWiseStateIDTax.Value = StateCodeWiseStateIDTax;


                #endregion
                //DataTable dtbtn = objPurchaseInvoice.PurchaseOrdTagInPurchaseInvoice("PurchaseOrdTagInTPurchaseInvoice");
                //if (dtbtn.Rows.Count > 0)
                //{
                //    string settingconfig = Convert.ToString(dtbtn.Rows[0]["Variable_Value"]);
                //    if (settingconfig.ToUpper().Trim() == "NO")
                //    {
                //rdBtn.items.remove("name_of_the_item");
                //rdl_PurchaseInvoice.Items.Remove("PO");
                //rdl_PurchaseInvoice.Attributes.CssStyle.Add("visibility", "hidden");
                //rdl_PurchaseInvoice.Items[0].Attributes[HtmlTextWriterStyle.Visibility] = "hidden";
                rdl_PurchaseInvoice.Items.RemoveAt(1);
                //rdl_PurchaseInvoice.Items[0].Attributes.Add("dsiplay","none"); 
                //}
                //else
                //{
                //    rdl_PurchaseInvoice.Items.Insert(0, new ListItem("Purchase Order", "PO"));

                //}
                //}
                #region Approvalval Section By Sam on 23052017 Start
                string branchid = "";
                string branch = "";


                if (Request.QueryString.AllKeys.Contains("status"))
                {
                    DataTable dt = objPurchaseInvoice.GetBranchIdByInvoiceID(Convert.ToString(Request.QueryString["key"]));
                    branchid = Convert.ToString(dt.Rows[0]["br"]);
                    branch = oDBEngine.getBranch(branchid, "") + branchid;

                    HttpContext.Current.Session["userbranchHierarchy"] = branch;
                    Session["LastCompany"] = Convert.ToString(dt.Rows[0]["comp"]);
                    Session["LastFinYear"] = Convert.ToString(dt.Rows[0]["finyear"]);
                    crossdiv.Visible = false;
                    btn_SaveRecords.Visible = false;
                    ApprovalCross.Visible = true;
                    ddl_Branch.Enabled = false;

                }
                else
                {
                    branchid = Convert.ToString(Session["userbranchID"]);
                    branch = oDBEngine.getBranch(branchid, "") + branchid;
                    crossdiv.Visible = true;
                    btn_SaveRecords.Visible = true;
                    ApprovalCross.Visible = false;
                    ////ddl_Branch.Enabled = false; // Change Due to Numbering Schema
                }
                #endregion Approvalval Section By Sam on 23052017 Start
                #region Session Initialization For Data Source
                this.Session["LastCompany1"] = Session["LastCompany"];
                this.Session["LastFinYear1"] = Session["LastFinYear"];
                this.Session["userbranch"] = Session["userbranchID"];
                #endregion Session Initialization For Data Source
                SetFinYearCurrentDate();
                //PopulateCustomerDetail();
                if (Session["userbranchHierarchy"] != null)
                {
                    userbranch = Convert.ToString(Session["userbranchHierarchy"]);
                }
                GetAllDropDownDetailForPurchaseInvoice(userbranch);
                IsUdfpresent.Value = Convert.ToString(getUdfCount());


                string finyear = Convert.ToString(Session["LastFinYear"]);

                #region NonInventory Section By Sam on 17052017

                Session["TPBNonInvProChgforShow"] = null;

                #endregion NonInventory Section By Sam on 17052017
                #region Session NUll initialization
                Session["TPBCustomerDetail"] = null;
                Session["TPurchaseInvoice_Id"] = null;
                Session["TPurchaseOrderFinalTaxRecord"] = null;
                Session["TProductInvoiceDetails"] = null;
                Session["TPurchaseInvoiceDetails"] = null;
                Session["TPOCWarehouseData"] = null;
                Session["TPBLoopWarehouse"] = 1;
                Session["TPBTaxDetails"] = null;
                Session["TPBAddressDtl"] = null;
                Session["TPBwarehousedetailstemp"] = null;
                Session["TPBwarehousedetailstempUpdate"] = null;
                Session["TPBwarehousedetailstempDelete"] = null;
                Session["TPBwarehousedetailstempNew"] = null;
                Session["TWbranchInEdit"] = null;
                Session["TPBNonInvProChg"] = null;
                Session["SaveModeTPB"] = null;
                #endregion Session NUll initialization
                PopulateGSTCSTVATCombo(DateTime.Now.ToString("yyyy-MM-dd"));
                PopulateChargeGSTCSTVATCombo(DateTime.Now.ToString("yyyy-MM-dd"));
                GetFinacialYearBasedQouteDate();
                PopulateTDSSchemeForNonInventoryItem();
                #region Add Mode Section Start
                if (Request.QueryString["key"] == "ADD")
                {
                    hdnADDEditMode.Value = "ADD";
                    #region To Show By Default Cursor after SAVE AND NEW
                    if (Session["SaveModeTPB"] != null)  // it has been removed from coding side of Quotation list 
                    {
                        SqlSchematype.SelectCommand = "Select * From ((Select '0' as ID,'Select' as SchemaName) Union (Select  convert(nvarchar(10),ID)+'~'+convert(nvarchar(10),b.branch_id) as ID,SchemaName+'('+b.branch_code +')'as SchemaName  From tbl_master_Idschema  join tbl_master_branch b on tbl_master_Idschema.Branch=b.branch_id  Where  TYPE_ID='50' and financial_year_id IN(select FinYear_ID from Master_FinYear where FinYear_Code='" + finyear + "') and Isnull(Branch,'') in (select s FROM dbo.GetSplit(',','" + userbranch + "')) and comapanyInt='" + Convert.ToString(Session["LastCompany"]) + "')) as X Order By ID ASC";
                        DataTable schematypedt = new DataTable();
                        //schematypedt = objPurchaseInvoice.PopulateSchemaTypeAfterInsertion(finyear, userbranch, Convert.ToString(Session["LastCompany"]));
                        //if (schematypedt.Rows.Count>0)
                        //{
                        //    ddl_numberingScheme.DataSource = schematypedt;
                        //    ddl_numberingScheme.DataBind();
                        //}
                        if (Session["schemavalueTPB"] != null)  // it has been removed from coding side of Quotation list 
                        {
                            //ddl_numberingScheme.SelectedValue = Convert.ToString(Session["schemavalueTPB"]); // it has been removed from coding side of Quotation list 
                            ddl_numberingScheme.Value = Convert.ToString(Session["schemavalueTPB"]);
                            string[] selectedbranch = new string[] { };
                            selectedbranch = Convert.ToString(Session["schemavalueTPB"]).Split('~');
                            ddl_Branch.SelectedValue = selectedbranch[1];

                        }


                        if (Convert.ToString(Session["SaveModeTPB"]) == "A")
                        {
                            dt_PLQuote.Focus();
                            txtVoucherNo.Enabled = false;
                            txtVoucherNo.Text = "Auto";
                        }
                        else if (Convert.ToString(Session["SaveModeTPB"]) == "M")
                        {
                            txtVoucherNo.Enabled = true;
                            txtVoucherNo.Text = "";

                            txtVoucherNo.Focus();

                        }
                    }
                    else if (!String.IsNullOrEmpty(Convert.ToString(Session["userbranchID"])))
                    {
                        string strdefaultBranch = Convert.ToString(Session["userbranchID"]);
                        ddl_Branch.SelectedValue = strdefaultBranch;
                        ddl_numberingScheme.Focus();
                    }
                    #endregion To Show By Default Cursor after SAVE AND NEW

                    ViewState["ActionType"] = "Add";
                    Keyval_internalId.Value = "Add";
                    hdnPageStatus.Value = "update";

                    if (!String.IsNullOrEmpty(Convert.ToString(Session["LocalCurrency"])))
                    {
                        string CompID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                        string LocalCurrency = Convert.ToString(Session["LocalCurrency"]);
                        string basedCurrency = Convert.ToString(LocalCurrency.Split('~')[0]);
                        string CurrencyId = Convert.ToString(basedCurrency[0]);
                        if (CurrencyId == "1")
                        {
                            txt_Rate.ClientEnabled = false;

                        }
                        else
                        {
                            txt_Rate.ClientEnabled = true;
                        }
                        ddl_Currency.SelectedValue = CurrencyId;
                        int ConvertedCurrencyId = Convert.ToInt32(CurrencyId);
                        //  SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
                        DataTable dt = objPurchaseOrderBL.GetCurrentConvertedRate(Convert.ToInt16(CurrencyId), ConvertedCurrencyId, CompID);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            txt_Rate.Text = Convert.ToString(dt.Rows[0]["PurchaseRate"]);
                        }
                        else
                        {
                            txt_Rate.Text = "0.00";
                        }
                    }
                    hdnPageStatus.Value = "first";
                    ddl_AmountAre.Value = "1";
                    ddl_VatGstCst.ClientEnabled = false;
                    //.................Tax..............
                    Session["TPBTaxDetails"] = null;
                    CreateDataTaxTable();
                    //..........End Tax-----------

                    // NonInventoryProductChgDtl Section Start By Sam on 16052017
                    NonInventoryProductChgDtl();
                    // NonInventoryProductChgDtl Section Start By Sam on 16052017
                }
                #endregion Add Mode Section End
                #region Edit Mode Section Start
                else
                {
                    Keyval_internalId.Value = "TPurchaseInvoice" + Request.QueryString["key"];
                    hdnPageStatus.Value = "Quoteupdate";
                    lblHeading.Text = "Modify Transit Purchase Invoice";
                    divNumberingScheme.Style.Add("display", "none");
                    lbl_NumberingScheme.Visible = false;
                    ddl_numberingScheme.ClientVisible = false;
                    txtVendorName.ClientEnabled = false;
                    //Session["TPurchaseInvoice_Id"] = Request.QueryString["key"];
                    ViewState["ActionType"] = "Edit";
                    hdnADDEditMode.Value = "Edit";
                    string strPurchaseInvoiceID = Convert.ToString(Request.QueryString["key"]);



                    Session["TPurchaseInvoice_Id"] = strPurchaseInvoiceID;

                    #region Main Header Section in Edit Mode Start
                    SetPBDetails();


                    #endregion Main Header Section End

                    // Code Added By Sam to Bind Warehouse in Edit Mode Section Start

                    //DataTable dtwarehouse = objPurchaseInvoice.PopulateWareHouseByInvoiceId(Convert.ToString(Request.QueryString["key"]));
                    ////Session["TPBwarehousedetailstemp"] = dtwarehouse;
                    //Session["TPBwarehousedetailstempNew"] = dtwarehouse;
                    //SetWareHouseDataInEditMode();
                    // Code Added By Sam to Bind Warehouse in Edit Mode Section End

                    //FillGrid();
                    //.................Tax..............
                    Session["TPBTaxDetails"] = GetTaxDataWithGST(GetTaxData(dt_PLQuote.Date.ToString("yyyy-MM-dd")));
                    DataTable quotetable = GetQuotationEditedTaxData().Tables[0];
                    if (quotetable == null)
                    {
                        CreateDataTaxTable();
                    }
                    else
                    {
                        Session["TPurchaseOrderFinalTaxRecord"] = quotetable;
                    }
                    //..........End Tax-----------
                    Session["TProductInvoiceDetails"] = GetPurchaseInvoiceBataData().Tables[0];
                    grid.DataSource = GetPurchaseInvoice();
                    grid.DataBind();

                    // hdnPageStatus.Value = "update";
                    // NonInventory Section During Editing by Sam on 18052017 Start

                    DataTable TDSdt = objPurchaseInvoice.PopulateTdsChargesByID(strPurchaseInvoiceID);
                    if (TDSdt.Rows.Count > 0)
                    {
                        string tdsbranchid = Convert.ToString(TDSdt.Rows[0]["BranchId"]);
                        if (tdsbranchid != "" && tdsbranchid != null)
                        {
                            ddl_noninventoryBranch.Value = tdsbranchid;
                        }

                        string monthid = Convert.ToString(TDSdt.Rows[0]["MonthId"]);
                        if (!string.IsNullOrEmpty(monthid))
                        {
                            ddl_month.Value = monthid;
                        }

                        string BasicAmount = Convert.ToString(TDSdt.Rows[0]["ProductBasicAmt"]);
                        txt_proamt.Text = BasicAmount;


                        Session["TPBNonInvProChgforShow"] = TDSdt;
                    }
                    else
                    {
                        Session["TPBNonInvProChgforShow"] = NonInventoryProductChgDtl();
                    }
                    PopulateBranchForTDS();

                    //Add TDS Section Tanmoy
                    DataTable dtTDSSection = objPurchaseInvoice.GetTdsSectionByID(strPurchaseInvoiceID);
                    if (dtTDSSection.Rows.Count > 0)
                    {
                        txtTDSSection.Value = Convert.ToString(dtTDSSection.Rows[0]["TDSTCS_MainAccountCode"]); ;
                        txtTDSapplAmount.Text = Convert.ToString(dtTDSSection.Rows[0]["TDSapplAmount"]);
                        txtTDSpercentage.Text = Convert.ToString(dtTDSSection.Rows[0]["TDS_RatePercentage"]);
                        txtTDSAmount.Text = Convert.ToString(dtTDSSection.Rows[0]["TDS_Amount"]);
                    }

                    //End of TDS Section Tanmoy
                    if (Request.QueryString["status"] == null)
                    {
                        IsExistsDocumentInERPDocApproveStatus(strPurchaseInvoiceID);
                    }
                    // NonInventory Section During Editing by Sam on 18052017 End

                    #region Samrat Roy -- Hide Save Button in Edit Mode
                    if (Request.QueryString["req"] != null && Request.QueryString["req"] == "V")
                    {
                        lblHeading.Text = "View Purchase Invoice";
                        lbl_quotestatusmsg.Text = "*** View Mode Only";
                        btn_SaveRecords.Visible = false;
                        ASPxButton1.Visible = false;
                        lbl_quotestatusmsg.Visible = true;
                    }
                    #endregion
                }
                #endregion Edit Mode Section End


                VisiblitySendEmail();


                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GridCallBack()", true);
                //if (hdnADDEditMode.Value == "Edit")
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Headerfunction()", true);
                //}
                //CmbScheme.Value = 0;


            }
            else
            {
                //PopulateCustomerDetail();
            }

        }

        #region Sam Section Start

        #region PrePopulated Data in Page Load Due to use Searching Functionality Section Start

        //public void PopulateCustomerDetail()
        //{
        //if (Session["TPBCustomerDetail"] == null)
        //{
        //DataTable dtCustomer = new DataTable();
        //string invtype = ddlInventory.SelectedItem.Value;
        //dtCustomer = objPurchaseInvoice.PopulateVendorsDetail();

        //dtCustomer = objPurchaseInvoice.PopulateVendorsDetailByInventoryItem(invtype);

        //if (dtCustomer != null && dtCustomer.Rows.Count > 0)
        //{
        //    lookup_Customer.DataSource = dtCustomer;
        //    lookup_Customer.DataBind();
        //    //Session["TPBCustomerDetail"] = dtCustomer;
        //}

        //}
        //else
        //{
        //    lookup_Customer.DataSource = (DataTable)Session["TPBCustomerDetail"];
        //    lookup_Customer.DataBind();
        //}

        //}
        #endregion PrePopulated Data in Page Load Due to use Searching Functionality Section End
        #region PrePopulated Data If Page is not Post Back Section Start
        public void SetFinYearCurrentDate()
        {
            dt_PLQuote.EditFormatString = objConverter.GetDateFormat("Date");
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

            dt_PLQuote.Value = DateTime.ParseExact(fDate, @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        }
        public void GetFinacialYearBasedQouteDate()
        {
            String finyear = "";
            string setdate = null;
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
                        dt_PLQuote.MinDate = Convert.ToDateTime(Convert.ToString(Session["FinYearStartDate"]));
                        dt_partyInvDt.MinDate = Convert.ToDateTime(Convert.ToString(Session["FinYearStartDate"]));
                    }
                    if (Session["FinYearEndDate"] != null)
                    {
                        dt_PLQuote.MaxDate = Convert.ToDateTime(Convert.ToString(Session["FinYearEndDate"]));
                        dt_partyInvDt.MaxDate = Convert.ToDateTime(Convert.ToString(Session["FinYearEndDate"]));
                    }
                    if (oDBEngine.GetDate().Date >= Convert.ToDateTime(Convert.ToString(Session["FinYearStartDate"])) && oDBEngine.GetDate().Date <= Convert.ToDateTime(Convert.ToString(Session["FinYearEndDate"])))
                    {

                    }
                    else if (oDBEngine.GetDate().Date >= Convert.ToDateTime(Convert.ToString(Session["FinYearStartDate"])) && oDBEngine.GetDate().Date >= Convert.ToDateTime(Convert.ToString(Session["FinYearEndDate"])))
                    {
                        setdate = Convert.ToString(Convert.ToDateTime(Session["FinYearEndDate"]).Month) + "/" + Convert.ToString(Convert.ToDateTime(Session["FinYearEndDate"]).Day) + "/" + Convert.ToString(Convert.ToDateTime(Session["FinYearEndDate"]).Year);
                        dt_PLQuote.Value = DateTime.ParseExact(setdate, @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        //dt_PLQuote.Value = DateTime.ParseExact(Convert.ToString(Session["FinYearStartDate"]), @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else if (oDBEngine.GetDate().Date <= Convert.ToDateTime(Convert.ToString(Session["FinYearStartDate"])) && oDBEngine.GetDate().Date <= Convert.ToDateTime(Convert.ToString(Session["FinYearEndDate"])))
                    {
                        setdate = Convert.ToString(Convert.ToDateTime(Session["FinYearStartDate"]).Month) + "/" + Convert.ToString(Convert.ToDateTime(Session["FinYearStartDate"]).Day) + "/" + Convert.ToString(Convert.ToDateTime(Session["FinYearStartDate"]).Year);
                        dt_PLQuote.Value = DateTime.ParseExact(setdate, @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                        //dt_PLQuote.Value = DateTime.ParseExact(Convert.ToString(Session["FinYearEndDate"]), @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    }
                }
            }
            //dt_PLQuote.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
        }

        public void GetAllDropDownDetailForPurchaseInvoice(string userbranch)
        {

            if (Session["userbranchID"] != null)
            {
                string companyId = "";
                if (Session["LastCompany"] != null)
                {
                    companyId = Convert.ToString(Session["LastCompany"]);
                }
                DataSet dst = new DataSet();
                dst = objPurchaseInvoice.GetAllDropDownDetailForPurchaseInvoice(userbranch, Convert.ToInt32(Session["userbranchID"]), companyId);
                //#region Schema Drop Down Start
                //if (dst.Tables[0] != null && dst.Tables[0].Rows.Count > 0)
                //{
                //    ddl_numberingScheme.DataTextField = "SchemaName";
                //    ddl_numberingScheme.DataValueField = "Id";
                //    ddl_numberingScheme.DataSource = dst.Tables[0];
                //    ddl_numberingScheme.DataBind();
                //}
                //#endregion Schema Drop Down Start

                #region Branch Drop Down Start
                if (dst.Tables[1] != null && dst.Tables[1].Rows.Count > 0)
                {
                    ddl_Branch.DataTextField = "branch_description";
                    ddl_Branch.DataValueField = "branch_id";
                    ddl_Branch.DataSource = dst.Tables[1];
                    ddl_Branch.DataBind();
                    //ddl_Branch.Items.Insert(0, new ListItem("Select", "0"));
                }
                if (Session["userbranchID"] != null)
                {
                    if (ddl_Branch.Items.Count > 0)
                    {
                        int branchindex = 0;
                        int cnt = 0;
                        foreach (ListItem li in ddl_Branch.Items)
                        {
                            if (li.Value == Convert.ToString(Session["userbranchID"]))
                            {
                                cnt = 1;
                                break;
                            }
                            else
                            {
                                branchindex += 1;
                            }
                        }
                        if (cnt == 1)
                        {
                            ddl_Branch.SelectedIndex = branchindex;
                        }
                        else
                        {
                            ddl_Branch.SelectedIndex = cnt;
                        }
                    }
                }

                #endregion Branch Drop Down End

                #region Cash Account DropDown Start
                if (dst.Tables[2] != null && dst.Tables[2].Rows.Count > 0)
                {
                    ddl_cashBank.TextField = "MainAccount_AccountName";
                    ddl_cashBank.ValueField = "MainAccount_AccountCode";
                    ddl_cashBank.DataSource = dst.Tables[2];
                    ddl_cashBank.DataBind();
                }
                #endregion Cash Account DropDown Start

                #region Currency Drop Down Start

                if (dst.Tables[3] != null && dst.Tables[3].Rows.Count > 0)
                {
                    ddl_Currency.DataTextField = "Currency_Name";
                    ddl_Currency.DataValueField = "Currency_ID";
                    ddl_Currency.DataSource = dst.Tables[3];
                    ddl_Currency.DataBind();
                }
                int currencyindex = 1;
                int no = 0;
                if (Session["ActiveCurrency"] != null)
                {
                    if (ddl_Currency.Items.Count > 0)
                    {
                        string[] ActCurrency = new string[] { };
                        string currency = Convert.ToString(HttpContext.Current.Session["ActiveCurrency"]);
                        ActCurrency = currency.Split('~');
                        foreach (ListItem li in ddl_Currency.Items)
                        {
                            if (li.Value == Convert.ToString(ActCurrency[0]))
                            {
                                //ddl_Currency.Items.Remove(li);
                                no = 1;
                                break;
                            }
                            else
                            {
                                currencyindex += 1;
                            }
                        }
                    }
                    ddl_Currency.Items.Insert(0, new ListItem("Select", "0"));
                    if (no == 1)
                    {
                        ddl_Currency.SelectedIndex = currencyindex;
                    }
                    else
                    {
                        ddl_Currency.SelectedIndex = no;
                    }
                }

                #endregion Currency Drop Down End

                #region TaxGroupType DropDown Start
                if (dst.Tables[4] != null && dst.Tables[4].Rows.Count > 0)
                {
                    ddl_AmountAre.TextField = "taxGrp_Description";
                    ddl_AmountAre.ValueField = "taxGrp_Id";
                    ddl_AmountAre.DataSource = dst.Tables[4];
                    ddl_AmountAre.DataBind();
                    ListEditItem li = new ListEditItem();
                    li.Text = "Import";
                    li.Value = "4";
                    ddl_AmountAre.Items.Insert(3, li);
                }
                #endregion TaxGroupType DropDown Start




            }
        }


        #endregion PrePopulated Data If Page is not Post Back Section End



        #endregion Sam Section End


        public IEnumerable GetPurchaseInvoice(DataTable PurchaseInvoicedt)
        {
            List<PurchaseInvoicedtl> PurchaseInvoiceList = new List<PurchaseInvoicedtl>();

            if (PurchaseInvoicedt != null && PurchaseInvoicedt.Rows.Count > 0)
            {
                for (int i = 0; i < PurchaseInvoicedt.Rows.Count; i++)
                {
                    PurchaseInvoicedtl PurchaseInvoices = new PurchaseInvoicedtl();

                    PurchaseInvoices.SrlNo = Convert.ToString(PurchaseInvoicedt.Rows[i]["SrlNo"]);
                    PurchaseInvoices.PurchaseInvoiceDetailID = Convert.ToString(PurchaseInvoicedt.Rows[i]["PurchaseInvoiceDetailID"]);
                    PurchaseInvoices.ProductID = Convert.ToString(PurchaseInvoicedt.Rows[i]["ProductID"]);
                    PurchaseInvoices.Description = Convert.ToString(PurchaseInvoicedt.Rows[i]["Description"]);
                    PurchaseInvoices.Quantity = Convert.ToString(PurchaseInvoicedt.Rows[i]["Quantity"]);
                    PurchaseInvoices.UOM = Convert.ToString(PurchaseInvoicedt.Rows[i]["UOM"]);
                    PurchaseInvoices.Warehouse = "";
                    //PurchaseInvoices.StockQuantity = Convert.ToString(PurchaseInvoicedt.Rows[i]["StockQuantity"]);
                    //PurchaseInvoices.StockUOM = Convert.ToString(PurchaseInvoicedt.Rows[i]["StockUOM"]);
                    PurchaseInvoices.PurchasePrice = Convert.ToString(PurchaseInvoicedt.Rows[i]["PurchasePrice"]);
                    PurchaseInvoices.Discount = Convert.ToString(PurchaseInvoicedt.Rows[i]["Discount"]);
                    PurchaseInvoices.Discountamt = Convert.ToString(PurchaseInvoicedt.Rows[i]["Discountamt"]);
                    PurchaseInvoices.Amount = Convert.ToString(PurchaseInvoicedt.Rows[i]["Amount"]);
                    PurchaseInvoices.TaxAmount = Convert.ToString(PurchaseInvoicedt.Rows[i]["TaxAmount"]);
                    PurchaseInvoices.TotalAmount = Convert.ToString(PurchaseInvoicedt.Rows[i]["TotalAmount"]);
                    PurchaseInvoices.ProductName = Convert.ToString(PurchaseInvoicedt.Rows[i]["ProductName"]);
                    if (Convert.ToString(PurchaseInvoicedt.Rows[i]["ComponentNumber"])=="0")
                    {
                        PurchaseInvoices.ComponentNumber = Convert.ToString("");
                    }
                    else
                    {
                        PurchaseInvoices.ComponentNumber = Convert.ToString(PurchaseInvoicedt.Rows[i]["ComponentNumber"]);

                    }
                    
                    PurchaseInvoices.ComponentID = Convert.ToString(PurchaseInvoicedt.Rows[i]["ComponentID"]);
                    PurchaseInvoices.TotalQty = Convert.ToString(PurchaseInvoicedt.Rows[i]["TotalQty"]);
                    PurchaseInvoices.BalanceQty = Convert.ToString(PurchaseInvoicedt.Rows[i]["BalanceQty"]);
                    PurchaseInvoices.IsComponentProduct = Convert.ToString(PurchaseInvoicedt.Rows[i]["IsComponentProduct"]);
                    PurchaseInvoiceList.Add(PurchaseInvoices);
                }
            }
            //Session["TProductInvoiceDetails"] = PurchaseInvoiceList;
            return PurchaseInvoiceList;
        }



        //public DataSet GetPurchaseOrderData()
        //{
        //    DataSet ds = new DataSet();
        //    ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
        //    proc.AddVarcharPara("@Action", 500, "PoBatchEditDetails");
        //    proc.AddVarcharPara("@PurchaseOrder_Id", 500, Convert.ToString(Session["TPurchaseInvoice_Id"]));
        //    proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
        //    proc.AddVarcharPara("@campany_Id", 500, Convert.ToString(Session["LastCompany"]));
        //    ds = proc.GetDataSet();
        //    return ds;
        //}

        #region Main Section in Edit Mode




        #endregion Main Section in Edit Mode

        public DataSet GetProductData()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
            proc.AddVarcharPara("@Action", 500, "ProductDetails");
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@campany_Id", 500, Convert.ToString(Session["LastCompany"]));
            ds = proc.GetDataSet();
            return ds;
        }
        public IEnumerable GetProduct()
        {
            List<Product> ProductList = new List<Product>();
          //  BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
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
        //    ((GridViewDataComboBoxColumn)grid.Columns["ProductID"]).PropertiesComboBox.DataSource = GetProduct();
        //}

        [WebMethod]
        public static bool CheckUniqueName(string VoucherNo)
        {
            MShortNameCheckingBL objMShortNameCheckingBL = new MShortNameCheckingBL();
            DataTable dt = new DataTable();
            Boolean status = false;
            BusinessLogicLayer.GenericMethod oGeneric = new BusinessLogicLayer.GenericMethod();

            if (VoucherNo != "" && Convert.ToString(VoucherNo).Trim() != "")
            {
                status = objMShortNameCheckingBL.CheckUnique(Convert.ToString(VoucherNo).Trim(), "0", "TansitPurchaseInvoice_Check");
            }
            return status;
        }

        [WebMethod]
        public static string ValidQuantity(string srlno, string previousqty)
        {

            string status = "1";
            if (HttpContext.Current.Session["TProductInvoiceDetails"] != null)
            {
                DataTable dt = (DataTable)HttpContext.Current.Session["TProductInvoiceDetails"];
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (Convert.ToInt32(dr["SrlNo"]) == Convert.ToInt32(srlno) && Convert.ToDecimal(dr["Quantity"]) < Convert.ToDecimal(previousqty))
                        {
                            status = "0" + "~" + Convert.ToString(dr["Quantity"]);
                        }


                    }
                }
            }
            return status;

        }






        [WebMethod]
        public static string getSchemeType(string sel_scheme_id)
        {
            //BusinessLogicLayer.DBEngine oDbEngine1 = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            //string[] scheme = oDbEngine1.GetFieldValue1("tbl_master_Idschema", "schema_type", "Id = " + Convert.ToInt32(sel_scheme_id), 1);
            //return Convert.ToString(scheme[0]);
            string strschematype = "", strschemalength = "", strschemavalue = "", Valid_From = "", Valid_Upto = "";
           // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();


            string[] schematypedtl = new string[] { };
            schematypedtl = sel_scheme_id.Split('~');
            string schematype = Convert.ToString(schematypedtl[0]);

            DataTable DT = objEngine.GetDataTable("tbl_master_Idschema", " schema_type,length,Valid_From,Valid_Upto ", " Id = " + Convert.ToInt32(schematype));

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                strschematype = Convert.ToString(DT.Rows[i]["schema_type"]);
                strschemalength = Convert.ToString(DT.Rows[i]["length"]);
                Valid_From = Convert.ToDateTime(DT.Rows[i]["Valid_From"]).ToString("MM-dd-yyyy");
                Valid_Upto = Convert.ToDateTime(DT.Rows[i]["Valid_Upto"]).ToString("MM-dd-yyyy");
                strschemavalue = strschematype + "~" + strschemalength + "~" + Valid_From + "~" + Valid_Upto;
            }
            return Convert.ToString(strschemavalue);
        }
        [WebMethod]
        public static string getIndentRequisitionDate(string IndentRequisitionNo)
        {
          //  BusinessLogicLayer.DBEngine oDbEngine1 = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            BusinessLogicLayer.DBEngine oDbEngine1 = new BusinessLogicLayer.DBEngine();
            string[] RequisitionDate = oDbEngine1.GetFieldValue1("tbl_trans_Indent", "Indent_RequisitionDate", "Indent_Id = " + Convert.ToInt32(IndentRequisitionNo), 1);
            return Convert.ToString(RequisitionDate[0]);
        }
        [WebMethod]
        public static String GetRate(string basedCurrency, string Currency_ID, string Campany_ID)
        {

            PurchaseOrderBL objPurchaseOrderBL = new PurchaseOrderBL();
            DataTable dt = objPurchaseOrderBL.GetCurrentConvertedRate(Convert.ToInt16(basedCurrency), Convert.ToInt16(Currency_ID), Campany_ID);
            string SalesRate = "";
            if (dt.Rows.Count > 0)
            {
                SalesRate = Convert.ToString(dt.Rows[0]["PurchaseRate"]);
            }

            return SalesRate;
        }
        protected void cmbContactPerson_Callback(object sender, CallbackEventArgsBase e)
        {
            cmbContactPerson.JSProperties["cpcountry"] = null;
            int i = 0;
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "BindContactPerson")
            {
                string InternalId = Convert.ToString(hdnCustomerId.Value);
                string vendorbranchid = Convert.ToString(ddl_Branch.SelectedValue);
                i = PopulateContactPersonOfCustomer(InternalId);
                if (i == 1)
                {
                    cmbContactPerson.JSProperties["cpContactdtl"] = "Y";
                }
                else
                {
                    cmbContactPerson.JSProperties["cpContactdtl"] = "N";
                }


                DataSet vendst = new DataSet();
                vendst = objPurchaseInvoice.GetCustomerDetails_InvoiceRelated(InternalId, vendorbranchid);
                if (vendst.Tables[0] != null && vendst.Tables[0].Rows.Count > 0)
                {
                    string strDueDate = Convert.ToString(vendst.Tables[0].Rows[0]["DueDate"]);
                    cmbContactPerson.JSProperties["cpDueDate"] = strDueDate;
                }
                else
                {
                    cmbContactPerson.JSProperties["cpDueDate"] = null;
                }             

                if (vendst.Tables[1] != null && vendst.Tables[1].Rows.Count > 0)
                {
                    var convertDecimal = Convert.ToDecimal(Convert.ToString(vendst.Tables[1].Rows[0]["NetOutstanding"]));
                    if (convertDecimal > 0)
                    {
                        cmbContactPerson.JSProperties["cpOutstanding"] = Convert.ToString(convertDecimal) + "(Db)";
                    }
                    else
                    {

                        cmbContactPerson.JSProperties["cpOutstanding"] = Convert.ToString(convertDecimal * -1).ToString() + "(Cr)";
                    }
                }
                else
                {
                    cmbContactPerson.JSProperties["cpOutstanding"] = "0.0";
                }              

                if (vendst.Tables[2] != null && vendst.Tables[2].Rows.Count > 0)
                {
                    string strGSTN = Convert.ToString(vendst.Tables[2].Rows[0]["CNT_GSTIN"]);
                    if (strGSTN != "")
                    {
                        cmbContactPerson.JSProperties["cpGSTN"] = "Yes";

                    }
                    else
                    {
                        cmbContactPerson.JSProperties["cpGSTN"] = "No";
                    }
                }

                if (vendst.Tables[3] != null && vendst.Tables[3].Rows.Count > 0)
                {
                    string strCountry = Convert.ToString(vendst.Tables[3].Rows[0]["add_country"]);
                    if (strCountry != "")
                    {
                        cmbContactPerson.JSProperties["cpcountry"] = strCountry;

                    }
                    else
                    {
                        cmbContactPerson.JSProperties["cpcountry"] = "1";
                    }
                }
                else
                {
                    cmbContactPerson.JSProperties["cpcountry"] = "1";
                }
            }
        }
        protected void ddl_VatGstCst_Callback(object sender, CallbackEventArgsBase e)
        {
            string type = e.Parameter.Split('~')[0];

            if (type == "SetTaxCode")
            {
                string TaxCode = e.Parameter.Split('~')[1];
                if (TaxCode != "" && TaxCode != null)
                {
                    PopulateGSTCSTVAT("2");
                    setValueForHeaderGST(ddl_VatGstCst, TaxCode);
                }
            }
            else
            {
                PopulateGSTCSTVAT(type);
            }

        }
        protected void PopulateGSTCSTVAT(string type)
        {
            DataTable dtGSTCSTVAT = new DataTable();

            if (type == "2")
            {
                dtGSTCSTVAT = objPurchaseOrderBL.PopulateGSTCSTVAT(dt_PLQuote.Date.ToString("yyyy-MM-dd"));


                #region Delete Igst,Cgst,Sgst respectively

                string CompInternalId = Convert.ToString(Session["LastCompany"]);
                string[] compGstin = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_gstin", "cmp_internalid='" + CompInternalId + "'", 1);

                string ShippingState = "";

                #region ##### Added By : Samrat Roy -- For BillingShippingUserControl ######
                string sstateCode = "";
                if (ddlPosGstTPurchase.Value.ToString() == "S")
                {
                    sstateCode = Convert.ToString(Purchase_BillingShipping.GetShippingStateId());
                }
                else if (ddlPosGstTPurchase.Value.ToString() == "B")
                {
                    sstateCode = Convert.ToString(Purchase_BillingShipping.GetBillingStateId());
                }
                ShippingState = sstateCode;

                if (ShippingState.Trim() != "")
                {
                    ShippingState = ShippingState;
                }

                #region ##### Old Code -- BillingShipping ######
                ////if (chkBilling.Checked)
                ////{
                ////    if (CmbState.Value != null)
                ////    {
                ////        ShippingState = CmbState.Text;
                ////        ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                ////    }
                ////}
                ////else
                ////{
                ////    if (CmbState1.Value != null)
                ////    {
                ////        ShippingState = CmbState1.Text;
                ////        ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                ////    }
                ////}
                #endregion

                #endregion

                if (ShippingState.Trim() != "" && compGstin[0].Trim() != "")
                {

                    if (compGstin.Length > 0)
                    {
                        if (compGstin[0].Substring(0, 2) == ShippingState)
                        {
                            //Check if the state is in union territories then only UTGST will apply
                            //   Chandigarh     Andaman and Nicobar Islands    DADRA & NAGAR HAVELI    DAMAN & DIU  Lakshadweep              PONDICHERRY
                            if (ShippingState == "4" || ShippingState == "26" || ShippingState == "25" || ShippingState == "35" || ShippingState == "31" || ShippingState == "34")
                            {
                                foreach (DataRow dr in dtGSTCSTVAT.Rows)
                                {
                                    if (Convert.ToString(dr["TaxRates_GSTtype"]).Trim() == "I" || Convert.ToString(dr["TaxRates_GSTtype"]).Trim() == "C" || Convert.ToString(dr["TaxRates_GSTtype"]).Trim() == "S")
                                    {
                                        dr.Delete();
                                    }
                                }

                            }
                            else
                            {
                                foreach (DataRow dr in dtGSTCSTVAT.Rows)
                                {
                                    if (Convert.ToString(dr["TaxRates_GSTtype"]).Trim() == "I" || Convert.ToString(dr["TaxRates_GSTtype"]).Trim() == "U")
                                    {
                                        dr.Delete();
                                    }
                                }
                            }
                            dtGSTCSTVAT.AcceptChanges();
                        }
                        else
                        {
                            foreach (DataRow dr in dtGSTCSTVAT.Rows)
                            {
                                if (Convert.ToString(dr["TaxRates_GSTtype"]).Trim() == "C" || Convert.ToString(dr["TaxRates_GSTtype"]).Trim() == "S" || Convert.ToString(dr["TaxRates_GSTtype"]).Trim() == "U")
                                {
                                    dr.Delete();
                                }
                            }
                            dtGSTCSTVAT.AcceptChanges();

                        }

                    }
                }

                //If Company GSTIN is blank then Delete All CGST,UGST,IGST,CGST
                if (compGstin[0].Trim() == "")
                {
                    foreach (DataRow dr in dtGSTCSTVAT.Rows)
                    {
                        if (Convert.ToString(dr["TaxRates_GSTtype"]).Trim() == "C" || Convert.ToString(dr["TaxRates_GSTtype"]).Trim() == "S" || Convert.ToString(dr["TaxRates_GSTtype"]).Trim() == "U" || Convert.ToString(dr["TaxRates_GSTtype"]).Trim() == "I")
                        {
                            dr.Delete();
                        }
                    }
                    dtGSTCSTVAT.AcceptChanges();
                }


                #endregion



                if (dtGSTCSTVAT != null && dtGSTCSTVAT.Rows.Count > 0)
                {
                    ddl_VatGstCst.TextField = "Taxes_Name";
                    ddl_VatGstCst.ValueField = "Taxes_ID";
                    ddl_VatGstCst.DataSource = dtGSTCSTVAT;
                    ddl_VatGstCst.DataBind();
                    ddl_VatGstCst.SelectedIndex = 0;
                }
            }
            else
            {
                ddl_VatGstCst.DataSource = null;
                ddl_VatGstCst.DataBind();
            }
        }
        protected int PopulateContactPersonOfCustomer(string InternalId)
        {

            string ContactPerson = "";
            DataTable dtContactPerson = new DataTable();
            dtContactPerson = objPurchaseInvoice.PopulateContactPersonOfCustomer(InternalId);
            if (dtContactPerson != null && dtContactPerson.Rows.Count > 0)
            {
                cmbContactPerson.TextField = "cp_name";
                cmbContactPerson.ValueField = "cp_contactId";
                cmbContactPerson.DataSource = dtContactPerson;
                cmbContactPerson.DataBind();
                cmbContactPerson.Value = cmbContactPerson.Items[0].Value;
                //foreach (DataRow dr in dtContactPerson.Rows)
                //{
                //    if (Convert.ToString(dr["Isdefault"]) == "True")
                //    {
                //        ContactPerson = Convert.ToString(dr["add_id"]);
                //        break;
                //    }
                //}
                //cmbContactPerson.SelectedItem = cmbContactPerson.Items.FindByValue(ContactPerson);
                return 1;
            }
            else
            {
                cmbContactPerson.DataSource = null;
                cmbContactPerson.DataBind();
                return 0;
                //divContactPhone.Style.Add("display", "none");
                //divContactPhone.Attributes.Add("display","none");

                //('style', 'display:block');
                //pagestyles.InnerText = ".hidden{display:none;}";
                //lblContactPhone.InnerText = ""
                //divContactPhone.Visible = false;
                //lblContactPhone.Text = "";
            }



            //dtContactPerson = objSlaesActivitiesBL.PopulateContactPersonForCustomerOrLead(InternalId);
            //oGenericMethod = new BusinessLogicLayer.GenericMethod();

            //DataTable dtCmb = new DataTable();
            //dtCmb = oGenericMethod.GetDataTable("Select city_id,city_name From tbl_master_city Where state_id=" + stateID + " Order By city_name");//+ " Order By state "
            //AspxHelper oAspxHelper = new AspxHelper();
            //if (dtCmb.Rows.Count > 0)
            //{
            //    //CmbState.Enabled = true;
            //    // oAspxHelper.Bind_Combo(CmbCity, dtCmb, "city_name", "city_id", 0);
            //}
            //else
            //{
            //    //CmbCity.Enabled = false;
            //}
        }





        #region Shipping DropDown Call Back Function End
        #region All address Dropdown Populated Data and function


        //string[,] GetState(int country)
        //{
        //    StateSelect.SelectParameters[0].DefaultValue = Convert.ToString(country);
        //    DataView view = (DataView)StateSelect.Select(DataSourceSelectArguments.Empty);
        //    string[,] DATA = new string[view.Count, 2];
        //    for (int i = 0; i < view.Count; i++)
        //    {
        //        DATA[i, 0] = Convert.ToString(view[i][0]);
        //        DATA[i, 1] = Convert.ToString(view[i][1]);
        //    }
        //    return DATA;

        //}

        //protected void FillStateCombo(ASPxComboBox cmb, int country)
        //{

        //    string[,] state = GetState(country);
        //    cmb.Items.Clear();

        //    for (int i = 0; i < state.GetLength(0); i++)
        //    {
        //        cmb.Items.Add(state[i, 1], state[i, 0]);
        //    }
        //    //cmb.SelectedIndex = -1;
        //    // Code Commented By Sandip on 08032017 To avoid Select Option By default End
        //    //cmb.Items.Insert(0, new ListEditItem("Select", "0"));
        //    // Code Above Commented By Sandip on 08032017 To avoid Select Option By default End
        //}


        //string[,] GetCities(int state)
        //{


        //    SelectCity.SelectParameters[0].DefaultValue = Convert.ToString(state);
        //    DataView view = (DataView)SelectCity.Select(DataSourceSelectArguments.Empty);
        //    string[,] DATA = new string[view.Count, 2];
        //    for (int i = 0; i < view.Count; i++)
        //    {
        //        DATA[i, 0] = Convert.ToString(view[i][0]);
        //        DATA[i, 1] = Convert.ToString(view[i][1]);
        //    }
        //    return DATA;

        //}
        //protected void FillCityCombo(ASPxComboBox cmb, int state)
        //{

        //    string[,] cities = GetCities(state);
        //    cmb.Items.Clear();

        //    for (int i = 0; i < cities.GetLength(0); i++)
        //    {
        //        cmb.Items.Add(cities[i, 1], cities[i, 0]);
        //    }
        //    //cmb.SelectedIndex = -1;
        //    // Code Commented By Sandip on 08032017 To avoid Select Option By default End
        //    //cmb.Items.Insert(0, new ListEditItem("Select", "0"));
        //    // Code Above Commented By Sandip on 08032017 To avoid Select Option By default End
        //}
        //protected void FillPinCombo(ASPxComboBox cmb, int city)
        //{
        //    string[,] pin = GetPin(city);
        //    cmb.Items.Clear();

        //    for (int i = 0; i < pin.GetLength(0); i++)
        //    {
        //        cmb.Items.Add(pin[i, 1], pin[i, 0]);
        //    }
        //    //cmb.SelectedIndex = -1;
        //}
        //string[,] GetPin(int city)
        //{
        //    SelectPin.SelectParameters[0].DefaultValue = Convert.ToString(city);
        //    DataView view = (DataView)SelectPin.Select(DataSourceSelectArguments.Empty);
        //    string[,] DATA = new string[view.Count, 2];
        //    for (int i = 0; i < view.Count; i++)
        //    {
        //        DATA[i, 0] = Convert.ToString(view[i][0]);
        //        DATA[i, 1] = Convert.ToString(view[i][1]);
        //    }
        //    return DATA;
        //}
        //string[,] GetArea(int city)
        //{
        //    SelectArea.SelectParameters[0].DefaultValue = Convert.ToString(city);
        //    DataView view = (DataView)SelectArea.Select(DataSourceSelectArguments.Empty);
        //    string[,] DATA = new string[view.Count, 2];
        //    for (int i = 0; i < view.Count; i++)
        //    {
        //        DATA[i, 0] = Convert.ToString(view[i][0]);
        //        DATA[i, 1] = Convert.ToString(view[i][1]);
        //    }
        //    return DATA;
        //}

        //protected void FillAreaCombo(ASPxComboBox cmb, int city)
        //{
        //    string[,] area = GetArea(city);
        //    cmb.Items.Clear();

        //    for (int i = 0; i < area.GetLength(0); i++)
        //    {
        //        cmb.Items.Add(area[i, 1], area[i, 0]);
        //    }
        //    //cmb.SelectedIndex = -1;
        //    // Code Commented By Sandip on 08032017 To avoid Select Option By default Start
        //    //cmb.Items.Insert(0, new ListEditItem("Select", "0"));
        //    // Code Above Commented By Sandip on 08032017 To avoid Select Option By default End
        //}

        #endregion

        #region Shipping DropDown Call Back Function End
        //protected void cmbState_OnCallback(object source, CallbackEventArgsBase e)
        //{
        //    if (e.Parameter != "")
        //    {
        //        FillStateCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
        //    }
        //    //else
        //    //{
        //    //    FillStateCombo(source as ASPxComboBox, 0);
        //    //}
        //}
        //protected void cmbCity_OnCallback(object source, CallbackEventArgsBase e)
        //{
        //    if (e.Parameter != "")
        //    {
        //        FillCityCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
        //    }
        //    //else
        //    //{
        //    //    FillCityCombo(source as ASPxComboBox, 0);
        //    //}
        //}
        //protected void cmbPin_OnCallback(object source, CallbackEventArgsBase e)
        //{
        //    if (e.Parameter != "")
        //    {
        //        FillPinCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
        //    }
        //    //else
        //    //{
        //    //    FillPinCombo(source as ASPxComboBox, 0);
        //    //}
        //}
        //protected void cmbArea_OnCallback(object source, CallbackEventArgsBase e)
        //{
        //    if (e.Parameter != "")
        //    {
        //        FillAreaCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
        //    }
        //    //else
        //    //{
        //    //    FillAreaCombo(source as ASPxComboBox, 0);
        //    //}
        //}

        #endregion Billing DropDown Call Back Function End

        #region Shipping DropDown Call Back Function Start
        //protected void cmbState1_OnCallback(object source, CallbackEventArgsBase e)
        //{
        //    if (e.Parameter != "")
        //    {
        //        FillStateCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
        //    }
        //    //else
        //    //{
        //    //    FillStateCombo(source as ASPxComboBox, 0);
        //    //}
        //}

        //protected void cmbCity1_OnCallback(object source, CallbackEventArgsBase e)
        //{
        //    if (e.Parameter != "")
        //    {
        //        FillCityCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
        //    }
        //    //else
        //    //{
        //    //    FillCityCombo(source as ASPxComboBox, 0);
        //    //}
        //}
        //protected void cmbArea1_OnCallback(object source, CallbackEventArgsBase e)
        //{
        //    if (e.Parameter != "")
        //    {
        //        FillAreaCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
        //    }
        //    //else
        //    //{
        //    //    FillAreaCombo(source as ASPxComboBox, 0);
        //    //}
        //}
        //protected void cmbPin1_OnCallback(object source, CallbackEventArgsBase e)
        //{
        //    if (e.Parameter != "")
        //    {
        //        FillPinCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
        //    }
        //    //else
        //    //{
        //    //    FillPinCombo(source as ASPxComboBox, 0);
        //    //}
        //}

        #endregion Shipping DropDown Call Back Function End


        #endregion Billing DropDown Call Back Function End

        #region  Batch Grid  ProductInvoiceDetails
        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {

            string strSplitCommand = e.Parameters.Split('~')[0];
            //if (strSplitCommand == "Display")
            if (strSplitCommand == "Display")
            {

                string IsDeleteFrom = Convert.ToString(hdfIsDelete.Value);
                string IsCompFrom = Convert.ToString(hdfIsComp.Value);
                if (IsDeleteFrom == "D" || IsCompFrom == "C")
                {
                    hdnPageStatus.Value = "delete";
                    DataTable POdt = (DataTable)Session["TProductInvoiceDetails"];
                    grid.DataSource = GetPurchaseInvoice(POdt);
                    grid.DataBind();
                }

                if (e.Parameters.Split('~').Length > 1)
                {
                    if (e.Parameters.Split('~')[1] == "fromComponent")
                    {
                        grid.JSProperties["cpComponent"] = "true";
                    }
                }
            }
            else if (strSplitCommand == "CurrencyChangeDisplay")
            {
                if (Session["TPurchaseInvoiceDetails"] != null)
                {
                    DataTable PurchaseInvoicedt = (DataTable)Session["TPurchaseInvoiceDetails"];

                    string BaseCurrencyId = Convert.ToString(Session["LocalCurrency"]).Split('~')[0];
                    string CurrentCurrencyId = Convert.ToString(ddl_Currency.SelectedValue);

                    string strCurrRate = Convert.ToString(txt_Rate.Text);
                    string strPreviouseRate = Convert.ToString(e.Parameters.Split('~')[1]);
                    decimal strRate = 1;

                    if (strCurrRate != "")
                    {
                        strRate = Convert.ToDecimal(strCurrRate);
                        if (strRate == 0) strRate = 1;
                    }

                    foreach (DataRow dr in PurchaseInvoicedt.Rows)
                    {
                        string Status = Convert.ToString(dr["Status"]);
                        if (Status != "D")
                        {
                            string ProductDetails = Convert.ToString(dr["ProductID"]);
                            string QuantityValue = Convert.ToString(dr["Quantity"]);
                            string Discount = Convert.ToString(dr["Discount"]);
                            string strPurchasePrice = Convert.ToString(dr["PurchasePrice"]);

                            string[] ProductDetailsList = ProductDetails.Split(new string[] { "||@||" }, StringSplitOptions.None);
                            string strMultiplier = ProductDetailsList[7];
                            string strFactor = ProductDetailsList[8];

                            if (Convert.ToDecimal(strPreviouseRate) != 0 && Convert.ToDecimal(strPreviouseRate) != 1)
                            {
                                strPurchasePrice = Convert.ToString(Convert.ToDecimal(strPurchasePrice) * Convert.ToDecimal(strPreviouseRate));
                            }

                            if (BaseCurrencyId == CurrentCurrencyId)
                            {
                                strPurchasePrice = ProductDetailsList[6];
                            }

                            decimal PurchasePrice = Math.Round(Convert.ToDecimal(strPurchasePrice) / strRate, 2);
                            string Amount = Convert.ToString(Math.Round((Convert.ToDecimal(QuantityValue) * Convert.ToDecimal(strFactor) * PurchasePrice), 2));
                            string amountAfterDiscount = Convert.ToString(Math.Round((Convert.ToDecimal(Amount) - ((Convert.ToDecimal(Discount) * Convert.ToDecimal(Amount)) / 100)), 2));

                            dr["PurchasePrice"] = Convert.ToString(Math.Round(PurchasePrice, 2));
                            dr["Amount"] = amountAfterDiscount;
                            dr["TaxAmount"] = ReCalculateTaxAmount(Convert.ToString(dr["SrlNo"]), Convert.ToDouble(amountAfterDiscount));
                            dr["TotalAmount"] = Convert.ToDecimal(dr["Amount"]) + Convert.ToDecimal(dr["TaxAmount"]);
                        }
                    }

                    Session["TPurchaseInvoiceDetails"] = PurchaseInvoicedt;

                    DataView dvData = new DataView(PurchaseInvoicedt);
                    dvData.RowFilter = "Status <> 'D'";

                    grid.DataSource = GetPurchaseInvoice(dvData.ToTable());
                    grid.DataBind();
                }
            }
            else if (strSplitCommand == "CurrencyRateChangeDisplay")
            {
                if (Session["TPurchaseInvoiceDetails"] != null)
                {
                    DataTable PurchaseInvoicedt = (DataTable)Session["TPurchaseInvoiceDetails"];

                    string BaseCurrencyId = Convert.ToString(Session["LocalCurrency"]).Split('~')[0];
                    string CurrentCurrencyId = Convert.ToString(ddl_Currency.SelectedValue);

                    string strCurrRate = Convert.ToString(txt_Rate.Text);
                    string strPreviouseRate = Convert.ToString(e.Parameters.Split('~')[1]);
                    decimal strRate = 1;

                    if (strCurrRate != "")
                    {
                        strRate = Convert.ToDecimal(strCurrRate);
                        if (strRate == 0) strRate = 1;
                    }

                    foreach (DataRow dr in PurchaseInvoicedt.Rows)
                    {
                        string Status = Convert.ToString(dr["Status"]);
                        if (Status != "D")
                        {
                            string ProductDetails = Convert.ToString(dr["ProductID"]);
                            string QuantityValue = Convert.ToString(dr["Quantity"]);
                            string Discount = Convert.ToString(dr["Discount"]);
                            string strPurchasePrice = Convert.ToString(dr["PurchasePrice"]);

                            string[] ProductDetailsList = ProductDetails.Split(new string[] { "||@||" }, StringSplitOptions.None);
                            string strMultiplier = ProductDetailsList[7];
                            string strFactor = ProductDetailsList[8];

                            if (Convert.ToDecimal(strPreviouseRate) != 0 && Convert.ToDecimal(strPreviouseRate) != 1)
                            {
                                strPurchasePrice = Convert.ToString(Convert.ToDecimal(strPurchasePrice) * Convert.ToDecimal(strPreviouseRate));
                            }

                            decimal PurchasePrice = Math.Round(Convert.ToDecimal(strPurchasePrice) / strRate, 2);
                            string Amount = Convert.ToString(Math.Round((Convert.ToDecimal(QuantityValue) * Convert.ToDecimal(strFactor) * PurchasePrice), 2));
                            string amountAfterDiscount = Convert.ToString(Math.Round((Convert.ToDecimal(Amount) - ((Convert.ToDecimal(Discount) * Convert.ToDecimal(Amount)) / 100)), 2));

                            dr["PurchasePrice"] = Convert.ToString(Math.Round(PurchasePrice, 2));
                            dr["Amount"] = amountAfterDiscount;
                            dr["TaxAmount"] = ReCalculateTaxAmount(Convert.ToString(dr["SrlNo"]), Convert.ToDouble(amountAfterDiscount));
                            dr["TotalAmount"] = Convert.ToDecimal(dr["Amount"]) + Convert.ToDecimal(dr["TaxAmount"]);
                        }
                    }

                    Session["TPurchaseInvoiceDetails"] = PurchaseInvoicedt;

                    DataView dvData = new DataView(PurchaseInvoicedt);
                    dvData.RowFilter = "Status <> 'D'";

                    grid.DataSource = GetPurchaseInvoice(dvData.ToTable());
                    grid.DataBind();
                }
            }
            else if (strSplitCommand == "DateChangeDisplay")
            {
                if (Session["TPurchaseInvoiceDetails"] != null)
                {
                    DataTable PurchaseInvoicedt = (DataTable)Session["TPurchaseInvoiceDetails"];

                    string strCurrRate = Convert.ToString(txt_Rate.Text);
                    decimal strRate = 1;

                    if (strCurrRate != "")
                    {
                        strRate = Convert.ToDecimal(strCurrRate);
                        if (strRate == 0) strRate = 1;
                    }

                    foreach (DataRow dr in PurchaseInvoicedt.Rows)
                    {
                        string Status = Convert.ToString(dr["Status"]);
                        if (Status != "D")
                        {
                            dr["TaxAmount"] = 0;
                            dr["TotalAmount"] = dr["Amount"];
                        }
                    }

                    Session["TPurchaseInvoiceDetails"] = PurchaseInvoicedt;

                    DataView dvData = new DataView(PurchaseInvoicedt);
                    dvData.RowFilter = "Status <> 'D'";

                    grid.DataSource = GetPurchaseInvoice(dvData.ToTable());
                    grid.DataBind();

                }
            }
            else if (strSplitCommand == "GridBlank")
            {
                Session["TProductInvoiceDetails"] = null;
                grid.DataSource = null;
                grid.DataBind();
            }
            else if (strSplitCommand == "BindGridOnQuotation")
            {
                ASPxGridView gv = sender as ASPxGridView;
                string command = e.Parameters.ToString();
                string State = Convert.ToString(e.Parameters.Split('~')[1]);
                string strType = Convert.ToString(rdl_PurchaseInvoice.SelectedValue);
                String QuoComponent1 = "";
                string strAction = "";
                string Product_id1 = "";
                //for (int i = 0; i < grid_Products.GetSelectedFieldValues("Quotation_No").Count; i++)
                for (int i = 0; i < grid_Products.GetSelectedFieldValues("ComponentDetailsID").Count; i++)
                {
                    QuoComponent1 += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("ComponentDetailsID")[i]);
                    //Product_id1 += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("ProductID")[i]);
                    //QuoComponent1 += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("Quotation_No")[i]);
                    //Product_id1 += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("ProductID")[i]);

                }
                if (QuoComponent1 != "")
                {
                    QuoComponent1 = QuoComponent1.TrimStart(',');


                    if (strType == "PO")
                    {
                        strAction = "GetSeletedOrderProducts";

                    }
                    else if (strType == "PC")
                    {
                        strAction = "GetSeletedChallanProducts";

                    }
                    // Code of Sudip Start
                    string strInvoiceID = Convert.ToString(Session["PurchaseInvoiceID"]);
                    DataTable dt_QuotationDetails = objPurchaseInvoice.GetSelectedComponentProductList(strAction, QuoComponent1, strInvoiceID);
                    Session["TPurchaseInvoiceDetails"] = dt_QuotationDetails;


                    // Code Added By sandip on 27042017 to add or remove selecteded product from UI Interface and Store it to Session["TProductInvoiceDetails"] session

                    Session["TProductInvoiceDetails"] = dt_QuotationDetails;  // 

                    // Code End By sandip on 27042017 to add or remove selecteded product from UI Interface and Store it to Session["TProductInvoiceDetails"] session


                    //grid.DataSource = GetPurchaseOrderInfo(dt_QuotationDetails);
                    grid.DataSource = GetPurchaseInvoice(dt_QuotationDetails);
                    grid.DataBind();

                    #region Code Commented by Sam on 07062017 to Arrange data according to warehouse Section
                    //Session["TPBwarehousedetailstemp"] = GetTaggingWarehouseData(QuoComponent1, strType);
                    //Session["TPBwarehousedetailstempNew"] = GetTaggingWarehouseData(QuoComponent1, strType);
                    //SetWareHouseDataInEditMode();
                    #endregion Code Commented by Sam on 07062017 to Arrange data according to warehouse Section

                    Session["TPurchaseOrderFinalTaxRecord"] = GetComponentEditedTaxData(QuoComponent1, strType);
                    //Session["PBFinalTaxRecord"] = GetComponentEditedTaxData(QuoComponent1, strType);
                }
            }
            //else if (strSplitCommand == "BindGridOnQuotation")
            //{
            //    ASPxGridView gv = sender as ASPxGridView;
            //    string command = e.Parameters.ToString();
            //    string State = Convert.ToString(e.Parameters.Split('~')[1]);
            //    String QuoComponent1 = "";
            //    string Product_id1 = "";
            //    string QuoteDetails_Id = "";

            //    for (int i = 0; i < grid_Products.GetSelectedFieldValues("Quotation_No").Count; i++)
            //    {

            //        QuoComponent1 += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("Quotation_No")[i]);
            //        Product_id1 += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("ProductID")[i]);
            //        QuoteDetails_Id += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("QuoteDetails_Id")[i]);

            //    }
            //    QuoComponent1 = QuoComponent1.TrimStart(',');
            //    Product_id1 = Product_id1.TrimStart(',');
            //    QuoteDetails_Id = QuoteDetails_Id.TrimStart(',');


            //    string Quote_Nos = Convert.ToString(e.Parameters.Split('~')[1]);

            //    if (Quote_Nos != "$")
            //    {

            //        DataTable dt_QuotationDetails = new DataTable();
            //        string IdKey = Convert.ToString(Request.QueryString["key"]);
            //        if (!string.IsNullOrEmpty(IdKey))
            //        {
            //            if (IdKey != "ADD")
            //            {
            //                dt_QuotationDetails = objPurchaseOrderBL.GetIndentDetailsForPOGridBind(QuoComponent1, QuoteDetails_Id, Product_id1,"Edit");
            //            }
            //            else
            //            {
            //                dt_QuotationDetails = objPurchaseOrderBL.GetIndentDetailsForPOGridBind(QuoComponent1, QuoteDetails_Id, Product_id1,"Add");
            //            }

            //        }
            //        else
            //        {
            //            //dt_QuotationDetails = objPurchaseOrderBL.GetIndentDetailsForPOGridBind(QuoComponent1, QuoteDetails_Id, Product_id1);
            //        }
            //        Session["TProductInvoiceDetails"] = null;
            //        grid.DataSource = GetSalesOrderInfo(dt_QuotationDetails, IdKey);
            //        grid.DataBind();
            //    }
            //    else
            //    {
            //        grid.DataSource = null;
            //        grid.DataBind();
            //    }

            //}
        }


        public IEnumerable GetSalesOrderInfo(DataTable SalesOrderdt1, string Order_Id)
        {
            List<SalesOrder> OrderList = new List<SalesOrder>();
            DataColumnCollection dtC = SalesOrderdt1.Columns;



            for (int i = 0; i < SalesOrderdt1.Rows.Count; i++)
            {
                SalesOrder Orders = new SalesOrder();

                Orders.SrlNo = Convert.ToString(i + 1);
                Orders.OrderDetails_Id = Convert.ToString(i + 1);
                Orders.ProductID = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_ProductId"]);
                Orders.gvColDiscription = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_ProductDescription"]);
                Orders.Quantity = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_Quantity"]);
                Orders.gvColUOM = Convert.ToString(SalesOrderdt1.Rows[i]["UOM_ShortName"]);
                Orders.Warehouse = "";
                Orders.gvColStockQty = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_StockQty"]);
                Orders.gvColStockUOM = Convert.ToString(SalesOrderdt1.Rows[i]["UOM_ShortName"]);
                Orders.PurchasePrice = "";
                Orders.Discount = "";
                Orders.Amount = "";
                Orders.TaxAmount = "";
                Orders.TotalAmount = "";

                if (!string.IsNullOrEmpty(Convert.ToString(SalesOrderdt1.Rows[i]["Indent_No"])))//Added on 15-02-2017
                { Orders.Indent = Convert.ToInt64(SalesOrderdt1.Rows[i]["Indent_No"]); }
                else
                { Orders.Indent = 0; }
                if (dtC.Contains("ComponentNumber"))
                {
                    Orders.Indent_Num = Convert.ToString(SalesOrderdt1.Rows[i]["ComponentNumber"]);//subhabrata on 21-02-2017
                }
                if (!string.IsNullOrEmpty(Convert.ToString(SalesOrderdt1.Rows[i]["Products_Name"])))
                { Orders.ProductName = Convert.ToString(SalesOrderdt1.Rows[i]["Products_Name"]); }
                else
                {
                    Orders.ProductName = "";
                }

                // Mapping With Warehouse with Product Srl No

                string strQuoteDetails_Id = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_Id"]).Trim();



                // End


                OrderList.Add(Orders);
            }

            BindSessionByDatatable(SalesOrderdt1);

            return OrderList;
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
        protected void grid_DataBinding(object sender, EventArgs e)
        {
            //grid.DataSource = GetVoucher();
            if (Session["TProductInvoiceDetails"] != null)
            {
                DataTable POdt = (DataTable)Session["TProductInvoiceDetails"];
                DataView dvData = new DataView(POdt);
                dvData.RowFilter = "Status <> 'D'";
                grid.DataSource = GetPurchaseInvoice(dvData.ToTable());
            }
        }

        public DataTable CreatePurchaseInvoicedt()
        {
            DataTable PurchaseInvoicedt = new DataTable();
            PurchaseInvoicedt.Columns.Add("SrlNo", typeof(string));
            PurchaseInvoicedt.Columns.Add("PurchaseInvoiceDetailID", typeof(string));
            //PurchaseInvoicedt.Columns.Add("PurchaseInvoiceID", typeof(string));
            PurchaseInvoicedt.Columns.Add("ProductID", typeof(string));
            PurchaseInvoicedt.Columns.Add("Description", typeof(string));
            PurchaseInvoicedt.Columns.Add("Quantity", typeof(string));
            PurchaseInvoicedt.Columns.Add("UOM", typeof(string));
            PurchaseInvoicedt.Columns.Add("Warehouse", typeof(string));
            PurchaseInvoicedt.Columns.Add("StockQuantity", typeof(string));
            PurchaseInvoicedt.Columns.Add("StockUOM", typeof(string));
            PurchaseInvoicedt.Columns.Add("PurchasePrice", typeof(string));
            PurchaseInvoicedt.Columns.Add("Discount", typeof(string));
            PurchaseInvoicedt.Columns.Add("DiscountAmt", typeof(string));
            PurchaseInvoicedt.Columns.Add("Amount", typeof(string));
            PurchaseInvoicedt.Columns.Add("TaxAmount", typeof(string));
            PurchaseInvoicedt.Columns.Add("TotalAmount", typeof(string));
            PurchaseInvoicedt.Columns.Add("Status", typeof(string));
            //PurchaseInvoicedt.Columns.Add("Indent_No", typeof(string));
            PurchaseInvoicedt.Columns.Add("ProductName", typeof(string));

            // Code added by sam for tagging purpose Start

            PurchaseInvoicedt.Columns.Add("ComponentID", typeof(string));
            PurchaseInvoicedt.Columns.Add("ComponentNumber", typeof(string));
            PurchaseInvoicedt.Columns.Add("TotalQty", typeof(string));
            PurchaseInvoicedt.Columns.Add("BalanceQty", typeof(string));
            PurchaseInvoicedt.Columns.Add("IsComponentProduct", typeof(string));
            return PurchaseInvoicedt;


        }

        protected void grid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            grid.JSProperties["cpSaveSuccessOrFail"] = null;
            grid.JSProperties["cpPurchaseOrderNo"] = null;
            DataTable PurchaseInvoicedt = new DataTable();
            string IsDeleteFrom = Convert.ToString(hdfIsDelete.Value);
            string IsCompFrom = Convert.ToString(hdfIsComp.Value);

            #region Table Creation For Batch Grid Section Start
            if (Session["TProductInvoiceDetails"] != null)
            {
                DataTable dt = new DataTable();
                dt = (DataTable)Session["TProductInvoiceDetails"];                
                PurchaseInvoicedt = dt;
            }
            else
            {
                PurchaseInvoicedt = CreatePurchaseInvoicedt();             
               
            }
            #endregion Table Creation For Batch Grid Section Start

            #region New Insert Detail  of Batch Grid Section Start
            foreach (var args in e.InsertValues)
            {
                string ProductDetails = Convert.ToString(args.NewValues["ProductID"]);

                string SrlNo = "";
                if (ProductDetails != "" && ProductDetails != "0")
                {
                    SrlNo = Convert.ToString(args.NewValues["SrlNo"]);
                    string[] ProductDetailsList = ProductDetails.Split(new string[] { "||@||" }, StringSplitOptions.None);
                    string ProductID = ProductDetailsList[0];
                    string IsComponentProduct = (Convert.ToString(ProductDetailsList[16]) != "") ? Convert.ToString(ProductDetailsList[16]) : "N";
                    string Description = ProductDetailsList[1];
                    string ProductName = Convert.ToString(args.NewValues["ProductName"]);
                    string Quantity = Convert.ToString(args.NewValues["Quantity"]);
                    //string UOM = Convert.ToString(ProductDetailsList[3]);
                    string UOM = Convert.ToString(args.NewValues["UOM"]);
                    string Warehouse = Convert.ToString(args.NewValues["Warehouse"]);
                    string StockQuantity = Convert.ToString(args.NewValues["Quantity"]);
                    string StockUOM = Convert.ToString(args.NewValues["StockUOM"]);
                    //string StockUOM = Convert.ToString(ProductDetailsList[5]);
                    string PurchasePrice = Convert.ToString(args.NewValues["PurchasePrice"]);                   
                    string Discount = Convert.ToString(args.NewValues["Discount"]);
                    string DiscountAmt = (Convert.ToString(args.NewValues["Discountamt"]) != "") ? Convert.ToString(args.NewValues["Discountamt"]) : "0";
                    string Amount = (Convert.ToString(args.NewValues["Amount"]) != "") ? Convert.ToString(args.NewValues["Amount"]) : "0";
                    string TaxAmount = (Convert.ToString(args.NewValues["TaxAmount"]) != "") ? Convert.ToString(args.NewValues["TaxAmount"]) : "0";
                    string TotalAmount = (Convert.ToString(args.NewValues["TotalAmount"]) != "") ? Convert.ToString(args.NewValues["TotalAmount"]) : "0";
                    string taggingID = (Convert.ToString(args.NewValues["ComponentID"]) != "") ? Convert.ToString(args.NewValues["ComponentID"]) : "0";

                    string taggingNo = (Convert.ToString(args.NewValues["ComponentNumber"]) != "") ? Convert.ToString(args.NewValues["ComponentNumber"]) : "0";
                    string TotalQty = (Convert.ToString(args.NewValues["TotalQty"]) != "") ? Convert.ToString(args.NewValues["TotalQty"]) : "0";
                    string BalanceQty = (Convert.ToString(args.NewValues["BalanceQty"]) != "") ? Convert.ToString(args.NewValues["BalanceQty"]) : "0";
                  
                    PurchaseInvoicedt.Rows.Add(SrlNo, "0", ProductDetails, Description, Quantity, UOM, Warehouse, StockQuantity, StockUOM, PurchasePrice, Discount, DiscountAmt,
                        Amount, TaxAmount, TotalAmount, "I", ProductName, taggingID, taggingNo, TotalQty, BalanceQty, IsComponentProduct);

                    if (IsComponentProduct == "Y")
                    {
                        if (hdfIsComp.Value == "C")
                        {
                            DataTable ComponentTable = objPurchaseInvoice.GetLinkedProductList("LinkedProduct", ProductID);
                            foreach (DataRow drr in ComponentTable.Rows)
                            {
                                string sProductsID = Convert.ToString(drr["sProductsID"]);
                                string Products_Description = Convert.ToString(drr["Products_Description"]);
                                string Sales_UOM_Name = Convert.ToString(drr["Sales_UOM_Name"]);
                                string Conversion_Multiplier = Convert.ToString(drr["Conversion_Multiplier"]);
                                string Stk_UOM_Name = Convert.ToString(drr["Stk_UOM_Name"]);
                                string Product_SalePrice = Convert.ToString(drr["Product_SalePrice"]);
                                string Products_Name = Convert.ToString(drr["Products_Name"]);
                                string StkQty = Convert.ToString(Convert.ToDecimal(Quantity) * Convert.ToDecimal(Conversion_Multiplier));
                                SrlNo = Convert.ToString(Convert.ToInt32(SrlNo) + 1);
                                PurchaseInvoicedt.Rows.Add(SrlNo, "0", sProductsID, Products_Description, Quantity, Sales_UOM_Name, "", StkQty, Stk_UOM_Name, Product_SalePrice, "0.0", "0.0", "0.0", "0.0", "0.0", "I", Products_Name, taggingID, taggingNo, "0.0", "0.0", "C");
                            }
                            hdfIsComp.Value = "";
                        }
                    }
                }
            }
            #endregion New Insert Detail  of Batch Grid Section End

            #region Update Detail  of Batch Grid Section Start
            foreach (var args in e.UpdateValues)
            {
                string SrlNo = Convert.ToString(args.NewValues["SrlNo"]);               
                string OrderDetails_Id = Convert.ToString(args.Keys["PurchaseInvoiceDetailID"]);
                string ProductDetails = Convert.ToString(args.NewValues["ProductID"]);

                bool isDeleted = false;
                foreach (var arg in e.DeleteValues)
                {
                    string DeleteID = Convert.ToString(arg.Keys["PurchaseInvoiceDetailID"]);
                    if (DeleteID == OrderDetails_Id)
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
                        string IsComponentProduct = (Convert.ToString(ProductDetailsList[16]) != "") ? Convert.ToString(ProductDetailsList[16]) : "N";
                        string Description = ProductDetailsList[1];
                        string Quantity = Convert.ToString(args.NewValues["Quantity"]);
                        //string UOM = Convert.ToString(ProductDetailsList[3]);
                        string UOM = Convert.ToString(args.NewValues["UOM"]);
                        string ProductName = Convert.ToString(args.NewValues["ProductName"]);                      
                        string Warehouse = Convert.ToString(args.NewValues["Warehouse"]);
                        string StockQuantity = Convert.ToString(args.NewValues["Quantity"]);                      
                        //string StockUOM = Convert.ToString(ProductDetailsList[5]);
                        string StockUOM = Convert.ToString(args.NewValues["StockUOM"]);
                        string PurchasePrice = Convert.ToString(args.NewValues["PurchasePrice"]);                       
                        string Discount = Convert.ToString(args.NewValues["Discount"]);
                        string DiscountAmt = (Convert.ToString(args.NewValues["Discountamt"]) != "") ? Convert.ToString(args.NewValues["Discountamt"]) : "0";
                        string Amount = (Convert.ToString(args.NewValues["Amount"]) != "") ? Convert.ToString(args.NewValues["Amount"]) : "0";
                        string TaxAmount = (Convert.ToString(args.NewValues["TaxAmount"]) != "") ? Convert.ToString(args.NewValues["TaxAmount"]) : "0";
                        string TotalAmount = (Convert.ToString(args.NewValues["TotalAmount"]) != "") ? Convert.ToString(args.NewValues["TotalAmount"]) : "0";                       
                        string taggingID = (Convert.ToString(args.NewValues["ComponentID"]) != "") ? Convert.ToString(args.NewValues["ComponentID"]) : "0";
                        string taggingNo = (Convert.ToString(args.NewValues["ComponentNumber"]) != "") ? Convert.ToString(args.NewValues["ComponentNumber"]) : "0";
                        string TotalQty = (Convert.ToString(args.NewValues["TotalQty"]) != "") ? Convert.ToString(args.NewValues["TotalQty"]) : "0";
                        string BalanceQty = (Convert.ToString(args.NewValues["BalanceQty"]) != "") ? Convert.ToString(args.NewValues["BalanceQty"]) : "0";
                       
                        bool Isexists = false;
                        foreach (DataRow drr in PurchaseInvoicedt.Rows)
                        {
                           
                            string OldOrderDetailsId = Convert.ToString(drr["PurchaseInvoiceDetailID"]);
                            if (OldOrderDetailsId == OrderDetails_Id)
                            {
                                Isexists = true;

                                drr["ProductID"] = ProductDetails;
                                drr["Description"] = Description;
                                drr["Quantity"] = Quantity;
                                drr["UOM"] = UOM;
                                drr["Warehouse"] = Warehouse;
                                drr["StockQuantity"] = StockQuantity;
                                drr["StockUOM"] = StockUOM;
                                drr["PurchasePrice"] = PurchasePrice;
                                drr["Discount"] = Discount;
                                drr["DiscountAmt"] = DiscountAmt;
                                drr["Amount"] = Amount;
                                drr["TaxAmount"] = TaxAmount;
                                drr["TotalAmount"] = TotalAmount;
                                drr["Status"] = "U";
                                if (!string.IsNullOrEmpty(taggingID))
                                { drr["ComponentID"] = taggingID; }                               
                                drr["ComponentNumber"] = taggingNo;
                                drr["ProductName"] = ProductName;
                                break;
                            }
                        }

                        if (Isexists == false)
                        {
                            PurchaseInvoicedt.Rows.Add(SrlNo, OrderDetails_Id, ProductDetails, Description, Quantity, UOM, Warehouse, StockQuantity, StockUOM,
                                PurchasePrice, Discount, DiscountAmt, Amount, TaxAmount, TotalAmount, "U", ProductName, taggingID, taggingNo, TotalQty, BalanceQty, IsComponentProduct);

                            if (IsComponentProduct == "Y")
                            {
                                if (hdfIsComp.Value == "C")
                                {
                                    DataTable ComponentTable = objPurchaseInvoice.GetLinkedProductList("LinkedProduct", ProductID);
                                    foreach (DataRow drr in ComponentTable.Rows)
                                    {
                                        string sProductsID = Convert.ToString(drr["sProductsID"]);
                                        string Products_Description = Convert.ToString(drr["Products_Description"]);
                                        string Sales_UOM_Name = Convert.ToString(drr["Sales_UOM_Name"]);
                                        string Conversion_Multiplier = Convert.ToString(drr["Conversion_Multiplier"]);
                                        string Stk_UOM_Name = Convert.ToString(drr["Stk_UOM_Name"]);
                                        string Product_SalePrice = Convert.ToString(drr["Product_SalePrice"]);
                                        string Products_Name = Convert.ToString(drr["Products_Name"]);
                                        string StkQty = Convert.ToString(Convert.ToDecimal(Quantity) * Convert.ToDecimal(Conversion_Multiplier));
                                        SrlNo = Convert.ToString(Convert.ToInt32(SrlNo) + 1);
                                        PurchaseInvoicedt.Rows.Add(SrlNo, "0", sProductsID, Products_Description, Quantity, Sales_UOM_Name, "", StkQty, Stk_UOM_Name, Product_SalePrice, "0.0", "0.0", "0.0", "0.0", "0.0", "I", Products_Name, taggingID, taggingNo, "0.0", "0.0", "C");
                                    }
                                    hdfIsComp.Value = "";
                                }
                            }
                        }
                    }
                }
            }
            #endregion Update Detail  of Batch Grid Section End

            #region Delete Detail  of Batch Grid Section Start
            foreach (var args in e.DeleteValues)
            {
                string OrderDetails_Id = Convert.ToString(args.Keys["PurchaseInvoiceDetailID"]);

                for (int i = PurchaseInvoicedt.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = PurchaseInvoicedt.Rows[i];
                    string delOrderDetailsId = Convert.ToString(dr["PurchaseInvoiceDetailID"]);

                    if (delOrderDetailsId == OrderDetails_Id)
                        dr.Delete();
                }
                PurchaseInvoicedt.AcceptChanges();

                if (OrderDetails_Id.Contains("~") != true)
                {
                    PurchaseInvoicedt.Rows.Add("0", OrderDetails_Id, "0", "", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "D", "0", "0", "0", "0", "0", "0");
                }
            }

            #endregion Delete Detail  of Batch Grid Section End

            ///////////////////////
            string strDeleteSrlNo = Convert.ToString(hdnDeleteSrlNo.Value);
            if (strDeleteSrlNo != "" && strDeleteSrlNo != "0")
            {
                if (hdinvetorttype.Value != "N")
                {
                    DeleteWarehouse(strDeleteSrlNo);
                    DeleteTaxDetails(strDeleteSrlNo);
                    grid.JSProperties["cpdelete"] = "Y";
                    hdnDeleteSrlNo.Value = "";
                }
                else
                {
                    DeleteNonInventoryDetail(strDeleteSrlNo);
                    DeleteTaxDetails(strDeleteSrlNo);
                    grid.JSProperties["cpdelete"] = "Y";
                    hdnDeleteSrlNo.Value = "";
                }
            }
            #region PurchaseInvoiceDetailID is keyfield It can not be duplicate by Sam due to Unknown Use and this affect clicking Delete Button on 10062017 Section Start
            int j = 1;
            foreach (DataRow dr in PurchaseInvoicedt.Rows)
            {
                string Status = Convert.ToString(dr["Status"]);
                dr["SrlNo"] = j.ToString();

                if (Status != "D")
                {
                    if (Status == "I")
                    {
                        string strID = Convert.ToString("Q~" + j);
                        dr["PurchaseInvoiceDetailID"] = strID;
                    }
                    j++;
                }
            }
            PurchaseInvoicedt.AcceptChanges();

            Session["TProductInvoiceDetails"] = PurchaseInvoicedt;
            //if (hdnRefreshType.Value == "E" && hdfIsDelete.Value == "I")
            //{
            //    foreach (DataRow dr in PurchaseInvoicedt.Rows)
            //    {
            //        dr["PurchaseInvoiceDetailID"] = "0";
            //    }
            //}
            //else if (hdnRefreshType.Value == "N" && hdfIsDelete.Value == "I")
            //{
            //    foreach (DataRow dr in PurchaseInvoicedt.Rows)
            //    {
            //        dr["PurchaseInvoiceDetailID"] = "0";
            //    }
            //}
            //PurchaseInvoicedt.AcceptChanges();
            //Session["TProductInvoiceDetails"] = PurchaseInvoicedt;
          
            #endregion PurchaseInvoiceDetailID is keyfield It can not be duplicate by Sam due to Unknown Use and this affect clicking Delete Button on 10062017 Section End

            if (IsDeleteFrom != "D" && IsCompFrom != "C")
            {

                #region Main Header Section Start

                string ActionType = Convert.ToString(ViewState["ActionType"]);
                string MainPurchaseInvoiceID = Convert.ToString(Session["TPurchaseInvoice_Id"]);
                string strSchemeType = Convert.ToString(ddl_numberingScheme.Value);
                string strPurchaseNumber = Convert.ToString(txtVoucherNo.Text);
                string strQuoteDate = Convert.ToString(dt_PLQuote.Date);


                #region Code added for Modification by Sam on 25052017

                string partyInvNo = Convert.ToString(txt_partyInvNo.Text);
                string partyInvDate = "";
                if (dt_partyInvDt.Date != DateTime.MinValue)
                {
                    partyInvDate = Convert.ToString(dt_partyInvDt.Date);
                }
                #endregion Code added for Modification by Sam on 25052017
                Boolean ReverseMechanism = false;
                if (chk_reversemechenism.Checked)
                {
                    ReverseMechanism = true;
                }
                else
                {
                    ReverseMechanism = false;
                }
               
                String IndentRequisitionNo = "";
                if (rdl_PurchaseInvoice.Items.Count > 1)
                {
                    if (rdl_PurchaseInvoice.Items[0].Selected)
                    {
                        InvoiceCreatedFromDoc = "PO";                       
                    }
                    else if (rdl_PurchaseInvoice.Items[1].Selected)
                    {
                        InvoiceCreatedFromDoc = "PC";                        
                    }
                }
                else
                {
                    InvoiceCreatedFromDoc = rdl_PurchaseInvoice.SelectedValue;
                }
                List<object> QuoList = lookup_quotation.GridView.GetSelectedFieldValues("ComponentID");
                foreach (object Quo in QuoList)
                {
                    IndentRequisitionNo += "," + Quo;
                }

                IndentRequisitionNo = IndentRequisitionNo.TrimStart(',');
                InvoiceCreatedFromDoc_Ids = IndentRequisitionNo;              
                string[] eachQuo = IndentRequisitionNo.Split(',');
                string IndentRequisitionDate = string.Empty;
                if (eachQuo.Length == 1)
                {
                    IndentRequisitionDate = dt_Quotation.Text;
                }
                else
                {
                    IndentRequisitionDate = "";
                }
             
                string strVendor = Convert.ToString(hdfLookupCustomer.Value);
                string strContactName = Convert.ToString(cmbContactPerson.Value);
                string Reference = Convert.ToString(txt_Refference.Text);
                string PosForGst = Convert.ToString(ddlPosGstTPurchase.Value);
                string strBranch = Convert.ToString(ddl_Branch.SelectedValue);
                string strAgents = "";               
                string strCurrency = Convert.ToString(ddl_Currency.SelectedValue);
                string strRate = Convert.ToString(txt_Rate.Value);
                string strTaxType = Convert.ToString(ddl_AmountAre.Value);
                string strTaxCode = Convert.ToString(ddl_VatGstCst.Value).Split('~')[0]; ;
                string CashBank = Convert.ToString(ddl_cashBank.Value);
                string CompanyID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string currency = Convert.ToString(HttpContext.Current.Session["LocalCurrency"]);
                string[] ActCurrency = currency.Split('~');
                int BaseCurrencyId = Convert.ToInt32(ActCurrency[0]);

                #endregion Main Header Section Start

                DataTable tempPurchaseInvoice = PurchaseInvoicedt.Copy();                

                #region During Saving and Update Arrange The Data According to Datatable By Sam on 10062017 Section Start

                foreach (DataRow dr in tempPurchaseInvoice.Rows)
                {
                    string Status = Convert.ToString(dr["Status"]);
                    if (Status == "I")
                    {
                        dr["PurchaseInvoiceDetailID"] = "0";

                        string[] list = Convert.ToString(dr["ProductID"]).Split(new string[] { "||@||" }, StringSplitOptions.None);
                        dr["ProductID"] = Convert.ToString(list[0]);
                        dr["UOM"] = Convert.ToString(list[3]);
                        dr["StockUOM"] = Convert.ToString(list[5]);
                    }
                    else if (Status == "U" || Status == "")
                    {
                        string[] list = Convert.ToString(dr["ProductID"]).Split(new string[] { "||@||" }, StringSplitOptions.None);
                        dr["ProductID"] = Convert.ToString(list[0]);
                        dr["UOM"] = Convert.ToString(list[3]);
                        dr["StockUOM"] = Convert.ToString(list[5]);
                        string OrderDetails_Id = Convert.ToString(dr["PurchaseInvoiceDetailID"]);
                        if (OrderDetails_Id.Contains("~") == true)
                        {
                            dr["PurchaseInvoiceDetailID"] = "0";
                        }
                    }
                }
                tempPurchaseInvoice.AcceptChanges();
                #endregion During Saving and Update Arrange The Data According to Datatable By Sam on 10062017 Section Start
              
                #region DataTable of Inline Tax

                DataTable TaxDetailTable = new DataTable();
                if (Session["TPurchaseOrderFinalTaxRecord"] != null)
                {
                    TaxDetailTable = (DataTable)Session["TPurchaseOrderFinalTaxRecord"];
                }

                #endregion Inline Tax End

                #region DataTable Of Purchase Order Tax Details

                DataTable POTaxDetailsdt = new DataTable();
                if (Session["TPBTaxDetails"] != null)
                {
                    POTaxDetailsdt = (DataTable)Session["TPBTaxDetails"];
                }
                else
                {
                    POTaxDetailsdt.Columns.Add("Taxes_ID", typeof(string));
                    POTaxDetailsdt.Columns.Add("Taxes_Name", typeof(string));
                    POTaxDetailsdt.Columns.Add("Percentage", typeof(string));
                    POTaxDetailsdt.Columns.Add("Amount", typeof(string));
                    POTaxDetailsdt.Columns.Add("AltTax_Code", typeof(string));
                }

                DataTable tempTaxDetailsdt = new DataTable();
                tempTaxDetailsdt = POTaxDetailsdt.DefaultView.ToTable(false, "Taxes_ID", "Percentage", "Amount", "AltTax_Code");

                tempTaxDetailsdt.Columns.Add("SlNo", typeof(string));
                //    tempTaxDetailsdt.Columns.Add("AltTaxCode", typeof(string));

                tempTaxDetailsdt.Columns["SlNo"].SetOrdinal(0);
                tempTaxDetailsdt.Columns["Taxes_ID"].SetOrdinal(1);
                tempTaxDetailsdt.Columns["AltTax_Code"].SetOrdinal(2);
                tempTaxDetailsdt.Columns["Percentage"].SetOrdinal(3);
                tempTaxDetailsdt.Columns["Amount"].SetOrdinal(4);

                foreach (DataRow d in tempTaxDetailsdt.Rows)
                {
                    d["SlNo"] = "0";
                    //d["AltTaxCode"] = "0";
                }

                #endregion DataTable Of Purchase Order Tax Details

                #region DataTable of Warehouse

                DataTable tempWarehousedt = new DataTable();
                //if (Session["POWarehouseData"] != null)
                //{
                //    DataTable Warehousedt = (DataTable)Session["POWarehouseData"];
                //    tempWarehousedt = Warehousedt.DefaultView.ToTable(false, "Product_SrlNo", "LoopID", "WarehouseID", "TotalQuantity", "BatchID", "SerialID");
                //}
                //else
                //{
                //    tempWarehousedt.Columns.Add("Product_SrlNo", typeof(string));
                //    tempWarehousedt.Columns.Add("SrlNo", typeof(string));
                //    tempWarehousedt.Columns.Add("WarehouseID", typeof(string));
                //    tempWarehousedt.Columns.Add("TotalQuantity", typeof(string));
                //    tempWarehousedt.Columns.Add("BatchID", typeof(string));
                //    tempWarehousedt.Columns.Add("SerialID", typeof(string));
                //}

                #endregion Warehouse End

                // DataTable Of Billing Address
                #region ##### Added By : Samrat Roy -- to get BillingShipping user control data
                DataTable tempBillAddress = new DataTable();
                tempBillAddress = Purchase_BillingShipping.GetBillingShippingTable();

                #region #### Old_Process ####
                //// DataTable Of Billing Address
                //DataTable tempBillAddress = new DataTable();
                //if (Session["TPBAddressDtl"] != null)
                //{
                //    tempBillAddress = (DataTable)Session["TPBAddressDtl"];
                //}
                //else
                //{
                //    tempBillAddress = StorePurchaseInvoiceAddressDetail();
                //}
                //// End

                #endregion

                #endregion

                string approveStatus = "";
                if (Request.QueryString["status"] != null)
                {
                    approveStatus = Convert.ToString(Request.QueryString["status"]);
                }

                string validate = "";
                if (ActionType == "Add")
                {
                    string[] SchemeList = strSchemeType.Split(new string[] { "~" }, StringSplitOptions.None);
                    validate = checkNMakeJVCode(strPurchaseNumber, Convert.ToInt32(SchemeList[0]));
                }
                else
                {
                    UniquePurchaseInvoice = txtVoucherNo.Text.Trim();
                }

                #region Code Add by Sam to Check Duplicate Product Id and Null Quantity Checking
                //int istagged=0;

                //if(rdl_PurchaseInvoice.Items.Count>1)
                //{ 
                //  if((rdl_PurchaseInvoice.Items[0].Selected) || (rdl_PurchaseInvoice.Items[1].Selected))
                //  {
                //      istagged=1;
                //  }
                //}
                //else if(rdl_PurchaseInvoice.Items.Count==1)
                //{
                //    if(rdl_PurchaseInvoice.Items[0].Selected)
                //    {
                //        istagged = 1;
                //    }
                //}
                //if (istagged == 0)
                //{
                var duplicateRecords = tempPurchaseInvoice.AsEnumerable()
               .GroupBy(r => r["ProductID"]) //coloumn name which has the duplicate values
               .Where(gr => gr.Count() > 1)
                .Select(g => g.Key);

                foreach (var d in duplicateRecords)
                {
                    validate = "duplicateProduct";
                }
                foreach (DataRow dr in tempPurchaseInvoice.Rows)
                {
                    if (Convert.ToString(ddlInventory.SelectedItem.Value) != "N")  // checking if Inventory Item is Y or N if Y then Check otherwise ignore this validation
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
                }
                //}

                #endregion Code Add by Sam to Check Duplicate Product Id and Null Quantity Checking


                //// ############# Added By : Samrat Roy -- 02/05/2017 -- To check Transporter Mandatory Control 
               // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

                DataTable DT = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='Transporter_TPBMandatory' AND IsActive=1");
                if (DT != null && DT.Rows.Count > 0)
                {
                    string IsMandatory = Convert.ToString(DT.Rows[0]["Variable_Value"]).Trim();                  
                    if (Convert.ToString(Session["TransporterVisibilty"]).Trim() == "Yes")
                    {
                        if (IsMandatory == "Yes")
                        {                          
                            if (hfControlData.Value.Trim() == "")
                            {
                                validate = "transporteMandatory";
                            }
                        }
                    }
                }
                #region Non Inventory Section By sam On 17052017 Section Start
                string inventorytype = "";
                inventorytype = ddlInventory.SelectedItem.Value;

                DataTable productcharges = new DataTable();
                if (Session["TPBNonInvProChgforShow"] != null)
                {
                    productcharges = (DataTable)Session["TPBNonInvProChgforShow"];
                    DataColumnCollection columns = productcharges.Columns;
                    if (columns.Contains("TDSTCSRates_Code"))
                    {
                        productcharges.Columns.Remove("TDSTCSRates_Code");
                        productcharges.AcceptChanges();
                    }                   
                }

                #endregion Non Inventory Section By sam On 17052017 Section End             

              
                //Details:To check T&C Mandatory Control
                #region TC
                if (Convert.ToString(ddl_AmountAre.Value) != "4")
                {
                DataTable DT_TC = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='Show_TC_TPB' AND IsActive=1");
                if (DT_TC != null && DT_TC.Rows.Count > 0)
                {
                    string IsMandatory = Convert.ToString(DT_TC.Rows[0]["Variable_Value"]).Trim();

                  //  objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                    objEngine = new BusinessLogicLayer.DBEngine();

                    DataTable DTVisible = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='TC_TPBMandatory' AND IsActive=1");
                    if (Convert.ToString(DTVisible.Rows[0]["Variable_Value"]).Trim() == "Yes")
                    {
                        if (IsMandatory == "Yes")
                        {
                            if (TermsConditionsControl.GetControlValue("dtDeliveryDate") == "" || TermsConditionsControl.GetControlValue("dtDeliveryDate") == "@")
                            {
                                validate = "TCMandatory";
                            }
                        }
                    }
                }
                }
                #endregion
                //----------End-------------------------
                #region ##### Added By : Samrat Roy -- to get BillingShipping user control data                
               
                if (tempBillAddress.Rows.Count != 2)
                {
                    validate = "AddressProblem";
                }
                #endregion
                #region Checking of Registered Billing/Shipping of Vendor if not exist will not allow to save

                int cnt = objPurchaseInvoice.CheckBillingShippingofVendor(Convert.ToString(hdnCustomerId.Value));
                if (cnt != 1)
                {
                    validate = "VendorAddressProblem";
                }

                #endregion Checking of Registered Billing/Shipping of Vendor if not exist will not allow to save

                //Add TDS section Tanmoy
                object sumObject;
                //sumObject = _tempQuotation.Compute("Sum(Amount)", string.Empty);
                sumObject = tempPurchaseInvoice.AsEnumerable()
                    .Sum(x => Convert.ToDecimal(x["Amount"]));
                object TotalsumObject;
                //TotalsumObject = _tempQuotation.Compute("Sum(TotalAmount)", string.Empty);
                TotalsumObject = tempPurchaseInvoice.AsEnumerable()
    .Sum(x => Convert.ToDecimal(x["TotalAmount"]));
                string MainInvoiceID = Convert.ToString(Session["PurchaseInvoice_Id"]);
                DataTable dt_TDS = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_TDSDetails_Calc");
                proc.AddVarcharPara("@VendorID", 500, hdnCustomerId.Value);
                proc.AddVarcharPara("@invoice_id", 500, MainInvoiceID);
                proc.AddVarcharPara("@Action", 500, "ShowTDSDetails");
                proc.AddVarcharPara("@date", 500, dt_PLQuote.Text);
                proc.AddVarcharPara("@totalAmount", 500, Convert.ToString(TotalsumObject));
                proc.AddVarcharPara("@taxableAmount", 500, Convert.ToString(sumObject));
                proc.AddVarcharPara("@branch_id", 500, strBranch);
                proc.AddVarcharPara("@tds_code", 500, txtTDSSection.Value.ToString());

                dt_TDS = proc.GetTable();

                if (dt_TDS != null && dt_TDS.Rows.Count > 0 && ddlInventory.SelectedValue == "Y")
                {
                    if (MainInvoiceID == "" && Convert.ToDecimal(dt_TDS.Rows[0]["Amount"]) > 0  && Convert.ToDecimal(txtTDSAmount.Text) == 0)
                    {
                        validate = "TDSMandatory";
                    }
                }

                //End of Rev TDS Section Tanmoy

                if (validate == "AddressProblem" || validate == "outrange" || validate == "nullQuantity" || validate == "duplicateProduct" || validate == "duplicate" || validate == "transporteMandatory" || validate == "TCMandatory" || validate == "VendorAddressProblem"
                     || validate == "TDSMandatory")
                {
                    grid.JSProperties["cpSaveSuccessOrFail"] = validate;
                    //if (Session["TProductInvoiceDetails"] != null)
                    //{
                    //    PurchaseInvoicedt = (DataTable)Session["TProductInvoiceDetails"];
                    //    if (PurchaseInvoicedt.Rows.Count > 0)
                    //    {
                    //        foreach (DataRow dr in PurchaseInvoicedt.Rows)
                    //        {
                    //            string Status = Convert.ToString(dr["Status"]);
                    //            dr["SrlNo"] = j.ToString();

                    //            if (Status != "D")
                    //            {
                    //                if (Status == "I")
                    //                {
                    //                    string strID = Convert.ToString("Q~" + j);
                    //                    dr["PurchaseInvoiceDetailID"] = strID;
                                       
                    //                }
                    //                j++;
                    //            }
                    //        }
                    //    }
                    //}
                    //PurchaseInvoicedt.AcceptChanges();
                    //DataView dvData = new DataView(PurchaseInvoicedt);
                    //dvData.RowFilter = "Status <> 'D'";

                    //grid.DataSource = GetPurchaseInvoice(dvData.ToTable());
                    //grid.DataBind();
                }
                else
                {
                    string incluexclutype = "";
                    string incluexcluitype = Convert.ToString(ddl_AmountAre.Value);
                    if (incluexcluitype == "1")
                    {
                        incluexclutype = "E";
                    }
                    else if (incluexcluitype == "2")
                    {
                        incluexclutype = "I";
                    }                 

                    DataTable ReverseTaxtable = CreateReverseTaxTable();
                    if (!chk_reversemechenism.Checked)
                    {
                        string ShippingState = "";
                       
                        if (ddlPosGstTPurchase.Value.ToString() == "S")
                        {
                            ShippingState = Purchase_BillingShipping.GeteShippingStateCode();
                        }
                        else if (ddlPosGstTPurchase.Value.ToString() == "B")
                        {
                            ShippingState = Purchase_BillingShipping.GetBillingStateCode();
                        }
                        //TaxDetailTable = gstTaxDetails.SetTaxTableDataWithProductSerialForPurchaseRoundOff(ref tempPurchaseInvoice, "SrlNo", "ProductID", "Amount", "TaxAmount", "TotalAmount", TaxDetailTable, "P", dt_PLQuote.Date.ToString("yyyy-MM-dd"), strBranch, ShippingState, incluexclutype, Convert.ToString(hdnCustomerId.Value));
                        TaxDetailTable = gstTaxDetails.SetTaxTableDataWithProductSerialForPurchaseRoundOffWithException(ref tempPurchaseInvoice, "SrlNo", "ProductID", "Amount", "TaxAmount", "TotalAmount", TaxDetailTable, "P", dt_PLQuote.Date.ToString("yyyy-MM-dd"), strBranch, ShippingState, incluexclutype, Convert.ToString(hdnCustomerId.Value), "Quantity", "PO");

                        string shippingStateCode = "";
                        ProcedureExecute procstateTable = new ProcedureExecute("Prc_taxForpurchase");
                        procstateTable.AddVarcharPara("@action", 500, "GetGSTINByBranch");
                        procstateTable.AddIntegerPara("@BranchId", Convert.ToInt32(strBranch));
                        procstateTable.AddVarcharPara("@companyintId", 50, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
                        procstateTable.AddVarcharPara("@vendInternalId", 20, Convert.ToString(hdnCustomerId.Value));
                        DataSet taxForpurchase = procstateTable.GetDataSet();

                        //  DataTable fetchedData = oDBEngine.GetDataTable("select StateCode  from tbl_master_branch br inner join tbl_master_state st on br.branch_state=st.id where branch_id=" + BranchId);
                        if (taxForpurchase != null)
                        {

                            shippingStateCode = Convert.ToString(taxForpurchase.Tables[1].Rows[0][0]).Trim();
                            if (shippingStateCode.Trim() != "")
                            {
                                shippingStateCode = shippingStateCode.Substring(0, 2);
                            }
                        }
                        

                        //DataTable dtTaxDetails = new DataTable();
                        //ProcedureExecute procT = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
                        //procT.AddVarcharPara("@Action", 500, "GetTaxDetailsByProductID");
                        //procT.AddPara("@PurchaseInvoiceDetails", tempPurchaseInvoice);
                        //procT.AddVarcharPara("@TaxOption", 10, Convert.ToString(strTaxType));
                        //procT.AddVarcharPara("@SupplyState", 15, Convert.ToString(shippingStateCode));
                        //procT.AddVarcharPara("@branchId", 10, Convert.ToString(strBranch));
                        //procT.AddVarcharPara("@CompanyId", 500, Convert.ToString(Session["LastCompany"]));
                        //procT.AddVarcharPara("@ENTITY_ID", 100, Convert.ToString(hdnCustomerId.Value));
                        //procT.AddVarcharPara("@TaxDATE", 100, Convert.ToString(dt_PLQuote.Date.ToString("yyyy-MM-dd")));
                        //dtTaxDetails = procT.GetTable();

                        //if (dtTaxDetails != null && dtTaxDetails.Rows.Count > 0)
                        //{

                        //    foreach (DataRow dr in dtTaxDetails.Rows)
                        //    {
                        //        string SerialID = Convert.ToString(dr["SrlNo"]);
                        //        string TaxID = Convert.ToString(dr["TaxCode"]);
                        //        decimal _TaxAmount = Math.Round(Convert.ToDecimal(dr["TaxAmount"]), 2);
                        //        string ProductName = Convert.ToString(dr["ProductName"]);
                        //        DataRow[] rows = TaxDetailTable.Select("SlNo = '" + SerialID + "' and TaxCode='" + TaxID + "'");

                        //        if (rows != null && rows.Length > 0)
                        //        {                                    
                        //            decimal EntryTaxAmount = Math.Round(Convert.ToDecimal(rows[0]["Amount"]), 2);

                        //            if (EntryTaxAmount != _TaxAmount)
                        //            {
                        //                validate = "checkAcurateTaxAmount";
                        //                grid.JSProperties["cpSerialNo"] = SerialID;
                        //                grid.JSProperties["cpProductName"] = ProductName;
                        //                break;
                        //            }


                        //        }

                        //    }
                        //}
                    }
                    else
                    {
                        string RShippingState = "";
                        if (ddlPosGstTPurchase.Value.ToString() == "S")
                        {
                            RShippingState = Purchase_BillingShipping.GeteShippingStateCode();
                        }
                        else if (ddlPosGstTPurchase.Value.ToString() == "B")
                        {
                            RShippingState = Purchase_BillingShipping.GetBillingStateCode();
                        }
                        ReverseTaxtable = gstTaxDetails.GetReverseTaxTable(tempPurchaseInvoice, "SrlNo", "ProductID", "Amount", "TaxAmount", ReverseTaxtable, "P", dt_PLQuote.Date.ToString("yyyy-MM-dd"), strBranch, RShippingState, incluexclutype, Convert.ToString(hdnCustomerId.Value));
                    }
                    Int64 Proj_Id = 0;
                    if (lookup_Project.Text != "")
                    {
                        Proj_Id = Convert.ToInt32(lookup_Project.Value);
                    }
                    int strIsComplete = 0, strInvoiceID = 0;


                    //////////////////  Add TDS Tanmoy  /////////////////////////
                    string strTDScode = Convert.ToString(txtTDSSection.Value);
                    string strTDSappl = Convert.ToString(txtTDSapplAmount.Text);
                    string strTDSpercentage = Convert.ToString(txtTDSpercentage.Text);
                    string strTDSamout = Convert.ToString(txtTDSAmount.Text);
                    //////////////////////////////////////////////////////////

                    if (ModifyQuatation(MainPurchaseInvoiceID, strSchemeType, UniquePurchaseInvoice, strQuoteDate, IndentRequisitionDate,
                        strVendor, strContactName, Reference, PosForGst, strBranch, strCurrency, strRate, strTaxType, strTaxCode, tempPurchaseInvoice,
                        TaxDetailTable, tempWarehousedt, tempTaxDetailsdt, tempBillAddress, approveStatus, ActionType, InvoiceCreatedFromDoc,
                        InvoiceCreatedFromDoc_Ids, ref strIsComplete, ref strInvoiceID, CashBank, ReverseMechanism,
                        inventorytype, productcharges, partyInvNo, partyInvDate, ReverseTaxtable, Proj_Id
                        , strTDScode, strTDSappl, strTDSpercentage, strTDSamout) == true)
                    {                       
                        grid.JSProperties["cpPurchaseOrderNo"] = UniquePurchaseInvoice;
                        #region To Show By Default Cursor after SAVE AND NEW
                        if (ActionType == "Add") // session has been removed from quotation list page working good
                        {
                            //string[] schemaid = new string[] { };
                            string schemavalue = Convert.ToString(ddl_numberingScheme.Value);
                            Session["schemavalueTPB"] = schemavalue;        // session has been removed from quotation list page working good
                            //schemaid = ddl_numberingScheme.SelectedValue.Split('~');

                            //string schematype = schemaid[1];
                            string schematype = strPurchaseNumber.Trim();
                            if (schematype == "Auto")
                            {
                                Session["SaveModeTPB"] = "A";
                            }
                            else
                            {
                                Session["SaveModeTPB"] = "M";
                            }
                        }

                        #endregion
                        if (strIsComplete == 1)
                        {
                            if (approveStatus != "")
                            {
                                if (approveStatus == "2")
                                {
                                    grid.JSProperties["cpApproverStatus"] = "approve";
                                }
                                else
                                {
                                    grid.JSProperties["cpApproverStatus"] = "rejected";
                                }
                            }
                            Employee_BL objemployeebal = new Employee_BL();
                            int MailStatus = 0;
                            string AssignedTo_Email = string.Empty;
                            string ReceiverEmail = string.Empty;
                            decimal Level1_User = Convert.ToDecimal(Session["userid"]);
                            decimal Level2User = Convert.ToDecimal(Session["userid"]);
                            decimal Level3User = Convert.ToDecimal(Session["userid"]);
                            bool L1 = false;
                            bool L2 = false;
                            bool L3 = false;
                            string ActivityName = string.Empty;

                            DataTable dtbl_AssignedTo = new DataTable();
                            DataTable dtbl_AssignedBy = new DataTable();
                            DataTable dtEmail_To = new DataTable();
                            DataTable dt_EmailConfig = new DataTable();
                            DataTable dt_ActivityName = new DataTable();
                            DataTable Dt_LevelUser = new DataTable();

                            dt_EmailConfig = objemployeebal.GetEmailAccountConfigDetails("", 1); // Checked fetch datatatable of email setup with action 1 param
                            Dt_LevelUser = objemployeebal.GetAllLevelUsers("1", 12);

                            ActivityName = UniquePurchaseInvoice;

                            if (Dt_LevelUser != null && string.IsNullOrEmpty(approveStatus))
                            {
                                L1 = true;
                                dtEmail_To = objemployeebal.GetEmailLevelUsersWise(1, 11, 1);
                            }
                            else if (Dt_LevelUser != null && Dt_LevelUser.Rows[0].Field<decimal>("Level1_user_id") == Level1_User && approveStatus == "2")
                            {
                                L2 = true;
                                dtEmail_To = objemployeebal.GetEmailLevelUsersWise(1, 11, 2);
                            }
                            else if (Dt_LevelUser != null && Dt_LevelUser.Rows[0].Field<decimal>("Level2_user_id") == Level2User && approveStatus == "2")
                            {
                                L3 = true;
                                dtEmail_To = objemployeebal.GetEmailLevelUsersWise(1, 11, 3);
                            }

                            if (dtEmail_To != null && dtEmail_To.Rows.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(dtEmail_To.Rows[0].Field<string>("Email_Id")))
                                {
                                    ReceiverEmail = Convert.ToString(dtEmail_To.Rows[0].Field<string>("Email_Id"));
                                }
                                else
                                {
                                    ReceiverEmail = "";
                                }
                            }
                            ListDictionary replacements = new ListDictionary();
                            if (dtEmail_To.Rows.Count > 0)
                            {
                                replacements.Add("<%Approver%>", Convert.ToString(dtEmail_To.Rows[0].Field<string>("Approver")));
                            }
                            else
                            {
                                replacements.Add("<%Approver%>", "");
                            }
                            replacements.Add("<%PurchaseInvoiceNo%>", UniquePurchaseInvoice);                         

                            if (!string.IsNullOrEmpty(ReceiverEmail))
                            {                               
                                MailStatus = ExceptionLogging.SendEmailToAssigneeByUser(ReceiverEmail, "", replacements, dt_EmailConfig, ActivityName, 12);
                            }
                        }
                        else if (strIsComplete == 2)
                        {
                            grid.JSProperties["cpSaveSuccessOrFail"] = "quantityTagged";
                        }
                        else if (strIsComplete == -12)
                        {
                            grid.JSProperties["cpSaveSuccessOrFail"] = "EmptyProject";
                        }
                        else
                        {
                            grid.JSProperties["cpSaveSuccessOrFail"] = "errorInsert";
                        }
                    }
                    //Udf Add mode
                    DataTable udfTable = (DataTable)Session["UdfDataOnAdd"];
                    if (udfTable != null)
                        Session["UdfDataOnAdd"] = reCat.insertRemarksCategoryAddMode("TPB", "TransitPurchaseInvoice" + Convert.ToString(strInvoiceID), udfTable, Convert.ToString(Session["userid"]));
                    if (Session["TProductInvoiceDetails"] != null && strIsComplete==1)
                    {
                        Session["TProductInvoiceDetails"] = null;
                    }                   
                }
            }
            else
            {
                DataView dvData = new DataView(PurchaseInvoicedt);
                dvData.RowFilter = "Status <> 'D'";
                grid.DataSource = GetPurchaseInvoice(dvData.ToTable());
                grid.DataBind();
            }
        }

        public bool ModifyQuatation(string PurchaseInvoiceID, string strSchemeType, string strQuoteNo, string strQuoteDate, string strQuoteExpiry, string strCustomer, string strContactName,
                                    string Reference, string PosForGst, string strBranch, string strCurrency, string strRate, string strTaxType, string strTaxCode, DataTable PurchaseInvoicedt,
                                    DataTable TaxDetailTable, DataTable Warehousedt, DataTable PurchaseInvoiceTaxdt, DataTable BillAddressdt,
                                   string approveStatus, string ActionType, string InvoiceCreatedFromDoc, string InvoiceCreatedFromDoc_Ids, ref int strIsComplete,
                                   ref int strInvoiceID, string CashBank, Boolean ReverseMechanism, string inventorytype, DataTable NonInventoryDtl,
            string partyInvNo, string partyInvDate, DataTable ReverseTaxtable, Int64 Proj_Id
             , string strTDScode, string strTDSappl, string strTDSpercentage, string strTDSamout)
        {
            try
            {
                DataSet dsInst = new DataSet();
               // SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                SqlCommand cmd = new SqlCommand("prc_TransitPurchaseInvoice", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", ActionType);
                cmd.Parameters.AddWithValue("@PinvoiceID", PurchaseInvoiceID);
                cmd.Parameters.AddWithValue("@InvoiceType", "T");
                cmd.Parameters.AddWithValue("@PinvoiceNo", strQuoteNo);
                cmd.Parameters.AddWithValue("@PinvoiceDate", strQuoteDate);
                cmd.Parameters.AddWithValue("@partyInvNo", partyInvNo);
                if (partyInvDate != null && partyInvDate != "")
                {
                    if (Convert.ToDateTime(partyInvDate) != DateTime.MinValue)
                    {
                        cmd.Parameters.AddWithValue("@partyInvDate", partyInvDate);
                    }
                }
                if (strQuoteExpiry != "")
                {
                    string cdate = strQuoteExpiry.Substring(3, 2) + "-" + strQuoteExpiry.Substring(0, 2) + "-" + strQuoteExpiry.Substring(6, 4);
                    DateTime d2;
                    //if (!DateTime.TryParse(strQuoteExpiry, out myDate))
                    //{
                    //    // handle parse failure
                    //}
                    d2 = DateTime.ParseExact(cdate, "MM-dd-yyyy", null);
                    //string dtchallan = Convert.ToDateTime(strQuoteExpiry).ToString("dd-MM-yyyy");
                    cmd.Parameters.AddWithValue("@ChallanOrderDate", d2);
                }
                cmd.Parameters.AddWithValue("@CustomerID", strCustomer);
                cmd.Parameters.AddWithValue("@ReverseMechanism", ReverseMechanism);

                cmd.Parameters.AddWithValue("@ContactPerson", strContactName);
                cmd.Parameters.AddWithValue("@Reference", Reference);
                cmd.Parameters.AddWithValue("@PosForGst", PosForGst);
                cmd.Parameters.AddWithValue("@BranchID", strBranch);
                //cmd.Parameters.AddWithValue("@Agents", strAgents);
                cmd.Parameters.AddWithValue("@Currency", strCurrency);
                cmd.Parameters.AddWithValue("@Rate", strRate);
                //cmd.Parameters.AddWithValue("@Rate", 1);
                cmd.Parameters.AddWithValue("@TaxType", strTaxType);
                if (strTaxCode == "")
                {
                    strTaxCode = "0";
                }
                cmd.Parameters.AddWithValue("@TaxCode", strTaxCode);
                cmd.Parameters.AddWithValue("@approveStatus", approveStatus);

                cmd.Parameters.AddWithValue("@CompanyID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FinYear", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(Session["userid"]));

                cmd.Parameters.AddWithValue("@PurchaseInvoiceDetails", PurchaseInvoicedt);
                
                if (Convert.ToString(ddl_AmountAre.Value) == "4")
                {
                    if (TaxDetailTable.Rows.Count > 0)
                    {

                    }
                    else
                    {
                        TaxDetailTable.Rows.Add(1, 0, 0, 0, 0);
                    }
                }
                cmd.Parameters.AddWithValue("@PBProductTaxDetails", TaxDetailTable);
                cmd.Parameters.AddWithValue("@PBunGSTTaxDetails", ReverseTaxtable);
                cmd.Parameters.AddWithValue("@Project_Id", Proj_Id);
                //if (Session["TPBwarehousedetailstempNew"] == null)


                //{
                //if (Session["TPBwarehousedetailstemp"] != null)
                //{
                //    DataTable temtable = new DataTable();
                //    DataTable Warehousedtssss = (DataTable)Session["TPBwarehousedetailstemp"];

                //    temtable = Warehousedtssss.DefaultView.ToTable(false, "SrlNo", "pcslno", "BatchWarehouseID", "BatchWarehousedetailsID", "BatchID", "SerialID", "WarehouseID", "WarehouseName", "BatchNo", "SerialNo", "MFGDate", "ExpiryDate", "Quantitysum", "productid", "Inventrytype", "StockID", "isnew");
                //    cmd.Parameters.AddWithValue("@WarehouseDetail", temtable);
                //    cmd.Parameters.AddWithValue("@isUpdated", "Y");  // to delete detail table to serial table and reinsert data

                //}
                ////}
                //else if (Session["TPBwarehousedetailstempNew"] != null)
                //{
                //    DataTable temtable = new DataTable();
                //    DataTable Warehousedtssss = (DataTable)Session["TPBwarehousedetailstempNew"];

                //    temtable = Warehousedtssss.DefaultView.ToTable(false, "SrlNo", "pcslno", "BatchWarehouseID", "BatchWarehousedetailsID", "BatchID", "SerialID", "WarehouseID", "WarehouseName", "BatchNo", "SerialNo", "MFGDate", "ExpiryDate", "Quantitysum", "productid", "Inventrytype", "StockID", "isnew");
                //    cmd.Parameters.AddWithValue("@WarehouseDetail", temtable);
                //}

                #region Non Inventory Section By Sam on !7052017
                if (NonInventoryDtl.Columns.Count > 0)
                {

                }
                else
                {
                    NonInventoryDtl = NonInventoryProductChgDtl();
                }


                cmd.Parameters.AddWithValue("@InventoryType", inventorytype);
                cmd.Parameters.AddWithValue("@NonInvProductChargesDetail", NonInventoryDtl);

                #endregion Non Inventory Section By Sam on !7052017
                //cmd.Parameters.AddWithValue("@WarehouseDetail", Warehousedt);
                cmd.Parameters.AddWithValue("@PurchaseInvoiceTax", PurchaseInvoiceTaxdt);
                cmd.Parameters.AddWithValue("@BillAddress", BillAddressdt);


                cmd.Parameters.AddWithValue("@InvoiceCreatedFromDoc", InvoiceCreatedFromDoc);
                cmd.Parameters.AddWithValue("@InvoiceCreatedFromDoc_Ids", InvoiceCreatedFromDoc_Ids);
                cmd.Parameters.AddWithValue("@CashBank", CashBank);


                //////////////Add TDS Tanmoy/////////////////////////////
                cmd.Parameters.AddWithValue("@TDScode", strTDScode);
                cmd.Parameters.AddWithValue("@TDSappAmount", strTDSappl);
                cmd.Parameters.AddWithValue("@TDSpercentage", strTDSpercentage);
                cmd.Parameters.AddWithValue("@TDSamount", strTDSamout);
                /////////////////////////////////////////////////////

                cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
                cmd.Parameters.Add("@ReturnPurchaseInvoiceID", SqlDbType.VarChar, 50);
                cmd.Parameters.Add("@ReturnLedgerAmt", SqlDbType.VarChar, 50);

                cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
                cmd.Parameters["@ReturnPurchaseInvoiceID"].Direction = ParameterDirection.Output;
                cmd.Parameters["@ReturnLedgerAmt"].Direction = ParameterDirection.Output;
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                string ReturnLedgerAmt = "";
                strIsComplete = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());
                if (strIsComplete == -20)
                {
                    grid.JSProperties["cpRVMechMainAc"] = strIsComplete;
                }
                else if (strIsComplete == -3)
                {
                    ReturnLedgerAmt = Convert.ToString(cmd.Parameters["@ReturnLedgerAmt"].Value);
                    grid.JSProperties["cpReturnLedgerAmt"] = strIsComplete;
                    string[] ReturnLedgerAmtList = new string[] { };
                    ReturnLedgerAmtList = ReturnLedgerAmt.Split('~');
                    grid.JSProperties["cpDRAmt"] = Convert.ToString(ReturnLedgerAmtList[0]);
                    grid.JSProperties["cpCRAmt"] = Convert.ToString(ReturnLedgerAmtList[1]);

                }
                strInvoiceID = Convert.ToInt32(cmd.Parameters["@ReturnPurchaseInvoiceID"].Value.ToString());
                grid.JSProperties["cpGeneratedInvoice"] = strInvoiceID;

                if (Convert.ToInt32(strInvoiceID) > 0)
                {
                    //####### Coded By Sayan Dutta For Custom Control Data Process #########
                    if (!string.IsNullOrEmpty(hfTermsConditionData.Value))
                    {
                        TermsConditionsControl.SaveTC(hfTermsConditionData.Value, Convert.ToString(strInvoiceID), "TPB");
                    }
                }

                if (Convert.ToInt32(strInvoiceID) > 0)
                {
                    //####### Coded By Samrat Roy For Custom Control Data Process #########
                    if (!string.IsNullOrEmpty(hfControlData.Value))
                    {
                        CommonBL objCommonBL = new CommonBL();
                        objCommonBL.InsertTransporterControlDetails(strInvoiceID, "TPB", hfControlData.Value, Convert.ToString(HttpContext.Current.Session["userid"]));
                    }
                }

                cmd.Dispose();
                con.Dispose();

                //DataTable udfTable = (DataTable)Session["UdfDataOnAdd"];
                //if (udfTable != null)
                //    Session["UdfDataOnAdd"] = reCat.insertRemarksCategoryAddMode("PO", "PurchaseOrder" + Convert.ToString(output.Value), udfTable, Convert.ToString(Session["userid"]));

                int retval = 0;
                if (!string.IsNullOrEmpty(Convert.ToString(strInvoiceID)))
                {
                    retval = Sendmail_PurchaseInvoice(strInvoiceID.ToString());
                }



                #region warehouse Update and delete

                updatewarehouse();
                deleteALL();

                #endregion

                return true;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private bool IsPOExist(string pcid)
        {
            bool IsExist = false;
            if (pcid != "" && Convert.ToString(pcid).Trim() != "")
            {
                DataTable dt = new DataTable();

                var elements = pcid.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
                foreach (string items in elements)
                {
                    dt = oDBEngine.GetDataTable("select Count(*) as isexist from tbl_trans_Indent where Indent_Id=" + items + "");
                    if (dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0]["isexist"]) != "0")
                    {
                        IsExist = true;
                    }
                }

            }
            else
            {
                IsExist = true;
            }

            return IsExist;
        }


        #endregion

        #region warehousepopup
        protected void GrdWarehousePC_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];
            GrdWarehousePC.JSProperties["cperrorMsg"] = null;

            if (hdniswarehouse.Value == "true")
            {
                GrdWarehousePC.Columns["viewWarehouseName"].Visible = true;
                GrdWarehousePC.Columns["Quantity"].Visible = true;
                GrdWarehousePC.Columns["viewQuantity"].Visible = false;

            }
            else
            {
                GrdWarehousePC.Columns["viewWarehouseName"].Visible = false;
                GrdWarehousePC.Columns["Quantity"].Visible = false;
                GrdWarehousePC.Columns["viewQuantity"].Visible = true;
            }
            if (hdnisbatch.Value == "true")
            {
                GrdWarehousePC.Columns["viewBatchNo"].Visible = true;
                GrdWarehousePC.Columns["viewMFGDate"].Visible = true;
                GrdWarehousePC.Columns["viewExpiryDate"].Visible = true;
                GrdWarehousePC.Columns["Quantity"].Visible = false;
                GrdWarehousePC.Columns["viewQuantity"].Visible = true;

            }
            else
            {
                GrdWarehousePC.Columns["viewBatchNo"].Visible = false;
                GrdWarehousePC.Columns["viewMFGDate"].Visible = false;
                GrdWarehousePC.Columns["viewExpiryDate"].Visible = false;
                GrdWarehousePC.Columns["Quantity"].Visible = true;
                GrdWarehousePC.Columns["viewQuantity"].Visible = false;
            }
            if (hdnisserial.Value == "true")
            {
                GrdWarehousePC.Columns["viewSerialNo"].Visible = true;
                GrdWarehousePC.Columns["Quantity"].Visible = false;
                GrdWarehousePC.Columns["viewQuantity"].Visible = true;
            }
            else
            {
                GrdWarehousePC.Columns["viewSerialNo"].Visible = false;
            }


            #region Savenew
            if (strSplitCommand == "Display")
            {

                string WarehouseName = Convert.ToString(e.Parameters.Split('~')[2]);

                string WarehouseID = Convert.ToString(e.Parameters.Split('~')[1]).Trim();

                string BatchName = Convert.ToString(e.Parameters.Split('~')[3]);

                string SerialName = Convert.ToString(e.Parameters.Split('~')[4]);
                string ProductID = Convert.ToString(hdfProductIDPC.Value);
                string stockid = Convert.ToString(hdfstockidPC.Value);

                // Code Commented And Added by Sam on 26052017 due to unavailable value of txtqnty.Text start for only serial type product
                decimal openingstock = 0;
                if (hdfProductType.Value == "S")
                {
                    if (hdnProductQuantity.Value != "" && hdnProductQuantity.Value != null)
                    {
                        openingstock = Convert.ToDecimal("1");
                    }
                }
                else
                {
                    openingstock = Convert.ToDecimal(txtqnty.Text);
                }
                //openingstock = Convert.ToDecimal(txtqnty.Text);
                // Code Commented And Added by Sam on 26052017 due to unavailable value of txtqnty.Text End

                string branchid = Convert.ToString(hdbranchIDPC.Value);
                string qnty = Convert.ToString(e.Parameters.Split('~')[5]);
                string productsrlno = Convert.ToString(e.Parameters.Split('~')[6]);

                decimal totalopeining = Convert.ToDecimal(hdfopeningstockPC.Value);
                decimal oldtotalopeining = Convert.ToDecimal(oldopeningqntity.Value);
                // Code Commented And Added   by Sam on 26052017 due to unavailable value of txtqnty.Text Start
                if (hdfProductType.Value != "S")
                {
                    if (qnty == "0.0000" && openingstock <= 0)
                    {
                        qnty = batchqnty.Text;
                        openingstock = Convert.ToDecimal(qnty);
                    }
                }
                //if (qnty == "0.0000" && openingstock <= 0)
                //{
                //    qnty = batchqnty.Text;
                //    openingstock = Convert.ToDecimal(qnty);
                //}
                // Code Commented And Added   by Sam on 26052017 due to unavailable value of txtqnty.Text End
                if (!string.IsNullOrEmpty(BatchName))
                {
                    BatchName = BatchName.Trim();
                }
                if (!string.IsNullOrEmpty(SerialName))
                {
                    SerialName = SerialName.Trim();
                }
                if (WarehouseID == "null")
                {
                    WarehouseID = "0";
                }

                bool isserialunique = false;
                if (hdnisserial.Value == "true")
                {
                    isserialunique = CheckUniqueSerial(SerialName, "new");
                }
                if (Convert.ToDecimal(openingstock) > totalopeining)
                {
                    GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("You have entered Quantity greater than Opening Quantity. Cannot Proceed.");

                }
                else if (isserialunique == false)
                {
                    DataTable Warehousedt = new DataTable();
                    DataTable Warehousedtnew = new DataTable();
                    if (Session["TPOCWarehouseData"] != null)
                    {
                        Warehousedt = (DataTable)Session["TPOCWarehouseData"];
                    }
                    else
                    {
                        Warehousedt.Columns.Add("SrlNo", typeof(Int32));
                        //Warehousedt.Columns.Add("productSrlNo", typeof(Int32));
                        Warehousedt.Columns.Add("WarehouseID", typeof(string));
                        Warehousedt.Columns.Add("WarehouseName", typeof(string));

                        Warehousedt.Columns.Add("BatchNo", typeof(string));
                        Warehousedt.Columns.Add("SerialNo", typeof(string));

                        Warehousedt.Columns.Add("MFGDate", typeof(DateTime));
                        Warehousedt.Columns.Add("ExpiryDate", typeof(DateTime));
                        Warehousedt.Columns.Add("Quantity", typeof(decimal));

                        Warehousedt.Columns.Add("BatchWarehouseID", typeof(string));
                        Warehousedt.Columns.Add("BatchWarehousedetailsID", typeof(string));
                        Warehousedt.Columns.Add("BatchID", typeof(string));
                        Warehousedt.Columns.Add("SerialID", typeof(string));


                        Warehousedt.Columns.Add("viewWarehouseName", typeof(string));

                        Warehousedt.Columns.Add("viewBatchNo", typeof(string));
                        Warehousedt.Columns.Add("viewQuantity", typeof(string));
                        Warehousedt.Columns.Add("viewSerialNo", typeof(string));



                        Warehousedt.Columns.Add("viewMFGDate", typeof(DateTime));
                        Warehousedt.Columns.Add("viewExpiryDate", typeof(DateTime));

                        Warehousedt.Columns.Add("Quantitysum", typeof(decimal));
                        Warehousedt.Columns.Add("isnew", typeof(string));
                        Warehousedt.Columns.Add("productid", typeof(string));
                        Warehousedt.Columns.Add("Inventrytype", typeof(string));
                        Warehousedt.Columns.Add("StockID", typeof(string));
                        Warehousedt.Columns.Add("pcslno", typeof(Int32));

                    }



                    DateTime dtmgh = txtmkgdate.Date;
                    DateTime dtexp = txtexpirdate.Date;

                    string isedited = hdnisedited.Value;
                    decimal inputqnty = 0;
                    int noofserial = 0;
                    int oldrowcount = Convert.ToInt32(hdnoldrowcount.Value);
                    int newrowcount = 0;
                    decimal hdntotalqntyPCs = Convert.ToDecimal(hdntotalqntyPC.Value);


                    if (Session["TPOCWarehouseData"] != null)
                    {
                        DataTable Warehousedts = (DataTable)Session["TPOCWarehouseData"];

                        newrowcount = Warehousedts.Select("isnew = 'new'").Count<DataRow>();

                        if (newrowcount != null && hdnisserial.Value == "true")
                        {

                            var inputqntys = Warehousedts.Select("WarehouseName= '" + WarehouseName + "' AND BatchNo = '" + BatchName + "' AND isnew = 'new'").Count<DataRow>();


                            if (inputqntys != null && !string.IsNullOrEmpty(Convert.ToString(inputqntys)))
                            {
                                noofserial = Convert.ToInt32(inputqntys + 1);

                            }
                            else
                            {
                                noofserial = 0;
                            }
                        }
                    }

                    if (hidencountforserial.Value != "2")
                    {
                        if (Warehousedt.Rows.Count > 0)  // to check total entered quantity with warehouse grid quantity.
                        {
                            //var inputqntys = Warehousedt.Compute("sum(Quantitysum)", "isnew = 'new'");   // code commented by sam for rechecking
                            var inputqntys = Warehousedt.Compute("sum(Quantitysum)", "isnew <> 'oldnew'"); // no use of isnew <> 'oldnew just to use filter parameter

                            if (inputqntys != null && !string.IsNullOrEmpty(Convert.ToString(inputqntys)))
                            {
                                inputqnty = Convert.ToDecimal(inputqntys);

                            }
                            else
                            {
                                inputqnty = 0;
                            }

                        }
                        //commentout below line for it should not add.
                        if (hdnisserial.Value == "false" && hdniswarehouse.Value == "true" && hdnisbatch.Value == "false")
                        {
                            inputqnty = inputqnty + Convert.ToDecimal(openingstock);
                        }
                        if (hdnisserial.Value == "false" && hdniswarehouse.Value == "true" && hdnisbatch.Value == "true")
                        {
                            inputqnty = inputqnty + Convert.ToDecimal(openingstock);
                        }
                        if (hdnisserial.Value == "false" && hdniswarehouse.Value == "false" && hdnisbatch.Value == "true")
                        {
                            inputqnty = inputqnty + Convert.ToDecimal(openingstock);
                        }


                    }

                    //checking quantity only for warehouse is true.
                    if (inputqnty == 0 && hdnisserial.Value == "false")
                    {
                        inputqnty = Convert.ToDecimal(openingstock);
                    }
                    #region Showhide Section
                    if (hidencountforserial.Value == "1" && hdnisserial.Value == "true" && noofserial != openingstock && hdnisbatch.Value == "true" && hdniswarehouse.Value == "true")
                    {
                        GrdWarehousePC.JSProperties["cpupdatemssgserialsetdisblebatch"] = Convert.ToString("disable");
                    }
                    if (hidencountforserial.Value == "2" && hdnisserial.Value == "true" && noofserial == openingstock && hdnisbatch.Value == "true" && hdniswarehouse.Value == "true")
                    {
                        GrdWarehousePC.JSProperties["cpupdatemssgserialsetenablebatch"] = Convert.ToString("enable");
                    }
                    if (hidencountforserial.Value == "1" && hdnisserial.Value == "true" && noofserial != openingstock && hdnisbatch.Value == "false" && hdniswarehouse.Value == "true")
                    {
                        GrdWarehousePC.JSProperties["cpupdatemssgserialsetdisblebatch"] = Convert.ToString("disable");
                    }
                    if (hidencountforserial.Value == "2" && hdnisserial.Value == "true" && noofserial == openingstock && hdnisbatch.Value == "false" && hdniswarehouse.Value == "true")
                    {
                        GrdWarehousePC.JSProperties["cpupdatemssgserialsetenablebatch"] = Convert.ToString("enable");
                    }

                    ///batch and serial only
                    if (hidencountforserial.Value == "1" && hdnisserial.Value == "true" && noofserial != openingstock && hdnisbatch.Value == "true" && hdniswarehouse.Value == "false")
                    {
                        GrdWarehousePC.JSProperties["cpupdatemssgserialsetdisblebatch"] = Convert.ToString("disable");
                    }

                    if (hidencountforserial.Value == "2" && hdnisserial.Value == "true" && noofserial == openingstock && hdnisbatch.Value == "true" && hdniswarehouse.Value == "false")
                    {
                        GrdWarehousePC.JSProperties["cpupdatemssgserialsetenablebatch"] = Convert.ToString("enable");
                    }
                    #endregion Showhide Section
                    ///end batch and serial only



                    //

                    //if (hdnisserial.Value == "true" && ((Convert.ToDecimal(Convert.ToString((oldrowcount - newrowcount)).Replace('-', '0')) == hdntotalqntyPCs) || (Convert.ToDecimal(Convert.ToString((oldrowcount - newrowcount)).Replace('-', '0')) == (hdntotalqntyPCs * Convert.ToDecimal(hdnbatchchanged.Value)))) && ((hdniswarehouse.Value == "true" && hdnisbatch.Value == "true") && (hdnoldwarehousname.Value == WarehouseName && hdnoldbatchno.Value == BatchName)) || (hdniswarehouse.Value == "true" && hdnisbatch.Value == "false") && (hdnoldwarehousname.Value == WarehouseName) || (hdniswarehouse.Value == "false" && hdnisbatch.Value == "true") && (hdnoldbatchno.Value == BatchName))
                    //{
                    //    GrdWarehousePC.JSProperties["cpupdatemssgserial"] = Convert.ToString("Please make sure quantity and no of Serial are equal or not.");
                    //}
                    //if (hdnisserial.Value == "true" && noofserial > openingstock)   //totalopeining

                    #region Code Commented and modified by Sam For Serial on 13062017
                    decimal rowcnt = Warehousedt.Rows.Count;

                    if (hdnisserial.Value == "true")  // totalopeining  quantity of batch grid column. 
                    {
                        if (Warehousedt.Rows.Count > 0)
                        {
                            var toatlserialqty = Warehousedt.Compute("sum(Quantitysum)", "isnew <> 'oldnew'");
                            if (rowcnt == Convert.ToDecimal(toatlserialqty))
                            {
                                decimal TotalSerialEnterQty = rowcnt + Convert.ToDecimal(txtqnty.Text);
                                if (TotalSerialEnterQty > totalopeining)
                                {
                                    GrdWarehousePC.JSProperties["cpupdatemssgserial"] = Convert.ToString("Please make sure quantity and no of Serial are equal or not.");
                                    return;
                                }
                                else
                                {

                                }
                            }
                        }

                    }

                    //if (hdnisserial.Value == "true" && noofserial > totalopeining)  // totalopeining  quantity of batch grid column. 
                    //{
                    //    GrdWarehousePC.JSProperties["cpupdatemssgserial"] = Convert.ToString("Please make sure quantity and no of Serial are equal or not.");
                    //}

                    //else
                    //{
                    #endregion Code modified by Sam For Serial on 13062017
                    if (inputqnty <= totalopeining || isedited == "true")
                    {
                        //  Code Commented By Sam on 05062017 to show proper data according to wraehouse and Batch No Section Start 
                        //if (hdniswarehouse.Value == "true" && hdnisbatch.Value == "true" && hidencountforserial.Value == "2" && hdnisserial.Value == "true")
                        //{
                        //    DataTable dttemp = Warehousedt.Copy();
                        //    foreach (DataRow srow in dttemp.Rows)
                        //   {
                        //       if(Convert.ToString(srow["WarehouseName"])==WarehouseName)
                        //       {
                        //           if(Convert.ToString(srow["BatchNo"])==BatchName)
                        //           {
                        //               var maxRow = dttemp.Select("SrlNo = MAX(SrlNo) AND WarehouseName= '" + WarehouseName + "' AND BatchNo='" + BatchName + "'");
                        //               var maxRow = dttemp.Select(" WarehouseName= '" + "" + "' AND BatchNo='" + "" + "'");
                        //               var maxRow = dttemp.Select("SrlNo = MAX(SrlNo) and WarehouseID='52'");
                        //               DataRow [] wdr=dttemp.Select("SrlNo="+)
                        //              var results =  dttemp.AsEnumerable().Max(x=>x[1]);
                        //               var results = from myRow in dttemp.AsEnumerable()
                        //                  where  myRow.Field<int>("SrlNo") == 
                        //                  select results;

                        //               List<int> levels = dttemp.AsEnumerable().Select(al => al.Field<int>("SrlNo")).Distinct().ToList().Where(x=>x.;

                        //                int max = levels.Max();
                        //                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, "", "", SerialName, dtmgh, dtexp, openingstock, "0", "0", "0", "0", "", "", "", SerialName, null, null, 0, "new", Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));


                        //           }
                        //       }
                        //       else
                        //       {
                        //           Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, dtmgh, dtexp, openingstock, "0", "0", "0", "0", "", "", "", SerialName, null, null, 0, "new", Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                        //       }

                        //   }


                        //}
                        if (hdniswarehouse.Value == "true" && hdnisbatch.Value == "true" && hidencountforserial.Value == "2" && hdnisserial.Value == "true" && hdnoldwarehousname.Value == WarehouseName && hdnoldbatchno.Value == BatchName)
                        {
                            if (Convert.ToString(dtmgh) != "1/1/0001 12:00:00 AM" && Convert.ToString(dtexp) != "1/1/0001 12:00:00 AM")
                            {

                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, dtmgh, dtexp, openingstock, "0", "0", "0", "0", "", "", "", SerialName, null, null, 0, "new", Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                                GrdWarehousePC.JSProperties["cpinsertmssgserial"] = Convert.ToString("Inserted.");

                            }
                            else
                            {
                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, null, null, openingstock, "0", "0", "0", "0", "", "", "", SerialName, null, null, 0, "new", Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));

                                GrdWarehousePC.JSProperties["cpinsertmssgserial"] = Convert.ToString("Inserted.");

                            }
                        }

                        //  Code Commented By Sam on 05062017 to show proper data according to wraehouse and Batch No Section End







                        else if (hdniswarehouse.Value == "false" && hdnisbatch.Value == "false" && hdnisserial.Value == "true")
                        {

                            if (Convert.ToString(dtmgh) != "1/1/0001 12:00:00 AM" && Convert.ToString(dtexp) != "1/1/0001 12:00:00 AM")
                            {

                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, dtmgh, dtexp, openingstock, "0", "0", "0", "0", WarehouseName, BatchName, openingstock, SerialName, dtmgh, dtexp, openingstock, "new", Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                                GrdWarehousePC.JSProperties["cpinsertmssgserial"] = Convert.ToString("Inserted.");
                            }
                            else
                            {
                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, null, null, openingstock, "0", "0", "0", "0", WarehouseName, BatchName, openingstock, SerialName, null, null, openingstock, "new", Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                                GrdWarehousePC.JSProperties["cpinsertmssgserial"] = Convert.ToString("Inserted.");

                            }

                        }
                        else if (hdniswarehouse.Value == "true" && hidencountforserial.Value == "2" && hdnisbatch.Value == "false" && hdnisserial.Value == "true")
                        {
                            if (Convert.ToString(dtmgh) != "1/1/0001 12:00:00 AM" && Convert.ToString(dtexp) != "1/1/0001 12:00:00 AM")
                            {

                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, dtmgh, dtexp, openingstock, "0", "0", "0", "0", "", "", "", SerialName, null, null, 0, "new", Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                                GrdWarehousePC.JSProperties["cpinsertmssgserial"] = Convert.ToString("Inserted.");

                            }
                            else
                            {
                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, null, null, openingstock, "0", "0", "0", "0", "", "", "", SerialName, null, null, 0, "new", Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));

                                GrdWarehousePC.JSProperties["cpinsertmssgserial"] = Convert.ToString("Inserted.");

                            }
                        }
                        //batch only with serial

                        else if (hdniswarehouse.Value == "false" && hidencountforserial.Value == "2" && hdnisbatch.Value == "true" && hdnisserial.Value == "true")
                        {
                            if (Convert.ToString(dtmgh) != "1/1/0001 12:00:00 AM" && Convert.ToString(dtexp) != "1/1/0001 12:00:00 AM")
                            {

                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, dtmgh, dtexp, openingstock, "0", "0", "0", "0", "", "", "", SerialName, null, null, 0, "new", Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                                GrdWarehousePC.JSProperties["cpinsertmssgserial"] = Convert.ToString("Inserted.");

                            }
                            else
                            {
                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, null, null, openingstock, "0", "0", "0", "0", "", "", "", SerialName, null, null, 0, "new", Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));

                                GrdWarehousePC.JSProperties["cpinsertmssgserial"] = Convert.ToString("Inserted.");

                            }
                        }
                        //batch only.
                        else
                        {
                            if (Convert.ToString(dtmgh) != "1/1/0001 12:00:00 AM" && Convert.ToString(dtexp) != "1/1/0001 12:00:00 AM")
                            {

                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, dtmgh, dtexp, openingstock, "0", "0", "0", "0", WarehouseName, BatchName, openingstock, SerialName, dtmgh, dtexp, openingstock, "new", Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));

                            }
                            else
                            {
                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, null, null, openingstock, "0", "0", "0", "0", WarehouseName, BatchName, openingstock, SerialName, null, null, openingstock, "new", Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));


                            }
                            if (hdnisserial.Value == "true")
                            {
                                GrdWarehousePC.JSProperties["cpinsertmssg"] = Convert.ToString("Inserted.");
                            }
                            if (hdnisserial.Value == "false" && hdniswarehouse.Value == "true" && hdnisbatch.Value == "true")
                            {
                                GrdWarehousePC.JSProperties["cpbatchinsertmssg"] = Convert.ToString("Inserted.");
                            }
                            if (hdnisserial.Value == "false" && hdniswarehouse.Value == "false" && hdnisbatch.Value == "true")
                            {
                                GrdWarehousePC.JSProperties["cpbatchinsertmssg"] = Convert.ToString("Inserted.");
                            }

                        }


                        Session["TPOCWarehouseData"] = Warehousedt;


                        GrdWarehousePC.DataSource = Warehousedt.DefaultView;
                        GrdWarehousePC.DataBind();
                    }
                    else
                    {
                        GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("You have entered Quantity greater than Opening Quantity. Cannot Proceed.");
                    }
                    //}
                }
                else
                {
                    GrdWarehousePC.JSProperties["cperrorMsg"] = "Duplicate serial";
                }
                //
            }
            #endregion
            #region SaveAll
            if (strSplitCommand == "Saveall")
            {
                string openingstock = Convert.ToString(hdfopeningstockPC.Value);

                if (Session["TPOCWarehouseData"] == null && hdnisolddeleted.Value == "false")
                {
                    if (!string.IsNullOrEmpty(hdnvalue.Value) && !string.IsNullOrEmpty(hdnrate.Value))
                    {

                    }
                    else
                    {
                        GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Nothing to Saved .");
                    }

                }
                else if (Session["TPOCWarehouseData"] == null && openingstock != "0.0")
                {

                    //deleteALL();

                    GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("You can not save blank data.");
                }
                else
                {
                    string ProductID = Convert.ToString(hdfProductIDPC.Value);
                    string stockid = Convert.ToString(hdfstockidPC.Value);
                    openingstock = Convert.ToString(hdfopeningstockPC.Value);
                    string branchid = Convert.ToString(hdbranchIDPC.Value);
                    string isolddeletd = hdnisolddeleted.Value;

                    int oldrowcount = Convert.ToInt32(hdnoldrowcount.Value);
                    int newrowcount = 0;
                    int updaterows = 0;
                    int deleted = 0;
                    decimal hdntotalqntyPCs = Convert.ToDecimal(hdntotalqntyPC.Value);

                    DataTable Warehousedts = (DataTable)Session["TPOCWarehouseData"];

                    newrowcount = Warehousedts.Select("isnew = 'new'").Count<DataRow>();
                    updaterows = Warehousedts.Select("isnew = 'Updated'").Count<DataRow>();


                    if (newrowcount != 0 || updaterows != 0 || Session["TPOWarehouseDataDelete"] != null)
                    {
                        decimal inputqnty = 0;
                        decimal totalopeining = Convert.ToDecimal(hdfopeningstockPC.Value);
                        decimal oldtotalopeining = Convert.ToDecimal(oldopeningqntity.Value);

                        var inputqntys = Warehousedts.Compute("sum(Quantitysum)", "isnew = 'new'");
                        var updateqnty = Warehousedts.Compute("sum(Quantitysum)", "isnew = 'Updated'");
                        var oldeqnty = Warehousedts.Compute("sum(Quantitysum)", "isnew = 'old'");
                        decimal deletd = Convert.ToDecimal(hdndeleteqnity.Value);

                        if (inputqntys != null && !string.IsNullOrEmpty(Convert.ToString(inputqntys)))
                        {
                            inputqnty = Convert.ToDecimal(inputqntys);

                        }
                        if (updateqnty != null && !string.IsNullOrEmpty(Convert.ToString(updateqnty)))
                        {
                            inputqnty = inputqnty + Convert.ToDecimal(updateqnty);


                        }
                        if (oldeqnty != null && !string.IsNullOrEmpty(Convert.ToString(oldeqnty)))
                        {
                            inputqnty = inputqnty + Convert.ToDecimal(oldeqnty);


                        }
                        //commnet for allowing reduce from main page
                        //if ((oldeqnty != null && !string.IsNullOrEmpty(Convert.ToString(oldeqnty))) || (updateqnty != null && !string.IsNullOrEmpty(Convert.ToString(updateqnty))))
                        //{

                        //    //totalopeining = totalopeining + oldtotalopeining; //commnet for allowing reduce from main page
                        //    //if (hdnisreduing.Value == "true")
                        //    //{
                        //    //    totalopeining = oldtotalopeining;
                        //    //}
                        //    //else
                        //    //{
                        //    //    totalopeining = totalopeining + oldtotalopeining;
                        //    //}
                        //    totalopeining = oldtotalopeining;
                        //}
                        //if (deletd > 0 & isolddeletd == "true")
                        //{
                        //    totalopeining = Convert.ToDecimal(oldopeningqntity.Value) - deletd;
                        //}

                        if (inputqnty == totalopeining)
                        {
                            //if (hdnisserial.Value == "true" && ((Convert.ToDecimal(Convert.ToString((oldrowcount - newrowcount)).Replace('-', '0')) == hdntotalqntyPCs) || (Convert.ToDecimal(Convert.ToString((oldrowcount - newrowcount)).Replace('-', '0')) == (hdntotalqntyPCs * Convert.ToDecimal(hdnbatchchanged.Value)))))
                            if (hdnisserial.Value == "true" && hdnisbatch.Value == "true" && hdniswarehouse.Value == "true")
                            {

                                if (ProductID != "0" && branchid != "0")
                                {
                                    int output = Insertupdatedata(ProductID, stockid, branchid);
                                    if (output > 0)
                                    {
                                        DataTable Warehousedt = new DataTable();
                                        Warehousedt = GetRecord(stockid);
                                        if (Warehousedt.Rows.Count > 0)
                                        {
                                            Session["TPOCWarehouseData"] = Warehousedt;

                                            GrdWarehousePC.DataSource = Warehousedt.DefaultView;
                                            GrdWarehousePC.DataBind();
                                        }
                                        else
                                        {

                                            Session["TPOCWarehouseData"] = null;

                                            GrdWarehousePC.DataSource = null;
                                            GrdWarehousePC.DataBind();

                                        }

                                        GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Saved Successfully.");

                                    }

                                }
                                else
                                {
                                    GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Please try again null parameters.");
                                }
                            }
                            else if (hdnisserial.Value != "true" && hdnisbatch.Value != "true" && hdniswarehouse.Value == "true")
                            {
                                if (ProductID != "0" && branchid != "0")
                                {
                                    int output = Insertupdatedata(ProductID, stockid, branchid);
                                    if (output > 0)
                                    {
                                        DataTable Warehousedt = new DataTable();
                                        Warehousedt = GetRecord(stockid);
                                        if (Warehousedt.Rows.Count > 0)
                                        {
                                            Session["TPOCWarehouseData"] = Warehousedt;

                                            GrdWarehousePC.DataSource = Warehousedt.DefaultView;
                                            GrdWarehousePC.DataBind();
                                        }
                                        else
                                        {

                                            Session["TPOCWarehouseData"] = null;

                                            GrdWarehousePC.DataSource = null;
                                            GrdWarehousePC.DataBind();

                                        }

                                        GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Saved Successfully.");

                                    }

                                }
                                else
                                {
                                    GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Please try again null parameters.");
                                }
                            }
                            else if (hdnisserial.Value != "true" && hdnisbatch.Value == "true")
                            {
                                if (ProductID != "0" && branchid != "0")
                                {
                                    int output = Insertupdatedata(ProductID, stockid, branchid);
                                    if (output > 0)
                                    {
                                        DataTable Warehousedt = new DataTable();
                                        Warehousedt = GetRecord(stockid);
                                        if (Warehousedt.Rows.Count > 0)
                                        {
                                            Session["TPOCWarehouseData"] = Warehousedt;

                                            GrdWarehousePC.DataSource = Warehousedt.DefaultView;
                                            GrdWarehousePC.DataBind();
                                        }
                                        else
                                        {

                                            Session["TPOCWarehouseData"] = null;

                                            GrdWarehousePC.DataSource = null;
                                            GrdWarehousePC.DataBind();

                                        }

                                        GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Saved Successfully.");

                                    }

                                }
                                else
                                {
                                    GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Please try again null parameters.");
                                }

                            }
                            else if (hdnisserial.Value == "true" && hdnisbatch.Value != "true" && hdniswarehouse.Value != "true")
                            {
                                if (ProductID != "0" && branchid != "0")
                                {
                                    int output = Insertupdatedata(ProductID, stockid, branchid);
                                    if (output > 0)
                                    {
                                        DataTable Warehousedt = new DataTable();
                                        Warehousedt = GetRecord(stockid);
                                        if (Warehousedt.Rows.Count > 0)
                                        {
                                            Session["TPOCWarehouseData"] = Warehousedt;

                                            GrdWarehousePC.DataSource = Warehousedt.DefaultView;
                                            GrdWarehousePC.DataBind();
                                        }
                                        else
                                        {

                                            Session["TPOCWarehouseData"] = null;

                                            GrdWarehousePC.DataSource = null;
                                            GrdWarehousePC.DataBind();

                                        }

                                        GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Saved Successfully.");

                                    }

                                }
                                else
                                {
                                    GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Please try again null parameters.");
                                }
                            }
                            #region WS section Final Save Checking
                            else if (hdnisserial.Value == "true" && hdnisbatch.Value != "true" && hdniswarehouse.Value == "true")
                            {

                                if (ProductID != "0" && branchid != "0")
                                {
                                    DataTable Warehousedt = new DataTable();
                                    int output = Insertupdatedata(ProductID, stockid, branchid);
                                    if (output > 0)
                                    {

                                        Warehousedt = GetRecord(stockid);
                                        if (Warehousedt.Rows.Count > 0)
                                        {
                                            Session["TPOCWarehouseData"] = Warehousedt;

                                            GrdWarehousePC.DataSource = Warehousedt.DefaultView;
                                            GrdWarehousePC.DataBind();
                                        }
                                        else
                                        {

                                            Session["TPOCWarehouseData"] = null;

                                            GrdWarehousePC.DataSource = null;
                                            GrdWarehousePC.DataBind();

                                        }
                                        if (totalopeining != Convert.ToDecimal(Warehousedt.Rows.Count))
                                        {
                                            GrdWarehousePC.JSProperties["cpupdatemssgserial"] = Convert.ToString("Please make sure quantity and no of Serial are equal or not.");
                                        }
                                        else
                                        {
                                            GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Saved Successfully.");
                                        }

                                    }

                                }
                                else
                                {
                                    GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Please try again null parameters.");
                                }
                            }
                            #endregion WS section Final Save Checking
                            else if (hdnisserial.Value == "true" && hdnisbatch.Value == "true" && hdniswarehouse.Value != "true")
                            {

                                if (ProductID != "0" && branchid != "0")
                                {
                                    int output = Insertupdatedata(ProductID, stockid, branchid);
                                    if (output > 0)
                                    {
                                        DataTable Warehousedt = new DataTable();
                                        Warehousedt = GetRecord(stockid);
                                        if (Warehousedt.Rows.Count > 0)
                                        {
                                            Session["TPOCWarehouseData"] = Warehousedt;

                                            GrdWarehousePC.DataSource = Warehousedt.DefaultView;
                                            GrdWarehousePC.DataBind();
                                        }
                                        else
                                        {

                                            Session["TPOCWarehouseData"] = null;

                                            GrdWarehousePC.DataSource = null;
                                            GrdWarehousePC.DataBind();

                                        }

                                        GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Saved Successfully.");

                                    }

                                }
                                else
                                {
                                    GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Please try again null parameters.");
                                }
                            }
                            else
                            {
                                GrdWarehousePC.JSProperties["cpupdatemssgserial"] = Convert.ToString("Please make sure quantity and no of Serial are equal or not.");

                            }

                        }
                        else
                        {
                            GrdWarehousePC.JSProperties["cpupdatemssgserial"] = Convert.ToString("You have entered Quantity greater than Opening Quantity. Cannot Proceed.");
                        }

                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(hdnvalue.Value) && !string.IsNullOrEmpty(hdnrate.Value))
                        {
                            int olfdatterows = Warehousedts.Select("isnew = 'old'").Count<DataRow>();
                            if (olfdatterows != 0)
                            {
                                var oldeqnty = Warehousedts.Compute("sum(Quantitysum)", "isnew = 'old'");
                                if (Convert.ToDecimal(hdfopeningstockPC.Value) < Convert.ToDecimal(oldeqnty))
                                {
                                    GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Opening quantity and enterd quantity are mismatch.");
                                }
                                else
                                {

                                    // Code Added By sam on 11042017

                                    GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Saved Successfully.");


                                    // Code Added by Sam on 11042017

                                }

                            }
                            else
                            {

                            }

                        }
                        else
                        {
                            GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("Nothing to Saved .");
                        }

                    }
                }

            }

            #endregion
            #region CheckDataExist
            if (strSplitCommand == "checkdataexist")
            {
                Session["TPOCWarehouseData"] = null;

                GrdWarehousePC.DataSource = null;

                string strProductname = Convert.ToString(e.Parameters.Split('~')[4]);
                string name = strProductname;
                name = name.Replace("squot", "'");
                name = name.Replace("coma", ",");
                name = name.Replace("slash", "/");

                Session["WarehouseUpdatedData"] = null;
                Session["TPOWarehouseDataDelete"] = null;

                GrdWarehousePC.JSProperties["cpproductname"] = Convert.ToString(name);


                string strProductID = Convert.ToString(e.Parameters.Split('~')[1]);

                string stockids = Convert.ToString(e.Parameters.Split('~')[2]);

                string branchid = Convert.ToString(e.Parameters.Split('~')[3]);

                DataTable Warehousedt = new DataTable();
                Warehousedt = GetRecord(stockids);
                if (Warehousedt.Rows.Count > 0)
                {
                    Session["TPOCWarehouseData"] = Warehousedt;

                    GrdWarehousePC.DataSource = Warehousedt.DefaultView;
                    GrdWarehousePC.DataBind();
                    if (rdl_PurchaseInvoice.Items.Count > 1)
                    {
                        if (rdl_PurchaseInvoice.Items[0].Selected)
                        {
                            GrdWarehousePC.Columns["Action"].Visible = false;
                        }
                        if (rdl_PurchaseInvoice.Items[1].Selected)
                        {
                            GrdWarehousePC.Columns["Action"].Visible = false;
                        }
                        if (!rdl_PurchaseInvoice.Items[0].Selected && !rdl_PurchaseInvoice.Items[1].Selected)
                        {
                            GrdWarehousePC.Columns["Action"].Visible = true;
                        }
                    }
                    else
                    {
                        if (rdl_PurchaseInvoice.Items[0].Selected)
                        {
                            GrdWarehousePC.Columns["Action"].Visible = false;
                        }
                        else
                        {
                            GrdWarehousePC.Columns["Action"].Visible = true;
                        }
                    }
                }
                else
                {

                    Session["TPOCWarehouseData"] = null;

                    GrdWarehousePC.DataSource = null;
                    GrdWarehousePC.DataBind();

                }


            }
            //if (strSplitCommand == "UpdatedataReffresh")
            //{
            //    if (Session["TPOCWarehouseData"] != null)
            //    {

            //        GrdWarehousePC.DataSource = Session["TPOCWarehouseData"];
            //        GrdWarehousePC.DataBind();
            //    }
            //}
            #endregion
            #region updatedata
            if (strSplitCommand == "Updatedata")
            {
                string WarehouseID = Convert.ToString(e.Parameters.Split('~')[1]);

                string WarehouseName = Convert.ToString(e.Parameters.Split('~')[2]);

                string BatchName = Convert.ToString(e.Parameters.Split('~')[3]);

                string SerialName = Convert.ToString(e.Parameters.Split('~')[4]);

                string slno = Convert.ToString(e.Parameters.Split('~')[5]);
                string qntity = Convert.ToString(e.Parameters.Split('~')[6]);
                string isviewqntitynull = hdnisviewqntityhas.Value;
                string isserialenable = hdnisserial.Value;
                DateTime expdate = txtexpirdate.Date;
                DateTime mkgdate = txtmkgdate.Date;
                DataTable Warehousedt = new DataTable();

                decimal openingstock = Convert.ToDecimal(txtqnty.Text);
                if (qntity == "0.0000" && openingstock <= 0)
                {
                    qntity = batchqnty.Text;
                    openingstock = Convert.ToDecimal(qntity);
                }

                if (WarehouseID == "null")
                {
                    WarehouseID = "0";
                    WarehouseName = "";
                }

                bool isserialunique = false;
                if (hdnisserial.Value == "true")
                {
                    isserialunique = CheckUniqueSerial(SerialName, slno);
                }

                if (Session["TPOCWarehouseData"] != null && isserialunique == false)
                {
                    Warehousedt = (DataTable)Session["TPOCWarehouseData"];
                    DataSet dataSet1 = new DataSet();
                    DataTable dt1 = new DataTable();
                    dt1 = Warehousedt.Copy();
                    dataSet1.Tables.Add(dt1);

                    if (dataSet1.Tables[0].Rows.Count > 0)
                    {
                        DataRow[] customerRow = dataSet1.Tables[0].Select("SrlNo ='" + slno + "'");
                        if (isserialenable == "false" && isviewqntitynull != "false")
                        {
                            customerRow[0]["WarehouseID"] = WarehouseID;
                            customerRow[0]["WarehouseName"] = WarehouseName;
                            customerRow[0]["BatchNo"] = BatchName;
                            customerRow[0]["SerialNo"] = SerialName;
                            customerRow[0]["Quantity"] = openingstock;

                            if (Convert.ToString(expdate) != "1/1/0001 12:00:00 AM" && Convert.ToString(mkgdate) != "1/1/0001 12:00:00 AM")
                            {
                                customerRow[0]["viewMFGDate"] = mkgdate;
                                customerRow[0]["viewExpiryDate"] = expdate;
                            }

                            customerRow[0]["viewWarehouseName"] = WarehouseName;
                            customerRow[0]["viewBatchNo"] = BatchName;
                            customerRow[0]["viewSerialNo"] = SerialName;
                            customerRow[0]["viewQuantity"] = qntity;
                            customerRow[0]["Quantitysum"] = openingstock;
                            customerRow[0]["isnew"] = "Updated";
                        }
                        if (isserialenable == "true" && isviewqntitynull == "false")
                        {
                            customerRow[0]["WarehouseID"] = WarehouseID;
                            customerRow[0]["WarehouseName"] = WarehouseName;
                            customerRow[0]["BatchNo"] = BatchName;
                            customerRow[0]["SerialNo"] = SerialName;
                            customerRow[0]["Quantity"] = openingstock;

                            if (Convert.ToString(expdate) != "1/1/0001 12:00:00 AM" && Convert.ToString(mkgdate) != "1/1/0001 12:00:00 AM")
                            {
                                customerRow[0]["viewMFGDate"] = mkgdate;
                                customerRow[0]["viewExpiryDate"] = expdate;
                            }

                            customerRow[0]["viewWarehouseName"] = WarehouseName;
                            customerRow[0]["viewBatchNo"] = BatchName;
                            customerRow[0]["viewSerialNo"] = SerialName;
                            customerRow[0]["viewQuantity"] = openingstock;
                            customerRow[0]["Quantitysum"] = openingstock;
                            customerRow[0]["isnew"] = "Updated";
                        }
                        if (isserialenable == "true" && isviewqntitynull == "true")
                        {
                            customerRow[0]["WarehouseID"] = WarehouseID;
                            customerRow[0]["WarehouseName"] = WarehouseName;
                            customerRow[0]["BatchNo"] = BatchName;
                            customerRow[0]["SerialNo"] = SerialName;
                            customerRow[0]["Quantity"] = openingstock;



                            customerRow[0]["viewSerialNo"] = SerialName;

                            customerRow[0]["Quantitysum"] = 0;
                            customerRow[0]["isnew"] = "Updated";
                        }
                        if (isserialenable == "false" && isviewqntitynull == "false")
                        {
                            customerRow[0]["WarehouseID"] = WarehouseID;
                            customerRow[0]["WarehouseName"] = WarehouseName;
                            customerRow[0]["BatchNo"] = BatchName;
                            customerRow[0]["SerialNo"] = SerialName;
                            customerRow[0]["Quantity"] = openingstock;

                            customerRow[0]["SerialNo"] = SerialName;
                            customerRow[0]["Quantity"] = openingstock;
                            if (Convert.ToString(expdate) != "1/1/0001 12:00:00 AM" && Convert.ToString(mkgdate) != "1/1/0001 12:00:00 AM")
                            {
                                customerRow[0]["viewMFGDate"] = mkgdate;
                                customerRow[0]["viewExpiryDate"] = expdate;
                            }
                            customerRow[0]["viewWarehouseName"] = WarehouseName;
                            customerRow[0]["viewBatchNo"] = BatchName;
                            customerRow[0]["viewSerialNo"] = SerialName;
                            customerRow[0]["viewQuantity"] = openingstock;
                            customerRow[0]["Quantitysum"] = openingstock;
                            customerRow[0]["isnew"] = "Updated";
                        }

                        GrdWarehousePC.JSProperties["cpupdateexistingdata"] = Convert.ToString("Saved.");
                    }
                    Warehousedt = null;
                    Warehousedt = dataSet1.Tables[0];
                    Session["TPOCWarehouseData"] = Warehousedt;

                    Session["WarehouseUpdatedData"] = Warehousedt;

                    GrdWarehousePC.DataSource = Warehousedt.DefaultView;
                    GrdWarehousePC.DataBind();

                }
            }


            #endregion
            #region newrowupdate
            if (strSplitCommand == "NewUpdatedata")
            {
                string WarehouseID = Convert.ToString(e.Parameters.Split('~')[1]);

                string WarehouseName = Convert.ToString(e.Parameters.Split('~')[2]);

                string BatchName = Convert.ToString(e.Parameters.Split('~')[3]);

                string SerialName = Convert.ToString(e.Parameters.Split('~')[4]);

                string slno = Convert.ToString(e.Parameters.Split('~')[5]);
                string qntity = Convert.ToString(e.Parameters.Split('~')[6]);
                string isviewqntitynull = hdnisviewqntityhas.Value;
                string isserialenable = hdnisserial.Value;
                DataTable Warehousedt = new DataTable();
                decimal openingstock = Convert.ToDecimal(txtqnty.Text);
                if (qntity == "0.0000" && openingstock <= 0)
                {
                    qntity = batchqnty.Text;
                    openingstock = Convert.ToDecimal(qntity);
                }
                bool isserialunique = false;
                if (hdnisserial.Value == "true")
                {
                    isserialunique = CheckUniqueSerial(SerialName, slno);
                }


                if (Session["TPOCWarehouseData"] != null && isserialunique == false)
                {
                    Warehousedt = (DataTable)Session["TPOCWarehouseData"];
                    DataSet dataSet1 = new DataSet();
                    DataTable dt1 = new DataTable();
                    dt1 = Warehousedt.Copy();
                    dataSet1.Tables.Add(dt1);

                    if (dataSet1.Tables[0].Rows.Count > 0)
                    {
                        DataRow[] customerRow = dataSet1.Tables[0].Select("SrlNo ='" + slno + "'");
                        if (customerRow.Length > 0)
                        {
                            if (isserialenable == "false" && isviewqntitynull != "false")
                            {
                                customerRow[0]["WarehouseID"] = WarehouseID;
                                customerRow[0]["WarehouseName"] = WarehouseName;
                                customerRow[0]["BatchNo"] = BatchName;
                                customerRow[0]["SerialNo"] = SerialName;
                                customerRow[0]["Quantity"] = openingstock;

                                customerRow[0]["viewWarehouseName"] = WarehouseName;
                                customerRow[0]["viewBatchNo"] = BatchName;
                                customerRow[0]["viewSerialNo"] = SerialName;
                                customerRow[0]["viewQuantity"] = openingstock;
                                customerRow[0]["Quantitysum"] = openingstock;
                                customerRow[0]["isnew"] = "new";
                            }
                            if (isserialenable == "true" && isviewqntitynull == "false")
                            {
                                customerRow[0]["WarehouseID"] = WarehouseID;
                                customerRow[0]["WarehouseName"] = WarehouseName;
                                customerRow[0]["BatchNo"] = BatchName;
                                customerRow[0]["SerialNo"] = SerialName;
                                customerRow[0]["Quantity"] = openingstock;

                                customerRow[0]["viewWarehouseName"] = WarehouseName;
                                customerRow[0]["viewBatchNo"] = BatchName;
                                customerRow[0]["viewSerialNo"] = SerialName;
                                customerRow[0]["viewQuantity"] = openingstock;
                                customerRow[0]["Quantitysum"] = openingstock;
                                customerRow[0]["isnew"] = "new";
                            }
                            if (isserialenable == "true" && isviewqntitynull == "true")
                            {
                                customerRow[0]["WarehouseID"] = WarehouseID;
                                customerRow[0]["WarehouseName"] = WarehouseName;
                                customerRow[0]["BatchNo"] = BatchName;
                                customerRow[0]["SerialNo"] = SerialName;
                                customerRow[0]["Quantity"] = openingstock;



                                customerRow[0]["viewSerialNo"] = SerialName;

                                customerRow[0]["Quantitysum"] = 0;
                                customerRow[0]["isnew"] = "new";
                            }
                            if (isserialenable == "false" && isviewqntitynull == "false")
                            {
                                customerRow[0]["WarehouseID"] = WarehouseID;
                                customerRow[0]["WarehouseName"] = WarehouseName;
                                customerRow[0]["BatchNo"] = BatchName;
                                customerRow[0]["SerialNo"] = SerialName;
                                customerRow[0]["Quantity"] = openingstock;

                                customerRow[0]["viewWarehouseName"] = WarehouseName;
                                customerRow[0]["viewBatchNo"] = BatchName;
                                customerRow[0]["viewSerialNo"] = SerialName;
                                customerRow[0]["viewQuantity"] = openingstock;
                                customerRow[0]["Quantitysum"] = openingstock;
                                customerRow[0]["isnew"] = "new";
                            }

                            GrdWarehousePC.JSProperties["cpupdatenewdata"] = Convert.ToString("Saved.");
                        }
                    }
                    Warehousedt = null;
                    Warehousedt = dataSet1.Tables[0];
                    Session["TPOCWarehouseData"] = Warehousedt;

                    GrdWarehousePC.DataSource = Warehousedt.DefaultView;
                    GrdWarehousePC.DataBind();

                }


            }
            #endregion
            #region Delete
            if (strSplitCommand == "Delete")
            {
                string slno = Convert.ToString(e.Parameters.Split('~')[1]);

                string viewQuantity = Convert.ToString(e.Parameters.Split('~')[2]);

                string Quantity = Convert.ToString(e.Parameters.Split('~')[3]);
                string warehouseid = Convert.ToString(e.Parameters.Split('~')[4]);
                string batch = Convert.ToString(e.Parameters.Split('~')[5]);
                DataTable Warehousedt = new DataTable();

                // string isviewqntitynull = hdnisviewqntityhas.Value;
                string isserialenable = hdnisserial.Value;
                string isbatch = hdnisbatch.Value;
                string iswarehouse = hdniswarehouse.Value;
                string isolddeletd = hdnisolddeleted.Value;
                if (Session["TPOCWarehouseData"] != null)
                {
                    Warehousedt = (DataTable)Session["TPOCWarehouseData"];
                    DataSet dataSet1 = new DataSet();

                    DataTable dt1 = new DataTable();
                    dt1 = Warehousedt.Copy();



                    dataSet1.Tables.Add(dt1);

                    if (dataSet1.Tables[0].Rows.Count > 0)
                    {

                        if (isserialenable == "true" && (viewQuantity == "" || viewQuantity == "0") && isbatch == "true" && iswarehouse == "true")
                        {
                            DataRow[] customerRows = dataSet1.Tables[0].Select("WarehouseID= '" + warehouseid + "' AND BatchNo='" + batch + "'");

                            for (int i = 0; i <= customerRows.Count() - 1; i++)
                            {
                                if (Convert.ToString(customerRows[i]["SrlNo"]) == slno && string.IsNullOrEmpty(Convert.ToString(customerRows[i]["viewQuantity"])))
                                {
                                    customerRows[i]["isnew"] = "DeleteSL";
                                }
                                else if (Convert.ToString(customerRows[i]["SrlNo"]) == slno && !string.IsNullOrEmpty(Convert.ToString(customerRows[i]["viewQuantity"])))
                                {

                                    customerRows[i]["isnew"] = "DeleteWHBTSL";

                                }

                                if (!string.IsNullOrEmpty(Convert.ToString(customerRows[i]["viewQuantity"])))
                                {
                                    customerRows[i]["viewQuantity"] = Convert.ToDecimal(customerRows[i]["Quantitysum"]) - 1;
                                    customerRows[i]["Quantity"] = Convert.ToDecimal(customerRows[i]["Quantity"]) - 1;
                                    customerRows[i]["Quantitysum"] = Convert.ToDecimal(customerRows[i]["Quantitysum"]) - 1;

                                    if (Convert.ToString(customerRows[i]["isnew"]) != "new")
                                    {
                                        customerRows[i]["isnew"] = "Updated";
                                    }
                                    else
                                    {
                                        customerRows[i]["isnew"] = "new";
                                    }

                                }
                                //else
                                //{
                                //    customerRows[i]["Quantity"] = 0;
                                //    customerRows[i]["Quantitysum"] = 0;
                                //}
                            }


                            GrdWarehousePC.JSProperties["cpdeletedata"] = Convert.ToString(1);

                        }
                        else if (isserialenable == "true" && (viewQuantity == "" || viewQuantity == "0") && isbatch != "true" && iswarehouse == "true")
                        {
                            DataRow[] customerRows = dataSet1.Tables[0].Select("WarehouseID= '" + warehouseid + "'");

                            for (int i = 0; i <= customerRows.Count() - 1; i++)
                            {
                                if (Convert.ToString(customerRows[i]["SrlNo"]) == slno && string.IsNullOrEmpty(Convert.ToString(customerRows[i]["viewQuantity"])))
                                {
                                    customerRows[i]["isnew"] = "DeleteSL";
                                }
                                else if (Convert.ToString(customerRows[i]["SrlNo"]) == slno && !string.IsNullOrEmpty(Convert.ToString(customerRows[i]["viewQuantity"])))
                                {

                                    customerRows[i]["isnew"] = "DeleteWHSL";


                                }

                                if (!string.IsNullOrEmpty(Convert.ToString(customerRows[i]["viewQuantity"])))
                                {
                                    customerRows[i]["viewQuantity"] = Convert.ToDecimal(customerRows[i]["Quantitysum"]) - 1;
                                    customerRows[i]["Quantity"] = Convert.ToDecimal(customerRows[i]["Quantity"]) - 1;
                                    customerRows[i]["Quantitysum"] = Convert.ToDecimal(customerRows[i]["Quantitysum"]) - 1;

                                    if (Convert.ToString(customerRows[i]["isnew"]) != "new")
                                    {
                                        customerRows[i]["isnew"] = "Updated";
                                    }
                                    else
                                    {
                                        customerRows[i]["isnew"] = "new";
                                    }

                                }
                                else
                                {
                                    customerRows[i]["Quantity"] = 0;
                                    customerRows[i]["Quantitysum"] = 0;
                                }
                            }


                            GrdWarehousePC.JSProperties["cpdeletedata"] = Convert.ToString(1);

                        }

                        else if (isserialenable == "true" && (viewQuantity == "" || viewQuantity == "0") && isbatch == "true" && iswarehouse != "true")
                        {
                            DataRow[] customerRows = dataSet1.Tables[0].Select("BatchNo= '" + batch + "'");

                            for (int i = 0; i <= customerRows.Count() - 1; i++)
                            {
                                if (Convert.ToString(customerRows[i]["SrlNo"]) == slno && string.IsNullOrEmpty(Convert.ToString(customerRows[i]["viewQuantity"])))
                                {
                                    customerRows[i]["isnew"] = "DeleteSL";
                                }
                                else if (Convert.ToString(customerRows[i]["SrlNo"]) == slno && !string.IsNullOrEmpty(Convert.ToString(customerRows[i]["viewQuantity"])))
                                {

                                    customerRows[i]["isnew"] = "DeleteWHBTSL";

                                    //customerRows[i]["isnew"] = "DeleteWHBTSL";
                                }
                                if (!string.IsNullOrEmpty(Convert.ToString(customerRows[i]["viewQuantity"])))
                                {
                                    customerRows[i]["viewQuantity"] = Convert.ToDecimal(customerRows[i]["Quantitysum"]) - 1;
                                    customerRows[i]["Quantity"] = Convert.ToDecimal(customerRows[i]["Quantity"]) - 1;
                                    customerRows[i]["Quantitysum"] = Convert.ToDecimal(customerRows[i]["Quantitysum"]) - 1;

                                    if (Convert.ToString(customerRows[i]["isnew"]) != "new")
                                    {
                                        customerRows[i]["isnew"] = "Updated";
                                    }
                                    else
                                    {
                                        customerRows[i]["isnew"] = "new";
                                    }

                                }
                                else
                                {

                                    customerRows[i]["Quantity"] = 0;
                                    customerRows[i]["Quantitysum"] = 0;

                                }

                            }

                            GrdWarehousePC.JSProperties["cpdeletedata"] = Convert.ToString(1);


                        }
                        else
                        {

                            DataRow[] customerRow = dataSet1.Tables[0].Select("SrlNo ='" + slno + "'");
                            if (isserialenable != "true" && isbatch == "true" && iswarehouse == "true")
                            {
                                customerRow[0]["isnew"] = "DeleteWHBT";
                                GrdWarehousePC.JSProperties["cpdeletedata"] = Convert.ToString(viewQuantity);
                            }
                            if (isserialenable != "true" && isbatch != "true" && iswarehouse == "true")
                            {
                                customerRow[0]["isnew"] = "DeleteWH";
                                GrdWarehousePC.JSProperties["cpdeletedata"] = Convert.ToString(Quantity);
                            }
                            if (isserialenable != "true" && isbatch == "true" && iswarehouse != "true")
                            {
                                customerRow[0]["isnew"] = "DeleteBT";
                                GrdWarehousePC.JSProperties["cpdeletedata"] = Convert.ToString(viewQuantity);
                            }
                            if (isserialenable == "true" && isbatch == "true" && iswarehouse == "true")
                            {
                                customerRow[0]["isnew"] = "DeleteWHBTSL";
                                GrdWarehousePC.JSProperties["cpdeletedata"] = Convert.ToString(Quantity);
                            }
                            if (isserialenable == "true" && isbatch != "true" && iswarehouse == "true")
                            {
                                customerRow[0]["isnew"] = "DeleteWHSL";
                                GrdWarehousePC.JSProperties["cpdeletedata"] = Convert.ToString(1);
                            }
                            if (isserialenable == "true" && isbatch == "true" && iswarehouse != "true")
                            {
                                customerRow[0]["isnew"] = "DeleteBTSL";
                                GrdWarehousePC.JSProperties["cpdeletedata"] = Convert.ToString(1);
                            }
                        }

                    }

                    Warehousedt = null;
                    DataTable setdeletedate = new DataTable();
                    DataTable getdeletedate = dataSet1.Tables[0];

                    DataRow[] drs = getdeletedate.Select("isnew = 'DeleteWHBT' OR isnew = 'DeleteWH' OR isnew = 'DeleteWHBT' OR isnew = 'DeleteBTSL' OR isnew = 'DeleteSL' OR isnew = 'DeleteWHSL' OR isnew = 'DeleteWHBTSL'");
                    if (drs.Count() != 0)
                    {
                        if (Session["TPOWarehouseDataDelete"] != null)
                        {
                            setdeletedate = drs.CopyToDataTable();
                            DataTable dtmp = (DataTable)Session["TPOWarehouseDataDelete"];
                            dtmp.Merge(setdeletedate);
                            Session["TPOWarehouseDataDelete"] = dtmp;
                        }
                        else
                        {
                            setdeletedate = drs.CopyToDataTable();
                            Session["TPOWarehouseDataDelete"] = setdeletedate;
                        }


                    }


                    DataTable Warehousedts = dataSet1.Tables[0];
                    DataRow[] dr = Warehousedts.Select("isnew = 'new' OR isnew = 'Updated' OR isnew = 'old'");
                    if (dr.Count() > 0)
                    {

                        Warehousedt = dr.CopyToDataTable();
                        Warehousedt.DefaultView.Sort = "SrlNo asc";
                        Warehousedt = Warehousedt.DefaultView.ToTable(true);

                        Session["TPOCWarehouseData"] = Warehousedt;

                        GrdWarehousePC.DataSource = Session["TPOCWarehouseData"];
                        GrdWarehousePC.DataBind();
                    }
                    else
                    {
                        Session["TPOCWarehouseData"] = null;

                        //Warehousedt.Columns.Add("SrlNo", typeof(string));
                        //Warehousedt.Columns.Add("WarehouseID", typeof(string));
                        //Warehousedt.Columns.Add("WarehouseName", typeof(string));

                        //Warehousedt.Columns.Add("BatchNo", typeof(string));
                        //Warehousedt.Columns.Add("SerialNo", typeof(string));

                        //Warehousedt.Columns.Add("MFGDate", typeof(DateTime));
                        //Warehousedt.Columns.Add("ExpiryDate", typeof(DateTime));
                        //Warehousedt.Columns.Add("Quantity", typeof(decimal));

                        //Warehousedt.Columns.Add("BatchWarehouseID", typeof(string));
                        //Warehousedt.Columns.Add("BatchWarehousedetailsID", typeof(string));
                        //Warehousedt.Columns.Add("BatchID", typeof(string));
                        //Warehousedt.Columns.Add("SerialID", typeof(string));


                        //Warehousedt.Columns.Add("viewWarehouseName", typeof(string));

                        //Warehousedt.Columns.Add("viewBatchNo", typeof(string));
                        //Warehousedt.Columns.Add("viewQuantity", typeof(string));
                        //Warehousedt.Columns.Add("viewSerialNo", typeof(string));



                        //Warehousedt.Columns.Add("viewMFGDate", typeof(DateTime));
                        //Warehousedt.Columns.Add("viewExpiryDate", typeof(DateTime));

                        //Warehousedt.Columns.Add("Quantitysum", typeof(decimal));
                        //Warehousedt.Columns.Add("isnew", typeof(string));

                        GrdWarehousePC.DataSource = Warehousedt;
                        GrdWarehousePC.DataBind();



                    }


                }
            #endregion
            }


        }

        public bool CheckUniqueSerial(string serial, string slno)
        {
            bool ISexist = false;

            if (Session["TPOCWarehouseData"] != null && hdnisserial.Value == "true")
            {
                DataTable Warehousedts = (DataTable)Session["TPOCWarehouseData"];
                //
                DataRow[] dr = Warehousedts.Select(" productid = '" + Convert.ToString(hdfProductIDPC.Value) + "'and isnew = 'new' OR isnew = 'Updated' OR isnew = 'old'");
                #region
                if (dr.Count() > 0)
                {
                    DataTable dt = dr.CopyToDataTable();
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                        {
                            if (slno == "new")
                            {
                                if (Convert.ToString(dt.Rows[i]["viewSerialNo"]).ToUpper() == serial.ToUpper())
                                {
                                    ISexist = true;
                                    break;
                                }
                            }
                            else
                            {
                                if (Convert.ToString(dt.Rows[i]["viewSerialNo"]).ToUpper() == serial.ToUpper() && Convert.ToString(dt.Rows[i]["SrlNo"]) != slno)
                                {
                                    ISexist = true;
                                    break;
                                }
                            }


                        }

                    }
                }
            }
                #endregion
            if (ISexist == false)
            {
                int serialcnt = 0;
                serialcnt = CheckSerialNoExists(serial);
                if (serialcnt > 0)
                {
                    ISexist = true;
                }
            }

            return ISexist;
        }







        public int CheckSerialNoExists(string SerialNo)
        {
            string ProductID = Convert.ToString(hdfProductID.Value);
            string BranchID = Convert.ToString(ddl_Branch.SelectedValue);

            //string query = "SELECT SerialNo FROM v_Stock_WarehouseDetails Where ProductID<>'" + ProductID.Trim() + "' AND SerialNo='" + SerialNo.Trim() + "' AND Stock_IN_Out_Type='IN'";
            //DataTable SerialCount = oDBEngine.GetDataTable(query);

            DataTable SerialCount = objPurchaseInvoice.CheckDuplicateSerial(SerialNo, ProductID, BranchID, "PurchaseSide");
            return SerialCount.Rows.Count;
        }
        public DataTable GetRecord(string stockids)
        {
            Session["TPOCWarehouseData"] = null;
            DataTable Warehousedt = new DataTable();

            if (Session["TPOCWarehouseData"] != null)
            {
                Warehousedt = (DataTable)Session["TPOCWarehouseData"];
            }
            else
            {
                Warehousedt.Columns.Add("SrlNo", typeof(Int32));
                Warehousedt.Columns.Add("WarehouseID", typeof(string));
                Warehousedt.Columns.Add("WarehouseName", typeof(string));

                Warehousedt.Columns.Add("BatchNo", typeof(string));
                Warehousedt.Columns.Add("SerialNo", typeof(string));

                Warehousedt.Columns.Add("MFGDate", typeof(DateTime));
                Warehousedt.Columns.Add("ExpiryDate", typeof(DateTime));
                Warehousedt.Columns.Add("Quantity", typeof(decimal));

                Warehousedt.Columns.Add("BatchWarehouseID", typeof(string));
                Warehousedt.Columns.Add("BatchWarehousedetailsID", typeof(string));
                Warehousedt.Columns.Add("BatchID", typeof(string));
                Warehousedt.Columns.Add("SerialID", typeof(string));


                Warehousedt.Columns.Add("viewWarehouseName", typeof(string));

                Warehousedt.Columns.Add("viewBatchNo", typeof(string));
                Warehousedt.Columns.Add("viewQuantity", typeof(string));
                Warehousedt.Columns.Add("viewSerialNo", typeof(string));



                Warehousedt.Columns.Add("viewMFGDate", typeof(DateTime));
                Warehousedt.Columns.Add("viewExpiryDate", typeof(DateTime));

                Warehousedt.Columns.Add("Quantitysum", typeof(decimal));
                Warehousedt.Columns.Add("isnew", typeof(string));

                Warehousedt.Columns.Add("productid", typeof(string));
                Warehousedt.Columns.Add("Inventrytype", typeof(string));
                Warehousedt.Columns.Add("StockID", typeof(string));
                Warehousedt.Columns.Add("pcslno", typeof(Int32));
            }


            DataTable dts = GetStockWarehouseData(stockids);

            string oldwarehousename = string.Empty;
            string oldbatchname = string.Empty;
            string oldquantity = string.Empty;
            string isoldornew = string.Empty;
            string BatchWarehouseID = string.Empty;

            if (dts.Rows.Count > 0)
            {
                for (int i = 0; i <= dts.Rows.Count - 1; i++)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(dts.Rows[i]["BatchWarehouseID"])) && Convert.ToString(dts.Rows[i]["BatchWarehouseID"]) != "0")
                    {
                        isoldornew = "old";
                    }
                    else
                    {
                        isoldornew = "new";
                    }

                    if (hdnisserial.Value == "true" && i != 0)
                    {
                        oldwarehousename = Convert.ToString(dts.Rows[i - 1]["warehouseID"]);
                        oldbatchname = Convert.ToString(dts.Rows[i - 1]["batchNO"]);
                        oldquantity = Convert.ToString(dts.Rows[i - 1]["Quantitysum"]);
                        BatchWarehouseID = Convert.ToString(dts.Rows[i - 1]["BatchWarehouseID"]);

                        if (oldwarehousename == Convert.ToString(dts.Rows[i]["warehouseID"]) && oldbatchname == Convert.ToString(dts.Rows[i]["batchNO"]) && (oldquantity == Convert.ToString(dts.Rows[i]["Quantitysum"]) || Convert.ToString(dts.Rows[i]["Quantitysum"]) == "0") && BatchWarehouseID == Convert.ToString(dts.Rows[i]["BatchWarehouseID"]))
                        {
                            if (Convert.ToString(dts.Rows[i]["MfgDate"]) == "1/1/1900 12:00:00 AM")
                            {
                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), "", "", "", Convert.ToString(dts.Rows[i]["serialno"]), null, null, 0, isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }
                            else if (string.IsNullOrEmpty(Convert.ToString(dts.Rows[i]["MfgDate"])))
                            {
                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), "", "", "", Convert.ToString(dts.Rows[i]["serialno"]), null, null, 0, isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }
                            else
                            {

                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), "", "", "", Convert.ToString(dts.Rows[i]["serialno"]), null, null, 0, isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }
                        }
                        //else if (oldwarehousename == Convert.ToString(dts.Rows[i]["warehouseID"]) && oldbatchname == Convert.ToString(dts.Rows[i]["batchNO"]))
                        //{
                        //    if (Convert.ToString(dts.Rows[i]["MfgDate"]) == "1/1/1900 12:00:00 AM")
                        //    {
                        //        Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), "", "", "", Convert.ToString(dts.Rows[i]["serialno"]), null, null, 0, "old");
                        //    }
                        //    else if (string.IsNullOrEmpty(Convert.ToString(dts.Rows[i]["MfgDate"])))
                        //    {
                        //        Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), "", "", "", Convert.ToString(dts.Rows[i]["serialno"]), null, null, 0, "old");
                        //    }
                        //    else
                        //    {

                        //        Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), "", "", "", Convert.ToString(dts.Rows[i]["serialno"]), null, null, 0, "old");
                        //    }
                        //}
                        else
                        {
                            //string viewqunatity = (Convert.ToString(dts.Rows[i]["viewQuantity"])!=string.Empty)? Convert.ToString(dts.Rows[i]["viewQuantity"]):"0";

                            // Code commented by Sam on 05062017 to set the value of viewQuantity to Quantitysum Start
                            //string viewqunatity = Convert.ToString(dts.Rows[i]["viewQuantity"]);
                            string viewqunatity = Convert.ToString(dts.Rows[i]["Quantitysum"]);

                            // Code commented by Sam on 05062017 to set the value of viewQuantity to Quantitysum End
                            string sumquantiy = (Convert.ToString(dts.Rows[i]["Quantitysum"]) != string.Empty) ? Convert.ToString(dts.Rows[i]["Quantitysum"]) : "0";
                            if (Convert.ToString(dts.Rows[i]["MfgDate"]) == "1/1/1900 12:00:00 AM")
                            {
                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(sumquantiy), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), viewqunatity, Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(sumquantiy), isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }
                            else if (string.IsNullOrEmpty(Convert.ToString(dts.Rows[i]["MfgDate"])))
                            {
                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(sumquantiy), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), viewqunatity, Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(sumquantiy), isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }
                            else
                            {

                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), viewqunatity, Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToString(sumquantiy), isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }

                        }

                    }
                    else if (hdnisserial.Value == "false" && hdniswarehouse.Value == "true" && hdnisbatch.Value == "true")
                    {
                        //string oldquantity=string.Empty;
                        if (i != 0)
                        {
                            oldwarehousename = Convert.ToString(dts.Rows[i - 1]["warehouseID"]);
                            oldquantity = Convert.ToString(dts.Rows[i - 1]["viewQuantity"]);
                        }



                        if (oldwarehousename == Convert.ToString(dts.Rows[i]["warehouseID"]))
                        {
                            if (Convert.ToString(dts.Rows[i]["MfgDate"]) == "1/1/1900 12:00:00 AM")
                            {
                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), "", Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["viewQuantity"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["viewQuantity"]), isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }
                            else if (string.IsNullOrEmpty(Convert.ToString(dts.Rows[i]["MfgDate"])))
                            {
                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), "", Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["viewQuantity"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["viewQuantity"]), isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }
                            else
                            {

                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), "", Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["viewQuantity"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["Quantitysum"]), isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }
                        }
                        else
                        {
                            if (Convert.ToString(dts.Rows[i]["MfgDate"]) == "1/1/1900 12:00:00 AM")
                            {
                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["viewQuantity"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["Quantitysum"]), isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }
                            else if (string.IsNullOrEmpty(Convert.ToString(dts.Rows[i]["MfgDate"])))
                            {
                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["viewQuantity"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["Quantitysum"]), isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }
                            else
                            {

                                Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["viewQuantity"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToString(dts.Rows[i]["Quantitysum"]), isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            }
                        }

                    }
                    else
                    {
                        // Code has been modified by Sandip on 20042017 Start

                        if (Convert.ToString(dts.Rows[i]["MfgDate"]) == "1/1/1900 12:00:00 AM")
                        {
                            // Code commented by Sam for recheck due to use of viewQuantity pending on 20042017 Start
                            //Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["viewQuantity"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["Quantitysum"]), isoldornew, Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["Quantitysum"]), isoldornew, Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            // Code commented by Sam for recheck due to use of viewQuantity pending on 20042017 End
                        }
                        else if (string.IsNullOrEmpty(Convert.ToString(dts.Rows[i]["MfgDate"])))
                        {
                            //Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["viewQuantity"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["Quantitysum"]), isoldornew, Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["Quantitysum"]), isoldornew, Convert.ToString(hdfProductIDPC.Value), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));

                        }
                        else
                        {

                            //Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["viewQuantity"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToString(dts.Rows[i]["Quantitysum"]), isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));
                            Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToDecimal(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["WarehouseName"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["Quantitysum"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToString(dts.Rows[i]["Quantitysum"]), isoldornew, Convert.ToString(dts.Rows[i]["productid"]), getinventry(), Convert.ToString(hdfstockidPC.Value), Convert.ToInt32(hdnpcslno.Value));

                        }
                        // Code has been modified by Sandip on 20042017 End
                    }


                    //Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, "", "", openingstock, "isnew");

                }
            }

            hdnoldrowcount.Value = Convert.ToString(Warehousedt.Rows.Count);

            return Warehousedt;
        }

        private int Insertupdatedata(string ProductID, string stockid, string branchid)
        {
            DataSet dsEmail = new DataSet();

            string IsserialActive = "false";

            int newrowcount = 0;

            #region Insert

            DataTable temtable = new DataTable();
            DataTable temtable2 = new DataTable();
            DataTable temtable23 = new DataTable();
            if (Session["TPOCWarehouseData"] != null)
            {
                DataTable Warehousedt = (DataTable)Session["TPOCWarehouseData"];

                temtable = Warehousedt.DefaultView.ToTable(false, "SrlNo", "BatchWarehouseID", "BatchWarehousedetailsID", "BatchID", "SerialID", "WarehouseID", "WarehouseName", "BatchNo", "SerialNo", "MFGDate", "ExpiryDate", "Quantitysum", "isnew", "viewQuantity", "productid", "Inventrytype", "StockID", "pcslno");

                if (Session["TPBwarehousedetailstemp"] != null)
                {
                    temtable23 = (DataTable)Session["TPBwarehousedetailstemp"];
                    DataRow[] rows;
                    rows = temtable23.Select("pcslno = '" + hdnpcslno.Value + "'");  //'UserName' is ColumnName
                    foreach (DataRow row in rows)
                        temtable23.Rows.Remove(row);
                    temtable23.Merge(temtable, true, MissingSchemaAction.Ignore);
                    //temtable23.Merge(temtable);
                    Session["TPBwarehousedetailstemp"] = temtable23;
                    //Session["TPBwarehousedetailstempNew"] = temtable23;   // To only check if stock has been modified in edit mode or not
                }
                else
                {
                    Session["TPBwarehousedetailstemp"] = temtable;
                    //Session["TPBwarehousedetailstempNew"] = temtable23;   // To only check if stock has been modified in edit mode or not
                }

            }


            #endregion
            #region Update
            if (Session["TPOCWarehouseData"] != null)
            {
                DataTable dt = new DataTable();
                DataTable Warehousedtups = (DataTable)Session["TPOCWarehouseData"];
                DataRow[] dr = Warehousedtups.Select("isnew = 'Updated'");
                if (dr.Count() != 0)
                {
                    dt = dr.CopyToDataTable();
                    if (Session["TPBwarehousedetailstempUpdate"] != null)
                    {
                        temtable23 = (DataTable)Session["TPBwarehousedetailstempUpdate"];
                        DataRow[] rows;
                        rows = temtable23.Select("pcslno = '" + hdnpcslno.Value + "'");  //'UserName' is ColumnName
                        foreach (DataRow row in rows)
                            temtable23.Rows.Remove(row);

                        temtable23.Merge(dt);
                        Session["TPBwarehousedetailstempUpdate"] = temtable23;
                    }
                    else
                    {
                        Session["TPBwarehousedetailstempUpdate"] = dt;
                    }


                }
            }

            #endregion
            #region delete

            if (Session["TPOWarehouseDataDelete"] != null)
            {
                DataTable dt = new DataTable();
                DataTable Warehousedtups = (DataTable)Session["TPOWarehouseDataDelete"];
                DataRow[] dr = Warehousedtups.Select("isnew = 'DeleteWHBT' OR isnew = 'DeleteWH' OR isnew = 'DeleteWHBT' OR isnew = 'DeleteBTSL' OR isnew = 'DeleteSL' OR isnew = 'DeleteWHSL' OR isnew = 'DeleteWHBTSL'");
                if (dr.Count() != 0)
                {
                    dt = dr.CopyToDataTable();
                    if (Session["TPBwarehousedetailstempDelete"] != null)
                    {
                        temtable23 = (DataTable)Session["TPBwarehousedetailstempDelete"];
                        DataRow[] rows;
                        rows = temtable23.Select("pcslno = '" + hdnpcslno.Value + "'");  //'UserName' is ColumnName
                        foreach (DataRow row in rows)
                            temtable23.Rows.Remove(row);

                        temtable23.Merge(dt);
                        Session["TPBwarehousedetailstempDelete"] = temtable23;
                    }
                    else
                    {
                        Session["TPBwarehousedetailstempDelete"] = dt;
                    }


                }

                //deleteALL(stockid);

            }
            #endregion

            return 1;


        }
        protected void GrdWarehousePC_DataBinding(object sender, EventArgs e)
        {
            if (Session["TPOCWarehouseData"] != null)
            {
                DataTable Warehousedt = (DataTable)Session["TPOCWarehouseData"];

                GrdWarehousePC.DataSource = Warehousedt;
            }
        }

        protected void CmbWarehouse_Callback(object sender, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];

            if (WhichCall == "BindWarehouse")
            {
                DataTable dt = GetWarehouseData();
                GetAvailableStock();

                CmbWarehouse.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CmbWarehouse.Items.Add(Convert.ToString(dt.Rows[i]["WarehouseName"]), Convert.ToString(dt.Rows[i]["WarehouseID"]));
                }
                if (hdndefaultID != null || hdndefaultID.Value != "")
                    CmbWarehouse.Value = hdndefaultID.Value;
            }

        }

        public DataTable GetWarehouseData()  // to fill DropDown detail of Warehouse
        {
            DataTable dt = new DataTable();
            dt = objPurchaseInvoice.PopulateWarehouseByBranchList(Convert.ToString(ddl_Branch.SelectedValue));
            //dt = oDBEngine.GetDataTable("select  b.bui_id as WarehouseID,b.bui_Name as WarehouseName from tbl_master_building where  b order by b.bui_Name");
            return dt;
        }

        public void GetAvailableStock()
        {
            //DataTable dt2 = oDBEngine.GetDataTable("select ISnull(ISNULL(tblwarehous.StockBranchWarehouse_StockIn,0)- isnull(tblwarehous.StockBranchWarehouse_StockOut,0),0) as branchopenstock from Trans_StockBranchWarehouse tblwarehous where tblwarehous.StockBranchWarehouse_StockId=" + hdfstockidPC.Value + " and tblwarehous.StockBranchWarehouse_CompanyId='" + Convert.ToString(Session["LastCompany"]) + "' and tblwarehous.StockBranchWarehouse_FinYear='" + Convert.ToString(Session["LastFinYear"]) + "' and tblwarehous.StockBranchWarehouse_BranchId=" + hdbranchIDPC.Value + "");
            try
            {
                DataTable dt2 = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableStockOpening(" + hdbranchIDPC.Value + ",'" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]) + "'," + hdfstockidPC.Value + ") as branchopenstock");

                if (dt2.Rows.Count > 0)
                {
                    CmbWarehouse.JSProperties["cpstock"] = Convert.ToString(dt2.Rows[0]["branchopenstock"]);
                }
                else
                {

                    CmbWarehouse.JSProperties["cpstock"] = "0.0000";
                }
            }
            catch (Exception ex)
            {

            }
        }


        private string getinventry()  // to get Inventory type of Product
        {

            string inventryType = string.Empty;
            if (hdniswarehouse.Value == "true")
            {
                inventryType = "WH";

            }
            if (hdnisbatch.Value == "true")
            {

                inventryType += "BT";
            }
            if (hdnisserial.Value == "true")
            {
                inventryType += "SL";

            }
            return inventryType;
        }

        public string SetVisibilityStock(object container)
        {
            string vs = string.Empty;
            GridViewDataItemTemplateContainer c = container as GridViewDataItemTemplateContainer;
            if (!string.IsNullOrEmpty(Convert.ToString(c.KeyValue)))
            {
                //if (isexist)
                //{
                vs = "display:block";
                //}
                //else
                //{
                //    vs = "display:none";
                //}

            }
            else
            {
                vs = "display:block";
            }

            return vs;


        }

        public void deleteALL()
        {

            #region delete

            if (Session["TPBwarehousedetailstempDelete"] != null)
            {
                string stockid = string.Empty;
                DataTable Warehousedtups = (DataTable)Session["TPBwarehousedetailstempDelete"];
                for (int i = 0; i <= Warehousedtups.Rows.Count - 1; i++)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"])) && Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"]) != "0")
                    {
                        stockid = Convert.ToString(Warehousedtups.Rows[i]["StockID"]);

                        if (Convert.ToString(Warehousedtups.Rows[i]["isnew"]) == "DeleteSL")
                        {
                            string sqls = "delete Trans_StockSerialMapping where PurchaseOrder_MapId=" + Convert.ToString(Warehousedtups.Rows[i]["SerialID"]);

                            oDBEngine.GetDataTable(sqls);
                        }
                        else if (Convert.ToString(Warehousedtups.Rows[i]["isnew"]) == "DeleteWHBTSL")
                        {

                            string sqls = "delete from tbl_trans_PurchaseOrderWarehouseDetails where PurchaseOrderWarehouseDetail_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]) + " " +
                                          " delete from tbl_trans_PurchaseOrderWarehouse where PurchaseOrderWarehouse_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"]) + " " +
                                          " delete from tbl_trans_PurchaseOrderBatch where PurchaseOrderBatch_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]) + " " +
                                          " delete from tbl_trans_PurchaseOrderSerialMapping where PurchaseOrder_BatchId=" + Convert.ToString(Warehousedtups.Rows[i]["BatchID"]);


                            oDBEngine.GetDataTable(sqls);


                        }
                        else if (Convert.ToString(Warehousedtups.Rows[i]["isnew"]) == "DeleteWHSL")
                        {

                            string sqls = "delete from tbl_trans_PurchaseOrderWarehouseDetails where PurchaseOrderWarehouseDetail_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]) + " " +
                                          " delete from Trans_StockBranchWarehouse where PurchaseOrderWarehouse_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"]) + " " +
                                          " delete from tbl_trans_PurchaseOrderBatch where PurchaseOrderBatch_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]) + " " +
                                          " delete from tbl_trans_PurchaseOrderSerialMapping where PurchaseOrder_BatchId=" + Convert.ToString(Warehousedtups.Rows[i]["BatchID"]);


                        }
                        else if (Convert.ToString(Warehousedtups.Rows[i]["isnew"]) == "DeleteWHBT")
                        {
                            string sqls = string.Empty;
                            var updateqnty = Warehousedtups.Compute("sum(Quantitysum)", "isnew = 'DeleteWHBT' AND BatchWarehouseID='" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"]) + "'");

                            if (string.IsNullOrEmpty(Convert.ToString(updateqnty)))
                            { updateqnty = 0.0; }


                            if (stockid != "0")
                            {
                                DataSet dsEmail = new DataSet();

                              //  String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];

                                String conn = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

                                SqlConnection con = new SqlConnection(conn);
                                SqlCommand cmd3 = new SqlCommand("prc_POwarehousbatchDeleteWHBT", con);
                                cmd3.CommandType = CommandType.StoredProcedure;

                                cmd3.Parameters.AddWithValue("@compnay", Convert.ToString(Session["LastCompany"]));
                                cmd3.Parameters.AddWithValue("@Finyear", Convert.ToString(Session["LastFinYear"]));
                                cmd3.Parameters.AddWithValue("@StockID", Convert.ToInt32(stockid));

                                cmd3.Parameters.AddWithValue("@BatchWarehouseID", Convert.ToInt32(Warehousedtups.Rows[i]["BatchWarehouseID"]));
                                cmd3.Parameters.AddWithValue("@BatchWarehousedetailsID", Convert.ToInt32(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]));
                                cmd3.Parameters.AddWithValue("@BatchID", Convert.ToInt32(Warehousedtups.Rows[i]["BatchID"]));


                                //cmd3.Parameters.AddWithValue("@branchID", Convert.ToInt32(hdnselectedbranch.Value));
                                cmd3.Parameters.AddWithValue("@modifiedBy", Convert.ToInt32(HttpContext.Current.Session["userid"]));
                                cmd3.Parameters.AddWithValue("@rate", Convert.ToDecimal(hdnrate.Value));
                                cmd3.Parameters.AddWithValue("@updateqnty", Convert.ToDecimal(updateqnty));

                                SqlParameter output = new SqlParameter("@ReturnValue", SqlDbType.Int);
                                output.Direction = ParameterDirection.Output;
                                cmd3.Parameters.Add(output);
                                cmd3.CommandTimeout = 0;
                                SqlDataAdapter Adap = new SqlDataAdapter();
                                Adap.SelectCommand = cmd3;
                                Adap.Fill(dsEmail);
                                dsEmail.Clear();
                                cmd3.Dispose();
                                con.Dispose();
                                GC.Collect();
                            }
                        }
                        else if (Convert.ToString(Warehousedtups.Rows[i]["isnew"]) == "DeleteWH")
                        {



                            string sqls = "delete from tbl_trans_PurchaseOrderWarehouseDetails where PurchaseOrderWarehouseDetail_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]) + " " +
                                          " delete from tbl_trans_PurchaseOrderWarehouse where PurchaseOrderWarehouse_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"]) + " " +
                                          " delete from tbl_trans_PurchaseOrderBatch where PurchaseOrderBatch_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]);

                            oDBEngine.GetDataTable(sqls);

                        }
                    }


                }


            }
            #endregion

        }


        private int Insertwaredataalldata(string ProductID, string stockid, string branchid)
        {
            DataSet dsEmail = new DataSet();
            string inventryType = string.Empty;
            string IsserialActive = "false";

            int newrowcount = 0;

            #region Insert
           // String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];
            String conn = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);


            SqlConnection con = new SqlConnection(conn);
            SqlCommand cmd3 = new SqlCommand("prc_StockPChallanwarehousentry", con);
            cmd3.CommandType = CommandType.StoredProcedure;

            cmd3.Parameters.AddWithValue("@compnay", Convert.ToString(Session["LastCompany"]));
            cmd3.Parameters.AddWithValue("@Finyear", Convert.ToString(Session["LastFinYear"]));
            cmd3.Parameters.AddWithValue("@StockID", Convert.ToInt32(stockid));
            cmd3.Parameters.AddWithValue("@ProductID", Convert.ToInt32(ProductID));
            cmd3.Parameters.AddWithValue("@branchID", Convert.ToInt32(branchid));
            cmd3.Parameters.AddWithValue("@createdBy", Convert.ToInt32(HttpContext.Current.Session["userid"]));
            cmd3.Parameters.AddWithValue("@PCNumber", Convert.ToInt32(HttpContext.Current.Session["userid"]));
            cmd3.Parameters.AddWithValue("@totalqntity", Convert.ToString(hdntotalqntyPC.Value));


            if (hdniswarehouse.Value == "true")
            {
                inventryType = "WH";

            }
            if (hdnisbatch.Value == "true")
            {

                inventryType += "BT";
            }
            if (hdnisserial.Value == "true")
            {
                inventryType += "SL";
                IsserialActive = "true";
            }

            cmd3.Parameters.AddWithValue("@IsSerialActive", IsserialActive);


            cmd3.Parameters.AddWithValue("@Inventrytype", inventryType);

            DataTable temtable = new DataTable();
            DataTable temtable2 = new DataTable();
            DataTable temtable23 = new DataTable();
            if (Session["TPOCWarehouseData"] != null)
            {
                DataTable Warehousedt = (DataTable)Session["TPOCWarehouseData"];

                temtable = Warehousedt.DefaultView.ToTable(false, "SrlNo", "BatchWarehouseID", "BatchWarehousedetailsID", "BatchID", "SerialID", "WarehouseID", "WarehouseName", "BatchNo", "SerialNo", "MFGDate", "ExpiryDate", "Quantitysum", "productid", "Inventrytype", "StockID", "isnew");
                cmd3.Parameters.AddWithValue("@udt_StockOpeningwarehousentried", temtable);
            }
            if (Session["TPOWarehouseDataDelete"] != null)
            {
                DataTable Warehousedts = (DataTable)Session["TPOWarehouseDataDelete"];

                temtable2 = Warehousedts.DefaultView.ToTable(false, "SrlNo", "BatchWarehouseID", "BatchWarehousedetailsID", "BatchID", "SerialID", "WarehouseID", "WarehouseName", "BatchNo", "SerialNo", "MFGDate", "ExpiryDate", "Quantity", "isnew");
                cmd3.Parameters.AddWithValue("@udt_StockOpeningwarehousentrieDelete", null);
            }

            #endregion

            return 1;


        }
        private int updatewarehouse()
        {
            if (Session["TPBwarehousedetailstempUpdate"] != null)
            {
                DataTable dt = new DataTable();
                DataTable Warehousedtups = (DataTable)Session["TPBwarehousedetailstempUpdate"];
                DataRow[] dr = Warehousedtups.Select("isnew = 'Updated'");
                if (dr.Count() != 0)
                {
                    dt = dr.CopyToDataTable();


                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        string iswarehouse = string.Empty;
                        string isbatch = string.Empty;
                        string isserial = string.Empty;



                        #region WHBTSL
                        if (Convert.ToString(dt.Rows[i]["Inventrytype"]) == "WHBTSL")
                        {
                            DateTime? mfgex;
                            DateTime? exdate;
                            if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewMFGDate"])) && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewExpiryDate"])))
                            {
                                if ((Convert.ToString(dt.Rows[i]["viewMFGDate"]) != "1/1/0001 12:00:00 AM" && Convert.ToString(dt.Rows[i]["viewExpiryDate"]) != "1/1/0001 12:00:00 AM"))
                                {
                                    mfgex = Convert.ToDateTime(dt.Rows[i]["viewMFGDate"]);
                                    exdate = Convert.ToDateTime(dt.Rows[i]["viewExpiryDate"]);
                                }
                                else
                                {
                                    mfgex = null;
                                    exdate = null;
                                }

                            }
                            else
                            {
                                mfgex = null;
                                exdate = null;
                            }

                            if (Convert.ToString(dt.Rows[i]["viewQuantity"]) != "0" && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewQuantity"])))
                            {
                                string sql = string.Empty;
                                if (mfgex == null && exdate == null)
                                {
                                    if (hdnisolddeleted.Value == "true")
                                    {

                                        var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated' AND WarehouseID='" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + "' AND BatchNo='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'");
                                        if (string.IsNullOrEmpty(Convert.ToString(updateqnty)))
                                        {
                                            updateqnty = Convert.ToDecimal(dt.Rows[i]["Quantitysum"]);
                                        }

                                        sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",PurchaseOrderWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +

                                                             "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                           "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +


                                                           "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                           "PurchaseOrderBatch_ModifiedDate=GETDATE() " +
                                                      "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                      "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                                      " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                                      " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
                                    }
                                    else
                                    {
                                        sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",PurchaseOrderWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +

                                                               "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                             "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +


                                                             "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                             "PurchaseOrderBatch_ModifiedDate=GETDATE() " +
                                                        "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                        "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                                        " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                                        " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
                                    }
                                }
                                else
                                {
                                    if (hdnisolddeleted.Value == "true")
                                    {

                                        var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated' AND WarehouseID='" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + "' AND BatchNo='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'");
                                        if (string.IsNullOrEmpty(Convert.ToString(updateqnty)))
                                        {
                                            updateqnty = Convert.ToDecimal(dt.Rows[i]["Quantitysum"]);
                                        }
                                        sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",PurchaseOrderWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +

                                                                 "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                               "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +

                                                               "StockBranchBatch_MfgDate='" + mfgex + "'," +
                                                               "StockBranchBatch_ExpiryDate='" + exdate + "'," +
                                                               "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                               "PurchaseOrderBatch_ModifiedDate=GETDATE() " +
                                                          "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                          "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                                          " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                                          " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
                                    }
                                    else
                                    {
                                        sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",PurchaseOrderWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +

                                                            "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                          "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +

                                                          "StockBranchBatch_MfgDate='" + mfgex + "'," +
                                                          "StockBranchBatch_ExpiryDate='" + exdate + "'," +
                                                          "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                          "PurchaseOrderBatch_ModifiedDate=GETDATE() " +
                                                          "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                          "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                                          " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                                          " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
                                    }


                                }


                                oDBEngine.GetDataTable(sql);

                            }
                            else
                            {
                                //var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated' AND WarehouseID='" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + "' AND BatchNo='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'");


                                string sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +


                                                  "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                        " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                        " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";

                                oDBEngine.GetDataTable(sql);
                            }
                        }
                        #endregion
                        #region WHBT
                        if (Convert.ToString(dt.Rows[i]["Inventrytype"]) == "WHBT")
                        {
                            DateTime? mfgex;
                            DateTime? exdate;

                            var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated' AND BatchWarehouseID='" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "'");
                            var OLDSAMEwarehouse = Warehousedtups.Compute("sum(Quantitysum)", "isnew = 'old' AND BatchWarehouseID='" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "'");
                            if (!string.IsNullOrEmpty(Convert.ToString(OLDSAMEwarehouse)) && !string.IsNullOrEmpty(Convert.ToString(updateqnty)))
                            {

                                updateqnty = Convert.ToDecimal(updateqnty) + Convert.ToDecimal(OLDSAMEwarehouse);
                            }

                            if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewMFGDate"])) && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewExpiryDate"])))
                            {
                                if ((Convert.ToString(dt.Rows[i]["viewMFGDate"]) != "1/1/0001 12:00:00 AM" && Convert.ToString(dt.Rows[i]["viewExpiryDate"]) != "1/1/0001 12:00:00 AM"))
                                {
                                    mfgex = Convert.ToDateTime(dt.Rows[i]["viewMFGDate"]);
                                    exdate = Convert.ToDateTime(dt.Rows[i]["viewExpiryDate"]);
                                }
                                else
                                {
                                    mfgex = null;
                                    exdate = null;
                                }

                            }
                            else
                            {
                                mfgex = null;
                                exdate = null;
                            }

                            if (Convert.ToString(dt.Rows[i]["viewQuantity"]) != "0" && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewQuantity"])) && !string.IsNullOrEmpty(Convert.ToString(updateqnty)))
                            {
                                string sql = string.Empty;
                                if (mfgex == null && exdate == null)
                                {
                                    sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",PurchaseOrderWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +

                                                         "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                       "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +


                                                       "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                       "PurchaseOrderBatch_ModifiedDate=GETDATE() " +
                                                  "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]);


                                }
                                else
                                {
                                    sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",PurchaseOrderWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +

                                                             "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                           "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +

                                                           "StockBranchBatch_MfgDate='" + mfgex + "'," +
                                                           "StockBranchBatch_ExpiryDate='" + exdate + "'," +
                                                           "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                           "PurchaseOrderBatch_ModifiedDate=GETDATE() " +
                                                      "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]);
                                }


                                oDBEngine.GetDataTable(sql);

                            }


                        }
                        #endregion
                        #region WHSL
                        if (Convert.ToString(dt.Rows[i]["Inventrytype"]) == "WHSL")
                        {
                            DateTime? mfgex;
                            DateTime? exdate;
                            if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewMFGDate"])) && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewExpiryDate"])))
                            {
                                if ((Convert.ToString(dt.Rows[i]["viewMFGDate"]) != "1/1/0001 12:00:00 AM" && Convert.ToString(dt.Rows[i]["viewExpiryDate"]) != "1/1/0001 12:00:00 AM"))
                                {
                                    mfgex = Convert.ToDateTime(dt.Rows[i]["viewMFGDate"]);
                                    exdate = Convert.ToDateTime(dt.Rows[i]["viewExpiryDate"]);
                                }
                                else
                                {
                                    mfgex = null;
                                    exdate = null;
                                }

                            }
                            else
                            {
                                mfgex = null;
                                exdate = null;
                            }

                            if (Convert.ToString(dt.Rows[i]["viewQuantity"]) != "0" && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewQuantity"])))
                            {
                                string sql = string.Empty;
                                if (mfgex == null && exdate == null)
                                {
                                    sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",PurchaseOrderWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                              "  update tbl_trans_PurchaseOrderWarehouseDetails set Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
                                                         "Stock_OpeningValue=ISNULL(isnull(" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",0)*isnull(" + Convert.ToDecimal(hdnrate.Value) + ",0),0)," +
                                                         "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                         "ModifiedDate=GETDATE()" +
                                                         "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +
                                                         "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                       "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +


                                                       "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                       "PurchaseOrderBatch_ModifiedDate=GETDATE()" +
                                                  "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                  "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                        " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                        " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";

                                }
                                else
                                {
                                    sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",PurchaseOrderWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                                  "  update tbl_trans_PurchaseOrderWarehouseDetails set Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
                                                             "Stock_OpeningValue=ISNULL(isnull(" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",0)*isnull(" + Convert.ToDecimal(hdnrate.Value) + ",0),0)," +
                                                             "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                             "ModifiedDate=GETDATE()" +
                                                             "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +
                                                             "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                           "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +

                                                           "StockBranchBatch_MfgDate='" + mfgex + "'," +
                                                           "StockBranchBatch_ExpiryDate='" + exdate + "'," +
                                                           "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                           "PurchaseOrderBatch_ModifiedDate=GETDATE()" +
                                                      "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                      "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                            " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                            " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
                                }


                                oDBEngine.GetDataTable(sql);

                            }
                            else
                            {
                                //var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated' AND WarehouseID='" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + "' AND BatchNo='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'");


                                string sql = "update tbl_trans_PurchaseOrderWarehouse set StockBranchWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                              "  update tbl_trans_PurchaseOrderWarehouseDetails set " +
                                                         "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                         "ModifiedDate=GETDATE()" +
                                                         "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +

                                                  "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                        " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                        " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";

                                oDBEngine.GetDataTable(sql);
                            }
                        }
                        #endregion

                        #region WH
                        if (Convert.ToString(dt.Rows[i]["Inventrytype"]) == "WH")
                        {
                            if (Convert.ToString(dt.Rows[i]["viewQuantity"]) != "0" && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Quantity"])))
                            {

                                string sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantity"]) + ",PurchaseOrderWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +

                                                       "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                     "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantity"]) + "," +


                                                     "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                     "PurchaseOrderBatch_ModifiedDate=GETDATE() " +
                                                " where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]);




                                oDBEngine.GetDataTable(sql);

                            }


                        }
                        #endregion

                        #region BTSL
                        if (Convert.ToString(dt.Rows[i]["Inventrytype"]) == "BTSL")
                        {
                            DateTime? mfgex;
                            DateTime? exdate;
                            if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewMFGDate"])) && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewExpiryDate"])))
                            {
                                if ((Convert.ToString(dt.Rows[i]["viewMFGDate"]) != "1/1/0001 12:00:00 AM" && Convert.ToString(dt.Rows[i]["viewExpiryDate"]) != "1/1/0001 12:00:00 AM"))
                                {
                                    mfgex = Convert.ToDateTime(dt.Rows[i]["viewMFGDate"]);
                                    exdate = Convert.ToDateTime(dt.Rows[i]["viewExpiryDate"]);
                                }
                                else
                                {
                                    mfgex = null;
                                    exdate = null;
                                }

                            }
                            else
                            {
                                mfgex = null;
                                exdate = null;
                            }

                            if (Convert.ToString(dt.Rows[i]["viewQuantity"]) != "0" && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewQuantity"])))
                            {
                                string sql = string.Empty;
                                if (mfgex == null && exdate == null)
                                {
                                    if (hdnisolddeleted.Value == "true")
                                    {

                                        var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated'  AND BatchNo='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'");
                                        if (string.IsNullOrEmpty(Convert.ToString(updateqnty)))
                                        {
                                            updateqnty = Convert.ToDecimal(dt.Rows[i]["Quantitysum"]);
                                        }

                                        sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +

                                                             "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                           "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +


                                                           "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                           "PurchaseOrderBatch_ModifiedDate=GETDATE() " +
                                                      "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                      "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                            " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                            " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
                                    }
                                    else
                                    {
                                        sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +

                                                               "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                             "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +


                                                             "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                             "PurchaseOrderBatch_ModifiedDate=GETDATE() " +
                                                        "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                        "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                              " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                              " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
                                    }
                                }
                                else
                                {
                                    if (hdnisolddeleted.Value == "true")
                                    {

                                        var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated' AND BatchNo='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'");
                                        if (string.IsNullOrEmpty(Convert.ToString(updateqnty)))
                                        {
                                            updateqnty = Convert.ToDecimal(dt.Rows[i]["Quantitysum"]);
                                        }
                                        sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +

                                                                 "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                               "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +

                                                               "StockBranchBatch_MfgDate='" + mfgex + "'," +
                                                               "StockBranchBatch_ExpiryDate='" + exdate + "'," +
                                                               "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                               "PurchaseOrderBatch_ModifiedDate=GETDATE() " +
                                                          "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                          "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                                " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                                " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
                                    }
                                    else
                                    {
                                        sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +

                                                            "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                          "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +

                                                          "StockBranchBatch_MfgDate='" + mfgex + "'," +
                                                          "StockBranchBatch_ExpiryDate='" + exdate + "'," +
                                                          "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                          "PurchaseOrderBatch_ModifiedDate=GETDATE() " +
                                                     "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
                                                     "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                           " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                           " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
                                    }


                                }


                                oDBEngine.GetDataTable(sql);

                            }
                            else
                            {
                                //var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated' AND WarehouseID='" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + "' AND BatchNo='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'");


                                string sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
                                              "  update tbl_trans_PurchaseOrderWarehouseDetails set " +
                                                         "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                         "ModifiedDate=GETDATE() " +
                                                         "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +

                                                  "  update  tbl_trans_PurchaseOrderSerialMapping" +
                                        " Set PurchaseOrder_SerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
                                        " where PurchaseOrder_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";

                                oDBEngine.GetDataTable(sql);
                            }
                        }
                        #endregion

                        #region BT
                        if (Convert.ToString(dt.Rows[i]["Inventrytype"]) == "BTSL")
                        {
                            DateTime? mfgex;
                            DateTime? exdate;

                            var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated' AND BatchWarehouseID='" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "'");
                            var OLDSAMEwarehouse = Warehousedtups.Compute("sum(Quantitysum)", "isnew = 'old' AND BatchWarehouseID='" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "'");
                            if (!string.IsNullOrEmpty(Convert.ToString(OLDSAMEwarehouse)) && !string.IsNullOrEmpty(Convert.ToString(updateqnty)))
                            {

                                updateqnty = Convert.ToDecimal(updateqnty) + Convert.ToDecimal(OLDSAMEwarehouse);
                            }

                            if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewMFGDate"])) && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewExpiryDate"])))
                            {
                                if ((Convert.ToString(dt.Rows[i]["viewMFGDate"]) != "1/1/0001 12:00:00 AM" && Convert.ToString(dt.Rows[i]["viewExpiryDate"]) != "1/1/0001 12:00:00 AM"))
                                {
                                    mfgex = Convert.ToDateTime(dt.Rows[i]["viewMFGDate"]);
                                    exdate = Convert.ToDateTime(dt.Rows[i]["viewExpiryDate"]);
                                }
                                else
                                {
                                    mfgex = null;
                                    exdate = null;
                                }

                            }
                            else
                            {
                                mfgex = null;
                                exdate = null;
                            }

                            if (Convert.ToString(dt.Rows[i]["viewQuantity"]) != "0" && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewQuantity"])) && !string.IsNullOrEmpty(Convert.ToString(updateqnty)))
                            {
                                string sql = string.Empty;
                                if (mfgex == null && exdate == null)
                                {
                                    sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",PurchaseOrderWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +

                                                         "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                       "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +


                                                       "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                       "PurchaseOrderBatch_ModifiedDate=GETDATE() " +
                                                  "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]);


                                }
                                else
                                {
                                    sql = "update tbl_trans_PurchaseOrderWarehouse set PurchaseOrderWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",PurchaseOrderWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where PurchaseOrderWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +

                                                             "   update tbl_trans_PurchaseOrderBatch set   PurchaseOrderBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
                                                           "PurchaseOrderBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +

                                                           "StockBranchBatch_MfgDate='" + mfgex + "'," +
                                                           "StockBranchBatch_ExpiryDate='" + exdate + "'," +
                                                           "PurchaseOrderBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
                                                           "PurchaseOrderBatch_ModifiedDate=GETDATE() " +
                                                      "where PurchaseOrderBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]);
                                }


                                oDBEngine.GetDataTable(sql);

                            }


                        }
                        #endregion

                    }
                }
            }

            return 0;
        }

        #endregion

        #region Taxpopup
        public void setValueForHeaderGST(ASPxComboBox aspxcmb, string taxId)
        {
            for (int i = 0; i < aspxcmb.Items.Count; i++)
            {
                if (Convert.ToString(aspxcmb.Items[i].Value).Split('~')[0] == taxId.Split('~')[0])
                {
                    aspxcmb.Items[i].Selected = true;
                    break;
                }
            }

        }

        protected DataTable GetTaxDataWithGST(DataTable existing)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
            proc.AddVarcharPara("@Action", 500, "TaxDetailsForGst");
            proc.AddIntegerPara("@PurchaseInvoiceId", Convert.ToInt32(Session["TPurchaseInvoice_Id"]));
            dt = proc.GetTable();
            if (dt.Rows.Count > 0)
            {
                DataRow gstRow = existing.NewRow();
                gstRow["Taxes_ID"] = 0;
                gstRow["Taxes_Name"] = dt.Rows[0]["TaxRatesSchemeName"];
                gstRow["Percentage"] = dt.Rows[0]["OrderTax_Percentage"];
                gstRow["Amount"] = dt.Rows[0]["OrderTax_Amount"];
                gstRow["AltTax_Code"] = dt.Rows[0]["Gst"];

                UpdateGstForCharges(Convert.ToString(dt.Rows[0]["Gst"]));
                // txtGstCstVatCharge.Value = gstRow["Amount"];
                existing.Rows.Add(gstRow);
            }
            SetTotalCharges(existing);
            return existing;
        }

        public void SetTotalCharges(DataTable taxTableFinal)
        {
            decimal totalCharges = 0;
            foreach (DataRow dr in taxTableFinal.Rows)
            {
                if (Convert.ToString(dr["Taxes_ID"]) != "0")
                {
                    if (Convert.ToString(dr["Taxes_Name"]).Contains("(+)"))
                    {
                        totalCharges += Convert.ToDecimal(dr["Amount"]);
                    }
                    else
                    {
                        totalCharges -= Convert.ToDecimal(dr["Amount"]);
                    }
                }
                else
                {//Else part For Gst 
                    totalCharges += Convert.ToDecimal(dr["Amount"]);
                }
            }
            txtQuoteTaxTotalAmt.Value = totalCharges;

        }

        protected void UpdateGstForCharges(string data)
        {
            //for (int i = 0; i < cmbGstCstVatcharge.Items.Count; i++)
            //{
            //    if (Convert.ToString(cmbGstCstVatcharge.Items[i].Value).Split('~')[0] == data)
            //    {
            //        cmbGstCstVatcharge.Items[i].Selected = true;
            //        break;
            //    }
            //}
        }
        protected void taxUpdatePanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

            string performpara = e.Parameter;
            if (performpara.Split('~')[0] == "DelProdbySl")
            {
                DataTable MainTaxDataTable = (DataTable)Session["TPurchaseOrderFinalTaxRecord"];

                DataRow[] deletedRow = MainTaxDataTable.Select("SlNo=" + performpara.Split('~')[1]);
                if (deletedRow.Length > 0)
                {
                    foreach (DataRow dr in deletedRow)
                    {
                        MainTaxDataTable.Rows.Remove(dr);
                    }

                }

                Session["TPurchaseOrderFinalTaxRecord"] = MainTaxDataTable;
                GetStock(Convert.ToString(performpara.Split('~')[2]));
                // DeleteWarehouse(Convert.ToString(performpara.Split('~')[1]));
                DataTable taxDetails = (DataTable)Session["TPBTaxDetails"];
                if (taxDetails != null)
                {
                    foreach (DataRow dr in taxDetails.Rows)
                    {
                        dr["Amount"] = "0.00";
                    }
                    Session["TPBTaxDetails"] = taxDetails;
                }
            }
            else if (performpara.Split('~')[0] == "DeleteAllTax")
            {
                CreateDataTaxTable();

                DataTable taxDetails = (DataTable)Session["TPBTaxDetails"];

                if (taxDetails != null)
                {
                    foreach (DataRow dr in taxDetails.Rows)
                    {
                        dr["Amount"] = "0.00";
                    }
                    Session["TPBTaxDetails"] = taxDetails;
                }
            }
            else
            {
                DataTable MainTaxDataTable = (DataTable)Session["TPurchaseOrderFinalTaxRecord"];

                DataRow[] deletedRow = MainTaxDataTable.Select("SlNo=" + performpara.Split('~')[1]);
                if (deletedRow.Length > 0)
                {
                    foreach (DataRow dr in deletedRow)
                    {
                        MainTaxDataTable.Rows.Remove(dr);
                    }

                }

                Session["TPurchaseOrderFinalTaxRecord"] = MainTaxDataTable;

                DataTable taxDetails = (DataTable)Session["TPBTaxDetails"];

                if (taxDetails != null)
                {
                    foreach (DataRow dr in taxDetails.Rows)
                    {
                        dr["Amount"] = "0.00";
                    }
                    Session["TPBTaxDetails"] = taxDetails;
                }
            }
        }
        public void GetStock(string strProductID)
        {
            string strBranch = Convert.ToString(ddl_Branch.SelectedValue);
            acpAvailableStock.JSProperties["cpstock"] = "0.00";

            try
            {
                DataTable dt2 = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableQuotation(" + strBranch + ",'" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]) + "'," + strProductID + ") as branchopenstock");

                if (dt2.Rows.Count > 0)
                {
                    taxUpdatePanel.JSProperties["cpstock"] = Convert.ToString(Math.Round(Convert.ToDecimal(dt2.Rows[0]["branchopenstock"]), 2));
                }
                else
                {
                    taxUpdatePanel.JSProperties["cpstock"] = "0.00";
                }
            }
            catch (Exception ex)
            {
            }
        }
        public void DeleteWarehouse(string SrlNo)
        {
            DataTable Warehousedt = new DataTable();
            //if (Session["WarehouseData"] != null)
            if (Session["TPBwarehousedetailstemp"] != null)
            {
                Warehousedt = (DataTable)Session["TPBwarehousedetailstemp"];
                //Warehousedt = (DataTable)Session["WarehouseData"];

                var rows = Warehousedt.Select(string.Format("pcslno ='{0}'", SrlNo));
                //var rows = Warehousedt.Select(string.Format("Product_SrlNo ='{0}'", SrlNo));
                foreach (var row in rows)
                {
                    row.Delete();
                }
                Warehousedt.AcceptChanges();
                #region Code Added by Sam on 09062017  To arrange Warehouse Data according to serial no Start
                int j = 0;
                foreach (DataRow dr in Warehousedt.Rows)
                {
                    string slno = Convert.ToString(dr["SrlNo"]);
                    if (slno == "1")
                    {
                        j++;
                        dr["pcslno"] = j.ToString();


                    }
                    else
                    {
                        dr["pcslno"] = j.ToString();
                    }
                    //Warehousedt.AcceptChanges();
                    //dr["SrlNo"] = j.ToString();

                    //if (Status != "D")
                    //{
                    //    if (Status == "I")
                    //    {
                    //        string strID = Convert.ToString("Q~" + j);
                    //        dr["PurchaseInvoiceDetailID"] = strID;
                    //    }
                    //    j++;
                    //}
                }
                #endregion Code Added by Sam on 09062017  To arrange Warehouse Data according to serial no Start


                Warehousedt.AcceptChanges();

                //Session["WarehouseData"] = Warehousedt;
                Session["TPBwarehousedetailstemp"] = Warehousedt;
            }
        }
        public double ReCalculateTaxAmount(string slno, double amount)
        {
            DataTable MainTaxDataTable = (DataTable)Session["TPurchaseOrderFinalTaxRecord"];
            double totalSum = 0.0;
            //Get The Existing datatable
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "PopulateAllTax");
            DataTable TaxDt = proc.GetTable();

            DataRow[] filterRow = MainTaxDataTable.Select("SlNo=" + slno);

            if (filterRow.Length > 0)
            {
                foreach (DataRow dr in filterRow)
                {
                    if (Convert.ToString(dr["TaxCode"]) != "0")
                    {
                        DataRow[] taxrow = TaxDt.Select("Taxes_ID=" + dr["TaxCode"]);
                        if (taxrow.Length > 0)
                        {
                            if (taxrow[0]["TaxCalculateMethods"] == "A")
                            {
                                dr["Amount"] = (amount * Convert.ToDouble(dr["Percentage"])) / 100;
                                totalSum += (amount * Convert.ToDouble(dr["Percentage"])) / 100;
                            }
                            else
                            {
                                dr["Amount"] = (amount * Convert.ToDouble(dr["Percentage"])) / 100;
                                totalSum -= (amount * Convert.ToDouble(dr["Percentage"])) / 100;
                            }
                        }
                    }
                    else
                    {
                        DataRow[] taxrow = TaxDt.Select("Taxes_ID=" + dr["AltTaxCode"]);
                        if (taxrow.Length > 0)
                        {
                            if (taxrow[0]["TaxCalculateMethods"] == "A")
                            {
                                dr["Amount"] = (amount * Convert.ToDouble(dr["Percentage"])) / 100;
                                totalSum += (amount * Convert.ToDouble(dr["Percentage"])) / 100;
                            }
                            else
                            {
                                dr["Amount"] = (amount * Convert.ToDouble(dr["Percentage"])) / 100;
                                totalSum -= (amount * Convert.ToDouble(dr["Percentage"])) / 100;
                            }
                        }
                    }
                }

            }
            Session["TPurchaseOrderFinalTaxRecord"] = MainTaxDataTable;

            return totalSum;

        }

        public void PopulateGSTCSTVATCombo(string quoteDate)
        {
            string LastCompany = "";
            if (Convert.ToString(Session["LastCompany"]) != null)
            {
                LastCompany = Convert.ToString(Session["LastCompany"]);
            }
            //DataTable dt = new DataTable();
            //dt = objCRMSalesDtlBL.PopulateGSTCSTVATCombo();
            //DataTable DT = oDBEngine.GetDataTable("select cast(td.TaxRates_ID as varchar(5))+'~'+ cast (td.TaxRates_Rate as varchar(25)) 'Taxes_ID',td.TaxRatesSchemeName 'Taxes_Name',th.Taxes_Name as 'TaxCodeName' from Master_Taxes th inner join Config_TaxRates td on th.Taxes_ID=td.TaxRates_TaxCode where (td.TaxRates_Country=0 or td.TaxRates_Country=(select add_country from tbl_master_address where add_cntId ='" + Convert.ToString(Session["LastCompany"]) + "' ))  and th.Taxes_ApplicableFor in ('B','S') and th.TaxTypeCode in('G','V','C')");

            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "LoadGSTCSTVATCombo");
            proc.AddVarcharPara("@S_QuoteAdd_CompanyID", 10, Convert.ToString(LastCompany));
            proc.AddVarcharPara("@S_quoteDate", 10, quoteDate);
            DataTable DT = proc.GetTable();
            cmbGstCstVat.DataSource = DT;
            cmbGstCstVat.TextField = "Taxes_Name";
            cmbGstCstVat.ValueField = "Taxes_ID";
            cmbGstCstVat.DataBind();
        }
        public void CreateDataTaxTable()
        {
            DataTable TaxRecord = new DataTable();

            TaxRecord.Columns.Add("SlNo", typeof(System.Int32));
            TaxRecord.Columns.Add("TaxCode", typeof(System.String));
            TaxRecord.Columns.Add("AltTaxCode", typeof(System.String));
            TaxRecord.Columns.Add("Percentage", typeof(System.Decimal));
            TaxRecord.Columns.Add("Amount", typeof(System.Decimal));
            Session["TPurchaseOrderFinalTaxRecord"] = TaxRecord;
        }
        public static  void CreateDataTaxTableByAjax()
        {
            DataTable TaxRecord = new DataTable();

            TaxRecord.Columns.Add("SlNo", typeof(System.Int32));
            TaxRecord.Columns.Add("TaxCode", typeof(System.String));
            TaxRecord.Columns.Add("AltTaxCode", typeof(System.String));
            TaxRecord.Columns.Add("Percentage", typeof(System.Decimal));
            TaxRecord.Columns.Add("Amount", typeof(System.Decimal));
            HttpContext.Current.Session["TPurchaseOrderFinalTaxRecord"] = TaxRecord;
        }

        public DataTable CreateReverseTaxTable()
        {
            DataTable TaxRecord = new DataTable();

            TaxRecord.Columns.Add("SlNo", typeof(System.Int32));
            TaxRecord.Columns.Add("TaxCode", typeof(System.String));
            TaxRecord.Columns.Add("AltTaxCode", typeof(System.String));
            TaxRecord.Columns.Add("Percentage", typeof(System.Decimal));
            TaxRecord.Columns.Add("Amount", typeof(System.Decimal));

            return TaxRecord;

        }


        //public IEnumerable GetTaxCode()
        //{
        //    List<taxCode> TaxList = new List<taxCode>();
        //    BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //    // DataTable DT = objEngine.GetDataTable("select Taxes_ID,Taxes_Name from dbo.Master_Taxes");
        //    DataTable DT = objEngine.GetDataTable("select cast(th.Taxes_ID as varchar(5))+'~'+ cast (td.TaxRates_Rate as varchar(25)) 'Taxes_ID',th.Taxes_Name 'Taxes_Name' from Master_Taxes th inner join Config_TaxRates td on th.Taxes_ID=td.TaxRates_TaxCode where (td.TaxRates_Country=0 or td.TaxRates_Country=(select add_country from tbl_master_address where add_cntId ='" + Convert.ToString(Session["LastCompany"]) + "' ))  and th.Taxes_ApplicableFor in ('B','S')");


        //    for (int i = 0; i < DT.Rows.Count; i++)
        //    {
        //        taxCode tax = new taxCode();
        //        tax.Taxes_ID = Convert.ToString(DT.Rows[i]["Taxes_ID"]);
        //        tax.Taxes_Name = Convert.ToString(DT.Rows[i]["Taxes_Name"]);
        //        TaxList.Add(tax);
        //    }

        //    return TaxList;
        //}

        public string GetTaxName(int id)
        {
            string taxName = "";
            string[] arr = oDBEngine.GetFieldValue1("Master_taxes", "Taxes_Name", "Taxes_ID=" + Convert.ToString(id), 1);
            if (arr[0] != "n")
            {
                taxName = arr[0];
            }
            return taxName;
        }
        public DataSet GetQuotationTaxData()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "ProductTaxDetails");
            proc.AddVarcharPara("@QuotationID", 500, Convert.ToString(Session["QuotationID"]));
            ds = proc.GetDataSet();
            return ds;
        }
        public DataSet GetQuotationEditedTaxData()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
            proc.AddVarcharPara("@Action", 500, "ProductEditedTaxDetailsForPo");
            proc.AddIntegerPara("@InvoiceID", Convert.ToInt32(Session["TPurchaseInvoice_Id"]));
            ds = proc.GetDataSet();
            return ds;
        }
        public double GetTotalTaxAmount(List<TaxDetails> tax)
        {
            double sum = 0;
            foreach (TaxDetails td in tax)
            {
                if (td.Taxes_Name.Substring(td.Taxes_Name.Length - 3, 3) == "(+)")
                    sum += td.Amount;
                else
                    sum -= td.Amount;

            }
            return sum;
        }
        protected void cgridTax_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string retMsg = "";
            if (e.Parameters.Split('~')[0] == "SaveGST")
            {
                DataTable TaxRecord = (DataTable)Session["TPurchaseOrderFinalTaxRecord"];
                int slNo = Convert.ToInt32(HdSerialNo.Value);
                //For GST/CST/VAT
                if (cmbGstCstVat.Value != null)
                {

                    DataRow[] finalRow = TaxRecord.Select("SlNo=" + Convert.ToString(slNo) + " and TaxCode='0'");
                    if (finalRow.Length > 0)
                    {
                        finalRow[0]["Percentage"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[1];
                        finalRow[0]["Amount"] = txtGstCstVat.Text;
                        finalRow[0]["AltTaxCode"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[0];

                    }
                    else
                    {
                        DataRow newRowGST = TaxRecord.NewRow();
                        newRowGST["slNo"] = slNo;
                        newRowGST["Percentage"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[1];
                        newRowGST["TaxCode"] = "0";
                        newRowGST["AltTaxCode"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[0];
                        newRowGST["Amount"] = txtGstCstVat.Text;
                        TaxRecord.Rows.Add(newRowGST);
                    }
                }
                //End Here

                aspxGridTax.JSProperties["cpUpdated"] = "";

                Session["TPurchaseOrderFinalTaxRecord"] = TaxRecord;
            }
            else
            {
                #region fetch All data For Tax

                DataTable taxDetail = new DataTable();
                DataTable MainTaxDataTable = (DataTable)Session["TPurchaseOrderFinalTaxRecord"];
                // DataTable databaseReturnTable = (DataTable)Session["QuotationTaxDetails"];

                //if (Convert.ToInt32(e.Parameters.Split('~')[1]) == 1)
                //    taxDetail = oDBEngine.GetDataTable("select TaxRates_ID as Taxes_ID,TaxRatesSchemeName as Taxes_Name,TaxCalculateMethods,tm.Taxes_Name as taxCodeName,tm.Taxes_ApplicableOn as 'ApplicableOn',isnull((select TaxRatesSchemeName from Config_TaxRates where TaxRates_ID=tm.Taxes_OtherTax),'')  as dependOn from Master_taxes tm inner join Config_TaxRates ct on tm.Taxes_ID=ct.TaxRates_TaxCode where tm.TaxTypeCode not in('G','V','C') and tm.TaxItemlevel='Y'");
                //else if (Convert.ToInt32(e.Parameters.Split('~')[1]) == 2)
                //taxDetail = oDBEngine.GetDataTable("select TaxRates_ID as Taxes_ID,TaxRatesSchemeName as Taxes_Name,TaxCalculateMethods,tm.Taxes_Name as taxCodeName,tm.Taxes_ApplicableOn as 'ApplicableOn',isnull((select TaxRatesSchemeName from Config_TaxRates where TaxRates_ID=tm.Taxes_OtherTax),'')  as dependOn from Master_taxes tm inner join Config_TaxRates ct on tm.Taxes_ID=ct.TaxRates_TaxCode where tm.TaxTypeCode not in('G','V','C') and tm.TaxItemlevel='Y'");
                //ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
                //proc.AddVarcharPara("@Action", 500, "LoadOtherTaxDetails");
                //proc.AddVarcharPara("@S_quoteDate", 10, dt_PLQuote.Date.ToString("yyyy-MM-dd"));
                //proc.AddVarcharPara("@ProductsID", 10, Convert.ToString(setCurrentProdCode.Value));
                //taxDetail = proc.GetTable();

                ProcedureExecute proc = new ProcedureExecute("prc_TaxExceptionFind");
                proc.AddVarcharPara("@Action", 500, "PO");
                proc.AddVarcharPara("@ProductID", 10, Convert.ToString(setCurrentProdCode.Value));
                proc.AddVarcharPara("@ENTITY_ID", 100, hdnCustomerId.Value);
                proc.AddVarcharPara("@Date", 10, dt_PLQuote.Date.ToString("yyyy-MM-dd"));
                proc.AddVarcharPara("@Amount", 100, HdProdGrossAmt.Value);
                proc.AddVarcharPara("@Qty", 100, hdnQty.Value);
                taxDetail = proc.GetTable();


                //Get Company Gstin 09032017
                string CompInternalId = Convert.ToString(Session["LastCompany"]);
                string[] compGstin = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_gstin", "cmp_internalid='" + CompInternalId + "'", 1);

                //Get BranchStateCode
                string BrancgStateCode = "", BranchGSTIN = "";
                DataTable BranchTable = oDBEngine.GetDataTable("select StateCode,branch_GSTIN   from tbl_master_branch branch inner join tbl_master_state st on branch.branch_state=st.id where branch_id=" + Convert.ToString(ddl_Branch.SelectedValue));
                if (BranchTable != null)
                {
                    BrancgStateCode = Convert.ToString(BranchTable.Rows[0][0]);
                    BranchGSTIN = Convert.ToString(BranchTable.Rows[0][1]);
                    if (BranchGSTIN.Trim() != "")
                    {
                        BrancgStateCode = BranchGSTIN.Substring(0, 2);
                    }
                }

                if (BranchGSTIN.Trim() == "")
                {
                    BrancgStateCode = compGstin[0].Substring(0, 2);
                }



                string VendorState = "";


                ProcedureExecute GetVendorGstin = new ProcedureExecute("prc_GstTaxDetails");
                GetVendorGstin.AddVarcharPara("@Action", 500, "GetVendorGSTINByBranch");
                GetVendorGstin.AddVarcharPara("@branchId", 10, Convert.ToString(ddl_Branch.SelectedValue));
                GetVendorGstin.AddVarcharPara("@entityId", 10, Convert.ToString(hdnCustomerId.Value));
                DataTable VendorGstin = GetVendorGstin.GetTable();

                if (VendorGstin.Rows.Count > 0)
                {
                    if (Convert.ToString(VendorGstin.Rows[0][0]).Trim() != "")
                    {
                        VendorState = Convert.ToString(VendorGstin.Rows[0][0]).Substring(0, 2);
                    }

                }


                #endregion


                if (VendorState.Trim() != "" && BrancgStateCode != "")
                {

                    if (BrancgStateCode == VendorState)
                    {
                        //Check if the state is in union territories then only UTGST will apply
                        //   Chandigarh     Andaman and Nicobar Islands    DADRA & NAGAR HAVELI    DAMAN & DIU    Lakshadweep              PONDICHERRY
                        if (VendorState == "4" || VendorState == "26" || VendorState == "25" || VendorState == "35" || VendorState == "31" || VendorState == "34")
                        {
                            foreach (DataRow dr in taxDetail.Rows)
                            {

                                if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                                {
                                    dr.Delete();
                                }
                            }

                        }
                        else
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
                    }
                    else
                    {
                        foreach (DataRow dr in taxDetail.Rows)
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                            {
                                dr.Delete();
                            }
                        }
                        taxDetail.AcceptChanges();

                    }


                }

                //If Company GSTIN is blank then Delete All CGST,UGST,IGST,CGST
                if ((compGstin[0].Trim() == "" && BranchGSTIN == "") || VendorState == "")
                {
                    foreach (DataRow dr in taxDetail.Rows)
                    {
                        if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST")
                        {
                            dr.Delete();
                        }
                    }
                    taxDetail.AcceptChanges();
                }

                ////Check If any TaxScheme Set Against that Product Then update there rate 22-03-2017 and rate
                //string[] schemeIDViaProdID = oDBEngine.GetFieldValue1("master_sproducts", "isnull(sProduct_TaxSchemeSale,0)sProduct_TaxSchemeSale", "sProducts_ID='" + Convert.ToString(setCurrentProdCode.Value) + "'", 1);
                ////&& schemeIDViaProdID[0] != ""
                //if (schemeIDViaProdID.Length > 0)
                //{

                //    if (taxDetail.Select("Taxes_ID='" + schemeIDViaProdID[0] + "'").Length > 0)
                //    {
                //        foreach (DataRow dr in taxDetail.Rows)
                //        {
                //            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST")
                //            {
                //                if (Convert.ToString(dr["Taxes_ID"]).Trim() != schemeIDViaProdID[0].Trim())
                //                    dr["TaxRates_Rate"] = 0;
                //            }
                //        }
                //    }
                //}



                int slNo = Convert.ToInt32(HdSerialNo.Value);

                //Get Gross Amount and Net Amount 
                decimal ProdGrossAmt = Convert.ToDecimal(HdProdGrossAmt.Value);
                decimal ProdNetAmt = Convert.ToDecimal(HdProdNetAmt.Value);



                List<TaxDetails> TaxDetailsDetails = new List<TaxDetails>();

                //Debjyoti 09032017
                decimal totalParcentage = 0;
                foreach (DataRow dr in taxDetail.Rows)
                {
                    if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                    {
                        totalParcentage += Convert.ToDecimal(dr["TaxRates_Rate"]);
                    }
                }



                if (e.Parameters.Split('~')[0] == "New")
                {
                    foreach (DataRow dr in taxDetail.Rows)
                    {
                        TaxDetails obj = new TaxDetails();
                        obj.Taxes_ID = Convert.ToInt32(dr["Taxes_ID"]);
                        obj.taxCodeName = Convert.ToString(dr["taxCodeName"]);
                        obj.TaxField = Convert.ToString(dr["TaxRates_Rate"]);
                        obj.Amount = 0.0;

                        #region set calculated on
                        //Check Tax Applicable on and set to calculated on
                        if (Convert.ToString(dr["ApplicableOn"]) == "G")
                        {
                            obj.calCulatedOn = ProdGrossAmt;
                        }
                        else if (Convert.ToString(dr["ApplicableOn"]) == "N")
                        {
                            obj.calCulatedOn = ProdNetAmt;
                        }
                        else
                        {
                            obj.calCulatedOn = 0;
                        }
                        //End Here
                        #endregion

                        //Debjyoti 09032017
                        if (Convert.ToString(ddl_AmountAre.Value) == "2")
                        {
                            if (Convert.ToString(ddl_VatGstCst.Value) == "0~0~X")
                            {
                                if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                                {
                                    decimal finalCalCulatedOn = 0;
                                    decimal backProcessRate = (1 + (totalParcentage / 100));
                                    finalCalCulatedOn = obj.calCulatedOn / backProcessRate;
                                    obj.calCulatedOn = Math.Round(finalCalCulatedOn);
                                }
                            }
                        }

                        if (Convert.ToString(dr["TaxCalculateMethods"]) == "A")
                        {
                            obj.Taxes_Name = Convert.ToString(dr["Taxes_Name"]) + "(+)";

                        }
                        else
                        {
                            obj.Taxes_Name = Convert.ToString(dr["Taxes_Name"]) + "(-)";
                        }

                        obj.Amount = Convert.ToDouble(obj.calCulatedOn * (Convert.ToDecimal(obj.TaxField) / 100));




                        DataRow[] filtr = MainTaxDataTable.Select("TaxCode ='" + obj.Taxes_ID + "' and slNo=" + Convert.ToString(slNo));
                        if (filtr.Length > 0)
                        {
                            obj.Amount = Convert.ToDouble(filtr[0]["Amount"]);
                            if (obj.Taxes_ID == 0)
                            {
                                //   obj.TaxField = GetTaxName(Convert.ToInt32(Convert.ToString(filtr[0]["AltTaxCode"])));
                                aspxGridTax.JSProperties["cpComboCode"] = Convert.ToString(filtr[0]["AltTaxCode"]);
                            }
                            else
                                obj.TaxField = Convert.ToString(filtr[0]["Percentage"]);
                        }

                        TaxDetailsDetails.Add(obj);
                    }
                }
                else
                {
                    string keyValue = e.Parameters.Split('~')[0];

                    DataTable TaxRecord = (DataTable)Session["TPurchaseOrderFinalTaxRecord"];


                    foreach (DataRow dr in taxDetail.Rows)
                    {
                        TaxDetails obj = new TaxDetails();
                        obj.Taxes_ID = Convert.ToInt32(dr["Taxes_ID"]);
                        obj.taxCodeName = Convert.ToString(dr["taxCodeName"]);

                        if (Convert.ToString(dr["TaxCalculateMethods"]) == "A")
                            obj.Taxes_Name = Convert.ToString(dr["Taxes_Name"]) + "(+)";
                        else
                            obj.Taxes_Name = Convert.ToString(dr["Taxes_Name"]) + "(-)";
                        obj.TaxField = "";
                        obj.Amount = 0.0;

                        #region set calculated on
                        //Check Tax Applicable on and set to calculated on
                        if (Convert.ToString(dr["ApplicableOn"]) == "G")
                        {
                            obj.calCulatedOn = ProdGrossAmt;
                        }
                        else if (Convert.ToString(dr["ApplicableOn"]) == "N")
                        {
                            obj.calCulatedOn = ProdNetAmt;
                        }
                        else
                        {
                            obj.calCulatedOn = 0;
                        }
                        //End Here
                        #endregion

                        //Debjyoti 09032017
                        if (Convert.ToString(ddl_AmountAre.Value) == "2")
                        {
                            if (Convert.ToString(ddl_VatGstCst.Value) == "0~0~X")
                            {
                                if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                                {
                                    decimal finalCalCulatedOn = 0;
                                    decimal backProcessRate = (1 + (totalParcentage / 100));
                                    finalCalCulatedOn = obj.calCulatedOn / backProcessRate;
                                    obj.calCulatedOn = Math.Round(finalCalCulatedOn);
                                }
                            }
                        }

                        DataRow[] filtronexsisting1 = TaxRecord.Select("TaxCode=" + obj.Taxes_ID + " and SlNo=" + Convert.ToString(slNo));
                        if (filtronexsisting1.Length > 0)
                        {
                            if (obj.Taxes_ID == 0)
                            {
                                obj.TaxField = "0";
                            }
                            else
                            {
                                obj.TaxField = Convert.ToString(filtronexsisting1[0]["Percentage"]);
                            }
                            obj.Amount = Convert.ToDouble(filtronexsisting1[0]["Amount"]);
                        }
                        else
                        {
                            #region checkingFordb


                            //DataRow[] filtr = databaseReturnTable.Select("ProductTax_ProductId ='" + keyValue + "' and ProductTax_QuoteId=" +Convert.ToString( Session["QuotationID"] )+ " and ProductTax_TaxTypeId=" + obj.Taxes_ID);
                            //if (filtr.Length > 0)
                            //{
                            //    obj.Amount = Convert.ToDouble(filtr[0]["ProductTax_Amount"]);
                            //    if (obj.Taxes_ID == 0)
                            //    {
                            //        //obj.TaxField = GetTaxName();
                            //        obj.TaxField = "0";
                            //    }
                            //    else
                            //    {
                            //        obj.TaxField = Convert.ToString(filtr[0]["ProductTax_Percentage"]);
                            //    }


                            //    DataRow[] filtronexsisting = TaxRecord.Select("TaxCode=" + obj.Taxes_ID + " and SlNo=" + Convert.ToString(slNo));
                            //    if (filtronexsisting.Length > 0)
                            //    {
                            //        filtronexsisting[0]["Amount"] = obj.Amount;
                            //        if (obj.Taxes_ID == 0)
                            //        {
                            //            filtronexsisting[0]["Percentage"] = 0;
                            //        }
                            //        else
                            //        {
                            //            filtronexsisting[0]["Percentage"] = obj.TaxField;
                            //        }

                            //    }
                            //    else
                            //    {

                            //        DataRow taxRecordNewRow = TaxRecord.NewRow();
                            //        taxRecordNewRow["SlNo"] = slNo;
                            //        taxRecordNewRow["TaxCode"] = obj.Taxes_ID;
                            //        taxRecordNewRow["AltTaxCode"] = "0";
                            //        taxRecordNewRow["Percentage"] = obj.TaxField;
                            //        taxRecordNewRow["Amount"] = obj.Amount;

                            //        TaxRecord.Rows.Add(taxRecordNewRow);
                            //    }

                            //}
                            //else
                            //{
                            //    DataRow[] filtronexsisting = TaxRecord.Select("TaxCode=" + obj.Taxes_ID + " and SlNo=" + Convert.ToString(slNo));
                            //    if (filtronexsisting.Length > 0)
                            //    {
                            //        DataRow taxRecordNewRow = TaxRecord.NewRow();
                            //        taxRecordNewRow["SlNo"] = slNo;
                            //        taxRecordNewRow["TaxCode"] = obj.Taxes_ID;
                            //        taxRecordNewRow["AltTaxCode"] = "0";
                            //        taxRecordNewRow["Percentage"] = 0.0;
                            //        taxRecordNewRow["Amount"] = "0";

                            //        TaxRecord.Rows.Add(taxRecordNewRow);
                            //    }
                            //}




                            #endregion


                            DataRow[] filtronexsisting = TaxRecord.Select("TaxCode=" + obj.Taxes_ID + " and SlNo=" + Convert.ToString(slNo));
                            if (filtronexsisting.Length > 0)
                            {
                                DataRow taxRecordNewRow = TaxRecord.NewRow();
                                taxRecordNewRow["SlNo"] = slNo;
                                taxRecordNewRow["TaxCode"] = obj.Taxes_ID;
                                taxRecordNewRow["AltTaxCode"] = "0";
                                taxRecordNewRow["Percentage"] = 0.0;
                                taxRecordNewRow["Amount"] = "0";

                                TaxRecord.Rows.Add(taxRecordNewRow);
                            }

                        }
                        TaxDetailsDetails.Add(obj);

                        //      DataRow[] filtrIndex = databaseReturnTable.Select("ProductTax_ProductId ='" + keyValue + "' and ProductTax_QuoteId=" + Session["QuotationID"] + " and ProductTax_TaxTypeId=0");
                        DataRow[] filtrIndex = TaxRecord.Select("SlNo=" + Convert.ToString(slNo) + " and TaxCode=0");
                        if (filtrIndex.Length > 0)
                        {
                            aspxGridTax.JSProperties["cpComboCode"] = Convert.ToString(filtrIndex[0]["AltTaxCode"]);
                        }
                    }
                    Session["TPurchaseOrderFinalTaxRecord"] = TaxRecord;

                }
                //New Changes 170217
                //GstCode should fetch everytime
                DataRow[] finalFiltrIndex = MainTaxDataTable.Select("SlNo=" + Convert.ToString(slNo) + " and TaxCode=0");
                if (finalFiltrIndex.Length > 0)
                {
                    aspxGridTax.JSProperties["cpComboCode"] = Convert.ToString(finalFiltrIndex[0]["AltTaxCode"]);
                }

                aspxGridTax.JSProperties["cpJsonData"] = createJsonForDetails(taxDetail);

                retMsg = Convert.ToString(GetTotalTaxAmount(TaxDetailsDetails));
                aspxGridTax.JSProperties["cpUpdated"] = "ok~" + retMsg;

                TaxDetailsDetails = setCalculatedOn(TaxDetailsDetails, taxDetail);
                aspxGridTax.DataSource = TaxDetailsDetails;
                aspxGridTax.DataBind();

        #endregion
            }
        }


        public string createJsonForDetails(DataTable lstTaxDetails)
        {
            List<TaxSetailsJson> jsonList = new List<TaxSetailsJson>();
            TaxSetailsJson jsonObj;
            int visIndex = 0;
            foreach (DataRow taxObj in lstTaxDetails.Rows)
            {
                jsonObj = new TaxSetailsJson();

                jsonObj.SchemeName = Convert.ToString(taxObj["Taxes_Name"]);
                jsonObj.vissibleIndex = visIndex;
                jsonObj.applicableOn = Convert.ToString(taxObj["ApplicableOn"]);
                if (jsonObj.applicableOn == "G" || jsonObj.applicableOn == "N")
                {
                    jsonObj.applicableBy = "None";
                }
                else
                {
                    jsonObj.applicableBy = Convert.ToString(taxObj["dependOn"]);
                }
                visIndex++;
                jsonList.Add(jsonObj);
            }

            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            return oSerializer.Serialize(jsonList);
        }

        public List<TaxDetails> setCalculatedOn(List<TaxDetails> gridSource, DataTable taxDt)
        {
            foreach (TaxDetails taxObj in gridSource)
            {
                DataRow[] dependOn = taxDt.Select("dependOn='" + taxObj.Taxes_Name.Replace("(+)", "").Replace("(-)", "") + "'");
                if (dependOn.Length > 0)
                {
                    foreach (DataRow dr in dependOn)
                    {
                        //  List<TaxDetails> dependObj = gridSource.Where(r => r.Taxes_Name.Replace("(+)", "").Replace("(-)", "") == Convert.ToString(dependOn[0]["Taxes_Name"])).ToList();
                        foreach (var setCalObj in gridSource.Where(r => r.Taxes_Name.Replace("(+)", "").Replace("(-)", "") == Convert.ToString(dependOn[0]["Taxes_Name"])))
                        {
                            setCalObj.calCulatedOn = Convert.ToDecimal(taxObj.Amount);
                        }
                    }

                }

            }
            return gridSource;
        }






        protected void aspxGridTax_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridViewEditorEventArgs e)
        {

            if (e.Column.FieldName == "Taxes_Name")
            {
                e.Editor.ReadOnly = true;
            }
            if (e.Column.FieldName == "taxCodeName")
            {
                e.Editor.ReadOnly = true;
            }
            if (e.Column.FieldName == "calCulatedOn")
            {
                e.Editor.ReadOnly = true;
            }
            //else if (e.Column.FieldName == "Amount")
            //{
            //    e.Editor.ReadOnly = true;
            //}
            else
            {
                e.Editor.ReadOnly = false;
            }
        }
        protected void aspxGridTax_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {

        }
        protected void taxgrid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {

            int slNo = Convert.ToInt32(HdSerialNo.Value);
            DataTable TaxRecord = (DataTable)Session["TPurchaseOrderFinalTaxRecord"];
            foreach (var args in e.UpdateValues)
            {

                string TaxCodeDes = Convert.ToString(args.NewValues["Taxes_Name"]);
                decimal Percentage = 0;

                Percentage = Convert.ToDecimal(args.NewValues["TaxField"]);

                decimal Amount = Convert.ToDecimal(args.NewValues["Amount"]);
                string TaxCode = "0";
                if (!Convert.ToString(args.Keys[0]).Contains('~'))
                {
                    TaxCode = Convert.ToString(args.Keys[0]);
                }



                DataRow[] finalRow = TaxRecord.Select("SlNo=" + Convert.ToString(slNo) + " and TaxCode='" + TaxCode + "'");
                if (finalRow.Length > 0)
                {
                    finalRow[0]["Percentage"] = Percentage;
                    // finalRow[0]["TaxCode"] = args.NewValues["TaxField"]; 
                    finalRow[0]["Amount"] = Amount;

                    finalRow[0]["TaxCode"] = args.Keys[0];
                    finalRow[0]["AltTaxCode"] = "0";

                }
                else
                {
                    DataRow newRow = TaxRecord.NewRow();
                    newRow["slNo"] = slNo;
                    newRow["Percentage"] = Percentage;
                    newRow["TaxCode"] = TaxCode;
                    newRow["AltTaxCode"] = "0";
                    newRow["Amount"] = Amount;
                    TaxRecord.Rows.Add(newRow);
                }


            }

            //For GST/CST/VAT
            if (cmbGstCstVat.Value != null)
            {

                DataRow[] finalRow = TaxRecord.Select("SlNo=" + Convert.ToString(slNo) + " and TaxCode='0'");
                if (finalRow.Length > 0)
                {
                    finalRow[0]["Percentage"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[1];
                    finalRow[0]["Amount"] = txtGstCstVat.Text;
                    finalRow[0]["AltTaxCode"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[0];

                }
                else
                {
                    DataRow newRowGST = TaxRecord.NewRow();
                    newRowGST["slNo"] = slNo;
                    newRowGST["Percentage"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[1];
                    newRowGST["TaxCode"] = "0";
                    newRowGST["AltTaxCode"] = Convert.ToString(cmbGstCstVat.Value).Split('~')[0];
                    newRowGST["Amount"] = txtGstCstVat.Text;
                    TaxRecord.Rows.Add(newRowGST);
                }
            }
            //End Here


            Session["TPurchaseOrderFinalTaxRecord"] = TaxRecord;


            #region oldpart


            //DataTable taxdtByProductCode = new DataTable();
            //if (Session["ProdTax" + "_" + Convert.ToString(HdSerialNo.Value)] == null)
            //{

            //    taxdtByProductCode.TableName = "ProdTax"  + "_" + Convert.ToString(HdSerialNo.Value);


            //    taxdtByProductCode.Columns.Add("TaxCode", typeof(System.String));
            //    taxdtByProductCode.Columns.Add("TaxCodeDescription", typeof(System.String));
            //    taxdtByProductCode.Columns.Add("Percentage", typeof(System.Decimal));
            //    taxdtByProductCode.Columns.Add("Amount", typeof(System.Decimal));
            //    DataRow dr;
            //    foreach (var args in e.UpdateValues)
            //    {
            //        dr = taxdtByProductCode.NewRow();
            //        string TaxCodeDes = Convert.ToString(args.NewValues["Taxes_Name"]);
            //        decimal Percentage = 0;
            //        if (TaxCodeDes == "GST/CST/VAT")
            //        {
            //            Percentage = Convert.ToDecimal(Convert.ToString(args.NewValues["TaxField"]).Split('~')[1]);
            //        }
            //        else
            //        {
            //            Percentage = Convert.ToDecimal(args.NewValues["TaxField"]);
            //        }
            //        decimal Amount = Convert.ToDecimal(args.NewValues["Amount"]);

            //        dr["TaxCodeDescription"] = TaxCodeDes;
            //        if (Convert.ToString(args.Keys[0]) == "0")
            //        {
            //            dr["TaxCode"] = "0~" + Convert.ToString(args.NewValues["TaxField"]).Split('~')[0];
            //        }
            //        else
            //        {
            //            dr["TaxCode"] = args.Keys[0];
            //        }
            //        dr["Percentage"] = Percentage;
            //        dr["Amount"] = Amount;

            //        taxdtByProductCode.Rows.Add(dr);
            //    }
            //}
            //else
            //{
            //    taxdtByProductCode = (DataTable)Session["ProdTax"  +"_"+ Convert.ToString(HdSerialNo.Value)];

            //    foreach (var args in e.UpdateValues)
            //    {
            //        DataRow[] filtr ;

            //        if (Convert.ToString(args.Keys[0]) == "0")
            //        {
            //            filtr = taxdtByProductCode.Select("TaxCode like '%0~%'"); ;
            //        }
            //        else
            //        {
            //            filtr = taxdtByProductCode.Select("TaxCode='" + args.Keys[0]+"'");
            //        }

            //        if (filtr.Length > 0)
            //        {
            //            string TaxCodeDes = Convert.ToString(args.NewValues["Taxes_Name"]);
            //        filtr[0]["TaxCodeDescription"] = TaxCodeDes;
            //        if (Convert.ToString(args.Keys[0]) == "0")
            //        {
            //            filtr[0]["TaxCode"] = "0~" + Convert.ToString(args.NewValues["TaxField"]).Split('~')[0];
            //        }
            //        else
            //        {
            //            filtr[0]["TaxCode"] = args.Keys[0];
            //        }

            //        decimal Percentage = 0;
            //        if (TaxCodeDes == "GST/CST/VAT")
            //        {
            //            Percentage = Convert.ToDecimal(Convert.ToString(args.NewValues["TaxField"]).Split('~')[1]);
            //        }
            //        else
            //        {
            //            Percentage = Convert.ToDecimal(args.NewValues["TaxField"]);
            //        }
            //        decimal Amount = Convert.ToDecimal(args.NewValues["Amount"]);
            //        filtr[0]["Percentage"] = Percentage;
            //        filtr[0]["Amount"] = Amount;

            //        }
            //    }


            //}

            #endregion
            //  Session[taxdtByProductCode.TableName] = taxdtByProductCode;

        }
        protected void cmbGstCstVat_Callback(object sender, CallbackEventArgsBase e)
        {
            DateTime quoteDate = Convert.ToDateTime(dt_PLQuote.Date.ToString("yyyy-MM-dd"));

            PopulateGSTCSTVATCombo(quoteDate.ToString("yyyy-MM-dd"));
            CreateDataTaxTable();
            //DataTable taxTableItemLvl = (DataTable)Session["TPurchaseOrderFinalTaxRecord"];
            //foreach (DataRow dr in taxTableItemLvl.Rows)
            //    dr.Delete();
            //taxTableItemLvl.AcceptChanges();
            //Session["TPurchaseOrderFinalTaxRecord"] = taxTableItemLvl;
        }

        protected void cmbGstCstVatcharge_Callback(object sender, CallbackEventArgsBase e)
        {
            Session["TPBTaxDetails"] = null;
            DateTime quoteDate = Convert.ToDateTime(dt_PLQuote.Date.ToString("yyyy-MM-dd"));
            PopulateChargeGSTCSTVATCombo(quoteDate.ToString("yyyy-MM-dd"));
        }
        public DataTable getAllTaxDetails(DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            DataTable FinalTable = new DataTable();
            FinalTable.Columns.Add("SlNo", typeof(System.Int32));
            FinalTable.Columns.Add("TaxCode", typeof(System.String));
            FinalTable.Columns.Add("AltTaxCode", typeof(System.String));
            FinalTable.Columns.Add("Percentage", typeof(System.Decimal));
            FinalTable.Columns.Add("Amount", typeof(System.Decimal));

            //for insert
            foreach (var args in e.InsertValues)
            {
                string Slno = Convert.ToString(args.NewValues["SrlNo"]);
                DataRow existsRow;
                if (Session["ProdTax_" + Slno] != null)
                {
                    DataTable sessiontable = (DataTable)Session["ProdTax_" + Slno];
                    foreach (DataRow dr in sessiontable.Rows)
                    {
                        existsRow = FinalTable.NewRow();

                        existsRow["SlNo"] = Slno;
                        if (Convert.ToString(dr["taxCode"]).Contains("~"))
                        {
                            existsRow["TaxCode"] = "0";
                            existsRow["AltTaxCode"] = Convert.ToString(dr["taxCode"]).Split('~')[1];
                        }
                        else
                        {
                            existsRow["TaxCode"] = Convert.ToString(dr["taxCode"]);
                            existsRow["AltTaxCode"] = "0";
                        }

                        existsRow["Percentage"] = Convert.ToString(dr["Percentage"]);
                        existsRow["Amount"] = Convert.ToString(dr["Amount"]);

                        FinalTable.Rows.Add(existsRow);
                    }
                    Session.Remove("ProdTax_" + Slno);
                }
            }

            return FinalTable;
        }
        protected void taxgrid_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void taxgrid_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void taxgrid_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            e.Cancel = true;
        }
        public void DeleteTaxDetails(string SrlNo)
        {
            DataTable TaxDetailTable = new DataTable();
            if (Session["TPurchaseOrderFinalTaxRecord"] != null)
            {
                TaxDetailTable = (DataTable)Session["TPurchaseOrderFinalTaxRecord"];

                var rows = TaxDetailTable.Select("SlNo ='" + SrlNo + "'");
                foreach (var row in rows)
                {
                    row.Delete();
                }
                TaxDetailTable.AcceptChanges();
                #region Code Added by Sam on 09062017  To arrange Warehouse Data according to serial no Start
                int j = 0;
                foreach (DataRow dr in TaxDetailTable.Rows)
                {
                    string slno = Convert.ToString(dr["SlNo"]);
                    if (Convert.ToInt32(slno) > Convert.ToInt32(SrlNo))
                    {
                        dr["SlNo"] = Convert.ToString(Convert.ToInt32(slno) - 1);
                    }
                }
                TaxDetailTable.AcceptChanges();
                #endregion Code Added by Sam on 09062017  To arrange Warehouse Data according to serial no Start
                Session["TPurchaseOrderFinalTaxRecord"] = TaxDetailTable;
            }
        }
        public void UpdateTaxDetails(string oldSrlNo, string newSrlNo)
        {
            DataTable TaxDetailTable = new DataTable();
            if (Session["TPurchaseOrderFinalTaxRecord"] != null)
            {
                TaxDetailTable = (DataTable)Session["TPurchaseOrderFinalTaxRecord"];

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

                Session["TPurchaseOrderFinalTaxRecord"] = TaxDetailTable;
            }
        }


        public string createJsonForChargesTax(DataTable lstTaxDetails)
        {
            List<TaxSetailsJson> jsonList = new List<TaxSetailsJson>();
            TaxSetailsJson jsonObj;
            int visIndex = 0;
            foreach (DataRow taxObj in lstTaxDetails.Rows)
            {
                jsonObj = new TaxSetailsJson();

                jsonObj.SchemeName = Convert.ToString(taxObj["Taxes_Name"]);
                jsonObj.vissibleIndex = visIndex;
                jsonObj.applicableOn = Convert.ToString(taxObj["ApplicableOn"]);
                if (jsonObj.applicableOn == "G" || jsonObj.applicableOn == "N")
                {
                    jsonObj.applicableBy = "None";
                }
                else
                {
                    jsonObj.applicableBy = Convert.ToString(taxObj["dependOn"]);
                }
                visIndex++;
                jsonList.Add(jsonObj);
            }

            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            return oSerializer.Serialize(jsonList);
        }
        public List<Taxes> setChargeCalculatedOn(List<Taxes> gridSource, DataTable taxDt)
        {
            foreach (Taxes taxObj in gridSource)
            {
                DataRow[] dependOn = taxDt.Select("dependOn='" + taxObj.TaxName.Replace("(+)", "").Replace("(-)", "").Trim() + "'");
                if (dependOn.Length > 0)
                {
                    foreach (DataRow dr in dependOn)
                    {
                        //  List<TaxDetails> dependObj = gridSource.Where(r => r.Taxes_Name.Replace("(+)", "").Replace("(-)", "") == Convert.ToString(dependOn[0]["Taxes_Name"])).ToList();
                        foreach (var setCalObj in gridSource.Where(r => r.TaxName.Replace("(+)", "").Replace("(-)", "").Trim() == Convert.ToString(dependOn[0]["Taxes_Name"]).Replace("(+)", "").Replace("(-)", "").Trim()))
                        {
                            setCalObj.calCulatedOn = Convert.ToDecimal(taxObj.Amount);
                        }
                    }

                }

            }
            return gridSource;
        }

        public void PopulateChargeGSTCSTVATCombo(string quoteDate)
        {
            string LastCompany = "";
            if (Convert.ToString(Session["LastCompany"]) != null)
            {
                LastCompany = Convert.ToString(Session["LastCompany"]);
            }
            ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
            proc.AddVarcharPara("@Action", 500, "LoadChargeGSTCSTVATCombo");
            proc.AddVarcharPara("@S_QuoteAdd_CompanyID", 10, Convert.ToString(LastCompany));
            proc.AddVarcharPara("@S_quoteDate", 10, quoteDate);
            DataTable DT = proc.GetTable();
            cmbGstCstVatcharge.DataSource = DT;
            cmbGstCstVatcharge.TextField = "Taxes_Name";
            cmbGstCstVatcharge.ValueField = "Taxes_ID";
            cmbGstCstVatcharge.DataBind();
        }

        #region Quotation Tax Details

        public IEnumerable GetTaxes()
        {
            List<Taxes> TaxList = new List<Taxes>();
          //  BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

            DataTable DT = GetTaxData(dt_PLQuote.Date.ToString("yyyy-MM-dd"));
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                Taxes Taxes = new Taxes();
                Taxes.TaxID = Convert.ToString(DT.Rows[i]["Taxes_ID"]);
                Taxes.TaxName = Convert.ToString(DT.Rows[i]["Taxes_Name"]);
                Taxes.Percentage = Convert.ToString(DT.Rows[i]["Percentage"]);
                Taxes.Amount = Convert.ToString(DT.Rows[i]["Amount"]);
                TaxList.Add(Taxes);
            }

            return TaxList;
        }
        public IEnumerable GetTaxes(DataTable DT)
        {
            List<Taxes> TaxList = new List<Taxes>();

            decimal totalParcentage = 0;
            foreach (DataRow dr in DT.Rows)
            {
                if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                {
                    totalParcentage += Convert.ToDecimal(dr["Percentage"]);
                }
            }




            for (int i = 0; i < DT.Rows.Count; i++)
            {
                if (Convert.ToString(DT.Rows[i]["Taxes_ID"]) != "0")
                {
                    Taxes Taxes = new Taxes();
                    Taxes.TaxID = Convert.ToString(DT.Rows[i]["Taxes_ID"]);
                    Taxes.TaxName = Convert.ToString(DT.Rows[i]["Taxes_Name"]);
                    Taxes.Percentage = Convert.ToString(DT.Rows[i]["Percentage"]);
                    Taxes.Amount = Convert.ToString(DT.Rows[i]["Amount"]);
                    if (Convert.ToString(DT.Rows[i]["ApplicableOn"]) == "G")
                    {
                        Taxes.calCulatedOn = Convert.ToDecimal(HdChargeProdAmt.Value);
                    }
                    else if (Convert.ToString(DT.Rows[i]["ApplicableOn"]) == "N")
                    {
                        Taxes.calCulatedOn = Convert.ToDecimal(HdChargeProdNetAmt.Value);
                    }
                    else
                    {
                        Taxes.calCulatedOn = 0;
                    }

                    //Set Amount Value 
                    if (Taxes.Amount == "0.00")
                    {
                        Taxes.Amount = Convert.ToString(Taxes.calCulatedOn * (Convert.ToDecimal(Taxes.Percentage) / 100));
                    }


                    if (Convert.ToString(ddl_AmountAre.Value) == "2")
                    {
                        if (Convert.ToString(ddl_VatGstCst.Value) == "0~0~X")
                        {
                            if (Convert.ToString(DT.Rows[i]["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(DT.Rows[i]["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(DT.Rows[i]["TaxTypeCode"]).Trim() == "SGST")
                            {
                                decimal finalCalCulatedOn = 0;
                                decimal backProcessRate = (1 + (totalParcentage / 100));
                                finalCalCulatedOn = Taxes.calCulatedOn / backProcessRate;
                                Taxes.calCulatedOn = Math.Round(finalCalCulatedOn);
                            }
                        }
                    }

                    TaxList.Add(Taxes);
                }
            }

            return TaxList;
        }
        public DataTable GetTaxData(string quoteDate)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
            proc.AddVarcharPara("@Action", 500, "TaxDetails");
            proc.AddIntegerPara("@PurchaseInvoiceId", Convert.ToInt32(Session["TPurchaseInvoice_Id"]));
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@branchId", 500, Convert.ToString(ddl_Branch.SelectedItem.Value));
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            proc.AddVarcharPara("@PurchaseInvoiceDate", 10, quoteDate);
            dt = proc.GetTable();
            return dt;
        }
        protected void gridTax_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];
            if (strSplitCommand == "Display")
            {
                DataTable TaxDetailsdt = new DataTable();
                if (Session["TPBTaxDetails"] == null)
                {
                    Session["TPBTaxDetails"] = GetTaxData(dt_PLQuote.Date.ToString("yyyy-MM-dd"));
                }

                if (Session["TPBTaxDetails"] != null)
                {
                    TaxDetailsdt = (DataTable)Session["TPBTaxDetails"];


                    #region Delete Igst,Cgst,Sgst respectively
                    //Get Company Gstin 09032017
                    string CompInternalId = Convert.ToString(Session["LastCompany"]);
                    string[] compGstin = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_gstin", "cmp_internalid='" + CompInternalId + "'", 1);
                    string ShippingState = "";

                    #region ##### Added By : Samrat Roy -- For BillingShippingUserControl ######
                    string sstateCode = ""; ;
                    if (ddlPosGstTPurchase.Value != null)
                    {
                        if (ddlPosGstTPurchase.Value.ToString() == "S")
                        {
                            sstateCode = Purchase_BillingShipping.GeteShippingStateCode();
                        }
                        else if (ddlPosGstTPurchase.Value.ToString() == "B")
                        {
                            sstateCode = Purchase_BillingShipping.GetBillingStateCode();
                        }
                    }
                    
                    ShippingState = sstateCode;

                    if (ShippingState.Trim() != "")
                    {
                        ShippingState = ShippingState;
                    }

                    #region ##### Old Code -- BillingShipping ######
                    ////if (chkBilling.Checked)
                    ////{
                    ////    if (CmbState.Value != null)
                    ////    {
                    ////        ShippingState = CmbState.Text;
                    ////        ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                    ////    }
                    ////}
                    ////else
                    ////{
                    ////    if (CmbState1.Value != null)
                    ////    {
                    ////        ShippingState = CmbState1.Text;
                    ////        ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                    ////    }
                    ////}
                    #endregion

                    #endregion

                    if (ShippingState.Trim() != "" && compGstin[0].Trim() != "")
                    {
                        if (compGstin.Length > 0)
                        {
                            if (compGstin[0].Substring(0, 2) == ShippingState)
                            {
                                //Check if the state is in union territories then only UTGST will apply
                                //   Chandigarh     Andaman and Nicobar Islands    DADRA & NAGAR HAVELI    DAMAN & DIU   Lakshadweep              PONDICHERRY
                                if (ShippingState == "4" || ShippingState == "26" || ShippingState == "25" || ShippingState == "35" || ShippingState == "31" || ShippingState == "34")
                                {
                                    foreach (DataRow dr in TaxDetailsdt.Rows)
                                    {
                                        if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                                        {
                                            dr.Delete();
                                        }
                                    }

                                }
                                else
                                {
                                    foreach (DataRow dr in TaxDetailsdt.Rows)
                                    {
                                        if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                                        {
                                            dr.Delete();
                                        }
                                    }
                                }
                                TaxDetailsdt.AcceptChanges();
                            }
                            else
                            {
                                foreach (DataRow dr in TaxDetailsdt.Rows)
                                {
                                    if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
                                    {
                                        dr.Delete();
                                    }
                                }
                                TaxDetailsdt.AcceptChanges();

                            }

                        }
                    }

                    //If Company GSTIN is blank then Delete All CGST,UGST,IGST,CGST
                    if (compGstin[0].Trim() == "")
                    {
                        foreach (DataRow dr in TaxDetailsdt.Rows)
                        {
                            if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST")
                            {
                                dr.Delete();
                            }
                        }
                        TaxDetailsdt.AcceptChanges();
                    }

                    #endregion

                    //gridTax.DataSource = GetTaxes();
                    var taxlist = (List<Taxes>)GetTaxes(TaxDetailsdt);
                    var taxChargeDataSource = setChargeCalculatedOn(taxlist, TaxDetailsdt);
                    gridTax.DataSource = taxChargeDataSource;
                    gridTax.DataBind();
                    gridTax.JSProperties["cpJsonChargeData"] = createJsonForChargesTax(TaxDetailsdt);
                }
            }
            else if (strSplitCommand == "SaveGst")
            {
                DataTable TaxDetailsdt = new DataTable();
                if (Session["TPBTaxDetails"] != null)
                {
                    TaxDetailsdt = (DataTable)Session["TPBTaxDetails"];
                }
                else
                {
                    TaxDetailsdt.Columns.Add("Taxes_ID", typeof(string));
                    TaxDetailsdt.Columns.Add("Taxes_Name", typeof(string));
                    TaxDetailsdt.Columns.Add("Percentage", typeof(string));
                    TaxDetailsdt.Columns.Add("Amount", typeof(string));
                    //ForGst
                    TaxDetailsdt.Columns.Add("AltTax_Code", typeof(string));
                }
                DataRow[] existingRow = TaxDetailsdt.Select("Taxes_ID='0'");
                if (Convert.ToString(cmbGstCstVatcharge.Value) == "0")
                {
                    if (existingRow.Length > 0)
                    {
                        TaxDetailsdt.Rows.Remove(existingRow[0]);
                    }
                }
                else
                {
                    if (existingRow.Length > 0)
                    {

                        existingRow[0]["Percentage"] = Convert.ToString(cmbGstCstVatcharge.Value).Split('~')[1];
                        existingRow[0]["Amount"] = txtGstCstVatCharge.Text;
                        existingRow[0]["AltTax_Code"] = Convert.ToString(cmbGstCstVatcharge.Value).Split('~')[0]; ;

                    }
                    else
                    {
                        string GstTaxId = Convert.ToString(cmbGstCstVatcharge.Value).Split('~')[0];
                        string GstPerCentage = Convert.ToString(cmbGstCstVatcharge.Value).Split('~')[1];
                        string GstAmount = txtGstCstVatCharge.Text;
                        DataRow gstRow = TaxDetailsdt.NewRow();
                        gstRow["Taxes_ID"] = 0;
                        gstRow["Taxes_Name"] = cmbGstCstVatcharge.Text;
                        gstRow["Percentage"] = GstPerCentage;
                        gstRow["Amount"] = GstAmount;
                        gstRow["AltTax_Code"] = GstTaxId;
                        TaxDetailsdt.Rows.Add(gstRow);
                    }

                    Session["TPBTaxDetails"] = TaxDetailsdt;
                }
            }
        }
        protected void gridTax_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void gridTax_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void gridTax_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void gridTax_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            DataTable TaxDetailsdt = new DataTable();
            if (Session["TPBTaxDetails"] != null)
            {
                TaxDetailsdt = (DataTable)Session["TPBTaxDetails"];
            }
            else
            {
                TaxDetailsdt.Columns.Add("Taxes_ID", typeof(string));
                TaxDetailsdt.Columns.Add("Taxes_Name", typeof(string));
                TaxDetailsdt.Columns.Add("Percentage", typeof(string));
                TaxDetailsdt.Columns.Add("Amount", typeof(string));
                //ForGst
                TaxDetailsdt.Columns.Add("AltTax_Code", typeof(string));
            }

            foreach (var args in e.UpdateValues)
            {
                string TaxID = Convert.ToString(args.Keys["TaxID"]);
                string TaxName = Convert.ToString(args.NewValues["TaxName"]);
                string Percentage = Convert.ToString(args.NewValues["Percentage"]);
                string Amount = Convert.ToString(args.NewValues["Amount"]);

                bool Isexists = false;
                foreach (DataRow drr in TaxDetailsdt.Rows)
                {
                    string OldTaxID = Convert.ToString(drr["Taxes_ID"]);

                    if (OldTaxID == TaxID)
                    {
                        Isexists = true;

                        drr["Percentage"] = Percentage;
                        drr["Amount"] = Amount;

                        break;
                    }
                }

                if (Isexists == false)
                {
                    TaxDetailsdt.Rows.Add(TaxID, TaxName, Percentage, Amount, 0);
                }
            }

            if (cmbGstCstVatcharge.Value != null)
            {
                DataRow[] existingRow = TaxDetailsdt.Select("Taxes_ID='0'");
                if (Convert.ToString(cmbGstCstVatcharge.Value) == "0")
                {
                    if (existingRow.Length > 0)
                    {
                        TaxDetailsdt.Rows.Remove(existingRow[0]);
                    }
                }
                else
                {
                    if (existingRow.Length > 0)
                    {

                        existingRow[0]["Percentage"] = Convert.ToString(cmbGstCstVatcharge.Value).Split('~')[1];
                        existingRow[0]["Amount"] = txtGstCstVatCharge.Text;
                        existingRow[0]["AltTax_Code"] = Convert.ToString(cmbGstCstVatcharge.Value).Split('~')[0]; ;

                    }
                    else
                    {
                        string GstTaxId = Convert.ToString(cmbGstCstVatcharge.Value).Split('~')[0];
                        string GstPerCentage = Convert.ToString(cmbGstCstVatcharge.Value).Split('~')[1];
                        string GstAmount = txtGstCstVatCharge.Text;
                        DataRow gstRow = TaxDetailsdt.NewRow();
                        gstRow["Taxes_ID"] = 0;
                        gstRow["Taxes_Name"] = cmbGstCstVatcharge.Text;
                        gstRow["Percentage"] = GstPerCentage;
                        gstRow["Amount"] = GstAmount;
                        gstRow["AltTax_Code"] = GstTaxId;
                        TaxDetailsdt.Rows.Add(gstRow);
                    }
                }
            }

            Session["TPBTaxDetails"] = TaxDetailsdt;

            gridTax.DataSource = GetTaxes(TaxDetailsdt);
            gridTax.DataBind();
        }
        protected void gridTax_DataBinding(object sender, EventArgs e)
        {
            if (Session["TPBTaxDetails"] != null)
            {
                DataTable TaxDetailsdt = (DataTable)Session["TPBTaxDetails"];

                //gridTax.DataSource = GetTaxes();
                var taxlist = (List<Taxes>)GetTaxes(TaxDetailsdt);
                var taxChargeDataSource = setChargeCalculatedOn(taxlist, TaxDetailsdt);
                gridTax.DataSource = taxChargeDataSource;
            }
        }


        #endregion

        #region   Indent/Requisition Number
        protected void lookup_quotation_DataBinding(object sender, EventArgs e)
        {
            if (Session["PB_ComponentData"] != null)
            {
                lookup_quotation.DataSource = (DataTable)Session["PB_ComponentData"];
            }
        }


        protected void BindLookUp(string Customer, string OrderDate, string ComponentType, string userbranch)  //  edited by sam adding  branch parameter
        {
            string Action = "";
            if (ComponentType == "PO")
            {
                Action = "GetOrder";
            }
            else if (ComponentType == "PC")
            {
                Action = "GetChallan";
            }

            string strInvoiceID = Convert.ToString(Session["TPurchaseInvoice_Id"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            //DataTable ComponentTable = objSalesInvoiceBL.GetComponent(Customer, OrderDate, ComponentType, FinYear, Action, strInvoiceID);

            DataTable ComponentTable = objPurchaseInvoice.GetComponent(strInvoiceID, Customer, OrderDate, ComponentType, Action, userbranch, Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]));

            lookup_quotation.GridView.Selection.CancelSelection();

            Session["PB_ComponentData"] = ComponentTable;
            lookup_quotation.DataSource = ComponentTable;
            lookup_quotation.DataBind();

            //for (int i = 0; i < lookup_quotation.; i++)
            //{
            //    gv.Selection.SelectRow(i);
            //}


        }

        //protected void BindLookUp(string OrderDate)
        //{
        //    string status = string.Empty;

        //    //Subhabrata
        //    if (Convert.ToString(Request.QueryString["key"]) != "ADD")
        //    {
        //        status = "DONE";
        //    }
        //    else
        //    {
        //        status = "NOT-DONE";
        //    }//End



        //    DataTable QuotationTable;



        //}


        public IEnumerable GetProductsInfo(DataTable SalesOrderdt1)
        {
            List<SalesOrder> OrderList = new List<SalesOrder>();
            for (int i = 0; i < SalesOrderdt1.Rows.Count; i++)
            {
                SalesOrder Orders = new SalesOrder();

                Orders.SrlNo = Convert.ToString(i + 1);
                Orders.Key_UniqueId = Convert.ToString(i + 1);
                if (!string.IsNullOrEmpty(Convert.ToString(SalesOrderdt1.Rows[i]["Indent_No"])))
                { Orders.Quotation_No = Convert.ToInt64(SalesOrderdt1.Rows[i]["Indent_No"]); }
                else
                { Orders.Quotation_No = 0; }
                Orders.ProductID = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_ProductId"]);
                Orders.gvColDiscription = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_ProductDescription"]);
                Orders.Quantity = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_Quantity"]);
                Orders.Quotation_Num = Convert.ToString(SalesOrderdt1.Rows[i]["ComponentNumber"]);
                Orders.Product_Shortname = Convert.ToString(SalesOrderdt1.Rows[i]["Product_Name"]);
                Orders.QuoteDetails_Id = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_Id"]);

                OrderList.Add(Orders);
            }

            return OrderList;
        }
        #endregion




        #region Common Use not Need to check of grid_CellEditorInitialize
        protected void grid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            //string invtype = ddlInventory.SelectedItem.Value;

            //    if (e.Column.FieldName == "Amount")
            //    {
            //        if (invtype == "N")
            //        {
            //            //e.Editor.ReadOnly = false;
            //            e.Editor.Enabled = true;
            //    }
            //    else
            //    {
            //        //e.Editor.ReadOnly = true;
            //        e.Editor.Enabled = true;
            //    }
            //}
            //else 

            if (e.Column.FieldName == "SrlNo")
            {
                e.Editor.Enabled = true;
                //e.Editor.ReadOnly = true;
            }
            else if (e.Column.FieldName == "ProductName")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "UOM")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "gvColDiscription")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "gvColUOM")
            {
                e.Editor.Enabled = true;
            }
            //else if (e.Column.FieldName == "Amount")
            //{
            //    e.Editor.Enabled = true;
            //}
            else if (e.Column.FieldName == "TotalAmount")
            {
                e.Editor.Enabled = true;
            }
            else
            {
                e.Editor.ReadOnly = false;
            }


        }

        #endregion Common Use not Need to check of grid_CellEditorInitialize



        #region Edit Section Start

        #region Main Header Section Start
        public void SetPBDetails() // To Get Main Header Section Detail in Edit Mode
        {
            DataSet PBEdit = GetPurchaseInvoiceDetails();
            DataTable PBEditdt = PBEdit.Tables[0];
            if (PBEditdt != null && PBEditdt.Rows.Count > 0)
            {
                string Branch_Id = Convert.ToString(PBEditdt.Rows[0]["Invoice_BranchId"]);
                Session["TWbranchInEdit"] = Branch_Id;
                string Quote_Number = Convert.ToString(PBEditdt.Rows[0]["Invoice_Number"]);
                string Invoice_Date = Convert.ToString(PBEditdt.Rows[0]["Invoice_Date"]);

                string partyInvNo = Convert.ToString(PBEditdt.Rows[0]["PartyInvoiceNo"]);
                string partyInvDt = Convert.ToString(PBEditdt.Rows[0]["PartyInvoiceDate"]);
                string cashBank = Convert.ToString(PBEditdt.Rows[0]["CashBank_Code"]);
                if (cashBank != null && cashBank != "")
                {
                    ddl_cashBank.Value = cashBank;
                }              
                string Customer_Id = Convert.ToString(PBEditdt.Rows[0]["Vendor_Id"]);
                string Contact_Person_Id = Convert.ToString(PBEditdt.Rows[0]["Contact_Person_Id"]);
                string Invoice_Reference = Convert.ToString(PBEditdt.Rows[0]["Invoice_Reference"]);

                ddlPosGstTPurchase.DataSource = PBEdit.Tables[2];
                ddlPosGstTPurchase.ValueField = "ID";
                ddlPosGstTPurchase.TextField = "Name";
                ddlPosGstTPurchase.DataBind();

                string PosForGst = Convert.ToString(PBEditdt.Rows[0]["PosForGst"]);
                ddlPosGstTPurchase.Value = PosForGst;
                string Currency_Id = Convert.ToString(PBEditdt.Rows[0]["Currency_Id"]);
                string Currency_Conversion_Rate = Convert.ToString(PBEditdt.Rows[0]["Currency_Conversion_Rate"]);
                string Tax_Option = Convert.ToString(PBEditdt.Rows[0]["Tax_Option"]);
                string Tax_Code = Convert.ToString(PBEditdt.Rows[0]["Tax_Code"]);            
                string IsUsed = Convert.ToString(PBEditdt.Rows[0]["IsUsed"]);

                Purchase_BillingShipping.SetBillingShippingTable(PBEdit.Tables[1]);
                DataTable dtship = PBEdit.Tables[1];
                string shiptopartyId="";
                if (Convert.ToString(dtship.Rows[1]["ShipToPartyId"]) != "")
                {
                    shiptopartyId = Convert.ToString(dtship.Rows[1]["ShipToPartyId"]);
                    shipTopartyId.Value = shiptopartyId;
                }
                else
                {
                    shiptopartyId = Convert.ToString(dtship.Rows[0]["ShipToPartyId"]);
                    shipTopartyId.Value = shiptopartyId;
                }
                #region Headerportion Dtl
                int i = 0;

                //string InternalId = Convert.ToString(Convert.ToString(e.Parameter.Split('~')[1]));
                //string vendorbranchid = Convert.ToString(Convert.ToString(e.Parameter.Split('~')[2]));
                //i = PopulateContactPersonOfCustomer(Customer_Id);
                //if (i == 1)
                //{
                //    cmbContactPerson.JSProperties["cpContactdtl"] = "Y";
                //}
                //else
                //{
                //    cmbContactPerson.JSProperties["cpContactdtl"] = "N";
                //}


                DataSet vendst = new DataSet();
                vendst = objPurchaseInvoice.GetCustomerDetails_InvoiceRelated(Customer_Id, Branch_Id);
                //if (vendst.Tables[0] != null && vendst.Tables[0].Rows.Count > 0)
                //{
                //    string strDueDate = Convert.ToString(vendst.Tables[0].Rows[0]["DueDate"]);
                //    cmbContactPerson.JSProperties["cpDueDate"] = strDueDate;
                //}
                //else
                //{
                //    cmbContactPerson.JSProperties["cpDueDate"] = null;
                //}

                DataTable dtt = GetProjectEditData();
                if (dtt != null && dtt.Rows.Count > 0)
                {
                    lookup_Project.GridView.Selection.SelectRowByKey(Convert.ToInt64(dtt.Rows[0]["Proj_Id"]));


                    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                    DataTable dt2 = oDBEngine.GetDataTable("Select Hierarchy_ID from V_ProjectList where Proj_Id='" + Convert.ToInt64(dtt.Rows[0]["Proj_Id"]) + "'");
                    if (dt2.Rows.Count > 0)
                    {
                        ddlHierarchy.SelectedValue = Convert.ToString(dt2.Rows[0]["Hierarchy_ID"]);
                    }

                }

                if (vendst.Tables[1] != null && vendst.Tables[1].Rows.Count > 0)
                {
                    pageheaderContent.Attributes.Add("style", "display:block");
                    divOutstanding.Attributes.Add("style", "display:block");
                    var convertDecimal = Convert.ToDecimal(Convert.ToString(vendst.Tables[1].Rows[0]["NetOutstanding"]));
                    if (convertDecimal > 0)
                    {
                        lblOutstanding.Text = Convert.ToString(convertDecimal) + "(Db)";
                    }
                    else
                    {

                        lblOutstanding.Text = Convert.ToString(convertDecimal * -1).ToString() + "(Cr)";
                    }
                }
                else
                {
                    pageheaderContent.Attributes.Add("style", "display:block");
                    divOutstanding.Attributes.Add("style", "display:block");
                    lblOutstanding.Text = "0.0";
                }



                if (vendst.Tables[2] != null && vendst.Tables[2].Rows.Count > 0)
                {
                    pageheaderContent.Attributes.Add("style", "display:block");
                    divGSTN.Attributes.Add("style", "display:block");
                    string strGSTN = Convert.ToString(vendst.Tables[2].Rows[0]["CNT_GSTIN"]);
                    if (strGSTN != "")
                    {
                        lblGSTIN.Text = "Yes";
                    }
                    else
                    {
                        lblGSTIN.Text = "No";
                    }
                    //cmbContactPerson.JSProperties["cpGSTN"] = "Yes";
                    //cmbContactPerson.JSProperties["cpGSTNNO"] = strGSTN.Substring(0, 2);
                    //}
                    //else
                    //{
                    //    cmbContactPerson.JSProperties["cpGSTN"] = "No";
                    //}
                }


                #endregion Headerportion Dtl

                string revmechnism = Convert.ToString(PBEditdt.Rows[0]["revmechnism"]);
                if (revmechnism.ToUpper() == "TRUE")
                {
                    chk_reversemechenism.Checked = true;
                }
                else
                {
                    chk_reversemechenism.Checked = false;
                }

                #region NonInventory Section By Sam on 18052017 Start
                string invtype = Convert.ToString(PBEditdt.Rows[0]["invtype"]);
                if (invtype == "N")
                {
                    DataTable tdsdt = objPurchaseInvoice.GetTdsMainIdByInvoiceID(Convert.ToString(Session["TPurchaseInvoice_Id"]));
                    if (tdsdt.Rows.Count > 0)
                    {
                        string tdsmainid = Convert.ToString(tdsdt.Rows[0]["TDSMainId"]);
                        if (tdsmainid != "0")
                        {
                            ddl_TdsScheme.Value = tdsmainid;
                        }
                        else
                        {

                        }
                    }

                    ddlInventory.SelectedValue = invtype;
                    rdl_PurchaseInvoice.Visible = false;
                    lookup_quotation.Visible = false;
                    lbl_InvoiceNO.Visible = false;
                    ComponentDatePanel.Visible = false;
                    dt_Quotation.Visible = false;
                    hdnTDSShoworNot.Value = "S";
                    //divTdsScheme.Attributes.Remove("hide");

                }
                else if (invtype == "" || invtype == "Y")
                {
                    ddlInventory.SelectedValue = "Y";
                    rdl_PurchaseInvoice.Visible = true;
                    lookup_quotation.Visible = true;
                    lbl_InvoiceNO.Visible = true;
                    ComponentDatePanel.Visible = true;
                    dt_Quotation.Visible = true;
                    hdnTDSShoworNot.Value = "H";
                    //divTdsScheme.Attributes.Add("class","hide");
                }
                ddlInventory.Enabled = false;
                #endregion NonInventory Section By Sam on 18052017 End
                txt_partyInvNo.Text = partyInvNo;
                if (partyInvDt != null && partyInvDt != "")
                {
                    dt_partyInvDt.Date = Convert.ToDateTime(partyInvDt);
                }
                txtVoucherNo.Text = Quote_Number;
                dt_PLQuote.Date = Convert.ToDateTime(Invoice_Date);
                PopulateGSTCSTVATCombo(Convert.ToDateTime(Invoice_Date).ToString("yyyy-MM-dd"));
                PopulateChargeGSTCSTVATCombo(Convert.ToDateTime(Invoice_Date).ToString("yyyy-MM-dd"));               
               // lookup_Customer.GridView.Selection.SelectRowByKey(Customer_Id);
                hdnCustomerId.Value = Customer_Id;
                txtVendorName.Text = Convert.ToString(PBEditdt.Rows[0]["VendorName"]);
                int y = PopulateContactPersonOfCustomer(Customer_Id);
                cmbContactPerson.SelectedItem = cmbContactPerson.Items.FindByValue(Contact_Person_Id);
                txt_Refference.Text = Invoice_Reference;
                ddl_Branch.SelectedValue = Branch_Id;            
                ddl_Currency.SelectedValue = Currency_Id;
                if (Currency_Id == "1")
                {
                    txt_Rate.ClientEnabled = false;
                }
                else
                {
                    txt_Rate.ClientEnabled = true;
                }
                txt_Rate.Value = Currency_Conversion_Rate;
                txt_Rate.Text = Currency_Conversion_Rate;
                if (Tax_Option != "0") ddl_AmountAre.Value = Tax_Option;
                if (Tax_Code != "0")
                {                   
                    PopulateGSTCSTVAT("2");
                    setValueForHeaderGST(ddl_VatGstCst, Tax_Code);
                }
                else
                {
                    PopulateGSTCSTVAT("2");
                    ddl_VatGstCst.SelectedIndex = 0;
                }
                ddl_AmountAre.ClientEnabled = false;
                ddl_VatGstCst.ClientEnabled = false;

                if (IsUsed == "Y")
                {
                    dt_PLQuote.ClientEnabled = false;
                    // lookup_Customer.ClientEnabled = false;
                }
                else
                {
                    dt_PLQuote.ClientEnabled = true;
                   // lookup_Customer.ClientEnabled = true;
                }


                // if PB is tagged with PO or PC then this section will Applicable Section Start

                //        //Subhabrata InvoiceCreatedFromDoc
                string doctype = Convert.ToString(PBEditdt.Rows[0]["InvoiceCreatedFromDoc"]);
                string Quoids = Convert.ToString(PBEditdt.Rows[0]["InvoiceCreatedFromDoc_Ids"]);

                if (!String.IsNullOrEmpty(Quoids))
                {

                    string[] eachQuo = Quoids.Split(',');
                    if (eachQuo.Length > 1)//More tha one quotation
                    {
                        if (doctype == "PO")
                        {
                            lookup_Project.ClientEnabled = false;
                            lbl_InvoiceNO.Text = "Order Dates";
                            dt_Quotation.Text = "Multiple Purchase Order Dates";
                        }
                        else if (doctype == "PC")
                        {
                            lbl_InvoiceNO.Text = "Challan Dates";
                            dt_Quotation.Text = "Multiple GRN Dates";
                        }                       
                        BindLookUp(Customer_Id, Invoice_Date, doctype, Branch_Id);                      
                        foreach (string val in eachQuo)
                        {
                            lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                        }                       
                    }
                    else if (eachQuo.Length == 1)//Single Quotation
                    {
                        lookup_Project.ClientEnabled = false;
                        DateTime ChallanOrderDate = Convert.ToDateTime(PBEditdt.Rows[0]["ChallanOrderDate"]);
                        dt_Quotation.Text = ChallanOrderDate.ToString("dd-MM-yyyy");                      
                        BindLookUp(Customer_Id, Invoice_Date, doctype, Branch_Id);
                    
                        foreach (string val in eachQuo)
                        {
                            lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));                           
                        }
                    }
                    else//No Quotation selected
                    {
                        BindLookUp(Customer_Id, Invoice_Date, doctype, Branch_Id);                      
                    }

                    if (rdl_PurchaseInvoice.Items.Count > 1)
                    {
                        if (doctype == "PO")
                        {
                            rdl_PurchaseInvoice.Items[0].Selected = true;
                            lookup_quotation.ClientEnabled = true;
                        }
                        else if (doctype == "PC")
                        {
                            rdl_PurchaseInvoice.Items[1].Selected = true;
                            lookup_quotation.ClientEnabled = false;
                        }
                        else
                        {
                            lookup_quotation.ClientEnabled = true;
                        }
                    }
                    else
                    {
                        if (doctype == "PO")
                        {
                            rdl_PurchaseInvoice.Items[0].Selected = true;
                            lookup_quotation.ClientEnabled = true;
                        }
                        else if (doctype == "PC")
                        {
                            rdl_PurchaseInvoice.Items[0].Selected = true;
                            lookup_quotation.ClientEnabled = false;
                        }
                        else
                        {
                            lookup_quotation.ClientEnabled = true;
                        }
                    }

                }
                // if PB is tagged with PO or PC then this section will Applicable Section 
            }
        }
        public DataTable GetPurchaseInvoiceEditData()  // To Get Main Header Section Detail in Edit Mode
        {
            DataTable dt = new DataTable();          
            ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
            proc.AddVarcharPara("@Action", 500, "PBEditDetails");
            proc.AddVarcharPara("@PurchaseInvoiceId", 500, Convert.ToString(Session["TPurchaseInvoice_Id"]));
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@branchId", 500, Convert.ToString(Session["userbranchID"]));
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetProjectEditData()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
            proc.AddVarcharPara("@Action", 500, "TPProjectEditdata");
            proc.AddVarcharPara("@PurchaseInvoiceId", 200, Convert.ToString(Session["TPurchaseInvoice_Id"]));
            dt = proc.GetTable();
            return dt;
        }
        public DataSet GetPurchaseInvoiceDetails()  // To Get Main Header Section Detail in Edit Mode
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
            proc.AddVarcharPara("@Action", 500, "TransitPurchaseEditDetails");
            proc.AddVarcharPara("@PurchaseInvoiceId", 500, Convert.ToString(Session["TPurchaseInvoice_Id"]));
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@branchId", 500, Convert.ToString(Session["userbranchID"]));
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            ds = proc.GetDataSet();
            return ds;
        }

        #endregion Main header Section End

        #region Main BatchGrid Section  in Edit Mode

        public DataSet GetPurchaseInvoiceBataData()  // To store Value in   Session["TProductInvoiceDetails"] in Edit Mode product Batch Grid
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
            proc.AddVarcharPara("@Action", 500, "PurchaseInvoiceProductDetails");
            proc.AddVarcharPara("@PurchaseInvoiceId", 500, Convert.ToString(Session["TPurchaseInvoice_Id"]));
            ds = proc.GetDataSet();
            return ds;
        }

        public IEnumerable GetPurchaseInvoice()  // TO bind Data in Main Batch Grid
        {
            List<PurchaseInvoicedtl> PurchaseInvoiceList = new List<PurchaseInvoicedtl>();
            DataTable PurchaseInvoicedt = GetPurchaseInvoiceBataData().Tables[0];

            for (int i = 0; i < PurchaseInvoicedt.Rows.Count; i++)
            {
                PurchaseInvoicedtl PurchaseInvoices = new PurchaseInvoicedtl();

                PurchaseInvoices.SrlNo = Convert.ToString(PurchaseInvoicedt.Rows[i]["SrlNo"]);
                PurchaseInvoices.PurchaseInvoiceDetailID = Convert.ToString(PurchaseInvoicedt.Rows[i]["PurchaseInvoiceDetailID"]);
                PurchaseInvoices.ProductID = Convert.ToString(PurchaseInvoicedt.Rows[i]["ProductID"]);
                PurchaseInvoices.Description = Convert.ToString(PurchaseInvoicedt.Rows[i]["Description"]);
                PurchaseInvoices.Quantity = Convert.ToString(PurchaseInvoicedt.Rows[i]["Quantity"]);
                PurchaseInvoices.UOM = Convert.ToString(PurchaseInvoicedt.Rows[i]["UOM"]);
                PurchaseInvoices.Warehouse = "";
                PurchaseInvoices.StockQuantity = Convert.ToString(PurchaseInvoicedt.Rows[i]["StockQuantity"]);
                PurchaseInvoices.StockUOM = Convert.ToString(PurchaseInvoicedt.Rows[i]["StockUOM"]);
                PurchaseInvoices.PurchasePrice = Convert.ToString(PurchaseInvoicedt.Rows[i]["PurchasePrice"]);
                PurchaseInvoices.Discount = Convert.ToString(PurchaseInvoicedt.Rows[i]["Discount"]);
                PurchaseInvoices.Discountamt = Convert.ToString(PurchaseInvoicedt.Rows[i]["Discountamt"]);
                PurchaseInvoices.Amount = Convert.ToString(PurchaseInvoicedt.Rows[i]["Amount"]);
                PurchaseInvoices.TaxAmount = Convert.ToString(PurchaseInvoicedt.Rows[i]["TaxAmount"]);
                PurchaseInvoices.TotalAmount = Convert.ToString(PurchaseInvoicedt.Rows[i]["TotalAmount"]);
                PurchaseInvoices.ProductName = Convert.ToString(PurchaseInvoicedt.Rows[i]["ProductName"]);
                PurchaseInvoices.ComponentID = Convert.ToString(PurchaseInvoicedt.Rows[i]["ComponentID"]);
                PurchaseInvoices.ComponentNumber = Convert.ToString(PurchaseInvoicedt.Rows[i]["ComponentNumber"]);
                PurchaseInvoices.TotalQty = Convert.ToString(PurchaseInvoicedt.Rows[i]["TotalQty"]);
                PurchaseInvoices.BalanceQty = Convert.ToString(PurchaseInvoicedt.Rows[i]["BalanceQty"]);
                PurchaseInvoices.IsComponentProduct = Convert.ToString(PurchaseInvoicedt.Rows[i]["IsComponentProduct"]);
                PurchaseInvoiceList.Add(PurchaseInvoices);
            }

            return PurchaseInvoiceList;
        }

        #endregion Main BatchGrid Section in Edit Mode

        #region GetProductWiseWarehouseDetail in Edit Mode

        public DataTable GetStockWarehouseData(string stockid)
        {
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            if (Convert.ToString(ViewState["ActionType"]) == "Edit")
            {
                DataSet dsInst = new DataSet();
                DataTable dtwarehouse = objPurchaseInvoice.PopulateProductWiseWarehouseDetail(Convert.ToInt32(hdfProductIDPC.Value), Convert.ToString(Session["TPurchaseInvoice_Id"]), Convert.ToString(Session["LastCompany"]), Convert.ToInt32(hdbranchIDPC.Value));

                //dsInst.Tables=objPurchaseInvoice.PopulateProductWiseWarehouseDetail
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                //SqlCommand cmd = new SqlCommand("prc_POGetwarehousentry", con);
                //cmd.CommandType = CommandType.StoredProcedure;

                //cmd.Parameters.AddWithValue("@ProductID", Convert.ToInt32(hdfProductIDPC.Value));
                //cmd.Parameters.AddWithValue("@branchID", Convert.ToInt32(hdbranchIDPC.Value));
                //cmd.Parameters.AddWithValue("@compnay", Convert.ToString(Session["LastCompany"]));
                //cmd.Parameters.AddWithValue("@Finyear", Convert.ToString(Session["LastFinYear"]));
                //cmd.Parameters.AddWithValue("@PONumber", Convert.ToString(Session["TPurchaseInvoice_Id"]));
                //cmd.CommandTimeout = 0;
                //dsInst.Tables[0]=dtwarehouse;
                //SqlDataAdapter Adap = new SqlDataAdapter();
                ////Adap.SelectCommand = cmd;
                //Adap.Fill(dtwarehouse);
                //cmd.Dispose();
                //con.Dispose();

                //if (dsInst.Tables != null)
                if (dtwarehouse != null)
                {


                    if (Session["TPBwarehousedetailstemp"] != null)
                    {
                        DataTable Warehousedtss = (DataTable)Session["TPBwarehousedetailstemp"];

                        DataRow[] dr = Warehousedtss.Select("productid = '" + Convert.ToString(hdfProductIDPC.Value) + "' AND pcslno = '" + hdnpcslno.Value + "'");
                        if (dr.Count() > 0)
                        {

                            Warehousedtss = dr.CopyToDataTable();
                            Warehousedtss.DefaultView.Sort = "SrlNo asc";
                            Warehousedtss = Warehousedtss.DefaultView.ToTable(true);

                            Session["PCWarehouseDatabyslno"] = Warehousedtss;

                            dt2 = (DataTable)Session["PCWarehouseDatabyslno"];
                            //DataTable dtmp = dsInst.Tables[0];
                            DataTable dtmp = dtwarehouse;

                            if (Session["TPBwarehousedetailstempUpdate"] != null)
                            {
                                DataRow[] drs = Warehousedtss.Select("productid = '" + Convert.ToString(hdfProductIDPC.Value) + "' AND pcslno = '" + hdnpcslno.Value + "' AND isnew='Updated'");
                                if (drs.Count() <= 0)
                                {
                                    dt2.Merge(dtmp);
                                }
                            }

                            dt = dt2;

                        }
                        else
                        {
                            //dt = dsInst.Tables[0];
                            dt = dtwarehouse;
                        }

                    }
                    else
                    {
                        //dt = dsInst.Tables[0];
                        dt = dtwarehouse;
                    }

                }
            }
            else if (Session["TPBwarehousedetailstemp"] != null)  // to filter warehouse data 
            {
                DataTable Warehousedtss = (DataTable)Session["TPBwarehousedetailstemp"];

                DataRow[] dr = Warehousedtss.Select("productid = '" + Convert.ToString(hdfProductIDPC.Value) + "' AND pcslno = '" + hdnpcslno.Value + "'");
                //DataRow[] dr = Warehousedtss.Select(" Product_SrlNo = '" + hdnpcslno.Value + "'");

                if (dr.Count() > 0)
                {

                    Warehousedtss = dr.CopyToDataTable();
                    Warehousedtss.DefaultView.Sort = "SrlNo asc";
                    Warehousedtss = Warehousedtss.DefaultView.ToTable(true);

                    Session["PCWarehouseDatabyslno"] = Warehousedtss;

                    dt = (DataTable)Session["PCWarehouseDatabyslno"];

                }

            }

            return dt;
        }

        #endregion GetProductWiseWarehouseDetail in Edit Mode


        #endregion Edit Section End

        #region All CallBack Function Section Start

        #region  Order or Challan detail in respect of order or Challan Radio Button Section Start
        protected void ComponentQuotation_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string status = string.Empty;
            string POrderDate = string.Empty;
            string customer = string.Empty;
            string IndentIds = string.Empty;
            string ComponentType = string.Empty;
            string Action = string.Empty;
            #region IndentGrid Not applicable
            if (e.Parameter.Split('~')[0] == "BindIndentGrid")
            {

                //if (Convert.ToString(Request.QueryString["key"]) != "ADD")
                //{
                //    status = "DONE";
                //}
                //else
                //{
                //    status = "NOT-DONE";
                //}

                //lookup_quotation.GridView.Selection.UnselectAll();
                //POrderDate = e.Parameter.Split('~')[1];
                //DataTable IndentTable = objPurchaseOrderBL.GetIndentOnPO(POrderDate, status);
                //if (IndentTable.Rows.Count > 0)
                //{
                //    lookup_quotation.GridView.Selection.CancelSelection();
                //    lookup_quotation.DataSource = IndentTable;
                //    lookup_quotation.DataBind();
                //}
                //else
                //{
                //    //lookup_quotation.GridView.Selection.CancelSelection();
                //    lookup_quotation.DataSource = null;
                //    lookup_quotation.DataBind();
                //}


                //Session["IndentRequiData"] = IndentTable;
            }
            #endregion IndentGrid Not applicable
            #region Bind order or Challan Detail for tagging Start
            else if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                if (e.Parameter.Split('~')[1] != null) customer = e.Parameter.Split('~')[1];
                if (e.Parameter.Split('~')[2] != null) POrderDate = e.Parameter.Split('~')[2];
                if (e.Parameter.Split('~')[4] != null) ComponentType = e.Parameter.Split('~')[4];

                //if(ddl_numberingScheme.SelectedItem.Value!="0")
                //{
                //    string[] schemadtl = ddl_numberingScheme.SelectedItem.Value.Split('~');
                //    userbranch = Convert.ToString(schemadtl[1]);
                //}
                userbranch = ddl_Branch.SelectedValue;
                // Code Added By Sam on 19042017 to use Child Branch and Company Wise Detail Start
                //if (Session["userbranchHierarchy"] != null)
                //{
                //    userbranch = Convert.ToString(Session["userbranchHierarchy"]);
                //}
                //userbranch = ddl_Branch.SelectedItem.Value;
                // Code Added By Sam on 19042017 to use Child Branch and Company Wise Detail End
                if (ComponentType == "PO")
                {
                    Action = "GetOrder";
                    lbl_InvoiceNO.Text = "Purchase Order Date";
                }
                else if (ComponentType == "PC")
                {
                    Action = "GetChallan";
                    lbl_InvoiceNO.Text = "GRN Date";
                }

                if (e.Parameter.Split('~')[3] == "DateCheck")
                {
                    lookup_quotation.GridView.Selection.UnselectAll();
                }
                string strInvoiceID = Convert.ToString(Session["TPurchaseInvoice_Id"]);
                DataTable ComponentTable = objPurchaseInvoice.GetComponent(strInvoiceID, customer, POrderDate, ComponentType, Action, userbranch, Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]));
                lookup_quotation.GridView.Selection.CancelSelection();
                lookup_quotation.DataSource = ComponentTable;
                lookup_quotation.DataBind();

                Session["PB_ComponentData"] = ComponentTable;
            }
            #endregion Bind order or Challan Detail for tagging End
            #region Bind order Product or Challan Product Detail for tagging Start
            else if (e.Parameter.Split('~')[0] == "BindComponentGridOnSelection")//Subhabrata for binding PurchaseInvoice
            {
                if (grid_Products.GetSelectedFieldValues("ComponentDetailsID").Count != 0)
                {
                    for (int i = 0; i < grid_Products.GetSelectedFieldValues("ComponentDetailsID").Count; i++)
                    {
                        PurchaseInvoiceIds += "," + grid_Products.GetSelectedFieldValues("ComponentID")[i];
                    }
                    PurchaseInvoiceIds = PurchaseInvoiceIds.TrimStart(',');
                    lookup_quotation.GridView.Selection.UnselectAll();
                    if (!String.IsNullOrEmpty(PurchaseInvoiceIds))
                    {
                        string[] eachQuo = PurchaseInvoiceIds.Split(',');
                        if (eachQuo.Length > 1)//More tha one PurchaseInvoice
                        {
                            //txt_InvoiceDate.Text = "Multiple Select PurchaseInvoice Dates";

                            foreach (string val in eachQuo)
                            {
                                lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                            }
                        }
                        else if (eachQuo.Length == 1)//Single PurchaseInvoice
                        {
                            foreach (string val in eachQuo)
                            {
                                lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                            }
                        }
                        else//No PurchaseInvoice selected
                        {
                            lookup_quotation.GridView.Selection.UnselectAll();
                        }
                    }
                }
                else if (grid_Products.GetSelectedFieldValues("ComponentDetailsID").Count == 0)
                {
                    lookup_quotation.GridView.Selection.UnselectAll();
                }
            }
            #endregion Bind order Product or Challan Product Detail for tagging end
            else if (e.Parameter.Split('~')[0] == "DateCheckOnChanged")//Subhabrata for binding PurchaseInvoice
            {
                if (grid_Products.GetSelectedFieldValues("PurchaseInvoice_No").Count != 0)
                {
                    DateTime SalesOrderDate = Convert.ToDateTime(e.Parameter.Split('~')[2]);
                    if (lookup_quotation.GridView.GetSelectedFieldValues("Date").Count() != 0)
                    {
                        DateTime PurchaseInvoiceDate = Convert.ToDateTime(lookup_quotation.GridView.GetSelectedFieldValues("Date")[0]);
                        if (SalesOrderDate < PurchaseInvoiceDate)
                        {
                            lookup_quotation.GridView.Selection.UnselectAll();
                        }
                    }
                }
            }
            else if (e.Parameter.Split('~')[0] == "BindQuotationGridOnSelection")  // To Set the Value after Product Selection in Qotation Lookup
            {

                //if (grid_Products.GetSelectedFieldValues("Quotation_No").Count != 0)
                if (grid_Products.GetSelectedFieldValues("ComponentDetailsID").Count != 0)
                {
                    for (int i = 0; i < grid_Products.GetSelectedFieldValues("ComponentID").Count; i++)
                    {
                        IndentIds += "," + grid_Products.GetSelectedFieldValues("ComponentID")[i];
                    }
                    IndentIds = IndentIds.TrimStart(',');
                    //lookup_quotation.GridView.Selection.UnselectAll();
                    if (!String.IsNullOrEmpty(IndentIds))
                    {
                        string[] eachQuo = IndentIds.Split(',');
                        if (eachQuo.Length > 1)//More tha one quotation
                        {
                            dt_Quotation.Text = "Multiple Select Quotation Dates";
                            //BindLookUp(Customer_Id, Order_Date);
                            foreach (string val in eachQuo)
                            {
                                lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                            }
                        }
                        else if (eachQuo.Length == 1)//Single Quotation
                        {
                            foreach (string val in eachQuo)
                            {
                                lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                            }
                        }
                        else//No Quotation selected
                        {
                            dt_Quotation.Text = "";
                            lookup_quotation.GridView.Selection.UnselectAll();
                        }
                    }
                }
                else if (grid_Products.GetSelectedFieldValues("ComponentDetailsID").Count == 0)
                {
                    if (Convert.ToString(e.Parameter.Split('~')[1]) != "Edit")
                    {
                        lookup_quotation.GridView.Selection.UnselectAll();
                        dt_Quotation.Text = "";
                    }
                }
            }

        }

        #endregion  Order or Challan detail in respect of order or Challan Radio Button Section Start

        #region  Not Defined Yet
        protected void ComponentDatePanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            if (strSplitCommand == "BindQuotationDate")
            {
                string partyInvNo = "";
                string PartyInvoiceDate = "";
                string reff = "";
                string curr = "";
                string rate = "";
                string person = "";
                string amtare = "";
                string taxcode = "";
                ComponentDatePanel.JSProperties["cppartydetail"] = null;
                string docNo = Convert.ToString(e.Parameter.Split('~')[1]);
                string doctype = "";
                if (rdl_PurchaseInvoice.SelectedValue == "PO")
                {
                    doctype = "PO";
                }
                else if (rdl_PurchaseInvoice.SelectedValue == "PC")
                {
                    doctype = "PC";
                }

                DataSet popcdst = new DataSet();
                popcdst = objPurchaseInvoice.GetFirstPOPCHeaderDtl(doctype, docNo);
                //DataTable dt_indentDetails = objPurchaseOrderBL.GetIndentRequisitionDate(Indent_No);
                if (popcdst.Tables[0] != null && popcdst.Tables[0].Rows.Count > 0)
                {
                    string quotationdate = Convert.ToString(popcdst.Tables[0].Rows[0]["docdate"]);
                    if (!string.IsNullOrEmpty(quotationdate))
                    {
                        dt_Quotation.Text = Convert.ToString(quotationdate);
                    }
                    partyInvNo = Convert.ToString(popcdst.Tables[0].Rows[0]["PartyInvoiceNo"]);
                    PartyInvoiceDate = Convert.ToString(popcdst.Tables[0].Rows[0]["PartyInvoiceDate"]);
                    if (PartyInvoiceDate != null && PartyInvoiceDate != "")
                    {
                        if (Convert.ToDateTime(PartyInvoiceDate) != DateTime.MinValue)
                        {
                            DateTime pdt = Convert.ToDateTime(PartyInvoiceDate);
                            //dt_partyInvDt.Date =
                            PartyInvoiceDate = pdt.ToString("dd-MM-yyyy");
                        }
                    }

                    ComponentDatePanel.JSProperties["cppartydetail"] = partyInvNo + "~" + PartyInvoiceDate;
                }
                if (popcdst.Tables[1] != null && popcdst.Tables[1].Rows.Count > 0)
                {
                    reff = Convert.ToString(popcdst.Tables[1].Rows[0]["ref"]);
                    curr = Convert.ToString(popcdst.Tables[1].Rows[0]["curr"]);
                    rate = Convert.ToString(popcdst.Tables[1].Rows[0]["rate"]);
                    person = Convert.ToString(popcdst.Tables[1].Rows[0]["person"]);
                    amtare = Convert.ToString(popcdst.Tables[1].Rows[0]["amtare"]);


                    taxcode = Convert.ToString(popcdst.Tables[1].Rows[0]["taxcode"]);
                    if (amtare == "2")
                    {
                        PopulateGSTCSTVAT("2");
                        //setValueForHeaderGST(ddl_VatGstCst, taxcode);
                    }

                    //txt_Refference.Text = Convert.ToString(popcdst.Tables[1].Rows[0]["ref"]);
                    //ddl_Currency.SelectedValue = Convert.ToString(popcdst.Tables[1].Rows[0]["curr"]);
                    //txt_Rate.Text = Convert.ToString(popcdst.Tables[1].Rows[0]["rate"]);
                    //cmbContactPerson.Value = Convert.ToString(popcdst.Tables[1].Rows[0]["person"]);
                    //ddl_AmountAre.Value = Convert.ToString(popcdst.Tables[1].Rows[0]["amtare"]);
                    //ddl_VatGstCst.Value = Convert.ToString(popcdst.Tables[1].Rows[0]["taxcode"]);
                    //txt_Refference.Text = Convert.ToString(popcdst.Tables[1].Rows[0][""]);
                }
                ComponentDatePanel.JSProperties["cppartydetail"] = partyInvNo + "~" + PartyInvoiceDate + "~" + reff + "~" + curr + "~" + rate + "~" + person + "~" + amtare + "~" + taxcode;

            }
        }

        #endregion  Not Defined Yet

        #region Product Detail in respect of Order or Challan Popup Section Start
        protected void cgridProducts_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];
            #region Bind Product Look up
            if (strSplitCommand == "BindProductsDetails")
            {

                string Quote_Nos = Convert.ToString(e.Parameters.Split('~')[1]);
                String QuoComponent = "";
                List<object> QuoList = lookup_quotation.GridView.GetSelectedFieldValues("ComponentID");
                foreach (object Quo in QuoList)
                {
                    QuoComponent += "," + Quo;
                }
                QuoComponent = QuoComponent.TrimStart(',');
                if (Quote_Nos != "$")
                {
                    string strAction = "";
                    string strType = Convert.ToString(rdl_PurchaseInvoice.SelectedValue);
                    if (strType == "PO")
                    {
                        strAction = "GetOrderProducts";
                        grid_Products.Columns["ComponentNumber"].Caption = "Order No";
                    }
                    else if (strType == "PC")
                    {
                        strAction = "GetChallanProducts";
                        grid_Products.Columns["ComponentNumber"].Caption = "GRN No";
                    }

                    string strInvoiceID = Convert.ToString(Session["TPurchaseInvoice_Id"]);
                    DataTable dtDetails = objPurchaseInvoice.GetComponentProductList(strAction, QuoComponent, strInvoiceID);

                    grid_Products.DataSource = dtDetails;
                    grid_Products.DataBind();
                    if (grid_Products.VisibleRowCount > 0)
                    {
                        for (int i = 0; i < grid_Products.VisibleRowCount; i++)
                        {
                            grid_Products.Selection.SelectRow(i);
                        }
                        if (rdl_PurchaseInvoice.Items.Count > 1)
                        {
                            if (rdl_PurchaseInvoice.Items[1].Selected)
                            {
                                grid_Products.JSProperties["cpSelectHide"] = "Y";
                                grid_Products.Enabled = false;
                                //divselectunselect.Visible = false;
                                //divselectunselect.Attributes.Add("visibility", "hidden");
                                //divselectunselect.Attributes.Add("display", "none");
                            }
                            else
                            {
                                grid_Products.JSProperties["cpSelectHide"] = "N";
                                grid_Products.Enabled = true;
                                //divselectunselect.Visible = true;
                                //divselectunselect.Attributes.Add("display", "block");
                            }
                        }
                        else
                        {
                            if (rdl_PurchaseInvoice.Items[0].Selected)
                            {
                                grid_Products.JSProperties["cpSelectHide"] = "Y";
                                grid_Products.Enabled = false;
                                //divselectunselect.Visible = false;
                                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "showhide('Y');", true);
                                //divselectunselect.Attributes.Add("visibility", "hidden");

                            }
                            else
                            {
                                grid_Products.JSProperties["cpSelectHide"] = "N";
                                grid_Products.Enabled = true;
                                //divselectunselect.Visible = true;
                                //divselectunselect.Attributes.Add("display", "block");
                            }
                        }

                    }
                }
                else
                {
                    grid_Products.DataSource = null;
                    grid_Products.DataBind();
                }

            }
            #endregion Bind Product Look up
            #region Select and Deselect All Products
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
            #endregion Select and Deselect All Products
        }

        #endregion Product Detail in respect of Order or Challan Popup Section Start

        #region  Available Stock
        protected void acpAvailableStock_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string performpara = e.Parameter;
            string strProductID = Convert.ToString(performpara.Split('~')[0]);
            string strBranch = Convert.ToString(ddl_Branch.SelectedValue);
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
        #endregion Available Stock

        #endregion All CallBack Function Section End

        #region Bind Warehouse and Tax detail after tagging by order or challan

        //public DataTable GetTaggingWarehouseData(string ComponentDetailsIDs, string strType)
        //{
        //    //try
        //    //{
        //    //    DataTable dt = new DataTable();
        //    //    ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
        //    //    proc.AddVarcharPara("@Action", 500, "ComponentWarehouse");
        //    //    proc.AddVarcharPara("@SelectedComponentList", 2000, ComponentDetailsIDs);
        //    //    proc.AddVarcharPara("@ComponentType", 10, strType);
        //    //    dt = proc.GetTable();
        //    //    return dt;
        //    //    #region Code Commented By sam on 07062017 due to use SetWareHouseDataInEditMode() function to arrange data Start.
        //    //    //string strNewVal = "", strOldVal = "";
        //    //    //DataTable tempdt = dt.Copy();
        //    //    //foreach (DataRow drr in tempdt.Rows)
        //    //    //{
        //    //    //    strNewVal = Convert.ToString(drr["WarehouseID"]);

        //    //    //    if (strNewVal == strOldVal)
        //    //    //    {
        //    //    //        drr["WarehouseName"] = "";
        //    //    //        drr["Quantity"] = "";
        //    //    //        drr["BatchNo"] = "";
        //    //    //        drr["SalesUOMName"] = "";
        //    //    //        drr["SalesQuantity"] = "";
        //    //    //        drr["StkUOMName"] = "";
        //    //    //        drr["StkQuantity"] = "";
        //    //    //        drr["ConversionMultiplier"] = "";
        //    //    //        drr["AvailableQty"] = "";
        //    //    //        drr["BalancrStk"] = "";
        //    //    //        drr["MfgDate"] = "";
        //    //    //        drr["ExpiryDate"] = "";
        //    //    //    } 
        //    //    //    strOldVal = strNewVal;
        //    //    //}

        //    //    //Session["SI_LoopWarehouse"] = Convert.ToString(Convert.ToInt32(strNewVal) + 1);
        //    //    ////tempdt.Columns.Remove("WarehouseID");
        //    //    //return tempdt;
        //    //    #endregion Code Commented By sam on 07062017 due to use SetWareHouseDataInEditMode() function to arrange data End.
        //    //}
        //    //catch
        //    //{
        //    //    return null;
        //    //}
        //}

        public DataTable GetComponentEditedTaxData(string ComponentDetailsIDs, string strType)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
            proc.AddVarcharPara("@Action", 500, "ComponentProductTax");
            proc.AddVarcharPara("@SelectedComponentList", 500, ComponentDetailsIDs);
            proc.AddVarcharPara("@ComponentType", 500, strType);
            ds = proc.GetTable();
            return ds;
        }

        #endregion

        #region Trash Code Start

        //public IEnumerable GetPurchaseInvoice(DataTable Quotationdt)
        //{
        //    List<PurchaseOrderList> PurchaseOrderList = new List<PurchaseOrderList>();
        //    // DataTable Quotationdt = GetPurchaseOrderData().Tables[0];
        //    DataColumnCollection dtC = Quotationdt.Columns;
        //    for (int i = 0; i < Quotationdt.Rows.Count; i++)
        //    {
        //        PurchaseOrderList PurchaseOrders = new PurchaseOrderList();

        //        PurchaseOrders.SrlNo = Convert.ToString(Quotationdt.Rows[i]["SrlNo"]);
        //        PurchaseOrders.OrderDetails_Id = Convert.ToString(Quotationdt.Rows[i]["PurchaseInvoiceID"]);
        //        PurchaseOrders.ProductID = Convert.ToString(Quotationdt.Rows[i]["ProductID"]);
        //        PurchaseOrders.gvColDiscription = Convert.ToString(Quotationdt.Rows[i]["Description"]);
        //        PurchaseOrders.Quantity = Convert.ToString(Quotationdt.Rows[i]["Quantity"]);
        //        PurchaseOrders.gvColUOM = Convert.ToString(Quotationdt.Rows[i]["UOM"]);
        //        PurchaseOrders.Warehouse = "";
        //        PurchaseOrders.gvColStockQty = Convert.ToString(Quotationdt.Rows[i]["StockQuantity"]);
        //        PurchaseOrders.gvColStockUOM = Convert.ToString(Quotationdt.Rows[i]["StockUOM"]);
        //        PurchaseOrders.PurchasePrice = Convert.ToString(Quotationdt.Rows[i]["PurchasePrice"]);
        //        PurchaseOrders.Discount = Convert.ToString(Quotationdt.Rows[i]["Discount"]);
        //        PurchaseOrders.Amount = Convert.ToString(Quotationdt.Rows[i]["Amount"]);
        //        PurchaseOrders.TaxAmount = Convert.ToString(Quotationdt.Rows[i]["TaxAmount"]);
        //        PurchaseOrders.TotalAmount = Convert.ToString(Quotationdt.Rows[i]["TotalAmount"]);
        //        if (!string.IsNullOrEmpty(Convert.ToString(Quotationdt.Rows[i]["Indent_No"])))
        //        { PurchaseOrders.Indent = Convert.ToInt64(Quotationdt.Rows[i]["Indent_No"]); }
        //        else
        //        { PurchaseOrders.Indent = 0; }
        //        if (dtC.Contains("Indent_Num"))
        //        {
        //            PurchaseOrders.Indent_Num = Convert.ToString(Quotationdt.Rows[i]["Indent_Num"]);
        //        }
        //        PurchaseOrders.ProductName = Convert.ToString(Quotationdt.Rows[i]["ProductName"]);

        //        PurchaseOrderList.Add(PurchaseOrders);
        //    }

        //    return PurchaseOrderList;
        //}
        //public bool AddModifyPurchaseOrder(string MainPurchaseInvoiceID, string UniquePurchaseInvoice, string strQuoteDate, string IndentRequisitionNo, string IndentRequisitionDate,

        //    string strVendor, string strContactName,
        //                           string Reference, string strBranch, string strAgents, string strCurrency, string strRate, string strTaxType,
        //    string strTaxCode, string CompanyID, int BaseCurrencyId, DataTable PurchaseInvoicedt,
        //                            string ActionType, DataTable Warehousedt, DataTable TaxDetailTable, DataTable PurchaseOrderTaxdt,DataTable tempBillAddress)
        //{
        //    try
        //    {
        //        DataSet dsInst = new DataSet();
        //        SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //        SqlCommand cmd = new SqlCommand("prc_PurchaseOrder", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@Action", ActionType);
        //        cmd.Parameters.AddWithValue("@PurchaseOrderEdit_Id", MainPurchaseInvoiceID);

        //        cmd.Parameters.AddWithValue("@PurchaseOrder_CompanyID", Convert.ToString(Session["LastCompany"]));
        //        cmd.Parameters.AddWithValue("@PurchaseOrder_BranchId", strBranch);
        //        cmd.Parameters.AddWithValue("@PurchaseOrder_FinYear", Convert.ToString(Session["LastFinYear"]));
        //        cmd.Parameters.AddWithValue("@PurchaseOrder_Number", UniquePurchaseInvoice);
        //        cmd.Parameters.AddWithValue("@PurchaseOrder_IndentIds", IndentRequisitionNo);
        //        if (!String.IsNullOrEmpty(IndentRequisitionDate))
        //        { cmd.Parameters.AddWithValue("@PurchaseOrder_IndentDate", IndentRequisitionDate); }

        //        cmd.Parameters.AddWithValue("@PurchaseOrder_Date", strQuoteDate);
        //        cmd.Parameters.AddWithValue("@PurchaseOrder_VendorId", strVendor);
        //        cmd.Parameters.AddWithValue("@Contact_Person_Id", strContactName);
        //        cmd.Parameters.AddWithValue("@PurchaseOrder_Reference", Reference);
        //        cmd.Parameters.AddWithValue("@PurchaseOrder_Currency_Id", BaseCurrencyId);
        //        cmd.Parameters.AddWithValue("@Currency_Conversion_Rate", strRate);
        //        cmd.Parameters.AddWithValue("@Tax_Option", strTaxType);
        //        //cmd.Parameters.AddWithValue("@Tax_Code", strTaxCode);
        //        cmd.Parameters.AddWithValue("@Tax_Code", 0);
        //        cmd.Parameters.AddWithValue("@PurchaseOrder_SalesmanId", strAgents);

        //        cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(Session["userid"]));

        //        cmd.Parameters.AddWithValue("@PurchaseOrderDetails", PurchaseInvoicedt);
        //        cmd.Parameters.AddWithValue("@TaxDetail", TaxDetailTable);
        //        cmd.Parameters.AddWithValue("@PurchaseOrderTax", PurchaseOrderTaxdt);
        //        cmd.Parameters.AddWithValue("@PurchaseOrderAddress", tempBillAddress);

        //        if (Session["TPBwarehousedetailstemp"] != null)
        //        {
        //            DataTable temtable = new DataTable();
        //            DataTable Warehousedtssss = (DataTable)Session["TPBwarehousedetailstemp"];

        //            temtable = Warehousedtssss.DefaultView.ToTable(false, "SrlNo", "BatchWarehouseID", "BatchWarehousedetailsID", "BatchID", "SerialID", "WarehouseID", "WarehouseName", "BatchNo", "SerialNo", "MFGDate", "ExpiryDate", "Quantitysum", "productid", "Inventrytype", "StockID", "isnew");
        //            cmd.Parameters.AddWithValue("@udt_StockOpeningwarehousentrie", temtable);
        //        }

        //        SqlParameter output = new SqlParameter("@ReturnValue", SqlDbType.Int);
        //        output.Direction = ParameterDirection.Output;
        //        cmd.Parameters.Add(output);


        //        cmd.CommandTimeout = 0;
        //        SqlDataAdapter Adap = new SqlDataAdapter();
        //        Adap.SelectCommand = cmd;
        //        Adap.Fill(dsInst);
        //        cmd.Dispose();
        //        con.Dispose();

        //        //Udf Add mode
        //        DataTable udfTable = (DataTable)Session["UdfDataOnAdd"];
        //        if (udfTable != null)
        //            Session["UdfDataOnAdd"] = reCat.insertRemarksCategoryAddMode("PO", "PurchaseOrder" + Convert.ToString(output.Value), udfTable, Convert.ToString(Session["userid"]));
        //        #region warehouse Update and delete

        //        updatewarehouse();
        //        deleteALL();

        //        #endregion

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        //public void GetAllDropDownDetailForPurchaseOrder()    by sam on 06042017
        //{
        //    DataSet dst = new DataSet();
        //    dst = objPurchaseOrderBL.GetAllDropDownDetailForPurchaseOrder(); 
        //    if (dst.Tables[3] != null && dst.Tables[3].Rows.Count > 0)
        //    {
        //        ddl_AmountAre.TextField = "taxGrp_Description";
        //        ddl_AmountAre.ValueField = "taxGrp_Id";
        //        ddl_AmountAre.DataSource = dst.Tables[3];
        //        ddl_AmountAre.DataBind();
        //    } 
        //}


        //DataTable IndentTable = objPurchaseOrderBL.GetIndentOnPO(OrderDate, status);
        //if (IndentTable.Rows.Count > 0)
        //{
        //    lookup_quotation.GridView.Selection.CancelSelection();
        //    lookup_quotation.DataSource = IndentTable;
        //    lookup_quotation.DataBind();
        //}
        //else
        //{
        //    //lookup_quotation.GridView.Selection.CancelSelection();
        //    lookup_quotation.DataSource = null;
        //    lookup_quotation.DataBind();
        //}



        //Session["IndentRequiData"] = IndentTable;


        //public void SetFinYearCurrentDate()
        //{
        //    dt_PLQuote.EditFormatString = objConverter.GetDateFormat("Date");
        //    string fDate = null;

        //    //DateTime dt = DateTime.ParseExact("3/31/2016", "MM/dd/yyy", CultureInfo.InvariantCulture);
        //    string[] FinYEnd = Convert.ToString(Session["FinYearEnd"]).Split(' ');
        //    string FinYearEnd = FinYEnd[0];

        //    DateTime date3 = DateTime.ParseExact(FinYearEnd, "M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);

        //    ForJournalDate = Convert.ToString(date3);

        //    //ForJournalDate =Session["FinYearEnd"].ToString();
        //    int month = oDBEngine.GetDate().Month;
        //    int date = oDBEngine.GetDate().Day;
        //    int Year = oDBEngine.GetDate().Year;

        //    if (date3 < oDBEngine.GetDate().Date)
        //    {
        //        fDate = Convert.ToString(Convert.ToDateTime(ForJournalDate).Month) + "/" + Convert.ToString(Convert.ToDateTime(ForJournalDate).Day) + "/" + Convert.ToString(Convert.ToDateTime(ForJournalDate).Year);
        //    }
        //    else
        //    {
        //        fDate = Convert.ToString(Convert.ToDateTime(oDBEngine.GetDate().Date).Month) + "/" + Convert.ToString(Convert.ToDateTime(oDBEngine.GetDate().Date).Day) + "/" + Convert.ToString(Convert.ToDateTime(oDBEngine.GetDate().Date).Year);
        //    }

        //    dt_PLQuote.Value = DateTime.ParseExact(fDate, @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        //}


        //public class PurchaseOrderList
        //{
        //    public string SrlNo { get; set; }
        //    public string OrderDetails_Id { get; set; }
        //    public string ProductID { get; set; }
        //    public string gvColDiscription { get; set; }
        //    public string Quantity { get; set; }
        //    public string gvColUOM { get; set; }
        //    public string Warehouse { get; set; }
        //    public string gvColStockQty { get; set; }
        //    public string gvColStockUOM { get; set; }
        //    public string PurchasePrice { get; set; }
        //    public string Discount { get; set; }
        //    public string Amount { get; set; }
        //    public string TaxAmount { get; set; }
        //    public string TotalAmount { get; set; }
        //    public string Quotation_No { get; set; }
        //    public string ProductName { get; set; }
        //    public string Indent_Num { get; set; }
        //    public Int64 Indent { get; set; }


        //}

        //public void FillGrid()
        //{
        //    DataTable PurchaseOrderEditdt = GetPurchaseOrderEditData();
        //    if (PurchaseOrderEditdt != null && PurchaseOrderEditdt.Rows.Count > 0)
        //    {
        //        string PurchaseOrder_Number = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrder_Number"]);//0
        //        string PurchaseOrder_IndentId = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrder_IndentIds"]);//1
        //        string PurchaseOrder_BranchId = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrder_BranchId"]);//2
        //        string PurchaseOrder_Date = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrder_Date"]);//3

        //        string Customer_Id = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrder_VendorId"]);//5
        //        string Contact_Person_Id = Convert.ToString(PurchaseOrderEditdt.Rows[0]["Contact_Person_Id"]);//6
        //        string PurchaseOrder_Reference = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrder_Reference"]);//7
        //        string PurchaseOrder_Currency_Id = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrder_Currency_Id"]);//8
        //        string Currency_Conversion_Rate = Convert.ToString(PurchaseOrderEditdt.Rows[0]["Currency_Conversion_Rate"]);//9
        //        string Tax_Option = Convert.ToString(PurchaseOrderEditdt.Rows[0]["Tax_Option"]);//10
        //        string Tax_Code = Convert.ToString(PurchaseOrderEditdt.Rows[0]["Tax_Code"]);//11
        //        string PurchaseOrder_SalesmanId = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrder_SalesmanId"]);//12
        //        string PurchaseOrder_IndentDate = Convert.ToString(PurchaseOrderEditdt.Rows[0]["IndentDate"]);//13


        //        txtVoucherNo.Text = PurchaseOrder_Number;
        //        dt_PLQuote.Date = Convert.ToDateTime(PurchaseOrder_Date);
        //        // ddl_IndentRequisitionNo.SelectedValue = PurchaseOrder_IndentId;
        //        if (!string.IsNullOrEmpty(PurchaseOrder_IndentDate))
        //        { 
        //            //txtDateIndentRequis.Date = Convert.ToDateTime(PurchaseOrder_IndentDate); 
        //            dt_Quotation.Text = PurchaseOrder_IndentDate; 
        //        }

        //        // ddl_Vendor.SelectedValue = Customer_Id;
        //        lookup_Customer.GridView.Selection.SelectRowByKey(Customer_Id);
        //        if (Customer_Id != "")
        //        {
        //            hdfLookupCustomer.Value = Customer_Id;
        //        }
        //        hdnCustomerId.Value = Customer_Id;
        //        TabPage page = ASPxPageControl1.TabPages.FindByName("Billing/Shipping");
        //        page.ClientEnabled = true;
        //        PopulateContactPersonOfCustomer(Customer_Id);
        //        if (Contact_Person_Id!="0")
        //        {
        //            cmbContactPerson.Value = Contact_Person_Id;
        //        }

        //        txt_Refference.Text = PurchaseOrder_Reference;
        //        ddl_Branch.SelectedValue = PurchaseOrder_BranchId;
        //        //ddl_SalesAgent.SelectedValue = PurchaseOrder_SalesmanId;
        //        ddl_Currency.SelectedValue = PurchaseOrder_Currency_Id;
        //        txt_Rate.Text = Currency_Conversion_Rate;

        //        //Subhabrata
        //        string Quoids = Convert.ToString(PurchaseOrderEditdt.Rows[0]["PurchaseOrder_IndentIds"]);

        //        if (!String.IsNullOrEmpty(Quoids))
        //        {
        //            string[] eachQuo = Quoids.Split(',');
        //            if (eachQuo.Length > 1)//More tha one quotation
        //            {
        //                dt_Quotation.Text = "Multiple Select Indent Dates";
        //                BindLookUp(Convert.ToString(dt_PLQuote.Date));
        //                foreach (string val in eachQuo)
        //                {
        //                    lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
        //                }
        //                // lbl_MultipleDate.Attributes.Add("style", "display:block");
        //            }
        //            else if (eachQuo.Length == 1)//Single Quotation
        //            { //lbl_MultipleDate.Attributes.Add("style", "display:none"); }
        //                BindLookUp(Convert.ToString(dt_PLQuote.Date));
        //                foreach (string val in eachQuo)
        //                {
        //                    lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
        //                }
        //            }
        //            else//No Quotation selected
        //            {
        //                BindLookUp(Convert.ToString(dt_PLQuote.Date));
        //            }
        //        }
        //        //End

        //        if (Tax_Option != "0")
        //        {
        //            ddl_AmountAre.Value = Tax_Option;
        //        }
        //        PopulateGSTCSTVAT("2");
        //        if (Tax_Code != "0")
        //        {
        //            PopulateGSTCSTVAT("2");
        //            setValueForHeaderGST(ddl_VatGstCst, Tax_Code);
        //        }
        //        else
        //        {
        //            PopulateGSTCSTVAT("2");
        //            ddl_VatGstCst.SelectedIndex = 0;
        //        }
        //        ddl_AmountAre.ClientEnabled = false;
        //        ddl_VatGstCst.ClientEnabled = false;

        //    }


        //}

        //public IEnumerable GetPurchaseInvoice()
        //{
        //    List<PurchaseInvoicedtl> PurchaseOrderList = new List<PurchaseInvoicedtl>();
        //    DataTable Quotationdt = GetPurchaseOrderData().Tables[0];

        //    for (int i = 0; i < Quotationdt.Rows.Count; i++)
        //    {
        //        PurchaseInvoicedtl PurchaseOrders = new PurchaseInvoicedtl();

        //        PurchaseOrders.SrlNo = Convert.ToString(Quotationdt.Rows[i]["SrlNo"]);
        //        PurchaseOrders.OrderDetails_Id = Convert.ToString(Quotationdt.Rows[i]["PurchaseInvoiceID"]);
        //        PurchaseOrders.ProductID = Convert.ToString(Quotationdt.Rows[i]["ProductID"]);
        //        PurchaseOrders.gvColDiscription = Convert.ToString(Quotationdt.Rows[i]["Description"]);
        //        PurchaseOrders.Quantity = Convert.ToString(Quotationdt.Rows[i]["Quantity"]);
        //        PurchaseOrders.gvColUOM = Convert.ToString(Quotationdt.Rows[i]["UOM"]);
        //        PurchaseOrders.Warehouse = "";
        //        PurchaseOrders.gvColStockQty = Convert.ToString(Quotationdt.Rows[i]["StockQuantity"]);
        //        PurchaseOrders.gvColStockUOM = Convert.ToString(Quotationdt.Rows[i]["StockUOM"]);
        //        PurchaseOrders.PurchasePrice = Convert.ToString(Quotationdt.Rows[i]["PurchasePrice"]);
        //        PurchaseOrders.Discount = Convert.ToString(Quotationdt.Rows[i]["Discount"]);
        //        PurchaseOrders.Amount = Convert.ToString(Quotationdt.Rows[i]["Amount"]);
        //        PurchaseOrders.TaxAmount = Convert.ToString(Quotationdt.Rows[i]["TaxAmount"]);
        //        PurchaseOrders.TotalAmount = Convert.ToString(Quotationdt.Rows[i]["TotalAmount"]);
        //        PurchaseOrders.Quotation_No = Convert.ToString(0);
        //        PurchaseOrders.ProductName = Convert.ToString(Quotationdt.Rows[i]["ProductName"]);
        //        PurchaseOrderList.Add(PurchaseOrders);
        //    }

        //    return PurchaseOrderList;
        //}

        //public DataTable GetPurchaseOrderEditData()
        //{
        //    DataTable dt = new DataTable();
        //    ProcedureExecute proc = new ProcedureExecute("prc_PurchaseOrderDetailsList");
        //    proc.AddVarcharPara("@Action", 500, "PurchaseOrderEditDetails");
        //    proc.AddIntegerPara("@PurchaseOrder_Id", Convert.ToInt32(Session["TPurchaseInvoice_Id"]));
        //    //proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
        //    //proc.AddVarcharPara("@branchId", 500, Convert.ToString(Session["userbranchID"]));
        //    //proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
        //    dt = proc.GetTable();
        //    return dt;
        //}
        #endregion Trash Code End

        #region Unique Code Generated Section Start

        public string checkNMakeJVCode(string manual_str, int sel_schema_Id)
        {

         //   oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            oDBEngine = new BusinessLogicLayer.DBEngine();


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

                    sqlQuery = "SELECT max(tjv.Invoice_Number) FROM tbl_trans_TransitPurchaseInvoice tjv WHERE dbo.RegexMatch('";
                    if (prefLen > 0)
                        sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                    else if (scheme_type == 2)
                        sqlQuery += "^";
                    sqlQuery += "[0-9]{" + paddCounter + "}";
                    if (sufxLen > 0)
                        sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                    sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1 and Invoice_Number like '" + prefCompCode + "%'";
                    if (scheme_type == 2)
                        sqlQuery += " AND CONVERT(DATE, JournalVoucher_CreateDateTime) = CONVERT(DATE, GETDATE())";
                    dtC = oDBEngine.GetDataTable(sqlQuery);

                    if (dtC.Rows[0][0].ToString() == "")
                    {
                        sqlQuery = "SELECT max(tjv.Invoice_Number) FROM tbl_trans_TransitPurchaseInvoice tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1 and Invoice_Number like '" + prefCompCode + "%'"; ;
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, JournalVoucher_CreateDateTime) = CONVERT(DATE, GETDATE())";
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
                            UniquePurchaseInvoice = prefCompCode + paddedStr + sufxCompCode;
                            return "ok";
                        }
                    }
                    else
                    {
                        UniquePurchaseInvoice = startNo.PadLeft(paddCounter, '0');
                        UniquePurchaseInvoice = prefCompCode + paddedStr + sufxCompCode;
                        return "ok";
                    }
                }
                else
                {
                    sqlQuery = "SELECT CashBank_VoucherNumber FROM Trans_CashBankVouchers WHERE CashBank_VoucherNumber LIKE '" + manual_str.Trim() + "'";
                    dtC = oDBEngine.GetDataTable(sqlQuery);
                    // duplicate manual entry check
                    if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
                    {
                        return "duplicate";
                    }

                    UniquePurchaseInvoice = manual_str.Trim();
                    return "ok";
                }
            }
            else
            {
                return "noid";
            }
        }

        #endregion Unique Code Generated Section End

        #region Class Section Start

        #region Main Batchgrid  Section Start
        public class PurchaseInvoicedtl
        {
            public string SrlNo { get; set; }
            public string PurchaseInvoiceID { get; set; }

            public string PurchaseInvoiceDetailID { get; set; }
            public string ProductID { get; set; }
            public string Description { get; set; }
            public string Quantity { get; set; }
            public string UOM { get; set; }
            public string Warehouse { get; set; }
            public string StockQuantity { get; set; }
            public string StockUOM { get; set; }
            public string PurchasePrice { get; set; }
            public string Discount { get; set; }
            public string Discountamt { get; set; }
            public string Amount { get; set; }
            public string TaxAmount { get; set; }
            public string TotalAmount { get; set; }
            public string ProductName { get; set; }

            public string ComponentID { get; set; }
            public string ComponentNumber { get; set; }
            public string TotalQty { get; set; }
            public string BalanceQty { get; set; }
            public string IsComponentProduct { get; set; }
        }

        #endregion Main Batchgrid  Section End

        #region Not define Yet
        public class SalesOrder
        {
            public string SrlNo { get; set; }
            public string OrderDetails_Id { get; set; }
            public string ProductID { get; set; }
            public string gvColDiscription { get; set; }
            public string Quantity { get; set; }
            public string gvColUOM { get; set; }
            public string Warehouse { get; set; }
            public string gvColStockQty { get; set; }
            public string gvColStockUOM { get; set; }
            public string PurchasePrice { get; set; }
            public string Discount { get; set; }
            public string Amount { get; set; }
            public string TaxAmount { get; set; }
            public string TotalAmount { get; set; }
            public Int64 Quotation_No { get; set; }
            public string Quotation_Num { get; set; }
            public string Key_UniqueId { get; set; }
            public string Product_Shortname { get; set; }
            public string ProductName { get; set; }
            public string QuoteDetails_Id { get; set; }
            public string Indent_Num { get; set; }
            public Int64 Indent { get; set; }
        }

        #endregion Not define Yet


        public class Product
        {
            public string ProductID { get; set; }
            public string ProductName { get; set; }
        }

        #region Tax Section Start

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
        class taxCode
        {
            public string Taxes_ID { get; set; }
            public string Taxes_Name { get; set; }
        }
        public class Taxes
        {
            public string TaxID { get; set; }
            public string TaxName { get; set; }
            public string Percentage { get; set; }
            public string Amount { get; set; }
            public decimal calCulatedOn { get; set; }
        }

        #endregion Tax Section End


        #endregion Class Section End

        #region Subhabrata/SessionBind
        //Subhabrata on 02-03-2017
        public bool BindSessionByDatatable(DataTable dt)
        {
            bool IsSuccess = false;
            DataTable purChaseDT = new DataTable();
            purChaseDT.Columns.Add("SrlNo", typeof(string));
            purChaseDT.Columns.Add("PurchaseInvoiceID", typeof(string));
            purChaseDT.Columns.Add("ProductID", typeof(string));
            purChaseDT.Columns.Add("Description", typeof(string));
            //SalesOrderdt.Columns.Add("Quotation", typeof(string));//Added By:subhabrata on 21-02-2017               
            purChaseDT.Columns.Add("Quantity", typeof(string));
            purChaseDT.Columns.Add("UOM", typeof(string));
            purChaseDT.Columns.Add("Warehouse", typeof(string));
            purChaseDT.Columns.Add("StockQuantity", typeof(string));
            purChaseDT.Columns.Add("StockUOM", typeof(string));
            //purChaseDT.Columns.Add("SalePrice", typeof(string));
            purChaseDT.Columns.Add("PurchasePrice", typeof(string));
            purChaseDT.Columns.Add("Discount", typeof(string));
            purChaseDT.Columns.Add("Amount", typeof(string));
            purChaseDT.Columns.Add("TaxAmount", typeof(string));
            purChaseDT.Columns.Add("TotalAmount", typeof(string));
            purChaseDT.Columns.Add("Status", typeof(string));
            purChaseDT.Columns.Add("Indent_No", typeof(string));
            purChaseDT.Columns.Add("Indent_Num", typeof(string));
            purChaseDT.Columns.Add("ProductName", typeof(string));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IsSuccess = true;
                DataColumnCollection dtC = dt.Columns;
                string SalePrice, Discount, Amount, TaxAmount, TotalAmount, Order_Num1, ProductName;
                //if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["SalePrice"])))
                //{ SalePrice = Convert.ToString(dt.Rows[i]["SalePrice"]); }
                //else
                //{ SalePrice = ""; }
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Discount"])))
                { Discount = Convert.ToString(dt.Rows[i]["Discount"]); }
                else
                { Discount = ""; }
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Amount"])))
                { Amount = Convert.ToString(dt.Rows[i]["Amount"]); }
                else { Amount = ""; }
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["TaxAmount"])))
                { TaxAmount = Convert.ToString(dt.Rows[i]["TaxAmount"]); }
                else { TaxAmount = ""; }
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["TotalAmount"])))
                { TotalAmount = Convert.ToString(dt.Rows[i]["TotalAmount"]); }
                else { TotalAmount = ""; }
                if (dtC.Contains("ComponentNumber"))
                {
                    Order_Num1 = Convert.ToString(dt.Rows[i]["ComponentNumber"]);//subhabrata on 21-02-2017
                }
                else
                {
                    Order_Num1 = "";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Products_Name"])))
                {
                    ProductName = Convert.ToString(dt.Rows[i]["Products_Name"]);
                }
                else
                {
                    ProductName = "";
                }
                purChaseDT.Rows.Add(Convert.ToString(i + 1), Convert.ToString(i + 1), Convert.ToString(dt.Rows[i]["QuoteDetails_ProductId"]), Convert.ToString(dt.Rows[i]["QuoteDetails_ProductDescription"]),
                    Convert.ToString(dt.Rows[i]["QuoteDetails_Quantity"]), Convert.ToString(dt.Rows[i]["UOM_ShortName"]), "", Convert.ToString(dt.Rows[i]["QuoteDetails_StockQty"]), Convert.ToString(dt.Rows[i]["UOM_ShortName"]),
                               0, Discount, Amount, TaxAmount, TotalAmount, "U", Convert.ToInt64(dt.Rows[i]["Indent_No"]), Order_Num1, ProductName);
            }

            Session["TProductInvoiceDetails"] = purChaseDT;

            return IsSuccess;
        }//End
        #endregion

        #region Subhabrata-Products
        protected void Productgrid_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void Productgrid_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void Productgrid_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            e.Cancel = true;
        }
        protected void aspxGridProduct_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {

        }

        [WebMethod]
        public static string getProductType(string Products_ID)
        {
            string Type = "";
            string query = @"Select
                           (Case When Isnull(Is_active_warehouse,'0')='0' AND Isnull(Is_active_Batch,'0')='0' AND Isnull(Is_active_serialno,'0')='0' Then ''
                           When Isnull(Is_active_warehouse,'0')='1' AND Isnull(Is_active_Batch,'0')='0' AND Isnull(Is_active_serialno,'0')='0' Then 'W'
                           When Isnull(Is_active_warehouse,'0')='0' AND Isnull(Is_active_Batch,'0')='1' AND Isnull(Is_active_serialno,'0')='0' Then 'B'
                           When Isnull(Is_active_warehouse,'0')='0' AND Isnull(Is_active_Batch,'0')='0' AND Isnull(Is_active_serialno,'0')='1' Then 'S'
                           When Isnull(Is_active_warehouse,'0')='1' AND Isnull(Is_active_Batch,'0')='1' AND Isnull(Is_active_serialno,'0')='0' Then 'WB'
                           When Isnull(Is_active_warehouse,'0')='1' AND Isnull(Is_active_Batch,'0')='0' AND Isnull(Is_active_serialno,'0')='1' Then 'WS'
                           When Isnull(Is_active_warehouse,'0')='1' AND Isnull(Is_active_Batch,'0')='1' AND Isnull(Is_active_serialno,'0')='1' Then 'WBS'
                           When Isnull(Is_active_warehouse,'0')='0' AND Isnull(Is_active_Batch,'0')='1' AND Isnull(Is_active_serialno,'0')='1' Then 'BS'
                           END) as Type
                           from Master_sProducts
                           where sProducts_ID='" + Products_ID + "'";

         //   BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine();

            DataTable dt = oDbEngine.GetDataTable(query);
            if (dt != null && dt.Rows.Count > 0)
            {
                Type = Convert.ToString(dt.Rows[0]["Type"]);
            }

            return Convert.ToString(Type);
        }
        #endregion

        protected void acbpCrpUdf_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
          //  BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

            if (Request.QueryString["key"] == "ADD")
            {
                if (reCat.isAllMandetoryDone((DataTable)Session["UdfDataOnAdd"], "TPB") == false)
                {
                    acbpCrpUdf.JSProperties["cpUDF"] = "false";

                }
                else
                {
                    acbpCrpUdf.JSProperties["cpUDF"] = "true";
                }


                acbpCrpUdf.JSProperties["cpTransport"] = "true";





                // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                DataTable DT = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='Transporter_TPBMandatory' AND IsActive=1");
                if (DT != null && DT.Rows.Count > 0)
                {
                    string IsMandatory = Convert.ToString(DT.Rows[0]["Variable_Value"]).Trim();
                    if (Convert.ToString(Session["TransporterVisibilty"]).Trim() == "Yes")
                    {
                        if (IsMandatory == "Yes")
                        {

                            if (hfControlData.Value.Trim() == "")
                            {
                                acbpCrpUdf.JSProperties["cpTransport"] = "false";
                            }

                            else { acbpCrpUdf.JSProperties["cpTransport"] = "true"; }
                        }
                    }
                    else { acbpCrpUdf.JSProperties["cpTransport"] = "true"; }
                }
                #region TC
                if (Convert.ToString(ddl_AmountAre.Value) != "4")
                {
                    DataTable DT_TC = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='TC_TPBMandatory' AND IsActive=1");
                    if (DT_TC != null && DT_TC.Rows.Count > 0)
                    {
                        string IsMandatory = Convert.ToString(DT_TC.Rows[0]["Variable_Value"]).Trim();

                      //  objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                        objEngine = new BusinessLogicLayer.DBEngine();

                        DataTable DTVisible = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='Show_TC_TPB' AND IsActive=1");
                        if (Convert.ToString(DTVisible.Rows[0]["Variable_Value"]).Trim() == "Yes")
                        {
                            if (IsMandatory == "Yes")
                            {
                                if (TermsConditionsControl.GetControlValue("dtDeliveryDate") == "" || TermsConditionsControl.GetControlValue("dtDeliveryDate") == "@")
                                {
                                    acbpCrpUdf.JSProperties["cpTC"] = "false";
                                }
                                else { acbpCrpUdf.JSProperties["cpTC"] = "true"; }
                            }
                            else
                            {
                                acbpCrpUdf.JSProperties["cpTC"] = "true";
                            }
                        }
                        else { acbpCrpUdf.JSProperties["cpTC"] = "true"; }
                    }
                    else
                    {
                        acbpCrpUdf.JSProperties["cpTC"] = "true";
                    }
                }
                else
                {
                    acbpCrpUdf.JSProperties["cpTC"] = "true";
                }
                #endregion

            }
            else
            {
                acbpCrpUdf.JSProperties["cpUDF"] = "true";
                acbpCrpUdf.JSProperties["cpTransport"] = "true";
                acbpCrpUdf.JSProperties["cpTC"] = "true";

                DataTable DT = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='Transporter_TPBMandatory' AND IsActive=1");
                if (DT != null && DT.Rows.Count > 0)
                {
                    string IsMandatory = Convert.ToString(DT.Rows[0]["Variable_Value"]).Trim();
                    if (Convert.ToString(Session["TransporterVisibilty"]).Trim() == "Yes")
                    {
                        if (IsMandatory == "Yes")
                        {
                            if (VehicleDetailsControl.GetControlValue("cmbTransporter") == "")
                            {
                                acbpCrpUdf.JSProperties["cpTransport"] = "false";
                            }

                            else { acbpCrpUdf.JSProperties["cpTransport"] = "true"; }
                        }
                    }
                    else { acbpCrpUdf.JSProperties["cpTransport"] = "true"; }
                }

                //----------Start-------------------------
                //Data: 01-06-2017 Author: Sayan Dutta
                //Details:To check T&C Mandatory Control
                #region TC
                DataTable DT_TC = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='TC_TPBMandatory' AND IsActive=1");
                if (Convert.ToString(ddl_AmountAre.Value) != "4")
                {
                    if (DT_TC != null && DT_TC.Rows.Count > 0)
                    {
                        string IsMandatory = Convert.ToString(DT_TC.Rows[0]["Variable_Value"]).Trim();

                      //  objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                        objEngine = new BusinessLogicLayer.DBEngine();

                        DataTable DTVisible = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='Show_TC_TPB' AND IsActive=1");
                        if (Convert.ToString(DTVisible.Rows[0]["Variable_Value"]).Trim() == "Yes")
                        {
                            if (IsMandatory == "Yes")
                            {
                                if (TermsConditionsControl.GetControlValue("dtDeliveryDate") == "" || TermsConditionsControl.GetControlValue("dtDeliveryDate") == "@")
                                {
                                    acbpCrpUdf.JSProperties["cpTC"] = "false";
                                }
                                else { acbpCrpUdf.JSProperties["cpTC"] = "true"; }
                            }
                        }
                        else { acbpCrpUdf.JSProperties["cpTC"] = "true"; }
                    }
                #endregion
                    //----------End-------------------------
                }
            }
        }
        protected void grid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data)
                return;          
            string IsLinkedProduct = Convert.ToString(e.GetValue("IsComponentProduct"));
            if (IsLinkedProduct == "C")
                e.Row.ForeColor = System.Drawing.Color.Blue;
        }
        #region Editable Permission Checking
        public void IsExistsDocumentInERPDocApproveStatus(string invoiceId)
        {
            string editable = "";
            string status = "";
            DataTable dt = new DataTable();
            int Id = Convert.ToInt32(invoiceId);
            dt = objERPDocPendingApproval.IsExistsDocumentInERPDocApproveStatus(Id, 12); // 8 for Purchase Invoice
            if (dt.Rows.Count > 0)
            {
                editable = Convert.ToString(dt.Rows[0]["editable"]);
                if (editable == "0")
                {
                    lbl_quotestatusmsg.Visible = true;
                    status = Convert.ToString(dt.Rows[0]["Status"]);
                    if (status == "Approved")
                    {
                        lbl_quotestatusmsg.Text = "Document already approved.";
                    }
                    else if (status == "Rejected")
                    {
                        lbl_quotestatusmsg.Text = "Document already rejected.";
                    }
                    else if (status == "Tagged")
                    {
                        lbl_quotestatusmsg.Text = "Tagged in Other Documents. Cannot modify.";
                    }
                    btn_SaveRecords.Visible = false;
                    ASPxButton1.Visible = false;
                }
                else
                {
                    lbl_quotestatusmsg.Visible = false;
                    btn_SaveRecords.Visible = true;
                    ASPxButton1.Visible = true;
                }
            }
        }

        #endregion Editable Permission Checking
        #region NonInventory Product Charges Start
        public DataTable NonInventoryTempDataTable()
        {
            DataTable TempDataTable = new DataTable();
            TempDataTable.Columns.Add("slno", typeof(System.Int32));
            TempDataTable.Columns.Add("TDSDetailsId", typeof(System.String));
            TempDataTable.Columns.Add("TDSMainId", typeof(System.String));
            TempDataTable.Columns.Add("BranchId", typeof(System.String));
            TempDataTable.Columns.Add("MonthId", typeof(System.String));
            TempDataTable.Columns.Add("ProductBasicAmt", typeof(System.String));
            TempDataTable.Columns.Add("ProductTotalAmt", typeof(System.String));
            TempDataTable.Columns.Add("TDSID", typeof(System.String));
            TempDataTable.Columns.Add("TDSRate", typeof(System.String));
            TempDataTable.Columns.Add("TDSAmount", typeof(System.String));

            TempDataTable.Columns.Add("SurchargeRate", typeof(System.String));
            TempDataTable.Columns.Add("SurchargeAmount", typeof(System.String));
            TempDataTable.Columns.Add("EducationCessRate", typeof(System.String));
            TempDataTable.Columns.Add("EducationCessAmt", typeof(System.String));
            TempDataTable.Columns.Add("HgrEducationCessRate", typeof(System.String));
            TempDataTable.Columns.Add("HgrEducationCessAmt", typeof(System.String));
            return TempDataTable;
        }

        public DataTable NonInventoryProductChgDtl()
        {
            if (Session["TPBNonInvProChgforShow"] == null)
            {
                DataTable NonInventoryProductChgdt = new DataTable();
                NonInventoryProductChgdt.Columns.Add("slno", typeof(System.Int32));
                NonInventoryProductChgdt.Columns.Add("TDSDetailsId", typeof(System.String));
                NonInventoryProductChgdt.Columns.Add("TDSMainId", typeof(System.String));
                NonInventoryProductChgdt.Columns.Add("BranchId", typeof(System.String));
                NonInventoryProductChgdt.Columns.Add("MonthId", typeof(System.String));
                NonInventoryProductChgdt.Columns.Add("ProductBasicAmt", typeof(System.String));
                NonInventoryProductChgdt.Columns.Add("ProductTotalAmt", typeof(System.String));
                NonInventoryProductChgdt.Columns.Add("TDSID", typeof(System.String));
                NonInventoryProductChgdt.Columns.Add("TDSRate", typeof(System.String));
                NonInventoryProductChgdt.Columns.Add("TDSAmount", typeof(System.String));

                NonInventoryProductChgdt.Columns.Add("SurchargeRate", typeof(System.String));
                NonInventoryProductChgdt.Columns.Add("SurchargeAmount", typeof(System.String));
                NonInventoryProductChgdt.Columns.Add("EducationCessRate", typeof(System.String));
                NonInventoryProductChgdt.Columns.Add("EducationCessAmt", typeof(System.String));
                NonInventoryProductChgdt.Columns.Add("HgrEducationCessRate", typeof(System.String));
                NonInventoryProductChgdt.Columns.Add("HgrEducationCessAmt", typeof(System.String));
                Session["TPBNonInvProChgforShow"] = NonInventoryProductChgdt;
                return NonInventoryProductChgdt;
            }
            else
            {
                DataTable dt = new DataTable();
                dt = (DataTable)Session["TPBNonInvProChgforShow"];
                return dt;
            }


        }

        protected void Inventorydtl_Callback(object sender, CallbackEventArgsBase e)
        {

        }

        protected void gridinventory_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {

            gridinventory.JSProperties["cpgrid"] = null;
            gridinventory.JSProperties["cpproamt"] = null;
            gridinventory.JSProperties["cpprochargeamt"] = null;
            gridinventory.JSProperties["cpbrMonAmtDtl"] = null;
            gridinventory.JSProperties["cppopuphide"] = null;
            gridinventory.JSProperties["cpPopupReq"] = null;
            string crgamt = "";
            decimal amountcharge = 0;

            string inventorydtl = e.Parameters.ToString();

            #region Show
            if (Convert.ToString(e.Parameters.Split('~')[2]) == "ShowBindGrid")
            {

                //int slNo = Convert.ToInt32(hdnInvWiseSlno.Value);
                ////string crgamt = "";
                ////decimal amountcharge = 0;
                //string productid = Convert.ToString(e.Parameters.Split('~')[0]);
                //decimal proamt = Convert.ToDecimal(Convert.ToString(e.Parameters.Split('~')[1]));
                //DataTable inventorychgdt = new DataTable();
                //DataTable tempTDSdt = new DataTable();

                //DataTable tempshow = NonInventoryTempDataTable();
                //DataRow tempshowrow = tempshow.NewRow();
                //if (Session["TPBNonInvProChgforShow"] != null)
                //{
                //    tempTDSdt = (DataTable)Session["TPBNonInvProChgforShow"];
                //    if (tempTDSdt.Rows.Count > 0)
                //    {
                //        //if(proamt==Convert.ToDecimal(Convert.ToString(tempTDSdt.Rows)))
                //        //DataRow[] tdsrow = tempTDSdt.Select("slno=" + Convert.ToInt32(slNo));
                //        DataRow[] tdsrow = tempTDSdt.Select("1=1");
                //        //if (tdsrow.it)

                //        //if (tdsrow.Where(c => IsNotEmpty(c)).ToArray().Length == 0)
                //        //{
                //        //    // Row is empty
                //        //}
                //        //if(tdsrow.are)
                //        //DataTable tempshow = new DataTable();
                //        //DataRow tempshowrow = tempshow.NewRow();
                //        if (tdsrow.Length > 0)
                //        {
                //            if (proamt == Convert.ToDecimal(Convert.ToString(tdsrow[0]["ProductBasicAmt"])))
                //            {
                //                DataTable temdt = new DataTable();
                //                tempshowrow["TDSID"] = Convert.ToString(tdsrow[0]["TDSID"]);
                //                //tempshowrow["TDSTCSRates_Code"] = Convert.ToString(tdsrow[0]["TDSTCSRates_Code"]);
                //                tempshowrow["TDSRate"] = Convert.ToString(tdsrow[0]["TDSRate"]);
                //                tempshowrow["TDSAmount"] = Convert.ToString(tdsrow[0]["TDSAmount"]);
                //                tempshowrow["SurchargeRate"] = Convert.ToString(tdsrow[0]["SurchargeRate"]);
                //                tempshowrow["SurchargeAmount"] = Convert.ToString(tdsrow[0]["SurchargeAmount"]);
                //                tempshowrow["EducationCessRate"] = Convert.ToString(tdsrow[0]["EducationCessRate"]);
                //                tempshowrow["EducationCessAmt"] = Convert.ToString(tdsrow[0]["EducationCessAmt"]);
                //                tempshowrow["HgrEducationCessRate"] = Convert.ToString(tdsrow[0]["HgrEducationCessRate"]);
                //                tempshowrow["HgrEducationCessAmt"] = Convert.ToString(tdsrow[0]["HgrEducationCessAmt"]);
                //                tempshow.Rows.Add(tempshowrow);
                //                temdt = tempshow.DefaultView.ToTable(false, "TDSID", "TDSRate", "TDSAmount", "SurchargeRate", "SurchargeAmount", "EducationCessRate", "EducationCessAmt", "HgrEducationCessRate", "HgrEducationCessAmt");
                //                inventorychgdt = tempshow;
                //                string tdsbranch = Convert.ToString(tdsrow[0]["BranchId"]);
                //                string tdsMonth = Convert.ToString(tdsrow[0]["MonthId"]);
                //                string tdsproBAmt = Convert.ToString(tdsrow[0]["ProductBasicAmt"]);
                //                string tdsproAmt = Convert.ToString(tdsrow[0]["ProductTotalAmt"]);
                //                gridinventory.JSProperties["cpbrMonAmtDtl"] = tdsbranch + "~" + tdsMonth + "~" + tdsproAmt + "~" + tdsproBAmt;
                //            }
                //            else
                //            {
                //                inventorychgdt = objPurchaseInvoice.PopulateNonInventoryCharges(productid, proamt);
                //            }
                //        }
                //        else
                //        {
                //            inventorychgdt = objPurchaseInvoice.PopulateNonInventoryCharges(productid, proamt);
                //        }
                //        //TDSTCSRates_ID,TDSTCSRates_Code,TDSTCSRates,TDSamt,Surcharge,Surchargeamt,EduCess,EduCessamt,HgrEduCess,HgrEduCessamt
                //        //tempTDSdt=tempTDSdt.tdsrow.de
                //    }
                //    else
                //    {
                //        inventorychgdt = objPurchaseInvoice.PopulateNonInventoryCharges(productid, proamt);
                //    }
                //}
                //else
                //{
                //    inventorychgdt = objPurchaseInvoice.PopulateNonInventoryCharges(productid, proamt);

                //}


                //foreach (DataRow dr in inventorychgdt.Rows)
                //{
                //    crgamt = Convert.ToString(dr["TDSAmount"]);
                //    amountcharge = amountcharge + Convert.ToDecimal(crgamt);
                //    crgamt = Convert.ToString(dr["SurchargeAmount"]);
                //    amountcharge = amountcharge + Convert.ToDecimal(crgamt);


                //    crgamt = Convert.ToString(dr["EducationCessAmt"]);
                //    amountcharge = amountcharge + Convert.ToDecimal(crgamt);
                //    crgamt = Convert.ToString(dr["HgrEducationCessAmt"]);
                //    amountcharge = amountcharge + Convert.ToDecimal(crgamt);

                //}
                //gridinventory.DataSource = inventorychgdt;
                //gridinventory.DataBind();
                //Session["TPBNonInvProChg"] = inventorychgdt;

                //gridinventory.JSProperties["cpgrid"] = "Y";
                //gridinventory.JSProperties["cpproamt"] = proamt;
                //gridinventory.JSProperties["cpprochargeamt"] = Convert.ToString(amountcharge);
            }
            #endregion Show
            #region Save
            else if (e.Parameters.Split('~')[2] == "SaveNonInventoryProductChg")
            {
                string cnt = Convert.ToString(e.Parameters.Split('~')[3]);
                gridinventory.JSProperties["cppopuphide"] = "Y";
                //int slNo = Convert.ToInt32(hdnInvWiseSlno.Value);
                int slNo = 0;
                DataTable storePBNonInvProDt = new DataTable();
                DataTable PBNonInvPro = (DataTable)Session["TPBNonInvProChg"];
                if (Session["TPBNonInvProChgforShow"] != null)
                {
                    storePBNonInvProDt = (DataTable)Session["TPBNonInvProChgforShow"];
                    for (int i = storePBNonInvProDt.Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow dr = storePBNonInvProDt.Rows[i];
                        if (Convert.ToString(dr["slno"]) == Convert.ToString(slNo))
                            dr.Delete();
                    }
                    //DataRow[] noninvrow = storePBNonInvProDt.Select("slno=" + Convert.ToInt32(slNo));
                    storePBNonInvProDt.AcceptChanges();
                }
                else
                {
                    storePBNonInvProDt = NonInventoryProductChgDtl();
                }





                DataRow newRownoninvpro = storePBNonInvProDt.NewRow();
                newRownoninvpro["slno"] = Convert.ToInt32(slNo);
                newRownoninvpro["TDSDetailsId"] = "0";
                newRownoninvpro["TDSMainId"] = Convert.ToString(ddl_TdsScheme.Value);
                newRownoninvpro["BranchId"] = Convert.ToString(ddl_noninventoryBranch.Value);
                newRownoninvpro["MonthId"] = Convert.ToString(ddl_month.Value);
                //if (gridinventory.rows)
                if (Convert.ToInt32(cnt) > 0)
                {
                    newRownoninvpro["ProductBasicAmt"] = Convert.ToString(txt_proamt.Text);
                    newRownoninvpro["ProductTotalAmt"] = Convert.ToString(txt_totalnoninventoryproductamt.Text);
                    newRownoninvpro["TDSID"] = Convert.ToString(PBNonInvPro.Rows[0]["TDSID"]);
                    newRownoninvpro["TDSRate"] = Convert.ToString(PBNonInvPro.Rows[0]["TDSRate"]);
                    newRownoninvpro["TDSAmount"] = Convert.ToString(PBNonInvPro.Rows[0]["TDSAmount"]);
                    newRownoninvpro["SurchargeRate"] = Convert.ToString(PBNonInvPro.Rows[0]["SurchargeRate"]);
                    newRownoninvpro["SurchargeAmount"] = Convert.ToString(PBNonInvPro.Rows[0]["SurchargeAmount"]);
                    newRownoninvpro["EducationCessRate"] = Convert.ToString(PBNonInvPro.Rows[0]["EducationCessRate"]);
                    newRownoninvpro["EducationCessAmt"] = Convert.ToString(PBNonInvPro.Rows[0]["EducationCessAmt"]);
                    newRownoninvpro["HgrEducationCessRate"] = Convert.ToString(PBNonInvPro.Rows[0]["HgrEducationCessRate"]);
                    newRownoninvpro["HgrEducationCessAmt"] = Convert.ToString(PBNonInvPro.Rows[0]["HgrEducationCessAmt"]);
                }
                else
                {
                    newRownoninvpro["ProductBasicAmt"] = Convert.ToString(txt_proamt.Text);
                    newRownoninvpro["ProductTotalAmt"] = Convert.ToString(txt_totalnoninventoryproductamt.Text);
                    newRownoninvpro["TDSID"] = Convert.ToString("0");
                    newRownoninvpro["TDSRate"] = Convert.ToString("0");
                    newRownoninvpro["TDSAmount"] = Convert.ToString("0");
                    newRownoninvpro["SurchargeRate"] = Convert.ToString("0");
                    newRownoninvpro["SurchargeAmount"] = Convert.ToString("0");
                    newRownoninvpro["EducationCessRate"] = Convert.ToString("0");
                    newRownoninvpro["EducationCessAmt"] = Convert.ToString("0");
                    newRownoninvpro["HgrEducationCessRate"] = Convert.ToString("0");
                    newRownoninvpro["HgrEducationCessAmt"] = Convert.ToString("0");
                }
                storePBNonInvProDt.Rows.Add(newRownoninvpro);
                storePBNonInvProDt.DefaultView.Sort = "slno asc";
                storePBNonInvProDt = storePBNonInvProDt.DefaultView.ToTable(true);
                Session["TPBNonInvProChgforShow"] = storePBNonInvProDt;
                hdntdschecking.Value = "0";
                //End Here

                //aspxGridTax.JSProperties["cpUpdated"] = "";

                //Session["TPurchaseOrderFinalTaxRecord"] = TaxRecord;
            }
            #endregion  Save
            #region TDS Checking
            else if (e.Parameters.Split('~')[0] == "ApplicableAmtChecking")
            {
                DataTable tempTDSdt = new DataTable();
                int slNo = 0;
                DataTable tempshow = NonInventoryTempDataTable();
                DataRow tempshowrow = tempshow.NewRow();
                decimal totalamt = 0;
                string Vendorid = e.Parameters.Split('~')[1];
                string schemeid = e.Parameters.Split('~')[2];
                string TAmt = e.Parameters.Split('~')[3];
                decimal TdsAmt = Convert.ToDecimal(TAmt);
                DataTable ApplicableAmtdt = objPurchaseInvoice.GetApplicableAmtDetail(Vendorid, schemeid);
                if (ApplicableAmtdt.Rows.Count > 0)
                {
                    string ApplicableAmount = Convert.ToString(ApplicableAmtdt.Rows[0]["applicableamt"]);
                    if (Convert.ToDecimal(ApplicableAmount) == 0)
                    {
                        gridinventory.JSProperties["cpApplicableAmount"] = "0";
                        gridinventory.JSProperties["cpPopupReq"] = "Y";
                        hdntdschecking.Value = "1";
                        DataTable inventorychgdt = new DataTable();
                        //DataTable inventorychgdt = objPurchaseInvoice.PopulateNonInventoryChargesForVendor(schemeid, TdsAmt);
                        if (Session["TPBNonInvProChgforShow"] != null)
                        {
                            tempTDSdt = (DataTable)Session["TPBNonInvProChgforShow"];
                            if (tempTDSdt.Rows.Count > 0)
                            {
                                //if(proamt==Convert.ToDecimal(Convert.ToString(tempTDSdt.Rows)))
                                //DataRow[] tdsrow = tempTDSdt.Select("slno=" + Convert.ToInt32(slNo));
                                DataRow[] tdsrow = tempTDSdt.Select("slno=" + Convert.ToInt32(slNo));
                                //DataRow[] tdsrow = tempTDSdt.Select("1=1");
                                //if (tdsrow.it)

                                //if (tdsrow.Where(c => IsNotEmpty(c)).ToArray().Length == 0)
                                //{
                                //    // Row is empty
                                //}
                                //if(tdsrow.are)
                                //DataTable tempshow = new DataTable();
                                //DataRow tempshowrow = tempshow.NewRow();
                                if (tdsrow.Length > 0)
                                {
                                    //if (proamt == Convert.ToDecimal(Convert.ToString(tdsrow[0]["ProductBasicAmt"])))
                                    //{
                                    DataTable temdt = new DataTable();
                                    tempshowrow["TDSID"] = Convert.ToString(tdsrow[0]["TDSID"]);
                                    //tempshowrow["TDSTCSRates_Code"] = Convert.ToString(tdsrow[0]["TDSTCSRates_Code"]);
                                    tempshowrow["TDSRate"] = Convert.ToString(tdsrow[0]["TDSRate"]);
                                    tempshowrow["TDSAmount"] = Convert.ToString(tdsrow[0]["TDSAmount"]);
                                    tempshowrow["SurchargeRate"] = Convert.ToString(tdsrow[0]["SurchargeRate"]);
                                    tempshowrow["SurchargeAmount"] = Convert.ToString(tdsrow[0]["SurchargeAmount"]);
                                    tempshowrow["EducationCessRate"] = Convert.ToString(tdsrow[0]["EducationCessRate"]);
                                    tempshowrow["EducationCessAmt"] = Convert.ToString(tdsrow[0]["EducationCessAmt"]);
                                    tempshowrow["HgrEducationCessRate"] = Convert.ToString(tdsrow[0]["HgrEducationCessRate"]);
                                    tempshowrow["HgrEducationCessAmt"] = Convert.ToString(tdsrow[0]["HgrEducationCessAmt"]);
                                    tempshow.Rows.Add(tempshowrow);
                                    temdt = tempshow.DefaultView.ToTable(false, "TDSID", "TDSRate", "TDSAmount", "SurchargeRate", "SurchargeAmount", "EducationCessRate", "EducationCessAmt", "HgrEducationCessRate", "HgrEducationCessAmt");
                                    inventorychgdt = tempshow;
                                    string tdsbranch = Convert.ToString(tdsrow[0]["BranchId"]);
                                    string tdsMonth = Convert.ToString(tdsrow[0]["MonthId"]);
                                    string tdsproBAmt = Convert.ToString(tdsrow[0]["ProductBasicAmt"]);
                                    string tdsproAmt = Convert.ToString(tdsrow[0]["ProductTotalAmt"]);
                                    gridinventory.JSProperties["cpbrMonAmtDtl"] = tdsbranch + "~" + tdsMonth + "~" + tdsproAmt + "~" + tdsproBAmt;
                                    //}
                                    //else
                                    //{
                                    //    inventorychgdt = objPurchaseInvoice.PopulateNonInventoryChargesForVendor(schemeid, TdsAmt);
                                    //    //inventorychgdt = objPurchaseInvoice.PopulateNonInventoryCharges(productid, proamt);
                                    //}
                                }
                                else
                                {
                                    inventorychgdt = objPurchaseInvoice.PopulateNonInventoryChargesForVendor(schemeid, TdsAmt);
                                    //inventorychgdt = objPurchaseInvoice.PopulateNonInventoryCharges(productid, proamt);
                                }
                                //TDSTCSRates_ID,TDSTCSRates_Code,TDSTCSRates,TDSamt,Surcharge,Surchargeamt,EduCess,EduCessamt,HgrEduCess,HgrEduCessamt
                                //tempTDSdt=tempTDSdt.tdsrow.de
                            }
                            else
                            {
                                inventorychgdt = objPurchaseInvoice.PopulateNonInventoryChargesForVendor(schemeid, TdsAmt);
                                //inventorychgdt = objPurchaseInvoice.PopulateNonInventoryCharges(productid, proamt);
                            }
                        }
                        else
                        {
                            inventorychgdt = objPurchaseInvoice.PopulateNonInventoryChargesForVendor(schemeid, TdsAmt);
                            //inventorychgdt = objPurchaseInvoice.PopulateNonInventoryCharges(productid, proamt);

                        }


                        foreach (DataRow dr in inventorychgdt.Rows)
                        {
                            crgamt = Convert.ToString(dr["TDSAmount"]);
                            amountcharge = amountcharge + Convert.ToDecimal(crgamt);
                            crgamt = Convert.ToString(dr["SurchargeAmount"]);
                            amountcharge = amountcharge + Convert.ToDecimal(crgamt);


                            crgamt = Convert.ToString(dr["EducationCessAmt"]);
                            amountcharge = amountcharge + Convert.ToDecimal(crgamt);
                            crgamt = Convert.ToString(dr["HgrEducationCessAmt"]);
                            amountcharge = amountcharge + Convert.ToDecimal(crgamt);

                        }
                        gridinventory.DataSource = inventorychgdt;
                        gridinventory.DataBind();
                        Session["TPBNonInvProChg"] = inventorychgdt;

                        gridinventory.JSProperties["cpgrid"] = "Y";
                        gridinventory.JSProperties["cpproamt"] = TdsAmt;
                        gridinventory.JSProperties["cpprochargeamt"] = Convert.ToString(amountcharge);
                        //if (inventorychgdt.Rows.Count > 0)
                        //{
                        //    foreach (DataRow dr in inventorychgdt.Rows)
                        //    {
                        //        crgamt = Convert.ToString(dr["TDSAmount"]);
                        //        amountcharge = amountcharge + Convert.ToDecimal(crgamt);
                        //        crgamt = Convert.ToString(dr["SurchargeAmount"]);
                        //        amountcharge = amountcharge + Convert.ToDecimal(crgamt);


                        //        crgamt = Convert.ToString(dr["EducationCessAmt"]);
                        //        amountcharge = amountcharge + Convert.ToDecimal(crgamt);
                        //        crgamt = Convert.ToString(dr["HgrEducationCessAmt"]);
                        //        amountcharge = amountcharge + Convert.ToDecimal(crgamt);

                        //    }
                        //    gridinventory.DataSource = inventorychgdt;
                        //    gridinventory.DataBind();
                        //    Session["TPBNonInvProChg"] = inventorychgdt;

                        //    gridinventory.JSProperties["cpgrid"] = "Y";
                        //    gridinventory.JSProperties["cpproamt"] = TdsAmt;
                        //    gridinventory.JSProperties["cpprochargeamt"] = Convert.ToString(amountcharge);
                        //}
                    }
                    else
                    {
                        //DataTable LimitcrossDt = objPurchaseInvoice.PopulateLimitcrossDtlofVendorBySchemeId(Vendorid, schemeid, TdsAmt);
                        //if (LimitcrossDt.Rows.Count > 0)
                        //{
                        //    decimal VendorTDSAmt = Convert.ToDecimal(Convert.ToString(LimitcrossDt.Rows[0]["totalTDSAmt"]));
                        if (Convert.ToDecimal(ApplicableAmount) > TdsAmt)
                        {
                            gridinventory.JSProperties["cpPopupReq"] = "N";
                            Session["TPBNonInvProChgforShow"] = null;
                            Session["TPBNonInvProChg"] = null;
                            hdntdschecking.Value = "0";
                        }
                        else
                        {
                            hdntdschecking.Value = "1";
                            gridinventory.JSProperties["cpPopupReq"] = "Y";
                            DataTable inventorychgdt = new DataTable();
                            //inventorychgdt = objPurchaseInvoice.PopulateNonInventoryChargesForVendor(schemeid, TdsAmt);
                            //int slNo = Convert.ToInt32(hdnInvWiseSlno.Value);
                            //int slNo = 0;
                            //string crgamt = "";
                            //decimal amountcharge = 0;
                            //string productid = Convert.ToString(e.Parameters.Split('~')[0]);
                            //decimal proamt = Convert.ToDecimal(Convert.ToString(e.Parameters.Split('~')[1]));

                            //DataTable tempTDSdt = new DataTable();

                            //DataTable tempshow = NonInventoryTempDataTable();
                            //DataRow tempshowrow = tempshow.NewRow();
                            if (Session["TPBNonInvProChgforShow"] != null)
                            {
                                tempTDSdt = (DataTable)Session["TPBNonInvProChgforShow"];
                                if (tempTDSdt.Rows.Count > 0)
                                {
                                    if (TdsAmt == Convert.ToDecimal(Convert.ToString(tempTDSdt.Rows[0]["ProductBasicAmt"])))
                                    {
                                        if (tempTDSdt.Rows.Count > 0)
                                        {
                                            //if(proamt==Convert.ToDecimal(Convert.ToString(tempTDSdt.Rows)))
                                            //DataRow[] tdsrow = tempTDSdt.Select("slno=" + Convert.ToInt32(slNo));
                                            DataRow[] tdsrow = tempTDSdt.Select("slno=" + Convert.ToInt32(slNo));
                                            //DataRow[] tdsrow = tempTDSdt.Select("1=1");
                                            //if (tdsrow.it)

                                            //if (tdsrow.Where(c => IsNotEmpty(c)).ToArray().Length == 0)
                                            //{
                                            //    // Row is empty
                                            //}
                                            //if(tdsrow.are)
                                            //DataTable tempshow = new DataTable();
                                            //DataRow tempshowrow = tempshow.NewRow();
                                            if (tdsrow.Length > 0)
                                            {
                                                //if (proamt == Convert.ToDecimal(Convert.ToString(tdsrow[0]["ProductBasicAmt"])))
                                                //{
                                                DataTable temdt = new DataTable();
                                                tempshowrow["TDSID"] = Convert.ToString(tdsrow[0]["TDSID"]);
                                                //tempshowrow["TDSTCSRates_Code"] = Convert.ToString(tdsrow[0]["TDSTCSRates_Code"]);
                                                tempshowrow["TDSRate"] = Convert.ToString(tdsrow[0]["TDSRate"]);
                                                tempshowrow["TDSAmount"] = Convert.ToString(tdsrow[0]["TDSAmount"]);
                                                tempshowrow["SurchargeRate"] = Convert.ToString(tdsrow[0]["SurchargeRate"]);
                                                tempshowrow["SurchargeAmount"] = Convert.ToString(tdsrow[0]["SurchargeAmount"]);
                                                tempshowrow["EducationCessRate"] = Convert.ToString(tdsrow[0]["EducationCessRate"]);
                                                tempshowrow["EducationCessAmt"] = Convert.ToString(tdsrow[0]["EducationCessAmt"]);
                                                tempshowrow["HgrEducationCessRate"] = Convert.ToString(tdsrow[0]["HgrEducationCessRate"]);
                                                tempshowrow["HgrEducationCessAmt"] = Convert.ToString(tdsrow[0]["HgrEducationCessAmt"]);
                                                tempshow.Rows.Add(tempshowrow);
                                                temdt = tempshow.DefaultView.ToTable(false, "TDSID", "TDSRate", "TDSAmount", "SurchargeRate", "SurchargeAmount", "EducationCessRate", "EducationCessAmt", "HgrEducationCessRate", "HgrEducationCessAmt");
                                                inventorychgdt = tempshow;
                                                string tdsbranch = Convert.ToString(tdsrow[0]["BranchId"]);
                                                string tdsMonth = Convert.ToString(tdsrow[0]["MonthId"]);
                                                string tdsproBAmt = Convert.ToString(tdsrow[0]["ProductBasicAmt"]);
                                                string tdsproAmt = Convert.ToString(tdsrow[0]["ProductTotalAmt"]);
                                                gridinventory.JSProperties["cpbrMonAmtDtl"] = tdsbranch + "~" + tdsMonth + "~" + tdsproAmt + "~" + tdsproBAmt;
                                                //}
                                                //else
                                                //{
                                                //    inventorychgdt = objPurchaseInvoice.PopulateNonInventoryChargesForVendor(schemeid, TdsAmt);
                                                //    //inventorychgdt = objPurchaseInvoice.PopulateNonInventoryCharges(productid, proamt);
                                                //}
                                            }
                                            else
                                            {
                                                inventorychgdt = objPurchaseInvoice.PopulateNonInventoryChargesForVendor(schemeid, TdsAmt);
                                                //inventorychgdt = objPurchaseInvoice.PopulateNonInventoryCharges(productid, proamt);
                                            }
                                            //TDSTCSRates_ID,TDSTCSRates_Code,TDSTCSRates,TDSamt,Surcharge,Surchargeamt,EduCess,EduCessamt,HgrEduCess,HgrEduCessamt
                                            //tempTDSdt=tempTDSdt.tdsrow.de
                                        }
                                    }
                                    else
                                    {
                                        inventorychgdt = objPurchaseInvoice.PopulateNonInventoryChargesForVendor(schemeid, TdsAmt);
                                    }
                                }
                                else
                                {
                                    inventorychgdt = objPurchaseInvoice.PopulateNonInventoryChargesForVendor(schemeid, TdsAmt);
                                    //inventorychgdt = objPurchaseInvoice.PopulateNonInventoryCharges(productid, proamt);
                                }
                            }
                            else
                            {
                                inventorychgdt = objPurchaseInvoice.PopulateNonInventoryChargesForVendor(schemeid, TdsAmt);
                                //inventorychgdt = objPurchaseInvoice.PopulateNonInventoryCharges(productid, proamt);

                            }


                            foreach (DataRow dr in inventorychgdt.Rows)
                            {
                                crgamt = Convert.ToString(dr["TDSAmount"]);
                                amountcharge = amountcharge + Convert.ToDecimal(crgamt);
                                crgamt = Convert.ToString(dr["SurchargeAmount"]);
                                amountcharge = amountcharge + Convert.ToDecimal(crgamt);


                                crgamt = Convert.ToString(dr["EducationCessAmt"]);
                                amountcharge = amountcharge + Convert.ToDecimal(crgamt);
                                crgamt = Convert.ToString(dr["HgrEducationCessAmt"]);
                                amountcharge = amountcharge + Convert.ToDecimal(crgamt);

                            }
                            gridinventory.DataSource = inventorychgdt;
                            gridinventory.DataBind();
                            Session["TPBNonInvProChg"] = inventorychgdt;

                            gridinventory.JSProperties["cpgrid"] = "Y";
                            gridinventory.JSProperties["cpproamt"] = TdsAmt;
                            gridinventory.JSProperties["cpprochargeamt"] = Convert.ToString(amountcharge);
                            //if (inventorychgdt.Rows.Count > 0)
                            //{
                            //    foreach (DataRow dr in inventorychgdt.Rows)
                            //    {
                            //        crgamt = Convert.ToString(dr["TDSAmount"]);
                            //        amountcharge = amountcharge + Convert.ToDecimal(crgamt);
                            //        crgamt = Convert.ToString(dr["SurchargeAmount"]);
                            //        amountcharge = amountcharge + Convert.ToDecimal(crgamt);


                            //        crgamt = Convert.ToString(dr["EducationCessAmt"]);
                            //        amountcharge = amountcharge + Convert.ToDecimal(crgamt);
                            //        crgamt = Convert.ToString(dr["HgrEducationCessAmt"]);
                            //        amountcharge = amountcharge + Convert.ToDecimal(crgamt);

                            //    }
                            //    gridinventory.DataSource = inventorychgdt;
                            //    gridinventory.DataBind();
                            //    Session["TPBNonInvProChg"] = inventorychgdt;

                            //    gridinventory.JSProperties["cpgrid"] = "Y";
                            //    gridinventory.JSProperties["cpproamt"] = TdsAmt;
                            //    gridinventory.JSProperties["cpprochargeamt"] = Convert.ToString(amountcharge);
                            //}
                        }
                        //}
                    }
                }



            }
            #endregion TDS Checking End

        }

        protected void DeleteNonInventoryDetail(string strDeleteSrlNo)
        {

            DataTable noninventorydt = new DataTable();
            //if (Session["WarehouseData"] != null)
            if (Session["TPBNonInvProChgforShow"] != null)
            {
                noninventorydt = (DataTable)Session["TPBNonInvProChgforShow"];

                //var rows = noninventorydt.Select("slno ='" + strDeleteSrlNo + "'");
                var rows = noninventorydt.Select("slno ='0'");
                foreach (var row in rows)
                {
                    row.Delete();
                }
                noninventorydt.AcceptChanges();
                #region Code Added by Sam on 09062017  To arrange noninventory Warehouse Data according to serial no Start
                //int j = 0;
                //foreach (DataRow dr in noninventorydt.Rows)
                //{
                //    string slno = Convert.ToString(dr["slno"]);
                //    if (Convert.ToInt32(slno) > Convert.ToInt32(strDeleteSrlNo))
                //    {
                //        dr["slno"] = Convert.ToString(Convert.ToInt32(slno) - 1);
                //    }
                //}
                //noninventorydt.AcceptChanges();

                //Session["TPBNonInvProChgforShow"] = noninventorydt;
                Session["TPBNonInvProChgforShow"] = null;
                hdntdschecking.Value = "1";
                #endregion Code Added by Sam on 09062017  To arrange  noninventory Warehouse Data according to serial no End
            }

        }

        protected void PopulateBranchForTDS()
        {
            DataTable branchdt = new DataTable();
            branchdt = objPurchaseInvoice.PopulateBranchForNonInventoryCharges(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            ddl_noninventoryBranch.TextField = "Code";
            ddl_noninventoryBranch.ValueField = "ID";
            ddl_noninventoryBranch.DataSource = branchdt;
            ddl_noninventoryBranch.DataBind();
        }



        protected void ddl_noninventoryBranch_Callback(object sender, CallbackEventArgsBase e)
        {
            PopulateBranchForTDS();
            //PopulateCustomerDetail();
        }

        #endregion NonInventory Product Charges Start

        #region #### Purchase Invoice Mail ####
        public int Sendmail_PurchaseInvoice(string Output)
        {
            int stat = 0;
            if (chkmail.Checked)
            {

                Employee_BL objemployeebal = new Employee_BL();
                ExceptionLogging mailobj = new ExceptionLogging();
                EmailSenderHelperEL emailSenderSettings = new EmailSenderHelperEL();
                DataTable dt_EmailConfig = new DataTable();
                DataTable dt_EmailConfigpurchase = new DataTable();

                DataTable dt_Emailbodysubject = new DataTable();
                PurchaseinvoiceEmailTags fetchModel = new PurchaseinvoiceEmailTags();
                string Subject = "";
                string Body = "";
                string emailTo = "";              
                int MailStatus = 0;               
                if (!string.IsNullOrEmpty(Convert.ToString(cmbContactPerson.Value)))
                {
                    var customerid = cmbContactPerson.Value.ToString();

                    dt_EmailConfig = objemployeebal.GetemailidsForChallan(customerid);
                }
                string FilePath = ConfigurationManager.AppSettings["Reportpdf"].ToString() + "Default_PI_" + Output + ".pdf";
                string FileName = FilePath;
                if (dt_EmailConfig.Rows.Count > 0)
                {
                    emailTo = Convert.ToString(dt_EmailConfig.Rows[0]["eml_email"]);                    
                    dt_Emailbodysubject = objemployeebal.Getemailtemplates("15");  //for purchase order
                    if (dt_Emailbodysubject.Rows.Count > 0)
                    {
                        Body = Convert.ToString(dt_Emailbodysubject.Rows[0]["body"]);
                        Subject = Convert.ToString(dt_Emailbodysubject.Rows[0]["subjct"]);

                        dt_EmailConfigpurchase = objemployeebal.Getemailtagsforpurchase(Output, "PurchaseInvoiceEmailTags");  //For Purchase Order Get all Tags Value

                        if (dt_EmailConfigpurchase.Rows.Count > 0)
                        {

                            fetchModel = DbHelpers.ToModel<PurchaseinvoiceEmailTags>(dt_EmailConfigpurchase);
                            Body = Employee_BL.GetFormattedString<PurchaseinvoiceEmailTags>(fetchModel, Body);
                            Subject = Employee_BL.GetFormattedString<PurchaseinvoiceEmailTags>(fetchModel, Subject);
                        }

                        emailSenderSettings = mailobj.GetEmailSettingsforAllreport(emailTo, "", "", FilePath, Body, Subject);
                        if (emailSenderSettings.IsSuccess)
                        {
                            string Message = "";

                            EmailSenderEL obj2 = new EmailSenderEL();
                            stat = SendEmailUL.sendMailInHtmlFormat(emailSenderSettings.ModelCast<EmailSenderEL>(), out Message);

                        }
                    }
                }
            }
            return stat;
        }
        #endregion

        #region Visibility of Send Mail

        public void VisiblitySendEmail()
        {
            Employee_BL objemployeebal = new Employee_BL();
            chkmail.Visible = true;
            if (Request.QueryString["key"] != "ADD")
            {
                chkmail.Visible = false;
            }
            DataTable dt2 = new DataTable();
            dt2 = objemployeebal.GetSystemsettingmail("Show Email in PI");
            if (Convert.ToString(dt2.Rows[0]["Variable_Value"]) == "Yes")
            {
                chkmail.Visible = true;
            }
            else
            {
                chkmail.Visible = false;
            }

        }

        #endregion

        #region Arrange Warehouse Existing Data according to insert
        public void SetWareHouseDataInEditMode()
        {
            #region Savenew
            if (Session["TPBwarehousedetailstempNew"] != null)
            {
                DataTable Warehousedt = EditWarehouse();
                DataTable Warehousedts = new DataTable();
                decimal cnt = 1;  // for WHBTSL
                decimal sumqty = 0;  // for WHBTSL
                decimal bcnt = 1;  // for WHBT 
                decimal bsumqty;   // for WHBT 
                decimal wscnt = 1;  // for WHSL 
                decimal wssumqty;   // for WHSL 

                decimal bscnt = 1;  // for WHSL 
                decimal bssumqty;   // for WHSL 
                int WarehouseID = 0;
                string batchno = "";
                //DataTable Warehousedt = new DataTable();
                DataTable Warehousedtnew = new DataTable();
                DataTable whdt = (DataTable)Session["TPBwarehousedetailstempNew"];
                foreach (DataRow dr in whdt.Rows)
                {

                    string pcslno = Convert.ToString(dr["pcslno"]);
                    string inventorytype = Convert.ToString(dr["Inventrytype"]);

                    #region WHBTSL
                    if (inventorytype == "WHBTSL")
                    {
                        if (ViewState["WSWarehouseID"] != null)    // if product and pcslno is changed make the view state of other product type =null otherwise quantity will not show proper.
                        {
                            ViewState["WSWarehouseID"] = null;
                        }
                        if (ViewState["bsbatchno"] != null)    // if product and pcslno is changed make the view state of other product type =null otherwise quantity will not show proper.
                        {
                            ViewState["bsbatchno"] = null;
                        }

                        wscnt = 1;    // Initialized with 1 for WHSL type product
                        if (cnt == 1)
                        {

                            ViewState["Pcslno"] = Convert.ToInt32(dr["pcslno"]);

                            WarehouseID = Convert.ToInt32(dr["warehouseid"]);
                            batchno = Convert.ToString(dr["BatchNO"]);
                            if (ViewState["WarehouseID"] == null)
                            {
                                ViewState["WarehouseID"] = WarehouseID;
                            }
                            if (ViewState["batchno"] == null)
                            {
                                ViewState["batchno"] = batchno;
                            }

                            //if (Convert.ToInt32(ViewState["WarehouseID"]) == WarehouseID && Convert.ToString(ViewState["batchno"]) == batchno)
                            //sumqty=Convert.ToDecimal(Convert.ToString(dr["quantitysum"]));
                            Warehousedt.Rows.Add(Convert.ToString(dr["SrlNo"]), Convert.ToString(dr["pcslno"]),
                           Convert.ToString(dr["BatchWareHouseId"]), Convert.ToString(dr["BatchWarehousedetailsID"]),
                           Convert.ToString(dr["batchid"]), Convert.ToString(dr["serialid"]), Convert.ToString(dr["warehouseid"]),
                           Convert.ToString(dr["warehousename"]), Convert.ToString(dr["BatchNO"]), Convert.ToString(dr["SerialNo"]),
                           Convert.ToString(dr["MFGDate"]), Convert.ToString(dr["ExpiryDate"]), Convert.ToString(dr["viewQuantity"]),
                           Convert.ToString(dr["productid"]), "0", Convert.ToString(dr["Inventrytype"]),
                           Convert.ToString(dr["isnew"]));

                        }
                        else if (cnt > 1)
                        {
                            WarehouseID = Convert.ToInt32(dr["warehouseid"]);
                            batchno = Convert.ToString(dr["BatchNO"]);
                            if (ViewState["Pcslno"] != null)
                            {
                                if (Convert.ToInt32(ViewState["Pcslno"]) == Convert.ToInt32(dr["pcslno"]))
                                {

                                    if (Convert.ToInt32(ViewState["WarehouseID"]) == WarehouseID && Convert.ToString(ViewState["batchno"]) == batchno)
                                    //else if(sumqty>=cnt)
                                    {
                                        Warehousedt.Rows.Add(Convert.ToString(dr["SrlNo"]), Convert.ToString(dr["pcslno"]),
                                        Convert.ToString(dr["BatchWareHouseId"]), Convert.ToString(dr["BatchWarehousedetailsID"]),
                                        Convert.ToString(dr["batchid"]), Convert.ToString(dr["serialid"]), Convert.ToString(dr["warehouseid"]),
                                        Convert.ToString(dr["warehousename"]), Convert.ToString(dr["BatchNO"]), Convert.ToString(dr["SerialNo"]),
                                        Convert.ToString(dr["MFGDate"]), Convert.ToString(dr["ExpiryDate"]), 0,
                                        Convert.ToString(dr["productid"]), "0", Convert.ToString(dr["Inventrytype"]),
                                        Convert.ToString(dr["isnew"]));
                                        cnt = cnt + 1;
                                    }
                                    else
                                    {
                                        Warehousedt.Rows.Add(Convert.ToString(dr["SrlNo"]), Convert.ToString(dr["pcslno"]),
                                           Convert.ToString(dr["BatchWareHouseId"]), Convert.ToString(dr["BatchWarehousedetailsID"]),
                                           Convert.ToString(dr["batchid"]), Convert.ToString(dr["serialid"]), Convert.ToString(dr["warehouseid"]),
                                           Convert.ToString(dr["warehousename"]), Convert.ToString(dr["BatchNO"]), Convert.ToString(dr["SerialNo"]),
                                           Convert.ToString(dr["MFGDate"]), Convert.ToString(dr["ExpiryDate"]), Convert.ToString(dr["viewQuantity"]),
                                           Convert.ToString(dr["productid"]), "0", Convert.ToString(dr["Inventrytype"]),
                                           Convert.ToString(dr["isnew"]));
                                        ViewState["WarehouseID"] = WarehouseID;
                                        ViewState["batchno"] = batchno;
                                    }
                                }
                                else
                                {
                                    ViewState["Pcslno"] = Convert.ToInt32(dr["pcslno"]);
                                    WarehouseID = Convert.ToInt32(dr["warehouseid"]);
                                    batchno = Convert.ToString(dr["BatchNO"]);
                                    ViewState["WarehouseID"] = WarehouseID;
                                    ViewState["batchno"] = batchno;
                                    Warehousedt.Rows.Add(Convert.ToString(dr["SrlNo"]), Convert.ToString(dr["pcslno"]),
                                   Convert.ToString(dr["BatchWareHouseId"]), Convert.ToString(dr["BatchWarehousedetailsID"]),
                                   Convert.ToString(dr["batchid"]), Convert.ToString(dr["serialid"]), Convert.ToString(dr["warehouseid"]),
                                   Convert.ToString(dr["warehousename"]), Convert.ToString(dr["BatchNO"]), Convert.ToString(dr["SerialNo"]),
                                   Convert.ToString(dr["MFGDate"]), Convert.ToString(dr["ExpiryDate"]), Convert.ToString(dr["viewQuantity"]),
                                   Convert.ToString(dr["productid"]), "0", Convert.ToString(dr["Inventrytype"]),
                                   Convert.ToString(dr["isnew"]));
                                }
                            }
                        }
                        if (cnt == 1)
                        {
                            cnt = cnt + 1;
                        }
                        //else if (sumqty < cnt)
                        //{
                        //    sumqty = Convert.ToDecimal(Convert.ToString(dr["quantitysum"]));
                        //    //sumqty = Convert.ToInt32(Convert.ToString(dr["quantitysum"]));
                        //    Warehousedt.Rows.Add(Convert.ToString(dr["SrlNo"]), Convert.ToString(dr["pcslno"]),
                        //   Convert.ToString(dr["BatchWareHouseId"]), Convert.ToString(dr["BatchWarehousedetailsID"]),
                        //   Convert.ToString(dr["batchid"]), Convert.ToString(dr["serialid"]), Convert.ToString(dr["warehouseid"]),
                        //   Convert.ToString(dr["warehousename"]), Convert.ToString(dr["BatchNO"]), Convert.ToString(dr["SerialNo"]),
                        //   Convert.ToString(dr["MFGDate"]), Convert.ToString(dr["ExpiryDate"]), Convert.ToString(dr["quantitysum"]),
                        //   Convert.ToString(dr["productid"]), "0", Convert.ToString(dr["Inventrytype"]),
                        //   Convert.ToString(dr["isnew"]));
                        //    cnt = cnt + 1;
                        //}
                    }
                    #endregion WHBTSL
                    #region WHBT
                    if (inventorytype == "WHBT" || inventorytype == "BT" || inventorytype == "SL")
                    {
                        if (ViewState["WSWarehouseID"] != null)    // if product and pcslno is changed make the view state of other product type =null otherwise quantity will not show proper.
                        {
                            ViewState["WSWarehouseID"] = null;
                        }
                        if (ViewState["WarehouseID"] != null)    // if product and pcslno is changed make the view state of other product type =null otherwise quantity will not show proper.
                        {
                            ViewState["WarehouseID"] = null;
                        }
                        if (ViewState["bsbatchno"] != null)    // if product and pcslno is changed make the view state of other product type =null otherwise quantity will not show proper.
                        {
                            ViewState["bsbatchno"] = null;
                        }
                        cnt = 1;      // Initialized with 1 for WHBTSL type product
                        wscnt = 1;    // Initialized with 1 for WHSL type product
                        Warehousedt.Rows.Add(Convert.ToString(dr["SrlNo"]), Convert.ToString(dr["pcslno"]),
                        Convert.ToString(dr["BatchWareHouseId"]), Convert.ToString(dr["BatchWarehousedetailsID"]),
                        Convert.ToString(dr["batchid"]), Convert.ToString(dr["serialid"]), Convert.ToString(dr["warehouseid"]),
                        Convert.ToString(dr["warehousename"]), Convert.ToString(dr["BatchNO"]), Convert.ToString(dr["SerialNo"]),
                        Convert.ToString(dr["MFGDate"]), Convert.ToString(dr["ExpiryDate"]), Convert.ToString(dr["viewQuantity"]),
                        Convert.ToString(dr["productid"]), "0", Convert.ToString(dr["Inventrytype"]),
                        Convert.ToString(dr["isnew"]));
                    }
                    #endregion WHBT
                    #region WHSL
                    if (inventorytype == "WHSL")
                    {
                        if (ViewState["WarehouseID"] != null)    // if product and pcslno is changed make the view state of other product type =null otherwise quantity will not show proper.
                        {
                            ViewState["WarehouseID"] = null;
                        }
                        if (ViewState["bsbatchno"] != null)    // if product and pcslno is changed make the view state of other product type =null otherwise quantity will not show proper.
                        {
                            ViewState["bsbatchno"] = null;
                        }
                        cnt = 1;      // Initialized with 1 for WHBTSL type product

                        if (wscnt == 1)
                        {
                            ViewState["Pcslno"] = Convert.ToInt32(dr["pcslno"]);

                            WarehouseID = Convert.ToInt32(dr["warehouseid"]);
                            batchno = Convert.ToString(dr["BatchNO"]);
                            if (ViewState["WSWarehouseID"] == null)
                            {
                                ViewState["WSWarehouseID"] = WarehouseID;
                            }


                            //if (Convert.ToInt32(ViewState["WarehouseID"]) == WarehouseID && Convert.ToString(ViewState["batchno"]) == batchno)
                            //sumqty=Convert.ToDecimal(Convert.ToString(dr["quantitysum"]));
                            Warehousedt.Rows.Add(Convert.ToString(dr["SrlNo"]), Convert.ToString(dr["pcslno"]),
                           Convert.ToString(dr["BatchWareHouseId"]), Convert.ToString(dr["BatchWarehousedetailsID"]),
                           Convert.ToString(dr["batchid"]), Convert.ToString(dr["serialid"]), Convert.ToString(dr["warehouseid"]),
                           Convert.ToString(dr["warehousename"]), Convert.ToString(dr["BatchNO"]), Convert.ToString(dr["SerialNo"]),
                           Convert.ToString(dr["MFGDate"]), Convert.ToString(dr["ExpiryDate"]), Convert.ToString(dr["quantitysum"]),
                           Convert.ToString(dr["productid"]), "0", Convert.ToString(dr["Inventrytype"]),
                           Convert.ToString(dr["isnew"]));

                        }
                        else if (wscnt > 1)
                        {
                            WarehouseID = Convert.ToInt32(dr["warehouseid"]);
                            batchno = Convert.ToString(dr["BatchNO"]);
                            if (ViewState["Pcslno"] != null)
                            {
                                if (Convert.ToInt32(ViewState["Pcslno"]) == Convert.ToInt32(dr["pcslno"]))
                                {
                                    if (Convert.ToInt32(ViewState["WSWarehouseID"]) == WarehouseID)
                                    {
                                        Warehousedt.Rows.Add(Convert.ToString(dr["SrlNo"]), Convert.ToString(dr["pcslno"]),
                                        Convert.ToString(dr["BatchWareHouseId"]), Convert.ToString(dr["BatchWarehousedetailsID"]),
                                        Convert.ToString(dr["batchid"]), Convert.ToString(dr["serialid"]), Convert.ToString(dr["warehouseid"]),
                                        Convert.ToString(dr["warehousename"]), Convert.ToString(dr["BatchNO"]), Convert.ToString(dr["SerialNo"]),
                                        Convert.ToString(dr["MFGDate"]), Convert.ToString(dr["ExpiryDate"]), 0,
                                        Convert.ToString(dr["productid"]), "0", Convert.ToString(dr["Inventrytype"]),
                                        Convert.ToString(dr["isnew"]));
                                        wscnt = wscnt + 1;
                                    }
                                    else
                                    {
                                        WarehouseID = Convert.ToInt32(dr["warehouseid"]);
                                        ViewState["WSWarehouseID"] = WarehouseID;
                                        Warehousedt.Rows.Add(Convert.ToString(dr["SrlNo"]), Convert.ToString(dr["pcslno"]),
                                       Convert.ToString(dr["BatchWareHouseId"]), Convert.ToString(dr["BatchWarehousedetailsID"]),
                                       Convert.ToString(dr["batchid"]), Convert.ToString(dr["serialid"]), Convert.ToString(dr["warehouseid"]),
                                       Convert.ToString(dr["warehousename"]), Convert.ToString(dr["BatchNO"]), Convert.ToString(dr["SerialNo"]),
                                       Convert.ToString(dr["MFGDate"]), Convert.ToString(dr["ExpiryDate"]), Convert.ToString(dr["quantitysum"]),
                                       Convert.ToString(dr["productid"]), "0", Convert.ToString(dr["Inventrytype"]),
                                       Convert.ToString(dr["isnew"]));
                                        ViewState["WSWarehouseID"] = WarehouseID;
                                    }
                                }
                                else
                                {
                                    WarehouseID = Convert.ToInt32(dr["warehouseid"]);
                                    ViewState["WSWarehouseID"] = WarehouseID;
                                    ViewState["Pcslno"] = Convert.ToInt32(dr["pcslno"]);
                                    Warehousedt.Rows.Add(Convert.ToString(dr["SrlNo"]), Convert.ToString(dr["pcslno"]),
                                   Convert.ToString(dr["BatchWareHouseId"]), Convert.ToString(dr["BatchWarehousedetailsID"]),
                                   Convert.ToString(dr["batchid"]), Convert.ToString(dr["serialid"]), Convert.ToString(dr["warehouseid"]),
                                   Convert.ToString(dr["warehousename"]), Convert.ToString(dr["BatchNO"]), Convert.ToString(dr["SerialNo"]),
                                   Convert.ToString(dr["MFGDate"]), Convert.ToString(dr["ExpiryDate"]), Convert.ToString(dr["quantitysum"]),
                                   Convert.ToString(dr["productid"]), "0", Convert.ToString(dr["Inventrytype"]),
                                   Convert.ToString(dr["isnew"]));
                                }
                            }
                        }
                        if (wscnt == 1)
                        {
                            wscnt = wscnt + 1;
                        }
                    }
                    #endregion WHSL
                    #region BTSL
                    if (inventorytype == "BTSL")
                    {
                        if (ViewState["WSWarehouseID"] != null)    // if product and pcslno is changed make the view state of other product type =null otherwise quantity will not show proper.
                        {
                            ViewState["WSWarehouseID"] = null;
                        }
                        if (ViewState["WarehouseID"] != null)    // if product and pcslno is changed make the view state of other product type =null otherwise quantity will not show proper.
                        {
                            ViewState["WarehouseID"] = null;
                        }
                        cnt = 1;      // Initialized with 1 for WHBTSL type product
                        wscnt = 1;    // Initialized with 1 for WHSL type product
                        if (bscnt == 1)
                        {

                            batchno = Convert.ToString(dr["BatchNO"]);
                            if (ViewState["bsbatchno"] == null)
                            {
                                ViewState["bsbatchno"] = batchno;
                            }
                            Warehousedt.Rows.Add(Convert.ToString(dr["SrlNo"]), Convert.ToString(dr["pcslno"]),
                            Convert.ToString(dr["BatchWareHouseId"]), Convert.ToString(dr["BatchWarehousedetailsID"]),
                            Convert.ToString(dr["batchid"]), Convert.ToString(dr["serialid"]), Convert.ToString(dr["warehouseid"]),
                            Convert.ToString(dr["warehousename"]), Convert.ToString(dr["BatchNO"]), Convert.ToString(dr["SerialNo"]),
                            Convert.ToString(dr["MFGDate"]), Convert.ToString(dr["ExpiryDate"]), Convert.ToString(dr["viewQuantity"]),
                            Convert.ToString(dr["productid"]), "0", Convert.ToString(dr["Inventrytype"]),
                            Convert.ToString(dr["isnew"]));

                        }
                        else if (bscnt > 1)
                        {
                            //WarehouseID = Convert.ToInt32(dr["warehouseid"]);
                            batchno = Convert.ToString(dr["BatchNO"]);
                            if (Convert.ToString(ViewState["bsbatchno"]) == batchno)
                            //else if(sumqty>=cnt)
                            {
                                Warehousedt.Rows.Add(Convert.ToString(dr["SrlNo"]), Convert.ToString(dr["pcslno"]),
                                Convert.ToString(dr["BatchWareHouseId"]), Convert.ToString(dr["BatchWarehousedetailsID"]),
                                Convert.ToString(dr["batchid"]), Convert.ToString(dr["serialid"]), Convert.ToString(dr["warehouseid"]),
                                Convert.ToString(dr["warehousename"]), Convert.ToString(dr["BatchNO"]), Convert.ToString(dr["SerialNo"]),
                                Convert.ToString(dr["MFGDate"]), Convert.ToString(dr["ExpiryDate"]), 0,
                                Convert.ToString(dr["productid"]), "0", Convert.ToString(dr["Inventrytype"]),
                                Convert.ToString(dr["isnew"]));
                                bscnt = bscnt + 1;
                            }
                            else
                            {
                                //WarehouseID = Convert.ToInt32(dr["warehouseid"]);
                                batchno = Convert.ToString(dr["BatchNO"]);
                                //ViewState["WarehouseID"] = WarehouseID;
                                ViewState["bsbatchno"] = batchno;
                                Warehousedt.Rows.Add(Convert.ToString(dr["SrlNo"]), Convert.ToString(dr["pcslno"]),
                               Convert.ToString(dr["BatchWareHouseId"]), Convert.ToString(dr["BatchWarehousedetailsID"]),
                               Convert.ToString(dr["batchid"]), Convert.ToString(dr["serialid"]), Convert.ToString(dr["warehouseid"]),
                               Convert.ToString(dr["warehousename"]), Convert.ToString(dr["BatchNO"]), Convert.ToString(dr["SerialNo"]),
                               Convert.ToString(dr["MFGDate"]), Convert.ToString(dr["ExpiryDate"]), Convert.ToString(dr["viewQuantity"]),
                               Convert.ToString(dr["productid"]), "0", Convert.ToString(dr["Inventrytype"]),
                               Convert.ToString(dr["isnew"]));
                            }
                        }
                        if (bscnt == 1)
                        {
                            bscnt = bscnt + 1;
                        }
                        //else if (sumqty < cnt)
                        //{
                        //    sumqty = Convert.ToDecimal(Convert.ToString(dr["quantitysum"]));
                        //    //sumqty = Convert.ToInt32(Convert.ToString(dr["quantitysum"]));
                        //    Warehousedt.Rows.Add(Convert.ToString(dr["SrlNo"]), Convert.ToString(dr["pcslno"]),
                        //   Convert.ToString(dr["BatchWareHouseId"]), Convert.ToString(dr["BatchWarehousedetailsID"]),
                        //   Convert.ToString(dr["batchid"]), Convert.ToString(dr["serialid"]), Convert.ToString(dr["warehouseid"]),
                        //   Convert.ToString(dr["warehousename"]), Convert.ToString(dr["BatchNO"]), Convert.ToString(dr["SerialNo"]),
                        //   Convert.ToString(dr["MFGDate"]), Convert.ToString(dr["ExpiryDate"]), Convert.ToString(dr["quantitysum"]),
                        //   Convert.ToString(dr["productid"]), "0", Convert.ToString(dr["Inventrytype"]),
                        //   Convert.ToString(dr["isnew"]));
                        //    cnt = cnt + 1;
                        //}
                    }
                    #endregion BTSL
                    #region WH
                    if (inventorytype == "WH")
                    {
                        if (ViewState["WSWarehouseID"] != null)    // if product and pcslno is changed make the view state of other product type =null otherwise quantity will not show proper.
                        {
                            ViewState["WSWarehouseID"] = null;
                        }
                        if (ViewState["WarehouseID"] != null)    // if product and pcslno is changed make the view state of other product type =null otherwise quantity will not show proper.
                        {
                            ViewState["WarehouseID"] = null;
                        }
                        if (ViewState["bsbatchno"] != null)    // if product and pcslno is changed make the view state of other product type =null otherwise quantity will not show proper.
                        {
                            ViewState["bsbatchno"] = null;
                        }

                        cnt = 1;      // Initialized with 1 for WHBTSL type product
                        wscnt = 1;    // Initialized with 1 for WHSL type product
                        Warehousedt.Rows.Add(Convert.ToString(dr["SrlNo"]), Convert.ToString(dr["pcslno"]),
                       Convert.ToString(dr["BatchWareHouseId"]), Convert.ToString(dr["BatchWarehousedetailsID"]),
                       Convert.ToString(dr["batchid"]), Convert.ToString(dr["serialid"]), Convert.ToString(dr["warehouseid"]),
                       Convert.ToString(dr["warehousename"]), Convert.ToString(dr["BatchNO"]), Convert.ToString(dr["SerialNo"]),
                       Convert.ToString(dr["MFGDate"]), Convert.ToString(dr["ExpiryDate"]), Convert.ToString(dr["quantitysum"]),
                       Convert.ToString(dr["productid"]), "0", Convert.ToString(dr["Inventrytype"]),
                       Convert.ToString(dr["isnew"]));
                    }
                    #endregion WHBTSL

                }
                #region Trash Code
                //string WarehouseName = Convert.ToString(dr["warehousename"]);

                //string WarehouseID = Convert.ToString(dr["warehouseid"]);

                //string BatchName = Convert.ToString(dr["BatchNO"]);

                //string SerialName = Convert.ToString(dr["SerialNo"]);
                //string ProductID = Convert.ToString(dr["productid"]);
                //string stockid = Convert.ToString(0);
                //string WarehouseName = Convert.ToString(e.Parameters.Split('~')[2]);

                //string WarehouseID = Convert.ToString(e.Parameters.Split('~')[1]).Trim();

                //string BatchName = Convert.ToString(e.Parameters.Split('~')[3]);

                //string SerialName = Convert.ToString(e.Parameters.Split('~')[4]);
                //string ProductID = Convert.ToString(hdfProductIDPC.Value);
                //string stockid = Convert.ToString(hdfstockidPC.Value);

                // Code Commented And Added by Sam on 26052017 due to unavailable value of txtqnty.Text start for only serial type product
                //decimal openingstock = 0;
                //if (hdfProductType.Value == "S")
                //{
                //    if (hdnProductQuantity.Value != "" && hdnProductQuantity.Value != null)
                //    {
                //        openingstock = Convert.ToDecimal("1");
                //    }
                //}
                //else
                //{
                //    openingstock = Convert.ToDecimal(txtqnty.Text);
                //}
                //openingstock = Convert.ToDecimal(txtqnty.Text);
                // Code Commented And Added by Sam on 26052017 due to unavailable value of txtqnty.Text End

                //string branchid = Convert.ToString(Session["TWbranchInEdit"]);


                //string qnty = Convert.ToString(dr["viewQuantity"]);
                //string productsrlno = Convert.ToString(dr["pcslno"]);
                //string branchid = Convert.ToString(hdbranchIDPC.Value);
                //string qnty = Convert.ToString(e.Parameters.Split('~')[5]);
                //string productsrlno = Convert.ToString(e.Parameters.Split('~')[6]);

                //decimal totalopeining = Convert.ToDecimal(hdfopeningstockPC.Value);
                //decimal oldtotalopeining = Convert.ToDecimal(oldopeningqntity.Value);
                //// Code Commented And Added   by Sam on 26052017 due to unavailable value of txtqnty.Text Start
                //if (hdfProductType.Value != "S")
                //{
                //    if (qnty == "0.0000" && openingstock <= 0)
                //    {
                //        qnty = batchqnty.Text;
                //        openingstock = Convert.ToDecimal(qnty);
                //    }
                //}
                //if (qnty == "0.0000" && openingstock <= 0)
                //{
                //    qnty = batchqnty.Text;
                //    openingstock = Convert.ToDecimal(qnty);
                //}
                // Code Commented And Added   by Sam on 26052017 due to unavailable value of txtqnty.Text End
                //if (!string.IsNullOrEmpty(BatchName))
                //{
                //    BatchName = BatchName.Trim();
                //}
                //if (!string.IsNullOrEmpty(SerialName))
                //{
                //    SerialName = SerialName.Trim();
                //}
                //if (WarehouseID == "null")
                //{
                //    WarehouseID = "0";
                //}

                //bool isserialunique = false;
                //if (hdnisserial.Value == "true")
                //{
                //    isserialunique = CheckUniqueSerial(SerialName, "new");
                //}
                //if (Convert.ToDecimal(openingstock) > totalopeining)
                //{
                //    GrdWarehousePC.JSProperties["cpupdatemssg"] = Convert.ToString("You have entered Quantity greater than Opening Quantity. Cannot Proceed.");

                //}
                #endregion Trash Code

                //} //
                //Session["TPOCWarehouseData"] = Warehousedt;
                //Session["TPBwarehousedetailstemp"] = Warehousedt;
                Session["TPBwarehousedetailstempNew"] = Warehousedt;

                Session["TPBwarehousedetailstemp"] = Warehousedt;

                Session["TPOCWarehouseData"] = Warehousedt;
                //GrdWarehousePC.DataSource = Warehousedt.DefaultView;
                //GrdWarehousePC.DataBind();

            }
            #endregion
        }


        #endregion

        public DataTable EditWarehouse()
        {
            #region Datatable Creation
            DataTable Warehousedt = new DataTable();
            Warehousedt.Columns.Add("SrlNo", typeof(Int32));
            Warehousedt.Columns.Add("pcslno", typeof(Int32));
            Warehousedt.Columns.Add("BatchWarehouseID", typeof(string));
            Warehousedt.Columns.Add("BatchWarehousedetailsID", typeof(string));
            Warehousedt.Columns.Add("BatchID", typeof(string));
            Warehousedt.Columns.Add("SerialID", typeof(string));

            Warehousedt.Columns.Add("WarehouseID", typeof(string));
            Warehousedt.Columns.Add("WarehouseName", typeof(string));

            Warehousedt.Columns.Add("BatchNo", typeof(string));
            Warehousedt.Columns.Add("SerialNo", typeof(string));

            Warehousedt.Columns.Add("MFGDate", typeof(DateTime));
            Warehousedt.Columns.Add("ExpiryDate", typeof(DateTime));

            Warehousedt.Columns.Add("Quantitysum", typeof(decimal));
            Warehousedt.Columns.Add("productid", typeof(string));
            Warehousedt.Columns.Add("StockID", typeof(string));
            Warehousedt.Columns.Add("Inventrytype", typeof(string));
            Warehousedt.Columns.Add("isnew", typeof(string));



            return Warehousedt;
            #endregion Datatable Creation
        }
        #region Contact Person Header Div Dtl

        protected void acpContactPersonPhone_Callback(object sender, CallbackEventArgsBase e)
        {
            try
            {
                string WhichCall = e.Parameter.Split('~')[0];
                if (WhichCall == "ContactPersonPhone")
                {
                    string InternalId = Convert.ToString(Convert.ToString(e.Parameter.Split('~')[1]));

                    DataTable dtContactPersonPhone = new DataTable();
                    dtContactPersonPhone = objPurchaseOrderBL.PopulateContactPersonPhone(InternalId);
                    if (dtContactPersonPhone != null && dtContactPersonPhone.Rows.Count > 0)
                    {
                        pageheaderContent.Attributes.Add("style", "display:block");
                        divContactPhone.Attributes.Add("style", "display:block");

                        foreach (DataRow dr in dtContactPersonPhone.Rows)
                        {
                            string phone = Convert.ToString(dr["phf_phoneNumber"]);
                            if (!string.IsNullOrEmpty(phone))
                            {
                                // lblContactPhone.Text = phone;
                                acpContactPersonPhone.JSProperties["cpPhone"] = phone;
                                break;
                            }
                        }

                    }
                    else
                    {
                        acpContactPersonPhone.JSProperties["cpPhone"] = null;
                    }

                }
            }
            catch { }
        }
        #endregion Contact Person Header Div Dtl
        protected void ApplicableAmtPopup_Callback(object sender, CallbackEventArgsBase e)
        {
            string ActionName = e.Parameter.Split('~')[0];
            if (ActionName == "ApplicableAmtChecking")
            {
                decimal totalamt = 0;
                string Vendorid = e.Parameter.Split('~')[1];
                string schemeid = e.Parameter.Split('~')[2];
                string TAmt = e.Parameter.Split('~')[3];
                decimal TdsAmt = Convert.ToDecimal(TAmt);
                DataTable ApplicableAmtdt = objPurchaseInvoice.GetApplicableAmtDetail(Vendorid, schemeid);
                if (ApplicableAmtdt.Rows.Count > 0)
                {
                    string ApplicableAmount = Convert.ToString(ApplicableAmtdt.Rows[0]["applicableamt"]);
                    if (Convert.ToDecimal(ApplicableAmount) == 0)
                    {
                        ApplicableAmtPopup.JSProperties["ApplicableAmount"] = "0";
                    }
                    else
                    {
                        DataTable LimitcrossDt = objPurchaseInvoice.PopulateLimitcrossDtlofVendorBySchemeId(Vendorid, schemeid, TdsAmt);
                        if (LimitcrossDt.Rows.Count > 0)
                        {
                            decimal VendorTDSAmt = Convert.ToDecimal(Convert.ToString(LimitcrossDt.Rows[0]["totalTDSAmt"]));
                            if (Convert.ToDecimal(ApplicableAmount) > VendorTDSAmt)
                            {
                                ApplicableAmtPopup.JSProperties["PopupReq"] = "N";
                            }
                            else
                            {
                                ApplicableAmtPopup.JSProperties["PopupReq"] = "Y";
                            }
                        }
                    }
                }


                if (ApplicableAmtdt.Rows.Count > 0)
                {

                }
                else
                {

                }
            }
            else if (ActionName == "BindProductForNonInvItem")
            {                
            }
        }

        protected void ddl_TdsScheme_Callback(object sender, CallbackEventArgsBase e)
        {            
            PopulateTDSSchemeForNonInventoryItem();
        }
        public void PopulateTDSSchemeForNonInventoryItem()
        {
            DataTable tdsSchemeTD = new DataTable();
            tdsSchemeTD = objPurchaseInvoice.PopulateTDSSchemeForNonInventoryItem();
            if (tdsSchemeTD.Rows.Count > 0)
            {
                ddl_TdsScheme.TextField = "TDSTCS_Code";
                ddl_TdsScheme.ValueField = "TDSTCS_ID";
                ddl_TdsScheme.DataSource = tdsSchemeTD;
                ddl_TdsScheme.DataBind();
                ddl_TdsScheme.SelectedIndex = 0;
            }
        }  
        protected void grid_Products_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
           
        }

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
        protected void EntityServerModeDataSalesInvoice_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Proj_Id";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            BusinessLogicLayer.DBEngine BEngine = new BusinessLogicLayer.DBEngine();
           

           
            
           var  q = from d in dc.V_ProjectLists
                    where d.ProjectStatus == "Approved" && d.ProjBracnchid == Convert.ToInt64(ddl_Branch.SelectedValue) && d.CustId == Convert.ToString(shipTopartyId.Value)
                    orderby d.Proj_Id descending
                    select d;

            e.QueryableSource = q;

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

        [WebMethod]
        public static object SetProjectCode(string OrderId, string TagDocType)
        {
            List<DocumentDetails> Detail = new List<DocumentDetails>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                ProcedureExecute proc = new ProcedureExecute("prc_TransitPurchaseInvoiceDetail");
                proc.AddVarcharPara("@Action", 500, "OrdertaggingProjectdata");
                proc.AddVarcharPara("@TPID", 100, OrderId);
                proc.AddVarcharPara("@TagDocType", 500, TagDocType);
                DataTable address = proc.GetTable();

                if (address != null)
                {
                    Detail = (from DataRow dr in address.Rows
                              select new DocumentDetails()

                              {
                                  ProjectId = Convert.ToInt64(dr["ProjectId"]),
                                  ProjectCode = Convert.ToString(dr["ProjectCode"])
                              }).ToList();
                }


                return Detail;

            }
            return null;

        }
        [WebMethod(EnableSession = true)]
        public static string GetContactPersonRelatedData(string Key, string branchid)
        {

            int i = 0;
            string InternalId = Convert.ToString(Key);
            string vendorbranchid = Convert.ToString(branchid);
            TransitPurchaseInvoiceBL objPurchaseInvoice = new TransitPurchaseInvoiceBL();
            string strDueDate = ""; string cpOutstanding = "0.0"; string cpGSTN = ""; string cpcountry = "";
            string str="";
            DataSet vendst = new DataSet();
            vendst = objPurchaseInvoice.GetCustomerDetails_InvoiceRelated(InternalId, vendorbranchid);
            if (vendst.Tables[0] != null && vendst.Tables[0].Rows.Count > 0)
            {
                 strDueDate = Convert.ToString(vendst.Tables[0].Rows[0]["DueDate"]);
               
            }
            else
            {
                strDueDate = null;
            }

            if (vendst.Tables[1] != null && vendst.Tables[1].Rows.Count > 0)
            {
                var convertDecimal = Convert.ToDecimal(Convert.ToString(vendst.Tables[1].Rows[0]["NetOutstanding"]));
                if (convertDecimal > 0)
                {
                    cpOutstanding = Convert.ToString(convertDecimal) + "(Db)";
                }
                else
                {

                    cpOutstanding = Convert.ToString(convertDecimal * -1).ToString() + "(Cr)";
                }
            }
            else
            {
                cpOutstanding = "0.0";
            }

            if (vendst.Tables[2] != null && vendst.Tables[2].Rows.Count > 0)
            {
                string strGSTN = Convert.ToString(vendst.Tables[2].Rows[0]["CNT_GSTIN"]);
                if (strGSTN != "")
                {
                    cpGSTN = "Yes";

                }
                else
                {
                    cpGSTN = "No";
                }
            }

            if (vendst.Tables[3] != null && vendst.Tables[3].Rows.Count > 0)
            {
                string strCountry = Convert.ToString(vendst.Tables[3].Rows[0]["add_country"]);
                if (strCountry != "")
                {
                    cpcountry = strCountry;

                }
                else
                {
                    cpcountry = "1";
                }
            }
            else
            {
                cpcountry = "1";
            }

            str = strDueDate + "@" + cpOutstanding + "@" + cpGSTN + "@" + cpcountry;
            return str;
        }

        public class ContactPersonTPB
        {
            public string cp_contactId { get; set; }
            public string cp_name { get; set; }

            public string InternalID { get; set; }

        }
        [WebMethod(EnableSession = true)]
        public static object GetContactPersonafterBillingShipping(string Key)
        {

            DataTable dtContactPerson = new DataTable();
            TransitPurchaseInvoiceBL objPurchaseInvoice = new TransitPurchaseInvoiceBL();
            List<ContactPersonTPB> contactPerson = new List<ContactPersonTPB>();
            dtContactPerson = objPurchaseInvoice.PopulateContactPersonOfCustomer(Key);
            if (dtContactPerson != null && dtContactPerson.Rows.Count > 0)
            {

                if (dtContactPerson != null && dtContactPerson.Rows.Count > 0)
                {
                    for (int i = 0; i < dtContactPerson.Rows.Count; i++)
                    {
                        contactPerson.Add(new ContactPersonTPB
                        {
                            cp_contactId = Convert.ToString(dtContactPerson.Rows[i]["cp_contactId"]),
                            cp_name = Convert.ToString(dtContactPerson.Rows[i]["cp_name"])
                        });
                    }
                }
            }
            return contactPerson;
        }

        [WebMethod(EnableSession = true)]
        public static object taxUpdatePanel_Callback(string Action, string srl, string prodid)
        {
            string output = "200";
            try
            {
                //string performpara = e.Parameter;
                if (Action == "DelProdbySl")
                {
                    DataTable MainTaxDataTable = (DataTable)HttpContext.Current.Session["TPurchaseOrderFinalTaxRecord"];

                    DataRow[] deletedRow = MainTaxDataTable.Select("SlNo=" + srl);
                    if (deletedRow.Length > 0)
                    {
                        foreach (DataRow dr in deletedRow)
                        {
                            MainTaxDataTable.Rows.Remove(dr);
                        }

                    }

                    HttpContext.Current.Session["TPurchaseOrderFinalTaxRecord"] = MainTaxDataTable;
                   // GetStock(Convert.ToString(performpara.Split('~')[2]));
                    // DeleteWarehouse(Convert.ToString(performpara.Split('~')[1]));
                    DataTable taxDetails = (DataTable)HttpContext.Current.Session["TPBTaxDetails"];
                    if (taxDetails != null)
                    {
                        foreach (DataRow dr in taxDetails.Rows)
                        {
                            dr["Amount"] = "0.00";
                        }
                        HttpContext.Current.Session["TPBTaxDetails"] = taxDetails;
                    }
                }
                else if (Action == "DeleteAllTax")
                {
                    CreateDataTaxTableByAjax();

                    DataTable taxDetails = (DataTable)HttpContext.Current.Session["TPBTaxDetails"];

                    if (taxDetails != null)
                    {
                        foreach (DataRow dr in taxDetails.Rows)
                        {
                            dr["Amount"] = "0.00";
                        }
                        HttpContext.Current.Session["TPBTaxDetails"] = taxDetails;
                    }
                }
                else
                {
                    DataTable MainTaxDataTable = (DataTable)HttpContext.Current.Session["TPurchaseOrderFinalTaxRecord"];

                    DataRow[] deletedRow = MainTaxDataTable.Select("SlNo=" + srl);
                    if (deletedRow.Length > 0)
                    {
                        foreach (DataRow dr in deletedRow)
                        {
                            MainTaxDataTable.Rows.Remove(dr);
                        }

                    }

                    HttpContext.Current.Session["TPurchaseOrderFinalTaxRecord"] = MainTaxDataTable;

                    DataTable taxDetails = (DataTable)HttpContext.Current.Session["TPBTaxDetails"];

                    if (taxDetails != null)
                    {
                        foreach (DataRow dr in taxDetails.Rows)
                        {
                            dr["Amount"] = "0.00";
                        }
                        HttpContext.Current.Session["TPBTaxDetails"] = taxDetails;
                    }
                }
            }
            catch
            {
                output = "201";

            }


            return output;

        }


        [WebMethod(EnableSession = true)]
        public static object getTDSDetails(string CustomerId, string invoice_id, string date, string totalAmount, string taxableAmount, string branch_id, string tds_code)
        {
            string Mode = Convert.ToString(HttpContext.Current.Session["key_QutId"]);
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_TDSDetails_Calc");
            proc.AddVarcharPara("@VendorID", 500, CustomerId);
            proc.AddVarcharPara("@invoice_id", 500, invoice_id);
            proc.AddVarcharPara("@Action", 500, "ShowTDSDetails");
            proc.AddVarcharPara("@date", 500, date);
            proc.AddVarcharPara("@totalAmount", 500, totalAmount);
            proc.AddVarcharPara("@taxableAmount", 500, taxableAmount);
            proc.AddVarcharPara("@branch_id", 500, branch_id);
            proc.AddVarcharPara("@tds_code", 500, tds_code);

            dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                return new { tds_amount = Convert.ToString(dt.Rows[0]["tds_amount"]), Rate = Convert.ToString(dt.Rows[0]["Rate"]), Code = Convert.ToString(dt.Rows[0]["Code"]), Amount = Convert.ToString(dt.Rows[0]["Amount"]) };
            }
            else
            {
                return new { tds_amount = 0, Rate = 0, Code = 0, Amount = 0 };
            }
        }

        protected void GridTDSdocs_DataBinding(object sender, EventArgs e)
        {
            if (Session["TDSGrid"] != null)
                GridTDSdocs.DataSource = (DataTable)Session["TDSGrid"];
        }

        protected void GridTDSdocs_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_TDSDetails_Calc");
            proc.AddVarcharPara("@VendorID", 500, hdnCustomerId.Value);
            proc.AddVarcharPara("@Action", 500, "ShowTDSList");
            proc.AddVarcharPara("@branch_id", 500, ddl_Branch.SelectedValue);
            proc.AddVarcharPara("@trans_date", 500, dt_PLQuote.Text);
            proc.AddVarcharPara("@invoice_id", 500, "");
            proc.AddVarcharPara("@module_name", 500, "SI");
            proc.AddVarcharPara("@AddOrEdit", 500, "Add");
            dt = proc.GetTable();

            Session["TDSGrid"] = dt;
            GridTDSdocs.DataBind();


        }
    }
}