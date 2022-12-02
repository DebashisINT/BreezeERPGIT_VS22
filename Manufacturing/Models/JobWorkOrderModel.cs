using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Manufacturing.Models
{
    public class JobWorkOrderModel
    {

        string ConnectionString = String.Empty;
        public JobWorkOrderModel()
        {
            ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }

        public DataTable GetWorkOrderData(string Action = null, string WorkOrderID = "0")
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_JobWORKORDERDATAGET");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddPara("@WorkOrderID", Convert.ToInt64(WorkOrderID));
            //proc.AddPara("@DetailsID", DetailsID);
            ds = proc.GetTable();
            return ds;
        }

        public DataTable GetWorkOrderdeleteData(string Action = null, string WorkOrderID = "0",string UserId="0")
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_JobWORKORDERDATAGET");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddPara("@WorkOrderID", Convert.ToInt64(WorkOrderID));
            //proc.AddPara("@DetailsID", DetailsID);
            proc.AddPara("@UserID", Convert.ToString(UserId));
            ds = proc.GetTable();
            return ds;
        }


        public DataTable ClosedWorkOrderData(string Action, Int64 WorkOrderID,string ClosedRemarks)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Proc_JobWORKORDERDATAGET");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddPara("@WorkOrderID", Convert.ToInt64(WorkOrderID));
            proc.AddVarcharPara("@ClosedRemarks",500, ClosedRemarks);
            ds = proc.GetTable();
            return ds;
        }

        public DataSet WorkOrderBOMProductInsertUpdate(string action, string WorkOrderID, string Production_ID, string WorkCenterID, string OrderNo, string Order_SchemaID, DateTime OrderDate,
        string Order_Qty, string ActualAdditionalCost, string TotalCost, string BRANCH_ID, string userid, string Remarks, string PartNo, DataTable dtBOM_PRODUCTS, string Desc, string DrawingNo, string DrawingRevNo, string ProjectId,
            string JobworkRate, DataTable FinishItem, string Finyear, string Order_id, string Schema_id)
        {
            DataSet ds = new DataSet();
            if(ProjectId=="" || ProjectId==null)
            {
                ProjectId="0";
            }
            ProcedureExecute proc = new ProcedureExecute("Proc_JobWorkOrderInsertUpdate");
            proc.AddVarcharPara("@ACTION", 150, action);
            proc.AddPara("@WorkOrderID", Convert.ToString(WorkOrderID));
            proc.AddPara("@Production_ID", Convert.ToString(Production_ID));
            proc.AddPara("@OrderNo", OrderNo);
            proc.AddPara("@Order_SchemaID", Convert.ToString(Order_SchemaID));
            proc.AddPara("@WorkCenterID", Convert.ToString(WorkCenterID));
            proc.AddPara("@OrderDate", OrderDate);
            proc.AddPara("@Order_Qty", Convert.ToString(Order_Qty));
            proc.AddPara("@ActualAdditionalCost", Convert.ToString(ActualAdditionalCost));
            proc.AddPara("@TotalCost", Convert.ToString(TotalCost));
            proc.AddPara("@Remarks", Remarks);
            proc.AddPara("@BRANCH_ID", Convert.ToString(BRANCH_ID));
            proc.AddPara("@UserID", Convert.ToString(userid));
            proc.AddPara("@PartNo", PartNo);
            proc.AddVarcharPara("@Desc", 300, Desc);
            proc.AddVarcharPara("@DrawingNo", 300, DrawingNo);
            proc.AddVarcharPara("@DrawingRevNo", 300, DrawingRevNo);
            proc.AddPara("@Udt_FinishItemDetails", FinishItem);
            proc.AddPara("@UDTPRODUCTIONORDER_DETAILS", dtBOM_PRODUCTS);
            proc.AddPara("@ProjectId", Convert.ToInt64(ProjectId));
            proc.AddPara("@JobworkRate", Convert.ToString(JobworkRate));
            proc.AddPara("@FinYear", Finyear);
            proc.AddPara("@SCHEMEID", Schema_id);

            ds = proc.GetDataSet();
            return ds;
        }
    }
}