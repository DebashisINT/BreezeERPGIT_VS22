using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class OutSourcing_Agent_Company
    {


      //"Outsourcing Agents/Companies", "", CmbSalutation.SelectedItem.Value.ToString(), txtFirstName.Text.Trim(),
      //                     "", "", txtCode.Text, cmbBranch.SelectedItem.Value.ToString(), "", "", dtDob, dtanniversary, cmbLegalStatus.SelectedItem.Value.ToString(),
      //                     "", "", "", "", "", "", "", "", "", "", cmbSource.SelectedItem.Value.ToString(), txtReferedBy.Text.Trim(), "OC",
      //                     cmbContactStatus.SelectedItem.Value.ToString(), HttpContext.Current.Session["userid"].ToString(), "",
      //                     "No", ""

        public string Insert_ContactGeneral(string vcontacttype, string vcnt_ucc, string vcnt_salutation, string vcnt_firstName,
                                           string vcnt_middleName, string vcnt_lastName, string vcnt_shortName, string vcnt_branchId, string vcnt_sex,
                                           string vcnt_maritalStatus, DateTime vcnt_DOB, DateTime vcnt_anniversaryDate, string vcnt_legalStatus,
                                           string vcnt_education, string vcnt_profession, string vcnt_organization, string vcnt_jobResponsibility,
                                           string vcnt_designation, string vcnt_industry, 
                                           string vRPartner,DateTime vcnt_RegistrationDate,string vcnt_rating,string vcnt_reason,
                                           string vcnt_contactSource,string vcnt_referedBy,string vcnt_contactType,
                                           string vcnt_contactStatus,string vlastModifyUser,string vbloodgroup,string vWebLogIn,string vPassWord
            )
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {


                using (proc = new ProcedureExecute("ContactInsert"))
                {
                    int vcnt_salutation_int = 0;
                    if (!int.TryParse(vcnt_salutation, out vcnt_salutation_int))
                    {
                        vcnt_salutation_int = 0;
                    }

                    int vcnt_branchId_int = 0;
                    if (!int.TryParse(vcnt_branchId, out vcnt_branchId_int))
                    {
                        vcnt_branchId_int = 0;
                    }

                    int vcnt_sex_int = 0;
                    if (!int.TryParse(vcnt_sex, out vcnt_sex_int))
                    {
                        vcnt_sex_int = 0;
                    }

                    int vcnt_maritalStatus_int = 0;
                    if (!int.TryParse(vcnt_maritalStatus, out vcnt_maritalStatus_int))
                    {
                        vcnt_maritalStatus_int = 0;
                    }

                    int vcnt_legalStatus_int = 0;
                    if (!int.TryParse(vcnt_legalStatus, out vcnt_legalStatus_int))
                    {
                        vcnt_legalStatus_int = 0;
                    }

                    int vcnt_education_int = 0;
                    if (!int.TryParse(vcnt_education, out vcnt_education_int))
                    {
                        vcnt_education_int = 0;
                    }

                    int vcnt_profession_int = 0;
                    if (!int.TryParse(vcnt_profession, out vcnt_profession_int))
                    {
                        vcnt_profession_int = 0;
                    }

                    int vcnt_jobResponsibility_int = 0;
                    if (!int.TryParse(vcnt_jobResponsibility, out vcnt_jobResponsibility_int))
                    {
                        vcnt_jobResponsibility_int = 0;
                    }

                    int vcnt_designation_int = 0;
                    if (!int.TryParse(vcnt_designation, out vcnt_designation_int))
                    {
                        vcnt_designation_int = 0;
                    }

                    int vcnt_industry_int = 0;
                    if (!int.TryParse(vcnt_industry, out vcnt_industry_int))
                    {
                        vcnt_industry_int = 0;
                    }

                    int vcnt_rating_int = 0;
                    if (!int.TryParse(vcnt_rating, out vcnt_rating_int))
                    {
                        vcnt_rating_int = 0;
                    }

                    int vcnt_contactSource_int = 0;
                    if (!int.TryParse(vcnt_contactSource, out vcnt_contactSource_int))
                    {
                        vcnt_contactSource_int = 0;
                    }

                    int vcnt_contactStatus_int = 0;
                    if (!int.TryParse(vcnt_contactStatus, out vcnt_contactStatus_int))
                    {
                        vcnt_contactStatus_int = 0;
                    }

                    proc.AddVarcharPara("@contacttype", 100, vcontacttype);
                    proc.AddVarcharPara("@cnt_ucc", 100, vcnt_ucc);
                    proc.AddIntegerPara("@cnt_salutation", Convert.ToInt32(vcnt_salutation_int));
                    proc.AddVarcharPara("@cnt_firstName", 100, vcnt_firstName);
                    proc.AddVarcharPara("@cnt_middleName", 100, vcnt_middleName);
                    proc.AddVarcharPara("@cnt_lastName", 100, vcnt_lastName);
                    proc.AddVarcharPara("@cnt_shortName", 100, vcnt_shortName);
                    proc.AddIntegerPara("@cnt_branchId", Convert.ToInt32(vcnt_branchId_int));
                    proc.AddIntegerPara("@cnt_sex", Convert.ToInt32(vcnt_sex_int));
                    proc.AddIntegerPara("@cnt_maritalStatus", Convert.ToInt32(vcnt_maritalStatus_int));
                    proc.AddDateTimePara("@cnt_DOB", vcnt_DOB);
                    proc.AddDateTimePara("@cnt_anniversaryDate", vcnt_anniversaryDate);                
                    proc.AddIntegerPara("@cnt_legalStatus", Convert.ToInt32(vcnt_legalStatus_int));
                    proc.AddIntegerPara("@cnt_education", Convert.ToInt32(vcnt_education_int));
                    proc.AddIntegerPara("@cnt_profession", Convert.ToInt32(vcnt_profession_int));
                    proc.AddVarcharPara("@cnt_organization", 100, vcnt_organization);
                    proc.AddIntegerPara("@cnt_jobResponsibility", Convert.ToInt32(vcnt_jobResponsibility_int));
                    proc.AddIntegerPara("@cnt_designation", Convert.ToInt32(vcnt_designation_int));
                    proc.AddIntegerPara("@cnt_industry", Convert.ToInt32(vcnt_industry_int));
                    proc.AddVarcharPara("@RPartner", 100, vRPartner);
                    proc.AddDateTimePara("@cnt_RegistrationDate", vcnt_RegistrationDate);
                    proc.AddIntegerPara("@cnt_rating", Convert.ToInt32(vcnt_rating_int));
                    proc.AddVarcharPara("@cnt_reason", 400, vcnt_reason);
                    proc.AddIntegerPara("@cnt_contactSource", Convert.ToInt32(vcnt_contactSource_int));
                    proc.AddVarcharPara("@cnt_referedBy", 100, vcnt_referedBy);
                    proc.AddVarcharPara("@cnt_contactType", 100, vcnt_contactType);
                    proc.AddIntegerPara("@cnt_contactStatus", Convert.ToInt32(vcnt_contactStatus_int));
                    proc.AddVarcharPara("@lastModifyUser", 100, vlastModifyUser);                 
                    proc.AddVarcharPara("@bloodgroup", 100, vbloodgroup);
                    proc.AddVarcharPara("@WebLogIn", 100, vWebLogIn);
                    proc.AddVarcharPara("@PassWord", 100, vPassWord);


                  


                   
                    //proc.AddVarcharPara("@placeincorporation", 100, vplaceincorporation);

                    //proc.AddDateTimePara("@datebusscommence", vdatebusscommence);

                    //proc.AddVarcharPara("@otheroccu", 100, votheroccu);
                    //proc.AddIntegerPara("@country", Convert.ToInt32(vcountry));
                  
                    proc.AddVarcharPara("@result", 50, "", QueryParameterDirection.Output);





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
