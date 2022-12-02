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
/// Summary description for frmReport_IFrameCashBankBookBL
namespace BusinessLogicLayer
{
    public class frmReport_IFrameCashBankBookBL
    {
        public DataSet Fetch_CashBankBook(string MainAccountID, string FromDate, string ToDate, string Segment, string FinYear, string Company, string RedirectTo,
                                      string BranchID, string ActiveCurrency, string TradeCurrency)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            DataSet ds = new DataSet();
            try
            {
                using (proc = new ProcedureExecute("Fetch_CashBankBook"))
                {

                    proc.AddVarcharPara("@MainAccountID", 100, MainAccountID);
                    proc.AddVarcharPara("@FromDate", 100, Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd"));
                    proc.AddVarcharPara("@ToDate", 100, Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd"));
                    proc.AddVarcharPara("@Segment", 5000, Segment);
                    proc.AddVarcharPara("@Company", 100, Company);
                    proc.AddVarcharPara("@FinYear", 100, FinYear);
                    proc.AddVarcharPara("@RedirectTo", 100, RedirectTo);
                    proc.AddVarcharPara("@BranchID", -1, BranchID);
                    proc.AddVarcharPara("@ActiveCurrency", 100, ActiveCurrency);
                    proc.AddVarcharPara("@TradeCurrency", 100, TradeCurrency);



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
