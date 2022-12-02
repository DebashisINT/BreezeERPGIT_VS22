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
/// Summary description for PullOtherMarketTradesBL
/// </summary>
namespace BusinessLogicLayer
{

    public class PullOtherMarketTradesBL
    {

        public void PULL_OTHERTRADES(string CLIENTID, string SEGMENT, string COMPANYID, string FROMDATE,
           string TODATE, string FINYEAR, string OTHERCOMPANYID, string OTHERSEGMENTID, string CREATUSER)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {
                using (proc = new ProcedureExecute("PULL_OTHERTRADES"))
                {

                    proc.AddVarcharPara("@CLIENTID", 100, CLIENTID);
                    proc.AddVarcharPara("@SEGMENT", 100, SEGMENT);
                    proc.AddVarcharPara("@COMPANYID", 100, COMPANYID);
                    proc.AddVarcharPara("@FROMDATE", 100, FROMDATE);
                    proc.AddVarcharPara("@TODATE", 100, TODATE);
                    proc.AddVarcharPara("@FINYEAR", 100, FINYEAR);
                    proc.AddVarcharPara("@OTHERCOMPANYID", 100, OTHERCOMPANYID);
                    proc.AddVarcharPara("@OTHERSEGMENTID", 100, OTHERSEGMENTID);
                    proc.AddIntegerPara("@CREATUSER", Convert.ToInt32(CREATUSER));





                    //    proc.AddVarcharPara("@result", 50, "", QueryParameterDirection.Output);


                    int i = proc.RunActionQuery();
                    //  rtrnvalue = proc.GetParaValue("@result").ToString();
                    // return i.ToString();


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
