#region======================================Revision History=========================================================================
//1.0   V2.0.35     Debashis    06/02/2023      Enhancement required in Trial balance(Group wise) Report.
//                                              Refer: 0025608
#endregion===================================End of Revision History==================================================================
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
    public partial class TrialBalanceGroupWise : System.Web.UI.Page
    {
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        DataTable dtLedgerTotal = null;
        string LedgerTotalOpDr = "";
        string LedgerTotalOpCr = "";
        string LedgerTotalPrDr = "";
        string LedgerTotalPrCr = "";
        string LedgerTotalClDr = "";
        string LedgerTotalClCr = "";
        string LedgerTotalBalDesc = "";
        //Rev 1.0 Mantis: 0025608
        ExcelFile objExcel = new ExcelFile();
        DataTable CompanyInfo = new DataTable();
        DataTable dtExport = new DataTable();
        DataTable dtReportHeader = new DataTable();
        DataTable dtReportFooter = new DataTable();
        //End of Rev 1.0 Mantis: 0025608

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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/TrialBalanceGroupWise.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            if (!IsPostBack)
            {
                Session["chk_presenttotal"] = 0;
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Trial Balance (Group wise)";
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
                Session["SI_ComponentData_Branch"] = null;
                Session["IsTrialBalanceGrpWiseFilter"] = null;
                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                ASPxFromDate.Text = dtFrom.ToString("dd-MM-yyyy");
                ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");

                lookupGroup.DataBind();

                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                Session["BranchNames"] = null;
                radAsDate.Attributes.Add("OnClick", "DateAll('all')");
                radPeriod.Attributes.Add("OnClick", "DateAll('Selc')");
            }
            else
            {
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);
            }

            if (!IsPostBack)
            {
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

        public void Date_finyearwise(string Finyear)
        {
            CommonBL bll1 = new CommonBL();
            DataTable stbill = new DataTable();

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

        #region Export
        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            //Rev 1.0 Mantis: 0025608
            //if (Filter != 0)
            //{
            //    if (Session["exportval"] == null)
            //    {
            //        bindexport(Filter);
            //    }
            //    else if (Convert.ToInt32(Session["exportval"]) != Filter)
            //    {
            //        bindexport(Filter);
            //    }
            //    drdExport.SelectedValue = "0";
            //}
            if (Convert.ToString(Session["IsTrialBalanceGrpWiseFilter"]) == "Y")
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
            //End of Rev 1.0 Mantis: 0025608
        }

        //Rev 1.0 Mantis: 0025608
        //public void BindDropDownList()
        //{
        //    // Declare a Dictionary to hold all the Options with Value and Text.
        //    Dictionary<string, string> options = new Dictionary<string, string>();
        //    options.Add("0", "Export to");
        //    options.Add("1", "XLSX");
        //    options.Add("2", "PDF");
        //    options.Add("3", "CSV");
        //    options.Add("4", "RTF");

        //    // Bind the Dictionary to the DropDownList.
        //    drdExport.DataSource = options;
        //    drdExport.DataTextField = "value";
        //    drdExport.DataValueField = "key";
        //    drdExport.DataBind();
        //    drdExport.SelectedValue = "0";
        //}

        //protected void exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        //{
        //    e.BrickStyle.BackColor = Color.White;
        //    e.BrickStyle.ForeColor = Color.Black;
        //}

        //public void bindexport(int Filter)
        //{
        //    string filename = "Trial Balance (Group wise)";
        //    exporter.FileName = filename;
        //    string FileHeader = "";
        //    BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

        //    if (radAsDate.Checked == true)
        //    {
        //        FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Trial Balance (Group wise)" + Environment.NewLine + "As on " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy");
        //        FileHeader = ReplaceFirst(FileHeader, "\r\n", Convert.ToString(Session["BranchNames"]));
        //    }
        //    else
        //    {
        //        FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Trial Balance (Group wise)" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
        //        FileHeader = ReplaceFirst(FileHeader, "\r\n", Convert.ToString(Session["BranchNames"]));
        //    }

        //    exporter.RenderBrick += exporter_RenderBrick;
        //    exporter.PageHeader.Left = FileHeader;
        //    exporter.PageHeader.Font.Size = 10;
        //    exporter.PageHeader.Font.Name = "Tahoma";
        //    exporter.Landscape = true;
        //    exporter.MaxColumnWidth = 100;
        //    exporter.GridViewID = "ShowGrid";
        //    switch (Filter)
        //    {
        //        case 1:                    
        //            exporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
        //            break;
        //        case 2:
        //            exporter.WritePdfToResponse();
        //            break;
        //        case 3:
        //            exporter.WriteCsvToResponse();
        //            break;
        //        case 4:
        //            exporter.WriteRtfToResponse();
        //            break;
        //    }
        //}

        //public string ReplaceFirst(string text, string search, string replace)
        //{
        //    int pos = text.IndexOf(search);
        //    if (pos < 0)
        //    {
        //        return text;
        //    }
        //    return text.Substring(0, pos) + Environment.NewLine + replace + Environment.NewLine + text.Substring(pos + search.Length);
        //}

        public void bindexport(int Filter)
        {
            string filename = "Trial_Balance_Group_Wise";
            exporter.FileName = filename;

            if (Filter == 1 || Filter == 2)
            {
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                con.Open();
                string selectQuery = "SELECT LEDGER,OP_DR,OP_CR,PR_DR,PR_CR,CLOSE_DR,CLOSE_CR FROM TRIALBALANCEGROUPWISE_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND SL<>7 AND PARENTGRPID<>9999999999999 AND LEDGER<>'NET TOTAL:' order by SEQ";
                SqlDataAdapter myCommand = new SqlDataAdapter(selectQuery, con);

                // Create and fill a DataSet.
                DataSet ds = new DataSet();
                myCommand.Fill(ds, "Main");
                myCommand = new SqlDataAdapter("Select LEDGER,OP_DR,OP_CR,PR_DR,PR_CR,CLOSE_DR,CLOSE_CR FROM TRIALBALANCEGROUPWISE_REPORT Where USERID=" + Convert.ToInt32(Session["userid"]) + " AND SL=7 AND PARENTGRPID=9999999999999 AND LEDGER='NET TOTAL:'", con);
                myCommand.Fill(ds, "GrossTotal");
                myCommand.Dispose();
                con.Dispose();
                Session["exporttbgwdataset"] = ds;

                dtExport = ds.Tables[0].Copy();
                dtExport.Clear();
                dtExport.Columns.Add(new DataColumn("Group/Ledger/Sub Ledger", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Dr.(Opening)", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Cr.(Opening)", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Dr.(Period)", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Cr.(Period)", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Dr.(Closing)", typeof(decimal)));
                dtExport.Columns.Add(new DataColumn("Cr.(Closing)", typeof(decimal)));

                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {
                    DataRow row2 = dtExport.NewRow();

                    row2["Group/Ledger/Sub Ledger"] = dr1["LEDGER"];
                    row2["Dr.(Opening)"] = dr1["OP_DR"];
                    row2["Cr.(Opening)"] = dr1["OP_CR"];
                    row2["Dr.(Period)"] = dr1["PR_DR"];
                    row2["Cr.(Period)"] = dr1["PR_CR"];
                    row2["Dr.(Closing)"] = dr1["CLOSE_DR"];
                    row2["Cr.(Closing)"] = dr1["CLOSE_CR"];

                    dtExport.Rows.Add(row2);
                }

                if (Convert.ToString(Session["Isasondate"]) == "Y")
                {
                    dtExport.Columns.Remove("Dr.(Opening)");
                    dtExport.Columns.Remove("Cr.(Opening)");
                }

                dtExport.Columns.Remove("LEDGER");
                dtExport.Columns.Remove("OP_DR");
                dtExport.Columns.Remove("OP_CR");
                dtExport.Columns.Remove("PR_DR");
                dtExport.Columns.Remove("PR_CR");
                dtExport.Columns.Remove("CLOSE_DR");
                dtExport.Columns.Remove("CLOSE_CR");

                DataRow row3 = dtExport.NewRow();
                row3["Group/Ledger/Sub Ledger"] = ds.Tables[1].Rows[0]["LEDGER"].ToString();
                if (Convert.ToString(Session["Isasondate"]) == "N")
                {
                    row3["Dr.(Opening)"] = ds.Tables[1].Rows[0]["OP_DR"].ToString();
                    row3["Cr.(Opening)"] = ds.Tables[1].Rows[0]["OP_CR"].ToString();
                }
                row3["Dr.(Period)"] = ds.Tables[1].Rows[0]["PR_DR"].ToString();
                row3["Cr.(Period)"] = ds.Tables[1].Rows[0]["PR_CR"].ToString();
                row3["Dr.(Closing)"] = ds.Tables[1].Rows[0]["CLOSE_DR"].ToString();
                row3["Cr.(Closing)"] = ds.Tables[1].Rows[0]["CLOSE_CR"].ToString();
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
                HeaderRow6[0] = "Trial Balance (Group wise)";
                dtReportHeader.Rows.Add(HeaderRow6);
                DataRow HeaderRow7 = dtReportHeader.NewRow();
                if (Convert.ToString(Session["Isasondate"]) == "Y")
                {
                    HeaderRow7[0] = "As On: " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
                }
                else
                {
                    HeaderRow7[0] = "For the period: " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + "To "+Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
                }
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
                    objExcel.ExportToExcelforExcel(dtExport, "Trial_Balance_Group_Wise", "Total:", "NET TOTAL:", dtReportHeader, dtReportFooter);
                    break;
                case 2:
                    objExcel.ExportToPDF(dtExport, "Trial_Balance_Group_Wise", "Total:", "NET TOTAL:", dtReportHeader, dtReportFooter);
                    break;
                case 3:
                    exporter.WriteCsvToResponse();
                    break;

                default:
                    return;
            }
        }
        //End of Rev 1.0 Mantis: 0025608
        #endregion

        #region main grid details
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string branchid = Convert.ToString(e.Parameter.Split('~')[2]);
            bool is_asondate = false;
            string[] CallVal = e.Parameter.ToString().Split('~');
            is_asondate = Convert.ToBoolean(CallVal[1]);

            string IsTrialBalanceGrpWiseFilter = Convert.ToString(hfIsTrialBalanceGrpWiseFilter.Value);
            Session["IsTrialBalanceGrpWiseFilter"] = IsTrialBalanceGrpWiseFilter;

            string asondate = "";
            if (is_asondate == false)
            {
                asondate = "N";
            }
            else
            {
                asondate = "Y";
            }

            Session["Isasondate"] = asondate;

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
                string FROMDATE = "";
                string TODATE = "";
                if (asondate == "Y")
                {
                    FROMDATE = dtTo.ToString("yyyy-MM-dd");
                    TODATE = dtTo.ToString("yyyy-MM-dd");
                }
                else
                {
                    FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                    TODATE = dtTo.ToString("yyyy-MM-dd");
                }

                string BRANCH_ID = "";

                string BranchComponent = "";
                List<object> BranchList = lookup_branch.GridView.GetSelectedFieldValues("ID");
                foreach (object Branch in BranchList)
                {
                    BranchComponent += "," + Branch;
                }
                BRANCH_ID = BranchComponent.TrimStart(',');

                string GroupList = "";
                List<object> CashBankList = lookupGroup.GridView.GetSelectedFieldValues("ID");
                foreach (object GroupIDs in CashBankList)
                {
                    GroupList += "," + GroupIDs;
                }
                GroupList = GroupList.TrimStart(',');

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

                Task PopulateStockTrialDataTask = new Task(() => GetTrialBalGrpdata(FROMDATE, TODATE, BRANCH_ID, asondate, checkshowzerobal, branchid, GroupList, checkBSPL));
                PopulateStockTrialDataTask.RunSynchronously();
                ShowGrid.ExpandAll();
            }
            else
            {
                ShowGrid.JSProperties["cpErrorFinancial"] = "ErrorFinancial";
            }
        }

        public void GetTrialBalGrpdata(string FROMDATE, string TODATE, string BRANCH_ID, string asondate, int checkshowzerobal, string HeadBranch, string GroupID, int checkBSPL)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_TRIALBALANCEGROUPWISE_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                cmd.Parameters.AddWithValue("@TODATE", TODATE);
                cmd.Parameters.AddWithValue("@BRANCH_ID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@ASONDATE", asondate);
                cmd.Parameters.AddWithValue("@SHOWZEROBAL", checkshowzerobal);
                cmd.Parameters.AddWithValue("@HO", HeadBranch);
                cmd.Parameters.AddWithValue("@GroupID", GroupID);
                cmd.Parameters.AddWithValue("@SHOWBSPL", checkBSPL);
                cmd.Parameters.AddWithValue("@VAL_TYPE", ddlValTech.SelectedValue);
                cmd.Parameters.AddWithValue("@OWMASTERVALTECH", (chkOWMSTVT.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@CONSOPBALSUBLEDG", (chkSLOPBal.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@CONSLANDEDCOST", (chkConsLandCost.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@CONSOVERHEADCOST", (chkConsOverHeadCost.Checked) ? "1" : "0");
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
            if (Convert.ToString(Session["IsTrialBalanceGrpWiseFilter"]) == "Y")
            {
                dtLedgerTotal = oDBEngine.GetDataTable("SELECT LEDGER,OP_DR,OP_CR,PR_DR,PR_CR,CLOSE_DR,CLOSE_CR FROM TRIALBALANCEGROUPWISE_REPORT WHERE USERID=" + Convert.ToInt32(Session["userid"]) + " AND SL=7 AND LEDGER='NET TOTAL:' AND PARENTGRPID='9999999999999'");
                LedgerTotalBalDesc = dtLedgerTotal.Rows[0][0].ToString();
                LedgerTotalOpDr= dtLedgerTotal.Rows[0][1].ToString();
                LedgerTotalOpCr = dtLedgerTotal.Rows[0][2].ToString();
                LedgerTotalPrDr = dtLedgerTotal.Rows[0][3].ToString();
                LedgerTotalPrCr= dtLedgerTotal.Rows[0][4].ToString();
                LedgerTotalClDr = dtLedgerTotal.Rows[0][5].ToString();
                LedgerTotalClCr = dtLedgerTotal.Rows[0][6].ToString();
            }

            string summaryTAG = (e.Item as ASPxSummaryItem).Tag;
            if (e.IsTotalSummary == true)
            {
                switch (summaryTAG)
                {
                    case "Item_Ledger":
                        e.Text = LedgerTotalBalDesc;
                        break;
                    case "Item_OpDr":
                        e.Text = LedgerTotalOpDr;
                        break;
                    case "Item_OpCr":
                        e.Text = LedgerTotalOpCr;
                        break;
                    case "Item_PrDr":
                        e.Text = LedgerTotalPrDr;
                        break;
                    case "Item_PrCr":
                        e.Text = LedgerTotalPrCr;
                        break;
                    case "Item_ClDr":
                        e.Text = LedgerTotalClDr;
                        break;
                    case "Item_ClCr":
                        e.Text = LedgerTotalClCr;
                        break;
                }
            }
        }

        protected void ShowGrid_DataBinding(object sender, EventArgs e)
        {
            if (Session["dtLedger"] != null)
            {
                ShowGrid.DataSource = (DataTable)Session["dtLedger"];
            }

            ASPxGridView grid = (ASPxGridView)sender;
            if (Convert.ToString(Session["Isasondate"]) == "Y")
            {
                foreach (GridViewDataColumn c in grid.Columns)
                {
                    if ((c.FieldName.ToString()).StartsWith("OP_DR"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("OP_CR"))
                    {
                        c.Visible = false;
                    }
                }
            }
            else
            {
                foreach (GridViewDataColumn c in grid.Columns)
                {
                    if ((c.FieldName.ToString()).StartsWith("OP_DR"))
                    {
                        c.Visible = true;
                    }
                    if ((c.FieldName.ToString()).StartsWith("OP_CR"))
                    {
                        c.Visible = true;
                    }
                }
            }
        }

        //protected void ShowGrid_DataBound(object sender, EventArgs e)
        //{
        //    ASPxGridView grid = (ASPxGridView)sender;
        //    foreach (GridViewDataColumn c in grid.Columns)
        //    {
        //        if ((c.FieldName.ToString()).StartsWith("BOLD_GRP"))
        //        {
        //            c.Visible = true;
        //        }
        //        if ((c.FieldName.ToString()).StartsWith("LEDGER"))
        //        {
        //            c.Visible = true;
        //            c.Caption = "Particulars";

        //        }
        //    }
        //}

        #endregion
                
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
        protected void lookupGroup_DataBinding(object sender, EventArgs e)
        {
            lookupGroup.DataSource = GetGroupList();
        }

        public DataTable GetGroupList()
        {
            string query = "";

            try
            {
                query = @"SELECT AccountGroup_ReferenceID ID, AccountGroup_Name Name FROM Master_AccountGroup order by AccountGroup_Name";

                DataTable dt = oDBEngine.GetDataTable(query);
                return dt;
            }
            catch
            {
                return null;
            }
        }

        protected void GroupPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            lookupGroup.GridView.Selection.CancelSelection();
            lookupGroup.DataSource = GetGroupList();
            lookupGroup.DataBind();
        }

        #region LinQ
        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsTrialBalanceGrpWiseFilter"]) == "Y")
            {
                var q = from d in dc.TRIALBALANCEGROUPWISE_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.SL) != "7" && Convert.ToString(d.PARENTGRPID) != "9999999999999"
                        orderby d.SEQ ascending
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.TRIALBALANCEGROUPWISE_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ ascending
                        select d;
                e.QueryableSource = q;
            }

            ShowGrid.ExpandAll();
        }

        protected void ShowGrid_HtmlFooterCellPrepared(object sender, ASPxGridViewTableFooterCellEventArgs e)
        {
            if (e.Column.Caption != "Particulars")
            {
                e.Cell.Style["text-align"] = "right";
            }

        }

        protected void ShowGrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (Convert.ToString(e.CellValue) == "B" || Convert.ToString(e.CellValue) == "LIABILITY" || Convert.ToString(e.CellValue) == "ASSET" || Convert.ToString(e.CellValue) == "INCOME" || Convert.ToString(e.CellValue) == "EXPENSE")
            {
                e.Cell.Font.Bold = true;
                e.Cell.CssClass = "makebold dxgv";
                Session["chk_presenttotal"] = 1;
                e.Cell.ForeColor = Color.Red;
            }
            if (Convert.ToInt32(Session["chk_presenttotal"]) == 1)
            {
                e.Cell.Font.Bold = true;
            }

            if (Convert.ToString(e.CellValue).Contains("Total:") || Convert.ToString(e.CellValue) == "B")
            {
                e.Cell.BackColor = Color.DarkSeaGreen;
            }

            if (e.DataColumn.FieldName == "CLOSE_CR")
            {
                Session["chk_presenttotal"] = 0;
            }
        }
        #endregion
    }
}