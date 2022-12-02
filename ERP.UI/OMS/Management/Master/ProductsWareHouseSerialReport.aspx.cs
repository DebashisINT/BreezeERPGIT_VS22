using DataAccessLayer;
using DevExpress.Web;
using DevExpress.Web.Data;
using EntityLayer.CommonELS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using DevExpress.XtraGrid.Views.Grid;

namespace ERP.OMS.Management.Master
{
    public partial class ProductsWareHouseSerialReport : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public static EntityLayer.CommonELS.UserRightsForPage rights;
        private string branchesid = string.Empty;
        private string Financialyear = string.Empty;
        int branchwisestock = 0;

        #region Page Event
        protected void Page_Init(object sender, EventArgs e)
        {
            //((GridViewDataComboBoxColumn)OpeningGrid.Columns["branch"]).PropertiesComboBox.DataSource = Cmblevel1();

            if (!IsPostBack)
            {
              
               // OpeningGrid.OptionsView.AllowCellMerge = true;
                OpeningGrid.DataBind();

            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {
                Session["exportval"] = null;
                Session["Openingexportval"] = null;

                branchesid = Convert.ToString(Session["userbranchHierarchy"]);
                rights = new UserRightsForPage();
                rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/ProductsOpeningEntries.aspx");

                cmbbranch.DataSource = Cmblevel1();
                cmbbranch.DataBind();
                cmbbranch.Value = Convert.ToString(Session["userbranchID"]);
                hdnselectedbranch.Value = Convert.ToString(Session["userbranchID"]);
            }


        }

        #endregion

        #region privateEvent
        private IEnumerable GetGriddata(out bool shoulddisplay)
        {

            List<Openinglist> approvallist = new List<Openinglist>();
            bool shoulddisplays = false;

            DataTable dt = new DataTable();


            try
            {
                if (hdnselectedbranch.Value != "00")
                {
                    //DataTable DTs = oDBEngine.GetDataTable("exec prc_StockInsertOpeingEntrys @Comapny='" + Convert.ToString(Session["LastCompany"]) + "',@Finyear='" + Convert.ToString(Session["LastFinYear"]) + "',@OwnBranch=" + Convert.ToString(Session["userbranchID"]) + "");


                    DataSet dsInst = new DataSet();
                    //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
                    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                    SqlCommand cmd = new SqlCommand("prc_GetStockWarehouseSerialReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    //For multiple branches data
                    if (Convert.ToString(Session["userbranchHierarchy"]).Contains(","))
                    {
                        cmd.Parameters.AddWithValue("@Action", 0);

                        cmd.Parameters.AddWithValue("@Branches", Convert.ToString(Session["userbranchHierarchy"]));
                    }
                    //For single branches data
                    else
                    {
                        cmd.Parameters.AddWithValue("@Action", 1);

                        cmd.Parameters.AddWithValue("@Branches", Convert.ToString(Session["userbranchHierarchy"]));
                    }
                    if (!string.IsNullOrEmpty(hdnselectedbranch.Value) && hdnselectedbranch.Value != "00" && hdnselectedbranch.Value != "0")
                    {
                        cmd.Parameters.AddWithValue("@BranchIDwise", Convert.ToString(hdnselectedbranch.Value));
                    }
                    else if (hdnselectedbranch.Value == "00")
                    {
                        cmd.Parameters.AddWithValue("@BranchIDwise", "0");
                    }
                    else
                    {

                        cmd.Parameters.AddWithValue("@BranchIDwise", Convert.ToString(Session["userbranchID"]));
                    }

                    cmd.Parameters.AddWithValue("@Comapny", Convert.ToString(Session["LastCompany"]));
                    cmd.Parameters.AddWithValue("@Finyear", Convert.ToString(Session["LastFinYear"]));
                    cmd.Parameters.AddWithValue("@OwnBranch", Convert.ToString(Session["userbranchID"]));

                    cmd.CommandTimeout = 0;
                    SqlDataAdapter Adap = new SqlDataAdapter();
                    Adap.SelectCommand = cmd;
                    Adap.Fill(dsInst);
                    cmd.Dispose();
                    con.Dispose();

                    if (dsInst.Tables != null)
                    {
                        dt = dsInst.Tables[0];


                    }
                }
                //return true;
            }
            catch (Exception ex)
            {
                //return false;
            }


            int newid = 1;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Openinglist Vouchers = new Openinglist();
              
                    Vouchers.uniqids = newid;
                    Vouchers.Stock_ID = newid; 

                    Vouchers.ProductID = Convert.ToString(dt.Rows[i]["ProductID"]);
                    Vouchers.ProductCode = Convert.ToString(dt.Rows[i]["ProductCode"]);
                    Vouchers.ProductName2 = Convert.ToString(dt.Rows[i]["ProductName"]);                  

                    Vouchers.ProductName = Convert.ToString(dt.Rows[i]["ProductName"]).Replace("'", "squot");
                    Vouchers.ProductName = Vouchers.ProductName.Replace(",", "coma");
                    Vouchers.ProductName = Vouchers.ProductName.Replace("/", "slash");

                    Vouchers.branch = "";                  

                    Vouchers.avilablestock = Convert.ToDecimal(dt.Rows[i]["avilablestock"]);                  
                    Vouchers.OpeningStock = Convert.ToDecimal(dt.Rows[i]["OpeningStock"]);                  
                    Vouchers.warehouseid = Convert.ToString(dt.Rows[i]["warehouseid"]);
                    Vouchers.bui_Name = Convert.ToString(dt.Rows[i]["bui_Name"]);
                    Vouchers.StockBranchWarehouse_StockIn = Convert.ToDecimal(dt.Rows[i]["StockBranchWarehouse_StockIn"]);
                    Vouchers.Batch = Convert.ToString(dt.Rows[i]["Batch"]);
                    Vouchers.Serial = Convert.ToString(dt.Rows[i]["Serial"]);
                    Vouchers.branch_description = Convert.ToString(dt.Rows[i]["branch_description"]);
                    Vouchers.ProductClass_Name = Convert.ToString(dt.Rows[i]["ProductClass_Name"]);
                    Vouchers.Brand_Name = Convert.ToString(dt.Rows[i]["Brand_Name"]);
                    Vouchers.Rate = Convert.ToDecimal(dt.Rows[i]["Rate"]);
                    Vouchers.OPVALUE = Convert.ToDecimal(dt.Rows[i]["OPVALUE"]);

                    approvallist.Add(Vouchers);
                    newid++;              
            }

            shoulddisplay = shoulddisplays;

            return approvallist;

        }

        private IEnumerable Cmblevel1()
        {
            List<branches> LevelList = new List<branches>();

            DataTable DT = oDBEngine.GetDataTable("select branch_id,branch_description from tbl_master_branch where branch_id in(" + Convert.ToString(Session["userbranchHierarchy"]) + ")  order by branch_description asc");
            branches Levelss = new branches();
            Levelss.branchID = "00";
            Levelss.branchName = " --Select-- ";

            LevelList.Add(Levelss);

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                branches Levels = new branches();

                Levels.branchID = Convert.ToString(DT.Rows[i]["branch_id"]);
                Levels.branchName = Convert.ToString(DT.Rows[i]["branch_description"]);



                LevelList.Add(Levels);
            }


            return LevelList;
        }

        private void Executedata(DataTable dt, string finyear, string company)
        {
            try
            {
                DataSet dsInst = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_InsertOrUpdateOpeningEntries", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompanyID", company);
                cmd.Parameters.AddWithValue("@FinYear", finyear);
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(Session["userid"]));

                cmd.Parameters.AddWithValue("@OpeningentryDetails", dt);


                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);
                cmd.Dispose();
                con.Dispose();

                //return true;
            }
            catch (Exception ex)
            {
                //return false;
            }

        }
        //protected void OpeningGrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        //{
        //    if (e.DataColumn.FieldName != "ContactTitle")
        //        return;
        //    string position = Convert.ToString(e.CellValue);
        //    ASPxImage positionIcon = (ASPxImage)OpeningGrid.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, "PositionIcon");
        //    positionIcon.Caption = position;
        //    positionIcon.EmptyImage.IconID = GetIconIDByPosition(position);
        //}
        private bool ISwarehouseexistOrnot(string stockid, string productid, string branchid, string OpeningStock)
        {
            bool Exist = false;

            DataTable DTs = oDBEngine.GetDataTable("select tw.StockBranchWarehouse_StockId as stockid,dbo.checkOpeningStockBranchwise(tw.StockBranchWarehouse_BranchId,'" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]) + "',tw.StockBranchWarehouse_StockId) as OpeningStock from Trans_StockBranchWarehouse tw join Trans_StockBranchWarehouseDetails trwdt on tw.StockBranchWarehouse_Id=trwdt.StockBranchWarehouse_Id where tw.StockBranchWarehouse_StockId=" + stockid + " and tw.StockBranchWarehouse_BranchId=" + branchid + " and tw.StockBranchWarehouse_CompanyId='" + Convert.ToString(Session["LastCompany"]) + "' and  tw.StockBranchWarehouse_FinYear='" + Convert.ToString(Session["LastFinYear"]) + "' and trwdt.StockBranchWarehouseDetail_ProductId=" + productid + "");
            if (DTs.Rows.Count > 0)
            {
                //decimal addopen = Convert.ToDecimal(DTs.Rows[0]["OpeningStock"]) + Convert.ToDecimal(OpeningStock);
                //if (Convert.ToDecimal(DTs.Rows[0]["OpeningStock"]) == addopen && !string.IsNullOrEmpty(Convert.ToString(DTs.Rows[0]["stockid"])))
                if (!string.IsNullOrEmpty(Convert.ToString(DTs.Rows[0]["stockid"])))
                {
                    Exist = true;
                }

            }
            return Exist;

        }

        private bool IsWarehouseBatchandSerialEnable(string productID)
        {
            bool Exist = false;

            DataTable DTs = oDBEngine.GetDataTable("select ps.Is_active_Batch as Isbatch,ps.Is_active_serialno as Isserial,ps.Is_active_warehouse as Iswarehouse from Master_sProducts ps where sProducts_ID=" + productID + "");
            if (DTs.Rows.Count > 0)
            {

                if (Convert.ToString(DTs.Rows[0]["Isbatch"]) == "True" || Convert.ToString(DTs.Rows[0]["Isserial"]) == "True" || Convert.ToString(DTs.Rows[0]["Iswarehouse"]) == "True")
                {
                    Exist = true;
                }

            }
            return Exist;
        }
        private bool ISwarehouseexistOrnotbystock(string stockid)
        {
            bool Exist = false;

            if (!string.IsNullOrEmpty(stockid))
            {
                DataTable DTs = oDBEngine.GetDataTable("select ps.Is_active_Batch as isbatch,ps.Is_active_serialno as Isserial, ps.Is_active_warehouse as iswarehouse from Master_sProducts ps join Trans_Stock ts on ps.sProducts_ID=ts.Stock_ProductID where ts.Stock_ID=" + stockid + "");
                if (DTs.Rows.Count > 0)
                {
                    if (Convert.ToString(DTs.Rows[0]["isbatch"]).ToLower() == "true" || Convert.ToString(DTs.Rows[0]["Isserial"]).ToLower() == "true" || Convert.ToString(DTs.Rows[0]["iswarehouse"]).ToLower() == "true")
                    {
                        Exist = true;
                    }
                }
            }
            return Exist;

        }

        #endregion

        #region class
        public class Openinglist
        {
            public int uniqids { get; set; }
            public int Stock_ID { get; set; }
            public string ProductID { get; set; }
            public string ProductCode { get; set; }

            public string ProductName { get; set; }
            public string ProductName2 { get; set; }
            public string branch { get; set; }
            public string UOM { get; set; }
            public string Stockdetails { get; set; }
            public decimal OpeningStock { get; set; }
            public decimal itemsvalue { get; set; }
            public decimal itemrate { get; set; }

            public String isactivebatch { get; set; }
            public String isactiveserial { get; set; }

            public string Is_active_warehouse { get; set; }
            public int UOMID { get; set; }

            public String warehouseid { get; set; }

            public string ALLrequiredpara { get; set; }

            public decimal avilablestock { get; set; }
            public decimal outstock { get; set; }

            public string bui_Name { get; set; }
            public decimal StockBranchWarehouse_StockIn { get; set; }
            public string Batch { get; set; }
            public string Serial { get; set; }
            public string branch_description { get; set; }

            public string ProductClass_Name { get; set; }
            public string Brand_Name { get; set; }
            public decimal Rate { get; set; }
            public decimal OPVALUE { get; set; }
        }

        public class branches
        {
            public string branchID { get; set; }
            public string branchName { get; set; }
        }


        #endregion

        #region grid Event

        protected void OpeningGrid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];
            if (strSplitCommand == "branchwise")
            {
                string branchid = e.Parameters.Split('~')[1];
                bool shoulddisplay = false;
                OpeningGrid.DataSource = GetGriddata(out shoulddisplay);
                OpeningGrid.DataBind();

             // new ASPxGridViewCellMerger(OpeningGrid);

                //OpeningGrid.Columns[0].FixedStyle = GridViewColumnFixedStyle.Left;
               // OpeningGrid.Columns[1].FixedStyle = GridViewColumnFixedStyle.Left;

                //OpeningGrid.Columns[0].CellStyle.BackColor = Color.FromArgb(0xEE, 0xEE, 0xEE);
                //OpeningGrid.Columns[1].CellStyle.BackColor = Color.FromArgb(0xEE, 0xEE, 0xEE);

                //OpeningGrid.Settings.ShowHorizontalScrollBar = true;


                if (shoulddisplay == true)
                {
                    OpeningGrid.JSProperties["cpsaveenableornot"] = Convert.ToString("enable");
                }
                else
                {
                    OpeningGrid.JSProperties["cpsaveenableornot"] = Convert.ToString("disable");
                }
            }

        }
        protected void OpeningGrid_DataBinding(object sender, EventArgs e)
        {
            bool shouldenable = false;
            OpeningGrid.DataSource = GetGriddata(out shouldenable);

            if (shouldenable == true)
            {
                OpeningGrid.JSProperties["cpsaveenableornot"] = Convert.ToString("enable");
            }
            else
            {
                OpeningGrid.JSProperties["cpsaveenableornot"] = Convert.ToString("disable");
            }


        }
        protected void OpeningGrid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            DataTable Quotationdt = new DataTable();
            bool isexist = false;
            bool Iswarehousorbatchorserialenable = false;

            Quotationdt.Columns.Add("SrlNo", typeof(Int32));
            Quotationdt.Columns.Add("Stock_ID", typeof(string));
            Quotationdt.Columns.Add("ProductID", typeof(string));
            Quotationdt.Columns.Add("branch", typeof(string));
            Quotationdt.Columns.Add("OpeningStock", typeof(string));
            Quotationdt.Columns.Add("itemrate", typeof(string));
            Quotationdt.Columns.Add("itemsvalue", typeof(string));
            Quotationdt.Columns.Add("UOMID", typeof(string));

            foreach (var args in e.UpdateValues)
            {
                string Stock_ID = Convert.ToString(args.Keys["Stock_ID"]);
                string SrlNo = Convert.ToString(args.NewValues["uniqids"]);

                string ProductID = Convert.ToString(args.NewValues["ProductID"]);
                string OpeningStock = Convert.ToString(args.NewValues["OpeningStock"]);
                //string branch = Convert.ToString(args.NewValues["branch"]);
                string branch = Convert.ToString(cmbbranch.SelectedItem.Value);
                //if (string.IsNullOrEmpty(branch))
                //{
                //    branch = Convert.ToString(Session["userbranchID"]);
                //}
                string itemrate = Convert.ToString(args.NewValues["itemrate"]);
                string itemsvalue = Convert.ToString(args.NewValues["itemsvalue"]);
                string UOMID = Convert.ToString(args.NewValues["UOMID"]);

                if ((OpeningStock != "0.0" || OpeningStock != "0.00" || OpeningStock != "000000000.00000" || OpeningStock != "0.0000" || OpeningStock != "0") && branch != "0")
                {
                    Iswarehousorbatchorserialenable = IsWarehouseBatchandSerialEnable(ProductID);
                    if (Iswarehousorbatchorserialenable)
                    {
                        isexist = ISwarehouseexistOrnot(Stock_ID, ProductID, branch, OpeningStock);
                    }
                    else
                    {
                        isexist = true;
                    }
                    if (isexist)
                    {
                        Quotationdt.Rows.Add(Quotationdt.Rows.Count + 1, Stock_ID, ProductID, branch, OpeningStock, itemrate, itemsvalue, UOMID);

                    }
                    else
                    {
                        OpeningGrid.JSProperties["cpupdatemssg"] = Convert.ToString("You must Save warehouse details first before update the stock.");
                        //e.Handled = true;
                        //ASPxGridView grid = sender as ASPxGridView;
                        //grid.CancelEdit();
                        //return;
                    }
                }
                else
                {
                    OpeningGrid.JSProperties["cpupdatemssg"] = Convert.ToString("Quantity should not be zero.");
                }

                //else { break; }

            }

            if (Quotationdt.Rows.Count > 0)
            {
                string Financialyear = Convert.ToString(Session["LastFinYear"]);
                string companyname = Convert.ToString(Session["LastCompany"]);


                Executedata(Quotationdt, Financialyear, companyname);
                OpeningGrid.JSProperties["cpupdatemssg"] = Convert.ToString("Saved Successfully.");
            }

            else if (!isexist)
            {
                OpeningGrid.JSProperties["cpupdatemssg"] = Convert.ToString("You must Save warehouse details first before update the stock.");

            }
            else
            {
                OpeningGrid.JSProperties["cpupdatemssg"] = Convert.ToString("Nothing to Save.");
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
        //public string SetVisibility(object container)
        //{
        //    string vs = string.Empty;
        //    GridViewDataItemTemplateContainer c = container as GridViewDataItemTemplateContainer;
        //    if (!string.IsNullOrEmpty(Convert.ToString(c.KeyValue)))
        //    {
        //        bool isexist = ISwarehouseexistOrnotbystock(Convert.ToString(c.KeyValue));
        //        if (isexist)
        //        {
        //            vs = "display:block";
        //        }
        //        else
        //        {
        //            vs = "display:none";
        //        }

        //    }
        //    else
        //    {
        //        vs = "display:block";
        //    }

        //    return vs;


        //}
        protected void openingGridExport_DataBinding(object sender, EventArgs e)
        {
            openingGridExport.DataSource = GetOpeningStockDetailsForExport();
        }
        public DataTable GetOpeningStockDetailsForExport()
        {
            try
            {
                string BranchID = Convert.ToString(cmbbranch.Value);
                string CompanyID = Convert.ToString(Session["LastCompany"]);
                string FinYear = Convert.ToString(Session["LastFinYear"]);

                DataTable dt = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_GetOpeningStockEntrys");
                proc.AddVarcharPara("@Action", 3000, "OpeningDataForExport");
                proc.AddVarcharPara("@BranchID", 3000, BranchID);
                proc.AddVarcharPara("@CompanyID", 3000, CompanyID);
                proc.AddVarcharPara("@FinYear", 3000, FinYear);
                dt = proc.GetTable();
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region Export event

        public void bindexport(int Filter)
        {
            OpeningGrid.Columns[0].Visible = false;
            OpeningGrid.Columns[1].Visible = false;
           // OpeningGrid.Columns[7].Visible = false;

            string strBranch = "";
            if (cmbbranch.Value != "00") strBranch = "( " + Convert.ToString(cmbbranch.Text) + " )";

            string filename = "ProductStock " + strBranch;
            Openingexporter.FileName = filename;

            exporter.PageHeader.Left = "Product Stock Status";
            exporter.PageFooter.Center = "[Page # of Pages #]";
            exporter.PageFooter.Right = "[Date Printed]";
         

            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsToResponse();
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }
        }
        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
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
        protected void ddlExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(ddlExport.SelectedItem.Value));

            if (Filter != 0)
            {
                if (Session["Openingexportval"] == null)
                {
                    Session["Openingexportval"] = Filter;
                    bindOpeningexport(Filter);
                }
                else if (Convert.ToInt32(Session["Openingexportval"]) != Filter)
                {
                    Session["Openingexportval"] = Filter;
                    bindOpeningexport(Filter);
                }
            }
        }
        public void bindOpeningexport(int Filter)
        {
            string strBranch="";
            if(cmbbranch.Value!="00") strBranch="( "+Convert.ToString(cmbbranch.Text)+" )";

            string filename = "Opening_ProductStock " + strBranch;
            Openingexporter.FileName = filename;

            Openingexporter.PageHeader.Left = "Product Stock Status";
            Openingexporter.PageFooter.Center = "[Page # of Pages #]";
            Openingexporter.PageFooter.Right = "[Date Printed]";

            Session["Openingexportval"] = null;

            Openingexporter.GridViewID = "openingGridExport";
            switch (Filter)
            {
                case 1:
                    Openingexporter.WritePdfToResponse();
                    break;
                case 2:
                    Openingexporter.WriteXlsToResponse();
                    break;
                case 3:
                    Openingexporter.WriteRtfToResponse();
                    break;
                case 4:
                    Openingexporter.WriteCsvToResponse();
                    break;
            }
        }

        #endregion

        #region popup
        protected void GrdWarehouse_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            string strSplitCommand = e.Parameters.Split('~')[0];


            //if (hdniswarehouse.Value == "true")
            //{
            //    GrdWarehouse.Columns["viewWarehouseName"].Visible = true;
            //    GrdWarehouse.Columns["Quantity"].Visible = true;
            //    GrdWarehouse.Columns["viewQuantity"].Visible = false;

            //}
            //else
            //{
            //    GrdWarehouse.Columns["viewWarehouseName"].Visible = false;
            //    GrdWarehouse.Columns["Quantity"].Visible = false;
            //    GrdWarehouse.Columns["viewQuantity"].Visible = true;
            //}
            //if (hdnisbatch.Value == "true")
            //{
            //    GrdWarehouse.Columns["viewBatchNo"].Visible = true;
            //    GrdWarehouse.Columns["viewMFGDate"].Visible = true;
            //    GrdWarehouse.Columns["viewExpiryDate"].Visible = true;
            //    GrdWarehouse.Columns["Quantity"].Visible = false;
            //    GrdWarehouse.Columns["viewQuantity"].Visible = true;

            //}
            //else
            //{
            //    GrdWarehouse.Columns["viewBatchNo"].Visible = false;
            //    GrdWarehouse.Columns["viewMFGDate"].Visible = false;
            //    GrdWarehouse.Columns["viewExpiryDate"].Visible = false;
            //    GrdWarehouse.Columns["Quantity"].Visible = true;
            //    GrdWarehouse.Columns["viewQuantity"].Visible = false;
            //}
            //if (hdnisserial.Value == "true")
            //{
            //    GrdWarehouse.Columns["viewSerialNo"].Visible = true;
            //    GrdWarehouse.Columns["Quantity"].Visible = false;
            //    GrdWarehouse.Columns["viewQuantity"].Visible = true;
            //}
            //else
            //{
            //    GrdWarehouse.Columns["viewSerialNo"].Visible = false;
            //}

            //if (strSplitCommand == "Delete")
            //{

            //    updateRateValuedata(hdfProductID.Value, hdfstockid.Value, hdnselectedbranch.Value, "delete");
            //}
            //#region Savenew
            //if (strSplitCommand == "Display")
            //{
            //    string WarehouseName = Convert.ToString(e.Parameters.Split('~')[2]);

            //    string WarehouseID = Convert.ToString(e.Parameters.Split('~')[1]).Trim();

            //    string BatchName = Convert.ToString(e.Parameters.Split('~')[3]);

            //    string SerialName = Convert.ToString(e.Parameters.Split('~')[4]);
            //    string ProductID = Convert.ToString(hdfProductID.Value);
            //    string stockid = Convert.ToString(hdfstockid.Value);
            //    decimal openingstock = Convert.ToDecimal(txtqnty.Text);
            //    string branchid = Convert.ToString(hdbranchID.Value);
            //    string qnty = Convert.ToString(e.Parameters.Split('~')[5]);

            //    decimal totalopeining = Convert.ToDecimal(hdfopeningstock.Value);
            //    decimal oldtotalopeining = Convert.ToDecimal(oldopeningqntity.Value);

            //    if (qnty == "0.0000" && openingstock <= 0)
            //    {
            //        qnty = batchqnty.Text;
            //        openingstock = Convert.ToDecimal(qnty);
            //    }

            //    if (!string.IsNullOrEmpty(BatchName))
            //    {
            //        BatchName = BatchName.Trim();
            //    }
            //    if (!string.IsNullOrEmpty(SerialName))
            //    {
            //        SerialName = SerialName.Trim();
            //    }
            //    if (WarehouseID == "null")
            //    {
            //        WarehouseID = "0";
            //    }

            //    if (Convert.ToDecimal(openingstock) > totalopeining)
            //    {
            //        GrdWarehouse.JSProperties["cpupdatemssg"] = Convert.ToString("You have entered Quantity greater than Opening Quantity. Cannot Proceed.");

            //    }
            //    else
            //    {
            //        DataTable Warehousedt = new DataTable();
            //        DataTable Warehousedtnew = new DataTable();
            //        if (Session["OpenWarehouseData"] != null)
            //        {
            //            Warehousedt = (DataTable)Session["OpenWarehouseData"];
            //        }
            //        else
            //        {
            //            Warehousedt.Columns.Add("SrlNo", typeof(Int32));
            //            Warehousedt.Columns.Add("WarehouseID", typeof(string));
            //            Warehousedt.Columns.Add("WarehouseName", typeof(string));

            //            Warehousedt.Columns.Add("BatchNo", typeof(string));
            //            Warehousedt.Columns.Add("SerialNo", typeof(string));

            //            Warehousedt.Columns.Add("MFGDate", typeof(DateTime));
            //            Warehousedt.Columns.Add("ExpiryDate", typeof(DateTime));
            //            Warehousedt.Columns.Add("Quantity", typeof(decimal));

            //            Warehousedt.Columns.Add("BatchWarehouseID", typeof(string));
            //            Warehousedt.Columns.Add("BatchWarehousedetailsID", typeof(string));
            //            Warehousedt.Columns.Add("BatchID", typeof(string));
            //            Warehousedt.Columns.Add("SerialID", typeof(string));


            //            Warehousedt.Columns.Add("viewWarehouseName", typeof(string));

            //            Warehousedt.Columns.Add("viewBatchNo", typeof(string));
            //            Warehousedt.Columns.Add("viewQuantity", typeof(string));
            //            Warehousedt.Columns.Add("viewSerialNo", typeof(string));



            //            Warehousedt.Columns.Add("viewMFGDate", typeof(DateTime));
            //            Warehousedt.Columns.Add("viewExpiryDate", typeof(DateTime));

            //            Warehousedt.Columns.Add("Quantitysum", typeof(decimal));
            //            Warehousedt.Columns.Add("isnew", typeof(string));
            //        }



            //        DateTime dtmgh = txtmkgdate.Date;
            //        DateTime dtexp = txtexpirdate.Date;

            //        string isedited = hdnisedited.Value;
            //        decimal inputqnty = 0;
            //        int noofserial = 0;
            //        int oldrowcount = Convert.ToInt32(hdnoldrowcount.Value);
            //        int newrowcount = 0;
            //        decimal hdntotalqntys = Convert.ToDecimal(hdntotalqnty.Value);


            //        if (Session["OpenWarehouseData"] != null)
            //        {
            //            DataTable Warehousedts = (DataTable)Session["OpenWarehouseData"];

            //            newrowcount = Warehousedts.Select("isnew = 'new'").Count<DataRow>();

            //            if (newrowcount != null && hdnisserial.Value == "true")
            //            {

            //                var inputqntys = Warehousedts.Select("WarehouseName= '" + WarehouseName + "' AND BatchNo = '" + BatchName + "' AND isnew = 'new'").Count<DataRow>();
            //                if (inputqntys != null && !string.IsNullOrEmpty(Convert.ToString(inputqntys)))
            //                {
            //                    noofserial = Convert.ToInt32(inputqntys + 1);

            //                }
            //                else
            //                {
            //                    noofserial = 0;
            //                }
            //            }
            //        }

            //        if (hidencountforserial.Value != "2")
            //        {
            //            if (Warehousedt.Rows.Count > 0)
            //            {
            //                var inputqntys = Warehousedt.Compute("sum(Quantitysum)", "isnew = 'new'");
            //                if (inputqntys != null && !string.IsNullOrEmpty(Convert.ToString(inputqntys)))
            //                {
            //                    inputqnty = Convert.ToDecimal(inputqntys);

            //                }
            //                else
            //                {
            //                    inputqnty = 0;
            //                }

            //            }
            //            //commentout below line for it should not add.
            //            if (hdnisserial.Value == "false" && hdniswarehouse.Value == "true" && hdnisbatch.Value == "false")
            //            {
            //                inputqnty = inputqnty + Convert.ToDecimal(openingstock);
            //            }
            //            if (hdnisserial.Value == "false" && hdniswarehouse.Value == "true" && hdnisbatch.Value == "true")
            //            {
            //                inputqnty = inputqnty + Convert.ToDecimal(openingstock);
            //            }
            //            if (hdnisserial.Value == "false" && hdniswarehouse.Value == "false" && hdnisbatch.Value == "true")
            //            {
            //                inputqnty = inputqnty + Convert.ToDecimal(openingstock);
            //            }


            //        }

            //        //checking quantity only for warehouse is true.
            //        if (inputqnty == 0 && hdnisserial.Value == "false")
            //        {
            //            inputqnty = Convert.ToDecimal(openingstock);
            //        }

            //        if (hidencountforserial.Value == "1" && hdnisserial.Value == "true" && noofserial != openingstock && hdnisbatch.Value == "true" && hdniswarehouse.Value == "true")
            //        {
            //            GrdWarehouse.JSProperties["cpupdatemssgserialsetdisblebatch"] = Convert.ToString("disable");
            //        }
            //        if (hidencountforserial.Value == "2" && hdnisserial.Value == "true" && noofserial == openingstock && hdnisbatch.Value == "true" && hdniswarehouse.Value == "true")
            //        {
            //            GrdWarehouse.JSProperties["cpupdatemssgserialsetenablebatch"] = Convert.ToString("enable");
            //        }
            //        if (hidencountforserial.Value == "1" && hdnisserial.Value == "true" && noofserial != openingstock && hdnisbatch.Value == "false" && hdniswarehouse.Value == "true")
            //        {
            //            GrdWarehouse.JSProperties["cpupdatemssgserialsetdisblebatch"] = Convert.ToString("disable");
            //        }
            //        if (hidencountforserial.Value == "2" && hdnisserial.Value == "true" && noofserial == openingstock && hdnisbatch.Value == "false" && hdniswarehouse.Value == "true")
            //        {
            //            GrdWarehouse.JSProperties["cpupdatemssgserialsetenablebatch"] = Convert.ToString("enable");
            //        }

            //        ///batch and serial only
            //        if (hidencountforserial.Value == "1" && hdnisserial.Value == "true" && noofserial != openingstock && hdnisbatch.Value == "true" && hdniswarehouse.Value == "false")
            //        {
            //            GrdWarehouse.JSProperties["cpupdatemssgserialsetdisblebatch"] = Convert.ToString("disable");
            //        }

            //        if (hidencountforserial.Value == "2" && hdnisserial.Value == "true" && noofserial == openingstock && hdnisbatch.Value == "true" && hdniswarehouse.Value == "false")
            //        {
            //            GrdWarehouse.JSProperties["cpupdatemssgserialsetenablebatch"] = Convert.ToString("enable");
            //        }

            //        ///end batch and serial only



            //        //

            //        //if (hdnisserial.Value == "true" && ((Convert.ToDecimal(Convert.ToString((oldrowcount - newrowcount)).Replace('-', '0')) == hdntotalqntys) || (Convert.ToDecimal(Convert.ToString((oldrowcount - newrowcount)).Replace('-', '0')) == (hdntotalqntys * Convert.ToDecimal(hdnbatchchanged.Value)))) && ((hdniswarehouse.Value == "true" && hdnisbatch.Value == "true") && (hdnoldwarehousname.Value == WarehouseName && hdnoldbatchno.Value == BatchName)) || (hdniswarehouse.Value == "true" && hdnisbatch.Value == "false") && (hdnoldwarehousname.Value == WarehouseName) || (hdniswarehouse.Value == "false" && hdnisbatch.Value == "true") && (hdnoldbatchno.Value == BatchName))
            //        //{
            //        //    GrdWarehouse.JSProperties["cpupdatemssgserial"] = Convert.ToString("Please make sure quantity and no of Serial are equal or not.");
            //        //}
            //        if (hdnisserial.Value == "true" && noofserial > openingstock)
            //        {
            //            GrdWarehouse.JSProperties["cpupdatemssgserial"] = Convert.ToString("Please make sure quantity and no of Serial are equal or not.");
            //        }
            //        else
            //        {

            //            if (inputqnty <= totalopeining || isedited == "true")
            //            {

            //                if (hdniswarehouse.Value == "true" && hdnisbatch.Value == "true" && hidencountforserial.Value == "2" && hdnisserial.Value == "true" && hdnoldwarehousname.Value == WarehouseName && hdnoldbatchno.Value == BatchName)
            //                {
            //                    if (Convert.ToString(dtmgh) != "1/1/0001 12:00:00 AM" && Convert.ToString(dtexp) != "1/1/0001 12:00:00 AM")
            //                    {

            //                        Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, dtmgh, dtexp, openingstock, "0", "0", "0", "0", "", "", "", SerialName, null, null, 0, "new");
            //                        GrdWarehouse.JSProperties["cpinsertmssgserial"] = Convert.ToString("Inserted.");

            //                    }
            //                    else
            //                    {
            //                        Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, null, null, openingstock, "0", "0", "0", "0", "", "", "", SerialName, null, null, 0, "new");

            //                        GrdWarehouse.JSProperties["cpinsertmssgserial"] = Convert.ToString("Inserted.");

            //                    }
            //                }
            //                else if (hdniswarehouse.Value == "false" && hdnisbatch.Value == "false" && hdnisserial.Value == "true")
            //                {

            //                    if (Convert.ToString(dtmgh) != "1/1/0001 12:00:00 AM" && Convert.ToString(dtexp) != "1/1/0001 12:00:00 AM")
            //                    {

            //                        Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, dtmgh, dtexp, openingstock, "0", "0", "0", "0", WarehouseName, BatchName, openingstock, SerialName, dtmgh, dtexp, openingstock, "new");
            //                        GrdWarehouse.JSProperties["cpinsertmssgserial"] = Convert.ToString("Inserted.");
            //                    }
            //                    else
            //                    {
            //                        Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, null, null, openingstock, "0", "0", "0", "0", WarehouseName, BatchName, openingstock, SerialName, null, null, openingstock, "new");
            //                        GrdWarehouse.JSProperties["cpinsertmssgserial"] = Convert.ToString("Inserted.");

            //                    }

            //                }
            //                else if (hdniswarehouse.Value == "true" && hidencountforserial.Value == "2" && hdnisbatch.Value == "false" && hdnisserial.Value == "true")
            //                {
            //                    if (Convert.ToString(dtmgh) != "1/1/0001 12:00:00 AM" && Convert.ToString(dtexp) != "1/1/0001 12:00:00 AM")
            //                    {

            //                        Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, dtmgh, dtexp, openingstock, "0", "0", "0", "0", "", "", "", SerialName, null, null, 0, "new");
            //                        GrdWarehouse.JSProperties["cpinsertmssgserial"] = Convert.ToString("Inserted.");

            //                    }
            //                    else
            //                    {
            //                        Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, null, null, openingstock, "0", "0", "0", "0", "", "", "", SerialName, null, null, 0, "new");

            //                        GrdWarehouse.JSProperties["cpinsertmssgserial"] = Convert.ToString("Inserted.");

            //                    }
            //                }
            //                //batch only with serial

            //                else if (hdniswarehouse.Value == "false" && hidencountforserial.Value == "2" && hdnisbatch.Value == "true" && hdnisserial.Value == "true")
            //                {
            //                    if (Convert.ToString(dtmgh) != "1/1/0001 12:00:00 AM" && Convert.ToString(dtexp) != "1/1/0001 12:00:00 AM")
            //                    {

            //                        Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, dtmgh, dtexp, openingstock, "0", "0", "0", "0", "", "", "", SerialName, null, null, 0, "new");
            //                        GrdWarehouse.JSProperties["cpinsertmssgserial"] = Convert.ToString("Inserted.");

            //                    }
            //                    else
            //                    {
            //                        Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, null, null, openingstock, "0", "0", "0", "0", "", "", "", SerialName, null, null, 0, "new");

            //                        GrdWarehouse.JSProperties["cpinsertmssgserial"] = Convert.ToString("Inserted.");

            //                    }
            //                }
            //                //batch only.
            //                else
            //                {
            //                    if (Convert.ToString(dtmgh) != "1/1/0001 12:00:00 AM" && Convert.ToString(dtexp) != "1/1/0001 12:00:00 AM")
            //                    {

            //                        Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, dtmgh, dtexp, openingstock, "0", "0", "0", "0", WarehouseName, BatchName, openingstock, SerialName, dtmgh, dtexp, openingstock, "new");

            //                    }
            //                    else
            //                    {
            //                        Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, null, null, openingstock, "0", "0", "0", "0", WarehouseName, BatchName, openingstock, SerialName, null, null, openingstock, "new");


            //                    }
            //                    if (hdnisserial.Value == "true")
            //                    {
            //                        GrdWarehouse.JSProperties["cpinsertmssg"] = Convert.ToString("Inserted.");
            //                    }
            //                    if (hdnisserial.Value == "false" && hdniswarehouse.Value == "true" && hdnisbatch.Value == "true")
            //                    {
            //                        GrdWarehouse.JSProperties["cpbatchinsertmssg"] = Convert.ToString("Inserted.");
            //                    }

            //                }


            //                Session["OpenWarehouseData"] = Warehousedt;


            //                GrdWarehouse.DataSource = Warehousedt.DefaultView;
            //                GrdWarehouse.DataBind();
            //            }
            //            else
            //            {
            //                GrdWarehouse.JSProperties["cpupdatemssg"] = Convert.ToString("You have entered Quantity greater than Opening Quantity. Cannot Proceed.");
            //            }
            //        }
            //    }
            //    //
            //}
            //#endregion
            //#region SaveAll
            //if (strSplitCommand == "Saveall")
            //{
            //    if (Session["OpenWarehouseData"] == null && hdnisolddeleted.Value == "false")
            //    {
            //        if (!string.IsNullOrEmpty(hdnvalue.Value) && !string.IsNullOrEmpty(hdnrate.Value))
            //        {
            //            updateRateValuedata(hdfProductID.Value, hdfstockid.Value, hdnselectedbranch.Value, "update");
            //            GrdWarehouse.JSProperties["cpupdatemssg"] = Convert.ToString("Saved Successfully.");
            //        }
            //        else
            //        {
            //            GrdWarehouse.JSProperties["cpupdatemssg"] = Convert.ToString("Nothing to Saved .");
            //        }

            //    }
            //    else if (Session["OpenWarehouseData"] == null && hdnisolddeleted.Value == "true")
            //    {

            //        deleteALL(Convert.ToString(hdfstockid.Value));
            //        GrdWarehouse.JSProperties["cpupdatemssg"] = Convert.ToString("Saved Successfully.");
            //    }
            //    else
            //    {
            //        string ProductID = Convert.ToString(hdfProductID.Value);
            //        string stockid = Convert.ToString(hdfstockid.Value);
            //        string openingstock = Convert.ToString(hdfopeningstock.Value);
            //        string branchid = Convert.ToString(hdbranchID.Value);
            //        string isolddeletd = hdnisolddeleted.Value;

            //        int oldrowcount = Convert.ToInt32(hdnoldrowcount.Value);
            //        int newrowcount = 0;
            //        int updaterows = 0;
            //        int deleted = 0;
            //        decimal hdntotalqntys = Convert.ToDecimal(hdntotalqnty.Value);

            //        DataTable Warehousedts = (DataTable)Session["OpenWarehouseData"];

            //        newrowcount = Warehousedts.Select("isnew = 'new'").Count<DataRow>();
            //        updaterows = Warehousedts.Select("isnew = 'Updated'").Count<DataRow>();


            //        if (newrowcount != 0 || updaterows != 0 || Session["WarehouseDataDelete"] != null)
            //        {
            //            decimal inputqnty = 0;
            //            decimal totalopeining = Convert.ToDecimal(hdfopeningstock.Value);
            //            decimal oldtotalopeining = Convert.ToDecimal(oldopeningqntity.Value);

            //            var inputqntys = Warehousedts.Compute("sum(Quantitysum)", "isnew = 'new'");
            //            var updateqnty = Warehousedts.Compute("sum(Quantitysum)", "isnew = 'Updated'");
            //            var oldeqnty = Warehousedts.Compute("sum(Quantitysum)", "isnew = 'old'");
            //            decimal deletd = Convert.ToDecimal(hdndeleteqnity.Value);

            //            if (inputqntys != null && !string.IsNullOrEmpty(Convert.ToString(inputqntys)))
            //            {
            //                inputqnty = Convert.ToDecimal(inputqntys);

            //            }
            //            if (updateqnty != null && !string.IsNullOrEmpty(Convert.ToString(updateqnty)))
            //            {
            //                inputqnty = inputqnty + Convert.ToDecimal(updateqnty);


            //            }
            //            if (oldeqnty != null && !string.IsNullOrEmpty(Convert.ToString(oldeqnty)))
            //            {
            //                inputqnty = inputqnty + Convert.ToDecimal(oldeqnty);


            //            }
            //            //commnet for allowing reduce from main page
            //            //if ((oldeqnty != null && !string.IsNullOrEmpty(Convert.ToString(oldeqnty))) || (updateqnty != null && !string.IsNullOrEmpty(Convert.ToString(updateqnty))))
            //            //{

            //            //    //totalopeining = totalopeining + oldtotalopeining; //commnet for allowing reduce from main page
            //            //    //if (hdnisreduing.Value == "true")
            //            //    //{
            //            //    //    totalopeining = oldtotalopeining;
            //            //    //}
            //            //    //else
            //            //    //{
            //            //    //    totalopeining = totalopeining + oldtotalopeining;
            //            //    //}
            //            //    totalopeining = oldtotalopeining;
            //            //}
            //            //if (deletd > 0 & isolddeletd == "true")
            //            //{
            //            //    totalopeining = Convert.ToDecimal(oldopeningqntity.Value) - deletd;
            //            //}

            //            if (inputqnty == totalopeining)
            //            {
            //                //if (hdnisserial.Value == "true" && ((Convert.ToDecimal(Convert.ToString((oldrowcount - newrowcount)).Replace('-', '0')) == hdntotalqntys) || (Convert.ToDecimal(Convert.ToString((oldrowcount - newrowcount)).Replace('-', '0')) == (hdntotalqntys * Convert.ToDecimal(hdnbatchchanged.Value)))))
            //                if (hdnisserial.Value == "true" && hdnisbatch.Value == "true" && hdniswarehouse.Value == "true")
            //                {

            //                    if (ProductID != "0" && stockid != "0" && branchid != "0")
            //                    {
            //                        int output = Insertupdatedata(ProductID, stockid, branchid);
            //                        if (output > 0)
            //                        {
            //                            DataTable Warehousedt = new DataTable();
            //                            Warehousedt = GetRecord(stockid);
            //                            if (Warehousedt.Rows.Count > 0)
            //                            {
            //                                Session["OpenWarehouseData"] = Warehousedt;

            //                                GrdWarehouse.DataSource = Warehousedt.DefaultView;
            //                                GrdWarehouse.DataBind();
            //                            }
            //                            else
            //                            {

            //                                Session["OpenWarehouseData"] = null;

            //                                GrdWarehouse.DataSource = null;
            //                                GrdWarehouse.DataBind();

            //                            }

            //                            GrdWarehouse.JSProperties["cpupdatemssg"] = Convert.ToString("Saved Successfully.");

            //                        }

            //                    }
            //                    else
            //                    {
            //                        GrdWarehouse.JSProperties["cpupdatemssg"] = Convert.ToString("Please try again null parameters.");
            //                    }
            //                }
            //                else if (hdnisserial.Value != "true" && hdnisbatch.Value != "true" && hdniswarehouse.Value == "true")
            //                {
            //                    if (ProductID != "0" && stockid != "0" && branchid != "0")
            //                    {
            //                        int output = Insertupdatedata(ProductID, stockid, branchid);
            //                        if (output > 0)
            //                        {
            //                            DataTable Warehousedt = new DataTable();
            //                            Warehousedt = GetRecord(stockid);
            //                            if (Warehousedt.Rows.Count > 0)
            //                            {
            //                                Session["OpenWarehouseData"] = Warehousedt;

            //                                GrdWarehouse.DataSource = Warehousedt.DefaultView;
            //                                GrdWarehouse.DataBind();
            //                            }
            //                            else
            //                            {

            //                                Session["OpenWarehouseData"] = null;

            //                                GrdWarehouse.DataSource = null;
            //                                GrdWarehouse.DataBind();

            //                            }

            //                            GrdWarehouse.JSProperties["cpupdatemssg"] = Convert.ToString("Saved Successfully.");

            //                        }

            //                    }
            //                    else
            //                    {
            //                        GrdWarehouse.JSProperties["cpupdatemssg"] = Convert.ToString("Please try again null parameters.");
            //                    }
            //                }
            //                else if (hdnisserial.Value != "true" && hdnisbatch.Value == "true")
            //                {
            //                    if (ProductID != "0" && stockid != "0" && branchid != "0")
            //                    {
            //                        int output = Insertupdatedata(ProductID, stockid, branchid);
            //                        if (output > 0)
            //                        {
            //                            DataTable Warehousedt = new DataTable();
            //                            Warehousedt = GetRecord(stockid);
            //                            if (Warehousedt.Rows.Count > 0)
            //                            {
            //                                Session["OpenWarehouseData"] = Warehousedt;

            //                                GrdWarehouse.DataSource = Warehousedt.DefaultView;
            //                                GrdWarehouse.DataBind();
            //                            }
            //                            else
            //                            {

            //                                Session["OpenWarehouseData"] = null;

            //                                GrdWarehouse.DataSource = null;
            //                                GrdWarehouse.DataBind();

            //                            }

            //                            GrdWarehouse.JSProperties["cpupdatemssg"] = Convert.ToString("Saved Successfully.");

            //                        }

            //                    }
            //                    else
            //                    {
            //                        GrdWarehouse.JSProperties["cpupdatemssg"] = Convert.ToString("Please try again null parameters.");
            //                    }

            //                }
            //                else if (hdnisserial.Value == "true" && hdnisbatch.Value != "true" && hdniswarehouse.Value != "true")
            //                {
            //                    if (ProductID != "0" && stockid != "0" && branchid != "0")
            //                    {
            //                        int output = Insertupdatedata(ProductID, stockid, branchid);
            //                        if (output > 0)
            //                        {
            //                            DataTable Warehousedt = new DataTable();
            //                            Warehousedt = GetRecord(stockid);
            //                            if (Warehousedt.Rows.Count > 0)
            //                            {
            //                                Session["OpenWarehouseData"] = Warehousedt;

            //                                GrdWarehouse.DataSource = Warehousedt.DefaultView;
            //                                GrdWarehouse.DataBind();
            //                            }
            //                            else
            //                            {

            //                                Session["OpenWarehouseData"] = null;

            //                                GrdWarehouse.DataSource = null;
            //                                GrdWarehouse.DataBind();

            //                            }

            //                            GrdWarehouse.JSProperties["cpupdatemssg"] = Convert.ToString("Saved Successfully.");

            //                        }

            //                    }
            //                    else
            //                    {
            //                        GrdWarehouse.JSProperties["cpupdatemssg"] = Convert.ToString("Please try again null parameters.");
            //                    }
            //                }
            //                else if (hdnisserial.Value == "true" && hdnisbatch.Value != "true" && hdniswarehouse.Value == "true")
            //                {

            //                    if (ProductID != "0" && stockid != "0" && branchid != "0")
            //                    {
            //                        int output = Insertupdatedata(ProductID, stockid, branchid);
            //                        if (output > 0)
            //                        {
            //                            DataTable Warehousedt = new DataTable();
            //                            Warehousedt = GetRecord(stockid);
            //                            if (Warehousedt.Rows.Count > 0)
            //                            {
            //                                Session["OpenWarehouseData"] = Warehousedt;

            //                                GrdWarehouse.DataSource = Warehousedt.DefaultView;
            //                                GrdWarehouse.DataBind();
            //                            }
            //                            else
            //                            {

            //                                Session["OpenWarehouseData"] = null;

            //                                GrdWarehouse.DataSource = null;
            //                                GrdWarehouse.DataBind();

            //                            }

            //                            GrdWarehouse.JSProperties["cpupdatemssg"] = Convert.ToString("Saved Successfully.");

            //                        }

            //                    }
            //                    else
            //                    {
            //                        GrdWarehouse.JSProperties["cpupdatemssg"] = Convert.ToString("Please try again null parameters.");
            //                    }
            //                }
            //                else if (hdnisserial.Value == "true" && hdnisbatch.Value == "true" && hdniswarehouse.Value != "true")
            //                {

            //                    if (ProductID != "0" && stockid != "0" && branchid != "0")
            //                    {
            //                        int output = Insertupdatedata(ProductID, stockid, branchid);
            //                        if (output > 0)
            //                        {
            //                            DataTable Warehousedt = new DataTable();
            //                            Warehousedt = GetRecord(stockid);
            //                            if (Warehousedt.Rows.Count > 0)
            //                            {
            //                                Session["OpenWarehouseData"] = Warehousedt;

            //                                GrdWarehouse.DataSource = Warehousedt.DefaultView;
            //                                GrdWarehouse.DataBind();
            //                            }
            //                            else
            //                            {

            //                                Session["OpenWarehouseData"] = null;

            //                                GrdWarehouse.DataSource = null;
            //                                GrdWarehouse.DataBind();

            //                            }

            //                            GrdWarehouse.JSProperties["cpupdatemssg"] = Convert.ToString("Saved Successfully.");

            //                        }

            //                    }
            //                    else
            //                    {
            //                        GrdWarehouse.JSProperties["cpupdatemssg"] = Convert.ToString("Please try again null parameters.");
            //                    }
            //                }
            //                else
            //                {
            //                    GrdWarehouse.JSProperties["cpupdatemssgserial"] = Convert.ToString("Please make sure quantity and no of Serial are equal or not.");

            //                }

            //            }
            //            else
            //            {
            //                GrdWarehouse.JSProperties["cpupdatemssgserial"] = Convert.ToString("You have entered Quantity greater than Opening Quantity. Cannot Proceed.");
            //            }

            //        }
            //        else
            //        {
            //            if (!string.IsNullOrEmpty(hdnvalue.Value) && !string.IsNullOrEmpty(hdnrate.Value))
            //            {
            //                int olfdatterows = Warehousedts.Select("isnew = 'old'").Count<DataRow>();
            //                if (olfdatterows != 0)
            //                {
            //                    var oldeqnty = Warehousedts.Compute("sum(Quantitysum)", "isnew = 'old'");
            //                    if (Convert.ToDecimal(hdfopeningstock.Value) < Convert.ToDecimal(oldeqnty))
            //                    {
            //                        GrdWarehouse.JSProperties["cpupdatemssg"] = Convert.ToString("Opening quantity and enterd quantity are mismatch.");
            //                    }
            //                    else
            //                    {
            //                        updateRateValuedata(hdfProductID.Value, hdfstockid.Value, hdnselectedbranch.Value, "update");
            //                        GrdWarehouse.JSProperties["cpupdatemssg"] = Convert.ToString("Saved Successfully.");
            //                    }

            //                }
            //                else
            //                {
            //                    updateRateValuedata(hdfProductID.Value, hdfstockid.Value, hdnselectedbranch.Value, "update");
            //                    GrdWarehouse.JSProperties["cpupdatemssg"] = Convert.ToString("Saved Successfully.");
            //                }

            //            }
            //            else
            //            {
            //                GrdWarehouse.JSProperties["cpupdatemssg"] = Convert.ToString("Nothing to Saved .");
            //            }

            //        }
            //    }

            //}

            //#endregion
            //#region CheckDataExist
            //if (strSplitCommand == "checkdataexist")
            //{
            //    Session["OpenWarehouseData"] = null;

            //    GrdWarehouse.DataSource = null;

            //    string strProductname = Convert.ToString(e.Parameters.Split('~')[4]);
            //    string name = strProductname;
            //    name = name.Replace("squot", "'");
            //    name = name.Replace("coma", ",");
            //    name = name.Replace("slash", "/");

            //    Session["WarehouseUpdatedData"] = null;
            //    Session["WarehouseDataDelete"] = null;

            //    GrdWarehouse.JSProperties["cpproductname"] = Convert.ToString(name);


            //    string strProductID = Convert.ToString(e.Parameters.Split('~')[1]);

            //    string stockids = Convert.ToString(e.Parameters.Split('~')[2]);

            //    string branchid = Convert.ToString(e.Parameters.Split('~')[3]);

            //    DataTable Warehousedt = new DataTable();
            //    Warehousedt = GetRecord(stockids);
            //    if (Warehousedt.Rows.Count > 0)
            //    {
            //        Session["OpenWarehouseData"] = Warehousedt;

            //        GrdWarehouse.DataSource = Warehousedt.DefaultView;
            //        GrdWarehouse.DataBind();
            //    }
            //    else
            //    {

            //        Session["OpenWarehouseData"] = null;

            //        GrdWarehouse.DataSource = null;
            //        GrdWarehouse.DataBind();

            //    }


            //}
            ////if (strSplitCommand == "UpdatedataReffresh")
            ////{
            ////    if (Session["OpenWarehouseData"] != null)
            ////    {

            ////        GrdWarehouse.DataSource = Session["OpenWarehouseData"];
            ////        GrdWarehouse.DataBind();
            ////    }
            ////}
            //#endregion
            //#region updatedata
            //if (strSplitCommand == "Updatedata")
            //{
            //    string WarehouseID = Convert.ToString(e.Parameters.Split('~')[1]);

            //    string WarehouseName = Convert.ToString(e.Parameters.Split('~')[2]);

            //    string BatchName = Convert.ToString(e.Parameters.Split('~')[3]);

            //    string SerialName = Convert.ToString(e.Parameters.Split('~')[4]);

            //    string slno = Convert.ToString(e.Parameters.Split('~')[5]);
            //    string qntity = Convert.ToString(e.Parameters.Split('~')[6]);
            //    string isviewqntitynull = hdnisviewqntityhas.Value;
            //    string isserialenable = hdnisserial.Value;
            //    DateTime expdate = txtexpirdate.Date;
            //    DateTime mkgdate = txtmkgdate.Date;
            //    DataTable Warehousedt = new DataTable();

            //    decimal openingstock = Convert.ToDecimal(txtqnty.Text);
            //    if (qntity == "0.0000" && openingstock <= 0)
            //    {
            //        qntity = batchqnty.Text;
            //        openingstock = Convert.ToDecimal(qntity);
            //    }

            //    if (WarehouseID == "null")
            //    {
            //        WarehouseID = "0";
            //        WarehouseName = "";
            //    }

            //    if (Session["OpenWarehouseData"] != null)
            //    {
            //        Warehousedt = (DataTable)Session["OpenWarehouseData"];
            //        DataSet dataSet1 = new DataSet();
            //        DataTable dt1 = new DataTable();
            //        dt1 = Warehousedt.Copy();
            //        dataSet1.Tables.Add(dt1);

            //        if (dataSet1.Tables[0].Rows.Count > 0)
            //        {
            //            DataRow[] customerRow = dataSet1.Tables[0].Select("SrlNo ='" + slno + "'");
            //            if (isserialenable == "false" && isviewqntitynull != "false")
            //            {
            //                customerRow[0]["WarehouseID"] = WarehouseID;
            //                customerRow[0]["WarehouseName"] = WarehouseName;
            //                customerRow[0]["BatchNo"] = BatchName;
            //                customerRow[0]["SerialNo"] = SerialName;
            //                customerRow[0]["Quantity"] = openingstock;

            //                if (Convert.ToString(expdate) != "1/1/0001 12:00:00 AM" && Convert.ToString(mkgdate) != "1/1/0001 12:00:00 AM")
            //                {
            //                    customerRow[0]["viewMFGDate"] = mkgdate;
            //                    customerRow[0]["viewExpiryDate"] = expdate;
            //                }

            //                customerRow[0]["viewWarehouseName"] = WarehouseName;
            //                customerRow[0]["viewBatchNo"] = BatchName;
            //                customerRow[0]["viewSerialNo"] = SerialName;
            //                customerRow[0]["viewQuantity"] = qntity;
            //                customerRow[0]["Quantitysum"] = openingstock;
            //                customerRow[0]["isnew"] = "Updated";
            //            }
            //            if (isserialenable == "true" && isviewqntitynull == "false")
            //            {
            //                customerRow[0]["WarehouseID"] = WarehouseID;
            //                customerRow[0]["WarehouseName"] = WarehouseName;
            //                customerRow[0]["BatchNo"] = BatchName;
            //                customerRow[0]["SerialNo"] = SerialName;
            //                customerRow[0]["Quantity"] = openingstock;

            //                if (Convert.ToString(expdate) != "1/1/0001 12:00:00 AM" && Convert.ToString(mkgdate) != "1/1/0001 12:00:00 AM")
            //                {
            //                    customerRow[0]["viewMFGDate"] = mkgdate;
            //                    customerRow[0]["viewExpiryDate"] = expdate;
            //                }

            //                customerRow[0]["viewWarehouseName"] = WarehouseName;
            //                customerRow[0]["viewBatchNo"] = BatchName;
            //                customerRow[0]["viewSerialNo"] = SerialName;
            //                customerRow[0]["viewQuantity"] = openingstock;
            //                customerRow[0]["Quantitysum"] = openingstock;
            //                customerRow[0]["isnew"] = "Updated";
            //            }
            //            if (isserialenable == "true" && isviewqntitynull == "true")
            //            {
            //                customerRow[0]["WarehouseID"] = WarehouseID;
            //                customerRow[0]["WarehouseName"] = WarehouseName;
            //                customerRow[0]["BatchNo"] = BatchName;
            //                customerRow[0]["SerialNo"] = SerialName;
            //                customerRow[0]["Quantity"] = openingstock;



            //                customerRow[0]["viewSerialNo"] = SerialName;

            //                customerRow[0]["Quantitysum"] = 0;
            //                customerRow[0]["isnew"] = "Updated";
            //            }
            //            if (isserialenable == "false" && isviewqntitynull == "false")
            //            {
            //                customerRow[0]["WarehouseID"] = WarehouseID;
            //                customerRow[0]["WarehouseName"] = WarehouseName;
            //                customerRow[0]["BatchNo"] = BatchName;
            //                customerRow[0]["SerialNo"] = SerialName;
            //                customerRow[0]["Quantity"] = openingstock;

            //                customerRow[0]["SerialNo"] = SerialName;
            //                customerRow[0]["Quantity"] = openingstock;
            //                if (Convert.ToString(expdate) != "1/1/0001 12:00:00 AM" && Convert.ToString(mkgdate) != "1/1/0001 12:00:00 AM")
            //                {
            //                    customerRow[0]["viewMFGDate"] = mkgdate;
            //                    customerRow[0]["viewExpiryDate"] = expdate;
            //                }
            //                customerRow[0]["viewWarehouseName"] = WarehouseName;
            //                customerRow[0]["viewBatchNo"] = BatchName;
            //                customerRow[0]["viewSerialNo"] = SerialName;
            //                customerRow[0]["viewQuantity"] = openingstock;
            //                customerRow[0]["Quantitysum"] = openingstock;
            //                customerRow[0]["isnew"] = "Updated";
            //            }

            //            GrdWarehouse.JSProperties["cpupdateexistingdata"] = Convert.ToString("Saved.");
            //        }
            //        Warehousedt = null;
            //        Warehousedt = dataSet1.Tables[0];
            //        Session["OpenWarehouseData"] = Warehousedt;
            //        Session["WarehouseUpdatedData"] = Warehousedt;

            //        GrdWarehouse.DataSource = Warehousedt.DefaultView;
            //        GrdWarehouse.DataBind();

            //    }
            //}

            //#endregion
            //#region newrowupdate
            //if (strSplitCommand == "NewUpdatedata")
            //{
            //    string WarehouseID = Convert.ToString(e.Parameters.Split('~')[1]);

            //    string WarehouseName = Convert.ToString(e.Parameters.Split('~')[2]);

            //    string BatchName = Convert.ToString(e.Parameters.Split('~')[3]);

            //    string SerialName = Convert.ToString(e.Parameters.Split('~')[4]);

            //    string slno = Convert.ToString(e.Parameters.Split('~')[5]);
            //    string qntity = Convert.ToString(e.Parameters.Split('~')[6]);
            //    string isviewqntitynull = hdnisviewqntityhas.Value;
            //    string isserialenable = hdnisserial.Value;
            //    DataTable Warehousedt = new DataTable();
            //    decimal openingstock = Convert.ToDecimal(txtqnty.Text);
            //    if (qntity == "0.0000" && openingstock <= 0)
            //    {
            //        qntity = batchqnty.Text;
            //        openingstock = Convert.ToDecimal(qntity);
            //    }

            //    if (Session["OpenWarehouseData"] != null)
            //    {
            //        Warehousedt = (DataTable)Session["OpenWarehouseData"];
            //        DataSet dataSet1 = new DataSet();
            //        DataTable dt1 = new DataTable();
            //        dt1 = Warehousedt.Copy();
            //        dataSet1.Tables.Add(dt1);

            //        if (dataSet1.Tables[0].Rows.Count > 0)
            //        {
            //            DataRow[] customerRow = dataSet1.Tables[0].Select("SrlNo ='" + slno + "'");
            //            if (isserialenable == "false" && isviewqntitynull != "false")
            //            {
            //                customerRow[0]["WarehouseID"] = WarehouseID;
            //                customerRow[0]["WarehouseName"] = WarehouseName;
            //                customerRow[0]["BatchNo"] = BatchName;
            //                customerRow[0]["SerialNo"] = SerialName;
            //                customerRow[0]["Quantity"] = openingstock;

            //                customerRow[0]["viewWarehouseName"] = WarehouseName;
            //                customerRow[0]["viewBatchNo"] = BatchName;
            //                customerRow[0]["viewSerialNo"] = SerialName;
            //                customerRow[0]["viewQuantity"] = openingstock;
            //                customerRow[0]["Quantitysum"] = openingstock;
            //                customerRow[0]["isnew"] = "new";
            //            }
            //            if (isserialenable == "true" && isviewqntitynull == "false")
            //            {
            //                customerRow[0]["WarehouseID"] = WarehouseID;
            //                customerRow[0]["WarehouseName"] = WarehouseName;
            //                customerRow[0]["BatchNo"] = BatchName;
            //                customerRow[0]["SerialNo"] = SerialName;
            //                customerRow[0]["Quantity"] = openingstock;

            //                customerRow[0]["viewWarehouseName"] = WarehouseName;
            //                customerRow[0]["viewBatchNo"] = BatchName;
            //                customerRow[0]["viewSerialNo"] = SerialName;
            //                customerRow[0]["viewQuantity"] = openingstock;
            //                customerRow[0]["Quantitysum"] = openingstock;
            //                customerRow[0]["isnew"] = "new";
            //            }
            //            if (isserialenable == "true" && isviewqntitynull == "true")
            //            {
            //                customerRow[0]["WarehouseID"] = WarehouseID;
            //                customerRow[0]["WarehouseName"] = WarehouseName;
            //                customerRow[0]["BatchNo"] = BatchName;
            //                customerRow[0]["SerialNo"] = SerialName;
            //                customerRow[0]["Quantity"] = openingstock;



            //                customerRow[0]["viewSerialNo"] = SerialName;

            //                customerRow[0]["Quantitysum"] = 0;
            //                customerRow[0]["isnew"] = "new";
            //            }
            //            if (isserialenable == "false" && isviewqntitynull == "false")
            //            {
            //                customerRow[0]["WarehouseID"] = WarehouseID;
            //                customerRow[0]["WarehouseName"] = WarehouseName;
            //                customerRow[0]["BatchNo"] = BatchName;
            //                customerRow[0]["SerialNo"] = SerialName;
            //                customerRow[0]["Quantity"] = openingstock;

            //                customerRow[0]["viewWarehouseName"] = WarehouseName;
            //                customerRow[0]["viewBatchNo"] = BatchName;
            //                customerRow[0]["viewSerialNo"] = SerialName;
            //                customerRow[0]["viewQuantity"] = openingstock;
            //                customerRow[0]["Quantitysum"] = openingstock;
            //                customerRow[0]["isnew"] = "new";
            //            }

            //            GrdWarehouse.JSProperties["cpupdatenewdata"] = Convert.ToString("Saved.");
            //        }
            //        Warehousedt = null;
            //        Warehousedt = dataSet1.Tables[0];
            //        Session["OpenWarehouseData"] = Warehousedt;

            //        GrdWarehouse.DataSource = Warehousedt.DefaultView;
            //        GrdWarehouse.DataBind();

            //    }


            //}
            //#endregion
            //#region Delete
            //if (strSplitCommand == "Delete")
            //{
            //    string slno = Convert.ToString(e.Parameters.Split('~')[1]);

            //    string viewQuantity = Convert.ToString(e.Parameters.Split('~')[2]);

            //    string Quantity = Convert.ToString(e.Parameters.Split('~')[3]);
            //    string warehouseid = Convert.ToString(e.Parameters.Split('~')[4]);
            //    string batch = Convert.ToString(e.Parameters.Split('~')[5]);
            //    DataTable Warehousedt = new DataTable();

            //    // string isviewqntitynull = hdnisviewqntityhas.Value;
            //    string isserialenable = hdnisserial.Value;
            //    string isbatch = hdnisbatch.Value;
            //    string iswarehouse = hdniswarehouse.Value;
            //    string isolddeletd = hdnisolddeleted.Value;
            //    if (Session["OpenWarehouseData"] != null)
            //    {
            //        Warehousedt = (DataTable)Session["OpenWarehouseData"];
            //        DataSet dataSet1 = new DataSet();

            //        DataTable dt1 = new DataTable();
            //        dt1 = Warehousedt.Copy();



            //        dataSet1.Tables.Add(dt1);

            //        if (dataSet1.Tables[0].Rows.Count > 0)
            //        {

            //            if (isserialenable == "true" && (viewQuantity == "" || viewQuantity == "0") && isbatch == "true" && iswarehouse == "true")
            //            {
            //                DataRow[] customerRows = dataSet1.Tables[0].Select("WarehouseID= '" + warehouseid + "' AND BatchNo='" + batch + "'");

            //                for (int i = 0; i <= customerRows.Count() - 1; i++)
            //                {
            //                    if (Convert.ToString(customerRows[i]["SrlNo"]) == slno && string.IsNullOrEmpty(Convert.ToString(customerRows[i]["viewQuantity"])))
            //                    {
            //                        customerRows[i]["isnew"] = "DeleteSL";
            //                    }
            //                    else if (Convert.ToString(customerRows[i]["SrlNo"]) == slno && !string.IsNullOrEmpty(Convert.ToString(customerRows[i]["viewQuantity"])))
            //                    {

            //                        customerRows[i]["isnew"] = "DeleteWHBTSL";

            //                    }

            //                    if (!string.IsNullOrEmpty(Convert.ToString(customerRows[i]["viewQuantity"])))
            //                    {
            //                        customerRows[i]["viewQuantity"] = Convert.ToDecimal(customerRows[i]["Quantitysum"]) - 1;
            //                        customerRows[i]["Quantity"] = Convert.ToDecimal(customerRows[i]["Quantity"]) - 1;
            //                        customerRows[i]["Quantitysum"] = Convert.ToDecimal(customerRows[i]["Quantitysum"]) - 1;

            //                        if (Convert.ToString(customerRows[i]["isnew"]) != "new")
            //                        {
            //                            customerRows[i]["isnew"] = "Updated";
            //                        }
            //                        else
            //                        {
            //                            customerRows[i]["isnew"] = "new";
            //                        }

            //                    }
            //                    //else
            //                    //{
            //                    //    customerRows[i]["Quantity"] = 0;
            //                    //    customerRows[i]["Quantitysum"] = 0;
            //                    //}
            //                }


            //                GrdWarehouse.JSProperties["cpdeletedata"] = Convert.ToString(1);

            //            }
            //            else if (isserialenable == "true" && (viewQuantity == "" || viewQuantity == "0") && isbatch != "true" && iswarehouse == "true")
            //            {
            //                DataRow[] customerRows = dataSet1.Tables[0].Select("WarehouseID= '" + warehouseid + "'");

            //                for (int i = 0; i <= customerRows.Count() - 1; i++)
            //                {
            //                    if (Convert.ToString(customerRows[i]["SrlNo"]) == slno && string.IsNullOrEmpty(Convert.ToString(customerRows[i]["viewQuantity"])))
            //                    {
            //                        customerRows[i]["isnew"] = "DeleteSL";
            //                    }
            //                    else if (Convert.ToString(customerRows[i]["SrlNo"]) == slno && !string.IsNullOrEmpty(Convert.ToString(customerRows[i]["viewQuantity"])))
            //                    {

            //                        customerRows[i]["isnew"] = "DeleteWHSL";


            //                    }

            //                    if (!string.IsNullOrEmpty(Convert.ToString(customerRows[i]["viewQuantity"])))
            //                    {
            //                        customerRows[i]["viewQuantity"] = Convert.ToDecimal(customerRows[i]["Quantitysum"]) - 1;
            //                        customerRows[i]["Quantity"] = Convert.ToDecimal(customerRows[i]["Quantity"]) - 1;
            //                        customerRows[i]["Quantitysum"] = Convert.ToDecimal(customerRows[i]["Quantitysum"]) - 1;

            //                        if (Convert.ToString(customerRows[i]["isnew"]) != "new")
            //                        {
            //                            customerRows[i]["isnew"] = "Updated";
            //                        }
            //                        else
            //                        {
            //                            customerRows[i]["isnew"] = "new";
            //                        }

            //                    }
            //                    else
            //                    {
            //                        customerRows[i]["Quantity"] = 0;
            //                        customerRows[i]["Quantitysum"] = 0;
            //                    }
            //                }


            //                GrdWarehouse.JSProperties["cpdeletedata"] = Convert.ToString(1);

            //            }

            //            else if (isserialenable == "true" && (viewQuantity == "" || viewQuantity == "0") && isbatch == "true" && iswarehouse != "true")
            //            {
            //                DataRow[] customerRows = dataSet1.Tables[0].Select("BatchNo= '" + batch + "'");

            //                for (int i = 0; i <= customerRows.Count() - 1; i++)
            //                {
            //                    if (Convert.ToString(customerRows[i]["SrlNo"]) == slno && string.IsNullOrEmpty(Convert.ToString(customerRows[i]["viewQuantity"])))
            //                    {
            //                        customerRows[i]["isnew"] = "DeleteSL";
            //                    }
            //                    else if (Convert.ToString(customerRows[i]["SrlNo"]) == slno && !string.IsNullOrEmpty(Convert.ToString(customerRows[i]["viewQuantity"])))
            //                    {

            //                        customerRows[i]["isnew"] = "DeleteWHBTSL";

            //                        //customerRows[i]["isnew"] = "DeleteWHBTSL";
            //                    }
            //                    if (!string.IsNullOrEmpty(Convert.ToString(customerRows[i]["viewQuantity"])))
            //                    {
            //                        customerRows[i]["viewQuantity"] = Convert.ToDecimal(customerRows[i]["Quantitysum"]) - 1;
            //                        customerRows[i]["Quantity"] = Convert.ToDecimal(customerRows[i]["Quantity"]) - 1;
            //                        customerRows[i]["Quantitysum"] = Convert.ToDecimal(customerRows[i]["Quantitysum"]) - 1;

            //                        if (Convert.ToString(customerRows[i]["isnew"]) != "new")
            //                        {
            //                            customerRows[i]["isnew"] = "Updated";
            //                        }
            //                        else
            //                        {
            //                            customerRows[i]["isnew"] = "new";
            //                        }

            //                    }
            //                    else
            //                    {

            //                        customerRows[i]["Quantity"] = 0;
            //                        customerRows[i]["Quantitysum"] = 0;

            //                    }

            //                }

            //                GrdWarehouse.JSProperties["cpdeletedata"] = Convert.ToString(1);


            //            }
            //            else
            //            {

            //                DataRow[] customerRow = dataSet1.Tables[0].Select("SrlNo ='" + slno + "'");
            //                if (isserialenable != "true" && isbatch == "true" && iswarehouse == "true")
            //                {
            //                    customerRow[0]["isnew"] = "DeleteWHBT";
            //                    GrdWarehouse.JSProperties["cpdeletedata"] = Convert.ToString(viewQuantity);
            //                }
            //                if (isserialenable != "true" && isbatch != "true" && iswarehouse == "true")
            //                {
            //                    customerRow[0]["isnew"] = "DeleteWH";
            //                    GrdWarehouse.JSProperties["cpdeletedata"] = Convert.ToString(Quantity);
            //                }
            //                if (isserialenable != "true" && isbatch == "true" && iswarehouse != "true")
            //                {
            //                    customerRow[0]["isnew"] = "DeleteWHBT";
            //                    GrdWarehouse.JSProperties["cpdeletedata"] = Convert.ToString(viewQuantity);
            //                }
            //                if (isserialenable == "true" && isbatch == "true" && iswarehouse == "true")
            //                {
            //                    customerRow[0]["isnew"] = "DeleteWHBTSL";
            //                    GrdWarehouse.JSProperties["cpdeletedata"] = Convert.ToString(Quantity);
            //                }
            //                if (isserialenable == "true" && isbatch != "true" && iswarehouse == "true")
            //                {
            //                    customerRow[0]["isnew"] = "DeleteWHBTSL";
            //                    GrdWarehouse.JSProperties["cpdeletedata"] = Convert.ToString(1);
            //                }
            //                if (isserialenable == "true" && isbatch == "true" && iswarehouse != "true")
            //                {
            //                    customerRow[0]["isnew"] = "DeleteWHBTSL";
            //                    GrdWarehouse.JSProperties["cpdeletedata"] = Convert.ToString(1);
            //                }
            //            }

            //        }

            //        Warehousedt = null;
            //        DataTable setdeletedate = new DataTable();
            //        DataTable getdeletedate = dataSet1.Tables[0];

            //        DataRow[] drs = getdeletedate.Select("isnew = 'DeleteWHBT' OR isnew = 'DeleteWH' OR isnew = 'DeleteWHBT' OR isnew = 'DeleteBTSL' OR isnew = 'DeleteSL' OR isnew = 'DeleteWHSL' OR isnew = 'DeleteWHBTSL'");
            //        if (drs.Count() != 0)
            //        {
            //            if (Session["WarehouseDataDelete"] != null)
            //            {
            //                setdeletedate = drs.CopyToDataTable();
            //                DataTable dtmp = (DataTable)Session["WarehouseDataDelete"];
            //                dtmp.Merge(setdeletedate);
            //                Session["WarehouseDataDelete"] = dtmp;
            //            }
            //            else
            //            {
            //                setdeletedate = drs.CopyToDataTable();
            //                Session["WarehouseDataDelete"] = setdeletedate;
            //            }


            //        }


            //        DataTable Warehousedts = dataSet1.Tables[0];
            //        DataRow[] dr = Warehousedts.Select("isnew = 'new' OR isnew = 'Updated' OR isnew = 'old'");
            //        if (dr.Count() > 0)
            //        {

            //            Warehousedt = dr.CopyToDataTable();
            //            Warehousedt.DefaultView.Sort = "SrlNo asc";
            //            Warehousedt = Warehousedt.DefaultView.ToTable(true);

            //            Session["OpenWarehouseData"] = Warehousedt;

            //            GrdWarehouse.DataSource = Session["OpenWarehouseData"];
            //            GrdWarehouse.DataBind();
            //        }
            //        else
            //        {
            //            Session["OpenWarehouseData"] = null;

            //            //Warehousedt.Columns.Add("SrlNo", typeof(string));
            //            //Warehousedt.Columns.Add("WarehouseID", typeof(string));
            //            //Warehousedt.Columns.Add("WarehouseName", typeof(string));

            //            //Warehousedt.Columns.Add("BatchNo", typeof(string));
            //            //Warehousedt.Columns.Add("SerialNo", typeof(string));

            //            //Warehousedt.Columns.Add("MFGDate", typeof(DateTime));
            //            //Warehousedt.Columns.Add("ExpiryDate", typeof(DateTime));
            //            //Warehousedt.Columns.Add("Quantity", typeof(decimal));

            //            //Warehousedt.Columns.Add("BatchWarehouseID", typeof(string));
            //            //Warehousedt.Columns.Add("BatchWarehousedetailsID", typeof(string));
            //            //Warehousedt.Columns.Add("BatchID", typeof(string));
            //            //Warehousedt.Columns.Add("SerialID", typeof(string));


            //            //Warehousedt.Columns.Add("viewWarehouseName", typeof(string));

            //            //Warehousedt.Columns.Add("viewBatchNo", typeof(string));
            //            //Warehousedt.Columns.Add("viewQuantity", typeof(string));
            //            //Warehousedt.Columns.Add("viewSerialNo", typeof(string));



            //            //Warehousedt.Columns.Add("viewMFGDate", typeof(DateTime));
            //            //Warehousedt.Columns.Add("viewExpiryDate", typeof(DateTime));

            //            //Warehousedt.Columns.Add("Quantitysum", typeof(decimal));
            //            //Warehousedt.Columns.Add("isnew", typeof(string));

            //            GrdWarehouse.DataSource = Warehousedt;
            //            GrdWarehouse.DataBind();



            //        }


            //    }
            //#endregion
            //}


        }

        //public DataTable GetRecord(string stockids)
        //{
        //    Session["OpenWarehouseData"] = null;
        //    DataTable Warehousedt = new DataTable();

        //    if (Session["OpenWarehouseData"] != null)
        //    {
        //        Warehousedt = (DataTable)Session["OpenWarehouseData"];
        //    }
        //    else
        //    {
        //        Warehousedt.Columns.Add("SrlNo", typeof(Int32));
        //        Warehousedt.Columns.Add("WarehouseID", typeof(string));
        //        Warehousedt.Columns.Add("WarehouseName", typeof(string));

        //        Warehousedt.Columns.Add("BatchNo", typeof(string));
        //        Warehousedt.Columns.Add("SerialNo", typeof(string));

        //        Warehousedt.Columns.Add("MFGDate", typeof(DateTime));
        //        Warehousedt.Columns.Add("ExpiryDate", typeof(DateTime));
        //        Warehousedt.Columns.Add("Quantity", typeof(decimal));

        //        Warehousedt.Columns.Add("BatchWarehouseID", typeof(string));
        //        Warehousedt.Columns.Add("BatchWarehousedetailsID", typeof(string));
        //        Warehousedt.Columns.Add("BatchID", typeof(string));
        //        Warehousedt.Columns.Add("SerialID", typeof(string));


        //        Warehousedt.Columns.Add("viewWarehouseName", typeof(string));

        //        Warehousedt.Columns.Add("viewBatchNo", typeof(string));
        //        Warehousedt.Columns.Add("viewQuantity", typeof(string));
        //        Warehousedt.Columns.Add("viewSerialNo", typeof(string));



        //        Warehousedt.Columns.Add("viewMFGDate", typeof(DateTime));
        //        Warehousedt.Columns.Add("viewExpiryDate", typeof(DateTime));

        //        Warehousedt.Columns.Add("Quantitysum", typeof(decimal));
        //        Warehousedt.Columns.Add("isnew", typeof(string));
        //    }


        //    DataTable dts = GetStockWarehouseData(stockids);

        //    string oldwarehousename = string.Empty;
        //    string oldbatchname = string.Empty;
        //    string oldquantity = string.Empty;
        //    if (dts.Rows.Count > 0)
        //    {
        //        for (int i = 0; i <= dts.Rows.Count - 1; i++)
        //        {

        //            if (hdnisserial.Value == "true" && i != 0)
        //            {
        //                oldwarehousename = Convert.ToString(dts.Rows[i - 1]["warehouseID"]);
        //                oldbatchname = Convert.ToString(dts.Rows[i - 1]["batchNO"]);
        //                oldquantity = Convert.ToString(dts.Rows[i - 1]["qnty"]);
        //                if (oldwarehousename == Convert.ToString(dts.Rows[i]["warehouseID"]) && oldbatchname == Convert.ToString(dts.Rows[i]["batchNO"]) && oldquantity == Convert.ToString(dts.Rows[i]["qnty"]))
        //                {
        //                    if (Convert.ToString(dts.Rows[i]["MfgDate"]) == "1/1/1900 12:00:00 AM")
        //                    {
        //                        Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["Warehousname"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["qnty"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), "", "", "", Convert.ToString(dts.Rows[i]["serialno"]), null, null, 0, "old");
        //                    }
        //                    else if (string.IsNullOrEmpty(Convert.ToString(dts.Rows[i]["MfgDate"])))
        //                    {
        //                        Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["Warehousname"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["qnty"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), "", "", "", Convert.ToString(dts.Rows[i]["serialno"]), null, null, 0, "old");
        //                    }
        //                    else
        //                    {

        //                        Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["Warehousname"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToDecimal(dts.Rows[i]["qnty"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), "", "", "", Convert.ToString(dts.Rows[i]["serialno"]), null, null, 0, "old");
        //                    }
        //                }
        //                //else if (oldwarehousename == Convert.ToString(dts.Rows[i]["warehouseID"]) && oldbatchname == Convert.ToString(dts.Rows[i]["batchNO"]))
        //                //{
        //                //    if (Convert.ToString(dts.Rows[i]["MfgDate"]) == "1/1/1900 12:00:00 AM")
        //                //    {
        //                //        Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["Warehousname"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["qnty"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), "", "", "", Convert.ToString(dts.Rows[i]["serialno"]), null, null, 0, "old");
        //                //    }
        //                //    else if (string.IsNullOrEmpty(Convert.ToString(dts.Rows[i]["MfgDate"])))
        //                //    {
        //                //        Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["Warehousname"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["qnty"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), "", "", "", Convert.ToString(dts.Rows[i]["serialno"]), null, null, 0, "old");
        //                //    }
        //                //    else
        //                //    {

        //                //        Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["Warehousname"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToDecimal(dts.Rows[i]["qnty"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), "", "", "", Convert.ToString(dts.Rows[i]["serialno"]), null, null, 0, "old");
        //                //    }
        //                //}
        //                else
        //                {

        //                    if (Convert.ToString(dts.Rows[i]["MfgDate"]) == "1/1/1900 12:00:00 AM")
        //                    {
        //                        Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["Warehousname"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["qnty"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["Warehousname"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["qntybatch"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["qntybatch"]), "old");
        //                    }
        //                    else if (string.IsNullOrEmpty(Convert.ToString(dts.Rows[i]["MfgDate"])))
        //                    {
        //                        Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["Warehousname"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["qnty"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["Warehousname"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["qntybatch"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["qntybatch"]), "old");
        //                    }
        //                    else
        //                    {

        //                        Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["Warehousname"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToDecimal(dts.Rows[i]["qnty"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["Warehousname"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["qnty"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToString(dts.Rows[i]["qntybatch"]), "old");
        //                    }

        //                }

        //            }
        //            else if (hdnisserial.Value == "false" && hdniswarehouse.Value == "true" && hdnisbatch.Value == "true")
        //            {
        //                //string oldquantity=string.Empty;
        //                if (i != 0)
        //                {
        //                    oldwarehousename = Convert.ToString(dts.Rows[i - 1]["warehouseID"]);
        //                    oldquantity = Convert.ToString(dts.Rows[i - 1]["qntybatch"]);
        //                }



        //                if (oldwarehousename == Convert.ToString(dts.Rows[i]["warehouseID"]))
        //                {
        //                    if (Convert.ToString(dts.Rows[i]["MfgDate"]) == "1/1/1900 12:00:00 AM")
        //                    {
        //                        Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["Warehousname"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["qnty"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), "", Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["qntybatch"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["qntybatch"]), "old");
        //                    }
        //                    else if (string.IsNullOrEmpty(Convert.ToString(dts.Rows[i]["MfgDate"])))
        //                    {
        //                        Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["Warehousname"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["qnty"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), "", Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["qntybatch"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["qntybatch"]), "old");
        //                    }
        //                    else
        //                    {

        //                        Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["Warehousname"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToDecimal(dts.Rows[i]["qnty"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), "", Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["qntybatch"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["qntybatch"]), "old");
        //                    }
        //                }
        //                else
        //                {
        //                    if (Convert.ToString(dts.Rows[i]["MfgDate"]) == "1/1/1900 12:00:00 AM")
        //                    {
        //                        Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["Warehousname"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["qnty"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["Warehousname"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["qntybatch"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["qntybatch"]), "old");
        //                    }
        //                    else if (string.IsNullOrEmpty(Convert.ToString(dts.Rows[i]["MfgDate"])))
        //                    {
        //                        Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["Warehousname"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["qnty"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["Warehousname"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["qntybatch"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["qntybatch"]), "old");
        //                    }
        //                    else
        //                    {

        //                        Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["Warehousname"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToDecimal(dts.Rows[i]["qnty"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["Warehousname"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["qntybatch"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToString(dts.Rows[i]["qntybatch"]), "old");
        //                    }
        //                }

        //            }
        //            else
        //            {

        //                if (Convert.ToString(dts.Rows[i]["MfgDate"]) == "1/1/1900 12:00:00 AM")
        //                {
        //                    Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["Warehousname"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["qnty"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["Warehousname"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["qntybatch"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["qntybatch"]), "old");
        //                }
        //                else if (string.IsNullOrEmpty(Convert.ToString(dts.Rows[i]["MfgDate"])))
        //                {
        //                    Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["Warehousname"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToDecimal(dts.Rows[i]["qnty"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["Warehousname"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["qntybatch"]), Convert.ToString(dts.Rows[i]["serialno"]), null, null, Convert.ToString(dts.Rows[i]["qntybatch"]), "old");
        //                }
        //                else
        //                {

        //                    Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, Convert.ToString(dts.Rows[i]["warehouseID"]).Trim(), Convert.ToString(dts.Rows[i]["Warehousname"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToDecimal(dts.Rows[i]["qnty"]), Convert.ToString(dts.Rows[i]["BatchWarehouseID"]), Convert.ToString(dts.Rows[i]["BatchWarehousedetailsID"]), Convert.ToString(dts.Rows[i]["BatchID"]), Convert.ToString(dts.Rows[i]["SerialID"]), Convert.ToString(dts.Rows[i]["Warehousname"]), Convert.ToString(dts.Rows[i]["batchNO"]), Convert.ToString(dts.Rows[i]["qntybatch"]), Convert.ToString(dts.Rows[i]["serialno"]), Convert.ToDateTime(dts.Rows[i]["MfgDate"]), Convert.ToDateTime(dts.Rows[i]["ExpiryDate"]), Convert.ToString(dts.Rows[i]["qntybatch"]), "old");
        //                }

        //            }


        //            //Warehousedt.Rows.Add(Warehousedt.Rows.Count + 1, WarehouseID, WarehouseName, BatchName, SerialName, "", "", openingstock, "isnew");

        //        }
        //    }

        //    hdnoldrowcount.Value = Convert.ToString(Warehousedt.Rows.Count);
        //    return Warehousedt;
        //}

        //private int Insertupdatedata(string ProductID, string stockid, string branchid)
        //{
        //    DataSet dsEmail = new DataSet();
        //    string inventryType = string.Empty;
        //    string IsserialActive = "false";

        //    int newrowcount = 0;

        //    #region Insert
        //    String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];
        //    SqlConnection con = new SqlConnection(conn);
        //    SqlCommand cmd3 = new SqlCommand("prc_StockOpeningwarehousentry", con);
        //    cmd3.CommandType = CommandType.StoredProcedure;

        //    cmd3.Parameters.AddWithValue("@compnay", Convert.ToString(Session["LastCompany"]));
        //    cmd3.Parameters.AddWithValue("@Finyear", Convert.ToString(Session["LastFinYear"]));
        //    cmd3.Parameters.AddWithValue("@StockID", Convert.ToInt32(stockid));
        //    cmd3.Parameters.AddWithValue("@ProductID", Convert.ToInt32(ProductID));
        //    cmd3.Parameters.AddWithValue("@branchID", Convert.ToInt32(branchid));
        //    cmd3.Parameters.AddWithValue("@createdBy", Convert.ToInt32(HttpContext.Current.Session["userid"]));
        //    cmd3.Parameters.AddWithValue("@totalqntity", Convert.ToString(hdntotalqnty.Value));
        //    cmd3.Parameters.AddWithValue("@rate", Convert.ToDecimal(hdnrate.Value));
        //    cmd3.Parameters.AddWithValue("@values", Convert.ToDecimal(hdnvalue.Value));

        //    if (hdniswarehouse.Value == "true")
        //    {
        //        inventryType = "WH";

        //    }
        //    if (hdnisbatch.Value == "true")
        //    {

        //        inventryType += "BT";
        //    }
        //    if (hdnisserial.Value == "true")
        //    {
        //        inventryType += "SL";
        //        IsserialActive = "true";
        //    }

        //    cmd3.Parameters.AddWithValue("@IsSerialActive", IsserialActive);


        //    cmd3.Parameters.AddWithValue("@Inventrytype", inventryType);

        //    DataTable temtable = new DataTable();
        //    DataTable temtable2 = new DataTable();
        //    DataTable temtable23 = new DataTable();
        //    if (Session["OpenWarehouseData"] != null)
        //    {
        //        DataTable Warehousedt = (DataTable)Session["OpenWarehouseData"];

        //        temtable = Warehousedt.DefaultView.ToTable(false, "SrlNo", "BatchWarehouseID", "BatchWarehousedetailsID", "BatchID", "SerialID", "WarehouseID", "WarehouseName", "BatchNo", "SerialNo", "MFGDate", "ExpiryDate", "Quantitysum", "isnew");
        //        cmd3.Parameters.AddWithValue("@udt_StockOpeningwarehousentrie", temtable);
        //    }
        //    if (Session["WarehouseDataDelete"] != null)
        //    {
        //        DataTable Warehousedts = (DataTable)Session["WarehouseDataDelete"];

        //        temtable2 = Warehousedts.DefaultView.ToTable(false, "SrlNo", "BatchWarehouseID", "BatchWarehousedetailsID", "BatchID", "SerialID", "WarehouseID", "WarehouseName", "BatchNo", "SerialNo", "MFGDate", "ExpiryDate", "Quantity", "isnew");
        //        cmd3.Parameters.AddWithValue("@udt_StockOpeningwarehousentrieDelete", null);
        //    }
        //    //if (Session["WarehouseUpdatedData"] != null)
        //    //{
        //    //    DataTable Warehousedtups = (DataTable)Session["WarehouseUpdatedData"];

        //    //    temtable23 = Warehousedtups.DefaultView.ToTable(false, "SrlNo", "BatchWarehouseID", "BatchWarehousedetailsID", "BatchID", "SerialID", "WarehouseID", "WarehouseName", "BatchNo", "SerialNo", "MFGDate", "ExpiryDate", "Quantity", "isnew");
        //    //    cmd3.Parameters.AddWithValue("@udt_StockOpeningwarehousentrieUpdate", temtable2);
        //    //}


        //    SqlParameter output = new SqlParameter("@ReturnValue", SqlDbType.Int);
        //    output.Direction = ParameterDirection.Output;
        //    cmd3.Parameters.Add(output);
        //    cmd3.CommandTimeout = 0;
        //    SqlDataAdapter Adap = new SqlDataAdapter();
        //    Adap.SelectCommand = cmd3;
        //    Adap.Fill(dsEmail);
        //    dsEmail.Clear();
        //    cmd3.Dispose();
        //    con.Dispose();
        //    GC.Collect();

        //    #endregion
        //    #region Update
        //    if (Session["OpenWarehouseData"] != null)
        //    {
        //        DataTable dt = new DataTable();
        //        DataTable Warehousedtups = (DataTable)Session["OpenWarehouseData"];
        //        DataRow[] dr = Warehousedtups.Select("isnew = 'Updated'");
        //        if (dr.Count() != 0)
        //        {
        //            dt = dr.CopyToDataTable();


        //            for (int i = 0; i <= dt.Rows.Count - 1; i++)
        //            {
        //                #region WHBTSL
        //                if (hdniswarehouse.Value == "true" && hdnisbatch.Value == "true" && hdnisserial.Value == "true")
        //                {
        //                    DateTime? mfgex;
        //                    DateTime? exdate;
        //                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewMFGDate"])) && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewExpiryDate"])))
        //                    {
        //                        if ((Convert.ToString(dt.Rows[i]["viewMFGDate"]) != "1/1/0001 12:00:00 AM" && Convert.ToString(dt.Rows[i]["viewExpiryDate"]) != "1/1/0001 12:00:00 AM"))
        //                        {
        //                            mfgex = Convert.ToDateTime(dt.Rows[i]["viewMFGDate"]);
        //                            exdate = Convert.ToDateTime(dt.Rows[i]["viewExpiryDate"]);
        //                        }
        //                        else
        //                        {
        //                            mfgex = null;
        //                            exdate = null;
        //                        }

        //                    }
        //                    else
        //                    {
        //                        mfgex = null;
        //                        exdate = null;
        //                    }

        //                    if (Convert.ToString(dt.Rows[i]["viewQuantity"]) != "0" && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewQuantity"])))
        //                    {
        //                        string sql = string.Empty;
        //                        if (mfgex == null && exdate == null)
        //                        {
        //                            if (hdnisolddeleted.Value == "true")
        //                            {

        //                                var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated' AND WarehouseID='" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + "' AND BatchNo='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'");
        //                                if (string.IsNullOrEmpty(Convert.ToString(updateqnty)))
        //                                {
        //                                    updateqnty = Convert.ToDecimal(dt.Rows[i]["Quantitysum"]);
        //                                }

        //                                sql = "update Trans_StockBranchWarehouse set StockBranchWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",StockBranchWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where StockBranchWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
        //                                          "  update Trans_StockBranchWarehouseDetails set Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
        //                                                     "Stock_OpeningValue=ISNULL(isnull(" + Convert.ToDecimal(updateqnty) + ",0)*isnull(" + Convert.ToDecimal(hdnrate.Value) + ",0),0)," +
        //                                                     "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //                                                     "ModifiedDate=GETDATE()" +
        //                                                     "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +
        //                                                     "   update Trans_StockBranchBatch set   StockBranchBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
        //                                                   "StockBranchBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +


        //                                                   "StockBranchBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //                                                   "StockBranchBatch_ModifiedDate=GETDATE() " +
        //                                              "where StockBranchBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
        //                                              "  update Trans_StockSerialMapping" +
        //                                    " Set Stock_BranchSerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
        //                                    " where Stock_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
        //                            }
        //                            else
        //                            {
        //                                sql = "update Trans_StockBranchWarehouse set StockBranchWarehouse_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",StockBranchWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where StockBranchWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
        //                                            "  update Trans_StockBranchWarehouseDetails set Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
        //                                                       "Stock_OpeningValue=ISNULL(isnull(" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",0)*isnull(" + Convert.ToDecimal(hdnrate.Value) + ",0),0)," +
        //                                                       "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //                                                       "ModifiedDate=GETDATE() " +
        //                                                       "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +
        //                                                       "   update Trans_StockBranchBatch set   StockBranchBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
        //                                                     "StockBranchBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +


        //                                                     "StockBranchBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //                                                     "StockBranchBatch_ModifiedDate=GETDATE() " +
        //                                                "where StockBranchBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
        //                                                "  update Trans_StockSerialMapping" +
        //                                      " Set Stock_BranchSerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
        //                                      " where Stock_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (hdnisolddeleted.Value == "true")
        //                            {

        //                                var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated' AND WarehouseID='" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + "' AND BatchNo='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'");
        //                                if (string.IsNullOrEmpty(Convert.ToString(updateqnty)))
        //                                {
        //                                    updateqnty = Convert.ToDecimal(dt.Rows[i]["Quantitysum"]);
        //                                }
        //                                sql = "update Trans_StockBranchWarehouse set StockBranchWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",StockBranchWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where StockBranchWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
        //                                              "  update Trans_StockBranchWarehouseDetails set Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
        //                                                         "Stock_OpeningValue=ISNULL(isnull(" + Convert.ToDecimal(updateqnty) + ",0)*isnull(" + Convert.ToDecimal(hdnrate.Value) + ",0),0)," +
        //                                                         "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //                                                         "ModifiedDate=GETDATE()" +
        //                                                         "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +
        //                                                         "   update Trans_StockBranchBatch set   StockBranchBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
        //                                                       "StockBranchBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +

        //                                                       "StockBranchBatch_MfgDate='" + mfgex + "'," +
        //                                                       "StockBranchBatch_ExpiryDate='" + exdate + "'," +
        //                                                       "StockBranchBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //                                                       "StockBranchBatch_ModifiedDate=GETDATE() " +
        //                                                  "where StockBranchBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
        //                                                  "  update Trans_StockSerialMapping" +
        //                                        " Set Stock_BranchSerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
        //                                        " where Stock_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
        //                            }
        //                            else
        //                            {
        //                                sql = "update Trans_StockBranchWarehouse set StockBranchWarehouse_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",StockBranchWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where StockBranchWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
        //                                         "  update Trans_StockBranchWarehouseDetails set Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
        //                                                    "Stock_OpeningValue=ISNULL(isnull(" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",0)*isnull(" + Convert.ToDecimal(hdnrate.Value) + ",0),0)," +
        //                                                    "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //                                                    "ModifiedDate=GETDATE()" +
        //                                                    "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +
        //                                                    "   update Trans_StockBranchBatch set   StockBranchBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
        //                                                  "StockBranchBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +

        //                                                  "StockBranchBatch_MfgDate='" + mfgex + "'," +
        //                                                  "StockBranchBatch_ExpiryDate='" + exdate + "'," +
        //                                                  "StockBranchBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //                                                  "StockBranchBatch_ModifiedDate=GETDATE() " +
        //                                             "where StockBranchBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
        //                                             "  update Trans_StockSerialMapping" +
        //                                   " Set Stock_BranchSerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
        //                                   " where Stock_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
        //                            }


        //                        }


        //                        oDBEngine.GetDataTable(sql);

        //                    }
        //                    else
        //                    {
        //                        //var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated' AND WarehouseID='" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + "' AND BatchNo='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'");


        //                        string sql = "update Trans_StockBranchWarehouse set StockBranchWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where StockBranchWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
        //                                      "  update Trans_StockBranchWarehouseDetails set " +
        //                                                 "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //                                                 "ModifiedDate=GETDATE() " +
        //                                                 "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +

        //                                          "  update Trans_StockSerialMapping" +
        //                                " Set Stock_BranchSerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
        //                                " where Stock_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";

        //                        oDBEngine.GetDataTable(sql);
        //                    }
        //                }
        //                #endregion
        //                #region WHBT
        //                if (hdniswarehouse.Value == "true" && hdnisbatch.Value == "true" && hdnisserial.Value != "true")
        //                {
        //                    DateTime? mfgex;
        //                    DateTime? exdate;

        //                    var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated' AND BatchWarehouseID='" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "'");
        //                    var OLDSAMEwarehouse = Warehousedtups.Compute("sum(Quantitysum)", "isnew = 'old' AND BatchWarehouseID='" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "'");
        //                    if (!string.IsNullOrEmpty(Convert.ToString(OLDSAMEwarehouse)) && !string.IsNullOrEmpty(Convert.ToString(updateqnty)))
        //                    {

        //                        updateqnty = Convert.ToDecimal(updateqnty) + Convert.ToDecimal(OLDSAMEwarehouse);
        //                    }

        //                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewMFGDate"])) && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewExpiryDate"])))
        //                    {
        //                        if ((Convert.ToString(dt.Rows[i]["viewMFGDate"]) != "1/1/0001 12:00:00 AM" && Convert.ToString(dt.Rows[i]["viewExpiryDate"]) != "1/1/0001 12:00:00 AM"))
        //                        {
        //                            mfgex = Convert.ToDateTime(dt.Rows[i]["viewMFGDate"]);
        //                            exdate = Convert.ToDateTime(dt.Rows[i]["viewExpiryDate"]);
        //                        }
        //                        else
        //                        {
        //                            mfgex = null;
        //                            exdate = null;
        //                        }

        //                    }
        //                    else
        //                    {
        //                        mfgex = null;
        //                        exdate = null;
        //                    }

        //                    if (Convert.ToString(dt.Rows[i]["viewQuantity"]) != "0" && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewQuantity"])) && !string.IsNullOrEmpty(Convert.ToString(updateqnty)))
        //                    {
        //                        string sql = string.Empty;
        //                        if (mfgex == null && exdate == null)
        //                        {
        //                            sql = "update Trans_StockBranchWarehouse set StockBranchWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",StockBranchWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where StockBranchWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
        //                                      "  update Trans_StockBranchWarehouseDetails set Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
        //                                                 "Stock_OpeningValue=ISNULL(isnull(" + Convert.ToDecimal(updateqnty) + ",0)*isnull(" + Convert.ToDecimal(hdnrate.Value) + ",0),0)," +
        //                                                 "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //                                                 "ModifiedDate=GETDATE() " +
        //                                                 "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +
        //                                                 "   update Trans_StockBranchBatch set   StockBranchBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
        //                                               "StockBranchBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +


        //                                               "StockBranchBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //                                               "StockBranchBatch_ModifiedDate=GETDATE() " +
        //                                          "where StockBranchBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]);


        //                        }
        //                        else
        //                        {
        //                            sql = "update Trans_StockBranchWarehouse set StockBranchWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",StockBranchWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where StockBranchWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
        //                                          "  update Trans_StockBranchWarehouseDetails set Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
        //                                                     "Stock_OpeningValue=ISNULL(isnull(" + Convert.ToDecimal(updateqnty) + ",0)*isnull(" + Convert.ToDecimal(hdnrate.Value) + ",0),0)," +
        //                                                     "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //                                                     "ModifiedDate=GETDATE() " +
        //                                                     "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +
        //                                                     "   update Trans_StockBranchBatch set   StockBranchBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
        //                                                   "StockBranchBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +

        //                                                   "StockBranchBatch_MfgDate='" + mfgex + "'," +
        //                                                   "StockBranchBatch_ExpiryDate='" + exdate + "'," +
        //                                                   "StockBranchBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //                                                   "StockBranchBatch_ModifiedDate=GETDATE() " +
        //                                              "where StockBranchBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]);
        //                        }


        //                        oDBEngine.GetDataTable(sql);

        //                    }


        //                }
        //                #endregion
        //                #region WHSL
        //                if (hdniswarehouse.Value == "true" && hdnisbatch.Value != "true" && hdnisserial.Value == "true")
        //                {
        //                    DateTime? mfgex;
        //                    DateTime? exdate;
        //                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewMFGDate"])) && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewExpiryDate"])))
        //                    {
        //                        if ((Convert.ToString(dt.Rows[i]["viewMFGDate"]) != "1/1/0001 12:00:00 AM" && Convert.ToString(dt.Rows[i]["viewExpiryDate"]) != "1/1/0001 12:00:00 AM"))
        //                        {
        //                            mfgex = Convert.ToDateTime(dt.Rows[i]["viewMFGDate"]);
        //                            exdate = Convert.ToDateTime(dt.Rows[i]["viewExpiryDate"]);
        //                        }
        //                        else
        //                        {
        //                            mfgex = null;
        //                            exdate = null;
        //                        }

        //                    }
        //                    else
        //                    {
        //                        mfgex = null;
        //                        exdate = null;
        //                    }

        //                    if (Convert.ToString(dt.Rows[i]["viewQuantity"]) != "0" && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewQuantity"])))
        //                    {
        //                        string sql = string.Empty;
        //                        if (mfgex == null && exdate == null)
        //                        {
        //                            sql = "update Trans_StockBranchWarehouse set StockBranchWarehouse_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",StockBranchWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where StockBranchWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
        //                                      "  update Trans_StockBranchWarehouseDetails set Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
        //                                                 "Stock_OpeningValue=ISNULL(isnull(" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",0)*isnull(" + Convert.ToDecimal(hdnrate.Value) + ",0),0)," +
        //                                                 "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //                                                 "ModifiedDate=GETDATE()" +
        //                                                 "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +
        //                                                 "   update Trans_StockBranchBatch set   StockBranchBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
        //                                               "StockBranchBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +


        //                                               "StockBranchBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //                                               "StockBranchBatch_ModifiedDate=GETDATE()" +
        //                                          "where StockBranchBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
        //                                          "  update Trans_StockSerialMapping" +
        //                                " Set Stock_BranchSerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
        //                                " where Stock_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";

        //                        }
        //                        else
        //                        {
        //                            sql = "update Trans_StockBranchWarehouse set StockBranchWarehouse_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",StockBranchWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where StockBranchWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
        //                                          "  update Trans_StockBranchWarehouseDetails set Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
        //                                                     "Stock_OpeningValue=ISNULL(isnull(" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",0)*isnull(" + Convert.ToDecimal(hdnrate.Value) + ",0),0)," +
        //                                                     "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //                                                     "ModifiedDate=GETDATE()" +
        //                                                     "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +
        //                                                     "   update Trans_StockBranchBatch set   StockBranchBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
        //                                                   "StockBranchBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +

        //                                                   "StockBranchBatch_MfgDate='" + mfgex + "'," +
        //                                                   "StockBranchBatch_ExpiryDate='" + exdate + "'," +
        //                                                   "StockBranchBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //                                                   "StockBranchBatch_ModifiedDate=GETDATE()" +
        //                                              "where StockBranchBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
        //                                              "  update Trans_StockSerialMapping" +
        //                                    " Set Stock_BranchSerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
        //                                    " where Stock_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
        //                        }


        //                        oDBEngine.GetDataTable(sql);

        //                    }
        //                    else
        //                    {
        //                        //var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated' AND WarehouseID='" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + "' AND BatchNo='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'");


        //                        string sql = "update Trans_StockBranchWarehouse set StockBranchWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where StockBranchWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
        //                                      "  update Trans_StockBranchWarehouseDetails set " +
        //                                                 "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //                                                 "ModifiedDate=GETDATE()" +
        //                                                 "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +

        //                                          "  update Trans_StockSerialMapping" +
        //                                " Set Stock_BranchSerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
        //                                " where Stock_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";

        //                        oDBEngine.GetDataTable(sql);
        //                    }
        //                }
        //                #endregion
        //                //#region BTSL
        //                //if (hdniswarehouse.Value != "true" && hdnisbatch.Value == "true" && hdnisserial.Value == "true")
        //                //{
        //                //    DateTime? mfgex;
        //                //    DateTime? exdate;
        //                //    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewMFGDate"])) && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewExpiryDate"])))
        //                //    {
        //                //        if ((Convert.ToString(dt.Rows[i]["viewMFGDate"]) != "1/1/0001 12:00:00 AM" && Convert.ToString(dt.Rows[i]["viewExpiryDate"]) != "1/1/0001 12:00:00 AM"))
        //                //        {
        //                //            mfgex = Convert.ToDateTime(dt.Rows[i]["viewMFGDate"]);
        //                //            exdate = Convert.ToDateTime(dt.Rows[i]["viewExpiryDate"]);
        //                //        }
        //                //        else
        //                //        {
        //                //            mfgex = null;
        //                //            exdate = null;
        //                //        }

        //                //    }
        //                //    else
        //                //    {
        //                //        mfgex = null;
        //                //        exdate = null;
        //                //    }

        //                //    if (Convert.ToString(dt.Rows[i]["viewQuantity"]) != "0" && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewQuantity"])))
        //                //    {
        //                //        string sql = string.Empty;
        //                //        if (mfgex == null && exdate == null)
        //                //        {
        //                //            sql = "update Trans_StockBranchWarehouse set StockBranchWarehouse_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",StockBranchWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where StockBranchWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
        //                //                      "  update Trans_StockBranchWarehouseDetails set Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
        //                //                                 "Stock_OpeningValue=ISNULL(isnull(" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",0)*isnull(" + Convert.ToDecimal(hdnrate.Value) + ",0),0)," +
        //                //                                 "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //                //                                 "ModifiedDate=GETDATE()" +
        //                //                                 "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +
        //                //                                 "   update Trans_StockBranchBatch set   StockBranchBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
        //                //                               "StockBranchBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +


        //                //                               "StockBranchBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //                //                               "StockBranchBatch_ModifiedDate=GETDATE()" +
        //                //                          "where StockBranchBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
        //                //                          "  update Trans_StockSerialMapping" +
        //                //                " Set Stock_BranchSerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
        //                //                " where Stock_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";

        //                //        }
        //                //        else
        //                //        {
        //                //            sql = "update Trans_StockBranchWarehouse set StockBranchWarehouse_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",StockBranchWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where StockBranchWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
        //                //                          "  update Trans_StockBranchWarehouseDetails set Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
        //                //                                     "Stock_OpeningValue=ISNULL(isnull(" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",0)*isnull(" + Convert.ToDecimal(hdnrate.Value) + ",0),0)," +
        //                //                                     "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //                //                                     "ModifiedDate=GETDATE()" +
        //                //                                     "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +
        //                //                                     "   update Trans_StockBranchBatch set   StockBranchBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
        //                //                                   "StockBranchBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +

        //                //                                   "StockBranchBatch_MfgDate='" + mfgex + "'," +
        //                //                                   "StockBranchBatch_ExpiryDate='" + exdate + "'," +
        //                //                                   "StockBranchBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //                //                                   "StockBranchBatch_ModifiedDate=GETDATE()" +
        //                //                              "where StockBranchBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
        //                //                              "  update Trans_StockSerialMapping" +
        //                //                    " Set Stock_BranchSerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
        //                //                    " where Stock_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
        //                //        }


        //                //        oDBEngine.GetDataTable(sql);

        //                //    }
        //                //    else
        //                //    {
        //                //        //var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated' AND WarehouseID='" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + "' AND BatchNo='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'");


        //                //        string sql = "update Trans_StockBranchWarehouse set StockBranchWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where StockBranchWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
        //                //                      "  update Trans_StockBranchWarehouseDetails set " +
        //                //                                 "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //                //                                 "ModifiedDate=GETDATE()" +
        //                //                                 "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +

        //                //                          "  update Trans_StockSerialMapping" +
        //                //                " Set Stock_BranchSerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
        //                //                " where Stock_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";

        //                //        oDBEngine.GetDataTable(sql);
        //                //    }
        //                //}
        //                //#endregion
        //                #region WH
        //                if (hdniswarehouse.Value == "true" && hdnisbatch.Value != "true" && hdnisserial.Value != "true")
        //                {
        //                    if (Convert.ToString(dt.Rows[i]["viewQuantity"]) != "0" && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Quantity"])))
        //                    {

        //                        string sql = "update Trans_StockBranchWarehouse set StockBranchWarehouse_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantity"]) + ",StockBranchWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where StockBranchWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
        //                                    "  update Trans_StockBranchWarehouseDetails set Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
        //                                               " Stock_OpeningValue=ISNULL(isnull(" + Convert.ToDecimal(dt.Rows[i]["Quantity"]) + ",0)*isnull(" + Convert.ToDecimal(hdnrate.Value) + ",0),0)," +
        //                                               " ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //                                               " ModifiedDate=GETDATE() " +
        //                                               " where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +
        //                                               "   update Trans_StockBranchBatch set   StockBranchBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
        //                                             "StockBranchBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantity"]) + "," +


        //                                             "StockBranchBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //                                             "StockBranchBatch_ModifiedDate=GETDATE() " +
        //                                        " where StockBranchBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]);




        //                        oDBEngine.GetDataTable(sql);

        //                    }


        //                }
        //                #endregion

        //                #region BTSL
        //                if (hdniswarehouse.Value != "true" && hdnisbatch.Value == "true" && hdnisserial.Value == "true")
        //                {
        //                    DateTime? mfgex;
        //                    DateTime? exdate;
        //                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewMFGDate"])) && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewExpiryDate"])))
        //                    {
        //                        if ((Convert.ToString(dt.Rows[i]["viewMFGDate"]) != "1/1/0001 12:00:00 AM" && Convert.ToString(dt.Rows[i]["viewExpiryDate"]) != "1/1/0001 12:00:00 AM"))
        //                        {
        //                            mfgex = Convert.ToDateTime(dt.Rows[i]["viewMFGDate"]);
        //                            exdate = Convert.ToDateTime(dt.Rows[i]["viewExpiryDate"]);
        //                        }
        //                        else
        //                        {
        //                            mfgex = null;
        //                            exdate = null;
        //                        }

        //                    }
        //                    else
        //                    {
        //                        mfgex = null;
        //                        exdate = null;
        //                    }

        //                    if (Convert.ToString(dt.Rows[i]["viewQuantity"]) != "0" && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewQuantity"])))
        //                    {
        //                        string sql = string.Empty;
        //                        if (mfgex == null && exdate == null)
        //                        {
        //                            if (hdnisolddeleted.Value == "true")
        //                            {

        //                                var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated'  AND BatchNo='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'");
        //                                if (string.IsNullOrEmpty(Convert.ToString(updateqnty)))
        //                                {
        //                                    updateqnty = Convert.ToDecimal(dt.Rows[i]["Quantitysum"]);
        //                                }

        //                                sql = "update Trans_StockBranchWarehouse set StockBranchWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where StockBranchWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
        //                                          "  update Trans_StockBranchWarehouseDetails set Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
        //                                                     "Stock_OpeningValue=ISNULL(isnull(" + Convert.ToDecimal(updateqnty) + ",0)*isnull(" + Convert.ToDecimal(hdnrate.Value) + ",0),0)," +
        //                                                     "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //                                                     "ModifiedDate=GETDATE()" +
        //                                                     "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +
        //                                                     "   update Trans_StockBranchBatch set   StockBranchBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
        //                                                   "StockBranchBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +


        //                                                   "StockBranchBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //                                                   "StockBranchBatch_ModifiedDate=GETDATE() " +
        //                                              "where StockBranchBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
        //                                              "  update Trans_StockSerialMapping" +
        //                                    " Set Stock_BranchSerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
        //                                    " where Stock_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
        //                            }
        //                            else
        //                            {
        //                                sql = "update Trans_StockBranchWarehouse set StockBranchWarehouse_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where StockBranchWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
        //                                            "  update Trans_StockBranchWarehouseDetails set Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
        //                                                       "Stock_OpeningValue=ISNULL(isnull(" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",0)*isnull(" + Convert.ToDecimal(hdnrate.Value) + ",0),0)," +
        //                                                       "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //                                                       "ModifiedDate=GETDATE() " +
        //                                                       "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +
        //                                                       "   update Trans_StockBranchBatch set   StockBranchBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
        //                                                     "StockBranchBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +


        //                                                     "StockBranchBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //                                                     "StockBranchBatch_ModifiedDate=GETDATE() " +
        //                                                "where StockBranchBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
        //                                                "  update Trans_StockSerialMapping" +
        //                                      " Set Stock_BranchSerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
        //                                      " where Stock_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (hdnisolddeleted.Value == "true")
        //                            {

        //                                var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated' AND BatchNo='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'");
        //                                if (string.IsNullOrEmpty(Convert.ToString(updateqnty)))
        //                                {
        //                                    updateqnty = Convert.ToDecimal(dt.Rows[i]["Quantitysum"]);
        //                                }
        //                                sql = "update Trans_StockBranchWarehouse set StockBranchWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where StockBranchWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
        //                                              "  update Trans_StockBranchWarehouseDetails set Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
        //                                                         "Stock_OpeningValue=ISNULL(isnull(" + Convert.ToDecimal(updateqnty) + ",0)*isnull(" + Convert.ToDecimal(hdnrate.Value) + ",0),0)," +
        //                                                         "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //                                                         "ModifiedDate=GETDATE()" +
        //                                                         "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +
        //                                                         "   update Trans_StockBranchBatch set   StockBranchBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
        //                                                       "StockBranchBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +

        //                                                       "StockBranchBatch_MfgDate='" + mfgex + "'," +
        //                                                       "StockBranchBatch_ExpiryDate='" + exdate + "'," +
        //                                                       "StockBranchBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //                                                       "StockBranchBatch_ModifiedDate=GETDATE() " +
        //                                                  "where StockBranchBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
        //                                                  "  update Trans_StockSerialMapping" +
        //                                        " Set Stock_BranchSerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
        //                                        " where Stock_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
        //                            }
        //                            else
        //                            {
        //                                sql = "update Trans_StockBranchWarehouse set StockBranchWarehouse_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where StockBranchWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
        //                                         "  update Trans_StockBranchWarehouseDetails set Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
        //                                                    "Stock_OpeningValue=ISNULL(isnull(" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + ",0)*isnull(" + Convert.ToDecimal(hdnrate.Value) + ",0),0)," +
        //                                                    "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //                                                    "ModifiedDate=GETDATE()" +
        //                                                    "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +
        //                                                    "   update Trans_StockBranchBatch set   StockBranchBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
        //                                                  "StockBranchBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +

        //                                                  "StockBranchBatch_MfgDate='" + mfgex + "'," +
        //                                                  "StockBranchBatch_ExpiryDate='" + exdate + "'," +
        //                                                  "StockBranchBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //                                                  "StockBranchBatch_ModifiedDate=GETDATE() " +
        //                                             "where StockBranchBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]) + "	  " +
        //                                             "  update Trans_StockSerialMapping" +
        //                                   " Set Stock_BranchSerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
        //                                   " where Stock_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";
        //                            }


        //                        }


        //                        oDBEngine.GetDataTable(sql);

        //                    }
        //                    else
        //                    {
        //                        //var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated' AND WarehouseID='" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + "' AND BatchNo='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'");


        //                        string sql = "update Trans_StockBranchWarehouse set StockBranchWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where StockBranchWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
        //                                      "  update Trans_StockBranchWarehouseDetails set " +
        //                                                 "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //                                                 "ModifiedDate=GETDATE() " +
        //                                                 "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +

        //                                          "  update Trans_StockSerialMapping" +
        //                                " Set Stock_BranchSerialNumber='" + Convert.ToString(dt.Rows[i]["SerialNo"]) + "'" +
        //                                " where Stock_MapId=" + Convert.ToString(dt.Rows[i]["SerialID"]) + "" + "";

        //                        oDBEngine.GetDataTable(sql);
        //                    }
        //                }
        //                #endregion

        //                #region BT
        //                if (hdniswarehouse.Value != "true" && hdnisbatch.Value == "true" && hdnisserial.Value != "true")
        //                {
        //                    DateTime? mfgex;
        //                    DateTime? exdate;

        //                    var updateqnty = dt.Compute("sum(Quantitysum)", "isnew = 'Updated' AND BatchWarehouseID='" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "'");
        //                    var OLDSAMEwarehouse = Warehousedtups.Compute("sum(Quantitysum)", "isnew = 'old' AND BatchWarehouseID='" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "'");
        //                    if (!string.IsNullOrEmpty(Convert.ToString(OLDSAMEwarehouse)) && !string.IsNullOrEmpty(Convert.ToString(updateqnty)))
        //                    {

        //                        updateqnty = Convert.ToDecimal(updateqnty) + Convert.ToDecimal(OLDSAMEwarehouse);
        //                    }

        //                    if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewMFGDate"])) && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewExpiryDate"])))
        //                    {
        //                        if ((Convert.ToString(dt.Rows[i]["viewMFGDate"]) != "1/1/0001 12:00:00 AM" && Convert.ToString(dt.Rows[i]["viewExpiryDate"]) != "1/1/0001 12:00:00 AM"))
        //                        {
        //                            mfgex = Convert.ToDateTime(dt.Rows[i]["viewMFGDate"]);
        //                            exdate = Convert.ToDateTime(dt.Rows[i]["viewExpiryDate"]);
        //                        }
        //                        else
        //                        {
        //                            mfgex = null;
        //                            exdate = null;
        //                        }

        //                    }
        //                    else
        //                    {
        //                        mfgex = null;
        //                        exdate = null;
        //                    }

        //                    if (Convert.ToString(dt.Rows[i]["viewQuantity"]) != "0" && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["viewQuantity"])) && !string.IsNullOrEmpty(Convert.ToString(updateqnty)))
        //                    {
        //                        string sql = string.Empty;
        //                        if (mfgex == null && exdate == null)
        //                        {
        //                            sql = "update Trans_StockBranchWarehouse set StockBranchWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",StockBranchWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where StockBranchWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
        //                                      "  update Trans_StockBranchWarehouseDetails set Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
        //                                                 "Stock_OpeningValue=ISNULL(isnull(" + Convert.ToDecimal(updateqnty) + ",0)*isnull(" + Convert.ToDecimal(hdnrate.Value) + ",0),0)," +
        //                                                 "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //                                                 "ModifiedDate=GETDATE() " +
        //                                                 "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +
        //                                                 "   update Trans_StockBranchBatch set   StockBranchBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
        //                                               "StockBranchBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +


        //                                               "StockBranchBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //                                               "StockBranchBatch_ModifiedDate=GETDATE() " +
        //                                          "where StockBranchBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]);


        //                        }
        //                        else
        //                        {
        //                            sql = "update Trans_StockBranchWarehouse set StockBranchWarehouse_StockIn=" + Convert.ToDecimal(updateqnty) + ",StockBranchWarehouse_WarehouseId=" + Convert.ToString(dt.Rows[i]["WarehouseID"]) + ",ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + ",ModifiedDate=getdate() where StockBranchWarehouse_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "" +
        //                                          "  update Trans_StockBranchWarehouseDetails set Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
        //                                                     "Stock_OpeningValue=ISNULL(isnull(" + Convert.ToDecimal(updateqnty) + ",0)*isnull(" + Convert.ToDecimal(hdnrate.Value) + ",0),0)," +
        //                                                     "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //                                                     "ModifiedDate=GETDATE() " +
        //                                                     "where StockBranchWarehouseDetail_Id=" + Convert.ToString(dt.Rows[i]["BatchWarehousedetailsID"]) + "" +
        //                                                     "   update Trans_StockBranchBatch set   StockBranchBatch_BatchNumber='" + Convert.ToString(dt.Rows[i]["BatchNo"]) + "'," +
        //                                                   "StockBranchBatch_StockIn=" + Convert.ToDecimal(dt.Rows[i]["Quantitysum"]) + "," +

        //                                                   "StockBranchBatch_MfgDate='" + mfgex + "'," +
        //                                                   "StockBranchBatch_ExpiryDate='" + exdate + "'," +
        //                                                   "StockBranchBatch_ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //                                                   "StockBranchBatch_ModifiedDate=GETDATE() " +
        //                                              "where StockBranchBatch_Id=" + Convert.ToString(dt.Rows[i]["BatchID"]);
        //                        }


        //                        oDBEngine.GetDataTable(sql);

        //                    }


        //                }
        //                #endregion

        //            }
        //        }
        //    }

        //    #endregion
        //    #region delete

        //    if (Session["WarehouseDataDelete"] != null)
        //    {

        //        deleteALL(stockid);
        //        //DataTable Warehousedtups = (DataTable)Session["WarehouseDataDelete"];
        //        //for (int i = 0; i <= Warehousedtups.Rows.Count - 1; i++)
        //        //{

        //        //    if (Convert.ToString(Warehousedtups.Rows[i]["isnew"]) == "DeleteSL")
        //        //    {
        //        //        string sqls = "delete Trans_StockSerialMapping where Stock_MapId=" + Convert.ToString(Warehousedtups.Rows[i]["SerialID"]) + " " +
        //        //                            " update Trans_Stock set Stock_Open=isnull(Stock_Open,0)-1," +
        //        //                    " Stock_ModifiedDate=GETDATE() WHERE Stock_ID=" + stockid + "	";

        //        //        oDBEngine.GetDataTable(sqls);
        //        //    }
        //        //    else if (Convert.ToString(Warehousedtups.Rows[i]["isnew"]) == "DeleteWHBTSL")
        //        //    {
        //        //        var updateqnty = Warehousedtups.Compute("sum(Quantitysum)", "isnew = 'DeleteWHBTSL' AND BatchWarehouseID='" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"]) + "'");

        //        //        string sqls = "delete from Trans_StockBranchWarehouseDetails where StockBranchWarehouseDetail_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]) + " " +
        //        //                      " delete from Trans_StockBranchWarehouse where StockBranchWarehouse_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"]) + " " +
        //        //                      " delete from Trans_StockBranchBatch where StockBranchWarehouseDetail_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]) + " " +
        //        //                      " delete from Trans_StockSerialMapping where Stock_BranchBatchId=" + Convert.ToString(Warehousedtups.Rows[i]["BatchID"]) + " " +
        //        //                           " update Trans_Stock set Stock_Open=isnull(Stock_Open,0)-" + updateqnty + "," +
        //        //                   " Stock_ModifiedDate=GETDATE() WHERE Stock_ID=" + stockid + "	";

        //        //        if (!string.IsNullOrEmpty(Convert.ToString(updateqnty)))
        //        //        {
        //        //            oDBEngine.GetDataTable(sqls);
        //        //        }

        //        //    }
        //        //    else if (Convert.ToString(Warehousedtups.Rows[i]["isnew"]) == "DeleteWHSL")
        //        //    {

        //        //        var updateqnty = Warehousedtups.Compute("sum(Quantitysum)", "isnew = 'DeleteWHSL' AND BatchWarehouseID='" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"]) + "'");

        //        //        string sqls = "delete from Trans_StockBranchWarehouseDetails where StockBranchWarehouseDetail_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]) + " " +
        //        //                      " delete from Trans_StockBranchWarehouse where StockBranchWarehouse_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"]) + " " +
        //        //                      " delete from Trans_StockBranchBatch where StockBranchWarehouseDetail_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]) + " " +
        //        //                      " delete from Trans_StockSerialMapping where Stock_BranchBatchId=" + Convert.ToString(Warehousedtups.Rows[i]["BatchID"]) + " " +
        //        //                           " update Trans_Stock set Stock_Open=isnull(Stock_Open,0)-" + updateqnty + "," +
        //        //                   " Stock_ModifiedDate=GETDATE() WHERE Stock_ID=" + stockid + "	";
        //        //        if (!string.IsNullOrEmpty(Convert.ToString(updateqnty)))
        //        //        {
        //        //            oDBEngine.GetDataTable(sqls);
        //        //        }


        //        //    }
        //        //    else if (Convert.ToString(Warehousedtups.Rows[i]["isnew"]) == "DeleteWHBT")
        //        //    {
        //        //        string sqls = string.Empty;
        //        //        var updateqnty = Warehousedtups.Compute("sum(Quantitysum)", "isnew = 'DeleteWHBT' AND BatchWarehouseID='" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"]) + "'");

        //        //        if (string.IsNullOrEmpty(Convert.ToString(updateqnty)))
        //        //        { updateqnty = 0.0; }

        //        //        if (string.IsNullOrEmpty(Convert.ToString(Warehousedtups.Rows[i]["viewWarehouseName"])))
        //        //        {

        //        //            sqls = "update Trans_StockBranchWarehouse set StockBranchWarehouse_StockIn=ISNULL(StockBranchWarehouse_StockIn,0)-" + updateqnty + " where StockBranchWarehouse_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"]) + " " +
        //        //                      " update Trans_StockBranchWarehouseDetails set Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
        //        //                                             "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //        //                                             "ModifiedDate=GETDATE()" +
        //        //                                             "where StockBranchWarehouseDetail_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]) + "" +
        //        //                      " delete from Trans_StockBranchBatch where StockBranchBatch_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchID"]) + " " +

        //        //                           " update Trans_Stock set Stock_Open=isnull(Stock_Open,0)-" + updateqnty + "," +
        //        //                   " Stock_ModifiedDate=GETDATE() WHERE Stock_ID=" + stockid + "	";

        //        //        }
        //        //        else
        //        //        {
        //        //            sqls = "delete from Trans_StockBranchWarehouseDetails where StockBranchWarehouseDetail_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]) + " " +
        //        //                         " delete from Trans_StockBranchWarehouse where StockBranchWarehouse_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"]) + " " +
        //        //                         " delete from Trans_StockBranchBatch where StockBranchWarehouseDetail_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]) + " " +

        //        //                              " update Trans_Stock set Stock_Open=isnull(Stock_Open,0)-" + updateqnty + "," +
        //        //                      " Stock_ModifiedDate=GETDATE() WHERE Stock_ID=" + stockid + "	";
        //        //        }

        //        //        oDBEngine.GetDataTable(sqls);


        //        //    }
        //        //    else if (Convert.ToString(Warehousedtups.Rows[i]["isnew"]) == "DeleteWH")
        //        //    {

        //        //        var updateqnty = Warehousedtups.Compute("sum(Quantity)", "isnew = 'DeleteWH' AND BatchWarehouseID='" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"]) + "'");

        //        //        string sqls = "delete from Trans_StockBranchWarehouseDetails where StockBranchWarehouseDetail_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]) + " " +
        //        //                      " delete from Trans_StockBranchWarehouse where StockBranchWarehouse_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"]) + " " +
        //        //                      " delete from Trans_StockBranchBatch where StockBranchWarehouseDetail_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]) + " " +

        //        //                           " update Trans_Stock set Stock_Open=isnull(Stock_Open,0)-" + updateqnty + "," +
        //        //                   " Stock_ModifiedDate=GETDATE() WHERE Stock_ID=" + stockid + "	";

        //        //        if (!string.IsNullOrEmpty(Convert.ToString(updateqnty)))
        //        //        {
        //        //            oDBEngine.GetDataTable(sqls);
        //        //        }

        //        //    }

        //        //}
        //        //var updateqnty = Warehousedtups.Compute("sum(Quantitysum)", "isnew = 'Updated' AND BatchWarehouseID='" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "'");

        //    }
        //    #endregion

        //    return 1;


        //}
        protected void GrdWarehouse_DataBinding(object sender, EventArgs e)
        {
            if (Session["OpenWarehouseData"] != null)
            {
                DataTable Warehousedt = (DataTable)Session["OpenWarehouseData"];

                GrdWarehouse.DataSource = Warehousedt;
            }
        }

        protected void CmbWarehouse_Callback(object sender, CallbackEventArgsBase e)
        {
            string WhichCall = e.Parameter.Split('~')[0];

            //if (WhichCall == "BindWarehouse")
            //{
            //    DataTable dt = GetWarehouseData();
            //    GetAvailableStock();

            //    CmbWarehouse.Items.Clear();
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        CmbWarehouse.Items.Add(Convert.ToString(dt.Rows[i]["WarehouseName"]), Convert.ToString(dt.Rows[i]["WarehouseID"]));
            //    }
            //    if (hdndefaultID != null || hdndefaultID.Value != "")
            //        CmbWarehouse.Value = hdndefaultID.Value;
            //}

        }

        public DataTable GetWarehouseData()
        {
            DataTable dt = new DataTable();
            dt = oDBEngine.GetDataTable("select  b.bui_id as WarehouseID,b.bui_Name as WarehouseName from tbl_master_building b order by b.bui_Name");
            return dt;
        }

        //public void GetAvailableStock()
        //{
        //    //DataTable dt2 = oDBEngine.GetDataTable("select ISnull(ISNULL(tblwarehous.StockBranchWarehouse_StockIn,0)- isnull(tblwarehous.StockBranchWarehouse_StockOut,0),0) as branchopenstock from Trans_StockBranchWarehouse tblwarehous where tblwarehous.StockBranchWarehouse_StockId=" + hdfstockid.Value + " and tblwarehous.StockBranchWarehouse_CompanyId='" + Convert.ToString(Session["LastCompany"]) + "' and tblwarehous.StockBranchWarehouse_FinYear='" + Convert.ToString(Session["LastFinYear"]) + "' and tblwarehous.StockBranchWarehouse_BranchId=" + hdbranchID.Value + "");
        //    try
        //    {
        //        DataTable dt2 = oDBEngine.GetDataTable("Select dbo.fn_CheckAvailableStockOpening(" + hdbranchID.Value + ",'" + Convert.ToString(Session["LastCompany"]) + "','" + Convert.ToString(Session["LastFinYear"]) + "'," + hdfstockid.Value + ") as branchopenstock");

        //        if (dt2.Rows.Count > 0)
        //        {
        //            //lblAvailableStk.Text =

        //            //CmbWarehouse.JSProperties["cpstock"] = String.Format(((Math.Round(Convert.ToDecimal(dt2.Rows[0]["branchopenstock"])) == Convert.ToDecimal(dt2.Rows[0]["branchopenstock"])) ? "{0:0}" : "{0:0.0000}"), Convert.ToDecimal(dt2.Rows[0]["branchopenstock"]));
        //            CmbWarehouse.JSProperties["cpstock"] = Convert.ToString(dt2.Rows[0]["branchopenstock"]);
        //        }
        //        else
        //        {
        //            //CmbWarehouse.JSProperties["cpstock"] = Convert.ToString(hdfopeningstock.Value);
        //            CmbWarehouse.JSProperties["cpstock"] = "0.0000";
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        //public DataTable GetStockWarehouseData(string stockid)
        //{
        //    DataTable dt = new DataTable();
        //    DataTable dt2 = new DataTable();

        //    DataSet dsInst = new DataSet();
        //    //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
        //    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
        //    SqlCommand cmd = new SqlCommand("prc_StockGetOpeningwarehousentry", con);
        //    cmd.CommandType = CommandType.StoredProcedure;

        //    cmd.Parameters.AddWithValue("@StockID", Convert.ToInt32(stockid));
        //    cmd.Parameters.AddWithValue("@ProductID", Convert.ToInt32(hdfProductID.Value));
        //    cmd.Parameters.AddWithValue("@branchID", Convert.ToInt32(hdbranchID.Value));
        //    cmd.Parameters.AddWithValue("@compnay", Convert.ToString(Session["LastCompany"]));
        //    cmd.Parameters.AddWithValue("@Finyear", Convert.ToString(Session["LastFinYear"]));

        //    cmd.CommandTimeout = 0;
        //    SqlDataAdapter Adap = new SqlDataAdapter();
        //    Adap.SelectCommand = cmd;
        //    Adap.Fill(dsInst);
        //    cmd.Dispose();
        //    con.Dispose();

        //    if (dsInst.Tables != null)
        //    {
        //        dt = dsInst.Tables[0];

        //    }

        //    return dt;
        //}

        //private void updateRateValuedata(string ProductID, string stockid, string branchid, string actiontype)
        //{
        //    if (ProductID != "0" && stockid != "0" && branchid != "0")
        //    {
        //        DataSet dsEmail = new DataSet();

        //        String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];
        //        SqlConnection con = new SqlConnection(conn);
        //        SqlCommand cmd3 = new SqlCommand("prc_StockOpeningwarehousUpdateRatevalue", con);
        //        cmd3.CommandType = CommandType.StoredProcedure;

        //        cmd3.Parameters.AddWithValue("@compnay", Convert.ToString(Session["LastCompany"]));
        //        cmd3.Parameters.AddWithValue("@Finyear", Convert.ToString(Session["LastFinYear"]));
        //        cmd3.Parameters.AddWithValue("@StockID", Convert.ToInt32(stockid));
        //        cmd3.Parameters.AddWithValue("@ProductID", Convert.ToInt32(ProductID));
        //        cmd3.Parameters.AddWithValue("@branchID", Convert.ToInt32(branchid));
        //        cmd3.Parameters.AddWithValue("@modifiedBy", Convert.ToInt32(HttpContext.Current.Session["userid"]));
        //        cmd3.Parameters.AddWithValue("@rate", Convert.ToDecimal(hdnrate.Value));
        //        cmd3.Parameters.AddWithValue("@values", Convert.ToDecimal(hdnvalue.Value));
        //        cmd3.Parameters.AddWithValue("@action", actiontype);
        //        SqlParameter output = new SqlParameter("@ReturnValue", SqlDbType.Int);
        //        output.Direction = ParameterDirection.Output;
        //        cmd3.Parameters.Add(output);
        //        cmd3.CommandTimeout = 0;
        //        SqlDataAdapter Adap = new SqlDataAdapter();
        //        Adap.SelectCommand = cmd3;
        //        Adap.Fill(dsEmail);
        //        dsEmail.Clear();
        //        cmd3.Dispose();
        //        con.Dispose();
        //        GC.Collect();
        //    }

        //    // return (Convert.ToInt32(output.Value));


        //}

        //public string SetVisibilityStock(object container)
        //{
        //    string vs = string.Empty;
        //    GridViewDataItemTemplateContainer c = container as GridViewDataItemTemplateContainer;
        //    if (!string.IsNullOrEmpty(Convert.ToString(c.KeyValue)))
        //    {
        //        //if (isexist)
        //        //{
        //        vs = "display:block";
        //        //}
        //        //else
        //        //{
        //        //    vs = "display:none";
        //        //}

        //    }
        //    else
        //    {
        //        vs = "display:block";
        //    }

        //    return vs;


        //}

        //public void deleteALL(string stockid)
        //{

        //    #region delete

        //    if (Session["WarehouseDataDelete"] != null && hdnisolddeleted.Value == "true")
        //    {

        //        DataTable Warehousedtups = (DataTable)Session["WarehouseDataDelete"];
        //        for (int i = 0; i <= Warehousedtups.Rows.Count - 1; i++)
        //        {

        //            if (Convert.ToString(Warehousedtups.Rows[i]["isnew"]) == "DeleteSL")
        //            {
        //                string sqls = "delete Trans_StockSerialMapping where Stock_MapId=" + Convert.ToString(Warehousedtups.Rows[i]["SerialID"]) + " " +
        //                                    " update Trans_Stock set Stock_Open=isnull(Stock_Open,0)-1," +
        //                            " Stock_ModifiedDate=GETDATE() WHERE Stock_ID=" + stockid + "	";

        //                oDBEngine.GetDataTable(sqls);
        //            }
        //            else if (Convert.ToString(Warehousedtups.Rows[i]["isnew"]) == "DeleteWHBTSL")
        //            {
        //                var updateqnty = Warehousedtups.Compute("sum(Quantitysum)", "isnew = 'DeleteWHBTSL' AND BatchWarehouseID='" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"]) + "'");

        //                string sqls = "delete from Trans_StockBranchWarehouseDetails where StockBranchWarehouseDetail_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]) + " " +
        //                              " delete from Trans_StockBranchWarehouse where StockBranchWarehouse_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"]) + " " +
        //                              " delete from Trans_StockBranchBatch where StockBranchWarehouseDetail_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]) + " " +
        //                              " delete from Trans_StockSerialMapping where Stock_BranchBatchId=" + Convert.ToString(Warehousedtups.Rows[i]["BatchID"]) + " " +
        //                                   " update Trans_Stock set Stock_Open=isnull(Stock_Open,0)-" + updateqnty + "," +
        //                           " Stock_ModifiedDate=GETDATE() WHERE Stock_ID=" + stockid + "	";

        //                if (!string.IsNullOrEmpty(Convert.ToString(updateqnty)))
        //                {
        //                    oDBEngine.GetDataTable(sqls);
        //                }

        //            }
        //            else if (Convert.ToString(Warehousedtups.Rows[i]["isnew"]) == "DeleteWHSL")
        //            {

        //                var updateqnty = Warehousedtups.Compute("sum(Quantitysum)", "isnew = 'DeleteWHSL' AND BatchWarehouseID='" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"]) + "'");

        //                string sqls = "delete from Trans_StockBranchWarehouseDetails where StockBranchWarehouseDetail_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]) + " " +
        //                              " delete from Trans_StockBranchWarehouse where StockBranchWarehouse_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"]) + " " +
        //                              " delete from Trans_StockBranchBatch where StockBranchWarehouseDetail_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]) + " " +
        //                              " delete from Trans_StockSerialMapping where Stock_BranchBatchId=" + Convert.ToString(Warehousedtups.Rows[i]["BatchID"]) + " " +
        //                                   " update Trans_Stock set Stock_Open=isnull(Stock_Open,0)-" + updateqnty + "," +
        //                           " Stock_ModifiedDate=GETDATE() WHERE Stock_ID=" + stockid + "	";
        //                if (!string.IsNullOrEmpty(Convert.ToString(updateqnty)))
        //                {
        //                    oDBEngine.GetDataTable(sqls);
        //                }


        //            }
        //            else if (Convert.ToString(Warehousedtups.Rows[i]["isnew"]) == "DeleteWHBT")
        //            {
        //                string sqls = string.Empty;
        //                var updateqnty = Warehousedtups.Compute("sum(Quantitysum)", "isnew = 'DeleteWHBT' AND BatchWarehouseID='" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"]) + "'");

        //                if (string.IsNullOrEmpty(Convert.ToString(updateqnty)))
        //                { updateqnty = 0.0; }

        //                //if (string.IsNullOrEmpty(Convert.ToString(Warehousedtups.Rows[i]["viewWarehouseName"])))
        //                //{

        //                //    sqls = "update Trans_StockBranchWarehouse set StockBranchWarehouse_StockIn=ISNULL(StockBranchWarehouse_StockIn,0)-" + updateqnty + " where StockBranchWarehouse_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"]) + " " +
        //                //              " update Trans_StockBranchWarehouseDetails set Stock_OpeningRate=" + Convert.ToDecimal(hdnrate.Value) + "," +
        //                //                                     "ModifiedBy=" + Convert.ToInt32(HttpContext.Current.Session["userid"]) + "," +
        //                //                                     "ModifiedDate=GETDATE() " +
        //                //                                     "where StockBranchWarehouseDetail_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]) + "" +
        //                //              " delete from Trans_StockBranchBatch where StockBranchBatch_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchID"]) + " " +

        //                //                   " update Trans_Stock set Stock_Open=isnull(Stock_Open,0)-" + updateqnty + "," +
        //                //           " Stock_ModifiedDate=GETDATE() WHERE Stock_ID=" + stockid + "	";

        //                //}
        //                //else
        //                //{

        //                //}


        //                if (stockid != "0")
        //                {
        //                    DataSet dsEmail = new DataSet();

        //                    String conn = ConfigurationSettings.AppSettings["DBConnectionDefault"];
        //                    SqlConnection con = new SqlConnection(conn);
        //                    SqlCommand cmd3 = new SqlCommand("prc_StockOpeningwarehousbatchDelete", con);
        //                    cmd3.CommandType = CommandType.StoredProcedure;

        //                    cmd3.Parameters.AddWithValue("@compnay", Convert.ToString(Session["LastCompany"]));
        //                    cmd3.Parameters.AddWithValue("@Finyear", Convert.ToString(Session["LastFinYear"]));
        //                    cmd3.Parameters.AddWithValue("@StockID", Convert.ToInt32(stockid));

        //                    cmd3.Parameters.AddWithValue("@BatchWarehouseID", Convert.ToInt32(Warehousedtups.Rows[i]["BatchWarehouseID"]));
        //                    cmd3.Parameters.AddWithValue("@BatchWarehousedetailsID", Convert.ToInt32(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]));
        //                    cmd3.Parameters.AddWithValue("@BatchID", Convert.ToInt32(Warehousedtups.Rows[i]["BatchID"]));


        //                    //cmd3.Parameters.AddWithValue("@branchID", Convert.ToInt32(hdnselectedbranch.Value));
        //                    cmd3.Parameters.AddWithValue("@modifiedBy", Convert.ToInt32(HttpContext.Current.Session["userid"]));
        //                    cmd3.Parameters.AddWithValue("@rate", Convert.ToDecimal(hdnrate.Value));
        //                    cmd3.Parameters.AddWithValue("@updateqnty", Convert.ToDecimal(updateqnty));

        //                    SqlParameter output = new SqlParameter("@ReturnValue", SqlDbType.Int);
        //                    output.Direction = ParameterDirection.Output;
        //                    cmd3.Parameters.Add(output);
        //                    cmd3.CommandTimeout = 0;
        //                    SqlDataAdapter Adap = new SqlDataAdapter();
        //                    Adap.SelectCommand = cmd3;
        //                    Adap.Fill(dsEmail);
        //                    dsEmail.Clear();
        //                    cmd3.Dispose();
        //                    con.Dispose();
        //                    GC.Collect();
        //                }


        //            }
        //            else if (Convert.ToString(Warehousedtups.Rows[i]["isnew"]) == "DeleteWH")
        //            {

        //                var updateqnty = Warehousedtups.Compute("sum(Quantity)", "isnew = 'DeleteWH' AND BatchWarehouseID='" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"]) + "'");

        //                string sqls = "delete from Trans_StockBranchWarehouseDetails where StockBranchWarehouseDetail_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]) + " " +
        //                              " delete from Trans_StockBranchWarehouse where StockBranchWarehouse_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehouseID"]) + " " +
        //                              " delete from Trans_StockBranchBatch where StockBranchWarehouseDetail_Id=" + Convert.ToString(Warehousedtups.Rows[i]["BatchWarehousedetailsID"]) + " " +

        //                                   " update Trans_Stock set Stock_Open=isnull(Stock_Open,0)-" + updateqnty + "," +
        //                           " Stock_ModifiedDate=GETDATE() WHERE Stock_ID=" + stockid + "	";

        //                if (!string.IsNullOrEmpty(Convert.ToString(updateqnty)))
        //                {
        //                    oDBEngine.GetDataTable(sqls);
        //                }

        //            }

        //        }
        //        //var updateqnty = Warehousedtups.Compute("sum(Quantitysum)", "isnew = 'Updated' AND BatchWarehouseID='" + Convert.ToString(dt.Rows[i]["BatchWarehouseID"]) + "'");

        //    }
        //    #endregion

        //}
        #endregion
    }
}


//public class ASPxGridViewCellMerger
//{
//    ASPxGridView grid;
//    Dictionary<GridViewDataColumn, TableCell> mergedCells = new Dictionary<GridViewDataColumn, TableCell>();
//    Dictionary<TableCell, int> cellRowSpans = new Dictionary<TableCell, int>();

//    public ASPxGridViewCellMerger(ASPxGridView grid)
//    {

//        this.grid = grid;
//        grid.HtmlRowCreated += new ASPxGridViewTableRowEventHandler(grid_HtmlRowCreated);
//        grid.HtmlDataCellPrepared += new ASPxGridViewTableDataCellEventHandler(grid_HtmlDataCellPrepared);
//    }

//    public ASPxGridView Grid { get { return grid; } }
//    void grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
//    {
//        //add the attribute that will be used to find which column the cell belongs to
//        e.Cell.Attributes.Add("Serial", e.DataColumn.VisibleIndex.ToString());

//        if (cellRowSpans.ContainsKey(e.Cell))
//        {
//            e.Cell.RowSpan = cellRowSpans[e.Cell];
//        }
//    }
//    protected void grid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
//    {
//       // if (Grid.GetRowLevel(e.VisibleIndex) != Grid.GroupCount) return;
//        for (int i = e.Row.Cells.Count - 1; i >= 0; i--)
//        {
//            // DevExpress.Web.ASPxGridView.Rendering.GridViewTableDataCell cell = (DevExpress.Web.ASPxGridView.Rendering.GridViewTableDataCell)e.Row.Cells[i];

//            DevExpress.Web.Rendering.GridViewTableDataCell dataCell = e.Row.Cells[i] as DevExpress.Web.Rendering.GridViewTableDataCell;
//            if (dataCell != null)
//            {
//                MergeCells(dataCell.DataColumn, e.VisibleIndex, dataCell);
//            }
//        }
//    }

//    void MergeCells(GridViewDataColumn column, int visibleIndex, TableCell cell)
//    {
//        bool isNextTheSame = IsNextRowHasSameData(column, visibleIndex);
//        if (isNextTheSame)
//        {
//            if (!mergedCells.ContainsKey(column))
//            {
//                mergedCells[column] = cell;
//            }
//        }
//        if (IsPrevRowHasSameData(column, visibleIndex))
//        {
//            ((TableRow)cell.Parent).Cells.Remove(cell);
//            if (mergedCells.ContainsKey(column))
//            {
//                TableCell mergedCell = mergedCells[column];
//                if (!cellRowSpans.ContainsKey(mergedCell))
//                {
//                    cellRowSpans[mergedCell] = 1;
//                }
//                cellRowSpans[mergedCell] = cellRowSpans[mergedCell] + 1;
//            }
//        }
//        if (!isNextTheSame)
//        {
//            mergedCells.Remove(column);
//        }
//    }
//    bool IsNextRowHasSameData(GridViewDataColumn column, int visibleIndex)
//    {
//        //is it the last visible row
//        if (visibleIndex >= Grid.VisibleRowCount - 1)
//            return false;

//        return IsSameData(column.FieldName, visibleIndex, visibleIndex + 1);
//    }
//    bool IsPrevRowHasSameData(GridViewDataColumn column, int visibleIndex)
//    {
//        ASPxGridView grid = column.Grid;
//        //is it the first visible row
//        if (visibleIndex <= Grid.VisibleStartIndex)
//            return false;

//        return IsSameData(column.FieldName, visibleIndex, visibleIndex - 1);
//    }
//    bool IsSameData(string fieldName, int visibleIndex1, int visibleIndex2)
//    {
//        // is it a group row?
//        if (Grid.GetRowLevel(visibleIndex2) != Grid.GroupCount)
//            return false;

//        return object.Equals(Grid.GetRowValues(visibleIndex1, fieldName), Grid.GetRowValues(visibleIndex2, fieldName));
//    }
//}