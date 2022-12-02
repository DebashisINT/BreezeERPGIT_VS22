using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;
using System.Web;
using System.Data;

namespace BusinessLogicLayer
{
    public class Employee_AddNew_BL
    {

        public int btnCTC_Click_BL(string emp_cntId, string emp_dateofJoining, 
        string emp_Organization, string emp_JobResponsibility, string emp_Designation, string emp_type,
        string emp_Department, string emp_reportTo, string emp_deputy, string emp_colleague,
        string emp_workinghours, string emp_totalLeavePA, string emp_LeaveSchemeAppliedFrom, string emp_branch, string emp_Remarks )
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
                    proc.AddDecimalPara("@emp_currentCTC",0,18, 0);
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

        public string btnSave_Click_BL(string cnt_ucc, string cnt_salutation, string cnt_firstName,
       string cnt_middleName, string cnt_lastName, string cnt_shortName, string cnt_branchId,
       string cnt_sex, string cnt_maritalStatus, string DOBDate, string AnniversaryDate,
       string cmbLegalStatus, string cmbEducation, string cmbSource, string referedID, string cmbBloodgroup,
       bool chkAllow, string password)
        {
            ProcedureExecute proc;

            try
            {
                using (proc = new ProcedureExecute("EmployeeInsert"))
                {
                    proc.AddVarcharPara("@cnt_ucc", 100, cnt_ucc);

                    proc.AddVarcharPara("@cnt_salutation", 100, cnt_salutation);

                    proc.AddVarcharPara("@cnt_firstName", 100, cnt_firstName);

                    proc.AddVarcharPara("@cnt_middleName", 100, cnt_middleName);

                    proc.AddVarcharPara("@cnt_lastName", 100, cnt_lastName);

                    proc.AddVarcharPara("@cnt_shortName", 100, cnt_shortName);

                    proc.AddVarcharPara("@cnt_branchId", 100, cnt_branchId);

                    proc.AddVarcharPara("@cnt_sex", 100, cnt_sex);

                    proc.AddVarcharPara("@cnt_maritalStatus", 100, cnt_maritalStatus);


                    if (DOBDate != null)
                    {
                        proc.AddVarcharPara("@cnt_DOB", 100, DOBDate);
                    }
                    else
                    {
                        proc.AddVarcharPara("@cnt_DOB", 100, "");
                    }
                    if (AnniversaryDate != null)
                    {
                        proc.AddVarcharPara("@cnt_anniversaryDate", 100, AnniversaryDate);
                    }
                    else
                    {
                        proc.AddVarcharPara("@cnt_anniversaryDate", 100, "");
                    }
                    proc.AddVarcharPara("@cnt_legalStatus", 100, cmbLegalStatus);
                    proc.AddVarcharPara("@cnt_education", 100, cmbEducation);
                    proc.AddVarcharPara("@cnt_contactSource", 100, cmbSource);
                    proc.AddVarcharPara("@cnt_referedBy", 100, referedID);
                    proc.AddVarcharPara("@cnt_contactType", 100, "EM");
                    proc.AddVarcharPara("@lastModifyUser", 100, Convert.ToString(HttpContext.Current.Session["userid"]));
                    proc.AddVarcharPara("@UserContactID", 100, Convert.ToString(HttpContext.Current.Session["usercontactID"]));
                    proc.AddVarcharPara("@bloodgroup", 100, cmbBloodgroup);
                    string webLogin = "No";
                    string Password = string.Empty;
                    if (chkAllow == true)
                    {
                        webLogin = "Yes";
                        Password = password;
                    }
                    proc.AddVarcharPara("@webLogin", 100, webLogin);
                    proc.AddVarcharPara("@Password", 100, Password);
                    proc.AddVarcharPara("@result", 100, "", QueryParameterDirection.Output);
                    int NoOfRowEffected = proc.RunActionQuery();
                    string RetVal = Convert.ToString(proc.GetParaValue("@RetVal"));
                    return RetVal;
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

        public DataTable GetContactTypeonEmployeeType(string strEmpType)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GetContactType");
            proc.AddVarcharPara("@EmployeeType", 100, strEmpType);
            ds = proc.GetTable();
            return ds;
        }

        public int InsertContactType_BL(string strInternalID, string strContactID, string strEmpType)
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("prc_InsertUpdateContactTypeOnEmpType"))
                {
                    proc.AddNVarcharPara("@InternalID", 20, strInternalID);
                    proc.AddNVarcharPara("@ContactID", 20, strContactID);
                    proc.AddNVarcharPara("@ContactTypePrefix", 5, strEmpType);
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
