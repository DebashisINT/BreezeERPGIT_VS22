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
    public partial class BOMRegisterDetails : System.Web.UI.Page
    {
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        CommonBL cbl = new CommonBL();

        ExcelFile objExcel = new ExcelFile();
        DataTable dtExport = new DataTable();
        DataTable dtReportHeader = new DataTable();
        DataTable dtReportFooter = new DataTable();

        DataTable dtBOMTotal = null;
        string BOMTotalBalDesc = "";
        string BOMFGQty = "";
        string BOMStkQty = "";
        string BOMAmt = "";
        string BOMAddCost = "";
        string BOMStandCost = "";
        string BOMActCost = "";

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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/BOMRegisterDetails.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");
            if (!String.IsNullOrEmpty(ProjectSelectInEntryModule))
            {
                if (ProjectSelectInEntryModule == "Yes")
                {
                    lookup_project.Visible = true;
                    Label2.Visible = true;
                    ShowGrid.Columns[1].Visible = true;
                    hdnProjectSelection.Value = "1";

                }
                else if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                {
                    lookup_project.Visible = false;
                    Label2.Visible = false;
                    ShowGrid.Columns[1].Visible = false;
                    hdnProjectSelection.Value = "0";
                }
            }
            if (!IsPostBack)
            {
                DataTable dtBranchSelection = new DataTable();
                DataTable dtProjectSelection = new DataTable();
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "BOM Register - Detail";
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

                Session["SI_ComponentData_Branch"] = null;
                Session["IsBOMRegDetSumFilter"] = null;
                BranchHoOffice();

                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;

                ASPxFromDate.Value = DateTime.Now;
                ASPxToDate.Value = DateTime.Now;
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                Session["BranchNames"] = null;
                dtBranchSelection = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='ReportsBranchSelection'");
                hdnBranchSelection.Value = dtBranchSelection.Rows[0][0].ToString();

                dtProjectSelection = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='ReportsProjectSelection'");
                hdnProjectSelectionInReport.Value = dtProjectSelection.Rows[0][0].ToString();
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
                string BRANCH_ID = "";
                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }
            }
        }

        #region Export
        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Convert.ToString(Session["IsBOMRegDetSumFilter"]) == "Y")
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

            string filename = "";
            if(chkShowSum.Checked==false)
            {
                filename = "BOMRegisterDetails";
            }
            else
            {
                filename = "BOMRegisterSummary";
            }
            exporter.FileName = filename;

            if (Filter == 1 || Filter == 2)
            {
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                con.Open();
                string selectQuery = "SELECT BRANCHDESC,PROJ_NAME,BOM_TYPE,BOM_NO,BOMDATE,BOMSTATUS,REV_NO,REVDATE,FGCODE,FGDESC,FGQTY,FGWHDESC,HREMARKS,PARTNO,PARTCODE,DRAWINGNO,DREVISIONNO,PRODWHDESC,RCCODE,RCDESC,STKQTY,STKUOM,PRICE,AMOUNT,REMARKS,ADDCOST,STANDARDCOST,ACTUALCOST,CREATEDBY FROM BOMREGISTERDETSUM_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND BRANCH_ID<>999999999999 AND BRANCHDESC<>'Total :' order by SEQ";
                SqlDataAdapter myCommand = new SqlDataAdapter(selectQuery, con);

                // Create and fill a DataSet.
                DataSet ds = new DataSet();
                myCommand.Fill(ds, "Main");
                myCommand = new SqlDataAdapter("Select BRANCHDESC,FGQTY,STKQTY,AMOUNT,ADDCOST,STANDARDCOST,ACTUALCOST from BOMREGISTERDETSUM_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND BRANCH_ID=999999999999 AND BRANCHDESC='Total :'", con);
                myCommand.Fill(ds, "GrossTotal");
                myCommand.Dispose();
                con.Dispose();
                Session["exportbomregdetdataset"] = ds;

                string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");

                dtExport = ds.Tables[0].Copy();
                dtExport.Clear();
                dtExport.Columns.Add(new DataColumn("Unit", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Project Name", typeof(string)));
                dtExport.Columns.Add(new DataColumn("BOM Type", typeof(string)));
                dtExport.Columns.Add(new DataColumn("BOM#", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Date", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Status", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Revision No.", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Revision Date", typeof(string)));
                dtExport.Columns.Add(new DataColumn("FG Code", typeof(string)));
                dtExport.Columns.Add(new DataColumn("FG Description", typeof(string)));
                dtExport.Columns.Add(new DataColumn("FG Qty.", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("FG Warehouse", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Header Remarks", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Part No.", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Description", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Drawing No.", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Drawing Rev. No.", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Warehouse", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Resource", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Resource Desc.", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Qty.", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("UOM", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Price", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Amount", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Remarks", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Additional Cost", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Standard Cost", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Actual Cost", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Created by", typeof(string)));

                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {
                    DataRow row2 = dtExport.NewRow();

                    row2["Unit"] = dr1["BRANCHDESC"];
                    row2["Project Name"] = dr1["PROJ_NAME"];
                    row2["BOM Type"] = dr1["BOM_TYPE"];
                    row2["BOM#"] = dr1["BOM_NO"];
                    row2["Date"] = dr1["BOMDATE"];
                    row2["Status"] = dr1["BOMSTATUS"];
                    row2["Revision No."] = dr1["REV_NO"];
                    row2["Revision Date"] = dr1["REVDATE"];
                    row2["FG Code"] = dr1["FGCODE"];
                    row2["FG Description"] = dr1["FGDESC"];
                    row2["FG Qty."] = dr1["FGQTY"];
                    row2["FG Warehouse"] = dr1["FGWHDESC"];
                    row2["Header Remarks"] = dr1["HREMARKS"];
                    row2["Part No."] = dr1["PARTNO"];
                    row2["Description"] = dr1["PARTCODE"];
                    row2["Drawing No."] = dr1["DRAWINGNO"];
                    row2["Drawing Rev. No."] = dr1["DREVISIONNO"];
                    row2["Warehouse"] = dr1["PRODWHDESC"];
                    row2["Resource"] = dr1["RCCODE"];
                    row2["Resource Desc."] = dr1["RCDESC"];
                    row2["Qty."] = dr1["STKQTY"];
                    row2["UOM"] = dr1["STKUOM"];
                    row2["Price"] = dr1["PRICE"];
                    row2["Amount"] = dr1["AMOUNT"];
                    row2["Remarks"] = dr1["REMARKS"];
                    row2["Additional Cost"] = dr1["ADDCOST"];
                    row2["Standard Cost"] = dr1["STANDARDCOST"];
                    row2["Actual Cost"] = dr1["ACTUALCOST"];
                    row2["Created by"] = dr1["CREATEDBY"];

                    dtExport.Rows.Add(row2);
                }

                if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                    dtExport.Columns.Remove("Project Name");

                if(chkShowSum.Checked==true)
                {
                    dtExport.Columns.Remove("Part No.");
                    dtExport.Columns.Remove("Description");
                    dtExport.Columns.Remove("Drawing No.");
                    dtExport.Columns.Remove("Drawing Rev. No.");
                    dtExport.Columns.Remove("Warehouse");
                    dtExport.Columns.Remove("Resource");
                    dtExport.Columns.Remove("Resource Desc.");
                    dtExport.Columns.Remove("Qty.");
                    dtExport.Columns.Remove("UOM");
                    dtExport.Columns.Remove("Price");
                    dtExport.Columns.Remove("Amount");
                    dtExport.Columns.Remove("Remarks");
                }

                if(chkShowCost.Checked==false)
                {
                    dtExport.Columns.Remove("Standard Cost");
                    dtExport.Columns.Remove("Actual Cost");
                }

                if (chkAddCost.Checked == false)
                    dtExport.Columns.Remove("Additional Cost");

                if (chkRevNoDt.Checked == false)
                {
                    dtExport.Columns.Remove("Revision No.");
                    dtExport.Columns.Remove("Revision Date");
                }

                if (chkFGWH.Checked == false)
                   dtExport.Columns.Remove("FG Warehouse");

                if (chkHeadRem.Checked == false)
                    dtExport.Columns.Remove("Header Remarks");

                if(chkStatus.Checked==false)
                    dtExport.Columns.Remove("Status");

                if (chkCreateBy.Checked == false)
                    dtExport.Columns.Remove("Created by");

                dtExport.Columns.Remove("BRANCHDESC");
                dtExport.Columns.Remove("PROJ_NAME");
                dtExport.Columns.Remove("BOM_TYPE");
                dtExport.Columns.Remove("BOM_NO");
                dtExport.Columns.Remove("BOMDATE");
                dtExport.Columns.Remove("BOMSTATUS");
                dtExport.Columns.Remove("REV_NO");
                dtExport.Columns.Remove("REVDATE");
                dtExport.Columns.Remove("FGCODE");
                dtExport.Columns.Remove("FGDESC");
                dtExport.Columns.Remove("FGQTY");
                dtExport.Columns.Remove("FGWHDESC");
                dtExport.Columns.Remove("HREMARKS");
                dtExport.Columns.Remove("PARTNO");
                dtExport.Columns.Remove("PARTCODE");
                dtExport.Columns.Remove("DRAWINGNO");
                dtExport.Columns.Remove("DREVISIONNO");
                dtExport.Columns.Remove("PRODWHDESC");
                dtExport.Columns.Remove("RCCODE");
                dtExport.Columns.Remove("RCDESC");
                dtExport.Columns.Remove("STKQTY");
                dtExport.Columns.Remove("STKUOM");
                dtExport.Columns.Remove("PRICE");
                dtExport.Columns.Remove("AMOUNT");
                dtExport.Columns.Remove("REMARKS");
                dtExport.Columns.Remove("ADDCOST");
                dtExport.Columns.Remove("STANDARDCOST");
                dtExport.Columns.Remove("ACTUALCOST");
                dtExport.Columns.Remove("CREATEDBY");

                DataRow row3 = dtExport.NewRow();
                row3["Unit"] = ds.Tables[1].Rows[0]["BRANCHDESC"].ToString();
                row3["FG Qty."] = ds.Tables[1].Rows[0]["FGQTY"].ToString();
                if (chkShowSum.Checked == false)
                {
                    row3["Qty."] = ds.Tables[1].Rows[0]["STKQTY"].ToString();
                    row3["Amount"] = ds.Tables[1].Rows[0]["AMOUNT"].ToString();
                }
                if (chkAddCost.Checked == true)
                    row3["Additional Cost"] = ds.Tables[1].Rows[0]["ADDCOST"].ToString();
                if(chkShowCost.Checked==true)
                {
                    row3["Standard Cost"] = ds.Tables[1].Rows[0]["STANDARDCOST"].ToString();
                    row3["Actual Cost"] = ds.Tables[1].Rows[0]["ACTUALCOST"].ToString();
                }
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
                HeaderRow6[0] = "BOM Register - Detail";
                dtReportHeader.Rows.Add(HeaderRow6);
                DataRow HeaderRow7 = dtReportHeader.NewRow();
                HeaderRow7[0] = "For the period: " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
                dtReportHeader.Rows.Add(HeaderRow7);

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
                    objExcel.ExportToExcelforExcel(dtExport, filename, "ZZZZZZZZZZZZZZZZ", "ZZZZZZZZZZZZZZZZ", dtReportHeader, dtReportFooter);
                    break;
                case 2:
                    objExcel.ExportToPDF(dtExport, filename, "ZZZZZZZZZZZZZZZZ", "ZZZZZZZZZZZZZZZZ", dtReportHeader, dtReportFooter);
                    break;
                case 3:
                    exporter.WriteCsvToResponse();
                    break;
                default:
                    return;
            }

        }

        #endregion

        #region BOM Register Detail grid
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string returnPara = Convert.ToString(e.Parameter);
            string HEAD_BRANCH = returnPara.Split('~')[1];

            string IsBOMRegDetSumFilter = Convert.ToString(hfIsBOMRegDetSumFilter.Value);
            Session["IsBOMRegDetSumFilter"] = IsBOMRegDetSumFilter;

            DateTime dtFrom;
            DateTime dtTo;

            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);

            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");

            string BRANCH_ID = "";
            string BranchComponent = "";
            List<object> BranchList = lookup_branch.GridView.GetSelectedFieldValues("ID");
            foreach (object branch in BranchList)
            {
                BranchComponent += "," + branch;
            }
            BRANCH_ID = BranchComponent.TrimStart(',');

            string PROJECT_ID = "";
            string Projects = "";
            List<object> ProjectList = lookup_project.GridView.GetSelectedFieldValues("ID");
            foreach (object Project in ProjectList)
            {
                Projects += "," + Project;
            }
            PROJECT_ID = Projects.TrimStart(',');

            string WH_ID = "";

            string WHID = "";
            List<object> WhidList = lookup_warehouse.GridView.GetSelectedFieldValues("ID");
            foreach (object WH in WhidList)
            {
                WHID += "," + WH;
            }
            WH_ID = WHID.TrimStart(',');

            string ReportType = "";
            if(chkShowSum.Checked==false)
            {
                ReportType = "Details";
            }
            else
            {
                ReportType = "Summary";
            }

            string BRANCH_NAME = "";
            string BranchNameComponent = "";
            List<object> BranchNameList = lookup_branch.GridView.GetSelectedFieldValues("branch_description");
            foreach (object BranchName in BranchNameList)
            {
                BranchNameComponent += "," + BranchName;
            }
            if (BranchNameList.Count > 1 || BranchNameList.Count == 0)
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

            Task PopulateStockTrialDataTask = new Task(() => GetBOMRegisterdata(FROMDATE, TODATE, BRANCH_ID, WH_ID, PROJECT_ID, ReportType));
            PopulateStockTrialDataTask.RunSynchronously();
        }
        public void GetBOMRegisterdata(string FROMDATE, string TODATE, string BRANCH_ID, string WH_ID, string PROJECT_ID, string ReportType)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_BOMREGISTERDETSUM_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                cmd.Parameters.AddWithValue("@TODATE", TODATE);
                cmd.Parameters.AddWithValue("@BRANCH_ID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@BOMID", hdnBOMId.Value);
                cmd.Parameters.AddWithValue("@PRODUCT_ID", hdnProductId.Value);
                cmd.Parameters.AddWithValue("@PROJECT_ID", PROJECT_ID);
                cmd.Parameters.AddWithValue("@WAREHOUSE_ID", WH_ID);
                cmd.Parameters.AddWithValue("@BOMTYPE", ddlBOMType.SelectedValue);
                cmd.Parameters.AddWithValue("@SHOWCREATEBY", (chkCreateBy.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@SHOWFGWH", (chkFGWH.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@SHOWCOST", (chkShowCost.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@REPORTTYPE", ReportType);
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
            if (Convert.ToString(Session["IsBOMRegDetSumFilter"]) == "Y")
            {
                dtBOMTotal = oDBEngine.GetDataTable("Select BRANCHDESC,FGQTY,STKQTY,AMOUNT,ADDCOST,STANDARDCOST,ACTUALCOST from BOMREGISTERDETSUM_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND BRANCH_ID=999999999999 AND BRANCHDESC='Total :'");
                BOMTotalBalDesc = dtBOMTotal.Rows[0][0].ToString();
                BOMFGQty = dtBOMTotal.Rows[0][1].ToString();
                BOMStkQty = dtBOMTotal.Rows[0][2].ToString();
                BOMAmt = dtBOMTotal.Rows[0][3].ToString();
                BOMAddCost = dtBOMTotal.Rows[0][4].ToString();
                BOMStandCost = dtBOMTotal.Rows[0][5].ToString();
                BOMActCost = dtBOMTotal.Rows[0][6].ToString();
            }

            string summaryTAG = (e.Item as ASPxSummaryItem).Tag;
            if (e.IsTotalSummary == true)
            {
                switch (summaryTAG)
                {
                    case "Branch":
                        e.Text = BOMTotalBalDesc;
                        break;
                    case "FGQty":
                        e.Text = BOMFGQty;
                        break;
                    case "StkQty":
                        e.Text = BOMStkQty;
                        break;
                    case "Amt":
                        e.Text = BOMAmt;
                        break;
                    case "AddCost":
                        e.Text = BOMAddCost;
                        break;
                    case "StandCost":
                        e.Text = BOMStandCost;
                        break;
                    case "ActualCost":
                        e.Text = BOMActCost;
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

            if (Convert.ToString(Session["IsBOMRegDetSumFilter"]) == "Y")
            {
                var q = from d in dc.BOMREGISTERDETSUM_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.BRANCH_ID) != "999999999999" && Convert.ToString(d.BRANCHDESC) != "Total :"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.BOMREGISTERDETSUM_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }

            if(chkShowSum.Checked==true)
            {
                ShowGrid.Columns[13].Visible = false;
                ShowGrid.Columns[14].Visible = false;
                ShowGrid.Columns[15].Visible = false;
                ShowGrid.Columns[16].Visible = false;
                ShowGrid.Columns[17].Visible = false;
                ShowGrid.Columns[18].Visible = false;
                ShowGrid.Columns[19].Visible = false;
                ShowGrid.Columns[20].Visible = false;
                ShowGrid.Columns[21].Visible = false;
                ShowGrid.Columns[22].Visible = false;
                ShowGrid.Columns[23].Visible = false;
                ShowGrid.Columns[24].Visible = false;
            }
            else
            {
                ShowGrid.Columns[13].Visible = true;
                ShowGrid.Columns[14].Visible = true;
                ShowGrid.Columns[15].Visible = true;
                ShowGrid.Columns[16].Visible = true;
                ShowGrid.Columns[17].Visible = true;
                ShowGrid.Columns[18].Visible = true;
                ShowGrid.Columns[19].Visible = true;
                ShowGrid.Columns[20].Visible = true;
                ShowGrid.Columns[21].Visible = true;
                ShowGrid.Columns[22].Visible = true;
                ShowGrid.Columns[23].Visible = true;
                ShowGrid.Columns[24].Visible = true;
            }

            if(chkShowCost.Checked==true)
            {
                ShowGrid.Columns[26].Visible = true;
                ShowGrid.Columns[27].Visible = true;
            }
            else
            {
                ShowGrid.Columns[26].Visible = false;
                ShowGrid.Columns[27].Visible = false;
            }

            if(chkStatus.Checked==true)
            {
                ShowGrid.Columns[5].Visible = true;
            }
            else
            {
                ShowGrid.Columns[5].Visible = false;
            }

            if (chkAddCost.Checked == true)
            {
                ShowGrid.Columns[25].Visible = true;
            }
            else
            {
                ShowGrid.Columns[25].Visible = false;
            }

            if (chkRevNoDt.Checked == true)
            {
                ShowGrid.Columns[6].Visible = true;
                ShowGrid.Columns[7].Visible = true;
            }
            else
            {
                ShowGrid.Columns[6].Visible = false;
                ShowGrid.Columns[7].Visible = false;
            }

            if (chkFGWH.Checked == true)
            {
                ShowGrid.Columns[11].Visible = true;
            }
            else
            {
                ShowGrid.Columns[11].Visible = false;
            }

            if (chkHeadRem.Checked == true)
            {
                ShowGrid.Columns[12].Visible = true;
            }
            else
            {
                ShowGrid.Columns[12].Visible = false;
            }

            if (chkCreateBy.Checked == true)
            {
                ShowGrid.Columns[28].Visible = true;
            }
            else
            {
                ShowGrid.Columns[28].Visible = false;
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
                else
                {
                    ComponentTable = oDBEngine.GetDataTable("select * from (select branch_id as ID,branch_description,branch_code from tbl_master_branch a where a.branch_id=1  union all select branch_id as ID,branch_description,branch_code from tbl_master_branch b where b.branch_parentId=1) a order by branch_description");

                    if (ComponentTable.Rows.Count > 0)
                    {
                        Session["SI_ComponentData_Branch"] = ComponentTable;
                        lookup_branch.DataSource = ComponentTable;
                        lookup_branch.DataBind();
                    }
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

        #region Project Populate
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

    }
}