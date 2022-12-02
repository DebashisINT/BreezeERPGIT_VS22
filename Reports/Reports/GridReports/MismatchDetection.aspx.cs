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
    public partial class MismatchDetection : System.Web.UI.Page
    {
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        CommonBL cbl = new CommonBL();

        ExcelFile objExcel = new ExcelFile();
        DataTable dtExport = new DataTable();
        DataTable dtReportHeader = new DataTable();
        DataTable dtReportFooter = new DataTable();

        DataTable dtMDTotal = null;
        string MDTotalBalDesc = "";
        string MDDr = "";
        string MDCr = "";
        string MDBal = "";

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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/MismatchDetection.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            if (!IsPostBack)
            {
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Mismatch Detection";
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

                Session["IsMismatchDectecFilter"] = null;

                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;

                ASPxFromDate.Value = DateTime.Now;
                ASPxToDate.Value = DateTime.Now;
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
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
            if (Convert.ToString(Session["IsMismatchDectecFilter"]) == "Y")
            {
                if (Filter != 0)
                {
                    bindexport(Filter);
                }
            }
        }

        public void bindexport(int Filter)
        {

            string filename = "MismatchDetection";
            exporter.FileName = filename;

            if (Filter == 1 || Filter == 2)
            {
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                con.Open();
                string selectQuery = "SELECT BRANCHNAME,TRAN_DATE,DOC_NO,DOC_TYPE,LEDGERDESC,DEBIT,CREDIT,BALANCE FROM LEDGERVALUEMISMATCH_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND SEQ<>999999999999 AND DOC_NO<>'Total :' order by SEQ";
                SqlDataAdapter myCommand = new SqlDataAdapter(selectQuery, con);

                // Create and fill a DataSet.
                DataSet ds = new DataSet();
                myCommand.Fill(ds, "Main");
                myCommand = new SqlDataAdapter("Select DOC_NO,DEBIT,CREDIT,BALANCE from LEDGERVALUEMISMATCH_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND SEQ=999999999999 AND DOC_NO='Total :'", con);
                myCommand.Fill(ds, "GrossTotal");
                myCommand.Dispose();
                con.Dispose();
                Session["exportmddataset"] = ds;

                dtExport = ds.Tables[0].Copy();
                dtExport.Clear();
                dtExport.Columns.Add(new DataColumn("Unit", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Transaction Date", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Document Number", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Document Type", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Ledger", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Debit", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Credit", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Balance", typeof(decimal)));

                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {
                    DataRow row2 = dtExport.NewRow();

                    row2["Unit"] = dr1["BRANCHNAME"];
                    row2["Transaction Date"] = dr1["TRAN_DATE"];
                    row2["Document Number"] = dr1["DOC_NO"];
                    row2["Document Type"] = dr1["DOC_TYPE"];
                    row2["Ledger"] = dr1["LEDGERDESC"];
                    row2["Debit"] = dr1["DEBIT"];
                    row2["Credit"] = dr1["CREDIT"];
                    row2["Balance"] = dr1["BALANCE"];

                    dtExport.Rows.Add(row2);
                }

                if (drplistofaction.SelectedValue == "BLANKBRANCH" || drplistofaction.SelectedValue == "BLANKMAINAC")
                    dtExport.Columns.Remove("Unit");
                if (drplistofaction.SelectedValue == "DRCRMISMATCH" || drplistofaction.SelectedValue == "BLANKBRANCH" || drplistofaction.SelectedValue == "BLANKMAINAC")
                {
                    dtExport.Columns.Remove("Debit");
                    dtExport.Columns.Remove("Credit");
                }

                dtExport.Columns.Remove("BRANCHNAME");
                dtExport.Columns.Remove("TRAN_DATE");
                dtExport.Columns.Remove("DOC_NO");
                dtExport.Columns.Remove("DOC_TYPE");
                dtExport.Columns.Remove("LEDGERDESC");
                dtExport.Columns.Remove("DEBIT");
                dtExport.Columns.Remove("CREDIT");
                dtExport.Columns.Remove("BALANCE");

                DataRow row3 = dtExport.NewRow();
                row3["Document Number"] = ds.Tables[1].Rows[0]["DOC_NO"].ToString();
                if (drplistofaction.SelectedValue == "ORPHANEREC")
                {
                    row3["Debit"] = ds.Tables[1].Rows[0]["DEBIT"].ToString();
                    row3["Credit"] = ds.Tables[1].Rows[0]["CREDIT"].ToString();
                }
                row3["Balance"] = ds.Tables[1].Rows[0]["BALANCE"].ToString();
                dtExport.Rows.Add(row3);

                //For Excel/PDF Header
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                dtReportHeader.Columns.Add(new DataColumn("Header", typeof(String)));

                string GridHeader = "";
                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, false, false, false, false, false);
                DataRow HeaderRow = dtReportHeader.NewRow();
                HeaderRow[0] = GridHeader.ToString();
                dtReportHeader.Rows.Add(HeaderRow);
                DataRow HeaderRow1 = dtReportHeader.NewRow();
                HeaderRow1[0] = Convert.ToString(Session["BranchNames"]);
                dtReportHeader.Rows.Add(HeaderRow1);
                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, true, false, false, false, false);
                DataRow HeaderRow2 = dtReportHeader.NewRow();
                HeaderRow2[0] = GridHeader.ToString();
                dtReportHeader.Rows.Add(HeaderRow2);
                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, true, false, false, false);
                DataRow HeaderRow3 = dtReportHeader.NewRow();
                HeaderRow3[0] = GridHeader.ToString();
                dtReportHeader.Rows.Add(HeaderRow3);
                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, false, true, false, false);
                DataRow HeaderRow4 = dtReportHeader.NewRow();
                HeaderRow4[0] = GridHeader.ToString();
                dtReportHeader.Rows.Add(HeaderRow4);
                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, false, false, false, true);
                DataRow HeaderRow5 = dtReportHeader.NewRow();
                HeaderRow5[0] = GridHeader.ToString();
                dtReportHeader.Rows.Add(HeaderRow5);
                DataRow HeaderRow6 = dtReportHeader.NewRow();
                HeaderRow6[0] = "Mismatch Detection";
                dtReportHeader.Rows.Add(HeaderRow6);
                DataRow HeaderRow7 = dtReportHeader.NewRow();
                HeaderRow7[0] = "For the period: " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
                dtReportHeader.Rows.Add(HeaderRow7);

                //For Excel/PDF Footer
                dtReportFooter.Columns.Add(new DataColumn("Footer", typeof(String))); //0
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
                    objExcel.ExportToExcelforExcel(dtExport, "MismatchDetection", "ZZZZZZZZZZZZZZZZ", "ZZZZZZZZZZZZZZZZ", dtReportHeader, dtReportFooter);
                    break;
                case 2:
                    objExcel.ExportToPDF(dtExport, "MismatchDetection", "ZZZZZZZZZZZZZZZZ", "ZZZZZZZZZZZZZZZZ", dtReportHeader, dtReportFooter);
                    break;
                case 3:
                    exporter.WriteCsvToResponse();
                    break;
                default:
                    return;
            }

        }

        #endregion

        #region Mismatch Detection grid
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string IsMismatchDectecFilter = Convert.ToString(hfIsMismatchDectecFilter.Value);
            Session["IsMismatchDectecFilter"] = IsMismatchDectecFilter;

            DateTime dtFrom;
            DateTime dtTo;

            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);

            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");

            Task PopulateStockTrialDataTask = new Task(() => GetMismatchDectecdata(FROMDATE, TODATE));
            PopulateStockTrialDataTask.RunSynchronously();
        }
        public void GetMismatchDectecdata(string FROMDATE, string TODATE)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_LEDGERVALUEMISMATCH_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                cmd.Parameters.AddWithValue("@TODATE", TODATE);
                cmd.Parameters.AddWithValue("@ACTION ", drplistofaction.SelectedValue);
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
            if (Convert.ToString(Session["IsMismatchDectecFilter"]) == "Y")
            {
                dtMDTotal = oDBEngine.GetDataTable("Select DOC_NO,DEBIT,CREDIT,BALANCE from LEDGERVALUEMISMATCH_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND SEQ=999999999999 AND DOC_NO='Total :'");
                if (dtMDTotal.Rows.Count > 0)
                {
                    MDTotalBalDesc = dtMDTotal.Rows[0][0].ToString();
                    MDDr = dtMDTotal.Rows[0][1].ToString();
                    MDCr = dtMDTotal.Rows[0][2].ToString();
                    MDBal = dtMDTotal.Rows[0][3].ToString();
                }
            }

            if (Convert.ToString(Session["IsMismatchDectecFilter"]) == "Y")
            {
                if (dtMDTotal.Rows.Count > 0)
                {
                    string summaryTAG = (e.Item as ASPxSummaryItem).Tag;
                    if (e.IsTotalSummary == true)
                    {
                        switch (summaryTAG)
                        {
                            case "TagDoc":
                                e.Text = MDTotalBalDesc;
                                break;
                            case "TagDr":
                                e.Text = MDDr;
                                break;
                            case "TagCr":
                                e.Text = MDCr;
                                break;
                            case "TagBal":
                                e.Text = MDBal;
                                break;
                        }
                    }
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

            if (Convert.ToString(Session["IsMismatchDectecFilter"]) == "Y")
            {
                var q = from d in dc.LEDGERVALUEMISMATCH_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.SEQ) != "999999999999" && Convert.ToString(d.DOC_NO) != "Total :"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.LEDGERVALUEMISMATCH_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }

            if (drplistofaction.SelectedValue == "DRCRMISMATCH" || drplistofaction.SelectedValue == "ORPHANEREC")
            {
                ShowGrid.Columns[0].Visible = true;
            }
            else
            {
                ShowGrid.Columns[0].Visible = false;
            }

            if(drplistofaction.SelectedValue == "ORPHANEREC")
            {
                ShowGrid.Columns[4].Visible = true;
                ShowGrid.Columns[5].Visible = true;
                ShowGrid.Columns[6].Visible = true;
            }
            else
            {
                ShowGrid.Columns[4].Visible = false;
                ShowGrid.Columns[5].Visible = false;
                ShowGrid.Columns[6].Visible = false;
            }
        }

        #endregion

        public void Date_finyearwise(string Finyear)
        {
            CommonBL salereg = new CommonBL();
            DataTable dtsalereg = new DataTable();

            dtsalereg = salereg.GetDateFinancila(Finyear);
            if (dtsalereg.Rows.Count > 0)
            {
                ASPxFromDate.MaxDate = Convert.ToDateTime((dtsalereg.Rows[0]["FinYear_EndDate"]));
                ASPxFromDate.MinDate = Convert.ToDateTime((dtsalereg.Rows[0]["FinYear_StartDate"]));

                ASPxToDate.MaxDate = Convert.ToDateTime((dtsalereg.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((dtsalereg.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((dtsalereg.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((dtsalereg.Rows[0]["FinYear_StartDate"]));

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
    }
}