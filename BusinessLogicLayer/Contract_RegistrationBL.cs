using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer;
using System.Data;

namespace BusinessLogicLayer
{
  public  class Contract_RegistrationBL
    {

        //public void Insert_ContactRegistration(string vcrg_cntID,string vcrg_company1,string vcrg_exchange1,DateTime vcrg_regisDate,
        //                                 DateTime vcrg_businessCmmDate, DateTime vcrg_suspensionDate, string vcrg_reasonforSuspension,
        //                                 string vcrg_tcode,string vCreateUser,string vcrg_SubBrokerFranchiseeID,string vcrg_Dealer,
        //                                 DateTime vcrg_AccountClosureDate, string vcrg_FrontOfficeBranchCode, string vcrg_FrontOfficeGroupCode,
        //                                 string vcrg_ParticipantSchemeCode,string vcrg_ClearingBankCode,string vcrg_SchemeCode,string vcrg_STTPattern,
        //                                 string vSttWap,string vMapinSwapSebi,string vcrg_exchangesegment
        //                                   )
        //{
        //    ProcedureExecute proc;
        //    string rtrnvalue = "";
        //    try
        //    {
                


        //        using (proc = new ProcedureExecute("ContactInsert"))
        //        {

        //            proc.AddVarcharPara("@crg_cntID", 100, vcrg_cntID);
        //            proc.AddVarcharPara("@crg_company1", 100, vcrg_company1);
        //            proc.AddVarcharPara("@crg_exchange1",100, vcrg_exchange1);
        //            proc.AddDateTimePara("@crg_regisDate",  vcrg_regisDate);
        //            proc.AddDateTimePara("@crg_businessCmmDate",  vcrg_businessCmmDate);
        //            proc.AddDateTimePara("@crg_suspensionDate",  vcrg_suspensionDate);
        //            proc.AddVarcharPara("@crg_reasonforSuspension", 2000, vcrg_reasonforSuspension);
        //            proc.AddVarcharPara("@crg_tcode",100,vcrg_tcode);
        //            proc.AddIntegerPara("@CreateUser", Convert.ToInt32(vCreateUser));
        //            proc.AddVarcharPara("@crg_SubBrokerFranchiseeID",100,vcrg_SubBrokerFranchiseeID);
        //            proc.AddVarcharPara("@crg_Dealer",100, vcrg_Dealer);
        //            proc.AddDateTimePara("@crg_AccountClosureDate", vcrg_AccountClosureDate);
        //            proc.AddVarcharPara("@crg_FrontOfficeBranchCode",50,vcrg_FrontOfficeBranchCode);
        //            proc.AddVarcharPara("@crg_FrontOfficeGroupCode",50, vcrg_FrontOfficeGroupCode);
        //            proc.AddVarcharPara("@crg_ParticipantSchemeCode",50,vcrg_ParticipantSchemeCode);
        //            proc.AddVarcharPara("@crg_ClearingBankCode", 50, vcrg_ClearingBankCode);
        //            proc.AddVarcharPara("@crg_SchemeCode", 50,vcrg_SchemeCode);
        //            proc.AddVarcharPara("@crg_STTPattern",50,vcrg_STTPattern);
        //            proc.AddVarcharPara("@SttWap",50,vSttWap);
        //            proc.AddVarcharPara("@MapinSwapSebi", 10, vMapinSwapSebi);
        //            proc.AddIntegerPara("@crg_exchangesegment",Convert.ToInt32(vcrg_exchangesegment));
                   
        //           // proc.AddVarcharPara("@result", 50, "", QueryParameterDirection.Output);

                  
        //            int i = proc.RunActionQuery();
        //            //rtrnvalue = proc.GetParaValue("@result").ToString();
        //            //return rtrnvalue;


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


        //public void Update_ContactRegistration(string vcrg_internalId, string vcrg_cntID, string vcrg_company1, string vcrg_exchange1, DateTime vcrg_regisDate,
        //                                 DateTime vcrg_businessCmmDate, DateTime vcrg_suspensionDate, string vcrg_reasonforSuspension,
        //                                 string vcrg_tcode, string vCreateUser, string vcrg_SubBrokerFranchiseeID, string vcrg_Dealer,
        //                                 DateTime vcrg_AccountClosureDate, string vcrg_FrontOfficeBranchCode, string vcrg_FrontOfficeGroupCode,
        //                                 string vcrg_ParticipantSchemeCode, string vcrg_ClearingBankCode, string vcrg_SchemeCode, string vcrg_STTPattern,
        //                                 string vSttWap, string vMapinSwapSebi, string vcrg_exchangesegment
        //                                   )
        //{
        //    ProcedureExecute proc;
        //    string rtrnvalue = "";
        //    try
        //    {

        //        using (proc = new ProcedureExecute("updateContactExchange"))
        //        {

        //            proc.AddVarcharPara("@crg_internalId", 100, vcrg_internalId);
        //            proc.AddVarcharPara("@crg_cntID", 100, vcrg_cntID);
        //            proc.AddVarcharPara("@crg_company1", 100, vcrg_company1);
        //            proc.AddVarcharPara("@crg_exchange1", 100, vcrg_exchange1);
        //            proc.AddDateTimePara("@crg_regisDate", vcrg_regisDate);
        //            proc.AddDateTimePara("@crg_businessCmmDate", vcrg_businessCmmDate);
        //            proc.AddDateTimePara("@crg_suspensionDate", vcrg_suspensionDate);
        //            proc.AddVarcharPara("@crg_reasonforSuspension", 2000, vcrg_reasonforSuspension);
        //            proc.AddVarcharPara("@crg_tcode", 100, vcrg_tcode);
        //            proc.AddIntegerPara("@CreateUser", Convert.ToInt32(vCreateUser));
        //            proc.AddVarcharPara("@crg_SubBrokerFranchiseeID", 100, vcrg_SubBrokerFranchiseeID);
        //            proc.AddVarcharPara("@crg_Dealer", 100, vcrg_Dealer);
        //            proc.AddDateTimePara("@crg_AccountClosureDate", vcrg_AccountClosureDate);
        //            proc.AddVarcharPara("@crg_FrontOfficeBranchCode", 50, vcrg_FrontOfficeBranchCode);
        //            proc.AddVarcharPara("@crg_FrontOfficeGroupCode", 50, vcrg_FrontOfficeGroupCode);
        //            proc.AddVarcharPara("@crg_ParticipantSchemeCode", 50, vcrg_ParticipantSchemeCode);
        //            proc.AddVarcharPara("@crg_ClearingBankCode", 50, vcrg_ClearingBankCode);
        //            proc.AddVarcharPara("@crg_SchemeCode", 50, vcrg_SchemeCode);
        //            proc.AddVarcharPara("@crg_STTPattern", 50, vcrg_STTPattern);
        //            proc.AddVarcharPara("@SttWap", 50, vSttWap);
        //            proc.AddVarcharPara("@MapinSwapSebi", 10, vMapinSwapSebi);
        //            proc.AddIntegerPara("@crg_exchangesegment", Convert.ToInt32(vcrg_exchangesegment));

        //            // proc.AddVarcharPara("@result", 50, "", QueryParameterDirection.Output);





        //            int i = proc.RunActionQuery();
        //            //rtrnvalue = proc.GetParaValue("@result").ToString();
        //            //return rtrnvalue;


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

      public Boolean CheckUniqueRegDocNumber(string customerid, string CrgNumber,int mode )
      {
          Boolean isPresent = false;
          DataTable dt = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("Prc_ContactGeneral_Details");
          proc.AddVarcharPara("@Action", 50, "CheckUniqueCrgNumber");
          proc.AddVarcharPara("@CustomerID", 100, customerid);
          proc.AddVarcharPara("@CrgNumber", 50, CrgNumber);
          proc.AddIntegerPara("@Mode",  mode);
          dt = proc.GetTable();
          if (dt != null)
          {
              if (dt.Rows.Count > 0)
              {
                  isPresent = true;
              }
          }

          return isPresent;
      }

      public Boolean CheckUniqueReginfluencerDocNumber(string customerid, string CrgNumber, int mode)
      {
          Boolean isPresent = false;
          DataTable dt = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("Prc_ContactGeneral_Details");
          proc.AddVarcharPara("@Action", 50, "CheckUniqueCrgInfluencerNumber");
          proc.AddVarcharPara("@CustomerID", 100, customerid);
          proc.AddVarcharPara("@CrgNumber", 50, CrgNumber);
          proc.AddIntegerPara("@Mode", mode);
          dt = proc.GetTable();
          if (dt != null)
          {
              if (dt.Rows.Count > 0)
              {
                  isPresent = true;
              }
          }

          return isPresent;
      }
      public Boolean CheckUniqueRegCustomerDocNumber(string customerid, string CrgNumber, int mode)
      {
          Boolean isPresent = false;
          DataTable dt = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("Prc_ContactGeneral_Details");
          proc.AddVarcharPara("@Action", 50, "CheckUniqueCrgCustomerNumber");
          proc.AddVarcharPara("@CustomerID", 100, customerid);
          proc.AddVarcharPara("@CrgNumber", 50, CrgNumber);
          proc.AddIntegerPara("@Mode", mode);
          dt = proc.GetTable();
          if (dt != null)
          {
              if (dt.Rows.Count > 0)
              {
                  isPresent = true;
              }
          }

          return isPresent;
      }
      public string CheckUniqueRegDocNumberRetCustomerName(string customerid, string CrgNumber, int mode)
      {
          string CusName = string.Empty;
          //Boolean isPresent = false;
          DataTable dt = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("Prc_ContactGeneral_Details");
          proc.AddVarcharPara("@Action", 50, "CheckUniqueCrgNumberRetCustomername");
          proc.AddVarcharPara("@CustomerID", 100, customerid);
          proc.AddVarcharPara("@CrgNumber", 50, CrgNumber);
          proc.AddIntegerPara("@Mode", mode);
          dt = proc.GetTable();
          if (dt != null)
          {
              if (dt.Rows.Count > 0)
              {
                  CusName = Convert.ToString(dt.Rows[0]["CusName"]);
              }
          }

          return CusName;
      }
      public string CheckUniqueRegDocNumberRetInfluencerName(string customerid, string CrgNumber, int mode)
      {
          string CusName = string.Empty;
          //Boolean isPresent = false;
          DataTable dt = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("Prc_ContactGeneral_Details");
          proc.AddVarcharPara("@Action", 50, "CheckUniqueCrgNumberRetInfluencername");
          proc.AddVarcharPara("@CustomerID", 100, customerid);
          proc.AddVarcharPara("@CrgNumber", 50, CrgNumber);
          proc.AddIntegerPara("@Mode", mode);
          dt = proc.GetTable();
          if (dt != null)
          {
              if (dt.Rows.Count > 0)
              {
                  CusName = Convert.ToString(dt.Rows[0]["CusName"]);
              }
          }

          return CusName;
      }

      public string SettingsCheckUniqueRegDocNumberRetCustomerName(string customerid, string CrgNumber, int mode)
      {
          string CusName = string.Empty;
          //Boolean isPresent = false;
          DataTable dt = new DataTable();
          ProcedureExecute proc = new ProcedureExecute("Prc_ContactGeneral_Details");
          proc.AddVarcharPara("@Action", 50, "SettingsCheckUniqueCrgNumberRetCustomername");
          proc.AddVarcharPara("@CustomerID", 100, customerid);
          proc.AddVarcharPara("@CrgNumber", 50, CrgNumber);
          proc.AddIntegerPara("@Mode", mode);
          dt = proc.GetTable();
          if (dt != null)
          {
              if (dt.Rows.Count > 0)
              {
                  CusName = Convert.ToString(dt.Rows[0]["CusName"]);
              }
          }

          return CusName;
      }


    }
}
