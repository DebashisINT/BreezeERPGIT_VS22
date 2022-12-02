using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccessLayer;
using System.Data.SqlClient;
using System.Configuration;
namespace BusinessLogicLayer
{
    public class PeriodicalReports
    {
        public DataSet BokerageChargesStatementALL(string fromdate, string todate, string Segment, string CompanyId, string GrpType, string GrpId,
           string BranchHierchy, string RptView, string ChkConsolidate, string ClientId, string SettType)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("BokerageChargesStatementALL");

            proc.AddVarcharPara("@fromdate", 30, fromdate);
            proc.AddVarcharPara("@todate", 30, todate);
            proc.AddVarcharPara("@Segment", 30, Segment);
            proc.AddVarcharPara("@CompanyId", 200, CompanyId);
            proc.AddVarcharPara("@GrpType", 40, GrpType);
            proc.AddVarcharPara("@GrpId", -1, GrpId);
            proc.AddVarcharPara("@BranchHierchy", -1, BranchHierchy);
            proc.AddVarcharPara("@RptView", 2, RptView);
            proc.AddVarcharPara("@ChkConsolidate", 2, ChkConsolidate);
            proc.AddVarcharPara("@ClientId", -1, ClientId);
            proc.AddVarcharPara("@SettType", 15, SettType);
            ds = proc.GetDataSet();
            return ds;
        }


        public DataSet BokerageChargesStatement_All(string CommandText, string fromdate, string todate,
            string Broker, string ClientId, string Asset, string TerminalId,
            string Segment, string companyid, string GrpType, string GrpId,
           string BranchHierchy, string RptView, string ChkConsolidate,
            string ChkConsolidateSegmentScrip, string TopRecord,
            string OrderBy, string Filter,
           string SettType)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute(CommandText);

            proc.AddVarcharPara("@Fromdate", 30, fromdate);
            proc.AddVarcharPara("@ToDate", 30, todate);
            proc.AddVarcharPara("@Broker", 5, Broker);
            proc.AddVarcharPara("@ClientId", -1, ClientId);
            proc.AddVarcharPara("@Asset", -1, Asset);
            proc.AddVarcharPara("@TerminalId", -1, TerminalId);
            proc.AddVarcharPara("@Segment", 30, Segment);
            proc.AddVarcharPara("@companyid", 30, companyid);
            proc.AddVarcharPara("@GrpType", 40, GrpType);
            proc.AddVarcharPara("@GrpId", -1, GrpId);
            proc.AddVarcharPara("@BranchHierchy", -1, BranchHierchy);
            proc.AddVarcharPara("@RptView", 2, RptView);
            proc.AddVarcharPara("@ChkConsolidate", 2, ChkConsolidate);
            proc.AddVarcharPara("@ChkConsolidateSegmentScrip", 2, ChkConsolidateSegmentScrip);
            proc.AddVarcharPara("@TopRecord", -1, TopRecord);
            proc.AddVarcharPara("@OrderBy", 2, OrderBy);
            proc.AddVarcharPara("@Filter", 15, Filter);
            proc.AddVarcharPara("@SettType", 15, SettType);



            ds = proc.GetDataSet();
            return ds;
        }


        public DataSet Report_SubBrokerageCalculationReport(string Companyid, string Segment, string FinYear,
            string Month, string FromDate, string ToDate,
          string CommissionFor, string Commission, string ReportView, string GrpType, string GrpId,
            string BranchHierchy, string ClientId)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Report_SubBrokerageCalculationReport");

            proc.AddVarcharPara("@Companyid", 10, Companyid);
            proc.AddVarcharPara("@Segment", 20, Segment);
            proc.AddVarcharPara("@FinYear", 15, FinYear);
            proc.AddVarcharPara("@Month", 20, Month);
            proc.AddVarcharPara("@FromDate", 35, FromDate);
            proc.AddVarcharPara("@ToDate", 35, ToDate);
            proc.AddVarcharPara("@CommissionFor", 30, CommissionFor);
            proc.AddVarcharPara("@Commission", -1, Commission);
            proc.AddVarcharPara("@ReportView", 20, ReportView);
            proc.AddVarcharPara("@GrpType", 40, GrpType);
            proc.AddVarcharPara("@GrpId", -1, GrpId);
            proc.AddVarcharPara("@BranchHierchy", -1, BranchHierchy);
            proc.AddVarcharPara("@ClientId", -1, ClientId);
            ds = proc.GetDataSet();
            return ds;
        }


        public DataSet MonthlyPerformanceReportComm(string companyid, string segment, string fromdate, string todate, string clients,
          string MasterSegment, string Seriesid, string Expiry, string GrpType, string GrpId,
            string BranchHierchy, string FinYear, string chkoptmtm)
        {
            BusinessLogicLayer.DBEngine obj=new BusinessLogicLayer.DBEngine();
           
            //ProcedureExecute proc = new ProcedureExecute("MonthlyPerformanceReportComm");
          //  string con = obj.ReturnConnectionstring();

          
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("MonthlyPerformanceReportComm");

            proc.AddVarcharPara("@companyid", 10,companyid);
            proc.AddVarcharPara("@segment", 10, segment);
            proc.AddVarcharPara("@fromdate", 30, fromdate);
            proc.AddVarcharPara("@todate", 30, todate);
            proc.AddVarcharPara("@clients", -1, clients);
            proc.AddVarcharPara("@MasterSegment", 10, MasterSegment);
            proc.AddVarcharPara("@Seriesid", 20, Seriesid);
            proc.AddVarcharPara("@Expiry", 50, Expiry);
            proc.AddVarcharPara("@GrpType", 40, GrpType);
            proc.AddVarcharPara("@GrpId", -1, GrpId);
            proc.AddVarcharPara("@BranchHierchy", -1, BranchHierchy);
            proc.AddVarcharPara("@FinYear", 15, FinYear);
            proc.AddVarcharPara("@chkoptmtm", 10, chkoptmtm);

            ds = proc.GetDataSet();
            return ds;



            //using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            //{

            //    SqlCommand cmd = new SqlCommand();
            //    cmd.Connection = con;
            //    cmd.CommandText = "[MonthlyPerformanceReportComm]";
            //    String range = "R" + '~' + Convert.ToDateTime(fromdate).ToString("yyyy-MM-dd") + '~' + Convert.ToDateTime(todate).ToString("yyyy-MM-dd");
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.AddWithValue("@companyid", companyid.ToString());
            //    cmd.Parameters.AddWithValue("@segment", Convert.ToInt32(segment.ToString()));

            //    cmd.Parameters.AddWithValue("@fromdate", fromdate);
            //    cmd.Parameters.AddWithValue("@todate", todate);

            //    cmd.Parameters.AddWithValue("@clients", clients);

            //    cmd.Parameters.AddWithValue("@MasterSegment", MasterSegment.ToString());

            //    cmd.Parameters.AddWithValue("@Seriesid", Seriesid.ToString().Trim());


            //    cmd.Parameters.AddWithValue("@Expiry", Expiry);

            //    cmd.Parameters.AddWithValue("@GrpType", GrpType);
            //    cmd.Parameters.AddWithValue("@GrpId", GrpId);


            //    cmd.Parameters.AddWithValue("@BranchHierchy", BranchHierchy.ToString());
            //    cmd.Parameters.AddWithValue("@FinYear", FinYear.ToString().Trim());

            //    cmd.Parameters.AddWithValue("@chkoptmtm", chkoptmtm);

            //    SqlDataAdapter da = new SqlDataAdapter();
            //    da.SelectCommand = cmd;
            //    cmd.CommandTimeout = 0;
            //    ds.Reset();
            //    da.Fill(ds);
            //    da.Dispose();
            //    return ds;
           // }
        }

        public DataSet Sp_OtherChargesStatement(string FROMDATE, string TODATE, string Segment,
            string Companyid, string MasterSegment, string ClientsID,
         string ChargeCode, string Chargename)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_OtherChargesStatement");

            proc.AddVarcharPara("@FROMDATE", 50, FROMDATE);
            proc.AddVarcharPara("@TODATE", 50, TODATE);
            proc.AddVarcharPara("@Segment", 50, Segment);
            proc.AddVarcharPara("@Companyid", 30, Companyid);
            proc.AddVarcharPara("@MasterSegment", 20, MasterSegment);
            proc.AddVarcharPara("@ClientsID", -1, ClientsID);
            proc.AddVarcharPara("@ChargeCode", 50, ChargeCode);
            proc.AddVarcharPara("@Chargename", 50, Chargename);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet finalsettlementreport(string companyid, string segment, string fromdate, string todate,
             string clients, string MasterSegment,
         string Seriesid, string Expiry, string grptype, string chktype, string PRINTCHK)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("finalsettlementreport");

            proc.AddVarcharPara("@Companyid", 10, companyid);
            proc.AddVarcharPara("@segment", 50, segment);
            proc.AddVarcharPara("@fromdate", 50, fromdate);
            proc.AddVarcharPara("@todate", 50, todate);
            proc.AddVarcharPara("@clients", -1, clients);
            proc.AddVarcharPara("@MasterSegment", 50, MasterSegment);
            proc.AddVarcharPara("@Seriesid", -1, Seriesid);
            proc.AddVarcharPara("@Expiry", -1, Expiry);
            proc.AddVarcharPara("@grptype", 20, grptype);
            proc.AddVarcharPara("@chktype", -1, chktype);
            proc.AddVarcharPara("@PRINTCHK", 200, PRINTCHK);

            ds = proc.GetDataSet();
            return ds;
        }


        public DataSet Report_TuroverTotAcrossSegment(string FromDate, string ToDate, string CompanyId, string ChkConsolidate,
                      string Segmentid, string Broker, string ClientId, string GrpType, string GrpId, string BranchHierchy, string ReportView)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Report_TuroverTotAcrossSegment");

            proc.AddVarcharPara("@FromDate", 35, FromDate);
            proc.AddVarcharPara("@ToDate", 35, ToDate);
            proc.AddVarcharPara("@CompanyId", 100, CompanyId);
            proc.AddVarcharPara("@ChkConsolidate", 2, ChkConsolidate);
            proc.AddVarcharPara("@Segmentid", 100, Segmentid);
            proc.AddVarcharPara("@Broker", 10, Broker);
            proc.AddVarcharPara("@ClientId", -1, ClientId);
            proc.AddVarcharPara("@GrpType", 40, GrpType);
            proc.AddVarcharPara("@GrpId", -1, GrpId);
            proc.AddVarcharPara("@BranchHierchy", -1, BranchHierchy);
            proc.AddVarcharPara("@ReportView", 3, ReportView);



    
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet Report_TuroverTotCm(string FromDate, string ToDate, string CompanyId, string Segmentid, string Broker,
                       string ClientId, string GrpType, string GrpId, string BranchHierchy, string ChkConsolidatedGrpBranch,
                       string ReportView, string ReportFor, string ActivePassive, string ProClientSplit, string instrument)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Report_TuroverTotCm");

            proc.AddVarcharPara("@FromDate", 35, FromDate);
            proc.AddVarcharPara("@ToDate", 35, ToDate);
            proc.AddVarcharPara("@CompanyId", 100, CompanyId);
            proc.AddVarcharPara("@Segmentid", 100, Segmentid);
            proc.AddVarcharPara("@Broker", 10, Broker);
            proc.AddVarcharPara("@ClientId", -1, ClientId);
            proc.AddVarcharPara("@GrpType", 40, GrpType);
            proc.AddVarcharPara("@GrpId", -1, GrpId);
            proc.AddVarcharPara("@BranchHierchy", -1, BranchHierchy);
            proc.AddVarcharPara("@ChkConsolidatedGrpBranch", 5, ChkConsolidatedGrpBranch);
            proc.AddVarcharPara("@ReportView", 5, ReportView);
            proc.AddVarcharPara("@ReportFor", 10, ReportFor);
            proc.AddVarcharPara("@ActivePassive", 8, ActivePassive);
            proc.AddVarcharPara("@ProClientSplit", 8, ProClientSplit);
            proc.AddVarcharPara("@instrument", -1, instrument);
            ds = proc.GetDataSet();
            return ds;
        }


        public DataSet Report_StampDuty(string FROMDATE, string TODATE, string ClientsID, string REPORTTYPE,
          string GRPTYPE, string Groupby, string MASTERSEGMENT, string userbranchHierarchy, string RPTVIEW, 
            string Segment, string Companyid,string ChkConsolidateAcrossSegment, string ConsiderDate, string Param)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Report_StampDuty");

            proc.AddVarcharPara("@FROMDATE", 50, FROMDATE);
            proc.AddVarcharPara("@TODATE", 50, TODATE);
            proc.AddVarcharPara("@ClientsID", -1, ClientsID);
            proc.AddVarcharPara("@REPORTTYPE", 20, REPORTTYPE);
            proc.AddVarcharPara("@GRPTYPE",30, GRPTYPE);
            proc.AddVarcharPara("@Groupby", -1, Groupby);
            proc.AddVarcharPara("@MASTERSEGMENT", 10, MASTERSEGMENT);
            proc.AddVarcharPara("@userbranchHierarchy", -1, userbranchHierarchy);
            proc.AddVarcharPara("@RPTVIEW", 5, RPTVIEW);
            proc.AddVarcharPara("@Segment", 100, Segment);
            proc.AddVarcharPara("@Companyid", 100, Companyid);
            proc.AddVarcharPara("@ChkConsolidateAcrossSegment", 2, ChkConsolidateAcrossSegment);
            proc.AddVarcharPara("@ConsiderDate", 20, ConsiderDate);
            proc.AddVarcharPara("@Param", 5, Param);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet ExcessMarginRefundStatement(string fordate, string ledgerbalndate, string cashdeptdate, string clients,
         string segment, string CompanyID, string Finyear, string WrkingDays, string markupappmrgn,
           string maintaincashcomp, string checkpartialrelease, string grptype, string branch, string grpval,
            string stockreleaseorder,string PRINTCHK)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("ExcessMarginRefundStatement");

            proc.AddVarcharPara("@fordate", 35, fordate);
            proc.AddVarcharPara("@ledgerbalndate", 35, ledgerbalndate);
            proc.AddVarcharPara("@cashdeptdate", 35, cashdeptdate);
            proc.AddVarcharPara("@clients", -1, clients);
            proc.AddVarcharPara("@segment", 50, segment);
            proc.AddVarcharPara("@CompanyID", 15, CompanyID);
            proc.AddVarcharPara("@Finyear", 15, Finyear);
            proc.AddIntegerPara("@WrkingDays",Convert.ToInt32(WrkingDays));
            proc.AddDecimalPara("@markupappmrgn", 2,28,Convert.ToDecimal(markupappmrgn));
            proc.AddDecimalPara("@maintaincashcomp" ,2,28,Convert.ToDecimal( maintaincashcomp));
            proc.AddVarcharPara("@checkpartialrelease", 10, checkpartialrelease);
            proc.AddVarcharPara("@grptype", 30, grptype);
            proc.AddVarcharPara("@branch", -1, branch);
            proc.AddVarcharPara("@grpval", -1, grpval);
            proc.AddVarcharPara("@stockreleaseorder", 10, stockreleaseorder);
            proc.AddVarcharPara("@PRINTCHK", 20, PRINTCHK);
            ds = proc.GetDataSet();
            return ds;
        }

        #region Reports_frmReport_cdslBill

        public DataSet Bill_Report(string qstr, string Billnumber, string AccountNumber, string SegmentId,
           string CompanyId, string billamt)
        {
            DataSet ds = new DataSet();
            string cmdClients;

            if (qstr == "CDSL")
            {
                cmdClients = "cdslBill_Report";
                ProcedureExecute proc = new ProcedureExecute(cmdClients);
                proc.AddVarcharPara("@billNumber", 50, Billnumber);
                proc.AddVarcharPara("@BenAccount", 8000, AccountNumber);
                proc.AddVarcharPara("@group", 8000, "NA");
                proc.AddVarcharPara("@DPChargeMembers_SegmentID", 20, SegmentId);
                proc.AddVarcharPara("@DPChargeMembers_CompanyID", 20, CompanyId);
                proc.AddVarcharPara("@billamt", 50, billamt);

                ds = proc.GetDataSet();
            }
            else if (qstr == "NSDL")
            {
                cmdClients = "sp_NsdlBill_Report";

                ProcedureExecute proc = new ProcedureExecute(cmdClients);
                proc.AddVarcharPara("@billNumber", 50, Billnumber);
                proc.AddVarcharPara("@BenAccount", 8000, AccountNumber);
                proc.AddVarcharPara("@group", 8000, "NA");
                proc.AddVarcharPara("@DPChargeMembers_SegmentID", 20, SegmentId);
                proc.AddVarcharPara("@DPChargeMembers_CompanyID", 20, CompanyId);
                proc.AddVarcharPara("@billamt", 50, billamt);

                ds = proc.GetDataSet();
            }
            return ds;
        }

        public DataSet Bill_ReportReportHolding( string Billnumber, string AccountNumber, string SegmentId,
         string CompanyId, string dp)
        {
            DataSet ds = new DataSet();
          


                ProcedureExecute proc = new ProcedureExecute("cdslBill_ReportHolding");
                proc.AddVarcharPara("@billNumber", 50, Billnumber);
                proc.AddVarcharPara("@BenAccount", 8000, AccountNumber);
                proc.AddVarcharPara("@group", 8000, "NA");
                proc.AddVarcharPara("@DPChargeMembers_SegmentID", 20, SegmentId);
                proc.AddVarcharPara("@DPChargeMembers_CompanyID", 20, CompanyId);
               

                proc.AddVarcharPara("@dp",10, dp);

                ds = proc.GetDataSet();
           
            return ds;
        }


        public DataSet cdslBill_ReportSummary(string Billnumber, string AccountNumber, string SegmentId,
       string CompanyId)
        {
            DataSet ds = new DataSet();



            ProcedureExecute proc = new ProcedureExecute("cdslBill_ReportSummary");
            proc.AddVarcharPara("@billNumber", 50, Billnumber);
            proc.AddVarcharPara("@BenAccount", 8000, AccountNumber);
            proc.AddVarcharPara("@group", 8000, "NA");
            proc.AddVarcharPara("@DPChargeMembers_SegmentID", 20, SegmentId);
            proc.AddVarcharPara("@DPChargeMembers_CompanyID", 20, CompanyId);


            ds = proc.GetDataSet();

            return ds;
        }


        public DataSet cdslBill_ReportAccountsLedger(string StartDate, string EndDate, string dpId,
     string CompanyId, string SegmentId, string qstr, string AccountNumber, string LastFinYear)
        {
            DataSet ds = new DataSet();

            ProcedureExecute proc = new ProcedureExecute("cdslBill_ReportAccountsLedger");

            proc.AddVarcharPara("@startDate",50, StartDate);
            proc.AddVarcharPara("@endDate",50, EndDate);
            proc.AddVarcharPara("@dpId",30, dpId);
            proc.AddVarcharPara("@companyID",30, CompanyId);
            proc.AddVarcharPara("@SegmentId",50, SegmentId);

            if (qstr == "CDSL")
                proc.AddVarcharPara("@MainAcID",50, "SYSTM00042");
            else if (qstr == "NSDL")
                proc.AddVarcharPara("@MainAcID",50, "SYSTM00043");


            proc.AddVarcharPara("@SubAccountID",-1, "'" + AccountNumber + "'");
            proc.AddVarcharPara("@financialYear",30,LastFinYear);
            proc.AddVarcharPara("@groupCode",-1, "NA");


            ds = proc.GetDataSet();

            return ds;
        }

        public DataTable cdslBill_cNsdlBill_ClientScreen(string qstr, string Billnumber, string AccountNumber,
            string CompanyId, string SegmentId, string billamt)
        {
            DataSet ds = new DataSet();

            if (qstr == "CDSL")
            {
                ProcedureExecute proc = new ProcedureExecute("cdslBill_DisplayScreen");

                proc.AddVarcharPara("@billNumber",30, Billnumber);
                proc.AddVarcharPara("@BenAccount",-1, AccountNumber);
                proc.AddVarcharPara("@group",-1, "NA");
                proc.AddVarcharPara("@DPChargeMembers_SegmentID",10, SegmentId);
                proc.AddVarcharPara("@DPChargeMembers_CompanyID",10, CompanyId);
                proc.AddVarcharPara("@billamt",50, billamt);
                proc.AddVarcharPara("@generationOrder", 50, "PinCode");
                ds = proc.GetDataSet();

            }
            else if (qstr == "NSDL")
            {
                ProcedureExecute proc = new ProcedureExecute("sp_NsdlBill_ClientScreen");

                proc.AddVarcharPara("@billNumber", 30, Billnumber);
                proc.AddVarcharPara("@BenAccount", -1, AccountNumber);
                proc.AddVarcharPara("@group", -1, "NA");
                proc.AddVarcharPara("@DPChargeMembers_SegmentID", 10, SegmentId);
                proc.AddVarcharPara("@DPChargeMembers_CompanyID", 10, CompanyId);
                proc.AddVarcharPara("@billamt", 50, billamt);
                proc.AddVarcharPara("@generationOrder", 50, "PinCode");

                ds = proc.GetDataSet();
            }
            return ds.Tables[0];
        }

        public DataTable cdslBill_NsdlBill_BillScreen(string qstr, string DPBillSummary_BillNumber)
        {
            DataSet ds = new DataSet();

            if (qstr == "CDSL")
            {
                ProcedureExecute proc = new ProcedureExecute("cdslBillDisplayonScreen");

                proc.AddVarcharPara("@DPBillSummary_BillNumber", 30, DPBillSummary_BillNumber);
                ds = proc.GetDataSet();

            }
            else if (qstr == "NSDL")
            {
                ProcedureExecute proc = new ProcedureExecute("sp_NsdlBill_BillScreen");

                proc.AddVarcharPara("@DPBillSummary_BillNumber", 30, DPBillSummary_BillNumber);

                ds = proc.GetDataSet();
            }
            return ds.Tables[0];
        }

        public DataTable cdslBill_NsdlBill_FetchTransaction(string qstr, string userid, int startIndex, int endIndex)
        {
            DataSet ds = new DataSet();

            if (qstr == "CDSL")
            {
                ProcedureExecute proc = new ProcedureExecute("cdslFeatchTransctionAfterBilling");            

                proc.AddVarcharPara("@userid", 10, userid);

                proc.AddIntegerPara("@startRowIndex", startIndex);

                proc.AddIntegerPara("@endIndex", endIndex);
                ds = proc.GetDataSet();

            }
            else if (qstr == "NSDL")
            {
                ProcedureExecute proc = new ProcedureExecute("sp_NsdlBill_FetchTransaction");

                proc.AddVarcharPara("@userid", 10, userid);

                proc.AddIntegerPara("@startRowIndex", startIndex);

                proc.AddIntegerPara("@endIndex", endIndex);

                ds = proc.GetDataSet();
            }
            return ds.Tables[0];
        }

        public DataTable cdslcdslBill_SummryDisplayScreen(string billNumber)
        {
            DataSet ds = new DataSet();


            ProcedureExecute proc = new ProcedureExecute("cdslBill_SummryDisplayScreen");

                proc.AddVarcharPara("@billNumber", 30, billNumber);
                ds = proc.GetDataSet();

           
            return ds.Tables[0];
        }

        public DataTable cdslcdslBillAccountsLedger(string StartDate, string EndDate, string dpId, string CompanyId, string SegmentId,
            string qstr, string SubAccountID, string LastFinYear)
        {
            DataSet ds = new DataSet();


            ProcedureExecute proc = new ProcedureExecute("cdslBillAccountsLedger");

                //proc.AddVarcharPara("@userid", 10, userid);

                //proc.AddIntegerPara("@startRowIndex", startIndex);

                //proc.AddIntegerPara("@endIndex", endIndex);

                proc.AddVarcharPara("@startDate",50, StartDate);
                proc.AddVarcharPara("@endDate",50, EndDate);
                proc.AddVarcharPara("@dpId",30, dpId);
                proc.AddVarcharPara("@companyID",30, CompanyId);
                proc.AddVarcharPara("@SegmentId",50, SegmentId);

                if (qstr == "CDSL")
                    proc.AddVarcharPara("@MainAcID",50, "SYSTM00042");
                else if (qstr == "NSDL")
                    proc.AddVarcharPara("@MainAcID",50, "SYSTM00043");

                  proc.AddVarcharPara("@SubAccountID",-1, SubAccountID);
                  proc.AddVarcharPara("@financialYear",30,LastFinYear);



                ds = proc.GetDataSet();
           
            return ds.Tables[0];
        }

        #endregion



        public DataSet Report_TuroverTotComm(string FromDate, string ToDate, string CompanyId, string Segmentid, string Broker,
                     string Clientid, string GrpType, string GrpId, string BranchHierchy, string ChkConsolidatedGrpBranch, string ReportView, 
            string ReportFor,string  instrument)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Report_TuroverTotComm");

            proc.AddVarcharPara("@FromDate", 35, FromDate);
            proc.AddVarcharPara("@ToDate", 35, ToDate);
            proc.AddVarcharPara("@CompanyId", 100, CompanyId);
            proc.AddVarcharPara("@Segmentid", 100, Segmentid);
            proc.AddVarcharPara("@Broker", 10, Broker);
            proc.AddVarcharPara("@Clientid", -1, Clientid);
            proc.AddVarcharPara("@GrpType", 40, GrpType);
            proc.AddVarcharPara("@GrpId", -1, GrpId);
            proc.AddVarcharPara("@BranchHierchy", -1, BranchHierchy);
            proc.AddVarcharPara("@ChkConsolidatedGrpBranch", 5, ChkConsolidatedGrpBranch);
            proc.AddVarcharPara("@ReportView", 5, ReportView);
            proc.AddVarcharPara("@ReportFor", 5, ReportFor);
            proc.AddVarcharPara("@instrument", 5, instrument);
            ds = proc.GetDataSet();
            return ds;
        }

    }
}
