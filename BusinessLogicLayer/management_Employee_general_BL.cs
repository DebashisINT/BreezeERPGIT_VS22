using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;
using System.Web;

namespace BusinessLogicLayer
{
    public class management_Employee_general_BL
    {
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
                        proc.AddVarcharPara("@cnt_DOB",100, "");
                    }
                    if (AnniversaryDate != null)
                    {
                        proc.AddVarcharPara("@cnt_anniversaryDate",100, AnniversaryDate);
                    }
                    else
                    {
                        proc.AddVarcharPara("@cnt_anniversaryDate",100, "");
                    }
                    proc.AddVarcharPara("@cnt_legalStatus", 100, cmbLegalStatus);
                    proc.AddVarcharPara("@cnt_education", 100, cmbEducation );
                    proc.AddVarcharPara("@cnt_contactSource", 100, cmbSource);
                    proc.AddVarcharPara("@cnt_referedBy",100, referedID);
                    proc.AddVarcharPara("@cnt_contactType",100, "EM");
                    proc.AddVarcharPara("@lastModifyUser", 100, Convert.ToString(HttpContext.Current.Session["userid"]));
                    proc.AddVarcharPara("@UserContactID",100, Convert.ToString(HttpContext.Current.Session["usercontactID"]));
                    proc.AddVarcharPara("@bloodgroup",100, cmbBloodgroup);
                    string webLogin = "No";
                    string Password = string.Empty;
                    if (chkAllow == true)
                    {
                        webLogin = "Yes";
                        Password = password;
                    } 
                    proc.AddVarcharPara("@webLogin",100, webLogin);
                    proc.AddVarcharPara("@Password", 100, Password); 
                    proc.AddVarcharPara("@result", 100, "", QueryParameterDirection.Output); 
                    int NoOfRowEffected = proc.RunActionQuery();
                    string RetVal = Convert.ToString(proc.GetParaValue("@result")); 
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
    }
}
