using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccessLayer;

namespace BusinessLogicLayer
{
    public class Special_DP_Account
    {
        public string Insert_DPAccount(string SpecialDPAccount_SegmentId, string SpecialDPAccount_DPID, string SpecialDPAccount_DPName,
                                       string SpecialDPAccount_AccountID, string SpecialDPAccount_AccountName, string SpecialDPAccount_CMBPID,
                                       string SpecialDPAccount_CMCode, string SpecialDPAccount_TMCode, string SpecialDPAccount_Groupcode,
                                       string SpecialDPAccount_CreateUser, string SpecialDPAccount_CreateDateTime, string SpecialDPAccount_ModifyUser)
        {
            ProcedureExecute proc;
            string rtrnvalue = "";
            try
            {


                using (proc = new ProcedureExecute("DPAccountInsert"))
                {
                    
                    proc.AddCharPara("@SpecialDPAccount_DPID", 8,Convert.ToChar(SpecialDPAccount_DPID));
                    proc.AddIntegerPara("@SpecialDPAccount_SegmentId",Convert.ToInt32(SpecialDPAccount_SegmentId));
                    proc.AddVarcharPara("@SpecialDPAccount_DPName", 100, SpecialDPAccount_DPName);
                    proc.AddCharPara("@SpecialDPAccount_AccountID", 8, Convert.ToChar(SpecialDPAccount_AccountID));
                    proc.AddVarcharPara("@SpecialDPAccount_AccountName", 100, SpecialDPAccount_AccountName);
                    proc.AddCharPara("@SpecialDPAccount_CMBPID", 8, Convert.ToChar(SpecialDPAccount_CMBPID));
                    proc.AddCharPara("@SpecialDPAccount_CMCode", 8, Convert.ToChar(SpecialDPAccount_CMCode));
                    proc.AddCharPara("@SpecialDPAccount_TMCode", 8, Convert.ToChar(SpecialDPAccount_TMCode));
                    proc.AddCharPara("@SpecialDPAccount_Groupcode", 10, Convert.ToChar(SpecialDPAccount_Groupcode));
                    proc.AddIntegerPara("@SpecialDPAccount_CreateUser",Convert.ToInt32(SpecialDPAccount_CreateUser));
                    proc.AddDateTimePara("@SpecialDPAccount_CreateDateTime", Convert.ToDateTime(SpecialDPAccount_CreateDateTime));
                    proc.AddIntegerPara("@SpecialDPAccount_ModifyUser", Convert.ToInt32(SpecialDPAccount_ModifyUser));
                    proc.AddBigIntegerPara("@Result", 20, QueryParameterDirection.Output);

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
