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
    public partial class WarehousewiseStockValuation : System.Web.UI.Page
    {
        DateTime dtFrom;
        DateTime dtTo;
        TotalvaluationClass objvaluation = new TotalvaluationClass();
        string data = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/WarehousewiseStockValuation.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            if (!IsPostBack)
            {
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Warehouse wise Stock Valuation";
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
                Session["IsWHwiseStockValFilter"] = null;
                Session["IsWHwiseStockValDetFilter"] = null;
                Session["SI_ComponentData_Branch"] = null;
                Session["Warehouse_Data"] = null;
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
                BranchHoOffice();
                Session["BranchNames"] = null;

                if (ddlValTech.SelectedValue == "F")
                {
                    grivaluation.Columns[9].Visible = true;
                    grivaluation.Columns[10].Visible = true;
                    grivaluation.Columns[11].Visible = true;
                    grivaluation.Columns[12].Visible = true;
                    grivaluation.Columns[13].Visible = false;
                    grivaluation.Columns[14].Visible = false;
                    grivaluation.Columns[15].Visible = false;
                    grivaluation.Columns[16].Visible = false;
                    grivaluation.Columns[17].Visible = false;
                    grivaluation.Columns[18].Visible = false;
                    grivaluation.Columns[19].Visible = false;
                    grivaluation.Columns[20].Visible = false;
                    grivaluation.Columns[21].Visible = false;
                    grivaluation.Columns[22].Visible = false;
                }
                else
                {
                    grivaluation.Columns[9].Visible = false;
                    grivaluation.Columns[10].Visible = false;
                    grivaluation.Columns[11].Visible = false;
                    grivaluation.Columns[12].Visible = false;
                    grivaluation.Columns[13].Visible = true;
                    grivaluation.Columns[14].Visible = true;
                    grivaluation.Columns[15].Visible = true;
                    grivaluation.Columns[16].Visible = true;
                    grivaluation.Columns[17].Visible = true;
                    grivaluation.Columns[18].Visible = true;
                    grivaluation.Columns[19].Visible = true;
                    grivaluation.Columns[20].Visible = true;
                    grivaluation.Columns[21].Visible = true;
                    grivaluation.Columns[22].Visible = true;
                }
            }
            else
            {
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);
            }
        }
        protected void Page_PreInit(object sender, EventArgs e)
        {

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
            string filename = "WHwiseStockValuationSummary";
            exporter.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Warehouse wise Stock Valuation Summary" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
            FileHeader = ReplaceFirst(FileHeader, "\r\n", Convert.ToString(Session["BranchNames"]));
            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";

            //exporter.PageFooter.Center = "[Page # of Pages #]";
            //exporter.PageFooter.Right = "[Date Printed]";
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

        protected void exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            e.BrickStyle.BackColor = Color.White;
            e.BrickStyle.ForeColor = Color.Black;
        }

        #endregion

        #region Export Valuation Details
        public void cmbExport1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(ddldetails.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval1"] == null)
                {
                    Session["exportval1"] = Filter;
                    bindexport1(Filter);
                }
                else if (Convert.ToInt32(Session["exportval1"]) != Filter)
                {
                    Session["exportval1"] = Filter;
                    bindexport1(Filter);
                }
            }
        }

        public void bindexport1(int Filter)
        {
            string filename = "WHwise StockValuationdetails";
            exporter.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();
            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Warehouse wise Stock Valuation Detail" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");

            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";
            exporter.Landscape = true;
            exporter.MaxColumnWidth = 100;
            exporter.GridViewID = "grivaluation";
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

        #region =======================Valuation Summary =========================
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string IsWHwiseStockValFilter = Convert.ToString(hfIsWHwiseStockValFilter.Value);
            Session["IsWHwiseStockValFilter"] = IsWHwiseStockValFilter;

            string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);

            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");

            string BRANCH_ID = "";

            string QuoComponent2 = "";
            List<object> QuoList2 = lookup_branch.GridView.GetSelectedFieldValues("ID");
            foreach (object Quo2 in QuoList2)
            {
                QuoComponent2 += "," + Quo2;
            }
            BRANCH_ID = QuoComponent2.TrimStart(',');

            string WH_ID = "";

            string WHID = "";
            List<object> WhidList = lookup_warehouse.GridView.GetSelectedFieldValues("ID");
            foreach (object WH in WhidList)
            {
                WHID += "," + WH;
            }
            WH_ID = WHID.TrimStart(',');

            string Product = "";
            Product = hdncWiseProductId.Value;

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

            Task PopulateStockTrialDataTask = new Task(() => GetWHwiseStockValuationdata(TODATE, BRANCH_ID, WH_ID,Product));
            PopulateStockTrialDataTask.RunSynchronously();

        }

        public void GetWHwiseStockValuationdata(string TODATE, string BRANCH_ID, string WHID, string ProductIds)
        {
            try
            {
                string strClassList = "", strBrandList = "";
                strClassList = hdnClassId.Value;
                strBrandList = hdnBranndId.Value;

                DataTable ds = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("PRC_WAREHOUSEWISESTOCKVALUATION_REPORT");
                proc.AddPara("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                proc.AddPara("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                proc.AddPara("@TODATE", TODATE);
                proc.AddPara("@BRANCHID", BRANCH_ID);
                proc.AddPara("@PRODUCT_ID", ProductIds);
                proc.AddPara("@VAL_TYPE", ddlValTech.SelectedValue);
                proc.AddPara("@GETTYPE", "Summary");
                proc.AddPara("@Class", strClassList);
                proc.AddPara("@Brand", strBrandList);
                proc.AddPara("@OWMASTERVALTECH", (chkOWMSTVT.Checked) ? "1" : "0");
                proc.AddPara("@WAREHOUSE_ID", WHID);
                proc.AddPara("@CONSOPASONDATE", (chkConsopasondt.Checked) ? "1" : "0");
                proc.AddPara("@CONSLANDEDCOST", (chkConsLandCost.Checked) ? "1" : "0");
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
                ASPxFromDate.MinDate = Convert.ToDateTime((dtstkval.Rows[0]["FinYear_StartDate"]));

                ASPxToDate.MaxDate = Convert.ToDateTime((dtstkval.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((dtstkval.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((dtstkval.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((dtstkval.Rows[0]["FinYear_StartDate"]));

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

        #region =====================Valuation Details===========================
        protected void CallbackPanelDetail_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string IsWHwiseStockValDetFilter = Convert.ToString(hfIsWHwiseStockValDetFilter.Value);
            Session["IsWHwiseStockValDetFilter"] = IsWHwiseStockValDetFilter;

            string returnPara = Convert.ToString(e.Parameter);
            string WhichCall = returnPara.Split('~')[0];

            if (WhichCall == "BndPopupgrid")
            {
                string branchid = returnPara.Split('~')[1];
                string whid = returnPara.Split('~')[2];
                string prodId = returnPara.Split('~')[3];
                GetWHwisevaluationDet(branchid, whid, prodId);
            }

        }

        public void GetWHwisevaluationDet(string Branchid, string Whid, string ProductId)
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
                ProcedureExecute proc = new ProcedureExecute("PRC_WAREHOUSEWISESTOCKVALUATION_REPORT");
                proc.AddPara("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                proc.AddPara("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                proc.AddPara("@TODATE", TODATE);
                proc.AddPara("@BRANCHID", Branchid);
                proc.AddPara("@PRODUCT_ID", ProductId);
                proc.AddPara("@VAL_TYPE", ddlValTech.SelectedValue);
                proc.AddPara("@GETTYPE", "Details");
                proc.AddPara("@Class", strClassList);
                proc.AddPara("@Brand", strBrandList);
                proc.AddPara("@OWMASTERVALTECH", (chkOWMSTVT.Checked) ? "1" : "0");
                proc.AddPara("@WAREHOUSE_ID", Whid);
                proc.AddPara("@CONSOPASONDATE", (chkConsopasondt.Checked) ? "1" : "0");
                proc.AddPara("@CONSLANDEDCOST", (chkConsLandCost.Checked) ? "1" : "0");
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
            e.Text = string.Format("{0}", e.Value);
        }
        #endregion


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

        #region Warehouse Populate

        protected void Componentwarehouse_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string FinYear = Convert.ToString(Session["LastFinYear"]);

            if (e.Parameter.Split('~')[0] == "BindWarehouseGrid")
            {
                DataTable WarehouseTable = new DataTable();
                WarehouseTable = GetWarehouse();

                if (WarehouseTable.Rows.Count > 0)
                {
                    Session["Warehouse_Data"] = WarehouseTable;
                    lookup_warehouse.DataSource = WarehouseTable;
                    lookup_warehouse.DataBind();
                }
                else
                {
                    Session["Warehouse_Data"] = WarehouseTable;
                    lookup_warehouse.DataSource = null;
                    lookup_warehouse.DataBind();
                }
            }
        }

        public DataTable GetWarehouse()
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("PRC_WAREHOUSESELECTION_REPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            cmd.Dispose();
            con.Dispose();

            return dt;
        }

        protected void lookup_warehouse_DataBinding(object sender, EventArgs e)
        {
            if (Session["Warehouse_Data"] != null)
            {
                lookup_warehouse.DataSource = (DataTable)Session["Warehouse_Data"];
            }
        }

        #endregion

        #region LinQ
        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SLNO";
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsWHwiseStockValFilter"]) == "Y")
            {
                var q = from d in dc.WAREHOUSEWISESTOCKVALUATION_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.REPORTTYPE) == "Summary"
                        orderby d.SLNO
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.WAREHOUSEWISESTOCKVALUATION_REPORTs
                        where Convert.ToString(d.SLNO) == "0"
                        orderby d.SLNO
                        select d;
                e.QueryableSource = q;
            }
        }
        protected void GenerateEntityServerDetailsModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SLNO";
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsWHwiseStockValDetFilter"]) == "Y")
            {
                var q = from d in dc.WAREHOUSEWISESTOCKVALUATION_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.REPORTTYPE) == "Details"
                        orderby d.SLNO
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.WAREHOUSEWISESTOCKVALUATION_REPORTs
                        where Convert.ToString(d.SLNO) == "0"
                        orderby d.SLNO
                        select d;
                e.QueryableSource = q;
            }

            if (ddlValTech.SelectedValue == "F")
            {
                grivaluation.Columns[9].Visible = true;
                grivaluation.Columns[10].Visible = true;
                grivaluation.Columns[11].Visible = true;
                grivaluation.Columns[12].Visible = true;
                grivaluation.Columns[13].Visible = false;
                grivaluation.Columns[14].Visible = false;
                grivaluation.Columns[15].Visible = false;
                grivaluation.Columns[16].Visible = false;
                grivaluation.Columns[17].Visible = false;
                grivaluation.Columns[18].Visible = false;
                grivaluation.Columns[19].Visible = false;
                grivaluation.Columns[20].Visible = false;
                grivaluation.Columns[21].Visible = false;
                grivaluation.Columns[22].Visible = false;
            }
            else
            {
                grivaluation.Columns[9].Visible = false;
                grivaluation.Columns[10].Visible = false;
                grivaluation.Columns[11].Visible = false;
                grivaluation.Columns[12].Visible = false;
                grivaluation.Columns[13].Visible = true;
                grivaluation.Columns[14].Visible = true;
                grivaluation.Columns[15].Visible = true;
                grivaluation.Columns[16].Visible = true;
                grivaluation.Columns[17].Visible = true;
                grivaluation.Columns[18].Visible = true;
                grivaluation.Columns[19].Visible = true;
                grivaluation.Columns[20].Visible = true;
                grivaluation.Columns[21].Visible = true;
                grivaluation.Columns[22].Visible = true;
            }
        }
        #endregion
    }
}