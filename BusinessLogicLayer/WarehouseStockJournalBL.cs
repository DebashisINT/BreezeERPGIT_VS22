
/**********************************************************************************************************************************
 1.0    v2.0.22     Sanchita	21/04/2021		New check box "Dont Consider in Profitability Report" introduced module Warehouse wise Stock Journal .
												checkbox "NotInProfReport" added.
												Refer:  23986
  
 ***********************************************************************************************************************************/
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
    public class WarehouseStockJournalBL
    {

        public DataSet PopulateStockAdjustmentDetails()
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_WarehousewiseStockJournal_details");
            proc.AddVarcharPara("@Action", 50, "GetLoadDetails");
            proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            return proc.GetDataSet();
        }
         public DataSet PopulateStockAdjustmentDetailsIN()
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_WarehousewiseStockJournal_details");
            proc.AddVarcharPara("@Action", 50, "GetLoadDetailsWHIN");
            proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            return proc.GetDataSet();
        }
         public DataSet PopulateStockAdjustmentDetailsOUT()
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_WarehousewiseStockJournal_details");
            proc.AddVarcharPara("@Action", 50, "GetLoadDetailsWHOUT");
            proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            return proc.GetDataSet();
        }
        public DataTable PopulateWHINADDNEW(string BranchId)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_WarehousewiseStockJournal_details");
            proc.AddVarcharPara("@Action", 50, "GetWHAddNew");
            proc.AddIntegerPara("@BranchId", Convert.ToInt32(BranchId));
            return proc.GetTable();
        }
        public DataTable PopulateStock(string ProductId, string WarehouseID, string BranchId)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_WarehousewiseStockJournal_details");
            proc.AddVarcharPara("@Action", 50, "GetStock");
            proc.AddIntegerPara("@ProductID", Convert.ToInt32(ProductId));
            proc.AddIntegerPara("@WarehouseId", Convert.ToInt32(WarehouseID));
            proc.AddIntegerPara("@BranchId", Convert.ToInt32(BranchId));
            return proc.GetTable();
        }


        public DataTable PopulateTechnician(string BranchId)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_WarehousewiseStockJournal_details");
            proc.AddVarcharPara("@Action", 50, "GetTechnician");
            proc.AddIntegerPara("@BranchId", Convert.ToInt32(BranchId));
            return proc.GetTable();
        }

        public DataTable PopulateEntity(string BranchId)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_WarehousewiseStockJournal_details");
            proc.AddVarcharPara("@Action", 50, "GetEntity");
            proc.AddIntegerPara("@BranchId", Convert.ToInt32(BranchId));
            return proc.GetTable();
        }


        public DataTable GetNegativeStock(string ProductId)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_WarehousewiseStockJournal_details");
            proc.AddVarcharPara("@Action", 50, "GetNegativeStock");
            proc.AddIntegerPara("@ProductID", Convert.ToInt32(ProductId));
            return proc.GetTable();
        }

        // Rev 1.0 parameter " Boolean NotInProfReport=false" added
        public void AddEditWarehouseStockJournal(string Mode, string SchemeId, string Adjustment_No, string Adjustment_Date, string Branch, string BranchTo, string userId, string Technician
         , ref int AdjustedId, ref string ReturnNumber,
         DataTable AdjustmentTable
            //Rev work start 18.05.2022
            , DataTable MultiUOMDetails
            , DataTable SourceMultiUOMDetails
            //Rev work close 18.05.2022
            , DataTable Warehousedt,DataTable tempSourceWarehousedt, ref int ErrorCode, string Adj_id, DataTable PackingDetailsdt,DataTable PackingDTWSH, string TransportationMode, string VehicleNo,
            string Remarks, DataTable TempAdjustmentTableSWH, Int64 Proj_id = 0, string Type = "", Boolean NotInProfReport = false)
        {
            DataTable dsInst = new DataTable();

            //Rev Subhra 07-08-2019   (Because this two fields are not present in udt)
            AdjustmentTable.Columns.Remove("DestPackingQty");
            TempAdjustmentTableSWH.Columns.Remove("PackingQty");
            //End of Rev
            //Rev work start 18.05.2022
            if (MultiUOMDetails.Columns.Contains("MultiUOMSR"))
            {
                MultiUOMDetails.Columns.Remove("MultiUOMSR");
            }
            if (SourceMultiUOMDetails.Columns.Contains("MultiUOMSR"))
            {
                SourceMultiUOMDetails.Columns.Remove("MultiUOMSR");
            }
            //Rev work close 18.05.2022
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            //Rev work start 18.05.2022
            //SqlCommand cmd = new SqlCommand("prc_WarehouseStockJournal_AddEdit", con);
            SqlCommand cmd = new SqlCommand("prc_WarehouseStockJournal_AddEditNew", con);
            //Rev work close 18.05.2022
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
            cmd.Parameters.AddWithValue("@DetailTable", TempAdjustmentTableSWH);
            cmd.Parameters.AddWithValue("@DetailTableDWH",AdjustmentTable);
            /*Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal*/
            cmd.Parameters.AddWithValue("@MultiUOMDetails", MultiUOMDetails);
            cmd.Parameters.AddWithValue("@SourceMultiUOMDetails", SourceMultiUOMDetails);
            /*Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal*/
            cmd.Parameters.AddWithValue("@WarehouseDetail", Warehousedt);
            cmd.Parameters.AddWithValue("@WarehouseDetailSource", tempSourceWarehousedt);
            cmd.Parameters.AddWithValue("@Adj_id", Adj_id);
            cmd.Parameters.AddWithValue("@FinYear", HttpContext.Current.Session["LastFinYear"].ToString());
            cmd.Parameters.AddWithValue("@CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
            cmd.Parameters.AddWithValue("@PackingDetails", PackingDetailsdt);
            cmd.Parameters.AddWithValue("@PackingDetailsDWH", PackingDTWSH);
            cmd.Parameters.AddWithValue("@TransportationMode", TransportationMode);
            cmd.Parameters.AddWithValue("@VehicleNo", VehicleNo);
            cmd.Parameters.AddWithValue("@Remarks", Remarks);
            cmd.Parameters.AddWithValue("@Project_Id", Proj_id);
            cmd.Parameters.AddWithValue("@Technician_ID", Technician);
            cmd.Parameters.AddWithValue("@Type", Type);
            cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, -1);
            cmd.Parameters.Add("@ReturnId", SqlDbType.VarChar, 10);
            cmd.Parameters.Add("@ErrorCode", SqlDbType.Int);
            // Rev 1.0
            cmd.Parameters.AddWithValue("@NotInProfReport", NotInProfReport);
            // End of Rev 1.0

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
            cmd.Dispose();
            con.Dispose();
        }        
        public void AddEditWarehouseStockJournalIN(string Mode, string SchemeId, string Adjustment_No, string Adjustment_Date, string Branch, string BranchTo, string userId, string Technician, string Entity, string Customer,
       string RefNo ,ref int AdjustedId, ref string ReturnNumber,
       DataTable AdjustmentTable, DataTable Warehousedt, DataTable tempSourceWarehousedt, ref int ErrorCode, string Adj_id, DataTable PackingDetailsdt, DataTable PackingDTWSH, string TransportationMode, string VehicleNo,
          string Remarks, DataTable TempAdjustmentTableSWH, DataTable MultiUOMDetails, String ddlType, Int64 Proj_id = 0, string Type = "", string Employee = "")
        {
            DataTable dsInst = new DataTable();

            //Rev Subhra 07-08-2019   (Because this two fields are not present in udt)
            AdjustmentTable.Columns.Remove("DestPackingQty");
            TempAdjustmentTableSWH.Columns.Remove("PackingQty");
            //End of Rev
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            //Mantis Issue 24428
            //SqlCommand cmd = new SqlCommand("prc_WarehouseStockIN_OUT_AddEdit", con);
            SqlCommand cmd = new SqlCommand("prc_WarehouseStockIN_OUT_AddEditNew", con);
            //End of Mantis Issue 24428

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
            cmd.Parameters.AddWithValue("@DetailTable", TempAdjustmentTableSWH);
            //Mantis Issue 24428
            cmd.Parameters.AddWithValue("@MultiUOMDetails", MultiUOMDetails);
            //End of Mantis Issue 24428
            cmd.Parameters.AddWithValue("@DetailTableDWH", AdjustmentTable);
            cmd.Parameters.AddWithValue("@WarehouseDetail", Warehousedt);
            cmd.Parameters.AddWithValue("@WarehouseDetailSource", tempSourceWarehousedt);
            cmd.Parameters.AddWithValue("@Adj_id", Adj_id);
            cmd.Parameters.AddWithValue("@FinYear", HttpContext.Current.Session["LastFinYear"].ToString());
            cmd.Parameters.AddWithValue("@CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
            cmd.Parameters.AddWithValue("@PackingDetails", PackingDetailsdt);
            cmd.Parameters.AddWithValue("@PackingDetailsDWH", PackingDTWSH);
            cmd.Parameters.AddWithValue("@TransportationMode", TransportationMode);
            cmd.Parameters.AddWithValue("@VehicleNo", VehicleNo);
            cmd.Parameters.AddWithValue("@Remarks", Remarks);
            cmd.Parameters.AddWithValue("@Project_Id", Proj_id);
            cmd.Parameters.AddWithValue("@Technician_ID", Technician);
            cmd.Parameters.AddWithValue("@Type", Type);
            cmd.Parameters.AddWithValue("@Entitycnt_internalId", Entity);
            cmd.Parameters.AddWithValue("@Customer_internalId", Customer);
            cmd.Parameters.AddWithValue("@RefNo", RefNo);

            cmd.Parameters.AddWithValue("@ReplaceableType", ddlType);
            cmd.Parameters.AddWithValue("@EmployeeId", Employee);
            cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, -1);
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
            cmd.Dispose();
            con.Dispose();
        }


       
        public void AddEditWarehouseStockJournal_IN(string Mode, string SchemeId, string Adjustment_No, string Adjustment_Date, string Branch, string BranchTo, string userId, string Technician, string Entity, string Customer,
        string RefNo, ref int AdjustedId, ref string ReturnNumber,
        DataTable AdjustmentTable, DataTable Warehousedt, DataTable tempSourceWarehousedt, ref int ErrorCode, string Adj_id, DataTable PackingDetailsdt, DataTable PackingDTWSH, string TransportationMode, string VehicleNo,
        string Remarks, DataTable TempAdjustmentTableSWH, DataTable MultiUOMDetails, String ddlType, Int64 Proj_id = 0, string Type = "", string Employee = "")
        {
            DataTable dsInst = new DataTable();

            //Rev Subhra 07-08-2019   (Because this two fields are not present in udt)
            AdjustmentTable.Columns.Remove("DestPackingQty");
            // TempAdjustmentTableSWH.Columns.Remove("PackingQty");
            //End of Rev
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            //Mantis Issue 24428
            //SqlCommand cmd = new SqlCommand("prc_WarehouseStockJournalIN_AddEdit", con);
            SqlCommand cmd = new SqlCommand("prc_WarehouseStockJournalIN_AddEditNew", con);
            //End of Mantis Issue 24428
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
            cmd.Parameters.AddWithValue("@DetailTable", TempAdjustmentTableSWH);
            cmd.Parameters.AddWithValue("@DetailTableDWH", AdjustmentTable);
            //Mantis Issue 24428
            cmd.Parameters.AddWithValue("@MultiUOMDetails", MultiUOMDetails);
            //End of Mantis Issue 24428
            cmd.Parameters.AddWithValue("@WarehouseDetail", Warehousedt);
            cmd.Parameters.AddWithValue("@WarehouseDetailSource", tempSourceWarehousedt);
            cmd.Parameters.AddWithValue("@Adj_id", Adj_id);
            cmd.Parameters.AddWithValue("@FinYear", HttpContext.Current.Session["LastFinYear"].ToString());
            cmd.Parameters.AddWithValue("@CompanyID", HttpContext.Current.Session["LastCompany"].ToString());
            cmd.Parameters.AddWithValue("@PackingDetails", PackingDetailsdt);
            cmd.Parameters.AddWithValue("@PackingDetailsDWH", PackingDTWSH);
            cmd.Parameters.AddWithValue("@TransportationMode", TransportationMode);
            cmd.Parameters.AddWithValue("@VehicleNo", VehicleNo);
            cmd.Parameters.AddWithValue("@Remarks", Remarks);
            cmd.Parameters.AddWithValue("@Project_Id", Proj_id);
            cmd.Parameters.AddWithValue("@Technician_ID", Technician);
            cmd.Parameters.AddWithValue("@Type", Type);
            cmd.Parameters.AddWithValue("@Entitycnt_internalId", Entity);
            cmd.Parameters.AddWithValue("@Customer_internalId", Customer);
            cmd.Parameters.AddWithValue("@RefNo", RefNo);

            cmd.Parameters.AddWithValue("@ReplaceableType", ddlType);
            cmd.Parameters.AddWithValue("@EmployeeId", Employee);
            cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, -1);
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

            cmd.Dispose();
            con.Dispose();
        }
        
        public DataSet GetEditedWHSTDataIN(string AdjId, string multiwarehouse)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_WarehousewiseStockJournal_details");
            proc.AddVarcharPara("@Action", 50, "GetEditedDataWHIN");
            proc.AddVarcharPara("@AdjId", 10, AdjId);
            proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            proc.AddVarcharPara("@Multiwarehouse", 500, Convert.ToString(multiwarehouse));
            return proc.GetDataSet();
        }

        public DataSet GetEditedWHSTDataOUT(string AdjId, string multiwarehouse)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_WarehousewiseStockJournal_details");
            proc.AddVarcharPara("@Action", 50, "GetEditedDataWHOUT");
            proc.AddVarcharPara("@AdjId", 10, AdjId);
            proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            proc.AddVarcharPara("@Multiwarehouse", 500, Convert.ToString(multiwarehouse));
            return proc.GetDataSet();
        }
        public DataSet GetEditedWHSTData(string AdjId, string multiwarehouse)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_WarehousewiseStockJournal_details");
            proc.AddVarcharPara("@Action", 50, "GetEditedData");
            proc.AddVarcharPara("@AdjId", 10, AdjId);
            proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            proc.AddVarcharPara("@Multiwarehouse", 500, Convert.ToString(multiwarehouse));
            return proc.GetDataSet();
        }
        //Rev work start 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal
        public DataSet GetEditedWHSTData_New(string AdjId, string multiwarehouse)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_WarehousewiseStockJournal_details");
            proc.AddVarcharPara("@Action", 50, "GetEditedData_New");
            proc.AddVarcharPara("@AdjId", 10, AdjId);
            proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            proc.AddVarcharPara("@Multiwarehouse", 500, Convert.ToString(multiwarehouse));
            return proc.GetDataSet();
        }
        //Rev work close 17.05.2022 Mantise no:0024901: Add multiuom option in Warehouse Wise Stock Journal
        public int DeleteAdj(string AdjId, string WDelete)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("Prc_WarehousewiseStockJournal_details");
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
                proc.AddVarcharPara("@Module", 100, "Warehouse_Wise_Stock_OUT");
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
