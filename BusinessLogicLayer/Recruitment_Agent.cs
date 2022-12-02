using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class Recruitment_Agents
    {
        //rev srijeeta mantis issue 0024515
        //public string Insert_ContactGeneral(string vcontacttype, string vcnt_ucc, string vcnt_salutation, string vcnt_firstName,
        //                                 string vcnt_middleName, string vcnt_lastName, string vcnt_shortName, string vcnt_branchId, string vcnt_sex,
        //                                 string vcnt_maritalStatus, DateTime vcnt_DOB, DateTime vcnt_anniversaryDate, string vcnt_legalStatus,
        //                                 string vcnt_education, string vcnt_profession, string vcnt_organization, string vcnt_jobResponsibility,
        //                                 string vcnt_designation, string vcnt_industry,
        //                                 string vRPartner, DateTime vcnt_RegistrationDate, string vcnt_rating, string vcnt_reason,
        //                                 string vcnt_contactSource, string vcnt_referedBy, string vcnt_contactType,
        //                                 string vcnt_contactStatus, string vlastModifyUser, string vbloodgroup, string vWebLogIn, string vPassWord, string GSTIN, string cnt_PrintNameToCheque, string vendortype,int NumberingId=0
        //                                   , string NameAsperPan = null, string DeducteeStatus=null,string TaxdeducteeType=null,string ProductList=null)
        public string Insert_ContactGeneral(string vcontacttype, string vcnt_ucc,string Alternative_Code, string vcnt_salutation, string vcnt_firstName,
                                         string vcnt_middleName, string vcnt_lastName, string vcnt_shortName, string vcnt_branchId, string vcnt_sex,
                                         string vcnt_maritalStatus, DateTime vcnt_DOB, DateTime vcnt_anniversaryDate, string vcnt_legalStatus,
                                         string vcnt_education, string vcnt_profession, string vcnt_organization, string vcnt_jobResponsibility,
                                         string vcnt_designation, string vcnt_industry,
                                         string vRPartner, DateTime vcnt_RegistrationDate, string vcnt_rating, string vcnt_reason,
                                         string vcnt_contactSource, string vcnt_referedBy, string vcnt_contactType,
                                         string vcnt_contactStatus, string vlastModifyUser, string vbloodgroup, string vWebLogIn, string vPassWord, string GSTIN, string cnt_PrintNameToCheque, string vendortype,int NumberingId=0
                                           , string NameAsperPan = null, string DeducteeStatus=null,string TaxdeducteeType=null,string ProductList=null)
        //rev srijeeta mantis issue 0024515
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {


                using (proc = new ProcedureExecute("ContactInsert"))
                {

                    proc.AddVarcharPara("@contacttype", 100, vcontacttype);
                    proc.AddVarcharPara("@cnt_ucc", 100, vcnt_ucc);
                    //rev srijeeta mantis issue 0024515
                    proc.AddVarcharPara("@Alternative_Code ", 100, Alternative_Code);
                    //end of rev srijeeta mantis issue 0024515
                    //proc.AddIntegerPara("@cnt_salutation", Convert.ToInt32(vcnt_salutation));
                    proc.AddVarcharPara("@cnt_salutation", 100, vcnt_salutation);
                    proc.AddVarcharPara("@cnt_firstName", 100, vcnt_firstName);
                    proc.AddVarcharPara("@cnt_middleName", 100, vcnt_middleName);
                    proc.AddVarcharPara("@cnt_lastName", 100, vcnt_lastName);
                    proc.AddVarcharPara("@cnt_shortName", 80, vcnt_shortName);
                    //proc.AddIntegerPara("@cnt_branchId", Convert.ToInt32(vcnt_branchId));
                    proc.AddVarcharPara("@cnt_branchId", 100, vcnt_branchId);
                    // proc.AddIntegerPara("@cnt_sex", Convert.ToInt32(vcnt_sex));
                    proc.AddVarcharPara("@cnt_sex", 100, vcnt_sex);
                    // proc.AddIntegerPara("@cnt_maritalStatus", Convert.ToInt32(vcnt_maritalStatus));
                    proc.AddVarcharPara("@cnt_maritalStatus", 100, vcnt_maritalStatus);
                    proc.AddDateTimePara("@cnt_DOB", vcnt_DOB);
                    proc.AddDateTimePara("@cnt_anniversaryDate", vcnt_anniversaryDate);
                    //proc.AddIntegerPara("@cnt_legalStatus", Convert.ToInt32(vcnt_legalStatus));
                    proc.AddVarcharPara("@cnt_legalStatus", 100, vcnt_legalStatus);
                    //proc.AddIntegerPara("@cnt_education", Convert.ToInt32(vcnt_education));
                    proc.AddVarcharPara("@cnt_education", 100, vcnt_education);
                    //proc.AddIntegerPara("@cnt_profession", Convert.ToInt32(vcnt_profession));
                    proc.AddVarcharPara("@cnt_profession", 100, vcnt_profession);
                    proc.AddVarcharPara("@cnt_organization", 100, vcnt_organization);
                    //proc.AddIntegerPara("@cnt_jobResponsibility", Convert.ToInt32(vcnt_jobResponsibility));
                    proc.AddVarcharPara("@cnt_jobResponsibility", 100, vcnt_jobResponsibility);
                    //proc.AddIntegerPara("@cnt_designation", Convert.ToInt32(vcnt_designation));
                    proc.AddVarcharPara("@cnt_designation", 100, vcnt_designation);
                    //proc.AddIntegerPara("@cnt_industry", Convert.ToInt32(vcnt_industry));
                    proc.AddVarcharPara("@cnt_industry", 100, vcnt_industry);
                    proc.AddVarcharPara("@RPartner", 100, vRPartner);
                    proc.AddDateTimePara("@cnt_RegistrationDate", vcnt_RegistrationDate);
                    //proc.AddIntegerPara("@cnt_rating", Convert.ToInt32(vcnt_rating));
                    proc.AddVarcharPara("@cnt_rating", 100, vcnt_rating);
                    proc.AddVarcharPara("@cnt_reason", 400, vcnt_reason);
                    //proc.AddIntegerPara("@cnt_contactSource", Convert.ToInt32(vcnt_contactSource));
                    proc.AddVarcharPara("@cnt_contactSource", 100, vcnt_contactSource);
                    proc.AddVarcharPara("@cnt_referedBy", 100, vcnt_referedBy);
                    proc.AddVarcharPara("@cnt_contactType", 100, vcnt_contactType);
                    //  proc.AddIntegerPara("@cnt_contactStatus", Convert.ToInt32(vcnt_contactStatus));
                    proc.AddVarcharPara("@cnt_contactStatus", 100, vcnt_contactStatus);
                    proc.AddVarcharPara("@lastModifyUser", 100, vlastModifyUser);
                    proc.AddVarcharPara("@bloodgroup", 100, vbloodgroup);
                    proc.AddVarcharPara("@WebLogIn", 100, vWebLogIn);
                    proc.AddVarcharPara("@PassWord", 100, vPassWord);


                    //-----------------------------
                    //Debjyoti Add GSTIN for Vendor
                    proc.AddVarcharPara("@CNT_GSTIN", 15, GSTIN);
                    //Debjyoti End Here
                    proc.AddVarcharPara("@cnt_PrintNameToCheque", 200, cnt_PrintNameToCheque);



                    proc.AddVarcharPara("@placeincorporation", 100, "");
                    proc.AddDateTimePara("@datebusscommence", Convert.ToDateTime("01-01-1900"));
                    proc.AddVarcharPara("@otheroccu", 100, "");
                    proc.AddIntegerPara("@country", Convert.ToInt32(0));
                    proc.AddVarcharPara("@vendortype", 3, vendortype);
                    proc.AddIntegerPara("@NumberSchemaId", Convert.ToInt32(NumberingId));
                    proc.AddVarcharPara("@NameAsperPan", 400, NameAsperPan);
                    proc.AddVarcharPara("@DeducteeStatus", 400, DeducteeStatus);
                    proc.AddVarcharPara("@TaxdecucteeType", 20, TaxdeducteeType);
                    //Rev Bapi
                    proc.AddVarcharPara("@ProductIds", 100, ProductList);
                    //End  rev Bapi
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
        //rev srijeeta mantis issue 0024515
       // public string Insert_ContactGeneralForCopyVendor(DateTime DtIncoorporationDtae, string OLDID, string vcontacttype, string vcnt_ucc, string vcnt_salutation, string vcnt_firstName,
       //                               string vcnt_middleName, string vcnt_lastName, string vcnt_shortName, string vcnt_branchId, string vcnt_sex,
       //                               string vcnt_maritalStatus, DateTime vcnt_DOB, DateTime vcnt_anniversaryDate, string vcnt_legalStatus,
       //                               string vcnt_education, string vcnt_profession, string vcnt_organization, string vcnt_jobResponsibility,
       //                               string vcnt_designation, string vcnt_industry,
       //                               string vRPartner, DateTime vcnt_RegistrationDate, string vcnt_rating, string vcnt_reason,
       //                               string vcnt_contactSource, string vcnt_referedBy, string vcnt_contactType,
       //                               string vcnt_contactStatus, string vlastModifyUser, string vbloodgroup, string vWebLogIn, string vPassWord, string GSTIN, string cnt_PrintNameToCheque, string vendortype
       //, int NumberingId = 0)
        public string Insert_ContactGeneralForCopyVendor(DateTime DtIncoorporationDtae, string OLDID, string vcontacttype, string vcnt_ucc, string Alternative_Code, string vcnt_salutation, string vcnt_firstName,
                                     string vcnt_middleName, string vcnt_lastName, string vcnt_shortName, string vcnt_branchId, string vcnt_sex,
                                     string vcnt_maritalStatus, DateTime vcnt_DOB, DateTime vcnt_anniversaryDate, string vcnt_legalStatus,
                                     string vcnt_education, string vcnt_profession, string vcnt_organization, string vcnt_jobResponsibility,
                                     string vcnt_designation, string vcnt_industry,
                                     string vRPartner, DateTime vcnt_RegistrationDate, string vcnt_rating, string vcnt_reason,
                                     string vcnt_contactSource, string vcnt_referedBy, string vcnt_contactType,
                                     string vcnt_contactStatus, string vlastModifyUser, string vbloodgroup, string vWebLogIn, string vPassWord, string GSTIN, string cnt_PrintNameToCheque, string vendortype
      , int NumberingId = 0)
        //end of rev srijeeta mantis issue 0024515
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            string newid = "";
            try
            {


                using (proc = new ProcedureExecute("ContactInsertForVendorCopy"))
                {
                    proc.AddVarcharPara("@oldcustomerid", 100, OLDID);
                    proc.AddVarcharPara("@contacttype", 100, vcontacttype);
                    proc.AddVarcharPara("@cnt_ucc", 100, vcnt_ucc);
                    //rev srijeeta mantis issue 0024515
                    proc.AddVarcharPara("@Alternative_Code", 100, Alternative_Code);
                    //end of rev srijeeta mantis issue 0024515
                    //proc.AddIntegerPara("@cnt_salutation", Convert.ToInt32(vcnt_salutation));
                    proc.AddVarcharPara("@cnt_salutation", 100, vcnt_salutation);
                    proc.AddVarcharPara("@cnt_firstName", 100, vcnt_firstName);
                    proc.AddVarcharPara("@cnt_middleName", 100, vcnt_middleName);
                    proc.AddVarcharPara("@cnt_lastName", 100, vcnt_lastName);
                    proc.AddVarcharPara("@cnt_shortName", 80, vcnt_shortName);
                    //proc.AddIntegerPara("@cnt_branchId", Convert.ToInt32(vcnt_branchId));
                    proc.AddVarcharPara("@cnt_branchId", 100, vcnt_branchId);
                    // proc.AddIntegerPara("@cnt_sex", Convert.ToInt32(vcnt_sex));
                    proc.AddVarcharPara("@cnt_sex", 100, vcnt_sex);
                    // proc.AddIntegerPara("@cnt_maritalStatus", Convert.ToInt32(vcnt_maritalStatus));
                    proc.AddVarcharPara("@cnt_maritalStatus", 100, vcnt_maritalStatus);
                    proc.AddDateTimePara("@cnt_DOB", vcnt_DOB);
                    proc.AddDateTimePara("@cnt_anniversaryDate", vcnt_anniversaryDate);
                    //proc.AddIntegerPara("@cnt_legalStatus", Convert.ToInt32(vcnt_legalStatus));
                    proc.AddVarcharPara("@cnt_legalStatus", 100, vcnt_legalStatus);
                    //proc.AddIntegerPara("@cnt_education", Convert.ToInt32(vcnt_education));
                    proc.AddVarcharPara("@cnt_education", 100, vcnt_education);
                    //proc.AddIntegerPara("@cnt_profession", Convert.ToInt32(vcnt_profession));
                    proc.AddVarcharPara("@cnt_profession", 100, vcnt_profession);
                    proc.AddVarcharPara("@cnt_organization", 100, vcnt_organization);
                    //proc.AddIntegerPara("@cnt_jobResponsibility", Convert.ToInt32(vcnt_jobResponsibility));
                    proc.AddVarcharPara("@cnt_jobResponsibility", 100, vcnt_jobResponsibility);
                    //proc.AddIntegerPara("@cnt_designation", Convert.ToInt32(vcnt_designation));
                    proc.AddVarcharPara("@cnt_designation", 100, vcnt_designation);
                    //proc.AddIntegerPara("@cnt_industry", Convert.ToInt32(vcnt_industry));
                    proc.AddVarcharPara("@cnt_industry", 100, vcnt_industry);
                    proc.AddVarcharPara("@RPartner", 100, vRPartner);
                    proc.AddDateTimePara("@cnt_RegistrationDate", vcnt_RegistrationDate);
                    //proc.AddIntegerPara("@cnt_rating", Convert.ToInt32(vcnt_rating));
                    proc.AddVarcharPara("@cnt_rating", 100, vcnt_rating);
                    proc.AddVarcharPara("@cnt_reason", 400, vcnt_reason);
                    //proc.AddIntegerPara("@cnt_contactSource", Convert.ToInt32(vcnt_contactSource));
                    proc.AddVarcharPara("@cnt_contactSource", 100, vcnt_contactSource);
                    proc.AddVarcharPara("@cnt_referedBy", 100, vcnt_referedBy);
                    proc.AddVarcharPara("@cnt_contactType", 100, vcnt_contactType);
                    //  proc.AddIntegerPara("@cnt_contactStatus", Convert.ToInt32(vcnt_contactStatus));
                    proc.AddVarcharPara("@cnt_contactStatus", 100, vcnt_contactStatus);
                    proc.AddVarcharPara("@lastModifyUser", 100, vlastModifyUser);
                    proc.AddVarcharPara("@bloodgroup", 100, vbloodgroup);
                    proc.AddVarcharPara("@WebLogIn", 100, vWebLogIn);
                    proc.AddVarcharPara("@PassWord", 100, vPassWord);

                    proc.AddDateTimePara("@DtIncoorporationDtae", DtIncoorporationDtae);

                    //-----------------------------
                    //Debjyoti Add GSTIN for Vendor
                    proc.AddVarcharPara("@CNT_GSTIN", 15, GSTIN);
                    //Debjyoti End Here
                    proc.AddVarcharPara("@cnt_PrintNameToCheque", 200, cnt_PrintNameToCheque);



                    proc.AddVarcharPara("@placeincorporation", 100, "");
                    proc.AddDateTimePara("@datebusscommence", Convert.ToDateTime("01-01-1900"));
                    proc.AddVarcharPara("@otheroccu", 100, "");
                    proc.AddIntegerPara("@country", Convert.ToInt32(0));
                    proc.AddVarcharPara("@vendortype", 3, vendortype);
                    proc.AddIntegerPara("@NumberSchemaId", Convert.ToInt32(NumberingId));
                    
                    proc.AddVarcharPara("@result", 50, "", QueryParameterDirection.Output);
                    proc.AddVarcharPara("@newid", 50, "", QueryParameterDirection.Output);


                    int i = proc.RunActionQuery();
                    rtrnvalue = proc.GetParaValue("@result").ToString();
                    newid = proc.GetParaValue("@newid").ToString();
                    return rtrnvalue +'~'+newid;


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
