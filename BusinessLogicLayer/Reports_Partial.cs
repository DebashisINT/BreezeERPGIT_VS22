using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;
using System.Data;
namespace BusinessLogicLayer
{
  public  partial class Reports
    {
      public DataTable cdslHoldingReport1(string startTime, string endTime, string isin, string settlementId, string boid)
      {
          DataTable dt = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("cdslHoldingReport1");

          proc.AddVarcharPara("@startTime", 30, startTime);
          proc.AddVarcharPara("@endTime", 50, endTime);
          proc.AddVarcharPara("@isin", 50, isin);
          proc.AddVarcharPara("@settlementId", 50, settlementId);
          proc.AddVarcharPara("@boid", 50, boid);
       
          dt = proc.GetTable();
          return dt;
      }
      public DataTable cdslTransctionShowList(string stdate, string eddate, string companyId, string BoID, string isin,
         string SettlementID, string boStatus, string userid, string branchid)
      {
          DataTable dt = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("cdslTransctionShowList");

          proc.AddVarcharPara("@stdate", 30, stdate);
          proc.AddVarcharPara("@eddate", 50, eddate);
          proc.AddVarcharPara("@companyId", 50, companyId);
          proc.AddVarcharPara("@BoID", -1, BoID);
          proc.AddVarcharPara("@isin", 50, isin);
          proc.AddVarcharPara("@SettlementID", 30, SettlementID);
          proc.AddVarcharPara("@boStatus", 50, boStatus);
          proc.AddVarcharPara("@userid", 50, userid);
          proc.AddVarcharPara("@branchid", -1, branchid);        
          dt = proc.GetTable();
          return dt;
      }

      public DataTable cdslTransctionDisplay1(string stdate, string eddate, string boid, string isin, string SettlementID, string userbranchHierarchy)
      {
          DataTable dt = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("cdslTransctionDisplay1");

          proc.AddVarcharPara("@stdate", 30, stdate);
          proc.AddVarcharPara("@eddate", 50, eddate);
          proc.AddVarcharPara("@boid", 50, boid);
          proc.AddVarcharPara("@isin", 50, isin);
          proc.AddVarcharPara("@SettlementID", 50, SettlementID);
          proc.AddVarcharPara("@userbranchHierarchy",-1, userbranchHierarchy);
       
          dt = proc.GetTable();
          return dt;
      }
      public DataTable cdslFeatchTransction(string userid, int startRowIndex, int endIndex)
      {
          DataTable dt = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("cdslFeatchTransction");

          proc.AddVarcharPara("@userid", 30, userid);
          proc.AddIntegerPara("@startRowIndex",startRowIndex);
          proc.AddIntegerPara("@endIndex", endIndex);
        
          dt = proc.GetTable();
          return dt;
      }

      public DataSet cdslTransctionShowwithDematandPledge(string stdate, string eddate, string compID, string dp, string BoID,
          string isin, string SettlementID, string boStatus, string branchid)
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("cdslTransctionShowwithDematandPledge");

          proc.AddVarcharPara("@stdate", 50, stdate);
          proc.AddVarcharPara("@eddate", 50, eddate);
          proc.AddVarcharPara("@compID", 50, compID);
          proc.AddVarcharPara("@dp", 50, dp);
          proc.AddVarcharPara("@BoID",-1, BoID);
          proc.AddVarcharPara("@isin", 50, isin);
          proc.AddVarcharPara("@SettlementID", 50, SettlementID);
          proc.AddVarcharPara("@boStatus", 50, boStatus);
          proc.AddVarcharPara("@branchid",-1, branchid);
          ds = proc.GetDataSet();
          return ds;
      }


      public DataSet cdslBill_ReportHolding_transaction(string billNumber, string BenAccount, string group, string DPChargeMembers_SegmentID, string DPChargeMembers_CompanyID,
     string dp, string billamt, string generationOrder, string dpId)
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("cdslBill_ReportHolding_transaction");

          proc.AddVarcharPara("@billNumber", 50, billNumber);
          proc.AddVarcharPara("@BenAccount",-1, BenAccount);
          proc.AddVarcharPara("@group", -1, group);
          proc.AddVarcharPara("@DPChargeMembers_SegmentID", 50, DPChargeMembers_SegmentID);
          proc.AddVarcharPara("@DPChargeMembers_CompanyID", 50, DPChargeMembers_CompanyID);
          proc.AddVarcharPara("@dp", 50, dp);
          proc.AddVarcharPara("@billamt", 50, billamt);
          proc.AddVarcharPara("@generationOrder", 50, generationOrder);
          proc.AddVarcharPara("@dpId", 50, dpId);
          ds = proc.GetDataSet();
          return ds;
      }

      public void Insert_NomineeRegister(string RegistrationNo, string RegistrationDate, string BenID, string Name, string Address,
         string Country, string State, string City, string PinCode, string IsMinor,
         string GuadianAddress, string GuardianCountry, string GuardianState, string GuardianCity, string GuardianPinCode,
          string DOBMinor, string ResidencePhone, string Mobile, string Email, string Remarks,
           string NominationFlag, string DpId, string Segment, string User, string EntryType
       )
      {

          ProcedureExecute proc = new ProcedureExecute("Insert_NomineeRegister");
          proc.AddNTextPara("@RegistrationNo", RegistrationNo);
          proc.AddNVarcharPara("@RegistrationDate", 50, RegistrationDate);
          proc.AddNVarcharPara("@BenID", 50, BenID);
          proc.AddNVarcharPara("@Name", 150, Name);
          proc.AddNVarcharPara("@Address", 200, Address);
          proc.AddIntegerPara("@Country", Convert.ToInt32(Country), QueryParameterDirection.Input);
          proc.AddIntegerPara("@State", Convert.ToInt32(State), QueryParameterDirection.Input);
          proc.AddIntegerPara("@City", Convert.ToInt32(City), QueryParameterDirection.Input);
          proc.AddNVarcharPara("@PinCode", 50, PinCode);
          proc.AddNVarcharPara("@IsMinor", 10, IsMinor);
          proc.AddNVarcharPara("@GuadianAddress", 200, GuadianAddress);
          proc.AddIntegerPara("@GuardianCountry", Convert.ToInt32(GuardianCountry), QueryParameterDirection.Input);
          proc.AddIntegerPara("@GuardianState", Convert.ToInt32(GuardianState), QueryParameterDirection.Input);
          proc.AddIntegerPara("@GuardianCity", Convert.ToInt32(GuardianCity), QueryParameterDirection.Input);
          proc.AddNVarcharPara("@GuardianPinCode", 20, GuardianPinCode);
          proc.AddNVarcharPara("@DOBMinor", 100, DOBMinor);
          proc.AddNVarcharPara("@ResidencePhone", 20, ResidencePhone);
          proc.AddNVarcharPara("@Mobile", 20, Mobile);
          proc.AddNVarcharPara("@Email", 20, Email);
          proc.AddNVarcharPara("@Remarks", 150, Remarks);
          proc.AddNVarcharPara("@NominationFlag", 10, NominationFlag);
          proc.AddNVarcharPara("@DpId", 20, DpId);
          proc.AddNVarcharPara("@Segment", 10, Segment);
          proc.AddNVarcharPara("@User", 10, User);
          proc.AddNVarcharPara("@EntryType", 20, EntryType);
         proc.RunActionQuery();
         
      }

      public void Update_NomineeRegister(string RegistrationNo, string RegistrationDate, string BenID, string Name, string Address,
       string Country, string State, string City, string PinCode, string IsMinor,
       string GuadianAddress, string GuardianCountry, string GuardianState, string GuardianCity, string GuardianPinCode,
        string DOBMinor, string ResidencePhone, string Mobile, string Email, string Remarks,
         string NominationFlag, string DpId, string Segment, string User, string EntryType, string NomineeId 
     )
      {

          ProcedureExecute proc = new ProcedureExecute("Insert_NomineeRegister");
          proc.AddNTextPara("@RegistrationNo", RegistrationNo);
          proc.AddNVarcharPara("@RegistrationDate", 50, RegistrationDate);
          proc.AddNVarcharPara("@BenID", 50, BenID);
          proc.AddNVarcharPara("@Name", 150, Name);
          proc.AddNVarcharPara("@Address", 200, Address);
          proc.AddIntegerPara("@Country", Convert.ToInt32(Country), QueryParameterDirection.Input);
          proc.AddIntegerPara("@State", Convert.ToInt32(State), QueryParameterDirection.Input);
          proc.AddIntegerPara("@City", Convert.ToInt32(City), QueryParameterDirection.Input);
          proc.AddNVarcharPara("@PinCode", 50, PinCode);
          proc.AddNVarcharPara("@IsMinor", 10, IsMinor);
          proc.AddNVarcharPara("@GuadianAddress", 200, GuadianAddress);
          proc.AddIntegerPara("@GuardianCountry", Convert.ToInt32(GuardianCountry), QueryParameterDirection.Input);
          proc.AddIntegerPara("@GuardianState", Convert.ToInt32(GuardianState), QueryParameterDirection.Input);
          proc.AddIntegerPara("@GuardianCity", Convert.ToInt32(GuardianCity), QueryParameterDirection.Input);
          proc.AddNVarcharPara("@GuardianPinCode", 20, GuardianPinCode);
          proc.AddNVarcharPara("@DOBMinor", 100, DOBMinor);
          proc.AddNVarcharPara("@ResidencePhone", 20, ResidencePhone);
          proc.AddNVarcharPara("@Mobile", 20, Mobile);
          proc.AddNVarcharPara("@Email", 20, Email);
          proc.AddNVarcharPara("@Remarks", 150, Remarks);
          proc.AddNVarcharPara("@NominationFlag", 10, NominationFlag);
          proc.AddNVarcharPara("@DpId", 20, DpId);
          proc.AddNVarcharPara("@Segment", 10, Segment);
          proc.AddNVarcharPara("@User", 10, User);
          proc.AddNVarcharPara("@EntryType", 20, EntryType);
          proc.AddIntegerPara("@NomineeId", Convert.ToInt32(NomineeId), QueryParameterDirection.Input);
          proc.RunActionQuery();

      }



      public DataSet Fetch_CDSLClientMaster(string SelectedClient, string DateRangeSelection, string AccountStatus, string FromDate, string ToDate,
   string AccType, string AccSubType, string BenCategory, string BenOcp,
           string ShowNHolder, string ShowNPOA, string ShowAcMinor, string ShowAcNNom, string ShowAcMNom, string ShowGroup, string GroupType
          )
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("Fetch_CDSLClientMaster");

          proc.AddVarcharPara("@SelectedClient", -1, SelectedClient);
          proc.AddVarcharPara("@DateRangeSelection", 10, DateRangeSelection);
          proc.AddVarcharPara("@AccountStatus", 10, AccountStatus);
          proc.AddVarcharPara("@FromDate", 100, FromDate);
          proc.AddVarcharPara("@ToDate", 100, ToDate);
          proc.AddVarcharPara("@AccType", -1, AccType);
          proc.AddVarcharPara("@AccSubType", -1, AccSubType);
          proc.AddVarcharPara("@BenCategory", 10, BenCategory);
          proc.AddVarcharPara("@BenOcp", 10, BenOcp);
          proc.AddVarcharPara("@ShowNHolder", 10, ShowNHolder);
          proc.AddVarcharPara("@ShowNPOA", 10, ShowNPOA);
          proc.AddVarcharPara("@ShowAcMinor", 10, ShowAcMinor);
          proc.AddVarcharPara("@ShowAcNNom", 10, ShowAcNNom);
          proc.AddVarcharPara("@ShowAcMNom", 10, ShowAcMNom);
          proc.AddVarcharPara("@ShowGroup", 10, ShowGroup);
          proc.AddVarcharPara("@GroupType", 100, GroupType);
          ds = proc.GetDataSet();
          return ds;
      }


      public DataSet Fetch_slipsinwardregister_Data_CDSL(string CompID, string CdslClients_BOID, string exchangesegmentid, string CdslClients_BranchID
     )
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("Fetch_slipsinwardregister_Data_CDSL");

          proc.AddVarcharPara("@CompID", 30, CompID);
          proc.AddVarcharPara("@CdslClients_BOID", 30, CdslClients_BOID);
          proc.AddVarcharPara("@exchangesegmentid", 30, exchangesegmentid);
          proc.AddVarcharPara("@CdslClients_BranchID", -1, CdslClients_BranchID);
       
          ds = proc.GetDataSet();
          return ds;
      }


      public DataSet Sp_Fetch_slipsinwardregister_Data(string BenAccountID, string exchangesegmentid, string branchid
     )
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("Sp_Fetch_slipsinwardregister_Data");

          proc.AddIntegerPara("@BenAccountID",Convert.ToInt32(BenAccountID));
          proc.AddVarcharPara("@exchangesegmentid", 50, exchangesegmentid);
          proc.AddVarcharPara("@branchid", -1, branchid);
        
          ds = proc.GetDataSet();
          return ds;
      }


      public DataSet Sp_Fetch_SlipsQBR_Data(string sliptype, string slipno, string segment, string Dpid
   )
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("Sp_Fetch_SlipsQBR_Data");

          proc.AddIntegerPara("@sliptype", Convert.ToInt32(sliptype));
          proc.AddVarcharPara("@slipno", 50, slipno);
          proc.AddIntegerPara("@segment", Convert.ToInt32(segment));
          proc.AddVarcharPara("@Dpid", 20, Dpid);
          ds = proc.GetDataSet();
          return ds;
      }


      public DataSet Report_SubBrokerageCalculationReport(string Companyid, string Segment, string FinYear, string Month,
          string FromDate, string ToDate, string CommissionFor, string Commission,
          string ReportView, string GrpType, string GrpId, string BranchHierchy, string ClientId
)
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("Report_SubBrokerageCalculationReport");

          proc.AddVarcharPara("@Companyid", 20,Companyid);
          proc.AddVarcharPara("@Segment", 20, Segment);
          proc.AddVarcharPara("@FinYear", 20,FinYear);
          proc.AddVarcharPara("@Month", 20, Month);
          proc.AddVarcharPara("@FromDate",50,FromDate);
          proc.AddVarcharPara("@ToDate", 50, ToDate);
          proc.AddVarcharPara("@CommissionFor", 50,CommissionFor);
          proc.AddVarcharPara("@Commission", -1, Commission);
          proc.AddVarcharPara("@ReportView", 20, ReportView);
          proc.AddVarcharPara("@GrpType", 50,GrpType);
          proc.AddVarcharPara("@GrpId", -1, GrpId);
          proc.AddVarcharPara("@BranchHierchy", -1, BranchHierchy);
          proc.AddVarcharPara("@ClientId", -1,ClientId);
        
          ds = proc.GetDataSet();
          return ds;
      }


      public DataSet Report_STTReports(string CompanyId, string fromdate, string todate, string clients,
         string Segment, string Asset, string GrpType, string GrpId,
         string BranchHierchy, string FinYear, string CalType, string RptView, string ChkConsolidate, string ChkConsolidateSegmentScrip, string ChkShowALL
)
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("Report_STTReports");

          proc.AddVarcharPara("@CompanyId",-1, CompanyId);
          proc.AddVarcharPara("@fromdate", 50, fromdate);
          proc.AddVarcharPara("@todate", 50, todate);
          proc.AddVarcharPara("@clients", -1, clients);
          proc.AddVarcharPara("@Segment", 50, Segment);
          proc.AddVarcharPara("@Asset", 50, Asset);
          proc.AddVarcharPara("@GrpType", 50, GrpType);
          proc.AddVarcharPara("@GrpId", -1, GrpId);
          proc.AddVarcharPara("@BranchHierchy", -1, BranchHierchy);
          proc.AddVarcharPara("@FinYear", 50, FinYear);
          proc.AddVarcharPara("@CalType", 20, CalType);
          proc.AddVarcharPara("@RptView", 10, RptView);
          proc.AddVarcharPara("@ChkConsolidate", 10, ChkConsolidate);
          proc.AddVarcharPara("@ChkConsolidateSegmentScrip", 10, ChkConsolidateSegmentScrip);
          proc.AddVarcharPara("@ChkShowALL", 10, ChkShowALL);        
              

          ds = proc.GetDataSet();
          return ds;
      }


      public DataSet Report_CTTReports(string CompanyId, string fromdate, string todate, string clients,
       string Segment, string Asset, string GrpType, string GrpId,
       string BranchHierchy, string FinYear, string CalType, string RptView, string ChkConsolidate, string ChkConsolidateSegmentScrip, string ChkShowALL
)
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("Report_CTTReports_Comm");

          proc.AddVarcharPara("@CompanyId", -1, CompanyId);
          proc.AddVarcharPara("@fromdate", 50, fromdate);
          proc.AddVarcharPara("@todate", 50, todate);
          proc.AddVarcharPara("@clients", -1, clients);
          proc.AddVarcharPara("@Segment", 50, Segment);
          proc.AddVarcharPara("@Asset", 50, Asset);
          proc.AddVarcharPara("@GrpType", 50, GrpType);
          proc.AddVarcharPara("@GrpId", -1, GrpId);
          proc.AddVarcharPara("@BranchHierchy", -1, BranchHierchy);
          proc.AddVarcharPara("@FinYear", 50, FinYear);
          proc.AddVarcharPara("@CalType", 20, CalType);
          proc.AddVarcharPara("@RptView", 10, RptView);
          proc.AddVarcharPara("@ChkConsolidate", 10, ChkConsolidate);
          proc.AddVarcharPara("@ChkConsolidateSegmentScrip", 10, ChkConsolidateSegmentScrip);
          proc.AddVarcharPara("@ChkShowALL", 10, ChkShowALL);


          ds = proc.GetDataSet();
          return ds;
      }

      public DataSet Report_NetPositionAnalysis(string FromDate, string ToDate, string Companyid, string Segmentid,
             string Clientid, string GrpType, string GrpId, string BranchHierchy,
             string Productid, string rptview, string ParameterFeild
    )
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("Report_NetPositionAnalysis");

          proc.AddVarcharPara("@FromDate", 50, FromDate);
          proc.AddVarcharPara("@ToDate", 50, ToDate);
          proc.AddVarcharPara("@Companyid", 50, Companyid);
          proc.AddVarcharPara("@Segmentid", 50, Segmentid);
          proc.AddVarcharPara("@Clientid",-1, Clientid);
          proc.AddVarcharPara("@GrpType", 50, GrpType);
          proc.AddVarcharPara("@GrpId", -1, GrpId);
          proc.AddVarcharPara("@BranchHierchy", -1, BranchHierchy);
          proc.AddVarcharPara("@Productid", 50, Productid);
          proc.AddVarcharPara("@rptview", 10, rptview);
          proc.AddVarcharPara("@ParameterFeild", -1, ParameterFeild);        
          ds = proc.GetDataSet();
          return ds;
      }


      public DataSet TOTReportFO(string companyid, string segment, string fromdate, string todate,
          string Broker, string clients, string MasterSegment, string grptype,
          string grp, string branch, string rpttype, string forVal, string option, string chFigures
 )
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("TOTReportFO");

          proc.AddVarcharPara("@companyid", 20, companyid);
          proc.AddVarcharPara("@segment", -1, segment);
          proc.AddVarcharPara("@fromdate", 50, fromdate);
          proc.AddVarcharPara("@todate", 50, todate);
          proc.AddVarcharPara("@Broker", 10, Broker);
          proc.AddVarcharPara("@clients", -1, clients);
          proc.AddVarcharPara("@MasterSegment", 50, MasterSegment);
          proc.AddVarcharPara("@grptype", 50, grptype);
          proc.AddVarcharPara("@grp", -1, grp);
          proc.AddVarcharPara("@branch", -1, branch);
          proc.AddVarcharPara("@rpttype", 20, rpttype);
          proc.AddVarcharPara("@for", 20, forVal);
          proc.AddVarcharPara("@option", 20, option);
          proc.AddVarcharPara("@chFigures", 20, chFigures);
          ds = proc.GetDataSet();
          return ds;
      }


      public DataSet MonthlyPerformanceReportFO(string companyid, string segment, string fromdate, string todate,
             string clients, string MasterSegment, string Seriesid, string Expiry,
             string GrpType, string GrpId, string BranchHierchy,string FinYear, string chkoptmtm
    )
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("MonthlyPerformanceReportFO");

          proc.AddVarcharPara("@companyid", 20, companyid);
          proc.AddVarcharPara("@segment", 20, segment);
          proc.AddVarcharPara("@fromdate", 50, fromdate);
          proc.AddVarcharPara("@todate", 50, todate);
          proc.AddVarcharPara("@clients", -1, clients);
          proc.AddVarcharPara("@MasterSegment", 20, MasterSegment);
          proc.AddVarcharPara("@Seriesid", 20, Seriesid);
          proc.AddVarcharPara("@Expiry", 50, Expiry);
          proc.AddVarcharPara("@GrpType", 50, GrpType);
          proc.AddVarcharPara("@GrpId", -1, GrpId);
          proc.AddVarcharPara("@BranchHierchy", -1, BranchHierchy);
          proc.AddVarcharPara("@FinYear", 20, FinYear);
          proc.AddVarcharPara("@chkoptmtm", 20, chkoptmtm);
         
          ds = proc.GetDataSet();
          return ds;
      }




      public DataSet Report_DailyMTMPremiumStatement(string usermail, string generation, string FromDate, string ToDate,
           string ClientId, string FinYear, string Asset, string Expiry,
           string Segment, string companyid, string GrpType, string GrpId, string BranchHierchy,
              string RptView, string CalType, string BfQtyFilter, string BfPriceFilter,
           string BuyQtyFilter, string BuyValueFilter, string SellQtyFilter, string SellValueFilter, string CfQtyFilter,
           string CfPriceFilter, string MTMFilter, string PremiumFilter, string AsnExcFilter,
           string FinSettFilter, string TranChargeFilter, string StampDutyFilter, string TotalServTaxFilter, string SebiFeeFilter,
          string NetObligationFilter, string OtherChargeFilter, string STTFilter
  )
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("Report_DailyMTMPremiumStatement");

          proc.AddVarcharPara("@usermail", 50, usermail);
          proc.AddVarcharPara("@generation", 20, generation);
          proc.AddVarcharPara("@FromDate", 50, FromDate);
          proc.AddVarcharPara("@ToDate", 50, ToDate);
          proc.AddVarcharPara("@ClientId", -1, ClientId);
          proc.AddVarcharPara("@FinYear", 20, FinYear);
          proc.AddVarcharPara("@Asset", -1, Asset);
          proc.AddVarcharPara("@Expiry", -1, Expiry);
          proc.AddVarcharPara("@Segment", 20, Segment);
          proc.AddVarcharPara("@companyid", 20, companyid);
          proc.AddVarcharPara("@GrpType", 50, GrpType);
          proc.AddVarcharPara("@GrpId", -1, GrpId);
          proc.AddVarcharPara("@BranchHierchy", -1, BranchHierchy);
          proc.AddVarcharPara("@RptView", 10, RptView);
          proc.AddVarcharPara("@CalType", 10, CalType);
          proc.AddVarcharPara("@BfQtyFilter", 10, BfQtyFilter);
          proc.AddVarcharPara("@BfPriceFilter", 10, BfPriceFilter);
          proc.AddVarcharPara("@BuyQtyFilter", 10, BuyQtyFilter);
          proc.AddVarcharPara("@BuyValueFilter", 10,BuyValueFilter );
          proc.AddVarcharPara("@SellQtyFilter", 10,SellQtyFilter );
          proc.AddVarcharPara("@SellValueFilter", 10,SellValueFilter );
          proc.AddVarcharPara("@CfQtyFilter", 10,CfQtyFilter);
          proc.AddVarcharPara("@CfPriceFilter",10,CfPriceFilter);
          proc.AddVarcharPara("@MTMFilter", 10,MTMFilter);
          proc.AddVarcharPara("@PremiumFilter",10,PremiumFilter);
          proc.AddVarcharPara("@AsnExcFilter",10,AsnExcFilter);
          proc.AddVarcharPara("@FinSettFilter", 10,FinSettFilter);
          proc.AddVarcharPara("@TranChargeFilter",10,TranChargeFilter);
          proc.AddVarcharPara("@StampDutyFilter", 10,StampDutyFilter);
          proc.AddVarcharPara("@TotalServTaxFilter", 10,TotalServTaxFilter);
          proc.AddVarcharPara("@SebiFeeFilter", 10,SebiFeeFilter);
          proc.AddVarcharPara("@NetObligationFilter", 10,NetObligationFilter);
          proc.AddVarcharPara("@OtherChargeFilter", 10,OtherChargeFilter);
          proc.AddVarcharPara("@STTFilter", 10, STTFilter);
          ds = proc.GetDataSet();
          return ds;
      }
      public DataSet Report_DailyMTMPremiumStatementComm(string usermail, string generation, string FromDate, string ToDate,
             string ClientId, string FinYear, string Asset, string Expiry,
             string Segment, string companyid, string GrpType, string GrpId, string BranchHierchy,
                string RptView, string CalType, string BfQtyFilter, string BfPriceFilter,
             string BuyQtyFilter, string BuyValueFilter, string SellQtyFilter, string SellValueFilter, string CfQtyFilter,
             string CfPriceFilter, string MTMFilter, string PremiumFilter, string AsnExcFilter,
             string FinSettFilter, string TranChargeFilter, string StampDutyFilter, string TotalServTaxFilter, string SebiFeeFilter,
            string NetObligationFilter, string OtherChargeFilter, string STTFilter
    )
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("Report_DailyMTMPremiumStatementComm");

          proc.AddVarcharPara("@usermail", 50, usermail);
          proc.AddVarcharPara("@generation", 20, generation);
          proc.AddVarcharPara("@FromDate", 50, FromDate);
          proc.AddVarcharPara("@ToDate", 50, ToDate);
          proc.AddVarcharPara("@ClientId", -1, ClientId);
          proc.AddVarcharPara("@FinYear", 20, FinYear);
          proc.AddVarcharPara("@Asset", -1, Asset);
          proc.AddVarcharPara("@Expiry", -1, Expiry);
          proc.AddVarcharPara("@Segment", 20, Segment);
          proc.AddVarcharPara("@companyid", 20, companyid);
          proc.AddVarcharPara("@GrpType", 50, GrpType);
          proc.AddVarcharPara("@GrpId", -1, GrpId);
          proc.AddVarcharPara("@BranchHierchy", -1, BranchHierchy);
          proc.AddVarcharPara("@RptView", 10, RptView);
          proc.AddVarcharPara("@CalType", 10, CalType);
          proc.AddVarcharPara("@BfQtyFilter", 10, BfQtyFilter);
          proc.AddVarcharPara("@BfPriceFilter", 10, BfPriceFilter);
          proc.AddVarcharPara("@BuyQtyFilter", 10, BuyQtyFilter);
          proc.AddVarcharPara("@BuyValueFilter", 10, BuyValueFilter);
          proc.AddVarcharPara("@SellQtyFilter", 10, SellQtyFilter);
          proc.AddVarcharPara("@SellValueFilter", 10, SellValueFilter);
          proc.AddVarcharPara("@CfQtyFilter", 10, CfQtyFilter);
          proc.AddVarcharPara("@CfPriceFilter", 10, CfPriceFilter);
          proc.AddVarcharPara("@MTMFilter", 10, MTMFilter);
          proc.AddVarcharPara("@PremiumFilter", 10, PremiumFilter);
          proc.AddVarcharPara("@AsnExcFilter", 10, AsnExcFilter);
          proc.AddVarcharPara("@FinSettFilter", 10, FinSettFilter);
          proc.AddVarcharPara("@TranChargeFilter", 10, TranChargeFilter);
          proc.AddVarcharPara("@StampDutyFilter", 10, StampDutyFilter);
          proc.AddVarcharPara("@TotalServTaxFilter", 10, TotalServTaxFilter);
          proc.AddVarcharPara("@SebiFeeFilter", 10, SebiFeeFilter);
          proc.AddVarcharPara("@NetObligationFilter", 10, NetObligationFilter);
          proc.AddVarcharPara("@OtherChargeFilter", 10, OtherChargeFilter);
          proc.AddVarcharPara("@STTFilter", 10, STTFilter);
          ds = proc.GetDataSet();
          return ds;
      }



      public DataSet Report_ActiveClientsNSDL(string Segment, string RecordNo, string Percentage, string FromDate, string ToDate
       )
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("Report_ActiveClientsNSDL");

          proc.AddVarcharPara("@Segment", 20, Segment);
          proc.AddIntegerPara("@RecordNo", Convert.ToInt32(RecordNo));
          proc.AddIntegerPara("@Percentage", Convert.ToInt32(Percentage));
          proc.AddVarcharPara("@FromDate", 50, FromDate);
          proc.AddVarcharPara("@ToDate", 50, ToDate);
        
          ds = proc.GetDataSet();
          return ds;
      }



      public DataSet NetPositionSPOT(string fromdate, string todate, string SettNo, string segment,
         string MasterSegment, string Companyid, string ClientsID, string instrument,
         string settype, string Branch, string GRPTYPE, string GRPID, string openposition,
           string ChkCharge, string Chksign, string rptview, decimal AmntGreaterThan, string SecurityType
)
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("NetPositionSPOT");
          proc.AddVarcharPara("@fromdate", 50, fromdate);
          proc.AddVarcharPara("@todate", 50, todate);
          proc.AddVarcharPara("@SettNo", 50, SettNo);
          proc.AddVarcharPara("@segment", 50, segment);
          proc.AddVarcharPara("@MasterSegment", 50, MasterSegment);
          proc.AddVarcharPara("@Companyid", 50, Companyid);
          proc.AddVarcharPara("@ClientsID", -1, ClientsID);
          proc.AddVarcharPara("@instrument", -1, instrument);
          proc.AddVarcharPara("@settype", -1, settype);
          proc.AddVarcharPara("@Branch", 50, Branch);
          proc.AddVarcharPara("@GRPTYPE", -1, GRPTYPE);
          proc.AddVarcharPara("@GRPID", -1, GRPID);
          proc.AddVarcharPara("@openposition", 50, openposition);
          proc.AddVarcharPara("@ChkCharge", 50, ChkCharge);
          proc.AddVarcharPara("@Chksign", 50, Chksign);
          proc.AddVarcharPara("@rptview", 50, rptview);
          proc.AddDecimalPara("@AmntGreaterThan",28,2, AmntGreaterThan);
          proc.AddVarcharPara("@SecurityType", 20, SecurityType);
         
          ds = proc.GetDataSet();
          return ds;
      }



      public DataSet Report_TransationCommCurrency(string segment, string companyid, string finyear, string Client,
         string Product, string SettNo, string TranId
)
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("Report_TransationCommCurrency");

          proc.AddVarcharPara("@segment", 10, segment);
          proc.AddVarcharPara("@companyid", 50, companyid);
          proc.AddVarcharPara("@finyear", 50, finyear);
          proc.AddVarcharPara("@Client", 50, Client);
          proc.AddVarcharPara("@Product", 50, Product);
          proc.AddVarcharPara("@SettNo", 50, SettNo);
          proc.AddVarcharPara("@TranId", 50, TranId);
        
          ds = proc.GetDataSet();
          return ds;
      }

      public DataSet Report_StocksCommCurrency(string finyear, string Product
     )
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("Report_StocksCommCurrency");

          proc.AddVarcharPara("@finyear", 50,finyear);
          proc.AddVarcharPara("@Product", 50, Product);
       
          ds = proc.GetDataSet();
          return ds;
      }
      public DataSet SettlementTrialSPOT(string date, string companyid, string segment, string MasterSegment
    )
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("SettlementTrialSPOT");

          proc.AddVarcharPara("@date", 50, date);
          proc.AddVarcharPara("@companyid", 50, companyid);
          proc.AddVarcharPara("@segment", 50, segment);
          proc.AddIntegerPara("@MasterSegment",Convert.ToInt32(MasterSegment));
          ds = proc.GetDataSet();
          return ds;
      }

      public DataSet ExportPosition(string date, string segment, string companyid, string MasterSegment, string ClientsID, string Check
      )
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("ExportPosition");

          proc.AddVarcharPara("@date", 50, date);
          proc.AddVarcharPara("@segment", 50, segment);
          proc.AddVarcharPara("@companyid", 50, companyid);
          proc.AddVarcharPara("@MasterSegment", 50,MasterSegment);
          proc.AddVarcharPara("@ClientsID", -1, ClientsID);
          proc.AddVarcharPara("@Check", 50, Check);
          ds = proc.GetDataSet();
          return ds;
      }
      public void sp_Insert_ExportFiles1(string segid, string file_type, string file_name, string userid, string batch_number, string file_path
          )
      {

          ProcedureExecute proc = new ProcedureExecute("sp_Insert_ExportFiles");

          proc.AddVarcharPara("@segid", 50, segid);
          proc.AddVarcharPara("@file_type", 50, file_type);
          proc.AddVarcharPara("@file_name", 150, file_name);
          proc.AddVarcharPara("@userid", 50, userid);
          proc.AddVarcharPara("@batch_number", 50, batch_number);
          proc.AddVarcharPara("@file_path", 200, file_path);
          proc.RunActionQuery();
         
      }


      public DataSet ExportPosition_NSECDX(string date, string segment, string companyid, string MasterSegment, string ClientsID
     )
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("ExportPosition_NSECDX");

          proc.AddVarcharPara("@date", 50, date);
          proc.AddVarcharPara("@segment", 50, segment);
          proc.AddVarcharPara("@companyid", 50, companyid);
          proc.AddVarcharPara("@MasterSegment", 50, MasterSegment);
          proc.AddVarcharPara("@ClientsID", -1, ClientsID);
         
          ds = proc.GetDataSet();
          return ds;
      }


      public DataSet ICEXContract_Reportdos(string CompanyID, string DpId, string dp, string tradedate,
         string CustomerID, string ContractNote, string AuthorizeName, string Mode,
         string SegmentExchangeID, string strFundPayoutDate, string BrkgFlag, string SettlementNumber, string SettlementType,
           string Branch, string Customer
)
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("ICEXContract_Reportdos");
          proc.AddVarcharPara("@CompanyID", 50, CompanyID);
          proc.AddVarcharPara("@DpId", 50, DpId);
          proc.AddVarcharPara("@dp", 50, dp);
          proc.AddVarcharPara("@tradedate", 50, tradedate);
          proc.AddVarcharPara("@CustomerID", -1, CustomerID);
          proc.AddVarcharPara("@ContractNote", -1, ContractNote);
          proc.AddVarcharPara("@AuthorizeName", -1, AuthorizeName);
          proc.AddVarcharPara("@Mode", -1, Mode);
          proc.AddIntegerPara("@SegmentExchangeID", Convert.ToInt32(SegmentExchangeID));
          proc.AddVarcharPara("@strFundPayoutDate", 50, strFundPayoutDate);
          proc.AddVarcharPara("@BrkgFlag", 20, BrkgFlag);
          proc.AddVarcharPara("@SettlementNumber", 50, SettlementNumber);
          proc.AddVarcharPara("@SettlementType", 10, SettlementType);
          proc.AddVarcharPara("@Branch", 10, Branch);
          proc.AddVarcharPara("@Customer", -1, Customer);
        
          ds = proc.GetDataSet();
          return ds;
      }



      public DataSet Report_BRS(string ReportView, string CompanyId, string BanckAc, string ConsiderDate, string Date, string FinYear
 )
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("Report_BRS");

          proc.AddVarcharPara("@ReportView", 10, ReportView);
          proc.AddVarcharPara("@CompanyId", 20, CompanyId);
          proc.AddVarcharPara("@BanckAc", -1, BanckAc);
          proc.AddVarcharPara("@ConsiderDate", 30, ConsiderDate);
          proc.AddVarcharPara("@Date", 50, Date);
          proc.AddVarcharPara("@FinYear", 30, FinYear);
          
          ds = proc.GetDataSet();
          return ds;
      }

      public DataSet Fetch_ClientMaster(string companyid, string segment, string IsBranchGroup, string BranchGroupValue, string fromdate, string todate,
string RPTTYPE, string CLIENTS
  )
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("Fetch_ClientMaster");

          proc.AddVarcharPara("@companyid", 50, companyid);
          proc.AddVarcharPara("@segment", -1, segment);
          proc.AddVarcharPara("@IsBranchGroup", 50, IsBranchGroup);
          proc.AddVarcharPara("@BranchGroupValue", -1, BranchGroupValue);
          proc.AddVarcharPara("@fromdate", 50, fromdate);
          proc.AddVarcharPara("@todate", 50, todate);
          proc.AddVarcharPara("@RPTTYPE", 30, RPTTYPE);
          proc.AddVarcharPara("@CLIENTS", -1, CLIENTS);
         
          ds = proc.GetDataSet();
          return ds;
      }


      public DataSet Report_DematBatchSummaryCommCurrency(string Account, string TransactionDate, string CDSLTranType
        )
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("Report_DematBatchSummaryCommCurrency");

          proc.AddVarcharPara("@Account", 150, Account);
          proc.AddVarcharPara("@TransactionDate", 50, TransactionDate);
          proc.AddVarcharPara("@CDSLTranType", 30, CDSLTranType);
        

          ds = proc.GetDataSet();
          return ds;
      }
      public DataSet Export_NsdlDematTransactionCommCurrency(string Account, string CreateUser, string TransactionDate, string ExecutionDate, string Param, string NextBatch
 )
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("Export_NsdlDematTransactionCommCurrency");

          proc.AddVarcharPara("@Account", 150, Account);
          proc.AddVarcharPara("@CreateUser", 30, CreateUser);
          proc.AddVarcharPara("@TransactionDate", 50, TransactionDate);
          proc.AddVarcharPara("@ExecutionDate", 50, ExecutionDate);
          proc.AddVarcharPara("@Param", 500, Param);
          proc.AddVarcharPara("@NextBatch", 200, NextBatch);
        

          ds = proc.GetDataSet();
          return ds;
      }




      public DataSet Export_CdslDematTransactionCommCurrency(string Account, string CreateUser, string TransactionDate, string ExecutionDate, string Param, string NextBatch
)
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("Export_CdslDematTransactionCommCurrency");

          proc.AddVarcharPara("@Account", 150, Account);
          proc.AddVarcharPara("@CreateUser", 30, CreateUser);
          proc.AddVarcharPara("@TransactionDate", 50, TransactionDate);
          proc.AddVarcharPara("@ExecutionDate", 50, ExecutionDate);
          proc.AddVarcharPara("@Param", 500, Param);
          proc.AddVarcharPara("@NextBatch", 200, NextBatch);


          ds = proc.GetDataSet();
          return ds;
      }



      public DataSet sp_Insert_ExportFiles2(string segid, string file_type, string file_name, string userid, string batch_number, string file_path
       )
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("sp_Insert_ExportFiles");

          proc.AddVarcharPara("@segid", 50, segid);
          proc.AddVarcharPara("@file_type", 50, file_type);
          proc.AddVarcharPara("@file_name", 150, file_name);
          proc.AddVarcharPara("@userid", 50, userid);
          proc.AddVarcharPara("@batch_number", 50, batch_number);
          proc.AddVarcharPara("@file_path", 200, file_path);
          proc.RunActionQuery();
          ds = proc.GetDataSet();
          return ds;
      }



      public DataSet Insert_TransactionCommCurrency(string finYear, string companyID, string segment, string clientid,
        string productseriesid, string deliverymode, string trantype, string transfertype,
        string nature, string settno, string slipno, decimal qty, string remarks,
          string generatetype, string ownaccountsource, string customeraccountsource, string ownaccountarget, string customeraccounttarget,
          string sourcedpid, string sourceclientid, string targetdpid, string targetclientid, string trandate,
          string execdate, string exchangesegmentid, string icin, string createuser
)
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("Insert_TransactionCommCurrency");
          proc.AddVarcharPara("@finYear", 20, finYear);
          proc.AddVarcharPara("@companyID", 20, companyID);
          proc.AddVarcharPara("@segment", 20, segment);
          proc.AddVarcharPara("@clientid", 20, clientid);
          proc.AddVarcharPara("@productseriesid", 20, productseriesid);
          proc.AddVarcharPara("@deliverymode", 20, deliverymode);
          proc.AddVarcharPara("@trantype", 10, trantype);
          proc.AddVarcharPara("@transfertype", 10, transfertype);
          proc.AddVarcharPara("@nature", 10, nature);
          proc.AddVarcharPara("@settno", 10, settno);
          proc.AddVarcharPara("@slipno", 40, slipno);
          proc.AddDecimalPara("@qty", 28,0, qty);
          proc.AddVarcharPara("@remarks", -1, remarks);
          proc.AddVarcharPara("@generatetype", 10, generatetype);
          proc.AddVarcharPara("@ownaccountsource", 50, ownaccountsource);
          proc.AddVarcharPara("@customeraccountsource", 50, customeraccountsource);
          proc.AddVarcharPara("@ownaccountarget", 50, ownaccountarget);
          proc.AddVarcharPara("@customeraccounttarget", 50, customeraccounttarget);
          proc.AddVarcharPara("@sourcedpid", 50, sourcedpid);
          proc.AddVarcharPara("@sourceclientid", 50, sourceclientid);
          proc.AddVarcharPara("@targetdpid", 50, targetdpid);
          proc.AddVarcharPara("@targetclientid", 50, targetclientid);
          proc.AddVarcharPara("@trandate", 50, trandate);
          proc.AddVarcharPara("@execdate", 50, execdate);
          proc.AddVarcharPara("@exchangesegmentid", 50, exchangesegmentid);
          proc.AddVarcharPara("@icin", 50, icin);
          proc.AddVarcharPara("@createuser", 20, createuser);
         

          ds = proc.GetDataSet();
          return ds;
      }




      public DataSet Report_DeliveryTransationCommCurrency(string deliverymode, string fromdate, string todate, string dpac, string Product, string ForClient
, string Clients, string finYear, string segment, string companyID)
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("Report_DeliveryTransationCommCurrency");

          proc.AddVarcharPara("@deliverymode", 10, deliverymode);
          proc.AddVarcharPara("@fromdate", 30, fromdate);
          proc.AddVarcharPara("@todate", 50, todate);
          proc.AddVarcharPara("@dpac", 50, dpac);
          proc.AddVarcharPara("@Product", -1, Product);
          proc.AddVarcharPara("@ForClient", 20, ForClient);
          proc.AddVarcharPara("@Clients", -1, Clients);
          proc.AddVarcharPara("@finYear", 20, finYear);
          proc.AddVarcharPara("@segment", 10, segment);
          proc.AddVarcharPara("@companyID", 20, companyID);
          ds = proc.GetDataSet();
          return ds;
      }




      public DataSet Report_DeliveryHoldingCommCurrency(string deliverymode, string asondate,string dpac, string Product, string finYear, string segment, string companyID)
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("Report_DeliveryHoldingCommCurrency");

          proc.AddVarcharPara("@deliverymode", 10, deliverymode);
          proc.AddVarcharPara("@asondate", 30, asondate);
          proc.AddVarcharPara("@dpac", 50, dpac);
          proc.AddVarcharPara("@Product", -1, Product);
          proc.AddVarcharPara("@finYear", 20, finYear);
          proc.AddVarcharPara("@segment", 10, segment);
          proc.AddVarcharPara("@companyID", 20, companyID);
          ds = proc.GetDataSet();
          return ds;
      }


      public DataSet Data_ICEXContract_Reportdos(string CompanyID,string DpId,string dp,string tradedate,string CustomerID,string ContractNote,
                                           string AuthorizeName,string Mode,string SegmentExchangeID,string strFundPayoutDate,string BrkgFlag,
                                           string SettlementNumber,string SettlementType,string Branch,string Customer)
      {
          ProcedureExecute proc;
          string rtrnvalue = "";
          DataSet ds = new DataSet();
          try
          {
              using (proc = new ProcedureExecute("ICEXContract_Reportdos"))
              {


                  proc.AddVarcharPara("@CompanyID", 50, CompanyID);
                  proc.AddVarcharPara("@DpId", 100, DpId);
                  proc.AddVarcharPara("@dp", 50,dp);
                  proc.AddVarcharPara("@tradedate", 50,tradedate);
                  proc.AddVarcharPara("@CustomerID", -1, CustomerID);
                  proc.AddVarcharPara("@ContractNote", -1, ContractNote);
                  proc.AddVarcharPara("@AuthorizeName", -1, AuthorizeName);
                  proc.AddVarcharPara("@Mode", -1, Mode);
                  proc.AddIntegerPara("@SegmentExchangeID", Convert.ToInt32(SegmentExchangeID));
                  proc.AddVarcharPara("@strFundPayoutDate", -1, strFundPayoutDate);
                  proc.AddVarcharPara("@BrkgFlag", 100, BrkgFlag);
                  proc.AddVarcharPara("@SettlementNumber", 100, SettlementNumber);
                  proc.AddVarcharPara("@SettlementType", 100, SettlementType);
                  proc.AddVarcharPara("@Branch",50,Branch);
                  proc.AddVarcharPara("@Customer", -1, Customer);             
                  ds = proc.GetDataSet();
                  return ds;

                                
              }
          }

          catch (Exception ex)
          {
              throw ex;
          }

          finally
          {
              proc = null;
          }
      }






      public DataSet Get_Contract_Report_BSECMdos(string CompanyID, string DpId, string dp, string tradedate, string CustomerID, string ContractNote,
                                          string AuthorizeName, string Mode, string SegmentExchangeID, string strFundPayoutDate, string BrkgFlag,
                                          string SettlementNumber, string SettlementType, string Branch, string Customer,
                                          string FromNo,string ToNo)
      {
          ProcedureExecute proc;
          string rtrnvalue = "";
          DataSet ds = new DataSet();
          try
          {
              using (proc = new ProcedureExecute("Contract_Report_BSECMdos"))
              {


                  proc.AddVarcharPara("@CompanyID", 50, CompanyID);
                  proc.AddVarcharPara("@DpId", 100, DpId);
                  proc.AddVarcharPara("@dp", 50, dp);
                  proc.AddVarcharPara("@tradedate", 50, tradedate);
                  proc.AddVarcharPara("@CustomerID", -1, CustomerID);
                  proc.AddVarcharPara("@ContractNote", -1, ContractNote);
                  proc.AddVarcharPara("@AuthorizeName", -1, AuthorizeName);
                  proc.AddVarcharPara("@Mode", -1, Mode);
                  proc.AddIntegerPara("@SegmentExchangeID", Convert.ToInt32(SegmentExchangeID));
                  proc.AddVarcharPara("@strFundPayoutDate", -1, strFundPayoutDate);
                  proc.AddVarcharPara("@BrkgFlag", 100, BrkgFlag);
                  proc.AddVarcharPara("@SettlementNumber", 100, SettlementNumber);
                  proc.AddVarcharPara("@SettlementType", 100, SettlementType);
                  proc.AddVarcharPara("@Branch", 50, Branch);
                  proc.AddVarcharPara("@Customer", -1, Customer);
                  proc.AddVarcharPara("@FromNo", -1, FromNo);
                  proc.AddVarcharPara("@ToNo", -1, ToNo);
                  ds = proc.GetDataSet();

                                   

                  return ds;


              }
          }

          catch (Exception ex)
          {
              throw ex;
          }

          finally
          {
              proc = null;
          }
      }




      public DataSet Get_Contract_Report13(string CompanyID, string DpId, string dp, string tradedate, string CustomerID, string ContractNote,
                                         string AuthorizeName, string Mode, string SegmentExchangeID, string strFundPayoutDate, string BrkgFlag,
                                         string SettlementNumber, string SettlementType, string Branch, string Customer,
                                         string FromNo, string ToNo)
      {
          ProcedureExecute proc;
          string rtrnvalue = "";
          DataSet ds = new DataSet();
          try
          {
              using (proc = new ProcedureExecute("Contract_Report13"))
              {


                  proc.AddVarcharPara("@CompanyID", 50, CompanyID);
                  proc.AddVarcharPara("@DpId", 100, DpId);
                  proc.AddVarcharPara("@dp", 50, dp);
                  proc.AddVarcharPara("@tradedate", 50, tradedate);
                  proc.AddVarcharPara("@CustomerID", -1, CustomerID);
                  proc.AddVarcharPara("@ContractNote", -1, ContractNote);
                  proc.AddVarcharPara("@AuthorizeName", -1, AuthorizeName);
                  proc.AddVarcharPara("@Mode", -1, Mode);
                  proc.AddIntegerPara("@SegmentExchangeID", Convert.ToInt32(SegmentExchangeID));
                  proc.AddVarcharPara("@strFundPayoutDate", -1, strFundPayoutDate);
                  proc.AddVarcharPara("@BrkgFlag", 100, BrkgFlag);
                  proc.AddVarcharPara("@SettlementNumber", 100, SettlementNumber);
                  proc.AddVarcharPara("@SettlementType", 100, SettlementType);
                  proc.AddVarcharPara("@Branch", 50, Branch);
                  proc.AddVarcharPara("@Customer", -1, Customer);
                  proc.AddVarcharPara("@FromNo", -1, FromNo);
                  proc.AddVarcharPara("@ToNo", -1, ToNo);
                  ds = proc.GetDataSet();



                  return ds;


              }
          }

          catch (Exception ex)
          {
              throw ex;
          }

          finally
          {
              proc = null;
          }
      }



      public DataSet Get_Contract_AnnextureSttax_DosPrint(string CompanyID, string DpId, string dp, string CustomerID, string tradedate,string ContractNote,
                                         string SettlementNumber, string SettlementType)
      {
          ProcedureExecute proc;
          string rtrnvalue = "";
          DataSet ds = new DataSet();
          try
          {
              using (proc = new ProcedureExecute("Contract_AnnextureSttax_DosPrint"))
              {


                  proc.AddVarcharPara("@CompanyID", 50, CompanyID);
                  proc.AddVarcharPara("@DpId", 100, DpId);
                  proc.AddVarcharPara("@dp", 50, dp);
                  proc.AddVarcharPara("@CustomerID", -1, CustomerID);
                  proc.AddVarcharPara("@tradedate", 50, tradedate);
                  proc.AddVarcharPara("@ContractNote", -1, ContractNote);
                  proc.AddVarcharPara("@SettlementNumber", 100, SettlementNumber);
                  proc.AddVarcharPara("@SettlementType", 100, SettlementType);
                  ds = proc.GetDataSet();

                  return ds;


              }
          }

          catch (Exception ex)
          {
              throw ex;
          }

          finally
          {
              proc = null;
          }
      }



      public DataSet Get_Contract_Report(string CompanyID, string DpId, string dp, string tradedate, string CustomerID, string ContractNote,
                                        string AuthorizeName, string Mode, string SegmentExchangeID, string strFundPayoutDate, string BrkgFlag,
                                        string SettlementNumber, string SettlementType)
      {
          ProcedureExecute proc;
          string rtrnvalue = "";
          DataSet ds = new DataSet();
          try
          {
              using (proc = new ProcedureExecute("Contract_Report"))
              {


                  proc.AddVarcharPara("@CompanyID", 50, CompanyID);
                  proc.AddVarcharPara("@DpId", 100, DpId);
                  proc.AddVarcharPara("@dp", 50, dp);
                  proc.AddVarcharPara("@tradedate", 50, tradedate);
                  proc.AddVarcharPara("@CustomerID", -1, CustomerID);
                  proc.AddVarcharPara("@ContractNote", -1, ContractNote);
                  proc.AddVarcharPara("@AuthorizeName", -1, AuthorizeName);
                  proc.AddVarcharPara("@Mode", -1, Mode);
                  proc.AddIntegerPara("@SegmentExchangeID", Convert.ToInt32(SegmentExchangeID));
                  proc.AddVarcharPara("@strFundPayoutDate", -1, strFundPayoutDate);
                  proc.AddVarcharPara("@BrkgFlag", 100, BrkgFlag);
                  proc.AddVarcharPara("@SettlementNumber", 100, SettlementNumber);
                  proc.AddVarcharPara("@SettlementType", 100, SettlementType);
               

                
                  ds = proc.GetDataSet();



                  return ds;


              }
          }

          catch (Exception ex)
          {
              throw ex;
          }

          finally
          {
              proc = null;
          }
      }



      public DataSet Get_CLIENTSECURITY(string FINYEAR,string FROMDATE,string TODATE,string SEGMENT,string COMPANYID,string CLIENTS,string BRANCHID)
      {
          ProcedureExecute proc;
          string rtrnvalue = "";
          DataSet ds = new DataSet();
          try
          {
              using (proc = new ProcedureExecute("CLIENTSECURITY"))
              {

                  proc.AddVarcharPara("@FINYEAR", 50, FINYEAR);
                  proc.AddVarcharPara("@FROMDATE", 100, FROMDATE);
                  proc.AddVarcharPara("@TODATE", 50, TODATE);
                  proc.AddVarcharPara("@SEGMENT", 50, SEGMENT);
                  proc.AddVarcharPara("@COMPANYID", 50, COMPANYID);
                  proc.AddVarcharPara("@CLIENTS", -1, CLIENTS);
                  proc.AddVarcharPara("@BRANCHID", -1, BRANCHID);    

                  ds = proc.GetDataSet();

                  return ds;


              }
          }

          catch (Exception ex)
          {
              throw ex;
          }

          finally
          {
              proc = null;
          }
      }


      public void Update_Cheque( string Doc,string UserID,string UsersegID,string Company,string customertype)
      {
          ProcedureExecute proc;
          int rtrnvalue = 0;
         
          try
          {
              using (proc = new ProcedureExecute("Update_Cheque"))
              {

                  proc.AddNVarcharPara("@Doc", -1, Doc);
                  proc.AddVarcharPara("@UserID", 100, UserID);
                  proc.AddVarcharPara("@UsersegID", 50, UsersegID);
                  proc.AddVarcharPara("@Company", 50, Company);
                  proc.AddVarcharPara("@customertype", 50, customertype);
                  rtrnvalue= proc.RunActionQuery();

              }
          }

          catch (Exception ex)
          {
              throw ex;
          }

          finally
          {
              proc = null;
          }
      }




      public DataSet CFOrMTMChargeStatement(string FromDate, string ToDate, string ClientId, string Scrip, string Segment, string Companyid,
          string GrpType,string GrpId,string BranchHierchy,string RptView,string SPName)
          
                {
          ProcedureExecute proc;
          string rtrnvalue = "";
          DataSet ds = new DataSet();
          try
          {
              using (proc = new ProcedureExecute(SPName))
              {

                  proc.AddVarcharPara("@FromDate", 50, FromDate);
                  proc.AddVarcharPara("@ToDate", 50, ToDate);
                  proc.AddVarcharPara("@ClientId", -1, ClientId);
                  proc.AddVarcharPara("@Scrip", -1, Scrip);
                  proc.AddVarcharPara("@Segment", 50, Segment);
                  proc.AddVarcharPara("@Companyid", 50, Companyid);
                  proc.AddVarcharPara("@GrpType", 50, GrpType);
                  proc.AddVarcharPara("@GrpId", -1, GrpId);
                  proc.AddVarcharPara("@BranchHierchy", -1, BranchHierchy);
                  proc.AddVarcharPara("@RptView", 50, RptView);

                  ds = proc.GetDataSet();

                  return ds;


              }
          }

          catch (Exception ex)
          {
              throw ex;
          }

          finally
          {
              proc = null;
          }
      }
  }
}
 