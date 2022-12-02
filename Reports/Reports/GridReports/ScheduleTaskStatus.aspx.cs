using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;

using DevExpress.XtraPrinting;
using DevExpress.Export;
using System.Drawing;
using DevExpress.Web;
using Reports.Model;

namespace Reports.Reports.GridReports
{
    public partial class ScheduleTaskStatus : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime dtFrom;
            DateTime dtTo;
            if (!IsPostBack)
            {
                Session["IsTransFilter"] = null;
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Schedule Task Status";
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

                Session.Remove("dt_ScheduleTstStatusRpt");

                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;

                ASPxFromDate.Text = dtFrom.ToString("dd-MM-yyyy");
                ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
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

        public void bindexport(int Filter)
        {
            string filename = "Schedule Task Status";
            exporter.FileName = filename;
            string FileHeader = "";

            exporter.FileName = filename;

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Schedule Task Status" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");

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

        protected void exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            e.BrickStyle.BackColor = Color.White;
            e.BrickStyle.ForeColor = Color.Black;
        }
        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "USER_ID";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsTransFilter"]) == "Y")
            {
                var q = from d in dc.SCHEDULETASKSTATUS_REPORTs
                        where Convert.ToString(d.USERID) == Userid
                        orderby d.USER_NAME ascending
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.SCHEDULETASKSTATUS_REPORTs
                        where Convert.ToString(d.USER_ID) == "0"
                        orderby d.USER_ID ascending
                        select d;
                e.QueryableSource = q;
            }
        }
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {

            Session.Remove("dt_ScheduleTstStatusRpt");
            ShowGrid.JSProperties["cpSave"] = null;

            DateTime dtTo;
            dtTo = Convert.ToDateTime(ASPxToDate.Date);
            string TODATE = dtTo.ToString("yyyy-MM-dd");

            DateTime dtFrom;
            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");

            string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);

            string TRANSPORTER_ID = "";
            string QuoComponent2 = "";

            string IsTransFilter = Convert.ToString(hfIsTransFilter.Value);
            Session["IsTransFilter"] = IsTransFilter;

            TRANSPORTER_ID = QuoComponent2.TrimStart(',');

            Task PopulateStockTrialDataTask = new Task(() => GetScheduleTaskReportData(TODATE, FROMDATE, COMPANYID));
            PopulateStockTrialDataTask.RunSynchronously();
        }


        public void GetScheduleTaskReportData(string TODATE, string FROMDATE, string COMPANYID)
        {
            try
            {
                DataSet ds = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_SCHEDULETASKSTATUS_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                cmd.Parameters.AddWithValue("@TODATE", TODATE);
                cmd.Parameters.AddWithValue("@USERID", Convert.ToString(Session["userid"]).Trim());

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);

                cmd.Dispose();
                con.Dispose();

                Session["dt_ScheduleTstStatusRpt"] = ds.Tables[0];

                ShowGrid.DataSource = ds.Tables[0];
                ShowGrid.DataBind();
            }
            catch (Exception ex)
            {
            }
        }

        protected void grid2_DataBinding(object sender, EventArgs e)
        {

            if (Session["dt_ScheduleTstStatusRpt"] != null)
            {
                ShowGrid.DataSource = (DataTable)Session["dt_ScheduleTstStatusRpt"];
            }
        }

    }
}