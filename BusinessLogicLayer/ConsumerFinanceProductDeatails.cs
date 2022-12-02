using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using DataAccessLayer;

/// <summary>
/// Summary description for ConsumerFinanceProductDeatails
/// </summary>
/// 
namespace BusinessLogicLayer
{
    public class ConsumerFinanceProductDeatails
    {
        public ConsumerFinanceProductDeatails()
        {
            //
            // TODO: Add constructor logic here
            //
        }




        public string Insert_ConsumerFinanceProduct(string cf_ptype, string cf_pname, string cf_conCode, string cf_proFeature, string cf_loanCurrency, string cf_appliMinAge_Sal,
                                      string cf_appliMaxAge_Sal, string cf_appliMinAge_Self, string cf_appliMaxAge_Self, string cf_annualIncome_Sal,
                                      string cf_annualIncome_Self, string cf_MinloanAmount, string cf_MaxLoanAmount,
                                      string cf_Minloantenuare, string cf_Maxloantenure, string cf_serviceContinuity, string cf_residenceStab,
                                      string cf_loanValue, string cf_validitySanction, string cf_moderepayment, string cf_partrepayment,
                                      string cf_noReference, string cf_fixedIncomeRatio, string CreateUser
              )
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {


                using (proc = new ProcedureExecute("ConsumerFinanceProductInsert"))
                {


                    proc.AddVarcharPara("@cf_ptype", 20, cf_ptype);
                    proc.AddVarcharPara("@cf_pname", 300, cf_pname);
                    proc.AddVarcharPara("@cf_conCode", 50, cf_conCode);
                    proc.AddVarcharPara("@cf_proFeature", 2000, cf_proFeature);
                    proc.AddVarcharPara("@cf_loanCurrency", 20, cf_loanCurrency);

                    proc.AddIntegerPara("@cf_appliMinAge_Sal", Convert.ToInt32(cf_appliMinAge_Sal));
                    proc.AddIntegerPara("@cf_appliMaxAge_Sal", Convert.ToInt32(cf_appliMaxAge_Sal));
                    proc.AddIntegerPara("@cf_appliMinAge_Self", Convert.ToInt32(cf_appliMinAge_Self));
                    proc.AddIntegerPara("@cf_appliMaxAge_Self", Convert.ToInt32(cf_appliMaxAge_Self));

                    proc.AddDecimalPara("@cf_annualIncome_Sal", 3, 10, Convert.ToDecimal(cf_annualIncome_Sal));
                    proc.AddDecimalPara("@cf_annualIncome_Self", 3, 10, Convert.ToDecimal(cf_annualIncome_Self));
                    proc.AddDecimalPara("@cf_MinloanAmount",  3, 10,Convert.ToDecimal(cf_MinloanAmount));
                    proc.AddDecimalPara("@cf_MaxLoanAmount",  3,10, Convert.ToDecimal(cf_MaxLoanAmount));
                    proc.AddDecimalPara("@cf_Minloantenuare",  3,10, Convert.ToDecimal(cf_Minloantenuare));
                    proc.AddDecimalPara("@cf_Maxloantenure",  3,10, Convert.ToDecimal(cf_Maxloantenure));
                    proc.AddDecimalPara("@cf_serviceContinuity",  3,10, Convert.ToDecimal(cf_serviceContinuity));
                    proc.AddDecimalPara("@cf_residenceStab", 3, 10, Convert.ToDecimal(cf_residenceStab));
                    proc.AddDecimalPara("@cf_loanValue",  3,10, Convert.ToDecimal(cf_loanValue));
                    proc.AddDecimalPara("@cf_validitySanction",  3,10, Convert.ToDecimal(cf_validitySanction));


                    proc.AddVarcharPara("@cf_moderepayment", 20, cf_moderepayment);
                    proc.AddVarcharPara("@cf_partrepayment", 2000, cf_partrepayment);
                    proc.AddDecimalPara("@cf_noReference",  3,10, Convert.ToDecimal(cf_noReference));
                    proc.AddVarcharPara("@cf_fixedIncomeRatio", 50, cf_fixedIncomeRatio);
                    proc.AddDecimalPara("@CreateUser", 3,10,  Convert.ToDecimal(CreateUser));
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
