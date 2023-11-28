//================================================== Revision History =============================================
//1.0   Priti V2.0.41      28-11-2023    	0026963: The Price & Amount should be calculated for the Finished Stock in Quality Control.
//====================================================== Revision History =============================================
using DataAccessLayer;
using Manufacturing.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Manufacturing.Models
{
    public class QualityControlModel
    {

        string ConnectionString = String.Empty;
        public QualityControlModel()
        {
            ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }

        public DataTable GetQCData(string Action = null, Int64 ID = 0, Int64 DetailsID = 0)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("usp_QualityControlDataGet");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddPara("@ID", ID);
            proc.AddPara("@DetailsID", DetailsID);
            ds = proc.GetTable();
            return ds;
        }

        public DataTable QCInsertUpdate(string Action = null, QualityControlViewModel obj = null)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("usp_QualityControlInsertUpdate");
            proc.AddVarcharPara("@ACTION", 100, Action);
            proc.AddPara("@QualityControlID", obj.QualityControlID);
            proc.AddPara("@ProductionReceiptID", obj.ProductionReceiptID);
            proc.AddPara("@ProductionIssueID", obj.ProductionIssueID);
            proc.AddPara("@QC_SchemaID", obj.Order_SchemaID);
            proc.AddPara("@QC_No", obj.QC_No);
            proc.AddPara("@QC_Date", obj.OrderDate);
            proc.AddPara("@FG_Qty", obj.FGQty);
            proc.AddPara("@Fresh_Qty", obj.FreshQuantity);
            proc.AddPara("@Rejected_Qty", obj.RejectedQuantity);
            proc.AddPara("@UserID", obj.UserID);
            proc.AddVarcharPara("@DocType", 100, obj.DocType);
            //Rev 1.0
            proc.AddPara("@FGPrice", obj.FGPrice);
            proc.AddPara("@TotalAmount", obj.TotalAmount);
            //Rev 1.0 End
            ds = proc.GetTable();
            return ds;
        }
    }
}