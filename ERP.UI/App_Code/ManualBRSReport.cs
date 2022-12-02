using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportSource;
using System.Runtime.Serialization;
using System.IO;
using System.Drawing.Imaging;
using System.Data.SqlClient;

/// <summary>
/// Summary description for ManualBRSReport
/// </summary>
public class ManualBRSReport
{
    public ManualBRSReport()
    {
        //
        // TODO: Add constructor logic here
        //

    }
    public void ViewReport(DataTable DT)
    {
        //DataTable DTBL = new DataTable();
        //DTBL = DT;
        ////DT.Copy(DTBL);
        //DT.WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\ManualBRS.xsd");
        ReportDocument BRSReportDocument = new ReportDocument();
        string ReportPath = HttpContext.Current.Server.MapPath("..\\Reports\\ManualBRSReport.rpt");
        BRSReportDocument.Load(ReportPath);
        BRSReportDocument.SetDataSource(DT);
        BRSReportDocument.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "ICEX Contract Report");
    }
}
