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
using System.Linq;
using System.Collections.Specialized;
using BusinessLogicLayer.EmailDetails;
using EntityLayer.MailingSystem;
using UtilityLayer;
using ERP.OMS.Tax_Details.ClassFile;
using ERP.OMS.ViewState_class;
using System.Web.Script.Services;
using System.Resources;
using ERP.Models;


namespace ERP.OMS.Management.Activities
{
    public partial class PaymentRequisition : ERP.OMS.ViewState_class.VSPage
    {
        Export.ExportToPDF exportToPDF = new Export.ExportToPDF();

        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();

        MasterSettings objmaster = new MasterSettings();
        public static string IsLighterCustomePage = string.Empty;

        // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(Convert.ToString(Session["ErpConnection"]));
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


        clsDropDownList oclsDropDownList = new clsDropDownList();
        BusinessLogicLayer.Contact oContactGeneralBL = new BusinessLogicLayer.Contact();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        BusinessLogicLayer.RemarkCategoryBL reCat = new BusinessLogicLayer.RemarkCategoryBL();
        BusinessLogicLayer.Others objBL = new BusinessLogicLayer.Others();
        // GSTtaxDetails gstTaxDetails = new GSTtaxDetails();
        //GSTtaxDetails gstTaxDetails = new GSTtaxDetails();
        BusinessLogicLayer.CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();
        SalesInvoiceBL objSalesInvoiceBL = new SalesInvoiceBL();
        static string ForJournalDate = null;
        SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
        #region Sandip Section For Approval Section Start
        ERPDocPendingApprovalBL objERPDocPendingApproval = new ERPDocPendingApprovalBL();
        //PaymentRequisition objPaymentRequisition = new PaymentRequisition();
        #endregion Sandip Section For Approval Dtl Section End
        string UniqueQuotation = string.Empty;
        public string pageAccess = "";
        string userbranch = "";
        string QuotationIds = string.Empty;

        DataTable dtPartyTotal = null;
        string PartyTotalBalAmt = "";
        string PartyTotalBalDesc = "";

        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            #region Sandip Section For Approval Section Start
            if (Request.QueryString.AllKeys.Contains("status") || Request.QueryString.AllKeys.Contains("SalId") || Request.QueryString.AllKeys.Contains("IsTagged"))
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/PopUp.Master";

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

        protected void Page_Load(object sender, EventArgs e)
        {
            hdnhidejobno.Value = "0";
            txt_SlOrderNo.ClientEnabled = false;
            tagged.Visible = false;
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            //DropDown Bind For Mode
            SqlDataAdapter damodebind = new SqlDataAdapter("select Paymentreqmode_Id as ID,Description from Trans_Paymentreqmode", con);
            DataTable dtmode = new DataTable();
            damodebind.Fill(dtmode);
            if (dtmode.Rows.Count > 0)
            {
                ddl_mode.TextField = "Description";
                ddl_mode.ValueField = "ID";
                ddl_mode.DataSource = dtmode;
                ddl_mode.DataBind();
                //  ddl_mode.SelectedIndex = 0;
            }

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/CustomerMasterList.aspx");
            if (HttpContext.Current.Session["userid"] == null)
            {
                // Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!String.IsNullOrEmpty(Request.QueryString["SalId"]))
            {
                //Cross_CloseWindow.Visible = false;
                //divcross.Visible = false;
            }

            //PopulateCustomerDetail();

            CommonBL ComBL = new CommonBL();
            string ProjectSelectInEntryModule = ComBL.GetSystemSettingsResult("ProjectSelectInEntryModule");


            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {
                    hdnProjectSelectInEntryModule.Value = "1";
                    //lookup_Project.ClientVisible = true;
                    divjob.Visible = true;
                    hdnhidejobno.Value = "0";
                    //lblProject.Visible = true;
                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    hdnProjectSelectInEntryModule.Value = "0";
                    //lookup_Project.ClientVisible = false;
                    divjob.Visible = false;
                    hdnhidejobno.Value = "1";
                    //lblProject.Visible = false;

                }
            }


            string RequiredSIPParty = ComBL.GetSystemSettingsResult("PlaceOfSupplyShipParty");

            if (!String.IsNullOrEmpty(RequiredSIPParty))
            {
                if (RequiredSIPParty == "Yes")
                {

                    hdnPlaceShiptoParty.Value = "1";
                }
                else if (RequiredSIPParty.ToUpper().Trim() == "NO")
                {
                    hdnPlaceShiptoParty.Value = "0";

                }
            }

            string HierarchySelectInEntryModule = ComBL.GetSystemSettingsResult("Show_Hierarchy");


            if (!IsPostBack)
            {


                Session["Entry_Type"] = null;
                #region Distance Calculator in Sales Order

                uniqueId.Value = Guid.NewGuid().ToString();
                hdnGuid.Value = uniqueId.Value;

                hdnIsDistanceCalculate.Value = "N";
                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(Convert.ToString(Session["ErpConnection"]));
                DataTable DT = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='DistanceCalculator_SOrder' AND IsActive=1");
                if (DT != null && DT.Rows.Count > 0)
                {
                    string IsMandatory = Convert.ToString(DT.Rows[0]["Variable_Value"]).Trim();
                    if (IsMandatory == "Yes")
                    {
                        hdnIsDistanceCalculate.Value = "Y";
                    }
                }

                #endregion

                hidIsLigherContactPage.Value = "0";
                IsLighterCustomePage = "";
                CommonBL cbl = new CommonBL();
                string ISLigherpage = cbl.GetSystemSettingsResult("LighterCustomerEntryPage");
                if (!String.IsNullOrEmpty(ISLigherpage))
                {
                    if (ISLigherpage == "Yes")
                    {
                        hidIsLigherContactPage.Value = "1";
                        IsLighterCustomePage = "1";
                    }
                }

                hdnnproductIds.Value = "";
                hddnOutStandingBlock.Value = "0";
                //SetTaxJSONData();

                #region Approval Section By Sam on 23052017 Start
                string branchid = "";
                string branch = "";


                if (Request.QueryString.AllKeys.Contains("status"))
                {
                    DataTable dt = objCRMSalesOrderDtlBL.GetBranchIdBySOID(Convert.ToString(Request.QueryString["key"]));
                    branchid = Convert.ToString(dt.Rows[0]["br"]);
                    branch = oDBEngine.getBranch(branchid, "") + branchid;

                    HttpContext.Current.Session["userbranchHierarchy"] = branch;
                    Session["LastCompany"] = Convert.ToString(dt.Rows[0]["comp"]);
                    Session["LastFinYear"] = Convert.ToString(dt.Rows[0]["finyear"]);

                    ddl_Branch.Enabled = false;


                }
                else
                {
                    branchid = Convert.ToString(Session["userbranchID"]);
                    branch = oDBEngine.getBranch(branchid, "") + branchid;

                    ddl_Branch.Enabled = false; // Change Due to Numbering Schema

                }
                #endregion Approvalval Section By Sam on 23052017 Start
                if (Request.QueryString["Permission"] != null)
                {
                    //RAJDIP
                    //if (Convert.ToString(Request.QueryString["Permission"]) == "1")
                    //{
                    //    pnl_quotation.Enabled = true;
                    //}
                    //else if (Convert.ToString(Request.QueryString["Permission"]) == "2")
                    //{
                    //    pnl_quotation.Enabled = true;
                    //}
                    //else if (Convert.ToString(Request.QueryString["Permission"]) == "3")
                    //{
                    //    pnl_quotation.Enabled = true;
                    //}
                    //RAJDIP
                }

                if (Convert.ToString(Request.QueryString["tab"]) != "")
                {
                    if (Convert.ToString(Request.QueryString["tab"]) == "billship")
                    {
                        //ASPxPageControl1.ActiveTabIndex = 2;
                        Session["tab"] = Request.QueryString["tab"];
                        hdntab2.Value = "2";
                    }
                }
                else
                {
                    //RAJDIP
                    //ASPxPageControl1.ActiveTabIndex = 0;
                    //RAJDIP
                    hdntab2.Value = "0";
                }

                if (Session["Entry_Type"] != null)
                {
                    //RAJDIP
                    //ddlInventory.SelectedValue = (string)Session["Entry_Type"];
                    //RAJDIP
                }

                Session["Entry_Type"] = null;


                string finyear = Convert.ToString(Session["LastFinYear"]);
                //GetAllDropDownDetailForSalesOrder();
                if (Session["userbranchHierarchy"] != null)
                {
                    userbranch = Convert.ToString(Session["userbranchHierarchy"]);
                }
                GetAllDropDownDetailForSalesOrder(userbranch);

                //Tanmoy Hierarchy
                bindHierarchy();

                Session["SalesOrderBillingAddressLookup"] = null;
                Session["SalesOrdeShippingAddressLookup"] = null;
                Session["SalesOrderAddressDtl"] = null;
                Session["CustomerDetail"] = null;
                Session["SalesWarehouseData"] = null;
                Session["LoopSalesOrderWarehouse"] = 1;
                Session["STaxDetails"] = null;
                Session["SalesOrderTaxDetails"] = null;
                Session["PayReqDetails"] = null;
                Session["exportval1"] = null;
                Session["MultiUOMData"] = null;
                Session["ActionType"] = null;
                //PopulateGSTCSTVATCombo(DateTime.Now.ToString("yyyy-MM-dd"));
                //PopulateChargeGSTCSTVATCombo(DateTime.Now.ToString("yyyy-MM-dd"));

                string strOrderId = "";
                //RAJDIP
                //if (Request.QueryString["key"] != null)
                //{
                //    if (Convert.ToString(Request.QueryString["key"]) != "ADD")
                //    {
                //        ddl_AmountAre.ClientEnabled = false;
                //        txtCustName.ClientEnabled = false;
                //        hdnPageStatus.Value = "update";
                //        ltrTitle.Text = "Modify Sales Order";
                //        strOrderId = Convert.ToString(Request.QueryString["key"]);
                //        Session["OrderID"] = strOrderId;
                //        Session["ActionType"] = "Edit";
                //        hdAddOrEdit.Value = "Edit";
                //        hddnActionFieldOnStockBlock.Value = "Edit";
                //        #region Sandip Section For Approval Section Start
                //        if (Request.QueryString["status"] == null)
                //        {
                //            IsExistsDocumentInERPDocApproveStatus(strOrderId);
                //        }
                //        #endregion Sandip Section For Approval Dtl Section End
                //        //   Session["STaxDetails"] = GetTaxData();
                //        Session["SalesWarehouseData"] = GetOrderWarehouseData();
                //        Session["OrderDetails"] = GetOrderData().Tables[0];
                //        ddl_Branch.Enabled = false;





                //        //kaushik 25-2-2017

                //        Session["KeyVal_InternalID"] = "PISO" + strOrderId;
                //        //kaushik 25-2-2017
                //        ddl_numberingScheme.Enabled = false;
                //        txt_SlOrderNo.ClientEnabled = false;
                //        //GetAllDropDownDetailForSalesOrder();
                //        if (Session["userbranchHierarchy"] != null)
                //        {
                //            userbranch = Convert.ToString(Session["userbranchHierarchy"]);
                //        }
                //        GetAllDropDownDetailForSalesOrder(userbranch);
                //        SetOrderDetails();

                //        #region Debjyoti Get Tax Details in Edit Mode

                //        Session["STaxDetails"] = GetTaxDataWithGST(GetTaxData(dt_PLSales.Date.ToString("yyyy-MM-dd")));


                //        // Session["SalesOrderTaxDetails"] = GetSalesOrderTaxData().Tables[0];
                //        DataTable quotetable = GetQuotationEditedTaxData().Tables[0];
                //        if (quotetable == null)
                //        {
                //            CreateDataTaxTable();
                //        }
                //        else
                //        {
                //            Session["SalesOrderFinalTaxRecord" + Convert.ToString(uniqueId.Value)] = quotetable;
                //        }

                //        Session["MultiUOMData"] = GetMultiUOMData();
                //        #endregion Debjyoti Get Tax Details in Edit Mode
                //        //rev rajdip for running data on edit mode

                //        DataTable Orderdt = GetOrderData().Tables[0];
                //        decimal TotalQty = Orderdt.AsEnumerable().Sum(x => x.Field<decimal>("Quantity"));
                //        decimal TotalAmt = Orderdt.AsEnumerable().Sum(x => x.Field<decimal>("Amount"));
                //        decimal TotalTaxableAmt = Orderdt.AsEnumerable().Sum(x => x.Field<decimal>("TaxAmount"));
                //        decimal saleprice = Orderdt.AsEnumerable().Sum(x => x.Field<decimal>("SalePrice"));
                //        decimal AmountWithTaxValue = TotalAmt + TotalTaxableAmt;
                //        ASPxLabel12.Text = TotalQty.ToString();
                //        bnrLblTaxableAmtval.Text = TotalTaxableAmt.ToString();
                //        bnrLblTaxAmtval.Text = TotalTaxableAmt.ToString();
                //        bnrlblAmountWithTaxValue.Text = AmountWithTaxValue.ToString();
                //        bnrLblInvValue.Text = saleprice.ToString();
                //        grid.DataSource = GetSalesOrder();
                //        grid.DataBind();
                //        Session["SalesOrderAddressDtl"] = GetBillingAddress();
                //        hdnmodeId.Value = "SalesOrder" + strOrderId;
                //    }
                //    else
                //    {
                //        ltrTitle.Text = "Add Sales Order";
                //        Session["ActionType"] = "Add";
                //        hdnmodeId.Value = "Add";
                //        hdAddOrEdit.Value = "Add";
                //        hddnActionFieldOnStockBlock.Value = "Add";
                //        hdnPageStatus.Value = "first";
                //        Session["OrderID"] = "";
                //        CreateDataTaxTable();

                //        if (Request.QueryString["BasketId"] != null)
                //        {
                //            string basketId = Convert.ToString(Request.QueryString["BasketId"]);
                //            fillRecordFromBasket(basketId);
                //            hdBasketId.Value = basketId;
                //            ddl_AmountAre.Value = "2";
                //        }
                //        else
                //        {
                //            ddl_AmountAre.Value = objmaster.GetSettings("DefaultTaxTypeForSalesOrder");
                //            hdBasketId.Value = "";
                //        }
                //    }
                //}

                //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GridCallBack()", true);

                //IsUdfpresent.Value = Convert.ToString(getUdfCount());


                //#region Samrat Roy -- Hide Save Button in Edit Mode

                //#endregion
                //if (lookup_quotation.GridView.GetSelectedFieldValues("Quote_Id").Count > 0)
                //{
                //    dt_PLSales.ClientEnabled = false;
                //    // dt_Quotation.Text
                //}
                //else
                //{
                //    if (Convert.ToString(hddnDocumentIdTagged.Value) == "1")
                //    {
                //        dt_PLSales.ClientEnabled = false;
                //    }
                //    else
                //    {
                //        dt_PLSales.ClientEnabled = true;
                //        //dt_PLSales.MinDate =Convert.ToDateTime(dt_Quotation.Text);
                //    }

                //}
                if (Convert.ToString(hddnDocumentIdTagged.Value) == "1")
                {
                    //ltrTitle.Text = "View Sales Order";
                    //lbl_quotestatusmsg.Text = "*** Used in other module.";
                    //btn_SaveRecords.Visible = false;
                    //ASPxButton1.Visible = false;
                    //lbl_quotestatusmsg.Visible = true;
                    //dt_PLSales.ClientEnabled = false;
                }

                #region By Surojit get key value for Convertion Overide

                hdnConvertionOverideVisible.Value = objmaster.GetSettings("ConvertionOverideVisible");
                hdnShowUOMConversionInEntry.Value = objmaster.GetSettings("ShowUOMConversionInEntry");

                #endregion
                Session["ActionType"] = "Add";
                if (Convert.ToString(Request.QueryString["key"]) != "ADD")
                {
                    btn_SaveRecords.ClientVisible = false;
                    bindgriddata();
                    if (Request.QueryString["req"] != null && Request.QueryString["req"] == "V")
                    {
                        // ltrTitle.Text = "View Sales Order";
                        lblcashfundreq.Text = "View Cash / Fund Requisition";
                        lbl_quotestatusmsg.Text = "*** View Mode Only";
                        btn_SaveRecords.Visible = false;
                        ASPxButton1.Visible = false;
                        lbl_quotestatusmsg.Visible = true;
                        ddl_Num.Visible = false;
                        txt_SlOrderNo.ClientEnabled = false;
                        //lookup_Project.ClientEnabled = false;
                        ddl_Branch.Enabled = false;
                        Session["ActionType"] = "View";
                        //bindgriddata();
                    }
                    else
                    {
                        Session["ActionType"] = "Edit";
                        lblcashfundreq.Text = "Cash / Fund Requisition Modify";
                        ddl_Num.Visible = false;
                        txt_SlOrderNo.ClientEnabled = false;
                        //lookup_Project.ClientEnabled = false;
                        ddl_Branch.Enabled = false;

                        //ddl_numberingScheme.Visible = false;

                        //bindgriddata();
                    }
                }
                else
                {

                }

            }
            #region Subhra Section Start


            #endregion Subhra Section End

            //if (Request.QueryString["key"] != null && Request.QueryString["key"] != "ADD")
            //{
            //    //SetOrderDetails();
            //    string strOrderId1 = Convert.ToString(Request.QueryString["key"]);
            //    Session["OrderID"] = strOrderId1;
            //    //Chinmoy Edited below line
            //    DataSet dsOrderEditdt = GetOrderEditData();
            //    DataTable OrderEditdt = dsOrderEditdt.Tables[0];
            //    if (OrderEditdt != null && OrderEditdt.Rows.Count > 0)
            //    {
            //        string Quoids = Convert.ToString(OrderEditdt.Rows[0]["Order_Quotation_Ids"]);
            //        Session["Lookup_QuotationIds"] = Quoids;
            //        string Order_Date = Convert.ToString(OrderEditdt.Rows[0]["Order_Date"]);grid.DataSource
            //        string Customer_Id = Convert.ToString(OrderEditdt.Rows[0]["Customer_Id"]);
            //        if (!String.IsNullOrEmpty(Quoids))
            //        {
            //            string[] eachQuo = Quoids.Split(',');
            //            if (eachQuo.Length > 1)//More than one quotation
            //            {
            //                dt_Quotation.Text = "Multiple Select Quotation Dates";
            //                BindLookUp(Customer_Id, Order_Date);
            //                foreach (string val in eachQuo)
            //                {
            //                    lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));

            //                }
            //                // lbl_MultipleDate.Attributes.Add("style", "display:block");
            //            }
            //            else if (eachQuo.Length == 1)//Single Quotation
            //            { 
            //                BindLookUp(Customer_Id, Order_Date);
            //                foreach (string val in eachQuo)
            //                {

            //                }
            //            }
            //            else//No Quotation selected
            //            {
            //                BindLookUp(Customer_Id, Order_Date);
            //            }
            //        }
            //    }
            //}

            //PopulateCustomerDetail();
            GetSalesOrderSchemaLength();
        }
        public void bindgriddata()
        {
            string reqid = Convert.ToString(Request.QueryString["key"]);
            DataSet dsreqdetails = new DataSet();
            DataTable dtreqdetails = new DataTable();
            //dtsalesproduct = objPaymentRequisition.getpaymentrequisitiondetails(reqid);
            dsreqdetails = getpaymentrequisitiondetails(reqid);
            dtreqdetails = dsreqdetails.Tables[0];
            if (dtreqdetails.Rows.Count > 0)
            {
                string name = dtreqdetails.Rows[0]["Name"].ToString();
                //string lbl_NumberingScheme = dtreqdetails.Rows[0]["Name"].ToString();
                string jobno = dtreqdetails.Rows[0]["Job_No"].ToString();
                string branch_id = dtreqdetails.Rows[0]["Branch_id"].ToString();
                string servicename = dtreqdetails.Rows[0]["Service_Name"].ToString();
                int mode = Convert.ToInt32(dtreqdetails.Rows[0]["Mode"].ToString());
                string docno = dtreqdetails.Rows[0]["Paymentrequisition_Number"].ToString();
                string numberingschemaid = dtreqdetails.Rows[0]["NumberingSchema_Id"].ToString();
                string Date = dtreqdetails.Rows[0]["Date"].ToString();
                //ddl_numberingScheme.SelectedValue = hdnnumschema;
                hdnnumschema.Value = numberingschemaid;
                dt_date.Date = Convert.ToDateTime(Date);
                txt_SlOrderNo.Text = docno;
                txtservicename.Text = servicename;
                txtname.Text = name;
                lookup_Project.GridView.Selection.SelectRowByKey(Convert.ToInt64(jobno.ToString()));
                if (mode == 0)
                {
                    ddl_mode.Value = null;
                }
                else
                {
                    ddl_mode.Value = mode.ToString();
                }
                ddl_Branch.SelectedValue = branch_id;


                Session["PayReqDetails"] = dtreqdetails;
                //grid.DataSource = dtreqdetails;
                //grid.DataBind();

                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("prc_GetTagModuleInCashFunReq", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", reqid);

                cmd.Parameters.Add("@Flag", SqlDbType.VarChar, 50);
                cmd.Parameters["@Flag"].Direction = ParameterDirection.Output;
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(ds);
                int ReturnValue = Convert.ToInt32(cmd.Parameters["@Flag"].Value.ToString());
                if (ReturnValue > 0)
                {
                    ASPxButton1.ClientVisible = false;
                    btn_SaveRecords.ClientVisible = false;
                    tagged.Visible = true;
                }



                grid.DataSource = GetPayReq(dtreqdetails);
                grid.DataBind();
            }
        }
        public DataSet getpaymentrequisitiondetails(string id)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_GetPaymentRequision");

            proc.AddVarcharPara("@Id", 30, id);

            ds = proc.GetDataSet();
            return ds;
        }

        #region Project Code Bind
        protected void EntityServerModeDataSalesOrder_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Proj_Id";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            BusinessLogicLayer.DBEngine BEngine = new BusinessLogicLayer.DBEngine();




            var q = from d in dc.V_ProjectLists
                    where d.ProjectStatus == "Approved" && d.ProjBracnchid == Convert.ToInt64(ddl_Branch.SelectedValue) //&& d.CustId == Convert.ToString(hdnCustomerId.Value)
                    orderby d.Proj_Id descending
                    select d;

            e.QueryableSource = q;

        }

        #endregion Project Code Bind
        //Tanmoy Hierarchy
        public void bindHierarchy()
        {
            DataTable hierarchydt = oDBEngine.GetDataTable("select 0 as ID ,'Select' as H_Name union select ID,H_Name from V_HIERARCHY");
            if (hierarchydt != null && hierarchydt.Rows.Count > 0)
            {
                //ddlHierarchy.DataTextField = "H_Name";
                //ddlHierarchy.DataValueField = "ID";
                //ddlHierarchy.DataSource = hierarchydt;
                //ddlHierarchy.DataBind();
            }
        }
        //End Tanmoy Hierarchy

        //Debjyoti GST on 30-06-2017
        //public void SetTaxJSONData()
        //{
        //    #region NewTaxblock
        //    string ItemLevelTaxDetails = string.Empty; string HSNCodewisetaxSchemid = string.Empty; string BranchWiseStateTax = string.Empty; string StateCodeWiseStateIDTax = string.Empty;
        //    gstTaxDetails.GetTaxData(ref ItemLevelTaxDetails, ref HSNCodewisetaxSchemid, ref BranchWiseStateTax, ref StateCodeWiseStateIDTax, "S");
        //    HDItemLevelTaxDetails.Value = ItemLevelTaxDetails;
        //    HDHSNCodewisetaxSchemid.Value = HSNCodewisetaxSchemid;
        //    HDBranchWiseStateTax.Value = BranchWiseStateTax;
        //    HDStateCodeWiseStateIDTax.Value = StateCodeWiseStateIDTax;


        //    #endregion
        //}
        //END

        #region Customer and Product Bind From Sales Phone Call


        void CallgridfromSales()
        {
            string Sproduct = "";
            //dt_PLSales.ClientEnabled = false;
            int id;
            string CustomerName = string.Empty;
            if (!string.IsNullOrEmpty(Request.QueryString["type"]) && !string.IsNullOrEmpty(Request.QueryString["SalId"]))
            {


                string strCustomer = oDBEngine.ExeSclar("select sls_contactlead_id from tbl_trans_sales where sls_id=" + Request.QueryString["SalId"]);
                hdnCustomerId.Value = strCustomer;
                CustomerName = oDBEngine.ExeSclar("select (isnull(TMC.cnt_firstName,'')+' '+ isnull(TMC.cnt_middleName,'')+' ' +isnull(TMC.cnt_LastName,'')) as CustomerName from tbl_master_contact TMC where TMC.cnt_internalId='" + strCustomer + "'");
                if (!string.IsNullOrEmpty(strCustomer))
                {
                    //txtCustName.Text = CustomerName;
                }

                DataTable dt = GetCustomerOnIndustry(Convert.ToInt32(Request.QueryString["SalId"]));
                if (dt != null && dt.Rows.Count > 0)
                {
                    hddnCustomers.Value = Convert.ToString(dt.Rows[0]["EntityId"]).TrimStart(',');
                }



                DataTable dtProducts = GetProductsOnIndustry(Convert.ToInt32(Request.QueryString["SalId"]));

                if (dtProducts != null && dtProducts.Rows.Count > 0)
                {
                    hdnnproductIds.Value = Convert.ToString(dtProducts.Rows[0]["ProductIds"]).TrimStart(',');
                }

                //hdnnproductIds.Value = Sproduct.TrimStart(',');

                DataTable dtsalesproduct = new DataTable();
                dtsalesproduct = objSlaesActivitiesBL.GetQuotationDetailsFromSalesOrder_PhoneCall(Sproduct);
                //grid.DataSource = GetSalesOrderInfo1(dtsalesproduct, "");
                //grid.DataSource = null;
                // grid.DataBind();

                hdnIsFromActivity.Value = "Y";
                //btn_SaveRecords.Visible = false;
            }
        }



        #endregion

        //kaushik 24-2-2017


        protected int getUdfCount()
        {
            DataTable udfCount = oDBEngine.GetDataTable("select 1 from tbl_master_remarksCategory rc where cat_applicablefor='SO'  and ( exists (select * from tbl_master_udfGroup where id=rc.cat_group_id and grp_isVisible=1) or rc.cat_group_id=0)");
            return udfCount.Rows.Count;
        }

        //kaushik 24-2-2017
        public void GetSalesOrderSchemaLength()
        {
            DataTable Dt = new DataTable();
            Dt = objBL.GetSchemaLengthSalesOrder();
            if (Dt != null && Dt.Rows.Count > 0)
            {
                hdnSchemaLength.Value = Convert.ToString(Dt.Rows[0]["length"]);

            }

        }


        protected void MultiUOM_DataBinding(object sender, EventArgs e)
        {

        }


        public void bindLookUP(string status)
        {
            DataTable QuotationTable;
        }

        #region OutstandingReportExport(16-04-2018)


        #endregion




        public void GetAllDropDownDetailForSalesOrder(string UserBranch)
        {
            DataSet dst = new DataSet();
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string strBranchID = Convert.ToString(Session["userbranchID"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            dst = objSlaesActivitiesBL.GetAllDropDownDetailForSalesOrder(UserBranch);

            SlaesActivitiesBL objSlaesActivitiesBL1 = new SlaesActivitiesBL();
            DataTable Schemadt = objSlaesActivitiesBL1.GetNumberingSchema(strCompanyID, userbranchHierarchy, FinYear, "125", "Y");
            if (dst.Tables[0] != null && dst.Tables[0].Rows.Count > 0)
            {
                //ddl_numberingScheme.DataTextField = "SchemaName";
                //ddl_numberingScheme.DataValueField = "Id";
                //ddl_numberingScheme.DataSource = dst.Tables[0];
                //ddl_numberingScheme.DataBind();

            }
            if (Schemadt != null && Schemadt.Rows.Count > 0)
            {
                ddl_numberingScheme.DataTextField = "SchemaName";
                ddl_numberingScheme.DataValueField = "Id";
                ddl_numberingScheme.DataSource = Schemadt;
                ddl_numberingScheme.DataBind();
            }
            if (dst.Tables[1] != null && dst.Tables[1].Rows.Count > 0)
            {
                ddl_Branch.DataTextField = "branch_description";
                ddl_Branch.DataValueField = "branch_id";
                ddl_Branch.DataSource = dst.Tables[1];
                ddl_Branch.DataBind();
                ddl_Branch.Items.Insert(0, new ListItem("Select", "0"));
            }
            if (dst.Tables[2] != null && dst.Tables[2].Rows.Count > 0)
            {
                //ddl_SalesAgent.DataTextField = "Name";
                //ddl_SalesAgent.DataValueField = "cnt_id";
                //ddl_SalesAgent.DataSource = dst.Tables[2];
                //ddl_SalesAgent.DataBind();
            }
            if (dst.Tables[3] != null && dst.Tables[3].Rows.Count > 0)
            {
                //ddl_Currency.DataTextField = "Currency_Name";
                //ddl_Currency.DataValueField = "Currency_ID";
                //ddl_Currency.DataSource = dst.Tables[3];
                //ddl_Currency.DataBind();
            }
            if (dst.Tables[4] != null && dst.Tables[4].Rows.Count > 0)
            {


            }
            if (dst.Tables[5] != null && dst.Tables[5].Rows.Count > 0)
            {

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


        }

        [WebMethod]
        public static object GetContactPersonafterBillingShipping(string Key)
        {

            DataTable dtContactPerson = new DataTable();
            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            List<ContactPerson> contactPerson = new List<ContactPerson>();
            dtContactPerson = objSlaesActivitiesBL.PopulateContactPersonOfCustomer(Key);
            if (dtContactPerson != null && dtContactPerson.Rows.Count > 0)
            {

                if (dtContactPerson != null && dtContactPerson.Rows.Count > 0)
                {
                    for (int i = 0; i < dtContactPerson.Rows.Count; i++)
                    {
                        contactPerson.Add(new ContactPerson
                        {
                            Id = Convert.ToInt32(dtContactPerson.Rows[i]["add_id"]),
                            Name = Convert.ToString(dtContactPerson.Rows[i]["contactperson"])
                        });
                    }
                }
            }
            return contactPerson;
        }

        [WebMethod]
        public static string GetCustomerReletedData(string CustomerID)
        {
            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            DataSet dtCustomer = objSlaesActivitiesBL.GetCustomerDetails_CDRelated(CustomerID);
            string strStatus = "";
            if (dtCustomer.Tables[0] != null && dtCustomer.Tables[0].Rows.Count > 0)
            {
                strStatus = Convert.ToString(dtCustomer.Tables[0].Rows[0]["Statustype"]);
            }
            return strStatus;
        }

        [WebMethod]
        public static object GetLastRates(string ProductID)
        {
            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            DataTable RateTable = objSlaesActivitiesBL.GetRateDetails(ProductID);
            List<RateList> RateLists = new List<RateList>();
            RateLists = DbHelpers.ToModelList<RateList>(RateTable);
            return RateLists;
        }

        [WebMethod]
        public static object AutoPopulateAltQuantity(Int64 ProductID)
        {
            List<MultiUOMPacking> RateLists = new List<MultiUOMPacking>();

            decimal packing_quantity = 0;
            decimal sProduct_quantity = 0;
            Int32 AltUOMId = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
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


        [WebMethod]
        public static Int32 GetQuantityfromSL(string SLNo)
        {

            DataTable dt = new DataTable();
            int SLVal = 0;
            if (HttpContext.Current.Session["MultiUOMData"] != null)
            {
                dt = (DataTable)HttpContext.Current.Session["MultiUOMData"];
                DataRow[] MultiUoMresult = dt.Select("SrlNo ='" + SLNo + "'");

                SLVal = MultiUoMresult.Length;


            }

            return SLVal;
        }



        [WebMethod]
        public static object GetPackingQuantity(Int32 UomId, Int32 AltUomId, Int64 ProductID)
        {
            List<MultiUOMPacking> RateLists = new List<MultiUOMPacking>();

            decimal packing_quantity = 0;
            decimal sProduct_quantity = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
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


        public class RateList
        {
            public string Order_Number { get; set; }
            public string Order_Date { get; set; }
            public string cnt_firstName { get; set; }

            public string OrderDetails_SalePrice { get; set; }

        }

        public class MultiUOMPacking
        {
            public decimal packing_quantity { get; set; }
            public decimal sProduct_quantity { get; set; }
            public Int32 AltUOMId { get; set; }
        }


        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
        }
        protected void grid_DataBinding(object sender, EventArgs e)
        {
            //    if (Session["OrderDetails"] != null)
            //    {
            //        DataTable Quotationdt = (DataTable)Session["OrderDetails"];
            //        DataView dvData = new DataView(Quotationdt);
            //        dvData.RowFilter = "Status <> 'D'";
            //        grid.DataSource = GetSalesOrder(dvData.ToTable());
            //    }
            if (Session["PayReqDetails"] != null)
            {
                DataTable Quotationdt = (DataTable)Session["PayReqDetails"];
                DataView dvData = new DataView(Quotationdt);
                //dvData.RowFilter = "Status <> 'D'";
                grid.DataSource = GetPayReq(dvData.ToTable());

            }
        }
        protected void ddl_VatGstCst_Callback(object sender, CallbackEventArgsBase e)
        {
            string type = e.Parameter.Split('~')[0];
            //PopulateGSTCSTVAT(type);
        }

        protected void grid_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            //if (e.Column.FieldName == "Number")
            //{
            //    e.Value = string.Format("Item #{0}", e.ListSourceRowIndex);
            //}
            //if (e.Column.FieldName == "Warehouse")
            //{
            //    //e.Value = string.Format("Item #{0}", e.ListSourceRowIndex);
            //    //e.Row.Cells[6].Attributes.Add("onclick", "javascript:ShowMissingData('" + ContactID + "');");
            //}

        }
        protected void grid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {

        }
        protected void grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {

        }
        #region Batch Edit Grid Function- Sudip on 20/01/2017

        public class Product
        {
            public string ProductID { get; set; }
            public string ProductName { get; set; }
        }
        public class Taxes
        {
            public string TaxID { get; set; }
            public string TaxName { get; set; }
            public string Percentage { get; set; }
            public string Amount { get; set; }
            public decimal calCulatedOn { get; set; }
        }
        public class SalesOrder
        {
            public string SrlNo { get; set; }
            public string OrderID { get; set; }
            public string ProductID { get; set; }
            public string Description { get; set; }
            public string Quantity { get; set; }
            public string UOM { get; set; }
            public string Warehouse { get; set; }
            public string StockQuantity { get; set; }
            public string StockUOM { get; set; }
            public string SalePrice { get; set; }
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
            public int TaxAmountType { get; set; }

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

        public DataSet GetBasketData()
        {
            DataSet dt = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "BhasketOrderDetails");
            proc.AddBigIntegerPara("@SalesOrder_Id", Convert.ToInt64(Request.QueryString["BasketId"]));
            dt = proc.GetDataSet();
            return dt;
        }

        public DataTable GetMultiUOMData()
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "MultiUOMQuotationDetails");
            proc.AddVarcharPara("@SalesOrder_Id", 500, Convert.ToString(Session["OrderID"]));
            ds = proc.GetTable();
            return ds;
        }
        public DataSet GetProductData()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "ProductDetails");
            ds = proc.GetDataSet();
            return ds;
        }

        public IEnumerable GetSalesOrder()
        {
            List<SalesOrder> OrderList = new List<SalesOrder>();
            DataTable Orderdt = GetOrderData().Tables[0];
            DataColumnCollection dtC = Orderdt.Columns;
            for (int i = 0; i < Orderdt.Rows.Count; i++)
            {
                SalesOrder Orders = new SalesOrder();

                Orders.SrlNo = Convert.ToString(Orderdt.Rows[i]["SrlNo"]);
                //Orders.OrderID = Convert.ToString(Orderdt.Rows[i]["OrderID"]);
                Orders.OrderID = Convert.ToString(Orderdt.Rows[i]["OrderID"]);
                Orders.ProductID = Convert.ToString(Orderdt.Rows[i]["ProductID"]);
                Orders.Description = Convert.ToString(Orderdt.Rows[i]["Description"]);
                Orders.Quantity = Convert.ToString(Orderdt.Rows[i]["Quantity"]);
                Orders.UOM = Convert.ToString(Orderdt.Rows[i]["UOM"]);
                Orders.Warehouse = "";
                Orders.StockQuantity = Convert.ToString(Orderdt.Rows[i]["StockQuantity"]);
                Orders.StockUOM = Convert.ToString(Orderdt.Rows[i]["StockUOM"]);
                Orders.SalePrice = Convert.ToString(Orderdt.Rows[i]["SalePrice"]);
                Orders.Discount = Convert.ToString(Orderdt.Rows[i]["Discount"]);
                Orders.Amount = Convert.ToString(Orderdt.Rows[i]["Amount"]);
                Orders.TaxAmount = Convert.ToString(Orderdt.Rows[i]["TaxAmount"]);
                Orders.TotalAmount = Convert.ToString(Orderdt.Rows[i]["TotalAmount"]);

                if (!string.IsNullOrEmpty(Convert.ToString(Orderdt.Rows[i]["Quotation_No"])))//Added on 15-02-2017
                { Orders.Quotation_No = Convert.ToInt64(Orderdt.Rows[i]["Quotation_No"]); }
                else
                { Orders.Quotation_No = 0; }
                OrderList.Add(Orders);
                if (dtC.Contains("Quotation"))
                {
                    Orders.Quotation_Num = Convert.ToString(Orderdt.Rows[i]["Quotation"]);//subhabrata on 21-02-2017
                }
                Orders.ProductName = Convert.ToString(Orderdt.Rows[i]["ProductName"]);

            }

            return OrderList;
        }
        public class Payreq
        {
            public string SrlNo { get; set; }
            public string Name { get; set; }
            public string Paymentrequisition_Number { get; set; }
            public string Date { get; set; }
            public string Job_No { get; set; }
            public string Branch_Id { get; set; }
            public string Service_Name { get; set; }
            public string Mode { get; set; }
            public string Particulars { get; set; }
            public string PaymentreqDetails_Id { get; set; }
            public string Amount { get; set; }
            public string Remarks { get; set; }
            //public string TaxAmount { get; set; }
            //public string TotalAmount { get; set; }
            //public Int64 Quotation_No { get; set; }
            //public string Quotation_Num { get; set; }
            //public string Key_UniqueId { get; set; }
            //public string Product_Shortname { get; set; }
            //public string ProductName { get; set; }
            //public string QuoteDetails_Id { get; set; }
            //public int TaxAmountType { get; set; }

        }

        public IEnumerable GetPayReq(DataTable payreqdt)
        {
            List<Payreq> OrderList = new List<Payreq>();
            DataColumnCollection dtC = payreqdt.Columns;
            for (int i = 0; i < payreqdt.Rows.Count; i++)
            {
                Payreq payreq = new Payreq();

                payreq.SrlNo = Convert.ToString(payreqdt.Rows[i]["SrlNo"]);
                //Orders.OrderID = Convert.ToString(SalesOrderdt.Rows[i]["OrderID"]);
                payreq.Particulars = Convert.ToString(payreqdt.Rows[i]["Particulars"]);
                payreq.Remarks = Convert.ToString(payreqdt.Rows[i]["Remarks"]);
                payreq.Amount = Convert.ToString(payreqdt.Rows[i]["Amount"]);
                //payreq.Quantity = Convert.ToString(payreqdt.Rows[i]["Quantity"]);
                //payreq.UOM = Convert.ToString(payreqdt.Rows[i]["UOM"]);
                //payreq.Warehouse = "";
                //payreq.StockQuantity = Convert.ToString(payreqdt.Rows[i]["StockQuantity"]);
                //payreq.StockUOM = Convert.ToString(payreqdt.Rows[i]["StockUOM"]);
                //payreq.SalePrice = Convert.ToString(payreqdt.Rows[i]["SalePrice"]);
                //payreq.Discount = Convert.ToString(payreqdt.Rows[i]["Discount"]);
                //payreq.Amount = Convert.ToString(payreqdt.Rows[i]["Amount"]);
                //payreq.TaxAmount = Convert.ToString(payreqdt.Rows[i]["TaxAmount"]);
                //payreq.TotalAmount = Convert.ToString(payreqdt.Rows[i]["TotalAmount"]);
                //if (!string.IsNullOrEmpty(Convert.ToString(payreqdt.Rows[i]["Quotation_No"])))//Added on 15-02-2017
                //{ payreq.Quotation_No = Convert.ToInt64(payreqdt.Rows[i]["Quotation_No"]); }
                //else
                //{ payreq.Quotation_No = 0; }
                //if (dtC.Contains("Quotation"))
                //{
                //    payreq.Quotation_Num = Convert.ToString(payreqdt.Rows[i]["Quotation"]);//subhabrata on 21-02-2017
                //}
                //payreq.ProductName = Convert.ToString(payreqdt.Rows[i]["ProductName"]);

                OrderList.Add(payreq);
            }

            return OrderList;
        }
        public IEnumerable GetSalesOrder(DataTable SalesOrderdt)
        {
            List<SalesOrder> OrderList = new List<SalesOrder>();
            DataColumnCollection dtC = SalesOrderdt.Columns;
            for (int i = 0; i < SalesOrderdt.Rows.Count; i++)
            {
                SalesOrder Orders = new SalesOrder();

                Orders.SrlNo = Convert.ToString(SalesOrderdt.Rows[i]["SrlNo"]);
                //Orders.OrderID = Convert.ToString(SalesOrderdt.Rows[i]["OrderID"]);
                Orders.OrderID = Convert.ToString(SalesOrderdt.Rows[i]["OrderID"]);
                Orders.ProductID = Convert.ToString(SalesOrderdt.Rows[i]["ProductID"]);
                Orders.Description = Convert.ToString(SalesOrderdt.Rows[i]["Description"]);
                Orders.Quantity = Convert.ToString(SalesOrderdt.Rows[i]["Quantity"]);
                Orders.UOM = Convert.ToString(SalesOrderdt.Rows[i]["UOM"]);
                Orders.Warehouse = "";
                Orders.StockQuantity = Convert.ToString(SalesOrderdt.Rows[i]["StockQuantity"]);
                Orders.StockUOM = Convert.ToString(SalesOrderdt.Rows[i]["StockUOM"]);
                Orders.SalePrice = Convert.ToString(SalesOrderdt.Rows[i]["SalePrice"]);
                Orders.Discount = Convert.ToString(SalesOrderdt.Rows[i]["Discount"]);
                Orders.Amount = Convert.ToString(SalesOrderdt.Rows[i]["Amount"]);
                Orders.TaxAmount = Convert.ToString(SalesOrderdt.Rows[i]["TaxAmount"]);
                Orders.TotalAmount = Convert.ToString(SalesOrderdt.Rows[i]["TotalAmount"]);
                if (!string.IsNullOrEmpty(Convert.ToString(SalesOrderdt.Rows[i]["Quotation_No"])))//Added on 15-02-2017
                { Orders.Quotation_No = Convert.ToInt64(SalesOrderdt.Rows[i]["Quotation_No"]); }
                else
                { Orders.Quotation_No = 0; }
                if (dtC.Contains("Quotation"))
                {
                    Orders.Quotation_Num = Convert.ToString(SalesOrderdt.Rows[i]["Quotation"]);//subhabrata on 21-02-2017
                }
                Orders.ProductName = Convert.ToString(SalesOrderdt.Rows[i]["ProductName"]);

                OrderList.Add(Orders);
            }

            return OrderList;
        }
        public DataTable GetQuotationWarehouse(string strQuotationList)
        {
            try
            {
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
                proc.AddVarcharPara("@Action", 500, "OrderWarehouseByQuotation");
                proc.AddVarcharPara("@QuotationList", 3000, strQuotationList);
                dt = proc.GetTable();

                string strNewVal = "", strOldVal = "";
                DataTable tempdt = dt.Copy();
                foreach (DataRow drr in tempdt.Rows)
                {
                    strNewVal = Convert.ToString(drr["QuoteWarehouse_Id"]);

                    if (strNewVal == strOldVal)
                    {
                        drr["WarehouseName"] = "";
                        drr["Quantity"] = "";
                        drr["BatchNo"] = "";
                        drr["SalesUOMName"] = "";
                        drr["SalesQuantity"] = "";
                        drr["StkUOMName"] = "";
                        drr["StkQuantity"] = "";
                        drr["ConversionMultiplier"] = "";
                        drr["AvailableQty"] = "";
                        drr["BalancrStk"] = "";
                        drr["MfgDate"] = "";
                        drr["ExpiryDate"] = "";
                    }

                    strOldVal = strNewVal;
                }

                Session["LoopSalesOrderWarehouse"] = Convert.ToString(Convert.ToInt32(strNewVal) + 1);
                tempdt.Columns.Remove("QuoteWarehouse_Id");
                return tempdt;
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable GetQuotation(DataTable Quotationdt)
        {
            List<Quotation> QuotationList = new List<Quotation>();

            if (Quotationdt != null && Quotationdt.Rows.Count > 0)
            {
                for (int i = 0; i < Quotationdt.Rows.Count; i++)
                {
                    Quotation Quotations = new Quotation();

                    Quotations.SrlNo = Convert.ToString(Quotationdt.Rows[i]["SrlNo"]);
                    Quotations.QuotationID = Convert.ToString(Quotationdt.Rows[i]["QuotationID"]);
                    Quotations.ProductID = Convert.ToString(Quotationdt.Rows[i]["ProductID"]);
                    Quotations.Description = Convert.ToString(Quotationdt.Rows[i]["Description"]);
                    Quotations.Quantity = Convert.ToString(Quotationdt.Rows[i]["Quantity"]);
                    Quotations.UOM = Convert.ToString(Quotationdt.Rows[i]["UOM"]);
                    Quotations.Warehouse = "";
                    Quotations.StockQuantity = Convert.ToString(Quotationdt.Rows[i]["StockQuantity"]);
                    Quotations.StockUOM = Convert.ToString(Quotationdt.Rows[i]["StockUOM"]);
                    Quotations.SalePrice = Convert.ToString(Quotationdt.Rows[i]["SalePrice"]);
                    Quotations.Discount = Convert.ToString(Quotationdt.Rows[i]["Discount"]);
                    Quotations.Amount = Convert.ToString(Quotationdt.Rows[i]["Amount"]);
                    Quotations.TaxAmount = Convert.ToString(Quotationdt.Rows[i]["TaxAmount"]);
                    Quotations.TotalAmount = Convert.ToString(Quotationdt.Rows[i]["TotalAmount"]);
                    Quotations.ProductName = Convert.ToString(Quotationdt.Rows[i]["ProductName"]);
                    QuotationList.Add(Quotations);
                }
            }

            return QuotationList;
        }

        public class WaitingProductQuantity
        {
            public Int64 productid { get; set; }
            public Int32 pslno { get; set; }
            public Decimal pQuantity { get; set; }
            public Decimal packing { get; set; }

            public Int32 PackingUom { get; set; }
            public Int32 PackingSelectUom { get; set; }
        }
        public class ProductQuantity
        {
            public Int64 productid { get; set; }
            public Int32 slno { get; set; }
            public Decimal Quantity { get; set; }
            public Decimal packing { get; set; }

            public Int32 PackingUom { get; set; }
            public Int32 PackingSelectUom { get; set; }
        }


        public IEnumerable GetWaitingProductDetails(DataTable Quotationdt)
        {
            List<WaitingProductQuantity> QuotationList = new List<WaitingProductQuantity>();

            if (Quotationdt != null && Quotationdt.Rows.Count > 0)
            {
                for (int i = 0; i < Quotationdt.Rows.Count; i++)
                {
                    WaitingProductQuantity Quotations = new WaitingProductQuantity();

                    Quotations.productid = Convert.ToInt64(Quotationdt.Rows[i]["productid"]);
                    Quotations.pslno = Convert.ToInt32(Quotationdt.Rows[i]["pslno"]);
                    Quotations.pQuantity = Convert.ToDecimal(Quotationdt.Rows[i]["pQuantity"]);
                    Quotations.packing = Convert.ToDecimal(Quotationdt.Rows[i]["packing"]);
                    Quotations.PackingUom = Convert.ToInt32(Quotationdt.Rows[i]["PackingUom"]);
                    Quotations.PackingSelectUom = Convert.ToInt32(Quotationdt.Rows[i]["PackingSelectUom"]);

                    QuotationList.Add(Quotations);
                }
            }

            return QuotationList;
        }
        public IEnumerable GetBasketOrderDetails(DataTable Quotationdt)
        {
            List<QuotationForBasket> QuotationList = new List<QuotationForBasket>();

            if (Quotationdt != null && Quotationdt.Rows.Count > 0)
            {
                for (int i = 0; i < Quotationdt.Rows.Count; i++)
                {
                    QuotationForBasket Quotations = new QuotationForBasket();

                    Quotations.SrlNo = Convert.ToString(Quotationdt.Rows[i]["SrlNo"]);
                    Quotations.OrderID = Convert.ToString(Quotationdt.Rows[i]["OrderID"]);
                    Quotations.ProductID = Convert.ToString(Quotationdt.Rows[i]["ProductID"]);
                    Quotations.Description = Convert.ToString(Quotationdt.Rows[i]["Description"]);
                    Quotations.Quantity = Convert.ToString(Quotationdt.Rows[i]["Quantity"]);
                    Quotations.UOM = Convert.ToString(Quotationdt.Rows[i]["UOM"]);
                    Quotations.Warehouse = "";
                    Quotations.StockQuantity = Convert.ToString(Quotationdt.Rows[i]["StockQuantity"]);
                    Quotations.StockUOM = Convert.ToString(Quotationdt.Rows[i]["StockUOM"]);
                    Quotations.SalePrice = Convert.ToString(Quotationdt.Rows[i]["SalePrice"]);
                    Quotations.Discount = Convert.ToString(Quotationdt.Rows[i]["Discount"]);
                    Quotations.Amount = Convert.ToString(Quotationdt.Rows[i]["Amount"]);
                    Quotations.TaxAmount = Convert.ToString(Quotationdt.Rows[i]["TaxAmount"]);
                    Quotations.TotalAmount = Convert.ToString(Quotationdt.Rows[i]["TotalAmount"]);
                    Quotations.Status = Convert.ToString(Quotationdt.Rows[i]["Status"]);
                    Quotations.ProductName = Convert.ToString(Quotationdt.Rows[i]["ProductName"]);
                    // Quotations.Quotation_Num = Convert.ToString(Quotationdt.Rows[i]["Quotation_Num"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(Quotationdt.Rows[i]["Quotation_No"])))//Added on 15-02-2017
                    {
                        Quotations.Quotation_No = Convert.ToInt64(Quotationdt.Rows[i]["Quotation_No"]);
                    }
                    else
                    {
                        Quotations.Quotation_No = 0;
                    }
                    QuotationList.Add(Quotations);
                }
            }

            return QuotationList;
        }
        public DataTable GetQuotationProductTaxData(string strQuotationList)
        {
            try
            {
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
                proc.AddVarcharPara("@Action", 500, "OrderProductTaxByQuotation");
                proc.AddVarcharPara("@QuotationList", 3000, strQuotationList);
                dt = proc.GetTable();

                return dt;
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable GetSalesOrderInfo(DataTable SalesOrderdt1, string Order_Id)
        {
            List<SalesOrder> OrderList = new List<SalesOrder>();
            DataColumnCollection dtC = SalesOrderdt1.Columns;

            // Fetch All Warehouse Data , Product Tax Details

            string commaSeparatedString = String.Join(",", SalesOrderdt1.AsEnumerable().Select(x => x.Field<Int64>("QuoteDetails_Id").ToString()).ToArray());
            DataTable tempWarehouse = GetQuotationWarehouse(commaSeparatedString);
            DataTable tempProductTax = GetQuotationProductTaxData(commaSeparatedString);

            // End


            for (int i = 0; i < SalesOrderdt1.Rows.Count; i++)
            {
                SalesOrder Orders = new SalesOrder();

                Orders.SrlNo = Convert.ToString(i + 1);
                Orders.OrderID = Convert.ToString(i + 1);
                Orders.ProductID = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_ProductId"]);
                Orders.Description = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_ProductDescription"]);
                Orders.Quantity = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_Quantity"]);
                Orders.UOM = Convert.ToString(SalesOrderdt1.Rows[i]["UOM_ShortName"]);
                Orders.Warehouse = "";
                Orders.StockQuantity = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_StockQty"]);
                Orders.StockUOM = Convert.ToString(SalesOrderdt1.Rows[i]["UOM_ShortName"]);
                Orders.SalePrice = Convert.ToString(SalesOrderdt1.Rows[i]["SalePrice"]);
                Orders.Discount = Convert.ToString(SalesOrderdt1.Rows[i]["Discount"]);
                Orders.Amount = Convert.ToString(SalesOrderdt1.Rows[i]["Amount"]);
                Orders.TaxAmount = Convert.ToString(SalesOrderdt1.Rows[i]["TaxAmount"]);
                Orders.TotalAmount = Convert.ToString(SalesOrderdt1.Rows[i]["TotalAmount"]);
                if (!string.IsNullOrEmpty(Convert.ToString(SalesOrderdt1.Rows[i]["Quotation_No"])))//Added on 15-02-2017
                { Orders.Quotation_No = Convert.ToInt64(SalesOrderdt1.Rows[i]["Quotation_No"]); }
                else
                { Orders.Quotation_No = 0; }
                if (dtC.Contains("Quotation"))
                {
                    Orders.Quotation_Num = Convert.ToString(SalesOrderdt1.Rows[i]["Quotation"]);//subhabrata on 21-02-2017
                }
                Orders.ProductName = Convert.ToString(SalesOrderdt1.Rows[i]["ProductName"]);

                // Mapping With Warehouse with Product Srl No

                string strQuoteDetails_Id = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_Id"]).Trim();

                if (tempWarehouse != null && tempWarehouse.Rows.Count > 0)
                {
                    var rows = tempWarehouse.Select("QuoteDetails_Id ='" + strQuoteDetails_Id + "'");
                    foreach (var row in rows)
                    {
                        row["Product_SrlNo"] = Convert.ToString(i + 1);
                    }
                    tempWarehouse.AcceptChanges();
                }

                if (tempProductTax != null && tempProductTax.Rows.Count > 0)
                {
                    var taxrows = tempProductTax.Select("ProductTax_ProductId ='" + strQuoteDetails_Id + "'");
                    foreach (var row in taxrows)
                    {
                        row["SlNo"] = Convert.ToString(i + 1);
                    }
                    tempProductTax.AcceptChanges();
                }

                // End


                OrderList.Add(Orders);
                // Session["OrderDetails"] = OrderList;
            }

            if (tempWarehouse != null && tempWarehouse.Rows.Count > 0)
            {
                tempWarehouse.Columns.Remove("QuoteDetails_Id");
                Session["SalesWarehouseData"] = tempWarehouse;
            }
            else { Session["SalesWarehouseData"] = null; }

            if (tempProductTax != null)
            {
                tempProductTax.Columns.Remove("ProductTax_ProductId");
                Session["SalesOrderFinalTaxRecord" + Convert.ToString(uniqueId.Value)] = tempProductTax;
            }
            else { Session["SalesOrderFinalTaxRecord" + Convert.ToString(uniqueId.Value)] = null; }

            BindSessionByDatatable(SalesOrderdt1);

            return OrderList;
        }

        public IEnumerable GetSalesOrderInfo1(DataTable SalesOrderdt1, string Order_Id)
        {

            List<SalesOrder> OrderList = new List<SalesOrder>();
            DataColumnCollection dtC = SalesOrderdt1.Columns;





            for (int i = 0; i < SalesOrderdt1.Rows.Count; i++)
            {
                SalesOrder Orders = new SalesOrder();

                Orders.SrlNo = Convert.ToString(i + 1);
                Orders.OrderID = Convert.ToString(i + 1);
                Orders.ProductID = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_ProductId"]);
                Orders.Description = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_ProductDescription"]);
                Orders.ProductName = Convert.ToString(SalesOrderdt1.Rows[i]["ProductName"]);
                Orders.Quantity = "";
                Orders.UOM = "";
                Orders.Warehouse = "";
                Orders.StockQuantity = "";
                Orders.StockUOM = "";
                Orders.SalePrice = "";
                Orders.Discount = "";
                Orders.Amount = "";
                Orders.TaxAmount = "";
                Orders.TotalAmount = "";
                Orders.Quotation_No = 0;
                Orders.Quotation_Num = "";
                OrderList.Add(Orders);
                BindSessionByDatatable(SalesOrderdt1);
            }
            return OrderList;
        }


        #region Subhabrata/SessionBind
        //Subhabrata on 02-03-2017
        public bool BindSessionByDatatable(DataTable dt)
        {
            bool IsSuccess = false;
            DataTable SalesChalladt = new DataTable();
            SalesChalladt.Columns.Add("SrlNo", typeof(string));
            SalesChalladt.Columns.Add("OrderID", typeof(string));
            SalesChalladt.Columns.Add("ProductID", typeof(string));
            SalesChalladt.Columns.Add("Description", typeof(string));
            //SalesOrderdt.Columns.Add("Quotation", typeof(string));//Added By:subhabrata on 21-02-2017               
            SalesChalladt.Columns.Add("Quantity", typeof(string));
            SalesChalladt.Columns.Add("UOM", typeof(string));
            SalesChalladt.Columns.Add("Warehouse", typeof(string));
            SalesChalladt.Columns.Add("StockQuantity", typeof(string));
            SalesChalladt.Columns.Add("StockUOM", typeof(string));
            SalesChalladt.Columns.Add("SalePrice", typeof(string));
            SalesChalladt.Columns.Add("Discount", typeof(string));
            SalesChalladt.Columns.Add("Amount", typeof(string));
            SalesChalladt.Columns.Add("TaxAmount", typeof(string));
            SalesChalladt.Columns.Add("TotalAmount", typeof(string));
            SalesChalladt.Columns.Add("Status", typeof(string));
            SalesChalladt.Columns.Add("Quotation_No", typeof(string));
            SalesChalladt.Columns.Add("Quotation", typeof(string));
            SalesChalladt.Columns.Add("ProductName", typeof(string));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IsSuccess = true;
                DataColumnCollection dtC = dt.Columns;
                string SalePrice, Discount, Amount, TaxAmount, TotalAmount, Order_Num1, ProductName;
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["SalePrice"])))
                { SalePrice = Convert.ToString(dt.Rows[i]["SalePrice"]); }
                else
                { SalePrice = ""; }
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
                if (dtC.Contains("Quotation"))
                {
                    Order_Num1 = Convert.ToString(dt.Rows[i]["Quotation"]);//subhabrata on 21-02-2017
                }
                else
                {
                    Order_Num1 = "";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["ProductName"])))
                {
                    ProductName = Convert.ToString(dt.Rows[i]["ProductName"]);
                }
                else
                {
                    ProductName = "";
                }
                SalesChalladt.Rows.Add(Convert.ToString(i + 1), Convert.ToString(i + 1), Convert.ToString(dt.Rows[i]["QuoteDetails_ProductId"]), Convert.ToString(dt.Rows[i]["QuoteDetails_ProductDescription"]),
                    Convert.ToString(dt.Rows[i]["QuoteDetails_Quantity"]), Convert.ToString(dt.Rows[i]["UOM_ShortName"]), "", Convert.ToString(dt.Rows[i]["QuoteDetails_StockQty"]), Convert.ToString(dt.Rows[i]["UOM_ShortName"]),
                               SalePrice, Discount, Amount, TaxAmount, TotalAmount, "U", Convert.ToInt64(dt.Rows[i]["Quotation_No"]), Order_Num1, ProductName);
            }

            Session["OrderDetails"] = SalesChalladt;

            return IsSuccess;
        }//End

        #endregion

        public IEnumerable GetProductsInfo(DataTable SalesOrderdt1)
        {
            List<SalesOrder> OrderList = new List<SalesOrder>();
            for (int i = 0; i < SalesOrderdt1.Rows.Count; i++)
            {
                SalesOrder Orders = new SalesOrder();

                Orders.SrlNo = Convert.ToString(i + 1);
                Orders.Key_UniqueId = Convert.ToString(i + 1);
                if (!string.IsNullOrEmpty(Convert.ToString(SalesOrderdt1.Rows[i]["Quotation_No"])))
                { Orders.Quotation_No = Convert.ToInt64(SalesOrderdt1.Rows[i]["Quotation_No"]); }
                else
                { Orders.Quotation_No = 0; }
                Orders.ProductID = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_ProductId"]);
                Orders.Description = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_ProductDescription"]);
                Orders.Quantity = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_Quantity"]);
                Orders.Quotation_Num = Convert.ToString(SalesOrderdt1.Rows[i]["Quotation"]);
                Orders.Product_Shortname = Convert.ToString(SalesOrderdt1.Rows[i]["Product_Name"]);
                Orders.QuoteDetails_Id = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_Id"]);

                if (!string.IsNullOrEmpty(Convert.ToString(SalesOrderdt1.Rows[i]["TaxAmountType"])))
                { Orders.TaxAmountType = Convert.ToInt32(SalesOrderdt1.Rows[i]["TaxAmountType"]); }
                else
                {
                    Orders.TaxAmountType = 0;
                }
                OrderList.Add(Orders);
            }

            return OrderList;
        }

        public DataSet GetOrderData()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "OrderDetails");
            proc.AddVarcharPara("@Order_Id", 500, Convert.ToString(Session["OrderID"]));
            ds = proc.GetDataSet();
            return ds;
        }
        protected void Page_Init(object sender, EventArgs e)
        {

            //CustomerDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            //SalesManDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            //CountrySelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            //StateSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            //SelectCity.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            //SelectArea.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            //SelectPin.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            //sqltaxDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            //SqlCurrency.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            //SqlCurrencyBind.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            //UomSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            //AltUomSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void grid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            //if (e.Column.FieldName == "Particulars")
            //{
            //   // e.Editor.Enabled = true;
            //}
            //else if (e.Column.FieldName == "Amount")
            //{
            //    //e.Editor.Enabled = true;
            //}
            //else if (e.Column.FieldName == "Remarks")
            //{
            //    //e.Editor.Enabled = true;
            //}

            ////else if (e.Column.FieldName == "Amount")
            ////{
            ////    e.Editor.Enabled = true;
            ////}
            //else if (e.Column.FieldName == "TotalAmount")
            //{
            //    e.Editor.Enabled = true;
            //}
            //else if (e.Column.FieldName == "SrlNo")
            //{
            //    e.Editor.Enabled = true;
            //}
            //else if (e.Column.FieldName == "ProductName")
            //{
            //    e.Editor.Enabled = true;
            //}
            //else
            //{
            if (e.Column.FieldName == "SrlNo")
            { e.Editor.ReadOnly = true; }
            else
            { e.Editor.ReadOnly = false; }

            //}
        }

        public DataTable GetComponentEditedAddressData(string ComponentDetailsIDs, string strType)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GetBillingShippingQuotation");
            proc.AddVarcharPara("@Action", 500, "ComponentBillingAddress");
            proc.AddVarcharPara("@SelectedComponentList", 500, ComponentDetailsIDs);
            proc.AddVarcharPara("@ComponentType", 500, strType);
            ds = proc.GetTable();
            return ds;
        }


        [WebMethod]
        public static List<string> CheckBalQuantity(string Id, string ProductID)
        {
            DataTable dt = new DataTable();
            List<string> obj = new List<string>();
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            try
            {
                SlaesActivitiesBL objSalActBL = new SlaesActivitiesBL();
                dt = objSalActBL.GetBalQuantityForQuantiyCheck(Id, ProductID, "SalesOrder");

                foreach (DataRow dr in dt.Rows)
                {

                    obj.Add(Convert.ToString(dr["BalanceQty"]) + "|");
                }
            }
            catch (Exception ex)
            {

            }
            return obj;
        }

        [WebMethod]
        public static string GetCurrentConvertedRate(string CurrencyId)
        {

            string[] ActCurrency = new string[] { };

            string CompID = "";
            if (HttpContext.Current.Session["LastCompany"] != null)
            {
                CompID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);


            }
            string currentRate = "";
            if (HttpContext.Current.Session["ActiveCurrency"] != null)
            {
                string currency = Convert.ToString(HttpContext.Current.Session["ActiveCurrency"]);
                ActCurrency = currency.Split('~');
                int BaseCurrencyId = Convert.ToInt32(ActCurrency[0]);
                int ConvertedCurrencyId = Convert.ToInt32(CurrencyId);
                SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
                DataTable dt = objSlaesActivitiesBL.GetCurrentConvertedRate(BaseCurrencyId, ConvertedCurrencyId, CompID);
                if (dt != null && dt.Rows.Count > 0)
                {
                    currentRate = Convert.ToString(dt.Rows[0]["SalesRate"]);
                    return currentRate;
                }
            }
            return null;

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

        public DataTable GetTaxData(string quoteDate)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "TaxDetails");
            proc.AddVarcharPara("@Order_Id", 500, Convert.ToString(Session["OrderID"]));
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@branchId", 500, Convert.ToString(ddl_Branch.SelectedValue));
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            proc.AddVarcharPara("@S_OrderDate", 10, quoteDate);
            dt = proc.GetTable();
            return dt;
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


        [WebMethod]
        public static bool GetCustomerOutStanding(string strAsOnDate, string strCustomerId, string BranchId)
        {
            bool flag = false;
            DataTable dtOutStanding = new DataTable();

            try
            {

                SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
                dtOutStanding = objSlaesActivitiesBL.GetCustomerOutstandingRecords(strAsOnDate, strCustomerId, BranchId);
                if (dtOutStanding == null)
                {
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                flag = false;
            }
            finally
            {
            }
            return flag;
        }

        [WebMethod]
        public static string GetCustomerOutStandingAmount(string strAsOnDate, string strCustomerId, string BranchId)
        {
            bool flag = false;
            string OutStandingAmount = "";
            DataTable dtOutStanding = new DataTable();

            try
            {

                SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
                dtOutStanding = objSlaesActivitiesBL.GetCustomerOutstandingAmount(strAsOnDate, strCustomerId, BranchId);
                if (dtOutStanding != null && dtOutStanding.Rows.Count > 0)
                {
                    OutStandingAmount = Convert.ToString(dtOutStanding.Rows[0]["BAL_AMOUNT"]);
                }
            }
            catch (Exception ex)
            {
                flag = false;
            }
            finally
            {
            }
            return OutStandingAmount;
        }

        [WebMethod]
        public static string GetBranchAddress(string BranchId)
        {
            string strAddress = "";

            try
            {
                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();
                DataTable dt = objEngine.GetDataTable("tbl_master_Branch", " branch_address1 ", " branch_id='" + BranchId + "'");
                if (dt != null && dt.Rows.Count > 0)
                {
                    strAddress = Convert.ToString(dt.Rows[0]["branch_address1"]).Trim();
                }
            }
            catch (Exception ex)
            {
                strAddress = "";
            }

            return strAddress;
        }


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
        #endregion
        public DataTable GetWarehouseData(string strProduct)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "GetWareHouseDtlByProductID");
            proc.AddVarcharPara("@QuotationID", 500, Convert.ToString(Session["OrderID"]));
            proc.AddVarcharPara("@ProductID", 500, Convert.ToString(strProduct));
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@branchId", 500, Convert.ToString(Session["userbranchID"]));
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            dt = proc.GetTable();
            return dt;
        }
        public void GetProductUOM(ref string Sales_UOM_Name, ref string Sales_UOM_Code, ref string Stk_UOM_Name, ref string Stk_UOM_Code, ref string Conversion_Multiplier, string ProductID)
        {
            DataTable Productdt = GetProductDetailsData(ProductID);
            if (Productdt != null && Productdt.Rows.Count > 0)
            {
                Sales_UOM_Name = Convert.ToString(Productdt.Rows[0]["Sales_UOM_Name"]);
                Sales_UOM_Code = Convert.ToString(Productdt.Rows[0]["Sales_UOM_Code"]);
                Stk_UOM_Name = Convert.ToString(Productdt.Rows[0]["Stk_UOM_Name"]);
                Stk_UOM_Code = Convert.ToString(Productdt.Rows[0]["Stk_UOM_Code"]);
                Conversion_Multiplier = Convert.ToString(Productdt.Rows[0]["Conversion_Multiplier"]);
            }
        }


        #endregion


        #region Product Details
        public DataTable GetProductDetailsData(string ProductID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "ProductDetailsSearch");
            proc.AddVarcharPara("@ProductID", 500, ProductID);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetSerialata(string WarehouseID, string BatchID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "GetSerialByProductIDWarehouseBatch");
            proc.AddVarcharPara("@Order_Id", 500, Convert.ToString(Session["OrderID"]));
            proc.AddVarcharPara("@ProductID", 500, Convert.ToString(hdfProductID.Value));
            proc.AddVarcharPara("@BatchID", 500, BatchID);
            proc.AddVarcharPara("@WarehouseID", 500, WarehouseID);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@branchId", 500, Convert.ToString(Session["userbranchID"]));
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetBatchData(string WarehouseID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "GetBatchByProductIDWarehouse");
            proc.AddVarcharPara("@Order_Id", 500, Convert.ToString(Session["QuotationID"]));
            proc.AddVarcharPara("@ProductID", 500, Convert.ToString(hdfProductID.Value));
            proc.AddVarcharPara("@WarehouseID", 500, WarehouseID);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@branchId", 500, Convert.ToString(Session["userbranchID"]));
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetWarehouseData()
        {
            MasterSettings masterBl = new MasterSettings();
            string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));


            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "GetWareHouseDtlByProductID");
            proc.AddVarcharPara("@Order_Id", 500, Convert.ToString(Session["QuotationID"]));
            proc.AddVarcharPara("@ProductID", 500, Convert.ToString(hdfProductID.Value));
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@branchId", 500, Convert.ToString(Session["userbranchID"]));
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            proc.AddVarcharPara("@Multiwarehouse", 500, Convert.ToString(multiwarehouse));
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetQuotationAddressDetails(string Quotation_Id)
        {
            DataTable dts = new DataTable();
            ProcedureExecute pro = new ProcedureExecute("prc_SalesOrder_Details");
            pro.AddVarcharPara("@Action", 500, "GetQuotationAddress");
            pro.AddVarcharPara("@Quotation_Id", 500, Quotation_Id);
            dts = pro.GetTable();
            return dts;

        }

        #endregion


        #region Unique Code Generated Section Start

        public string checkNMakeJVCode(string manual_str, int sel_schema_Id)
        {

            //  oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
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

                    sqlQuery = "SELECT max(tjv.Order_Number) FROM tbl_trans_SalesOrder tjv WHERE dbo.RegexMatch('";
                    if (prefLen > 0)
                        sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                    else if (scheme_type == 2)
                        sqlQuery += "^";
                    sqlQuery += "[0-9]{" + paddCounter + "}";
                    if (sufxLen > 0)
                        sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                    //sqlQuery += "?$', LTRIM(RTRIM(tjv.Order_Number))) = 1";
                    sqlQuery += "?$', LTRIM(RTRIM(tjv.Order_Number))) = 1 and Order_Number like '" + prefCompCode + "%'";
                    if (scheme_type == 2)
                        sqlQuery += " AND CONVERT(DATE, CreatedDate) = CONVERT(DATE, GETDATE())";
                    dtC = oDBEngine.GetDataTable(sqlQuery);

                    if (dtC.Rows[0][0].ToString() == "")
                    {
                        sqlQuery = "SELECT max(tjv.Order_Number) FROM tbl_trans_SalesOrder tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.Order_Number))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.Order_Number))) = 1 and Order_Number like '" + prefCompCode + "%' and Order_Number like '%" + sufxCompCode + "'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, CreatedDate) = CONVERT(DATE, GETDATE())";
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
                            UniqueQuotation = prefCompCode + paddedStr + sufxCompCode;
                            return "ok";
                        }
                    }
                    else
                    {
                        UniqueQuotation = startNo.PadLeft(paddCounter, '0');
                        UniqueQuotation = prefCompCode + paddedStr + sufxCompCode;
                        return "ok";
                    }
                }
                else
                {
                    sqlQuery = "SELECT Order_Number FROM tbl_trans_SalesOrder WHERE Order_Number LIKE '" + manual_str.Trim() + "'";
                    dtC = oDBEngine.GetDataTable(sqlQuery);
                    // duplicate manual entry check
                    if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
                    {
                        return "duplicate";
                    }

                    UniqueQuotation = manual_str.Trim();
                    return "ok";
                }
            }
            else
            {
                return "noid";
            }
        }

        #endregion Unique Code Generated Section End



        #region Debu
        public DataSet GetSalesOrderTaxData()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "ProductTaxDetails");
            proc.AddVarcharPara("@Order_Id", 500, Convert.ToString(Session["OrderID"]));
            ds = proc.GetDataSet();
            return ds;
        }
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




        protected void taxUpdatePanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

            string performpara = e.Parameter;
            if (performpara.Split('~')[0] == "DelProdbySl")
            {
                DataTable MainTaxDataTable = (DataTable)Session["SalesOrderFinalTaxRecord" + Convert.ToString(uniqueId.Value)];

                DataRow[] deletedRow = MainTaxDataTable.Select("SlNo=" + performpara.Split('~')[1]);
                if (deletedRow.Length > 0)
                {
                    foreach (DataRow dr in deletedRow)
                    {
                        MainTaxDataTable.Rows.Remove(dr);
                    }

                }

                Session["SalesOrderFinalTaxRecord" + Convert.ToString(uniqueId.Value)] = MainTaxDataTable;
                //GetStock(Convert.ToString(performpara.Split('~')[2]));
                //DeleteWarehouse(Convert.ToString(performpara.Split('~')[1]));
                DataTable taxDetails = (DataTable)Session["STaxDetails"];
                if (taxDetails != null)
                {
                    foreach (DataRow dr in taxDetails.Rows)
                    {
                        dr["Amount"] = "0.00";
                    }
                    Session["STaxDetails"] = taxDetails;
                }

            }
            else if (performpara.Split('~')[0] == "DeleteAllTax")
            {
                CreateDataTaxTable();

                DataTable taxDetails = (DataTable)Session["STaxDetails"];

                if (taxDetails != null)
                {
                    foreach (DataRow dr in taxDetails.Rows)
                    {
                        dr["Amount"] = "0.00";
                    }
                    Session["STaxDetails"] = taxDetails;
                }
            }
            else
            {
                DataTable MainTaxDataTable = (DataTable)Session["SalesOrderFinalTaxRecord" + Convert.ToString(uniqueId.Value)];

                DataRow[] deletedRow = MainTaxDataTable.Select("SlNo=" + performpara.Split('~')[1]);
                if (deletedRow.Length > 0)
                {
                    foreach (DataRow dr in deletedRow)
                    {
                        MainTaxDataTable.Rows.Remove(dr);
                    }

                }

                Session["SalesOrderFinalTaxRecord" + Convert.ToString(uniqueId.Value)] = MainTaxDataTable;
                DataTable taxDetails = (DataTable)Session["STaxDetails"];
                if (taxDetails != null)
                {
                    foreach (DataRow dr in taxDetails.Rows)
                    {
                        dr["Amount"] = "0.00";
                    }
                    Session["STaxDetails"] = taxDetails;
                }
            }
        }

        public double ReCalculateTaxAmount(string slno, double amount)
        {
            DataTable MainTaxDataTable = (DataTable)Session["SalesOrderFinalTaxRecord" + Convert.ToString(uniqueId.Value)];
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
            Session["SalesOrderFinalTaxRecord" + Convert.ToString(uniqueId.Value)] = MainTaxDataTable;

            return totalSum;

        }
        protected void grid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            string name = txtname.Text.ToString();
            string numberingScheme = string.Empty;
            //numberingScheme = hdnnumschema.Value;
            string IsDeleteFrom = Convert.ToString(hdfIsDelete.Value);
            numberingScheme = Convert.ToString(ddl_numberingScheme.SelectedValue);
            string[] SchemeList = numberingScheme.Split(new string[] { "~" }, StringSplitOptions.None);
            string schemaid = SchemeList[0];
            string docNo = txt_SlOrderNo.Text.ToString();
            string Branch = ddl_Branch.SelectedValue.ToString();

            string Projectcode = "";
            if (hdnhidejobno.Value.ToString() == "0") { 
            if (lookup_Project.Text.ToString() != "")
            {
                Projectcode = lookup_Project.Value.ToString();

            }
            else
            {
                validjob.Visible = true;
                return;
            }
            }
            int mode = Convert.ToInt32(ddl_mode.Value);
            string servicename = txtservicename.Text.ToString();
            DateTime date = Convert.ToDateTime(dt_date.Value);
            DataTable Paymentreq = new DataTable();
            DataTable dtTemp = new DataTable();
            if (Session["PayReqDetails"] != null)
            {
                DataTable dt = new DataTable();
                dtTemp = (DataTable)Session["PayReqDetails"];
                dt = dtTemp.Copy();
                foreach (DataRow row in dt.Rows)
                {
                    DataColumnCollection dtC = dt.Columns;

                    if (dtC.Contains("NumberingSchema_Id"))
                    { dt.Columns.Remove("NumberingSchema_Id"); }
                    //if (dtC.Contains("SrlNo"))
                    //{ dt.Columns.Remove("SrlNo"); }
                    if (dtC.Contains("Name"))
                    { dt.Columns.Remove("Name"); }
                    //if (dtC.Contains("Paymentrequisition_Number"))
                    //{ dt.Columns.Remove("Paymentrequisition_Number"); }
                    if (dtC.Contains("Date"))
                    { dt.Columns.Remove("Date"); }
                    if (dtC.Contains("Job_No"))
                    { dt.Columns.Remove("Job_No"); }

                    if (dtC.Contains("Branch_Id"))
                    { dt.Columns.Remove("Branch_Id"); }
                    if (dtC.Contains("Service_Name"))
                    { dt.Columns.Remove("Service_Name"); }
                    if (dtC.Contains("Mode"))
                    { dt.Columns.Remove("Mode"); }
                    if (dtC.Contains("PaymentreqDetails_Id"))
                    { dt.Columns.Remove("PaymentreqDetails_Id"); }
                    if (dtC.Contains("Paymentreqhead_Id"))
                    { dt.Columns.Remove("Paymentreqhead_Id"); }
                    //if (dtC.Contains("Job_No"))
                    //{ dt.Columns.Remove("Job_No"); }
                    if (Paymentreq.Columns.Contains("Paymentrequisition_Number"))
                    {
                        Paymentreq.Columns.Remove("Paymentrequisition_Number");
                    }
                    break;
                }
                Paymentreq = dt;
            }
            else
            {
                Paymentreq.Columns.Add("SrlNo", typeof(Int64));
                //Paymentreq.Columns.Add("Paymentreqhead_Id", typeof(Int64));
                Paymentreq.Columns.Add("Particulars", typeof(string));
                Paymentreq.Columns.Add("Amount", typeof(decimal));
                Paymentreq.Columns.Add("Remarks", typeof(string));
            }
            int InitVal = Paymentreq.Columns.Count + 1;
            foreach (var args in e.InsertValues)
            {
                string Particulars = Convert.ToString(args.NewValues["Particulars"]);
                decimal Amount = Convert.ToDecimal(args.NewValues["Amount"]);
                string Remarks = Convert.ToString(args.NewValues["Remarks"]);
                // DataTable Quotationdt = (DataTable)Session["PayReqDetails"];
                if (Particulars == "" || Particulars == null)
                {
                    //grid.JSProperties["cpProductNotExists"] = "Please enter Particulars";
                    //return;
                }
                if (Amount == 0 || Amount == null)
                {
                    //grid.JSProperties["cpProductNotExists"] = "Please enter First";
                    //return;
                }




                if ((Particulars != "" && Particulars != null) && (Amount != 0 && Amount != null))
                {
                    if (Paymentreq.Columns.Contains("Paymentrequisition_Number"))
                    {
                        Paymentreq.Columns.Remove("Paymentrequisition_Number");
                    }
                    Paymentreq.Rows.Add(InitVal, Particulars, Amount, Remarks);
                    InitVal = InitVal + 1;
                }

            }
            foreach (var args in e.UpdateValues)
            {
                // dtTemp = (DataTable)Session["PayReqDetails"];

                string newsrlno = Convert.ToString(args.NewValues["SrlNo"]);
                string newParticulars = Convert.ToString(args.NewValues["Particulars"]);
                decimal newAmount = Convert.ToDecimal(args.NewValues["Amount"]);
                string Remarks = Convert.ToString(args.NewValues["Remarks"]);
                //string OrderID = Convert.ToString(args.Keys["OrderID"]);
                if ((newParticulars != "" && newParticulars != null) && (newAmount != 0 && newAmount != null))
                {
                    bool Isexists = false;
                    foreach (DataRow drr in Paymentreq.Rows)
                    {
                        string oldParticulars = Convert.ToString(drr["Particulars"]);
                        decimal OldAmount = Convert.ToDecimal(drr["Amount"]);
                        string oldsrnl = drr["SrlNo"].ToString();
                        if (oldsrnl == newsrlno)
                        {
                            Isexists = true;
                            drr["Particulars"] = newParticulars;
                            drr["Remarks"] = Remarks;
                            drr["Amount"] = newAmount;
                            break;
                        }
                    }

                    if (Isexists == false)
                    {
                        Paymentreq.Rows.Add(InitVal, newParticulars, newAmount, Remarks);
                    }
                    Paymentreq.AcceptChanges();
                }
            }
            foreach (var args in e.DeleteValues)
            {
                //hdnflag.Value = "1";
                string newsrlno = Convert.ToString(args.Keys["SrlNo"]);

                for (int i = Paymentreq.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = Paymentreq.Rows[i];
                    string oldsrnl = dr["SrlNo"].ToString();
                    //string delQuotationID = Convert.ToString(dr["OrderID"]);
                    if (oldsrnl == newsrlno)
                        dr.Delete();
                }
                //if (newsrlno.Contains("~") != true)
                //{
                //    Paymentreq.Rows.Add("0", "0", "0", "D");
                //}
                //for (int i = Paymentreq.Rows.Count - 1; i >= 0; i--)
                //{
                //    if (Paymentreq.Rows[i][1] == DBNull.Value)
                //    {
                //        Paymentreq.Rows[i].Delete();
                //    }
                //}
                Paymentreq.AcceptChanges();
            }
            if (IsDeleteFrom == "D" || IsDeleteFrom == "I")
            {
                int j = 1;
                foreach (DataRow dr in Paymentreq.Rows)
                {
                    string Remarks = Convert.ToString(dr["Remarks"]);
                    dr["SrlNo"] = j.ToString();

                    //  if (Remarks != "D")
                    //  {
                    //if (Status == "I" && IsDeleteFrom == "D")
                    //{
                    //    string strID = Convert.ToString("Q~" + j);
                    //    dr["OrderID"] = strID;
                    //}
                    j++;
                    // }


                }
                Paymentreq.AcceptChanges();

            }

            Paymentreq.AcceptChanges();
            Session["PayReqDetails"] = Paymentreq;
            if (hdnflag.Value == "1")
            {
                ModifyPaymentrequisition(name, schemaid, docNo, Branch, Projectcode, Paymentreq, mode, date, servicename);
            }

            grid.DataBind();
        }

        public int ModifyPaymentrequisition(string name, string schemaid, string docNo, string Branch, string Projectcode, DataTable dtPaymentreq, int mode, DateTime date, string servicename)
        {
            try
            {
                if (dtPaymentreq.Rows.Count > 0)
                {
                    foreach (DataRow drr in dtPaymentreq.Rows)
                    {
                        if (((drr["Particulars"] == null) || (drr["Particulars"] == "")) && ((drr["Amount"] == null) || (drr["Amount"] == null)))
                        {
                            grid.JSProperties["cpReturnValue"] = 3;
                            return 3;
                        }

                    }
                    if (dtPaymentreq.Columns.Contains("Paymentrequisition_Number"))
                    {
                        dtPaymentreq.Columns.Remove("Paymentrequisition_Number");
                    }
                    DataSet dsInst = new DataSet();
                    //    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                    string Action = Convert.ToString(Session["ActionType"]);
                    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                    SqlCommand cmd = new SqlCommand("prc_PaymentRequision", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", Action);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@numberingScheme", schemaid);
                    cmd.Parameters.AddWithValue("@docNo", docNo);
                    cmd.Parameters.AddWithValue("@Branch", Branch);
                    cmd.Parameters.AddWithValue("@Projectcode", Projectcode);
                    cmd.Parameters.AddWithValue("@mode", mode);
                    cmd.Parameters.AddWithValue("@date", date);
                    cmd.Parameters.AddWithValue("@paymentrequisitiondetails", dtPaymentreq);
                    cmd.Parameters.AddWithValue("@servicename", servicename);
                    cmd.Parameters.AddWithValue("@createdby", Convert.ToString(Session["userid"]));
                    cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
                    cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@GeneratedDocNumber", SqlDbType.VarChar, 50);
                    cmd.Parameters["@GeneratedDocNumber"].Direction = ParameterDirection.Output;



                    cmd.CommandTimeout = 0;
                    SqlDataAdapter Adap = new SqlDataAdapter();
                    Adap.SelectCommand = cmd;
                    Adap.Fill(dsInst);
                    int ReturnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());
                    string docno = Convert.ToString(cmd.Parameters["@GeneratedDocNumber"].Value.ToString());

                    grid.JSProperties["cpdocno"] = docno;
                    grid.JSProperties["cpReturnValue"] = ReturnValue;
                    return ReturnValue;
                }
                else
                {
                    grid.JSProperties["cpReturnValue"] = 3;
                    return 3;
                }
            }
            catch (Exception ex)
            {
                grid.JSProperties["cpReturnValue"] = ex;
                return 0;
            }
        }
        public void CreateDataTaxTable()
        {
            DataTable TaxRecord = new DataTable();

            TaxRecord.Columns.Add("SlNo", typeof(System.Int32));
            TaxRecord.Columns.Add("TaxCode", typeof(System.String));
            TaxRecord.Columns.Add("AltTaxCode", typeof(System.String));
            TaxRecord.Columns.Add("Percentage", typeof(System.Decimal));
            TaxRecord.Columns.Add("Amount", typeof(System.Decimal));
            Session["SalesOrderFinalTaxRecord" + Convert.ToString(uniqueId.Value)] = TaxRecord;
        }

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
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "ProductEditedTaxDetails");
            proc.AddVarcharPara("@Order_Id", 10, Convert.ToString(Session["OrderID"]));
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
                taxObj.Amount = Math.Round(taxObj.Amount, 2);
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

        public class Quotation
        {
            public string SrlNo { get; set; }
            public string QuotationID { get; set; }
            public string ProductID { get; set; }
            public string Description { get; set; }
            public string Quantity { get; set; }
            public string UOM { get; set; }
            public string Warehouse { get; set; }
            public string StockQuantity { get; set; }
            public string StockUOM { get; set; }
            public string SalePrice { get; set; }
            public string Discount { get; set; }
            public string Amount { get; set; }
            public string TaxAmount { get; set; }
            public string TotalAmount { get; set; }
            public string ProductName { get; set; }
            public string Quotation_Num { get; set; }
            public Int64 Quotation_No { get; set; }
        }

        public class QuotationForBasket
        {
            public string SrlNo { get; set; }
            public string OrderID { get; set; }
            public string ProductID { get; set; }
            public string Description { get; set; }
            public string Quantity { get; set; }
            public string UOM { get; set; }
            public string Warehouse { get; set; }
            public string StockQuantity { get; set; }
            public string StockUOM { get; set; }
            public string SalePrice { get; set; }
            public string Discount { get; set; }
            public string Amount { get; set; }
            public string TaxAmount { get; set; }
            public string TotalAmount { get; set; }
            public string Status { get; set; }
            public string ProductName { get; set; }
            public string Quotation_Num { get; set; }
            public Int64 Quotation_No { get; set; }
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
        class taxCode
        {
            public string Taxes_ID { get; set; }
            public string Taxes_Name { get; set; }
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

        protected void lookup_CustomerControlPanelMain_Callback1(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string CustomerId = e.Parameter;
            Session["CustomerDetail"] = null;
        }


        #region BindCustomerOnDemand


        #endregion

        #region BindSalesManAgentOnDemand



        #endregion

        protected void ComponentPanel_Callback(object sender, CallbackEventArgsBase e)
        {
        }
        public DataSet GetOrderEditData()
        {
            DataSet dt = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "OrderEditDetails");
            proc.AddVarcharPara("@Order_Id", 500, Convert.ToString(Session["OrderID"]));
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@branchId", 500, Convert.ToString(Session["userbranchID"]));
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            dt = proc.GetDataSet();
            return dt;
        }
        public DataTable GetProjectCode(string Proj_Code)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "GetProjectCode");
            proc.AddVarcharPara("@Proj_Code", 200, Proj_Code);
            dt = proc.GetTable();
            return dt;
        }


        public DataTable GetProjectEditData(string SalesOrderId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "ProjectEditdata");
            proc.AddIntegerPara("@SalesOrder_Id", Convert.ToInt32(SalesOrderId));
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetCustomerOnIndustry(int SlsId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GetCustomerOnIndustryMap");
            proc.AddBigIntegerPara("@sls_Id", SlsId);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetProductsOnIndustry(int SlsId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GetProductIdsOnIndustryMap");
            proc.AddBigIntegerPara("@sls_Id", SlsId);
            dt = proc.GetTable();
            return dt;
        }

        [WebMethod]
        public static bool CheckUniqueCode(string OrderNo)
        {
            bool flag = false;
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            try
            {
                MShortNameCheckingBL objShortNameChecking = new MShortNameCheckingBL();
                flag = objShortNameChecking.CheckUnique(OrderNo, "0", "SalesOrder");
            }
            catch (Exception ex)
            {

            }
            finally
            {
            }
            return flag;
        }








        protected void ComponentIsInventory_Callback(object sender, CallbackEventArgsBase e)
        {
            Session["IsInvenory"] = null;
            string type = e.Parameter.Split('~')[0];
            string IsInventoryVal = e.Parameter.Split('~')[1];
            if (type == "BindSession")
            {
                if (IsInventoryVal == "1")
                {
                    Session["IsInvenory"] = "1";
                }
                else
                {
                    Session["IsInvenory"] = "0";
                }

            }
        }






        //Subhabrata

        public DataTable GetBillingAddress()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 100, "SalesOrderBillingAddress");
            proc.AddVarcharPara("@Order_Id", 20, Convert.ToString(Session["OrderID"]));
            dt = proc.GetTable();
            return dt;
        }

        #region Warehouse Details



        public DataTable GetOrderWarehouseData()
        {
            try
            {
                MasterSettings masterBl = new MasterSettings();
                string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));

                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
                proc.AddVarcharPara("@Action", 500, "OrderWarehouse");
                proc.AddVarcharPara("@Order_Id", 500, Convert.ToString(Session["OrderID"]));
                dt = proc.GetTable();

                string strNewVal = "", strOldVal = "";
                DataTable tempdt = dt.Copy();
                foreach (DataRow drr in tempdt.Rows)
                {
                    strNewVal = Convert.ToString(drr["OrderWarehouse_Id"]);

                    if (strNewVal == strOldVal)
                    {
                        drr["WarehouseName"] = "";
                        drr["Quantity"] = "";
                        drr["BatchNo"] = "";
                        drr["SalesUOMName"] = "";
                        drr["SalesQuantity"] = "";
                        drr["StkUOMName"] = "";
                        drr["StkQuantity"] = "";
                        drr["ConversionMultiplier"] = "";
                        drr["AvailableQty"] = "";
                        drr["BalancrStk"] = "";
                        drr["MfgDate"] = "";
                        drr["ExpiryDate"] = "";
                    }

                    strOldVal = strNewVal;
                }

                Session["LoopSalesOrderWarehouse"] = Convert.ToString(Convert.ToInt32(strNewVal) + 1);
                tempdt.Columns.Remove("OrderWarehouse_Id");
                return tempdt;
            }
            catch
            {
                return null;
            }
        }



        [WebMethod]
        public static string GetSerialId(string id, string wareHouseStr, string BatchID, string ProducttId)
        {
            CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();

            string[] Serials = id.Split(';');
            string Serial = Serials[0].TrimStart(';');
            string ispermission = string.Empty;
            string LastSerial = string.Empty;
            for (int i = 0; i < Serials.Length; i++)
            {
                LastSerial = Serials[Serials.Length - 1].TrimStart(';');

            }
            //string SerialLast=
            DataTable dt = new DataTable();
            //ispermission = objCRMSalesOrderDtlBL.GetInvoiceCustomerId(Convert.ToInt32(KeyVal));
            if (!string.IsNullOrEmpty(LastSerial))
            {
                dt = objCRMSalesOrderDtlBL.GetSerialataOnFIFOBasis(wareHouseStr, BatchID, Serial, ProducttId, id, LastSerial);
            }
            else
            {
                dt = objCRMSalesOrderDtlBL.GetSerialataOnFIFOBasis(wareHouseStr, BatchID, Serial, ProducttId, id, Serial);
            }


            ispermission = Convert.ToString(dt.Rows[0].Field<Int32>("ResturnVal"));
            return Convert.ToString(ispermission);

        }


        protected void CmbSerial_Callback(object sender, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "BindSerial")
            {
                string WarehouseID = Convert.ToString(e.Parameter.Split('~')[1]);
                string BatchID = Convert.ToString(e.Parameter.Split('~')[2]);
                DataTable dt = GetSerialata(WarehouseID, BatchID);

                if (Session["SalesWarehouseData"] != null)
                {
                    DataTable Warehousedt = (DataTable)Session["SalesWarehouseData"];
                    DataTable tempdt = Warehousedt.DefaultView.ToTable(false, "SerialID");

                    foreach (DataRow dr in dt.Rows)
                    {
                        string SerialID = Convert.ToString(dr["SerialID"]);
                        DataRow[] rows = tempdt.Select("SerialID = '" + SerialID + "' AND SerialID<>'0'");

                        if (rows != null && rows.Length > 0)
                        {
                            dr.Delete();
                        }

                        //foreach (DataRow drr in tempdt.Rows)
                        //{
                        //    string oldSerialID = Convert.ToString(drr["SerialID"]);
                        //    if (newSerialID == oldSerialID)
                        //    {
                        //        dr.Delete();
                        //    }
                        //}

                    }
                    dt.AcceptChanges();

                }

                ASPxListBox lb = sender as ASPxListBox;
                lb.DataSource = dt;
                lb.ValueField = "SerialID";
                lb.TextField = "SerialName";
                lb.ValueType = typeof(string);
                lb.DataBind();
            }
            else if (WhichCall == "EditSerial")
            {
                string WarehouseID = Convert.ToString(e.Parameter.Split('~')[1]);
                string BatchID = Convert.ToString(e.Parameter.Split('~')[2]);
                string editSerialID = Convert.ToString(e.Parameter.Split('~')[3]);
                DataTable dt = GetSerialata(WarehouseID, BatchID);

                if (Session["SalesWarehouseData"] != null)
                {
                    DataTable Warehousedt = (DataTable)Session["SalesWarehouseData"];
                    DataTable tempdt = Warehousedt.DefaultView.ToTable(false, "SerialID");

                    foreach (DataRow dr in dt.Rows)
                    {
                        string SerialID = Convert.ToString(dr["SerialID"]);
                        DataRow[] rows = tempdt.Select("SerialID = '" + SerialID + "' AND SerialID not in ('0','" + editSerialID + "')");

                        if (rows != null && rows.Length > 0)
                        {
                            dr.Delete();
                        }

                        //foreach (DataRow drr in tempdt.Rows)
                        //{
                        //    string oldSerialID = Convert.ToString(drr["SerialID"]);
                        //    if (newSerialID == oldSerialID)
                        //    {
                        //        dr.Delete();
                        //    }
                        //}

                    }
                    dt.AcceptChanges();

                }

                ASPxListBox lb = sender as ASPxListBox;
                lb.DataSource = dt;
                lb.ValueField = "SerialID";
                lb.TextField = "SerialName";
                lb.ValueType = typeof(string);
                lb.DataBind();
            }
        }
        protected void listBox_Init(object sender, EventArgs e)
        {
            ASPxListBox lb = sender as ASPxListBox;
            DataTable dt = GetSerialata("", "");

            lb.DataSource = dt;
            lb.ValueField = "SerialID";
            lb.TextField = "SerialName";
            lb.ValueType = typeof(string);
            lb.DataBind();
        }

        public void GetTotalStock(ref string Trans_Stock, string WarehouseID)
        {
            string ProductID = Convert.ToString(hdfProductID.Value);

            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "GetBatchDetails");
            proc.AddVarcharPara("@ProductID", 100, Convert.ToString(ProductID));
            proc.AddVarcharPara("@WarehouseID", 100, Convert.ToString(WarehouseID));
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@branchId", 500, Convert.ToString(Session["userbranchID"]));
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            DataTable dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                Trans_Stock = Convert.ToString(dt.Rows[0]["Trans_Stock"]);
            }
        }

        public void GetBatchDetails(ref string MfgDate, ref string ExpiryDate, string BatchID)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "GetBatchDetails");
            proc.AddVarcharPara("@BatchID", 100, Convert.ToString(BatchID));
            DataTable Batchdt = proc.GetTable();

            if (Batchdt != null && Batchdt.Rows.Count > 0)
            {
                MfgDate = Convert.ToString(Batchdt.Rows[0]["MfgDate"]);
                ExpiryDate = Convert.ToString(Batchdt.Rows[0]["ExpiryDate"]);
            }
        }
        public void GetProductType(ref string Type)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "GetSchemeType");
            proc.AddVarcharPara("@ProductID", 100, Convert.ToString(hdfProductID.Value));
            DataTable dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                Type = Convert.ToString(dt.Rows[0]["Type"]);
            }
        }



        [WebMethod]
        public static string getSchemeType(string Products_ID)
        {
            string Type = "";
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "GetSchemeType");
            proc.AddVarcharPara("@ProductID", 100, Convert.ToString(Products_ID));
            DataTable dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                Type = Convert.ToString(dt.Rows[0]["Type"]);
            }

            return Convert.ToString(Type);
        }

        public void DeleteUnsaveWarehouse(string SrlNo)
        {
            DataTable Warehousedt = new DataTable();
            if (Session["SalesWarehouseData"] != null)
            {
                Warehousedt = (DataTable)Session["SalesWarehouseData"];

                var rows = Warehousedt.Select("Product_SrlNo ='" + SrlNo + "' AND Status='D'");
                foreach (var row in rows)
                {
                    row.Delete();
                }
                Warehousedt.AcceptChanges();

                Session["SalesWarehouseData"] = Warehousedt;
            }
        }

        public DataTable DeleteWarehouseBySrl(string strKey)
        {
            string strLoopID = "", strPreLoopID = "";

            DataTable Warehousedt = new DataTable();
            if (Session["SalesWarehouseData"] != null)
            {
                Warehousedt = (DataTable)Session["SalesWarehouseData"];
            }

            DataRow[] result = Warehousedt.Select("SrlNo ='" + strKey + "'");
            foreach (DataRow row in result)
            {
                strLoopID = row["LoopID"].ToString();
            }

            if (Warehousedt != null && Warehousedt.Rows.Count > 0)
            {
                int count = 0;
                bool IsFirst = false, IsAssign = false;
                string WarehouseName = "", Quantity = "", BatchNo = "", SalesUOMName = "", SalesQuantity = "", StkUOMName = "", StkQuantity = "", ConversionMultiplier = "", AvailableQty = "", BalancrStk = "", MfgDate = "", ExpiryDate = "";


                for (int i = 0; i < Warehousedt.Rows.Count; i++)
                {
                    DataRow dr = Warehousedt.Rows[i];
                    string delSrlID = Convert.ToString(dr["SrlNo"]);
                    string delLoopID = Convert.ToString(dr["LoopID"]);

                    if (strPreLoopID != delLoopID)
                    {
                        count = 0;
                    }

                    if ((delLoopID == strLoopID) && (strKey == delSrlID) && count == 0)
                    {
                        IsFirst = true;

                        WarehouseName = Convert.ToString(dr["WarehouseName"]);
                        Quantity = Convert.ToString(dr["Quantity"]);
                        BatchNo = Convert.ToString(dr["BatchNo"]);
                        SalesUOMName = Convert.ToString(dr["SalesUOMName"]);
                        SalesQuantity = Convert.ToString(dr["SalesQuantity"]);
                        StkUOMName = Convert.ToString(dr["StkUOMName"]);
                        StkQuantity = Convert.ToString(dr["StkQuantity"]);
                        ConversionMultiplier = Convert.ToString(dr["ConversionMultiplier"]);
                        AvailableQty = Convert.ToString(dr["AvailableQty"]);
                        BalancrStk = Convert.ToString(dr["BalancrStk"]);
                        MfgDate = Convert.ToString(dr["MfgDate"]);
                        ExpiryDate = Convert.ToString(dr["ExpiryDate"]);

                        //dr.Delete();
                    }
                    else
                    {
                        if (delLoopID == strLoopID)
                        {
                            if (strKey == delSrlID)
                            {
                                //dr.Delete();
                            }
                            else
                            {
                                int S_Quantity = Convert.ToInt32(dr["TotalQuantity"]);
                                dr["Quantity"] = S_Quantity - 1;
                                dr["TotalQuantity"] = S_Quantity - 1;

                                if (IsFirst == true && IsAssign == false)
                                {
                                    IsAssign = true;

                                    dr["WarehouseName"] = WarehouseName;
                                    dr["BatchNo"] = BatchNo;
                                    dr["SalesUOMName"] = SalesUOMName;
                                    dr["SalesQuantity"] = (S_Quantity - 1) + " " + SalesUOMName;//SalesQuantity;
                                    dr["StkUOMName"] = StkUOMName;
                                    dr["StkQuantity"] = StkQuantity;
                                    dr["ConversionMultiplier"] = ConversionMultiplier;
                                    dr["AvailableQty"] = AvailableQty;
                                    dr["BalancrStk"] = BalancrStk;
                                    dr["MfgDate"] = MfgDate;
                                    dr["ExpiryDate"] = ExpiryDate;
                                }
                                else
                                {
                                    if (IsAssign == false)
                                    {
                                        IsAssign = true;
                                        SalesUOMName = Convert.ToString(dr["SalesUOMName"]);
                                        dr["SalesQuantity"] = (S_Quantity - 1) + " " + SalesUOMName;//SalesQuantity;
                                    }
                                }
                            }
                        }
                    }

                    strPreLoopID = delLoopID;
                    count++;
                }
                Warehousedt.AcceptChanges();


                for (int i = 0; i < Warehousedt.Rows.Count; i++)
                {
                    DataRow dr = Warehousedt.Rows[i];
                    string delSrlID = Convert.ToString(dr["SrlNo"]);
                    if (strKey == delSrlID)
                    {
                        dr.Delete();
                    }
                }
                Warehousedt.AcceptChanges();
            }

            return Warehousedt;
        }
        public void UpdateWarehouse(string oldSrlNo, string newSrlNo)
        {
            DataTable Warehousedt = new DataTable();
            if (Session["SalesWarehouseData"] != null)
            {
                Warehousedt = (DataTable)Session["SalesWarehouseData"];

                for (int i = 0; i < Warehousedt.Rows.Count; i++)
                {
                    DataRow dr = Warehousedt.Rows[i];
                    string Product_SrlNo = Convert.ToString(dr["Product_SrlNo"]);
                    if (oldSrlNo == Product_SrlNo)
                    {
                        dr["Product_SrlNo"] = newSrlNo;
                    }
                }
                Warehousedt.AcceptChanges();

                Session["SalesWarehouseData"] = Warehousedt;
            }
        }

        public void DeleteWarehouse(string SrlNo)
        {
            DataTable Warehousedt = new DataTable();
            if (Session["WarehouseData"] != null)
            {
                Warehousedt = (DataTable)Session["WarehouseData"];

                var rows = Warehousedt.Select(string.Format("Product_SrlNo ='{0}'", SrlNo));
                foreach (var row in rows)
                {
                    row.Delete();
                }
                Warehousedt.AcceptChanges();

                Session["WarehouseData"] = Warehousedt;
            }
        }
        #endregion



        #region sales Order Mail
        public int Sendmail_SalesOrder(string Output)
        {

            int stat = 0;

            Employee_BL objemployeebal = new Employee_BL();
            DataTable dt2 = new DataTable();
            dt2 = objemployeebal.GetSystemsettingmail("Show Email in SO");
            if (Convert.ToString(dt2.Rows[0]["Variable_Value"]) == "Yes")
            {


                ExceptionLogging mailobj = new ExceptionLogging();
                EmailSenderHelperEL emailSenderSettings = new EmailSenderHelperEL();
                DataTable dt_EmailConfig = new DataTable();
                DataTable dt_EmailConfigpurchase = new DataTable();

                DataTable dt_Emailbodysubject = new DataTable();
                SalesOrderEmailTags fetchModel = new SalesOrderEmailTags();
                string Subject = "";
                string Body = "";
                string emailTo = "";

                int MailStatus = 0;


                //var customerid = lookup_Customer.GridView.GetRowValues(lookup_Customer.GridView.FocusedRowIndex, lookup_Customer.KeyFieldName).ToString();
                //var customerid = Convert.ToString(CustomerComboBox.Value);subhabrata on 02-01-2018
                var customerid = Convert.ToString(hdnCustomerId.Value);


                //   var customerid = cmbContactPerson.Value.ToString();

                dt_EmailConfig = objemployeebal.Getemailids(customerid);
                // string FilePath = ConfigurationManager.AppSettings["ReportPOpdf"].ToString() + "PO-Default-" + Output + ".pdf";
                // string FilePath = Server.MapPath("~/Reports/RepxReportDesign/SalesOrder/DocDesign/PDFFiles/" + "SO-Default-" + Output + ".pdf");
                string FilePath = string.Empty;
                string path = System.Web.HttpContext.Current.Server.MapPath("~");
                string path1 = string.Empty;
                string DesignPath = "";
                string fullpath = Server.MapPath("~");

                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    FilePath = Server.MapPath("~/Reports/Reports/RepxReportDesign/SalesOrder/DocDesign/PDFFiles/" + "SO-Default-" + Output + ".pdf");
                }
                else
                {
                    FilePath = Server.MapPath("~/Reports/RepxReportDesign/SalesOrder/DocDesign/PDFFiles/" + "SO-Default-" + Output + ".pdf");
                }
                FilePath = FilePath.Replace("ERP.UI\\", "");





                string FileName = FilePath;
                if (dt_EmailConfig.Rows.Count > 0)
                {
                    emailTo = Convert.ToString(dt_EmailConfig.Rows[0]["eml_email"]);
                    //  emailTo = "sayan.dutta@indusnet.co.in";
                    dt_Emailbodysubject = objemployeebal.Getemailtemplates("16");  //for purchase order

                    if (dt_Emailbodysubject.Rows.Count > 0)
                    {
                        Body = Convert.ToString(dt_Emailbodysubject.Rows[0]["body"]);
                        Subject = Convert.ToString(dt_Emailbodysubject.Rows[0]["subjct"]);

                        dt_EmailConfigpurchase = objemployeebal.Getemailtagsforpurchase(Output, "SalesOrderEmailTags");  //For Purchase Order Get all Tags Value

                        if (dt_EmailConfigpurchase.Rows.Count > 0)
                        {

                            fetchModel = DbHelpers.ToModel<SalesOrderEmailTags>(dt_EmailConfigpurchase);

                            Body = Employee_BL.GetFormattedString<SalesOrderEmailTags>(fetchModel, Body);
                            Subject = Employee_BL.GetFormattedString<SalesOrderEmailTags>(fetchModel, Subject);


                        }

                        emailSenderSettings = mailobj.GetEmailSettingsforAllreport(emailTo, "", "", FilePath, Body, Subject);

                        if (emailSenderSettings.IsSuccess)
                        {
                            string Message = "";
                            // checkmailId = new SendEmailUL().CheckMailIdExistence(emailSenderSettings.ModelCast<EmailSenderEL>());
                            //if (checkmailId == true)
                            //{
                            EmailSenderEL obj2 = new EmailSenderEL();
                            stat = SendEmailUL.sendMailInHtmlFormat(emailSenderSettings.ModelCast<EmailSenderEL>(), out Message);
                            //}


                        }
                    }
                }



            }
            return stat;
        }
        #endregion

        protected void ShowGridCustOut_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            string CustomerId = Convert.ToString(hdnCustomerId.Value);
            if (Convert.ToString(hddnOutStandingBlock.Value) == "1")
            {
                dtPartyTotal = oDBEngine.GetDataTable("Select DOC_TYPE,BAL_AMOUNT from PARTYOUTSTANDINGDET_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND SLNO=999999999 AND DOC_TYPE='Gross Outstanding:' AND PARTYTYPE='C'");
                PartyTotalBalDesc = dtPartyTotal.Rows[0][0].ToString();
                PartyTotalBalAmt = dtPartyTotal.Rows[0][1].ToString();

            }
            string summaryTAG = (e.Item as ASPxSummaryItem).Tag;
            if (e.IsTotalSummary == true)
            {
                switch (summaryTAG)
                {
                    case "Item_BalAmt":
                        e.Text = PartyTotalBalAmt;
                        break;
                    case "Item_DocType":
                        e.Text = PartyTotalBalDesc;
                        break;
                }
            }

        }


        protected void ShowGridCustOut_HtmlFooterCellPrepared(object sender, ASPxGridViewTableFooterCellEventArgs e)
        {
            if (e.Column.Caption != "Doc. Type")
            {
                e.Cell.Style["text-align"] = "right";
            }

        }
        protected void ShowGridCustOut_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {

            if (Convert.ToString(e.CellValue) == "Party Outstanding:" || Convert.ToString(e.CellValue) == "Total:")
            {
                Session["chk_presenttotal"] = 1;
            }
            if (Convert.ToInt32(Session["chk_presenttotal"]) == 1)
            {
                e.Cell.Font.Bold = true;
                e.Cell.BackColor = System.Drawing.Color.DarkSeaGreen;
            }

            if (e.DataColumn.FieldName == "BAL_AMOUNT")
            {
                Session["chk_presenttotal"] = 0;
            }
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
        [WebMethod]
        public static string DeleteTaxForShipPartyChange(string UniqueVal)
        {
            DataTable dt = new DataTable();
            if (HttpContext.Current.Session["SalesOrderFinalTaxRecord" + Convert.ToString(UniqueVal)] != null)
            {
                dt = (DataTable)HttpContext.Current.Session["SalesOrderFinalTaxRecord" + Convert.ToString(UniqueVal)];
                dt.Rows.Clear();
                //HttpContext.Current.Session["FinalTaxRecord"]=null;
                HttpContext.Current.Session["SalesOrderFinalTaxRecord" + Convert.ToString(UniqueVal)] = dt;
            }


            return null;

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
        public class ContactPerson
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public string InternalID { get; set; }

        }
    }

        #endregion
}
