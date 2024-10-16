﻿using BusinessLogicLayer;
using ClsDropDownlistNameSpace;
using DataAccessLayer;
using DevExpress.Web;
using DevExpress.Web.Data;
using EntityLayer.CommonELS;
using ERP.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities
{
    public partial class OldUnitReceivedFromServiceCenter : ERP.OMS.ViewState_class.VSPage
    {
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();


       // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        
        clsDropDownList oclsDropDownList = new clsDropDownList();
        BusinessLogicLayer.Contact oContactGeneralBL = new BusinessLogicLayer.Contact();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        BusinessLogicLayer.RemarkCategoryBL reCat = new BusinessLogicLayer.RemarkCategoryBL();
        BusinessLogicLayer.Others objBL = new BusinessLogicLayer.Others();
        static string ForJournalDate = null;
        SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
        OldUnitAssignReceivedBL oBL = new OldUnitAssignReceivedBL();
        string UniqueQuotation = string.Empty;
        string UniqueQuotationDN = string.Empty;
        public string pageAccess = "";
        string userbranch = "";

        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);


                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //foreach (Control c in Page.Controls)
            //   c.Visible = false;
            //rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/SalesQuotation.aspx");
            if (HttpContext.Current.Session["userid"] == null)
            {
                // Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {


                CountrySelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                StateSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                SelectCity.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                SelectArea.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                SelectPin.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                sqltaxDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                SqlCurrency.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                SqlCurrencyBind.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                ProductDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                SqlSchematype.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                dsBranch.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]); 


                if (Session["userbranchHierarchy"] != null)
                {
                    userbranch = Convert.ToString(Session["userbranchHierarchy"]);
                }
                GetAllDropDownDetailForSalesOrder(userbranch);
                Session["OldUnitStockInvoice"] = null;
                GetFinacialYearBasedQouteDate();
            }
            if (Request.QueryString["key"] != null && Request.QueryString["key"] != "ADD")
            {
                //SetOrderDetails();
                Session["ActionType"] = Convert.ToString(Request.QueryString["Type"]);
                string strOrderId1 = Convert.ToString(Request.QueryString["key"]);
                #region Set Invoice Number
               
                
                //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

                DataTable DT = objEngine.GetDataTable("tbl_trans_SalesInvoice", " Invoice_Number,Invoice_BranchId ", " Invoice_Id = " + Convert.ToInt32(strOrderId1) + "");
                if (DT != null && DT.Rows.Count > 0)
                {
                    string Invoice_Number = Convert.ToString(DT.Rows[0]["Invoice_Number"]).Trim();
                    txt_InvoiceNo.Text = Invoice_Number;
                    PosBranchId.Value = Convert.ToString(DT.Rows[0]["Invoice_BranchId"]).Trim();
                }
                #endregion
                Session["Invoice_ID"] = strOrderId1;
                DataTable OrderEditdt = GetOrderEditDataForBinding();
                if (OrderEditdt != null && OrderEditdt.Rows.Count > 0)
                {
                    string Quoids = Convert.ToString(OrderEditdt.Rows[0]["Service_OutIds"]);
                    //Session["Lookup_QuotationIds"] = Quoids;
                    string Order_Date = Convert.ToString(OrderEditdt.Rows[0]["Issue_Date"]);

                    if (!String.IsNullOrEmpty(Quoids))
                    {
                        string[] eachQuo = Quoids.Split(',');
                        if (eachQuo.Length > 1)//More than one quotation
                        {
                            dt_Quotation.Text = "Multiple Select Quotation Dates";
                            BindLookUp(Order_Date);
                            foreach (string val in eachQuo)
                            {
                                lookup_order.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));

                            }
                            // lbl_MultipleDate.Attributes.Add("style", "display:block");
                        }
                        else if (eachQuo.Length == 1)//Single Quotation
                        { //lbl_MultipleDate.Attributes.Add("style", "display:none"); }
                            BindLookUp(Order_Date);
                            foreach (string val in eachQuo)
                            {
                                lookup_order.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                            }
                        }
                        else//No Quotation selected
                        {
                            BindLookUp(Order_Date);
                        }
                    }
                }

            }
            else
            {
                Session["ActionType"] = "Edit";
            }
            if (!IsPostBack)
            {
                this.Session["LastCompany1"] = Session["LastCompany"];
                this.Session["LastFinYear1"] = Session["LastFinYear"];
                this.Session["userbranch"] = Session["userbranchID"];
                Session["OUR_WarehouseData"] = null;

                if (Request.QueryString["Permission"] != null)
                {
                    if (Convert.ToString(Request.QueryString["Permission"]) == "1")
                    {
                        pnl_quotation.Enabled = true;
                    }
                    else if (Convert.ToString(Request.QueryString["Permission"]) == "2")
                    {
                        pnl_quotation.Enabled = true;
                    }
                    else if (Convert.ToString(Request.QueryString["Permission"]) == "3")
                    {
                        pnl_quotation.Enabled = true;
                    }
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
                SetFinYearCurrentDate();
                
                dt_PlOrderExpiry.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                string finyear = Convert.ToString(Session["LastFinYear"]);
                //GetAllDropDownDetailForSalesOrder();


                PopulateGSTCSTVATCombo(DateTime.Now.ToString("yyyy-MM-dd"));
                PopulateChargeGSTCSTVATCombo(DateTime.Now.ToString("yyyy-MM-dd"));
                //kaushik 24-2-2017
                Session["OldUnitStockInvoiceBillingAddressLookup"] = null;
                Session["OldUnitStockInvoiceShippingAddressLookup"] = null;
                Session["OldUnitStockInvoiceAddressDtl"] = null;
                //kaushik 24-2-2017
                Session["CustomerDetail"] = null;

                Session["OldUnitStockInvoice_LoopWarehouse"] = 1;
                Session["OldUnitStockInvoiceTaxDetails"] = null;

                //Purpose : Binding Batch Edit Grid
                //Name : Sudip 
                // Dated : 21-01-2017
                string strOrderId = "";
                grid.AddNewRow();
                if (Request.QueryString["key"] != null)
                {
                    if (Convert.ToString(Request.QueryString["key"]) != "ADD")
                    {


                        //sqlQuationList.SelectParameters.Clear();
                        //sqlQuationList.SelectParameters.Add("status", "DONE");
                        //lookup_quotation.DataBind();

                        //  bindLookUP("DONE");
                        ltrTitle.Text = "Old Unit Received";
                        strOrderId = Convert.ToString(Request.QueryString["key"]);
                        Session["Invoice_ID"] = strOrderId;
                        //Session["ActionType"] = "Edit";
                        //Session["OUR_WarehouseData"] = GetOrderWarehouseData();
                        Session["OldUnitStockInvoice"] = null; //GetOrderData().Tables[0];
                        #region Subhabrata Get Tax Details in Edit Mode

                        //  Session["SalesChallanTaxDetails"] = GetQuotationTaxData().Tables[0];
                        //from quotation
                        Session["OldUnitStockInvoiceTaxDetails"] = GetTaxDataWithGST(GetTaxData(dt_BTOut.Date.ToString("yyyy-MM-dd")));






                        DataTable quotetable = GetQuotationEditedTaxData().Tables[0];
                        if (quotetable == null)
                        {
                            CreateDataTaxTable();
                        }
                        else
                        {
                            Session["OldUnitStockInvoiceFinalTaxRecord"] = quotetable;
                        }

                        #endregion Debjyoti Get Tax Details in Edit Mode

                        //kaushik 25-2-2017


                        if (Convert.ToString(Request.QueryString["ViewMode"]) == "yes")
                        {
                            DivSelectDN.Style.Add("display", "none");
                           
                            
                           // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

                            DataTable DT = objEngine.GetDataTable("tbl_trans_OldUnitReceived", " Received_DNNote_VoucherId ", " OldUnit_Ids = " + Convert.ToInt32(strOrderId) + "");
                            if (DT != null && DT.Rows.Count > 0)
                            {
                                string Received_DNNote_VoucherId = Convert.ToString(DT.Rows[0]["Received_DNNote_VoucherId"]).Trim();
                                txtDNnumber.Text = Received_DNNote_VoucherId;
                            }
                        }
                        







                        //  Session["KeyVal_InternalID"] = "PISC" + strOrderId;
                        //kaushik 25-2-2017
                        //ddl_numberingScheme.Enabled = false;
                        txt_SlBTOutNo.ClientEnabled = false;
                        lookup_order.ClientEnabled = false;
                        //GetAllDropDownDetailForSalesOrder();
                        if (Session["userbranchHierarchy"] != null)
                        {
                            userbranch = Convert.ToString(Session["userbranchHierarchy"]);
                        }
                        GetAllDropDownDetailForSalesOrder(userbranch);
                        //SetOrderDetails();
                        //grid.DataSource = GetSalesOrder();
                        DataTable dt_QuotationDetails = oBL.GetOldUnitDetailsByInvoice(Convert.ToInt32(Request.QueryString["key"]));
                        Session["OldUnitStockInvoice"] = null;
                        grid.DataSource = GetOldUnitDetailsInfo(dt_QuotationDetails);
                        grid.DataBind();

                        if (Request.QueryString["ViewMode"] == "yes")
                        {
                            btnWarehouse.Visible = false;
                            Session["OUR_WarehouseData"] = GetChallanWarehouseData();
                        }
                        else
                        {
                            Session["OUR_WarehouseData"] = GetChallanWarehouseData(); 
                            btnWarehouse.Visible = true;
                        }

                        //kaushik 24-2-2017
                        Session["OldUnitStockInvoiceAddressDtl"] = GetBillingAddress();
                        //kaushik 24-2-2017
                        hdnmodeId.Value = "BranchStockIn" + strOrderId;
                        //lookup_quotation.Properties.PopupFilterMode = DevExpress.XtraEditors.Popup.TreeListLookUpEditPopupForm.Contains;


                        hdnPageStatus.Value = "update";
                    }
                    else
                    {

                        //sqlQuationList.SelectParameters.Clear();
                        //sqlQuationList.SelectParameters.Add("status", "NOT-DONE");
                        //lookup_quotation.DataBind();
                        // bindLookUP("NOT-DONE");
                        ltrTitle.Text = "Add - Receive From Service Center";
                        //Session["ActionType"] = "Add";
                        hdnPageStatus.Value = "first";
                        CreateDataTaxTable();
                        hdnmodeId.Value = "Add";
                    }
                }

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GridCallBack()", true);


                IsUdfpresent.Value = Convert.ToString(getUdfCount());
            }


            GetSalesOrderSchemaLength();
        }
        public DataTable GetQuotationWarehouseData(string key)
        {
            try
            {
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_CRMSalesInvoice_Details");
                proc.AddVarcharPara("@Action", 500, "InvoiceWarehouse");
                proc.AddVarcharPara("@InvoiceID", 500, Convert.ToString(key));
                dt = proc.GetTable();

                string strNewVal = "", strOldVal = "";
                DataTable tempdt = dt.Copy();
                foreach (DataRow drr in tempdt.Rows)
                {
                    strNewVal = Convert.ToString(drr["QuoteWarehouse_Id"]);

                    if (strNewVal == strOldVal)
                    {
                        drr["WarehouseName"] = "";
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

                Session["SI_LoopWarehouse"] = Convert.ToString(Convert.ToInt32(strNewVal) + 1);
                tempdt.Columns.Remove("QuoteWarehouse_Id");
                return tempdt;
            }
            catch
            {
                return null;
            }
        }
        protected int getUdfCount()
        {
            DataTable udfCount = oDBEngine.GetDataTable("select 1 from tbl_master_remarksCategory rc where cat_applicablefor='BSO'  and ( exists (select * from tbl_master_udfGroup where id=rc.cat_group_id and grp_isVisible=1) or rc.cat_group_id=0)");
            return udfCount.Rows.Count;
        }
        public void GetSalesOrderSchemaLength()
        {
            DataTable Dt = new DataTable();
            Dt = objBL.GetSchemaLengthSalesOrder();
            if (Dt != null && Dt.Rows.Count > 0)
            {
                hdnSchemaLength.Value = Convert.ToString(Dt.Rows[0]["length"]);

            }

        }
        public void SetFinYearCurrentDate()
        {
            dt_BTOut.EditFormatString = objConverter.GetDateFormat("Date");
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

            dt_BTOut.Value = DateTime.ParseExact(fDate, @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        }
        

        public DataTable GetNumberingSchemaDN(string strCompanyID, string strBranchID, string strFinYear, string strType, string strIsSplit)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PROC_OLDUNITDETAILS");
            proc.AddVarcharPara("@Action", 100, "GetDnNumbering");
            proc.AddVarcharPara("@CompanyID", 100, strCompanyID);
            //proc.AddVarcharPara("@BranchID", 100, strBranchID);
            proc.AddVarcharPara("@BranchID", 4000, strBranchID);
            proc.AddVarcharPara("@FinYear", 100, strFinYear);
            proc.AddVarcharPara("@Type", 100, strType);
            proc.AddVarcharPara("@IsSplit", 100, strIsSplit);
            ds = proc.GetTable();
            return ds;
        }



        public void GetAllDropDownDetailForSalesOrder(string UserBranch)
        {
            DataSet dst = new DataSet();
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string strBranchID = Convert.ToString(Session["userbranchID"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            dsBranch.SelectCommand = "SELECT BRANCH_id AS BANKBRANCH_ID , RTRIM(BRANCH_DESCRIPTION)+' ['+ISNULL(RTRIM(BRANCH_CODE),'')+']' AS BANKBRANCH_NAME  FROM TBL_MASTER_BRANCH where BRANCH_id in(" + Convert.ToString(Session["userbranchHierarchy"]) + ")";
            ddlDnBranch.DataBind();
            //SlaesActivitiesBL objSlaesActivitiesBL1 = new SlaesActivitiesBL();
            //DataTable Schemadt = objSlaesActivitiesBL1.GetNumberingSchema(strCompanyID, strBranchID, FinYear, "36", "Y");

            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            dst = objSlaesActivitiesBL.GetAllDropDownDetailForSalesChallan(UserBranch);
            DataTable Schemadt = objSlaesActivitiesBL.GetNumberingSchema(strCompanyID, userbranchHierarchy, FinYear, "36", "Y");
            DataTable CNSchemadt = objSlaesActivitiesBL.GetNumberingSchema(strCompanyID, PosBranchId.Value, FinYear, "25", "Y");
            if (dst.Tables[0] != null && dst.Tables[0].Rows.Count > 0)
            {
                //ddl_numberingScheme.DataTextField = "SchemaName";
                //ddl_numberingScheme.DataValueField = "Id";
                //ddl_numberingScheme.DataSource = dst.Tables[0];
                //ddl_numberingScheme.DataBind();
            }
            if (dst.Tables[1] != null && dst.Tables[1].Rows.Count > 0)
            {
                ddl_transferFrom_Branch.DataTextField = "branch_description";
                ddl_transferFrom_Branch.DataValueField = "branch_id";
                ddl_transferFrom_Branch.DataSource = dst.Tables[1];
                ddl_transferFrom_Branch.DataBind();
                ddl_transferFrom_Branch.Items.Insert(0, new ListItem("Select", "0"));

                //ddl_transferTo_Branch.DataTextField = "branch_description";
                //ddl_transferTo_Branch.DataValueField = "branch_id";
                //ddl_transferTo_Branch.DataSource = dst.Tables[1];
                //ddl_transferTo_Branch.DataBind();
                //ddl_transferTo_Branch.Items.Insert(0, new ListItem("Select", "0"));
            }
            if (Schemadt != null && Schemadt.Rows.Count > 0)
            {
                ddl_numberingScheme.DataTextField = "SchemaName";
                ddl_numberingScheme.DataValueField = "Id";
                ddl_numberingScheme.DataSource = Schemadt;
                ddl_numberingScheme.DataBind();
            }
            if (CNSchemadt != null && CNSchemadt.Rows.Count > 0)
            {
                ddlDNnumbering.DataTextField = "SchemaName";
                ddlDNnumbering.DataValueField = "Id";
                ddlDNnumbering.DataSource = CNSchemadt;
                ddlDNnumbering.DataBind();
            }
            if (dst.Tables[2] != null && dst.Tables[2].Rows.Count > 0)
            {

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

            //if (dst.Tables[8] != null && dst.Tables[8].Rows.Count > 0)
            //{
            //    ddl_ServiceCenter.DataTextField = "Name";
            //    ddl_ServiceCenter.DataValueField = "Id";
            //    ddl_ServiceCenter.DataSource = dst.Tables[8];
            //    ddl_ServiceCenter.DataBind();
            //    ddl_ServiceCenter.Items.Insert(0, new ListItem("Select", "0"));
            //}
            DataTable DT_OldUnitDetailsByInvoice = oBL.GetOldUnitByInvoice(Convert.ToInt32(Request.QueryString["key"]));
            DataTable DT_OldUnitReceivedByInvoice = oBL.GetOldUnitReceivedByInvoice(Convert.ToInt32(Request.QueryString["key"]));

            if (DT_OldUnitDetailsByInvoice != null && DT_OldUnitDetailsByInvoice.Rows.Count > 0)
            {
                if (ddl_transferFrom_Branch.Items.Count > 0)
                {
                    if (DT_OldUnitReceivedByInvoice != null && DT_OldUnitReceivedByInvoice.Rows.Count > 0)
                    {
                        txt_SlBTOutNo.Text = Convert.ToString(DT_OldUnitReceivedByInvoice.Rows[0]["Received_ReceivedNumber"]).Trim();
                    }
                    txt_Refference.Text = Convert.ToString(DT_OldUnitDetailsByInvoice.Rows[0]["receiver_remark"]).Trim();
                    if (DT_OldUnitDetailsByInvoice.Rows[0]["Received_On"] != null && Convert.ToString(DT_OldUnitDetailsByInvoice.Rows[0]["Received_On"]).Trim() != "")
                    {
                        dt_OADate.Value = (Convert.ToDateTime(Convert.ToString(DT_OldUnitDetailsByInvoice.Rows[0]["Received_On"]).Trim()));
                    }
                    ddl_transferFrom_Branch.Enabled = false;
                    int branchindex = 0;
                    int cnt = 0;
                    foreach (ListItem li in ddl_transferFrom_Branch.Items)
                    {
                        if (li.Value == Convert.ToString(DT_OldUnitDetailsByInvoice.Rows[0]["assign_to_branch"]))
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
                        ddl_transferFrom_Branch.SelectedIndex = branchindex;
                    }
                    else
                    {
                        ddl_transferFrom_Branch.SelectedIndex = cnt;
                    }
                }
            }
            //if (Session["userbranchID"] != null)
            //{
            //    if (ddl_transferFrom_Branch.Items.Count > 0)
            //    {
            //        int branchindex = 0;
            //        int cnt = 0;
            //        foreach (ListItem li in ddl_transferFrom_Branch.Items)
            //        {
            //            if (li.Value == Convert.ToString(Session["userbranchID"]))
            //            {
            //                cnt = 1;
            //                break;
            //            }
            //            else
            //            {
            //                branchindex += 1;
            //            }
            //        }
            //        if (cnt == 1)
            //        {
            //            ddl_transferFrom_Branch.SelectedIndex = branchindex;
            //        }
            //        else
            //        {
            //            ddl_transferFrom_Branch.SelectedIndex = cnt;
            //        }
            //    }
            //}

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
                //PopulateContactPersonOfCustomer(InternalId);
            }

            //if (WhichCall == "BindQuotation")
            //{

            //    string status = string.Empty;
            //    string customer = string.Empty;
            //    string OrderDate = string.Empty;

            //    if (Convert.ToString(Request.QueryString["key"]) != "ADD")
            //    {
            //        status = "DONE";
            //    }
            //    else
            //    {
            //        status = "NOT-DONE";
            //    }

            //    if (e.Parameter.Split('~')[1] != null)
            //    { customer = e.Parameter.Split('~')[1]; }
            //    if (e.Parameter.Split('~')[2] != null)
            //    { OrderDate = e.Parameter.Split('~')[2]; }


            //    DataTable QuotationTable;


            //    //   string[] taxconfigDetails = oDBEngine.GetFieldValue1("Config_TaxRates", "TaxRates_TaxCode,TaxRatesSchemeName", "TaxRates_ID=" + code, 2);
            //    //productTable = oDBEngine.GetDataTable("select sProducts_ID,sProducts_Code,sProducts_Name from Master_sProducts where sProducts_ID not in (select prodId from tbl_trans_ProductTaxRate where TaxRates_TaxCode<>" + taxconfigDetails[0] + " and TaxRatesSchemeName<>'" + taxconfigDetails[1] + "' )");
            //    //GridLookup.DataSource = productTable;
            //    //GridLookup.DataBind();
            //    QuotationTable = objBL.GetQuotationOnSalesOrder(customer, OrderDate, status);
            //    lookup_quotation.GridView.Selection.CancelSelection();
            //    lookup_quotation.DataSource = QuotationTable;
            //    lookup_quotation.DataBind();



            //    Session["QuotationData"] = QuotationTable;
            //}
        }
        protected void ddl_VatGstCst_Callback(object sender, CallbackEventArgsBase e)
        {
            string type = e.Parameter.Split('~')[0];
            PopulateGSTCSTVAT(type);
        }
        protected void PopulateGSTCSTVAT(string type)
        {
            DataTable dtGSTCSTVAT = new DataTable();
            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            if (type == "2")
            {
                dtGSTCSTVAT = objSlaesActivitiesBL.PopulateGSTCSTVAT();
                if (dtGSTCSTVAT != null && dtGSTCSTVAT.Rows.Count > 0)
                {
                    ddl_VatGstCst.TextField = "Taxes_Name";
                    ddl_VatGstCst.ValueField = "Taxes_ID";
                    ddl_VatGstCst.DataSource = dtGSTCSTVAT;
                    ddl_VatGstCst.DataBind();
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

        #region  Billing and Shipping Detail

        string[,] GetState(int country)
        {
            StateSelect.SelectParameters[0].DefaultValue = Convert.ToString(country);
            DataView view = (DataView)StateSelect.Select(DataSourceSelectArguments.Empty);
            string[,] DATA = new string[view.Count, 2];
            for (int i = 0; i < view.Count; i++)
            {
                DATA[i, 0] = Convert.ToString(view[i][0]);
                DATA[i, 1] = Convert.ToString(view[i][1]);
            }
            return DATA;

        }
        protected void FillStateCombo(ASPxComboBox cmb, int country)
        {

            string[,] state = GetState(country);
            cmb.Items.Clear();

            for (int i = 0; i < state.GetLength(0); i++)
            {
                cmb.Items.Add(state[i, 1], state[i, 0]);
            }
            cmb.Items.Insert(0, new ListEditItem("Select", "0"));
        }
        string[,] GetCities(int state)
        {


            SelectCity.SelectParameters[0].DefaultValue = Convert.ToString(state);
            DataView view = (DataView)SelectCity.Select(DataSourceSelectArguments.Empty);
            string[,] DATA = new string[view.Count, 2];
            for (int i = 0; i < view.Count; i++)
            {
                DATA[i, 0] = Convert.ToString(view[i][0]);
                DATA[i, 1] = Convert.ToString(view[i][1]);
            }
            return DATA;

        }
        protected void FillCityCombo(ASPxComboBox cmb, int state)
        {

            string[,] cities = GetCities(state);
            cmb.Items.Clear();

            for (int i = 0; i < cities.GetLength(0); i++)
            {
                cmb.Items.Add(cities[i, 1], cities[i, 0]);
            }
            //cmb.Items.Insert(0, new ListEditItem("Select", "0"));
        }
        protected void FillPinCombo(ASPxComboBox cmb, int city)
        {
            string[,] pin = GetPin(city);
            cmb.Items.Clear();

            for (int i = 0; i < pin.GetLength(0); i++)
            {
                cmb.Items.Add(pin[i, 1], pin[i, 0]);
            }

        }
        string[,] GetPin(int city)
        {
            SelectPin.SelectParameters[0].DefaultValue = Convert.ToString(city);
            DataView view = (DataView)SelectPin.Select(DataSourceSelectArguments.Empty);
            string[,] DATA = new string[view.Count, 2];
            for (int i = 0; i < view.Count; i++)
            {
                DATA[i, 0] = Convert.ToString(view[i][0]);
                DATA[i, 1] = Convert.ToString(view[i][1]);
            }
            return DATA;
        }
        string[,] GetArea(int city)
        {
            SelectArea.SelectParameters[0].DefaultValue = Convert.ToString(city);
            DataView view = (DataView)SelectArea.Select(DataSourceSelectArguments.Empty);
            string[,] DATA = new string[view.Count, 2];
            for (int i = 0; i < view.Count; i++)
            {
                DATA[i, 0] = Convert.ToString(view[i][0]);
                DATA[i, 1] = Convert.ToString(view[i][1]);
            }
            return DATA;
        }
        protected void FillAreaCombo(ASPxComboBox cmb, int city)
        {
            string[,] area = GetArea(city);
            cmb.Items.Clear();

            for (int i = 0; i < area.GetLength(0); i++)
            {
                cmb.Items.Add(area[i, 1], area[i, 0]);
            }
            //cmb.Items.Insert(0, new ListEditItem("Select", "0"));
        }
        protected void cmbCity_OnCallback(object source, CallbackEventArgsBase e)
        {
            if (e.Parameter != "")
            {
                FillCityCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
            }
        }
        protected void cmbState_OnCallback(object source, CallbackEventArgsBase e)
        {
            if (e.Parameter != "")
            {
                FillStateCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
            }
        }
        protected void cmbState1_OnCallback(object source, CallbackEventArgsBase e)
        {
            if (e.Parameter != "")
            {
                FillStateCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
            }
        }
        protected void cmbCity1_OnCallback(object source, CallbackEventArgsBase e)
        {
            if (e.Parameter != "")
            {
                FillCityCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
            }
        }
        protected void cmbArea_OnCallback(object source, CallbackEventArgsBase e)
        {
            if (e.Parameter != "")
            {
                FillAreaCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
            }
        }

        protected void DnCallBack_Callback(object source,CallbackEventArgs e)
        {
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string strBranchID = Convert.ToString(Session["userbranchID"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            DataTable CNSchemadt = GetNumberingSchemaDN(strCompanyID, userbranchHierarchy, FinYear, "25", "Y");
            if (CNSchemadt != null && CNSchemadt.Rows.Count > 0)
            {
                ddlDNnumbering.DataTextField = "SchemaName";
                ddlDNnumbering.DataValueField = "Id";
                ddlDNnumbering.DataSource = CNSchemadt;
                ddlDNnumbering.DataBind();
            }
        }
        protected void cmbArea1_OnCallback(object source, CallbackEventArgsBase e)
        {
            if (e.Parameter != "")
            {
                FillAreaCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
            }

        }
        protected void cmbPin_OnCallback(object source, CallbackEventArgsBase e)
        {
            if (e.Parameter != "")
            {
                FillPinCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
            }
        }
        protected void cmbPin1_OnCallback(object source, CallbackEventArgsBase e)
        {
            if (e.Parameter != "")
            {
                FillPinCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
            }
        }

        #endregion

        #region Batch Edit Grid Function

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
            public Int64 Service_Id { get; set; }
            public string Quotation_Num { get; set; }
            public string Key_UniqueId { get; set; }
            public string Product_Shortname { get; set; }
            public string Order_Num { get; set; }
            public string ProductName { get; set; }
            public string OrderDetails_Id { get; set; }
            public string Indent { get; set; }
            public string StkDetails_Id { get; set; }
            public string Indent_UniqueId { get; set; }
        }
        public IEnumerable GetProduct()
        {
            List<Product> ProductList = new List<Product>();

            //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
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
            ProcedureExecute proc = new ProcedureExecute("prc_SalesChallan_Details");
            proc.AddVarcharPara("@Action", 500, "ProductDetails");
            ds = proc.GetDataSet();
            return ds;
        }
        public IEnumerable GetTaxes(DataTable DT)
        {
            List<Taxes> TaxList = new List<Taxes>();

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

                    TaxList.Add(Taxes);
                }
            }

            return TaxList;
        }
        public DataTable GetTaxData(string quoteDate)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesChallan_Details");
            proc.AddVarcharPara("@Action", 500, "TaxDetails");
            proc.AddVarcharPara("@Challan_Id", 500, Convert.ToString(Session["Invoice_ID"]) == "ADD" ? "" : Convert.ToString(Session["Invoice_ID"]));
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@branchId", 500, Convert.ToString(Session["userbranchID"]));
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
                Orders.UOM = Convert.ToString(Orderdt.Rows[i]["oldUnit_Uom"]);
                Orders.Warehouse = "";
                Orders.StockQuantity = Convert.ToString(Orderdt.Rows[i]["StockQuantity"]);
                Orders.StockUOM = Convert.ToString(Orderdt.Rows[i]["StockUOM"]);
                Orders.SalePrice = Convert.ToString(Orderdt.Rows[i]["Rate"]);
                Orders.Discount = "0";
                Orders.Amount = Convert.ToString(Orderdt.Rows[i]["Amount"]);
                Orders.TotalAmount = Convert.ToString(Orderdt.Rows[i]["TotalAmount"]);
                if (!string.IsNullOrEmpty(Convert.ToString(Orderdt.Rows[i]["Service_OutId"])))//Added on 15-02-2017
                { Orders.Service_Id = Convert.ToInt64(Orderdt.Rows[i]["Service_OutId"]); }
                else
                { Orders.Service_Id = 0; }
                if (dtC.Contains("Indent"))
                {
                    Orders.Order_Num = Convert.ToString(Orderdt.Rows[i]["Indent"]);//subhabrata on 21-02-2017
                }
                Orders.ProductName = Convert.ToString(Orderdt.Rows[i]["ProductName"]);

            }
            BindSessionByDatatableForgridBind(Orderdt);
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
                Orders.Indent_UniqueId = Convert.ToString(SalesOrderdt.Rows[i]["OrderID"]);
                Orders.ProductID = Convert.ToString(SalesOrderdt.Rows[i]["ProductID"]);
                Orders.Description = Convert.ToString(SalesOrderdt.Rows[i]["Description"]);
                Orders.Quantity = Convert.ToString(SalesOrderdt.Rows[i]["Quantity"]);
                Orders.UOM = Convert.ToString(SalesOrderdt.Rows[i]["oldUnit_Uom"]);
                Orders.Warehouse = "";
                Orders.StockQuantity = Convert.ToString(SalesOrderdt.Rows[i]["StockQuantity"]);
                Orders.StockUOM = Convert.ToString(SalesOrderdt.Rows[i]["StockUOM"]);
                Orders.SalePrice = Convert.ToString(SalesOrderdt.Rows[i]["SalePrice"]);
                Orders.Discount = Convert.ToString(SalesOrderdt.Rows[i]["Discount"]);
                Orders.Amount = Convert.ToString(SalesOrderdt.Rows[i]["Amount"]);
                Orders.TaxAmount = Convert.ToString(SalesOrderdt.Rows[i]["TaxAmount"]);
                Orders.TotalAmount = Convert.ToString(SalesOrderdt.Rows[i]["TotalAmount"]);
                if (!string.IsNullOrEmpty(Convert.ToString(SalesOrderdt.Rows[i]["Service_Id"])))//Added on 15-02-2017
                { Orders.Service_Id = Convert.ToInt64(SalesOrderdt.Rows[i]["Service_Id"]); }
                else
                { Orders.Service_Id = 0; }
                if (dtC.Contains("Indent"))
                {
                    Orders.Order_Num = Convert.ToString(SalesOrderdt.Rows[i]["Indent"]);//subhabrata on 21-02-2017
                }
                Orders.ProductName = Convert.ToString(SalesOrderdt.Rows[i]["ProductName"]);

                OrderList.Add(Orders);
            }


            return OrderList;
        }
        public IEnumerable GetSalesOrderInfo(DataTable SalesOrderdt1, string Order_Id)
        {
            List<SalesOrder> OrderList = new List<SalesOrder>();
            DataColumnCollection dtC = SalesOrderdt1.Columns;

            for (int i = 0; i < SalesOrderdt1.Rows.Count; i++)
            {
                SalesOrder Orders = new SalesOrder();

                Orders.SrlNo = Convert.ToString(i + 1);
                Orders.Indent_UniqueId = Convert.ToString(i + 1);
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

                if (!string.IsNullOrEmpty(Convert.ToString(SalesOrderdt1.Rows[i]["Stk_Id"])))//Added on 15-02-2017
                { Orders.Service_Id = Convert.ToInt64(SalesOrderdt1.Rows[i]["Stk_Id"]); }
                else
                { Orders.Service_Id = 0; }
                if (dtC.Contains("Indent"))
                {
                    Orders.Order_Num = Convert.ToString(SalesOrderdt1.Rows[i]["Indent"]);//subhabrata on 21-02-2017
                }
                Orders.ProductName = Convert.ToString(SalesOrderdt1.Rows[i]["ProductName"]);
                OrderList.Add(Orders);


            }

            BindSessionByDatatable(SalesOrderdt1);

            return OrderList;
        }
        public IEnumerable GetOldUnitDetailsInfo(DataTable DT_OldUnit)
        {
            List<tbl_trans_Oldunit_details> OldUnitDetailsList = new List<tbl_trans_Oldunit_details>();
            DataColumnCollection dtC = DT_OldUnit.Columns;

            for (int i = 0; i < DT_OldUnit.Rows.Count; i++)
            {
                tbl_trans_Oldunit_details Oldunit_details = new tbl_trans_Oldunit_details();

                Oldunit_details.SrlNo = Convert.ToString(i + 1);
                Oldunit_details.oldUnit_id = Convert.ToInt32(DT_OldUnit.Rows[i]["oldUnit_id"]);
                Oldunit_details.doc_id = Convert.ToInt32(DT_OldUnit.Rows[i]["doc_id"]);
                Oldunit_details.ProductID = Convert.ToString(DT_OldUnit.Rows[i]["ProductID"]);
                Oldunit_details.oldUnit_qty = Convert.ToInt32(DT_OldUnit.Rows[i]["oldUnit_qty"]);
                Oldunit_details.oldUnit_Uom = Convert.ToInt32(DT_OldUnit.Rows[i]["oldUnit_Uom"]);
                Oldunit_details.createDate = DateTime.Now;
                Oldunit_details.CreatedBy = Convert.ToString(DT_OldUnit.Rows[i]["CreatedBy"]).Equals("") ? 0 : Convert.ToInt32(DT_OldUnit.Rows[i]["CreatedBy"]);
                Oldunit_details.LastModified = DateTime.Now;
                Oldunit_details.sProducts_Name = Convert.ToString(DT_OldUnit.Rows[i]["sProducts_Name"]);
                Oldunit_details.sProducts_Description = Convert.ToString(DT_OldUnit.Rows[i]["sProducts_Description"]);
                Oldunit_details.oldUnit_value = Convert.ToDecimal(DT_OldUnit.Rows[i]["oldUnit_value"]);
                Oldunit_details.UOM_Name = Convert.ToString(DT_OldUnit.Rows[i]["UOM_Name"]);
                Oldunit_details.Quantity_Received = Convert.ToString(DT_OldUnit.Rows[i]["Quantity_Received"]);
                Oldunit_details.Balance_Quantity = Convert.ToString(DT_OldUnit.Rows[i]["Balance_Quantity"]);
                OldUnitDetailsList.Add(Oldunit_details);
            }

            BindSessionByDatatable(DT_OldUnit);

            return OldUnitDetailsList;
        }

        #region Subhabrata/SessionBind

        //Subhabrata on 02-03-2017
        public bool BindSessionByDatatable(DataTable dt)
        {
            bool IsSuccess = false;
            DataTable OldUnitChalladt = new DataTable();
            OldUnitChalladt.Columns.Add("SrlNo", typeof(int));
            OldUnitChalladt.Columns.Add("oldUnit_id", typeof(int));
            OldUnitChalladt.Columns.Add("doc_id", typeof(int));
            OldUnitChalladt.Columns.Add("ProductID", typeof(string));
            OldUnitChalladt.Columns.Add("oldUnit_qty", typeof(string));
            OldUnitChalladt.Columns.Add("oldUnit_Uom", typeof(int));
            //OldUnitChalladt.Columns.Add("createDate", typeof(string));
            OldUnitChalladt.Columns.Add("CreatedBy", typeof(string));
            OldUnitChalladt.Columns.Add("LastModified", typeof(string));
            OldUnitChalladt.Columns.Add("sProducts_Name", typeof(string));
            OldUnitChalladt.Columns.Add("sProducts_Description", typeof(string));
            OldUnitChalladt.Columns.Add("Status", typeof(string));
            OldUnitChalladt.Columns.Add("oldUnit_value", typeof(decimal));
            OldUnitChalladt.Columns.Add("UOM_Name", typeof(string));
            OldUnitChalladt.Columns.Add("Quantity_Received", typeof(string));
            OldUnitChalladt.Columns.Add("Balance_Quantity", typeof(string));


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IsSuccess = true;
                DataColumnCollection dtC = dt.Columns;
                int oldUnit_id, doc_id, oldUnit_Uom, CreatedBy;
                decimal oldUnit_value;
                string createDate = string.Empty;
                string LastModified = string.Empty;
                string sProducts_Name = string.Empty;
                string sProducts_Description = string.Empty;
                string oldUnit_qty = string.Empty;
                string ProductID = string.Empty;
                string UOM_Name = string.Empty;
                string Quantity_Received = string.Empty;
                string Balance_Quantity = string.Empty;

                oldUnit_id = Convert.ToInt32(dt.Rows[i]["oldUnit_id"]);
                doc_id = Convert.ToInt32(dt.Rows[i]["doc_id"]);
                ProductID = Convert.ToString(dt.Rows[i]["ProductID"]);
                oldUnit_qty = Convert.ToString(dt.Rows[i]["oldUnit_qty"]);
                oldUnit_Uom = Convert.ToInt32(dt.Rows[i]["oldUnit_Uom"]);
                LastModified = Convert.ToString(dt.Rows[i]["LastModified"]);
                //createDate = Convert.ToString(dt.Rows[i]["createDate"]);
                //CreatedBy = Convert.ToInt32(dt.Rows[i]["CreatedBy"]);
                CreatedBy = Convert.ToString(dt.Rows[i]["CreatedBy"]).Equals("") ? 0 : Convert.ToInt32(dt.Rows[i]["CreatedBy"]);
                sProducts_Name = Convert.ToString(dt.Rows[i]["sProducts_Name"]);
                sProducts_Description = Convert.ToString(dt.Rows[i]["sProducts_Description"]);
                oldUnit_value = Convert.ToDecimal(dt.Rows[i]["oldUnit_value"]);
                UOM_Name = Convert.ToString(dt.Rows[i]["UOM_Name"]);
                Quantity_Received = Convert.ToString(dt.Rows[i]["Quantity_Received"]);
                Balance_Quantity = Convert.ToString(dt.Rows[i]["Balance_Quantity"]);

                OldUnitChalladt.Rows.Add(Convert.ToString(i + 1), oldUnit_id, doc_id, ProductID, oldUnit_qty, oldUnit_Uom, CreatedBy, LastModified, sProducts_Name, sProducts_Description, "U", oldUnit_value, UOM_Name, Quantity_Received, Balance_Quantity);
            }
            Session["OldUnitStockInvoice"] = OldUnitChalladt;

            return IsSuccess;
        }


        public bool BindSessionByDatatableForgridBind(DataTable dt)
        {
            bool IsSuccess = false;
            DataTable SalesChalladt = new DataTable();
            SalesChalladt.Columns.Add("SrlNo", typeof(string));
            SalesChalladt.Columns.Add("OrderID", typeof(string));
            SalesChalladt.Columns.Add("ProductID", typeof(string));
            SalesChalladt.Columns.Add("Description", typeof(string));
            //SalesOrderdt.Columns.Add("Quotation", typeof(string));//Added By:subhabrata on 21-02-2017               
            SalesChalladt.Columns.Add("Quantity", typeof(string));
            SalesChalladt.Columns.Add("oldUnit_Uom", typeof(string));
            SalesChalladt.Columns.Add("Warehouse", typeof(string));
            SalesChalladt.Columns.Add("StockQuantity", typeof(string));
            SalesChalladt.Columns.Add("StockUOM", typeof(string));
            SalesChalladt.Columns.Add("SalePrice", typeof(string));
            SalesChalladt.Columns.Add("Discount", typeof(string));
            SalesChalladt.Columns.Add("Amount", typeof(string));
            SalesChalladt.Columns.Add("TaxAmount", typeof(string));
            SalesChalladt.Columns.Add("TotalAmount", typeof(string));
            SalesChalladt.Columns.Add("Status", typeof(string));
            SalesChalladt.Columns.Add("Service_Id", typeof(string));
            SalesChalladt.Columns.Add("Indent", typeof(string));
            SalesChalladt.Columns.Add("ProductName", typeof(string));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IsSuccess = true;
                DataColumnCollection dtC = dt.Columns;
                string SalePrice, Discount, Amount, TaxAmount, TotalAmount, Order_Num1, ProductName;
                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Rate"])))
                { SalePrice = Convert.ToString(dt.Rows[i]["Rate"]); }
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
                if (dtC.Contains("Indent"))
                {
                    Order_Num1 = Convert.ToString(dt.Rows[i]["Indent"]);//subhabrata on 21-02-2017
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
                    Convert.ToString(dt.Rows[i]["Quantity"]), Convert.ToString(dt.Rows[i]["oldUnit_Uom"]), "", Convert.ToString(dt.Rows[i]["StockQuantity"]), Convert.ToString(dt.Rows[i]["oldUnit_Uom"]),
                               SalePrice, Discount, Amount, TaxAmount, TotalAmount, "U", Convert.ToInt64(dt.Rows[i]["Service_OutId"]), Order_Num1, ProductName);
            }

            Session["OldUnitStockInvoice"] = SalesChalladt;

            return IsSuccess;
        }
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
                if (!string.IsNullOrEmpty(Convert.ToString(SalesOrderdt1.Rows[i]["Stk_Id"])))
                { Orders.Service_Id = Convert.ToInt64(SalesOrderdt1.Rows[i]["Stk_Id"]); }
                else
                { Orders.Service_Id = 0; }
                Orders.ProductID = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_ProductId"]);
                Orders.Description = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_ProductDescription"]);
                Orders.Quantity = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_Quantity"]);
                Orders.Indent = Convert.ToString(SalesOrderdt1.Rows[i]["Indent"]);
                Orders.Product_Shortname = Convert.ToString(SalesOrderdt1.Rows[i]["Product_Name"]);
                Orders.StkDetails_Id = Convert.ToString(SalesOrderdt1.Rows[i]["SalesOrder_Id"]);
                OrderList.Add(Orders);
            }

            return OrderList;
        }

        #endregion

        public DataSet GetOrderData()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesChallan_Details");
            proc.AddVarcharPara("@Action", 500, "ReceivedOldQty");
            proc.AddVarcharPara("@Order_Id", 500, Convert.ToString(Session["Invoice_ID"]));
            ds = proc.GetDataSet();
            return ds;
        }



        protected void grid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "Description")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "oldUnit_Uom")
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
            else if (e.Column.FieldName == "Balance_Quantity")
            {
                e.Editor.Enabled = true;
                e.Editor.ReadOnly = true;
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

            if (Session["OldUnitStockInvoice"] != null)
            {
                //subhabrata: foreach@deleted extra column 
                DataTable dt = new DataTable();
                dt = (DataTable)Session["OldUnitStockInvoice"];
                foreach (DataRow row in dt.Rows)
                {
                    DataColumnCollection dtC = dt.Columns;

                    if (dtC.Contains("Indent"))
                    { dt.Columns.Remove("Indent"); }
                    break;
                }

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
                SalesOrderdt.Columns.Add("oldUnit_Uom", typeof(string));
                SalesOrderdt.Columns.Add("Warehouse", typeof(string));
                SalesOrderdt.Columns.Add("StockQuantity", typeof(string));
                SalesOrderdt.Columns.Add("StockUOM", typeof(string));
                SalesOrderdt.Columns.Add("SalePrice", typeof(string));
                SalesOrderdt.Columns.Add("Discount", typeof(string));
                SalesOrderdt.Columns.Add("Amount", typeof(string));
                SalesOrderdt.Columns.Add("TaxAmount", typeof(string));
                SalesOrderdt.Columns.Add("TotalAmount", typeof(string));
                SalesOrderdt.Columns.Add("Status", typeof(string));
                SalesOrderdt.Columns.Add("Stk_Id", typeof(string));
                SalesOrderdt.Columns.Add("ProductName", typeof(string));
                SalesOrderdt.Columns.Add("Quantity_Received", typeof(string));
                SalesOrderdt.Columns.Add("Balance_Quantity", typeof(string));
            }


            foreach (var args in e.UpdateValues)
            {
                string SrlNo = Convert.ToString(args.NewValues["SrlNo"]);
                string oldUnit_id = Convert.ToString(args.Keys["oldUnit_id"]);
                //Session["Invoice_ID"] = OrderID;
                string ProductDetails = Convert.ToString(args.NewValues["ProductID"]);


                bool isDeleted = false;
                foreach (var arg in e.DeleteValues)
                {
                    string DeleteID = Convert.ToString(arg.Keys["Indent_UniqueId"]);
                    if (DeleteID == oldUnit_id)
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

                        string sProducts_Description = Convert.ToString(args.NewValues["sProducts_Description"]);
                        string oldUnit_qty = Convert.ToString(args.NewValues["oldUnit_qty"]);
                        //string ProductID = Convert.ToString(ProductDetailsList[0]);
                        string ProductID = ProductDetails;
                        string sProducts_Name = Convert.ToString(args.NewValues["sProducts_Name"]);
                        string oldUnit_value = Convert.ToString(args.NewValues["oldUnit_value"]);
                        int oldUnit_Uom = Convert.ToInt32(args.NewValues["oldUnit_Uom"]);
                        string Balance_Quantity = Convert.ToString(args.NewValues["Balance_Quantity"]);
                        string Quantity_Received = Convert.ToString(args.NewValues["Quantity_Received"]);


                        bool Isexists = false;
                        foreach (DataRow drr in SalesOrderdt.Rows)
                        {
                            string oldUnit_ID = Convert.ToString(drr["oldUnit_id"]);

                            if (oldUnit_ID == oldUnit_id)
                            {
                                Isexists = true;

                                drr["sProducts_Name"] = sProducts_Name;
                                drr["ProductID"] = ProductID;
                                drr["sProducts_Description"] = sProducts_Description;
                                drr["oldUnit_qty"] = oldUnit_qty;
                                drr["oldUnit_Uom"] = oldUnit_Uom;
                                drr["Balance_Quantity"] = Balance_Quantity;
                                drr["Quantity_Received"] = Quantity_Received;
                                break;
                            }
                        }

                    }
                }
            }
            SalesOrderdt.AcceptChanges();

          //  string UniqueQuotation="";
                string strQuoteNo = Convert.ToString(txt_SlBTOutNo.Text);
                string DNstrQuoteNo = Convert.ToString(txtDNnumber.Text);
                string validate = string.Empty;
                DataTable tempWarehousedt = new DataTable();
                DataTable OldUnitDT = new DataTable();
               // Session["ActionType"] = "Add";
                string ActionType = Convert.ToString(Session["ActionType"]);
                string MainOrderID = string.Empty;
                if (!string.IsNullOrEmpty(Convert.ToString(Session["Invoice_ID"])))
                {

                    MainOrderID = Convert.ToString(Session["Invoice_ID"]);
                }
                else
                {
                    MainOrderID = ""; 
                }
                string strSchemeType = Convert.ToString(ddl_numberingScheme.SelectedValue);
                string DNstrSchemeType = Convert.ToString(HdnDnNumbering.Value); //Convert.ToString(ddlDNnumbering.SelectedValue);

                string[] DNSchemeList = DNstrSchemeType.Split(new string[] { "~" }, StringSplitOptions.None);
                if (DNSchemeList[0] != "")
                {
                    validate = checkNMakeJVCodeDN(DNstrQuoteNo, Convert.ToInt32(DNSchemeList[0]));

                }
                  if (ActionType=="Edit")
                  {
                      UniqueQuotation = Convert.ToString(txt_SlBTOutNo.Text);
                  }
               
            

                string strQuoteDate = Convert.ToString(dt_BTOut.Date);
                string strQuoteExpiry = Convert.ToString(dt_PlOrderExpiry.Date);
                string strCustomer = Convert.ToString(hdfLookupCustomer.Value);
                string strContactName = Convert.ToString("");
                string Reference = Convert.ToString(txt_Refference.Text);
                string strBranch = Convert.ToString(ddl_transferFrom_Branch.SelectedValue);
                string DNstrBranch = Convert.ToString(ddlDnBranch.SelectedValue);
                string strCurrency = Convert.ToString(ddl_Currency.SelectedValue);
                string CompID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string currency = Convert.ToString(HttpContext.Current.Session["ActiveCurrency"]);
                string[] ActCurrency = currency.Split('~');
                int BaseCurrencyId = Convert.ToInt32(ActCurrency[0]);
                int ConvertedCurrencyId = Convert.ToInt32(strCurrency);
                string Adjust =Convert.ToString(hasBalanceQty.Value);
                SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
                DataTable dtt = objSlaesActivitiesBL.GetCurrentConvertedRate(BaseCurrencyId, ConvertedCurrencyId, CompID);
                string strRate = "0";
                if (dtt != null && dtt.Rows.Count > 0)
                {
                    strRate = Convert.ToString(dtt.Rows[0]["SalesRate"]);
                }
                string strTaxType = Convert.ToString(ddl_AmountAre.Value);
                string strTaxCode = Convert.ToString(ddl_VatGstCst.Value);
                DataTable TaxDetailTable = new DataTable();
                if (Session["OldUnitStockInvoiceFinalTaxRecord"] != null)
                {
                    TaxDetailTable = (DataTable)Session["OldUnitStockInvoiceFinalTaxRecord"];
                }
                string OANumber = Convert.ToString(txt_OANumber.Text);
                string OADate = Convert.ToString(dt_OADate.Date);
                string QuotationDate = string.Empty;
                String QuoComponent = "";
                DataTable TaxDetailsdt = new DataTable();
                if (Session["OldUnitStockInvoiceTaxDetails"] != null)
                {
                    TaxDetailsdt = (DataTable)Session["OldUnitStockInvoiceTaxDetails"];
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
                DataTable tempBillAddress = new DataTable();
                if (ActionType == "Add")
                {
                    string[] SchemeList = strSchemeType.Split(new string[] { "~" }, StringSplitOptions.None);
                    
                    if (SchemeList[0] != "")
                    {
                        validate = checkNMakeJVCode(strQuoteNo, Convert.ToInt32(SchemeList[0]));
                        
                    }

                }

        #endregion

                if (Session["OldUnitStockInvoiceAddressDtl"] != null)
                {
                    tempBillAddress = (DataTable)Session["OldUnitStockInvoiceAddressDtl"];
                }
                else
                {
                    tempBillAddress = StoreSalesOrderAddressDetail();
                }
                string strBranchTrasferFrom = ddl_transferFrom_Branch.SelectedValue;

                if (Session["OldUnitStockInvoice"] != null)
                {
                    OldUnitDT = (DataTable)Session["OldUnitStockInvoice"];
                }
                else
                {
                    DataTable dt_QuotationDetails = oBL.GetOldUnitDetailsByInvoice(Convert.ToInt32(Request.QueryString["key"]));
                    Session["OldUnitStockInvoice"] = null;
                    GetOldUnitDetailsInfo(dt_QuotationDetails);
                    OldUnitDT = (DataTable)Session["OldUnitStockInvoice"];
                }
                if (Session["OUR_WarehouseData"] != null)
                {
                    DataTable Warehousedt = (DataTable)Session["OUR_WarehouseData"];
                    tempWarehousedt = Warehousedt.DefaultView.ToTable(false, "Product_SrlNo", "LoopID", "SrlNo", "WarehouseID", "Quantity", "BatchID", "SerialNo", "MfgDate", "ExpiryDate");
                }
                else
                {
                    tempWarehousedt.Columns.Add("Product_SrlNo", typeof(string));
                    tempWarehousedt.Columns.Add("SrlNo", typeof(string));
                    tempWarehousedt.Columns.Add("Row_Number", typeof(string));
                    tempWarehousedt.Columns.Add("WarehouseID", typeof(string));
                    tempWarehousedt.Columns.Add("Quantity", typeof(string));
                    tempWarehousedt.Columns.Add("BatchID", typeof(string));
                    tempWarehousedt.Columns.Add("SerialNo", typeof(string));
                    tempWarehousedt.Columns.Add("MfgDate", typeof(string));
                    tempWarehousedt.Columns.Add("ExpiryDate", typeof(string));
                }

                foreach (DataRow dr in OldUnitDT.Rows)
                {
                    if (Convert.ToString(dr["Quantity_Received"]) == "" && Convert.ToString(dr["Balance_Quantity"]) == "")
                    {
                        dr["Balance_Quantity"] = Convert.ToDecimal(dr["oldUnit_qty"]);
                        dr["Quantity_Received"] = 0.00;
                    }
                }
                OldUnitDT.AcceptChanges();
                if (hasBalanceQty.Value == "")
                {
                    foreach (DataRow OldDr in OldUnitDT.Rows){
                        if (Convert.ToDecimal(OldDr["Balance_Quantity"]) > 0)
                        {
                            validate = "HasBalanceQuantity";
                            hasBalanceQty.Value = "Yes";
                            break;
                        }
                    }
                }

                if (Session["OUR_WarehouseData"] != null)
                {
                    foreach (DataRow dr in OldUnitDT.Rows)
                    {
                        string SrlNo = Convert.ToString(dr["SrlNo"]);
                        double oldUnit_qty = Convert.ToDouble(dr["oldUnit_qty"]);
                        double Quantity_Received = Convert.ToDouble(dr["Quantity_Received"]);
                        if (CheckIfWareHousedataExist(SrlNo, (DataTable)Session["OUR_WarehouseData"]))
                        {
                            if (!CheckIfQuantityOK(SrlNo, Quantity_Received, (DataTable)Session["OUR_WarehouseData"]))
                            {
                                validate = "checkWarehouse";
                                grid.JSProperties["cpProductSrlIDCheck"] = SrlNo;
                                break;
                            }
                        }
                        else
                        {
                            //validate = "checkWarehouse";
                            //grid.JSProperties["cpProductSrlIDCheck"] = SrlNo;
                            //break;
                        }
                    }

                }

                if (tempWarehousedt.Rows.Count == 0 )
                {
                    foreach (DataRow OldDr in OldUnitDT.Rows)
                    {
                        if (Convert.ToDecimal(OldDr["Quantity_Received"]) != 0)
                        {
                            validate = "checkWarehouse";
                            grid.JSProperties["cpProductSrlIDCheck"] = "";
                            break;
                        }
                    }
                    
                }

                if (validate == "outrange" || validate == "duplicateProduct" || validate == "checkWarehouse" || validate == "HasBalanceQuantity")
                {
                    grid.JSProperties["cpSaveSuccessOrFail"] = validate;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GridCallBack()", true);
                }
                else
                {
                    #region update tbl_trans_OldUnit Entity
                    if (Session["OldUnitStockInvoice"] != null && ((DataTable)Session["OldUnitStockInvoice"]).Rows.Count > 0)
                    {
                        tbl_trans_oldunit _Obj = new tbl_trans_oldunit();

                        _Obj.Invoice_Id = Convert.ToInt32(((DataTable)Session["OldUnitStockInvoice"]).Rows[0]["doc_id"]);
                        _Obj.receiver_remark = txt_Refference.Text;
                        _Obj.current_status = 2;
                        _Obj.received_by = Convert.ToInt32(HttpContext.Current.Session["userid"]);

                        OldUnitAssignReceivedBL.ReceivedBranch((object)_Obj);
                    }

                    #endregion
                    int id = 0;
                    grid.JSProperties["cpSaveSuccessOrFail"] = "";
                    id = ModifySalesChallan(MainOrderID, strSchemeType, UniqueQuotation, strQuoteDate, strQuoteExpiry, strCustomer, strContactName,
                                            Reference, strBranch, strCurrency, strRate, strTaxType, strTaxCode, OldUnitDT, TaxDetailTable, ActionType, OANumber, OADate, "0", QuotationDate, QuoComponent, tempWarehousedt, tempBillAddress
                                            , tempTaxDetailsdt, strBranchTrasferFrom, Adjust, UniqueQuotationDN, DNstrBranch);

                    if (id <= 0)
                    {
                        grid.JSProperties["cpSaveSuccessOrFail"] = "errorInsert";
                    }
                    else
                    {
                        //Udf Add mode
                        DataTable udfTable = (DataTable)Session["UdfDataOnAdd"];
                        if (udfTable != null)
                            Session["UdfDataOnAdd"] = reCat.insertRemarksCategoryAddMode("BSO", "BranchStockOut" + Convert.ToString(id), udfTable, Convert.ToString(Session["userid"]));

                        //grid.JSProperties["cpSalesOrderNo"] = UniqueQuotation;

                        if (ActionType == "Add")
                        {
                            grid.JSProperties["cpSalesOrderNo"] = UniqueQuotation;
                        }
                        else if (ActionType != "Add")
                        {
                            if (!string.IsNullOrEmpty(txt_SlBTOutNo.Text))
                            {
                                grid.JSProperties["cpIssueToServiceInEdit"] = Convert.ToString(txt_SlBTOutNo.Text);
                            }

                        }
                    }
                    if (Session["OldUnitStockInvoice"] != null)
                    {
                        Session["OldUnitStockInvoice"] = null;
                    }
                }



                      
        }
        


        #region Old batch update by Sayan
        //protected void grid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        //{
        //    Session["ActionType"] = "Add";

        //    DataTable SalesOrderdt = new DataTable();
        //    string IsDeleteFrom = Convert.ToString(hdfIsDelete.Value);

        //    if (Session["OldUnitStockInvoice"] != null)
        //    {
        //        //subhabrata: foreach@deleted extra column 
        //        DataTable dt = new DataTable();
        //        dt = (DataTable)Session["OldUnitStockInvoice"];
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            DataColumnCollection dtC = dt.Columns;

        //            if (dtC.Contains("Indent"))
        //            { dt.Columns.Remove("Indent"); }
        //            break;
        //        }//End

        //        //SalesOrderdt = (DataTable)Session["OrderDetails"];

        //        SalesOrderdt = dt;
        //    }
        //    else
        //    {
        //        SalesOrderdt.Columns.Add("SrlNo", typeof(string));
        //        SalesOrderdt.Columns.Add("OrderID", typeof(string));
        //        SalesOrderdt.Columns.Add("ProductID", typeof(string));
        //        SalesOrderdt.Columns.Add("Description", typeof(string));
        //        //SalesOrderdt.Columns.Add("Quotation", typeof(string));//Added By:subhabrata on 21-02-2017               
        //        SalesOrderdt.Columns.Add("Quantity", typeof(string));
        //        SalesOrderdt.Columns.Add("oldUnit_Uom", typeof(string));
        //        SalesOrderdt.Columns.Add("Warehouse", typeof(string));
        //        SalesOrderdt.Columns.Add("StockQuantity", typeof(string));
        //        SalesOrderdt.Columns.Add("StockUOM", typeof(string));
        //        SalesOrderdt.Columns.Add("SalePrice", typeof(string));
        //        SalesOrderdt.Columns.Add("Discount", typeof(string));
        //        SalesOrderdt.Columns.Add("Amount", typeof(string));
        //        SalesOrderdt.Columns.Add("TaxAmount", typeof(string));
        //        SalesOrderdt.Columns.Add("TotalAmount", typeof(string));
        //        SalesOrderdt.Columns.Add("Status", typeof(string));
        //        SalesOrderdt.Columns.Add("Stk_Id", typeof(string));
        //        SalesOrderdt.Columns.Add("ProductName", typeof(string));
        //    }
        //    int init_Val = 0;
        //    foreach (var args in e.InsertValues)
        //    {
        //        string ProductDetails = Convert.ToString(args.NewValues["ProductID"]);

        //        if (ProductDetails != "" && ProductDetails != "0")
        //        {
        //            init_Val = 1;
        //            string SrlNo = Convert.ToString(args.NewValues["SrlNo"]);
        //            string[] ProductDetailsList = ProductDetails.Split(new string[] { "||@||" }, StringSplitOptions.None);
        //            string ProductID = ProductDetailsList[0];

        //            string ProductName = Convert.ToString(args.NewValues["ProductName"]);
        //            string Description = Convert.ToString(args.NewValues["Description"]);
        //            string Quantity = Convert.ToString(args.NewValues["Quantity"]);
        //            string UOM = Convert.ToString(args.NewValues["oldUnit_Uom"]);
        //            string Warehouse = Convert.ToString(args.NewValues["Warehouse"]);
        //            string StockQuantity = Convert.ToString(args.NewValues["StockQuantity"]) == "" ? "0" : Convert.ToString(args.NewValues["StockQuantity"]);
        //            string StockUOM = Convert.ToString(args.NewValues["StockUOM"]);
        //            string SalePrice = Convert.ToString(args.NewValues["SalePrice"]);
        //            string Discount = Convert.ToString(args.NewValues["Discount"]);
        //            string Amount = (Convert.ToString(args.NewValues["Amount"]) != "") ? Convert.ToString(args.NewValues["Amount"]) : "0";
        //            string TaxAmount = (Convert.ToString(args.NewValues["TaxAmount"]) != "") ? Convert.ToString(args.NewValues["TaxAmount"]) : "0";
        //            string TotalAmount = (Convert.ToString(args.NewValues["TotalAmount"]) != "") ? Convert.ToString(args.NewValues["TotalAmount"]) : "0";
        //            string QuotationNumber = Convert.ToString(args.NewValues["Quotation_Number"]);
        //            //string Quotation = Convert.ToString(args.NewValues["Quotation_Num"]);//Added By:Subhabrata on 21-02-2017

        //            SalesOrderdt.Rows.Add(SrlNo, init_Val, ProductDetails, Description, Quantity, UOM, Warehouse, StockQuantity, StockUOM, SalePrice, Discount, Amount, TaxAmount, TotalAmount, "I", QuotationNumber, ProductName);
        //        }
        //        init_Val = init_Val + 1;
        //    }

        //    foreach (var args in e.UpdateValues)
        //    {
        //        string SrlNo = Convert.ToString(args.NewValues["SrlNo"]);
        //        string oldUnit_id = Convert.ToString(args.Keys["Quantity_Received"]);
        //        //Session["Invoice_ID"] = OrderID;
        //        string ProductDetails = Convert.ToString(args.NewValues["ProductID"]);


        //        bool isDeleted = false;
        //        foreach (var arg in e.DeleteValues)
        //        {
        //            string DeleteID = Convert.ToString(arg.Keys["Indent_UniqueId"]);
        //            if (DeleteID == oldUnit_id)
        //            {
        //                isDeleted = true;
        //                break;
        //            }
        //        }

        //        if (isDeleted == false)
        //        {
        //            if (ProductDetails != "" && ProductDetails != "0")
        //            {
        //                string[] ProductDetailsList = ProductDetails.Split(new string[] { "||@||" }, StringSplitOptions.None);

        //                string sProducts_Description = Convert.ToString(args.NewValues["sProducts_Description"]);
        //                string oldUnit_qty = Convert.ToString(args.NewValues["oldUnit_qty"]);
        //                //string ProductID = Convert.ToString(ProductDetailsList[0]);
        //                string ProductID = ProductDetails;
        //                string sProducts_Name = Convert.ToString(args.NewValues["sProducts_Name"]);
        //                string oldUnit_value = Convert.ToString(args.NewValues["oldUnit_value"]);
        //                string oldUnit_Uom = Convert.ToString(args.NewValues["oldUnit_Uom"]);


        //                bool Isexists = false;
        //                foreach (DataRow drr in SalesOrderdt.Rows)
        //                {
        //                    string oldUnit_ID = Convert.ToString(drr["oldUnit_id"]);

        //                    if (oldUnit_ID == oldUnit_id)
        //                    {
        //                        Isexists = true;

        //                        drr["sProducts_Name"] = sProducts_Name;
        //                        drr["ProductID"] = ProductID;
        //                        drr["sProducts_Description"] = sProducts_Description;
        //                        drr["oldUnit_qty"] = oldUnit_qty;
        //                        drr["oldUnit_Uom"] = oldUnit_Uom;

        //                        break;
        //                    }
        //                }

        //                if (Isexists == false)
        //                {
        //                    //SalesOrderdt.Rows.Add(SrlNo, oldUnit_id, ProductDetails, Description, Quantity, UOM, Warehouse, StockQuantity, StockUOM, SalePrice, Discount, Amount, TaxAmount, TotalAmount, "U", QuotationNumber, ProductName);
        //                    //}
        //                    //SalesOrderdt.Rows.Add(SrlNo, OrderID, ProductDetails, Description, Quotation, Quantity, UOM, Warehouse, StockQuantity, StockUOM, SalePrice, Discount, Amount, TaxAmount, TotalAmount, "U", QuotationNumber);
        //                }
        //            }
        //        }
        //    }

        //    foreach (var args in e.DeleteValues)
        //    {
        //        string OrderID = Convert.ToString(args.Keys["Indent_UniqueId"]);

        //        for (int i = SalesOrderdt.Rows.Count - 1; i >= 0; i--)
        //        {
        //            DataRow dr = SalesOrderdt.Rows[i];
        //            string delQuotationID = Convert.ToString(dr["Indent_UniqueId"]);

        //            if (delQuotationID == OrderID)
        //                dr.Delete();
        //        }
        //        SalesOrderdt.AcceptChanges();

        //        if (OrderID.Contains("~") != true)
        //        {
        //            SalesOrderdt.Rows.Add("0", OrderID, "0", "", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "D", "0", "0");
        //        }
        //    }

        //    ///////////////////////

        //    if (IsDeleteFrom == "D")
        //    {
        //        int j = 1;
        //        foreach (DataRow dr in SalesOrderdt.Rows)
        //        {
        //            string Status = Convert.ToString(dr["Status"]);
        //            dr["SrlNo"] = j.ToString();

        //            if (Status != "D")
        //            {
        //                if (Status == "I" && IsDeleteFrom == "D")
        //                {
        //                    string strID = Convert.ToString("Q~" + j);
        //                    dr["OrderID"] = strID;
        //                }
        //                j++;
        //            }
        //        }
        //        SalesOrderdt.AcceptChanges();
        //    }

        //    Session["OldUnitStockInvoice"] = SalesOrderdt;
        //    //////////////////////


        //    if (IsDeleteFrom != "D")
        //    {
        //        try
        //        {
        //            string ActionType = Convert.ToString(Session["ActionType"]);
        //            string MainOrderID = string.Empty;
        //            if (Convert.ToString(Request.QueryString["key"]) != null && Convert.ToString(Request.QueryString["key"]) != "")
        //            {

        //                Session["Invoice_ID"] = Convert.ToString(Request.QueryString["key"]);

        //                if (!string.IsNullOrEmpty(Convert.ToString(Session["Invoice_ID"])))
        //                {

        //                    MainOrderID = Convert.ToString(Session["Invoice_ID"]);
        //                }
        //                else
        //                {
        //                    MainOrderID = "";
        //                }

        //            }
        //            else
        //            {
        //                if (!string.IsNullOrEmpty(Convert.ToString(Session["Invoice_ID"])))
        //                {

        //                    MainOrderID = Convert.ToString(Session["Invoice_ID"]);
        //                }
        //                else
        //                {
        //                    MainOrderID = "";
        //                }

        //            }



        //            string strSchemeType = Convert.ToString(ddl_numberingScheme.SelectedValue);
        //            string strQuoteNo = Convert.ToString(txt_SlBTOutNo.Text);
        //            string strQuoteDate = Convert.ToString(dt_BTOut.Date);
        //            string strQuoteExpiry = Convert.ToString(dt_PlOrderExpiry.Date);
        //            string strCustomer = Convert.ToString(hdfLookupCustomer.Value);
        //            string strContactName = Convert.ToString("");
        //            string Reference = Convert.ToString(txt_Refference.Text);
        //            string strBranch = Convert.ToString(ddl_transferFrom_Branch.SelectedValue);
        //            //string strAgents = Convert.ToString(ddl_SalesAgent.SelectedValue);
        //            string strCurrency = Convert.ToString(ddl_Currency.SelectedValue);
        //            //string strRate = Convert.ToString(txt_Rate.Value);
        //            string strTaxType = Convert.ToString(ddl_AmountAre.Value);
        //            string strTaxCode = Convert.ToString(ddl_VatGstCst.Value);
        //            //Added by:Subhabrata
        //            string OANumber = Convert.ToString(txt_OANumber.Text);
        //            //Subhabrata
        //            string strBranchTrasferFrom = ddl_transferFrom_Branch.SelectedValue;
        //            //string strBranchTransferTo = ddl_transferTo_Branch.SelectedValue;
        //            //string ServiceCentre_ID = ddl_ServiceCenter.SelectedValue;
        //            //string Call_No = txtCallNo.Text;

        //            //End
        //            string OADate = Convert.ToString(dt_OADate.Date);
        //            //  string QuotationDate = Convert.ToString(dt_Quotation.Text);
        //            string QuotationDate = string.Empty;
        //            //Get Quotation details
        //            String QuoComponent = "";
        //            //List<object> QuoList = lookup_order.Value;
        //            //foreach (object Quo in QuoList)
        //            //{
        //            //    QuoComponent += "," + Quo;
        //            //}
        //            QuoComponent = Convert.ToString(lookup_order.Value);
        //            string[] eachQuo = QuoComponent.Split(',');
        //            if (eachQuo.Length == 1)
        //            {
        //                QuotationDate = Convert.ToString(dt_Quotation.Text);
        //                //  strQuoteDate = Convert.ToString(dt_Quotation.Text);
        //                // dt_Quotation.Text = "Multiple Select Quotation Dates";
        //                // lbl_MultipleDate.Attributes.Add("style", "display:block");
        //            }
        //            else
        //            {
        //                //  lbl_MultipleDate.Attributes.Add("style","display:none"); 
        //            }
        //            // string QuotationNumber = Convert.ToString(ddl_Quotation.Value);
        //            //End   
        //            string strRate = "0";
        //            string CompID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
        //            string currency = Convert.ToString(HttpContext.Current.Session["ActiveCurrency"]);
        //            string[] ActCurrency = currency.Split('~');
        //            int BaseCurrencyId = Convert.ToInt32(ActCurrency[0]);
        //            int ConvertedCurrencyId = Convert.ToInt32(strCurrency);
        //            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
        //            DataTable dt = objSlaesActivitiesBL.GetCurrentConvertedRate(BaseCurrencyId, ConvertedCurrencyId, CompID);
        //            if (dt != null && dt.Rows.Count > 0)
        //            {
        //                strRate = Convert.ToString(dt.Rows[0]["SalesRate"]);
        //            }

        //            foreach (DataRow dr in SalesOrderdt.Rows)
        //            {
        //                string Status = Convert.ToString(dr["Status"]);

        //                if (Status == "I")
        //                {
        //                    dr["OrderID"] = "0";

        //                    string[] list = Convert.ToString(dr["ProductID"]).Split(new string[] { "||@||" }, StringSplitOptions.None);
        //                    //dr["ProductID"] = Convert.ToString(list[0]);
        //                    dr["oldUnit_Uom"] = Convert.ToString(list[3]);
        //                    dr["StockUOM"] = Convert.ToString(list[5]);
        //                }
        //                else if (Status == "U" || Status == "")
        //                {
        //                    string[] list = Convert.ToString(dr["ProductID"]).Split(new string[] { "||@||" }, StringSplitOptions.None);
        //                    //dr["ProductID"] = Convert.ToString(list[0]);
        //                    if (list.Length > 1)
        //                    {
        //                        //dr["oldUnit_Uom"] = Convert.ToString(list[3]);
        //                        //dr["StockUOM"] = Convert.ToString(list[5]);
        //                    }
        //                    else
        //                    {
        //                        string UOM = Convert.ToString(dr["oldUnit_Uom"]);
        //                        //string stockUOM = Convert.ToString(dr["StockUOM"]);
        //                        //DataSet dtUOMs = new DataSet();

        //                        //if (!String.IsNullOrEmpty(UOM) && !String.IsNullOrEmpty(stockUOM))
        //                        //{
        //                        //    dtUOMs = objSlaesActivitiesBL.GetQuotationDetailsUOMInfo(UOM, stockUOM);
        //                        //    dr["oldUnit_Uom"] = Convert.ToString(dtUOMs.Tables[0].Rows[0].Field<Int64>("UOM_ID"));
        //                        //    dr["StockUOM"] = Convert.ToString(dtUOMs.Tables[1].Rows[0].Field<Int64>("UOM_ID"));
        //                        //}

        //                    }
        //                }
        //            }
        //            SalesOrderdt.AcceptChanges();


        //            DataTable TaxDetailTable = new DataTable();
        //            if (Session["OldUnitStockInvoiceFinalTaxRecord"] != null)
        //            {
        //                TaxDetailTable = (DataTable)Session["OldUnitStockInvoiceFinalTaxRecord"];
        //            }

        //            DataTable TaxDetailsdt = new DataTable();
        //            if (Session["OldUnitStockInvoiceTaxDetails"] != null)
        //            {
        //                TaxDetailsdt = (DataTable)Session["OldUnitStockInvoiceTaxDetails"];
        //            }
        //            else
        //            {
        //                TaxDetailsdt.Columns.Add("Taxes_ID", typeof(string));
        //                TaxDetailsdt.Columns.Add("Taxes_Name", typeof(string));
        //                TaxDetailsdt.Columns.Add("Percentage", typeof(string));
        //                TaxDetailsdt.Columns.Add("Amount", typeof(string));
        //                TaxDetailsdt.Columns.Add("AltTax_Code", typeof(string));
        //            }

        //            DataTable tempTaxDetailsdt = new DataTable();
        //            tempTaxDetailsdt = TaxDetailsdt.DefaultView.ToTable(false, "Taxes_ID", "Percentage", "Amount", "AltTax_Code");

        //            tempTaxDetailsdt.Columns.Add("SlNo", typeof(string));
        //            //    tempTaxDetailsdt.Columns.Add("AltTaxCode", typeof(string));

        //            tempTaxDetailsdt.Columns["SlNo"].SetOrdinal(0);
        //            tempTaxDetailsdt.Columns["Taxes_ID"].SetOrdinal(1);
        //            tempTaxDetailsdt.Columns["AltTax_Code"].SetOrdinal(2);
        //            tempTaxDetailsdt.Columns["Percentage"].SetOrdinal(3);
        //            tempTaxDetailsdt.Columns["Amount"].SetOrdinal(4);

        //            foreach (DataRow d in tempTaxDetailsdt.Rows)
        //            {
        //                d["SlNo"] = "0";
        //                //d["AltTaxCode"] = "0";
        //            }

        //            // End



        //            string validate = string.Empty;
        //            // Datattable of Warehouse
        //            DataTable tempWarehousedt = new DataTable();
        //            if (Session["OUR_WarehouseData"] != null)
        //            {
        //                DataTable Warehousedt = (DataTable)Session["OUR_WarehouseData"];
        //                tempWarehousedt = Warehousedt.DefaultView.ToTable(false, "Product_SrlNo", "LoopID", "SrlNo", "WarehouseID", "Quantity", "BatchID", "SerialNo", "MfgDate", "ExpiryDate");
        //            }
        //            else
        //            {
        //                tempWarehousedt.Columns.Add("Product_SrlNo", typeof(string));
        //                tempWarehousedt.Columns.Add("SrlNo", typeof(string));
        //                tempWarehousedt.Columns.Add("Row_Number", typeof(string));
        //                tempWarehousedt.Columns.Add("WarehouseID", typeof(string));
        //                tempWarehousedt.Columns.Add("Quantity", typeof(string));
        //                tempWarehousedt.Columns.Add("BatchID", typeof(string));
        //                tempWarehousedt.Columns.Add("SerialNo", typeof(string));
        //                tempWarehousedt.Columns.Add("MfgDate", typeof(string));
        //                tempWarehousedt.Columns.Add("ExpiryDate", typeof(string));
        //            }

        //            //End

        //            // DataTable Of Billing Address

        //            DataTable tempBillAddress = new DataTable();
        //            if (Session["OldUnitStockInvoiceAddressDtl"] != null)
        //            {
        //                tempBillAddress = (DataTable)Session["OldUnitStockInvoiceAddressDtl"];
        //            }
        //            else
        //            {
        //                tempBillAddress = StoreSalesOrderAddressDetail();
        //            }

        //            // End
        //            if (ActionType == "Add")
        //            {
        //                string[] SchemeList = strSchemeType.Split(new string[] { "~" }, StringSplitOptions.None);

        //                if (SchemeList[0] != "")
        //                {
        //                    validate = checkNMakeJVCode(strQuoteNo, Convert.ToInt32(SchemeList[0]));
        //                }
        //            }
        //            DataTable duplicatedt = SalesOrderdt.Copy();
        //            var duplicateRecords = duplicatedt.AsEnumerable()
        //            .GroupBy(r => r["ProductID"]) //coloumn name which has the duplicate values
        //            .Where(gr => gr.Count() > 1)
        //            .Select(g => g.Key);

        //            foreach (var d in duplicateRecords)
        //            {
        //                validate = "duplicateProduct";
        //            }

        //            if (Session["OUR_WarehouseData"] != null)
        //            {
        //                foreach (DataRow dr in duplicatedt.Rows)
        //                {
        //                    string SrlNo = Convert.ToString(dr["SrlNo"]);
        //                    double oldUnit_qty = Convert.ToDouble(dr["oldUnit_qty"]);

        //                    if (CheckIfWareHousedataExist(SrlNo, (DataTable)Session["OUR_WarehouseData"]))
        //                    {
        //                        if (!CheckIfQuantityOK(SrlNo, oldUnit_qty, (DataTable)Session["OUR_WarehouseData"]))
        //                        {
        //                            validate = "checkWarehouse";
        //                            grid.JSProperties["cpProductSrlIDCheck"] = SrlNo;
        //                            break;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        validate = "checkWarehouse";
        //                        grid.JSProperties["cpProductSrlIDCheck"] = SrlNo;
        //                        break;
        //                    }
        //                }

        //            }

        //            if (tempWarehousedt.Rows.Count == 0)
        //            {
        //                validate = "checkWarehouse";
        //                grid.JSProperties["cpProductSrlIDCheck"] = "";
        //            }

        //            if (validate == "outrange" || validate == "duplicateProduct" || validate == "checkWarehouse")
        //            {
        //                grid.JSProperties["cpSaveSuccessOrFail"] = validate;
        //                //DataTable dt_QuotationDetails = oBL.GetOldUnitDetailsByInvoice(Convert.ToInt32(Request.QueryString["key"]));
        //                //Session["OldUnitStockInvoice"] = null;
        //                //grid.DataSource = GetOldUnitDetailsInfo(dt_QuotationDetails);
        //                //grid.DataBind();
        //                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GridCallBack()", true);
        //            }
        //            else
        //            {
        //                #region update tbl_trans_OldUnit Entity
        //                if (Session["OldUnitStockInvoice"] != null && ((DataTable)Session["OldUnitStockInvoice"]).Rows.Count > 0)
        //                {
        //                    tbl_trans_oldunit _Obj = new tbl_trans_oldunit();

        //                    _Obj.Invoice_Id = Convert.ToInt32(((DataTable)Session["OldUnitStockInvoice"]).Rows[0]["doc_id"]);
        //                    _Obj.receiver_remark = txt_Refference.Text;
        //                    _Obj.current_status = 2;
        //                    _Obj.received_by = Convert.ToInt32(HttpContext.Current.Session["userid"]);

        //                    OldUnitAssignReceivedBL.ReceivedBranch((object)_Obj);
        //                }

        //                #endregion
        //                int id = 0;
        //                grid.JSProperties["cpSaveSuccessOrFail"] = "";
        //                //grid.JSProperties["cpSaveSuccessOrFail"] = "";
        //                //if (tempWarehousedt.Rows.Count > 0)
        //                //{
        //                //int id = ModifySalesChallan(MainOrderID, strSchemeType, UniqueQuotation, strQuoteDate, strQuoteExpiry, strCustomer, strContactName,
        //                //                        Reference, strBranch, strCurrency, strRate, strTaxType, strTaxCode, SalesOrderdt, TaxDetailTable, ActionType, OANumber, OADate, "0", QuotationDate, QuoComponent, tempWarehousedt, tempBillAddress
        //                //                        , tempTaxDetailsdt, strBranchTrasferFrom, strBranchTransferTo, ServiceCentre_ID, Call_No);
        //                id = ModifySalesChallan(MainOrderID, strSchemeType, UniqueQuotation, strQuoteDate, strQuoteExpiry, strCustomer, strContactName,
        //                                        Reference, strBranch, strCurrency, strRate, strTaxType, strTaxCode, SalesOrderdt, TaxDetailTable, ActionType, OANumber, OADate, "0", QuotationDate, QuoComponent, tempWarehousedt, tempBillAddress
        //                                        , tempTaxDetailsdt, strBranchTrasferFrom);
        //                //if (ModifySalesChallan(MainOrderID, strSchemeType, UniqueQuotation, strQuoteDate, strQuoteExpiry, strCustomer, strContactName,
        //                //                       Reference, strBranch, strAgents, strCurrency, strRate, strTaxType, strTaxCode, SalesOrderdt, TaxDetailTable, ActionType, OANumber, OADate, "0", QuotationDate, QuoComponent, tempWarehousedt, tempBillAddress) >0)
        //                //
        //                if (id <= 0)
        //                {
        //                    grid.JSProperties["cpSaveSuccessOrFail"] = "errorInsert";
        //                }
        //                else
        //                {
        //                    //Udf Add mode
        //                    DataTable udfTable = (DataTable)Session["UdfDataOnAdd"];
        //                    if (udfTable != null)
        //                        Session["UdfDataOnAdd"] = reCat.insertRemarksCategoryAddMode("BSO", "BranchStockOut" + Convert.ToString(id), udfTable, Convert.ToString(Session["userid"]));

        //                    //grid.JSProperties["cpSalesOrderNo"] = UniqueQuotation;

        //                    if (ActionType == "Add")
        //                    {
        //                        grid.JSProperties["cpSalesOrderNo"] = UniqueQuotation;
        //                    }
        //                    else if (ActionType != "Add")
        //                    {
        //                        if (!string.IsNullOrEmpty(txt_SlBTOutNo.Text))
        //                        {
        //                            grid.JSProperties["cpIssueToServiceInEdit"] = Convert.ToString(txt_SlBTOutNo.Text);
        //                        }

        //                    }
        //                }
        //                //Added:Subhabrat
        //                if (Session["OldUnitStockInvoice"] != null)
        //                {
        //                    Session["OldUnitStockInvoice"] = null;
        //                    //  Session.Remove("OrderDetails");
        //                }
        //                //}
        //                //else
        //                //{
        //                //    grid.JSProperties["cpSaveSuccessOrFail"] = "wareHouseDataMissing";

        //                //    DataTable dt_QuotationDetails = oBL.GetOldUnitDetailsByInvoice(Convert.ToInt32(Request.QueryString["key"]));
        //                //    Session["OldUnitStockInvoice"] = null;
        //                //    grid.DataSource = GetOldUnitDetailsInfo(dt_QuotationDetails);
        //                //    grid.DataBind();
        //                //    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GridCallBack()", true);
        //                //}
        //            }
        //        }
        //        catch { }
        //    }
        //    else
        //    {
        //        DataView dvData = new DataView(SalesOrderdt);
        //        dvData.RowFilter = "Status <> 'D'";

        //        grid.DataSource = GetSalesOrder(dvData.ToTable());
        //        grid.DataBind();
        //    }
        //}
        #endregion
        private bool CheckIfQuantityOK(string SrlNo, double oldUnit_qty, DataTable PC_WarehouseData)
        {
            if (PC_WarehouseData.Rows.Count > 0)
            {
                double totalQuantity = PC_WarehouseData.AsEnumerable()
                                    .Where(R => SrlNo.Trim().Equals(R.Field<String>("Product_SrlNo").Trim()) && Convert.ToDouble(R.Field<String>("TotalQuantity").Trim()) != 0)
                                    .Sum(r => Convert.ToDouble(r.Field<String>("Quantity")));

                if (totalQuantity == oldUnit_qty)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool CheckIfWareHousedataExist(string SrlNo, DataTable PC_WarehouseData)
        {
            if (PC_WarehouseData.Rows.Count > 0)
            {
                return PC_WarehouseData.AsEnumerable().Any(R => SrlNo.Trim().Equals(R.Field<String>("Product_SrlNo").Trim()));
            }
            else
            {
                return false;
            }
        }
        protected void grid_DataBinding(object sender, EventArgs e)
        {
            if (Session["OldUnitStockInvoice"] != null && ((DataTable)Session["OldUnitStockInvoice"]).Rows.Count > 0)
            {
                DataTable Quotationdt = (DataTable)Session["OldUnitStockInvoice"];
                DataView dvData = new DataView(Quotationdt);
                //dvData.RowFilter = "Status <> 'D'";
                grid.DataSource = GetOldUnitDetailsInfo(dvData.ToTable());
            }
        }
        //SUBHABRATA
        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];

            if (strSplitCommand == "OldUnitReceivedsaveChanges")
            {
                #region Initialize property
                string strQuoteNo = Convert.ToString(txt_SlBTOutNo.Text);
                string validate = string.Empty;
                DataTable tempWarehousedt = new DataTable();
                DataTable OldUnitDT = new DataTable();
                //Session["ActionType"] = "Add";
                string ActionType = Convert.ToString(Session["ActionType"]);
                string MainOrderID = string.Empty;
                if (!string.IsNullOrEmpty(Convert.ToString(Session["Invoice_ID"])))
                {

                    MainOrderID = Convert.ToString(Session["Invoice_ID"]);
                }
                else
                {
                    MainOrderID = "";
                }
                string strSchemeType = Convert.ToString(ddl_numberingScheme.SelectedValue);
                string strQuoteDate = Convert.ToString(dt_BTOut.Date);
                string strQuoteExpiry = Convert.ToString(dt_PlOrderExpiry.Date);
                string strCustomer = Convert.ToString(hdfLookupCustomer.Value);
                string strContactName = Convert.ToString("");
                string Reference = Convert.ToString(txt_Refference.Text);
                string strBranch = Convert.ToString(ddl_transferFrom_Branch.SelectedValue);
                string strCurrency = Convert.ToString(ddl_Currency.SelectedValue);
                string CompID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string currency = Convert.ToString(HttpContext.Current.Session["ActiveCurrency"]);
                string[] ActCurrency = currency.Split('~');
                int BaseCurrencyId = Convert.ToInt32(ActCurrency[0]);
                int ConvertedCurrencyId = Convert.ToInt32(strCurrency);
                SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
                DataTable dt = objSlaesActivitiesBL.GetCurrentConvertedRate(BaseCurrencyId, ConvertedCurrencyId, CompID);
                string strRate = "0";
                if (dt != null && dt.Rows.Count > 0)
                {
                    strRate = Convert.ToString(dt.Rows[0]["SalesRate"]);
                }
                string strTaxType = Convert.ToString(ddl_AmountAre.Value);
                string strTaxCode = Convert.ToString(ddl_VatGstCst.Value);
                DataTable TaxDetailTable = new DataTable();
                if (Session["OldUnitStockInvoiceFinalTaxRecord"] != null)
                {
                    TaxDetailTable = (DataTable)Session["OldUnitStockInvoiceFinalTaxRecord"];
                }
                string OANumber = Convert.ToString(txt_OANumber.Text);
                string OADate = Convert.ToString(dt_OADate.Date);
                string QuotationDate = string.Empty;
                String QuoComponent = "";
                DataTable TaxDetailsdt = new DataTable();
                if (Session["OldUnitStockInvoiceTaxDetails"] != null)
                {
                    TaxDetailsdt = (DataTable)Session["OldUnitStockInvoiceTaxDetails"];
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
                DataTable tempBillAddress = new DataTable();
                if (ActionType == "Add")
                {
                    string[] SchemeList = strSchemeType.Split(new string[] { "~" }, StringSplitOptions.None);

                    if (SchemeList[0] != "")
                    {
                        validate = checkNMakeJVCode(strQuoteNo, Convert.ToInt32(SchemeList[0]));
                    }
                }
                #endregion

                if (Session["OldUnitStockInvoiceAddressDtl"] != null)
                {
                    tempBillAddress = (DataTable)Session["OldUnitStockInvoiceAddressDtl"];
                }
                else
                {
                    tempBillAddress = StoreSalesOrderAddressDetail();
                }
                string strBranchTrasferFrom = ddl_transferFrom_Branch.SelectedValue;

                if (Session["OldUnitStockInvoice"] != null)
                {
                    OldUnitDT = (DataTable)Session["OldUnitStockInvoice"];
                }
                else
                {
                    DataTable dt_QuotationDetails = oBL.GetOldUnitDetailsByInvoice(Convert.ToInt32(Request.QueryString["key"]));
                    Session["OldUnitStockInvoice"] = null;
                    GetOldUnitDetailsInfo(dt_QuotationDetails);
                    OldUnitDT = (DataTable)Session["OldUnitStockInvoice"];
                }
                if (Session["OUR_WarehouseData"] != null)
                {
                    DataTable Warehousedt = (DataTable)Session["OUR_WarehouseData"];
                    tempWarehousedt = Warehousedt.DefaultView.ToTable(false, "Product_SrlNo", "LoopID", "SrlNo", "WarehouseID", "Quantity", "BatchID", "SerialNo", "MfgDate", "ExpiryDate");
                }
                else
                {
                    tempWarehousedt.Columns.Add("Product_SrlNo", typeof(string));
                    tempWarehousedt.Columns.Add("SrlNo", typeof(string));
                    tempWarehousedt.Columns.Add("Row_Number", typeof(string));
                    tempWarehousedt.Columns.Add("WarehouseID", typeof(string));
                    tempWarehousedt.Columns.Add("Quantity", typeof(string));
                    tempWarehousedt.Columns.Add("BatchID", typeof(string));
                    tempWarehousedt.Columns.Add("SerialNo", typeof(string));
                    tempWarehousedt.Columns.Add("MfgDate", typeof(string));
                    tempWarehousedt.Columns.Add("ExpiryDate", typeof(string));
                }

                if (Session["OUR_WarehouseData"] != null)
                {
                    foreach (DataRow dr in OldUnitDT.Rows)
                    {
                        string SrlNo = Convert.ToString(dr["SrlNo"]);
                        double oldUnit_qty = Convert.ToDouble(dr["oldUnit_qty"]);

                        if (CheckIfWareHousedataExist(SrlNo, (DataTable)Session["OUR_WarehouseData"]))
                        {
                            if (!CheckIfQuantityOK(SrlNo, oldUnit_qty, (DataTable)Session["OUR_WarehouseData"]))
                            {
                                validate = "checkWarehouse";
                                grid.JSProperties["cpProductSrlIDCheck"] = SrlNo;
                                break;
                            }
                        }
                        else
                        {
                            validate = "checkWarehouse";
                            grid.JSProperties["cpProductSrlIDCheck"] = SrlNo;
                            break;
                        }
                    }

                }

                if (tempWarehousedt.Rows.Count == 0)
                {
                    validate = "checkWarehouse";
                    grid.JSProperties["cpProductSrlIDCheck"] = "";
                }

                if (validate == "outrange" || validate == "duplicateProduct" || validate == "checkWarehouse")
                {
                    grid.JSProperties["cpSaveSuccessOrFail"] = validate;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GridCallBack()", true);
                }
                else
                {
                    #region update tbl_trans_OldUnit Entity
                    if (Session["OldUnitStockInvoice"] != null && ((DataTable)Session["OldUnitStockInvoice"]).Rows.Count > 0)
                    {
                        tbl_trans_oldunit _Obj = new tbl_trans_oldunit();

                        _Obj.Invoice_Id = Convert.ToInt32(((DataTable)Session["OldUnitStockInvoice"]).Rows[0]["doc_id"]);
                        _Obj.receiver_remark = txt_Refference.Text;
                        _Obj.current_status = 2;
                        _Obj.received_by = Convert.ToInt32(HttpContext.Current.Session["userid"]);

                        OldUnitAssignReceivedBL.ReceivedBranch((object)_Obj);
                    }

                    #endregion
                    int id = 0;
                    grid.JSProperties["cpSaveSuccessOrFail"] = "";
                    id = 0;
                    //id = ModifySalesChallan(MainOrderID, strSchemeType, UniqueQuotation, strQuoteDate, strQuoteExpiry, strCustomer, strContactName,
                    //                        Reference, strBranch, strCurrency, strRate, strTaxType, strTaxCode, OldUnitDT, TaxDetailTable, ActionType, OANumber, OADate, "0", QuotationDate, QuoComponent, tempWarehousedt, tempBillAddress
                    //                        , tempTaxDetailsdt, strBranchTrasferFrom);

                    if (id <= 0)
                    {
                        grid.JSProperties["cpSaveSuccessOrFail"] = "errorInsert";
                    }
                    else
                    {
                        //Udf Add mode
                        DataTable udfTable = (DataTable)Session["UdfDataOnAdd"];
                        if (udfTable != null)
                            Session["UdfDataOnAdd"] = reCat.insertRemarksCategoryAddMode("BSO", "BranchStockOut" + Convert.ToString(id), udfTable, Convert.ToString(Session["userid"]));

                        //grid.JSProperties["cpSalesOrderNo"] = UniqueQuotation;

                        if (ActionType == "Add")
                        {
                            grid.JSProperties["cpSalesOrderNo"] = UniqueQuotation;
                        }
                        else if (ActionType != "Add")
                        {
                            if (!string.IsNullOrEmpty(txt_SlBTOutNo.Text))
                            {
                                grid.JSProperties["cpIssueToServiceInEdit"] = Convert.ToString(txt_SlBTOutNo.Text);
                            }

                        }
                    }
                    if (Session["OldUnitStockInvoice"] != null)
                    {
                        Session["OldUnitStockInvoice"] = null;
                    }
                }
            }
            else if (strSplitCommand == "Display")
            {
                string IsDeleteFrom = Convert.ToString(hdfIsDelete.Value);
                if (IsDeleteFrom == "D")
                {
                    DataTable Quotationdt = (DataTable)Session["OldUnitStockInvoice"];
                    DataView dvData = new DataView(Quotationdt);
                    dvData.RowFilter = "Status <> 'D'";
                    grid.DataSource = GetSalesOrder(dvData.ToTable());
                    grid.DataBind();
                }
            }
            else if (strSplitCommand == "GridBlank")
            {
                Session["OldUnitStockInvoice"] = null;
                grid.DataSource = null;
                grid.DataBind();
            }
            if (strSplitCommand == "BindGridOnQuotation")
            {
                ASPxGridView gv = sender as ASPxGridView;
                string command = e.Parameters.ToString();
                string State = Convert.ToString(e.Parameters.Split('~')[1]);
                String QuoComponent1 = "";
                string ProductID1 = "";
                string QuoteDetails_Id = "";
                //for (int i = 0; i < grid_Products.GetSelectedFieldValues("Indent_No").Count; i++)
                //{

                //    QuoComponent1 += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("Indent_No")[i]);
                //    ProductID1 += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("ProductID")[i]);
                //    QuoteDetails_Id += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("OrderDetails_Id")[i]);
                //}
                //QuoComponent1 = QuoComponent1.TrimStart(',');
                //ProductID1 = ProductID1.TrimStart(',');
                string Quote_Nos = Convert.ToString(e.Parameters.Split('~')[1]);
                for (int i = 0; i < grid_Products.GetSelectedFieldValues("ProductID").Count; i++)
                {
                    QuoteDetails_Id += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("StkDetails_Id")[i]);
                }

                QuoComponent1 = Convert.ToString(grid_Products.GetSelectedFieldValues("Service_Id")[0]);
                QuoteDetails_Id = QuoteDetails_Id.TrimStart(',');
                if (Quote_Nos != "$")
                {

                    DataTable dt_QuotationDetails = new DataTable();
                    string IdKey = Convert.ToString(Request.QueryString["key"]);
                    if (!string.IsNullOrEmpty(IdKey))
                    {
                        if (IdKey != "ADD")
                        {
                            dt_QuotationDetails = objSlaesActivitiesBL.GetIssueToServiceDetailsToBindCGrid(QuoComponent1, QuoteDetails_Id, ProductID1, "Edit");
                        }
                        else
                        {
                            dt_QuotationDetails = objSlaesActivitiesBL.GetIssueToServiceDetailsToBindCGrid(QuoComponent1, QuoteDetails_Id, ProductID1, "Add");
                        }

                    }
                    else
                    {
                        dt_QuotationDetails = objSlaesActivitiesBL.GetIssueToServiceDetailsToBindCGrid(QuoComponent1, QuoteDetails_Id, ProductID1, "");
                    }
                    Session["OldUnitStockInvoice"] = null;
                    grid.DataSource = GetSalesOrderInfo(dt_QuotationDetails, IdKey);
                    grid.DataBind();
                }
                else
                {
                    grid.DataSource = null;
                    grid.DataBind();
                }

            }

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

        [WebMethod]
        public static String GetDriverNamePhNo(string cnt_Id)
        {

            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            //DataTable dt = objSlaesActivitiesBL.GetCurrentConvertedRate(Convert.ToInt16(basedCurrency), Convert.ToInt16(Currency_ID), Campany_ID);
            DataTable dt = objSlaesActivitiesBL.GetDriverNamePhNo(Convert.ToInt32(cnt_Id));
            string StringVal = "";
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    StringVal = (Convert.ToString(dt.Rows[0]["Driver_Name"]) + "," + Convert.ToString(dt.Rows[0]["PhoneNo"]));
                }
            }


            return StringVal;
        }
        //public int ModifySalesChallan(string Invoice_ID, string strSchemeType, string strOrderNo, string strOrderDate, string strOrderExpiry, string strCustomer, string strContactName,
        //                            string Reference, string strBranch, string strCurrency, string strRate, string strTaxType, string strTaxCode, DataTable salesChallandt,
        //                            DataTable TaxDetailTable, string ActionType, string OANumber, string OADate, string QuotationNumber, string QuotationDate, string QuotationIdList, DataTable Warehousedt, DataTable BillAddressdt,
        //                            DataTable tempTaxDetailsdt, string TransferFrom, string TransferTo, string SreviceCentre, string CallNo)
        public int ModifySalesChallan(string Invoice_ID, string strSchemeType, string strOrderNo, string strOrderDate, string strOrderExpiry, string strCustomer, string strContactName,
                                    string Reference, string strBranch, string strCurrency, string strRate, string strTaxType, string strTaxCode, DataTable salesChallandt,
                                    DataTable TaxDetailTable, string ActionType, string OANumber, string OADate, string QuotationNumber, string QuotationDate, string QuotationIdList, DataTable Warehousedt, DataTable BillAddressdt,
                                    DataTable tempTaxDetailsdt, string TransferFrom, string Adjust, string UniqueQuotationDN, string DNstrBranch)
        {
            try
            {
                // DateTime myDatetime = DateTime.ParseExact(QuotationDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DataSet dsInst = new DataSet();
                salesChallandt.Columns.Remove("UOM_Name");
                if (salesChallandt != null && salesChallandt.Rows.Count > 0)
                {
                    foreach (DataRow dr in salesChallandt.Rows)
                    {
                        string[] list = Convert.ToString(dr["ProductID"]).Split(new string[] { "||@||" }, StringSplitOptions.None);
                        dr["ProductID"] = Convert.ToString(list[0]);
                    }
                }

               // SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                SqlCommand cmd = new SqlCommand("prc_CRMOldUnitReceivedFromBranch", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", ActionType);
                cmd.Parameters.AddWithValue("@Challan_Id", Invoice_ID);

                cmd.Parameters.AddWithValue("@OrderNo", strOrderNo);

                if (!String.IsNullOrEmpty(strOrderDate))
                    cmd.Parameters.AddWithValue("@OrderDate", Convert.ToDateTime(strOrderDate));
                cmd.Parameters.AddWithValue("@QuoteExpiry", strOrderExpiry);
                cmd.Parameters.AddWithValue("@CustomerID", strCustomer);
                cmd.Parameters.AddWithValue("@ContactPerson", strContactName);
                cmd.Parameters.AddWithValue("@Reference", Reference);
                cmd.Parameters.AddWithValue("@BranchID", strBranch);
                cmd.Parameters.AddWithValue("@DNBranchID", DNstrBranch);

                cmd.Parameters.AddWithValue("@TransferBranchFrom", strBranch);
                //cmd.Parameters.AddWithValue("@Agents", Convert.ToInt32(0));
                cmd.Parameters.AddWithValue("@Currency", strCurrency);
                cmd.Parameters.AddWithValue("@Rate", Convert.ToDecimal(strRate));
                cmd.Parameters.AddWithValue("@TaxType", strTaxType);
                cmd.Parameters.AddWithValue("@TaxCode", strTaxCode);
                cmd.Parameters.AddWithValue("@DNOrderNo", UniqueQuotationDN);

                cmd.Parameters.AddWithValue("@CompanyID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FinYear", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(Session["userid"]));

                cmd.Parameters.AddWithValue("@SalesOrderDetails", salesChallandt);
                cmd.Parameters.AddWithValue("@TaxDetail", TaxDetailTable);
                cmd.Parameters.AddWithValue("@ChallanTax", tempTaxDetailsdt);

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
                //Subhabrata
                //cmd.Parameters.AddWithValue("@TransferBranchFrom", TransferFrom);
                cmd.Parameters.AddWithValue("@TransferBranchTo", TransferFrom);
                cmd.Parameters.AddWithValue("@IsAdjust", Adjust);
                //End 
                //kaushik 24-2-2017
                //cmd.Parameters.AddWithValue("@ServiceCenter", SreviceCentre);
                //cmd.Parameters.AddWithValue("@CallNo", CallNo);
                //cmd.Parameters.AddWithValue("@BillAddress", BillAddressdt);
                //SqlParameter outputIdParam = new SqlParameter("@ReturnValue", SqlDbType.VarChar)
                //{
                //    Direction = ParameterDirection.Output
                //};
                cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
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
                if (Session["OldUnitStockInvoiceTaxDetails"] == null)
                {
                    Session["OldUnitStockInvoiceTaxDetails"] = GetTaxData(dt_BTOut.Date.ToString("yyyy-MM-dd"));
                }

                if (Session["OldUnitStockInvoiceTaxDetails"] != null)
                {
                    TaxDetailsdt = (DataTable)Session["OldUnitStockInvoiceTaxDetails"];

                    #region Delete Igst,Cgst,Sgst respectively
                    //Get Company Gstin 09032017
                    string CompInternalId = Convert.ToString(Session["LastCompany"]);
                    string[] compGstin = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_gstin", "cmp_internalid='" + CompInternalId + "'", 1);
                    string ShippingState = "";
                    if (chkBilling.Checked)
                    {
                        if (CmbState.Value != null)
                        {
                            ShippingState = CmbState.Text;
                            ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                        }
                    }
                    else
                    {
                        if (CmbState1.Value != null)
                        {
                            ShippingState = CmbState1.Text;
                            ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                        }
                    }

                    if (ShippingState.Trim() != "")
                    {
                        if (compGstin.Length > 0)
                        {
                            if (compGstin[0].Substring(0, 2) == ShippingState)
                            {
                                foreach (DataRow dr in TaxDetailsdt.Rows)
                                {
                                    if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST")
                                    {
                                        dr.Delete();
                                    }
                                }
                                TaxDetailsdt.AcceptChanges();
                            }
                            else
                            {
                                foreach (DataRow dr in TaxDetailsdt.Rows)
                                {
                                    if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                                    {
                                        dr.Delete();
                                    }
                                }
                                TaxDetailsdt.AcceptChanges();

                            }

                        }
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
                if (Session["OldUnitStockInvoiceTaxDetails"] != null)
                {
                    TaxDetailsdt = (DataTable)Session["OldUnitStockInvoiceTaxDetails"];
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

                    Session["OldUnitStockInvoiceTaxDetails"] = TaxDetailsdt;
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
            if (Session["OldUnitStockInvoiceTaxDetails"] != null)
            {
                TaxDetailsdt = (DataTable)Session["OldUnitStockInvoiceTaxDetails"];
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

            Session["OldUnitStockInvoiceTaxDetails"] = TaxDetailsdt;

            gridTax.DataSource = GetTaxes(TaxDetailsdt);
            gridTax.DataBind();
        }
        protected void cgridProducts_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];
            if (strSplitCommand == "BindProductsDetails")
            {

                string Quote_Nos = Convert.ToString(e.Parameters.Split('~')[1]);

                String OrderComponent = "";
                //List<object> QuoList = lookup_order.Value;
                //foreach (object Quo in QuoList)
                //{
                //    OrderComponent += "," + Quo;
                //}
                //OrderComponent = OrderComponent.TrimStart(',');

                OrderComponent = Convert.ToString(lookup_order.Value);

                if (Quote_Nos != "$")
                {

                    DataTable dt_OrderDetails = new DataTable();
                    string IdKey = Convert.ToString(Request.QueryString["key"]);
                    if (!string.IsNullOrEmpty(IdKey))
                    {
                        if (IdKey != "ADD")
                        {
                            dt_OrderDetails = objSlaesActivitiesBL.GetIssueToServiceCentreToBindCGridProductPopUp(OrderComponent, IdKey, "");
                        }
                        else
                        {
                            dt_OrderDetails = objSlaesActivitiesBL.GetIssueToServiceCentreToBindCGridProductPopUp(OrderComponent, "", "");
                        }

                    }
                    else
                    {
                        dt_OrderDetails = objSlaesActivitiesBL.GetIssueToServiceCentreToBindCGridProductPopUp(OrderComponent, "", "");
                    }
                    Session["OldUnitStockInvoice"] = null;
                    //grid.DataSource = GetSalesOrderInfo(dt_QuotationDetails, IdKey);
                    //grid.DataBind();
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

        protected void ComponentTransferFromBranch_Callback(object sender, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            if (strSplitCommand == "TransferBranchFrom")
            {
                if (Session["userbranchHierarchy"] != null)
                {
                    userbranch = Convert.ToString(Session["userbranchHierarchy"]);
                }
                SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
                DataSet dst = new DataSet();
                dst = objSlaesActivitiesBL.GetAllDropDownDetailForSalesChallan(userbranch);
                if (dst.Tables[1] != null && dst.Tables[1].Rows.Count > 0)
                {
                    ddl_transferFrom_Branch.DataTextField = "branch_description";
                    ddl_transferFrom_Branch.DataValueField = "branch_id";
                    ddl_transferFrom_Branch.DataSource = dst.Tables[1];
                    ddl_transferFrom_Branch.DataBind();
                    ddl_transferFrom_Branch.Items.Insert(0, new ListItem("Select", "0"));
                }
                string Stk_Id = Convert.ToString(e.Parameter.Split('~')[1]);
                DataTable dt_BranchTransferFrom = new DataTable();
                dt_BranchTransferFrom = objSlaesActivitiesBL.GetIssueToServiceCentreDate(Stk_Id);
                ddl_transferFrom_Branch.SelectedValue = Convert.ToString(dt_BranchTransferFrom.Rows[0]["Service_TransferFormBranch"]);
                ddl_transferFrom_Branch.Enabled = false;
            }
        }

        protected void ComponentServiceCentre_Callback(object sender, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            if (strSplitCommand == "ServiceCentre")
            {
                if (Session["userbranchHierarchy"] != null)
                {
                    userbranch = Convert.ToString(Session["userbranchHierarchy"]);
                }
                SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
                DataSet dst = new DataSet();
                dst = objSlaesActivitiesBL.GetAllDropDownDetailForSalesChallan(userbranch);
                //if (dst.Tables[8] != null && dst.Tables[8].Rows.Count > 0)
                //{
                //    ddl_ServiceCenter.DataTextField = "Name";
                //    ddl_ServiceCenter.DataValueField = "Id";
                //    ddl_ServiceCenter.DataSource = dst.Tables[8];
                //    ddl_ServiceCenter.DataBind();
                //    ddl_ServiceCenter.Items.Insert(0, new ListItem("Select", "0"));
                //}
                string Stk_Id = Convert.ToString(e.Parameter.Split('~')[1]);
                DataTable dt_BranchTransferFrom = new DataTable();
                dt_BranchTransferFrom = objSlaesActivitiesBL.GetIssueToServiceCentreDate(Stk_Id);
                //ddl_ServiceCenter.SelectedValue = Convert.ToString(dt_BranchTransferFrom.Rows[0]["Service_CenterId"]);
                //ddl_ServiceCenter.Enabled = false;
            }
        }

        protected void ComponentCallNo_Callback(object sender, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            if (strSplitCommand == "CallNo")
            {
                string Stk_Id = Convert.ToString(e.Parameter.Split('~')[1]);
                DataTable dt_BranchTransferFrom = new DataTable();
                dt_BranchTransferFrom = objSlaesActivitiesBL.GetIssueToServiceCentreDate(Stk_Id);
                //txtCallNo.Text = Convert.ToString(dt_BranchTransferFrom.Rows[0]["Call_No"]);
                //txtCallNo.Enabled = false;
            }
        }

        protected void ComponentNarration_Callback(object sender, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            if (strSplitCommand == "Narration")
            {
                string Stk_Id = Convert.ToString(e.Parameter.Split('~')[1]);
                DataTable dt_BranchTransferFrom = new DataTable();
                dt_BranchTransferFrom = objSlaesActivitiesBL.GetIssueToServiceCentreDate(Stk_Id);
                txt_Refference.Text = Convert.ToString(dt_BranchTransferFrom.Rows[0]["Service_Purpose"]);
                txt_Refference.Enabled = false;
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
            proc.AddVarcharPara("@Order_Id", 500, Convert.ToString(Session["Invoice_ID"]));
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
            //DataTable dt = new DataTable();
            //ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            //proc.AddVarcharPara("@Action", 500, "GetWareHouseDtlByProductID");
            //proc.AddVarcharPara("@Order_Id", 500, Convert.ToString(Session["QuotationID"]));
            //proc.AddVarcharPara("@ProductID", 500, Convert.ToString(hdfProductID.Value));
            //proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            //proc.AddVarcharPara("@branchId", 500, Convert.ToString(Session["userbranchID"]));
            //proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            //dt = proc.GetTable();
            //return dt;

            //DataTable dt = new DataTable();
            //dt = oDBEngine.GetDataTable("select  b.bui_id as WarehouseID,b.bui_Name as WarehouseName from tbl_master_building b order by b.bui_Name");
            //return dt;

            string strBranch = Convert.ToString(ddl_transferFrom_Branch.SelectedValue);

            DataTable dt = new DataTable();
            dt = oDBEngine.GetDataTable("select  bui_id as WarehouseID,bui_Name as WarehouseName from tbl_master_building Where IsNull(bui_BranchId,0) in ('0','" + strBranch + "') order by bui_Name");
            return dt;
        }

        #endregion

        #region Unique Code Generated Section Start

        //public string checkNMakeJVCode(string manual_str, int sel_schema_Id)
        //{

        //    oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //    DataTable dtSchema = new DataTable();
        //    DataTable dtC = new DataTable();
        //    string prefCompCode = string.Empty, sufxCompCode = string.Empty, startNo, paddedStr, sqlQuery = string.Empty;
        //    int EmpCode, prefLen, sufxLen, paddCounter;

        //    if (sel_schema_Id > 0)
        //    {
        //        dtSchema = oDBEngine.GetDataTable("tbl_master_idschema", "prefix, suffix, digit, startno, schema_type", "id=" + sel_schema_Id);
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

        //            sqlQuery = "SELECT max(tjv.Service_IssueNumber) FROM tbl_trans_ServiceIn tjv WHERE dbo.RegexMatch('";
        //            if (prefLen > 0)
        //                sqlQuery += "[" + prefCompCode + "]{" + prefLen + "}";
        //            else if (scheme_type == 2)
        //                sqlQuery += "^";
        //            sqlQuery += "[0-9]{" + paddCounter + "}";
        //            if (sufxLen > 0)
        //                sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
        //            sqlQuery += "?$', LTRIM(RTRIM(tjv.Service_IssueNumber))) = 1";
        //            if (scheme_type == 2)
        //                sqlQuery += " AND CONVERT(DATE, CreatedDate) = CONVERT(DATE, GETDATE())";
        //            dtC = oDBEngine.GetDataTable(sqlQuery);

        //            if (dtC.Rows[0][0].ToString() == "")
        //            {
        //                sqlQuery = "SELECT max(tjv.Service_IssueNumber) FROM tbl_trans_ServiceIn tjv WHERE dbo.RegexMatch('";
        //                if (prefLen > 0)
        //                    sqlQuery += "[" + prefCompCode + "]{" + prefLen + "}";
        //                else if (scheme_type == 2)
        //                    sqlQuery += "^";
        //                sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
        //                if (sufxLen > 0)
        //                    sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
        //                sqlQuery += "?$', LTRIM(RTRIM(tjv.Service_IssueNumber))) = 1";
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
        //                    paddedStr = EmpCode.ToString().PadLeft(paddCounter, '0');
        //                    UniqueQuotation = prefCompCode + paddedStr + sufxCompCode;
        //                    return "ok";
        //                }
        //            }
        //            else
        //            {
        //                UniqueQuotation = startNo.PadLeft(paddCounter, '0');
        //                UniqueQuotation = prefCompCode + paddedStr + sufxCompCode;
        //                return "ok";
        //            }
        //        }
        //        else
        //        {
        //            sqlQuery = "SELECT Service_IssueNumber FROM tbl_trans_ServiceIn WHERE Service_IssueNumber LIKE '" + manual_str.Trim() + "'";
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

                    sqlQuery = "SELECT max(tjv.Received_ReceivedNumber) FROM tbl_trans_OldUnitReceived tjv WHERE dbo.RegexMatch('";
                    if (prefLen > 0)
                        sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                    else if (scheme_type == 2)
                        sqlQuery += "^";
                    sqlQuery += "[0-9]{" + paddCounter + "}";
                    if (sufxLen > 0)
                        sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                    //sqlQuery += "?$', LTRIM(RTRIM(tjv.PurchaseOrder_Number))) = 1";
                    sqlQuery += "?$', LTRIM(RTRIM(tjv.Received_ReceivedNumber))) = 1 and Received_ReceivedNumber like '" + prefCompCode + "%'";
                    if (scheme_type == 2)
                        sqlQuery += " AND CONVERT(DATE, CreatedDate) = CONVERT(DATE, GETDATE())";
                    dtC = oDBEngine.GetDataTable(sqlQuery);

                    if (dtC.Rows[0][0].ToString() == "")
                    {
                        sqlQuery = "SELECT max(tjv.Received_ReceivedNumber) FROM tbl_trans_OldUnitReceived tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.PurchaseOrder_Number))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.Received_ReceivedNumber))) = 1 and Received_ReceivedNumber like '" + prefCompCode + "%'";
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
                    sqlQuery = "SELECT Received_ReceivedNumber FROM tbl_trans_OldUnitReceived WHERE Received_ReceivedNumber LIKE '" + manual_str.Trim() + "'";
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
        public string checkNMakeJVCodeDN(string manual_str, int sel_schema_Id)
        {
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

                    sqlQuery = "SELECT max(tjv.DCNote_DocumentNumber) FROM Trans_CustDebitCreditNote tjv WHERE dbo.RegexMatch('";
                    if (prefLen > 0)
                        sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                    else if (scheme_type == 2)
                        sqlQuery += "^";
                    sqlQuery += "[0-9]{" + paddCounter + "}";
                    if (sufxLen > 0)
                        sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                    //sqlQuery += "?$', LTRIM(RTRIM(tjv.DCNote_DocumentNumber))) = 1";
                    sqlQuery += "?$', LTRIM(RTRIM(tjv.DCNote_DocumentNumber))) = 1 and DCNote_DocumentNumber like '" + prefCompCode + "%'";
                    if (scheme_type == 2)
                        sqlQuery += " AND CONVERT(DATE, JournalVoucher_CreateDateTime) = CONVERT(DATE, GETDATE())";
                    dtC = oDBEngine.GetDataTable(sqlQuery);

                    if (dtC.Rows[0][0].ToString() == "")
                    {
                        sqlQuery = "SELECT max(tjv.DCNote_DocumentNumber) FROM Trans_CustDebitCreditNote tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        // sqlQuery += "?$', LTRIM(RTRIM(tjv.DCNote_DocumentNumber))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.DCNote_DocumentNumber))) = 1 and DCNote_DocumentNumber like '" + prefCompCode + "%'";
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
                            UniqueQuotationDN = prefCompCode + paddedStr + sufxCompCode;
                            return "ok";
                        }
                    }
                    else
                    {
                        UniqueQuotationDN = startNo.PadLeft(paddCounter, '0');
                        UniqueQuotationDN = prefCompCode + paddedStr + sufxCompCode;
                        return "ok";
                    }
                }
                else
                {
                    sqlQuery = "SELECT DCNote_DocumentNumber FROM Trans_CustDebitCreditNote WHERE DCNote_DocumentNumber LIKE '" + manual_str.Trim() + "'";
                    dtC = oDBEngine.GetDataTable(sqlQuery);
                    // duplicate manual entry check
                    if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
                    {
                        return "duplicate";
                    }

                    UniqueQuotationDN = manual_str.Trim();
                    return "ok";
                }
            }
            else
            {
                return "noid";
            }
        }
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
            ProcedureExecute proc = new ProcedureExecute("prc_SalesChallan_Details");
            proc.AddVarcharPara("@Action", 500, "TaxDetailsForGst");
            proc.AddVarcharPara("@Challan_Id", 500, Convert.ToString(Session["Invoice_ID"]));
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
            DataTable DT = proc.GetTable();
            cmbGstCstVatcharge.DataSource = DT;
            cmbGstCstVatcharge.TextField = "Taxes_Name";
            cmbGstCstVatcharge.ValueField = "Taxes_ID";
            cmbGstCstVatcharge.DataBind();
        }



        protected void gridTax_DataBinding(object sender, EventArgs e)
        {
            if (Session["OldUnitStockInvoiceTaxDetails"] != null)
            {
                DataTable TaxDetailsdt = (DataTable)Session["OldUnitStockInvoiceTaxDetails"];

                //gridTax.DataSource = GetTaxes();
                var taxlist = (List<Taxes>)GetTaxes(TaxDetailsdt);
                var taxChargeDataSource = setChargeCalculatedOn(taxlist, TaxDetailsdt);
                gridTax.DataSource = taxChargeDataSource;
            }
        }
        public void GetStock(string strProductID)
        {
            string strBranch = Convert.ToString(ddl_transferFrom_Branch.SelectedValue);
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
                DataTable MainTaxDataTable = (DataTable)Session["OldUnitStockInvoiceFinalTaxRecord"];

                DataRow[] deletedRow = MainTaxDataTable.Select("SlNo=" + performpara.Split('~')[1]);
                if (deletedRow.Length > 0)
                {
                    foreach (DataRow dr in deletedRow)
                    {
                        MainTaxDataTable.Rows.Remove(dr);
                    }

                }

                Session["OldUnitStockInvoiceFinalTaxRecord"] = MainTaxDataTable;
                GetStock(Convert.ToString(performpara.Split('~')[1]));
                DeleteWarehouse(Convert.ToString(performpara.Split('~')[1]));
            }
            else
            {
                DataTable MainTaxDataTable = (DataTable)Session["OldUnitStockInvoiceFinalTaxRecord"];

                DataRow[] deletedRow = MainTaxDataTable.Select("SlNo=" + performpara.Split('~')[1]);
                if (deletedRow.Length > 0)
                {
                    foreach (DataRow dr in deletedRow)
                    {
                        MainTaxDataTable.Rows.Remove(dr);
                    }

                }

                Session["OldUnitStockInvoiceFinalTaxRecord"] = MainTaxDataTable;
            }
        }
        public double ReCalculateTaxAmount(string slno, double amount)
        {
            DataTable MainTaxDataTable = (DataTable)Session["OldUnitStockInvoiceFinalTaxRecord"];
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
            Session["OldUnitStockInvoiceFinalTaxRecord"] = MainTaxDataTable;

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
            Session["OldUnitStockInvoiceFinalTaxRecord"] = TaxRecord;
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
            ProcedureExecute proc = new ProcedureExecute("prc_SalesChallan_Details");
            proc.AddVarcharPara("@Action", 500, "SalesChallanInlineTax");
            proc.AddVarcharPara("@Challan_Id", 500, Convert.ToString(Session["Invoice_ID"]));
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
                DataTable TaxRecord = (DataTable)Session["OldUnitStockInvoiceFinalTaxRecord"];
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

                Session["OldUnitStockInvoiceFinalTaxRecord"] = TaxRecord;
            }
            else
            {
                #region fetch All data For Tax

                DataTable taxDetail = new DataTable();
                DataTable MainTaxDataTable = (DataTable)Session["OldUnitStockInvoiceFinalTaxRecord"];
                DataTable databaseReturnTable = (DataTable)Session["QuotationTaxDetails"];

                //if (Convert.ToInt32(e.Parameters.Split('~')[1]) == 1)
                //    taxDetail = oDBEngine.GetDataTable("select TaxRates_ID as Taxes_ID,TaxRatesSchemeName as Taxes_Name,TaxCalculateMethods,tm.Taxes_Name as taxCodeName,tm.Taxes_ApplicableOn as 'ApplicableOn',isnull((select TaxRatesSchemeName from Config_TaxRates where TaxRates_ID=tm.Taxes_OtherTax),'')  as dependOn from Master_taxes tm inner join Config_TaxRates ct on tm.Taxes_ID=ct.TaxRates_TaxCode where tm.TaxTypeCode not in('G','V','C') and tm.TaxItemlevel='Y'");
                //else if (Convert.ToInt32(e.Parameters.Split('~')[1]) == 2)
                //taxDetail = oDBEngine.GetDataTable("select TaxRates_ID as Taxes_ID,TaxRatesSchemeName as Taxes_Name,TaxCalculateMethods,tm.Taxes_Name as taxCodeName,tm.Taxes_ApplicableOn as 'ApplicableOn',isnull((select TaxRatesSchemeName from Config_TaxRates where TaxRates_ID=tm.Taxes_OtherTax),'')  as dependOn from Master_taxes tm inner join Config_TaxRates ct on tm.Taxes_ID=ct.TaxRates_TaxCode where tm.TaxTypeCode not in('G','V','C') and tm.TaxItemlevel='Y'");

                ProcedureExecute proc = new ProcedureExecute("prc_SalesCRM_Details");
                proc.AddVarcharPara("@Action", 500, "LoadOtherTaxDetails");
                proc.AddVarcharPara("@S_quoteDate", 10, dt_BTOut.Date.ToString("yyyy-MM-dd"));
                taxDetail = proc.GetTable();

                //Get Company Gstin 09032017
                string CompInternalId = Convert.ToString(Session["LastCompany"]);
                string[] compGstin = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_gstin", "cmp_internalid='" + CompInternalId + "'", 1);

                string ShippingState = "";
                if (chkBilling.Checked)
                {
                    if (CmbState.Value != null)
                    {
                        ShippingState = CmbState.Text;
                        ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                    }
                }
                else
                {
                    if (CmbState1.Value != null)
                    {
                        ShippingState = CmbState1.Text;
                        ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                    }
                }



                if (ShippingState.Trim() != "")
                {

                    if (compGstin.Length > 0)
                    {
                        if (compGstin[0].Substring(0, 2) == ShippingState)
                        {
                            foreach (DataRow dr in taxDetail.Rows)
                            {
                                if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST")
                                {
                                    dr.Delete();
                                }
                            }
                            taxDetail.AcceptChanges();
                        }
                        else
                        {
                            foreach (DataRow dr in taxDetail.Rows)
                            {
                                if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                                {
                                    dr.Delete();
                                }
                            }
                            taxDetail.AcceptChanges();

                        }

                    }
                }
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

                    DataTable TaxRecord = (DataTable)Session["OldUnitStockInvoiceFinalTaxRecord"];


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
                    Session["OldUnitStockInvoiceFinalTaxRecord"] = TaxRecord;

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
        protected void taxgrid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {

            int slNo = Convert.ToInt32(HdSerialNo.Value);
            DataTable TaxRecord = (DataTable)Session["OldUnitStockInvoiceFinalTaxRecord"];
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


            Session["OldUnitStockInvoiceFinalTaxRecord"] = TaxRecord;


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
            DateTime quoteDate = Convert.ToDateTime(dt_BTOut.Date.ToString("yyyy-MM-dd"));

            PopulateGSTCSTVATCombo(quoteDate.ToString("yyyy-MM-dd"));
            CreateDataTaxTable();
        }

        protected void cmbGstCstVatcharge_Callback(object sender, CallbackEventArgsBase e)
        {
            Session["OldUnitStockInvoiceTaxDetails"] = null;
            DateTime quoteDate = Convert.ToDateTime(dt_BTOut.Date.ToString("yyyy-MM-dd"));
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
            if (Session["OldUnitStockInvoiceFinalTaxRecord"] != null)
            {
                TaxDetailTable = (DataTable)Session["OldUnitStockInvoiceFinalTaxRecord"];

                var rows = TaxDetailTable.Select("SlNo ='" + SrlNo + "'");
                foreach (var row in rows)
                {
                    row.Delete();
                }
                TaxDetailTable.AcceptChanges();

                Session["OldUnitStockInvoiceFinalTaxRecord"] = TaxDetailTable;
            }
        }
        public void UpdateTaxDetails(string oldSrlNo, string newSrlNo)
        {
            DataTable TaxDetailTable = new DataTable();
            if (Session["OldUnitStockInvoiceFinalTaxRecord"] != null)
            {
                TaxDetailTable = (DataTable)Session["OldUnitStockInvoiceFinalTaxRecord"];

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

                Session["OldUnitStockInvoiceFinalTaxRecord"] = TaxDetailTable;
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
            if (Session["OUR_WarehouseData"] != null)
            {
                Warehousedt = (DataTable)Session["OUR_WarehouseData"];

                var rows = Warehousedt.Select(string.Format("Product_SrlNo ='{0}'", SrlNo));
                foreach (var row in rows)
                {
                    row.Delete();
                }
                Warehousedt.AcceptChanges();

                Session["OUR_WarehouseData"] = Warehousedt;
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

        public DataTable StoreSalesOrderAddressDetail()
        {
            //QuoteAdd_id, QuoteAdd_QuoteId, QuoteAdd_CompanyID, QuoteAdd_BranchId, QuoteAdd_FinYear,
            //QuoteAdd_ContactPerson, QuoteAdd_addressType, QuoteAdd_address1, QuoteAdd_address2, QuoteAdd_address3, 
            //QuoteAdd_landMark, QuoteAdd_countryId, QuoteAdd_stateId, QuoteAdd_cityId, QuoteAdd_areaId, 
            //QuoteAdd_pin, QuoteAdd_CreatedDate, QuoteAdd_CreatedUser, QuoteAdd_LastModifyDate, QuoteAdd_LastModifyUser

            DataTable AddressDetaildt = new DataTable();

            AddressDetaildt.Columns.Add("OrderAdd_OrderId", typeof(System.Int32));
            AddressDetaildt.Columns.Add("OrderAdd_CompanyID", typeof(System.String));
            AddressDetaildt.Columns.Add("OrderAdd_BranchId", typeof(System.Int32));
            AddressDetaildt.Columns.Add("OrderAdd_FinYear", typeof(System.String));

            AddressDetaildt.Columns.Add("OrderAdd_ContactPerson", typeof(System.String));
            AddressDetaildt.Columns.Add("OrderAdd_addressType", typeof(System.String));
            AddressDetaildt.Columns.Add("OrderAdd_address1", typeof(System.String));
            AddressDetaildt.Columns.Add("OrderAdd_address2", typeof(System.String));
            AddressDetaildt.Columns.Add("OrderAdd_address3", typeof(System.String));


            AddressDetaildt.Columns.Add("OrdereAdd_landMark", typeof(System.String));
            AddressDetaildt.Columns.Add("OrderAdd_countryId", typeof(System.Int32));
            AddressDetaildt.Columns.Add("OrderAdd_stateId", typeof(System.Int32));
            AddressDetaildt.Columns.Add("OrderAdd_cityId", typeof(System.Int32));
            AddressDetaildt.Columns.Add("OrderAdd_areaId", typeof(System.Int32));


            AddressDetaildt.Columns.Add("OrderAdd_pin", typeof(System.String));
            AddressDetaildt.Columns.Add("OrderAdd_CreatedDate", typeof(System.DateTime));
            AddressDetaildt.Columns.Add("OrderAdd_CreatedUser", typeof(System.Int32));
            AddressDetaildt.Columns.Add("OrderAdd_LastModifyDate", typeof(System.DateTime));
            AddressDetaildt.Columns.Add("OrderAdd_LastModifyUser", typeof(System.Int32));
            return AddressDetaildt;


        }
        protected void ComponentPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            #region Addresss Lookup Section Start
            DataSet dst = new DataSet();
            CRMSalesDtlBL objCRMSalesDtlBL = new CRMSalesDtlBL();
            dst = objCRMSalesDtlBL.PopulateBillingandShippingDetailByCustomerID(hdnCustomerId.Value);
            billingAddress.DataSource = dst.Tables[0];
            billingAddress.DataBind();
            if (dst.Tables[0].Rows.Count > 0)
            {
                Session["RcvFrmServBillingAddressLookup"] = dst.Tables[0];
            }
            shippingAddress.DataSource = dst.Tables[1];
            shippingAddress.DataBind();
            if (dst.Tables[1].Rows.Count > 0)
            {
                Session["OldUnitStockInvoiceShippingAddressLookup"] = dst.Tables[1];
            }


            #endregion Addresss Lookup Section End

            #region Variable Declaration to send value using jsproperties Start
            string add_addressType = "";
            string add_address1 = "";
            string add_address2 = "";
            string add_address3 = "";
            string add_landMark = "";
            string add_country = "";
            string add_state = "";
            string add_city = "";
            string add_pin = "";
            string add_area = "";

            ///// shipping variable

            string add_saddressType = "";
            string add_saddress1 = "";
            string add_saddress2 = "";
            string add_saddress3 = "";
            string add_slandMark = "";
            string add_scountry = "";
            string add_sstate = "";
            string add_scity = "";
            string add_spin = "";
            string add_sarea = "";

            #endregion Variable Declaration to send value using jsproperties Start
            ComponentPanel.JSProperties["cpshow"] = null;
            ComponentPanel.JSProperties["cpshowShip"] = null;
            string WhichCall = e.Parameter.Split('~')[0];

            #region BillingLookup Edit Section Start
            if (WhichCall == "BlookupEdit")
            {
                int BillingAddressID = Convert.ToInt32(e.Parameter.Split('~')[1]);
                DataTable dt = objCRMSalesDtlBL.PopulateAddressDetailByAddressId(BillingAddressID);
                if (dt.Rows.Count > 0 && dt != null)
                {
                    //for (int m = 0; m < dtaddbill.Rows.Count; m++)
                    //{
                    add_addressType = Convert.ToString(dt.Rows[0]["add_addressType"]);
                    add_address1 = Convert.ToString(dt.Rows[0]["add_address1"]);
                    add_address2 = Convert.ToString(dt.Rows[0]["add_address2"]);
                    add_address3 = Convert.ToString(dt.Rows[0]["add_address3"]);
                    add_landMark = Convert.ToString(dt.Rows[0]["add_landMark"]);
                    add_country = Convert.ToString(dt.Rows[0]["add_country"]);
                    add_state = Convert.ToString(dt.Rows[0]["add_state"]);
                    add_city = Convert.ToString(dt.Rows[0]["add_city"]);
                    add_pin = Convert.ToString(dt.Rows[0]["add_pin"]);
                    add_area = Convert.ToString(dt.Rows[0]["add_area"]);

                    ComponentPanel.JSProperties["cpshow"] = add_addressType + "~"
                                                       + add_address1 + "~"
                                                       + add_address2 + "~"
                                                       + add_address3 + "~"
                                                       + add_landMark + "~"
                                                       + add_country + "~"
                                                       + add_state + "~"
                                                       + add_city + "~"
                                                       + add_pin + "~"
                                                       + add_area + "~"
                                                       + "Y" + "~";

                }
                else
                {
                    ComponentPanel.JSProperties["cpshow"] = add_addressType + "~"
                                                      + add_address1 + "~"
                                                      + add_address2 + "~"
                                                      + add_address3 + "~"
                                                      + add_landMark + "~"
                                                      + add_country + "~"
                                                      + add_state + "~"
                                                      + add_city + "~"
                                                      + add_pin + "~"
                                                      + add_area + "~"
                                                       + "N" + "~";
                }

            }



            #endregion BillingLookup Edit Section Start

            #region ShippingLookup Edit Section Start
            if (WhichCall == "SlookupEdit")
            {
                int AddressID = Convert.ToInt32(e.Parameter.Split('~')[1]);
                DataTable dt = objCRMSalesDtlBL.PopulateAddressDetailByAddressId(AddressID);
                if (dt.Rows.Count > 0 && dt != null)
                {
                    add_saddressType = Convert.ToString(dt.Rows[0]["add_addressType"]);
                    add_saddress1 = Convert.ToString(dt.Rows[0]["add_address1"]);
                    add_saddress2 = Convert.ToString(dt.Rows[0]["add_address2"]);
                    add_saddress3 = Convert.ToString(dt.Rows[0]["add_address3"]);
                    add_slandMark = Convert.ToString(dt.Rows[0]["add_landMark"]);
                    add_scountry = Convert.ToString(dt.Rows[0]["add_country"]);
                    add_sstate = Convert.ToString(dt.Rows[0]["add_state"]);
                    add_scity = Convert.ToString(dt.Rows[0]["add_city"]);
                    add_spin = Convert.ToString(dt.Rows[0]["add_pin"]);
                    add_sarea = Convert.ToString(dt.Rows[0]["add_area"]);

                    ComponentPanel.JSProperties["cpshowShip"] = add_saddressType + "~"
                                                      + add_saddress1 + "~"
                                                      + add_saddress2 + "~"
                                                      + add_saddress3 + "~"
                                                      + add_slandMark + "~"
                                                      + add_scountry + "~"
                                                      + add_sstate + "~"
                                                      + add_scity + "~"
                                                      + add_spin + "~"
                                                      + add_sarea + "~"
                                                      + "Y" + "~";

                }
                else
                {
                    ComponentPanel.JSProperties["cpshowShip"] = add_saddressType + "~"
                                                      + add_saddress1 + "~"
                                                      + add_saddress2 + "~"
                                                      + add_saddress3 + "~"
                                                      + add_slandMark + "~"
                                                      + add_scountry + "~"
                                                      + add_sstate + "~"
                                                      + add_scity + "~"
                                                      + add_spin + "~"
                                                      + add_sarea + "~"
                                                       + "N" + "~";
                }

            }

            #endregion ShippingLookup Edit Section End
            #region Edit Section of Address Start

            if (WhichCall == "Edit")
            {
                //DataTable dtaddress=(DataTable)
                //string AddressStatus = Convert.ToString(e.Parameter.Split('~')[1]);
                if (Session["OldUnitStockInvoiceAddressDtl"] == null)
                {
                    string customerid = hdnCustomerId.Value;
                    #region Billing Detail fillup
                    DataTable dtaddbill = oDBEngine.GetDataTable("select add_addressType,add_address1,add_address2,add_address3,add_landMark,add_country,add_state,add_city,add_pin,add_area from tbl_master_address where add_cntId='" + customerid + "' and add_addressType='Billing' and Isdefault='1' ");

                    #region Function To get All Detail

                    if (dtaddbill.Rows.Count > 0 && dtaddbill != null)
                    {

                        //for (int m = 0; m < dtaddbill.Rows.Count; m++)
                        //{
                        add_addressType = Convert.ToString(dtaddbill.Rows[0]["add_addressType"]);
                        add_address1 = Convert.ToString(dtaddbill.Rows[0]["add_address1"]);
                        add_address2 = Convert.ToString(dtaddbill.Rows[0]["add_address2"]);
                        add_address3 = Convert.ToString(dtaddbill.Rows[0]["add_address3"]);
                        add_landMark = Convert.ToString(dtaddbill.Rows[0]["add_landMark"]);
                        add_country = Convert.ToString(dtaddbill.Rows[0]["add_country"]);
                        add_state = Convert.ToString(dtaddbill.Rows[0]["add_state"]);
                        add_city = Convert.ToString(dtaddbill.Rows[0]["add_city"]);
                        add_pin = Convert.ToString(dtaddbill.Rows[0]["add_pin"]);
                        add_area = Convert.ToString(dtaddbill.Rows[0]["add_area"]);

                        //}

                        ComponentPanel.JSProperties["cpshow"] = add_addressType + "~"
                                                           + add_address1 + "~"
                                                           + add_address2 + "~"
                                                           + add_address3 + "~"
                                                           + add_landMark + "~"
                                                           + add_country + "~"
                                                           + add_state + "~"
                                                           + add_city + "~"
                                                           + add_pin + "~"
                                                           + add_area + "~"
                                                           + "Y" + "~";

                    }
                    else
                    {
                        ComponentPanel.JSProperties["cpshow"] = add_addressType + "~"
                                                          + add_address1 + "~"
                                                          + add_address2 + "~"
                                                          + add_address3 + "~"
                                                          + add_landMark + "~"
                                                          + add_country + "~"
                                                          + add_state + "~"
                                                          + add_city + "~"
                                                          + add_pin + "~"
                                                          + add_area + "~"
                                                           + "N" + "~";
                    }
                    #endregion Function Calling End
                    #endregion Billing Detail fillup end
                    #region Shipping Detail fillup
                    DataTable dtaship = oDBEngine.GetDataTable("select add_addressType,add_address1,add_address2,add_address3,add_landMark,add_country,add_state,add_city,add_pin,add_area from tbl_master_address where add_cntId='" + customerid + "' and add_addressType='Shipping' and Isdefault='1' ");
                    if (dtaship.Rows.Count > 0 && dtaship != null)
                    {
                        add_saddressType = Convert.ToString(dtaship.Rows[0]["add_addressType"]);
                        add_saddress1 = Convert.ToString(dtaship.Rows[0]["add_address1"]);
                        add_saddress2 = Convert.ToString(dtaship.Rows[0]["add_address2"]);
                        add_saddress3 = Convert.ToString(dtaship.Rows[0]["add_address3"]);
                        add_slandMark = Convert.ToString(dtaship.Rows[0]["add_landMark"]);
                        add_scountry = Convert.ToString(dtaship.Rows[0]["add_country"]);
                        add_sstate = Convert.ToString(dtaship.Rows[0]["add_state"]);
                        add_scity = Convert.ToString(dtaship.Rows[0]["add_city"]);
                        add_spin = Convert.ToString(dtaship.Rows[0]["add_pin"]);
                        add_sarea = Convert.ToString(dtaship.Rows[0]["add_area"]);

                        ComponentPanel.JSProperties["cpshowShip"] = add_saddressType + "~"
                                                          + add_saddress1 + "~"
                                                          + add_saddress2 + "~"
                                                          + add_saddress3 + "~"
                                                          + add_slandMark + "~"
                                                          + add_scountry + "~"
                                                          + add_sstate + "~"
                                                          + add_scity + "~"
                                                          + add_spin + "~"
                                                          + add_sarea + "~"
                                                          + "Y" + "~";

                    }
                    else
                    {
                        ComponentPanel.JSProperties["cpshowShip"] = add_saddressType + "~"
                                                          + add_saddress1 + "~"
                                                          + add_saddress2 + "~"
                                                          + add_saddress3 + "~"
                                                          + add_slandMark + "~"
                                                          + add_scountry + "~"
                                                          + add_sstate + "~"
                                                          + add_scity + "~"
                                                          + add_spin + "~"
                                                          + add_sarea + "~"
                                                           + "N" + "~";
                    }
                    #endregion Shipping detail Fillup
                }
                else if (Session["OldUnitStockInvoiceAddressDtl"] != null)
                {
                    DataTable dt = (DataTable)Session["OldUnitStockInvoiceAddressDtl"];
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows.Count == 2) // when 2 row  data found in edit mode
                        {
                            #region billing Address Dtl using session
                            add_addressType = Convert.ToString(dt.Rows[0]["ChallanAdd_addressType"]);
                            add_address1 = Convert.ToString(dt.Rows[0]["ChallanAdd_address1"]);
                            add_address2 = Convert.ToString(dt.Rows[0]["ChallanAdd_address2"]);
                            add_address3 = Convert.ToString(dt.Rows[0]["ChallanAdd_address3"]);
                            add_landMark = Convert.ToString(dt.Rows[0]["ChallanAdd_landMark"]);
                            add_country = Convert.ToString(dt.Rows[0]["ChallanAdd_countryId"]);
                            add_state = Convert.ToString(dt.Rows[0]["ChallanAdd_stateId"]);
                            add_city = Convert.ToString(dt.Rows[0]["ChallanAdd_cityId"]);
                            add_pin = Convert.ToString(dt.Rows[0]["ChallanAdd_pin"]);
                            add_area = Convert.ToString(dt.Rows[0]["ChallanAdd_areaId"]);
                            ComponentPanel.JSProperties["cpshow"] = add_addressType + "~"
                                                                  + add_address1 + "~"
                                                                  + add_address2 + "~"
                                                                  + add_address3 + "~"
                                                                  + add_landMark + "~"
                                                                  + add_country + "~"
                                                                  + add_state + "~"
                                                                  + add_city + "~"
                                                                  + add_pin + "~"
                                                                  + add_area + "~"
                                                                  + "Y" + "~";
                            #endregion billing Address Dtl using session
                            #region Shipping Address Dtl using session
                            add_saddressType = Convert.ToString(dt.Rows[1]["ChallanAdd_addressType"]);
                            add_saddress1 = Convert.ToString(dt.Rows[1]["ChallanAdd_address1"]);
                            add_saddress2 = Convert.ToString(dt.Rows[1]["ChallanAdd_address2"]);
                            add_saddress3 = Convert.ToString(dt.Rows[1]["ChallanAdd_address3"]);
                            add_slandMark = Convert.ToString(dt.Rows[1]["ChallanAdd_landMark"]);
                            add_scountry = Convert.ToString(dt.Rows[1]["ChallanAdd_countryId"]);
                            add_sstate = Convert.ToString(dt.Rows[1]["ChallanAdd_stateId"]);
                            add_scity = Convert.ToString(dt.Rows[1]["ChallanAdd_cityId"]);
                            add_spin = Convert.ToString(dt.Rows[1]["ChallanAdd_pin"]);
                            add_sarea = Convert.ToString(dt.Rows[1]["ChallanAdd_areaId"]);
                            ComponentPanel.JSProperties["cpshowShip"] = add_saddressType + "~"
                                                                 + add_saddress1 + "~"
                                                                 + add_saddress2 + "~"
                                                                 + add_saddress3 + "~"
                                                                 + add_slandMark + "~"
                                                                 + add_scountry + "~"
                                                                 + add_sstate + "~"
                                                                 + add_scity + "~"
                                                                 + add_spin + "~"
                                                                 + add_sarea + "~"
                                                                 + "Y" + "~";
                            #endregion Shipping Address Dtl using session end

                        }
                        else if (dt.Rows.Count == 1) // when 1 row  data found in edit mode
                        {
                            if (Convert.ToString(dt.Rows[0]["ChallanAdd_addressType"]) == "Billing")
                            {
                                #region billing Address Dtl using session
                                add_addressType = Convert.ToString(dt.Rows[0]["ChallanAdd_addressType"]);
                                add_address1 = Convert.ToString(dt.Rows[0]["ChallanAdd_address1"]);
                                add_address2 = Convert.ToString(dt.Rows[0]["ChallanAdd_address2"]);
                                add_address3 = Convert.ToString(dt.Rows[0]["ChallanAdd_address3"]);
                                add_landMark = Convert.ToString(dt.Rows[0]["ChallanAdd_landMark"]);
                                add_country = Convert.ToString(dt.Rows[0]["ChallanAdd_countryId"]);
                                add_state = Convert.ToString(dt.Rows[0]["ChallanAdd_stateId"]);
                                add_city = Convert.ToString(dt.Rows[0]["ChallanAdd_cityId"]);
                                add_pin = Convert.ToString(dt.Rows[0]["ChallanAdd_pin"]);
                                add_area = Convert.ToString(dt.Rows[0]["ChallanAdd_areaId"]);
                                ComponentPanel.JSProperties["cpshow"] = add_addressType + "~"
                                                                      + add_address1 + "~"
                                                                      + add_address2 + "~"
                                                                      + add_address3 + "~"
                                                                      + add_landMark + "~"
                                                                      + add_country + "~"
                                                                      + add_state + "~"
                                                                      + add_city + "~"
                                                                      + add_pin + "~"
                                                                      + add_area + "~"
                                                                      + "Y" + "~";

                                ComponentPanel.JSProperties["cpshowShip"] = add_saddressType + "~"
                                                     + add_saddress1 + "~"
                                                     + add_saddress2 + "~"
                                                     + add_saddress3 + "~"
                                                     + add_slandMark + "~"
                                                     + add_scountry + "~"
                                                     + add_sstate + "~"
                                                     + add_scity + "~"
                                                     + add_spin + "~"
                                                     + add_sarea + "~"
                                                      + "N" + "~";

                                #endregion billing Address Dtl using session
                            }
                            if (Convert.ToString(dt.Rows[0]["ChallanAdd_addressType"]) == "Shipping")
                            {
                                #region Shipping Address Dtl using session
                                ComponentPanel.JSProperties["cpshow"] = add_addressType + "~"
                                                     + add_address1 + "~"
                                                     + add_address2 + "~"
                                                     + add_address3 + "~"
                                                     + add_landMark + "~"
                                                     + add_country + "~"
                                                     + add_state + "~"
                                                     + add_city + "~"
                                                     + add_pin + "~"
                                                     + add_area + "~"
                                                      + "N" + "~";

                                add_saddressType = Convert.ToString(dt.Rows[0]["ChallanAdd_addressType"]);
                                add_saddress1 = Convert.ToString(dt.Rows[0]["ChallanAdd_address1"]);
                                add_saddress2 = Convert.ToString(dt.Rows[0]["ChallanAdd_address2"]);
                                add_saddress3 = Convert.ToString(dt.Rows[0]["ChallanAdd_address3"]);
                                add_slandMark = Convert.ToString(dt.Rows[0]["ChallanAdd_landMark"]);
                                add_scountry = Convert.ToString(dt.Rows[0]["ChallanAdd_countryId"]);
                                add_sstate = Convert.ToString(dt.Rows[0]["ChallanAdd_stateId"]);
                                add_scity = Convert.ToString(dt.Rows[0]["ChallanAdd_cityId"]);
                                add_spin = Convert.ToString(dt.Rows[0]["ChallanAdd_pin"]);
                                add_sarea = Convert.ToString(dt.Rows[0]["ChallanAdd_areaId"]);
                                ComponentPanel.JSProperties["cpshowShip"] = add_saddressType + "~"
                                                                     + add_saddress1 + "~"
                                                                     + add_saddress2 + "~"
                                                                     + add_saddress3 + "~"
                                                                     + add_slandMark + "~"
                                                                     + add_scountry + "~"
                                                                     + add_sstate + "~"
                                                                     + add_scity + "~"
                                                                     + add_spin + "~"
                                                                     + add_sarea + "~"
                                                                     + "Y" + "~";
                                #endregion Shipping Address Dtl using session end
                            }
                        }
                        else // when no data found in edit mode
                        {
                            #region billing Address Dtl using session
                            //add_addressType = Convert.ToString(dt.Rows[0]["QuoteAdd_addressType"]);
                            //add_address1 = Convert.ToString(dt.Rows[0]["QuoteAdd_address1"]);
                            //add_address2 = Convert.ToString(dt.Rows[0]["QuoteAdd_address2"]);
                            //add_address3 = Convert.ToString(dt.Rows[0]["QuoteAdd_address3"]);
                            //add_landMark = Convert.ToString(dt.Rows[0]["QuoteAdd_landMark"]);
                            //add_country = Convert.ToString(dt.Rows[0]["QuoteAdd_countryId"]);
                            //add_state = Convert.ToString(dt.Rows[0]["QuoteAdd_stateId"]);
                            //add_city = Convert.ToString(dt.Rows[0]["QuoteAdd_cityId"]);
                            //add_pin = Convert.ToString(dt.Rows[0]["QuoteAdd_pin"]);
                            //add_area = Convert.ToString(dt.Rows[0]["QuoteAdd_areaId"]);
                            ComponentPanel.JSProperties["cpshow"] = add_addressType + "~"
                                                                  + add_address1 + "~"
                                                                  + add_address2 + "~"
                                                                  + add_address3 + "~"
                                                                  + add_landMark + "~"
                                                                  + add_country + "~"
                                                                  + add_state + "~"
                                                                  + add_city + "~"
                                                                  + add_pin + "~"
                                                                  + add_area + "~"
                                                                  + "Y" + "~";
                            #endregion billing Address Dtl using session
                            #region Shipping Address Dtl using session
                            //add_saddressType = Convert.ToString(dt.Rows[1]["QuoteAdd_addressType"]);
                            //add_saddress1 = Convert.ToString(dt.Rows[1]["QuoteAdd_address1"]);
                            //add_saddress2 = Convert.ToString(dt.Rows[1]["QuoteAdd_address2"]);
                            //add_saddress3 = Convert.ToString(dt.Rows[1]["QuoteAdd_address3"]);
                            //add_slandMark = Convert.ToString(dt.Rows[1]["QuoteAdd_landMark"]);
                            //add_scountry = Convert.ToString(dt.Rows[1]["QuoteAdd_countryId"]);
                            //add_sstate = Convert.ToString(dt.Rows[1]["QuoteAdd_stateId"]);
                            //add_scity = Convert.ToString(dt.Rows[1]["QuoteAdd_cityId"]);
                            //add_spin = Convert.ToString(dt.Rows[1]["QuoteAdd_pin"]);
                            //add_sarea = Convert.ToString(dt.Rows[1]["QuoteAdd_areaId"]);
                            ComponentPanel.JSProperties["cpshowShip"] = add_saddressType + "~"
                                                                 + add_saddress1 + "~"
                                                                 + add_saddress2 + "~"
                                                                 + add_saddress3 + "~"
                                                                 + add_slandMark + "~"
                                                                 + add_scountry + "~"
                                                                 + add_sstate + "~"
                                                                 + add_scity + "~"
                                                                 + add_spin + "~"
                                                                 + add_sarea + "~"
                                                                 + "Y" + "~";

                            #endregion Shipping Address Dtl using session end

                        }
                    }
                }
            }
            #endregion Edit Section of Address End

            #region Save Section of Address Start
            if (WhichCall == "save")
            {
                #region Global Data for Address Start
                string companyId = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                int branchid = Convert.ToInt32(HttpContext.Current.Session["userbranchID"]);
                string fin_year = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
                int userid = Convert.ToInt32(HttpContext.Current.Session["userid"]);
                #endregion Global Data for Address End
                string AddressStatus = e.Parameter.Split('~')[1];
                if (AddressStatus == "1") // Both Billing and Shipping Address Available
                {
                    #region Billing Address Detail Start
                    string contactperson = "";


                    //int insertcount = 0;

                    //string AddressType = Convert.ToString(CmbAddressType.SelectedItem.Value);
                    string AddressType = "Billing";
                    string address1 = txtAddress1.Text;
                    string address2 = txtAddress2.Text;
                    string address3 = txtAddress3.Text;
                    string landmark = txtlandmark.Text;
                    int country = Convert.ToInt32(CmbCountry.SelectedItem.Value);
                    int State = Convert.ToInt32(CmbState.Value);
                    int city = Convert.ToInt32(CmbCity.Value);
                    int area = Convert.ToInt32(CmbArea.Value);
                    string pin = Convert.ToString(CmbPin.Value);
                    DataTable dt = StoreSalesOrderAddressDetail();
                    dt.Rows.Add(0, companyId, branchid, fin_year, "", AddressType, address1, address2, address3, landmark, country, State, city, area, pin, System.DateTime.Now, userid, System.DateTime.Now, userid);




                    #endregion Billing Address Detail Start end

                    #region Shipping Address Detail Start
                    // CRMSalesAddressEL objCRMSalesSAddress  = new CRMSalesAddressEL();
                    string scontactperson = "";
                    //string sAddressType = Convert.ToString(CmbAddressType1.SelectedItem.Value);
                    string sAddressType = "Shipping";
                    string saddress1 = txtsAddress1.Text;
                    string saddress2 = txtsAddress2.Text;
                    string saddress3 = txtsAddress3.Text;
                    string slandmark = txtslandmark.Text;
                    int scountry = Convert.ToInt32(CmbCountry1.SelectedItem.Value);
                    int sState = Convert.ToInt32(CmbState1.Value);
                    int scity = Convert.ToInt32(CmbCity1.Value);
                    int sarea = Convert.ToInt32(CmbArea1.Value);
                    string spin = Convert.ToString(CmbPin1.Value);
                    dt.Rows.Add(0, companyId, branchid, fin_year, "", sAddressType, saddress1, saddress2, saddress3, slandmark, scountry, sState, scity, sarea, spin, System.DateTime.Now, userid, System.DateTime.Now, userid);


                    Session["OldUnitStockInvoiceAddressDtl"] = dt;
                    #endregion Shipping Address Detail Start end
                }
                else if (AddressStatus == "2") // Copy Billing to Shipping Address
                {
                    //string AddressType = Convert.ToString(CmbAddressType.SelectedItem.Value);
                    string AddressType = "Billing";
                    string address1 = txtAddress1.Text;
                    string address2 = txtAddress2.Text;
                    string address3 = txtAddress3.Text;
                    string landmark = txtlandmark.Text;
                    int country = Convert.ToInt32(CmbCountry.SelectedItem.Value);
                    int State = Convert.ToInt32(CmbState.Value);
                    int city = Convert.ToInt32(CmbCity.Value);
                    int area = Convert.ToInt32(CmbArea.Value);
                    string pin = Convert.ToString(CmbPin.Value);
                    DataTable dt = StoreSalesOrderAddressDetail();
                    dt.Rows.Add(0, companyId, branchid, fin_year, "", AddressType, address1, address2, address3, landmark, country, State, city, area, pin, System.DateTime.Now, userid, System.DateTime.Now, userid);
                    dt.Rows.Add(0, companyId, branchid, fin_year, "", "Shipping", address1, address2, address3, landmark, country, State, city, area, pin, System.DateTime.Now, userid, System.DateTime.Now, userid);
                    Session["OldUnitStockInvoiceAddressDtl"] = dt;
                }
                else if (AddressStatus == "3") // Copy  Shipping to Billing  Address
                {
                    string scontactperson = "";
                    //string sAddressType = Convert.ToString(CmbAddressType1.SelectedItem.Value);
                    string sAddressType = "Shipping";
                    string saddress1 = txtsAddress1.Text;
                    string saddress2 = txtsAddress2.Text;
                    string saddress3 = txtsAddress3.Text;
                    string slandmark = txtslandmark.Text;
                    int scountry = Convert.ToInt32(CmbCountry1.SelectedItem.Value);
                    int sState = Convert.ToInt32(CmbState1.Value);
                    int scity = Convert.ToInt32(CmbCity1.Value);
                    int sarea = Convert.ToInt32(CmbArea1.Value);
                    string spin = Convert.ToString(CmbPin1.Value);
                    DataTable dt = StoreSalesOrderAddressDetail();
                    dt.Rows.Add(0, companyId, branchid, fin_year, "", "Billing", saddress1, saddress2, saddress3, slandmark, scountry, sState, scity, sarea, spin, System.DateTime.Now, userid, System.DateTime.Now, userid);
                    dt.Rows.Add(0, companyId, branchid, fin_year, "", sAddressType, saddress1, saddress2, saddress3, slandmark, scountry, sState, scity, sarea, spin, System.DateTime.Now, userid, System.DateTime.Now, userid);
                    Session["OldUnitStockInvoiceAddressDtl"] = dt;
                }

            }

            #endregion Save Section of Address Start

        }
        public DataTable GetOrderEditData()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GetChallanInvoiceDetails");
            proc.AddVarcharPara("@Action", 500, "ReceivedFromServicecentreDetails");
            proc.AddVarcharPara("@Order_Id", 500, Convert.ToString(Session["Invoice_ID"]));
            //proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            //proc.AddVarcharPara("@branchId", 500, Convert.ToString(Session["userbranchID"]));
            //proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetOrderEditDataForBinding()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GetChallanInvoiceDetails");
            proc.AddVarcharPara("@Action", 500, "ReceivedFromServiceCenterHeader");
            proc.AddVarcharPara("@Order_Id", 500, Convert.ToString(Session["Invoice_ID"]));
            //proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            //proc.AddVarcharPara("@branchId", 500, Convert.ToString(Session["userbranchID"]));
            //proc.AddVarcharPara("@companyId", 500, Convert.ToString(Session["LastCompany"]));
            dt = proc.GetTable();
            return dt;
        }



        public void SetOrderDetails()
        {
            DataTable OrderEditdt = GetOrderEditData();
            if (OrderEditdt != null && OrderEditdt.Rows.Count > 0)
            {
                string Branch_From = Convert.ToString(OrderEditdt.Rows[0]["Service_TransferFromBranch"]);
                string Branch_To = Convert.ToString(OrderEditdt.Rows[0]["Service_TransferToBranch"]);
                string StkOut_Number = Convert.ToString(OrderEditdt.Rows[0]["Service_IssueNumber"]);
                string StkOut_Date = Convert.ToString(OrderEditdt.Rows[0]["Issue_Date"]);

                //New Added
                //string Order_NumberingScheme = Convert.ToString(OrderEditdt.Rows[0]["Challan_NumScheme"]);
                //End
                ddl_numbering.Visible = false;

                string Requisition_Date = Convert.ToString(OrderEditdt.Rows[0]["Issue_Date"]);
                string Purpose = Convert.ToString(OrderEditdt.Rows[0]["Service_Purpose"]);

                string Service_Center = Convert.ToString(OrderEditdt.Rows[0]["Service_CenterId"]);

                string Call_No = Convert.ToString(OrderEditdt.Rows[0]["Call_No"]);


                string Quoids = Convert.ToString(OrderEditdt.Rows[0]["Service_OutIds"]);
                DataTable BindLookup = GetOrderEditDataForBinding();
                string Order_Date = Convert.ToString(BindLookup.Rows[0]["Issue_Date"]);
                if (!String.IsNullOrEmpty(Quoids))
                {
                    string[] eachQuo = Quoids.Split(',');
                    if (eachQuo.Length > 1)//More tha one quotation
                    {
                        dt_Quotation.Text = "Multiple Select Stock Out Dates";

                        BindLookUp(Order_Date);
                        foreach (string val in eachQuo)
                        {
                            lookup_order.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                        }
                        // lbl_MultipleDate.Attributes.Add("style", "display:block");
                    }
                    else if (eachQuo.Length == 1)//Single Quotation
                    { //lbl_MultipleDate.Attributes.Add("style", "display:none"); }
                        BindLookUp(Order_Date);
                        foreach (string val in eachQuo)
                        {
                            lookup_order.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                        }
                    }
                    else//No Quotation selected
                    {
                        BindLookUp(Order_Date);
                    }
                }





                txt_SlBTOutNo.Text = StkOut_Number;
                if (!string.IsNullOrEmpty(StkOut_Date))
                {
                    dt_BTOut.Date = Convert.ToDateTime(Order_Date);
                }
                if (!string.IsNullOrEmpty(Requisition_Date))
                {
                    dt_Quotation.Text = Requisition_Date;
                }

                txt_Refference.Text = Purpose;
                ddl_transferFrom_Branch.SelectedValue = Branch_From;
                //ddl_transferTo_Branch.SelectedValue = Branch_To;
                //ddl_ServiceCenter.SelectedValue = Service_Center;
                //txtCallNo.Text = Call_No;

                //ddl_SalesAgent.SelectedValue = Order_SalesmanId;
                //Added 15-02-2017
                //ddl_numberingScheme.SelectedValue = Order_NumberingScheme;

                //End

                txt_SlBTOutNo.Value = StkOut_Number;


                //cmbContactPerson.Value = Contact_Person_Id;





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
        public static List<string> CheckBalQuantity(string Id, string ProductID)
        {
            DataTable dt = new DataTable();
            List<string> obj = new List<string>();
            BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
            try
            {
                SlaesActivitiesBL objSalActBL = new SlaesActivitiesBL();
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
        protected void CmbWarehouseIDID_Callback(object sender, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];

            if (WhichCall == "BindWarehouse")
            {
                DataTable dt = GetWarehouseData();
                GetAvailableStock();

                CmbWarehouseID.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CmbWarehouseID.Items.Add(Convert.ToString(dt.Rows[i]["WarehouseName"]), Convert.ToString(dt.Rows[i]["WarehouseID"]));
                }
                if (hdndefaultID != null || hdndefaultID.Value != "")
                    CmbWarehouseID.Value = hdndefaultID.Value;
            }

        }
        public void GetAvailableStock()
        {
            //DataTable dt2 = oDBEngine.GetDataTable("select ISnull(ISNULL(tblwarehous.StockBranchWarehouse_StockIn,0)- isnull(tblwarehous.StockBranchWarehouse_StockOut,0),0) as branchopenstock from Trans_StockBranchWarehouse tblwarehous where tblwarehous.StockBranchWarehouse_StockId=" + hdfstockidPC.Value + " and tblwarehous.StockBranchWarehouse_CompanyId='" + Convert.ToString(Session["LastCompany"]) + "' and tblwarehous.StockBranchWarehouse_FinYear='" + Convert.ToString(Session["LastFinYear"]) + "' and tblwarehous.StockBranchWarehouse_BranchId=" + hdbranchIDPC.Value + "");
            try
            {
                DataTable dt2 = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableStockOpening(" + hdbranchIDPC.Value + ",'" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]) + "'," + hdfstockidPC.Value + ") as branchopenstock");

                if (dt2.Rows.Count > 0)
                {
                    CmbWarehouseID.JSProperties["cpstock"] = Convert.ToString(dt2.Rows[0]["branchopenstock"]);
                }
                else
                {

                    CmbWarehouseID.JSProperties["cpstock"] = "0.0000";
                }
            }
            catch (Exception ex)
            {

            }
        }
        #region Subhabrata/StcokAvailable
        protected void acpAvailableStock_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string performpara = e.Parameter;
            string strProductID = Convert.ToString(performpara.Split('~')[0]);
            //string strBranch = Convert.ToString(ddl_transferTo_Branch.SelectedValue);
            acpAvailableStock.JSProperties["cpstock"] = "0.00";

            try
            {
                //DataTable dt2 = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableQuotation(" + strBranch + ",'" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]) + "'," + strProductID + ") as branchopenstock");

                //if (dt2.Rows.Count > 0)
                //{
                //    acpAvailableStock.JSProperties["cpstock"] = Convert.ToString(Math.Round(Convert.ToDecimal(dt2.Rows[0]["branchopenstock"]), 2));
                //}
                //else
                //{
                //    acpAvailableStock.JSProperties["cpstock"] = "0.00";
                //}
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
                string Stk_Id = Convert.ToString(e.Parameter.Split('~')[1]);
                if (Stk_Id != "0")
                {
                    DataTable dt_QuotationDetails = objSlaesActivitiesBL.GetIssueToServiceCentreDate(Stk_Id);
                    if (dt_QuotationDetails != null && dt_QuotationDetails.Rows.Count > 0)
                    {
                        string quotationdate = Convert.ToString(dt_QuotationDetails.Rows[0]["IssueToService_Date"]);
                        if (!string.IsNullOrEmpty(quotationdate))
                        {

                            dt_Quotation.Text = Convert.ToString(quotationdate);


                        }
                    }
                }
                else
                {
                    lbl_MultipleDate.Visible = true;
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
        protected void BindLookUp(string OrderDate)
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


            SalesOrderTable = objBL.GetIssueToServiceDetails(OrderDate, status);
            lookup_order.GridView.Selection.CancelSelection();
            lookup_order.DataSource = SalesOrderTable;
            Session["OrderData"] = SalesOrderTable;
            lookup_order.DataBind();




        }
        protected void ComponentSalesOrder_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string status = string.Empty;
            string customer = string.Empty;
            string OrderDate = string.Empty;
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




                DataTable SalesOrderTable;
                if (e.Parameter.Split('~')[3] == "DateCheck")
                {
                    lookup_order.GridView.Selection.UnselectAll();
                }
                OrderDate = Convert.ToString(e.Parameter.Split('~')[1]);
                SalesOrderTable = objBL.GetIssueToServiceDetails(OrderDate, status);
                lookup_order.GridView.Selection.CancelSelection();
                lookup_order.DataSource = SalesOrderTable;
                Session["OrderData"] = SalesOrderTable;
                Session["OrderData"] = SalesOrderTable;
                lookup_order.DataBind();




            }
            else if (e.Parameter.Split('~')[0] == "BindOrderLookupOnSelection")//Subhabrata for binding quotation
            {
                if (grid_Products.GetSelectedFieldValues("Indent_No").Count != 0)
                {
                    string OrderIds = string.Empty;
                    for (int i = 0; i < grid_Products.GetSelectedFieldValues("Indent_No").Count; i++)
                    {
                        OrderIds += "," + grid_Products.GetSelectedFieldValues("Indent_No")[i];
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
                else if (grid_Products.GetSelectedFieldValues("Indent_No").Count == 0)
                {
                    lookup_order.GridView.Selection.UnselectAll();
                }
            }
            else if (e.Parameter.Split('~')[0] == "DateCheckOnChanged")//Subhabrata for binding quotation
            {

                if (grid_Products.GetSelectedFieldValues("Indent_No").Count != 0)
                {

                    DateTime SalesOrderDate = Convert.ToDateTime(e.Parameter.Split('~')[2]);
                    if (lookup_order.GridView.GetSelectedFieldValues("Order_Date").Count() != 0)
                    {
                        DateTime QuotationDate = Convert.ToDateTime(lookup_order.GridView.GetSelectedFieldValues("Order_Date")[0]);
                        if (SalesOrderDate < QuotationDate)
                        {
                            lookup_order.GridView.Selection.UnselectAll();
                        }
                    }
                }
            }


        }
        public DataTable GetBillingAddress()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesChallan_Details");
            proc.AddVarcharPara("@Action", 100, "ChallanBillingAddress");
            proc.AddVarcharPara("@Challan_Id", 20, Convert.ToString(Session["Invoice_ID"]));
            dt = proc.GetTable();
            return dt;
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
                        dt_OADate.MinDate = Convert.ToDateTime(Convert.ToString(Session["FinYearStartDate"]));
                    }
                    if (Session["FinYearEndDate"] != null)
                    {
                        dt_OADate.MaxDate = Convert.ToDateTime(Convert.ToString(Session["FinYearEndDate"]));
                    }
                    if (oDBEngine.GetDate().Date >= Convert.ToDateTime(Convert.ToString(Session["FinYearStartDate"])) && oDBEngine.GetDate().Date <= Convert.ToDateTime(Convert.ToString(Session["FinYearEndDate"])))
                    {

                    }
                    else if (oDBEngine.GetDate().Date >= Convert.ToDateTime(Convert.ToString(Session["FinYearStartDate"])) && oDBEngine.GetDate().Date >= Convert.ToDateTime(Convert.ToString(Session["FinYearEndDate"])))
                    {
                        setdate = Convert.ToString(Convert.ToDateTime(Session["FinYearEndDate"]).Month) + "/" + Convert.ToString(Convert.ToDateTime(Session["FinYearEndDate"]).Day) + "/" + Convert.ToString(Convert.ToDateTime(Session["FinYearEndDate"]).Year);
                        dt_OADate.Value = DateTime.ParseExact(setdate, @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        //dt_PLQuote.Value = DateTime.ParseExact(Convert.ToString(Session["FinYearStartDate"]), @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else if (oDBEngine.GetDate().Date <= Convert.ToDateTime(Convert.ToString(Session["FinYearStartDate"])) && oDBEngine.GetDate().Date <= Convert.ToDateTime(Convert.ToString(Session["FinYearEndDate"])))
                    {
                        setdate = Convert.ToString(Convert.ToDateTime(Session["FinYearStartDate"]).Month) + "/" + Convert.ToString(Convert.ToDateTime(Session["FinYearStartDate"]).Day) + "/" + Convert.ToString(Convert.ToDateTime(Session["FinYearStartDate"]).Year);
                        dt_OADate.Value = DateTime.ParseExact(setdate, @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                        //dt_PLQuote.Value = DateTime.ParseExact(Convert.ToString(Session["FinYearEndDate"]), @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    }
                }
            } 
        }


        #region Warehouse Details

        public string GetWarehouseMaxValue(DataTable dt)
        {
            string maxValue = "1";
            if (dt != null && dt.Rows.Count > 0)
            {
                List<int> myList = new List<int>();
                foreach (DataRow rrow in dt.Rows)
                {
                    string value = (Convert.ToString(rrow["SrlNo"]) != "") ? Convert.ToString(rrow["SrlNo"]) : "0";
                    myList.Add(Convert.ToInt32(value));
                }

                maxValue = Convert.ToString(Convert.ToInt32(myList.Max()) + 1);
            }

            return maxValue;
        }
        public int CheckSerialNoExists(string SerialNo)
        {
            //DataTable SerialCount = oDBEngine.GetDataTable("Select * From Trans_StockSerialMapping Where Stock_BranchSerialNumber='" + SerialNo.Trim() + "'");
            DataTable SerialCount = oDBEngine.GetDataTable("Select * From v_Stock_WarehouseDetails Where Stock_IN_Out_Type='IN' AND SerialNo='" + SerialNo.Trim() + "'");
            return SerialCount.Rows.Count;
        }
        public void GetQuantityBaseOnProduct(string strProductSrlNo, ref decimal WarehouseQty)
        {
            decimal sum = 0;

            DataTable Warehousedt = new DataTable();
            if (Session["OUR_WarehouseData"] != null)
            {
                Warehousedt = (DataTable)Session["OUR_WarehouseData"];
                for (int i = 0; i < Warehousedt.Rows.Count; i++)
                {
                    DataRow dr = Warehousedt.Rows[i];
                    string Product_SrlNo = Convert.ToString(dr["Product_SrlNo"]);

                    if (strProductSrlNo == Product_SrlNo)
                    {
                        string strQuantity = (Convert.ToString(dr["SalesQuantity"]) != "") ? Convert.ToString(dr["SalesQuantity"]) : "0";
                        var weight = Decimal.Parse(Regex.Match(strQuantity, "[0-9]*\\.*[0-9]*").Value);

                        sum = sum + Convert.ToDecimal(weight);
                    }
                }
            }

            WarehouseQty = sum;
        }
        public DataTable GetChallanWarehouseData()
        {
            try
            {
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("GetOldUnitDetails");
                proc.AddVarcharPara("@action", 500, "WarehouseDetails");
                proc.AddIntegerPara("@doc_id", Convert.ToInt32(Request.QueryString["key"]));
                dt = proc.GetTable();

                DataTable grid_dt = (DataTable)Session["OldUnitStockInvoice"];
                if (grid_dt != null)
                {
                    for (int i = 0; i < grid_dt.Rows.Count; i++)
                    {
                        string oldUnit_id = Convert.ToString(grid_dt.Rows[i]["oldUnit_id"]);
                        string SrlNo = Convert.ToString(grid_dt.Rows[i]["SrlNo"]);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            var rows = dt.Select("QuoteWarehouse_Id ='" + oldUnit_id + "'");
                            foreach (var row in rows)
                            {
                                row["Product_SrlNo"] = Convert.ToString(SrlNo);
                            }
                            dt.AcceptChanges();
                        }
                    }
                }

                string strNewVal = "", strOldVal = "", strProductType = "";
                DataTable tempdt = dt.Copy();

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow drr in tempdt.Rows)
                    {
                        strNewVal = Convert.ToString(drr["QuoteWarehouse_Id"]);
                        strProductType = Convert.ToString(drr["ProductType"]);

                        if (strNewVal == strOldVal)
                        {
                            drr["WarehouseName"] = "";
                            drr["TotalQuantity"] = "0";
                            drr["BatchNo"] = "";
                            drr["SalesQuantity"] = "";
                            drr["ViewMfgDate"] = "";
                            drr["ViewExpiryDate"] = "";
                        }

                        strOldVal = strNewVal;
                    }
                }

                Session["PC_LoopWarehouse"] = Convert.ToString(Convert.ToInt32(strNewVal) + 1);
                tempdt.Columns.Remove("QuoteWarehouse_Id");
                tempdt.Columns.Remove("ProductType");
                return tempdt;
            }
            catch
            {
                return null;
            }
        }
        public DataTable GetOrderWarehouseData()
        {
            try
            {
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_StockGetSCwarehousentry");
                proc.AddVarcharPara("@Action", 500, "OrderWarehouse");
                proc.AddVarcharPara("@PCNumber", 500, Convert.ToString(Session["Invoice_ID"]));
                proc.AddVarcharPara("@Finyear", 500, Convert.ToString(Session["LastFinYear"]));
                proc.AddVarcharPara("@ProductID", 500, Convert.ToString(hdfProductID.Value));
                //proc.AddVarcharPara("@branchID", 500, Convert.ToString(ddl_transferTo_Branch.SelectedValue));
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

                Session["OldUnitStockInvoice_LoopWarehouse"] = Convert.ToString(Convert.ToInt32(strNewVal) + 1);
                tempdt.Columns.Remove("warehouseID");
                return tempdt;
            }
            catch
            {
                return null;
            }
        }
        protected void CmbWarehouseID_Callback(object sender, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];

            if (WhichCall == "BindWarehouse")
            {
                DataTable dt = GetWarehouseData();
                GetAvailableStock();

                CmbWarehouseID.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CmbWarehouseID.Items.Add(Convert.ToString(dt.Rows[i]["WarehouseName"]), Convert.ToString(dt.Rows[i]["WarehouseID"]));
                }

                //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();



                string strBranch = Convert.ToString(ddl_transferFrom_Branch.SelectedValue);
                DataTable dtdefaultStock = objEngine.GetDataTable("tbl_master_building", " top 1 bui_id ", " bui_BranchId='" + strBranch + "' ");
                if (dtdefaultStock != null && dtdefaultStock.Rows.Count > 0)
                {
                    string defaultID = Convert.ToString(dtdefaultStock.Rows[0]["bui_id"]).Trim();

                    if (defaultID != null || defaultID != "" || defaultID != "0")
                    {
                        CmbWarehouseID.Value = defaultID;
                    }
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

                if (Session["OUR_WarehouseData"] != null)
                {
                    DataTable Warehousedt = (DataTable)Session["OUR_WarehouseData"];
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

                if (Session["OUR_WarehouseData"] != null)
                {
                    DataTable Warehousedt = (DataTable)Session["OUR_WarehouseData"];
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



        protected void GrdWarehouse_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GrdWarehouse.JSProperties["cpIsSave"] = "";
            string strSplitCommand = e.Parameters.Split('~')[0];
            string Type = "", Sales_UOM_Name = "", Sales_UOM_Code = "", Stk_UOM_Name = "", Stk_UOM_Code = "", Conversion_Multiplier = "", Trans_Stock = "0";
            bool IsDelete = false;

            DataTable Warehousedt = new DataTable();
            if (Session["OUR_WarehouseData"] != null)
            {
                Warehousedt = (DataTable)Session["OUR_WarehouseData"];
            }
            else
            {
                Warehousedt.Columns.Add("Product_SrlNo", typeof(string));
                Warehousedt.Columns.Add("SrlNo", typeof(int));
                Warehousedt.Columns.Add("WarehouseID", typeof(string));
                Warehousedt.Columns.Add("WarehouseName", typeof(string));
                Warehousedt.Columns.Add("Quantity", typeof(string));
                Warehousedt.Columns.Add("TotalQuantity", typeof(string));
                Warehousedt.Columns.Add("SalesQuantity", typeof(string));
                Warehousedt.Columns.Add("SalesUOMName", typeof(string));
                Warehousedt.Columns.Add("BatchID", typeof(string));
                Warehousedt.Columns.Add("BatchNo", typeof(string));
                Warehousedt.Columns.Add("MfgDate", typeof(string));
                Warehousedt.Columns.Add("ExpiryDate", typeof(string));
                Warehousedt.Columns.Add("SerialNo", typeof(string));
                Warehousedt.Columns.Add("LoopID", typeof(string));
                Warehousedt.Columns.Add("Status", typeof(string));
                Warehousedt.Columns.Add("ViewMfgDate", typeof(string));
                Warehousedt.Columns.Add("ViewExpiryDate", typeof(string));
                Warehousedt.Columns.Add("IsOutStatus", typeof(string));
                Warehousedt.Columns.Add("IsOutStatusMsg", typeof(string));
            }

            Warehousedt.Columns["IsOutStatus"].DefaultValue = "visibility: visible";
            Warehousedt.Columns["IsOutStatusMsg"].DefaultValue = "visibility: hidden";

            if (strSplitCommand == "Display")
            {
                GetProductType(ref Type);
                string ProductID = Convert.ToString(hdfProductID.Value);
                string SerialID = Convert.ToString(e.Parameters.Split('~')[1]);

                if (Warehousedt != null && Warehousedt.Rows.Count > 0)
                {
                    Warehousedt.DefaultView.Sort = "LoopID,SrlNo ASC";
                    DataTable sortWarehousedt = Warehousedt.DefaultView.ToTable();

                    DataView dvData = new DataView(sortWarehousedt);
                    dvData.RowFilter = "Product_SrlNo = '" + SerialID + "'";

                    GrdWarehouse.DataSource = dvData;
                    GrdWarehouse.DataBind();
                }
                else
                {
                    GrdWarehouse.DataSource = Warehousedt.DefaultView;
                    GrdWarehouse.DataBind();
                }
                changeGridOrder();
            }
            else if (strSplitCommand == "SaveDisplay")
            {
                GetProductType(ref Type);
                int loopId = Convert.ToInt32(Session["PC_LoopWarehouse"]);

                string WarehouseID = Convert.ToString(e.Parameters.Split('~')[1]);
                string WarehouseName = Convert.ToString(e.Parameters.Split('~')[2]);
                string BatchName = Convert.ToString(e.Parameters.Split('~')[3]);
                string MfgDate = Convert.ToString(e.Parameters.Split('~')[4]);
                string ExpiryDate = Convert.ToString(e.Parameters.Split('~')[5]);
                string SerialNo = Convert.ToString(e.Parameters.Split('~')[6]);
                string Qty = Convert.ToString(e.Parameters.Split('~')[7]);
                string editWarehouseID = Convert.ToString(e.Parameters.Split('~')[8]);

                string ProductID = Convert.ToString(hdfProductID.Value);
                string ProductSerialID = Convert.ToString(hdfProductSerialID.Value);

                if (Type == "WBS")
                {
                    GetProductUOM(ref Sales_UOM_Name, ref Sales_UOM_Code, ref Stk_UOM_Name, ref Stk_UOM_Code, ref Conversion_Multiplier, ProductID);
                    int SerialNoCount_Server = CheckSerialNoExists(SerialNo);
                    var SerialNoCount_Local = Warehousedt.Select("SerialNo ='" + SerialNo + "' AND SrlNo<>'" + editWarehouseID + "'");

                    if (editWarehouseID == "0")
                    {
                        if (SerialNoCount_Server == 0 && SerialNoCount_Local.Length == 0)
                        {
                            string maxID = GetWarehouseMaxValue(Warehousedt);
                            var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND BatchID ='" + BatchName + "' AND Product_SrlNo='" + ProductSerialID + "'");

                            if (updaterows.Length == 0)
                            {
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, "1", "1", Convert.ToDecimal("1") + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, loopId, "D", MfgDate, ExpiryDate);
                            }
                            else
                            {
                                int newloopID = 0;
                                decimal oldQuantity = 0;

                                foreach (var row in updaterows)
                                {
                                    oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                    row["Quantity"] = (oldQuantity + Convert.ToDecimal(1));
                                    row["MfgDate"] = MfgDate;
                                    row["ExpiryDate"] = ExpiryDate;
                                    newloopID = Convert.ToInt32(row["LoopID"]);


                                    if (Convert.ToDecimal(row["TotalQuantity"]) != 0)
                                    {
                                        row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(1)) + " " + Sales_UOM_Name;
                                        row["ViewMfgDate"] = MfgDate;
                                        row["ViewExpiryDate"] = ExpiryDate;
                                    }
                                }
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", (oldQuantity + Convert.ToDecimal(1)), "0", "", Sales_UOM_Name, BatchName, "", MfgDate, ExpiryDate, SerialNo, newloopID, "D", "", "");
                            }
                        }
                        else
                        {
                            GrdWarehouse.JSProperties["cperrorMsg"] = "duplicateSerial";
                        }
                    }
                    else
                    {
                        if (SerialNoCount_Server == 0 && SerialNoCount_Local.Length == 0)
                        {
                            var rows = Warehousedt.Select("SerialNo ='" + SerialNo + "' AND WarehouseID ='" + WarehouseID + "' AND BatchID ='" + BatchName + "' AND SrlNo='" + editWarehouseID + "'");
                            if (rows.Length == 0)
                            {
                                int oldloopID = 0;
                                decimal oldTotalQuantity = 0;
                                string Pre_batcgid = "", Pre_batchname = "", Pre_warehouse = "", Pre_warehouseName = "", Pre_MfgDate = "", Pre_ExpiryDate = "";

                                var oldrows = Warehousedt.Select("SrlNo='" + editWarehouseID + "'");
                                foreach (var roww in oldrows)
                                {
                                    Pre_warehouse = Convert.ToString(roww["WarehouseID"]);
                                    Pre_warehouseName = Convert.ToString(roww["WarehouseName"]);
                                    Pre_batcgid = Convert.ToString(roww["BatchID"]);
                                    Pre_batchname = Convert.ToString(roww["BatchID"]);
                                    Pre_MfgDate = Convert.ToString(roww["MfgDate"]);
                                    Pre_ExpiryDate = Convert.ToString(roww["ExpiryDate"]);

                                    oldloopID = Convert.ToInt32(roww["LoopID"]);
                                    oldTotalQuantity = Convert.ToDecimal(roww["TotalQuantity"]);
                                }

                                if ((Pre_batcgid.Trim() == BatchName.Trim()) && (Pre_warehouse.Trim() == WarehouseID.Trim()))
                                {
                                    foreach (var rowww in oldrows)
                                    {
                                        rowww["SerialNo"] = SerialNo;
                                    }

                                    var Oldupdaterows = Warehousedt.Select("LoopID='" + oldloopID + "'");
                                    foreach (DataRow updaterow in Oldupdaterows)
                                    {
                                        updaterow["MfgDate"] = MfgDate;
                                        updaterow["ExpiryDate"] = ExpiryDate;

                                        if (Convert.ToString(updaterow["ViewMfgDate"]) != "") updaterow["ViewMfgDate"] = MfgDate;
                                        if (Convert.ToString(updaterow["ViewExpiryDate"]) != "") updaterow["ViewExpiryDate"] = ExpiryDate;
                                    }
                                }
                                else
                                {
                                    foreach (DataRow delrow in oldrows)
                                    {
                                        delrow.Delete();
                                    }
                                    Warehousedt.AcceptChanges();

                                    var Oldupdaterows = Warehousedt.Select("LoopID='" + oldloopID + "'");

                                    if (oldTotalQuantity != 0)
                                    {
                                        foreach (DataRow updaterow in Oldupdaterows)
                                        {
                                            decimal PreQuantity = Convert.ToDecimal(updaterow["Quantity"]);
                                            updaterow["WarehouseName"] = Pre_warehouseName;
                                            updaterow["BatchNo"] = Pre_batchname;
                                            updaterow["SalesQuantity"] = (PreQuantity - Convert.ToDecimal(1)) + " " + Sales_UOM_Name;

                                            updaterow["ViewMfgDate"] = Pre_MfgDate;
                                            updaterow["ViewExpiryDate"] = Pre_ExpiryDate;

                                            break;
                                        }
                                    }

                                    foreach (DataRow updaterow in Oldupdaterows)
                                    {
                                        decimal PreQuantity = Convert.ToDecimal(updaterow["Quantity"]);

                                        if (Convert.ToString(updaterow["SalesQuantity"]) != "")
                                        {
                                            updaterow["Quantity"] = (PreQuantity - Convert.ToDecimal(1));
                                            updaterow["SalesQuantity"] = (PreQuantity - Convert.ToDecimal(1)) + " " + Sales_UOM_Name;
                                            updaterow["TotalQuantity"] = (PreQuantity - Convert.ToDecimal(1));
                                        }
                                        else
                                        {
                                            updaterow["Quantity"] = (PreQuantity - Convert.ToDecimal(1));
                                            updaterow["TotalQuantity"] = "0";
                                            updaterow["WarehouseName"] = "";
                                            updaterow["BatchNo"] = "";
                                        }
                                    }

                                    string maxID = GetWarehouseMaxValue(Warehousedt);
                                    var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND BatchID ='" + BatchName + "' AND Product_SrlNo='" + ProductSerialID + "'");

                                    if (updaterows.Length == 0)
                                    {
                                        Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, "1", "1", Convert.ToDecimal("1") + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, loopId, "D", MfgDate, ExpiryDate);
                                    }
                                    else
                                    {
                                        int newloopID = 0;
                                        decimal oldQuantity = 0;

                                        foreach (var row in updaterows)
                                        {
                                            oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                            row["Quantity"] = (oldQuantity + Convert.ToDecimal(1));
                                            row["MfgDate"] = MfgDate;
                                            row["ExpiryDate"] = ExpiryDate;
                                            newloopID = Convert.ToInt32(row["LoopID"]);

                                            if (Convert.ToDecimal(row["TotalQuantity"]) != 0)
                                            {
                                                row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(1)) + " " + Sales_UOM_Name;
                                                row["ViewMfgDate"] = MfgDate;
                                                row["ViewExpiryDate"] = ExpiryDate;
                                            }
                                        }
                                        Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", (oldQuantity + Convert.ToDecimal(1)), "0", "", Sales_UOM_Name, BatchName, "", MfgDate, ExpiryDate, SerialNo, newloopID, "D", "", "");
                                    }
                                }
                            }
                        }
                        else
                        {
                            GrdWarehouse.JSProperties["cperrorMsg"] = "duplicateSerial";
                        }
                    }
                }
                else if (Type == "W")
                {
                    GetProductUOM(ref Sales_UOM_Name, ref Sales_UOM_Code, ref Stk_UOM_Name, ref Stk_UOM_Code, ref Conversion_Multiplier, ProductID);

                    if (editWarehouseID == "0")
                    {
                        string maxID = GetWarehouseMaxValue(Warehousedt);
                        var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND Product_SrlNo='" + ProductSerialID + "'");

                        if (updaterows.Length == 0)
                        {
                            Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, Qty, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, loopId, "D", MfgDate, ExpiryDate);
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
                        var rows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND Convert(Quantity, 'System.Decimal')='" + Qty + "' AND SrlNo='" + editWarehouseID + "'");
                        if (rows.Length == 0)
                        {
                            string whID = "";
                            string maxID = GetWarehouseMaxValue(Warehousedt);

                            var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND Product_SrlNo='" + ProductSerialID + "'");
                            foreach (var row in updaterows)
                            {
                                whID = Convert.ToString(row["SrlNo"]);
                            }

                            if (updaterows.Length == 0)
                            {
                                IsDelete = true;
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, Qty, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, loopId, "D", MfgDate, ExpiryDate);
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
                else if (Type == "WB")
                {
                    GetProductUOM(ref Sales_UOM_Name, ref Sales_UOM_Code, ref Stk_UOM_Name, ref Stk_UOM_Code, ref Conversion_Multiplier, ProductID);

                    if (editWarehouseID == "0")
                    {
                        string maxID = GetWarehouseMaxValue(Warehousedt);
                        var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND BatchID='" + BatchName + "' AND Product_SrlNo='" + ProductSerialID + "'");

                        if (updaterows.Length == 0)
                        {
                            Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, Qty, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, loopId, "D", MfgDate, ExpiryDate);
                        }
                        else
                        {
                            foreach (var row in updaterows)
                            {
                                decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                row["Quantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                row["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(Qty)) + " " + Sales_UOM_Name;
                                row["MfgDate"] = MfgDate;
                                row["ExpiryDate"] = ExpiryDate;
                            }
                        }
                    }
                    else
                    {
                        var rows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND BatchID='" + BatchName + "' AND Convert(TotalQuantity, 'System.Decimal')='" + Qty + "' AND SrlNo='" + editWarehouseID + "'");
                        if (rows.Length == 0)
                        {
                            string whID = "";
                            string maxID = GetWarehouseMaxValue(Warehousedt);

                            var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND BatchID='" + BatchName + "' AND Product_SrlNo='" + ProductSerialID + "'");
                            foreach (var row in updaterows)
                            {
                                whID = Convert.ToString(row["SrlNo"]);
                            }

                            if (updaterows.Length == 0)
                            {
                                IsDelete = true;
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, Qty, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, loopId, "D", MfgDate, ExpiryDate);
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
                                    row["MfgDate"] = MfgDate;
                                    row["ExpiryDate"] = ExpiryDate;
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
                                    row["MfgDate"] = MfgDate;
                                    row["ExpiryDate"] = ExpiryDate;
                                }
                            }
                        }
                    }
                }
                else if (Type == "B")
                {
                    GetProductUOM(ref Sales_UOM_Name, ref Sales_UOM_Code, ref Stk_UOM_Name, ref Stk_UOM_Code, ref Conversion_Multiplier, ProductID);
                    WarehouseID = "0";

                    if (editWarehouseID == "0")
                    {
                        string maxID = GetWarehouseMaxValue(Warehousedt);
                        var updaterows = Warehousedt.Select("BatchID ='" + BatchName + "' AND Product_SrlNo='" + ProductSerialID + "'");

                        if (updaterows.Length == 0)
                        {
                            Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, Qty, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, loopId, "D", MfgDate, ExpiryDate);
                        }
                        else
                        {
                            foreach (var row in updaterows)
                            {
                                decimal oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                row["Quantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                row["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(Qty));
                                row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(Qty)) + " " + Sales_UOM_Name;
                                row["MfgDate"] = MfgDate;
                                row["ExpiryDate"] = ExpiryDate;
                            }
                        }
                    }
                    else
                    {
                        var rows = Warehousedt.Select("BatchID='" + BatchName + "' AND Convert(TotalQuantity, 'System.Decimal')='" + Qty + "' AND SrlNo='" + editWarehouseID + "'");
                        if (rows.Length == 0)
                        {
                            string whID = "";
                            string maxID = GetWarehouseMaxValue(Warehousedt);

                            var updaterows = Warehousedt.Select("BatchID ='" + BatchName + "' AND Product_SrlNo='" + ProductSerialID + "'");
                            foreach (var row in updaterows)
                            {
                                whID = Convert.ToString(row["SrlNo"]);
                            }

                            if (updaterows.Length == 0)
                            {
                                IsDelete = true;
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, Qty, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, loopId, "D", MfgDate, ExpiryDate);
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
                                    row["MfgDate"] = MfgDate;
                                    row["ExpiryDate"] = ExpiryDate;
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
                                    row["MfgDate"] = MfgDate;
                                    row["ExpiryDate"] = ExpiryDate;
                                }
                            }
                        }
                    }
                }
                else if (Type == "S")
                {
                    GetProductUOM(ref Sales_UOM_Name, ref Sales_UOM_Code, ref Stk_UOM_Name, ref Stk_UOM_Code, ref Conversion_Multiplier, ProductID);
                    int SerialNoCount_Server = CheckSerialNoExists(SerialNo);
                    var SerialNoCount_Local = Warehousedt.Select("SerialNo ='" + SerialNo + "' AND SrlNo<>'" + editWarehouseID + "'");

                    if (editWarehouseID == "0")
                    {
                        if (SerialNoCount_Server == 0 && SerialNoCount_Local.Length == 0)
                        {
                            string maxID = GetWarehouseMaxValue(Warehousedt);

                            int newloopID = 0;
                            decimal oldQuantity = 0;

                            var updaterows = Warehousedt.Select("Product_SrlNo='" + ProductSerialID + "'");
                            if (updaterows.Length > 0)
                            {
                                foreach (var row in updaterows)
                                {
                                    oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                    newloopID = Convert.ToInt32(row["LoopID"]);

                                    row["Quantity"] = (oldQuantity + Convert.ToDecimal(1));
                                    if (Convert.ToDecimal(row["TotalQuantity"]) != 0)
                                    {
                                        row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(1)) + " " + Sales_UOM_Name;
                                    }
                                }

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, (oldQuantity + Convert.ToDecimal(1)), "0", "", Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, newloopID, "D", MfgDate, ExpiryDate);
                            }
                            else
                            {
                                newloopID = loopId;
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, "1", "1", Convert.ToDecimal(1) + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, newloopID, "D", MfgDate, ExpiryDate);
                            }
                        }
                        else
                        {
                            GrdWarehouse.JSProperties["cperrorMsg"] = "duplicateSerial";
                        }
                    }
                    else
                    {
                        if (SerialNoCount_Server == 0 && SerialNoCount_Local.Length == 0)
                        {
                            var rows = Warehousedt.Select("SerialNo ='" + SerialNo + "' AND SrlNo='" + editWarehouseID + "'");
                            if (rows.Length == 0)
                            {
                                DataRow[] updaterows = Warehousedt.Select("SrlNo ='" + editWarehouseID + "'");
                                foreach (DataRow row in updaterows)
                                {
                                    row["SerialNo"] = SerialNo;
                                }
                            }
                        }
                        else
                        {
                            GrdWarehouse.JSProperties["cperrorMsg"] = "duplicateSerial";
                        }
                    }
                }
                else if (Type == "WS")
                {
                    GetProductUOM(ref Sales_UOM_Name, ref Sales_UOM_Code, ref Stk_UOM_Name, ref Stk_UOM_Code, ref Conversion_Multiplier, ProductID);
                    int SerialNoCount_Server = CheckSerialNoExists(SerialNo);
                    var SerialNoCount_Local = Warehousedt.Select("SerialNo ='" + SerialNo + "' AND SrlNo<>'" + editWarehouseID + "'");

                    if (editWarehouseID == "0")
                    {
                        if (SerialNoCount_Server == 0 && SerialNoCount_Local.Length == 0)
                        {
                            string maxID = GetWarehouseMaxValue(Warehousedt);
                            var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND Product_SrlNo='" + ProductSerialID + "'");

                            if (updaterows.Length == 0)
                            {
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, "1", "1", Convert.ToDecimal("1") + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, loopId, "D", MfgDate, ExpiryDate);
                            }
                            else
                            {
                                int newloopID = 0;
                                decimal oldQuantity = 0;

                                foreach (var row in updaterows)
                                {
                                    oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                    row["Quantity"] = (oldQuantity + Convert.ToDecimal(1));
                                    row["MfgDate"] = MfgDate;
                                    row["ExpiryDate"] = ExpiryDate;
                                    newloopID = Convert.ToInt32(row["LoopID"]);

                                    if (Convert.ToDecimal(row["TotalQuantity"]) != 0)
                                    {
                                        row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(1)) + " " + Sales_UOM_Name;
                                    }
                                }
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", (oldQuantity + Convert.ToDecimal(1)), "0", "", Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, newloopID, "D", MfgDate, ExpiryDate);
                            }
                        }
                        else
                        {
                            GrdWarehouse.JSProperties["cperrorMsg"] = "duplicateSerial";
                        }
                    }
                    else
                    {
                        if (SerialNoCount_Server == 0 && SerialNoCount_Local.Length == 0)
                        {
                            var rows = Warehousedt.Select("SerialNo ='" + SerialNo + "' AND WarehouseID ='" + WarehouseID + "' AND SrlNo='" + editWarehouseID + "'");
                            if (rows.Length == 0)
                            {
                                int oldloopID = 0;
                                decimal oldTotalQuantity = 0;
                                string Pre_warehouse = "", Pre_warehouseName = "";

                                var oldrows = Warehousedt.Select("SrlNo='" + editWarehouseID + "'");
                                foreach (var roww in oldrows)
                                {
                                    Pre_warehouse = Convert.ToString(roww["WarehouseID"]);
                                    Pre_warehouseName = Convert.ToString(roww["WarehouseName"]);
                                    oldloopID = Convert.ToInt32(roww["LoopID"]);
                                    oldTotalQuantity = Convert.ToDecimal(roww["TotalQuantity"]);
                                }

                                if (Pre_warehouse.Trim() == WarehouseID.Trim())
                                {
                                    foreach (var rowww in oldrows)
                                    {
                                        rowww["SerialNo"] = SerialNo;
                                    }
                                }
                                else
                                {
                                    foreach (DataRow delrow in oldrows)
                                    {
                                        delrow.Delete();
                                    }
                                    Warehousedt.AcceptChanges();

                                    var Oldupdaterows = Warehousedt.Select("LoopID='" + oldloopID + "'");

                                    if (oldTotalQuantity != 0)
                                    {
                                        foreach (DataRow updaterow in Oldupdaterows)
                                        {
                                            decimal PreQuantity = Convert.ToDecimal(updaterow["Quantity"]);
                                            updaterow["WarehouseName"] = Pre_warehouseName;
                                            updaterow["SalesQuantity"] = (PreQuantity - Convert.ToDecimal(1)) + " " + Sales_UOM_Name;

                                            break;
                                        }
                                    }

                                    foreach (DataRow updaterow in Oldupdaterows)
                                    {
                                        decimal PreQuantity = Convert.ToDecimal(updaterow["Quantity"]);

                                        if (Convert.ToString(updaterow["SalesQuantity"]) != "")
                                        {
                                            updaterow["Quantity"] = (PreQuantity - Convert.ToDecimal(1));
                                            updaterow["SalesQuantity"] = (PreQuantity - Convert.ToDecimal(1)) + " " + Sales_UOM_Name;
                                            updaterow["TotalQuantity"] = (PreQuantity - Convert.ToDecimal(1));
                                        }
                                        else
                                        {
                                            updaterow["Quantity"] = (PreQuantity - Convert.ToDecimal(1));
                                            updaterow["TotalQuantity"] = "0";
                                            updaterow["WarehouseName"] = "";
                                        }
                                    }

                                    string maxID = GetWarehouseMaxValue(Warehousedt);
                                    var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND Product_SrlNo='" + ProductSerialID + "'");

                                    if (updaterows.Length == 0)
                                    {
                                        Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, "1", "1", Convert.ToDecimal("1") + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, loopId, "D", MfgDate, ExpiryDate);
                                    }
                                    else
                                    {
                                        int newloopID = 0;
                                        decimal oldQuantity = 0;

                                        foreach (var row in updaterows)
                                        {
                                            oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                            row["Quantity"] = (oldQuantity + Convert.ToDecimal(1));
                                            row["MfgDate"] = MfgDate;
                                            row["ExpiryDate"] = ExpiryDate;
                                            newloopID = Convert.ToInt32(row["LoopID"]);

                                            if (Convert.ToDecimal(row["TotalQuantity"]) != 0)
                                            {
                                                row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(1)) + " " + Sales_UOM_Name;
                                            }
                                        }
                                        Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", (oldQuantity + Convert.ToDecimal(1)), "0", "", Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, newloopID, "D", MfgDate, ExpiryDate);
                                    }
                                }
                            }
                        }
                        else
                        {
                            GrdWarehouse.JSProperties["cperrorMsg"] = "duplicateSerial";
                        }
                    }
                }
                else if (Type == "BS")
                {
                    GetProductUOM(ref Sales_UOM_Name, ref Sales_UOM_Code, ref Stk_UOM_Name, ref Stk_UOM_Code, ref Conversion_Multiplier, ProductID);
                    int SerialNoCount_Server = CheckSerialNoExists(SerialNo);
                    var SerialNoCount_Local = Warehousedt.Select("SerialNo ='" + SerialNo + "' AND SrlNo<>'" + editWarehouseID + "'");

                    if (editWarehouseID == "0")
                    {
                        if (SerialNoCount_Server == 0 && SerialNoCount_Local.Length == 0)
                        {
                            string maxID = GetWarehouseMaxValue(Warehousedt);
                            var updaterows = Warehousedt.Select("BatchID ='" + BatchName + "' AND Product_SrlNo='" + ProductSerialID + "'");

                            if (updaterows.Length == 0)
                            {
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, "1", "1", Convert.ToDecimal("1") + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, loopId, "D", MfgDate, ExpiryDate);
                            }
                            else
                            {
                                int newloopID = 0;
                                decimal oldQuantity = 0;

                                foreach (var row in updaterows)
                                {
                                    oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                    row["Quantity"] = (oldQuantity + Convert.ToDecimal(1));
                                    row["MfgDate"] = MfgDate;
                                    row["ExpiryDate"] = ExpiryDate;
                                    newloopID = Convert.ToInt32(row["LoopID"]);

                                    if (Convert.ToDecimal(row["TotalQuantity"]) != 0)
                                    {
                                        row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(1)) + " " + Sales_UOM_Name;
                                    }
                                }
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", (oldQuantity + Convert.ToDecimal(1)), "0", "", Sales_UOM_Name, BatchName, "", MfgDate, ExpiryDate, SerialNo, newloopID, "D", "", "");
                            }
                        }
                        else
                        {
                            GrdWarehouse.JSProperties["cperrorMsg"] = "duplicateSerial";
                        }
                    }
                    else
                    {
                        if (SerialNoCount_Server == 0 && SerialNoCount_Local.Length == 0)
                        {
                            var rows = Warehousedt.Select("SerialNo ='" + SerialNo + "' AND BatchID ='" + BatchName + "' AND SrlNo='" + editWarehouseID + "'");
                            if (rows.Length == 0)
                            {
                                int oldloopID = 0;
                                decimal oldTotalQuantity = 0;
                                string Pre_batcgid = "", Pre_batchname = "";

                                var oldrows = Warehousedt.Select("SrlNo='" + editWarehouseID + "'");
                                foreach (var roww in oldrows)
                                {
                                    Pre_batcgid = Convert.ToString(roww["BatchID"]);
                                    Pre_batchname = Convert.ToString(roww["BatchID"]);
                                    oldloopID = Convert.ToInt32(roww["LoopID"]);
                                    oldTotalQuantity = Convert.ToDecimal(roww["TotalQuantity"]);
                                }

                                if (Pre_batcgid.Trim() == BatchName.Trim())
                                {
                                    foreach (var rowww in oldrows)
                                    {
                                        rowww["SerialNo"] = SerialNo;
                                    }
                                }
                                else
                                {
                                    foreach (DataRow delrow in oldrows)
                                    {
                                        delrow.Delete();
                                    }
                                    Warehousedt.AcceptChanges();

                                    var Oldupdaterows = Warehousedt.Select("LoopID='" + oldloopID + "'");

                                    if (oldTotalQuantity != 0)
                                    {
                                        foreach (DataRow updaterow in Oldupdaterows)
                                        {
                                            decimal PreQuantity = Convert.ToDecimal(updaterow["Quantity"]);
                                            updaterow["BatchNo"] = Pre_batchname;
                                            updaterow["SalesQuantity"] = (PreQuantity - Convert.ToDecimal(1)) + " " + Sales_UOM_Name;

                                            break;
                                        }
                                    }

                                    foreach (DataRow updaterow in Oldupdaterows)
                                    {
                                        decimal PreQuantity = Convert.ToDecimal(updaterow["Quantity"]);

                                        if (Convert.ToString(updaterow["SalesQuantity"]) != "")
                                        {
                                            updaterow["Quantity"] = (PreQuantity - Convert.ToDecimal(1));
                                            updaterow["SalesQuantity"] = (PreQuantity - Convert.ToDecimal(1)) + " " + Sales_UOM_Name;
                                            updaterow["TotalQuantity"] = (PreQuantity - Convert.ToDecimal(1));
                                        }
                                        else
                                        {
                                            updaterow["Quantity"] = (PreQuantity - Convert.ToDecimal(1));
                                            updaterow["TotalQuantity"] = "0";
                                            updaterow["BatchNo"] = "";
                                        }
                                    }

                                    string maxID = GetWarehouseMaxValue(Warehousedt);
                                    var updaterows = Warehousedt.Select("BatchID ='" + BatchName + "' AND Product_SrlNo='" + ProductSerialID + "'");

                                    if (updaterows.Length == 0)
                                    {
                                        Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, "1", "1", Convert.ToDecimal("1") + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, loopId, "D", MfgDate, ExpiryDate);
                                    }
                                    else
                                    {
                                        int newloopID = 0;
                                        decimal oldQuantity = 0;

                                        foreach (var row in updaterows)
                                        {
                                            oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                            row["Quantity"] = (oldQuantity + Convert.ToDecimal(1));
                                            row["MfgDate"] = MfgDate;
                                            row["ExpiryDate"] = ExpiryDate;
                                            newloopID = Convert.ToInt32(row["LoopID"]);

                                            if (Convert.ToDecimal(row["TotalQuantity"]) != 0)
                                            {
                                                row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(1)) + " " + Sales_UOM_Name;
                                            }
                                        }
                                        Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", (oldQuantity + Convert.ToDecimal(1)), "0", "", Sales_UOM_Name, BatchName, "", MfgDate, ExpiryDate, SerialNo, newloopID, "D", "", "");
                                    }
                                }
                            }
                        }
                        else
                        {
                            GrdWarehouse.JSProperties["cperrorMsg"] = "duplicateSerial";
                        }
                    }
                }


                if (IsDelete == true)
                {
                    DataRow[] delResult = Warehousedt.Select("SrlNo ='" + editWarehouseID + "'");
                    foreach (DataRow delrow in delResult)
                    {
                        delrow.Delete();
                    }
                    Warehousedt.AcceptChanges();
                }

                Session["OUR_WarehouseData"] = Warehousedt;
                Session["PC_LoopWarehouse"] = loopId + 1;
                changeGridOrder();

                Warehousedt.DefaultView.Sort = "LoopID,SrlNo ASC";
                GrdWarehouse.DataSource = Warehousedt.DefaultView.ToTable();
                GrdWarehouse.DataBind();
            }
            else if (strSplitCommand == "Delete")
            {
                string strKey = e.Parameters.Split('~')[1];
                string strLoopID = "", strPreLoopID = "";

                if (Session["OUR_WarehouseData"] != null)
                {
                    Warehousedt = (DataTable)Session["OUR_WarehouseData"];
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
                    string WarehouseName = "", Quantity = "", SalesUOMName = "", BatchNo = "", SalesQuantity = "", MfgDate = "", ExpiryDate = "";

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
                            SalesUOMName = Convert.ToString(dr["SalesUOMName"]);
                            BatchNo = Convert.ToString(dr["BatchNo"]);
                            SalesQuantity = Convert.ToString(dr["SalesQuantity"]);
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
                                    decimal S_Quantity = Convert.ToDecimal(dr["Quantity"]);
                                    dr["Quantity"] = S_Quantity - 1;

                                    if (IsFirst == true && IsAssign == false)
                                    {
                                        IsAssign = true;

                                        dr["WarehouseName"] = WarehouseName;
                                        dr["BatchNo"] = BatchNo;
                                        dr["SalesUOMName"] = SalesUOMName;
                                        dr["SalesQuantity"] = (S_Quantity - 1) + " " + SalesUOMName;
                                        dr["MfgDate"] = MfgDate;
                                        dr["ExpiryDate"] = ExpiryDate;
                                        dr["TotalQuantity"] = S_Quantity - 1;
                                        dr["ViewMfgDate"] = MfgDate;
                                        dr["ViewExpiryDate"] = ExpiryDate;
                                    }
                                    else
                                    {
                                        if (IsAssign == false)
                                        {
                                            IsAssign = true;
                                            SalesUOMName = Convert.ToString(dr["SalesUOMName"]);
                                            dr["SalesQuantity"] = (S_Quantity - 1) + " " + SalesUOMName;
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

                Session["OUR_WarehouseData"] = Warehousedt;
                GrdWarehouse.DataSource = Warehousedt.DefaultView;
                GrdWarehouse.DataBind();
            }
            else if (strSplitCommand == "WarehouseFinal")
            {
                if (Session["OUR_WarehouseData"] != null)
                {
                    DataTable PC_WarehouseData = (DataTable)Session["OUR_WarehouseData"];
                    string ProductID = Convert.ToString(hdfProductSerialID.Value);
                    decimal sum = 0;

                    for (int i = 0; i < PC_WarehouseData.Rows.Count; i++)
                    {
                        DataRow dr = PC_WarehouseData.Rows[i];
                        string delLoopID = Convert.ToString(dr["LoopID"]);
                        string Product_SrlNo = Convert.ToString(dr["Product_SrlNo"]);

                        if (ProductID == Product_SrlNo)
                        {
                            string strQuantity = (Convert.ToString(dr["SalesQuantity"]) != "") ? Convert.ToString(dr["SalesQuantity"]) : "0";
                            var weight = Decimal.Parse(Regex.Match(strQuantity, "[0-9]*\\.*[0-9]*").Value);

                            sum = sum + Convert.ToDecimal(weight);
                        }
                    }

                    if (Convert.ToDecimal(sum) == Convert.ToDecimal(hdnProductQuantity.Value))
                    {
                        GrdWarehouse.JSProperties["cpIsSave"] = "Y";
                        for (int i = 0; i < PC_WarehouseData.Rows.Count; i++)
                        {
                            DataRow dr = PC_WarehouseData.Rows[i];
                            string Product_SrlNo = Convert.ToString(dr["Product_SrlNo"]);
                            if (ProductID == Product_SrlNo)
                            {
                                dr["Status"] = "I";
                            }
                        }
                        PC_WarehouseData.AcceptChanges();
                    }
                    else
                    {
                        GrdWarehouse.JSProperties["cpIsSave"] = "N";
                    }

                    Session["OUR_WarehouseData"] = PC_WarehouseData;
                }
            }
            else if (strSplitCommand == "WarehouseDelete")
            {
                string ProductID = Convert.ToString(hdfProductSerialID.Value);
                DeleteUnsaveWarehouse(ProductID);
            }
        }
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string performpara = e.Parameter;
            if (performpara.Split('~')[0] == "EditWarehouse")
            {
                string SrlNo = performpara.Split('~')[1];
                string ProductType = Convert.ToString(hdfProductType.Value);

                if (Session["OUR_WarehouseData"] != null)
                {
                    DataTable Warehousedt = (DataTable)Session["OUR_WarehouseData"];

                    string strWarehouse = "", strBatchID = "", strSrlID = "", strQuantity = "0", MfgDate = "", ExpiryDate = "";
                    var rows = Warehousedt.Select(string.Format("SrlNo ='{0}'", SrlNo));
                    foreach (var dr in rows)
                    {
                        strWarehouse = (Convert.ToString(dr["WarehouseID"]) != "") ? Convert.ToString(dr["WarehouseID"]) : "0";
                        strBatchID = (Convert.ToString(dr["BatchID"]) != "") ? Convert.ToString(dr["BatchID"]) : "";
                        MfgDate = (Convert.ToString(dr["MfgDate"]) != "") ? Convert.ToString(dr["MfgDate"]) : "";
                        ExpiryDate = (Convert.ToString(dr["ExpiryDate"]) != "") ? Convert.ToString(dr["ExpiryDate"]) : "";
                        strSrlID = (Convert.ToString(dr["SerialNo"]) != "") ? Convert.ToString(dr["SerialNo"]) : "";
                        strQuantity = (Convert.ToString(dr["Quantity"]) != "") ? Convert.ToString(dr["Quantity"]) : "0";
                    }

                    CallbackPanel.JSProperties["cpEdit"] = strWarehouse + "~" + strBatchID + "~" + MfgDate + "~" + ExpiryDate + "~" + strSrlID + "~" + strQuantity;
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
            if (Session["OUR_WarehouseData"] != null)
            {
                string Type = "";
                GetProductType(ref Type);
                string SerialID = Convert.ToString(hdfProductSerialID.Value);
                DataTable Warehousedt = (DataTable)Session["OUR_WarehouseData"];

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
        public void DeleteUnsaveWarehouse(string SrlNo)
        {
            DataTable Warehousedt = new DataTable();
            if (Session["OUR_WarehouseData"] != null)
            {
                Warehousedt = (DataTable)Session["OUR_WarehouseData"];

                var rows = Warehousedt.Select("Product_SrlNo ='" + SrlNo + "' AND Status='D'");
                foreach (var row in rows)
                {
                    row.Delete();
                }
                Warehousedt.AcceptChanges();

                Session["OUR_WarehouseData"] = Warehousedt;
            }
        }
        public DataTable DeleteWarehouseBySrl(string strKey)
        {
            string strLoopID = "", strPreLoopID = "";

            DataTable Warehousedt = new DataTable();
            if (Session["OUR_WarehouseData"] != null)
            {
                Warehousedt = (DataTable)Session["OUR_WarehouseData"];
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
            if (Session["OUR_WarehouseData"] != null)
            {
                Warehousedt = (DataTable)Session["OUR_WarehouseData"];

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

                Session["OUR_WarehouseData"] = Warehousedt;
            }
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
        public void changeGridOrder()
        {
            string Type = "";
            GetProductType(ref Type);
            if (Type == "W")
            {
                GrdWarehouse.Columns["WarehouseName"].Visible = true;
                GrdWarehouse.Columns["BatchNo"].Visible = false;
                GrdWarehouse.Columns["ViewMfgDate"].Visible = false;
                GrdWarehouse.Columns["ViewExpiryDate"].Visible = false;
                GrdWarehouse.Columns["SerialNo"].Visible = false;
            }
            else if (Type == "WB")
            {
                GrdWarehouse.Columns["WarehouseName"].Visible = true;
                GrdWarehouse.Columns["BatchNo"].Visible = true;
                GrdWarehouse.Columns["ViewMfgDate"].Visible = true;
                GrdWarehouse.Columns["ViewExpiryDate"].Visible = true;
                GrdWarehouse.Columns["SerialNo"].Visible = false;
            }
            else if (Type == "WBS")
            {
                GrdWarehouse.Columns["WarehouseName"].Visible = true;
                GrdWarehouse.Columns["BatchNo"].Visible = true;
                GrdWarehouse.Columns["ViewMfgDate"].Visible = true;
                GrdWarehouse.Columns["ViewExpiryDate"].Visible = true;
                GrdWarehouse.Columns["SerialNo"].Visible = true;
            }
            else if (Type == "B")
            {
                GrdWarehouse.Columns["WarehouseName"].Visible = false;
                GrdWarehouse.Columns["BatchNo"].Visible = true;
                GrdWarehouse.Columns["ViewMfgDate"].Visible = true;
                GrdWarehouse.Columns["ViewExpiryDate"].Visible = true;
                GrdWarehouse.Columns["SerialNo"].Visible = false;
            }
            else if (Type == "S")
            {
                GrdWarehouse.Columns["WarehouseName"].Visible = false;
                GrdWarehouse.Columns["BatchNo"].Visible = false;
                GrdWarehouse.Columns["ViewMfgDate"].Visible = false;
                GrdWarehouse.Columns["ViewExpiryDate"].Visible = false;
                GrdWarehouse.Columns["SerialNo"].Visible = true;
            }
            else if (Type == "WS")
            {
                GrdWarehouse.Columns["WarehouseName"].Visible = true;
                GrdWarehouse.Columns["BatchNo"].Visible = false;
                GrdWarehouse.Columns["ViewMfgDate"].Visible = false;
                GrdWarehouse.Columns["ViewExpiryDate"].Visible = false;
                GrdWarehouse.Columns["SerialNo"].Visible = true;
            }
            else if (Type == "BS")
            {
                GrdWarehouse.Columns["WarehouseName"].Visible = false;
                GrdWarehouse.Columns["BatchNo"].Visible = true;
                GrdWarehouse.Columns["ViewMfgDate"].Visible = true;
                GrdWarehouse.Columns["ViewExpiryDate"].Visible = true;
                GrdWarehouse.Columns["SerialNo"].Visible = true;
            }
        }

        #endregion

        protected void ddl_numberingScheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string strBranchID = Convert.ToString(Session["userbranchID"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            DataTable CNSchemadt = GetNumberingSchemaDN(strCompanyID, userbranchHierarchy, FinYear, "25", "Y");
            if (CNSchemadt != null && CNSchemadt.Rows.Count > 0)
            {
                ddlDNnumbering.DataTextField = "SchemaName";
                ddlDNnumbering.DataValueField = "Id";
                ddlDNnumbering.DataSource = CNSchemadt;
                ddlDNnumbering.DataBind();
            }
        }
    }
}