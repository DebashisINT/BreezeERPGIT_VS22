using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class EmployeeStatutoryBal
    {
        public DataTable PopulateStatutoryDtlsById(ref string retmsg, string EmployeeCode)
        {
            DataTable dt = new DataTable();
            string output = string.Empty;
            try
            {
                ProcedureExecute proc = new ProcedureExecute("prc_employee_statutory_dtls");
                proc.AddVarcharPara("@Action", 50, "SelectStatutoryDtls");
                proc.AddVarcharPara("@EmployeeCode",50 ,EmployeeCode);
                proc.AddVarcharPara("@ReturnMessage", 500, "", QueryParameterDirection.Output);
                proc.AddVarcharPara("@ReturnCode",20, "", QueryParameterDirection.Output);
                dt = proc.GetTable();
                output = Convert.ToString(proc.GetParaValue("@ReturnMessage"));

            }
            catch (Exception ex)
            {
                throw;
            }
            retmsg = output;
            return dt;

        }

        public DataTable PfESIDtlsById(ref string retmsg, string EmployeeCode)
        {
            DataTable dt = new DataTable();
            string output = string.Empty;
            try
            {
                ProcedureExecute proc = new ProcedureExecute("prc_employee_statutory_dtls");
                proc.AddVarcharPara("@Action",50, "SelectStatutoryDtls");
                proc.AddVarcharPara("@EmployeeCode", 50, EmployeeCode);
                proc.AddVarcharPara("@ReturnMessage", 500, "", QueryParameterDirection.Output);
                proc.AddVarcharPara("@ReturnCode", 20, "", QueryParameterDirection.Output);
                dt = proc.GetTable();
                output = Convert.ToString(proc.GetParaValue("@ReturnMessage"));

            }
            catch (Exception ex)
            {
                throw;
            }
            retmsg = output;
            return dt;

        }
        public DataTable EmployeeOtherDetailsById(ref string retmsg, string EmployeeCode)
        {
            DataTable dt = new DataTable();
            string output = string.Empty;
            try
            {
                ProcedureExecute proc = new ProcedureExecute("prc_employee_statutory_dtls");
                proc.AddVarcharPara("@Action",50, "SelectEmployeeOtherDetails");
                proc.AddVarcharPara("@EmployeeCode", 50, EmployeeCode);
                proc.AddVarcharPara("@ReturnMessage", 500, "", QueryParameterDirection.Output);
                proc.AddVarcharPara("@ReturnCode", 20, "", QueryParameterDirection.Output);
                dt = proc.GetTable();
                output = Convert.ToString(proc.GetParaValue("@ReturnMessage"));

            }
            catch (Exception ex)
            {
                throw;
            }
            retmsg = output;
            return dt;

        }
        public string Save(string EmployeeCode, string Pan, string Passport, DateTime? ValidUpTo, string EpicNo, string AadharNo, ref string ReturnCode, string CreateUser, string NameAsperPan = null, string DeducteeStatus=null)
        {
            ProcedureExecute proc;
            string output = string.Empty;
            int NoOfRowEffected = 0;
            try
            {

                using (proc = new ProcedureExecute("prc_employee_statutory_dtls"))
                {

                    proc.AddVarcharPara("@Action", 50, "AddStatutoryDtls");
                    proc.AddVarcharPara("@EmployeeCode", 100, EmployeeCode);
                    proc.AddVarcharPara("@PanNumber", 100, Pan);
                    proc.AddVarcharPara("@PassportNumber", 100, Passport);
                    proc.AddPara("@ValidUpTo", ValidUpTo);
                    proc.AddVarcharPara("@EpicNumber", 50, EpicNo);
                    proc.AddVarcharPara("@AadhaarNumber", 100, AadharNo);
                    proc.AddIntegerPara("@CreatedBy", Convert.ToInt32(CreateUser));

                    proc.AddVarcharPara("@NameAsPerPAN", 400, NameAsperPan);
                    proc.AddVarcharPara("@DeducteeStatus", 50, DeducteeStatus);

                    proc.AddVarcharPara("@ReturnMessage", 500, "", QueryParameterDirection.Output);
                    proc.AddVarcharPara("@ReturnCode", 20, "", QueryParameterDirection.Output);
                    NoOfRowEffected = proc.RunActionQuery();

                    output = Convert.ToString(proc.GetParaValue("@ReturnMessage"));
                    ReturnCode = Convert.ToString(proc.GetParaValue("@ReturnCode"));


                }
            }

            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }

            finally
            {
                proc = null;
            }
            return output;
        }

        public string SavePFESI(string EmployeeCode, int PFApplcbl, string PfNo,string Uan, int EsiApplcbl, string EsiNO, ref string ReturnCode, string CreateUser)
        {
            ProcedureExecute proc;
            string output = string.Empty;
            int NoOfRowEffected = 0;
            try
            {

                using (proc = new ProcedureExecute("prc_employee_statutory_dtls"))
                {

                    proc.AddVarcharPara("@Action", 50, "ADDPfEsiDtls");
                    proc.AddVarcharPara("@EmployeeCode", 100, EmployeeCode);
                    proc.AddIntegerPara("@PfApplicable", PFApplcbl);
                    proc.AddVarcharPara("@PfNumber", 100, PfNo);
                    proc.AddVarcharPara("@UANNumber",100, Uan);
                    proc.AddIntegerPara("@EsiApplicable", EsiApplcbl);
                    proc.AddVarcharPara("@EsicNumber", 100, EsiNO);
                    proc.AddIntegerPara("@CreatedBy", Convert.ToInt32(CreateUser));
                    proc.AddVarcharPara("@ReturnMessage", 500, "", QueryParameterDirection.Output);
                    proc.AddVarcharPara("@ReturnCode", 20, "", QueryParameterDirection.Output);
                    NoOfRowEffected = proc.RunActionQuery();

                    output = Convert.ToString(proc.GetParaValue("@ReturnMessage"));
                    ReturnCode = Convert.ToString(proc.GetParaValue("@ReturnCode"));


                }
            }

            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }

            finally
            {
                proc = null;
            }
            return output;
        }

        public string SaveEmpOtherDtls(string EmployeeCode, DateTime? JoiningDate, DateTime?LeavingDate, string unit, string dept, string desig, string grade, ref string ReturnCode, string CreateUser)
        {
            ProcedureExecute proc;
            string output = string.Empty;
            int NoOfRowEffected = 0;
            try
            {

                using (proc = new ProcedureExecute("prc_employee_statutory_dtls"))
                {

                    proc.AddVarcharPara("@Action", 50, "SaveEmpOthrDtls");
                    proc.AddVarcharPara("@EmployeeCode", 100, EmployeeCode);
                    proc.AddPara("@JoiningDate", JoiningDate);
                    proc.AddPara("@LeavingDate", LeavingDate);
                    proc.AddVarcharPara("@Unit", 10,unit);
                    proc.AddVarcharPara("@Department", 100, dept);
                    proc.AddVarcharPara("@Designation", 100, desig);
                    proc.AddVarcharPara("@Grade", 10,grade);
                    proc.AddIntegerPara("@CreatedBy", Convert.ToInt32(CreateUser));
                    proc.AddVarcharPara("@ReturnMessage", 500, "", QueryParameterDirection.Output);
                    proc.AddVarcharPara("@ReturnCode", 20, "", QueryParameterDirection.Output);
                    NoOfRowEffected = proc.RunActionQuery();

                    output = Convert.ToString(proc.GetParaValue("@ReturnMessage"));
                    ReturnCode = Convert.ToString(proc.GetParaValue("@ReturnCode"));


                }
            }

            catch (Exception ex)
            {
                output = ex.Message.ToString();
            }

            finally
            {
                proc = null;
            }
            return output;
        }
    }

     
}
