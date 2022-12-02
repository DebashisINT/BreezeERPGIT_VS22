using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PMS.Models
{
    public class BillofQuantitiesModel
    {
        string ConnectionString = String.Empty;
        public BillofQuantitiesModel()
        {
            ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }

        public DataSet DropDownDetailForBOQ(String Action, String FinYear, String CompanyID, String userbranchlist, Int32 branchid, Int32 id)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("usp_BOQEntryDataGet");
            proc.AddVarcharPara("@Action", 100, Action);
            proc.AddVarcharPara("@FinYear", 500, FinYear);
            proc.AddVarcharPara("@CompanyID", 500, CompanyID);
            proc.AddVarcharPara("@userbranchlist", 5000, userbranchlist);
            proc.AddIntegerPara("@BRANCHID", branchid);
            proc.AddIntegerPara("@ID", id);
            proc.AddPara("@doc_Type", "BOQ");
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet BOQProductEntryInsertUpdate(String action, String BOQ_No, DateTime? BOQDATE, String RevisionNo, DateTime? RevisionDate,
            Int32 Unit, DataTable dtBOQ_PRODUCTS, DataTable dtBOQ_RESOURCES, Int32 BOQ_SchemaID, Decimal ActualAdditionalCost, String Proposal_ID, String Quotation_ID,
            String HeadRemarks, String customer_id, String ContractNo, String ProjectID, String TaxID, String Approve, String BOQStatus, String Approved_Remarks,String ApprovRevSettings, DataTable dtProductAddlDesc,
            DataTable dtResourceAddlDesc,String EstimateID,String TaggingModuleSave, Int64 userid = 0, Int32 production_id = 0, Int64 details_id = 0)
        {
            DataSet ds = new DataSet();
            try
            {
                if (ApprovRevSettings == "No")
                {
                    BOQStatus = "1";
                    Approve = "Approve";
                }

                ProcedureExecute proc = new ProcedureExecute("PRC_PMSBOQProductEntryGet");
                proc.AddVarcharPara("@ACTION", 150, action);
                proc.AddVarcharPara("@BOQ_No", 100, BOQ_No);
                proc.AddDateTimePara("@BOQDATE", Convert.ToDateTime(BOQDATE));
                proc.AddVarcharPara("@REVISIONNO", 100, RevisionNo);
                proc.AddDateTimePara("@REVISIONDATE", Convert.ToDateTime(RevisionDate));
                proc.AddIntegerPara("@BRANCH_ID", Unit);
                proc.AddBigIntegerPara("@BOQ_SchemaID", BOQ_SchemaID);
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
                proc.AddVarcharPara("@BOQStatus", 500, BOQStatus);
                proc.AddVarcharPara("@Approved_Remarks", 500, Approved_Remarks);
                proc.AddVarcharPara("@EstimateID", 50, EstimateID);
                proc.AddVarcharPara("@TaggingModuleSave", 500, TaggingModuleSave);
                proc.AddPara("@doc_Type", "BOQ");

                if (action == "INSERTMAINPRODUCT" || action == "UPDATEMAINPRODUCT")
                {
                    proc.AddPara("@UDTBOQ_PRODUCTS", dtBOQ_PRODUCTS);

                    proc.AddPara("@UDTBOQ_RESOURCES", dtBOQ_RESOURCES);

                    proc.AddPara("@UDTAddlDesc_PRODUCTS", dtProductAddlDesc);

                    proc.AddPara("@UDTAddlDesc_RESOURCES", dtResourceAddlDesc);
                }
                if (action == "INSERTRESOURCES")
                {
                    // proc.AddPara("@UDTBOQ_RESOURCES", dtBOQ_RESOURCES);
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
            ProcedureExecute proc = new ProcedureExecute("usp_BOQEntryDataGet");
            proc.AddVarcharPara("@ACTION", 100, "GetNumberingSchema");
            proc.AddVarcharPara("@CompanyID", 100, strCompanyID);
            //proc.AddVarcharPara("@BranchID", 100, strBranchID);
            proc.AddVarcharPara("@strBranchID", 2000, strBranchID);
            proc.AddVarcharPara("@FinYear", 100, strFinYear);
            proc.AddVarcharPara("@Type", 100, strType);
            proc.AddVarcharPara("@IsSplit", 100, strIsSplit);
            proc.AddPara("@doc_Type", "BOQ");
            ds = proc.GetTable();
            return ds;
        }

        public DataTable GetBOQProductEntryListByID(String Action, Int64 DetailsID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("usp_BOQEntryDataGet");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddBigIntegerPara("@DetailsID", DetailsID);
            proc.AddPara("@doc_Type", "BOQ");
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

        public DataSet CancelReOpenForBOQ(String Action, String Cancel_Remarks, Int64 USERID, Int32 id)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("usp_BOQEntryDataGet");
            proc.AddVarcharPara("@Action", 100, Action);
            proc.AddVarcharPara("@Cancel_Remarks", 500, Cancel_Remarks);
            proc.AddIntegerPara("@ID", id);
            proc.AddBigIntegerPara("@USERID", USERID);
            proc.AddPara("@doc_Type", "BOQ");
            ds = proc.GetDataSet();
            return ds;
        }

        public DataTable GetEstimateCode(String CustomerID, String BRANCH, String DOC_No, DateTime? BOQDATE)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_PROJECTCODE_CUSTOMER");
            proc.AddVarcharPara("@ACTION", 500, "ESTIMATE");
            proc.AddVarcharPara("@CUSTOMER_ID", 50, CustomerID);
            proc.AddVarcharPara("@BRANCH", 10, BRANCH);
            proc.AddVarcharPara("@DOC_No", 50, DOC_No);
            proc.AddDateTimePara("@DOC_DATE", Convert.ToDateTime(BOQDATE));
            ds = proc.GetTable();
            return ds;
        }

        public DataTable GetTagProducts(String EstimateID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_PROJECTCODE_CUSTOMER");
            proc.AddVarcharPara("@ACTION", 500, "TagProduct");
            proc.AddVarcharPara("@EstimateID", 50, EstimateID);
            ds = proc.GetTable();
            return ds;
        }

        public DataTable GetBOQProductTagListByID(String Action, String DetailsID)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("usp_BOQEntryDataGet");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddVarcharPara("@TAGPRODID", 500, DetailsID);
            proc.AddPara("@doc_Type", "BOQ");
            ds = proc.GetTable();
            return ds;
        }

        public DataSet TaggingDetails(String Action, Int32 id,String tagid)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("usp_BOQEntryDataGet");
            proc.AddVarcharPara("@Action", 100, Action);
            proc.AddIntegerPara("@ID", id);
            proc.AddVarcharPara("@TAGPRODID", 500, tagid);
            proc.AddPara("@doc_Type", "Estimate");
            ds = proc.GetDataSet();
            return ds;
        }

        public DataSet CheckEstimateBalQty(String Action, Int32 id)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("usp_BOQEntryDataGet");
            proc.AddVarcharPara("@Action", 100, Action);
            proc.AddIntegerPara("@ID", id);
            ds = proc.GetDataSet();
            return ds;
        }

        public DataTable GetProposalCode(String CustomerID, String BRANCH, String DOC_No, DateTime? BOQDATE)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_PROJECTCODE_CUSTOMER");
            proc.AddVarcharPara("@ACTION", 500, "PROPOSAL");
            proc.AddVarcharPara("@CUSTOMER_ID", 50, CustomerID);
            proc.AddVarcharPara("@BRANCH", 10, BRANCH);
            proc.AddVarcharPara("@DOC_No", 50, DOC_No);
            proc.AddDateTimePara("@DOC_DATE", Convert.ToDateTime(BOQDATE));
            ds = proc.GetTable();
            return ds;
        }

        public DataTable GetTagProposalProducts(String Proposal)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("PRC_PROJECTCODE_CUSTOMER");
            proc.AddVarcharPara("@ACTION", 500, "TagProposalProduct");
            proc.AddVarcharPara("@EstimateID", 50, Proposal);
            ds = proc.GetTable();
            return ds;
        }
    }
}