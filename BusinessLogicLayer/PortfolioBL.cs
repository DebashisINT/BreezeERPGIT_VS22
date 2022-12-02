using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class PortfolioBL
    {
        int rtrnvalue = 0;
        ProcedureExecute proc;
        public int DeletePortFolio(int Id)
        {
            using (proc = new ProcedureExecute("DeletePortFolio"))
            {
                proc.AddIntegerPara("@ID", Id);
                rtrnvalue = proc.RunActionQuery();
            }
            return rtrnvalue;
        }

        public int InsertUpdatePortfolio(string For, string FinYear, string CustomerID, string CompanyID, int Segment,
        int Exchange, int ProductID, int SeriesID, DateTime openingDate, DateTime TradeDate, string BuyQty, string NetAvgUnitCost,
        string HostoricalCost, string SecTranTax, string HoldingMode, string ISIN, int CreateUser, string InUpType, int ID)
        {
            using (proc = new ProcedureExecute("InsertUpdatePortfolio"))
            {
                proc.AddVarcharPara("@For", 100, For);
                proc.AddVarcharPara("@FinYear", 100, FinYear);
                proc.AddVarcharPara("@CustomerID", 100, CustomerID);
                proc.AddVarcharPara("@CompanyID", 100, CompanyID);
                proc.AddIntegerPara("@Segment", Segment);
                proc.AddIntegerPara("@Exchange", Exchange);
                proc.AddIntegerPara("@ProductID", ProductID);
                proc.AddIntegerPara("@SeriesID", SeriesID);
                proc.AddVarcharPara("@openingDate", 100, openingDate.ToShortDateString());
                proc.AddVarcharPara("@TradeDate", 100, TradeDate.ToShortDateString());
                proc.AddVarcharPara("@BuyQty", 100, BuyQty);
                proc.AddVarcharPara("@NetAvgUnitCost", 100, NetAvgUnitCost);
                proc.AddVarcharPara("@HostoricalCost", 100, HostoricalCost);
                proc.AddVarcharPara("@SecTranTax", 100, SecTranTax);
                proc.AddVarcharPara("@HoldingMode", 100, HoldingMode);
                proc.AddVarcharPara("@ISIN", 100, ISIN);
                proc.AddIntegerPara("@CreateUser", CreateUser);
                proc.AddVarcharPara("@InUpType", 100, InUpType);
                proc.AddIntegerPara("@ID", ID);

                rtrnvalue = proc.RunActionQuery();
            }
            return rtrnvalue;
        }


        public DataSet PerformanceReportCM_JournalVoucher(string companyid, string segment, string fromdate, string todate, string clients,
                      string Seriesid, string finyear, string grptype, string rpttype, string PRINTCHK,
                      string clienttype, string closemethod, string jvcreate, string SEGMENTJV, string CreateUser,
            string chkexcludesqr, string chkstt, string ValuationDate, string ReportView)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PerformanceReportCM_JournalVoucher");

            proc.AddVarcharPara("@companyid", -1, companyid);
            proc.AddVarcharPara("@segment", -1, segment);
            proc.AddVarcharPara("@fromdate", 50, fromdate);
            proc.AddVarcharPara("@todate", 50, todate);
            proc.AddVarcharPara("@clients", -1, clients);
            proc.AddVarcharPara("@Seriesid", -1, Seriesid);
            proc.AddVarcharPara("@finyear", 50, finyear);
            proc.AddVarcharPara("@grptype", 50, grptype);
            proc.AddVarcharPara("@rpttype", 10, rpttype);
            proc.AddVarcharPara("@PRINTCHK", 200, PRINTCHK);
            proc.AddVarcharPara("@clienttype", 500, clienttype);
            proc.AddVarcharPara("@closemethod", 10, closemethod);
            proc.AddVarcharPara("@jvcreate", 20, jvcreate);
            proc.AddVarcharPara("@SEGMENTJV", 50, SEGMENTJV);
            proc.AddVarcharPara("@CreateUser", 35, CreateUser);
            proc.AddVarcharPara("@chkexcludesqr", 10, chkexcludesqr);
            proc.AddVarcharPara("@chkstt", 10, chkstt);
            proc.AddVarcharPara("@ValuationDate", 35, ValuationDate);
            proc.AddCharPara("@ReportView", 1, Convert.ToChar(ReportView));

            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet PerformanceReportCM(string companyid, string segment, string fromdate, string todate, string clients,
                      string Seriesid, string finyear, string grptype, string rpttype, string PRINTCHK,
                      string clienttype, string closemethod, string chkexcludesqr, string chkstt, string ValuationDate, string ReportView)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PerformanceReportCM");

            proc.AddVarcharPara("@companyid", -1, companyid);
            proc.AddVarcharPara("@segment", -1, segment);
            proc.AddVarcharPara("@fromdate", 50, fromdate);
            proc.AddVarcharPara("@todate", 50, todate);
            proc.AddVarcharPara("@clients", -1, clients);
            proc.AddVarcharPara("@Seriesid", -1, Seriesid);
            proc.AddVarcharPara("@finyear", 50, finyear);
            proc.AddVarcharPara("@grptype", 50, grptype);
            proc.AddVarcharPara("@rpttype", 10, rpttype);
            proc.AddVarcharPara("@PRINTCHK", 200, PRINTCHK);
            proc.AddVarcharPara("@clienttype", 500, clienttype);
            proc.AddVarcharPara("@closemethod", 10, closemethod);
            proc.AddVarcharPara("@chkexcludesqr", 10, chkexcludesqr);
            proc.AddVarcharPara("@chkstt", 10, chkstt);
            proc.AddVarcharPara("@ValuationDate", 35, ValuationDate);
            proc.AddCharPara("@ReportView", 1, Convert.ToChar(ReportView));

            ds = proc.GetDataSet();
            return ds;
        }
    }
}
