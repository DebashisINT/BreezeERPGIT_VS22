using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;
using System.Web;
using System.Globalization;


namespace BusinessLogicLayer
{
    public class Leads
    {

        IFormatProvider provider = new CultureInfo("en-US");

        public string btnSave_Click_BL(string cnt_ucc, string cnt_salutation, string cnt_firstName,
      string cnt_middleName, string cnt_lastName, string cnt_shortName, string cnt_branchId,
      string cnt_sex, string cnt_maritalStatus, string DOBDate, string AnniversaryDate,
      string cmbLegalStatus, string cmbEducation, string cmbSource, string referedID, string cmbBloodgroup,
      string cmbProfession, string organization, string cmbJobResponsibility, string cmbDesignation, string cmbIndustry,
      string cmbContactStatus, string cmbRating)
        {
            ProcedureExecute proc;

            try
            {
                using (proc = new ProcedureExecute("LeadInsert"))
                {
                    proc.AddVarcharPara("@cnt_ucc", 100, cnt_ucc);

                    proc.AddIntegerPara("@cnt_salutation", Convert.ToInt32(cnt_salutation));

                    proc.AddVarcharPara("@cnt_firstName", 100, cnt_firstName);

                    proc.AddVarcharPara("@cnt_middleName", 100, cnt_middleName);

                    proc.AddVarcharPara("@cnt_lastName", 100, cnt_lastName);

                    proc.AddVarcharPara("@cnt_shortName", 100, cnt_shortName);

                    proc.AddIntegerPara("@cnt_branchId", Convert.ToInt32(cnt_branchId));

                    proc.AddIntegerPara("@cnt_sex", Convert.ToInt32(cnt_sex));

                    proc.AddIntegerPara("@cnt_maritalStatus", Convert.ToInt32(cnt_maritalStatus));


                    if (!string.IsNullOrEmpty(DOBDate))
                    {
                        proc.AddDateTimePara("@cnt_DOB", Convert.ToDateTime(DOBDate, provider));
                    }

                    if (!string.IsNullOrEmpty(AnniversaryDate))
                    {
                        proc.AddDateTimePara("@cnt_anniversaryDate", Convert.ToDateTime(AnniversaryDate, provider));
                    }

                    proc.AddIntegerPara("@cnt_legalStatus", Convert.ToInt32(cmbLegalStatus));
                    proc.AddIntegerPara("@cnt_education", Convert.ToInt32(cmbEducation));
                    //................................
                    proc.AddIntegerPara("@cnt_profession", 0);
                    proc.AddVarcharPara("@cnt_organization", 100, organization);
                    proc.AddVarcharPara("@cnt_jobResponsibility", 100, cmbJobResponsibility);
                    proc.AddIntegerPara("@cnt_designation", Convert.ToInt32(cmbDesignation));
                    proc.AddIntegerPara("@cnt_industry", Convert.ToInt32(cmbIndustry));
                    proc.AddIntegerPara("@cnt_contactSource", Convert.ToInt32(cmbSource));
                    int referedIDInt;
                    Int32.TryParse(referedID, out referedIDInt);
                    proc.AddIntegerPara("@cnt_referedBy", referedIDInt);
                    proc.AddVarcharPara("@cnt_contactType", 100, "EM");
                    proc.AddIntegerPara("@cnt_contactStatus", Convert.ToInt32(cmbContactStatus));
                    proc.AddVarcharPara("@cnt_rating", 100, cmbRating);

                    proc.AddVarcharPara("@lastModifyUser", 100, Convert.ToString(HttpContext.Current.Session["userid"]));
                    proc.AddVarcharPara("@bloodgroup", 100, cmbBloodgroup);
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
