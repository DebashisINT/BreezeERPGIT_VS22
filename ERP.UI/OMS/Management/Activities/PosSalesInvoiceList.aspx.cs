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
using System.IO;
using System.Web.Script.Serialization;
using ERP.Models;
using System.Linq;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using System.Reflection;
using System.Web.Script.Services;

namespace ERP.OMS.Management.Activities
{
    public partial class PosSalesInvoiceList : ERP.OMS.ViewState_class.VSPage
    {
        // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        SalesInvoiceBL objSalesInvoiceBL = new SalesInvoiceBL();
        PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
        DataTable dtPartyTotal = null;
        string PartyTotalBalAmt = "";
        string PartyTotalBalDesc = "";
        SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
        protected void Page_PreInit(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            SqlSchematype.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            //((GridViewDataComboBoxColumn)massBranch.Columns["pos_assignBranch"]).PropertiesComboBox.DataSource = LoadBranch();

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Management/Activities/PosSalesInvoiceList.aspx");


            DataTable dtposTimeEdit = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(50),Lock_Fromdate,110) LockCon_Fromdate,convert(varchar(50),Lock_Todate,110) LockCon_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Edit' and Module_Id=44");
            DataTable dtposTimeDelete = oDBEngine.GetDataTable("SELECT  top 1 convert(varchar(50),Lock_Fromdate,110) LockCon_Fromdate,convert(varchar(50),Lock_Todate,110) LockCon_Todate FROM Trans_LockConfigouration_Details WHERE  Type='Delete' and Module_Id=44");
            if (dtposTimeEdit != null && dtposTimeEdit.Rows.Count > 0)
            {
                hdnLockFromDateedit.Value = Convert.ToString(dtposTimeEdit.Rows[0]["LockCon_Fromdate"]);
                hdnLockToDateedit.Value = Convert.ToString(dtposTimeEdit.Rows[0]["LockCon_Todate"]);
                spnEditLock.Style.Add("Display", "block");
                spnEditLock.InnerText = "Editing Invoice(POS) is not allowed as the Data is freezed from   " + hdnLockFromDateedit.Value + " to " + hdnLockToDateedit.Value + ".";
            }
            if (dtposTimeDelete != null && dtposTimeDelete.Rows.Count > 0)
            {
                spnDeleteLock.Style.Add("Display", "block");
                hdnLockFromDatedelete.Value = Convert.ToString(dtposTimeDelete.Rows[0]["LockCon_Fromdate"]);
                hdnLockToDatedelete.Value = Convert.ToString(dtposTimeDelete.Rows[0]["LockCon_Todate"]);
                spnDeleteLock.InnerText = "Delete Invoice(POS) is not allowed as the Data is freezed from   " + hdnLockFromDatedelete.Value + " to " + hdnLockToDatedelete.Value + ".";
            }

            if (!IsPostBack)
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


                MasterSettings objmasterposprint = new MasterSettings();
                hdnPosDocPrintDesignBasedOnTaxCategory.Value = objmasterposprint.GetSettings("PosDocPrintDesignBasedOnTaxCategory");

                if (Session["LastCompany"] != null && Session["LastCompany"] != null)
                {
                    string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                    string userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                    PopulateBranch(userbranchHierachy, Convert.ToString(Session["userbranchID"]));
                    PopulateBranchByHierchy(userbranchHierachy);
                    SetledgerOutPresentOrNot();
                    #region waitinginvoice
                    DataTable watingInvoice = posSale.GetBasketDetails(userbranchHierachy);
                    waitingInvoiceCount.Value = Convert.ToString(watingInvoice.Rows.Count);
                    lblweatingCount.Text = Convert.ToString(watingInvoice.Rows.Count);
                    watingInvoicegrid.DataSource = watingInvoice;
                    watingInvoicegrid.DataBind();

                    FormDate.Date = DateTime.Now;
                    toDate.Date = DateTime.Now;
                    cmbBranchfilter.Value = Convert.ToString(Session["userbranchID"]);
                    //   GetQuotationListGridData(userbranchHierachy, lastCompany);
                    GrdQuotation.SettingsCookies.CookiesID = "BreeezeErpGridCookiesGrdQuotationPosList";

                    this.Page.ClientScript.RegisterStartupScript(GetType(), "setCookieOnStorage", "<script>addCookiesKeyOnStorage('BreeezeErpGridCookiesGrdQuotationPosList');</script>");
                    loadGridDataYesorNO();
                    #endregion

                    DataTable Schemadt = objSlaesActivitiesBL.GetNumberingSchema(lastCompany, userbranchHierachy, finyear, "12", "Y");
                    if (Schemadt != null && Schemadt.Rows.Count > 0)
                    {
                        ddl_numberingScheme.DataTextField = "SchemaName";
                        ddl_numberingScheme.DataValueField = "Id";
                        ddl_numberingScheme.DataSource = Schemadt;
                        ddl_numberingScheme.DataBind();
                    }
                }




                string strInvoiceID = Convert.ToString(Session["LastCompany"]);
                DataTable ComponentTable = objSalesInvoiceBL.GetVehicle();
                lookup_quotation.GridView.Selection.CancelSelection();
                lookup_quotation.DataSource = ComponentTable;
                lookup_quotation.DataBind();
                Session["SI_ComponentData"] = ComponentTable;
                //Rev Subhra 30-04-2019
                MasterSettings objmaster = new MasterSettings();
                hdnPosDocPrintDesignBasedOnTaxCategory.Value = objmaster.GetSettings("PosDocPrintDesignBasedOnTaxCategory");
                //End of Rev Subhra 30-04-2019


                DataTable DtDefaultSettings = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='DefaultExclusiveTaxtypeInPOS'");
                if (Convert.ToString(DtDefaultSettings.Rows[0][0]) == "Yes")
                {
                    hdnInvoiceType.Value = "PosSalesInvoiceExclusive.aspx";
                }
                else
                {
                    hdnInvoiceType.Value = "PosSalesInvoice.aspx";
                }




            }
        }

        private void loadGridDataYesorNO()
        {
            ProcedureExecute proc = new ProcedureExecute("prc_PosSalesInvoice");
            proc.AddVarcharPara("@Action", 500, "ShowGridOnLoad");
            DataTable dt = proc.GetTable();
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    LoadGridData.Value = "ok";
                }
                else
                {
                    LoadGridData.Value = "no";
                }
            }

        }


        private void PopulateBranchByHierchy(string userbranchhierchy)
        {
            PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
            DataTable branchtable = posSale.getBranchListByHierchy(userbranchhierchy);
            cmbBranchfilter.DataSource = branchtable;
            cmbBranchfilter.ValueField = "branch_id";
            cmbBranchfilter.TextField = "branch_description";
            cmbBranchfilter.DataBind();
            cmbBranchfilter.SelectedIndex = 0;

            DataRow[] name = branchtable.Select("branch_id=" + Convert.ToString(Session["userbranchID"]));
            if (name.Length > 0)
            {
                branchName.Text = Convert.ToString(name[0]["branch_description"]);
            }


        }

        private void PopulateBranch(string userbranchhierchy, string UserBranch)
        {
            PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();

            BranchAssignmentBranch.DataSource = posSale.getBranchListByBranchList(userbranchhierchy, UserBranch);
            BranchAssignmentBranch.ValueField = "branch_id";
            BranchAssignmentBranch.TextField = "branch_description";
            BranchAssignmentBranch.DataBind();
            BranchAssignmentBranch.Value = "0";

            AssignedBranch.DataSource = posSale.getBranchListByBranchList(userbranchhierchy, UserBranch);
            AssignedBranch.ValueField = "branch_id";
            AssignedBranch.TextField = "branch_description";
            AssignedBranch.DataBind();
            AssignedBranch.Value = "0";


        }

        #region Export Grid Section Start
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                //bindexport(Filter);
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
            if (ASPxPageControl1.ActiveTabIndex == 0)
            {
                GrdQuotation.Columns[6].Visible = false;
                string filename = "Sales Invoice";
                exporter.FileName = filename;
                exporter.FileName = "POS";

                exporter.PageHeader.Left = "Sales Invoice";
                exporter.PageFooter.Center = "[Page # of Pages #]";
                exporter.PageFooter.Right = "[Date Printed]";
                exporter.Landscape = true;
            }
            else
            {
                string filename = "Advance Receipt";
                exporter.FileName = filename;
                exporter.FileName = "Receipt";

                exporter.PageHeader.Left = "Advance Receipt";
                exporter.PageFooter.Center = "[Page # of Pages #]";
                exporter.PageFooter.Right = "[Date Printed]";
                exporter.Landscape = true;
                exporter.GridViewID = "CustomerReceiptGrid";
            }
            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }
        }

        #endregion


        public static DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();
            DataTable dataTable = new DataTable();
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, typeof(System.String)));
            }
            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                } dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        [WebMethod]
        public static string GetEditablePermission(string ActiveUser)
        {
            CRMSalesDtlBL objCRMSalesDtlBL = new CRMSalesDtlBL();
            int ispermission = 0;
            ispermission = objCRMSalesDtlBL.QuotationEditablePermission(Convert.ToInt32(ActiveUser));
            return Convert.ToString(ispermission);
        }

        [WebMethod]
        public static string SavePOSMainDoc(string SchemaID, string DocNo, string invoice_id, string Challan_SchemaID, string Challan_Doc)
        {
            string output = "";
            CRMSalesDtlBL objCRMSalesDtlBL = new CRMSalesDtlBL();
            DBEngine oDBEngine = new DBEngine();

            DataTable dt = objCRMSalesDtlBL.GetWarehouseDataTable(invoice_id);


            string BranchId = "";

            bool validation = true;

            if (dt != null)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    string ProductId = Convert.ToString(dr["StockBranchWarehouseDetail_ProductId"]);
                    string warehouseid = Convert.ToString(dr["StockBranchWarehouseDetail_WarehouseId"]);
                    DateTime Date = Convert.ToDateTime(dr["Stock_IN_OUT_DATE"]);
                    DataTable dtAvailableStockCheck = oDBEngine.GetDataTable("Select dbo.fn_GetWarehousewiseStock(" + BranchId + ",'" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "','" + Convert.ToString(HttpContext.Current.Session["LastFinYear"]) + "','" + ProductId + "','" + warehouseid + "','" + Convert.ToDateTime(Date).ToString("yyyy-MM-dd") + "') as branchopenstock");
                    string StockCheck = string.Empty;

                    if (dtAvailableStockCheck.Rows.Count > 0)
                    {
                        StockCheck = Convert.ToString(Math.Round(Convert.ToDecimal(dtAvailableStockCheck.Rows[0]["branchopenstock"]), 4));

                        if (Convert.ToDecimal(dr["StockBranchWarehouse_StockOut"]) > Convert.ToDecimal(StockCheck))
                        {
                            validation = false;
                            break;
                        }
                    }
                
                }
            }

            if (validation)
            {
                if (!string.IsNullOrEmpty(SchemaID) && !string.IsNullOrEmpty(DocNo) && !string.IsNullOrEmpty(invoice_id))
                {

                    output = objCRMSalesDtlBL.SavePOSMainBill(SchemaID, DocNo, invoice_id, Challan_SchemaID, Challan_Doc);
                }
            }
            else
            {
                output = "0" + "Product entered quantity more than stock quantity.Can not proceed.";
            }

            return Convert.ToString(output);
        }



        [WebMethod]
        public static string GetTotalWatingInvoiceCount()
        {
            PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
            string userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
            return Convert.ToString(posSale.GetWaitingCount(userbranchHierachy));
        }

        [WebMethod]
        public static object GetInfluencerDetails(string invid)
        {
            DataSet ds = new DataSet();
            PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
            ds = posSale.GetInfluencerDetails(invid);

            Inf_Header_Details IHD = new Inf_Header_Details();

            INF_Inv_Details inv = new INF_Inv_Details();


            inv.Inv_Id = Convert.ToString(ds.Tables[0].Rows[0]["Inv_Id"]);
            inv.Inv_No = Convert.ToString(ds.Tables[0].Rows[0]["Inv_No"]);
            inv.Amount = Convert.ToString(ds.Tables[0].Rows[0]["Amount"]);
            inv.Inv_BranchId = Convert.ToString(ds.Tables[0].Rows[0]["Inv_BranchId"]);

            IHD.INF_Inv_Details = inv;

            Influencer inf = new Influencer();
            if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
            {
                inf.MainAccount_AccountCode = Convert.ToString(ds.Tables[2].Rows[0]["MainAccount_AccountCode"]);
                inf.CALCULATED_AMOUNT = Convert.ToString(ds.Tables[2].Rows[0]["CALCULATED_AMOUNT"]);
                inf.MAINACCOUNT_DR = Convert.ToString(ds.Tables[2].Rows[0]["MAINACCOUNT_DR"]);
                inf.AMOUNT_DR = Convert.ToString(ds.Tables[2].Rows[0]["AMOUNT_DR"]);
                inf.AUTOJV_ID = Convert.ToString(ds.Tables[2].Rows[0]["AUTOJV_ID"]);
                inf.AUTOJV_NUMBER = Convert.ToString(ds.Tables[2].Rows[0]["AUTOJV_NUMBER"]);
                inf.COMM_AMOUNT = Convert.ToString(ds.Tables[2].Rows[0]["COMM_AMOUNT"]);
                inf.POSTING_DATE = Convert.ToDateTime(ds.Tables[2].Rows[0]["POSTING_DATE"]);
                inf.IsTagged = Convert.ToString(ds.Tables[2].Rows[0]["TaggedCount"]);
                inf.Remarks = Convert.ToString(ds.Tables[2].Rows[0]["REMARKS"]);
            }

            IHD.Influencer = inf;




            IHD.INF_Inv_Products = (from DataRow dr in ds.Tables[1].Rows
                                    select new INF_Inv_Products()
                        {

                            prod_details_id = Convert.ToString(dr["prod_details_id"]),
                            Prod_id = Convert.ToString(dr["Prod_id"]),
                            Prod_description = Convert.ToString(dr["Prod_description"]),
                            prod_Qty = Convert.ToString(dr["prod_Qty"]),
                            prod_Salesprice = Convert.ToString(dr["prod_Salesprice"]),
                            prod_amt = Convert.ToString(dr["prod_amt"]),
                            prod_SalespriceWithGST = Convert.ToString(dr["prod_amtWGST"]),
                            Prod_Percentage = Convert.ToString(dr["Prod_Percentage"]),
                            Applicable_On = Convert.ToString(dr["Applicable_On"]),
                            PROD_COMM_AMOUNT = Convert.ToString(dr["PROD_COMM_AMOUNT"])


                        }).ToList();

            IHD.Influencer_Details = (from DataRow dr in ds.Tables[3].Rows
                                      select new Influencer_Details()
                                    {

                                        DET_AMOUNT_CR = Convert.ToString(dr["DET_AMOUNT_CR"]),
                                        DET_INFLUENCER_ID = Convert.ToString(dr["DET_INFLUENCER_ID"]),
                                        INF_Name = Convert.ToString(dr["INF_Name"]),
                                        DET_MAINACCOUNT_CR = Convert.ToString(dr["DET_MAINACCOUNT_CR"]),
                                        DET_MAINACCOUNT_NAME = Convert.ToString(dr["DET_MAINACCOUNT_NAME"])
                                    }).ToList();



            if (ds.Tables[4] != null && ds.Tables[4].Rows.Count > 0)
            {
                IHD.RemainingBalance = Convert.ToString(ds.Tables[4].Rows[0]["RemainingQty"]);
            }


            return IHD;
        }

        [WebMethod]
        public static object GetInfluencerSchemeDetails(string invid)
        {
            DataSet ds = new DataSet();
            PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
            ds = posSale.GetInfluencerSchemeDetails(invid);

            Inf_Header_Details IHD = new Inf_Header_Details();

            INF_Inv_Details inv = new INF_Inv_Details();


            inv.Inv_Id = Convert.ToString(ds.Tables[0].Rows[0]["Inv_Id"]);
            inv.Inv_No = Convert.ToString(ds.Tables[0].Rows[0]["Inv_No"]);
            inv.Amount = Convert.ToString(ds.Tables[0].Rows[0]["Amount"]);
            inv.Inv_BranchId = Convert.ToString(ds.Tables[0].Rows[0]["Inv_BranchId"]);

            IHD.INF_Inv_Details = inv;

            Influencer inf = new Influencer();
            if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
            {
                inf.MainAccount_AccountCode = Convert.ToString(ds.Tables[2].Rows[0]["MainAccount_AccountCode"]);
                inf.CALCULATED_AMOUNT = Convert.ToString(ds.Tables[2].Rows[0]["CALCULATED_AMOUNT"]);
                inf.MAINACCOUNT_DR = Convert.ToString(ds.Tables[2].Rows[0]["MAINACCOUNT_DR"]);
                inf.AMOUNT_DR = Convert.ToString(ds.Tables[2].Rows[0]["AMOUNT_DR"]);
                inf.AUTOJV_ID = Convert.ToString(ds.Tables[2].Rows[0]["AUTOJV_ID"]);
                inf.AUTOJV_NUMBER = Convert.ToString(ds.Tables[2].Rows[0]["AUTOJV_NUMBER"]);
                inf.COMM_AMOUNT = Convert.ToString(ds.Tables[2].Rows[0]["COMM_AMOUNT"]);
                inf.POSTING_DATE = Convert.ToDateTime(ds.Tables[2].Rows[0]["POSTING_DATE"]);
                inf.IsTagged = Convert.ToString(ds.Tables[2].Rows[0]["TaggedCount"]);
                inf.Remarks = Convert.ToString(ds.Tables[2].Rows[0]["REMARKS"]);
            }

            IHD.Influencer = inf;




            IHD.INF_Inv_Products = (from DataRow dr in ds.Tables[1].Rows
                                    select new INF_Inv_Products()
                                    {

                                        prod_details_id = Convert.ToString(dr["prod_details_id"]),
                                        Prod_id = Convert.ToString(dr["Prod_id"]),
                                        Prod_description = Convert.ToString(dr["Prod_description"]),
                                        prod_Qty = Convert.ToString(dr["prod_Qty"]),
                                        prod_Salesprice = Convert.ToString(dr["prod_Salesprice"]),
                                        prod_amt = Convert.ToString(dr["prod_amt"]),
                                        prod_SalespriceWithGST = Convert.ToString(dr["prod_amtWGST"]),
                                        Prod_Percentage = Convert.ToString(dr["Prod_Percentage"]),
                                        Applicable_On = Convert.ToString(dr["Applicable_On"]),
                                        PROD_COMM_AMOUNT = Convert.ToString(dr["PROD_COMM_AMOUNT"])


                                    }).ToList();

            IHD.Influencer_Details = (from DataRow dr in ds.Tables[3].Rows
                                      select new Influencer_Details()
                                      {

                                          DET_AMOUNT_CR = Convert.ToString(dr["DET_AMOUNT_CR"]),
                                          DET_INFLUENCER_ID = Convert.ToString(dr["DET_INFLUENCER_ID"]),
                                          INF_Name = Convert.ToString(dr["INF_Name"]),
                                          DET_MAINACCOUNT_CR = Convert.ToString(dr["DET_MAINACCOUNT_CR"]),
                                          DET_MAINACCOUNT_NAME = Convert.ToString(dr["DET_MAINACCOUNT_NAME"])
                                      }).ToList();



            if (ds.Tables[4] != null && ds.Tables[4].Rows.Count > 0)
            {
                IHD.RemainingBalance = Convert.ToString(ds.Tables[4].Rows[0]["RemainingQty"]);
            }


            return IHD;
        }

        [WebMethod]
        public static object GetInfluencerReturnDetails(string invid)
        {
            DataSet ds = new DataSet();
            PosSalesInvoiceBl posSale = new PosSalesInvoiceBl();
            ds = posSale.GetInfluencerReturnDetails(invid);

            infulencerReturnAdjustMent IHD = new infulencerReturnAdjustMent();
            //infulencerReturnAdjustMentDetails inv = new infulencerReturnAdjustMentDetails();
            //infulencerReturnAdjustMentDetails ret = new infulencerReturnAdjustMentDetails();

            IHD.Invoice_Data = (from DataRow dr in ds.Tables[0].Rows
                                select new infulencerReturnAdjustMentDetails()
                                      {
                                          DOC_ID = Convert.ToString(dr["INVOICE_ID"]),
                                          CON_ID = Convert.ToString(dr["CON_ID"]),
                                          NAME = Convert.ToString(dr["NAME"]),
                                          DOC_NUMBER = Convert.ToString(dr["INVOICE_NUMBER"]),
                                          DOC_DATE = Convert.ToDateTime(dr["INVOICE_DATE"]).ToString("dd-MM-yyyy"),
                                          Total_Comm = Convert.ToString(dr["Total_Comm"]),
                                          Unpaid = Convert.ToString(dr["Unpaid"])
                                      }).ToList();
            IHD.Return_Data = (from DataRow dr in ds.Tables[1].Rows
                               select new infulencerReturnAdjustMentDetails()
                               {
                                   DOC_ID = Convert.ToString(dr["RETURN_ID"]),
                                   CON_ID = Convert.ToString(dr["CON_ID"]),
                                   NAME = Convert.ToString(dr["NAME"]),
                                   DOC_NUMBER = Convert.ToString(dr["RETURN_NUMBER"]),
                                   DOC_DATE = Convert.ToDateTime(dr["RETURN_DATE"]).ToString("dd-MM-yyyy"),
                                   Total_Comm = Convert.ToString(dr["Total_Comm"]),
                                   Unpaid = Convert.ToString(dr["Unpaid"])
                               }).ToList();



            return IHD;
        }


        [WebMethod]
        public static object SaveInfluencer(infulencerSaveData infsave)
        {
            DataTable Prod = CreateDataTable(infsave.product);
            DataTable Influencer = CreateDataTable(infsave.Influencer);

            InfluencerBL ibl = new InfluencerBL();
            string output = ibl.SaveInfluencerData(infsave, Prod, Influencer, "");

            return output;
        }

        [WebMethod]
        public static object SaveInfluencerScheme(infulencerSaveData infsave)
        {
            DataTable Prod = CreateDataTable(infsave.product);
            DataTable Influencer = CreateDataTable(infsave.Influencer);

            InfluencerBL ibl = new InfluencerBL();
            string output = ibl.SaveInfluencerSchemeData(infsave, Prod, Influencer, "");

            return output;
        }

        [WebMethod]
        public static object GetInfluencerAmount(infulencerSaveData infsave)
        {
            DataTable Prod = CreateDataTable(infsave.product);
            DataTable Influencer = CreateDataTable(infsave.Influencer);

            InfluencerBL ibl = new InfluencerBL();
            string output = ibl.GetInfluencerAmount(infsave, Prod, Influencer, "");

            return output;
        }

        [WebMethod]
        public static object SaveInfluencerAdj(List<INF_ADJ> invoice, List<INF_ADJ> returns)
        {
            DataTable dtInvoice = new DataTable();
            DataTable dtReturn = new DataTable();

            //DataTable dtInvoice = CreateDataTable(invoice);
            //DataTable dtReturn = CreateDataTable(returns);

            dtInvoice.Columns.Add("AMOUNT", typeof(decimal));
            dtInvoice.Columns.Add("DOC_ID", typeof(string));
            dtInvoice.Columns.Add("INF_ID", typeof(string));

            dtReturn.Columns.Add("AMOUNT", typeof(decimal));
            dtReturn.Columns.Add("DOC_ID", typeof(string));
            dtReturn.Columns.Add("INF_ID", typeof(string));


            foreach (INF_ADJ invDet in invoice)
            {
                dtInvoice.Rows.Add(invDet.AMOUNT, invDet.DOC_ID, invDet.INF_ID);
            }
            foreach (INF_ADJ retDet in returns)
            {
                dtReturn.Rows.Add(retDet.AMOUNT, retDet.DOC_ID, retDet.INF_ID);

            }

            InfluencerBL ibl = new InfluencerBL();
            string output = ibl.SaveInfluencerAdjustmentData(dtInvoice, dtReturn);

            return output;
        }

        [WebMethod]
        public static object DeleteInfluencer(string Invoice_Id)
        {

            InfluencerBL ibl = new InfluencerBL();
            string output = ibl.DeleteInfluencerData(Invoice_Id);

            return output;
        }
        [WebMethod]
        public static string VeichleSave(DeleviryDetails Delsave)//, List<GridList> VehicleNo
        {
            // Delsave.Grid = VehicleNo;
            DataTable inv = CreateDataTable(Delsave.Grid);

            PosSalesInvoiceBl ibl = new PosSalesInvoiceBl();
            string output = ibl.SaveVehicleData(Delsave, inv);
            return output;
        }

        [WebMethod]
        public static int CopyInvoiceAndChallanSave(string INvnum, string newINvnum, string CntId, string PostingDate, string RfeNo, string Salesman, string Vehicle, string uniquechallanNo)
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("PRC_CopyInvoiceAndChallan"))
                {
                    proc.AddVarcharPara("@Invoice_Number", 200, INvnum);
                    proc.AddVarcharPara("@New_Invoice_Number", 200, newINvnum);
                    proc.AddVarcharPara("@Customer_Name", 500, CntId);
                    proc.AddVarcharPara("@Posting_Date", 10, PostingDate);
                    proc.AddVarcharPara("@Ref_No", 200, RfeNo);
                    proc.AddVarcharPara("@salesman", 200, Salesman);
                    proc.AddVarcharPara("@vehicle_No", 500, Vehicle);
                    proc.AddVarcharPara("@UniqueChallan", 100, uniquechallanNo);



                    proc.RunActionQuery();

                    return 1;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }


        [WebMethod]

        public static int CheckUniqueinvoiceName(string INvnum)
        {
            DataTable dt = new DataTable();

            int IsPresent = 0;
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

            if (INvnum != "")
            {
                dt = oDBEngine.GetDataTable("select count(Invoice_Number) Invoice_Number from tbl_trans_SalesInvoice where isFromPos=1 and Invoice_Number='" + INvnum + "'");
            }



            if (dt.Rows.Count > 0)
            {
                if (Convert.ToInt32(dt.Rows[0]["Invoice_Number"]) > 0)
                {
                    IsPresent = 1;
                }
            }
            return IsPresent;
        }
        [WebMethod]

        public static int CheckUniqueChallanName(string challanNo)
        {
            DataTable dt = new DataTable();

            int IsPresent = 0;
            //BusinessLogicLayer.GenericMethod oGeneric = new BusinessLogicLayer.GenericMethod();
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

            if (challanNo != "")
            {
                dt = oDBEngine.GetDataTable("select count(Challan_Number) Challan_Number from tbl_trans_SalesChallan where  Challan_Number='" + challanNo + "'");
            }



            if (dt.Rows.Count > 0)
            {
                if (Convert.ToInt32(dt.Rows[0]["Challan_Number"]) > 0)
                {
                    IsPresent = 1;
                }
            }
            return IsPresent;
        }

        [WebMethod]
        public static object viewDelavery(string invoice)
        {
            DeleviryDetails Delsave = new DeleviryDetails();
            HttpContext.Current.Session["Invoices"] = invoice;
            PosSalesInvoiceBl ibl = new PosSalesInvoiceBl();
            DataSet dtdata = new DataSet();
            dtdata = ibl.GetInvoiceDeleviryData(invoice);
            DataTable headTable = dtdata.Tables[0];
            DataTable DetailsTable = dtdata.Tables[1];
            if (DetailsTable != null && DetailsTable.Rows.Count > 0)
            {
                Delsave.CmbOtehrChrgs = Convert.ToString(DetailsTable.Rows[0]["Other_Charges"]);
                Delsave.cmbPaymentTrms = Convert.ToString(DetailsTable.Rows[0]["Payment_Terms"]);
                Delsave.ENo = Convert.ToString(DetailsTable.Rows[0]["Eway_BillNo"]);
                Delsave.EwayValu = Convert.ToString(DetailsTable.Rows[0]["Eway_Value"]);
                if (!string.IsNullOrEmpty(Convert.ToString(DetailsTable.Rows[0]["Eway_Date"])))
                {
                    Delsave.PostingDate = Convert.ToDateTime(DetailsTable.Rows[0]["Eway_Date"]);
                }
                else
                {
                    Delsave.PostingDate = null;
                }
                Delsave.Remarks = Convert.ToString(DetailsTable.Rows[0]["Otehr_Remarks"]);
            }
            GridList list = new GridList();
            if (headTable != null && headTable.Rows.Count > 0)
            {
                // object obj = new object();
                // new PosSalesInvoiceList().ComponentQuotation_Callback(null, null);
                // list.VECHICLE_ID = Convert.ToString(headTable.Rows[0]["VECHICLE_ID"]);
                // list.VECHICLE_NO = Convert.ToString(headTable.Rows[0]["VECHICLE_NO"]);
            }

            //  Delsave.Grid = list;

            return Delsave;
        }


        #region Grid Section Start
        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Invoice_Id";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);




            string IsFilter = Convert.ToString(hfIsFilter.Value);
            string strFromDate = Convert.ToString(hfFromDate.Value);
            string strToDate = Convert.ToString(hfToDate.Value);
            string strBranchID = (Convert.ToString(hfBranchID.Value) == "") ? "0" : Convert.ToString(hfBranchID.Value);

            List<int> branchidlist;
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);
            if (IsFilter == "Y")
            {
                if (strBranchID == "0")
                {
                    string BranchList = Convert.ToString(Session["userbranchHierarchy"]);
                    branchidlist = new List<int>(Array.ConvertAll(BranchList.Split(','), int.Parse));
                    var q = from d in dc.v_posLists
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.Invoice_branchId))
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    branchidlist = new List<int>(Array.ConvertAll(strBranchID.Split(','), int.Parse));
                    var q = from d in dc.v_posLists
                            where d.Invoice_DateTimeFormat >= Convert.ToDateTime(strFromDate) &&
                                  d.Invoice_DateTimeFormat <= Convert.ToDateTime(strToDate) &&
                                  d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"]) &&
                                  branchidlist.Contains(Convert.ToInt32(d.Invoice_branchId))
                            orderby d.Invoice_DateTimeFormat descending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                var q = from d in dc.v_posLists
                        where d.Invoice_branchId == '0' &&
                                d.Invoice_CompanyID == Convert.ToString(Session["LastCompany"])
                        orderby d.Invoice_DateTimeFormat descending
                        select d;
                e.QueryableSource = q;
            }
        }
        protected void GrdQuotation_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            CRMSalesDtlBL objCRMSalesDtlBL = new CRMSalesDtlBL();
            GrdQuotation.JSProperties["cpinsert"] = null;
            GrdQuotation.JSProperties["cpEdit"] = null;
            GrdQuotation.JSProperties["cpUpdate"] = null;
            GrdQuotation.JSProperties["cpDelete"] = null;
            GrdQuotation.JSProperties["cpExists"] = null;
            GrdQuotation.JSProperties["cpUpdateValid"] = null;
            int insertcount = 0;

            int updtcnt = 0;
            int deletecnt = 0;
            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];
            string WhichType = null;
            string QuoteStatus = "";
            string remarks = "";

            if (Convert.ToString(e.Parameters).Contains("~"))
            {
                if (Convert.ToString(e.Parameters).Split('~')[1] != "")
                {
                    WhichType = Convert.ToString(e.Parameters).Split('~')[1];
                }
            }

            if (WhichCall == "Delete")
            {
                deletecnt = posSale.DeleteInvoice(WhichType);
                if (deletecnt == 1)
                {
                    GrdQuotation.JSProperties["cpDelete"] = "Deleted successfully";
                    GetQuotationListGridData(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(HttpContext.Current.Session["LastCompany"]));
                }
                else
                {
                    GrdQuotation.JSProperties["cpDelete"] = "Used in other module.can not delete.";
                }

            }
            else if (WhichCall == "FilterGridByDate")
            {
                string fromdate = e.Parameters.Split('~')[1];
                string toDate = e.Parameters.Split('~')[2];
                string branch = e.Parameters.Split('~')[3];
                DataTable dtdata = new DataTable();
                //dtdata = posSale.GetInvoiceListGridDataByDate(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(HttpContext.Current.Session["LastCompany"]), fromdate, toDate, branch);
                //if (dtdata != null)
                //{
                //    Session["PosSalesInvoiceList"] = dtdata;
                //    GrdQuotation.DataBind();
                //}
            }
            else if (WhichCall == "RefreshGrid")
            {
                //string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                //string userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                //GetQuotationListGridData(userbranchHierachy, lastCompany);
            }
            else if (WhichCall == "CancelAssignment")
            {
                posSale.CancelBranchAssignment(Convert.ToInt32(e.Parameters.Split('~')[1]));
                GrdQuotation.JSProperties["cpCancelAssignMent"] = "yes";
                //string lastCompany = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                //string userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                //GetQuotationListGridData(userbranchHierachy, lastCompany);
            }
        }





        public void GetQuotationListGridData(string userbranch, string lastCompany)
        {
            //DataTable dtdata = new DataTable();
            //dtdata = posSale.GetInvoiceListGridData(userbranch, lastCompany);
            ////dtdata = objSalesInvoiceBL.GetQuotationListGridData(userbranch, lastCompany);
            //if (dtdata != null && dtdata.Rows.Count > 0)
            //{
            //    //GrdQuotation.DataSource = dtdata;
            //    Session["PosSalesInvoiceList"] = dtdata;
            //    GrdQuotation.DataBind();
            //}
            //else
            //{
            //  //  GrdQuotation.DataSource = null;
            //    Session["PosSalesInvoiceList"] = null;
            //    GrdQuotation.DataBind();
            //}

            /*Abhisek
            DataTable dtdata = posSale.GetInvoiceListGridDataByDate(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Convert.ToString(HttpContext.Current.Session["LastCompany"]), FormDate.Date.ToString("yyyy-MM-dd"), toDate.Date.ToString("yyyy-MM-dd"), Convert.ToString(cmbBranchfilter.Value));
            if (dtdata != null)
            {
                Session["PosSalesInvoiceList"] = dtdata;
                GrdQuotation.DataBind();
            }
            */
        }






        #endregion


        protected void watingInvoicegrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string receivedString = e.Parameters;

            if (receivedString.Split('~')[0] == "Remove")
            {
                string key = receivedString.Split('~')[1];
                posSale.DeleteBasketDetailsFromtable(key, Convert.ToInt32(Session["userid"]));
                watingInvoicegrid.JSProperties["cpReturnMsg"] = "Billing Request has been Deleted Successfully.";
                watingInvoicegrid.DataBind();
            }
        }
        protected void watingInvoicegrid_DataBinding(object sender, EventArgs e)
        {
            watingInvoicegrid.DataSource = posSale.GetBasketDetails(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
        }

        protected void SelectPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            //Rev Subhra 05-04-2019
            int internlid = Convert.ToInt32(e.Parameter.Split('~')[1]);
            //End of Rev Subhra 
            if (strSplitCommand == "Bindalldesignes")
            {
                string[] filePaths = new string[] { };
                string DesignPath = "";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\SalesInvoice\DocDesign\SPOS";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\SalesInvoice\DocDesign\SPOS";
                }
                string fullpath = Server.MapPath("~");
                fullpath = fullpath.Replace("ERP.UI\\", "");

                string DesignFullPath = fullpath + DesignPath;


                filePaths = System.IO.Directory.GetFiles(DesignFullPath, "*.repx");
                string invoiceType = Convert.ToString(HdInvoiceType.Value);
                if (invoiceType == "Stock Transfer")
                    invoiceType = "InterstateStockTransfer";

                foreach (string filename in filePaths)
                {
                    string reportname = Path.GetFileNameWithoutExtension(filename);
                    string name = "";
                    //Rev Subhra 05-04-2019
                    if (hdnPosDocPrintDesignBasedOnTaxCategory.Value == "1")
                    {
                        //End of Rev Subhra 05-04-2019
                        if (reportname.Contains(invoiceType))
                        {
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
                    }
                    //Rev Subhra 05-04-2019
                    else
                    {
                        BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
                        DataTable dtCmb = new DataTable();
                        dtCmb = oGenericMethod.GetDataTable("select dbo.fInvoiceGSTType(" + internlid + ")");
                        if (reportname.Split('~')[0] == "GatePass")
                        {
                            CmbDesignName.Items.Add(reportname.Split('~')[0], "GatePass~D");
                        }
                        //Rev Debashis
                        if (reportname.Split('~')[0] == "POS-Cash")
                        {
                            CmbDesignName.Items.Add(reportname.Split('~')[0], "POS-Cash~D");
                        }
                        //End of Rev Debashis
                        if (dtCmb.Rows[0][0].ToString() != "")
                        {
                            if (reportname.Contains(dtCmb.Rows[0][0].ToString()))
                            {
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
                        }
                    }
                    //End Rev
                }
                CmbDesignName.SelectedIndex = 0;
                SelectPanel.JSProperties["cpChecked"] = "";
            }
            else
            {
                string DesignPath = @"Reports\Reports\REPXReports";
                string fullpath = Server.MapPath("~");
                fullpath = fullpath.Replace("ERP.UI\\", "");
                string filename = @"\RepxReportViewer.aspx";
                string DesignFullPath = fullpath + DesignPath + filename;

                string reportName = Convert.ToString(CmbDesignName.Value);
                string NoofCopy = "";
                if (selectOriginal.Checked == true)
                {
                    NoofCopy += 1 + ",";
                }
                if (selectDuplicate.Checked == true)
                {
                    NoofCopy += 2 + ",";
                }
                if (selectFDuplicate.Checked == true)
                {
                    NoofCopy += 3 + ",";
                }
                if (selectTriplicate.Checked == true)
                {
                    NoofCopy += 4 + ",";
                }
                SelectPanel.JSProperties["cpSuccess"] = NoofCopy;
                SelectPanel.JSProperties["cpChecked"] = "Checked";
            }
        }

        #region Assignment
        protected void AssignmentGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {

            if (e.Parameters.Split('~')[0] == "AssignBranch")
            {
                int assignBranch = Convert.ToInt32(e.Parameters.Split('~')[2]);
                int warehouse = Convert.ToInt32(e.Parameters.Split('~')[3]);
                int invoiceid = Convert.ToInt32(e.Parameters.Split('~')[1]);
                posSale.UpdateAssignBranch(assignBranch, warehouse, invoiceid);
                AssignmentGrid.JSProperties["cpMsg"] = "Updated Successfully.";
            }
            else
            {
                string invoiceId = e.Parameters.Split('~')[0];
                string BranchId = e.Parameters.Split('~')[1];
                DataTable availableStock = posSale.GetBranchAssignmentDetails(Convert.ToInt32(invoiceId), Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), Convert.ToInt32(BranchId));
                Session["BranchAssignmentTableForGrid"] = availableStock;
                AssignmentGrid.DataBind();
            }
        }
        protected void AssignmentGrid_DataBinding(object sender, EventArgs e)
        {
            DataTable availableStock = (DataTable)Session["BranchAssignmentTableForGrid"];
            AssignmentGrid.DataSource = availableStock;
        }

        protected void AssignedWareHouse_Callback(object sender, CallbackEventArgsBase e)
        {
            MasterSettings masterBl = new MasterSettings();
            string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));

            if (multiwarehouse != "1")
            {
                AssignedWareHouse.DataSource = posSale.getWareHouseByBranch(Convert.ToInt32(e.Parameter));
                AssignedWareHouse.TextField = "bui_Name";
                AssignedWareHouse.ValueField = "bui_id";
            }
            else
            {
                AssignedWareHouse.DataSource = oDBEngine.GetDataTable("EXEC [GET_BRANCHWISEWAREHOUSE] '1','" + e.Parameter + "'");
                AssignedWareHouse.TextField = "WarehouseName";
                AssignedWareHouse.ValueField = "WarehouseID";
            }


            AssignedWareHouse.DataBind();

            AssignedWareHouse.SelectedIndex = 0;
            if (e.Parameter != "0")
                if (AssignedWareHouse.Items.Count > 1)
                {
                    AssignedWareHouse.SelectedIndex = 1;
                }

        }
        protected void BranchRequUpdatePanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string InvoiceId = e.Parameter;
            DataTable invoiceDetails = oDBEngine.GetDataTable("select isnull(pos_assignBranch,0)pos_assignBranch,isnull(pos_wareHouse,0)pos_wareHouse   from tbl_trans_SalesInvoice where Invoice_Id =" + InvoiceId);
            if (invoiceDetails.Rows.Count > 0)
            {
                AssignedBranch.Value = Convert.ToString(invoiceDetails.Rows[0]["pos_assignBranch"]);
                AssignedWareHouse.DataSource = posSale.getWareHouseByBranch(Convert.ToInt32(invoiceDetails.Rows[0]["pos_assignBranch"]));
                AssignedWareHouse.TextField = "bui_Name";
                AssignedWareHouse.ValueField = "bui_id";
                AssignedWareHouse.DataBind();
                AssignedWareHouse.Value = Convert.ToString(invoiceDetails.Rows[0]["pos_wareHouse"]);
            }
        }
        #endregion




        public DataTable LoadBranch()
        {
            string userbranchHierachy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);

            return posSale.getBranchListByBranchListForMassBranch(userbranchHierachy, Convert.ToString(Session["userbranchID"]));
        }



        public class MassBranchAssign
        {
            public int BranchId { get; set; }
            public int InvoiceId { get; set; }
        }


        [WebMethod]
        public static string GetCurrentBankBalance(string BranchId, string fromDate, string todate)
        {
            BusinessLogicLayer.Converter oConverter = new BusinessLogicLayer.Converter();
            string MainAccountValID = string.Empty;
            string strColor = string.Empty;
            DataTable DT = new DataTable();
            DBEngine objEngine = new DBEngine();
            if (BranchId.Trim() != "0")
            {
                //+ " and convert(varchar(10),AccountsLedger_TransactionDate,120) >= '" + fromDate + "' and convert(varchar(10),AccountsLedger_TransactionDate,120) <='" + todate + "'"
                DT = objEngine.GetDataTable("Select isnull(Sum(AccountsLedger_AmountDr-AccountsLedger_AmountCr),0) TotalAmount from Trans_AccountsLedger WHERE AccountsLedger_MainAccountID=(select top 1  MainAccount_AccountCode from Master_MainAccount where MainAccount_BankCashType='Cash')  and AccountsLedger_BranchId=" + BranchId);
                if (DT.Rows.Count != 0)
                {

                    if (!String.IsNullOrEmpty(Convert.ToString(DT.Rows[0]["TotalAmount"])))
                    {
                        MainAccountValID = oConverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(DT.Rows[0]["TotalAmount"]));
                        strColor = Convert.ToDecimal(MainAccountValID) > 0 ? "White" : "Red";
                    }
                }
            }
            else
            {
                // and  convert(varchar(10),AccountsLedger_TransactionDate,120) >= '" + fromDate + "' and convert(varchar(10),AccountsLedger_TransactionDate,120) <='" + todate + "'
                DT = objEngine.GetDataTable("Select isnull(Sum(AccountsLedger_AmountDr-AccountsLedger_AmountCr),0) TotalAmount from Trans_AccountsLedger WHERE AccountsLedger_MainAccountID=(select top 1  MainAccount_AccountCode from Master_MainAccount where MainAccount_BankCashType='Cash') and  AccountsLedger_BranchId in( " + Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) + ")");
                if (DT.Rows.Count != 0)
                {

                    if (!String.IsNullOrEmpty(Convert.ToString(DT.Rows[0]["TotalAmount"])))
                    {
                        MainAccountValID = oConverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(DT.Rows[0]["TotalAmount"]));
                        strColor = Convert.ToDecimal(MainAccountValID) > 0 ? "White" : "Red";
                    }
                }

            }

            return MainAccountValID + "~" + strColor;
        }

        public void SetledgerOutPresentOrNot()
        {

            ProcedureExecute proc = new ProcedureExecute("prc_PosSalesInvoice");
            proc.AddVarcharPara("@Action", 500, "GetOutLedger");
            DataTable dt = proc.GetTable();
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    hdIsStockLedger.Value = "ok";
                }
                else
                {
                    hdIsStockLedger.Value = "no";
                }
            }
        }


        protected void CustRacPayPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            string strSplitCommand = e.Parameter.Split('~')[0];
            if (strSplitCommand == "Bindalldesignes")
            {
                string[] filePaths = new string[] { };
                string DesignPath = "";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\CustomerRecPay\DocDesign\Designes";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\CustomerRecPay\DocDesign\Designes";
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
                    CustCmbDesignName.Items.Add(name, reportValue);
                }
                CustCmbDesignName.SelectedIndex = 1;
                //CustCmbDesignName.Value = "MoneyReceipt~D";
            }
            else
            {
                string DesignPath = @"Reports\Reports\REPXReports";
                string fullpath = Server.MapPath("~");
                fullpath = fullpath.Replace("ERP.UI\\", "");
                string filename = @"\RepxReportViewer.aspx";
                string DesignFullPath = fullpath + DesignPath + filename;
                string reportName = Convert.ToString(CustCmbDesignName.Value);
                CustomerRecpayPanel.JSProperties["cpSuccess"] = "Success";
            }
        }

        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }

        protected void ComponentQuotation_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string strInvoiceID = Convert.ToString(Session["SI_InvoiceID"]);
            DataTable ComponentTable = objSalesInvoiceBL.GetVehicle();
            lookup_quotation.GridView.Selection.CancelSelection();
            lookup_quotation.DataSource = ComponentTable;
            lookup_quotation.DataBind();
            string inv = Session["Invoices"].ToString();
            PosSalesInvoiceBl ibl = new PosSalesInvoiceBl();
            DataSet dtdata = new DataSet();
            dtdata = ibl.GetInvoiceDeleviryData(inv);
            DataTable headTable = dtdata.Tables[0];
            if (headTable != null && headTable.Rows.Count > 0)
            {
                foreach (DataRow item in headTable.Rows)
                {
                    lookup_quotation.GridView.Selection.SelectRowByKey(Convert.ToInt32(item["VECHICLE_ID"].ToString()));
                }
            }
        }
        protected void lookup_quotation_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_ComponentData"] != null)
            {
                lookup_quotation.DataSource = (DataTable)Session["SI_ComponentData"];
            }
        }
        protected void EntityServerModeDataSourceCO_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {

            #region Block By Sudip
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
                        orderby d.SEQ ascending
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.PARTYOUTSTANDINGDET_REPORTs
                        where Convert.ToString(d.SLNO) == "0"
                        orderby d.SEQ ascending
                        select d;
                e.QueryableSource = q;
            }


            #endregion
            CustomerOutstanding.ExpandAll();

        }
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

        #region Tanmoy
        protected void ddl_SalesAgent_Callback(object sender, CallbackEventArgsBase e)
        {
            string CopyInvoiceId = e.Parameter;
            DataTable ds = new DataTable();
            // bindSalesmanByBranch(BranchId);
            DataTable branchId = oDBEngine.GetDataTable("select Invoice_BranchId branchId from tbl_trans_SalesInvoice where Invoice_Number= '" + CopyInvoiceId + "'");
            //PosSalesInvoiceBl PosData = new PosSalesInvoiceBl();
            //DataTable salsemanData = PosData.GetSalesmanByBranch(Convert.ToString(Convert.ToInt32(branchId.Rows[0]["branchId"])));
            ProcedureExecute proc = new ProcedureExecute("prc_PosSalesInvoice");
            proc.AddVarcharPara("@Action", 100, "GetSalesmanByBranch");
            proc.AddIntegerPara("@branch", Convert.ToInt32(branchId.Rows[0]["branchId"]));
            ds = proc.GetTable();

            if (ds != null)
            {
                ddl_SalesAgent.DataSource = ds;
                ddl_SalesAgent.TextField = "Name";
                ddl_SalesAgent.ValueField = "cnt_internalId";
                ddl_SalesAgent.DataBind();
                ddl_SalesAgent.SelectedIndex = 0;
            }
        }

        [WebMethod(EnableSession = true)]

        public static object bindSalesmanByBranch(string Inv)
        {
            SalesManList SalesManList = new SalesManList();
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable branchId = oDBEngine.GetDataTable("select Invoice_BranchId branchId from tbl_trans_SalesInvoice where Invoice_Number= '" + Inv + "'");
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_PosSalesInvoice");
            proc.AddVarcharPara("@Action", 100, "GetSalesmanByBranch");
            proc.AddIntegerPara("@branch", Convert.ToInt32(branchId.Rows[0]["branchId"]));
            ds = proc.GetTable();

            SalesManList.SalesManDetails = (from DataRow dr in ds.Rows
                                            select new SalesManDetails()
                                            {
                                                Name = dr["Name"].ToString(),
                                                CNTId = dr["cnt_internalId"].ToString()

                                            }).ToList();
            return SalesManList;
        }
        #endregion
        public class SalesManDetails
        {
            public string Name { get; set; }
            public string CNTId { get; set; }
        }
        public class SalesManList
        {
            public List<SalesManDetails> SalesManDetails { get; set; }
        }

        protected void GridApproval_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {

        }

        protected void challanNoScheme_Callback(object sender, CallbackEventArgsBase e)
        {
            string type = e.Parameter.Split('~')[0];
            if (type == "BindChallanScheme")
            {
                string branchId = e.Parameter.Split('~')[1];

                DataSet dst = new DataSet();
                string strCompanyID = Convert.ToString(Session["LastCompany"]);
                string FinYear = Convert.ToString(Session["LastFinYear"]);
                dst = objSlaesActivitiesBL.GetAllDropDownDetailForSalesChallan(branchId);
                SlaesActivitiesBL objSlaesActivitiesBL1 = new SlaesActivitiesBL();
                DataTable Schemadt = objSlaesActivitiesBL1.GetNumberingSchema(strCompanyID, branchId, FinYear, "10", "Y");


                if (Schemadt != null && Schemadt.Rows.Count > 0)
                {
                    challanNoScheme.TextField = "SchemaName";
                    challanNoScheme.ValueField = "Id";
                    challanNoScheme.DataSource = Schemadt;
                    challanNoScheme.DataBind();
                }
            }
        }

        protected void GridApproval_DataBinding(object sender, EventArgs e)
        {
            GridApproval.DataSource = posSale.GetApprovalList(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
        }
    }
}
