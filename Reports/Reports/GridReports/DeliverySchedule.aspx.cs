#region =======================Revision History=============================================================================================================================
//1.0   v2 .0.37    Debashis    18/04/2023  Error occurred "Input string was not in a correct format." while export Delivery schedule Report.
//                                          Now it has been taken care of.Refer: 0025850
#endregion=======================End Revision History=======================================================================================================================
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
    public partial class DeliverySchedule : System.Web.UI.Page
    {
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        ExcelFile objExcel = new ExcelFile();
        DataTable CompanyInfo = new DataTable();
        DataTable dtExport = new DataTable();
        DataTable dtReportHeader = new DataTable();
        DataTable dtReportFooter = new DataTable();

        DataTable dtProductTotal = null;
        string TotalOrdQty = "";
        string ProductOrdQty = "";
        string ProductSchDelQty = "";
        string ProductActDelQty = "";
        string ProductPendDelQty = "";
        string ProductWarrantyDays = "";
        string ProductTotalDesc = "";

        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime dtFrom;
            DateTime dtTo;
            if (!IsPostBack)
            {
                rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/DeliverySchedule.aspx");
                DataTable dtBranchSelection = new DataTable();
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Delivery Schedule";
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
                Session["IsDelvSchldFilter"] = null;
                BranchHoOffice();

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

        #region Export
        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Convert.ToString(Session["IsDelvSchldFilter"]) == "Y")
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
            string filename = "DeliverySchedule";
            exporter.FileName = filename;
            if (Filter == 1 || Filter == 2)
            {
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                con.Open();
                string selectQuery = "SELECT BRANCHDESC,PARTYNAME,ORDNO,ORDDT,TOTORDQTY,PRODNAME,PRODQTY,STOCKUOM,SCHDELDT,SCHDELQTY,ACTDELDT,ACTDELQTY,PENDDELQTY,WARRANTYDAYS,WARRANTYSTDT,WARRANTYENDDT FROM DELIVERYSCHEDULE_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND PARTYNAME<>'Gross Total :' AND BRANCH_ID<>99999999999999 order by SEQ";
                SqlDataAdapter myCommand = new SqlDataAdapter(selectQuery, con);

                // Create and fill a DataSet.
                DataSet ds = new DataSet();
                myCommand.Fill(ds, "Main");
                //Rev 1.0
                //myCommand = new SqlDataAdapter("SELECT PARTYNAME,TOTORDQTY,PRODQTY,SCHDELQTY,ACTDELQTY,PENDDELQTY,WARRANTYDAYS FROM DELIVERYSCHEDULE_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND PARTYNAME='Gross Total :' AND BRANCH_ID=99999999999999", con);
                myCommand = new SqlDataAdapter("SELECT PARTYNAME,ISNULL(TOTORDQTY,0.00) AS TOTORDQTY,ISNULL(PRODQTY,0.00) AS PRODQTY,ISNULL(SCHDELQTY,0.00) AS SCHDELQTY,ISNULL(ACTDELQTY,0.00) AS ACTDELQTY,ISNULL(PENDDELQTY,0.00) AS PENDDELQTY,ISNULL(WARRANTYDAYS,0.00) AS WARRANTYDAYS FROM DELIVERYSCHEDULE_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND PARTYNAME='Gross Total :' AND BRANCH_ID=99999999999999", con);
                //End of Rev 1.0
                myCommand.Fill(ds, "GrossTotal");
                myCommand.Dispose();
                con.Dispose();
                Session["exportdelvschlddataset"] = ds;

                dtExport = ds.Tables[0].Copy();
                dtExport.Clear();
                dtExport.Columns.Add(new DataColumn("Unit", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Customer", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Order No.", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Order Date", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Total Order Qty.", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Product", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Product Ord. Qty.", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("UOM", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Schedule Del. Date", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Schedule Del. Qty.", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Actual Delivery Date", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Actual Delivery Qty.", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Pending Delivery Qty.", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Warranty Day(s)", typeof(int)));
                dtExport.Columns.Add(new DataColumn("Warranty Start Date", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Warranty End Date", typeof(string)));

                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {
                    DataRow row2 = dtExport.NewRow();

                    row2["Unit"] = dr1["BRANCHDESC"];
                    row2["Customer"] = dr1["PARTYNAME"];
                    row2["Order No."] = dr1["ORDNO"];
                    row2["Order Date"] = dr1["ORDDT"];
                    row2["Total Order Qty."] = dr1["TOTORDQTY"];
                    row2["Product"] = dr1["PRODNAME"];
                    row2["Product Ord. Qty."] = dr1["PRODQTY"];
                    row2["UOM"] = dr1["STOCKUOM"];
                    row2["Schedule Del. Date"] = dr1["SCHDELDT"];
                    row2["Schedule Del. Qty."] = dr1["SCHDELQTY"];
                    row2["Actual Delivery Date"] = dr1["ACTDELDT"];
                    row2["Actual Delivery Qty."] = dr1["ACTDELQTY"];
                    row2["Pending Delivery Qty."] = dr1["PENDDELQTY"];
                    row2["Warranty Day(s)"] = dr1["WARRANTYDAYS"];
                    row2["Warranty Start Date"] = dr1["WARRANTYSTDT"];
                    row2["Warranty End Date"] = dr1["WARRANTYENDDT"];

                    dtExport.Rows.Add(row2);
                }

                dtExport.Columns.Remove("BRANCHDESC");
                dtExport.Columns.Remove("PARTYNAME");
                dtExport.Columns.Remove("ORDNO");
                dtExport.Columns.Remove("ORDDT");
                dtExport.Columns.Remove("TOTORDQTY");
                dtExport.Columns.Remove("PRODNAME");
                dtExport.Columns.Remove("PRODQTY");
                dtExport.Columns.Remove("STOCKUOM");
                dtExport.Columns.Remove("SCHDELDT");
                dtExport.Columns.Remove("SCHDELQTY");
                dtExport.Columns.Remove("ACTDELDT");
                dtExport.Columns.Remove("ACTDELQTY");
                dtExport.Columns.Remove("PENDDELQTY");
                dtExport.Columns.Remove("WARRANTYDAYS");
                dtExport.Columns.Remove("WARRANTYSTDT");
                dtExport.Columns.Remove("WARRANTYENDDT");

                DataRow row3 = dtExport.NewRow();
                row3["Customer"] = ds.Tables[1].Rows[0]["PARTYNAME"].ToString();
                row3["Total Order Qty."] = ds.Tables[1].Rows[0]["TOTORDQTY"].ToString();
                row3["Product Ord. Qty."] = ds.Tables[1].Rows[0]["PRODQTY"].ToString();
                row3["Schedule Del. Qty."] = ds.Tables[1].Rows[0]["SCHDELQTY"].ToString();
                row3["Actual Delivery Qty."] = ds.Tables[1].Rows[0]["ACTDELQTY"].ToString();
                row3["Pending Delivery Qty."] = ds.Tables[1].Rows[0]["PENDDELQTY"].ToString();
                row3["Warranty Day(s)"] = ds.Tables[1].Rows[0]["WARRANTYDAYS"].ToString();
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
                HeaderRow6[0] = "Branch/Warehouse Wise Stock - Detail";
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
                    objExcel.ExportToExcelforExcel(dtExport, "DeliverySchedule", "Total of : ", "Gross Total : ", dtReportHeader, dtReportFooter);
                    break;
                case 2:
                    objExcel.ExportToPDF(dtExport, "DeliverySchedule", "Total of : ", "Gross Total : ", dtReportHeader, dtReportFooter);
                    break;
                case 3:
                    exporter.WriteCsvToResponse();
                    break;
                default:
                    return;
            }

        }

        #endregion

        #region =======================Delivery Schedule Data =========================
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string returnPara = Convert.ToString(e.Parameter);
            string HEAD_BRANCH = returnPara.Split('~')[1];

            string IsDelvSchldFilter = Convert.ToString(hfIsDelvSchldFilter.Value);
            Session["IsDelvSchldFilter"] = IsDelvSchldFilter;

            DateTime dtFrom;
            DateTime dtTo;

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

            Task PopulateStockTrialDataTask = new Task(() => GetDeliveryScheduledata(FROMDATE, TODATE, BRANCH_ID));
            PopulateStockTrialDataTask.RunSynchronously();
        }
        public void GetDeliveryScheduledata(string FROMDATE, string TODATE, string BRANCH_ID)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_DELIVERYSCHEDULE_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                cmd.Parameters.AddWithValue("@TODATE", TODATE);
                cmd.Parameters.AddWithValue("@BRANCHID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@PARTY_CODE", hdnCustomerId.Value);
                cmd.Parameters.AddWithValue("@PRODUCT_ID", hdnProductId.Value);
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
            if (Convert.ToString(Session["IsDelvSchldFilter"]) == "Y")
            {
                dtProductTotal = oDBEngine.GetDataTable("SELECT PARTYNAME,TOTORDQTY,PRODQTY,SCHDELQTY,ACTDELQTY,PENDDELQTY,WARRANTYDAYS FROM DELIVERYSCHEDULE_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND PARTYNAME='Gross Total :'");
                ProductTotalDesc = dtProductTotal.Rows[0][0].ToString();
                TotalOrdQty = dtProductTotal.Rows[0][1].ToString();
                ProductOrdQty = dtProductTotal.Rows[0][2].ToString();
                ProductSchDelQty = dtProductTotal.Rows[0][3].ToString();
                ProductActDelQty = dtProductTotal.Rows[0][4].ToString();
                ProductPendDelQty = dtProductTotal.Rows[0][5].ToString();
                ProductWarrantyDays = dtProductTotal.Rows[0][6].ToString();
            }

            string summaryTAG = (e.Item as ASPxSummaryItem).Tag;
            if (e.IsTotalSummary == true)
            {
                switch (summaryTAG)
                {
                    case "Item_Prod":
                        e.Text = ProductTotalDesc;
                        break;
                    case "Item_TotOrdQty":
                        e.Text = TotalOrdQty;
                        break;
                    case "Item_OrdQty":
                        e.Text = ProductOrdQty;
                        break;
                    case "Item_SchDelQty":
                        e.Text = ProductSchDelQty;
                        break;
                    case "Item_ActDelQty":
                        e.Text = ProductActDelQty;
                        break;
                    case "Item_PendDelQty":
                        e.Text = ProductPendDelQty;
                        break;
                    case "Item_WarrantyDays":
                        e.Text = ProductWarrantyDays;
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

            if (Convert.ToString(Session["IsDelvSchldFilter"]) == "Y")
            {
                var q = from d in dc.DELIVERYSCHEDULE_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.PARTYNAME) != "Gross Total :"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.DELIVERYSCHEDULE_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
        }

        #endregion

        public void Date_finyearwise(string Finyear)
        {
            //CommonBL bll1 = new CommonBL();
            //DataTable stbill = new DataTable();
            //stbill = bll1.GetDateFinancila(Finyear);
            //if (stbill.Rows.Count > 0)
            //{
            //    ASPxFromDate.Text = Convert.ToDateTime(stbill.Rows[0]["FinYear_StartDate"]).ToString("dd-MM-yyyy");
            //    ASPxToDate.Text = Convert.ToDateTime(stbill.Rows[0]["FinYear_EndDate"]).ToString("dd-MM-yyyy");
            //}

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
     
    }
}