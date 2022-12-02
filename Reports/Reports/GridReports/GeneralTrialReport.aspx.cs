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
    public partial class GeneralTrialReport : ERP.OMS.ViewState_class.VSPage
    {
        //DataTable DTIndustry = new DataTable();
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        //string data = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        decimal TotalDebit = 0, TotalCredit = 0;
        decimal _totalDebit = 0, _totalCredit = 0, _totalBalance = 0;
        decimal _totalDiffofdbcr = 0;
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
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/GeneralTrialReport.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            if (!IsPostBack)
            {
                //ACPrd.Text = "Accounting Period : " + Convert.ToDateTime(Session["FinYearStart"].ToString()).ToString("dd/MM/yyyy") + " To " + Convert.ToDateTime(Session["FinYearEnd"].ToString()).ToString("dd/MM/yyyy");
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Trial Balance";
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
                ddlExport3.SelectedIndex = 0;
                Session["SI_ComponentData"] = null;
                Session["SI_ComponentData_ledger"] = null;
                //Session.Remove("dt_CombineStockTrailRptLeve2");
                //Session["dtLedger"] = null;
                Session["SI_ComponentData_Branch"] = null;
                Session["IsGenTrialFilter"] = null;
                Session["IsGenTrialDetFilter"] = null;
                Session["LedgerId"] = null;
                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                //   BindDropDownList();
                ASPxFromDate.Text = dtFrom.ToString("dd-MM-yyyy");
                ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");

                lookupCashBank.DataBind();

                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                //ASPxFromDate.Value = DateTime.Now;
                //ASPxToDate.Value = DateTime.Now;
                //Rev Subhra 18-12-2018   0017670
                Session["BranchNames"] = null;
                //End Rev
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
                //Session.Remove("dtLedger");
                //ShowGrid.JSProperties["cpSave"] = null;
                //  string[] CallVal = e.Parameters.ToString().Split('~');

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
                ////ASPxFromDate.Text = Convert.ToDateTime(stbill.Rows[0]["FinYear_StartDate"]).ToString("dd-MM-yyyy");
                ////ASPxToDate.Text = Convert.ToDateTime(stbill.Rows[0]["FinYear_EndDate"]).ToString("dd-MM-yyyy");
                //FromDate.Value = Convert.ToDateTime(stbill.Rows[0]["FinYear_StartDate"]).ToString("dd-MM-yyyy");
                //ToDate.Value = Convert.ToDateTime(stbill.Rows[0]["FinYear_EndDate"]).ToString("dd-MM-yyyy");

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
                    // Session["exportval"] = Filter;
                    // BindDropDownList();
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    //  Session["exportval"] = Filter;
                    // BindDropDownList();
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
            //Rev  Subhra  0019243  20-12-2018 
            //string filename = Convert.ToString((Session["Contactrequesttype"] ?? "General Trial Summary Report"));
            string filename = "General Trial Summary Report";
            //End of Rev
            exporter.FileName = filename;
            string FileHeader = "";
            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            if (radAsDate.Checked == true)
            {
                FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true,true, true, true) + Environment.NewLine + "General Trial Summary Report" + Environment.NewLine + "As on " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy");
                //Rev Subhra 18-12-2018   0017670
                FileHeader = ReplaceFirst(FileHeader, "\r\n", Convert.ToString(Session["BranchNames"]));
                //End of Rev
            }
            else
            {
                FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true,true, true, true) + Environment.NewLine + "General Trial Summary Report" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
                //Rev Subhra 18-12-2018   0017670
                FileHeader = ReplaceFirst(FileHeader, "\r\n", Convert.ToString(Session["BranchNames"]));
                //End of Rev
            }

            exporter.RenderBrick += exporter_RenderBrick;
            //exporter.PageHeader.Left = Convert.ToString((Session["Contactrequesttype"] ?? "General Trial Summary Report"));
            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";
            //exporter.PageFooter.Center = "[Page # of Pages #]";
            //exporter.PageFooter.Right = "[Date Printed]";
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

        #region main grid details
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string branchid = Convert.ToString(e.Parameter.Split('~')[2]);
            bool is_asondate = false;
            //Session.Remove("dtLedger");
            
            //ShowGrid.JSProperties["cpSave"] = null;
            string[] CallVal = e.Parameter.ToString().Split('~');
            is_asondate = Convert.ToBoolean(CallVal[1]);

            string IsGenTrialFilter = Convert.ToString(hfIsGeneralTrialFilter.Value);
            Session["IsGenTrialFilter"] = IsGenTrialFilter;

            //string type = CallVal[1];
            //string code = CallVal[2];
            //if (CallVal[1]== "null")
            //{
            //    type = "";
            //}
            //if (CallVal[2]== "null")
            //{
            //    code = "";
            //}
            string asondate = "";
             if (is_asondate==false)
             {
                 asondate = "N";
             }
             else
             {
                 asondate = "Y";
             }

            //Rev Debashis && Suppress all Opening Columns
             Session["Isasondate"] = asondate;
            //End of Rev Debashis && Suppress all Opening Columns
           
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
                //Rev Debashis
                //string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                //string TODATE = dtTo.ToString("yyyy-MM-dd");
                string FROMDATE = "";
                string TODATE = "";
                if(asondate=="Y")
                {
                    //FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                    //TODATE = dtFrom.ToString("yyyy-MM-dd");
                    FROMDATE = dtTo.ToString("yyyy-MM-dd");
                    TODATE = dtTo.ToString("yyyy-MM-dd");
                }
                else
                {
                    FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                    TODATE = dtTo.ToString("yyyy-MM-dd");
                }
                //End of Rev Debashis

                string BRANCH_ID = "";

                string QuoComponent = "";
                List<object> QuoList = lookup_branch.GridView.GetSelectedFieldValues("ID");
                foreach (object Quo in QuoList)
                {
                    QuoComponent += "," + Quo;
                }
                BRANCH_ID = QuoComponent.TrimStart(',');

                string cashbankList = "";
                List<object> CashBankList = lookupCashBank.GridView.GetSelectedFieldValues("ID");
                foreach (object cashbankIDs in CashBankList)
                {
                    cashbankList += "," + cashbankIDs;
                }
                cashbankList = cashbankList.TrimStart(',');

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

                int checkPARTY = 0;
                if (chkparty.Checked == true)
                {
                    checkPARTY = 1;
                }
                else if (chkparty.Checked == false)
                {
                    checkPARTY = 0;
                }

                //string HeadBranch = branchid;

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


                Task PopulateStockTrialDataTask = new Task(() => GetLedgerdata(FROMDATE, TODATE, BRANCH_ID, asondate, checkshowzerobal, branchid, cashbankList, checkBSPL, checkPARTY));
                PopulateStockTrialDataTask.RunSynchronously();
                ShowGrid.ExpandAll();
               // lbldiffcalculationText.Text = "Mismatch Defeated";
                //lbldiffcalculation.Text = Convert.ToString(_totalDiffofdbcr);
                ShowGrid.JSProperties["cpMismatch"] = _totalDiffofdbcr;
            }
            else
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Notify", "jAlert('Date Range should be within Financial Year');", true);
                ShowGrid.JSProperties["cpErrorFinancial"] = "ErrorFinancial";
            }

        
        }

        public void GetLedgerdata(string FROMDATE, string TODATE, string BRANCH_ID, string asondate, int checkshowzerobal, string HeadBranch, string GroupID, int checkBSPL, int checkPARTY)
       {
            try
            {                
                DataSet ds = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_GENERAL_TRIAL_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                cmd.Parameters.AddWithValue("@TODATE", TODATE);
                cmd.Parameters.AddWithValue("@BRANCH_ID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@AsonDate", asondate);
                cmd.Parameters.AddWithValue("@SHOWZEROBAL", checkshowzerobal);
                cmd.Parameters.AddWithValue("@HO", HeadBranch);
                cmd.Parameters.AddWithValue("@GroupID", GroupID);
                cmd.Parameters.AddWithValue("@SHOWBSPL", checkBSPL);
                cmd.Parameters.AddWithValue("@P_INVOICE_DATE", checkPARTY);
                cmd.Parameters.AddWithValue("@VAL_TYPE", ddlValTech.SelectedValue);
                cmd.Parameters.AddWithValue("@OWMASTERVALTECH", (chkOWMSTVT.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@CONSLANDEDCOST", (chkConsLandCost.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@CONSOVERHEADCOST", (chkConsOverHeadCost.Checked) ? "1" : "0");
                cmd.Parameters.AddWithValue("@USERID", Convert.ToInt32(Session["userid"]));

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);

                cmd.Dispose();
                con.Dispose();

                //Session["dtLedger"] = ds.Tables[0];

                //ShowGrid.DataSource = ds.Tables[0];
                //ShowGrid.DataBind();
                ShowGrid.JSProperties["cpSummary"] = (Convert.ToDecimal(ShowGrid.GetTotalSummaryValue(ShowGrid.TotalSummary["Close_Dr"])) - Convert.ToDecimal(ShowGrid.GetTotalSummaryValue(ShowGrid.TotalSummary["Close_Cr"])));

            }
            catch (Exception ex)
            {
            }
        }

     protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
     {

         //if (e.Item.FieldName == "CREDIT")
         //{
         //    TotalCredit = Convert.ToDecimal(e.Value);
         //}
         //else if (e.Item.FieldName == "DEBIT")
         //{
         //    TotalDebit = Convert.ToDecimal(e.Value);
         //}

         if (e.Item.FieldName == "Ledger")
         {
             e.Text = "Net Total";
         }
         //else if (e.Item.FieldName == "Close_Dr")
         //{
         //    TotalDebit = Convert.ToDecimal(e.Value);
         //}
         //else if (e.Item.FieldName == "Close_Cr")
         //{
         //    TotalCredit = Convert.ToDecimal(e.Value);
         //}

         //if (e.Item.FieldName == "Close_Dr")
         //{
         //    e.Text = Convert.ToString(TotalDebit);
         //}
         //else if (e.Item.FieldName == "Close_Cr")
         //{
         //    e.Text = Convert.ToString(TotalCredit);
         //}
         else
         {
             e.Text = string.Format("{0}", Math.Abs(Convert.ToDecimal(e.Value)));
         }
     }

     //protected void ShowGrid_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
     //{
        
     //    //if (e.Item == ShowGrid.TotalSummary["Close_Dr"])
     //    //{
     //    //    if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
     //    //    {
     //    //        Decimal gmv = Convert.ToDecimal(ShowGrid.GetTotalSummaryValue(ShowGrid.TotalSummary["DEBIT"]));
     //    //        Decimal equity = Convert.ToDecimal(ShowGrid.GetTotalSummaryValue(ShowGrid.TotalSummary["CREDIT"]));

     //    //        e.TotalValue = gmv - equity;
     //    //        e.TotalValueReady = true;
     //    //    }
     //    //}
     //   // lbldiffcalculation.Text = "";
     //    string summaryTAG = (e.Item as ASPxSummaryItem).Tag;

     //    if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Start)
     //    {
     //        _totalDebit = 0;
     //        _totalCredit = 0;
     //        _totalBalance = 0;
     //        _totalDiffofdbcr = 0;
     //    }
     //    else if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
     //    {
     //        _totalDebit += Convert.ToDecimal(e.GetValue("Close_Dr"));
     //        _totalCredit += Convert.ToDecimal(e.GetValue("Close_Cr"));

     //    }
     //    else if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
     //    {
     //        switch (summaryTAG)
     //        {
     //            case "Close_Dr":
     //                e.TotalValue = _totalDebit;
     //                break;
     //            case "Close_Cr":
     //                e.TotalValue = _totalCredit;
     //                break;                     
     //        }
     //    }
     //}


     protected void ShowGrid_DataBinding(object sender, EventArgs e)
     {
         if (Session["dtLedger"] != null)
         {
             ShowGrid.DataSource = (DataTable)Session["dtLedger"];
             //  ShowGrid.DataBind();
         }

         //Rev Debashis && Suppress all Opening Columns
         ASPxGridView grid = (ASPxGridView)sender;
         if (Convert.ToString(Session["Isasondate"]) == "Y")
         {
             foreach (GridViewDataColumn c in grid.Columns)
             {
                 if ((c.FieldName.ToString()).StartsWith("Op_Dr"))
                 {
                     c.Visible = false;
                 }
                 if ((c.FieldName.ToString()).StartsWith("Op_Cr"))
                 {
                     c.Visible = false;
                 }
             }
         }
         else
         {
             foreach (GridViewDataColumn c in grid.Columns)
             {
                 if ((c.FieldName.ToString()).StartsWith("Op_Dr"))
                 {
                     c.Visible = true;
                 }
                 if ((c.FieldName.ToString()).StartsWith("Op_Cr"))
                 {
                     c.Visible = true;
                 }
             }
         }
         //End of Rev Debashis && Suppress all Opening Columns

     }

     protected void ShowGrid_DataBound(object sender, EventArgs e)
     {
         ASPxGridView gridView = sender as ASPxGridView;
         gridView.JSProperties["cpSummary"] = gridView.GetTotalSummaryValue(gridView.TotalSummary["Close_Dr"]);
         //hfmismatchvalue.Value = Convert.ToString(gridView.GetTotalSummaryValue(gridView.TotalSummary["Close_Dr"]));
     }


        #endregion

        //[WebMethod]
        //public static List<string> GetBranchesList(String NoteId)
        //{
        //    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        //    StringBuilder filter = new StringBuilder();
        //    StringBuilder Supervisorfilter = new StringBuilder();
        //    BusinessLogicLayer.Others objbl = new BusinessLogicLayer.Others();
        //    DataTable dtbl = new DataTable();
        //    if (NoteId.Trim() == "")
        //    {
        //        dtbl = oDBEngine.GetDataTable("select branch_id,branch_description from tbl_master_branch where branch_id in(" + Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) + ")  order by branch_description asc");
        //    }
        //    List<string> obj = new List<string>();
        //    foreach (DataRow dr in dtbl.Rows)
        //    {
        //        obj.Add(Convert.ToString(dr["branch_description"]) + "|" + Convert.ToString(dr["branch_id"]));
        //    }
        //    return obj;
        //}

        //[WebMethod]
        //public static List<string> BindLedgerType(String Ids)
        //{
        //    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        //    DataTable dtbl = new DataTable();
        //    //if (Ids.Trim() != "")
        //    //{
        //    //    dtbl = oDBEngine.GetDataTable("select A.MainAccount_ReferenceID AS ID,A.MainAccount_Name as 'AccountName' FROM Master_MainAccount A WHERE A.MainAccount_AccountCode IN(SELECT RTRIM(B.AccountsLedger_MainAccountID) FROM Trans_AccountsLedger B WHERE B.AccountsLedger_BranchId in(" + Ids + ")) ORDER BY A.MainAccount_Name ");

        //    //}         
        //    if (Ids == "null")
        //    {
        //        Ids = "0";
        //    }
        //    List<string> obj = new List<string>();
        //    obj = GetLedgerBind(Ids.Trim());
        //    //foreach (DataRow dr in dtbl.Rows)
        //    //{
        //    //    obj.Add(Convert.ToString(dr["AccountName"]) + "|" + Convert.ToString(dr["Id"]));
        //    //}
        //    return obj;
        //}

        //[WebMethod]
        //public static List<string> BindCustomerVendor()
        //{
        //    BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        //    DataTable dtbl = new DataTable();
        //    //dtbl = oDBEngine.GetDataTable("select cnt_id AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name'  FROM tbl_master_contact WHERE cnt_contactType in('CL','DV') AND cnt_branchid IN("+ Ids +") ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");
        //    dtbl = oDBEngine.GetDataTable("select cnt_id AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') as 'Name'  FROM tbl_master_contact WHERE cnt_contactType in('CL','DV') ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");
        //    List<string> obj = new List<string>();
        //    foreach (DataRow dr in dtbl.Rows)
        //    {
        //        obj.Add(Convert.ToString(dr["Name"]) + "|" + Convert.ToString(dr["Id"]));
        //    }
        //    return obj;
        //}

        //public static List<string> GetLedgerBind(string branch)
        //{
        //    CommonBL bll1 = new CommonBL();
        //    DataTable stbill = new DataTable();
        //    stbill = bll1.GetLedgerBind(branch);
        //    List<string> obj = new List<string>();
        //    if (stbill.Rows.Count > 0)
        //    {
        //        foreach (DataRow dr in stbill.Rows)
        //        {
        //            obj.Add(Convert.ToString(dr["AccountName"]) + "|" + Convert.ToString(dr["Id"]));
        //        }
        //    }
        //    return obj;
        //}

        #region Branch Populate

        protected void Componentbranch_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            //string FinYear = Convert.ToString(Session["LastFinYear"]);
            //if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            //{
            //    DataTable ComponentTable = new DataTable();
            //    string Hoid = e.Parameter.Split('~')[1];
            //    if (Session["userbranchHierarchy"] != null)
            //    {
            //        ComponentTable = oDBEngine.GetDataTable("select branch_id as ID,branch_description,branch_code from tbl_master_branch   where branch_id in(" + Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) + ")   order by branch_description asc");
            //    }
            //    if (ComponentTable.Rows.Count > 0)
            //    {
            //        Session["SI_ComponentData_Branch"] = ComponentTable;
            //        lookup_branch.DataSource = ComponentTable;
            //        lookup_branch.DataBind();
            //    }
            //    else
            //    {
            //        lookup_branch.DataSource = null;
            //        lookup_branch.DataBind();
            //    }
            //}

            //string FinYear = Convert.ToString(Session["LastFinYear"]);
            //if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            //{
            //    DataTable ComponentTable = new DataTable();
            //    string Hoid = e.Parameter.Split('~')[1];
            //    ComponentTable = oDBEngine.GetDataTable("select branch_id as ID,branch_description,branch_code from tbl_master_branch where branch_parentId='" + Hoid + "' and branch_id in(" + Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) + ") order by branch_description asc");
            //    //if (Session["userbranchHierarchy"] != null)
            //    //{
            //    //    ComponentTable = oDBEngine.GetDataTable("select branch_id as ID,branch_description,branch_code from tbl_master_branch where branch_id in(" + Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) + ") order by branch_description asc");
            //    //}
            //    if (ComponentTable.Rows.Count > 0)
            //    {
            //        Session["SI_ComponentData_Branch"] = ComponentTable;
            //        lookup_branch.DataSource = ComponentTable;
            //        lookup_branch.DataBind();
            //    }
            //    else
            //    {
            //        Session["SI_ComponentData_Branch"] = null;
            //        lookup_branch.DataSource = null;
            //        lookup_branch.DataBind();

            //    }
            //}

            string FinYear = Convert.ToString(Session["LastFinYear"]);

            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                DataTable ComponentTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];
                if (Hoid != "All")
                {
                    //if (Session["userbranchHierarchy"] != null)
                    //{
                    //    ComponentTable = oDBEngine.GetDataTable("select branch_id as ID,branch_description,branch_code from tbl_master_branch   where branch_id in(" + Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) + ")   order by branch_description asc");
                    //}
                    ///  ComponentTable = oDBEngine.GetDataTable("select branch_id as ID,branch_description,branch_code from tbl_master_branch where branch_parentId='" + Hoid + "' and  branch_id in(" + Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) + ")  order by branch_description asc");

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
                    //Rev Debashis && Implement All parent branch.
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
            cmd.Dispose();
            con.Dispose();

            return dt;
        }
        protected void lookup_branch_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_ComponentData_Branch"] != null)
            {
                //    DataTable ComponentTable = oDBEngine.GetDataTable("select branch_id,branch_description,branch_code from tbl_master_branch where branch_parentId='" + Hoid + "' order by branch_description asc");
                lookup_branch.DataSource = (DataTable)Session["SI_ComponentData_Branch"];
            }
        }

        #endregion

        #region ##### 2nd Level Grid Details #########
        protected void CallbackPanel2_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string ledger;
            string asondatewise;
            string ledgerDesc;
            //Rev Debashis && Implement For Branch Zoomin facility
            string ledgertype;
            //End of Rev Debashis && Implement For Branch Zoomin facility
            //Rev Debashis && Implement Document Date
            string shortnametype;
            //End of Rev Debashis && Implement Document Date
            string[] CallVal2ndlevel= e.Parameter.ToString().Split('~');
            ledger = CallVal2ndlevel[0];
            Session["LedgerId"] = ledger;
            asondatewise = Convert.ToString(CallVal2ndlevel[1]);
            //Rev Debashis && Implement For Branch Zoomin facility
            ledgertype = CallVal2ndlevel[3];
            Session["ledgertype"] = ledgertype;
            //End of Rev Debashis && Implement For Branch Zoomin facility
            //Rev Debashis && Implement Document Date
            DataTable dtshortnametype = null;
            shortnametype = "";
            //shortnametype = CallVal2ndlevel[4];
            //Session["shortnametype"] = shortnametype;
            //End of Rev Debashis && Implement Document Date

            string IsGenTrialDetFilter = Convert.ToString(hfIsGeneralTrialFilterDetails.Value);
            Session["IsGenTrialDetFilter"] = IsGenTrialDetFilter;

            DataTable dtledgdesc = null;
            ledgerDesc = "";

            if (ledger != "null" && ledger != "0") 
            {
                //Rev Debashis
                //dtledgdesc = oDBEngine.GetDataTable("Select MainAccount_Name from Master_MainAccount Where MainAccount_ReferenceID='" + ledger + "'");
                //Rev Debashis && Implement For Branch Zoomin facility
                //if (ledger != "SYSTM00010")
                if (ledger != "SYSTM00010" && ledgertype == "LEDG")
                //End of Rev Debashis && Implement For Branch Zoomin facility
                {
                    dtledgdesc = oDBEngine.GetDataTable("Select MainAccount_Name from Master_MainAccount Where CONVERT(NVARCHAR(MAX),MainAccount_ReferenceID)='" + ledger + "'");
                    dtshortnametype = oDBEngine.GetDataTable("Select MainAccount_AccountCode from Master_MainAccount Where CONVERT(NVARCHAR(MAX),MainAccount_ReferenceID)='" + ledger + "'");
                    shortnametype = dtshortnametype.Rows[0][0].ToString();
                }
                //Rev Debashis && Implement For Branch Zoomin facility
                //else
                else if (ledger == "SYSTM00010" && ledgertype == "SUSP")
                //End of Rev Debashis && Implement For Branch Zoomin facility
                {
                    dtledgdesc = oDBEngine.GetDataTable("Select MainAccount_Name from Master_MainAccount Where MainAccount_AccountCode='" + ledger + "'");
                    dtshortnametype = oDBEngine.GetDataTable("Select MainAccount_AccountCode from Master_MainAccount Where MainAccount_AccountCode='" + ledger + "'");
                    shortnametype = dtshortnametype.Rows[0][0].ToString();
                    CallbackPanel2.JSProperties["cpSuspLedger"] = "SYSTM00010";
                }                
                //End of Rev Debashis
                //Rev Debashis && Implement For Branch Zoomin facility
                else
                {
                    dtledgdesc = oDBEngine.GetDataTable("Select branch_description from tbl_master_branch Where branch_id='" + ledger + "'");
                    shortnametype = "BRAN";
                }
                //End of Rev Debashis && Implement For Branch Zoomin facility
                ledgerDesc = dtledgdesc.Rows[0][0].ToString();
            }
            else
            {
                dtledgdesc = null;
                ledgerDesc = null;
                dtshortnametype = null;
                shortnametype = null;
            }

            //Rev Debashis && Implement Document Date
            Session["shortnametype"] = shortnametype;
            //End of Rev Debashis && Implement Document Date

            if (!string.IsNullOrEmpty(ledger) && ledger != "0")
            {
                //Session.Remove("dt_CombineStockTrailRptLeve2");
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

                //DateTime dtFrom;
                //dtFrom = Convert.ToDateTime(ASPxFromDate.Date);

                //string FROMDATE = dtFrom.ToString("yyyy-MM-dd");

                //DateTime dtTo;
                //dtTo = Convert.ToDateTime(ASPxToDate.Date);

                //string TODATE = dtTo.ToString("yyyy-MM-dd");
                DateTime dtFrom;
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                DateTime dtTo;
                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                string FROMDATE = "";
                string TODATE = "";
                if (asondatewise == "Y")
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

                string QuoComponent = "";
                List<object> QuoList = lookup_branch.GridView.GetSelectedFieldValues("ID");
                foreach (object Quo in QuoList)
                {
                    QuoComponent += "," + Quo;
                }
                BRANCH_ID = QuoComponent.TrimStart(',');

                string branchid = Convert.ToString(e.Parameter.Split('~')[2]);

                int checkPARTY = 0;
                if (chkparty.Checked == true)
                {
                    checkPARTY = 1;
                }
                else if (chkparty.Checked == false)
                {
                    checkPARTY = 0;
                }

                DataTable dt = new DataTable();
                //Rev Debashis && Implement For Branch Zoomin facility
                //dt = GetGeneralLedger2ndLevel(FROMDATE, TODATE, ledger, asondatewise, BRANCH_ID, branchid, checkPARTY);
                //Rev Debashis && Implement Document Date
                //dt = GetGeneralLedger2ndLevel(FROMDATE, TODATE, ledger, asondatewise, BRANCH_ID, branchid, checkPARTY, ledgertype);
                ////dt = GetGeneralLedger2ndLevel(FROMDATE, TODATE, ledger, asondatewise, BRANCH_ID, branchid, checkPARTY, ledgertype,shortnametype);
                GetGeneralLedger2ndLevel(FROMDATE, TODATE, ledger, asondatewise, BRANCH_ID, branchid, checkPARTY, ledgertype, shortnametype);

                //End of Rev Debashis && Implement Document Date
                //End of Rev Debashis && Implement For Branch Zoomin facility

                //Session["dt_CombineStockTrailRptLeve2"] = dt;
                //if (Session["dt_CombineStockTrailRptLeve2"] != null)
                //{
                    // DataTable dt = (DataTable)Session["dt_CombineStockTrailRptLeve2"];
                    //if (dt.Rows.Count > 0)
                    //{
                        //ShowGridDetails2Level.JSProperties["cpLedger"] = Convert.ToString(ledgerDesc);
                        //ShowGridDetails2Level.JSProperties["cpFromDate"] = dtFrom.ToString("dd-MM-yyyy");
                        //ShowGridDetails2Level.JSProperties["cpToDate"] = dtTo.ToString("dd-MM-yyyy");
                        ////Rev Debashis && Implement For Branch Zoomin facility
                        //ShowGridDetails2Level.JSProperties["cpLedgerType"] = Convert.ToString(ledgertype);
                        //End of Rev Debashis && Implement For Branch Zoomin facility
                        //Rev Debashis && Implement Document Date
                        //ShowGridDetails2Level.JSProperties["cpshortnametype"] = Convert.ToString(shortnametype);
                        //End of Rev Debashis && Implement Document Date
                    //}
                //}

                //ShowGridDetails2Level.DataSource = dt;
                //ShowGridDetails2Level.DataBind();

                CallbackPanel2.JSProperties["cpLedger"] = Convert.ToString(ledgerDesc);
                CallbackPanel2.JSProperties["cpFromDate"] = dtFrom.ToString("dd-MM-yyyy");
                CallbackPanel2.JSProperties["cpToDate"] = dtTo.ToString("dd-MM-yyyy");
                CallbackPanel2.JSProperties["cpLedgerType"] = Convert.ToString(ledgertype);
            }
            else
            {
                Session["dt_CombineStockTrailRptLeve2"] = null;
                ShowGridDetails2Level.DataSource = null;
                ShowGridDetails2Level.DataBind();
            }
        }

        protected void ShowGridDetails2Level_DataBound(object sender, EventArgs e)
        {
            //ASPxGridView grid = (ASPxGridView)sender;
            //foreach (GridViewDataColumn c in grid.Columns)
            //{
            //    if ((c.FieldName.ToString()).StartsWith("ledgr"))
            //    {
            //        //c.Visible = false;
            //        c.Width = 10;
            //    }

                //if ((c.FieldName.ToString()).StartsWith("sProducts_ID"))
                //{
                //    //c.Visible = false;
                //    c.Width = 0;
                //}

                //if ((c.FieldName.ToString()).StartsWith("sProducts_Code"))
                //{
                //    //c.Visible = false;
                //    c.Width = 0;
                //}

                //if ((c.FieldName.ToString()).StartsWith("branch_id"))
                //{
                //    //c.Visible = false;
                //    c.Width = 0;
                //}

            //}
        }

        //Rev Debashis && Implement For Branch Zoomin facility
        //private DataTable GetGeneralLedger2ndLevel(string FromDate, string ToDate, string ledger, string asondatewise, string BRANCH_ID, string HeadBranch, int checkPARTY)
        //Rev Debashis && Implement Document Date
        //private DataTable GetGeneralLedger2ndLevel(string FromDate, string ToDate, string ledger, string asondatewise, string BRANCH_ID, string HeadBranch, int checkPARTY, string ledgertype)
        private void GetGeneralLedger2ndLevel(string FromDate, string ToDate, string ledger, string asondatewise, string BRANCH_ID, string HeadBranch, int checkPARTY, string ledgertype, string shortnametype)
        //End of Rev Debashis && Implement Document Date
        //End of Rev Debashis && Implement For Branch Zoomin facility
        {
            
            try
            {
                DataSet ds = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_GENERAL_TRIAL_DETAIL_REPORT", con);
             
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@FROMDATE", FromDate);
                cmd.Parameters.AddWithValue("@TODATE", ToDate);
                cmd.Parameters.AddWithValue("@BRANCH_ID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@AsonDate", asondatewise);
                cmd.Parameters.AddWithValue("@MainAcc_Ledger", ledger);
                cmd.Parameters.AddWithValue("@HO", HeadBranch);
                cmd.Parameters.AddWithValue("@P_INVOICE_DATE", checkPARTY);
                //Rev Debashis && Implement For Branch Zoomin facility
                cmd.Parameters.AddWithValue("@Ledger_Type", ledgertype);
                //End of Rev Debashis && Implement For Branch Zoomin facility
                //Rev Debashis && Implement Document Date
                cmd.Parameters.AddWithValue("@Ledger_ShortName", shortnametype);
                cmd.Parameters.AddWithValue("@USERID", Convert.ToInt32(Session["userid"]));

                //End of Rev Debashis && Implement Document Date
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);
                cmd.Dispose();
                con.Dispose();             
             
                //ShowGridDetails2Level.Columns.Clear();
                //return ds.Tables[0];
            }
            catch (Exception ex)
            {
                //return null;
            }
        }
        protected void ShowGridDetails2Level_DataBinding(object sender, EventArgs e)
        {
            //////////if (Session["dt_CombineStockTrailRptLeve2"] != null)
            //////////{
            //////////    ShowGridDetails2Level.DataSource = (DataTable)Session["dt_CombineStockTrailRptLeve2"];
            //////////}
            //////////else
            //////////{
            //////////    ShowGridDetails2Level.DataSource = null;
            //////////}

            ASPxGridView grid = (ASPxGridView)sender;
            //Rev Debashis && Implement For Branch Zoomin facility
            //if (Convert.ToString(Session["LedgerId"]) == "3" || Convert.ToString(Session["LedgerId"]) == "6")
            //Rev Debashis && Impement Document Date
            //if ((Convert.ToString(Session["LedgerId"]) == "3" || Convert.ToString(Session["LedgerId"]) == "6") && Convert.ToString(Session["ledgertype"]) == "LEDG")
            if ((Convert.ToString(Session["shortnametype"]) == "SYSTM00003" || Convert.ToString(Session["shortnametype"]) == "SYSTM00006") && Convert.ToString(Session["ledgertype"]) == "LEDG")
            //End of Rev Debashis && Impement Document Date
            //End of Rev Debashis && Implement For Branch Zoomin facility
            {
                foreach (GridViewDataColumn c in grid.Columns)
                {
                    if ((c.FieldName.ToString()).StartsWith("Trn_RefId"))
                    {
                        c.Visible = false;
                    }

                    if ((c.FieldName.ToString()).StartsWith("Acl_TrnDt"))
                    {
                        c.Visible = false;
                    }

                    if ((c.FieldName.ToString()).StartsWith("TRAN_TYPE"))
                    {
                        c.Visible = false;
                    }
                    if ((c.FieldName.ToString()).StartsWith("CUSTVENDNAME"))
                    {
                        c.Visible = true;
                        c.Width = 200;
                    }
                    //Rev Debashis && Group Name has been introduced.
                    if (Convert.ToString(Session["shortnametype"]) == "SYSTM00006")
                    {
                        if ((c.FieldName.ToString()).StartsWith("CVGRP_NAME"))
                        {
                            c.Visible = true;
                            c.Width = 200;
                        }
                    }
                    else if (Convert.ToString(Session["shortnametype"]) == "SYSTM00003")
                    {
                        if ((c.FieldName.ToString()).StartsWith("CVGRP_NAME"))
                        {
                            c.Visible = false;
                        }
                    }
                    //End of Rev Debashis && Group Name has been introduced.
                }
            }
            //Rev Debashis && Implement For Branch Zoomin facility
            //else
            //Rev Debashis && Impement Document Date
            //else if ((Convert.ToString(Session["LedgerId"]) != "3" || Convert.ToString(Session["LedgerId"]) != "6") && Convert.ToString(Session["ledgertype"]) == "LEDG")
            else if ((Convert.ToString(Session["shortnametype"]) != "SYSTM00003" || Convert.ToString(Session["shortnametype"]) != "SYSTM00006") && Convert.ToString(Session["ledgertype"]) == "LEDG")
            //End of Rev Debashis && Impement Document Date
            //End of Rev Debashis && Implement For Branch Zoomin facility
            {
                foreach (GridViewDataColumn c in grid.Columns)
                {
                    if ((c.FieldName.ToString()).StartsWith("CUSTVENDNAME"))
                    {
                        c.Visible = false;
                    }
                    //Rev Debashis && Group Name has been introduced.
                    if ((c.FieldName.ToString()).StartsWith("CVGRP_NAME"))
                    {
                        c.Visible = false;
                    }
                    //End of Rev Debashis && Group Name has been introduced.
                    if ((c.FieldName.ToString()).StartsWith("Trn_RefId"))
                    {
                        //Rev Debashis
                        c.Visible = true;
                        //End of Rev Debashis
                        c.Width = 110;
                    }
                    if ((c.FieldName.ToString()).StartsWith("Acl_TrnDt"))
                    {
                        c.Visible = true;
                        c.Width = 110;
                    }
                    if ((c.FieldName.ToString()).StartsWith("TRAN_TYPE"))
                    {
                        //Rev Debashis
                        c.Visible = true;
                        //End of Rev Debashis
                        c.Width = 110;
                    }
                }
            }
            //Rev Debashis && Implement For Branch Zoomin facility
            else if (Convert.ToString(Session["ledgertype"]) == "BRAN")
            {
                foreach (GridViewDataColumn c in grid.Columns)
                {
                    if ((c.FieldName.ToString()).StartsWith("CUSTVENDNAME"))
                    {
                        c.Visible = true;
                    }
                    //Rev Debashis && Group Name has been introduced.
                    if ((c.FieldName.ToString()).StartsWith("CVGRP_NAME"))
                    {
                        c.Visible = false;
                    }
                    //End of Rev Debashis && Group Name has been introduced.
                    if ((c.FieldName.ToString()).StartsWith("Trn_RefId"))
                    {
                        c.Visible = true;
                        c.Width = 110;
                    }
                    if ((c.FieldName.ToString()).StartsWith("Acl_TrnDt"))
                    {
                        c.Visible = true;
                        c.Width = 110;
                    }
                    if ((c.FieldName.ToString()).StartsWith("TRAN_TYPE"))
                    {
                        c.Visible = true;
                        c.Width = 110;
                    }
                }
            }
            //End of Rev Debashis && Implement For Branch Zoomin facility
        }

        protected void ShowGridDetails2Level_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }



        protected void ddlExport3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(ddlExport3.SelectedItem.Value));
            if (Filter != 0)
            {
                bindexport_Details3(Filter);
            }
        }

        public void bindexport_Details3(int Filter)
        {
            string filename = "General Trial Details Report";
            exporterDetails.FileName = filename;
            exporterDetails.FileName = "General Trial Details Report";

            string FileHeader = "";
            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            if (radAsDate.Checked == true)
            {
                FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true,true, true, true) + Environment.NewLine + "General Trial Details Report" + Environment.NewLine + "As on " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy");
            }
            else
            {
                FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true,true, true, true) + Environment.NewLine + "General Trial Details Report" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
            }
            
            exporterDetails.RenderBrick += exporter_RenderBrick;

            //exporterDetails.PageHeader.Left = "General Trial Details Report";
            exporterDetails.PageHeader.Left = FileHeader;
            exporterDetails.PageHeader.Font.Size = 10;
            exporterDetails.PageHeader.Font.Name = "Tahoma";
            exporterDetails.PageFooter.Center = "[Page # of Pages #]";
            exporterDetails.PageFooter.Right = "[Date Printed]";
            exporterDetails.GridViewID = "ShowGridDetails2Level";
            switch (Filter)
            {
                case 1:
                    //exporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    exporterDetails.WriteXlsxToResponse();
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

        protected void lookupCashBank_DataBinding(object sender, EventArgs e)
        {
            lookupCashBank.DataSource = GetCashBankList();
        }

        public DataTable GetCashBankList()
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

        protected void CashBankPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            lookupCashBank.GridView.Selection.CancelSelection();
            lookupCashBank.DataSource = GetCashBankList();
            lookupCashBank.DataBind();
        }

        #region LinQ
        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsGenTrialFilter"]) == "Y")
            {
                var q = from d in dc.TRIALBALANCESUMMARY_REPORTs
                        where Convert.ToString(d.USERID) == Userid 
                        orderby  d.SEQ ascending
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.TRIALBALANCESUMMARY_REPORTs
                        where Convert.ToString(d.SEQ) == "0" 
                        orderby  d.SEQ ascending
                        select d;
                e.QueryableSource = q;
            }

            ShowGrid.ExpandAll();
        }

        protected void GenerateEntityServerModeDataSourceDetails_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsGenTrialDetFilter"]) == "Y")
            {
                var q = from d in dc.TRIALBALANCEDETAIL_REPORTs
                        where Convert.ToString(d.USERID) == Userid
                        orderby d.SEQ ascending
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.TRIALBALANCEDETAIL_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ ascending
                        select d;
                e.QueryableSource = q;
            }

            //ShowGridDetails2Level.ExpandAll();
        }


        #endregion

    }
}