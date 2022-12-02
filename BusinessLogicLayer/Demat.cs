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
/// Summary description for Demat
/// </summary>

namespace BusinessLogicLayer
{
    public class Demat
    {
        public Demat()
        {
            //
            // TODO: Add constructor logic here
            //
        }



        public string Insert_DematPositions(string DematTransactions_FinYear, string DematTransactions_CompanyID, string DematTransactions_SegmentID,
                                           string DematTransactions_CustomerID, string DematTransactions_BranchID, string DematTransactions_ProductSeriesID,
                                           string DematTransactions_ISIN, string DematTransactions_Type, string DematTransactions_SettlementNumberT,
                                           string DematTransactions_SettlementTypeT, string DematTransactions_Quantity, string DematTransactions_Remarks,
                                           string DematTransactions_GenerationType, string DematTransactions_GenerateUser,
                                           string DematTransactions_IsAuthorized, string DematTransactions_OwnAccountT, string DematTransactions_Date,
                                           string DematTransactions_CreateUser
                                             )
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {


                using (proc = new ProcedureExecute("sp_Update_DematPositions"))
                {

                    proc.AddVarcharPara("@DematTransactions_FinYear", 100, DematTransactions_FinYear);
                    proc.AddVarcharPara("@DematTransactions_CompanyID", 100, DematTransactions_CompanyID);
                    proc.AddIntegerPara("@DematTransactions_SegmentID", Convert.ToInt32(DematTransactions_SegmentID));
                    proc.AddVarcharPara("@DematTransactions_CustomerID", 100, DematTransactions_CustomerID);
                    proc.AddIntegerPara("@DematTransactions_BranchID", Convert.ToInt32(DematTransactions_BranchID));
                    proc.AddBigIntegerPara("@DematTransactions_ProductSeriesID", Convert.ToInt64(DematTransactions_ProductSeriesID));
                    proc.AddVarcharPara("@DematTransactions_ISIN", 100, DematTransactions_ISIN);
                    proc.AddVarcharPara("@DematTransactions_Type", 100, DematTransactions_Type);
                    proc.AddVarcharPara("@DematTransactions_SettlementNumberT", 100, DematTransactions_SettlementNumberT);
                    proc.AddVarcharPara("@DematTransactions_SettlementTypeT", 100, DematTransactions_SettlementTypeT);
                    proc.AddIntegerPara("@DematTransactions_Quantity", Convert.ToInt32(DematTransactions_Quantity));
                    proc.AddVarcharPara("@DematTransactions_Remarks", 100, DematTransactions_Remarks);
                    proc.AddVarcharPara("@DematTransactions_GenerationType", 100, DematTransactions_GenerationType);
                    proc.AddIntegerPara("@DematTransactions_GenerateUser", Convert.ToInt32(DematTransactions_GenerateUser));
                    proc.AddVarcharPara("@DematTransactions_IsAuthorized", 100, DematTransactions_IsAuthorized);
                    proc.AddBigIntegerPara("@DematTransactions_OwnAccountT", Convert.ToInt64(DematTransactions_OwnAccountT));
                    proc.AddDateTimePara("@DematTransactions_Date", Convert.ToDateTime(DematTransactions_Date));
                    proc.AddIntegerPara("@DematTransactions_CreateUser", Convert.ToInt32(DematTransactions_CreateUser));
                    proc.AddBigIntegerPara("@result", 16, QueryParameterDirection.Output);

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


        public void Update_DematPositions(string userid, string finyear, string compid, string segid, string postype, string sett_num, string sett_type,
                                             string singleRecord, string actionFlag, string accid, string custid, string branchid,
                                             string prodseriesid, string isin, string qty, string old_sett_num, string old_sett_type, string old_custid,
                                             string old_prodseriesid, string old_isin, string old_qty, string old_accid, string TransactionID
                                             )
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {



                using (proc = new ProcedureExecute("sp_Update_DematPositions"))
                {

                    proc.AddVarcharPara("@userid", 100, userid);
                    proc.AddVarcharPara("@finyear", 100, finyear);
                    proc.AddVarcharPara("@compid", 100, compid);
                    proc.AddVarcharPara("@segid", 100, segid);
                    proc.AddVarcharPara("@postype", 100, postype);
                    proc.AddVarcharPara("@sett_num", 100, sett_num);
                    proc.AddVarcharPara("@sett_type", 100, sett_type);
                    proc.AddVarcharPara("@singleRecord", 100, singleRecord);
                    proc.AddVarcharPara("@actionFlag", 100, actionFlag);
                    proc.AddVarcharPara("@accid", 100, accid);
                    proc.AddVarcharPara("@custid", 100, custid);
                    proc.AddVarcharPara("@branchid", 100, branchid);
                    proc.AddVarcharPara("@prodseriesid", 100, prodseriesid);
                    proc.AddVarcharPara("@isin", 100, isin);
                    proc.AddVarcharPara("@qty", 100, qty);
                    proc.AddVarcharPara("@old_sett_num", 100, old_sett_num);
                    proc.AddVarcharPara("@old_sett_type", 100, old_sett_type);
                    proc.AddVarcharPara("@old_custid", 100, old_custid);
                    proc.AddVarcharPara("@old_prodseriesid", 100, old_prodseriesid);
                    proc.AddVarcharPara("@old_isin", 100, old_isin);
                    proc.AddVarcharPara("@old_qty", 100, old_qty);
                    proc.AddVarcharPara("@old_accid", 100, old_accid);
                    proc.AddVarcharPara("@TransactionID", 100, TransactionID);


                    //    proc.AddVarcharPara("@result", 50, "", QueryParameterDirection.Output);

                    int i = proc.RunActionQuery();
                    //rtrnvalue = proc.GetParaValue("@result").ToString();
                    //return rtrnvalue;


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
