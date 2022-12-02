using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.YearEnding
{
  public class AccountsBL
    {
      public string CashBankCutOff(string CashBankIds, string CutoffDb, DateTime CutOffDate)
      {
          DataTable CutoffValue = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("Prc_YearEnding_CashBank");
          proc.AddVarcharPara("@Action", 100, "CurrentYear");
          proc.AddVarcharPara("@CashBankIds", -1, CashBankIds);
          proc.AddVarcharPara("@CutoffdbName", 100,CutoffDb);
          proc.AddDateTimePara("@CutOffdate", CutOffDate);

          CutoffValue = proc.GetTable();
          
          return "";
      }
      public string JournalCutOff(string JournalVoucherID, string CutoffdbName, DateTime CutOffdate)
      {
          DataTable CutoffValue = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("Prc_YearEnding_Journal");
          proc.AddVarcharPara("@Action", 100, "CurrentYear");
          proc.AddVarcharPara("@JournalVoucherID", -1, JournalVoucherID);
          proc.AddVarcharPara("@CutoffdbName", 100, CutoffdbName);
          proc.AddDateTimePara("@CutOffdate", CutOffdate);

          CutoffValue = proc.GetTable();

          return "";
      }
      
    }
}
