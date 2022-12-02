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
using DevExpress.XtraPrinting;
using DevExpress.Export;
using System.Drawing;

namespace Reports.Reports.GridReports
{
    public partial class GSTR_HSNcodeRate : System.Web.UI.Page
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
                RptHeading.Text = "GST Rate HSN/SAC Code Wise";
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

                Session["gridGstR1"] = null;
                Session["dt_GSTRRpt"] = null;
                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                //   BindDropDownList();


                //    Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                //  BranchpopulateGSTN();


                GetSalesRegisterdata("0");
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
                //    ASPxFromDate.Text = Convert.ToDateTime(stbill.Rows[0]["FinYear_StartDate"]).ToString("dd-MM-yyyy");
                //    ASPxToDate.Text = Convert.ToDateTime(stbill.Rows[0]["FinYear_EndDate"]).ToString("dd-MM-yyyy");
            }

        }
        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {


                if (Session["exportval"] == null)
                {
                    ///  Session["exportval"] = Filter;
                    // BindDropDownList();
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    /// Session["exportval"] = Filter;
                    // BindDropDownList();
                    bindexport(Filter);
                }
            }

        }



        public void bindexport(int Filter)
        {

            if (ASPxPageControl1.ActiveTabIndex == 0)
            {
                string filename = "GST Rate HSN Code(input)";
                exporter.FileName = filename;
                exporter.GridViewID = "ShowGrid";
                string FileHeader = "";

                exporter.FileName = filename;

                BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

                FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true,true, true, true) + Environment.NewLine + "GST Rate HSN Code(Input)";

                exporter.PageHeader.Left = FileHeader;
                exporter.PageHeader.Font.Size = 10;
                exporter.PageHeader.Font.Name = "Tahoma";

                exporter.RenderBrick += exporter_RenderBrick;

                //exporter.PageFooter.Center = "[Page # of Pages #]";
                //exporter.PageFooter.Right = "[Date Printed]";
                exporter.Landscape = true;
                exporter.MaxColumnWidth = 100;

            }
            else
            {
                string filename = "GST Rate HSN Code(Output)";
                exporter.FileName = filename;
                exporter.GridViewID = "ShowGrid2";
                string FileHeader = "";

                exporter.FileName = filename;

                BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

                FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true,true, true, true) + Environment.NewLine + "GST Rate HSN Code(Output)";

                exporter.PageHeader.Left = FileHeader;
                exporter.PageHeader.Font.Size = 10;
                exporter.PageHeader.Font.Name = "Tahoma";
                exporter.RenderBrick += exporter_RenderBrick;

                exporter.PageFooter.Center = "[Page # of Pages #]";
                exporter.PageFooter.Right = "[Date Printed]";
                exporter.Landscape = true;
                exporter.MaxColumnWidth = 100;

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

        protected void exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            e.BrickStyle.BackColor = Color.White;
            e.BrickStyle.ForeColor = Color.Black;
        }

        protected void Grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

            Session.Remove("dt_GSTRRpt");

            ShowGrid.JSProperties["cpSave"] = null;
            string returnPara = Convert.ToString(e.Parameters);

            string WhichCall = returnPara.Split('~')[0];
            if (WhichCall == "ListData")
            {
                string WhichCall2 = returnPara.Split('~')[1];
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);


                string BRANCH_ID = "";
                //if (hdnSelectedBranches.Value != "")
                //{
                //    BRANCH_ID = hdnSelectedBranches.Value;
                //}




                GetSalesRegisterdata(WhichCall2);
            }
        }

        protected void grid_DataBinding(object sender, EventArgs e)
        {
            if (Session["gridGstR1"] != null)
            {
                ShowGrid.DataSource = (DataTable)Session["gridGstR1"];

            }

        }

        protected void grid2_DataBinding(object sender, EventArgs e)
        {
            if (Session["gridGstR1"] != null)
            {
                ShowGrid2.DataSource = (DataTable)Session["gridGstR1"];

            }

        }

        public void GetSalesRegisterdata(string Gstn)
        {
            try
            {

                DataTable dttab = new DataTable();

                DateTime dtFrom;
                DateTime dtTo;


                //dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                //dtTo = Convert.ToDateTime(ASPxToDate.Date);

                //string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                //string TODATE = dtTo.ToString("yyyy-MM-dd");

                dttab = GetBranchGestn(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]).Trim(), "hsn", Gstn, "", "");

                if (dttab.Rows.Count > 0)
                {
                    Session["gridGstR1"] = dttab;
                    ShowGrid.DataSource = dttab;
                    ShowGrid.DataBind();
                    ShowGrid2.DataSource = dttab;
                    ShowGrid2.DataBind();



                }
                else
                {
                    Session["gridGstR1"] = null;
                    ShowGrid.DataSource = null;
                    ShowGrid.DataBind();

                    ShowGrid2.DataSource = null;
                    ShowGrid2.DataBind();

                }
            }
            catch (Exception ex)
            {
            }
        }
        public DataTable GetBranchGestn(string Company, string Finyear, string Action, string Gstn, string FROMDATE, string TODATE)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("GetHSNgstr");
            //proc.AddPara("@COMPANYID", Company);
            //proc.AddPara("@FINYEAR", Finyear);
            //proc.AddPara("@Action", Action);
            //proc.AddPara("@GSTIN", Gstn);

            //proc.AddPara("@FROMDATE", FROMDATE);
            //proc.AddPara("@TODATE", TODATE);
            ds = proc.GetTable();
            return ds;
        }

        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {

            e.Text = string.Format("{0}", Convert.ToDecimal(e.Value));

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
                //ddlgstn.DataSource = ds.Tables[0];
                //ddlgstn.DataTextField = "branch_GSTIN";
                //ddlgstn.DataValueField = "branch_GSTIN";
                //ddlgstn.DataBind();
                //ddlgstn.Items.Insert(0, "");
            }


        }

        #endregion

        protected void Showgrid_Datarepared(object sender, EventArgs e)
        {
            ASPxGridView grid = (ASPxGridView)sender;
            //if (ddlgstrtype.SelectedValue == "7" )
            //{


            //    grid.Columns["GSTIN/UIN"].Visible = false;
            //    grid.Columns["Date"].Visible = false;
            //    grid.Columns["Value"].Visible = false;
            //    grid.Columns["Taxable value"].Visible = false;
            //    grid.Columns["POS"].Visible = false;
            //    grid.Columns["Reverse Charge"].Visible = false;
            //    grid.Columns["GSTIN E-Commerce"].Visible = false;




            //}

            ////else 
            ////{

            ////}
            //    //grid.Columns["GSTIN/UIN"].Visible = true;
            //    //grid.Columns["Date"].Visible = true;
            //    //grid.Columns["Value"].Visible = true;
            //    //grid.Columns["Taxable value"].Visible = false;
            //    //grid.Columns["POS"].Visible = true;
            //    //grid.Columns["Reverse Charge"].Visible = true;
            //    //grid.Columns["GSTIN E-Commerce"].Visible = true;




            //else if (ddlgstrtype.SelectedValue == "7A")
            //{


            //    grid.Columns["GSTIN/UIN"].Visible = false;
            //    grid.Columns["Date"].Visible = false;
            //    grid.Columns["Value"].Visible = false;
            //    grid.Columns["Taxable value"].Visible = false;
            //    grid.Columns["POS"].Visible = false;
            //    grid.Columns["Reverse Charge"].Visible = false;
            //    grid.Columns["GSTIN E-Commerce"].Visible = false;




            //}






        }

    }
}