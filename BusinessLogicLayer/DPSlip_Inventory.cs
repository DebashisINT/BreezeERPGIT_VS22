using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class DPSlip_Inventory
    {
        public DataTable DPSlipsStock_Insert(string DpId, string SlipType, string SlipNoPrefix, string SlipsPerBook, string BookNoFrom,
            string SlipNoFrom, string SlipNoTo, string BookNoTo, string TotalNoOfBooks, string Remarks, string user, string stockEntryDate)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("insertMaster_DPSlipsStock");

            proc.AddVarcharPara("@DpId", 100, DpId);
            proc.AddVarcharPara("@SlipType", 100, SlipType);
            proc.AddVarcharPara("@SlipNoPrefix", 100, SlipNoPrefix);
            proc.AddVarcharPara("@SlipsPerBook", 100, SlipsPerBook);
            proc.AddVarcharPara("@BookNoFrom", 100, BookNoFrom);
            proc.AddVarcharPara("@SlipNoFrom", 100, SlipNoFrom);
            proc.AddVarcharPara("@SlipNoTo", 100, SlipNoTo);
            proc.AddVarcharPara("@BookNoTo", 100, BookNoTo);
            proc.AddVarcharPara("@TotalNoOfBooks", 100, TotalNoOfBooks);
            proc.AddVarcharPara("@Remarks", 100, Remarks);
            proc.AddVarcharPara("@user", 100, user);
            proc.AddVarcharPara("@stockEntryDate", 100, stockEntryDate);
            ds = proc.GetDataSet();
            return ds.Tables[0];
        }
    }
}
