using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PMS.Models
{
    public class EstimateModel
    {
        string ConnectionString = String.Empty;
        public EstimateModel()
        {
            ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }

        public DataSet DropDownDetailForEstimate(String Action, String FinYear, String CompanyID, String userbranchlist, Int32 branchid, Int32 id)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("usp_EstimateEntryDataGet");
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

        public DataSet EstimateProductEntryInsertUpdate(String action, String Estimate_No, DateTime? EstimateDATE, String RevisionNo, DateTime? RevisionDate,
            Int32 Unit, DataTable dtEstimate_PRODUCTS, DataTable dtEstimate_RESOURCES, Int32 Estimate_SchemaID, Decimal ActualAdditionalCost, String Proposal_ID, String Quotation_ID,
            String HeadRemarks, String customer_id, String ContractNo, String ProjectID, String TaxID, String Approve, String EstimateStatus, String Approved_Remarks, String ApprovRevSettings, DataTable dtProductAddlDesc,
            DataTable dtResourceAddlDesc, String LastCompany, String LastFinYear, Int64 userid = 0, Int32 production_id = 0, Int64 details_id = 0)
        {
            DataSet ds = new DataSet();
            try
            {
                if (ApprovRevSettings == "No")
                {
                    EstimateStatus = "1";
                    Approve = "Approve";
                }

                ProcedureExecute proc = new ProcedureExecute("PRC_PMSEstimateProductEntryGet");
                proc.AddVarcharPara("@ACTION", 150, action);
                proc.AddVarcharPara("@Estimate_No", 100, Estimate_No);
                proc.AddDateTimePara("@EstimateDATE", Convert.ToDateTime(EstimateDATE));
                proc.AddVarcharPara("@REVISIONNO", 100, RevisionNo);
                proc.AddDateTimePara("@REVISIONDATE", Convert.ToDateTime(RevisionDate));
                proc.AddIntegerPara("@BRANCH_ID", Unit);
                proc.AddBigIntegerPara("@Estimate_SchemaID", Estimate_SchemaID);
                proc.AddDecimalPara("@ACTUALADDITIONALCOST", 2, 18, ActualAdditionalCost);

                proc.AddBigIntegerPara("@RPRODUCTION_ID", production_id);
                proc.AddBigIntegerPara("@RDETAILS_ID", details_id);
                proc.AddVarcharPara("@Proposal_ID", 100, Proposal_ID);
                proc.AddVarcharPara("@Quotation_ID", 100, Quotation_ID);
                proc.AddVarcharPara("@HeadRemarks", 500, HeadRemarks);
                proc.AddVarcharPara("@customer_id", 500, customer_id);
                proc.AddVarcharPara("@ContractNo", 50, ContractNo);
                proc.AddVarcharPara("@ProjectID", 50, ProjectID);
                proc.AddVarcharPara("@TaxID", 10, TaxID);
                proc.AddBigIntegerPara("@USERID", userid);
                proc.AddVarcharPara("@Approve", 500, Approve);
                proc.AddVarcharPara("@EstimateStatus", 500, EstimateStatus);
                proc.AddVarcharPara("@Approved_Remarks", 500, Approved_Remarks);

                proc.AddPara("@CompanyID", LastCompany);
                proc.AddPara("@FinYear", LastFinYear);
                proc.AddPara("@doc_Type", "Estimate");

                if (action == "INSERTMAINPRODUCT" || action == "UPDATEMAINPRODUCT")
                {
                    proc.AddPara("@UDTEstimate_PRODUCTS", dtEstimate_PRODUCTS);

                    proc.AddPara("@UDTEstimate_RESOURCES", dtEstimate_RESOURCES);

                    proc.AddPara("@UDTAddlDesc_PRODUCTS", dtProductAddlDesc);

                    proc.AddPara("@UDTAddlDesc_RESOURCES", dtResourceAddlDesc);
                }
                if (action == "INSERTRESOURCES")
                {
                    // proc.AddPara("@UDTEstimate_RESOURCES", dtEstimate_RESOURCES);
                }
                ds = proc.GetDataSet();
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }

        public DataTable GetNumberingSchema(string strCompanyID, string strBranchID, string strFinYear, string strType, string strIsSplit)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("usp_EstimateEntryDataGet");
            proc.AddVarcharPara("@ACTION", 100, "GetNumberingSchema");
            proc.AddVarcharPara("@CompanyID", 100, strCompanyID);
            //proc.AddVarcharPara("@BranchID", 100, strBranchID);
            proc.AddVarcharPara("@strBranchID", 2000, strBranchID);
            proc.AddVarcharPara("@FinYear", 100, strFinYear);
            proc.AddVarcharPara("@Type", 100, strType);
            proc.AddVarcharPara("@IsSplit", 100, strIsSplit);
            proc.AddPara("@doc_Type", "Estimate");
            ds = proc.GetTable();
            return ds;
        }

        public DataTable GetEstimateProductEntryListByID(String Action, Int64 DetailsID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("usp_EstimateEntryDataGet");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddBigIntegerPara("@DetailsID", DetailsID);
            proc.AddPara("@doc_Type", "Estimate");
            ds = proc.GetTable();
            return ds;
        }

        public DataTable GetProjectCode(String CustomerID, String BRANCH)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_PROJECTCODE_CUSTOMER");
            proc.AddVarcharPara("@ACTION", 500, "PROJECT");
            proc.AddVarcharPara("@CUSTOMER_ID", 50, CustomerID);
            proc.AddVarcharPara("@BRANCH", 10, BRANCH);
            ds = proc.GetTable();
            return ds;
        }

        public DataTable GetProjectCodeFromQuotation(String CustomerID, String BRANCH, string QuoteId)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_PROJECTCODE_CUSTOMER");
            proc.AddVarcharPara("@ACTION", 500, "PROJECTForQuotation");
            proc.AddVarcharPara("@CUSTOMER_ID", 50, CustomerID);
            proc.AddVarcharPara("@BRANCH", 10, BRANCH);
            proc.AddBigIntegerPara("@QuoteId", Convert.ToInt64(QuoteId));
            ds = proc.GetTable();
            return ds;
        }
        public DataTable GetContractCode(String CustomerID, String BRANCH, String ProjectID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_PROJECTCODE_CUSTOMER");
            proc.AddVarcharPara("@ACTION", 500, "CONTRACTORDER");
            proc.AddVarcharPara("@CUSTOMER_ID", 50, CustomerID);
            proc.AddVarcharPara("@BRANCH", 10, BRANCH);
            proc.AddVarcharPara("@Proj_id", 10, ProjectID);
            ds = proc.GetTable();
            return ds;
        }

        public DataSet CancelReOpenForEstimate(String Action, String Cancel_Remarks, Int64 USERID, Int32 id)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("usp_EstimateEntryDataGet");
            proc.AddVarcharPara("@Action", 100, Action);
            proc.AddVarcharPara("@Cancel_Remarks", 500, Cancel_Remarks);
            proc.AddIntegerPara("@ID", id);
            proc.AddBigIntegerPara("@USERID", USERID);
            proc.AddPara("@doc_Type", "Estimate");
            ds = proc.GetDataSet();
            return ds;
        }

        public DataTable EstimateApproval(String DetailsId, String Userid)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_ApprovalCheckforMultiUserApproval");
            proc.AddVarcharPara("@Action", 500, "Project Estimate");
            proc.AddVarcharPara("@User_Id", 50, Userid);
            proc.AddVarcharPara("@ModuleId", 50, DetailsId);
            ds = proc.GetTable();
            return ds;
        }
    }
}