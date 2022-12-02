using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class Client_DP_Account
    {
        public DataSet POAFetch(string Dp)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("POAFetch");

            proc.AddVarcharPara("@Dp", 20, Dp);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet POAFetchDetail(string PoaName, string Dp)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("POAFetchDetail");
            proc.AddVarcharPara("@PoaName", 20, PoaName);
            proc.AddVarcharPara("@Dp", 20, Dp);
            ds = proc.GetDataSet();
            return ds;
        }


        public DataTable DPSlipsStockIssue_Insert(string DpId, string SlipType, string Account, string Poa, string IsLoosed, string DisFormat,
            string IssueDate,string SlipNoPrefix, string BookNoFrom,
           string SlipNoFrom, string SlipNoTo, string BookNoTo, string TotalNoOfBooks, string Remarks, string user)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("insertMaster_DPSlipsStockIssue");

            proc.AddVarcharPara("@DpId", 20, DpId);
            proc.AddVarcharPara("@SlipType", 3, SlipType);
            proc.AddVarcharPara("@Account", 20, Account);
            proc.AddVarcharPara("@Poa", 2, Poa);
            proc.AddCharPara("@IsLoosed", 1,Convert.ToChar(IsLoosed));
            proc.AddCharPara("@DisFormat", 1, Convert.ToChar(DisFormat));
            proc.AddVarcharPara("@IssueDate", 20, IssueDate);
            proc.AddVarcharPara("@SlipNoPrefix", 4, SlipNoPrefix);
            proc.AddVarcharPara("@BookNoFrom", 20, BookNoFrom);
            proc.AddVarcharPara("@SlipNoFrom", 20, SlipNoFrom);
            proc.AddVarcharPara("@SlipNoTo", 20, SlipNoTo);
            proc.AddVarcharPara("@BookNoTo", 20, BookNoTo);
            proc.AddVarcharPara("@TotalNoOfBooks", 30, TotalNoOfBooks);
            proc.AddVarcharPara("@Remarks", 30, Remarks);
            proc.AddVarcharPara("@user", 10, user);
            ds = proc.GetDataSet();
            return ds.Tables[0];
        }


        public DataTable Fetch_doublecapture_EnteredSlipDetailByID(string dp, string dpId, string slipType, string AccountID, string usertype)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("sp_Fetch_doublecapture_EnteredSlipDetailByID");

            proc.AddVarcharPara("@dp", 100, dp);
            proc.AddVarcharPara("@dpId", 100, dpId);
            proc.AddVarcharPara("@slipType", 100, slipType);
            proc.AddVarcharPara("@AccountID", 100, AccountID);
            proc.AddVarcharPara("@usertype", 100, usertype);
            ds = proc.GetDataSet();
            return ds.Tables[0];
        }


        public DataSet cdslClientNomineeDetails(string id)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("cdslClientNomineeDetails");

            proc.AddVarcharPara("@id", 20, id);
            ds = proc.GetDataSet();
            return ds;
        }
    }
}
