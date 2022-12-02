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
using DataAccessLayer;


namespace Reports.Reports.GridReports
{
    public partial class InventoryControlWithStockValuation : System.Web.UI.Page
    {
        //DataTable DTIndustry = new DataTable();
        DateTime dtFrom;
        DateTime dtTo;
        //TotalvaluationClass objvaluation = new TotalvaluationClass();
        //string data = "";
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();
        protected void Page_Load(object sender, EventArgs e)
        {
            //rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/Reports/master/InventoryControlWithStockValuation.aspx");
            rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/InventoryControlWithStockValuation.aspx");
            DateTime dtFrom;
            DateTime dtTo;
            if (!IsPostBack)
            {
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Inventory Control";
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

                Session["SI_ComponentData"] = null;
                Session["IsInventoryFilter"] = null;
                Session["IsInventoryDetFilter"] = null;

                Session["IsInventoryAvgMaxlvlFilter"] = null;
                Session["IsInventoryBelowMaxlvlFilter"] = null;
                Session["IsInventoryBlwMinlvlFilter"] = null;
                //Rev Subhra  0019275  21-12-2018
                Session["IsInventoryReorderlvlFilter"] = null;
                //end of Subhra
                Session["exportval1"] = null;
                Session["exportAvgMaxlvlval"]= null;
                Session["exportBelowMaxlvlval"]= null;
                Session["exportBelowMinlvlval"] = null;
                Session["exportReorderlvlval"] = null;


                Session["SI_ComponentData_Branch"] = null;
                Session["Quantity"] = 0;
                Session["Minlvl"]=0;
                Session["Maxlvl"] = 0;
                //Rev Subhra  21-12-2018
                Session["Reorderlvl"] = 0;
                //End of Rev
                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                ASPxFromDate.Text = dtFrom.ToString("dd-MM-yyyy");
                ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");
                ASPxFromDate.Value = DateTime.Now;
                ASPxToDate.Value = DateTime.Now;

                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                string TODATE = dtTo.ToString("yyyy-MM-dd");
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                BranchHoOffice();
                //Rev Subhra 24-12-2018   0017670
                Session["BranchNames"] = null;
                //End Rev

                //Rev Subhra 21-01-2019  
                if(chkCmpStkWthPrvsMnths.Checked==false)
                {
                    ShowGrid.Columns[8].Visible = false;
                    ShowGrid.Columns[9].Visible = false;
                }
                //End of Rev Subhra
            }
            else
            {
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);
            }
        }
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
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

        #region Export Valuation Summary
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
            string filename = "InventoryControl";
            exporter.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Inventory Control" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
            //Rev Subhra 24-12-2018   0017670
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
        //Rev Subhra 11-12-2018   0017670
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

        #region Export Valuation Details
        public void cmbExport1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(ddldetails.SelectedItem.Value));
            if (Filter != 0)
            {
                BranchHoOffice();
                if (Session["exportval1"] == null)
                {
                    Session["exportval1"] = Filter;
                    bindexport1(Filter);
                }
                else if (Convert.ToInt32(Session["exportval1"]) != Filter)
                {
                    Session["exportval1"] = Filter;
                    bindexport1(Filter);
                }
            }
        }

        public void bindexport1(int Filter)
        {
            string filename = "InventoryControlDetails";
            exporter.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();
            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Inventory Control-Detailed" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");

            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";
            exporter.Landscape = true;
            exporter.MaxColumnWidth = 100;
            exporter.GridViewID = "grivaluation";
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
        #endregion


        #region =======================Valuation Summary =========================
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string IsInventoryFilter = Convert.ToString(hfIsInventoryFilter.Value);
            Session["IsInventoryFilter"] = IsInventoryFilter;

            string IsInventoryAvgMaxlvlFilter = Convert.ToString(hfIsInventoryAvgMaxlvlFilter.Value);
            Session["IsInventoryAvgMaxlvlFilter"] = IsInventoryAvgMaxlvlFilter;

            string IsInventoryBelowMaxlvlFilter = Convert.ToString(hfIsInventoryBelowMaxlvlFilter.Value);
            Session["IsInventoryBelowMaxlvlFilter"] = IsInventoryBelowMaxlvlFilter;

            string IsInventoryBlwMinlvlFilter = Convert.ToString(hfIsInventoryBlwMinlvlFilter.Value);
            Session["IsInventoryBlwMinlvlFilter"] = IsInventoryBlwMinlvlFilter;
            //Rev Subhra 0019275  21-12-2018
            string IsInventoryReorderlvlFilter = Convert.ToString(hfIsInventoryReorderlvlFilter.Value);
            Session["IsInventoryReorderlvlFilter"] = IsInventoryReorderlvlFilter;
            //End Rev
            string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);

            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");

            string BRANCH_ID = "";

            string QuoComponent2 = "";
            List<object> QuoList2 = lookup_branch.GridView.GetSelectedFieldValues("ID");
            foreach (object Quo2 in QuoList2)
            {
                QuoComponent2 += "," + Quo2;
            }
            BRANCH_ID = QuoComponent2.TrimStart(',');

            string Product = "";
            Product = hdncWiseProductId.Value;


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

            Task PopulateStockTrialDataTask = new Task(() => GetInventoryControldata(FROMDATE, TODATE, BRANCH_ID, Product));
            PopulateStockTrialDataTask.RunSynchronously();
        }

        public void GetInventoryControldata(string FROMDATE, string TODATE, string BRANCH_ID, string ProductIds)
        {
            try
            {
                string strClassList = "", strBrandList = "";
                strClassList = hdnClassId.Value;
                strBrandList = hdnBranndId.Value;
                int is_comparestock = 0;
                if(chkCmpStkWthPrvsMnths.Checked==true)
                {
                    is_comparestock = 1;
                }
                else
                {
                    is_comparestock = 0;
                }
                DataTable ds = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("PRC_INVENTORYCONTROLWITHSTOCKVALUATION_REPORT");
                proc.AddPara("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                proc.AddPara("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                proc.AddPara("@FROMDATE", FROMDATE);
                proc.AddPara("@TODATE", TODATE);
                proc.AddPara("@BRANCHID", BRANCH_ID);
                proc.AddPara("@PRODUCT_ID", ProductIds);
                proc.AddPara("@VAL_TYPE", "F");
                proc.AddPara("@GETTYPE", "Summary");
                proc.AddPara("@Class", strClassList);
                proc.AddPara("@Brand", strBrandList);
                //Rev Subhra 22-01-2019
                proc.AddPara("@MONTHCOMPARE", is_comparestock);
                //End of Rev
                proc.AddPara("@CONSLANDEDCOST", (chkConsLandCost.Checked) ? "1" : "0");
                proc.AddPara("@CONSOVERHEADCOST", (chkConsOverHeadCost.Checked) ? "1" : "0");
                proc.AddPara("@USERID", Convert.ToInt32(Session["userid"]));

                ds = proc.GetTable();

            }
            catch (Exception ex)
            {
            }
        }

        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }
        #endregion

        public void Date_finyearwise(string Finyear)
        {
            CommonBL stkval = new CommonBL();
            DataTable dtstkval = new DataTable();

            dtstkval = stkval.GetDateFinancila(Finyear);
            if (dtstkval.Rows.Count > 0)
            {
                ASPxFromDate.MaxDate = Convert.ToDateTime((dtstkval.Rows[0]["FinYear_EndDate"]));
                ASPxFromDate.MinDate = Convert.ToDateTime((dtstkval.Rows[0]["FinYear_StartDate"]));

                ASPxToDate.MaxDate = Convert.ToDateTime((dtstkval.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((dtstkval.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((dtstkval.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((dtstkval.Rows[0]["FinYear_StartDate"]));

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

        #region =====================Valuation Details===========================
        protected void CallbackPanelDetail_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string IsInventoryDetFilter = Convert.ToString(hfIsInventoryDetFilter.Value);
            Session["IsInventoryDetFilter"] = IsInventoryDetFilter;

            string returnPara = Convert.ToString(e.Parameter);
            string WhichCall = returnPara.Split('~')[0];

            if (WhichCall == "BndPopupgrid")
            {
                string branch = returnPara.Split('~')[1];
                string prodId = returnPara.Split('~')[2];
                Getvaluation(prodId, branch);
            }

        }

        public void Getvaluation(string ProductIds, string BRANCH_ID)
        {
            try
            {
                string strClassList = "", strBrandList = "";

                strClassList = hdnClassId.Value;
                strBrandList = hdnBranndId.Value;

                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                string TODATE = dtTo.ToString("yyyy-MM-dd");

                DataTable ds = new DataTable();
                ProcedureExecute proc = new ProcedureExecute("PRC_INVENTORYCONTROLWITHSTOCKVALUATION_REPORT");
                proc.AddPara("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                proc.AddPara("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                proc.AddPara("@FROMDATE", FROMDATE);
                proc.AddPara("@TODATE", TODATE);
                proc.AddPara("@BRANCHID", BRANCH_ID);
                proc.AddPara("@PRODUCT_ID", ProductIds);
                proc.AddPara("@VAL_TYPE", "F");
                proc.AddPara("@GETTYPE", "Details");
                proc.AddPara("@Class", strClassList);
                proc.AddPara("@Brand", strBrandList);
                proc.AddPara("@CONSLANDEDCOST", (chkConsLandCost.Checked) ? "1" : "0");
                proc.AddPara("@CONSOVERHEADCOST", (chkConsOverHeadCost.Checked) ? "1" : "0");
                proc.AddPara("@USERID", Convert.ToInt32(Session["userid"]));

                ds = proc.GetTable();

            }
            catch (Exception ex)
            {
            }
        }

        protected void ShowGrid1_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
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
                if (Hoid != "All")
                {
                    ComponentTable = GetBranch(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Hoid);
                }
                else
                {
                    ComponentTable = oDBEngine.GetDataTable("select * from (select branch_id as ID,branch_description,branch_code from tbl_master_branch a where a.branch_id=1  union all select branch_id as ID,branch_description,branch_code from tbl_master_branch b where b.branch_parentId=1) a order by branch_description");
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

        #region LinQ
        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SLNO";
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsInventoryFilter"]) == "Y")
            {
                var q = from d in dc.INVENTORYCONTROLWITHSTOCKVALUATION_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.REPORTTYPE) == "Summary"
                        orderby d.SLNO
                        select d;
                e.QueryableSource = q;

                //var ContMaxqty = from d in dc.INVENTORYCONTROLWITHSTOCKVALUATION_REPORTs
                //                 where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.REPORTTYPE) == "Summary" && d.IN_QTY_OP > d.MAXLEVEL
                //                 orderby d.SLNO
                //                 select d.PRODUCTS_ID;

                System.Nullable<Decimal> totalvalue = (from d in dc.INVENTORYCONTROLWITHSTOCKVALUATION_REPORTs
                                         where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.REPORTTYPE) == "Summary"
                                         orderby d.SLNO
                                         select d.IN_TOTAL_OP).Sum();


                var abovemaxlvl = from d in dc.INVENTORYCONTROLWITHSTOCKVALUATION_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.REPORTTYPE) == "Summary" && d.IN_QTY_OP > d.MAXLEVEL 
                        orderby d.SLNO
                        select d;


                var aboveminbelowmaxlvl = from d in dc.INVENTORYCONTROLWITHSTOCKVALUATION_REPORTs
                                  where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.REPORTTYPE) == "Summary" && d.IN_QTY_OP < d.MAXLEVEL && d.IN_QTY_OP > d.MINLEVEL
                                  orderby d.SLNO
                                  select d;

                var belowminlvl = from d in dc.INVENTORYCONTROLWITHSTOCKVALUATION_REPORTs
                                          where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.REPORTTYPE) == "Summary" && d.IN_QTY_OP < d.MINLEVEL
                                          orderby d.SLNO
                                          select d;
                //Rev Subhra  0019275  21-12-2018
                //Rev Debashis 0020569  25-07-2019
                //var reorderlvl = from d in dc.INVENTORYCONTROLWITHSTOCKVALUATION_REPORTs
                //                  where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.REPORTTYPE) == "Summary" && d.IN_QTY_OP == d.REORDERLEVEL
                //                  orderby d.SLNO
                //                  select d;
                var reorderlvl = from d in dc.INVENTORYCONTROLWITHSTOCKVALUATION_REPORTs
                                 where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.REPORTTYPE) == "Summary" && d.IN_QTY_OP <= d.REORDERLEVEL && d.IN_QTY_OP > d.MINLEVEL
                                 orderby d.SLNO
                                 select d;
                //End of Rev Debashis 0020569  25-07-2019
                //End of Rev 
                ShowGrid.JSProperties["cpCountMaxQty"] = q.Count();
                ShowGrid.JSProperties["cpTotalValue"] = totalvalue;
                ShowGrid.JSProperties["cpCountAboveMaxlvl"]=abovemaxlvl.Count();
                ShowGrid.JSProperties["cpCountAboveMinBelowMaxlvl"]=aboveminbelowmaxlvl.Count();
                ShowGrid.JSProperties["cpCountBelowMinQty"] = belowminlvl.Count();
                ShowGrid.JSProperties["cpBaseCurrency"] = Session["LocalCurrency"].ToString().Split('~')[1].Trim();
                ShowGrid.JSProperties["cpBaseCurrencySymboll"] = Session["LocalCurrency"].ToString().Split('~')[2].Trim();
                //Rev Subhra  0019275  21-12-2018
                ShowGrid.JSProperties["cpReorderLevel"] = reorderlvl.Count();
                //End of Rev 
                //ShowGrid.JSProperties["cpBaseCurrency"] = "USD";
                //ShowGrid.JSProperties["cpBaseCurrencySymboll"] = "$";

                //Rev 21-01-2019
                if (chkCmpStkWthPrvsMnths.Checked == true)
                {

                    ShowGrid.Columns[8].Visible = true;
                    ShowGrid.Columns[9].Visible = true;


                    ShowGrid.Columns[8].Caption = "Stock in hand As on " + ASPxToDate.Date.AddMonths(-2).ToString("dd-MM-yyyy");
                    ShowGrid.Columns[9].Caption = "Stock in hand As on " + ASPxToDate.Date.AddMonths(-1).ToString("dd-MM-yyyy");
                    ShowGrid.Columns[10].Caption = "Stock in hand As on " + ASPxToDate.Date.ToString("dd-MM-yyyy");

                    ShowGrid.Columns[8].HeaderStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;
                    ShowGrid.Columns[9].HeaderStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;
                    ShowGrid.Columns[10].HeaderStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;

                }
                else
                {
                    ShowGrid.Columns[8].Visible = false;
                    ShowGrid.Columns[9].Visible = false;

                }

                //End of Rev 21-01-2019

            }
            else
            {
                var q = from d in dc.INVENTORYCONTROLWITHSTOCKVALUATION_REPORTs
                        where Convert.ToString(d.SLNO) == "0"
                        orderby d.SLNO
                        select d;
                e.QueryableSource = q;
            }


        }
        protected void GenerateEntityServerDetailsModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SLNO";
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsInventoryDetFilter"]) == "Y")
            {
                var q = from d in dc.INVENTORYCONTROLWITHSTOCKVALUATION_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.REPORTTYPE) == "Details"
                        orderby d.SLNO
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.INVENTORYCONTROLWITHSTOCKVALUATION_REPORTs
                        where Convert.ToString(d.SLNO) == "0"
                        orderby d.SLNO
                        select d;
                e.QueryableSource = q;
            }

          
        }
        #endregion

        protected void ShowGrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
           
            decimal min_qty = 0;
            decimal max_qty = 0;

            if (e.DataColumn.FieldName == "MINLEVEL")
            {
                //min_qty = Convert.ToDecimal(e.CellValue);
                Session["Minlvl"] = e.CellValue;
            }
            if (e.DataColumn.FieldName == "MAXLEVEL")
            {
                //max_qty = Convert.ToDecimal(e.CellValue);
                Session["Maxlvl"] = e.CellValue;
            }

            //Rev Subhra  21-12-2018
            if (e.DataColumn.FieldName == "REORDERLEVEL")
            {
                Session["Reorderlvl"] = e.CellValue;
            }
            //End of Rev

            if (e.DataColumn.FieldName == "IN_QTY_OP")
            {
                if (Session["Maxlvl"]!=null)
                {
                    if (Convert.ToDecimal(e.CellValue) > Convert.ToDecimal(Session["Maxlvl"]))
                    {
                        e.Cell.ForeColor = Color.FromArgb(0xd9, 0xc0, 0x15);
                        
                    }
                }

                if (Session["Maxlvl"] != null)
                {
                    if (Convert.ToDecimal(e.CellValue) < Convert.ToDecimal(Session["Maxlvl"]))
                    {
                        e.Cell.ForeColor = Color.LightGreen;
                    }
                }

                if (Session["Minlvl"] != null)
                {
                    if (Convert.ToDecimal(e.CellValue) < Convert.ToDecimal(Session["Minlvl"]))
                    {
                        e.Cell.ForeColor = Color.Red;
                    }
                }

                //Rev Subhra  21-12-2018
                if (Session["Reorderlvl"] != null)
                {
                    if (Convert.ToDecimal(e.CellValue) == Convert.ToDecimal(Session["Reorderlvl"]))
                    {
                        e.Cell.ForeColor = Color.FromArgb(0x33, 0x66, 0xFF);
                    }
                }
                //End of Rev
            }

        }

        protected void drdMaxLvl_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Rev Subhra 13-12-2018
            //Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            try
            {
                Int32 Filter = int.Parse(Convert.ToString(drdMaxLvl.SelectedItem.Value));
                //End of Rev
                if (Filter != 0)
                {
                    BranchHoOffice();
                    if (Session["exportAvgMaxlvlval"] == null)
                    {
                        Session["exportAvgMaxlvlval"] = Filter;
                        bindexportAvgMaxLvl(Filter);
                    }
                    else if (Convert.ToInt32(Session["exportAvgMaxlvlval"]) != Filter)
                    {
                        Session["exportAvgMaxlvlval"] = Filter;
                        bindexportAvgMaxLvl(Filter);
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }
        public void bindexportAvgMaxLvl(int Filter)
        {
            string filename = "InventoryAvgMaxLvlControlDetails";
            exporter.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();
            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Inventory Control-Above Max Level" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");

            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";
            exporter.Landscape = true;
            exporter.MaxColumnWidth = 100;
            exporter.GridViewID = "ShowAMaxLvl";
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
        protected void GenerateAvgMaxLvlEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SLNO";
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsInventoryAvgMaxlvlFilter"]) == "Y")
            {
                var q = from d in dc.INVENTORYCONTROLWITHSTOCKVALUATION_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.REPORTTYPE) == "Summary" && d.IN_QTY_OP > d.MAXLEVEL
                        orderby d.SLNO
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.INVENTORYCONTROLWITHSTOCKVALUATION_REPORTs
                        where Convert.ToString(d.SLNO) == "0"
                        orderby d.SLNO
                        select d;
                e.QueryableSource = q;

            }


        
        }
        protected void ShowAMaxLvl_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }

        protected void drdBelowMaxLvl_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Rev Subhra 13-12-2018
            //Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            Int32 Filter = int.Parse(Convert.ToString(drdBelowMaxLvl.SelectedItem.Value));
            //End of Rev
            if (Filter != 0)
            {
                BranchHoOffice();
                if (Session["exportBelowMaxlvlval"] == null)
                {
                    Session["exportBelowMaxlvlval"] = Filter;
                    bindexportBelowMaxLvl(Filter);
                }
                else if (Convert.ToInt32(Session["exportBelowMaxlvlval"]) != Filter)
                {
                    Session["exportBelowMaxlvlval"] = Filter;
                    bindexportBelowMaxLvl(Filter);
                }
            }
        }
        public void bindexportBelowMaxLvl(int Filter)
        {
            string filename = "InventoryBelowMaxLvlControlDetails";
            exporter.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();
            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Inventory Control-Below Max Above Min Level" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");

            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";
            exporter.Landscape = true;
            exporter.MaxColumnWidth = 100;
            exporter.GridViewID = "ShowBelowMaxLvl";
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
        protected void ShowBelowMaxLvl_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }
        protected void GenerateBelowMaxLvlEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SLNO";
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsInventoryBelowMaxlvlFilter"]) == "Y")
            {
                var q = from d in dc.INVENTORYCONTROLWITHSTOCKVALUATION_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.REPORTTYPE) == "Summary" && d.IN_QTY_OP < d.MAXLEVEL && d.IN_QTY_OP > d.MINLEVEL
                        orderby d.SLNO
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.INVENTORYCONTROLWITHSTOCKVALUATION_REPORTs
                        where Convert.ToString(d.SLNO) == "0"
                        orderby d.SLNO
                        select d;
                e.QueryableSource = q;
            }
        }

        protected void ShowBelowMinLvl_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }
        protected void drdBelowMinLvl_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Rev Subhra 13-12-2018
            //Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            Int32 Filter = int.Parse(Convert.ToString(drdBelowMinLvl.SelectedItem.Value));
            //End of Rev
            if (Filter != 0)
            {
                BranchHoOffice();
                if (Session["exportBelowMinlvlval"] == null)
                {
                    Session["exportBelowMinlvlval"] = Filter;
                    bindexportBelowMinLvl(Filter);
                }
                else if (Convert.ToInt32(Session["exportBelowMinlvlval"]) != Filter)
                {
                    Session["exportBelowMinlvlval"] = Filter;
                    bindexportBelowMinLvl(Filter);
                }
            }
        }
        public void bindexportBelowMinLvl(int Filter)
        {
            string filename = "InventoryBelowMinLvlControlDetails";
            exporter.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();
            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Inventory Control-Below Min Level" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");

            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";
            exporter.Landscape = true;
            exporter.MaxColumnWidth = 100;
            exporter.GridViewID = "ShowBelowMinLvl";
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
        protected void GenerateBelowMinLvlEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SLNO";
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsInventoryBlwMinlvlFilter"]) == "Y")
            {
                var q = from d in dc.INVENTORYCONTROLWITHSTOCKVALUATION_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.REPORTTYPE) == "Summary" && d.IN_QTY_OP < d.MINLEVEL
                        orderby d.SLNO
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.INVENTORYCONTROLWITHSTOCKVALUATION_REPORTs
                        where Convert.ToString(d.SLNO) == "0"
                        orderby d.SLNO
                        select d;
                e.QueryableSource = q;
            }
        }

    //Rev Subhra  21-12-2018
        protected void drdReorderMaxLvl_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdReorderMaxLvl.SelectedItem.Value));
            //End of Rev
            if (Filter != 0)
            {
                BranchHoOffice();
                if (Session["exportReorderlvlval"] == null)
                {
                    Session["exportReorderlvlval"] = Filter;
                    bindexportReorderLvl(Filter);
                }
                else if (Convert.ToInt32(Session["exportReorderlvlval"]) != Filter)
                {
                    Session["exportReorderlvlval"] = Filter;
                    bindexportReorderLvl(Filter);
                }
            }
        }
        public void bindexportReorderLvl(int Filter)
        {
            string filename = "InventoryReorderLvlControlDetails";
            exporter.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();
            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Inventory Control-Reorder Level" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");

            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";
            exporter.Landscape = true;
            exporter.MaxColumnWidth = 100;
            exporter.GridViewID = "ShowReorderLvl";
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
        protected void GenerateReorderLvlEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SLNO";
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsInventoryReorderlvlFilter"]) == "Y")
            {
                //Rev Debashis 0020569 25-07-2019
                //var q = from d in dc.INVENTORYCONTROLWITHSTOCKVALUATION_REPORTs
                //        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.REPORTTYPE) == "Summary" && d.IN_QTY_OP == d.REORDERLEVEL
                //        orderby d.SLNO
                //        select d;
                //e.QueryableSource = q;
                var q = from d in dc.INVENTORYCONTROLWITHSTOCKVALUATION_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.REPORTTYPE) == "Summary" && d.IN_QTY_OP<=d.REORDERLEVEL && d.IN_QTY_OP > d.MINLEVEL
                        orderby d.SLNO
                        select d;
                e.QueryableSource = q;
                //End of Rev Debashis 0020569 25-07-2019
            }
            else
            {
                var q = from d in dc.INVENTORYCONTROLWITHSTOCKVALUATION_REPORTs
                        where Convert.ToString(d.SLNO) == "0"
                        orderby d.SLNO
                        select d;
                e.QueryableSource = q;
            }
        }

        protected void ShowReorderLvl_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }

        //End of Rev

      

    }
}