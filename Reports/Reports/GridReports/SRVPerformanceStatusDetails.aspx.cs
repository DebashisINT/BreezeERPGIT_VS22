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
    public partial class SRVPerformanceStatusDetails : System.Web.UI.Page
    {
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        ExcelFile objExcel = new ExcelFile();
        DataTable dtExport = new DataTable();
        DataTable dtReportHeader = new DataTable();
        DataTable dtReportFooter = new DataTable();

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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/SRVPerformanceStatusDetails.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            if (!IsPostBack)
            {
                DataTable dtBranchSelection = new DataTable();
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "TAT - Details";
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
                Session["IsSrvPerformanceStatDetFilter"] = null;
                BranchHoOffice();

                ShowGrid.Columns[1].Visible = false;

                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;

                ASPxFromDate.Value = DateTime.Now;
                ASPxToDate.Value = DateTime.Now;
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                Session["BranchNames"] = null;
                dtBranchSelection = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='ReportsBranchSelection'");
                hdnBranchSelection.Value = dtBranchSelection.Rows[0][0].ToString();
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

        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Convert.ToString(Session["IsSrvPerformanceStatDetFilter"]) == "Y")
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
            string filename = "TATDetails";
            exporter.FileName = filename;

            if (Filter == 1 || Filter == 2)
            {
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                con.Open();
                string selectQuery = "SELECT BRANCHDESC,PARTY,CHALLAN_NO,RCVDT,RCVTIME,SRVDT,SRVTIME,TAKEMINUTELESS1HR,TAKEMINUTEGREATER1HR,DAYSTAKEN FROM SRVPERFORMANCESTATDET_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " order by SEQ";
                SqlDataAdapter myCommand = new SqlDataAdapter(selectQuery, con);

                // Create and fill a DataSet.
                DataSet ds = new DataSet();
                myCommand.Fill(ds, "Main");
                myCommand.Dispose();
                con.Dispose();
                Session["exportperformancestatdetdataset"] = ds;

                dtExport = ds.Tables[0].Copy();
                dtExport.Clear();
                if (chkIsBranchWise.Checked == true)
                {
                    dtExport.Columns.Add(new DataColumn("Unit", typeof(string)));
                    dtExport.Columns.Add(new DataColumn("Technician", typeof(string)));
                }
                else
                {
                    dtExport.Columns.Add(new DataColumn("Technician", typeof(string)));
                    dtExport.Columns.Add(new DataColumn("Unit", typeof(string)));
                }
                dtExport.Columns.Add(new DataColumn("Challan No.", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Received Date", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Received Time", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Serviced Date", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Serviced Time", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Time Taken (< 1) Hour", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Time Taken (>= 1) Hour", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Days Taken", typeof(Int32)));

                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {
                    DataRow row2 = dtExport.NewRow();

                    row2["Unit"] = dr1["BRANCHDESC"];
                    row2["Technician"] = dr1["PARTY"];
                    row2["Challan No."] = dr1["CHALLAN_NO"];
                    row2["Received Date"] = dr1["RCVDT"];
                    row2["Received Time"] = dr1["RCVTIME"];
                    row2["Serviced Date"] = dr1["SRVDT"];
                    row2["Serviced Time"] = dr1["SRVTIME"];
                    row2["Time Taken (< 1) Hour"] = dr1["TAKEMINUTELESS1HR"];
                    row2["Time Taken (>= 1) Hour"] = dr1["TAKEMINUTEGREATER1HR"];
                    row2["Days Taken"] = dr1["DAYSTAKEN"];

                    dtExport.Rows.Add(row2);
                }

                if(chkIsBranchWise.Checked==true)
                    dtExport.Columns.Remove("Technician");

                dtExport.Columns.Remove("BRANCHDESC");
                dtExport.Columns.Remove("PARTY");
                dtExport.Columns.Remove("CHALLAN_NO");
                dtExport.Columns.Remove("RCVDT");
                dtExport.Columns.Remove("RCVTIME");
                dtExport.Columns.Remove("SRVDT");
                dtExport.Columns.Remove("SRVTIME");
                dtExport.Columns.Remove("TAKEMINUTELESS1HR");
                dtExport.Columns.Remove("TAKEMINUTEGREATER1HR");
                dtExport.Columns.Remove("DAYSTAKEN");

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
                HeaderRow6[0] = "TAT - Details";
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
                    objExcel.ExportToExcelforExcel(dtExport, "TATDetails", "ZZZZZZZZZZZZZZZZZZZZ", "ZZZZZZZZZZZZZZZZZZZZ", dtReportHeader, dtReportFooter);
                    break;
                case 2:
                    objExcel.ExportToPDF(dtExport, "TATDetails", "ZZZZZZZZZZZZZZZZZZZZ", "ZZZZZZZZZZZZZZZZZZZZ", dtReportHeader, dtReportFooter);
                    break;
                case 3:
                    exporter.WriteCsvToResponse();
                    break;
                default:
                    return;
            }
        }

        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string returnPara = Convert.ToString(e.Parameter);
            string HEAD_BRANCH = returnPara.Split('~')[1];

            string IsSrvPerformanceStatDetFilter = Convert.ToString(hfIsSrvPerformanceStatDetFilter.Value);
            Session["IsSrvPerformanceStatDetFilter"] = IsSrvPerformanceStatDetFilter;

            DateTime dtFrom;
            DateTime dtTo;

            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);

            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");
            string BRANCH_ID = "";

            string BranComponent = "";
            List<object> BranList = lookup_branch.GridView.GetSelectedFieldValues("ID");
            foreach (object Bran in BranList)
            {
                BranComponent += "," + Bran;
            }
            BRANCH_ID = BranComponent.TrimStart(',');

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

            Task PopulateStockTrialDataTask = new Task(() => GetPerformanceStatdata(FROMDATE, TODATE, BRANCH_ID, HEAD_BRANCH));
            PopulateStockTrialDataTask.RunSynchronously();
        }
        public void GetPerformanceStatdata(string FROMDATE, string TODATE, string BRANCH_ID, string HEAD_BRANCH)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_SRVPERFORMANCESTATDET_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                cmd.Parameters.AddWithValue("@TODATE", TODATE);
                cmd.Parameters.AddWithValue("@HO", HEAD_BRANCH);
                cmd.Parameters.AddWithValue("@BRANCHID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@PARTY_CODE", hdnTechnicianId.Value);
                cmd.Parameters.AddWithValue("@ISBRANCHWISE", (chkIsBranchWise.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@ISTECHNICIANWISE", (chkIsTechnicianWise.Checked) ? "1" : "0");
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

        #region LinQ
        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsSrvPerformanceStatDetFilter"]) == "Y")
            {
                var q = from d in dc.SRVPERFORMANCESTATDET_REPORTs
                        where Convert.ToString(d.USERID) == Userid
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.SRVPERFORMANCESTATDET_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }

            if(chkIsBranchWise.Checked==true)
            {
                ShowGrid.Columns[1].Visible = false;
            }
            else
            {
                ShowGrid.Columns[1].Visible = true;
                ShowGrid.Columns[1].VisibleIndex = 1;
                ShowGrid.Columns[0].VisibleIndex = 2;
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

        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
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
    }
}