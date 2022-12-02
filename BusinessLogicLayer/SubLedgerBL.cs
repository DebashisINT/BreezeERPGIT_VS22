using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace BusinessLogicLayer
{
   public class SubLedgerBL
    {

       public DataTable PopulateGridForAssetDetail(string MainAccountCode, string SubAccountCode)
       {
           ProcedureExecute proc = new ProcedureExecute("prc_Subledger");
           proc.AddVarcharPara("@Action", 100, "PopulateDropDownFortdstcs"); 
           return proc.GetTable();
       }
    }
}
