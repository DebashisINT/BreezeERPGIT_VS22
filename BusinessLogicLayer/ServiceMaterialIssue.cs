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
    public class ServiceMaterialIssue
    {
        public DataSet PopulateServiceTemplateDetails()
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_ServiceMaterialIssue_details");
            proc.AddVarcharPara("@Action", 50, "GetLoadDetails");
            proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            return proc.GetDataSet();
        }

        public DataSet GetEditedData(string @ServiceTemplate_ID)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_ServiceMaterialIssue_details");
            proc.AddVarcharPara("@Action", 50, "GetEditedData");
            proc.AddVarcharPara("@MaterialIssueID", 10, @ServiceTemplate_ID);
            proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));

            return proc.GetDataSet();
        }

        
               

        public void AddEditServiceMaterialIssue(string Mode,string SchemeID, string VoucherNo, string strMaterialDate, string Branch, string Reference, string CustomerId
          , string ContactPersonId,string strComponenyType,string Component,string ComponentDate,string TechnicianId,string Segment1,string Segment2,string Segment3,
            string Segment4, string Segment5, string userId, ref int ServiceId, ref string MaterialNumber,
         DataTable AdjustmentTable, DataTable Warehousedt, ref int ErrorCode, string Material_id)
        {
            DataTable dsInst = new DataTable();

            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("prc_ServiceMaterialIssue_AddEdit", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", Mode);
            cmd.Parameters.AddWithValue("@SchemeID", SchemeID);
            cmd.Parameters.AddWithValue("@VoucherNo", VoucherNo);
            cmd.Parameters.AddWithValue("@MaterialDate", strMaterialDate);
            cmd.Parameters.AddWithValue("@Branch", Branch);
            cmd.Parameters.AddWithValue("@Reference", Reference);
            cmd.Parameters.AddWithValue("@CustomerId", CustomerId);

            cmd.Parameters.AddWithValue("@ContactPersonId", ContactPersonId);
            cmd.Parameters.AddWithValue("@ComponenyType", strComponenyType);
            cmd.Parameters.AddWithValue("@Component", Component);
            cmd.Parameters.AddWithValue("@ComponentDate", ComponentDate);
            cmd.Parameters.AddWithValue("@TechnicianId", TechnicianId);

            cmd.Parameters.AddWithValue("@Segment1", Segment1);
            cmd.Parameters.AddWithValue("@Segment2", Segment2);
            cmd.Parameters.AddWithValue("@Segment3", Segment3);
            cmd.Parameters.AddWithValue("@Segment4", Segment4);
            cmd.Parameters.AddWithValue("@Segment5", Segment5);

            cmd.Parameters.AddWithValue("@WarehouseDetail", Warehousedt);
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@DetailTable", AdjustmentTable);
            cmd.Parameters.AddWithValue("@Material_id", Material_id);
            cmd.Parameters.AddWithValue("@FinYear", HttpContext.Current.Session["LastFinYear"].ToString());
            cmd.Parameters.AddWithValue("@CompanyID", HttpContext.Current.Session["LastCompany"].ToString());


            cmd.Parameters.Add("@ReturnValue", SqlDbType.VarChar, 100);
            cmd.Parameters.Add("@ReturnId", SqlDbType.VarChar, 10);
            cmd.Parameters.Add("@ErrorCode", SqlDbType.Int);


            cmd.Parameters["@ReturnValue"].Direction = ParameterDirection.Output;
            cmd.Parameters["@ReturnId"].Direction = ParameterDirection.Output;
            cmd.Parameters["@ErrorCode"].Direction = ParameterDirection.Output;

            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dsInst);

            MaterialNumber = Convert.ToString(cmd.Parameters["@ReturnValue"].Value);
            ServiceId = Convert.ToInt32(cmd.Parameters["@ReturnId"].Value);
            ErrorCode = Convert.ToInt32(cmd.Parameters["@ErrorCode"].Value);
        }

        public int DeleteServiceTemplate(string ServiceTemplateId)
        {
            int i;
            int rtrnvalue = 0;
            ProcedureExecute proc = new ProcedureExecute("Prc_ServiceTemplate_details");
            proc.AddVarcharPara("@Action", 50, "Delete");
            proc.AddVarcharPara("@ServiceTemplate_ID", 10, ServiceTemplateId);
            proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@ReturnValue", 50, "0", QueryParameterDirection.Output);
            i = proc.RunActionQuery();
            rtrnvalue = Convert.ToInt32(proc.GetParaValue("@ReturnValue"));

            return rtrnvalue;
        }

        public DataTable GetComponent(string Customer, string Date, string ComponentType, string FinYear, string BranchID, string Action, string strInvoiceID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_ServiceMaterialIssue_details");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@CustomerID", 500, Customer);
            proc.AddDateTimePara("@Date", Convert.ToDateTime(Date));
            proc.AddVarcharPara("@ComponentType", 10, ComponentType);
            proc.AddVarcharPara("@FinYear", 10, FinYear);
            proc.AddVarcharPara("@BranchId", 3000, BranchID);
            proc.AddVarcharPara("@MaterialIssueID", 20, strInvoiceID);

            dt = proc.GetTable();
            return dt;
        }
        public DataTable GetComponentProductList(string Action, string ComponentList, string InvoiceID)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("Prc_ServiceMaterialIssue_details");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@ComponentList", 2000, ComponentList);
            proc.AddVarcharPara("@MaterialIssueID", 20, InvoiceID);
            dt = proc.GetTable();
            return dt;
        }
        public DataSet GetSelectedSalesInvoiceComponentProductList(string Action, string SelectedComponentList, string InvoiceID)
        {
            DataSet dt = new DataSet();
            ProcedureExecute proc = new ProcedureExecute("Prc_ServiceMaterialIssue_details");
            proc.AddVarcharPara("@Action", 500, Action);
            proc.AddVarcharPara("@SelectedComponentList", 2000, SelectedComponentList);
            proc.AddVarcharPara("@InvoiceID", 20, InvoiceID);
            dt = proc.GetDataSet();
            return dt;
        }

    }

}
