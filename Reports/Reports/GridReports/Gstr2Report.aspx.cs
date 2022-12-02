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
using DevExpress.XtraPrinting;
using DevExpress.Export;
using DataAccessLayer;
using System.Threading.Tasks;
using System.Drawing;


namespace Reports.Reports.GridReports
{

    public partial class Gstr2Report : System.Web.UI.Page
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
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "GSTR-2 All";
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

                Session["GstR2B2B"] = null;
                Session["GSTRRpt2B2BUR"]= null;
                Session["GSTRRpt2IMPS"]= null;
                Session["GSTRRpt2IMPG"]= null;
                Session["GSTRRpt2CDNR"]= null;
                Session["GSTRRpt2CDNUR"]= null;
                Session["GSTRRpt2EXEMP"] = null;
                Session["GSTRRpt2ITCR"] = null;
                Session["GSTRRpt2HSNSUM"] = null;

                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                //   BindDropDownList();
                //ASPxFromDate.Text = dtFrom.ToString("dd-MM-yyyy");
                //ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");
                //Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                ASPxFromDate.Value = DateTime.Now;
                ASPxToDate.Value = DateTime.Now;

                BranchpopulateGSTN();
            }
            else
            {

            }
        }

        public void Date_finyearwise(string Finyear)
        {
            CommonBL bll1 = new CommonBL();
            DataTable stbill = new DataTable();
            stbill = bll1.GetDateFinancila(Finyear);
            if (stbill.Rows.Count > 0)
            {
                ASPxFromDate.Text = Convert.ToDateTime(stbill.Rows[0]["FinYear_StartDate"]).ToString("dd-MM-yyyy");
                ASPxToDate.Text = Convert.ToDateTime(stbill.Rows[0]["FinYear_EndDate"]).ToString("dd-MM-yyyy");
            }

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

        protected void B2B_Datarepared(object sender, EventArgs e)
        {
            ASPxGridView grid = (ASPxGridView)sender;

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
            }

        }

        public void bindexport(int Filter)
        {
            string Rptfilename = "GSTR-2 All Report";
            string FileHeader = "";
            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            if (ASPxPageControl1.ActiveTabIndex == 0)
            {
                string filename = "GSTR-2 B2B";
                exporter.FileName = filename;
                exporter.PageHeader.Left = "GSTR-2 B2B Reort";
                FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true,true, true, true) + Environment.NewLine + "GSTR-2 B2B" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
                exporter.PageHeader.Left = FileHeader;
                exporter.PageHeader.Font.Size = 10;
                exporter.PageHeader.Font.Name = "Tahoma";

                //exporter.PageFooter.Center = "[Page # of Pages #]";
                //exporter.PageFooter.Right = "[Date Printed]";
                exporter.Landscape = true;
                exporter.MaxColumnWidth = 100;
                exporter.GridViewID = "B2B";
            }
            else if (ASPxPageControl1.ActiveTabIndex == 1)
            {
                string filename = "GSTR-2 B2BUR";
                exporter.FileName = filename;
                exporter.PageHeader.Left = "GSTR-2 B2BUR Reort";
                FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true,true, true, true) + Environment.NewLine + "GSTR-2 B2BUR" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
                exporter.PageHeader.Left = FileHeader;
                exporter.PageHeader.Font.Size = 10;
                exporter.PageHeader.Font.Name = "Tahoma";

                //exporter.PageFooter.Center = "[Page # of Pages #]";
                //exporter.PageFooter.Right = "[Date Printed]";
                exporter.Landscape = true;
                exporter.MaxColumnWidth = 100;
                exporter.GridViewID = "grid_B2BUR";
            }

            else if (ASPxPageControl1.ActiveTabIndex == 2)
            {
                string filename = "GSTR-2 IMPS";
                exporter.FileName = filename;
                exporter.PageHeader.Left = "GSTR-2 IMPS Reort";
                FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true,true, true, true) + Environment.NewLine + "GSTR-2 IMPS" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
                exporter.PageHeader.Left = FileHeader;
                exporter.PageHeader.Font.Size = 10;
                exporter.PageHeader.Font.Name = "Tahoma";

                //exporter.PageFooter.Center = "[Page # of Pages #]";
                //exporter.PageFooter.Right = "[Date Printed]";
                exporter.Landscape = true;
                exporter.MaxColumnWidth = 100;
                exporter.GridViewID = "grid_IMPS";
            }

            else if (ASPxPageControl1.ActiveTabIndex == 3)
            {
                string filename = "GSTR-2 IMPG";
                exporter.FileName = filename;
                exporter.PageHeader.Left = "GSTR-2 IMPG Reort";
                FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true,true, true, true) + Environment.NewLine + "GSTR-2 IMPG" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
                exporter.PageHeader.Left = FileHeader;
                exporter.PageHeader.Font.Size = 10;
                exporter.PageHeader.Font.Name = "Tahoma";

                //exporter.PageFooter.Center = "[Page # of Pages #]";
                //exporter.PageFooter.Right = "[Date Printed]";
                exporter.Landscape = true;
                exporter.MaxColumnWidth = 100;
                exporter.GridViewID = "grid_IMPG";
            }

            else if (ASPxPageControl1.ActiveTabIndex == 4)
            {
                string filename = "GSTR-2 CDNR";
                exporter.FileName = filename;
                exporter.PageHeader.Left = "GSTR-2 CDNR Reort";
                
                FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true,true, true, true) + Environment.NewLine + "GSTR-2 CDNR" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
                exporter.PageHeader.Left = FileHeader;
                exporter.PageHeader.Font.Size = 10;
                exporter.PageHeader.Font.Name = "Tahoma";

                //exporter.PageFooter.Center = "[Page # of Pages #]";
                //exporter.PageFooter.Right = "[Date Printed]";
                exporter.Landscape = true;
                exporter.MaxColumnWidth = 100;
                exporter.GridViewID = "grid_CDNR";
            }

            else if (ASPxPageControl1.ActiveTabIndex == 5)
            {
                string filename = "GSTR-2 CDNUR";
                exporter.FileName = filename;
                exporter.PageHeader.Left = "GSTR-2 CDNUR Reort";                
                FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true,true, true, true) + Environment.NewLine + "GSTR-2 CDNUR" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
                exporter.PageHeader.Left = FileHeader;
                exporter.PageHeader.Font.Size = 10;
                exporter.PageHeader.Font.Name = "Tahoma";

                //exporter.PageFooter.Center = "[Page # of Pages #]";
                //exporter.PageFooter.Right = "[Date Printed]";
                exporter.Landscape = true;
                exporter.MaxColumnWidth = 100;
                exporter.GridViewID = "grid_CDNUR";
            }
            else if (ASPxPageControl1.ActiveTabIndex == 6)
            {
                string filename = "GSTR-2 EXEMP";
                exporter.FileName = filename;
                exporter.PageHeader.Left = "GSTR-2 EXEMP Reort";                
                FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true,true, true, true) + Environment.NewLine + "GSTR-2 EXEMP" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
                exporter.PageHeader.Left = FileHeader;
                exporter.PageHeader.Font.Size = 10;
                exporter.PageHeader.Font.Name = "Tahoma";

                //exporter.PageFooter.Center = "[Page # of Pages #]";
                //exporter.PageFooter.Right = "[Date Printed]";
                exporter.Landscape = true;
                exporter.MaxColumnWidth = 100;
                exporter.GridViewID = "grid_EXEMP";
            }
            else if (ASPxPageControl1.ActiveTabIndex == 7)
            {
                string filename = "GSTR-2 ITCR";
                exporter.FileName = filename;
                exporter.PageHeader.Left = "GSTR-2 ITCR Reort";                
                FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true,true, true, true) + Environment.NewLine + "GSTR-2 ITCR" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
                exporter.PageHeader.Left = FileHeader;
                exporter.PageHeader.Font.Size = 10;
                exporter.PageHeader.Font.Name = "Tahoma";

                //exporter.PageFooter.Center = "[Page # of Pages #]";
                //exporter.PageFooter.Right = "[Date Printed]";
                exporter.Landscape = true;
                exporter.MaxColumnWidth = 100;
                exporter.GridViewID = "grid_ITCR";
            }
            else if (ASPxPageControl1.ActiveTabIndex == 8)
            {
                string filename = "GSTR-2 HSNSUM";
                exporter.FileName = filename;
                exporter.PageHeader.Left = "GSTR-2 HSNSUM Reort";                
                FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true,true, true, true) + Environment.NewLine + "GSTR-2 HSNSUM" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
                exporter.PageHeader.Left = FileHeader;
                exporter.PageHeader.Font.Size = 10;
                exporter.PageHeader.Font.Name = "Tahoma";

                //exporter.PageFooter.Center = "[Page # of Pages #]";
                //exporter.PageFooter.Right = "[Date Printed]";
                exporter.Landscape = true;
                exporter.MaxColumnWidth = 100;
                exporter.GridViewID = "grid_HSNSUM";
            }

            exporter.RenderBrick += exporter_RenderBrick;
            //exporter.FileName = Rptfilename;
           

            //exporter.PageFooter.Center = "[Page # of Pages #]";
            //exporter.PageFooter.Right = "[Date Printed]";
            //exporter.Landscape = true;
            //exporter.MaxColumnWidth = 100;
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
        
        protected void exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            e.BrickStyle.BackColor = Color.White;
            e.BrickStyle.ForeColor = Color.Black;
        }
       
        #region  ################## GSTR-2 Summary (B2B) ########################

        protected void B2B_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

            Session.Remove("GstR2B2B");

            B2B.JSProperties["cpSave"] = null;
            string returnPara = Convert.ToString(e.Parameters);

            string WhichCall = returnPara.Split('~')[0];
            if (WhichCall == "ListData")
            {
                string WhichCall2 = returnPara.Split('~')[1];
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);


                string BRANCH_ID = "";
                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }



                Task PopulateStockTrialDataTask = new Task(() => GetGstrReportB2B(WhichCall2));
                PopulateStockTrialDataTask.RunSynchronously();


            }
        }

        protected void B2B_DataBinding(object sender, EventArgs e)
        {
            if (Session["GstR2B2B"] != null)
            {
                B2B.DataSource = (DataTable)Session["GstR2B2B"];

            }

        }

        public void GetGstrReportB2B(string Gstn)
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


                dttab = GetGstrReportB2BAll(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]).Trim(), "b2b", Gstn, FROMDATE, TODATE);


                if (dttab.Rows.Count > 0)
                {
                    Session["GstR2B2B"] = dttab;
                    B2B.DataSource = dttab;
                    B2B.DataBind();
                }
                else
                {
                    Session["GstR2B2B"] = null;
                    B2B.DataSource = null;
                    B2B.DataBind();

                }
            }
            catch (Exception ex)
            {
            }
        }
        public DataTable GetGstrReportB2BAll(string Company, string Finyear, string Action, string Gstn, string FROMDATE, string TODATE)
        {
            DataTable ds = new DataTable();
            //ProcedureExecute proc = new ProcedureExecute("prc_GSTR2_Type_Report");

            ProcedureExecute proc = new ProcedureExecute("prc_GSTR2_Type_Report");            
            proc.AddPara("@COMPANYID", Company);
            proc.AddPara("@FINYEAR", Finyear);
            proc.AddPara("@Action", Action);
            proc.AddPara("@GSTIN", Gstn);
            //proc.AddPara("@MONTH", branch);
            proc.AddPara("@FROMDATE", FROMDATE);
            proc.AddPara("@TODATE", TODATE);
            proc.AddPara("@ISDocumentdate", chkdocument.Checked);
            ds = proc.GetTable();
            return ds;
        }

        protected void B2B_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }

        private int totalCount;
        private int totalCountrecp;
        private int totalInvADPaymentVoucherNumber;   

        private List<string> InvoiceNo = new List<string>();

        private List<string> GSTINUIN = new List<string>();

        protected void B2B_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            ASPxSummaryItem item = e.Item as ASPxSummaryItem;

            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Start)
            {
                InvoiceNo.Clear();
                GSTINUIN.Clear();
                totalCount = 0;
                totalCountrecp = 0;


            }
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
            {

                string val = Convert.ToString(e.FieldValue);
                if (!InvoiceNo.Contains(val))
                {
                    totalCount++;
                    InvoiceNo.Add(val);
                }
                if (!GSTINUIN.Contains(val))
                {
                    totalCountrecp++;
                    GSTINUIN.Add(val);
                }

            }
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
            {

                if (e.Item == B2B.TotalSummary["Invoice_Number"])
                {
                    e.TotalValue = string.Format("{0}", totalCount);
                }

                if (e.Item == B2B.TotalSummary["GSTIN_of_Supplier"])
                {
                    e.TotalValue = string.Format("{0}", totalCountrecp);

                }
                
            }



        }




        #endregion

        #region ###############   GSTR-2 Summary (B2BUR) ####################
        protected void grid_B2BUR__CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

            Session.Remove("GSTRRpt2B2BUR");

            B2B.JSProperties["cpSave"] = null;
            string returnPara = Convert.ToString(e.Parameters);

            string WhichCall = returnPara.Split('~')[0];
            if (WhichCall == "ListData")
            {
                string WhichCall2 = returnPara.Split('~')[1];
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);


                string BRANCH_ID = "";
                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }



                Task PopulateStockTrialDataTask = new Task(() => GetGstrB2Cldata(WhichCall2));
                PopulateStockTrialDataTask.RunSynchronously();


            }
           
        }


        protected void grid_B2BUR_DataBinding(object sender, EventArgs e)
        {
            if (Session["GSTRRpt2B2BUR"] != null)
            {
                grid_B2BUR.DataSource = (DataTable)Session["GSTRRpt2B2BUR"];

            }

        }


        public void GetGstrB2Cldata(string Gstn)
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


                dttab = GetGSTRB2ClDetailsGestn(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]).Trim(), "b2bur", Gstn, FROMDATE, TODATE);


                if (dttab.Rows.Count > 0)
                {
                    Session["GSTRRpt2B2BUR"] = dttab;
                    grid_B2BUR.DataSource = dttab;
                    grid_B2BUR.DataBind();
                }
                else
                {
                    Session["GSTRRpt2B2BUR"] = null;
                    grid_B2BUR.DataSource = null;
                    grid_B2BUR.DataBind();

                }
            }
            catch (Exception ex)
            {
            }
        }
        public DataTable GetGSTRB2ClDetailsGestn(string Company, string Finyear, string Action, string Gstn, string FROMDATE, string TODATE)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GSTR2_Type_Report");
            proc.AddPara("@COMPANYID", Company);
            proc.AddPara("@FINYEAR", Finyear);
            proc.AddPara("@Action", Action);
            proc.AddPara("@GSTIN", Gstn);
            //proc.AddPara("@MONTH", branch);
            proc.AddPara("@FROMDATE", FROMDATE);
            proc.AddPara("@TODATE", TODATE);
            proc.AddPara("@ISDocumentdate", chkdocument.Checked);
            ds = proc.GetTable();
            return ds;
        }

        protected void grid_B2BUR_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }
        protected void grid_B2BUR_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            ASPxSummaryItem item = e.Item as ASPxSummaryItem;

            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Start)
            {
                InvoiceNo.Clear();

                totalCount = 0;
            }
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
            {

                string val = Convert.ToString(e.FieldValue);
                if (!InvoiceNo.Contains(val))
                {
                    totalCount++;
                    InvoiceNo.Add(val);
                }


            }
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
            {

                if (e.Item == grid_B2BUR.TotalSummary["Invoice_Number"])
                {
                    e.TotalValue = string.Format("{0}", totalCount);
                }



            }



        }

        #endregion

        #region ###############   GSTR-2 Summary (IMPS) ####################


        protected void Grid_IMPS__CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

            Session.Remove("dt_GSTRRpt_IMPS");

            B2B.JSProperties["cpSave"] = null;
            string returnPara = Convert.ToString(e.Parameters);

            string WhichCall = returnPara.Split('~')[0];
            if (WhichCall == "ListData")
            {
                string WhichCall2 = returnPara.Split('~')[1];
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);


                string BRANCH_ID = "";
                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }
                Task PopulateStockTrialDataTask = new Task(() => GetGstrIMPSdata(WhichCall2));
                PopulateStockTrialDataTask.RunSynchronously();
            }
           
        }



        protected void grid_IMPS_DataBinding(object sender, EventArgs e)
        {
            if (Session["GSTRRpt2IMPS"] != null)
            {
                grid_IMPS.DataSource = (DataTable)Session["GSTRRpt2IMPS"];

            }

        }

        public void GetGstrIMPSdata(string Gstn)
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

                dttab = GetGstrIMPSdatadetails(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]).Trim(), "imps", Gstn, FROMDATE, TODATE);


                if (dttab.Rows.Count > 0)
                {
                    Session["GSTRRpt2IMPS"] = dttab;
                    grid_IMPS.DataSource = dttab;
                    grid_IMPS.DataBind();
                }
                else
                {
                    Session["GSTRRpt2IMPS"] = null;
                    grid_IMPS.DataSource = null;
                    grid_IMPS.DataBind();

                }
            }
            catch (Exception ex)
            {
            }
        }


        public void GetGstrDocumentCount(string Gstn)
        {
            try
            {
                DataTable dttab = new DataTable();

                DateTime dtFrom;
                DateTime dtTo;


                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                string TODATE = dtTo.ToString("yyyy-MM-dd");

                dttab = GetGstrDocumentCountdetails(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]).Trim(), "IMPS", Gstn, FROMDATE, TODATE);


                if (dttab.Rows.Count > 0)
                {
                    Session["dt_GSTRDocCount_hsn"] = dttab;
                    //GST_DocumentCount.DataSource = dttab;
                    //GST_DocumentCount.DataBind();
                }
                else
                {
                    Session["dt_GSTRDocCount_hsn"] = null;
                    //GST_DocumentCount.DataSource = null;
                    //GST_DocumentCount.DataBind();

                }
            }
            catch (Exception ex)
            {
            }
        }


        public DataTable GetGstrIMPSdatadetails(string Company, string Finyear, string Action, string Gstn, string FROMDATE, string TODATE)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GSTR2_Type_Report");
            proc.AddPara("@COMPANYID", Company);
            proc.AddPara("@FINYEAR", Finyear);
            proc.AddPara("@Action", Action);
            proc.AddPara("@GSTIN", Gstn);
            proc.AddPara("@FROMDATE", FROMDATE);
            proc.AddPara("@TODATE", TODATE);
            proc.AddPara("@ISDocumentdate", chkdocument.Checked);
            ds = proc.GetTable();
            return ds;
        }

        public DataTable GetGstrDocumentCountdetails(string Company, string Finyear, string Action, string Gstn, string FROMDATE, string TODATE)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_DocumentNumberCount");
            //proc.AddPara("@COMPANYID", Company);
            //proc.AddPara("@FINYEAR", Finyear);
            //proc.AddPara("@Action", Action);
            proc.AddPara("@Gstin", Gstn);

            proc.AddPara("@Fromdate", FROMDATE);
            proc.AddPara("@ToDate", TODATE);
            ds = proc.GetTable();
            return ds;
        }

        protected void Grid_IMPS_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }
        protected void GridView_IMPS_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            ASPxSummaryItem item = e.Item as ASPxSummaryItem;

            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Start)
            {
                Invoice.Clear();
                //GSTINUIN.Clear();
                totalCount = 0;
                //totalCountrecp = 0;


            }
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
            {

                string val = Convert.ToString(e.FieldValue);
                if (!Invoice.Contains(val))
                {
                    totalCount++;
                    Invoice.Add(val);
                }
                //if (!GSTINUIN.Contains(val))
                //{
                //    totalCountrecp++;
                //    GSTINUIN.Add(val);
                //}

            }
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
            {

                if (e.Item == grid_IMPS.TotalSummary["Invoice Number of Reg Recipient"])
                {
                    e.TotalValue = string.Format("{0}", totalCount);
                }

                //if (e.Item == grid_IMPG.TotalSummary["GSTINUIN"])
                //{
                //    e.TotalValue = string.Format("No. of Recipients={0}", totalCountrecp);

                //}


            }



        }


        #endregion

        #region  ###############  GSTR-2 Summary (IMPG) ######################


        protected void Grid_IMPG_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

            Session.Remove("dt_GSTRRpt_IMPG");

            B2B.JSProperties["cpSave"] = null;
            string returnPara = Convert.ToString(e.Parameters);

            string WhichCall = returnPara.Split('~')[0];
            if (WhichCall == "ListData")
            {
                string WhichCall2 = returnPara.Split('~')[1];
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);


                string BRANCH_ID = "";
                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }



                Task PopulateStockTrialDataTask = new Task(() => GetGstrIMPGdata(WhichCall2));
                PopulateStockTrialDataTask.RunSynchronously();
            }
        }


        protected void Grid_IMPG_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }
        protected void grid_IMPG_DataBinding(object sender, EventArgs e)
        {
            if (Session["GSTRRpt2IMPG"] != null)
            {
                grid_IMPG.DataSource = (DataTable)Session["GSTRRpt2IMPG"];

            }

        }

        public void GetGstrIMPGdata(string Gstn)
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


                dttab = GetGstrIMPGdataDetails(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]).Trim(), "IMPG", Gstn, FROMDATE, TODATE);


                if (dttab.Rows.Count > 0)
                {
                    Session["GSTRRpt2IMPG"] = dttab;
                    grid_IMPG.DataSource = dttab;
                    grid_IMPG.DataBind();
                }
                else
                {
                    Session["GSTRRpt2IMPG"] = null;
                    grid_IMPG.DataSource = null;
                    grid_IMPG.DataBind();

                }
            }
            catch (Exception ex)
            {
            }
        }
        public DataTable GetGstrIMPGdataDetails(string Company, string Finyear, string Action, string Gstn, string FROMDATE, string TODATE)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GSTR2_Type_Report");
            proc.AddPara("@COMPANYID", Company);
            proc.AddPara("@FINYEAR", Finyear);
            proc.AddPara("@Action", Action);
            proc.AddPara("@GSTIN", Gstn);
            //proc.AddPara("@MONTH", branch);
            proc.AddPara("@FROMDATE", FROMDATE);
            proc.AddPara("@TODATE", TODATE);
            proc.AddPara("@ISDocumentdate", chkdocument.Checked);
            ds = proc.GetTable();
            return ds;
        }


        private List<string> Invoice = new List<string>();
        private List<string> InvAdPaymentVoucherNumber = new List<string>(); 

        protected void GridView_IMPG_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            ASPxSummaryItem item = e.Item as ASPxSummaryItem;

            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Start)
            {
                Invoice.Clear();
                //GSTINUIN.Clear();
                totalCount = 0;
                //totalCountrecp = 0;


            }
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
            {

                string val = Convert.ToString(e.FieldValue);
                if (!Invoice.Contains(val))
                {
                    totalCount++;
                    Invoice.Add(val);
                }
                //if (!GSTINUIN.Contains(val))
                //{
                //    totalCountrecp++;
                //    GSTINUIN.Add(val);
                //}

            }
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
            {

                if (e.Item == grid_IMPG.TotalSummary["Invoice Number of Reg Recipient"])
                {
                    e.TotalValue = string.Format("{0}", totalCount);
                }

                //if (e.Item == grid_IMPG.TotalSummary["GSTINUIN"])
                //{
                //    e.TotalValue = string.Format("No. of Recipients={0}", totalCountrecp);

                //}


            }



        }

        #endregion

        #region ################### GSTR-2 Summary (CDNR) ###################

        protected void grid_CDNR_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

            Session.Remove("dt_GSTRRpt_cdnur");

            B2B.JSProperties["cpSave"] = null;
            string returnPara = Convert.ToString(e.Parameters);

            string WhichCall = returnPara.Split('~')[0];
            if (WhichCall == "ListData")
            {
                string WhichCall2 = returnPara.Split('~')[1];
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);


                string BRANCH_ID = "";
                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }


                Task PopulateStockTrialDataTask = new Task(() => GetGSTRCDNRdata(WhichCall2));
                PopulateStockTrialDataTask.RunSynchronously();
            }
        }



        protected void grid_CDNR_DataBinding(object sender, EventArgs e)
        {
            if (Session["GSTRRpt2CDNR"] != null)
            {
                grid_CDNR.DataSource = (DataTable)Session["GSTRRpt2CDNR"];

            }

        }

        public void GetGSTRCDNRdata(string Gstn)
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


                dttab = GetGSTRCDNRdatadetails(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]).Trim(), "cdnr", Gstn, FROMDATE, TODATE);


                if (dttab.Rows.Count > 0)
                {
                    Session["GSTRRpt2CDNR"] = dttab;
                    grid_CDNR.DataSource = dttab;
                    grid_CDNR.DataBind();
                }
                else
                {
                    Session["GSTRRpt2CDNR"] = null;
                    grid_CDNR.DataSource = null;
                    grid_CDNR.DataBind();

                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void grid_CDNR_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }
        public DataTable GetGSTRCDNRdatadetails(string Company, string Finyear, string Action, string Gstn, string FROMDATE, string TODATE)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GSTR2_Type_Report");
            proc.AddPara("@COMPANYID", Company);
            proc.AddPara("@FINYEAR", Finyear);
            proc.AddPara("@Action", Action);
            proc.AddPara("@GSTIN", Gstn);
            //proc.AddPara("@MONTH", branch);
            proc.AddPara("@FROMDATE", FROMDATE);
            proc.AddPara("@TODATE", TODATE);
            proc.AddPara("@ISDocumentdate", chkdocument.Checked);
            ds = proc.GetTable();
            return ds;
        }

        protected void grid_CDNR_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            ASPxSummaryItem item = e.Item as ASPxSummaryItem;

            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Start)
            {
                Invoice.Clear();
                GSTINUIN.Clear();
                InvAdPaymentVoucherNumber.Clear();
                totalCount = 0;
                totalCountrecp = 0;
                totalInvADPaymentVoucherNumber = 0;

            }
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
            {

                string val = Convert.ToString(e.FieldValue);
                
                if (!GSTINUIN.Contains(val))
                {
                    totalCountrecp++;
                    GSTINUIN.Add(val);
                }
                if (!Invoice.Contains(val))
                {
                    totalCount++;
                    Invoice.Add(val);
                }
                if (!InvAdPaymentVoucherNumber.Contains(val))
                {
                    totalInvADPaymentVoucherNumber++;
                    InvAdPaymentVoucherNumber.Add(val);
                }

            }
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
            {

                if (e.Item == grid_CDNR.TotalSummary["GSTIN of Supplier"])
                {
                    e.TotalValue = string.Format("{0}", totalCount);
                }

                if (e.Item == grid_CDNR.TotalSummary["Note/Refund Voucher Number"])
                {
                    e.TotalValue = string.Format("{0}", totalCountrecp);

                }
                if (e.Item == grid_CDNR.TotalSummary["Invoice/Advance Payment Voucher Number"])
                {
                    e.TotalValue = string.Format("{0}", totalInvADPaymentVoucherNumber);

                }


            }



        }

        #endregion

        #region  ################ GSTR-2 Summary CDNUR  ####################

        protected void grid_CDNUR_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            Session.Remove("GSTRRpt2CDNUR");

            //B2B.JSProperties["cpSave"] = null;
            string returnPara = Convert.ToString(e.Parameters);

            string WhichCall = returnPara.Split('~')[0];
            if (WhichCall == "ListData")
            {
                string WhichCall2 = returnPara.Split('~')[1];
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);


                string BRANCH_ID = "";
                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }



                Task PopulateStockTrialDataTask = new Task(() => GetCDUNRdata(WhichCall2));
                PopulateStockTrialDataTask.RunSynchronously();
            }
        }

        protected void grid_CDNUR_DataBinding(object sender, EventArgs e)
        {
            if (Session["GSTRRpt2CDNUR"] != null)
            {
                grid_CDNUR.DataSource = (DataTable)Session["GSTRRpt2CDNUR"];

            }

        }

        protected void grid_CDNUR_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }
        public void GetCDUNRdata(string Gstn)
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


                dttab = GetCDNURdataDetails(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]).Trim(), "cdnur", Gstn, FROMDATE, TODATE);


                if (dttab.Rows.Count > 0)
                {
                    Session["GSTRRpt2CDNUR"] = dttab;
                    grid_CDNUR.DataSource = dttab;
                    grid_CDNUR.DataBind();
                }
                else
                {
                    Session["GSTRRpt2CDNUR"] = null;
                    grid_CDNUR.DataSource = null;
                    grid_CDNUR.DataBind();

                }
            }
            catch (Exception ex)
            {
            }
        }
        public DataTable GetCDNURdataDetails(string Company, string Finyear, string Action, string Gstn, string FROMDATE, string TODATE)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GSTR2_Type_Report");
            proc.AddPara("@COMPANYID", Company);
            proc.AddPara("@FINYEAR", Finyear);
            proc.AddPara("@Action", Action);
            proc.AddPara("@GSTIN", Gstn);
            //proc.AddPara("@MONTH", branch);
            proc.AddPara("@FROMDATE", FROMDATE);
            proc.AddPara("@TODATE", TODATE);
            proc.AddPara("@ISDocumentdate", chkdocument.Checked);
            ds = proc.GetTable();
            return ds;
        }


        protected void grid_CDNUR_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            ASPxSummaryItem item = e.Item as ASPxSummaryItem;

            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Start)
            {
                InvoiceNo.Clear();
                GSTINUIN.Clear();
                totalCount = 0;
                totalCountrecp = 0;


            }
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
            {

                string val = Convert.ToString(e.FieldValue);
                if (!InvoiceNo.Contains(val))
                {
                    totalCount++;
                    InvoiceNo.Add(val);
                }
                if (!GSTINUIN.Contains(val))
                {
                    totalCountrecp++;
                    GSTINUIN.Add(val);
                }

            }
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
            {

                if (e.Item == grid_CDNUR.TotalSummary["Note/Refund Voucher Number"])
                {
                    e.TotalValue = string.Format("{0}", totalCount);
                }

                if (e.Item == grid_CDNUR.TotalSummary["Invoice/Advance Payment Voucher Number"])
                {
                    e.TotalValue = string.Format("{0}", totalCountrecp);

                }


            }

        }

        #endregion

        #region  ################ GSTR-2 Summary EXEMP  ####################
        protected void grid_EXEMP_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            Session.Remove("GSTRRpt2EXEMP");

            string returnPara = Convert.ToString(e.Parameters);

            string WhichCall = returnPara.Split('~')[0];
            if (WhichCall == "ListData")
            {
                string WhichCall2 = returnPara.Split('~')[1];
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

                string BRANCH_ID = "";
                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }

                Task PopulateStockTrialDataTask = new Task(() => GetEXEMPdata(WhichCall2));
                PopulateStockTrialDataTask.RunSynchronously();
            }
        }

        protected void grid_EXEMP_DataBinding(object sender, EventArgs e)
        {
            if (Session["GSTRRpt2EXEMP"] != null)
            {
                grid_EXEMP.DataSource = (DataTable)Session["GSTRRpt2EXEMP"];

            }
        }

        public void GetEXEMPdata(string Gstn)
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


                dttab = GetEXEMPdataDetails(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]).Trim(), "exemp", Gstn, FROMDATE, TODATE);


                if (dttab.Rows.Count > 0)
                {
                    Session["GSTRRpt2EXEMP"] = dttab;
                    grid_EXEMP.DataSource = dttab;
                    grid_EXEMP.DataBind();
                }
                else
                {
                    Session["GSTRRpt2EXEMP"] = null;
                    grid_EXEMP.DataSource = null;
                    grid_EXEMP.DataBind();

                }
            }
            catch (Exception ex)
            {
            }
        }

        public DataTable GetEXEMPdataDetails(string Company, string Finyear, string Action, string Gstn, string FROMDATE, string TODATE)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GSTR2_Type_Report");
            proc.AddPara("@COMPANYID", Company);
            proc.AddPara("@FINYEAR", Finyear);
            proc.AddPara("@Action", Action);
            proc.AddPara("@GSTIN", Gstn);
            proc.AddPara("@FROMDATE", FROMDATE);
            proc.AddPara("@TODATE", TODATE);
            proc.AddPara("@ISDocumentdate", chkdocument.Checked);
            ds = proc.GetTable();
            return ds;
        }

        protected void grid_EXEMP_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            ASPxSummaryItem item = e.Item as ASPxSummaryItem;

            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Start)
            {
                InvoiceNo.Clear();
                GSTINUIN.Clear();
                totalCount = 0;
                totalCountrecp = 0;
            }
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
            {

                string val = Convert.ToString(e.FieldValue);
                if (!InvoiceNo.Contains(val))
                {
                    totalCount++;
                    InvoiceNo.Add(val);
                }
                if (!GSTINUIN.Contains(val))
                {
                    totalCountrecp++;
                    GSTINUIN.Add(val);
                }

            }
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
            {

                if (e.Item == grid_EXEMP.TotalSummary["Description"])
                {
                    e.TotalValue = string.Format("{0}", totalCount);
                }

            }

        }

        protected void grid_EXEMP_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }

        #endregion

        #region  ################ GSTR-2 Summary ITCR  ####################

        protected void grid_ITCR_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            Session.Remove("GSTRRpt2ITCR");

            string returnPara = Convert.ToString(e.Parameters);

            string WhichCall = returnPara.Split('~')[0];
            if (WhichCall == "ListData")
            {
                string WhichCall2 = returnPara.Split('~')[1];
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

                string BRANCH_ID = "";
                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }

                Task PopulateStockTrialDataTask = new Task(() => GetITCRdata(WhichCall2));
                PopulateStockTrialDataTask.RunSynchronously();
            }
        }

        protected void grid_ITCR_DataBinding(object sender, EventArgs e)
        {
            if (Session["GSTRRpt2ITCR"] != null)
            {
                grid_ITCR.DataSource = (DataTable)Session["GSTRRpt2ITCR"];

            }
        }

        public void GetITCRdata(string Gstn)
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


                dttab = GetITCRdataDetails(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]).Trim(), "itcr", Gstn, FROMDATE, TODATE);


                if (dttab.Rows.Count > 0)
                {
                    Session["GSTRRpt2ITCR"] = dttab;
                    grid_ITCR.DataSource = dttab;
                    grid_ITCR.DataBind();
                }
                else
                {
                    Session["GSTRRpt2ITCR"] = null;
                    grid_ITCR.DataSource = null;
                    grid_ITCR.DataBind();

                }
            }
            catch (Exception ex)
            {
            }
        }

        public DataTable GetITCRdataDetails(string Company, string Finyear, string Action, string Gstn, string FROMDATE, string TODATE)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GSTR2_Type_Report");
            proc.AddPara("@COMPANYID", Company);
            proc.AddPara("@FINYEAR", Finyear);
            proc.AddPara("@Action", Action);
            proc.AddPara("@GSTIN", Gstn);
            proc.AddPara("@FROMDATE", FROMDATE);
            proc.AddPara("@TODATE", TODATE);
            proc.AddPara("@ISDocumentdate", chkdocument.Checked);
            ds = proc.GetTable();
            return ds;
        }

        protected void grid_ITCR_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            ASPxSummaryItem item = e.Item as ASPxSummaryItem;

            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Start)
            {
                InvoiceNo.Clear();
                GSTINUIN.Clear();
                totalCount = 0;
                totalCountrecp = 0;


            }
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
            {

                string val = Convert.ToString(e.FieldValue);
                if (!InvoiceNo.Contains(val))
                {
                    totalCount++;
                    InvoiceNo.Add(val);
                }
                if (!GSTINUIN.Contains(val))
                {
                    totalCountrecp++;
                    GSTINUIN.Add(val);
                }

            }
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
            {

                if (e.Item == grid_ITCR.TotalSummary["Description_for_reversal_of_ITC"])
                {
                    e.TotalValue = string.Format("{0}", totalCount);
                }

            }

        }
        protected void grid_ITCR_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }

        #endregion

        #region  ################ GSTR-2 Summary HSNSUM  ####################
        protected void grid_HSNSUM_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            Session.Remove("GSTRRpt2HSNSUM");

            string returnPara = Convert.ToString(e.Parameters);

            string WhichCall = returnPara.Split('~')[0];
            if (WhichCall == "ListData")
            {
                string WhichCall2 = returnPara.Split('~')[1];
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

                string BRANCH_ID = "";
                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }

                Task PopulateStockTrialDataTask = new Task(() => GetHSNSUMdata(WhichCall2));
                PopulateStockTrialDataTask.RunSynchronously();
            }
        }

        protected void grid_HSNSUM_DataBinding(object sender, EventArgs e)
        {
            if (Session["GSTRRpt2HSNSUM"] != null)
            {
                grid_HSNSUM.DataSource = (DataTable)Session["GSTRRpt2HSNSUM"];

            }
        }

        public void GetHSNSUMdata(string Gstn)
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


                dttab = GetHSNSUMdataDetails(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]).Trim(), "hsnsum", Gstn, FROMDATE, TODATE);


                if (dttab.Rows.Count > 0)
                {
                    Session["GSTRRpt2HSNSUM"] = dttab;
                    grid_HSNSUM.DataSource = dttab;
                    grid_HSNSUM.DataBind();
                }
                else
                {
                    Session["GSTRRpt2HSNSUM"] = null;
                    grid_HSNSUM.DataSource = null;
                    grid_HSNSUM.DataBind();

                }
            }
            catch (Exception ex)
            {
            }
        }
        public DataTable GetHSNSUMdataDetails(string Company, string Finyear, string Action, string Gstn, string FROMDATE, string TODATE)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GSTR2_Type_Report");
            proc.AddPara("@COMPANYID", Company);
            proc.AddPara("@FINYEAR", Finyear);
            proc.AddPara("@Action", Action);
            proc.AddPara("@GSTIN", Gstn);
            proc.AddPara("@FROMDATE", FROMDATE);
            proc.AddPara("@TODATE", TODATE);
            proc.AddPara("@ISDocumentdate", chkdocument.Checked);
            ds = proc.GetTable();
            return ds;
        }
        protected void grid_HSNSUM_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            ASPxSummaryItem item = e.Item as ASPxSummaryItem;

            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Start)
            {
                InvoiceNo.Clear();
                GSTINUIN.Clear();
                totalCount = 0;
                totalCountrecp = 0;


            }
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
            {

                string val = Convert.ToString(e.FieldValue);
                if (!InvoiceNo.Contains(val))
                {
                    totalCount++;
                    InvoiceNo.Add(val);
                }
                if (!GSTINUIN.Contains(val))
                {
                    totalCountrecp++;
                    GSTINUIN.Add(val);
                }

            }
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
            {

                if (e.Item == grid_HSNSUM.TotalSummary["sProducts_HsnCode"])
                {
                    e.TotalValue = string.Format("{0}", totalCount);
                }
            }

        }

        protected void grid_HSNSUM_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }
        #endregion
        
    }
}