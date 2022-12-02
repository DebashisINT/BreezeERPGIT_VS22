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
using System.Threading.Tasks;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using System.Drawing;
using Reports.Model;
using DataAccessLayer;

namespace Reports.Reports.GridReports
{
    public partial class LeadActivity : System.Web.UI.Page
    {
        DataTable DTIndustry = new DataTable();
        DateTime dtFrom;
        DateTime dtTo;
        TotalvaluationClass objvaluation = new TotalvaluationClass();
        string data = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Reports/master/LeadActivity.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            if (!IsPostBack)
            {
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Lead Activity";
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

                Session["SI_ComponentData"] = null;
                Session["IsLeadActivityFilter"] = null;
                Session["SI_ComponentData_Branch"] = null;
                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                ASPxFromDate.Text = dtFrom.ToString("dd-MM-yyyy");
                ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");
                ASPxFromDate.Value = DateTime.Now;
                ASPxToDate.Value = DateTime.Now;

                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                string TODATE = dtTo.ToString("yyyy-MM-dd");
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));

            }
            else
            {
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);
            }
        }
        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {

            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

      

        #region Export Valuation Summary
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
            string filename = "LeadActivity";
            exporter.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Lead Activity Report" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");

            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";

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

        #endregion

        #region =======================Assigned Salesman Details =========================
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

            ShowGrid.JSProperties["cpSave"] = null;

            string IsLeadActivityFilter = Convert.ToString(hfLeadActivityFilter.Value);
            Session["IsLeadActivityFilter"] = IsLeadActivityFilter;

            string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);

            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");

            string  strCustList = "";
          
            strCustList = hdnUserId.Value;

            Task PopulateStockTrialDataTask = new Task(() => GetLeadActivitydata(FROMDATE, TODATE,strCustList));
            PopulateStockTrialDataTask.RunSynchronously();

        }

        public void GetLeadActivitydata(string FROMDATE, string TODATE,string USERID)
        {
            try
            {
                DataTable ds = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("PRC_LEAD_ACTIVITY_REPORT");
                proc.AddPara("@TODATE", TODATE);
                proc.AddPara("@FROMDATE", FROMDATE);
                proc.AddPara("@PARTY_CODE", USERID);
                proc.AddPara("@USERID", Convert.ToInt32(Session["userid"]));

                ds = proc.GetTable();

            }
            catch (Exception ex)
            {
            }
        }

        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }
        #endregion



        public void Date_finyearwise(string Finyear)
        {

            CommonBL stkval = new CommonBL();
            DataTable dtstkval = new DataTable();

            dtstkval = stkval.GetDateFinancila(Finyear);
            if (dtstkval.Rows.Count > 0)
            {
                ASPxFromDate.MaxDate = Convert.ToDateTime((dtstkval.Rows[0]["FinYear_EndDate"]));
                //Rev 1.0  v1.0.98  0018820  commented by subhra  ---start
                //ASPxFromDate.MinDate = Convert.ToDateTime((dtstkval.Rows[0]["FinYear_StartDate"]));
                //---end

                ASPxToDate.MaxDate = Convert.ToDateTime((dtstkval.Rows[0]["FinYear_EndDate"]));
                //Rev 1.0  v1.0.98  0018820  commented by subhra  ---start
                //ASPxToDate.MinDate = Convert.ToDateTime((dtstkval.Rows[0]["FinYear_StartDate"]));
                //---end

                DateTime MaximumDate = Convert.ToDateTime((dtstkval.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((dtstkval.Rows[0]["FinYear_StartDate"]));

                DateTime TodayDate = Convert.ToDateTime(DateTime.Now);
                DateTime FinYearEndDate = MaximumDate;

                if (TodayDate > FinYearEndDate)
                {
                    ASPxToDate.Date = FinYearEndDate;
                    //ASPxFromDate.Date = MinimumDate;
                }
                else
                {
                    ASPxToDate.Date = TodayDate;
                    //ASPxFromDate.Date = MinimumDate;
                }

            }

        }

        #region LinQ

        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SLNO";

            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsLeadActivityFilter"]) == "Y")
            {
                var q = from d in dc.LEAD_ACTIVITY_REPORTs
                        where Convert.ToString(d.USERID) == Userid
                        orderby d.SLNO
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.LEAD_ACTIVITY_REPORTs
                        where Convert.ToString(d.SLNO) == "0"
                        orderby d.SLNO
                        select d;
                e.QueryableSource = q;
            }
        }
        #endregion
    }
}