using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PMS.Models
{
    public class ContractOrderModel
    {
        string ConnectionString = String.Empty;
        public ContractOrderModel()
        {
            ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }

        public DataSet DropDownDetailForContract(String Action, String FinYear, String CompanyID, String userbranchlist, Int32 branchid, Int32 id)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("usp_ContractEntryDataGet");
            proc.AddVarcharPara("@Action", 100, Action);
            proc.AddVarcharPara("@FinYear", 500, FinYear);
            proc.AddVarcharPara("@CompanyID", 500, CompanyID);
            proc.AddVarcharPara("@userbranchlist", 5000, userbranchlist);
            proc.AddIntegerPara("@BRANCHID", branchid);
            proc.AddIntegerPara("@ID", id);
            proc.AddPara("@doc_Type", "ContractOrder");
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet ComtractProductEntryInsertUpdate(String action, String ContractNo, DateTime? ContractDate,String RevisionNo, DateTime? RevisionDate,
            Int32 Unit,DataTable dtContarct_PRODUCTS, DataTable dtContract_RESOURCES, Int32 BOM_SCHEMAID, Decimal ActualAdditionalCost,
             String  BOQ_ID, String Estimate_ID, String Proposal_ID, String Quotation_ID, String HeadRemarks,Int64 userid = 0, Int32 production_id = 0, Int64 details_id = 0)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("PRC_PMSContractProductEntryGet");
            proc.AddVarcharPara("@ACTION", 150, action);
            proc.AddVarcharPara("@Contract_No", 100, ContractNo);
            proc.AddDateTimePara("@ContractDATE", Convert.ToDateTime(ContractDate));
            proc.AddVarcharPara("@REVISIONNO", 100, RevisionNo);
            proc.AddDateTimePara("@REVISIONDATE", Convert.ToDateTime(RevisionDate));
            proc.AddIntegerPara("@BRANCH_ID", Unit);
            proc.AddBigIntegerPara("@Contract_SchemaID", BOM_SCHEMAID);
            proc.AddDecimalPara("@ACTUALADDITIONALCOST", 2, 18, ActualAdditionalCost);

            proc.AddBigIntegerPara("@RPRODUCTION_ID", production_id);
            proc.AddBigIntegerPara("@RDETAILS_ID", details_id);
            proc.AddVarcharPara("@BOQ_ID", 100, BOQ_ID);
            proc.AddVarcharPara("@Estimate_ID", 100, Estimate_ID);
            proc.AddVarcharPara("@Proposal_ID", 100, Proposal_ID);
            proc.AddVarcharPara("@Quotation_ID", 100, Quotation_ID);
            proc.AddVarcharPara("@HeadRemarks", 500, HeadRemarks);

            proc.AddBigIntegerPara("@USERID", userid);

            proc.AddPara("@doc_Type", "ContractOrder");

            if (action == "INSERTMAINPRODUCT" || action == "UPDATEMAINPRODUCT")
            {
                proc.AddPara("@UDTContract_PRODUCTS", dtContarct_PRODUCTS);
            }
            if (action == "INSERTRESOURCES")
            {
                proc.AddPara("@UDTContract_RESOURCES", dtContract_RESOURCES);
            }
            ds = proc.GetDataSet();
            return ds;
        }

        public DataTable GetNumberingSchema(string strCompanyID, string strBranchID, string strFinYear, string strType, string strIsSplit)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("usp_ContractEntryDataGet");
            proc.AddVarcharPara("@ACTION", 100, "GetNumberingSchema");
            proc.AddVarcharPara("@CompanyID", 100, strCompanyID);
            //proc.AddVarcharPara("@BranchID", 100, strBranchID);
            proc.AddVarcharPara("@strBranchID", 2000, strBranchID);
            proc.AddVarcharPara("@FinYear", 100, strFinYear);
            proc.AddVarcharPara("@Type", 100, strType);
            proc.AddVarcharPara("@IsSplit", 100, strIsSplit);
            proc.AddPara("@doc_Type", "ContractOrder");
            ds = proc.GetTable();
            return ds;
        }

        public DataTable GetBOMProductEntryListByID(String Action, Int64 DetailsID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("usp_ContractEntryDataGet");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddBigIntegerPara("@DetailsID", DetailsID);
            proc.AddPara("@doc_Type", "ContractOrder");
            ds = proc.GetTable();
            return ds;
        }
    }
}