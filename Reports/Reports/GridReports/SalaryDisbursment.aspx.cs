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

using DevExpress.Data;
using System.Threading.Tasks;

using DevExpress.XtraPrinting;
using DevExpress.Export;
using System.Drawing;
namespace Reports.Reports.GridReports
{
    public partial class SalaryDisbursment : System.Web.UI.Page
    {
        DataTable DTIndustry = new DataTable();
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        string data = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Reports/master/SalaryDisbursment.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            if (!IsPostBack)
            {
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Salary Disbursement";
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

                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;

                ASPxFromDate.Text = dtFrom.ToString("dd-MM-yyyy");
                ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");

                Session["exportval"] = null;
                Session["dt_PartyLedgerRpt"] = null;
                Session["SI_ComponentData_Branch"] = null;

                //fnBindBranch();
                //cmbBranchfilter.SelectedIndex = 0;
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);
                string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                string TODATE = dtTo.ToString("yyyy-MM-dd");
                //string BRANCH_ID = "0";
                
                //Task PopulateSalaryDisDataTask = new Task(() => GetSalary(FROMDATE, TODATE, BRANCH_ID));
                //PopulateSalaryDisDataTask.RunSynchronously();
            }
            else
            {
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);
            }
        }
        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {

            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);


                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {


                if (Session["exportval"] == null)
                {
                    //Session["exportval"] = Filter;
                    // BindDropDownList();
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    // Session["exportval"] = Filter;
                    // BindDropDownList();
                    bindexport(Filter);
                }

                //drdExport.SelectedValue = "0";
            }

        }
        public void BindDropDownList()
        {
            // Declare a Dictionary to hold all the Options with Value and Text.
            Dictionary<string, string> options = new Dictionary<string, string>();
            options.Add("0", "Export to");
            options.Add("1", "PDF");
            options.Add("2", "XLSX");
            options.Add("3", "RTF");
            options.Add("4", "CSV");


            // Bind the Dictionary to the DropDownList.
            drdExport.DataSource = options;
            drdExport.DataTextField = "value";
            drdExport.DataValueField = "key";
            drdExport.DataBind();
            drdExport.SelectedValue = "0";
        }
        public void bindexport(int Filter)
        {
            string filename = "SalaryDisbursement";
            string FileHeader = "";

            exporter.FileName = filename;

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true,true, true, true) + Environment.NewLine + "Salary Disbursement Statement" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
            
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
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;
            }

        }

        protected void exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            e.BrickStyle.BackColor = Color.White;
            e.BrickStyle.ForeColor = Color.Black;
        }

        //protected void BindLedgerPosting()
        //{
        //    try
        //    {
        //        if (Session["dt_PartyLedgerRpt"] != null)
        //        {
        //            ShowGrid.DataSource = (DataTable)Session["dt_PartyLedgerRpt"];
        //            ShowGrid.DataBind();
        //        }
        //    }
        //    catch { }
        //}
        protected void Grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            Session.Remove("dt_PartyLedgerRpt");


            ShowGrid.JSProperties["cpSave"] = null;


            string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

            DateTime dtFrom;
            DateTime dtTo;
            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);

            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");

            //string BRANCH_ID = Convert.ToString(cmbBranchfilter.Value);

            //Task PopulateStockTrialDataTask = new Task(() => GetSalary(FROMDATE, TODATE, BRANCH_ID));
            Task PopulateStockTrialDataTask = new Task(() => GetSalary(FROMDATE, TODATE));

            PopulateStockTrialDataTask.RunSynchronously();

        }
        protected void grid2_DataBinding(object sender, EventArgs e)
        {

            if (Session["dt_PartyLedgerRpt"] != null)
            {

                ShowGrid.DataSource = (DataTable)Session["dt_PartyLedgerRpt"];

            }

        }
        public void GetSalary(string FROMDATE, string TODATE)
        {
            try
            {
                DataSet ds = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PROC_SalaryDisbursement", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]).Trim());
                //cmd.Parameters.AddWithValue("@BRANCH_ID", Convert.ToString(BRANCH_ID).Trim());
                cmd.Parameters.AddWithValue("@FROMDATE", Convert.ToString(FROMDATE).Trim());
                cmd.Parameters.AddWithValue("@TODATE", Convert.ToString(TODATE).Trim());

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);

                cmd.Dispose();
                con.Dispose();

                Session["dt_PartyLedgerRpt"] = ds.Tables[0];

                ShowGrid.DataSource = ds.Tables[0];
                ShowGrid.DataBind();
            }
            catch (Exception ex)
            {
            }
        }
        [WebMethod]
        public void Date_finyearwise(string Finyear)
        {
            CommonBL bll1 = new CommonBL();
            DataTable stbill = new DataTable();
            stbill = bll1.GetDateFinancila(Finyear);
            if (stbill.Rows.Count > 0)
            {
                ASPxFromDate.Text = Convert.ToDateTime(stbill.Rows[0]["FinYear_StartDate"]).ToString("dd-MM-yyyy");
                ASPxToDate.Text = Convert.ToDateTime(stbill.Rows[0]["FinYear_EndDate"]).ToString("dd-MM-yyyy");
            }

        }
        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }

        #region Branch Populate

        //protected void Componentbranch_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        //{
            
        //}
        //private void fnBindBranch()
        //{
        //    DataTable ComponentTable = new DataTable();
        //    if (Session["userbranchHierarchy"] != null)
        //    {
        //        ComponentTable = oDBEngine.GetDataTable("select '0' as ID,'-All-' as branch_description union all select branch_id as ID,branch_description from tbl_master_branch where branch_id in(" + Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) + ") order by branch_description asc");
        //    }
        //    if (ComponentTable.Rows.Count > 0)
        //    {

        //        Session["SI_ComponentData_Branch"] = ComponentTable;
        //        cmbBranchfilter.TextField = "branch_description";
        //        cmbBranchfilter.ValueField = "ID";
        //        cmbBranchfilter.DataSource = ComponentTable;
        //        cmbBranchfilter.DataBind();

        //    }
        //    else
        //    {
        //        cmbBranchfilter.DataSource = null;
        //        cmbBranchfilter.DataBind();

        //    }
        //}
        //protected void lookup_branch_DataBinding(object sender, EventArgs e)
        //{
        //    if (Session["SI_ComponentData_Branch"] != null)
        //    {
        //        //    DataTable ComponentTable = oDBEngine.GetDataTable("select branch_id,branch_description,branch_code from tbl_master_branch where branch_parentId='" + Hoid + "' order by branch_description asc");
        //        cmbBranchfilter.DataSource = (DataTable)Session["SI_ComponentData_Branch"];
        //    }
        //    else
        //    {
        //        cmbBranchfilter.DataSource = null;
        //    }
        //}

        #endregion

       

     
    }
}