using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ImportModuleBusinessLayer.GoodReceivedNote
{
    public class GoodReceivedNoteBL
    {
        public DataSet GetAllDropDownDetailForGRN()
        {
            DataSet ds = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_GoodReceiveNoteDetailsList_Import");
            proc.AddVarcharPara("@Action", 100, "GetLoadDetails");
            proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));            
            ds = proc.GetDataSet();
            return ds;
        }
        public DataTable GetBillEntry(string OrderDate, string Status, string branch, string vendor)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GetImportGoodReceivedNotetagged");
            proc.AddDateTimePara("@OrderDate", Convert.ToDateTime(OrderDate));
            proc.AddVarcharPara("@Status", 50, Status);           
            proc.AddVarcharPara("@branch", 500, branch);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@campany_Id", 500, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@vendorID", 500, vendor);

            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetBillOfEntryDetailsForTagg(string Indent_Id, string Order_Key, string Product_Ids, string Action)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_GoodReceiveNoteDetailsList_Import");
            proc.AddVarcharPara("@Action", 100, "GetBillEntryProductDetailsOnly");
            proc.AddVarcharPara("@Indent_Id", 4000, Indent_Id);
            proc.AddIntegerPara("@PurchaseOrder_Id", Convert.ToInt32(Order_Key));
            proc.AddVarcharPara("@Mode", 10, Action);
            return proc.GetTable();
        }

        public DataSet GetBillEntryDetailsForGRNGridBind(string Indent_Id, string Order_Key, string Product_Ids, string Action)
        {
            ProcedureExecute proc = new ProcedureExecute("prc_GoodReceiveNoteDetailsList_Import");
            proc.AddVarcharPara("@Action", 100, "GetBillEntryDetailsForGridBind");
            proc.AddVarcharPara("@Indent_Id", 4000, Indent_Id);
            proc.AddVarcharPara("@IndentDetails_Id", 1000, Order_Key);
            proc.AddVarcharPara("@Product_Id", 1000, Product_Ids);
            proc.AddVarcharPara("@Mode", 10, Action);
            proc.AddVarcharPara("@FinYear", 500, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@campany_Id", 500, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            return proc.GetDataSet();
        }

        public DataSet GetStockReceiptEditData(string StockReceiptID)
        {
            DataSet dt = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("prc_GoodReceiveNoteDetailsList_Import");
            proc.AddVarcharPara("@Action", 500, "StockReceiptEditDetails");
            proc.AddIntegerPara("@StockReceiptID", Convert.ToInt32(StockReceiptID));           
            dt = proc.GetDataSet();
            return dt;
        }

        public DataTable PopulateContactPersonOfCustomer(string InternalId)
        {
            DataTable ds = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_GoodReceiveNoteDetailsList_Import");
            proc.AddVarcharPara("@Action", 100, "PopulateContactPersonOfCustomer");
            proc.AddVarcharPara("@InternalId", 100, InternalId);
            ds = proc.GetTable();
            return ds;
        }

        public int DeleteStockReceipt(string StkReceiptID)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("prc_GoodReceiveNoteDetailsList_Import");
            proc.AddVarcharPara("@Action", 100, "DeleteStockReceipt");
            proc.AddIntegerPara("@DeletePOId", Convert.ToInt32(StkReceiptID));
            proc.AddVarcharPara("@ReturnValueDelete", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValueDelete"));
            return rtrnvalue;

        }

       
    }
}
