using DevExpress.Web;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using BusinessLogicLayer;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using System.Drawing;
using Reports.Model;

namespace Reports.Reports.GridReports
{
    public partial class CRMSummary_Performance_F : System.Web.UI.Page
    {
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        DataTable dtCumSalesTillDtPrcn = null;
        decimal CumSalesTillDtPrcn;
        DataTable dtMonthWiseSalesPrcn = null;
        decimal AprilSalesPrcn;
        decimal MaySalesPrcn;
        decimal JuneSalesPrcn;
        decimal JulySalesPrcn;
        decimal AugustSalesPrcn;
        decimal SeptemberSalesPrcn;
        decimal OctoberSalesPrcn;
        decimal NovemberSalesPrcn;
        decimal DecemberSalesPrcn;
        decimal JanuarySalesPrcn;
        decimal FebruarySalesPrcn;
        decimal MarchSalesPrcn;

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
             DateTime dtFrom;
            DateTime dtTo;
            if (!IsPostBack)
            {
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "CRM Summary";
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

                Session["IsCRMSummaryFilter"] = null;
                Session["exportval"] = null;

                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;

                ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");
                ASPxToDate.Value = DateTime.Now;
                dtTo = Convert.ToDateTime(ASPxToDate.Date);
                string TODATE = dtTo.ToString("yyyy-MM-dd");

                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));

                chkallCustomer.Attributes.Add("OnClick", "AllCustomer('selectAllCustomer')");
                chkallSalesman.Attributes.Add("OnClick", "AllSalesman('selectAllSalesman')");
                chkallProduct.Attributes.Add("OnClick", "AllProduct('selectAllProduct')");              
            }
            else
            {
                dtTo = Convert.ToDateTime(ASPxToDate.Date);
            }           

        }


        public void Date_finyearwise(string Finyear)
        {
            CommonBL crmsum = new CommonBL();
            DataTable dtcrmsum = new DataTable();

            dtcrmsum = crmsum.GetDateFinancila(Finyear);
            if (dtcrmsum.Rows.Count > 0)
            {
                ASPxToDate.MaxDate = Convert.ToDateTime((dtcrmsum.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((dtcrmsum.Rows[0]["FinYear_StartDate"]));
                DateTime MaximumDate = Convert.ToDateTime((dtcrmsum.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((dtcrmsum.Rows[0]["FinYear_StartDate"]));
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

        #region Export
        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
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
            string filename = "CRM Summary";
            exporter.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "CRM Summary" + Environment.NewLine + "As On: " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");

            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";

            exporter.GridViewID = "ShowGrid";
            exporter.RenderBrick += exporter_RenderBrick;
            switch (Filter)
            {
                case 1:
                    exporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });                    
                    break;
                case 2:
                    exporter.WriteCsvToResponse();
                    break;
                case 3:
                    exporter.WritePdfToResponse();
                    break;
                case 4:
                    exporter.WriteRtfToResponse();
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
        #endregion

        #region CRM Summary grid
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string IsCRMSummaryFilter = Convert.ToString(hfIsCRMSummaryFilter.Value);
            Session["IsCRMSummaryFilter"] = IsCRMSummaryFilter;
            DateTime dtTo;
            dtTo = Convert.ToDateTime(ASPxToDate.Date);
            string TODATE = dtTo.ToString("yyyy-MM-dd");

            Task PopulateStockTrialDataTask = new Task(() => GetCRMSummarydata(TODATE));
            PopulateStockTrialDataTask.RunSynchronously();
        }
        public void GetCRMSummarydata(string TODATE)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_CRMSUMMARY_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@ASONDATE", TODATE);
                cmd.Parameters.AddWithValue("@SALESMAN_CODE", hdnSalesManAgentId.Value);
                cmd.Parameters.AddWithValue("@PARTY_CODE", hdnCustomerId.Value);
                cmd.Parameters.AddWithValue("@PRODUCT_ID", hdncWiseProductId.Value);
                cmd.Parameters.AddWithValue("@STATUS", ddlstatus.SelectedValue);
                cmd.Parameters.AddWithValue("@USERID", Convert.ToInt32(Session["userid"]));

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);

                cmd.Dispose();
                con.Dispose();

            }
            catch (Exception ex)
            {

            }
        }

        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            if (Convert.ToString(Session["IsCRMSummaryFilter"]) == "Y")
            {
                dtCumSalesTillDtPrcn = oDBEngine.GetDataTable("SELECT CAST((SUM(CUMULATIVE_SALES)/SUM(CASE WHEN QTY_CURRENTFY<>0 THEN QTY_CURRENTFY ELSE QTY_CURRENTFY END))*100 AS DECIMAL(18,2)) FROM CRMSUMMARY_REPORT WHERE USERID=" + Convert.ToInt32(Session["userid"]));
                if (dtCumSalesTillDtPrcn.Rows.Count > 0)
                {
                    CumSalesTillDtPrcn = Convert.ToDecimal((dtCumSalesTillDtPrcn.Rows[0][0].ToString()) == "" ? "0" : (dtCumSalesTillDtPrcn.Rows[0][0].ToString()));
                }                

                dtMonthWiseSalesPrcn = oDBEngine.GetDataTable("SELECT CAST((SUM(APRIL_CS)/SUM(CASE WHEN MONTHLY_BUDGET<>0 THEN MONTHLY_BUDGET ELSE MONTHLY_BUDGET END))*100 AS DECIMAL(18,2)) FROM CRMSUMMARY_REPORT WHERE USERID=" + Convert.ToInt32(Session["userid"]));
                if (dtMonthWiseSalesPrcn.Rows.Count > 0)
                {
                    AprilSalesPrcn = Convert.ToDecimal((dtMonthWiseSalesPrcn.Rows[0][0].ToString()) == "" ? "0" : (dtMonthWiseSalesPrcn.Rows[0][0].ToString()));
                }
                dtMonthWiseSalesPrcn = null;
                
                dtMonthWiseSalesPrcn = oDBEngine.GetDataTable("SELECT CAST((SUM(MAY_CS)/SUM(CASE WHEN MONTHLY_BUDGET<>0 THEN MONTHLY_BUDGET ELSE MONTHLY_BUDGET END))*100 AS DECIMAL(18,2)) FROM CRMSUMMARY_REPORT WHERE USERID=" + Convert.ToInt32(Session["userid"]));
                if (dtMonthWiseSalesPrcn.Rows.Count > 0)
                {
                    MaySalesPrcn = Convert.ToDecimal((dtMonthWiseSalesPrcn.Rows[0][0].ToString()) == "" ? "0" : (dtMonthWiseSalesPrcn.Rows[0][0].ToString()));
                }
                dtMonthWiseSalesPrcn = null;

                dtMonthWiseSalesPrcn = oDBEngine.GetDataTable("SELECT CAST((SUM(JUNE_CS)/SUM(CASE WHEN MONTHLY_BUDGET<>0 THEN MONTHLY_BUDGET ELSE MONTHLY_BUDGET END))*100 AS DECIMAL(18,2)) FROM CRMSUMMARY_REPORT WHERE USERID=" + Convert.ToInt32(Session["userid"]));
                if (dtMonthWiseSalesPrcn.Rows.Count > 0)
                {
                    JuneSalesPrcn = Convert.ToDecimal((dtMonthWiseSalesPrcn.Rows[0][0].ToString()) == "" ? "0" : (dtMonthWiseSalesPrcn.Rows[0][0].ToString()));
                }
                dtMonthWiseSalesPrcn = null;

                dtMonthWiseSalesPrcn = oDBEngine.GetDataTable("SELECT CAST((SUM(JULY_CS)/SUM(CASE WHEN MONTHLY_BUDGET<>0 THEN MONTHLY_BUDGET ELSE MONTHLY_BUDGET END))*100 AS DECIMAL(18,2)) FROM CRMSUMMARY_REPORT WHERE USERID=" + Convert.ToInt32(Session["userid"]));
                if (dtMonthWiseSalesPrcn.Rows.Count > 0)
                {
                    JulySalesPrcn = Convert.ToDecimal((dtMonthWiseSalesPrcn.Rows[0][0].ToString()) == "" ? "0" : (dtMonthWiseSalesPrcn.Rows[0][0].ToString()));
                }
                dtMonthWiseSalesPrcn = null;

                dtMonthWiseSalesPrcn = oDBEngine.GetDataTable("SELECT CAST((SUM(AUGUST_CS)/SUM(CASE WHEN MONTHLY_BUDGET<>0 THEN MONTHLY_BUDGET ELSE MONTHLY_BUDGET END))*100 AS DECIMAL(18,2)) FROM CRMSUMMARY_REPORT WHERE USERID=" + Convert.ToInt32(Session["userid"]));
                if (dtMonthWiseSalesPrcn.Rows.Count > 0)
                {
                    AugustSalesPrcn = Convert.ToDecimal((dtMonthWiseSalesPrcn.Rows[0][0].ToString()) == "" ? "0" : (dtMonthWiseSalesPrcn.Rows[0][0].ToString()));
                }
                dtMonthWiseSalesPrcn = null;

                dtMonthWiseSalesPrcn = oDBEngine.GetDataTable("SELECT CAST((SUM(SEPTEMBER_CS)/SUM(CASE WHEN MONTHLY_BUDGET<>0 THEN MONTHLY_BUDGET ELSE MONTHLY_BUDGET END))*100 AS DECIMAL(18,2)) FROM CRMSUMMARY_REPORT WHERE USERID=" + Convert.ToInt32(Session["userid"]));
                if (dtMonthWiseSalesPrcn.Rows.Count > 0)
                {
                    SeptemberSalesPrcn = Convert.ToDecimal((dtMonthWiseSalesPrcn.Rows[0][0].ToString()) == "" ? "0" : (dtMonthWiseSalesPrcn.Rows[0][0].ToString()));
                }
                dtMonthWiseSalesPrcn = null;

                dtMonthWiseSalesPrcn = oDBEngine.GetDataTable("SELECT CAST((SUM(OCTOBER_CS)/SUM(CASE WHEN MONTHLY_BUDGET<>0 THEN MONTHLY_BUDGET ELSE MONTHLY_BUDGET END))*100 AS DECIMAL(18,2)) FROM CRMSUMMARY_REPORT WHERE USERID=" + Convert.ToInt32(Session["userid"]));
                if (dtMonthWiseSalesPrcn.Rows.Count > 0)
                {
                    OctoberSalesPrcn = Convert.ToDecimal((dtMonthWiseSalesPrcn.Rows[0][0].ToString()) == "" ? "0" : (dtMonthWiseSalesPrcn.Rows[0][0].ToString()));
                }
                dtMonthWiseSalesPrcn = null;

                dtMonthWiseSalesPrcn = oDBEngine.GetDataTable("SELECT CAST((SUM(NOVEMBER_CS)/SUM(CASE WHEN MONTHLY_BUDGET<>0 THEN MONTHLY_BUDGET ELSE MONTHLY_BUDGET END))*100 AS DECIMAL(18,2)) FROM CRMSUMMARY_REPORT WHERE USERID=" + Convert.ToInt32(Session["userid"]));
                if (dtMonthWiseSalesPrcn.Rows.Count > 0)
                {
                    NovemberSalesPrcn = Convert.ToDecimal((dtMonthWiseSalesPrcn.Rows[0][0].ToString()) == "" ? "0" : (dtMonthWiseSalesPrcn.Rows[0][0].ToString()));
                }
                dtMonthWiseSalesPrcn = null;

                dtMonthWiseSalesPrcn = oDBEngine.GetDataTable("SELECT CAST((SUM(DECEMBER_CS)/SUM(CASE WHEN MONTHLY_BUDGET<>0 THEN MONTHLY_BUDGET ELSE MONTHLY_BUDGET END))*100 AS DECIMAL(18,2)) FROM CRMSUMMARY_REPORT WHERE USERID=" + Convert.ToInt32(Session["userid"]));
                if (dtMonthWiseSalesPrcn.Rows.Count > 0)
                {
                    DecemberSalesPrcn = Convert.ToDecimal((dtMonthWiseSalesPrcn.Rows[0][0].ToString()) == "" ? "0" : (dtMonthWiseSalesPrcn.Rows[0][0].ToString()));
                }
                dtMonthWiseSalesPrcn = null;

                dtMonthWiseSalesPrcn = oDBEngine.GetDataTable("SELECT CAST((SUM(JANUARY_CS)/SUM(CASE WHEN MONTHLY_BUDGET<>0 THEN MONTHLY_BUDGET ELSE MONTHLY_BUDGET END))*100 AS DECIMAL(18,2)) FROM CRMSUMMARY_REPORT WHERE USERID=" + Convert.ToInt32(Session["userid"]));
                if (dtMonthWiseSalesPrcn.Rows.Count > 0)
                {
                    JanuarySalesPrcn = Convert.ToDecimal((dtMonthWiseSalesPrcn.Rows[0][0].ToString()) == "" ? "0" : (dtMonthWiseSalesPrcn.Rows[0][0].ToString()));
                }
                dtMonthWiseSalesPrcn = null;

                dtMonthWiseSalesPrcn = oDBEngine.GetDataTable("SELECT CAST((SUM(FEBRUARY_CS)/SUM(CASE WHEN MONTHLY_BUDGET<>0 THEN MONTHLY_BUDGET ELSE MONTHLY_BUDGET END))*100 AS DECIMAL(18,2)) FROM CRMSUMMARY_REPORT WHERE USERID=" + Convert.ToInt32(Session["userid"]));
                if (dtMonthWiseSalesPrcn.Rows.Count > 0)
                {
                    FebruarySalesPrcn = Convert.ToDecimal((dtMonthWiseSalesPrcn.Rows[0][0].ToString()) == "" ? "0" : (dtMonthWiseSalesPrcn.Rows[0][0].ToString()));
                }
                dtMonthWiseSalesPrcn = null;

                dtMonthWiseSalesPrcn = oDBEngine.GetDataTable("SELECT CAST((SUM(MARCH_CS)/SUM(CASE WHEN MONTHLY_BUDGET<>0 THEN MONTHLY_BUDGET ELSE MONTHLY_BUDGET END))*100 AS DECIMAL(18,2)) FROM CRMSUMMARY_REPORT WHERE USERID=" + Convert.ToInt32(Session["userid"]));
                if (dtMonthWiseSalesPrcn.Rows.Count > 0)
                {
                    MarchSalesPrcn = Convert.ToDecimal((dtMonthWiseSalesPrcn.Rows[0][0].ToString()) == "" ? "0" : (dtMonthWiseSalesPrcn.Rows[0][0].ToString()));
                }
                dtMonthWiseSalesPrcn = null;

            }
            if (e.Item.FieldName == "PRODUCTS_NAME")
            {
                e.Text = "Total:";
            }
            else if (e.Item.FieldName == "CUMULATIVESALES_PERCENTAGE")
            {
                e.Text = CumSalesTillDtPrcn.ToString();
            }
            else if (e.Item.FieldName == "APRIL_PS")
            {
                e.Text = AprilSalesPrcn.ToString();
            }
            else if (e.Item.FieldName == "MAY_PS")
            {
                e.Text = MaySalesPrcn.ToString();
            }
            else if (e.Item.FieldName == "JUNE_PS")
            {
                e.Text = JuneSalesPrcn.ToString();
            }
            else if (e.Item.FieldName == "JULY_PS")
            {
                e.Text = JulySalesPrcn.ToString();
            }
            else if (e.Item.FieldName == "AUGUST_PS")
            {
                e.Text = AugustSalesPrcn.ToString();
            }
            else if (e.Item.FieldName == "SEPTEMBER_PS")
            {
                e.Text = SeptemberSalesPrcn.ToString();
            }
            else if (e.Item.FieldName == "OCTOBER_PS")
            {
                e.Text = OctoberSalesPrcn.ToString();
            }
            else if (e.Item.FieldName == "November_PS")
            {
                e.Text = NovemberSalesPrcn.ToString();
            }
            else if (e.Item.FieldName == "DECEMBER_PS")
            {
                e.Text = DecemberSalesPrcn.ToString();
            }
            else if (e.Item.FieldName == "JANUARY_PS")
            {
                e.Text = JanuarySalesPrcn.ToString();
            }
            else if (e.Item.FieldName == "FEBRUARY_PS")
            {
                e.Text = FebruarySalesPrcn.ToString();
            }
            else if (e.Item.FieldName == "MARCH_PS")
            {
                e.Text = MarchSalesPrcn.ToString();
            }
            else
            {
                e.Text = string.Format("{0}", Math.Abs(Convert.ToDecimal(e.Value)));
            }
        }

        #endregion

        #region LinQ
        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsCRMSummaryFilter"]) == "Y")
            {
                var q = from d in dc.CRMSUMMARY_REPORTs
                        where Convert.ToString(d.USERID) == Userid 
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.CRMSUMMARY_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
        }
        #endregion

        }    
}