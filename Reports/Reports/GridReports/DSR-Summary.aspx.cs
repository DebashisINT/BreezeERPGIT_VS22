using DevExpress.Export;
using DevExpress.Web;
using DevExpress.XtraPrinting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Reports.Reports.GridReports
{
    public partial class DSR_Summary : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FormDate.Date = DateTime.Now;
                toDate.Date = DateTime.Now;
                Session["dt_DSRSummary"] = null;
                Session["chk_presenttotal"] = 0;
            }
        }

        protected void Grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            string WhichCall = Convert.ToString(e.Parameters).Split('~')[0];

            if (WhichCall == "FilterGridByDate")
            {
                string FromDate = (e.Parameters.Split('~')[1]);
                string ToDate = (e.Parameters.Split('~')[2]);
                GetDsrsummary(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), FromDate, ToDate);

            }

        }

        protected void ShowGrid_DataBinding(object sender, EventArgs e)
        {
            if (Session["dt_DSRSummary"] != null)
            {
                ShowGrid.DataSource = (DataTable)Session["dt_DSRSummary"];
            }
        }

        public void GetDsrsummary(string branch, string FROMDATE, string TODATE)
        {
            try
            {

                string DriverName = string.Empty;
                string PhoneNo = string.Empty;
                string VehicleNo = string.Empty;
                DataSet ds = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("Proc_Report_DSRSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                cmd.Parameters.AddWithValue("@TODATE", TODATE);


                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);
                cmd.Dispose();
                con.Dispose();

                Session["dt_DSRSummary"] = ds.Tables[0];

                ShowGrid.DataSource = ds.Tables[0];
                ShowGrid.DataBind();

            }
            catch (Exception ex)
            {
            }
        }

        protected void ShowGridCustOut_HtmlFooterCellPrepared(object sender, ASPxGridViewTableFooterCellEventArgs e)
        {
            if (e.Column.Caption != "Recordno")
            {
                e.Cell.Style["text-align"] = "right";
            }

        }

        protected void ShowGridCustOut_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (Convert.ToString(e.CellValue).Contains("Total of ")) 
            {
                Session["chk_presenttotal"] = 1;
            }
            if (Convert.ToInt32(Session["chk_presenttotal"]) == 1)
            {
                e.Cell.Font.Bold = true;
                e.Cell.BackColor = Color.DarkSeaGreen;
            }

            if (e.DataColumn.FieldName == "Recordno")
            {
                Session["chk_presenttotal"] = 0;
            }
        }

        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                bindexport(Filter);
                drdExport.SelectedValue = "0";
            }

        }

        public void bindexport(int Filter)
        {
            string filename = "DSR Summary";
            exporter.FileName = filename;
            string FileHeader = "";
            exporter.GridViewID = "ShowGrid";
            exporter.FileName = filename;
            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";



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

    }
}