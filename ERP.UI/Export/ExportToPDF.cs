using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.OMS.Reports.XtraReports;
using DevExpress.DataAccess.Sql;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Preview;
using System.Net.Mail;
using DevExpress.DataAccess.ConnectionParameters;
using System.Data.SqlClient;

namespace ERP.Export
{
    public class ExportToPDF
    {

        public void ExportToPdfforEmail(string rptName, string RptModuleName, String mapPath, string PrintType, string DocumentID)
        {
            string filePath = "";
            string Module_name = "";
            string DesignPath = "";
            string PDFFilePath = "";
            string filePathtoPDF = "";
            string ReportType = "";

            #region create Path
            if (RptModuleName == "Invoice")
            {

                Module_name = "SALETAX";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\SalesInvoice\DocDesign\Normal\";
                    PDFFilePath = @"Reports\Reports\RepxReportDesign\SalesInvoice\DocDesign\PDFFiles\";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\SalesInvoice\DocDesign\Normal\";
                    PDFFilePath = @"Reports\RepxReportDesign\SalesInvoice\DocDesign\PDFFiles\";
                }
            }
            else if (RptModuleName == "Invoice_POS")
            {

                Module_name = "SALETAX";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\SalesInvoice\DocDesign\SPOS\";
                    PDFFilePath = @"Reports\Reports\RepxReportDesign\SalesInvoice\DocDesign\PDFFiles\";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\SalesInvoice\DocDesign\SPOS\";
                    PDFFilePath = @"Reports\RepxReportDesign\SalesInvoice\DocDesign\PDFFiles\";
                }
            }
            else if (RptModuleName == "TSInvoice")
            {
                Module_name = "SALETAX";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\SalesInvoice\DocDesign\Transit\";
                    PDFFilePath = @"Reports\Reports\RepxReportDesign\SalesInvoice\DocDesign\PDFFiles\";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\SalesInvoice\DocDesign\Transit\";
                    PDFFilePath = @"Reports\RepxReportDesign\SalesInvoice\DocDesign\PDFFiles\";
                }
            }
            else if (RptModuleName == "Install_Coupon")
            {

                Module_name = "INSCUPN";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\SalesInvoice\DocDesign\InstCoupon\";
                    PDFFilePath = @"Reports\Reports\RepxReportDesign\SalesInvoice\DocDesign\PDFFiles\";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\SalesInvoice\DocDesign\InstCoupon\";
                    PDFFilePath = @"Reports\RepxReportDesign\SalesInvoice\DocDesign\PDFFiles\";
                }
            }
            else if (RptModuleName == "BranchReq")
            {

                Module_name = "BRANCHREQ";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\BranchRequisition\DocDesign\Designes\";
                    PDFFilePath = @"Reports\Reports\RepxReportDesign\BranchRequisition\DocDesign\PDFFiles\";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\BranchRequisition\DocDesign\Designes\";
                    PDFFilePath = @"Reports\RepxReportDesign\BranchRequisition\DocDesign\PDFFiles\";
                }
            }
            else if (RptModuleName == "Porder")
            {

                Module_name = "PORDER";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\PurchaseOrder\DocDesign\Designes\";
                    PDFFilePath = @"Reports\Reports\RepxReportDesign\PurchaseOrder\DocDesign\PDFFiles\";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\PurchaseOrder\DocDesign\Designes\";
                    PDFFilePath = @"Reports\RepxReportDesign\PurchaseOrder\DocDesign\PDFFiles\";
                }
            }
            else if (RptModuleName == "Sorder")
            {

                Module_name = "SORDER";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\SalesOrder\DocDesign\Designes\";
                    PDFFilePath = @"Reports\Reports\RepxReportDesign\SalesOrder\DocDesign\PDFFiles\";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\SalesOrder\DocDesign\Designes\";
                    PDFFilePath = @"Reports\RepxReportDesign\SalesOrder\DocDesign\PDFFiles\";
                }
            }
            else if (RptModuleName == "PIQuotation")
            {
                Module_name = "PIQUOTATION";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\Proforma\DocDesign\Designes\";
                    PDFFilePath = @"Reports\Reports\RepxReportDesign\Proforma\DocDesign\PDFFiles\";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\Proforma\DocDesign\Designes\";
                    PDFFilePath = @"Reports\RepxReportDesign\Proforma\DocDesign\PDFFiles\";
                }
            }
            #endregion

            HttpContext.Current.Session["Module_Name"] = Module_name;
            string fullpath = mapPath;
            fullpath = fullpath.Replace("ERP.UI\\", "");
            string DesignFullPath = fullpath + DesignPath;
            filePath = System.IO.Path.GetDirectoryName(DesignFullPath);
            filePath = filePath + "\\" + rptName + ".repx";


            DevExpress.DataAccess.Sql.SqlDataSource sql = GenerateSqlDataSource(RptModuleName, DocumentID);
            XtraReport newXtraReport = XtraReport.FromFile(filePath, true);
            newXtraReport.LoadLayout(filePath);
            newXtraReport.DataSource = sql;
            filePathtoPDF = filePath;
            filePathtoPDF = filePathtoPDF.Split('~')[0];

            if (RptModuleName == "Invoice" || RptModuleName == "Invoice_POS" || RptModuleName == "TSInvoice")
            {
                if (PrintType == "1")
                {
                    ReportType = "Original";
                }
                else if (PrintType == "2")
                {
                    ReportType = "Duplicate";
                }
                else
                {
                    ReportType = "Triplicate";
                }
                
                if (RptModuleName == "Invoice")
                {
                    filePathtoPDF = filePathtoPDF.Replace("Normal", "PDFFiles");
                }
                else if (RptModuleName == "TSInvoice")
                {
                    filePathtoPDF = filePathtoPDF.Replace("\\Transit\\", "\\PDFFiles\\");
                }
                else
                {
                    filePathtoPDF = filePathtoPDF.Replace("SPOS", "PDFFiles");
                }
            }
            else if (RptModuleName == "Install_Coupon")
            {
                filePathtoPDF = filePathtoPDF.Replace("InstCoupon", "PDFFiles");
            }
            else if (RptModuleName == "BranchReq" || RptModuleName == "Porder" || RptModuleName == "Sorder" || RptModuleName == "PIQuotation")
            {
                filePathtoPDF = filePathtoPDF.Replace("Designes", "PDFFiles");
            }
            else
            {
                filePathtoPDF = filePathtoPDF + ".pdf";
            }

            if (RptModuleName == "Invoice" || RptModuleName == "Invoice_POS" || RptModuleName == "TSInvoice")
            {
                filePathtoPDF = filePathtoPDF + "-" + ReportType + "-" + DocumentID + ".pdf";
            }
            else
            {
                filePathtoPDF = filePathtoPDF + "-" + DocumentID + ".pdf";
            }

            newXtraReport.ExportToPdf(filePathtoPDF);
        }


        private DevExpress.DataAccess.Sql.SqlDataSource GenerateSqlDataSource(String RptModuleName, string DocumentID)
        {
            CustomStringConnectionParameters connectionParameters = new CustomStringConnectionParameters(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));

            DevExpress.DataAccess.Sql.SqlDataSource result = new DevExpress.DataAccess.Sql.SqlDataSource(connectionParameters);
            //BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
            BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine();

            string Module_Name = Convert.ToString(HttpContext.Current.Session["Module_Name"]);
            DataTable dtRptTables = new DataTable();
            string query = "";
            query = @"Select Query_Table_name from tbl_trans_ReportSql where Module_name = '" + Module_Name + "' order by Query_ID ";
            dtRptTables = oDbEngine.GetDataTable(query);
            string[] filePaths = new string[] { };
            string path = System.Web.HttpContext.Current.Server.MapPath("~");
            string path1 = path.Replace("Reports\\", "ERP.UI");
            string fullpath = path1.Replace("\\", "/");
         

            if (RptModuleName == "Invoice" || RptModuleName == "Invoice_POS")
            {
                string PrintOption = HttpContext.Current.Request.QueryString["PrintOption"];

                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_TAXINVOICE '" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "','" + Convert.ToString(HttpContext.Current.Session["LastFinYear"]) + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "S" + "','" + PrintOption + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "TSInvoice" || RptModuleName == "TPInvoice")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_TRANSITSALEPURCHASE_REPORT '" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "','" + Convert.ToString(HttpContext.Current.Session["LastFinYear"]).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "S" + "','" + "" + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "Install_Coupon")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_INSTALLATIONCOUPON_REPORT '" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "','" + fullpath + "','" + Convert.ToString(HttpContext.Current.Session["LastFinYear"]) + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "BranchReq")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_BRANCHREQ_REPORT '" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "','" + Convert.ToString(HttpContext.Current.Session["LastFinYear"]) + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "Porder")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_SALEPURCHASEORDER_REPORT '" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "','" + Convert.ToString(HttpContext.Current.Session["LastFinYear"]) + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "P" + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "Sorder")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_SALEPURCHASEORDER_REPORT '" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "','" + Convert.ToString(HttpContext.Current.Session["LastFinYear"]) + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "S" + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "PIQuotation")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_PROFORMAINVQUOTATION '" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "','" + Convert.ToString(HttpContext.Current.Session["LastFinYear"]) + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "P" + "'"));
                }
            }

            DataTable dtRptRelation = new DataTable();
            string RelationQuery = "";
            RelationQuery = @"Select Parent_Query_name,Child_Query_name, Parent_Column_name,Child_Column_name from tbl_trans_ReportTableRelation where Module_name = '" + Module_Name + "' order by Query_ID ";
            dtRptRelation = oDbEngine.GetDataTable(RelationQuery);
            if (dtRptRelation.Rows.Count > 0)
            {
                foreach (DataRow row in dtRptRelation.Rows)
                {
                    result.Relations.Add(Convert.ToString(row[0]), Convert.ToString(row[1]), Convert.ToString(row[2]), Convert.ToString(row[3]));
                }
            }

            result.RebuildResultSchema();
            return result;
        }

        public void ExportToPdfforEmail(string rptName, string RptModuleName, String mapPath, string PrintType, string DocumentID,string dbname)
        {
            string filePath = "";
            string Module_name = "";
            string DesignPath = "";
            string PDFFilePath = "";
            string filePathtoPDF = "";
            string ReportType = "";

            #region create Path
            if (RptModuleName == "Invoice")
            {

                Module_name = "SALETAX";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\SalesInvoice\DocDesign\Normal\";
                    PDFFilePath = @"Reports\Reports\RepxReportDesign\SalesInvoice\DocDesign\PDFFiles\";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\SalesInvoice\DocDesign\Normal\";
                    PDFFilePath = @"Reports\RepxReportDesign\SalesInvoice\DocDesign\PDFFiles\";
                }
            }
            else if (RptModuleName == "Invoice_POS")
            {

                Module_name = "SALETAX";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\SalesInvoice\DocDesign\SPOS\";
                    PDFFilePath = @"Reports\Reports\RepxReportDesign\SalesInvoice\DocDesign\PDFFiles\";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\SalesInvoice\DocDesign\SPOS\";
                    PDFFilePath = @"Reports\RepxReportDesign\SalesInvoice\DocDesign\PDFFiles\";
                }
            }
            else if (RptModuleName == "TSInvoice")
            {
                Module_name = "SALETAX";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\SalesInvoice\DocDesign\Transit\";
                    PDFFilePath = @"Reports\Reports\RepxReportDesign\SalesInvoice\DocDesign\PDFFiles\";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\SalesInvoice\DocDesign\Transit\";
                    PDFFilePath = @"Reports\RepxReportDesign\SalesInvoice\DocDesign\PDFFiles\";
                }
            }
            else if (RptModuleName == "Install_Coupon")
            {

                Module_name = "INSCUPN";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\SalesInvoice\DocDesign\InstCoupon\";
                    PDFFilePath = @"Reports\Reports\RepxReportDesign\SalesInvoice\DocDesign\PDFFiles\";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\SalesInvoice\DocDesign\InstCoupon\";
                    PDFFilePath = @"Reports\RepxReportDesign\SalesInvoice\DocDesign\PDFFiles\";
                }
            }
            else if (RptModuleName == "BranchReq")
            {

                Module_name = "BRANCHREQ";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\BranchRequisition\DocDesign\Designes\";
                    PDFFilePath = @"Reports\Reports\RepxReportDesign\BranchRequisition\DocDesign\PDFFiles\";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\BranchRequisition\DocDesign\Designes\";
                    PDFFilePath = @"Reports\RepxReportDesign\BranchRequisition\DocDesign\PDFFiles\";
                }
            }
            else if (RptModuleName == "Porder")
            {

                Module_name = "PORDER";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\PurchaseOrder\DocDesign\Designes\";
                    PDFFilePath = @"Reports\Reports\RepxReportDesign\PurchaseOrder\DocDesign\PDFFiles\";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\PurchaseOrder\DocDesign\Designes\";
                    PDFFilePath = @"Reports\RepxReportDesign\PurchaseOrder\DocDesign\PDFFiles\";
                }
            }
            else if (RptModuleName == "Sorder")
            {

                Module_name = "SORDER";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\SalesOrder\DocDesign\Designes\";
                    PDFFilePath = @"Reports\Reports\RepxReportDesign\SalesOrder\DocDesign\PDFFiles\";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\SalesOrder\DocDesign\Designes\";
                    PDFFilePath = @"Reports\RepxReportDesign\SalesOrder\DocDesign\PDFFiles\";
                }
            }
            else if (RptModuleName == "PIQuotation")
            {
                Module_name = "PIQUOTATION";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\Proforma\DocDesign\Designes\";
                    PDFFilePath = @"Reports\Reports\RepxReportDesign\Proforma\DocDesign\PDFFiles\";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\Proforma\DocDesign\Designes\";
                    PDFFilePath = @"Reports\RepxReportDesign\Proforma\DocDesign\PDFFiles\";
                }
            }
            #endregion

            HttpContext.Current.Session["Module_Name"] = Module_name;
            string fullpath = mapPath;
            fullpath = fullpath.Replace("ERP.UI\\", "");
            string DesignFullPath = fullpath + DesignPath;
            filePath = System.IO.Path.GetDirectoryName(DesignFullPath);
            filePath = filePath + "\\" + rptName + ".repx";


            DevExpress.DataAccess.Sql.SqlDataSource sql = GenerateSqlDataSource(RptModuleName, DocumentID,dbname);
            XtraReport newXtraReport = XtraReport.FromFile(filePath, true);
            newXtraReport.LoadLayout(filePath);
            newXtraReport.DataSource = sql;
            filePathtoPDF = filePath;
            filePathtoPDF = filePathtoPDF.Split('~')[0];

            if (RptModuleName == "Invoice" || RptModuleName == "Invoice_POS" || RptModuleName == "TSInvoice")
            {
                if (PrintType == "1")
                {
                    ReportType = "Original";
                }
                else if (PrintType == "2")
                {
                    ReportType = "Duplicate";
                }
                else
                {
                    ReportType = "Triplicate";
                }

                if (RptModuleName == "Invoice")
                {
                    filePathtoPDF = filePathtoPDF.Replace("Normal", "PDFFiles");
                }
                else if (RptModuleName == "TSInvoice")
                {
                    filePathtoPDF = filePathtoPDF.Replace("\\Transit\\", "\\PDFFiles\\");
                }
                else
                {
                    filePathtoPDF = filePathtoPDF.Replace("SPOS", "PDFFiles");
                }
            }
            else if (RptModuleName == "Install_Coupon")
            {
                filePathtoPDF = filePathtoPDF.Replace("InstCoupon", "PDFFiles");
            }
            else if (RptModuleName == "BranchReq" || RptModuleName == "Porder" || RptModuleName == "Sorder" || RptModuleName == "PIQuotation")
            {
                filePathtoPDF = filePathtoPDF.Replace("Designes", "PDFFiles");
            }
            else
            {
                filePathtoPDF = filePathtoPDF + ".pdf";
            }

            if (RptModuleName == "Invoice" || RptModuleName == "Invoice_POS" || RptModuleName == "TSInvoice")
            {
                filePathtoPDF = filePathtoPDF + "-" + ReportType + "-" + DocumentID + ".pdf";
            }
            else
            {
                filePathtoPDF = filePathtoPDF + "-" + DocumentID + ".pdf";
            }

            newXtraReport.ExportToPdf(filePathtoPDF);
        }


        private DevExpress.DataAccess.Sql.SqlDataSource GenerateSqlDataSource(String RptModuleName, string DocumentID,string dbanem)
        {
            CustomStringConnectionParameters connectionParameters = new CustomStringConnectionParameters(GetConnectionString(dbanem));

            DevExpress.DataAccess.Sql.SqlDataSource result = new DevExpress.DataAccess.Sql.SqlDataSource(connectionParameters);
            //BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]); MULTI
            //BusinessLogicLayer.DBEngine oDbEngine = new BusinessLogicLayer.DBEngine();

            string companyid = "";
            string finyear = "";

            DataTable dtCompany = GetDataTable("Select cmp_internalid from tbl_master_company", dbanem);
            if (dtCompany != null && dtCompany.Rows.Count > 0)
            {
                companyid = Convert.ToString(dtCompany.Rows[0][0]);
            }

            DataTable dtFinyear = GetDataTable("Select FinYear_Code from Master_FinYear where finyear_isactive=1", dbanem);
            if (dtFinyear != null && dtFinyear.Rows.Count > 0)
            {
                finyear = Convert.ToString(dtFinyear.Rows[0][0]);
            }


            string Module_Name = Convert.ToString(HttpContext.Current.Session["Module_Name"]);
            DataTable dtRptTables = new DataTable();
            string query = "";
            query = @"Select Query_Table_name from tbl_trans_ReportSql where Module_name = '" + Module_Name + "' order by Query_ID ";
            dtRptTables = GetDataTable(query, dbanem);
            string[] filePaths = new string[] { };
            string path = System.Web.HttpContext.Current.Server.MapPath("~");
            string path1 = path.Replace("Reports\\", "ERP.UI");
            string fullpath = path1.Replace("\\", "/");


            if (RptModuleName == "Invoice" || RptModuleName == "Invoice_POS")
            {
                string PrintOption = HttpContext.Current.Request.QueryString["PrintOption"];

                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_TAXINVOICE '" + Convert.ToString(companyid) + "','" + Convert.ToString(finyear) + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "S" + "','" + PrintOption + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "TSInvoice" || RptModuleName == "TPInvoice")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_TRANSITSALEPURCHASE_REPORT '" + Convert.ToString(companyid) + "','" + Convert.ToString(finyear).Trim() + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "S" + "','" + "" + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "Install_Coupon")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_INSTALLATIONCOUPON_REPORT '" + Convert.ToString(companyid) + "','" + fullpath + "','" + Convert.ToString(finyear) + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "BranchReq")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_BRANCHREQ_REPORT '" + Convert.ToString(companyid) + "','" + Convert.ToString(finyear) + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "Porder")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_SALEPURCHASEORDER_REPORT '" + Convert.ToString(companyid) + "','" + Convert.ToString(finyear) + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "P" + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "Sorder")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_SALEPURCHASEORDER_REPORT '" + Convert.ToString(companyid) + "','" + Convert.ToString(finyear) + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "S" + "','" + "P" + "'"));
                }
            }
            else if (RptModuleName == "PIQuotation")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_PROFORMAINVQUOTATION '" + Convert.ToString(companyid) + "','" + Convert.ToString(finyear) + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + DocumentID + "','" + "P" + "'"));
                }
            }

            DataTable dtRptRelation = new DataTable();
            string RelationQuery = "";
            RelationQuery = @"Select Parent_Query_name,Child_Query_name, Parent_Column_name,Child_Column_name from tbl_trans_ReportTableRelation where Module_name = '" + Module_Name + "' order by Query_ID ";
            dtRptRelation = GetDataTable(RelationQuery, dbanem);
            if (dtRptRelation.Rows.Count > 0)
            {
                foreach (DataRow row in dtRptRelation.Rows)
                {
                    result.Relations.Add(Convert.ToString(row[0]), Convert.ToString(row[1]), Convert.ToString(row[2]), Convert.ToString(row[3]));
                }
            }

            result.RebuildResultSchema();
            return result;
        }

        public string GetConnectionString(string dbName)
        {
            string Conn = "";
            string DtSource = ConfigurationSettings.AppSettings["sqlDatasource"];
            string UserId = ConfigurationSettings.AppSettings["sqlUserId"];
            string Pwd = ConfigurationSettings.AppSettings["sqlPassword"];
            string IntSq = ConfigurationSettings.AppSettings["sqlAuth"];
            string ispool = ConfigurationSettings.AppSettings["isPool"];
            string poolsize = ConfigurationSettings.AppSettings["PoolSize"];

            //string connectionString="Data Source=LABSERVER\MSSQLSERVERR2;Initial Catalog=PK24092018;Persist Security Info=True;User ID=sa;Password=sql@123";


            //if (IntSq != "Windows")
            //{
            //    Conn = "Data Source=" + DtSource + ";Initial Catalog=" + dbName + ";Persist Security Info=True;User ID=" + UserId + ";Password=" + Pwd + ";pooling=" + ispool + ";Max Pool Size=" + poolsize;
            //}
            //else
            //{
            //    Conn = "Data Source=" + DtSource + ";Initial Catalog=" + dbName + ";Integrated Security=SSPI" + ";pooling=" + ispool + ";Max Pool Size=" + poolsize;
            //}


            SqlConnectionStringBuilder connectionString = new SqlConnectionStringBuilder();
            connectionString.DataSource = DtSource;
            connectionString.InitialCatalog = dbName;
            if (IntSq == "Windows")
            {
                connectionString.IntegratedSecurity = true;
            }
            else
            {
                connectionString.PersistSecurityInfo = true;
                connectionString.IntegratedSecurity = false;
                connectionString.UserID = UserId;
                connectionString.Password = Pwd;

            }
            connectionString.ConnectTimeout = 950;
            connectionString.Pooling = Convert.ToBoolean(ispool);
            connectionString.MaxPoolSize = Convert.ToInt32(poolsize);



            string str = connectionString.ConnectionString;



            return str;
        }


        private DataTable GetDataTable(string lcSql,string dbname)
        {
            string oSql = Convert.ToString(GetConnectionString(dbname));
            SqlConnection oSqlConnection = new SqlConnection(oSql);
            SqlDataAdapter lda = new SqlDataAdapter(lcSql, oSqlConnection);
            DataTable GetTable = new DataTable();
            lda.SelectCommand.CommandTimeout = 0;
            lda.Fill(GetTable);
            oSqlConnection.Close();
            return GetTable;
        }


    }
}