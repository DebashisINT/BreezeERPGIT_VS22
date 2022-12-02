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
/// Summary description for DailyTask_CarryForwardLedgerBL
/// </summary>
namespace BusinessLogicLayer
{
    public class DailyTask_CarryForwardLedgerBL
    {
        public int CarryForwardValue(string companyID, string segmentID, string FinYear, string UserID)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {
                using (proc = new ProcedureExecute("CarryForwardValue"))
                {

                    proc.AddVarcharPara("@companyID", 100, companyID);
                    proc.AddVarcharPara("@segmentID", 100, segmentID);
                    proc.AddVarcharPara("@FinYear", 100, FinYear);
                    proc.AddIntegerPara("@UserID", Convert.ToInt32(UserID));


                    //    proc.AddVarcharPara("@result", 50, "", QueryParameterDirection.Output);


                    int i = proc.RunActionQuery();
                    //  rtrnvalue = proc.GetParaValue("@result").ToString();
                    return i;

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
