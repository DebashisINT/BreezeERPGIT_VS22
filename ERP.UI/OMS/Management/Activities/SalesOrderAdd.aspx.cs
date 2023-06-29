/*********************************************************************************************************
 * Rev 1.0      Sanchita      V2.0.37               Tolerance feature required in Sales Order Module
 *                                                  Refer: 25223  -- WORK REVERTED
 * Rev 2.0      Sanchita      V2.0.39   28/06/2023  Some of the issues are there in Sales Invoice regarding 
                                                    Multi UOM in EVAC - FOR ALL SALES ORDER. Refer: 26453
 **********************************************************************************************************/
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
using ERP.OMS.Tax_Details.ClassFile;
using EntityLayer.MailingSystem;
using BusinessLogicLayer.EmailDetails;
using UtilityLayer;
using System.Web.Script.Serialization;
using ERP.Models;
using System.Web.Script.Services;

namespace ERP.OMS.Management.Activities
{

    public partial class SalesOrderAdd : ERP.OMS.ViewState_class.VSPage //System.Web.UI.Page
    {
        Export.ExportToPDF exportToPDF = new Export.ExportToPDF();
        BusinessLogicLayer.Converter objConverter = new BusinessLogicLayer.Converter();
        MasterSettings objmaster = new MasterSettings();
        public static string IsLighterCustomePage = string.Empty;
        //  BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        clsDropDownList oclsDropDownList = new clsDropDownList();
        BusinessLogicLayer.Contact oContactGeneralBL = new BusinessLogicLayer.Contact();
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        BusinessLogicLayer.RemarkCategoryBL reCat = new BusinessLogicLayer.RemarkCategoryBL();
        BusinessLogicLayer.Others objBL = new BusinessLogicLayer.Others();
        // GSTtaxDetails gstTaxDetails = new GSTtaxDetails();
        GSTtaxDetails gstTaxDetails = new GSTtaxDetails();
        BusinessLogicLayer.CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();
        SalesInvoiceBL objSalesInvoiceBL = new SalesInvoiceBL();
        static string ForJournalDate = null;
        SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
        #region Sandip Section For Approval Section Start
        ERPDocPendingApprovalBL objERPDocPendingApproval = new ERPDocPendingApprovalBL();
        #endregion Sandip Section For Approval Dtl Section End
        string UniqueQuotation = string.Empty;
        public string pageAccess = "";
        string userbranch = "";
        string QuotationIds = string.Empty;
        DataTable dtPartyTotal = null;
        string PartyTotalBalAmt = "";
        string PartyTotalBalDesc = "";
        DataTable Remarks = null;
        // Rev 1.0
        //public string IsToleranceInSalesOrder = null;
        // End of Rev 1.0

        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            #region Sandip Section For Approval Section Start
            if (Request.QueryString.AllKeys.Contains("status") || Request.QueryString.AllKeys.Contains("SalId") || Request.QueryString.AllKeys.Contains("IsTagged"))
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/PopUp.Master";
                //this.Page.MasterPageFile = "~/OMS/MasterPage/ERP.Master";
                //divcross.Visible = false;
                //divCrossActivity.Visible = false;
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
        #region Sandip Section For Approval Section Start
        public void IsExistsDocumentInERPDocApproveStatus(string orderId)
        {
            string editable = "";
            string status = "";
            DataTable dt = new DataTable();
            int Id = Convert.ToInt32(orderId);
            dt = objERPDocPendingApproval.IsExistsDocumentInERPDocApproveStatus(Id, 2); // 2 for Sale order
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
                    btn_SaveExit.Visible = false;
                }
                else
                {
                    lbl_quotestatusmsg.Visible = false;
                    btn_SaveRecords.Visible = true;
                    btn_SaveExit.Visible = true;
                }
            }
        }

        #endregion Sandip Section For Approval Dtl Section End
        protected void Page_Load(object sender, EventArgs e)
        {
            // rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/frmContactMain.aspx?requesttype=customer");
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/CustomerMasterList.aspx");
            Cross_CloseWindow.Visible = false;
            if (HttpContext.Current.Session["userid"] == null)
            {
            }
            if (!String.IsNullOrEmpty(Request.QueryString["SalId"]))
            {
            }

            CommonBL ComBL = new CommonBL();
            string ProjectSelectInEntryModule = ComBL.GetSystemSettingsResult("ProjectSelectInEntryModule");
            string ProjectMandatoryInEntry = ComBL.GetSystemSettingsResult("ProjectMandatoryInEntry");
            string ApproveSettingsSalesOrder = ComBL.GetSystemSettingsResult("ApproveSettingsSalesOrder");
            hdnDocumentSegmentSettings.Value = objmaster.GetSettings("DocumentSegment");
            //Rev work start 24.06.2022 mantise no:0024987            
            string SalesmanCaption = ComBL.GetSystemSettingsResult("ShowCoordinator");            
            if (!String.IsNullOrEmpty(SalesmanCaption))
            {
                if (SalesmanCaption.ToUpper().Trim() == "NO")
                {
                    ASPxLabel3.Text = "Salesman/Agents";
                    hs1.InnerText = "Salesman/Agents";
                    hdnCoordinate.Value = "NO";
                }
                else if (SalesmanCaption.ToUpper().Trim() == "YES")
                {
                    ASPxLabel3.Text = "Coordinator";
                    hs1.InnerText = "Coordinator";
                    hdnCoordinate.Value = "YES";
                }
            }
            //Rev work close 24.06.2022 mantise no:0024987
            //Rev work start 28.06.2022 Mantise no:24949
            string SalesRateScheme = ComBL.GetSystemSettingsResult("IsSalesRateSchemeApplyInSalesModule");
            if (!String.IsNullOrEmpty(SalesRateScheme))
            {
                if (SalesRateScheme.ToUpper().Trim() == "NO")
                {
                    hdnSettings.Value = "NO";
                }
                else if (SalesRateScheme.ToUpper().Trim() == "YES")
                {
                    hdnSettings.Value = "YES";
                }
            }
            //Rev work close 28.06.2022 Mantise no:24949
            if (hdnApproveStatus.Value == "")
            {
                hdnApproveStatus.Value = "0";
            }
            if (hdnDocumentSegmentSettings.Value=="0")
            {
                DivSegment1.Attributes.Add("style", "display:none");
                DivSegment2.Attributes.Add("style", "display:none");
                DivSegment3.Attributes.Add("style", "display:none");
                DivSegment4.Attributes.Add("style", "display:none");
                DivSegment5.Attributes.Add("style", "display:none"); 
            }
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

            if (!String.IsNullOrEmpty(ApproveSettingsSalesOrder))
            {
                if (ApproveSettingsSalesOrder == "Yes")
                {
                    hdnApprovalReqInq.Value = "1";

                }
                else if (ApproveSettingsSalesOrder.ToUpper().Trim() == "NO")
                {
                    hdnApprovalReqInq.Value = "0";
                    dvAppRejRemarks.Style.Add("display", "none");
                    dvRevision.Style.Add("display", "none");
                    dvRevisionDate.Style.Add("display", "none");
                    dvApprove.Style.Add("display", "none");
                    dvReject.Style.Add("display", "none");


                }
            }

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

            //chinmoy added for MUltiUOM settings start           
            string MultiUOMSelection = ComBL.GetSystemSettingsResult("MultiUOMSelection");
            if (!String.IsNullOrEmpty(MultiUOMSelection))
            {
                if (MultiUOMSelection.ToUpper().Trim() == "YES")
                {
                    hddnMultiUOMSelection.Value = "1";
                }
                else if (MultiUOMSelection.ToUpper().Trim() == "NO")
                {
                    hddnMultiUOMSelection.Value = "0";
                    grid.Columns[9].Width = 0;
                    // Mantis Issue 24425, 24428
                    grid.Columns[10].Width = 0;
                    grid.Columns[11].Width = 0;
                    // End of Mantis Issue 24425, 24428
                }
            }
            //End
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

            // Rev Sanchita
            //IsToleranceInSalesOrder = ComBL.GetSystemSettingsResult("IsToleranceInSalesOrder");
            //if (!String.IsNullOrEmpty(IsToleranceInSalesOrder))
            //{
            //    if (IsToleranceInSalesOrder.ToUpper().Trim() == "YES")
            //    {
            //        divQtyTolerance.Visible = true;
            //    }
            //    else if (IsToleranceInSalesOrder.ToUpper().Trim() == "NO")
            //    {
            //        divQtyTolerance.Visible = false;
            //    }
            //}
            // End of Rev Sanchita

            if (!IsPostBack)
            {
                Session["SO_ProductDetails"] = null;
                Session["Entry_Type"] = null;
                Session["SO_ProductDetails"] = null;
                // Mantis Issue 25408
                Session["QuotationData"] = null;
                // End of Mantis Issue 25408
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
                string ShowPricingDetailsSalesOrder = ComBL.GetSystemSettingsResult("ShowPricingDetailsSalesOrder");
                if (!String.IsNullOrEmpty(ShowPricingDetailsSalesOrder))
                {
                    if (ShowPricingDetailsSalesOrder == "Yes")
                    {
                        hdnPricingDetail.Value = "1";
                    }
                    else if (ShowPricingDetailsSalesOrder.ToUpper().Trim() == "NO")
                    {
                        hdnPricingDetail.Value = "0";
                    }
                }

                string DeliveryScheduleRequired = ComBL.GetSystemSettingsResult("DeliveryScheduleRequired");
                if (!String.IsNullOrEmpty(DeliveryScheduleRequired))
                {
                    if (DeliveryScheduleRequired == "Yes")
                    {
                        hdnShowDeliverySchedule.Value = "1";
                    }
                    else if (DeliveryScheduleRequired.ToUpper().Trim() == "NO")
                    {
                        hdnShowDeliverySchedule.Value = "0";
                    }
                }
                else
                {
                    hdnShowDeliverySchedule.Value = "0";
                }
                string SalesRateBuyRateChecking = cbl.GetSystemSettingsResult("SalesRateBuyRateChecking");
                if (!String.IsNullOrEmpty(SalesRateBuyRateChecking))
                {
                    if (SalesRateBuyRateChecking == "Yes")
                    {
                        hdnSalesRateBuyRateChecking.Value = "1";

                    }
                    else if (SalesRateBuyRateChecking.ToUpper().Trim() == "NO")
                    {
                        hdnSalesRateBuyRateChecking.Value = "0";

                    }
                }





                hdnnproductIds.Value = "";
                SetControlValuesOnConfigSettings();
                hddnOutStandingBlock.Value = "0";
                //; SetTaxJSONData();

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

                    divcross.Visible = false;
                    btn_SaveRecords.ClientVisible = false;
                    ApprovalCross.Visible = true;
                    ddl_Branch.Enabled = false;
                    openlink.Visible = false;//Subhabrata

                }
                else
                {
                    branchid = Convert.ToString(Session["userbranchID"]);
                    branch = oDBEngine.getBranch(branchid, "") + branchid;
                    divcross.Visible = true;
                    btn_SaveRecords.ClientVisible = true;
                    ApprovalCross.Visible = false;
                    ddl_Branch.Enabled = false; // Change Due to Numbering Schema
                    openlink.Visible = true;//Subhabrata
                }
                #endregion Approvalval Section By Sam on 23052017 Start

                if (Request.QueryString.AllKeys.Contains("UpperApprove"))
                {
                    hdnUpperApproveReject.Value = Convert.ToString(Request.QueryString["UpperApprove"]);
                }
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
                if (Session["Entry_Type"] != null)
                {
                    ddlInventory.SelectedValue = (string)Session["Entry_Type"];
                }
                Session["Entry_Type"] = null;
                TabPage page = ASPxPageControl1.TabPages.FindByName("General");
                page.Visible = true;
                SetFinYearCurrentDate();
                GetFinacialYearBasedQouteDate();
                dt_PlOrderExpiry.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
                string finyear = Convert.ToString(Session["LastFinYear"]);
                //rev Pratik copy
                if (Request.QueryString["type"] == "COPY")
                {
                    DataTable dtBranch = oDBEngine.GetDataTable("select Order_BranchId from tbl_trans_SalesOrder where Order_Id ='" + Request.QueryString["key"].ToString() + "'");
                    if (dtBranch.Rows.Count > 0)
                    {
                        HttpContext.Current.Session["BranchID"] = dtBranch.Rows[0]["Order_BranchId"].ToString();
                    }

                }
                //if (Session["userbranchHierarchy"] != null)
                //{
                //    userbranch = Convert.ToString(Session["userbranchHierarchy"]);
                //}
                if (Request.QueryString["type"] == "COPY")
                {
                    userbranch = Convert.ToString(Session["BranchID"]);
                }
                else
                {
                    if (Session["userbranchHierarchy"] != null)
                    {
                        userbranch = Convert.ToString(Session["userbranchHierarchy"]);
                    }
                }
                
                //End of rev Pratik copy
                GetAllDropDownDetailForSalesOrder(userbranch);
                //Tanmoy Hierarchy
                bindHierarchy();
                ddlHierarchy.Enabled = false;
                //Tanmoy Hierarchy End
                #region Sandip Section For Approval Section Start
                if (Request.QueryString.AllKeys.Contains("status"))
                {
                    divcross.Visible = false;
                    divCrossActivity.Visible = true;
                    btn_SaveRecords.ClientVisible = false;
                    ApprovalCross.Visible = true;
                    ddl_Branch.Enabled = false;
                }
                else if (!string.IsNullOrEmpty(Request.QueryString["SalId"]))
                {

                    ApprovalCross.Visible = false;

                    divcross.Visible = false;
                    divCrossActivity.Visible = false;//subhabrata on 02-01-2018
                    //openlink.Visible = false;
                    hddnCustIdFromCRM.Value = "1";

                    // Mantis Issue 24913
                   // hdnSalesManAgentId.Value = Request.QueryString["AssignedToId"];
                    //string[] SalesManAgentId = oDBEngine.GetFieldValue1("tbl_trans_Sales", "sls_assignedTo", "sls_id ='" + Request.QueryString["SalId"].ToString() + "'", 1);
                    //if (SalesManAgentId.Length > 0)
                    //{
                    //    hdnSalesManAgentId.Value = SalesManAgentId[0].Trim();
                    //}

                    ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
                    proc.AddVarcharPara("@Action", 500, "GetSalesmanID");
                    proc.AddVarcharPara("@sla_id", 500, Request.QueryString["SalId"].ToString());
                    DataTable dtAgnt = proc.GetTable();
                    if (dtAgnt.Rows.Count > 0)
                    {
                        hdnSalesManAgentId.Value = dtAgnt.Rows[0]["SalesManAgentId"].ToString().Trim();
                    }
                   
                    txtSalesManAgent.Text = Request.QueryString["AssignedToName"];
                    txtSalesManAgent.Enabled = false;
                    I1.Disabled = true;
                    // End of Mantis Issue 24913


                    //PopulateCustomerDetailForCRM();
                    //PopulateCustomerDetail();//Subhabrata:11-12-2017

                    CallgridfromSales();

                }
                else
                {
                    divcross.Visible = true;
                    divCrossActivity.Visible = false;//subhabrata on 02-01-2018
                    btn_SaveRecords.ClientVisible = true;
                    ApprovalCross.Visible = false;
                    hddnCustIdFromCRM.Value = "0";
                    ddl_Branch.Enabled = true;
                    //lookup_Customer.ClientEnabled = true;
                    //PopulateCustomerDetail();//Subhabrata:11-12-2017
                    openlink.Visible = true;
                    dt_PLSales.ClientEnabled = true;
                }

                //if (!string.IsNullOrEmpty(Request.QueryString["SalId"]))
                //{
                //    divcross1.Visible = false;
                //    divcross.Visible = false;
                //}
                //else
                //{
                //    divcross1.Visible = true;
                //    divcross.Visible = true;

                //}
                #endregion Sandip Section For Approval Dtl Section End
                //kaushik 24-2-2017
                Session["SalesOrderBillingAddressLookup"] = null;
                Session["SalesOrdeShippingAddressLookup"] = null;
                Session["SalesOrderAddressDtl"] = null;
                //kaushik 24-2-2017
                Session["InlineRemarks"] = null;
                Session["CustomerDetail"] = null;
                Session["SalesWarehouseData"] = null;
                Session["LoopSalesOrderWarehouse"] = 1;
                Session["STaxDetails"] = null;
                Session["SalesOrderTaxDetails"] = null;
                Session["OrderDetails"] = null;
                Session["exportval1"] = null;
                Session["MultiUOMData"] = null;
                Session["ProjectadditionRemarks"] = null;
                PopulateGSTCSTVATCombo(DateTime.Now.ToString("yyyy-MM-dd"));
                PopulateChargeGSTCSTVATCombo(DateTime.Now.ToString("yyyy-MM-dd"));
                string strOrderId = "";
                string strApprovestatus = "";
                if (Request.QueryString["key"] != null)
                {
                    //rev Pratik Copy
                    //if (Convert.ToString(Request.QueryString["key"]) != "ADD")
                    if (Convert.ToString(Request.QueryString["key"]) != "ADD" && Convert.ToString(Request.QueryString["type"]) != "COPY")
                    //End of rev Pratik Copy
                    {
                        ddl_AmountAre.ClientEnabled = false;
                        txtCustName.ClientEnabled = false;
                        hdnPageStatus.Value = "update";
                        strApprovestatus = Convert.ToString(Request.QueryString["type1"]);
                        hdnPageStatForApprove.Value = strApprovestatus;
                        if (Request.QueryString["type1"] == "PO")
                        {
                            ltrTitle.Text = "Approve Sales Order";
                            btn_SaveRecords.ClientVisible = false;
                            btn_SaveExit.ClientVisible = false;

                        }
                        else
                        {
                            ltrTitle.Text = "Modify Sales Order";
                        }
                        strOrderId = Convert.ToString(Request.QueryString["key"]);
                        hdnEditOrderId.Value = strOrderId;
                        Session["OrderID"] = strOrderId;
                        Session["ActionType"] = "Edit";
                        hdAddOrEdit.Value = "Edit";
                        hddnActionFieldOnStockBlock.Value = "Edit";
                        #region Sandip Section For Approval Section Start
                        if (Request.QueryString["status"] == null)
                        {
                            IsExistsDocumentInERPDocApproveStatus(strOrderId);
                        }
                        #endregion Sandip Section For Approval Dtl Section End
                        Session["SalesWarehouseData"] = GetOrderWarehouseData();
                        Session["OrderDetails"] = GetOrderData().Tables[0];
                        Session["InlineRemarks"] = GetOrderData().Tables[1];
                        ddl_Branch.Enabled = false;
                        rdl_Salesquotation.Enabled = false;
                        //kaushik 25-2-2017
                        Session["KeyVal_InternalID"] = "PISO" + strOrderId;
                        //kaushik 25-2-2017
                        rdl_Salesquotation.Enabled = false;
                        ddl_numberingScheme.Enabled = false;
                        txt_SlOrderNo.ClientEnabled = false;
                        //GetAllDropDownDetailForSalesOrder();
                        if (Session["userbranchHierarchy"] != null)
                        {
                            userbranch = Convert.ToString(Session["userbranchHierarchy"]);
                        }
                        GetAllDropDownDetailForSalesOrder(userbranch);
                        SetOrderDetails();
                        #region Debjyoti Get Tax Details in Edit Mode

                        Session["STaxDetails"] = GetTaxDataWithGST(GetTaxData(dt_PLSales.Date.ToString("yyyy-MM-dd")));


                        // Session["SalesOrderTaxDetails"] = GetSalesOrderTaxData().Tables[0];
                        DataTable quotetable = GetQuotationEditedTaxData().Tables[0];
                        if (quotetable == null)
                        {
                            CreateDataTaxTable(uniqueId.Value.ToString());
                        }
                        else
                        {
                            Session["SalesOrderFinalTaxRecord" + Convert.ToString(uniqueId.Value)] = quotetable;
                        }

                        Session["MultiUOMData"] = GetMultiUOMData();
                        #endregion Debjyoti Get Tax Details in Edit Mode
                        //rev rajdip for running data on edit mode 
                        DataTable Orderdt = GetOrderData().Tables[0];
                        decimal TotalQty = Orderdt.AsEnumerable().Sum(x => x.Field<decimal>("Quantity"));
                        decimal TotalAmt = Orderdt.AsEnumerable().Sum(x => x.Field<decimal>("Amount"));
                        decimal TotalTaxableAmt = Orderdt.AsEnumerable().Sum(x => x.Field<decimal>("TaxAmount"));
                        decimal saleprice = Orderdt.AsEnumerable().Sum(x => x.Field<decimal>("SalePrice"));
                        decimal AmountWithTaxValue = TotalAmt + TotalTaxableAmt;
                        ASPxLabel12.Text = TotalQty.ToString();
                        bnrLblTaxableAmtval.Text = TotalTaxableAmt.ToString();
                        bnrLblTaxAmtval.Text = TotalTaxableAmt.ToString();
                        bnrlblAmountWithTaxValue.Text = AmountWithTaxValue.ToString();
                        bnrLblInvValue.Text = AmountWithTaxValue.ToString();
                        //end rev rajdip
                        grid.DataSource = GetSalesOrder();
                        grid.DataBind();
                        //kaushik 24-2-2017
                        Session["SalesOrderAddressDtl"] = GetBillingAddress();
                        hdnmodeId.Value = "SalesOrder" + strOrderId;
                        //kaushik 24-2-2017
                        // Mantis Issue 24913
                        DataTable dtFromCRM = GetOrderData().Tables[2];
                        if (dtFromCRM.Rows.Count > 0 && Convert.ToInt32(dtFromCRM.Rows[0]["IsFromCRM"]) ==1 )
                        {
                             txtSalesManAgent.Enabled = false;
                             I1.Disabled = true;
                        }
                        // End of Mantis Issue 24913

                        //lookup_quotation.Properties.PopupFilterMode = DevExpress.XtraEditors.Popup.TreeListLookUpEditPopupForm.Contains;
                    }
                    //rev Pratik Copy
                    else if (Convert.ToString(Request.QueryString["key"]) != "ADD" && Convert.ToString(Request.QueryString["type"]) == "COPY")
                    {
                        ddl_AmountAre.ClientEnabled = false;
                        txtCustName.ClientEnabled = false;
                        hdnPageStatus.Value = "first";
                        strApprovestatus = Convert.ToString(Request.QueryString["type1"]);
                        hdnPageStatForApprove.Value = strApprovestatus;
                        if (Request.QueryString["type1"] == "PO")
                        {
                            ltrTitle.Text = "Approve Sales Order";
                            btn_SaveRecords.ClientVisible = false;
                            btn_SaveExit.ClientVisible = false;

                        }
                        else
                        {
                            ltrTitle.Text = "Add Sales Order";
                        }
                        strOrderId = Convert.ToString(Request.QueryString["key"]);
                        hdnEditOrderId.Value = strOrderId;
                        Session["OrderID"] = strOrderId;
                        Session["ActionType"] = "Add";
                        hdAddOrEdit.Value = "Add";
                        hddnActionFieldOnStockBlock.Value = "Add";
                        #region Sandip Section For Approval Section Start
                        //if (Request.QueryString["status"] == null)
                        //{
                        //    IsExistsDocumentInERPDocApproveStatus(strOrderId);
                        //}
                        #endregion Sandip Section For Approval Dtl Section End
                        Session["SalesWarehouseData"] = GetOrderWarehouseData();
                        Session["OrderDetails"] = GetOrderData().Tables[0];
                        Session["InlineRemarks"] = GetOrderData().Tables[1];
                        ddl_Branch.Enabled = false;
                        rdl_Salesquotation.Enabled = false;
                        //kaushik 25-2-2017
                        Session["KeyVal_InternalID"] = "PISO" + strOrderId;
                        //kaushik 25-2-2017
                        rdl_Salesquotation.Enabled = false;
                        //ddl_numberingScheme.Enabled = false;
                       // txt_SlOrderNo.ClientEnabled = false;
                        //GetAllDropDownDetailForSalesOrder();
                        if (Request.QueryString["type"] == "COPY")
                        {
                            DataTable dtBranch = oDBEngine.GetDataTable("select Order_BranchId from tbl_trans_SalesOrder where Order_Id ='" + Request.QueryString["key"].ToString() + "'");
                            if (dtBranch.Rows.Count > 0)
                            {
                                HttpContext.Current.Session["BranchID"] = dtBranch.Rows[0]["Order_BranchId"].ToString();
                            }

                        }
                        //if (Session["userbranchHierarchy"] != null)
                        //{
                        //    userbranch = Convert.ToString(Session["userbranchHierarchy"]);
                        //}
                        if (Request.QueryString["type"] == "COPY")
                        {
                            userbranch = Convert.ToString(Session["BranchID"]);
                        }
                        else
                        {
                            if (Session["userbranchHierarchy"] != null)
                            {
                                userbranch = Convert.ToString(Session["userbranchHierarchy"]);
                            }
                        }
                        GetAllDropDownDetailForSalesOrder(userbranch);
                        SetOrderDetailsCopy();
                        ddl_Num.Visible = true;
                        txt_SlOrderNo.Text = "";
                        txtCustName.ClientEnabled = true;
                        dt_PLSales.ClientEnabled = true;
                        Session["STaxDetails"] = GetTaxDataWithGST(GetTaxData(dt_PLSales.Date.ToString("yyyy-MM-dd")));


                        // Session["SalesOrderTaxDetails"] = GetSalesOrderTaxData().Tables[0];
                        DataTable quotetable = GetQuotationEditedTaxData().Tables[0];
                        if (quotetable == null)
                        {
                            CreateDataTaxTable(uniqueId.Value.ToString());
                        }
                        else
                        {
                            Session["SalesOrderFinalTaxRecord" + Convert.ToString(uniqueId.Value)] = quotetable;
                        }

                        Session["MultiUOMData"] = GetMultiUOMData();
                        //rev rajdip for running data on edit mode 
                        DataTable Orderdt = GetOrderData().Tables[0];
                        decimal TotalQty = Orderdt.AsEnumerable().Sum(x => x.Field<decimal>("Quantity"));
                        decimal TotalAmt = Orderdt.AsEnumerable().Sum(x => x.Field<decimal>("Amount"));
                        decimal TotalTaxableAmt = Orderdt.AsEnumerable().Sum(x => x.Field<decimal>("TaxAmount"));
                        decimal saleprice = Orderdt.AsEnumerable().Sum(x => x.Field<decimal>("SalePrice"));
                        decimal AmountWithTaxValue = TotalAmt + TotalTaxableAmt;
                        ASPxLabel12.Text = TotalQty.ToString();
                        bnrLblTaxableAmtval.Text = TotalTaxableAmt.ToString();
                        bnrLblTaxAmtval.Text = TotalTaxableAmt.ToString();
                        bnrlblAmountWithTaxValue.Text = AmountWithTaxValue.ToString();
                        bnrLblInvValue.Text = AmountWithTaxValue.ToString();
                        //end rev rajdip
                        grid.DataSource = GetSalesOrder();
                        grid.DataBind();
                        //kaushik 24-2-2017
                        Session["SalesOrderAddressDtl"] = GetBillingAddress();
                        hdnmodeId.Value = "SalesOrder" + strOrderId;
                        //kaushik 24-2-2017

                        //lookup_quotation.Properties.PopupFilterMode = DevExpress.XtraEditors.Popup.TreeListLookUpEditPopupForm.Contains;
                    }
                    //End of rev Pratik Copy
                    else
                    {
                        ltrTitle.Text = "Add Sales Order";
                        Session["ActionType"] = "Add";
                        hdnmodeId.Value = "Add";
                        hdAddOrEdit.Value = "Add";
                        hddnActionFieldOnStockBlock.Value = "Add";
                        hdnPageStatus.Value = "first";
                        Session["OrderID"] = "";
                        CreateDataTaxTable(uniqueId.Value.ToString());
                        if (Request.QueryString["BasketId"] != null)
                        {
                            string basketId = Convert.ToString(Request.QueryString["BasketId"]);
                            fillRecordFromBasket(basketId);
                            hdBasketId.Value = basketId;
                            ddl_AmountAre.Value = "2";
                        }
                        else
                        {
                            ddl_AmountAre.Value = objmaster.GetSettings("DefaultTaxTypeForSalesOrder");
                            hdBasketId.Value = "";
                        }
                    }
                }

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GridCallBack()", true);
                IsUdfpresent.Value = Convert.ToString(getUdfCount());
                #region Samrat Roy -- Hide Save Button in Edit Mode
                //rev Pratik Copy
                //if (Request.QueryString["req"] != null && Request.QueryString["req"] == "V")
                if (Request.QueryString["req"] != null && Request.QueryString["req"] == "V" && Convert.ToString(Request.QueryString["type"]) != "COPY")
                //End of rev Pratik copy
                {
                    ltrTitle.Text = "View Sales Order";
                    lbl_quotestatusmsg.Text = "*** View Mode Only";
                    btn_SaveRecords.ClientVisible = false;
                    btn_SaveExit.Visible = false;
                    lbl_quotestatusmsg.Visible = true;
                }
                #endregion
                if (lookup_quotation.GridView.GetSelectedFieldValues("Quote_Id").Count > 0)
                {
                    dt_PLSales.ClientEnabled = false;
                }
                else
                {
                    //rev Pratik Copy
                    //if (Convert.ToString(hddnDocumentIdTagged.Value) == "1")
                    if (Convert.ToString(hddnDocumentIdTagged.Value) == "1" && Convert.ToString(Request.QueryString["type"]) != "COPY")
                    //End of rev Pratik copy
                    {
                        dt_PLSales.ClientEnabled = false;
                    }
                    else
                    {
                        dt_PLSales.ClientEnabled = true;
                    }
                }
                //rev Pratik Copy
                //if (Convert.ToString(hddnDocumentIdTagged.Value) == "1" && hdnApprovalReqInq.Value == "0")
                if (Convert.ToString(hddnDocumentIdTagged.Value) == "1" && hdnApprovalReqInq.Value == "0" && Convert.ToString(Request.QueryString["type"]) != "COPY")
                //End of rev Pratik copy
                {
                    ltrTitle.Text = "View Sales Order";
                    lbl_quotestatusmsg.Text = "*** Used in other module.";
                    btn_SaveRecords.ClientVisible = false;
                    btn_SaveExit.Visible = false;
                    lbl_quotestatusmsg.Visible = true;
                    dt_PLSales.ClientEnabled = false;
                }

                #region By Surojit get key value for Convertion Overide

                hdnConvertionOverideVisible.Value = objmaster.GetSettings("ConvertionOverideVisible");
                hdnShowUOMConversionInEntry.Value = objmaster.GetSettings("ShowUOMConversionInEntry");

                #endregion

                //Cross butten Link Tanmoy 
                if (Convert.ToString(Request.QueryString["isApprovsalesOrder"]) == "Y")
                {
                    lnkBack.HRef = "/OMS/Management/Activities/ApproveSaleasOrder.aspx";
                }
                else if (Convert.ToString(Request.QueryString["isInvoicecumChallan"]) == "Y")
                {
                    lnkBack.HRef = "/OMS/Management/Activities/InvoiceCumChallanSO.aspx";
                }
                else
                {
                    lnkBack.HRef = "/OMS/Management/Activities/SalesOrderEntityList.aspx";
                }
                //Cross butten Link Tanmoy
            }
            //  Bind_Currency();
            #region Subhra Section Start

            //ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "Js1", "fn_PopOpen('LDP0000002');", true);

            #endregion Subhra Section End
            if (Request.QueryString["key"] != null && Request.QueryString["key"] != "ADD")
            {
                //SetOrderDetails();
                string strOrderId1 = Convert.ToString(Request.QueryString["key"]);
                Session["OrderID"] = strOrderId1;
                //Chinmoy Edited below line
                DataSet dsOrderEditdt = GetOrderEditData();
                DataTable OrderEditdt = dsOrderEditdt.Tables[0];
                if (OrderEditdt != null && OrderEditdt.Rows.Count > 0)
                {
                    string Doctype = Convert.ToString(OrderEditdt.Rows[0]["Doctype"]);
                    string Quoids = Convert.ToString(OrderEditdt.Rows[0]["Order_Quotation_Ids"]);

                    string Order_Date = Convert.ToString(OrderEditdt.Rows[0]["Order_Date"]);
                    string Customer_Id = Convert.ToString(OrderEditdt.Rows[0]["Customer_Id"]);
                    if (!String.IsNullOrEmpty(Quoids) && Quoids.Split(',')[0] != "0")
                    {
                        Session["Lookup_QuotationIds"] = Quoids;
                        string[] eachQuo = Quoids.Split(',');
                        if (eachQuo.Length > 1)//More than one quotation
                        {
                            if (Doctype == "QN")
                            {
                                rdl_Salesquotation.SelectedValue = "QN";
                                dt_Quotation.Text = "Multiple Select Quotation Dates";
                            }
                            if (Doctype == "SINQ")
                            {
                                rdl_Salesquotation.SelectedValue = "SINQ";
                                dt_Quotation.Text = "Multiple Select Inquiry Dates";
                            }
                            BindLookUp(Customer_Id, Order_Date, Doctype);
                            foreach (string val in eachQuo)
                            {
                                lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                            }
                            // lbl_MultipleDate.Attributes.Add("style", "display:block");
                        }
                        else if (eachQuo.Length == 1)//Single Quotation
                        { //lbl_MultipleDate.Attributes.Add("style", "display:none"); }
                            if (Doctype == "QN")
                            {
                                rdl_Salesquotation.SelectedValue = "QN";

                            }
                            if (Doctype == "SINQ")
                            {
                                rdl_Salesquotation.SelectedValue = "SINQ";

                            }

                            BindLookUp(Customer_Id, Order_Date, Doctype);
                            foreach (string val in eachQuo)
                            {
                                lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                            }
                        }
                        else//No Quotation selected
                        {
                            BindLookUp(Customer_Id, Order_Date, Doctype);
                        }
                    }
                }
            }
            //rev Pratik copy
            else if (Request.QueryString["key"] != null && Request.QueryString["key"] != "ADD" && Request.QueryString["key"] == "COPY")
            {
                //SetOrderDetails();
                string strOrderId1 = Convert.ToString(Request.QueryString["key"]);
                Session["OrderID"] = strOrderId1;
                //Chinmoy Edited below line
                DataSet dsOrderEditdt = GetOrderEditData();
                DataTable OrderEditdt = dsOrderEditdt.Tables[0];
                if (OrderEditdt != null && OrderEditdt.Rows.Count > 0)
                {
                    string Doctype = Convert.ToString(OrderEditdt.Rows[0]["Doctype"]);
                    string Quoids = Convert.ToString(OrderEditdt.Rows[0]["Order_Quotation_Ids"]);

                    string Order_Date = Convert.ToString(OrderEditdt.Rows[0]["Order_Date"]);
                    string Customer_Id = Convert.ToString(OrderEditdt.Rows[0]["Customer_Id"]);
                    //Pratik
                    if (!String.IsNullOrEmpty(Quoids) && Quoids.Split(',')[0] != "0")
                    {
                        Session["Lookup_QuotationIds"] = Quoids;
                        string[] eachQuo = Quoids.Split(',');
                        if (eachQuo.Length > 1)//More than one quotation
                        {
                            if (Doctype == "QN")
                            {
                                rdl_Salesquotation.SelectedValue = "QN";
                                dt_Quotation.Text = "Multiple Select Quotation Dates";
                            }
                            if (Doctype == "SINQ")
                            {
                                rdl_Salesquotation.SelectedValue = "SINQ";
                                dt_Quotation.Text = "Multiple Select Inquiry Dates";
                            }
                            BindLookUp(Customer_Id, Order_Date, Doctype);
                            foreach (string val in eachQuo)
                            {
                                lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                            }
                            // lbl_MultipleDate.Attributes.Add("style", "display:block");
                        }
                        else if (eachQuo.Length == 1)//Single Quotation
                        { //lbl_MultipleDate.Attributes.Add("style", "display:none"); }
                            if (Doctype == "QN")
                            {
                                rdl_Salesquotation.SelectedValue = "QN";

                            }
                            if (Doctype == "SINQ")
                            {
                                rdl_Salesquotation.SelectedValue = "SINQ";

                            }

                            BindLookUp(Customer_Id, Order_Date, Doctype);
                            foreach (string val in eachQuo)
                            {
                                lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                            }
                        }
                        else//No Quotation selected
                        {
                            BindLookUp(Customer_Id, Order_Date, Doctype);
                        }
                    }
                    //Pratik
                }
                dt_PLSales.ClientEnabled = true;
            }
            //End of rev Pratik copy
            //PopulateCustomerDetail();
            GetSalesOrderSchemaLength();
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

        public void SetControlValuesOnConfigSettings()
        {
            //order Discount
            string strSQL = "Select 'Y' From Config_SystemSettings Where Variable_Name='IsSODiscountPercentage' AND Variable_Value='Yes'";
            DataTable dtSQL = oDBEngine.GetDataTable(strSQL);
            if (dtSQL != null && dtSQL.Rows.Count > 0)
            {
                IsDiscountPercentage.Value = "Y";
                // Mantis Issue 24721
                //grid.Columns[12].Caption = "Add/Less (%)";
                grid.Columns[14].Caption = "Add/Less (%)";
                // End of Mantis Issue 24721
            }
            else
            {
                // Mantis Issue 24721
                //grid.Columns[12].Caption = "Add/Less Amt";
                grid.Columns[14].Caption = "Add/Less Amt";
                // End of Mantis Issue 24721
                IsDiscountPercentage.Value = "N";
            }//End

            //Tagging Checking from Config
            string strTagging = "select 'Y' from Config_SystemSettings where Variable_Name='SaleOrder_With_Proforma_Quotation_Tagging'  AND Variable_Value='Yes'";
            DataTable dtTagging = oDBEngine.GetDataTable(strTagging);

            if (dtTagging != null && dtTagging.Rows.Count > 0)
            {
                hdnConfigValueForTagging.Value = "Y";
            }
            else
            {
                hdnConfigValueForTagging.Value = "N";
            }
            //End


            //Inventory Item Checking from Config
            string strIsInventory = "select 'Y' from Config_SystemSettings where Variable_Name='SaleOrder_InventoryConfig'  AND Variable_Value='Yes'";
            DataTable dtIsInventory = oDBEngine.GetDataTable(strIsInventory);

            if (dtIsInventory != null && dtIsInventory.Rows.Count > 0)
            {
                ddlInventory.SelectedValue = "Y";
            }
            else
            {
                ddlInventory.SelectedValue = "N";
            }
            //End
        }
        //Debjyoti GST on 30-06-2017
        public void SetTaxJSONData()
        {
            #region NewTaxblock
            //string ItemLevelTaxDetails = string.Empty; string HSNCodewisetaxSchemid = string.Empty; string BranchWiseStateTax = string.Empty; string StateCodeWiseStateIDTax = string.Empty;
            //gstTaxDetails.GetTaxData(ref ItemLevelTaxDetails, ref HSNCodewisetaxSchemid, ref BranchWiseStateTax, ref StateCodeWiseStateIDTax, "S");
            //HDItemLevelTaxDetails.Value = ItemLevelTaxDetails;
            //HDHSNCodewisetaxSchemid.Value = HSNCodewisetaxSchemid;
            //HDBranchWiseStateTax.Value = BranchWiseStateTax;
            //HDStateCodeWiseStateIDTax.Value = StateCodeWiseStateIDTax;


            #endregion
        }
        //END
        #region Customer and Product Bind From Sales Phone Call


        void CallgridfromSales()
        {
            string Sproduct = "";
            dt_PLSales.ClientEnabled = false;
            int id;
            string CustomerName = string.Empty;
            if (!string.IsNullOrEmpty(Request.QueryString["type"]) && !string.IsNullOrEmpty(Request.QueryString["SalId"]))
            {


                string strCustomer = oDBEngine.ExeSclar("select sls_contactlead_id from tbl_trans_sales where sls_id=" + Request.QueryString["SalId"]);
                hdnCustomerId.Value = strCustomer;
                CustomerName = oDBEngine.ExeSclar("select (isnull(TMC.cnt_firstName,'')+' '+ isnull(TMC.cnt_middleName,'')+' ' +isnull(TMC.cnt_LastName,'')) as CustomerName from tbl_master_contact TMC where TMC.cnt_internalId='" + strCustomer + "'");
                if (!string.IsNullOrEmpty(strCustomer))
                {
                    txtCustName.Text = CustomerName;
                }

                DataTable dt = GetCustomerOnIndustry(Convert.ToInt32(Request.QueryString["SalId"]));
                if (dt != null && dt.Rows.Count > 0)
                {
                    hddnCustomers.Value = Convert.ToString(dt.Rows[0]["EntityId"]).TrimStart(',');
                }

                //Region As told by Sanjoy sir Product will be populated as per Industry 
                //if (Request.QueryString["type"] == "2" || Request.QueryString["type"] == "3")
                //{
                //    Sproduct = oDBEngine.ExeSclar("select a.act_productIds from tbl_trans_sales as s,tbl_trans_activies  as a  where s.sls_activity_id=a.act_id and  s.sls_id=" + Request.QueryString["SalId"]);
                //}
                //else
                //{
                //    id = oDBEngine.ExeInteger("select sls_product_id from tbl_trans_sales where sls_id=" + Request.QueryString["SalId"]);
                //    Sproduct = Convert.ToString(id);
                //}
                //END

                DataTable dtProducts = GetProductsOnIndustry(Convert.ToInt32(Request.QueryString["SalId"]));

                if (dtProducts != null && dtProducts.Rows.Count > 0)
                {
                    hdnnproductIds.Value = Convert.ToString(dtProducts.Rows[0]["ProductIds"]).TrimStart(',');
                }

                //hdnnproductIds.Value = Sproduct.TrimStart(',');

                DataTable dtsalesproduct = new DataTable();
                dtsalesproduct = objSlaesActivitiesBL.GetQuotationDetailsFromSalesOrder_PhoneCall(Sproduct);
                //grid.DataSource = GetSalesOrderInfo1(dtsalesproduct, "");
                grid.DataSource = null;
                grid.DataBind();

                hdnIsFromActivity.Value = "Y";
                btn_SaveRecords.ClientVisible = false;
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
            //DataTable dt = (DataTable)Session["MultiUOMData"];
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
            // Mantis Issue 24913
            grid_MultiUOM.JSProperties["cpSetBaseQtyRateInGrid"] = "";
            // End of Mantis Issue 24913

            if (SpltCmmd == "MultiUOMDisPlay")
            {
                grid_MultiUOM.JSProperties["cpOpenFocus"] = "";
                DataTable MultiUOMData = new DataTable();

                if (Session["MultiUOMData"] != null)
                {
                    MultiUOMData = (DataTable)Session["MultiUOMData"];
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
                    // Mantis Issue 24425, 24428
                    MultiUOMData.Columns.Add("DetailsId", typeof(string));
                    MultiUOMData.Columns.Add("BaseRate", typeof(Decimal));
                    MultiUOMData.Columns.Add("AltRate", typeof(Decimal));
                    MultiUOMData.Columns.Add("UpdateRow", typeof(string));
                    MultiUOMData.Columns.Add("MultiUOMSR", typeof(string));
                    // End of Mantis Issue 24425, 24428

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
                // Mantis Issue 24425, 24428
                int MultiUOMSR = 1;
                // End of Mantis Issue 24425, 24428

                string SrlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                string Quantity = Convert.ToString(e.Parameters.Split('~')[2]);
                string UOM = Convert.ToString(e.Parameters.Split('~')[3]);
                string AltUOM = Convert.ToString(e.Parameters.Split('~')[4]);
                string AltQuantity = Convert.ToString(e.Parameters.Split('~')[5]);
                string UomId = Convert.ToString(e.Parameters.Split('~')[6]);
                string AltUomId = Convert.ToString(e.Parameters.Split('~')[7]);
                string ProductId = Convert.ToString(e.Parameters.Split('~')[8]);
                // Mantis Issue 24397
                string BaseRate = Convert.ToString(e.Parameters.Split('~')[9]);
                string AltRate = Convert.ToString(e.Parameters.Split('~')[10]);
                string UpdateRow = Convert.ToString(e.Parameters.Split('~')[11]);
                // End of Mantis Issue 24397


                DataTable allMultidataDetails = (DataTable)Session["MultiUOMData"];



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
                        // Mantis Issue 24397 [ if "Update Row" checkbox is checked, then all existing Update Row in the grid will be set to False]
                        if(UpdateRow == "True"){
                            item["UpdateRow"] = "False";
                        }
                        // End of Mantis Issue 24397
                    }
                }

                if (Validcheck != "DuplicateUOM")
                {
                    if (Session["MultiUOMData"] != null)
                    {

                        MultiUOMSaveData = (DataTable)Session["MultiUOMData"];

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
                        // Mantis Issue 24397
                        MultiUOMSaveData.Columns.Add("DetailsId", typeof(string));
                        MultiUOMSaveData.Columns.Add("BaseRate", typeof(Decimal));
                        MultiUOMSaveData.Columns.Add("AltRate", typeof(Decimal));
                        MultiUOMSaveData.Columns.Add("UpdateRow", typeof(string));
                        MultiUOMSaveData.Columns.Add("MultiUOMSR", typeof(string));
                        // End of Mantis Issue 24397
                    }

                    // Mantis Issue 24397
                    //MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId);

                    DataRow thisRow;
                    if (MultiUOMSaveData.Rows.Count > 0)
                    {
                        // Mantis Issue 24913
                        //thisRow = (DataRow)MultiUOMSaveData.Rows[MultiUOMSaveData.Rows.Count - 1];
                        //MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, "", BaseRate, AltRate, UpdateRow, (Convert.ToInt16(thisRow["MultiUOMSR"]) + 1));
                        MultiUOMSR = Convert.ToInt32(MultiUOMSaveData.Compute("max([MultiUOMSR])", string.Empty)) + 1;
                        MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, "", BaseRate, AltRate, UpdateRow, MultiUOMSR);
                        // End of Mantis Issue 24913
                    }
                    else
                    {
                        MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, "", BaseRate, AltRate, UpdateRow, MultiUOMSR);
                    }
                    // End of Mantis Issue 24397
                    MultiUOMSaveData.AcceptChanges();
                    Session["MultiUOMData"] = MultiUOMSaveData;

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
                        //Session["MultiUOMData"] = MultiUOMSaveData;
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
                DataTable dt = (DataTable)Session["MultiUOMData"];

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
                Session["MultiUOMData"] = dt;
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
                DataTable dt = (DataTable)Session["MultiUOMData"];

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] MultiUoMresult = dt.Select("SrlNo ='" + SrlNo + "'");
                    foreach (DataRow item in MultiUoMresult)
                    {
                        item.Table.Rows.Remove(item);
                    }
                }
                Session["MultiUOMData"] = dt;
            }

            // Mantis Issue 24397
            else if (SpltCmmd == "SetBaseQtyRateInGrid")
            {
                DataTable dt = new DataTable();

                if (Session["MultiUOMData"] != null)
                {
                    string SrlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                    dt = (DataTable)HttpContext.Current.Session["MultiUOMData"];
                    DataRow[] MultiUoMresult = dt.Select("SrlNo ='" + SrlNo + "' and UpdateRow ='True'");

                    Int64 SelNo = Convert.ToInt64(MultiUoMresult[0]["SrlNo"]);
                    Decimal BaseQty = Convert.ToDecimal(MultiUoMresult[0]["Quantity"]);
                    Decimal BaseRate = Convert.ToDecimal(MultiUoMresult[0]["BaseRate"]);
                    // Mantis Issue 24425, 24428
                    Decimal AltQuantity = Convert.ToDecimal(MultiUoMresult[0]["AltQuantity"]);
                    String AltUOM = Convert.ToString(MultiUoMresult[0]["AltUOM"]);
                    // End of Mantis Issue 24425, 24428

                    grid_MultiUOM.JSProperties["cpSetBaseQtyRateInGrid"] = "1";
                    grid_MultiUOM.JSProperties["cpBaseQty"] = BaseQty;
                    grid_MultiUOM.JSProperties["cpBaseRate"] = BaseRate;
                    // Mantis Issue 24425, 24428
                    grid_MultiUOM.JSProperties["cpAltQuantity"] = AltQuantity;
                    grid_MultiUOM.JSProperties["cpAltUOM"] = AltUOM;
                    // End of Mantis Issue 24425, 24428

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
            // End of Mantis Issue 24397
            // Mantis Issue 24425, 24428
            else if (SpltCmmd == "EditData")
            {
                string AltUOMKeyValuewithqnty = e.Parameters.Split('~')[1];
                string AltUOMKeyValue = e.Parameters.Split('~')[2];
                string AltUOMKeyqnty = AltUOMKeyValuewithqnty.Split('|')[1];

                string SrlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                DataTable dt = (DataTable)Session["MultiUOMData"];

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
                Session["MultiUOMData"] = dt;
            }



            else if (SpltCmmd == "UpdateRow")
            {


                string SrlNoR = e.Parameters.Split('~')[1];
                string AltUOMKeyValue = e.Parameters.Split('~')[7];
                string AltUOMKeyqnty = e.Parameters.Split('~')[5];
                string muid = e.Parameters.Split('~')[12];
                // Mantis Issue 24913
                string SrlNo = "0";
                string Validcheck = "";
                // End of Mantis Issue 24913

                DataTable MultiUOMSaveData = new DataTable();

                DataTable dt = (DataTable)Session["MultiUOMData"];
                // Mantis Issue 24913
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] MultiUoMresult = dt.Select("MultiUOMSR ='" + muid + "'");
                    foreach (DataRow item in MultiUoMresult)
                    {
                        // Mantis Issue 24913
                        SrlNo = Convert.ToString(item["SrlNo"]);
                        // End of Mantis Issue 24913
                        //item.Table.Rows.Remove(item);
                        //break;

                    }
                }
                // End of Mantis Issue 24913

                // Mantis Issue 24913
                //string SrlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                // End of Mantis Issue 24913
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
           
                // Mantis Issue 24913
                //dt.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId,"", BaseRate, AltRate, UpdateRow, muid);

                DataRow[] MultiUoMresultResult = dt.Select("SrlNo ='" + SrlNo + "' and MultiUOMSR <>'" + muid + "'");

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
                    // Mantis Issue 24428  [ if "Update Row" checkbox is checked, then all existing Update Row in the grid will be set to False]
                    if (UpdateRow == "True")
                    {
                        item["UpdateRow"] = "False";
                    }
                    // End of Mantis Issue 24428 
                }


                if (Validcheck != "DuplicateUOM")
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataRow[] MultiUoMresult = dt.Select("MultiUOMSR ='" + muid + "'");
                        foreach (DataRow item in MultiUoMresult)
                        {
                            //// Mantis Issue 24913
                            //SrlNo = Convert.ToString(item["SrlNo"]);
                            //// End of Mantis Issue 24913
                            item.Table.Rows.Remove(item);
                            break;

                        }
                    }
                    /*Rev work start 03.05.2022 Mantise :24867 Multi UOM Window becoming Blank in case of Sales Order made from Quotation with Partial Items*/
                    //dt.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, "", BaseRate, AltRate, UpdateRow, muid);
                    dt.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId,0, BaseRate, AltRate, UpdateRow, muid);
                    /*Rev work close 03.05.2022 Mantise :24867 Multi UOM Window becoming Blank in case of Sales Order made from Quotation with Partial Items*/
                }
                //End Mantis Issue 24913

                Session["MultiUOMData"] = dt;
                MultiUOMSaveData = (DataTable)Session["MultiUOMData"];

                MultiUOMSaveData.AcceptChanges();
                Session["MultiUOMData"] = MultiUOMSaveData;

                if (MultiUOMSaveData != null && MultiUOMSaveData.Rows.Count > 0)
                {
                    DataView dvData = new DataView(MultiUOMSaveData);
                    // dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                    // Mantis Issue 24913
                    dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                    // End of Mantis Issue 24913

                    grid_MultiUOM.DataSource = dvData;
                    grid_MultiUOM.DataBind();
                }
            }
            // End of Mantis Issue 24425, 24428
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

        #region OutstandingReportExport(16-04-2018)

        protected void cmbExport1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval1"] == null)
                {
                    Session["exportval1"] = Filter;
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval1"]) != Filter)
                {
                    Session["exportval1"] = Filter;
                    bindexport(Filter);
                }
            }
        }
        public void bindexport(int Filter)
        {
            //CustomerOutstanding.Columns[5].Visible = false;
            string filename = "CustomerOutStanding";
            exporter1.FileName = filename;
            exporter1.FileName = "GrdCustomerOutstanding";

            exporter1.PageHeader.Left = "CustomerOutStanding";
            exporter1.PageFooter.Center = "[Page # of Pages #]";
            exporter1.PageFooter.Right = "[Date Printed]";

            switch (Filter)
            {
                case 1:
                    exporter1.WritePdfToResponse();
                    break;
                case 2:
                    exporter1.WriteXlsToResponse();
                    break;
                case 3:
                    exporter1.WriteRtfToResponse();
                    break;
                case 4:
                    exporter1.WriteCsvToResponse();
                    break;
            }
        }

        #endregion
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
                DataTable dtFinYear = objSlaesActivitiesBL.GetFinacialYearBasedQouteDate(finyear);
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
        //public void PopulateCustomerDetail()
        //{
        //    if (Session["CustomerDetail"] == null)
        //    {
        //        DataTable dtCustomer = new DataTable();
        //        dtCustomer = objSlaesActivitiesBL.PopulateCustomerDetail();

        //        if (dtCustomer != null && dtCustomer.Rows.Count > 0)
        //        {
        //            lookup_Customer.DataSource = dtCustomer;
        //            lookup_Customer.DataBind();

        //            if (!string.IsNullOrEmpty(Request.QueryString["key"]) && !string.IsNullOrEmpty(Request.QueryString["SalId"]))
        //            {
        //                string udfCount = oDBEngine.ExeSclar("select sls_contactlead_id from tbl_trans_sales where sls_id=" + Request.QueryString["SalId"]);
        //                if (!string.IsNullOrEmpty(udfCount))
        //                {
        //                    lookup_Customer.GridView.Selection.SelectRowByKey(udfCount);
        //                }
        //            }
        //            Session["CustomerDetail"] = dtCustomer;
        //        }
        //    }
        //    else
        //    {
        //        lookup_Customer.DataSource = (DataTable)Session["CustomerDetail"];
        //        lookup_Customer.DataBind();
        //    }

        //}

        //public void PopulateCustomerDetailForCRM()
        //{
        //    if (Session["CustomerDetail"] == null)
        //    {
        //        DataTable dtCustomer = new DataTable();
        //        dtCustomer = objSlaesActivitiesBL.PopulateCustomerDetailForCRM();

        //        if (dtCustomer != null && dtCustomer.Rows.Count > 0)
        //        {
        //            lookup_Customer.DataSource = dtCustomer;
        //            lookup_Customer.DataBind();

        //            if (!string.IsNullOrEmpty(Request.QueryString["key"]) && !string.IsNullOrEmpty(Request.QueryString["SalId"]))
        //            {
        //                string udfCount = oDBEngine.ExeSclar("select sls_contactlead_id from tbl_trans_sales where sls_id=" + Request.QueryString["SalId"]);
        //                if (!string.IsNullOrEmpty(udfCount))
        //                {
        //                    lookup_Customer.GridView.Selection.SelectRowByKey(udfCount);
        //                }
        //            }
        //            Session["CustomerDetail"] = dtCustomer;
        //        }
        //    }
        //    else
        //    {
        //        lookup_Customer.DataSource = (DataTable)Session["CustomerDetail"];
        //        lookup_Customer.DataBind();
        //    }

        //}
        public void GetAllDropDownDetailForSalesOrder(string UserBranch)
        {
            DataSet dst = new DataSet();
            string strCompanyID = Convert.ToString(Session["LastCompany"]);
            string strBranchID = Convert.ToString(Session["userbranchID"]);
            string FinYear = Convert.ToString(Session["LastFinYear"]);
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);
            dst = objSlaesActivitiesBL.GetAllDropDownDetailForSalesOrder(UserBranch);

            SlaesActivitiesBL objSlaesActivitiesBL1 = new SlaesActivitiesBL();
            DataTable Schemadt = objSlaesActivitiesBL1.GetNumberingSchema(strCompanyID, userbranchHierarchy, FinYear, "9", "Y");
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

            //ddl_SalesAgent.Items.Insert(0, new ListItem("Select", "0"));//subhabrata on 12-12-2017


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
        [WebMethod]
        public static object GetContactPersonafterBillingShipping(string Key)
        {

            DataTable dtContactPerson = new DataTable();
            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            List<ContactPerson> contactPerson = new List<ContactPerson>();
            dtContactPerson = objSlaesActivitiesBL.PopulateMultipleContactPersonOfCustomer(Key);
            if (dtContactPerson != null && dtContactPerson.Rows.Count > 0)
            {

                if (dtContactPerson != null && dtContactPerson.Rows.Count > 0)
                {
                    for (int i = 0; i < dtContactPerson.Rows.Count; i++)
                    {
                        contactPerson.Add(new ContactPerson
                        {
                            Id = Convert.ToInt32(dtContactPerson.Rows[i]["cp_id"]),
                            Name = Convert.ToString(dtContactPerson.Rows[i]["cp_name"])
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
                // Mantis Issue 24397
                //DataRow[] MultiUoMresult = dt.Select("SrlNo ='" + SLNo + "'");
                DataRow[] MultiUoMresult = dt.Select("SrlNo ='" + SLNo + "' and UpdateRow ='True'");
                // End of Mantis Issue 24397

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
        [WebMethod]
        // public static string DeleteTaxForShipPartyChange(string UniqueVal)
        public static string DeleteTaxOnSrl(string SrlNo, string uniqueId)
        {
            DataTable dt = new DataTable();
            if (HttpContext.Current.Session["SalesOrderFinalTaxRecord" + Convert.ToString(uniqueId)] != null)
            {
                dt = (DataTable)HttpContext.Current.Session["SalesOrderFinalTaxRecord" + Convert.ToString(uniqueId)];
                DataRow[] MultiUoMresult = dt.Select("SlNo='" + SrlNo + "'");

                foreach (DataRow dr in MultiUoMresult)
                {
                    dt.Rows.Remove(dr);
                }
                HttpContext.Current.Session["SalesOrderFinalTaxRecord" + Convert.ToString(uniqueId)] = dt;
            }
            return null;

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
            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();

            dtContactPerson = objSlaesActivitiesBL.PopulateMultipleContactPersonOfCustomer(InternalId);
            if (dtContactPerson != null && dtContactPerson.Rows.Count > 0)
            {
                cmbContactPerson.TextField = "cp_name";
                cmbContactPerson.ValueField = "cp_id";
                cmbContactPerson.DataSource = dtContactPerson;
                cmbContactPerson.DataBind();
                //cmbContactPerson.TextField = "contactperson";
                //cmbContactPerson.ValueField = "add_id";
                //cmbContactPerson.DataSource = dtContactPerson;
                //cmbContactPerson.DataBind();
                //foreach (DataRow dr in dtContactPerson.Rows)
                //{
                //    if (Convert.ToString(dr["Isdefault"]) == "True")
                //    {
                //        ContactPerson = Convert.ToString(dr["add_id"]);
                //        break;
                //    }
                //}
                //cmbContactPerson.SelectedItem = cmbContactPerson.Items.FindByValue(ContactPerson);

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
        protected void ddl_VatGstCst_Callback(object sender, CallbackEventArgsBase e)
        {
            string type = e.Parameter.Split('~')[0];
            PopulateGSTCSTVAT(type);
        }
        protected void PopulateContactPerson(string customerId)
        {
            PopulateContactPersonOfCustomer(customerId);
        }
        protected void PopulateGSTCSTVAT(string type)
        {
            DataTable dtGSTCSTVAT = new DataTable();
            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            if (type == "2")
            {
                dtGSTCSTVAT = objSlaesActivitiesBL.PopulateGSTCSTVAT(dt_PLSales.Date.ToString("yyyy-MM-dd"));

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







                //// Date: 30-05-2017    Author: Kallol Samanta  [START] 
                //// Details: Billing/Shipping user control integration 

                //Old Code
                //if (chkBilling.Checked)
                //{
                //    if (CmbState.Value != null)
                //    {
                //        ShippingState = CmbState.Text;
                //        ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                //    }
                //}
                //else
                //{
                //    if (CmbState1.Value != null)
                //    {
                //        ShippingState = CmbState1.Text;
                //        ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                //    }
                //}

                //New Code
                string sstateCode = "";
                if (ddl_PosGstSalesOrder.Value != null)
                {
                    if (ddl_PosGstSalesOrder.Value.ToString() == "S")
                    {
                        ShippingState = Convert.ToString(Sales_BillingShipping.GetShippingStateId());
                    }
                    else
                    {
                        ShippingState = Convert.ToString(Sales_BillingShipping.GetBillingStateId());
                    }

                }

                ShippingState = sstateCode;
                if (ShippingState.Trim() != "")
                {
                    ShippingState = ShippingState;
                }

                //// Date: 30-05-2017    Author: Kallol Samanta  [END]











                if (ShippingState.Trim() != "" && GSTIN.Trim() != "")
                {
                    if (GSTIN.Substring(0, 2) == ShippingState)
                    {
                        //Check if the state is in union territories then only UTGST will apply
                        //   Chandigarh     Andaman and Nicobar Islands    DADRA & NAGAR HAVELI    DAMAN & DIU      Lakshadweep              PONDICHERRY
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

        //// Date: 30-05-2017    Author: Kallol Samanta  [START] 
        //// Details: Billing/Shipping user control integration

        //#region  Billing and Shipping Detail

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
        //    //cmb.Items.Insert(0, new ListEditItem("Select", "0"));
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
        //    //cmb.Items.Insert(0, new ListEditItem("Select", "0"));
        //}
        //protected void FillPinCombo(ASPxComboBox cmb, int city)
        //{
        //    string[,] pin = GetPin(city);
        //    cmb.Items.Clear();

        //    for (int i = 0; i < pin.GetLength(0); i++)
        //    {
        //        cmb.Items.Add(pin[i, 1], pin[i, 0]);
        //    }

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
        //    cmb.Items.Insert(0, new ListEditItem("Select", "0"));
        //}
        //protected void cmbCity_OnCallback(object source, CallbackEventArgsBase e)
        //{
        //    if (e.Parameter != "")
        //    {
        //        FillCityCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
        //    }
        //}
        //protected void cmbState_OnCallback(object source, CallbackEventArgsBase e)
        //{
        //    if (e.Parameter != "")
        //    {
        //        FillStateCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
        //    }
        //}
        //protected void cmbState1_OnCallback(object source, CallbackEventArgsBase e)
        //{
        //    if (e.Parameter != "")
        //    {
        //        FillStateCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
        //    }
        //}
        //protected void cmbCity1_OnCallback(object source, CallbackEventArgsBase e)
        //{
        //    if (e.Parameter != "")
        //    {
        //        FillCityCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
        //    }
        //}
        //protected void cmbArea_OnCallback(object source, CallbackEventArgsBase e)
        //{
        //    if (e.Parameter != "")
        //    {
        //        FillAreaCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
        //    }
        //}
        //protected void cmbArea1_OnCallback(object source, CallbackEventArgsBase e)
        //{
        //    if (e.Parameter != "")
        //    {
        //        FillAreaCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
        //    }
        //}
        //protected void cmbPin_OnCallback(object source, CallbackEventArgsBase e)
        //{
        //    if (e.Parameter != "")
        //    {
        //        FillPinCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
        //    }
        //}
        //protected void cmbPin1_OnCallback(object source, CallbackEventArgsBase e)
        //{
        //    if (e.Parameter != "")
        //    {
        //        FillPinCombo(source as ASPxComboBox, Convert.ToInt32(e.Parameter));
        //    }
        //}

        //#endregion

        //// Date: 30-05-2017    Author: Kallol Samanta  [END] 

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
            public string ProjectRemarks { get; set; }
            public int TaxAmountType { get; set; }

            // Mantis Issue 24425, 24428
            public string Order_AltQuantity { get; set; }
            public string Order_AltUOM { get; set; }
            // End of Mantis Issue 24425, 24428

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
            // Mantis Issue 24913
            //proc.AddVarcharPara("@Action", 500, "MultiUOMQuotationDetails");
            proc.AddVarcharPara("@Action", 500, "MultiUOMQuotationDetails_New");
            // End of Mantis Issue 24913
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
        public IEnumerable GetTaxes()
        {
            List<Taxes> TaxList = new List<Taxes>();
            // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();


            DataTable DT = GetTaxData(dt_PLSales.Date.ToString("yyyy-MM-dd"));
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
                Orders.ProjectRemarks = Convert.ToString(Orderdt.Rows[i]["ProjectRemarks"]);
                Orders.QuoteDetails_Id = Convert.ToString(Orderdt.Rows[i]["QuoteDetails_Id"]);
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
                Orders.ProjectRemarks = Convert.ToString(SalesOrderdt.Rows[i]["ProjectRemarks"]);
                Orders.QuoteDetails_Id = Convert.ToString(SalesOrderdt.Rows[i]["QuoteDetails_Id"]);
                // Mantis Issue 24425, 24428
                Orders.Order_AltQuantity = Convert.ToString(SalesOrderdt.Rows[i]["Order_AltQuantity"]);
                Orders.Order_AltUOM = Convert.ToString(SalesOrderdt.Rows[i]["Order_AltUOM"]);
                // End of Mantis Issue 24425, 24428
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
                Orders.ProjectRemarks = Convert.ToString(SalesOrderdt1.Rows[i]["ProjectRemarks"]);
                Orders.QuoteDetails_Id = Convert.ToString(SalesOrderdt1.Rows[i]["QuoteDetails_Id"]);

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
            SalesChalladt.Columns.Add("ProjectRemarks", typeof(string));
            SalesChalladt.Columns.Add("QuoteDetails_Id", typeof(string));
            //Rev  Manis 24428
            SalesChalladt.Columns.Add("Order_AltQuantity", typeof(string));
            SalesChalladt.Columns.Add("Order_AltUOM", typeof(string));
            //End Rev  Manis 24428

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IsSuccess = true;
                DataColumnCollection dtC = dt.Columns;
                string SalePrice, Discount, Amount, TaxAmount, TotalAmount, Order_Num1, ProductName, ProjectRemarks, QuoteDetails_Id;
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

                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["ProjectRemarks"])))
                {
                    ProjectRemarks = Convert.ToString(dt.Rows[i]["ProjectRemarks"]);
                }
                else
                {
                    ProjectRemarks = "";
                }


                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["QuoteDetails_Id"])))
                {
                    QuoteDetails_Id = Convert.ToString(dt.Rows[i]["QuoteDetails_Id"]);
                }
                else
                {
                    QuoteDetails_Id = "";
                }
                //Rev  Manis 24428
                string Order_AltQuantity = Convert.ToString(dt.Rows[i]["QuoteDetails__AltQuantity"]);
                string Order_AltUOM = Convert.ToString(dt.Rows[i]["QuoteDetails_AltUOM"]);
                //End Rev  Manis 24428
                SalesChalladt.Rows.Add(Convert.ToString(i + 1), Convert.ToString(i + 1), Convert.ToString(dt.Rows[i]["QuoteDetails_ProductId"]), Convert.ToString(dt.Rows[i]["QuoteDetails_ProductDescription"]),
                    Convert.ToString(dt.Rows[i]["QuoteDetails_Quantity"]), Convert.ToString(dt.Rows[i]["UOM_ShortName"]), "", Convert.ToString(dt.Rows[i]["QuoteDetails_StockQty"]), Convert.ToString(dt.Rows[i]["UOM_ShortName"]),
                               SalePrice, Discount, Amount, TaxAmount, TotalAmount, "U", Convert.ToInt64(dt.Rows[i]["Quotation_No"]), Order_Num1, ProductName, ProjectRemarks, QuoteDetails_Id, Order_AltQuantity, Order_AltUOM);
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
            // Mantis Issue 24913
            //proc.AddVarcharPara("@Action", 500, "OrderDetails");
            proc.AddVarcharPara("@Action", 500, "OrderDetails_New");
            // End of Mantis Issue 24913
            proc.AddVarcharPara("@Order_Id", 500, Convert.ToString(Session["OrderID"]));
            ds = proc.GetDataSet();
            return ds;
        }
        protected void Page_Init(object sender, EventArgs e)
        {


            SqlCurrency.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            UomSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            AltUomSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
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

            //else if (e.Column.FieldName == "Amount")
            //{
            //    e.Editor.Enabled = true;
            //}
            else if (e.Column.FieldName == "TaxAmount")
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
            else if (e.Column.FieldName == "ProductName")
            {
                e.Editor.Enabled = true;
            }
                // Mantis Issue 24425, 24428
            else if (e.Column.FieldName == "Order_AltQuantity")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "Order_AltUOM")
            {
                e.Editor.Enabled = true;
            }
            else if (hddnMultiUOMSelection.Value == "1" && e.Column.FieldName == "Quantity")
            {
                e.Editor.Enabled = true;
            }
            // End of Mantis Issue 24425, 24428
            // Mantis Issue 25377
            else if (hddnMultiUOMSelection.Value == "1" && e.Column.FieldName == "SalePrice" )
            {
                e.Editor.Enabled = true;
            }
            else if (hddnMultiUOMSelection.Value == "1" && e.Column.FieldName == "Amount")
            {
                e.Editor.Enabled = true;
            }
            // End of Mantis Issue 25377
            else
            {
                e.Editor.ReadOnly = false;
            }
        }
        protected void grid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            DataTable SalesOrderdt = new DataTable();
            string IsDeleteFrom = Convert.ToString(hdfIsDelete.Value);
            DataTable dtTemp = new DataTable();
            DataTable dtGlobal = new DataTable();

            DataTable AdditionalDetails = new DataTable();
            AdditionalDetails = (DataTable)Session["InlineRemarks"];
            grid.JSProperties["cpProductSrlIDCheck"] = "";
            grid.JSProperties["cpSalesOrderNo"] = "";
            if (Convert.ToString(Session["ActionType"]) == "Add")
            {
                if (!reCat.isAllMandetoryDone((DataTable)Session["UdfDataOnAdd"], "SO"))
                {
                    grid.JSProperties["cpSaveSuccessOrFail"] = "udfNotSaved";
                    return;
                }
            }
            if (Session["OrderDetails"] != null)
            {   
                DataTable dt = new DataTable();
                dtTemp = (DataTable)Session["OrderDetails"];
                dt = dtTemp.Copy();
                foreach (DataRow row in dt.Rows)
                {
                    DataColumnCollection dtC = dt.Columns;
                    if (dtC.Contains("Quotation"))
                    { dt.Columns.Remove("Quotation"); }
                    break;
                }//End              

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
                SalesOrderdt.Columns.Add("ProjectRemarks", typeof(string));
                SalesOrderdt.Columns.Add("QuoteDetails_Id", typeof(string));
                // Mantis Issue 24425, 24428
                SalesOrderdt.Columns.Add("Order_AltQuantity", typeof(string));
                SalesOrderdt.Columns.Add("Order_AltUOM", typeof(string));
                // End of Mantis Issue 24425, 24428
            }
            int InitVal = 1;
            foreach (var args in e.InsertValues)
            {
                string ProductDetails = Convert.ToString(args.NewValues["ProductID"]);
                if (ProductDetails != "" && ProductDetails != "0")
                {
                    string SrlNo = Convert.ToString(args.NewValues["SrlNo"]);
                    string[] ProductDetailsList = ProductDetails.Split(new string[] { "||@||" }, StringSplitOptions.None);
                    string ProductID = ProductDetailsList[0];
                    string ProductName = Convert.ToString(args.NewValues["ProductName"]);
                    string Description = Convert.ToString(args.NewValues["Description"]);
                    string Quantity = Convert.ToString(args.NewValues["Quantity"]);
                    string UOM = Convert.ToString(args.NewValues["UOM"]);
                    string Warehouse = Convert.ToString(args.NewValues["Warehouse"]);
                    decimal strMultiplier = Convert.ToDecimal(ProductDetailsList[7]);
                    string StockQuantity = Convert.ToString(Convert.ToDecimal(Quantity) * strMultiplier);
                    string StockUOM = Convert.ToString(ProductDetailsList[4]);
                    //string StockQuantity = Convert.ToString(args.NewValues["StockQuantity"]);
                    //string StockUOM = Convert.ToString(args.NewValues["StockUOM"]); 
                    // Mantis Issue 24425, 24428
                    string Order_AltQuantity = Convert.ToString(args.NewValues["Order_AltQuantity"]);
                    string Order_AltUOM = Convert.ToString(args.NewValues["Order_AltUOM"]);
                    // End of Mantis Issue 24425, 24428

                    string SalePrice = Convert.ToString(args.NewValues["SalePrice"]);
                    string Discount = Convert.ToString(args.NewValues["Discount"]);
                    string Amount = (Convert.ToString(args.NewValues["Amount"]) != "") ? Convert.ToString(args.NewValues["Amount"]).Replace(",", string.Empty).Trim() : "0";
                    string TaxAmount = (Convert.ToString(args.NewValues["TaxAmount"]) != "") ? Convert.ToString(args.NewValues["TaxAmount"]) : "0";
                    string TotalAmount = (Convert.ToString(args.NewValues["TotalAmount"]) != "") ? Convert.ToString(args.NewValues["TotalAmount"]).Replace(",", string.Empty).Trim() : "0";
                    string QuotationNumber = (Convert.ToString(args.NewValues["Quotation_Number"]) != "") ? Convert.ToString(args.NewValues["Quotation_Number"]) : "0";
                    //string Quotation = Convert.ToString(args.NewValues["Quotation_Num"]);//Added By:Subhabrata on 21-02-2017
                    string QuoteDetails_Id = (Convert.ToString(args.NewValues["QuoteDetails_Id"]) != "") ? Convert.ToString(args.NewValues["QuoteDetails_Id"]) : "0";
                    string ProjectRemarks = Convert.ToString(args.NewValues["ProjectRemarks"]);
                    // Mantis Issue 24425, 24428
                    //SalesOrderdt.Rows.Add(SrlNo, Convert.ToString(InitVal), ProductDetails, Description, Quantity, UOM, Warehouse, StockQuantity, StockUOM, SalePrice, Discount, Amount, TaxAmount, TotalAmount, "I", QuotationNumber, ProductName, ProjectRemarks, QuoteDetails_Id);
                    SalesOrderdt.Rows.Add(SrlNo, Convert.ToString(InitVal), ProductDetails, Description, Quantity, UOM, Warehouse, StockQuantity, StockUOM, SalePrice, Discount, Amount, TaxAmount, TotalAmount, "I", QuotationNumber, ProductName, ProjectRemarks, QuoteDetails_Id, Order_AltQuantity, Order_AltUOM);
                    // End of Mantis Issue 24425, 24428
                    InitVal = InitVal + 1;
                }
            }

            foreach (var args in e.UpdateValues)
            {
                string SrlNo = Convert.ToString(args.NewValues["SrlNo"]);
                string OrderID = Convert.ToString(args.Keys["OrderID"]);
                Session["OrderID"] = OrderID;
                string ProductDetails = Convert.ToString(args.NewValues["ProductID"]);


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
                        string Description = Convert.ToString(args.NewValues["Description"]);
                        string Quantity = Convert.ToString(args.NewValues["Quantity"]);
                        string UOM = Convert.ToString(args.NewValues["UOM"]);
                        string Warehouse = Convert.ToString(args.NewValues["Warehouse"]);
                        decimal strMultiplier = Convert.ToDecimal(ProductDetailsList[7]);
                        string StockQuantity = Convert.ToString(Convert.ToDecimal(Quantity) * strMultiplier);
                        string StockUOM = Convert.ToString(ProductDetailsList[4]);                      
                        // Mantis Issue 24425, 24428
                        string Order_AltQuantity = Convert.ToString(args.NewValues["Order_AltQuantity"]);
                        string Order_AltUOM = Convert.ToString(args.NewValues["Order_AltUOM"]);
                        // End of Mantis Issue 24425, 24428
                        string SalePrice = (Convert.ToString(args.NewValues["SalePrice"]) != "") ? Convert.ToString(args.NewValues["SalePrice"]) : "0";
                        string Discount = (Convert.ToString(args.NewValues["Discount"]) != "") ? Convert.ToString(args.NewValues["Discount"]) : "0";
                        string Amount = (Convert.ToString(args.NewValues["Amount"]) != "") ? Convert.ToString(args.NewValues["Amount"]).Replace(",", string.Empty).Trim() : "0";
                        string TaxAmount = (Convert.ToString(args.NewValues["TaxAmount"]) != "") ? Convert.ToString(args.NewValues["TaxAmount"]) : "0";
                        string TotalAmount = (Convert.ToString(args.NewValues["TotalAmount"]) != "") ? Convert.ToString(args.NewValues["TotalAmount"]).Replace(",", string.Empty).Trim() : "0";
                        string QuotationNumber = (Convert.ToString(args.NewValues["Quotation_No"]) != "") ? Convert.ToString(args.NewValues["Quotation_No"]) : "0";
                        //string Quotation = Convert.ToString(args.NewValues["Quotation_Num"]);//Added By:Subhabrata on 21-02-2017
                        string QuoteDetails_Id = (Convert.ToString(args.NewValues["QuoteDetails_Id"]) != "") ? Convert.ToString(args.NewValues["QuoteDetails_Id"]) : "0";
                        string ProjectRemarks = Convert.ToString(args.NewValues["ProjectRemarks"]);

                        bool Isexists = false;
                        foreach (DataRow drr in SalesOrderdt.Rows)
                        {
                            string OldOrderID = Convert.ToString(drr["OrderID"]);

                            if (OldOrderID == OrderID)
                            {
                                Isexists = true;

                                drr["ProductID"] = ProductDetails;
                                drr["Description"] = Description;
                                drr["Quantity"] = Quantity;
                                drr["UOM"] = UOM;
                                drr["Warehouse"] = Warehouse;
                                drr["StockQuantity"] = StockQuantity;
                                drr["StockUOM"] = StockUOM;
                                drr["SalePrice"] = SalePrice;
                                drr["Discount"] = Discount;
                                drr["Amount"] = Amount.Replace(",", string.Empty).Trim();
                                drr["TaxAmount"] = TaxAmount;
                                drr["TotalAmount"] = TotalAmount.Replace(",", string.Empty).Trim();
                                drr["Status"] = "U";
                                if (!string.IsNullOrEmpty(QuotationNumber))
                                { drr["Quotation_No"] = QuotationNumber; }
                                drr["ProductName"] = ProductName;
                                drr["ProjectRemarks"] = ProjectRemarks;                                
                                drr["QuoteDetails_Id"] = QuoteDetails_Id;
                                // Mantis Issue 24425, 24428
                                drr["Order_AltQuantity"] = Order_AltQuantity;
                                drr["Order_AltUOM"] = Order_AltUOM;
                                // End of Mantis Issue 24425, 24428
                                break;
                            }
                        }

                        if (Isexists == false)
                        {
                            // Mantis Issue 24425, 24428
                            //SalesOrderdt.Rows.Add(SrlNo, OrderID, ProductDetails, Description, Quantity, UOM, Warehouse, StockQuantity, StockUOM, SalePrice, Discount, Amount, TaxAmount, TotalAmount, "U", QuotationNumber, ProductName, ProjectRemarks, QuoteDetails_Id);
                            SalesOrderdt.Rows.Add(SrlNo, OrderID, ProductDetails, Description, Quantity, UOM, Warehouse, StockQuantity, StockUOM, SalePrice, Discount, Amount, TaxAmount, TotalAmount, "U", QuotationNumber, ProductName, ProjectRemarks, QuoteDetails_Id, Order_AltQuantity, Order_AltUOM);
                            // End of Mantis Issue 24425, 24428
                            //}
                            //SalesOrderdt.Rows.Add(SrlNo, OrderID, ProductDetails, Description, Quotation, Quantity, UOM, Warehouse, StockQuantity, StockUOM, SalePrice, Discount, Amount, TaxAmount, TotalAmount, "U", QuotationNumber);
                        }
                    }
                }
            }

            foreach (var args in e.DeleteValues)
            {
                string OrderID = Convert.ToString(args.Keys["OrderID"]);
                string SrlNo = "";

                for (int i = AdditionalDetails.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = SalesOrderdt.Rows[i];
                    string delQuotationID = Convert.ToString(dr["OrderID"]);
                    DataRow daddr = AdditionalDetails.Rows[i];
                    //chinmoy add for addrprojectDesc
                    string delSrlNo = Convert.ToString(daddr["SrlNo"]);
                    if (delQuotationID == OrderID)
                        daddr.Delete();
                    // dr.Delete();
                }
                AdditionalDetails.AcceptChanges();


                for (int i = SalesOrderdt.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = SalesOrderdt.Rows[i];
                    string delQuotationID = Convert.ToString(dr["OrderID"]);
                    if (delQuotationID == OrderID)
                    {
                        SrlNo = Convert.ToString(dr["SrlNo"]);
                        dr.Delete();
                    }

                }
                SalesOrderdt.AcceptChanges();

               // DeleteTaxDetails(SrlNo);
              //  DeleteWarehouse(SrlNo);

                if (OrderID.Contains("~") != true)
                {
                    SalesOrderdt.Rows.Add("0", OrderID, "0", "", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "D", "0", "", "0");
                }
            }
                      

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
           
            Session["OrderDetails"] = SalesOrderdt;
            Session["InlineRemarks"] = AdditionalDetails;
           

            if (IsDeleteFrom != "D")
            {
                try
                {
                    string ActionType = Convert.ToString(Session["ActionType"]);
                    string MainOrderID = string.Empty;
                    if (Convert.ToString(Request.QueryString["key"]) != null && Convert.ToString(Request.QueryString["key"]) != "")
                    {
                        Session["OrderID"] = Convert.ToString(Request.QueryString["key"]);
                        if (!string.IsNullOrEmpty(Convert.ToString(Session["OrderID"])))
                        {

                            MainOrderID = Convert.ToString(Session["OrderID"]);
                        }
                        else
                        {
                            MainOrderID = "";
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(Session["OrderID"])))
                        {

                            MainOrderID = Convert.ToString(Session["OrderID"]);
                        }
                        else
                        {
                            MainOrderID = "";
                        }
                    }
                    //Credit Days
                    int IsInvenotry;
                    if (ddlInventory.SelectedValue.ToUpper() == "Y")
                    {
                        IsInvenotry = 1;
                    }
                    else
                    {
                        IsInvenotry = 0;
                    }
                    string creditdays = txtCreditDays.Text;
                    string strDueDate = Convert.ToString(dt_SaleInvoiceDue.Date);
                    //End
                    string strSchemeType = Convert.ToString(ddl_numberingScheme.SelectedValue);
                    string strQuoteNo = Convert.ToString(txt_SlOrderNo.Text);
                    string strQuoteDate = Convert.ToString(dt_PLSales.Date);
                    string strQuoteExpiry = Convert.ToString(dt_PlOrderExpiry.Date);
                    string strCustomer = Convert.ToString(hdfLookupCustomer.Value);
                    string strContactName = Convert.ToString(cmbContactPerson.Value);
                    string Reference = Convert.ToString(txt_Refference.Text);
                    string strBranch = Convert.ToString(ddl_Branch.SelectedValue);

                    string RevisionNo = Convert.ToString(txtRevisionNo.Text);
                    string RevisionDate = "";

                    if (hdnApprovalReqInq.Value == "1")
                    {
                        RevisionDate = Convert.ToString(txtRevisionDate.Text);
                    }
                    string projectValidFrom = Convert.ToString(dtProjValidFrom.Text);
                    string projectValidUpto = Convert.ToString(dtProjValidUpto.Text);
                    string AppRejRemarks = Convert.ToString(txtAppRejRemarks.Text);
                    // Rev 1.0
                    //string strQtyTolerance = Convert.ToString(txtQtyTolerance.Text);
                    // End of Rev 1.0

                    // Mantis Issue 24913 
                    if (!string.IsNullOrEmpty(Request.QueryString["SalId"]) && string.IsNullOrEmpty(Convert.ToString(hdnSalesManAgentId.Value)) )
                    { 
                        ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
                        proc.AddVarcharPara("@Action", 500, "GetSalesmanID");
                        proc.AddVarcharPara("@sla_id", 500, Request.QueryString["SalId"].ToString());
                        DataTable dtAgnt = proc.GetTable();
                        if (dtAgnt.Rows.Count > 0)
                        {
                            hdnSalesManAgentId.Value = dtAgnt.Rows[0]["SalesManAgentId"].ToString().Trim();
                        }
                    }
                    // End of Mantis Issue 24913                   
                    string strAgents = string.Empty;//subhabrata on 03-01-2018
                    if (!string.IsNullOrEmpty(Convert.ToString(hdnSalesManAgentId.Value)))
                    {
                        strAgents = Convert.ToString(hdnSalesManAgentId.Value);
                    }
                    else
                    {
                        strAgents = "0";
                    }

                    string strCurrency = Convert.ToString(ddl_Currency.SelectedValue);
                    string PosForGst = Convert.ToString(ddl_PosGstSalesOrder.Value);
                    //string strRate = Convert.ToString(txt_Rate.Value);
                    string strTaxType = Convert.ToString(ddl_AmountAre.Value);
                    string strTaxCode = Convert.ToString(ddl_VatGstCst.Value);
                    //Added by:Subhabrata
                    string OANumber = Convert.ToString(txt_OANumber.Text);
                    string OADate = Convert.ToString(dt_OADate.Date);
                    //  string QuotationDate = Convert.ToString(dt_Quotation.Text);
                    string QuotationDate = string.Empty;
                    //Get Quotation details
                    String QuoComponent = "";
                    List<object> QuoList = lookup_quotation.GridView.GetSelectedFieldValues("Quote_Id");
                    foreach (object Quo in QuoList)
                    {
                        QuoComponent += "," + Quo;
                    }
                    QuoComponent = QuoComponent.TrimStart(',');
                    string[] eachQuo = QuoComponent.Split(',');
                    if (eachQuo.Length == 1)
                    {
                        QuotationDate = Convert.ToString(dt_Quotation.Text);                       
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
                    SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
                    DataTable dt = objSlaesActivitiesBL.GetCurrentConvertedRate(BaseCurrencyId, ConvertedCurrencyId, CompID);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        strRate = Convert.ToString(dt.Rows[0]["SalesRate"]);
                    }


                    string Segment1 = Convert.ToString(hdnSegment1.Value);
                    string Segment2 = Convert.ToString(hdnSegment2.Value);
                    string Segment3 = Convert.ToString(hdnSegment3.Value);
                    string Segment4 = Convert.ToString(hdnSegment4.Value);
                    string Segment5 = Convert.ToString(hdnSegment5.Value);
                    // Add tempSalesOrderdt By Sudip

                    DataTable tempSalesOrderdt = SalesOrderdt.Copy();
                   
                    //DataTable tempSalesOrderdt = (DataTable)Session["OrderDetails"];
                    int InitValR = 1;
                    foreach (DataRow dr in tempSalesOrderdt.Rows)
                    {

                        string Status = Convert.ToString(dr["Status"]);


                        if (Status == "I")
                        {
                            //dr["OrderID"] = "0";

                            if (Convert.ToString(dr["OrderID"]).Contains("~") == true)
                            {
                                dr["OrderID"] = "0";
                            }
                            if (Convert.ToString(dr["OrderID"]) == "0")
                            { dr["OrderID"] = Convert.ToString(InitValR); }

                            string[] list = Convert.ToString(dr["ProductID"]).Split(new string[] { "||@||" }, StringSplitOptions.None);
                            dr["ProductID"] = Convert.ToString(list[0]);
                            dr["UOM"] = Convert.ToString(list[3]);
                            dr["StockUOM"] = Convert.ToString(list[5]);
                        }
                        else if (Status == "U" || Status == "")
                        {
                            if (Convert.ToString(dr["OrderID"]).Contains("~") == true)
                            {
                                dr["OrderID"] = "0";
                            }
                            if (dr["OrderID"] == "0")
                            { dr["OrderID"] = Convert.ToString(InitValR); }

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
                                    dtUOMs = objSlaesActivitiesBL.GetQuotationDetailsUOMInfo(UOM, stockUOM);
                                    dr["UOM"] = Convert.ToString(dtUOMs.Tables[0].Rows[0].Field<Int64>("UOM_ID"));
                                    dr["StockUOM"] = Convert.ToString(dtUOMs.Tables[1].Rows[0].Field<Int64>("UOM_ID"));
                                }

                            }
                        }
                        InitValR = InitValR + 1;
                    }
                    tempSalesOrderdt.AcceptChanges();


                    DataTable TaxDetailTable = new DataTable();
                    if (Session["SalesOrderFinalTaxRecord" + Convert.ToString(uniqueId.Value)] != null)
                    {
                        TaxDetailTable = (DataTable)Session["SalesOrderFinalTaxRecord" + Convert.ToString(uniqueId.Value)];
                    }

                    string validate = string.Empty;
                    // Datattable of Warehouse

                    DataTable tempWarehousedt = new DataTable();
                    if (Session["SalesWarehouseData"] != null)
                    {
                        DataTable Warehousedt = (DataTable)Session["SalesWarehouseData"];
                        tempWarehousedt = Warehousedt.DefaultView.ToTable(false, "Product_SrlNo", "LoopID", "WarehouseID", "TotalQuantity", "BatchID", "SerialID");
                    }
                    else
                    {
                        tempWarehousedt.Columns.Add("Product_SrlNo", typeof(string));
                        tempWarehousedt.Columns.Add("SrlNo", typeof(string));
                        tempWarehousedt.Columns.Add("WarehouseID", typeof(string));
                        tempWarehousedt.Columns.Add("TotalQuantity", typeof(string));
                        tempWarehousedt.Columns.Add("BatchID", typeof(string));
                        tempWarehousedt.Columns.Add("SerialID", typeof(string));
                    }

                    //End

                    //datatable for MultiUOm start chinmoy 14-01-2020
                    DataTable MultiUOMDetails = new DataTable();

                    if (Session["MultiUOMData"] != null)
                    {
                        DataTable MultiUOM = (DataTable)Session["MultiUOMData"];
                        // Mantis Issue 24397
                        //MultiUOMDetails = MultiUOM.DefaultView.ToTable(false, "SrlNo", "Quantity", "UOM", "AltUOM", "AltQuantity", "UomId", "AltUomId", "ProductId");
                        MultiUOMDetails = MultiUOM.DefaultView.ToTable(false, "SrlNo", "Quantity", "UOM", "AltUOM", "AltQuantity", "UomId", "AltUomId", "ProductId","DetailsId", "BaseRate","AltRate","UpdateRow");
                        // End of Mantis Issue 24397
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
                        // Mantis Issue 24397
                        MultiUOMDetails.Columns.Add("DetailsId", typeof(string));
                        MultiUOMDetails.Columns.Add("BaseRate", typeof(Decimal));
                        MultiUOMDetails.Columns.Add("AltRate", typeof(Decimal));
                        MultiUOMDetails.Columns.Add("UpdateRow", typeof(string));
                        // End of Mantis Issue 24397
                    }


                    //End



                    // DataTable Of Quotation Tax Details 

                    DataTable TaxDetailsdt = new DataTable();
                    if (Session["STaxDetails"] != null)
                    {
                        TaxDetailsdt = (DataTable)Session["STaxDetails"];
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
                    // tempTaxDetailsdt.Columns.Add("AltTaxCode", typeof(string));
                    tempTaxDetailsdt.Columns["SlNo"].SetOrdinal(0);
                    tempTaxDetailsdt.Columns["Taxes_ID"].SetOrdinal(1);
                    tempTaxDetailsdt.Columns["AltTax_Code"].SetOrdinal(2);
                    tempTaxDetailsdt.Columns["Percentage"].SetOrdinal(3);
                    tempTaxDetailsdt.Columns["Amount"].SetOrdinal(4);

                    foreach (DataRow d in tempTaxDetailsdt.Rows)
                    {
                        d["SlNo"] = "0";
                        // d["AltTaxCode"] = "0";
                    }


                    // DataTable Of Billing Address

                    DataTable tempBillAddress = new DataTable();

                    //Party Order Date and Order Date compare 
                    int result = DateTime.Compare(dt_PLSales.Date, dt_OADate.Date);
                    string partyOrderdtMasg = string.Empty;
                    if (result < 0)
                    {
                        partyOrderdtMasg = "PartyOrderDateMisMatch";

                    }
                    //END
                    tempBillAddress = Sales_BillingShipping.GetBillingShippingTable();


                    #region Sandip Section For Approval Section Start
                    string approveStatus = "";
                    if (Request.QueryString["status"] != null)
                    {
                        approveStatus = Convert.ToString(Request.QueryString["status"]);
                    }
                    #endregion Sandip Section For Approval Dtl Section End

                    if (hddnMultiUOMSelection.Value == "1")
                    {
                        foreach (DataRow dr in tempSalesOrderdt.Rows)
                        {
                            string strSrlNo = Convert.ToString(dr["SrlNo"]);
                            // string strDetailsId = Convert.ToString(dr["DetailsId"]);
                            string StockUOM = Convert.ToString(dr["StockUOM"]);
                            decimal strProductQuantity = Convert.ToDecimal(dr["Quantity"]);
                            decimal strUOMQuantity = 0;
                            if (StockUOM != "0")
                            {
                                GetQuantityBaseOnProductforDetailsId(strSrlNo, ref strUOMQuantity);


                                // Rev 2.0
                                ////Rev 24428
                                //DataTable dtb = new DataTable();
                                //dtb = (DataTable)Session["MultiUOMData"];
                                ////if (Session["MultiUOMData"] != null)
                                ////{
                                //if (dtb.Rows.Count > 0)
                                //{
                                //   // Mantis Issue 24397
                                //    //if (strUOMQuantity != null)
                                //    //{
                                //    //    if (strProductQuantity != strUOMQuantity)
                                //    //    {
                                //    //        validate = "checkMultiUOMData";
                                //    //        grid.JSProperties["cpcheckMultiUOMData"] = strSrlNo;
                                //    //        break;
                                //    //    }
                                //    //}

                                //    // End of Mantis Issue 24397
                                //}
                                //else if (dtb.Rows.Count < 1)
                                //{
                                //    validate = "checkMultiUOMData";
                                //    grid.JSProperties["cpcheckMultiUOMData"] = strSrlNo;
                                //    break;
                                //}
                                ////End Rev 24428
                                ///

                                DataTable dtb = new DataTable();
                                dtb = (DataTable)Session["MultiUOMData"];

                                if (dtb.Rows.Count > 0)
                                {
                                    DataRow[] MultiUoMresult;

                                    MultiUoMresult = dtb.Select("SrlNo ='" + strSrlNo + "' and UpdateRow ='True'");

                                    if (MultiUoMresult.Length > 0)
                                    {
                                        if ((Convert.ToDecimal(MultiUoMresult[0]["Quantity"]) != Convert.ToDecimal(dr["Quantity"])) ||
                                            (Math.Round(Convert.ToDecimal(MultiUoMresult[0]["AltQuantity"]), 2) != Math.Round(Convert.ToDecimal(dr["Order_AltQuantity"]), 2)) ||
                                            (Math.Round(Convert.ToDecimal(MultiUoMresult[0]["BaseRate"]), 2) != Math.Round(Convert.ToDecimal(dr["SalePrice"]), 2))
                                            )
                                        {
                                            validate = "checkMultiUOMData_QtyMismatch";
                                            grid.JSProperties["cpcheckMultiUOMData"] = strSrlNo;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        validate = "checkMultiUOMData_NotFound";
                                        grid.JSProperties["cpcheckMultiUOMData"] = strSrlNo;
                                        break;
                                    }
                                }
                                else if (dtb.Rows.Count < 1)
                                {
                                    validate = "checkMultiUOMData";
                                    grid.JSProperties["cpcheckMultiUOMData"] = strSrlNo;
                                    break;
                                }
                                // End of Rev 2.0
                            }
                        }
                    }
                    DataTable duplicatedt = SalesOrderdt.Copy();
                    DataView dvData = new DataView(duplicatedt);
                    dvData.RowFilter = "Status <> 'D'";
                    duplicatedt = dvData.ToTable();

                   // var duplicateRecords = duplicatedt.AsEnumerable()
                   //.GroupBy(r => r["ProductID"]) //coloumn name which has the duplicate values
                   //.Where(gr => gr.Count() > 1)
                   //.Select(g => g.Key);
                    //foreach (var d in duplicateRecords)
                    //{
                    //    validate = "duplicateProduct";
                    //}

                    foreach (DataRow dr in duplicatedt.Rows)
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

                    if (hdAddOrEdit.Value == "Add")
                    {

                        if (lookup_quotation.GridView.GetSelectedFieldValues("Quote_Id").Count > 0)
                        {
                            if (tempSalesOrderdt.Rows.Count > 0)
                            {
                                foreach (DataRow dr in tempSalesOrderdt.Rows)
                                {
                                    Int64 DetailsId = 0;
                                    string ProductID = Convert.ToString(dr["ProductID"]);
                                    decimal ProductQuantity = Convert.ToDecimal(dr["Quantity"]);
                                    string Status = Convert.ToString(dr["Status"]);
                                    //QuoteOrderDetails_id = Convert.ToString(Session["QuoteOrderDetails_id"]);
                                    //Int64 i = 0;
                                    //for ( i =(Convert.ToInt64(dr["SrlNo"])-1); i <Convert.ToInt64(dr["SrlNo"]); i++)
                                    //{
                                    //     DetailsId = Convert.ToInt64(QuoteOrderDetails_id.Split(',')[i]);
                                    //}
                                    if (rdl_Salesquotation.SelectedValue == "QN")
                                    {
                                        DataTable dtq = oDBEngine.GetDataTable("select isnull(BalanceQty,0) TotQty from tbl_trans_SalesBalanceMap where  SalesDocId='" + Convert.ToInt32(dr["Quotation_No"]) + "' and  ProductId='" + ProductID + "'");

                                        if (ProductID != "" && dtq.Rows.Count > 0)
                                        {
                                            if (ProductQuantity > Convert.ToDecimal(dtq.Rows[0]["TotQty"]))
                                            {
                                                validate = "ExceedQuantity";
                                                break;
                                            }
                                        }
                                    }
                                    else if (rdl_Salesquotation.SelectedValue == "SINQ")
                                    {
                                        DataTable dtq = oDBEngine.GetDataTable("select isnull(BalanceQty,0) TotQty from tbl_trans_SalesInquiryBalanceMap where  SalesInquirysDocId='" + Convert.ToInt32(dr["Quotation_No"]) + "' and  ProductId='" + ProductID + "'");

                                        if (ProductID != "" && dtq.Rows.Count > 0)
                                        {
                                            if (ProductQuantity > Convert.ToDecimal(dtq.Rows[0]["TotQty"]))
                                            {
                                                validate = "ExceedQuantity";
                                                break;
                                            }
                                        }
                                    }

                                }

                            }
                        }
                    }



                    if ((hdAddOrEdit.Value == "Edit") && (Convert.ToString(Session["Lookup_QuotationIds"]) != ""))
                    {


                        if (tempSalesOrderdt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in tempSalesOrderdt.Rows)
                            {
                                string ProductID = Convert.ToString(dr["ProductID"]);
                                decimal ProductQuantity = Convert.ToDecimal(dr["Quantity"]);
                                string Status = Convert.ToString(dr["Status"]);

                                if (rdl_Salesquotation.SelectedValue == "QN")
                                {
                                    DataTable dtq = oDBEngine.GetDataTable("select (ISNULL(TotalQty,0)+isnull(BalanceQty,0)) TotQty from tbl_trans_SalesBalanceMap where  SalesDocId='" + Convert.ToInt32(dr["Quotation_No"]) + "' and ProductId='" + ProductID + "'");

                                    if (ProductID != "" && dtq.Rows.Count > 0)
                                    {
                                        if (ProductQuantity > Convert.ToDecimal(dtq.Rows[0]["TotQty"]))
                                        {
                                            validate = "ExceedQuantity";
                                            break;
                                        }
                                    }
                                }



                            }

                        }
                    }

                    if (hdAddOrEdit.Value == "Edit" && hdnApprovalReqInq.Value == "1")
                    {


                        if (tempSalesOrderdt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in tempSalesOrderdt.Rows)
                            {
                                string ProductID = Convert.ToString(dr["ProductID"]);
                                decimal ProductQuantity = Convert.ToDecimal(dr["Quantity"]);
                                string Status = Convert.ToString(dr["Status"]);

                                DataTable dtProductqty = oDBEngine.GetDataTable("select top 1 InvoiceDetails_Quantity from tbl_trans_SalesInvoiceProducts inner join tbl_trans_SalesInvoice on Invoice_Id=InvoiceDetails_OrderId  where  DocDetailsId='" + Convert.ToInt32(dr["OrderID"]) + "'");

                                if (ProductID != "" && dtProductqty.Rows.Count > 0 && dtProductqty != null)
                                {
                                    if (ProductQuantity < Convert.ToDecimal(dtProductqty.Rows[0]["InvoiceDetails_Quantity"]))
                                    {
                                        validate = "OverRatedTaggedQuantity";
                                        break;
                                    }
                                }

                            }

                        }
                    }

                    foreach (DataRow dr in tempSalesOrderdt.Rows)
                    {
                        string strSrlNo = Convert.ToString(dr["SrlNo"]);
                        decimal strProductQuantity = Convert.ToDecimal(dr["Quantity"]);
                        decimal strWarehouseQuantity = 0;
                        GetQuantityBaseOnProduct(strSrlNo, ref strWarehouseQuantity);
                        if (strWarehouseQuantity != 0)
                        {
                            if (strProductQuantity != strWarehouseQuantity)
                            {
                                validate = "checkWarehouse";
                                grid.JSProperties["cpProductSrlIDCheck"] = strSrlNo;
                                break;
                            }
                        }
                    }
                    //Check Whether there is Available Stock or Not
                    //foreach (DataRow dr in SalesOrderdt.Rows)
                    //{
                    //    string[] list = Convert.ToString(dr["ProductID"]).Split(new string[] { "||@||" }, StringSplitOptions.None);
                    //    string ProductId = Convert.ToString(list[0]);
                    //    DataTable dt2 = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableQuotation(" + strBranch + ",'" + Convert.ToString(Session["LastCompany"]) + "','"
                    //        + Convert.ToString(Session["LastFinYear"]) + "','" + ProductId + "') as branchopenstock");
                    //    if (!String.IsNullOrEmpty(Convert.ToString(dt2.Rows[0]["branchopenstock"])))
                    //    {
                    //        if (Convert.ToDecimal(dt2.Rows[0]["branchopenstock"]) == (decimal)0.00)
                    //        {
                    //            grid.JSProperties["cpSaveSuccessOrFailOnAvaiableStock"] = "IsAvailableStock";
                    //        }
                    //    }
                    //}


                    foreach (DataRow dr in SalesOrderdt.Rows)
                    {
                        if (Convert.ToDecimal(dr["Quantity"]) == (decimal)0.00)
                        {
                            grid.JSProperties["cpQuantityOrAmount"] = "gridQuantityZero";
                        }
                        else if (Convert.ToDecimal(dr["Amount"]) == (decimal)0.00)
                        {
                            grid.JSProperties["cpQuantityOrAmount"] = "gridAmountZero";
                        }
                    }
                    //End
                    if (Convert.ToString(Sales_BillingShipping.GeteShippingStateCode()) == "0")
                    {
                        validate = "BillingShippingNull";
                    }
                    if (Convert.ToString(Sales_BillingShipping.GetBillingStateCode()) == "0")
                    {
                        validate = "BillingShippingNull";
                    }
                    SlaesActivitiesBL objSlaesActivitiesBL1 = new SlaesActivitiesBL();
                    //DataTable dtCredidays = objSlaesActivitiesBL1.GetConfigSettingsOnCreditDaysBasis("SalesManAgentsMandatory,CreditdaysExist,StockCheckInSalesOrder");
                    if (strAgents == "0")
                    {
                        //SlaesActivitiesBL objSlaesActivitiesBL1 = new SlaesActivitiesBL();
                        DataTable dtCredidays = objSlaesActivitiesBL1.GetConfigSettingsOnCreditDaysBasis("SalesManAgentsMandatory");
                        string Variable_Val = "";
                        string TempVar = "";

                        if (dtCredidays != null && dtCredidays.Rows.Count > 0)
                        {
                            TempVar = Convert.ToString(dtCredidays.Rows[0]["Variable_Value"]);
                            if (TempVar.ToUpper() == "YES")
                            {
                                validate = "SalesManAgentMandatoryCheck";
                            }
                        }
                    }
                    if (Convert.ToInt32(creditdays) == 0)
                    {
                        //SlaesActivitiesBL objSlaesActivitiesBL1 = new SlaesActivitiesBL();
                        DataTable dtCredidays = objSlaesActivitiesBL1.GetConfigSettingsOnCreditDaysBasis("CreditdaysExist");
                        string Variable_Val = "";
                        string TempVar = "";

                        if (dtCredidays != null && dtCredidays.Rows.Count > 0)
                        {
                            TempVar = Convert.ToString(dtCredidays.Rows[0]["Variable_Value"]);
                            if (TempVar.ToUpper() == "YES")
                            {
                                validate = "CrediDaysZero";
                            }
                        }
                    }
                    string StockCheckMsg = string.Empty;
                    DataTable dtStockCheck = objSlaesActivitiesBL1.GetConfigSettingsOnCreditDaysBasis("StockCheckInSalesOrder");
                    if (dtStockCheck != null && dtStockCheck.Rows.Count > 0)
                    {
                        if (Convert.ToString(dtStockCheck.Rows[0]["Variable_Value"]).ToUpper() == "YES")
                        {
                            StockCheckMsg = objBL.GetAvailableStockCheckForSalesOrder(tempSalesOrderdt, Convert.ToString(ddl_Branch.SelectedValue),
                                Convert.ToString(strQuoteDate));
                            if (!string.IsNullOrEmpty(StockCheckMsg))
                            {
                                validate = StockCheckMsg;
                            }
                        }
                    }



                    //Subhabrata Is From CRM
                    int IsFromCRM = 0;
                    if (!string.IsNullOrEmpty(Request.QueryString["SalId"]))
                    {
                        IsFromCRM = 1;
                    }
                    //END

                    #region CustomerValidation

                    if (string.IsNullOrEmpty(strCustomer))
                    {
                        validate = "BlankCustomerNotSaved";
                    }

                    #endregion

                  

                    ////// ############# Added By : Samrat Roy -- 02/05/2017 -- To check Transporter Mandatory Control 
                    #region Dorment Customer Check

                    //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                    //DataTable DT1 = objEngine.GetDataTable("tbl_master_Contact", " Statustype ", " cnt_internalId='" + strCustomer + "'");
                    //if (DT1 != null && DT1.Rows.Count > 0)
                    //{
                    //    string IsDorment = Convert.ToString(DT1.Rows[0]["Statustype"]).Trim();
                    //    if (IsDorment == "D") validate = "Dormant_Customer";
                    //}

                    #endregion

                    string TaxType = "", ShippingState = "";
                    // Edited Chinmoy Below Line


                    if (ddl_PosGstSalesOrder.Value.ToString() == "S")
                    {
                        ShippingState = Convert.ToString(Sales_BillingShipping.GetShippingStateId());
                    }
                    else
                    {
                        ShippingState = Convert.ToString(Sales_BillingShipping.GetBillingStateId());
                    }

                    if (Convert.ToString(ddl_AmountAre.Value) == "1")
                    { TaxType = "E"; }
                    else if (Convert.ToString(ddl_AmountAre.Value) == "2")
                    { TaxType = "I"; }


                    #region Delete Product Tax

                    string TaxDetails = Convert.ToString(hdnJsonProductTax.Value);
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    ser.MaxJsonLength = Int32.MaxValue;
                    List<ProductTaxDetails> TaxEntryJson = ser.Deserialize<List<ProductTaxDetails>>(TaxDetails);


                    foreach (DataRow productRow in tempSalesOrderdt.Rows)
                    {
                        string _ProductID = Convert.ToString(productRow["ProductID"]);
                        string _SrlNo = Convert.ToString(productRow["SrlNo"]);
                        string _IsEntry = "";

                        if (_ProductID != "")
                        {
                            var TaxValue = TaxEntryJson.Where(x => x.SrlNo == _SrlNo).ToList().SingleOrDefault();

                            if (TaxValue != null)
                            {
                                _IsEntry = TaxValue.IsTaxEntry;

                                if (_IsEntry == "N")
                                {
                                    DataRow[] deletedRow = TaxDetailTable.Select("SlNo=" + _SrlNo);
                                    if (deletedRow.Length > 0)
                                    {
                                        foreach (DataRow dr in deletedRow)
                                        {
                                            TaxDetailTable.Rows.Remove(dr);
                                        }
                                        TaxDetailTable.AcceptChanges();
                                    }
                                }
                            }
                        }
                    }

                    #endregion




                    //TaxDetailTable = gstTaxDetails.SetTaxTableDataWithProductSerialRoundOff(ref tempSalesOrderdt, "SrlNo", "ProductID",
                    //    "Amount", "TaxAmount", TaxDetailTable, "S", dt_PLSales.Date.ToString("yyyy-MM-dd"), strBranch, ShippingState, TaxType);


                    TaxDetailTable = gstTaxDetails.SetTaxTableDataWithProductSerialWithException(tempSalesOrderdt, "SrlNo", "ProductID", "Amount", "TaxAmount", TaxDetailTable, "S", dt_PLSales.Date.ToString("yyyy-MM-dd"), strBranch, ShippingState, TaxType, hdnCustomerId.Value, "Quantity", "SQ");

                    string sstateCode = "";
                    if (ddl_PosGstSalesOrder.Value != null)
                    {
                        if (ddl_PosGstSalesOrder.Value.ToString() == "S")
                        {
                            sstateCode = Sales_BillingShipping.GeteShippingStateCode();
                        }
                        else
                        {
                            sstateCode = Sales_BillingShipping.GetBillingStateCode();
                        }
                    }

                    CommonBL ComBL = new CommonBL();                   
                    string GSTRateTaxMasterMandatory = ComBL.GetSystemSettingsResult("GSTRateTaxMasterMandatory");

                    if (hdnTransCategory.Value != "EXWOP")
                    {
                        if (hdnTransCategory.Value != "SEZWOP")
                        {
                            if (!String.IsNullOrEmpty(GSTRateTaxMasterMandatory))
                            {
                                if (GSTRateTaxMasterMandatory == "Yes")
                                {
                                    // Rev 24425, 24428
                                    DataTable temp_SalesOrderdt = tempSalesOrderdt.Copy();
                                    if (temp_SalesOrderdt.Columns.Contains("Order_AltQuantity"))
                                    {
                                        temp_SalesOrderdt.Columns.Remove("Order_AltQuantity");
                                    }
                                    if (temp_SalesOrderdt.Columns.Contains("Order_AltUOM"))
                                    {
                                        temp_SalesOrderdt.Columns.Remove("Order_AltUOM");
                                    }
                                    // End of Rev 24425, 24428

                                    DataTable dtTaxDetails = new DataTable();
                                    ProcedureExecute procT = new ProcedureExecute("PRC_SALESORDER_DETAILS");
                                    procT.AddVarcharPara("@Action", 500, "GetTaxDetailsByProductID");
                                    // Mantis Issue 24425, 25528
                                    //procT.AddPara("@SalesOrderDetails", tempSalesOrderdt);
                                    procT.AddPara("@SalesOrderDetails", temp_SalesOrderdt);
                                    // End of Mantis Issue 24425, 25528
                                    procT.AddVarcharPara("@TaxOption", 10, Convert.ToString(strTaxType));
                                    procT.AddVarcharPara("@SupplyState", 15, Convert.ToString(sstateCode));
                                    procT.AddVarcharPara("@BRANCHID", 10, Convert.ToString(strBranch));
                                    procT.AddVarcharPara("@COMPANYID", 500, Convert.ToString(Session["LastCompany"]));
                                    procT.AddVarcharPara("@ENTITY_ID", 100, Convert.ToString(strCustomer));
                                    procT.AddVarcharPara("@DATE", 100, Convert.ToString(dt_PLSales.Date.ToString("yyyy-MM-dd")));

                                    dtTaxDetails = procT.GetTable();

                                    if (dtTaxDetails != null && dtTaxDetails.Rows.Count > 0)
                                    {

                                        foreach (DataRow dr in dtTaxDetails.Rows)
                                        {
                                            string SerialID = Convert.ToString(dr["SrlNo"]);
                                            string TaxID = Convert.ToString(dr["TaxCode"]);
                                            decimal _TaxAmount = Math.Round(Convert.ToDecimal(dr["TaxAmount"]), 2);
                                            string ProductName = Convert.ToString(dr["ProductName"]);
                                            DataRow[] rows = TaxDetailTable.Select("SlNo = '" + SerialID + "' and TaxCode='" + TaxID + "'");

                                            if (rows != null && rows.Length > 0)
                                            {
                                                //decimal EntryTaxAmount = Math.Round(Convert.ToDecimal(rows[0]["Amount"]), 2);
                                                decimal EntryTaxAmount = Math.Round(Convert.ToDecimal(rows[0]["Amount"]), 2);

                                                if (EntryTaxAmount != _TaxAmount)
                                                {
                                                    validate = "checkAcurateTaxAmount";
                                                    grid.JSProperties["cpSerialNo"] = SerialID;
                                                    grid.JSProperties["cpProductName"] = ProductName;
                                                    break;
                                                }


                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }

                    // Rev 2.0 [validate == "checkMultiUOMData_QtyMismatch", "checkMultiUOMData_NotFound" added]
                    if (validate == "outrange" || validate == "duplicate" || validate == "checkWarehouse" ||
                        validate == "duplicateProduct" ||
                        validate == "BillingShippingNull" || validate == "CrediDaysZero" || validate == "SalesManAgentMandatoryCheck" || validate == "BlankCustomerNotSaved"
                        || validate == "MoreThanStock" || validate == "ZeroStock" || validate == "Dormant_Customer" || validate == "checkMultiUOMData"
                        || validate == "ExceedQuantity" || validate == "OverRatedTaggedQuantity" || validate == "nullQuantity" 
                        || validate == "checkAcurateTaxAmount"
                        || validate == "checkMultiUOMData_QtyMismatch" || validate == "checkMultiUOMData_NotFound"
                        )
                    {
                        grid.JSProperties["cpSaveSuccessOrFail"] = validate;
                    }
                    else if (partyOrderdtMasg == "PartyOrderDateMisMatch")
                    {
                        grid.JSProperties["cpPartyOrderDate"] = partyOrderdtMasg;
                    }
                    else
                    {
                        if (tempSalesOrderdt.Rows.Count <= 0)
                        {
                            grid.JSProperties["cpProductNotExists"] = "Select Product First";
                        }
                        else
                        {
                            
                            if (TaxType == "E")
                            {
                                foreach (DataRow drAmt in tempSalesOrderdt.Rows)
                                {
                                    drAmt["TotalAmount"] = Convert.ToDecimal(drAmt["Amount"]) + Convert.ToDecimal(drAmt["TaxAmount"]);
                                }
                            }

                           

                            UniqueQuotation = txt_SlOrderNo.Text;
                            string[] SchemeList = strSchemeType.Split(new string[] { "~" }, StringSplitOptions.None);

                            //Code block comment End  


                            #region Add New Filed To Check from Table
                            DataTable duplicatedt2 = new DataTable();
                            if (Convert.ToString(hdBasketId.Value).Trim() == "")
                            {

                                duplicatedt2.Columns.Add("productid", typeof(Int64));
                                duplicatedt2.Columns.Add("slno", typeof(Int32));
                                duplicatedt2.Columns.Add("Quantity", typeof(Decimal));
                                duplicatedt2.Columns.Add("packing", typeof(Decimal));
                                duplicatedt2.Columns.Add("PackingUom", typeof(Int32));
                                duplicatedt2.Columns.Add("PackingSelectUom", typeof(Int32));

                                if (HttpContext.Current.Session["SessionPackingDetails"] != null)
                                {
                                    List<ERP.OMS.Management.Activities.SalesOrderAdd.ProductQuantity> obj = new List<ERP.OMS.Management.Activities.SalesOrderAdd.ProductQuantity>();
                                    obj = (List<ERP.OMS.Management.Activities.SalesOrderAdd.ProductQuantity>)HttpContext.Current.Session["SessionPackingDetails"];
                                    foreach (var item in obj)
                                    {
                                        duplicatedt2.Rows.Add(item.productid, item.slno, item.Quantity, item.packing, item.PackingUom, item.PackingSelectUom);
                                    }
                                }

                            }
                            else
                            {
                                CRMSalesOrderDtlBL CRMSales = new CRMSalesOrderDtlBL();
                                DataSet busketSet = CRMSales.GetOrderBusketById(Convert.ToInt32(hdBasketId.Value));

                                DataTable GrdpackQty = busketSet.Tables[3];
                                // DataTable duplicatedt2 = new DataTable();
                                duplicatedt2.Columns.Add("productid", typeof(Int64));
                                duplicatedt2.Columns.Add("pslno", typeof(Int32));
                                duplicatedt2.Columns.Add("pQuantity", typeof(Decimal));
                                duplicatedt2.Columns.Add("packing", typeof(Decimal));
                                duplicatedt2.Columns.Add("PackingUom", typeof(Int32));
                                duplicatedt2.Columns.Add("PackingSelectUom", typeof(Int32));
                                HttpContext.Current.Session["SessionPackingDetails"] = GetWaitingProductDetails(GrdpackQty);
                                if (HttpContext.Current.Session["SessionPackingDetails"] != null)
                                {
                                    List<WaitingProductQuantity> obj = new List<WaitingProductQuantity>();
                                    obj = (List<WaitingProductQuantity>)HttpContext.Current.Session["SessionPackingDetails"];
                                    foreach (var item in obj)
                                    {
                                        duplicatedt2.Rows.Add(item.productid, item.pslno, item.pQuantity, item.packing, item.PackingUom, item.PackingSelectUom);
                                    }
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

                            DataTable addrDesc = new DataTable();
                            addrDesc = (DataTable)Session["InlineRemarks"];

                            string Doctype = Convert.ToString(rdl_Salesquotation.SelectedValue);

                            if (hdnApproveStatus.Value == "")
                            {
                                hdnApproveStatus.Value = "0";
                            }
                            int ApproveRejectstatus = Convert.ToInt32(hdnApproveStatus.Value);

                            // Rev 1.0 [strQtyTolerance added ] -- , strQtyTolerance REMOVED
                            //int id = (ModifySalesOrder(MainOrderID, strSchemeType, UniqueQuotation, strQuoteDate, strQuoteExpiry, strCustomer, strContactName, ProjId,
                            //                  Reference, strBranch, strAgents, strCurrency, strRate, strTaxType, strTaxCode, tempSalesOrderdt, addrDesc, TaxDetailTable, ActionType, OANumber, OADate, "0", QuotationDate, QuoComponent, tempWarehousedt, tempBillAddress, tempTaxDetailsdt, approveStatus
                            //                  , creditdays, strDueDate, IsInvenotry, IsFromCRM, PosForGst, SchemeList[0], duplicatedt2, ddlInventory.SelectedValue, MultiUOMDetails, Doctype, projectValidFrom, projectValidUpto, ApproveRejectstatus, AppRejRemarks, RevisionDate, RevisionNo
                            //                  , Segment1, Segment2, Segment3, Segment4, Segment5, strQtyTolerance));
                            int id = (ModifySalesOrder(MainOrderID, strSchemeType, UniqueQuotation, strQuoteDate, strQuoteExpiry, strCustomer, strContactName, ProjId,
                                              Reference, strBranch, strAgents, strCurrency, strRate, strTaxType, strTaxCode, tempSalesOrderdt, addrDesc, TaxDetailTable, ActionType, OANumber, OADate, "0", QuotationDate, QuoComponent, tempWarehousedt, tempBillAddress, tempTaxDetailsdt, approveStatus
                                              , creditdays, strDueDate, IsInvenotry, IsFromCRM, PosForGst, SchemeList[0], duplicatedt2, ddlInventory.SelectedValue, MultiUOMDetails, Doctype, projectValidFrom, projectValidUpto, ApproveRejectstatus, AppRejRemarks, RevisionDate, RevisionNo
                                              , Segment1, Segment2, Segment3, Segment4, Segment5));
                            if (id <= 0)
                            {
                                if (id == -1)
                                {
                                    grid.JSProperties["cpIsDocIdExists"] = "NotExistsDocId";
                                }
                                else if (id == -15)
                                {
                                    grid.JSProperties["cpIsDocIdExists"] = "NotExistsDocId";
                                }
                                else if (id == -20)
                                {
                                    grid.JSProperties["cpDormantCustomer"] = "DormantCustomer";
                                }
                                else if (id == -12)
                                {
                                    grid.JSProperties["cpSaveSuccessOrFail"] = "EmptyProject";
                                }
                                else
                                {
                                    grid.JSProperties["cpSaveSuccessOrFail"] = "errorInsert";
                                }

                            }
                            else
                            {
                                //####### Coded By Samrat Roy For Custom Control Data Process #########

                                if (Request.QueryString.AllKeys.Contains("SalId") && !string.IsNullOrEmpty(Request.QueryString["SalId"]))
                                {
                                    grid.JSProperties["cpCRMSavedORNot"] = "crmOrderSaved";
                                }
                                grid.JSProperties["cpDocumentNo"] = id;
                                if (!string.IsNullOrEmpty(hfControlData.Value))
                                {
                                    CommonBL objCommonBL = new CommonBL();
                                    objCommonBL.InsertTransporterControlDetails(id, "SO", hfControlData.Value, Convert.ToString(HttpContext.Current.Session["userid"]));
                                }
                                DataTable udfTable = (DataTable)Session["UdfDataOnAdd"];
                                if (udfTable != null)
                                Session["UdfDataOnAdd"] = reCat.insertRemarksCategoryAddMode("SO", "SalesOrder" + Convert.ToString(id), udfTable, Convert.ToString(Session["userid"]));

                                if (Session["OrderDetails"] != null)
                                {
                                    Session["OrderDetails"] = null;
                                    //  Session.Remove("OrderDetails");
                                }

                                if (!string.IsNullOrEmpty(hfOtherTermsConditionData.Value))
                                {
                                    OtherTermsAndCondition.SaveTC(hfOtherTermsConditionData.Value, Convert.ToString(id), "SO","AddEdit");
                                }
                                #region Sandip Section For Approval Section Start
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

                            }

                            Session["Entry_Type"] = ddlInventory.SelectedValue;

                        }
                        //  grid.JSProperties["cpSalesOrderNo"] = UniqueQuotation;

                        //Added:Subhabrat

                                #endregion Sandip Section For Approval Dtl Section End

                                
                                
                        //End

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
            if (Session["OrderDetails"] != null)
            {
                DataTable Quotationdt = (DataTable)Session["OrderDetails"];
                DataView dvData = new DataView(Quotationdt);
                dvData.RowFilter = "Status <> 'D'";
                grid.DataSource = GetSalesOrder(dvData.ToTable());
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
                    DataTable Quotationdt = (DataTable)Session["OrderDetails"];
                    DataView dvData = new DataView(Quotationdt);
                    dvData.RowFilter = "Status <> 'D'";
                    grid.DataSource = GetSalesOrder(dvData.ToTable());
                    grid.DataBind();
                }
            }
            else if (strSplitCommand == "GridBlank")
            {
                Session["OrderDetails"] = null;
                grid.DataSource = null;
                grid.DataBind();
            }
            else if (strSplitCommand == "CurrencyChangeDisplay")
            {
                if (Session["OrderDetails"] != null)
                {
                    DataTable SalesOrderdt = (DataTable)Session["OrderDetails"];
                    string strCurrRate = Convert.ToString(txt_Rate.Text);
                    decimal strRate = 1;
                    if (strCurrRate != "")
                    {
                        strRate = Convert.ToDecimal(strCurrRate);
                        if (strRate == 0) strRate = 1;
                    }
                    foreach (DataRow dr in SalesOrderdt.Rows)
                    {
                        string Status = Convert.ToString(dr["Status"]);
                        string strSalePrice = "0";
                        if (Status != "D")
                        {
                            string ProductDetails = Convert.ToString(dr["ProductID"]);
                            string QuantityValue = Convert.ToString(dr["Quantity"]);
                            string Discount = Convert.ToString(dr["Discount"]);
                            string[] ProductDetailsList = ProductDetails.Split(new string[] { "||@||" }, StringSplitOptions.None);
                            string strMultiplier = ProductDetailsList[7];
                            string strFactor = ProductDetailsList[8];
                            if (Convert.ToDecimal(dr["SalePrice"]) == 0)
                            {
                                strSalePrice = ProductDetailsList[6];
                            }
                            decimal SalePrice = Convert.ToDecimal(strSalePrice) / strRate;
                            string Amount = Convert.ToString(Math.Round((Convert.ToDecimal(QuantityValue) * Convert.ToDecimal(strFactor) * SalePrice), 2));
                            string amountAfterDiscount = Convert.ToString(Math.Round((Convert.ToDecimal(Amount) - ((Convert.ToDecimal(Discount) * Convert.ToDecimal(Amount)) / 100)), 2));
                            dr["SalePrice"] = Convert.ToString(Math.Round(SalePrice, 2));
                            dr["Amount"] = amountAfterDiscount;
                            //dr["TaxAmount"] = amountAfterDiscount;
                            //dr["TotalAmount"] = TotalAmount;
                            dr["TaxAmount"] = ReCalculateTaxAmount(Convert.ToString(dr["SrlNo"]), Convert.ToDouble(amountAfterDiscount));
                            dr["TotalAmount"] = Convert.ToDecimal(dr["Amount"]) + Convert.ToDecimal(dr["TaxAmount"]);
                        }
                    }
                    Session["OrderDetails"] = SalesOrderdt;
                    DataView dvData = new DataView(SalesOrderdt);
                    dvData.RowFilter = "Status <> 'D'";
                    grid.DataSource = GetSalesOrder(dvData.ToTable());
                    grid.DataBind();
                }
            }
            else if (strSplitCommand == "BindGridOnQuotation")
            {
                ASPxGridView gv = sender as ASPxGridView;
                string command = e.Parameters.ToString();
                string State = Convert.ToString(e.Parameters.Split('~')[1]);
                String QuoComponent1 = "";
                string Product_id1 = "";
                string QuoteDetails_Id = "";
                QuotationIds = string.Empty;
                int AmountsAre = 0;
                for (int i = 0; i < grid_Products.GetSelectedFieldValues("Quotation_No").Count; i++)
                {
                    QuoComponent1 += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("Quotation_No")[i]);
                    Product_id1 += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("ProductID")[i]);
                    QuoteDetails_Id += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("QuoteDetails_Id")[i]);
                    //chinmoy added for taxtype 
                    AmountsAre = Convert.ToInt32(grid_Products.GetSelectedFieldValues("TaxAmountType")[0]);
                }
                QuotationIds = QuoComponent1;
                QuoComponent1 = QuoComponent1.TrimStart(',');
                Product_id1 = Product_id1.TrimStart(',');
                QuoteDetails_Id = QuoteDetails_Id.TrimStart(',');
                string Quote_Nos = Convert.ToString(e.Parameters.Split('~')[1]);
                if (AmountsAre == 1)
                {
                    ddl_AmountAre.Value = "1";
                    ddl_AmountAre.ClientEnabled = false;
                }
                else if (AmountsAre == 2)
                {
                    ddl_AmountAre.Value = "2";
                    ddl_AmountAre.ClientEnabled = false;
                }
                string mode = "";
                // Mantis Issue 24913
                //if (rdl_Salesquotation.SelectedValue == "QN")
                //{
                //    mode = "GetQuotationDetailsFromSalesOrder";
                //}
                //else if (rdl_Salesquotation.SelectedValue == "SINQ")
                //{
                //    mode = "GetInquiryTagInSalesOrder";
                //}

                string mode_MultiUOM = "";

                if (rdl_Salesquotation.SelectedValue == "QN")
                {
                    mode = "GetQuotationDetailsFromSalesOrder_New";
                    mode_MultiUOM = "Add_QN";
                }
                else if (rdl_Salesquotation.SelectedValue == "SINQ")
                {
                    mode = "GetInquiryTagInSalesOrder_New";
                    mode_MultiUOM = "Add_SINQ";
                }
                // End of Mantis Issue 24913
                if (Quote_Nos != "$")
                {
                    if (!string.IsNullOrEmpty(QuoComponent1))
                    {
                        DataSet dt_QuotationDetailtagged = new DataSet();
                        DataTable dt_QuotationDetails = new DataTable();
                        DataTable MultiUOMTaggedData = new DataTable();
                        string IdKey = Convert.ToString(Request.QueryString["key"]);
                        if (!string.IsNullOrEmpty(IdKey))
                        {
                            // Mantis Issue 24913
                            //if (IdKey != "ADD")
                            //{
                            //    dt_QuotationDetailtagged = objSlaesActivitiesBL.GetInquriydetailsfromSalesOrdertagged(mode, QuoComponent1, QuoteDetails_Id, Product_id1, "Edit");
                            //    dt_QuotationDetails = dt_QuotationDetailtagged.Tables[0];
                            //}
                            //else
                            //{
                            //    dt_QuotationDetailtagged = objSlaesActivitiesBL.GetInquriydetailsfromSalesOrdertagged(mode, QuoComponent1, QuoteDetails_Id, Product_id1, "Add");
                            //    dt_QuotationDetails = dt_QuotationDetailtagged.Tables[0];
                            //    MultiUOMTaggedData = objSlaesActivitiesBL.GetQuotationMultiUOM(QuoComponent1, QuoteDetails_Id, Product_id1, "Add");
                            //}
                            if (IdKey != "ADD")
                            {
                                dt_QuotationDetailtagged = objSlaesActivitiesBL.GetInquriydetailsfromSalesOrdertagged_New(mode, QuoComponent1, QuoteDetails_Id, Product_id1, "Edit");
                                dt_QuotationDetails = dt_QuotationDetailtagged.Tables[0];
                                MultiUOMTaggedData = objSlaesActivitiesBL.GetQuotationMultiUOM_New(QuoComponent1, QuoteDetails_Id, Product_id1, mode_MultiUOM);
                            }
                            else
                            {
                                dt_QuotationDetailtagged = objSlaesActivitiesBL.GetInquriydetailsfromSalesOrdertagged_New(mode, QuoComponent1, QuoteDetails_Id, Product_id1, "Add");
                                dt_QuotationDetails = dt_QuotationDetailtagged.Tables[0];
                                MultiUOMTaggedData = objSlaesActivitiesBL.GetQuotationMultiUOM_New(QuoComponent1, QuoteDetails_Id, Product_id1, mode_MultiUOM);
                            }
                            // End of Mantis Issue 24913

                        }
                        else
                        {
                            //dt_QuotationDetails = objSlaesActivitiesBL.GetQuotationDetailsFromSalesOrder(QuoComponent1, QuoteDetails_Id, Product_id1);
                        }
                        DataTable additionaldesc = dt_QuotationDetailtagged.Tables[1];
                        Session["ProjectadditionRemarks"] = GetProjectQuotationInfo(additionaldesc, IdKey);
                        Session["MultiUOMData"] = MultiUOMTaggedData;
                        //Session["OrderDetails"] = null;
                        grid.DataSource = GetSalesOrderInfo(dt_QuotationDetails, IdKey);
                        grid.DataBind();
                        //Rev Rajdip For Running Total
                        DataTable Orderdt = dt_QuotationDetails.Copy();
                        decimal TotalQty = 0;
                        decimal TotalAmt = 0;
                        decimal TaxAmount = 0;
                        decimal Amount = 0;
                        decimal SalePrice = 0;
                        decimal AmountWithTaxValue = 0;
                        for (int i = 0; i < Orderdt.Rows.Count; i++)
                        {
                            TotalQty = TotalQty + Convert.ToDecimal(Orderdt.Rows[i]["QuoteDetails_Quantity"]);
                            Amount = Amount + Convert.ToDecimal(Orderdt.Rows[i]["Amount"]);
                            TaxAmount = TaxAmount + Convert.ToDecimal(Orderdt.Rows[i]["TaxAmount"]);
                            SalePrice = SalePrice + Convert.ToDecimal(Orderdt.Rows[i]["SalePrice"]);
                            TotalAmt = TotalAmt + Convert.ToDecimal(Orderdt.Rows[i]["TotalAmount"]);
                        }
                        AmountWithTaxValue = TaxAmount + Amount;
                        ASPxLabel12.Text = TotalQty.ToString();
                        bnrLblTaxableAmtval.Text = Amount.ToString();
                        bnrLblTaxAmtval.Text = TaxAmount.ToString();
                        bnrlblAmountWithTaxValue.Text = AmountWithTaxValue.ToString();
                        bnrLblInvValue.Text = TotalAmt.ToString();
                        grid.JSProperties["cpRunningTotal"] = TotalQty + "~" + Amount + "~" + TaxAmount + "~" + AmountWithTaxValue + "~" + TotalAmt;
                        //end rev rajdip
                    }
                }
                else
                {
                    grid.DataSource = null;
                    grid.DataBind();
                }
                string QuoCom = Convert.ToString(QuoComponent1.Split(',')[0]);
                grid.JSProperties["cpLoadAddressFromQuote"] = QuoCom;
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

        // Rev 1.0 [strQtyTolerance added] -- , string strQtyTolerance REMOVED
        //public int ModifySalesOrder(string OrderID, string strSchemeType, string strOrderNo, string strOrderDate, string strOrderExpiry, string strCustomer, string strContactName, Int64 ProjId,
        //                            string Reference, string strBranch, string strAgents, string strCurrency, string strRate, string strTaxType, string strTaxCode, DataTable salesOrderdt, DataTable addrDesc,
        //                            DataTable TaxDetailTable, string ActionType, string OANumber, string OADate, string QuotationNumber, string QuotationDate, string QuotationIdList, DataTable Warehousedt, DataTable BillAddressdt, DataTable QuotationTaxdt, string approveStatus,
        //                            string CreditDays, string strDueDate, int IsInventory, int IsFromCRM, string PosForGst, string SchemaID, DataTable PackingDetailsdt, string Entry_type, DataTable MultiUOMDetails, string Doctype, string projectValidFrom, string projectValidUpto, int ApproveRejectstatus, string AppRejRemarks, string RevisionDate, string RevisionNo
        //    , string Segment1, string Segment2, string Segment3, string Segment4, string Segment5, string strQtyTolerance)
        public int ModifySalesOrder(string OrderID, string strSchemeType, string strOrderNo, string strOrderDate, string strOrderExpiry, string strCustomer, string strContactName, Int64 ProjId,
                                    string Reference, string strBranch, string strAgents, string strCurrency, string strRate, string strTaxType, string strTaxCode, DataTable salesOrderdt, DataTable addrDesc,
                                    DataTable TaxDetailTable, string ActionType, string OANumber, string OADate, string QuotationNumber, string QuotationDate, string QuotationIdList, DataTable Warehousedt, DataTable BillAddressdt, DataTable QuotationTaxdt, string approveStatus,
                                    string CreditDays, string strDueDate, int IsInventory, int IsFromCRM, string PosForGst, string SchemaID, DataTable PackingDetailsdt, string Entry_type, DataTable MultiUOMDetails, string Doctype, string projectValidFrom, string projectValidUpto, int ApproveRejectstatus, string AppRejRemarks, string RevisionDate, string RevisionNo
            , string Segment1, string Segment2, string Segment3, string Segment4, string Segment5)
        {
            try
            {
                // Mantis Issue 24425, 24428
                if (MultiUOMDetails.Columns.Contains("MultiUOMSR"))
                {
                    MultiUOMDetails.Columns.Remove("MultiUOMSR");
                }
                // End of Mantis Issue 24425, 24428

                DataSet dsInst = new DataSet();
                //    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                // Mantis Issue 24425, 24428
                //SqlCommand cmd = new SqlCommand("prc_CRMSalesOrder", con);
                SqlCommand cmd = new SqlCommand("prc_SalesOrderAddEdit", con);
                // End of Mantis Issue 24425, 24428
                cmd.CommandType = CommandType.StoredProcedure;

                if (hdnApprovalReqInq.Value == "0")
                {
                    ApproveRejectstatus = 1;
                }


                if ((ApproveRejectstatus == 1) && (hdnPageStatForApprove.Value == "") && (hdnApprovalReqInq.Value == "1") && (hdnUpperApproveReject.Value == ""))
                {
                    cmd.Parameters.AddWithValue("@Action", "Add");
                }
                else if (ApproveRejectstatus == 1 && hdnApprovalReqInq.Value == "1")
                {
                    cmd.Parameters.AddWithValue("@Action", "AppprovalEdit");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Action", ActionType);
                }
                cmd.Parameters.AddWithValue("@Order_Id", OrderID);
                cmd.Parameters.AddWithValue("@OrderNo", strOrderNo);
                if (!String.IsNullOrEmpty(strOrderDate))
                    cmd.Parameters.AddWithValue("@OrderDate", Convert.ToDateTime(strOrderDate));
                cmd.Parameters.AddWithValue("@QuoteExpiry", strOrderExpiry);
                cmd.Parameters.AddWithValue("@CustomerID", strCustomer);
                if (Convert.ToString(hdBasketId.Value).Trim() != "")
                {
                    cmd.Parameters.AddWithValue("@ContactPerson", 0);
                }
                else if (strContactName != "")
                {
                    cmd.Parameters.AddWithValue("@ContactPerson", Convert.ToInt64(strContactName));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ContactPerson", 0);
                }
                cmd.Parameters.AddWithValue("@Reference", Reference);
                cmd.Parameters.AddWithValue("@BranchID", strBranch);
                cmd.Parameters.AddWithValue("@Agents", Convert.ToInt32(strAgents));
                cmd.Parameters.AddWithValue("@Currency", strCurrency);
                cmd.Parameters.AddWithValue("@PosForGst", PosForGst);
                cmd.Parameters.AddWithValue("@Rate", Convert.ToDecimal(strRate));
                cmd.Parameters.AddWithValue("@TaxType", strTaxType);
                cmd.Parameters.AddWithValue("@SCHEMEID", SchemaID); //Mantis Issue number 0018841 
                if (strTaxCode != "" && strTaxCode != null)
                {
                    if (strTaxCode.Split('~')[0] != "0")
                    {
                        cmd.Parameters.AddWithValue("@TaxCode", strTaxCode);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@TaxCode", "0");
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@TaxCode", "0");
                }
                if (Convert.ToString(hdBasketId.Value).Trim() != "")
                {
                    cmd.Parameters.AddWithValue("@BasketId", Convert.ToInt64(hdBasketId.Value));
                }
                cmd.Parameters.AddWithValue("@CompanyID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FinYear", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(Session["userid"]));
                cmd.Parameters.AddWithValue("@SalesOrderDetails", salesOrderdt);
                cmd.Parameters.AddWithValue("@TaxDetail", TaxDetailTable);
                cmd.Parameters.AddWithValue("@IsFromCRM", IsFromCRM);

                #region By Surojit Insert Or Update UOM conversion in Order
                cmd.Parameters.AddWithValue("@PackingDetails", PackingDetailsdt);
                #endregion
                cmd.Parameters.AddWithValue("@Order_OANumber", OANumber);
                if (Convert.ToDateTime(OADate) != default(DateTime))
                { cmd.Parameters.AddWithValue("@Order_OADate", Convert.ToDateTime(OADate)); }
                if (!String.IsNullOrEmpty(QuotationDate))
                    cmd.Parameters.AddWithValue("@QuotationDate", QuotationDate);
                cmd.Parameters.AddWithValue("@Order_Quotation_Ids", QuotationIdList);
                cmd.Parameters.AddWithValue("@WarehouseDetail", Warehousedt);
                cmd.Parameters.AddWithValue("@Numbering_Scheme", strSchemeType);
                cmd.Parameters.AddWithValue("@IsInvenotry", IsInventory);
                cmd.Parameters.AddWithValue("@QuotationTax", QuotationTaxdt);
                cmd.Parameters.AddWithValue("@BillAddress", BillAddressdt);
                cmd.Parameters.AddWithValue("@Entry_type", Entry_type);
                cmd.Parameters.AddWithValue("@creditDays", CreditDays);//added on 21-06-2017
                cmd.Parameters.AddWithValue("@RemarksDetails", addrDesc);
                if (Convert.ToDateTime(strDueDate) != default(DateTime))
                {
                    cmd.Parameters.AddWithValue("@CreditDueDate", strDueDate);//added on 21-06-2017
                }
                //kaushik 24-2-2017
                //End
                #region Sandip Section For Approval Section Start
                cmd.Parameters.AddWithValue("@approveStatus", approveStatus);
                cmd.Parameters.AddWithValue("@Project_Id", ProjId);
                cmd.Parameters.AddWithValue("@Doctype", Doctype);
                cmd.Parameters.AddWithValue("@MultiUOMDetails", MultiUOMDetails);


                if (projectValidFrom != "1/1/0001 12:00:00 AM")
                {
                    if (!String.IsNullOrEmpty(projectValidFrom))
                        //cmd.Parameters.AddWithValue("@ProjectValidfromDate", Convert.ToDateTime(projectValidFrom));
                        cmd.Parameters.AddWithValue("@ProjectValidfromDate", DateTime.ParseExact(projectValidFrom, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"));
                }
                if (projectValidUpto != "1/1/0001 12:00:00 AM")
                {
                    if (!String.IsNullOrEmpty(projectValidUpto))
                        //cmd.Parameters.AddWithValue("@ProjectValidUptoDate", Convert.ToDateTime(projectValidUpto));
                        cmd.Parameters.AddWithValue("@ProjectValidUptoDate", DateTime.ParseExact(projectValidUpto, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"));
                }

                if (RevisionDate != "1/1/0001 12:00:00 AM")
                {
                    if (!String.IsNullOrEmpty(RevisionDate))
                        //cmd.Parameters.AddWithValue("@ProjectValidfromDate", Convert.ToDateTime(projectValidFrom));
                        cmd.Parameters.AddWithValue("@RevisionDate", DateTime.ParseExact(RevisionDate, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"));
                }

                cmd.Parameters.AddWithValue("@RevisionNo", RevisionNo);

                cmd.Parameters.AddWithValue("@ApproveRejectStatus", ApproveRejectstatus);
                cmd.Parameters.AddWithValue("@ApproveRejectRemarks", AppRejRemarks);



                #endregion Sandip Section For Approval Dtl Section End

                cmd.Parameters.AddWithValue("@SegmentID1", Segment1);
                cmd.Parameters.AddWithValue("@SegmentID2", Segment2);
                cmd.Parameters.AddWithValue("@SegmentID3", Segment3);
                cmd.Parameters.AddWithValue("@SegmentID4", Segment4);
                cmd.Parameters.AddWithValue("@SegmentID5", Segment5);
                // Rev 1.0
                //if(IsToleranceInSalesOrder.ToUpper().Trim() == "NO")
                //{
                //    cmd.Parameters.AddWithValue("@QtyTolerance", 0.00);
                //}
                //else
                //{
                //    cmd.Parameters.AddWithValue("@QtyTolerance", Convert.ToDecimal(strQtyTolerance));
                //}
                // End of Rev 1.0

                cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
                cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@GeneratedDocNumber", SqlDbType.VarChar, 50);
                cmd.Parameters["@GeneratedDocNumber"].Direction = ParameterDirection.Output;
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                int idFromString = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());
                string orderNumber = Convert.ToString(cmd.Parameters["@GeneratedDocNumber"].Value.ToString());

                if (idFromString > 0)
                {
                    grid.JSProperties["cpSalesOrderNo"] = orderNumber;
                    if (!string.IsNullOrEmpty(hfTermsConditionData.Value))
                    {
                        TermsConditionsControl.SaveTC(hfTermsConditionData.Value, Convert.ToString(idFromString), "SO");
                    }
                }
                if (idFromString > 0)
                {
                    if (Request.QueryString.AllKeys.Contains("status") || Request.QueryString.AllKeys.Contains("SalId"))
                    {
                        if (chkSendMail.Checked)
                        {
                            exportToPDF.ExportToPdfforEmail("SO-Default~D", "Sorder", Server.MapPath("~"), "", Convert.ToString(idFromString));
                            Sendmail_SalesOrder(Convert.ToString(idFromString));
                        }
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
        protected void gridTax_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];
            if (strSplitCommand == "Display")
            {
                DataTable TaxDetailsdt = new DataTable();
                if (Session["STaxDetails"] == null)
                {
                    Session["STaxDetails"] = GetTaxData(dt_PLSales.Date.ToString("yyyy-MM-dd"));
                }

                if (Session["STaxDetails"] != null)
                {
                    TaxDetailsdt = (DataTable)Session["STaxDetails"];

                    #region Delete Igst,Cgst,Sgst respectively
                    //Get Company Gstin 09032017
                    string CompInternalId = Convert.ToString(Session["LastCompany"]);
                    string BranchId = Convert.ToString(ddl_Branch.SelectedValue);
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






                    //// Date: 30-05-2017    Author: Kallol Samanta  [START] 
                    //// Details: Billing/Shipping user control integration 

                    //Old Code
                    //if (chkBilling.Checked)
                    //{
                    //    if (CmbState.Value != null)
                    //    {
                    //        ShippingState = CmbState.Text;
                    //        ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                    //    }
                    //}
                    //else
                    //{
                    //    if (CmbState1.Value != null)
                    //    {
                    //        ShippingState = CmbState1.Text;
                    //        ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                    //    }
                    //}

                    //New Code
                    // Edited Chinmoy Below Line
                    string sstateCode = "";
                    if (ddl_PosGstSalesOrder.Value != null)
                    {
                        if (ddl_PosGstSalesOrder.Value.ToString() == "S")
                        {
                            sstateCode = Sales_BillingShipping.GeteShippingStateCode();
                        }
                        else
                        {
                            sstateCode = Sales_BillingShipping.GetBillingStateCode();
                        }
                    }

                    ShippingState = sstateCode;
                    //if (ShippingState.Trim() != "")
                    //{
                    //    ShippingState = ShippingState.Substring(ShippingState.IndexOf("(State Code:")).Replace("(State Code:", "").Replace(")", "");
                    //}

                    //// Date: 30-05-2017    Author: Kallol Samanta  [END]







                    if (ShippingState.Trim() != "" && GSTIN.Trim() != "")
                    {

                        if (GSTIN.Substring(0, 2) == ShippingState)
                        {

                            //Check if the state is in union territories then only UTGST will apply
                            //   Chandigarh     Andaman and Nicobar Islands    DADRA & NAGAR HAVELI    DAMAN & DIU      Lakshadweep              PONDICHERRY
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
                if (Session["STaxDetails"] != null)
                {
                    TaxDetailsdt = (DataTable)Session["STaxDetails"];
                }
                else
                {
                    TaxDetailsdt.Columns.Add("Taxes_ID", typeof(string));
                    TaxDetailsdt.Columns.Add("Taxes_Name", typeof(string));
                    TaxDetailsdt.Columns.Add("Percentage", typeof(string));
                    TaxDetailsdt.Columns.Add("Amount", typeof(string));
                    TaxDetailsdt.Columns.Add("TaxTypeCode", typeof(string));
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

                    Session["STaxDetails"] = TaxDetailsdt;
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
            if (Session["STaxDetails"] != null)
            {
                TaxDetailsdt = (DataTable)Session["STaxDetails"];
            }
            else
            {
                TaxDetailsdt.Columns.Add("Taxes_ID", typeof(string));
                TaxDetailsdt.Columns.Add("Taxes_Name", typeof(string));
                TaxDetailsdt.Columns.Add("Percentage", typeof(string));
                TaxDetailsdt.Columns.Add("Amount", typeof(string));
                TaxDetailsdt.Columns.Add("TaxTypeCode", typeof(string));
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

            Session["STaxDetails"] = TaxDetailsdt;

            gridTax.DataSource = GetTaxes(TaxDetailsdt);
            gridTax.DataBind();
        }

        protected void gridTax_DataBinding(object sender, EventArgs e)
        {
            if (Session["STaxDetails"] != null)
            {
                DataTable TaxDetailsdt = (DataTable)Session["STaxDetails"];

                //gridTax.DataSource = GetTaxes();
                var taxlist = (List<Taxes>)GetTaxes(TaxDetailsdt);
                var taxChargeDataSource = setChargeCalculatedOn(taxlist, TaxDetailsdt);
                gridTax.DataSource = taxChargeDataSource;
            }
        }

        protected void grid_Products_DataBinding(Object sender, EventArgs e)
        {

            if (Session["SO_ProductDetails"] != null)
            {
                DataTable Quotationdt = (DataTable)Session["SO_ProductDetails"];
                DataView dvData = new DataView(Quotationdt);
                //dvData.RowFilter = "Status <> 'D'";
                grid_Products.DataSource = GetProductsInfo(Quotationdt);
            }
        }

        protected void cgridProducts_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];
            if (strSplitCommand == "BindProductsDetails")
            {

                string Quote_Nos = Convert.ToString(e.Parameters.Split('~')[1]);

                String QuoComponent = "";
                List<object> QuoList = lookup_quotation.GridView.GetSelectedFieldValues("Quote_Id");
                foreach (object Quo in QuoList)
                {
                    QuoComponent += "," + Quo;
                }
                QuoComponent = QuoComponent.TrimStart(',');

                string strType = Convert.ToString(rdl_Salesquotation.SelectedValue);
                string Mode = "";
                if (strType == "QN")
                {
                    Mode = "GetQuotationDetailsOnly";
                }
                else if (strType == "SINQ")
                {
                    Mode = "GetInquiryDetailsOnly";
                }

                if (Quote_Nos != "$")
                {
                    //grid.DataSource = null;
                    //grid.DataBind();
                    //string OrderId = Convert.ToString(e.Parameters.Split('~')[2]);

                    //Session["OrderID"] = OrderId;
                    DataTable dt_QuotationDetails = new DataTable();
                    string IdKey = Convert.ToString(Request.QueryString["key"]);
                    if (!string.IsNullOrEmpty(IdKey))
                    {
                        if (IdKey != "ADD")
                        {
                            dt_QuotationDetails = objSlaesActivitiesBL.GetDocumentDetailsFromSalesOrderonly(Mode, QuoComponent, IdKey, "");
                        }
                        else
                        {
                            dt_QuotationDetails = objSlaesActivitiesBL.GetDocumentDetailsFromSalesOrderonly(Mode, QuoComponent, "", "");
                        }

                    }
                    else
                    {
                        dt_QuotationDetails = objSlaesActivitiesBL.GetDocumentDetailsFromSalesOrderonly(QuoComponent, "", "", "");
                    }
                    // Session["OrderDetails"] = null;
                    //grid.DataSource = GetSalesOrderInfo(dt_QuotationDetails, IdKey);
                    //grid.DataBind();
                    Session["SO_ProductDetails"] = dt_QuotationDetails;
                    grid_Products.DataSource = GetProductsInfo(dt_QuotationDetails);
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

        protected void cCustomerOutstanding_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strAsOnDate = Convert.ToString(e.Parameters.Split('~')[3]);
            string strCustomerId = Convert.ToString(e.Parameters.Split('~')[1]);
            string BranchId = e.Parameters.Split('~')[2];
            string strCommand = Convert.ToString(e.Parameters.Split('~')[0]);
            DataTable dtOutStanding = new DataTable();
            if (strCommand == "BindOutStanding")
            {
                dtOutStanding = objSlaesActivitiesBL.GetCustomerOutstandingRecords(strAsOnDate, strCustomerId, BranchId);

                //CustomerOutstanding.DataSource = dtOutStanding;
                //CustomerOutstanding.DataBind();
                CustomerOutstanding.JSProperties["cpOutStanding"] = "OutStanding";


            }
        }

        [WebMethod]
        public static string SetApproveReject(string ApproveRemarks, int ApproveRejStatus, string OrderId)
        {
            CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();
            string val = objCRMSalesOrderDtlBL.ApproveRejectProject(ApproveRemarks, ApproveRejStatus, OrderId);
            return val;
        }
        [WebMethod]
        public static int Duplicaterevnumbercheck(string RevNo, string Order)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            int returnVal = 0;
            CRMSalesOrderDtlBL objCRMSalesOrderDtlBL = new CRMSalesOrderDtlBL();
            DataTable dtrev = new DataTable();
            DataTable OrderNumber = new DataTable();
            OrderNumber = oDBEngine.GetDataTable("select ISNULL(Order_Number,'') Order_Number from tbl_trans_SalesOrder  where Order_Id='" + Order + "'");
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
            dtrev = oDBEngine.GetDataTable("select Order_RevisionNo from tbl_trans_SalesOrder where  Order_Number='" + orderno + "'");
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
            //DataSet ds = new DataSet();
            //ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            //proc.AddVarcharPara("@Action", 500, "WarehouseList");
            //proc.AddVarcharPara("@Order_Id", 500, Convert.ToString(Session["OrderID"]));
            //proc.AddVarcharPara("@ProductID", 500, strProduct);
            //ds = proc.GetDataSet();
            //return ds;


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

        public void GetQuantityBaseOnProductforDetailsId(string strSrlNo, ref decimal strUOMQuantity)
        {
            decimal sum = 0;

            DataTable MultiUOMData = new DataTable();
            if (Session["MultiUOMData"] != null)
            {
                MultiUOMData = (DataTable)Session["MultiUOMData"];
                for (int i = 0; i < MultiUOMData.Rows.Count; i++)
                {
                    DataRow dr = MultiUOMData.Rows[i];
                    string SrlNo = Convert.ToString(dr["SrlNo"]);

                    if (strSrlNo == SrlNo)
                    {
                        string strQuantity = (Convert.ToString(dr["Quantity"]) != "") ? Convert.ToString(dr["Quantity"]) : "0";
                        var weight = Decimal.Parse(Regex.Match(strQuantity, "[0-9]*\\.*[0-9]*").Value);

                        sum = Convert.ToDecimal(weight);
                    }
                }
            }

            strUOMQuantity = sum;

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


        public IEnumerable GetProjectQuotationInfo(DataTable ProjectOrderdt1, string Order_Id)
        {
            List<ProjectQuotationdet> OrderList = new List<ProjectQuotationdet>();
            DataColumnCollection dtC = ProjectOrderdt1.Columns;
            DataTable dtdfg = new DataTable();


            dtdfg.Columns.Add("SrlNo", typeof(string));
            dtdfg.Columns.Add("ProjectAdditionRemarks", typeof(string));

            for (int i = 0; i < ProjectOrderdt1.Rows.Count; i++)
            {
                ProjectQuotationdet Orders = new ProjectQuotationdet();

                Orders.SrlNo = Convert.ToString(i + 1);
                Orders.ProjectAdditionRemarks = Convert.ToString(ProjectOrderdt1.Rows[i]["ProjectAdditionRemarks"]);
                OrderList.Add(Orders);
                dtdfg.Rows.Add(Orders.SrlNo, Orders.ProjectAdditionRemarks);

            }


            Session["InlineRemarks"] = dtdfg;
            return OrderList;
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


            //else if(strSplitCommand=="BindRemarks")
            //{

            //    DataTable dt = GetOrderData().Tables[1];

            //   //txtInlineRemarks.Text=


            //}

            else if (strSplitCommand == "DisplayRemarks")
            {
                //DataTable Remarksdt=null;
                //if (Session["AdditionalRemarks"] != null)
                //{
                //    Remarksdt = (DataTable)Session["AdditionalRemarks"];
                //}
                //else
                //{
                DataTable Remarksdt = (DataTable)Session["InlineRemarks"];
                //}



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
            //Rev work start 25.07.2022 mantise no:0025067 Unable to check warehouse wise stock in Sales Order module
           // proc.AddVarcharPara("@branchId", 500, Convert.ToString(Session["userbranchID"]));
            proc.AddVarcharPara("@branchId", 500, Convert.ToString(ddl_Branch.SelectedValue));
            //Rev work close 25.07.2022 mantise no:0025067 Unable to check warehouse wise stock in Sales Order module
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

        protected DataTable GetTaxDataWithGST(DataTable existing)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "TaxDetailsForGst");
            proc.AddVarcharPara("@Order_Id", 500, Convert.ToString(Session["OrderID"]));
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
                txtGstCstVatCharge.Value = gstRow["Amount"];
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
            //Rev Rajdip
            bnrOtherChargesvalue.Text = totalCharges.ToString();
            //End Rev Rajdip
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
        [WebMethod(EnableSession = true)]
        public static object taxUpdatePanel_Callback(string Action, string srl, string prodid, string uniqueId)
        {
            string output = "200";
            if (Action == "DelProdbySl")
            {
                DataTable MainTaxDataTable = (DataTable)HttpContext.Current.Session["SalesOrderFinalTaxRecord" + Convert.ToString(uniqueId)];

                DataRow[] deletedRow = MainTaxDataTable.Select("SlNo=" + srl);
                if (deletedRow.Length > 0)
                {
                    foreach (DataRow dr in deletedRow)
                    {
                        MainTaxDataTable.Rows.Remove(dr);
                    }

                }

                HttpContext.Current.Session["SalesOrderFinalTaxRecord" + Convert.ToString(uniqueId)] = MainTaxDataTable;
                //GetStock(Convert.ToString(prodid));
                //DeleteWarehouse(Convert.ToString(performpara.Split('~')[1]));
                DataTable taxDetails = (DataTable)HttpContext.Current.Session["STaxDetails"];
                if (taxDetails != null)
                {
                    foreach (DataRow dr in taxDetails.Rows)
                    {
                        dr["Amount"] = "0.00";
                    }
                    HttpContext.Current.Session["STaxDetails"] = taxDetails;
                }

            }
            else if (Action == "DeleteAllTax")
            {
                CreateDataTaxTable(uniqueId);

                DataTable taxDetails = (DataTable)HttpContext.Current.Session["STaxDetails"];

                if (taxDetails != null)
                {
                    foreach (DataRow dr in taxDetails.Rows)
                    {
                        dr["Amount"] = "0.00";
                    }
                    HttpContext.Current.Session["STaxDetails"] = taxDetails;
                }
            }
            else
            {
                DataTable MainTaxDataTable = (DataTable)HttpContext.Current.Session["SalesOrderFinalTaxRecord" + Convert.ToString(uniqueId)];

                DataRow[] deletedRow = MainTaxDataTable.Select("SlNo=" + srl);
                if (deletedRow.Length > 0)
                {
                    foreach (DataRow dr in deletedRow)
                    {
                        MainTaxDataTable.Rows.Remove(dr);
                    }

                }

                HttpContext.Current.Session["SalesOrderFinalTaxRecord" + Convert.ToString(uniqueId)] = MainTaxDataTable;
                DataTable taxDetails = (DataTable)HttpContext.Current.Session["STaxDetails"];
                if (taxDetails != null)
                {
                    foreach (DataRow dr in taxDetails.Rows)
                    {
                        dr["Amount"] = "0.00";
                    }
                    HttpContext.Current.Session["STaxDetails"] = taxDetails;
                }
            }
            return output;
        }




        //public static void GetStock(string strProductID)
        //{
        //    string strBranch = Convert.ToString(ddl_Branch.SelectedValue);
        //    acpAvailableStock.JSProperties["cpstock"] = "0.00";

        //    try
        //    {
        //        DataTable dt2 = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableQuotation(" + strBranch + ",'" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]) + "'," + strProductID + ") as branchopenstock");

        //        if (dt2.Rows.Count > 0)
        //        {
        //            taxUpdatePanel.JSProperties["cpstock"] = Convert.ToString(Math.Round(Convert.ToDecimal(dt2.Rows[0]["branchopenstock"]), 2));
        //        }
        //        else
        //        {
        //            taxUpdatePanel.JSProperties["cpstock"] = "0.00";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        taxUpdatePanel.JSProperties["cpstock"] = "0.00";
        //    }
        //}


        protected void acbpCrpUdf_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            //  BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

            if (Request.QueryString["key"] == "ADD")
            {
                if (reCat.isAllMandetoryDone((DataTable)Session["UdfDataOnAdd"], "SO") == false)
                {
                    acbpCrpUdf.JSProperties["cpUDF"] = "false";

                }
                else
                {
                    acbpCrpUdf.JSProperties["cpUDF"] = "true";
                }


                acbpCrpUdf.JSProperties["cpTransport"] = "true";
                acbpCrpUdf.JSProperties["cpTC"] = "true";

                #region Transporter
                // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                DataTable DT = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='Transporter_SOMandatory' AND IsActive=1");
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
                #endregion
                #region TC
                DataTable DT_TC = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='TC_SOMandatory' AND IsActive=1");
                if (DT_TC != null && DT_TC.Rows.Count > 0)
                {
                    string IsMandatory = Convert.ToString(DT_TC.Rows[0]["Variable_Value"]).Trim();

                    //  objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                    objEngine = new BusinessLogicLayer.DBEngine();


                    DataTable DTVisible = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='Show_TC_SO' AND IsActive=1");
                    if (Convert.ToString(DTVisible.Rows[0]["Variable_Value"]).Trim() == "Yes")
                    {
                        if (IsMandatory == "Yes")
                        {
                            if (hfTermsConditionData.Value.Trim() == "" || TermsConditionsControl.GetControlValue("dtDeliveryDate") == "" || TermsConditionsControl.GetControlValue("dtDeliveryDate") == "@")
                            {
                                acbpCrpUdf.JSProperties["cpTC"] = "false";
                            }

                            else { acbpCrpUdf.JSProperties["cpTC"] = "true"; }
                        }
                    }
                    else { acbpCrpUdf.JSProperties["cpTC"] = "true"; }
                }
                #endregion


               
            }
            else
            {
                acbpCrpUdf.JSProperties["cpUDF"] = "true";
                acbpCrpUdf.JSProperties["cpTransport"] = "true";
                acbpCrpUdf.JSProperties["cpTC"] = "true";

                #region transporter
                DataTable DT = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='Transporter_SOMandatory' AND IsActive=1");
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
                #endregion
                #region TC
                DataTable DT_TC = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='TC_SOMandatory' AND IsActive=1");
                if (DT_TC != null && DT_TC.Rows.Count > 0)
                {
                    string IsMandatory = Convert.ToString(DT_TC.Rows[0]["Variable_Value"]).Trim();

                    //objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                    objEngine = new BusinessLogicLayer.DBEngine();


                    DataTable DTVisible = objEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='Show_TC_SO' AND IsActive=1");
                    if (Convert.ToString(DTVisible.Rows[0]["Variable_Value"]).Trim() == "Yes")
                    {
                        if (IsMandatory == "Yes")
                        {
                            if (TermsConditionsControl.GetControlValue("dtDeliveryDate") == "")
                            {
                                acbpCrpUdf.JSProperties["cpTC"] = "false";
                            }

                            else { acbpCrpUdf.JSProperties["cpTC"] = "true"; }
                        }
                    }
                    else { acbpCrpUdf.JSProperties["cpTC"] = "true"; }
                }
                #endregion
                
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
        public static void CreateDataTaxTable(string uniqueid)
        {
            DataTable TaxRecord = new DataTable();

            TaxRecord.Columns.Add("SlNo", typeof(System.Int32));
            TaxRecord.Columns.Add("TaxCode", typeof(System.String));
            TaxRecord.Columns.Add("AltTaxCode", typeof(System.String));
            TaxRecord.Columns.Add("Percentage", typeof(System.Decimal));
            TaxRecord.Columns.Add("Amount", typeof(System.Decimal));
            HttpContext.Current.Session["SalesOrderFinalTaxRecord" + Convert.ToString(uniqueid)] = TaxRecord;
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
        protected void cgridTax_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string retMsg = "";
            if (e.Parameters.Split('~')[0] == "SaveGST")
            {
                DataTable TaxRecord = (DataTable)Session["SalesOrderFinalTaxRecord" + Convert.ToString(uniqueId.Value)];
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

                Session["SalesOrderFinalTaxRecord" + Convert.ToString(uniqueId.Value)] = TaxRecord;
            }
            else
            {
                #region fetch All data For Tax

                DataTable taxDetail = new DataTable();
                DataTable MainTaxDataTable = (DataTable)Session["SalesOrderFinalTaxRecord" + Convert.ToString(uniqueId.Value)];
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








                //New Code
                // Edited Chinmoy Below Line
                string sstateCode = "";

                if (ddl_PosGstSalesOrder.Value.ToString() == "S")
                {
                    sstateCode = Sales_BillingShipping.GeteShippingStateCode();
                }
                else
                {
                    sstateCode = Sales_BillingShipping.GetBillingStateCode();
                }


                ShippingState = sstateCode;
                if (ShippingState.Trim() != "")
                {
                    ShippingState = ShippingState;
                }

                //// Date: 30-05-2017    Author: Kallol Samanta  [END]












                if (ShippingState.Trim() != "" && BranchStateCode != "")
                {
                    if (BranchStateCode == ShippingState)
                    {
                        //Check if the state is in union territories then only UTGST will apply
                        //   Chandigarh     Andaman and Nicobar Islands    DADRA & NAGAR HAVELI    DAMAN & DIU                               Lakshadweep              PONDICHERRY
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




                int slNo = Convert.ToInt32(HdSerialNo.Value);

                //Get Gross Amount and Net Amount 
                decimal ProdGrossAmt = Convert.ToDecimal(HdProdGrossAmt.Value);
                decimal ProdNetAmt = Convert.ToDecimal(HdProdNetAmt.Value);

                List<TaxDetails> TaxDetailsDetails = new List<TaxDetails>();

                //Debjyoti 09032017
                decimal totalParcentage = 0;
                foreach (DataRow dr in taxDetail.Rows)
                {
                    if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST")
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

                    DataTable TaxRecord = (DataTable)Session["SalesOrderFinalTaxRecord" + Convert.ToString(uniqueId.Value)];


                    foreach (DataRow dr in taxDetail.Rows)
                    {
                        TaxDetails obj = new TaxDetails();
                        obj.Taxes_ID = Convert.ToInt32(dr["Taxes_ID"]);
                        obj.taxCodeName = Convert.ToString(dr["taxCodeName"]);

                        if (Convert.ToString(dr["TaxCalculateMethods"]) == "A")
                            obj.Taxes_Name = Convert.ToString(dr["Taxes_Name"]) + "(+)";
                        else
                            obj.Taxes_Name = Convert.ToString(dr["Taxes_Name"]) + "(-)";
                        //chinmoy edited below code for edit mode billing/shipping change
                        //obj.TaxField = "";


                        if (Convert.ToDecimal(dr["TaxRates_Rate"]) == 0)
                        {
                            obj.TaxField = "";
                        }
                        else
                        {
                            obj.TaxField = Convert.ToString(dr["TaxRates_Rate"]);
                        }
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
                                    //obj.calCulatedOn = Math.Round(finalCalCulatedOn, 2);
                                }
                            }
                        }


                        //chinmoy added below code for edit mode billing/shipping change start
                        if (Convert.ToDecimal(dr["TaxRates_Rate"]) != 0)
                        {
                            obj.Amount = Convert.ToDouble(obj.calCulatedOn * (Convert.ToDecimal(obj.TaxField) / 100));
                        }

                        //End

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
                    Session["SalesOrderFinalTaxRecord" + Convert.ToString(uniqueId.Value)] = TaxRecord;

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
        public class ProjectQuotationdet
        {
            public string SrlNo { get; set; }
            public string ProjectAdditionRemarks { get; set; }
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


        protected void EntityServerModeDataSalesOrder_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Proj_Id";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            BusinessLogicLayer.DBEngine BEngine = new BusinessLogicLayer.DBEngine();



            // Mantis Issue 24976
            //var q = from d in dc.V_ProjectLists
            //        where d.ProjectStatus == "Approved" && d.ProjBracnchid == Convert.ToInt64(ddl_Branch.SelectedValue) && d.CustId == Convert.ToString(hdnCustomerId.Value)
            //        orderby d.Proj_Id descending
            //        select d;

            //e.QueryableSource = q;

            CommonBL cbl = new CommonBL();
            string ISProjectIndependentOfBranch = cbl.GetSystemSettingsResult("AllowProjectIndependentOfBranch");

            if (ISProjectIndependentOfBranch == "No")
            {
                var q = from d in dc.V_ProjectLists
                        where d.ProjectStatus == "Approved" && d.ProjBracnchid == Convert.ToInt64(ddl_Branch.SelectedValue) && d.CustId == Convert.ToString(hdnCustomerId.Value)
                        orderby d.Proj_Id descending
                        select d;

                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.V_ProjectLists
                        where d.ProjectStatus == "Approved" && d.CustId == Convert.ToString(hdnCustomerId.Value)
                        orderby d.Proj_Id descending
                        select d;

                e.QueryableSource = q;
            }
            // End of Mantis Issue 24976

        }



        protected void taxgrid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {

            int slNo = Convert.ToInt32(HdSerialNo.Value);
            DataTable TaxRecord = (DataTable)Session["SalesOrderFinalTaxRecord" + Convert.ToString(uniqueId.Value)];

            DataRow[] finalRow1 = TaxRecord.Select("SlNo=" + Convert.ToString(slNo));
            if (finalRow1.Length > 0)
            {
                foreach (DataRow delRow in finalRow1)
                    delRow.Delete();

                TaxRecord.AcceptChanges();
            }

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




                //else
                //{
                DataRow newRow = TaxRecord.NewRow();
                newRow["slNo"] = slNo;
                newRow["Percentage"] = Percentage;
                newRow["TaxCode"] = TaxCode;
                newRow["AltTaxCode"] = "0";
                newRow["Amount"] = Amount;
                TaxRecord.Rows.Add(newRow);
                //}


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


            Session["SalesOrderFinalTaxRecord" + Convert.ToString(uniqueId.Value)] = TaxRecord;


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
            CreateDataTaxTable(uniqueId.Value.ToString());
            //DataTable taxTableItemLvl = (DataTable)Session["SalesOrderFinalTaxRecord"];
            //foreach (DataRow dr in taxTableItemLvl.Rows)
            //    dr.Delete();
            //taxTableItemLvl.AcceptChanges();
            //Session["SalesOrderFinalTaxRecord"] = taxTableItemLvl;
        }

        protected void cmbGstCstVatcharge_Callback(object sender, CallbackEventArgsBase e)
        {
            Session["STaxDetails"] = null;
            DateTime quoteDate = Convert.ToDateTime(dt_PLSales.Date.ToString("yyyy-MM-dd"));
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
            if (Session["SalesOrderFinalTaxRecord" + Convert.ToString(uniqueId.Value)] != null)
            {
                TaxDetailTable = (DataTable)Session["SalesOrderFinalTaxRecord" + Convert.ToString(uniqueId.Value)];

                var rows = TaxDetailTable.Select("SlNo ='" + SrlNo + "'");
                foreach (var row in rows)
                {
                    row.Delete();
                }
                TaxDetailTable.AcceptChanges();

                Session["SalesOrderFinalTaxRecord" + Convert.ToString(uniqueId.Value)] = TaxDetailTable;
            }
        }
        public void UpdateTaxDetails(string oldSrlNo, string newSrlNo)
        {
            DataTable TaxDetailTable = new DataTable();
            if (Session["SalesOrderFinalTaxRecord" + Convert.ToString(uniqueId.Value)] != null)
            {
                TaxDetailTable = (DataTable)Session["SalesOrderFinalTaxRecord" + Convert.ToString(uniqueId.Value)];

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

                Session["SalesOrderFinalTaxRecord" + Convert.ToString(uniqueId.Value)] = TaxDetailTable;
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
                jsonObj.applicableOn = "G";//Convert.ToString(taxObj["ApplicableOn"]);
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
            proc.AddIntegerPara("@branchId", Convert.ToInt32(Session["userbranchID"]));
            DataTable DT = proc.GetTable();
            cmbGstCstVatcharge.DataSource = DT;
            cmbGstCstVatcharge.TextField = "Taxes_Name";
            cmbGstCstVatcharge.ValueField = "Taxes_ID";
            cmbGstCstVatcharge.DataBind();
        }

        #endregion

        protected void lookup_CustomerControlPanelMain_Callback1(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string CustomerId = e.Parameter;
            Session["CustomerDetail"] = null;
            //PopulateCustomerDetail();
            //lookup_Customer.GridView.Selection.SelectRowByKey(CustomerId);
            //SetCustomerDDbyValue(CustomerId);subhabrata 02-01-2018
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
        #region BindSalesManAgentOnDemand

        //protected void ASPxComboBox_OnItemsRequestedBySalesMenFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        //{
        //    if (e.Filter != "")
        //    {
        //        ASPxComboBox comboBox = (ASPxComboBox)source;
        //        SalesManDataSource.SelectCommand =
        //               @"select cnt_id,Name from (SELECT cnt_id,Name, row_number()over(order by t.[Name]) as [rn] from v_GetAllSalesManAgent  as t where (([Name] ) LIKE @filter)) as st where st.[rn] between @startIndex and @endIndex";

        //        SalesManDataSource.SelectParameters.Clear();
        //        SalesManDataSource.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
        //        SalesManDataSource.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
        //        SalesManDataSource.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
        //        comboBox.DataSource = SalesManDataSource;
        //        comboBox.DataBind();
        //    }
        //}

        //protected void ASPxComboBox_OnItemRequestedBySalesMenValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        //{
        //    long value = 0;
        //    if (e.Value == null || !Int64.TryParse(e.Value.ToString(), out value))
        //        return;
        //    ASPxComboBox comboBox = (ASPxComboBox)source;
        //    SalesManDataSource.SelectCommand = @"SELECT cnt_id,Name FROM v_GetAllSalesManAgent WHERE  (cnt_id = @ID) ";

        //    SalesManDataSource.SelectParameters.Clear();
        //    SalesManDataSource.SelectParameters.Add("ID", TypeCode.String, e.Value.ToString());
        //    comboBox.DataSource = SalesManDataSource;
        //    comboBox.DataBind();
        //}

        //protected void SetSalesManDDbyValue(string cnt_Id)
        //{

        //    SalesManDataSource.SelectCommand = @"SELECT cnt_id,Name FROM v_GetAllSalesManAgent WHERE (cnt_id = @ID) ";

        //    SalesManDataSource.SelectParameters.Clear();
        //    SalesManDataSource.SelectParameters.Add("ID", TypeCode.String, cnt_Id);
        //    SalesManComboBox.DataSource = SalesManDataSource;
        //    SalesManComboBox.DataBind();
        //    SalesManComboBox.Value = cnt_Id;

        //}



        #endregion
        //// Date: 30-05-2017    Author: Kallol Samanta  [START] 
        //// Details: Billing/Shipping user control integration

        //public DataTable StoreSalesOrderAddressDetail()
        //{
        //    //QuoteAdd_id, QuoteAdd_QuoteId, QuoteAdd_CompanyID, QuoteAdd_BranchId, QuoteAdd_FinYear,
        //    //QuoteAdd_ContactPerson, QuoteAdd_addressType, QuoteAdd_address1, QuoteAdd_address2, QuoteAdd_address3, 
        //    //QuoteAdd_landMark, QuoteAdd_countryId, QuoteAdd_stateId, QuoteAdd_cityId, QuoteAdd_areaId, 
        //    //QuoteAdd_pin, QuoteAdd_CreatedDate, QuoteAdd_CreatedUser, QuoteAdd_LastModifyDate, QuoteAdd_LastModifyUser

        //    DataTable AddressDetaildt = new DataTable();

        //    AddressDetaildt.Columns.Add("OrderAdd_OrderId", typeof(System.Int32));
        //    AddressDetaildt.Columns.Add("OrderAdd_CompanyID", typeof(System.String));
        //    AddressDetaildt.Columns.Add("OrderAdd_BranchId", typeof(System.Int32));
        //    AddressDetaildt.Columns.Add("OrderAdd_FinYear", typeof(System.String));

        //    AddressDetaildt.Columns.Add("OrderAdd_ContactPerson", typeof(System.String));
        //    AddressDetaildt.Columns.Add("OrderAdd_addressType", typeof(System.String));
        //    AddressDetaildt.Columns.Add("OrderAdd_address1", typeof(System.String));
        //    AddressDetaildt.Columns.Add("OrderAdd_address2", typeof(System.String));
        //    AddressDetaildt.Columns.Add("OrderAdd_address3", typeof(System.String));

        //    AddressDetaildt.Columns.Add("OrderAdd_landMark", typeof(System.String));
        //    AddressDetaildt.Columns.Add("OrderAdd_countryId", typeof(System.Int32));
        //    AddressDetaildt.Columns.Add("OrderAdd_stateId", typeof(System.Int32));
        //    AddressDetaildt.Columns.Add("OrderAdd_cityId", typeof(System.Int32));
        //    AddressDetaildt.Columns.Add("OrderAdd_areaId", typeof(System.Int32));

        //    AddressDetaildt.Columns.Add("OrderAdd_pin", typeof(System.String));
        //    AddressDetaildt.Columns.Add("OrderAdd_CreatedDate", typeof(System.DateTime));
        //    AddressDetaildt.Columns.Add("OrderAdd_CreatedUser", typeof(System.Int32));
        //    AddressDetaildt.Columns.Add("OrderAdd_LastModifyDate", typeof(System.DateTime));
        //    AddressDetaildt.Columns.Add("OrderAdd_LastModifyUser", typeof(System.Int32));
        //    return AddressDetaildt;

        //}

        //// Date: 30-05-2017    Author: Kallol Samanta  [END]











        //// Date: 30-05-2017    Author: Kallol Samanta  [START] 
        //// Details: Billing/Shipping user control integration
        protected void ComponentPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            //string customerid = hdnCustomerId.Value;
            //Populatecountry();

            //#region Addresss Lookup Section Start
            //DataSet dst = new DataSet();
            //CRMSalesDtlBL objCRMSalesDtlBL = new CRMSalesDtlBL();
            //dst = objCRMSalesDtlBL.PopulateBillingandShippingDetailByCustomerID(hdnCustomerId.Value);
            //billingAddress.DataSource = dst.Tables[0];
            //billingAddress.DataBind();
            //if (dst.Tables[0].Rows.Count > 0)
            //{
            //    Session["SalesOrderBillingAddressLookup"] = dst.Tables[0];
            //}
            //shippingAddress.DataSource = dst.Tables[1];
            //shippingAddress.DataBind();
            //if (dst.Tables[1].Rows.Count > 0)
            //{
            //    Session["SalesOrdeShippingAddressLookup"] = dst.Tables[1];
            //}


            //#endregion Addresss Lookup Section End

            //#region Variable Declaration to send value using jsproperties Start
            //string add_addressType = "";
            //string add_address1 = "";
            //string add_address2 = "";
            //string add_address3 = "";
            //string add_landMark = "";
            //string add_country = "";
            //string add_state = "";
            //string add_city = "";
            //string add_pin = "";
            //string add_area = "";

            /////// shipping variable

            //string add_saddressType = "";
            //string add_saddress1 = "";
            //string add_saddress2 = "";
            //string add_saddress3 = "";
            //string add_slandMark = "";
            //string add_scountry = "";
            //string add_sstate = "";
            //string add_scity = "";
            //string add_spin = "";
            //string add_sarea = "";

            //#endregion Variable Declaration to send value using jsproperties Start
            //ComponentPanel.JSProperties["cpshow"] = null;
            //ComponentPanel.JSProperties["cpshowShip"] = null;
            //string WhichCall = e.Parameter.Split('~')[0];

            //#region BillingLookup Edit Section Start
            //if (WhichCall == "BlookupEdit")
            //{
            //    int BillingAddressID = Convert.ToInt32(e.Parameter.Split('~')[1]);
            //    DataTable dt = objCRMSalesDtlBL.PopulateAddressDetailByAddressId(BillingAddressID);
            //    if (dt.Rows.Count > 0 && dt != null)
            //    {
            //        //for (int m = 0; m < dtaddbill.Rows.Count; m++)
            //        //{
            //        add_addressType = Convert.ToString(dt.Rows[0]["add_addressType"]);
            //        add_address1 = Convert.ToString(dt.Rows[0]["add_address1"]);
            //        add_address2 = Convert.ToString(dt.Rows[0]["add_address2"]);
            //        add_address3 = Convert.ToString(dt.Rows[0]["add_address3"]);
            //        add_landMark = Convert.ToString(dt.Rows[0]["add_landMark"]);
            //        add_country = Convert.ToString(dt.Rows[0]["add_country"]);
            //        add_state = Convert.ToString(dt.Rows[0]["add_state"]);
            //        add_city = Convert.ToString(dt.Rows[0]["add_city"]);
            //        add_pin = Convert.ToString(dt.Rows[0]["add_pin"]);
            //        add_area = Convert.ToString(dt.Rows[0]["add_area"]);

            //        PopulateBilling(add_address1, add_address2, add_address3, add_landMark, add_country, add_state, add_city, add_pin, add_area);

            //        ComponentPanel.JSProperties["cpshow"] = add_addressType + "~"
            //                                           + add_address1 + "~"
            //                                           + add_address2 + "~"
            //                                           + add_address3 + "~"
            //                                           + add_landMark + "~"
            //                                           + add_country + "~"
            //                                           + add_state + "~"
            //                                           + add_city + "~"
            //                                           + add_pin + "~"
            //                                           + add_area + "~"
            //                                           + "Y" + "~";

            //    }
            //    else
            //    {
            //        ComponentPanel.JSProperties["cpshow"] = add_addressType + "~"
            //                                          + add_address1 + "~"
            //                                          + add_address2 + "~"
            //                                          + add_address3 + "~"
            //                                          + add_landMark + "~"
            //                                          + add_country + "~"
            //                                          + add_state + "~"
            //                                          + add_city + "~"
            //                                          + add_pin + "~"
            //                                          + add_area + "~"
            //                                           + "N" + "~";
            //    }

            //}



            //#endregion BillingLookup Edit Section Start

            //#region ShippingLookup Edit Section Start
            //if (WhichCall == "SlookupEdit")
            //{
            //    int AddressID = Convert.ToInt32(e.Parameter.Split('~')[1]);
            //    DataTable dt = objCRMSalesDtlBL.PopulateAddressDetailByAddressId(AddressID);
            //    if (dt.Rows.Count > 0 && dt != null)
            //    {
            //        add_saddressType = Convert.ToString(dt.Rows[0]["add_addressType"]);
            //        add_saddress1 = Convert.ToString(dt.Rows[0]["add_address1"]);
            //        add_saddress2 = Convert.ToString(dt.Rows[0]["add_address2"]);
            //        add_saddress3 = Convert.ToString(dt.Rows[0]["add_address3"]);
            //        add_slandMark = Convert.ToString(dt.Rows[0]["add_landMark"]);
            //        add_scountry = Convert.ToString(dt.Rows[0]["add_country"]);
            //        add_sstate = Convert.ToString(dt.Rows[0]["add_state"]);
            //        add_scity = Convert.ToString(dt.Rows[0]["add_city"]);
            //        add_spin = Convert.ToString(dt.Rows[0]["add_pin"]);
            //        add_sarea = Convert.ToString(dt.Rows[0]["add_area"]);

            //        PopulateShipping(add_saddress1, add_saddress2, add_saddress3, add_slandMark, add_scountry, add_sstate, add_scity, add_spin, add_sarea);

            //        ComponentPanel.JSProperties["cpshowShip"] = add_saddressType + "~"
            //                                          + add_saddress1 + "~"
            //                                          + add_saddress2 + "~"
            //                                          + add_saddress3 + "~"
            //                                          + add_slandMark + "~"
            //                                          + add_scountry + "~"
            //                                          + add_sstate + "~"
            //                                          + add_scity + "~"
            //                                          + add_spin + "~"
            //                                          + add_sarea + "~"
            //                                          + "Y" + "~";

            //    }
            //    else
            //    {
            //        ComponentPanel.JSProperties["cpshowShip"] = add_saddressType + "~"
            //                                          + add_saddress1 + "~"
            //                                          + add_saddress2 + "~"
            //                                          + add_saddress3 + "~"
            //                                          + add_slandMark + "~"
            //                                          + add_scountry + "~"
            //                                          + add_sstate + "~"
            //                                          + add_scity + "~"
            //                                          + add_spin + "~"
            //                                          + add_sarea + "~"
            //                                           + "N" + "~";
            //    }

            //}

            //#endregion ShippingLookup Edit Section End
            //#region Edit Section of Address Start

            //if (WhichCall == "Edit")
            //{
            //    //DataTable dtaddress=(DataTable)
            //    //string AddressStatus = Convert.ToString(e.Parameter.Split('~')[1]);
            //    if (Session["SalesOrderAddressDtl"] == null)
            //    {
            //        string customerid1 = hdnCustomerId.Value;
            //        #region Billing Detail fillup
            //        DataTable dtaddbill = oDBEngine.GetDataTable("select add_addressType,add_address1,add_address2,add_address3,add_landMark,add_country,add_state,add_city,add_pin,add_area from tbl_master_address where add_cntId='" + customerid + "' and add_addressType='Billing' and Isdefault='1' ");

            //        #region Function To get All Detail

            //        if (dtaddbill.Rows.Count > 0 && dtaddbill != null)
            //        {

            //            //for (int m = 0; m < dtaddbill.Rows.Count; m++)
            //            //{
            //            add_addressType = Convert.ToString(dtaddbill.Rows[0]["add_addressType"]);
            //            add_address1 = Convert.ToString(dtaddbill.Rows[0]["add_address1"]);
            //            add_address2 = Convert.ToString(dtaddbill.Rows[0]["add_address2"]);
            //            add_address3 = Convert.ToString(dtaddbill.Rows[0]["add_address3"]);
            //            add_landMark = Convert.ToString(dtaddbill.Rows[0]["add_landMark"]);
            //            add_country = Convert.ToString(dtaddbill.Rows[0]["add_country"]);
            //            add_state = Convert.ToString(dtaddbill.Rows[0]["add_state"]);
            //            add_city = Convert.ToString(dtaddbill.Rows[0]["add_city"]);
            //            add_pin = Convert.ToString(dtaddbill.Rows[0]["add_pin"]);
            //            add_area = Convert.ToString(dtaddbill.Rows[0]["add_area"]);

            //            //}

            //            PopulateBilling(add_address1, add_address2, add_address3, add_landMark, add_country, add_state, add_city, add_pin, add_area);
            //            ComponentPanel.JSProperties["cpshow"] = add_addressType + "~"
            //                                               + add_address1 + "~"
            //                                               + add_address2 + "~"
            //                                               + add_address3 + "~"
            //                                               + add_landMark + "~"
            //                                               + add_country + "~"
            //                                               + add_state + "~"
            //                                               + add_city + "~"
            //                                               + add_pin + "~"
            //                                               + add_area + "~"
            //                                               + "Y" + "~";

            //        }
            //        else
            //        {
            //            ComponentPanel.JSProperties["cpshow"] = add_addressType + "~"
            //                                              + add_address1 + "~"
            //                                              + add_address2 + "~"
            //                                              + add_address3 + "~"
            //                                              + add_landMark + "~"
            //                                              + add_country + "~"
            //                                              + add_state + "~"
            //                                              + add_city + "~"
            //                                              + add_pin + "~"
            //                                              + add_area + "~"
            //                                               + "N" + "~";
            //        }
            //        #endregion Function Calling End
            //        #endregion Billing Detail fillup end
            //        #region Shipping Detail fillup
            //        DataTable dtaship = oDBEngine.GetDataTable("select add_addressType,add_address1,add_address2,add_address3,add_landMark,add_country,add_state,add_city,add_pin,add_area from tbl_master_address where add_cntId='" + customerid + "' and add_addressType='Shipping' and Isdefault='1' ");
            //        if (dtaship.Rows.Count > 0 && dtaship != null)
            //        {
            //            add_saddressType = Convert.ToString(dtaship.Rows[0]["add_addressType"]);
            //            add_saddress1 = Convert.ToString(dtaship.Rows[0]["add_address1"]);
            //            add_saddress2 = Convert.ToString(dtaship.Rows[0]["add_address2"]);
            //            add_saddress3 = Convert.ToString(dtaship.Rows[0]["add_address3"]);
            //            add_slandMark = Convert.ToString(dtaship.Rows[0]["add_landMark"]);
            //            add_scountry = Convert.ToString(dtaship.Rows[0]["add_country"]);
            //            add_sstate = Convert.ToString(dtaship.Rows[0]["add_state"]);
            //            add_scity = Convert.ToString(dtaship.Rows[0]["add_city"]);
            //            add_spin = Convert.ToString(dtaship.Rows[0]["add_pin"]);
            //            add_sarea = Convert.ToString(dtaship.Rows[0]["add_area"]);


            //            PopulateShipping(add_saddress1, add_saddress2, add_saddress3, add_slandMark, add_scountry, add_sstate, add_scity, add_spin, add_sarea);
            //            ComponentPanel.JSProperties["cpshowShip"] = add_saddressType + "~"
            //                                              + add_saddress1 + "~"
            //                                              + add_saddress2 + "~"
            //                                              + add_saddress3 + "~"
            //                                              + add_slandMark + "~"
            //                                              + add_scountry + "~"
            //                                              + add_sstate + "~"
            //                                              + add_scity + "~"
            //                                              + add_spin + "~"
            //                                              + add_sarea + "~"
            //                                              + "Y" + "~";

            //        }
            //        else
            //        {
            //            ComponentPanel.JSProperties["cpshowShip"] = add_saddressType + "~"
            //                                              + add_saddress1 + "~"
            //                                              + add_saddress2 + "~"
            //                                              + add_saddress3 + "~"
            //                                              + add_slandMark + "~"
            //                                              + add_scountry + "~"
            //                                              + add_sstate + "~"
            //                                              + add_scity + "~"
            //                                              + add_spin + "~"
            //                                              + add_sarea + "~"
            //                                               + "N" + "~";
            //        }
            //        #endregion Shipping detail Fillup
            //    }
            //    else if (Session["SalesOrderAddressDtl"] != null)
            //    {
            //        DataTable dt = (DataTable)Session["SalesOrderAddressDtl"];
            //        if (dt.Rows.Count > 0)
            //        {
            //            if (dt.Rows.Count == 2) // when 2 row  data found in edit mode
            //            {
            //                #region billing Address Dtl using session
            //                add_addressType = Convert.ToString(dt.Rows[0]["OrderAdd_addressType"]);
            //                add_address1 = Convert.ToString(dt.Rows[0]["OrderAdd_address1"]);
            //                add_address2 = Convert.ToString(dt.Rows[0]["OrderAdd_address2"]);
            //                add_address3 = Convert.ToString(dt.Rows[0]["OrderAdd_address3"]);
            //                add_landMark = Convert.ToString(dt.Rows[0]["OrderAdd_landMark"]);
            //                add_country = Convert.ToString(dt.Rows[0]["OrderAdd_countryId"]);
            //                add_state = Convert.ToString(dt.Rows[0]["OrderAdd_stateId"]);
            //                add_city = Convert.ToString(dt.Rows[0]["OrderAdd_cityId"]);
            //                add_pin = Convert.ToString(dt.Rows[0]["OrderAdd_pin"]);
            //                add_area = Convert.ToString(dt.Rows[0]["OrderAdd_areaId"]);

            //                PopulateBilling(add_address1, add_address2, add_address3, add_landMark, add_country, add_state, add_city, add_pin, add_area);

            //                ComponentPanel.JSProperties["cpshow"] = add_addressType + "~"
            //                                                      + add_address1 + "~"
            //                                                      + add_address2 + "~"
            //                                                      + add_address3 + "~"
            //                                                      + add_landMark + "~"
            //                                                      + add_country + "~"
            //                                                      + add_state + "~"
            //                                                      + add_city + "~"
            //                                                      + add_pin + "~"
            //                                                      + add_area + "~"
            //                                                      + "Y" + "~";
            //                #endregion billing Address Dtl using session
            //                #region Shipping Address Dtl using session
            //                add_saddressType = Convert.ToString(dt.Rows[1]["OrderAdd_addressType"]);
            //                add_saddress1 = Convert.ToString(dt.Rows[1]["OrderAdd_address1"]);
            //                add_saddress2 = Convert.ToString(dt.Rows[1]["OrderAdd_address2"]);
            //                add_saddress3 = Convert.ToString(dt.Rows[1]["OrderAdd_address3"]);
            //                add_slandMark = Convert.ToString(dt.Rows[1]["OrderAdd_landMark"]);
            //                add_scountry = Convert.ToString(dt.Rows[1]["OrderAdd_countryId"]);
            //                add_sstate = Convert.ToString(dt.Rows[1]["OrderAdd_stateId"]);
            //                add_scity = Convert.ToString(dt.Rows[1]["OrderAdd_cityId"]);
            //                add_spin = Convert.ToString(dt.Rows[1]["OrderAdd_pin"]);
            //                add_sarea = Convert.ToString(dt.Rows[1]["OrderAdd_areaId"]);

            //                PopulateShipping(add_saddress1, add_saddress2, add_saddress3, add_slandMark, add_scountry, add_sstate, add_scity, add_spin, add_sarea);


            //                ComponentPanel.JSProperties["cpshowShip"] = add_saddressType + "~"
            //                                                     + add_saddress1 + "~"
            //                                                     + add_saddress2 + "~"
            //                                                     + add_saddress3 + "~"
            //                                                     + add_slandMark + "~"
            //                                                     + add_scountry + "~"
            //                                                     + add_sstate + "~"
            //                                                     + add_scity + "~"
            //                                                     + add_spin + "~"
            //                                                     + add_sarea + "~"
            //                                                     + "Y" + "~";
            //                #endregion Shipping Address Dtl using session end

            //            }
            //            else if (dt.Rows.Count == 1) // when 1 row  data found in edit mode
            //            {
            //                if (Convert.ToString(dt.Rows[0]["OrderAdd_addressType"]) == "Billing")
            //                {
            //                    #region billing Address Dtl using session
            //                    add_addressType = Convert.ToString(dt.Rows[0]["OrderAdd_addressType"]);
            //                    add_address1 = Convert.ToString(dt.Rows[0]["OrderAdd_address1"]);
            //                    add_address2 = Convert.ToString(dt.Rows[0]["OrderAdd_address2"]);
            //                    add_address3 = Convert.ToString(dt.Rows[0]["OrderAdd_address3"]);
            //                    add_landMark = Convert.ToString(dt.Rows[0]["OrderAdd_landMark"]);
            //                    add_country = Convert.ToString(dt.Rows[0]["OrderAdd_countryId"]);
            //                    add_state = Convert.ToString(dt.Rows[0]["OrderAdd_stateId"]);
            //                    add_city = Convert.ToString(dt.Rows[0]["OrderAdd_cityId"]);
            //                    add_pin = Convert.ToString(dt.Rows[0]["OrderAdd_pin"]);
            //                    add_area = Convert.ToString(dt.Rows[0]["OrderAdd_areaId"]);

            //                    PopulateBilling(add_address1, add_address2, add_address3, add_landMark, add_country, add_state, add_city, add_pin, add_area);

            //                    ComponentPanel.JSProperties["cpshow"] = add_addressType + "~"
            //                                                          + add_address1 + "~"
            //                                                          + add_address2 + "~"
            //                                                          + add_address3 + "~"
            //                                                          + add_landMark + "~"
            //                                                          + add_country + "~"
            //                                                          + add_state + "~"
            //                                                          + add_city + "~"
            //                                                          + add_pin + "~"
            //                                                          + add_area + "~"
            //                                                          + "Y" + "~";

            //                    ComponentPanel.JSProperties["cpshowShip"] = add_saddressType + "~"
            //                                         + add_saddress1 + "~"
            //                                         + add_saddress2 + "~"
            //                                         + add_saddress3 + "~"
            //                                         + add_slandMark + "~"
            //                                         + add_scountry + "~"
            //                                         + add_sstate + "~"
            //                                         + add_scity + "~"
            //                                         + add_spin + "~"
            //                                         + add_sarea + "~"
            //                                          + "N" + "~";

            //                    #endregion billing Address Dtl using session
            //                }
            //                if (Convert.ToString(dt.Rows[0]["OrderAdd_addressType"]) == "Shipping")
            //                {
            //                    #region Shipping Address Dtl using session
            //                    ComponentPanel.JSProperties["cpshow"] = add_addressType + "~"
            //                                         + add_address1 + "~"
            //                                         + add_address2 + "~"
            //                                         + add_address3 + "~"
            //                                         + add_landMark + "~"
            //                                         + add_country + "~"
            //                                         + add_state + "~"
            //                                         + add_city + "~"
            //                                         + add_pin + "~"
            //                                         + add_area + "~"
            //                                          + "N" + "~";

            //                    add_saddressType = Convert.ToString(dt.Rows[0]["OrderAdd_addressType"]);
            //                    add_saddress1 = Convert.ToString(dt.Rows[0]["OrderAdd_address1"]);
            //                    add_saddress2 = Convert.ToString(dt.Rows[0]["OrderAdd_address2"]);
            //                    add_saddress3 = Convert.ToString(dt.Rows[0]["OrderAdd_address3"]);
            //                    add_slandMark = Convert.ToString(dt.Rows[0]["OrderAdd_landMark"]);
            //                    add_scountry = Convert.ToString(dt.Rows[0]["OrderAdd_countryId"]);
            //                    add_sstate = Convert.ToString(dt.Rows[0]["OrderAdd_stateId"]);
            //                    add_scity = Convert.ToString(dt.Rows[0]["OrderAdd_cityId"]);
            //                    add_spin = Convert.ToString(dt.Rows[0]["OrderAdd_pin"]);
            //                    add_sarea = Convert.ToString(dt.Rows[0]["OrderAdd_areaId"]);


            //                    PopulateShipping(add_saddress1, add_saddress2, add_saddress3, add_slandMark, add_scountry, add_sstate, add_scity, add_spin, add_sarea);


            //                    ComponentPanel.JSProperties["cpshowShip"] = add_saddressType + "~"
            //                                                         + add_saddress1 + "~"
            //                                                         + add_saddress2 + "~"
            //                                                         + add_saddress3 + "~"
            //                                                         + add_slandMark + "~"
            //                                                         + add_scountry + "~"
            //                                                         + add_sstate + "~"
            //                                                         + add_scity + "~"
            //                                                         + add_spin + "~"
            //                                                         + add_sarea + "~"
            //                                                         + "Y" + "~";
            //                    #endregion Shipping Address Dtl using session end
            //                }
            //            }
            //            else // when no data found in edit mode
            //            {
            //                #region billing Address Dtl using session
            //                //add_addressType = Convert.ToString(dt.Rows[0]["QuoteAdd_addressType"]);
            //                //add_address1 = Convert.ToString(dt.Rows[0]["QuoteAdd_address1"]);
            //                //add_address2 = Convert.ToString(dt.Rows[0]["QuoteAdd_address2"]);
            //                //add_address3 = Convert.ToString(dt.Rows[0]["QuoteAdd_address3"]);
            //                //add_landMark = Convert.ToString(dt.Rows[0]["QuoteAdd_landMark"]);
            //                //add_country = Convert.ToString(dt.Rows[0]["QuoteAdd_countryId"]);
            //                //add_state = Convert.ToString(dt.Rows[0]["QuoteAdd_stateId"]);
            //                //add_city = Convert.ToString(dt.Rows[0]["QuoteAdd_cityId"]);
            //                //add_pin = Convert.ToString(dt.Rows[0]["QuoteAdd_pin"]);
            //                //add_area = Convert.ToString(dt.Rows[0]["QuoteAdd_areaId"]);
            //                ComponentPanel.JSProperties["cpshow"] = add_addressType + "~"
            //                                                      + add_address1 + "~"
            //                                                      + add_address2 + "~"
            //                                                      + add_address3 + "~"
            //                                                      + add_landMark + "~"
            //                                                      + add_country + "~"
            //                                                      + add_state + "~"
            //                                                      + add_city + "~"
            //                                                      + add_pin + "~"
            //                                                      + add_area + "~"
            //                                                      + "Y" + "~";
            //                #endregion billing Address Dtl using session
            //                #region Shipping Address Dtl using session
            //                //add_saddressType = Convert.ToString(dt.Rows[1]["QuoteAdd_addressType"]);
            //                //add_saddress1 = Convert.ToString(dt.Rows[1]["QuoteAdd_address1"]);
            //                //add_saddress2 = Convert.ToString(dt.Rows[1]["QuoteAdd_address2"]);
            //                //add_saddress3 = Convert.ToString(dt.Rows[1]["QuoteAdd_address3"]);
            //                //add_slandMark = Convert.ToString(dt.Rows[1]["QuoteAdd_landMark"]);
            //                //add_scountry = Convert.ToString(dt.Rows[1]["QuoteAdd_countryId"]);
            //                //add_sstate = Convert.ToString(dt.Rows[1]["QuoteAdd_stateId"]);
            //                //add_scity = Convert.ToString(dt.Rows[1]["QuoteAdd_cityId"]);
            //                //add_spin = Convert.ToString(dt.Rows[1]["QuoteAdd_pin"]);
            //                //add_sarea = Convert.ToString(dt.Rows[1]["QuoteAdd_areaId"]);
            //                ComponentPanel.JSProperties["cpshowShip"] = add_saddressType + "~"
            //                                                     + add_saddress1 + "~"
            //                                                     + add_saddress2 + "~"
            //                                                     + add_saddress3 + "~"
            //                                                     + add_slandMark + "~"
            //                                                     + add_scountry + "~"
            //                                                     + add_sstate + "~"
            //                                                     + add_scity + "~"
            //                                                     + add_spin + "~"
            //                                                     + add_sarea + "~"
            //                                                     + "Y" + "~";

            //                #endregion Shipping Address Dtl using session end

            //            }
            //        }
            //    }
            //}
            //#endregion Edit Section of Address End

            //#region Save Section of Address Start
            //if (WhichCall == "save")
            //{
            //    #region Global Data for Address Start
            //    string companyId = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            //    int branchid = Convert.ToInt32(HttpContext.Current.Session["userbranchID"]);
            //    string fin_year = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
            //    int userid = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            //    #endregion Global Data for Address End
            //    string AddressStatus = e.Parameter.Split('~')[1];
            //    if (AddressStatus == "1") // Both Billing and Shipping Address Available
            //    {
            //        #region Billing Address Detail Start
            //        string contactperson = "";


            //        //int insertcount = 0;

            //        //string AddressType = Convert.ToString(CmbAddressType.SelectedItem.Value);
            //        string AddressType = "Billing";
            //        string address1 = txtAddress1.Text;
            //        string address2 = txtAddress2.Text;
            //        string address3 = txtAddress3.Text;
            //        string landmark = txtlandmark.Text;
            //        int country = Convert.ToInt32(CmbCountry.SelectedItem.Value);
            //        int State = Convert.ToInt32(CmbState.Value);
            //        int city = Convert.ToInt32(CmbCity.Value);
            //        int area = Convert.ToInt32(CmbArea.Value);
            //        string pin = Convert.ToString(CmbPin.Value);
            //        DataTable dt = StoreSalesOrderAddressDetail();
            //        dt.Rows.Add(0, companyId, branchid, fin_year, "", AddressType, address1, address2, address3, landmark, country, State, city, area, pin, System.DateTime.Now, userid, System.DateTime.Now, userid);




            //        #endregion Billing Address Detail Start end

            //        #region Shipping Address Detail Start
            //        // CRMSalesAddressEL objCRMSalesSAddress  = new CRMSalesAddressEL();
            //        string scontactperson = "";
            //        //string sAddressType = Convert.ToString(CmbAddressType1.SelectedItem.Value);
            //        string sAddressType = "Shipping";
            //        string saddress1 = txtsAddress1.Text;
            //        string saddress2 = txtsAddress2.Text;
            //        string saddress3 = txtsAddress3.Text;
            //        string slandmark = txtslandmark.Text;
            //        int scountry = Convert.ToInt32(CmbCountry1.Value);
            //        int sState = Convert.ToInt32(CmbState1.Value);
            //        int scity = Convert.ToInt32(CmbCity1.Value);
            //        int sarea = Convert.ToInt32(CmbArea1.Value);
            //        string spin = Convert.ToString(CmbPin1.Value);
            //        dt.Rows.Add(0, companyId, branchid, fin_year, "", sAddressType, saddress1, saddress2, saddress3, slandmark, scountry, sState, scity, sarea, spin, System.DateTime.Now, userid, System.DateTime.Now, userid);


            //        Session["SalesOrderAddressDtl"] = dt;
            //        #endregion Shipping Address Detail Start end
            //    }
            //    else if (AddressStatus == "2") // Copy Billing to Shipping Address
            //    {
            //        //string AddressType = Convert.ToString(CmbAddressType.SelectedItem.Value);
            //        string AddressType = "Billing";
            //        string address1 = txtAddress1.Text;
            //        string address2 = txtAddress2.Text;
            //        string address3 = txtAddress3.Text;
            //        string landmark = txtlandmark.Text;
            //        string CountryTrim = Convert.ToString(CmbCountry.Value);
            //        int country = Convert.ToInt32(CountryTrim);
            //        string StateTrim = Convert.ToString(CmbState.Value);
            //        int State = Convert.ToInt32(StateTrim);
            //        string CityTrim = Convert.ToString(CmbCity.Value);
            //        int city = Convert.ToInt32(CityTrim);
            //        string areaTrim = Convert.ToString(CmbArea.Value);
            //        int area = 0;
            //        if (!string.IsNullOrEmpty(areaTrim))
            //        { area = Convert.ToInt32(areaTrim); }


            //        string pinTrim = Convert.ToString(CmbPin.Value);
            //        string pin = Convert.ToString(pinTrim);
            //        DataTable dt = StoreSalesOrderAddressDetail();
            //        dt.Rows.Add(0, companyId, branchid, fin_year, "", AddressType, address1, address2, address3, landmark, country, State, city, area, pin, System.DateTime.Now, userid, System.DateTime.Now, userid);
            //        dt.Rows.Add(0, companyId, branchid, fin_year, "", "Shipping", address1, address2, address3, landmark, country, State, city, area, pin, System.DateTime.Now, userid, System.DateTime.Now, userid);
            //        Session["SalesOrderAddressDtl"] = dt;
            //    }
            //    else if (AddressStatus == "3") // Copy  Shipping to Billing  Address
            //    {
            //        string scontactperson = "";
            //        //string sAddressType = Convert.ToString(CmbAddressType1.SelectedItem.Value);
            //        string sAddressType = "Shipping";
            //        string saddress1 = txtsAddress1.Text;
            //        string saddress2 = txtsAddress2.Text;
            //        string saddress3 = txtsAddress3.Text;
            //        string slandmark = txtslandmark.Text;
            //        int scountry = Convert.ToInt32(CmbCountry1.SelectedItem.Value);
            //        int sState = Convert.ToInt32(CmbState1.Value);
            //        int scity = Convert.ToInt32(CmbCity1.Value);
            //        int sarea = Convert.ToInt32(CmbArea1.Value);
            //        string spin = Convert.ToString(CmbPin1.Value);
            //        DataTable dt = StoreSalesOrderAddressDetail();
            //        dt.Rows.Add(0, companyId, branchid, fin_year, "", "Billing", saddress1, saddress2, saddress3, slandmark, scountry, sState, scity, sarea, spin, System.DateTime.Now, userid, System.DateTime.Now, userid);
            //        dt.Rows.Add(0, companyId, branchid, fin_year, "", sAddressType, saddress1, saddress2, saddress3, slandmark, scountry, sState, scity, sarea, spin, System.DateTime.Now, userid, System.DateTime.Now, userid);
            //        Session["SalesOrderAddressDtl"] = dt;
            //    }

            //}

            //#endregion Save Section of Address Start

        }

        //// Date: 30-05-2017    Author: Kallol Samanta  [END]








        //// Date: 30-05-2017    Author: Kallol Samanta  [START] 
        //// Details: Billing/Shipping user control integration


        //#region populate Billing Address
        //public void PopulateBilling(string add_address1, string add_address2, string add_address3, string add_landMark, string add_country, string add_state, string add_city, string add_pin, string add_area)
        //{
        //    DataTable dtcountry = new DataTable();

        //    dtcountry = oDBEngine.GetDataTable("SELECT cou_id, cou_country as Country FROM tbl_master_country order by cou_country");

        //    CmbCountry.DataSource = dtcountry;
        //    CmbCountry.TextField = "Country";
        //    CmbCountry.ValueField = "cou_id";
        //    CmbCountry.DataBind();
        //    if (!string.IsNullOrEmpty(add_country))
        //    {
        //        CmbCountry.Value = add_country;
        //    }
        //    DataTable dtstate = new DataTable();

        //    dtstate = oDBEngine.GetDataTable("SELECT s.id as ID,s.state+' (State Code:' +s.StateCode+')' as State from tbl_master_state s where (s.countryId = " + add_country + ") ORDER BY s.state");



        //    CmbState.DataSource = dtstate;
        //    CmbState.TextField = "State";
        //    CmbState.ValueField = "ID";
        //    CmbState.DataBind();
        //    if (!string.IsNullOrEmpty(add_state))
        //    {
        //        CmbState.Value = add_state;
        //    }
        //    DataTable dtcity = new DataTable();

        //    dtcity = oDBEngine.GetDataTable("SELECT c.city_id AS CityId, c.city_name AS City FROM tbl_master_city c where c.state_id=" + add_state + " order by c.city_name");


        //    CmbCity.DataSource = dtcity;
        //    CmbCity.TextField = "City";
        //    CmbCity.ValueField = "CityId";
        //    CmbCity.DataBind();

        //    if (!string.IsNullOrEmpty(add_city))
        //    {
        //        CmbCity.Value = add_city;

        //    }
        //    DataTable dtpin = new DataTable();
        //    dtpin = oDBEngine.GetDataTable("select pin_id,pin_code from tbl_master_pinzip where city_id=" + add_city + " order by pin_code");


        //    CmbPin.DataSource = dtpin;
        //    CmbPin.TextField = "pin_code";
        //    CmbPin.ValueField = "pin_id";
        //    CmbPin.DataBind();

        //    if (!string.IsNullOrEmpty(add_pin))
        //    {
        //        CmbPin.Value = add_pin;
        //    }

        //    DataTable dtarea = new DataTable();
        //    dtarea = oDBEngine.GetDataTable("SELECT area_id, area_name from tbl_master_area where (city_id = " + add_city + ") ORDER BY area_name");

        //    CmbArea.DataSource = dtarea;
        //    CmbArea.TextField = "area_name";
        //    CmbArea.ValueField = "area_id";
        //    CmbArea.DataBind();

        //    CmbArea.Value = add_area;



        //    txtAddress1.Text = add_address1;
        //    txtAddress2.Text = add_address2;
        //    txtAddress3.Text = add_address3;
        //    txtlandmark.Text = add_landMark;

        //}
        //#endregion

        //#region populate Shipping Address
        //public void PopulateShipping(string add_saddress1, string add_saddress2, string add_saddress3, string add_slandMark, string add_scountry, string add_sstate, string add_scity, string add_spin, string add_sarea)
        //{
        //    DataTable dtcountry1 = new DataTable();

        //    dtcountry1 = oDBEngine.GetDataTable("SELECT cou_id, cou_country as Country FROM tbl_master_country order by cou_country");

        //    CmbCountry1.DataSource = dtcountry1;
        //    CmbCountry1.TextField = "Country";
        //    CmbCountry1.ValueField = "cou_id";
        //    CmbCountry1.DataBind();
        //    if (!string.IsNullOrEmpty(add_scountry))
        //    {
        //        CmbCountry1.Value = add_scountry;
        //    }
        //    DataTable dtstate1 = new DataTable();

        //    dtstate1 = oDBEngine.GetDataTable("SELECT s.id as ID,s.state+' (State Code:' +s.StateCode+')' as State from tbl_master_state s where (s.countryId = " + add_scountry + ") ORDER BY s.state");


        //    CmbState1.DataSource = dtstate1;
        //    CmbState1.TextField = "State";
        //    CmbState1.ValueField = "ID";
        //    CmbState1.DataBind();
        //    if (!string.IsNullOrEmpty(add_sstate))
        //    {
        //        CmbState1.Value = add_sstate;
        //    }

        //    DataTable dtcity1 = new DataTable();
        //    dtcity1 = oDBEngine.GetDataTable("SELECT c.city_id AS CityId, c.city_name AS City FROM tbl_master_city c where c.state_id=" + add_sstate + " order by c.city_name");

        //    CmbCity1.DataSource = dtcity1;
        //    CmbCity1.TextField = "City";
        //    CmbCity1.ValueField = "CityId";
        //    CmbCity1.DataBind();
        //    if (!string.IsNullOrEmpty(add_scity))
        //    {
        //        CmbCity1.Value = add_scity;
        //    }


        //    DataTable dtpin1 = new DataTable();
        //    dtpin1 = oDBEngine.GetDataTable("select pin_id,pin_code from tbl_master_pinzip where city_id=" + add_scity + " order by pin_code");


        //    CmbPin1.DataSource = dtpin1;
        //    CmbPin1.TextField = "pin_code";
        //    CmbPin1.ValueField = "pin_id";
        //    CmbPin1.DataBind();
        //    if (!string.IsNullOrEmpty(add_spin))
        //    {
        //        CmbPin1.Value = add_spin;
        //    }

        //    DataTable dtarea1 = new DataTable();
        //    dtarea1 = oDBEngine.GetDataTable("SELECT area_id, area_name from tbl_master_area where (city_id = " + add_scity + ") ORDER BY area_name");

        //    CmbArea1.DataSource = dtarea1;
        //    CmbArea1.TextField = "area_name";
        //    CmbArea1.ValueField = "area_id";
        //    CmbArea1.DataBind();

        //    if (!string.IsNullOrEmpty(add_sarea))
        //    {
        //        CmbArea1.Value = add_sarea;
        //    }


        //    txtsAddress1.Text = add_saddress1;
        //    txtsAddress2.Text = add_saddress2;
        //    txtsAddress3.Text = add_saddress3;
        //    txtslandmark.Text = add_slandMark;

        //}
        //#endregion

        //#region Populate Country
        //public void Populatecountry()
        //{
        //    DataTable dtcountry = new DataTable();
        //    dtcountry = oDBEngine.GetDataTable("SELECT cou_id, cou_country as Country FROM tbl_master_country order by cou_country");
        //    CmbCountry.DataSource = dtcountry;
        //    CmbCountry.TextField = "Country";
        //    CmbCountry.ValueField = "cou_id";
        //    CmbCountry.DataBind();

        //    CmbCountry1.DataSource = dtcountry;
        //    CmbCountry1.TextField = "Country";
        //    CmbCountry1.ValueField = "cou_id";
        //    CmbCountry1.DataBind();
        //}
        //#endregion

        //// Date: 30-05-2017    Author: Kallol Samanta  [END] 

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
        public void SetOrderDetails()
        {
            DataSet OrderEdit = GetOrderEditData();
            DataTable OrderEditdt = OrderEdit.Tables[0];
            if (OrderEditdt != null && OrderEditdt.Rows.Count > 0)
            {
                string Branch_Id = Convert.ToString(OrderEditdt.Rows[0]["Order_BranchId"]);
                string Order_Number = Convert.ToString(OrderEditdt.Rows[0]["Order_Number"]);
                string Order_Date = Convert.ToString(OrderEditdt.Rows[0]["Order_Date"]);
                //New Added
                string Order_NumberingScheme = Convert.ToString(OrderEditdt.Rows[0]["Order_NumScheme"]);
                //End
                // string Order_Salesman=Convert.ToString(OrderEditdt.Rows[0]["Order_NumScheme"])
                string Order_OANumber = Convert.ToString(OrderEditdt.Rows[0]["Order_OANumber"]);
                string Order_OADate = Convert.ToString(OrderEditdt.Rows[0]["Order_OADate"]);
                string Quotation_Date = Convert.ToString(OrderEditdt.Rows[0]["Order_Quotation_Date"]);
                string Quotation_Number = Convert.ToString(OrderEditdt.Rows[0]["Order_Quotation"]);
                string Order_Expiry = Convert.ToString(OrderEditdt.Rows[0]["Order_Expiry"]);
                string Customer_Id = Convert.ToString(OrderEditdt.Rows[0]["Customer_Id"]);
                hdnCustomerId.Value = Customer_Id;//added on 222-02-2017
                string Contact_Person_Id = Convert.ToString(OrderEditdt.Rows[0]["Contact_Person_Id"]);
                string Order_Reference = Convert.ToString(OrderEditdt.Rows[0]["Order_Reference"]);
                string Currency_Id = Convert.ToString(OrderEditdt.Rows[0]["Currency_Id"]);
                string Currency_Conversion_Rate = Convert.ToString(OrderEditdt.Rows[0]["Currency_Conversion_Rate"]);
                string Tax_Option = Convert.ToString(OrderEditdt.Rows[0]["Tax_Option"]);

                string CreditDueDate = Convert.ToString(OrderEditdt.Rows[0]["DueDate"]);
                string CreditDays = Convert.ToString(OrderEditdt.Rows[0]["CreditDays"]);

                string VehicleNumber = Convert.ToString(OrderEditdt.Rows[0]["VehicleNumber"]);
                string TransporterName = Convert.ToString(OrderEditdt.Rows[0]["TransporterName"]);
                string TransporterPhone = Convert.ToString(OrderEditdt.Rows[0]["TransporterPhone"]);
                string IsInvenotry = Convert.ToString(OrderEditdt.Rows[0]["IsInventory"]);
                string CustomerName = Convert.ToString(OrderEditdt.Rows[0]["CustomerName"]);

                ddl_PosGstSalesOrder.DataSource = OrderEdit.Tables[3];
                ddl_PosGstSalesOrder.ValueField = "ID";
                ddl_PosGstSalesOrder.TextField = "Name";
                ddl_PosGstSalesOrder.DataBind();


                string Segment1Name = Convert.ToString(OrderEditdt.Rows[0]["Segment1Name"]);
                string Segment2Name = Convert.ToString(OrderEditdt.Rows[0]["Segment2Name"]);
                string Segment3Name = Convert.ToString(OrderEditdt.Rows[0]["Segment3Name"]);
                string Segment4Name = Convert.ToString(OrderEditdt.Rows[0]["Segment4Name"]);
                string Segment5Name = Convert.ToString(OrderEditdt.Rows[0]["Segment5Name"]);

                string Segment1 = Convert.ToString(OrderEditdt.Rows[0]["Segment1"]);
                string Segment2 = Convert.ToString(OrderEditdt.Rows[0]["Segment2"]);
                string Segment3 = Convert.ToString(OrderEditdt.Rows[0]["Segment3"]);
                string Segment4 = Convert.ToString(OrderEditdt.Rows[0]["Segment4"]);
                string Segment5 = Convert.ToString(OrderEditdt.Rows[0]["Segment5"]);

                txtSegment1.Text = Segment1Name;
                txtSegment2.Text = Segment2Name;
                txtSegment3.Text = Segment3Name;
                txtSegment4.Text = Segment4Name;
                txtSegment5.Text = Segment5Name;


                hdnSegment1.Value = Segment1;
                hdnSegment2.Value = Segment2;
                hdnSegment3.Value = Segment3;
                hdnSegment4.Value = Segment4;
                hdnSegment5.Value = Segment5;

                string PosForGst = Convert.ToString(OrderEditdt.Rows[0]["PosForGst"]);

                ddl_PosGstSalesOrder.Value = PosForGst;

                Sales_BillingShipping.SetBillingShippingTable(OrderEdit.Tables[1]);

                string RevisionNo = Convert.ToString(OrderEditdt.Rows[0]["Order_RevisionNo"]);
                txtRevisionNo.Text = RevisionNo;
                string ApproveProjectRem = Convert.ToString(OrderEditdt.Rows[0]["Project_ApproveRejectREmarks"]);

                txtAppRejRemarks.Text = ApproveProjectRem;
                hdnApproveStatus.Value = Convert.ToString(OrderEditdt.Rows[0]["Project_ApproveStatus"]);
                string ProjectValidFrom = Convert.ToString(OrderEditdt.Rows[0]["Order_ProjectValidFrom"]);
                string ProjectValidUpto = Convert.ToString(OrderEditdt.Rows[0]["Order_ProjectValidUpto"]);

                //string Order_ProjectRemarks = Convert.ToString(OrderEditdt.Rows[0]["Order_ProjectRemarks"]);

                //txtProjRemarks.Text = Order_ProjectRemarks;
                if (!string.IsNullOrEmpty(Convert.ToString(OrderEditdt.Rows[0]["Order_ProjectValidFrom"])))
                {
                    dtProjValidFrom.Date = Convert.ToDateTime(OrderEditdt.Rows[0]["Order_ProjectValidFrom"]);

                }
                if (!string.IsNullOrEmpty(Convert.ToString(OrderEditdt.Rows[0]["Order_ProjectValidUpto"])))
                {
                    dtProjValidUpto.Date = Convert.ToDateTime(OrderEditdt.Rows[0]["Order_ProjectValidUpto"]);

                }

                if (!string.IsNullOrEmpty(Convert.ToString(OrderEditdt.Rows[0]["Order_RevisionDate"])))
                {
                    txtRevisionDate.Date = Convert.ToDateTime(OrderEditdt.Rows[0]["Order_RevisionDate"]);
                    txtRevisionDate.MinDate = Convert.ToDateTime(Convert.ToDateTime(OrderEditdt.Rows[0]["Order_RevisionDate"]).ToShortDateString());

                }
                else
                {
                    txtRevisionDate.Date = Convert.ToDateTime(Order_Date);
                    txtRevisionDate.MinDate = Convert.ToDateTime(Convert.ToDateTime(Order_Date).ToShortDateString());
                }

                if (hdnApproveStatus.Value == "1")
                {
                    lookup_Project.ClientEnabled = false;
                }



                if (!string.IsNullOrEmpty(IsInvenotry))
                {
                    if (IsInvenotry.ToUpper() == "0")
                    {
                        ddlInventory.SelectedValue = "N";
                    }
                    else if (IsInvenotry.ToUpper() == "TRUE")
                    {
                        ddlInventory.SelectedValue = "1";
                    }
                    else
                    {
                        ddlInventory.SelectedValue = IsInvenotry;
                    }

                    ddlInventory.Enabled = false;

                }

                //ASPxTextBox txtDriverName = (ASPxTextBox)VehicleDetailsControl.FindControl("txtDriverName");
                //ASPxTextBox txtPhoneNo = (ASPxTextBox)VehicleDetailsControl.FindControl("txtPhoneNo");
                //DropDownList ddl_VehicleNo = (DropDownList)VehicleDetailsControl.FindControl("ddl_VehicleNo");

                //txtDriverName.Text = TransporterName;
                //txtPhoneNo.Text = TransporterPhone;
                //ddl_VehicleNo.SelectedValue = VehicleNumber;


                txtCreditDays.Text = CreditDays;
                if (!string.IsNullOrEmpty(CreditDueDate))
                {
                    dt_SaleInvoiceDue.Date = Convert.ToDateTime(CreditDueDate);
                }
                string Doctype = Convert.ToString(OrderEditdt.Rows[0]["Doctype"]);
                string Quoids = Convert.ToString(OrderEditdt.Rows[0]["Order_Quotation_Ids"]);

                if (!String.IsNullOrEmpty(Quoids) && Quoids.Split(',')[0] != "0")
                {
                    Session["Lookup_QuotationIds"] = Quoids;
                    string[] eachQuo = Quoids.Split(',');
                    if (eachQuo.Length > 1)//More tha one quotation
                    {
                        lookup_Project.ClientEnabled = false;
                        if (Doctype == "QN")
                        {
                            rdl_Salesquotation.SelectedValue = "QN";
                            dt_Quotation.Text = "Multiple Select Quotation Dates";
                        }
                        if (Doctype == "SINQ")
                        {
                            rdl_Salesquotation.SelectedValue = "SINQ";
                            dt_Quotation.Text = "Multiple Select Inquiry Dates";
                        }

                        BindLookUp(Customer_Id, Order_Date, Doctype);
                        foreach (string val in eachQuo)
                        {
                            lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));

                        }
                        // lbl_MultipleDate.Attributes.Add("style", "display:block");
                    }
                    else if (eachQuo.Length == 1)//Single Quotation
                    { //lbl_MultipleDate.Attributes.Add("style", "display:none"); }
                        lookup_Project.ClientEnabled = false;
                        if (Doctype == "QN")
                        {
                            rdl_Salesquotation.SelectedValue = "QN";
                            //dt_Quotation.Text = "Multiple Select Quotation Dates";
                        }
                        if (Doctype == "SINQ")
                        {
                            rdl_Salesquotation.SelectedValue = "SINQ";
                            //dt_Quotation.Text = "Multiple Select Inquiry Dates";
                        }
                        BindLookUp(Customer_Id, Order_Date, Doctype);
                        foreach (string val in eachQuo)
                        {
                            lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                        }
                    }
                    else//No Quotation selected
                    {
                        BindLookUp(Customer_Id, Order_Date, Doctype);
                    }
                }

                string Order_SalesmanId = Convert.ToString(OrderEditdt.Rows[0]["Order_SalesmanId"]);
                if (Tax_Option != "")
                { PopulateGSTCSTVAT(Tax_Option); }

                string Tax_Code = Convert.ToString(OrderEditdt.Rows[0]["Tax_Code"]);

                txt_SlOrderNo.Text = Order_Number;
                if (!string.IsNullOrEmpty(Order_Date))
                {
                    dt_PLSales.Date = Convert.ToDateTime(Order_Date);
                    //dt_PLSales.ClientEnabled =false;
                    dt_PLSales.ClientEnabled = true;
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

                //lookup_Customer.GridView.Selection.SelectRowByKey(Customer_Id);//Subhabrata 11-12-2017
                //SetCustomerDDbyValue(Customer_Id);subhabrata 02-01-2018
                txtCustName.Text = CustomerName;
                // string strCustomer = Convert.ToString(hdfLookupCustomer.Value);
                //cmbContactPerson.SelectedItem.Value = Customer_Id;
                //  cmbContactPerson.SelectedItem = cmbContactPerson.Items.FindByValue(Customer_Id);
                txt_Refference.Text = Order_Reference;
                ddl_Branch.SelectedValue = Branch_Id;
                //ddl_SalesAgent.SelectedValue = Order_SalesmanId;//subhabrata on 12-12-2017
                //SetSalesManDDbyValue(Order_SalesmanId);//subhabrata on 03-01-2018
                hdnSalesManAgentId.Value = Order_SalesmanId;
                txtSalesManAgent.Text = Convert.ToString(OrderEditdt.Rows[0]["SalesMan"]);
                //Added 15-02-2017
                ddl_numberingScheme.SelectedValue = Order_NumberingScheme;
                //End
                ddl_Currency.SelectedValue = Currency_Id;
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
                ddl_VatGstCst.Value = Tax_Code;
                txt_SlOrderNo.Value = Order_Number;
                hddnOrderNumber.Value = Order_Number;
                //  ddl_Quotation.Value = Quotation_Number;
                txt_OANumber.Text = Order_OANumber;
                if (Customer_Id != "")
                { PopulateContactPerson(Customer_Id); }
                ddl_Num.Visible = false;
                if (Contact_Person_Id != "0")
                {
                    cmbContactPerson.Value = Contact_Person_Id;
                }
                else
                {
                    cmbContactPerson.Text = "";
                }
                // Rev 1.0
                //string strQtyTolerance = Convert.ToString(OrderEditdt.Rows[0]["Order_QtyTolerancePerc"]);
                //txtQtyTolerance.Text = strQtyTolerance;
                // End of Rev 1.0

            }

            if (OrderEdit != null && OrderEdit.Tables[2].Rows.Count > 0)
            {
                decimal BalQty = Convert.ToDecimal(OrderEdit.Tables[2].Rows[0]["BalQty"]);
                if (BalQty == 0)
                {
                    hddnDocumentIdTagged.Value = "1";
                }
            }

            DataTable dtt = GetProjectEditData(Convert.ToString(Session["OrderID"]));
            if (dtt != null)
            {
                lookup_Project.GridView.Selection.SelectRowByKey(Convert.ToInt64(dtt.Rows[0]["Proj_Id"]));

                //Tanmoy  Hierarchy
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable dt2 = oDBEngine.GetDataTable("Select Hierarchy_ID from V_ProjectList where Proj_Id='" + Convert.ToInt64(dtt.Rows[0]["Proj_Id"]) + "'");
                if (dt2.Rows.Count > 0)
                {
                    ddlHierarchy.SelectedValue = Convert.ToString(dt2.Rows[0]["Hierarchy_ID"]);
                }
                //Tanmoy  Hierarchy End
            }

            if (OrderEdit != null && OrderEdit.Tables[4].Rows.Count > 0)
            {
                int Cnt = Convert.ToInt32(OrderEdit.Tables[4].Rows[0]["Cnt"]);
                if (Cnt > 0)
                {
                    hddnDocumentIdTagged.Value = "1";
                }
            }



        }

        //rev Pratik Copy
        public void SetOrderDetailsCopy()
        {
            DataSet OrderEdit = GetOrderEditData();
            DataTable OrderEditdt = OrderEdit.Tables[0];
            if (OrderEditdt != null && OrderEditdt.Rows.Count > 0)
            {
                string Branch_Id = Convert.ToString(OrderEditdt.Rows[0]["Order_BranchId"]);
                string Order_Number = Convert.ToString(OrderEditdt.Rows[0]["Order_Number"]);
                string Order_Date = Convert.ToString(OrderEditdt.Rows[0]["Order_Date"]);
                //New Added
                string Order_NumberingScheme = Convert.ToString(OrderEditdt.Rows[0]["Order_NumScheme"]);
                //End
                // string Order_Salesman=Convert.ToString(OrderEditdt.Rows[0]["Order_NumScheme"])
                string Order_OANumber = Convert.ToString(OrderEditdt.Rows[0]["Order_OANumber"]);
                string Order_OADate = Convert.ToString(OrderEditdt.Rows[0]["Order_OADate"]);
                string Quotation_Date = Convert.ToString(OrderEditdt.Rows[0]["Order_Quotation_Date"]);
                string Quotation_Number = Convert.ToString(OrderEditdt.Rows[0]["Order_Quotation"]);
                string Order_Expiry = Convert.ToString(OrderEditdt.Rows[0]["Order_Expiry"]);
                string Customer_Id = Convert.ToString(OrderEditdt.Rows[0]["Customer_Id"]);
                hdnCustomerId.Value = Customer_Id;//added on 222-02-2017
                string Contact_Person_Id = Convert.ToString(OrderEditdt.Rows[0]["Contact_Person_Id"]);
                string Order_Reference = Convert.ToString(OrderEditdt.Rows[0]["Order_Reference"]);
                string Currency_Id = Convert.ToString(OrderEditdt.Rows[0]["Currency_Id"]);
                string Currency_Conversion_Rate = Convert.ToString(OrderEditdt.Rows[0]["Currency_Conversion_Rate"]);
                string Tax_Option = Convert.ToString(OrderEditdt.Rows[0]["Tax_Option"]);

                string CreditDueDate = Convert.ToString(OrderEditdt.Rows[0]["DueDate"]);
                string CreditDays = Convert.ToString(OrderEditdt.Rows[0]["CreditDays"]);

                string VehicleNumber = Convert.ToString(OrderEditdt.Rows[0]["VehicleNumber"]);
                string TransporterName = Convert.ToString(OrderEditdt.Rows[0]["TransporterName"]);
                string TransporterPhone = Convert.ToString(OrderEditdt.Rows[0]["TransporterPhone"]);
                string IsInvenotry = Convert.ToString(OrderEditdt.Rows[0]["IsInventory"]);
                string CustomerName = Convert.ToString(OrderEditdt.Rows[0]["CustomerName"]);

                ddl_PosGstSalesOrder.DataSource = OrderEdit.Tables[3];
                ddl_PosGstSalesOrder.ValueField = "ID";
                ddl_PosGstSalesOrder.TextField = "Name";
                ddl_PosGstSalesOrder.DataBind();


                string Segment1Name = Convert.ToString(OrderEditdt.Rows[0]["Segment1Name"]);
                string Segment2Name = Convert.ToString(OrderEditdt.Rows[0]["Segment2Name"]);
                string Segment3Name = Convert.ToString(OrderEditdt.Rows[0]["Segment3Name"]);
                string Segment4Name = Convert.ToString(OrderEditdt.Rows[0]["Segment4Name"]);
                string Segment5Name = Convert.ToString(OrderEditdt.Rows[0]["Segment5Name"]);

                string Segment1 = Convert.ToString(OrderEditdt.Rows[0]["Segment1"]);
                string Segment2 = Convert.ToString(OrderEditdt.Rows[0]["Segment2"]);
                string Segment3 = Convert.ToString(OrderEditdt.Rows[0]["Segment3"]);
                string Segment4 = Convert.ToString(OrderEditdt.Rows[0]["Segment4"]);
                string Segment5 = Convert.ToString(OrderEditdt.Rows[0]["Segment5"]);

                txtSegment1.Text = Segment1Name;
                txtSegment2.Text = Segment2Name;
                txtSegment3.Text = Segment3Name;
                txtSegment4.Text = Segment4Name;
                txtSegment5.Text = Segment5Name;


                hdnSegment1.Value = Segment1;
                hdnSegment2.Value = Segment2;
                hdnSegment3.Value = Segment3;
                hdnSegment4.Value = Segment4;
                hdnSegment5.Value = Segment5;

                string PosForGst = Convert.ToString(OrderEditdt.Rows[0]["PosForGst"]);

                ddl_PosGstSalesOrder.Value = PosForGst;

                Sales_BillingShipping.SetBillingShippingTable(OrderEdit.Tables[1]);

                string RevisionNo = Convert.ToString(OrderEditdt.Rows[0]["Order_RevisionNo"]);
                txtRevisionNo.Text = RevisionNo;
                string ApproveProjectRem = Convert.ToString(OrderEditdt.Rows[0]["Project_ApproveRejectREmarks"]);

                txtAppRejRemarks.Text = ApproveProjectRem;
                hdnApproveStatus.Value = Convert.ToString(OrderEditdt.Rows[0]["Project_ApproveStatus"]);
                string ProjectValidFrom = Convert.ToString(OrderEditdt.Rows[0]["Order_ProjectValidFrom"]);
                string ProjectValidUpto = Convert.ToString(OrderEditdt.Rows[0]["Order_ProjectValidUpto"]);

                //string Order_ProjectRemarks = Convert.ToString(OrderEditdt.Rows[0]["Order_ProjectRemarks"]);

                //txtProjRemarks.Text = Order_ProjectRemarks;
                if (!string.IsNullOrEmpty(Convert.ToString(OrderEditdt.Rows[0]["Order_ProjectValidFrom"])))
                {
                    dtProjValidFrom.Date = Convert.ToDateTime(OrderEditdt.Rows[0]["Order_ProjectValidFrom"]);

                }
                if (!string.IsNullOrEmpty(Convert.ToString(OrderEditdt.Rows[0]["Order_ProjectValidUpto"])))
                {
                    dtProjValidUpto.Date = Convert.ToDateTime(OrderEditdt.Rows[0]["Order_ProjectValidUpto"]);

                }

                if (!string.IsNullOrEmpty(Convert.ToString(OrderEditdt.Rows[0]["Order_RevisionDate"])))
                {
                    txtRevisionDate.Date = Convert.ToDateTime(OrderEditdt.Rows[0]["Order_RevisionDate"]);
                    txtRevisionDate.MinDate = Convert.ToDateTime(Convert.ToDateTime(OrderEditdt.Rows[0]["Order_RevisionDate"]).ToShortDateString());

                }
                else
                {
                    txtRevisionDate.Date = Convert.ToDateTime(Order_Date);
                    txtRevisionDate.MinDate = Convert.ToDateTime(Convert.ToDateTime(Order_Date).ToShortDateString());
                }

                if (hdnApproveStatus.Value == "1")
                {
                    lookup_Project.ClientEnabled = false;
                }



                if (!string.IsNullOrEmpty(IsInvenotry))
                {
                    if (IsInvenotry.ToUpper() == "0")
                    {
                        ddlInventory.SelectedValue = "N";
                    }
                    else if (IsInvenotry.ToUpper() == "TRUE")
                    {
                        ddlInventory.SelectedValue = "1";
                    }
                    else
                    {
                        ddlInventory.SelectedValue = IsInvenotry;
                    }

                    ddlInventory.Enabled = false;

                }

                //ASPxTextBox txtDriverName = (ASPxTextBox)VehicleDetailsControl.FindControl("txtDriverName");
                //ASPxTextBox txtPhoneNo = (ASPxTextBox)VehicleDetailsControl.FindControl("txtPhoneNo");
                //DropDownList ddl_VehicleNo = (DropDownList)VehicleDetailsControl.FindControl("ddl_VehicleNo");

                //txtDriverName.Text = TransporterName;
                //txtPhoneNo.Text = TransporterPhone;
                //ddl_VehicleNo.SelectedValue = VehicleNumber;


                txtCreditDays.Text = CreditDays;
                if (!string.IsNullOrEmpty(CreditDueDate))
                {
                    dt_SaleInvoiceDue.Date = Convert.ToDateTime(CreditDueDate);
                }
                string Doctype = Convert.ToString(OrderEditdt.Rows[0]["Doctype"]);
                string Quoids = Convert.ToString(OrderEditdt.Rows[0]["Order_Quotation_Ids"]);

                if (!String.IsNullOrEmpty(Quoids) && Quoids.Split(',')[0] != "0")
                {
                    Session["Lookup_QuotationIds"] = Quoids;
                    string[] eachQuo = Quoids.Split(',');
                    if (eachQuo.Length > 1)//More tha one quotation
                    {
                        lookup_Project.ClientEnabled = false;
                        if (Doctype == "QN")
                        {
                            rdl_Salesquotation.SelectedValue = "QN";
                            dt_Quotation.Text = "Multiple Select Quotation Dates";
                        }
                        if (Doctype == "SINQ")
                        {
                            rdl_Salesquotation.SelectedValue = "SINQ";
                            dt_Quotation.Text = "Multiple Select Inquiry Dates";
                        }

                        BindLookUp(Customer_Id, Order_Date, Doctype);
                        foreach (string val in eachQuo)
                        {
                            lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));

                        }
                        // lbl_MultipleDate.Attributes.Add("style", "display:block");
                    }
                    else if (eachQuo.Length == 1)//Single Quotation
                    { //lbl_MultipleDate.Attributes.Add("style", "display:none"); }
                        lookup_Project.ClientEnabled = false;
                        if (Doctype == "QN")
                        {
                            rdl_Salesquotation.SelectedValue = "QN";
                            //dt_Quotation.Text = "Multiple Select Quotation Dates";
                        }
                        if (Doctype == "SINQ")
                        {
                            rdl_Salesquotation.SelectedValue = "SINQ";
                            //dt_Quotation.Text = "Multiple Select Inquiry Dates";
                        }
                        BindLookUp(Customer_Id, Order_Date, Doctype);
                        foreach (string val in eachQuo)
                        {
                            lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(val));
                        }
                    }
                    else//No Quotation selected
                    {
                        BindLookUp(Customer_Id, Order_Date, Doctype);
                    }
                }

                string Order_SalesmanId = Convert.ToString(OrderEditdt.Rows[0]["Order_SalesmanId"]);
                if (Tax_Option != "")
                { PopulateGSTCSTVAT(Tax_Option); }

                string Tax_Code = Convert.ToString(OrderEditdt.Rows[0]["Tax_Code"]);

                txt_SlOrderNo.Text = Order_Number;
                if (!string.IsNullOrEmpty(Order_Date))
                {
                    dt_PLSales.Date = Convert.ToDateTime(Order_Date);
                    //dt_PLSales.ClientEnabled =false;
                    dt_PLSales.ClientEnabled = true;
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

                //lookup_Customer.GridView.Selection.SelectRowByKey(Customer_Id);//Subhabrata 11-12-2017
                //SetCustomerDDbyValue(Customer_Id);subhabrata 02-01-2018
                txtCustName.Text = CustomerName;
                // string strCustomer = Convert.ToString(hdfLookupCustomer.Value);
                //cmbContactPerson.SelectedItem.Value = Customer_Id;
                //  cmbContactPerson.SelectedItem = cmbContactPerson.Items.FindByValue(Customer_Id);
                txt_Refference.Text = Order_Reference;
                ddl_Branch.SelectedValue = Branch_Id;
                //ddl_SalesAgent.SelectedValue = Order_SalesmanId;//subhabrata on 12-12-2017
                //SetSalesManDDbyValue(Order_SalesmanId);//subhabrata on 03-01-2018
                hdnSalesManAgentId.Value = Order_SalesmanId;
                txtSalesManAgent.Text = Convert.ToString(OrderEditdt.Rows[0]["SalesMan"]);
                //Added 15-02-2017
                ddl_numberingScheme.SelectedValue = Order_NumberingScheme;
                //End
                ddl_Currency.SelectedValue = Currency_Id;
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
                ddl_VatGstCst.Value = Tax_Code;
                txt_SlOrderNo.Value = Order_Number;
                hddnOrderNumber.Value = Order_Number;
                //  ddl_Quotation.Value = Quotation_Number;
                txt_OANumber.Text = Order_OANumber;
                if (Customer_Id != "")
                { PopulateContactPerson(Customer_Id); }
                ddl_Num.Visible = false;
                if (Contact_Person_Id != "0")
                {
                    cmbContactPerson.Value = Contact_Person_Id;
                }
                else
                {
                    cmbContactPerson.Text = "";
                }
                // Rev 1.0
                //string strQtyTolerance = Convert.ToString(OrderEditdt.Rows[0]["Order_QtyTolerancePerc"]);
                //txtQtyTolerance.Text = strQtyTolerance;
                // End of Rev 1.0

            }

            if (OrderEdit != null && OrderEdit.Tables[2].Rows.Count > 0)
            {
                decimal BalQty = Convert.ToDecimal(OrderEdit.Tables[2].Rows[0]["BalQty"]);
                if (BalQty == 0)
                {
                    hddnDocumentIdTagged.Value = "1";
                }
            }

            DataTable dtt = GetProjectEditData(Convert.ToString(Session["OrderID"]));
            if (dtt != null)
            {
                lookup_Project.GridView.Selection.SelectRowByKey(Convert.ToInt64(dtt.Rows[0]["Proj_Id"]));

                //Tanmoy  Hierarchy
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable dt2 = oDBEngine.GetDataTable("Select Hierarchy_ID from V_ProjectList where Proj_Id='" + Convert.ToInt64(dtt.Rows[0]["Proj_Id"]) + "'");
                if (dt2.Rows.Count > 0)
                {
                    ddlHierarchy.SelectedValue = Convert.ToString(dt2.Rows[0]["Hierarchy_ID"]);
                }
                //Tanmoy  Hierarchy End
            }

            if (OrderEdit != null && OrderEdit.Tables[4].Rows.Count > 0)
            {
                int Cnt = Convert.ToInt32(OrderEdit.Tables[4].Rows[0]["Cnt"]);
                if (Cnt > 0)
                {
                    hddnDocumentIdTagged.Value = "1";
                }
            }

            txt_SlOrderNo.Text = "";
            txt_SlOrderNo.Value = "";
            dt_PLSales.ClientEnabled = true;

        }
        //End of rev Pratik Copy
        public void fillRecordFromBasket(string basketId)
        {
            CRMSalesOrderDtlBL CRMSales = new CRMSalesOrderDtlBL();
            DataSet busketget = GetBasketData();

            //#region SetHeader
            if (busketget.Tables[0].Rows.Count > 0)
            {
                DataTable headerTable = busketget.Tables[0];
                string customerId = Convert.ToString(headerTable.Rows[0]["CustomerId"]);
                txtCustName.Text = Convert.ToString(headerTable.Rows[0]["CustomerName"]);
                string SalesmanId = Convert.ToString(headerTable.Rows[0]["SalesmanId"]);
                txtSalesManAgent.Text = Convert.ToString(headerTable.Rows[0]["SalesmanName"]);
                ddlInventory.SelectedIndex = 0;
                ddl_numberingScheme.SelectedIndex = 1;

                //ddl_AmountAre.SelectedIndex = 2;
                ddl_AmountAre.Value = "2";
                ddl_AmountAre.ClientEnabled = false;

                ddl_AmountAre.DataBind();
                hdnCustomerId.Value = customerId;
                hdnSalesManAgentId.Value = SalesmanId;

                PopulateGSTCSTVAT("2");
            }

            Sales_BillingShipping.SetBillingShippingTable(busketget.Tables[1]);
            string stateName = Convert.ToString(busketget.Tables[1].Rows[0]["StateName"]);
            int StateId = Convert.ToInt32(busketget.Tables[1].Rows[0]["StateId"]);
            ddl_PosGstSalesOrder.DataSource = busketget.Tables[2];
            ddl_PosGstSalesOrder.TextField = "Name";
            ddl_PosGstSalesOrder.ValueField = "ID";
            ddl_PosGstSalesOrder.DataBind();
            ddl_PosGstSalesOrder.Value = "S";
            ddl_PosGstSalesOrder.ClientEnabled = false;
            //hdStateIdShipping.Value = StateId;
            DataSet busketSet = CRMSales.GetOrderBusketById(Convert.ToInt32(basketId));
            DataTable DetailGrid = busketSet.Tables[0];
            DataTable GrdpackQty = busketSet.Tables[3];
            #region SetShippingState
            string ShippingCode = "";
            if (busketSet.Tables[2].Rows.Count > 0)
            {
                ShippingCode = Convert.ToString(busketSet.Tables[2].Rows[0][0]);
            }


            #endregion
            DetailGrid = gstTaxDetails.SetTaxAmountWithGSTonDetailsTableOrderBasket(DetailGrid, "sProductsID", "TaxAmount", "Amount", "TotalAmount", dt_PLSales.Date.ToString("yyyy-MM-dd"), "S", Convert.ToString(ddl_Branch.SelectedValue), ShippingCode, "I");

            CreateDataTaxTable(uniqueId.Value.ToString());
            DataTable taxtable = (DataTable)Session["SalesOrderFinalTaxRecord" + Convert.ToString(uniqueId.Value)];
            taxtable = gstTaxDetails.SetTaxTableDataWithProductSerial(DetailGrid, "SrlNo", "sProductsID", "Amount", "TaxAmount", taxtable, "S", dt_PLSales.Date.ToString("yyyy-MM-dd"), Convert.ToString(ddl_Branch.SelectedValue), ShippingCode, "I");
            Session["SalesOrderFinalTaxRecord" + Convert.ToString(uniqueId.Value)] = taxtable;
            DetailGrid.Columns.Remove("sProductsID");
            DetailGrid.Columns.Remove("Quotation_Num");
            Session["OrderDetails"] = DetailGrid;
            // DetailGrid.Columns.Remove("sProductsID");
            string totalAmount = Convert.ToString(DetailGrid.Compute("SUM(TotalAmount)", string.Empty));
            // bnrlblAmountWithTaxValue.Text = totalAmount;
            // bnrLblInvValue.Text = totalAmount;

            //Chinmoy comment fordocument number addition
            //Start
            // grid.DataSource = GetQuotation(DetailGrid);
            DataTable duplicatedt2 = new DataTable();
            duplicatedt2.Columns.Add("productid", typeof(Int64));
            duplicatedt2.Columns.Add("pslno", typeof(Int32));
            duplicatedt2.Columns.Add("pQuantity", typeof(Decimal));
            duplicatedt2.Columns.Add("packing", typeof(Decimal));
            duplicatedt2.Columns.Add("PackingUom", typeof(Int32));
            duplicatedt2.Columns.Add("PackingSelectUom", typeof(Int32));
            HttpContext.Current.Session["SessionPackingDetails"] = GetWaitingProductDetails(GrdpackQty);
            if (HttpContext.Current.Session["SessionPackingDetails"] != null)
            {
                List<WaitingProductQuantity> obj = new List<WaitingProductQuantity>();
                obj = (List<WaitingProductQuantity>)HttpContext.Current.Session["SessionPackingDetails"];
                foreach (var item in obj)
                {
                    duplicatedt2.Rows.Add(item.productid, item.pslno, item.pQuantity, item.packing, item.PackingUom, item.PackingSelectUom);
                }
            }
            //HttpContext.Current.Session["SessionPackingDetails"] = null;


            grid.DataSource = GetWaitingProductDetails(GrdpackQty);

            grid.DataSource = GetBasketOrderDetails(DetailGrid);
            //End
            grid.DataBind();
            Session["OrderDetails" + Convert.ToString(uniqueId.Value)] = DetailGrid;

            //#region setComponentProduct
            //if (busketSet.Tables[2].Rows.Count > 0)
            //{
            //    isBasketContainComponent.Value = "yes";
            //}
            //#endregion
            
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
        public DataTable GetSINqDate(string Quote_Nos)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_GetQuotationDetails");
            proc.AddVarcharPara("@Quote_Number", 100, Quote_Nos);
            proc.AddVarcharPara("@Mode", 100, "GetSalesInquiryDate");

            return proc.GetTable();
        }
        protected void ComponentDatePanel_Callback(object sender, CallbackEventArgsBase e)
        {


            string strSplitCommand = e.Parameter.Split('~')[0];
            if (strSplitCommand == "BindQuotationDate")
            {
                string Quote_No = Convert.ToString(e.Parameter.Split('~')[1]);

                //DataTable dt_QuotationDetails = objSlaesActivitiesBL.GetQuotationDate(Quote_No);
                DataTable dt_QuotationDetails = new DataTable();
                if (rdl_Salesquotation.SelectedValue == "QN")
                {
                    dt_QuotationDetails = objSlaesActivitiesBL.GetQuotationDate(Quote_No);
                }
                else
                {
                    dt_QuotationDetails = GetSINqDate(Quote_No);
                }
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
        protected void lookup_quotation_DataBinding(object sender, EventArgs e)
        {
            //rev Pratik Copy
            //if (Session["QuotationData"] != null)
            if (Session["QuotationData"] != null && Request.QueryString["type"] != "COPY")
            //End of rev Pratik Copy
            {
                lookup_quotation.DataSource = (DataTable)Session["QuotationData"];
            }
        }
        protected void BindLookUp(String customer, string OrderDate, string Doctype)
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



            DataTable QuotationTable;


            QuotationTable = objBL.GetDocOnSalesOrder(customer, OrderDate, status,
                Convert.ToInt32(ddl_Branch.SelectedValue), Convert.ToString(ddlInventory.SelectedValue), Doctype);
            lookup_quotation.GridView.Selection.CancelSelection();
            //rev Pratik Copy
            //lookup_quotation.DataSource = QuotationTable;
            //lookup_quotation.DataBind();
            if (Request.QueryString["type"] != "COPY")
            {
                // Mantis Issue 25408
                Session["QuotationData"] = QuotationTable;
                // End of Mantis Issue 25408
                lookup_quotation.DataSource = QuotationTable;
                lookup_quotation.DataBind();
            }
                //End of rev Pratik Copy

            Session["QuotationData"] = QuotationTable;
        }
        //Subhabrata
        protected void ComponentQuotation_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string status = string.Empty;
            string customer = string.Empty;
            string OrderDate = string.Empty;
            string BranchId = string.Empty;
            string Type = string.Empty;
            if (e.Parameter.Split('~')[0] == "BindQuotationGrid")
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
                if (e.Parameter.Split('~')[4] != null)
                { Type = e.Parameter.Split('~')[4]; }

                DataTable QuotationTable;

                if (e.Parameter.Split('~')[3] == "DateCheck")
                {
                    lookup_quotation.GridView.Selection.UnselectAll();
                }
                QuotationTable = objBL.GetDocOnSalesOrder(customer, OrderDate, status, Convert.ToInt32(ddl_Branch.SelectedValue), Convert.ToString(ddlInventory.SelectedValue), Type);
                lookup_quotation.GridView.Selection.CancelSelection();
                //rev Pratik copy
                //lookup_quotation.DataSource = QuotationTable;
                //lookup_quotation.DataBind();
                if (Request.QueryString["type"] != "COPY")
                {
                    // Mantis Issue 25408
                    Session["QuotationData"] = QuotationTable;
                    // End of Mantis Issue 25408
                    lookup_quotation.DataSource = QuotationTable;
                    lookup_quotation.DataBind();
                }
                //End of rev Pratik copy



                Session["QuotationData"] = QuotationTable;

            }
            else if (e.Parameter.Split('~')[0] == "BindQuotationGridOnSelection")//Subhabrata for binding quotation
            {
                int AmountsAre = 0;
                ComponentQuotationPanel.JSProperties["cpTaggedTaxAmountType"] = "";
                if (grid_Products.GetSelectedFieldValues("Quotation_No").Count != 0)
                {
                    for (int i = 0; i < grid_Products.GetSelectedFieldValues("Quotation_No").Count; i++)
                    {
                        QuotationIds += "," + grid_Products.GetSelectedFieldValues("Quotation_No")[i];
                        AmountsAre = Convert.ToInt32(grid_Products.GetSelectedFieldValues("TaxAmountType")[0]);
                    }
                    hdnAmountareTax.Value = Convert.ToString(AmountsAre);
                    ComponentQuotationPanel.JSProperties["cpTaggedTaxAmountType"] = Convert.ToString(AmountsAre);
                    QuotationIds = QuotationIds.TrimStart(',');
                    lookup_quotation.GridView.Selection.UnselectAll();
                    if (!String.IsNullOrEmpty(QuotationIds))
                    {
                        if (AmountsAre == 1)
                        {
                            ddl_AmountAre.Value = "1";
                            ddl_AmountAre.ClientEnabled = false;
                        }
                        else if (AmountsAre == 2)
                        {
                            ddl_AmountAre.Value = "2";
                            ddl_AmountAre.ClientEnabled = false;
                        }

                        string[] eachQuo = QuotationIds.Split(',');
                        if (eachQuo.Length > 1)//More tha one quotation
                        {
                            if (rdl_Salesquotation.SelectedValue == "QN")
                                dt_Quotation.Text = "Multiple Select Quotation Dates";
                            else if (rdl_Salesquotation.SelectedValue == "SINQ")
                                dt_Quotation.Text = "Multiple Select Inquiry Dates";
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
                            lookup_quotation.GridView.Selection.UnselectAll();
                        }
                    }
                }
                else if (grid_Products.GetSelectedFieldValues("Quotation_No").Count == 0)
                {
                    lookup_quotation.GridView.Selection.UnselectAll();
                    Session["OrderDetails"] = null;
                    grid.DataSource = null;
                    grid.DataBind();
                }
            }
            else if (e.Parameter.Split('~')[0] == "DateCheckOnChanged")//Subhabrata for binding quotation
            {

                if (grid_Products.GetSelectedFieldValues("Quotation_No").Count != 0)
                {

                    DateTime SalesOrderDate = Convert.ToDateTime(e.Parameter.Split('~')[2]);
                    if (lookup_quotation.GridView.GetSelectedFieldValues("Quote_Date").Count() != 0)
                    {
                        //DateTime QuotationDate = DateTime.Parse(Convert.ToString(lookup_quotation.GridView.GetSelectedFieldValues("Quote_Date")[0]));
                        //if (SalesOrderDate < QuotationDate)
                        //{
                        lookup_quotation.GridView.Selection.UnselectAll();
                        //}
                    }


                    //Quote_Date

                }
            }

        }
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

                DataTable Warehousedt = new DataTable();
                if (Session["SalesWarehouseData"] != null)
                {
                    Warehousedt = (DataTable)Session["SalesWarehouseData"];
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
                changeGridOrder();
            }
            else if (strSplitCommand == "SaveDisplay")
            {
                int loopId = Convert.ToInt32(Session["LoopSalesOrderWarehouse"]);

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

                string Sales_UOM_Name = "", Sales_UOM_Code = "", Stk_UOM_Name = "", Stk_UOM_Code = "", Conversion_Multiplier = "", Trans_Stock = "0", MfgDate = "", ExpiryDate = "";
                GetProductType(ref Type);

                DataTable Warehousedt = new DataTable();
                if (Session["SalesWarehouseData"] != null)
                {
                    Warehousedt = (DataTable)Session["SalesWarehouseData"];
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

                bool IsDelete = false;

                if (Type == "WBS")
                {
                    GetTotalStock(ref Trans_Stock, WarehouseID);
                    GetProductUOM(ref Sales_UOM_Name, ref Sales_UOM_Code, ref Stk_UOM_Name, ref Stk_UOM_Code, ref Conversion_Multiplier, ProductID);
                    GetBatchDetails(ref MfgDate, ref ExpiryDate, BatchID);

                    string[] SerialIDList = SerialID.Split(new string[] { "||@||" }, StringSplitOptions.None);
                    string[] SerialNameList = SerialName.Split(new string[] { "||@||" }, StringSplitOptions.None);

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

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, SerialIDList.Length, "D");
                            }
                            else
                            {
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", "", BatchID, "", strSrlID, strSrlName, "", Sales_UOM_Code, "", "", Stk_UOM_Code, "", "", "", "", "", "", loopId, SerialIDList.Length, "D");
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

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, strLoopID, SerialIDList.Length, repute);
                            }
                            else
                            {
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", "", BatchID, "", strSrlID, strSrlName, "", Sales_UOM_Code, "", "", Stk_UOM_Code, "", "", "", "", "", "", strLoopID, SerialIDList.Length, repute);
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
                            Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, BatchID, BatchName, "0", "", Sales_UOM_Name, Sales_UOM_Code, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, Qty, "D");
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
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, BatchID, BatchName, "0", "", Sales_UOM_Name, Sales_UOM_Code, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, Qty, "D");

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
                            Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, BatchID, BatchName, "0", "", Sales_UOM_Name, Sales_UOM_Code, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, Qty, "D");
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
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, BatchID, BatchName, "0", "", Sales_UOM_Name, Sales_UOM_Code, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, Qty, "D");
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
                            Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, BatchID, BatchName, "0", "", Sales_UOM_Name, Sales_UOM_Code, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, Qty, "D");
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
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, Qty, BatchID, BatchName, "0", "", Sales_UOM_Name, Sales_UOM_Code, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, Qty, "D");
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
                            Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, SerialIDList.Length, "D");
                        }
                        else
                        {
                            var rows = Warehousedt.Select("SerialID ='" + strSrlID + "' AND SrlNo='" + editWarehouseID + "'");
                            if (rows.Length == 0)
                            {
                                IsDelete = true;
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, SerialIDList.Length, "D");
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

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, "0", BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, SerialIDList.Length, "D");
                            }
                            else
                            {
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", "", "0", "", strSrlID, strSrlName, "", Sales_UOM_Code, "", "", Stk_UOM_Code, "", "", "", "", "", "", loopId, SerialIDList.Length, "D");
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

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, "0", BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, SerialIDList.Length, repute);
                            }
                            else
                            {
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", "", "0", "", strSrlID, strSrlName, "", Sales_UOM_Code, "", "", Stk_UOM_Code, "", "", "", "", "", "", loopId, SerialIDList.Length, repute);
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

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, SerialIDList.Length, "D");
                            }
                            else
                            {
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", "", BatchID, "", strSrlID, strSrlName, "", Sales_UOM_Code, "", "", Stk_UOM_Code, "", "", "", "", "", "", loopId, SerialIDList.Length, "D");
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

                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, WarehouseName, SerialIDList.Length, BatchID, BatchName, strSrlID, strSrlName, Sales_UOM_Name, Sales_UOM_Code, SerialIDList.Length + " " + Sales_UOM_Name, Stk_UOM_Name, Stk_UOM_Code, stkqtn + " " + Stk_UOM_Name, Conversion_Multiplier, Convert.ToString(Math.Round(Convert.ToDecimal(Trans_Stock))) + " " + Stk_UOM_Name, Convert.ToString(Math.Round(BalanceStk, 2)) + " " + Stk_UOM_Name, MfgDate, ExpiryDate, loopId, SerialIDList.Length, repute);
                            }
                            else
                            {
                                string maxID = (Convert.ToString(Warehousedt.Compute("MAX([SrlNo])", "")) != "") ? Convert.ToString(Convert.ToInt32(Warehousedt.Compute("MAX([SrlNo])", "")) + 1) : "1";
                                Warehousedt.Rows.Add(ProductSerialID, maxID, WarehouseID, "", "", BatchID, "", strSrlID, strSrlName, "", Sales_UOM_Code, "", "", Stk_UOM_Code, "", "", "", "", "", "", loopId, SerialIDList.Length, repute);
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

                Session["SalesWarehouseData"] = Warehousedt;
                changeGridOrder();

                GrdWarehouse.DataSource = Warehousedt.DefaultView;
                GrdWarehouse.DataBind();

                Session["LoopSalesOrderWarehouse"] = loopId + 1;

                CmbWarehouse.SelectedIndex = -1;
                CmbBatch.SelectedIndex = -1;
            }
            else if (strSplitCommand == "Delete")
            {
                string strKey = e.Parameters.Split('~')[1];
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

                Session["WarehouseData"] = Warehousedt;
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
                if (Session["SalesWarehouseData"] != null)
                {
                    DataTable Warehousedt = (DataTable)Session["SalesWarehouseData"];
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

                    Session["SalesWarehouseData"] = Warehousedt;
                }
            }
        }
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string performpara = e.Parameter;
            if (performpara.Split('~')[0] == "EditWarehouse")
            {
                string SrlNo = performpara.Split('~')[1];
                string ProductType = Convert.ToString(hdfProductType.Value);

                if (Session["SalesWarehouseData"] != null)
                {
                    DataTable Warehousedt = (DataTable)Session["SalesWarehouseData"];

                    string strWarehouse = "", strBatchID = "", strSrlID = "", strQuantity = "0";
                    var rows = Warehousedt.Select(string.Format("SrlNo ='{0}'", SrlNo));
                    foreach (var dr in rows)
                    {
                        strWarehouse = (Convert.ToString(dr["WarehouseID"]) != "") ? Convert.ToString(dr["WarehouseID"]) : "0";
                        strBatchID = (Convert.ToString(dr["BatchID"]) != "") ? Convert.ToString(dr["BatchID"]) : "0";
                        strSrlID = (Convert.ToString(dr["SerialID"]) != "") ? Convert.ToString(dr["SerialID"]) : "0";
                        strQuantity = (Convert.ToString(dr["TotalQuantity"]) != "") ? Convert.ToString(dr["TotalQuantity"]) : "0";
                    }

                    //CmbWarehouse.DataSource = GetWarehouseData();
                    CmbBatch.DataSource = GetBatchData(strWarehouse);
                    CmbBatch.DataBind();

                    CallbackPanel.JSProperties["cpEdit"] = strWarehouse + "~" + strBatchID + "~" + strSrlID + "~" + strQuantity;
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

        //[WebMethod]
        //public static string GetIsMandatory(string Products_ID)
        //{
        //    string a = string.Empty;

        //    CRMSalesDtlBL objCRMSalesDtlBL = new CRMSalesDtlBL();
        //    int ispermission = 0;
        //    ispermission = objCRMSalesDtlBL.getIsMandatoryPartyOrder(Convert.ToInt32(Products_ID));

        //    //}
        //    return Convert.ToString(ispermission);
        //}
        protected void GrdWarehouse_DataBinding(object sender, EventArgs e)
        {
            if (Session["SalesWarehouseData"] != null)
            {
                string Type = "";
                GetProductType(ref Type);
                string SerialID = Convert.ToString(hdfProductSerialID.Value);
                DataTable Warehousedt = (DataTable)Session["SalesWarehouseData"];

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
        public void GetQuantityBaseOnProduct(string strProductSrlNo, ref decimal WarehouseQty)
        {
            decimal sum = 0;

            DataTable Warehousedt = new DataTable();
            if (Session["SalesWarehouseData"] != null)
            {
                Warehousedt = (DataTable)Session["SalesWarehouseData"];
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
        protected void acpAvailableStock_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string performpara = e.Parameter;
            string AssignedVal = Convert.ToString(performpara.Split('~')[0]);
            string strProductID = Convert.ToString(performpara.Split('~')[1]);

            string strBranch = Convert.ToString(ddl_Branch.SelectedValue);
            acpAvailableStock.JSProperties["cpstock"] = "0.00";

            if (AssignedVal == "MainAviableStockBind")
            {
                try
                {
                    //DataTable dt2 = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableQuotation(" + strBranch + ",'" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]) + "'," + strProductID + ") as branchopenstock");
                    DataTable dt2 = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableQuotation(" + strBranch + ",'" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]) + "','" + strProductID + "') as branchopenstock");

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
            else if (AssignedVal == "GetAvailableStockOnOrder")
            {
                string SalesOrder_Id = Convert.ToString(performpara.Split('~')[2]);
                decimal Quantity = Convert.ToDecimal(performpara.Split('~')[3]);

                try
                {
                    DataTable dt2 = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableQuotation(" + strBranch + ",'" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]) + "'," + strProductID + ") as branchopenstock");

                    if (dt2.Rows.Count > 0)
                    {
                        acpAvailableStock.JSProperties["cpstock"] = Math.Round((Convert.ToDecimal(dt2.Rows[0]["branchopenstock"]) + Quantity), 2);
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

        }
        public static void DeleteWarehouse(string SrlNo)
        {
            DataTable Warehousedt = new DataTable();
            if (HttpContext.Current.Session["SalesWarehouseData"] != null)
            {
                Warehousedt = (DataTable)HttpContext.Current.Session["SalesWarehouseData"];

                var rows = Warehousedt.Select(string.Format("Product_SrlNo ='{0}'", SrlNo));
                foreach (var row in rows)
                {
                    row.Delete();
                }
                Warehousedt.AcceptChanges();

                HttpContext.Current.Session["SalesWarehouseData"] = Warehousedt;
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
        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            //e.KeyExpression = "DOC_ID";

            #region Block By Sudip

            //  string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);



            string CustomerId = Convert.ToString(hdnCustomerId.Value);
            string strToDate = Convert.ToString(hddnAsOnDate.Value);
            e.KeyExpression = "SLNO";

            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            if (Convert.ToString(hddnOutStandingBlock.Value) == "1")
            {
                var q = from d in dc.PARTYOUTSTANDINGDET_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.SLNO) != "999999999" && Convert.ToString(d.PARTYTYPE) == "C"
                        //&& Convert.ToString(d.DOC_DATE) == strToDate &&
                        //Convert.ToString(d.PARTYID) == CustomerId 
                        //orderby d.BRANCH_ID ascending, d.PARTYID ascending, d.SLNO ascending, d.DOC_TYPE_ORDERBY ascending
                        orderby d.SEQ ascending
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.PARTYOUTSTANDINGDET_REPORTs
                        where Convert.ToString(d.SLNO) == "0"
                        //&& Convert.ToString(d.DOC_DATE) == strToDate &&
                        //Convert.ToString(d.PARTYID) == CustomerId 
                        //orderby d.BRANCH_ID ascending, d.PARTYID ascending, d.SLNO ascending, d.DOC_TYPE_ORDERBY ascending
                        orderby d.SEQ ascending
                        select d;
                e.QueryableSource = q;
            }


            #endregion
            CustomerOutstanding.ExpandAll();
            //if (Convert.ToString(Session["IsCustOutDetFilter"]) == "Y")
            //{
            //    var q = from d in dc.PARTYOUTSTANDINGDET_REPORTs
            //            where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.SLNO) != "999999999" 
            //            orderby d.BRANCH_ID ascending, d.PARTYID ascending, d.SLNO ascending, d.DOC_TYPE_ORDERBY ascending
            //            select d;
            //    e.QueryableSource = q;
            //}
            //else
            //{
            //    var q = from d in dc.PARTYOUTSTANDINGDET_REPORTs
            //            where Convert.ToString(d.SLNO) == "0"
            //            orderby d.BRANCH_ID ascending, d.PARTYID ascending, d.SLNO ascending, d.DOC_TYPE_ORDERBY ascending
            //            select d;
            //    e.QueryableSource = q;
            //}
        }
        protected void QuotationBillingShipping_Callback(object sender, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "BindAddress")
            {
                string Quotation_Id = Convert.ToString(e.Parameter.Split('~')[1]);
                DataTable dt = GetQuotationAddressDetails(Quotation_Id);
                Sales_BillingShipping.SetBillingShippingTable(dt);


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

        public class CreditDay
        {
            public int CreditDays { get; set; }

        }
        #region Rajdip For Get Credit Days
        //[WebMethod]
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object GetCreditdays(string CustomerId)
        {
            if (HttpContext.Current.Session["userid"] != null)
            {
                ProcedureExecute proc = new ProcedureExecute("prc_getCreditDays");
                proc.AddVarcharPara("@Action", 50, "GetCreditDays");
                proc.AddVarcharPara("@CustomerId", 50, CustomerId);
                DataTable Address = proc.GetTable();

                if (Address.Rows.Count > 0)
                {
                    CreditDay details = new CreditDay();
                    details.CreditDays = Convert.ToInt32(Address.Rows[0]["cnt_CreditDays"]);
                    return details;

                }

            }
            return null;
        }
        #endregion Rajdip
        //Rev Rajdip



        public class SegmentDetails
        {
            public string Segment1 { get; set; }
            public string Segment2 { get; set; }
            public string Segment3 { get; set; }
            public string Segment4 { get; set; }
            public string Segment5 { get; set; }
            public string SegmentName1 { get; set; }
            public string SegmentName2 { get; set; }
            public string SegmentName3 { get; set; }
            public string SegmentName4 { get; set; }
            public string SegmentName5 { get; set; }           

        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object GetSegmentDetails(string CustomerId)
        {
            if (HttpContext.Current.Session["userid"] != null)
            {
                ProcedureExecute proc = new ProcedureExecute("prc_getCreditDays");
                proc.AddVarcharPara("@Action", 50, "GetSegmentDetails");
                proc.AddVarcharPara("@CustomerId", 50, CustomerId);
                DataTable Address = proc.GetTable();

                if (Address.Rows.Count > 0)
                {
                    SegmentDetails details = new SegmentDetails();
                    details.Segment1 = Convert.ToString(Address.Rows[0]["Segment1"]);
                    details.Segment2 = Convert.ToString(Address.Rows[0]["Segment2"]);
                    details.Segment3 = Convert.ToString(Address.Rows[0]["Segment3"]);
                    details.Segment4 = Convert.ToString(Address.Rows[0]["Segment4"]);
                    details.Segment5 = Convert.ToString(Address.Rows[0]["Segment5"]);

                    details.SegmentName1 = Convert.ToString(Address.Rows[0]["UsedFor1"]);
                    details.SegmentName2 = Convert.ToString(Address.Rows[0]["UsedFor2"]);
                    details.SegmentName3 = Convert.ToString(Address.Rows[0]["UsedFor3"]);
                    details.SegmentName4 = Convert.ToString(Address.Rows[0]["UsedFor4"]);
                    details.SegmentName5 = Convert.ToString(Address.Rows[0]["UsedFor5"]);
                    return details;

                }

            }
            return null;
        }


        public class SalesManAgntModel
        {
            public string id { get; set; }
            public string Na { get; set; }
        }
        [WebMethod(EnableSession = true)]
        public static object GetSalesManAgent(string SearchKey, string CustomerId)
        {
            List<SalesManAgntModel> listSalesMan = new List<SalesManAgntModel>();
            string Mode = Convert.ToString(HttpContext.Current.Session["key_QutId"]);
            if (HttpContext.Current.Session["userid"] != null)
            {

                //  BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable dtexistcheckfrommaptable = oDBEngine.GetDataTable("SELECT * FROM tbl_Salesman_Entitymap WHERE CustomerId=(Select cnt_id from tbl_master_contact WHERE cnt_internalId='" + CustomerId + "' )");
                if (dtexistcheckfrommaptable.Rows.Count > 0)
                {
                    if (Mode != "ADD")
                    {
                        DataTable cust = new DataTable();
                        ProcedureExecute proc = new ProcedureExecute("prc_GetSalesOrdermappedSalesMan");
                        proc.AddVarcharPara("@CustomerID", 500, CustomerId);
                        proc.AddVarcharPara("@SearchKey", 500, SearchKey);
                        proc.AddVarcharPara("@Action", 500, "Edit");
                        proc.AddVarcharPara("@OrderId", 500, Mode);
                        cust = proc.GetTable();

                        listSalesMan = (from DataRow dr in cust.Rows
                                        select new SalesManAgntModel()
                                        {
                                            id = dr["SalesmanId"].ToString(),
                                            Na = dr["Name"].ToString()
                                        }).ToList();
                    }
                    else
                    {
                        DataTable cust = new DataTable();
                        ProcedureExecute proc = new ProcedureExecute("prc_GetSalesOrdermappedSalesMan");
                        proc.AddVarcharPara("@Action", 500, "Add");
                        proc.AddVarcharPara("@CustomerID", 500, CustomerId);
                        proc.AddVarcharPara("@SearchKey", 500, SearchKey);

                        cust = proc.GetTable();

                        listSalesMan = (from DataRow dr in cust.Rows
                                        select new SalesManAgntModel()
                                        {
                                            id = dr["SalesmanId"].ToString(),
                                            Na = dr["Name"].ToString()
                                        }).ToList();

                    }

                }
                else
                {

                    DataTable cust = oDBEngine.GetDataTable("select top 10 cnt_id ,Name from v_GetAllSalesManAgent where  Name like '%" + SearchKey + "%'");


                    listSalesMan = (from DataRow dr in cust.Rows
                                    select new SalesManAgntModel()
                                    {
                                        id = dr["cnt_id"].ToString(),
                                        Na = dr["Name"].ToString()
                                    }).ToList();

                }

            }

            return listSalesMan;
        }
        public class GetSalesMan
        {
            public int Id { get; set; }
            public string Name { get; set; }

            //  public string Ifexists { get; set; }

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
                ProcedureExecute proc = new ProcedureExecute("prc_ProjectOrder_Details");
                proc.AddVarcharPara("@Action", 100, "GetSimilarProjectCheck");
                proc.AddVarcharPara("@Doc_Type", 100, Doctype);
                proc.AddVarcharPara("@ProjectDetails", 500, quote_Id);
                proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
                proc.RunActionQuery();
                returnValue = Convert.ToString(proc.GetParaValue("@ReturnValue"));

            }
            return returnValue;

        }

        [WebMethod]
        public static object MappedSalesManOnetoOne(string Id)
        {

            DataTable dtContactPerson = new DataTable();
            SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
            List<GetSalesMan> GetSalesMan = new List<GetSalesMan>();
            //dtContactPerson = objSlaesActivitiesBL.PopulateContactPersonOfCustomer(Key);
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GetOneToOnemappedCustomer");
            proc.AddVarcharPara("@CustomerID", 500, Id);
            dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    GetSalesMan.Add(new GetSalesMan
                    {
                        Id = Convert.ToInt32(dt.Rows[i]["SalesmanId"]),
                        Name = Convert.ToString(dt.Rows[i]["Name"])
                        //Ifexists = Convert.ToString(dt.Rows[i]["Name"])
                    });
                }
            }
            return GetSalesMan;
        }
        //End Rev Rajdip



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


        [WebMethod]
        public static object SetProjectCode(Int64 OrderId, string TagDocType)
        {
            List<ProjectDocumentDetails> Detail = new List<ProjectDocumentDetails>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                ProcedureExecute proc = new ProcedureExecute("Prc_ProjectQuotation_Details");
                proc.AddVarcharPara("@Action", 500, "SalesQuotationtaggingProjectdata");
                proc.AddBigIntegerPara("@Order_Id", OrderId);
                proc.AddVarcharPara("@TagDocType", 500, TagDocType);
                DataTable address = proc.GetTable();



                Detail = (from DataRow dr in address.Rows
                          select new ProjectDocumentDetails()

                          {
                              ProjectId = Convert.ToInt64(dr["ProjectId"]),
                              ProjectCode = Convert.ToString(dr["ProjectCode"])
                          }).ToList();
                return Detail;

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

        [WebMethod]
        public static string CheckDeliveryScheduleProductWise(string SODetails_ID,string ProductID,string OrderId)
        {
            string IsExist = "";
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "CheckDeliverySchedule");            
            proc.AddVarcharPara("@ORDER_ID", 100, Convert.ToString(OrderId));
            proc.AddVarcharPara("@Details_Id", 100, Convert.ToString(SODetails_ID));
            proc.AddVarcharPara("@ProductID", 100, Convert.ToString(ProductID));
            DataTable dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                IsExist = Convert.ToString(dt.Rows[0]["IsExist"]);
            }

            return Convert.ToString(IsExist);
        }
        [WebMethod]
        public static string CheckDeliveryScheduleProductQtyWise(string QuantityValue, string ProductID, string OrderId)
        {
            string IsExist = "";
            ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
            proc.AddVarcharPara("@Action", 500, "CheckDeliveryScheduleQTY");
            proc.AddVarcharPara("@ORDER_ID", 100, Convert.ToString(OrderId));
            proc.AddVarcharPara("@QTY", 100, Convert.ToString(QuantityValue));
            proc.AddVarcharPara("@ProductID", 100, Convert.ToString(ProductID));
            DataTable dt = proc.GetTable();

            if (dt != null && dt.Rows.Count > 0)
            {
                IsExist = Convert.ToString(dt.Rows[0]["IsExist"]);
            }

            return Convert.ToString(IsExist);
        }
        

    }



    public class ContactPerson
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string InternalID { get; set; }

    }

}

