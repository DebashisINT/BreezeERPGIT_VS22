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
    public class TaxExceptionrulesBL : IDisposable
    {
        public void Dispose()
        {
           
        }
        public string SaveException(string ActionType,string id, string EntityType, string BasedOn, string operators, string criteria,
            DateTime fromdate, DateTime todate, string HSNSACcode, string type, object INPUT_CGST_TAXRATESID, object INPUT_SGST_TAXRATESID,
            object INPUT_UTGST_TAXRATESID, object INPUT_IGST_TAXRATESID, object OUTPUT_CGST_TAXRATESID, object OUTPUT_SGST_TAXRATESID,
           object OUTPUT_UTGST_TAXRATESID, object OUTPUT_IGST_TAXRATESID)
        {
            string output = "";

            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("prc_AddEditTaxException", con);
            DataTable dsInst = new DataTable();


            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", ActionType);
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@EntityType", EntityType);
            cmd.Parameters.AddWithValue("@BasedOn", BasedOn);
            cmd.Parameters.AddWithValue("@operators", operators);
            cmd.Parameters.AddWithValue("@criteria", criteria);
            cmd.Parameters.AddWithValue("@fromdate", fromdate);
            cmd.Parameters.AddWithValue("@todate", todate);
            cmd.Parameters.AddWithValue("@HSNSACcode", HSNSACcode);
            cmd.Parameters.AddWithValue("@type", type);

            cmd.Parameters.AddWithValue("@INPUT_CGST_TAXRATESID", INPUT_CGST_TAXRATESID);
            cmd.Parameters.AddWithValue("@INPUT_SGST_TAXRATESID", INPUT_SGST_TAXRATESID);
            cmd.Parameters.AddWithValue("@INPUT_UTGST_TAXRATESID", INPUT_UTGST_TAXRATESID);
            cmd.Parameters.AddWithValue("@INPUT_IGST_TAXRATESID", INPUT_IGST_TAXRATESID);

            cmd.Parameters.AddWithValue("@OUTPUT_CGST_TAXRATESID", OUTPUT_CGST_TAXRATESID);
            cmd.Parameters.AddWithValue("@OUTPUT_SGST_TAXRATESID", OUTPUT_SGST_TAXRATESID);
            cmd.Parameters.AddWithValue("@OUTPUT_UTGST_TAXRATESID", OUTPUT_UTGST_TAXRATESID);
            cmd.Parameters.AddWithValue("@OUTPUT_IGST_TAXRATESID", OUTPUT_IGST_TAXRATESID);
            cmd.Parameters.AddWithValue("@userid", Convert.ToString(HttpContext.Current.Session["userid"]));


            SqlParameter outputText = new SqlParameter("@ReturnText", SqlDbType.VarChar, 200);
            outputText.Direction = ParameterDirection.Output;


            cmd.Parameters.Add(outputText);

            cmd.CommandTimeout = 0;
            SqlDataAdapter Adap = new SqlDataAdapter();
            Adap.SelectCommand = cmd;
            Adap.Fill(dsInst);
            cmd.Dispose();
            con.Dispose();
            output = Convert.ToString(cmd.Parameters["@ReturnText"].Value.ToString());

            return output;

        }

        public DataTable GetDataforHSNSAC(string HSNSACcode, string type)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_AddEditTaxException");
            proc.AddVarcharPara("@Action", 100, "GetEditedData");
            proc.AddVarcharPara("@HSNSACcode", 100, Convert.ToString(HSNSACcode));
            proc.AddVarcharPara("@type", 50, Convert.ToString(type));
            dt = proc.GetTable();
            return dt;
        }

        public DataTable GetEditData(string id)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_AddEditTaxException");
            proc.AddVarcharPara("@Action", 100, "EDIT");
            proc.AddVarcharPara("@ID", 100, Convert.ToString(id));
            dt = proc.GetTable();
            return dt;
        }

        public DataTable DeleteData(string id)
        {
            DataTable dt = new DataTable();
            ProcedureExecute proc = new ProcedureExecute("prc_AddEditTaxException");
            proc.AddVarcharPara("@Action", 100, "Delete");
            proc.AddVarcharPara("@ID", 100, Convert.ToString(id));
            dt = proc.GetTable();
            return dt;
        }

    }
}
