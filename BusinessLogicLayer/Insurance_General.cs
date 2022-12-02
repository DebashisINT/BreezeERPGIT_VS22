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
/// Summary description for Insurance_General
/// </summary>
/// 
namespace BusinessLogicLayer
{
    public class Insurance_General
    {
        public Insurance_General()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public string Insert_InsuranceNonLife(string vDescription, string vNameOfCompany, string vIPlanType, string vIRDAUIN,
                                                DateTime vOTillDate, DateTime vTilldate, string vCreateUser
                                                 )
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {



                using (proc = new ProcedureExecute("InsuNonLifeInsert"))
                {

                    proc.AddVarcharPara("@Description", 100, vDescription);
                    proc.AddVarcharPara("@NameOfCompany", 100, vNameOfCompany);
                    proc.AddVarcharPara("@IPlanType", 100, vIPlanType);
                    proc.AddVarcharPara("@IRDAUIN", 100, vIRDAUIN);
                    proc.AddDateTimePara("@OTillDate", vOTillDate);
                    proc.AddDateTimePara("@Tilldate", vTilldate);
                    proc.AddVarcharPara("@CreateUser", 100, vCreateUser);

                    proc.AddVarcharPara("@result", 100, "", QueryParameterDirection.Output);

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
    }
}
