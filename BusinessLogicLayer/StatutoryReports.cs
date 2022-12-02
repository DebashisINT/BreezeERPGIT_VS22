using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class StatutoryReports
    {
        public DataSet Report_RegisterOfTransaction(string ReportView, string Companyid, string Segment,
              string FromDate, string Todate, string FinYear,
           string Scrip, string Client, string ExchangeSegmentId, string TradeTime,
            string ClientsDisplay, string BranchHierchy, string BannedEntity, string SecurityType)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Report_RegisterOfTransaction");

            proc.AddVarcharPara("@ReportView", 5, ReportView);
            proc.AddVarcharPara("@Companyid", 15, Companyid);
            proc.AddVarcharPara("@Segment", 8, Segment);
            proc.AddVarcharPara("@FromDate", 35, FromDate);
            proc.AddVarcharPara("@Todate", 35, Todate);
            proc.AddVarcharPara("@FinYear", 15, FinYear);
            proc.AddVarcharPara("@Scrip", -1, Scrip);
            proc.AddVarcharPara("@Client", -1, Client);
            proc.AddVarcharPara("@ExchangeSegmentId", 8, ExchangeSegmentId);
            proc.AddVarcharPara("@TradeTime", 500, TradeTime);
            proc.AddVarcharPara("@ClientsDisplay", 25, ClientsDisplay);
            proc.AddVarcharPara("@BranchHierchy", -1, BranchHierchy);
            proc.AddVarcharPara("@BannedEntity", 10, BannedEntity);
            proc.AddVarcharPara("@SecurityType", 15, SecurityType);

            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet CLIENTSECURITY(string FINYEAR, string FROMDATE, string TODATE,
            string companyid, string CLIENTS, string BRANCHID,
         string GRPTYPE, string Groupby,string Type, string Header, string Footer,
          string ReportType, string GenerationType, string SEGMENT)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("CLIENTSECURITY");

            proc.AddVarcharPara("@FINYEAR", 50, FINYEAR);
            proc.AddVarcharPara("@FROMDATE", 35, FROMDATE);
            proc.AddVarcharPara("@TODATE", 35, TODATE);
            proc.AddVarcharPara("@companyid", 15, companyid);
            proc.AddVarcharPara("@CLIENTS", -1, CLIENTS);
            proc.AddVarcharPara("@BRANCHID", -1, BRANCHID);
            proc.AddVarcharPara("@GRPTYPE", 15, GRPTYPE);
            proc.AddVarcharPara("@Groupby", -1, Groupby);
            proc.AddVarcharPara("@Type", -1, Type);
            proc.AddVarcharPara("@Header", -1, Header);
            proc.AddVarcharPara("@Footer", -1, Footer);
            //proc.AddVarcharPara("@ReportType", 25, ReportType);
            //proc.AddVarcharPara("@GenerationType", -1, GenerationType);
            proc.AddVarcharPara("@SEGMENT", 30, SEGMENT);

            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet Report_FormNo10DB(string companyid, string fromdate, string todate,
          string clients, string Segment, string GrpType,
      string GrpId, string BranchHierchy, string FinYear, string CalType)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Report_FormNo10DB");

            proc.AddVarcharPara("@Companyid", 10, companyid);
            proc.AddVarcharPara("@fromdate", 30, fromdate);
            proc.AddVarcharPara("@todate", 30, todate);
            proc.AddVarcharPara("@clients", -1, clients);
            proc.AddVarcharPara("@Segment", 10, Segment);
            proc.AddVarcharPara("@GrpType", 40, GrpType);
            proc.AddVarcharPara("@GrpId", -1, GrpId);
            proc.AddVarcharPara("@BranchHierchy", -1, BranchHierchy);
            proc.AddVarcharPara("@FinYear", 15, FinYear);
            proc.AddVarcharPara("@CalType", 10, CalType);

            ds = proc.GetDataSet();
            return ds;
        }


    }
}
