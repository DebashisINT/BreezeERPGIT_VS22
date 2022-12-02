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
    public partial class ProfitabilitySales : System.Web.UI.Page
    {
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        ExcelFile objExcel = new ExcelFile();
        DataTable dtExport = new DataTable();
        DataTable dtReportHeader = new DataTable();
        DataTable dtReportFooter = new DataTable();

        DataTable dtUnitTotal = null;
        string PSTotalDesc = "";
        string PSTotalQty = "";
        string PSTotalSA = "";
        string PSTotalCOS = "";
        string PSTotalPS = "";

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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/ProfitabilitySales.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            if (!IsPostBack)
            {
                DataTable dtBranchSelection = new DataTable();
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Profitability Sales";
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
                BranchHoOffice();
                Session["IsProfitabilitySalesFilter"] = null;

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
            if (Convert.ToString(Session["IsProfitabilitySalesFilter"]) == "Y")
            {
                if (Filter != 0)
                {
                    bindexport(Filter);
                }
            }
        }

        public void bindexport(int Filter)
        {
            string filename = "ProfitabilitySales";
            exporter.FileName = filename;

            if (Filter == 1 || Filter == 2)
            {
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                con.Open();
                string selectQuery = "SELECT BRANCHDESC,PRODNAME,STOCKUOM,PRODCLASS,BRAND_NAME,NETSALESQTY,NETSALESAMT,COSTOFSALES,PROFITABILITY,GPMARGIN FROM PROFITABILITYSALES_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND BRANCH_ID<>999999999999 AND BRANCHDESC<>'Total :' order by SEQ";
                SqlDataAdapter myCommand = new SqlDataAdapter(selectQuery, con);

                // Create and fill a DataSet.
                DataSet ds = new DataSet();
                myCommand.Fill(ds, "Main");
                myCommand = new SqlDataAdapter("SELECT BRANCHDESC,NETSALESQTY,NETSALESAMT,COSTOFSALES,PROFITABILITY from PROFITABILITYSALES_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND BRANCH_ID=999999999999 AND BRANCHDESC='Total :'", con);
                myCommand.Fill(ds, "GrossTotal");
                myCommand.Dispose();
                con.Dispose();
                Session["exportpsdataset"] = ds;

                dtExport = ds.Tables[0].Copy();
                dtExport.Clear();
                dtExport.Columns.Add(new DataColumn("Unit", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Item", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Stock UOM", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Class", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Brand", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Net Sale Qty.", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Net Sales", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Cost of Sales", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Profitability", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("GP Margin", typeof(decimal)));

                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {
                    DataRow row2 = dtExport.NewRow();

                    row2["Unit"] = dr1["BRANCHDESC"];
                    row2["Item"] = dr1["PRODNAME"];
                    row2["Stock UOM"] = dr1["STOCKUOM"];
                    row2["Class"] = dr1["PRODCLASS"];
                    row2["Brand"] = dr1["BRAND_NAME"];
                    row2["Net Sale Qty."] = dr1["NETSALESQTY"];
                    row2["Net Sales"] = dr1["NETSALESAMT"];
                    row2["Cost of Sales"] = dr1["COSTOFSALES"];
                    row2["Profitability"] = dr1["PROFITABILITY"];
                    row2["GP Margin"] = dr1["GPMARGIN"];

                    dtExport.Rows.Add(row2);
                }

                dtExport.Columns.Remove("BRANCHDESC");
                dtExport.Columns.Remove("PRODNAME");
                dtExport.Columns.Remove("STOCKUOM");
                dtExport.Columns.Remove("PRODCLASS");
                dtExport.Columns.Remove("BRAND_NAME");
                dtExport.Columns.Remove("NETSALESQTY");
                dtExport.Columns.Remove("NETSALESAMT");
                dtExport.Columns.Remove("COSTOFSALES");
                dtExport.Columns.Remove("PROFITABILITY");
                dtExport.Columns.Remove("GPMARGIN");

                DataRow row3 = dtExport.NewRow();
                row3["Unit"] = ds.Tables[1].Rows[0]["BRANCHDESC"].ToString();
                row3["Net Sale Qty."] = ds.Tables[1].Rows[0]["NETSALESQTY"].ToString();
                row3["Net Sales"] = ds.Tables[1].Rows[0]["NETSALESAMT"].ToString();
                row3["Cost of Sales"] = ds.Tables[1].Rows[0]["COSTOFSALES"].ToString();
                row3["Profitability"] = ds.Tables[1].Rows[0]["PROFITABILITY"].ToString();
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
                HeaderRow6[0] = "Profitability Sales";
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
                    objExcel.ExportToExcelforExcel(dtExport, "ProfitabilitySales", "ZZZZZZZZZZZZZZZZ", "ZZZZZZZZZZZZZZZZ", dtReportHeader, dtReportFooter);
                    break;
                case 2:
                    objExcel.ExportToPDF(dtExport, "ProfitabilitySales", "ZZZZZZZZZZZZZZZZ", "ZZZZZZZZZZZZZZZZ", dtReportHeader, dtReportFooter);
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

            string IsProfitabilitySalesFilter = Convert.ToString(hfIsProfitabilitySalesFilter.Value);
            Session["IsProfitabilitySalesFilter"] = IsProfitabilitySalesFilter;

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

            Task PopulateStockTrialDataTask = new Task(() => GetProfitabilitySalesdata(FROMDATE, TODATE, BRANCH_ID, HEAD_BRANCH));
            PopulateStockTrialDataTask.RunSynchronously();
        }
        public void GetProfitabilitySalesdata(string FROMDATE, string TODATE, string BRANCH_ID, string HEAD_BRANCH)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_PROFITABILITYSALES_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                cmd.Parameters.AddWithValue("@TODATE", TODATE);
                cmd.Parameters.AddWithValue("@HO", HEAD_BRANCH);
                cmd.Parameters.AddWithValue("@BRANCHID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@PRODUCTID", hdnProductId.Value);
                cmd.Parameters.AddWithValue("@CLASS", hdnClassId.Value);
                cmd.Parameters.AddWithValue("@BRAND", hdnBranndId.Value);
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
            if (Convert.ToString(Session["IsProfitabilitySalesFilter"]) == "Y")
            {
                dtUnitTotal = oDBEngine.GetDataTable("SELECT BRANCHDESC,NETSALESQTY,NETSALESAMT,COSTOFSALES,PROFITABILITY from PROFITABILITYSALES_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND BRANCH_ID=999999999999 AND BRANCHDESC='Total :'");
                PSTotalDesc = dtUnitTotal.Rows[0][0].ToString();
                PSTotalQty = dtUnitTotal.Rows[0][1].ToString();
                PSTotalSA = dtUnitTotal.Rows[0][2].ToString();
                PSTotalCOS = dtUnitTotal.Rows[0][3].ToString();
                PSTotalPS = dtUnitTotal.Rows[0][4].ToString();
            }

            string summaryTAG = (e.Item as ASPxSummaryItem).Tag;
            if (e.IsTotalSummary == true)
            {
                switch (summaryTAG)
                {
                    case "TagUnit":
                        e.Text = PSTotalDesc;
                        break;
                    case "TagQty":
                        e.Text = PSTotalQty;
                        break;
                    case "TagSA":
                        e.Text = PSTotalSA;
                        break;
                    case "TagCOS":
                        e.Text = PSTotalCOS;
                        break;
                    case "TagPS":
                        e.Text = PSTotalPS;
                        break;
                }
            }
        }

        public void Date_finyearwise(string Finyear)
        {
            CommonBL ps = new CommonBL();
            DataTable dtps = new DataTable();

            dtps = ps.GetDateFinancila(Finyear);
            if (dtps.Rows.Count > 0)
            {
                ASPxFromDate.MaxDate = Convert.ToDateTime((dtps.Rows[0]["FinYear_EndDate"]));
                ASPxFromDate.MinDate = Convert.ToDateTime((dtps.Rows[0]["FinYear_StartDate"]));

                ASPxToDate.MaxDate = Convert.ToDateTime((dtps.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((dtps.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((dtps.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((dtps.Rows[0]["FinYear_StartDate"]));

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

        #region LinQ
        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsProfitabilitySalesFilter"]) == "Y")
            {
                var q = from d in dc.PROFITABILITYSALES_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.BRANCH_ID) != "999999999999" && Convert.ToString(d.BRANCHDESC) != "Total :"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.PROFITABILITYSALES_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
        }

        #endregion
    }
}