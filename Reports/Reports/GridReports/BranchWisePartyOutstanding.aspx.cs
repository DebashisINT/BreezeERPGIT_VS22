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
    public partial class BranchWisePartyOutstanding : System.Web.UI.Page
    {
        DataTable DTIndustry = new DataTable();
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        string data = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        decimal TotalOp_dr = 0;
        decimal TotalDebit = 0, TotalCredit = 0, TotalDBCR = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            //rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Reports/master/LedgerPostingReport.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            if (!IsPostBack)
            {
                DataTable dtBranchSelection = new DataTable();
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Branch Wise Party Outstanding";
                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, false,false, false, false, false);
                CompName.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, true, false, false, false, false);
                CompAdd.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, true, false, false, false);
                CompOth.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, false, true, false, false);
                CompPh.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, false, false, false, true);
                CompAccPrd.Text = GridHeader;

                Session["IsBranchWisePartyOutstanding"] = null;
                Session["ISBranchWisePartyOutstandingCustDet"] = null;
                
                Session["SI_ComponentData"] = null;
                Session["SI_ComponentData_Branch"] = null;
                //Session["dt_PartyOutstandingRpt"] = null;
                //Session["dt_PartyOutstandingDetRpt"] = null;
                Session["CustVendType"] = null;

                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;

                ASPxFromDate.Value = DateTime.Now;
                ASPxToDate.Value = DateTime.Now;
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                string TODATE = dtTo.ToString("yyyy-MM-dd");

                string BRANCH_ID = "";
                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }

                string GROUP_ID = "";     //Suvankar
                if (hdnSelectedGroups.Value != "")
                {
                    GROUP_ID = hdnSelectedGroups.Value;
                }
                //string CUSTVENDID = "";
                //if (hdnSelectedCustomerVendor.Value != "")
                //{
                //    CUSTVENDID = hdnSelectedCustomerVendor.Value;
                //}
                BranchHoOffice();
                //Rev Debashis
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                //End of Rev Debashis

                //Rev Subhra 20-12-2018   0017670
                Session["BranchNames"] = null;
                //End Rev

                dtBranchSelection = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='ReportsBranchSelection'");
                hdnBranchSelection.Value = dtBranchSelection.Rows[0][0].ToString();
            }
            else
            {
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);
            }
            //BindPartyOutstanding();
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
        protected void ddlExport2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(ddlExport2.SelectedItem.Value));
            if (Filter != 0)
            {
                if (Session["exportval"] == null)
                {
                    bindexport_Details2(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    bindexport_Details2(Filter);
                }
                ddlExport2.SelectedValue = "0";
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
            ShowGrid.Columns[9].Visible = false;
            ShowGrid.Columns[10].Visible = false;
            ShowGrid.Columns[11].Visible = false;

            string filename = "BranchWisePartyOutstanding";
            exporter.FileName = filename;
            string FileHeader = "";

            exporter.FileName = filename;

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true,true, true, true, true) + Environment.NewLine + "Branch Wise Party Outstanding Report" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
            //Rev Subhra 20-12-2018   0017670
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

        public void bindexport_Details2(int Filter)
        {
            string filename = "Branch Wise Party Outstanding Details Report";
            exporterDetails.FileName = filename;
            exporterDetails.FileName = "Branch Wise Party Outstanding Details Report";

            string FileHeader = "";
            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();
            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true,true, true, true, true) + Environment.NewLine + "Branch Wise Party Outstanding Details Report" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");

            exporterDetails.RenderBrick += exporter_RenderBrick;

            //exporterDetails.PageHeader.Left = "General Trial Details Report";
            exporterDetails.PageHeader.Left = FileHeader;
            exporterDetails.PageHeader.Font.Size = 10;
            exporterDetails.PageHeader.Font.Name = "Tahoma";
            //exporterDetails.PageFooter.Center = "[Page # of Pages #]";
            //exporterDetails.PageFooter.Right = "[Date Printed]";
            exporterDetails.GridViewID = "Custdetails";
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


        protected void exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            e.BrickStyle.BackColor = Color.White;
            e.BrickStyle.ForeColor = Color.Black;
        }

        //protected void ShowGrid_DataBinding(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (Session["dt_PartyOutstandingRpt"] != null)
        //        {
        //            ShowGrid.DataSource = (DataTable)Session["dt_PartyOutstandingRpt"];
        //            //ShowGrid.DataBind();
        //        }
        //    }
        //    catch { }
        //}

        //protected void Custdetails_DataBinding(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (Session["dt_PartyOutstandingDetRpt"] != null)
        //        {
        //            Custdetails.DataSource = (DataTable)Session["dt_PartyOutstandingDetRpt"];
        //            //ShowGrid.DataBind();
        //        }
        //    }
        //    catch { }
        //}
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

            string IsBranchWisePartyOutstanding = Convert.ToString(hfIsBranchWisePartyOutstanding.Value);
            Session["IsBranchWisePartyOutstanding"] = IsBranchWisePartyOutstanding;


            //Session.Remove("dt_PartyOutstandingRpt");
            ShowGrid.JSProperties["cpSave"] = null;

            string HeadBranch = Convert.ToString(e.Parameter.Split('~')[3]);

            string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

            DateTime dtFrom;
            DateTime dtTo;
            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);

            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");
            string BRANCH_ID = "";
            string Group_ID = "";
            if (lookup_Group.GridView.GetSelectedFieldValues("GroupCode").Count() != GetGroupList().Rows.Count)
            {
                string QuoComponent3 = "";
                List<object> QuoList3 = lookup_Group.GridView.GetSelectedFieldValues("GroupCode");
                foreach (object Quo3 in QuoList3)
                {
                    QuoComponent3 += "," + Quo3;
                }
                Group_ID = QuoComponent3.TrimStart(',');
            }

            string QuoComponent2 = "";
            List<object> QuoList2 = lookup_branch.GridView.GetSelectedFieldValues("ID");
            foreach (object Quo2 in QuoList2)
            {
                QuoComponent2 += "," + Quo2;
            }
            BRANCH_ID = QuoComponent2.TrimStart(',');
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
            CallbackPanel.JSProperties["cpBranchNames"] = Convert.ToString(Session["BranchNames"]);
            //End of Rev

            Task PopulateStockTrialDataTask = new Task(() => GetBranchWisePartyOutstandingdata(FROMDATE, TODATE, BRANCH_ID, Group_ID, HeadBranch));
            PopulateStockTrialDataTask.RunSynchronously();
        }

        public DataTable GetGroupList()
        {
            try
            {
                DataTable dt = oDBEngine.GetDataTable("select GroupCode, GroupDescription from ( select AccountGroup_ReferenceID as GroupCode, AccountGroup_Name + ' ('+AccountGroup_Type + ')' as GroupDescription from Master_AccountGroup where AccountGroup_Type = 'Liability' or AccountGroup_Type = 'Asset' ) AcGrp order by GroupDescription");
                return dt;
            }
            catch
            {
                return null;
            }
        }

        public void GetBranchWisePartyOutstandingdata(string FROMDATE, string TODATE, string BRANCH_ID, string GROUP_ID, string HeadBranch)
        {
            try
            {             
                DataSet ds = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_BranchwisePartyOutstanding_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]).Trim());
                cmd.Parameters.AddWithValue("@CUSTOMER_TYPE", drp_partytype.SelectedValue);
                cmd.Parameters.AddWithValue("@BALANCE_SORT", drp_balancetype.SelectedValue);
                cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                cmd.Parameters.AddWithValue("@TODATE", TODATE);
                cmd.Parameters.AddWithValue("@BRANCHID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@Group_Code", GROUP_ID);
                cmd.Parameters.AddWithValue("@P_INVOICE_DATE", chkparty.Checked == true ? "1" : "0");
                cmd.Parameters.AddWithValue("@HO", HeadBranch);
                cmd.Parameters.AddWithValue("@USERID", Convert.ToInt32(Session["userid"]));

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);

                cmd.Dispose();
                con.Dispose();

                //Session["dt_PartyOutstandingRpt"] = ds.Tables[0];

                ShowGrid.DataSource = ds.Tables[0];
                ShowGrid.DataBind();
            }
            catch (Exception ex)
            {
            }
        }

        [WebMethod]
        public static List<string> GetBranchesList(String NoteId)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
            StringBuilder filter = new StringBuilder();
            StringBuilder Supervisorfilter = new StringBuilder();
            BusinessLogicLayer.Others objbl = new BusinessLogicLayer.Others();
            DataTable dtbl = new DataTable();
            if (NoteId.Trim() == "")
            {
                dtbl = oDBEngine.GetDataTable("select branch_id,branch_description from tbl_master_branch where branch_id in(" + Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) + ")  order by branch_description asc");

            }

            List<string> obj = new List<string>();

            foreach (DataRow dr in dtbl.Rows)
            {

                obj.Add(Convert.ToString(dr["branch_description"]) + "|" + Convert.ToString(dr["branch_id"]));
            }
            return obj;
        }

        public static List<string> GetGroupList(String NoteId)   //Suvankar
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
            StringBuilder filter = new StringBuilder();
            StringBuilder Supervisorfilter = new StringBuilder();
            BusinessLogicLayer.Others objbl = new BusinessLogicLayer.Others();
            DataTable dtbl = new DataTable();
            if (NoteId.Trim() == "")
            {
                dtbl = oDBEngine.GetDataTable("select GroupCode, GroupDescription from ( select AccountGroup_ReferenceID as GroupCode, AccountGroup_Name + ' ('+AccountGroup_Type + ')' as GroupDescription from Master_AccountGroup where AccountGroup_Type = 'Liability' or AccountGroup_Type = 'Asset' ) AcGrp order by GroupDescription");

            }

            List<string> obj = new List<string>();

            foreach (DataRow dr in dtbl.Rows)
            {

                obj.Add(Convert.ToString(dr["GroupDescription"]) + "|" + Convert.ToString(dr["GroupCode"]));
            }
            return obj;
        }

        public void Date_finyearwise(string Finyear)
        {
            //Rev Debashis
            //CommonBL bll1 = new CommonBL();
            //DataTable stbill = new DataTable();
            //stbill = bll1.GetDateFinancila(Finyear);
            //if (stbill.Rows.Count > 0)
            //{
            //    ASPxFromDate.Text = Convert.ToDateTime(DateTime.Now).ToString("dd-MM-yyyy");
            //    ASPxToDate.Text = Convert.ToDateTime(DateTime.Now).ToString("dd-MM-yyyy");
            //}
            CommonBL partyout = new CommonBL();
            DataTable dtpartyout = new DataTable();

            dtpartyout = partyout.GetDateFinancila(Finyear);
            if (dtpartyout.Rows.Count > 0)
            {
                ASPxFromDate.MaxDate = Convert.ToDateTime((dtpartyout.Rows[0]["FinYear_EndDate"]));
                ASPxFromDate.MinDate = Convert.ToDateTime((dtpartyout.Rows[0]["FinYear_StartDate"]));

                ASPxToDate.MaxDate = Convert.ToDateTime((dtpartyout.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((dtpartyout.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((dtpartyout.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((dtpartyout.Rows[0]["FinYear_StartDate"]));

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

        protected void Showgrid_Htmlprepared(object sender, EventArgs e)
        {
            ASPxGridView grid = (ASPxGridView)sender;
            if (drp_partytype.SelectedValue == "ALL")
            {
                grid.Columns["CustomerVendor"].Caption = "Customer/Vendor";

            }

            else if (drp_partytype.SelectedValue == "CL")
            {
                grid.Columns["CustomerVendor"].Caption = "Customer";

            }

            else if (drp_partytype.SelectedValue == "DV")
            {
                grid.Columns["CustomerVendor"].Caption = "Vendor";

            }

      
        }

        protected void ComponentGroup_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)  //Suvankar
        {
            string FinYear = Convert.ToString(Session["LastFinYear"]);

            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                DataTable ComponentTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];
                ComponentTable = oDBEngine.GetDataTable("select GroupCode, GroupDescription from (select AccountGroup_ReferenceID as GroupCode, AccountGroup_Name + ' ('+AccountGroup_Type + ')' as GroupDescription from Master_AccountGroup where AccountGroup_Type = 'Liability' or AccountGroup_Type = 'Asset' ) AcGrp order by GroupDescription");
                if (ComponentTable.Rows.Count > 0)
                {
                    Session["SI_ComponentData_Group"] = ComponentTable;
                    lookup_Group.DataSource = ComponentTable;
                    lookup_Group.DataBind();
                }
                else
                {
                    lookup_Group.DataSource = null;
                    lookup_Group.DataBind();

                }
            }
        }

        protected void lookup_Group_DataBinding(object sender, EventArgs e) //Suvankar
        {
            if (Session["SI_ComponentData_Group"] != null)
            {
                lookup_Group.DataSource = (DataTable)Session["SI_ComponentData_Group"];
            }
        }

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
                    //Session["SI_ComponentData_Branch"] = null;
                    //lookup_branch.DataSource = null;
                    //lookup_branch.DataBind();
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
            return dt;
            cmd.Dispose();
            con.Dispose();
        }
           
        protected void lookup_branch_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_ComponentData_Branch"] != null)
            {
                lookup_branch.DataSource = (DataTable)Session["SI_ComponentData_Branch"];
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

        # region customer/vendor details part
        protected void Custdetails_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
        {
            if (e.Item == Custdetails.TotalSummary["Closing_Balance"])
            {
                if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
                {
                    Decimal gmv = Convert.ToDecimal(Custdetails.GetTotalSummaryValue(Custdetails.TotalSummary["DEBIT"]));
                    Decimal equity = Convert.ToDecimal(Custdetails.GetTotalSummaryValue(Custdetails.TotalSummary["CREDIT"]));

                    e.TotalValue = gmv - equity;
                    if (Convert.ToDecimal(e.TotalValue) != 0)
                    {
                        if (Convert.ToDecimal(e.TotalValue) < 0)
                        {
                            e.TotalValue = System.Math.Abs(Convert.ToDecimal(e.TotalValue)) + "Cr";
                        }
                        else
                        {
                            e.TotalValue = e.TotalValue + "Dr";
                        }
                    }
                    e.TotalValueReady = true;
                }
            }

        }

        protected void Custdetails_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            //e.Text = string.Format("{0}", e.Value);

            if (e.Item.FieldName == "DEBIT")
            {
                TotalDebit = Convert.ToDecimal(e.Value);
            }
            else if (e.Item.FieldName == "CREDIT")
            {
                TotalCredit = Convert.ToDecimal(e.Value);
            }
            if (e.Item.FieldName == "Closing_Balance")
            {
                e.Text = Convert.ToString(Math.Abs((TotalDebit - TotalCredit)));
            }
            else
            {
                e.Text = string.Format("{0}", Math.Abs(Convert.ToDecimal(e.Value)));
            }
            
            
            if (e.Item.FieldName == "Closebal_DBCR")
            {
                if ((TotalDebit - TotalCredit) > 0)
                {
                    e.Text = "Dr";
                }
                else if ((TotalDebit - TotalCredit) < 0)
                {
                    e.Text = "Cr";
                }
                 else if ((TotalDebit - TotalCredit) == 0)
                {
                    e.Text = "";
                }
            }

         


        }
        protected void Custdetails_Htmlprepared(object sender, EventArgs e)
        {
            ASPxGridView grid = (ASPxGridView)sender;
            string custvendtype = Convert.ToString(Session["CustVendType"]);
            if (custvendtype == "ALL"){
                grid.Columns["CustomerVendor"].Caption = "Customer/Vendor";
            }
            else if (custvendtype == "CL"){
                grid.Columns["CustomerVendor"].Caption = "Customer";
            }
            else if (custvendtype == "DV") { 
                grid.Columns["CustomerVendor"].Caption = "Vendor";
            }
        }

        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
           e.Text = string.Format("{0}", e.Value);
        }

        protected void CallbackPaneCustDet_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

            string IsBranchWisePartyOutstandingCustDet = Convert.ToString(hfIsBranchWisePartyOutstandingCustDet.Value);
            Session["IsBranchWisePartyOutstandingCustDet"] = IsBranchWisePartyOutstandingCustDet;

            //Session.Remove("dt_PartyOutstandingDetRpt");

            string[] CallVal = e.Parameter.ToString().Split('~');

            string BRANCHDESC = Convert.ToString(CallVal[0]);
            string CUSTVENDDESC = Convert.ToString(CallVal[1]);
            string BRANCHID = Convert.ToString(CallVal[2]);
            string CUSTVENDCODE = Convert.ToString(CallVal[3]);
            string CUSTVENDTYPE = Convert.ToString(CallVal[4]);

            
         
            ShowGrid.JSProperties["cpSave"] = null;
            string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

            DateTime dtFrom;
            DateTime dtTo;
            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);

            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");

            string BRANCH_ID = "";
           
            string Group_ID = ""; 
            if (lookup_Group.GridView.GetSelectedFieldValues("GroupCode").Count() != GetGroupList().Rows.Count)
            {
                string QuoComponent3 = "";
                List<object> QuoList3 = lookup_Group.GridView.GetSelectedFieldValues("GroupCode");
                foreach (object Quo3 in QuoList3)
                {
                    QuoComponent3 += "," + Quo3;
                }
                Group_ID = QuoComponent3.TrimStart(',');
            }

            string QuoComponent2 = "";
            List<object> QuoList2 = lookup_branch.GridView.GetSelectedFieldValues("ID");
            foreach (object Quo2 in QuoList2)
            {
                QuoComponent2 += "," + Quo2;
            }
            BRANCH_ID = QuoComponent2.TrimStart(',');
            string CUSTVENDID = "";
            string QuoComponent = "";

            Task PopulateStockTrialDataTask = new Task(() => GetCustomerDetailsdata(FROMDATE, TODATE, BRANCHDESC, CUSTVENDDESC, BRANCHID, CUSTVENDCODE, CUSTVENDTYPE));
            PopulateStockTrialDataTask.RunSynchronously();

        }

        public void GetCustomerDetailsdata(string FROMDATE, string TODATE, string BRANCHDESC,string CUSTVENDDESC,string BRANCHID,string CUSTVENDCODE,string CUSTVENDTYPE)
        {
            try
            {
                DataSet ds = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prc_BranchwisePartyOutstandingDetails_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]).Trim());
                cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                cmd.Parameters.AddWithValue("@TODATE", TODATE);
                cmd.Parameters.AddWithValue("@BRANCHID", BRANCHID);
                cmd.Parameters.AddWithValue("@P_CODE", CUSTVENDCODE);
                cmd.Parameters.AddWithValue("@Customer_Type", CUSTVENDTYPE);
                cmd.Parameters.AddWithValue("@Group_Code", "");
                cmd.Parameters.AddWithValue("@Partydate", chkparty.Checked == true ? "1" : "0");
                cmd.Parameters.AddWithValue("@USERID", Convert.ToInt32(Session["userid"]));

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);

                cmd.Dispose();
                con.Dispose();

                //Session["dt_PartyOutstandingDetRpt"] = ds.Tables[0];


                CallbackPaneCustDet.JSProperties["cpBranchDesc"] = BRANCHDESC;
                CallbackPaneCustDet.JSProperties["cpCustVendDesc"] = CUSTVENDDESC;
                CallbackPaneCustDet.JSProperties["cpFromDate"] = FROMDATE;
                CallbackPaneCustDet.JSProperties["cpToDate"] = TODATE;
                CallbackPaneCustDet.JSProperties["cpCustVendType"] = CUSTVENDTYPE;
                Session["CustVendType"] = CUSTVENDTYPE;

                

                //Custdetails.DataSource = ds.Tables[0];
                //Custdetails.DataBind();

            }
            catch (Exception ex)
            {
            }
        }

      

        # endregion

        #region LinQ
        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SLNO";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsBranchWisePartyOutstanding"]) == "Y")
            {
                var q = from d in dc.BRANCHWISEPARTYOUTSTANDINGSUM_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.CUSTOMER_TYPE) == drp_partytype.SelectedValue
                        orderby d.Data_Type, d.CODE
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.BRANCHWISEPARTYOUTSTANDINGSUM_REPORTs
                        where Convert.ToString(d.SLNO) == "0"
                        orderby d.SLNO
                        select d;
                e.QueryableSource = q;
            }
            ShowGrid.ExpandAll();
        }

        protected void GenerateEntityServerModeCustDetailsDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SLNO";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["ISBranchWisePartyOutstandingCustDet"]) == "Y")
            {
                var q = from d in dc.BRANCHWISEPARTYOUTSTANDINGDET_REPORTs
                        //where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.CUSTOMER_TYPE) == drp_partytype.SelectedValue
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.CUSTOMER_TYPE) == Convert.ToString(Session["CustVendType"])
                        orderby d.SLNO
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.BRANCHWISEPARTYOUTSTANDINGDET_REPORTs
                        where Convert.ToString(d.SLNO) == "0"
                        orderby d.SLNO
                        select d;
                e.QueryableSource = q;
            }
            ShowGrid.ExpandAll();
        }

        #endregion
    }
}