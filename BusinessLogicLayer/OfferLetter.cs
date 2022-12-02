using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class OfferLetter
    {
        public string AddNewCandidate(string rde_Salutation, string rde_Name, string rde_SourceType, string rde_SourceName,
                                string rde_CandidateSex, string rde_DOB, string rde_EduQualification,
                                string rde_ProfQualification, string rde_Certification, string rde_WorkExp,
            string rde_CTC, string rde_ReasonForChange, string rde_ProbableJoinDate, string rde_ResidenceLocation, 
            string rde_CurrentEmployer, string rde_CurrentJobProfile,
            string rde_CurrentCTC, string rde_PreviousEmployer, string rde_PreviousJobProfile, string rde_PreviousCTC,
            string rde_NoofDepedent, string rde_PhoneNo,
            string rde_Email, string rde_MaritalStatus, string rde_Company, string rde_Branch, string rde_Designation,
            string rde_ApprovedCTC, DateTime CreateDate,
            string CreateUser, string rde_Status, string rde_FatherName, string rde_EmpType, string rde_ReportTo, string rde_Dept)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {


                using (proc = new ProcedureExecute("sp_AddNewCandidate"))
                {
                    proc.AddVarcharPara("@rde_Salutation", 100, rde_Salutation);
                    proc.AddVarcharPara("@rde_Name", 100, rde_Name);
                    proc.AddVarcharPara("@rde_SourceType", 100, rde_SourceType);
                    proc.AddVarcharPara("@rde_SourceName", 100, rde_SourceName);
                    proc.AddVarcharPara("@rde_CandidateSex", 100, rde_CandidateSex);
                    proc.AddVarcharPara("@rde_DOB", 100, rde_DOB);
                    proc.AddVarcharPara("@rde_EduQualification", 100, rde_EduQualification);
                    proc.AddVarcharPara("@rde_ProfQualification", 100, rde_ProfQualification);
                    proc.AddVarcharPara("@rde_Certification", 100, rde_Certification);
                    proc.AddVarcharPara("@rde_WorkExp", 100, rde_WorkExp);
                    proc.AddVarcharPara("@rde_CTC", 100, rde_CTC);
                    proc.AddVarcharPara("@rde_ReasonForChange", 100, rde_ReasonForChange);
                    proc.AddVarcharPara("@rde_ProbableJoinDate", 100, rde_ProbableJoinDate);
                    proc.AddVarcharPara("@rde_ResidenceLocation", 100, rde_ResidenceLocation);
                    proc.AddVarcharPara("@rde_CurrentEmployer", 100, rde_CurrentEmployer);
                    proc.AddVarcharPara("@rde_CurrentJobProfile", 100, rde_CurrentJobProfile);
                    proc.AddVarcharPara("@rde_CurrentCTC", 100, rde_CurrentCTC);
                    proc.AddVarcharPara("@rde_PreviousEmployer", 100, rde_PreviousEmployer);
                    proc.AddVarcharPara("@rde_PreviousJobProfile", 100, rde_PreviousJobProfile);
                    proc.AddVarcharPara("@rde_PreviousCTC", 100, rde_PreviousCTC);
                    proc.AddVarcharPara("@rde_NoofDepedent", 100, rde_NoofDepedent);
                    proc.AddVarcharPara("@rde_PhoneNo", 100, rde_PhoneNo);
                    proc.AddVarcharPara("@rde_Email", 100, rde_Email);
                    proc.AddVarcharPara("@rde_MaritalStatus", 100, rde_MaritalStatus);
                    proc.AddVarcharPara("@rde_Company", 100, rde_Company);
                    proc.AddVarcharPara("@rde_Branch", 100, rde_Branch);
                    proc.AddVarcharPara("@rde_Designation", 100, rde_Designation);
                    proc.AddVarcharPara("@rde_ApprovedCTC", 100, rde_ApprovedCTC);
                    proc.AddDateTimePara("@CreateDate", CreateDate);
                    proc.AddVarcharPara("@CreateUser", 100, CreateUser);
                    proc.AddVarcharPara("@rde_Status", 100, rde_Status);
                    proc.AddVarcharPara("@rde_FatherName", 100, rde_FatherName);
                    proc.AddVarcharPara("@rde_EmpType", 100, rde_EmpType);
                    proc.AddVarcharPara("@rde_ReportTo", 100, rde_ReportTo);
                    proc.AddVarcharPara("@rde_Dept", 100, rde_Dept);
                    proc.AddVarcharPara("@result", 4, rtrnvalue);

                    int i = proc.RunActionQuery();
                    rtrnvalue = proc.GetParaValue("@result").ToString();
                    return rtrnvalue;


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
