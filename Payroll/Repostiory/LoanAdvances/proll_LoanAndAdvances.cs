using DataAccessLayer;
using Payroll.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Payroll.Repostiory.LoanAdvances
{
    public class proll_LoanAndAdvances:IloanAndAdvances
    {
        public string SaveLoanAndAdveances(LoanAndAdvances model,string Action)
        {
            string output = "";

            try
            {
                DataSet ds = new DataSet();
                ProcedureExecute proc = new ProcedureExecute("PRC_PROLL_LOANADVANCES");
                proc.AddVarcharPara("@Action", 100, Action);

                proc.AddVarcharPara("@EditLoanId", 100, model.LoanId);

                proc.AddVarcharPara("@Amount", 100, model.Amount);
                proc.AddVarcharPara("@Code", 100, model.Code);
                proc.AddVarcharPara("@Deduction_Start", 100, model.Deduction_Start);
                proc.AddVarcharPara("@Deduction_Starts_Period", 100, model.Period);
                proc.AddVarcharPara("@Disb_Date", 100, model.Disb_Date);
                proc.AddVarcharPara("@EmpLOYEECode", 100, model.Emp_Code);
                if (model.Freeze_Upto != "dd-MM-yyyy" && model.Freeze_Upto != "" && model.Freeze_Upto != "01-01-0100")
                proc.AddVarcharPara("@Freeze_Upto", 100, model.Freeze_Upto);
                proc.AddVarcharPara("@ins_Amount", 100, model.ins_Amount);
                proc.AddVarcharPara("@Installment", 100, model.Installment);
                proc.AddVarcharPara("@IsFreeze", 100, model.IsFreeze);
                proc.AddVarcharPara("@Max_Amount", 100, model.Max_Amount);
                proc.AddVarcharPara("@Max_Based_On", 100, model.Max_Based_On);
                proc.AddVarcharPara("@Max_Check", 100, model.Max_Check);
                proc.AddVarcharPara("@Min_Amount", 100, model.Min_Amount);
                proc.AddVarcharPara("@Min_Check", 100, model.Min_Check);
                proc.AddVarcharPara("@Name", 100, model.Name);
               // proc.AddVarcharPara("@Period", 100, model.Period);
                proc.AddVarcharPara("@TYPE", 100, model.TYPE);
                proc.AddVarcharPara("@returnvalue", 100, output,QueryParameterDirection.Output);


                proc.AddVarcharPara("@USER_ID", 100, Convert.ToString(HttpContext.Current.Session["userid"]));

                ds = proc.GetDataSet();

                output = proc.GetParaValue("@returnvalue").ToString();
                
            }
            catch (Exception ex)
            {
                return null;
            }

            return output;
        }

        public DataTable GetLoanAndAdveances(string LoanId,string Action)
        {
            string output = "";
            DataTable ds = new DataTable();
            try
            {
                
                ProcedureExecute proc = new ProcedureExecute("PRC_PROLL_LOANADVANCES");
                proc.AddVarcharPara("@Action", 100, Action);
                proc.AddVarcharPara("@EditLoanId", 100, LoanId);
                ds = proc.GetTable();

            }
            catch (Exception ex)
            {
                return null;
            }

            return ds;
        }   

    }
}