using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Sql;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace ERP.Export
{
    public class PayslipExport
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        public void ExportToPdfforEmail(string rptName, string RptModuleName, String mapPath, string EmployeeCode, string StructureID, string YYMM, string Physical_Path)
        {
            string filePath = "";
            string Module_name = "";
            string DesignPath = "";
            string PDFFilePath = "";
            string filePathtoPDF = "";
            string ReportType = "";

            #region create Path
            if (RptModuleName == "PAYSLIP")
            {
                Module_name = "Payslip";
                if (ConfigurationManager.AppSettings["IsDevelopedZone"] != null)
                {
                    DesignPath = @"Reports\Reports\RepxReportDesign\Payslip\DocDesign\Designes\";
                    PDFFilePath = @"Reports\Reports\RepxReportDesign\Payslip\DocDesign\PDFFiles\";
                }
                else
                {
                    DesignPath = @"Reports\RepxReportDesign\Payslip\DocDesign\Designes\";
                    PDFFilePath = @"Reports\RepxReportDesign\Payslip\DocDesign\PDFFiles\";
                }
            }
            #endregion

            HttpContext.Current.Session["Module_Name"] = Module_name;
            string fullpath = mapPath;
            fullpath = fullpath.Replace("ERP.UI\\", "");
            string DesignFullPath = fullpath + DesignPath;
            filePath = System.IO.Path.GetDirectoryName(DesignFullPath);
            filePath = filePath + "\\" + rptName + ".repx";


            DevExpress.DataAccess.Sql.SqlDataSource sql = GenerateSqlDataSource(RptModuleName, EmployeeCode, StructureID, YYMM);
            XtraReport newXtraReport = XtraReport.FromFile(filePath, true);
            newXtraReport.LoadLayout(filePath);
            newXtraReport.DataSource = sql;
            filePathtoPDF = filePath;
            filePathtoPDF = filePathtoPDF.Split('~')[0];
            string strPassword = "";
            DataTable dtPassword = oDBEngine.GetDataTable("Select EmpPassword From proll_PayslipPassword where Empcode='"+EmployeeCode+"' and YYMM='"+YYMM+"' ");
            if (dtPassword != null && dtPassword.Rows.Count > 0)
            {
                strPassword = Convert.ToString(dtPassword.Rows[0]["EmpPassword"]);
            }
            else
            {
                 strPassword = Convert.ToString(Guid.NewGuid());

                 DataTable ds = new DataTable();
                 DataAccessLayer.ProcedureExecute proc = new DataAccessLayer.ProcedureExecute("prc_GetPayslipData");
                 proc.AddVarcharPara("@Action", 100, "SavePassword");
                 proc.AddVarcharPara("@EmployeeCode", 50, EmployeeCode);
                 proc.AddVarcharPara("@Password", 500, strPassword);
                 proc.AddVarcharPara("@StructureID", 500, StructureID);
                 proc.AddVarcharPara("@YYMM", 50, YYMM);
                 proc.GetTable();
                 
            }
          


            ///
            newXtraReport.ExportOptions.Pdf.PasswordSecurityOptions.PermissionsOptions.EnableCopying = false;
            newXtraReport.ExportOptions.Pdf.PasswordSecurityOptions.PermissionsOptions.ChangingPermissions = DevExpress.XtraPrinting.ChangingPermissions.None;
            newXtraReport.ExportOptions.Pdf.PasswordSecurityOptions.PermissionsPassword = strPassword;
            newXtraReport.ExportOptions.Pdf.PasswordSecurityOptions.OpenPassword = strPassword;

            filePathtoPDF = Physical_Path;

            newXtraReport.ExportToPdf(filePathtoPDF);
        }


        private DevExpress.DataAccess.Sql.SqlDataSource GenerateSqlDataSource(String RptModuleName, string EmpCode, String StructureId, String YYMM)
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


            if (RptModuleName == "PAYSLIP")
            {
                foreach (DataRow row in dtRptTables.Rows)
                {
                    //Mantis Issue 24459
                    //result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_PAYSLIP '" + Convert.ToString(row[0]) + "','" + Convert.ToString(YYMM).Trim() + "','" + Convert.ToString(StructureId).Trim() + "','" + Convert.ToString(EmpCode).Trim() + "'"));
                    result.Queries.Add(new CustomSqlQuery(Convert.ToString(row[0]), "EXEC PROC_PAYSLIP '" + Convert.ToString(HttpContext.Current.Session["LastCompany"]) + "','" + fullpath + "','" + Convert.ToString(row[0]) + "','" + Convert.ToString(YYMM).Trim() + "','" + Convert.ToString(EmpCode).Trim() + "'"));
                    //End of Mantis ISsue 24459
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


    }
}