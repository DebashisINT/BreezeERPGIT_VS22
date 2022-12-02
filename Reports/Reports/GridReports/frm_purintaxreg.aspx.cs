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
using DataAccessLayer;
using System.Threading.Tasks;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using System.Drawing;
using Reports.Model;

namespace Reports.Reports.GridReports
{
    public partial class frm_purintaxreg : ERP.OMS.ViewState_class.VSPage
    {
        ReportData rpt = new ReportData();


        DataTable DTIndustry = new DataTable();
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        string data = "";
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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Reports/master/GstrReport.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            if (!IsPostBack)
            {
                DataTable dtBranchSelection = new DataTable();
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "GST Input Tax Register";
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
                Session["GSTIN"] = null;
                Session["IsPurchaseInTaxFilter"] = null;
                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                //   BindDropDownList();
                chkPartyInvDt.Checked = false;
                ASPxFromDate.Text = dtFrom.ToString("dd-MM-yyyy");
                ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");
                //  Date_finyearwise(Convert.ToString(Session["LastFinYear"]));

                ASPxFromDate.Value = DateTime.Now;
                ASPxToDate.Value = DateTime.Now;
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                BranchpopulateGSTN();
                //Rev Subhra 18-12-2018   0017670
                Session["BranchNames"] = null;
                //End Rev
                dtBranchSelection = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='ReportsBranchSelection'");
                hdnBranchSelection.Value = dtBranchSelection.Rows[0][0].ToString();
                //==========================================for document Type==================================================

                DataTable dtdoctype = new DataTable();
                dtdoctype.Columns.Add("doctype_code", typeof(string));
                dtdoctype.Columns.Add("doctype_description", typeof(string));

                DataRow dr;
                dr = dtdoctype.NewRow();
                dr["doctype_code"] = "PB";
                dr["doctype_description"] = "Purchase Invoice";
                dtdoctype.Rows.Add(dr);
                dr = dtdoctype.NewRow();
                dr["doctype_code"] = "TPI";
                dr["doctype_description"] = "Transit Purchase Invoice";
                dtdoctype.Rows.Add(dr);
                dr = dtdoctype.NewRow();
                dr["doctype_code"] = "IPINV";
                dr["doctype_description"] = "Import Purchase Invoice";
                dtdoctype.Rows.Add(dr);
                dr = dtdoctype.NewRow();
                dr["doctype_code"] = "PR";
                dr["doctype_description"] = "Purchase Return Normal";
                dtdoctype.Rows.Add(dr);
                dr = dtdoctype.NewRow();
                dr["doctype_code"] = "PRM";
                dr["doctype_description"] = "Purchase Return  Manual";
                dtdoctype.Rows.Add(dr);
                dr = dtdoctype.NewRow();
                dr["doctype_code"] = "VENP";
                dr["doctype_description"] = "Vendor Payment";
                dtdoctype.Rows.Add(dr);
                dr = dtdoctype.NewRow();
                dr["doctype_code"] = "VENDN";
                dr["doctype_description"] = "Vendor Debit Note";
                dtdoctype.Rows.Add(dr);
                dr = dtdoctype.NewRow();
                dr["doctype_code"] = "VENR";
                dr["doctype_description"] = "Vendor Receipt";
                dtdoctype.Rows.Add(dr);
                dr = dtdoctype.NewRow();
                dr["doctype_code"] = "VENCN";
                dr["doctype_description"] = "Vendor Credit Note";
                dtdoctype.Rows.Add(dr);
                dr = dtdoctype.NewRow();
                dr["doctype_code"] = "CASHBANK_P";
                dr["doctype_description"] = "Cash/Bank";
                dtdoctype.Rows.Add(dr);
                dr = dtdoctype.NewRow();
                dr["doctype_code"] = "JOURNAL";
                dr["doctype_description"] = "Journal";
                dtdoctype.Rows.Add(dr);

                Session["SI_DocumentType"] = dtdoctype;

                if (Session["SI_DocumentType"] != null)
                {
                    lookup_doctype.DataSource = (DataTable)Session["SI_DocumentType"];
                    lookup_doctype.DataBind();
                }
                //============================================================================================================
            }
            else
            {

            }

        }
        protected void lookup_doctype_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_DocumentType"] != null)
            {
                lookup_doctype.DataSource = (DataTable)Session["SI_DocumentType"];
            }
        }
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

        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {


                if (Session["exportval"] == null)
                {
                    //  Session["exportval"] = Filter;
                    // BindDropDownList();
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    // Session["exportval"] = Filter;
                    // BindDropDownList();
                    bindexport(Filter);
                }
            }

        }
        public void BindDropDownList()
        {
            // Declare a Dictionary to hold all the Options with Value and Text.
            Dictionary<string, string> options = new Dictionary<string, string>();
            options.Add("0", "Export to");
            options.Add("1", "PDF");
            options.Add("2", "XLS");
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
            if (OPTREGGrid.VisibleRowCount > 0)
            {
                string filename = "GST Input Tax Register";
                exporter.FileName = filename;
                string FileHeader = "";

                exporter.FileName = filename;

                BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

                FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "GST Input Tax Register" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
                //Rev Subhra 18-12-2018   0017670
                FileHeader = ReplaceFirst(FileHeader, "\r\n", Convert.ToString(Session["BranchNames"]));
                //End of Rev
                exporter.RenderBrick += exporter_RenderBrick;
                exporter.PageHeader.Left = FileHeader;
                exporter.PageHeader.Font.Size = 10;
                exporter.PageHeader.Font.Name = "Tahoma";

                //exporter.PageFooter.Center = "[Page # of Pages #]";
                //exporter.PageFooter.Right = "[Date Printed]";
                exporter.GridViewID = "OPTREGGrid";
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
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "jAlert('There is no record to export.');", true);
                // return;
                BranchpopulateGSTN();
            }
        }

        //Rev Subhra 18-12-2018   0017670
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

        //protected void BindLedgerPosting()
        //{
        //    try
        //    {
        //        if (Session["dtLedger"] != null)
        //        {
        //            ShowGrid.DataSource = (DataTable)Session["dtLedger"];
        //            ShowGrid.DataBind();
        //        }
        //    }
        //    catch { }
        //}

        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            //Session.Remove("dt_SaleOutputtaxGstReg");
            OPTREGGrid.JSProperties["cpSave"] = null;

            string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

            string IsPurchaseInTaxFilter = Convert.ToString(hfIsPurchaseInTaxFilter.Value);
            Session["IsPurchaseInTaxFilter"] = IsPurchaseInTaxFilter;

            DateTime dtFrom;
            DateTime dtTo;

            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);

            //dtFrom = Convert.ToDateTime(DateTime.ParseExact(FromDate.Value, "dd-MM-yyyy", CultureInfo.InvariantCulture));
            //dtTo = Convert.ToDateTime(DateTime.ParseExact(ToDate.Value, "dd-MM-yyyy", CultureInfo.InvariantCulture));

            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");

            string BRANCH_ID = "";

            string BranchComponent = "";
            if (Session["SI_ComponentData_Branch"] != null)
            {
                if (lookup_branch.GridView.GetSelectedFieldValues("branch_id").Count() != ((DataTable)Session["SI_ComponentData_Branch"]).Rows.Count)
                {
                    List<object> BranchList = lookup_branch.GridView.GetSelectedFieldValues("branch_id");
                    foreach (object Branch in BranchList)
                    {
                        BranchComponent += "," + Branch;
                    }
                    BRANCH_ID = BranchComponent.TrimStart(',');
                }

                string DoctypeComponent = "";
                string DOCTYPE_ID = "";
                List<object> DoctypeList = lookup_doctype.GridView.GetSelectedFieldValues("doctype_code");
                foreach (object DocType in DoctypeList)
                {
                    DoctypeComponent += "," + DocType;
                }
                DOCTYPE_ID = DoctypeComponent.TrimStart(',');
                string GSTINID = e.Parameter.Split('~')[1];
                Session["GSTIN"] = GSTINID;
                string vendinternlid = Convert.ToString(hdnVendorId.Value);
                int checkpartyinvdate = 0;
                if (chkPartyInvDt.Checked == true)
                {
                    checkpartyinvdate = 1;
                }
                else if (chkPartyInvDt.Checked == false)
                {
                    //checkpartyinvdate = 2;
                    checkpartyinvdate = 0;
                }

                //Rev Subhra 18-12-2018   0017670

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

                Task PopulateStockTrialDataTask = new Task(() => GetINTREGdata(FROMDATE, TODATE, BRANCH_ID, DOCTYPE_ID, GSTINID, vendinternlid, checkpartyinvdate));
                PopulateStockTrialDataTask.RunSynchronously();
            }

        }

        public void GetINTREGdata(string FROMDATE, string TODATE, string BRANCH_ID, string DOCTYPE_ID, string GSTINID, string vendinternlid, int checkpartyinvdate)
        {
            try
            {

                string DriverName = string.Empty;
                string PhoneNo = string.Empty;
                string VehicleNo = string.Empty;

                DataSet ds = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_PURCHASE_GST_INPTAXREG_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]).Trim());
                cmd.Parameters.AddWithValue("@GSTIN", GSTINID);
                cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                cmd.Parameters.AddWithValue("@TODATE", TODATE);
                cmd.Parameters.AddWithValue("@BRANCH_ID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@DOCTYPE", DOCTYPE_ID);
                cmd.Parameters.AddWithValue("@VENDORID", vendinternlid);
                cmd.Parameters.AddWithValue("@SEARCHBYPRTYINVDT", checkpartyinvdate);
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
        protected void OPTREGGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }

        protected void OPTREGGrid_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {

        }


        #region ########  Branch GRN Populate  #######
        protected void BranchpopulateGSTN()
        {
            // DataTable dst = new DataTable();
            string userbranchID = Convert.ToString(Session["userbranchID"]);

            DataSet ds = new DataSet();
            //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("GetGSTNfetch", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Company", Convert.ToString(Session["LastCompany"]));
            cmd.Parameters.AddWithValue("@Branchlist", Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(ds);

            cmd.Dispose();
            con.Dispose();


            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlgstn.DataSource = ds.Tables[0];
                ddlgstn.DataTextField = "branch_GSTIN";
                ddlgstn.DataValueField = "branch_GSTIN";
                ddlgstn.DataBind();
                ddlgstn.Items.Insert(0, "");
            }


        }

        #endregion
        private int totalCount;
        private int totalCountrecp;

        private List<string> Invoice_Number = new List<string>();

        private List<string> ReceiptPayment_VoucherNumber = new List<string>();

        protected void ASPxGridView1_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {

        }

        protected void ASPxCallbackPanel1_Callback(object sender, CallbackEventArgsBase e)
        {

            string FinYear = Convert.ToString(Session["LastFinYear"]);

            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                DataTable ComponentTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];

                if (Hoid != "0")
                {
                    DataSet ds = new DataSet();
                    //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                    SqlCommand cmd = new SqlCommand("Getbranchlist_Gsitnwise", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@GstinId", ddlgstn.SelectedValue);
                    cmd.Parameters.AddWithValue("@Branch", Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
                    cmd.Parameters.AddWithValue("@GstinId", Hoid);
                    cmd.CommandTimeout = 0;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(ds);

                    cmd.Dispose();
                    con.Dispose();


                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Session["SI_ComponentData_Branch"] = ds.Tables[0];
                        lookup_branch.DataSource = ds.Tables[0];
                        lookup_branch.DataBind();
                    }
                    else
                    {
                        Session["SI_ComponentData_Branch"] = ds.Tables[0];
                        lookup_branch.DataSource = null;
                        lookup_branch.DataBind();
                    }

                }
                else
                {
                    Session["SI_ComponentData_Branch"] = null;
                    lookup_branch.DataSource = null;
                    lookup_branch.DataBind();
                }
            }
        }

        protected void lookup_branch_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_ComponentData_Branch"] != null)
            {
                //    DataTable ComponentTable = oDBEngine.GetDataTable("select branch_id,branch_description,branch_code from tbl_master_branch where branch_parentId='" + Hoid + "' order by branch_description asc");
                lookup_branch.DataSource = (DataTable)Session["SI_ComponentData_Branch"];
            }
        }

        #region LinQ
        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsPurchaseInTaxFilter"]) == "Y")
            {
                var q = from d in dc.PURCHASE_GST_INPTAXREG_REPORTs
                        where Convert.ToString(d.USERID) == Userid
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.PURCHASE_GST_INPTAXREG_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            OPTREGGrid.ExpandAll();
        }

        #endregion

    }
}