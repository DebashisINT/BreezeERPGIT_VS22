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
    public partial class GSTR_PurchaseRegister : System.Web.UI.Page
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
                RptHeading.Text = "Purchase GSTR Register";
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

                //Session["gridGstR1"] = null;
                //Session["gridGstRreurnpur1"] = null;
                Session["SI_ComponentData_Branch"] = null;
                Session["dt_GSTRRpt"] = null;
                //Session["GSTR_DebitNote"] = null;

                Session["IsPurchaseGSTRegFilter"] = null;
                Session["IsPurchaseRetGSTRegFilter"] = null;
                Session["IsVendDBNoteGSTRegFilter"] = null;
                Session["IsVendCRNoteGSTRegFilter"] = null;
                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                //   BindDropDownList();
                //ASPxFromDate.Text = dtFrom.ToString("dd-MM-yyyy");
                //ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");
                //Date_finyearwise(Convert.ToString(Session["LastFinYear"]));

                ASPxFromDate.Value = DateTime.Now;
                ASPxToDate.Value = DateTime.Now;
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                BranchpopulateGSTN();
                //Rev Subhra 18-12-2018   0017670
                Session["BranchNames"] = null;
                //End Rev
                dtBranchSelection = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='ReportsBranchSelection'");
                hdnBranchSelection.Value = dtBranchSelection.Rows[0][0].ToString();
            }
            else
            {

            }
        }

        public void Date_finyearwise(string Finyear)
        {
            CommonBL cmbl = new CommonBL();
            DataTable dtcmbl = new DataTable();

            dtcmbl = cmbl.GetDateFinancila(Finyear);
            if (dtcmbl.Rows.Count > 0)
            {
                ASPxFromDate.MaxDate = Convert.ToDateTime((dtcmbl.Rows[0]["FinYear_EndDate"]));
                ASPxFromDate.MinDate = Convert.ToDateTime((dtcmbl.Rows[0]["FinYear_StartDate"]));

                ASPxToDate.MaxDate = Convert.ToDateTime((dtcmbl.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((dtcmbl.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((dtcmbl.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((dtcmbl.Rows[0]["FinYear_StartDate"]));

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

        #region  Export
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

        public void bindexport(int Filter)
        {
            if (ASPxPageControl1.ActiveTabIndex == 0)
            {
                string filename = "GSTR Purchase Register";
                exporter.FileName = filename;
                string FileHeader = "";

                exporter.FileName = filename;

                BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

                FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true,true, true, true) + Environment.NewLine + "GSTR Purchase Register" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
                //Rev Subhra 18-12-2018   0017670
                FileHeader = ReplaceFirst(FileHeader, "\r\n", Convert.ToString(Session["BranchNames"]));
                //End of Rev
                exporter.PageHeader.Left = FileHeader;
                exporter.PageHeader.Font.Size = 10;
                exporter.PageHeader.Font.Name = "Tahoma";
                exporter.RenderBrick += exporter_RenderBrick;

                //exporter.PageFooter.Center = "[Page # of Pages #]";
                //exporter.PageFooter.Right = "[Date Printed]";
                exporter.Landscape = true;
                exporter.MaxColumnWidth = 100;
                exporter.GridViewID = "ShowGrid";
            }

            else if (ASPxPageControl1.ActiveTabIndex == 1)
            
            {
                string filename = "GSTR Purchase Return Register";
                exporter.FileName = filename;
                string FileHeader = "";

                exporter.FileName = filename;

                BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

                FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true,true, true, true) + Environment.NewLine + "GSTR Purchase Return Register" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
                //Rev Subhra 18-12-2018   0017670
                FileHeader = ReplaceFirst(FileHeader, "\r\n", Convert.ToString(Session["BranchNames"]));
                //End of Rev
                exporter.PageHeader.Left = FileHeader;
                exporter.PageHeader.Font.Size = 10;
                exporter.PageHeader.Font.Name = "Tahoma";
                exporter.RenderBrick += exporter_RenderBrick;

                exporter.PageFooter.Center = "[Page # of Pages #]";
                exporter.PageFooter.Right = "[Date Printed]";
                exporter.Landscape = true;
                exporter.MaxColumnWidth = 100;
                exporter.GridViewID = "ShowGrid2";

            }
            else if (ASPxPageControl1.ActiveTabIndex == 2)
            {
                string filename = "GSTR Purchase Debit Note";
                exporter.FileName = filename;
                string FileHeader = "";

                exporter.FileName = filename;

                BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

                FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true,true, true, true) + Environment.NewLine + "GSTR Purchase Debit Note" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
                //Rev Subhra 18-12-2018   0017670
                FileHeader = ReplaceFirst(FileHeader, "\r\n", Convert.ToString(Session["BranchNames"]));
                //End of Rev
                exporter.PageHeader.Left = FileHeader;
                exporter.PageHeader.Font.Size = 10;
                exporter.PageHeader.Font.Name = "Tahoma";
                exporter.RenderBrick += exporter_RenderBrick;

                exporter.PageFooter.Center = "[Page # of Pages #]";
                exporter.PageFooter.Right = "[Date Printed]";
                exporter.Landscape = true;
                exporter.MaxColumnWidth = 100;
                exporter.GridViewID = "grid_debitNote";
            }

            else if (ASPxPageControl1.ActiveTabIndex == 3)
            {
                string filename = "GSTR Purchase Credit Note";
                exporter.FileName = filename;
                string FileHeader = "";

                exporter.FileName = filename;

                BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

                FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true,true, true, true) + Environment.NewLine + "GSTR Purchase Credit Note" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
                //Rev Subhra 18-12-2018   0017670
                FileHeader = ReplaceFirst(FileHeader, "\r\n", Convert.ToString(Session["BranchNames"]));
                //End of Rev
                exporter.PageHeader.Left = FileHeader;
                exporter.PageHeader.Font.Size = 10;
                exporter.PageHeader.Font.Name = "Tahoma";
                exporter.RenderBrick += exporter_RenderBrick;

                exporter.PageFooter.Center = "[Page # of Pages #]";
                exporter.PageFooter.Right = "[Date Printed]";
                exporter.Landscape = true;
                exporter.MaxColumnWidth = 100;
                exporter.GridViewID = "grid_creditNote";
            }

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

        #endregion


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


        #region   ########## Purchase ############

        protected void Showgrid_Datarepared(object sender, EventArgs e)
        {
            ASPxGridView grid = (ASPxGridView)sender;
        }
        protected void gridpur_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.Caption == "Sl")
            {
                e.DisplayText = (e.VisibleRowIndex + 1).ToString();
            }
        }

        protected void CallbackPanelPurchase_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

            Session.Remove("dt_GSTRRpt");

            ShowGrid.JSProperties["cpSave"] = null;
            string returnPara = Convert.ToString(e.Parameter);

            string IsPurchaseGSTRegFilter = Convert.ToString(hfIsPurchaseGSTRegFilter.Value);
            Session["IsPurchaseGSTRegFilter"] = IsPurchaseGSTRegFilter;
            

            string WhichCall = returnPara.Split('~')[0];
            if (WhichCall == "ListData")
            {
                string WhichCall2 = returnPara.Split('~')[1];
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);


                string BRANCH_ID = "";
                string QuoComponent2 = "";
                List<object> QuoList2 = lookup_branch.GridView.GetSelectedFieldValues("branch_id");
                foreach (object Quo2 in QuoList2)
                {
                    QuoComponent2 += "," + Quo2;
                }

                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }

                BRANCH_ID = QuoComponent2.TrimStart(',');



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
                CallbackPanelPurchase.JSProperties["cpBranchNames"] = Convert.ToString(Session["BranchNames"]);

                //End of Rev


                // GetSalesRegisterdata(WhichCall2);

                Task PopulateStockTrialDataTask = new Task(() => GetPurchaseRegisterdata(WhichCall2, BRANCH_ID));
                PopulateStockTrialDataTask.RunSynchronously();
            }
        }

        //protected void grid_DataBinding(object sender, EventArgs e)
        //{
        //    if (Session["gridGstR1"] != null)
        //    {
        //        ShowGrid.DataSource = (DataTable)Session["gridGstR1"];

        //    }

        //}

        public void GetPurchaseRegisterdata(string Gstn, string Brnch)
        {
            try
            {
                string DriverName = string.Empty;
                string PhoneNo = string.Empty;
                string VehicleNo = string.Empty;
                DataTable dttab = new DataTable();

                DateTime dtFrom;
                DateTime dtTo;


                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                string TODATE = dtTo.ToString("yyyy-MM-dd");


                dttab = GetPurchaseRegisterdataData(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]).Trim(), "purchase", Gstn, FROMDATE, TODATE, Brnch);


                //if (dttab.Rows.Count > 0)
                //{
                //    Session["gridGstR1"] = dttab;
                //    ShowGrid.DataSource = dttab;
                //    ShowGrid.DataBind();
                //}
                //else
                //{
                //    Session["gridGstR1"] = null;
                //    ShowGrid.DataSource = null;
                //    ShowGrid.DataBind();

                //}
            }
            catch (Exception ex)
            {
            }
        }
        public DataTable GetPurchaseRegisterdataData(string Company, string Finyear, string Action, string Gstn, string FROMDATE, string TODATE, string Brnch)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("GST_Salepurchase");
            proc.AddPara("@COMPANYID", Company);
            proc.AddPara("@FINYEAR", Finyear);
            proc.AddPara("@Acton", Action);
            proc.AddPara("@FROMDATE", FROMDATE);
            proc.AddPara("@TODATE", TODATE);
            proc.AddPara("@GSTIN", Gstn);
            proc.AddPara("@P_BRANCH_ID", Brnch);
            proc.AddPara("@INVENTORY", ddlinventory.SelectedValue);

            proc.AddPara("@Partydate", chkparty.Checked);
            proc.AddPara("@Chkgrndate", chkgrndate.Checked);
            proc.AddPara("@ChkwithoutTax", chkwithouttax.Checked);

            proc.AddPara("@RCM", chkrcm.Checked == true ? "1" : "");
            proc.AddPara("@ITC", chkitc.Checked == true ? "1" : "");
            proc.AddPara("@USERID", Convert.ToInt32(Session["userid"]));
            proc.AddPara("@DOCUMENT_TYPE", "PB");
            ds = proc.GetTable();
            return ds;
        }


        private int totalCount;
        private int totalCountrecp;

        private List<string> Number = new List<string>();

        protected void ASPxGridView1_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            ASPxSummaryItem item = e.Item as ASPxSummaryItem;

            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Start)
            {
                Number.Clear();

                totalCount = 0;
                totalCountrecp = 0;


            }
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
            {
                string val = Convert.ToString(e.FieldValue);
                if (!Number.Contains(val))
                {
                    totalCount++;
                    Number.Add(val);
                }
            }
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
            {
                if (e.Item == ShowGrid.TotalSummary["Number"])
                {
                    e.TotalValue = string.Format("Doc Count={0}", totalCount);
                }

            }



        }

        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {

            //if (e.Item == ShowGrid.TotalSummary["TAxAmt"])
            //{
            //    e.Text = string.Format("Total={0}", e.Value);

            //}
            //if (e.Item == ShowGrid.TotalSummary["Net"])
            //{
            //    e.Text = string.Format("Total={0}", e.Value);

            //}


            //if (e.Item == ShowGrid.TotalSummary["CGSTRate"])
            //{
            //    e.Text = string.Format("Total={0}", e.Value);

            //}

            //if (e.Item == ShowGrid.TotalSummary["SGSTRate"])
            //{
            //    e.Text = string.Format("Total={0}", e.Value);

            //}

            //if (e.Item == ShowGrid.TotalSummary["IGSTRate"])
            //{
            //    e.Text = string.Format("Total={0}", e.Value);

            //}

            //if (e.Item == ShowGrid.TotalSummary["Qty"])
            //{
            //    e.Text = string.Format("Total={0}", e.Value);

            //}


            if (e.Item == ShowGrid.TotalSummary["othertax"])
            {
                decimal s = Math.Round(Convert.ToDecimal(e.Value), 2);
                var formattedNumber = string.Format("{0,00}", s);
                e.Text = string.Format("Total={0}", formattedNumber);
            }
            else if (e.Item == ShowGrid.TotalSummary["Totalnet"])
            {
                decimal s = Math.Round(Convert.ToDecimal(e.Value), 2);
                e.Text = string.Format("Total={0}", s);
            }
            else
            {
                e.Text = string.Format("{0}", e.Value);
            }

        }

        #endregion


        #region   ########## Purchase Return ############


        protected void gridpur2_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.Caption == "Sl")
            {
                e.DisplayText = (e.VisibleRowIndex + 1).ToString();
            }
        }


        protected void CallbackPanelPurchaseRet_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

            Session.Remove("dt_GSTRRpt");

            ShowGrid.JSProperties["cpSave"] = null;
            string returnPara = Convert.ToString(e.Parameter);

            string IsPurchaseRetGSTRegFilter = Convert.ToString(hfIsPurchaseRetGSTRegFilter.Value);
            Session["IsPurchaseRetGSTRegFilter"] = IsPurchaseRetGSTRegFilter;

            string WhichCall = returnPara.Split('~')[0];
            if (WhichCall == "ListData")
            {
                string WhichCall2 = returnPara.Split('~')[1];
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);


                string BRANCH_ID = "";
                string QuoComponent2 = "";
                List<object> QuoList2 = lookup_branch.GridView.GetSelectedFieldValues("branch_id");
                foreach (object Quo2 in QuoList2)
                {
                    QuoComponent2 += "," + Quo2;
                }

                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }

                BRANCH_ID = QuoComponent2.TrimStart(',');


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
                CallbackPanelPurchaseRet.JSProperties["cpBranchNames"] = Convert.ToString(Session["BranchNames"]);

                //End of Rev

                //  GetPurchasereturnsRegisterdata(WhichCall2);


                Task PopulateStockTrialDataTask = new Task(() => GetPurchasereturnsRegisterdata(WhichCall2, BRANCH_ID));
                PopulateStockTrialDataTask.RunSynchronously();
            }
        }


        //protected void grid2_DataBinding(object sender, EventArgs e)
        //{
        //    if (Session["gridGstRreurnpur1"] != null)
        //    {
        //        ShowGrid2.DataSource = (DataTable)Session["gridGstRreurnpur1"];

        //    }

        //}

        public void GetPurchasereturnsRegisterdata(string Gstn, string Branch)
        {
            try
            {
                string DriverName = string.Empty;
                string PhoneNo = string.Empty;
                string VehicleNo = string.Empty;
                DataTable dttab = new DataTable();

                DateTime dtFrom;
                DateTime dtTo;


                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                string TODATE = dtTo.ToString("yyyy-MM-dd");


                dttab = GetBranchGestnreturn(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]).Trim(), "purchasereturn", Gstn, FROMDATE, TODATE, Branch);


                //if (dttab.Rows.Count > 0)
                //{
                //    Session["gridGstRreurnpur1"] = dttab;
                //    ShowGrid2.DataSource = dttab;
                //    ShowGrid2.DataBind();
                //}
                //else
                //{
                //    Session["gridGstRreurnpur1"] = null;
                //    ShowGrid2.DataSource = null;
                //    ShowGrid2.DataBind();

                //}
            }
            catch (Exception ex)
            {
            }
        }
        public DataTable GetBranchGestnreturn(string Company, string Finyear, string Action, string Gstn, string FROMDATE, string TODATE, string Branch)
        {

            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("GST_Salepurchase");
            proc.AddPara("@COMPANYID", Company);
            proc.AddPara("@FINYEAR", Finyear);
            proc.AddPara("@Acton", Action);
            proc.AddPara("@FROMDATE", FROMDATE);
            proc.AddPara("@TODATE", TODATE);
            proc.AddPara("@GSTIN", Gstn);
            proc.AddPara("@P_BRANCH_ID", Branch);
            proc.AddPara("@ChkwithoutTax", chkwithouttax.Checked);
            proc.AddPara("@INVENTORY", ddlinventory.SelectedValue);
            proc.AddPara("@RCM", chkrcm.Checked == true ? "1" : "");
            proc.AddPara("@ITC", chkitc.Checked == true ? "1" : "");
            proc.AddPara("@USERID", Convert.ToInt32(Session["userid"]));
            proc.AddPara("@DOCUMENT_TYPE", "PR");
            ds = proc.GetTable();


            return ds;
        }


        private int totalCount2;
        private int totalCountrecp2;


        private List<string> Number2 = new List<string>();

        protected void ASPxGridView2_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            ASPxSummaryItem item = e.Item as ASPxSummaryItem;

            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Start)
            {
                Number2.Clear();

                totalCount2 = 0;
                totalCountrecp2 = 0;


            }
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
            {

                string val = Convert.ToString(e.FieldValue);
                if (!Number2.Contains(val))
                {
                    totalCount2++;
                    Number2.Add(val);
                }


            }

            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
            {

                if (e.Item == ShowGrid2.TotalSummary["Number"])
                {
                    e.TotalValue = string.Format("Doc Count={0}", totalCount2);
                }

            }



        }


        protected void ShowGrid2_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            //if (e.Item == ShowGrid.TotalSummary["TAxAmt"])
            //{
            //    e.Text = string.Format("Total={0}", e.Value);

            //}
            //if (e.Item == ShowGrid.TotalSummary["Net"])
            //{
            //    e.Text = string.Format("Total={0}", e.Value);

            //}


            //if (e.Item == ShowGrid.TotalSummary["CGSTRate"])
            //{
            //    e.Text = string.Format("Total={0}", e.Value);

            //}

            //if (e.Item == ShowGrid.TotalSummary["SGSTRate"])
            //{
            //    e.Text = string.Format("Total={0}", e.Value);

            //}

            //if (e.Item == ShowGrid.TotalSummary["IGSTRate"])
            //{
            //    e.Text = string.Format("Total={0}", e.Value);

            //}

            //if (e.Item == ShowGrid.TotalSummary["Qty"])
            //{
            //    e.Text = string.Format("Total={0}", e.Value);

            //}


            //if (e.Item == ShowGrid.TotalSummary["othertax"])
            //{
            //    e.Text = string.Format("Total={0}", e.Value);

            //}

            //if (e.Item == ShowGrid.TotalSummary["Totalnet"])
            //{
            //    e.Text = string.Format("Total={0}", e.Value);

            //}
            e.Text = string.Format("{0}", e.Value);

        }


        #endregion

        #region  ############# Debit  Note  #################

        protected void CallbackPanelVendDebitNote_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string returnPara = Convert.ToString(e.Parameter);
            //Session["GSTR_DebitNote"] = null;
            string WhichCall = returnPara.Split('~')[0];
            if (WhichCall == "ListData")
            {
                string WhichCall2 = returnPara.Split('~')[1];
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

                string IsVendDBNoteGSTRegFilter = Convert.ToString(hfIsVendDBNoteGSTRegFilter.Value);
                Session["IsVendDBNoteGSTRegFilter"] = IsVendDBNoteGSTRegFilter;


                string BRANCH_ID = "";
                string QuoComponent2 = "";
                List<object> QuoList2 = lookup_branch.GridView.GetSelectedFieldValues("branch_id");
                foreach (object Quo2 in QuoList2)
                {
                    QuoComponent2 += "," + Quo2;
                }

                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }

                BRANCH_ID = QuoComponent2.TrimStart(',');
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
                CallbackPanelVendDebitNote.JSProperties["cpBranchNames"] = Convert.ToString(Session["BranchNames"]);

                //End of Rev

                Task PopulateStockTrialDataTask = new Task(() => GetDebitNote(WhichCall2, BRANCH_ID));
                PopulateStockTrialDataTask.RunSynchronously();

            }
        }
        //protected void grid_debitNote_DataBinding(object sender, EventArgs e)
        //{
        //    if (Session["GSTR_DebitNote"] != null)
        //    {
        //        grid_debitNote.DataSource = (DataTable)Session["GSTR_DebitNote"];
        //    }
        //}
        protected void grid_debitNote_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            //if (e.Item == grid_debitNote.TotalSummary["TaxableAmount"])
            //{
            //    e.Text = string.Format("Total={0}", e.Value);
            //}
            //else if (e.Item == grid_debitNote.TotalSummary["CGSTAmount"])
            //{
            //    e.Text = string.Format("Total={0}", e.Value);
            //}
            //else if (e.Item == grid_debitNote.TotalSummary["SGSTAmount"])
            //{
            //    e.Text = string.Format("Total={0}", e.Value);
            //}
            //else if (e.Item == grid_debitNote.TotalSummary["IGSTAmount"])
            //{
            //    e.Text = string.Format("Total={0}", e.Value);
            //}
            //else if (e.Item == grid_debitNote.TotalSummary["UTGSTAmount"])
            //{
            //    e.Text = string.Format("Total={0}", e.Value);
            //}
            //else if (e.Item == grid_debitNote.TotalSummary["OtherAmount"])
            //{
            //    e.Text = string.Format("Total={0}", e.Value);
            //}
            //else if (e.Item == grid_debitNote.TotalSummary["NetAmount"])
            //{
            //    e.Text = string.Format("Total={0}", e.Value);
            //}

                  e.Text = string.Format("{0}", e.Value);
        }
        public void GetDebitNote(string Gstn, string Branch)
        {
            try
            {
                string DriverName = string.Empty;
                string PhoneNo = string.Empty;
                string VehicleNo = string.Empty;
                DataTable dttab = new DataTable();

                DateTime dtFrom;
                DateTime dtTo;

                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                string TODATE = dtTo.ToString("yyyy-MM-dd");
                string doctype = "VDR";

                dttab = GetGstrDebitNote(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]).Trim(), "debitnotevendor", Gstn, FROMDATE, TODATE, Branch, doctype);

                //if (dttab.Rows.Count > 0)
                //{
                //    Session["GSTR_DebitNote"] = dttab;
                //    grid_debitNote.DataSource = dttab;
                //    grid_debitNote.DataBind();
                //}
                //else
                //{
                //    Session["GSTR_DebitNote"] = null;
                //    grid_debitNote.DataSource = null;
                //    grid_debitNote.DataBind();
                //}
            }
            catch (Exception ex)
            {
            }
        }
        public DataTable GetGstrDebitNote(string Company, string Finyear, string Action, string Gstn, string FROMDATE, string TODATE, string Branch,string doctype)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("GST_Salepurchase");
            proc.AddPara("@COMPANYID", Company);
            proc.AddPara("@FINYEAR", Finyear);
            proc.AddPara("@Acton", Action);
            proc.AddPara("@FROMDATE", FROMDATE);
            proc.AddPara("@TODATE", TODATE);
            proc.AddPara("@GSTIN", Gstn);
            proc.AddPara("@P_BRANCH_ID", Branch);
            proc.AddPara("@ChkwithoutTax", chkwithouttax.Checked);
            proc.AddPara("@INVENTORY", ddlinventory.SelectedValue);
            proc.AddPara("@USERID", Convert.ToInt32(Session["userid"]));
            proc.AddPara("@DOCUMENT_TYPE", doctype);
            ds = proc.GetTable();
            return ds;
        }

        #endregion

        #region  ############# Cerdit  Note  #################

        protected void CallbackPanelVendCreditNote_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string returnPara = Convert.ToString(e.Parameter);
            //Session["GSTR_DebitNote"] = null;
            string WhichCall = returnPara.Split('~')[0];
            if (WhichCall == "ListData")
            {
                string WhichCall2 = returnPara.Split('~')[1];
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

                string IsVendCRNoteGSTRegFilter = Convert.ToString(hfIsVendCRNoteGSTRegFilter.Value);
                Session["IsVendCRNoteGSTRegFilter"] = IsVendCRNoteGSTRegFilter;

                string BRANCH_ID = "";
                string QuoComponent2 = "";
                List<object> QuoList2 = lookup_branch.GridView.GetSelectedFieldValues("branch_id");
                foreach (object Quo2 in QuoList2)
                {
                    QuoComponent2 += "," + Quo2;
                }

                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }

                BRANCH_ID = QuoComponent2.TrimStart(',');

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
                CallbackPanelVendCreditNote.JSProperties["cpBranchNames"] = Convert.ToString(Session["BranchNames"]);

                //End of Rev


                Task PopulateStockTrialDataTask = new Task(() => GetCreditNote(WhichCall2, BRANCH_ID));
                PopulateStockTrialDataTask.RunSynchronously();

            }
        }
        //protected void grid_creditNote_DataBinding(object sender, EventArgs e)
        //{
        //    if (Session["GSTR_DebitNote"] != null)
        //    {
        //        grid_creditNote.DataSource = (DataTable)Session["GSTR_DebitNote"];
        //    }
        //}
        protected void grid_creditNote_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            //if (e.Item == grid_creditNote.TotalSummary["TaxableAmount"])
            //{
            //    e.Text = string.Format("Total={0}", e.Value);
            //}
            //else if (e.Item == grid_creditNote.TotalSummary["CGSTAmount"])
            //{
            //    e.Text = string.Format("Total={0}", e.Value);
            //}
            //else if (e.Item == grid_creditNote.TotalSummary["SGSTAmount"])
            //{
            //    e.Text = string.Format("Total={0}", e.Value);
            //}
            //else if (e.Item == grid_creditNote.TotalSummary["IGSTAmount"])
            //{
            //    e.Text = string.Format("Total={0}", e.Value);
            //}
            //else if (e.Item == grid_creditNote.TotalSummary["UTGSTAmount"])
            //{
            //    e.Text = string.Format("Total={0}", e.Value);
            //}
            //else if (e.Item == grid_creditNote.TotalSummary["OtherAmount"])
            //{
            //    e.Text = string.Format("Total={0}", e.Value);
            //}
            //else if (e.Item == grid_creditNote.TotalSummary["NetAmount"])
            //{
            //    e.Text = string.Format("Total={0}", e.Value);
            //}

                e.Text = string.Format("{0}", e.Value);
        }
        public void GetCreditNote(string Gstn, string Branch)
        {
            try
            {
                string DriverName = string.Empty;
                string PhoneNo = string.Empty;
                string VehicleNo = string.Empty;
                DataTable dttab = new DataTable();

                DateTime dtFrom;
                DateTime dtTo;

                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                string TODATE = dtTo.ToString("yyyy-MM-dd");
                string doctype = "VCR";

                dttab = GetGstrDebitNote(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]).Trim(), "creditnotevendor", Gstn, FROMDATE, TODATE, Branch, doctype);

                //if (dttab.Rows.Count > 0)
                //{
                //    Session["GSTR_DebitNote"] = dttab;
                //    grid_creditNote.DataSource = dttab;
                //    grid_creditNote.DataBind();
                //}
                //else
                //{
                //    Session["GSTR_DebitNote"] = null;
                //    grid_creditNote.DataSource = null;
                //    grid_creditNote.DataBind();
                //}
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

                if (Hoid != "0")
                {
                    DataSet ds = new DataSet();
                    //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                    SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                    SqlCommand cmd = new SqlCommand("Getbranchlist_Gsitnwise", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Changes By Subhra on 19-04-2018
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
                lookup_branch.DataSource = (DataTable)Session["SI_ComponentData_Branch"];
            }
        }
        #endregion

        #region LinQ
        protected void GeneratePurchaseEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsPurchaseGSTRegFilter"]) == "Y")
            {
                var q = from d in dc.SALESPURCHASEGSTREGISTER_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.DOCUMENT_TYPE) == "PB"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.SALESPURCHASEGSTREGISTER_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            //ShowGrid.ExpandAll();
        }

        protected void GeneratePurchaseReturnEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsPurchaseRetGSTRegFilter"]) == "Y")
            {
                var q = from d in dc.SALESPURCHASEGSTREGISTER_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.DOCUMENT_TYPE) == "PR"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.SALESPURCHASEGSTREGISTER_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
        }

        protected void GenerateVendDebitNoteEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsVendDBNoteGSTRegFilter"]) == "Y")
            {
                var q = from d in dc.SALESPURCHASEGSTREGISTER_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.DOCUMENT_TYPE) == "VDR"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.SALESPURCHASEGSTREGISTER_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
        }

        protected void GenerateVendCreditNoteEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsVendCRNoteGSTRegFilter"]) == "Y")
            {
                var q = from d in dc.SALESPURCHASEGSTREGISTER_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.DOCUMENT_TYPE) == "VCR"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.SALESPURCHASEGSTREGISTER_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
        }

        #endregion
    }
}