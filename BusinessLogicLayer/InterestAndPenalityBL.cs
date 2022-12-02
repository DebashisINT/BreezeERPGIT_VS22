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
/// Summary description for InterestAndPenalityBL
/// </summary>

namespace BusinessLogicLayer
{

    public class InterestAndPenalityBL
    {

        public DataSet Processing_InterestPenaltyGenerationFecth(string FromDate, string ToDate, string MainAccount, string MainAccountForInterest,
                                      string MainAccountSubLedgerType, string GroupType, string GrpId, string Client, string SubAccount, string Segment,
                                      string CompanyId, string FinYear, string PreDefineRate, string Rate, string GracePeriod, string BalnType, string RptView,
                                      string ConsiderInAmnt, string Days, string CalculateOn
            )
        {
            ProcedureExecute proc;
            DataSet ds = new DataSet();

            string rtrnvalue = "";
            try
            {
                using (proc = new ProcedureExecute("Processing_InterestPenaltyGenerationFecth"))
                {

                    proc.AddVarcharPara("@FromDate", 100, FromDate);
                    proc.AddVarcharPara("@ToDate", 100, ToDate);
                    proc.AddVarcharPara("@MainAccount", 100, MainAccount);
                    proc.AddVarcharPara("@MainAccountForInterest", 100, MainAccountForInterest);
                    proc.AddVarcharPara("@MainAccountSubLedgerType", 100, MainAccountSubLedgerType);
                    proc.AddVarcharPara("@GroupType", 100, GroupType);
                    proc.AddVarcharPara("@GrpId", -1, GrpId);
                    proc.AddVarcharPara("@Client", -1, Client);
                    proc.AddVarcharPara("@SubAccount", -1, SubAccount);
                    proc.AddNVarcharPara("@Segment", 100, Segment);
                    proc.AddVarcharPara("@CompanyId", 100, CompanyId);
                    proc.AddVarcharPara("@FinYear", 100, FinYear);
                    proc.AddVarcharPara("@PreDefineRate", 100, PreDefineRate);

                    //proc.AddDecimalPara("@Rate", 28, 2, Convert.ToDecimal(Rate));
                    proc.AddIntegerPara("@Rate", Convert.ToInt32(Convert.ToDecimal(Rate)));
                    proc.AddIntegerPara("@GracePeriod", Convert.ToInt32(GracePeriod));
                    proc.AddVarcharPara("@BalnType", 100, BalnType);
                    proc.AddVarcharPara("@RptView", 100, RptView);
                    proc.AddIntegerPara("@ConsiderInAmnt", Convert.ToInt32( Convert.ToDecimal(ConsiderInAmnt)));
                  //  proc.AddDecimalPara("@ConsiderInAmnt", 28, 2, Convert.ToDecimal(ConsiderInAmnt));
                    proc.AddIntegerPara("@Days", Convert.ToInt32(Days));
                    proc.AddNVarcharPara("@CalculateOn", 100, CalculateOn);


                    ds = proc.GetDataSet();
                    return ds;

                    //    proc.AddVarcharPara("@result", 50, "", QueryParameterDirection.Output);


                    //int i = proc.RunActionQuery();
                    ////  rtrnvalue = proc.GetParaValue("@result").ToString();
                    //return i.ToString();


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



        public string Insert_PenaltyGeneration(string AllData, string CompanyId, string Segment, string FromDate,
            string ToDate, string Finyear, string CreateUser, string SrvTax, string Narration, string MainAccountSubLedgerType
              )
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {
                using (proc = new ProcedureExecute("Processing_InterestPenaltyGeneration"))
                {

                    proc.AddVarcharPara("@AllData", 100, AllData);
                    proc.AddVarcharPara("@CompanyId", 100, CompanyId);
                    proc.AddVarcharPara("@Segment", 100, Segment);
                    proc.AddVarcharPara("@FromDate", 100, FromDate);
                    proc.AddVarcharPara("@ToDate", 100, ToDate);
                    proc.AddVarcharPara("@Finyear", 100, Finyear);
                    proc.AddIntegerPara("@CreateUser", Convert.ToInt32(CreateUser));
                    proc.AddVarcharPara("@SrvTax", 100, SrvTax);
                    proc.AddVarcharPara("@Narration", 200, Narration);
                    proc.AddNVarcharPara("@MainAccountSubLedgerType", 100, MainAccountSubLedgerType);



                    //    proc.AddVarcharPara("@result", 50, "", QueryParameterDirection.Output);


                    int i = proc.RunActionQuery();
                    //  rtrnvalue = proc.GetParaValue("@result").ToString();
                    return i.ToString();


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



        public string Delete_InterestPenaltyGeneration(string AllData, string CompanyId, string Segment, string FromDate, string ToDate)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {
                using (proc = new ProcedureExecute("Delete_InterestPenaltyGeneration"))
                {

                    proc.AddVarcharPara("@AllData", 100, AllData);
                    proc.AddVarcharPara("@CompanyId", 100, CompanyId);
                    proc.AddIntegerPara("@Segment", Convert.ToInt32(Segment));
                    proc.AddVarcharPara("@FromDate", 100, FromDate);
                    proc.AddVarcharPara("@ToDate", 100, ToDate);


                    //    proc.AddVarcharPara("@result", 50, "", QueryParameterDirection.Output);


                    int i = proc.RunActionQuery();
                    //  rtrnvalue = proc.GetParaValue("@result").ToString();
                    return i.ToString();


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
