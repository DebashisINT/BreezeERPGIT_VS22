/***********************************************************************************************************************************
// 1.0   v2.0.37	Priti	    04-03-2023	    0025690:Alt Qty is not updating as per the main qty while entering Purchase GRN
// 2.0   v2.0.37	Priti	    13-03-2023	    0025723:There is multiple rows inserted in warehouse tables while making stock batch wise in same warehouse in Purchase GRN
// 3.0   V2.0.38    Priti       11-04-2023      0025797:Cannot enter duplicate batch in Same warehouse, for the same product with same batch number
// 4.0   V2.0.39    Sanchita    22-09-2023      GST is showing Zero in the TAX Window whereas GST in the Grid calculated. Mantis: 26843
//                                              Session["MultiUOMData"] has been renamed to Session["MultiUOMDataGRN"]
// 5.0   V2.0.42    Priti       02-01-2024     Mantis : 0027050 A settings is required for the Duplicates Items Allowed or not in the Transaction Module.
// 6.0   V2.0.43    Priti       26-03-2024     0027334: Mfg Date & Exp date should load automatically if the batch details exists for the product while making Purchase GRN.
// 7.0   V2.0.43    Priti       03-04-2024     0027340: GST % able to change in GRN entry. Validation Required like PO and PI

* *******************************************************************************************************************************/


using BusinessLogicLayer;
using BusinessLogicLayer.EmailDetails;
using DataAccessLayer;
using DevExpress.Web;
using DevExpress.Web.Data;
using EntityLayer.CommonELS;
using EntityLayer.MailingSystem;
using ERP.Models;
using ERP.OMS.Tax_Details.ClassFile;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using UtilityLayer;

namespace ERP.OMS.Management.Activities
{
    public partial class PurchaseChallan : ERP.OMS.ViewState_class.VSPage//ERP.OMS.ViewState_class.VSPage//System.Web.UI.Page//PersistViewStateToFileSystem
    {
        // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        public EntityLayer.CommonELS.UserRightsForPage rightsVendor = new UserRightsForPage();
        BusinessLogicLayer.RemarkCategoryBL reCat = new BusinessLogicLayer.RemarkCategoryBL();
        ERPDocPendingApprovalBL objERPDocPendingApproval = new ERPDocPendingApprovalBL();
        SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
        PurchaseChallanBL objPurchaseChallanBL = new PurchaseChallanBL();
        SalesInvoiceBL objSalesInvoiceBL = new SalesInvoiceBL();
        GSTtaxDetails gstTaxDetails = new GSTtaxDetails();
        CommonBL cbl = new CommonBL();
        DataTable Remarks = null;
        public EntityLayer.CommonELS.UserRightsForPage rightsProd = new UserRightsForPage();
        protected void Page_Load(object sender, EventArgs e)
        {
            rightsVendor = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Master/HRrecruitmentagent.aspx?requesttype=VendorService");
            rightsProd = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/store/Master/sProducts.aspx");
            if (Session["userid"] != null)
            {
                //Rev start Tanmoy
                string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
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

                string PurchaseOrderInEntryModule = cbl.GetSystemSettingsResult("RevisionNoDateinPurchasedOrder");
                if (!String.IsNullOrEmpty(PurchaseOrderInEntryModule))
                {
                    if (PurchaseOrderInEntryModule == "Yes")
                    {
                        taggingGrid.Columns[6].Visible = true;
                        taggingGrid.Columns[7].Visible = true;
                    }
                    else if (PurchaseOrderInEntryModule.ToUpper().Trim() == "NO")
                    {
                        taggingGrid.Columns[6].Visible = false;
                        taggingGrid.Columns[7].Visible = false;
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

                // Rev Mantis Issue 24061
                string PurchaseOrderItemNegative = cbl.GetSystemSettingsResult("PurchaseOrderItemNegative");
                if (!String.IsNullOrEmpty(PurchaseOrderItemNegative))
                {
                    if (PurchaseOrderItemNegative == "Yes")
                    {
                        hdnPurchaseOrderItemNegative.Value = "1";

                    }
                    else if (PurchaseOrderItemNegative.ToUpper().Trim() == "NO")
                    {
                        hdnPurchaseOrderItemNegative.Value = "0";

                    }
                }
                // End of Rev Mantis Issue 24061

                //chinmoy added for MUltiUOM settings start

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
                        grid.Columns[8].Width = 0;
                        //Mantis Issue 24429
                        grid.Columns[9].Width = 0;
                        grid.Columns[10].Width = 0;
                        //End of Mantis Issue 24429
                    }
                   
                }

                //End



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
                hdnBackdateddate.Value = "0";
                DataTable Backdateddate=new DataTable();
                Backdateddate = oDBEngine.GetDataTable(" select top 1 ISNULL(tbl.Days_Number,0) Datecount from tbl_BackDated_ListedModule tbl  where  Module_UniqueName='Purchase_GRN'");
                if(Backdateddate !=null && Backdateddate.Rows.Count>0)
                {
                    hdnBackdateddate.Value = Convert.ToString(Backdateddate.Rows[0]["Datecount"]);
                }
                //For Hierarchy End Tanmoy

                if (!IsPostBack)
                {
                    //REV 5.0
                    string IsDuplicateItemAllowedOrNot = cbl.GetSystemSettingsResult("IsDuplicateItemAllowedOrNot");
                    if (!String.IsNullOrEmpty(IsDuplicateItemAllowedOrNot))
                    {
                        if (IsDuplicateItemAllowedOrNot == "Yes")
                        {
                            hdnIsDuplicateItemAllowedOrNot.Value = "1";
                        }
                        else if (IsDuplicateItemAllowedOrNot.ToUpper().Trim() == "NO")
                        {
                            hdnIsDuplicateItemAllowedOrNot.Value = "0";
                        }
                    }
                    //REV 5.0 END
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

                    string BackDatedEntryPurchaseGRN = cbl.GetSystemSettingsResult("BackDatedEntryPurchaseGRN");
                    if (!String.IsNullOrEmpty(BackDatedEntryPurchaseGRN))
                    {
                        if (BackDatedEntryPurchaseGRN.ToUpper().Trim() == "YES")
                        {
                            HdnBackDatedEntryPurchaseGRN.Value = "1";

                        }
                        else if (BackDatedEntryPurchaseGRN.ToUpper().Trim() == "NO")
                        {
                            HdnBackDatedEntryPurchaseGRN.Value = "0";
                          
                        }
                    }


                    VendorDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    ProductDataSource.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    UomSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
                    AltUomSelect.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

                    #region Tax Block

                    string ItemLevelTaxDetails = string.Empty; string HSNCodewisetaxSchemid = string.Empty; string BranchWiseStateTax = string.Empty; string StateCodeWiseStateIDTax = string.Empty;
                    gstTaxDetails.GetTaxData(ref ItemLevelTaxDetails, ref HSNCodewisetaxSchemid, ref BranchWiseStateTax, ref StateCodeWiseStateIDTax, "P");
                    HDItemLevelTaxDetails.Value = ItemLevelTaxDetails;
                    HDHSNCodewisetaxSchemid.Value = HSNCodewisetaxSchemid;
                    HDBranchWiseStateTax.Value = BranchWiseStateTax;
                    HDStateCodeWiseStateIDTax.Value = StateCodeWiseStateIDTax;
                    CreateDataTaxTable();

                    #endregion
                    //Chinmoy comment  below line
                    //Start
                    //CommonBL cbl = new CommonBL();
                    //string ISLigherpage = cbl.GetSystemSettingsResult("LighterVendorEntryPage");
                    //if (!String.IsNullOrEmpty(ISLigherpage))
                    //{
                    //    if (ISLigherpage == "Yes")
                    //    {
                    //        hidIsLigherContactPage.Value = "1";
                    //    }
                    //} 
                    //End
                    string branchHierchy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                    Session["TaxChargeRecord"] = null;
                    Session["MultiUOMDataGRN"] = null;
                    Session["InlineRemarks"] = null;
                    Task PopulateStockTrialDataTask = new Task(() => GetAllDropDownDetailForPurchaseChallan(branchHierchy));
                    PopulateStockTrialDataTask.RunSynchronously();
                    //Tanmoy Hierarchy
                    bindHierarchy();
                    ddlHierarchy.Enabled = false;
                    //Tanmoy Hierarchy End
                    if (IsBarcodeGeneratete() == true) IsBarcodeActive.Value = "Y";
                    else IsBarcodeActive.Value = "";

                    if (Request.QueryString["key"] == "ADD")
                    {
                        DataTable Transactiondt = CreateTempTable("Transaction");
                        Transactiondt.Rows.Add("1", "1", "", "", "", "", "", "0.0000", "", "0.000", "0.00", "0.00", "0.00", "0.00", "0", "0", "", "", "", "I", "0", "0", "0", "0","0","0");

                        DataTable Warehousedt = CreateTempTable("Warehouse");

                        Session["TransactionList"] = Transactiondt;
                        //Session["Product_StockList"] = Warehousedt;
                        Session["Stock_LoopID"] = "1";
                        Session["PurchaseChallan_Id"] = "";
                        Session["PORequiData"] = null;

                        grid.DataSource = GetPurchaseChallanGrid(Transactiondt);
                        grid.DataBind();

                        hdnPageStatus.Value = "ADD";
                        Keyval_internalId.Value = "Add";
                        hdnADDEditMode.Value = "ADD";
                        hdnChallanType.Value = Request.QueryString["InvType"];

                        ddlInventory.SelectedValue = Request.QueryString["InvType"];
                        ddlInventory.Enabled = false;
                        ddl_numberingScheme.Focus();
                        //ddlInventory.Focus();
                    }
                    else
                    {
                        hdnPageStatus.Value = "EDIT";

                        lblHeading.Text = "Modify Purchase GRN";
                        hdnADDEditMode.Value = "Edit";

                        Keyval_internalId.Value = "PurchaseChallan" + Request.QueryString["key"];
                        //Subhra 14-03-2019
                        Keyval_Id.Value = Request.QueryString["key"];
                        //End Subhra 14-03-2019

                        //Chinmoy added below code
                        //Start
                        //lbl_NumberingScheme.Visible = false;
                        //ddl_numberingScheme.Visible = false;
                        divNumberingScheme.Style.Add("display", "none");
                        //divNumberingScheme.Visible = false;
                        //End



                        string PC_ID = Convert.ToString(Request.QueryString["key"]);

                       


                        Session["PurchaseChallan_Id"] = PC_ID;

                        DataSet ds_GRNDetails = GetGRNDetails(PC_ID);
                        ddlPosGstChallan.DataSource = ds_GRNDetails.Tables[7];
                        ddlPosGstChallan.ValueField = "ID";
                        ddlPosGstChallan.TextField = "Name";
                        ddlPosGstChallan.DataBind();

                        DataTable Details_dt = ds_GRNDetails.Tables[0];
                        DataTable Transaction_dt = ds_GRNDetails.Tables[1];
                        DataTable Warehouse_dt = ds_GRNDetails.Tables[2];
                        DataTable ProductTax_dt = ds_GRNDetails.Tables[3];
                        DataTable Tax_dt = ds_GRNDetails.Tables[4];
                        DataTable Calculate_dt = ds_GRNDetails.Tables[5];
                        Session["InlineRemarks"] = ds_GRNDetails.Tables[8];
                        //Chinmoy added below line
                        Purchase_BillingShipping.SetBillingShippingTable(ds_GRNDetails.Tables[6]);

                        Session["TransactionList"] = Transaction_dt;
                        //Session["Product_StockList"] = GetChallanWarehouseData(Warehouse_dt);
                        Session["TaxChargeRecord"] = GetTaxDataWithGST(GetTaxData(dt_PLQuote.Date.ToString("yyyy-MM-dd")));


                        var jsonSerialiser = new JavaScriptSerializer();
                        DataTable _Warehouse_dt = GetChallanWarehouseData(Warehouse_dt);
                        var json = jsonSerialiser.Serialize(GetStock(_Warehouse_dt));
                        hdnJsonTempStock.Value = json;

                        Fill_GRNDetails(Details_dt);
                        CalculateGRNAmount(Calculate_dt);
                        Session["MultiUOMDataGRN"] = GetMultiUOMData();
                        grid.DataSource = GetPurchaseChallanGrid(Transaction_dt);
                        grid.DataBind();

                        if (ProductTax_dt == null)
                        {
                            CreateDataTaxTable();
                        }
                        else
                        {
                            Session["Product_TaxRecord"] = ProductTax_dt;
                        }

                        #region Approval Section

                        if (Request.QueryString["status"] == null)
                        {
                            IsExistsDocumentInERPDocApproveStatus(PC_ID);
                        }

                        #endregion

                        #region View Section

                        if (Request.QueryString["req"] == "V")
                        {
                            lblHeading.Text = "View Purchase GRN";
                            btn_SaveRecords.Visible = false;
                            btn_SaveRecordsExit.Visible = false;
                            lbl_quotestatusmsg.Text = "*** View Mode Only";
                        }
                        else
                        {
                            bool IsExist = IsPITransactionExist(Convert.ToString(Session["PurchaseChallan_Id"]));
                            if (IsExist == true)
                            {
                                btn_SaveRecords.Visible = false;
                                btn_SaveRecordsExit.Visible = false;
                                lbl_IsTagged.Text = "*** This Purchase Challan is tagged in other modules. Cannot Modify";
                            }
                        }

                        #endregion

                        BindWarehouse();
                        ddl_numberingScheme.Focus();
                    }


                    if (IsBarcodeGeneratete() == true) hdfIsBarcodeGenerator.Value = "Y";
                    else hdfIsBarcodeGenerator.Value = "N";
                    VisiblitySendEmail();
                }
                //Rev Subhra 13-03-2019
                MasterSettings objmaster = new MasterSettings();
                hdnConvertionOverideVisible.Value = objmaster.GetSettings("ConvertionOverideVisible");
                hdnShowUOMConversionInEntry.Value = objmaster.GetSettings("ShowUOMConversionInEntry");
                //End of Rev 
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            //Rev 01 0018192 Subhra start
            //if (Request.QueryString.AllKeys.Contains("status"))
            if (Request.QueryString.AllKeys.Contains("status") || Request.QueryString.AllKeys.Contains("IsTagged"))
            {
                //End of Rev Subhra 
                this.Page.MasterPageFile = "~/OMS/MasterPage/PopUp.Master";
            }
            else
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/ERP.Master";
            }

            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        #region Page Classes

        public DataTable GetMultiUOMData()
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_Purchasechallan_Details");
            // Rev Mantis Issue 24429
            //proc.AddVarcharPara("@Action", 500, "MultiUOMQuotationDetails");
            proc.AddVarcharPara("@Action", 500, "MultiUOMQuotationDetails_New");
            // End of Rev Mantis Issue 24429
            proc.AddVarcharPara("@ChallanId", 500, Convert.ToString(Session["PurchaseChallan_Id"]));
            ds = proc.GetTable();
            return ds;
        }

        public DataTable CreateTempTable(string Type)
        {

            DataTable dt = new DataTable();
            
            if (Type == "Transaction")
            {
                dt.Columns.Add("SrlNo", typeof(string));
                dt.Columns.Add("RowNo", typeof(string));
                dt.Columns.Add("DocNumber", typeof(string));
                dt.Columns.Add("DocID", typeof(string));
                dt.Columns.Add("ProductID", typeof(string));
                dt.Columns.Add("ProductName", typeof(string));
                dt.Columns.Add("ProductDiscription", typeof(string));
                dt.Columns.Add("Quantity", typeof(string));
                dt.Columns.Add("PurchaseUOM", typeof(string));
                dt.Columns.Add("PurchasePrice", typeof(string));
                dt.Columns.Add("Discount", typeof(string));
                dt.Columns.Add("TotalAmount", typeof(string));
                dt.Columns.Add("TaxAmount", typeof(string));
                dt.Columns.Add("NetAmount", typeof(string));
                dt.Columns.Add("TotalQty", typeof(string));
                dt.Columns.Add("BalanceQty", typeof(string));
                dt.Columns.Add("IsComponentProduct", typeof(string));
                dt.Columns.Add("IsLinkedProduct", typeof(string));
                dt.Columns.Add("ComponentProduct", typeof(string));
                dt.Columns.Add("Status", typeof(string));
                dt.Columns.Add("DetailsId",typeof(string));
                dt.Columns.Add("ChallanDetails_InlineRemarks", typeof(string));
                // Rev Mantis Issue 24061
                dt.Columns.Add("Balance_Amount", typeof(string));
                // End of Rev Mantis Issue 24061
                // Mantis Issue 24429
                dt.Columns.Add("Order_AltQuantity", typeof(string));
                dt.Columns.Add("Order_AltUOM", typeof(string));
                // End of Mantis Issue 24429

                dt.Columns.Add("ChallanDetails_Id", typeof(string));
            }
            else if (Type == "Warehouse")
            {
                dt.Columns.Add("Product_SrlNo", typeof(string));
                dt.Columns.Add("SrlNo", typeof(int));
                dt.Columns.Add("WarehouseID", typeof(string));
                dt.Columns.Add("WarehouseName", typeof(string));
                dt.Columns.Add("Quantity", typeof(string));
                dt.Columns.Add("TotalQuantity", typeof(string));
                dt.Columns.Add("SalesQuantity", typeof(string));
                dt.Columns.Add("SalesUOMName", typeof(string));
                dt.Columns.Add("BatchID", typeof(string));
                dt.Columns.Add("BatchNo", typeof(string));
                dt.Columns.Add("MfgDate", typeof(string));
                dt.Columns.Add("ExpiryDate", typeof(string));
                dt.Columns.Add("SerialNo", typeof(string));
                dt.Columns.Add("LoopID", typeof(string));
                dt.Columns.Add("Status", typeof(string));
                dt.Columns.Add("ViewMfgDate", typeof(string));
                dt.Columns.Add("ViewExpiryDate", typeof(string));
                dt.Columns.Add("IsOutStatus", typeof(string));
                dt.Columns.Add("IsOutStatusMsg", typeof(string));

                dt.Columns["IsOutStatus"].DefaultValue = "visibility: visible";
                dt.Columns["IsOutStatusMsg"].DefaultValue = "visibility: hidden";
            }

            return dt;
        }


        public IEnumerable GetProjectQuotationInfo(DataTable ProjectOrderdt1)
        {
            List<ProjectPurchaseOrder> OrderList = new List<ProjectPurchaseOrder>();
            DataColumnCollection dtC = ProjectOrderdt1.Columns;
            DataTable dtdfg = new DataTable();


            dtdfg.Columns.Add("SrlNo", typeof(string));
            dtdfg.Columns.Add("ChallanDetails_AddiDesc", typeof(string));

            for (int i = 0; i < ProjectOrderdt1.Rows.Count; i++)
            {
                ProjectPurchaseOrder Orders = new ProjectPurchaseOrder();

                Orders.SrlNo = Convert.ToString(i + 1);
                Orders.ChallanDetails_AddiDesc = Convert.ToString(ProjectOrderdt1.Rows[i]["ChallanDetails_AddiDesc"]);
                OrderList.Add(Orders);
                dtdfg.Rows.Add(Orders.SrlNo, Orders.ChallanDetails_AddiDesc);

            }


            Session["InlineRemarks"] = dtdfg;
            return OrderList;
        }

        public IEnumerable GetPurchaseChallanGrid(DataTable dt)
        {
            List<TransactionList> TransactionList = new List<TransactionList>();


            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToString(dt.Rows[i]["Status"]) != "D")
                    {
                        TransactionList Transactions = new TransactionList();
                       
                        Transactions.SrlNo = Convert.ToString(dt.Rows[i]["SrlNo"]);
                        Transactions.RowNo = Convert.ToString(dt.Rows[i]["RowNo"]);
                        Transactions.DocNumber = Convert.ToString(dt.Rows[i]["DocNumber"]);
                        Transactions.DocID = Convert.ToString(dt.Rows[i]["DocID"]);
                        Transactions.ProductID = Convert.ToString(dt.Rows[i]["ProductID"]);
                        Transactions.ProductName = Convert.ToString(dt.Rows[i]["ProductName"]);
                        Transactions.ProductDiscription = Convert.ToString(dt.Rows[i]["ProductDiscription"]);
                        Transactions.Quantity = Convert.ToString(dt.Rows[i]["Quantity"]);
                        hdnTaggedQuantity.Value = Convert.ToString(dt.Rows[i]["Quantity"]);
                        Transactions.PurchaseUOM = Convert.ToString(dt.Rows[i]["PurchaseUOM"]);
                        Transactions.PurchasePrice = Convert.ToString(dt.Rows[i]["PurchasePrice"]);
                        Transactions.Discount = Convert.ToString(dt.Rows[i]["Discount"]);
                        Transactions.TotalAmount = Convert.ToString(dt.Rows[i]["TotalAmount"]);
                        Transactions.TaxAmount = Convert.ToString(dt.Rows[i]["TaxAmount"]);
                        Transactions.NetAmount = Convert.ToString(dt.Rows[i]["NetAmount"]);
                        Transactions.TotalQty = Convert.ToString(dt.Rows[i]["TotalQty"]);
                        Transactions.BalanceQty = Convert.ToString(dt.Rows[i]["BalanceQty"]);
                        Transactions.IsComponentProduct = Convert.ToString(dt.Rows[i]["IsComponentProduct"]);
                        Transactions.IsLinkedProduct = Convert.ToString(dt.Rows[i]["IsLinkedProduct"]);
                        Transactions.ComponentProduct = Convert.ToString(dt.Rows[i]["ComponentProduct"]);
                        Transactions.Status = Convert.ToString(dt.Rows[i]["Status"]);
                        Transactions.DetailsId = Convert.ToString(dt.Rows[i]["DetailsId"]);
                        if (dt.Columns.Contains("ChallanDetails_InlineRemarks"))
                        {
                            Transactions.ChallanDetails_InlineRemarks = Convert.ToString(dt.Rows[i]["ChallanDetails_InlineRemarks"]);
                        }
                        else 
                        {
                            Transactions.ChallanDetails_InlineRemarks = "";
                        }
                        // Rev Mantis Issue 24061
                        if (dt.Columns.Contains("Balance_Amount"))
                        {
                            Transactions.Balance_Amount = Convert.ToString(dt.Rows[i]["Balance_Amount"]);
                        }
                        else
                        {
                            Transactions.Balance_Amount = Convert.ToString(0);
                        }
                        // End of Rev Mantis Issue 24061
                        // Mantis Issue 24429
                        Transactions.Order_AltQuantity = Convert.ToString(dt.Rows[i]["Order_AltQuantity"]);
                        Transactions.Order_AltUOM = Convert.ToString(dt.Rows[i]["Order_AltUOM"]);
                        // End of Mantis Issue 24429

                        if (dt.Columns.Contains("ChallanDetails_Id"))
                        {
                            Transactions.ChallanDetails_Id = Convert.ToString(dt.Rows[i]["ChallanDetails_Id"]);
                        }
                        else
                        {
                            Transactions.ChallanDetails_Id = Convert.ToString(0);
                        }
                        TransactionList.Add(Transactions);
                    }
                }


            }

            return TransactionList;
        }
        public class ProjectPurchaseOrder
        {
            public string SrlNo { get; set; }
            public string ChallanDetails_AddiDesc { get; set; }
        }
        public class TransactionList
        {
            public string SrlNo { get; set; }
            public string RowNo { get; set; }
            public string DocNumber { get; set; }
            public string DocID { get; set; }
            public string ProductID { get; set; }
            public string ProductName { get; set; }
            public string ProductDiscription { get; set; }
            public string Quantity { get; set; }
            public string PurchaseUOM { get; set; }
            public string PurchasePrice { get; set; }
            public string Discount { get; set; }
            public string TotalAmount { get; set; }
            public string TaxAmount { get; set; }
            public string NetAmount { get; set; }
            public string TotalQty { get; set; }
            public string BalanceQty { get; set; }
            public string IsComponentProduct { get; set; }
            public string IsLinkedProduct { get; set; }
            public string ComponentProduct { get; set; }
            public string Status { get; set; }
            public string DetailsId { get; set; }
            public string ChallanDetails_InlineRemarks { get; set; }
            // Rev Mantis Issue 24061
            public string Balance_Amount { get; set; }
            // End of Rev Mantis Issue 24061
            // Mantis Issue 24429
            public string Order_AltQuantity { get; set; }
            public string Order_AltUOM { get; set; }
            // End of Mantis Issue 24429

            public string ChallanDetails_Id { get; set; }
        }
        public class Taxes
        {
            public string TaxID { get; set; }
            public string TaxName { get; set; }
            public string Percentage { get; set; }
            public string Amount { get; set; }
            public decimal calCulatedOn { get; set; }
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
        public class TaxSetailsJson
        {
            public string SchemeName { get; set; }
            public int vissibleIndex { get; set; }
            public string applicableOn { get; set; }
            public string applicableBy { get; set; }
        }

        #endregion

        #region GRN Other Details

        public void GetAllDropDownDetailForPurchaseChallan(string branchHierchy)
        {
            DataSet dst = new DataSet();
            dst = objPurchaseChallanBL.GetNewAllDropDownDetailForPurchaseChallan(branchHierchy);

            //Numbering Scheme
            ddl_numberingScheme.DataSource = dst.Tables[1];
            ddl_numberingScheme.DataBind();

            //set Branch
            ddl_Branch.DataSource = dst.Tables[0];
            ddl_Branch.DataBind();

            ddlForBranch.DataSource = dst.Tables[0];
            ddlForBranch.DataBind();


            //set Tax Type
            if (dst.Tables[2] != null && dst.Tables[2].Rows.Count > 0)
            {
                ddl_AmountAre.TextField = "taxGrp_Description";
                ddl_AmountAre.ValueField = "taxGrp_Id";
                ddl_AmountAre.DataSource = dst.Tables[2];
                ddl_AmountAre.DataBind();
            }

            //set Branch
            if (dst.Tables[4] != null && dst.Tables[4].Rows.Count > 0)
            {
                ddl_Currency.DataSource = dst.Tables[4];
                ddl_Currency.DataBind();
            }

            // NO TAX IN GRN - Checking
            if (dst.Tables[3] != null && dst.Tables[3].Rows.Count > 0)
            {
                string IsMandatory = Convert.ToString(dst.Tables[3].Rows[0]["Variable_Value"]).Trim();

                if (IsMandatory == "Yes")
                {
                    ddl_AmountAre.Value = "3";
                    ddl_AmountAre.ClientEnabled = false;
                }
            }

            if (dst.Tables[5] != null && dst.Tables[5].Rows.Count > 0)
            {
                string strVariableName = Convert.ToString(dst.Tables[5].Rows[0]["Variable_Value"]).Trim();
                if (strVariableName == "Yes")
                {
                    hdfEWayBillMendatory.Value = "Yes";
                }
                else
                {
                    hdfEWayBillMendatory.Value = "No";
                }


            }

            // Purchase GRN Date

            string finyear = "";
            DateTime MinDate, MaxDate;

            if (Session["LastFinYear"] != null)
            {
                finyear = Convert.ToString(Session["LastFinYear"]).Trim();
                DataTable dtFinYear = objSlaesActivitiesBL.GetFinacialYearBasedQouteDate(finyear);
                if (dtFinYear != null && dtFinYear.Rows.Count > 0)
                {
                    MinDate = Convert.ToDateTime(dtFinYear.Rows[0]["finYearStartDate"]);
                    MaxDate = Convert.ToDateTime(dtFinYear.Rows[0]["finYearEndDate"]);

                    dt_PLQuote.Value = null;
                    dt_PLQuote.MinDate = MinDate;
                    dt_PLQuote.MaxDate = MaxDate;

                    if (DateTime.Now > MaxDate)
                    {
                        dt_PLQuote.Value = MaxDate;
                    }
                    else
                    {
                        dt_PLQuote.Value = DateTime.Now;
                    }
                }
            }

            if (!String.IsNullOrEmpty(Convert.ToString(Session["LocalCurrency"])))
            {
                string CompID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string LocalCurrency = Convert.ToString(Session["LocalCurrency"]);
                string basedCurrency = Convert.ToString(LocalCurrency.Split('~')[0]);
                string CurrencyId = Convert.ToString(basedCurrency[0]);
                ddl_Currency.SelectedValue = CurrencyId;
            }

            IsUdfpresent.Value = Convert.ToString(getUdfCount());

            //BindWarehouse();
        }
        protected void cmbContactPerson_Callback(object sender, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];
            if (WhichCall == "BindContactPerson")
            {
                string InternalId = Convert.ToString(Convert.ToString(e.Parameter.Split('~')[1]));
                string vendorbranchid = Convert.ToString(ddl_Branch.SelectedValue);

                PopulateContactPersonOfCustomer(InternalId);

                DataSet vendst = new DataSet();
                PurchaseInvoiceBL objPurchaseInvoice = new PurchaseInvoiceBL();
                vendst = objPurchaseInvoice.GetCustomerDetails_InvoiceRelated(InternalId, vendorbranchid);

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
        protected void PopulateContactPersonOfCustomer(string InternalId)
        {
            string ContactPerson = "";
            DataTable dtContactPerson = new DataTable();
            dtContactPerson = objPurchaseChallanBL.PopulateContactPersonOfCustomer(InternalId);
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
        }
        protected int getUdfCount()
        {
            DataTable udfCount = oDBEngine.GetDataTable("select 1 from tbl_master_remarksCategory rc where cat_applicablefor='PC' and ( exists (select * from tbl_master_udfGroup where id=rc.cat_group_id and grp_isVisible=1) or rc.cat_group_id=0)");
            return udfCount.Rows.Count;
        }
        public DataSet GetGRNDetails(string PC_ID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("proc_Fetch_PruchaseChallanDetails");
            // Rev Mantis Issue 24429
            //proc.AddVarcharPara("@Action", 500, "GRNDetails");
            proc.AddVarcharPara("@Action", 500, "GRNDetails_New");
            // End of Rev Mantis Issue 24429
            proc.AddVarcharPara("@PurchaseChallan_Id", 500, PC_ID);
            ds = proc.GetDataSet();
            return ds;
        }
        public void Fill_GRNDetails(DataTable dt)
        {


            if (dt != null && dt.Rows.Count > 0)
            {
                string PurchaseOrder_Number = Convert.ToString(dt.Rows[0]["PurchaseChallan_Number"]);
                string PurchaseOrder_IndentId = Convert.ToString(dt.Rows[0]["PurchaseChallan_PurchaseOrderId"]);
                string PurchaseOrder_BranchId = Convert.ToString(dt.Rows[0]["PurchaseChallan_BranchId"]);
                TermsConditionsControl.SetBranchId(PurchaseOrder_BranchId);
                string PurchaseOrder_Date = Convert.ToString(dt.Rows[0]["PurchaseChallan_Date"]);
                string Customer_Id = Convert.ToString(dt.Rows[0]["PurchaseChallan_VendorId"]);
                string VendorName = Convert.ToString(dt.Rows[0]["VendorName"]);
                string Contact_Person_Id = Convert.ToString(dt.Rows[0]["Contact_Person_Id"]);
                string PurchaseOrder_Reference = Convert.ToString(dt.Rows[0]["PurchaseChallan_Reference"]);
                string PurchaseOrder_Currency_Id = Convert.ToString(dt.Rows[0]["PurchaseChallan_Currency_Id"]);
                string Currency_Conversion_Rate = Convert.ToString(dt.Rows[0]["Currency_Conversion_Rate"]);
                string Tax_Option = Convert.ToString(dt.Rows[0]["Tax_Option"]);
                string Tax_Code = Convert.ToString(dt.Rows[0]["Tax_Code"]);
                string PurchaseOrderDate = Convert.ToString(dt.Rows[0]["OrderDate"]);
                string IsInventory = Convert.ToString(dt.Rows[0]["IsInventoryType"]);
                string strPartyInvoice = Convert.ToString(dt.Rows[0]["PartyInvoiceNo"]);
                string strPartyDate = Convert.ToString(dt.Rows[0]["PartyInvoiceDate"]);
                string SchemeID = Convert.ToString(dt.Rows[0]["SchemeID"]);
                string PosForGst = Convert.ToString(dt.Rows[0]["PosForGst"]);
                string EWayBillNumber = Convert.ToString(dt.Rows[0]["EWayBillNumber"]);
                string Narrarion = Convert.ToString(dt.Rows[0]["Narration"]);
                ddlPosGstChallan.Value = PosForGst;

                txtEWayBillNumber.Text = EWayBillNumber.Trim();
                txtNarration.Text = Narrarion;
                if (IsInventory != "") ddlInventory.SelectedValue = IsInventory;
                ddlInventory.Enabled = false;

                if (SchemeID != "0") ddl_numberingScheme.SelectedValue = SchemeID;
                ddl_numberingScheme.Enabled = false;

                txtPartyInvoice.Text = strPartyInvoice;
                if (strPartyDate != "") dt_PartyDate.Date = Convert.ToDateTime(strPartyDate);

                txtVoucherNo.Text = PurchaseOrder_Number;
                txtVoucherNo.ReadOnly = true;
                dt_PLQuote.Date = Convert.ToDateTime(PurchaseOrder_Date);

                txtCustName.Text = VendorName;
                hdnCustomerId.Value = Customer_Id;
                PopulateContactPersonOfCustomer(Customer_Id);

                if (Contact_Person_Id != "0")
                {
                    cmbContactPerson.Value = Contact_Person_Id;
                }

                ProcedureExecute GetVendorGstin = new ProcedureExecute("prc_GstTaxDetails");
                GetVendorGstin.AddVarcharPara("@Action", 500, "GetVendorGSTINByBranch");
                GetVendorGstin.AddVarcharPara("@branchId", 10, Convert.ToString(ddl_Branch.SelectedValue));
                GetVendorGstin.AddVarcharPara("@entityId", 10, Convert.ToString(Customer_Id));
                DataTable VendorGstin = GetVendorGstin.GetTable();

                if (VendorGstin.Rows.Count > 0)
                {
                    if (Convert.ToString(VendorGstin.Rows[0][0]).Trim() != "")
                    {
                        //Edited By Chinmoy
                        // this.BillingShippingControl.hfVendorGSTIN.Value = Convert.ToString(VendorGstin.Rows[0][0]).Trim();
                        string VendorGst = Convert.ToString(VendorGstin.Rows[0][0]).Trim();
                        this.Purchase_BillingShipping.GetGSTIN(VendorGst);
                    }
                }

                txt_Refference.Text = PurchaseOrder_Reference;
                ddl_Branch.SelectedValue = PurchaseOrder_BranchId;
                ddl_Currency.SelectedValue = PurchaseOrder_Currency_Id;
                txt_Rate.Text = Currency_Conversion_Rate;

                if (Tax_Option != "0")
                {
                    if (Tax_Option == "4")
                    {
                        ListEditItem li = new ListEditItem();
                        li.Text = "Import";
                        li.Value = "4";
                        ddl_AmountAre.Items.Insert(3, li);
                    }

                    ddl_AmountAre.Value = Tax_Option;
                }

                ddl_AmountAre.ClientEnabled = false;
                ddl_VatGstCst.ClientEnabled = false;
                txtCustName.ClientEnabled = false;
                dt_PLQuote.ClientEnabled = false;
                ddl_Currency.Enabled = false;
                txt_Rate.Enabled = false;
                taggingList.ClientEnabled = false;

                string dates = Convert.ToDateTime(PurchaseOrder_Date).ToString("yyyy-MM-dd");
                string strChallanID = Convert.ToString(Session["PurchaseChallan_Id"]);
                string FinYear = Convert.ToString(Session["LastFinYear"]);
                string Action = "GetPurchaseOrder";

                #region Order Tagging

                DataTable IndentTable = objSalesInvoiceBL.GetComponent(Customer_Id, dates, "", FinYear, PurchaseOrder_BranchId, Action, strChallanID);
                Session["PORequiData"] = IndentTable;

                taggingGrid.DataSource = IndentTable;
                taggingGrid.DataBind();

                string[] eachQuo = PurchaseOrder_IndentId.Split(',');
                string QuoComponentNumber = "", QuoComponentDate = "";

                for (int i = 0; i < taggingGrid.VisibleRowCount; i++)
                {
                    string PurchaseOrder_Id = Convert.ToString(taggingGrid.GetRowValues(i, "PurchaseOrder_Id"));
                    if (eachQuo.Contains(PurchaseOrder_Id))
                    {
                        QuoComponentNumber += "," + Convert.ToString(taggingGrid.GetRowValues(i, "PurchaseOrder_Number"));
                        QuoComponentDate += "," + Convert.ToString(taggingGrid.GetRowValues(i, "ComponentDate"));

                        taggingGrid.Selection.SelectRow(i);
                    }
                }

                QuoComponentNumber = QuoComponentNumber.TrimStart(',');
                QuoComponentDate = QuoComponentDate.TrimStart(',');

                if (taggingGrid.GetSelectedFieldValues("PurchaseOrder_Id").Count > 0)
                {
                    if (taggingGrid.GetSelectedFieldValues("PurchaseOrder_Id").Count > 1)
                    {
                        QuoComponentDate = "Multiple Purchase Order Dates";
                    }
                }
                else
                {
                    QuoComponentDate = "";
                }

                dt_Quotation.Text = QuoComponentDate;
                taggingList.Text = QuoComponentNumber;
                if(QuoComponentNumber !="")
                {
                    BusinessLogicLayer.DBEngine oDB= new BusinessLogicLayer.DBEngine();
                    DataTable dts = oDB.GetDataTable("select min(convert(varchar(50),PurchaseOrder_Date,110)) PurchaseOrder_Date from tbl_trans_PurchaseOrder where PurchaseOrder_Id in  (select s FROM dbo.GetSplit(',','" + PurchaseOrder_IndentId + "')) ");
                    if(dts!=null && dts.Rows.Count>0)
                    {
                        hdnTagDateForbackdated.Value = Convert.ToString(dts.Rows[0]["PurchaseOrder_Date"]);        
                    }
                }
             
                #endregion

                #region Order Product Tagging

                string strAction = "GetPurchaseOrderProducts";
                DataTable dtDetails = objSalesInvoiceBL.GetComponentProductList(strAction, PurchaseOrder_IndentId, strChallanID);

                grid_Products.DataSource = dtDetails;
                grid_Products.DataBind();

                DataTable Transaction_dt = (DataTable)Session["TransactionList"];

                for (int i = 0; i < grid_Products.VisibleRowCount; i++)
                {
                    string ComponentID = Convert.ToString(grid_Products.GetRowValues(i, "ComponentID"));
                    string ProductList = Convert.ToString(grid_Products.GetRowValues(i, "ProductID"));

                    string[] list = ProductList.Split(new string[] { "||@||" }, StringSplitOptions.None);
                    string ProductID = Convert.ToString(list[0]) + "||@||%";

                    var Checkdt = Transaction_dt.Select("DocID='" + ComponentID + "' AND ProductID LIKE '" + ProductID + "'");
                    if (Checkdt.Length > 0)
                    {
                        grid_Products.Selection.SelectRow(i);
                    }
                }

                #endregion

                //Rev Tanmoy for Project
                string ProjectSelectEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");

                lookup_Project.GridView.Selection.SelectRowByKey(Convert.ToInt64(dt.Rows[0]["Proj_Id"]));

                //Tanmoy  Hierarchy
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                DataTable dt2 = oDBEngine.GetDataTable("Select Hierarchy_ID from V_ProjectList where Proj_Id='" + Convert.ToInt64(dt.Rows[0]["Proj_Id"]) + "'");
                if (dt2.Rows.Count > 0)
                {
                    ddlHierarchy.SelectedValue = Convert.ToString(dt2.Rows[0]["Hierarchy_ID"]);
                }
                //Tanmoy  Hierarchy End
                if (Convert.ToString(dt.Rows[0]["PurchaseChallan_PurchaseOrderId"]).Trim() != "")
                {
                    lookup_Project.ClientEnabled = false;
                }
                //End Rev
            }
        }
        public DataTable GetChallanWarehouseData(DataTable dt)
        {
            try
            {
                DataTable tempdt = new DataTable();
                if (dt != null && dt.Rows.Count > 0)
                {
                    var CheckStockOut_dt = dt.Select("IsOutStatusMsg='visibility: visible'");
                    if (CheckStockOut_dt.Length > 0)
                    {
                        dt_PLQuote.ClientEnabled = false;
                    }
                    tempdt = dt.Copy();

                    //Rev 2.0
                    //string strNewVal = "", strOldVal = "", strProductType = "";
                    //foreach (DataRow drr in tempdt.Rows)
                    //{
                    //    strNewVal = Convert.ToString(drr["QuoteWarehouse_Id"]);
                    //    strProductType = Convert.ToString(drr["ProductType"]);
                    //    if (strNewVal == strOldVal)
                    //    {
                    //        drr["WarehouseName"] = "";
                    //        drr["TotalQuantity"] = "0";
                    //        //drr["BatchNo"] = "";
                    //        drr["ViewBatch"] = "";
                    //        drr["SalesQuantity"] = "";
                    //        drr["ViewMfgDate"] = "";
                    //        drr["ViewExpiryDate"] = "";
                    //    }
                    //    strOldVal = strNewVal;
                    //}
                    //Rev 2.0 End

                    tempdt.Columns.Remove("QuoteWarehouse_Id");
                    tempdt.Columns.Remove("ProductType");

                    string maxLoopID = GetWarehouseMaxLoopValue(tempdt);
                    Session["PC_LoopWarehouse"] = maxLoopID;
                }
                else
                {
                    tempdt.Columns.Add("Product_SrlNo", typeof(string));
                    tempdt.Columns.Add("SrlNo", typeof(int));
                    tempdt.Columns.Add("WarehouseID", typeof(string));
                    tempdt.Columns.Add("WarehouseName", typeof(string));
                    tempdt.Columns.Add("Quantity", typeof(string));
                    tempdt.Columns.Add("TotalQuantity", typeof(string));
                    tempdt.Columns.Add("SalesQuantity", typeof(string));
                    tempdt.Columns.Add("SalesUOMName", typeof(string));
                    tempdt.Columns.Add("BatchID", typeof(string));
                    tempdt.Columns.Add("BatchNo", typeof(string));
                    tempdt.Columns.Add("MfgDate", typeof(string));
                    tempdt.Columns.Add("ExpiryDate", typeof(string));
                    tempdt.Columns.Add("SerialNo", typeof(string));
                    tempdt.Columns.Add("LoopID", typeof(string));
                    tempdt.Columns.Add("Status", typeof(string));
                    tempdt.Columns.Add("ViewMfgDate", typeof(string));
                    tempdt.Columns.Add("ViewExpiryDate", typeof(string));
                    tempdt.Columns.Add("IsOutStatus", typeof(string));
                    tempdt.Columns.Add("IsOutStatusMsg", typeof(string));

                    tempdt.Columns.Add("AltQty", typeof(string));
                    tempdt.Columns.Add("AltUOM", typeof(string));

                    Session["PC_LoopWarehouse"] = "1";
                }

                return tempdt;
            }
            catch
            {
                return null;
            }
        }
        public void CalculateGRNAmount(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                string ProductAmount = Convert.ToString(dt.Rows[0]["ProductAmount"]);
                string ProductTotalAmount = Convert.ToString(dt.Rows[0]["ProductTotalAmount"]);
                string NetAmount = Convert.ToString(dt.Rows[0]["NetAmount"]);
                string ChargesAmount = Convert.ToString(dt.Rows[0]["ChargesAmount"]);
                string ProductTaxAmount = Convert.ToString(dt.Rows[0]["ProductTaxAmount"]);
                string ProductQuantity = Convert.ToString(dt.Rows[0]["ProductQuantity"]);

                txt_Charges.Text = ChargesAmount;
                txt_TaxableAmtval.Text = ProductAmount;
                txt_OtherTaxAmtval.Text = ChargesAmount;
                txt_TaxAmtval.Text = ProductTaxAmount;
                txt_cInvValue.Text = ProductTotalAmount;
                txt_TotalAmt.Text = NetAmount;
                txt_TotalQty.Text = ProductQuantity;
            }
        }
        private bool IsPITransactionExist(string PIid)
        {
            bool IsExist = false;
            if (PIid != "" && Convert.ToString(PIid).Trim() != "")
            {
                DataTable dt = new DataTable();
                dt = CheckPITraanaction(PIid);
                if (dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0]["isexist"]) != "0")
                {
                    IsExist = true;
                }
            }

            return IsExist;
        }
        public void IsExistsDocumentInERPDocApproveStatus(string orderId)
        {
            string editable = "";
            string status = "";
            DataTable dt = new DataTable();
            int Id = Convert.ToInt32(orderId);
            dt = objERPDocPendingApproval.IsExistsDocumentInERPDocApproveStatus(Id, 10); // 2 for Purchase Challan Invoice
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
        public void VisiblitySendEmail()
        {
            Employee_BL objemployeebal = new Employee_BL();
            chkmail.Visible = true;
            if (Request.QueryString["key"] != "ADD")
            {
                chkmail.Visible = false;
            }

            DataTable dt2 = new DataTable();
            dt2 = objemployeebal.GetSystemsettingmail("Show Email in GRN");
            if (Convert.ToString(dt2.Rows[0]["Variable_Value"]) == "Yes")
            {
                chkmail.Visible = true;
            }
            else
            {
                chkmail.Visible = false;
            }
        }
        public DataTable CheckPITraanaction(string piid)
        {
            DataTable dt = new DataTable();

            ProcedureExecute proc = new ProcedureExecute("prc_Purchasechallan_Details");
            proc.AddVarcharPara("@Action", 100, "PurchaseChallanTagOrNot");
            proc.AddVarcharPara("@PCID", 200, piid);

            dt = proc.GetTable();
            return dt;
        }
        protected void DeletePanel_Callback(object source, CallbackEventArgs e)
        {
            string performpara = e.Parameter;
            if (performpara.Split('~')[0] == "DelProdbySl")
            {
                string Product_SlNo = Convert.ToString(performpara.Split('~')[1]);

                #region Delete Product Tax

                DataTable dt_ProductTax = (DataTable)Session["Product_TaxRecord"];
                DataRow[] deletedRow = dt_ProductTax.Select("SlNo=" + Product_SlNo);
                if (deletedRow.Length > 0)
                {
                    foreach (DataRow dr in deletedRow)
                    {
                        dt_ProductTax.Rows.Remove(dr);
                    }
                    dt_ProductTax.AcceptChanges();
                }
                Session["Product_TaxRecord"] = dt_ProductTax;

                #endregion

                #region Delete Tax & Charges

                DataTable taxDetails = (DataTable)Session["Product_TaxRecord"];
                if (taxDetails != null && taxDetails.Rows.Count > 0)
                {
                    foreach (DataRow dr in taxDetails.Rows)
                    {
                        dr["Amount"] = "0.00";
                    }
                    dt_ProductTax.AcceptChanges();
                }
                Session["Product_TaxRecord"] = taxDetails;

                #endregion

                DeleteWarehouse(Product_SlNo);
            }
        }


        [WebMethod(EnableSession = true)]
        public static object taxUpdatePanel_Callback(string Action, string srl)
        {
            string output = "200";
            try
            {              
                if (Action == "DelProdbySl")
                {
                    string Product_SlNo = Convert.ToString(srl);

                    #region Delete Product Tax

                    DataTable dt_ProductTax = (DataTable)HttpContext.Current.Session["Product_TaxRecord"];
                    DataRow[] deletedRow = dt_ProductTax.Select("SlNo=" + Product_SlNo);
                    if (deletedRow.Length > 0)
                    {
                        foreach (DataRow dr in deletedRow)
                        {
                            dt_ProductTax.Rows.Remove(dr);
                        }
                        dt_ProductTax.AcceptChanges();
                    }
                    HttpContext.Current.Session["Product_TaxRecord"] = dt_ProductTax;

                    #endregion

                    #region Delete Tax & Charges

                    DataTable taxDetails = (DataTable)HttpContext.Current.Session["Product_TaxRecord"];
                    if (taxDetails != null && taxDetails.Rows.Count > 0)
                    {
                        foreach (DataRow dr in taxDetails.Rows)
                        {
                            dr["Amount"] = "0.00";
                        }
                        dt_ProductTax.AcceptChanges();
                    }
                    HttpContext.Current.Session["Product_TaxRecord"] = taxDetails;

                    #endregion

                    DeleteWarehouse(Product_SlNo);
                }
            }
            catch
            {
                output = "201";

            }
            return output;
        }

        public bool IsBarcodeGeneratete()
        {
            bool IsGeneratete = false;

            try
            {
                //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();


                DataTable DT_TC = objEngine.GetDataTable("tbl_Master_SystemControl", " BarcodeGeneration ", null);
                if (DT_TC != null && DT_TC.Rows.Count > 0)
                {
                    IsGeneratete = Convert.ToBoolean(DT_TC.Rows[0]["BarcodeGeneration"]);
                }

                return IsGeneratete;
            }
            catch
            {
                return IsGeneratete;
            }
        }

        #endregion

        #region Predictive Product

        protected void cmbProductList_ItemsRequestedByFilterCondition(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            if (e.Filter != "")
            {
                ASPxComboBox comboBox = (ASPxComboBox)source;
                ProductDataSource.SelectCommand = @" Select * From
                                                    (Select Product_ID,Product_Code,Product_Name,Product_Desc,Purchase_UOMID,Purchase_UOM,Purchase_Price,IsPackingActive,Packing_Factor,Packing_UOM,
                                                    Warehousetype,IsComponent,ComponentProduct,HSNSAC,IsInventory,ClassCode,BrandName,IsInstall,row_number() Over(Order By Product_Name Asc) as [rn]
                                                    From v_ProductDetails 
                                                    Where (Product_Code + ' ' + Product_Name) LIKE @filter) as st where st.[rn] between @startIndex and @endIndex";

                ProductDataSource.SelectParameters.Clear();
                ProductDataSource.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
                ProductDataSource.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
                ProductDataSource.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
                comboBox.DataSource = ProductDataSource;
                comboBox.DataBind();
            }
        }
        protected void cmbProductList_ItemRequestedByValue(object source, ListEditItemRequestedByValueEventArgs e)
        {
            long value = 0;
            if (e.Value == null || !Int64.TryParse(e.Value.ToString(), out value))
                return;

            ASPxComboBox comboBox = (ASPxComboBox)source;
            ProductDataSource.SelectCommand = @"Select Product_ID,Product_Code,Product_Name,Product_Desc,Purchase_UOMID,Purchase_UOM,Purchase_Price,IsPackingActive,Packing_Factor,Packing_UOM,
                                                Warehousetype,IsComponent,ComponentProduct,HSNSAC,IsInventory,ClassCode,BrandName,IsInstall From v_ProductDetails Where (Product_ID = @ID) ";

            ProductDataSource.SelectParameters.Clear();
            ProductDataSource.SelectParameters.Add("ID", TypeCode.String, e.Value.ToString());
            comboBox.DataSource = ProductDataSource;
            comboBox.DataBind();
        }

        #endregion

        #region Product Batch Grid

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
        protected void grid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "SrlNo")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "RowNo")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "DocNumber")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "ProductName")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "ProductDiscription")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "PurchaseUOM")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "TaxAmount")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "NetAmount")
            {
                e.Editor.Enabled = true;
            }
            //Mantis Issue 24429
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
            //End of Mantis Issue 24429
            else
            {
                e.Editor.ReadOnly = false;
            }
        }
        protected void grid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data)
                return;

            string IsLinkedProduct = Convert.ToString(e.GetValue("IsLinkedProduct"));
            if (IsLinkedProduct == "Y")
                e.Row.ForeColor = Color.Blue;
        }
        protected void grid_DataBinding(object sender, EventArgs e)
        {
            if (Session["TransactionList"] != null)
            {
                DataTable Transactiondt = (DataTable)Session["TransactionList"];
                grid.DataSource = GetPurchaseChallanGrid(Transactiondt);
            }
        }

        protected void MultiUOM_DataBinding(object sender, EventArgs e)
        {
            //DataTable dt = (DataTable)Session["MultiUOMDataGRN"];
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
            // Rev Mantis Issue 24429
            grid_MultiUOM.JSProperties["cpSetBaseQtyRateInGrid"] = "";
            // End of Rev Mantis Issue 24429
             grid_MultiUOM.JSProperties["cpOpenFocus"] = "";


            if (SpltCmmd == "MultiUOMDisPlay")
            {

                DataTable MultiUOMData = new DataTable();

                if (Session["MultiUOMDataGRN"] != null)
                {
                    MultiUOMData = (DataTable)Session["MultiUOMDataGRN"];
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
                    MultiUOMData.Columns.Add("DetailsId", typeof(string));
                    // Mantis Issue 24429
                    MultiUOMData.Columns.Add("BaseRate", typeof(Decimal));
                    MultiUOMData.Columns.Add("AltRate", typeof(Decimal));
                    MultiUOMData.Columns.Add("UpdateRow", typeof(string));
                    //MultiUOMData.Columns.Add("MultiUOM_SrlNo", typeof(string));
                    // End of Mantis Issue 24429

                }
                if (MultiUOMData != null && MultiUOMData.Rows.Count > 0)
                {

                    string SrlNo = e.Parameters.Split('~')[1];

                    string DetailsId = e.Parameters.Split('~')[2];
                    //Mantis Issue 24429
                    string MultiUOM_SrlNo = "0";
                    //End of Mantis Issue 24429


                    DataView dvData = new DataView(MultiUOMData);
                    //dvData.RowFilter = "Product_SrlNo = '" + SerialID + "'";
                    //Mantis Issue 24429
                    //Rev Bapi
                    //if (DetailsId != "" && DetailsId != null && DetailsId != "null")
                    //{
                    //    //dvData.RowFilter = "SrlNo = '" + SrlNo + "' and Doc_DetailsId='" + DetailsId + "'";
                    //    dvData.RowFilter = "DetailsId='" + DetailsId + "'";
                    //}
                    //else
                    //{
                    //    dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                    //}
                    dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                    //End Rev Bapi
                   // dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                    //End of Mantis Issue 24429
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
                string DetailsId = Convert.ToString(e.Parameters.Split('~')[9]);
                //string DetailsId = Convert.ToString(0);
                // Mantis Issue 24429 
                string BaseRate = Convert.ToString(e.Parameters.Split('~')[10]);
                string AltRate = Convert.ToString(e.Parameters.Split('~')[11]);
                string UpdateRow = Convert.ToString(e.Parameters.Split('~')[12]);
                // End of Mantis Issue 24429

                DataTable allMultidataDetails = (DataTable)Session["MultiUOMDataGRN"];


                DataRow[] MultiUoMresult;

                if (allMultidataDetails != null && allMultidataDetails.Rows.Count > 0)
                {

                    // Mantis Issue 24429
                    //if (DetailsId != "" && DetailsId != null && DetailsId != "null")
                    //{
                        
                    //    //MultiUoMresult = allMultidataDetails.Select("SrlNo ='" + SrlNo + "' and Doc_DetailsId='" + DetailsId + "'");
                    //    MultiUoMresult = allMultidataDetails.Select("DetailsId='" + DetailsId + "'");
                    //}
                    //else
                    //{
                    //    MultiUoMresult = allMultidataDetails.Select("SrlNo ='" + SrlNo + "'");
                    //}
                    MultiUoMresult = allMultidataDetails.Select("SrlNo ='" + SrlNo + "'");
                    // End of Mantis Issue 24429
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
                        // Mantis Issue 24429 [ if "Update Row" checkbox is checked, then all existing Update Row in the grid will be set to False]
                        if (UpdateRow == "True")
                        {
                            item["UpdateRow"] = "False";
                        }
                        // End of Mantis Issue 24429
                    }
                }

                if (Validcheck != "DuplicateUOM")
                {
                    if (Session["MultiUOMDataGRN"] != null)
                    {

                        MultiUOMSaveData = (DataTable)Session["MultiUOMDataGRN"];

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
                        MultiUOMSaveData.Columns.Add("DetailsId", typeof(string));
                        // Mantis Issue 24429
                        MultiUOMSaveData.Columns.Add("BaseRate", typeof(Decimal));
                        MultiUOMSaveData.Columns.Add("AltRate", typeof(Decimal));
                        MultiUOMSaveData.Columns.Add("UpdateRow", typeof(string));
                        MultiUOMSaveData.Columns.Add("MultiUOMSR", typeof(string));
                        // End of Mantis Issue 24429
                    }
                    //Mantis Issue 24429
                    //if (DetailsId != "" && DetailsId != null && DetailsId != "null")
                    //{
                    //    //Mantis Issue 24429
                    //    //MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, DetailsId);
                    //    MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, DetailsId, BaseRate, AltRate, UpdateRow);
                    //    //End of Mantis Issue 24429
                    //}
                    //else
                    //{
                    //    //Mantis Issue 24429
                    //    int i = 0;
                    //    i = MultiUOMSaveData.Rows.Count;
                    //    //foreach (DataRow row in MultiUOMSaveData.Rows)
                    //    //{
                    //    //    i = MultiUOMSaveData.Rows.Count;
                    //    //}
                    //    //Mantis Issue 24429
                    //    //string DetailsId = e.Parameters.Split('~')[2];
                    //    DetailsId = Convert.ToString(i+1);
                    //    //string MultiUOM_SrlNo = Convert.ToString(i + 1);
                    //    //Details_Id + i; 
                    //    //End of Mantis Issue 24429
                    //    //MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, DetailsId);
                    //    MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, DetailsId, BaseRate, AltRate, UpdateRow);
                    //    //End of Mantis Issue 24429
                    //}
                    DataRow thisRow;
                    if (MultiUOMSaveData.Rows.Count > 0)
                    {

                        MultiUOMSR = Convert.ToInt32(MultiUOMSaveData.Compute("max([MultiUOMSR])", string.Empty)) + 1;
                        MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, DetailsId, BaseRate, AltRate, UpdateRow, MultiUOMSR);
                       
                    }
                    else
                    {
                        MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, DetailsId, BaseRate, AltRate, UpdateRow, MultiUOMSR);
                    }
                    //End of Mantis Issue 24429
                    MultiUOMSaveData.AcceptChanges();
                    Session["MultiUOMDataGRN"] = MultiUOMSaveData;

                    if (MultiUOMSaveData != null && MultiUOMSaveData.Rows.Count > 0)
                    {
                        DataView dvData = new DataView(MultiUOMSaveData);
                        //Mantis Issue 24429
                        //if (DetailsId != "" && DetailsId != null && DetailsId != "null")
                        //{
                        //    //dvData.RowFilter = "SrlNo = '" + SrlNo + "' and Doc_DetailsId='" + DetailsId + "'";
                        //    dvData.RowFilter = "DetailsId='" + DetailsId + "'";
                        //}
                        //else
                        //{
                        //    dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                        //}
                        dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                        //End of Mantis Issue 24429
                        grid_MultiUOM.DataSource = dvData;
                        grid_MultiUOM.DataBind();
                    }
                    else
                    {
                        //MultiUOMSaveData.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId);
                        //Session["MultiUOMDataGRN"] = MultiUOMSaveData;
                        grid_MultiUOM.DataSource = MultiUOMSaveData.DefaultView;
                        grid_MultiUOM.DataBind();
                    }
                }
            }
            //Mantis Issue 24429
            else if (SpltCmmd == "EditData")
            {
                string AltUOMKeyValuewithqnty = e.Parameters.Split('~')[1];
                string AltUOMKeyValue = e.Parameters.Split('~')[2];
                string AltUOMKeyqnty = AltUOMKeyValuewithqnty.Split('|')[1];

                string SrlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                DataTable dt = (DataTable)Session["MultiUOMDataGRN"];

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
                Session["MultiUOMDataGRN"] = dt;
            }



            else if (SpltCmmd == "UpdateRow")
            {

                string Validcheck = "";
                string SrlNoR = e.Parameters.Split('~')[1];
                string AltUOMKeyValue = e.Parameters.Split('~')[7];
                string AltUOMKeyqnty = e.Parameters.Split('~')[5];
                string muid = e.Parameters.Split('~')[13];
                string SrlNo = "0";
               
                DataTable MultiUOMSaveData = new DataTable();

                DataTable dt = (DataTable)Session["MultiUOMDataGRN"];

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] MultiUoMresult = dt.Select("MultiUOMSR ='" + muid + "'");
                    foreach (DataRow item in MultiUoMresult)
                    {
                        SrlNo = Convert.ToString(item["SrlNo"]);
                    }
                }


                //string SrlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                string Quantity = Convert.ToString(e.Parameters.Split('~')[2]);
                string UOM = Convert.ToString(e.Parameters.Split('~')[3]);
                string AltUOM = Convert.ToString(e.Parameters.Split('~')[4]);
                string AltQuantity = Convert.ToString(e.Parameters.Split('~')[5]);
                string UomId = Convert.ToString(e.Parameters.Split('~')[6]);
                string AltUomId = Convert.ToString(e.Parameters.Split('~')[7]);
                string ProductId = Convert.ToString(e.Parameters.Split('~')[8]);
                string DetailsId = Convert.ToString(e.Parameters.Split('~')[9]);

                // Mantis Issue 24428
                string BaseRate = Convert.ToString(e.Parameters.Split('~')[10]);
                string AltRate = Convert.ToString(e.Parameters.Split('~')[11]);
                string UpdateRow = Convert.ToString(e.Parameters.Split('~')[12]);
                // End of Mantis Issue 24428
                //Rev Mantis Issue 24429
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
                            SrlNo = Convert.ToString(item["SrlNo"]);
                            item.Table.Rows.Remove(item);
                            break;

                        }
                    }

                    dt.Rows.Add(SrlNo, Quantity, UOM, AltUOM, AltQuantity, UomId, AltUomId, ProductId, DetailsId, BaseRate, AltRate, UpdateRow, muid);
                }
               
                Session["MultiUOMDataGRN"] = dt;
                MultiUOMSaveData = (DataTable)Session["MultiUOMDataGRN"];

                MultiUOMSaveData.AcceptChanges();
                Session["MultiUOMDataGRN"] = MultiUOMSaveData;

                if (MultiUOMSaveData != null && MultiUOMSaveData.Rows.Count > 0)
                {
                    DataView dvData = new DataView(MultiUOMSaveData);
                    // dvData.RowFilter = "SrlNo = '" + SrlNo + "'";

                   dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                   
                    grid_MultiUOM.DataSource = dvData;
                    grid_MultiUOM.DataBind();
                }
            }
            //End of Mantis Issue 24429
            else if (SpltCmmd == "MultiUomDelete")
            {
                string AltUOMKeyValuewithqnty = e.Parameters.Split('~')[1];
                string AltUOMKeyValue = AltUOMKeyValuewithqnty.Split('|')[0];
                string AltUOMKeyqnty = AltUOMKeyValuewithqnty.Split('|')[1];

                string SrlNo = Convert.ToString(e.Parameters.Split('~')[2]);
                string DetailsId = Convert.ToString(e.Parameters.Split('~')[3]);

                DataRow[] MultiUoMresult;
                DataTable dt = (DataTable)Session["MultiUOMDataGRN"];

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (DetailsId != "" && DetailsId != null && DetailsId != "null")
                    {
                        //MultiUoMresult = dt.Select("SrlNo ='" + SrlNo + "' and Doc_DetailsId='" + DetailsId + "'");
                        MultiUoMresult = dt.Select("DetailsId='" + DetailsId + "'");
                    }
                    else
                    {
                        MultiUoMresult = dt.Select("SrlNo ='" + SrlNo + "'");
                    }

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
                Session["MultiUOMDataGRN"] = dt;
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataView dvData = new DataView(dt);
                    if (DetailsId != "" && DetailsId != null && DetailsId != "null")
                    {
                        //dvData.RowFilter = "SrlNo = '" + SrlNo + "'and Doc_DetailsId='" + DetailsId + "'";
                        dvData.RowFilter = "DetailsId='" + DetailsId + "'";
                    }
                    else
                    {
                        dvData.RowFilter = "SrlNo = '" + SrlNo + "'";
                    }
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
                DataTable dt = (DataTable)Session["MultiUOMDataGRN"];
                string detailsId = Convert.ToString(e.Parameters.Split('~')[2]);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] MultiUoMresult;
                    if (detailsId != null && detailsId != "" && detailsId != "null")
                    {
                        MultiUoMresult = dt.Select("DetailsId ='" + detailsId + "'");
                    }
                    else
                    {
                        MultiUoMresult = dt.Select("SrlNo ='" + SrlNo + "'");
                    }
                    foreach (DataRow item in MultiUoMresult)
                    {
                        item.Table.Rows.Remove(item);
                    }
                }
                Session["MultiUOMDataGRN"] = dt;
            }
            // Mantis Issue 24429
            else if (SpltCmmd == "SetBaseQtyRateInGrid")
            {
                DataTable dt = new DataTable();

                if (Session["MultiUOMDataGRN"] != null)
                {
                    string SrlNo = Convert.ToString(e.Parameters.Split('~')[1]);
                    //Mantis 24428
                    string DetailsID = Convert.ToString(e.Parameters.Split('~')[2]);
                    //End Mantis 24428
                    DataRow[] MultiUoMresult;
                    dt = (DataTable)HttpContext.Current.Session["MultiUOMDataGRN"];
                    //Rev Bapi
                    //if (DetailsID != null && DetailsID != "" && DetailsID != "null")
                    //{
                    //    MultiUoMresult = dt.Select("DetailsId ='" + DetailsID + "'  and UpdateRow ='True'");
                    //}
                    //else
                    //{
                    //     MultiUoMresult = dt.Select("SrlNo ='" + SrlNo + "' and UpdateRow ='True'");
                    //}
                    MultiUoMresult = dt.Select("SrlNo ='" + SrlNo + "' and UpdateRow ='True'");
                  //End Rev Bapi

                    Int64 SelNo = Convert.ToInt64(MultiUoMresult[0]["SrlNo"]);
                    Decimal BaseQty = Convert.ToDecimal(MultiUoMresult[0]["Quantity"]);
                    Decimal BaseRate = Convert.ToDecimal(MultiUoMresult[0]["BaseRate"]);
                    // Mantis Issue 24429
                    Decimal AltQuantity = Convert.ToDecimal(MultiUoMresult[0]["AltQuantity"]);
                    String AltUOM = Convert.ToString(MultiUoMresult[0]["AltUOM"]);
                    // End of Mantis Issue 24429

                    grid_MultiUOM.JSProperties["cpSetBaseQtyRateInGrid"] = "1";
                    grid_MultiUOM.JSProperties["cpBaseQty"] = BaseQty;
                    grid_MultiUOM.JSProperties["cpBaseRate"] = BaseRate;
                    // Mantis Issue 24429
                    grid_MultiUOM.JSProperties["cpAltQuantity"] = AltQuantity;
                    grid_MultiUOM.JSProperties["cpAltUOM"] = AltUOM;
                    // End of Mantis Issue 24429


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
            // End of Mantis Issue 24429

        }



        protected void grid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            DataTable Transactiondt = (DataTable)Session["TransactionList"];
            string IsDeleteFrom = Convert.ToString(hdfIsDelete.Value);
            string validate = "";

            foreach (var args in e.InsertValues)
            {
                string Product_ID = Convert.ToString(args.NewValues["ProductID"]);
                if (Product_ID != "" && Product_ID != "0")
                {
                    string SrlNo = Convert.ToString(args.NewValues["SrlNo"]);
                    string RowNo = Convert.ToString(args.NewValues["RowNo"]);
                    string DocNumber = Convert.ToString(args.NewValues["DocNumber"]);
                    string DocID = Convert.ToString(args.NewValues["DocID"]);
                    string ProductID = Convert.ToString(args.NewValues["ProductID"]);
                    string ProductName = Convert.ToString(args.NewValues["ProductName"]);
                    string ProductDiscription = Convert.ToString(args.NewValues["ProductDiscription"]);
                    string Quantity = Convert.ToString(args.NewValues["Quantity"]);
                    string PurchaseUOM = Convert.ToString(args.NewValues["PurchaseUOM"]);
                    string PurchasePrice = Convert.ToString(args.NewValues["PurchasePrice"]);
                    string Discount = Convert.ToString(args.NewValues["Discount"]);
                    string TotalAmount = Convert.ToString(args.NewValues["TotalAmount"]);
                    string TaxAmount = Convert.ToString(args.NewValues["TaxAmount"]);
                    string NetAmount = Convert.ToString(args.NewValues["NetAmount"]);
                    string TotalQty = Convert.ToString(args.NewValues["TotalQty"]);
                    string BalanceQty = Convert.ToString(args.NewValues["BalanceQty"]);
                    string IsComponentProduct = Convert.ToString(args.NewValues["IsComponentProduct"]);
                    string IsLinkedProduct = Convert.ToString(args.NewValues["IsLinkedProduct"]);
                    string ComponentProduct = Convert.ToString(args.NewValues["ComponentProduct"]);
                    // Rev Mantis Issue 24061
                    string Balance_Amount = Convert.ToString(args.NewValues["Balance_Amount"]);
                    // End of Rev Mantis Issue 24061
                    string DetailsId = (Convert.ToString(args.NewValues["DetailsId"]) != "") ? Convert.ToString(args.NewValues["DetailsId"]) : "0";
                    // Mantis Issue 24429
                    string Order_AltQuantity = Convert.ToString(args.NewValues["Order_AltQuantity"]);
                    string Order_AltUOM = Convert.ToString(args.NewValues["Order_AltUOM"]);
                    // End of Mantis Issue 24429
                    // Rev Mantis Issue 24061 [Balance_Amount added in Rows.Add]
                    string ChallanDetails_InlineRemarks = (Convert.ToString(args.NewValues["ChallanDetails_InlineRemarks"]) != "") ? Convert.ToString(args.NewValues["ChallanDetails_InlineRemarks"]) : "";
                    //Mantis Issue 24429
                    //Transactiondt.Rows.Add(SrlNo, RowNo, DocNumber, DocID, ProductID, ProductName, ProductDiscription, Quantity, PurchaseUOM, PurchasePrice,
                    //    Discount, TotalAmount, TaxAmount, NetAmount, TotalQty, BalanceQty, IsComponentProduct, IsLinkedProduct, ComponentProduct, "I", DetailsId, ChallanDetails_InlineRemarks,
                    //    Balance_Amount);

                    string ChallanDetails_Id = (Convert.ToString(args.NewValues["ChallanDetails_Id"]) != "") ? Convert.ToString(args.NewValues["ChallanDetails_Id"]) : "0";

                    Transactiondt.Rows.Add(SrlNo, RowNo, DocNumber, DocID, ProductID, ProductName, ProductDiscription, Quantity, PurchaseUOM, PurchasePrice,
                        Discount, TotalAmount, TaxAmount, NetAmount, TotalQty, BalanceQty, IsComponentProduct, IsLinkedProduct, ComponentProduct, "I", DetailsId, ChallanDetails_InlineRemarks,
                        Balance_Amount, Order_AltQuantity, Order_AltUOM, ChallanDetails_Id);
                    //End of Mantis Issue 24429
                    if (IsComponentProduct == "Y")
                    {
                        string[] ProductDetailsList = ProductID.Split(new string[] { "||@||" }, StringSplitOptions.None);
                        string sProductID = ProductDetailsList[0];

                        DataTable ComponentTable = objSalesInvoiceBL.GetLinkedProductList("ComponentProduct", sProductID);
                        foreach (DataRow Comp_dr in ComponentTable.Rows)
                        {
                            string cProductID = Convert.ToString(Comp_dr["ProductID"]);
                            string cProductName = Convert.ToString(Comp_dr["ProductName"]);
                            string cProductDiscription = Convert.ToString(Comp_dr["ProductDiscription"]);
                            string cPurchaseUOM = Convert.ToString(Comp_dr["PurchaseUOM"]);
                            string cPurchasePrice = Convert.ToString(Comp_dr["PurchasePrice"]);

                            SrlNo = Convert.ToString(Convert.ToInt32(SrlNo) + 1);

                            Transactiondt.Rows.Add(SrlNo, SrlNo, DocNumber, DocID,
                                cProductID, cProductName, cProductDiscription, Quantity, cPurchaseUOM, cPurchasePrice,
                                "0.00", "0.00", "0.00", "0.00", "0", "0", "", "Y", ProductID, "I", DetailsId, ChallanDetails_InlineRemarks,"0.00","0.00","0" ,ChallanDetails_Id);
                        }
                    }
                }
            }

            foreach (var args in e.UpdateValues)
            {
                string SrlNo = Convert.ToString(args.Keys["SrlNo"]);
                string Product_ID = Convert.ToString(args.NewValues["ProductID"]);

                bool isDeleted = false;
                foreach (var arg in e.DeleteValues)
                {
                    string Del_SrlNo = Convert.ToString(args.Keys["SrlNo"]);
                    if (SrlNo == Del_SrlNo)
                    {
                        isDeleted = true;
                        break;
                    }
                }

                if (isDeleted == false)
                {
                    if (Product_ID != "" && Product_ID != "0")
                    {
                        string RowNo = Convert.ToString(args.NewValues["RowNo"]);
                        string DocNumber = Convert.ToString(args.NewValues["DocNumber"]);
                        string DocID = Convert.ToString(args.NewValues["DocID"]);
                        string ProductID = Convert.ToString(args.NewValues["ProductID"]);
                        string ProductName = Convert.ToString(args.NewValues["ProductName"]);
                        string ProductDiscription = Convert.ToString(args.NewValues["ProductDiscription"]);
                        string Quantity = Convert.ToString(args.NewValues["Quantity"]);
                        string PurchaseUOM = Convert.ToString(args.NewValues["PurchaseUOM"]);
                        string PurchasePrice = Convert.ToString(args.NewValues["PurchasePrice"]);
                        string Discount = Convert.ToString(args.NewValues["Discount"]);
                        string TotalAmount = Convert.ToString(args.NewValues["TotalAmount"]);
                        string TaxAmount = Convert.ToString(args.NewValues["TaxAmount"]);
                        string NetAmount = Convert.ToString(args.NewValues["NetAmount"]);
                        string TotalQty = Convert.ToString(args.NewValues["TotalQty"]);
                        string BalanceQty = Convert.ToString(args.NewValues["BalanceQty"]);
                        string IsComponentProduct = Convert.ToString(args.NewValues["IsComponentProduct"]);
                        string IsLinkedProduct = Convert.ToString(args.NewValues["IsLinkedProduct"]);
                        string ComponentProduct = Convert.ToString(args.NewValues["ComponentProduct"]);
                        string DetailsId = (Convert.ToString(args.NewValues["DetailsId"]) != "") ? Convert.ToString(args.NewValues["DetailsId"]) : "0";
                        string ChallanDetails_InlineRemarks = Convert.ToString(args.NewValues["ChallanDetails_InlineRemarks"]);
                        // Rev Mantis Issue 24061
                        string Balance_Amount = Convert.ToString(args.NewValues["Balance_Amount"]);
                        // End of Rev Mantis Issue 24061
                        // Mantis Issue 24429
                        string Order_AltQuantity = Convert.ToString(args.NewValues["Order_AltQuantity"]);
                        string Order_AltUOM = Convert.ToString(args.NewValues["Order_AltUOM"]);
                        // End of Mantis Issue 24429

                        string ChallanDetails_Id = (Convert.ToString(args.NewValues["ChallanDetails_Id"]) != "") ? Convert.ToString(args.NewValues["ChallanDetails_Id"]) : "0";
                        bool Isexists = false;
                        foreach (DataRow Update_dr in Transactiondt.Rows)
                        {
                            string Old_SrlNo = Convert.ToString(Update_dr["SrlNo"]);

                            if (SrlNo == Old_SrlNo)
                            {
                                Isexists = true;

                                Update_dr["DocNumber"] = DocNumber;
                                Update_dr["DocID"] = DocID;
                                Update_dr["ProductID"] = ProductID;
                                Update_dr["ProductName"] = ProductName;
                                Update_dr["ProductDiscription"] = ProductDiscription;
                                Update_dr["Quantity"] = Quantity;
                                Update_dr["PurchaseUOM"] = PurchaseUOM;
                                Update_dr["PurchasePrice"] = PurchasePrice;
                                Update_dr["PurchasePrice"] = PurchasePrice;
                                Update_dr["Discount"] = Discount;
                                Update_dr["TotalAmount"] = TotalAmount;
                                Update_dr["TaxAmount"] = TaxAmount;
                                Update_dr["NetAmount"] = NetAmount;
                                Update_dr["TotalQty"] = TotalQty;
                                Update_dr["BalanceQty"] = BalanceQty;
                                Update_dr["IsComponentProduct"] = IsComponentProduct;
                                Update_dr["IsLinkedProduct"] = IsLinkedProduct;
                                Update_dr["ComponentProduct"] = ComponentProduct;
                                Update_dr["Status"] = "U";
                                Update_dr["DetailsId"] = DetailsId;
                                Update_dr["ChallanDetails_InlineRemarks"] = ChallanDetails_InlineRemarks;
                                // Rev Mantis Issue 24061
                                Update_dr["Balance_Amount"] = Balance_Amount;
                                // End of Rev Mantis Issue 24061
                                // Mantis Issue 24429
                                Update_dr["Order_AltQuantity"] = Order_AltQuantity;
                                Update_dr["Order_AltUOM"] = Order_AltUOM;
                                // End of Mantis Issue 24429
                                Update_dr["ChallanDetails_Id"] = ChallanDetails_Id;

                                break;
                            }
                        }

                        if (Isexists == false)
                        {
                            // Rev Mantis Issue 24061 [ Balance_amount added in Rows.Add ]
                            //Mantis Issue 24429
                            //Transactiondt.Rows.Add(SrlNo, RowNo, DocNumber, DocID, ProductID, ProductName, ProductDiscription, Quantity, PurchaseUOM, PurchasePrice,
                            //    Discount, TotalAmount, TaxAmount, NetAmount, TotalQty, BalanceQty, IsComponentProduct, IsLinkedProduct, ComponentProduct, "U", DetailsId, 
                            //    ChallanDetails_InlineRemarks, Balance_Amount);
                            Transactiondt.Rows.Add(SrlNo, RowNo, DocNumber, DocID, ProductID, ProductName, ProductDiscription, Quantity, PurchaseUOM, PurchasePrice,
                                Discount, TotalAmount, TaxAmount, NetAmount, TotalQty, BalanceQty, IsComponentProduct, IsLinkedProduct, ComponentProduct, "U", DetailsId,
                                ChallanDetails_InlineRemarks, Balance_Amount, Order_AltQuantity, Order_AltUOM,ChallanDetails_Id);
                            //End of Mantis Issue 24429
                        }

                        if (IsComponentProduct == "Y")
                        {
                            string[] ProductDetailsList = ProductID.Split(new string[] { "||@||" }, StringSplitOptions.None);
                            string sProductID = ProductDetailsList[0];

                            DataTable ComponentTable = objSalesInvoiceBL.GetLinkedProductList("ComponentProduct", sProductID);
                            foreach (DataRow Comp_dr in ComponentTable.Rows)
                            {
                                string cProductID = Convert.ToString(Comp_dr["ProductID"]);
                                string cProductName = Convert.ToString(Comp_dr["ProductName"]);
                                string cProductDiscription = Convert.ToString(Comp_dr["ProductDiscription"]);
                                string cPurchaseUOM = Convert.ToString(Comp_dr["PurchaseUOM"]);
                                string cPurchasePrice = Convert.ToString(Comp_dr["PurchasePrice"]);

                                SrlNo = Convert.ToString(Convert.ToInt32(SrlNo) + 1);

                                Transactiondt.Rows.Add(SrlNo, SrlNo, DocNumber, DocID,
                                    cProductID, cProductName, cProductDiscription, Quantity, cPurchaseUOM, cPurchasePrice,
                                    "0.00", "0.00", "0.00", "0.00", "0", "0", "", "Y", ProductID, "U", DetailsId, ChallanDetails_InlineRemarks, "0.00", "0.00", "0", ChallanDetails_Id);
                            }
                        }
                    }
                }
            }

            foreach (var args in e.DeleteValues)
            {
                string SrlNo = Convert.ToString(args.Keys["SrlNo"]);
                bool IsDelete = true;

                if (Session["Product_StockList"] != null)
                {
                    DataTable Warehousedt = (DataTable)Session["Product_StockList"];

                    var CheckStockOut_dt = Warehousedt.Select("Product_SrlNo ='" + SrlNo + "' AND IsOutStatusMsg='visibility: visible'");
                    if (CheckStockOut_dt.Length > 0)
                    {
                        IsDelete = false;
                        grid.JSProperties["cpSaveSuccessOrFail"] = "stockOut";
                    }
                }

                if (IsDelete == true)
                {
                    foreach (DataRow Update_dr in Transactiondt.Rows)
                    {
                        string Old_SrlNo = Convert.ToString(Update_dr["SrlNo"]);

                        if (SrlNo == Old_SrlNo)
                        {
                            Update_dr["Status"] = "D";
                            break;
                        }
                    }
                }
            }

            int j = 1;
            foreach (DataRow dr in Transactiondt.Rows)
            {
                string Status = Convert.ToString(dr["Status"]);
                string oldRowNo = Convert.ToString(dr["RowNo"]);
                string newRowNo = j.ToString();

                if (Status != "D")
                {
                    dr["RowNo"] = j.ToString();
                    j++;
                }
                else
                {
                    dr["RowNo"] = "0";
                }
            }
            Transactiondt.AcceptChanges();

            Session["TransactionList"] = Transactiondt;
            if (IsDeleteFrom != "D" && IsDeleteFrom != "C")
            {
                DataTable _tempTransactiondt = Transactiondt.Copy();
                foreach (DataRow dr in _tempTransactiondt.Rows)
                {
                    string IDs = Convert.ToString(dr["ProductID"]);

                    string[] list = Convert.ToString(dr["ProductID"]).Split(new string[] { "||@||" }, StringSplitOptions.None);
                    dr["ProductID"] = Convert.ToString(list[0]);
                }
                _tempTransactiondt.AcceptChanges();

                DataRow[] _deletedRow = _tempTransactiondt.Select("ProductID=''");
                if (_deletedRow.Length > 0)
                {
                    foreach (DataRow dr in _deletedRow)
                    {
                        _tempTransactiondt.Rows.Remove(dr);
                    }
                    _tempTransactiondt.AcceptChanges();
                }

                string strPartyDate = "";
                string IndentRequisitionDate = "";
                string strSchemeID = string.Empty;
                string strSchemeType = string.Empty;

                strSchemeID = Convert.ToString(ddl_numberingScheme.SelectedValue);
                if (!string.IsNullOrEmpty(strSchemeID))
                {
                    strSchemeType = strSchemeID.Split('~')[0];
                }

                string ActionType = Convert.ToString(hdnPageStatus.Value);
                string PurchaseOrder_Id = Convert.ToString(Session["PurchaseChallan_Id"]);
                string strIsInventory = Convert.ToString(ddlInventory.SelectedValue);
                string strPurchaseNumber = Convert.ToString(txtVoucherNo.Text);
                string strPurchaseDate = Convert.ToString(dt_PLQuote.Date);
                string strBranch = Convert.ToString(ddl_Branch.SelectedValue);
                string strVendor = Convert.ToString(hdnCustomerId.Value);

                string strContactName = Convert.ToString(cmbContactPerson.Value);
                string Reference = Convert.ToString(txt_Refference.Text);
                string PosGst = Convert.ToString(ddlPosGstChallan.Value);
                string strPartyInvoice = Convert.ToString(txtPartyInvoice.Text);
                if (dt_PartyDate.Date.ToString("yyyy-MM-dd") != "0001-01-01") strPartyDate = dt_PartyDate.Date.ToString("yyyy-MM-dd");

                string strCurrency = Convert.ToString(ddl_Currency.SelectedValue);
                string strRate = (Convert.ToString(txt_Rate.Value).Trim() != "") ? Convert.ToString(txt_Rate.Value) : "0";
                string strTaxOption = Convert.ToString(ddl_AmountAre.Value);
                string strTaxCode = "0";//Convert.ToString(ddl_VatGstCst.Value).Split('~')[0];

                string CompanyID = Convert.ToString(Session["LastCompany"]);
                string FinYear = Convert.ToString(Session["LastFinYear"]);
                string Currency = Convert.ToString(Session["LocalCurrency"]);
                string[] ActCurrency = Currency.Split('~');
                int BaseCurrencyId = Convert.ToInt32(ActCurrency[0]);

                string strForBranch = "0";
                if (hdnForBranchTaggingPurchase.Value == "1")
                {
                    strForBranch = Convert.ToString(ddlForBranch.SelectedItem.Value);
                }

                String IndentRequisitionNo = "";
                for (int i = 0; i < taggingGrid.GetSelectedFieldValues("PurchaseOrder_Id").Count; i++)
                {
                    IndentRequisitionNo += "," + Convert.ToString(taggingGrid.GetSelectedFieldValues("PurchaseOrder_Id")[i]);
                }
                IndentRequisitionNo = IndentRequisitionNo.TrimStart(',');

                if (taggingGrid.GetSelectedFieldValues("PurchaseOrder_Id").Count == 1)
                {
                    IndentRequisitionDate = Convert.ToString(dt_Quotation.Text);
                }

                #region Product Stock Generate

                string StockDetails = Convert.ToString(hdnJsonProductStock.Value);
                JavaScriptSerializer _ser = new JavaScriptSerializer();
                _ser.MaxJsonLength = Int32.MaxValue;
                List<ProductStockDetailsPC> StockEntryJson = _ser.Deserialize<List<ProductStockDetailsPC>>(StockDetails);
                DataTable _Stockdt = ToDataTable(StockEntryJson);

                #endregion

                #region DataTable of Warehouse

                DataTable tempWarehousedt = new DataTable();
                //if (Session["Product_StockList"] != null)
                if (_Stockdt != null)
                {
                    //DataTable Warehousedt = (DataTable)Session["Product_StockList"];
                    DataTable Warehousedt = _Stockdt.Copy();
                    tempWarehousedt = Warehousedt.DefaultView.ToTable(false, "Product_SrlNo", "LoopID", "SrlNo", "WarehouseID", "Quantity", "Batch", "SerialNo", "Barcode", "MfgDate", "ExpiryDate", "AltQty","AltUOM");
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
                    tempWarehousedt.Columns.Add("Barcode", typeof(string));
                    tempWarehousedt.Columns.Add("MfgDate", typeof(string));
                    tempWarehousedt.Columns.Add("ExpiryDate", typeof(string));
                    tempWarehousedt.Columns.Add("AltQty", typeof(string));
                    tempWarehousedt.Columns.Add("AltUOM", typeof(string));
                }

                foreach (DataRow wtrow in tempWarehousedt.Rows)
                {
                    string strMfgDate = Convert.ToString(wtrow["MfgDate"]).Trim();
                    string strExpiryDate = Convert.ToString(wtrow["ExpiryDate"]).Trim();

                    if (strMfgDate != "")
                    {
                        string DD = strMfgDate.Substring(0, 2);
                        string MM = strMfgDate.Substring(3, 2);
                        string YYYY = strMfgDate.Substring(6, 4);
                        string Date = YYYY + '-' + MM + '-' + DD;

                        wtrow["MfgDate"] = Date;
                    }

                    if (strExpiryDate != "")
                    {
                        string DD = strExpiryDate.Substring(0, 2);
                        string MM = strExpiryDate.Substring(3, 2);
                        string YYYY = strExpiryDate.Substring(6, 4);
                        string Date = YYYY + '-' + MM + '-' + DD;

                        wtrow["ExpiryDate"] = Date;
                    }
                }

                #endregion

                #region DataTable of Product Tax

                DataTable TaxDetailTable = new DataTable();
                if (Session["Product_TaxRecord"] != null)
                {
                    TaxDetailTable = (DataTable)Session["Product_TaxRecord"];
                }

                #endregion

                //datatable for MultiUOm start chinmoy 14-01-2020
                DataTable MultiUOMDetails = new DataTable();

                if (Session["MultiUOMDataGRN"] != null)
                {
                    // Mantis Issue 24429
                    DataTable MultiUOM = (DataTable)Session["MultiUOMDataGRN"];
                    //MultiUOMDetails = MultiUOM.DefaultView.ToTable(false, "SrlNo", "Quantity", "UOM", "AltUOM", "AltQuantity", "UomId", "AltUomId", "ProductId", "DetailsId");
                    MultiUOMDetails = MultiUOM.DefaultView.ToTable(false, "SrlNo", "Quantity", "UOM", "AltUOM", "AltQuantity", "UomId", "AltUomId", "ProductId", "DetailsId", "BaseRate", "AltRate", "UpdateRow");
                    //End of Mantis Issue 24429
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
                    MultiUOMDetails.Columns.Add("DetailsId", typeof(string));
                    // Mantis Issue 24429
                    MultiUOMDetails.Columns.Add("BaseRate", typeof(Decimal));
                    MultiUOMDetails.Columns.Add("AltRate", typeof(Decimal));
                    MultiUOMDetails.Columns.Add("UpdateRow", typeof(string));
                    // End of Mantis Issue 24429

                }


                //End



                #region DataTable Of Tax & Charges Details

                DataTable TaxDetailsdt = new DataTable();
                if (Session["TaxChargeRecord"] != null)
                {
                    TaxDetailsdt = (DataTable)Session["TaxChargeRecord"];
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
                tempTaxDetailsdt.Columns["SlNo"].SetOrdinal(0);
                tempTaxDetailsdt.Columns["Taxes_ID"].SetOrdinal(1);
                tempTaxDetailsdt.Columns["AltTax_Code"].SetOrdinal(2);
                tempTaxDetailsdt.Columns["Percentage"].SetOrdinal(3);
                tempTaxDetailsdt.Columns["Amount"].SetOrdinal(4);

                foreach (DataRow d in tempTaxDetailsdt.Rows)
                {
                    d["SlNo"] = "0";
                }

                #endregion

                #region Datatable of BillingShipping

                DataTable tempBillAddress = new DataTable();
                tempBillAddress = Purchase_BillingShipping.GetBillingShippingTable();

                #endregion

                #region Section For Approval Section

                string approveStatus = "";
                if (Request.QueryString["status"] != null)
                {
                    approveStatus = Convert.ToString(Request.QueryString["status"]);
                }

                #endregion

                #region Section For Stock Section

                if (ddlInventory.SelectedValue != "N" && ddlInventory.SelectedValue != "S")
                {
                    foreach (DataRow dr in _tempTransactiondt.Rows)
                    {
                        string ProductID = Convert.ToString(dr["ProductID"]);
                        decimal ProductQuantity = Convert.ToDecimal(dr["Quantity"]);
                        string Status = Convert.ToString(dr["Status"]);

                        if (ProductID != "")
                        {
                            if (CheckProduct_IsInventory(ProductID) == true)
                            {
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
                    }
                }


                if (hdnPageStatus.Value == "EDIT")
                {

                    if (grid_Products.GetSelectedFieldValues("ComponentDetailsID").Count > 0)
                    {
                        if (_tempTransactiondt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in _tempTransactiondt.Rows)
                            {
                              
                                string ProductID = Convert.ToString(dr["ProductID"]);
                                decimal ProductQuantity = Convert.ToDecimal(dr["Quantity"]);
                                string Status = Convert.ToString(dr["Status"]);

                                string Doc_DetailsId = Convert.ToString(dr["DetailsId"]); 

                                DataTable dtq = oDBEngine.GetDataTable("select (ISNULL(TotalQty,0)+isnull(BalanceQty,0)) TotQty from tbl_trans_BalanceMapForPurchaseChallan where  POID='" + Convert.ToInt32(dr["DocID"]) + "' and ProductId='" + ProductID + "'   and PODetailsID='" + Convert.ToInt32(dr["DetailsId"]) + "' ");

                                if (ProductID != "" && dtq.Rows.Count > 0)
                                {
                                    if (ProductQuantity > Convert.ToDecimal(dtq.Rows[0]["TotQty"]))
                                    {
                                        validate = "ExceedQuantity";
                                        break;
                                    }
                                }

                              
                                //if (ProductID != "" && dtq.Rows.Count > 0)
                                //{
                                //    if (ProductQuantity <Convert.ToDecimal(hdnTaggedQuantity.Value))
                                //    {
                                //        validate = "MINExceedQuantity";
                                //        break;
                                //    }
                                //}
                                
                            }

                        }
                    }
                }
                else   if (hdnPageStatus.Value == "ADD")
                {

                    if (grid_Products.GetSelectedFieldValues("ComponentDetailsID").Count > 0)
                    {
                        if (_tempTransactiondt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in _tempTransactiondt.Rows)
                            {

                                string ProductID = Convert.ToString(dr["ProductID"]);
                                decimal ProductQuantity = Convert.ToDecimal(dr["Quantity"]);
                                string Status = Convert.ToString(dr["Status"]);
                                string Doc_DetailsId = Convert.ToString(dr["DetailsId"]);

                                DataTable dtq = oDBEngine.GetDataTable("select isnull(BalanceQty,0) TotQty from tbl_trans_BalanceMapForPurchaseChallan where  POID='" + Convert.ToInt32(dr["DocID"]) + "' and  ProductId='" + ProductID + "' and PODetailsID='" + Convert.ToInt32(dr["DetailsId"]) + "'");

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
                // Rev Mantis Issue 24061
                if (hdnPurchaseOrderItemNegative.Value == "1")
                {
                    //if (InvoiceComponent != "" && InvoiceCreatedFromDoc == "PO")
                    if (grid_Products.GetSelectedFieldValues("ComponentDetailsID").Count > 0)
                    {
                        if (_tempTransactiondt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in _tempTransactiondt.Rows)
                            {
                                decimal Balance_Amount = Convert.ToDecimal(dr["Balance_Amount"]);
                                decimal TotalAmount = Convert.ToDecimal(dr["TotalAmount"]);
                                string Status = Convert.ToString(dr["Status"]);

                                if (Status != "D")
                                {
                                    if (TotalAmount > Balance_Amount)
                                    {
                                        validate = "NetAmountExceed";
                                        break;
                                    }
                                }
                            }
                        }
                        
                    }

                }
                // Enf of Rev Mantis Issue 24061

                if (ddlInventory.SelectedValue != "N" && ddlInventory.SelectedValue != "S")
                {
                    foreach (DataRow dr in _tempTransactiondt.Rows)
                    {
                        string ProductID = Convert.ToString(dr["ProductID"]);
                        string strSrlNo = Convert.ToString(dr["SrlNo"]);
                        decimal strProductQuantity = Convert.ToDecimal(dr["Quantity"]);
                        string Status = Convert.ToString(dr["Status"]);
                        string IsLinkedProduct = Convert.ToString(dr["IsLinkedProduct"]);

                        decimal strWarehouseQuantity = 0;
                        GetQuantityBaseOnProduct(_Stockdt, strSrlNo, ref strWarehouseQuantity);

                        if (ProductID != "")
                        {
                            if (CheckProduct_IsInventory(ProductID) == true)
                            {                                
                                if (Status != "D")
                                {
                                    if (IsLinkedProduct != "Y")
                                    {
                                        if (strProductQuantity != strWarehouseQuantity)
                                        {
                                            string strRowNo = Convert.ToString(dr["RowNo"]);
                                            validate = "checkWarehouse";
                                            grid.JSProperties["cpProductSrlIDCheck"] = strRowNo;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (hddnMultiUOMSelection.Value == "1")
                {
                    foreach (DataRow dr in _tempTransactiondt.Rows)
                    {

                        DataTable dtStockUOMch = new DataTable();
                        dtStockUOMch = oDBEngine.GetDataTable("select isnull(sProduct_StockUOM,0) sProduct_StockUOM from Master_sProducts where sProducts_ID='" + Convert.ToString(dr["ProductID"]) + "'");

                        string StockUOM = Convert.ToString(dtStockUOMch.Rows[0]["sProduct_StockUOM"]);

                        string strSrlNo = Convert.ToString(dr["SrlNo"]);
                        string strDetailsId = Convert.ToString(dr["DetailsId"]);
                        decimal strProductQuantity = Convert.ToDecimal(dr["Quantity"]);
                        decimal strUOMQuantity = 0;
                        string Val = "";

                         //Mantis Issue 24429
                        //if (taggingList.Value != null)
                        //{
                        //    Val = strDetailsId;
                        //}
                        //else
                        //{
                        //    Val = strSrlNo;
                        //}
                        //Rev Bapi
                         Val = strSrlNo;
                        //End Rev Bapi
                        //End of Mantis Issue 24429


                        if (StockUOM != "0")
                        {
                            GetQuantityBaseOnProductforDetailsId(Val, ref strUOMQuantity);


                            //Rev 24428
                            DataTable dtb = new DataTable();
                            dtb = (DataTable)Session["MultiUOMDataGRN"];
                            //if (Session["MultiUOMDataGRN"] != null)
                            //{
                            if (dtb.Rows.Count > 0)
                            { 
                                if (strUOMQuantity != null)
                                {
                                    if (strProductQuantity != strUOMQuantity)
                                    {
                                        validate = "checkMultiUOMData";
                                        grid.JSProperties["cpcheckMultiUOMData"] = strSrlNo;
                                        break;
                                    }
                                }
                            }
                            //else if (Session["MultiUOMDataGRN"] == null)
                            //{
                            else if (dtb.Rows.Count < 1)
                            {
                                validate = "checkMultiUOMData";
                                grid.JSProperties["cpcheckMultiUOMData"] = strSrlNo;
                                break;
                            }
                            //End Rev 24428
                        }

                    }

                }





                DataTable Copy_tempWarehousedt = tempWarehousedt.Copy();
                DataView dvData = new DataView(Copy_tempWarehousedt);
                dvData.RowFilter = "SerialNo <> ''";
                DataTable Filter_tempWarehousedt = dvData.ToTable();

                var duplicateRecords = Filter_tempWarehousedt.AsEnumerable()
               .GroupBy(r => r["SerialNo"])
               .Where(gr => gr.Count() > 1)
               .Select(g => g.Key);
                foreach (var d in duplicateRecords)
                {
                    grid.JSProperties["cpduplicateSerialMsg"] = "Cannot process with duplicate Serial No.";
                    validate = "duplicateSerial";
                }

                DataTable duplicateSerial = GetDuplicateDetails(PurchaseOrder_Id, _tempTransactiondt, Filter_tempWarehousedt, "FullFinal");
                if (duplicateSerial != null && duplicateSerial.Rows.Count > 0)
                {
                    foreach (DataRow Serialrow in duplicateSerial.Rows)
                    {
                        string ProductID = Convert.ToString(Serialrow["ProductID"]);
                        string SerialNo = Convert.ToString(Serialrow["SerialNo"]);

                        grid.JSProperties["cpduplicateSerialMsg"] = "(" + SerialNo + ")  Serials already exists for  (" + ProductID + ")  Product.";
                        validate = "duplicateSerial";
                        break;
                    }
                }

                #endregion
                //Rev 5.0
                DataView dvDataDuplicate = new DataView(_tempTransactiondt);
                dvDataDuplicate.RowFilter = "Status<>'D'";
                DataTable dt_tempQuotation = dvDataDuplicate.ToTable();
                var duplicate_Records = dt_tempQuotation.AsEnumerable()
               .GroupBy(r => r["ProductID"]) //coloumn name which has the duplicate values
               .Where(gr => gr.Count() > 1)
                .Select(g => g.Key);

               
                if (hdnIsDuplicateItemAllowedOrNot.Value == "0")
                {
                    foreach (var d in duplicate_Records)
                    {
                        validate = "duplicateProduct";
                    }
                }
                //Rev 5.0 END

                if (CheckPartyTagged(strVendor, strPartyInvoice) == true)
                {
                    validate = "checkPartyInvoice";
                }

                #region Check Mandatory - Terms & Condition Section

                if (Convert.ToString(ddl_AmountAre.Value) != "4")
                {
                    DataTable DT_TC = oDBEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='TC_PCMandatory' AND IsActive=1");
                    if (DT_TC != null && DT_TC.Rows.Count > 0)
                    {
                        string IsMandatory = Convert.ToString(DT_TC.Rows[0]["Variable_Value"]).Trim();

                        //  oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                        oDBEngine = new BusinessLogicLayer.DBEngine();

                        DataTable DTVisible = oDBEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='Show_TC_PC' AND IsActive=1");
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

                #region Check Mandatory - Transporte Section

                //oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                oDBEngine = new BusinessLogicLayer.DBEngine();


                DataTable DT = oDBEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='Transporter_PCMandatory' AND IsActive=1");
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

                #endregion

                #region Check Purchase Order Mandatory

                // oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable _DT = oDBEngine.GetDataTable("Config_SystemSettings", " Variable_Value ", " Variable_Name='PurchaseChallan_With_Order_Tagging' AND IsActive=1");
                if (_DT != null && _DT.Rows.Count > 0)
                {
                    string IsMandatory = Convert.ToString(_DT.Rows[0]["Variable_Value"]).Trim();
                    if (IsMandatory == "Yes")
                    {
                        if (taggingGrid.GetSelectedFieldValues("PurchaseOrder_Id").Count == 0)
                        {
                            validate = "PurchaseOrderMandatory";
                        }
                    }
                }

                #endregion

                //Rev 7.0
                CommonBL ComBL = new CommonBL();
                string GSTRateTaxMasterMandatory = ComBL.GetSystemSettingsResult("GSTRateTaxMasterMandatory");
                if (!String.IsNullOrEmpty(GSTRateTaxMasterMandatory))
                {
                    if (GSTRateTaxMasterMandatory == "Yes")
                    {
                        
                        string strTaxType = Convert.ToString(ddl_AmountAre.Value);

                        string shippingStateCode = "";
                        ProcedureExecute procstateTable = new ProcedureExecute("Prc_taxForpurchase");
                        procstateTable.AddVarcharPara("@action", 500, "GetGSTINByBranch");
                        procstateTable.AddIntegerPara("@BranchId", Convert.ToInt32(strBranch));
                        procstateTable.AddVarcharPara("@companyintId", 50, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
                        procstateTable.AddVarcharPara("@vendInternalId", 20, Convert.ToString(hdnCustomerId.Value));
                        DataSet taxForpurchase = procstateTable.GetDataSet();

                        if (taxForpurchase != null)
                        {

                            shippingStateCode = Convert.ToString(taxForpurchase.Tables[1].Rows[0][0]).Trim();
                            if (shippingStateCode.Trim() != "")
                            {
                                shippingStateCode = shippingStateCode.Substring(0, 2);
                            }
                        }

                        if (_tempTransactiondt.Columns.Contains("PurchasePriceValue"))
                        {
                            _tempTransactiondt.Columns.Remove("PurchasePriceValue");
                            _tempTransactiondt.AcceptChanges();
                        }
                        if (_tempTransactiondt.Columns.Contains("PurchaseAmountValue"))
                        {
                            _tempTransactiondt.Columns.Remove("PurchaseAmountValue");
                            _tempTransactiondt.AcceptChanges();
                        }
                        // Rev Mantis Issue 24061
                        if (_tempTransactiondt.Columns.Contains("Balance_Amount"))
                        {
                            _tempTransactiondt.Columns.Remove("Balance_Amount");
                            _tempTransactiondt.AcceptChanges();
                        }

                        DataTable dtTaxDetails = new DataTable();
                        ProcedureExecute procT = new ProcedureExecute("proc_Fetch_PruchaseChallanDetails");
                        procT.AddVarcharPara("@Action", 500, "GetTaxDetailsByProductID");
                        procT.AddPara("@ProductDetails", _tempTransactiondt);
                        procT.AddVarcharPara("@TaxOption", 10, Convert.ToString(strTaxType));
                        procT.AddVarcharPara("@SupplyState", 15, Convert.ToString(shippingStateCode));
                        procT.AddIntegerPara("@branchId",Convert.ToInt32(strBranch));
                        procT.AddVarcharPara("@CompanyId", 500, Convert.ToString(Session["LastCompany"]));
                        procT.AddVarcharPara("@ENTITY_ID", 100, Convert.ToString(hdnCustomerId.Value));
                        procT.AddVarcharPara("@TaxDATE", 100, Convert.ToString(dt_PLQuote.Date.ToString("yyyy-MM-dd")));
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
                                    decimal EntryTaxAmount = Math.Round(Convert.ToDecimal(rows[0]["Amount"]), 2);

                                    decimal Tolerance = 0;
                                    if (EntryTaxAmount != _TaxAmount)
                                    {
                                        if (EntryTaxAmount > _TaxAmount)
                                        {
                                            Tolerance = (EntryTaxAmount - _TaxAmount);
                                        }
                                        else if (EntryTaxAmount < _TaxAmount)
                                        {
                                            Tolerance = (_TaxAmount - EntryTaxAmount);
                                        }

                                        if (Convert.ToDecimal(0.05) <= Convert.ToDecimal(Tolerance))
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
                //Rev 7.0 End





                // Rev Mantis Issue 24061 [ "NetAmountExceed" added in validate check ]
                if (validate == "outrange" || validate == "checkPartyInvoice" || validate == "duplicateProduct" || validate == "nullQuantity" || validate == "duplicate" || validate == "checkWarehouse" || validate == "duplicateSerial" || validate == "TCMandatory" || validate == "nullPurchasePrice" || validate == "transporteMandatory" || validate == "PurchaseOrderMandatory" || validate == "checkMultiUOMData" || validate == "ExceedQuantity" || validate == "MINExceedQuantity" || validate=="NetAmountExceed" || validate == "checkAcurateTaxAmount")
                {
                    grid.JSProperties["cpSaveSuccessOrFail"] = validate;
                }
                else
                {
                    if (!IsPcTransactionExist(PurchaseOrder_Id))
                    {
                        int strIsComplete = 0, strChallanID = 0;
                        string strChallanNumber = "";

                        string TaxType = "", ShippingState = "";
                        //ShippingState = Convert.ToString(Purchase_BillingShipping.GetShippingStateId());

                        if (ddlPosGstChallan.Value.ToString() == "S")
                        {
                            ShippingState = Convert.ToString(Purchase_BillingShipping.GetShippingStateId());
                        }
                        else
                        {
                            ShippingState = Convert.ToString(Purchase_BillingShipping.GetBillingStateId());
                        }

                        if (Convert.ToString(ddl_AmountAre.Value) == "1") TaxType = "E";
                        else if (Convert.ToString(ddl_AmountAre.Value) == "2") TaxType = "I";

                        #region Delete Product Tax

                        string TaxDetails = Convert.ToString(hdnJsonProductTax.Value);
                        JavaScriptSerializer ser = new JavaScriptSerializer();
                        ser.MaxJsonLength = Int32.MaxValue;
                        List<ProductTaxDetails> TaxEntryJson = ser.Deserialize<List<ProductTaxDetails>>(TaxDetails);


                        foreach (DataRow productRow in _tempTransactiondt.Rows)
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

                       // TaxDetailTable = gstTaxDetails.SetTaxTableDataWithProductSerialForPurchaseRoundOff(ref _tempTransactiondt, "SrlNo", "ProductID", "TotalAmount", "TaxAmount", "NetAmount", TaxDetailTable, "P", dt_PLQuote.Date.ToString("yyyy-MM-dd"), strBranch, ShippingState, TaxType, Convert.ToString(hdnCustomerId.Value));

                        TaxDetailTable = gstTaxDetails.SetTaxTableDataWithProductSerialForPurchaseRoundOffWithException(ref _tempTransactiondt, "SrlNo", "ProductID", "TotalAmount", "TaxAmount", "NetAmount", TaxDetailTable, "P", dt_PLQuote.Date.ToString("yyyy-MM-dd"), strBranch, ShippingState, TaxType, Convert.ToString(hdnCustomerId.Value), "Quantity", "PO");

                        
                        //Subhra 13-03-2019
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


                        //AddModifyPurchaseOrder(ActionType, PurchaseOrder_Id, strIsInventory, strSchemeType, strPurchaseNumber, strPurchaseDate, strBranch, strVendor, strContactName, Reference, PosGst,
                        //                       strPartyInvoice, strPartyDate, strCurrency, strRate, strTaxOption, strTaxCode, CompanyID, FinYear, IndentRequisitionNo, IndentRequisitionDate, approveStatus,
                        //                       _tempTransactiondt, tempWarehousedt, TaxDetailTable, tempTaxDetailsdt, tempBillAddress, ref strIsComplete, ref strChallanID, ref strChallanNumber);
                        DataTable addrDesc = new DataTable();
                        addrDesc = (DataTable)Session["InlineRemarks"];

                        AddModifyPurchaseOrder(ActionType, PurchaseOrder_Id, strIsInventory, strSchemeType, strPurchaseNumber, strPurchaseDate, strBranch, strVendor, strContactName, Reference, PosGst,
                                              strPartyInvoice, strPartyDate, strCurrency, strRate, strTaxOption, strTaxCode, CompanyID, FinYear, IndentRequisitionNo, IndentRequisitionDate, approveStatus,addrDesc,
                                              _tempTransactiondt, tempWarehousedt, TaxDetailTable, tempTaxDetailsdt, tempBillAddress, ref strIsComplete, ref strChallanID, ref strChallanNumber, duplicatedt2, ProjId, MultiUOMDetails, strForBranch);



                        //End



                        if (strIsComplete == 1)
                        {
                            #region Section For Approval Section

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

                            #endregion

                            grid.JSProperties["cpPurchaseOrderNo"] = strChallanNumber;
                            Session["TaxChargeRecord"] = null;
                            if (Session["PCProductOrderDetails"] != null)
                            {
                                Session["PCProductOrderDetails"] = null;
                            }
                        }
                        else if (strIsComplete == -12)
                        {
                            grid.JSProperties["cpSaveSuccessOrFail"] = "EmptyProject";
                        }

                        else if (strIsComplete == -99)
                        {
                            grid.JSProperties["cpSaveSuccessOrFail"] = "allStockOut";
                        }
                        else
                        {
                            grid.JSProperties["cpSaveSuccessOrFail"] = "errorInsert";
                        }
                    }
                    else
                    {
                        grid.JSProperties["cpSaveSuccessOrFail"] = "transactionbeingused";
                    }
                }
            }
        }
        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];
            if (strSplitCommand == "Display")
            {
                string IsDeleteFrom = Convert.ToString(hdfIsDelete.Value);
                if (IsDeleteFrom == "D" || IsDeleteFrom == "C")
                {
                    DataTable Transactiondt = (DataTable)Session["TransactionList"];
                    grid.DataSource = GetPurchaseChallanGrid(Transactiondt);
                    grid.DataBind();

                    if (e.Parameters.Split('~').Length > 1)
                    {
                        if (e.Parameters.Split('~')[1] == "fromComponent")
                        {
                            grid.JSProperties["cpComponent"] = "true";
                        }
                    }
                }
            }
            else if (strSplitCommand == "BindGridOnQuotation")
            {
                string command = e.Parameters.ToString();
                string State = Convert.ToString(e.Parameters.Split('~')[1]);
                DataTable additionaldesc = new DataTable();
                string QuoComponent = "", QuoId = "";
                for (int i = 0; i < grid_Products.GetSelectedFieldValues("ComponentDetailsID").Count; i++)
                {
                    QuoComponent += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("ComponentDetailsID")[i]);
                    QuoId += "," + Convert.ToString(grid_Products.GetSelectedFieldValues("ComponentID")[i]);
                }
                QuoComponent = QuoComponent.TrimStart(',');
                QuoId = QuoId.TrimStart(',');
                string Quote_Nos = Convert.ToString(e.Parameters.Split('~')[1]);
                string companyId = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string fin_year = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

                if (Quote_Nos != "$")
                {
                    string strInvoiceID = Convert.ToString(Session["PurchaseChallan_Id"]);
                    // Rev Mantis Issue 24429
                    //DataSet dt_QuotationDetails = GetSelectedComponentProductList("GetSeletedPurchaseOrderDetails", QuoComponent, strInvoiceID);
                    DataSet dt_QuotationDetails = GetSelectedComponentProductList("GetSeletedPurchaseOrderDetails_New", QuoComponent, strInvoiceID);
                    // End of Mantis Issue 24429
                    DataTable MultiUOM = new DataTable();
                    MultiUOM = GetSelectedMultiUOMList(QuoComponent, QuoId);

                    Session["MultiUOMDataGRN"] = MultiUOM;
                    DataTable Transaction_dt = dt_QuotationDetails.Tables[0];
                    DataTable ProductTax_dt = dt_QuotationDetails.Tables[1];
                    DataTable Warehouse_dt = dt_QuotationDetails.Tables[2];
                    DataTable Calculate_dt = dt_QuotationDetails.Tables[3];
                    additionaldesc = dt_QuotationDetails.Tables[4];

                    GetProjectQuotationInfo(additionaldesc);
                    Session["TransactionList"] = Transaction_dt;
                    //Session["Product_StockList"] = GetChallanWarehouseData(Warehouse_dt);

                    var jsonSerialiser = new JavaScriptSerializer();
                    DataTable _Warehouse_dt = GetChallanWarehouseData(Warehouse_dt);
                    var json = jsonSerialiser.Serialize(GetStock(_Warehouse_dt));

                    if (ProductTax_dt == null) CreateDataTaxTable();
                    else Session["Product_TaxRecord"] = ProductTax_dt;

                    grid.DataSource = GetPurchaseChallanGrid(Transaction_dt);
                    grid.DataBind();

                    String _Amount = CalculateOrderAmount(Calculate_dt);

                    grid.JSProperties["cpOrderRunningBalance"] = _Amount;
                    grid.JSProperties["cpTaggingStockData"] = json;
                }
                else
                {
                    grid.DataSource = null;
                    grid.DataBind();
                }
            }
        }
        //public bool AddModifyPurchaseOrder(string ActionType, string PurchaseChallan_ID, string strIsInventory, string strSchemeType, string strPurchaseNumber,
        //                                    string strPurchaseDate, string strBranch, string strVendor, string strContactName, string Reference,string PosGst,
        //                                    string strPartyInvoice, string strPartyDate, string strCurrency, string strRate, string strTaxOption,
        //                                    string strTaxCode, string CompanyID, string FinYear,
        //                                    string IndentRequisitionNo, string IndentRequisitionDate, string ApproveStatus,
        //                                    DataTable Transactiondt, DataTable Warehousedt, DataTable TaxDetailTable,
        //                                    DataTable TaxChargesTable, DataTable BillAddress,
        //                                    ref int strIsComplete, ref int strChallanID, ref string strChallanNumber)
        public bool AddModifyPurchaseOrder(string ActionType, string PurchaseChallan_ID, string strIsInventory, string strSchemeType, string strPurchaseNumber,
                                           string strPurchaseDate, string strBranch, string strVendor, string strContactName, string Reference, string PosGst,
                                           string strPartyInvoice, string strPartyDate, string strCurrency, string strRate, string strTaxOption,
                                           string strTaxCode, string CompanyID, string FinYear,
                                           string IndentRequisitionNo, string IndentRequisitionDate, string ApproveStatus,DataTable addrDesc,
                                           DataTable Transactiondt, DataTable Warehousedt, DataTable TaxDetailTable,
                                           DataTable TaxChargesTable, DataTable BillAddress,
                                           ref int strIsComplete, ref int strChallanID, ref string strChallanNumber, DataTable PurchaseChallanPackingDetailsdt, Int64 ProjId
            , DataTable MultiUOMDetails, string strForBranch)
        {
            try
            {
                string IndentRequisitionDateformate = string.Empty;
                if (!string.IsNullOrEmpty(IndentRequisitionDate))
                {
                    //IndentRequisitionDateformate = IndentRequisitionDate.Split('-')[2] + "/" + IndentRequisitionDate.Split('-')[1] + "/" + IndentRequisitionDate.Split('-')[0] + " 00:01:47.480";

                    string Day = IndentRequisitionDate.Substring(0, 2);
                    string Month = IndentRequisitionDate.Substring(3, 2);
                    string Year = IndentRequisitionDate.Substring(6, 4);
                    IndentRequisitionDateformate = Year + "-" + Month + "-" + Day;
                }

                if (Transactiondt.Columns.Contains("PurchasePriceValue"))
                {
                    Transactiondt.Columns.Remove("PurchasePriceValue");
                    Transactiondt.AcceptChanges();
                }
                if (Transactiondt.Columns.Contains("PurchaseAmountValue"))
                {
                    Transactiondt.Columns.Remove("PurchaseAmountValue");
                    Transactiondt.AcceptChanges();
                }
                // Rev Mantis Issue 24061
                if (Transactiondt.Columns.Contains("Balance_Amount"))
                {
                    Transactiondt.Columns.Remove("Balance_Amount");
                    Transactiondt.AcceptChanges();
                }
                // End of Rev Mantis Issue 24061

                DataSet dsInst = new DataSet();


                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);

                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                //Mantis Issue 24429 
                //SqlCommand cmd = new SqlCommand("prc_PurchaseChallanModify", con);
                SqlCommand cmd = new SqlCommand("prc_trans_PurchaseChallanModify", con);
                //End of Mantis Issue 24429
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", ActionType);
                cmd.Parameters.AddWithValue("@PurchaseChallan_ID", PurchaseChallan_ID);

                cmd.Parameters.AddWithValue("@IsInventory", strIsInventory);
                cmd.Parameters.AddWithValue("@SchemeType", strSchemeType);
                cmd.Parameters.AddWithValue("@PurchaseChallan_Number", strPurchaseNumber);
                cmd.Parameters.AddWithValue("@PurchaseChallan_Date", Convert.ToDateTime(strPurchaseDate).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@PurchaseChallan_BranchId", strBranch);
                cmd.Parameters.AddWithValue("@PurchaseChallan_VendorId", strVendor);

                cmd.Parameters.AddWithValue("@ContactName_ID", strContactName);
                cmd.Parameters.AddWithValue("@Reference", Reference);
                cmd.Parameters.AddWithValue("@PosGst", PosGst);
                cmd.Parameters.AddWithValue("@PartyInvoiceNo", strPartyInvoice);
                cmd.Parameters.AddWithValue("@PartyInvoiceDate", strPartyDate);
                cmd.Parameters.AddWithValue("@AdditionalDesc", addrDesc);
                cmd.Parameters.AddWithValue("@Currency_ID", strCurrency);
                cmd.Parameters.AddWithValue("@Rate", strRate);
                cmd.Parameters.AddWithValue("@Tax_Option", strTaxOption);
                cmd.Parameters.AddWithValue("@Tax_Code", "0");
                //Tanmoy 26-09-2019
                cmd.Parameters.AddWithValue("@Project_Id", ProjId);
                //END
                //Subhra 13-03-2019
                cmd.Parameters.AddWithValue("@ForBranchId", strForBranch);
                cmd.Parameters.AddWithValue("@PurchaseChallanPackingDetails", PurchaseChallanPackingDetailsdt);
                //End
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                cmd.Parameters.AddWithValue("@FinYear", FinYear);
                cmd.Parameters.AddWithValue("@OrderIDs", IndentRequisitionNo);
                if (!string.IsNullOrEmpty(IndentRequisitionDateformate)) cmd.Parameters.AddWithValue("@OrderDate", Convert.ToDateTime(IndentRequisitionDateformate).ToString("yyyy-MM-dd"));

                cmd.Parameters.AddWithValue("@ProductDetails", Transactiondt);
                cmd.Parameters.AddWithValue("@WarehouseDetail", Warehousedt);
                cmd.Parameters.AddWithValue("@ProductTax", TaxDetailTable);
                cmd.Parameters.AddWithValue("@TaxCharges", TaxChargesTable);
                cmd.Parameters.AddWithValue("@AddressDetails", BillAddress);
                cmd.Parameters.AddWithValue("@MultiUOMDetails", MultiUOMDetails);
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(Session["userid"]));
                cmd.Parameters.AddWithValue("@approveStatus", ApproveStatus);
                cmd.Parameters.AddWithValue("@Entitytype", "DV");
                cmd.Parameters.AddWithValue("@EWayBillNumber", Convert.ToString(txtEWayBillNumber.Text.Trim()));
                cmd.Parameters.AddWithValue("@Narration", Convert.ToString(txtNarration.Text));
                //Mantis Issue 24429
                cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
                //End of Mantis Issue 24429
                cmd.Parameters.Add("@ReturnChallanID", SqlDbType.VarChar, 50);
                cmd.Parameters.Add("@ReturnChallanNumber", SqlDbType.VarChar, 50);

                cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
                cmd.Parameters["@ReturnChallanID"].Direction = ParameterDirection.Output;
                cmd.Parameters["@ReturnChallanNumber"].Direction = ParameterDirection.Output;

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);

                strIsComplete = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());
                strChallanID = Convert.ToInt32(cmd.Parameters["@ReturnChallanID"].Value.ToString());
                strChallanNumber = Convert.ToString(cmd.Parameters["@ReturnChallanNumber"].Value.ToString());

                if (strIsComplete == 1)
                {
                    if (!string.IsNullOrEmpty(hfControlData.Value))
                    {
                        CommonBL objCommonBL = new CommonBL();
                        objCommonBL.InsertTransporterControlDetails(Convert.ToInt64(strChallanID), "PC", hfControlData.Value, Convert.ToString(HttpContext.Current.Session["userid"]));
                    }

                    if (strChallanID > 0)
                    {
                        if (!string.IsNullOrEmpty(hfTermsConditionData.Value))
                        {
                            TermsConditionsControl.SaveTC(hfTermsConditionData.Value, Convert.ToString(strChallanID), "PC");
                        }
                    }


                    if (hdnShowUOMConversionInEntry.Value == "1")
                    {
                        if (HttpContext.Current.Session["SecondUOMDetails"] != null)
                        {
                            SecondUOMDetailsBL uomBL = new SecondUOMDetailsBL();
                            List<SecondUOMDetails> finalResult = (List<SecondUOMDetails>)HttpContext.Current.Session["SecondUOMDetails"];



                            var finallist = (from l in finalResult
                                             join d in Warehousedt.AsEnumerable()
                                              on l.ProductId equals d.Field<string>("Lastname")
                                             select d.ItemArray.ToList());

                            DataTable dtoutput = uomBL.SaveSencondUOMDetails(finalResult, "PC", "IN", Convert.ToString(strChallanID));
                            HttpContext.Current.Session["SecondUOMDetails"] = null;
                        }

                    }






                    //Udf Add mode
                    DataTable udfTable = (DataTable)Session["UdfDataOnAdd"];
                    if (udfTable != null)
                    {
                        Session["UdfDataOnAdd"] = reCat.insertRemarksCategoryAddMode("PC", "PurchaseChallan" + Convert.ToString(strChallanID), udfTable, Convert.ToString(Session["userid"]));
                    }

                    int retval = 0;
                    if (!string.IsNullOrEmpty(Convert.ToString(strChallanID)))
                    {
                        retval = Sendmail_PurchaseChallan(strChallanID.ToString());
                    }
                }
                cmd.Dispose();
                con.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    

        public void GetQuantityBaseOnProduct(DataTable Warehousedt, string strProductSrlNo, ref decimal WarehouseQty)
        {
            decimal sum = 0;

            if (Warehousedt != null)
            {
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

        public void GetQuantityBaseOnProductforDetailsId(string Val, ref decimal strUOMQuantity)
        {
            decimal sum = 0;
            string UomDetailsid = "";
            DataTable MultiUOMData = new DataTable();
            if (Session["MultiUOMDataGRN"] != null)
            {
                MultiUOMData = (DataTable)Session["MultiUOMDataGRN"];
                for (int i = 0; i < MultiUOMData.Rows.Count; i++)
                {
                    DataRow dr = MultiUOMData.Rows[i];
                    
                    // Mantis Issue 24429
                    //if (taggingList.Value != null)
                    //{
                    //    UomDetailsid = Convert.ToString(dr["DetailsId"]);
                    //}
                    //else
                    //{
                    //    UomDetailsid = Convert.ToString(dr["SrlNo"]);
                    //}
                    //Rev Mantis 24428
                     UomDetailsid = Convert.ToString(dr["SrlNo"]);
                    //End Mantis 24428
                    // End of Mantis Issue 24429

                    // Mantis Issue 24429
                    //if (Val == UomDetailsid)
                    if (Val == UomDetailsid && Convert.ToString(dr["Updaterow"]) == "True")
                        // End of Mantis Issue 24429
                    {
                        string strQuantity = (Convert.ToString(dr["Quantity"]) != "") ? Convert.ToString(dr["Quantity"]) : "0";
                        var weight = Decimal.Parse(Regex.Match(strQuantity, "[0-9]*\\.*[0-9]*").Value);

                        sum = Convert.ToDecimal(weight);
                    }
                }
            }

            strUOMQuantity = sum;

        }

        public bool CheckProduct_IsInventory(string ProductID)
        {
            bool IsInventory = true;

            //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();


            DataTable DT = objEngine.GetDataTable("Master_sProducts", " sProduct_IsInventory ", " sProducts_ID='" + ProductID + "'");
            if (DT != null && DT.Rows.Count > 0)
            {
                string strIsInventory = Convert.ToString(DT.Rows[0]["sProduct_IsInventory"]).Trim();

                if (strIsInventory == "True") IsInventory = true;
                else IsInventory = false;
            }

            return IsInventory;
        }
        public bool CheckPartyTagged(string strVendor, string strPartyID)
        {
            bool IsTagged = false;

            if (strPartyID != "")
            {
                //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();


                DataTable DT = objEngine.GetDataTable("tbl_trans_PurchaseChallan", " 'Y' ", " PurchaseChallan_Id<>'" + Convert.ToString(Session["PurchaseChallan_Id"]) + "' AND PurchaseChallan_VendorId='" + Convert.ToString(strVendor) + "' AND PartyInvoiceNo='" + Convert.ToString(strPartyID) + "'");
                if (DT != null && DT.Rows.Count > 0)
                {
                    IsTagged = true;
                }
            }

            return IsTagged;
        }
        private bool IsPcTransactionExist(string pcid)
        {
            bool IsExist = false;
            if (pcid != "" && Convert.ToString(pcid).Trim() != "")
            {
                DataTable dt = new DataTable();
                dt = objPurchaseChallanBL.CheckPCTraanaction(pcid);
                if (dt.Rows.Count > 0 && Convert.ToString(dt.Rows[0]["isexist"]) != "0")
                {
                    IsExist = true;
                }
            }

            return IsExist;
        }
        public int Sendmail_PurchaseChallan(string Output)
        {
            int stat = 0;
            if (chkmail.Checked)
            {
                EmailSenderHelperEL emailSenderSettings = new EmailSenderHelperEL();
                PurchasechallanEmailTags fetchModel = new PurchasechallanEmailTags();
                DataTable dt_EmailConfigpurchase = new DataTable();
                ExceptionLogging mailobj = new ExceptionLogging();
                DataTable dt_Emailbodysubject = new DataTable();
                Employee_BL objemployeebal = new Employee_BL();
                DataTable dt_EmailConfig = new DataTable();

                string Subject = "";
                string Body = "";
                string emailTo = "";
                int MailStatus = 0;

                if (!string.IsNullOrEmpty(Convert.ToString(cmbContactPerson.Value)))
                {
                    var customerid = cmbContactPerson.Value.ToString();
                    dt_EmailConfig = objemployeebal.GetemailidsForChallan(customerid);
                }

                string FilePath = Server.MapPath("~/Reports/RepxReportDesign/PurchaseChallan/DocDesign/PDFFiles/" + "PC-Default-" + Output + ".pdf");
                string FileName = FilePath;

                if (dt_EmailConfig.Rows.Count > 0)
                {
                    emailTo = Convert.ToString(dt_EmailConfig.Rows[0]["eml_email"]);
                    dt_Emailbodysubject = objemployeebal.Getemailtemplates("14");  //for purchase order

                    if (dt_Emailbodysubject.Rows.Count > 0)
                    {
                        Body = Convert.ToString(dt_Emailbodysubject.Rows[0]["body"]);
                        Subject = Convert.ToString(dt_Emailbodysubject.Rows[0]["subjct"]);

                        dt_EmailConfigpurchase = objemployeebal.Getemailtagsforpurchase(Output, "PurchaseChallanEmailTags");  //For Purchase Order Get all Tags Value

                        if (dt_EmailConfigpurchase.Rows.Count > 0)
                        {
                            fetchModel = DbHelpers.ToModel<PurchasechallanEmailTags>(dt_EmailConfigpurchase);

                            Body = Employee_BL.GetFormattedString<PurchasechallanEmailTags>(fetchModel, Body);
                            Subject = Employee_BL.GetFormattedString<PurchasechallanEmailTags>(fetchModel, Subject);
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
        public DataSet GetSelectedComponentProductList(string Action, string SelectedComponentList, string InvoiceID)
        {
            DataSet dt = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("_p_CRMTagging_Details");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@SelectedComponentList", 2000, SelectedComponentList);
            proc.AddVarcharPara("@InvoiceID", 20, InvoiceID);
            dt = proc.GetDataSet();
            return dt;
        }
        
             public DataTable GetSelectedMultiUOMList(string SelectedComponentList, string InvoiceID)
           {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_Purchasechallan_Details");
            // Rev Mantis Issue 24429
            //proc.AddVarcharPara("@Action", 500, "GetIndentDetailsForMultiUOMGridBind");
            proc.AddVarcharPara("@Action", 500, "GetIndentDetailsForMultiUOMGridBind_New");
            // End of Rev Mantis Issue 24429
            proc.AddVarcharPara("@SelectedQuotationtdetailsList", 2000, SelectedComponentList);
            proc.AddVarcharPara("@SelectedQuotationtList", 2000, InvoiceID);
            dt = proc.GetTable();
            return dt;
           }
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        #endregion

        #region TaxBlock

        protected void taxUpdatePanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

            string performpara = e.Parameter;
            if (performpara.Split('~')[0] == "DelProdbySl")
            {
                DataTable MainTaxDataTable = (DataTable)Session["Product_TaxRecord"];

                DataRow[] deletedRow = MainTaxDataTable.Select("SlNo=" + performpara.Split('~')[1]);
                if (deletedRow.Length > 0)
                {
                    foreach (DataRow dr in deletedRow)
                    {
                        MainTaxDataTable.Rows.Remove(dr);
                    }

                }

                Session["Product_TaxRecord"] = MainTaxDataTable;

                //GetStock(Convert.ToString(performpara.Split('~')[2]));
                //DeleteWarehouse(Convert.ToString(performpara.Split('~')[1]));

                DataTable taxDetails = (DataTable)Session["TaxChargeRecord"];
                if (taxDetails != null)
                {
                    foreach (DataRow dr in taxDetails.Rows)
                    {
                        dr["Amount"] = "0.00";
                    }
                    Session["TaxChargeRecord"] = taxDetails;
                }
            }
            else if (performpara.Split('~')[0] == "DeleteAllTax")
            {
                CreateDataTaxTable();

                DataTable taxDetails = (DataTable)Session["TaxChargeRecord"];

                if (taxDetails != null)
                {
                    foreach (DataRow dr in taxDetails.Rows)
                    {
                        dr["Amount"] = "0.00";
                    }
                    Session["TaxChargeRecord"] = taxDetails;
                }
            }
            else
            {
                DataTable MainTaxDataTable = (DataTable)Session["Product_TaxRecord"];

                DataRow[] deletedRow = MainTaxDataTable.Select("SlNo=" + performpara.Split('~')[1]);
                if (deletedRow.Length > 0)
                {
                    foreach (DataRow dr in deletedRow)
                    {
                        MainTaxDataTable.Rows.Remove(dr);
                    }

                }

                Session["Product_TaxRecord"] = MainTaxDataTable;
                DataTable taxDetails = (DataTable)Session["TaxChargeRecord"];
                if (taxDetails != null)
                {
                    foreach (DataRow dr in taxDetails.Rows)
                    {
                        dr["Amount"] = "0.00";
                    }
                    Session["TaxChargeRecord"] = taxDetails;
                }
            }
        }
        protected void cgridTax_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string retMsg = "";
            if (e.Parameters.Split('~')[0] != "SaveGST")
            {
                #region fetch All data For Tax

                DataTable taxDetail = new DataTable();
                DataTable MainTaxDataTable = (DataTable)Session["Product_TaxRecord"];
                if (Convert.ToString(ddl_AmountAre.Value).Trim() != "4")
                {
                    //ProcedureExecute proc = new ProcedureExecute("prc_Purchasechallan_Details");
                    //proc.AddVarcharPara("@Action", 500, "LoadOtherTaxDetails");
                    //proc.AddVarcharPara("@SProducts_ID", 10, Convert.ToString(setCurrentProdCode.Value));
                    //proc.AddVarcharPara("@OrderDate", 10, dt_PLQuote.Date.ToString("yyyy-MM-dd"));
                    //taxDetail = proc.GetTable();


                    ProcedureExecute proc = new ProcedureExecute("prc_TaxExceptionFind");
                    proc.AddVarcharPara("@Action", 500, "PO");
                    proc.AddVarcharPara("@ProductID", 10, Convert.ToString(setCurrentProdCode.Value));
                    proc.AddVarcharPara("@ENTITY_ID", 100, hdnCustomerId.Value);
                    proc.AddVarcharPara("@Date", 10, dt_PLQuote.Date.ToString("yyyy-MM-dd"));
                    proc.AddVarcharPara("@Amount", 100, HdProdGrossAmt.Value);
                    proc.AddVarcharPara("@Qty", 100, hdnQty.Value);
                    taxDetail = proc.GetTable();



                }
                else
                {
                    ProcedureExecute proc = new ProcedureExecute("prc_Purchasechallan_Details");
                    proc.AddVarcharPara("@Action", 500, "LoadImportTaxDetails");
                    proc.AddVarcharPara("@OrderDate", 10, dt_PLQuote.Date.ToString("yyyy-MM-dd"));
                    taxDetail = proc.GetTable();
                }
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

                    if (BranchGSTIN.Trim() != "")
                    {
                        BranchStateCode = BranchGSTIN.Substring(0, 2);
                    }

                }

                if (BranchGSTIN.Trim() == "")
                {
                    BranchStateCode = compGstin[0].Substring(0, 2);
                }




                string VendorState = "", _VendorCountry="";


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
                    //Mantis Issue 24806
                    else
                    {

                        DataTable GetVendorRateNew = oDBEngine.GetDataTable("SELECT top 1 add_state FROM tbl_master_contact CNT INNER JOIN tbl_master_address AD ON AD.add_cntId=cnt.cnt_internalId    where cnt_internalId='" + Convert.ToString(hdnCustomerId.Value) + "'");




                        VendorState = Convert.ToString(GetVendorRateNew.Rows[0][0]);
                    }


                    //End of Mantis Issue 24806
                }

                ProcedureExecute GetVendorCountry = new ProcedureExecute("prc_GstTaxDetails");
                GetVendorCountry.AddVarcharPara("@Action", 500, "GetVendorCountry");               
                GetVendorCountry.AddVarcharPara("@entityId", 10, Convert.ToString(hdnCustomerId.Value));
                DataTable VendorCountry = GetVendorCountry.GetTable();
                if (VendorCountry.Rows.Count > 0)
                {
                    if (Convert.ToString(VendorCountry.Rows[0]["cou_id"]).Trim() != "")
                    {
                        _VendorCountry = Convert.ToString(VendorCountry.Rows[0]["cou_id"]);
                    }

                }





                if (Convert.ToString(ddl_AmountAre.Value).Trim() != "4")
                {

                    if (VendorState.Trim() != "" && BranchStateCode != "")
                    {


                        if (BranchStateCode == VendorState)
                        {
                            //Check if the state is in union territories then only UTGST will apply
                            //   Chandigarh     Andaman and Nicobar Islands    DADRA & NAGAR HAVELI    DAMAN & DIU                  Lakshadweep              PONDICHERRY
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

                   
                    if ((compGstin[0].Trim() == "" && BranchGSTIN == "") || VendorState == "")
                    {
                        //Mantis Id 0022416 For the foreign vendor, it will be Exclusive tax and the tax category will be IGST always.
                        if (_VendorCountry != "1" && VendorState == "" && Convert.ToString(ddl_AmountAre.Value).Trim() == "1")
                        {
                            foreach (DataRow dr in taxDetail.Rows)
                            {
                                if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST" )
                                {
                                    dr.Delete();
                                }
                            }
                        }
                        else
                        {
                            //If Company GSTIN is blank then Delete All CGST,UGST,IGST,CGST
                            foreach (DataRow dr in taxDetail.Rows)
                            {
                                if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "UTGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST")
                                {
                                    dr.Delete();
                                }
                            }
                        }

                        
                        taxDetail.AcceptChanges();
                    }
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
                            if (Convert.ToString(ddl_VatGstCst.Value) == "0~0~X" || Convert.ToString(ddl_VatGstCst.Value) == "")
                            {
                                if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                                {
                                    decimal finalCalCulatedOn = 0;
                                    decimal backProcessRate = (1 + (totalParcentage / 100));
                                    finalCalCulatedOn = obj.calCulatedOn / backProcessRate;
                                    obj.calCulatedOn = finalCalCulatedOn;
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

                        //DataRow[] filtr = MainTaxDataTable.Select("TaxCode ='" + obj.Taxes_ID + "' and slNo=" + Convert.ToString(slNo));
                        //if (filtr.Length > 0)
                        //{
                        //    obj.Amount = Convert.ToDouble(filtr[0]["Amount"]);
                        //    if (obj.Taxes_ID == 0)
                        //    {
                        //        //   obj.TaxField = GetTaxName(Convert.ToInt32(Convert.ToString(filtr[0]["AltTaxCode"])));
                        //        aspxGridTax.JSProperties["cpComboCode"] = Convert.ToString(filtr[0]["AltTaxCode"]);
                        //    }
                        //    else
                        //        obj.TaxField = Convert.ToString(filtr[0]["Percentage"]);
                        //}

                        TaxDetailsDetails.Add(obj);
                    }
                }
                else
                {
                    string keyValue = e.Parameters.Split('~')[0];

                    DataTable TaxRecord = (DataTable)Session["Product_TaxRecord"];


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
                            if (Convert.ToString(ddl_VatGstCst.Value) == "0~0~X" || Convert.ToString(ddl_VatGstCst.Value) == "")
                            {
                                if (Convert.ToString(dr["TaxTypeCode"]).Trim() == "IGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "CGST" || Convert.ToString(dr["TaxTypeCode"]).Trim() == "SGST")
                                {
                                    decimal finalCalCulatedOn = 0;
                                    decimal backProcessRate = (1 + (totalParcentage / 100));
                                    finalCalCulatedOn = obj.calCulatedOn / backProcessRate;
                                    obj.calCulatedOn = finalCalCulatedOn;
                                }
                            }
                        }

                        // Rev 4.0
                        if (Convert.ToDecimal(dr["TaxRates_Rate"]) != 0)
                        {
                            obj.TaxField = Convert.ToString(dr["TaxRates_Rate"]); 
                            obj.Amount = Convert.ToDouble(obj.calCulatedOn * (Convert.ToDecimal(obj.TaxField) / 100));
                        }
                        // End of Rev 4.0

                        DataRow[] filtronexsisting1 = TaxRecord.Select("TaxCode='" + obj.Taxes_ID + "' and SlNo='" + Convert.ToString(slNo) + "'");
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



                            DataRow[] filtronexsisting = TaxRecord.Select("TaxCode='" + obj.Taxes_ID + "' and SlNo='" + Convert.ToString(slNo) + "'");
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

                        //      DataRow[] filtrIndex = databaseReturnTable.Select("ProductTax_ProductId ='" + keyValue + "' and ProductTax_QuoteId=" + Session["StockTransferID"] + " and ProductTax_TaxTypeId=0");
                        DataRow[] filtrIndex = TaxRecord.Select("SlNo='" + Convert.ToString(slNo) + "' and TaxCode=0");
                        if (filtrIndex.Length > 0)
                        {
                            aspxGridTax.JSProperties["cpComboCode"] = Convert.ToString(filtrIndex[0]["AltTaxCode"]);
                        }
                    }
                    Session["Product_TaxRecord"] = TaxRecord;

                }
                //New Changes 170217
                //GstCode should fetch everytime
                DataRow[] finalFiltrIndex = MainTaxDataTable.Select("SlNo='" + Convert.ToString(slNo) + "' and TaxCode=0");
                if (finalFiltrIndex.Length > 0)
                {
                    aspxGridTax.JSProperties["cpComboCode"] = Convert.ToString(finalFiltrIndex[0]["AltTaxCode"]);
                }

                aspxGridTax.JSProperties["cpJsonData"] = createJsonForDetails(taxDetail);

                for (var i = 0; i < TaxDetailsDetails.Count; i++)
                {
                    decimal _Amount = Convert.ToDecimal(TaxDetailsDetails[i].Amount);
                    decimal _calCulatedOn = Convert.ToDecimal(TaxDetailsDetails[i].calCulatedOn);

                    TaxDetailsDetails[i].Amount = GetRoundOfValue(_Amount);
                    TaxDetailsDetails[i].calCulatedOn = Convert.ToDecimal(GetRoundOfValue(_calCulatedOn));
                }

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
        protected void taxgrid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {

            int slNo = Convert.ToInt32(HdSerialNo.Value);
            DataTable TaxRecord = (DataTable)Session["Product_TaxRecord"];
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



                DataRow[] finalRow = TaxRecord.Select("SlNo='" + Convert.ToString(slNo) + "' and TaxCode='" + TaxCode + "'");
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

                DataRow[] finalRow = TaxRecord.Select("SlNo='" + Convert.ToString(slNo) + "' and TaxCode='0'");
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


            Session["Product_TaxRecord"] = TaxRecord;



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

            else
            {
                e.Editor.ReadOnly = false;
            }
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
        protected void gridTax_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];
            if (strSplitCommand == "Display")
            {
                DataTable TaxDetailsdt = new DataTable();
                if (Session["TaxChargeRecord"] == null)
                {
                    Session["TaxChargeRecord"] = GetTaxData(dt_PLQuote.Date.ToString("yyyy-MM-dd"));
                }

                if (Session["TaxChargeRecord"] != null)
                {
                    TaxDetailsdt = (DataTable)Session["TaxChargeRecord"];

                    #region Delete Igst,Cgst,Sgst respectively
                    //Get Company Gstin 09032017
                    string CompInternalId = Convert.ToString(Session["LastCompany"]);
                    string[] compGstin = oDBEngine.GetFieldValue1("tbl_master_company", "cmp_gstin", "cmp_internalid='" + CompInternalId + "'", 1);
                    string ShippingState = "";
                    #region ##### Added By : Samrat Roy -- For BillingShippingUserControl ######
                    string sstateCode = "";
                    //Purchase_BillingShipping.GeteShippingStateCode();
                    if (ddlPosGstChallan.Value.ToString() == "S")
                    {
                        sstateCode = Purchase_BillingShipping.GeteShippingStateCode();
                    }
                    else
                    {
                        sstateCode = Purchase_BillingShipping.GetBillingStateCode();
                    }
                    ShippingState = sstateCode;
                    if (ShippingState.Trim() != "")
                    {
                        ShippingState = ShippingState;
                    }



                    #endregion

                    if (ShippingState.Trim() != "" && compGstin[0].Trim() != "")
                    {
                        if (compGstin.Length > 0)
                        {
                            if (compGstin[0].Substring(0, 2) == ShippingState)
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
                if (Session["TaxChargeRecord"] != null)
                {
                    TaxDetailsdt = (DataTable)Session["TaxChargeRecord"];
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

                    Session["TaxChargeRecord"] = TaxDetailsdt;
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

        [WebMethod]
        public static string SaveSecondUOMDetails(string list)
        {

            System.Web.Script.Serialization.JavaScriptSerializer jsSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<SecondUOMDetails> finalResult = jsSerializer.Deserialize<List<SecondUOMDetails>>(list);
            HttpContext.Current.Session["SecondUOMDetails"] = finalResult;
            //DataTable dtoutput = uomBL.SaveSencondUOMDetails(finalResult, "SC", "OUT");

            return null;

        }


        [WebMethod]
        public static Int32 GetQuantityfromSL(string SLNo, string val)
        {

            DataTable dt = new DataTable();
             int SLVal = 0;
            if (HttpContext.Current.Session["MultiUOMDataGRN"] != null)
            {
                 //DataRow[] MultiUoMresult;
                dt = (DataTable)HttpContext.Current.Session["MultiUOMDataGRN"];
                //if (val == "1")
                //{
                //    // Mantis Issue 24429
                //    //MultiUoMresult = dt.Select("DetailsId ='" + SLNo + "'");
                //    MultiUoMresult = dt.Select("DetailsId ='" + SLNo + "' and UpdateRow ='True'");
                //    // End of Mantis Issue 24397

                //}
                //else
                //{
                //    // Mantis Issue 24429
                //    //DataRow[] MultiUoMresult = dt.Select("SrlNo ='" + SLNo + "'");
                //     MultiUoMresult = dt.Select("SrlNo ='" + SLNo + "' and UpdateRow ='True'");
                //    // End of Mantis Issue 24397
                //}
                 DataRow[] MultiUoMresult = dt.Select("SrlNo ='" + SLNo + "' and UpdateRow ='True'");
                SLVal = MultiUoMresult.Length;
                // End of Mantis Issue 24429


            }

            return SLVal;
        }


        [WebMethod]
        public static object AutoPopulateAltQuantity(Int64 ProductID)
        {
            List<MultiUOMPacking> RateLists = new List<MultiUOMPacking>();

            decimal packing_quantity = 0;
            decimal sProduct_quantity = 0;
            Int32 AltUOMId = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_Purchasechallan_Details");
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
        public static object GetPackingQuantity(Int32 UomId, Int32 AltUomId, Int64 ProductID)
        {
            List<MultiUOMPacking> RateLists = new List<MultiUOMPacking>();

            decimal packing_quantity = 0;
            decimal sProduct_quantity = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_Purchasechallan_Details");
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
                        if (Convert.ToString(ddl_VatGstCst.Value) == "0~0~X" || Convert.ToString(ddl_VatGstCst.Value) == "")
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
        public DataTable GetTaxData(string quoteDate)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_Purchasechallan_Details");
            proc.AddVarcharPara("@Action", 500, "TaxDetails");
            proc.AddVarcharPara("@PurchaseChallan_Id", 500, Convert.ToString(Session["PurchaseChallan_Id"]));
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(Session["LastFinYear"]));
            proc.AddVarcharPara("@branchid", 500, Convert.ToString(ddl_Branch.SelectedValue));
            proc.AddVarcharPara("@campany_Id", 500, Convert.ToString(Session["LastCompany"]));
            proc.AddVarcharPara("@OrderDate", 10, quoteDate);
            dt = proc.GetTable();
            return dt;
        }
        protected void gridTax_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            DataTable TaxDetailsdt = new DataTable();
            if (Session["TaxChargeRecord"] != null)
            {
                TaxDetailsdt = (DataTable)Session["TaxChargeRecord"];
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

            Session["TaxChargeRecord"] = TaxDetailsdt;

            decimal cpChargesAmt = 0;
            if (TaxDetailsdt != null && TaxDetailsdt.Rows.Count > 0)
            {
                foreach (DataRow rrow in TaxDetailsdt.Rows)
                {
                    string value = (Convert.ToString(rrow["Amount"]) != "") ? Convert.ToString(rrow["Amount"]) : "0";

                    if (Convert.ToString(rrow["Taxes_Name"]).Contains('+') == true)
                    {
                        cpChargesAmt = cpChargesAmt + Convert.ToDecimal(value);
                    }
                    else if (Convert.ToString(rrow["Taxes_Name"]).Contains('-') == true)
                    {
                        cpChargesAmt = cpChargesAmt - Convert.ToDecimal(value);
                    }
                }
            }
            gridTax.JSProperties["cpChargesAmt"] = Convert.ToString(cpChargesAmt);

            gridTax.DataSource = GetTaxes(TaxDetailsdt);
            gridTax.DataBind();
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
        protected void gridTax_DataBinding(object sender, EventArgs e)
        {
            if (Session["TaxChargeRecord"] != null)
            {
                DataTable TaxDetailsdt = (DataTable)Session["TaxChargeRecord"];

                //gridTax.DataSource = GetTaxes();
                var taxlist = (List<Taxes>)GetTaxes(TaxDetailsdt);
                var taxChargeDataSource = setChargeCalculatedOn(taxlist, TaxDetailsdt);
                gridTax.DataSource = taxChargeDataSource;
            }
        }
        public DataTable GetQuotationProductTaxData(string strQuotationList)
        {
            try
            {
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_Purchasechallan_Details");
                proc.AddVarcharPara("@Action", 500, "PurchaseOrderProductTax");
                proc.AddVarcharPara("@OrderList", 3000, strQuotationList);
                dt = proc.GetTable();

                return dt;
            }
            catch
            {
                return null;
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
            Session["Product_TaxRecord"] = TaxRecord;
        }
        public void DeleteTaxDetails(string SrlNo)
        {
            DataTable TaxDetailTable = new DataTable();
            if (Session["Product_TaxRecord"] != null)
            {
                TaxDetailTable = (DataTable)Session["Product_TaxRecord"];

                var rows = TaxDetailTable.Select("SlNo ='" + SrlNo + "'");
                foreach (var row in rows)
                {
                    row.Delete();
                }
                TaxDetailTable.AcceptChanges();

                Session["Product_TaxRecord"] = TaxDetailTable;
            }
        }
        public bool ImplementTaxOnTagging(int id, int slno)
        {
            Boolean inUse = false;
            DataTable taxTable = (DataTable)Session["Product_TaxRecord"];
            ProcedureExecute proc = new ProcedureExecute("prc_taxReturnTable");
            proc.AddVarcharPara("@Module", 20, "PurchaseChallan");
            proc.AddIntegerPara("@id", id);
            proc.AddIntegerPara("@slno", slno);
            proc.AddBooleanPara("@inUse", true, QueryParameterDirection.Output);

            DataTable returnedTable = proc.GetTable();
            inUse = Convert.ToBoolean(proc.GetParaValue("@inUse"));

            if (returnedTable != null)
                taxTable.Merge(returnedTable);


            Session["Product_TaxRecord"] = taxTable;

            return inUse;
        }
        protected DataTable GetTaxDataWithGST(DataTable existing)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_Purchasechallan_Details");
            proc.AddVarcharPara("@Action", 500, "TaxDetailsForGst");
            proc.AddVarcharPara("@PurchaseChallan_Id", 500, Convert.ToString(Session["StockTransferID"]));
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
        public DataSet GetQuotationEditedTaxData()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_Purchasechallan_Details");
            proc.AddVarcharPara("@Action", 500, "ProductEditedTaxDetails");
            proc.AddVarcharPara("@PurchaseChallan_Id", 500, Convert.ToString(Session["StockTransferID"]));
            ds = proc.GetDataSet();
            return ds;
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
        public void UpdateTaxDetails(string oldSrlNo, string newSrlNo)
        {
            DataTable TaxDetailTable = new DataTable();
            if (Session["Product_TaxRecord"] != null)
            {
                TaxDetailTable = (DataTable)Session["Product_TaxRecord"];

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

                Session["Product_TaxRecord"] = TaxDetailTable;
            }
        }
        public double GetRoundOfValue(decimal Value)
        {
            double RoundOfValue = 0;

            if (Value > 0)
            {
                decimal _Value = Convert.ToDecimal(Value);
                _Value = Math.Round(Value, 2);

                RoundOfValue = Convert.ToDouble(_Value);
            }

            return RoundOfValue;
        }

        #endregion

        #region WebMethod

        [WebMethod]
        public static bool CheckUniqueName(string VoucherNo)
        {
            MShortNameCheckingBL objMShortNameCheckingBL = new MShortNameCheckingBL();
            DataTable dt = new DataTable();
            Boolean status = false;
            BusinessLogicLayer.GenericMethod oGeneric = new BusinessLogicLayer.GenericMethod();

            if (VoucherNo != "" && Convert.ToString(VoucherNo).Trim() != "")
            {
                status = objMShortNameCheckingBL.CheckUnique(Convert.ToString(VoucherNo).Trim(), "0", "PurchaseChallan_Check");
            }
            return status;
        }
        //REV 3.0
        [WebMethod]
        public static bool CheckUniqueBatchNo(string BatchNo,string WarehouseID,string ProductID)
        {           
            DataTable dt = new DataTable();
            Boolean status = false;
            BusinessLogicLayer.GenericMethod oGeneric = new BusinessLogicLayer.GenericMethod();

            if (BatchNo != "" && Convert.ToString(BatchNo).Trim() != "")
            {
                ProcedureExecute proc;
                try
                {
                    using (proc = new ProcedureExecute("proc_Fetch_PruchaseChallanDetails"))
                    {
                        proc.AddVarcharPara("@action", 50, "CheckUniqueBatchNo");                       
                        proc.AddVarcharPara("@BatchNo", 200, BatchNo);
                        proc.AddBigIntegerPara("@WarehouseID",Convert.ToInt32(WarehouseID));
                        proc.AddBigIntegerPara("@ProductID", Convert.ToInt32(ProductID));
                        proc.AddBooleanPara("@ReturnValue", false, QueryParameterDirection.Output);
                        int i = proc.RunActionQuery();
                        status = Convert.ToBoolean(proc.GetParaValue("@ReturnValue"));
                        
                    }
                }

                catch (Exception ex)
                {
                    return false;
                }

                finally
                {
                    proc = null;
                }
            }
            return status;
        }
        //REV 3.0 END
        [WebMethod]
        public static object PurchaseOrderDocumentAddress(string OrderId)
        {
            List<PurchaseOrderDetails> Detail = new List<PurchaseOrderDetails>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                ProcedureExecute proc = new ProcedureExecute("proc_Fetch_PruchaseChallanDetails");
                proc.AddVarcharPara("@Action", 500, "GetPurchaseOrderTaggingAddress");
                proc.AddVarcharPara("@OrderId", 100, OrderId);
                DataTable address = proc.GetTable();



                Detail = (from DataRow dr in address.Rows
                          select new PurchaseOrderDetails()

                          {
                              Type = Convert.ToString(dr["Type"]),
                              Address1 = Convert.ToString(dr["Address1"]),
                              Address2 = Convert.ToString(dr["Address2"]),
                              Address3 = Convert.ToString(dr["Address3"]),
                              CountryId = Convert.ToInt32(dr["CountryId"]),
                              CountryName = Convert.ToString(dr["CountryName"]),
                              StateId = Convert.ToInt32(dr["StateId"]),
                              StateName = Convert.ToString(dr["StateName"]),
                              StateCode = Convert.ToString(dr["StateCode"]),
                              CityId = Convert.ToInt32(dr["CityId"]),
                              CityName = Convert.ToString(dr["CityName"]),
                              PinId = Convert.ToInt32(dr["PinId"]),
                              PinCode = Convert.ToString(dr["PinCode"]),
                              AreaId = Convert.ToInt32(dr["AreaId"]),
                              AreaName = Convert.ToString(dr["AreaName"]),
                              GSTIN = Convert.ToString(dr["GSTIN"]),
                              Landmark = Convert.ToString(dr["Landmark"]),
                              ShipToPartyId = Convert.ToString(dr["ShipToPartyId"]),
                              ShipToPartyName = Convert.ToString(dr["ShipToPartyName"]),
                              PosForGst = Convert.ToString(dr["PosForGst"])


                          }).ToList();
                return Detail;

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

        #endregion

        #region   Purchase Order Tagging

        protected void taggingGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = Convert.ToString(e.Parameters.Split('~')[0]);

            if (strSplitCommand == "BindComponentGrid")
            {
                string Action = "GetPurchaseOrder";
                string BranchID = Convert.ToString(ddl_Branch.SelectedValue);
                string FinYear = Convert.ToString(Session["LastFinYear"]);
                string OrderDate = dt_PLQuote.Date.ToString("yyyy-MM-dd");
                string Vendor = Convert.ToString(hdnCustomerId.Value);
                string ComponentType = "";
                DataTable ComponentTable = new DataTable();
                string strChallanID = Convert.ToString(Session["PurchaseChallan_Id"]);
                ComponentTable = objSalesInvoiceBL.GetComponent(Vendor, OrderDate, ComponentType, FinYear, BranchID, Action, strChallanID);

                Session["PORequiData"] = ComponentTable;
                taggingGrid.DataSource = ComponentTable;
                taggingGrid.DataBind();
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
            if (Session["PORequiData"] != null)
            {
                taggingGrid.DataSource = (DataTable)Session["PORequiData"];
            }
        }
        protected void cgridProducts_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];
            if (strSplitCommand == "BindProductsDetails")
            {
                String QuoComponent = "", QuoComponentNumber = "", QuoComponentDate = "";
                for (int i = 0; i < taggingGrid.GetSelectedFieldValues("PurchaseOrder_Id").Count; i++)
                {
                    QuoComponent += "," + Convert.ToString(taggingGrid.GetSelectedFieldValues("PurchaseOrder_Id")[i]);
                    QuoComponentNumber += "," + Convert.ToString(taggingGrid.GetSelectedFieldValues("PurchaseOrder_Number")[i]);
                    QuoComponentDate += "," + Convert.ToString(taggingGrid.GetSelectedFieldValues("ComponentDate")[i]);
                }

                QuoComponent = QuoComponent.TrimStart(',');
                QuoComponentNumber = QuoComponentNumber.TrimStart(',');
                QuoComponentDate = QuoComponentDate.TrimStart(',');

                string GRNID = Convert.ToString(QuoComponent.Split(',')[0]);

                Int64 ProjId = 0;
                DataTable dtproj = GetProjectEditData(GRNID);
                if (dtproj != null && dtproj.Rows.Count > 0)
                {
                    ProjId = Convert.ToInt64(dtproj.Rows[0]["Proj_Id"]);
                }
                else
                {
                    ProjId = 0;
                }


                if (taggingGrid.GetSelectedFieldValues("PurchaseOrder_Id").Count > 0)
                {
                    if (taggingGrid.GetSelectedFieldValues("PurchaseOrder_Id").Count > 1)
                    {
                        QuoComponentDate = "Multiple Purchase Order Dates";
                    }
                }
                else
                {
                    QuoComponentDate = "";
                }

                string strAction = "GetPurchaseOrderProducts";
                string strChallanID = Convert.ToString(Session["PurchaseChallan_Id"]);
                DataTable dtDetails = objSalesInvoiceBL.GetComponentProductList(strAction, QuoComponent, strChallanID);

                grid_Products.DataSource = dtDetails;
                grid_Products.DataBind();

                grid_Products.JSProperties["cpComponentDetails"] = QuoComponentNumber + "~" + QuoComponentDate + "~" + ProjId;
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
        public string CalculateOrderAmount(DataTable dt)
        {
            decimal SUM_Amount = 0, SUM_TotalAmount = 0, SUM_TaxAmount = 0, SUM_ChargesAmount = 0, SUM_ProductQuantity = 0,TaxableAmount=0;
            string Tax_Option = "", Currency = "1", Rate = "0";

            if (dt != null && dt.Rows.Count > 0)
            {
                string ProductAmount = Convert.ToString(dt.Rows[0]["ProductAmount"]);
                string ProductTotalAmount = Convert.ToString(dt.Rows[0]["ProductTotalAmount"]);
                string NetAmount = Convert.ToString(dt.Rows[0]["NetAmount"]);
                string ChargesAmount = Convert.ToString(dt.Rows[0]["ChargesAmount"]);
                string ProductTaxAmount = Convert.ToString(dt.Rows[0]["ProductTaxAmount"]);
                string ProductQuantity = Convert.ToString(dt.Rows[0]["ProductQuantity"]);
                Tax_Option = Convert.ToString(dt.Rows[0]["Tax_Option"]);
                Currency = Convert.ToString(dt.Rows[0]["Currency"]);
                Rate = Convert.ToString(dt.Rows[0]["Rate"]);

                SUM_Amount = Convert.ToDecimal(ProductTotalAmount);
                SUM_TotalAmount = Convert.ToDecimal(NetAmount);
                SUM_ProductQuantity = Convert.ToDecimal(ProductQuantity);
                SUM_TaxAmount = Convert.ToDecimal(ChargesAmount);
                TaxableAmount = Convert.ToDecimal(ProductAmount);
            }

            return Convert.ToString(SUM_ChargesAmount + "~" + SUM_Amount + "~" + SUM_ChargesAmount + "~" + SUM_TaxAmount + "~" + SUM_TotalAmount + "~" + SUM_TotalAmount + "~" + SUM_ProductQuantity + "~" + Tax_Option + '~' + Currency + '~' + Rate + '~' + TaxableAmount);
        }

        #endregion

        protected void callback_InlineRemarks_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            string SrlNo = e.Parameter.Split('~')[1];
            string RemarksVal = e.Parameter.Split('~')[2];
            Remarks = new DataTable();


            callback_InlineRemarks.JSProperties["cpDisplayFocus"] = "";
            callback_InlineRemarks.JSProperties["cpRemarksFinalFocus"] = "";

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
                        Remarks.Columns.Add("ChallanDetails_AddiDesc", typeof(string));

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
                callback_InlineRemarks.JSProperties["cpRemarksFinalFocus"] = "RemarksFinalFocus";
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
                        txtInlineRemarks.Text = dvData[0]["ChallanDetails_AddiDesc"].ToString();
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

        public DataTable GetProjectEditData(string VendorePayID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_CustomerPaymentReciptProjectID");
            proc.AddIntegerPara("@Receipt_ID", Convert.ToInt32(VendorePayID));
            proc.AddVarcharPara("@Action", 100, "Purchase_Order");
            dt = proc.GetTable();
            return dt;
        }

        #region Warehouse Details

        protected void WarehousePanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string strIsViewMode = "";
            string ProductSrlNo = "";
            string strSplitCommand = e.Parameter.Split('~')[0];
            WarehousePanel.JSProperties["cpIsShow"] = null;

            if (strSplitCommand == "StockDisplay")
            {
                ProductSrlNo = Convert.ToString(hdfProductSrlNo.Value);
                strIsViewMode = "Y";
            }
            else if (strSplitCommand == "StockSave")
            {
                string Sales_UOM_Name = Convert.ToString(hdfUOM.Value);
                string Stk_UOM_Name = Convert.ToString(hdfUOM.Value);
                string Conversion_Multiplier = "1";
                string Type = Convert.ToString(hdfWarehousetype.Value);
                bool IsDelete = false;
                int loopId = Convert.ToInt32(Session["Stock_LoopID"]);

                string WarehouseID = Convert.ToString(e.Parameter.Split('~')[1]);
                string WarehouseName = Convert.ToString(e.Parameter.Split('~')[2]);
                string BatchName = Convert.ToString(e.Parameter.Split('~')[3]);
                string MfgDate = Convert.ToString(e.Parameter.Split('~')[4]);
                string ExpiryDate = Convert.ToString(e.Parameter.Split('~')[5]);
                string SerialNo = Convert.ToString(e.Parameter.Split('~')[6]);
                string Qty = Convert.ToString(e.Parameter.Split('~')[7]);
                string editWarehouseID = Convert.ToString(e.Parameter.Split('~')[8]);
                string AltQty = Convert.ToString(e.Parameter.Split('~')[9]);

                string ProductID = Convert.ToString(hdfProductID.Value);
                ProductSrlNo = Convert.ToString(hdfProductSrlNo.Value);

                DataTable Warehousedt = (DataTable)Session["Product_StockList"];

                if (Type == "WBS")
                {
                    int SerialNoCount_Server = CheckSerialNoExists(SerialNo);
                    var SerialNoCount_Local = Warehousedt.Select("SerialNo ='" + SerialNo + "' AND SrlNo<>'" + editWarehouseID + "'");

                    if (editWarehouseID == "0")
                    {
                        if (SerialNoCount_Server == 0 && SerialNoCount_Local.Length == 0)
                        {
                            string maxID = GetWarehouseMaxValue(Warehousedt);
                            var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND BatchID ='" + BatchName + "' AND Product_SrlNo='" + ProductSrlNo + "'");

                            if (updaterows.Length == 0)
                            {
                                Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, WarehouseName, "1", "1", Convert.ToDecimal("1") + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, loopId, "D", MfgDate, ExpiryDate);
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
                                Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, "", (oldQuantity + Convert.ToDecimal(1)), "0", "", Sales_UOM_Name, BatchName, "", MfgDate, ExpiryDate, SerialNo, newloopID, "D", "", "");
                            }
                        }
                        else
                        {
                            WarehousePanel.JSProperties["cperrorMsg"] = "duplicateSerial";
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
                                    var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND BatchID ='" + BatchName + "' AND Product_SrlNo='" + ProductSrlNo + "'");

                                    if (updaterows.Length == 0)
                                    {
                                        Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, WarehouseName, "1", "1", Convert.ToDecimal("1") + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, loopId, "D", MfgDate, ExpiryDate);
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
                                        Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, "", (oldQuantity + Convert.ToDecimal(1)), "0", "", Sales_UOM_Name, BatchName, "", MfgDate, ExpiryDate, SerialNo, newloopID, "D", "", "");
                                    }
                                }
                            }
                        }
                        else
                        {
                            WarehousePanel.JSProperties["cperrorMsg"] = "duplicateSerial";
                        }
                    }
                }
                else if (Type == "W")
                {
                    if (editWarehouseID == "0")
                    {
                        string maxID = GetWarehouseMaxValue(Warehousedt);
                        var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND Product_SrlNo='" + ProductSrlNo + "'");

                        if (updaterows.Length == 0)
                        {
                            Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, WarehouseName, Qty, Qty, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, loopId, "D", MfgDate, ExpiryDate);
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

                            var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND Product_SrlNo='" + ProductSrlNo + "'");
                            foreach (var row in updaterows)
                            {
                                whID = Convert.ToString(row["SrlNo"]);
                            }

                            if (updaterows.Length == 0)
                            {
                                IsDelete = true;
                                Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, WarehouseName, Qty, Qty, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, loopId, "D", MfgDate, ExpiryDate);
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
                    if (editWarehouseID == "0")
                    {
                        string maxID = GetWarehouseMaxValue(Warehousedt);
                        var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND BatchID='" + BatchName + "' AND Product_SrlNo='" + ProductSrlNo + "'");

                        if (updaterows.Length == 0)
                        {
                            Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, WarehouseName, Qty, Qty, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, loopId, "D", MfgDate, ExpiryDate);
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

                            var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND BatchID='" + BatchName + "' AND Product_SrlNo='" + ProductSrlNo + "'");
                            foreach (var row in updaterows)
                            {
                                whID = Convert.ToString(row["SrlNo"]);
                            }

                            if (updaterows.Length == 0)
                            {
                                IsDelete = true;
                                Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, WarehouseName, Qty, Qty, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, loopId, "D", MfgDate, ExpiryDate);
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
                    WarehouseID = "0";

                    if (editWarehouseID == "0")
                    {
                        string maxID = GetWarehouseMaxValue(Warehousedt);
                        var updaterows = Warehousedt.Select("BatchID ='" + BatchName + "' AND Product_SrlNo='" + ProductSrlNo + "'");

                        if (updaterows.Length == 0)
                        {
                            Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, WarehouseName, Qty, Qty, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, loopId, "D", MfgDate, ExpiryDate);
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

                            var updaterows = Warehousedt.Select("BatchID ='" + BatchName + "' AND Product_SrlNo='" + ProductSrlNo + "'");
                            foreach (var row in updaterows)
                            {
                                whID = Convert.ToString(row["SrlNo"]);
                            }

                            if (updaterows.Length == 0)
                            {
                                IsDelete = true;
                                Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, WarehouseName, Qty, Qty, Convert.ToDecimal(Qty) + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, loopId, "D", MfgDate, ExpiryDate);
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
                    int SerialNoCount_Server = CheckSerialNoExists(SerialNo);
                    var SerialNoCount_Local = Warehousedt.Select("SerialNo ='" + SerialNo + "' AND SrlNo<>'" + editWarehouseID + "'");

                    if (editWarehouseID == "0")
                    {
                        if (SerialNoCount_Server == 0 && SerialNoCount_Local.Length == 0)
                        {
                            string maxID = GetWarehouseMaxValue(Warehousedt);

                            int newloopID = 0;
                            decimal oldQuantity = 0;

                            var updaterows = Warehousedt.Select("Product_SrlNo='" + ProductSrlNo + "'");
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

                                Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, WarehouseName, (oldQuantity + Convert.ToDecimal(1)), "0", "", Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, newloopID, "D", MfgDate, ExpiryDate);
                            }
                            else
                            {
                                newloopID = loopId;
                                Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, WarehouseName, "1", "1", Convert.ToDecimal(1) + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, newloopID, "D", MfgDate, ExpiryDate);
                            }
                        }
                        else
                        {
                            WarehousePanel.JSProperties["cperrorMsg"] = "duplicateSerial";
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
                            WarehousePanel.JSProperties["cperrorMsg"] = "duplicateSerial";
                        }
                    }
                }
                else if (Type == "WS")
                {
                    int SerialNoCount_Server = CheckSerialNoExists(SerialNo);
                    var SerialNoCount_Local = Warehousedt.Select("SerialNo ='" + SerialNo + "' AND SrlNo<>'" + editWarehouseID + "'");

                    if (editWarehouseID == "0")
                    {
                        if (SerialNoCount_Server == 0 && SerialNoCount_Local.Length == 0)
                        {
                            string maxID = GetWarehouseMaxValue(Warehousedt);
                            var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND Product_SrlNo='" + ProductSrlNo + "'");

                            if (updaterows.Length == 0)
                            {
                                Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, WarehouseName, "1", "1", Convert.ToDecimal("1") + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, loopId, "D", MfgDate, ExpiryDate);
                            }
                            else
                            {
                                int newloopID = 0;
                                decimal oldQuantity = 0;

                                foreach (var row in updaterows)
                                {
                                    oldQuantity = Convert.ToDecimal(row["Quantity"]);
                                    newloopID = Convert.ToInt32(row["LoopID"]);
                                    break;

                                    //row["Quantity"] = (oldQuantity + Convert.ToDecimal(1));
                                    //row["MfgDate"] = MfgDate;
                                    //row["ExpiryDate"] = ExpiryDate;
                                    //newloopID = Convert.ToInt32(row["LoopID"]);

                                    //if (Convert.ToDecimal(row["TotalQuantity"]) != 0)
                                    //{
                                    //    row["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(1)) + " " + Sales_UOM_Name;
                                    //}
                                }

                                Warehousedt.Select(string.Format("[WarehouseID]='{0}' and [Product_SrlNo]='{1}' and TotalQuantity<>'0' ", WarehouseID, ProductSrlNo)).ToList<DataRow>().ForEach(
                                  r => { r["SalesQuantity"] = (oldQuantity + Convert.ToDecimal(1)) + " " + Sales_UOM_Name; r["TotalQuantity"] = (oldQuantity + Convert.ToDecimal(1)); });

                                Warehousedt.Select(string.Format("[WarehouseID]='{0}' and [Product_SrlNo]='{1}' ", WarehouseID, ProductSrlNo)).ToList<DataRow>().ForEach(
                                    r => { r["Quantity"] = (oldQuantity + Convert.ToDecimal(1)); r["MfgDate"] = MfgDate; r["ExpiryDate"] = ExpiryDate; });

                                Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, "", (oldQuantity + Convert.ToDecimal(1)), "0", "", Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, newloopID, "D", MfgDate, ExpiryDate);
                            }
                        }
                        else
                        {
                            WarehousePanel.JSProperties["cperrorMsg"] = "duplicateSerial";
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
                                    var updaterows = Warehousedt.Select("WarehouseID ='" + WarehouseID + "' AND Product_SrlNo='" + ProductSrlNo + "'");

                                    if (updaterows.Length == 0)
                                    {
                                        Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, WarehouseName, "1", "1", Convert.ToDecimal("1") + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, loopId, "D", MfgDate, ExpiryDate);
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
                                        Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, "", (oldQuantity + Convert.ToDecimal(1)), "0", "", Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, newloopID, "D", MfgDate, ExpiryDate);
                                    }
                                }
                            }
                        }
                        else
                        {
                            WarehousePanel.JSProperties["cperrorMsg"] = "duplicateSerial";
                        }
                    }
                }
                else if (Type == "BS")
                {
                    int SerialNoCount_Server = CheckSerialNoExists(SerialNo);
                    var SerialNoCount_Local = Warehousedt.Select("SerialNo ='" + SerialNo + "' AND SrlNo<>'" + editWarehouseID + "'");

                    if (editWarehouseID == "0")
                    {
                        if (SerialNoCount_Server == 0 && SerialNoCount_Local.Length == 0)
                        {
                            string maxID = GetWarehouseMaxValue(Warehousedt);
                            var updaterows = Warehousedt.Select("BatchID ='" + BatchName + "' AND Product_SrlNo='" + ProductSrlNo + "'");

                            if (updaterows.Length == 0)
                            {
                                Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, WarehouseName, "1", "1", Convert.ToDecimal("1") + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, loopId, "D", MfgDate, ExpiryDate);
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
                                Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, "", (oldQuantity + Convert.ToDecimal(1)), "0", "", Sales_UOM_Name, BatchName, "", MfgDate, ExpiryDate, SerialNo, newloopID, "D", "", "");
                            }
                        }
                        else
                        {
                            WarehousePanel.JSProperties["cperrorMsg"] = "duplicateSerial";
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
                                    var updaterows = Warehousedt.Select("BatchID ='" + BatchName + "' AND Product_SrlNo='" + ProductSrlNo + "'");

                                    if (updaterows.Length == 0)
                                    {
                                        Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, WarehouseName, "1", "1", Convert.ToDecimal("1") + " " + Sales_UOM_Name, Sales_UOM_Name, BatchName, BatchName, MfgDate, ExpiryDate, SerialNo, loopId, "D", MfgDate, ExpiryDate);
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
                                        Warehousedt.Rows.Add(ProductSrlNo, maxID, WarehouseID, "", (oldQuantity + Convert.ToDecimal(1)), "0", "", Sales_UOM_Name, BatchName, "", MfgDate, ExpiryDate, SerialNo, newloopID, "D", "", "");
                                    }
                                }
                            }
                        }
                        else
                        {
                            WarehousePanel.JSProperties["cperrorMsg"] = "duplicateSerial";
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

                Session["Product_StockList"] = Warehousedt;
                Session["Stock_LoopID"] = loopId + 1;

                if (Type == "W" || Type == "WB" || Type == "WS")
                {
                    #region Bind WarehouseList with Default Warehouse

                    DataTable dt = GetWarehouseData();
                    string strBranch = Convert.ToString(ddl_Branch.SelectedValue);

                    CmbWarehouseID.Items.Clear();
                    CmbWarehouseID.DataSource = dt;
                    CmbWarehouseID.DataBind();
                    CmbWarehouseID.Value = WarehouseID;

                    #endregion

                    txtBatchName.Text = "";
                    txtQuantity.Text = "0";
                    txtserialID.Text = "";
                    txtStartDate.Value = null;
                    txtEndDate.Value = null;
                }
                else if (Type == "WBS")
                {
                    #region Bind WarehouseList with Default Warehouse

                    DataTable dt = GetWarehouseData();
                    string strBranch = Convert.ToString(ddl_Branch.SelectedValue);

                    CmbWarehouseID.Items.Clear();
                    CmbWarehouseID.DataSource = dt;
                    CmbWarehouseID.DataBind();
                    CmbWarehouseID.Value = WarehouseID;

                    #endregion

                    txtBatchName.Text = "";
                    txtQuantity.Text = "0";
                    txtStartDate.Value = null;
                    txtEndDate.Value = null;
                }
                else if (Type == "B" || Type == "BS")
                {
                    txtQuantity.Text = "0";
                    txtserialID.Text = "";
                }
                else if (Type == "S")
                {
                    txtBatchName.Text = "";
                    txtQuantity.Text = "0";
                    txtserialID.Text = "";
                    txtStartDate.Value = null;
                    txtEndDate.Value = null;
                }

                #region Bind WarehouseList with Default Warehouse

                DataTable _dt = GetWarehouseData();
                string strBranchID = Convert.ToString(ddl_Branch.SelectedValue);

                CmbWarehouseID.Items.Clear();
                CmbWarehouseID.DataSource = _dt;
                CmbWarehouseID.DataBind();

                //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

                DataTable dtdefaultStock = objEngine.GetDataTable("tbl_master_building", " top 1 bui_id ", " bui_BranchId='" + strBranchID + "' ");
                if (dtdefaultStock != null && dtdefaultStock.Rows.Count > 0)
                {
                    string defaultID = Convert.ToString(dtdefaultStock.Rows[0]["bui_id"]).Trim();

                    if (defaultID != null || defaultID != "" || defaultID != "0")
                    {
                        CmbWarehouseID.Value = defaultID;
                    }
                }

                #endregion

                txtBatchName.Text = "";
                txtQuantity.Text = "0";
                txtserialID.Text = "";
                txtStartDate.Value = null;
                txtEndDate.Value = null;

                #region Bind Stock Record of the Product

                if (Warehousedt != null && Warehousedt.Rows.Count > 0)
                {
                    Warehousedt.DefaultView.Sort = "LoopID,SrlNo ASC";
                    DataTable sortWarehousedt = Warehousedt.DefaultView.ToTable();

                    DataView dvData = new DataView(sortWarehousedt);
                    dvData.RowFilter = "Product_SrlNo = '" + ProductSrlNo + "'";

                    GrdWarehouse.DataSource = dvData;
                    GrdWarehouse.DataBind();
                }
                else
                {
                    GrdWarehouse.DataSource = Warehousedt.DefaultView;
                    GrdWarehouse.DataBind();
                }

                #endregion

                WarehousePanel.JSProperties["cpIsShow"] = "Y";
            }
            else if (strSplitCommand == "WarehouseFinal")
            {
                if (Session["Product_StockList"] != null)
                {
                    DataTable PC_WarehouseData = (DataTable)Session["Product_StockList"];
                    string ProductID = Convert.ToString(hdfProductSrlNo.Value);
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
                        WarehousePanel.JSProperties["cpIsSave"] = "Y";
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
                        WarehousePanel.JSProperties["cpIsSave"] = "N";
                    }

                    Session["Product_StockList"] = PC_WarehouseData;
                    WarehousePanel.JSProperties["cpIsShow"] = "Y";
                }
            }
            else if (strSplitCommand == "Delete")
            {
                string strKey = e.Parameter.Split('~')[1];
                string strLoopID = "", strPreLoopID = "";
                DataTable Warehousedt = (DataTable)Session["Product_StockList"];

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

                Session["Product_StockList"] = Warehousedt;
                GrdWarehouse.DataSource = Warehousedt.DefaultView;
                GrdWarehouse.DataBind();
                WarehousePanel.JSProperties["cpIsShow"] = "Y";
            }
            else if (strSplitCommand == "EditWarehouse")
            {
                string strKey = e.Parameter.Split('~')[1];

                if (Session["Product_StockList"] != null)
                {
                    DataTable Warehousedt = (DataTable)Session["Product_StockList"];

                    string strWarehouse = "", strBatchID = "", strSrlID = "", strQuantity = "0", MfgDate = "", ExpiryDate = "";
                    var rows = Warehousedt.Select(string.Format("SrlNo ='{0}'", strKey));
                    foreach (var dr in rows)
                    {
                        strWarehouse = (Convert.ToString(dr["WarehouseID"]) != "") ? Convert.ToString(dr["WarehouseID"]) : "0";
                        strBatchID = (Convert.ToString(dr["BatchID"]) != "") ? Convert.ToString(dr["BatchID"]) : "";
                        MfgDate = (Convert.ToString(dr["MfgDate"]) != "") ? Convert.ToString(dr["MfgDate"]) : "";
                        ExpiryDate = (Convert.ToString(dr["ExpiryDate"]) != "") ? Convert.ToString(dr["ExpiryDate"]) : "";
                        strSrlID = (Convert.ToString(dr["SerialNo"]) != "") ? Convert.ToString(dr["SerialNo"]) : "";
                        strQuantity = (Convert.ToString(dr["Quantity"]) != "") ? Convert.ToString(dr["Quantity"]) : "0";
                    }

                    #region Bind WarehouseList with Default Warehouse

                    DataTable dt = GetWarehouseData();
                    string strBranch = Convert.ToString(ddl_Branch.SelectedValue);

                    CmbWarehouseID.Items.Clear();
                    CmbWarehouseID.DataSource = dt;
                    CmbWarehouseID.DataBind();

                    #endregion

                    CmbWarehouseID.Value = strWarehouse;
                    txtQuantity.Value = strQuantity;
                    txtBatchName.Value = strBatchID;
                    txtserialID.Value = strSrlID;
                    if (MfgDate != "") txtStartDate.Value = Convert.ToDateTime(DateFormatting(MfgDate));
                    if (ExpiryDate != "") txtEndDate.Value = Convert.ToDateTime(DateFormatting(ExpiryDate));
                    WarehousePanel.JSProperties["cpIsShow"] = "Y";
                }
            }
            else if (strSplitCommand == "WarehouseDelete")
            {
                string ProductID = Convert.ToString(hdfProductSrlNo.Value);
                DeleteUnsaveWarehouse(ProductID);
            }

            if (strIsViewMode == "Y")
            {
                #region Bind WarehouseList with Default Warehouse

                DataTable dt = GetWarehouseData();
                string strBranch = Convert.ToString(ddl_Branch.SelectedValue);

                CmbWarehouseID.Items.Clear();
                CmbWarehouseID.DataSource = dt;
                CmbWarehouseID.DataBind();

                // BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine();

                DataTable dtdefaultStock = objEngine.GetDataTable("tbl_master_building", " top 1 bui_id ", " bui_BranchId='" + strBranch + "' ");
                if (dtdefaultStock != null && dtdefaultStock.Rows.Count > 0)
                {
                    string defaultID = Convert.ToString(dtdefaultStock.Rows[0]["bui_id"]).Trim();

                    if (defaultID != null || defaultID != "" || defaultID != "0")
                    {
                        CmbWarehouseID.Value = defaultID;
                    }
                }

                #endregion

                #region Bind Stock Record of the Product

                DataTable Warehousedt = (DataTable)Session["Product_StockList"];

                if (Warehousedt != null && Warehousedt.Rows.Count > 0)
                {
                    Warehousedt.DefaultView.Sort = "LoopID,SrlNo ASC";
                    DataTable sortWarehousedt = Warehousedt.DefaultView.ToTable();

                    DataView dvData = new DataView(sortWarehousedt);
                    dvData.RowFilter = "Product_SrlNo = '" + ProductSrlNo + "'";

                    GrdWarehouse.DataSource = dvData;
                    GrdWarehouse.DataBind();
                }
                else
                {
                    GrdWarehouse.DataSource = Warehousedt.DefaultView;
                    GrdWarehouse.DataBind();
                }

                #endregion

                txtBatchName.Text = "";
                txtQuantity.Text = "0";
                txtserialID.Text = "";
                txtStartDate.Value = null;
                txtEndDate.Value = null;

                WarehousePanel.JSProperties["cpIsShow"] = "Y";
            }
        }
        public void DeleteUnsaveWarehouse(string SrlNo)
        {
            DataTable Warehousedt = new DataTable();
            if (Session["Product_StockList"] != null)
            {
                Warehousedt = (DataTable)Session["Product_StockList"];

                var rows = Warehousedt.Select("Product_SrlNo ='" + SrlNo + "' AND Status='D'");
                foreach (var row in rows)
                {
                    row.Delete();
                }
                Warehousedt.AcceptChanges();

                Session["Product_StockList"] = Warehousedt;
            }
        }
        protected void GrdWarehouse_DataBinding(object sender, EventArgs e)
        {
            if (Session["Product_StockList"] != null)
            {
                #region Bind Stock Record of the Product

                string ProductSrlNo = Convert.ToString(hdfProductSrlNo.Value);
                DataTable Warehousedt = (DataTable)Session["Product_StockList"];

                if (Warehousedt != null && Warehousedt.Rows.Count > 0)
                {
                    Warehousedt.DefaultView.Sort = "LoopID,SrlNo ASC";
                    DataTable sortWarehousedt = Warehousedt.DefaultView.ToTable();

                    DataView dvData = new DataView(sortWarehousedt);
                    dvData.RowFilter = "Product_SrlNo = '" + ProductSrlNo + "'";

                    GrdWarehouse.DataSource = dvData;
                }
                else
                {
                    GrdWarehouse.DataSource = Warehousedt.DefaultView;
                }

                #endregion
            }
        }
        public DataTable GetWarehouseData()
        {
            string strBranch = Convert.ToString(ddl_Branch.SelectedValue);

            DataTable dt = new DataTable();
            //dt = oDBEngine.GetDataTable("select  bui_id as WarehouseID,bui_Name as WarehouseName from tbl_master_building Where IsNull(bui_BranchId,0) in ('0','" + strBranch + "') order by bui_Name");

            MasterSettings masterBl = new MasterSettings();
            string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));

            if (multiwarehouse != "1")
            {
                dt = oDBEngine.GetDataTable("select  tmb.bui_id as WarehouseID,bui_Name as WarehouseName from tbl_master_building tmb inner join Master_Warehouse_Branchmap mwb on tmb.bui_id=mwb.Bui_id  Where isnull(mwb.Branch_id,0) in ('" + strBranch + "') order by bui_Name");
            }
            else
            {
                dt = oDBEngine.GetDataTable("EXEC [GET_BRANCHWISEWAREHOUSE] '1','" + strBranch + "'");
            }




            return dt;
        }
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
            string ProductID = Convert.ToString(hdfProductID.Value);
            string BranchID = Convert.ToString(ddl_Branch.SelectedValue);

            DataTable SerialCount = CheckDuplicateSerial(SerialNo, ProductID, BranchID, "PurchaseChallan");
            return SerialCount.Rows.Count;
        }
        public DataTable CheckDuplicateSerial(string SerialNo, string ProductID, string BranchID, string Action)
        {
            try
            {
                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("proc_CheckDuplicateProduct");
                proc.AddVarcharPara("@Action", 500, Action);
                proc.AddVarcharPara("@SerialNo", 500, SerialNo);
                proc.AddVarcharPara("@ProductID", 500, ProductID);
                proc.AddVarcharPara("@BranchID", 500, BranchID);
                proc.AddVarcharPara("@DocID", 500, Convert.ToString(Session["PurchaseChallan_Id"]));
                dt = proc.GetTable();

                return dt;
            }
            catch
            {
                return null;
            }
        }
        public string GetWarehouseMaxLoopValue(DataTable dt)
        {
            string maxValue = "1";
            if (dt != null && dt.Rows.Count > 0)
            {
                List<int> myList = new List<int>();
                foreach (DataRow rrow in dt.Rows)
                {
                    string value = (Convert.ToString(rrow["LoopID"]) != "") ? Convert.ToString(rrow["LoopID"]) : "0";
                    myList.Add(Convert.ToInt32(value));
                }

                maxValue = Convert.ToString(Convert.ToInt32(myList.Max()) + 1);
            }

            return maxValue;
        }
        public string DateFormatting(string Date)
        {
            string Day = Date.Substring(0, 2);
            string Month = Date.Substring(3, 2);
            string Year = Date.Substring(6, 4);

            string returnDate = Year + "-" + Month + "-" + Day;
            return returnDate;
        }
        public DataTable GetDuplicateDetails(string GRNID, DataTable Productlist, DataTable Stockdt, string Action)
        {
            try
            {
                DataSet dsInst = new DataSet();


                // SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));


                SqlCommand cmd = new SqlCommand("proc_CheckDuplicateSerial", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@PurchaseChallan_ID", GRNID);
                cmd.Parameters.AddWithValue("@ProductDetails", Productlist);
                cmd.Parameters.AddWithValue("@StockDetail", Stockdt);

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                DataTable dt = dsInst.Tables[0];

                return dt;
            }
            catch
            {
                return null;
            }
        }
        public static void DeleteWarehouse(string SrlNo)
        {
            DataTable Warehousedt = new DataTable();
            if (HttpContext.Current.Session["Product_StockList"] != null)
            {
                Warehousedt = (DataTable)HttpContext.Current.Session["Product_StockList"];

                var rows = Warehousedt.Select(string.Format("Product_SrlNo ='{0}'", SrlNo));
                foreach (var row in rows)
                {
                    row.Delete();
                }
                Warehousedt.AcceptChanges();

                HttpContext.Current.Session["Product_StockList"] = Warehousedt;
            }
        }
        public object GetStock(DataTable dt)
        {

            List<ProductStockDetailsPC> ProductList = new List<ProductStockDetailsPC>();

            ProductList = (from DataRow dr in dt.Rows
                           select new ProductStockDetailsPC()
                           {
                               Product_SrlNo = Convert.ToString(dr["Product_SrlNo"]),
                               SrlNo = Convert.ToString(dr["SrlNo"]),
                               WarehouseID = Convert.ToString(dr["WarehouseID"]),
                               WarehouseName = Convert.ToString(dr["WarehouseName"]),
                               Quantity = Convert.ToString(dr["Quantity"]),
                               SalesQuantity = Convert.ToString(dr["SalesQuantity"]),
                               Batch = Convert.ToString(dr["Batch"]),
                               MfgDate = Convert.ToString(dr["MfgDate"]),
                               ExpiryDate = Convert.ToString(dr["ExpiryDate"]),
                               SerialNo = Convert.ToString(dr["SerialNo"]),
                               Barcode = Convert.ToString(dr["Barcode"]),
                               ViewBatch = Convert.ToString(dr["ViewBatch"]),
                               ViewMfgDate = Convert.ToString(dr["ViewMfgDate"]),
                               ViewExpiryDate = Convert.ToString(dr["ViewExpiryDate"]),
                               IsOutStatus = Convert.ToString(dr["IsOutStatus"]),
                               IsOutStatusMsg = Convert.ToString(dr["IsOutStatusMsg"]),
                               LoopID = Convert.ToString(dr["LoopID"]),
                               Status = Convert.ToString(dr["Status"]),
                               AltQty = Convert.ToString(dr["AltQty"]),
                               AltUOM = Convert.ToString(dr["AltUOM"]),
                               //rev 1.0
                               AltUOMName= Convert.ToString(dr["AltUOMName"])
                               //rev 1.0 end
                           }).ToList();

            return ProductList;
        }
        public void BindWarehouse()
        {
            DataTable dt = GetWarehouseData();
            string strBranch = Convert.ToString(ddl_Branch.SelectedValue);

            ddlWarehouse.Items.Clear();
            ddlWarehouse.DataSource = dt;
            ddlWarehouse.DataValueField = "WarehouseID";
            ddlWarehouse.DataTextField = "WarehouseName";
            ddlWarehouse.DataBind();

            //BusinessLogicLayer.DBEngine objEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            //DataTable dtdefaultStock = objEngine.GetDataTable("tbl_master_building", " top 1 bui_id ", " bui_BranchId='" + strBranch + "' ");
            //if (dtdefaultStock != null && dtdefaultStock.Rows.Count > 0)
            //{
            //    string defaultID = Convert.ToString(dtdefaultStock.Rows[0]["bui_id"]).Trim();

            //    if (defaultID != null || defaultID != "" || defaultID != "0")
            //    {
            //        hdndefaultWarehouse.Value = defaultID;
            //    }
            //}
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

        [WebMethod]
        public static object GetSecondUOMDetails(string ProductID, string warehouseid, string docid)
        {
            SecondUOMDetailsBL uomBL = new SecondUOMDetailsBL();
            List<SecondUOMDetails> finalResult = uomBL.GetSencondUOMDetails(ProductID, null, "PC", "IN", warehouseid, docid);
            return finalResult;

        }

        //Rev Tanmoy 26-09-2019
        protected void ProjectServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Proj_Id";
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            string branch = ddl_Branch.SelectedValue;
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);

            var q = from d in dc.V_ProjectLists
                    where d.ProjectStatus == "Approved" && d.ProjBracnchid == Convert.ToInt32(branch)
                    orderby d.Proj_Id descending
                    select d;

            e.QueryableSource = q;
        }
        //End Rev
        [WebMethod]
        public static object GetWareHousePackingQuantity(Int64 ProductID)
        {
            List<WareHouseUomConversion> RateLists = new List<WareHouseUomConversion>();

            decimal packing_quantity = 0;
            decimal sProduct_quantity = 0;
            bool isOverideConvertion = false;
            ProcedureExecute proc = new ProcedureExecute("Prc_GetProductForStock");
            proc.AddVarcharPara("@Action", 500, "ChallanPackingQuantityDetails");
            //proc.AddIntegerPara("@UomId", UomId);
            //proc.AddIntegerPara("@AltUomId", AltUomId);
            proc.AddBigIntegerPara("@PackingProductId", ProductID);
            DataTable dt = proc.GetTable();
            RateLists = DbHelpers.ToModelList<WareHouseUomConversion>(dt);
            if (dt != null && dt.Rows.Count > 0)
            {
                packing_quantity = Convert.ToDecimal(dt.Rows[0]["packing_quantity"]);
                sProduct_quantity = Convert.ToDecimal(dt.Rows[0]["sProduct_quantity"]);
                isOverideConvertion = Convert.ToBoolean(dt.Rows[0]["isOverideConvertion"]);
            }
            //return packing_quantity + '~' + sProduct_quantity;
            return RateLists;
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
        public static object DocWiseSimilarProjectCheck(string quote_Id, string Doctype)
        {
            string returnValue = "0";
            if (HttpContext.Current.Session["userid"] != null)
            {
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                //quote_Id = quote_Id.Replace("'", "''");

                DataTable dtMainAccount = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc__Project_PurchaseOrderDetailsList");
                proc.AddVarcharPara("@Action", 100, "GetSimilarProjectCheckforProjPurcOrderfromChallan");
                proc.AddVarcharPara("@TagDocType", 100, Doctype);
                proc.AddVarcharPara("@SelectedComponentList", 500, quote_Id);
                proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
                proc.RunActionQuery();
                returnValue = Convert.ToString(proc.GetParaValue("@ReturnValue"));

            }
            return returnValue;

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

        [WebMethod]
        public static object GetEntityType(string Id)
        {
            string output = "O";
            DataTable dtEntity = new DataTable();
            DBEngine obj = new DBEngine();

            dtEntity = obj.GetDataTable("select ISNULL(CNT_TAX_ENTITYTYPE,'O') from tbl_master_contact where cnt_internalId='" + Convert.ToString(Id) + "'");

            if (dtEntity != null && dtEntity.Rows.Count > 0)
            {
                output = Convert.ToString(dtEntity.Rows[0][0]);
            }

            return output;

        }


        [WebMethod]
        public static object SetProjectCode(string OrderId, string TagDocType)
        {
            List<DocumentDetails> Detail = new List<DocumentDetails>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                ProcedureExecute proc = new ProcedureExecute("prc_PurchaseInvoiceDetail");
                proc.AddVarcharPara("@Action", 500, "PurchaseChallantaggingProjectdata");
                proc.AddVarcharPara("@OrderId", 100, OrderId);
                proc.AddVarcharPara("@TagDocType", 500, TagDocType);
                DataTable address = proc.GetTable();



                if (address != null && address.Rows.Count > 0)
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

        //REV 6.0
       
        [WebMethod]
        public static string FetchBatchWiseMfgDateExpiryDate(string BatchNo, string WarehouseID, string ProductID)
        {
            DataTable dt = new DataTable();            
            BusinessLogicLayer.GenericMethod oGeneric = new BusinessLogicLayer.GenericMethod();
            string Batch_MfgDate="", Batch_ExpiryDate="";
            if (BatchNo != "" && Convert.ToString(BatchNo).Trim() != "")
            {
                ProcedureExecute proc;
                try
                {
                    using (proc = new ProcedureExecute("proc_Fetch_PruchaseChallanDetails"))
                    {
                        proc.AddVarcharPara("@action", 50, "FetchBatchWiseMfgDateExpiryDate");
                        proc.AddVarcharPara("@BatchNo", 200, BatchNo);
                        proc.AddBigIntegerPara("@WarehouseID", Convert.ToInt32(WarehouseID));
                        proc.AddBigIntegerPara("@ProductID", Convert.ToInt32(ProductID));
                        dt = proc.GetTable();
                        if (dt.Rows.Count > 0)
                        {
                            Batch_MfgDate = Convert.ToString(dt.Rows[0]["Batch_MfgDate"]);
                            Batch_ExpiryDate = Convert.ToString(dt.Rows[0]["Batch_ExpiryDate"]);
                            return Batch_MfgDate+"~"+ Batch_ExpiryDate;
                        }

                    }
                }

                catch (Exception ex)
                {
                    return Batch_MfgDate + "~" + Batch_ExpiryDate;
                }

                finally
                {
                    proc = null;
                }
            }
            return Batch_MfgDate + "~" + Batch_ExpiryDate;
        }
        //REV 6.0 End

    }

    public class WareHouseUomConversion
    {
        public decimal packing_quantity { get; set; }
        public decimal sProduct_quantity { get; set; }

        public Int32 AltUOMId { get; set; }
        public bool isOverideConvertion { get; set; }
    }
    public class MultiUOMPacking
    {
        public decimal packing_quantity { get; set; }
        public decimal sProduct_quantity { get; set; }

        public Int32 AltUOMId { get; set; }
    }

    public class ProductTaxDetails
    {
        public string SrlNo { get; set; }
        public string IsTaxEntry { get; set; }
    }
    public class ProductStockDetailsPC
    {
        public string Product_SrlNo { get; set; }
        public string SrlNo { get; set; }
        public string WarehouseID { get; set; }
        public string WarehouseName { get; set; }
        public string Quantity { get; set; }
        public string SalesQuantity { get; set; }
        public string Batch { get; set; }
        public string MfgDate { get; set; }
        public string ExpiryDate { get; set; }
        public string Rate { get; set; }
        public string SerialNo { get; set; }
        public string Barcode { get; set; }
        public string ViewBatch { get; set; }
        public string ViewMfgDate { get; set; }
        public string ViewExpiryDate { get; set; }
        public string ViewRate { get; set; }
        public string IsOutStatus { get; set; }
        public string IsOutStatusMsg { get; set; }
        public string LoopID { get; set; }
        public string Status { get; set; }
        public string AltQty { get; set; }
        public string AltUOM { get; set; }

        //Rev 1.0
        public string AltUOMName { get; set; }
        //Rev 1.0 End
    }

    public class ProductStockDetails
    {
        public string Product_SrlNo { get; set; }
        public string SrlNo { get; set; }
        public string WarehouseID { get; set; }
        public string WarehouseName { get; set; }
        public string Quantity { get; set; }
        public string SalesQuantity { get; set; }
        public string Batch { get; set; }
        public string MfgDate { get; set; }
        public string ExpiryDate { get; set; }
        public string Rate { get; set; }
        public string SerialNo { get; set; }
        public string Barcode { get; set; }
        public string ViewBatch { get; set; }
        public string ViewMfgDate { get; set; }
        public string ViewExpiryDate { get; set; }
        public string ViewRate { get; set; }
        public string IsOutStatus { get; set; }
        public string IsOutStatusMsg { get; set; }
        public string LoopID { get; set; }
        public string Status { get; set; }
        public string Value { get; set; }
        public string AltQuantity { get; set; }
        public string UOM { get; set; }
        public string AltUOM { get; set; }
        public string AlterQty { get; set; }

        // Mantis Issue 24429
        public string Order_AltQuantity { get; set; }
        public string Order_AltUOM { get; set; }
        // End of Mantis Issue 24429

    }
    public class PurchaseOrderDetails
    {

        public string Type { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public string StateCode { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public int PinId { get; set; }
        public string PinCode { get; set; }
        public string GSTIN { get; set; }
        public string Landmark { get; set; }
        public string PosForGst { get; set; }
        public string ShipToPartyId { get; set; }
        public string ShipToPartyName { get; set; }


    }



}