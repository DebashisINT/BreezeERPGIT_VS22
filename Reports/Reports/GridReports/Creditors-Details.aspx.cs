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
using Reports.Model;

namespace Reports.Reports.GridReports
{

    public partial class Creditors_Details : System.Web.UI.Page
    {
        //DataTable DTIndustry = new DataTable();
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        //string data = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Reports/master/Creditors-Details.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            if (!IsPostBack)
            {
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Creditors Details";
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

                Session["IsCreditorsDetFilter"] = null;
                //Session["dt_PartyLedgerRpt"] = null;
                Session["SI_ComponentData_Branch"] = null;

                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                //   BindDropDownList();
              //  ASPxFromDate.Text = dtFrom.ToString("dd-MM-yyyy");
                //ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");
                dtTo = Convert.ToDateTime(ASPxToDate.Date);
                string TODATE = dtTo.ToString("yyyy-MM-dd");
                ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");

                //cmbbranch.DataSource = CmbBranch();
                //cmbbranch.DataBind();
                //cmbbranch.Value = Convert.ToString(Session["userbranchID"]);

                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));

                //Rev Subhra 17-12-2018   0017670
                Session["BranchNames"] = null;
                //End Rev

                //dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                //dtTo = Convert.ToDateTime(ASPxToDate.Date);

               // string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                //string TODATE = dtTo.ToString("yyyy-MM-dd");

                //string TABLENAME = "Ledger Details";

                string BRANCH_ID = "";
                //if (hdnSelectedBranches.Value != "")
                //{
                //    BRANCH_ID = hdnSelectedBranches.Value;
                //}


                //Task PopulateStockTrialDataTask = new Task(() => GetCreditorDetails(TODATE, BRANCH_ID));
                //PopulateStockTrialDataTask.RunSynchronously(); 

                Session["SI_ComponentData_Branch"] = null;
            }
            else
            {
                //dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
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
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    bindexport(Filter);
                }

                drdExport.SelectedValue = "0";
            }
        }

  
        public void bindexport(int Filter)
        {
            string filename = "Creditors Dtails";
            exporter.FileName = filename;
            exporter.PageHeader.Left = "Creditors Dtails";

            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true,true, true, true, true) + Environment.NewLine + "Creditors Dtails" + Environment.NewLine + "As on " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
            //Rev Subhra 17-12-2018   0017670
            FileHeader = ReplaceFirst(FileHeader, "\r\n", Convert.ToString(Session["BranchNames"]));
            //End of Rev
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
        //Rev Subhra 17-12-2018   0017670
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

        //protected void Grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            //Session.Remove("dt_PartyLedgerRpt");
            //ShowGrid.JSProperties["cpSave"] = null;

            string IsCreditorsDetFilter = Convert.ToString(hfIsCreditorsDetFilter.Value);
            Session["IsCreditorsDetFilter"] = IsCreditorsDetFilter;

            string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

            //DateTime dtFrom;
            DateTime dtTo;
        //    dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);

         //   string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");

            //string TABLENAME = "Ledger Details";

            string BRANCH_ID = "";

            string BranchList = "";
            List<object> BranchList1 = lookup_branch.GridView.GetSelectedFieldValues("branch_id");
            foreach (object Branch in BranchList1)
            {
                BranchList += "," + Branch;
            }
            BRANCH_ID = BranchList.TrimStart(',');


            //string CUSTVENDID = "";
            //string QuoComponent = "";
           
            //CUSTVENDID = QuoComponent.TrimStart(',');

            //Rev Subhra 17-12-2018   0017670

            string BRANCH_NAME = "";
            string BranchNameComponent = "";
            List<object> BranchNameList = lookup_branch.GridView.GetSelectedFieldValues("Code");
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

            //End of Rev

            Task PopulateStockTrialDataTask = new Task(() => GetCreditorDetails(TODATE, BRANCH_ID));
            PopulateStockTrialDataTask.RunSynchronously(); 
        }



        public void GetCreditorDetails(string TODATE,string BranchId)
        {
            try
            {
                //string DriverName = string.Empty;
                //string PhoneNo = string.Empty;
                //string VehicleNo = string.Empty;

                DataSet ds = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_CreditorsDetails_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]).Trim());
                cmd.Parameters.AddWithValue("@Asondate", TODATE);
                cmd.Parameters.AddWithValue("@P_BRANCH_ID", BranchId);
                //cmd.Parameters.AddWithValue("@REPORT", 0);
                cmd.Parameters.AddWithValue("@WHICHMODULE", "Y");
                cmd.Parameters.AddWithValue("@USERID", Convert.ToInt32(Session["userid"]));

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);

                cmd.Dispose();
                con.Dispose();

                //Session["dt_PartyLedgerRpt"] = ds.Tables[0];

                //ShowGrid.DataSource = ds.Tables[0];
                //ShowGrid.DataBind();
            }
            catch (Exception ex)
            {
            }
        }

        public void Date_finyearwise(string Finyear)
        {
            //CommonBL bll1 = new CommonBL();
            //DataTable stbill = new DataTable();
            //stbill = bll1.GetDateFinancila(Finyear);
            //if (stbill.Rows.Count > 0)
            //{
            //    //ASPxFromDate.Text = Convert.ToDateTime(stbill.Rows[0]["FinYear_StartDate"]).ToString("dd-MM-yyyy");
            //    ASPxToDate.Text = Convert.ToDateTime(stbill.Rows[0]["FinYear_EndDate"]).ToString("dd-MM-yyyy");
            //}

            CommonBL debdet = new CommonBL();
            DataTable dtdebdet = new DataTable();

            dtdebdet = debdet.GetDateFinancila(Finyear);
            if (dtdebdet.Rows.Count > 0)
            {
                ASPxToDate.MaxDate = Convert.ToDateTime((dtdebdet.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((dtdebdet.Rows[0]["FinYear_StartDate"]));
                DateTime MaximumDate = Convert.ToDateTime((dtdebdet.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((dtdebdet.Rows[0]["FinYear_StartDate"]));
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
     
        //protected void Showgrid_Htmlprepared(object sender, EventArgs e)
        //{
           
        //}


        #region Branch Populate

        protected void Componentbranch_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string FinYear = Convert.ToString(Session["LastFinYear"]);

            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                DataTable ComponentTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];
                if (Session["userbranchHierarchy"] != null)
                {
                    //Commented By Subhra 18-04-2018
                    //ComponentTable = oDBEngine.GetDataTable("SELECT DISTINCT A.branch_id,A.branch_description as Code FROM tbl_master_branch A, tbl_master_branch B WHERE A.branch_id=B.branch_parentId ORDER BY A.branch_id asc");
                    ComponentTable = oDBEngine.GetDataTable("SELECT  A.branch_id,A.branch_description as Code FROM tbl_master_branch A WHERE A.branch_parentId=0 UNION SELECT  A.branch_id,A.branch_description as Code FROM tbl_master_branch A, tbl_master_branch B WHERE A.branch_id=B.branch_parentId AND B.branch_parentId <>0");
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

        protected void lookup_branch_DataBinding(object sender, EventArgs e)
        {

            //DataTable ComponentTable = ComponentTable = oDBEngine.GetDataTable("SELECT DISTINCT A.branch_id,A.branch_description as Code FROM tbl_master_branch A, tbl_master_branch B WHERE A.branch_id=B.branch_parentId ORDER BY A.branch_id asc");
            //lookup_branch.DataSource = ComponentTable;
            if (Session["SI_ComponentData_Branch"] != null)
            {
                lookup_branch.DataSource = (DataTable)Session["SI_ComponentData_Branch"];
            }

        }

        #endregion

        //protected void grid2_DataBinding(object sender, EventArgs e)
        //{
        //    if (Session["dt_PartyLedgerRpt"] != null)
        //    {
        //        ShowGrid.DataSource = (DataTable)Session["dt_PartyLedgerRpt"];
        //    }
        //}

        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }

        #region LinQ
        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsCreditorsDetFilter"]) == "Y")
            {
                var q = from d in dc.CREDITORSDETAILS_REPORTs
                        where Convert.ToString(d.USERID) == Userid 
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.CREDITORSDETAILS_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
        }
        #endregion

    }
}