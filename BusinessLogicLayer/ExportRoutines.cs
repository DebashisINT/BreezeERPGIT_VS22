using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;
using System.Data;

namespace BusinessLogicLayer
{
    public partial class ExportRoutines
    {
        public void Fetch_CashBankDepositSlipsDetails
            (string BankID, 
            string TransactionDate, 
            string PrintDateTime, 
            string Checknull, 
            string CompanyID)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {
                using (proc = new ProcedureExecute("Fetch_CashBankDepositSlipsDetails"))
                {
                    proc.AddVarcharPara("@BankID", 50, BankID);
                    proc.AddVarcharPara("@TransactionDate", 100, TransactionDate);
                    proc.AddVarcharPara("@PrintDateTime", 50, PrintDateTime);
                    proc.AddVarcharPara("@Checknull", 50, Checknull);
                    proc.AddVarcharPara("@CompanyID", 50, CompanyID);
                    int i = proc.RunActionQuery();
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

        public DataSet Fetch_CashBankDepositSlips(string BankID,string TransactionDate)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Fetch_CashBankDepositSlips");
            proc.AddVarcharPara("@BankID", 100, BankID);
            proc.AddVarcharPara("@TransactionDate", 100, TransactionDate); 
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet Sp_MONTHLYDISCLOSURE( string segment,string companyid,string reportdate,string finyear,string month,string obligationamnt,string exchsegment)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_MONTHLYDISCLOSURE");
            proc.AddVarcharPara("@segment", 100, segment);
            proc.AddVarcharPara("@companyid", 100, companyid);
            proc.AddVarcharPara("@reportdate", 100, reportdate);
            proc.AddVarcharPara("@finyear", 100, finyear);
            proc.AddVarcharPara("@month", 100, month);
            proc.AddVarcharPara("@obligationamnt", 100, obligationamnt);
           
            proc.AddBigIntegerPara("@exchsegment",  Convert.ToInt64(exchsegment));
          

          

            ds = proc.GetDataSet();
            return ds;
        }


        public void Insert_ExportFiles(string segid, string file_type, string file_name, string userid, string batch_number, string file_path)
        {
            ProcedureExecute proc;
            int rtrnvalue = 0;
            try
            {
                using (proc = new ProcedureExecute("sp_Insert_ExportFiles"))
                {

                    proc.AddVarcharPara("@segid", 100, segid);
                    proc.AddVarcharPara("@file_type", 100, file_type);
                    proc.AddVarcharPara("@file_name", 255, file_name);
                    proc.AddVarcharPara("@userid",100, userid);
                    proc.AddVarcharPara("@batch_number",100, batch_number);
                    proc.AddVarcharPara("@file_path", 200, file_path);

                                                       

                    rtrnvalue = proc.RunActionQuery();
                    //  rtrnvalue = proc.GetParaValue("@ResultCode").ToString();
                    // return rtrnvalue;


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


        public DataSet Customfiles( string companyid,string segment,string fromdate,string todate,string clients,string MasterSegment,string filetype)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Customfiles");
            proc.AddVarcharPara("@companyid", 100, companyid);
            proc.AddVarcharPara("@segment", 100, segment);
            proc.AddVarcharPara("@fromdate", 100, fromdate);
            proc.AddVarcharPara("@todate", 100, todate);
            proc.AddVarcharPara("@clients", -1, clients);
            proc.AddVarcharPara("@MasterSegment", 100, MasterSegment);
            proc.AddVarcharPara("@filetype", 100, filetype);

            ds = proc.GetDataSet();
            return ds;
        }


        public DataSet SP_UnreconsiledTradesCOMM( string segment,string SettNo,string SettType,string date,string Companyid,string MasterSegment, 
            string vType,string Client,string Instrument,string Terminalid,string CTCLID,string spname)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute(spname);
            proc.AddVarcharPara("@segment", 100, segment);
            proc.AddVarcharPara("@SettNo", 100, SettNo);
            proc.AddVarcharPara("@SettType", 100, SettType);
            proc.AddVarcharPara("@date", 100, date);
            proc.AddVarcharPara("@Companyid", -1, Companyid);
            proc.AddVarcharPara("@MasterSegment", 100, MasterSegment);
            proc.AddVarcharPara("@Type", 100, vType);
            proc.AddVarcharPara("@Client", -1, Client);
            proc.AddVarcharPara("@Instrument", -1, Instrument);
            proc.AddVarcharPara("@Terminalid", -1, Terminalid);
            proc.AddVarcharPara("@CTCLID", -1, CTCLID);
                    
            ds = proc.GetDataSet();
            return ds;
        }



        public DataSet TradeChangeNSEBSEDISPLAY(string date,string ClientsID,string segment,string Companyid,string Instruments,string OrderNo,
            string Terminalid ,string CTCLID ,string TIME,string Settno ,string Setttype ,string MasterSegment, string tradetype,string actype 
                ,string actypeCli,string mktprice,string Sortorder,string TranType)
        {

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("TradeChangeNSEBSEDISPLAY");

            proc.AddVarcharPara("@date", 100, date);
            proc.AddVarcharPara("@ClientsID", -1, ClientsID);
            proc.AddVarcharPara("@segment", 100, segment);
            proc.AddVarcharPara("@Companyid", 100, Companyid);
            proc.AddVarcharPara("@Instruments", -1, Instruments);
            proc.AddVarcharPara("@OrderNo", -1, OrderNo);
            proc.AddVarcharPara("@Terminalid", -1, Terminalid);
            proc.AddVarcharPara("@CTCLID", -1, CTCLID);
            proc.AddVarcharPara("@TIME", -1, TIME);
            proc.AddVarcharPara("@Settno", 100, Settno);
            proc.AddVarcharPara("@Setttype", 100, Setttype);
            proc.AddVarcharPara("@MasterSegment", 100, MasterSegment);
            proc.AddVarcharPara("@tradetype", 100, tradetype);
            proc.AddVarcharPara("@actype", 100, actype);
            proc.AddVarcharPara("@actypeCli", -1, actypeCli);
            proc.AddVarcharPara("@mktprice", -1, mktprice);
            proc.AddVarcharPara("@Sortorder", 100, Sortorder);
            proc.AddVarcharPara("@TranType", 100, TranType);
                                 

            ds = proc.GetDataSet();
            return ds;
        }


        public int TRADECHANGE( string TABLEReport,string date,string segment,string Companyid,string createuser,string RPTTOEXCHANGE,string SETTNO,
            string SETTTYPE,string FROMCLIENT,string TOCLIENT)
        {
            ProcedureExecute proc;
            int rtrnvalue = 0;
            try
            {
                using (proc = new ProcedureExecute("TRADECHANGE"))
                {

                    proc.AddVarcharPara("@TABLEReport", 100, TABLEReport);
                    proc.AddVarcharPara("@date", 100, date);
                    proc.AddVarcharPara("@segment", 255, segment);
                    proc.AddVarcharPara("@Companyid", 100, Companyid);
                    proc.AddVarcharPara("@createuser", 100, createuser);
                    proc.AddVarcharPara("@RPTTOEXCHANGE", 200, RPTTOEXCHANGE);
                    proc.AddVarcharPara("@SETTNO", 255, SETTNO);
                    proc.AddVarcharPara("@SETTTYPE", 100, SETTTYPE);
                    proc.AddVarcharPara("@FROMCLIENT", 100, FROMCLIENT);
                    proc.AddVarcharPara("@TOCLIENT", 200, TOCLIENT);

                  
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


        public DataSet TRADEDELETE( string SEGMENT,string FROMDATE,string TODATE,string COMPANYID,string SETTNO,string SETTTYPE,
                                    string TRADETYPE,string EXCHANGESEGMENT,string BRANCHID,string TERMINALID,string CLIENTID,string SCRIPID)
        {

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("TRADEDELETE");

            proc.AddVarcharPara("@SEGMENT", 100, SEGMENT);
            proc.AddVarcharPara("@FROMDATE", 100, FROMDATE);
            proc.AddVarcharPara("@TODATE", 100, TODATE);
            proc.AddVarcharPara("@COMPANYID", 100, COMPANYID);
            proc.AddVarcharPara("@SETTNO", 100, SETTNO);
            proc.AddVarcharPara("@SETTTYPE", 100, SETTTYPE);
            proc.AddVarcharPara("@TRADETYPE", 100, TRADETYPE);
            proc.AddVarcharPara("@EXCHANGESEGMENT", 5000, EXCHANGESEGMENT);
            proc.AddVarcharPara("@BRANCHID", -1, BRANCHID);
            proc.AddVarcharPara("@TERMINALID", -1, TERMINALID);
            proc.AddVarcharPara("@CLIENTID", -1, CLIENTID);
           
                    

            ds = proc.GetDataSet();
            return ds;
        }



        public DataSet SP_BrokerageChangeDisplayNSECM( string segment,string Companyid,string MasterSegment,string Client,string date,
                                                        string SettNo,string SettType,string Instrument)
        {

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("SP_BrokerageChangeDisplayNSECM");

            proc.AddIntegerPara("@segment", Convert.ToInt32(segment));
            proc.AddVarcharPara("@Companyid", 100, Companyid);
            proc.AddIntegerPara("@MasterSegment", Convert.ToInt32(MasterSegment));
            proc.AddVarcharPara("@Client", 100, Client);
            proc.AddVarcharPara("@date", 100, date);
            proc.AddVarcharPara("@SettNo", 100, SettNo);
            proc.AddVarcharPara("@SettType", 100, SettType);
            proc.AddVarcharPara("@Instrument", -1, Instrument);
           
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet sp_Export_NsdlDematTransaction1(string userid, string accid, string transdate, string execdate, string param_batch,
            string batch_num, string slip_MI, string slip_Intra, string slip_Inter, string slip_IS,string create_datetime, string version, string dpid)
        {

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("sp_Export_NsdlDematTransaction1");

            proc.AddVarcharPara("@userid",100, userid);
            proc.AddVarcharPara("@accid", 100, accid);
            proc.AddVarcharPara("@transdate", 100, transdate);
            proc.AddVarcharPara("@execdate", 100, execdate);
            proc.AddVarcharPara("@param_batch", 100, param_batch);
            proc.AddVarcharPara("@batch_num", 100, batch_num);
            proc.AddVarcharPara("@slip_MI", 100, slip_MI);
            proc.AddVarcharPara("@slip_Intra", -1, slip_Intra);
            proc.AddVarcharPara("@slip_Inter",100, slip_Inter);
            proc.AddVarcharPara("@slip_IS", 100, slip_IS);
            proc.AddVarcharPara("@create_datetime", 100, create_datetime);
            proc.AddVarcharPara("@version", 100, version);
            proc.AddVarcharPara("@dpid", 100, dpid); 
            ds = proc.GetDataSet();
            return ds;
        }



        public int SP_BrokerageChangeNSECM( string ALLBRKGDATA,string segment,string Companyid,string Client,string date,string MasterSegment,string CreateUser)
        {
            ProcedureExecute proc;
            int rtrnvalue = 0;
            try
            {
                using (proc = new ProcedureExecute("SP_BrokerageChangeNSECM"))
                {

                    proc.AddVarcharPara("@ALLBRKGDATA", 100, ALLBRKGDATA);
                    proc.AddIntegerPara("@segment", Convert.ToInt32(segment));
                    proc.AddVarcharPara("@Companyid", 100, Companyid);
                    proc.AddVarcharPara("@Client", 100, Client);
                    proc.AddVarcharPara("@date", 100, date);
                    proc.AddIntegerPara("@MasterSegment", Convert.ToInt32(MasterSegment));
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


        public DataSet Sp_SplitTrades(string segment, string Companyid, string date, string client, string product,
                                                      string MasterSegment,string SettNo,string SetType,string type,string custid)
        {

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_SplitTrades");

            proc.AddVarcharPara("@segment",100,segment);
            proc.AddVarcharPara("@Companyid", 100, Companyid);
            proc.AddVarcharPara("@date", 100,date);
            proc.AddVarcharPara("@client", 100, client);
            proc.AddVarcharPara("@product", 100, product);
            proc.AddIntegerPara("@MasterSegment", Convert.ToInt32(MasterSegment));
            proc.AddVarcharPara("@SettNo", 100, SettNo);
            proc.AddVarcharPara("@SetType", -1, SetType);
            proc.AddVarcharPara("@type", 100, type);
            proc.AddVarcharPara("@custid", 100, custid);
                                          
            ds = proc.GetDataSet();

            return ds;
        }

        public DataSet EARLYPAYIN_NSC(string settno, string clients, string Scrip, string branchid, string finyear,
                                                     string companyid, int segment)
        {

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("EARLYPAYIN");

            proc.AddVarcharPara("@settno", 100, settno);
            proc.AddVarcharPara("@clients", 100, clients);
            proc.AddVarcharPara("@Scrip", 100, Scrip);
            proc.AddVarcharPara("@branchid", 100, branchid);
            proc.AddVarcharPara("@finyear", 100, finyear);
            proc.AddVarcharPara("@companyid", 100, companyid);
            proc.AddIntegerPara("@segment",  segment); 

            ds = proc.GetDataSet();

            return ds;
        }

        public DataSet EARLYPAYIN_FILE( string EARLYPAYIN, string companyid, int segment)
        {

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("EARLYPAYIN");
            proc.AddVarcharPara("@EARLYPAYIN", 100, EARLYPAYIN);
            proc.AddVarcharPara("@settno", 100, companyid);
            proc.AddIntegerPara("@segment", segment); 
            ds = proc.GetDataSet(); 
            return ds;
        }

        public int sp_Insert_ExportFiles(string segid, string file_type, string file_name, string userid, string batch_number, string file_path)
        {
            ProcedureExecute proc;
            int rtrnvalue = 0;
            try
            {
                using (proc = new ProcedureExecute("SP_BrokerageChangeNSECM"))
                {

                    proc.AddVarcharPara("@segid", 100, segid); 
                    proc.AddVarcharPara("@file_type", 100, file_type);
                    proc.AddVarcharPara("@file_name", 100, file_name);
                    proc.AddVarcharPara("@userid", 100, userid); 
                    proc.AddVarcharPara("@batch_number", 100, batch_number);
                    proc.AddVarcharPara("@file_path", 100, file_path);


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

        public DataSet EARLYPAYIN_FILE(int SEGMENT, string companyid, string SETTNO, string EXPORTTYPE, string CNTRNO, string CLIENT, string FINYEAR, string DATE)
        {

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_STPFILES");
            proc.AddIntegerPara("@SEGMENT", SEGMENT);
            proc.AddVarcharPara("@settno", 100, companyid);
            proc.AddVarcharPara("@segment", 100, SETTNO);

            proc.AddVarcharPara("@EXPORTTYPE", 100, EXPORTTYPE);
            proc.AddVarcharPara("@CNTRNO", 100, CNTRNO);
            proc.AddVarcharPara("@CLIENT", 100, CLIENT);
            proc.AddVarcharPara("@FINYEAR", 100, FINYEAR);
            proc.AddVarcharPara("@DATE", 100, DATE);

            ds = proc.GetDataSet();
            return ds;
        }

        public int sp_Insert_ExportFilesStp(int segid, string file_type, string file_name, string userid, string batch_number, string file_path)
        {
            ProcedureExecute proc;
            int rtrnvalue = 0;
            try
            {
                using (proc = new ProcedureExecute("SP_BrokerageChangeNSECM"))
                {

                    proc.AddIntegerPara("@segid", segid);
                    proc.AddVarcharPara("@file_type", 100, file_type);
                    proc.AddVarcharPara("@file_name", 100, file_name);
                    proc.AddVarcharPara("@userid", 100, userid);
                    proc.AddVarcharPara("@batch_number", 100, batch_number);
                    proc.AddVarcharPara("@file_path", 100, file_path);
 
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

        public DataSet Sp_MKtRateChangeCM(int SEGMENT, string companyid, string SETTNO, string EXPORTTYPE, string CNTRNO, string CLIENT, string FINYEAR, string DATE)
        { 
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Sp_STPFILES");
            proc.AddIntegerPara("@SEGMENT", SEGMENT);
            proc.AddVarcharPara("@settno", 100, companyid);
            proc.AddVarcharPara("@segment", 100, SETTNO);

            proc.AddVarcharPara("@EXPORTTYPE", 100, EXPORTTYPE);
            proc.AddVarcharPara("@CNTRNO", 100, CNTRNO);
            proc.AddVarcharPara("@CLIENT", 100, CLIENT);
            proc.AddVarcharPara("@FINYEAR", 100, FINYEAR);
            proc.AddVarcharPara("@DATE", 100, DATE);

            ds = proc.GetDataSet();
            return ds;
        }
        //public DataSet Sp_MKtRateChangeCM(int SEGMENT, string companyid, string date, string SettNo, string SettType, string product, string TYPE, string ALLMKTRATEDATE)
        //{
        //    DataSet ds = new DataSet();
        //    ProcedureExecute proc = new ProcedureExecute("Sp_MKtRateChangeCM");
        //    proc.AddIntegerPara("@segment", SEGMENT);
        //    proc.AddVarcharPara("@companyid", 100, companyid);
        //    proc.AddVarcharPara("@date", 100, date);

        //    proc.AddVarcharPara("@SettNo", 100, SettNo);
        //    proc.AddVarcharPara("@SettType", 100, SettType);
        //    proc.AddVarcharPara("@product", 100, product);
        //    proc.AddVarcharPara("@TYPE", 100, TYPE);
        //    proc.AddVarcharPara("@ALLMKTRATEDATE", 100, ALLMKTRATEDATE);

        //    ds = proc.GetDataSet();
        //    return ds;
        //}

        //public DataSet Sp_MKtRateChangeCM(int p, string p_2, string p_3, string p_4, string p_5, string Scrips, string p_7, string p_8)
        //{
        //    DataSet ds = new DataSet();
        //    ProcedureExecute proc = new ProcedureExecute("Sp_MKtRateChangeCM");
        //    proc.AddIntegerPara("@segment", p);
        //    proc.AddVarcharPara("@companyid", 100, p_2);
        //    proc.AddVarcharPara("@date", 100, p_3);

        //    proc.AddVarcharPara("@SettNo", 100, p_4);
        //    proc.AddVarcharPara("@SettType", 100, p_5);
        //    proc.AddVarcharPara("@product", 100, Scrips);
        //    proc.AddVarcharPara("@TYPE", 100, p_7);
        //    proc.AddVarcharPara("@ALLMKTRATEDATE", 100, p_8);

        //    ds = proc.GetDataSet();
        //    return ds;
        //}

        public int Sp_MKtRateChangeCM_Change(string ALLMKTRATEDATE, int segment, string companyid, string date, string SettNo, string SettType, string product, string TYPE)
        {
            ProcedureExecute proc;
            int rtrnvalue = 0;
            try
            {
                using (proc = new ProcedureExecute("SP_BrokerageChangeNSECM"))
                {
                    proc.AddVarcharPara("@ALLMKTRATEDATE", 100, ALLMKTRATEDATE);
                    proc.AddIntegerPara("@segment", segment);
                    proc.AddVarcharPara("@companyid", 100, companyid);

                    proc.AddVarcharPara("@date", 100, date);
                    proc.AddVarcharPara("@SettNo", 100, SettNo);
                    proc.AddVarcharPara("@SettType", 100, SettType);
                    proc.AddVarcharPara("@product", 100, product);
                    proc.AddVarcharPara("@TYPE", 100, TYPE);
                    rtrnvalue = proc.RunActionQuery(); 

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
            return rtrnvalue;
        }


        public DataSet ContactNumberGenarationCHECKING(string Companyid, string segment, string settlementnom, string settlementtype, string DATE, 
            string CreateUser, string FINYEAR,
           string clients, string instrument, string MasterSegment, string REUSECHK, string Mode)
        {

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("ContactNumberGenarationCHECKING");
            proc.AddVarcharPara("@Companyid", 100, Companyid);
            proc.AddVarcharPara("@segment",100, segment);
            proc.AddVarcharPara("@settlementnom", 100, settlementnom);
            proc.AddVarcharPara("@settlementtype", -1, settlementtype);
            proc.AddVarcharPara("@DATE", 100, DATE);
            proc.AddVarcharPara("@CreateUser", 100, CreateUser);
            proc.AddVarcharPara("@FINYEAR", 100, FINYEAR);
            proc.AddVarcharPara("@clients", -1, clients);
            proc.AddVarcharPara("@instrument", -1, instrument);
            proc.AddVarcharPara("@MasterSegment", 100, MasterSegment);
            proc.AddVarcharPara("@REUSECHK", 100, REUSECHK);
           // proc.AddVarcharPara("@Mode", 100, Mode); 

            ds = proc.GetDataSet();
            return ds;
        }



        public DataSet CheckContractNoForProcessing(string trades_segment, string trades_settlementno, string tradedate, string companyid, string ClientsID,
            string Instrument, string SettType,
           string EXCHANGESEGMENT)
        {

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("CheckContractNoForProcessing");
            proc.AddVarcharPara("@trades_segment", 100, trades_segment);
            proc.AddVarcharPara("@trades_settlementno", 100, trades_settlementno);
            proc.AddVarcharPara("@tradedate", 100, tradedate);
            proc.AddVarcharPara("@companyid", 100, companyid);
            proc.AddVarcharPara("@ClientsID", 100, ClientsID);
            proc.AddVarcharPara("@Instrument", 100, Instrument);
            proc.AddVarcharPara("@SettType", 50, SettType);
            proc.AddVarcharPara("@EXCHANGESEGMENT", 50, EXCHANGESEGMENT);
           
            // proc.AddVarcharPara("@Mode", 100, Mode); 

            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet sp_Export_CdslDematTransaction_Ekool(string userid, string accid, string dpid, string clientid, string transdate,
            string execdate, string param_batch, string batch_num, string slip_num, string operatorid, string type, string create_datetime, string ExportType,
            string SegmentID)
        {

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("sp_Export_CdslDematTransaction_Ekool");
            proc.AddVarcharPara("@userid", 100, userid);
            proc.AddVarcharPara("@accid", 100, accid);
            proc.AddVarcharPara("@dpid", 100, dpid);
            proc.AddVarcharPara("@clientid", 100, clientid);
            proc.AddVarcharPara("@transdate", 100, transdate);
            proc.AddVarcharPara("@execdate", 100, execdate);
            proc.AddVarcharPara("@param_batch", -1, param_batch);
            proc.AddVarcharPara("@batch_num", 50, batch_num); 
            proc.AddVarcharPara("@slip_num", 100, slip_num);
            proc.AddVarcharPara("@operatorid", 100, operatorid);
            proc.AddVarcharPara("@type", 100, type);
            proc.AddVarcharPara("@create_datetime", 100, create_datetime);
            proc.AddVarcharPara("@ExportType",100, ExportType);
            proc.AddVarcharPara("@SegmentID", 100, SegmentID); 
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet sp_Export_POATransaction1(string userid, string accid, string dpid, string sourceDP, string transdate,
            string execdate, string param_batch, string batch_num, string slip_num, string operatorid, string create_datetime,
            string version, string FileType)
        {

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("sp_Export_POATransaction1");
            proc.AddVarcharPara("@userid", 100, userid);
            proc.AddVarcharPara("@accid", 100, accid);
            proc.AddVarcharPara("@dpid", 100, dpid);
            proc.AddVarcharPara("@sourceDP", 100, sourceDP);
            proc.AddVarcharPara("@transdate", 100, transdate);
            proc.AddVarcharPara("@execdate", 100, execdate);
            proc.AddVarcharPara("@param_batch", -1, param_batch);
            proc.AddVarcharPara("@batch_num", 50, batch_num);
            proc.AddVarcharPara("@slip_num", 100, slip_num);
            proc.AddVarcharPara("@operatorid", 100, operatorid);
            proc.AddVarcharPara("@create_datetime", 100, create_datetime);
            proc.AddVarcharPara("@version", 100, version);
            proc.AddVarcharPara("@FileType", 100, FileType);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet Export_SpeedE_ClearingMember_Transaction(string userid, string dpid, string accid, string cmbpid, string transdate,
            string execdate, string param_batch, string batch_num, string slip_MI, string slip_Intra, string slip_Inter,
            string slip_IS, string create_datetime, string version)
        {

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Export_SpeedE_ClearingMember_Transaction");
            proc.AddVarcharPara("@userid", 100, userid);
            proc.AddVarcharPara("@dpid", 100, dpid);
            proc.AddVarcharPara("@accid", 100, accid);
            proc.AddVarcharPara("@cmbpid", 100, cmbpid);
            proc.AddVarcharPara("@transdate", 100, transdate);
            proc.AddVarcharPara("@execdate", 100, execdate);
            proc.AddVarcharPara("@param_batch", -1, param_batch);
            proc.AddVarcharPara("@batch_num", 50, batch_num);
            proc.AddVarcharPara("@slip_MI", 100, slip_MI);
            proc.AddVarcharPara("@slip_Intra", 100, slip_Intra);
            proc.AddVarcharPara("@slip_Inter", 100, slip_Inter);
            proc.AddVarcharPara("@slip_IS", 100, slip_IS);
            proc.AddVarcharPara("@create_datetime", 100, create_datetime);
            proc.AddVarcharPara("@version", 100, version);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet Export_SpeedE_Transaction1(string userid, string dpid, string accid, string cmbpid, string transdate,
            string execdate, string param_batch, string batch_num, string slip_MI, string slip_Intra, string slip_Inter,
            string slip_IS, string create_datetime, string version)
        {

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Export_SpeedE_Transaction1");
            proc.AddVarcharPara("@userid", 100, userid);
            proc.AddVarcharPara("@dpid", 100, dpid);
            proc.AddVarcharPara("@accid", 100, accid);
            proc.AddVarcharPara("@cmbpid", 100, cmbpid);
            proc.AddVarcharPara("@transdate", 100, transdate);
            proc.AddVarcharPara("@execdate", 100, execdate);
            proc.AddVarcharPara("@param_batch", -1, param_batch);
            proc.AddVarcharPara("@batch_num", 50, batch_num);
            proc.AddVarcharPara("@slip_MI", 100, slip_MI);
            proc.AddVarcharPara("@slip_Intra", 100, slip_Intra);
            proc.AddVarcharPara("@slip_Inter", 100, slip_Inter);
            proc.AddVarcharPara("@slip_IS", 100, slip_IS);
            proc.AddVarcharPara("@create_datetime", 100, create_datetime);
            proc.AddVarcharPara("@version", 100, version);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet Insert_ExportFiles_Demat(string userid, string segid, string file_type, string file_name, string batch_number,
           string DpID, string file_path)
        {

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Insert_ExportFiles_Demat");
            proc.AddVarcharPara("@userid", 100, userid);
            proc.AddVarcharPara("@segid", 100, segid);
            proc.AddVarcharPara("@file_type", 100, file_type);
            proc.AddVarcharPara("@file_name", 100, file_name);
            proc.AddVarcharPara("@batch_number", 100, batch_number);
            proc.AddVarcharPara("@DpID", 100, DpID);
            proc.AddVarcharPara("@file_path", 200, file_path);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet sp_Show_DematBatchSummary(string accid, string dpid, string transdate, string type)
        {

            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("sp_Show_DematBatchSummary");
            proc.AddVarcharPara("@accid", 100, accid);
            proc.AddVarcharPara("@dpid", 100, dpid);
            proc.AddVarcharPara("@transdate", 100, transdate);
            proc.AddVarcharPara("@type", 100, type);
            ds = proc.GetDataSet();
            return ds;
        }
   }
}
