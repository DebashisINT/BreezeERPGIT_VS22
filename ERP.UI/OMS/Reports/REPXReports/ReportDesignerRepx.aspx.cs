using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ERP.OMS.Reports.XtraReports;
using DevExpress.DataAccess.Sql;
using DevExpress.XtraReports.UI;

namespace ERP.OMS.Reports.REPXReports //.XtraReports
{
    public partial class ReportDesignerRepx : System.Web.UI.Page
    {
        BusinessLogicLayer.ReportLayout rpLayout = new BusinessLogicLayer.ReportLayout();
        BusinessLogicLayer.ReportData rptData = new BusinessLogicLayer.ReportData();

        protected void Page_Load(object sender, EventArgs e)
        {
            // The name for a file to save a report.
            //RptName.Value = HttpContext.Current.Request.QueryString["LoadrptName"];
            if (!IsPostBack && !IsCallback)
            {
                //string tempFile = HttpContext.Current.Request.QueryString["LoadrptName"];
                if (string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["LoadrptName"]))
                {
                    // Run the Wizard to create a new report.
                    RptName.Value = HttpContext.Current.Request.QueryString["NewReport"];
                    string tempFile = RptName.Value;
                    CreateReport(tempFile);
                }
                else if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["LoadrptName"]))                    
                {
                    // Load report.
                    RptName.Value = HttpContext.Current.Request.QueryString["LoadrptName"];
                    string tempFile = RptName.Value;
                    LoadReport(tempFile);
                }
            }            
        }

        private void CreateReport(string fileName)
        {            
            DevExpress.DataAccess.Sql.SqlDataSource sql = GenerateSqlDataSource();
            var rpt = new DevExpress.XtraReports.UI.XtraReport();
            string rptName = fileName;
            XtraReport newXtraReport = new XtraReport();
            newXtraReport.DataSource = sql;            
            ASPxReportDesigner1.OpenReport(newXtraReport);
        }

        private void LoadReport(string fileName)
        {
            string rptName = fileName;
            string filePath = Server.MapPath("/oms/Reports/RepxReportDesign/" + rptName + ".repx");
            DevExpress.DataAccess.Sql.SqlDataSource sql = GenerateSqlDataSource();
            XtraReport newXtraReport = XtraReport.FromFile(filePath, true);
            newXtraReport.LoadLayout(filePath);
            newXtraReport.DataSource = sql;
            ASPxReportDesigner1.OpenReport(newXtraReport);
        }
       
        private DevExpress.DataAccess.Sql.SqlDataSource GenerateSqlDataSource()
        {
            DevExpress.DataAccess.Sql.SqlDataSource result = new DevExpress.DataAccess.Sql.SqlDataSource("crmConnectionString");

            //QUATATION REGISTER
            DataTable dtRptTables = new DataTable();
            string query = @"Select Query_Table_name from tbl_trans_ReportSql where Module_name = 'PIQUOTATION' order by Query_ID ";
            //BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine();
            dtRptTables = oDbEngine.GetDataTable(query);
            //dtRptTables.TableName = "aaa";
            foreach (DataRow row in dtRptTables.Rows)
            {
                result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), rptData.GenerateSqlDataSource(Convert.ToString(row[0]))));
            }

            DataTable dtRptRelation = new DataTable();
            string RelationQuery = @"Select Parent_Query_name,Child_Query_name, Parent_Column_name,Child_Column_name from tbl_trans_ReportTableRelation where Module_name = 'PIQUOTATION' order by Query_ID ";
            dtRptRelation = oDbEngine.GetDataTable(RelationQuery);
            foreach (DataRow row in dtRptRelation.Rows)
            {
                result.Relations.Add(Convert.ToString(row[0]), Convert.ToString(row[1]), Convert.ToString(row[2]), Convert.ToString(row[3]));
            }

            result.RebuildResultSchema();
            return result;
        }

        // Save a report to a file.
        protected void ASPxReportDesigner1_SaveReportLayout(object sender, DevExpress.XtraReports.Web.SaveReportLayoutEventArgs e)
        {
            string FileName = "";
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["NewReport"]))
            {
                FileName = HttpContext.Current.Request.QueryString["NewReport"] + "PInvQuot~N";
            }
            else if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["LoadrptName"]))
            {
                FileName = HttpContext.Current.Request.QueryString["LoadrptName"];
            }                
            XtraReport newXtraReport = new XtraReport();
            string filePath = Server.MapPath("/oms/Reports/RepxReportDesign/" + FileName + ".repx");
            var bytarr = e.ReportLayout;
            Stream stream = new MemoryStream(bytarr);
            newXtraReport.LoadLayout(stream);
            newXtraReport.SaveLayout(filePath);            
        }
    }
}