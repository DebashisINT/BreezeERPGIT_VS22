using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
   public class MShortNameCheckingBL
    {
        /// <summary>
        /// This function will check whether the short name is unique or not. in the procedure prc_MShortNameCheckingDtl
        /// </summary>
        /// <param name="shortname">the value of new short name</param>
        /// <param name="code"> 0 if add new else primary Id</param>
        /// <param name="action">Random sequence of char to identify each zone in procedure</param>
        /// <returns> return true or false (Unique or not)</returns>
        /// 
        public bool CheckUnique(string shortname, string code, string action)
        {
            ProcedureExecute proc;

            try
            {
                using (proc = new ProcedureExecute("prc_MShortNameCheckingDtl"))
                {
                    proc.AddVarcharPara("@action", 50, action);
                    proc.AddBooleanPara("@ReturnValue", false, QueryParameterDirection.Output);
                    proc.AddVarcharPara("@ShortName", 50, shortname);
                    proc.AddVarcharPara("@code", 50, code);
                    
                    int i = proc.RunActionQuery();
                    var retData =Convert.ToBoolean( proc.GetParaValue("@ReturnValue"));
                    return retData;
                }
            }

            catch (Exception ex)
            {
                return false;
            }

            finally
            {
                proc = null;
            }

        }


//(Convert.ToString(vendorid).Trim(), partyno, "PurchaseInvoice_CheckPartyNo");

        public bool CheckUniquePartyNo(string vendorid, string partyno, string action, string mode,string pbid)
        {
            ProcedureExecute proc;

            try
            {
                using (proc = new ProcedureExecute("prc_MShortNameCheckingDtl"))
                {
                    proc.AddVarcharPara("@action", 50, action);
                    proc.AddBooleanPara("@ReturnValue", false, QueryParameterDirection.Output);
                    proc.AddVarcharPara("@VendorId", 50, vendorid);
                    proc.AddVarcharPara("@partyno", 50, partyno);
                    proc.AddVarcharPara("@mode", 5, mode);
                    proc.AddVarcharPara("@PBID", 50, pbid);
                    
                    int i = proc.RunActionQuery();
                    var retData =Convert.ToBoolean( proc.GetParaValue("@ReturnValue"));
                    return retData;
                }
            }

            catch (Exception ex)
            {
                return false;
            }

            finally
            {
                proc = null;
            }

        }


       

       //added for schema unique name 
        public bool CheckUniqueWithtype(string shortname, string code, string action,string typeid)
        {
            ProcedureExecute proc;

            try
            {
                using (proc = new ProcedureExecute("prc_MShortNameCheckingDtl"))
                {
                    proc.AddVarcharPara("@action", 50, action);
                    proc.AddBooleanPara("@ReturnValue", false, QueryParameterDirection.Output);
                    proc.AddVarcharPara("@ShortName", 50, shortname);
                    proc.AddVarcharPara("@typeid", 50, typeid);
                    proc.AddVarcharPara("@code", 50, code);

                    int i = proc.RunActionQuery();
                    var retData = Convert.ToBoolean(proc.GetParaValue("@ReturnValue"));
                    return retData;
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


            return false;
        }
        //added for contactgeneral Page of unique name 
        public bool CheckUniqueWithtypeContactMaster(string shortname, string code, string action, string contacttype, ref string CustName)
        {
            ProcedureExecute proc;

            try
            {
                using (proc = new ProcedureExecute("prc_MShortNameCheckingDtl"))
                {
                    proc.AddVarcharPara("@action", 50, action);
                    proc.AddBooleanPara("@ReturnValue", false, QueryParameterDirection.Output);
                    proc.AddVarcharPara("@ShortName", 50, shortname);
                    proc.AddVarcharPara("@code", 50, code);
                    proc.AddVarcharPara("@contacttype", 50, contacttype);
                    proc.AddVarcharPara("@EntityName", 100, "", QueryParameterDirection.Output);
                    int i = proc.RunActionQuery();
                    var retData = Convert.ToBoolean(proc.GetParaValue("@ReturnValue"));
                    CustName = Convert.ToString(proc.GetParaValue("@EntityName"));
                    return retData;
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


            return false;
        }
        public bool CheckUniqueDefaulttypeContactMaster(string shortname, string code, string action, string contacttype)
        {
            ProcedureExecute proc;

            try
            {
                using (proc = new ProcedureExecute("[Prc_checkdefaoultAddreddEmailPhone]"))
                {
                    proc.AddVarcharPara("@action", 50, action);
                    proc.AddBooleanPara("@ReturnValue", false, QueryParameterDirection.Output);
                    proc.AddVarcharPara("@ShortName", 50, shortname);
                    proc.AddVarcharPara("@code", 50, code);
                    proc.AddVarcharPara("@contacttype", 50, contacttype);

                    int i = proc.RunActionQuery();
                    var retData = Convert.ToBoolean(proc.GetParaValue("@ReturnValue"));
                    return retData;
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


            return false;
        }
       
    }
}
