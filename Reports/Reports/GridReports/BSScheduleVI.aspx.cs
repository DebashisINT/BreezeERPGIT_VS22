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
using DevExpress.Web.Data;

namespace Reports.Reports.GridReports
{
    public partial class BSScheduleVI : System.Web.UI.Page
    {
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        bool blBold=false;
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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/BSScheduleVI.aspx");
            DateTime dtAsOn;
            if (!IsPostBack)
            {
                Session["chk_presenttotal"] = 0;
                Session["chk_presentSchld"] = 0;
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Consolidated Balance Sheet";
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

                drdExport.SelectedIndex = 0;
                Session["SI_ComponentData"] = null;
                Session["SI_ComponentData_ledger"] = null;
                Session["IsBSScheduleVIFilter"] = null;
                Session["IsBSScheduleVIL1Filter"] = null;

                Session["SI_ComponentData_Branch"] = null;
                dtAsOn = DateTime.Now;
                ASPxAsOnDate.Text = dtAsOn.ToString("dd-MM-yyyy");
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                //Rev Subhra 24-12-2018   0017670
                Session["BranchNames"] = null;
                //End Rev
            }
            else
            {
                dtAsOn = Convert.ToDateTime(ASPxAsOnDate.Date);
            }

            if (!IsPostBack)
            {
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
                dtAsOn = Convert.ToDateTime(ASPxAsOnDate.Date);
                string ASONDATE = dtAsOn.ToString("yyyy-MM-dd");
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
            CommonBL bsschldvi = new CommonBL();
            DataTable dtbsschldvi = new DataTable();

            dtbsschldvi = bsschldvi.GetDateFinancila(Finyear);
            if (dtbsschldvi.Rows.Count > 0)
            {

                ASPxAsOnDate.MaxDate = Convert.ToDateTime((dtbsschldvi.Rows[0]["FinYear_EndDate"]));
                ASPxAsOnDate.MinDate = Convert.ToDateTime((dtbsschldvi.Rows[0]["FinYear_StartDate"]));


                DateTime MaximumDate = Convert.ToDateTime((dtbsschldvi.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((dtbsschldvi.Rows[0]["FinYear_StartDate"]));

                DateTime TodayDate = Convert.ToDateTime(DateTime.Now);
                DateTime FinYearEndDate = MaximumDate;

                if (TodayDate > FinYearEndDate)
                {
                    ASPxAsOnDate.Date = FinYearEndDate;
                }
                else
                {
                    ASPxAsOnDate.Date = TodayDate;
                }

            }
        }

        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                BranchHoOffice();
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

        protected void exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            e.BrickStyle.BackColor = Color.White;
            e.BrickStyle.ForeColor = Color.Black;
        }

        public void bindexport(int Filter)
        {
            DateTime dtAsOn;
            dtAsOn = Convert.ToDateTime(ASPxAsOnDate.Date);
            string ASONDATE = dtAsOn.ToString("dd-MM-yyyy");
            string HASONDATE = dtAsOn.ToString("dd MMMM , yyyy");
            DateTime dtPAsOn;
            dtPAsOn = Convert.ToDateTime(ASPxAsOnDate.Date.AddYears(-1));
            string PREVIOUSASONDATE = dtPAsOn.ToString("dd-MM-yyyy");

            ShowGrid.Columns[0].Visible = false;
            ShowGrid.Columns[3].Caption = "As at " + ASONDATE;
            ShowGrid.Columns[4].Caption = "As at " + PREVIOUSASONDATE;
            ShowGrid.Columns[5].Visible = false;

            string filename = Convert.ToString((Session["Contactrequesttype"] ?? "Consolidated Balance Sheet"));
            exporter.FileName = filename;
            string FileHeader = "";
            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();
            //FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Consolidated Balance Sheet" + Environment.NewLine + "As On: " + Convert.ToDateTime(ASPxAsOnDate.Date).ToString("dd-MM-yyyy");
            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Consolidated Balance Sheet" + Environment.NewLine + "As at " + HASONDATE;
            //Rev Subhra 24-12-2018   0017670
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
                    exporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 2:
                    exporter.WritePdfToResponse();
                    break;
                case 3:
                    exporter.WriteCsvToResponse();
                    break;
                case 4:
                    exporter.WriteRtfToResponse();
                    break;
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
        #region main grid details
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string branchid = Convert.ToString(e.Parameter.Split('~')[0]);
            string IsBSScheduleVIFilter = Convert.ToString(hfIsBSScheduleVIFilter.Value);
            Session["IsBSScheduleVIFilter"] = IsBSScheduleVIFilter;
            
            string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

            DateTime dtAsOn;
            dtAsOn = Convert.ToDateTime(ASPxAsOnDate.Date);

            string ASONDATE = dtAsOn.ToString("yyyy-MM-dd");
            string BRANCH_ID = "";

            string Branch = "";
            List<object> BranchList = lookup_branch.GridView.GetSelectedFieldValues("ID");
            foreach (object B in BranchList)
            {
                Branch += "," + B;
            }
            BRANCH_ID = Branch.TrimStart(',');

            int checkCLSTK = 0;
            if (chkCLSTK.Checked == true)
            {
                checkCLSTK = 1;
            }
            else if (chkCLSTK.Checked == false)
            {
                checkCLSTK = 0;
            }
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
            Task PopulateStockTrialDataTask = new Task(() => GetBSScheduleVIdata(ASONDATE, BRANCH_ID, checkCLSTK, branchid));
            PopulateStockTrialDataTask.RunSynchronously();
            ShowGrid.ExpandAll();
        }

        public void GetBSScheduleVIdata(string ASONDATE, string BRANCH_ID, int checkCLSTK, string HeadBranch)
        {
            try
            {
                DataSet ds = new DataSet();

               // SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);  MULTI


                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                SqlCommand cmd = new SqlCommand("PRC_BALANCESHEETSCHEDULEVI_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@ASONDATE", ASONDATE);
                cmd.Parameters.AddWithValue("@BRANCH_ID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@SHOWCLSTK", checkCLSTK);
                cmd.Parameters.AddWithValue("@UNITS", ddlunits.SelectedValue);
                cmd.Parameters.AddWithValue("@HO", HeadBranch);
                cmd.Parameters.AddWithValue("@CONSIDERRE", (chkRE.Checked) ? "1" : "0");
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

                if (ddlunits.SelectedValue == "1")
                {
                    ShowGrid.JSProperties["cpUnit"] = "Ones";
                }
                else if (ddlunits.SelectedValue == "10")
                {
                    ShowGrid.JSProperties["cpUnit"] = "Tens";
                }
                else if (ddlunits.SelectedValue == "100")
                {
                    ShowGrid.JSProperties["cpUnit"] = "Hundreds";
                }
                else if (ddlunits.SelectedValue == "1000")
                {
                    ShowGrid.JSProperties["cpUnit"] = "Thousands";
                }
                else if (ddlunits.SelectedValue == "10000")
                {
                    ShowGrid.JSProperties["cpUnit"] = "Ten Thousands";
                }
                else if (ddlunits.SelectedValue == "100000")
                {
                    ShowGrid.JSProperties["cpUnit"] = "Lakhs";
                }
                else if (ddlunits.SelectedValue == "1000000")
                {
                    ShowGrid.JSProperties["cpUnit"] = "Ten Lakhs";
                }
                else if (ddlunits.SelectedValue == "10000000")
                {
                    ShowGrid.JSProperties["cpUnit"] = "Crores";
                }
                else if (ddlunits.SelectedValue == "100000000")
                {
                    ShowGrid.JSProperties["cpUnit"] = "Ten Crores";
                }
            }
            catch (Exception ex)
            {
            }
        }
       
        #endregion

        #region Branch Populate

        protected void Componentbranch_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string FinYear = Convert.ToString(Session["LastFinYear"]);

            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                DataTable ComponentTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];
                if (Hoid != "All" && Hoid != "null")
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


          //  SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);MULTI
            
            
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

        protected void ddlExport2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(ddlExport2.SelectedItem.Value));
            if (Filter != 0)
            {
                BranchHoOffice();
                bindexport_Details2(Filter);
            }
        }

        protected void ShowGridDetails2Level_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }

        public void bindexport_Details2(int Filter)
        {
            DateTime dtAsOn;
            dtAsOn = Convert.ToDateTime(ASPxAsOnDate.Date);
            string HASONDATE = dtAsOn.ToString("dd MMMM , yyyy");

            ShowGridDetails2Level.Columns[8].Visible = false;
            ShowGridDetails2Level.Columns[9].Visible = false;

            string filename = "Consolidated Balance Sheet - Ledger Detailed";
            exporterDetails.FileName = filename;
            exporterDetails.FileName = "Consolidated Balance Sheet - Ledger Detailed";

            string FileHeader = "";
            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();
            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Consolidated Balance Sheet - Ledger Detailed" + Environment.NewLine + "As at " + HASONDATE;

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
                    exporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 2:
                    exporter.WritePdfToResponse();
                    break;
                case 3:
                    exporter.WriteCsvToResponse();
                    break;
                case 4:
                    exporter.WriteRtfToResponse();
                    break;

                default:
                    return;
            }
        }

        #endregion

        protected void CallbackPanelDetailL1_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string group;
            string groupdesc;
            string[] CallVal2ndlevel = e.Parameter.ToString().Split('~');
            group = CallVal2ndlevel[0];
            Session["GroupId"] = group;
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            DataTable dtgroupdesc = null;
            groupdesc = "";

            string IsBSScheduleVIL1Filter = Convert.ToString(hfIsBSScheduleVIL1Filter.Value);
            Session["IsBSScheduleVIL1Filter"] = IsBSScheduleVIL1Filter;

            if (Convert.ToString(Session["IsBSScheduleVIL1Filter"]) == "Y")
            {
                if (group != "null" && group != "0" && group != "")
                {
                    dtgroupdesc = oDBEngine.GetDataTable("Select LTRIM(RTRIM(NAME)) AS NAME from BALANCESHEETSCHEDULEVI_REPORT Where SCHEDULE_ID='" + group + "' AND USERID='" + Userid + "'");
                }
                groupdesc = dtgroupdesc.Rows[0][0].ToString();
            }

            if (!string.IsNullOrEmpty(group) && group != "0")
            {
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

                DateTime dtAsOnDate;
                dtAsOnDate = Convert.ToDateTime(ASPxAsOnDate.Date);

                string ASONDATE = dtAsOnDate.ToString("yyyy-MM-dd");

                string BRANCH_ID = "";

                string Branch = "";
                List<object> BranchList = lookup_branch.GridView.GetSelectedFieldValues("ID");
                foreach (object B in BranchList)
                {
                    Branch += "," + B;
                }
                BRANCH_ID = Branch.TrimStart(',');

                string branchid = Convert.ToString(e.Parameter.Split('~')[1]);
                DataTable dt = new DataTable();
                dt = GetBalanceSheetScheduleVIL1(ASONDATE, group,BRANCH_ID, branchid);

                CallbackPanelDetailL1.JSProperties["cpGroupDesc"] = Convert.ToString(groupdesc);
                CallbackPanelDetailL1.JSProperties["cpdtAsOnDate"] = dtAsOnDate.ToString("dd-MM-yyyy");
            }
            else
            {
                CallbackPanelDetailL1.JSProperties["cpBlankLedger"] = "0";
            }
        }

        private DataTable GetBalanceSheetScheduleVIL1(string AsOnDate, string group, string BRANCH_ID, string HeadBranch)
        {

            try
            {
                DataSet ds = new DataSet();


               // SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));


                SqlCommand cmd = new SqlCommand("PRC_BALANCESHEETSCHEDULEVIL1_REPORT", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@ASONDATE", AsOnDate);
                cmd.Parameters.AddWithValue("@BRANCH_ID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@UNITS", ddlunits.SelectedValue);
                cmd.Parameters.AddWithValue("@HO", HeadBranch);
                cmd.Parameters.AddWithValue("@SCHEDULE_ID", group);
                cmd.Parameters.AddWithValue("@USERID", Convert.ToString(Session["userid"]).Trim());
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);

                cmd.Dispose();
                con.Dispose();

                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        protected void Showgrid_DataBound(object sender, EventArgs e)
        {
            DateTime dtAsOn;
            dtAsOn = Convert.ToDateTime(ASPxAsOnDate.Date);
            string ASONDATE = dtAsOn.ToString("dd-MM-yyyy");
            //string HASONDATE = dtAsOn.ToString("MMMM dd, yyyy");
            string HASONDATE = dtAsOn.ToString("dd MMMM , yyyy");
            DateTime dtPAsOn;
            dtPAsOn = Convert.ToDateTime(ASPxAsOnDate.Date.AddYears(-1));
            string PREVIOUSASONDATE = dtPAsOn.ToString("dd-MM-yyyy");
            
            ASPxGridView grid = (ASPxGridView)sender;
            foreach (GridViewDataColumn c in grid.Columns)
            {
                if ((c.FieldName.ToString()).StartsWith("SLSPACE"))
                {
                    c.Visible = true;
                }
                if ((c.FieldName.ToString()).StartsWith("NAME"))
                {
                    c.Visible = true;
                    c.Caption = "Particulars";
                    
                }
                if ((c.FieldName.ToString()).StartsWith("SCHEDULE_NUMBER"))
                {
                    c.Visible = true;
                    c.Caption = "Schedule #";
                }
                if ((c.FieldName.ToString()).StartsWith("OPENING1"))
                {
                    c.Visible = true;
                    c.Caption = "As at <br/> " + ASONDATE;                    
                }
                if ((c.FieldName.ToString()).StartsWith("OPENING2"))
                {
                    c.Visible = true;
                    c.Caption = "As at <br/> " + PREVIOUSASONDATE;
                }
            }

            if (ddlunits.SelectedValue == "1")
            {
                grid.JSProperties["cpUnit"] = "Ones";
            }
            else if (ddlunits.SelectedValue == "10")
            {
                grid.JSProperties["cpUnit"] = "Tens";
            }
            else if (ddlunits.SelectedValue == "100")
            {
                grid.JSProperties["cpUnit"] = "Hundreds";
            }
            else if (ddlunits.SelectedValue == "1000")
            {
                grid.JSProperties["cpUnit"] = "Thousands";
            }
            else if (ddlunits.SelectedValue == "10000")
            {
                grid.JSProperties["cpUnit"] = "Ten Thousands";
            }
            else if (ddlunits.SelectedValue == "100000")
            {
                grid.JSProperties["cpUnit"] = "Lakhs";
            }
            else if (ddlunits.SelectedValue == "1000000")
            {
                grid.JSProperties["cpUnit"] = "Ten Lakhs";
            }
            else if (ddlunits.SelectedValue == "10000000")
            {
                grid.JSProperties["cpUnit"] = "Crores";
            }
            else if (ddlunits.SelectedValue == "100000000")
            {
                grid.JSProperties["cpUnit"] = "Ten Crores";
            }
            
        }

        protected void ShowGridDetails2Level_DataBound(object sender, EventArgs e)
        {
            ASPxGridView grid2 = (ASPxGridView)sender;
            if (ddlunits.SelectedValue == "1")
            {
                grid2.JSProperties["cpUnit2"] = "Ones";
            }
            else if (ddlunits.SelectedValue == "10")
            {
                grid2.JSProperties["cpUnit2"] = "Tens";
            }
            else if (ddlunits.SelectedValue == "100")
            {
                grid2.JSProperties["cpUnit2"] = "Hundreds";
            }
            else if (ddlunits.SelectedValue == "1000")
            {
                grid2.JSProperties["cpUnit2"] = "Thousands";
            }
            else if (ddlunits.SelectedValue == "10000")
            {
                grid2.JSProperties["cpUnit2"] = "Ten Thousands";
            }
            else if (ddlunits.SelectedValue == "100000")
            {
                grid2.JSProperties["cpUnit2"] = "Lakhs";
            }
            else if (ddlunits.SelectedValue == "1000000")
            {
                grid2.JSProperties["cpUnit2"] = "Ten Lakhs";
            }
            else if (ddlunits.SelectedValue == "10000000")
            {
                grid2.JSProperties["cpUnit2"] = "Crores";
            }
            else if (ddlunits.SelectedValue == "100000000")
            {
                grid2.JSProperties["cpUnit2"] = "Ten Crores";
            }
        }

        protected void ShowGridDetails3Level_DataBound(object sender, EventArgs e)
        {
            ASPxGridView grid3 = (ASPxGridView)sender;
            if (ddlunits.SelectedValue == "1")
            {
                grid3.JSProperties["cpUnit3"] = "Ones";
            }
            else if (ddlunits.SelectedValue == "10")
            {
                grid3.JSProperties["cpUnit3"] = "Tens";
            }
            else if (ddlunits.SelectedValue == "100")
            {
                grid3.JSProperties["cpUnit3"] = "Hundreds";
            }
            else if (ddlunits.SelectedValue == "1000")
            {
                grid3.JSProperties["cpUnit3"] = "Thousands";
            }
            else if (ddlunits.SelectedValue == "10000")
            {
                grid3.JSProperties["cpUnit3"] = "Ten Thousands";
            }
            else if (ddlunits.SelectedValue == "100000")
            {
                grid3.JSProperties["cpUnit3"] = "Lakhs";
            }
            else if (ddlunits.SelectedValue == "1000000")
            {
                grid3.JSProperties["cpUnit3"] = "Ten Lakhs";
            }
            else if (ddlunits.SelectedValue == "10000000")
            {
                grid3.JSProperties["cpUnit3"] = "Crores";
            }
            else if (ddlunits.SelectedValue == "100000000")
            {
                grid3.JSProperties["cpUnit3"] = "Ten Crores";
            }
        }

        protected void ShowGrid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            string Space1 = Convert.ToString(e.GetValue("SLSPACE"));
            if(Space1.Contains("~SPACE1"))
            {
                //e.Row.Cells.Remove()
            }
        }
        protected void ShowGrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            string strcellvalue = Convert.ToString(e.CellValue);
            if (e.DataColumn.FieldName == "OPENING2" && blBold==false)
            {
                Session["chk_presenttotal"] = 0;
            }
            if (e.CellValue != null)
            {
               if (e.CellValue.ToString() == "Asset" || e.CellValue.ToString() == "Non-Current Assets" || e.CellValue.ToString() == "TOTAL NON-CURRENT ASSET" || e.CellValue.ToString() == "Current Assets" ||
               e.CellValue.ToString() == "TOTAL CRRENT ASSETS" || e.CellValue.ToString() == "TOTAL ASSETS" || e.CellValue.ToString() == "Equity and Liability" || e.CellValue.ToString() == "Equity" ||
               e.CellValue.ToString() == "TOTAL EQUITY" || e.CellValue.ToString() == "Liability" || e.CellValue.ToString() == "Non-Current Liability" || e.CellValue.ToString() == "TOTAL NON CURRENT LIABLITIES" ||
               e.CellValue.ToString() == "Current Liability" || e.CellValue.ToString() == "TOTAL CURRENT LIABLITIES" || e.CellValue.ToString() == "TOTAL EQUITY AND LIABILITY")
                {
                    e.Cell.Font.Bold = true;
                    e.Cell.CssClass = "makebold dxgv";
                    
                }

               if (e.CellValue.ToString() == "TOTAL NON-CURRENT ASSET" || e.CellValue.ToString() == "TOTAL CRRENT ASSETS" || e.CellValue.ToString() == "TOTAL ASSETS" || e.CellValue.ToString() == "TOTAL EQUITY" ||
                   e.CellValue.ToString() == "TOTAL NON CURRENT LIABLITIES" || e.CellValue.ToString() == "TOTAL CURRENT LIABLITIES" || e.CellValue.ToString() == "TOTAL EQUITY AND LIABILITY")
               {
                   Session["chk_presenttotal"] = 1;
                   blBold = true;
                   //e.Cell.BackColor = Color.DarkSeaGreen;
               }
               else
               {
                   blBold = false;
               }

               //if (Convert.ToInt32(Session["chk_presenttotal"]) == 1)
               //{
               //    e.Cell.Font.Bold = true;
               //    e.Cell.BackColor = Color.DarkSeaGreen;
               //}
               //if (e.DataColumn.FieldName == "OPENING2")
               //{
               //    Session["chk_presenttotal"] = 0;
               //}

               if ((e.DataColumn.FieldName == "OPENING1" || e.DataColumn.FieldName == "OPENING2") && (Convert.ToInt32(Session["chk_presenttotal"]) == 1))
               {
                   e.Cell.Font.Bold = true;
                   blBold = true;
                   //e.Cell.BackColor = Color.DarkSeaGreen;
               }
                else
               {
                   blBold = false;
               }

               if (e.CellValue.ToString().Contains("padding-left:52px"))
               {
                   e.Cell.Style.Add("padding-left", "52px");
               }
               else if (e.CellValue.ToString().Contains("padding-left:62px"))
               {
                   e.Cell.Style.Add("padding-left", "62px");
               }
               else if (e.CellValue.ToString().Contains("padding-left:72px"))
               {
                   e.Cell.Style.Add("padding-left", "72px");
               }
            }
        }

        protected void CallbackPanelDetailL2_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string ledger;
            string ledgerDesc;
            string branchdesc;
            string[] CallVal2ndlevel = e.Parameter.ToString().Split('~');
            ledger = CallVal2ndlevel[0];
            Session["LedgerId"] = ledger;

            DataTable dtledgdesc = null;
            ledgerDesc = "";

            string IsBSScheduleVIL2Filter = Convert.ToString(hfIsBSScheduleVIL2Filter.Value);
            Session["IsBSScheduleVIL2Filter"] = IsBSScheduleVIL2Filter;

            if (ledger != "null" && ledger != "0" && ledger != "")
            {
                dtledgdesc = oDBEngine.GetDataTable("Select MainAccount_Name from Master_MainAccount Where CONVERT(NVARCHAR(MAX),MainAccount_ReferenceID)='" + ledger + "'");
                ledgerDesc = dtledgdesc.Rows[0][0].ToString();
            }
            else
            {
                dtledgdesc = null;
                ledgerDesc = null;
            }

            string BRANCH_ID = Convert.ToString(e.Parameter.Split('~')[1]);
            DataTable dtbranchdesc = null;
            branchdesc = "";
            if (BRANCH_ID != "null" && BRANCH_ID != "0" && BRANCH_ID != "")
            {
                dtbranchdesc = oDBEngine.GetDataTable("Select branch_description from tbl_master_branch Where CONVERT(NVARCHAR(MAX),branch_id)='" + BRANCH_ID + "'");
                branchdesc = dtbranchdesc.Rows[0][0].ToString();
            }
            else
            {
                dtbranchdesc = null;
                branchdesc = null;
            }

            if (!string.IsNullOrEmpty(ledger) && ledger != "0")
            {
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

                DateTime dtAsOnDate;
                dtAsOnDate = Convert.ToDateTime(ASPxAsOnDate.Date);

                string ASONDATE = dtAsOnDate.ToString("yyyy-MM-dd");

                DataTable dt = new DataTable();
                dt = GetBalanceSheetScheduleVIL2(ASONDATE, ledger, BRANCH_ID);

                CallbackPanelDetailL2.JSProperties["cpLedger"] = Convert.ToString(ledgerDesc);
                CallbackPanelDetailL2.JSProperties["cpBranch"] = Convert.ToString(branchdesc);
                CallbackPanelDetailL2.JSProperties["cpAsOnDate"] = dtAsOnDate.ToString("dd-MM-yyyy");
                CallbackPanelDetailL2.JSProperties["cpBlankLedger"] = "1";
            }
            else
            {
                CallbackPanelDetailL2.JSProperties["cpBlankLedger"] = "0";
            }
        }

        private DataTable GetBalanceSheetScheduleVIL2(string AsOnDate, string ledger, string BRANCH_ID)
        {
            try
            {
                DataSet ds = new DataSet();

                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);  MULTI
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

                SqlCommand cmd = new SqlCommand("PRC_BALANCESHEETSCHEDULEVIL2_REPORT", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@ASONDATE", AsOnDate);
                cmd.Parameters.AddWithValue("@BRANCH_ID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@LEDGERID", ledger);
                cmd.Parameters.AddWithValue("@UNITS", ddlunits.SelectedValue);
                cmd.Parameters.AddWithValue("@USERID", Convert.ToString(Session["userid"]).Trim());
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);

                cmd.Dispose();
                con.Dispose();

                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        protected void ddlExport3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(ddlExport3.SelectedItem.Value));
            if (Filter != 0)
            {
                BranchHoOffice();
                bindexport_Details3(Filter);
            }
        }

        protected void ShowGridDetails3Level_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }

        public void bindexport_Details3(int Filter)
        {
            DateTime dtAsOn;
            dtAsOn = Convert.ToDateTime(ASPxAsOnDate.Date);
            string HASONDATE = dtAsOn.ToString("dd MMMM , yyyy");

            ShowGridDetails3Level.Columns[10].Visible = false;
            ShowGridDetails3Level.Columns[11].Visible = false;
            ShowGridDetails3Level.Columns[12].Visible = false;

            string filename = "Consolidated Balance Sheet - Document Detailed";
            exporterDetails.FileName = filename;
            exporterDetails.FileName = "Consolidated Balance Sheet - Document Detailed";

            string FileHeader = "";
            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();
            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Consolidated Balance Sheet - Document Detailed" + Environment.NewLine + "As at " + HASONDATE;

            exporterDetails.RenderBrick += exporter_RenderBrick;
            exporterDetails.PageHeader.Left = FileHeader;
            exporterDetails.PageHeader.Font.Size = 10;
            exporterDetails.PageHeader.Font.Name = "Tahoma";
            exporterDetails.PageFooter.Center = "[Page # of Pages #]";
            exporterDetails.PageFooter.Right = "[Date Printed]";
            exporterDetails.GridViewID = "ShowGridDetails3Level";
            switch (Filter)
            {
                case 1:
                    exporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 2:
                    exporter.WritePdfToResponse();
                    break;
                case 3:
                    exporter.WriteCsvToResponse();
                    break;
                case 4:
                    exporter.WriteRtfToResponse();
                    break;
                default:
                    return;
            }
        }

        #region Linq

        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SL";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString; MULTI
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsBSScheduleVIFilter"]) == "Y")
            {
                var q = from d in dc.BALANCESHEETSCHEDULEVI_REPORTs
                        where Convert.ToString(d.USERID) == Userid
                        orderby d.SL ascending
                        select d;
                e.QueryableSource = q;                
            }
            else
            {
                var q = from d in dc.BALANCESHEETSCHEDULEVI_REPORTs
                        where Convert.ToString(d.SL) == "0"
                        orderby d.SL ascending
                        select d;
                e.QueryableSource = q;
            }
            ShowGrid.ExpandAll();
        }

        protected void GenerateEntityServerModeDetailsL1DataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SL";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString; MULTI
           
            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsBSScheduleVIL1Filter"]) == "Y")
            {
                if (!string.IsNullOrEmpty(Session["GroupId"].ToString()) && Session["GroupId"].ToString() != "0")
                {
                    var q = from d in dc.BALANCESHEETSCHEDULEVIL1_REPORTs
                            where Convert.ToString(d.USERID) == Userid
                            orderby d.SL ascending
                            select d;
                    e.QueryableSource = q;
                }
                else
                {
                    var q = from d in dc.BALANCESHEETSCHEDULEVIL1_REPORTs
                            where Convert.ToString(d.SL) == "0"
                            orderby d.SL ascending
                            select d;
                    e.QueryableSource = q;
                }
            }
            else
            {
                var q = from d in dc.BALANCESHEETSCHEDULEVIL1_REPORTs
                        where Convert.ToString(d.SL) == "0"
                        orderby d.SL ascending
                        select d;
                e.QueryableSource = q;
            }
        }

        protected void GenerateEntityServerModeDetailsL2DataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";

            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString; MULTI

            string connectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);

            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsBSScheduleVIL2Filter"]) == "Y")
            {
                var q = from d in dc.BALANCESHEETSCHEDULEVIL2_REPORTs
                        where Convert.ToString(d.USERID) == Userid
                        orderby d.SEQ ascending
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.BALANCESHEETSCHEDULEVIL2_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ ascending
                        select d;
                e.QueryableSource = q;
            }
        }

        #endregion
    }
}