using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccessLayer;

namespace BusinessLogicLayer
{
   public partial class DailyTask_GenerateDemat
    {

       public DataSet Report_DeliveryProcessing(string Finyear, string CompanyID, string SegmentID, string PayoutDate, string Client,
          string branchhiearchy, string Scrip, string GrpId, string GrpType, string vType, string ReleaseAcDetails)
       {
           DataSet dt = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("Report_DeliveryProcessing");
           proc.AddVarcharPara("@Finyear", 100, Finyear);
           proc.AddNVarcharPara("@CompanyID", 100, CompanyID);
           proc.AddVarcharPara("@SegmentID", 100, SegmentID);
           proc.AddVarcharPara("@PayoutDate", 100, PayoutDate);
           proc.AddNVarcharPara("@Client", -1, Client);
           proc.AddVarcharPara("@branchhiearchy", -1, branchhiearchy);
           proc.AddVarcharPara("@Scrip", -1, Scrip);
           proc.AddNVarcharPara("@GrpId", -1, GrpId);
           proc.AddVarcharPara("@GrpType", 100, GrpType);
           proc.AddVarcharPara("@Type", 100, vType);
           proc.AddNVarcharPara("@ReleaseAcDetails", 100, ReleaseAcDetails);
           dt = proc.GetDataSet();
           return dt;
       }


       public int Process_InterAccountTransfer(string TABLEReport, string Finyear, string CompanyID, string PositingDate, string MenuId, string Remarks,
           string Createuser,string ReleaseFromAcDetails,string ReleaseToAcDetails,string Segmentid)
       {
           ProcedureExecute proc;
           int rtrnvalue = 0;
           try
           {
               using (proc = new ProcedureExecute("Process_InterAccountTransfer"))
               {

                   proc.AddVarcharPara("@TABLEReport", 100, TABLEReport);
                   proc.AddVarcharPara("@Finyear", 100, Finyear);
                   proc.AddVarcharPara("@CompanyID", 100, CompanyID);
                   proc.AddVarcharPara("@PositingDate", 100, PositingDate);
                   proc.AddVarcharPara("@MenuId", 100,MenuId);
                   proc.AddVarcharPara("@Remarks", 100, Remarks);
                   proc.AddIntegerPara("@Createuser", Convert.ToInt32(Createuser));
                   proc.AddVarcharPara("@ReleaseFromAcDetails", 100, ReleaseFromAcDetails);
                   proc.AddVarcharPara("@ReleaseToAcDetails",100, ReleaseToAcDetails);
                   proc.AddVarcharPara("@Segmentid", 100, Segmentid);

                                    
                   
                   rtrnvalue = proc.RunActionQuery();
                   //  rtrnvalue = proc.GetParaValue("@ResultCode").ToString();
                   return rtrnvalue;


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


       public DataSet Processing_Auction(string TableData,string Finyear,string CompanyID,string SegmentID,string PendingPosition,
           string TransferTo,string MasterSegment,string CreateUser)
       {
           DataSet dt = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("Processing_Auction");
           proc.AddVarcharPara("@TableData", 100, TableData);
           proc.AddNVarcharPara("@Finyear", 100, Finyear);
           proc.AddVarcharPara("@CompanyID", 100, CompanyID);
           proc.AddVarcharPara("@SegmentID", 100, SegmentID);
           proc.AddNVarcharPara("@PendingPosition", 100, PendingPosition);
           proc.AddVarcharPara("@TransferTo", 100, TransferTo);
           proc.AddVarcharPara("@MasterSegment", 100, MasterSegment);
           proc.AddNVarcharPara("@CreateUser", 100, CreateUser);
         
           dt = proc.GetDataSet();
           return dt;
       }


       public int DeliveryProcessingMarginHoldBack(string clientPayoutData, string Finyear, string compID, string segmentid,
                                  string TransferDate, string CreateUser)
       {
           ProcedureExecute proc;
           int rtrnvalue = 0;
           try
           {
               using (proc = new ProcedureExecute("DeliveryProcessingMarginHoldBack"))
               {

                   proc.AddVarcharPara("@clientPayoutData", 100, clientPayoutData);
                   proc.AddVarcharPara("@Finyear", 100, Finyear);
                   proc.AddVarcharPara("@compID", 100, compID);
                   proc.AddVarcharPara("@segmentid", 100, segmentid);
                   proc.AddDateTimePara("@TransferDate", Convert.ToDateTime(TransferDate));
                   proc.AddVarcharPara("@CreateUser", 100, CreateUser);



                   rtrnvalue = proc.RunActionQuery();
                   //  rtrnvalue = proc.GetParaValue("@ResultCode").ToString();
                   return rtrnvalue;


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



       public int DeliveryProcessingPayInOwnAccount(string PayInOwnAccount, string Finyear, string compID, string segmentid,
                                  string TransferDate, string CreateUser)
       {
           ProcedureExecute proc;
           int rtrnvalue = 0;
           try
           {
               using (proc = new ProcedureExecute("DeliveryProcessingPayInOwnAccount"))
               {

                   proc.AddVarcharPara("@PayInOwnAccount", 100, PayInOwnAccount);
                   proc.AddVarcharPara("@Finyear", 100, Finyear);
                   proc.AddVarcharPara("@compID", 100, compID);
                   proc.AddVarcharPara("@segmentid", 100, segmentid);
                   proc.AddDateTimePara("@TransferDate", Convert.ToDateTime(TransferDate));
                   proc.AddVarcharPara("@CreateUser", 100, CreateUser);



                   rtrnvalue = proc.RunActionQuery();
                   //  rtrnvalue = proc.GetParaValue("@ResultCode").ToString();
                   return rtrnvalue;


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


       public DataSet Processing_ClientPayout( string PayoutData,string Finyear,string CompanyID,string StocksSettlementNumber,
           string StocksAcId,string TransferDate,string PositionSettlementNumber,string CreateUser,string SegmentID,
               string Remarks,string RecieveAll,string PoolAcc
)
       {
           DataSet dt = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("Processing_ClientPayout");
           proc.AddVarcharPara("@PayoutData", 100, PayoutData);
           proc.AddNVarcharPara("@Finyear", 100, Finyear);
           proc.AddVarcharPara("@CompanyID", 100, CompanyID);
           proc.AddVarcharPara("@StocksSettlementNumber", 100, StocksSettlementNumber);
           proc.AddNVarcharPara("@StocksAcId", 100, StocksAcId);
           proc.AddVarcharPara("@TransferDate", 100, TransferDate);
           proc.AddVarcharPara("@PositionSettlementNumber", 100, PositionSettlementNumber);
           proc.AddNVarcharPara("@CreateUser", 100, CreateUser);
           proc.AddNVarcharPara("@SegmentID", 100, SegmentID);
           proc.AddNVarcharPara("@Remarks", 100, Remarks);
           proc.AddNVarcharPara("@RecieveAll", 100, RecieveAll);
           proc.AddNVarcharPara("@PoolAcc", 100, PoolAcc);
                  
           dt = proc.GetDataSet();
           return dt;
       }




       public int DeliveryProcessingPOAClient( string clientPayoutData,string Finyear,string compID,string segmentid,string settlementNumber,
           string settlementType,string TransferDate,string CreateUser,string TargetAC,string SlipNumber)
       {
           int rtrnvalue = 0;
           ProcedureExecute proc = new ProcedureExecute("DeliveryProcessingPOAClient");
           proc.AddVarcharPara("@clientPayoutData", 100, clientPayoutData);
           proc.AddNVarcharPara("@Finyear", 100, Finyear);
           proc.AddVarcharPara("@compID", 100, compID);
           proc.AddVarcharPara("@segmentid", 100, segmentid);
           proc.AddNVarcharPara("@settlementNumber", 100, settlementNumber);
           proc.AddVarcharPara("@settlementType", 100, settlementType);
           proc.AddDateTimePara("@TransferDate", Convert.ToDateTime(TransferDate));
           proc.AddNVarcharPara("@CreateUser", 100, CreateUser);
           proc.AddNVarcharPara("@TargetAC", 100, TargetAC);
           proc.AddNVarcharPara("@SlipNumber", 100, SlipNumber);


           rtrnvalue = proc.RunActionQuery();
           //  rtrnvalue = proc.GetParaValue("@ResultCode").ToString();
           return rtrnvalue;
       }



       public int DeliveryProcessingPayInMarginHoldBack(string PayInMarginHoldBack, string Finyear, string compID, string segmentid,
                                string TransferDate, string CreateUser)
       {
           ProcedureExecute proc;
           int rtrnvalue = 0;
           try
           {
               using (proc = new ProcedureExecute("DeliveryProcessingPayInMarginHoldBack"))
               {

                   proc.AddVarcharPara("@PayInMarginHoldBack", 100, PayInMarginHoldBack);
                   proc.AddVarcharPara("@Finyear", 100, Finyear);
                   proc.AddVarcharPara("@compID", 100, compID);
                   proc.AddVarcharPara("@segmentid", 100, segmentid);
                   proc.AddDateTimePara("@TransferDate", Convert.ToDateTime(TransferDate));
                   proc.AddVarcharPara("@CreateUser", 100, CreateUser);



                   rtrnvalue = proc.RunActionQuery();
                   //  rtrnvalue = proc.GetParaValue("@ResultCode").ToString();
                   return rtrnvalue;


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
