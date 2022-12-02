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
/// Summary description for ProfitLossStatementBL
namespace BusinessLogicLayer
{
    public class ProfitLossStatementBL
    {


        public DataSet Report_ProfitLossStatement(string FromDate, string ToDate, string CompanyId, string Segmentid, string Branch, string ReportView,
            string ReportStyle, string MonthlyBreakUp, string ZeroAmntAc, string ActiveCurrency, string TradeCurrency)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            DataSet ds = new DataSet();
            try
            {
                using (proc = new ProcedureExecute("Report_ProfitLossStatement"))
                {


                    proc.AddVarcharPara("@FromDate", 50, FromDate);
                    proc.AddVarcharPara("@ToDate", 50, ToDate);
                    proc.AddVarcharPara("@CompanyId", -1, CompanyId);
                    proc.AddVarcharPara("@Segmentid", 100, Segmentid);
                    proc.AddVarcharPara("@Branch", 50, Branch);
                    proc.AddVarcharPara("@ReportView", 100, ReportView);
                    proc.AddVarcharPara("@ReportStyle", 100, ReportStyle);
                    proc.AddVarcharPara("@MonthlyBreakUp", 100, MonthlyBreakUp);
                    proc.AddVarcharPara("@ZeroAmntAc", 100, ZeroAmntAc);
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
