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
/// Summary description for FrmReport_MonthWiseTrialBalanceBL
namespace BusinessLogicLayer
{
    public class FrmReport_MonthWiseTrialBalanceBL
    {

        public DataTable Fetch_GeneralTrial(string FinYear)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            DataSet ds = new DataSet();
            try
            {
                using (proc = new ProcedureExecute("Fetch_MonthYearCurrentFinyear"))
                {

                    proc.AddVarcharPara("@FinYear", 100, FinYear);

                    ds = proc.GetDataSet();

                    return ds.Tables[0];


                    //    proc.AddVarcharPara("@result", 50, "", QueryParameterDirection.Output);
                    //int i = proc.RunActionQuery();
                    ////  rtrnvalue = proc.GetParaValue("@result").ToString();
                    //return i;

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

        public DataSet Fetch_MonthWiseTrialBalance(string CompanyID, string FinYear, string FromInd, string ToInd, string MainAccount, string SubAccount,
                                                 string Branch, string ReportType, string Segment, string Group, string BranchGroutType, string GroupType,
                                                  string SubledgerType, string ZeroBal, string ActiveCurrency, string TradeCurrency)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            DataSet ds = new DataSet();
            try
            {
                using (proc = new ProcedureExecute("Fetch_MonthWiseTrialBalance"))
                {

                    proc.AddVarcharPara("@CompanyID", 100, CompanyID);
                    proc.AddVarcharPara("@FinYear", 100, FinYear);
                    proc.AddIntegerPara("@FromInd", Convert.ToInt32(FromInd));
                    proc.AddIntegerPara("@ToInd", Convert.ToInt32(ToInd));
                    proc.AddVarcharPara("@MainAccount", -1, MainAccount);
                    proc.AddVarcharPara("@SubAccount", -1, SubAccount);
                    proc.AddVarcharPara("@Branch", -1, Branch);
                    proc.AddVarcharPara("@ReportType", 100, ReportType);
                    proc.AddVarcharPara("@Segment", -1, Segment);
                    proc.AddVarcharPara("@Group", -1, Group);
                    proc.AddVarcharPara("@BranchGroutType", 100, BranchGroutType);
                    proc.AddVarcharPara("@GroupType", 100, GroupType);
                    proc.AddVarcharPara("@SubledgerType", 250, SubledgerType);
                    proc.AddVarcharPara("@ZeroBal", 100, ZeroBal);
                    proc.AddVarcharPara("@ActiveCurrency", 3, ActiveCurrency);
                    proc.AddVarcharPara("@TradeCurrency", 3, TradeCurrency);




                    //----------------------------------------------------



                    ds = proc.GetDataSet();
                    return ds;


                    //    proc.AddVarcharPara("@result", 50, "", QueryParameterDirection.Output);
                    //int i = proc.RunActionQuery();
                    ////  rtrnvalue = proc.GetParaValue("@result").ToString();
                    //return i;

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





        public DataSet Fetch_SubSideryTrial(String MainAccountID, String SubAccount, String Branch, String Segment, String FinancialYr, String Company,
                                             String DrCrAmt, String Group, String ReportType, String BranchGroutType, String GroupType, String ShowStatus,
                                             String ZeroBal, String ActiveCurrency, String TradeCurrency)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            DataSet ds = new DataSet();
            try
            {
                using (proc = new ProcedureExecute("Fetch_SubSideryTrial"))
                {


                    proc.AddVarcharPara("@MainAccountID", -1, MainAccountID);
                    proc.AddVarcharPara("@SubAccount", -1, SubAccount);
                    proc.AddVarcharPara("@Branch", -1, Branch);
                    //proc.AddVarcharPara("@FromDate", 100, "");
                    //proc.AddVarcharPara("@ToDate", 100, "");

                    proc.AddVarcharPara("@Segment", -1, Segment);
                    proc.AddVarcharPara("@FinancialYr", 50, FinancialYr);
                    proc.AddVarcharPara("@Company", 100, Company);

                    proc.AddVarcharPara("@DrCrAmt", 100, DrCrAmt);
                    proc.AddVarcharPara("@Group", -1, Group);
                    proc.AddVarcharPara("@ReportType", 100, ReportType);


                    proc.AddVarcharPara("@BranchGroutType", 5000, BranchGroutType);
                    proc.AddVarcharPara("@GroupType", 100, GroupType);
                    proc.AddVarcharPara("@ShowStatus", 100, ShowStatus);
                    proc.AddVarcharPara("@ZeroBal", 100, ZeroBal);
                    proc.AddVarcharPara("@ActiveCurrency", 3, ActiveCurrency);
                    proc.AddVarcharPara("@TradeCurrency", 3, TradeCurrency);


                    ds = proc.GetDataSet();
                    return ds;


                    //    proc.AddVarcharPara("@result", 50, "", QueryParameterDirection.Output);
                    //int i = proc.RunActionQuery();
                    ////  rtrnvalue = proc.GetParaValue("@result").ToString();
                    //return i;

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
