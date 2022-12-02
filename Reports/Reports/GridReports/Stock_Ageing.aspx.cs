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
using DataAccessLayer;
using System.Threading.Tasks;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using System.Drawing;
using Reports.Model;

namespace Reports.Reports.GridReports
{
    public partial class Stock_Ageing : System.Web.UI.Page
    {
        ReportData rpt = new ReportData();
        DataTable DTIndustry = new DataTable();
        DateTime dtFrom;
        DateTime dtTo;
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        string data = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        string strClassList = "", strBrandList = "";

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Reports/master/GstrReport.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            if (!IsPostBack)
            {
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Stock Ageing";
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

                //Session["dt_PartyLedgerRpt"] = null;
                //Session["dt_GSTRRpt"] = null;
                Session["IsStockAgeingFilter"] = null;
                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                ASPxToDate.Value = DateTime.Now;
                dtTo = Convert.ToDateTime(ASPxToDate.Date);
                string TODATE = dtTo.ToString("yyyy-MM-dd");
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                BranchpopulateGSTN();

                //lookupClass.DataSource = GetClassList();
                //lookupClass.DataBind();
                //lookupBrand.DataSource = GetBrandList();
                //lookupBrand.DataBind();
                // ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");
                //ASPxToDate.Value = DateTime.Now;
                //Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                //ASPxToDate.Value = DateTime.Now;                
            }
            else
            {

            }
        }

        public void Date_finyearwise(string Finyear)
        {
            //CommonBL bll1 = new CommonBL();
            //DataTable stbill = new DataTable();
            //stbill = bll1.GetDateFinancila(Finyear);
            //if (stbill.Rows.Count > 0)
            //{

            //    ASPxToDate.Text = Convert.ToDateTime(stbill.Rows[0]["FinYear_EndDate"]).ToString("dd-MM-yyyy");
            //}
            CommonBL stkage = new CommonBL();
            DataTable dtstkage = new DataTable();

            dtstkage = stkage.GetDateFinancila(Finyear);
            if (dtstkage.Rows.Count > 0)
            {

                ASPxToDate.MaxDate = Convert.ToDateTime((dtstkage.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((dtstkage.Rows[0]["FinYear_StartDate"]));


                DateTime MaximumDate = Convert.ToDateTime((dtstkage.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((dtstkage.Rows[0]["FinYear_StartDate"]));

                DateTime TodayDate = Convert.ToDateTime(DateTime.Now);
                DateTime FinYearEndDate = MaximumDate;

                if (TodayDate > FinYearEndDate)
                {
                    ASPxToDate.Date = FinYearEndDate;
                }
                else
                {
                    ASPxToDate.Date = TodayDate;
                }

            }
        }

        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
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
            string filename = "Stock Aging";
            exporter.FileName = filename;
            string FileHeader = "";

            exporter.FileName = filename;

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true,true, true, true) + Environment.NewLine + "Stock Aging" + Environment.NewLine + "As on " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");

            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";

            //exporter.PageFooter.Center = "[Page # of Pages #]";
            //exporter.PageFooter.Right = "[Date Printed]";
            exporter.Landscape = true;
            exporter.MaxColumnWidth = 50;
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

        //protected void Grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            //Session.Remove("dt_GSTRRpt");
            //DateTime dtFrom;
            //DateTime dtTo;
            //ShowGrid.JSProperties["cpSave"] = null;

            string IsStockAgeingFilter = Convert.ToString(hfIsStockAgeingFilter.Value);
            Session["IsStockAgeingFilter"] = IsStockAgeingFilter;

            dtTo = Convert.ToDateTime(ASPxToDate.Date);
            string TODATE = dtTo.ToString("yyyy-MM-dd");            
            string returnPara = Convert.ToString(e.Parameter);

            string WhichCall = returnPara.Split('~')[0];
            if (WhichCall == "ListData")
            {
                string WhichCall2 = returnPara.Split('~')[1];
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

                string BRANCH_ID = "";
                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }
                Task PopulateStockTrialDataTask = new Task(() => GetStockAging(TODATE, WhichCall2));
                PopulateStockTrialDataTask.RunSynchronously();
            }
        }

        //protected void grid_DataBinding(object sender, EventArgs e)
        //{
        //    if (Session["dt_PartyLedgerRpt"] != null)
        //    {
        //        ShowGrid.DataSource = (DataTable)Session["dt_PartyLedgerRpt"];

        //    }

        //}

        public void GetStockAging(string Todate, string Gstn)
        {
            try
            {
                //string DriverName = string.Empty;
                //string PhoneNo = string.Empty;
                //string VehicleNo = string.Empty;
                //DataTable dttab = new DataTable();
                //DateTime dtFrom;
                //DateTime dtTo;              

                //List<object> ClassList = lookupClass.GridView.GetSelectedFieldValues("ProductClass_ID");
                //foreach (object Class in ClassList)
                //{
                //    strClassList += "," + Class;
                //}
                //strClassList = strClassList.TrimStart(',');

                //List<object> BrandList = lookupBrand.GridView.GetSelectedFieldValues("Brand_Id");
                //foreach (object Brand in BrandList)
                //{
                //    strBrandList += "," + Brand;
                //}
                //strBrandList = strBrandList.TrimStart(',');

                //dttab = GetStockagingbind(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]).Trim(), Gstn, TODATE, strClassList, strBrandList);

                //if (dttab.Rows.Count > 0)
                //{
                //    Session["dt_PartyLedgerRpt"] = dttab;
                //    ShowGrid.DataSource = dttab;
                //    ShowGrid.DataBind();
                //}
                //else
                //{
                //    Session["dt_PartyLedgerRpt"] = null;
                //    ShowGrid.DataSource = null;
                //    ShowGrid.DataBind();
                //}

                dtTo = Convert.ToDateTime(ASPxToDate.Date);
                string TODATE = dtTo.ToString("yyyy-MM-dd");

                DataTable ds = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("prc_StockAgeing_Report");
                proc.AddPara("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                proc.AddPara("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                proc.AddPara("@GSTIN", Gstn);
                proc.AddPara("@ASONDATE", TODATE);
                proc.AddPara("@CLASS", hdnClassId.Value);
                proc.AddPara("@CATEGORY", hdnBranndId.Value);
                proc.AddPara("@USERID", Convert.ToInt32(Session["userid"]));

                ds = proc.GetTable();
            }
            catch (Exception ex)
            {
            }
        }

        #region LinQ
        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsStockAgeingFilter"]) == "Y")
            {
                var q = from d in dc.STOCKAGEING_REPORTs
                        where Convert.ToString(d.USERID) == Userid 
                        orderby d.SEQ ascending
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.STOCKAGEING_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
        }
        #endregion

        protected void ShowGrid_HtmlFooterCellPrepared(object sender, ASPxGridViewTableFooterCellEventArgs e)
        {
            if (e.Column.Caption == "Stock in Hand")
            {
                e.Cell.Style["text-align"] = "right";
            }

        }

        //public DataTable GetStockagingbind(string Company, string Finyear, string Gstn, string TODATE, string ClassList, string strBrandList)
        //{
        //    DataTable ds = new DataTable();
        //    ProcedureExecute proc = new ProcedureExecute("prc_StockAgeing_Report");
        //    proc.AddPara("@COMPANYID", Company);
        //    proc.AddPara("@FINYEAR", Finyear);
        //    proc.AddPara("@GSTIN", Gstn);
        //    proc.AddPara("@ASONDATE", TODATE);
        //    proc.AddPara("@CLASS", strClassList);
        //    proc.AddPara("@CATEGORY", strBrandList);
        //    ds = proc.GetTable();
        //    return ds;
        //}

        #region ########  Branch GRN Populate  #######
        protected void BranchpopulateGSTN()
        {
            // DataTable dst = new DataTable();
            string userbranchID = Convert.ToString(Session["userbranchID"]);

            DataSet ds = new DataSet();
            //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("GetGSTNfetch", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Company", Convert.ToString(Session["LastCompany"]));
            cmd.Parameters.AddWithValue("@Branchlist", Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);

            cmd.Dispose();
            con.Dispose();


            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlgstn.DataSource = ds.Tables[0];
                ddlgstn.DataTextField = "branch_GSTIN";
                ddlgstn.DataValueField = "branch_GSTIN";
                ddlgstn.DataBind();
                ddlgstn.Items.Insert(0, "");
            }
        }

        #endregion

        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }

        //protected void lookupClass_DataBinding(object sender, EventArgs e)
        //{
        //    lookupClass.DataSource = GetClassList();
        //}

        //protected void lookupBrand_DataBinding(object sender, EventArgs e)
        //{
        //    lookupBrand.DataSource = GetBrandList();
        //}

        //public DataTable GetClassList()
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

        //public DataTable GetBrandList()
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
    }
}