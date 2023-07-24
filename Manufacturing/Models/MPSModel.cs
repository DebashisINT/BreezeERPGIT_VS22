//================================================== Revision History =============================================
//Rev Number         DATE              VERSION          DEVELOPER           CHANGES
//1.0                24-07-2023        2.0.39           Priti              0026599: Auto Selection of BOM is required in MPS Based on Settings
//====================================================== Revision History =============================================
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Manufacturing.Models
{
    public class MPSModel
    {
        string ConnectionString = String.Empty;

        public MPSModel()
        {
            ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }

        public DataSet DropDownDetailForEstimate(String Action, String FinYear, String CompanyID, String userbranchlist, Int32 branchid, Int32 id)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("usp_MPSEntryDataGet");
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
       
        public DataSet MPSProductEntryInsertUpdate(String action, String Estimate_No, DateTime? EstimateDATE
            ,Int32 Unit, DataTable dtEstimate_PRODUCTS, Int32 Estimate_SchemaID,
             String customer_id, String ContractNo, String LastCompany, String LastFinYear, Int64 userid = 0, Int64 details_id = 0
              , String EstimateStartDate_dt = null, String EstimateEndDate_dt = null, String ActualsStartDate_dt = null, String ActualsEndDate_dt = null
            )
        {
            DataSet ds = new DataSet();
            try
            {


                ProcedureExecute proc = new ProcedureExecute("PRC_MPSProductEntryGet");
                proc.AddVarcharPara("@ACTION", 150, action);
                proc.AddVarcharPara("@Estimate_No", 100, Estimate_No);
                proc.AddDateTimePara("@EstimateDATE", Convert.ToDateTime(EstimateDATE));
               
               
                proc.AddIntegerPara("@BRANCH_ID", Unit);
                proc.AddBigIntegerPara("@Estimate_SchemaID", Estimate_SchemaID);
                

                
                proc.AddBigIntegerPara("@RDETAILS_ID", details_id);
               
                proc.AddVarcharPara("@customer_id", 500, customer_id);
                proc.AddVarcharPara("@ContractNo", 50, ContractNo);
                
                proc.AddBigIntegerPara("@USERID", userid);
                

                proc.AddPara("@CompanyID", LastCompany);
                proc.AddPara("@FinYear", LastFinYear);
                proc.AddPara("@doc_Type", "Estimate");

                if (action == "INSERTMAINPRODUCT" || action == "UPDATEMAINPRODUCT")
                {
                    if (dtEstimate_PRODUCTS.Columns.Contains("SrlNo"))
                    {
                        dtEstimate_PRODUCTS.Columns.Remove("SrlNo");
                    }
                    if (dtEstimate_PRODUCTS.Columns.Contains("StkUOM"))
                    {
                        dtEstimate_PRODUCTS.Columns.Remove("StkUOM");
                    }
                    proc.AddPara("@UDTEstimate_PRODUCTS", dtEstimate_PRODUCTS);

                   
                }


                proc.AddVarcharPara("@EstimateStartDate",50, Convert.ToString(EstimateStartDate_dt));
                proc.AddVarcharPara("@EstimateEndDate",50, Convert.ToString(EstimateEndDate_dt));
                proc.AddVarcharPara("@ActualsStartDate",50, Convert.ToString(ActualsStartDate_dt));
                proc.AddVarcharPara("@ActualsEndDate",50, Convert.ToString(ActualsEndDate_dt));
                
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
            ProcedureExecute proc = new ProcedureExecute("usp_MPSEntryDataGet");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddBigIntegerPara("@DetailsID", DetailsID);
           // proc.AddPara("@doc_Type", "Estimate");
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
        public DataTable GetContractCode(String CustomerID, String BRANCH)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("usp_MPSEntryDataGet");
            proc.AddVarcharPara("@ACTION", 500, "CONTRACTORDER");
            proc.AddVarcharPara("@CUSTOMER_ID", 50, CustomerID);
            proc.AddVarcharPara("@BranchId", 10, BRANCH);
           // proc.AddVarcharPara("@Proj_id", 10, ProjectID);
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
        //Rev 1.0
        public DataTable GetParentBOM(string Branch)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("usp_MPSEntryDataGet");
            proc.AddVarcharPara("@ACTION", 100, "GetBOMList");
            proc.AddBigIntegerPara("@BRANCHID", Convert.ToInt32(Branch));           
            ds = proc.GetTable();
            return ds;
        }
        public DataTable GetParentBOM(string Branch, string ProductID)
        //Rev 1.0 End
        {            
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("usp_MPSEntryDataGet");
            proc.AddVarcharPara("@ACTION", 100, "GetBOMList");
            proc.AddBigIntegerPara("@BRANCHID", Convert.ToInt32(Branch));
            //Rev 1.0
            proc.AddBigIntegerPara("@Product_ID", Convert.ToInt32(ProductID));
            //Rev 1.0 End
            ds = proc.GetTable();
            return ds;
        }
    }
}