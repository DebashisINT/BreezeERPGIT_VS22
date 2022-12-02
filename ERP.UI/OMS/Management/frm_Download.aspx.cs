using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using CrystalDecisions.CrystalReports.Engine;

using BusinessLogicLayer;


namespace ERP.OMS.Management
{
    public partial class management_frm_Download : System.Web.UI.Page
    {
        string batch_print;
        string slip_print;
        string type;
        string seg;
        string pdfName;
        string rptName;
        string AccountID;
        string Date;

        string spname;

        Management_BL ObjMan = new Management_BL();


        protected void Page_Load(object sender, EventArgs e)
        {
            batch_print = Request.QueryString["b"];
            slip_print = Request.QueryString["s"];
            type = Request.QueryString["t"];
            seg = Request.QueryString["seg"];
            AccountID = Request.QueryString["ACID"];
            Date = Request.QueryString["Date"];

            if (Request.QueryString["partype"].ToString().Trim() == "'Spot'")
            {
                spname = "[Report_DematTransactionCommCurrencyPrint]";
            }
            else
            {
                spname = "[sp_CdslDematTransaction_Report]";
            }
            PrintBatch(batch_print, slip_print, type, seg, AccountID, Date, spname.ToString().Trim());

        }

        protected void PrintBatch(string batch_print, string slip_print, string type, string exch_seg_id, string AccountID, string Date, string spname)
        {
            DataSet dsDemat = new DataSet();
            /*

            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            {
                using (SqlDataAdapter da = new SqlDataAdapter(spname.ToString().Trim(), con))
                {

                    da.SelectCommand.Parameters.AddWithValue("batch", batch_print);
                    da.SelectCommand.Parameters.AddWithValue("slip", slip_print);
                    da.SelectCommand.Parameters.AddWithValue("type", type);
                    da.SelectCommand.Parameters.AddWithValue("ExchSegId", exch_seg_id);
                    if (Request.QueryString["partype"].ToString().Trim() != "'Spot'")
                    {
                        da.SelectCommand.Parameters.AddWithValue("AccountID", Convert.ToInt32(AccountID));
                        da.SelectCommand.Parameters.AddWithValue("TransDate", Convert.ToDateTime(Date));
                    }

                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.CommandTimeout = 0;

                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    dsDemat.Reset();
                    da.Fill(dsDemat);

                }

            }

            */

            dsDemat = ObjMan.Get_Download(batch_print, slip_print, type, exch_seg_id, AccountID, Date, spname, Request.QueryString["partype"].ToString().Trim());


            //dsDemat.WriteXmlSchema("D:\\Demat.xsd");

            ReportDocument DematCdslReportDocu = new ReportDocument();

            if (type == "InterDP-CDSL")
            {
                rptName = "..\\Reports\\CDSL_Demat.rpt";
                pdfName = "CDSL-InterDepository";
            }
            else if (type == "OffMarket-CDSL")
            {
                rptName = "..\\Reports\\CDSL_Demat.rpt";
                pdfName = "CDSL-OffMarket";
            }
            else if (type == "Combined-CDSL")
            {
                rptName = "..\\Reports\\CDSL_Demat.rpt";
                pdfName = "CDSL-Combined(InterDp/OffMkt)";
            }
            else if (type == "POA-CDSL")
            {
                rptName = "..\\Reports\\POA_Demat.rpt";
                pdfName = "POA-CDSL";
            }
            else if (type == "POA-NSDL")
            {
                rptName = "..\\Reports\\POA_Demat.rpt";
                pdfName = "POA-NSDL";
            }
            else if (type == "IntraDP-NSDL")
            {
                rptName = "..\\Reports\\NSDL_Demat.rpt";
                pdfName = "NSDL-IntraDepository";
            }
            else if (type == "InterDP-NSDL")
            {
                rptName = "..\\Reports\\NSDL_Demat.rpt";
                pdfName = "NSDL-InterDepository";
            }
            else if (type == "InterSettlement-NSDL")
            {
                rptName = "..\\Reports\\InterSettlement_Demat.rpt";
                pdfName = "NSDL-InterSettlement";
            }
            else if (type == "EarlyPayIn")
            {
                rptName = "..\\Reports\\InterSettlement_Demat.rpt";
                pdfName = "NSDL-EarlyPayIn";
            }

            string reportPath = HttpContext.Current.Server.MapPath(rptName);

            DematCdslReportDocu.Load(reportPath);
            DematCdslReportDocu.SetDataSource(dsDemat.Tables[0]);


            DematCdslReportDocu.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, pdfName);

            DematCdslReportDocu.Dispose();
            DematCdslReportDocu = null;
            GC.Collect();
        }
    }
}
