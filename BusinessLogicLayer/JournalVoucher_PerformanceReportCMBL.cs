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
/// Summary description for JournalVoucher_PerformanceReportCMBL
namespace BusinessLogicLayer
{
    public class JournalVoucher_PerformanceReportCMBL
    {

        public DataSet PerformanceReportCM_JournalVoucher(string companyid, string segment, string fromdate, string todate, string clients, string Seriesid, string finyear,
                                      string grptype, string rpttype, string PRINTCHK, string clienttype, string closemethod, string jvcreate, string SEGMENTJV,
                                      string chkexcludesqr, string chkstt, string ValuationDate, string CreateUser, string ReportView)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            DataSet ds = new DataSet();
            try
            {
                using (proc = new ProcedureExecute("PerformanceReportCM_JournalVoucher"))
                {

                    proc.AddVarcharPara("@companyid", -1, companyid);
                    proc.AddVarcharPara("@segment", -1, segment);
                    proc.AddVarcharPara("@fromdate", 100, fromdate);
                    proc.AddVarcharPara("@todate", 100, todate);
                    proc.AddVarcharPara("@clients", -1, clients);
                    proc.AddVarcharPara("@Seriesid", -1, Seriesid);
                    proc.AddVarcharPara("@finyear", 100, finyear);
                    proc.AddVarcharPara("@grptype", 100, grptype);
                    proc.AddVarcharPara("@rpttype", 100, rpttype);
                    proc.AddVarcharPara("@PRINTCHK", 100, PRINTCHK);
                    proc.AddVarcharPara("@clienttype", 100, clienttype);


                    proc.AddVarcharPara("@closemethod", 100, closemethod);
                    proc.AddVarcharPara("@jvcreate", 100, jvcreate);
                    proc.AddVarcharPara("@SEGMENTJV", 100, SEGMENTJV);
                    proc.AddVarcharPara("@chkexcludesqr", 100, chkexcludesqr);

                    proc.AddVarcharPara("@chkstt", 100, chkstt);
                    proc.AddVarcharPara("@ValuationDate", 100, ValuationDate);

                    proc.AddVarcharPara("@CreateUser", 100, CreateUser);
                    proc.AddVarcharPara("@ReportView", 100, ReportView);

                    ds = proc.GetDataSet();
                    return ds;


                    //  proc.AddVarcharPara("@result", 50, "", QueryParameterDirection.Output);
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






        public DataSet xmlJournalVoucherInsert_Update(String journalData, String createuser, String finyear, String compID, String JournalVoucher_Narration,
                                                       String JournalVoucherDetail_TransactionDate, String JournalVoucher_SettlementNumber,
                                                       String JournalVoucher_SettlementType, String JournalVoucher_BillNumber,
                                                       String JournalVoucher_Prefix, String segmentid)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            DataSet ds = new DataSet();
            try
            {
                using (proc = new ProcedureExecute("xmlJournalVoucherInsert_Update"))
                {

                    proc.AddVarcharPara("@journalData", 100, journalData);
                    proc.AddVarcharPara("@createuser", 100, createuser);
                    proc.AddVarcharPara("@finyear", 100, finyear);
                    proc.AddVarcharPara("@compID", 100, compID);
                    proc.AddVarcharPara("@JournalVoucher_Narration", -1, JournalVoucher_Narration);
                    proc.AddDateTimePara("@JournalVoucherDetail_TransactionDate", Convert.ToDateTime(JournalVoucherDetail_TransactionDate));
                    proc.AddVarcharPara("@JournalVoucher_SettlementNumber", 100, JournalVoucher_SettlementNumber);
                    proc.AddVarcharPara("@JournalVoucher_SettlementType", 100, JournalVoucher_SettlementType);
                    proc.AddVarcharPara("@JournalVoucher_BillNumber", 100, JournalVoucher_BillNumber);
                    proc.AddVarcharPara("@JournalVoucher_Prefix", 100, JournalVoucher_Prefix);
                    proc.AddVarcharPara("@segmentid", 100, segmentid);

                    ds = proc.GetDataSet();
                    return ds;


                    //  proc.AddVarcharPara("@result", 50, "", QueryParameterDirection.Output);
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
