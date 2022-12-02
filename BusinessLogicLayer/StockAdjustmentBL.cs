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
    public class StockAdjustmentBL
    {

        public DataSet PopulateStockAdjustmentDetails()
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_StockAdjustment_details");
            proc.AddVarcharPara("@Action", 50, "GetLoadDetails");
            proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            return proc.GetDataSet();
        }
        public DataTable PopulateStock(string ProductId, string WarehouseID, string BranchId)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_StockAdjustment_details");
            proc.AddVarcharPara("@Action", 50, "GetStock");
            proc.AddIntegerPara("@ProductID", Convert.ToInt32(ProductId));
            proc.AddIntegerPara("@WarehouseId", Convert.ToInt32(WarehouseID));
            proc.AddIntegerPara("@BranchId", Convert.ToInt32(BranchId));
            return proc.GetTable();
        }
        public DataTable PopulateProductUOM(string ProductId)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_StockAdjustment_details");
            proc.AddVarcharPara("@Action", 50, "GetProductUOM");
            proc.AddIntegerPara("@ProductID", Convert.ToInt32(ProductId));           
            return proc.GetTable();
        }
        
        public void AddEditStockAdjustment(string Mode, string SchemeId, string Adjustment_No, DateTime Adjustment_Date, string Branch, string Warehouse_ID,
        string ProductId, string StockInHand, string EnterAdjustQty, string TotalStockInHand, string Reason,
        string userId, string EnterRate,string Technician,Int64 ProjId, ref int AdjustedId, ref string ReturnNumber, ref int ErrorCode, string Adj_id, string StockUOM, string PackingUOM, string PackingQty)
        {
            DataTable dsInst = new DataTable();            
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("[prc_StockAdjustment_AddEdit]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", Mode);
            cmd.Parameters.AddWithValue("@SchemeId", SchemeId);
            cmd.Parameters.AddWithValue("@Adjustment_No", Adjustment_No);
            cmd.Parameters.AddWithValue("@Adjustment_Date", Adjustment_Date);
            cmd.Parameters.AddWithValue("@Branch", Branch);
            cmd.Parameters.AddWithValue("@Warehouse_ID", Warehouse_ID);
            cmd.Parameters.AddWithValue("@ProductId", ProductId);
            cmd.Parameters.AddWithValue("@StockInHand", StockInHand);
            cmd.Parameters.AddWithValue("@EnterAdjustQty", EnterAdjustQty);
            cmd.Parameters.AddWithValue("@TotalStockInHand", TotalStockInHand);
            cmd.Parameters.AddWithValue("@Reason", Reason);         
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@EnterRate", EnterRate);  
            cmd.Parameters.AddWithValue("@Adj_id", Adj_id);            
            cmd.Parameters.AddWithValue("@FinYear", HttpContext.Current.Session["LastFinYear"].ToString());
            cmd.Parameters.AddWithValue("@CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
            cmd.Parameters.AddWithValue("@StockUOM", StockUOM);
            cmd.Parameters.AddWithValue("@PackingUOM", PackingUOM);
            cmd.Parameters.AddWithValue("@PackingQty", PackingQty);
            cmd.Parameters.AddWithValue("@Technician_ID", Technician);
            cmd.Parameters.AddWithValue("@Project_Id", ProjId);
            cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 50);
            //Rev Subhra 24-07-2019 
            //cmd.Parameters.Add("@ReturnId", SqlDbType.VarChar, 10);
            //cmd.Parameters.Add("@ErrorCode", SqlDbType.Int);
            cmd.Parameters.Add("@ReturnId", SqlDbType.Int);
            cmd.Parameters.Add("@ErrorCode", SqlDbType.VarChar, 10);

            //End of Rev
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

        public int DeleteAdj(string AdjId)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_StockAdjustment_details");
            proc.AddVarcharPara("@Action", 50, "Delete");
            proc.AddVarcharPara("@AdjId", 10, AdjId);
            return proc.RunActionQuery();
        }

        public DataSet GetEditedData(string AdjId)
        {
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            MasterSettings masterBl = new MasterSettings();
            DataTable WareHouse = new DataTable();
            string multiwarehouse = Convert.ToString(masterBl.GetSettings("IaMultilevelWarehouse"));

            ProcedureExecute proc = new ProcedureExecute("Prc_StockAdjustment_details");
            proc.AddVarcharPara("@Action", 50, "GetEditedData");
            proc.AddVarcharPara("@AdjId", 10, AdjId);
            proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            proc.AddVarcharPara("@multiwarehouse", 10, multiwarehouse);
            return proc.GetDataSet();
        }
    }
}
