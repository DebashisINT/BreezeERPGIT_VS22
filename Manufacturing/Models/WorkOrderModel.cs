using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Manufacturing.Models
{
    public class WorkOrderModel
    {
        string ConnectionString = String.Empty;
        public WorkOrderModel()
        {
            ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }

        public DataTable GetWorkOrderData(string Action = null, Int64 WorkOrderID = 0, Int64 DetailsID = 0)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("usp_WorkOrderDataGet");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddPara("@WorkOrderID", WorkOrderID);
            proc.AddPara("@DetailsID", DetailsID);
            ds = proc.GetTable();
            return ds;
        }
        public DataTable GetPODataByDetailsID(string Action = null, Int64 WorkOrderID = 0, Int64 DetailsID = 0, string SelectedDetailsIDtList="")
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("usp_WorkOrderDataGet");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddPara("@WorkOrderID", WorkOrderID);
            proc.AddPara("@DetailsID", DetailsID);
            proc.AddPara("@SelectedDetailsIDtList", SelectedDetailsIDtList);
            
            ds = proc.GetTable();
            return ds;
        }
        public DataSet WorkOrderBOMProductInsertUpdate(String action, Int64 WorkOrderID, Int64 Production_ID, Int64 WorkCenterID, String OrderNo, Int64 Order_SchemaID, DateTime OrderDate,
        Decimal Order_Qty, Decimal ActualAdditionalCost, Decimal TotalCost,Int64 BRANCH_ID, Int64 userid, String Remarks,string PartNo, DataTable dtBOM_PRODUCTS)
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("usp_WorkOrderInsertUpdate");
            proc.AddVarcharPara("@ACTION", 150, action);
            proc.AddPara("@WorkOrderID", WorkOrderID);
            proc.AddPara("@Production_ID", Production_ID);
            proc.AddPara("@OrderNo", OrderNo);
            proc.AddPara("@Order_SchemaID", Order_SchemaID);
            proc.AddPara("@WorkCenterID", WorkCenterID);
            proc.AddPara("@OrderDate", OrderDate);
            proc.AddPara("@Order_Qty", Order_Qty);
            proc.AddPara("@ActualAdditionalCost", ActualAdditionalCost);
            proc.AddPara("@TotalCost", TotalCost);
            proc.AddPara("@Remarks", Remarks);
            proc.AddPara("@BRANCH_ID", BRANCH_ID);
            proc.AddPara("@UserID", userid);
            proc.AddPara("@PartNo", PartNo);
            proc.AddPara("@UDTPRODUCTIONORDER_DETAILS", dtBOM_PRODUCTS);
            ds = proc.GetDataSet();
            return ds;
        }
    }
}