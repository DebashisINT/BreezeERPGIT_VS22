using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DataAccessLayer;
using System.Web;
using EntityLayer;

namespace BusinessLogicLayer
{
    
   public class ContactGeneralBL
    {

      

       //public string Insert_ContactGeneral(string vcontacttype, string vcnt_ucc, string vcnt_salutation, string vcnt_firstName, 
       //                                     string vcnt_middleName, string vcnt_lastName,string vcnt_shortName, string vcnt_branchId, string vcnt_sex,
       //                                     string vcnt_maritalStatus, DateTime vcnt_DOB, DateTime vcnt_anniversaryDate, string vcnt_legalStatus, 
       //                                     string vcnt_education, string vcnt_profession, string vcnt_organization, string vcnt_jobResponsibility,
       //                                     string vcnt_designation, string vcnt_industry, string vcnt_contactSource, string vcnt_referedBy, 
       //                                     string vRPartner, string vcnt_contactType, string vcnt_contactStatus,
       //                                     DateTime vcnt_RegistrationDate, string vcnt_rating, string vcnt_reason, string vbloodgroup, string vWebLogIn,
       //                                     string vPassWord, string vlastModifyUser, string vplaceincorporation, DateTime vdatebusscommence, 
       //                                     string votheroccu, string vcountry
       //                                     )
          


       //{
       //    ProcedureExecute proc;
       //    string rtrnvalue = "";
       //    try
       //    {


       //        using (proc = new ProcedureExecute("ContactInsert"))
       //        {

       //            proc.AddVarcharPara("@contacttype", 100, vcontacttype);
       //            proc.AddVarcharPara("@cnt_ucc", 100, vcnt_ucc);
       //            proc.AddIntegerPara("@cnt_salutation", Convert.ToInt32(vcnt_salutation));
       //            proc.AddVarcharPara("@cnt_firstName", 100, vcnt_firstName);
       //            proc.AddVarcharPara("@cnt_middleName", 100, vcnt_middleName);
       //            proc.AddVarcharPara("@cnt_lastName", 100, vcnt_lastName);
       //            proc.AddVarcharPara("@cnt_shortName", 100, vcnt_shortName);
       //            proc.AddIntegerPara("@cnt_branchId", Convert.ToInt32(vcnt_branchId));
       //            proc.AddIntegerPara("@cnt_sex", Convert.ToInt32(vcnt_sex));
       //            proc.AddIntegerPara("@cnt_maritalStatus", Convert.ToInt32(vcnt_maritalStatus));
                  
       //            proc.AddDateTimePara("@cnt_DOB", vcnt_DOB);
                  
       //            proc.AddDateTimePara("@cnt_anniversaryDate", vcnt_anniversaryDate);
                   
       //            proc.AddIntegerPara("@cnt_legalStatus", Convert.ToInt32(vcnt_legalStatus));
       //            proc.AddIntegerPara("@cnt_education", Convert.ToInt32(vcnt_education));
       //            proc.AddIntegerPara("@cnt_profession", Convert.ToInt32(vcnt_profession));
       //            proc.AddVarcharPara("@cnt_organization", 100, vcnt_organization);
       //            proc.AddIntegerPara("@cnt_jobResponsibility", Convert.ToInt32(vcnt_jobResponsibility));
       //            proc.AddIntegerPara("@cnt_designation", Convert.ToInt32(vcnt_designation));
       //            proc.AddIntegerPara("@cnt_industry", Convert.ToInt32(vcnt_industry));
       //            proc.AddIntegerPara("@cnt_contactSource", Convert.ToInt32(vcnt_contactSource));
       //            proc.AddVarcharPara("@cnt_referedBy", 100, vcnt_referedBy);
       //            proc.AddVarcharPara("@RPartner", 100, vRPartner);
       //            proc.AddVarcharPara("@cnt_contactType", 100, vcnt_contactType);
       //            proc.AddIntegerPara("@cnt_contactStatus", Convert.ToInt32(vcnt_contactStatus));
                 
       //            proc.AddDateTimePara("@cnt_RegistrationDate", vcnt_RegistrationDate);
                  
       //            proc.AddIntegerPara("@cnt_rating", Convert.ToInt32(vcnt_rating));
       //            proc.AddVarcharPara("@cnt_reason", 400, vcnt_reason);
       //            proc.AddVarcharPara("@bloodgroup", 100, vbloodgroup);
       //            proc.AddVarcharPara("@WebLogIn", 100, vWebLogIn);
       //            proc.AddVarcharPara("@PassWord", 100, vPassWord);
       //            proc.AddVarcharPara("@lastModifyUser", 100, vlastModifyUser);
       //            proc.AddVarcharPara("@placeincorporation", 100, vplaceincorporation);
                  
       //            proc.AddDateTimePara("@datebusscommence", vdatebusscommence);
                  
       //            proc.AddVarcharPara("@otheroccu", 100, votheroccu);
       //            proc.AddIntegerPara("@country", Convert.ToInt32(vcountry));
       //            proc.AddVarcharPara("@result", 50, "", QueryParameterDirection.Output);

                 

                 

       //            int i = proc.RunActionQuery();
       //            rtrnvalue = proc.GetParaValue("@result").ToString();
       //            return rtrnvalue;


       //        }
       //    }

       //    catch (Exception ex)
       //    {
       //        throw ex;
       //    }

       //    finally
       //    {
       //        proc = null;
       //    }
       //}



       //public string Get_UCCCode(string vUCC)
       //{
       //    ProcedureExecute proc;
       //    string rtrnvalue = "";
       //    try
       //    {
       //        using (proc = new ProcedureExecute("sp_GenerateContactUCC"))
       //        {

       //            proc.AddVarcharPara("@UCC", 50, vUCC);
       //            proc.AddVarcharPara("@result", 50, "", QueryParameterDirection.Output);
                 
       //            int i = proc.RunActionQuery();
       //            rtrnvalue = proc.GetParaValue("@result").ToString();
       //            return rtrnvalue;

       //        }
       //    }

       //    catch (Exception ex)
       //    {
       //        throw ex;
       //    }

       //    finally
       //    {
       //        proc = null;
       //    }
       //}


       //public void Insert_DummyAdress(string vcontacttype, string vInternalId)
       //{
       //    ProcedureExecute proc;
       //    string rtrnvalue = "";
       //    try
       //    {
       //        using (proc = new ProcedureExecute("AdressDummyInsert"))
       //        {

       //            proc.AddVarcharPara("@contacttype", 50, vcontacttype);
       //            proc.AddVarcharPara("@InternalId", 20, vInternalId);
       //            int i = proc.RunActionQuery();

       //        }
       //    }

       //    catch (Exception ex)
       //    {
       //        throw ex;
       //    }

       //    finally
       //    {
       //        proc = null;
       //    }
       //}
       public DataTable CheckUniqueGSTIN(string customerid, int mode, string gstin)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("Prc_ContactGeneral_Details");
           proc.AddVarcharPara("@Action", 500, "CheckUniqueGSTIN");
           proc.AddVarcharPara("@CustomerID", 500, customerid);
           proc.AddIntegerPara("@Mode", mode);
           proc.AddVarcharPara("@GSTIN", 500, gstin);
           
           dt = proc.GetTable();
           return dt;
       }

       public static DataTable CheckUniqueGSTINByUserType(string UserId, int mode, string gstin, string UserType)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("prc_CheckUniqueGSTINByUserType");
           proc.AddVarcharPara("@Action", 500, "CheckUniqueGSTIN");
           proc.AddVarcharPara("@UserId", 20, UserId);
           proc.AddIntegerPara("@Mode", mode);
           proc.AddVarcharPara("@GSTIN", 100, gstin);
           proc.AddVarcharPara("@UserType", 10, UserType);
           dt = proc.GetTable();
           return dt;
       }
       public static bool ISUniqueGSTIN(string customerid, string mode, string gstin, string ContType)
       {
           bool flag = false;
           BusinessLogicLayer.GenericMethod oGenericMethod = new BusinessLogicLayer.GenericMethod();
           ContactGeneralBL objContactGeneralBL = new ContactGeneralBL();
           try
           {

               DataTable dt = new DataTable();
               dt = CheckUniqueGSTINByUserType(customerid, Convert.ToInt32(mode), gstin, ContType);

               int cnt = 0;
               if (dt != null && dt.Rows.Count > 0)
               {
                   cnt = dt.Rows.Count;
               }


               if (cnt > 0)
               {
                   flag = true;
               }
           }
           catch (Exception ex)
           {

           }
           finally
           {
           }
           return flag;
       }
   }
}
