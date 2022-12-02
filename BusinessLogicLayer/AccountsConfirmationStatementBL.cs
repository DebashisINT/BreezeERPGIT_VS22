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
/// Summary description for AccountsConfirmationStatementBL
namespace BusinessLogicLayer
{
    public class AccountsConfirmationStatementBL
    {


        public DataSet Fetch_AccountsConfirmation(string companyID, string segmentID, string DateFrom, string DateTo, string SubAccountSearch,
                                               string FinYear, string SingleDouble, string Header, string Footer, string SubAcID, string ExchangeSegment
                                              , string groupType, string orderType, string StatementDate, string MainAccID)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            DataSet ds = new DataSet();
            try
            {
                using (proc = new ProcedureExecute("Fetch_AccountsConfirmation"))
                {


                    proc.AddVarcharPara("@companyID", 50, companyID);
                    proc.AddVarcharPara("@segmentID", 100, segmentID);
                    proc.AddDateTimePara("@DateFrom", Convert.ToDateTime(DateFrom));

                    proc.AddDateTimePara("@DateTo", Convert.ToDateTime(DateTo));
                    proc.AddVarcharPara("@SubAccountSearch", -1, SubAccountSearch);
                    proc.AddVarcharPara("@FinYear", 100, FinYear);
                    proc.AddVarcharPara("@SingleDouble", 100, SingleDouble);

                    proc.AddVarcharPara("@Header", 100, Header);
                    proc.AddVarcharPara("@Footer", 100, Footer);
                    proc.AddVarcharPara("@SubAcID", -1, SubAcID);
                    proc.AddVarcharPara("@ExchangeSegment", 100, ExchangeSegment);

                    proc.AddVarcharPara("@groupType", 100, groupType);
                    proc.AddVarcharPara("@orderType", 100, orderType);
                    proc.AddDateTimePara("@StatementDate", Convert.ToDateTime(StatementDate));
                    proc.AddVarcharPara("@MainAccID", 100, MainAccID);



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
