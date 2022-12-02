using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace BusinessLogicLayer
{
    public class TdsTcsBL
    {
        public DataTable PopulateMainAccountDropDownForTDSTCS()
        {
            ProcedureExecute proc;

            try
            {
                using (proc = new ProcedureExecute("prc_TDSTCS"))
                {
                    proc.AddVarcharPara("@action", 50, "PopulateMainAccountDropDownForTDSTCS");
                    return proc.GetTable();
                }
            }

            catch (Exception ex)
            {
                return null;
            }

            finally
            {
                proc = null;
            }



        }



        public DataTable PopulateTDSTCSInEditMode(int TDSTCSID)
        {
            ProcedureExecute proc;

            try
            {
                using (proc = new ProcedureExecute("prc_TDSTCS"))
                {
                    proc.AddVarcharPara("@action", 50, "PopulateTDSTCSInEditMode");
                    proc.AddIntegerPara("@TDSTCS_ID", TDSTCSID);
                    return proc.GetTable();
                }
            }

            catch (Exception ex)
            {
                return null;
            }

            finally
            {
                proc = null;
            }



        }



        public DataTable PopulateSubAccountDropDownForTDSTCS(string MainAccountID, string branch)
        {
            ProcedureExecute proc;

            try
            {
                using (proc = new ProcedureExecute("dbo.prc_TDSTCS"))
                {
                    proc.AddVarcharPara("@action", 100, "PopulateSubAccountDropDownForTDSTCS");
                    proc.AddVarcharPara("@MainAccountID", 100, MainAccountID);
                    proc.AddVarcharPara("@branch", 500, branch);
                    return proc.GetTable();
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


        public DataTable GetTDSSettings()
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("Prc_GetConfigDetails"))
                {
                    proc.AddVarcharPara("@Variable_Name", 500, "OLDTDSSetting");
                    return proc.GetTable();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                proc = null;
            }

        }

        public int DeleteNewTDSTCS(string TDSTCSRates_ID, string Action)
        {
            int i = 0;
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("PRC_NEWTDSTCSEDITDELETE"))
                {
                    proc.AddVarcharPara("@TDSTCSRates_ID", 10, TDSTCSRates_ID);
                    proc.AddVarcharPara("@Action", 100, Action);
                    i = proc.RunActionQuery();
                }
            }
            catch
            {
            }
            finally
            {
                proc = null;
            }
            return i;
        }

        public DataTable GetNewTDSTcsDetails(String TDSTCSRates_ID)
        {
            ProcedureExecute proc;
            try
            {
                using (proc = new ProcedureExecute("PRC_NEWTDSTCSEDITDELETE"))
                {
                    proc.AddVarcharPara("@TDSTCSRates_ID", 10, TDSTCSRates_ID);
                    proc.AddVarcharPara("@Action", 100, "EDIT");
                    return proc.GetTable();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                proc = null;
            }

        }
    }

    public class NewTdsTcsDetails
    {
        public String TDSTCSRates_ID { get; set; }
        public String TDSTCSRates_Code { get; set; }
        public DateTime TDSTCSRates_DateFrom { get; set; }
        public String TDSTCSRates_Rate { get; set; }
        public String TDSTCSRates_Surcharge { get; set; }
        public String TDSTCSRates_EduCess { get; set; }
        public String TDSTCSRates_HgrEduCess { get; set; }
        public String TDSTCSRates_ApplicableAmount { get; set; }
        public String TDSTCSRates_ApplicableFor { get; set; }
        public String TDSTCSRates_Rouding { get; set; }
    }
}
