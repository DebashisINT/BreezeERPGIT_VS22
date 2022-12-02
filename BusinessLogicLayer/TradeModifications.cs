using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class TradeModifications
    {
        //public DataSet Reports_ComTrade_Register(string fromdate,)
        //{
        //    DataSet ds = new DataSet();
        //    ProcedureExecute proc = new ProcedureExecute("Reports_ComTrade_Register");

        //    proc.AddVarcharPara("@fromdate", 30, fromdate);
        //    proc.AddVarcharPara("@todate", 50, todate);
        //    proc.AddVarcharPara("@tradetype", 50, tradetype);
        //    ds = proc.GetDataSet();
        //    return ds;
        //}

        //public int ICEXCOMM_TRADE_PROCESS(string trades_segment,string trades_settlementno,string tradedate,string CreateUser,string companyid,string ClientsID,
        //    string Instrument)
        //{
        //    ProcedureExecute proc;
        //    string rtrnvalue = "";
        //    try
        //    {
        //        using (proc = new ProcedureExecute("sp_GenerateContactUCC"))
        //        {
        //            proc.AddVarcharPara("@trades_segment", 50, trades_segment);
        //            proc.AddVarcharPara("@trades_settlementno", 50,trades_settlementno);
        //            proc.AddVarcharPara("@tradedate", 50,tradedate);
        //            proc.AddVarcharPara("@CreateUser", 50,CreateUser);
        //            proc.AddVarcharPara("@companyid", 50,companyid);
        //            proc.AddVarcharPara("@ClientsID", 50,ClientsID);
        //            proc.AddVarcharPara("@Instrument", 50,Instrument); 

        //            int i = proc.RunActionQuery(); 
        //            return rtrnvalue;
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        proc = null;
        //    }
        //}
    }
}
