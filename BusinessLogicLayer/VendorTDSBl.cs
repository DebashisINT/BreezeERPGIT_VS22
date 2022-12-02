using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace BusinessLogicLayer
{
   public class VendorTDSBl
    {

       public void SaveVendorTDSMap(string vendor_internalId, string TDS_Deductees,int userId)
       {
           ProcedureExecute proc;
           try
           {


               using (proc = new ProcedureExecute("prc_Vendor_tds"))
               {

                   proc.AddVarcharPara("@Action", 20, "Add");
                   proc.AddVarcharPara("@Vendor_internalId", 50, vendor_internalId);
                   proc.AddVarcharPara("@TDS_Deductees", 50, TDS_Deductees);
                   proc.AddIntegerPara("@CreatedUser", userId);

                   int i = proc.RunActionQuery();

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


       public void UpdateVendorTDSMap(string vendor_internalId, string TDS_Deductees, int userId)
       {
           ProcedureExecute proc;
           try
           {


               using (proc = new ProcedureExecute("prc_Vendor_tds"))
               {

                   proc.AddVarcharPara("@Action", 20, "update");
                   proc.AddVarcharPara("@Vendor_internalId", 50, vendor_internalId);
                   proc.AddVarcharPara("@TDS_Deductees", 50, TDS_Deductees);
                   proc.AddIntegerPara("@CreatedUser", userId);

                   int i = proc.RunActionQuery();

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


       public DataTable GetVendorTdsDetails(string vendor_internalId)
       {
           DataTable returnTable = new DataTable();

           ProcedureExecute proc;
           try
           {


               using (proc = new ProcedureExecute("prc_Vendor_tds"))
               {

                   proc.AddVarcharPara("@Action", 20, "Select");
                   proc.AddVarcharPara("@Vendor_internalId", 50, vendor_internalId);

                   returnTable = proc.GetTable();
                   return returnTable;
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

       public DataTable GetTDSMASTERLIST()
       {
           DataTable returnTable = new DataTable();
           ProcedureExecute proc;
           try
           {
               using (proc = new ProcedureExecute("PRC_TDSMASTERLIST"))
               {
                   returnTable = proc.GetTable();
                   return returnTable;
               }
           }
           catch
           {
               return null;
           }
           finally
           {
               proc = null;
           }
       }

       public void UpdateTDSMap(string cnt_internalId, string TDSRATE_TYPE)
       {
           ProcedureExecute proc;
           try
           {
               using (proc = new ProcedureExecute("prc_tdsUpdate"))
               {
                   proc.AddVarcharPara("@cnt_internalId", 10, cnt_internalId);
                   proc.AddVarcharPara("@TDSRATE_TYPE", 50, TDSRATE_TYPE);
                   int i = proc.RunActionQuery();
               }
           }
           catch
           {
             
           }
           finally
           {
               proc = null;
           }
       }

       public DataTable GetVendorTdsDetailsContact(string vendor_internalId)
       {
           DataTable returnTable = new DataTable();
           ProcedureExecute proc;
           try
           {
               using (proc = new ProcedureExecute("PRC_GetVendorTDSCustomerDetails"))
               {
                   proc.AddVarcharPara("@cnt_internalId", 50, vendor_internalId);
                   returnTable = proc.GetTable();
                   return returnTable;
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
