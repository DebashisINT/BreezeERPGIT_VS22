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
/// Summary description for frm_ReportBalanceSheetBL
/// </summary>
public class frm_ReportBalanceSheetBL
{
    public DataSet REPORT_BALANCESHEET( string SEGMENT,string COMPANYID,string ASONDATE,string BRANCHHIERCHY,string FINYEAR,string RPTTYPE,
                                        string ActiveCurrency, string TradeCurrency)
    {
        ProcedureExecute proc;
        string rtrnvalue = "";
        DataSet ds = new DataSet();
        try
        {
            using (proc = new ProcedureExecute("REPORT_BALANCESHEET"))
            {

                proc.AddVarcharPara("@SEGMENT", -1, SEGMENT);
                proc.AddVarcharPara("@COMPANYID", 50, COMPANYID);
                proc.AddDateTimePara("@ASONDATE", Convert.ToDateTime(ASONDATE));
                proc.AddVarcharPara("@BRANCHHIERCHY", 5000, BRANCHHIERCHY);
                proc.AddVarcharPara("@FINYEAR", 20, FINYEAR);
                proc.AddVarcharPara("@RPTTYPE", 10, RPTTYPE);
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
