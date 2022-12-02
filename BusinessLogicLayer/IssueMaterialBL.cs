using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BusinessLogicLayer
{
   public class IssueMaterialBL
    {
       public int UpdateEWayBillForChallanBL(string IssueID, string EWayBillNumber, string EWayBillDate, string EWayBillValue)
       {
           int i;
           int rtrnvalue = 0;
           ProcedureExecute proc = new ProcedureExecute("prc_PMSIssueMaterial_Details");
           proc.AddVarcharPara("@Action", 100, "UpdateEWayBill");
           proc.AddBigIntegerPara("@IssueId", Convert.ToInt32(IssueID));
           proc.AddVarcharPara("@EWayBillNumber", 50, EWayBillNumber);
           proc.AddVarcharPara("@EWayBillDate", 50, EWayBillDate);
           proc.AddVarcharPara("@EWayBillValue", 50, EWayBillValue);
           proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
           i = proc.RunActionQuery();
           rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
           return rtrnvalue;

       }

       public int DeleteMaterialIssue(string SalesChallanid)
       {
           int i;
           int rtrnvalue = 0;
           ProcedureExecute proc = new ProcedureExecute("prc_PMSCustomerIssueMaterial");
           proc.AddVarcharPara("@Action", 100, "MaterialIssueDelete");
           proc.AddVarcharPara("@Issue_Id", 50, SalesChallanid);
           proc.AddVarcharPara("@FinYear", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
           proc.AddVarcharPara("@CompanyID", 500, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
           proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
           i = proc.RunActionQuery();
           rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
           return rtrnvalue;
       }

       public DataTable GetMaterialIssueListGridDataByDate(string Branch, string company, DateTime StartDate, DateTime EndDate, string BranchId)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_PMSIssueMaterial_Details");
           proc.AddVarcharPara("@Action", 500, "IssueMaterialFilteredByDate");
           proc.AddVarcharPara("@userbranchlist", 500, Branch);
           proc.AddVarcharPara("@lastCompany", 500, company);
           proc.AddDateTimePara("@FromDate", StartDate);
           proc.AddDateTimePara("@ToDate", EndDate);
           proc.AddIntegerPara("@branchId", Convert.ToInt32(BranchId));
           dt = proc.GetTable();
           return dt;
       }

       public DataTable GetMaterialIssueStatusByOrderID(string SalesOrder_Id)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_SalesOrder_Details");
           proc.AddNVarcharPara("@action", 150, "GetSalesOrderStatusByOrderID");
           proc.AddIntegerPara("@SalesOrder_Id", Convert.ToInt32(SalesOrder_Id));
           dt = proc.GetTable();
           return dt;
       }

      
    }
}
