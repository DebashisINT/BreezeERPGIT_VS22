using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;
using System.Web;

namespace BusinessLogicLayer
{
    public class Lead_general_BL                                                              
    {
        public int btnSave_Click(string emp_cntId, string emp_dateofJoining,
        string emp_Organization, string emp_JobResponsibility, string emp_Designation, string emp_type,
        string emp_Department, string emp_reportTo, string emp_deputy, string emp_colleague,
        string emp_workinghours, string emp_totalLeavePA, string emp_LeaveSchemeAppliedFrom, string emp_branch, string emp_Remarks)
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("EmployeeCTCInsert"))
                {
                    proc.AddNVarcharPara("@emp_cntId", 100, emp_cntId);
                    proc.AddNVarcharPara("@emp_dateofJoining", 100, emp_dateofJoining);
                    proc.AddIntegerPara("@emp_Organization", Convert.ToInt32(emp_Organization));
                    proc.AddIntegerPara("@emp_JobResponsibility", Convert.ToInt32(emp_JobResponsibility));
                    proc.AddIntegerPara("@emp_Designation", Convert.ToInt32(emp_Designation));
                    proc.AddIntegerPara("@emp_type", Convert.ToInt32(emp_type));

                    proc.AddIntegerPara("@emp_Department", Convert.ToInt32(emp_Department));
                    proc.AddIntegerPara("@emp_reportTo", Convert.ToInt32(emp_reportTo));
                    proc.AddIntegerPara("@emp_deputy", Convert.ToInt32(emp_deputy));
                    proc.AddIntegerPara("@emp_colleague", Convert.ToInt32(emp_colleague));
                    proc.AddIntegerPara("@emp_workinghours", Convert.ToInt32(emp_workinghours));
                    proc.AddDecimalPara("@emp_currentCTC", 0, 18, 0);
                    proc.AddDecimalPara("@emp_basic", 0, 18, 0);
                    proc.AddDecimalPara("@emp_HRA", 0, 18, 0);
                    proc.AddDecimalPara("@emp_CCA", 0, 18, 0);
                    proc.AddDecimalPara("@emp_spAllowance", 0, 18, 0);
                    proc.AddDecimalPara("@emp_childrenAllowance", 0, 18, 0);
                    proc.AddNVarcharPara("@emp_totalLeavePA", 100, emp_totalLeavePA);

                    proc.AddDecimalPara("@emp_PF", 0, 18, 0);
                    proc.AddDecimalPara("@emp_medicalAllowance", 0, 18, 0);
                    proc.AddDecimalPara("@emp_LTA", 0, 18, 0);
                    proc.AddDecimalPara("@emp_convence", 0, 18, 0);
                    proc.AddDecimalPara("@emp_mobilePhoneExp", 0, 18, 0);
                    proc.AddNVarcharPara("@emp_totalLeavePA", 100, emp_totalLeavePA);
                    proc.AddNVarcharPara("@emp_totalMedicalLeavePA", 100, "");

                    proc.AddIntegerPara("@userid", Convert.ToInt32(HttpContext.Current.Session["userid"]));
                    proc.AddNVarcharPara("@emp_LeaveSchemeAppliedFrom", 100, emp_LeaveSchemeAppliedFrom);
                    proc.AddIntegerPara("@emp_branch", Convert.ToInt32(emp_branch));
                    proc.AddNVarcharPara("@emp_Remarks", 100, emp_Remarks);

                    proc.AddDecimalPara("@EMP_CarAllowance", 0, 18, 0);
                    proc.AddDecimalPara("@EMP_UniformAllowance", 0, 18, 0);
                    proc.AddDecimalPara("@EMP_BooksPeriodicals", 0, 18, 0);
                    proc.AddDecimalPara("@EMP_SeminarAllowance", 0, 18, 0);
                    proc.AddDecimalPara("@EMP_OtherAllowance", 0, 18, 0);
 
                    int NoOfRowEffected = proc.RunActionQuery();
                    return NoOfRowEffected;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                proc = null;
            }
        }
    }
}
