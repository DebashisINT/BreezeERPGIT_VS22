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
using DataAccessLayer;
using Reports.Model;

namespace Reports.Reports.GridReports
{
    public partial class ProfitandLossStatement_Tabular : System.Web.UI.Page
    {
        //DataTable DTIndustry = new DataTable();
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        //string data = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/ProfitandLossStatement_Tabular.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            if (!IsPostBack)
            {
                //ACPrd.Text = "Accounting Period : " + Convert.ToDateTime(Session["FinYearStart"].ToString()).ToString("dd/MM/yyyy") + " To " + Convert.ToDateTime(Session["FinYearEnd"].ToString()).ToString("dd/MM/yyyy");
                Session["chk_presenttotal"] = 0;
                Session["chk_presenttotal1"] = 0;
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Profit & Loss Statement (Tabular)";
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

                Session["exportval"] = null;
                Session["exportval2"] = null;
                drdExport.SelectedIndex = 0;
                drdExport2.SelectedIndex = 0;
                Session["SI_ComponentData"] = null;
                Session["SI_ComponentData_ledger"] = null;
                //Session.Remove("dt_PLLeve2");
                //Session["dtProfitnLoss"] = null;
                Session["SI_ComponentData_Branch"] = null;
                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                Session["IsProfnLossFFilter"] = null;
                Session["IsProfnLossDetailsFilter"] = null;
                ASPxFromDate.Text = dtFrom.ToString("dd-MM-yyyy");
                ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");
                LayoutFill();
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                //Rev Subhra 20-12-2018   0017670
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
                //Session.Remove("dtProfitnLoss");
                ShowGrid.JSProperties["cpSave"] = null;
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
                BranchHoOffice();
            }
        }
        public void BranchHoOffice()
        {
            CommonBL bll1 = new CommonBL();
            DataTable stbill = new DataTable();
            //Rev Debashis && Hierarchy wise Head Branch Bind
            //stbill = bll1.GetBranchheadoffice("HO");
            DataTable dtBranchChild = new DataTable();
            stbill = bll1.GetBranchheadoffice(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), "HO");
            //End of Rev Debashis
            if (stbill.Rows.Count > 0)
            {
                ddlbranchHO.DataSource = stbill;
                ddlbranchHO.DataTextField = "Code";
                ddlbranchHO.DataValueField = "branch_id";
                ddlbranchHO.DataBind();
                //Rev Debashis && Hierarchy wise Head Branch Bind
                //ddlbranchHO.Items.Insert(0, new ListItem("All", "All"));
                dtBranchChild = GetChildBranch(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
                if (dtBranchChild.Rows.Count > 0)
                {
                    ddlbranchHO.Items.Insert(0, new ListItem("All", "All"));
                }
                //End of Rev Debashis
            }
        }

        //Rev Debashis && Hierarchy wise Head Branch Bind
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
        //End of Rev Debashis

        public void Date_finyearwise(string Finyear)
        {
            CommonBL bll1 = new CommonBL();
            DataTable stbill = new DataTable();
            DateTime MinDate, MaxDate;

            stbill = bll1.GetDateFinancila(Finyear);
            if (stbill.Rows.Count > 0)
            {
                ASPxFromDate.MaxDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_EndDate"]));
                ASPxFromDate.MinDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_StartDate"]));

                ASPxToDate.MaxDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_StartDate"]));

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
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    bindexport(Filter);
                }
                drdExport.SelectedValue = "0";
            }

        }

        protected void exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            e.BrickStyle.BackColor = Color.White;
            e.BrickStyle.ForeColor = Color.Black;
        }
        public void bindexport(int Filter)
        {
            string filename = Convert.ToString((Session["Contactrequesttype"] ?? "Profit & Loss Statement (Tabular-Expense)"));
            exporter.FileName = filename;
            string FileHeader = "";
            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();
            ShowGrid.Columns[2].Visible = false;
            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true,true, true, true) + Environment.NewLine + "Profit & Loss Statement (Tabular-Expense)" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
            //Rev Subhra 20-12-2018   0017670
            FileHeader = ReplaceFirst(FileHeader, "\r\n", Convert.ToString(Session["BranchNames"]));
            //End of Rev 
            exporter.RenderBrick += exporter_RenderBrick;
            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";
            exporter.Landscape = true;
            exporter.MaxColumnWidth = 100;
            exporter.GridViewID = "ShowGrid";
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
        //Rev Subhra 20-12-2018   0017670
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
        protected void drdExport2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport2.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval2"] == null)
                {
                    bindexport2(Filter);
                }
                else if (Convert.ToInt32(Session["exportval2"]) != Filter)
                {
                    bindexport2(Filter);
                }
                drdExport2.SelectedValue = "0";
            }
        }

        public void bindexport2(int Filter)
        {
            string filename = Convert.ToString((Session["Contactrequesttype"] ?? "Profit & Loss Statement (Tabular-Income)"));
            exporter2.FileName = filename;
            string FileHeader = "";
            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();
            ShowGrid_Income.Columns[2].Visible = false;
            ShowGrid_Income.Columns[2].Visible = false;
            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true,true, true, true) + Environment.NewLine + "Profit & Loss Statement (Tabular-Income)" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
           
            exporter2.RenderBrick += exporter_RenderBrick;
            exporter2.PageHeader.Left = FileHeader;
            exporter2.PageHeader.Font.Size = 10;
            exporter2.PageHeader.Font.Name = "Tahoma";
            exporter2.Landscape = true;
            exporter2.MaxColumnWidth = 100;
            exporter2.GridViewID = "ShowGrid_Income";
            switch (Filter)
            {
                case 1:
                    exporter2.WritePdfToResponse();

                    break;
                case 2:
                    exporter2.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 3:
                    exporter2.WriteRtfToResponse();
                    break;
                case 4:
                    exporter2.WriteCsvToResponse();
                    break;
            }

        }
        #region main grid details
        protected void ComponentPnL_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string branchid = Convert.ToString(e.Parameter.Split('~')[2]);
            string layoutid = Convert.ToString(e.Parameter.Split('~')[3]);
            bool is_asondate = false;
            //Session.Remove("dtProfitnLoss");
            string IsProfnLossFFilter = Convert.ToString(hfIsProfnLossFilter.Value);
            Session["IsProfnLossFFilter"] = IsProfnLossFFilter;
            ShowGrid.JSProperties["cpSave"] = null;
            string[] CallVal = e.Parameter.ToString().Split('~');
            is_asondate = Convert.ToBoolean(CallVal[1]);
            string asondate = "";
            if (is_asondate == false)
            {
                asondate = "N";
            }
            else
            {
                asondate = "Y";
            }

            string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

            DateTime dtFrom;
            DateTime dtTo;
            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);


            CommonBL bll1 = new CommonBL();
            DataTable stbill = new DataTable();
            stbill = bll1.GetDateFinancila(Finyear);

            if ((ASPxFromDate.Date <= Convert.ToDateTime((stbill.Rows[0]["FinYear_EndDate"])) && ASPxFromDate.Date >= Convert.ToDateTime((stbill.Rows[0]["FinYear_StartDate"]))) || (ASPxToDate.Date <= Convert.ToDateTime((stbill.Rows[0]["FinYear_EndDate"])) && ASPxToDate.Date >= Convert.ToDateTime((stbill.Rows[0]["FinYear_StartDate"]))))
            {
                string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                string TODATE = dtTo.ToString("yyyy-MM-dd");
                string BRANCH_ID = "";

                string QuoComponent = "";
                List<object> QuoList = lookup_branch.GridView.GetSelectedFieldValues("ID");
                foreach (object Quo in QuoList)
                {
                    QuoComponent += "," + Quo;
                }
                BRANCH_ID = QuoComponent.TrimStart(',');

                int checkshowzerobal = 0;
                if (chkZero.Checked == true)
                {
                    checkshowzerobal = 1;
                }
                else if (chkZero.Checked == false)
                {
                    checkshowzerobal = 0;
                }

                int checkBSPL = 0;
                if (chkPL.Checked == true)
                {
                    checkBSPL = 1;
                }
                else if (chkPL.Checked == false)
                {
                    checkBSPL = 0;
                }
                //Rev Subhra 20-12-2018   0017670
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
                ProffitnLossPanel.JSProperties["cpBranchNames"] = Convert.ToString(Session["BranchNames"]);
                //End of Rev
                Task PopulateStockTrialDataTask = new Task(() => GetPLdata(FROMDATE, TODATE, BRANCH_ID, asondate, checkshowzerobal, checkBSPL, branchid, layoutid));
                PopulateStockTrialDataTask.RunSynchronously();
                ShowGrid.ExpandAll();
                ShowGrid_Income.ExpandAll();
            }
            else
            {
                ShowGrid.JSProperties["cpErrorFinancial"] = "ErrorFinancial";
            }
        }
        public void GetPLdata(string FROMDATE, string TODATE, string BRANCH_ID, string asondate, int checkshowzerobal, int checkBSPL, string HeadBranch, string layoutid)
        {
            try
            {
                DataSet ds = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_PROFIT_AND_LOSS_TABULAR_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                cmd.Parameters.AddWithValue("@TODATE", TODATE);
                cmd.Parameters.AddWithValue("@BRANCH_ID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@AsonDate", asondate);
                cmd.Parameters.AddWithValue("@SHOWZEROBAL", checkshowzerobal);
                cmd.Parameters.AddWithValue("@SHOWBSPL", checkBSPL);
                cmd.Parameters.AddWithValue("@HO", HeadBranch);
                cmd.Parameters.AddWithValue("@LAYOUT_ID", layoutid);
                cmd.Parameters.AddWithValue("@VAL_TYPE", ddlValTech.SelectedValue);
                cmd.Parameters.AddWithValue("@OWMASTERVALTECH", (chkOWMSTVT.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@CONSLANDEDCOST", (chkConsLandCost.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@CONSOVERHEADCOST", (chkConsOverHeadCost.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@USERID", Convert.ToString(Session["userid"]).Trim());

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);

                cmd.Dispose();
                con.Dispose();

                //Session["dtProfitnLoss"] = ds.Tables[0];

                //ShowGrid.DataSource = ds.Tables[0];
                //ShowGrid.DataBind();

                //ShowGrid_Income.DataSource = ds.Tables[1];
                //ShowGrid_Income.DataBind();

            }
            catch (Exception ex)
            {
            }
        }
        public void LayoutFill()
        {
            CommonBL bll1 = new CommonBL();
            DataTable stbill = new DataTable();
            stbill = bll1.GetLayout("PL");
            if (stbill.Rows.Count > 0)
            {
                ddlLayout.DataSource = stbill;
                ddlLayout.DataTextField = "NAME";
                ddlLayout.DataValueField = "LAYOUT_ID";
                ddlLayout.DataBind();
            }
        }
        protected void ShowGrid_DataBound(object sender, EventArgs e)
        {
            ASPxGridView gridView = sender as ASPxGridView;
        }
        //protected void ShowGrid_DataBinding(object sender, EventArgs e)
        //{
            //if (Session["dtProfitnLoss"] != null)
            //{
            //    ShowGrid.DataSource = (DataTable)Session["dtProfitnLoss"];
            //}
      //  }
        protected void ShowGrid_Income_DataBound(object sender, EventArgs e)
        {
            ASPxGridView gridView = sender as ASPxGridView;
        }
        //protected void ShowGrid_Income_DataBinding(object sender, EventArgs e)
        //{
            //if (Session["dtProfitnLoss"] != null)
            //{
            //    ShowGrid.DataSource = (DataTable)Session["dtProfitnLoss"];
            //}
        //}

        #endregion

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
                    ComponentTable = oDBEngine.GetDataTable("select * from(select branch_id as ID,branch_description,branch_code from tbl_master_branch a where a.branch_id=1 union all select branch_id as ID,branch_description,branch_code from tbl_master_branch b where b.branch_parentId=1) a order by branch_description");
                    Session["SI_ComponentData_Branch"] = ComponentTable;
                    lookup_branch.DataSource = ComponentTable;
                    lookup_branch.DataBind();
                }
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
        protected void ddlExport3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(ddlExport3.SelectedItem.Value));
            if (Filter != 0)
            {
                bindexport_Details3(Filter);
            }
        }
        protected void ShowGridDetails2Level_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }
        public void bindexport_Details3(int Filter)
        {
            string filename = "Profit & Loss Statement (Tabular) Detail";
            exporterDetails.FileName = filename;
            exporterDetails.FileName = "Profit & Loss Statement (Tabular) Detail";

            string FileHeader = "";
            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();
            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true,true, true, true) + Environment.NewLine + "Profit & Loss Statement (Tabular) Detail" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
            exporterDetails.RenderBrick += exporter_RenderBrick;

            exporterDetails.PageHeader.Left = FileHeader;
            exporterDetails.PageHeader.Font.Size = 10;
            exporterDetails.PageHeader.Font.Name = "Tahoma";
            exporterDetails.PageFooter.Center = "[Page # of Pages #]";
            exporterDetails.PageFooter.Right = "[Date Printed]";
            exporterDetails.GridViewID = "ShowGridDetails2Level";
            switch (Filter)
            {
                case 1:
                    exporterDetails.WritePdfToResponse();
                    break;
                case 2:
                    exporterDetails.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 3:
                    exporterDetails.WriteRtfToResponse();
                    break;
                case 4:
                    exporterDetails.WriteCsvToResponse();
                    break;

                default:
                    return;
            }
        }

        #endregion

        protected void ShowGridDetails2Level_DataBinding(object sender, EventArgs e)
        {
            //if (Session["dt_PLLeve2"] != null)
            //{
            //    ShowGridDetails2Level.DataSource = (DataTable)Session["dt_PLLeve2"];
            //}
            //else
            //{
            //    ShowGridDetails2Level.DataSource = null;
            //}
        }
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string ledger;
            string asondatewise;
            string ledgerDesc;
            string ledgertype;
            string[] CallVal2ndlevel = e.Parameter.ToString().Split('~');
            asondatewise = Convert.ToString(CallVal2ndlevel[0]);
            ledger = CallVal2ndlevel[1];
            Session["LedgerId"] = ledger;
            ledgertype = CallVal2ndlevel[2];
            Session["ledgertype"] = ledgertype;

            string IsProfnLossDetailsFilter = Convert.ToString(hfIsProfnlossDetails.Value);
            Session["IsProfnLossDetailsFilter"] = IsProfnLossDetailsFilter;

            DataTable dtledgdesc = null;
            ledgerDesc = "";

            if (ledger != "null" && ledger != "0" && ledger != "")
            {
                if (ledgertype != "FOR BRANCH")
                {
                    dtledgdesc = oDBEngine.GetDataTable("Select MainAccount_Name from Master_MainAccount Where CONVERT(NVARCHAR(MAX),MainAccount_ReferenceID)='" + ledger + "'");
                }
                else
                {
                    dtledgdesc = oDBEngine.GetDataTable("Select branch_description from tbl_master_branch Where branch_id='" + ledger + "'");
                }

                ledgerDesc = dtledgdesc.Rows[0][0].ToString();
            }
            else
            {
                dtledgdesc = null;
                ledgerDesc = null;
            }


            if (!string.IsNullOrEmpty(ledger) && ledger != "0")
            {
                //Session.Remove("dt_PLLeve2");
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

                DateTime dtFrom;
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);

                string FROMDATE = dtFrom.ToString("yyyy-MM-dd");

                DateTime dtTo;
                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                string TODATE = dtTo.ToString("yyyy-MM-dd");

                string BRANCH_ID = "";

                string QuoComponent = "";
                List<object> QuoList = lookup_branch.GridView.GetSelectedFieldValues("ID");
                foreach (object Quo in QuoList)
                {
                    QuoComponent += "," + Quo;
                }
                BRANCH_ID = QuoComponent.TrimStart(',');

                string branchid = Convert.ToString(e.Parameter.Split('~')[3]);

                //DataTable dt = new DataTable();
                //dt = GetGeneralLedger2ndLevel(FROMDATE, TODATE, ledger, asondatewise, BRANCH_ID, branchid, ledgertype);

             

                Task PopulateStockTrialDataTask = new Task(() => GetGeneralLedger2ndLevel(FROMDATE, TODATE, ledger, asondatewise, BRANCH_ID, branchid, ledgertype));
                PopulateStockTrialDataTask.RunSynchronously();

                CallbackPanel.JSProperties["cpLedger"] = Convert.ToString(ledgerDesc);
                CallbackPanel.JSProperties["cpFromDate"] = dtFrom.ToString("dd-MM-yyyy");
                CallbackPanel.JSProperties["cpToDate"] = dtTo.ToString("dd-MM-yyyy");
                CallbackPanel.JSProperties["cpLedgerType"] = Convert.ToString(ledgertype);
                CallbackPanel.JSProperties["cpBlankLedger"] = "1";
                //Session["dt_PLLeve2"] = dt;
                //if (Session["dt_PLLeve2"] != null)
                //{
                //    if (dt.Rows.Count > 0)
                //    {
                //        ShowGridDetails2Level.JSProperties["cpLedger"] = Convert.ToString(ledgerDesc);
                //        ShowGridDetails2Level.JSProperties["cpFromDate"] = dtFrom.ToString("dd-MM-yyyy");
                //        ShowGridDetails2Level.JSProperties["cpToDate"] = dtTo.ToString("dd-MM-yyyy");
                //        ShowGridDetails2Level.JSProperties["cpLedgerType"] = Convert.ToString(ledgertype);
                //    }
                //}

                //ShowGridDetails2Level.DataSource = dt;
                //ShowGridDetails2Level.DataBind();
            }
            else
            {
                //Session["dt_PLLeve2"] = null;
                //ShowGridDetails2Level.DataSource = null;
                //ShowGridDetails2Level.DataBind();
                CallbackPanel.JSProperties["cpBlankLedger"] = "0";
            }
        }
        private void GetGeneralLedger2ndLevel(string FromDate, string ToDate, string ledger, string asondatewise, string BRANCH_ID, string HeadBranch, string ledgertype)
        {

            try
            {
                DataSet ds = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_PROFIT_AND_LOSS_STATEMENT_DETAILED_REPORT", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@FROMDATE", FromDate);
                cmd.Parameters.AddWithValue("@TODATE", ToDate);
                cmd.Parameters.AddWithValue("@BRANCH_ID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@AsonDate", asondatewise);
                cmd.Parameters.AddWithValue("@LEDGERID", ledger);
                cmd.Parameters.AddWithValue("@HO", HeadBranch);
                cmd.Parameters.AddWithValue("@LEGDERTYPE", ledgertype);
                cmd.Parameters.AddWithValue("@USERID", Convert.ToString(Session["userid"]).Trim());
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);

                cmd.Dispose();
                con.Dispose();

                //return ds.Tables[0];
            }
            catch (Exception ex)
            {
                //return null;
            }
        }

        protected void ShowGrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            String strcellvalue = Convert.ToString(e.CellValue);
            if (strcellvalue.Contains("Group Total for") == true || strcellvalue == "Total Expense" || strcellvalue == "Total" || strcellvalue == "Net Profit/Loss")
            {
                Session["chk_presenttotal"] = 1;
            }
            if (Convert.ToInt32(Session["chk_presenttotal"]) == 1)
            {
                e.Cell.Font.Bold = true;
            }
            if (e.DataColumn.FieldName == "PERIOD")
            {
                Session["chk_presenttotal"] = 0;
            }

        }

        protected void ShowGrid_Income_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            String strcellvalue = Convert.ToString(e.CellValue);
            if (strcellvalue.Contains("Group Total for") == true || strcellvalue == "Total Income" || strcellvalue == "Net Profit/Loss")
            {
                Session["chk_presenttotal1"] = 1;
            }
            if (Convert.ToInt32(Session["chk_presenttotal1"]) == 1)
            {
                e.Cell.Font.Bold = true;
            }
            if (e.DataColumn.FieldName == "PERIOD")
            {
                Session["chk_presenttotal1"] = 0;
            }

        }

        #region Linq

        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SLNO";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);
           
            if (Convert.ToString(Session["IsProfnLossFFilter"]) == "Y")
            {
                var q = from d in dc.PROFITANDLOSSTABULAR_REPORTs
                        where (d.LEDG_GRP_TYPE == "Expense" || d.LEDG_GRP_TYPE == "Net Total") && Convert.ToString(d.USERID) == Userid
                        orderby d.SLNO ascending
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.PROFITANDLOSSTABULAR_REPORTs
                        where Convert.ToString(d.SLNO) == "0"
                        orderby d.SLNO ascending
                        select d;
                e.QueryableSource = q;
            }
        }

        protected void GenerateEntityServerModeDataSourceIncome_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SLNO";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsProfnLossFFilter"]) == "Y")
            {
                var q = from d in dc.PROFITANDLOSSTABULAR_REPORTs
                        where d.LEDG_GRP_TYPE == "Income" && Convert.ToString(d.USERID) == Userid
                        orderby d.SLNO ascending
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.PROFITANDLOSSTABULAR_REPORTs
                        where Convert.ToString(d.SLNO) == "0"
                        orderby d.SLNO ascending
                        select d;
                e.QueryableSource = q;
            }
        }

        protected void GenerateEntityServerModeDataSourceDetails_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsProfnLossDetailsFilter"]) == "Y")
            {
                if (!string.IsNullOrEmpty(Session["LedgerId"].ToString()) && Session["LedgerId"].ToString() != "0")
                {
                    var q = from d in dc.PROFITANDLOSSDETAILSTATEMENT_REPORTs
                            where Convert.ToString(d.USERID) == Userid
                            orderby d.SEQ ascending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    var q = from d in dc.PROFITANDLOSSDETAILSTATEMENT_REPORTs
                            where Convert.ToString(d.SEQ) == "0"
                            orderby d.SEQ ascending
                            select d;
                    e.QueryableSource = q;
                }
              
            }
            else
            {
                var q = from d in dc.PROFITANDLOSSDETAILSTATEMENT_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ ascending
                        select d;
                e.QueryableSource = q;
            }
        }

        #endregion

    }
}