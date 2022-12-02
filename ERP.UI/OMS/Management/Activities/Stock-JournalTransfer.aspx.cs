using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using ERP.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities
{
    public partial class Stock_JournalTransfer : System.Web.UI.Page
    {
        SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
        Stocktransferjournal objjournal = new Stocktransferjournal();
        SalesInvoiceBL objSalesInvoiceBL = new SalesInvoiceBL();
        static string branchHierchy = "";
        string UniquePurchaseNumber = string.Empty;


      //  BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();


        string validate = "";
        string strSchemeType = "";
        string strJournalNumber = "";

        protected void Page_Load(object sender, EventArgs e)
        {
             CommonBL ComBL = new CommonBL();
             string ProjectSelectInEntryModule = ComBL.GetSystemSettingsResult("ProjectSelectInEntryModule");
             string ProjectMandatoryInEntry = ComBL.GetSystemSettingsResult("ProjectMandatoryInEntry");
             string HierarchySelectInEntryModule = ComBL.GetSystemSettingsResult("Show_Hierarchy");

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


            if (!IsPostBack)
            {
                ddlHierarchy.Enabled = false;
                 bindHierarchy();
                Session["Stock_Journal"] = null;
                Session["Stock_Journalqty"] = null;
                Session["Transfertobranch"] = null;
                branchHierchy = Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]);
                GetFromBranchNumberingScheme(branchHierchy);

                if (Request.QueryString["key"] == "ADD")
                {
                    Gridpopulate();
                    btn_stocktransferViemode.Visible = false;

                    if ((Session["LastFinYear"]) != null)
                    {
                        FinancialdateLocking(Convert.ToString(Session["LastFinYear"]));
                    }
                    lblHeading.Text = "Add Stock Journal (Stock Transfer)";
                }
                else
                {
                    btn_stocktransferViemode.Visible = true;
                    btn_SaveRecords.Visible = false;
                    btn_SaveRecordsExit.Visible = false;
                    btn_stocktransfer.Visible = false;
                    grid.Enabled = false;
                    ddl_numberingScheme.Enabled = false;
                    lbl_PIQuoteNo.Enabled = false;
                    ddl_Branch.Enabled = false;

                    lblHeading.Text = "View Stock Journal (Stock Transfer)";
                    Gridpopulatemodify(Request.QueryString["key"]);
               
                }

            }
        }
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        public void FinancialdateLocking(string finyear)
        {
            DateTime MinDate, MaxDate;
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
        public void Gridpopulatemodify(string journalID)
        {
            DataSet ds = new DataSet();

            ds = objjournal.GetListJournalsModifyList(journalID);
            if (ds.Tables[0].Rows.Count > 0)
            {

                dt_PLQuote.Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["JournalDate"]);
                txtVoucherNo.Text = Convert.ToString(ds.Tables[0].Rows[0]["Number"]);
                ddl_Branch.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["FromBranch"]);
                //     ddl_to_branch.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["ToBranch"]);
                ddlInventory.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["InventoryId"]);
                ddl_numberingScheme.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["NumberingSchemeId"]);
                txtnarration.Text = Convert.ToString(ds.Tables[0].Rows[0]["Narration"]);
                grid.DataSource = GetSstockJournalGrid(ds.Tables[1]);
                grid.DataBind();

            }
            ds = objjournal.GetListJournalsModifyStocksList(journalID);
            if (ds.Tables[1].Rows.Count > 0)
            {

                ///  In Stock /////
                if (Convert.ToString(ds.Tables[0].Rows[0]["Inventory_type"]) == "WH")
                {
                    txtwarehouseJI.Value = Convert.ToString(ds.Tables[0].Rows[0]["WarehouseName"]);
                    txtqtyJI.Value = Convert.ToString(ds.Tables[0].Rows[0]["IN_Quantity"]);
                    gridstockjournalin.Visible = false;
                }

                if (Convert.ToString(ds.Tables[0].Rows[0]["Inventory_type"]) == "WHSL")
                {

                    txtwarehouseJI.Value = Convert.ToString(ds.Tables[0].Rows[0]["WarehouseName"]);
                    txtqtyJI.Value = Convert.ToString(ds.Tables[0].Rows[0]["IN_Quantity"]);
                    txtqtyJI.Visible = false;
                    gridstockjournalin.DataSource = ds.Tables[0];
                    gridstockjournalin.DataBind();
                }

                ///  In Stock /////

                ///  Out Stock /////
                if (Convert.ToString(ds.Tables[1].Rows[0]["Inventory_type"]) == "WH")
                {
                    txtwarehouseJO.Value = Convert.ToString(ds.Tables[1].Rows[0]["WarehouseName"]);
                    txtqtyJO.Value = Convert.ToString(ds.Tables[1].Rows[0]["OUT_Quantity"]);
                    gridstockjournalout.Visible = false;
                }

                 if (Convert.ToString(ds.Tables[1].Rows[0]["Inventory_type"]) == "WHSL")
                {
                    txtwarehouseJO.Value = Convert.ToString(ds.Tables[1].Rows[0]["WarehouseName"]);
                    txtqtyJO.Visible = false;
                    gridstockjournalout.DataSource = ds.Tables[1];
                    gridstockjournalout.DataBind();

                }

                ///  Out Stock /////

            }
              DataTable dtt =GetProjectEditData(journalID);
                if (dtt != null && dtt.Rows.Count>0)
                {
                    lookup_Project.GridView.Selection.SelectRowByKey(Convert.ToInt64(dtt.Rows[0]["Proj_Id"]));

                   
                    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
                    DataTable dt2 = oDBEngine.GetDataTable("Select Hierarchy_ID from V_ProjectList where Proj_Id='" + Convert.ToInt64(dtt.Rows[0]["Proj_Id"]) + "'");
                    if (dt2.Rows.Count > 0)
                    {
                        ddlHierarchy.SelectedValue = Convert.ToString(dt2.Rows[0]["Hierarchy_ID"]);
                    }
                 
                }

        }

         public DataTable GetProjectEditData(string DocId)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("StockJournal_Modification");
            proc.AddVarcharPara("@Action", 500, "ProjectEditdata");
            proc.AddVarcharPara("@DocId", 200, DocId);
            dt = proc.GetTable();
            return dt;
        }
        #region  From branch and Numbering scheme populate

        public void GetFromBranchNumberingScheme(string branchHierchy)
        {
            DataSet dst = new DataSet();
            dst = objjournal.GetFromBranches(branchHierchy);

            //Numbering Scheme
            ddl_numberingScheme.DataSource = dst.Tables[1];
            ddl_numberingScheme.DataBind();

            //set Branch
            ddl_Branch.DataSource = dst.Tables[0];
            ddl_Branch.DataBind();



            // DataView dv = new DataView(dst.Tables[0]);
            //dv.RowFilter = "BANKBRANCH_ID <>" + frmBranch;
            //set Branch
            //ddl_to_branch.DataSource = dst.Tables[0];
            //ddl_to_branch.DataBind();

        }

        #endregion

        #region Grid Population
        public void Gridpopulate()
        {
            DataTable Transactiondt = CreateTempTable("Transaction");

            //Transactiondt.Rows.Add("1", "1", "Transfer From", "", "", "", "", "", "0.0000", "", "0.00", "0.00", "0.00", "0.00", "0.00", "0", "0", "", "", "", "I");
            //Transactiondt.Rows.Add("2", "2", "Transfer To", "", "", "", "", "", "0.0000", "", "0.00", "0.00", "0.00", "0.00", "0.00", "0", "0", "", "", "", "I");

            Transactiondt.Rows.Add("1", "Stk-Out (Consumable)", "", "", "", "0", "", "0.00", "0.00", "0.00", "");
            Transactiondt.Rows.Add("2", "Stk-In (Receipt)", "", "", "", "0", "", "0.00", "0.00", "0.00", "");


            //DataTable Warehousedt = CreateTempTable("Warehouse");


            Session["TransactionList"] = Transactiondt;
            //Session["Product_StockList"] = Warehousedt;
            Session["Stock_LoopID"] = "1";

            grid.DataSource = GetSstockJournalGrid(Transactiondt);
            grid.DataBind();

        }


        public DataTable CreateTempTable(string Type)
        {

            DataTable dt = new DataTable();

            if (Type == "Transaction")
            {
                dt.Columns.Add("SrlNo", typeof(string));
                //dt.Columns.Add("RowNo", typeof(string));
                dt.Columns.Add("Type", typeof(string));
                //dt.Columns.Add("DocNumber", typeof(string));
                //dt.Columns.Add("DocID", typeof(string));
                dt.Columns.Add("ProductID", typeof(string));
                dt.Columns.Add("ProductName", typeof(string));
                dt.Columns.Add("ProductDiscription", typeof(string));
                dt.Columns.Add("Quantity", typeof(string));
                dt.Columns.Add("PurchaseUOM", typeof(string));
                dt.Columns.Add("PurchasePrice", typeof(string));
                //dt.Columns.Add("Discount", typeof(string));
                dt.Columns.Add("TotalAmount", typeof(string));
                //dt.Columns.Add("TaxAmount", typeof(string));
                dt.Columns.Add("NetAmount", typeof(string));
                //dt.Columns.Add("TotalQty", typeof(string));
                //dt.Columns.Add("BalanceQty", typeof(string));
                //dt.Columns.Add("IsComponentProduct", typeof(string));
                //dt.Columns.Add("IsLinkedProduct", typeof(string));
                //dt.Columns.Add("ComponentProduct", typeof(string));
                //dt.Columns.Add("Status", typeof(string));

                dt.Columns.Add("ProductClass", typeof(string));
                dt.Columns.Add("Brand", typeof(string));

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
            else if (Type == "JournarStock")
            {
                dt.Columns.Add("TypeId", typeof(string));
                dt.Columns.Add("ProductId", typeof(int));
                dt.Columns.Add("Quantity", typeof(string));
                dt.Columns.Add("UOM", typeof(string));
                dt.Columns.Add("Product_Price", typeof(decimal));
                dt.Columns.Add("Amount", typeof(decimal));

            }

            else if (Type == "WarehouseWHSL")
            {
                dt.Columns.Add("WarehouseID", typeof(string));
                dt.Columns.Add("Qtycount", typeof(int));
                dt.Columns.Add("SerialID", typeof(string));
            }


            return dt;
        }

        public IEnumerable GetSstockJournalGrid(DataTable dt)
        {
            List<TransactionListstock> TransactionList = new List<TransactionListstock>();

            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //if (Convert.ToString(dt.Rows[i]["Status"]) != "D")
                    //{
                    TransactionListstock Transactions = new TransactionListstock();

                    Transactions.SrlNo = Convert.ToString(dt.Rows[i]["SrlNo"]);
                    //Transactions.RowNo = Convert.ToString(dt.Rows[i]["RowNo"]);
                    Transactions.Type = Convert.ToString(dt.Rows[i]["Type"]);

                    //Transactions.DocNumber = Convert.ToString(dt.Rows[i]["DocNumber"]);
                    //Transactions.DocID = Convert.ToString(dt.Rows[i]["DocID"]);
                    Transactions.ProductID = Convert.ToString(dt.Rows[i]["ProductID"]);
                    Transactions.ProductName = Convert.ToString(dt.Rows[i]["ProductName"]);
                    Transactions.ProductDiscription = Convert.ToString(dt.Rows[i]["ProductDiscription"]);
                    Transactions.Quantity = Convert.ToString(dt.Rows[i]["Quantity"]);
                    Transactions.PurchaseUOM = Convert.ToString(dt.Rows[i]["PurchaseUOM"]);
                    Transactions.PurchasePrice = Convert.ToString(dt.Rows[i]["PurchasePrice"]);
                    //Transactions.Discount = Convert.ToString(dt.Rows[i]["Discount"]);
                    Transactions.TotalAmount = Convert.ToString(dt.Rows[i]["TotalAmount"]);
                    //Transactions.TaxAmount = Convert.ToString(dt.Rows[i]["TaxAmount"]);
                    Transactions.NetAmount = Convert.ToString(dt.Rows[i]["NetAmount"]);
                    //Transactions.TotalQty = Convert.ToString(dt.Rows[i]["TotalQty"]);
                    //Transactions.BalanceQty = Convert.ToString(dt.Rows[i]["BalanceQty"]);
                    //Transactions.IsComponentProduct = Convert.ToString(dt.Rows[i]["IsComponentProduct"]);
                    //Transactions.IsLinkedProduct = Convert.ToString(dt.Rows[i]["IsLinkedProduct"]);
                    //Transactions.ComponentProduct = Convert.ToString(dt.Rows[i]["ComponentProduct"]);
                    //Transactions.Status = Convert.ToString(dt.Rows[i]["Status"]);
                    Transactions.ProductClass = Convert.ToString(dt.Rows[i]["ProductClass"]);
                    Transactions.Brand = Convert.ToString(dt.Rows[i]["Brand"]);
                    TransactionList.Add(Transactions);
                    //}
                }
            }

            return TransactionList;
        }
        #endregion

        #region Class Structure
        public class TransactionListstock
        {
            public string SrlNo { get; set; }
            public string RowNo { get; set; }
            public string Type { get; set; }
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

            public string ProductClass { get; set; }
            public string Brand { get; set; }

        }
        #endregion

        #region Insert Modification
        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];
            string saveexit = e.Parameters.Split('~')[2];
            DataTable dtjournal = CreateTempTable("JournarStock");
            string User = "";
            DataTable dtstockwarehouse = new DataTable();

            string strCompanyID = Convert.ToString(Session["LastCompany"]);

            string FinYear = Convert.ToString(Session["LastFinYear"]);


            if (Session["userid"] != null)
            {
                User = Convert.ToString(HttpContext.Current.Session["userid"]).Trim();
            }
            if (strSplitCommand == "InsertJournal")
            {
                string transfertowarehouse = "";

                if (Session["Transfertobranch"] != null)
                {
                    transfertowarehouse = Convert.ToString(Session["Transfertobranch"]);
                }

                if (Request.QueryString["key"] == "ADD")
                {
                    string command = e.Parameters.ToString();
                    //string voucher = txtVoucherNo.Text;
                    //string Date = dt_PLQuote.Date.ToString("yyyy-MM-dd");
                    //string frombranch = ddl_Branch.SelectedValue;
                    //string tobranch = ddl_to_branch.SelectedValue;


                    strSchemeType = Convert.ToString(ddl_numberingScheme.SelectedValue);
                    strJournalNumber = Convert.ToString(txtVoucherNo.Text);

                    string[] SchemeList = strSchemeType.Split(new string[] { "~" }, StringSplitOptions.None);
                    validate = checkNMakeJVCode(strJournalNumber, Convert.ToInt32(SchemeList[0]));



                    string strloop = e.Parameters.Split('~')[1];
                    string[] items = strloop.Split('@');
                    foreach (var item in items)
                    {
                        string[] itemssub = item.Split('|');
                        dtjournal.Rows.Add(itemssub[0] == "Stk-Out (Consumable)" ? "From" : "To", itemssub[1], itemssub[2], itemssub[3], itemssub[4], Convert.ToDecimal(itemssub[4]) * Convert.ToDecimal(itemssub[2]));
                    }


                    if (Session["Stock_Journal"] != null)
                    {
                        dtstockwarehouse = (DataTable)Session["Stock_Journal"];

                    }
                    if (Session["Transfertobranch"] != null)
                    {


                        int i = Convert.ToInt32(e.Parameters.Split('~')[3].IndexOf('.'));
                        if (Session["Stock_Journalqty"] != null)
                        {
                            Int64 Proj_id = 0;
                            if (lookup_Project.Text != "")
                            {
                                Proj_id = Convert.ToInt64(lookup_Project.Value);
                            }
                            int ex = objjournal.ExecuteNonqueryStockJournal(User, UniquePurchaseNumber, dt_PLQuote.Date.ToString("yyyy-MM-dd"), ddl_Branch.SelectedValue, ddl_Branch.SelectedValue, dtjournal, dtstockwarehouse, ddlInventory.SelectedValue, ddl_numberingScheme.SelectedValue, FinYear, strCompanyID, transfertowarehouse, "InsertJournal", txtnarration.Text,"" ,Proj_id);


                            if (ex > 0)
                            {
                                grid.JSProperties["cpSuccess"] = saveexit;
                            }
                            else if (ex == -12)
                            {
                                grid.JSProperties["cpSuccess"] = "EmptyProject";
                            }
                            else
                            {

                                grid.JSProperties["cpSuccess"] = "Different type of Inventory Product not allowed ";
                            }
                        }

                        else
                        {
                            grid.JSProperties["cpSuccess"] = "Quantity should be same as warehouse quantity.";

                        }
                    }
                    else
                    {

                        grid.JSProperties["cpSuccess"] = "Warehouse data need to be seleted.";
                    }
                }
                else
                {

                    strSchemeType = Convert.ToString(ddl_numberingScheme.SelectedValue);
                    strJournalNumber = Convert.ToString(txtVoucherNo.Text);

                    string[] SchemeList = strSchemeType.Split(new string[] { "~" }, StringSplitOptions.None);
                    validate = checkNMakeJVCode(strJournalNumber, Convert.ToInt32(SchemeList[0]));



                    string strloop = e.Parameters.Split('~')[1];
                    string[] items = strloop.Split('@');
                    foreach (var item in items)
                    {
                        string[] itemssub = item.Split('|');
                        dtjournal.Rows.Add(itemssub[0] == "Stk-Out (Consumable)" ? "From" : "To", itemssub[1], itemssub[2], itemssub[3], itemssub[4], Convert.ToDecimal(itemssub[4]) * Convert.ToDecimal(itemssub[2]));
                    }
                    if (Session["Stock_Journal"] != null)
                    {
                        dtstockwarehouse = (DataTable)Session["Stock_Journal"];

                    }
                    Int64 Proj_id=0;
                    if(lookup_Project.Text!="")
                    {
                       Proj_id=Convert.ToInt64(lookup_Project.Value);
                    }

                    int ex = objjournal.ExecuteNonqueryStockJournal(User, UniquePurchaseNumber, dt_PLQuote.Date.ToString("yyyy-MM-dd"), ddl_Branch.SelectedValue, ddl_Branch.SelectedValue, dtjournal, dtstockwarehouse, ddlInventory.SelectedValue, ddl_numberingScheme.SelectedValue, FinYear, strCompanyID, transfertowarehouse, "Modifyjournal", txtnarration.Text, Request.QueryString["key"],Proj_id);


                    if (ex > 0)
                    {
                        grid.JSProperties["cpSuccess"] = saveexit;
                    }

                }
            }
        }
        #endregion

        #region Warehouse Bind

        protected void ComponentQuotation_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

            Session["Stock_Journal"] = null;
            Session["Transfertobranch"] = null;
            Session["Stock_Journalqty"] = null;
            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                if (ddl_numberingScheme.SelectedValue != "")
                {
                    string s = ddl_Branch.SelectedValue;
                    string tobranch = ddl_Branch.SelectedValue;
                    string num = ddl_numberingScheme.SelectedValue;

                    string fromproduct = "";
                    DataTable dttransferto = new DataTable();
                    DataTable dtwarehouse = new DataTable();

                    dttransferto = objjournal.Gettransfertobranch(ddl_Branch.SelectedValue);

                    ddlwarehouse.DataSource = dttransferto;
                    ddlwarehouse.DataTextField = "WarehouseName";
                    ddlwarehouse.DataValueField = "WarehouseID";
                    ddlwarehouse.DataBind();



                    fromproduct = e.Parameter.Split('~')[1];

                    dtwarehouse = objjournal.WarehouseGridbund(ddl_Branch.SelectedValue, ddl_Branch.SelectedValue, fromproduct);
                    if (dtwarehouse != null)
                    {
                        if (dtwarehouse.Rows.Count > 0)
                        {
                            if (Convert.ToString(dtwarehouse.Rows[0]["Inventorytype"]) == "WHSL")
                            {
                                dvWH.Visible = false;
                                dvWHSL.Visible = true;
                                gridwarehouse.DataSource = dtwarehouse;
                                gridwarehouse.DataBind();
                            }
                            else if (Convert.ToString(dtwarehouse.Rows[0]["Inventorytype"]) == "WH")
                            {
                                dvWH.Visible = true;
                                dvWHSL.Visible = false;
                                gridwarehousewithoutserial.DataSource = dtwarehouse;
                                gridwarehousewithoutserial.DataBind();

                                hdntotWHquantity.Value = Convert.ToString(dtwarehouse.Rows[0]["Quantity"]);
                            }


                            hdninventorytype.Value = Convert.ToString(dtwarehouse.Rows[0]["Inventorytype"]);

                            ComponentQuotationPanel.JSProperties["cpSuccess"] = Convert.ToString(dtwarehouse.Rows[0]["Inventorytype"]);
                            ComponentQuotationPanel.JSProperties["cphiddenqty"] = hdntotWHquantity.Value;
                        }
                        else
                        {
                            ComponentQuotationPanel.JSProperties["cpSuccess"] = "No warehouse";

                        }
                    }
                    else
                    {
                        ComponentQuotationPanel.JSProperties["cpSuccess"] = "No warehouse";

                    }

                }
                ComponentQuotationPanel.JSProperties["cpclose"] = "open";

            }

            else if (e.Parameter.Split('~')[0] == "Closewarehouse")
            {

                if (e.Parameter.Split('~')[1] == "WH")
                {
                    DataTable dtwh = new DataTable();
                    dtwh = CreateTempTable("WarehouseWHSL");
                    for (int i = 0; i < gridwarehousewithoutserial.VisibleRowCount; i++)
                    {
                        string check = gridwarehousewithoutserial.GetRowValues(i, "Quantity").ToString();
                        string warehouseid = gridwarehousewithoutserial.GetRowValues(i, "WarehouseID").ToString();
                        //if (!string.IsNullOrEmpty(check) && !string.IsNullOrEmpty(txtqty.Value) && Int32.Parse(check) <= Int32.Parse(txtqty.Value))
                        //{
                        dtwh.Rows.Add(warehouseid, txtqty.Value, "");
                        Session["Stock_Journal"] = dtwh;
                        Session["Stock_Journalqty"] = txtqty.Value;
                        ComponentQuotationPanel.JSProperties["cpquantity"] = txtqty.Value;

                        //}
                    }
                }

                else if (e.Parameter.Split('~')[1] == "WHSL")
                {

                    DataTable dtwhsl = new DataTable();
                    dtwhsl = CreateTempTable("WarehouseWHSL");
                    int counter = 0;

                    int totalquantitycount = gridwarehouse.Selection.Count;
                    for (int i = 0; i < gridwarehouse.VisibleRowCount; i++)
                    {
                        if (gridwarehouse.Selection.IsRowSelected(i))
                        {
                            string warehouse = gridwarehouse.GetRowValues(i, "WarehouseID").ToString();
                            string SerialNo = gridwarehouse.GetRowValues(i, "SerialID").ToString();
                            //counter++;
                            dtwhsl.Rows.Add(warehouse, totalquantitycount, SerialNo);

                            Session["Stock_Journal"] = dtwhsl;
                        }

                    }
                    ComponentQuotationPanel.JSProperties["cpquantity"] = totalquantitycount;
                    Session["Stock_Journalqty"] = Convert.ToString(totalquantitycount);
                }

                Session["Transfertobranch"] = e.Parameter.Split('~')[2];

                ComponentQuotationPanel.JSProperties["cpclose"] = "close";
            }
        }




        #endregion

        #region  Numbering Schema

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

                    sqlQuery = "SELECT max(tjv.Journal_Number) FROM tbl_trans_stockjournalhead tjv WHERE dbo.RegexMatch('";
                    if (prefLen > 0)
                        sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                    else if (scheme_type == 2)
                        sqlQuery += "^";
                    sqlQuery += "[0-9]{" + paddCounter + "}";
                    if (sufxLen > 0)
                        sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                    //sqlQuery += "?$', LTRIM(RTRIM(tjv.PurchaseChallan_Number))) = 1";
                    sqlQuery += "?$', LTRIM(RTRIM(tjv.Journal_Number))) = 1 and Journal_Number like '" + prefCompCode + "%'";
                    if (scheme_type == 2)
                        sqlQuery += " AND CONVERT(DATE, CreatedDate) = CONVERT(DATE, GETDATE())";
                    dtC = oDBEngine.GetDataTable(sqlQuery);

                    if (dtC.Rows[0][0].ToString() == "")
                    {
                        sqlQuery = "SELECT max(tjv.Journal_Number) FROM tbl_trans_stockjournalhead tjv WHERE dbo.RegexMatch('";
                        if (prefLen > 0)
                            sqlQuery += "^[" + prefCompCode + "]{" + prefLen + "}";
                        else if (scheme_type == 2)
                            sqlQuery += "^";
                        sqlQuery += "[0-9]{" + (paddCounter - 1) + "}";
                        if (sufxLen > 0)
                            sqlQuery += "[" + sufxCompCode + "]{" + sufxLen + "}";
                        //sqlQuery += "?$', LTRIM(RTRIM(tjv.PurchaseChallan_Number))) = 1";
                        sqlQuery += "?$', LTRIM(RTRIM(tjv.Journal_Number))) = 1 and Journal_Number like '" + prefCompCode + "%'";
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
                            UniquePurchaseNumber = prefCompCode + paddedStr + sufxCompCode;
                            return "ok";
                        }
                    }
                    else
                    {
                        UniquePurchaseNumber = startNo.PadLeft(paddCounter, '0');
                        UniquePurchaseNumber = prefCompCode + paddedStr + sufxCompCode;
                        return "ok";
                    }
                }
                else
                {
                    sqlQuery = "SELECT Journal_Number FROM tbl_trans_stockjournalhead WHERE Journal_Number LIKE '" + manual_str.Trim() + "'";
                    dtC = oDBEngine.GetDataTable(sqlQuery);
                    // duplicate manual entry check
                    if (dtC.Rows.Count > 0 && dtC.Rows[0][0].ToString().Trim() != "")
                    {
                        return "duplicate";
                    }

                    UniquePurchaseNumber = manual_str.Trim();
                    return "ok";
                }
            }
            else
            {
                return "noid";
            }
        }
        #endregion

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

      protected void EntityServerModeDataStock_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Proj_Id";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            BusinessLogicLayer.DBEngine BEngine = new BusinessLogicLayer.DBEngine();




            var q = from d in dc.V_ProjectLists
                    where d.ProjectStatus == "Approved" && d.ProjBracnchid == Convert.ToInt64(ddl_Branch.SelectedValue) 
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
       
    }


    }
