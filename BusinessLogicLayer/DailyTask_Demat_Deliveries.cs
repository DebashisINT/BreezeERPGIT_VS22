using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DataAccessLayer;

namespace BusinessLogicLayer
{
  public partial class DailyTask_Demat_Deliveries
    {



      public DataSet sp_Show_Unreconciled_DematTransactions(string exchSegId)
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("sp_Show_Unreconciled_DematTransactions");


          proc.AddVarcharPara("@exchSegId", 100, exchSegId);
         
          ds = proc.GetDataSet();
          return ds;
      }


      public DataSet DeleteTransaction(string TranID, string CreateUser)
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("DeleteTransaction");

          proc.AddIntegerPara("@TranID", Convert.ToInt32(TranID));
          proc.AddIntegerPara("@CreateUser", Convert.ToInt32(CreateUser));

          ds = proc.GetDataSet();
          return ds;
      }

      public DataSet BatchFetch_Cancel(string SegmentID, string ExportDate)
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("BatchFetch_Cancel");

          proc.AddVarcharPara("@SegmentID",100, SegmentID);
          proc.AddVarcharPara("@ExportDate", 100, ExportDate);

          ds = proc.GetDataSet();
          return ds;
      }


      public SqlDataReader Check_BatchDelete(string BatchNo, string ExportDate)
      {
          SqlDataReader ds ;
          ProcedureExecute proc = new ProcedureExecute("Check_BatchDelete");

          proc.AddVarcharPara("@BatchNo", 100, BatchNo);
          proc.AddVarcharPara("@ExportDate", 100, ExportDate);

          ds = proc.GetReader();
          return ds;
      }

      public int DeleteBatch(string TransactionIds, string CreateUser)
      {
          ProcedureExecute proc;
          int rtrnvalue = 0;
          try
          {
              using (proc = new ProcedureExecute("DeleteBatch"))
              {

                  proc.AddVarcharPara("@TransactionIds", 100, TransactionIds);
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


      public DataSet FetchDeliveryProcessing( string SettlementNumber,string SettlementType,string FinYear,string CompanyID,
          string SegmentID,string PayoutDate,string Client,string Scrip,string AllCMSegment)
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("FetchDeliveryProcessing");

          proc.AddVarcharPara("@SettlementNumber", 100, SettlementNumber);
          proc.AddVarcharPara("@SettlementType", 100, SettlementType);
          proc.AddVarcharPara("@FinYear", 100, FinYear);
          proc.AddVarcharPara("@CompanyID", 100, CompanyID);
          proc.AddVarcharPara("@SegmentID", 100, SegmentID);
          proc.AddDateTimePara("@PayoutDate", Convert.ToDateTime(PayoutDate));
          proc.AddVarcharPara("@Client", -1, Client);

          proc.AddVarcharPara("@Scrip", -1, Scrip);
          proc.AddVarcharPara("@AllCMSegment", 100, AllCMSegment);
         

          ds = proc.GetDataSet();
          return ds;
      }




      public DataSet Report_FetchForInterSegment( string CompanyID,string SegmentID,string PayIn,string ExchangeSegmentID,string SettNumIntSeg,
          string SettTypeIntSeg,string DematAccID,string FinYear)
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("Report_FetchForInterSegment");

          proc.AddVarcharPara("@CompanyID", 100, CompanyID);
          proc.AddVarcharPara("@SegmentID", 100, SegmentID);
          proc.AddVarcharPara("@PayIn", 100, PayIn);
          proc.AddVarcharPara("@ExchangeSegmentID", 100, ExchangeSegmentID);
          proc.AddVarcharPara("@SettNumIntSeg", 100, SettNumIntSeg);
          proc.AddVarcharPara("@SettTypeIntSeg",100,SettTypeIntSeg);
          proc.AddVarcharPara("@DematAccID", 100, DematAccID);
          proc.AddVarcharPara("@FinYear", 100, FinYear);
        
          
          ds = proc.GetDataSet();
          return ds;
      }


      public DataSet FetchForMarketPayIN( string SettlementNumber,string SettlementType,string CompanyID,string SegmentID,string AccID,string FinYear)
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("FetchForMarketPayIN");

          proc.AddVarcharPara("@SettlementNumber", 100, SettlementNumber);
          proc.AddVarcharPara("@SettlementType", 100, SettlementType);
          proc.AddVarcharPara("@CompanyID", 100, CompanyID);
          proc.AddVarcharPara("@SegmentID", 100, SegmentID);
          proc.AddVarcharPara("@AccID", 100, AccID);
          proc.AddVarcharPara("@FinYear", 100, FinYear);
          ds = proc.GetDataSet();
          return ds;
      }


      public DataSet PayINFromOwnAccount(string companyID, string SegmentID, string PayIn, string OwnAccountID, string FinYear)
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("PayINFromOwnAccount");

          proc.AddVarcharPara("@companyID", 100, companyID);
          proc.AddVarcharPara("@SegmentID", 100, SegmentID);
          proc.AddVarcharPara("@PayIn", 100, PayIn);
          proc.AddIntegerPara("@OwnAccountID", Convert.ToInt32(OwnAccountID));
          proc.AddVarcharPara("@FinYear", 100, FinYear);
        
  

          ds = proc.GetDataSet();
          return ds;
      }


      public DataSet FertchPayINFromMarginHoldBack(string companyID, string SegmentID, string PayIn, string SettNum,string SettType, 
          string PayInHoldBackAccount, string FinYear)
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("FertchPayINFromMarginHoldBack");

          proc.AddVarcharPara("@companyID", 100, companyID);
          proc.AddVarcharPara("@SegmentID", 100, SegmentID);
          proc.AddDateTimePara("@PayIn", Convert.ToDateTime(PayIn));
          proc.AddVarcharPara("@SettNum", 100, SettNum);
          proc.AddVarcharPara("@SettType", 100, SettType);
          proc.AddIntegerPara("@PayInHoldBackAccount", Convert.ToInt32(PayInHoldBackAccount));
          proc.AddVarcharPara("@FinYear", 100, FinYear);

       
          ds = proc.GetDataSet();
          return ds;
      }


      public DataSet FetchForOffSetPosition(string companyID, string SegmentID,string SettNumberTarget,string SettTypeTarget,
                                             string SettNumberSource,string SettTypeSource,string FinYear)
      {
          DataSet ds = new DataSet();
          ProcedureExecute proc = new ProcedureExecute("FetchForOffSetPosition");

          proc.AddVarcharPara("@companyID", 100, companyID);
          proc.AddVarcharPara("@SegmentID", 100, SegmentID);
          proc.AddVarcharPara("@SettNumberTarget", 100, SettNumberTarget);
          proc.AddVarcharPara("@SettTypeTarget", 100, SettTypeTarget);
          proc.AddVarcharPara("@SettNumberSource", 100, SettNumberSource);
          proc.AddVarcharPara("@SettTypeSource",100, SettTypeSource);
          proc.AddVarcharPara("@FinYear", 100, FinYear);

          ds = proc.GetDataSet();
          return ds;
      }


      public int DeliveryProcessingClientPayout(string clientPayoutData, string Finyear, string compID, string segmentid, string settlementNumber,
          string settlementType,string TransferDate,string CreateUser)
      {
          ProcedureExecute proc;
          int rtrnvalue = 0;
          try
          {
              using (proc = new ProcedureExecute("DeliveryProcessingClientPayout"))
              {

                  proc.AddVarcharPara("@clientPayoutData", 100, clientPayoutData);
                  proc.AddVarcharPara("@Finyear", 100, Finyear);
                  proc.AddVarcharPara("@compID", 100, compID);
                  proc.AddVarcharPara("@segmentid", 100, segmentid);
                  proc.AddVarcharPara("@settlementNumber", 100, settlementNumber);
                  proc.AddVarcharPara("@settlementType", 100, settlementType);
                  proc.AddDateTimePara("@TransferDate",Convert.ToDateTime(TransferDate));
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



      public int DeliveryProcessingInterSettlement(string InterSettlementData, string Finyear, string compID, string segmentid, 
                                       string TransferDate, string CreateUser)
      {
          ProcedureExecute proc;
          int rtrnvalue = 0;
          try
          {
              using (proc = new ProcedureExecute("DeliveryProcessingInterSettlement"))
              {

                  proc.AddVarcharPara("@InterSettlementData", 100, InterSettlementData);
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


      public int DeliveryProcessingMarketPayIN(string MarketPayINData, string Finyear, string compID, string segmentid,
                                    string TransferDate, string CreateUser,string MrPayIN)
      {
          ProcedureExecute proc;
          int rtrnvalue = 0;
          try
          {
              using (proc = new ProcedureExecute("DeliveryProcessingMarketPayIN"))
              {

                  proc.AddVarcharPara("@MarketPayINData", 100, MarketPayINData);
                  proc.AddVarcharPara("@Finyear", 100, Finyear);
                  proc.AddVarcharPara("@compID", 100, compID);
                  proc.AddVarcharPara("@segmentid", 100, segmentid);
                  proc.AddDateTimePara("@TransferDate", Convert.ToDateTime(TransferDate));
                  proc.AddVarcharPara("@CreateUser", 100, CreateUser);
                  proc.AddVarcharPara("@MrPayIN", 100, MrPayIN);



                
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


      public int DeliveryProcessingForOffsetPosition(string OffsetPositionData, string Finyear, string compID, string segmentid,
                                 string TransferDate, string CreateUser)
      {
          ProcedureExecute proc;
          int rtrnvalue = 0;
          try
          {
              using (proc = new ProcedureExecute("DeliveryProcessingForOffsetPosition"))
              {

                  proc.AddVarcharPara("@OffsetPositionData", 100, OffsetPositionData);
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
