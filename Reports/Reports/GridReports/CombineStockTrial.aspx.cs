using DevExpress.Web;
using DevExpress.Web.Mvc;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using System.Collections.Specialized;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using DevExpress.Data;
using System.Threading.Tasks;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using System.Drawing;
using Reports.Model;
using DataAccessLayer;

namespace Reports.Reports.GridReports
{
    public partial class CombineStockTrial : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Combined Stock Trial";
                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, false, false, false, false, false);
                CompName.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, true, false, false, false, false);
                CompAdd.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, true, false, false, false);
                CompOth.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, false, true, false, false);
                CompPh.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, false, false, false, true);
                CompAccPrd.Text = GridHeader;

                //Session.Remove("dt_CombineStockTrailRpt");
                //Session.Remove("dt_CombineStockTrailRptLeve2");
                //Session.Remove("dt_CombineStockTrailRptLeve3");

                Session["IsStockTrialL1Filter"] = null;
                Session["IsStockTrialL2Filter"] = null;
                Session["IsStockTrialL3Filter"] = null;
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));

                //lookupClass.DataSource = GetClassList();
                //lookupClass.DataBind();

                //lookupBrand.DataSource = GetBrandList();
                //lookupBrand.DataBind();

                //Rev Debashis
                //string strFinYear = Convert.ToString(Session["LastFinYear"]);
                //DataTable dt = oDBEngine.GetDataTable("Select FinYear_Code,FinYear_StartDate,FinYear_EndDate From Master_FinYear Where FinYear_Code='" + strFinYear + "'");
                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    //string strStartDate = Convert.ToString(dt.Rows[0]["FinYear_StartDate"]);
                //    //DateTime StartDate = Convert.ToDateTime(strStartDate);
                //    //ASPxFromDate.Value = StartDate;

                //    //string strEndDate = Convert.ToString(dt.Rows[0]["FinYear_EndDate"]);
                //    //DateTime EndDate = Convert.ToDateTime(strEndDate);
                //    //ASPxToDate.Value = EndDate;

                //    ASPxFromDate.Value = DateTime.Now;
                //    ASPxToDate.Value = DateTime.Now;
                //}
                //else
                //{
                //    ASPxFromDate.Value = DateTime.Now;
                //    ASPxToDate.Value = DateTime.Now;
                //}
                //End of Rev Debashis
            }
        }

        //#region #### I/P Parameters Binding ####

        //private DataTable GetClassList()
        //{
        //    try
        //    {
        //        DataTable dt = oDBEngine.GetDataTable("Select ProductClass_ID,ProductClass_Name From Master_ProductClass Order By ProductClass_Name Asc");
        //        return dt;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}
        //private DataTable GetBrandList()
        //{
        //    try
        //    {
        //        DataTable dt = oDBEngine.GetDataTable("Select Brand_Id,Brand_Name From tbl_master_brand Where Brand_IsActive=1 Order By Brand_Name Asc");
        //        return dt;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}
        //#endregion

        //#region Lookup Details

        //protected void lookupClass_DataBinding(object sender, EventArgs e)
        //{
        //    lookupClass.DataSource = GetClassList();
        //}
        //protected void lookupBrand_DataBinding(object sender, EventArgs e)
        //{
        //    lookupBrand.DataSource = GetBrandList();
        //}

        //#endregion

        #region #### 1st Level Grid Details #####

        //protected void Grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        protected void CallbackPanelL1_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            //Session.Remove("dt_CombineStockTrailRpt");
            //ShowGrid.JSProperties["cpSave"] = null;
            string IsStockTrialL1Filter = Convert.ToString(hfIsStockTrialL1Filter.Value);
            Session["IsStockTrialL1Filter"] = IsStockTrialL1Filter;

            string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

            DateTime dtFrom;
            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);

            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");

            DateTime dtTo;
            dtTo = Convert.ToDateTime(ASPxToDate.Date);

            string TODATE = dtTo.ToString("yyyy-MM-dd");

            string Class_Ids = string.Empty;
            string Brand_Ids = string.Empty;

            //string ClassList = "";
            //if (hflookupClassAllFlag.Value.ToUpper() != "ALL")
            //{
            //    List<object> QuoList1 = lookupClass.GridView.GetSelectedFieldValues("ProductClass_ID");
            //    foreach (object Quo in QuoList1)
            //    {
            //        ClassList += "," + Quo;
            //    }
            //    Class_Ids = ClassList.TrimStart(',');
            //}

            //string BrandList = "";
            //if (hflookupBrandAllFlag.Value.ToUpper() != "ALL")
            //{
            //    List<object> QuoList2 = lookupBrand.GridView.GetSelectedFieldValues("Brand_Id");
            //    foreach (object Quo in QuoList2)
            //    {
            //        BrandList += "," + Quo;
            //    }
            //    Brand_Ids = BrandList.TrimStart(',');
            //}
            Class_Ids = hdnClassId.Value;
            Brand_Ids = hdnBranndId.Value;

            string TransType = Convert.ToString(ddlisdocument.SelectedValue);

            Task PopulateStockTrialDataTask = new Task(() => GetCombineStockTrailLevel1(FROMDATE, TODATE, Brand_Ids, Class_Ids, TransType));
            PopulateStockTrialDataTask.RunSynchronously();
        }
        //Rev Debashis
        public void Date_finyearwise(string Finyear)
        {
            CommonBL cmbstktrl = new CommonBL();
            DataTable dtcmbstktrl = new DataTable();

            dtcmbstktrl = cmbstktrl.GetDateFinancila(Finyear);
            if (dtcmbstktrl.Rows.Count > 0)
            {
                ASPxFromDate.MaxDate = Convert.ToDateTime((dtcmbstktrl.Rows[0]["FinYear_EndDate"]));
                ASPxFromDate.MinDate = Convert.ToDateTime((dtcmbstktrl.Rows[0]["FinYear_StartDate"]));

                ASPxToDate.MaxDate = Convert.ToDateTime((dtcmbstktrl.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((dtcmbstktrl.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((dtcmbstktrl.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((dtcmbstktrl.Rows[0]["FinYear_StartDate"]));

                DateTime TodayDate = Convert.ToDateTime(DateTime.Now);
                DateTime FinYearEndDate = MaximumDate;

                if (TodayDate > FinYearEndDate)
                {
                    ASPxToDate.Date = FinYearEndDate;
                    ASPxFromDate.Date = MinimumDate;
                }
                else
                {
                    ASPxToDate.Date = TodayDate;
                    ASPxFromDate.Date = MinimumDate;
                }

            }
        }
        //End of Rev Debashis

        protected void Showgrid_DataBound(object sender, EventArgs e)
        {
            ASPxGridView grid = (ASPxGridView)sender;
            foreach (GridViewDataColumn c in grid.Columns)
            {
                if (Session["IsStockTrialL1Filter"] == null)
                {
                    if ((c.FieldName.ToString()).StartsWith("USERID"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("TRANTYPE"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("SLNO"))
                    {
                        c.Visible = true;
                        c.Caption = "Sl. No.";
                    }
                    if ((c.FieldName.ToString()).StartsWith("sProducts_ID"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("Class"))
                    {
                        c.Visible = true;
                    }
                    if ((c.FieldName.ToString()).StartsWith("Category"))
                    {
                        c.Visible = true;
                    }
                    if ((c.FieldName.ToString()).StartsWith("Particulars"))
                    {
                        c.Visible = true;
                    }
                    if ((c.FieldName.ToString()).StartsWith("Opening"))
                    {
                        c.Visible = true;
                        c.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                    }
                    if ((c.FieldName.ToString()).StartsWith("Received"))
                    {
                        c.Visible = true;
                        c.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                    }
                    if ((c.FieldName.ToString()).StartsWith("Issue"))
                    {
                        c.Visible = true;
                        c.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                    }
                    if ((c.FieldName.ToString()).StartsWith("Closing"))
                    {
                        c.Visible = true;
                        c.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                    }
                    if ((c.FieldName.ToString()).StartsWith("Units"))
                    {
                        c.Visible = true;
                    }
                    if ((c.FieldName.ToString()).StartsWith("Sale_Purchase"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("SPReturn"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("Net_SalesPurchase"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("SalePurchase_Value"))
                    {
                        c.Visible = false;
                    }
                }
                else
                {
                    if ((c.FieldName.ToString()).StartsWith("USERID"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("TRANTYPE"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("SLNO"))
                    {
                        c.Caption = "Sl. No.";
                        c.Width = 75;
                        c.VisibleIndex = 1;
                        c.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                    }
                    if ((c.FieldName.ToString()).StartsWith("sProducts_ID"))
                    {
                        //c.Visible = false;
                        c.VisibleIndex = 0;
                        c.Width = 0;
                    }
                    if ((c.FieldName.ToString()).StartsWith("Class"))
                    {                        
                        c.Width = 200;
                        c.VisibleIndex = 2;
                    }
                    if ((c.FieldName.ToString()).StartsWith("Category"))
                    {
                        c.Width = 200;
                        c.VisibleIndex = 3;
                    }
                    if ((c.FieldName.ToString()).StartsWith("Particulars"))
                    {
                        c.Width = 350;
                        c.VisibleIndex = 4;
                    }
                    if (ddlisdocument.SelectedValue == "All")
                    {
                        if ((c.FieldName.ToString()).StartsWith("Opening"))
                        {
                            c.VisibleIndex = 5;
                            c.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                        }
                        if ((c.FieldName.ToString()).StartsWith("Received"))
                        {
                            c.VisibleIndex = 6;
                            c.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                        }
                        if ((c.FieldName.ToString()).StartsWith("Issue"))
                        {
                            c.VisibleIndex = 7;
                            c.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                        }
                        if ((c.FieldName.ToString()).StartsWith("Closing"))
                        {
                            c.VisibleIndex = 8;
                            c.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                        }
                        if ((c.FieldName.ToString()).StartsWith("Units"))
                        {
                            c.VisibleIndex = 9;
                        }
                        if ((c.FieldName.ToString()).StartsWith("Sale_Purchase"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("SPReturn"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("Net_SalesPurchase"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("SalePurchase_Value"))
                        {
                            c.Visible = false;
                        }
                    }
                    else if (ddlisdocument.SelectedValue == "Sales")
                    {
                        if ((c.FieldName.ToString()).StartsWith("Sale_Purchase"))
                        {
                            c.Visible = true;
                            c.VisibleIndex = 5;
                            c.Caption = "Sale";
                            c.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                        }
                        if ((c.FieldName.ToString()).StartsWith("SPReturn"))
                        {
                            c.Visible = true;
                            c.VisibleIndex = 6;
                            c.Caption = "Return";
                            c.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                        }
                        if ((c.FieldName.ToString()).StartsWith("Net_SalesPurchase"))
                        {
                            c.Visible = true;
                            c.VisibleIndex = 7;
                            c.Caption = "Net Sale";
                            c.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                        }
                        if ((c.FieldName.ToString()).StartsWith("SalePurchase_Value"))
                        {
                            c.Visible = true;
                            c.VisibleIndex = 8;
                            c.Caption = "Sale Value";
                            c.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                        }
                        if ((c.FieldName.ToString()).StartsWith("Opening"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("Received"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("Issue"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("Closing"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("Units"))
                        {
                            c.Visible = false;
                        }
                    }
                    else if (ddlisdocument.SelectedValue == "Purchases")
                    {
                        if ((c.FieldName.ToString()).StartsWith("Sale_Purchase"))
                        {
                            c.Visible = true;
                            c.VisibleIndex = 5;
                            c.Caption = "Purchase";
                            c.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                        }
                        if ((c.FieldName.ToString()).StartsWith("SPReturn"))
                        {
                            c.Visible = true;
                            c.VisibleIndex = 6;
                            c.Caption = "Return";
                            c.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                        }
                        if ((c.FieldName.ToString()).StartsWith("Net_SalesPurchase"))
                        {
                            c.Visible = true;
                            c.VisibleIndex = 7;
                            c.Caption = "Net Purchase";
                            c.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                        }
                        if ((c.FieldName.ToString()).StartsWith("SalePurchase_Value"))
                        {
                            c.Visible = true;
                            c.VisibleIndex = 8;
                            c.Caption = "Purchase Value";
                            c.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                        }
                        if ((c.FieldName.ToString()).StartsWith("Opening"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("Received"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("Issue"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("Closing"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("Units"))
                        {
                            c.Visible = false;
                        }
                    }
                }
                //if ((c.FieldName.ToString()).StartsWith("sProducts_Description"))
                //{
                //    c.Caption = "Product";
                //}
            }
        }

        private void GetCombineStockTrailLevel1(string FromDate, string ToDate, string BrandIds, string ClassIds, string TransType)
        {
            try
            {
                //DataSet ds = new DataSet();
                DataTable dtL1 = new DataTable();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                if (TransType == "All")
                {
                    //SqlCommand cmd = new SqlCommand("PROC_COMBINDSTOCKTRIALBRANDCLASS_REPORT", con);
                    //cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                    //cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]).Trim());
                    //cmd.Parameters.AddWithValue("@FROMDATE", FromDate);
                    //cmd.Parameters.AddWithValue("@TODATE", ToDate);
                    //cmd.Parameters.AddWithValue("@BRAND_ID", BrandIds);
                    //cmd.Parameters.AddWithValue("@CLASS", ClassIds);

                    //cmd.CommandTimeout = 0;
                    //SqlDataAdapter da = new SqlDataAdapter();
                    //da.SelectCommand = cmd;
                    //da.Fill(ds);
                    //cmd.Dispose();
                    ProcedureExecute proc = new ProcedureExecute("PROC_COMBINDSTOCKTRIALBRANDCLASS_REPORT");
                    proc.AddPara("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                    proc.AddPara("@FINYEAR", Convert.ToString(Session["LastFinYear"]).Trim());
                    proc.AddPara("@FROMDATE", FromDate);
                    proc.AddPara("@TODATE", ToDate);
                    proc.AddPara("@BRAND_ID", BrandIds);
                    proc.AddPara("@CLASS", ClassIds);
                    proc.AddPara("@USERID", Convert.ToInt32(Session["userid"]));

                    dtL1 = proc.GetTable();
                }
                else
                {
                    //SqlCommand cmd = new SqlCommand("prc_COMBINEDITEMWISESALESPURCHASE_REPORT", con);
                    //cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                    //cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]).Trim());
                    //cmd.Parameters.AddWithValue("@FROMDATE", FromDate);
                    //cmd.Parameters.AddWithValue("@TODATE", ToDate);
                    //cmd.Parameters.AddWithValue("@CLASS", ClassIds);
                    //cmd.Parameters.AddWithValue("@CATEGORY", BrandIds);
                    //cmd.Parameters.AddWithValue("@TRANTYPE", TransType);

                    //cmd.CommandTimeout = 0;
                    //SqlDataAdapter da = new SqlDataAdapter();
                    //da.SelectCommand = cmd;
                    //da.Fill(ds);
                    //cmd.Dispose();
                    ProcedureExecute proc = new ProcedureExecute("prc_COMBINEDITEMWISESALESPURCHASE_REPORT");
                    proc.AddPara("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                    proc.AddPara("@FINYEAR", Convert.ToString(Session["LastFinYear"]).Trim());
                    proc.AddPara("@FROMDATE", FromDate);
                    proc.AddPara("@TODATE", ToDate);
                    proc.AddPara("@CLASS", ClassIds);
                    proc.AddPara("@CATEGORY", BrandIds);
                    proc.AddPara("@TRANTYPE", TransType);
                    proc.AddPara("@USERID", Convert.ToInt32(Session["userid"]));

                    dtL1 = proc.GetTable();
                }

                con.Dispose();

                //Session["dt_CombineStockTrailRpt"] = ds.Tables[0];
                ShowGrid.Columns.Clear();
                //ShowGrid.DataSource = ds.Tables[0];
                //ShowGrid.DataBind();
            }
            catch (Exception ex)
            {
            }
        }

        //protected void grid2_DataBinding(object sender, EventArgs e)
        //{

        //    if (Session["dt_CombineStockTrailRpt"] != null)
        //    {
        //        ShowGrid.DataSource = (DataTable)Session["dt_CombineStockTrailRpt"];
        //    }
        //    else
        //    {
        //        ShowGrid.DataSource = null;
        //    }
        //}

        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {

                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    bindexport(Filter);
                }

                drdExport.SelectedValue = "0";
            }
        }

        public void bindexport(int Filter)
        {
            string filename = "Combined Stock Trial Report";
            string FileHeader = "";

            exporter.FileName = filename;

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true,true, true, true, true) + Environment.NewLine + "Combined Stock Trial Report" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");

            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";

            //exporter.PageFooter.Center = "[Page # of Pages #]";
            //exporter.PageFooter.Right = "[Date Printed]";
            exporter.Landscape = true;
            exporter.MaxColumnWidth = 100;
            exporter.GridViewID = "ShowGrid";
            exporter.RenderBrick += exporter_RenderBrick;
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

        protected void exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            e.BrickStyle.BackColor = Color.White;
            e.BrickStyle.ForeColor = Color.Black;
        }

        #region LinQL1
        protected void GenerateEntityServerModeDataSourceLevel1_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SLNO";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsStockTrialL1Filter"]) == "Y")
            {
                var q = from d in dc.COMBINDSTOCKTRIALLEVEL1_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.TRANTYPE) == ddlisdocument.SelectedValue
                        orderby d.SLNO
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.COMBINDSTOCKTRIALLEVEL1_REPORTs
                        where Convert.ToString(d.SLNO) == "0"
                        orderby d.SLNO
                        select d;
                e.QueryableSource = q;
            }
        }
        #endregion

        #endregion       

        #region ##### 2nd Level Grid Details #########
        //protected void ShowGridDetails2Level_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        protected void CallbackPanelL2_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            //string _ProductID = Convert.ToString(e.Parameters);
            string _ProductID = Convert.ToString(e.Parameter);
            
            string IsStockTrialL2Filter = Convert.ToString(hfIsStockTrialL2Filter.Value);
            Session["IsStockTrialL2Filter"] = IsStockTrialL2Filter;

            if (!string.IsNullOrEmpty(_ProductID))
            {
                //Session.Remove("dt_CombineStockTrailRptLeve2");
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

                DateTime dtFrom;
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);

                string FROMDATE = dtFrom.ToString("yyyy-MM-dd");                

                DateTime dtTo;
                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                string TODATE = dtTo.ToString("yyyy-MM-dd");

                string FRMDT = dtFrom.ToString("dd-MM-yyyy");
                string TODT = dtTo.ToString("dd-MM-yyyy");

                string TransType = Convert.ToString(ddlisdocument.SelectedValue);

                DataTable dt = oDBEngine.GetDataTable("Select sProducts_Code,sProducts_Name From Master_sProducts Where sProducts_ID='" + _ProductID + "'");
                if (dt != null && dt.Rows.Count > 0)
                {
                    CallbackPanelL2.JSProperties["cpProductCode"] = Convert.ToString(dt.Rows[0]["sProducts_Code"]);
                    CallbackPanelL2.JSProperties["cpProductDesc"] = Convert.ToString(dt.Rows[0]["sProducts_Name"]);
                    CallbackPanelL2.JSProperties["cpFromDate"] = FRMDT;
                    CallbackPanelL2.JSProperties["cpToDate"] = TODT;
                }

                //  ShowGridDetails2Level.DataSource = GetCombineStockTrail2ndLevel(FROMDATE, TODATE, _ProductID, TransType);

                //DataTable dt = new DataTable();
                //dt = GetCombineStockTrail2ndLevel(FROMDATE, TODATE, _ProductID, TransType);
                //Session["dt_CombineStockTrailRptLeve2"] = dt;
                //if (Session["dt_CombineStockTrailRptLeve2"] != null)
                //{
                //    // DataTable dt = (DataTable)Session["dt_CombineStockTrailRptLeve2"];
                //    if (dt.Rows.Count > 0)
                //    {
                //        ShowGridDetails2Level.JSProperties["cpProductCode"] = Convert.ToString(dt.Rows[0]["sProducts_Code"]);
                //        ShowGridDetails2Level.JSProperties["cpProductDesc"] = Convert.ToString(dt.Rows[0]["Particulars"]);
                //        ShowGridDetails2Level.JSProperties["cpFromDate"] = dtFrom.ToString("dd-MM-yyyy");
                //        ShowGridDetails2Level.JSProperties["cpToDate"] = dtTo.ToString("dd-MM-yyyy");
                //    }
                //}

                //ShowGridDetails2Level.DataSource = dt;
                //ShowGridDetails2Level.DataBind();

                Task PopulateStockTrialDataTask = new Task(() => GetCombineStockTrail2ndLevel(FROMDATE, TODATE, _ProductID, TransType));
                PopulateStockTrialDataTask.RunSynchronously();
            }
        }

        protected void ShowGridDetails2Level_DataBound(object sender, EventArgs e)
        {
            ASPxGridView grid = (ASPxGridView)sender;
            foreach (GridViewDataColumn c in grid.Columns)
            {
                if (Session["IsStockTrialL2Filter"] == null)
                {
                    if ((c.FieldName.ToString()).StartsWith("USERID"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("TRANTYPE"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("SL"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("sProducts_ID"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("sProducts_Code"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("branch_id"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("Branch"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("Particulars"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("Opening"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("Received"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("Issue"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("Closing"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("Sale_Purchase"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("SPReturn"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("Rate"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("Value"))
                    {
                        c.Visible = false;
                    }
                }
                else
                {
                    if ((c.FieldName.ToString()).StartsWith("USERID"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("TRANTYPE"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("SL"))
                    {
                        //c.Visible = false;
                        c.VisibleIndex = 0;
                        c.Width = 0;
                    }

                    if ((c.FieldName.ToString()).StartsWith("sProducts_ID"))
                    {
                        //c.Visible = false;
                        c.VisibleIndex = 1;
                        c.Width = 0;
                    }

                    if ((c.FieldName.ToString()).StartsWith("sProducts_Code"))
                    {
                        //c.Visible = false;
                        c.VisibleIndex = 2;
                        c.Width = 0;
                    }

                    if ((c.FieldName.ToString()).StartsWith("branch_id"))
                    {
                        //c.Visible = false;
                        c.VisibleIndex = 3;
                        c.Width = 0;
                    }
                    if ((c.FieldName.ToString()).StartsWith("Branch"))
                    {
                        c.VisibleIndex = 4;
                        c.Caption = "Unit";
                    }
                    if ((c.FieldName.ToString()).StartsWith("Particulars"))
                    {
                        c.VisibleIndex = 5;
                        c.Caption = "Particulars";
                    }
                    if (ddlisdocument.SelectedValue == "All")
                    {
                        if ((c.FieldName.ToString()).StartsWith("Opening"))
                        {
                            c.VisibleIndex = 6;
                            c.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                            c.PropertiesEdit.DisplayFormatString = "0.00";
                        }
                        if ((c.FieldName.ToString()).StartsWith("Received"))
                        {
                            c.VisibleIndex = 7;
                            c.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                            c.PropertiesEdit.DisplayFormatString = "0.00";
                        }
                        if ((c.FieldName.ToString()).StartsWith("Issue"))
                        {
                            c.VisibleIndex = 8;
                            c.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                            c.PropertiesEdit.DisplayFormatString = "0.00";
                        }
                        if ((c.FieldName.ToString()).StartsWith("Closing"))
                        {
                            c.VisibleIndex = 9;
                            c.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                            c.PropertiesEdit.DisplayFormatString = "0.00";
                        }
                        if ((c.FieldName.ToString()).StartsWith("Sale_Purchase"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("SPReturn"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("Rate"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("Value"))
                        {
                            c.Visible = false;
                        }
                    }
                    else if(ddlisdocument.SelectedValue != "All")
                    {
                        if ((c.FieldName.ToString()).StartsWith("Branch"))
                        {
                            c.VisibleIndex = 4;
                        }
                        if ((c.FieldName.ToString()).StartsWith("Particulars"))
                        {
                            c.VisibleIndex = 5;
                        }
                        if ((c.FieldName.ToString()).StartsWith("Opening"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("Received"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("Issue"))
                        {
                            c.Visible = false;
                        }
                        if ((c.FieldName.ToString()).StartsWith("Sale_Purchase"))
                        {
                            c.Visible = true;
                            c.VisibleIndex = 6;
                            c.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                            c.PropertiesEdit.DisplayFormatString = "0.00";
                            if (ddlisdocument.SelectedValue == "Sales")
                            {
                                c.Caption = "Sale";
                            }
                            else
                            {
                                c.Caption = "Purchase";
                            }
                        }
                        if ((c.FieldName.ToString()).StartsWith("SPReturn"))
                        {
                            c.Visible = true;
                            c.Caption = "Return";
                            c.VisibleIndex = 7;
                            c.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                            c.PropertiesEdit.DisplayFormatString = "0.00";
                        }
                        if ((c.FieldName.ToString()).StartsWith("Closing"))
                        {
                            c.Visible = true;
                            c.VisibleIndex = 8;
                            c.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                            c.PropertiesEdit.DisplayFormatString = "0.00";
                        }
                        if ((c.FieldName.ToString()).StartsWith("Rate"))
                        {
                            c.Visible = true;
                            c.VisibleIndex = 9;
                            c.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                            c.PropertiesEdit.DisplayFormatString = "0.00";
                        }
                        if ((c.FieldName.ToString()).StartsWith("Value"))
                        {
                            c.Visible = true;
                            c.VisibleIndex = 10;
                            c.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                            c.PropertiesEdit.DisplayFormatString = "0.00";
                        }
                    }

                    //if ((c.FieldName.ToString()).StartsWith("Branch"))
                    //{
                    //    c.Width = 100;
                    //}

                    //if ((c.FieldName.ToString()).StartsWith("Particulars"))
                    //{
                    //    c.Width = 200;
                    //}

                    //if ((c.FieldName.ToString()).StartsWith("Opening"))
                    //{
                    //    c.Width = 100;
                    //}

                    //if ((c.FieldName.ToString()).StartsWith("Received"))
                    //{
                    //    c.Width = 100;
                    //}

                    //if ((c.FieldName.ToString()).StartsWith("Issue"))
                    //{
                    //    c.Width = 100;
                    //}

                    //if ((c.FieldName.ToString()).StartsWith("Closing"))
                    //{
                    //    c.Width = 100;
                    //}
                }
            }
        }

        //private DataTable GetCombineStockTrail2ndLevel(string FromDate, string ToDate, string ProductID, string TransType)
        private void GetCombineStockTrail2ndLevel(string FromDate, string ToDate, string ProductID, string TransType)
        {
            try
            {
                //DataSet ds = new DataSet();
                DataTable dtL2 = new DataTable();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                if (TransType == "All")
                {
                    //SqlCommand cmd = new SqlCommand("PROC_BRANCHWISESTOCKTRIAL_REPORT", con);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                    //cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]).Trim());
                    //cmd.Parameters.AddWithValue("@FROMDATE", FromDate);
                    //cmd.Parameters.AddWithValue("@TODATE", ToDate);
                    //cmd.Parameters.AddWithValue("@PRODUCT_ID", ProductID);
                    //cmd.CommandTimeout = 0;
                    //SqlDataAdapter da = new SqlDataAdapter();
                    //da.SelectCommand = cmd;
                    //da.Fill(ds);
                    //cmd.Dispose();
                    //con.Dispose();

                    ProcedureExecute proc = new ProcedureExecute("PROC_BRANCHWISESTOCKTRIAL_REPORT");
                    proc.AddPara("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                    proc.AddPara("@FINYEAR", Convert.ToString(Session["LastFinYear"]).Trim());
                    proc.AddPara("@FROMDATE", FromDate);
                    proc.AddPara("@TODATE", ToDate);
                    proc.AddPara("@PRODUCT_ID", ProductID);
                    proc.AddPara("@USERID", Convert.ToInt32(Session["userid"]));

                    dtL2 = proc.GetTable();
                }
                else
                {
                    //SqlCommand cmd = new SqlCommand("prc_COMBINEDBRANCHWISESALESPURCHASE_REPORT", con);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                    //cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]).Trim());
                    //cmd.Parameters.AddWithValue("@FROMDATE", FromDate);
                    //cmd.Parameters.AddWithValue("@TODATE", ToDate);
                    //cmd.Parameters.AddWithValue("@PRODUCT_ID", ProductID);
                    //cmd.Parameters.AddWithValue("@TRANTYPE", TransType);
                    //cmd.CommandTimeout = 0;
                    //SqlDataAdapter da = new SqlDataAdapter();
                    //da.SelectCommand = cmd;
                    //da.Fill(ds);
                    //cmd.Dispose();
                    //con.Dispose();

                    ProcedureExecute proc = new ProcedureExecute("prc_COMBINEDBRANCHWISESALESPURCHASE_REPORT");
                    proc.AddPara("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                    proc.AddPara("@FINYEAR", Convert.ToString(Session["LastFinYear"]).Trim());
                    proc.AddPara("@FROMDATE", FromDate);
                    proc.AddPara("@TODATE", ToDate);
                    proc.AddPara("@PRODUCT_ID", ProductID);
                    proc.AddPara("@TRANTYPE", TransType);
                    proc.AddPara("@USERID", Convert.ToInt32(Session["userid"]));

                    dtL2 = proc.GetTable();
                }
                con.Dispose();
                ShowGridDetails2Level.Columns.Clear();

                //Session["dt_CombineStockTrailRptLeve2"] = ds.Tables[0];
                //ShowGridDetails2Level.DataSource = ds.Tables[0];
                //ShowGridDetails2Level.DataBind();
                //return ds.Tables[0];
            }
            catch (Exception ex)
            {
                //return null;
            }
        }
        //protected void ShowGridDetails2Level_DataBinding(object sender, EventArgs e)
        //{
        //    if (Session["dt_CombineStockTrailRptLeve2"] != null)
        //    {
        //        ShowGridDetails2Level.DataSource = (DataTable)Session["dt_CombineStockTrailRptLeve2"];
        //    }
        //    else
        //    {
        //        ShowGridDetails2Level.DataSource = null;
        //    }
        //}

        protected void ShowGridDetails2Level_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }


        protected void ddldetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(ddldetails.SelectedItem.Value));
            if (Filter != 0)
            {
                bindexport_Details(Filter);
            }
        }

        public void bindexport_Details(int Filter)
        {
            //if (Convert.ToString(ddlisdocument.SelectedValue) == "All")
            //{
            ShowGridDetails2Level.Columns[0].Visible = false;
            ShowGridDetails2Level.Columns[1].Visible = false;
            ShowGridDetails2Level.Columns[2].Visible = false;
            ShowGridDetails2Level.Columns[3].Visible = false;

            //}

            //    ShowGridDetails2Level.DataBind();
            string filename = "Combine Stock Trial 2nd Level Report";
            exporterDetails.FileName = filename;
            exporterDetails.FileName = "Combine Stock Trial 2nd Level Report";

            exporterDetails.PageHeader.Left = "Combine Stock Trial 2nd Level Report";
            exporterDetails.PageFooter.Center = "[Page # of Pages #]";
            exporterDetails.PageFooter.Right = "[Date Printed]";
            exporterDetails.GridViewID = "ShowGridDetails2Level";
            switch (Filter)
            {
                case 1:
                    exporterDetails.WritePdfToResponse();
                    break;
                case 2:
                    exporterDetails.WriteXlsxToResponse();
                    break;
                case 3:
                    exporterDetails.WriteRtfToResponse();
                    break;
                case 4:
                    exporterDetails.WriteCsvToResponse();
                    break;

                default:
                    return;
            }
        }

        #region LinQL2
        protected void GenerateEntityServerModeDataSourceLevel2_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SL";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsStockTrialL2Filter"]) == "Y")
            {
                var q = from d in dc.COMBINDSTOCKTRIALLEVEL2_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.TRANTYPE) == ddlisdocument.SelectedValue
                        orderby d.SL
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.COMBINDSTOCKTRIALLEVEL2_REPORTs
                        where Convert.ToString(d.SL) == "0"
                        orderby d.SL
                        select d;
                e.QueryableSource = q;
            }
        }
        #endregion

        #endregion

        #region ##### 3rd Level Grid Details #########
        //protected void ShowGridDetails3Level_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        protected void CallbackPanelL3_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            //string _ProductID = Convert.ToString(e.Parameters).Split('~')[0];
            //string _BranchID = Convert.ToString(e.Parameters).Split('~')[1];
            string _ProductID = Convert.ToString(e.Parameter).Split('~')[0];
            string _BranchID = Convert.ToString(e.Parameter).Split('~')[1];

            string IsStockTrialL3Filter = Convert.ToString(hfIsStockTrialL3Filter.Value);
            Session["IsStockTrialL3Filter"] = IsStockTrialL3Filter;

            if (!string.IsNullOrEmpty(_ProductID) && !string.IsNullOrEmpty(_BranchID))
            {
                //Session.Remove("dt_CombineStockTrailRptLeve3");
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

                DateTime dtFrom;
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);

                string FROMDATE = dtFrom.ToString("yyyy-MM-dd");

                DateTime dtTo;
                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                string TODATE = dtTo.ToString("yyyy-MM-dd");

                string TransType = Convert.ToString(ddlisdocument.SelectedValue);

                //ShowGridDetails3Level.DataSource = GetCombineStockTrail3rdLevel(FROMDATE, TODATE, _ProductID, _BranchID, "F", TransType);
                //ShowGridDetails3Level.DataBind();
                Task PopulateStockTrialDataTask = new Task(() => GetCombineStockTrail3rdLevel(FROMDATE, TODATE, _ProductID, _BranchID, "F", TransType));
                PopulateStockTrialDataTask.RunSynchronously();
            }
        }

        //private DataTable GetCombineStockTrail3rdLevel(string FromDate, string ToDate, string ProductID, string BranchID, string ValType, string TransType)
        private void GetCombineStockTrail3rdLevel(string FromDate, string ToDate, string ProductID, string BranchID, string ValType, string TransType)
        {
            try
            {
                //DataSet ds = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                //SqlCommand cmd = new SqlCommand("prc_StockLedger_Report", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                //cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]).Trim());
                //cmd.Parameters.AddWithValue("@FROMDATE", FromDate);
                //cmd.Parameters.AddWithValue("@TODATE", ToDate);
                //cmd.Parameters.AddWithValue("@BRANCHID", BranchID);
                //cmd.Parameters.AddWithValue("@PRODUCT_ID", ProductID);
                //cmd.Parameters.AddWithValue("@VAL_TYPE", ValType);
                //cmd.Parameters.AddWithValue("@TRANTYPE", TransType);
                //cmd.CommandTimeout = 0;
                //SqlDataAdapter da = new SqlDataAdapter();
                //da.SelectCommand = cmd;
                //da.Fill(ds);
                //cmd.Dispose();
                //con.Dispose();
                //Session["dt_CombineStockTrailRptLeve3"] = ds.Tables[0];
                //return ds.Tables[0];

                DataTable dtL3 = new DataTable();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                ProcedureExecute proc = new ProcedureExecute("prc_StockLedger_Report");
                proc.AddPara("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                proc.AddPara("@FINYEAR", Convert.ToString(Session["LastFinYear"]).Trim());
                proc.AddPara("@FROMDATE", FromDate);
                proc.AddPara("@TODATE", ToDate);
                proc.AddPara("@BRANCHID", BranchID);
                proc.AddPara("@PRODUCT_ID", ProductID);
                proc.AddPara("@VAL_TYPE", ValType);
                proc.AddPara("@TRANTYPE", TransType);
                proc.AddPara("@USERID", Convert.ToInt32(Session["userid"]));

                dtL3 = proc.GetTable();
                con.Dispose();
            }
            catch (Exception ex)
            {
                //return null;
            }
        }

        #region LinQL3
        protected void GenerateEntityServerModeDataSourceLevel3_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsStockTrialL3Filter"]) == "Y")
            {
                var q = from d in dc.COMBINDSTOCKTRIALLEVEL3_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.TRANTYPE) == ddlisdocument.SelectedValue
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.COMBINDSTOCKTRIALLEVEL3_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
        }
        #endregion

        //protected void ShowGridDetails3Level_DataBinding(object sender, EventArgs e)
        //{
        //    if (Session["dt_CombineStockTrailRptLeve3"] != null)
        //    {
        //        ShowGridDetails3Level.DataSource = (DataTable)Session["dt_CombineStockTrailRptLeve3"];
        //    }
        //    else
        //    {
        //        ShowGridDetails3Level.DataSource = null;
        //    }
        //}

        protected void ShowGridDetails3Level_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }

        protected void ddlExport3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(ddlExport3.SelectedItem.Value));
            if (Filter != 0)
            {
                bindexport_Details3(Filter);
            }
        }

        public void bindexport_Details3(int Filter)
        {
            string filename = "Combine Stock Trial 2nd Level Report";
            exporterDetails.FileName = filename;
            exporterDetails.FileName = "Combine Stock Trial 3rd Level Report";

            exporterDetails.PageHeader.Left = "Combine Stock Trial 3rd Level Report";
            exporterDetails.PageFooter.Center = "[Page # of Pages #]";
            exporterDetails.PageFooter.Right = "[Date Printed]";
            exporterDetails.GridViewID = "ShowGridDetails3Level";
            switch (Filter)
            {
                case 1:
                    exporterDetails.WritePdfToResponse();
                    break;
                case 2:
                    exporterDetails.WriteXlsxToResponse();
                    break;
                case 3:
                    exporterDetails.WriteRtfToResponse();
                    break;
                case 4:
                    exporterDetails.WriteCsvToResponse();
                    break;

                default:
                    return;
            }
        }
        #endregion




    }
}