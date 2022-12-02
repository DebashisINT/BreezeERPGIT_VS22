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
    public partial class SRVTechnicianProductivity : System.Web.UI.Page
    {
        DateTime dtFrom;
        DateTime dtTo;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        ExcelFile objExcel = new ExcelFile();
        DataTable dtExport = new DataTable();
        DataTable dtReportHeader = new DataTable();
        DataTable dtReportFooter = new DataTable();

        DataTable dtPartyTotal = null;
        string TPTotalDesc= "";
        string TPTotalTRSA = "";
        string TPTotalTRSR = "";
        string TPTotalTBSA = "";
        string TPTotalTBSR = "";
        string TPTotalOA = "";
        string TPTotalOR = "";
        string TPTotalWD = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/SRVTechnicianProductivity.aspx");
            if (!IsPostBack)
            {
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Technician Productivity";
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

                drdExport.SelectedIndex = 0;
                Session["IsSrvTechProdFilter"] = null;
                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                ASPxFromDate.Text = dtFrom.ToString("dd-MM-yyyy");
                ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");
                ASPxFromDate.Value = DateTime.Now;
                ASPxToDate.Value = DateTime.Now;

                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
            }
            else
            {
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);
            }
        }
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Request.QueryString.AllKeys.Contains("From"))
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/PopUp.Master";
            }
            else
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/ERP.Master";
            }
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        #region Export
        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Convert.ToString(Session["IsSrvTechProdFilter"]) == "Y")
            {
                if (Filter != 0)
                {
                    bindexport(Filter);
                }
            }
        }

        public void bindexport(int Filter)
        {
            string filename = "TechnicianProductivity";
            exporter.FileName = filename;

            if (Filter == 1 || Filter == 2)
            {
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                con.Open();
                string selectQuery = "SELECT PARTYNAME,TOTALRUNNINGSTBALLOCATED,TOTALRUNNINGSTBREPAIRED,TOTALBACKLOGSTBALLOCATED,TOTALBACKLOGSTBREPAIRED,OVERALLSTBALLOCATED,OVERALLREPAIRED,PERCNTOFREP,WORKINGDAYS,DAILYAVGBOXCHECKED,BOXCHECKINGGRADE,DAILYAVGBOXREPAIRED,PERFORMANCEGRADE FROM TECHNICIANPRODUCTIVITY_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND SEQ<>999999999999 AND PARTYNAME<>'Total :' order by SEQ";
                SqlDataAdapter myCommand = new SqlDataAdapter(selectQuery, con);

                // Create and fill a DataSet.
                DataSet ds = new DataSet();
                myCommand.Fill(ds, "Main");
                myCommand = new SqlDataAdapter("Select PARTYNAME,TOTALRUNNINGSTBALLOCATED,TOTALRUNNINGSTBREPAIRED,TOTALBACKLOGSTBALLOCATED,TOTALBACKLOGSTBREPAIRED,OVERALLSTBALLOCATED,OVERALLREPAIRED,WORKINGDAYS from TECHNICIANPRODUCTIVITY_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND SEQ=999999999999 AND PARTYNAME='Total :'", con);
                myCommand.Fill(ds, "GrossTotal");
                myCommand.Dispose();
                con.Dispose();
                Session["exporttpdataset"] = ds;

                dtExport = ds.Tables[0].Copy();
                dtExport.Clear();
                dtExport.Columns.Add(new DataColumn("Repaired By", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Total Running STB Allocated", typeof(Int32)));
                dtExport.Columns.Add(new DataColumn("Total Running STB Repaired", typeof(Int32)));
                dtExport.Columns.Add(new DataColumn("Total Backlog STB Allocated", typeof(Int32)));
                dtExport.Columns.Add(new DataColumn("Total Backlog STB Repaired", typeof(Int32)));
                dtExport.Columns.Add(new DataColumn("Overall STB Allocated", typeof(Int32)));
                dtExport.Columns.Add(new DataColumn("Overall Repaired", typeof(Int32)));
                dtExport.Columns.Add(new DataColumn("% of Repairing", typeof(Int32)));
                dtExport.Columns.Add(new DataColumn("WorkIng Days", typeof(Int32)));
                dtExport.Columns.Add(new DataColumn("Daily Avg. Box Checked", typeof(Int32)));
                dtExport.Columns.Add(new DataColumn("Box Checking Grade", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Daily Avg. Box Repaired", typeof(Int32)));
                dtExport.Columns.Add(new DataColumn("Performance Grade", typeof(string)));

                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {
                    DataRow row2 = dtExport.NewRow();

                    row2["Repaired By"] = dr1["PARTYNAME"];
                    row2["Total Running STB Allocated"] = dr1["TOTALRUNNINGSTBALLOCATED"];
                    row2["Total Running STB Repaired"] = dr1["TOTALRUNNINGSTBREPAIRED"];
                    row2["Total Backlog STB Allocated"] = dr1["TOTALBACKLOGSTBALLOCATED"];
                    row2["Total Backlog STB Repaired"] = dr1["TOTALBACKLOGSTBREPAIRED"];
                    row2["Overall STB Allocated"] = dr1["OVERALLSTBALLOCATED"];
                    row2["Overall Repaired"] = dr1["OVERALLREPAIRED"];
                    row2["% of Repairing"] = dr1["PERCNTOFREP"];
                    row2["WorkIng Days"] = dr1["WORKINGDAYS"];
                    row2["Daily Avg. Box Checked"] = dr1["DAILYAVGBOXCHECKED"];
                    row2["Box Checking Grade"] = dr1["BOXCHECKINGGRADE"];
                    row2["Daily Avg. Box Repaired"] = dr1["DAILYAVGBOXREPAIRED"];
                    row2["Performance Grade"] = dr1["PERFORMANCEGRADE"];

                    dtExport.Rows.Add(row2);
                }

                dtExport.Columns.Remove("PARTYNAME");
                dtExport.Columns.Remove("TOTALRUNNINGSTBALLOCATED");
                dtExport.Columns.Remove("TOTALRUNNINGSTBREPAIRED");
                dtExport.Columns.Remove("TOTALBACKLOGSTBALLOCATED");
                dtExport.Columns.Remove("TOTALBACKLOGSTBREPAIRED");
                dtExport.Columns.Remove("OVERALLSTBALLOCATED");
                dtExport.Columns.Remove("OVERALLREPAIRED");
                dtExport.Columns.Remove("PERCNTOFREP");
                dtExport.Columns.Remove("WORKINGDAYS");
                dtExport.Columns.Remove("DAILYAVGBOXCHECKED");
                dtExport.Columns.Remove("BOXCHECKINGGRADE");
                dtExport.Columns.Remove("DAILYAVGBOXREPAIRED");
                dtExport.Columns.Remove("PERFORMANCEGRADE");

                DataRow row3 = dtExport.NewRow();
                row3["Repaired By"] = ds.Tables[1].Rows[0]["PARTYNAME"].ToString();
                row3["Total Running STB Allocated"] = ds.Tables[1].Rows[0]["TOTALRUNNINGSTBALLOCATED"].ToString();
                row3["Total Running STB Repaired"] = ds.Tables[1].Rows[0]["TOTALRUNNINGSTBREPAIRED"].ToString();
                row3["Total Backlog STB Allocated"] = ds.Tables[1].Rows[0]["TOTALBACKLOGSTBALLOCATED"].ToString();
                row3["Total Backlog STB Repaired"] = ds.Tables[1].Rows[0]["TOTALBACKLOGSTBREPAIRED"].ToString();
                row3["Overall STB Allocated"] = ds.Tables[1].Rows[0]["OVERALLSTBALLOCATED"].ToString();
                row3["Overall Repaired"] = ds.Tables[1].Rows[0]["OVERALLREPAIRED"].ToString();
                row3["WorkIng Days"] = ds.Tables[1].Rows[0]["WORKINGDAYS"].ToString();
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
                HeaderRow6[0] = "Technician Productivity";
                dtReportHeader.Rows.Add(HeaderRow6);
                DataRow HeaderRow7 = dtReportHeader.NewRow();
                HeaderRow7[0] = "For the period: " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
                dtReportHeader.Rows.Add(HeaderRow7);
                DataRow HeaderRow8 = dtReportHeader.NewRow();
                HeaderRow8[0] = "Daily No. of Box Checked: " + "LEVEL A+>35.00; LEVEL A<=35.00; LEVEL B<=25.00; LEVEL C<20.00";
                dtReportHeader.Rows.Add(HeaderRow8);
                DataRow HeaderRow9 = dtReportHeader.NewRow();
                HeaderRow9[0] = "Performance Grade by Daily No. of Box Repaired: " + "LEVEL A+>20.00; LEVEL A<=20.00; LEVEL B<=15.00; LEVEL C<10.00";
                dtReportHeader.Rows.Add(HeaderRow9);

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
                    objExcel.ExportToExcelforCellBackColor(dtExport, "TechnicianProductivity", "ZZZZZZZZZZZZZZZZ", "ZZZZZZZZZZZZZZZZ", dtReportHeader, dtReportFooter);
                    break;
                case 2:
                    objExcel.ExportToPDF(dtExport, "TechnicianProductivity", "ZZZZZZZZZZZZZZZZ", "ZZZZZZZZZZZZZZZZ", dtReportHeader, dtReportFooter);
                    break;
                case 3:
                    exporter.WriteCsvToResponse();
                    break;
                default:
                    return;
            }

        }

        #endregion

        #region =======================Technician Productivity=========================
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string IsSrvTechProdFilter = Convert.ToString(hfIsSrvTechProdFilter.Value);
            Session["IsSrvTechProdFilter"] = IsSrvTechProdFilter;

            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);

            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");

            Task PopulateStockTrialDataTask = new Task(() => GetTechProdDetails(FROMDATE, TODATE));
            PopulateStockTrialDataTask.RunSynchronously();
        }

        public void GetTechProdDetails(string FROMDATE, string TODATE)
        {
            try
            {
                DataTable ds = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("PRC_TECHNICIANPRODUCTIVITY_REPORT");
                proc.AddPara("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                proc.AddPara("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                proc.AddPara("@FROMDATE", FROMDATE);
                proc.AddPara("@TODATE", TODATE);
                proc.AddPara("@PARTY_CODE", hdnTechnicianId.Value);
                proc.AddPara("@USERID", Convert.ToInt32(Session["userid"]));

                ds = proc.GetTable();

            }
            catch (Exception ex)
            {
            }
        }
        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            if (Convert.ToString(Session["IsSrvTechProdFilter"]) == "Y")
            {
                dtPartyTotal = oDBEngine.GetDataTable("SELECT PARTYNAME,TOTALRUNNINGSTBALLOCATED,TOTALRUNNINGSTBREPAIRED,TOTALBACKLOGSTBALLOCATED,TOTALBACKLOGSTBREPAIRED,OVERALLSTBALLOCATED,OVERALLREPAIRED,WORKINGDAYS from TECHNICIANPRODUCTIVITY_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND SEQ=999999999999 AND PARTYNAME='Total :'");
                TPTotalDesc = dtPartyTotal.Rows[0][0].ToString();
                TPTotalTRSA = dtPartyTotal.Rows[0][1].ToString();
                TPTotalTRSR = dtPartyTotal.Rows[0][2].ToString();
                TPTotalTBSA = dtPartyTotal.Rows[0][3].ToString();
                TPTotalTBSR = dtPartyTotal.Rows[0][4].ToString();
                TPTotalOA = dtPartyTotal.Rows[0][5].ToString();
                TPTotalOR = dtPartyTotal.Rows[0][6].ToString();
                TPTotalWD = dtPartyTotal.Rows[0][7].ToString();
            }

            string summaryTAG = (e.Item as ASPxSummaryItem).Tag;
            if (e.IsTotalSummary == true)
            {
                switch (summaryTAG)
                {
                    case "TagParty":
                        e.Text = TPTotalDesc;
                        break;
                    case "TagTRSA":
                        e.Text = TPTotalTRSA;
                        break;
                    case "TagTRSR":
                        e.Text = TPTotalTRSR;
                        break;
                    case "TagTBSA":
                        e.Text = TPTotalTBSA;
                        break;
                    case "TagTBSR":
                        e.Text = TPTotalTBSR;
                        break;
                    case "TagOA":
                        e.Text = TPTotalOA;
                        break;
                    case "TagOR":
                        e.Text = TPTotalOR;
                        break;
                    case "TagWD":
                        e.Text = TPTotalWD;
                        break;
                }
            }
        }

        protected void ShowGrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "BOXCHECKINGGRADE")
            {
                if (Convert.ToString(e.CellValue) == "A+")
                {
                    e.Cell.Font.Bold = true;
                    e.Cell.BackColor = Color.DarkGreen;
                }

                if (Convert.ToString(e.CellValue) == "A")
                {
                    e.Cell.Font.Bold = true;
                    e.Cell.BackColor = Color.SeaGreen;
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

            if (e.DataColumn.FieldName == "PERFORMANCEGRADE")
            {
                if (Convert.ToString(e.CellValue) == "A+")
                {
                    e.Cell.Font.Bold = true;
                    e.Cell.BackColor = Color.DarkGreen;
                }

                if (Convert.ToString(e.CellValue) == "A")
                {
                    e.Cell.Font.Bold = true;
                    e.Cell.BackColor = Color.SeaGreen;
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
        #endregion

        public void Date_finyearwise(string Finyear)
        {
            CommonBL stkledg = new CommonBL();
            DataTable dtstkledg = new DataTable();

            dtstkledg = stkledg.GetDateFinancila(Finyear);
            if (dtstkledg.Rows.Count > 0)
            {
                ASPxFromDate.MaxDate = Convert.ToDateTime((dtstkledg.Rows[0]["FinYear_EndDate"]));
                ASPxFromDate.MinDate = Convert.ToDateTime((dtstkledg.Rows[0]["FinYear_StartDate"]));

                ASPxToDate.MaxDate = Convert.ToDateTime((dtstkledg.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((dtstkledg.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((dtstkledg.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((dtstkledg.Rows[0]["FinYear_StartDate"]));

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

        #region LinQ
        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsSrvTechProdFilter"]) == "Y")
            {
                var q = from d in dc.TECHNICIANPRODUCTIVITY_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.SEQ) != "999999999999"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.TECHNICIANPRODUCTIVITY_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            ShowGrid.ExpandAll();
        }
        #endregion
    }
}