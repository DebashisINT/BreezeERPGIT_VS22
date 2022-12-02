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
/// Summary description for frmReport_IframeGeneralTrialBL
namespace BusinessLogicLayer
{
    public class frmReport_IframeGeneralTrialBL
    {
        public DataSet Fetch_GeneralTrialAsOnDate(string ToDate, string Segment, string Company, string FinancialYr, string ZeroBal, string ActiveCurrency,
                                         string TradeCurrency, string branchList)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            DataSet ds = new DataSet();
            try
            {
                using (proc = new ProcedureExecute("Fetch_GeneralTrialAsOnDate"))
                {

                    proc.AddVarcharPara("@ToDate", 50, ToDate);
                    proc.AddVarcharPara("@Segment", -1, Segment);
                    proc.AddVarcharPara("@Company", 50, Company);
                    proc.AddVarcharPara("@FinancialYr", 20, FinancialYr);
                    proc.AddVarcharPara("@ZeroBal", 1, ZeroBal);
                    proc.AddVarcharPara("@ActiveCurrency", 3, ActiveCurrency);
                    proc.AddVarcharPara("@TradeCurrency", 3, TradeCurrency);
                    proc.AddVarcharPara("@BranchList", 3000, branchList);

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


        public DataSet Fetch_GeneralTrial(string FromDate, string ToDate, string Segment, string Company, string FinancialYr, string ZeroBal, string ActiveCurrency,
                                         string TradeCurrency, string branchList)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            DataSet ds = new DataSet();
            try
            {
                using (proc = new ProcedureExecute("Fetch_GeneralTrial"))
                {

                    proc.AddVarcharPara("@FromDate", 50, FromDate);
                    proc.AddVarcharPara("@ToDate", 50, ToDate);
                    proc.AddVarcharPara("@Segment", -1, Segment);
                    proc.AddVarcharPara("@Company", 50, Company);
                    proc.AddVarcharPara("@FinancialYr", 20, FinancialYr);
                    proc.AddVarcharPara("@ZeroBal", 1, ZeroBal);
                    proc.AddVarcharPara("@ActiveCurrency", 3, ActiveCurrency);
                    proc.AddVarcharPara("@TradeCurrency", 3, TradeCurrency);
                    proc.AddVarcharPara("@BranchList", 3000, branchList);


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
