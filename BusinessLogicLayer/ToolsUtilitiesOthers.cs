using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;
using System.Data;

namespace BusinessLogicLayer
{
   public class ToolsUtilitiesOthers
   {
       public DataSet Fetch_MessageTemplateReservedWord(string SenderID, string RecipientID)
       {
           DataSet ds = new DataSet();
           ProcedureExecute proc = new ProcedureExecute("Fetch_MessageTemplateReservedWord");
           proc.AddVarcharPara("@SenderID", 50, SenderID);
           proc.AddVarcharPara("@RecipientID", 50, RecipientID);
           ds = proc.GetDataSet();
           return ds;
       }
    }
}
