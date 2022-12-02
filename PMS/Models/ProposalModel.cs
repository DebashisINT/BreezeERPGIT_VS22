using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PMS.Models
{
    public class ProposalModel
    {
         string ConnectionString = String.Empty;
         public ProposalModel()
        {
            ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }

        public DataSet DropDownDetailForBOQ(String Action, String FinYear, String CompanyID, String userbranchlist, Int32 branchid, Int32 id)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("usp_BOMEntryDataGet");
            proc.AddVarcharPara("@Action", 100, Action);
            proc.AddVarcharPara("@FinYear", 500, FinYear);
            proc.AddVarcharPara("@CompanyID", 500, CompanyID);
            proc.AddVarcharPara("@userbranchlist", 5000, userbranchlist);
            proc.AddIntegerPara("@BRANCHID", branchid);
            proc.AddIntegerPara("@ID", id);
            proc.AddPara("@doc_Type", "Proposal");
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet BOMProductEntryInsertUpdate(String action, String BOMNo, DateTime? BOMDate, Int64 FinishedItem, Decimal FinishedQty, String FinishedUom, String BOMType, String RevisionNo, DateTime? RevisionDate,
            Int32 Unit, Int32 Warehouse, DataTable dtBOM_PRODUCTS, DataTable dtBOM_RESOURCES, Int32 BOM_SCHEMAID, Decimal ActualAdditionalCost, Int64 userid = 0, Int32 production_id = 0, Int64 details_id = 0)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("usp_BOMProductEntryGet");
            proc.AddVarcharPara("@ACTION", 150, action);
            proc.AddVarcharPara("@BOMNO", 100, BOMNo);
            proc.AddDateTimePara("@BOMDATE", Convert.ToDateTime(BOMDate));
            proc.AddBigIntegerPara("@FINISHEDITEM", FinishedItem);
            proc.AddDecimalPara("@FINISHEDQTY", 4, 18, FinishedQty);
            proc.AddVarcharPara("@FINISHEDUOM", 100, FinishedUom);
            proc.AddVarcharPara("@BOMTYPE", 150, BOMType);
            proc.AddVarcharPara("@REVISIONNO", 100, RevisionNo);
            proc.AddDateTimePara("@REVISIONDATE", Convert.ToDateTime(RevisionDate));
            proc.AddIntegerPara("@BRANCH_ID", Unit);
            proc.AddIntegerPara("@WAREHOUSEID", Warehouse);
            proc.AddBigIntegerPara("@BOM_SCHEMAID", BOM_SCHEMAID);
            proc.AddDecimalPara("@ACTUALADDITIONALCOST", 2, 18, ActualAdditionalCost);

            proc.AddBigIntegerPara("@RPRODUCTION_ID", production_id);
            proc.AddBigIntegerPara("@RDETAILS_ID", details_id);

            proc.AddBigIntegerPara("@USERID", userid);

            proc.AddPara("@doc_Type", "Proposal");

            if (action == "INSERTMAINPRODUCT" || action == "UPDATEMAINPRODUCT")
            {
                proc.AddPara("@UDTBOM_PRODUCTS", dtBOM_PRODUCTS);
            }
            if (action == "INSERTRESOURCES")
            {
                proc.AddPara("@UDTBOM_RESOURCES", dtBOM_RESOURCES);
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
            proc.AddPara("@doc_Type", "Proposal");
            ds = proc.GetTable();
            return ds;
        }

        public DataTable GetBOMProductEntryListByID(String Action, Int64 DetailsID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("usp_BOMEntryDataGet");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddBigIntegerPara("@DetailsID", DetailsID);
            proc.AddPara("@doc_Type", "Proposal");
            ds = proc.GetTable();
            return ds;
        }
    }
}