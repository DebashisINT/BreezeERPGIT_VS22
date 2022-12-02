using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class MISReports
    {                  
        public DataSet Report_ComMISReport(string Module, string NoOfClients,
           string dateFrom, string dateTo, string segment,
           string Companyid, string FinancialYear)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Report_ComMISReport");

            proc.AddVarcharPara("@Module", 100, Module);
            proc.AddVarcharPara("@NoOfClients", -1, NoOfClients);
            proc.AddVarcharPara("@dateFrom",100, dateFrom);
            proc.AddVarcharPara("@dateTo", 100, dateTo);
            proc.AddVarcharPara("@segment", 100, segment);
            proc.AddVarcharPara("@Companyid", 100, Companyid);
            proc.AddVarcharPara("@FinancialYear",100, FinancialYear);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet Report_ComMISReport_Inactive(int days, string segment, string Companyid)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Report_ComMISReport_Inactive");

            proc.AddIntegerPara("@days", days);
            proc.AddVarcharPara("@segment", 100, segment);
            proc.AddVarcharPara("@Companyid", 100, Companyid);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet Report_ComMISReport_Active(string Module, string NoOfClients,
            string dateFrom, string dateTo,string Percentage, string segment,
            string Companyid, string FinancialYear)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Report_ComMISReport_Active");

            proc.AddVarcharPara("@Module", 20, Module);
            proc.AddVarcharPara("@NoOfClients", -1, NoOfClients);
            proc.AddVarcharPara("@dateFrom", 35, dateFrom);
            proc.AddVarcharPara("@dateTo", 35, dateTo);
            proc.AddVarcharPara("@Percentage", 100, Percentage);
            proc.AddVarcharPara("@segment", 40, segment);
            proc.AddVarcharPara("@Companyid", 15, Companyid);
            proc.AddVarcharPara("@FinancialYear", 15, FinancialYear);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet Report_TerminalIdWiseTot_CommCurrency(string CommandText, string FromDate, string ToDate,
          string BranchHierchy, string ClientId, string GrpType, string Grpid,
          string Segment, string Companyid, string Instrument, string UserId,
            string ParameterFeild,string ReportType)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute(CommandText);

            proc.AddVarcharPara("@FromDate", 35, FromDate);
            proc.AddVarcharPara("@ToDate", 35, ToDate);
            proc.AddVarcharPara("@BranchHierchy", -1, BranchHierchy);
            proc.AddVarcharPara("@ClientId", -1, ClientId);
            proc.AddVarcharPara("@GrpType", 25, GrpType);
            proc.AddVarcharPara("@Grpid", -1, Grpid);
            proc.AddVarcharPara("@Segment", 20, Segment);
            proc.AddVarcharPara("@Companyid", 15, Companyid);
            proc.AddVarcharPara("@Instrument", -1, Instrument);
            proc.AddVarcharPara("@UserId", -1, UserId);
            proc.AddVarcharPara("@ParameterFeild", -1, ParameterFeild);
            proc.AddVarcharPara("@ReportType", 5, ReportType);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet Report_RateDateFetch(string Segment, string Companyid)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Report_RateDateFetch");

            proc.AddVarcharPara("@segment", 30, Segment);
            proc.AddVarcharPara("@Companyid", 15, Companyid);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet Report_ClientRisk(string ClientId, string BranchHierchy, string Segment, string GrpType, string GrpId,
            string Companyid, string FinYear, string RptType, string ApplyHaircut, string PendingPurSalevalueMethod,
            string OnlyShortageClient,string ShortageMoreThanType,string ShortageMoreThan, string ColumnMrgnHldbkDpStocks, 
            string ColumnPndgSales, string ColumnPndgPur,string ColumnAppMargin, string ColumnShortExcess, string IncludeDP)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Report_ClientRisk");
            proc.AddVarcharPara("@ClientId", -1, ClientId);
            proc.AddVarcharPara("@BranchHierchy", -1, BranchHierchy);
            proc.AddVarcharPara("@Segment", 30, Segment);
            proc.AddVarcharPara("@GrpType", 25, GrpType);
            proc.AddVarcharPara("@GrpId", -1, GrpId);
            proc.AddVarcharPara("@Companyid", 15, Companyid);
            proc.AddVarcharPara("@FinYear", 15, FinYear);
            proc.AddVarcharPara("@RptType", 10, RptType);
            proc.AddVarcharPara("@ApplyHaircut", 10, ApplyHaircut);
            proc.AddVarcharPara("@PendingPurSalevalueMethod", 6, PendingPurSalevalueMethod);
            proc.AddVarcharPara("@OnlyShortageClient", 6, OnlyShortageClient);
            proc.AddVarcharPara("@ShortageMoreThanType", 15, ShortageMoreThanType);
            proc.AddDecimalPara("@ShortageMoreThan",6, 28,Convert.ToDecimal(ShortageMoreThan));
            proc.AddVarcharPara("@ColumnMrgnHldbkDpStocks", 3, ColumnMrgnHldbkDpStocks);
            proc.AddVarcharPara("@ColumnPndgSales", 3, ColumnPndgSales);
            proc.AddVarcharPara("@ColumnPndgPur", 3, ColumnPndgPur);
            proc.AddVarcharPara("@ColumnAppMargin", 3, ColumnAppMargin);
            proc.AddVarcharPara("@ColumnShortExcess", 3, ColumnShortExcess);
            proc.AddCharPara("@IncludeDP", 1,Convert.ToChar(IncludeDP));
            ds = proc.GetDataSet();
            return ds;
        }


        public DataSet Report_MISReport(string Module, string NoOfClients,
          string dateFrom, string dateTo, string segment,
          string Companyid, string FinancialYear, string BRANCHID)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Report_MISReport");

            proc.AddVarcharPara("@Module", 100, Module);
            proc.AddVarcharPara("@NoOfClients", -1, NoOfClients);
            proc.AddVarcharPara("@dateFrom", 100, dateFrom);
            proc.AddVarcharPara("@dateTo", 100, dateTo);
            proc.AddVarcharPara("@segment", 100, segment);
            proc.AddVarcharPara("@Companyid", 100, Companyid);
            proc.AddVarcharPara("@FinancialYear", 100, FinancialYear);
            proc.AddVarcharPara("@BRANCHID", -1, BRANCHID); 
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet Report_MISReport_Inactive(int days, string segment, string Companyid)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Report_MISReport_Inactive");

            proc.AddIntegerPara("@days", days);
            proc.AddVarcharPara("@segment", 100, segment);
            proc.AddVarcharPara("@Companyid", 100, Companyid);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet Report_MISReport_Active(string Module, string NoOfClients,
       string dateFrom, string dateTo, string Percentage, string segment,
       string Companyid, string FinancialYear)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Report_MISReport_Active");

            proc.AddVarcharPara("@Module", 20, Module);
            proc.AddVarcharPara("@NoOfClients", -1, NoOfClients);
            proc.AddVarcharPara("@dateFrom", 35, dateFrom);
            proc.AddVarcharPara("@dateTo", 35, dateTo);
            proc.AddVarcharPara("@Percentage", 100, Percentage);
            proc.AddVarcharPara("@segment", 40, segment);
            proc.AddVarcharPara("@Companyid", 15, Companyid);
            proc.AddVarcharPara("@FinancialYear", 15, FinancialYear);
            ds = proc.GetDataSet();
            return ds;
        }
    }
}
