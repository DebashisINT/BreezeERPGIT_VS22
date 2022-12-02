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
    public partial class IssueForProductionRegister : System.Web.UI.Page
    {
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        CommonBL cbl = new CommonBL();

        ExcelFile objExcel = new ExcelFile();
        DataTable dtExport = new DataTable();
        DataTable dtReportHeader = new DataTable();
        DataTable dtReportFooter = new DataTable();

        DataTable dtPRDITotal = null;
        string PRDITotalBalDesc = "";
        string PRDIFGQty = "";
        string PRDIStkQty = "";
        string PRDIAmt = "";

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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/IssueForProductionRegister.aspx");
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
                RptHeading.Text = "Issue for Production Register - Detail";
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
                Session["IsPRDIRegDetSumFilter"] = null;
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
            if (Convert.ToString(Session["IsPRDIRegDetSumFilter"]) == "Y")
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
            filename = "PRDIRegisterDetails";
            exporter.FileName = filename;

            if (Filter == 1 || Filter == 2)
            {
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                con.Open();
                string selectQuery = "SELECT BRANCHDESC,PROJ_NAME,PRDINO,PRDIDATE,PONO,PODT,WONO,WODT,BOM_NO,BOM_DATE,REV_NO,REV_DATE,FGCODE,FGDESC,FGQTY,FGUOM,FGWHDESC,WCCODE,WCDESC,REMARKS,BOMPRDCODE,BOMPRDDESC,BOMPRDDNO,BOMPRDREVNO,MATCODE,MATDESC,DESIGNNO,DREVISIONNO,MATQTY,MATUOM,MATRATE,MATAMT,WAREHOUSENAME,CREATEDBY FROM MFISSUEFORPRODUCTIONREGISTERDETAIL_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND BRANCHID<>999999999999 AND BRANCHDESC<>'Total :' order by SEQ";
                SqlDataAdapter myCommand = new SqlDataAdapter(selectQuery, con);

                // Create and fill a DataSet.
                DataSet ds = new DataSet();
                myCommand.Fill(ds, "Main");
                myCommand = new SqlDataAdapter("Select BRANCHDESC,FGQTY,MATQTY,MATAMT from MFISSUEFORPRODUCTIONREGISTERDETAIL_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND BRANCHID=999999999999 AND BRANCHDESC='Total :'", con);
                myCommand.Fill(ds, "GrossTotal");
                myCommand.Dispose();
                con.Dispose();
                Session["exportprdiregdetdataset"] = ds;

                string ProjectSelectInEntryModule = cbl.GetSystemSettingsResult("ProjectSelectInEntryModule");

                dtExport = ds.Tables[0].Copy();
                dtExport.Clear();
                dtExport.Columns.Add(new DataColumn("Unit", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Project Name", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Issue#", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Date", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Production Order No.", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Production Order Date", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Work Order No.", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Work Order Date", typeof(string)));
                dtExport.Columns.Add(new DataColumn("BOM No.", typeof(string)));
                dtExport.Columns.Add(new DataColumn("BOM Date", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Revision No.", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Revision Date", typeof(string)));
                dtExport.Columns.Add(new DataColumn("FG Code", typeof(string)));
                dtExport.Columns.Add(new DataColumn("FG Description", typeof(string)));
                dtExport.Columns.Add(new DataColumn("FG Qty.", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("FG UOM", typeof(string)));
                dtExport.Columns.Add(new DataColumn("FG Warehouse", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Work Center Code", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Work Center Name", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Header Remarks", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Part No.", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Description", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Drawing No.", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Drawing Rev. No.", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Det. Part No.", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Det. Description", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Det. Drawing No.", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Det. Drawing Rev. No.", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Qty.", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("UOM", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Price", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Amount", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Warehouse", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Created by", typeof(string)));

                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {
                    DataRow row2 = dtExport.NewRow();

                    row2["Unit"] = dr1["BRANCHDESC"];
                    row2["Project Name"] = dr1["PROJ_NAME"];
                    row2["Issue#"] = dr1["PRDINO"];
                    row2["Date"] = dr1["PRDIDATE"];
                    row2["Production Order No."] = dr1["PONO"];
                    row2["Production Order Date"] = dr1["PODT"];
                    row2["Work Order No."] = dr1["WONO"];
                    row2["Work Order Date"] = dr1["WODT"];
                    row2["BOM No."] = dr1["BOM_NO"];
                    row2["BOM Date"] = dr1["BOM_DATE"];
                    row2["Revision No."] = dr1["REV_NO"];
                    row2["Revision Date"] = dr1["REV_DATE"];
                    row2["FG Code"] = dr1["FGCODE"];
                    row2["FG Description"] = dr1["FGDESC"];
                    row2["FG Qty."] = dr1["FGQTY"];
                    row2["FG UOM"] = dr1["FGUOM"];
                    row2["FG Warehouse"] = dr1["FGWHDESC"];
                    row2["Work Center Code"] = dr1["WCCODE"];
                    row2["Work Center Name"] = dr1["WCDESC"];
                    row2["Header Remarks"] = dr1["REMARKS"];
                    row2["Part No."] = dr1["BOMPRDCODE"];
                    row2["Description"] = dr1["BOMPRDDESC"];
                    row2["Drawing No."] = dr1["BOMPRDDNO"];
                    row2["Drawing Rev. No."] = dr1["BOMPRDREVNO"];
                    row2["Det. Part No."] = dr1["MATCODE"];
                    row2["Det. Description"] = dr1["MATDESC"];
                    row2["Det. Drawing No."] = dr1["DESIGNNO"];
                    row2["Det. Drawing Rev. No."] = dr1["DREVISIONNO"];
                    row2["Qty."] = dr1["MATQTY"];
                    row2["UOM"] = dr1["MATUOM"];
                    row2["Price"] = dr1["MATRATE"];
                    row2["Amount"] = dr1["MATAMT"];
                    row2["Warehouse"] = dr1["WAREHOUSENAME"];
                    row2["Created by"] = dr1["CREATEDBY"];

                    dtExport.Rows.Add(row2);
                }

                if (ProjectSelectInEntryModule.ToUpper().Trim() == "NO")
                    dtExport.Columns.Remove("Project Name");

                if (chkPODet.Checked == false)
                {
                    dtExport.Columns.Remove("Production Order No.");
                    dtExport.Columns.Remove("Production Order Date");
                }

                if (chkWODet.Checked == false)
                {
                    dtExport.Columns.Remove("Work Order No.");
                    dtExport.Columns.Remove("Work Order Date");
                }

                if (chkFGWH.Checked == false)
                    dtExport.Columns.Remove("FG Warehouse");

                if (chkHeadRem.Checked == false)
                    dtExport.Columns.Remove("Header Remarks");

                if (chkWCDet.Checked == false)
                {
                    dtExport.Columns.Remove("Work Center Code");
                    dtExport.Columns.Remove("Work Center Name");
                }

                if (chkCreateBy.Checked == false)
                    dtExport.Columns.Remove("Created by");

                dtExport.Columns.Remove("BRANCHDESC");
                dtExport.Columns.Remove("PROJ_NAME");
                dtExport.Columns.Remove("PRDINO");
                dtExport.Columns.Remove("PRDIDATE");
                dtExport.Columns.Remove("PONO");
                dtExport.Columns.Remove("PODT");
                dtExport.Columns.Remove("WONO");
                dtExport.Columns.Remove("WODT");
                dtExport.Columns.Remove("BOM_NO");
                dtExport.Columns.Remove("BOM_DATE");
                dtExport.Columns.Remove("REV_NO");
                dtExport.Columns.Remove("REV_DATE");
                dtExport.Columns.Remove("FGCODE");
                dtExport.Columns.Remove("FGDESC");
                dtExport.Columns.Remove("FGQTY");
                dtExport.Columns.Remove("FGUOM");
                dtExport.Columns.Remove("FGWHDESC");
                dtExport.Columns.Remove("WCCODE");
                dtExport.Columns.Remove("WCDESC");
                dtExport.Columns.Remove("REMARKS");
                dtExport.Columns.Remove("BOMPRDCODE");
                dtExport.Columns.Remove("BOMPRDDESC");
                dtExport.Columns.Remove("BOMPRDDNO");
                dtExport.Columns.Remove("BOMPRDREVNO");
                dtExport.Columns.Remove("MATCODE");
                dtExport.Columns.Remove("MATDESC");
                dtExport.Columns.Remove("DESIGNNO");
                dtExport.Columns.Remove("DREVISIONNO");
                dtExport.Columns.Remove("MATQTY");
                dtExport.Columns.Remove("MATUOM");
                dtExport.Columns.Remove("MATRATE");
                dtExport.Columns.Remove("MATAMT");
                dtExport.Columns.Remove("WAREHOUSENAME");
                dtExport.Columns.Remove("CREATEDBY");

                DataRow row3 = dtExport.NewRow();
                row3["Unit"] = ds.Tables[1].Rows[0]["BRANCHDESC"].ToString();
                row3["FG Qty."] = ds.Tables[1].Rows[0]["FGQTY"].ToString();
                row3["Qty."] = ds.Tables[1].Rows[0]["MATQTY"].ToString();
                row3["Amount"] = ds.Tables[1].Rows[0]["MATAMT"].ToString();
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
                HeaderRow6[0] = "Issue for Production Register - Detail";
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

            string IsPRDIRegDetSumFilter = Convert.ToString(hfIsPRDIRegDetSumFilter.Value);
            Session["IsPRDIRegDetSumFilter"] = IsPRDIRegDetSumFilter;

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

            Task PopulateStockTrialDataTask = new Task(() => GetPRDIRegisterdata(FROMDATE, TODATE, BRANCH_ID, WH_ID, PROJECT_ID));
            PopulateStockTrialDataTask.RunSynchronously();
        }
        public void GetPRDIRegisterdata(string FROMDATE, string TODATE, string BRANCH_ID, string WH_ID, string PROJECT_ID)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_MFISSUEFORPRODUCTIONREGISTERDETAIL_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                cmd.Parameters.AddWithValue("@TODATE", TODATE);
                cmd.Parameters.AddWithValue("@BRANCH_ID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@PRDIID", hdnPRDIId.Value);
                cmd.Parameters.AddWithValue("@PRODUCT_ID", hdnProductId.Value);
                cmd.Parameters.AddWithValue("@PROJECT_ID", PROJECT_ID);
                cmd.Parameters.AddWithValue("@WAREHOUSE_ID", WH_ID);
                cmd.Parameters.AddWithValue("@SHOWWO", (chkWODet.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@SHOWPO", (chkPODet.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@SHOWCREATEBY", (chkCreateBy.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@SHOWFGWH", (chkFGWH.Checked) ? "1" : "0");                
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
            if (Convert.ToString(Session["IsPRDIRegDetSumFilter"]) == "Y")
            {
                dtPRDITotal = oDBEngine.GetDataTable("Select BRANCHDESC,FGQTY,MATQTY,MATAMT from MFISSUEFORPRODUCTIONREGISTERDETAIL_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND BRANCHID=999999999999 AND BRANCHDESC='Total :'");
                PRDITotalBalDesc = dtPRDITotal.Rows[0][0].ToString();
                PRDIFGQty = dtPRDITotal.Rows[0][1].ToString();
                PRDIStkQty = dtPRDITotal.Rows[0][2].ToString();
                PRDIAmt = dtPRDITotal.Rows[0][3].ToString();
            }

            string summaryTAG = (e.Item as ASPxSummaryItem).Tag;
            if (e.IsTotalSummary == true)
            {
                switch (summaryTAG)
                {
                    case "Branch":
                        e.Text = PRDITotalBalDesc;
                        break;
                    case "FGQty":
                        e.Text = PRDIFGQty;
                        break;
                    case "StkQty":
                        e.Text = PRDIStkQty;
                        break;
                    case "Amt":
                        e.Text = PRDIAmt;
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

            if (Convert.ToString(Session["IsPRDIRegDetSumFilter"]) == "Y")
            {
                var q = from d in dc.MFISSUEFORPRODUCTIONREGISTERDETAIL_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.BRANCHID) != "999999999999" && Convert.ToString(d.BRANCHDESC) != "Total :"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.MFISSUEFORPRODUCTIONREGISTERDETAIL_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }

            if (chkPODet.Checked == true)
            {
                ShowGrid.Columns[4].Visible = true;
                ShowGrid.Columns[5].Visible = true;
            }
            else
            {
                ShowGrid.Columns[4].Visible = false;
                ShowGrid.Columns[5].Visible = false;
            }

            if (chkWODet.Checked == true)
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
                ShowGrid.Columns[16].Visible = true;
            }
            else
            {
                ShowGrid.Columns[16].Visible = false;
            }

            if (chkHeadRem.Checked == true)
            {
                ShowGrid.Columns[19].Visible = true;
            }
            else
            {
                ShowGrid.Columns[19].Visible = false;
            }

            if (chkWCDet.Checked == true)
            {
                ShowGrid.Columns[17].Visible = true;
                ShowGrid.Columns[18].Visible = true;
            }
            else
            {
                ShowGrid.Columns[17].Visible = false;
                ShowGrid.Columns[18].Visible = false;
            }

            if (chkCreateBy.Checked == true)
            {
                ShowGrid.Columns[33].Visible = true;
            }
            else
            {
                ShowGrid.Columns[33].Visible = false;
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