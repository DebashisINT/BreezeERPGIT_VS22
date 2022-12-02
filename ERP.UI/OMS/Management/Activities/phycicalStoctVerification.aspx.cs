using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using DevExpress.Web.Data;
using EntityLayer.CommonELS;
using ERP.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Activities
{
    public partial class phycicalStoctVerification : System.Web.UI.Page
    {
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        SlaesActivitiesBL objSlaesActivitiesBL = new SlaesActivitiesBL();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        protected void Page_Load(object sender, EventArgs e)
        {

            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/Activities/phycicalStoctVerification.aspx");

            if (!IsPostBack)
            {
                Session["BindProduct_Details"] = null;
                Session["BindProduct_DetailsAftertSave"] = null;
                Session["BindProduct_CalcuCommitDetails"] = null;
                Session["BindProduct_Check"] = null;
               
                
                DataTable dt = GetWarehouseData();
                CmbWarehouse.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    CmbWarehouse.Items.Add(Convert.ToString(dt.Rows[i]["WarehouseName"]), Convert.ToString(dt.Rows[i]["WarehouseID"]));
                }
                GetFinacialYearBasedQouteDate();

            }
            //if(GrdQuotation.VisibleRowCount>0)
            //{
            //    ASPxButton1.ClientVisible = false;
            //}
            //else
            //{
            //    ASPxButton1.ClientVisible = true;
            //}


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
                        dt_PLQuote.MinDate = Convert.ToDateTime(Convert.ToString(Session["FinYearStartDate"]));
                    }
                    if (Session["FinYearEndDate"] != null)
                    {
                        dt_PLQuote.MaxDate = Convert.ToDateTime(Convert.ToString(Session["FinYearEndDate"]));
                    }
                    if (oDBEngine.GetDate().Date >= Convert.ToDateTime(Convert.ToString(Session["FinYearStartDate"])) && oDBEngine.GetDate().Date <= Convert.ToDateTime(Convert.ToString(Session["FinYearEndDate"])))
                    {

                    }
                    else if (oDBEngine.GetDate().Date >= Convert.ToDateTime(Convert.ToString(Session["FinYearStartDate"])) && oDBEngine.GetDate().Date >= Convert.ToDateTime(Convert.ToString(Session["FinYearEndDate"])))
                    {
                        setdate = Convert.ToString(Convert.ToDateTime(Session["FinYearEndDate"]).Month) + "/" + Convert.ToString(Convert.ToDateTime(Session["FinYearEndDate"]).Day) + "/" + Convert.ToString(Convert.ToDateTime(Session["FinYearEndDate"]).Year);
                        dt_PLQuote.Value = DateTime.ParseExact(setdate, @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        //dt_PLQuote.Value = DateTime.ParseExact(Convert.ToString(Session["FinYearStartDate"]), @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else if (oDBEngine.GetDate().Date <= Convert.ToDateTime(Convert.ToString(Session["FinYearStartDate"])) && oDBEngine.GetDate().Date <= Convert.ToDateTime(Convert.ToString(Session["FinYearEndDate"])))
                    {
                        setdate = Convert.ToString(Convert.ToDateTime(Session["FinYearStartDate"]).Month) + "/" + Convert.ToString(Convert.ToDateTime(Session["FinYearStartDate"]).Day) + "/" + Convert.ToString(Convert.ToDateTime(Session["FinYearStartDate"]).Year);
                        dt_PLQuote.Value = DateTime.ParseExact(setdate, @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                        //dt_PLQuote.Value = DateTime.ParseExact(Convert.ToString(Session["FinYearEndDate"]), @"M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    }
                }
            }
            //dt_PLQuote.Value = Convert.ToDateTime(oDBEngine.GetDate().ToString());
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

       
        protected void EntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {

            e.KeyExpression = "sProducts_ID";
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
            ERPDataClassesDataContext dc = new ERPDataClassesDataContext(connectionString);

            var q = from d in dc.V_CalculateStockLists
                        //where
                        //d.Invoice_Date >= Convert.ToDateTime(strFromDate) && d.Invoice_Date <= Convert.ToDateTime(strToDate)
                        //&& branchidlist.Contains(Convert.ToInt32(d.BranchID)) && d.InvoiceFor == "CL" && d.CompanyID == CompanyID
                        //&& d.FinYear == FinYear
                        //&& d.CreatedBy == userid
                        //orderby d.Invoice_Date descending
                        select d;
                e.QueryableSource = q;

           


        }

        protected void grid_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            grid.JSProperties["cpAfterWareFocus"] = "";
            grid.JSProperties["cpwidthLoad"] = "";
            string strSplitCommand = e.Parameters.Split('~')[0];

            if (strSplitCommand == "Display")
            {
                string IsDeleteFrom = Convert.ToString(hdfIsDelete.Value);
                if (IsDeleteFrom == "D")
                {
                    DataTable Quotationdt = (DataTable)Session["BindProduct_Details"];
                    grid.DataSource = GetQuotation(Quotationdt);
                    grid.DataBind();
                }


            }

            else if (strSplitCommand == "QuantityZero")
            {
                grid.JSProperties["cpAfterWareFocus"] = "AfterWareFocus";
                DataTable dt = (DataTable)Session["BindProduct_Details"];

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {

                        dr["StockUnitQnty"] = "";
                        dr["AltUnitQnty"] = "";
                    }

                    dt.AcceptChanges();
                }
                Session["BindProduct_Details"] = dt;
                grid.DataSource = GetQuotation(dt);
               grid.DataBind();
            }

            else if (strSplitCommand == "BindGrid")
            {
                grid.JSProperties["cpwidthLoad"] = "widthLoad";
                DataTable dt = new DataTable();
                string ProductId = Convert.ToString(e.Parameters.Split('~')[1]);

                if (ProductId != "AllProducts")
                {
                    string ClassId = Convert.ToString(e.Parameters.Split('~')[2]);
                    string BrandId = Convert.ToString(e.Parameters.Split('~')[3]);

                    dt = GetGridProductDetailsBind(ProductId, ClassId, BrandId);
                }
                else
                {
                    dt = GetAllProductDetailsBind();
                }
                Session["BindProduct_Details"] = dt;
                Session["BindProduct_DetailsAftertSave"] = dt;

                Session["BindProduct_Check"] = dt;
                grid.DataSource = GetQuotation(dt);
                grid.DataBind();
                if(grid.VisibleRowCount>0)
                {
                    btn_SaveRecords.ClientVisible = true;
                }
                else
                {
                    btn_SaveRecords.ClientVisible = false;
                }

            }
        }

        protected void grid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            grid.JSProperties["cpSuccess"] = "";
            string IsDeleteFrom = Convert.ToString(hdfIsDelete.Value);

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
                Quotationdt.Columns.Add("StockId", typeof(Int64));
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

                    string Class = Convert.ToString(args.NewValues["Class"]);
                    string StockUnit = Convert.ToString(args.NewValues["StockUnit"]);
                    string AltUnit = Convert.ToString(args.NewValues["AltUnit"]);

                    string StockUnitQnty = Convert.ToString(args.NewValues["StockUnitQnty"]);
                    string AltUnitQnty = Convert.ToString(args.NewValues["AltUnitQnty"]);

                    Int64 ClassId = Convert.ToInt64(args.NewValues["ClassId"]);
                    Int64 BrandId = Convert.ToInt64(args.NewValues["BrandId"]);

                    Quotationdt.Rows.Add(SrlNo, ProductdisID, Product, Description, Class, StockUnit, AltUnit, StockUnitQnty, AltUnitQnty, ProductDetailsForId, ClassId, BrandId, StockUnitId, AltUnitId, StockId);


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

                        string Class = Convert.ToString(args.NewValues["Class"]);
                        string StockUnit = Convert.ToString(args.NewValues["StockUnit"]);
                        string AltUnit = Convert.ToString(args.NewValues["AltUnit"]);

                        string StockUnitQnty = Convert.ToString(args.NewValues["StockUnitQnty"]);
                        string AltUnitQnty = Convert.ToString(args.NewValues["AltUnitQnty"]);

                        Int64 ClassId = Convert.ToInt64(args.NewValues["ClassId"]);
                        Int64 BrandId = Convert.ToInt64(args.NewValues["BrandId"]);
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

                                drr["StockId"] = StockId;


                                break;
                            }
                        }

                        if (Isexists == false)
                        {
                            Quotationdt.Rows.Add(SrlNo, ProductdisID, Product, Description, Class, StockUnit, AltUnit, StockUnitQnty, AltUnitQnty, ProductDetailsForId, ClassId, BrandId, StockUnitId, AltUnitId, StockId);
                        }


                    }
                }
            }


            //foreach (var args in e.DeleteValues)
            //{
            //    string sProducts_ID = Convert.ToString(args.Keys["sProducts_ID"]);
            //    string SrlNo = "";

            //    for (int i = Quotationdt.Rows.Count - 1; i >= 0; i--)
            //    {
            //        DataRow dr = Quotationdt.Rows[i];
            //        string delQuotationID = Convert.ToString(dr["sProducts_ID"]);

            //        if (delQuotationID == sProducts_ID)
            //        {
            //            SrlNo = Convert.ToString(dr["SrlNo"]);
            //            dr.Delete();
            //        }
            //    }
            //    Quotationdt.AcceptChanges();



            //    if (sProducts_ID.Contains("~") != true)
            //    {
            //        //Quotationdt.Rows.Add("0", QuotationID, "0", "", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "D", "", "0", "", "0", "0", "", "","");
            //        Quotationdt.Rows.Add("0", sProducts_ID, "0", "", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0");
            //    }
            //}

            ///////////////////////



            //int j = 1;
            //foreach (DataRow dr in Quotationdt.Rows)
            //{
            //   // string Status = Convert.ToString(dr["Status"]);
            //    string oldSrlNo = Convert.ToString(dr["SrlNo"]);
            //    string newSrlNo = j.ToString();

            //    dr["SrlNo"] = j.ToString();

            //    string strDeleteSrlNo = Convert.ToString(hdnDeleteSrlNo.Value);
            //    if (strDeleteSrlNo != "" && strDeleteSrlNo != "0")
            //    {


            //        hdnDeleteSrlNo.Value = "";
            //    }

            //    if (hdnDeleteSrlNo.Value != "D")
            //    {
            //        if (hdnDeleteSrlNo.Value == "")
            //        {
            //            string strID = Convert.ToString("Q~" + j);
            //            dr["QuotationID"] = strID;
            //        }
            //        j++;
            //    }
            //}



            Quotationdt.AcceptChanges();

            Session["BindProduct_Details"] = Quotationdt;

            int strIsComplete = 0;
            if (IsDeleteFrom == "I")
            {
                Int64 warehouseid = Convert.ToInt64(CmbWarehouse.Value);

                DataTable dt = new DataTable();
                if (HttpContext.Current.Session["BindProduct_Details"] != null)
                {
                    dt = (DataTable)Session["BindProduct_Details"];
                    DataRow[] result = dt.Select("StockUnitQnty='' and AltUnitQnty=''");

                    foreach (DataRow dr in result)
                    {
                        dt.Rows.Remove(dr);
                    }
                    Session["BindProduct_Details"] = dt;
                }             

                //if (HttpContext.Current.Session["BindProduct_Details"] != null)
                //{
                //    dt = (DataTable)Session["BindProduct_Details"];
                //    DataRow[] result = dt.Select("StockUnitQnty='0.0000' and AltUnitQnty='0.0000'");

                //    foreach (DataRow dr in result)
                //    {
                //        dt.Rows.Remove(dr);
                //    }
                //    Session["BindProduct_Details"] = dt;
                //}
                DataTable dtrowcount=(DataTable)Session["BindProduct_Details"];
                if (dtrowcount.Rows.Count > 0)
                {
                    ModifystockSheet(Quotationdt, warehouseid, ref strIsComplete);

                    if (strIsComplete == 1)
                    {

                        Session["BindProduct_CalcuCommitDetails"] = Session["BindProduct_Details"];

                        Session["BindProduct_Details"] = null;
                        //Session["BindProduct_DetailsAftertSave"];
                        grid.JSProperties["cpSuccess"] = "Success";
                    }
                }
                else
                {
                    grid.JSProperties["cpSuccess"] = "RecordNotFound";
                }

            }

        }



        public void ModifystockSheet(DataTable Quotationdt, Int64 warehouseid, ref int strIsComplete)
        {
            try
            {
                DataSet dsInst = new DataSet();

                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                if (Quotationdt.Columns.Contains("ProductID"))
                {
                    Quotationdt.Columns.Remove("ProductID");
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

                SqlCommand cmd = new SqlCommand("Prc_SaveStockSheet", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "SaveStock");
                cmd.Parameters.AddWithValue("@StockSheetDetails", Quotationdt);
                cmd.Parameters.AddWithValue("@WareHouseId", warehouseid);
                cmd.Parameters.AddWithValue("@CreateUser", Convert.ToString(Session["userid"]));
                cmd.Parameters.AddWithValue("@AsOnDate", Convert.ToDateTime(dt_PLQuote.Date));




                cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);


                cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;


                cmd.CommandTimeout = 0;
                SqlDataAdapter Adap = new SqlDataAdapter();
                Adap.SelectCommand = cmd;
                Adap.Fill(dsInst);

                strIsComplete = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());



                cmd.Dispose();
                con.Dispose();
            }
            catch (Exception ex)
            {

            }
        }

        //protected void ASPxGridView2_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        //{
        //    if (e.Column.FieldName == "StockUnitQnty" && e.Value != "" && e.Value != null)
        //    {
        //        if (e.Column.FieldName == "StockUnitQnty" && (int)e.Value == 0)
        //        {
        //            e.DisplayText = string.Empty;
        //        }
        //    }
        //}
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
            else
            {
                e.Editor.ReadOnly = false;
            }
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
        //protected void CmbWarehouse_Callback(object sender, CallbackEventArgsBase e)
        //{
        //    string WhichCall = e.Parameter.Split('~')[0];
        //    if (WhichCall == "BindWarehouse")
        //    {


        //        DataTable dt = GetWarehouseData();
        //        CmbWarehouse.Items.Clear();
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {

        //            CmbWarehouse.Items.Add(Convert.ToString(dt.Rows[i]["WarehouseName"]), Convert.ToString(dt.Rows[i]["WarehouseID"]));
        //        }


        //    }
        //}


        protected void CalculateCommitExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(calCuCommit.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportvalcalcommit"] == null)
                {
                    Session["exportvalcalcommit"] = Filter;
                    bindCalculationexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportvalcalcommit"]) != Filter)
                {
                    Session["exportvalcalcommit"] = Filter;
                    bindCalculationexport(Filter);
                }
            }
        }

        public void bindCalculationexport(int Filter)
        {
            //GrdQuotation.Columns[6].Visible = false;
            GrdQuotation.Columns[0].Visible = false;
           

            string filename = "Physical Stock Calculation";
            Calculateexport.FileName = filename;
            Calculateexport.FileName = "PhySicalStockCalculation";

            Calculateexport.PageHeader.Left = "Physical Stock Calculation";
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

        public void bindexport(int Filter)
        {
            //GrdQuotation.Columns[6].Visible = false;
            grid.Columns[9].Visible = false;
            grid.Columns[10].Visible = false;
            grid.Columns[11].Visible = false;
            grid.Columns[12].Visible = false;
            grid.Columns[13].Visible = false;
            grid.Columns[14].Visible = false;

            string filename = "Physical Stock";
            exporter.FileName = filename;
            exporter.FileName = "PhySicalStock";

            exporter.PageHeader.Left = "Physical Stock";
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

        protected void StockCommit(object sender, EventArgs e)
        {
            Int32 IsComplete = 0;
            int Numberofrow=0;
            Int64 warehouseid = Convert.ToInt64(CmbWarehouse.Value);
            DataTable dt = (DataTable)Session["BindProduct_CalcuCommitDetails"];
            DataTable StockRowcount = oDBEngine.GetDataTable("select count(StockSheet_id) StockCount from Master_StockSheet");
            if(StockRowcount.Rows.Count>0)
             Numberofrow = Convert.ToInt32(StockRowcount.Rows[0]["StockCount"]);
            try
            {
                DataSet dsInst = new DataSet();

                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                SqlCommand cmd = new SqlCommand("Prc_SaveStockSheet", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "SaveCommitStockData");
                //cmd.Parameters.AddWithValue("@SelectedProductList", hdnCalcommitProductId.Value);
                //cmd.Parameters.AddWithValue("@WareHouseId", warehouseid);
                cmd.Parameters.AddWithValue("@CreateUser", Convert.ToString(Session["userid"]));




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
                GrdQuotation.JSProperties["cpSavecheck"] = "";
                //Response.Redirect("stockReconcileList.aspx");
                Popup_CalculateCommit.ShowOnPageLoad = false; 
                Session["BindProduct_CalcuCommitDetails"] = null;
                if (Numberofrow > 0)
                {
                    GrdQuotation.JSProperties["cpSavecheck"] = "Savecheck";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "jAlert('Data Commit Successfully.')", true);
                }
                if (grid.VisibleRowCount > 0)
                {
                    btn_SaveRecords.ClientVisible = true;
                }
                else
                {
                    btn_SaveRecords.ClientVisible = false;
                }

                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "jAlert('Please try again later.')", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "jAlert('Please try again later.')", true);
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
                    Quotations.sProducts_ID = Convert.ToInt64(Quotationdt.Rows[i]["sProducts_ID"]);
                    Quotations.Product = Convert.ToString(Quotationdt.Rows[i]["Product"]);
                    Quotations.Description = Convert.ToString(Quotationdt.Rows[i]["Description"]);
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
                    Quotations.StockId = Convert.ToInt64(Quotationdt.Rows[i]["StockId"]);

                    QuotationList.Add(Quotations);
                }
            }

            return QuotationList;
        }

        public class Quotation
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


            public Int64 ClassId { get; set; }

            public Int64 BrandId { get; set; }
            public Int64 StockUnitId { get; set; }
            public Int64 AltUnitId { get; set; }
            public Int64 StockId { get; set; }


        }

        [WebMethod]

        public static object GetClass(string SearchKey)
        {
            List<ClassModel> listcwiseProducts = new List<ClassModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable cwiseProduct = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                //SqlCommand cmd = new SqlCommand("Proc_GetSubLedger", con);
                SqlCommand cmd = new SqlCommand("Prc_GetProductForStock", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.Parameters.AddWithValue("@Action", "GetClassDetails");
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(cwiseProduct);


                cmd.Dispose();
                con.Dispose();

                listcwiseProducts = (from DataRow dr in cwiseProduct.Rows
                                     select new ClassModel()
                                     {
                                         Class_ID = Convert.ToInt64(dr["Class_ID"]),
                                         ClassCode = Convert.ToString(dr["ClassCode"])

                                     }).ToList();
            }

            return listcwiseProducts;
        }

        [WebMethod]

        public static object GetProduct(string SearchKey)
        {
            List<ProductModel> listcwiseProducts = new List<ProductModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable cwiseProduct = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                //SqlCommand cmd = new SqlCommand("Proc_GetSubLedger", con);
                SqlCommand cmd = new SqlCommand("Prc_GetProductForStock", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.Parameters.AddWithValue("@Action", "GetProductDetails");
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(cwiseProduct);


                cmd.Dispose();
                con.Dispose();

                listcwiseProducts = (from DataRow dr in cwiseProduct.Rows
                                     select new ProductModel()
                                     {
                                         sProducts_ID = Convert.ToString(dr["sProducts_ID"]),
                                         sProducts_Code = Convert.ToString(dr["sProducts_Code"]),
                                         sProducts_Name = Convert.ToString(dr["sProducts_Name"]),
                                         ClassCode = Convert.ToString(dr["ClassCode"]),
                                         BrandName = Convert.ToString(dr["BrandName"])

                                     }).ToList();
            }

            return listcwiseProducts;
        }


        [WebMethod]

        public static object GetBrand(string SearchKey)
        {
            List<BrandModel> listcwiseProducts = new List<BrandModel>();
            if (HttpContext.Current.Session["userid"] != null)
            {
                SearchKey = SearchKey.Replace("'", "''");
                BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

                DataTable cwiseProduct = new DataTable();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                //SqlCommand cmd = new SqlCommand("Proc_GetSubLedger", con);
                SqlCommand cmd = new SqlCommand("Prc_GetProductForStock", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@filtertext", SearchKey);
                cmd.Parameters.AddWithValue("@Action", "GetBrandDetails");
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(cwiseProduct);


                cmd.Dispose();
                con.Dispose();

                listcwiseProducts = (from DataRow dr in cwiseProduct.Rows
                                     select new BrandModel()
                                     {
                                         Brand_ID = Convert.ToInt64(dr["Brand_ID"]),
                                         BrandName = Convert.ToString(dr["BrandName"])

                                     }).ToList();
            }

            return listcwiseProducts;
        }




        [WebMethod]
        public static object GetProductQuantity(Int64 ProductDisId, Int64 WarhouseId, DateTime OnDate)
        {
            List<ProductRowQuantity> RateLists = new List<ProductRowQuantity>();

            decimal StockSheet_StockQty = Convert.ToDecimal(0.0000);
            decimal StockSheet_AltQty = Convert.ToDecimal(0.0000);
            string AsOnDate = "";
            string CountRow="";
            ProcedureExecute proc = new ProcedureExecute("Prc_GetProductForStock");
            proc.AddVarcharPara("@Action", 500, "GetProductQuantityRowWise");
            proc.AddBigIntegerPara("@ProductDisId", ProductDisId);
            proc.AddBigIntegerPara("@WarehouseId", WarhouseId);
            proc.AddIntegerPara("@UserId", Convert.ToInt32(HttpContext.Current.Session["userid"]));
            proc.AddDateTimePara("@AsOnDate", Convert.ToDateTime(OnDate));

            DataTable dt = proc.GetTable();
            RateLists = DbHelpers.ToModelList<ProductRowQuantity>(dt);
            if (dt != null && dt.Rows.Count > 0)
            {
                StockSheet_StockQty = Convert.ToDecimal(dt.Rows[0]["StockSheet_StockQty"]);
                StockSheet_AltQty = Convert.ToDecimal(dt.Rows[0]["StockSheet_AltQty"]);
                AsOnDate = Convert.ToString(dt.Rows[0]["AsOnDate"]);
                CountRow = Convert.ToString(dt.Rows[0]["CountRow"]);
            }
            //return packing_quantity + '~' + sProduct_quantity;
            return RateLists;
        }




        [WebMethod]
        public static object GetPackingQuantity(Int32 UomId, Int32 AltUomId, Int64 ProductID)
        {
            List<UomConersiopn> RateLists = new List<UomConersiopn>();

            decimal packing_quantity = 0;
            decimal sProduct_quantity = 0;
            bool isOverideConvertion = false;
            ProcedureExecute proc = new ProcedureExecute("Prc_GetProductForStock");
            proc.AddVarcharPara("@Action", 500, "PackingQuantityDetails");
            proc.AddIntegerPara("@UomId", UomId);
            proc.AddIntegerPara("@AltUomId", AltUomId);
            proc.AddBigIntegerPara("@PackingProductId", ProductID);
            DataTable dt = proc.GetTable();
            RateLists = DbHelpers.ToModelList<UomConersiopn>(dt);
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
        public static int GetCommitRowcount()
        {
           int i=0;

            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable dy = new DataTable();
            dy = oDBEngine.GetDataTable("select count(StockSheet_id) StockCount from Master_StockSheet");
            if(dy.Rows.Count>0)
            {
                i = Convert.ToInt32(dy.Rows[0]["StockCount"]);
            }
            return i;
        }



        [WebMethod]
        public static int DifferentDateProductEntryCheck(Int64 WarhouseId, DateTime OnDate)
        {
            
              
                Int32 returnValue = 0;
                //ProcedureExecute proc = new ProcedureExecute("Prc_PhysicalStockProductCheck");
                //proc.AddDateTimePara("@AsOnDate", OnDate);
                //proc.AddVarcharPara("@Action", 500, "DateWiseProductEntryCheck");
                //proc.AddVarcharPara("@ProductCheck", 4000, pR);
                //proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
                //proc.AddBigIntegerPara("@WarehouseId", WarhouseId);
              
                
                //proc.RunActionQuery();
                //returnValue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));


                try
                {
                    DataSet dsInst = new DataSet();

                    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                    DataTable pR = new DataTable();
                    DataTable pRDT = new DataTable();
                    DataTable DEtailsDT = new DataTable();
                    DEtailsDT = (DataTable)HttpContext.Current.Session["BindProduct_Check"];
                    HttpContext.Current.Session["BindProduct_Details"] = DEtailsDT;
                    if (HttpContext.Current.Session["BindProduct_Check"] != null) 
                    {                        
                        pR = (DataTable)HttpContext.Current.Session["BindProduct_Check"];
                        //HttpContext.Current.Session["BindProduct_Check1"] = pR;
                        //pRDT = (DataTable)HttpContext.Current.Session["BindProduct_Check1"];
                        DataRow[] result = pR.Select("StockUnitQnty='' and AltUnitQnty=''");

                        foreach (DataRow dr in result)
                        {
                            pR.Rows.Remove(dr);
                        }
                        pR.AcceptChanges();

                        HttpContext.Current.Session["BindProduct_Check"] = pR;
                    }

                    pR = (DataTable)HttpContext.Current.Session["BindProduct_Check"];

                    //if ((pRDT.Columns.Contains("ProductID")) && (pRDT.Columns.Contains("Class")) && (pRDT.Columns.Contains("StockUnit")) && (pRDT.Columns.Contains("AltUnit")) && (pRDT.Columns.Contains("SrlNo")) && (pRDT.Columns.Contains("Product")) && (pRDT.Columns.Contains("Description")) && (pRDT.Columns.Contains("StockUnitQnty")) && (pRDT.Columns.Contains("AltUnitQnty")) && (pRDT.Columns.Contains("ClassId")) && (pRDT.Columns.Contains("BrandId")) && (pRDT.Columns.Contains("StockUnitId")) && (pRDT.Columns.Contains("AltUnitId")) && (pRDT.Columns.Contains("StockId")))
                    //{
                    //    pRDT.Columns.Remove("Class");
                    //    pRDT.Columns.Remove("StockUnit");
                    //    pRDT.Columns.Remove("AltUnit");
                    //    pRDT.Columns.Remove("ProductID");
                    //    pRDT.AcceptChanges();
                    //}                   

                    SqlCommand cmd = new SqlCommand("Prc_PhysicalStockProductCheck", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "DateWiseProductEntryCheck");
                    cmd.Parameters.AddWithValue("@ProductCheck", pR);
                    cmd.Parameters.AddWithValue("@WarehouseId", WarhouseId);
                    cmd.Parameters.AddWithValue("@AsOnDate", Convert.ToDateTime(OnDate));
                    cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
                    cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;

                    cmd.CommandTimeout = 0;
                    SqlDataAdapter Adap = new SqlDataAdapter();
                    Adap.SelectCommand = cmd;
                    Adap.Fill(dsInst);

                    returnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value.ToString());

                    cmd.Dispose();
                    con.Dispose();
                }
                catch (Exception ex)
                {

                }


                return returnValue;
        }


        [WebMethod]
        public static int GetProductCheck()
        {
                DataTable dt = new DataTable();
                if (HttpContext.Current.Session["BindProduct_Details"] != null)
                {
                    dt = (DataTable)(DataTable)HttpContext.Current.Session["BindProduct_Details"];
                    DataRow[] result = dt.Select("StockUnitQnty='' and AltUnitQnty=''");

                    foreach (DataRow dr in result)
                    {
                        dt.Rows.Remove(dr);
                    }
                    dt.AcceptChanges();
                    HttpContext.Current.Session["BindProduct_Details"] = dt;
                }
                if (HttpContext.Current.Session["BindProduct_Details"] != null)
                {
                    dt = (DataTable)HttpContext.Current.Session["BindProduct_Details"];
                    DataRow[] result = dt.Select("StockUnitQnty='0.0000' and AltUnitQnty='0.0000'");

                    foreach (DataRow dr in result)
                    {
                        dt.Rows.Remove(dr);
                    }
                    dt.AcceptChanges();
                    HttpContext.Current.Session["BindProduct_Details"] = dt;
                }
            if((dt.Rows.Count>1 || dt.Rows.Count==1)&& dt!=null )
            {
                return 1;
            }
            else
            {
                return 0;
            }


            
        }


        [WebMethod]
        public static int DeleteStockSheetData(Int64 ProductID)
        {
            Int32 a = 0;
            ProcedureExecute proc = new ProcedureExecute("Prc_SaveStockSheet");
            proc.AddVarcharPara("@Action", 500, "DeleteStockData");
            proc.AddBigIntegerPara("@ProductsId", ProductID);
            a = proc.RunActionQuery();
            return a;
        }

        public DataTable GetWarehouseData()
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_GetProductForStock");
            proc.AddVarcharPara("@Action", 500, "GetWareHouse");

            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetAllProductDetailsBind()
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_GetProductForStock");
            proc.AddVarcharPara("@Action", 500, "AllProductDetailsInGrid");

            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetGridProductDetailsBind(string ProductId, string ClassId, string BrandId)
        {

            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_GetProductForStock");
            proc.AddVarcharPara("@Action", 500, "GetProductDetailsInGrid");
            proc.AddVarcharPara("@ProductId", 500, ProductId);
            proc.AddVarcharPara("@ClassId", 500, ClassId);
            proc.AddVarcharPara("@BrandId", 500, BrandId);
            dt = proc.GetTable();
            return dt;
        }

    }
    public class ProductModel
    {
        public string sProducts_ID { get; set; }
        public string sProducts_Code { get; set; }
        public string sProducts_Name { get; set; }
        public string ClassCode { get; set; }
        public string BrandName { get; set; }

    }
    public class ClassModel
    {
        public Int64 Class_ID { get; set; }

        public string ClassCode { get; set; }


    }

    public class BrandModel
    {
        public Int64 Brand_ID { get; set; }

        public string BrandName { get; set; }


    }
    public class UomConersiopn
    {
        public decimal packing_quantity { get; set; }
        public decimal sProduct_quantity { get; set; }

        public Int32 AltUOMId { get; set; }
        public bool isOverideConvertion { get; set; }
    }

    public class ProductRowQuantity
    {
        public Int64 ProductDisId { get; set; }
        public Int64 WarhouseId { get; set; }

        public Int32 UserId { get; set; }
        public decimal StockSheet_StockQty { get; set; }
        public decimal StockSheet_AltQty { get; set; }
        public string AsOnDate { get; set; }
        public string CountRow { get; set; }
    }
    public class ProductcHECK
    {
        public Int64 RETURNVALUE { get; set; }
      
    }
}