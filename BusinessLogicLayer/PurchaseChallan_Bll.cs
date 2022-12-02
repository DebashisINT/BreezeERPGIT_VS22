using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class PurchaseChallan_Bll
    {
        public DataSet GetNewAllDropDownDetail(string branchHierchy)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("proc_GetPurchaseChallan_Details");
            proc.AddVarcharPara("@Action", 100, "GetAllDropDownDetails");
            proc.AddVarcharPara("@userbranchlist", -1, branchHierchy);
            ds = proc.GetDataSet();
            return ds;
        }
    }
}
