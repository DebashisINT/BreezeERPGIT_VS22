using DataAccessLayer;
using Manufacturing.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
namespace Manufacturing.Models
{
    public class IssueReturnModel
    {
         string ConnectionString = String.Empty;
         public IssueReturnModel()
        {
            ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }

         public DataTable GetIssueReturnData(string Action = null, Int64 IssueReturnID = 0, Int64 DetailsID = 0)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("usp_IssueReturnDataGet");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddPara("@IssueReturnID", IssueReturnID);
            proc.AddPara("@DetailsID", DetailsID);
            ds = proc.GetTable();
            return ds;
        }

         public DataTable GetMaterissueIssueReturnData(string Action, Int64 IssueReturnID, DateTime dtOrderDate)
         {
             DataTable ds = new DataTable();
             ProcedureExecute proc = new ProcedureExecute("usp_IssueReturnDataGet");
             proc.AddVarcharPara("@ACTION", 100, Action);
             proc.AddBigIntegerPara("@IssueReturnID", IssueReturnID);
             if (Convert.ToString(dtOrderDate) !="" && Convert.ToString(dtOrderDate) != "01-01-0001 00:00:00")
             proc.AddVarcharPara("@MatissuedtOrderDate", 10, dtOrderDate.ToString("yyyy-MM-dd"));
        
             ds = proc.GetTable();
             return ds;
         }

         public DataTable GetFGReturnData(string Action, Int64 IssueReturnID, DateTime dtOrderDate)
         {
             DataTable ds = new DataTable();
             ProcedureExecute proc = new ProcedureExecute("usp_ReturnFGDataGet");
             proc.AddVarcharPara("@ACTION", 100, Action);
             proc.AddBigIntegerPara("@IssueReturnID", IssueReturnID);
             if (Convert.ToString(dtOrderDate) != "" && Convert.ToString(dtOrderDate) != "01-01-0001 00:00:00")
                 proc.AddVarcharPara("@MatissuedtOrderDate", 10, dtOrderDate.ToString("yyyy-MM-dd"));

             ds = proc.GetTable();
             return ds;
         }

        public DataTable GetManufacturingProductionIssue(string Action = null, String ProductID = null, String LastFinYear = null, String Branch = null, String LastCompany = null, String multiwarehouse = null, String warehouseid = null, String BatchID = null, String Row_No = null, String SC_Date = null)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_ManufacturingProductionIssue_Get");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@ProductID", 500, ProductID);
            proc.AddVarcharPara("@FinYear", 500, LastFinYear);
            proc.AddVarcharPara("@branchId", 2000, Branch);
            proc.AddVarcharPara("@companyId", 500, LastCompany);
            proc.AddVarcharPara("@Multiwarehouse", 500, multiwarehouse);
            proc.AddVarcharPara("@WarehouseID", 500, warehouseid);
            proc.AddVarcharPara("@BatchID", 10, BatchID);
            proc.AddVarcharPara("@Row_No", 100, Row_No);
            proc.AddVarcharPara("@SC_Date", 10, SC_Date);
            ds = proc.GetTable();
            return ds;
        }

        public DataSet IssueReturnBOMProductInsertUpdate(String action, Int64 IssueReturnID, Int64 ProductionIssueID, Int64 WorkOrderID, Int64 ProductionOrderID, Int64 Details_ID, Int64 WorkCenterID, String Issue_No, Int64 Issue_SchemaID, DateTime Issue_Date,
        Decimal Issue_Qty, Decimal TotalCost, Int64 BRANCH_ID, Int64 userid,String CompanyID,String FinYear,String Remarks, string PartNo, DataTable dtBOM_PRODUCTS,DataTable dtWarehouseFresh, DataTable dtWarehouse)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("usp_IssueReturnInsertUpdate");
            proc.AddVarcharPara("@ACTION", 150, action);
            proc.AddPara("@IssueReturnID", IssueReturnID);
            proc.AddPara("@ProductionIssueID", ProductionIssueID);
            proc.AddPara("@WorkOrderID", WorkOrderID);
            proc.AddPara("@ProductionOrderID", ProductionOrderID);
            proc.AddPara("@Issue_No", Issue_No);
            proc.AddPara("@Issue_SchemaID", Issue_SchemaID);
            proc.AddPara("@WorkCenterID", WorkCenterID);
            proc.AddPara("@Issue_Date", Issue_Date);
            proc.AddPara("@Issue_Qty", Issue_Qty);
            proc.AddPara("@Details_ID", Details_ID);
            proc.AddPara("@TotalCost", TotalCost);
            proc.AddPara("@BRANCH_ID", BRANCH_ID);
            proc.AddPara("@Remarks", Remarks);
            proc.AddPara("@UserID", userid);
            proc.AddPara("@CompanyID", CompanyID);
            proc.AddPara("@FinYear", FinYear);
            proc.AddVarcharPara("@PartNo", 500, PartNo);
            proc.AddPara("@UDTPRODUCTIONORDER_DETAILS", dtBOM_PRODUCTS);
            if (dtWarehouse.Rows.Count > 0)
            {

                foreach (DataRow dr in dtWarehouse.Rows)
                {
                    IFormatProvider culture = new CultureInfo("en-US", true);

                    string _ViewMfgDate = Convert.ToString(dr["ViewMfgDate"]);
                    DateTime dateVal4 = DateTime.ParseExact(_ViewMfgDate, "dd-MM-yyyy", culture);
                    string ViewMfgDate = dateVal4.ToString("yyyy-MM-dd");
                    //string ViewMfgDate = Convert.ToDateTime(_ViewMfgDate).ToString("yyyy-MM-dd");

                    string _ViewExpiryDate = Convert.ToString(dr["ViewExpiryDate"]);
                    DateTime dateVal5 = DateTime.ParseExact(_ViewExpiryDate, "dd-MM-yyyy", culture);
                    string ViewExpiryDate = dateVal5.ToString("yyyy-MM-dd");

                    if (ViewMfgDate != null)
                    {
                        dr["ViewMfgDate"] = ViewMfgDate;
                    }
                    if (ViewExpiryDate != null)
                    {
                        dr["ViewExpiryDate"] = ViewExpiryDate;
                    }

                }
                dtWarehouse.AcceptChanges();


                proc.AddPara("@UDTWAREHOUSE_DETAILS", dtWarehouse);
            }
            if (dtWarehouseFresh.Rows.Count > 0)
            {
                proc.AddPara("@UDTWAREHOUSE_DETAILSFresh", dtWarehouseFresh);
            }
            //proc.AddPara("@UDTWarehouse", dtWarehouse);
            ds = proc.GetDataSet();
            return ds;
        }
    }
}