using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Manufacturing.Models
{
    public class BomRelationshipModel
    {
        public DataTable GetChildEntryListByID(String Action, Int64 BOMRelationID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_BOMRelationshipDetails");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddBigIntegerPara("@BOMRelationID", BOMRelationID);        
            ds = proc.GetTable();
            return ds;
        }
        public DataTable GetParentBOM(string Branch)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_BOMRelationshipDetails");
            proc.AddVarcharPara("@ACTION", 100, "GetBOMList");
            proc.AddBigIntegerPara("@BranchId",Convert.ToInt32(Branch));     
            ds = proc.GetTable();
            return ds;
        }
        public DataTable GetChildBOM(string Branch,string ParentBOMID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_BOMRelationshipDetails");
            proc.AddVarcharPara("@ACTION", 100, "GetChildBOMList");
            proc.AddBigIntegerPara("@BranchId", Convert.ToInt32(Branch));     
            proc.AddVarcharPara("@ParentBOMID", 100, ParentBOMID);  
            ds = proc.GetTable();
            return ds;
        }
        public DataSet DropDownDetailForBOMRelation(String Action, String FinYear, String CompanyID, String userbranchlist, Int32 branchid, Int32 id)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_BOMRelationshipDetails");
            proc.AddVarcharPara("@Action", 100, Action);
            proc.AddVarcharPara("@FinYear", 500, FinYear);
            proc.AddVarcharPara("@CompanyID", 500, CompanyID);
            proc.AddVarcharPara("@userbranchlist", 5000, userbranchlist);
            proc.AddIntegerPara("@BRANCHID", branchid);
            proc.AddIntegerPara("@ID", id);
            proc.AddPara("@doc_Type", "Estimate");
            ds = proc.GetDataSet();
            return ds;
        }
        
        public DataSet BOMRelationEntryInsertUpdate(String action, String BomRelationshipNo, string BomRelationshipName
           , Int32 Unit, DataTable dtEstimate_PRODUCTS, Int32 ParentBOMID,
            String ParentBOMFG, String ParentBOMREV, String LastCompany, String LastFinYear, Int64 userid = 0, Int64 BOMRelation_ID = 0
            
           )
        {
            DataSet ds = new DataSet();
            try
            {             
                ProcedureExecute proc = new ProcedureExecute("PRC_BOMRelationshipAddEdit");
                proc.AddVarcharPara("@ACTION", 150, action);
                proc.AddVarcharPara("@BOMRelation_No", 100, BomRelationshipNo);
                proc.AddVarcharPara("@BOMRelation_Name", 100, BomRelationshipName);
                proc.AddIntegerPara("@BRANCH_ID", Unit);
                proc.AddBigIntegerPara("@ParentBOM_ID", ParentBOMID);
                proc.AddBigIntegerPara("@BOMRelation_ID", BOMRelation_ID);
                proc.AddVarcharPara("@ParentBOM_FG",100, ParentBOMFG);
                proc.AddVarcharPara("@ParentBOM_REV", 500, ParentBOMREV);
                    

                proc.AddBigIntegerPara("@USERID", userid);
              //  proc.AddPara("@CompanyID", LastCompany);
              //  proc.AddPara("@FinYear", LastFinYear);
                

                if (action == "INSERTMAINPRODUCT" || action == "UPDATEMAINPRODUCT")
                {
                    //if (dtEstimate_PRODUCTS.Columns.Contains("SrlNo"))
                    //{
                    //    dtEstimate_PRODUCTS.Columns.Remove("SrlNo");
                    //}
                    //if (dtEstimate_PRODUCTS.Columns.Contains("StkUOM"))
                    //{
                    //    dtEstimate_PRODUCTS.Columns.Remove("StkUOM");
                    //}
                    proc.AddPara("@UDTEstimate_PRODUCTS", dtEstimate_PRODUCTS);


                }
                
                ds = proc.GetDataSet();
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }

        public DataTable GetBOMRelationHeaderListByID(String Action, Int64 BOMRelationID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_BOMRelationshipDetails");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddBigIntegerPara("@BOMRelationID", BOMRelationID);
            // proc.AddPara("@doc_Type", "Estimate");
            ds = proc.GetTable();
            return ds;
        }


        public DataSet DeleteForBOMRelation(String Action, Int32 id)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_BOMRelationshipDetails");
            proc.AddVarcharPara("@Action", 100, Action);
            proc.AddIntegerPara("@BOMRelationID", id);            
            ds = proc.GetDataSet();
            return ds;
        }


    }
}