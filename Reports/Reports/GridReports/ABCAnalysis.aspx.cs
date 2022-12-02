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
    public partial class ABCAnalysis : System.Web.UI.Page
    {

        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        decimal TotalProduct = 0;
        decimal TotalA = 0;
        decimal TotalB = 0;
        decimal TotalC = 0;
        decimal TotalAPercentage=0;
        decimal TotalBPercentage = 0;
        decimal TotalCPercentage = 0;
        string TotalAPercentageString = "";
        string TotalBPercentageString = "";
        string TotalCPercentageString = "";

        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
            if (Request.QueryString.AllKeys.Contains("dashboard"))
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/PopUp.Master";
                //divcross.Visible = false;
            }
            else
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/ERP.Master";
                //divcross.Visible = true;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/ABCAnalysis.aspx");
            if (!IsPostBack)
            {
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "ABC Analysis";
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
                Session["IsABCAnalysis"] = null;
                ShowGrid.Columns[6].Visible = false;
                ShowGrid.Columns[7].Visible = false;
                ShowGrid.Columns[12].Visible = false;
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
        #region Grid Details

        protected void CallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            string IsABCAnalysis = Convert.ToString(hfIsABCAnalysis.Value);
            Session["IsABCAnalysis"] = IsABCAnalysis;

            Task PopulateStockTrialDataTask = new Task(() => GetABCAnalysis());
            PopulateStockTrialDataTask.RunSynchronously();
        }
        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }

        #endregion

        #region Export Details

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
            string filename = "ABCAnalysis_Report";
            exporter.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "ABC Analysis" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
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
                    exporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 2:
                    exporter.WritePdfToResponse();
                    break;
                case 3:
                    exporter.WriteCsvToResponse();
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

        #region Database Details

        public void GetABCAnalysis()
        {
            string strFromDate = Convert.ToDateTime(ASPxFromDate.Value).ToString("yyyy-MM-dd");
            string strToDate = Convert.ToDateTime(ASPxToDate.Value).ToString("yyyy-MM-dd");

            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                SqlCommand cmd = new SqlCommand("PRC_ABCANALYSIS_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@FROMDATE", strFromDate);
                cmd.Parameters.AddWithValue("@TODATE", strToDate);
                cmd.Parameters.AddWithValue("@CLASS", hdnClassId.Value);
                cmd.Parameters.AddWithValue("@CATEGORY", hdnBranndId.Value);
                cmd.Parameters.AddWithValue("@INDICATORA", Convert.ToDecimal(txtIndicatorA.Text));
                cmd.Parameters.AddWithValue("@INDICATORB", Convert.ToDecimal(txtIndicatorB.Text));
                cmd.Parameters.AddWithValue("@INDICATORC", Convert.ToDecimal(txtIndicatorC.Text));
                cmd.Parameters.AddWithValue("@CALCULATEON", ddlOnCriteria.SelectedValue);
                cmd.Parameters.AddWithValue("@VALTECH", ddlValTech.SelectedValue);
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

            if (Convert.ToString(Session["IsABCAnalysis"]) == "Y")
            {
                var q = from d in dc.ABCANALYSIS_REPORTs
                        where Convert.ToString(d.USERID) == Userid
                        orderby d.SEQ ascending
                        select d;
                e.QueryableSource = q;

                TotalProduct = (from d in dc.ABCANALYSIS_REPORTs
                                where Convert.ToString(d.USERID) == Userid
                                orderby d.SEQ
                                select d.SEQ).Count();
               
                if(ddlOnCriteria.SelectedValue=="S")
                {
                    TotalA = (from d in dc.ABCANALYSIS_REPORTs
                                    where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.ABCONSALEVALUE)=="A"
                                    orderby d.SEQ
                              select d.ABCONSALEVALUE).Count();                    
                    
                    TotalB = (from d in dc.ABCANALYSIS_REPORTs
                              where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.ABCONSALEVALUE) == "B"
                              orderby d.SEQ
                              select d.ABCONSALEVALUE).Count();                    

                    TotalC = (from d in dc.ABCANALYSIS_REPORTs
                              where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.ABCONSALEVALUE) == "C"
                              orderby d.SEQ
                              select d.ABCONSALEVALUE).Count();
                }
                else if (ddlOnCriteria.SelectedValue == "C")
                {
                    TotalA = (from d in dc.ABCANALYSIS_REPORTs
                              where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.ABCONCOSTVALUE) == "A"
                              orderby d.SEQ
                              select d.ABCONCOSTVALUE).Count();

                    TotalB = (from d in dc.ABCANALYSIS_REPORTs
                              where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.ABCONCOSTVALUE) == "B"
                              orderby d.SEQ
                              select d.ABCONCOSTVALUE).Count();

                    TotalC = (from d in dc.ABCANALYSIS_REPORTs
                              where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.ABCONCOSTVALUE) == "C"
                              orderby d.SEQ
                              select d.ABCONCOSTVALUE).Count();
                }
                TotalAPercentage = (TotalA / TotalProduct) * 100;
                TotalAPercentageString = TotalAPercentage.ToString("#.##");
                TotalBPercentage = (TotalB / TotalProduct) * 100;
                TotalBPercentageString = TotalBPercentage.ToString("#.##");
                TotalCPercentage = (TotalC / TotalProduct) * 100;
                TotalCPercentageString = TotalCPercentage.ToString("#.##");

                ShowGrid.JSProperties["cpTotalProduct"] = TotalProduct;
                ShowGrid.JSProperties["cpTotalA"] = Convert.ToString(TotalA) + "(" + Convert.ToString(TotalAPercentageString) + "%)";
                ShowGrid.JSProperties["cpTotalB"] = Convert.ToString(TotalB) + "(" + Convert.ToString(TotalBPercentageString) + "%)";
                ShowGrid.JSProperties["cpTotalC"] = Convert.ToString(TotalC) + "(" + Convert.ToString(TotalCPercentageString) + "%)";
            }
            else
            {
                var q = from d in dc.ABCANALYSIS_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ ascending
                        select d;
                e.QueryableSource = q;
            }

            if(ddlOnCriteria.SelectedValue=="S")
            {
                ShowGrid.Columns[6].Visible = false;
                ShowGrid.Columns[7].Visible = false;
                ShowGrid.Columns[11].Visible = true;
                ShowGrid.Columns[12].Visible = false;
            }
            else if (ddlOnCriteria.SelectedValue == "C")
            {
                ShowGrid.Columns[6].Visible = true;
                ShowGrid.Columns[7].Visible = true;
                ShowGrid.Columns[11].Visible = false;
                ShowGrid.Columns[12].Visible = true;                
            }
        }
        
        #endregion       
 
        protected void ShowGrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {

            if (Convert.ToString(e.CellValue) == "A")            
            {
                e.Cell.Font.Bold = true;
                e.Cell.BackColor = Color.Green;
            }

            if (Convert.ToString(e.CellValue) == "B")
            {
                e.Cell.Font.Bold = true;
                e.Cell.BackColor = Color.Yellow;
            }

            if (Convert.ToString(e.CellValue) == "C")
            {
                e.Cell.Font.Bold = true;
                e.Cell.BackColor = Color.Red;
            } 
        }
    }
}