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
    public partial class PhysicalStockVsStockAvailability : System.Web.UI.Page
    {
        DateTime dtFrom;
        DateTime dtTo;
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        DataTable dtProductTotal = null;
        string ProductTotalPSPcs = "";
        string ProductTotalPSMeter = "";
        string ProductTotalPSWt = "";
        string ProductTotalSOPcs = "";
        string ProductTotalSOMeter = "";
        string ProductTotalSOWt = "";
        string ProductTotalSQAPcs = "";
        string ProductTotalSQAMeter = "";
        string ProductTotalSQAWt = "";
        string ProductTotalPOPcs = "";
        string ProductTotalPOMeter = "";
        string ProductTotalPOWt = "";
        string ProductTotalDesc = "";

        DataTable dtSOTotal = null;
        string SOTotalASOPcs = "";
        string SOTotalASOMeter = "";
        string SOTotalASOWt = "";
        string SOTotalMSOPcs = "";
        string SOTotalMSOMeter = "";
        string SOTotalMSOWt = "";
        string SOTotalBSOPcs = "";
        string SOTotalBSOMeter = "";
        string SOTotalBSOWt = "";
        string SOTotalASOValue = "";
        string SOTotalMSOValue = "";
        string SOTotalBSOValue = "";
        string SOTotalDesc = "";

        DataTable dtPOTotal = null;
        string POTotalAPOPcs = "";
        string POTotalAPOMeter = "";
        string POTotalAPOWt = "";
        string POTotalMPOPcs = "";
        string POTotalMPOMeter = "";
        string POTotalMPOWt = "";
        string POTotalBPOPcs = "";
        string POTotalBPOMeter = "";
        string POTotalBPOWt = "";
        string POTotalAPOValue = "";
        string POTotalMPOValue = "";
        string POTotalBPOValue = "";
        string POTotalDesc = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/PhysicalStockVsStockAvailability.aspx");
            if (!IsPostBack)
            {
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Physical Stock Vs Stock Availability";
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
                Session["IsPhyStockVsStockAvailSummaryFilter"] = null;
                Session["SI_ComponentData_Branch"] = null;
                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");
                ASPxToDate.Value = DateTime.Now;

                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                string TODATE = dtTo.ToString("yyyy-MM-dd");

                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                BranchHoOffice();
                Session["BranchNames"] = null;
            }
            else
            {
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

        #region Export Summary
        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Convert.ToString(Session["IsPhyStockVsStockAvailSummaryFilter"]) == "Y")
            {
                if (Filter != 0)
                {
                    bindexport(Filter);
                }
            }
            else
            {
                BranchHoOffice();
            }
        }

        public void bindexport(int Filter)
        {
            string filename = "PhyStockVsStockAvailSummary";
            exporter.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Physical Stock Vs Stock Availability" + Environment.NewLine + "As On: " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
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

        #region Export SO Details
        public void cmbExportSODet_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(ddlSOdetails.SelectedItem.Value));
            if (Convert.ToString(Session["IsPhyStockVsStockAvailSOFilter"]) == "Y")
            {
                if (Filter != 0)
                {
                    bindSOexport(Filter);
                }
            }
            else
            {
                BranchHoOffice();
            }
        }

        public void bindSOexport(int Filter)
        {
            string filename = "PhyStockVsStockAvailSODetails";
            exporter.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Sales Order - Detail" + Environment.NewLine + "As On: " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
            FileHeader = SOReplaceFirst(FileHeader, "\r\n", Convert.ToString(Session["BranchNames"]));
            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";

            //exporter.PageFooter.Center = "[Page # of Pages #]";
            //exporter.PageFooter.Right = "[Date Printed]";
            exporter.Landscape = true;
            exporter.MaxColumnWidth = 100;
            exporter.GridViewID = "GridSODetails";
            exporter.RenderBrick += SOexporter_RenderBrick;
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

        public string SOReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + Environment.NewLine + replace + Environment.NewLine + text.Substring(pos + search.Length);
        }

        protected void SOexporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            e.BrickStyle.BackColor = Color.White;
            e.BrickStyle.ForeColor = Color.Black;
        }

        #endregion

        #region Export PO Details
        public void cmbExportPODet_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(ddlPOdetails.SelectedItem.Value));
            if (Convert.ToString(Session["IsPhyStockVsStockAvailPOFilter"]) == "Y")
            {
                if (Filter != 0)
                {
                    bindPOexport(Filter);
                }
            }
            else
            {
                BranchHoOffice();
            }
        }

        public void bindPOexport(int Filter)
        {
            string filename = "PhyStockVsStockAvailPODetails";
            exporter.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Purchase Order - Detail" + Environment.NewLine + "As On: " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
            FileHeader = POReplaceFirst(FileHeader, "\r\n", Convert.ToString(Session["BranchNames"]));
            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";

            //exporter.PageFooter.Center = "[Page # of Pages #]";
            //exporter.PageFooter.Right = "[Date Printed]";
            exporter.Landscape = true;
            exporter.MaxColumnWidth = 100;
            exporter.GridViewID = "GridPODetails";
            exporter.RenderBrick += POexporter_RenderBrick;
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

        public string POReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + Environment.NewLine + replace + Environment.NewLine + text.Substring(pos + search.Length);
        }

        protected void POexporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            e.BrickStyle.BackColor = Color.White;
            e.BrickStyle.ForeColor = Color.Black;
        }

        #endregion

        #region =======================Physical Stock Vs Stock Availability Summary =========================
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string IsPhyStockVsStockAvailSummaryFilter = Convert.ToString(hfIsPhyStockVsStockAvailSummaryFilter.Value);
            Session["IsPhyStockVsStockAvailSummaryFilter"] = IsPhyStockVsStockAvailSummaryFilter;

            dtTo = Convert.ToDateTime(ASPxToDate.Date);

            string TODATE = dtTo.ToString("yyyy-MM-dd");

            string BRANCH_ID = "";

            string BranchList = "";
            List<object> BranchList1 = lookup_branch.GridView.GetSelectedFieldValues("ID");
            foreach (object Branch in BranchList1)
            {
                BranchList += "," + Branch;
            }
            BRANCH_ID = BranchList.TrimStart(',');

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

            Task PopulateStockTrialDataTask = new Task(() => GetPhyStockVsStockAvailSummarydata(TODATE, BRANCH_ID));
            PopulateStockTrialDataTask.RunSynchronously();

        }

        public void GetPhyStockVsStockAvailSummarydata(string TODATE, string BRANCH_ID)
        {
            try
            {
                DataTable ds = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("PRC_PHYSICALSTOCKVSSTOCKAVAILABILITY_REPORT");
                proc.AddPara("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                proc.AddPara("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                proc.AddPara("@ASONDATE", TODATE);
                proc.AddPara("@BRANCHID", BRANCH_ID);
                proc.AddPara("@PRODUCT_ID", hdncWiseProductId.Value);
                proc.AddPara("@CLASS", hdnClassId.Value);
                proc.AddPara("@BRAND", hdnBranndId.Value);
                proc.AddPara("@ISCONSOLIDATED", (chkIsConsolidated.Checked) ? "1" : "0");
                proc.AddPara("@REPORTTYPE", "Summary");
                proc.AddPara("@USERID", Convert.ToInt32(Session["userid"]));

                ds = proc.GetTable();
            }
            catch (Exception ex)
            {
            }
        }

        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            if (Convert.ToString(Session["IsPhyStockVsStockAvailSummaryFilter"]) == "Y")
            {
                dtProductTotal = oDBEngine.GetDataTable("SELECT PSPRODNAME,PCSMULTQTY,METERMULTQTY,PSSTOCKUOMQTY,SOPCSMULTQTY,SOMETERMULTQTY,SOSTOCKQTY,SQAPCSSTOCKQTY,SQAMETERSTOCKQTY,SQASTOCKQTY,POPCSMULTQTY,POMETERMULTQTY,POSTOCKQTY FROM PHYSICALSTOCKVSSTOCKAVAILABILITY_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND REPORTTYPE='Summary' AND PSPRODNAME='Gross Total :'");
                ProductTotalDesc = dtProductTotal.Rows[0][0].ToString();
                ProductTotalPSPcs = dtProductTotal.Rows[0][1].ToString();
                ProductTotalPSMeter = dtProductTotal.Rows[0][2].ToString();
                ProductTotalPSWt = dtProductTotal.Rows[0][3].ToString();
                ProductTotalSOPcs = dtProductTotal.Rows[0][4].ToString();
                ProductTotalSOMeter = dtProductTotal.Rows[0][5].ToString();
                ProductTotalSOWt = dtProductTotal.Rows[0][6].ToString();
                ProductTotalSQAPcs = dtProductTotal.Rows[0][7].ToString();
                ProductTotalSQAMeter = dtProductTotal.Rows[0][8].ToString();
                ProductTotalSQAWt = dtProductTotal.Rows[0][9].ToString();
                ProductTotalPOPcs = dtProductTotal.Rows[0][10].ToString();
                ProductTotalPOMeter = dtProductTotal.Rows[0][11].ToString();
                ProductTotalPOWt = dtProductTotal.Rows[0][12].ToString();
            }

            string summaryTAG = (e.Item as ASPxSummaryItem).Tag;
            if (e.IsTotalSummary == true)
            {
                switch (summaryTAG)
                {
                    case "Item_Name":
                        e.Text = ProductTotalDesc;
                        break;
                    case "PS_Pcs":
                        e.Text = ProductTotalPSPcs;
                        break;
                    case "PS_Meter":
                        e.Text = ProductTotalPSMeter;
                        break;
                    case "PS_Wt":
                        e.Text = ProductTotalPSWt;
                        break;
                    case "SO_Pcs":
                        e.Text = ProductTotalSOPcs;
                        break;
                    case "SO_Meter":
                        e.Text = ProductTotalSOMeter;
                        break;
                    case "SO_Wt":
                        e.Text = ProductTotalSOWt;
                        break;
                    case "SQA_Pcs":
                        e.Text = ProductTotalSQAPcs;
                        break;
                    case "SQA_Meter":
                        e.Text = ProductTotalSQAMeter;
                        break;
                    case "SQA_Wt":
                        e.Text = ProductTotalSQAWt;
                        break;
                    case "PO_Pcs":
                        e.Text = ProductTotalPOPcs;
                        break;
                    case "PO_Meter":
                        e.Text = ProductTotalPOMeter;
                        break;
                    case "PO_Wt":
                        e.Text = ProductTotalPOWt;
                        break;
                }
            }
        }
        #endregion

        public void Date_finyearwise(string Finyear)
        {
            CommonBL stkdb = new CommonBL();
            DataTable dtstkdb = new DataTable();

            dtstkdb = stkdb.GetDateFinancila(Finyear);
            if (dtstkdb.Rows.Count > 0)
            {
                ASPxToDate.MaxDate = Convert.ToDateTime((dtstkdb.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((dtstkdb.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((dtstkdb.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((dtstkdb.Rows[0]["FinYear_StartDate"]));

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

            if (Convert.ToString(Session["IsPhyStockVsStockAvailSummaryFilter"]) == "Y")
            {
                var q = from d in dc.PHYSICALSTOCKVSSTOCKAVAILABILITY_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.REPORTTYPE) == "Summary" && Convert.ToString(d.PSPRODNAME) != "Gross Total :"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.PHYSICALSTOCKVSSTOCKAVAILABILITY_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }

            string strisConsolidated = (chkIsConsolidated.Checked) ? "1" : "0";

            if (Convert.ToString(strisConsolidated) == "1")
            {
                ShowGrid.Columns[0].Visible = false;
            }
            else
            {
                ShowGrid.Columns[0].Visible = true;
            }
        }
        #endregion

        #region =====================Sales Order Detail===========================
        protected void CallbackPanelSODetail_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string IsPhyStockVsStockAvailSOFilter = Convert.ToString(hfIsPhyStockVsStockAvailSOFilter.Value);
            Session["IsPhyStockVsStockAvailSOFilter"] = IsPhyStockVsStockAvailSOFilter;

            string returnPara = Convert.ToString(e.Parameter);
            string WhichCall = returnPara.Split('~')[0];

            if (WhichCall == "BndSOPopupgrid")
            {
                string prodId = returnPara.Split('~')[1];
                string branchid = returnPara.Split('~')[2];
                GetSODetail(prodId, branchid);
            }

        }

        public void GetSODetail(string ProductIds, string BranchIds)
        {
            try
            {
                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                string TODATE = dtTo.ToString("yyyy-MM-dd");

                DataTable ds = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("PRC_PHYSICALSTOCKVSSTOCKAVAILABILITY_REPORT");
                proc.AddPara("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                proc.AddPara("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                proc.AddPara("@ASONDATE", TODATE);
                proc.AddPara("@BRANCHID", BranchIds);
                proc.AddPara("@PRODUCT_ID", ProductIds);
                proc.AddPara("@CLASS", hdnClassId.Value);
                proc.AddPara("@BRAND", hdnBranndId.Value);
                proc.AddPara("@ISCONSOLIDATED", (chkIsConsolidated.Checked) ? "1" : "0");
                proc.AddPara("@REPORTTYPE", "SO");
                proc.AddPara("@USERID", Convert.ToInt32(Session["userid"]));

                ds = proc.GetTable();
            }
            catch (Exception ex)
            {
            }
        }

        protected void GridSODetails_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            if (Convert.ToString(Session["IsPhyStockVsStockAvailSOFilter"]) == "Y")
            {
                dtSOTotal = oDBEngine.GetDataTable("SELECT PSPRODNAME,ACTUALPCSMULTQTY,ACTUALMETERMULTQTY,ACTUALSTOCKQTY,MATUREPCSQTY,MATUREMETERQTY,MATURESTOCKQTY,BALPCSQTY,BALMETERQTY,BALSTOCKQTY,ACTUAL_VALUES,MATURE_VALUES,BALANCE_VALUES FROM PHYSICALSTOCKVSSTOCKAVAILABILITY_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND REPORTTYPE='SO' AND PSPRODID=9999999999999999 AND PSPRODNAME='Gross Total :'");
                SOTotalDesc = dtSOTotal.Rows[0][0].ToString();
                SOTotalASOPcs = dtSOTotal.Rows[0][1].ToString();
                SOTotalASOMeter = dtSOTotal.Rows[0][2].ToString();
                SOTotalASOWt = dtSOTotal.Rows[0][3].ToString();
                SOTotalMSOPcs = dtSOTotal.Rows[0][4].ToString();
                SOTotalMSOMeter = dtSOTotal.Rows[0][5].ToString();
                SOTotalMSOWt = dtSOTotal.Rows[0][6].ToString();
                SOTotalBSOPcs = dtSOTotal.Rows[0][7].ToString();
                SOTotalBSOMeter = dtSOTotal.Rows[0][8].ToString();
                SOTotalBSOWt = dtSOTotal.Rows[0][9].ToString();
                SOTotalASOValue = dtSOTotal.Rows[0][10].ToString();
                SOTotalMSOValue = dtSOTotal.Rows[0][11].ToString();
                SOTotalBSOValue = dtSOTotal.Rows[0][12].ToString();
            }

            string summaryTAG = (e.Item as ASPxSummaryItem).Tag;
            if (e.IsTotalSummary == true)
            {
                switch (summaryTAG)
                {
                    case "SOItem_Name":
                        e.Text = SOTotalDesc;
                        break;
                    case "ASO_Pcs":
                        e.Text = SOTotalASOPcs;
                        break;
                    case "ASO_Meter":
                        e.Text = SOTotalASOMeter;
                        break;
                    case "ASO_Wt":
                        e.Text = SOTotalASOWt;
                        break;
                    case "MSO_Pcs":
                        e.Text = SOTotalMSOPcs;
                        break;
                    case "MSO_Meter":
                        e.Text = SOTotalMSOMeter;
                        break;
                    case "MSO_Wt":
                        e.Text = SOTotalMSOWt;
                        break;
                    case "BSO_Pcs":
                        e.Text = SOTotalBSOPcs;
                        break;
                    case "BSO_Meter":
                        e.Text = SOTotalBSOMeter;
                        break;
                    case "BSO_Wt":
                        e.Text = SOTotalBSOWt;
                        break;
                    case "ASO_Value":
                        e.Text = SOTotalASOValue;
                        break;
                    case "MSO_Value":
                        e.Text = SOTotalMSOValue;
                        break;
                    case "BSO_Value":
                        e.Text = SOTotalBSOValue;
                        break;
                }
            }
        }

        protected void GenerateEntityServerSODetailsModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsPhyStockVsStockAvailSOFilter"]) == "Y")
            {
                var q = from d in dc.PHYSICALSTOCKVSSTOCKAVAILABILITY_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.REPORTTYPE) == "SO" && Convert.ToString(d.PSPRODID) != "9999999999999999" && Convert.ToString(d.PSPRODNAME) != "Gross Total :"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.PHYSICALSTOCKVSSTOCKAVAILABILITY_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }

            string strisConsolidated = (chkIsConsolidated.Checked) ? "1" : "0";

            if (Convert.ToString(strisConsolidated) == "1")
            {
                GridSODetails.Columns[0].Visible = false;
            }
            else
            {
                GridSODetails.Columns[0].Visible = true;
            }
        }
        #endregion

        #region =====================Purchase Order Detail===========================
        protected void CallbackPanelPODetail_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string IsPhyStockVsStockAvailPOFilter = Convert.ToString(hfIsPhyStockVsStockAvailPOFilter.Value);
            Session["IsPhyStockVsStockAvailPOFilter"] = IsPhyStockVsStockAvailPOFilter;

            string returnPara = Convert.ToString(e.Parameter);
            string WhichCall = returnPara.Split('~')[0];

            if (WhichCall == "BndPOPopupgrid")
            {
                string prodId = returnPara.Split('~')[1];
                string branchid = returnPara.Split('~')[2];
                GetPODetail(prodId, branchid);
            }

        }

        public void GetPODetail(string ProductIds, string BranchIds)
        {
            try
            {
                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                string TODATE = dtTo.ToString("yyyy-MM-dd");

                DataTable ds = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("PRC_PHYSICALSTOCKVSSTOCKAVAILABILITY_REPORT");
                proc.AddPara("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                proc.AddPara("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                proc.AddPara("@ASONDATE", TODATE);
                proc.AddPara("@BRANCHID", BranchIds);
                proc.AddPara("@PRODUCT_ID", ProductIds);
                proc.AddPara("@CLASS", hdnClassId.Value);
                proc.AddPara("@BRAND", hdnBranndId.Value);
                proc.AddPara("@ISCONSOLIDATED", (chkIsConsolidated.Checked) ? "1" : "0");
                proc.AddPara("@REPORTTYPE", "PO");
                proc.AddPara("@USERID", Convert.ToInt32(Session["userid"]));

                ds = proc.GetTable();
            }
            catch (Exception ex)
            {
            }
        }

        protected void GridPODetails_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            if (Convert.ToString(Session["IsPhyStockVsStockAvailPOFilter"]) == "Y")
            {
                dtPOTotal = oDBEngine.GetDataTable("SELECT PSPRODNAME,ACTUALPCSMULTQTY,ACTUALMETERMULTQTY,ACTUALSTOCKQTY,MATUREPCSQTY,MATUREMETERQTY,MATURESTOCKQTY,BALPCSQTY,BALMETERQTY,BALSTOCKQTY,ACTUAL_VALUES,MATURE_VALUES,BALANCE_VALUES FROM PHYSICALSTOCKVSSTOCKAVAILABILITY_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND REPORTTYPE='PO' AND PSPRODID=9999999999999999 AND PSPRODNAME='Gross Total :'");
                POTotalDesc = dtPOTotal.Rows[0][0].ToString();
                POTotalAPOPcs = dtPOTotal.Rows[0][1].ToString();
                POTotalAPOMeter = dtPOTotal.Rows[0][2].ToString();
                POTotalAPOWt = dtPOTotal.Rows[0][3].ToString();
                POTotalMPOPcs = dtPOTotal.Rows[0][4].ToString();
                POTotalMPOMeter = dtPOTotal.Rows[0][5].ToString();
                POTotalMPOWt = dtPOTotal.Rows[0][6].ToString();
                POTotalBPOPcs = dtPOTotal.Rows[0][7].ToString();
                POTotalBPOMeter = dtPOTotal.Rows[0][8].ToString();
                POTotalBPOWt = dtPOTotal.Rows[0][9].ToString();
                POTotalAPOValue = dtPOTotal.Rows[0][10].ToString();
                POTotalMPOValue = dtPOTotal.Rows[0][11].ToString();
                POTotalBPOValue = dtPOTotal.Rows[0][12].ToString();
            }

            string summaryTAG = (e.Item as ASPxSummaryItem).Tag;
            if (e.IsTotalSummary == true)
            {
                switch (summaryTAG)
                {
                    case "POItem_Name":
                        e.Text = POTotalDesc;
                        break;
                    case "APO_Pcs":
                        e.Text = POTotalAPOPcs;
                        break;
                    case "APO_Meter":
                        e.Text = POTotalAPOMeter;
                        break;
                    case "APO_Wt":
                        e.Text = POTotalAPOWt;
                        break;
                    case "MPO_Pcs":
                        e.Text = POTotalMPOPcs;
                        break;
                    case "MPO_Meter":
                        e.Text = POTotalMPOMeter;
                        break;
                    case "MPO_Wt":
                        e.Text = POTotalMPOWt;
                        break;
                    case "BPO_Pcs":
                        e.Text = POTotalBPOPcs;
                        break;
                    case "BPO_Meter":
                        e.Text = POTotalBPOMeter;
                        break;
                    case "BPO_Wt":
                        e.Text = POTotalBPOWt;
                        break;
                    case "APO_Value":
                        e.Text = POTotalAPOValue;
                        break;
                    case "MPO_Value":
                        e.Text = POTotalMPOValue;
                        break;
                    case "BPO_Value":
                        e.Text = POTotalBPOValue;
                        break;
                }
            }
        }

        protected void GenerateEntityServerPODetailsModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsPhyStockVsStockAvailPOFilter"]) == "Y")
            {
                var q = from d in dc.PHYSICALSTOCKVSSTOCKAVAILABILITY_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.REPORTTYPE) == "PO" && Convert.ToString(d.PSPRODID) != "9999999999999999" && Convert.ToString(d.PSPRODNAME) != "Gross Total :"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.PHYSICALSTOCKVSSTOCKAVAILABILITY_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }

            string strisConsolidated = (chkIsConsolidated.Checked) ? "1" : "0";

            if (Convert.ToString(strisConsolidated) == "1")
            {
                GridPODetails.Columns[0].Visible = false;
            }
            else
            {
                GridPODetails.Columns[0].Visible = true;
            }
        }
        #endregion
    }
}