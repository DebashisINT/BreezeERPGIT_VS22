using BusinessLogicLayer;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Manufacturing.Models
{
    public class BOMEntryModel
    {
        string ConnectionString = String.Empty;
        public BOMEntryModel()
        {
            ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }

        public DataSet DropDownDetailForBOM(String Action, String FinYear, String CompanyID, String userbranchlist, Int32 branchid, Int32 id, String ClosedBOMRemarks = "")
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("usp_BOMEntryDataGet");
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

        //public DataSet GetBOMEntryList(String Action)
        //{
        //    DataSet ds = new DataSet();
        //    ProcedureExecute proc = new ProcedureExecute("usp_BOMEntryListGet");
        //    proc.AddVarcharPara("@Action", 100, Action);
        //    ds = proc.GetDataSet();
        //    return ds;
        //}

        public DataSet BOMProductEntryInsertUpdate(String action, String BOMNo, DateTime? BOMDate, Int64 FinishedItem, Decimal FinishedQty, String FinishedUom, String BOMType, String RevisionNo, DateTime? RevisionDate,
            Int32 Unit, Int32 Warehouse, DataTable dtBOM_PRODUCTS, DataTable dtBOM_RESOURCES, Int32 BOM_SCHEMAID, Decimal ActualAdditionalCost, Int64 userid = 0, Int32 production_id = 0
            , Int64 details_id = 0, Decimal TotalResourceCost = 0, String Remarks = "", string PartNo="",string ProjectID="",string strCompanyID="", string FinYear="",Int64 MPS_ID=0,DataTable MultiUomDetails=null)
        {
            DataSet ds = new DataSet();
            //rev Pratik
            //ProcedureExecute proc = new ProcedureExecute("usp_BOMProductEntryGet");
            ProcedureExecute proc = new ProcedureExecute("usp_BOMProductEntryGetNew");
            //End of rev Pratik
            proc.AddVarcharPara("@ACTION", 150, action);
            proc.AddVarcharPara("@BOMNO", 100, BOMNo);
            proc.AddDateTimePara("@BOMDATE", Convert.ToDateTime(BOMDate));
            proc.AddBigIntegerPara("@FINISHEDITEM",  FinishedItem);
            proc.AddDecimalPara("@FINISHEDQTY",4,18, FinishedQty);
            proc.AddVarcharPara("@FINISHEDUOM", 100, FinishedUom);
            proc.AddVarcharPara("@BOMTYPE", 150, BOMType);
            proc.AddVarcharPara("@REVISIONNO", 100, RevisionNo);
            proc.AddDateTimePara("@REVISIONDATE", Convert.ToDateTime(RevisionDate));
            proc.AddIntegerPara("@BRANCH_ID", Unit);
            proc.AddIntegerPara("@WAREHOUSEID", Warehouse);
            //proc.AddIntegerPara("@SLNO", SlNO);
            //proc.AddIntegerPara("@PRODUCTID", ProductId);
            proc.AddVarcharPara("@REMARKS", 500, Remarks);
            proc.AddDecimalPara("@TotalResourceCost", 2, 18, TotalResourceCost);
            //proc.AddDecimalPara("@PRODUCTQTY", 4,18, ProductQty);
            //proc.AddVarcharPara("@PRODUCTUOM", 100, ProductUOM);

            //proc.AddDecimalPara("@STOCKQTY", 4,18, StockQty);
            //proc.AddVarcharPara("@STOCKUOM", 100, StockUOM);
            //proc.AddBigIntegerPara("@PRODUCTSWAREHOUSEID", ProductsWarehouseID);
            ////@PRODUCTSWAREHOUSEID
            //proc.AddDecimalPara("@PRICE",2,18, Price);
            //proc.AddDecimalPara("@AMOUNT",2,18, Amount);
            //proc.AddVarcharPara("@TAGBOMNo", 100, BOMNo2);
            //proc.AddVarcharPara("@REVNO", 100, RevNo);
            //proc.AddVarcharPara("@REMARKS", 250, Remarks);
            proc.AddBigIntegerPara("@BOM_SCHEMAID", BOM_SCHEMAID);
            proc.AddDecimalPara("@ACTUALADDITIONALCOST", 2, 18, ActualAdditionalCost);
            proc.AddBigIntegerPara("@RPRODUCTION_ID", production_id);
            proc.AddBigIntegerPara("@RDETAILS_ID", details_id);
            proc.AddVarcharPara("@PartNo", 500,PartNo);
            proc.AddPara("@doc_Type", "BOM");
            proc.AddVarcharPara("@ProjectID", 50, ProjectID);
            //rev Pratik
            proc.AddPara("@MultiUOMDetails", MultiUomDetails);
            //End of rev Pratik
            proc.AddBigIntegerPara("@USERID", userid);
            proc.AddVarcharPara("@CompanyID", 50, strCompanyID);
            proc.AddVarcharPara("@FinYear", 50, FinYear);
            proc.AddBigIntegerPara("@MPS_ID", MPS_ID);

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
            proc.AddPara("@doc_Type", "BOM");
            ds = proc.GetTable();
            return ds;
        }

        public DataTable GetNumberingSchemaJobWorkOrder(string strCompanyID, string strBranchID, string strFinYear, string strType, string strIsSplit)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("usp_BOMEntryDataGet");
            proc.AddVarcharPara("@ACTION", 100, "GetNumberingSchemaJobWorkOrder");
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
            ProcedureExecute proc = new ProcedureExecute("usp_BOMEntryDataGet");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddBigIntegerPara("@DetailsID", DetailsID);
            proc.AddPara("@doc_Type", "BOM");
            ds = proc.GetTable();
            return ds;
        }
        

        public DataTable GetBOMStandardCost(String Action, Int64 DetailsID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("usp_BOMEntryDataGet");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddBigIntegerPara("@DetailsID", DetailsID);
            proc.AddPara("@doc_Type", "BOM");
            ds = proc.GetTable();
            return ds;
        }

        //public DataTable GetBOMProductEntryListByID(String Action, String BOMNo,String RevNo)
        //{
        //    DataTable ds = new DataTable();
        //    ProcedureExecute proc = new ProcedureExecute("usp_BOMEntryDataGet");
        //    proc.AddVarcharPara("@ACTION", 100, Action);
        //    proc.AddPara("@BOMNo", BOMNo);
        //    proc.AddPara("@RevNo", RevNo);
        //    ds = proc.GetTable();
        //    return ds;
        //}

        public DataTable GetProjectCode(String BRANCH)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_PROJECTCODE_CUSTOMER");
            proc.AddVarcharPara("@ACTION", 500, "PROJECTALL");          
            proc.AddVarcharPara("@BRANCH", 10, BRANCH);
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
        public DataTable GetProjectCodeFromBOM(String BRANCH, string QuoteId)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_PROJECTCODE_CUSTOMER");
            proc.AddVarcharPara("@ACTION", 500, "PROJECTForBOM");           
            proc.AddVarcharPara("@BRANCH", 10, BRANCH);
            proc.AddBigIntegerPara("@QuoteId", Convert.ToInt64(QuoteId));
            ds = proc.GetTable();
            return ds;
        }

    }
}