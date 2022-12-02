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
    public partial class StockLedgerSummary : System.Web.UI.Page
    {
        DateTime dtFrom;
        DateTime dtTo;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        DataTable dtProductTotalForSummary = null;
        string ProductTotalInQtyForSummary = "";
        string ProductTotalInMQtyForSummary = "";
        string ProductTotalOutQtyForSummary = "";
        string ProductTotalOutMQtyForSummary = "";
        string ProductTotalInValueForSummary = "";
        string ProductTotalInOutValueForSummary = "";
        string ProductTotalCloseValForSummary = "";
        string ProductTotalCloseQtyForSummary = "";
        string ProductTotalCloseMQtyForSummary = "";
        string ProductTotalDescForSummary = "";

        DataTable dtProductTotalForDetail = null;
        string ProductTotalInQtyForDetail = "";
        string ProductTotalInMQtyForDetail = "";
        string ProductTotalOutQtyForDetail = "";
        string ProductTotalOutMQtyForDetail = "";
        string ProductTotalInValueForDetail = "";
        string ProductTotalInOutValueForDetail = "";
        string ProductTotalCloseValForDetail = "";
        string ProductTotalCloseQtyForDetail = "";
        string ProductTotalCloseMQtyForDetail = "";
        string ProductTotalDescForDetail = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/StockLedgerSummary.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            if (!IsPostBack)
            {
                Session["chk_presenttotal"] = 0;
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Stock Ledger - Summary";
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
                Session["IsStockLedgerSummaryFilter"] = null;
                Session["IsStockLedgerDetailFilter"] = null;
                Session["SI_ComponentData_Branch"] = null;
                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                ASPxFromDate.Text = dtFrom.ToString("dd-MM-yyyy");
                ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");
                ASPxFromDate.Value = DateTime.Now;
                ASPxToDate.Value = DateTime.Now;

                ShowGrid.Columns[5].Visible = false;
                ShowGrid.Columns[7].Visible = false;
                ShowGrid.Columns[9].Visible = false;
                ShowGrid.Columns[14].Visible = false;

                GridStkDet.Columns[0].Visible = false;
                //GridStkDet.Columns[10].Visible = false;
                //GridStkDet.Columns[12].Visible = false;
                //GridStkDet.Columns[14].Visible = false;
                //GridStkDet.Columns[21].Visible = false;
                GridStkDet.Columns[12].Visible = false;
                GridStkDet.Columns[14].Visible = false;
                GridStkDet.Columns[16].Visible = false;
                GridStkDet.Columns[23].Visible = false;

                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                string TODATE = dtTo.ToString("yyyy-MM-dd");
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                BranchHoOffice();
                Session["BranchNames"] = null;
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
        public void BranchHoOffice()
        {
            CommonBL bll1 = new CommonBL();
            DataTable stbill = new DataTable();
            DataTable dtBranchChild = new DataTable();
            stbill = bll1.GetBranchheadoffice(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), "HO");
            if (stbill.Rows.Count > 0)
            {
                ddlbranchHO.DataSource = stbill;
                ddlbranchHO.DataTextField = "Code";
                ddlbranchHO.DataValueField = "branch_id";
                ddlbranchHO.DataBind();
                dtBranchChild = GetChildBranch(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
                if (dtBranchChild.Rows.Count > 0)
                {
                    ddlbranchHO.Items.Insert(0, new ListItem("All", "All"));
                }
            }
        }

        public DataTable GetChildBranch(string CHILDBRANCH)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("PRC_FINDCHILDBRANCH_REPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CHILDBRANCH", CHILDBRANCH);
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            cmd.Dispose();
            con.Dispose();
            return dt;
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
            string filename = "StockLedgerSummary";
            exporter.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Stock Ledger - Summary" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
            FileHeader = ReplaceFirst(FileHeader, "\r\n", Convert.ToString(Session["BranchNames"]));
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
            string filename = "StockLedgerDetail";
            exporter.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();
            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Stock Ledger - Detail" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");

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


        #region =======================Stock Ledger Summary=========================
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string IsStockLedgerSummaryFilter = Convert.ToString(hfIsStockLedgerSummaryFilter.Value);
            Session["IsStockLedgerSummaryFilter"] = IsStockLedgerSummaryFilter;

            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);

            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");

            string BRANCH_ID = "";

            string BranchList = "";
            List<object> BranchList1 = lookup_branch.GridView.GetSelectedFieldValues("ID");
            foreach (object Branch in BranchList1)
            {
                BranchList += "," + Branch;
            }
            BRANCH_ID = BranchList.TrimStart(',');

            string Product = "";
            Product = hdnProductId.Value;
            string BRANCH_NAME = "";
            string BranchNameComponent = "";
            List<object> BranchNameList = lookup_branch.GridView.GetSelectedFieldValues("branch_description");
            foreach (object BranchName in BranchNameList)
            {
                BranchNameComponent += "," + BranchName;
            }
            if (BranchNameList.Count > 1)
            {
                BRANCH_NAME = "Multiple Branch Selected";
                Session["BranchNames"] = BRANCH_NAME;
            }
            else
            {
                BRANCH_NAME = BranchNameComponent.TrimStart(',');
                Session["BranchNames"] = "For Unit : " + BRANCH_NAME + " ";
            }
            CallbackPanel.JSProperties["cpBranchNames"] = Convert.ToString(Session["BranchNames"]);
            Task PopulateStockTrialDataTask = new Task(() => GetStockLedgerSummary(FROMDATE, TODATE, BRANCH_ID, Product));
            PopulateStockTrialDataTask.RunSynchronously();
        }

        public void GetStockLedgerSummary(string FROMDATE, string TODATE, string BRANCH_ID, string ProductIds)
        {
            try
            {
                string strClassList = "", strBrandList = "";
                strClassList = hdnClassId.Value;
                strBrandList = hdnBranndId.Value;

                DataTable ds = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("PRC_STOCKLEDGERRUNNINGBALANCE_REPORT");
                proc.AddPara("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                proc.AddPara("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                proc.AddPara("@FROMDATE", FROMDATE);
                proc.AddPara("@TODATE", TODATE);
                proc.AddPara("@BRANCHID", BRANCH_ID);
                proc.AddPara("@PRODUCT_ID", ProductIds);
                proc.AddPara("@VAL_TYPE", ddlValTech.SelectedValue);
                proc.AddPara("@CLASS", strClassList);
                proc.AddPara("@BRAND", strBrandList);
                proc.AddPara("@OWMASTERVALTECH", (chkOWMSTVT.Checked) ? "1" : "0");
                proc.AddPara("@REPORTTYPE", "Summary");
                proc.AddPara("@WHICHMODULE", "StockLedgerSummary");
                proc.AddPara("@CONSLANDEDCOST", (chkConsLandCost.Checked) ? "1" : "0");
                proc.AddPara("@SHOWMULTIUOM", (chkShowMultiUOM.Checked) ? "1" : "0");
                proc.AddPara("@CONSOVERHEADCOST", (chkConsOverHeadCost.Checked) ? "1" : "0");
                proc.AddPara("@USERID", Convert.ToInt32(Session["userid"]));

                ds = proc.GetTable();

            }
            catch (Exception ex)
            {
            }
        }
        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            if (Convert.ToString(Session["IsStockLedgerSummaryFilter"]) == "Y")
            {
                dtProductTotalForSummary = oDBEngine.GetDataTable("SELECT BRAND_NAME,IN_QTY,IN_MULTQTY,OUT_QTY,OUT_MULTQTY,IN_TOTAL,OUT_TOTAL,CLOSE_VAL,CLOSE_QTY,MULTCLOSE_QTY FROM STOCKLEDGERRUNNINGBALANCE_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND REPORTTYPE='Summary' AND WHICHMODULE='StockLedgerSummary' AND SPRODUCTS_ID=999999999999 AND BRAND_NAME='Gross Total :'");
                ProductTotalDescForSummary = dtProductTotalForSummary.Rows[0][0].ToString();
                ProductTotalInQtyForSummary = dtProductTotalForSummary.Rows[0][1].ToString();
                ProductTotalInMQtyForSummary = dtProductTotalForSummary.Rows[0][2].ToString();
                ProductTotalOutQtyForSummary = dtProductTotalForSummary.Rows[0][3].ToString();
                ProductTotalOutMQtyForSummary = dtProductTotalForSummary.Rows[0][4].ToString();
                ProductTotalInValueForSummary = dtProductTotalForSummary.Rows[0][5].ToString();
                ProductTotalInOutValueForSummary = dtProductTotalForSummary.Rows[0][6].ToString();
                ProductTotalCloseValForSummary = dtProductTotalForSummary.Rows[0][7].ToString();
                ProductTotalCloseQtyForSummary = dtProductTotalForSummary.Rows[0][8].ToString();
                ProductTotalCloseMQtyForSummary = dtProductTotalForSummary.Rows[0][9].ToString();
            }

            string summaryTAG = (e.Item as ASPxSummaryItem).Tag;
            if (e.IsTotalSummary == true)
            {
                switch (summaryTAG)
                {
                    case "Item_Brand":
                        e.Text = ProductTotalDescForSummary;
                        break;
                    case "Item_InQty":
                        e.Text = ProductTotalInQtyForSummary;
                        break;
                    case "Item_InMQty":
                        e.Text = ProductTotalInMQtyForSummary;
                        break;
                    case "Item_OutQty":
                        e.Text = ProductTotalOutQtyForSummary;
                        break;
                    case "Item_OutMQty":
                        e.Text = ProductTotalOutMQtyForSummary;
                        break;
                    case "Item_InValue":
                        e.Text = ProductTotalInValueForSummary;
                        break;
                    case "Item_OutValue":
                        e.Text = ProductTotalInOutValueForSummary;
                        break;
                    case "Item_Close":
                        e.Text = ProductTotalCloseQtyForSummary;
                        break;
                    case "Item_MClose":
                        e.Text = ProductTotalCloseMQtyForSummary;
                        break;
                    case "Val_Close":
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

        #region Branch Populate

        protected void Componentbranch_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string FinYear = Convert.ToString(Session["LastFinYear"]);

            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                DataTable ComponentTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];
                if (Hoid != "All")
                {
                    ComponentTable = GetBranch(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Hoid);
                }
                else
                {
                    ComponentTable = oDBEngine.GetDataTable("select * from (select branch_id as ID,branch_description,branch_code from tbl_master_branch a where a.branch_id=1  union all select branch_id as ID,branch_description,branch_code from tbl_master_branch b where b.branch_parentId=1) a order by branch_description");
                }

                if (ComponentTable.Rows.Count > 0)
                {
                    Session["SI_ComponentData_Branch"] = ComponentTable;
                    lookup_branch.DataSource = ComponentTable;
                    lookup_branch.DataBind();
                }
                else
                {
                    Session["SI_ComponentData_Branch"] = ComponentTable;
                    lookup_branch.DataSource = null;
                    lookup_branch.DataBind();
                }
            }
        }

        public DataTable GetBranch(string BRANCH_ID, string Ho)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("GetFinancerBranchfetchhowise", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Branch", BRANCH_ID);
            cmd.Parameters.AddWithValue("@Hoid", Ho);
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            cmd.Dispose();
            con.Dispose();

            return dt;
        }

        protected void lookup_branch_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_ComponentData_Branch"] != null)
            {
                lookup_branch.DataSource = (DataTable)Session["SI_ComponentData_Branch"];
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

            if (Convert.ToString(Session["IsStockLedgerSummaryFilter"]) == "Y")
            {
                var q = from d in dc.STOCKLEDGERRUNNINGBALANCE_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.REPORTTYPE) == "Summary" && Convert.ToString(d.SPRODUCTS_ID) != "999999999999" && Convert.ToString(d.WHICHMODULE) == "StockLedgerSummary"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.STOCKLEDGERRUNNINGBALANCE_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }

            string strshowmultuomsum = (chkShowMultiUOM.Checked) ? "1" : "0";
            if (Convert.ToString(strshowmultuomsum) == "0")
            {
                ShowGrid.Columns[5].Visible = false;
                ShowGrid.Columns[7].Visible = false;
                ShowGrid.Columns[9].Visible = false;
                ShowGrid.Columns[14].Visible = false;
            }
            else
            {
                ShowGrid.Columns[5].Visible = true;
                ShowGrid.Columns[7].Visible = true;
                ShowGrid.Columns[9].Visible = true;
                ShowGrid.Columns[14].Visible = true;
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

        #region =====================Stock Ledger Detail===========================
        protected void CallbackPanelDetail_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string IsStockLedgerDetailFilter = Convert.ToString(hfIsStockLedgerDetailFilter.Value);
            Session["IsStockLedgerDetailFilter"] = IsStockLedgerDetailFilter;

            string returnPara = Convert.ToString(e.Parameter);
            string WhichCall = returnPara.Split('~')[0];

            if (WhichCall == "BndPopupgrid")
            {
                string branch = returnPara.Split('~')[1];
                string prodId = returnPara.Split('~')[2];
                GetStockLedgerDetail(prodId, branch);
            }

        }

        public void GetStockLedgerDetail(string ProductIds, string BRANCH_ID)
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
                ProcedureExecute proc = new ProcedureExecute("PRC_STOCKLEDGERRUNNINGBALANCE_REPORT");
                proc.AddPara("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                proc.AddPara("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                proc.AddPara("@FROMDATE", FROMDATE);
                proc.AddPara("@TODATE", TODATE);
                proc.AddPara("@BRANCHID", BRANCH_ID);
                proc.AddPara("@PRODUCT_ID", ProductIds);
                proc.AddPara("@VAL_TYPE", ddlValTech.SelectedValue);
                proc.AddPara("@CLASS", strClassList);
                proc.AddPara("@BRAND", strBrandList);
                proc.AddPara("@OWMASTERVALTECH", (chkOWMSTVT.Checked) ? "1" : "0");
                proc.AddPara("@REPORTTYPE", "Detail");
                proc.AddPara("@WHICHMODULE", "StockLedgerSummary");
                proc.AddPara("@CONSLANDEDCOST", (chkConsLandCost.Checked) ? "1" : "0");
                proc.AddPara("@SHOWMULTIUOM", (chkShowMultiUOM.Checked) ? "1" : "0");
                proc.AddPara("@CONSOVERHEADCOST", (chkConsOverHeadCost.Checked) ? "1" : "0");
                proc.AddPara("@USERID", Convert.ToInt32(Session["userid"]));

                ds = proc.GetTable();

            }
            catch (Exception ex)
            {
            }
        }

        protected void ShowGrid1_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            //e.Text = string.Format("{0}", e.Value);
            if (Convert.ToString(Session["IsStockLedgerDetailFilter"]) == "Y")
            {
                dtProductTotalForDetail = oDBEngine.GetDataTable("SELECT BRAND_NAME,IN_QTY,IN_MULTQTY,OUT_QTY,OUT_MULTQTY,IN_TOTAL,OUT_TOTAL,CLOSE_VAL,CLOSE_QTY,MULTCLOSE_QTY FROM STOCKLEDGERRUNNINGBALANCE_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND REPORTTYPE='Detail' AND WHICHMODULE='StockLedgerSummary' AND SPRODUCTS_ID=999999999999 AND BRAND_NAME='Gross Total :'");
                ProductTotalDescForDetail = dtProductTotalForDetail.Rows[0][0].ToString();
                ProductTotalInQtyForDetail = dtProductTotalForDetail.Rows[0][1].ToString();
                ProductTotalInMQtyForDetail = dtProductTotalForDetail.Rows[0][2].ToString();
                ProductTotalOutQtyForDetail = dtProductTotalForDetail.Rows[0][3].ToString();
                ProductTotalOutMQtyForDetail = dtProductTotalForDetail.Rows[0][4].ToString();
                ProductTotalInValueForDetail = dtProductTotalForDetail.Rows[0][5].ToString();
                ProductTotalInOutValueForDetail = dtProductTotalForDetail.Rows[0][6].ToString();
                ProductTotalCloseValForDetail = dtProductTotalForDetail.Rows[0][7].ToString();
                ProductTotalCloseQtyForDetail = dtProductTotalForDetail.Rows[0][8].ToString();
                ProductTotalCloseMQtyForDetail = dtProductTotalForDetail.Rows[0][9].ToString();
            }

            string summaryTAG = (e.Item as ASPxSummaryItem).Tag;
            if (e.IsTotalSummary == true)
            {
                switch (summaryTAG)
                {
                    case "Item_Brand_Det":
                        e.Text = ProductTotalDescForDetail;
                        break;
                    case "Item_InQty_Det":
                        e.Text = ProductTotalInQtyForDetail;
                        break;
                    case "Item_InMQty_Det":
                        e.Text = ProductTotalInMQtyForDetail;
                        break;
                    case "Item_OutQty_Det":
                        e.Text = ProductTotalOutQtyForDetail;
                        break;
                    case "Item_OutMQty_Det":
                        e.Text = ProductTotalOutMQtyForDetail;
                        break;
                    case "Item_InValue_Det":
                        e.Text = ProductTotalInValueForDetail;
                        break;
                    case "Item_OutValue_Det":
                        e.Text = ProductTotalInOutValueForDetail;
                        break;
                    case "Item_Close_Det":
                        e.Text = ProductTotalCloseQtyForDetail;
                        break;
                    case "Item_MClose_Det":
                        e.Text = ProductTotalCloseMQtyForDetail;
                        break;
                    case "Val_Close_Det":
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

            if (Convert.ToString(Session["IsStockLedgerDetailFilter"]) == "Y")
            {
                var q = from d in dc.STOCKLEDGERRUNNINGBALANCE_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.REPORTTYPE) == "Detail" && Convert.ToString(d.SPRODUCTS_ID) != "999999999999" && Convert.ToString(d.WHICHMODULE) == "StockLedgerSummary"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.STOCKLEDGERRUNNINGBALANCE_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }

            string strshowmultuomdet = (chkShowMultiUOM.Checked) ? "1" : "0";
            if (Convert.ToString(strshowmultuomdet) == "0")
            {
                GridStkDet.Columns[0].Visible = false;
                //GridStkDet.Columns[10].Visible = false;
                //GridStkDet.Columns[12].Visible = false;
                //GridStkDet.Columns[14].Visible = false;
                //GridStkDet.Columns[21].Visible = false;
                GridStkDet.Columns[12].Visible = false;
                GridStkDet.Columns[14].Visible = false;
                GridStkDet.Columns[16].Visible = false;
                GridStkDet.Columns[23].Visible = false;
            }
            else
            {
                GridStkDet.Columns[0].Visible = true;
                //GridStkDet.Columns[10].Visible = true;
                //GridStkDet.Columns[12].Visible = true;
                //GridStkDet.Columns[14].Visible = true;
                //GridStkDet.Columns[21].Visible = true;
                GridStkDet.Columns[12].Visible = true;
                GridStkDet.Columns[14].Visible = true;
                GridStkDet.Columns[16].Visible = true;
                GridStkDet.Columns[23].Visible = true;
            }
        }
        #endregion
    }
}