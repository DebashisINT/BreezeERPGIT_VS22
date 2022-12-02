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
    public partial class StockMovement : System.Web.UI.Page
    {
        DateTime dtFrom;
        DateTime dtTo;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        CommonBL cbl = new CommonBL();

        DataTable dtProductTotalForSummary = null;
        string ProductTotalDescForSummary = "";
        string ProductTotalOpQtyForSummary = "";
        string ProductTotalOpAltQtyForSummary = "";
        string ProductTotalOpValueForSummary = "";
        string ProductTotalInQtyForSummary = "";
        string ProductTotalInAltQtyForSummary = "";
        string ProductTotalInValForSummary = "";
        string ProductTotalOthInQtyForSummary = "";
        string ProductTotalOthInAltQtyForSummary = "";
        string ProductTotalOthInValForSummary = "";
        string ProductTotalOutQtyForSummary = "";
        string ProductTotalOutAltQtyForSummary = "";
        string ProductTotalOutValForSummary = "";
        string ProductTotalOthOutQtyForSummary = "";
        string ProductTotalOthOutAltQtyForSummary = "";
        string ProductTotalOthOutValForSummary = "";
        string ProductTotalCloseQtyForSummary = "";
        string ProductTotalCloseAltQtyForSummary = "";
        string ProductTotalCloseValForSummary = "";

        DataTable dtProductTotalForDetail = null;
        string ProductTotalDescForDetail = "";
        string ProductTotalOpQtyForDetail = "";
        string ProductTotalOpAltQtyForDetail = "";
        string ProductTotalOpValueForDetail = "";
        string ProductTotalInQtyForDetail = "";
        string ProductTotalInAltQtyForDetail = "";
        string ProductTotalInValForDetail = "";
        string ProductTotalOthInQtyForDetail = "";
        string ProductTotalOthInAltQtyForDetail = "";
        string ProductTotalOthInValForDetail = "";
        string ProductTotalOutQtyForDetail = "";
        string ProductTotalOutAltQtyForDetail = "";
        string ProductTotalOutValForDetail = "";
        string ProductTotalOthOutQtyForDetail = "";
        string ProductTotalOthOutAltQtyForDetail = "";
        string ProductTotalOthOutValForDetail = "";
        string ProductTotalCloseQtyForDetail = "";
        string ProductTotalCloseAltQtyForDetail = "";
        string ProductTotalCloseValForDetail = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/StockMovement.aspx");
            //Rev Debashis 0025145
            string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {
                    lookup_project.Visible = true;
                    lblProj.Visible = true;
                    GridStkDet.Columns[2].Visible = true;
                    hdnProjectSelection.Value = "1";

                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    lookup_project.Visible = false;
                    lblProj.Visible = false;
                    GridStkDet.Columns[2].Visible = false;
                    hdnProjectSelection.Value = "0";
                }
            }
            //End of Rev Debashis 0025145
            if (!IsPostBack)
            {
                //Rev Debashis 0025145
                DataTable dtProjectSelection = new DataTable();
                //End of Rev Debashis 0025145
                Session["chk_presenttotal"] = 0;
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Stock Movement - Summary";
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

                Session["IsStockMoveSummaryFilter"] = null;
                Session["IsStockMoveDetailFilter"] = null;
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

                //Rev Debashis 0025145
                dtProjectSelection = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='ReportsProjectSelection'");
                hdnProjectSelectionInReport.Value = dtProjectSelection.Rows[0][0].ToString();
                //End of Rev Debashis 0025145
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
            string filename = "StockMoveSummary";
            exporter.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Stock Movement - Summary" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
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
            }

        }
        public string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + Environment.NewLine + replace + Environment.NewLine + text.Substring(pos + search.Length);
        }
        //End of Rev
        protected void exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            e.BrickStyle.BackColor = Color.White;
            e.BrickStyle.ForeColor = Color.Black;
        }

        #endregion

        #region Export Stock Ledger Details
        public void cmbExportDet_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(ddldetails.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval1"] == null)
                {
                    Session["exportval1"] = Filter;
                    bindexportDet(Filter);
                }
                else if (Convert.ToInt32(Session["exportval1"]) != Filter)
                {
                    Session["exportval1"] = Filter;
                    bindexportDet(Filter);
                }
            }
        }

        public void bindexportDet(int Filter)
        {
            string filename = "StockMoveDetail";
            exporter.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();
            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Stock Movement - Detail" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");

            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";
            exporter.Landscape = true;
            exporter.MaxColumnWidth = 100;
            exporter.GridViewID = "GridStkDet";
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
            }

        }
        #endregion


        #region =======================Stock Move Summary=========================
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string IsStockMoveSummaryFilter = Convert.ToString(hfIsStockMoveSummaryFilter.Value);
            Session["IsStockMoveSummaryFilter"] = IsStockMoveSummaryFilter;

            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);

            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");

            //Rev Debashis 0025145
            //Task PopulateStockTrialDataTask = new Task(() => GetStockMoveSummary(FROMDATE, TODATE));
            //PopulateStockTrialDataTask.RunSynchronously();

            string PROJECT_ID = "";
            string Projects = "";
            List<object> ProjectList = lookup_project.GridView.GetSelectedFieldValues("ID");
            foreach (object Project in ProjectList)
            {
                Projects += "," + Project;
            }
            PROJECT_ID = Projects.TrimStart(',');

            Task PopulateStockTrialDataTask = new Task(() => GetStockMoveSummary(FROMDATE, TODATE,PROJECT_ID));
            PopulateStockTrialDataTask.RunSynchronously();
            //End of Rev Debashis 0025145
        }

        public void GetStockMoveSummary(string FROMDATE, string TODATE, string PROJECT_ID)
        {
            try
            {
                DataTable ds = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("PRC_STOCKMOVEMENT_REPORT");
                proc.AddPara("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                proc.AddPara("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                proc.AddPara("@FROMDATE", FROMDATE);
                proc.AddPara("@TODATE", TODATE);
                proc.AddPara("@BRANCHID", "");
                proc.AddPara("@PRODUCT_ID", hdnProductId.Value);
                proc.AddPara("@WAREHOUSE_ID", "");
                proc.AddPara("@VAL_TYPE", ddlValTech.SelectedValue);
                proc.AddPara("@CLASS", hdnClassId.Value);
                proc.AddPara("@BRAND", hdnBranndId.Value);
                proc.AddPara("@OWMASTERVALTECH", (chkOWMSTVT.Checked) ? "1" : "0");
                proc.AddPara("@REPORTTYPE", "Summary");
                proc.AddPara("@WHICHMODULE", "StockMoveSummary");
                proc.AddPara("@CONSLANDEDCOST", (chkConsLandCost.Checked) ? "1" : "0");
                proc.AddPara("@CONSOVERHEADCOST", (chkConsOverHeadCost.Checked) ? "1" : "0");
                proc.AddPara("@ISBRANCHWISE", (chkIsBranchWise.Checked) ? "1" : "0");
                proc.AddPara("@ISWHWISE", (chkIsWHWise.Checked) ? "1" : "0");
                //Rev Debashis 0025145
                proc.AddPara("@PROJECT_ID", PROJECT_ID);
                //End of Rev Debashis 0025145
                proc.AddPara("@USERID", Convert.ToInt32(Session["userid"]));

                ds = proc.GetTable();

            }
            catch (Exception ex)
            {
            }
        }
        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            if (Convert.ToString(Session["IsStockMoveSummaryFilter"]) == "Y")
            {
                dtProductTotalForSummary = oDBEngine.GetDataTable("SELECT BRANDNAME,OP_QTY,OP_ALTQTY,OP_TOTAL,IN_QTY,ALTIN_QTY,IN_TOTAL,OTHIN_QTY,OTHALTIN_QTY,OTHIN_TOTAL,OUT_QTY,ALTOUT_QTY,OUT_TOTAL,OTHOUT_QTY,OTHALTOUT_QTY,OTHOUT_TOTAL,CLOSE_QTY,CLOSE_ALTQTY,CLOSE_VAL FROM STOCKMOVEMENT_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND REPORTTYPE='Summary' AND WHICHMODULE='StockMoveSummary' AND PRODID=999999999999 AND BRANDNAME='Gross Total :'");
                ProductTotalDescForSummary = dtProductTotalForSummary.Rows[0][0].ToString();
                ProductTotalOpQtyForSummary = dtProductTotalForSummary.Rows[0][1].ToString();
                ProductTotalOpAltQtyForSummary = dtProductTotalForSummary.Rows[0][2].ToString();
                ProductTotalOpValueForSummary = dtProductTotalForSummary.Rows[0][3].ToString();
                ProductTotalInQtyForSummary = dtProductTotalForSummary.Rows[0][4].ToString();
                ProductTotalInAltQtyForSummary = dtProductTotalForSummary.Rows[0][5].ToString();
                ProductTotalInValForSummary = dtProductTotalForSummary.Rows[0][6].ToString();
                ProductTotalOthInQtyForSummary = dtProductTotalForSummary.Rows[0][7].ToString();
                ProductTotalOthInAltQtyForSummary = dtProductTotalForSummary.Rows[0][8].ToString();
                ProductTotalOthInValForSummary = dtProductTotalForSummary.Rows[0][9].ToString();
                ProductTotalOutQtyForSummary = dtProductTotalForSummary.Rows[0][10].ToString();
                ProductTotalOutAltQtyForSummary = dtProductTotalForSummary.Rows[0][11].ToString();
                ProductTotalOutValForSummary = dtProductTotalForSummary.Rows[0][12].ToString();
                ProductTotalOthOutQtyForSummary = dtProductTotalForSummary.Rows[0][13].ToString();
                ProductTotalOthOutAltQtyForSummary = dtProductTotalForSummary.Rows[0][14].ToString();
                ProductTotalOthOutValForSummary = dtProductTotalForSummary.Rows[0][15].ToString();
                ProductTotalCloseQtyForSummary = dtProductTotalForSummary.Rows[0][16].ToString();
                ProductTotalCloseAltQtyForSummary = dtProductTotalForSummary.Rows[0][17].ToString();
                ProductTotalCloseValForSummary = dtProductTotalForSummary.Rows[0][18].ToString();
            }

            string summaryTAG = (e.Item as ASPxSummaryItem).Tag;
            if (e.IsTotalSummary == true)
            {
                switch (summaryTAG)
                {
                    case "Sum_Brand":
                        e.Text = ProductTotalDescForSummary;
                        break;
                    case "Sum_OpQty":
                        e.Text = ProductTotalOpQtyForSummary;
                        break;
                    case "Sum_OpAltQty":
                        e.Text = ProductTotalOpAltQtyForSummary;
                        break;
                    case "Sum_OpValue":
                        e.Text = ProductTotalOpValueForSummary;
                        break;
                    case "Sum_InQty":
                        e.Text = ProductTotalInQtyForSummary;
                        break;
                    case "Sum_InAltQty":
                        e.Text = ProductTotalInAltQtyForSummary;
                        break;
                    case "Sum_InVal":
                        e.Text = ProductTotalInValForSummary;
                        break;
                    case "Sum_OthInQty":
                        e.Text = ProductTotalOthInQtyForSummary;
                        break;
                    case "Sum_OthInAltQty":
                        e.Text = ProductTotalOthInAltQtyForSummary;
                        break;
                    case "Sum_OthInVal":
                        e.Text = ProductTotalOthInValForSummary;
                        break;
                    case "Sum_OutQty":
                        e.Text = ProductTotalOutQtyForSummary;
                        break;
                    case "Sum_OutAltQty":
                        e.Text = ProductTotalOutAltQtyForSummary;
                        break;
                    case "Sum_OutVal":
                        e.Text = ProductTotalOutValForSummary;
                        break;
                    case "Sum_OthOutQty":
                        e.Text = ProductTotalOthOutQtyForSummary;
                        break;
                    case "Sum_OthOutAltQty":
                        e.Text = ProductTotalOthOutAltQtyForSummary;
                        break;
                    case "Sum_OthOutVal":
                        e.Text = ProductTotalOthOutValForSummary;
                        break;
                    case "Sum_CloseQty":
                        e.Text = ProductTotalCloseQtyForSummary;
                        break;
                    case "Sum_CloseAltQty":
                        e.Text = ProductTotalCloseAltQtyForSummary;
                        break;
                    case "Sum_CloseVal":
                        e.Text = ProductTotalCloseValForSummary;
                        break;
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

            if (Convert.ToString(Session["IsStockMoveSummaryFilter"]) == "Y")
            {
                var q = from d in dc.STOCKMOVEMENT_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.REPORTTYPE) == "Summary" && Convert.ToString(d.PRODID) != "999999999999" && Convert.ToString(d.WHICHMODULE) == "StockMoveSummary"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.STOCKMOVEMENT_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            ShowGrid.ExpandAll();
        }
        #endregion

        protected void ShowGrid_HtmlFooterCellPrepared(object sender, ASPxGridViewTableFooterCellEventArgs e)
        {
            if (e.Column.Caption != "Brand")
            {
                e.Cell.Style["text-align"] = "right";
            }
        }

        #region =====================Stock Movement Detail===========================
        protected void CallbackPanelDetail_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string IsStockMoveDetailFilter = Convert.ToString(hfIsStockMoveDetailFilter.Value);
            Session["IsStockMoveDetailFilter"] = IsStockMoveDetailFilter;

            string returnPara = Convert.ToString(e.Parameter);
            string WhichCall = returnPara.Split('~')[0];

            //Rev Debashis 0025145
            string PROJECT_ID = "";
            string Projects = "";
            List<object> ProjectList = lookup_project.GridView.GetSelectedFieldValues("ID");
            foreach (object Project in ProjectList)
            {
                Projects += "," + Project;
            }
            PROJECT_ID = Projects.TrimStart(',');
            //End of Rev Debashis 0025145

            if (WhichCall == "BndPopupgrid")
            {
                string prodId = returnPara.Split('~')[1];
                GetStockMoveDetail(prodId, PROJECT_ID);
            }

        }

        public void GetStockMoveDetail(string ProductIds, string PROJECT_ID)
        {
            try
            {
                string strClassList = "", strBrandList = "";

                strClassList = hdnClassId.Value;
                strBrandList = hdnBranndId.Value;

                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                string TODATE = dtTo.ToString("yyyy-MM-dd");

                DataTable ds = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("PRC_STOCKMOVEMENT_REPORT");
                proc.AddPara("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                proc.AddPara("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                proc.AddPara("@FROMDATE", FROMDATE);
                proc.AddPara("@TODATE", TODATE);
                proc.AddPara("@BRANCHID", "");
                proc.AddPara("@PRODUCT_ID", ProductIds);
                proc.AddPara("@WAREHOUSE_ID", "");
                proc.AddPara("@VAL_TYPE", ddlValTech.SelectedValue);
                proc.AddPara("@CLASS", hdnClassId.Value);
                proc.AddPara("@BRAND", hdnBranndId.Value);
                proc.AddPara("@OWMASTERVALTECH", (chkOWMSTVT.Checked) ? "1" : "0");
                proc.AddPara("@REPORTTYPE", "Detail");
                proc.AddPara("@WHICHMODULE", "StockMoveDetail");
                proc.AddPara("@CONSLANDEDCOST", (chkConsLandCost.Checked) ? "1" : "0");
                proc.AddPara("@CONSOVERHEADCOST", (chkConsOverHeadCost.Checked) ? "1" : "0");
                proc.AddPara("@ISBRANCHWISE", (chkIsBranchWise.Checked) ? "1" : "0");
                proc.AddPara("@ISWHWISE", (chkIsWHWise.Checked) ? "1" : "0");
                //Rev Debashis 0025145
                proc.AddPara("@PROJECT_ID", PROJECT_ID);
                //End of Rev Debashis 0025145
                proc.AddPara("@USERID", Convert.ToInt32(Session["userid"]));

                ds = proc.GetTable();

            }
            catch (Exception ex)
            {
            }
        }

        protected void ShowGrid1_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            if (Convert.ToString(Session["IsStockMoveDetailFilter"]) == "Y")
            {
                dtProductTotalForDetail = oDBEngine.GetDataTable("SELECT BRANDNAME,OP_QTY,OP_ALTQTY,OP_TOTAL,IN_QTY,ALTIN_QTY,IN_TOTAL,OTHIN_QTY,OTHALTIN_QTY,OTHIN_TOTAL,OUT_QTY,ALTOUT_QTY,OUT_TOTAL,OTHOUT_QTY,OTHALTOUT_QTY,OTHOUT_TOTAL,CLOSE_QTY,CLOSE_ALTQTY,CLOSE_VAL FROM STOCKMOVEMENT_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND REPORTTYPE='Detail' AND WHICHMODULE='StockMoveDetail' AND PRODID=999999999999 AND BRANDNAME='Gross Total :'");
                ProductTotalDescForDetail = dtProductTotalForDetail.Rows[0][0].ToString();
                ProductTotalOpQtyForDetail = dtProductTotalForDetail.Rows[0][1].ToString();
                ProductTotalOpAltQtyForDetail = dtProductTotalForDetail.Rows[0][2].ToString();
                ProductTotalOpValueForDetail = dtProductTotalForDetail.Rows[0][3].ToString();
                ProductTotalInQtyForDetail = dtProductTotalForDetail.Rows[0][4].ToString();
                ProductTotalInAltQtyForDetail = dtProductTotalForDetail.Rows[0][5].ToString();
                ProductTotalInValForDetail = dtProductTotalForDetail.Rows[0][6].ToString();
                ProductTotalOthInQtyForDetail = dtProductTotalForDetail.Rows[0][7].ToString();
                ProductTotalOthInAltQtyForDetail = dtProductTotalForDetail.Rows[0][8].ToString();
                ProductTotalOthInValForDetail = dtProductTotalForDetail.Rows[0][9].ToString();
                ProductTotalOutQtyForDetail = dtProductTotalForDetail.Rows[0][10].ToString();
                ProductTotalOutAltQtyForDetail = dtProductTotalForDetail.Rows[0][11].ToString();
                ProductTotalOutValForDetail = dtProductTotalForDetail.Rows[0][12].ToString();
                ProductTotalOthOutQtyForDetail = dtProductTotalForDetail.Rows[0][13].ToString();
                ProductTotalOthOutAltQtyForDetail = dtProductTotalForDetail.Rows[0][14].ToString();
                ProductTotalOthOutValForDetail = dtProductTotalForDetail.Rows[0][15].ToString();
                ProductTotalCloseQtyForDetail = dtProductTotalForDetail.Rows[0][16].ToString();
                ProductTotalCloseAltQtyForDetail = dtProductTotalForDetail.Rows[0][17].ToString();
                ProductTotalCloseValForDetail = dtProductTotalForDetail.Rows[0][18].ToString();
            }

            string summaryTAG = (e.Item as ASPxSummaryItem).Tag;
            if (e.IsTotalSummary == true)
            {
                switch (summaryTAG)
                {
                    case "Det_Brand":
                        e.Text = ProductTotalDescForDetail;
                        break;
                    case "Det_OpQty":
                        e.Text = ProductTotalOpQtyForDetail;
                        break;
                    case "Det_OpAltQty":
                        e.Text = ProductTotalOpAltQtyForDetail;
                        break;
                    case "Det_OpValue":
                        e.Text = ProductTotalOpValueForDetail;
                        break;
                    case "Det_InQty":
                        e.Text = ProductTotalInQtyForDetail;
                        break;
                    case "Det_InAltQty":
                        e.Text = ProductTotalInAltQtyForDetail;
                        break;
                    case "Det_InVal":
                        e.Text = ProductTotalInValForDetail;
                        break;
                    case "Det_OthInQty":
                        e.Text = ProductTotalOthInQtyForDetail;
                        break;
                    case "Det_OthInAltQty":
                        e.Text = ProductTotalOthInAltQtyForDetail;
                        break;
                    case "Det_OthInVal":
                        e.Text = ProductTotalOthInValForDetail;
                        break;
                    case "Det_OutQty":
                        e.Text = ProductTotalOutQtyForDetail;
                        break;
                    case "Det_OutAltQty":
                        e.Text = ProductTotalOutAltQtyForDetail;
                        break;
                    case "Det_OutVal":
                        e.Text = ProductTotalOutValForDetail;
                        break;
                    case "Det_OthOutQty":
                        e.Text = ProductTotalOthOutQtyForDetail;
                        break;
                    case "Det_OthOutAltQty":
                        e.Text = ProductTotalOthOutAltQtyForDetail;
                        break;
                    case "Det_OthOutVal":
                        e.Text = ProductTotalOthOutValForDetail;
                        break;
                    case "Det_CloseQty":
                        e.Text = ProductTotalCloseQtyForDetail;
                        break;
                    case "Det_CloseAltQty":
                        e.Text = ProductTotalCloseAltQtyForDetail;
                        break;
                    case "Det_CloseVal":
                        e.Text = ProductTotalCloseValForDetail;
                        break;
                }
            }
        }

        protected void GenerateEntityServerDetailsModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsStockMoveDetailFilter"]) == "Y")
            {
                var q = from d in dc.STOCKMOVEMENT_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.REPORTTYPE) == "Detail" && Convert.ToString(d.PRODID) != "999999999999" && Convert.ToString(d.WHICHMODULE) == "StockMoveDetail"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.STOCKMOVEMENT_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }

            string strisbranchchk = (chkIsBranchWise.Checked) ? "1" : "0";
            string striswhchk = (chkIsWHWise.Checked) ? "1" : "0";

            if (Convert.ToString(strisbranchchk) == "1")
            {
                GridStkDet.Columns[0].Visible = true;
            }
            else
            {
                GridStkDet.Columns[0].Visible = false;
            }

            if (Convert.ToString(striswhchk) == "1")
            {
                GridStkDet.Columns[1].Visible = true;
            }
            else
            {
                GridStkDet.Columns[1].Visible = false;
            }
            //Rev Debashis 0025145
            string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
            if (ProjectSelectInEntryModule.ToUpper().Trim() == "YES")
            {
                GridStkDet.Columns[2].Visible = true;
            }
            else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
            {
                GridStkDet.Columns[2].Visible = false;
            }
            //End of Rev Debashis 0025145
        }

        protected void ShowGrid1_HtmlFooterCellPrepared(object sender, ASPxGridViewTableFooterCellEventArgs e)
        {
            if (e.Column.Caption != "Brand")
            {
                e.Cell.Style["text-align"] = "right";
            }
        }

        protected void ShowGrid1_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (Convert.ToString(e.CellValue) == "Total :")
            {
                Session["chk_presenttotal"] = 1;
            }
            if (Convert.ToInt32(Session["chk_presenttotal"]) == 1)
            {
                e.Cell.Font.Bold = true;
                e.Cell.BackColor = Color.DarkSeaGreen;
            }

            if (e.DataColumn.FieldName == "CLOSE_VAL")
            {
                Session["chk_presenttotal"] = 0;
            }
        }
        #endregion
        //Rev Debashis 0025145
        protected void Project_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "BindProjectGrid")
            {
                DataTable ProjectTable = new DataTable();
                ProjectTable = GetProject();

                if (ProjectTable.Rows.Count > 0)
                {
                    Session["ProjectData"] = ProjectTable;
                    lookup_project.DataSource = ProjectTable;
                    lookup_project.DataBind();
                }
                else
                {
                    Session["ProjectData"] = ProjectTable;
                    lookup_project.DataSource = null;
                    lookup_project.DataBind();
                }
            }
        }

        public DataTable GetProject()
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("PRC_FETCHPROJECTS_REPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            cmd.Dispose();
            con.Dispose();

            return dt;
        }

        protected void lookup_project_DataBinding(object sender, EventArgs e)
        {
            if (Session["ProjectData"] != null)
            {
                lookup_project.DataSource = (DataTable)Session["ProjectData"];
            }
        }
        //End of Rev Debashis 0025145
    }
}