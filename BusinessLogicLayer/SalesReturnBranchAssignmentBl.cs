using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace BusinessLogicLayer
{
   public class SalesReturnBranchAssignmentBl
    {
       public DataTable GetSalesReturnForBranchAssignment(string companyId,string finYear,String barnch)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_salesReturnAssignBranch");
           proc.AddVarcharPara("@Action", 20, "GetList");
           proc.AddVarcharPara("@companyId", 10, companyId);
           proc.AddVarcharPara("@finYear", 10, finYear);
           proc.AddIntegerPara("@branchId", Convert.ToInt32(barnch));
           dt = proc.GetTable();
           return dt;
       
       }
       public DataTable getBranchListByBranchList(string userbranchhierchy, string userBranch)
       {
           DataTable ds = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_PosSalesInvoice");
           proc.AddVarcharPara("@Action", 100, "getBranchList");
           proc.AddVarcharPara("@BranchList", 1000, userbranchhierchy);
           proc.AddIntegerPara("@branch", Convert.ToInt32(userBranch));
           ds = proc.GetTable();
           return ds;
       }

       public DataTable GetAssignBranchdata(int id)
       {
           DataTable ds = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_salesReturnAssignBranch");
           proc.AddVarcharPara("@Action", 100, "GetExistingData"); 
           proc.AddIntegerPara("@id", id);
           ds = proc.GetTable();
           return ds;
       }

       public void UpdateAssignBranch(int BranchId, string Remarks,int invoiceId)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_salesReturnAssignBranch");
            proc.AddVarcharPara("@Action", 100, "UpdateBranch");
            proc.AddIntegerPara("@branchId", BranchId);
            proc.AddVarcharPara("@Remarks",1000, Remarks);
            proc.AddIntegerPara("@id", invoiceId);
            proc.RunActionQuery();
        }
    }
}
