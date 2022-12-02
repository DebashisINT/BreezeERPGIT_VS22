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
/// Summary description for Brokerage_SchemesBL
/// </summary>
namespace BusinessLogicLayer
{
    public class Brokerage_SchemesBL
    {
        public string Insert_MasterChargeGroup(string ChargeGroup_Code, string ChargeGroup_Name, string ChargeGroup_Type, string ChargeGroup_IsDefault,
                                                string ChargeGroup_CreateUser, string ChargeGroup_CreateDateTime, string ChargeGroup_ModifyUser)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {
                using (proc = new ProcedureExecute("MasterChargeGroupInsert"))
                {

                    proc.AddVarcharPara("@ChargeGroup_Code", 100, ChargeGroup_Code);
                    proc.AddVarcharPara("@ChargeGroup_Name", 100, ChargeGroup_Name);
                    proc.AddIntegerPara("@ChargeGroup_Type", Convert.ToInt32(ChargeGroup_Type));
                    proc.AddVarcharPara("@ChargeGroup_IsDefault", 100, ChargeGroup_IsDefault);
                    proc.AddIntegerPara("@ChargeGroup_CreateUser", Convert.ToInt32(ChargeGroup_CreateUser));
                    proc.AddDateTimePara("@ChargeGroup_CreateDateTime", Convert.ToDateTime(ChargeGroup_CreateDateTime));
                    proc.AddIntegerPara("@ChargeGroup_ModifyUser", Convert.ToInt32(ChargeGroup_ModifyUser));
                    proc.AddVarcharPara("@ResultCode", 100, "", QueryParameterDirection.Output);


                    int i = proc.RunActionQuery();
                    rtrnvalue = proc.GetParaValue("@ResultCode").ToString();
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
