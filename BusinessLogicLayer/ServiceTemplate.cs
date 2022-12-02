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
    public class ServiceTemplate
    {
        public DataSet PopulateServiceTemplateDetails()
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_ServiceTemplate_details");
            proc.AddVarcharPara("@Action", 50, "GetLoadDetails");
            proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
            return proc.GetDataSet();
        }

        public DataSet GetEditedData(string @ServiceTemplate_ID)
        {
            ProcedureExecute proc = new ProcedureExecute("Prc_ServiceTemplate_details");
            proc.AddVarcharPara("@Action", 50, "GetEditedData");
            proc.AddVarcharPara("@ServiceTemplate_ID", 10, @ServiceTemplate_ID);
            proc.AddVarcharPara("@companyId", 20, Convert.ToString(HttpContext.Current.Session["LastCompany"]));
            proc.AddVarcharPara("@FinYear", 10, Convert.ToString(HttpContext.Current.Session["LastFinYear"]));
            proc.AddVarcharPara("@BranchList", -1, Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
           
            return proc.GetDataSet();
        }


        public void AddEditServiceTemplate(string Mode, string ServiceDescription, string ServiceProductId, string Quantity, string Branch,string Remarks
          , string userId, ref int ServiceId,
         DataTable AdjustmentTable, ref int ErrorCode, string Adj_id)
        {
            DataTable dsInst = new DataTable();            

            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("prc_ServiceTemplate_AddEdit", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", Mode);
            cmd.Parameters.AddWithValue("@ServiceDescription", ServiceDescription);
            cmd.Parameters.AddWithValue("@ServiceProductId", ServiceProductId);
            cmd.Parameters.AddWithValue("@Quantity", Quantity);
            cmd.Parameters.AddWithValue("@Branch", Branch);
            cmd.Parameters.AddWithValue("@Remarks", Remarks);
                     
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@DetailTable", AdjustmentTable);           
            cmd.Parameters.AddWithValue("@Adj_id", Adj_id);
            cmd.Parameters.AddWithValue("@FinYear", HttpContext.Current.Session["LastFinYear"].ToString());
            cmd.Parameters.AddWithValue("@CompanyID", HttpContext.Current.Session["LastCompany"].ToString());

            
            
            cmd.Parameters.Add("@ReturnId", SqlDbType.VarChar, 10);
            cmd.Parameters.Add("@ErrorCode", SqlDbType.Int);

            
            cmd.Parameters["@ReturnId"].Direction = ParameterDirection.Output;
            cmd.Parameters["@ErrorCode"].Direction = ParameterDirection.Output;

            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dsInst);

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

    }

}
