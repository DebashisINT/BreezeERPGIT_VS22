using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace BusinessLogicLayer
{
   public class RemarkCategoryBL
    {

        public int insertRemarkCategory(string rea_internalId, int cat_id, string rea_Remarks,string CreateUser) 
        {
            ProcedureExecute proc;
            try
            {


                using (proc = new ProcedureExecute("InsertRemarkCategory"))
                {

                    proc.AddVarcharPara("@rea_internalId", 50, rea_internalId);
                    proc.AddBigIntegerPara("@cat_id", cat_id);
                    proc.AddVarcharPara("@rea_Remarks", 500, rea_Remarks);
                    proc.AddVarcharPara("@CreateUser", 100, CreateUser);  
                    int i = proc.RunActionQuery(); 
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

        public int insertVendorBranchMap(string BranchIdList, string Ven_Internalid, int BranchId)
        {
            ProcedureExecute proc;
            try
            {


                using (proc = new ProcedureExecute("Prc_VendorBranchMap"))
                {
                    proc.AddVarcharPara("@Vendor_InternalId", 50, Ven_Internalid);
                    proc.AddVarcharPara("@BranchIdList", 500, BranchIdList);
                    proc.AddIntegerPara("@BranchId", BranchId);   
                    
                    int i = proc.RunActionQuery();
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


        public int insertVehicleBranchMap(string BranchIdList, string VendorId, int BranchId)
        {
            ProcedureExecute proc;
            try
            {


                using (proc = new ProcedureExecute("Prc_VehicleBranchMap"))
                {
                    proc.AddIntegerPara("@VehicleId",Convert.ToInt32( VendorId));
                    proc.AddVarcharPara("@BranchIdList", 500, BranchIdList);
                    proc.AddIntegerPara("@BranchId", BranchId);

                    int i = proc.RunActionQuery();
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
       public DataTable insertRemarksCategoryAddMode(string type ,string rea_internalId,DataTable UdfTable,string CreateUser)
        {
            DataRow[] saveData = UdfTable.Select("Type='" + type + "'");
            foreach (DataRow dr in saveData)
            {
                insertRemarkCategory(rea_internalId,Convert.ToInt32( dr["cat_id"]), Convert.ToString( dr["RemarksData"]), CreateUser);
                UdfTable.Rows.Remove(dr);
            }
           
            return UdfTable;
        }


       public Boolean isAllMandetoryDone(DataTable udfTable, string type)
       {
           bool returnValue = true;
           ProcedureExecute proc;
           using (proc = new ProcedureExecute("prc_getudfmandetory"))
           {

               proc.AddVarcharPara("@appliFor", 10, type);
               DataTable ExistingUdfTable = proc.GetTable();
               if (ExistingUdfTable != null)
               {

                   if (udfTable == null && ExistingUdfTable.Rows.Count > 0) {
                       return false;
                   }

                   if (ExistingUdfTable.Rows.Count > 0)
                   {
                       foreach (DataRow dr in ExistingUdfTable.Rows) {
                           DataRow[] udfRow = udfTable.Select("Type='"+type+"' and cat_id="+Convert.ToString(dr[0]));
                           if (udfRow.Length < 1) {
                               return false;
                           }
                       }
                   }
               }                
           }

           return returnValue;
       }




       public int insertVendorBranchMap(char p, string InternalID)
       {
           throw new NotImplementedException();
       }
    }
}
