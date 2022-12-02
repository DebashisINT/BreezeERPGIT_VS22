using DataAccessLayer;
using DevExpress.Web;
using DevExpress.Web.Data;
using ERP.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities
{
    public partial class stockReconcileAdd : System.Web.UI.Page
    {
        string userbranch = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                Session["BindProduct_Details"] = null;
                Session["BindProduct_CalcuCommitDetails"] = null;
                DataTable dt = GetWarehouseData();
                CmbWarehouse.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CmbWarehouse.Items.Add(Convert.ToString(dt.Rows[i]["WarehouseName"]), Convert.ToString(dt.Rows[i]["WarehouseID"]));
                }
                if (Session["userbranchHierarchy"] != null)
                {
                    userbranch = Convert.ToString(Session["userbranchHierarchy"]);
                }           


                if (Request.QueryString["key"] != null)
                {
                    if (Convert.ToString(Request.QueryString["key"]) != "ADD")
                    {
                        lblHeadTitle.Text = "Modify Stock Reconcile";
                        hdnPageStatus.Value = "Edit";
                        Int64 ReconCileId = Convert.ToInt64(Request.QueryString["key"]);
                        hdnReconcileId.Value = Convert.ToString(ReconCileId);
                        SetEditDetails(ReconCileId);

                        if(Convert.ToString(Request.QueryString["mode"]) == "V")
                        {
                            lblHeadTitle.Text = "View Stock Reconcile";
                            btn_SaveRecords.Visible = false;
                            ASPxButton1.Visible = false;
                            ASPxButton4.Visible = false;
                        }                     

                    }
                    else
                    {
                        lblHeadTitle.Text = "Add Stock Reconcile";
                        hdnPageStatus.Value = "ADD";
                        GetAllDropDownDetailForReconcillation(userbranch);
                        ASPxButton1.Visible = false;

                    }
                }                
            }
        }
        public void SetEditDetails(Int64 strReconcileId)
        {
            DataSet ds = new DataSet();

            ds = GetEditProductDetailsBind(strReconcileId);
            DataTable productDetsils = ds.Tables[0];
            DataTable ReconDetails = ds.Tables[1];
            if(productDetsils.Rows.Count>0)
            {
                Session["BindProduct_Details"] = productDetsils;
                grid.DataSource = GetQuotation(productDetsils);
                grid.DataBind();
            }
            if(ReconDetails.Rows.Count>0)
            {
                txt_CustDocNo.Text = Convert.ToString(ReconDetails.Rows[0]["ReconNumber"]);
                dt_PLQuote.Date = Convert.ToDateTime(ReconDetails.Rows[0]["PostingDate"]);
                dt_PLQuote.ClientEnabled = false;
                hdnWarehouseTextF.Value = Convert.ToString(ReconDetails.Rows[0]["WareHouseName"]);
                CmbWarehouse.Value = Convert.ToString(ReconDetails.Rows[0]["WareHouseId"]);
                txtRemarks.InnerText = Convert.ToString(ReconDetails.Rows[0]["Remarks"]);
                hdnPostedToStockCheck.Value = Convert.ToString(ReconDetails.Rows[0]["Is_PostedToStkAdj"]);
                if(hdnPostedToStockCheck.Value=="Y")
                {
                    btn_SaveRecords.Visible = false;
                    ASPxButton1.Visible = false;
                }

            }


        }

        public void GetAllDropDownDetailForReconcillation(string userbranch)
        {

            DataTable Schemadt = new DataTable();
            
            string userbranchHierarchy = Convert.ToString(Session["userbranchHierarchy"]);




            Schemadt = GetNumberingSchema(userbranchHierarchy);
            if (Schemadt != null && Schemadt.Rows.Count > 0)
            {
                ddl_numberingScheme.DataTextField = "SchemaName";
                ddl_numberingScheme.DataValueField = "Id";
                ddl_numberingScheme.DataSource = Schemadt;
                ddl_numberingScheme.DataBind();
            }
        }
        public DataTable GetNumberingSchema(string strBranchID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_ReconcileStock");
            proc.AddVarcharPara("@Action", 100, "GetNUmberingSchema");
            proc.AddVarcharPara("@userbranchlist", 4000, strBranchID);
            ds = proc.GetTable();
            return ds;
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
        protected void grid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (e.Column.FieldName == "Description")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "Product")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "Class")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "StockUnit")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "AltUnit")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "StockUnitQnty")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "SrlNo")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "AltUnitQnty")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "DiffStockUnitQnty")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "DiffAltUnitQnty")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "Price")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "ALTCLOSE_QTY")
            {
                e.Editor.Enabled = true;
            }
            else if (e.Column.FieldName == "CLOSE_QTY")
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
            grid.JSProperties["cpSuccess"] = "";
            grid.JSProperties["cpDocId"] = "";
            string IsDeleteFrom = "";
            IsDeleteFrom= Convert.ToString(hdfIsDelete.Value);

            DataTable Quotationdt = new DataTable();
            if (Session["BindProduct_Details"] != null)
            {
                Quotationdt = (DataTable)Session["BindProduct_Details"];
            }
            else
            {

                Quotationdt.Columns.Add("SrlNo", typeof(string));
                Quotationdt.Columns.Add("sProducts_ID", typeof(Int64));
                Quotationdt.Columns.Add("Product", typeof(string));
                Quotationdt.Columns.Add("Description", typeof(string));
                Quotationdt.Columns.Add("sProduct_Status", typeof(string));
                Quotationdt.Columns.Add("Class", typeof(string));
                Quotationdt.Columns.Add("StockUnit", typeof(string));
                Quotationdt.Columns.Add("AltUnit", typeof(string));
                Quotationdt.Columns.Add("StockUnitQnty", typeof(string));
                Quotationdt.Columns.Add("AltUnitQnty", typeof(string));
                Quotationdt.Columns.Add("ProductID", typeof(string));
                Quotationdt.Columns.Add("ClassId", typeof(Int64));
                Quotationdt.Columns.Add("BrandId", typeof(Int64));
                Quotationdt.Columns.Add("StockUnitId", typeof(Int64));
                Quotationdt.Columns.Add("AltUnitId", typeof(Int64));
                Quotationdt.Columns.Add("CLOSE_QTY", typeof(string));
                Quotationdt.Columns.Add("ALTCLOSE_QTY", typeof(string));
                Quotationdt.Columns.Add("Price", typeof(string));
                Quotationdt.Columns.Add("DiffStockUnitQnty", typeof(string));
                Quotationdt.Columns.Add("DiffAltUnitQnty", typeof(string));                
                
            }
            foreach (var args in e.InsertValues)
            {
                string ProductDetails = Convert.ToString(args.NewValues["ProductID"]);
                if (ProductDetails != "" && ProductDetails != "0")
                {
                    string SrlNo = Convert.ToString(args.NewValues["SrlNo"]);
                    string[] ProductDetailsList = ProductDetails.Split(new string[] { "||@||" }, StringSplitOptions.None);
                    string ProductID = ProductDetailsList[0];
                    string ProductDetailsForId = Convert.ToString(args.NewValues["ProductID"]);
                    Int64 ProductdisID = Convert.ToInt64(ProductID);
                    Int64 StockId = Convert.ToInt64(args.NewValues["StockId"]);
                    Int64 StockUnitId = Convert.ToInt64(args.NewValues["StockUnitId"]);
                    Int64 AltUnitId = Convert.ToInt64(args.NewValues["AltUnitId"]);

                    //Int64 ProductdisIdID = Convert.ToInt64(ProductDetailsList[0]);
                    //Int64 StockId = Convert.ToInt64(ProductDetailsList[1]);
                    //Int64 StockUnitId = Convert.ToInt64(ProductDetailsList[2]);
                    //Int64 AltUnitId = Convert.ToInt64(ProductDetailsList[3]);
                    string Product = Convert.ToString(args.NewValues["Product"]);
                    string Description = Convert.ToString(args.NewValues["Description"]);
                    string sProduct_Status = Convert.ToString(args.NewValues["sProduct_Status"]);
                    string Class = Convert.ToString(args.NewValues["Class"]);
                    string StockUnit = Convert.ToString(args.NewValues["StockUnit"]);
                    string AltUnit = Convert.ToString(args.NewValues["AltUnit"]);

                    string StockUnitQnty = Convert.ToString(args.NewValues["StockUnitQnty"]);
                    string AltUnitQnty = Convert.ToString(args.NewValues["AltUnitQnty"]);

                    Int64 ClassId = Convert.ToInt64(args.NewValues["ClassId"]);
                    Int64 BrandId = Convert.ToInt64(args.NewValues["BrandId"]);


                    string CLOSE_QTY = Convert.ToString(args.NewValues["CLOSE_QTY"]);
                    string ALTCLOSE_QTY = Convert.ToString(args.NewValues["ALTCLOSE_QTY"]);
                    string Price = Convert.ToString(args.NewValues["Price"]);

                    string DiffStockUnitQnty = Convert.ToString(args.NewValues["DiffStockUnitQnty"]);
                    string DiffAltUnitQnty = Convert.ToString(args.NewValues["DiffAltUnitQnty"]);
                  

                    Quotationdt.Rows.Add(SrlNo, ProductdisID, Product, Description,sProduct_Status, Class, StockUnit, AltUnit, StockUnitQnty, AltUnitQnty, ProductDetailsForId, ClassId, BrandId, StockUnitId, AltUnitId, CLOSE_QTY, ALTCLOSE_QTY, Price, DiffStockUnitQnty, DiffAltUnitQnty);


                }
            }

            foreach (var args in e.UpdateValues)
            {
                string SrlNo = Convert.ToString(args.NewValues["SrlNo"]);
                Int64 sProducts_ID = Convert.ToInt64(args.Keys["sProducts_ID"]);
                string ProductDetails = Convert.ToString(args.NewValues["ProductID"]);
                bool isDeleted = false;
                foreach (var arg in e.DeleteValues)
                {
                    Int64 DeleteID = Convert.ToInt64(arg.Keys["sProducts_ID"]);
                    if (DeleteID == sProducts_ID)
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


                        string ProductDetailsForId = Convert.ToString(args.NewValues["ProductID"]);
                        string ProductID = ProductDetailsList[0];
                        Int64 ProductdisID = Convert.ToInt64(ProductID);
                        Int64 StockId = Convert.ToInt64(args.NewValues["StockId"]);
                        Int64 StockUnitId = Convert.ToInt64(args.NewValues["StockUnitId"]);
                        Int64 AltUnitId = Convert.ToInt64(args.NewValues["AltUnitId"]);
                        string Product = Convert.ToString(args.NewValues["Product"]);
                        string Description = Convert.ToString(args.NewValues["Description"]);
                        string sProduct_Status = Convert.ToString(args.NewValues["sProduct_Status"]);
                        string Class = Convert.ToString(args.NewValues["Class"]);
                        string StockUnit = Convert.ToString(args.NewValues["StockUnit"]);
                        string AltUnit = Convert.ToString(args.NewValues["AltUnit"]);

                        string StockUnitQnty = Convert.ToString(args.NewValues["StockUnitQnty"]);
                        string AltUnitQnty = Convert.ToString(args.NewValues["AltUnitQnty"]);
                        Int64 ClassId = Convert.ToInt64(args.NewValues["ClassId"]);
                        Int64 BrandId = Convert.ToInt64(args.NewValues["BrandId"]);
                        string CLOSE_QTY = Convert.ToString(args.NewValues["CLOSE_QTY"]);
                        string ALTCLOSE_QTY = Convert.ToString(args.NewValues["ALTCLOSE_QTY"]);
                        string Price = Convert.ToString(args.NewValues["Price"]);
                        string DiffStockUnitQnty = Convert.ToString(args.NewValues["DiffStockUnitQnty"]);
                        string DiffAltUnitQnty = Convert.ToString(args.NewValues["DiffAltUnitQnty"]);                      

                        bool Isexists = false;
                        foreach (DataRow drr in Quotationdt.Rows)
                        {
                            Int64 OldQuotationID = Convert.ToInt64(drr["sProducts_ID"]);

                            if (OldQuotationID == sProducts_ID)
                            {
                                Isexists = true;
                                drr["sProducts_ID"] = ProductdisID;
                                drr["Product"] = Product;
                                drr["Description"] = Description;
                                drr["sProduct_Status"] = sProduct_Status;
                                drr["Class"] = Class;
                                drr["StockUnit"] = StockUnit;
                                drr["AltUnit"] = AltUnit;
                                drr["StockUnitQnty"] = StockUnitQnty;
                                drr["AltUnitQnty"] = AltUnitQnty;
                                drr["ProductID"] = ProductDetailsForId;
                                drr["ClassId"] = ClassId;
                                drr["BrandId"] = BrandId;
                                drr["StockUnitId"] = StockUnitId;
                                drr["AltUnitId"] = AltUnitId;
                                drr["CLOSE_QTY"] = CLOSE_QTY;
                                drr["ALTCLOSE_QTY"] = ALTCLOSE_QTY;
                                drr["Price"] = Price;
                                drr["DiffStockUnitQnty"] = DiffStockUnitQnty;
                                drr["DiffAltUnitQnty"] = DiffAltUnitQnty;
                                break;
                            }
                        }

                        if (Isexists == false)
                        {
                            Quotationdt.Rows.Add(SrlNo, ProductdisID, Product, Description,sProduct_Status, Class, StockUnit, AltUnit, StockUnitQnty, AltUnitQnty, ProductDetailsForId, ClassId, BrandId, StockUnitId, AltUnitId, CLOSE_QTY, ALTCLOSE_QTY, Price , DiffStockUnitQnty, DiffAltUnitQnty);
                        }
                    }
                }
            }
            
            Quotationdt.AcceptChanges();

            Session["BindProduct_Details"] = Quotationdt;
            int strIsComplete = 0;
            string strText = "";
            if (IsDeleteFrom == "I")
            {
                Int64 warehouseid = Convert.ToInt64(CmbWarehouse.Value);
                ModifystockSheet(Quotationdt, warehouseid, ref strIsComplete, ref strText);
                if (strIsComplete == 1)
                {
                   // Session["BindProduct_CalcuCommitDetails"] = Session["BindProduct_Details"];
                    Session["BindProduct_Details"] = null;
                    grid.JSProperties["cpSuccess"] = "Success";
                    grid.JSProperties["cpDocId"] = strText;
                }
            }

        }
        public void ModifystockSheet(DataTable Quotationdt, Int64 warehouseid, ref int strIsComplete, ref string strText)
        {
            try
            {
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                if (Quotationdt.Columns.Contains("Product"))
                {
                    Quotationdt.Columns.Remove("Product");
                    Quotationdt.AcceptChanges();
                }
                if (Quotationdt.Columns.Contains("Description"))
                {
                    Quotationdt.Columns.Remove("Description");
                    Quotationdt.AcceptChanges();
                }
                if (Quotationdt.Columns.Contains("sProduct_Status"))
                {
                    Quotationdt.Columns.Remove("sProduct_Status");
                    Quotationdt.AcceptChanges();
                }
                if (Quotationdt.Columns.Contains("Class"))
                {
                    Quotationdt.Columns.Remove("Class");
                    Quotationdt.AcceptChanges();
                }
                if (Quotationdt.Columns.Contains("StockUnit"))
                {
                    Quotationdt.Columns.Remove("StockUnit");
                    Quotationdt.AcceptChanges();
                }
                if (Quotationdt.Columns.Contains("AltUnit"))
                {
                    Quotationdt.Columns.Remove("AltUnit");
                    Quotationdt.AcceptChanges();
                }
                if (Quotationdt.Columns.Contains("ProductID"))
                {
                    Quotationdt.Columns.Remove("ProductID");
                    Quotationdt.AcceptChanges();
                }

                SqlCommand cmd = new SqlCommand("Prc_AddReconcilation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "SaveReconcile");
                cmd.Parameters.AddWithValue("@ReconcileStock", Quotationdt);
                cmd.Parameters.AddWithValue("@NumberSchemaId", NumId.Value);
                cmd.Parameters.AddWithValue("@Doc_no", txt_CustDocNo.Text);
                //cmd.Parameters.AddWithValue("@PostingDate", dt_PLQuote.Date.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@PostingDate", dt_PLQuote.Date);
                cmd.Parameters.AddWithValue("@WarehouseId", warehouseid);
                cmd.Parameters.AddWithValue("@Remarks", txtRemarks.Value);
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToInt32(Session["userid"]));

                cmd.Parameters.Add("@ReturnText", SqlDbType.VarChar, 50);
                cmd.Parameters["@ReturnText"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
                cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;

                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);

                strIsComplete = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());
                strText = Convert.ToString(cmd.Parameters["@ReturnText"].Value.ToString());
                cmd.Dispose();
                con.Dispose();
            }
            catch (Exception ex)
            {

            }
        }
        protected void ReconcileStockAdjusted(object sender, EventArgs e)
        {
            Int32 IsComplete = 0;
            Int64 warehouseid = Convert.ToInt64(CmbWarehouse.Value);
            Int64 ReconcileId = Convert.ToInt64(hdnReconcileId.Value);
            DataTable dt = GetReconcileListForAdj(ReconcileId);
            try
            {
                DataSet dsInst = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                SqlCommand cmd = new SqlCommand("Prc_ReconcileStockAdjSave", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userId", Convert.ToInt32(Session["userid"]));
                cmd.Parameters.AddWithValue("@CompanyID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FinYear" ,Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@ReconcileStkAdj", dt);
                cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
                cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                IsComplete = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());
                cmd.Dispose();
                con.Dispose();
            }
            catch (Exception ex)
            {

            }

            if (IsComplete == 1)
            {
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "jAlert('Posting Successfully Completed.')", true);
                Page.ClientScript.RegisterStartupScript(GetType(), "JScript3", "<script language='javascript'>StockAdjustComplete();</script>");
               // Response.Redirect("stockReconcileList.aspx");
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "jAlert('Please try again later.')", true);
            }

        }



        //protected void ReconcileStockAdjustedCancel(object sender, EventArgs e)
        //{
        //    Response.Redirect("stockReconcileList.aspx");
        //}
        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {

            string strSplitCommand = e.Parameters.Split('~')[0];
            if (strSplitCommand == "Display")
            {
                string IsDeleteFrom = "";
                //Convert.ToString(hdfIsDelete.Value);
                if (IsDeleteFrom == "D")
                {
                    DataTable Quotationdt = (DataTable)Session["BindProduct_Details"];
                    grid.DataSource = GetQuotation(Quotationdt);
                    grid.DataBind();
                }
            }
            else if (strSplitCommand == "QuantityZero")
            {
                DataTable dt = (DataTable)Session["BindProduct_Details"];
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["StockUnitQnty"] = "0";
                        dr["AltUnitQnty"] = "0";
                    }

                    dt.AcceptChanges();
                }
                Session["BindProduct_Details"] = dt;
               // grid.DataSource = GetQuotation(dt);
                grid.DataBind();
            }

            else if (strSplitCommand == "BindGrid")
            {
                DataTable dt = new DataTable();               
                string warehouseId = Convert.ToString(CmbWarehouse.Value);
               // DateTime PhyStockDate = Convert.ToDateTime(dt_PLQuote.Date);
                dt = GetGridProductDetailsBind(warehouseId);               
                Session["BindProduct_Details"] = dt;
                grid.DataSource = GetQuotation(dt);
                grid.DataBind();

            }
        }


        protected void ReconcileStkAdjustmentExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdStkAdj.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["Calculateexport"] == null)
                {
                    Session["Calculateexport"] = Filter;
                    bindReconcileStkAdjustmentexport(Filter);
                }
                else if (Convert.ToInt32(Session["Calculateexport"]) != Filter)
                {
                    Session["Calculateexport"] = Filter;
                    bindReconcileStkAdjustmentexport(Filter);
                }
            }
        }
        public void bindReconcileStkAdjustmentexport(int Filter)
        {
            //GrdQuotation.Columns[6].Visible = false;
            GrdQuotation.Columns[0].Visible = false;
            string filename = "Reconcile Stock AdjustmentList";
            Calculateexport.FileName = filename;
            Calculateexport.FileName = "StockAdjustmentList";
            Calculateexport.PageHeader.Left = "Reconcile Stock AdjustmentList";
            Calculateexport.PageFooter.Center = "[Page # of Pages #]";
            Calculateexport.PageFooter.Right = "[Date Printed]";

            switch (Filter)
            {
                case 1:
                    Calculateexport.WritePdfToResponse();
                    break;
                case 2:
                    Calculateexport.WriteXlsToResponse();
                    break;
                case 3:
                    Calculateexport.WriteRtfToResponse();
                    break;
                case 4:
                    Calculateexport.WriteCsvToResponse();
                    break;
            }
        }
        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {

            e.KeyExpression = "sProducts_ID";
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            var q = from d in dc.V_ReconcileAdjustmentLists
                    where d.Reconcile_Id == Convert.ToInt64(hdnReconcileId.Value)
                    //d.Invoice_Date >= Convert.ToDateTime(strFromDate) && d.Invoice_Date <= Convert.ToDateTime(strToDate)
                    //&& branchidlist.Contains(Convert.ToInt32(d.BranchID)) && d.InvoiceFor == "CL" && d.CompanyID == CompanyID
                    //&& d.FinYear == FinYear
                    //&& d.CreatedBy == userid
                    //orderby d.Invoice_Date descending
                    select d;
            e.QueryableSource = q;
        }
        protected void grid_DataBinding(object sender, EventArgs e)
        {
            if (Session["BindProduct_Details"] != null)
            {
                DataTable Quotationdt = (DataTable)Session["BindProduct_Details"];
                DataView dvData = new DataView(Quotationdt);
                grid.DataSource = GetQuotation(dvData.ToTable());
            }
        }
        public IEnumerable GetQuotation(DataTable Quotationdt)
        {
            List<ReconcileStock> QuotationList = new List<ReconcileStock>();
            if (Quotationdt != null && Quotationdt.Rows.Count > 0)
            {
                for (int i = 0; i < Quotationdt.Rows.Count; i++)
                {
                    ReconcileStock Quotations = new ReconcileStock();
                    Quotations.SrlNo = Convert.ToString(Quotationdt.Rows[i]["SrlNo"]);
                    Quotations.sProducts_ID = Convert.ToInt64(Quotationdt.Rows[i]["sProducts_ID"]);
                    Quotations.Product = Convert.ToString(Quotationdt.Rows[i]["Product"]);
                    Quotations.Description = Convert.ToString(Quotationdt.Rows[i]["Description"]);
                    Quotations.sProduct_Status = Convert.ToString(Quotationdt.Rows[i]["sProduct_Status"]);
                    if (Quotationdt.Columns.Contains("Class"))
                        Quotations.Class = Convert.ToString(Quotationdt.Rows[i]["Class"]);
                    if (Quotationdt.Columns.Contains("StockUnit"))
                        Quotations.StockUnit = Convert.ToString(Quotationdt.Rows[i]["StockUnit"]);
                    if (Quotationdt.Columns.Contains("AltUnit"))
                        Quotations.AltUnit = Convert.ToString(Quotationdt.Rows[i]["AltUnit"]);
                    Quotations.StockUnitQnty = Convert.ToString(Quotationdt.Rows[i]["StockUnitQnty"]);
                    Quotations.AltUnitQnty = Convert.ToString(Quotationdt.Rows[i]["AltUnitQnty"]);
                    if (Quotationdt.Columns.Contains("ProductID"))
                        Quotations.ProductID = Convert.ToString(Quotationdt.Rows[i]["ProductID"]);
                    Quotations.ClassId = Convert.ToInt64(Quotationdt.Rows[i]["ClassId"]);
                    Quotations.BrandId = Convert.ToInt64(Quotationdt.Rows[i]["BrandId"]);
                    Quotations.StockUnitId = Convert.ToInt64(Quotationdt.Rows[i]["StockUnitId"]);
                    Quotations.AltUnitId = Convert.ToInt64(Quotationdt.Rows[i]["AltUnitId"]);
                    Quotations.CLOSE_QTY = Convert.ToString(Quotationdt.Rows[i]["CLOSE_QTY"]);
                    Quotations.ALTCLOSE_QTY = Convert.ToString(Quotationdt.Rows[i]["ALTCLOSE_QTY"]);
                    Quotations.Price = Convert.ToString(Quotationdt.Rows[i]["Price"]);
                    Quotations.DiffStockUnitQnty = Convert.ToString(Quotationdt.Rows[i]["DiffStockUnitQnty"]);
                    Quotations.DiffAltUnitQnty = Convert.ToString(Quotationdt.Rows[i]["DiffAltUnitQnty"]);               

                    QuotationList.Add(Quotations);
                }
            }
            return QuotationList;
        }

        public DataTable GetGridProductDetailsBind(string warehouseId)
        {
            string _Date = dt_PLQuote.Date.ToString("yyyy-MM-dd");
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_ReconcileStock");
            proc.AddVarcharPara("@Action", 500, "GetDataForReconcile");
            proc.AddVarcharPara("@COMPANYID", 500, Convert.ToString(Session["LastCompany"]));
            proc.AddVarcharPara("@FINYEAR", 500, Convert.ToString(Session["LastFinYear"]));
            //proc.AddVarcharPara("@TODATE", 500, Convert.ToString(dt_PLQuote.Date));
            proc.AddVarcharPara("@TODATE", 500, _Date);           
            proc.AddVarcharPara("@WAREHOUSE_ID", 500, warehouseId);
            dt = proc.GetTable();
            return dt;
        }

        public DataSet GetEditProductDetailsBind(Int64 strReconcileId)
        {
            DataSet dt = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Prc_ReconcileStock");
            proc.AddVarcharPara("@Action", 500, "ReconcileStockEdit");
            proc.AddBigIntegerPara("@Reconcile_Id", strReconcileId);
            dt = proc.GetDataSet();
            return dt;
        }
        public DataTable GetReconcileListForAdj(Int64 Reconcile_Id)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_ReconcileStock");
            proc.AddVarcharPara("@Action", 500, "GetReconcileStockForAdjustment");
            proc.AddBigIntegerPara("@Reconcile_Id", Reconcile_Id);
            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetWarehouseData()
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_GetProductForStock");
            proc.AddVarcharPara("@Action", 500, "GetWareHouse");

            dt = proc.GetTable();
            return dt;
        }
        public class ReconcileStock
        {
            public string SrlNo { get; set; }
            public Int64 sProducts_ID { get; set; }
            public string Product { get; set; }
            public string Description { get; set; }
            public string Class { get; set; }
            public string StockUnit { get; set; }
            public string AltUnit { get; set; }
            public string StockUnitQnty { get; set; }
            public string AltUnitQnty { get; set; }
            public string ProductID { get; set; }
            public string CLOSE_QTY { get; set; }
            public string ALTCLOSE_QTY { get; set; }
            public string Price { get; set; }
            public string DiffStockUnitQnty { get; set; }
            public string DiffAltUnitQnty { get; set; }     

            public Int64 ClassId { get; set; }
            public Int64 BrandId { get; set; }
            public Int64 StockUnitId { get; set; }
            public Int64 AltUnitId { get; set; }
            public Int64 StockId { get; set; }
            public string sProduct_Status { get; set; }         


        }

    }
}