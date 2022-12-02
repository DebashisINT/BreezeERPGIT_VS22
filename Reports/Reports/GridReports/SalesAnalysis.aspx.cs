using BusinessLogicLayer;
using DataAccessLayer;
using DevExpress.Web;
using EntityLayer.CommonELS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using System.Drawing;
using Reports.Model;
 
namespace Reports.Reports.GridReports
{
    public partial class SalesAnalysis : System.Web.UI.Page
    {
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        static string _ClassID = "";
        static string _CategoryID = "";
        static string _ProductID = "";
        static decimal _PU_RATE = 0;

        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/SalesAnalysis.aspx");
            if (!IsPostBack)
            {
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Sales Analysis";
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

                lookupClass.DataSource = GetClassList();
                lookupClass.DataBind();

                lookupBrand.DataSource = GetBrandList();
                lookupBrand.DataBind();

                string strFinYear = Convert.ToString(Session["LastFinYear"]);
                DataTable dt = oDBEngine.GetDataTable("Select FinYear_Code,FinYear_StartDate,FinYear_EndDate From Master_FinYear Where FinYear_Code='" + strFinYear + "'");
                if (dt != null && dt.Rows.Count > 0)
                {
                    string strStartDate = Convert.ToString(dt.Rows[0]["FinYear_StartDate"]);
                    DateTime StartDate = Convert.ToDateTime(strStartDate);

                    ASPxFromDate.Value = StartDate;
                    ASPxToDate.Value = DateTime.Now;
                }
                else
                {
                    ASPxFromDate.Value = DateTime.Now;
                    ASPxToDate.Value = DateTime.Now;
                }
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                Session["IsSalesAnaSumm"] = null;
                Session["IsSalesAnaDet"] = null;
            }
        }
        public void Date_finyearwise(string Finyear)
        {
            CommonBL cbl = new CommonBL();
            DataTable fdtbl = new DataTable();
            DateTime MinDate, MaxDate;

            fdtbl = cbl.GetDateFinancila(Finyear);
            if (fdtbl.Rows.Count > 0)
            {

                ASPxFromDate.MaxDate = Convert.ToDateTime((fdtbl.Rows[0]["FinYear_EndDate"]));
                ASPxFromDate.MinDate = Convert.ToDateTime((fdtbl.Rows[0]["FinYear_StartDate"]));

                ASPxToDate.MaxDate = Convert.ToDateTime((fdtbl.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((fdtbl.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((fdtbl.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((fdtbl.Rows[0]["FinYear_StartDate"]));

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
        #region Lookup Details

        protected void lookupClass_DataBinding(object sender, EventArgs e)
        {
            lookupClass.DataSource = GetClassList();
        }
        protected void lookupBrand_DataBinding(object sender, EventArgs e)
        {
            lookupBrand.DataSource = GetBrandList();
        }
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string performpara = e.Parameter;
            string strProductID = Convert.ToString(performpara.Split('~')[0]);
            string strProductName = "", strProducCode = "";

            DataTable dt = oDBEngine.GetDataTable("Select sProducts_Code,sProducts_Name From Master_sProducts Where sProducts_ID='" + strProductID + "'");
            if (dt != null && dt.Rows.Count > 0)
            {
                strProducCode = Convert.ToString(dt.Rows[0]["sProducts_Code"]).Replace("'", "squot").Replace(",", "coma").Replace("/", "slash").Trim();
                strProductName = Convert.ToString(dt.Rows[0]["sProducts_Name"]).Replace("'", "squot").Replace(",", "coma").Replace("/", "slash").Trim();
            }

            CallbackPanel.JSProperties["cpProductValue"] = strProducCode + "||@||" + strProductName;
        }

        #endregion

        #region Grid Details

        protected void CallbackPanelSumm_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            string IsSalesAnaSumm = Convert.ToString(hfIsSalesAnaSumm.Value);
            Session["IsSalesAnaSumm"] = IsSalesAnaSumm;

            DataTable dtanalysis = new DataTable();
            Task PopulateStockTrialDataTask = new Task(() => GetSalesAnalysisHeader());
            PopulateStockTrialDataTask.RunSynchronously();
        }
        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }
        protected void CallbackPanelDet_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {

            string IsSalesAnaDet = Convert.ToString(hfIsSalesAnaDet.Value);
            Session["IsSalesAnaDet"] = IsSalesAnaDet;

            _ClassID = Convert.ToString(hdnClassID.Value);
            _CategoryID = Convert.ToString(hdnCategoryID.Value);
            _ProductID = Convert.ToString(hdnProductID.Value);

            if (Convert.ToString(hdnRate.Value) != "")
            {
                _PU_RATE = Convert.ToDecimal(hdnRate.Value);
            }
            else
            {
                _PU_RATE = 0;
            }
            //ShowGridDetails.DataSource = GetSalesAnalysisDetails();
            //ShowGridDetails.DataBind();

            Task PopulateStockTrialDataTaskDet = new Task(() => GetSalesAnalysisDetails());
            PopulateStockTrialDataTaskDet.RunSynchronously();
        }
        //protected void ShowGridDetails_DataBinding(object sender, EventArgs e)
        //{
        //    if (Session["DetReportDataSource"] != null)
        //    {
        //        ShowGridDetails.DataSource = (DataTable)Session["DetReportDataSource"];
        //    }
        //}
        protected void ShowGridDetails_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }

        #endregion

        #region Export Details
        protected void ddldetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(ddldetails.SelectedItem.Value));
            if (Filter != 0)
            {
                bindexport_Details(Filter);
            }
        }
        protected void drdExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                bindexport(Filter);
            }
        }
        public void bindexport(int Filter)
        {
            string filename = "SalesAnalysis Report";
            exporter.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true,true, true, true) + Environment.NewLine + "Sales Analysis Report" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");

            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";

            //exporter.PageFooter.Center = "[Page # of Pages #]";
            //exporter.PageFooter.Right = "[Date Printed]";
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

                default:
                    return;
            }
        }

        protected void exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            e.BrickStyle.BackColor = Color.White;
            e.BrickStyle.ForeColor = Color.Black;
        }
        public void bindexport_Details(int Filter)
        {
            string filename = "SalesAnalysisDetails Report";
            exporterDetails.FileName = filename;
            exporterDetails.FileName = "SalesAnalysisDetailsReport";
            string FileHeader = "";
            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();
            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Sales Analysis Details Report" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");

            exporterDetails.PageHeader.Left = "SalesAnalysisDetails Report";
            exporterDetails.PageFooter.Center = "[Page # of Pages #]";
            exporterDetails.PageFooter.Right = "[Date Printed]";
            exporterDetails.GridViewID = "ShowGridDetails";
            switch (Filter)
            {
                case 1:
                    exporterDetails.WritePdfToResponse();
                    break;
                case 2:
                    exporterDetails.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
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

        #region Database Details

        public DataTable GetClassList()
        {
            try
            {
                DataTable dt = oDBEngine.GetDataTable("Select ProductClass_ID,ProductClass_Name From Master_ProductClass Order By ProductClass_Name Asc");
                return dt;
            }
            catch
            {
                return null;
            }
        }
        public DataTable GetBrandList()
        {
            try
            {
                DataTable dt = oDBEngine.GetDataTable("Select Brand_Id,Brand_Name From tbl_master_brand Where Brand_IsActive=1 Order By Brand_Name Asc");
                return dt;
            }
            catch
            {
                return null;
            }
        }
        public void GetSalesAnalysisHeader()
        {
            string strClassList = "", strBrandList = "";
            string strCompany = Convert.ToString(Session["LastCompany"]);
            string strFinYear = Convert.ToString(Session["LastFinYear"]);
            string strFromDate = Convert.ToDateTime(ASPxFromDate.Value).ToString("yyyy-MM-dd");
            string strToDate = Convert.ToDateTime(ASPxToDate.Value).ToString("yyyy-MM-dd");
            string strRateCriteria = Convert.ToString(ddlRateCriteria.SelectedValue);

            if (lookupClass.GridView.GetSelectedFieldValues("ProductClass_ID").Count() != GetClassList().Rows.Count)
            {
                List<object> ClassList = lookupClass.GridView.GetSelectedFieldValues("ProductClass_ID");
                foreach (object Class in ClassList)
                {
                    strClassList += "," + Class;
                }
                strClassList = strClassList.TrimStart(',');
            }

            if (lookupBrand.GridView.GetSelectedFieldValues("Brand_Id").Count() != GetBrandList().Rows.Count)
            {
                List<object> BrandList = lookupBrand.GridView.GetSelectedFieldValues("Brand_Id");
                foreach (object Brand in BrandList)
                {
                    strBrandList += "," + Brand;
                }
                strBrandList = strBrandList.TrimStart(',');
            }
            try
            {
                //DataTable dt = new DataTable();
                //ProcedureExecute proc = new ProcedureExecute("prc_SalesAnalysis_Report");
                //proc.AddPara("@COMPANYID", strCompany);
                //proc.AddPara("@FINYEAR", strFinYear);
                //proc.AddPara("@FROMDATE", strFromDate);
                //proc.AddPara("@TODATE", strToDate);
                //proc.AddPara("@CLASS", strClassList);
                //proc.AddPara("@CATEGORY", strBrandList);
                //proc.AddBooleanPara("@ISGST", Convert.ToBoolean(strRateCriteria));
                //proc.AddPara("@USERID", Convert.ToInt32(Session["userid"]));
                //dt = proc.GetTable();

                //Session["ReportDataSource"] = dt;
                //ShowGrid.DataBind();
                //return dt;


                DataSet ds = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                SqlCommand cmd = new SqlCommand("prc_SalesAnalysis_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", strCompany);
                cmd.Parameters.AddWithValue("@FINYEAR", strFinYear);
                cmd.Parameters.AddWithValue("@FROMDATE", strFromDate);
                cmd.Parameters.AddWithValue("@TODATE", strToDate);
                cmd.Parameters.AddWithValue("@CLASS", strClassList);
                cmd.Parameters.AddWithValue("@CATEGORY", strBrandList);
                cmd.Parameters.AddWithValue("@ISGST", Convert.ToBoolean(strRateCriteria));
                cmd.Parameters.AddWithValue("@USERID", Convert.ToInt32(Session["userid"]));

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);

                cmd.Dispose();
                con.Dispose();



            }
            catch
            {
                //return null;
            }
        }
        public void GetSalesAnalysisDetails()
        {
            string strClassList = "", strBrandList = "";
            string strCompany = Convert.ToString(Session["LastCompany"]);
            string strFinYear = Convert.ToString(Session["LastFinYear"]);
            string strFromDate = Convert.ToDateTime(ASPxFromDate.Value).ToString("yyyy-MM-dd");
            string strToDate = Convert.ToDateTime(ASPxToDate.Value).ToString("yyyy-MM-dd");
            string strRateCriteria = Convert.ToString(ddlRateCriteria.SelectedValue);
            string strClassID = Convert.ToString(_ClassID);
            string strCategoryID = Convert.ToString(_CategoryID);
            string strProductID = Convert.ToString(_ProductID);
            decimal Rate = Convert.ToDecimal(_PU_RATE);

            try
            {
                //DataTable dt = new DataTable();
                //ProcedureExecute proc = new ProcedureExecute("prc_SalesAnalysisDetails_Report");
                //proc.AddPara("@COMPANYID", strCompany);
                //proc.AddPara("@FINYEAR", strFinYear);
                //proc.AddPara("@FROMDATE", strFromDate);
                //proc.AddPara("@TODATE", strToDate);
                //proc.AddPara("@CLASS", strClassID);
                //proc.AddPara("@CATEGORY", strCategoryID);
                //proc.AddPara("@PRODUCT_ID", strProductID);
                //proc.AddPara("@PU_RATE", Rate);
                //proc.AddBooleanPara("@ISGST", Convert.ToBoolean(strRateCriteria));
                //dt = proc.GetTable();

                ////Session["DetReportDataSource"] = dt;
                //ShowGridDetails.DataBind();


                DataSet ds = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                SqlCommand cmd = new SqlCommand("prc_SalesAnalysisDetails_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", strCompany);
                cmd.Parameters.AddWithValue("@FINYEAR", strFinYear);
                cmd.Parameters.AddWithValue("@FROMDATE", strFromDate);
                cmd.Parameters.AddWithValue("@TODATE", strToDate);
                cmd.Parameters.AddWithValue("@CLASS", strClassID);
                cmd.Parameters.AddWithValue("@CATEGORY", strCategoryID);
                cmd.Parameters.AddWithValue("@PRODUCT_ID", strProductID);
                cmd.Parameters.AddWithValue("@PU_RATE", Rate);
                cmd.Parameters.AddWithValue("@ISGST", Convert.ToBoolean(strRateCriteria));
                cmd.Parameters.AddWithValue("@USERID", Convert.ToInt32(Session["userid"]));

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);

                cmd.Dispose();
                con.Dispose();


                //return dt;
            }
            catch
            {
                //return null;
            }
        }

        #endregion

        #region LinQ
        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Srlno";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsSalesAnaSumm"]) == "Y")
            {
                var q = from d in dc.SALESANALYSISSUMM_REPORTs
                        where Convert.ToString(d.USERID) == Userid
                        //orderby d.Category ascending, d.Class ascending, d.Particulars ascending
                        orderby d.Srlno ascending
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.SALESANALYSISSUMM_REPORTs
                        where Convert.ToString(d.Srlno) == "0"
                        //orderby d.Category ascending, d.Class ascending, d.Particulars ascending
                        orderby d.Srlno ascending
                        select d;
                e.QueryableSource = q;
            }
        }

        protected void GenerateEntityServerModeDetailsDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "Srlno";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsSalesAnaDet"]) == "Y")
            {
                var q = from d in dc.SALESANALYSISDET_REPORTs
                        where Convert.ToString(d.USERID) == Userid
                        //orderby d.Branch ascending,d.Category ascending, d.Class ascending, d.Particulars ascending
                        orderby d.Srlno ascending
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.SALESANALYSISDET_REPORTs
                        where Convert.ToString(d.Srlno) == "0"
                        //orderby d.Branch ascending, d.Category ascending, d.Class ascending, d.Particulars ascending
                        orderby d.Srlno ascending
                        select d;
                e.QueryableSource = q;
            }
        }
        #endregion
    }
}