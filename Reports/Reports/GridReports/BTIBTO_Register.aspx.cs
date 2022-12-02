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
    public partial class BTIBTO_Register : System.Web.UI.Page
    {
        DataTable DTIndustry = new DataTable();
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
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
                Session["chk_presenttotal"] = 0;
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "BTI/BTO Register";
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

                Session["IsBTIBTOFilter"] = null;
                Session["SI_ComponentData_Branch"] = null;
                Session["exportval"] = null;
                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                ASPxFromDate.Text = dtFrom.ToString("dd-MM-yyyy");
                ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");

                ASPxFromDate.Value = DateTime.Now;
                ASPxToDate.Value = DateTime.Now;
                BranchHoOffice();
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                //Rev Subhra 24-12-2018   0017670
                Session["BranchNames"] = null;
                //End Rev
            }
            else
            {
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);
            }

            if (!IsPostBack)
            {
                //Rev Debashis
                //Session.Remove("dt_BTIBTO_Register_Rpt");
                //End of Rev Debashis
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                string TODATE = dtTo.ToString("yyyy-MM-dd");
                string BRANCH_ID = "";
                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }
            }
        }

        public void Date_finyearwise(string Finyear)
        {
            //Rev Debashis
            //CommonBL bll1 = new CommonBL();
            //DataTable stbill = new DataTable();
            //DateTime MinDate, MaxDate;

            //stbill = bll1.GetDateFinancila(Finyear);
            //if (stbill.Rows.Count > 0)
            //{

            //    ASPxFromDate.MaxDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_EndDate"]));
            //    ASPxFromDate.MinDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_StartDate"]));

            //    ASPxToDate.MaxDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_EndDate"]));
            //    ASPxToDate.MinDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_StartDate"]));

            //    DateTime MaximumDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_EndDate"]));
            //    DateTime MinimumDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_StartDate"]));

            //    DateTime TodayDate = Convert.ToDateTime(DateTime.Now);
            //    DateTime FinYearEndDate = MaximumDate;

            //    if (TodayDate > FinYearEndDate)
            //    {
            //        ASPxToDate.Date = FinYearEndDate;
            //        ASPxFromDate.Date = MinimumDate;
            //    }
            //    else
            //    {
            //        ASPxToDate.Date = TodayDate;
            //        ASPxFromDate.Date = MinimumDate;
            //    }

            //}
            CommonBL btibto = new CommonBL();
            DataTable dtbtibto = new DataTable();

            dtbtibto = btibto.GetDateFinancila(Finyear);
            if (dtbtibto.Rows.Count > 0)
            {
                ASPxFromDate.MaxDate = Convert.ToDateTime((dtbtibto.Rows[0]["FinYear_EndDate"]));
                ASPxFromDate.MinDate = Convert.ToDateTime((dtbtibto.Rows[0]["FinYear_StartDate"]));

                ASPxToDate.MaxDate = Convert.ToDateTime((dtbtibto.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((dtbtibto.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((dtbtibto.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((dtbtibto.Rows[0]["FinYear_StartDate"]));

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
            //End of Rev Debashis
        }
        public void BranchHoOffice()
        {
            //Rev Debashis
            //CommonBL bll1 = new CommonBL();
            //DataTable stbill = new DataTable();
            //stbill = bll1.GetBranchheadoffice("HO");
            //if (stbill.Rows.Count > 0)
            //{
            //    ddlbranchHO.DataSource = stbill;
            //    ddlbranchHO.DataTextField = "Code";
            //    ddlbranchHO.DataValueField = "branch_id";
            //    ddlbranchHO.DataBind();
            //    //ddlbranchHO.Items.Insert(0, new ListItem("All", "All"));
            //}
            CommonBL blbranch = new CommonBL();
            DataTable dtbranch = new DataTable();
            //Rev Debashis && Hierarchy wise Head Branch Bind
            //dtbranch = blbranch.GetBranchheadoffice("HO");
            DataTable dtBranchChild = new DataTable();
            dtbranch = blbranch.GetBranchheadoffice(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), "HO");
            //End of Rev Debashis
            if (dtbranch.Rows.Count > 0)
            {
                ddlbranchHO.DataSource = dtbranch;
                ddlbranchHO.DataTextField = "Code";
                ddlbranchHO.DataValueField = "branch_id";
                ddlbranchHO.DataBind();
            }
            //End of Rev Debashis
        }
        #region Branch Populate
        protected void Componentbranch_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string FinYear = Convert.ToString(Session["LastFinYear"]);

            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                DataTable ComponentTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];

                //if (Hoid != "All")
                //{
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
                //}
                //else
                //{
                //    ComponentTable = oDBEngine.GetDataTable("select * from(select branch_id as ID,branch_description,branch_code from tbl_master_branch a where a.branch_id=1 union all select branch_id as ID,branch_description,branch_code from tbl_master_branch b where b.branch_parentId=1) a order by branch_description");
                //    Session["SI_ComponentData_Branch"] = ComponentTable;
                //    lookup_branch.DataSource = ComponentTable;
                //    lookup_branch.DataBind();
                //}
            }
        }
        public DataTable GetBranch(string BRANCH_ID, string Ho)
        {
            DataTable dt = new DataTable();
            //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
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

            string filename = "BTI/BTO Register";
            exporter.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "BTI/BTO Register" + Environment.NewLine + "For the period ";
            //Rev Subhra 24-12-2018   0017670
            FileHeader = ReplaceFirst(FileHeader, "\r\n", Convert.ToString(Session["BranchNames"]));
            //End of Rev
            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";

            exporter.GridViewID = "ShowGridList";
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

                default:
                    return;
            }

        }
        //Rev Subhra 24-12-2018   0017670
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

        #region BTI/BTO Register grid
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            //Rev Debashis
            //string type = "PR";
            //Session.Remove("dt_BTIBTO_Register_Rpt");
            //End of Rev Debashis

            string IsBTIBTOFilter = Convert.ToString(hfIsBTOBTIFilter.Value);
            Session["IsBTIBTOFilter"] = IsBTIBTOFilter;

            string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

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

                   
            string transfer_type=ddltransfertype.SelectedValue;
            //Rev Subhra 24-12-2018   0017670
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
            //End of Rev
            Task PopulateStockTrialDataTask = new Task(() => GetBTIBTORegdata(BRANCH_ID,FROMDATE,TODATE,transfer_type));
            PopulateStockTrialDataTask.RunSynchronously();
        }
        public void GetBTIBTORegdata(string BRANCH_ID,string FROMDATE, string TODATE,string transfer_type)
        {
            try
            {
                DataSet ds = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_BRANCHTRANSFERINOUTREGISTER_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                cmd.Parameters.AddWithValue("@TODATE", TODATE);
                cmd.Parameters.AddWithValue("@BRANCHID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@PRODUCT_CODE", hdnProductId.Value);
                cmd.Parameters.AddWithValue("@TRANSFER_TYPE", transfer_type);
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

        protected void ShowGridList_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            if (e.Item == ShowGridList.TotalSummary["PRODUCTS_NAME"])
            {
                e.Text = "Total";
            }
            else
            { 
                e.Text = string.Format("{0}", Math.Abs(Convert.ToDecimal(e.Value)));
            }
        }

        #endregion

        #region LinQ
        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SLNO";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsBTIBTOFilter"]) == "Y")
            {
                var q = from d in dc.BRANCHTRANSFERINOUT_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.TRANSFER_TYPE) == ddltransfertype.SelectedValue
                        orderby d.SLNO
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.BRANCHTRANSFERINOUT_REPORTs
                        where Convert.ToString(d.SLNO) == "0"
                        orderby d.SLNO
                        select d;
                e.QueryableSource = q;
            }
            ShowGridList.ExpandAll();
        }
        #endregion

    }
}