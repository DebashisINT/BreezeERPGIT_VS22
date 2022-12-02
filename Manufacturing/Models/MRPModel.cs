using BusinessLogicLayer;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Manufacturing.Models
{
    public class MRPModel
    {
        string ConnectionString = String.Empty;
        public MRPModel()
        {
            ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }

        public DataSet DropDownDetailForBOM(String Action, String FinYear, String CompanyID, String userbranchlist, Int32 branchid, Int32 id, String ClosedBOMRemarks = "")
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Prc_MRPEntryDataGet");
            proc.AddVarcharPara("@Action", 100, Action);
            proc.AddVarcharPara("@FinYear", 500, FinYear);
            proc.AddVarcharPara("@CompanyID", 500, CompanyID);
            proc.AddVarcharPara("@userbranchlist", 5000, userbranchlist);
            proc.AddIntegerPara("@BRANCHID",branchid);
            proc.AddIntegerPara("@ID", id);
            proc.AddPara("@doc_Type", "BOM");
            proc.AddVarcharPara("@ClosedBOMRemarks", 500, ClosedBOMRemarks);
            ds = proc.GetDataSet();
            return ds;
        }
        public DataTable GetParentBOM(string Branch)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_MRPEntryDataGet");
            proc.AddVarcharPara("@ACTION", 100, "GetBOMList");
            proc.AddBigIntegerPara("@BRANCHID", Convert.ToInt32(Branch));
            ds = proc.GetTable();
            return ds;
        }
        public DataTable GetBOMDetails(string BOMID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_MRPEntryDataGet");
            proc.AddVarcharPara("@ACTION", 100, "GetBOMListDetails");
            proc.AddBigIntegerPara("@DETAILSID", Convert.ToInt32(BOMID));
            ds = proc.GetTable();
            return ds;
        }
        public DataTable GetMPSDetails(string MPSID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_MRPEntryDataGet");
            proc.AddVarcharPara("@ACTION", 100, "GetMPSListDetails");
            proc.AddBigIntegerPara("@DETAILSID", Convert.ToInt32(MPSID));
            ds = proc.GetTable();
            return ds;
        }
        //public DataSet GetBOMEntryList(String Action)
        //{
        //    DataSet ds = new DataSet();
        //    ProcedureExecute proc = new ProcedureExecute("usp_BOMEntryListGet");
        //    proc.AddVarcharPara("@Action", 100, Action);
        //    ds = proc.GetDataSet();
        //    return ds;
        //}

        public DataSet MRPProductEntryInsertUpdate(String action, Int64 MRP_ID,Int64 MRP_SCHEMAID, String MRPNo, DateTime? MRPDate, Int64 BOMID, Int32 Unit
            ,DataTable dtBOM_PRODUCTS, Decimal FinishedQty=0
            , Int64 userid = 0, String Remarks = "", string strCompanyID = "", string FinYear = "", Int64 MPS_ID = 0, Int64 FG_ID = 0)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Prc_MRPEntryAddEdit");
            proc.AddVarcharPara("@ACTION", 150, action);
            proc.AddBigIntegerPara("@MRP_ID", MRP_ID);
            proc.AddVarcharPara("@MRPNo", 100, MRPNo);
            proc.AddDateTimePara("@MRPDate", Convert.ToDateTime(MRPDate));           
            proc.AddDecimalPara("@FINISHEDQTY",4,18, FinishedQty);            
            proc.AddIntegerPara("@BRANCH_ID", Unit);           
            proc.AddVarcharPara("@REMARKS", 500, Remarks);      
            proc.AddBigIntegerPara("@BOM_SCHEMAID", MRP_SCHEMAID);
            proc.AddBigIntegerPara("@RDETAILS_ID", BOMID);           
            proc.AddBigIntegerPara("@USERID", userid);
            proc.AddVarcharPara("@CompanyID", 50, strCompanyID);
            proc.AddVarcharPara("@FinYear", 50, FinYear);
            proc.AddBigIntegerPara("@MPS_ID", MPS_ID);
            proc.AddBigIntegerPara("@FINISHEDITEM", FG_ID);
            if (action == "INSERTMAINPRODUCT" || action == "UPDATEMAINPRODUCT")
            {
                proc.AddPara("@UDTBOM_PRODUCTS", dtBOM_PRODUCTS);
            }            
            ds = proc.GetDataSet();
            return ds;
        }

        public DataTable GetNumberingSchema(string strCompanyID, string strBranchID, string strFinYear, string strType, string strIsSplit)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("usp_BOMEntryDataGet");
            proc.AddVarcharPara("@ACTION", 100, "GetNumberingSchema");
            proc.AddVarcharPara("@CompanyID", 100, strCompanyID);
            //proc.AddVarcharPara("@BranchID", 100, strBranchID);
            proc.AddVarcharPara("@strBranchID", 2000, strBranchID);
            proc.AddVarcharPara("@FinYear", 100, strFinYear);
            proc.AddVarcharPara("@Type", 100, strType);
            proc.AddVarcharPara("@IsSplit", 100, strIsSplit);
            proc.AddPara("@doc_Type", "BOM");
            ds = proc.GetTable();
            return ds;
        }
         public DataTable GetBOMProductEntryListByID(String Action , Int64 DetailsID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_MRPEntryDataGet");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddBigIntegerPara("@DetailsID", DetailsID);           
            ds = proc.GetTable();
            return ds;
        }
         public DataTable GetBOMHeaderEntryListByID(String Action, Int64 MRP_ID)
         {
             DataTable ds = new DataTable();
             ProcedureExecute proc = new ProcedureExecute("Prc_MRPEntryDataGet");
             proc.AddVarcharPara("@ACTION", 100, Action);
             proc.AddBigIntegerPara("@MRP_ID", MRP_ID);
             ds = proc.GetTable();
             return ds;
         }
         public DataTable GetMPSEntryListByFGQtyID(String Action, Int64 MPS_ID, string FGQTY)
         {
             DataTable ds = new DataTable();
             ProcedureExecute proc = new ProcedureExecute("Prc_MRPEntryDataGet");
             proc.AddVarcharPara("@ACTION", 100, Action);
             proc.AddBigIntegerPara("@MPS_ID", MPS_ID);             
             //proc.AddVarcharPara("@FGQTY", 100, FGQTY);

             ds = proc.GetTable();
             return ds;
         }
         public DataTable GetMPSEntryListByID(String Action, Int64 MPS_ID, Int64 PRODUCTID,string FGQTY)
         {
             DataTable ds = new DataTable();
             ProcedureExecute proc = new ProcedureExecute("Prc_MRPEntryDataGet");
             proc.AddVarcharPara("@ACTION", 100, Action);
             proc.AddBigIntegerPara("@MPS_ID", MPS_ID);
             proc.AddBigIntegerPara("@Finished_ProductID", PRODUCTID);
             proc.AddVarcharPara("@FGQTY",100, FGQTY);
             
             ds = proc.GetTable();
             return ds;
         }
         public DataTable GetMPSNO(String BRANCH)
         {
             DataTable ds = new DataTable();
             ProcedureExecute proc = new ProcedureExecute("usp_BOMEntryDataGet");
             proc.AddVarcharPara("@ACTION", 500, "GetMPSNO");
             proc.AddVarcharPara("@BRANCHID", 10, BRANCH);
             ds = proc.GetTable();
             return ds;
         }

    }
}