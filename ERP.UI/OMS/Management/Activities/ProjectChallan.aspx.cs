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
using ClsDropDownlistNameSpace;
using System.Collections;
using DevExpress.Web.Data;
using DataAccessLayer;
using System.ComponentModel;
using System.Linq;
using System.Globalization;
using System.Text.RegularExpressions;
using ERP.OMS.Management.Activities.UserControls;
using DevExpress.Xpo;
using ERP.OMS.Tax_Details.ClassFile;
using ERP.Models;


namespace ERP.OMS.Management.Activities
{
    public partial class ProjectChallan : System.Web.UI.Page
    {
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


        clsDropDownList oclsDropDownList = new clsDropDownList();
        BusinessLogicLayer.Contact oContactGeneralBL = new BusinessLogicLayer.Contact();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        BusinessLogicLayer.RemarkCategoryBL reCat = new BusinessLogicLayer.RemarkCategoryBL();
        BusinessLogicLayer.Others objBL = new BusinessLogicLayer.Others();
        static string ForJournalDate = null;
        ProjectActivitiesBL objProjectActivitiesBL = new ProjectActivitiesBL();
        GSTtaxDetails gstTaxDetails = new GSTtaxDetails();
        SalesInvoiceBL objSalesInvoiceBL = new SalesInvoiceBL();
        string UniqueQuotation = string.Empty;
        DataTable Remarks = new DataTable();
        public string pageAccess = "";
        string userbranch = "";
        #region Sandip Section For Approval Section Start
        ERPDocPendingApprovalBL objERPDocPendingApproval = new ERPDocPendingApprovalBL();
        #endregion Sandip Section For Approval Dtl Section End

        #region Sandip Section For Approval Section Start
        public void IsExistsDocumentInERPDocApproveStatus(string orderId)
        {
            string editable = "";
            string status = "";
            DataTable dt = new DataTable();
            int Id = Convert.ToInt32(orderId);
            dt = objERPDocPendingApproval.IsExistsDocumentInERPDocApproveStatus(Id, 4); // 2 for Sale Invoice
            if (dt.Rows.Count > 0)
            {
                editable = Convert.ToString(dt.Rows[0]["editable"]);
                if (editable == "0")
                {
                    lbl_quotestatusmsg.Visible = true;
                    status = Convert.ToString(dt.Rows[0]["Status"]);
                    if (status == "Approved")
                    {
                        lbl_quotestatusmsg.Text = "Document already Approved";
                    }
                    if (status == "Rejected")
                    {
                        lbl_quotestatusmsg.Text = "Document already Rejected";
                    }
                    btn_SaveRecords.Visible = false;
                    ASPxButton12.Visible = false;
                }
                else
                {
                    lbl_quotestatusmsg.Visible = false;
                    btn_SaveRecords.Visible = true;
                    ASPxButton12.Visible = true;
                }
            }
        }

        #endregion Sandip Section For Approval Dtl Section End
        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            #region Sandip Section For Approval Section Start
            if (Request.QueryString.AllKeys.Contains("status"))
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
            //CountrySelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            //StateSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            //SelectCity.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            //SelectArea.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            //SelectPin.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            sqltaxDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlCurrency.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            SqlCurrencyBind.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            //CustomerDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            //ProductDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //rdl_SaleInvoice.Items[0].Enabled = false;
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

            //Tanmoy Hierarchy
            string HierarchySelectInEntryModule = ComBL.GetSystemSettingsResult("Show_Hierarchy");
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
            //Tanmoy Hierarchy End

            if (!IsPostBack)
            {
                Session["psc_ProductDetails"] = null;
                //order Discount
                if (Request.QueryString.AllKeys.Contains("DlvTyeId"))
                {
                    grid.Columns[10].Caption = "Disc (%)";
                }
                else
                {
                    string strSQL = "Select 'Y' From Config_SystemSettings Where Variable_Name='IsSalesChallanDiscountPercentage' AND Variable_Value='Yes'";
                    DataTable dtSQL = oDBEngine.GetDataTable(strSQL);
                    if (dtSQL != null && dtSQL.Rows.Count > 0)
                    {
                        IsDiscountPercentage.Value = "Y";
                        grid.Columns[11].Caption = "Add/Less (%)";
                    }
                    else
                    {
                        grid.Columns[11].Caption = "Add/Less Amt";
                        IsDiscountPercentage.Value = "N";
                    }
                }
                //End
                MasterSettings objmaster = new MasterSettings();
                hdnConvertionOverideVisible.Value = objmaster.GetSettings("ConvertionOverideVisible");
                hdnShowUOMConversionInEntry.Value = objmaster.GetSettings("ShowUOMConversionInEntry");
            }

            if (HttpContext.Current.Session["userid"] == null)
            {

            }

            if (Session["userbranchHierarchy"] != null)
            {
                userbranch = Convert.ToString(Session["userbranchHierarchy"]);
            }
            if (string.IsNullOrEmpty(Request.QueryString["CustID"]))
            {
                crossBtnId.Visible = true;
            }

            if (Request.QueryString.AllKeys.Contains("Permission") && Request.QueryString.AllKeys.Contains("type"))
            {
                hddnPermissionString.Value = "1";
            }
            else if (!Request.QueryString.AllKeys.Contains("Permission") && Request.QueryString.AllKeys.Contains("type"))
            {
                hddnPermissionString.Value = "0";
            }

            LastCompany.Value = Convert.ToString(Session["LastCompany"]);
            LastFinancialYear.Value = Convert.ToString(Session["LastFinYear"]);
            DataTable CustDeliveryCount = null;
            if (!string.IsNullOrEmpty(Request.QueryString["DlvTyeId"]) && !Request.QueryString.AllKeys.Contains("CustID"))
            {
                CustDeliveryCount = oDBEngine.GetDataTable("select * from tbl_trans_SalesChallan hChal inner join tbl_trans_SalesChallanProducts dChal on hChal.Challan_Id =dChal.ChallanDetails_OrderId  and Doc_Type='SI' and dChal.ChallanDetails_OrderId=" + Convert.ToString(Request.QueryString["key"]));
                //View Mode only
                //DataTable CustDeliveryCount = oDBEngine.GetDataTable("select 1 from tbl_trans_SalesChallan hChal inner join tbl_trans_SalesChallanProducts dChal on hChal.Challan_Id =dChal.ChallanDetails_OrderId  and Doc_Type='SI' and dChal.Doc_Id=" + Convert.ToString(Request.QueryString["key"]));
                if (CustDeliveryCount != null)
                {
                    if (CustDeliveryCount.Rows.Count > 0)
                    {
                        ASPxButton12.ClientVisible = false;
                        btn_SaveRecords.ClientVisible = false;
                        ClientShowMsg.Visible = true;
                        ddl_Branch.Enabled = false;
                        dt_PLSales.ClientEnabled = false;
                    }
                }
            }
            if (!IsPostBack)
            {
                //CustomerComboBox.FilterMinLength = 4;//Subhabrata on 27-12-2017
                hddnIsODSDFirstTime.Value = "0";
                SetFinYearCurrentDate();
                //Debjyoti GST
                SetTaxJSONData();
                //End
                if (Request.QueryString.AllKeys.Contains("status"))
                {
                    crossBtnCustDeliveryListId.Visible = false;
                    crossBtnPendingSecondHand.Visible = false;
                    crossBtnPendingDeliveryListId.Visible = false;
                    crossBtnCustDeliveryListForSD.Visible = false;
                    crossBtnId.Visible = false;
                    //divcross.Visible = false;
                    btn_SaveRecords.Visible = false;
                    ApprovalCross.Visible = true;
                    ddl_Branch.Enabled = false;
                }
                else
                {
                    crossBtnCustDeliveryListId.Visible = true;
                    crossBtnPendingSecondHand.Visible = true;
                    crossBtnPendingDeliveryListId.Visible = true;
                    crossBtnCustDeliveryListForSD.Visible = true;
                    crossBtnId.Visible = true;
                    btn_SaveRecords.Visible = true;
                    ApprovalCross.Visible = false;
                    //ddl_Branch.Enabled = true;
                }
                GetAllDropDownDetailForSalesOrder(userbranch);
                Session["ChallanDetails"] = null;

                bindHierarchy();
                ddlHierarchy.Enabled = false;
            }


            //Subhabrata
            if (!string.IsNullOrEmpty(Request.QueryString["CustID"]) && hddnIsODSDFirstTime.Value == "0")
            {
                //Tax button hide
                Button4.Visible = false;
                //End

                hddnBillId.Value = Convert.ToString(Request.QueryString["key"]);
                BindProjectChallanDirect(Convert.ToString(Request.QueryString["key"]), Convert.ToString(Request.QueryString["CustID"]));

                hdnCustomerId.Value = Convert.ToString(Request.QueryString["CustID"]);
                DataTable dt_Date = GetInvoiceDateData();
                //DataTable dt_InvoiceHeader = GetInvoiceDateData();
                DataTable dt_InvoiceHeader = dt_Date.Copy();

                PopulateContactPersonOfCustomer(hdnCustomerId.Value);
                if (!string.IsNullOrEmpty(Convert.ToString(dt_InvoiceHeader.Rows[0].Field<Int32>("salesmancnt_id"))))
                {
                    //on 28-12-2017
                    //ddl_SalesAgent.SelectedValue = Convert.ToString(dt_InvoiceHeader.Rows[0].Field<Int32>("salesmancnt_id"));
                    //ddl_SalesAgent.Enabled = false;

                    hdnSalesManAgentId.Value = Convert.ToString(dt_InvoiceHeader.Rows[0].Field<Int32>("salesmancnt_id"));
                    txtSalesManAgent.Text = Convert.ToString(dt_InvoiceHeader.Rows[0]["SalesMan"]);
                    txtSalesManAgent.ClientEnabled = false;
                }


                if (!string.IsNullOrEmpty(Convert.ToString(dt_InvoiceHeader.Rows[0].Field<Int64>("Contact_Person_Id"))))
                {
                    cmbContactPerson.SelectedItem = cmbContactPerson.Items.FindByValue(Convert.ToString(dt_InvoiceHeader.Rows[0].Field<Int64>("Contact_Person_Id")));
                }

                string Quotation_Date = Convert.ToString(dt_Date.Rows[0].Field<string>("Invoice_DocDt"));
                if (!string.IsNullOrEmpty(Quotation_Date))
                {
                    dt_Quotation.Text = Quotation_Date;
                }
                if (Convert.ToString(Request.QueryString["Flag"]) == "CustomerDeliveryFlag")
                {
                    hddnCustomerDelivery.Value = "Yes";
                    dt_PLSales.Date = DateTime.Now;
                    ddlInventoryId.Visible = false;
                    if (!string.IsNullOrEmpty(Request.QueryString["DlvTyeId"]))
                    {
                        if (Convert.ToString(Request.QueryString["DlvTyeId"]) == "1")
                        {
                            hddnCustomerDeliverySDOrOD.Value = "1";
                            crossBtnId.Visible = false;
                            crossBtnCustDeliveryListId.Visible = false;
                            crossBtnPendingSecondHand.Visible = false;
                            crossBtnCustDeliveryListForSD.Visible = true;
                            crossBtnPendingDeliveryListId.Visible = false;
                        }
                        else if (Convert.ToString(Request.QueryString["DlvTyeId"]) == "0")
                        {
                            hddnCustomerDeliverySDOrOD.Value = "0";
                            crossBtnId.Visible = false;
                            crossBtnCustDeliveryListId.Visible = true;
                            crossBtnPendingSecondHand.Visible = false;
                            crossBtnCustDeliveryListForSD.Visible = false;
                            crossBtnPendingDeliveryListId.Visible = false;
                        }
                        else if (Convert.ToString(Request.QueryString["DlvTyeId"]) == "4")
                        {
                            hddnCustomerDeliverySDOrOD.Value = "4";
                            crossBtnId.Visible = false;
                            crossBtnCustDeliveryListId.Visible = false;
                            crossBtnPendingSecondHand.Visible = true;
                            crossBtnCustDeliveryListForSD.Visible = false;
                            crossBtnPendingDeliveryListId.Visible = false;
                        }
                    }


                    hdnnCustomerOrPendingDelivery.Value = "CustomDeliveryPending";

                    //View Mode only

                    if (CustDeliveryCount != null)
                    {
                        if (CustDeliveryCount.Rows.Count > 0)
                        {
                            ASPxButton12.ClientVisible = false;
                            btn_SaveRecords.ClientVisible = false;
                            ClientShowMsg.Visible = true;
                        }
                    }//End




                }
                else if (Convert.ToString(Request.QueryString["Flag"]) == "PendingDeliveryFlag")
                {
                    crossBtnId.Visible = false;
                    crossBtnCustDeliveryListId.Visible = false;
                    crossBtnPendingSecondHand.Visible = false;
                    crossBtnPendingDeliveryListId.Visible = true;
                    hdnnCustomerOrPendingDelivery.Value = "PendingDeliveryList";
                    hddnCustomerDelivery.Value = "No";


                }




                txtCustName.ClientEnabled = false;
                dt_PLSales.ClientEnabled = false;
                ddl_Branch.Enabled = false;
                txt_Refference.Enabled = false;
                cmbContactPerson.ClientEnabled = false;
                txtCreditDays.ClientEnabled = false;
                dt_SaleInvoiceDue.ClientEnabled = false;
                ddl_Currency.Enabled = false;
                ddl_AmountAre.ClientEnabled = false;


                ProjectActivitiesBL objProjectActivitiesBL1 = new ProjectActivitiesBL();
                string strCompanyID = Convert.ToString(Session["LastCompany"]);
                //string strBranchID = Convert.ToString(Session["userbranchID"]);
                string strBranchID = Convert.ToString(dt_InvoiceHeader.Rows[0]["pos_assignBranch"]);
                string FinYear = Convert.ToString(Session["LastFinYear"]);
                //DataTable Schemadt = objProjectActivitiesBL1.GetNumberingSchema(strCompanyID, strBranchID, FinYear, "10", "Y");
                string DlvTypeId = Convert.ToString(Request.QueryString["DlvTyeId"]);
                DataTable Schemadt = null;
                //userbranch pijush da :branc heiarchy
                if (DlvTypeId == "1")
                {
                    Schemadt = objProjectActivitiesBL1.GetNumberingSchema(strCompanyID, strBranchID, FinYear, "115", "Y");
                }
                else if (DlvTypeId == "0")
                {
                    Schemadt = objProjectActivitiesBL1.GetNumberingSchema(strCompanyID, strBranchID, FinYear, "115", "Y");
                }

                if (Schemadt != null && Schemadt.Rows.Count > 0)
                {
                    ddl_numberingScheme.DataTextField = "SchemaName";
                    ddl_numberingScheme.DataValueField = "Id";
                    ddl_numberingScheme.DataSource = Schemadt;
                    ddl_numberingScheme.DataBind();

                    if (Schemadt.Rows.Count > 1)
                    {
                        ddl_numberingScheme.SelectedValue = Convert.ToString(Schemadt.Rows[1]["Id"]);
                        //ddl_numberingScheme.SelectedValue = Convert.ToString(dt_InvoiceHeader.Rows[0]["pos_assignBranch"]);
                    }

                    ddl_numberingScheme.Enabled = false;

                }




                ltrTitle.Text = "Add Project Sales Challan";
                rdl_SaleInvoice.SelectedValue = "SI";
                rdl_SaleInvoice.Enabled = false;
                hdnPageStatus.Value = "EditModeOnDirect";
                Session["ActionType"] = "Add";
                Session["Doc_Type"] = "SI";
                btn_SaveRecords.Visible = false;
                hdAddOrEdit.Value = "Add";
                //ddl_numberingDiv.Visible = false;
            }//End


            if (!IsPostBack)
            {
                this.Session["LastCompany1"] = Session["LastCompany"];
                this.Session["LastFinYear1"] = Session["LastFinYear"];
                this.Session["userbranch"] = Session["userbranchID"];
                if (Request.QueryString["Permission"] != null)
                {
                    //pnl_quotation.Enabled = true;
                    if (Convert.ToString(Request.QueryString["Permission"]) == "1")
                    {
                        //pnl_quotation.Enabled = true;
                    }
                    else if (Convert.ToString(Request.QueryString["Permission"]) == "2")
                    {
                        //pnl_quotation.Enabled = true;
                    }
                    else if (Convert.ToString(Request.QueryString["Permission"]) == "3")
                    {
                        //pnl_quotation.Enabled = true;
                    }
                }



                if (string.IsNullOrEmpty(Request.QueryString["CustID"]))
                {
                    crossBtnId.Visible = true;
                    crossBtnCustDeliveryListId.Visible = false;
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
                    ASPxPageControl1.ActiveTabIndex = 0;
                    hdntab2.Value = "0";
                }
                TabPage page = ASPxPageControl1.TabPages.FindByName("General");
                page.Visible = true;
                //SetFinYearCurrentDate();
                Session["Doc_Type"] = "SO";
                GetFinacialYearBasedQouteDate();
                dt_PlOrderExpiry.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                string finyear = Convert.ToString(Session["LastFinYear"]);
                //GetAllDropDownDetailForSalesOrder();
                //GetAllDropDownDetailForSalesOrder(userbranch);
                PopulateGSTCSTVATCombo(DateTime.Now.ToString("yyyy-MM-dd"));
                PopulateChargeGSTCSTVATCombo(DateTime.Now.ToString("yyyy-MM-dd"));
                //kaushik 24-2-2017
                Session["SCBillingAddressLookup"] = null;
                Session["SCShippingAddressLookup"] = null;
                Session["ChallanAddressDtl"] = null;
                //kaushik 24-2-2017
                Session["CustomerDetail"] = null;
                Session["SC_WarehouseData"] = null;
                Session["SC_LoopWarehouse"] = 1;
                Session["ProjectChallanTaxDetails"] = null;
                Session["WarehouseBindQty"] = null;
                Session["InlineRemarks"] = null;


                //Purpose : Binding Batch Edit Grid
                //Name : SuBHABRATA 
                // Dated : 21-01-2017
                string strOrderId = "";
                grid.AddNewRow();
                if (Request.QueryString["key"] != null && string.IsNullOrEmpty(Request.QueryString["CustID"]))
                {

                    if (Convert.ToString(Request.QueryString["key"]) != "ADD")
                    {
                        hddnCustomerDelivery.Value = "No";
                        ddl_AmountAre.ClientEnabled = false;
                        //CustomerComboBox.ClientEnabled = false; //subhabrata on 27-12-2017
                        txtCustName.ClientEnabled = false;
                        GetAllDropDownDetailForSalesOrder(userbranch);
                        //SUBHABRATA:Purpose(For cross button on dlvType)
                        if (Request.QueryString.AllKeys.Contains("DlvTyeId"))
                        {
                            if (!string.IsNullOrEmpty(Request.QueryString["DlvTyeId"]))
                            {
                                //lookup_Customer.ClientEnabled = false;
                                //CustomerComboBox.ClientEnabled = false;//subhabrata on 27-12-2017
                                txtCustName.ClientEnabled = false;
                                if (Convert.ToString(Request.QueryString["DlvTyeId"]) == "1")
                                {
                                    hddnCustomerDeliverySDOrOD.Value = "1";
                                    crossBtnId.Visible = false;
                                    crossBtnCustDeliveryListId.Visible = false;
                                    crossBtnPendingSecondHand.Visible = false;
                                    crossBtnCustDeliveryListForSD.Visible = true;
                                    crossBtnPendingDeliveryListId.Visible = false;
                                }
                                else if (Convert.ToString(Request.QueryString["DlvTyeId"]) == "0")
                                {
                                    hddnCustomerDeliverySDOrOD.Value = "0";
                                    crossBtnId.Visible = false;
                                    crossBtnCustDeliveryListId.Visible = true;
                                    crossBtnPendingSecondHand.Visible = false;
                                    crossBtnCustDeliveryListForSD.Visible = false;
                                    crossBtnPendingDeliveryListId.Visible = false;
                                }
                                else if (Convert.ToString(Request.QueryString["DlvTyeId"]) == "2")
                                {
                                    //hddnCustomerDeliverySDOrOD.Value = "0";
                                    crossBtnId.Visible = false;
                                    crossBtnCustDeliveryListId.Visible = false;
                                    crossBtnPendingSecondHand.Visible = false;
                                    crossBtnCustDeliveryListForSD.Visible = false;
                                    crossBtnPendingDeliveryListId.Visible = true;
                                }
                            }
                        }
                        else
                        {
                            crossBtnId.Visible = true;
                            crossBtnCustDeliveryListId.Visible = false;
                            crossBtnPendingSecondHand.Visible = false;
                            crossBtnCustDeliveryListForSD.Visible = false;
                            crossBtnPendingDeliveryListId.Visible = false;
                        }



                        //  bindLookUP("DONE");
                        hdnPageStatus.Value = "update";
                        ltrTitle.Text = "Modify Project Sales Challan";
                        strOrderId = Convert.ToString(Request.QueryString["key"]);
                        hfDocId.Value = strOrderId;
                        Session["ChallanID"] = strOrderId;
                        Session["ActionType"] = "Edit";
                        Session["SC_WarehouseData"] = GetChallanWarehouseData();
                        //Session["SC_WarehouseData"] = GetOrderWarehouseData();
                        hdAddOrEdit.Value = "Edit";




                        //kaushik 25-2-2017

                        Session["KeyVal_InternalID"] = "PISC" + strOrderId;
                        //kaushik 25-2-2017
                        //ddl_numberingScheme.Visible = false;
                        ddl_numberingDiv.Visible = false;

                        txt_SlChallanNo.ClientEnabled = false;

                        //GetAllDropDownDetailForSalesOrder();
                        if (Session["userbranchHierarchy"] != null)
                        {
                            userbranch = Convert.ToString(Session["userbranchHierarchy"]);
                        }

                        DataTable dt = new DataTable();
                        dt = GetIdFromSalesInvoiceExists();

                        SetOrderDetails();

                        #region Subhabrata Get Tax Details in Edit Mode

                        //  Session["ProjectChallanTaxDetails"] = GetQuotationTaxData().Tables[0];
                        //from quotation


                        Session["ProjectChallanTaxDetails"] = GetTaxDataWithGST(GetTaxData(dt_PLSales.Date.ToString("yyyy-MM-dd")));




                        DataTable quotetable = GetQuotationEditedTaxData().Tables[0];

                        if (quotetable == null)
                        {
                            CreateDataTaxTable();
                        }
                        else
                        {
                            Session["ProjectChallanFinalTaxRecord"] = quotetable;
                        }

                        #endregion Debjyoti Get Tax Details in Edit Mode

                        Session["ChallanDetails"] = GetOrderData().Tables[0];
                        Session["InlineRemarks"] = GetOrderData().Tables[1];
                        grid.DataSource = GetSalesOrder();
                        grid.DataBind();

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            if (Convert.ToInt64(dt.Rows[0]["InvoiceDetails_Id"]) > 0)
                            {
                                //pnl_quotation.Enabled = false;
                                //ASPxButton12.Enabled = false;
                                //btn_SaveRecords.Enabled = false;
                                ddl_Branch.Enabled = false;
                                dt_PLSales.ClientEnabled = false;
                            }
                        }



                        //kaushik 24-2-2017
                        hdnmodeId.Value = "ProjectChallan" + strOrderId;
                        //lookup_quotation.Properties.PopupFilterMode = DevExpress.XtraEditors.Popup.TreeListLookUpEditPopupForm.Contains;


                    }
                    else
                    {

                        //sqlQuationList.SelectParameters.Clear();
                        //sqlQuationList.SelectParameters.Add("status", "NOT-DONE");
                        //lookup_quotation.DataBind();
                        // bindLookUP("NOT-DONE");
                        ltrTitle.Text = "Add Project Sales Challan";
                        Session["ActionType"] = "Add";
                        hdnPageStatus.Value = "first";
                        CreateDataTaxTable();
                        hdnmodeId.Value = "Add";
                        crossBtnId.Visible = true;
                        crossBtnCustDeliveryListId.Visible = false;
                        crossBtnPendingSecondHand.Visible = false;
                        crossBtnPendingDeliveryListId.Visible = false;
                        crossBtnCustDeliveryListForSD.Visible = false;
                        hddnCustomerDelivery.Value = "No";

                    }
                }

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GridCallBack()", true);


                IsUdfpresent.Value = Convert.ToString(getUdfCount());




            }

            //PopulateCustomerDetail();//Commented by:Subhabrata on 01-12-2017
            GetSalesOrderSchemaLength();

            if (!IsPostBack)
            {
                #region Samrat Roy -- Hide Save Button in Edit Mode
                if (Request.QueryString["req"] != null && Request.QueryString["req"] == "V")
                {
                    ltrTitle.Text = "View Project Sales Challan";
                    lbl_quotestatusmsg.Text = "*** View Mode Only";
                    btn_SaveRecords.Visible = false;
                    ASPxButton12.Visible = false;
                    lbl_quotestatusmsg.Visible = true;
                }
                #endregion
            }

            if (Request.QueryString["key"] != null && Request.QueryString["key"] != "ADD" && string.IsNullOrEmpty(Request.QueryString["CustID"]))
            {
                //SetOrderDetails();
                string strOrderId1 = Convert.ToString(Request.QueryString["key"]);
                if (Request.QueryString["status"] == null && Request.QueryString["req"] != "V")
                {
                    IsExistsDocumentInERPDocApproveStatus(strOrderId1);
                }
                Session["ChallanID"] = strOrderId1;
                DataTable OrderEditdt = GetOrderEditData();
                if (OrderEditdt != null && OrderEditdt.Rows.Count > 0)
                {
                    string Quoids = Convert.ToString(OrderEditdt.Rows[0]["Challan_Doc_Ids"]);
                    //Session["Lookup_QuotationIds"] = Quoids;
                    string Order_Date = Convert.ToString(OrderEditdt.Rows[0]["Challan_Date"]);
                    string Customer_Id = Convert.ToString(OrderEditdt.Rows[0]["Customer_Id"]);
                    string Doc_type = Convert.ToString(OrderEditdt.Rows[0]["Doc_Type"]);
                    if (!String.IsNullOrEmpty(Quoids))
                    {
                        string[] eachQuo = Quoids.Split(',');
                        if (eachQuo.Length > 1)//More than one quotation
                        {
                            if (Doc_type == "SO")
                            {
                                dt_Quotation.Text = "Multiple Select Order Dates";
                            }
                            else
                            {
                                dt_Quotation.Text = "Multiple Select Invoice Dates";
                            }
                            BindLookUp(Customer_Id, Order_Date, Doc_type);
                            foreach (string val in eachQuo)
                            {
                                lookup_order.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));

                            }
                            // lbl_MultipleDate.Attributes.Add("style", "display:block");
                        }
                        else if (eachQuo.Length == 1)//Single Quotation
                        { //lbl_MultipleDate.Attributes.Add("style", "display:none"); }
                            BindLookUp(Customer_Id, Order_Date, Doc_type);
                            foreach (string val in eachQuo)
                            {
                                lookup_order.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                            }
                        }
                        else//No Quotation selected
                        {
                            BindLookUp(Customer_Id, Order_Date, Doc_type);
                        }
                    }
                }

            }
        }

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

        //kaushik 24-2-2017

        protected int getUdfCount()
        {
            DataTable udfCount = oDBEngine.GetDataTable("select 1 from tbl_master_remarksCategory rc where cat_applicablefor='SC'  and ( exists (select * from tbl_master_udfGroup where id=rc.cat_group_id and grp_isVisible=1) or rc.cat_group_id=0)");
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



        #region BindCustomerOnDemand
        //protected void ASPxComboBox_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        //{
        //    if (e.Filter != "")
        //    {
        //        ASPxComboBox comboBox = (ASPxComboBox)source;
        //        CustomerDataSource.SelectCommand =
        //               @"select cnt_internalid,uniquename,Name,Billing from (SELECT cnt_internalid,uniquename,Name,Billing, row_number()over(order by t.[Name]) as [rn]  from v_pos_customerDetails  as t where (([uniquename] + ' ' + [Name] ) LIKE @filter)) as st where st.[rn] between @startIndex and @endIndex";

        //        CustomerDataSource.SelectParameters.Clear();
        //        CustomerDataSource.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
        //        CustomerDataSource.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
        //        CustomerDataSource.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
        //        comboBox.DataSource = CustomerDataSource;
        //        comboBox.DataBind();
        //    }
        //}


        //protected void ASPxComboBox_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        //{
        //    long value = 0;
        //    if (e.Value == null || !Int64.TryParse(e.Value.ToString(), out value))
        //        return;
        //    ASPxComboBox comboBox = (ASPxComboBox)source;
        //    CustomerDataSource.SelectCommand = @"SELECT cnt_internalid,uniquename,Name,Billing FROM v_pos_customerDetails WHERE (cnt_internalid = @ID) ";

        //    CustomerDataSource.SelectParameters.Clear();
        //    CustomerDataSource.SelectParameters.Add("ID", TypeCode.String, e.Value.ToString());
        //    comboBox.DataSource = CustomerDataSource;
        //    comboBox.DataBind();
        //}


        //protected void SetCustomerDDbyValue(string customerId)
        //{

        //    CustomerDataSource.SelectCommand = @"SELECT cnt_internalid,uniquename,Name,Billing FROM v_pos_customerDetails WHERE (cnt_internalid = @ID) ";

        //    CustomerDataSource.SelectParameters.Clear();
        //    CustomerDataSource.SelectParameters.Add("ID", TypeCode.String, customerId);
        //    CustomerComboBox.DataSource = CustomerDataSource;
        //    CustomerComboBox.DataBind();
        //    CustomerComboBox.Value = customerId;

        //}

        #endregion



        public void BindProjectChallanDirect(string Bill_No, string Cust_Id)
        {
            DateTime Order_Date = DateTime.Now;
            DataTable InvoiceInfo = GetInvoiceDateData();
            string strBranchID = string.Empty;

            if (Convert.ToString(Request.QueryString["Flag"]) == "PendingDeliveryFlag" || Convert.ToString(Request.QueryString["Flag"]) == "SecondHandSale")//added on 26-06-2017
            {
                strBranchID = Convert.ToString(InvoiceInfo.Rows[0]["Invoice_BranchId"]);
                dt_PLSales.Date = Convert.ToDateTime(InvoiceInfo.Rows[0]["Invoice_Dt"]);

                ProjectActivitiesBL objProjectActivitiesBL1 = new ProjectActivitiesBL();
                string strCompanyID = Convert.ToString(Session["LastCompany"]);
                //string strBranchID = Convert.ToString(Session["userbranchID"]);
                string FinYear = Convert.ToString(Session["LastFinYear"]);
                //DataTable Schemadt = objProjectActivitiesBL1.GetNumberingSchema(strCompanyID, strBranchID, FinYear, "10", "Y");
                DataTable Schemadt = null;
                //userbranch pijush da :branc heiarchy
                Schemadt = objProjectActivitiesBL1.GetNumberingSchemaPendingDelivery(strCompanyID, strBranchID, FinYear, "10", "Y");

                if (Schemadt != null && Schemadt.Rows.Count > 0)
                {
                    ddl_numberingScheme.DataTextField = "SchemaName";
                    ddl_numberingScheme.DataValueField = "Id";
                    ddl_numberingScheme.DataSource = Schemadt;
                    ddl_numberingScheme.DataBind();

                    if (Schemadt.Rows.Count > 1)
                    {
                        ddl_numberingScheme.SelectedValue = Convert.ToString(Schemadt.Rows[1]["Id"]);
                        //ddl_numberingScheme.SelectedValue = Convert.ToString(dt_InvoiceHeader.Rows[0]["pos_assignBranch"]);
                    }

                    ddl_numberingScheme.Enabled = false;

                }

                if (Convert.ToString(Request.QueryString["Flag"]) == "SecondHandSale")
                {
                    hddnCustomerDeliverySDOrOD.Value = "4";
                }
                txt_SlChallanNo.Text = Convert.ToString(InvoiceInfo.Rows[0]["Invoice_Number"]);
                //ddl_numberingDiv.Visible = false;
            }
            else
            {
                strBranchID = Convert.ToString(InvoiceInfo.Rows[0]["pos_assignBranch"]);
            }

            //Subhabrata Lookup_Customer
            //DataTable dtCustomer = new DataTable();
            //dtCustomer = objProjectActivitiesBL.PopulateCustomerDetail();

            //if (dtCustomer != null && dtCustomer.Rows.Count > 0)
            //{
            //    lookup_Customer.DataSource = dtCustomer;
            //    lookup_Customer.DataBind();
            //    Session["CustomerDetail"] = dtCustomer;
            //    lookup_Customer.GridView.Selection.SelectRowByKey(Cust_Id);
            //} 

            //SetCustomerDDbyValue(Cust_Id);//Subhabrata on 27-12-2017
            txtCustName.Text = Convert.ToString(InvoiceInfo.Rows[0]["Customer"]);
            //End
            Session["Doc_Type"] = "SI";

            DataTable SalesOrderTable;

            string dt = Convert.ToString(Request.QueryString["BillDate"]);
            //Subhabrata Lookup Order bind
            SalesOrderTable = objBL.GetProjectInvoiceonSalesChallan(Cust_Id, dt, "NOT-DONE", Convert.ToInt32(strBranchID));
            Session["CDPData"] = SalesOrderTable;
            lookup_order.GridView.Selection.CancelSelection();
            lookup_order.DataSource = SalesOrderTable;
            lookup_order.DataBind();
            lookup_order.GridView.Selection.SelectRowByKey(Convert.ToInt32(Bill_No));
            //End

            grid.DataSource = GetSalesOrderLinesDirect(Bill_No);
            grid.DataBind();

        }

        //Debjyoti GST on 30-06-2017
        public void SetTaxJSONData()
        {
            #region NewTaxblock
            string ItemLevelTaxDetails = string.Empty; string HSNCodewisetaxSchemid = string.Empty; string BranchWiseStateTax = string.Empty; string StateCodeWiseStateIDTax = string.Empty;
            gstTaxDetails.GetTaxData(ref ItemLevelTaxDetails, ref HSNCodewisetaxSchemid, ref BranchWiseStateTax, ref StateCodeWiseStateIDTax, "S");
            HDItemLevelTaxDetails.Value = ItemLevelTaxDetails;
            HDHSNCodewisetaxSchemid.Value = HSNCodewisetaxSchemid;
            HDBranchWiseStateTax.Value = BranchWiseStateTax;
            HDStateCodeWiseStateIDTax.Value = StateCodeWiseStateIDTax;


            #endregion
        }
        //END
        public IEnumerable GetSalesOrderLinesDirect(string Bill_No)
        {
            List<SalesOrder> OrderList = new List<SalesOrder>();
            DataTable Orderdt = GetOrderDataDirect(Bill_No).Tables[0];
            string commaSeparatedString = String.Join(",", Orderdt.AsEnumerable().Select(x => x.Field<Int64>("OrderID").ToString()).ToArray());
            DataTable tempProductTax = GetDocumentProductTaxData(commaSeparatedString);
            DataColumnCollection dtC = Orderdt.Columns;
            for (int i = 0; i < Orderdt.Rows.Count; i++)
            {
                SalesOrder Orders = new SalesOrder();

                Orders.SrlNo = Convert.ToString(Orderdt.Rows[i]["SrlNo"]);
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
                Orders.Remarks = Convert.ToString(Orderdt.Rows[i]["Remarks"]);
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
                string strQuoteDetails_Id = Convert.ToString(Orderdt.Rows[i]["OrderID"]).Trim();
                if (tempProductTax != null && tempProductTax.Rows.Count > 0)
                {
                    var taxrows = tempProductTax.Select("ProductTax_ProductId ='" + strQuoteDetails_Id + "'");
                    foreach (var row in taxrows)
                    {
                        row["SlNo"] = Convert.ToString(i + 1);
                    }
                    tempProductTax.AcceptChanges();
                }


            }

            if (tempProductTax != null)
            {
                tempProductTax.Columns.Remove("ProductTax_ProductId");
                Session["ProjectChallanFinalTaxRecord"] = tempProductTax;
            }
            else { Session["ProjectChallanFinalTaxRecord"] = null; }

            BindSessionByDatatableDirect(Orderdt);

            return OrderList;
        }


        public void bindLookUP(string status)
        {
            DataTable QuotationTable;


            //   string[] taxconfigDetails = oDBEngine.GetFieldValue1("Config_TaxRates", "TaxRates_TaxCode,TaxRatesSchemeName", "TaxRates_ID=" + code, 2);
            //productTable = oDBEngine.GetDataTable("select sProducts_ID,sProducts_Code,sProducts_Name from Master_sProducts where sProducts_ID not in (select prodId from tbl_trans_ProductTaxRate where TaxRates_TaxCode<>" + taxconfigDetails[0] + " and TaxRatesSchemeName<>'" + taxconfigDetails[1] + "' )");
            //GridLookup.DataSource = productTable;
            //GridLookup.DataBind();
            //QuotationTable = objBL.GetQuotationOnSalesOrder(status);
            //lookup_quotation.GridView.Selection.CancelSelection();
            //lookup_quotation.DataSource = QuotationTable;
            //lookup_quotation.DataBind();



            //Session["QuotationData"] = QuotationTable;
        }
        public void SetFinYearCurrentDate()
        {
            dt_PLSales.EditFormatString = objConverter.GetDateFormat("Date");
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

            dt_PLSales.Value = DateTime.ParseExact(fDate, @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        }
        public void GetFinacialYearBasedQouteDate()
        {
            String finyear = "";
            if (Session["LastFinYear"] != null)
            {
                finyear = Convert.ToString(Session["LastFinYear"]).Trim();
                DataTable dtFinYear = objProjectActivitiesBL.GetFinacialYearBasedQouteDate(finyear);
                if (dtFinYear != null && dtFinYear.Rows.Count > 0)
                {
                    Session["FinYearStartDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearStartDate"]);
                    Session["FinYearEndDate"] = Convert.ToString(dtFinYear.Rows[0]["finYearEndDate"]);
                    if (Session["FinYearStartDate"] != null)
                    {
                        dt_PLSales.MinDate = Convert.ToDateTime(Convert.ToString(Session["FinYearStartDate"]));
                    }
                    if (Session["FinYearEndDate"] != null)
                    {
                        dt_PLSales.MaxDate = Convert.ToDateTime(Convert.ToString(Session["FinYearEndDate"]));
                    }
                }
            }
            //dt_PLQuote.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
        }
        public void PopulateCustomerDetail()
        {
            if (Session["CustomerDetail"] == null)
            {
                DataTable dtCustomer = new DataTable();
                dtCustomer = objProjectActivitiesBL.PopulateCustomerDetail();

                if (dtCustomer != null && dtCustomer.Rows.Count > 0)
                {
                    //lookup_Customer.DataSource = dtCustomer;
                    //lookup_Customer.DataBind();
                    Session["CustomerDetail"] = dtCustomer;
                }
            }
            else
            {
                //lookup_Customer.DataSource = (DataTable)Session["CustomerDetail"];
                //lookup_Customer.DataBind();
            }

        }
        public void GetAllDropDownDetailForSalesOrder(string UserBranch)
        {
            DataSet dst = new DataSet();
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string strBranchID = Convert.ToString(Session["userbranchID"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            dst = objProjectActivitiesBL.GetAllDropDownDetailForProjectChallan(UserBranch);
            ProjectActivitiesBL objProjectActivitiesBL1 = new ProjectActivitiesBL();
            DataTable Schemadt = objProjectActivitiesBL1.GetNumberingSchema(strCompanyID, userbranchHierarchy, FinYear, "10", "Y");
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
            //if (dst.Tables[2] != null && dst.Tables[2].Rows.Count > 0)
            //{
            //    ddl_SalesAgent.DataTextField = "Name";
            //    ddl_SalesAgent.DataValueField = "cnt_id";
            //    ddl_SalesAgent.DataSource = dst.Tables[2];
            //    ddl_SalesAgent.DataBind();
            //    ddl_SalesAgent.Items.Insert(0, new ListItem("Select", "0"));
            //}
            if (dst.Tables[3] != null && dst.Tables[3].Rows.Count > 0)
            {
                //ddl_Currency.DataTextField = "Currency_Name";
                //ddl_Currency.DataValueField = "Currency_ID";
                //ddl_Currency.DataSource = dst.Tables[3];
                //ddl_Currency.DataBind();
            }
            if (dst.Tables[4] != null && dst.Tables[4].Rows.Count > 0)
            {
                ddl_AmountAre.TextField = "taxGrp_Description";
                ddl_AmountAre.ValueField = "taxGrp_Id";
                ddl_AmountAre.DataSource = dst.Tables[4];
                ddl_AmountAre.DataBind();
                ddl_AmountAre.SelectedIndex = 0;


            }
            if (dst.Tables[5] != null && dst.Tables[5].Rows.Count > 0)
            {
                //ddl_Quotation_No.DataValueField = "Quote_Id";
                //ddl_Quotation_No.DataTextField = "Quote_Number";
                //ddl_Quotation_No.DataSource = dst.Tables[5];
                //ddl_Quotation_No.DataBind();

                //ddl_Quotation.ValueField = "Quote_Id";
                //ddl_Quotation.TextField = "Quote_Number";
                //ddl_Quotation.DataSource = dst.Tables[5];
                //ddl_Quotation.DataBind();
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

            //ddl_SalesAgent.Items.Insert(0, new ListItem("Select", "0"));


            //if (Session["ActiveCurrency"] != null)
            //{
            //    if (ddl_Currency.Items.Count > 0)
            //    {
            //        string[] ActCurrency = new string[] { };
            //        string currency = Convert.ToString(HttpContext.Current.Session["ActiveCurrency"]);
            //        ActCurrency = currency.Split('~');
            //        foreach (ListItem li in ddl_Currency.Items)
            //        {
            //            if (li.Value == Convert.ToString(ActCurrency[0]))
            //            {
            //                ddl_Currency.Items.Remove(li);
            //                break;
            //            }
            //        }
            //    }
            //    ddl_Currency.Items.Insert(0, new ListItem("Select", "0"));
            //    ddl_Currency.SelectedIndex = 0;
            //}
        }
        protected void cmbContactPerson_Callback(object sender, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "BindContactPerson")
            {
                string InternalId = Convert.ToString(Convert.ToString(e.Parameter.Split('~')[1]));
                PopulateContactPersonOfCustomer(InternalId);

                DataTable dtDeuDate = objSalesInvoiceBL.GetCustomerDetails_InvoiceRelated(InternalId);
                foreach (DataRow dr in dtDeuDate.Rows)
                {
                    string strDueDate = Convert.ToString(dr["DueDate"]);
                    cmbContactPerson.JSProperties["cpDueDate"] = strDueDate;
                    //dt_SaleInvoiceDue.Date = Convert.ToDateTime(strDeuDate);
                }

                DataTable dtTotalDues = objSalesInvoiceBL.GetCustomerTotalDues(InternalId);
                if (dtTotalDues != null && dtTotalDues.Rows.Count > 0)
                {
                    string totalDues = Convert.ToString(dtTotalDues.Rows[0]["NetOutstanding"]);
                    cmbContactPerson.JSProperties["cpTotalDue"] = totalDues;
                }
                else
                {
                    cmbContactPerson.JSProperties["cpTotalDue"] = "0.00";
                }
            }


        }
        protected void PopulateContactPersonOfCustomer(string InternalId)
        {
            string ContactPerson = "";
            DataTable dtContactPerson = new DataTable();
            ProjectActivitiesBL objProjectActivitiesBL = new ProjectActivitiesBL();
            dtContactPerson = objProjectActivitiesBL.PopulateContactPersonOfCustomer(InternalId);
            if (dtContactPerson != null && dtContactPerson.Rows.Count > 0)
            {
                cmbContactPerson.TextField = "contactperson";
                cmbContactPerson.ValueField = "add_id";
                cmbContactPerson.DataSource = dtContactPerson;
                cmbContactPerson.DataBind();
                foreach (DataRow dr in dtContactPerson.Rows)
                {
                    if (Convert.ToString(dr["Isdefault"]) == "True")
                    {
                        ContactPerson = Convert.ToString(dr["add_id"]);
                        break;
                    }
                }
                cmbContactPerson.SelectedItem = cmbContactPerson.Items.FindByValue(ContactPerson);

            }



            //dtContactPerson = objProjectActivitiesBL.PopulateContactPersonForCustomerOrLead(InternalId);
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
        protected void ddl_VatGstCst_Callback(object sender, CallbackEventArgsBase e)
        {
            string type = e.Parameter.Split('~')[0];
            PopulateGSTCSTVAT(type);
        }

        //protected void ComponentIsInventory_Callback(object sender, CallbackEventArgsBase e)
        //{
        //    Session["IsInvenory"] = null;
        //    string type = e.Parameter.Split('~')[0];
        //    string IsInventoryVal = e.Parameter.Split('~')[1];
        //    if (type == "BindSession")
        //    {
        //        if (IsInventoryVal == "1")
        //        {
        //            Session["IsInvenory"] = "1";
        //        }
        //        else
        //        {
        //            Session["IsInvenory"] = "0";
        //        }

        //    }
        //}
        protected void PopulateContactPerson(string customerId)
        {
            PopulateContactPersonOfCustomer(customerId);
        }
        protected void PopulateGSTCSTVAT(string type)
        {
            DataTable dtGSTCSTVAT = new DataTable();
            ProjectActivitiesBL objProjectActivitiesBL = new ProjectActivitiesBL();
            if (type == "2")
            {
                dtGSTCSTVAT = objProjectActivitiesBL.PopulateGSTCSTVAT(dt_PLSales.Date.ToString("yyyy-MM-dd"));

                #region Delete Igst,Cgst,Sgst respectively

                string CompInternalId = Convert.ToString(Session["LastCompany"]);
                string BranchId = Convert.ToString(Session["userbranchID"]);
                string[] compGstin = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_gstin", "cmp_internalid='" + CompInternalId + "'", 1);
                string[] branchGstin = oDBEngine.GetFieldValue1("tbl_master_branch", "isnull(branch_GSTIN,'')branch_GSTIN", "branch_id='" + BranchId + "'", 1);
                String GSTIN = "";
                if (branchGstin[0].Trim() != "")
                {
                    GSTIN = branchGstin[0].Trim();
                }
                else
                {
                    if (compGstin.Length > 0)
                    {
                        GSTIN = compGstin[0].Trim();
                    }
                }

                string ShippingState = "";

                #region ##### Added By : Samrat Roy -- For BillingShippingUserControl ######
                string sstateCode = BillingShippingControl.GetShippingStateCode(Request.QueryString["key"]);
                ShippingState = sstateCode;
                if (ShippingState.Trim() != "")
                {
                    ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
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

                if (ShippingState.Trim() != "" && GSTIN.Trim() != "")
                {


                    if (GSTIN.Substring(0, 2) == ShippingState)
                    {
                        //Check if the state is in union territories then only UTGST will apply
                        //   Chandigarh     Andaman and Nicobar Islands    DADRA & NAGAR HAVELI    DAMAN & DIU     Lakshadweep              PONDICHERRY
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

                //If Company GSTIN is blank then Delete All CGST,UGST,IGST,CGST
                if (GSTIN.Trim() == "")
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
        protected void grid_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "Number")
            {
                e.Value = string.Format("Item #{0}", e.ListSourceRowIndex);
            }
            if (e.Column.FieldName == "Warehouse")
            {
                //e.Value = string.Format("Item #{0}", e.ListSourceRowIndex);
                //e.Row.Cells[6].Attributes.Add("onclick", "javascript:ShowMissingData('" + ContactID + "');");
            }

        }
        protected void grid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            //if (e.RowType == GridViewRowType.Data)
            //{
            //    string AssetVal = Request.QueryString["accountType"].ToString();
            //    string AssetVal = Convert.ToString(Request.QueryString["accountType"]);
            //    //string kv = e.GetValue("SubAccount_Code").ToString();
            //    string kv = Convert.ToString(e.GetValue("SubAccount_Code"));
            //    //string cellv = e.GetValue("SubAccount_MainAcReferenceID").ToString();
            //    string cellv = Convert.ToString(e.GetValue("SubAccount_MainAcReferenceID"));
            //    //string subaccountcode123 = Request.QueryString["accountcode"].ToString().Trim();
            //    string subaccountcode123 = Convert.ToString(Request.QueryString["accountcode"]).Trim();
            //    if (Segment == "5")
            //    {
            //        if (AssetVal == "Asset" && Segment == "5")
            //        {
            //            e.Row.Cells[6].Style.Add("cursor", "hand");
            //            // e.Row.Cells[12].Attributes.Add("onclick", "javascript:ShowMissingData('" + ContactID + "');");
            //            //e.Row.Cells[6].Attributes.Add("onclick", "javascript:showhistory('" + kv + cellv + "');");
            //            e.Row.Cells[6].Attributes.Add("onclick", "javascript:showhistory('" + ViewState["MainAccountCode"] + "-" + kv + "');");
            //            e.Row.Cells[6].ToolTip = "ADD/VIEW";
            //            e.Row.Cells[6].Style.Add("color", "Blue");
            //        }
            //        else
            //        {
            //            e.Row.Cells[5].Style.Add("cursor", "hand");
            //            // e.Row.Cells[12].Attributes.Add("onclick", "javascript:ShowMissingData('" + ContactID + "');");
            //            //e.Row.Cells[5].Attributes.Add("onclick", "javascript:showhistory('" + kv + cellv + "');");
            //            e.Row.Cells[5].Attributes.Add("onclick", "javascript:showhistory('" + ViewState["MainAccountCode"] + "-" + kv + "');");
            //            e.Row.Cells[5].ToolTip = "ADD/VIEW";
            //            e.Row.Cells[5].Style.Add("color", "Blue");
            //        }
            //    }
            //}
        }
        protected void grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            //if (e.DataColumn.FieldName == "Warehouse")
            //{
            //    //string kv = e.GetValue("SubAccount_Code").ToString();
            //    //string cellv = e.GetValue("SubAccount_MainAcReferenceID").ToString();
            //    string cellv = Convert.ToString(e.GetValue("SubAccount_MainAcReferenceID"));
            //    e.Cell.Style.Value = "cursor:pointer;color: #000099;text-align:center;";
            //    e.Cell.Attributes.Add("onclick", "ShowCustom()");
            //    //e.Cell.Attributes.Add("onclick", "ShowCustom('" + kv + "','" + Request.QueryString["id"].ToString() + "')");
            //    //e.Cell.Attributes.Add("onclick", "ShowCustom('" + kv + "','" + Convert.ToString(Request.QueryString["id"]) + "')");
            //}
        }

        #region Batch Edit Grid Function
        public class MultiUOMPRConversionPacking
        {
            public decimal packing_quantity { get; set; }
            public decimal sProduct_quantity { get; set; }

            public bool isOverideConvertion { get; set; }

        }
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
            public string Order_Num { get; set; }
            public string ProductName { get; set; }
            public string OrderDetails_Id { get; set; }
            public string Remarks { get; set; }
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
        public DataSet GetProductData()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_ProjectChallan_Details");
            proc.AddVarcharPara("@Action", 500, "ProductDetails");
            ds = proc.GetDataSet();
            return ds;
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
            ProcedureExecute proc = new ProcedureExecute("prc_ProjectChallan_Details");
            proc.AddVarcharPara("@Action", 500, "TaxDetails");
            proc.AddVarcharPara("@Challan_Id", 500, Convert.ToString(Session["ChallanID"]) == "ADD" ? "" : Convert.ToString(Session["ChallanID"]));
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@branchId", 500, Convert.ToString(ddl_Branch.SelectedValue));
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            proc.AddVarcharPara("@S_challanDate", 10, quoteDate);
            dt = proc.GetTable();
            return dt;
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

                Orders.Remarks = Convert.ToString(Orderdt.Rows[i]["Remarks"]);
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
                    Orders.Order_Num = Convert.ToString(SalesOrderdt.Rows[i]["Quotation"]);//subhabrata on 21-02-2017
                }
                Orders.ProductName = Convert.ToString(SalesOrderdt.Rows[i]["ProductName"]);
                Orders.Remarks = Convert.ToString(SalesOrderdt.Rows[i]["Remarks"]);

                OrderList.Add(Orders);
            }

            return OrderList;
        }
        public IEnumerable GetSalesOrderInfo(DataTable SalesOrderdt1, string Order_Id)
        {
            List<SalesOrder> OrderList = new List<SalesOrder>();
            DataColumnCollection dtC = SalesOrderdt1.Columns;
            string commaSeparatedString = String.Join(",", SalesOrderdt1.AsEnumerable().Select(x => x.Field<Int64>("SalesOrder_Id").ToString()).ToArray());
            DataTable tempWarehouse = GetOrderWarehouse(commaSeparatedString);
            DataTable tempProductTax = GetDocumentProductTaxData(commaSeparatedString);
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
                if (!string.IsNullOrEmpty(Convert.ToString(SalesOrderdt1.Rows[i]["SalePrice"])))
                { Orders.SalePrice = Convert.ToString(SalesOrderdt1.Rows[i]["SalePrice"]); }
                else
                { Orders.SalePrice = ""; }
                if (!string.IsNullOrEmpty(Convert.ToString(SalesOrderdt1.Rows[i]["Discount"])))
                { Orders.Discount = Convert.ToString(SalesOrderdt1.Rows[i]["Discount"]); }
                else
                { Orders.Discount = ""; }
                if (!string.IsNullOrEmpty(Convert.ToString(SalesOrderdt1.Rows[i]["Amount"])))
                { Orders.Amount = Convert.ToString(SalesOrderdt1.Rows[i]["Amount"]); }
                else { Orders.Amount = ""; }
                if (!string.IsNullOrEmpty(Convert.ToString(SalesOrderdt1.Rows[i]["TaxAmount"])))
                { Orders.TaxAmount = Convert.ToString(SalesOrderdt1.Rows[i]["TaxAmount"]); }
                else { Orders.TaxAmount = ""; }
                if (!string.IsNullOrEmpty(Convert.ToString(SalesOrderdt1.Rows[i]["TotalAmount"])))
                { Orders.TotalAmount = Convert.ToString(SalesOrderdt1.Rows[i]["TotalAmount"]); }
                else { Orders.TotalAmount = ""; }

                if (!string.IsNullOrEmpty(Convert.ToString(SalesOrderdt1.Rows[i]["Quotation_No"])))//Added on 15-02-2017
                { Orders.Quotation_No = Convert.ToInt64(SalesOrderdt1.Rows[i]["Quotation_No"]); }
                else
                { Orders.Quotation_No = 0; }
                if (dtC.Contains("Quotation"))
                {
                    Orders.Order_Num = Convert.ToString(SalesOrderdt1.Rows[i]["Quotation"]);//subhabrata on 21-02-2017
                }
                Orders.ProductName = Convert.ToString(SalesOrderdt1.Rows[i]["ProductName"]);


                string strQuoteDetails_Id = Convert.ToString(SalesOrderdt1.Rows[i]["SalesOrder_Id"]).Trim();

                if (tempWarehouse != null && tempWarehouse.Rows.Count > 0)
                {
                    var rows = tempWarehouse.Select("OrderDetails_Id ='" + strQuoteDetails_Id + "'");
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

                Orders.Remarks = Convert.ToString(SalesOrderdt1.Rows[i]["Remarks"]);

                OrderList.Add(Orders);


            }

            if (tempWarehouse != null && tempWarehouse.Rows.Count > 0)
            {
                tempWarehouse.Columns.Remove("OrderDetails_Id");
                Session["SC_WarehouseData"] = tempWarehouse;
            }
            else { Session["SC_WarehouseData"] = null; }

            if (tempProductTax != null)
            {
                tempProductTax.Columns.Remove("ProductTax_ProductId");
                Session["ProjectChallanFinalTaxRecord"] = tempProductTax;
            }
            else { Session["ProjectChallanFinalTaxRecord"] = null; }

            BindSessionByDatatable(SalesOrderdt1);

            return OrderList;
        }

        public DataTable GetOrderWarehouse(string strOrderList)
        {
            try
            {
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
                proc.AddVarcharPara("@Action", 500, "ChallanWarehouseByOrder");
                proc.AddVarcharPara("@QuotationList", 3000, strOrderList);
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

        public DataTable GetDocumentProductTaxData(string strQuotationList)
        {
            try
            {
                DataTable dt = new DataTable();

                if (Convert.ToString(Session["Doc_Type"]) == "SI")
                {
                    ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
                    proc.AddVarcharPara("@Action", 500, "OrderProductTaxByInvoice");
                    proc.AddVarcharPara("@QuotationList", 3000, strQuotationList);
                    dt = proc.GetTable();
                }
                else
                {
                    ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
                    proc.AddVarcharPara("@Action", 500, "OrderProductTaxByOrder");
                    proc.AddVarcharPara("@QuotationList", 3000, strQuotationList);
                    dt = proc.GetTable();
                }



                return dt;
            }
            catch
            {
                return null;
            }
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
            SalesChalladt.Columns.Add("Remarks", typeof(string));

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
                               SalePrice, Discount, Amount, TaxAmount, TotalAmount, "U", Convert.ToInt64(dt.Rows[i]["Quotation_No"]), Order_Num1, ProductName, Convert.ToString(dt.Rows[i]["Remarks"]));
            }

            Session["ChallanDetails"] = SalesChalladt;

            return IsSuccess;
        }//End

        public bool BindSessionByDatatableDirect(DataTable dt)
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
                SalesChalladt.Rows.Add(Convert.ToString(i + 1), Convert.ToString(i + 1), Convert.ToString(dt.Rows[i]["ProductID"]), Convert.ToString(dt.Rows[i]["Description"]),
                    Convert.ToString(dt.Rows[i]["Quantity"]), Convert.ToString(dt.Rows[i]["UOM"]), "", Convert.ToString(dt.Rows[i]["StockQuantity"]), Convert.ToString(dt.Rows[i]["UOM"]),
                               SalePrice, Discount, Amount, TaxAmount, TotalAmount, "U", Convert.ToInt64(dt.Rows[i]["Quotation_No"]), Order_Num1, ProductName);
            }

            Session["ChallanDetails"] = SalesChalladt;

            return IsSuccess;
        }//End

        #endregion

        #region Subhabrata/GetProductInfo

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
                Orders.OrderDetails_Id = Convert.ToString(SalesOrderdt1.Rows[i]["SalesOrder_Id"]);
                OrderList.Add(Orders);
            }

            return OrderList;
        }

        #endregion

        public DataSet GetOrderData()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_ProjectChallan_Details");
            proc.AddVarcharPara("@Action", 500, "OrderDetails");
            proc.AddVarcharPara("@Order_Id", 500, Convert.ToString(Session["ChallanID"]));
            proc.AddVarcharPara("@Doct_type", 500, Convert.ToString(Session["Doc_Type"]));
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet GetOrderDataDirect(string BillNo)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_ProjectChallan_Details");
            proc.AddVarcharPara("@Action", 500, "SalesInvoiceDetailsDirect");
            proc.AddVarcharPara("@Order_Id", 500, Convert.ToString(BillNo));
            ds = proc.GetDataSet();
            return ds;
        }

        protected void grid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "Description")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "UOM")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "StkUOM")
            {
                e.Editor.Enabled = true;
            }

            else if (e.Column.FieldName == "Amount")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "TotalAmount")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "SrlNo")
            {
                e.Editor.Enabled = true;
            }
            else
            {
                e.Editor.ReadOnly = false;
            }
        }
        protected void grid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            DataTable SalesOrderdt = new DataTable();
            string IsDeleteFrom = Convert.ToString(hdfIsDelete.Value);
            string ProductisInventoryDetails = string.Empty;
            if (Session["ChallanDetails"] != null)
            {
                //subhabrata: foreach@deleted extra column 
                DataTable dt = new DataTable();
                DataTable dtTemp = new DataTable();

                dtTemp = (DataTable)Session["ChallanDetails"];
                dt = dtTemp.Copy();
                foreach (DataRow row in dt.Rows)
                {
                    DataColumnCollection dtC = dt.Columns;

                    if (dtC.Contains("Quotation"))
                    { dt.Columns.Remove("Quotation"); }
                    break;
                }//End

                //SalesOrderdt = (DataTable)Session["OrderDetails"];

                SalesOrderdt = dt;
            }
            else
            {
                SalesOrderdt.Columns.Add("SrlNo", typeof(string));
                SalesOrderdt.Columns.Add("OrderID", typeof(string));
                SalesOrderdt.Columns.Add("ProductID", typeof(string));
                SalesOrderdt.Columns.Add("Description", typeof(string));
                //SalesOrderdt.Columns.Add("Quotation", typeof(string));//Added By:subhabrata on 21-02-2017               
                SalesOrderdt.Columns.Add("Quantity", typeof(string));
                SalesOrderdt.Columns.Add("UOM", typeof(string));
                SalesOrderdt.Columns.Add("Warehouse", typeof(string));
                SalesOrderdt.Columns.Add("StockQuantity", typeof(string));
                SalesOrderdt.Columns.Add("StockUOM", typeof(string));
                SalesOrderdt.Columns.Add("SalePrice", typeof(string));
                SalesOrderdt.Columns.Add("Discount", typeof(string));
                SalesOrderdt.Columns.Add("Amount", typeof(string));
                SalesOrderdt.Columns.Add("TaxAmount", typeof(string));
                SalesOrderdt.Columns.Add("TotalAmount", typeof(string));
                SalesOrderdt.Columns.Add("Status", typeof(string));
                SalesOrderdt.Columns.Add("Quotation_No", typeof(string));
                SalesOrderdt.Columns.Add("ProductName", typeof(string));
                SalesOrderdt.Columns.Add("Remarks", typeof(string));
            }
            int InitVal = 1;
            string ProductNameVar = string.Empty;
            foreach (var args in e.InsertValues)
            {
                string ProductDetails = Convert.ToString(args.NewValues["ProductID"]);
                ProductisInventoryDetails = Convert.ToString(args.NewValues["ProductID"]);
                if (ProductDetails != "" && ProductDetails != "0")
                {
                    string SrlNo = Convert.ToString(args.NewValues["SrlNo"]);
                    string[] ProductDetailsList = ProductDetails.Split(new string[] { "||@||" }, StringSplitOptions.None);
                    string ProductID = ProductDetailsList[0];

                    string ProductName = Convert.ToString(args.NewValues["ProductName"]);
                    ProductNameVar = ProductName;
                    string Description = Convert.ToString(args.NewValues["Description"]);
                    string Quantity = Convert.ToString(args.NewValues["Quantity"]);
                    string UOM = Convert.ToString(args.NewValues["UOM"]);
                    string Warehouse = Convert.ToString(args.NewValues["Warehouse"]);
                    string StockQuantity = Convert.ToString(args.NewValues["StockQuantity"]) == "" ? "0" : Convert.ToString(args.NewValues["StockQuantity"]);
                    string StockUOM = Convert.ToString(args.NewValues["StockUOM"]);
                    string SalePrice = Convert.ToString(args.NewValues["SalePrice"]);
                    string Discount = Convert.ToString(args.NewValues["Discount"]);
                    string Amount = (Convert.ToString(args.NewValues["Amount"]) != "") ? Convert.ToString(args.NewValues["Amount"]) : "0";
                    string TaxAmount = (Convert.ToString(args.NewValues["TaxAmount"]) != "") ? Convert.ToString(args.NewValues["TaxAmount"]) : "0";
                    string TotalAmount = (Convert.ToString(args.NewValues["TotalAmount"]) != "") ? Convert.ToString(args.NewValues["TotalAmount"]) : "0";
                    string QuotationNumber = (Convert.ToString(args.NewValues["Doc_Number"]) != "") ? Convert.ToString(args.NewValues["Doc_Number"]) : "0";
                    string Remarks = (Convert.ToString(args.NewValues["Remarks"]) != "") ? Convert.ToString(args.NewValues["Remarks"]) : "";

                    //string Quotation = Convert.ToString(args.NewValues["Quotation_Num"]);//Added By:Subhabrata on 21-02-2017

                    SalesOrderdt.Rows.Add(SrlNo, Convert.ToString(InitVal), ProductDetails, Description, Quantity, UOM, Warehouse, StockQuantity, StockUOM, SalePrice, Discount, Amount, TaxAmount, TotalAmount, "I", QuotationNumber, ProductName, Remarks);
                    InitVal = InitVal + 1;
                }
            }

            foreach (var args in e.UpdateValues)
            {
                string SrlNo = Convert.ToString(args.NewValues["SrlNo"]);
                string OrderID = Convert.ToString(args.Keys["OrderID"]);
                //Session["ChallanID"] = OrderID;
                string ProductDetails = Convert.ToString(args.NewValues["ProductID"]);
                ProductisInventoryDetails = Convert.ToString(args.NewValues["ProductID"]);

                bool isDeleted = false;
                foreach (var arg in e.DeleteValues)
                {
                    string DeleteID = Convert.ToString(arg.Keys["OrderID"]);
                    if (DeleteID == OrderID)
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
                        ProductNameVar = ProductName;
                        string Description = Convert.ToString(args.NewValues["Description"]);
                        string Quantity = Convert.ToString(args.NewValues["Quantity"]);
                        string UOM = Convert.ToString(args.NewValues["UOM"]);
                        string Warehouse = Convert.ToString(args.NewValues["Warehouse"]);
                        string StockQuantity = Convert.ToString(args.NewValues["StockQuantity"]);
                        string StockUOM = Convert.ToString(args.NewValues["StockUOM"]);
                        string SalePrice = (Convert.ToString(args.NewValues["SalePrice"]) != "") ? Convert.ToString(args.NewValues["SalePrice"]) : "0";
                        string Discount = (Convert.ToString(args.NewValues["Discount"]) != "") ? Convert.ToString(args.NewValues["Discount"]) : "0";
                        string Amount = (Convert.ToString(args.NewValues["Amount"]) != "") ? Convert.ToString(args.NewValues["Amount"]) : "0";
                        string TaxAmount = (Convert.ToString(args.NewValues["TaxAmount"]) != "") ? Convert.ToString(args.NewValues["TaxAmount"]) : "0";
                        string TotalAmount = (Convert.ToString(args.NewValues["TotalAmount"]) != "") ? Convert.ToString(args.NewValues["TotalAmount"]) : "0";
                        string QuotationNumber = (Convert.ToString(args.NewValues["Quotation_No"]) != "") ? Convert.ToString(args.NewValues["Quotation_No"]) : "0";
                        string Remarks = (Convert.ToString(args.NewValues["Remarks"]) != "") ? Convert.ToString(args.NewValues["Remarks"]) : "";

                        //string Quotation = Convert.ToString(args.NewValues["Quotation_Num"]);//Added By:Subhabrata on 21-02-2017

                        bool Isexists = false;
                        foreach (DataRow drr in SalesOrderdt.Rows)
                        {
                            string OldOrderID = Convert.ToString(drr["OrderID"]);

                            if (OldOrderID == OrderID)
                            {
                                Isexists = true;

                                drr["ProductName"] = ProductName;
                                drr["ProductID"] = ProductDetails;
                                drr["Description"] = Description;
                                drr["Quantity"] = Quantity;
                                drr["UOM"] = UOM;
                                drr["Warehouse"] = Warehouse;
                                drr["StockQuantity"] = StockQuantity;
                                drr["StockUOM"] = StockUOM;
                                drr["SalePrice"] = SalePrice;
                                drr["Discount"] = Discount;
                                drr["Amount"] = Amount;
                                drr["TaxAmount"] = TaxAmount;
                                drr["TotalAmount"] = TotalAmount;
                                drr["Status"] = "U";
                                if (!string.IsNullOrEmpty(QuotationNumber))
                                { drr["Quotation_No"] = QuotationNumber; }
                                drr["ProductName"] = ProductName;
                                drr["Remarks"] = Remarks;

                                //else
                                //{ drr["Quotation_No"] = ""; }
                                //if (!string.IsNullOrEmpty(Quotation))
                                //{
                                //    drr["Quotation"] = Quotation;
                                //}
                                //else
                                //{
                                //    drr["Quotation"] = "N/A";
                                //}

                                break;
                            }
                        }

                        if (Isexists == false)
                        {
                            SalesOrderdt.Rows.Add(SrlNo, OrderID, ProductDetails, Description, Quantity, UOM, Warehouse, StockQuantity, StockUOM, SalePrice, Discount, Amount, TaxAmount, TotalAmount, "U", QuotationNumber, ProductName, Remarks);
                            //}
                            //SalesOrderdt.Rows.Add(SrlNo, OrderID, ProductDetails, Description, Quotation, Quantity, UOM, Warehouse, StockQuantity, StockUOM, SalePrice, Discount, Amount, TaxAmount, TotalAmount, "U", QuotationNumber);
                        }
                    }
                }
            }

            foreach (var args in e.DeleteValues)
            {
                string OrderID = Convert.ToString(args.Keys["OrderID"]);

                for (int i = SalesOrderdt.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = SalesOrderdt.Rows[i];
                    string delQuotationID = Convert.ToString(dr["OrderID"]);

                    if (delQuotationID == OrderID)
                        dr.Delete();
                }
                SalesOrderdt.AcceptChanges();

                if (OrderID.Contains("~") != true)
                {
                    SalesOrderdt.Rows.Add("0", OrderID, "0", "", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "D", "0", "0");
                }
            }

            ///////////////////////

            if (IsDeleteFrom == "D")
            {
                int j = 1;
                foreach (DataRow dr in SalesOrderdt.Rows)
                {
                    string Status = Convert.ToString(dr["Status"]);
                    dr["SrlNo"] = j.ToString();

                    if (Status != "D")
                    {
                        if (Status == "I" && IsDeleteFrom == "D")
                        {
                            string strID = Convert.ToString("Q~" + j);
                            dr["OrderID"] = strID;
                        }
                        j++;
                    }
                }
                SalesOrderdt.AcceptChanges();
            }
            DataTable dtSession = SalesOrderdt.Copy();
            Session["ChallanDetails"] = dtSession;
            //////////////////////


            if (IsDeleteFrom != "D")
            {
                try
                {
                    string ActionType = Convert.ToString(Session["ActionType"]);
                    string MainOrderID = string.Empty;
                    if (Convert.ToString(Request.QueryString["key"]) != null && Convert.ToString(Request.QueryString["key"]) != "" &&
                        string.IsNullOrEmpty(Request.QueryString["CustID"]))
                    {

                        Session["ChallanID"] = Convert.ToString(Request.QueryString["key"]);
                        if (!string.IsNullOrEmpty(Convert.ToString(Session["ChallanID"])))
                        {

                            MainOrderID = Convert.ToString(Session["ChallanID"]);
                        }
                        else
                        {
                            MainOrderID = "";
                        }

                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(Session["ChallanID"])))
                        {

                            MainOrderID = Convert.ToString(Session["ChallanID"]);
                        }
                        else
                        {
                            MainOrderID = "";
                        }

                    }
                    string strIsInventory = string.Empty;

                    string[] ProductDetailsList = ProductisInventoryDetails.Split(new string[] { "||@||" }, StringSplitOptions.None);


                    string creditdays = txtCreditDays.Text;
                    string strDueDate = Convert.ToString(dt_SaleInvoiceDue.Date);

                    string strEwayBillNo = Convert.ToString(txt_EWayBillNO.Text);
                    string strCustomerDueDays = Convert.ToString(dt_SaleInvoiceDue.Date);
                    string Doc_Type = Convert.ToString(Session["Doc_Type"]);//Added on 30
                    string strSchemeType = Convert.ToString(ddl_numberingScheme.SelectedValue);
                    string strQuoteNo = Convert.ToString(txt_SlChallanNo.Text);
                    string strQuoteDate = Convert.ToString(dt_PLSales.Date);
                    string strQuoteExpiry = Convert.ToString(dt_PlOrderExpiry.Date);
                    string strCustomer = Convert.ToString(hdfLookupCustomer.Value);
                    string strContactName = Convert.ToString(cmbContactPerson.Value);
                    string Reference = Convert.ToString(txt_Refference.Text);
                    string strBranch = Convert.ToString(ddl_Branch.SelectedValue);
                    //string strAgents = Convert.ToString(ddl_SalesAgent.SelectedValue);//Subhabrata On 28-12-2017
                    string strAgents = string.Empty;
                    if (!string.IsNullOrEmpty(Convert.ToString(hdnSalesManAgentId.Value)))
                    { strAgents = Convert.ToString(hdnSalesManAgentId.Value); }
                    else
                    {
                        strAgents = "0";
                    }

                    string strCurrency = Convert.ToString(ddl_Currency.SelectedValue);
                    //string strRate = Convert.ToString(txt_Rate.Value);
                    string strTaxType = Convert.ToString(ddl_AmountAre.Value);
                    string strTaxCode = string.Empty;
                    if (Convert.ToString(ddl_VatGstCst.Value) != "0~0~X")
                    { strTaxCode = Convert.ToString(ddl_VatGstCst.Value); }

                    //Added by:Subhabrata
                    string OANumber = Convert.ToString(txt_OANumber.Text);
                    string OADate = Convert.ToString(dt_OADate.Date);
                    //  string QuotationDate = Convert.ToString(dt_Quotation.Text);
                    string QuotationDate = string.Empty;
                    //Get Quotation details
                    String QuoComponent = "";
                    List<object> QuoList = lookup_order.GridView.GetSelectedFieldValues("Order_Id");
                    foreach (object Quo in QuoList)
                    {
                        QuoComponent += "," + Quo;
                    }
                    QuoComponent = QuoComponent.TrimStart(',');
                    string[] eachQuo = QuoComponent.Split(',');
                    if (eachQuo.Length == 1)
                    {
                        QuotationDate = Convert.ToString(dt_Quotation.Text);
                        //  strQuoteDate = Convert.ToString(dt_Quotation.Text);
                        // dt_Quotation.Text = "Multiple Select Quotation Dates";
                        // lbl_MultipleDate.Attributes.Add("style", "display:block");
                    }
                    else
                    {
                        //  lbl_MultipleDate.Attributes.Add("style","display:none"); 
                    }
                    // string QuotationNumber = Convert.ToString(ddl_Quotation.Value);
                    //End   
                    string strRate = "0";
                    string CompID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                    string currency = Convert.ToString(HttpContext.Current.Session["ActiveCurrency"]);
                    string[] ActCurrency = currency.Split('~');
                    int BaseCurrencyId = Convert.ToInt32(ActCurrency[0]);
                    int ConvertedCurrencyId = Convert.ToInt32(strCurrency);
                    ProjectActivitiesBL objProjectActivitiesBL = new ProjectActivitiesBL();
                    DataTable dt = objProjectActivitiesBL.GetCurrentConvertedRate(BaseCurrencyId, ConvertedCurrencyId, CompID);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        strRate = Convert.ToString(dt.Rows[0]["SalesRate"]);
                    }
                    int InitValR = 1;
                    foreach (DataRow dr in SalesOrderdt.Rows)
                    {

                        string Status = Convert.ToString(dr["Status"]);

                        if (Status == "I")
                        {
                            dr["OrderID"] = Convert.ToString(InitValR);

                            string[] list = Convert.ToString(dr["ProductID"]).Split(new string[] { "||@||" }, StringSplitOptions.None);
                            dr["ProductID"] = Convert.ToString(list[0]);
                            if (list.Count() > 1)
                            {
                                dr["UOM"] = Convert.ToString(list[3]);
                                dr["StockUOM"] = Convert.ToString(list[5]);
                            }

                        }
                        else if (Status == "U" || Status == "")
                        {
                            string[] list = Convert.ToString(dr["ProductID"]).Split(new string[] { "||@||" }, StringSplitOptions.None);
                            dr["ProductID"] = Convert.ToString(list[0]);
                            if (list.Count() > 1)
                            {
                                dr["UOM"] = Convert.ToString(list[3]);
                                dr["StockUOM"] = Convert.ToString(list[5]);
                            }
                            else
                            {
                                string UOM = Convert.ToString(dr["UOM"]);
                                string stockUOM = Convert.ToString(dr["StockUOM"]);
                                DataSet dtUOMs = new DataSet();

                                if (!String.IsNullOrEmpty(UOM) && !String.IsNullOrEmpty(stockUOM))
                                {
                                    dtUOMs = objProjectActivitiesBL.GetQuotationDetailsUOMInfo(UOM, stockUOM);
                                    dr["UOM"] = Convert.ToString(dtUOMs.Tables[0].Rows[0].Field<Int64>("UOM_ID"));
                                    dr["StockUOM"] = Convert.ToString(dtUOMs.Tables[1].Rows[0].Field<Int64>("UOM_ID"));
                                }

                            }
                        }
                        InitValR = InitValR + 1;
                    }
                    SalesOrderdt.AcceptChanges();


                    DataTable TaxDetailTable = new DataTable();
                    if (Session["ProjectChallanFinalTaxRecord"] != null)
                    {
                        TaxDetailTable = (DataTable)Session["ProjectChallanFinalTaxRecord"];
                    }
                    else
                    {
                        TaxDetailTable.Columns.Add("SlNo", typeof(string));
                        TaxDetailTable.Columns.Add("TaxCode", typeof(string));
                        TaxDetailTable.Columns.Add("AltTaxCode", typeof(string));
                        TaxDetailTable.Columns.Add("Percentage", typeof(string));
                        TaxDetailTable.Columns.Add("Amount", typeof(string));
                    }

                    DataTable TaxDetailsdt = new DataTable();
                    if (Session["ProjectChallanTaxDetails"] != null)
                    {
                        TaxDetailsdt = (DataTable)Session["ProjectChallanTaxDetails"];
                    }
                    else
                    {
                        TaxDetailsdt.Columns.Add("Taxes_ID", typeof(string));
                        TaxDetailsdt.Columns.Add("Taxes_Name", typeof(string));
                        TaxDetailsdt.Columns.Add("Percentage", typeof(string));
                        TaxDetailsdt.Columns.Add("Amount", typeof(string));
                        TaxDetailsdt.Columns.Add("AltTax_Code", typeof(string));
                    }

                    DataTable tempTaxDetailsdt = new DataTable();
                    tempTaxDetailsdt = TaxDetailsdt.DefaultView.ToTable(false, "Taxes_ID", "Percentage", "Amount", "AltTax_Code");

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

                    // End



                    string validate = string.Empty;
                    string QtyExistsOrNot = string.Empty;
                    string ProductExistsOrNot = string.Empty;
                    string StockCheck = string.Empty;
                    string StockCheckMsg = string.Empty;
                    // Datattable of Warehouse
                    DataTable tempWarehousedt = new DataTable();
                    if (Session["SC_WarehouseData"] != null)
                    {
                        DataTable Warehousedt = (DataTable)Session["SC_WarehouseData"];
                        tempWarehousedt = Warehousedt.DefaultView.ToTable(false, "Product_SrlNo", "LoopID", "WarehouseID", "Quantity", "BatchID", "SerialID", "AltQty", "AltUOM");
                    }
                    else
                    {
                        tempWarehousedt.Columns.Add("Product_SrlNo", typeof(string));
                        tempWarehousedt.Columns.Add("SrlNo", typeof(string));
                        tempWarehousedt.Columns.Add("WarehouseID", typeof(string));
                        tempWarehousedt.Columns.Add("TotalQuantity", typeof(string));
                        tempWarehousedt.Columns.Add("BatchID", typeof(string));
                        tempWarehousedt.Columns.Add("SerialID", typeof(string));
                        tempWarehousedt.Columns.Add("AltQty", typeof(string));
                        tempWarehousedt.Columns.Add("AltUOM", typeof(string));
                    }

                    //End

                    // DataTable Of Billing Address
                    #region ##### Added By : Samrat Roy -- to get BillingShipping user control data
                    DataTable tempBillAddress = new DataTable();
                    tempBillAddress = BillingShippingControl.SaveBillingShippingControlData();

                    #region #### Old_Process ####
                    ////// DataTable Of Billing Address

                    ////DataTable tempBillAddress = new DataTable();
                    ////if (Session["ChallanAddressDtl"] != null)
                    ////{
                    ////    tempBillAddress = (DataTable)Session["ChallanAddressDtl"];
                    ////}
                    ////else
                    ////{
                    ////    tempBillAddress = StoreSalesOrderAddressDetail();
                    ////}

                    #endregion

                    #endregion

                    string approveStatus = "";
                    if (Request.QueryString["status"] != null)
                    {
                        approveStatus = Convert.ToString(Request.QueryString["status"]);
                    }
                    // End
                    if (ActionType == "Add")
                    {
                        string[] SchemeList = strSchemeType.Split(new string[] { "~" }, StringSplitOptions.None);

                        if (SchemeList[0] != "")
                        {
                            validate = checkNMakeJVCode(strQuoteNo, Convert.ToInt32(SchemeList[0]));
                        }
                    }
                    if (!string.IsNullOrEmpty(Request.QueryString["CustID"]))
                    {
                        if (Convert.ToString(Request.QueryString["Flag"]) == "PendingDeliveryFlag")//added on 26-06-2017
                        {
                            UniqueQuotation = txt_SlChallanNo.Text;
                        }
                        else
                        {
                            string[] SchemeList = strSchemeType.Split(new string[] { "~" }, StringSplitOptions.None);

                            if (SchemeList[0] != "")
                            {
                                validate = checkNMakeJVCode(strQuoteNo, Convert.ToInt32(SchemeList[0]));
                            }
                        }

                    }


                    DataTable duplicatedt = SalesOrderdt.Copy();
                    var duplicateRecords = duplicatedt.AsEnumerable()
                    .GroupBy(r => r["ProductID"]) //coloumn name which has the duplicate values
                    .Where(gr => gr.Count() > 1)
                    .Select(g => g.Key);

                    foreach (var d in duplicateRecords)
                    {
                        validate = "duplicateProduct";
                    }




                    decimal DocTotalAmount = 0;
                    #region WarehouseCheck
                    //if (strIsInventory != "N")
                    //{
                    foreach (DataRow dr in duplicatedt.Rows)
                    {
                        string IsInventory = getProductIsInventoryExists(Convert.ToString(dr["ProductID"]));
                        if (Convert.ToString(dr["ProductID"]) != "0")
                        {
                            if (IsInventory.ToUpper() != "N")
                            {
                                string strSrlNo = Convert.ToString(dr["SrlNo"]);
                                decimal strProductQuantity = Convert.ToDecimal(dr["Quantity"]);

                                if (tempWarehousedt.Rows.Count == 0 && strProductQuantity > 0)
                                {
                                    validate = "checkWarehouse";
                                    grid.JSProperties["cpProductSrlIDCheck"] = ProductNameVar;
                                    break;
                                }
                            }
                        }

                    }
                    //}
                    #endregion

                    #region EwayBillCheck
                    string EwayBillNo = string.Empty;
                    string EwayMsg = string.Empty;
                    //if (DocTotalAmount > (decimal)50000.00)
                    //{
                    //    EwayBillNo = txt_EWayBillNO.Text;
                    //    if (string.IsNullOrEmpty(EwayBillNo))
                    //    {
                    //        EwayMsg = "ExceedsEway";

                    //    }

                    //}

                    #endregion
                    //if (strIsInventory != "N")
                    //{
                    foreach (DataRow dr in SalesOrderdt.Rows)
                    {
                        string IsInventory = getProductIsInventoryExists(Convert.ToString(dr["ProductID"]));
                        if (Convert.ToString(dr["ProductID"]) != "0")
                        {
                            if (IsInventory.ToUpper() != "N")
                            {
                                string strSrlNo = Convert.ToString(dr["SrlNo"]);
                                decimal strProductQuantity = Convert.ToDecimal(dr["Quantity"]);

                                decimal strWarehouseQuantity = 0;
                                GetQuantityBaseOnProduct(strSrlNo, ref strWarehouseQuantity);

                                //if (strWarehouseQuantity != 0)
                                //{
                                if (strProductQuantity != strWarehouseQuantity)
                                {
                                    validate = "checkWarehouseQty";
                                    grid.JSProperties["cpProductSrlIDCheck1"] = strSrlNo;
                                    break;
                                }
                                //}
                            }
                        }

                    }
                    //}

                    //To Check available Stock:Subhabrata
                    //if (strIsInventory != "N")
                    //{
                    if (MainOrderID.ToUpper() != "ADD")
                    {
                        StockCheckMsg = objBL.GetAvailableStockCheckSalesChallan(duplicatedt, Convert.ToString(ddl_Branch.SelectedValue), Convert.ToString(strQuoteDate), ActionType, Convert.ToInt32(MainOrderID));
                    }
                    else
                    {
                        StockCheckMsg = objBL.GetAvailableStockCheckForOutModules(duplicatedt, Convert.ToString(ddl_Branch.SelectedValue), Convert.ToString(strQuoteDate));
                    }

                    if (!string.IsNullOrEmpty(StockCheckMsg))
                    {
                        validate = StockCheckMsg;
                    }
                    //}
                    //End
                    string ShippingStateCheck = string.Empty;
                    ShippingStateCheck = Convert.ToString(BillingShippingControl.GetShippingStateCode());
                    if (string.IsNullOrEmpty(ShippingStateCheck) || ShippingStateCheck == "0")
                    {
                        validate = "BillingShippingBlank";
                    }


                    //// ############# Added By : Samrat Roy -- 02/05/2017 -- To check Transporter Mandatory Control 
                    if (validate == "outrange" || validate == "duplicateProduct" || validate == "checkWarehouse" || validate == "checkWarehouseQty" || validate == "BillingShippingBlank" || validate == "MoreThanStock")
                    {
                        grid.JSProperties["cpSaveSuccessOrFail"] = validate;
                    }
                    else if (StockCheckMsg == "ZeroStock")
                    {
                        grid.JSProperties["cpProductZeroStock"] = StockCheckMsg;
                    }
                    else if (EwayMsg == "ExceedsEway")
                    {
                        grid.JSProperties["cpProductTotalAmountEway"] = EwayMsg;
                    }
                    else
                    {

                        if (ActionType == "Add")
                        {
                            if (!reCat.isAllMandetoryDone((DataTable)Session["UdfDataOnAdd"], "SC"))
                            {
                                //grid.JSProperties["cpSaveSuccessOrFail"] = "udfNotSaved";
                                //return;

                            }
                        }
                        if (SalesOrderdt.Rows.Count <= 0)
                        {
                            ProductExistsOrNot = "Select Product First";
                            grid.JSProperties["cpProductNotExists"] = ProductExistsOrNot;

                        }
                        else
                        {
                            bool isQtyExists = true;
                            //foreach (DataRow dr in SalesOrderdt.Rows)
                            //{
                            //    if (Convert.ToString(dr["ProductID"]) != "0")
                            //    {
                            //        if (Convert.ToDecimal(dr["Quantity"]) == (decimal)0.00)
                            //        {
                            //            isQtyExists = false;
                            //            QtyExistsOrNot = "QtyNotExists";
                            //        }
                            //    }
                            //}

                            if (isQtyExists == true)
                            {
                                string TaxType = "", ShippingState = "";
                                ShippingState = Convert.ToString(BillingShippingControl.GetShippingStateCode());

                                if (Convert.ToString(ddl_AmountAre.Value) == "1")
                                { TaxType = "E"; }
                                else if (Convert.ToString(ddl_AmountAre.Value) == "2") { TaxType = "I"; }
                                //TaxDetailTable = gstTaxDetails.SetTaxTableDataWithProductSerialRoundOff(ref SalesOrderdt, "SrlNo", "ProductID",
                                //    "Amount", "TaxAmount", TaxDetailTable, "S", dt_PLSales.Date.ToString("yyyy-MM-dd"), strBranch, ShippingState, "I");


                                TaxDetailTable = gstTaxDetails.SetTaxTableDataWithProductSerialWithException(SalesOrderdt, "SrlNo", "ProductID",
                                   "Amount", "TaxAmount", TaxDetailTable, "S", dt_PLSales.Date.ToString("yyyy-MM-dd"), strBranch, ShippingState, "I", hdnCustomerId.Value, "Quantity", "SQ");




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
                                #endregion


                                CommonBL cbl = new CommonBL();
                                string ProjectSelectcashbankModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
                                Int64 ProjId = 0;
                                if (lookup_Project.Text != "")
                                {
                                    string projectCode = lookup_Project.Text;
                                    DataTable dtSlOrd = GetProjectCode(projectCode);
                                    //oDbEngine.GetDataTable("select Proj_Id from Master_ProjectManagement where Proj_Code='" + projectCode + "'");
                                    ProjId = Convert.ToInt64(dtSlOrd.Rows[0]["Proj_Id"]);
                                }
                                else if (lookup_Project.Text == "")
                                {
                                    ProjId = 0;
                                }

                                else
                                {
                                    ProjId = 0;
                                }


                                DataTable dtAddlDesc = (DataTable)Session["InlineRemarks"];
                                int id = ModifyProjectChallan(MainOrderID, strSchemeType, UniqueQuotation, strQuoteDate, strQuoteExpiry, strCustomer, strContactName, ProjId,
                                                   Reference, strBranch, strAgents, strCurrency, strRate, strTaxType, strTaxCode, SalesOrderdt, TaxDetailTable, ActionType, OANumber, OADate, "0", QuotationDate, QuoComponent, tempWarehousedt, tempBillAddress
                                                   , tempTaxDetailsdt, Doc_Type, approveStatus, strCustomerDueDays, strEwayBillNo, creditdays, strDueDate, duplicatedt2, dtAddlDesc);

                                if (id <= 0 && id != -12)
                                {
                                    grid.JSProperties["cpSaveSuccessOrFail"] = "errorInsert";
                                }
                                else if (id == -12)
                                {
                                    grid.JSProperties["cpSaveSuccessOrFail"] = "EmptyProject";
                                }
                                else
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
                                    //####### Coded By Samrat Roy For Custom Control Data Process #########
                                    if (!string.IsNullOrEmpty(hfControlData.Value))
                                    {
                                        CommonBL objCommonBL = new CommonBL();
                                        //objCommonBL.InsertTransporterControlDetails(id, "SC", hfControlData.Value, Convert.ToString(HttpContext.Current.Session["userid"]));
                                        objCommonBL.InsertSalesChallanDetails(id, "SC", hfControlData.Value, Convert.ToString(HttpContext.Current.Session["userid"]));
                                    }

                                    //Udf Add mode
                                    DataTable udfTable = (DataTable)Session["UdfDataOnAdd"];
                                    if (udfTable != null)
                                        Session["UdfDataOnAdd"] = reCat.insertRemarksCategoryAddMode("SC", "SalesChallan" + Convert.ToString(id), udfTable, Convert.ToString(Session["userid"]));

                                    grid.JSProperties["cpSalesOrderNo"] = UniqueQuotation;

                                    if (Convert.ToString(Request.QueryString["Flag"]) == "CustomerDeliveryFlag")
                                    {
                                        int Bill_Id = Convert.ToInt32(Request.QueryString["key"]);
                                        grid.JSProperties["cpDocumentNo"] = Bill_Id;
                                    }
                                    //Added:Subhabrat
                                    if (Session["ChallanDetails"] != null)
                                    {
                                        Session["ChallanDetails"] = null;
                                        //  Session.Remove("OrderDetails");
                                    }

                                    //End
                                }
                            }
                            else
                            {
                                grid.JSProperties["cpIsQtyNotExists"] = QtyExistsOrNot;


                            }

                        }

                        //if (Session["ChallanDetails"] != null)
                        //{
                        //    Session["ChallanDetails"] = null;
                        //    //  Session.Remove("OrderDetails");
                        //}
                        if (!string.IsNullOrEmpty(Request.QueryString["CustID"]))
                        {

                            if (Convert.ToString(Request.QueryString["Flag"]) == "CustomerDeliveryFlag")
                            {
                                grid.JSProperties["cpSalesOrderExitOnCustomerDelivery"] = "CustomerDelivery";
                            }
                            else if (Convert.ToString(Request.QueryString["Flag"]) == "PendingDeliveryFlag")
                            {
                                grid.JSProperties["cpSalesOrderExitOnPendingDelivery"] = "PendingDelivery";
                            }


                        }




                    }
                }

                catch { }
            }
            else
            {
                DataView dvData = new DataView(SalesOrderdt);
                dvData.RowFilter = "Status <> 'D'";

                grid.DataSource = GetSalesOrder(dvData.ToTable());
                grid.DataBind();
            }
        }
        protected void grid_DataBinding(object sender, EventArgs e)
        {
            if (Session["ChallanDetails"] != null)
            {
                DataTable Quotationdt = (DataTable)Session["ChallanDetails"];
                DataView dvData = new DataView(Quotationdt);
                dvData.RowFilter = "Status <> 'D'";
                grid.DataSource = GetSalesOrder(dvData.ToTable());
            }
        }


        protected void EntityServerModeDataProjectChallan_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Proj_Id";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            BusinessLogicLayer.DBEngine BEngine = new BusinessLogicLayer.DBEngine();




            var q = from d in dc.V_ProjectLists
                    where d.ProjectStatus == "Approved" && d.ProjBracnchid == Convert.ToInt64(ddl_Branch.SelectedValue) && d.CustId == hdnCustomerId.Value
                    orderby d.Proj_Id descending
                    select d;

            e.QueryableSource = q;

        }


        protected void acbpCrpUdf_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

            if (Request.QueryString["key"] == "ADD")
            {
                if (reCat.isAllMandetoryDone((DataTable)Session["UdfDataOnAdd"], "SC") == false)
                {
                    acbpCrpUdf.JSProperties["cpUDF"] = "false";

                }
                else
                {
                    acbpCrpUdf.JSProperties["cpUDF"] = "true";
                }


                acbpCrpUdf.JSProperties["cpTransport"] = "true";
                acbpCrpUdf.JSProperties["cpTC"] = "true";

                // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                DataTable DT = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='Transporter_SCMandatory' AND IsActive=1");
                if (DT != null && DT.Rows.Count > 0)
                {
                    string IsMandatory = Convert.ToString(DT.Rows[0]["Variable_Value"]).Trim();
                    //objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                    //DataTable DTVisible = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='Show_Transporter' AND IsActive=1");

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

                //----------Start-------------------------
                //Data: 01-06-2017 Author: Sayan Dutta
                //Details:To check T&C Mandatory Control
                #region TC
                DataTable DT_TC = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='TC_SCMandatory' AND IsActive=1");
                if (DT_TC != null && DT_TC.Rows.Count > 0)
                {
                    string IsMandatory = Convert.ToString(DT_TC.Rows[0]["Variable_Value"]).Trim();

                    //  objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                    objEngine = new BusinessLogicLayer.DBEngine();


                    DataTable DTVisible = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='Show_TC_SC' AND IsActive=1");
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
            else
            {
                acbpCrpUdf.JSProperties["cpUDF"] = "true";
                acbpCrpUdf.JSProperties["cpTransport"] = "true";
                acbpCrpUdf.JSProperties["cpTC"] = "true";

                DataTable DT = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='Transporter_SCMandatory' AND IsActive=1");
                if (DT != null && DT.Rows.Count > 0)
                {
                    string IsMandatory = Convert.ToString(DT.Rows[0]["Variable_Value"]).Trim();
                    //objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                    //DataTable DTVisible = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='Show_Transporter' AND IsActive=1");
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
                //Data: 31-05-2017 Author: Sayan Dutta
                //Details:To check T&C Mandatory Control
                #region TC
                DataTable DT_TC = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='TC_SCMandatory' AND IsActive=1");
                if (DT_TC != null && DT_TC.Rows.Count > 0)
                {
                    string IsMandatory = Convert.ToString(DT_TC.Rows[0]["Variable_Value"]).Trim();

                    // objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                    objEngine = new BusinessLogicLayer.DBEngine();

                    DataTable DTVisible = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='Show_TC_SC' AND IsActive=1");
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

        //SUBHABRATA
        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];
            if (strSplitCommand == "Display")
            {
                string IsDeleteFrom = Convert.ToString(hdfIsDelete.Value);
                if (IsDeleteFrom == "D")
                {
                    DataTable Quotationdt = (DataTable)Session["ChallanDetails"];
                    DataView dvData = new DataView(Quotationdt);
                    dvData.RowFilter = "Status <> 'D'";
                    grid.DataSource = GetSalesOrder(dvData.ToTable());
                    grid.DataBind();
                }
            }
            else if (strSplitCommand == "GridBlank")
            {
                Session["ChallanDetails"] = null;
                grid.DataSource = null;
                grid.DataBind();
            }
            if (strSplitCommand == "BindGridOnQuotation")
            {
                ASPxGridView gv = sender as ASPxGridView;
                string command = e.Parameters.ToString();
                string State = Convert.ToString(e.Parameters.Split('~')[1]);
                String QuoComponent1 = "";
                string Product_id1 = "";
                string QuoteDetails_Id = "";
                for (int i = 0; i < grid_Products.GetSelectedFieldValues("Quotation_No").Count; i++)
                {

                    QuoComponent1 += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("Quotation_No")[i]);
                    Product_id1 += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("ProductID")[i]);
                    QuoteDetails_Id += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("OrderDetails_Id")[i]);
                }
                QuoComponent1 = QuoComponent1.TrimStart(',');
                Product_id1 = Product_id1.TrimStart(',');
                string Quote_Nos = Convert.ToString(e.Parameters.Split('~')[1]);

                Session["Doc_Type"] = Convert.ToString(e.Parameters.Split('~')[2]);

                if (Quote_Nos != "$")
                {

                    DataSet dt_QuotationDetails = new DataSet();
                    string IdKey = Convert.ToString(Request.QueryString["key"]);
                    if (!string.IsNullOrEmpty(IdKey))
                    {
                        if (IdKey != "ADD")
                        {
                            if (e.Parameters.Split('~')[2] == "SO")
                            {
                                dt_QuotationDetails = objProjectActivitiesBL.GetSalesOrderDetailsFromProjectChallan(QuoComponent1, QuoteDetails_Id, Product_id1, "Edit");
                            }
                            else if (e.Parameters.Split('~')[2] == "SI")
                            {
                                dt_QuotationDetails = objProjectActivitiesBL.GetSalesInvoiceFromProjectChallan(QuoComponent1, QuoteDetails_Id, Product_id1, "Edit");
                            }

                        }
                        else
                        {
                            if (e.Parameters.Split('~')[2] == "SO")
                            {
                                dt_QuotationDetails = objProjectActivitiesBL.GetSalesOrderDetailsFromProjectChallan(QuoComponent1, QuoteDetails_Id, Product_id1, "Add");
                            }
                            else if (e.Parameters.Split('~')[2] == "SI")
                            {
                                dt_QuotationDetails = objProjectActivitiesBL.GetSalesInvoiceFromProjectChallan(QuoComponent1, QuoteDetails_Id, Product_id1, "Add");
                            }
                        }

                    }
                    else
                    {
                        dt_QuotationDetails = objProjectActivitiesBL.GetSalesOrderDetailsFromProjectChallan(QuoComponent1, QuoteDetails_Id, Product_id1, "");
                    }
                    Session["ChallanDetails"] = null;
                    grid.DataSource = GetSalesOrderInfo(dt_QuotationDetails.Tables[0], IdKey);
                    Session["InlineRemarks"] = dt_QuotationDetails.Tables[1];
                    grid.DataBind();
                }
                else
                {
                    grid.DataSource = null;
                    grid.DataBind();
                }

                #region ##### Existing BillingShipping Code : ############
                //Session["ChallanAddressDtl"] = GetComponentEditedAddressData(QuoteDetails_Id, "SO");
                //if (Session["ChallanAddressDtl"] != null)
                //{
                //    DataTable dt = (DataTable)Session["ChallanAddressDtl"];
                //    if (dt != null && dt.Rows.Count > 0)
                //    {
                //        if (dt.Rows.Count == 2)
                //        {
                //            if (Convert.ToString(dt.Rows[0]["ChallanAdd_addressType"]) == "Shipping")
                //            {
                //                string countryid = Convert.ToString(dt.Rows[0]["ChallanAdd_countryId"]);
                //                CmbCountry1.Value = countryid;
                //                FillStateCombo(CmbState1, Convert.ToInt32(countryid));
                //                string stateid = Convert.ToString(dt.Rows[0]["ChallanAdd_stateId"]);
                //                CmbState1.Value = stateid;
                //            }
                //            else if (Convert.ToString(dt.Rows[1]["ChallanAdd_addressType"]) == "Shipping")
                //            {
                //                string countryid = Convert.ToString(dt.Rows[0]["ChallanAdd_countryId"]);
                //                CmbCountry1.Value = countryid;
                //                FillStateCombo(CmbState1, Convert.ToInt32(countryid));
                //                string stateid = Convert.ToString(dt.Rows[0]["ChallanAdd_stateId"]);
                //                CmbState1.Value = stateid;
                //            }
                //        }
                //        else if (dt.Rows.Count == 1)
                //        {
                //            if (Convert.ToString(dt.Rows[0]["ChallanAdd_addressType"]) == "Shipping")
                //            {
                //                string countryid = Convert.ToString(dt.Rows[0]["ChallanAdd_countryId"]);
                //                CmbCountry1.Value = countryid;
                //                FillStateCombo(CmbState1, Convert.ToInt32(countryid));
                //                string stateid = Convert.ToString(dt.Rows[0]["ChallanAdd_stateId"]);
                //                CmbState1.Value = stateid;
                //            }

                //        }
                //    }
                //}
                #endregion
            }
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


        public DataTable GetProjectCode(string Proj_Code)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "GetProjectCode");
            proc.AddVarcharPara("@Proj_Code", 200, Proj_Code);
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetProjectEditData()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GetChallanInvoiceDetails");
            proc.AddVarcharPara("@Action", 500, "ProjectEditdata");
            proc.AddVarcharPara("@Order_Id", 500, Convert.ToString(Session["ChallanID"]));
            dt = proc.GetTable();
            return dt;
        }

        public string getProductIsInventoryExists(string ProductId)
        {
            string IsInventory = string.Empty;
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("GetIsInventoryFlagByProductID");
            proc.AddVarcharPara("@ProductId", 500, ProductId);
            dt = proc.GetTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                if (Convert.ToString(dt.Rows[0]["sProduct_IsInventory"]).ToUpper() == "TRUE")
                {
                    IsInventory = "Y";
                }
                else
                {
                    IsInventory = "N";
                }
            }
            return IsInventory;
        }






        [WebMethod]
        public static object SetProjectCode(Int64 OrderId, string TagDocType)
        {
            List<DocumentDetails> Detail = new List<DocumentDetails>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                ProcedureExecute proc = new ProcedureExecute("prc_GetChallanInvoiceDetails");
                proc.AddVarcharPara("@Action", 500, "SalesInvoicetaggingProjectdata");
                proc.AddBigIntegerPara("@Order_Id", OrderId);
                proc.AddVarcharPara("@TagDocType", 500, TagDocType);
                DataTable address = proc.GetTable();



                Detail = (from DataRow dr in address.Rows
                          select new DocumentDetails()

                          {
                              ProjectId = Convert.ToInt64(dr["ProjectId"]),
                              ProjectCode = Convert.ToString(dr["ProjectCode"])
                          }).ToList();
                return Detail;

            }
            return null;

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
                ProjectActivitiesBL objProjectActivitiesBL = new ProjectActivitiesBL();
                DataTable dt = objProjectActivitiesBL.GetCurrentConvertedRate(BaseCurrencyId, ConvertedCurrencyId, CompID);
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

            ProjectActivitiesBL objProjectActivitiesBL = new ProjectActivitiesBL();
            DataTable dt = objProjectActivitiesBL.GetCurrentConvertedRate(Convert.ToInt16(basedCurrency), Convert.ToInt16(Currency_ID), Campany_ID);
            string SalesRate = "";
            if (dt.Rows.Count > 0)
            {
                SalesRate = Convert.ToString(dt.Rows[0]["SalesRate"]);
            }

            return SalesRate;
        }

        [WebMethod]
        public static String GetConfigSettingRights(string VariableName)//Subhabrata on 23-06-2017
        {

            ProjectActivitiesBL objProjectActivitiesBL = new ProjectActivitiesBL();
            DataTable dt = objProjectActivitiesBL.GetConfigSettingsFIFOWise(VariableName);
            string Variable_Val = "";
            string TempVar = "";
            if (dt.Rows.Count > 0)
            {
                TempVar = Convert.ToString(dt.Rows[0]["Variable_Value"]);
                if (TempVar.ToUpper() == "YES")
                {
                    Variable_Val = "1";
                }
                else
                {
                    Variable_Val = "0";
                }
            }

            return Variable_Val;
        }

        [WebMethod]
        public static String GetAvaiableStockCheckStockOut(string ProductID, string FinYear, string Company, string Branch, string Date)
        {
            //  BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

            //DataTable dt = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableQuotation(" + Branch + ",'" + Convert.ToString(Company) + "','" + Convert.ToString(FinYear) + "'," + ProductID + ") as branchopenstock");
            DataTable dt = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableStockSCBOIST(" + Branch + ",'" + Company + "','" + FinYear + "','" + ProductID + "','" + Convert.ToDateTime(Date) + "') as branchopenstock");
            string SalesRate = "Y";

            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (Convert.ToString(Math.Round(Convert.ToDecimal(dt.Rows[0]["branchopenstock"]), 2)) != "0.00")
                    {
                        SalesRate = "N";
                    }
                    else
                    {
                        SalesRate = "Y";
                    }
                }
                else
                {
                    SalesRate = "Y";
                }
            }
            catch
            {

            }

            //if (dt.Rows.Count <= 0)
            //{
            //    SalesRate = "N";
            //}
            //else
            //{
            //    SalesRate = "Y";
            //}

            return SalesRate;
        }

        [WebMethod]
        public static String GetContactSalesManReference(string KeyVal, string type)
        {

            ProjectActivitiesBL objProjectActivitiesBL = new ProjectActivitiesBL();
            DataTable dt = null;
            if (type.ToUpper() == "SO")
            {
                dt = objProjectActivitiesBL.GetConatctReferenceSalesManInfo(KeyVal);
            }
            else if (type.ToUpper() == "SI")
            {
                dt = objProjectActivitiesBL.GetConatctReferenceSalesManInfoInvoice(KeyVal);
            }

            string ResultString = "";
            if (dt.Rows.Count > 0)
            {
                ResultString = Convert.ToString(dt.Rows[0]["Order_Reference"]) + "~" + Convert.ToString(dt.Rows[0]["Order_SalesmanId"]) + "~" + Convert.ToString(dt.Rows[0]["Currency_Id"]) +
                    "~" + Convert.ToString(dt.Rows[0]["Name"]) + "~" + Convert.ToString(dt.Rows[0]["CreditDays"]) + "~" + Convert.ToString(dt.Rows[0]["Due_Date"] + "~" +
                    Convert.ToString(dt.Rows[0]["EWayBillNumber"]));
            }

            return ResultString;
        }
        public int ModifyProjectChallan(string ChallanID, string strSchemeType, string strOrderNo, string strOrderDate, string strOrderExpiry, string strCustomer, string strContactName, Int64 ProjId,
                                    string Reference, string strBranch, string strAgents, string strCurrency, string strRate, string strTaxType, string strTaxCode, DataTable ProjectChallandt,
                                    DataTable TaxDetailTable, string ActionType, string OANumber, string OADate, string QuotationNumber, string QuotationDate, string QuotationIdList, DataTable Warehousedt, DataTable BillAddressdt,
                                    DataTable tempTaxDetailsdt, string Doc_type, string approveStatus, string CustomerDueDate, string EwayBillNo, string CreditDays, string strDueDate, DataTable QuotationPackingDetailsdt, DataTable dtAddlDesc)
        {
            try
            {

                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_CRMProjectChallan", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", ActionType);
                cmd.Parameters.AddWithValue("@Challan_Id", ChallanID);
                if (!string.IsNullOrEmpty(strOrderNo))
                { cmd.Parameters.AddWithValue("@OrderNo", strOrderNo); }

                if (!String.IsNullOrEmpty(strOrderDate))
                    cmd.Parameters.AddWithValue("@OrderDate", Convert.ToDateTime(strOrderDate));
                cmd.Parameters.AddWithValue("@QuoteExpiry", strOrderExpiry);
                cmd.Parameters.AddWithValue("@CustomerID", strCustomer);
                cmd.Parameters.AddWithValue("@ContactPerson", strContactName);
                cmd.Parameters.AddWithValue("@Reference", Reference);
                cmd.Parameters.AddWithValue("@approveStatus", approveStatus);
                cmd.Parameters.AddWithValue("@BranchID", strBranch);
                cmd.Parameters.AddWithValue("@Agents", Convert.ToInt32(strAgents));
                cmd.Parameters.AddWithValue("@Currency", strCurrency);
                cmd.Parameters.AddWithValue("@Rate", Convert.ToDecimal(strRate));
                cmd.Parameters.AddWithValue("@TaxType", strTaxType);
                cmd.Parameters.AddWithValue("@TaxCode", strTaxCode);

                cmd.Parameters.AddWithValue("@CompanyID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FinYear", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(Session["userid"]));
                cmd.Parameters.AddWithValue("@EwayBillNo", Convert.ToString(EwayBillNo));

                cmd.Parameters.AddWithValue("@SalesOrderDetails", ProjectChallandt);
                cmd.Parameters.AddWithValue("@TaxDetail", TaxDetailTable);
                cmd.Parameters.AddWithValue("@ChallanTax", tempTaxDetailsdt);
                cmd.Parameters.AddWithValue("@udt_AddlDesc", dtAddlDesc);
                //if (Convert.ToDateTime(CustomerDueDate) != default(DateTime))
                //{
                //    cmd.Parameters.AddWithValue("@CustomerDueDate", Convert.ToDateTime(CustomerDueDate));
                //}
                cmd.Parameters.AddWithValue("@creditDays", CreditDays);//added on 21-06-2017
                if (Convert.ToDateTime(strDueDate) != default(DateTime))
                {
                    cmd.Parameters.AddWithValue("@CreditDueDate", strDueDate);//added on 21-06-2017
                }


                cmd.Parameters.AddWithValue("@QuotationPackingDetails", QuotationPackingDetailsdt); //Surojit 25-02-2019

                //cmd.Parameters.AddWithValue("@VehicleNo", VehicleNo);
                //cmd.Parameters.AddWithValue("@DriverName", DriverName);
                //cmd.Parameters.AddWithValue("@PhoneNo", PhoneNo);

                //Added by:Subhabrata
                //cmd.Parameters.AddWithValue("@QuotationID", Convert.ToInt64((QuotationNumber)));
                cmd.Parameters.AddWithValue("@Order_OANumber", OANumber);
                if (Convert.ToDateTime(OADate) != default(DateTime))
                { cmd.Parameters.AddWithValue("@Order_OADate", Convert.ToDateTime(OADate)); }

                // cmd.Parameters.AddWithValue("@QuotationDate", QuotationDate);
                if (!String.IsNullOrEmpty(QuotationDate))
                    cmd.Parameters.AddWithValue("@QuotationDate", QuotationDate);
                //   cmd.Parameters.Add("@QuotationDate", SqlDbType.DateTime).Value = Convert.ToDateTime(QuotationDate).ToString("yyyy-MM-dd HH:mm:ss");
                cmd.Parameters.AddWithValue("@Order_Quotation_Ids", QuotationIdList);
                cmd.Parameters.AddWithValue("@WarehouseDetail", Warehousedt);
                cmd.Parameters.AddWithValue("@Numbering_Scheme", strSchemeType);
                //kaushik 24-2-2017
                cmd.Parameters.AddWithValue("@BillAddress", BillAddressdt);
                cmd.Parameters.AddWithValue("@Project_Id", ProjId);
                //SqlParameter outputIdParam = new SqlParameter("@ReturnValue", SqlDbType.VarChar)
                //{
                //    Direction = ParameterDirection.Output
                //};
                cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
                cmd.Parameters.AddWithValue("@Doc_Type", Doc_type);//Added by:Subhabrata on 30-03-2017
                cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
                // cmd.Parameters.AddWithValue("@ReturnValue",  QueryParameterDirection.Output);
                //  cmd.Parameters.Add(outputIdParam);
                //kaushik 24-2-2017
                //End
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                int idFromString = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());

                if (hdnShowUOMConversionInEntry.Value == "1")
                {
                    if (HttpContext.Current.Session["SecondUOMDetails"] != null)
                    {
                        SecondUOMDetailsBL uomBL = new SecondUOMDetailsBL();
                        List<SecondUOMDetails> finalResult = (List<SecondUOMDetails>)HttpContext.Current.Session["SecondUOMDetails"];
                        DataTable dtoutput = uomBL.SaveSencondUOMDetails(finalResult, "SC", "OUT", Convert.ToString(idFromString));
                        HttpContext.Current.Session["SecondUOMDetails"] = null;
                    }

                }

                if (idFromString > 0)
                {
                    //####### Coded By Sayan Dutta For Custom Control Data Process #########
                    if (!string.IsNullOrEmpty(hfTermsConditionData.Value))
                    {
                        TermsConditionsControl.SaveTC(hfTermsConditionData.Value, Convert.ToString(idFromString), "SC");
                    }
                }

                cmd.Dispose();
                con.Dispose();

                return idFromString;
            }
            catch (Exception ex)
            {
                return 0;
            }
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
        protected void gridTax_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];
            if (strSplitCommand == "Display")
            {
                DataTable TaxDetailsdt = new DataTable();
                if (Session["ProjectChallanTaxDetails"] == null)
                {
                    Session["ProjectChallanTaxDetails"] = GetTaxData(dt_PLSales.Date.ToString("yyyy-MM-dd"));
                }

                if (Session["ProjectChallanTaxDetails"] != null)
                {
                    TaxDetailsdt = (DataTable)Session["ProjectChallanTaxDetails"];

                    #region Delete Igst,Cgst,Sgst respectively
                    //Get Company Gstin 09032017
                    string CompInternalId = Convert.ToString(Session["LastCompany"]);
                    string BranchId = Convert.ToString(Session["userbranchID"]);
                    string[] compGstin = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_gstin", "cmp_internalid='" + CompInternalId + "'", 1);
                    string[] branchGstin = oDBEngine.GetFieldValue1("tbl_master_branch", "isnull(branch_GSTIN,'')branch_GSTIN", "branch_id='" + BranchId + "'", 1);
                    String GSTIN = "";
                    if (compGstin.Length > 0)
                    {
                        if (branchGstin[0].Trim() != "")
                        {
                            GSTIN = branchGstin[0].Trim();
                        }
                        else
                        {
                            GSTIN = compGstin[0].Trim();
                        }
                    }

                    string ShippingState = "";

                    #region ##### Added By : Samrat Roy -- For BillingShippingUserControl ######
                    string sstateCode = BillingShippingControl.GetShippingStateCode(Request.QueryString["key"]);
                    ShippingState = sstateCode;
                    if (ShippingState.Trim() != "")
                    {
                        ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
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

                    if (ShippingState.Trim() != "" && GSTIN.Trim() != "")
                    {

                        if (GSTIN.Substring(0, 2) == ShippingState)
                        {
                            //Check if the state is in union territories then only UTGST will apply
                            //   Chandigarh     Andaman and Nicobar Islands    DADRA & NAGAR HAVELI    DAMAN & DIU     Lakshadweep              PONDICHERRY
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
                    //If Company GSTIN is blank then Delete All CGST,UGST,IGST,CGST
                    if (GSTIN.Trim() == "")
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
                    gridTax.JSProperties["cpTotalCharges"] = ClculatedTotalCharge(taxChargeDataSource);
                }
            }
            else if (strSplitCommand == "SaveGst")
            {
                DataTable TaxDetailsdt = new DataTable();
                if (Session["ProjectChallanTaxDetails"] != null)
                {
                    TaxDetailsdt = (DataTable)Session["ProjectChallanTaxDetails"];
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
                        existingRow[0]["Percentage"] = (cmbGstCstVatcharge.Value != null) ? Convert.ToString(cmbGstCstVatcharge.Value).Split('~')[1] : "0";
                        existingRow[0]["Amount"] = txtGstCstVatCharge.Text;
                        existingRow[0]["AltTax_Code"] = (cmbGstCstVatcharge.Value != null) ? Convert.ToString(cmbGstCstVatcharge.Value).Split('~')[0] : "0";

                    }
                    else
                    {
                        string GstTaxId = (cmbGstCstVatcharge.Value != null) ? Convert.ToString(cmbGstCstVatcharge.Value).Split('~')[0] : "0";
                        string GstPerCentage = (cmbGstCstVatcharge.Value != null) ? Convert.ToString(cmbGstCstVatcharge.Value).Split('~')[1] : "0";

                        string GstAmount = txtGstCstVatCharge.Text;
                        DataRow gstRow = TaxDetailsdt.NewRow();
                        gstRow["Taxes_ID"] = 0;
                        gstRow["Taxes_Name"] = cmbGstCstVatcharge.Text;
                        gstRow["Percentage"] = GstPerCentage;
                        gstRow["Amount"] = GstAmount;
                        gstRow["AltTax_Code"] = GstTaxId;
                        TaxDetailsdt.Rows.Add(gstRow);
                    }

                    Session["ProjectChallanTaxDetails"] = TaxDetailsdt;
                }
            }
        }

        protected decimal ClculatedTotalCharge(List<Taxes> taxChargeDataSource)
        {
            decimal totalCharges = 0;
            foreach (Taxes txObj in taxChargeDataSource)
            {

                if (Convert.ToString(txObj.TaxName).Contains("(+)"))
                {
                    totalCharges += Convert.ToDecimal(txObj.Amount);
                }
                else
                {
                    totalCharges -= Convert.ToDecimal(txObj.Amount);
                }

            }
            totalCharges += Convert.ToDecimal(txtGstCstVatCharge.Text);

            return totalCharges;

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
            if (Session["ProjectChallanTaxDetails"] != null)
            {
                TaxDetailsdt = (DataTable)Session["ProjectChallanTaxDetails"];
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

            Session["ProjectChallanTaxDetails"] = TaxDetailsdt;

            gridTax.DataSource = GetTaxes(TaxDetailsdt);
            gridTax.DataBind();
        }
        protected void grid_Products_DataBinding(Object sender, EventArgs e)
        {

            if (Session["psc_ProductDetails"] != null)
            {
                DataTable Quotationdt = (DataTable)Session["psc_ProductDetails"];
                DataView dvData = new DataView(Quotationdt);
                //dvData.RowFilter = "Status <> 'D'";
                grid_Products.DataSource = GetProductsInfo(Quotationdt);
            }
        }
        protected void cgridProducts_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];
            string strEway = string.Empty;
            if (strSplitCommand == "BindProductsDetails")
            {

                string Quote_Nos = Convert.ToString(e.Parameters.Split('~')[1]);

                String OrderComponent = "";
                List<object> QuoList = lookup_order.GridView.GetSelectedFieldValues("Order_Id");

                //////################# Coded By Samrat Roy 27/04/2017 --- to store Order ID 
                //grid_Products.JSProperties["cpDocID"] = Convert.ToString(QuoList[0]);
                //grid_Products.JSProperties["cpDocType"] = Convert.ToString(e.Parameters.Split('~')[2]);

                foreach (object Quo in QuoList)
                {
                    OrderComponent += "," + Quo;
                }
                OrderComponent = OrderComponent.TrimStart(',');

                if (Quote_Nos != "$")
                {
                    //grid.DataSource = null;
                    //grid.DataBind();
                    //string OrderId = Convert.ToString(e.Parameters.Split('~')[2]);

                    //Session["OrderID"] = OrderId;
                    DataTable dt_OrderDetails = new DataTable();
                    string IdKey = Convert.ToString(Request.QueryString["key"]);
                    if (!string.IsNullOrEmpty(IdKey))
                    {
                        if (IdKey != "ADD")
                        {
                            if (e.Parameters.Split('~')[2] == "SO")
                            {
                                dt_OrderDetails = objProjectActivitiesBL.GetSalesOrderDetailsFromProjectChallanonly(OrderComponent, IdKey, "");
                            }
                            else if (e.Parameters.Split('~')[2] == "SI")
                            {
                                dt_OrderDetails = objProjectActivitiesBL.GetSalesInvoiceDetailsFromProjectChallanonly(OrderComponent, IdKey, "");
                                if (dt_OrderDetails != null && dt_OrderDetails.Rows.Count > 0)
                                {
                                    strEway = Convert.ToString(dt_OrderDetails.Rows[0]["EwayBillNo"]);
                                }
                            }
                        }
                        else
                        {
                            if (e.Parameters.Split('~')[2] == "SO")
                            {
                                dt_OrderDetails = objProjectActivitiesBL.GetSalesOrderDetailsFromProjectChallanonly(OrderComponent, "", "");
                            }
                            else if (e.Parameters.Split('~')[2] == "SI")
                            {
                                dt_OrderDetails = objProjectActivitiesBL.GetSalesInvoiceDetailsFromProjectChallanonly(OrderComponent, IdKey, "");
                                if (dt_OrderDetails != null && dt_OrderDetails.Rows.Count > 0)
                                {
                                    strEway = Convert.ToString(dt_OrderDetails.Rows[0]["EwayBillNo"]);
                                }
                            }
                        }

                    }
                    else
                    {
                        dt_OrderDetails = objProjectActivitiesBL.GetSalesOrderDetailsFromProjectChallanonly(OrderComponent, "", "");
                    }
                    //Session["ChallanDetails"] = null;
                    //grid.DataSource = GetSalesOrderInfo(dt_QuotationDetails, IdKey);
                    //grid.DataBind();

                    //if (!string.IsNullOrEmpty(strEway))
                    //{
                    //    grid_Products.JSProperties["cpEwayBill"] = strEway;
                    //}
                    Session["psc_ProductDetails"] = dt_OrderDetails;
                    grid_Products.DataSource = GetProductsInfo(dt_OrderDetails);
                    grid_Products.DataBind();
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
            //proc.AddVarcharPara("@Action", 500, "GetSerialOnFIFOBasis");
            proc.AddVarcharPara("@Order_Id", 500, Convert.ToString(Session["ChallanID"]));
            proc.AddVarcharPara("@ProductID", 500, Convert.ToString(hdfProductID.Value));
            proc.AddVarcharPara("@BatchID", 500, BatchID);
            proc.AddVarcharPara("@WarehouseID", 500, WarehouseID);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            //proc.AddVarcharPara("@branchId", 2000, Convert.ToString(Session["userbranchID"]));
            proc.AddVarcharPara("@branchId", 2000, Convert.ToString(ddl_Branch.SelectedValue));
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            proc.AddVarcharPara("@Doc_Type", 500, "SC");
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetSerialataNew(string WarehouseID, string BatchID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "GetSerialByProductIDWarehouseBatch");
            //proc.AddVarcharPara("@Action", 500, "GetSerialOnFIFOBasis");
            proc.AddVarcharPara("@Order_Id", 500, Convert.ToString(Session["ChallanID"]));
            proc.AddVarcharPara("@ProductID", 500, Convert.ToString(hdfProductID.Value));
            proc.AddVarcharPara("@BatchID", 500, BatchID);
            proc.AddVarcharPara("@WarehouseID", 500, WarehouseID);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            //proc.AddVarcharPara("@branchId", 2000, Convert.ToString(Session["userbranchID"]));
            proc.AddVarcharPara("@branchId", 2000, Convert.ToString(ddl_Branch.SelectedValue));
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            proc.AddVarcharPara("@Doc_Type", 500, "SC");
            proc.AddVarcharPara("@SC_Date", 10, Convert.ToDateTime(dt_PLSales.Value).ToString("yyyy-MM-dd"));
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetSerialataForFifo(string WarehouseID, string BatchID, string Qty)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            //proc.AddVarcharPara("@Action", 500, "GetSerialByProductIDWarehouseBatch");
            proc.AddVarcharPara("@Action", 500, "GetSerialOnFIFOBasis");
            proc.AddVarcharPara("@Order_Id", 500, Convert.ToString(Session["ChallanID"]));
            proc.AddVarcharPara("@ProductID", 500, Convert.ToString(hdfProductID.Value));
            proc.AddVarcharPara("@BatchID", 500, BatchID);
            proc.AddVarcharPara("@WarehouseID", 500, WarehouseID);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            //proc.AddVarcharPara("@branchId", 2000, Convert.ToString(Session["userbranchID"]));
            proc.AddVarcharPara("@branchId", 2000, Convert.ToString(ddl_Branch.SelectedValue));
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            proc.AddVarcharPara("@Row_No", 100, Convert.ToString(Qty));//Added by Subhabrata on 21-06-201
            proc.AddVarcharPara("@Doc_Type", 500, "SC");
            proc.AddVarcharPara("@SC_Date", 10, Convert.ToDateTime(dt_PLSales.Value).ToString("yyyy-MM-dd"));
            dt = proc.GetTable();
            return dt;
        }


        public DataTable GetSerialata1(string WarehouseID, string BatchID, string Qty)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "GetSerialByProductIDWarehouseBatch");
            proc.AddVarcharPara("@Order_Id", 500, Convert.ToString(Session["ChallanID"]));
            proc.AddVarcharPara("@ProductID", 500, Convert.ToString(hdfProductID.Value));
            proc.AddVarcharPara("@BatchID", 500, BatchID);
            proc.AddVarcharPara("@WarehouseID", 500, WarehouseID);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@branchId", 2000, Convert.ToString(Session["userbranchID"]));
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            proc.AddIntegerPara("@Quantity", Convert.ToInt32(Qty));
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
            //proc.AddVarcharPara("@branchId", 2000, Convert.ToString(Session["userbranchID"]));
            proc.AddVarcharPara("@branchId", 2000, Convert.ToString(ddl_Branch.SelectedValue));
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
            //proc.AddVarcharPara("@branchId", 2000, Convert.ToString(Session["userbranchID"]));
            proc.AddVarcharPara("@branchId", 2000, Convert.ToString(ddl_Branch.SelectedValue));
            proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            proc.AddVarcharPara("@Multiwarehouse", 500, Convert.ToString(multiwarehouse));
            dt = proc.GetTable();
            return dt;
        }

        #endregion

        #region Unique Code Generated Section Start

        //Rev Sayantani
        //public string checkNMakeJVCode(string manual_str, int sel_schema_Id)
        //{

        //    //oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        //    oDBEngine = new BusinessLogicLayer.DBEngine();

        //    DataTable dtSchema = new DataTable();
        //    DataTable dtC = new DataTable();
        //    string prefCompCode = string.Empty, sufxCompCode = string.Empty, startNo, paddedStr, sqlQuery = string.Empty;
        //    int EmpCode, prefLen, sufxLen, paddCounter;
        //    bool suppressZero = false;

        //    if (sel_schema_Id > 0)
        //    {
        //        dtSchema = oDBEngine.GetDataTable("tbl_master_idschema", "prefix, suffix, digit, startno, schema_type,suppressZero", "id=" + sel_schema_Id);

        //        int scheme_type = Convert.ToInt32(dtSchema.Rows[0]["schema_type"]);





        //        if (scheme_type != 0)
        //        {
        //            startNo = dtSchema.Rows[0]["startno"].ToString();
        //            paddCounter = Convert.ToInt32(dtSchema.Rows[0]["digit"]);
        //            paddedStr = startNo.PadLeft(paddCounter, '0');
        //            prefCompCode = dtSchema.Rows[0]["prefix"].ToString();
        //            sufxCompCode = dtSchema.Rows[0]["suffix"].ToString();
        //            prefLen = Convert.ToInt32(prefCompCode.Length);
        //            sufxLen = Convert.ToInt32(sufxCompCode.Length);
        //            suppressZero = Convert.ToBoolean(dtSchema.Rows[0]["suppressZero"]);

        //            if (!suppressZero)
        //            {
        //                sqlQuery = "SELECT max(tjv.Challan_Number) FROM tbl_trans_ProjectChallan tjv WHERE dbo.RegexMatch('";
        //                if (prefLen > 0)
        //                    sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
        //                else if (scheme_type == 2)
        //                    sqlQuery += "^";
        //                sqlQuery += "[0-9]{" + paddCounter + "}";
        //                if (sufxLen > 0)
        //                    sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
        //                //sqlQuery += "?$', LTRIM(RTRIM(tjv.Challan_Number))) = 1";
        //                sqlQuery += "?$', LTRIM(RTRIM(tjv.Challan_Number))) = 1 and Challan_Number like '" + prefCompCode + "%'";
        //                if (scheme_type == 2)
        //                    sqlQuery += " AND CONVERT(DATE, CreatedDate) = CONVERT(DATE, GETDATE())";
        //                dtC = oDBEngine.GetDataTable(sqlQuery);
        //            }

        //            else
        //            {

        //                sqlQuery = "SELECT max(tjv.Challan_Number) FROM tbl_trans_ProjectChallan tjv WHERE dbo.RegexMatch('";
        //                if (prefLen > 0)
        //                    sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
        //                sqlQuery += "[0-9]*";
        //                if (sufxLen > 0)
        //                    sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
        //                //sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1";
        //                sqlQuery += "?$', LTRIM(RTRIM(tjv.Challan_Number))) = 1 and Challan_Number like '" + prefCompCode + "%'";
        //                dtC = oDBEngine.GetDataTable(sqlQuery);
        //                //sqlQuery = "SELECT max(tjv.Challan_Number) FROM tbl_trans_ProjectChallan tjv ";
        //                //if (prefLen > 0)
        //                //{
        //                //    sqlQuery += "Where Challan_Number like '" + prefCompCode + "%' ";
        //                //    if (sufxLen > 0)
        //                //        sqlQuery += "and Challan_Number like '%" + sufxCompCode + "'";
        //                //}
        //                //else
        //                //{
        //                //    if (sufxLen > 0)
        //                //        sqlQuery += "Where Challan_Number like '%" + sufxCompCode + "'";
        //                //}

        //                //dtC = oDBEngine.GetDataTable(sqlQuery);
        //            }



        //            if (dtC.Rows[0][0].ToString() == "")
        //            {
        //                sqlQuery = "SELECT max(tjv.Challan_Number) FROM tbl_trans_ProjectChallan tjv WHERE dbo.RegexMatch('";
        //                if (prefLen > 0)
        //                    sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
        //                else if (scheme_type == 2)
        //                    sqlQuery += "^";
        //                sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
        //                if (sufxLen > 0)
        //                    sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
        //                //sqlQuery += "?$', LTRIM(RTRIM(tjv.Challan_Number))) = 1";
        //                sqlQuery += "?$', LTRIM(RTRIM(tjv.Challan_Number))) = 1 and Challan_Number like '" + prefCompCode + "%'";
        //                if (scheme_type == 2)
        //                    sqlQuery += " AND CONVERT(DATE, CreatedDate) = CONVERT(DATE, GETDATE())";
        //                dtC = oDBEngine.GetDataTable(sqlQuery);
        //            }

        //            if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
        //            {
        //                string uccCode = dtC.Rows[0][0].ToString().Trim();
        //                int UCCLen = uccCode.Length;
        //                int decimalPartLen = UCCLen - (prefCompCode.Length + sufxCompCode.Length);
        //                string uccCodeSubstring = uccCode.Substring(prefCompCode.Length, decimalPartLen);
        //                EmpCode = Convert.ToInt32(uccCodeSubstring) + 1;
        //                // out of range journal scheme
        //                if (EmpCode.ToString().Length > paddCounter)
        //                {
        //                    return "outrange";
        //                }
        //                else
        //                {
        //                    if (!suppressZero)
        //                        paddedStr = EmpCode.ToString().PadLeft(paddCounter, '0');
        //                    else
        //                    paddedStr = EmpCode.ToString();
        //                    UniqueQuotation = prefCompCode + paddedStr + sufxCompCode;
        //                    return "ok";
        //                }
        //            }
        //            else
        //            {
        //                if (!suppressZero)
        //                    paddedStr = startNo.PadLeft(paddCounter, '0');

        //                else

        //                    paddedStr = startNo;

        //                UniqueQuotation = prefCompCode + paddedStr + sufxCompCode;
        //                return "ok";
        //            }
        //        }




        //        else
        //        {
        //            sqlQuery = "SELECT Challan_Number FROM tbl_trans_ProjectChallan WHERE Challan_Number LIKE '" + manual_str.Trim() + "'";
        //            dtC = oDBEngine.GetDataTable(sqlQuery);
        //            // duplicate manual entry check
        //            if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
        //            {
        //                return "duplicate";
        //            }

        //            UniqueQuotation = manual_str.Trim();
        //            return "ok";
        //        }
        //    }
        //    else
        //    {
        //        return "noid";
        //    }
        //}
        public string checkNMakeJVCode(string manual_str, int sel_schema_Id)
        {

            //oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            oDBEngine = new BusinessLogicLayer.DBEngine();

            DataTable dtSchema = new DataTable();
            DataTable dtC = new DataTable();
            string prefCompCode = string.Empty, sufxCompCode = string.Empty, startNo, paddedStr, sqlQuery = string.Empty;
            int EmpCode, prefLen, sufxLen, paddCounter;
            bool suppressZero = false;

            if (sel_schema_Id > 0)
            {
                dtSchema = oDBEngine.GetDataTable("tbl_master_idschema", "prefix, suffix, digit, startno, schema_type,suppressZero", "id=" + sel_schema_Id);

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
                    suppressZero = Convert.ToBoolean(dtSchema.Rows[0]["suppressZero"]);

                    if (!suppressZero)
                    {
                        sqlQuery = "SELECT max(tjv.Challan_Number) FROM tbl_trans_SalesChallan tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + paddCounter + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.Challan_Number))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.Challan_Number))) = 1 and Challan_Number like '" + prefCompCode + "%'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, CreatedDate) = CONVERT(DATE, GETDATE())";
                        dtC = oDBEngine.GetDataTable(sqlQuery);
                    }

                    else
                    {


                        //sqlQuery = "SELECT max(tjv.Challan_Number) FROM tbl_trans_SalesChallan tjv WHERE dbo.RegexMatch('";
                        //if (prefLen > 0)
                        //    sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        //sqlQuery += "[0-9]*";
                        //if (sufxLen > 0)
                        //    sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        ////sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.Challan_Number))) = 1 and Challan_Number like '" + prefCompCode + "%'";
                        //dtC = oDBEngine.GetDataTable(sqlQuery);

                        int i = startNo.Length;
                        while (i < paddCounter)
                        {


                            sqlQuery = "SELECT max(tjv.Challan_Number) FROM tbl_trans_SalesChallan tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            sqlQuery += "[0-9]{" + i + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.Challan_Number))) = 1 and Challan_Number like '" + prefCompCode + "%'";

                            if (prefLen == 0 && sufxLen == 0)
                            {
                                sqlQuery += " and LEN(tjv.Challan_Number)=" + i;
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
                            sqlQuery = "SELECT max(tjv.Challan_Number) FROM tbl_trans_SalesChallan tjv WHERE dbo.RegexMatch('";
                            if (prefLen > 0)
                                sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                            sqlQuery += "[0-9]{" + (i - 1) + "}";
                            if (sufxLen > 0)
                                sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                            //sqlQuery += "?$', LTRIM(RTRIM(tjv.Invoice_Number))) = 1";
                            sqlQuery += "?$', LTRIM(RTRIM(tjv.Challan_Number))) = 1 and Challan_Number like '" + prefCompCode + "%'";
                            if (prefLen == 0 && sufxLen == 0)
                            {
                                sqlQuery += " and LEN(tjv.Challan_Number)=" + (i - 1);
                            }
                            dtC = oDBEngine.GetDataTable(sqlQuery);
                        }


                        //sqlQuery = "SELECT max(tjv.Challan_Number) FROM tbl_trans_SalesChallan tjv ";
                        //if (prefLen > 0)
                        //{
                        //    sqlQuery += "Where Challan_Number like '" + prefCompCode + "%' ";
                        //    if (sufxLen > 0)
                        //        sqlQuery += "and Challan_Number like '%" + sufxCompCode + "'";
                        //}
                        //else
                        //{
                        //    if (sufxLen > 0)
                        //        sqlQuery += "Where Challan_Number like '%" + sufxCompCode + "'";
                        //}

                        //dtC = oDBEngine.GetDataTable(sqlQuery);
                    }



                    if (dtC.Rows[0][0].ToString() == "")
                    {
                        sqlQuery = "SELECT max(tjv.Challan_Number) FROM tbl_trans_SalesChallan tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.Challan_Number))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.Challan_Number))) = 1 and Challan_Number like '" + prefCompCode + "%'";
                        if (scheme_type == 2)
                            sqlQuery += " AND CONVERT(DATE, CreatedDate) = CONVERT(DATE, GETDATE())";


                        if (prefLen == 0 && sufxLen == 0)
                        {
                            sqlQuery += " and LEN(tjv.Challan_Number)=" + (paddCounter - 1);
                        }
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
                            if (!suppressZero)
                                paddedStr = EmpCode.ToString().PadLeft(paddCounter, '0');
                            else
                                paddedStr = EmpCode.ToString();
                            UniqueQuotation = prefCompCode + paddedStr + sufxCompCode;
                            return "ok";
                        }
                    }
                    else
                    {
                        if (!suppressZero)
                            paddedStr = startNo.PadLeft(paddCounter, '0');

                        else

                            paddedStr = startNo;

                        UniqueQuotation = prefCompCode + paddedStr + sufxCompCode;
                        return "ok";
                    }
                }




                else
                {
                    sqlQuery = "SELECT Challan_Number FROM tbl_trans_SalesChallan WHERE Challan_Number LIKE '" + manual_str.Trim() + "'";
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
        // End Of Sayantani 23-08-2019
        #endregion Unique Code Generated Section End

        #region Debu
        public decimal SetTotalCharges(DataTable taxTableFinal)
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
            return totalCharges;

        }
        protected void UpdateGstForCharges(string data)
        {
            for (int i = 0; i < cmbGstCstVatcharge.Items.Count; i++)
            {
                if (Convert.ToString(cmbGstCstVatcharge.Items[i].Value).Split('~')[0] == data)
                {
                    cmbGstCstVatcharge.Items[i].Selected = true;
                    break;
                }
            }
        }
        protected DataTable GetTaxDataWithGST(DataTable existing)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_ProjectChallan_Details");
            proc.AddVarcharPara("@Action", 500, "TaxDetailsForGst");
            proc.AddVarcharPara("@Challan_Id", 500, Convert.ToString(Session["ChallanID"]));
            dt = proc.GetTable();
            if (dt.Rows.Count > 0)
            {
                DataRow gstRow = existing.NewRow();
                gstRow["Taxes_ID"] = 0;
                gstRow["Taxes_Name"] = dt.Rows[0]["TaxRatesSchemeName"];
                gstRow["Percentage"] = dt.Rows[0]["ChallanTax_Percentage"];
                gstRow["Amount"] = dt.Rows[0]["ChallanTax_Amount"];
                gstRow["AltTax_Code"] = dt.Rows[0]["Gst"];

                UpdateGstForCharges(Convert.ToString(dt.Rows[0]["Gst"]));
                txtGstCstVatCharge.Value = gstRow["Amount"];
                existing.Rows.Add(gstRow);
            }
            SetTotalCharges(existing);
            return existing;
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
            proc.AddIntegerPara("@branchId", Convert.ToInt32(Session["userbranchID"]));
            DataTable DT = proc.GetTable();
            cmbGstCstVatcharge.DataSource = DT;
            cmbGstCstVatcharge.TextField = "Taxes_Name";
            cmbGstCstVatcharge.ValueField = "Taxes_ID";
            cmbGstCstVatcharge.DataBind();
        }



        protected void gridTax_DataBinding(object sender, EventArgs e)
        {
            if (Session["ProjectChallanTaxDetails"] != null)
            {
                DataTable TaxDetailsdt = (DataTable)Session["ProjectChallanTaxDetails"];

                //gridTax.DataSource = GetTaxes();
                var taxlist = (List<Taxes>)GetTaxes(TaxDetailsdt);
                var taxChargeDataSource = setChargeCalculatedOn(taxlist, TaxDetailsdt);
                gridTax.DataSource = taxChargeDataSource;
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
        protected void taxUpdatePanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

            string performpara = e.Parameter;
            if (performpara.Split('~')[0] == "DelProdbySl")
            {
                DataTable MainTaxDataTable = (DataTable)Session["ProjectChallanFinalTaxRecord"];

                DataRow[] deletedRow = MainTaxDataTable.Select("SlNo=" + performpara.Split('~')[1]);
                if (deletedRow.Length > 0)
                {
                    foreach (DataRow dr in deletedRow)
                    {
                        MainTaxDataTable.Rows.Remove(dr);
                    }

                }

                Session["ProjectChallanFinalTaxRecord"] = MainTaxDataTable;
                GetStock(Convert.ToString(performpara.Split('~')[1]));
                DeleteWarehouse(Convert.ToString(performpara.Split('~')[1]));
                DataTable taxDetails = (DataTable)Session["ProjectChallanTaxDetails"];
                if (taxDetails != null)
                {
                    foreach (DataRow dr in taxDetails.Rows)
                    {
                        dr["Amount"] = "0.00";
                    }
                    Session["ProjectChallanTaxDetails"] = taxDetails;
                }
            }
            else if (performpara.Split('~')[0] == "DeleteAllTax")
            {
                CreateDataTaxTable();

                DataTable taxDetails = (DataTable)Session["ProjectChallanTaxDetails"];

                if (taxDetails != null)
                {
                    foreach (DataRow dr in taxDetails.Rows)
                    {
                        dr["Amount"] = "0.00";
                    }
                    Session["ProjectChallanTaxDetails"] = taxDetails;
                }
            }
            else
            {
                DataTable MainTaxDataTable = (DataTable)Session["ProjectChallanFinalTaxRecord"];

                DataRow[] deletedRow = MainTaxDataTable.Select("SlNo=" + performpara.Split('~')[1]);
                if (deletedRow.Length > 0)
                {
                    foreach (DataRow dr in deletedRow)
                    {
                        MainTaxDataTable.Rows.Remove(dr);
                    }

                }

                Session["ProjectChallanFinalTaxRecord"] = MainTaxDataTable;
                DataTable taxDetails = (DataTable)Session["ProjectChallanTaxDetails"];
                if (taxDetails != null)
                {
                    foreach (DataRow dr in taxDetails.Rows)
                    {
                        dr["Amount"] = "0.00";
                    }
                    Session["ProjectChallanTaxDetails"] = taxDetails;
                }
            }
        }
        public double ReCalculateTaxAmount(string slno, double amount)
        {
            DataTable MainTaxDataTable = (DataTable)Session["ProjectChallanFinalTaxRecord"];
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
            Session["ProjectChallanFinalTaxRecord"] = MainTaxDataTable;

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
            proc.AddIntegerPara("@branchId", Convert.ToInt32(Session["userbranchID"]));
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
            Session["ProjectChallanFinalTaxRecord"] = TaxRecord;
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
            ProcedureExecute proc = new ProcedureExecute("prc_ProjectChallan_Details");
            proc.AddVarcharPara("@Action", 500, "ProjectChallanInlineTax");
            proc.AddVarcharPara("@Challan_Id", 500, Convert.ToString(Session["ChallanID"]));
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
                DataTable TaxRecord = (DataTable)Session["ProjectChallanFinalTaxRecord"];
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

                Session["ProjectChallanFinalTaxRecord"] = TaxRecord;
            }
            else
            {
                #region fetch All data For Tax

                DataTable taxDetail = new DataTable();
                DataTable MainTaxDataTable = (DataTable)Session["ProjectChallanFinalTaxRecord"];
                DataTable databaseReturnTable = (DataTable)Session["QuotationTaxDetails"];

                //if (Convert.ToInt32(e.Parameters.Split('~')[1]) == 1)
                //    taxDetail = oDBEngine.GetDataTable("select TaxRates_ID as Taxes_ID,TaxRatesSchemeName as Taxes_Name,TaxCalculateMethods,tm.Taxes_Name as taxCodeName,tm.Taxes_ApplicableOn as 'ApplicableOn',isnull((select TaxRatesSchemeName from Config_TaxRates where TaxRates_ID=tm.Taxes_OtherTax),'')  as dependOn from Master_taxes tm inner join Config_TaxRates ct on tm.Taxes_ID=ct.TaxRates_TaxCode where tm.TaxTypeCode not in('G','V','C') and tm.TaxItemlevel='Y'");
                //else if (Convert.ToInt32(e.Parameters.Split('~')[1]) == 2)
                //taxDetail = oDBEngine.GetDataTable("select TaxRates_ID as Taxes_ID,TaxRatesSchemeName as Taxes_Name,TaxCalculateMethods,tm.Taxes_Name as taxCodeName,tm.Taxes_ApplicableOn as 'ApplicableOn',isnull((select TaxRatesSchemeName from Config_TaxRates where TaxRates_ID=tm.Taxes_OtherTax),'')  as dependOn from Master_taxes tm inner join Config_TaxRates ct on tm.Taxes_ID=ct.TaxRates_TaxCode where tm.TaxTypeCode not in('G','V','C') and tm.TaxItemlevel='Y'");

                //ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
                //proc.AddVarcharPara("@Action", 500, "LoadOtherTaxDetails");
                //proc.AddVarcharPara("@ProductID", 10, Convert.ToString(setCurrentProdCode.Value));
                //proc.AddVarcharPara("@S_quoteDate", 10, dt_PLSales.Date.ToString("yyyy-MM-dd"));
                //taxDetail = proc.GetTable();


                ProcedureExecute proc = new ProcedureExecute("prc_TaxExceptionFind");
                proc.AddVarcharPara("@Action", 500, "SQ");
                proc.AddVarcharPara("@ProductID", 10, Convert.ToString(setCurrentProdCode.Value));
                proc.AddVarcharPara("@ENTITY_ID", 100, hdnCustomerId.Value);
                proc.AddVarcharPara("@Date", 10, dt_PLSales.Date.ToString("yyyy-MM-dd"));
                proc.AddVarcharPara("@Amount", 100, HdProdGrossAmt.Value);
                proc.AddVarcharPara("@Qty", 100, hdnQty.Value);
                taxDetail = proc.GetTable();

                //Get Company Gstin 09032017
                string CompInternalId = Convert.ToString(Session["LastCompany"]);
                string[] compGstin = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_gstin", "cmp_internalid='" + CompInternalId + "'", 1);


                //Get BranchStateCode
                string BranchStateCode = "", BranchGSTIN = "";
                DataTable BranchTable = oDBEngine.GetDataTable("select StateCode,branch_GSTIN   from tbl_master_branch branch inner join tbl_master_state st on branch.branch_state=st.id where branch_id=" + Convert.ToString(ddl_Branch.SelectedValue));
                if (BranchTable != null)
                {
                    BranchStateCode = Convert.ToString(BranchTable.Rows[0][0]);
                    BranchGSTIN = Convert.ToString(BranchTable.Rows[0][1]);
                }

                string ShippingState = "";

                #region ##### Added By : Samrat Roy -- For BillingShippingUserControl ######
                string sstateCode = BillingShippingControl.GetShippingStateCode(Request.QueryString["key"]);
                ShippingState = sstateCode;
                if (ShippingState.Trim() != "")
                {
                    ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
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

                if (ShippingState.Trim() != "" && BranchStateCode != "")
                {


                    if (BranchStateCode == ShippingState)
                    {

                        //Check if the state is in union territories then only UTGST will apply
                        //   Chandigarh     Andaman and Nicobar Islands    DADRA & NAGAR HAVELI    DAMAN & DIU  Lakshadweep              PONDICHERRY
                        if (ShippingState == "4" || ShippingState == "26" || ShippingState == "25" || ShippingState == "35" || ShippingState == "31" || ShippingState == "34")
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
                if (compGstin[0].Trim() == "" && BranchGSTIN == "")
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
                                    obj.calCulatedOn = Math.Round(finalCalCulatedOn, 2);
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

                    DataTable TaxRecord = (DataTable)Session["ProjectChallanFinalTaxRecord"];


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
                                    obj.calCulatedOn = Math.Round(finalCalCulatedOn, 2);
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
                    Session["ProjectChallanFinalTaxRecord"] = TaxRecord;

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



        public class TaxSetailsJson
        {
            public string SchemeName { get; set; }
            public int vissibleIndex { get; set; }
            public string applicableOn { get; set; }
            public string applicableBy { get; set; }
        }
        public class DocumentDetails
        {
            public Int64 ProjectId { get; set; }
            public string ProjectCode { get; set; }
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
        protected void taxgrid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {

            int slNo = Convert.ToInt32(HdSerialNo.Value);
            DataTable TaxRecord = (DataTable)Session["ProjectChallanFinalTaxRecord"];
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


            Session["ProjectChallanFinalTaxRecord"] = TaxRecord;


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
            DateTime quoteDate = Convert.ToDateTime(dt_PLSales.Date.ToString("yyyy-MM-dd"));

            PopulateGSTCSTVATCombo(quoteDate.ToString("yyyy-MM-dd"));
            CreateDataTaxTable();
        }

        protected void cmbGstCstVatcharge_Callback(object sender, CallbackEventArgsBase e)
        {
            Session["ProjectChallanTaxDetails"] = null;
            DateTime quoteDate = Convert.ToDateTime(dt_PLSales.Date.ToString("yyyy-MM-dd"));
            //PopulateChargeGSTCSTVATCombo(quoteDate.ToString("yyyy-MM-dd"));
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
            if (Session["ProjectChallanFinalTaxRecord"] != null)
            {
                TaxDetailTable = (DataTable)Session["ProjectChallanFinalTaxRecord"];

                var rows = TaxDetailTable.Select("SlNo ='" + SrlNo + "'");
                foreach (var row in rows)
                {
                    row.Delete();
                }
                TaxDetailTable.AcceptChanges();

                Session["ProjectChallanFinalTaxRecord"] = TaxDetailTable;
            }
        }
        public void UpdateTaxDetails(string oldSrlNo, string newSrlNo)
        {
            DataTable TaxDetailTable = new DataTable();
            if (Session["ProjectChallanFinalTaxRecord"] != null)
            {
                TaxDetailTable = (DataTable)Session["ProjectChallanFinalTaxRecord"];

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

                Session["ProjectChallanFinalTaxRecord"] = TaxDetailTable;
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

        //public void PopulateChargeGSTCSTVATCombo(string quoteDate)
        //{
        //    string LastCompany = "";
        //    if (Convert.ToString(Session["LastCompany"]) != null)
        //    {
        //        LastCompany = Convert.ToString(Session["LastCompany"]);
        //    }
        //    ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
        //    proc.AddVarcharPara("@Action", 500, "LoadChargeGSTCSTVATCombo");
        //    proc.AddVarcharPara("@S_QuoteAdd_CompanyID", 10, Convert.ToString(LastCompany));
        //    proc.AddVarcharPara("@S_quoteDate", 10, quoteDate);
        //    DataTable DT = proc.GetTable();
        //    cmbGstCstVatcharge.DataSource = DT;
        //    cmbGstCstVatcharge.TextField = "Taxes_Name";
        //    cmbGstCstVatcharge.ValueField = "Taxes_ID";
        //    cmbGstCstVatcharge.DataBind();
        //}

        #endregion

        #region Subhabrata


        public void DeleteWarehouse(string SrlNo)
        {
            DataTable Warehousedt = new DataTable();
            if (Session["SC_WarehouseData"] != null)
            {
                Warehousedt = (DataTable)Session["SC_WarehouseData"];

                var rows = Warehousedt.Select(string.Format("Product_SrlNo ='{0}'", SrlNo));
                foreach (var row in rows)
                {
                    row.Delete();
                }
                Warehousedt.AcceptChanges();

                Session["SC_WarehouseData"] = Warehousedt;
            }
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


        #endregion


        public DataTable GetOrderEditData()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GetChallanInvoiceDetails");
            proc.AddVarcharPara("@Action", 500, "ChallanEditDetails");
            proc.AddVarcharPara("@Order_Id", 500, Convert.ToString(Session["ChallanID"]));
            proc.AddVarcharPara("@Doc_Type", 500, Convert.ToString(Session["Doc_Type"]));
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetIdFromSalesInvoiceExists()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GetChallanInvoiceDetails");
            proc.AddVarcharPara("@Action", 500, "IsChallanIdExistInInvoice");
            proc.AddVarcharPara("@Order_Id", 500, Convert.ToString(Session["ChallanID"]));
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetInvoiceDateData()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GetInvoiceDate");
            proc.AddVarcharPara("@Action", 500, "InvoiceDateDetails");
            proc.AddVarcharPara("@InvoiceID", 500, Convert.ToString(Request.QueryString["key"]));
            dt = proc.GetTable();
            return dt;
        }
        public void SetOrderDetails()
        {
            DataTable OrderEditdt = GetOrderEditData();
            if (OrderEditdt != null && OrderEditdt.Rows.Count > 0)
            {
                string Branch_Id = Convert.ToString(OrderEditdt.Rows[0]["Challan_BranchId"]);
                string Order_Number = Convert.ToString(OrderEditdt.Rows[0]["Challan_Number"]);
                string Order_Date = Convert.ToString(OrderEditdt.Rows[0]["Challan_Date"]);
                //New Added
                string Order_NumberingScheme = Convert.ToString(OrderEditdt.Rows[0]["Challan_NumScheme"]);
                //End
                string Order_OANumber = Convert.ToString(OrderEditdt.Rows[0]["Challan_OANumber"]);
                string Order_OADate = Convert.ToString(OrderEditdt.Rows[0]["Challan_OADate"]);
                string Quotation_Date = Convert.ToString(OrderEditdt.Rows[0]["Order_Quotation_Date"]);
                string Quotation_Number = Convert.ToString(OrderEditdt.Rows[0]["Challan_Quotation"]);
                string Order_Expiry = Convert.ToString(OrderEditdt.Rows[0]["Challan_Expiry"]);
                string Customer_Id = Convert.ToString(OrderEditdt.Rows[0]["Customer_Id"]);
                hdnCustomerId.Value = Customer_Id;//added on 222-02-2017
                string Contact_Person_Id = Convert.ToString(OrderEditdt.Rows[0]["Contact_Person_Id"]);
                string Order_Reference = Convert.ToString(OrderEditdt.Rows[0]["Challan_Reference"]);
                string Currency_Id = Convert.ToString(OrderEditdt.Rows[0]["Currency_Id"]);
                string Currency_Conversion_Rate = Convert.ToString(OrderEditdt.Rows[0]["Currency_Conversion_Rate"]);
                string Tax_Option = Convert.ToString(OrderEditdt.Rows[0]["Tax_Option"]);
                string Doc_type = Convert.ToString(OrderEditdt.Rows[0]["Doc_Type"]);
                string Quoids = Convert.ToString(OrderEditdt.Rows[0]["Challan_Doc_Ids"]);
                Session["Doc_Type"] = Doc_type;

                //Subhabrata
                string VehicleNumber = Convert.ToString(OrderEditdt.Rows[0]["VehicleNumber"]);
                string TransporterName = Convert.ToString(OrderEditdt.Rows[0]["TransporterName"]);
                string TransporterPhone = Convert.ToString(OrderEditdt.Rows[0]["TransporterPhone"]);
                string strEwayBill = Convert.ToString(OrderEditdt.Rows[0]["EwayBillNo"]);


                string CreditDueDate = Convert.ToString(OrderEditdt.Rows[0]["DueDate"]);
                string CreditDays = Convert.ToString(OrderEditdt.Rows[0]["CreditDays"]);

                //ASPxTextBox txtDriverName = (ASPxTextBox)VehicleDetailsControl.FindControl("txtDriverName");
                //ASPxTextBox txtPhoneNo = (ASPxTextBox)VehicleDetailsControl.FindControl("txtPhoneNo");
                //DropDownList ddl_VehicleNo = (DropDownList)VehicleDetailsControl.FindControl("ddl_VehicleNo");

                //txtDriverName.Text = TransporterName;
                //txtPhoneNo.Text = TransporterPhone;
                //ddl_VehicleNo.SelectedValue = VehicleNumber;
                //End

                DataTable dtt = GetProjectEditData();
                if (dtt != null)
                {
                    lookup_Project.GridView.Selection.SelectRowByKey(Convert.ToInt64(dtt.Rows[0]["Proj_Id"]));

                    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                    DataTable dt2 = oDBEngine.GetDataTable("Select Hierarchy_ID from V_ProjectList where Proj_Id='" + Convert.ToInt64(dtt.Rows[0]["Proj_Id"]) + "'");
                    if (dt2.Rows.Count > 0)
                    {
                        ddlHierarchy.SelectedValue = Convert.ToString(dt2.Rows[0]["Hierarchy_ID"]);
                    }
                }


                if (!String.IsNullOrEmpty(Quoids))
                {
                    string[] eachQuo = Quoids.Split(',');
                    if (eachQuo.Length > 1)//More tha one quotation
                    {
                        if (Doc_type == "SO")
                        {
                            dt_Quotation.Text = "Multiple Select Order Dates";
                        }
                        else
                        {
                            dt_Quotation.Text = "Multiple Select Invoice Dates";
                        }

                        BindLookUp(Customer_Id, Order_Date, Doc_type);
                        foreach (string val in eachQuo)
                        {
                            lookup_order.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                        }
                        // lbl_MultipleDate.Attributes.Add("style", "display:block");
                    }
                    else if (eachQuo.Length == 1)//Single Quotation
                    { //lbl_MultipleDate.Attributes.Add("style", "display:none"); }
                        BindLookUp(Customer_Id, Order_Date, Doc_type);
                        foreach (string val in eachQuo)
                        {
                            lookup_order.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                        }
                    }
                    else//No Quotation selected
                    {
                        BindLookUp(Customer_Id, Order_Date, Doc_type);
                    }
                }
                //Radio button doc type selected
                if (Doc_type != "POS")
                {
                    if (!string.IsNullOrEmpty(Doc_type))
                    {
                        //rdl_SaleInvoice.Items.FindByText(Doc_type).Selected = true;
                        rdl_SaleInvoice.SelectedValue = Doc_type;
                        rdl_SaleInvoice.Enabled = false;
                    }
                }
                else if (Doc_type == "POS")
                {
                    rdl_SaleInvoice.SelectedValue = "SI";
                    rdl_SaleInvoice.Enabled = false;
                }

                txtCreditDays.Text = CreditDays;
                if (!string.IsNullOrEmpty(CreditDueDate))
                {
                    dt_SaleInvoiceDue.Date = Convert.ToDateTime(CreditDueDate);
                }

                string Order_SalesmanId = Convert.ToString(OrderEditdt.Rows[0]["Challan_SalesmanId"]);
                if (Tax_Option != "")
                { PopulateGSTCSTVAT(Tax_Option); }

                string Tax_Code = Convert.ToString(OrderEditdt.Rows[0]["Tax_Code"]);

                txt_SlChallanNo.Text = Order_Number;
                if (!string.IsNullOrEmpty(Order_Date))
                {
                    dt_PLSales.Date = Convert.ToDateTime(Order_Date);
                }
                if (!string.IsNullOrEmpty(strEwayBill))
                {
                    txt_EWayBillNO.Text = strEwayBill;
                }
                else
                {
                    txt_EWayBillNO.Text = "";
                }
                if (!string.IsNullOrEmpty(Quotation_Date))
                {
                    dt_Quotation.Text = Quotation_Date;
                }
                if (!string.IsNullOrEmpty(Order_OADate))
                {
                    dt_OADate.Date = Convert.ToDateTime(Order_OADate);
                }
                // dt_PlOrderExpiry.Date = Convert.ToDateTime(Order_Expiry);

                //lookup_Customer.GridView.Selection.SelectRowByKey(Customer_Id);//Commented by:Subhabrata on 01-12-2017

                //SetCustomerDDbyValue(Customer_Id);//subhabrata on 27-12-2017
                txtCustName.Text = Convert.ToString(OrderEditdt.Rows[0]["Customer"]);


                // string strCustomer = Convert.ToString(hdfLookupCustomer.Value);
                //cmbContactPerson.SelectedItem.Value = Customer_Id;
                //  cmbContactPerson.SelectedItem = cmbContactPerson.Items.FindByValue(Customer_Id);
                txt_Refference.Text = Order_Reference;
                ddl_Branch.SelectedValue = Branch_Id;
                //ddl_SalesAgent.SelectedValue = Order_SalesmanId;//Subhabrata on 28-12-2017
                txtSalesManAgent.Text = Convert.ToString(OrderEditdt.Rows[0]["SalesMan"]);
                hdnSalesManAgentId.Value = Order_SalesmanId;
                //Added 15-02-2017
                ddl_numberingScheme.SelectedValue = Order_NumberingScheme;
                //End
                if (!string.IsNullOrEmpty(Currency_Id))
                { ddl_Currency.SelectedValue = Currency_Id; }
                else
                {
                    ddl_Currency.SelectedValue = "1";
                }


                txt_Rate.Value = Currency_Conversion_Rate;
                txt_Rate.Text = Currency_Conversion_Rate;
                if (Tax_Option != "0")
                {
                    ddl_AmountAre.Value = Tax_Option;
                    if (Tax_Option == "3")
                    {
                        ddl_AmountAre.ClientEnabled = false;
                    }
                }
                if (Tax_Code != "0")
                {
                    //ddl_VatGstCst.Value = Tax_Code;
                    PopulateGSTCSTVAT("2");
                    setValueForHeaderGST(ddl_VatGstCst, Tax_Code);
                }
                else
                {
                    PopulateGSTCSTVAT("2");
                    ddl_VatGstCst.SelectedIndex = 0;
                }


                txt_SlChallanNo.Value = Order_Number;
                hddnOrderNumber.Value = Order_Number;
                //  ddl_Quotation.Value = Quotation_Number;
                txt_OANumber.Text = Order_OANumber;
                if (Customer_Id != "")
                { PopulateContactPerson(Customer_Id); }

                cmbContactPerson.Value = Contact_Person_Id;





            }
        }

        //public void Bind_Currency()
        //{
        //    string LocalCurrency = Convert.ToString(Session["LocalCurrency"]);
        //    string basedCurrency = Convert.ToString(LocalCurrency.Split('~')[0]);
        //    //SqlCurrencyBind.SelectCommand = "Select * From ((Select '0' as Currency_ID , 'Select' as Currency_AlphaCode) Union select Currency_ID,Currency_AlphaCode from Master_Currency where Currency_ID<>'" + basedCurrency[0] + "' )tbl Order By Currency_ID";
        //    //CmbCurrency.DataBind();
        //    //SqlCurrencyBind.SelectCommand = "Select * From ((Select '0' as Currency_ID , 'Select' as Currency_AlphaCode) Union select Currency_ID,Currency_AlphaCode from Master_Currency)tbl Order By Currency_ID";
        //    SqlCurrencyBind.SelectCommand = "select Currency_ID,Currency_AlphaCode from Master_Currency Order By Currency_ID";
        //    CmbCurrency.DataBind();


        //}

        [WebMethod]
        public static bool CheckUniqueCode(string OrderNo)
        {
            bool flag = false;
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            try
            {
                MShortNameCheckingBL objShortNameChecking = new MShortNameCheckingBL();
                flag = objShortNameChecking.CheckUnique(OrderNo, "0", "SalesChallan");
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
        public static object GetPackingQuantityWarehouse(Int64 ProductID)
        {
            List<MultiUOMPRConversionPacking> RateLists = new List<MultiUOMPRConversionPacking>();

            decimal packing_quantity = 0;
            decimal sProduct_quantity = 0;
            bool isOverideConvertion = false;
            ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesReturn_Details");
            proc.AddVarcharPara("@Action", 500, "PackingQuantityDetailsForSalesManual");
            proc.AddBigIntegerPara("@PackingProductId", ProductID);
            DataTable dt = proc.GetTable();
            RateLists = DbHelpers.ToModelList<MultiUOMPRConversionPacking>(dt);
            if (dt != null && dt.Rows.Count > 0)
            {
                packing_quantity = Convert.ToDecimal(dt.Rows[0]["packing_quantity"]);
                sProduct_quantity = Convert.ToDecimal(dt.Rows[0]["sProduct_quantity"]);
                isOverideConvertion = Convert.ToBoolean(dt.Rows[0]["isOverideConvertion"]);
            }
            //return packing_quantity + '~' + sProduct_quantity;
            return RateLists;
        }

        [WebMethod]
        public static List<string> CheckBalQuantity(string Id, string ProductID)
        {
            DataTable dt = new DataTable();
            List<string> obj = new List<string>();
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            try
            {
                ProjectActivitiesBL objSalActBL = new ProjectActivitiesBL();
                dt = objSalActBL.GetBalQuantityForQuantiyCheck(Id, ProductID, "SalesChallan");

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

        #region Subhabrata/StcokAvailable
        protected void acpAvailableStock_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string performpara = e.Parameter;
            string strProductID = Convert.ToString(performpara.Split('~')[0]);
            //DateTime strDate = Convert.ToDateTime(Convert.ToString(performpara.Split('~')[1]));
            string strDate = Convert.ToString(dt_PLSales.Date);
            string strBranch = Convert.ToString(ddl_Branch.SelectedValue);
            //string strBranch = Convert.ToString(Session["userbranchID"]);
            acpAvailableStock.JSProperties["cpstock"] = "0.00";

            try
            {
                //DataTable dt2 = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableQuotation(" + strBranch + ",'" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]) + "'," + strProductID + ") as branchopenstock");
                DataTable dt2 = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableStockSCBOIST(" + strBranch + ",'" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]) + "','" + strProductID + "','" + Convert.ToDateTime(strDate) + "') as branchopenstock");

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
        #endregion
        protected void ComponentDatePanel_Callback(object sender, CallbackEventArgsBase e)
        {


            string strSplitCommand = e.Parameter.Split('~')[0];
            if (strSplitCommand == "BindQuotationDate")
            {
                string Quote_No = Convert.ToString(e.Parameter.Split('~')[1]);

                DataTable dt_QuotationDetails = objProjectActivitiesBL.GetQuotationDate(Quote_No);
                if (dt_QuotationDetails != null && dt_QuotationDetails.Rows.Count > 0)
                {
                    string quotationdate = Convert.ToString(dt_QuotationDetails.Rows[0]["Quote_Date"]);
                    if (!string.IsNullOrEmpty(quotationdate))
                    {

                        dt_Quotation.Text = Convert.ToString(quotationdate);


                    }
                }
            }




        }
        protected void lookup_order_DataBinding(object sender, EventArgs e)
        {
            if (Session["OrderData"] != null)
            {
                lookup_order.DataSource = (DataTable)Session["OrderData"];
            }
        }
        protected void BindLookUp(String customer, string OrderDate, string Doc_type)
        {
            string status = string.Empty;

            //Subhabrata
            if (Convert.ToString(Request.QueryString["key"]) != "ADD")
            {
                status = "DONE";
            }
            else
            {
                status = "NOT-DONE";
            }//End



            DataTable SalesOrderTable;

            if (Doc_type == "SO")
            {
                if (Convert.ToString(Session["ActionType"]).ToUpper() != "ADD")
                {
                    SalesOrderTable = objBL.GetSalesOrderonSalesChallan(customer, OrderDate, status, Convert.ToString(Session["userbranchHierarchy"]));
                }
                else
                {
                    SalesOrderTable = objBL.GetSalesOrderonSalesChallan(customer, OrderDate, status, Convert.ToString(ddl_Branch.SelectedValue));
                    //SalesOrderTable = objBL.GetSalesOrderonProjectChallan(customer, OrderDate, status, Convert.ToString(Session["userbranchHierarchy"]));
                }


                Session["OrderData"] = SalesOrderTable;
                lookup_order.GridView.Selection.CancelSelection();
                lookup_order.DataSource = SalesOrderTable;
                lookup_order.DataBind();
            }
            else if (Doc_type == "SI")
            {
                SalesOrderTable = objBL.GetSalesInvoiceonSalesChallan(customer, OrderDate, status, Convert.ToInt32(ddl_Branch.SelectedValue));
                Session["OrderData"] = SalesOrderTable;
                lookup_order.GridView.Selection.CancelSelection();
                lookup_order.DataSource = SalesOrderTable;
                lookup_order.DataBind();
            }




            //Session["OrderData"] = SalesOrderTable;
        }


        protected void ComponentBranch_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

        }
        protected void ComponentSalesOrder_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string status = string.Empty;
            string customer = string.Empty;
            string OrderDate = string.Empty;
            string BranchId = string.Empty;
            if (e.Parameter.Split('~')[0] == "BindSalesOrderGrid")
            {
                if (Convert.ToString(Request.QueryString["key"]) != "ADD")
                {
                    status = "DONE";
                }
                else
                {
                    status = "NOT-DONE";
                }

                if (e.Parameter.Split('~')[1] != null)
                { customer = e.Parameter.Split('~')[1]; }
                if (e.Parameter.Split('~')[2] != null)
                { OrderDate = e.Parameter.Split('~')[2]; }

                var branchId = ddl_Branch.SelectedValue;

                DataTable SalesOrderTable;
                if (e.Parameter.Split('~')[3] == "DateCheck")
                {
                    lookup_order.GridView.Selection.UnselectAll();
                }

                if (e.Parameter.Split('~')[4] == "SO")
                {
                    hddnTextTaggedDoc.Value = e.Parameter.Split('~')[4];

                    //SalesOrderTable = objBL.GetSalesOrderonProjectChallan(customer, OrderDate, status, Convert.ToInt32(ddl_Branch.SelectedValue));

                    if (Convert.ToString(Session["ActionType"]).ToUpper() != "ADD")
                    {
                        SalesOrderTable = objBL.GetSalesOrderonSalesChallan(customer, OrderDate, status, Convert.ToString(Session["userbranchHierarchy"]));
                    }
                    else
                    {
                        SalesOrderTable = objBL.GetSalesOrderonSalesChallan(customer, OrderDate, status, Convert.ToString(ddl_Branch.SelectedValue));
                        //SalesOrderTable = objBL.GetSalesOrderonProjectChallan(customer, OrderDate, status, Convert.ToString(Session["userbranchHierarchy"]));
                    }

                    Session["OrderData"] = SalesOrderTable;
                    lookup_order.GridView.Selection.CancelSelection();
                    lookup_order.DataSource = SalesOrderTable;
                    lookup_order.DataBind();

                }
                else if (e.Parameter.Split('~')[4] == "SI")
                {
                    hddnTextTaggedDoc.Value = e.Parameter.Split('~')[4];
                    SalesOrderTable = objBL.GetProjectInvoiceonProjectChallan(customer, OrderDate, status, Convert.ToInt32(ddl_Branch.SelectedValue));
                    Session["OrderData"] = SalesOrderTable;
                    lookup_order.GridView.Selection.CancelSelection();
                    lookup_order.DataSource = SalesOrderTable;
                    lookup_order.DataBind();

                }


            }
            else if (e.Parameter.Split('~')[0] == "BindOrderLookupOnSelection")//Subhabrata for binding quotation
            {
                if (grid_Products.GetSelectedFieldValues("Quotation_No").Count != 0)
                {
                    string OrderIds = string.Empty;
                    for (int i = 0; i < grid_Products.GetSelectedFieldValues("Quotation_No").Count; i++)
                    {
                        OrderIds += "," + grid_Products.GetSelectedFieldValues("Quotation_No")[i];
                    }
                    OrderIds = OrderIds.TrimStart(',');
                    lookup_order.GridView.Selection.UnselectAll();
                    if (!String.IsNullOrEmpty(OrderIds))
                    {
                        string[] eachQuo = OrderIds.Split(',');
                        if (eachQuo.Length > 1)//More tha one quotation
                        {
                            dt_Quotation.Text = "Multiple Select Quotation Dates";
                            //BindLookUp(Customer_Id, Order_Date);
                            foreach (string val in eachQuo)
                            {
                                lookup_order.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                            }
                        }
                        else if (eachQuo.Length == 1)//Single Quotation
                        {
                            foreach (string val in eachQuo)
                            {
                                lookup_order.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                            }
                        }
                        else//No Quotation selected
                        {
                            lookup_order.GridView.Selection.UnselectAll();
                        }
                    }
                }
                else if (grid_Products.GetSelectedFieldValues("Quotation_No").Count == 0)
                {
                    lookup_order.GridView.Selection.UnselectAll();
                }
            }
            else if (e.Parameter.Split('~')[0] == "DateCheckOnChanged")//Subhabrata for binding quotation
            {

                if (grid_Products.GetSelectedFieldValues("Quotation_No").Count != 0)
                {

                    DateTime SalesOrderDate = Convert.ToDateTime(e.Parameter.Split('~')[2]);
                    if (lookup_order.GridView.GetSelectedFieldValues("Order_Date").Count() != 0)
                    {
                        //DateTime QuotationDate = Convert.ToDateTime(lookup_order.GridView.GetSelectedFieldValues("Order_Date")[0]);
                        //if (SalesOrderDate < QuotationDate)
                        //{
                        lookup_order.GridView.Selection.UnselectAll();

                        //}
                    }
                }
            }


        }

        [WebMethod]
        public static object GetSecondUOMDetails(string ProductID, string warehouseid, string docid)
        {
            SecondUOMDetailsBL uomBL = new SecondUOMDetailsBL();
            List<SecondUOMDetails> finalResult = uomBL.GetSencondUOMDetails(ProductID, null, "SC", "OUT", warehouseid, docid);

            return finalResult;

        }
        [WebMethod]
        public static string SaveSecondUOMDetails(string list)
        {

            System.Web.Script.Serialization.JavaScriptSerializer jsSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<SecondUOMDetails> finalResult = jsSerializer.Deserialize<List<SecondUOMDetails>>(list);
            HttpContext.Current.Session["SecondUOMDetails"] = finalResult;
            //DataTable dtoutput = uomBL.SaveSencondUOMDetails(finalResult, "SC", "OUT");

            return null;

        }


        #region Warehouse Details

        public DataTable GetOrderWarehouseData()
        {
            try
            {
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_StockGetSCwarehousentry");
                proc.AddVarcharPara("@Action", 500, "OrderWarehouse");
                proc.AddVarcharPara("@PCNumber", 500, Convert.ToString(Session["ChallanID"]));
                proc.AddVarcharPara("@Finyear", 500, Convert.ToString(Session["LastFinYear"]));
                proc.AddVarcharPara("@ProductID", 500, Convert.ToString(hdfProductID.Value));
                proc.AddVarcharPara("@branchID", 500, Convert.ToString(ddl_Branch.SelectedValue));
                proc.AddVarcharPara("@compnay", 500, Convert.ToString(Session["LastCompany"]));
                dt = proc.GetTable();

                string strNewVal = "", strOldVal = "";
                DataTable tempdt = dt.Copy();
                foreach (DataRow drr in tempdt.Rows)
                {
                    strNewVal = Convert.ToString(drr["warehouseID"]);

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

                Session["SC_LoopWarehouse"] = Convert.ToString(Convert.ToInt32(strNewVal) + 1);
                tempdt.Columns.Remove("warehouseID");
                return tempdt;
            }
            catch
            {
                return null;
            }
        }

        public DataTable GetChallanWarehouseData()
        {
            try
            {
                MasterSettings masterBl = new MasterSettings();
                string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));

                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
                proc.AddVarcharPara("@Action", 500, "ChallanWarehouse");
                proc.AddVarcharPara("@Order_Id", 500, Convert.ToString(Session["ChallanID"]));
                proc.AddVarcharPara("@Multiwarehouse", 500, Convert.ToString(multiwarehouse));
                dt = proc.GetTable();

                string strNewVal = "", strOldVal = "";
                DataTable tempdt = dt.Copy();
                foreach (DataRow drr in tempdt.Rows)
                {
                    strNewVal = Convert.ToString(drr["ChallanWarehouse_Id"]);

                    if (strNewVal == strOldVal)
                    {
                        drr["WarehouseName"] = "";
                        //drr["Quantity"] = "";
                        //drr["BatchNo"] = "";
                        drr["SalesUOMName"] = "";
                        //drr["SalesQuantity"] = "";
                        drr["StkUOMName"] = "";
                        //drr["StkQuantity"] = "";
                        //drr["ConversionMultiplier"] = "";
                        //drr["AvailableQty"] = "";
                        //drr["BalancrStk"] = "";
                        drr["MfgDate"] = "";
                        drr["ExpiryDate"] = "";
                    }

                    strOldVal = strNewVal;
                }

                Session["LoopSalesOrderWarehouse"] = Convert.ToString(Convert.ToInt32(strNewVal) + 1);
                tempdt.Columns.Remove("ChallanWarehouse_Id");
                return tempdt;
            }
            catch
            {
                return null;
            }
        }
        protected void CmbWarehouse_Callback(object sender, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "BindWarehouse")
            {
                DataTable dt = GetWarehouseData();

                CmbWarehouse.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CmbWarehouse.Items.Add(Convert.ToString(dt.Rows[i]["WarehouseName"]), Convert.ToString(dt.Rows[i]["WarehouseID"]));
                }
            }
        }
        protected void CmbBatch_Callback(object sender, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "BindBatch")
            {
                string WarehouseID = Convert.ToString(e.Parameter.Split('~')[1]);
                DataTable dt = GetBatchData(WarehouseID);

                CmbBatch.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CmbBatch.Items.Add(Convert.ToString(dt.Rows[i]["BatchName"]), Convert.ToString(dt.Rows[i]["BatchID"]));
                }
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
                dt = objCRMSalesOrderDtlBL.GetSerialataOnFIFOBasisForChallan(wareHouseStr, BatchID, Serial, ProducttId, id, LastSerial);
            }
            else
            {
                dt = objCRMSalesOrderDtlBL.GetSerialataOnFIFOBasisForChallan(wareHouseStr, BatchID, Serial, ProducttId, id, Serial);
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
                string Qty = Convert.ToString(e.Parameter.Split('~')[3]);
                string strQuoteDate = Convert.ToString(dt_PLSales.Date);
                DataTable dt;
                if (string.IsNullOrEmpty(Qty))
                {
                    Qty = "0";
                    dt = GetSerialataForFifo(WarehouseID, BatchID, Qty);
                }
                else if (Qty == "NoFIFO")
                {
                    dt = GetSerialataNew(WarehouseID, BatchID);
                }
                else
                {
                    dt = GetSerialataForFifo(WarehouseID, BatchID, Qty);
                }

                //DataTable dt = GetSerialata(WarehouseID, BatchID);

                if (Session["SC_WarehouseData"] != null)
                {
                    DataTable Warehousedt = (DataTable)Session["SC_WarehouseData"];
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
            else if (WhichCall == "BindSerialAfterCross")
            {
                ASPxListBox lb = sender as ASPxListBox;
                lb.DataSource = null;
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

                if (Session["SC_WarehouseData"] != null)
                {
                    DataTable Warehousedt = (DataTable)Session["SC_WarehouseData"];
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
            else if (WhichCall == "CheckSerialOnFIFO")
            {
                //string WarehouseID = Convert.ToString(e.Parameter.Split('~')[1]);
                //string BatchID = Convert.ToString(e.Parameter.Split('~')[2]);
                //string editSerialID = Convert.ToString(e.Parameter.Split('~')[3]);
                //string[] Serials = editSerialID.Split(',');
                //string Serial = Serials[0].TrimStart(',');

                //DataTable dt = GetSerialataOnFIFOBasis(WarehouseID, BatchID, Serial);
                //if (Convert.ToInt32(dt.Rows[0].Field<Int32>("ResturnVal")) == 0)
                //{
                //    ASPxListBox lb = sender as ASPxListBox;
                //    lb.JSProperties["cpFifoMsg"]="Check Not Possible";
                //}

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
        protected void GrdWarehouse_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GrdWarehouse.JSProperties["cpIsSave"] = "";
            string strSplitCommand = e.Parameters.Split('~')[0];
            string Type = "";

            if (strSplitCommand == "Display")
            {
                GetProductType(ref Type);

                string ProductID = Convert.ToString(hdfProductID.Value);
                string SerialID = Convert.ToString(e.Parameters.Split('~')[1]);
                //Session["SC_WarehouseData"] = GetOrderWarehouseData();
                DataTable Warehousedt = new DataTable();
                if (Session["SC_WarehouseData"] != null)
                {
                    Warehousedt = (DataTable)Session["SC_WarehouseData"];
                }
                else
                {
                    Warehousedt.Columns.Add("Product_SrlNo", typeof(string));
                    Warehousedt.Columns.Add("SrlNo", typeof(string));
                    Warehousedt.Columns.Add("WarehouseID", typeof(string));
                    Warehousedt.Columns.Add("WarehouseName", typeof(string));
                    Warehousedt.Columns.Add("Quantity", typeof(string));
                    Warehousedt.Columns.Add("BatchID", typeof(string));
                    Warehousedt.Columns.Add("BatchNo", typeof(string));
                    Warehousedt.Columns.Add("SerialID", typeof(string));
                    Warehousedt.Columns.Add("SerialNo", typeof(string));
                    Warehousedt.Columns.Add("SalesUOMName", typeof(string));
                    Warehousedt.Columns.Add("SalesUOMCode", typeof(string));
                    Warehousedt.Columns.Add("SalesQuantity", typeof(string));
                    Warehousedt.Columns.Add("StkUOMName", typeof(string));
                    Warehousedt.Columns.Add("StkUOMCode", typeof(string));
                    Warehousedt.Columns.Add("StkQuantity", typeof(string));
                    Warehousedt.Columns.Add("ConversionMultiplier", typeof(string));
                    Warehousedt.Columns.Add("AvailableQty", typeof(string));
                    Warehousedt.Columns.Add("BalancrStk", typeof(string));
                    Warehousedt.Columns.Add("MfgDate", typeof(string));
                    Warehousedt.Columns.Add("ExpiryDate", typeof(string));
                    Warehousedt.Columns.Add("LoopID", typeof(string));
                    Warehousedt.Columns.Add("TotalQuantity", typeof(string));
                    Warehousedt.Columns.Add("Status", typeof(string));
                }

                if (Warehousedt != null && Warehousedt.Rows.Count > 0)
                {
                    DataView dvData = new DataView(Warehousedt);
                    dvData.RowFilter = "Product_SrlNo = '" + SerialID + "'";

                    GrdWarehouse.DataSource = dvData;
                    GrdWarehouse.DataBind();
                }
                else
                {
                    GrdWarehouse.DataSource = Warehousedt.DefaultView;
                    GrdWarehouse.DataBind();
                }
                // changeGridOrder();

                //added on 26-06-2017
                if (Warehousedt != null && Warehousedt.Rows.Count > 0)
                {
                    DataView dvViewData = new DataView(Warehousedt);
                    dvViewData.RowFilter = "Product_SrlNo = '" + SerialID + "'";
                    if (dvViewData.Count > 0)
                    {
                        GrdWarehouse.JSProperties["cpWarehouseQty"] = Convert.ToString(dvViewData.Count);

                    }
                    else
                    {
                        GrdWarehouse.JSProperties["cpWarehouseQty"] = "";
                    }
                }


                //End
            }
            else if (strSplitCommand == "ClearWarehouseGrid")
            {
                Session["SC_WarehouseData"] = null;
            }
            else if (strSplitCommand == "SaveDisplay")
            {
                int loopId = Convert.ToInt32(Session["SC_LoopWarehouse"]);

                string WarehouseID = Convert.ToString(e.Parameters.Split('~')[1]);
                string WarehouseName = Convert.ToString(e.Parameters.Split('~')[2]);
                string BatchID = Convert.ToString(e.Parameters.Split('~')[3]);
                string BatchName = Convert.ToString(e.Parameters.Split('~')[4]);
                string SerialID = Convert.ToString(e.Parameters.Split('~')[5]);
                string SerialName = Convert.ToString(e.Parameters.Split('~')[6]);
                string ProductID = Convert.ToString(hdfProductID.Value);
                string ProductSerialID = Convert.ToString(hdfProductSerialID.Value);
                string Qty = Convert.ToString(e.Parameters.Split('~')[7]);
                string editWarehouseID = Convert.ToString(e.Parameters.Split('~')[8]);
                string AltQty = Convert.ToString(e.Parameters.Split('~')[9]);
                string AltUOM = Convert.ToString(e.Parameters.Split('~')[10]);
                string MatchQty = string.Empty;

                string Sales_UOM_Name = "", Sales_UOM_Code = "", Stk_UOM_Name = "", Stk_UOM_Code = "", Conversion_Multiplier = "", Trans_Stock = "0", MfgDate = "", ExpiryDate = "";
                GetProductType(ref Type);

                DataTable dt = new DataTable();
                dt = GetSerialata(WarehouseID, BatchID);



                DataTable Warehousedt = new DataTable();
                if (Session["SC_WarehouseData"] != null)
                {
                    Warehousedt = (DataTable)Session["SC_WarehouseData"];
                }
                else
                {
                    Warehousedt.Columns.Add("Product_SrlNo", typeof(string));
                    Warehousedt.Columns.Add("SrlNo", typeof(string));
                    Warehousedt.Columns.Add("WarehouseID", typeof(string));
                    Warehousedt.Columns.Add("WarehouseName", typeof(string));
                    Warehousedt.Columns.Add("Quantity", typeof(string));
                    Warehousedt.Columns.Add("BatchID", typeof(string));
                    Warehousedt.Columns.Add("BatchNo", typeof(string));
                    Warehousedt.Columns.Add("SerialID", typeof(string));
                    Warehousedt.Columns.Add("SerialNo", typeof(string));
                    Warehousedt.Columns.Add("SalesUOMName", typeof(string));
                    Warehousedt.Columns.Add("SalesUOMCode", typeof(string));
                    Warehousedt.Columns.Add("SalesQuantity", typeof(string));
                    Warehousedt.Columns.Add("StkUOMName", typeof(string));
                    Warehousedt.Columns.Add("StkUOMCode", typeof(string));
                    Warehousedt.Columns.Add("StkQuantity", typeof(string));
                    Warehousedt.Columns.Add("ConversionMultiplier", typeof(string));
                    Warehousedt.Columns.Add("AvailableQty", typeof(string));
                    Warehousedt.Columns.Add("BalancrStk", typeof(string));
                    Warehousedt.Columns.Add("MfgDate", typeof(string));
                    Warehousedt.Columns.Add("ExpiryDate", typeof(string));
                    Warehousedt.Columns.Add("LoopID", typeof(string));
                    Warehousedt.Columns.Add("TotalQuantity", typeof(string));
                    Warehousedt.Columns.Add("Status", typeof(string));
                    Warehousedt.Columns.Add("AltQty", typeof(string));
                    Warehousedt.Columns.Add("AltUOM", typeof(string));
                }

                bool IsDelete = false;

                if (Type == "WBS")
                {
                    GetTotalStock(ref Trans_Stock, WarehouseID);
                    GetProductUOM(ref Sales_UOM_Name, ref Sales_UOM_Code, ref Stk_UOM_Name, ref Stk_UOM_Code, ref Conversion_Multiplier, ProductID);
                    GetBatchDetails(ref MfgDate, ref ExpiryDate, BatchID);

                    string[] SerialIDList = SerialID.Split(new string[] { "||@||" }, StringSplitOptions.None);
                    string[] SerialNameList = SerialName.Split(new string[] { "||@||" }, StringSplitOptions.None);

                    MatchQty = Convert.ToString(SerialIDList.Length);
                    if (editWarehouseID == "0")
                    {
                        for (int i = 0; i < SerialIDList.Length; i++)
                        {
                            string strSrlID = SerialIDList[i];
                            string strSrlName = SerialNameList[i];


                            if (i == 0)
                            {
                                decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                                string stkqtn = Convert.ToString(Math.Round((SerialIDList.Length * ConversionMultiplier), 2));
                                decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, SerialIDList.Length, "D", AltQty, AltUOM);
                            }
                            else
                            {
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", "", BatchID, "", strSrlID, strSrlName, "", Sales_UOM_Code, "", "", Stk_UOM_Code, "", "", "", "", "", "", loopId, SerialIDList.Length, "D", AltQty, AltUOM);
                            }
                        }
                    }
                    else
                    {
                        string strLoopID = "0", strSerial = "0";

                        DataRow[] result = Warehousedt.Select("SrlNo ='" + editWarehouseID + "'");
                        foreach (DataRow row in result)
                        {
                            strSerial = Convert.ToString(row["SerialID"]);
                            strLoopID = Convert.ToString(row["LoopID"]);
                        }

                        int count = 0;
                        DataRow[] dr = Warehousedt.Select("LoopID ='" + strLoopID + "'");
                        int value = (dr.Length + SerialIDList.Length - 1);

                        string[] temp_SerialIDList = new string[value];
                        string[] temp_SerialNameList = new string[value];

                        string[] check_SerialIDList = new string[value];
                        string[] check_SerialNameList = new string[value];

                        foreach (DataRow rows in dr)
                        {
                            if (strSerial != Convert.ToString(rows["SerialID"]))
                            {
                                temp_SerialIDList[count] = Convert.ToString(rows["SerialID"]);
                                temp_SerialNameList[count] = Convert.ToString(rows["SerialNo"]);

                                check_SerialIDList[count] = Convert.ToString(rows["SerialID"]);
                                check_SerialNameList[count] = Convert.ToString(rows["SerialNo"]);

                                count++;
                            }
                        }

                        for (int i = 0; i < SerialIDList.Length; i++)
                        {
                            temp_SerialIDList[count] = Convert.ToString(SerialIDList[i]);
                            temp_SerialNameList[count] = Convert.ToString(SerialNameList[i]);

                            count++;
                        }

                        DataRow[] delResult = Warehousedt.Select("LoopID ='" + strLoopID + "'");
                        foreach (DataRow delrow in delResult)
                        {
                            delrow.Delete();
                        }
                        Warehousedt.AcceptChanges();

                        SerialIDList = temp_SerialIDList;
                        SerialNameList = temp_SerialNameList;

                        for (int i = 0; i < SerialIDList.Length; i++)
                        {
                            string strSrlID = SerialIDList[i];
                            string strSrlName = SerialNameList[i];
                            string repute = "D";
                            if (check_SerialIDList.Contains(strSrlID)) repute = "I";

                            if (i == 0)
                            {
                                decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                                string stkqtn = Convert.ToString(Math.Round((SerialIDList.Length * ConversionMultiplier), 2));
                                decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, strLoopID, SerialIDList.Length, repute, AltQty, AltUOM);
                            }
                            else
                            {
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", "", BatchID, "", strSrlID, strSrlName, "", Sales_UOM_Code, "", "", Stk_UOM_Code, "", "", "", "", "", "", strLoopID, SerialIDList.Length, repute, AltQty, AltUOM);
                            }
                        }
                    }
                }
                else if (Type == "W")
                {
                    GetTotalStock(ref Trans_Stock, WarehouseID);
                    GetProductUOM(ref Sales_UOM_Name, ref Sales_UOM_Code, ref Stk_UOM_Name, ref Stk_UOM_Code, ref Conversion_Multiplier, ProductID);

                    decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                    string stkqtn = Convert.ToString(Math.Round((Convert.ToDecimal(Qty) * ConversionMultiplier), 2));
                    decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);
                    BatchID = "0";

                    if (editWarehouseID == "0")
                    {
                        string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                        var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND Product_SrlNo='" + ProductSerialID + "'");

                        if (updaterows.Length == 0)
                        {
                            Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, BatchID, BatchName, "0", "", Sales_UOM_Name, Sales_UOM_Code, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, Qty, "D", AltQty, AltUOM);
                        }
                        else
                        {
                            foreach (var row in updaterows)
                            {
                                decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                row["Quantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                row["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(Qty)) + " " + Sales_UOM_Name;
                                row["AltQty"] = AltQty;
                            }
                        }
                    }
                    else
                    {
                        var rows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND Convert(TotalQuantity, 'System.Decimal')='" + Qty + "' AND SrlNo='" + editWarehouseID + "'");

                        if (rows.Length == 0)
                        {
                            string whID = "";
                            string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                            var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND Product_SrlNo='" + ProductSerialID + "'");
                            foreach (var row in updaterows)
                            {
                                whID = Convert.ToString(row["SrlNo"]);
                            }

                            if (updaterows.Length == 0)
                            {
                                IsDelete = true;
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, BatchID, BatchName, "0", "", Sales_UOM_Name, Sales_UOM_Code, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, Qty, "D", AltQty, AltUOM);

                            }
                            else if (editWarehouseID == whID)
                            {
                                foreach (var row in updaterows)
                                {
                                    whID = Convert.ToString(row["SrlNo"]);
                                    decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);

                                    row["Quantity"] = Qty;
                                    row["TotalQuantity"] = Qty;
                                    row["SalesQuantity"] = Qty + " " + Sales_UOM_Name;
                                    row["AltQty"] = AltQty;
                                }
                            }
                            else if (editWarehouseID != whID)
                            {
                                IsDelete = true;
                                foreach (var row in updaterows)
                                {
                                    ID = Convert.ToString(row["SrlNo"]);
                                    decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);

                                    row["Quantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                    row["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                    row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(Qty)) + " " + Sales_UOM_Name;
                                    row["AltQty"] = Convert.ToDecimal(AltQty);
                                }
                            }
                        }
                    }
                }
                else if (Type == "WB")
                {
                    GetTotalStock(ref Trans_Stock, WarehouseID);
                    GetProductUOM(ref Sales_UOM_Name, ref Sales_UOM_Code, ref Stk_UOM_Name, ref Stk_UOM_Code, ref Conversion_Multiplier, ProductID);
                    GetBatchDetails(ref MfgDate, ref ExpiryDate, BatchID);

                    decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                    string stkqtn = Convert.ToString(Math.Round((Convert.ToDecimal(Qty) * ConversionMultiplier), 2));
                    decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);

                    if (editWarehouseID == "0")
                    {
                        string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                        var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND BatchID='" + BatchID + "' AND Product_SrlNo='" + ProductSerialID + "'");

                        if (updaterows.Length == 0)
                        {
                            Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, BatchID, BatchName, "0", "", Sales_UOM_Name, Sales_UOM_Code, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, Qty, "D", AltQty, AltUOM);
                        }
                        else
                        {
                            foreach (var row in updaterows)
                            {
                                decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                row["Quantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                row["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(Qty)) + " " + Sales_UOM_Name;
                            }
                        }
                    }
                    else
                    {
                        var rows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND BatchID='" + BatchID + "' AND Convert(TotalQuantity, 'System.Decimal')='" + Qty + "' AND SrlNo='" + editWarehouseID + "'");
                        if (rows.Length == 0)
                        {
                            string whID = "";
                            string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                            var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND BatchID='" + BatchID + "' AND Product_SrlNo='" + ProductSerialID + "'");
                            foreach (var row in updaterows)
                            {
                                whID = Convert.ToString(row["SrlNo"]);
                            }

                            if (updaterows.Length == 0)
                            {
                                IsDelete = true;
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, BatchID, BatchName, "0", "", Sales_UOM_Name, Sales_UOM_Code, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, Qty, "D", AltQty, AltUOM);
                            }
                            else if (editWarehouseID == whID)
                            {
                                foreach (var row in updaterows)
                                {
                                    whID = Convert.ToString(row["SrlNo"]);
                                    decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);

                                    row["Quantity"] = Qty;
                                    row["TotalQuantity"] = Qty;
                                    row["SalesQuantity"] = Qty + " " + Sales_UOM_Name;
                                }
                            }
                            else if (editWarehouseID != whID)
                            {
                                IsDelete = true;
                                foreach (var row in updaterows)
                                {
                                    ID = Convert.ToString(row["SrlNo"]);
                                    decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);

                                    row["Quantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                    row["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                    row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(Qty)) + " " + Sales_UOM_Name;
                                }
                            }
                        }
                    }
                }
                else if (Type == "B")
                {
                    GetProductUOM(ref Sales_UOM_Name, ref Sales_UOM_Code, ref Stk_UOM_Name, ref Stk_UOM_Code, ref Conversion_Multiplier, ProductID);
                    GetBatchDetails(ref MfgDate, ref ExpiryDate, BatchID);

                    decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                    string stkqtn = Convert.ToString(Math.Round((Convert.ToDecimal(Qty) * ConversionMultiplier), 2));
                    decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);
                    WarehouseID = "0";


                    if (editWarehouseID == "0")
                    {
                        string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                        var updaterows = Warehousedt.Select("BatchID ='" + BatchID + "' AND Product_SrlNo='" + ProductSerialID + "'");

                        if (updaterows.Length == 0)
                        {
                            Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, BatchID, BatchName, "0", "", Sales_UOM_Name, Sales_UOM_Code, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, Qty, "D", AltQty, AltUOM);
                        }
                        else
                        {
                            foreach (var row in updaterows)
                            {
                                decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                row["Quantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                row["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(Qty)) + " " + Sales_UOM_Name;
                            }
                        }
                    }
                    else
                    {
                        var rows = Warehousedt.Select("BatchID='" + BatchID + "' AND Convert(TotalQuantity, 'System.Decimal')='" + Qty + "' AND SrlNo='" + editWarehouseID + "'");
                        if (rows.Length == 0)
                        {
                            string whID = "";
                            string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                            var updaterows = Warehousedt.Select("BatchID ='" + BatchID + "' AND Product_SrlNo='" + ProductSerialID + "'");
                            foreach (var row in updaterows)
                            {
                                whID = Convert.ToString(row["SrlNo"]);
                            }

                            if (updaterows.Length == 0)
                            {
                                IsDelete = true;
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, BatchID, BatchName, "0", "", Sales_UOM_Name, Sales_UOM_Code, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, Qty, "D", AltQty, AltUOM);
                            }
                            else if (editWarehouseID == whID)
                            {
                                foreach (var row in updaterows)
                                {
                                    whID = Convert.ToString(row["SrlNo"]);
                                    decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);

                                    row["Quantity"] = Qty;
                                    row["TotalQuantity"] = Qty;
                                    row["SalesQuantity"] = Qty + " " + Sales_UOM_Name;
                                }
                            }
                            else if (editWarehouseID != whID)
                            {
                                IsDelete = true;
                                foreach (var row in updaterows)
                                {
                                    ID = Convert.ToString(row["SrlNo"]);
                                    decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);

                                    row["Quantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                    row["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                    row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(Qty)) + " " + Sales_UOM_Name;
                                }
                            }
                        }
                    }
                }
                else if (Type == "S")
                {
                    GetProductUOM(ref Sales_UOM_Name, ref Sales_UOM_Code, ref Stk_UOM_Name, ref Stk_UOM_Code, ref Conversion_Multiplier, ProductID);

                    string[] SerialIDList = SerialID.Split(new string[] { "||@||" }, StringSplitOptions.None);
                    string[] SerialNameList = SerialName.Split(new string[] { "||@||" }, StringSplitOptions.None);
                    MatchQty = Convert.ToString(SerialIDList.Length);
                    //Qty = Convert.ToString(SerialIDList.Length);
                    Qty = "1";
                    decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                    string stkqtn = Convert.ToString(Math.Round((Convert.ToDecimal(Qty) * ConversionMultiplier), 2));
                    decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);
                    WarehouseID = "0";
                    BatchID = "0";

                    for (int i = 0; i < SerialIDList.Length; i++)
                    {
                        string strSrlID = SerialIDList[i];
                        string strSrlName = SerialNameList[i];

                        if (editWarehouseID == "0")
                        {
                            string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                            Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, SerialIDList.Length, "D", AltQty, AltUOM);
                        }
                        else
                        {
                            var rows = Warehousedt.Select("SerialID ='" + strSrlID + "' AND SrlNo='" + editWarehouseID + "'");
                            if (rows.Length == 0)
                            {
                                IsDelete = true;
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, SerialIDList.Length, "D", AltQty, AltUOM);
                            }
                        }
                    }
                }
                else if (Type == "WS")
                {
                    GetTotalStock(ref Trans_Stock, WarehouseID);
                    GetProductUOM(ref Sales_UOM_Name, ref Sales_UOM_Code, ref Stk_UOM_Name, ref Stk_UOM_Code, ref Conversion_Multiplier, ProductID);
                    //GetBatchDetails(ref MfgDate, ref ExpiryDate, BatchID);

                    string[] SerialIDList = SerialID.Split(new string[] { "||@||" }, StringSplitOptions.None);
                    string[] SerialNameList = SerialName.Split(new string[] { "||@||" }, StringSplitOptions.None);
                    MatchQty = Convert.ToString(SerialIDList.Length);
                    if (editWarehouseID == "0")
                    {
                        for (int i = 0; i < SerialIDList.Length; i++)
                        {
                            string strSrlID = SerialIDList[i];
                            string strSrlName = SerialNameList[i];


                            if (i == 0)
                            {
                                decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                                string stkqtn = Convert.ToString(Math.Round((SerialIDList.Length * ConversionMultiplier), 2));
                                decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, "0", BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, SerialIDList.Length, "D", AltQty, AltUOM);
                            }
                            else
                            {
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", "", "0", "", strSrlID, strSrlName, "", Sales_UOM_Code, "", "", Stk_UOM_Code, "", "", "", "", "", "", loopId, SerialIDList.Length, "D", AltQty, AltUOM);
                            }
                        }
                    }
                    else
                    {
                        string strLoopID = "0", strSerial = "0";

                        DataRow[] result = Warehousedt.Select("SrlNo ='" + editWarehouseID + "'");
                        foreach (DataRow row in result)
                        {
                            strSerial = Convert.ToString(row["SerialID"]);
                            strLoopID = Convert.ToString(row["LoopID"]);
                        }

                        int count = 0;
                        DataRow[] dr = Warehousedt.Select("LoopID ='" + strLoopID + "'");
                        int value = (dr.Length + SerialIDList.Length - 1);

                        string[] temp_SerialIDList = new string[value];
                        string[] temp_SerialNameList = new string[value];

                        string[] check_SerialIDList = new string[value];
                        string[] check_SerialNameList = new string[value];

                        foreach (DataRow rows in dr)
                        {
                            if (strSerial != Convert.ToString(rows["SerialID"]))
                            {
                                temp_SerialIDList[count] = Convert.ToString(rows["SerialID"]);
                                temp_SerialNameList[count] = Convert.ToString(rows["SerialNo"]);

                                check_SerialIDList[count] = Convert.ToString(rows["SerialID"]);
                                check_SerialNameList[count] = Convert.ToString(rows["SerialNo"]);

                                count++;
                            }
                        }

                        for (int i = 0; i < SerialIDList.Length; i++)
                        {
                            temp_SerialIDList[count] = Convert.ToString(SerialIDList[i]);
                            temp_SerialNameList[count] = Convert.ToString(SerialNameList[i]);

                            count++;
                        }

                        DataRow[] delResult = Warehousedt.Select("LoopID ='" + strLoopID + "'");
                        foreach (DataRow delrow in delResult)
                        {
                            delrow.Delete();
                        }
                        Warehousedt.AcceptChanges();

                        SerialIDList = temp_SerialIDList;
                        SerialNameList = temp_SerialNameList;

                        for (int i = 0; i < SerialIDList.Length; i++)
                        {
                            string strSrlID = SerialIDList[i];
                            string strSrlName = SerialNameList[i];
                            string repute = "D";
                            if (check_SerialIDList.Contains(strSrlID)) repute = "I";

                            if (i == 0)
                            {
                                decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                                string stkqtn = Convert.ToString(Math.Round((SerialIDList.Length * ConversionMultiplier), 2));
                                decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, "0", BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, SerialIDList.Length, repute, AltQty, AltUOM);
                            }
                            else
                            {
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", "", "0", "", strSrlID, strSrlName, "", Sales_UOM_Code, "", "", Stk_UOM_Code, "", "", "", "", "", "", loopId, SerialIDList.Length, repute, AltQty, AltUOM);
                            }
                        }
                    }
                }
                else if (Type == "BS")
                {
                    // GetTotalStock(ref Trans_Stock, WarehouseID);
                    GetProductUOM(ref Sales_UOM_Name, ref Sales_UOM_Code, ref Stk_UOM_Name, ref Stk_UOM_Code, ref Conversion_Multiplier, ProductID);
                    GetBatchDetails(ref MfgDate, ref ExpiryDate, BatchID);

                    string[] SerialIDList = SerialID.Split(new string[] { "||@||" }, StringSplitOptions.None);
                    string[] SerialNameList = SerialName.Split(new string[] { "||@||" }, StringSplitOptions.None);
                    MatchQty = Convert.ToString(SerialIDList.Length);
                    if (editWarehouseID == "0")
                    {
                        for (int i = 0; i < SerialIDList.Length; i++)
                        {
                            string strSrlID = SerialIDList[i];
                            string strSrlName = SerialNameList[i];
                            WarehouseID = "0";

                            if (i == 0)
                            {
                                decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                                string stkqtn = Convert.ToString(Math.Round((SerialIDList.Length * ConversionMultiplier), 2));
                                decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, SerialIDList.Length, "D", AltQty, AltUOM);
                            }
                            else
                            {
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", "", BatchID, "", strSrlID, strSrlName, "", Sales_UOM_Code, "", "", Stk_UOM_Code, "", "", "", "", "", "", loopId, SerialIDList.Length, "D", AltQty, AltUOM);
                            }
                        }
                    }
                    else
                    {
                        string strLoopID = "0", strSerial = "0";

                        DataRow[] result = Warehousedt.Select("SrlNo ='" + editWarehouseID + "'");
                        foreach (DataRow row in result)
                        {
                            strSerial = Convert.ToString(row["SerialID"]);
                            strLoopID = Convert.ToString(row["LoopID"]);
                        }

                        int count = 0;
                        DataRow[] dr = Warehousedt.Select("LoopID ='" + strLoopID + "'");
                        int value = (dr.Length + SerialIDList.Length - 1);

                        string[] temp_SerialIDList = new string[value];
                        string[] temp_SerialNameList = new string[value];

                        string[] check_SerialIDList = new string[value];
                        string[] check_SerialNameList = new string[value];

                        foreach (DataRow rows in dr)
                        {
                            if (strSerial != Convert.ToString(rows["SerialID"]))
                            {
                                temp_SerialIDList[count] = Convert.ToString(rows["SerialID"]);
                                temp_SerialNameList[count] = Convert.ToString(rows["SerialNo"]);

                                check_SerialIDList[count] = Convert.ToString(rows["SerialID"]);
                                check_SerialNameList[count] = Convert.ToString(rows["SerialNo"]);

                                count++;
                            }
                        }

                        for (int i = 0; i < SerialIDList.Length; i++)
                        {
                            temp_SerialIDList[count] = Convert.ToString(SerialIDList[i]);
                            temp_SerialNameList[count] = Convert.ToString(SerialNameList[i]);

                            count++;
                        }

                        DataRow[] delResult = Warehousedt.Select("LoopID ='" + strLoopID + "'");
                        foreach (DataRow delrow in delResult)
                        {
                            delrow.Delete();
                        }
                        Warehousedt.AcceptChanges();

                        SerialIDList = temp_SerialIDList;
                        SerialNameList = temp_SerialNameList;

                        for (int i = 0; i < SerialIDList.Length; i++)
                        {
                            string strSrlID = SerialIDList[i];
                            string strSrlName = SerialNameList[i];
                            WarehouseID = "0";
                            string repute = "D";
                            if (check_SerialIDList.Contains(strSrlID)) repute = "I";

                            if (i == 0)
                            {
                                decimal ConversionMultiplier = Convert.ToDecimal(Conversion_Multiplier);
                                string stkqtn = Convert.ToString(Math.Round((SerialIDList.Length * ConversionMultiplier), 2));
                                decimal BalanceStk = Convert.ToDecimal(Trans_Stock) - Convert.ToDecimal(stkqtn);
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, SerialIDList.Length, repute, AltQty, AltUOM);
                            }
                            else
                            {
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", "", BatchID, "", strSrlID, strSrlName, "", Sales_UOM_Code, "", "", Stk_UOM_Code, "", "", "", "", "", "", loopId, SerialIDList.Length, repute, AltQty, AltUOM);
                            }
                        }
                    }
                }

                //Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, "1", BatchID, BatchName, SerialID, SerialName);
                //string sortExpression = string.Format("{0} {1}", colName, direction);
                //dt.DefaultView.Sort = sortExpression;
                //Warehousedt.DefaultView.Sort = "SrlNo Asc";
                //Warehousedt = Warehousedt.DefaultView.ToTable(true);

                if (IsDelete == true)
                {
                    DataRow[] delResult = Warehousedt.Select("SrlNo ='" + editWarehouseID + "'");
                    foreach (DataRow delrow in delResult)
                    {
                        delrow.Delete();
                    }
                    Warehousedt.AcceptChanges();
                }

                Session["SC_WarehouseData"] = Warehousedt;

                //changeGridOrder();

                //Subhabrata on 22-06-2017
                GrdWarehouse.JSProperties["cpWarehouseSaveDisplay"] = "SaveDisplay";
                //End


                GrdWarehouse.DataSource = Warehousedt.DefaultView;
                GrdWarehouse.DataBind();

                Session["SC_LoopWarehouse"] = loopId + 1;

                CmbWarehouse.SelectedIndex = -1;
                CmbBatch.SelectedIndex = -1;

                //Added on 26-06-2017
                if (!string.IsNullOrEmpty(MatchQty))
                {
                    GrdWarehouse.JSProperties["cpWarehouseQty"] = Convert.ToString(MatchQty);
                }
                else
                {
                    GrdWarehouse.JSProperties["cpWarehouseQty"] = "";
                }
                //END
            }
            else if (strSplitCommand == "Delete")
            {
                string strKey = e.Parameters.Split('~')[1];
                string strLoopID = "", strPreLoopID = "";

                DataTable Warehousedt = new DataTable();
                if (Session["SC_WarehouseData"] != null)
                {
                    Warehousedt = (DataTable)Session["SC_WarehouseData"];
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
                                    decimal S_Quantity = Convert.ToDecimal(dr["TotalQuantity"]);
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

                Session["SC_WarehouseData"] = Warehousedt;
                //Subhabrata 22-06-2017
                if (Warehousedt != null)
                {
                    if (Warehousedt.Rows.Count > 0)
                    {
                        GrdWarehouse.JSProperties["cpWarehouseDeleticity"] = "WareHouseDeleticity";
                        //GrdWarehouse.JSProperties["cpWarehouseQty"] = Convert.ToString(Warehousedt.Rows.Count);
                    }
                }//End
                GrdWarehouse.DataSource = Warehousedt.DefaultView;
                GrdWarehouse.DataBind();
            }
            else if (strSplitCommand == "WarehouseDelete")
            {
                string ProductID = Convert.ToString(hdfProductSerialID.Value);
                DeleteUnsaveWarehouse(ProductID);
            }
            else if (strSplitCommand == "WarehouseFinal")
            {
                if (Session["SC_WarehouseData"] != null)
                {
                    DataTable Warehousedt = (DataTable)Session["SC_WarehouseData"];
                    string ProductID = Convert.ToString(hdfProductSerialID.Value);
                    string strPreLoopID = "";
                    decimal sum = 0;

                    for (int i = 0; i < Warehousedt.Rows.Count; i++)
                    {
                        DataRow dr = Warehousedt.Rows[i];
                        string delLoopID = Convert.ToString(dr["LoopID"]);
                        string Product_SrlNo = Convert.ToString(dr["Product_SrlNo"]);

                        if (ProductID == Product_SrlNo)
                        {
                            string strQuantity = (Convert.ToString(dr["SalesQuantity"]) != "") ? Convert.ToString(dr["SalesQuantity"]) : "0";
                            var weight = Decimal.Parse(Regex.Match(strQuantity, "[0-9]*\\.*[0-9]*").Value);
                            //string resultString = Regex.Match(strQuantity, @"[^0-9\.]+").Value;

                            sum = sum + Convert.ToDecimal(weight);
                        }
                    }

                    if (Convert.ToDecimal(sum) == Convert.ToDecimal(hdnProductQuantity.Value))
                    {
                        GrdWarehouse.JSProperties["cpIsSave"] = "Y";
                        for (int i = 0; i < Warehousedt.Rows.Count; i++)
                        {
                            DataRow dr = Warehousedt.Rows[i];
                            string Product_SrlNo = Convert.ToString(dr["Product_SrlNo"]);
                            if (ProductID == Product_SrlNo)
                            {
                                dr["Status"] = "I";
                            }
                        }
                        Warehousedt.AcceptChanges();
                    }
                    else
                    {
                        GrdWarehouse.JSProperties["cpIsSave"] = "N";
                    }

                    Session["SC_WarehouseData"] = Warehousedt;
                }
            }

            //Added By :Subhabrata
            if (Session["SC_WarehouseData"] != null)
            {
                //DataTable Warehousedt = (DataTable)Session["SC_WarehouseData"];
                //if (Warehousedt.Rows.Count > 0)
                //{
                //    Session["WarehouseBindQty"] = Warehousedt.Rows.Count;
                //    GrdWarehouse.JSProperties["cpWarehouseQty"] = Convert.ToString(Warehousedt.Rows.Count);
                //}
                //else
                //{
                //    GrdWarehouse.JSProperties["cpWarehouseQty"] = "";
                //}
            }
            //End
        }
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string performpara = e.Parameter;
            if (performpara.Split('~')[0] == "EditWarehouse")
            {
                string SrlNo = performpara.Split('~')[1];
                string ProductType = Convert.ToString(hdfProductType.Value);

                if (Session["SC_WarehouseData"] != null)
                {
                    DataTable Warehousedt = (DataTable)Session["SC_WarehouseData"];

                    string strWarehouse = "", strBatchID = "", strSrlID = "", strQuantity = "0", StrAltQty = "0", StrAltUOM = "0";
                    var rows = Warehousedt.Select(string.Format("SrlNo ='{0}'", SrlNo));
                    foreach (var dr in rows)
                    {
                        strWarehouse = (Convert.ToString(dr["WarehouseID"]) != "") ? Convert.ToString(dr["WarehouseID"]) : "0";
                        strBatchID = (Convert.ToString(dr["BatchID"]) != "") ? Convert.ToString(dr["BatchID"]) : "0";
                        strSrlID = (Convert.ToString(dr["SerialID"]) != "") ? Convert.ToString(dr["SerialID"]) : "0";
                        strQuantity = (Convert.ToString(dr["TotalQuantity"]) != "") ? Convert.ToString(dr["TotalQuantity"]) : "0";
                        StrAltQty = (Convert.ToString(dr["AltQty"]) != "") ? Convert.ToString(dr["AltQty"]) : "0";
                        StrAltUOM = (Convert.ToString(dr["AltUOM"]) != "") ? Convert.ToString(dr["AltUOM"]) : "0";
                    }

                    //CmbWarehouse.DataSource = GetWarehouseData();
                    CmbBatch.DataSource = GetBatchData(strWarehouse);
                    CmbBatch.DataBind();

                    CallbackPanel.JSProperties["cpEdit"] = strWarehouse + "~" + strBatchID + "~" + strSrlID + "~" + strQuantity + "~" + StrAltQty + "~" + StrAltUOM;
                }
            }
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
        protected void GrdWarehouse_DataBinding(object sender, EventArgs e)
        {
            if (Session["SC_WarehouseData"] != null)
            {
                string Type = "";
                GetProductType(ref Type);
                string SerialID = Convert.ToString(hdfProductSerialID.Value);
                DataTable Warehousedt = (DataTable)Session["SC_WarehouseData"];

                if (Warehousedt != null && Warehousedt.Rows.Count > 0)
                {
                    DataView dvData = new DataView(Warehousedt);
                    dvData.RowFilter = "Product_SrlNo = '" + SerialID + "'";

                    GrdWarehouse.DataSource = dvData;
                }
                else
                {
                    GrdWarehouse.DataSource = Warehousedt.DefaultView;
                }
            }
        }
        [WebMethod]
        public static string getSchemeType(string Products_ID)
        {
            //string Type = "";
            //ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            //proc.AddVarcharPara("@Action", 500, "GetSchemeTypeCheck");
            //proc.AddVarcharPara("@ProductID", 100, Convert.ToString(Products_ID));
            //DataTable dt = proc.GetTable();

            //if (dt != null && dt.Rows.Count > 0)
            //{
            //    Type = Convert.ToString(dt.Rows[0]["Type"]);
            //}

            //return Convert.ToString(Type);
            string strschematype = "", strschemalength = "", strschemavalue = "";
            // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();


            DataTable DT = objEngine.GetDataTable("tbl_master_Idschema", " schema_type,length ", " Id = " + Convert.ToInt32(Products_ID));

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                strschematype = Convert.ToString(DT.Rows[i]["schema_type"]);
                strschemalength = Convert.ToString(DT.Rows[i]["length"]);
                strschemavalue = strschematype + "~" + strschemalength;
            }
            return Convert.ToString(strschemavalue);
        }
        public void GetQuantityBaseOnProduct(string strProductSrlNo, ref decimal WarehouseQty)
        {
            decimal sum = 0;

            DataTable Warehousedt = new DataTable();
            if (Session["SC_WarehouseData"] != null)
            {
                Warehousedt = (DataTable)Session["SC_WarehouseData"];
                for (int i = 0; i < Warehousedt.Rows.Count; i++)
                {
                    DataRow dr = Warehousedt.Rows[i];
                    string Product_SrlNo = Convert.ToString(dr["Product_SrlNo"]);

                    if (strProductSrlNo == Product_SrlNo)
                    {
                        string strQuantity = (Convert.ToString(dr["Quantity"]) != "") ? Convert.ToString(dr["Quantity"]) : "0";
                        var weight = Decimal.Parse(Regex.Match(strQuantity, "[0-9]*\\.*[0-9]*").Value);

                        sum = sum + Convert.ToDecimal(weight);
                    }
                }
            }

            WarehouseQty = sum;
        }
        public void DeleteUnsaveWarehouse(string SrlNo)
        {
            DataTable Warehousedt = new DataTable();
            if (Session["SC_WarehouseData"] != null)
            {
                Warehousedt = (DataTable)Session["SC_WarehouseData"];

                var rows = Warehousedt.Select("Product_SrlNo ='" + SrlNo + "' AND Status='D'");
                foreach (var row in rows)
                {
                    row.Delete();
                }
                Warehousedt.AcceptChanges();

                Session["SC_WarehouseData"] = Warehousedt;
            }
        }
        public DataTable DeleteWarehouseBySrl(string strKey)
        {
            string strLoopID = "", strPreLoopID = "";

            DataTable Warehousedt = new DataTable();
            if (Session["SC_WarehouseData"] != null)
            {
                Warehousedt = (DataTable)Session["SC_WarehouseData"];
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
            if (Session["SC_WarehouseData"] != null)
            {
                Warehousedt = (DataTable)Session["SC_WarehouseData"];

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

                Session["SC_WarehouseData"] = Warehousedt;
            }
        }
        //protected void acpAvailableStock_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        //{
        //    string performpara = e.Parameter;
        //    string strProductID = Convert.ToString(performpara.Split('~')[0]);
        //    string strBranch = Convert.ToString(ddl_Branch.SelectedValue);
        //    acpAvailableStock.JSProperties["cpstock"] = "0.00";

        //    try
        //    {
        //        DataTable dt2 = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableQuotation(" + strBranch + ",'" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]) + "'," + strProductID + ") as branchopenstock");

        //        if (dt2.Rows.Count > 0)
        //        {
        //            acpAvailableStock.JSProperties["cpstock"] = Convert.ToString(Math.Round(Convert.ToDecimal(dt2.Rows[0]["branchopenstock"]), 2));
        //        }
        //        else
        //        {
        //            acpAvailableStock.JSProperties["cpstock"] = "0.00";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
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
        public void changeGridOrder()
        {
            string Type = "";
            GetProductType(ref Type);
            if (Type == "W")
            {
                GrdWarehouse.Columns["WarehouseName"].Visible = true;
                GrdWarehouse.Columns["AvailableQty"].Visible = false;
                GrdWarehouse.Columns["BatchNo"].Visible = false;
                GrdWarehouse.Columns["MfgDate"].Visible = false;
                GrdWarehouse.Columns["ExpiryDate"].Visible = false;
                GrdWarehouse.Columns["SerialNo"].Visible = false;
            }
            else if (Type == "WB")
            {
                GrdWarehouse.Columns["WarehouseName"].Visible = true;
                GrdWarehouse.Columns["AvailableQty"].Visible = false;
                GrdWarehouse.Columns["BatchNo"].Visible = true;
                GrdWarehouse.Columns["MfgDate"].Visible = true;
                GrdWarehouse.Columns["ExpiryDate"].Visible = true;
                GrdWarehouse.Columns["SerialNo"].Visible = false;
            }
            else if (Type == "WBS")
            {
                GrdWarehouse.Columns["WarehouseName"].Visible = true;
                GrdWarehouse.Columns["AvailableQty"].Visible = false;
                GrdWarehouse.Columns["BatchNo"].Visible = true;
                GrdWarehouse.Columns["MfgDate"].Visible = true;
                GrdWarehouse.Columns["ExpiryDate"].Visible = true;
                GrdWarehouse.Columns["SerialNo"].Visible = true;
            }
            else if (Type == "B")
            {
                GrdWarehouse.Columns["WarehouseName"].Visible = false;
                GrdWarehouse.Columns["AvailableQty"].Visible = false;
                GrdWarehouse.Columns["BatchNo"].Visible = true;
                GrdWarehouse.Columns["MfgDate"].Visible = true;
                GrdWarehouse.Columns["ExpiryDate"].Visible = true;
                GrdWarehouse.Columns["SerialNo"].Visible = false;
            }
            else if (Type == "S")
            {
                GrdWarehouse.Columns["WarehouseName"].Visible = false;
                GrdWarehouse.Columns["AvailableQty"].Visible = false;
                GrdWarehouse.Columns["BatchNo"].Visible = false;
                GrdWarehouse.Columns["MfgDate"].Visible = false;
                GrdWarehouse.Columns["ExpiryDate"].Visible = false;
                GrdWarehouse.Columns["SerialNo"].Visible = true;
            }
            else if (Type == "WS")
            {
                GrdWarehouse.Columns["WarehouseName"].Visible = true;
                GrdWarehouse.Columns["AvailableQty"].Visible = false;
                GrdWarehouse.Columns["BatchNo"].Visible = false;
                GrdWarehouse.Columns["MfgDate"].Visible = false;
                GrdWarehouse.Columns["ExpiryDate"].Visible = false;
                GrdWarehouse.Columns["SerialNo"].Visible = true;
            }
            else if (Type == "BS")
            {
                GrdWarehouse.Columns["WarehouseName"].Visible = false;
                GrdWarehouse.Columns["AvailableQty"].Visible = false;
                GrdWarehouse.Columns["BatchNo"].Visible = true;
                GrdWarehouse.Columns["MfgDate"].Visible = true;
                GrdWarehouse.Columns["ExpiryDate"].Visible = true;
                GrdWarehouse.Columns["SerialNo"].Visible = true;
            }
        }

        #endregion

        #region Set session For Packing Quantity
        [WebMethod]
        public static string SetSessionPacking(string list)
        {
            System.Web.Script.Serialization.JavaScriptSerializer jsSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            List<ProductQuantity> finalResult = jsSerializer.Deserialize<List<ProductQuantity>>(list);
            HttpContext.Current.Session["SessionPackingDetails"] = finalResult;
            return null;

        }
        #endregion

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


            //else if(strSplitCommand=="BindRemarks")
            //{

            //    DataTable dt = GetOrderData().Tables[1];

            //   //txtInlineRemarks.Text=


            //}

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
            //else if (strSplitCommand=="RemarksDelete")
            //{
            //    DeleteUnsaveaddRemarks(SrlNo, RemarksVal);

            //}


        }
        [WebMethod]
        public static object DocWiseSimilarProjectCheck(string quote_Id, string Doctype)
        {
            string returnValue = "0";
            if (HttpContext.Current.Session["userid"] != null)
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                //quote_Id = quote_Id.Replace("'", "''");

                DataTable dtMainAccount = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_CRMProjectInvoice_Details");
                proc.AddVarcharPara("@Action", 100, "GetSimilarProjectCheckforChallan");
                proc.AddVarcharPara("@TagDocType", 100, Doctype);
                proc.AddVarcharPara("@SelectedComponentList", 2000, quote_Id);
                proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
                proc.RunActionQuery();
                returnValue = Convert.ToString(proc.GetParaValue("@ReturnValue"));

            }
            return returnValue;

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
    }
}