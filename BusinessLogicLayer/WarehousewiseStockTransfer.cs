using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BusinessLogicLayer
{
    public class WarehousewiseStockTransfer
    {
        public DataSet PopulateStockAdjustmentDetails()
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_WarehousewiseStockTransfer_details");
            proc.AddVarcharPara("@Action", 50, "GetLoadDetails");
            proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            return proc.GetDataSet();
        }
        public DataTable PopulateWHINADDNEW(string BranchId)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_WarehousewiseStockTransfer_details");
            proc.AddVarcharPara("@Action", 50, "GetWHAddNew");
            proc.AddIntegerPara("@BranchId", Convert.ToInt32(BranchId));
            return proc.GetTable();
        }
        public DataTable PopulateStock(string ProductId, string WarehouseID, string BranchId)
        {
            //Rev Rajdip 
            ProcedureExecute proc = new ProcedureExecute("Prc_WarehousewiseStockTransfer_details");
            proc.AddVarcharPara("@Action", 50, "GetStock");
            proc.AddIntegerPara("@ProductID", Convert.ToInt32(ProductId));
            proc.AddIntegerPara("@WarehouseId", Convert.ToInt32(WarehouseID));
            proc.AddIntegerPara("@BranchId", Convert.ToInt32(BranchId));
            return proc.GetTable();
            //ProcedureExecute proc = new ProcedureExecute("Prc_WarehousewiseStockTransfer_details");
            //proc.AddVarcharPara("@Action", 50, "GetAviableStockStock");
            //proc.AddIntegerPara("@BranchFrom", Convert.ToInt32(fromBranch));
            //proc.AddIntegerPara("@BranchTo", Convert.ToInt32(ToBranch));
            //proc.AddIntegerPara("@FromDate", Convert.ToInt32(Fromdate));
            //return proc.GetTable();
            //End Rev Rajdip
        }
        public DataTable PopulateStockforWarehousewiseStockTransffer(string ProductId, string WarehouseID, string BranchId, string fromBranch, string ToBranch, string Fromdate)
        {
            //Rev Rajdip 
            //ProcedureExecute proc = new ProcedureExecute("Prc_WarehousewiseStockTransfer_details");
            //proc.AddVarcharPara("@Action", 50, "GetStock");
            //proc.AddIntegerPara("@ProductID", Convert.ToInt32(ProductId));
            //proc.AddIntegerPara("@WarehouseId", Convert.ToInt32(WarehouseID));
            //proc.AddIntegerPara("@BranchId", Convert.ToInt32(BranchId));
            //return proc.GetTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_WarehousewiseStockTransfer_details");
            proc.AddVarcharPara("@Action", 50, "GetAviableStockStock");
            proc.AddIntegerPara("@BranchFrom", Convert.ToInt32(fromBranch));
            proc.AddIntegerPara("@BranchTo", Convert.ToInt32(ToBranch));
            proc.AddVarcharPara("@FromDate",20, Fromdate);
            proc.AddIntegerPara("@ProductID", Convert.ToInt32(ProductId));
            proc.AddIntegerPara("@WarehouseId", Convert.ToInt32(WarehouseID));
            proc.AddIntegerPara("@BranchId", Convert.ToInt32(BranchId));
            return proc.GetTable();
            //End Rev Rajdip
        }
        public DataTable PopulateDailyStock(string fromdate,string todate,string BranchId,string ProductId, string WarehouseID,string iscreateorpreview )
        {
            ProcedureExecute proc = new ProcedureExecute("PROC_MATERIALINOUTREGWH_REPORT");

            proc.AddVarcharPara("@COMPANYID", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FROMDATE", 10, fromdate);
            proc.AddVarcharPara("@TODATE", 10, todate);
            proc.AddVarcharPara("@FINYEAR", 9, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@TABLENAME", 50, "WareHouseBatchSerial Details");
            proc.AddVarcharPara("@BRANCH_ID", -1, BranchId);
            proc.AddVarcharPara("@PRODUCT_ID", -1, ProductId);
            proc.AddVarcharPara("@WAREHOUSE_ID", -1, WarehouseID);
            proc.AddVarcharPara("@ISCREATEORPREVIEW", 1, iscreateorpreview);
            proc.AddVarcharPara("@RPTMODULENAME", 30, "StockTrialWH");
            return proc.GetTable();
        }
        public DataTable GetNegativeStock(string ProductId)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_WarehousewiseStockTransfer_details");
            proc.AddVarcharPara("@Action", 50, "GetNegativeStock");
            proc.AddIntegerPara("@ProductID", Convert.ToInt32(ProductId));           
            return proc.GetTable();
        }
        public void AddEditWarehouseStockTransfer(string Mode, string SchemeId, string Adjustment_No, string Adjustment_Date, string Branch,string BranchTo,string Technician
           , string Entity, string Employee, string userId, ref int AdjustedId, ref string ReturnNumber,
         DataTable AdjustmentTable, DataTable MultiUOMDetails, DataTable Warehousedt, ref int ErrorCode, string Adj_id, DataTable PackingDetailsdt, string TransportationMode, string VehicleNo, string Remarks, Int64 Proj_id
            , string IndentRequisitionNo, string IndentRequisitionDate, string Is_Return, Int64 ToProj_id)
        {
            DataTable dsInst = new DataTable();

            string IndentRequisitionDateformate = string.Empty;
            if (!string.IsNullOrEmpty(IndentRequisitionDate))
            {                
                string Day = IndentRequisitionDate.Substring(0, 2);
                string Month = IndentRequisitionDate.Substring(3, 2);
                string Year = IndentRequisitionDate.Substring(6, 4);
                IndentRequisitionDateformate = Year + "-" + Month + "-" + Day;
            }

            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            //Rev  Mantis Issue 24428
            //SqlCommand cmd = new SqlCommand("prc_WarehouseStockTransfer_AddEdit", con);
            SqlCommand cmd = new SqlCommand("prc_WarehouseStockTransfer_AddEditNew", con);
            //End of Rev  Mantis Issue 24428
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", Mode);
            cmd.Parameters.AddWithValue("@SchemeId", SchemeId);
            cmd.Parameters.AddWithValue("@Adjustment_No", Adjustment_No);
            cmd.Parameters.AddWithValue("@Adjustment_Date", Adjustment_Date);
            cmd.Parameters.AddWithValue("@Branch", Branch);
            cmd.Parameters.AddWithValue("@BranchTo", BranchTo);
           // cmd.Parameters.AddWithValue("@SourceWarehouse_id", SourceWarehouse_id);
            //cmd.Parameters.AddWithValue("@DestWarehouse_id", DestWarehouse_id);            
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@DetailTable", AdjustmentTable);
            //Rev  Mantis Issue 24428
            cmd.Parameters.AddWithValue("@MultiUOMDetails", MultiUOMDetails);
            //End of Rev  Mantis Issue 24428

            cmd.Parameters.AddWithValue("@WarehouseDetail", Warehousedt);
            cmd.Parameters.AddWithValue("@Adj_id", Adj_id);           
            cmd.Parameters.AddWithValue("@FinYear", HttpContext.Current.Session["LastFinYear"].ToString());
            cmd.Parameters.AddWithValue("@CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
            cmd.Parameters.AddWithValue("@PackingDetails", PackingDetailsdt);
            cmd.Parameters.AddWithValue("@TransportationMode", TransportationMode);
            cmd.Parameters.AddWithValue("@VehicleNo", VehicleNo);
            cmd.Parameters.AddWithValue("@Remarks", Remarks);
            cmd.Parameters.AddWithValue("@Technician", Technician);
            cmd.Parameters.AddWithValue("@Project_Id", Proj_id);
            cmd.Parameters.AddWithValue("@Entitycnt_internalId", Entity);
            cmd.Parameters.AddWithValue("@EmployeeId", Employee);
            cmd.Parameters.AddWithValue("@BRIDs", IndentRequisitionNo);
            if (!string.IsNullOrEmpty(IndentRequisitionDateformate)) cmd.Parameters.AddWithValue("@BRDate", Convert.ToDateTime(IndentRequisitionDateformate).ToString("yyyy-MM-dd"));

           // cmd.Parameters.AddWithValue("@BRDate", Convert.ToDateTime(IndentRequisitionDate).ToString("yyyy-MM-dd"));

            cmd.Parameters.AddWithValue("@Is_Return", Is_Return);
            cmd.Parameters.AddWithValue("@ToProject_Id", ToProj_id);
            cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@ReturnId", SqlDbType.VarChar, 10);
            cmd.Parameters.Add("@ErrorCode", SqlDbType.Int);

            cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
            cmd.Parameters["@ReturnId"].Direction = ParameterDirection.Output;
            cmd.Parameters["@ErrorCode"].Direction = ParameterDirection.Output;

            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dsInst);

            AdjustedId = Convert.ToInt32(cmd.Parameters["@ReturnId"].Value);
            ReturnNumber = Convert.ToString(cmd.Parameters["@ReturnValue"].Value);
            ErrorCode = Convert.ToInt32(cmd.Parameters["@ErrorCode"].Value);
        }

        public DataSet GetEditedWHSTData(string AdjId, string multiwarehouse)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_WarehousewiseStockTransfer_details");
            proc.AddVarcharPara("@Action", 50, "GetEditedData");
            proc.AddVarcharPara("@AdjId", 10, AdjId);
            proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            proc.AddVarcharPara("@Multiwarehouse", 500, Convert.ToString(multiwarehouse));
            return proc.GetDataSet();
        }

        public int DeleteAdj(string AdjId,string WDelete)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("Prc_WarehousewiseStockTransfer_details");
            proc.AddVarcharPara("@Action", 50, "Delete");
            proc.AddVarcharPara("@AdjId", 10, AdjId);
            proc.AddVarcharPara("@WarnDelete", 10, WDelete);
            proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
           
            return rtrnvalue;
        }
        //Rev work start 12.07.2022 mantise no :0025011: Update E-way Bill
        public int UpdateEWayBillFor(string Stk_Id, string EWayBillNumber, string EWayBillDate)
        {
            try
            {
                int i;
                int rtrnvalue = 0;
                ProcedureExecute proc = new ProcedureExecute("prc_UpdateEwayBillNo");
                proc.AddVarcharPara("@Action", 100, "UpdateEWayBill");
                proc.AddVarcharPara("@Module", 100, "Warehouse_Wise_Stock_Transfer");
                proc.AddBigIntegerPara("@Stk_Id", Convert.ToInt32(Stk_Id));
                proc.AddVarcharPara("@EWayBillNumber", 50, EWayBillNumber);
                proc.AddVarcharPara("@EWayBillDate", 50, EWayBillDate);
                proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
                i = proc.RunActionQuery();
                rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));
                return rtrnvalue;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        //Rev work close 12.07.2022 mantise no :0025011: Update E-way Bill

    }
}
