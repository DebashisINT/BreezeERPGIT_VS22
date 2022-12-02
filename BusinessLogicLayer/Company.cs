using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class Company
    {



        // Code  Added and Commented By Priti on 15122016 to add 6 fields
        //public string Insert_Company(string vcmp_Name, string vcmp_natureOfBusiness, string vcmp_directors,
        //     string vcmp_authorizedSignatories, string vcmp_registrationNo, string vcmp_panNo, string vcmp_serviceTaxNo,
        //     string vcmp_salesTaxNo, string vCreateUser, string vcmp_CIN, DateTime vcmp_DateIncorporation,
        //     DateTime vcmp_CINdt, DateTime vcmp_VregisNo, DateTime vcmp_VPanNo,
        //     string vcmp_parentid, string vcmp_currencyid, string vcmp_OffRoleShortName, string vcmp_OnRoleShortName,
        //     string vcmp_Kra_Prefix, string vcmp_Kra_imid,string vat_registration_no 
        //    )
     
        public string Insert_Company(string vcmp_Name, string vcmp_natureOfBusiness, string vcmp_directors,
            string vcmp_authorizedSignatories, string vcmp_registrationNo, string vcmp_panNo, string vcmp_serviceTaxNo,
            string vcmp_salesTaxNo, string vCreateUser, string vcmp_CIN, DateTime vcmp_DateIncorporation,
            DateTime vcmp_CINdt, DateTime vcmp_VregisNo, DateTime vcmp_VPanNo,
            string vcmp_parentid, string vcmp_currencyid, string vcmp_OffRoleShortName, string vcmp_OnRoleShortName,
            string vcmp_Kra_Prefix, string vcmp_Kra_imid, string vat_registration_no, string cmp_EPFRegistrationNo, DateTime cmp_EPFRegisNoValidfrom,
            DateTime cmp_EPFRegistrationNoValidupto, string cmp_ESICRegistrationNo, DateTime cmp_ESICRegistrationNoValidfrom, DateTime cmp_ESICRegistrationNoValidupto,
            string GSTIN, String MSMEUdyamRCNo
           )
            //..............end...............
                                   
          {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {
                using (proc = new ProcedureExecute("CompanyInsert"))
                {
                    
                    proc.AddVarcharPara("@cmp_Name",100, vcmp_Name);
                    proc.AddVarcharPara("@cmp_natureOfBusiness",100, vcmp_natureOfBusiness);
                    proc.AddVarcharPara("@cmp_directors", 100, vcmp_directors);
                    proc.AddVarcharPara("@cmp_authorizedSignatories", 100, vcmp_authorizedSignatories);
                    proc.AddVarcharPara("@cmp_registrationNo", 100, vcmp_registrationNo);
                    proc.AddVarcharPara("@cmp_panNo", 100, vcmp_panNo);
                    proc.AddVarcharPara("@cmp_serviceTaxNo", 100, vcmp_serviceTaxNo);
                    proc.AddVarcharPara("@cmp_salesTaxNo", 100, vcmp_salesTaxNo);
                    proc.AddVarcharPara("@CreateUser", 100, vCreateUser);
                    proc.AddNVarcharPara("@cmp_CIN", 100, vcmp_CIN);

                    proc.AddDateTimePara("@cmp_DateIncorporation", vcmp_DateIncorporation);
                    proc.AddDateTimePara("@cmp_CINdt", vcmp_CINdt);
                    proc.AddDateTimePara("@cmp_VregisNo", vcmp_VregisNo);
                    proc.AddDateTimePara("@cmp_VPanNo", vcmp_VPanNo);

                    proc.AddVarcharPara("@cmp_parentid", 100, vcmp_parentid);
                    proc.AddVarcharPara("@cmp_currencyid", 100, vcmp_currencyid);
                    proc.AddVarcharPara("@cmp_OffRoleShortName", 15, vcmp_OffRoleShortName);
                    proc.AddVarcharPara("@cmp_OnRoleShortName", 15, vcmp_OnRoleShortName);
                    proc.AddVarcharPara("@cmp_Kra_Prefix", 100, vcmp_Kra_Prefix);
                    proc.AddVarcharPara("@cmp_Kra_imid", 100, vcmp_Kra_imid);
                    //bellow two parameter vat no and tin no added by debjyoti 30-11-2016
                    proc.AddVarcharPara("@cmp_vat_registration_no", 20, vat_registration_no);
                   // proc.AddVarcharPara("@cmp_tin_no", 10, tin_no);
                    // Code  Added and Commented By Priti on 15122016 to add 6 fields
                    proc.AddVarcharPara("@cmp_EPFRegistrationNo", 20, cmp_EPFRegistrationNo);
                    proc.AddDateTimePara("@cmp_EPFRegisNoValidfrom", cmp_EPFRegisNoValidfrom);
                    proc.AddDateTimePara("@cmp_EPFRegisNoValidupto", cmp_EPFRegistrationNoValidupto);
                    proc.AddVarcharPara("@cmp_ESICRegisNo", 20, cmp_ESICRegistrationNo);
                    proc.AddDateTimePara("@cmp_ESICRegisNoValidfrom", cmp_ESICRegistrationNoValidfrom);
                    proc.AddDateTimePara("@cmp_ESICRegisNoValidupto", cmp_ESICRegistrationNoValidupto);
                    //............end................

                    //Debjyoti -----------For GSTIN-------------
                    proc.AddVarcharPara("@cmp_gstin", 15, GSTIN);
                    //------------------End Here

                    proc.AddVarcharPara("@result",50, "", QueryParameterDirection.Output);
                  
                  
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



        public void Insert_DummyAdress(string vcontacttype, string vInternalId)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {
                using (proc = new ProcedureExecute("AdressDummyInsert"))
                {

                    proc.AddVarcharPara("@contacttype", 50, vcontacttype);
                    proc.AddVarcharPara("@InternalId", 20, vInternalId);
                    int i = proc.RunActionQuery();
                
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
