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
   public class WarehouseConfigMasterBL
    {
       public int UpdateProductWarehouseConfig(string prod_id, string warehouse_id, bool chkisactive, string chkisactivatebatchno, string chkisactivateserialno, string chkisstockblock,string chkIsActiveBarCode)
       {
           ProcedureExecute proc;
           int retval = 0;
           try
           {
               using (proc = new ProcedureExecute("sp_WarehouseConfig"))
               {
                   proc.AddVarcharPara("@p_prod_ids", 2000, prod_id);
                   proc.AddVarcharPara("@p_warehouseconfig_ids", 2000, warehouse_id);
                   proc.AddBooleanPara("@p_Is_active_warehouse", chkisactive);
                   proc.AddVarcharPara("@p_Is_active_Batch",2000, chkisactivatebatchno);
                   proc.AddVarcharPara("@p_Is_active_serialno", 2000,chkisactivateserialno);
                   proc.AddVarcharPara("@p_Is_stock_block", 2000, chkisstockblock);
                   proc.AddVarcharPara("@p_IsActiveBarCode", 2000, chkIsActiveBarCode);
                   int i = proc.RunActionQuery();
                   //retval = Convert.ToInt32(proc.GetParaValue("@result"));
                   retval = i;
               }
           }
           catch (Exception ex)
           {

           }
           finally
           {

           }
           return retval;
       }

       public DataTable PopulateBranchByBranchHierarchy(string @userbranch)
       {
           DataTable dt = new DataTable();
           ProcedureExecute proc = new ProcedureExecute("Prc_Warehouse");
           proc.AddVarcharPara("@Action", 100, "PopulateBranchByBranchHierarchy");
           proc.AddVarcharPara("@userbranch", 250, @userbranch);
           dt = proc.GetTable();
           return dt;
       }
    }
}
