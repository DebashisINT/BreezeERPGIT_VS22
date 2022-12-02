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

namespace Reports.Reports.GridReports
{
    public partial class DSROnOpportunities : System.Web.UI.Page
    {
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        ExcelFile objExcel = new ExcelFile();
        DataTable dtExport = new DataTable();
        DataTable dtReportHeader = new DataTable();
        DataTable dtReportFooter = new DataTable();

        DataTable dtDSRTotal = null;
        string DSRTotalBalDesc = "";
        string DSRBudget = "";
        string DSRBudgetMnth = "";
        string DSRCloseQty = "";

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
            rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/DSROnOpportunities.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            if (!IsPostBack)
            {
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "DSR On Opportunities";
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

                Session["IsDSRONOPPORTUNITYDetFilter"] = null;

                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;

                ASPxFromDate.Value = DateTime.Now;
                ASPxToDate.Value = DateTime.Now;
                //Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
            }
            else
            {
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);
            }


            if (!IsPostBack)
            {
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);
            }
        }

        #region Export
        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Convert.ToString(Session["IsDSRONOPPORTUNITYDetFilter"]) == "Y")
            {
                if (Filter != 0)
                {
                    bindexport(Filter);
                }
            }
        }

        public void bindexport(int Filter)
        {
            string filename = "";
            filename = "DSROnOpportunities";
            exporter.FileName = filename;

            if (Filter == 1 || Filter == 2)
            {
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                con.Open();
                string selectQuery = "SELECT CUSTNAME,MULTIPLEPRODUCTCLASSNAME,ASSIGNED_TO,INDUSTRY,BUDGET,BUDGET_MONTHWISE,CLOSE_REASON,CLOSED_DATE,CLOSE_QTY,CLOSE_REMARKS,REOPEN_DATE,REOPEN_FEEDBACK,ORDERSTATUS FROM DSRONOPPORTUNITIES_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND CUSTNAME<>'Total :' order by SEQ";
                SqlDataAdapter myCommand = new SqlDataAdapter(selectQuery, con);

                // Create and fill a DataSet.
                DataSet ds = new DataSet();
                myCommand.Fill(ds, "Main");
                myCommand = new SqlDataAdapter("Select CUSTNAME,BUDGET,BUDGET_MONTHWISE,CLOSE_QTY from DSRONOPPORTUNITIES_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND CUSTNAME='Total :'", con);
                myCommand.Fill(ds, "GrossTotal");
                myCommand.Dispose();
                con.Dispose();
                Session["exportdsronopprdataset"] = ds;

                dtExport = ds.Tables[0].Copy();
                dtExport.Clear();
                dtExport.Columns.Add(new DataColumn("Customer/Lead", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Product Class", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Salesman", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Industry", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Product:Budget", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Monthly Budget", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Reason for Close", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Close Date", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Quantity", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Remarks of Close", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Reopen Date", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Feedback for Reopen", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Status", typeof(string)));

                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {
                    DataRow row2 = dtExport.NewRow();

                    row2["Customer/Lead"] = dr1["CUSTNAME"];
                    row2["Product Class"] = dr1["MULTIPLEPRODUCTCLASSNAME"];
                    row2["Salesman"] = dr1["ASSIGNED_TO"];
                    row2["Industry"] = dr1["INDUSTRY"];
                    row2["Product:Budget"] = dr1["BUDGET"];
                    row2["Monthly Budget"] = dr1["BUDGET_MONTHWISE"];
                    row2["Reason for Close"] = dr1["CLOSE_REASON"];
                    row2["Close Date"] = dr1["CLOSED_DATE"];
                    row2["Quantity"] = dr1["CLOSE_QTY"];
                    row2["Remarks of Close"] = dr1["CLOSE_REMARKS"];
                    row2["Reopen Date"] = dr1["REOPEN_DATE"];
                    row2["Feedback for Reopen"] = dr1["REOPEN_FEEDBACK"];
                    row2["Status"] = dr1["ORDERSTATUS"];

                    dtExport.Rows.Add(row2);
                }

                dtExport.Columns.Remove("CUSTNAME");
                dtExport.Columns.Remove("MULTIPLEPRODUCTCLASSNAME");
                dtExport.Columns.Remove("ASSIGNED_TO");
                dtExport.Columns.Remove("INDUSTRY");
                dtExport.Columns.Remove("BUDGET");
                dtExport.Columns.Remove("BUDGET_MONTHWISE");
                dtExport.Columns.Remove("CLOSE_REASON");
                dtExport.Columns.Remove("CLOSED_DATE");
                dtExport.Columns.Remove("CLOSE_QTY");
                dtExport.Columns.Remove("CLOSE_REMARKS");
                dtExport.Columns.Remove("REOPEN_DATE");
                dtExport.Columns.Remove("REOPEN_FEEDBACK");
                dtExport.Columns.Remove("ORDERSTATUS");

                DataRow row3 = dtExport.NewRow();
                row3["Customer/Lead"] = ds.Tables[1].Rows[0]["CUSTNAME"].ToString();
                row3["Product:Budget"] = ds.Tables[1].Rows[0]["BUDGET"].ToString();
                row3["Monthly Budget"] = ds.Tables[1].Rows[0]["BUDGET_MONTHWISE"].ToString();
                row3["Quantity"] = ds.Tables[1].Rows[0]["CLOSE_QTY"].ToString();
                dtExport.Rows.Add(row3);

                //For Excel/PDF Header
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String)));

                string GridHeader = "";
                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, false, false, false, false, false);
                DataRow HeaderRow = dtReportHeader.NewRow();
                HeaderRow[0] = GridHeader.ToString();
                dtReportHeader.Rows.Add(HeaderRow);
                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, true, false, false, false, false);
                DataRow HeaderRow1 = dtReportHeader.NewRow();
                HeaderRow1[0] = GridHeader.ToString();
                dtReportHeader.Rows.Add(HeaderRow1);
                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, true, false, false, false);
                DataRow HeaderRow2 = dtReportHeader.NewRow();
                HeaderRow2[0] = GridHeader.ToString();
                dtReportHeader.Rows.Add(HeaderRow2);
                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, false, true, false, false);
                DataRow HeaderRow3 = dtReportHeader.NewRow();
                HeaderRow3[0] = GridHeader.ToString();
                dtReportHeader.Rows.Add(HeaderRow3);
                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, false, false, false, true);
                DataRow HeaderRow4 = dtReportHeader.NewRow();
                HeaderRow4[0] = GridHeader.ToString();
                dtReportHeader.Rows.Add(HeaderRow4);
                DataRow HeaderRow5 = dtReportHeader.NewRow();
                HeaderRow5[0] = "DSR On Opportunities";
                dtReportHeader.Rows.Add(HeaderRow5);
                DataRow HeaderRow6 = dtReportHeader.NewRow();
                HeaderRow6[0] = "For the period: " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
                dtReportHeader.Rows.Add(HeaderRow6);

                //For Excel/PDF Footer
                dtReportFooter.Columns.Add(new DataColumn("Footer", typeof(String)));
                DataRow FooterRow1 = dtReportFooter.NewRow();
                dtReportFooter.Rows.Add(FooterRow1);
                DataRow FooterRow2 = dtReportFooter.NewRow();
                dtReportFooter.Rows.Add(FooterRow2);
                DataRow FooterRow = dtReportFooter.NewRow();
                FooterRow[0] = "* * *  End Of Report * * *   ";
                dtReportFooter.Rows.Add(FooterRow);
            }
            else
            {
                exporter.PageHeader.Font.Size = 10;
                exporter.PageHeader.Font.Name = "Tahoma";
                exporter.GridViewID = "ShowGrid";
            }
            switch (Filter)
            {
                case 1:
                    objExcel.ExportToExcelforExcel(dtExport, "DSROnOpportunities", "ZZZZZZZZZZZZZZZZ", "ZZZZZZZZZZZZZZZZ", dtReportHeader, dtReportFooter);
                    break;
                case 2:
                    objExcel.ExportToPDF(dtExport, "DSROnOpportunities", "ZZZZZZZZZZZZZZZZ", "ZZZZZZZZZZZZZZZZ", dtReportHeader, dtReportFooter);
                    break;
                case 3:
                    exporter.WriteCsvToResponse();
                    break;
                default:
                    return;
            }
        }

        #endregion
        

        #region DSR On Opportunity grid
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string returnPara = Convert.ToString(e.Parameter);

            string IsDSRONOPPORTUNITYDetFilter = Convert.ToString(hfIsDSRONOPPORTUNITYDetFilter.Value);
            Session["IsDSRONOPPORTUNITYDetFilter"] = IsDSRONOPPORTUNITYDetFilter;
            string cnt_internalId = Convert.ToString(HttpContext.Current.Session["owninternalid"]);

            DateTime dtFrom;
            DateTime dtTo;

            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);

            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");

            Task PopulateStockTrialDataTask = new Task(() => GetDSROnOpportunitydata(cnt_internalId,FROMDATE, TODATE));
            PopulateStockTrialDataTask.RunSynchronously();
        }
        public void GetDSROnOpportunitydata(string cnt_internalId, string FROMDATE, string TODATE)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_DSRONOPPORTUNITIES_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CNT_INTERNALID", cnt_internalId);
                cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                cmd.Parameters.AddWithValue("@TODATE", TODATE);
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
            if (Convert.ToString(Session["IsDSRONOPPORTUNITYDetFilter"]) == "Y")
            {
                dtDSRTotal = oDBEngine.GetDataTable("Select CUSTNAME,BUDGET,BUDGET_MONTHWISE,CLOSE_QTY from DSRONOPPORTUNITIES_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND CUSTNAME='Total :'");
                if (dtDSRTotal.Rows.Count > 0)
                {
                    DSRTotalBalDesc = dtDSRTotal.Rows[0][0].ToString();
                    DSRBudget = dtDSRTotal.Rows[0][1].ToString();
                    DSRBudgetMnth = dtDSRTotal.Rows[0][2].ToString();
                    DSRCloseQty = dtDSRTotal.Rows[0][3].ToString();
                }
            }

            string summaryTAG = (e.Item as ASPxSummaryItem).Tag;
            if (e.IsTotalSummary == true)
            {
                switch (summaryTAG)
                {
                    case "Custname":
                        e.Text = DSRTotalBalDesc;
                        break;
                    case "Budget":
                        e.Text = DSRBudget;
                        break;
                    case "BudgetMnth":
                        e.Text = DSRBudgetMnth;
                        break;
                    case "CloseQty":
                        e.Text = DSRCloseQty;
                        break;
                }
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

            if (Convert.ToString(Session["IsDSRONOPPORTUNITYDetFilter"]) == "Y")
            {
                var q = from d in dc.DSRONOPPORTUNITIES_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.CUSTNAME) != "Total :"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.DSRONOPPORTUNITIES_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
        }

        #endregion

        //public void Date_finyearwise(string Finyear)
        //{
        //    CommonBL salereg = new CommonBL();
        //    DataTable dtsalereg = new DataTable();

        //    dtsalereg = salereg.GetDateFinancila(Finyear);
        //    if (dtsalereg.Rows.Count > 0)
        //    {
        //        ASPxFromDate.MaxDate = Convert.ToDateTime((dtsalereg.Rows[0]["FinYear_EndDate"]));
        //        ASPxFromDate.MinDate = Convert.ToDateTime((dtsalereg.Rows[0]["FinYear_StartDate"]));

        //        ASPxToDate.MaxDate = Convert.ToDateTime((dtsalereg.Rows[0]["FinYear_EndDate"]));
        //        ASPxToDate.MinDate = Convert.ToDateTime((dtsalereg.Rows[0]["FinYear_StartDate"]));

        //        DateTime MaximumDate = Convert.ToDateTime((dtsalereg.Rows[0]["FinYear_EndDate"]));
        //        DateTime MinimumDate = Convert.ToDateTime((dtsalereg.Rows[0]["FinYear_StartDate"]));

        //        DateTime TodayDate = Convert.ToDateTime(DateTime.Now);
        //        DateTime FinYearEndDate = MaximumDate;

        //        if (TodayDate > FinYearEndDate)
        //        {
        //            ASPxToDate.Date = FinYearEndDate;
        //            ASPxFromDate.Date = MinimumDate;
        //        }
        //        else
        //        {
        //            ASPxToDate.Date = TodayDate;
        //            ASPxFromDate.Date = MinimumDate;
        //        }

        //    }
        //}
    }
}