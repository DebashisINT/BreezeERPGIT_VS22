using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DataAccessLayer;
using System.Web;
using EntityLayer;

namespace BusinessLogicLayer
{
   public class DriverMasterBL
    {
       public DataTable PopulateDriverDetail(string cntId)
       {
           try
           {
               DataTable dt = new DataTable();
               ProcedureExecute proc = new ProcedureExecute("prc_DriversVechilesInformation");
               proc.AddVarcharPara("@Action", 500, "DriverVehiclesHistory");
               proc.AddVarcharPara("@cntid", 20, cntId);

               dt = proc.GetTable();

               return dt;
           }
           catch
           {
               return null;
           }
       }
       public string InsertDriverMaster(string contacttype, string cnt_ucc, string cnt_firstName, int cnt_branchId, string cnt_VerifcationRemarks, bool Is_Active, string lastModifyUser, string cnt_contactType,string MOD)
        {
            ProcedureExecute proc;
            string retval = "";
            try
            {
                using (proc = new ProcedureExecute("ContactInsert_Driver"))
                {
                    proc.AddVarcharPara("@contacttype", 50, contacttype);
                    proc.AddVarcharPara("@cnt_ucc", 80, cnt_ucc);
                    proc.AddVarcharPara("@cnt_firstName", 150, cnt_firstName);
                    proc.AddIntegerPara("@cnt_branchId", cnt_branchId);
                    proc.AddVarcharPara("@cnt_VerifcationRemarks", 150, cnt_VerifcationRemarks);
                    proc.AddBooleanPara("@Is_Active", Is_Active);
                    proc.AddVarcharPara("@lastModifyUser", 20, lastModifyUser);
                    proc.AddVarcharPara("@cnt_contactType", 5, cnt_contactType);
                    proc.AddVarcharPara("@Mod", 3, MOD);
                    proc.AddVarcharPara("@result", 50, "", QueryParameterDirection.Output);
                    int i = proc.RunActionQuery();
                    retval = Convert.ToString(proc.GetParaValue("@result"));

                }
            }
            catch (Exception ex)
            {
                retval = "";
            }
            finally
            {
                proc = null;
            }
            return retval;
        }
       public string InsertServiceCenter(string contacttype, string cnt_ucc, string cnt_firstName, int cnt_branchId, string cnt_VerifcationRemarks, bool Is_Active, string lastModifyUser, string cnt_contactType, string MOD)
        {
            ProcedureExecute proc;
            string retval = "";
            try
            {
                using (proc = new ProcedureExecute("ContactInsert_ServiceCenter"))
                {
                    proc.AddVarcharPara("@contacttype", 50, contacttype);
                    proc.AddVarcharPara("@cnt_ucc", 80, cnt_ucc);
                    proc.AddVarcharPara("@cnt_firstName", 150, cnt_firstName);
                    proc.AddIntegerPara("@cnt_branchId", cnt_branchId);
                    proc.AddVarcharPara("@cnt_VerifcationRemarks", 150, cnt_VerifcationRemarks);
                    proc.AddBooleanPara("@Is_Active", Is_Active);
                    proc.AddVarcharPara("@lastModifyUser", 20, lastModifyUser);
                    proc.AddVarcharPara("@cnt_contactType", 5, cnt_contactType);
                    proc.AddVarcharPara("@Mod", 3, MOD);
                    proc.AddVarcharPara("@result", 50, "", QueryParameterDirection.Output);
                    int i = proc.RunActionQuery();
                    retval = Convert.ToString(proc.GetParaValue("@result"));

                }
            }
            catch (Exception ex)
            {
                retval = "";
            }
            finally
            {
                proc = null;
            }
            return retval;
        }
       public Boolean UpdateDriverMaster(string contacttype, string cnt_ucc, string cnt_firstName, int cnt_branchId, string cnt_VerifcationRemarks, bool Is_Active, string lastModifyUser, string cnt_contactType,int id, string MOD)
       {
           ProcedureExecute proc;
           bool retval = true;
           try
           {
               using (proc = new ProcedureExecute("ContactInsert_Driver"))
               {
                   proc.AddVarcharPara("@contacttype", 50, contacttype);
                   proc.AddVarcharPara("@cnt_ucc", 80, cnt_ucc);
                   proc.AddVarcharPara("@cnt_firstName", 150, cnt_firstName);
                   proc.AddIntegerPara("@cnt_branchId", cnt_branchId);
                   proc.AddVarcharPara("@cnt_VerifcationRemarks", 150, cnt_VerifcationRemarks);
                   proc.AddBooleanPara("@Is_Active", Is_Active);
                   proc.AddVarcharPara("@lastModifyUser", 20, lastModifyUser);
                   proc.AddVarcharPara("@cnt_contactType", 5, cnt_contactType);
                   proc.AddIntegerPara("@cnt_id", id);
                   proc.AddVarcharPara("@Mod", 4, MOD);
                   proc.AddVarcharPara("@result", 50, "", QueryParameterDirection.Output);
                   int i = proc.RunActionQuery();

               }
           }
           catch (Exception ex)
           {
               retval = false;
           }
           finally
           {
               proc = null;
           }
           return retval;
       }


       public Boolean UpdateServiceCenter(string contacttype, string cnt_ucc, string cnt_firstName, int cnt_branchId, string cnt_VerifcationRemarks, bool Is_Active, string lastModifyUser, string cnt_contactType, int id, string MOD)
       {
           ProcedureExecute proc;
           bool retval = true;
           try
           {
               using (proc = new ProcedureExecute("ContactInsert_ServiceCenter"))
               {
                   proc.AddVarcharPara("@contacttype", 50, contacttype);
                   proc.AddVarcharPara("@cnt_ucc", 80, cnt_ucc);
                   proc.AddVarcharPara("@cnt_firstName", 150, cnt_firstName);
                   proc.AddIntegerPara("@cnt_branchId", cnt_branchId);
                   proc.AddVarcharPara("@cnt_VerifcationRemarks", 150, cnt_VerifcationRemarks);
                   proc.AddBooleanPara("@Is_Active", Is_Active);
                   proc.AddVarcharPara("@lastModifyUser", 20, lastModifyUser);
                   proc.AddVarcharPara("@cnt_contactType", 5, cnt_contactType);
                   proc.AddIntegerPara("@cnt_id", id);
                   proc.AddVarcharPara("@Mod", 4, MOD);
                   proc.AddVarcharPara("@result", 50, "", QueryParameterDirection.Output);
                   int i = proc.RunActionQuery();

               }
           }
           catch (Exception ex)
           {
               retval = false;
           }
           finally
           {
               proc = null;
           }
           return retval;
       }

       public string InsertVehiclesAllocationForEachDriver(string contacttype, string intrnlID, string regNums, string lastModifyUser, string cnt_contactType, string MOD)
       {
           ProcedureExecute proc;
           string retval = "";
           try
           {
               using (proc = new ProcedureExecute("VehicleAllocationFor_Driver"))
               {
                   proc.AddVarcharPara("@contacttype", 50, contacttype);
                   proc.AddVarcharPara("@cnt_intID", 50, intrnlID);
                   proc.AddVarcharPara("@cnt_regNumbers", 1000, regNums);
                   proc.AddVarcharPara("@lastModifyUser", 20, lastModifyUser);
                   proc.AddVarcharPara("@cnt_contactType", 5, cnt_contactType);
                   proc.AddVarcharPara("@Mod", 10, MOD);
                   proc.AddVarcharPara("@result", 50, "", QueryParameterDirection.Output);
                   int i = proc.RunActionQuery();
                   retval = Convert.ToString(proc.GetParaValue("@result"));
               }
           }
           catch (Exception ex)
           {
               retval = "";
           }
           finally
           {
               proc = null;
           }
           return retval;
       }

   


       public DataTable PopulateDriverUsers()
       {
           try
           {
               DataTable dt = new DataTable();
               ProcedureExecute proc = new ProcedureExecute("Proc_DriverMasterUser");
               proc.AddVarcharPara("@Action", 500, "FetchUsers");
               dt = proc.GetTable();
               return dt;
           }
           catch
           {
               return null;
           }
       }


       public DataTable PopulateDriverUsersModify(string username)
       {
           try
           {
               DataTable dt = new DataTable();
               ProcedureExecute proc = new ProcedureExecute("Proc_DriverMasterUser");
               proc.AddVarcharPara("@Action", 500, "DriverUsersModifylist");
               proc.AddVarcharPara("@UniqueName", 500, username);
               dt = proc.GetTable();
               return dt;
           }
           catch
           {
               return null;
           }
       }

       public string InsertDriverMasterCedential(string uniqueID, string uniquename,string InternalID)
       {
           ProcedureExecute proc;
           string retval = "";
           try
           {
               using (proc = new ProcedureExecute("Proc_DriverMasterUser"))
               {
                   proc.AddVarcharPara("@Action", 500, "DriverUsersInsert");
                   proc.AddVarcharPara("@UniqueID", 50, uniqueID);
                   proc.AddVarcharPara("@UniqueName", 600, uniquename);
                   proc.AddVarcharPara("@UniqueInternalID", 600, InternalID);
                   int i = proc.RunActionQuery();
                   retval = i.ToString();
               }
           }
           catch (Exception ex)
           {
               retval = "";
           }
           finally
           {
               proc = null;
           }
           return retval;
       }
 


   }
}
