using System;
using System.Data;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
using System.Configuration;
using BusinessLogicLayer;
using EntityLayer.CommonELS;


namespace ERP.OMS.Management.Activities
{
    public partial class EntriesDocuments : ERP.OMS.ViewState_class.VSPage
    {
       // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
         BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        
        
        public string pageAccess = "";
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //sas
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                #region PageHeader
                string  cntypename=Convert.ToString(Request.QueryString["type"]);

                if (cntypename == "SalesOrder")
                {
                    lblheader.Text = "Document of Sales Order";
                    lnkListPage.HRef = "/OMS/management/Activities/SalesOrderEntityList.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/SalesOrderEntityList.aspx");
                }
                if (cntypename == "SINQ")
                {
                    lblheader.Text = "Document of Sales Inquiry";
                    lnkListPage.HRef = "/OMS/management/Activities/SalesInquiry.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/SalesInquiry.aspx");
                }
                if (cntypename == "PurQuote")
                {
                    lblheader.Text = "Document of Purchase Quotation";
                    lnkListPage.HRef = "/OMS/management/Activities/PurchaseQuotationList.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/PurchaseQuotationList.aspx");
                }
                if (cntypename == "ProjPurQuote")
                {
                    lblheader.Text = "Document of Project Purchase Quotation";
                    lnkListPage.HRef = "/OMS/management/Activities/ProjectPurchaseQuotationList.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/ProjectPurchaseQuotationList.aspx");
                }
                if (cntypename == "ProSINQ")
                {
                    lblheader.Text = "Document of Project Sales Inquiry";
                    lnkListPage.HRef = "/OMS/management/Activities/ProjectInquiry.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/ProjectInquiry.aspx");
                }
                #region Code Added By Sam on 21062017 Section Start
                else if (cntypename == "TransitPurchaseInvoice")
                {
                    lblheader.Text = "Document of Sales Invoice";
                    lnkListPage.HRef = "/OMS/management/Activities/TPurchaseInvoiceList.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/TPurchaseInvoiceList.aspx");

                }
                #endregion Code Added By Sam on 21062017 Section End
                else if (cntypename == "SalesInvoice")
                {
                    lblheader.Text = "Document of Sales Invoice";
                    lnkListPage.HRef = "/OMS/management/Activities/SalesInvoiceList.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/SalesInvoiceList.aspx");
                }
                else if (cntypename == "InvDelvChallan")
                {
                    lblheader.Text = "Document of Sales Invoice Cum Challan";
                    lnkListPage.HRef = "/OMS/management/Activities/InvoiceDeliveryChallanList.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/InvoiceDeliveryChallanList.aspx");
                }
                else if (cntypename == "ProjectSOrder")
                {
                    lblheader.Text = "Document of Project Sales Order";
                    lnkListPage.HRef = "/OMS/management/Activities/projectOrderList.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/projectOrderList.aspx");
                }
                else if (cntypename == "TransitSalesInvoice")
                {
                    lblheader.Text = "Document of Transit Sales Invoice";
                    lnkListPage.HRef = "/OMS/management/Activities/TSalesInvoiceList.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/TSalesInvoiceList.aspx");
                }
                    
                else if (cntypename == "SaleReturn")
                {
                    lblheader.Text = "Document of Sales Return";
                    lnkListPage.HRef = "/OMS/management/Activities/SalesReturnList.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/SalesReturnList.aspx");
                }
                else if (cntypename == "NormalSaleReturn")
                {
                    lblheader.Text = "Document of Normal Sale Return";
                    lnkListPage.HRef = "/OMS/management/Activities/ReturnNormalList.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/ReturnNormalList.aspx");
                }
                else if (cntypename == "ManualSaleReturn")
                {
                    lblheader.Text = "Document of Manual Sale Return";
                    lnkListPage.HRef = "/OMS/management/Activities/ReturnManualList.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Activities/PurchaseReturnManualList.aspx");
                }
                else if (cntypename == "CustomerReturn")
                {
                    lblheader.Text = "Document of Customer Return";
                    lnkListPage.HRef = "/OMS/management/Activities/CustomerReturnList.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/CustomerReturnList.aspx");
                }
                else if (cntypename == "IssueToReturn")
                {
                    lblheader.Text = "Document of Issue To Customer";
                    lnkListPage.HRef = "/OMS/management/Activities/IssueToCustomerReturnList.aspx";
                }
                else if (Convert.ToString(Request.QueryString["type"]) == "Quotation")
                {
                    lblheader.Text = "Document of Quotation";
                    lnkListPage.HRef = "/OMS/management/Activities/SalesQuotationList.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/SalesQuotationList.aspx");
                }

                else if (cntypename == "ProjectQuotation")
                {
                    lblheader.Text = "Document of Project Quotation/Proposal";
                    lnkListPage.HRef = "/OMS/management/Activities/ProjectQuotationList.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/ProjectQuotationList.aspx");
                }
                else if (Convert.ToString(Request.QueryString["type"]) == "PurchaseReturn")
                {
                    lblheader.Text = "Document of Purchase Return";
                    lnkListPage.HRef = "/OMS/management/Activities/PReturnList.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Activities/PReturnList.aspx");
                }
                else if (Convert.ToString(Request.QueryString["type"]) == "ReturnAgainstGRN")
                {
                    lblheader.Text = "Document of Return Against GRN";
                    lnkListPage.HRef = "/OMS/management/Activities/PurchaseReturnIssueList.aspx";
                }
                else if (Convert.ToString(Request.QueryString["type"]) == "PurchaseInvoice")
                {
                    lblheader.Text = "Document of Purchase Invoice";
                    lnkListPage.HRef = "/OMS/management/Activities/PurchaseInvoiceList.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/PurchaseInvoiceList.aspx");
                }
                else if (Convert.ToString(Request.QueryString["type"]) == "PC")
                {
                    lblheader.Text = "Document of Purchase Challan";
                    lnkListPage.HRef = "/OMS/management/Activities/PurchaseChallanList.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/PurchaseChallanList.aspx");
                }
                else if (Convert.ToString(Request.QueryString["type"]) == "SalesChallan")
                {
                    lblheader.Text = "Document of Sales Challan";
                    lnkListPage.HRef = "/OMS/management/Activities/SalesChallanEntityList.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/SalesChallanEntityList.aspx");
                }
                else if (Convert.ToString(Request.QueryString["type"]) == "BTOUT")
                {
                    lblheader.Text = "Document of Branch Transfer Out";
                    lnkListPage.HRef = "/OMS/management/Activities/BranchTransferOutList.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/BranchTransferOutLEntityList.aspx");

                }
                else if (Convert.ToString(Request.QueryString["type"]) == "BTIN")
                {
                    lblheader.Text = "Document of Branch Transfer In";
                    lnkListPage.HRef = "/OMS/management/Activities/BranchTransferInList.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/BranchTransferInListEntity.aspx");
                }
              
                else if (Convert.ToString(Request.QueryString["type"]) == "UndeliveryReturn")
                {
                    lblheader.Text = "Document of Undelivery Return";
                    lnkListPage.HRef = "/OMS/management/Activities/UndeliveryReturnList.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/UndeliveryReturnList.aspx");
                }

                else if (Convert.ToString(Request.QueryString["type"]) == "PurchaseChallan")
                {
                    lblheader.Text = "Document of Purchase GRN";
                    lnkListPage.HRef = "/OMS/management/Activities/PurchaseChallanList.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/PurchaseChallanList.aspx");
                }

                else if (Convert.ToString(Request.QueryString["type"]) == "ProjectPurchaseChallan")
                {
                    lblheader.Text = "Document of Project Purchase GRN";
                    lnkListPage.HRef = "/OMS/management/Activities/ProjectPurchaseChallanList.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/ProjectPurchaseChallanList.aspx");
                }

                else if (Convert.ToString(Request.QueryString["type"]) == "PurchaseReturnManual")
                {
                    lblheader.Text = "Document of Purchase Return Manual";
                    lnkListPage.HRef = "/OMS/management/Activities/PurchaseReturnManualList.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Activities/PurchaseReturnManualList.aspx");
                }
                else if (Convert.ToString(Request.QueryString["type"]) == "RateDifferenceEntryVendor")
                {
                    lblheader.Text = "Document of Rate Difference Entry Vendor";
                    lnkListPage.HRef = "/OMS/management/Activities/RateDifferenceEntryVendorList.aspx";
                }
                else if (Convert.ToString(Request.QueryString["type"]) == "RateDifferenceEntryCustomer")
                {
                    lblheader.Text = "Document of Rate Difference Entry Customer";
                    lnkListPage.HRef = "/OMS/management/Activities/RateDifferenceEntryCustomerList.aspx";
                }
                else if (Convert.ToString(Request.QueryString["type"]) == "ProjectInvoice")
                {
                    lblheader.Text = "Project Sales Invoice";
                    lnkListPage.HRef = "/OMS/management/Activities/ProjectInvoiceList.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Activities/ProjectInvoiceList.aspx");
                }
                else if (Convert.ToString(Request.QueryString["type"]) == "MaterialIssue")
                {
                    lblheader.Text = "Material Issue";
                    lnkListPage.HRef = "/OMS/management/Activities/ProjectIssueMaterialsList.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/ProjectIssueMaterialsList.aspx");
                }
                else if (Convert.ToString(Request.QueryString["type"]) == "SrvDelivery")
                {
                    lblheader.Text = "Delivery";
                    lnkListPage.HRef = "../../../ServiceManagement/Transaction/Delivery/DeliveryList.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Transaction/Delivery/DeliveryList.aspx");
                }
                //Add for Delivery Attachment from Search Module Tanmoy 15-12-2020
                else if (Convert.ToString(Request.QueryString["type"]) == "SrvSearch")
                {
                    lblheader.Text = "Delivery";
                    lnkListPage.HRef = "../../../ServiceManagement/Transaction/search/searchqueries.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Transaction/Delivery/DeliveryList.aspx");
                }
                //Add for Delivery Attachment from Search Module Tanmoy 15-12-2020 End
              


                else if (Convert.ToString(Request.QueryString["type"]) == "ProjectPurchaseOrder")
                {
                    lblheader.Text = "Document of Project Purchase Order";
                    lnkListPage.HRef = "/OMS/management/Activities/ProjectPurchaseOrderList.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/ProjectPurchaseOrderList.aspx");
                }

                else if (Convert.ToString(Request.QueryString["type"]) == "PurchaseOrder")
                {
                    lblheader.Text = "Document of Purchase Order";
                    lnkListPage.HRef = "/OMS/management/Activities/PurchaseOrderList.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/PurchaseOrderList.aspx");
                }
                //Add Tanmoy 12-01-2021
                else if (Convert.ToString(Request.QueryString["type"]) == "STBWalletRecharge")
                {
                    lblheader.Text = "Wallet Recharge";
                    lnkListPage.HRef = "../../../STBManagement/WalletRecharge/index.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/WalletRecharge/index.aspx");
                }
                else if (Convert.ToString(Request.QueryString["type"]) == "STBMoneyReceipt")
                {
                    lblheader.Text = "Money Receipt";
                    lnkListPage.HRef = "../../../STBManagement/MoneyReceipt/index.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/MoneyReceipt/index.aspx");
                }
                //Add Tanmoy 12-01-2021 End
                //Add Tanmoy 18-02-2021
                else if (Convert.ToString(Request.QueryString["type"]) == "StbWalletRechargeSearch")
                {
                    lblheader.Text = "Wallet Recharge";
                    lnkListPage.HRef = "../../../STBManagement/Search/Search.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/WalletRecharge/index.aspx");
                }
                else if (Convert.ToString(Request.QueryString["type"]) == "StbMoneyReceiptSearch")
                {
                    lblheader.Text = "Money Receipt";
                    lnkListPage.HRef = "../../../STBManagement/Search/Search.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/MoneyReceipt/index.aspx");
                }
                else if (Convert.ToString(Request.QueryString["type"]) == "StbRequisition")
                {
                    lblheader.Text = "STB Requisition";
                    lnkListPage.HRef = "../../../STBManagement/Requisition/STBRequisition.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Requisition/STBRequisition.aspx");
                }
                else if (Convert.ToString(Request.QueryString["type"]) == "StbReturnRequisition")
                {
                    lblheader.Text = "Return Requisition";
                    lnkListPage.HRef = "../../../STBManagement/ReturnRequisition/ReturnRequisitionList.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/ReturnRequisition/ReturnRequisitionList.aspx");
                }
                else if (Convert.ToString(Request.QueryString["type"]) == "StbRequisitionSearch")
                {
                    lblheader.Text = "STB Requisition";
                    lnkListPage.HRef = "../../../STBManagement/Requisition/STBRequisition.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Requisition/STBRequisition.aspx");
                }
                else if (Convert.ToString(Request.QueryString["type"]) == "StbReturnRequisitionSearch")
                {
                    lblheader.Text = "Return Requisition";
                    lnkListPage.HRef = "../../../STBManagement/ReturnRequisition/ReturnRequisitionList.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/ReturnRequisition/ReturnRequisitionList.aspx");
                }
                //Add Tanmoy 18-02-2021 End
                //Mantis Issue 25010
                else if (Convert.ToString(Request.QueryString["type"]) == "BranchRequisition")
                {
                    lblheader.Text = "Document of Branch Requisition";
                    lnkListPage.HRef = "/OMS/management/Activities/BranchRequisition.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/BranchRequisition.aspx");
                }
                else if (Convert.ToString(Request.QueryString["type"]) == "WarehousewiseStockTransfer")
                {
                    lblheader.Text = "Document of Warehouse Wise Stock Transfer";
                    lnkListPage.HRef = "/OMS/Management/Activities/WarehousewiseStockTransferList.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Activities/WarehousewiseStockTransferList.aspx");
                }
                else if (Convert.ToString(Request.QueryString["type"]) == "WarehouseWiseStockOUT")
                {
                    lblheader.Text = "Document of Warehouse Wise Stock OUT";
                    lnkListPage.HRef = "/OMS/Management/Activities/WarehousewiseStockJournalListOUT.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Activities/WarehousewiseStockJournalListOUT.aspx");
                }
                else if (Convert.ToString(Request.QueryString["type"]) == "WarehouseWiseStockIN")
                {
                    lblheader.Text = "Document of Warehouse Wise Stock IN";
                    lnkListPage.HRef = "/OMS/Management/Activities/WarehousewiseStockJournalListIN.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Activities/WarehousewiseStockJournalListIN.aspx");
                }
                //End of Mantis Issue 25010
                //Mantis Issue 25065
                else if (Convert.ToString(Request.QueryString["type"]) == "ProjectIndentRequisition")
                {
                    lblheader.Text = "Document of Project Indent Requisition";
                    lnkListPage.HRef = "/OMS/management/Activities/PmsProjectIndent.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Activities/PmsProjectIndent.aspx");
                }
                //End of Mantis Issue 25065
                //Mantis Issue 25125
                else if (Convert.ToString(Request.QueryString["type"]) == "PurchaseIndent")
                {
                    lblheader.Text = "Document of Purchase Indent";
                    lnkListPage.HRef = "/OMS/management/Activities/PurchaseIndent.aspx";
                    rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Activities/PurchaseIndent.aspx");
                }
                //End of Mantis Issue 25125
                #endregion
            }


            if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["type"])))
            {
                Session["requesttype"] = Convert.ToString(Request.QueryString["type"]);

                if (Convert.ToString(Request.QueryString["type"]) == "SrvSearch")
                {
                    Session["requesttype"] = "SrvDelivery";
                }

                if (Convert.ToString(Request.QueryString["type"]) == "StbWalletRechargeSearch")
                {
                    Session["requesttype"] = "STBWalletRecharge";
                }
                if (Convert.ToString(Request.QueryString["type"]) == "StbMoneyReceiptSearch")
                {
                    Session["requesttype"] = "STBMoneyReceipt";
                }

                if (Convert.ToString(Request.QueryString["type"]) == "StbRequisitionSearch")
                {
                    Session["requesttype"] = "StbRequisition";
                }
                if (Convert.ToString(Request.QueryString["type"]) == "StbReturnRequisitionSearch")
                {
                    Session["requesttype"] = "StbReturnRequisition";
                }
            }

            


            Session["KeyVal2"] = null;

          

            if (Session["Name"] != null)
            {
                lblName.Text = Session["Name"].ToString();
            }
            else if (Session["CompanyName"] != null)
            {
                lblName.Text = "Company Name :" + "  " + Session["CompanyName"].ToString();
            }

            BindGrid();

            if (Session["ContactType"] != null || Session["requesttype"] != null)
            {
                string cnttype = Convert.ToString(Session["ContactType"]);
                string reqsttype = Convert.ToString(Session["requesttype"]);

                //if (reqsttype == "SalesOrder")
                //{                    
                    //TabPage page = ASPxPageControl1.TabPages.FindByName("General");
                    //page.Visible = false;
                    //page = ASPxPageControl1.TabPages.FindByName("[B]illing/Shipping");
                    //page.Visible = false;                  
                //}
                //else  if (reqsttype == "SalesInvoice")
                //{
                    //TabPage page = ASPxPageControl1.TabPages.FindByName("General");
                    //page.Visible = false;
                    //page = ASPxPageControl1.TabPages.FindByName("[B]illing/Shipping");
                    //page.Visible = false;                   
                //}

                TabPage page = ASPxPageControl1.TabPages.FindByName("General");
                page.Visible = false;
                page = ASPxPageControl1.TabPages.FindByName("[B]illing/Shipping");
                page.Visible = false;
            }
        }
        public void BindGrid()
        {
            string bldng = "";
            string verify = "";
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataColumn col1 = new DataColumn("Id");
            DataColumn col2 = new DataColumn("Type");
            DataColumn col3 = new DataColumn("FileName");
            DataColumn col4 = new DataColumn("Src");
            DataColumn col5 = new DataColumn("FilePath");
            DataColumn col6 = new DataColumn("ReceiveDate");
            DataColumn col7 = new DataColumn("RenewDate");
            DataColumn col8 = new DataColumn("Bldng");
            DataColumn col9 = new DataColumn("Fileno");
            DataColumn col10 = new DataColumn("vrfy");
            DataColumn col11 = new DataColumn("Note1");
            DataColumn col12 = new DataColumn("Note2");
            DataColumn col13 = new DataColumn("createuser");
            DataColumn col14 = new DataColumn("doc");

            dt.Columns.Add(col1);
            dt.Columns.Add(col2);
            dt.Columns.Add(col3);
            dt.Columns.Add(col4);
            dt.Columns.Add(col5);
            dt.Columns.Add(col6);
            dt.Columns.Add(col7);
            dt.Columns.Add(col8);
            dt.Columns.Add(col9);
            dt.Columns.Add(col10);
            dt.Columns.Add(col11);
            dt.Columns.Add(col12);
            dt.Columns.Add(col13);
            dt.Columns.Add(col14);
           

            if (Request.QueryString["idbldng"] != null)           
            {
                Session["KeyVal_InternalID"] = Request.QueryString["idbldng"];
                dt1 = oDBEngine.GetDataTable("tbl_master_document INNER JOIN tbl_master_documentType ON tbl_master_document.doc_documentTypeId = tbl_master_documentType.dty_id", "tbl_master_document.doc_id AS Id, tbl_master_documentType.dty_documentType AS Type,tbl_master_document.doc_documentName AS FileName, tbl_master_document.doc_source AS Src,doc_buildingid,doc_Floor,doc_RoomNo,doc_CellNo,doc_FileNo,convert(varchar,doc_receivedate,106)as ReceiveDate,convert(varchar,doc_renewdate,106)as RenewDate,(case when doc_verifydatetime is not null then ((select user_loginid from tbl_master_user where user_id=doc_verifyuser)  + '['+ convert(varchar,doc_verifydatetime,106)+']') else 'Not Verified' end) as vrfy,doc_Note1,doc_Note2,(select user_loginid from tbl_master_user where user_id=tbl_master_document.Createuser) as createuser, tbl_master_document.doc_documentTypeId as doc ", "doc_contactId='" + Request.QueryString["idbldng"] + "' and tbl_master_documentType.dty_applicableFor='"+Convert.ToString(Session["requesttype"])+"'");
            }
         


            if (dt1.Rows.Count != 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    if (dt1.Rows[i][4].ToString() == "0")
                    {
                        DataRow RowNew = dt.NewRow();
                        RowNew["Id"] = dt1.Rows[i][0].ToString();
                        RowNew["Type"] = dt1.Rows[i][1].ToString();
                        RowNew["FileName"] = dt1.Rows[i][2].ToString();
                        RowNew["Src"] = dt1.Rows[i][3].ToString();
                        if (dt1.Rows[i][9] != null)
                            RowNew["ReceiveDate"] = dt1.Rows[i][9].ToString();
                        else
                            RowNew["ReceiveDate"] = "";
                        if (dt1.Rows[i][10] != null)
                            RowNew["RenewDate"] = dt1.Rows[i][10].ToString();
                        else
                            RowNew["RenewDate"] = "";

                        string BName = "Floor No : " + dt1.Rows[i][5].ToString() + " " + "/ Room No-" + dt1.Rows[i][6].ToString() + " " + "/ Cabinet No-" + dt1.Rows[i][7].ToString() + " ";
                        RowNew["FilePath"] = BName;
                        RowNew["vrfy"] = dt1.Rows[i]["vrfy"].ToString();
                        RowNew["Fileno"] = dt1.Rows[i][8].ToString();
                        RowNew["bldng"] = "";
                        RowNew["Note1"] = dt1.Rows[i]["doc_Note1"].ToString();
                        RowNew["Note2"] = dt1.Rows[i]["doc_Note2"].ToString();
                        RowNew["createuser"] = dt1.Rows[i]["createuser"].ToString();
                        RowNew["doc"] = dt1.Rows[i]["doc"].ToString();
                        dt.Rows.Add(RowNew);

                    }
                    else
                    {
                        DataRow RowNew = dt.NewRow();
                        RowNew["Id"] = dt1.Rows[i][0].ToString();
                        RowNew["Type"] = dt1.Rows[i][1].ToString();
                        RowNew["FileName"] = dt1.Rows[i][2].ToString();
                        RowNew["Src"] = dt1.Rows[i][3].ToString();

                        if (dt1.Rows[i][9] != null)
                            RowNew["ReceiveDate"] = dt1.Rows[i][9].ToString();
                        else
                            RowNew["ReceiveDate"] = "";
                        if (dt1.Rows[i][10] != null)
                            RowNew["RenewDate"] = dt1.Rows[i][10].ToString();
                        else
                            RowNew["RenewDate"] = "";

                        string BuildingName = "";
                        string[,] bname1 = oDBEngine.GetFieldValue("tbl_master_building", "bui_name", " bui_id='" + dt1.Rows[i][4].ToString() + "'", 1);
                        if (bname1[0, 0] != "n")
                        {
                            BuildingName = bname1[0, 0];
                        }


                        RowNew["vrfy"] = dt1.Rows[i]["vrfy"].ToString();
                        RowNew["bldng"] = BuildingName;
                        string BName = "Floor No : " + dt1.Rows[i][5].ToString() + " " + "/ Room No-" + dt1.Rows[i][6].ToString() + " " + "/ Cabinet No-" + dt1.Rows[i][7].ToString() + " ";
                        RowNew["FilePath"] = BName;
                        RowNew["Fileno"] = dt1.Rows[i][8].ToString();
                        RowNew["Note1"] = dt1.Rows[i]["doc_Note1"].ToString();
                        RowNew["Note2"] = dt1.Rows[i]["doc_Note2"].ToString();
                        RowNew["createuser"] = dt1.Rows[i]["createuser"].ToString();
                        RowNew["doc"] = dt1.Rows[i]["doc"].ToString();
                        dt.Rows.Add(RowNew);
                    }
                }
            }
            EmployeeDocumentGrid.DataSource = dt.DefaultView;
            EmployeeDocumentGrid.DataBind();
        }

        protected void EmployeeDocumentGrid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

            string[] CallVal = e.Parameters.ToString().Split('~');
            if (CallVal[0].ToString() == "Delete")
            {
                oDBEngine.DeleteValue("tbl_master_document", " doc_id='" + CallVal[1].ToString() + "'");
                BindGrid();
            }


        }

        protected void EmployeeDocumentGrid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != DevExpress.Web.GridViewRowType.Data) return;
            int rowindex = e.VisibleIndex;
            string Verify = EmployeeDocumentGrid.GetRowValues(rowindex, "vrfy").ToString();
            string ContactID = e.GetValue("Src").ToString();
            string Rowid = e.GetValue("Id").ToString();
            if (Verify != "Not Verified")
            {
                DataTable dt = oDBEngine.GetDataTable("select doc_verifyremarks from tbl_master_document where doc_id=" + Rowid + "");
                string tooltip = dt.Rows[0][0].ToString();
                e.Row.Cells[0].Style.Add("cursor", "hand");
                e.Row.Cells[0].ToolTip = "View Document!";
                e.Row.Cells[0].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                e.Row.Cells[1].Style.Add("cursor", "hand");
                e.Row.Cells[1].ToolTip = "View Document!";
                e.Row.Cells[1].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                e.Row.Cells[2].Style.Add("cursor", "hand");
                e.Row.Cells[2].ToolTip = "View Document!";
                e.Row.Cells[2].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                e.Row.Cells[3].Style.Add("cursor", "hand");
                e.Row.Cells[3].ToolTip = "View Document!";
                e.Row.Cells[3].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                e.Row.Cells[4].Style.Add("cursor", "hand");
                e.Row.Cells[4].ToolTip = "View Document!";
                e.Row.Cells[4].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                e.Row.Cells[5].Style.Add("cursor", "hand");
                e.Row.Cells[5].ToolTip = "View Document!";
                e.Row.Cells[5].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                e.Row.Cells[6].Style.Add("cursor", "hand");
                e.Row.Cells[6].ToolTip = "View Document!";
                e.Row.Cells[6].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");

                e.Row.Cells[7].Style.Add("cursor", "hand");
                e.Row.Cells[7].ToolTip = "View Document!";
                e.Row.Cells[7].Attributes.Add("onclick", "javascript:OnDocumentView('" + ContactID + "');");


                e.Row.Cells[8].Style.Add("cursor", "hand");
                e.Row.Cells[8].ToolTip = tooltip.ToString();
            }
            if (Verify == "Not Verified")
            {
                DataTable dt = oDBEngine.GetDataTable("select doc_verifyremarks from tbl_master_document where doc_id=" + Rowid + "");
                string tooltip = dt.Rows[0][0].ToString();

                e.Row.Cells[8].Style.Add("cursor", "Pointer");
                e.Row.Cells[8].ToolTip = "Click here to Verify !";
                e.Row.Cells[8].Attributes.Add("onclick", "javascript:Changestatus('" + Rowid + "');");
                e.Row.Cells[8].Style.Add("color", "Red");
            }

        }
        protected void EmployeeDocumentGrid_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {

        }
    }
}